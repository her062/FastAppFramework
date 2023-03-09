using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using FastAppFramework.Core.Comparers;
using FastAppFramework.Wpf;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace FastAppFramework.Demo.ViewModels
{
    public class ThemePageViewModel : BindableBase
    {
#region Commands
        public ReactiveCommand<Action<ITheme>> SetThemeCommand
        {
            get; private set;
        }
#endregion

#region Properties
        public ReactivePropertySlim<BaseTheme> BaseTheme
        {
            get; private set;
        }
        public ReactivePropertySlim<KeyValuePair<ColorSet.Order, ColorSet>> SelectedColorSet
        {
            get; private set;
        }
        public ReactivePropertySlim<Color> SelectedColor
        {
            get; private set;
        }
        public ReactivePropertySlim<ColorPalette?> SelectedPalette
        {
            get; private set;
        }
        public ReactivePropertySlim<ColorSwatch?> SelectedSwatch
        {
            get; private set;
        }

        public ReactivePropertySlim<int> TransitionIndex => new ReactivePropertySlim<int>().AddTo(this);

        public IEnumerable<BaseTheme> BaseThemes => Enum.GetValues<BaseTheme>().Where(v => (v != MaterialDesignThemes.Wpf.BaseTheme.Inherit));
        public IEnumerable<KeyValuePair<ColorSet.Order, ColorSet>> ColorSets => this._colorSets;
        public IEnumerable<ColorPalette> ColorPalettes => this._swatchesProvider.Swatches
            .Select(v => new ColorPalette(
                v.Name, new ColorPair(v.ExemplarHue.Color, v.ExemplarHue.Foreground), new ColorPair(v.AccentExemplarHue?.Color ?? v.ExemplarHue.Color, v.AccentExemplarHue?.Foreground ?? v.ExemplarHue.Foreground),
                v.PrimaryHues.OrderBy(v => v.Name, NaturalStringComparer.Instance).Select(s => new ColorSwatch(s.Name, new ColorPair(s.Color, s.Foreground))),
                v.AccentHues.OrderBy(v => v.Name, NaturalStringComparer.Instance).Select(s => new ColorSwatch(s.Name, new ColorPair(s.Color, s.Foreground)))
            ));
#endregion

#region Fields
        private PaletteHelper _paletteHelper;
        private ITheme _theme;
        private Dictionary<ColorSet.Order, ColorSet> _colorSets;
        private SwatchesProvider _swatchesProvider;
#endregion

#region Constructor/Destructor
        public ThemePageViewModel()
        {
            // Setup Fields.
            {
                this._paletteHelper = new PaletteHelper();
                this._theme = this._paletteHelper.GetTheme();
                this._colorSets = new Dictionary<ColorSet.Order, ColorSet>(){
                    { ColorSet.Order.Primary, ColorSet.FromTheme(this._theme, ColorSet.Order.Primary) },
                    { ColorSet.Order.Secondary, ColorSet.FromTheme(this._theme, ColorSet.Order.Secondary) },
                };
                this._swatchesProvider = new SwatchesProvider();
            }

            // Setup Properties.
            {
                this.BaseTheme = new ReactivePropertySlim<BaseTheme>(this._theme.GetBaseTheme()).AddTo(this);
                this.SelectedColorSet = new ReactivePropertySlim<KeyValuePair<ColorSet.Order, ColorSet>>(this._colorSets.First()).AddTo(this);
                this.SelectedColor = new ReactivePropertySlim<Color>(this.SelectedColorSet.Value.Value.Color).AddTo(this);
                this.SelectedPalette = new ReactivePropertySlim<ColorPalette?>().AddTo(this);
                this.SelectedSwatch = new ReactivePropertySlim<ColorSwatch?>().AddTo(this);
            }

            // Setup Commands.
            {
                this.SetThemeCommand = new ReactiveCommand<Action<ITheme>>()
                    .WithSubscribe(action => {
                        action(this._theme);
                        this._paletteHelper.SetTheme(this._theme);
                        foreach (var item in this._colorSets)
                            item.Value.Apply(this._theme, item.Key);

                        this.SelectedColor.Value = this.SelectedColorSet.Value.Value.Color;
                        switch (this.SelectedColorSet.Value.Key)
                        {
                            case ColorSet.Order.Primary:
                                this.SelectedPalette.Value = this.ColorPalettes.FirstOrDefault(v => (v.Swatches.FirstOrDefault(s => (this.SelectedColor.Value == s.Pair.Color)) != null));
                                this.SelectedSwatch.Value = this.ColorPalettes.FirstOrDefault(v => (this.SelectedColor.Value == v.Pair.Color)) ?? this.SelectedPalette.Value?.Swatches.FirstOrDefault(v => (this.SelectedColor.Value == v.Pair.Color));
                                break;
                            case ColorSet.Order.Secondary:
                                this.SelectedPalette.Value = this.ColorPalettes.FirstOrDefault(v => (v.SecondarySwatches.FirstOrDefault(s => (this.SelectedColor.Value == s.Pair.Color)) != null));
                                this.SelectedSwatch.Value = this.ColorPalettes.FirstOrDefault(v => (this.SelectedColor.Value == v.SecondaryPair.Color)) ?? this.SelectedPalette.Value?.SecondarySwatches.FirstOrDefault(v => (this.SelectedColor.Value == v.Pair.Color));
                                break;
                            default:
                                break;
                        }
                    }).AddTo(this);
            }

            // Subscribes.
            {
                this.BaseTheme.Subscribe(v => {
                    this.SetThemeCommand.Execute(t => t.SetBaseTheme(v.GetBaseTheme()));
                }).AddTo(this);
                this.SelectedColorSet.Subscribe(v => {
                    this.SetThemeCommand.Execute(t => {});
                }).AddTo(this);
                this.SelectedColor.Subscribe(v => {
                    this.SetThemeCommand.Execute(t => {
                        switch (this.SelectedColorSet.Value.Key)
                        {
                            case ColorSet.Order.Primary:
                                t.SetPrimaryColor(v);
                                break;
                            case ColorSet.Order.Secondary:
                                t.SetSecondaryColor(v);
                                break;
                            default:
                                break;
                        }
                    });
                }).AddTo(this);
                this.SelectedSwatch.Subscribe(v => {
                    if (v == null)
                        return;
                    this.SelectedColor.Value = ((v is ColorPalette p) && (this.SelectedColorSet.Value.Key == ColorSet.Order.Secondary)) ? p.SecondaryPair.Color : v.Pair.Color;
                }).AddTo(this);

                var themeManager = this._paletteHelper.GetThemeManager();
                if (themeManager != null)
                    themeManager.ThemeChanged += ThemeManager_ThemeChanged;
            }
        }

        private void ThemeManager_ThemeChanged(object? sender, ThemeChangedEventArgs e)
        {
            this.BaseTheme.Value = this._theme.GetBaseTheme();
            foreach (var item in this._colorSets)
                item.Value.Apply(this._theme, item.Key);
        }
#endregion
    }
}
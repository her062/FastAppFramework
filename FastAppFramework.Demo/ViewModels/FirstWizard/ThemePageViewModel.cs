using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
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
        public ReactivePropertySlim<Swatch?> SelectedSwatch
        {
            get; private set;
        }

        public IEnumerable<BaseTheme> BaseThemes => Enum.GetValues<BaseTheme>().Where(v => (v != MaterialDesignThemes.Wpf.BaseTheme.Inherit));
        public IEnumerable<KeyValuePair<ColorSet.Order, ColorSet>> ColorSets => this._colorSets;
        public IEnumerable<Swatch> Swatches => this._swatchesProvider.Swatches;
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
                this.SelectedSwatch = new ReactivePropertySlim<Swatch?>(this.Swatches.FirstOrDefault(v => (v.ExemplarHue.Color == this.SelectedColor.Value))).AddTo(this);
            }

            // Setup Commands.
            {
                this.SetThemeCommand = new ReactiveCommand<Action<ITheme>>()
                    .WithSubscribe(v => {
                        v(this._theme);
                        this._paletteHelper.SetTheme(this._theme);

                        foreach (var item in this._colorSets)
                            item.Value.Apply(this._theme, item.Key);
                        this.SelectedColor.Value = this.SelectedColorSet.Value.Value.Color;
                        this.SelectedSwatch.Value = this.Swatches.FirstOrDefault(s => (s.ExemplarHue.Color == this.SelectedColor.Value));
                    }).AddTo(this);
            }

            // Subscribes.
            {
                this.BaseTheme.Subscribe(v => {
                    this.SetThemeCommand.Execute(t => t.SetBaseTheme(v.GetBaseTheme()));
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

                    this.SelectedColor.Value = ((this.SelectedColorSet.Value.Key == ColorSet.Order.Secondary) && (v.AccentExemplarHue != null)) ? v.AccentExemplarHue.Color : v.ExemplarHue.Color;
                }).AddTo(this);
            }
        }
#endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using FastAppFramework.Demo.Models;
using FastAppFramework.Wpf;
using MaterialDesignThemes.Wpf;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace FastAppFramework.Demo.ViewModels
{
    public class ThemeSettingsPageViewModel : CloneableModelBindingBase<ThemeSettings>
    {
#region Properties
        public ReactiveProperty<bool> LoadFromFile
        {
            get; private set;
        }
        public ReactiveProperty<BaseTheme> BaseTheme
        {
            get; private set;
        }
        public ReactiveProperty<Color> PrimaryColor
        {
            get; private set;
        }
        public ReactiveProperty<Color> SecondaryColor
        {
            get; private set;
        }

        public ThemePageViewModel Super => this._super;
#endregion

#region Fields
        private ThemePageViewModel _super;
#endregion

#region Constructor/Destructor
        public ThemeSettingsPageViewModel(ThemeSettings model) : base(model, UpdateModelTrigger.Commit)
        {
            // Setup Fields.
            {
                this._super = new ThemePageViewModel();
            }

            // Setup Properties.
            {
                this.LoadFromFile = this.Model.ToReactivePropertyAsSynchronized(o => o.LoadFromFile).Observe(this).AddTo(this);
                this.BaseTheme = this.Model.ToReactivePropertyAsSynchronized(o => o.BaseTheme).Observe(this).AddTo(this);
                this.PrimaryColor = this.Model.ToReactivePropertyAsSynchronized(o => o.PrimaryColor).Observe(this).AddTo(this);
                this.SecondaryColor = this.Model.ToReactivePropertyAsSynchronized(o => o.SecondaryColor).Observe(this).AddTo(this);
            }

            // Subscribes.
            {
                this.BaseTheme.Subscribe(v => {
                    this._super.SetThemeCommand.Execute(t => t.SetBaseTheme(v.GetBaseTheme()));
                }).AddTo(this);
                this._super.BaseTheme.Subscribe(v => {
                    this.BaseTheme.Value = v;
                }).AddTo(this);

                this.PrimaryColor.Subscribe(v => {
                    this._super.SetThemeCommand.Execute(t => t.SetPrimaryColor(v));
                }).AddTo(this);
                this.SecondaryColor.Subscribe(v => {
                    this._super.SetThemeCommand.Execute(t => t.SetSecondaryColor(v));
                }).AddTo(this);
                this._super.SelectedColor.Subscribe(v => {
                    switch (this._super.SelectedColorSet.Value.Key)
                    {
                        case ColorSet.Order.Primary:
                            this.PrimaryColor.Value = v;
                            break;
                        case ColorSet.Order.Secondary:
                            this.SecondaryColor.Value = v;
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }).AddTo(this);
            }
        }
#endregion
    }
}
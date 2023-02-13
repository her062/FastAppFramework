using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace FastAppFramework.Wpf
{
    public static class MetroDialogServiceExtensions
    {
        public static T GetDefault<T>(this T self, MetroDialogService.DialogType type, CancellationToken? cancellationToken = null) where T : MetroDialogSettings
        {
            self.CustomResourceDictionary = new System.Windows.ResourceDictionary(){ Source = new Uri($"pack://application:,,,/FastAppFramework.Wpf;component/Themes/{type.ToString()}Dialog.xaml") };
            if (cancellationToken != null)
                self.CancellationToken = cancellationToken.Value;
            return self;
        }
    }
    public class MetroDialogService : IMetroDialogService
    {
#region Constants
        public enum DialogType
        {
            Input,
            Login,
            Message,
            Progress,
            Custom,
        }
#endregion

#region Fields
        private MetroWindow _window;
#endregion

#region Constructor/Destructor
        public MetroDialogService(MetroWindow window)
        {
            // Setup Fields.
            {
                this._window = window;
            }
        }
#endregion

#region Public Functions
        public Task<string?> ShowInputAsync(string title, string message, CancellationToken? cancellationToken = null)
        {
            var settings = (new MetroDialogSettings()).GetDefault(DialogType.Input, cancellationToken);
            return this._window.ShowInputAsync(title, message, settings);
        }
        public string? ShowModalInputExternal(string title, string message, CancellationToken? cancellationToken = null)
        {
            var settings = (new MetroDialogSettings()).GetDefault(DialogType.Input, cancellationToken);
            return this._window.ShowModalInputExternal(title, message, settings);
        }
//        public Task<LoginDialogData?> ShowLoginAsync(string title, string message, LoginDialogSettings? settings = null)
//        {
//            settings = (settings ?? new LoginDialogSettings(){ UsernameWatermark = "Username", PasswordWatermark = "Password" }).GetDefault(DialogType.Login);
//            return this._window.ShowLoginAsync(title, message, settings);
//        }
//        public LoginDialogData? ShowModalLoginExternal(string title, string message, LoginDialogSettings? settings = null)
//        {
//            settings = (settings ?? new LoginDialogSettings()).GetDefault(DialogType.Login);
//            return this._window.ShowModalLoginExternal(title, message, settings);
//        }
        public Task<MessageDialogResult> ShowMessageAsync(string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative, CancellationToken? cancellationToken = null)
        {
            var settings = (new MetroDialogSettings()).GetDefault(DialogType.Message, cancellationToken);
            return this._window.ShowMessageAsync(title, message, style, settings);
        }
        public MessageDialogResult ShowModalMessageExternal(string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative, CancellationToken? cancellationToken = null)
        {
            var settings = (new MetroDialogSettings()).GetDefault(DialogType.Message, cancellationToken);
            return this._window.ShowModalMessageExternal(title, message, style, settings);
        }
        public Task<ProgressDialogController> ShowProgressAsync(string title, string message, bool isCancelable = false, CancellationToken? cancellationToken = null)
        {
            var settings = (new MetroDialogSettings()).GetDefault(DialogType.Progress, cancellationToken);
            return this._window.ShowProgressAsync(title, message, isCancelable, settings);
        }
        public Task ShowMetroDialogAsync(BaseMetroDialog dialog, CancellationToken? cancellationToken = null)
        {
            var settings = (new MetroDialogSettings()).GetDefault(DialogType.Custom, cancellationToken);
            return this._window.ShowMetroDialogAsync(dialog, settings);
        }
        public Task HideMetroDialogAsync(BaseMetroDialog dialog, CancellationToken? cancellationToken = null)
        {
            var settings = (new MetroDialogSettings()).GetDefault(DialogType.Custom, cancellationToken);
            return this._window.HideMetroDialogAsync(dialog, settings);
        }
        public Task<TDialog?> GetCurrentDialogAsync<TDialog>() where TDialog : BaseMetroDialog
        {
            return this._window.GetCurrentDialogAsync<TDialog?>();
        }
#endregion
    }
}
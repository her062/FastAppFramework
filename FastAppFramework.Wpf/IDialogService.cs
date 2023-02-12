using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;

namespace FastAppFramework.Wpf
{
    public interface IDialogService
    {
        Task<string?> ShowInputAsync(string title, string message, CancellationToken? cancellationToken = null);
        string? ShowModalInputExternal(string title, string message, CancellationToken? cancellationToken = null);
        Task<LoginDialogData?> ShowLoginAsync(string title, string message, LoginDialogSettings? settings = null);
        LoginDialogData? ShowModalLoginExternal(string title, string message, LoginDialogSettings? settings = null);
        Task<MessageDialogResult> ShowMessageAsync(string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative, CancellationToken? cancellationToken = null);
        MessageDialogResult ShowModalMessageExternal(string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative, CancellationToken? cancellationToken = null);
        Task<ProgressDialogController> ShowProgressAsync(string title, string message, bool isCancelable = false, CancellationToken? cancellationToken = null);
        Task ShowMetroDialogAsync(BaseMetroDialog dialog, CancellationToken? cancellationToken = null);
        Task HideMetroDialogAsync(BaseMetroDialog dialog, CancellationToken? cancellationToken = null);
        Task<TDialog?> GetCurrentDialogAsync<TDialog>() where TDialog : BaseMetroDialog;
    }
}
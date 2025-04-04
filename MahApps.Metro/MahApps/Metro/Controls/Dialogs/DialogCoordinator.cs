// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.Dialogs.DialogCoordinator
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Threading.Tasks;
using System.Windows;

#nullable disable
namespace MahApps.Metro.Controls.Dialogs
{
  public class DialogCoordinator : IDialogCoordinator
  {
    public static readonly DialogCoordinator Instance = new DialogCoordinator();

    public Task<string> ShowInputAsync(
      object context,
      string title,
      string message,
      MetroDialogSettings metroDialogSettings = null)
    {
      return DialogCoordinator.GetMetroWindow(context).ShowInputAsync(title, message, metroDialogSettings);
    }

    public Task<LoginDialogData> ShowLoginAsync(
      object context,
      string title,
      string message,
      LoginDialogSettings settings = null)
    {
      return DialogCoordinator.GetMetroWindow(context).ShowLoginAsync(title, message, settings);
    }

    public Task<MessageDialogResult> ShowMessageAsync(
      object context,
      string title,
      string message,
      MessageDialogStyle style = MessageDialogStyle.Affirmative,
      MetroDialogSettings settings = null)
    {
      return DialogCoordinator.GetMetroWindow(context).ShowMessageAsync(title, message, style, settings);
    }

    public Task<ProgressDialogController> ShowProgressAsync(
      object context,
      string title,
      string message,
      bool isCancelable = false,
      MetroDialogSettings settings = null)
    {
      return DialogCoordinator.GetMetroWindow(context).ShowProgressAsync(title, message, isCancelable, settings);
    }

    public Task ShowMetroDialogAsync(
      object context,
      BaseMetroDialog dialog,
      MetroDialogSettings settings = null)
    {
      return DialogCoordinator.GetMetroWindow(context).ShowMetroDialogAsync(dialog, settings);
    }

    public Task HideMetroDialogAsync(
      object context,
      BaseMetroDialog dialog,
      MetroDialogSettings settings = null)
    {
      return DialogCoordinator.GetMetroWindow(context).HideMetroDialogAsync(dialog, settings);
    }

    public Task<TDialog> GetCurrentDialogAsync<TDialog>(object context) where TDialog : BaseMetroDialog
    {
      return DialogCoordinator.GetMetroWindow(context).GetCurrentDialogAsync<TDialog>();
    }

    private static MetroWindow GetMetroWindow(object context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      if (!DialogParticipation.IsRegistered(context))
        throw new InvalidOperationException("Context is not registered. Consider using DialogParticipation.Register in XAML to bind in the DataContext.");
      return Window.GetWindow(DialogParticipation.GetAssociation(context)) is MetroWindow window ? window : throw new InvalidOperationException("Control is not inside a MetroWindow.");
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.Dialogs.IDialogCoordinator
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Threading.Tasks;

#nullable disable
namespace MahApps.Metro.Controls.Dialogs
{
  public interface IDialogCoordinator
  {
    Task<string> ShowInputAsync(
      object context,
      string title,
      string message,
      MetroDialogSettings settings = null);

    Task<LoginDialogData> ShowLoginAsync(
      object context,
      string title,
      string message,
      LoginDialogSettings settings = null);

    Task<MessageDialogResult> ShowMessageAsync(
      object context,
      string title,
      string message,
      MessageDialogStyle style = MessageDialogStyle.Affirmative,
      MetroDialogSettings settings = null);

    Task<ProgressDialogController> ShowProgressAsync(
      object context,
      string title,
      string message,
      bool isCancelable = false,
      MetroDialogSettings settings = null);

    Task ShowMetroDialogAsync(object context, BaseMetroDialog dialog, MetroDialogSettings settings = null);

    Task HideMetroDialogAsync(object context, BaseMetroDialog dialog, MetroDialogSettings settings = null);

    Task<TDialog> GetCurrentDialogAsync<TDialog>(object context) where TDialog : BaseMetroDialog;
  }
}

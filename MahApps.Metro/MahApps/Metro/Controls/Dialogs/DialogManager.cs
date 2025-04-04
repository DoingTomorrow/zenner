// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.Dialogs.DialogManager
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace MahApps.Metro.Controls.Dialogs
{
  public static class DialogManager
  {
    public static Task<LoginDialogData> ShowLoginAsync(
      this MetroWindow window,
      string title,
      string message,
      LoginDialogSettings settings = null)
    {
      window.Dispatcher.VerifyAccess();
      return DialogManager.HandleOverlayOnShow((MetroDialogSettings) settings, window).ContinueWith<Task<LoginDialogData>>((Func<Task, Task<LoginDialogData>>) (z => window.Dispatcher.Invoke<Task<LoginDialogData>>((Func<Task<LoginDialogData>>) (() =>
      {
        if (settings == null)
          settings = new LoginDialogSettings();
        LoginDialog dialog = new LoginDialog(window, settings)
        {
          Title = title,
          Message = message
        };
        SizeChangedEventHandler sizeHandler = DialogManager.SetupAndOpenDialog(window, (BaseMetroDialog) dialog);
        dialog.SizeChangedHandler = sizeHandler;
        return dialog.WaitForLoadAsync().ContinueWith<Task<Task<LoginDialogData>>>((Func<Task, Task<Task<LoginDialogData>>>) (x =>
        {
          if (DialogManager.DialogOpened != null)
            window.Dispatcher.BeginInvoke((Delegate) (() => DialogManager.DialogOpened((object) window, new DialogStateChangedEventArgs())));
          return dialog.WaitForButtonPressAsync().ContinueWith<Task<Task<LoginDialogData>>>((Func<Task<LoginDialogData>, Task<Task<LoginDialogData>>>) (y =>
          {
            dialog.OnClose();
            if (DialogManager.DialogClosed != null)
              window.Dispatcher.BeginInvoke((Delegate) (() => DialogManager.DialogClosed((object) window, new DialogStateChangedEventArgs())));
            return window.Dispatcher.Invoke<Task>((Func<Task>) (() => dialog._WaitForCloseAsync())).ContinueWith<Task<LoginDialogData>>((Func<Task, Task<LoginDialogData>>) (a => window.Dispatcher.Invoke<Task>((Func<Task>) (() =>
            {
              window.SizeChanged -= sizeHandler;
              window.RemoveDialog((BaseMetroDialog) dialog);
              return DialogManager.HandleOverlayOnHide((MetroDialogSettings) settings, window);
            })).ContinueWith<Task<LoginDialogData>>((Func<Task, Task<LoginDialogData>>) (y3 => y)).Unwrap<LoginDialogData>()));
          })).Unwrap<Task<LoginDialogData>>();
        })).Unwrap<Task<LoginDialogData>>().Unwrap<LoginDialogData>();
      })))).Unwrap<LoginDialogData>();
    }

    public static Task<string> ShowInputAsync(
      this MetroWindow window,
      string title,
      string message,
      MetroDialogSettings settings = null)
    {
      window.Dispatcher.VerifyAccess();
      return DialogManager.HandleOverlayOnShow(settings, window).ContinueWith<Task<string>>((Func<Task, Task<string>>) (z => window.Dispatcher.Invoke<Task<string>>((Func<Task<string>>) (() =>
      {
        if (settings == null)
          settings = window.MetroDialogOptions;
        InputDialog dialog = new InputDialog(window, settings)
        {
          Title = title,
          Message = message,
          Input = settings.DefaultText
        };
        SizeChangedEventHandler sizeHandler = DialogManager.SetupAndOpenDialog(window, (BaseMetroDialog) dialog);
        dialog.SizeChangedHandler = sizeHandler;
        return dialog.WaitForLoadAsync().ContinueWith<Task<Task<string>>>((Func<Task, Task<Task<string>>>) (x =>
        {
          if (DialogManager.DialogOpened != null)
            window.Dispatcher.BeginInvoke((Delegate) (() => DialogManager.DialogOpened((object) window, new DialogStateChangedEventArgs())));
          return dialog.WaitForButtonPressAsync().ContinueWith<Task<Task<string>>>((Func<Task<string>, Task<Task<string>>>) (y =>
          {
            dialog.OnClose();
            if (DialogManager.DialogClosed != null)
              window.Dispatcher.BeginInvoke((Delegate) (() => DialogManager.DialogClosed((object) window, new DialogStateChangedEventArgs())));
            return window.Dispatcher.Invoke<Task>((Func<Task>) (() => dialog._WaitForCloseAsync())).ContinueWith<Task<string>>((Func<Task, Task<string>>) (a => window.Dispatcher.Invoke<Task>((Func<Task>) (() =>
            {
              window.SizeChanged -= sizeHandler;
              window.RemoveDialog((BaseMetroDialog) dialog);
              return DialogManager.HandleOverlayOnHide(settings, window);
            })).ContinueWith<Task<string>>((Func<Task, Task<string>>) (y3 => y)).Unwrap<string>()));
          })).Unwrap<Task<string>>();
        })).Unwrap<Task<string>>().Unwrap<string>();
      })))).Unwrap<string>();
    }

    public static Task<MessageDialogResult> ShowMessageAsync(
      this MetroWindow window,
      string title,
      string message,
      MessageDialogStyle style = MessageDialogStyle.Affirmative,
      MetroDialogSettings settings = null)
    {
      window.Dispatcher.VerifyAccess();
      return DialogManager.HandleOverlayOnShow(settings, window).ContinueWith<Task<MessageDialogResult>>((Func<Task, Task<MessageDialogResult>>) (z => window.Dispatcher.Invoke<Task<MessageDialogResult>>((Func<Task<MessageDialogResult>>) (() =>
      {
        if (settings == null)
          settings = window.MetroDialogOptions;
        MessageDialog dialog = new MessageDialog(window, settings)
        {
          Message = message,
          Title = title,
          ButtonStyle = style
        };
        SizeChangedEventHandler sizeHandler = DialogManager.SetupAndOpenDialog(window, (BaseMetroDialog) dialog);
        dialog.SizeChangedHandler = sizeHandler;
        return dialog.WaitForLoadAsync().ContinueWith<Task<Task<MessageDialogResult>>>((Func<Task, Task<Task<MessageDialogResult>>>) (x =>
        {
          if (DialogManager.DialogOpened != null)
            window.Dispatcher.BeginInvoke((Delegate) (() => DialogManager.DialogOpened((object) window, new DialogStateChangedEventArgs())));
          return dialog.WaitForButtonPressAsync().ContinueWith<Task<Task<MessageDialogResult>>>((Func<Task<MessageDialogResult>, Task<Task<MessageDialogResult>>>) (y =>
          {
            dialog.OnClose();
            if (DialogManager.DialogClosed != null)
              window.Dispatcher.BeginInvoke((Delegate) (() => DialogManager.DialogClosed((object) window, new DialogStateChangedEventArgs())));
            return window.Dispatcher.Invoke<Task>((Func<Task>) (() => dialog._WaitForCloseAsync())).ContinueWith<Task<MessageDialogResult>>((Func<Task, Task<MessageDialogResult>>) (a => window.Dispatcher.Invoke<Task>((Func<Task>) (() =>
            {
              window.SizeChanged -= sizeHandler;
              window.RemoveDialog((BaseMetroDialog) dialog);
              return DialogManager.HandleOverlayOnHide(settings, window);
            })).ContinueWith<Task<MessageDialogResult>>((Func<Task, Task<MessageDialogResult>>) (y3 => y)).Unwrap<MessageDialogResult>()));
          })).Unwrap<Task<MessageDialogResult>>();
        })).Unwrap<Task<MessageDialogResult>>().Unwrap<MessageDialogResult>();
      })))).Unwrap<MessageDialogResult>();
    }

    public static Task<ProgressDialogController> ShowProgressAsync(
      this MetroWindow window,
      string title,
      string message,
      bool isCancelable = false,
      MetroDialogSettings settings = null)
    {
      window.Dispatcher.VerifyAccess();
      return DialogManager.HandleOverlayOnShow(settings, window).ContinueWith<Task<ProgressDialogController>>((Func<Task, Task<ProgressDialogController>>) (z => window.Dispatcher.Invoke<Task<ProgressDialogController>>((Func<Task<ProgressDialogController>>) (() =>
      {
        ProgressDialog dialog = new ProgressDialog(window)
        {
          Message = message,
          Title = title,
          IsCancelable = isCancelable
        };
        if (settings == null)
          settings = window.MetroDialogOptions;
        dialog.NegativeButtonText = settings.NegativeButtonText;
        SizeChangedEventHandler sizeHandler = DialogManager.SetupAndOpenDialog(window, (BaseMetroDialog) dialog);
        dialog.SizeChangedHandler = sizeHandler;
        return dialog.WaitForLoadAsync().ContinueWith<ProgressDialogController>((Func<Task, ProgressDialogController>) (x =>
        {
          if (DialogManager.DialogOpened != null)
            window.Dispatcher.BeginInvoke((Delegate) (() => DialogManager.DialogOpened((object) window, new DialogStateChangedEventArgs())));
          return new ProgressDialogController(dialog, (Func<Task>) (() =>
          {
            dialog.OnClose();
            if (DialogManager.DialogClosed != null)
              window.Dispatcher.BeginInvoke((Delegate) (() => DialogManager.DialogClosed((object) window, new DialogStateChangedEventArgs())));
            return window.Dispatcher.Invoke<Task>((Func<Task>) (() => dialog._WaitForCloseAsync())).ContinueWith<Task>((Func<Task, Task>) (a => window.Dispatcher.Invoke<Task>((Func<Task>) (() =>
            {
              window.SizeChanged -= sizeHandler;
              window.RemoveDialog((BaseMetroDialog) dialog);
              return DialogManager.HandleOverlayOnHide(settings, window);
            })))).Unwrap();
          }));
        }));
      })))).Unwrap<ProgressDialogController>();
    }

    private static Task HandleOverlayOnHide(MetroDialogSettings settings, MetroWindow window)
    {
      if (!window.metroActiveDialogContainer.Children.OfType<BaseMetroDialog>().Any<BaseMetroDialog>())
        return settings != null && !settings.AnimateHide ? Task.Factory.StartNew((Action) (() => window.Dispatcher.Invoke(new Action(window.HideOverlay)))) : window.HideOverlayAsync();
      TaskCompletionSource<object> completionSource = new TaskCompletionSource<object>();
      completionSource.SetResult((object) null);
      return (Task) completionSource.Task;
    }

    private static Task HandleOverlayOnShow(MetroDialogSettings settings, MetroWindow window)
    {
      if (!window.metroActiveDialogContainer.Children.OfType<BaseMetroDialog>().Any<BaseMetroDialog>())
        return settings != null && !settings.AnimateShow ? Task.Factory.StartNew((Action) (() => window.Dispatcher.Invoke(new Action(window.ShowOverlay)))) : window.ShowOverlayAsync();
      TaskCompletionSource<object> completionSource = new TaskCompletionSource<object>();
      completionSource.SetResult((object) null);
      return (Task) completionSource.Task;
    }

    public static Task ShowMetroDialogAsync(
      this MetroWindow window,
      BaseMetroDialog dialog,
      MetroDialogSettings settings = null)
    {
      window.Dispatcher.VerifyAccess();
      if (window.metroActiveDialogContainer.Children.Contains((UIElement) dialog) || window.metroInactiveDialogContainer.Children.Contains((UIElement) dialog))
        throw new InvalidOperationException("The provided dialog is already visible in the specified window.");
      return (Task) DialogManager.HandleOverlayOnShow(settings, window).ContinueWith((Action<Task>) (z => dialog.Dispatcher.Invoke((Action) (() => dialog.SizeChangedHandler = DialogManager.SetupAndOpenDialog(window, dialog))))).ContinueWith<Task>((Func<Task, Task>) (y => dialog.Dispatcher.Invoke<Task>((Func<Task>) (() => dialog.WaitForLoadAsync().ContinueWith((Action<Task>) (x =>
      {
        dialog.OnShown();
        if (DialogManager.DialogOpened == null)
          return;
        DialogManager.DialogOpened((object) window, new DialogStateChangedEventArgs());
      }))))));
    }

    public static Task HideMetroDialogAsync(
      this MetroWindow window,
      BaseMetroDialog dialog,
      MetroDialogSettings settings = null)
    {
      window.Dispatcher.VerifyAccess();
      if (!window.metroActiveDialogContainer.Children.Contains((UIElement) dialog) && !window.metroInactiveDialogContainer.Children.Contains((UIElement) dialog))
        throw new InvalidOperationException("The provided dialog is not visible in the specified window.");
      window.SizeChanged -= dialog.SizeChangedHandler;
      dialog.OnClose();
      return window.Dispatcher.Invoke<Task>(new Func<Task>(dialog._WaitForCloseAsync)).ContinueWith<Task>((Func<Task, Task>) (a =>
      {
        if (DialogManager.DialogClosed != null)
          window.Dispatcher.BeginInvoke((Delegate) (() => DialogManager.DialogClosed((object) window, new DialogStateChangedEventArgs())));
        return window.Dispatcher.Invoke<Task>((Func<Task>) (() =>
        {
          window.RemoveDialog(dialog);
          return DialogManager.HandleOverlayOnHide(settings, window);
        }));
      })).Unwrap();
    }

    public static Task<TDialog> GetCurrentDialogAsync<TDialog>(this MetroWindow window) where TDialog : BaseMetroDialog
    {
      window.Dispatcher.VerifyAccess();
      TaskCompletionSource<TDialog> t = new TaskCompletionSource<TDialog>();
      window.Dispatcher.Invoke((Action) (() => t.TrySetResult(window.metroActiveDialogContainer.Children.OfType<TDialog>().LastOrDefault<TDialog>())));
      return t.Task;
    }

    private static SizeChangedEventHandler SetupAndOpenDialog(
      MetroWindow window,
      BaseMetroDialog dialog)
    {
      dialog.SetValue(Panel.ZIndexProperty, (object) ((int) window.overlayBox.GetValue(Panel.ZIndexProperty) + 1));
      dialog.MinHeight = window.ActualHeight / 4.0;
      dialog.MaxHeight = window.ActualHeight;
      SizeChangedEventHandler changedEventHandler = (SizeChangedEventHandler) ((sender, args) =>
      {
        dialog.MinHeight = window.ActualHeight / 4.0;
        dialog.MaxHeight = window.ActualHeight;
      });
      window.SizeChanged += changedEventHandler;
      window.AddDialog(dialog);
      dialog.OnShown();
      return changedEventHandler;
    }

    private static void AddDialog(this MetroWindow window, BaseMetroDialog dialog)
    {
      UIElement element = window.metroActiveDialogContainer.Children.Cast<UIElement>().SingleOrDefault<UIElement>();
      if (element != null)
      {
        window.metroActiveDialogContainer.Children.Remove(element);
        window.metroInactiveDialogContainer.Children.Add(element);
      }
      window.metroActiveDialogContainer.Children.Add((UIElement) dialog);
    }

    private static void RemoveDialog(this MetroWindow window, BaseMetroDialog dialog)
    {
      if (window.metroActiveDialogContainer.Children.Contains((UIElement) dialog))
      {
        window.metroActiveDialogContainer.Children.Remove((UIElement) dialog);
        UIElement element = window.metroInactiveDialogContainer.Children.Cast<UIElement>().LastOrDefault<UIElement>();
        if (element == null)
          return;
        window.metroInactiveDialogContainer.Children.Remove(element);
        window.metroActiveDialogContainer.Children.Add(element);
      }
      else
        window.metroInactiveDialogContainer.Children.Remove((UIElement) dialog);
    }

    public static BaseMetroDialog ShowDialogExternally(this BaseMetroDialog dialog)
    {
      Window window = DialogManager.SetupExternalDialogWindow(dialog);
      dialog.OnShown();
      window.Show();
      return dialog;
    }

    public static BaseMetroDialog ShowModalDialogExternally(this BaseMetroDialog dialog)
    {
      Window window = DialogManager.SetupExternalDialogWindow(dialog);
      dialog.OnShown();
      window.ShowDialog();
      return dialog;
    }

    private static Window SetupExternalDialogWindow(BaseMetroDialog dialog)
    {
      MetroWindow metroWindow = new MetroWindow();
      metroWindow.ShowInTaskbar = false;
      metroWindow.ShowActivated = true;
      metroWindow.Topmost = true;
      metroWindow.ResizeMode = ResizeMode.NoResize;
      metroWindow.WindowStyle = WindowStyle.None;
      metroWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
      metroWindow.ShowTitleBar = false;
      metroWindow.ShowCloseButton = false;
      metroWindow.WindowTransitionsEnabled = false;
      MetroWindow win = metroWindow;
      try
      {
        win.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
          Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml")
        });
        win.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
          Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml")
        });
        win.Resources.MergedDictionaries.Add(new ResourceDictionary()
        {
          Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Colors.xaml")
        });
        win.SetResourceReference(MetroWindow.GlowBrushProperty, (object) "AccentColorBrush");
      }
      catch (Exception ex)
      {
      }
      win.Width = SystemParameters.PrimaryScreenWidth;
      win.MinHeight = SystemParameters.PrimaryScreenHeight / 4.0;
      win.SizeToContent = SizeToContent.Height;
      dialog.ParentDialogWindow = (Window) win;
      win.Content = (object) dialog;
      EventHandler closedHandler = (EventHandler) null;
      closedHandler = (EventHandler) ((sender, args) =>
      {
        win.Closed -= closedHandler;
        dialog.ParentDialogWindow = (Window) null;
        win.Content = (object) null;
      });
      win.Closed += closedHandler;
      return (Window) win;
    }

    public static event EventHandler<DialogStateChangedEventArgs> DialogOpened;

    public static event EventHandler<DialogStateChangedEventArgs> DialogClosed;
  }
}

// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.Dialogs.MessageDialog
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace MahApps.Metro.Controls.Dialogs
{
  public class MessageDialog : BaseMetroDialog, IComponentConnector
  {
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof (Message), typeof (string), typeof (MessageDialog), new PropertyMetadata((object) null));
    public static readonly DependencyProperty AffirmativeButtonTextProperty = DependencyProperty.Register(nameof (AffirmativeButtonText), typeof (string), typeof (MessageDialog), new PropertyMetadata((object) "OK"));
    public static readonly DependencyProperty NegativeButtonTextProperty = DependencyProperty.Register(nameof (NegativeButtonText), typeof (string), typeof (MessageDialog), new PropertyMetadata((object) "Cancel"));
    public static readonly DependencyProperty FirstAuxiliaryButtonTextProperty = DependencyProperty.Register(nameof (FirstAuxiliaryButtonText), typeof (string), typeof (MessageDialog), new PropertyMetadata((object) "Cancel"));
    public static readonly DependencyProperty SecondAuxiliaryButtonTextProperty = DependencyProperty.Register(nameof (SecondAuxiliaryButtonText), typeof (string), typeof (MessageDialog), new PropertyMetadata((object) "Cancel"));
    public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register(nameof (ButtonStyle), typeof (MessageDialogStyle), typeof (MessageDialog), new PropertyMetadata((object) MessageDialogStyle.Affirmative, (PropertyChangedCallback) ((s, e) => MessageDialog.SetButtonState((MessageDialog) s))));
    internal ScrollViewer PART_MessageScrollViewer;
    internal TextBlock PART_MessageTextBlock;
    internal Button PART_AffirmativeButton;
    internal Button PART_NegativeButton;
    internal Button PART_FirstAuxiliaryButton;
    internal Button PART_SecondAuxiliaryButton;
    private bool _contentLoaded;

    internal MessageDialog(MetroWindow parentWindow)
      : this(parentWindow, (MetroDialogSettings) null)
    {
    }

    internal MessageDialog(MetroWindow parentWindow, MetroDialogSettings settings)
      : base(parentWindow, settings)
    {
      this.InitializeComponent();
      this.PART_MessageScrollViewer.Height = this.DialogSettings.MaximumBodyHeight;
    }

    internal Task<MessageDialogResult> WaitForButtonPressAsync()
    {
      this.Dispatcher.BeginInvoke((Delegate) (() =>
      {
        this.Focus();
        MessageDialogResult messageDialogResult = this.DialogSettings.DefaultButtonFocus;
        if (!this.IsApplicable(messageDialogResult))
          messageDialogResult = this.ButtonStyle == MessageDialogStyle.Affirmative ? MessageDialogResult.Affirmative : MessageDialogResult.Negative;
        switch (messageDialogResult)
        {
          case MessageDialogResult.Negative:
            this.PART_NegativeButton.Focus();
            break;
          case MessageDialogResult.Affirmative:
            this.PART_AffirmativeButton.Focus();
            break;
          case MessageDialogResult.FirstAuxiliary:
            this.PART_FirstAuxiliaryButton.Focus();
            break;
          case MessageDialogResult.SecondAuxiliary:
            this.PART_SecondAuxiliaryButton.Focus();
            break;
        }
      }));
      TaskCompletionSource<MessageDialogResult> tcs = new TaskCompletionSource<MessageDialogResult>();
      RoutedEventHandler negativeHandler = (RoutedEventHandler) null;
      KeyEventHandler negativeKeyHandler = (KeyEventHandler) null;
      RoutedEventHandler affirmativeHandler = (RoutedEventHandler) null;
      KeyEventHandler affirmativeKeyHandler = (KeyEventHandler) null;
      RoutedEventHandler firstAuxHandler = (RoutedEventHandler) null;
      KeyEventHandler firstAuxKeyHandler = (KeyEventHandler) null;
      RoutedEventHandler secondAuxHandler = (RoutedEventHandler) null;
      KeyEventHandler secondAuxKeyHandler = (KeyEventHandler) null;
      KeyEventHandler escapeKeyHandler = (KeyEventHandler) null;
      Action cleanUpHandlers = (Action) null;
      CancellationTokenRegistration cancellationTokenRegistration = this.DialogSettings.CancellationToken.Register((Action) (() =>
      {
        cleanUpHandlers();
        tcs.TrySetResult(this.ButtonStyle == MessageDialogStyle.Affirmative ? MessageDialogResult.Affirmative : MessageDialogResult.Negative);
      }));
      cleanUpHandlers = (Action) (() =>
      {
        this.PART_NegativeButton.Click -= negativeHandler;
        this.PART_AffirmativeButton.Click -= affirmativeHandler;
        this.PART_FirstAuxiliaryButton.Click -= firstAuxHandler;
        this.PART_SecondAuxiliaryButton.Click -= secondAuxHandler;
        this.PART_NegativeButton.KeyDown -= negativeKeyHandler;
        this.PART_AffirmativeButton.KeyDown -= affirmativeKeyHandler;
        this.PART_FirstAuxiliaryButton.KeyDown -= firstAuxKeyHandler;
        this.PART_SecondAuxiliaryButton.KeyDown -= secondAuxKeyHandler;
        this.KeyDown -= escapeKeyHandler;
        cancellationTokenRegistration.Dispose();
      });
      negativeKeyHandler = (KeyEventHandler) ((sender, e) =>
      {
        if (e.Key != Key.Return)
          return;
        cleanUpHandlers();
        tcs.TrySetResult(MessageDialogResult.Negative);
      });
      affirmativeKeyHandler = (KeyEventHandler) ((sender, e) =>
      {
        if (e.Key != Key.Return)
          return;
        cleanUpHandlers();
        tcs.TrySetResult(MessageDialogResult.Affirmative);
      });
      firstAuxKeyHandler = (KeyEventHandler) ((sender, e) =>
      {
        if (e.Key != Key.Return)
          return;
        cleanUpHandlers();
        tcs.TrySetResult(MessageDialogResult.FirstAuxiliary);
      });
      secondAuxKeyHandler = (KeyEventHandler) ((sender, e) =>
      {
        if (e.Key != Key.Return)
          return;
        cleanUpHandlers();
        tcs.TrySetResult(MessageDialogResult.SecondAuxiliary);
      });
      negativeHandler = (RoutedEventHandler) ((sender, e) =>
      {
        cleanUpHandlers();
        tcs.TrySetResult(MessageDialogResult.Negative);
        e.Handled = true;
      });
      affirmativeHandler = (RoutedEventHandler) ((sender, e) =>
      {
        cleanUpHandlers();
        tcs.TrySetResult(MessageDialogResult.Affirmative);
        e.Handled = true;
      });
      firstAuxHandler = (RoutedEventHandler) ((sender, e) =>
      {
        cleanUpHandlers();
        tcs.TrySetResult(MessageDialogResult.FirstAuxiliary);
        e.Handled = true;
      });
      secondAuxHandler = (RoutedEventHandler) ((sender, e) =>
      {
        cleanUpHandlers();
        tcs.TrySetResult(MessageDialogResult.SecondAuxiliary);
        e.Handled = true;
      });
      escapeKeyHandler = (KeyEventHandler) ((sender, e) =>
      {
        if (e.Key == Key.Escape)
        {
          cleanUpHandlers();
          tcs.TrySetResult(this.ButtonStyle == MessageDialogStyle.Affirmative ? MessageDialogResult.Affirmative : MessageDialogResult.Negative);
        }
        else
        {
          if (e.Key != Key.Return)
            return;
          cleanUpHandlers();
          tcs.TrySetResult(MessageDialogResult.Affirmative);
        }
      });
      this.PART_NegativeButton.KeyDown += negativeKeyHandler;
      this.PART_AffirmativeButton.KeyDown += affirmativeKeyHandler;
      this.PART_FirstAuxiliaryButton.KeyDown += firstAuxKeyHandler;
      this.PART_SecondAuxiliaryButton.KeyDown += secondAuxKeyHandler;
      this.PART_NegativeButton.Click += negativeHandler;
      this.PART_AffirmativeButton.Click += affirmativeHandler;
      this.PART_FirstAuxiliaryButton.Click += firstAuxHandler;
      this.PART_SecondAuxiliaryButton.Click += secondAuxHandler;
      this.KeyDown += escapeKeyHandler;
      return tcs.Task;
    }

    private static void SetButtonState(MessageDialog md)
    {
      if (md.PART_AffirmativeButton == null)
        return;
      switch (md.ButtonStyle)
      {
        case MessageDialogStyle.Affirmative:
          md.PART_AffirmativeButton.Visibility = Visibility.Visible;
          md.PART_NegativeButton.Visibility = Visibility.Collapsed;
          md.PART_FirstAuxiliaryButton.Visibility = Visibility.Collapsed;
          md.PART_SecondAuxiliaryButton.Visibility = Visibility.Collapsed;
          break;
        case MessageDialogStyle.AffirmativeAndNegative:
        case MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary:
        case MessageDialogStyle.AffirmativeAndNegativeAndDoubleAuxiliary:
          md.PART_AffirmativeButton.Visibility = Visibility.Visible;
          md.PART_NegativeButton.Visibility = Visibility.Visible;
          if (md.ButtonStyle == MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary || md.ButtonStyle == MessageDialogStyle.AffirmativeAndNegativeAndDoubleAuxiliary)
            md.PART_FirstAuxiliaryButton.Visibility = Visibility.Visible;
          if (md.ButtonStyle == MessageDialogStyle.AffirmativeAndNegativeAndDoubleAuxiliary)
          {
            md.PART_SecondAuxiliaryButton.Visibility = Visibility.Visible;
            break;
          }
          break;
      }
      md.AffirmativeButtonText = md.DialogSettings.AffirmativeButtonText;
      md.NegativeButtonText = md.DialogSettings.NegativeButtonText;
      md.FirstAuxiliaryButtonText = md.DialogSettings.FirstAuxiliaryButtonText;
      md.SecondAuxiliaryButtonText = md.DialogSettings.SecondAuxiliaryButtonText;
      if (md.DialogSettings.ColorScheme != MetroDialogColorScheme.Accented)
        return;
      md.PART_NegativeButton.Style = md.FindResource((object) "AccentedDialogHighlightedSquareButton") as Style;
      md.PART_FirstAuxiliaryButton.Style = md.FindResource((object) "AccentedDialogHighlightedSquareButton") as Style;
      md.PART_SecondAuxiliaryButton.Style = md.FindResource((object) "AccentedDialogHighlightedSquareButton") as Style;
    }

    protected override void OnLoaded() => MessageDialog.SetButtonState(this);

    public MessageDialogStyle ButtonStyle
    {
      get => (MessageDialogStyle) this.GetValue(MessageDialog.ButtonStyleProperty);
      set => this.SetValue(MessageDialog.ButtonStyleProperty, (object) value);
    }

    public string Message
    {
      get => (string) this.GetValue(MessageDialog.MessageProperty);
      set => this.SetValue(MessageDialog.MessageProperty, (object) value);
    }

    public string AffirmativeButtonText
    {
      get => (string) this.GetValue(MessageDialog.AffirmativeButtonTextProperty);
      set => this.SetValue(MessageDialog.AffirmativeButtonTextProperty, (object) value);
    }

    public string NegativeButtonText
    {
      get => (string) this.GetValue(MessageDialog.NegativeButtonTextProperty);
      set => this.SetValue(MessageDialog.NegativeButtonTextProperty, (object) value);
    }

    public string FirstAuxiliaryButtonText
    {
      get => (string) this.GetValue(MessageDialog.FirstAuxiliaryButtonTextProperty);
      set => this.SetValue(MessageDialog.FirstAuxiliaryButtonTextProperty, (object) value);
    }

    public string SecondAuxiliaryButtonText
    {
      get => (string) this.GetValue(MessageDialog.SecondAuxiliaryButtonTextProperty);
      set => this.SetValue(MessageDialog.SecondAuxiliaryButtonTextProperty, (object) value);
    }

    private void OnKeyCopyExecuted(object sender, ExecutedRoutedEventArgs e)
    {
      Clipboard.SetDataObject((object) this.Message);
    }

    private bool IsApplicable(MessageDialogResult value)
    {
      switch (value)
      {
        case MessageDialogResult.Negative:
          return this.PART_NegativeButton.IsVisible;
        case MessageDialogResult.Affirmative:
          return this.PART_AffirmativeButton.IsVisible;
        case MessageDialogResult.FirstAuxiliary:
          return this.PART_FirstAuxiliaryButton.IsVisible;
        case MessageDialogResult.SecondAuxiliary:
          return this.PART_SecondAuxiliaryButton.IsVisible;
        default:
          return false;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MahApps.Metro;component/themes/dialogs/messagedialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((CommandBinding) target).Executed += new ExecutedRoutedEventHandler(this.OnKeyCopyExecuted);
          break;
        case 2:
          this.PART_MessageScrollViewer = (ScrollViewer) target;
          break;
        case 3:
          this.PART_MessageTextBlock = (TextBlock) target;
          break;
        case 4:
          this.PART_AffirmativeButton = (Button) target;
          break;
        case 5:
          this.PART_NegativeButton = (Button) target;
          break;
        case 6:
          this.PART_FirstAuxiliaryButton = (Button) target;
          break;
        case 7:
          this.PART_SecondAuxiliaryButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}

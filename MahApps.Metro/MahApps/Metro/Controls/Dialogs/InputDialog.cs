// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.Dialogs.InputDialog
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
  public class InputDialog : BaseMetroDialog, IComponentConnector
  {
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof (Message), typeof (string), typeof (InputDialog), new PropertyMetadata((object) null));
    public static readonly DependencyProperty InputProperty = DependencyProperty.Register(nameof (Input), typeof (string), typeof (InputDialog), new PropertyMetadata((object) null));
    public static readonly DependencyProperty AffirmativeButtonTextProperty = DependencyProperty.Register(nameof (AffirmativeButtonText), typeof (string), typeof (InputDialog), new PropertyMetadata((object) "OK"));
    public static readonly DependencyProperty NegativeButtonTextProperty = DependencyProperty.Register(nameof (NegativeButtonText), typeof (string), typeof (InputDialog), new PropertyMetadata((object) "Cancel"));
    internal TextBox PART_TextBox;
    internal Button PART_AffirmativeButton;
    internal Button PART_NegativeButton;
    private bool _contentLoaded;

    internal InputDialog(MetroWindow parentWindow)
      : this(parentWindow, (MetroDialogSettings) null)
    {
    }

    internal InputDialog(MetroWindow parentWindow, MetroDialogSettings settings)
      : base(parentWindow, settings)
    {
      this.InitializeComponent();
    }

    internal Task<string> WaitForButtonPressAsync()
    {
      this.Dispatcher.BeginInvoke((Delegate) (() =>
      {
        this.Focus();
        this.PART_TextBox.Focus();
      }));
      TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
      RoutedEventHandler negativeHandler = (RoutedEventHandler) null;
      KeyEventHandler negativeKeyHandler = (KeyEventHandler) null;
      RoutedEventHandler affirmativeHandler = (RoutedEventHandler) null;
      KeyEventHandler affirmativeKeyHandler = (KeyEventHandler) null;
      KeyEventHandler escapeKeyHandler = (KeyEventHandler) null;
      Action cleanUpHandlers = (Action) null;
      CancellationTokenRegistration cancellationTokenRegistration = this.DialogSettings.CancellationToken.Register((Action) (() =>
      {
        cleanUpHandlers();
        tcs.TrySetResult((string) null);
      }));
      cleanUpHandlers = (Action) (() =>
      {
        this.PART_TextBox.KeyDown -= affirmativeKeyHandler;
        this.KeyDown -= escapeKeyHandler;
        this.PART_NegativeButton.Click -= negativeHandler;
        this.PART_AffirmativeButton.Click -= affirmativeHandler;
        this.PART_NegativeButton.KeyDown -= negativeKeyHandler;
        this.PART_AffirmativeButton.KeyDown -= affirmativeKeyHandler;
        cancellationTokenRegistration.Dispose();
      });
      escapeKeyHandler = (KeyEventHandler) ((sender, e) =>
      {
        if (e.Key != Key.Escape)
          return;
        cleanUpHandlers();
        tcs.TrySetResult((string) null);
      });
      negativeKeyHandler = (KeyEventHandler) ((sender, e) =>
      {
        if (e.Key != Key.Return)
          return;
        cleanUpHandlers();
        tcs.TrySetResult((string) null);
      });
      affirmativeKeyHandler = (KeyEventHandler) ((sender, e) =>
      {
        if (e.Key != Key.Return)
          return;
        cleanUpHandlers();
        tcs.TrySetResult(this.Input);
      });
      negativeHandler = (RoutedEventHandler) ((sender, e) =>
      {
        cleanUpHandlers();
        tcs.TrySetResult((string) null);
        e.Handled = true;
      });
      affirmativeHandler = (RoutedEventHandler) ((sender, e) =>
      {
        cleanUpHandlers();
        tcs.TrySetResult(this.Input);
        e.Handled = true;
      });
      this.PART_NegativeButton.KeyDown += negativeKeyHandler;
      this.PART_AffirmativeButton.KeyDown += affirmativeKeyHandler;
      this.PART_TextBox.KeyDown += affirmativeKeyHandler;
      this.KeyDown += escapeKeyHandler;
      this.PART_NegativeButton.Click += negativeHandler;
      this.PART_AffirmativeButton.Click += affirmativeHandler;
      return tcs.Task;
    }

    protected override void OnLoaded()
    {
      this.AffirmativeButtonText = this.DialogSettings.AffirmativeButtonText;
      this.NegativeButtonText = this.DialogSettings.NegativeButtonText;
      if (this.DialogSettings.ColorScheme != MetroDialogColorScheme.Accented)
        return;
      this.PART_NegativeButton.Style = this.FindResource((object) "AccentedDialogHighlightedSquareButton") as Style;
      this.PART_TextBox.SetResourceReference(Control.ForegroundProperty, (object) "BlackColorBrush");
      this.PART_TextBox.SetResourceReference(ControlsHelper.FocusBorderBrushProperty, (object) "TextBoxFocusBorderBrush");
    }

    public string Message
    {
      get => (string) this.GetValue(InputDialog.MessageProperty);
      set => this.SetValue(InputDialog.MessageProperty, (object) value);
    }

    public string Input
    {
      get => (string) this.GetValue(InputDialog.InputProperty);
      set => this.SetValue(InputDialog.InputProperty, (object) value);
    }

    public string AffirmativeButtonText
    {
      get => (string) this.GetValue(InputDialog.AffirmativeButtonTextProperty);
      set => this.SetValue(InputDialog.AffirmativeButtonTextProperty, (object) value);
    }

    public string NegativeButtonText
    {
      get => (string) this.GetValue(InputDialog.NegativeButtonTextProperty);
      set => this.SetValue(InputDialog.NegativeButtonTextProperty, (object) value);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MahApps.Metro;component/themes/dialogs/inputdialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler)
    {
      return Delegate.CreateDelegate(delegateType, (object) this, handler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.PART_TextBox = (TextBox) target;
          break;
        case 2:
          this.PART_AffirmativeButton = (Button) target;
          break;
        case 3:
          this.PART_NegativeButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}

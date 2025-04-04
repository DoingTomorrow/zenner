// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.Dialogs.LoginDialog
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
  public class LoginDialog : BaseMetroDialog, IComponentConnector
  {
    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof (Message), typeof (string), typeof (LoginDialog), new PropertyMetadata((object) null));
    public static readonly DependencyProperty UsernameProperty = DependencyProperty.Register(nameof (Username), typeof (string), typeof (LoginDialog), new PropertyMetadata((object) null));
    public static readonly DependencyProperty UsernameWatermarkProperty = DependencyProperty.Register(nameof (UsernameWatermark), typeof (string), typeof (LoginDialog), new PropertyMetadata((object) null));
    public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(nameof (Password), typeof (string), typeof (LoginDialog), new PropertyMetadata((object) null));
    public static readonly DependencyProperty PasswordWatermarkProperty = DependencyProperty.Register(nameof (PasswordWatermark), typeof (string), typeof (LoginDialog), new PropertyMetadata((object) null));
    public static readonly DependencyProperty AffirmativeButtonTextProperty = DependencyProperty.Register(nameof (AffirmativeButtonText), typeof (string), typeof (LoginDialog), new PropertyMetadata((object) "OK"));
    public static readonly DependencyProperty NegativeButtonTextProperty = DependencyProperty.Register(nameof (NegativeButtonText), typeof (string), typeof (LoginDialog), new PropertyMetadata((object) "Cancel"));
    public static readonly DependencyProperty NegativeButtonButtonVisibilityProperty = DependencyProperty.Register(nameof (NegativeButtonButtonVisibility), typeof (Visibility), typeof (LoginDialog), new PropertyMetadata((object) Visibility.Collapsed));
    public static readonly DependencyProperty ShouldHideUsernameProperty = DependencyProperty.Register(nameof (ShouldHideUsername), typeof (bool), typeof (LoginDialog), new PropertyMetadata((object) false));
    internal TextBox PART_TextBox;
    internal PasswordBox PART_TextBox2;
    internal Button PART_AffirmativeButton;
    internal Button PART_NegativeButton;
    private bool _contentLoaded;

    internal LoginDialog(MetroWindow parentWindow)
      : this(parentWindow, (LoginDialogSettings) null)
    {
    }

    internal LoginDialog(MetroWindow parentWindow, LoginDialogSettings settings)
      : base(parentWindow, (MetroDialogSettings) settings)
    {
      this.InitializeComponent();
      this.Username = settings.InitialUsername;
      this.Password = settings.InitialPassword;
      this.UsernameWatermark = settings.UsernameWatermark;
      this.PasswordWatermark = settings.PasswordWatermark;
      this.NegativeButtonButtonVisibility = settings.NegativeButtonVisibility;
      this.ShouldHideUsername = settings.ShouldHideUsername;
    }

    internal Task<LoginDialogData> WaitForButtonPressAsync()
    {
      this.Dispatcher.BeginInvoke((Delegate) (() =>
      {
        this.Focus();
        if (string.IsNullOrEmpty(this.PART_TextBox.Text) && !this.ShouldHideUsername)
          this.PART_TextBox.Focus();
        else
          this.PART_TextBox2.Focus();
      }));
      TaskCompletionSource<LoginDialogData> tcs = new TaskCompletionSource<LoginDialogData>();
      RoutedEventHandler negativeHandler = (RoutedEventHandler) null;
      KeyEventHandler negativeKeyHandler = (KeyEventHandler) null;
      RoutedEventHandler affirmativeHandler = (RoutedEventHandler) null;
      KeyEventHandler affirmativeKeyHandler = (KeyEventHandler) null;
      KeyEventHandler escapeKeyHandler = (KeyEventHandler) null;
      Action cleanUpHandlers = (Action) null;
      CancellationTokenRegistration cancellationTokenRegistration = this.DialogSettings.CancellationToken.Register((Action) (() =>
      {
        cleanUpHandlers();
        tcs.TrySetResult((LoginDialogData) null);
      }));
      cleanUpHandlers = (Action) (() =>
      {
        this.PART_TextBox.KeyDown -= affirmativeKeyHandler;
        this.PART_TextBox2.KeyDown -= affirmativeKeyHandler;
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
        tcs.TrySetResult((LoginDialogData) null);
      });
      negativeKeyHandler = (KeyEventHandler) ((sender, e) =>
      {
        if (e.Key != Key.Return)
          return;
        cleanUpHandlers();
        tcs.TrySetResult((LoginDialogData) null);
      });
      affirmativeKeyHandler = (KeyEventHandler) ((sender, e) =>
      {
        if (e.Key != Key.Return)
          return;
        cleanUpHandlers();
        tcs.TrySetResult(new LoginDialogData()
        {
          Username = this.Username,
          Password = this.PART_TextBox2.Password
        });
      });
      negativeHandler = (RoutedEventHandler) ((sender, e) =>
      {
        cleanUpHandlers();
        tcs.TrySetResult((LoginDialogData) null);
        e.Handled = true;
      });
      affirmativeHandler = (RoutedEventHandler) ((sender, e) =>
      {
        cleanUpHandlers();
        tcs.TrySetResult(new LoginDialogData()
        {
          Username = this.Username,
          Password = this.PART_TextBox2.Password
        });
        e.Handled = true;
      });
      this.PART_NegativeButton.KeyDown += negativeKeyHandler;
      this.PART_AffirmativeButton.KeyDown += affirmativeKeyHandler;
      this.PART_TextBox.KeyDown += affirmativeKeyHandler;
      this.PART_TextBox2.KeyDown += affirmativeKeyHandler;
      this.KeyDown += escapeKeyHandler;
      this.PART_NegativeButton.Click += negativeHandler;
      this.PART_AffirmativeButton.Click += affirmativeHandler;
      return tcs.Task;
    }

    protected override void OnLoaded()
    {
      if (this.DialogSettings is LoginDialogSettings dialogSettings && dialogSettings.EnablePasswordPreview && this.FindResource((object) "Win8MetroPasswordBox") is Style resource)
        this.PART_TextBox2.Style = resource;
      this.AffirmativeButtonText = this.DialogSettings.AffirmativeButtonText;
      this.NegativeButtonText = this.DialogSettings.NegativeButtonText;
      if (this.DialogSettings.ColorScheme != MetroDialogColorScheme.Accented)
        return;
      this.PART_NegativeButton.Style = this.FindResource((object) "AccentedDialogHighlightedSquareButton") as Style;
      this.PART_TextBox.SetResourceReference(Control.ForegroundProperty, (object) "BlackColorBrush");
      this.PART_TextBox2.SetResourceReference(Control.ForegroundProperty, (object) "BlackColorBrush");
    }

    public string Message
    {
      get => (string) this.GetValue(LoginDialog.MessageProperty);
      set => this.SetValue(LoginDialog.MessageProperty, (object) value);
    }

    public string Username
    {
      get => (string) this.GetValue(LoginDialog.UsernameProperty);
      set => this.SetValue(LoginDialog.UsernameProperty, (object) value);
    }

    public string Password
    {
      get => (string) this.GetValue(LoginDialog.PasswordProperty);
      set => this.SetValue(LoginDialog.PasswordProperty, (object) value);
    }

    public string UsernameWatermark
    {
      get => (string) this.GetValue(LoginDialog.UsernameWatermarkProperty);
      set => this.SetValue(LoginDialog.UsernameWatermarkProperty, (object) value);
    }

    public string PasswordWatermark
    {
      get => (string) this.GetValue(LoginDialog.PasswordWatermarkProperty);
      set => this.SetValue(LoginDialog.PasswordWatermarkProperty, (object) value);
    }

    public string AffirmativeButtonText
    {
      get => (string) this.GetValue(LoginDialog.AffirmativeButtonTextProperty);
      set => this.SetValue(LoginDialog.AffirmativeButtonTextProperty, (object) value);
    }

    public string NegativeButtonText
    {
      get => (string) this.GetValue(LoginDialog.NegativeButtonTextProperty);
      set => this.SetValue(LoginDialog.NegativeButtonTextProperty, (object) value);
    }

    public Visibility NegativeButtonButtonVisibility
    {
      get => (Visibility) this.GetValue(LoginDialog.NegativeButtonButtonVisibilityProperty);
      set => this.SetValue(LoginDialog.NegativeButtonButtonVisibilityProperty, (object) value);
    }

    public bool ShouldHideUsername
    {
      get => (bool) this.GetValue(LoginDialog.ShouldHideUsernameProperty);
      set => this.SetValue(LoginDialog.ShouldHideUsernameProperty, (object) value);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MahApps.Metro;component/themes/dialogs/logindialog.xaml", UriKind.Relative));
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
          this.PART_TextBox2 = (PasswordBox) target;
          break;
        case 3:
          this.PART_AffirmativeButton = (Button) target;
          break;
        case 4:
          this.PART_NegativeButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}

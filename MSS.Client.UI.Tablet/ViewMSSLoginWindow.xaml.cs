// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.MSSLoginWindow
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using MSS.Client.UI.Common;
using MSS.Client.UI.Tablet.Common;
using MSS.Client.UI.Tablet.CustomControls;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using WpfKb.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet.View
{
  public partial class MSSLoginWindow : KeyboardMetroWindow, IComponentConnector
  {
    internal MSSLoginWindow UserControl;
    internal Grid NewLoginWindow;
    internal Grid MSSCommonInfo;
    internal TextBox HardwareBox1;
    internal TextBox UsernameTextBox;
    internal Image CopyIcon;
    internal Border TxtPasswordBorder;
    internal PasswordBox TxtPassword;
    internal CheckBox RememberUserCheckBox;
    internal Button LoginButton;
    internal Button CancelButton;
    internal TouchScreenKeyboardUserControl Keyboard;
    private bool _contentLoaded;

    public MSSLoginWindow()
    {
      this.InitializeComponent();
      this.PasswordNotEmpty((object) this, new RoutedEventArgs());
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.Loaded += new RoutedEventHandler(this.MetroWindow_Loaded);
    }

    ~MSSLoginWindow()
    {
      this.TxtPassword.KeyUp -= new KeyEventHandler(this.PasswordNotEmpty);
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.Loaded -= new RoutedEventHandler(this.MetroWindow_Loaded);
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
      Clipboard.SetText(this.HardwareBox1.Text);
    }

    private void PasswordNotEmpty(object sender, RoutedEventArgs e)
    {
      if (this.TxtPassword.Password.Length == 0)
        this.TxtPasswordBorder.BorderBrush = (Brush) new BrushConverter().ConvertFrom((object) "#DE3914");
      else
        this.TxtPasswordBorder.BorderBrush = (Brush) Brushes.DarkGray;
    }

    private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
    {
      this.RegisterKeyboardEvents(this.Keyboard);
      this.UsernameTextBox.Focus();
    }

    public override void RegisterKeyboardEvents(TouchScreenKeyboardUserControl kb)
    {
      this.FindVisualChildren<TextBox>().ForEach<TextBox>((Action<TextBox>) (_ =>
      {
        _.InputScope = new InputScope()
        {
          Names = {
            (object) new InputScopeName()
            {
              NameValue = InputScopeNameValue.EmailSmtpAddress
            }
          }
        };
        CommonHandlers<OpenKeyboardEventParams>.RegisterKeyboardEvents((Control) _, kb);
      }));
      this.FindVisualChildren<PasswordBox>().ForEach<PasswordBox>((Action<PasswordBox>) (_ =>
      {
        _.InputScope = new InputScope()
        {
          Names = {
            (object) new InputScopeName()
            {
              NameValue = InputScopeNameValue.EmailSmtpAddress
            }
          }
        };
        CommonHandlers<OpenKeyboardEventParams>.RegisterKeyboardEvents((Control) _, kb);
      }));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/mssloginwindow.xaml", UriKind.Relative));
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
          this.UserControl = (MSSLoginWindow) target;
          break;
        case 2:
          this.NewLoginWindow = (Grid) target;
          break;
        case 3:
          this.MSSCommonInfo = (Grid) target;
          break;
        case 4:
          this.HardwareBox1 = (TextBox) target;
          break;
        case 5:
          this.UsernameTextBox = (TextBox) target;
          break;
        case 6:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.ButtonBase_OnClick);
          break;
        case 7:
          this.CopyIcon = (Image) target;
          break;
        case 8:
          this.TxtPasswordBorder = (Border) target;
          break;
        case 9:
          this.TxtPassword = (PasswordBox) target;
          this.TxtPassword.KeyUp += new KeyEventHandler(this.PasswordNotEmpty);
          break;
        case 10:
          this.RememberUserCheckBox = (CheckBox) target;
          break;
        case 11:
          this.LoginButton = (Button) target;
          break;
        case 12:
          this.CancelButton = (Button) target;
          break;
        case 13:
          this.Keyboard = (TouchScreenKeyboardUserControl) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}

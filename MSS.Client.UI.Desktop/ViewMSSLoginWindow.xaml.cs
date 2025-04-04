// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.MSSLoginWindow
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

using MahApps.Metro.Controls;
using MSS.Business.Utils;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace MSS.Client.UI.Desktop.View
{
  public partial class MSSLoginWindow : MetroWindow, IComponentConnector
  {
    internal TextBox UsernameTextBox;
    internal Border TxtPasswordBorder;
    internal PasswordBox TxtPassword;
    internal CheckBox RememberUserCheckBox;
    internal Button LoginButton;
    internal Button CancelButton;
    private bool _contentLoaded;

    public MSSLoginWindow()
    {
      this.InitializeComponent();
      this.PasswordNotEmpty((object) this, new RoutedEventArgs());
      this.UsernameTextBox.Focus();
      this.Icon = (ImageSource) new BitmapImage(new Uri(CustomerConfiguration.GetPropertyValue("LauncherIcon")));
    }

    ~MSSLoginWindow() => this.TxtPassword.KeyUp -= new KeyEventHandler(this.PasswordNotEmpty);

    private void Drag_Window(object sender, MouseButtonEventArgs e)
    {
      if (e.ChangedButton != MouseButton.Left)
        return;
      this.DragMove();
    }

    private void PasswordNotEmpty(object sender, RoutedEventArgs e)
    {
      if (this.TxtPassword.Password.Length == 0)
        this.TxtPasswordBorder.BorderBrush = (Brush) new BrushConverter().ConvertFrom((object) "#DE3914");
      else
        this.TxtPasswordBorder.BorderBrush = (Brush) Brushes.DarkGray;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/mssloginwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.UsernameTextBox = (TextBox) target;
          break;
        case 2:
          this.TxtPasswordBorder = (Border) target;
          break;
        case 3:
          this.TxtPassword = (PasswordBox) target;
          this.TxtPassword.KeyUp += new KeyEventHandler(this.PasswordNotEmpty);
          break;
        case 4:
          this.RememberUserCheckBox = (CheckBox) target;
          break;
        case 5:
          this.LoginButton = (Button) target;
          break;
        case 6:
          this.CancelButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}

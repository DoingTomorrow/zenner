// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Users.CreateUserDialog
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using MSS.Client.UI.Common;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Users
{
  public partial class CreateUserDialog : ResizableMetroWindow, IComponentConnector
  {
    internal TextBox FirstNameBox;
    internal TextBox LastNameBox;
    internal TextBox UsernameBox;
    internal PasswordBox TxtPassword;
    internal PasswordBox TxtPasswordConfirm;
    internal CheckBox PasswordsDoNotMatch;
    internal TextBox OfficeBox;
    internal TextBlock PasswordMatchingErrorBlock;
    internal RadListBox OperationListBox1;
    internal RadListBox RoleList;
    internal Button OkButton;
    internal Button CancelButton;
    private bool _contentLoaded;

    public CreateUserDialog()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      if (this.TxtPassword.Password.Length != 0)
        return;
      this.PasswordMatchingErrorBlock.Visibility = Visibility.Visible;
      this.ConfirmPassword((object) this, new RoutedEventArgs());
    }

    ~CreateUserDialog()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.TxtPassword.KeyUp -= new KeyEventHandler(this.ConfirmPassword);
      this.TxtPasswordConfirm.KeyUp -= new KeyEventHandler(this.ConfirmPassword);
    }

    private void ConfirmPassword(object sender, RoutedEventArgs e)
    {
      if (this.TxtPassword.Password != this.TxtPasswordConfirm.Password || this.TxtPassword.Password.Length == 0)
      {
        this.PasswordMatchingErrorBlock.Visibility = Visibility.Visible;
        this.TxtPassword.BorderBrush = (Brush) new BrushConverter().ConvertFrom((object) "#DE3914");
        this.TxtPassword.BorderThickness = new Thickness(1.2);
        this.TxtPasswordConfirm.BorderBrush = (Brush) new BrushConverter().ConvertFrom((object) "#DE3914");
        this.TxtPasswordConfirm.BorderThickness = new Thickness(1.2);
        this.PasswordsDoNotMatch.IsChecked = new bool?(true);
      }
      else
      {
        this.PasswordMatchingErrorBlock.Visibility = Visibility.Hidden;
        this.TxtPassword.BorderBrush = (Brush) Brushes.LightGray;
        this.TxtPasswordConfirm.BorderBrush = (Brush) Brushes.LightGray;
        this.PasswordsDoNotMatch.IsChecked = new bool?(false);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/users/createuserdialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.FirstNameBox = (TextBox) target;
          break;
        case 2:
          this.LastNameBox = (TextBox) target;
          break;
        case 3:
          this.UsernameBox = (TextBox) target;
          break;
        case 4:
          this.TxtPassword = (PasswordBox) target;
          this.TxtPassword.KeyUp += new KeyEventHandler(this.ConfirmPassword);
          break;
        case 5:
          this.TxtPasswordConfirm = (PasswordBox) target;
          this.TxtPasswordConfirm.KeyUp += new KeyEventHandler(this.ConfirmPassword);
          break;
        case 6:
          this.PasswordsDoNotMatch = (CheckBox) target;
          break;
        case 7:
          this.OfficeBox = (TextBox) target;
          break;
        case 8:
          this.PasswordMatchingErrorBlock = (TextBlock) target;
          break;
        case 9:
          this.OperationListBox1 = (RadListBox) target;
          break;
        case 10:
          this.RoleList = (RadListBox) target;
          break;
        case 11:
          this.OkButton = (Button) target;
          break;
        case 12:
          this.CancelButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}

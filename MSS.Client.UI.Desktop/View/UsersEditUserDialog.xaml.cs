// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Users.EditUserDialog
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

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
namespace MSS.Client.UI.Desktop.View.Users
{
  public partial class EditUserDialog : ResizableMetroWindow, IComponentConnector
  {
    internal PasswordBox TxtPassword;
    internal PasswordBox TxtPasswordConfirm;
    internal CheckBox PasswordsDoNotMatch;
    internal TextBox OfficeBox;
    internal System.Windows.Controls.Label PasswordMatchingErrorBlock;
    internal RadListBox OperationListBox1;
    internal Button OkButton;
    internal Button CancelButton;
    private bool _contentLoaded;

    public EditUserDialog()
    {
      this.InitializeComponent();
      this.PasswordMatchingErrorBlock.Visibility = Visibility.Hidden;
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged += new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated += new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
    }

    ~EditUserDialog()
    {
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged -= new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated -= new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
      this.TxtPassword.KeyUp -= new KeyEventHandler(this.ConfirmPassword);
      this.TxtPasswordConfirm.KeyUp -= new KeyEventHandler(this.ConfirmPassword);
    }

    private void ConfirmPassword(object sender, RoutedEventArgs e)
    {
      if (this.TxtPassword.Password != this.TxtPasswordConfirm.Password)
      {
        this.PasswordMatchingErrorBlock.Visibility = Visibility.Visible;
        this.TxtPassword.BorderBrush = (Brush) new BrushConverter().ConvertFrom((object) "#DE3914");
        this.TxtPasswordConfirm.BorderBrush = (Brush) new BrushConverter().ConvertFrom((object) "#DE3914");
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
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/users/edituserdialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.TxtPassword = (PasswordBox) target;
          this.TxtPassword.KeyUp += new KeyEventHandler(this.ConfirmPassword);
          break;
        case 2:
          this.TxtPasswordConfirm = (PasswordBox) target;
          this.TxtPasswordConfirm.KeyUp += new KeyEventHandler(this.ConfirmPassword);
          break;
        case 3:
          this.PasswordsDoNotMatch = (CheckBox) target;
          break;
        case 4:
          this.OfficeBox = (TextBox) target;
          break;
        case 5:
          this.PasswordMatchingErrorBlock = (System.Windows.Controls.Label) target;
          break;
        case 6:
          this.OperationListBox1 = (RadListBox) target;
          break;
        case 7:
          this.OkButton = (Button) target;
          break;
        case 8:
          this.CancelButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}

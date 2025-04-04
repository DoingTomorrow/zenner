// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Users.UsersUserControl
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Desktop.View.Users
{
  public partial class UsersUserControl : UserControl, IComponentConnector
  {
    private int _currentPagingNumber = 0;
    internal Button AddUser;
    internal TextBlock CreateUserCommand;
    internal Button EditUser;
    internal TextBlock EditUserCommand;
    internal Button RemoveUser;
    internal TextBlock DeleteUserCommand;
    internal RadGridView UsersGridView;
    internal RadDataPager RadDataPager;
    internal Button AddRole;
    internal TextBlock CreateRoleCommand;
    internal Button EditRole;
    internal TextBlock EditRoleCommand;
    internal Button RemoveRole;
    internal TextBlock DeleteRoleCommand;
    internal Button SeeRolePermissions;
    internal RadGridView RoleGridView;
    internal RadDataPager RadDataPager2;
    private bool _contentLoaded;

    public UsersUserControl()
    {
      this.InitializeComponent();
      Windows8Palette.Palette.AccentColor = Color.FromRgb((byte) 15, (byte) 95, (byte) 142);
    }

    ~UsersUserControl()
    {
      this.RadDataPager.PageIndexChanged -= new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
      this.RadDataPager2.PageIndexChanged -= new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
    }

    public void OnPageIndexChange(object sender, PageIndexChangedEventArgs e)
    {
      if (e.NewPageIndex == this._currentPagingNumber)
        return;
      if (e.OldPageIndex >= 0)
        this._currentPagingNumber = e.NewPageIndex;
      else
        ((RadDataPager) sender).MoveToPage(this._currentPagingNumber);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/users/usersusercontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.AddUser = (Button) target;
          break;
        case 2:
          this.CreateUserCommand = (TextBlock) target;
          break;
        case 3:
          this.EditUser = (Button) target;
          break;
        case 4:
          this.EditUserCommand = (TextBlock) target;
          break;
        case 5:
          this.RemoveUser = (Button) target;
          break;
        case 6:
          this.DeleteUserCommand = (TextBlock) target;
          break;
        case 7:
          this.UsersGridView = (RadGridView) target;
          break;
        case 8:
          this.RadDataPager = (RadDataPager) target;
          this.RadDataPager.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
          break;
        case 9:
          this.AddRole = (Button) target;
          break;
        case 10:
          this.CreateRoleCommand = (TextBlock) target;
          break;
        case 11:
          this.EditRole = (Button) target;
          break;
        case 12:
          this.EditRoleCommand = (TextBlock) target;
          break;
        case 13:
          this.RemoveRole = (Button) target;
          break;
        case 14:
          this.DeleteRoleCommand = (TextBlock) target;
          break;
        case 15:
          this.SeeRolePermissions = (Button) target;
          break;
        case 16:
          this.RoleGridView = (RadGridView) target;
          break;
        case 17:
          this.RadDataPager2 = (RadDataPager) target;
          this.RadDataPager2.PageIndexChanged += new EventHandler<PageIndexChangedEventArgs>(this.OnPageIndexChange);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}

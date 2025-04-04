// Decompiled with JetBrains decompiler
// Type: StartupLib.UsersWindow
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using ZR_ClassLibrary;

#nullable disable
namespace StartupLib
{
  public partial class UsersWindow : Window, IComponentConnector
  {
    private List<UserInfo> AllUsers;
    private List<KeyValuePair<string, int>> AllRoles;
    internal GmmCorporateControl gmmCorporateControl1;
    internal System.Windows.Controls.TextBox TextBoxUserFilter;
    internal System.Windows.Controls.Button ButtonReloadByFilter;
    internal System.Windows.Controls.Button bDelete;
    internal System.Windows.Controls.Button bAdd;
    internal System.Windows.Controls.Button bEdit;
    internal System.Windows.Controls.ListView lvUsers;
    private bool _contentLoaded;

    public UsersWindow() => this.InitializeComponent();

    private void Window_Loaded(object sender, RoutedEventArgs e) => this.ShowUsers();

    private void bAdd_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        new EditUserWindow()
        {
          editUserInfo = ((UserInfo) null)
        }.ShowDialog();
        this.ShowUsers();
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("Add error", ex.Message, true);
      }
    }

    private void bEdit_Click(object sender, RoutedEventArgs e) => this.EditUser();

    private void EditUser()
    {
      try
      {
        if (this.lvUsers.SelectedIndex == -1)
          return;
        UserInfo user = UserManager.GetUser(((UserInfo) this.lvUsers.SelectedItem).UserId);
        bool? nullable = new EditUserWindow()
        {
          editUserInfo = user
        }.ShowDialog();
        bool flag = true;
        if (nullable.GetValueOrDefault() == flag & nullable.HasValue && UserManager.CurrentUser.UserId == user.UserId)
        {
          this.DialogResult = new bool?(true);
          this.Close();
        }
        this.ShowUsers();
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("Edit error", ex.Message, true);
      }
    }

    private void bDelete_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.lvUsers.SelectedIndex == -1)
          return;
        UserInfo selectedItem = (UserInfo) this.lvUsers.SelectedItem;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Would you really delete the user '" + selectedItem.Name + "' ?");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("The user never will be complete deleted from database!");
        stringBuilder.AppendLine("* The user will be hidden!");
        stringBuilder.AppendLine("* The user can never be used for loggin in!");
        stringBuilder.AppendLine("* The user name is blocked forever!");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Delete really?");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        if (GMM_MessageBox.ShowMessage("Delete user", stringBuilder.ToString()) != System.Windows.Forms.DialogResult.OK)
          return;
        UserManager.DeleteUser(selectedItem.UserId);
        this.ShowUsers();
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("Delete error", ex.Message, true);
      }
    }

    private void ShowUsers()
    {
      this.AllUsers = UserManager.LoadAndGetUsers((string) null);
      this.AllRoles = UserManager.GetUserRoles();
      this.ShowFilterdUsers();
    }

    private void ShowFilterdUsers()
    {
      bool flag1 = UserManager.CheckPermission("Administrator");
      bool flag2 = UserManager.CheckPermission("Developer");
      string lower = this.TextBoxUserFilter.Text.ToLower();
      List<UserInfo> userInfoList = new List<UserInfo>();
      foreach (UserInfo allUser in this.AllUsers)
      {
        if ((lower.Length <= 0 || allUser.Name.ToLower().Contains(lower)) && (allUser.RoleUserId <= 0 || flag1 && (flag2 || !allUser.Name.Contains("Developer"))))
          userInfoList.Add(allUser);
      }
      foreach (UserInfo userInfo1 in userInfoList)
      {
        UserInfo userInfo = userInfo1;
        if (userInfo.RoleUserId > 0)
        {
          object obj = (object) this.AllRoles.Find((Predicate<KeyValuePair<string, int>>) (item => item.Value == userInfo.RoleUserId));
          if (obj != null)
            userInfo.UserRole = ((KeyValuePair<string, int>) obj).Key;
        }
      }
      this.lvUsers.ItemsSource = (IEnumerable) userInfoList;
      if (!this.lvUsers.HasItems)
        return;
      this.lvUsers.SelectedIndex = 0;
    }

    private void lvUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e) => this.EditUser();

    private void ButtonReloadByFilter_Click(object sender, RoutedEventArgs e)
    {
      this.ShowFilterdUsers();
    }

    private void TextBoxUserFilter_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
      if (e.Key != Key.Return)
        return;
      this.ShowFilterdUsers();
    }

    private void lvUsers_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
      if (this.lvUsers.Items == null && this.lvUsers.Items.Count == 0)
        return;
      if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.C)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Name;Role;UserID");
        for (int index = 0; index < this.lvUsers.Items.Count; ++index)
        {
          UserInfo userInfo = (UserInfo) this.lvUsers.Items[index];
          stringBuilder.Append(userInfo.Name);
          stringBuilder.Append(';');
          stringBuilder.Append(userInfo.UserRole);
          stringBuilder.Append(';');
          stringBuilder.Append(userInfo.UserId.ToString());
          stringBuilder.AppendLine();
        }
        System.Windows.Clipboard.SetText(stringBuilder.ToString());
      }
      else
      {
        if (e.KeyboardDevice.Modifiers != 0)
          return;
        string str = e.Key.ToString();
        for (int index = 0; index < this.lvUsers.Items.Count; ++index)
        {
          if (this.lvUsers.Items[index] is UserInfo && ((UserInfo) this.lvUsers.Items[index]).Name.StartsWith(str))
          {
            this.lvUsers.SelectedIndex = index;
            this.lvUsers.ScrollIntoView(this.lvUsers.Items[index]);
            break;
          }
        }
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      System.Windows.Application.LoadComponent((object) this, new Uri("/StartupLib;component/userswindow.xaml", UriKind.Relative));
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
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
          break;
        case 2:
          this.gmmCorporateControl1 = (GmmCorporateControl) target;
          break;
        case 3:
          this.TextBoxUserFilter = (System.Windows.Controls.TextBox) target;
          this.TextBoxUserFilter.KeyDown += new System.Windows.Input.KeyEventHandler(this.TextBoxUserFilter_KeyDown);
          break;
        case 4:
          this.ButtonReloadByFilter = (System.Windows.Controls.Button) target;
          this.ButtonReloadByFilter.Click += new RoutedEventHandler(this.ButtonReloadByFilter_Click);
          break;
        case 5:
          this.bDelete = (System.Windows.Controls.Button) target;
          this.bDelete.Click += new RoutedEventHandler(this.bDelete_Click);
          break;
        case 6:
          this.bAdd = (System.Windows.Controls.Button) target;
          this.bAdd.Click += new RoutedEventHandler(this.bAdd_Click);
          break;
        case 7:
          this.bEdit = (System.Windows.Controls.Button) target;
          this.bEdit.Click += new RoutedEventHandler(this.bEdit_Click);
          break;
        case 8:
          this.lvUsers = (System.Windows.Controls.ListView) target;
          this.lvUsers.MouseDoubleClick += new MouseButtonEventHandler(this.lvUsers_MouseDoubleClick);
          this.lvUsers.KeyDown += new System.Windows.Input.KeyEventHandler(this.lvUsers_KeyDown);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}

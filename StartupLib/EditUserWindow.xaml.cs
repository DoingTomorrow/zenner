// Decompiled with JetBrains decompiler
// Type: StartupLib.EditUserWindow
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using GmmDbLib;
using PlugInLib;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using ZR_ClassLibrary;

#nullable disable
namespace StartupLib
{
  public partial class EditUserWindow : Window, IComponentConnector
  {
    public UserInfo editUserInfo;
    private List<KeyValuePair<string, int>> UserRolesList;
    private bool EditMode;
    private bool Initializing;
    private List<CheckBox> allCheckBoxes = new List<CheckBox>();
    private bool onlyRoleAllowed;
    private FullRightInfoList rightInfoList;
    private StringBuilder infoText = new StringBuilder();
    private bool CheckRunning = false;
    internal GmmCorporateControl gmmCorporateControl1;
    internal CheckBox CheckBoxRemoveDisabledRightsFromDatabase;
    internal StackPanel StackPanalButtons;
    internal Button ButtonSave;
    internal Button ShowLastFingerprintData;
    internal Button ButtonCopyRights;
    internal Button ButtonPastRights;
    internal Button ButtonAddPastedRights;
    internal Label LabelRightColoring;
    internal ListBox ListBoxRightsColoring;
    internal TextBox TextBoxInfo;
    internal StackPanel StackPanalExtendedUserInfo;
    internal TextBox tbUserInfo;
    internal CheckBox CheckBoxRole;
    internal Label lUserName;
    internal TextBox tbUserName;
    internal StackPanel StackPanalPersonalInfow;
    internal Label lPassword;
    internal PasswordBox tbPassword;
    internal Label lConfirmPassword;
    internal PasswordBox tbConfirmPassword;
    internal Label lPersonalNumber;
    internal TextBox tbPersonalNumber;
    internal TextBox tbPhoneNumber;
    internal TextBox tbEmailAddress;
    internal ComboBox ComboBoxRoles;
    internal StackPanel StackPanalFingerPrintData;
    internal Label lPNSource;
    internal ComboBox ComboBosPNSource;
    internal CheckBox OnlyFingerprintLogin;
    internal Label lPermissions;
    internal TreeView TreeViewRights;
    private bool _contentLoaded;

    public EditUserWindow()
    {
      this.Initializing = true;
      this.InitializeComponent();
      this.onlyRoleAllowed = !UserManager.CheckPermission("Administrator");
      this.ListBoxRightsColoring.Items.Clear();
      ItemCollection items = this.ListBoxRightsColoring.Items;
      CheckBox newItem = new CheckBox();
      newItem.FontSize = 12.0;
      newItem.Content = (object) new TextBlock()
      {
        Text = "Out of admin range",
        Background = (Brush) Brushes.LightPink
      };
      items.Add((object) newItem);
      this.UserRolesList = UserManager.GetUserRoles();
      if (this.onlyRoleAllowed && this.UserRolesList.Count < 1)
      {
        int num = (int) GMM_MessageBox.ShowMessage("Defination error", "User editing by roles without roles not possible.", true);
        this.Close();
      }
      else
      {
        this.ComboBosPNSource.ItemsSource = (IEnumerable) Enum.GetNames(typeof (UserInfo.PN_Source));
        this.ComboBosPNSource.SelectedIndex = 0;
        List<string> stringList = new List<string>();
        if (!this.onlyRoleAllowed)
          stringList.Add("none");
        foreach (KeyValuePair<string, int> userRoles in this.UserRolesList)
          stringList.Add(userRoles.Key);
        this.ComboBoxRoles.ItemsSource = (IEnumerable) stringList;
        if (this.onlyRoleAllowed)
        {
          this.CheckBoxRole.Visibility = Visibility.Hidden;
          this.LabelRightColoring.Visibility = Visibility.Hidden;
          this.ButtonCopyRights.Visibility = Visibility.Hidden;
          this.ButtonPastRights.Visibility = Visibility.Hidden;
          this.ButtonAddPastedRights.Visibility = Visibility.Hidden;
          this.ListBoxRightsColoring.Visibility = Visibility.Hidden;
        }
        this.Initializing = false;
        this.ComboBoxRoles.SelectedIndex = 0;
      }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      if (this.editUserInfo != null)
      {
        this.Title = "Edit user";
        this.EditMode = true;
        if (this.editUserInfo.RoleUserId > 0)
        {
          int num = -1;
          for (int index = 0; index < this.UserRolesList.Count; ++index)
          {
            if (this.UserRolesList[index].Value == this.editUserInfo.RoleUserId)
            {
              num = !this.onlyRoleAllowed ? index + 1 : index;
              break;
            }
          }
          this.ComboBoxRoles.SelectedIndex = num;
          this.LoadPermissions(UserManager.GetUser(this.editUserInfo.RoleUserId));
          foreach (UIElement allCheckBox in this.allCheckBoxes)
            allCheckBox.IsEnabled = false;
        }
        else
          this.LoadPermissions(this.editUserInfo);
        if (this.editUserInfo.Name.StartsWith("* UserRole * "))
        {
          this.tbUserName.Text = this.editUserInfo.Name.Substring("* UserRole * ".Length);
          this.CheckBoxRole.IsChecked = new bool?(true);
        }
        else
          this.tbUserName.Text = this.editUserInfo.Name;
        this.tbPersonalNumber.Text = this.editUserInfo.PersonalNumber.ToString();
        if (this.editUserInfo.Password == "??")
        {
          this.tbPassword.Password = "??";
          this.tbConfirmPassword.Password = "???";
        }
        else
        {
          this.tbPassword.Password = "";
          this.tbConfirmPassword.Password = "";
        }
        if (this.editUserInfo.OnlyFinterprintLogin)
          this.OnlyFingerprintLogin.IsChecked = new bool?(true);
        else
          this.OnlyFingerprintLogin.IsChecked = new bool?(false);
        int num1 = -1;
        for (int index = 0; index < this.ComboBosPNSource.Items.Count; ++index)
        {
          if (this.ComboBosPNSource.Items[index].ToString() == this.editUserInfo.PNSource.ToString())
          {
            num1 = index;
            break;
          }
        }
        this.ComboBosPNSource.SelectedIndex = num1;
        this.tbPhoneNumber.Text = this.editUserInfo.PhoneNumber;
        this.tbEmailAddress.Text = this.editUserInfo.EmailAddress;
        this.tbUserInfo.Text = this.editUserInfo.UserExtendedInfo;
        foreach (CheckBox allCheckBox in this.allCheckBoxes)
        {
          CheckBox cbRight = allCheckBox;
          if (this.editUserInfo.Permissions.Where<PermissionInfo>((Func<PermissionInfo, bool>) (a => a.PermissionName.Equals(cbRight.Content) && a.PermissionValue.Equals(true))).Count<PermissionInfo>() > 0)
            cbRight.IsChecked = new bool?(true);
        }
        this.CheckBoxRole.IsEnabled = false;
        this.ShowLastFingerprintData.IsEnabled = false;
      }
      else
      {
        this.Title = "Add user";
        if (!this.onlyRoleAllowed)
          this.LoadPermissions((UserInfo) null);
      }
      this.UserDataChanged();
      if ((List<KeyValuePair<string, bool>>) Clipboard.GetData(DataFormats.Serializable) == null)
      {
        this.ButtonPastRights.IsEnabled = false;
        this.ButtonAddPastedRights.IsEnabled = false;
      }
      else
      {
        this.ButtonPastRights.IsEnabled = true;
        this.ButtonAddPastedRights.IsEnabled = true;
      }
    }

    private void ButtonSave_Click(object sender, RoutedEventArgs e)
    {
      this.PrepareUser();
      bool flag;
      if (this.EditMode)
      {
        flag = UserManager.EditUser(this.editUserInfo);
        if (flag)
        {
          this.DialogResult = new bool?(true);
        }
        else
        {
          int num = (int) GMM_MessageBox.ShowMessage("Edit user", this.editUserInfo.ErrorMessageText);
        }
      }
      else
      {
        flag = UserManager.AddUser(this.editUserInfo, -1);
        if (flag)
        {
          this.DialogResult = new bool?(true);
        }
        else
        {
          int num = (int) GMM_MessageBox.ShowMessage("Add user", this.editUserInfo.ErrorMessageText);
        }
      }
      if (!flag)
        return;
      this.Close();
    }

    private void LoadPermissions(UserInfo activeUserInfo)
    {
      this.rightInfoList = new FullRightInfoList();
      this.rightInfoList.AddLicenseRights();
      this.rightInfoList.AddScannedRights();
      this.rightInfoList.AddCurrentUserRights(UserManager.CurrentUser.UserId);
      this.rightInfoList.AddEditUserRights(activeUserInfo);
      this.rightInfoList.FinalWorkAndSort();
      this.rightInfoList.SetEnabledRights();
      this.allCheckBoxes.Clear();
      this.TreeViewRights.Items.Clear();
      string str = (string) null;
      TreeViewItem treeViewItem1 = (TreeViewItem) null;
      foreach (FullRightInfo rights in this.rightInfoList.RightsList)
      {
        if (!(rights.rightPathParts[0] == "Setup"))
        {
          while (rights.rightPath != str)
          {
            if (string.IsNullOrEmpty(str))
            {
              treeViewItem1 = this.CreateNewTreeNodeItem(rights.rightPath);
              str = treeViewItem1.Name;
              this.TreeViewRights.Items.Add((object) treeViewItem1);
            }
            else
            {
              string extentionForPath = rights.GetExtentionForPath(str);
              if (extentionForPath != null)
              {
                ItemCollection items = treeViewItem1.Items;
                treeViewItem1 = this.CreateNewTreeNodeItem(extentionForPath);
                str = str + "\\" + extentionForPath;
                bool flag = false;
                for (int index = 0; index < items.Count; ++index)
                {
                  if (((HeaderedItemsControl) items[index]).Header is CheckBox)
                  {
                    items.Insert(index, (object) treeViewItem1);
                    flag = true;
                    break;
                  }
                }
                if (!flag)
                  items.Add((object) treeViewItem1);
              }
              else if (!(treeViewItem1.Parent is TreeViewItem))
              {
                str = (string) null;
              }
              else
              {
                treeViewItem1 = (TreeViewItem) treeViewItem1.Parent;
                str = rights.GetParentPath(str);
                ItemCollection itemCollection = treeViewItem1.Parent is TreeViewItem ? ((ItemsControl) treeViewItem1.Parent).Items : this.TreeViewRights.Items;
              }
            }
          }
          TextBlock textBlock = new TextBlock();
          textBlock.Text = rights.rightName;
          CheckBox checkBox = new CheckBox();
          checkBox.FontSize = 12.0;
          checkBox.Content = (object) textBlock;
          checkBox.IsChecked = new bool?(rights.Enabled);
          checkBox.Tag = (object) rights;
          if (this.onlyRoleAllowed)
            checkBox.IsEnabled = false;
          else if (!this.rightInfoList.IsEditByUserEnabled(rights))
          {
            textBlock.Background = (Brush) Brushes.LightPink;
            checkBox.IsEnabled = false;
          }
          this.allCheckBoxes.Add(checkBox);
          TreeViewItem treeViewItem2 = new TreeViewItem();
          treeViewItem2.Header = (object) checkBox;
          TreeViewItem treeViewItem3 = treeViewItem2;
          treeViewItem3.ContextMenu = this.GetRemoveFromDbMenu(treeViewItem3);
          treeViewItem1.Items.Add((object) treeViewItem3);
        }
      }
    }

    private TreeViewItem CreateNewTreeNodeItem(string name)
    {
      string str = name;
      int length = name.IndexOf("\\");
      if (length >= 0)
        str = name.Substring(0, length);
      TreeViewItem controlItem = new TreeViewItem();
      controlItem.Header = (object) str;
      controlItem.Name = str;
      controlItem.ContextMenu = this.GetSetClearAllMenu(controlItem);
      return controlItem;
    }

    private ContextMenu GetSetClearAllMenu(TreeViewItem controlItem)
    {
      ContextMenu setClearAllMenu = new ContextMenu();
      MenuItem newItem1 = new MenuItem();
      newItem1.Header = (object) "Clear all";
      newItem1.Click += new RoutedEventHandler(this.ContextManuClearAll);
      newItem1.Tag = (object) controlItem;
      setClearAllMenu.Items.Add((object) newItem1);
      MenuItem newItem2 = new MenuItem();
      newItem2.Header = (object) "Set all";
      newItem2.Click += new RoutedEventHandler(this.ContextManuSetAll);
      newItem2.Tag = (object) controlItem;
      setClearAllMenu.Items.Add((object) newItem2);
      MenuItem newItem3 = new MenuItem();
      newItem3.Header = (object) "Remove from database";
      newItem3.Click += new RoutedEventHandler(this.ContextManuRemoveFromDatabase);
      newItem3.Tag = (object) controlItem;
      setClearAllMenu.Items.Add((object) newItem3);
      return setClearAllMenu;
    }

    private ContextMenu GetRemoveFromDbMenu(TreeViewItem controlItem)
    {
      ContextMenu removeFromDbMenu = new ContextMenu();
      MenuItem newItem = new MenuItem();
      newItem.Header = (object) "Remove from database";
      newItem.Click += new RoutedEventHandler(this.ContextManuRemoveFromDatabase);
      newItem.Tag = (object) controlItem;
      removeFromDbMenu.Items.Add((object) newItem);
      return removeFromDbMenu;
    }

    private void ContextManuSetAll(object sender, RoutedEventArgs e)
    {
      if (!(sender is MenuItem))
        return;
      MenuItem menuItem = (MenuItem) sender;
      if (menuItem.Tag != null && menuItem.Tag is TreeViewItem)
        this.SetAllToState((TreeViewItem) menuItem.Tag, true);
    }

    private void ContextManuClearAll(object sender, RoutedEventArgs e)
    {
      if (!(sender is MenuItem))
        return;
      MenuItem menuItem = (MenuItem) sender;
      if (menuItem.Tag != null && menuItem.Tag is TreeViewItem)
        this.SetAllToState((TreeViewItem) menuItem.Tag, false);
    }

    private void ContextManuRemoveFromDatabase(object sender, RoutedEventArgs e)
    {
      if (!(sender is MenuItem))
        return;
      MenuItem menuItem = (MenuItem) sender;
      if (menuItem.Tag != null && menuItem.Tag is TreeViewItem)
      {
        TreeViewItem tag = (TreeViewItem) menuItem.Tag;
        if (tag.Header is CheckBox)
          ((ItemsControl) tag.Parent).Items.Remove((object) tag);
        else if (tag.Parent != null && tag.Parent is TreeViewItem)
          ((ItemsControl) tag.Parent).Items.Remove((object) tag);
        else
          this.TreeViewRights.Items.Remove((object) tag);
      }
    }

    private void SetAllToState(TreeViewItem startItem, bool state)
    {
      if (startItem.Header is CheckBox)
      {
        CheckBox header = (CheckBox) startItem.Header;
        if (this.rightInfoList.IsEditByUserEnabled((FullRightInfo) header.Tag))
          header.IsChecked = new bool?(state);
      }
      if (!startItem.HasItems)
        return;
      foreach (object startItem1 in (IEnumerable) startItem.Items)
      {
        if (startItem1 is TreeViewItem)
          this.SetAllToState((TreeViewItem) startItem1, state);
      }
    }

    private void RemoveItems(TreeViewItem startItem)
    {
      if (startItem.Header is CheckBox)
        this.allCheckBoxes.Remove((CheckBox) startItem.Header);
      if (!startItem.HasItems)
        return;
      foreach (object startItem1 in (IEnumerable) startItem.Items)
      {
        if (startItem1 is TreeViewItem)
          this.RemoveItems((TreeViewItem) startItem1);
      }
    }

    private void PrepareUser()
    {
      if (this.editUserInfo == null)
        this.editUserInfo = new UserInfo();
      this.editUserInfo.Name = this.GetUserName();
      this.editUserInfo.Password = this.tbPassword.Password;
      this.editUserInfo.PersonalNumber = Convert.ToInt32(this.tbPersonalNumber.Text);
      this.editUserInfo.Permissions = new List<PermissionInfo>();
      this.editUserInfo.PhoneNumber = this.tbPhoneNumber.Text;
      this.editUserInfo.EmailAddress = this.tbEmailAddress.Text;
      this.editUserInfo.OnlyFinterprintLogin = this.OnlyFingerprintLogin.IsChecked.Value;
      this.editUserInfo.PNSource = (UserInfo.PN_Source) Enum.Parse(typeof (UserInfo.PN_Source), this.ComboBosPNSource.SelectedValue.ToString());
      this.editUserInfo.UserExtendedInfo = this.tbUserInfo.Text;
      if (!this.onlyRoleAllowed && this.ComboBoxRoles.SelectedIndex == 0)
      {
        this.editUserInfo.RoleUserId = 0;
        foreach (CheckBox allCheckBox in this.allCheckBoxes)
        {
          FullRightInfo tag = (FullRightInfo) allCheckBox.Tag;
          bool? isChecked = allCheckBox.IsChecked;
          bool flag1 = isChecked.Value;
          isChecked = this.CheckBoxRemoveDisabledRightsFromDatabase.IsChecked;
          bool flag2 = true;
          if (!(isChecked.GetValueOrDefault() == flag2 & isChecked.HasValue) || !allCheckBox.IsEnabled || flag1)
          {
            PermissionInfo permissionInfo1 = new PermissionInfo();
            permissionInfo1.PermissionName = ((TextBlock) allCheckBox.Content).Text;
            PermissionInfo permissionInfo2 = permissionInfo1;
            isChecked = allCheckBox.IsChecked;
            int num = isChecked.Value ? 1 : 0;
            permissionInfo2.PermissionValue = num != 0;
            this.editUserInfo.Permissions.Add(permissionInfo1);
          }
        }
      }
      else
        this.editUserInfo.RoleUserId = this.UserRolesList[this.GetRoleIndex()].Value;
    }

    private void ComboBoxRoles_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.Initializing)
        return;
      if (this.onlyRoleAllowed || this.ComboBoxRoles.SelectedIndex > 0)
      {
        this.LoadPermissions(UserManager.GetUser(this.UserRolesList[this.GetRoleIndex()].Value));
        foreach (UIElement allCheckBox in this.allCheckBoxes)
          allCheckBox.IsEnabled = false;
      }
      else if (this.EditMode)
        this.LoadPermissions(this.editUserInfo);
      else
        this.LoadPermissions((UserInfo) null);
    }

    private int GetRoleIndex()
    {
      return this.onlyRoleAllowed ? this.ComboBoxRoles.SelectedIndex : this.ComboBoxRoles.SelectedIndex - 1;
    }

    private void UserDataChanged(object sender, RoutedEventArgs e) => this.UserDataChanged();

    private void UserDataChanged(object sender, TextChangedEventArgs e) => this.UserDataChanged();

    private void tbPersonalNumber_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (!(this.tbPersonalNumber.Text != "") || int.TryParse(this.tbPersonalNumber.Text, out int _))
        return;
      this.tbPersonalNumber.Text = "";
    }

    private void CheckBoxRole_Checked(object sender, RoutedEventArgs e) => this.UserDataChanged();

    private void CheckBoxRole_Unchecked(object sender, RoutedEventArgs e)
    {
      this.tbPassword.Password = "";
      this.tbConfirmPassword.Password = "";
      this.UserDataChanged();
    }

    private string GetUserName()
    {
      string userName = this.tbUserName.Text.Trim();
      bool? isChecked = this.CheckBoxRole.IsChecked;
      bool flag = true;
      if (isChecked.GetValueOrDefault() == flag & isChecked.HasValue)
        userName = "* UserRole * " + userName;
      return userName;
    }

    private void UserDataChanged()
    {
      if (this.CheckRunning)
        return;
      this.CheckRunning = true;
      this.infoText.Length = 0;
      bool? isChecked = this.CheckBoxRole.IsChecked;
      bool flag1 = true;
      bool flag2 = isChecked.GetValueOrDefault() == flag1 & isChecked.HasValue;
      string userName = this.GetUserName();
      bool flag3 = !new Regex("^[一-龥]+$").IsMatch(this.tbUserName.Text.Trim()) ? this.tbUserName.Text.Trim().Length > 6 : this.tbUserName.Text.Trim().Length >= 2;
      bool flag4 = this.EditMode && this.editUserInfo.Name == userName;
      bool flag5 = UserManager.AllUsersUpper.ContainsKey(userName.ToUpper());
      bool flag6 = this.tbPassword.Password.Length == 0 && this.tbConfirmPassword.Password.Length == 0;
      bool flag7 = this.tbPassword.Password.Length < 5;
      bool flag8 = flag7 || UserManager.IsPasswordPoor(this.tbPassword.Password);
      bool flag9 = !flag8 && this.tbPassword.Password == this.tbConfirmPassword.Password;
      bool flag10 = flag9 || flag6 && this.EditMode | flag5;
      bool flag11 = ((!(this.EditMode & flag3) ? 0 : (flag4 ? 1 : (!flag5 ? 1 : 0))) & (flag10 ? 1 : 0)) != 0;
      bool flag12 = (!this.EditMode & flag3 && !flag5 && flag9 | flag2) | flag11;
      if (this.editUserInfo != null)
        this.infoText.AppendLine("Source user: " + this.editUserInfo.Name);
      if (!flag3)
        this.infoText.AppendLine("Name to short");
      else if (flag4)
        this.infoText.AppendLine("Name unchanged");
      else if (flag5)
        this.infoText.AppendLine("Name exists in database");
      else
        this.infoText.AppendLine("Name unused in database");
      if (flag2)
      {
        this.StackPanalPersonalInfow.Visibility = Visibility.Collapsed;
        this.tbPassword.Password = userName;
        this.tbConfirmPassword.Password = userName;
        this.tbPersonalNumber.Text = "0";
        this.ComboBoxRoles.SelectedIndex = 0;
      }
      else
      {
        if (flag6 & flag12)
          this.infoText.AppendLine("Password not changed");
        else if (!flag2)
        {
          if (flag9)
            this.infoText.AppendLine("Password ok");
          else if (flag7)
            this.infoText.AppendLine("Password to short");
          else if (flag8)
            this.infoText.AppendLine("Password poor");
          else
            this.infoText.AppendLine("Passwords different");
        }
        this.StackPanalPersonalInfow.Visibility = Visibility.Visible;
        if (DbBasis.PrimaryDB.BaseDbConnection.ConnectionInfo.DbType == MeterDbTypes.MSSQL)
        {
          this.StackPanalFingerPrintData.Visibility = Visibility.Visible;
        }
        else
        {
          this.StackPanalFingerPrintData.Visibility = Visibility.Collapsed;
          this.ShowLastFingerprintData.Visibility = Visibility.Collapsed;
        }
        if (this.tbPassword.Password.StartsWith("* UserRole * "))
        {
          this.tbPassword.Password = "";
          this.tbConfirmPassword.Password = "";
        }
      }
      this.tbPassword.IsEnabled = flag3;
      this.tbConfirmPassword.IsEnabled = !flag8;
      this.tbPersonalNumber.IsEnabled = flag3;
      this.ComboBoxRoles.IsEnabled = flag3;
      this.ButtonSave.IsEnabled = flag12;
      this.TextBoxInfo.Text = this.infoText.ToString();
      this.CheckRunning = false;
    }

    private void ButtonCopyRights_Click(object sender, RoutedEventArgs e)
    {
      List<KeyValuePair<string, bool>> data = new List<KeyValuePair<string, bool>>();
      foreach (CheckBox allCheckBox in this.allCheckBoxes)
      {
        KeyValuePair<string, bool> keyValuePair = new KeyValuePair<string, bool>(((FullRightInfo) allCheckBox.Tag).Right, allCheckBox.IsChecked.Value);
        data.Add(keyValuePair);
      }
      Clipboard.SetData(DataFormats.Serializable, (object) data);
      this.ButtonPastRights.IsEnabled = true;
      this.ButtonAddPastedRights.IsEnabled = true;
    }

    private void ButtonPastRights_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        List<KeyValuePair<string, bool>> data = (List<KeyValuePair<string, bool>>) Clipboard.GetData(DataFormats.Serializable);
        if (data == null)
        {
          this.ButtonPastRights.IsEnabled = false;
          this.ButtonAddPastedRights.IsEnabled = false;
        }
        else
        {
          foreach (CheckBox allCheckBox in this.allCheckBoxes)
          {
            CheckBox cb = allCheckBox;
            int index = data.FindIndex((Predicate<KeyValuePair<string, bool>>) (item => item.Key == ((FullRightInfo) cb.Tag).Right));
            if (index >= 0)
              cb.IsChecked = new bool?(data[index].Value);
          }
        }
      }
      catch
      {
      }
    }

    private void ButtonAddPastedRights_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        List<KeyValuePair<string, bool>> data = (List<KeyValuePair<string, bool>>) Clipboard.GetData(DataFormats.Serializable);
        if (data == null)
        {
          this.ButtonPastRights.IsEnabled = false;
          this.ButtonAddPastedRights.IsEnabled = false;
        }
        else
        {
          foreach (CheckBox allCheckBox in this.allCheckBoxes)
          {
            CheckBox cb = allCheckBox;
            int index = data.FindIndex((Predicate<KeyValuePair<string, bool>>) (item => item.Key == ((FullRightInfo) cb.Tag).Right));
            if (index >= 0 && data[index].Value)
              cb.IsChecked = new bool?(true);
          }
        }
      }
      catch
      {
      }
    }

    private void ShowLastFingerprintData_Click(object sender, RoutedEventArgs e)
    {
    }

    private void tbPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (!(this.tbPersonalNumber.Text != "") || int.TryParse(this.tbPersonalNumber.Text, out int _))
        return;
      this.tbPersonalNumber.Text = "";
    }

    private void ComboBosPNSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/StartupLib;component/edituserwindow.xaml", UriKind.Relative));
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
          this.CheckBoxRemoveDisabledRightsFromDatabase = (CheckBox) target;
          break;
        case 4:
          this.StackPanalButtons = (StackPanel) target;
          break;
        case 5:
          this.ButtonSave = (Button) target;
          this.ButtonSave.Click += new RoutedEventHandler(this.ButtonSave_Click);
          break;
        case 6:
          this.ShowLastFingerprintData = (Button) target;
          this.ShowLastFingerprintData.Click += new RoutedEventHandler(this.ShowLastFingerprintData_Click);
          break;
        case 7:
          this.ButtonCopyRights = (Button) target;
          this.ButtonCopyRights.Click += new RoutedEventHandler(this.ButtonCopyRights_Click);
          break;
        case 8:
          this.ButtonPastRights = (Button) target;
          this.ButtonPastRights.Click += new RoutedEventHandler(this.ButtonPastRights_Click);
          break;
        case 9:
          this.ButtonAddPastedRights = (Button) target;
          this.ButtonAddPastedRights.Click += new RoutedEventHandler(this.ButtonAddPastedRights_Click);
          break;
        case 10:
          this.LabelRightColoring = (Label) target;
          break;
        case 11:
          this.ListBoxRightsColoring = (ListBox) target;
          break;
        case 12:
          this.TextBoxInfo = (TextBox) target;
          break;
        case 13:
          this.StackPanalExtendedUserInfo = (StackPanel) target;
          break;
        case 14:
          this.tbUserInfo = (TextBox) target;
          break;
        case 15:
          this.CheckBoxRole = (CheckBox) target;
          this.CheckBoxRole.Checked += new RoutedEventHandler(this.CheckBoxRole_Checked);
          this.CheckBoxRole.Unchecked += new RoutedEventHandler(this.CheckBoxRole_Unchecked);
          break;
        case 16:
          this.lUserName = (Label) target;
          break;
        case 17:
          this.tbUserName = (TextBox) target;
          this.tbUserName.TextChanged += new TextChangedEventHandler(this.UserDataChanged);
          break;
        case 18:
          this.StackPanalPersonalInfow = (StackPanel) target;
          break;
        case 19:
          this.lPassword = (Label) target;
          break;
        case 20:
          this.tbPassword = (PasswordBox) target;
          this.tbPassword.PasswordChanged += new RoutedEventHandler(this.UserDataChanged);
          break;
        case 21:
          this.lConfirmPassword = (Label) target;
          break;
        case 22:
          this.tbConfirmPassword = (PasswordBox) target;
          this.tbConfirmPassword.PasswordChanged += new RoutedEventHandler(this.UserDataChanged);
          break;
        case 23:
          this.lPersonalNumber = (Label) target;
          break;
        case 24:
          this.tbPersonalNumber = (TextBox) target;
          this.tbPersonalNumber.TextChanged += new TextChangedEventHandler(this.tbPersonalNumber_TextChanged);
          break;
        case 25:
          this.tbPhoneNumber = (TextBox) target;
          this.tbPhoneNumber.TextChanged += new TextChangedEventHandler(this.tbPhoneNumber_TextChanged);
          break;
        case 26:
          this.tbEmailAddress = (TextBox) target;
          break;
        case 27:
          this.ComboBoxRoles = (ComboBox) target;
          this.ComboBoxRoles.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxRoles_SelectionChanged);
          break;
        case 28:
          this.StackPanalFingerPrintData = (StackPanel) target;
          break;
        case 29:
          this.lPNSource = (Label) target;
          break;
        case 30:
          this.ComboBosPNSource = (ComboBox) target;
          this.ComboBosPNSource.SelectionChanged += new SelectionChangedEventHandler(this.ComboBosPNSource_SelectionChanged);
          break;
        case 31:
          this.OnlyFingerprintLogin = (CheckBox) target;
          break;
        case 32:
          this.lPermissions = (Label) target;
          break;
        case 33:
          this.TreeViewRights = (TreeView) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}

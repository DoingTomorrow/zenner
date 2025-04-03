// Decompiled with JetBrains decompiler
// Type: ReadoutConfiguration.ProfilesList
// Assembly: ReadoutConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 1BD19DC4-A290-473A-8451-94ED3EF61361
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ReadoutConfiguration.dll

using CommonWPF;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace ReadoutConfiguration
{
  public partial class ProfilesList : Window, IComponentConnector
  {
    internal int startProfileID;
    internal int selectedProfileID = -1;
    private System.Windows.Point dragNewPoint;
    private System.Windows.Point dragStartPoint;
    private StringBuilder listInfo = new StringBuilder();
    private StringBuilder selectInfo = new StringBuilder();
    private Stopwatch stopwatch = new Stopwatch();
    internal System.Windows.Controls.Menu MenuMain;
    internal System.Windows.Controls.MenuItem MenuItemAdditionalTools;
    internal System.Windows.Controls.MenuItem MenuItemShowLostItems;
    internal System.Windows.Controls.MenuItem MenuItemCreateProfilesForMBusConverters;
    internal System.Windows.Controls.MenuItem MenuItemCopyTagsToParameters;
    internal System.Windows.Controls.MenuItem MenuItemCheckSettings;
    internal StartupLib.GmmCorporateControl gmmCorporateControl1;
    internal TextBlock TextBlockInfo;
    internal System.Windows.Controls.Button ButtonUseSelectedProfile;
    internal System.Windows.Controls.TextBox TextBoxDeviceFilter;
    internal System.Windows.Controls.TextBox TextBoxEquipmentFilter;
    internal System.Windows.Controls.TextBox TextBoxTypeFilter;
    internal System.Windows.Controls.ComboBox ComboBoxFilter;
    internal System.Windows.Controls.CheckBox CheckBoxUseFilter;
    internal System.Windows.Controls.Button ButtonAddFilter;
    internal System.Windows.Controls.Button ButtonDeleteFilter;
    internal System.Windows.Controls.Button ButtonChangeFilter;
    internal System.Windows.Controls.CheckBox CheckBoxShowParameters;
    internal System.Windows.Controls.CheckBox CheckBoxShowProfileNames;
    internal System.Windows.Controls.CheckBox CheckBoxShowItemIDs;
    internal System.Windows.Controls.DataGrid DataGridAllProfiles;
    internal System.Windows.Controls.MenuItem MenuItemCopySelectedProfile;
    internal System.Windows.Controls.MenuItem MenuItemPast;
    internal System.Windows.Controls.MenuItem MenuItemDeviceModelParameters;
    internal System.Windows.Controls.MenuItem MenuItemEquipmentModelParameters;
    internal System.Windows.Controls.MenuItem MenuItemProfileTypeParameters;
    internal System.Windows.Controls.MenuItem MenuItemProfileParameters;
    internal System.Windows.Controls.MenuItem MenuItemDeleteProfiles;
    internal System.Windows.Controls.MenuItem MenuItemDefineNewModel;
    internal System.Windows.Controls.MenuItem MenuItemDefineNewEquipment;
    internal System.Windows.Controls.MenuItem MenuItemDefineNewType;
    internal System.Windows.Controls.MenuItem MenuItemSetDifferentModel;
    internal System.Windows.Controls.MenuItem MenuItemSetDifferentEquipment;
    internal System.Windows.Controls.MenuItem MenuItemSetDifferentType;
    internal System.Windows.Controls.MenuItem MenuItemChangeDeviceModelID;
    internal System.Windows.Controls.MenuItem MenuItemChangeEquipmentModelID;
    internal System.Windows.Controls.MenuItem MenuItemChangeProfileTypeID;
    internal System.Windows.Controls.MenuItem MenuItemChangeSettingsID;
    internal System.Windows.Controls.MenuItem MenuItemCreateSettingsCloneAndAssingIt;
    internal System.Windows.Controls.MenuItem MenuItemShowAndEditSettings;
    internal System.Windows.Controls.MenuItem MenuItemEditCommonSettings;
    private bool _contentLoaded;

    public ProfilesList(int startProfileID)
    {
      this.startProfileID = startProfileID;
      this.selectedProfileID = startProfileID;
      this.dragNewPoint = new System.Windows.Point();
      this.dragStartPoint = this.dragNewPoint;
      this.InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e) => this.UpdateList();

    private void UpdateList()
    {
      this.listInfo.Clear();
      this.selectInfo.Clear();
      this.DataGridAllProfiles.ItemsSource = (IEnumerable) null;
      this.DataGridAllProfiles.Items.Clear();
      ConnectionProfileFilter profileFilter = (ConnectionProfileFilter) null;
      this.stopwatch.Reset();
      this.stopwatch.Start();
      if (this.CheckBoxUseFilter.IsChecked.Value && this.ComboBoxFilter.SelectedItem != null)
      {
        string key = this.ComboBoxFilter.SelectedItem.ToString();
        this.listInfo.Append("FilterName:" + key);
        profileFilter = ReadoutConfigFunctions.DbData.CachedProfileFilters[key];
      }
      ObservableCollection<ConnectionListRow> connectionProfilesList = ReadoutConfigFunctions.DbData.GetConnectionProfilesList(this.startProfileID, this.selectedProfileID, profileFilter);
      this.stopwatch.Stop();
      this.listInfo.Append(" FilterdProfiles:" + connectionProfilesList.Count.ToString());
      this.listInfo.Append(" ListGenerationTime:" + this.stopwatch.Elapsed.TotalMilliseconds.ToString() + "ms");
      string str1 = this.TextBoxDeviceFilter.Text.Trim();
      string str2 = this.TextBoxEquipmentFilter.Text.Trim();
      string str3 = this.TextBoxTypeFilter.Text.Trim();
      if (str1.Length > 0 || str2.Length > 0 || str3.Length > 0)
      {
        ObservableCollection<ConnectionListRow> observableCollection = new ObservableCollection<ConnectionListRow>();
        foreach (ConnectionListRow connectionListRow in (Collection<ConnectionListRow>) connectionProfilesList)
        {
          if (str1.Length > 0 && connectionListRow.DeviceModel.Contains(str1))
            observableCollection.Add(connectionListRow);
          else if (str2.Length > 0 && connectionListRow.EquipmentModel.Contains(str2))
            observableCollection.Add(connectionListRow);
          else if (str3.Length > 0 && connectionListRow.ProfileType.Contains(str3))
            observableCollection.Add(connectionListRow);
        }
        this.DataGridAllProfiles.ItemsSource = (IEnumerable) observableCollection;
        this.listInfo.Append(" DisplaydProfiles:" + observableCollection.Count.ToString());
      }
      else
        this.DataGridAllProfiles.ItemsSource = (IEnumerable) connectionProfilesList;
      for (int displayIndex = 0; displayIndex < this.DataGridAllProfiles.Columns.Count; ++displayIndex)
      {
        DataGridColumn dataGridColumn = this.DataGridAllProfiles.ColumnFromDisplayIndex(displayIndex);
        string str4 = dataGridColumn.Header.ToString();
        if (str4 == "Parameters")
        {
          bool? isChecked = this.CheckBoxShowParameters.IsChecked;
          dataGridColumn.Visibility = !isChecked.Value ? Visibility.Collapsed : Visibility.Visible;
        }
        if (str4 == "SettingsName")
        {
          bool? isChecked = this.CheckBoxShowProfileNames.IsChecked;
          dataGridColumn.Visibility = !isChecked.Value ? Visibility.Collapsed : Visibility.Visible;
        }
        if (str4 == "D_ID" || str4 == "E_ID" || str4 == "T_ID")
        {
          bool? isChecked = this.CheckBoxShowItemIDs.IsChecked;
          dataGridColumn.Visibility = !isChecked.Value ? Visibility.Collapsed : Visibility.Visible;
        }
      }
      this.DataGridAllProfiles.Items.SortDescriptions.Add(new SortDescription("DeviceModel", ListSortDirection.Ascending));
      this.DataGridAllProfiles.Items.SortDescriptions.Add(new SortDescription("ProfileType", ListSortDirection.Ascending));
      this.DataGridAllProfiles.Items.SortDescriptions.Add(new SortDescription("EquipmentModel", ListSortDirection.Ascending));
      if (ReadoutConfigFunctions.DbData.CachedProfileFilters != null)
        this.ComboBoxFilter.ItemsSource = (IEnumerable) ReadoutConfigFunctions.DbData.CachedProfileFilters.Keys.ToList<string>();
      this.DataGridAllProfiles.UpdateLayout();
      for (int index = 0; index < this.DataGridAllProfiles.Items.Count; ++index)
      {
        ConnectionListRow connectionListRow = (ConnectionListRow) this.DataGridAllProfiles.Items[index];
        if (connectionListRow.ID == this.startProfileID)
        {
          this.DataGridAllProfiles.SelectedIndex = index;
          this.DataGridAllProfiles.ScrollIntoView((object) connectionListRow);
          break;
        }
      }
      this.ShowInfo();
    }

    private void DataGridAllProfiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      this.selectInfo.Clear();
      if (this.DataGridAllProfiles.SelectedItems.Count > 0)
      {
        this.DataGridAllProfiles.ContextMenu.IsEnabled = true;
        this.ButtonUseSelectedProfile.IsEnabled = true;
        this.MenuItemDeleteProfiles.IsEnabled = true;
        this.MenuItemCreateSettingsCloneAndAssingIt.IsEnabled = true;
        this.MenuItemShowAndEditSettings.IsEnabled = true;
        this.MenuItemEditCommonSettings.IsEnabled = false;
        this.MenuItemDefineNewModel.IsEnabled = true;
        this.MenuItemDefineNewEquipment.IsEnabled = true;
        this.MenuItemDefineNewType.IsEnabled = true;
        if (this.DataGridAllProfiles.SelectedItems.Count > 1)
        {
          this.MenuItemCopySelectedProfile.IsEnabled = false;
          ConnectionListRow selectedItem1 = (ConnectionListRow) this.DataGridAllProfiles.SelectedItems[0];
          for (int index = 0; index < this.DataGridAllProfiles.SelectedItems.Count; ++index)
          {
            ConnectionListRow selectedItem2 = (ConnectionListRow) this.DataGridAllProfiles.SelectedItems[index];
            if (selectedItem2.SettingsId != selectedItem1.SettingsId)
            {
              this.MenuItemCreateSettingsCloneAndAssingIt.IsEnabled = false;
              this.MenuItemShowAndEditSettings.IsEnabled = false;
              this.MenuItemEditCommonSettings.IsEnabled = true;
            }
            if (selectedItem2.DeviceModelID != selectedItem1.DeviceModelID)
              this.MenuItemDefineNewModel.IsEnabled = false;
            if (selectedItem2.EquipmentModelID != selectedItem1.EquipmentModelID)
              this.MenuItemDefineNewEquipment.IsEnabled = false;
            if (selectedItem2.ProfileTypeID != selectedItem1.ProfileTypeID)
              this.MenuItemDefineNewType.IsEnabled = false;
          }
        }
        else
          this.MenuItemCopySelectedProfile.IsEnabled = true;
      }
      else
        this.DataGridAllProfiles.ContextMenu.IsEnabled = false;
      this.ShowInfo();
      this.SetNewProfileID();
    }

    private void ShowInfo()
    {
      this.TextBlockInfo.Text = this.listInfo.ToString() + " / " + this.selectInfo.ToString();
    }

    private void DataGridAllProfiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (!this.SetNewProfileID())
        return;
      this.DialogResult = new bool?(true);
      this.Close();
    }

    private void ButtonUseSelectedProfile_Click(object sender, RoutedEventArgs e)
    {
      if (!this.SetNewProfileID())
        return;
      this.DialogResult = new bool?(true);
      this.Close();
    }

    private void ButtonCreateClonedSettings_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.DataGridAllProfiles.SelectedItems == null || this.DataGridAllProfiles.SelectedItems.Count < 1)
          return;
        List<int> profileIdList = new List<int>();
        foreach (ConnectionListRow selectedItem in (IEnumerable) this.DataGridAllProfiles.SelectedItems)
          profileIdList.Add(selectedItem.ID);
        ReadoutConfigFunctions.DbData.CloneSettings(profileIdList);
        this.UpdateList();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Clone profiles error");
      }
    }

    private void MenuItemShowAndEditSettings_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (!this.SetNewProfileID())
          return;
        SettingsList settingsList = new SettingsList(ReadoutConfigFunctions.DbData.GetPartiallyConnectionProfiles().Find((Predicate<ConnectionProfile>) (x => x.ConnectionProfileID == this.selectedProfileID)).ConnectionSettings);
        settingsList.Owner = (Window) this;
        settingsList.ShowDialog();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Work profile settings error");
      }
    }

    private void MenuItemDefineNewModel_Click(object sender, RoutedEventArgs e)
    {
      if (this.DataGridAllProfiles.SelectedItems == null || this.DataGridAllProfiles.SelectedItems.Count < 1)
        return;
      try
      {
        string oneValue = EnterOneValue.GetOneValue("Define new DeviceModel", "Define new DeviceModel" + Environment.NewLine + "Type in the name of a new DeviceModel." + Environment.NewLine + Environment.NewLine + "All selected profiles will be copied." + Environment.NewLine + "Inside the copy the DeviceModel will be changed" + Environment.NewLine + "to the new created DeviceModel.");
        if (string.IsNullOrEmpty(oneValue))
          return;
        List<int> connectionProfileIDsList = new List<int>();
        foreach (ConnectionListRow selectedItem in (IEnumerable) this.DataGridAllProfiles.SelectedItems)
          connectionProfileIDsList.Add(selectedItem.ID);
        int deviceGroupId = ((ConnectionListRow) this.DataGridAllProfiles.SelectedItems[0]).DeviceGroupID;
        ReadoutConfigFunctions.DbData.CreateCopiedProfiles(connectionProfileIDsList, oneValue, deviceGroupId, ConnectionItemTypes.DeviceModel);
        this.UpdateList();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error by defining new DeviceModel");
      }
    }

    private void MenuItemDefineNewEquipment_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        string oneValue = EnterOneValue.GetOneValue("Define new EquipmentType", "Define new EquipmentType" + Environment.NewLine + "Type in the name of a new EquipmentType." + Environment.NewLine + Environment.NewLine + "All selected profiles will be copied." + Environment.NewLine + "Inside the copy the EquipmentType will be changed" + Environment.NewLine + "to the new created EquipmentType.");
        if (string.IsNullOrEmpty(oneValue))
          return;
        List<int> connectionProfileIDsList = new List<int>();
        foreach (ConnectionListRow selectedItem in (IEnumerable) this.DataGridAllProfiles.SelectedItems)
          connectionProfileIDsList.Add(selectedItem.ID);
        int equipmentGroupId = ((ConnectionListRow) this.DataGridAllProfiles.SelectedItems[0]).EquipmentGroupID;
        ReadoutConfigFunctions.DbData.CreateCopiedProfiles(connectionProfileIDsList, oneValue, equipmentGroupId, ConnectionItemTypes.EquipmentModel);
        this.UpdateList();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error by defining new EquipmentType");
      }
    }

    private void MenuItemDefineNewType_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        string oneValue = EnterOneValue.GetOneValue("Define new ProfileType", "Define new ProfileType" + Environment.NewLine + "Type in the name of a new ProfileType." + Environment.NewLine + Environment.NewLine + "All selected profiles will be copied." + Environment.NewLine + "Inside the copy the ProfileType will be changed" + Environment.NewLine + "to the new created ProfileType.");
        if (string.IsNullOrEmpty(oneValue))
          return;
        List<int> connectionProfileIDsList = new List<int>();
        foreach (ConnectionListRow selectedItem in (IEnumerable) this.DataGridAllProfiles.SelectedItems)
          connectionProfileIDsList.Add(selectedItem.ID);
        int profileTypeGroupId = ((ConnectionListRow) this.DataGridAllProfiles.SelectedItems[0]).ProfileTypeGroupID;
        ReadoutConfigFunctions.DbData.CreateCopiedProfiles(connectionProfileIDsList, oneValue, profileTypeGroupId, ConnectionItemTypes.ProfileType);
        this.UpdateList();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error by defining new ProfileType");
      }
    }

    private bool SetNewProfileID()
    {
      System.Windows.Controls.DataGrid dataGridAllProfiles = this.DataGridAllProfiles;
      if (dataGridAllProfiles != null && dataGridAllProfiles.SelectedItems != null && dataGridAllProfiles.SelectedItems.Count >= 1)
      {
        this.selectedProfileID = this.GetProfileID(dataGridAllProfiles.SelectedItem);
        ReadoutConfigFunctions.DbData.MarkSelectedProfile(this.selectedProfileID);
        return this.selectedProfileID >= 0;
      }
      this.selectedProfileID = -1;
      return false;
    }

    private int GetProfileID(object dataGridRowItem) => ((ConnectionListRow) dataGridRowItem).ID;

    private void MenuItemDeleteProfiles_Click(object sender, RoutedEventArgs e)
    {
      if (this.DataGridAllProfiles.SelectedItems == null || this.DataGridAllProfiles.SelectedItems.Count < 1)
        return;
      List<ConnectionProfile> source = new List<ConnectionProfile>();
      foreach (object selectedItem in (IEnumerable) this.DataGridAllProfiles.SelectedItems)
      {
        int profileId = this.GetProfileID(selectedItem);
        ConnectionProfile connectionProfile = ReadoutConfigFunctions.DbData.GetPartiallyConnectionProfiles().Find((Predicate<ConnectionProfile>) (x => x.ConnectionProfileID == profileId));
        source.Add(connectionProfile);
      }
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Would you like to delete following profiles?");
      stringBuilder.AppendLine("----------------------------------------------------------");
      stringBuilder.AppendLine("");
      foreach (ConnectionProfile connectionProfile in source)
      {
        stringBuilder.Append(connectionProfile.ConnectionProfileID.ToString("d6"));
        stringBuilder.Append("; " + connectionProfile.DeviceModel.Name);
        stringBuilder.Append("; " + connectionProfile.EquipmentModel.Name);
        stringBuilder.Append("; " + connectionProfile.ProfileType.Name);
        stringBuilder.AppendLine();
      }
      if (GMM_MessageBox.ShowMessage("Profiles list command", stringBuilder.ToString(), MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
        return;
      int num = (int) GMM_MessageBox.ShowMessage("Profiles list command", ReadoutConfigFunctions.DbData.DeleteProfiles(source.Select<ConnectionProfile, int>((Func<ConnectionProfile, int>) (item => item.ConnectionProfileID)).ToList<int>()));
    }

    private void MenuItemShowLostItems_Click(object sender, RoutedEventArgs e)
    {
      new LostItems().ShowDialog();
    }

    private void MenuItemEditCommonSettings_Click(object sender, RoutedEventArgs e)
    {
      if (this.DataGridAllProfiles.SelectedItems == null || this.DataGridAllProfiles.SelectedItems.Count < 1)
        return;
      SortedList<int, ConnectionProfile> sortedList = new SortedList<int, ConnectionProfile>();
      foreach (object selectedItem in (IEnumerable) this.DataGridAllProfiles.SelectedItems)
      {
        int profileId = this.GetProfileID(selectedItem);
        ConnectionProfile connectionProfile = ReadoutConfigFunctions.DbData.GetPartiallyConnectionProfiles().Find((Predicate<ConnectionProfile>) (x => x.ConnectionProfileID == profileId));
        sortedList.Add(profileId, connectionProfile);
      }
      List<int> settingIds;
      List<CommonEditValues> settingsEditValues = ReadoutConfigFunctions.DbData.GetCommonSettingsEditValues(sortedList.Keys.ToList<int>(), out settingIds);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Would you like common edit for following " + settingIds.Count.ToString() + " settings?");
      stringBuilder.AppendLine("----------------------------------------------------------");
      stringBuilder.AppendLine("");
      foreach (int key in settingIds)
      {
        ConnectionSettings connectionSettings = ReadoutConfigFunctions.DbData.CachedConnectionSettingsById[key];
        stringBuilder.AppendLine(key.ToString() + "; " + connectionSettings.Name);
      }
      if (GMM_MessageBox.ShowMessage("Profiles list command", stringBuilder.ToString(), MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
        return;
      CommonParameterEditor commonParameterEditor = new CommonParameterEditor(settingsEditValues, settingIds.Count);
      commonParameterEditor.Owner = (Window) this;
      commonParameterEditor.ShowDialog();
    }

    private void MenuItemChangeDeviceModelID_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        string oneValue = EnterOneValue.GetOneValue("Change DeviceModelID", "Change DeviceModelID" + Environment.NewLine + "Type in the DeviceModelID" + Environment.NewLine + Environment.NewLine + "It has to be a existing DeviceModelID !!!" + Environment.NewLine + "All selected profiles will be changed.");
        if (string.IsNullOrEmpty(oneValue))
          return;
        List<int> connectionProfileIDsList = new List<int>();
        foreach (ConnectionListRow selectedItem in (IEnumerable) this.DataGridAllProfiles.SelectedItems)
          connectionProfileIDsList.Add(selectedItem.ID);
        ReadoutConfigFunctions.DbData.ChangeIDs(connectionProfileIDsList, oneValue, ConnectionItemTypes.DeviceModel);
        this.UpdateList();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error by changing DeviceModelID");
      }
    }

    private void MenuItemChangeEquipmentModelID_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        string oneValue = EnterOneValue.GetOneValue("Change EquipmentModelID", "Change EquipmentModelID" + Environment.NewLine + "Type in the EquipmentModelID" + Environment.NewLine + Environment.NewLine + "It has to be a existing EquipmentModelID !!!" + Environment.NewLine + "All selected profiles will be changed.");
        if (string.IsNullOrEmpty(oneValue))
          return;
        List<int> connectionProfileIDsList = new List<int>();
        foreach (ConnectionListRow selectedItem in (IEnumerable) this.DataGridAllProfiles.SelectedItems)
          connectionProfileIDsList.Add(selectedItem.ID);
        ReadoutConfigFunctions.DbData.ChangeIDs(connectionProfileIDsList, oneValue, ConnectionItemTypes.EquipmentModel);
        this.UpdateList();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error by changing EquipmentModelID");
      }
    }

    private void MenuItemChangeProfileTypeID_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        string oneValue = EnterOneValue.GetOneValue("Change ProfileTypeID", "Change ProfileTypeID" + Environment.NewLine + "Type in the ProfileTypeID" + Environment.NewLine + Environment.NewLine + "It has to be a existing ProfileTypeID !!!" + Environment.NewLine + "All selected profiles will be changed.");
        if (string.IsNullOrEmpty(oneValue))
          return;
        List<int> connectionProfileIDsList = new List<int>();
        foreach (ConnectionListRow selectedItem in (IEnumerable) this.DataGridAllProfiles.SelectedItems)
          connectionProfileIDsList.Add(selectedItem.ID);
        ReadoutConfigFunctions.DbData.ChangeIDs(connectionProfileIDsList, oneValue, ConnectionItemTypes.ProfileType);
        this.UpdateList();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error by changing ProfileTypeID");
      }
    }

    private void MenuItemChangeSettingsID_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        string oneValue = EnterOneValue.GetOneValue("Change ConnectionSettingsID", "Change ConnectionSettingsID" + Environment.NewLine + "Type in the ConnectionSettingsID" + Environment.NewLine + Environment.NewLine + "It has to be a existing ConnectionSettingsID !!!" + Environment.NewLine + "All selected profiles will be changed.");
        if (string.IsNullOrEmpty(oneValue))
          return;
        List<int> connectionProfileIDsList = new List<int>();
        foreach (ConnectionListRow selectedItem in (IEnumerable) this.DataGridAllProfiles.SelectedItems)
          connectionProfileIDsList.Add(selectedItem.ID);
        ReadoutConfigFunctions.DbData.ChangeIDs(connectionProfileIDsList, oneValue, ~ConnectionItemTypes.EquipmentModel);
        this.UpdateList();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error by changing ProfileTypeID");
      }
    }

    private void MenuItemSetDifferentModel_Click(object sender, RoutedEventArgs e)
    {
      if (this.DataGridAllProfiles.SelectedItems == null)
        return;
      try
      {
        if (System.Windows.MessageBox.Show("Create new profile" + Environment.NewLine + "by using the existing DeviceModelID that is past from clipboard:" + ReadoutConfigFunctions.DbData.GetProfileFromClipboard().DeviceModel.DeviceModelID.ToString() + Environment.NewLine + Environment.NewLine + "All selected profiles will be copied." + Environment.NewLine + "Inside the copy the existing DeviceModelID will be set" + Environment.NewLine + Environment.NewLine + "Are you sure to create new profiles?", "Copy profiles", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
          return;
        ReadoutConfigFunctions.DbData.CreateCopiedProfiles(this.GetListOfSelectedProfileIDs(), ConnectionItemTypes.DeviceModel);
        this.UpdateList();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error by defining new profiles");
      }
    }

    private List<int> GetListOfSelectedProfileIDs()
    {
      List<int> selectedProfileIds = new List<int>();
      foreach (ConnectionListRow selectedItem in (IEnumerable) this.DataGridAllProfiles.SelectedItems)
        selectedProfileIds.Add(selectedItem.ID);
      return selectedProfileIds;
    }

    private void MenuItemSetDifferentEquipment_Click(object sender, RoutedEventArgs e)
    {
      if (this.DataGridAllProfiles.SelectedItems == null)
        return;
      try
      {
        if (System.Windows.MessageBox.Show("Create new profile" + Environment.NewLine + "by using the existing EquipmentModelID that is past from clipboard:" + ReadoutConfigFunctions.DbData.GetProfileFromClipboard().EquipmentModel.EquipmentModelID.ToString() + Environment.NewLine + Environment.NewLine + "All selected profiles will be copied." + Environment.NewLine + "Inside the copy the existing EquipmentModelID will be set" + Environment.NewLine + Environment.NewLine + "Are you sure to create new profiles?", "Copy profiles", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
          return;
        ReadoutConfigFunctions.DbData.CreateCopiedProfiles(this.GetListOfSelectedProfileIDs(), ConnectionItemTypes.EquipmentModel);
        this.UpdateList();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error by defining new profiles");
      }
    }

    private void MenuItemSetDifferentType_Click(object sender, RoutedEventArgs e)
    {
      if (this.DataGridAllProfiles.SelectedItems == null)
        return;
      try
      {
        if (System.Windows.MessageBox.Show("Create new profile" + Environment.NewLine + "by using the existing ProfileTypeID that is past from clipboard:" + ReadoutConfigFunctions.DbData.GetProfileFromClipboard().ProfileType.ProfileTypeID.ToString() + Environment.NewLine + Environment.NewLine + "All selected profiles will be copied." + Environment.NewLine + "Inside the copy the existing ProfileTypeID will be set" + Environment.NewLine + Environment.NewLine + "Are you sure to create new profiles?", "Copy profiles", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
          return;
        ReadoutConfigFunctions.DbData.CreateCopiedProfiles(this.GetListOfSelectedProfileIDs(), ConnectionItemTypes.ProfileType);
        this.UpdateList();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error by defining new profiles");
      }
    }

    private void TextBoxFilter_LostFocus(object sender, RoutedEventArgs e) => this.UpdateList();

    private void TextBoxFilter_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
      if (e.Key != Key.Return)
        return;
      e.Handled = true;
      this.UpdateList();
    }

    private void MenuItemCopyTagsToParameters_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        int parameters = ReadoutConfigFunctions.DbData.CopyTagsToParameters();
        this.UpdateList();
        int num = (int) System.Windows.MessageBox.Show(parameters.ToString() + " tags copied");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void MenuItemCopySelectedProfile_Click(object sender, RoutedEventArgs e)
    {
      if (this.DataGridAllProfiles.SelectedItems == null || this.DataGridAllProfiles.SelectedItems.Count != 1)
        return;
      ReadoutConfigFunctions.DbData.CopyProfileToClipboard(((ConnectionListRow) this.DataGridAllProfiles.SelectedItems[0]).ID);
    }

    private void CallPastFunction(ProfilesList.PastFunction theFunction)
    {
      try
      {
        List<int> selectedIds = this.GetSelectedIds();
        if (selectedIds == null)
          return;
        string messageBoxText = theFunction(selectedIds);
        this.UpdateList();
        int num = (int) System.Windows.MessageBox.Show(messageBoxText);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void MenuItemDeviceModelParameters_Click(object sender, RoutedEventArgs e)
    {
      this.CallPastFunction(new ProfilesList.PastFunction(ReadoutConfigFunctions.DbData.PastDeviceParametersFromClipboard));
    }

    private void MenuItemEquipmentModelParameters_Click(object sender, RoutedEventArgs e)
    {
      this.CallPastFunction(new ProfilesList.PastFunction(ReadoutConfigFunctions.DbData.PastEquipmentParametersFromClipboard));
    }

    private void MenuItemProfileTypeParameters_Click(object sender, RoutedEventArgs e)
    {
      this.CallPastFunction(new ProfilesList.PastFunction(ReadoutConfigFunctions.DbData.PastTypeParametersFromClipboard));
    }

    private void MenuItemProfileParameters_Click(object sender, RoutedEventArgs e)
    {
      this.CallPastFunction(new ProfilesList.PastFunction(ReadoutConfigFunctions.DbData.PastProfileParametersFromClipboard));
    }

    private List<int> GetSelectedIds()
    {
      if (this.DataGridAllProfiles.SelectedItems == null)
        return (List<int>) null;
      List<int> selectedIds = new List<int>();
      foreach (ConnectionListRow selectedItem in (IEnumerable) this.DataGridAllProfiles.SelectedItems)
        selectedIds.Add(selectedItem.ID);
      return selectedIds;
    }

    private void ButtonAddFilter_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        string baseName = (string) null;
        if (this.ComboBoxFilter.SelectedItem != null)
          baseName = (string) this.ComboBoxFilter.SelectedItem;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Define new filter.");
        string oneValue;
        bool flag;
        if (baseName == null)
        {
          stringBuilder.AppendLine("Please type in a new filter name.");
          oneValue = EnterOneValue.GetOneValue("Define new filter", stringBuilder.ToString());
          if (string.IsNullOrEmpty(oneValue))
            return;
          if (ReadoutConfigFunctions.DbData.CachedProfileFilters.ContainsKey(oneValue))
          {
            int num = (int) System.Windows.MessageBox.Show("The filter exists." + Environment.NewLine + "Please use change function !!!");
            return;
          }
          flag = ParameterListEditor.EditFilter(oneValue);
        }
        else
        {
          stringBuilder.AppendLine("Please type in a new filter name.");
          stringBuilder.AppendLine("The new filter will be created as copy of");
          stringBuilder.AppendLine("existing filter: " + baseName);
          oneValue = EnterOneValue.GetOneValue("Define new filter", stringBuilder.ToString());
          if (string.IsNullOrEmpty(oneValue))
            return;
          if (ReadoutConfigFunctions.DbData.CachedProfileFilters.ContainsKey(oneValue))
          {
            int num = (int) System.Windows.MessageBox.Show("The filter exists." + Environment.NewLine + "Please use change function !!!");
            return;
          }
          flag = ParameterListEditor.EditFilter(oneValue, baseName);
        }
        if (!flag)
          return;
        ReadoutConfigFunctions.DbData.LoadProfileFilters();
        this.ComboBoxFilter.ItemsSource = (IEnumerable) ReadoutConfigFunctions.DbData.CachedProfileFilters.Keys.ToList<string>();
        if (ReadoutConfigFunctions.DbData.CachedProfileFilters.ContainsKey(oneValue))
          this.ComboBoxFilter.SelectedItem = (object) oneValue;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonChangeFilter_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.ComboBoxFilter.SelectedItem == null)
          return;
        string selectedItem = (string) this.ComboBoxFilter.SelectedItem;
        if (!ParameterListEditor.EditFilter(selectedItem))
          return;
        ReadoutConfigFunctions.DbData.LoadProfileFilters();
        this.ComboBoxFilter.ItemsSource = (IEnumerable) ReadoutConfigFunctions.DbData.CachedProfileFilters.Keys.ToList<string>();
        if (ReadoutConfigFunctions.DbData.CachedProfileFilters.ContainsKey(selectedItem))
          this.ComboBoxFilter.SelectedItem = (object) selectedItem;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonDeleteFilter_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.ComboBoxFilter.SelectedItem == null || System.Windows.MessageBox.Show("Would you really delete the filter: '" + (string) this.ComboBoxFilter.SelectedItem + "' ?", "Delete filter", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
          ;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ComboBoxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.ComboBoxFilter.SelectedItem != null)
      {
        this.ButtonDeleteFilter.IsEnabled = true;
        this.ButtonChangeFilter.IsEnabled = true;
        this.CheckBoxUseFilter.IsEnabled = true;
        if (!this.CheckBoxUseFilter.IsChecked.Value)
          return;
        this.UpdateList();
      }
      else
      {
        this.ButtonDeleteFilter.IsEnabled = false;
        this.ButtonChangeFilter.IsEnabled = false;
        this.CheckBoxUseFilter.IsEnabled = false;
      }
    }

    private void CheckBoxUseFilter_Checked(object sender, RoutedEventArgs e) => this.UpdateList();

    private void CheckBoxUseFilter_Unchecked(object sender, RoutedEventArgs e) => this.UpdateList();

    private void MenuItemCreateProfilesForMBusConverters_Click(object sender, RoutedEventArgs e)
    {
      if (this.DataGridAllProfiles.SelectedItem == null)
        return;
      ReadoutConfigFunctions.DbData.CreateProfilesForMBusConverters(((ConnectionListRow) this.DataGridAllProfiles.SelectedItem).ProfileTypeID);
      this.UpdateList();
    }

    private void CheckBoxChanged(object sender, RoutedEventArgs e) => this.UpdateList();

    private void MenuItemCheckSettings_Click(object sender, RoutedEventArgs e)
    {
      ReadoutConfigFunctions.DbData.CheckSettings();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      System.Windows.Application.LoadComponent((object) this, new Uri("/ReadoutConfiguration;component/profileslist.xaml", UriKind.Relative));
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
          this.MenuMain = (System.Windows.Controls.Menu) target;
          break;
        case 3:
          this.MenuItemAdditionalTools = (System.Windows.Controls.MenuItem) target;
          break;
        case 4:
          this.MenuItemShowLostItems = (System.Windows.Controls.MenuItem) target;
          this.MenuItemShowLostItems.Click += new RoutedEventHandler(this.MenuItemShowLostItems_Click);
          break;
        case 5:
          this.MenuItemCreateProfilesForMBusConverters = (System.Windows.Controls.MenuItem) target;
          this.MenuItemCreateProfilesForMBusConverters.Click += new RoutedEventHandler(this.MenuItemCreateProfilesForMBusConverters_Click);
          break;
        case 6:
          this.MenuItemCopyTagsToParameters = (System.Windows.Controls.MenuItem) target;
          this.MenuItemCopyTagsToParameters.Click += new RoutedEventHandler(this.MenuItemCopyTagsToParameters_Click);
          break;
        case 7:
          this.MenuItemCheckSettings = (System.Windows.Controls.MenuItem) target;
          this.MenuItemCheckSettings.Click += new RoutedEventHandler(this.MenuItemCheckSettings_Click);
          break;
        case 8:
          this.gmmCorporateControl1 = (StartupLib.GmmCorporateControl) target;
          break;
        case 9:
          this.TextBlockInfo = (TextBlock) target;
          break;
        case 10:
          this.ButtonUseSelectedProfile = (System.Windows.Controls.Button) target;
          this.ButtonUseSelectedProfile.Click += new RoutedEventHandler(this.ButtonUseSelectedProfile_Click);
          break;
        case 11:
          this.TextBoxDeviceFilter = (System.Windows.Controls.TextBox) target;
          this.TextBoxDeviceFilter.LostFocus += new RoutedEventHandler(this.TextBoxFilter_LostFocus);
          this.TextBoxDeviceFilter.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.TextBoxFilter_PreviewKeyDown);
          break;
        case 12:
          this.TextBoxEquipmentFilter = (System.Windows.Controls.TextBox) target;
          this.TextBoxEquipmentFilter.LostFocus += new RoutedEventHandler(this.TextBoxFilter_LostFocus);
          this.TextBoxEquipmentFilter.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.TextBoxFilter_PreviewKeyDown);
          break;
        case 13:
          this.TextBoxTypeFilter = (System.Windows.Controls.TextBox) target;
          this.TextBoxTypeFilter.LostFocus += new RoutedEventHandler(this.TextBoxFilter_LostFocus);
          this.TextBoxTypeFilter.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.TextBoxFilter_PreviewKeyDown);
          break;
        case 14:
          this.ComboBoxFilter = (System.Windows.Controls.ComboBox) target;
          this.ComboBoxFilter.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxFilter_SelectionChanged);
          break;
        case 15:
          this.CheckBoxUseFilter = (System.Windows.Controls.CheckBox) target;
          this.CheckBoxUseFilter.Checked += new RoutedEventHandler(this.CheckBoxUseFilter_Checked);
          this.CheckBoxUseFilter.Unchecked += new RoutedEventHandler(this.CheckBoxUseFilter_Unchecked);
          break;
        case 16:
          this.ButtonAddFilter = (System.Windows.Controls.Button) target;
          this.ButtonAddFilter.Click += new RoutedEventHandler(this.ButtonAddFilter_Click);
          break;
        case 17:
          this.ButtonDeleteFilter = (System.Windows.Controls.Button) target;
          this.ButtonDeleteFilter.Click += new RoutedEventHandler(this.ButtonDeleteFilter_Click);
          break;
        case 18:
          this.ButtonChangeFilter = (System.Windows.Controls.Button) target;
          this.ButtonChangeFilter.Click += new RoutedEventHandler(this.ButtonChangeFilter_Click);
          break;
        case 19:
          this.CheckBoxShowParameters = (System.Windows.Controls.CheckBox) target;
          this.CheckBoxShowParameters.Checked += new RoutedEventHandler(this.CheckBoxChanged);
          this.CheckBoxShowParameters.Unchecked += new RoutedEventHandler(this.CheckBoxChanged);
          break;
        case 20:
          this.CheckBoxShowProfileNames = (System.Windows.Controls.CheckBox) target;
          this.CheckBoxShowProfileNames.Checked += new RoutedEventHandler(this.CheckBoxChanged);
          this.CheckBoxShowProfileNames.Unchecked += new RoutedEventHandler(this.CheckBoxChanged);
          break;
        case 21:
          this.CheckBoxShowItemIDs = (System.Windows.Controls.CheckBox) target;
          this.CheckBoxShowItemIDs.Checked += new RoutedEventHandler(this.CheckBoxChanged);
          this.CheckBoxShowItemIDs.Unchecked += new RoutedEventHandler(this.CheckBoxChanged);
          break;
        case 22:
          this.DataGridAllProfiles = (System.Windows.Controls.DataGrid) target;
          this.DataGridAllProfiles.SelectionChanged += new SelectionChangedEventHandler(this.DataGridAllProfiles_SelectionChanged);
          this.DataGridAllProfiles.MouseDoubleClick += new MouseButtonEventHandler(this.DataGridAllProfiles_MouseDoubleClick);
          break;
        case 23:
          this.MenuItemCopySelectedProfile = (System.Windows.Controls.MenuItem) target;
          this.MenuItemCopySelectedProfile.Click += new RoutedEventHandler(this.MenuItemCopySelectedProfile_Click);
          break;
        case 24:
          this.MenuItemPast = (System.Windows.Controls.MenuItem) target;
          break;
        case 25:
          this.MenuItemDeviceModelParameters = (System.Windows.Controls.MenuItem) target;
          this.MenuItemDeviceModelParameters.Click += new RoutedEventHandler(this.MenuItemDeviceModelParameters_Click);
          break;
        case 26:
          this.MenuItemEquipmentModelParameters = (System.Windows.Controls.MenuItem) target;
          this.MenuItemEquipmentModelParameters.Click += new RoutedEventHandler(this.MenuItemEquipmentModelParameters_Click);
          break;
        case 27:
          this.MenuItemProfileTypeParameters = (System.Windows.Controls.MenuItem) target;
          this.MenuItemProfileTypeParameters.Click += new RoutedEventHandler(this.MenuItemProfileTypeParameters_Click);
          break;
        case 28:
          this.MenuItemProfileParameters = (System.Windows.Controls.MenuItem) target;
          this.MenuItemProfileParameters.Click += new RoutedEventHandler(this.MenuItemProfileParameters_Click);
          break;
        case 29:
          this.MenuItemDeleteProfiles = (System.Windows.Controls.MenuItem) target;
          this.MenuItemDeleteProfiles.Click += new RoutedEventHandler(this.MenuItemDeleteProfiles_Click);
          break;
        case 30:
          this.MenuItemDefineNewModel = (System.Windows.Controls.MenuItem) target;
          this.MenuItemDefineNewModel.Click += new RoutedEventHandler(this.MenuItemDefineNewModel_Click);
          break;
        case 31:
          this.MenuItemDefineNewEquipment = (System.Windows.Controls.MenuItem) target;
          this.MenuItemDefineNewEquipment.Click += new RoutedEventHandler(this.MenuItemDefineNewEquipment_Click);
          break;
        case 32:
          this.MenuItemDefineNewType = (System.Windows.Controls.MenuItem) target;
          this.MenuItemDefineNewType.Click += new RoutedEventHandler(this.MenuItemDefineNewType_Click);
          break;
        case 33:
          this.MenuItemSetDifferentModel = (System.Windows.Controls.MenuItem) target;
          this.MenuItemSetDifferentModel.Click += new RoutedEventHandler(this.MenuItemSetDifferentModel_Click);
          break;
        case 34:
          this.MenuItemSetDifferentEquipment = (System.Windows.Controls.MenuItem) target;
          this.MenuItemSetDifferentEquipment.Click += new RoutedEventHandler(this.MenuItemSetDifferentEquipment_Click);
          break;
        case 35:
          this.MenuItemSetDifferentType = (System.Windows.Controls.MenuItem) target;
          this.MenuItemSetDifferentType.Click += new RoutedEventHandler(this.MenuItemSetDifferentType_Click);
          break;
        case 36:
          this.MenuItemChangeDeviceModelID = (System.Windows.Controls.MenuItem) target;
          this.MenuItemChangeDeviceModelID.Click += new RoutedEventHandler(this.MenuItemChangeDeviceModelID_Click);
          break;
        case 37:
          this.MenuItemChangeEquipmentModelID = (System.Windows.Controls.MenuItem) target;
          this.MenuItemChangeEquipmentModelID.Click += new RoutedEventHandler(this.MenuItemChangeEquipmentModelID_Click);
          break;
        case 38:
          this.MenuItemChangeProfileTypeID = (System.Windows.Controls.MenuItem) target;
          this.MenuItemChangeProfileTypeID.Click += new RoutedEventHandler(this.MenuItemChangeProfileTypeID_Click);
          break;
        case 39:
          this.MenuItemChangeSettingsID = (System.Windows.Controls.MenuItem) target;
          this.MenuItemChangeSettingsID.Click += new RoutedEventHandler(this.MenuItemChangeSettingsID_Click);
          break;
        case 40:
          this.MenuItemCreateSettingsCloneAndAssingIt = (System.Windows.Controls.MenuItem) target;
          this.MenuItemCreateSettingsCloneAndAssingIt.Click += new RoutedEventHandler(this.ButtonCreateClonedSettings_Click);
          break;
        case 41:
          this.MenuItemShowAndEditSettings = (System.Windows.Controls.MenuItem) target;
          this.MenuItemShowAndEditSettings.Click += new RoutedEventHandler(this.MenuItemShowAndEditSettings_Click);
          break;
        case 42:
          this.MenuItemEditCommonSettings = (System.Windows.Controls.MenuItem) target;
          this.MenuItemEditCommonSettings.Click += new RoutedEventHandler(this.MenuItemEditCommonSettings_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    private delegate string PastFunction(List<int> theIds);
  }
}

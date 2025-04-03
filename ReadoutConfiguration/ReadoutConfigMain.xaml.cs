// Decompiled with JetBrains decompiler
// Type: ReadoutConfiguration.ReadoutConfigMain
// Assembly: ReadoutConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 1BD19DC4-A290-473A-8451-94ED3EF61361
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ReadoutConfiguration.dll

using GmmDbLib;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace ReadoutConfiguration
{
  public partial class ReadoutConfigMain : Window, IComponentConnector
  {
    private const string translaterBaseKey = "ReadoutConfigMain";
    private bool initialising = false;
    public static BitmapImage NotDefinedImage;
    internal string nextComponent = "";
    private ConnectionProfile activeProfile;
    private SortedList<string, string> activeProfileChangedParameters;
    private ProfileSelecterLists activeSelectorLists;
    private ConfigurationSelector configSelector;
    internal ReadoutPreferences myReadoutPreferences;
    private ConfigList EditConfigList;
    internal System.Windows.Controls.Menu menuMain;
    internal System.Windows.Controls.MenuItem MenuItemOpenProfile;
    internal System.Windows.Controls.MenuItem MenuItemSaveProfileAs;
    internal System.Windows.Controls.MenuItem MenuItemComponents;
    internal GmmCorporateControl gmmCorporateControl1;
    internal Grid GridGroups;
    internal System.Windows.Controls.Label LableDeviceGroupName;
    internal System.Windows.Controls.ComboBox ComboBoxDeviceGroup;
    internal System.Windows.Controls.Button ButtonDeviceGroup;
    internal Image ImageDeviceGroup;
    internal System.Windows.Controls.Label LableEquipmentGroupName;
    internal System.Windows.Controls.ComboBox ComboBoxEquipmentGroup;
    internal System.Windows.Controls.Button ButtonEquipmentGroup;
    internal Image ImageEquipmentGroup;
    internal System.Windows.Controls.Label LableProfileTypeGroupName;
    internal System.Windows.Controls.ComboBox ComboBoxProfileTypeGroup;
    internal System.Windows.Controls.Button ButtonProfileTypeGroup;
    internal Image ImageProfileTypeGroup;
    internal Grid GridModels;
    internal System.Windows.Controls.Label LableDeviceModelName;
    internal System.Windows.Controls.ComboBox ComboBoxDeviceModelName;
    internal System.Windows.Controls.Button ButtonDeviceModel;
    internal Image ImageDeviceModel;
    internal System.Windows.Controls.Label LableEquipmentModel;
    internal System.Windows.Controls.ComboBox ComboBoxEquipmentModelName;
    internal System.Windows.Controls.Button ButtonEquipmentModel;
    internal Image ImageEquipmentModel;
    internal System.Windows.Controls.Label LableProfileTypeName;
    internal System.Windows.Controls.ComboBox ComboBoxProfileTypeName;
    internal System.Windows.Controls.Button ButtonProfileType;
    internal Image ImageProfileTypeModel;
    internal System.Windows.Controls.Button ButtonResetToDefault;
    internal System.Windows.Controls.Button ButtonResetToPreSet;
    internal System.Windows.Controls.Button ButtonProfileParameters;
    internal System.Windows.Controls.Button ButtonShowProfilesList;
    internal System.Windows.Controls.Button ButtonUseThisConfig;
    internal System.Windows.Controls.GroupBox GroupBoxDeveloperInfo;
    internal StackPanel StackPanelDeveloperPanel;
    internal System.Windows.Controls.TextBox TextBoxIdInfos0;
    internal System.Windows.Controls.TextBox TextBoxIdInfos1;
    internal System.Windows.Controls.TextBox TextBoxIdInfos2;
    internal System.Windows.Controls.CheckBox CheckBoxShowAllChangableParameters;
    internal StackPanel StackPanelNotDefault;
    internal PropertyGrid PropertyGridSettings;
    private bool _contentLoaded;

    public ReadoutConfigMain(ReadoutPreferences readoutPreferences, bool enableComponentMenu)
    {
      this.myReadoutPreferences = readoutPreferences != null ? readoutPreferences : throw new Exception("ReadoutPreferences cannot be null");
      this.myReadoutPreferences.GarantSelectedProfile();
      this.configSelector = new ConfigurationSelector(this.myReadoutPreferences.FilteredProfiles);
      this.InitializeComponent();
      if (enableComponentMenu)
        UserInterfaceServices.AddDefaultMenu((System.Windows.Controls.MenuItem) this.menuMain.Items[1], new RoutedEventHandler(this.componentsClick));
      if (((ItemsControl) this.menuMain.Items[1]).Items.Count == 0)
        this.menuMain.Visibility = Visibility.Collapsed;
      if (readoutPreferences.IsProfileEditingEnabled)
      {
        this.ButtonShowProfilesList.Visibility = Visibility.Visible;
        this.ButtonProfileParameters.Visibility = Visibility.Visible;
      }
      else
      {
        this.ButtonShowProfilesList.Visibility = Visibility.Hidden;
        this.ButtonProfileParameters.Visibility = Visibility.Hidden;
      }
      WpfTranslatorSupport.TranslateWindow(Tg.ReadoutConfigMain, (Window) this);
      if (ReadoutConfigMain.NotDefinedImage == null)
        ReadoutConfigMain.NotDefinedImage = new BitmapImage(new Uri("pack://application:,,,/ReadoutConfiguration;component/Resources/NotDefinedImage.png"));
      this.ShowConnectionProfile(this.myReadoutPreferences.AdjustedProfile.ConnectionProfileID);
    }

    internal SortedList<string, string> GetResultSetup()
    {
      return this.myReadoutPreferences.AdjustedProfile.GetAdjustedList();
    }

    private void componentsClick(object sender, RoutedEventArgs e)
    {
      this.nextComponent = ((HeaderedItemsControl) sender).Header.ToString();
      this.Hide();
    }

    private void ButtonShowProfilesList_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        int connectionProfileId = this.activeProfile.ConnectionProfileID;
        ProfilesList profilesList = new ProfilesList(this.activeProfile.ConnectionProfileID);
        profilesList.Owner = (Window) this;
        bool? nullable = profilesList.ShowDialog();
        bool flag = true;
        if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
          return;
        this.ReloadAndShowConnectionProfile(profilesList.selectedProfileID);
      }
      catch (Exception ex)
      {
        this.WorkException("Change profiles error", ex);
      }
    }

    private void ButtonProfileParameters_Click(object sender, RoutedEventArgs e)
    {
      if (!ParameterListEditor.EditProfileParameter(this.activeProfile.ConnectionProfileID, (Window) this))
        return;
      this.ReloadAndShowConnectionProfile(this.activeProfile.ConnectionProfileID);
    }

    private void WorkException(string Message, Exception ex)
    {
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.Exception, ex.ToString());
      ZR_ClassLibMessages.AddErrorDescription("Exception:");
      ZR_ClassLibMessages.AddErrorDescription(Message);
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void AddInfo(StringBuilder changeInfo, string line, string more)
    {
      changeInfo.Append(line);
      changeInfo.Append(' ');
      for (int length = line.Length; length < 60; ++length)
        changeInfo.Append('.');
      changeInfo.Append(' ');
      changeInfo.AppendLine(more);
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      e.Cancel = true;
      this.nextComponent = "";
      this.Hide();
    }

    private void ReloadAndShowConnectionProfile(int ConnectionProfileID)
    {
      this.myReadoutPreferences.ReloadDatabaseAndEnableAllChanges();
      this.configSelector = new ConfigurationSelector(this.myReadoutPreferences.FilteredProfiles);
      this.ShowConnectionProfile(ConnectionProfileID);
    }

    private void ShowConnectionProfile(int ConnectionProfileID)
    {
      try
      {
        ConfigDatabaseAccess dbData = ReadoutConfigFunctions.DbData;
        if (this.myReadoutPreferences.AdjustedProfile.ConnectionProfileID != ConnectionProfileID)
          this.myReadoutPreferences.ChangeToProfile(ConnectionProfileID);
        this.activeProfileChangedParameters = new SortedList<string, string>();
        if (this.myReadoutPreferences != null && this.myReadoutPreferences.AdjustedProfile != null && this.myReadoutPreferences.AdjustedProfile.AdjustedParameters != null && this.myReadoutPreferences.AdjustedProfile.AdjustedParameters.Count > 0)
        {
          ConnectionProfile connectionProfile = dbData.CachedPartiallyConnectionProfiles.FirstOrDefault<ConnectionProfile>((Func<ConnectionProfile, bool>) (x => x.ConnectionProfileID == ConnectionProfileID));
          foreach (string key in (IEnumerable<string>) connectionProfile.ConnectionSettings.SetupParameterList.Keys)
          {
            if (this.myReadoutPreferences.AdjustedProfile.AdjustedParameters.ContainsKey(key))
            {
              string setupParameter = connectionProfile.ConnectionSettings.SetupParameterList[key];
              string adjustedParameter = this.myReadoutPreferences.AdjustedProfile.AdjustedParameters[key];
              if (setupParameter != adjustedParameter)
                this.activeProfileChangedParameters.Add(key, adjustedParameter);
            }
          }
        }
        this.initialising = true;
        this.activeProfile = dbData.CachedPartiallyConnectionProfiles.FirstOrDefault<ConnectionProfile>((Func<ConnectionProfile, bool>) (x => x.ConnectionProfileID == ConnectionProfileID));
        if (this.activeProfile == null)
          throw new Exception("Unknown ConnectionProfileID: " + ConnectionProfileID.ToString());
        dbData.PreloadImages(new List<int>()
        {
          this.activeProfile.DeviceModel.ImageID,
          this.activeProfile.DeviceModel.DeviceGroup.ImageID,
          this.activeProfile.EquipmentModel.ImageID,
          this.activeProfile.EquipmentModel.EquipmentGroup.ImageID,
          this.activeProfile.ProfileType.ImageID,
          this.activeProfile.ProfileType.ProfileTypeGroup.ImageID
        });
        this.activeSelectorLists = this.configSelector.GetAllowedSelectorLists(ConnectionProfileID);
        List<DeviceModel> deviceModelList = this.activeSelectorLists != null ? this.activeSelectorLists.allDeviceModelsList : throw new Exception("Unknown ConnectionProfileID: " + ConnectionProfileID.ToString());
        if (this.myReadoutPreferences.ChangeDeviceModelsAllowed == null || this.myReadoutPreferences.ChangeDeviceModelsAllowed.Count == 0)
        {
          int num = (int) System.Windows.MessageBox.Show("No DeviceModel allowed. Please change restrictions.");
          this.Close();
        }
        else
        {
          if (this.myReadoutPreferences.ChangeDeviceModelsAllowed.Count == 1)
          {
            this.ComboBoxDeviceModelName.IsEnabled = false;
            this.ButtonDeviceModel.IsEnabled = false;
          }
          deviceModelList.RemoveAll((Predicate<DeviceModel>) (item => this.myReadoutPreferences.ChangeDeviceModelsAllowed.Find((Predicate<int>) (allowedItem => allowedItem == item.DeviceModelID)) == 0));
          DeviceModel deviceModel = deviceModelList.Find((Predicate<DeviceModel>) (item => item.DeviceModelID == this.activeProfile.DeviceModel.DeviceModelID));
          if (deviceModel == null)
            throw new InvalidDataException("Profile device model is not part of allowed device models");
          this.ComboBoxDeviceModelName.ItemsSource = (IEnumerable) deviceModelList;
          this.ComboBoxDeviceModelName.SelectedItem = (object) deviceModel;
          this.ImageDeviceModel.Source = (ImageSource) this.GetImage(this.activeProfile.DeviceModel.Image500x500);
          this.ButtonDeviceModel.Tag = (object) this.activeProfile.DeviceModel;
          DeviceGroup deviceGroup = this.activeSelectorLists.allDeviceGroupsList.Find((Predicate<DeviceGroup>) (item => item.DeviceGroupID == this.activeProfile.DeviceModel.DeviceGroup.DeviceGroupID));
          this.ComboBoxDeviceGroup.ItemsSource = (IEnumerable) this.activeSelectorLists.allDeviceGroupsList;
          this.ComboBoxDeviceGroup.SelectedItem = (object) deviceGroup;
          this.ComboBoxDeviceGroup.SelectedItem = (object) this.activeProfile.DeviceModel.DeviceGroup;
          this.ImageDeviceGroup.Source = (ImageSource) this.GetImage(this.activeProfile.DeviceModel.DeviceGroup.Image500x500);
          this.ButtonDeviceGroup.Tag = (object) this.activeProfile.DeviceModel.DeviceGroup;
          if (this.myReadoutPreferences.ChangeDeviceGroupsAllowed == null || this.myReadoutPreferences.ChangeDeviceGroupsAllowed.Count == 1)
          {
            this.ComboBoxDeviceGroup.IsEnabled = false;
            this.ButtonDeviceGroup.IsEnabled = false;
          }
          EquipmentModel equipmentModel = this.activeSelectorLists.reducedEquipmentModels.Find((Predicate<EquipmentModel>) (item => item.EquipmentModelID == this.activeProfile.EquipmentModel.EquipmentModelID));
          this.ComboBoxEquipmentModelName.ItemsSource = (IEnumerable) this.activeSelectorLists.reducedEquipmentModels;
          this.ComboBoxEquipmentModelName.SelectedItem = (object) equipmentModel;
          this.ImageEquipmentModel.Source = (ImageSource) this.GetImage(this.activeProfile.EquipmentModel.Image500x500);
          this.ButtonEquipmentModel.Tag = (object) this.activeProfile.EquipmentModel;
          if (this.myReadoutPreferences.ChangeEquipmentModelsAllowed == null || this.myReadoutPreferences.ChangeEquipmentModelsAllowed.Count == 1)
          {
            this.ComboBoxEquipmentModelName.IsEnabled = false;
            this.ButtonEquipmentModel.IsEnabled = false;
          }
          EquipmentGroup equipmentGroup = this.activeSelectorLists.reducedEquipmentGroups.Find((Predicate<EquipmentGroup>) (item => item.EquipmentGroupID == this.activeProfile.EquipmentModel.EquipmentGroup.EquipmentGroupID));
          this.ComboBoxEquipmentGroup.ItemsSource = (IEnumerable) this.activeSelectorLists.reducedEquipmentGroups;
          this.ComboBoxEquipmentGroup.SelectedItem = (object) equipmentGroup;
          this.ImageEquipmentGroup.Source = (ImageSource) this.GetImage(this.activeProfile.EquipmentModel.EquipmentGroup.Image500x500);
          this.ButtonEquipmentGroup.Tag = (object) this.activeProfile.EquipmentModel.EquipmentGroup;
          if (this.myReadoutPreferences.ChangeEquipmentGroupsAllowed == null || this.myReadoutPreferences.ChangeEquipmentGroupsAllowed.Count == 1)
          {
            this.ComboBoxEquipmentGroup.IsEnabled = false;
            this.ButtonEquipmentGroup.IsEnabled = false;
          }
          ProfileType profileType = this.activeSelectorLists.reducedProfileTypes.Find((Predicate<ProfileType>) (item => item.ProfileTypeID == this.activeProfile.ProfileType.ProfileTypeID));
          this.ComboBoxProfileTypeName.ItemsSource = (IEnumerable) this.activeSelectorLists.reducedProfileTypes;
          this.ComboBoxProfileTypeName.SelectedItem = (object) profileType;
          this.ImageProfileTypeModel.Source = (ImageSource) this.GetImage(this.activeProfile.ProfileType.Image500x500);
          this.ButtonProfileType.Tag = (object) this.activeProfile.ProfileType;
          if (this.myReadoutPreferences.ChangeProfileTypeModelsAllowed == null || this.myReadoutPreferences.ChangeProfileTypeModelsAllowed.Count == 1)
          {
            this.ComboBoxProfileTypeName.IsEnabled = false;
            this.ButtonProfileType.IsEnabled = false;
          }
          ProfileTypeGroup profileTypeGroup = this.activeSelectorLists.reducedProfileTypeGroups.Find((Predicate<ProfileTypeGroup>) (item => item.ProfileTypeGroupID == this.activeProfile.ProfileType.ProfileTypeGroup.ProfileTypeGroupID));
          this.ComboBoxProfileTypeGroup.ItemsSource = (IEnumerable) this.activeSelectorLists.reducedProfileTypeGroups;
          this.ComboBoxProfileTypeGroup.SelectedItem = (object) profileTypeGroup;
          this.ImageProfileTypeGroup.Source = (ImageSource) this.GetImage(this.activeProfile.ProfileType.ProfileTypeGroup.Image500x500);
          this.ButtonProfileTypeGroup.Tag = (object) this.activeProfile.ProfileType.ProfileTypeGroup;
          if (this.myReadoutPreferences.ChangeProfileTypeGroupsAllowed == null || this.myReadoutPreferences.ChangeProfileTypeGroupsAllowed.Count == 1)
          {
            this.ComboBoxProfileTypeGroup.IsEnabled = false;
            this.ButtonProfileTypeGroup.IsEnabled = false;
          }
          if (UserManager.CheckPermission("Developer"))
          {
            this.GroupBoxDeveloperInfo.Visibility = Visibility.Visible;
            this.TextBoxIdInfos0.Text = "ConnectionSettingsName: " + this.activeProfile.ConnectionSettings.Name;
            StringBuilder stringBuilder1 = new StringBuilder();
            stringBuilder1.AppendLine("ConnectionProfileID: " + this.activeProfile.ConnectionProfileID.ToString());
            stringBuilder1.AppendLine("DeviceModelID: " + this.activeProfile.DeviceModel.DeviceModelID.ToString());
            StringBuilder stringBuilder2 = stringBuilder1;
            int num = this.activeProfile.EquipmentModel.EquipmentModelID;
            string str1 = "EquipmentModelID: " + num.ToString();
            stringBuilder2.AppendLine(str1);
            StringBuilder stringBuilder3 = stringBuilder1;
            num = this.activeProfile.ProfileType.ProfileTypeID;
            string str2 = "ProfileTypeID: " + num.ToString();
            stringBuilder3.AppendLine(str2);
            this.TextBoxIdInfos1.Text = stringBuilder1.ToString();
            stringBuilder1.Clear();
            StringBuilder stringBuilder4 = stringBuilder1;
            num = this.activeProfile.ConnectionSettings.ConnectionSettingsID;
            string str3 = "ConnectionSettingsID: " + num.ToString();
            stringBuilder4.AppendLine(str3);
            StringBuilder stringBuilder5 = stringBuilder1;
            num = this.activeProfile.DeviceModel.DeviceGroup.DeviceGroupID;
            string str4 = "DeviceGroupID: " + num.ToString();
            stringBuilder5.AppendLine(str4);
            StringBuilder stringBuilder6 = stringBuilder1;
            num = this.activeProfile.EquipmentModel.EquipmentGroup.EquipmentGroupID;
            string str5 = "EquipmentGroupID: " + num.ToString();
            stringBuilder6.AppendLine(str5);
            StringBuilder stringBuilder7 = stringBuilder1;
            num = this.activeProfile.ProfileType.ProfileTypeGroup.ProfileTypeGroupID;
            string str6 = "ProfileTypeGroupID: " + num.ToString();
            stringBuilder7.AppendLine(str6);
            this.TextBoxIdInfos2.Text = stringBuilder1.ToString();
          }
          else
            this.GroupBoxDeveloperInfo.Visibility = Visibility.Collapsed;
          this.ShowParameterByList(this.myReadoutPreferences.AdjustedProfile.GetAdjustedList());
          if (!this.myReadoutPreferences.IsProfileEditingEnabled)
            return;
          System.Windows.Controls.ContextMenu contextMenu1 = new System.Windows.Controls.ContextMenu();
          this.AddChangePictureMenuItem(contextMenu1, this.activeProfile.DeviceModel.DeviceGroup.DeviceGroupID);
          this.AddNameAndDescriptionMenuItem(contextMenu1, this.activeProfile.DeviceModel.DeviceGroup.DeviceGroupID);
          this.ImageDeviceGroup.ContextMenu = contextMenu1;
          System.Windows.Controls.ContextMenu contextMenu2 = new System.Windows.Controls.ContextMenu();
          this.AddChangePictureMenuItem(contextMenu2, this.activeProfile.DeviceModel.DeviceModelID);
          this.AddNameAndDescriptionMenuItem(contextMenu2, this.activeProfile.DeviceModel.DeviceModelID);
          this.AddChangeParameterMenuItem(contextMenu2, this.activeProfile.DeviceModel.DeviceModelID);
          this.AddGroupChangeMenuItem(contextMenu2, this.activeProfile.DeviceModel.DeviceModelID);
          System.Windows.Controls.MenuItem newItem1 = new System.Windows.Controls.MenuItem();
          newItem1.Header = (object) "Create and select new device model";
          newItem1.Click += new RoutedEventHandler(this.AddDeviceModelHandler);
          contextMenu2.Items.Add((object) newItem1);
          System.Windows.Controls.MenuItem newItem2 = new System.Windows.Controls.MenuItem();
          newItem2.Header = (object) "Create new device group and move current device model to new group";
          newItem2.Click += new RoutedEventHandler(this.AddDeviceGroupForModelHandler);
          contextMenu2.Items.Add((object) newItem2);
          this.ImageDeviceModel.ContextMenu = contextMenu2;
          System.Windows.Controls.ContextMenu contextMenu3 = new System.Windows.Controls.ContextMenu();
          this.AddChangePictureMenuItem(contextMenu3, this.activeProfile.EquipmentModel.EquipmentGroup.EquipmentGroupID);
          this.AddNameAndDescriptionMenuItem(contextMenu3, this.activeProfile.EquipmentModel.EquipmentGroup.EquipmentGroupID);
          this.ImageEquipmentGroup.ContextMenu = contextMenu3;
          System.Windows.Controls.ContextMenu contextMenu4 = new System.Windows.Controls.ContextMenu();
          this.AddChangePictureMenuItem(contextMenu4, this.activeProfile.EquipmentModel.EquipmentModelID);
          this.AddNameAndDescriptionMenuItem(contextMenu4, this.activeProfile.EquipmentModel.EquipmentModelID);
          this.AddChangeParameterMenuItem(contextMenu4, this.activeProfile.EquipmentModel.EquipmentModelID);
          this.AddGroupChangeMenuItem(contextMenu4, this.activeProfile.EquipmentModel.EquipmentModelID);
          System.Windows.Controls.MenuItem newItem3 = new System.Windows.Controls.MenuItem();
          newItem3.Header = (object) "Create and select new equipment model";
          newItem3.Click += new RoutedEventHandler(this.AddEquipmentModelHandler);
          contextMenu4.Items.Add((object) newItem3);
          System.Windows.Controls.MenuItem newItem4 = new System.Windows.Controls.MenuItem();
          newItem4.Header = (object) "Create new equipment group and move current equipment model to new group.";
          newItem4.Click += new RoutedEventHandler(this.AddEquipmentGroupForModelHandler);
          contextMenu4.Items.Add((object) newItem4);
          this.ImageEquipmentModel.ContextMenu = contextMenu4;
          System.Windows.Controls.ContextMenu contextMenu5 = new System.Windows.Controls.ContextMenu();
          this.AddChangePictureMenuItem(contextMenu5, this.activeProfile.ProfileType.ProfileTypeGroup.ProfileTypeGroupID);
          this.AddNameAndDescriptionMenuItem(contextMenu5, this.activeProfile.ProfileType.ProfileTypeGroup.ProfileTypeGroupID);
          this.ImageProfileTypeGroup.ContextMenu = contextMenu5;
          System.Windows.Controls.ContextMenu contextMenu6 = new System.Windows.Controls.ContextMenu();
          this.AddChangePictureMenuItem(contextMenu6, this.activeProfile.ProfileType.ProfileTypeID);
          this.AddNameAndDescriptionMenuItem(contextMenu6, this.activeProfile.ProfileType.ProfileTypeID);
          this.AddChangeParameterMenuItem(contextMenu6, this.activeProfile.ProfileType.ProfileTypeID);
          this.AddGroupChangeMenuItem(contextMenu6, this.activeProfile.ProfileType.ProfileTypeID);
          System.Windows.Controls.MenuItem newItem5 = new System.Windows.Controls.MenuItem();
          newItem5.Header = (object) "Create and select new profile type";
          newItem5.Click += new RoutedEventHandler(this.AddProfileTypeHandler);
          contextMenu6.Items.Add((object) newItem5);
          System.Windows.Controls.MenuItem newItem6 = new System.Windows.Controls.MenuItem();
          newItem6.Header = (object) "Create new profile type group and move current profile type to new group";
          newItem6.Click += new RoutedEventHandler(this.AddProfileTypeGroupForProfileTypeHandler);
          contextMenu6.Items.Add((object) newItem6);
          this.ImageProfileTypeModel.ContextMenu = contextMenu6;
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Initializing error", ex);
      }
      finally
      {
        this.initialising = false;
      }
    }

    private void ShowParameterByList(SortedList<string, string> theList)
    {
      if (!this.CheckBoxShowAllChangableParameters.IsChecked.Value)
      {
        SortedList<string, string> changableParameters = ReadoutConfigFunctions.DbData.CachedConnectionSettingsById[this.myReadoutPreferences.AdjustedProfile.SettingsID].AllChangableParameters;
        List<string> stringList = new List<string>();
        foreach (KeyValuePair<string, string> the in theList)
        {
          if (!changableParameters.ContainsKey(the.Key))
            stringList.Add(the.Key);
        }
        foreach (string key in stringList)
          theList.Remove(key);
      }
      this.EditConfigList = new ConfigList(theList);
      this.PropertyGridSettings.SelectedObject = (object) this.EditConfigList;
      this.ShowNotDefaultValues();
    }

    private void ShowNotDefaultValues()
    {
      this.StackPanelNotDefault.Children.Clear();
      SortedList<string, string> adjustedList = this.myReadoutPreferences.AdjustedProfile.GetAdjustedList();
      SortedList<string, string> sortedList = this.EditConfigList.GetSortedList();
      foreach (KeyValuePair<string, string> defaultParameter in this.myReadoutPreferences.AdjustedProfile.DefaultParameters)
      {
        string str1 = defaultParameter.Value;
        bool flag = false;
        string str2;
        if (sortedList.ContainsKey(defaultParameter.Key))
        {
          str2 = sortedList[defaultParameter.Key];
          if (str2 != str1)
            flag = true;
        }
        else
          str2 = "-";
        string str3;
        if (adjustedList.ContainsKey(defaultParameter.Key))
        {
          str3 = adjustedList[defaultParameter.Key];
          if (str3 != str1)
            flag = true;
        }
        else
          str3 = "-";
        if (flag)
        {
          Grid element1 = new Grid();
          element1.ColumnDefinitions.Add(new ColumnDefinition()
          {
            Width = new GridLength(2.0, GridUnitType.Star)
          });
          element1.ColumnDefinitions.Add(new ColumnDefinition()
          {
            Width = new GridLength(1.0, GridUnitType.Star)
          });
          element1.ColumnDefinitions.Add(new ColumnDefinition()
          {
            Width = new GridLength(1.0, GridUnitType.Star)
          });
          element1.ColumnDefinitions.Add(new ColumnDefinition()
          {
            Width = new GridLength(1.0, GridUnitType.Star)
          });
          TextBlock element2 = new TextBlock();
          element2.Text = defaultParameter.Key;
          Grid.SetColumn((UIElement) element2, 0);
          element1.Children.Add((UIElement) element2);
          TextBlock element3 = new TextBlock();
          element3.Text = str1;
          Grid.SetColumn((UIElement) element3, 1);
          element1.Children.Add((UIElement) element3);
          TextBlock element4 = new TextBlock();
          element4.Text = str3;
          Grid.SetColumn((UIElement) element4, 2);
          element1.Children.Add((UIElement) element4);
          TextBlock element5 = new TextBlock();
          element5.Text = str2;
          Grid.SetColumn((UIElement) element5, 3);
          element1.Children.Add((UIElement) element5);
          this.StackPanelNotDefault.Children.Add((UIElement) element1);
        }
      }
    }

    private void PropertyGridSettings_PropertyValueChanged(
      object s,
      PropertyValueChangedEventArgs e)
    {
      this.ShowNotDefaultValues();
    }

    private void OnShowAllParametersCheckedChanged(object sender, RoutedEventArgs e)
    {
      SortedList<string, string> theList = new SortedList<string, string>();
      SortedList<string, string> sortedList = this.EditConfigList.GetSortedList();
      SortedList<string, string> adjustedList = this.myReadoutPreferences.AdjustedProfile.GetAdjustedList();
      foreach (KeyValuePair<string, string> defaultParameter in this.myReadoutPreferences.AdjustedProfile.DefaultParameters)
      {
        if (sortedList.ContainsKey(defaultParameter.Key))
          theList.Add(defaultParameter.Key, sortedList[defaultParameter.Key]);
        else if (adjustedList.ContainsKey(defaultParameter.Key))
          theList.Add(defaultParameter.Key, adjustedList[defaultParameter.Key]);
        else
          theList.Add(defaultParameter.Key, defaultParameter.Value);
      }
      this.ShowParameterByList(theList);
    }

    private void AddChangePictureMenuItem(System.Windows.Controls.ContextMenu contextMenu, int ConnectionItemID)
    {
      System.Windows.Controls.MenuItem newItem1 = new System.Windows.Controls.MenuItem();
      newItem1.Header = (object) "Change picture from file ..";
      newItem1.Tag = (object) ConnectionItemID;
      newItem1.Click += new RoutedEventHandler(this.ChangePictureHandler);
      contextMenu.Items.Add((object) newItem1);
      System.Windows.Controls.MenuItem newItem2 = new System.Windows.Controls.MenuItem();
      newItem2.Header = (object) "Past picture";
      newItem2.Tag = (object) ConnectionItemID;
      newItem2.Click += new RoutedEventHandler(this.PastPictureHandler);
      contextMenu.Items.Add((object) newItem2);
      System.Windows.Controls.MenuItem newItem3 = new System.Windows.Controls.MenuItem();
      newItem3.Header = (object) "Copy picture";
      newItem3.Tag = (object) ConnectionItemID;
      newItem3.Click += new RoutedEventHandler(this.CopyPictureHandler);
      contextMenu.Items.Add((object) newItem3);
    }

    private void AddNameAndDescriptionMenuItem(System.Windows.Controls.ContextMenu contextMenu, int ConnectionItemID)
    {
      System.Windows.Controls.MenuItem newItem = new System.Windows.Controls.MenuItem();
      newItem.Header = (object) "Change name and description";
      newItem.Tag = (object) ConnectionItemID;
      newItem.Click += new RoutedEventHandler(this.ChangeNameAndDescriptionHandler);
      contextMenu.Items.Add((object) newItem);
    }

    private void AddChangeParameterMenuItem(System.Windows.Controls.ContextMenu contextMenu, int ConnectionItemID)
    {
      System.Windows.Controls.MenuItem newItem = new System.Windows.Controls.MenuItem();
      newItem.Header = (object) "Show and edit parameters";
      newItem.Tag = (object) ConnectionItemID;
      newItem.Click += new RoutedEventHandler(this.ShowAndEditParameters);
      contextMenu.Items.Add((object) newItem);
    }

    private void AddGroupChangeMenuItem(System.Windows.Controls.ContextMenu contextMenu, int ConnectionItemID)
    {
      System.Windows.Controls.MenuItem newItem = new System.Windows.Controls.MenuItem();
      newItem.Header = (object) "Change group";
      newItem.Tag = (object) ConnectionItemID;
      newItem.Click += new RoutedEventHandler(this.ChooseGroupHandler);
      contextMenu.Items.Add((object) newItem);
    }

    private BitmapImage GetImage(BitmapImage imageObject)
    {
      return imageObject != null ? imageObject : ReadoutConfigMain.NotDefinedImage;
    }

    private void ChangePictureHandler(object sender, RoutedEventArgs e)
    {
      if (!ReadoutConfigFunctions.DbData.ChangePicture((int) ((FrameworkElement) sender).Tag))
        return;
      this.ReloadAndShowConnectionProfile(this.activeProfile.ConnectionProfileID);
    }

    private void PastPictureHandler(object sender, RoutedEventArgs e)
    {
      if (!ReadoutConfigFunctions.DbData.PastPicture((int) ((FrameworkElement) sender).Tag))
        return;
      this.ReloadAndShowConnectionProfile(this.activeProfile.ConnectionProfileID);
    }

    private void CopyPictureHandler(object sender, RoutedEventArgs e)
    {
      ReadoutConfigFunctions.DbData.CopyPicture((int) ((FrameworkElement) sender).Tag);
    }

    private void ChangeNameAndDescriptionHandler(object sender, RoutedEventArgs e)
    {
      if (!ReadoutConfigFunctions.DbData.ChangeNameAndDescription((int) ((FrameworkElement) sender).Tag))
        return;
      this.ReloadAndShowConnectionProfile(this.activeProfile.ConnectionProfileID);
    }

    private void ShowAndEditParameters(object sender, RoutedEventArgs e)
    {
      if (!ParameterListEditor.EditItemParameter((int) ((FrameworkElement) sender).Tag))
        return;
      this.ReloadAndShowConnectionProfile(this.activeProfile.ConnectionProfileID);
    }

    private void ChooseGroupHandler(object sender, RoutedEventArgs e)
    {
      if (!ReadoutConfigFunctions.DbData.ChooseNewGroup((int) ((FrameworkElement) sender).Tag))
        return;
      this.ReloadAndShowConnectionProfile(this.activeProfile.ConnectionProfileID);
    }

    private void AddDeviceModelHandler(object sender, RoutedEventArgs e)
    {
      if (!ReadoutConfigFunctions.DbData.AddDeviceModel(this.activeProfile))
        return;
      this.ReloadAndShowConnectionProfile(this.activeProfile.ConnectionProfileID);
    }

    private void AddDeviceGroupForModelHandler(object sender, RoutedEventArgs e)
    {
      if (!ReadoutConfigFunctions.DbData.AddDeviceGroupForDeviceModel(this.activeProfile))
        return;
      this.ReloadAndShowConnectionProfile(this.activeProfile.ConnectionProfileID);
    }

    private void AddEquipmentModelHandler(object sender, RoutedEventArgs e)
    {
      if (!ReadoutConfigFunctions.DbData.AddEquipmentModel(this.activeProfile))
        return;
      this.ReloadAndShowConnectionProfile(this.activeProfile.ConnectionProfileID);
    }

    private void AddEquipmentGroupForModelHandler(object sender, RoutedEventArgs e)
    {
      if (!ReadoutConfigFunctions.DbData.AddEquipmentGroupForModel(this.activeProfile))
        return;
      this.ReloadAndShowConnectionProfile(this.activeProfile.ConnectionProfileID);
    }

    private void AddProfileTypeHandler(object sender, RoutedEventArgs e)
    {
      if (!ReadoutConfigFunctions.DbData.AddProfileType(this.activeProfile))
        return;
      this.ReloadAndShowConnectionProfile(this.activeProfile.ConnectionProfileID);
    }

    private void AddProfileTypeGroupForProfileTypeHandler(object sender, RoutedEventArgs e)
    {
      if (!ReadoutConfigFunctions.DbData.AddProfileTypeGroupForProfileType(this.activeProfile))
        return;
      this.ReloadAndShowConnectionProfile(this.activeProfile.ConnectionProfileID);
    }

    private void ComboBoxSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.initialising)
        return;
      try
      {
        this.ShowNextProfileBySelectorIDs(this.GetSelectorIDs());
      }
      catch (Exception ex)
      {
        this.WorkException("Selection changed error", ex);
      }
    }

    private ProfileSelecterIDs GetSelectorIDs()
    {
      return new ProfileSelecterIDs()
      {
        DeviceModelID = ((DeviceModel) this.ComboBoxDeviceModelName.SelectedItem).DeviceModelID,
        DeviceGroupID = ((DeviceGroup) this.ComboBoxDeviceGroup.SelectedItem).DeviceGroupID,
        EquipmentModelID = ((EquipmentModel) this.ComboBoxEquipmentModelName.SelectedItem).EquipmentModelID,
        EquipmentGroupID = ((EquipmentGroup) this.ComboBoxEquipmentGroup.SelectedItem).EquipmentGroupID,
        ProfileTypeID = ((ProfileType) this.ComboBoxProfileTypeName.SelectedItem).ProfileTypeID,
        ProfileTypeGroupID = ((ProfileTypeGroup) this.ComboBoxProfileTypeGroup.SelectedItem).ProfileTypeGroupID
      };
    }

    private void ShowNextProfileBySelectorIDs(ProfileSelecterIDs selectorIDs)
    {
      this.ShowConnectionProfile(this.configSelector.GetProfileIdFromProfileSelecterIDs(this.activeProfile, selectorIDs));
    }

    private void ButtonDeviceGroup_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        DeviceGroup tag = (DeviceGroup) ((FrameworkElement) sender).Tag;
        List<DeviceGroup> deviceGroupsList = this.activeSelectorLists.allDeviceGroupsList;
        List<int> theImageIDs = new List<int>();
        foreach (DeviceGroup deviceGroup in deviceGroupsList)
          theImageIDs.Add(deviceGroup.ImageID);
        ReadoutConfigFunctions.DbData.PreloadImages(theImageIDs);
        ConnectionItemSelectObject selObject = new ConnectionItemSelectObject();
        foreach (DeviceGroup deviceGroup in deviceGroupsList)
        {
          if (deviceGroup == tag)
            selObject.selectedItem = (IConnectionItem) deviceGroup;
          selObject.itemList.Add((IConnectionItem) deviceGroup);
        }
        SelectByPictureWindow selectByPictureWindow = new SelectByPictureWindow(selObject);
        selectByPictureWindow.Owner = (Window) this;
        bool? nullable = selectByPictureWindow.ShowDialog();
        bool flag = true;
        if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
          return;
        int index = selObject.itemList.IndexOf(selObject.selectedItem);
        ProfileSelecterIDs selectorIds = this.GetSelectorIDs();
        selectorIds.DeviceGroupID = deviceGroupsList[index].DeviceGroupID;
        this.ShowNextProfileBySelectorIDs(selectorIds);
      }
      catch (Exception ex)
      {
        this.WorkException("Change by button error", ex);
      }
    }

    private void ButtonDeviceModel_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        DeviceModel tag = (DeviceModel) ((FrameworkElement) sender).Tag;
        List<DeviceModel> deviceModelsList = this.activeSelectorLists.allDeviceModelsList;
        List<int> theImageIDs = new List<int>();
        foreach (DeviceModel deviceModel in deviceModelsList)
          theImageIDs.Add(deviceModel.ImageID);
        ReadoutConfigFunctions.DbData.PreloadImages(theImageIDs);
        ConnectionItemSelectObject selObject = new ConnectionItemSelectObject();
        foreach (DeviceModel deviceModel in deviceModelsList)
        {
          if (deviceModel == tag)
            selObject.selectedItem = (IConnectionItem) deviceModel;
          selObject.itemList.Add((IConnectionItem) deviceModel);
        }
        SelectByPictureWindow selectByPictureWindow = new SelectByPictureWindow(selObject);
        selectByPictureWindow.Owner = (Window) this;
        bool? nullable = selectByPictureWindow.ShowDialog();
        bool flag = true;
        if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
          return;
        int index = selObject.itemList.IndexOf(selObject.selectedItem);
        ProfileSelecterIDs selectorIds = this.GetSelectorIDs();
        selectorIds.DeviceModelID = deviceModelsList[index].DeviceModelID;
        this.ShowNextProfileBySelectorIDs(selectorIds);
      }
      catch (Exception ex)
      {
        this.WorkException("Change by button error", ex);
      }
    }

    private void ButtonEquipmentGroup_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        EquipmentGroup tag = (EquipmentGroup) ((FrameworkElement) sender).Tag;
        List<EquipmentGroup> reducedEquipmentGroups = this.activeSelectorLists.reducedEquipmentGroups;
        List<int> theImageIDs = new List<int>();
        foreach (EquipmentGroup equipmentGroup in reducedEquipmentGroups)
          theImageIDs.Add(equipmentGroup.ImageID);
        ReadoutConfigFunctions.DbData.PreloadImages(theImageIDs);
        ConnectionItemSelectObject selObject = new ConnectionItemSelectObject();
        foreach (EquipmentGroup equipmentGroup in reducedEquipmentGroups)
        {
          if (equipmentGroup == tag)
            selObject.selectedItem = (IConnectionItem) equipmentGroup;
          selObject.itemList.Add((IConnectionItem) equipmentGroup);
        }
        SelectByPictureWindow selectByPictureWindow = new SelectByPictureWindow(selObject);
        selectByPictureWindow.Owner = (Window) this;
        bool? nullable = selectByPictureWindow.ShowDialog();
        bool flag = true;
        if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
          return;
        int index = selObject.itemList.IndexOf(selObject.selectedItem);
        ProfileSelecterIDs selectorIds = this.GetSelectorIDs();
        selectorIds.EquipmentGroupID = reducedEquipmentGroups[index].EquipmentGroupID;
        this.ShowNextProfileBySelectorIDs(selectorIds);
      }
      catch (Exception ex)
      {
        this.WorkException("Change by button error", ex);
      }
    }

    private void ButtonEquipmentModel_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        EquipmentModel tag = (EquipmentModel) ((FrameworkElement) sender).Tag;
        List<EquipmentModel> reducedEquipmentModels = this.activeSelectorLists.reducedEquipmentModels;
        List<int> theImageIDs = new List<int>();
        foreach (EquipmentModel equipmentModel in reducedEquipmentModels)
          theImageIDs.Add(equipmentModel.ImageID);
        ReadoutConfigFunctions.DbData.PreloadImages(theImageIDs);
        ConnectionItemSelectObject selObject = new ConnectionItemSelectObject();
        foreach (EquipmentModel equipmentModel in reducedEquipmentModels)
        {
          if (equipmentModel == tag)
            selObject.selectedItem = (IConnectionItem) equipmentModel;
          selObject.itemList.Add((IConnectionItem) equipmentModel);
        }
        SelectByPictureWindow selectByPictureWindow = new SelectByPictureWindow(selObject);
        selectByPictureWindow.Owner = (Window) this;
        bool? nullable = selectByPictureWindow.ShowDialog();
        bool flag = true;
        if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
          return;
        int index = selObject.itemList.IndexOf(selObject.selectedItem);
        ProfileSelecterIDs selectorIds = this.GetSelectorIDs();
        selectorIds.EquipmentModelID = reducedEquipmentModels[index].EquipmentModelID;
        this.ShowNextProfileBySelectorIDs(selectorIds);
      }
      catch (Exception ex)
      {
        this.WorkException("Change by button error", ex);
      }
    }

    private void ButtonProfileTypeGroup_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        ProfileTypeGroup tag = (ProfileTypeGroup) ((FrameworkElement) sender).Tag;
        List<ProfileTypeGroup> profileTypeGroups = this.activeSelectorLists.reducedProfileTypeGroups;
        List<int> theImageIDs = new List<int>();
        foreach (ProfileTypeGroup profileTypeGroup in profileTypeGroups)
          theImageIDs.Add(profileTypeGroup.ImageID);
        ReadoutConfigFunctions.DbData.PreloadImages(theImageIDs);
        ConnectionItemSelectObject selObject = new ConnectionItemSelectObject();
        foreach (ProfileTypeGroup profileTypeGroup in profileTypeGroups)
        {
          if (profileTypeGroup == tag)
            selObject.selectedItem = (IConnectionItem) profileTypeGroup;
          selObject.itemList.Add((IConnectionItem) profileTypeGroup);
        }
        SelectByPictureWindow selectByPictureWindow = new SelectByPictureWindow(selObject);
        selectByPictureWindow.Owner = (Window) this;
        bool? nullable = selectByPictureWindow.ShowDialog();
        bool flag = true;
        if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
          return;
        int index = selObject.itemList.IndexOf(selObject.selectedItem);
        ProfileSelecterIDs selectorIds = this.GetSelectorIDs();
        selectorIds.ProfileTypeGroupID = profileTypeGroups[index].ProfileTypeGroupID;
        this.ShowNextProfileBySelectorIDs(selectorIds);
      }
      catch (Exception ex)
      {
        this.WorkException("Change by button error", ex);
      }
    }

    private void ButtonProfileType_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        ProfileType tag = (ProfileType) ((FrameworkElement) sender).Tag;
        List<ProfileType> reducedProfileTypes = this.activeSelectorLists.reducedProfileTypes;
        List<int> theImageIDs = new List<int>();
        foreach (ProfileType profileType in reducedProfileTypes)
          theImageIDs.Add(profileType.ImageID);
        ReadoutConfigFunctions.DbData.PreloadImages(theImageIDs);
        ConnectionItemSelectObject selObject = new ConnectionItemSelectObject();
        foreach (ProfileType profileType in reducedProfileTypes)
        {
          if (profileType.Name == tag.Name)
            selObject.selectedItem = (IConnectionItem) profileType;
          selObject.itemList.Add((IConnectionItem) profileType);
        }
        SelectByPictureWindow selectByPictureWindow = new SelectByPictureWindow(selObject);
        selectByPictureWindow.Owner = (Window) this;
        bool? nullable = selectByPictureWindow.ShowDialog();
        bool flag = true;
        if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
          return;
        int index = selObject.itemList.IndexOf(selObject.selectedItem);
        ProfileSelecterIDs selectorIds = this.GetSelectorIDs();
        selectorIds.ProfileTypeID = reducedProfileTypes[index].ProfileTypeID;
        this.ShowNextProfileBySelectorIDs(selectorIds);
      }
      catch (Exception ex)
      {
        this.WorkException("Change by button error", ex);
      }
    }

    private void ButtonUseThisConfig_Click(object sender, RoutedEventArgs e)
    {
      this.myReadoutPreferences.DisableBaseChanges();
      this.AcceptEditResults();
      this.DialogResult = new bool?(true);
      this.Close();
    }

    private void ButtonResetToDefault_Click(object sender, RoutedEventArgs e)
    {
      SortedList<string, string> theList = new SortedList<string, string>();
      this.myReadoutPreferences.AdjustedProfile.AdjustedParameters.Clear();
      foreach (KeyValuePair<string, string> defaultParameter in this.myReadoutPreferences.AdjustedProfile.DefaultParameters)
        theList.Add(defaultParameter.Key, defaultParameter.Value);
      this.ShowParameterByList(theList);
    }

    private void ButtonResetToPreSet_Click(object sender, RoutedEventArgs e)
    {
      SortedList<string, string> theList = new SortedList<string, string>();
      SortedList<string, string> adjustedList = this.myReadoutPreferences.AdjustedProfile.GetAdjustedList();
      foreach (KeyValuePair<string, string> defaultParameter in this.myReadoutPreferences.AdjustedProfile.DefaultParameters)
      {
        if (adjustedList.ContainsKey(defaultParameter.Key))
          theList.Add(defaultParameter.Key, adjustedList[defaultParameter.Key]);
        else
          theList.Add(defaultParameter.Key, defaultParameter.Value);
      }
      this.ShowParameterByList(theList);
    }

    private void AcceptEditResults()
    {
      foreach (KeyValuePair<string, string> sorted in this.EditConfigList.GetSortedList())
        this.myReadoutPreferences.AdjustedProfile.ChangeParameter(sorted.Key, sorted.Value);
      this.myReadoutPreferences.AdjustedProfile.ChangeParameter("ConnectionProfileID", this.myReadoutPreferences.AdjustedProfile.ConnectionProfileID.ToString());
    }

    private void MenuItemSaveProfileAs_Click(object sender, RoutedEventArgs e)
    {
      string path1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Path.Combine("ZENNER", "GMM"));
      Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
      saveFileDialog.Filter = "Profile files (*.profile)|*.profile|All files (*.*)|*.*";
      saveFileDialog.FilterIndex = 1;
      saveFileDialog.RestoreDirectory = true;
      saveFileDialog.InitialDirectory = Path.Combine(path1, "Settings");
      saveFileDialog.FileName = string.Format("{0}.profile", (object) ReadoutConfigFunctions.DbData.CachedConnectionSettingsById[this.myReadoutPreferences.AdjustedProfile.SettingsID].Name);
      bool? nullable = saveFileDialog.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      this.AcceptEditResults();
      ConfigList.Save(saveFileDialog.FileName, new ConfigList(this.myReadoutPreferences.AdjustedProfile.AdjustedParameters)
      {
        ConnectionProfileID = this.myReadoutPreferences.AdjustedProfile.ConnectionProfileID
      });
    }

    private void MenuItemOpenProfile_Click(object sender, RoutedEventArgs e)
    {
      string path1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Path.Combine("ZENNER", "GMM"));
      Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
      openFileDialog.Filter = "Profile files (*.profile)|*.profile|All files (*.*)|*.*";
      openFileDialog.FilterIndex = 1;
      openFileDialog.RestoreDirectory = true;
      openFileDialog.InitialDirectory = Path.Combine(path1, "Settings");
      bool? nullable = openFileDialog.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      this.myReadoutPreferences.AdjustedProfile = new ConnectionProfileAdjusted(ConfigList.Load(openFileDialog.FileName));
      this.ShowConnectionProfile(this.myReadoutPreferences.AdjustedProfile.ConnectionProfileID);
    }

    public static ConfigList ShowDialog(int connectionProfileID)
    {
      ReadoutPreferences readoutPreferences = new ReadoutPreferences(ReadoutConfigFunctions.Manager.GetConnectionProfile(connectionProfileID));
      readoutPreferences.EnableAllChanges();
      ReadoutConfigMain readoutConfigMain = new ReadoutConfigMain(readoutPreferences, false);
      return !readoutConfigMain.ShowDialog().Value ? (ConfigList) null : readoutConfigMain.myReadoutPreferences.AdjustedProfile.GetConfigList();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      System.Windows.Application.LoadComponent((object) this, new Uri("/ReadoutConfiguration;component/readoutconfigmain.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.menuMain = (System.Windows.Controls.Menu) target;
          break;
        case 2:
          this.MenuItemOpenProfile = (System.Windows.Controls.MenuItem) target;
          this.MenuItemOpenProfile.Click += new RoutedEventHandler(this.MenuItemOpenProfile_Click);
          break;
        case 3:
          this.MenuItemSaveProfileAs = (System.Windows.Controls.MenuItem) target;
          this.MenuItemSaveProfileAs.Click += new RoutedEventHandler(this.MenuItemSaveProfileAs_Click);
          break;
        case 4:
          this.MenuItemComponents = (System.Windows.Controls.MenuItem) target;
          break;
        case 5:
          this.gmmCorporateControl1 = (GmmCorporateControl) target;
          break;
        case 6:
          this.GridGroups = (Grid) target;
          break;
        case 7:
          this.LableDeviceGroupName = (System.Windows.Controls.Label) target;
          break;
        case 8:
          this.ComboBoxDeviceGroup = (System.Windows.Controls.ComboBox) target;
          this.ComboBoxDeviceGroup.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxSelector_SelectionChanged);
          break;
        case 9:
          this.ButtonDeviceGroup = (System.Windows.Controls.Button) target;
          this.ButtonDeviceGroup.Click += new RoutedEventHandler(this.ButtonDeviceGroup_Click);
          break;
        case 10:
          this.ImageDeviceGroup = (Image) target;
          break;
        case 11:
          this.LableEquipmentGroupName = (System.Windows.Controls.Label) target;
          break;
        case 12:
          this.ComboBoxEquipmentGroup = (System.Windows.Controls.ComboBox) target;
          this.ComboBoxEquipmentGroup.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxSelector_SelectionChanged);
          break;
        case 13:
          this.ButtonEquipmentGroup = (System.Windows.Controls.Button) target;
          this.ButtonEquipmentGroup.Click += new RoutedEventHandler(this.ButtonEquipmentGroup_Click);
          break;
        case 14:
          this.ImageEquipmentGroup = (Image) target;
          break;
        case 15:
          this.LableProfileTypeGroupName = (System.Windows.Controls.Label) target;
          break;
        case 16:
          this.ComboBoxProfileTypeGroup = (System.Windows.Controls.ComboBox) target;
          this.ComboBoxProfileTypeGroup.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxSelector_SelectionChanged);
          break;
        case 17:
          this.ButtonProfileTypeGroup = (System.Windows.Controls.Button) target;
          this.ButtonProfileTypeGroup.Click += new RoutedEventHandler(this.ButtonProfileTypeGroup_Click);
          break;
        case 18:
          this.ImageProfileTypeGroup = (Image) target;
          break;
        case 19:
          this.GridModels = (Grid) target;
          break;
        case 20:
          this.LableDeviceModelName = (System.Windows.Controls.Label) target;
          break;
        case 21:
          this.ComboBoxDeviceModelName = (System.Windows.Controls.ComboBox) target;
          this.ComboBoxDeviceModelName.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxSelector_SelectionChanged);
          break;
        case 22:
          this.ButtonDeviceModel = (System.Windows.Controls.Button) target;
          this.ButtonDeviceModel.Click += new RoutedEventHandler(this.ButtonDeviceModel_Click);
          break;
        case 23:
          this.ImageDeviceModel = (Image) target;
          break;
        case 24:
          this.LableEquipmentModel = (System.Windows.Controls.Label) target;
          break;
        case 25:
          this.ComboBoxEquipmentModelName = (System.Windows.Controls.ComboBox) target;
          this.ComboBoxEquipmentModelName.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxSelector_SelectionChanged);
          break;
        case 26:
          this.ButtonEquipmentModel = (System.Windows.Controls.Button) target;
          this.ButtonEquipmentModel.Click += new RoutedEventHandler(this.ButtonEquipmentModel_Click);
          break;
        case 27:
          this.ImageEquipmentModel = (Image) target;
          break;
        case 28:
          this.LableProfileTypeName = (System.Windows.Controls.Label) target;
          break;
        case 29:
          this.ComboBoxProfileTypeName = (System.Windows.Controls.ComboBox) target;
          this.ComboBoxProfileTypeName.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxSelector_SelectionChanged);
          break;
        case 30:
          this.ButtonProfileType = (System.Windows.Controls.Button) target;
          this.ButtonProfileType.Click += new RoutedEventHandler(this.ButtonProfileType_Click);
          break;
        case 31:
          this.ImageProfileTypeModel = (Image) target;
          break;
        case 32:
          this.ButtonResetToDefault = (System.Windows.Controls.Button) target;
          this.ButtonResetToDefault.Click += new RoutedEventHandler(this.ButtonResetToDefault_Click);
          break;
        case 33:
          this.ButtonResetToPreSet = (System.Windows.Controls.Button) target;
          this.ButtonResetToPreSet.Click += new RoutedEventHandler(this.ButtonResetToPreSet_Click);
          break;
        case 34:
          this.ButtonProfileParameters = (System.Windows.Controls.Button) target;
          this.ButtonProfileParameters.Click += new RoutedEventHandler(this.ButtonProfileParameters_Click);
          break;
        case 35:
          this.ButtonShowProfilesList = (System.Windows.Controls.Button) target;
          this.ButtonShowProfilesList.Click += new RoutedEventHandler(this.ButtonShowProfilesList_Click);
          break;
        case 36:
          this.ButtonUseThisConfig = (System.Windows.Controls.Button) target;
          this.ButtonUseThisConfig.Click += new RoutedEventHandler(this.ButtonUseThisConfig_Click);
          break;
        case 37:
          this.GroupBoxDeveloperInfo = (System.Windows.Controls.GroupBox) target;
          break;
        case 38:
          this.StackPanelDeveloperPanel = (StackPanel) target;
          break;
        case 39:
          this.TextBoxIdInfos0 = (System.Windows.Controls.TextBox) target;
          break;
        case 40:
          this.TextBoxIdInfos1 = (System.Windows.Controls.TextBox) target;
          break;
        case 41:
          this.TextBoxIdInfos2 = (System.Windows.Controls.TextBox) target;
          break;
        case 42:
          this.CheckBoxShowAllChangableParameters = (System.Windows.Controls.CheckBox) target;
          this.CheckBoxShowAllChangableParameters.Checked += new RoutedEventHandler(this.OnShowAllParametersCheckedChanged);
          this.CheckBoxShowAllChangableParameters.Unchecked += new RoutedEventHandler(this.OnShowAllParametersCheckedChanged);
          break;
        case 43:
          this.StackPanelNotDefault = (StackPanel) target;
          break;
        case 44:
          this.PropertyGridSettings = (PropertyGrid) target;
          this.PropertyGridSettings.PropertyValueChanged += new PropertyValueChangedEventHandler(this.PropertyGridSettings_PropertyValueChanged);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}

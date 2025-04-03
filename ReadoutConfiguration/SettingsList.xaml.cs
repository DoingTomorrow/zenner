// Decompiled with JetBrains decompiler
// Type: ReadoutConfiguration.SettingsList
// Assembly: ReadoutConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 1BD19DC4-A290-473A-8451-94ED3EF61361
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ReadoutConfiguration.dll

using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace ReadoutConfiguration
{
  public partial class SettingsList : Window, IComponentConnector
  {
    private ObservableCollection<SettingsParameterData> ParameterList;
    private ConnectionSettings settings;
    internal bool DataChanged = false;
    internal GmmCorporateControl gmmCorporateControl1;
    internal DataGrid DataGridSettings;
    internal Button ButtonSaveChanges;
    internal Button ButtonDeleteMarked;
    internal Button ButtonMarkeUnused;
    private bool _contentLoaded;

    public SettingsList(ConnectionSettings settings)
    {
      this.settings = settings;
      this.InitializeComponent();
      if (!UserManager.CheckPermission("Developer"))
      {
        this.DataGridSettings.IsReadOnly = true;
        this.DataGridSettings.Margin = new Thickness(0.0);
        this.ButtonSaveChanges.Visibility = Visibility.Hidden;
      }
      this.ParameterList = ReadoutConfigFunctions.DbData.GetSettingList(settings.ConnectionSettingsID);
      this.DataGridSettings.AutoGenerateColumns = true;
      this.DataGridSettings.ItemsSource = (IEnumerable) this.ParameterList;
    }

    private void ButtonSaveChanges_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.DataChanged = ReadoutConfigFunctions.DbData.ChangeSettingList(this.settings.ConnectionSettingsID, this.ParameterList);
        this.Close();
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("Change parameter error", ex.ToString());
      }
    }

    private void ButtonDeleteMarked_Click(object sender, RoutedEventArgs e)
    {
      if (this.DataGridSettings.SelectedItems == null || this.DataGridSettings.SelectedItems.Count < 1)
        return;
      List<SettingsParameterData> settingsParameterDataList = new List<SettingsParameterData>();
      foreach (SettingsParameterData selectedItem in (IEnumerable) this.DataGridSettings.SelectedItems)
        settingsParameterDataList.Add(selectedItem);
      foreach (SettingsParameterData settingsParameterData in settingsParameterDataList)
        this.ParameterList.Remove(settingsParameterData);
    }

    private void ButtonMarkeUnused_Click(object sender, RoutedEventArgs e)
    {
      bool flag = true;
      foreach (SettingsParameterData settingsParameterData in (IEnumerable) this.DataGridSettings.Items)
      {
        if (settingsParameterData.Name == ParameterKey.TransceiverDevice.ToString() && settingsParameterData.Value == "None")
        {
          flag = false;
          break;
        }
      }
      if (flag)
        return;
      foreach (object obj in (IEnumerable) this.DataGridSettings.Items)
      {
        if (obj is SettingsParameterData)
        {
          SettingsParameterData settingsParameterData = (SettingsParameterData) obj;
          if ((settingsParameterData.Name == ParameterKey.IrDaSelection.ToString() || settingsParameterData.Name == ParameterKey.MinoConnectIrDaPulseTime.ToString() || settingsParameterData.Name == ParameterKey.MinoConnectPowerOffTime.ToString() || settingsParameterData.Name == ParameterKey.MinoConnectBaseState.ToString() || settingsParameterData.Name == ParameterKey.ForceMinoConnectState.ToString()) && !this.DataGridSettings.SelectedItems.Contains(obj))
            this.DataGridSettings.SelectedItems.Add(obj);
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
      Application.LoadComponent((object) this, new Uri("/ReadoutConfiguration;component/settingslist.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.gmmCorporateControl1 = (GmmCorporateControl) target;
          break;
        case 2:
          this.DataGridSettings = (DataGrid) target;
          break;
        case 3:
          this.ButtonSaveChanges = (Button) target;
          this.ButtonSaveChanges.Click += new RoutedEventHandler(this.ButtonSaveChanges_Click);
          break;
        case 4:
          this.ButtonDeleteMarked = (Button) target;
          this.ButtonDeleteMarked.Click += new RoutedEventHandler(this.ButtonDeleteMarked_Click);
          break;
        case 5:
          this.ButtonMarkeUnused = (Button) target;
          this.ButtonMarkeUnused.Click += new RoutedEventHandler(this.ButtonMarkeUnused_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}

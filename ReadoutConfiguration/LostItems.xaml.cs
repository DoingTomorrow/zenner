// Decompiled with JetBrains decompiler
// Type: ReadoutConfiguration.LostItems
// Assembly: ReadoutConfiguration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 1BD19DC4-A290-473A-8451-94ED3EF61361
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ReadoutConfiguration.dll

using GmmDbLib;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace ReadoutConfiguration
{
  public partial class LostItems : Window, IComponentConnector
  {
    internal GmmCorporateControl gmmCorporateControl1;
    internal DockPanel DockPanalLostProfiles;
    internal TextBlock TextBlockLostItems;
    internal DataGrid DataGridLostItems;
    internal TextBlock TextBlockLostSettings;
    internal DataGrid DataGridLostSetings;
    internal Button ButtonDeleteSelected;
    private bool _contentLoaded;

    public ObservableCollection<LostConnectionItem> LostConnectionItems { get; set; }

    public ObservableCollection<LostConnectionSetting> LostConnectionSettings { get; set; }

    public LostItems()
    {
      this.InitializeComponent();
      this.LostConnectionItems = new ObservableCollection<LostConnectionItem>();
      this.LostConnectionSettings = new ObservableCollection<LostConnectionSetting>();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e) => this.LoadData();

    private void LoadData()
    {
      this.LostConnectionItems.Clear();
      this.LostConnectionSettings.Clear();
      DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection();
      DbDataAdapter dataAdapter1 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter("SELECT * FROM ConnectionProfiles", newConnection);
      Schema.ConnectionProfilesDataTable profilesDataTable = new Schema.ConnectionProfilesDataTable();
      dataAdapter1.Fill((DataTable) profilesDataTable);
      SortedList<int, int> sortedList1 = new SortedList<int, int>();
      SortedList<int, int> sortedList2 = new SortedList<int, int>();
      foreach (Schema.ConnectionProfilesRow connectionProfilesRow in (TypedTableBase<Schema.ConnectionProfilesRow>) profilesDataTable)
      {
        if (!sortedList1.ContainsKey(connectionProfilesRow.DeviceModelID))
          sortedList1.Add(connectionProfilesRow.DeviceModelID, 0);
        if (!sortedList1.ContainsKey(connectionProfilesRow.EquipmentModelID))
          sortedList1.Add(connectionProfilesRow.EquipmentModelID, 0);
        if (!sortedList1.ContainsKey(connectionProfilesRow.ProfileTypeID))
          sortedList1.Add(connectionProfilesRow.ProfileTypeID, 0);
        if (!sortedList2.ContainsKey(connectionProfilesRow.ConnectionSettingsID))
          sortedList2.Add(connectionProfilesRow.ConnectionSettingsID, 0);
      }
      DbDataAdapter dataAdapter2 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter("SELECT * FROM ConnectionItems", newConnection);
      Schema.ConnectionItemsDataTable connectionItemsDataTable = new Schema.ConnectionItemsDataTable();
      dataAdapter2.Fill((DataTable) connectionItemsDataTable);
      foreach (Schema.ConnectionItemsRow connectionItemsRow in (TypedTableBase<Schema.ConnectionItemsRow>) connectionItemsDataTable)
      {
        if (!sortedList1.ContainsKey(connectionItemsRow.ItemGroupID))
          sortedList1.Add(connectionItemsRow.ItemGroupID, 0);
      }
      int num1 = 0;
      foreach (Schema.ConnectionItemsRow connectionItemsRow in (TypedTableBase<Schema.ConnectionItemsRow>) connectionItemsDataTable)
      {
        if (!sortedList1.ContainsKey(connectionItemsRow.ConnectionItemID))
        {
          ++num1;
          LostConnectionItem lostConnectionItem = new LostConnectionItem(connectionItemsRow.ConnectionItemID, connectionItemsRow.Name);
          if (!connectionItemsRow.IsImageIDNull())
          {
            DbDataAdapter dataAdapter3 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter("SELECT * FROM GmmImages WHERE ImageID = " + connectionItemsRow.ImageID.ToString(), newConnection);
            Schema.GmmImagesDataTable gmmImagesDataTable = new Schema.GmmImagesDataTable();
            dataAdapter3.Fill((DataTable) gmmImagesDataTable);
            if (gmmImagesDataTable.Rows.Count == 1)
            {
              PngBitmapDecoder pngBitmapDecoder = new PngBitmapDecoder((Stream) new MemoryStream(gmmImagesDataTable[0].ImageData), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
              lostConnectionItem.ItemImage = pngBitmapDecoder.Frames[0];
            }
          }
          this.LostConnectionItems.Add(lostConnectionItem);
        }
      }
      this.TextBlockLostItems.Text = "Number of ConnectionItems without reference: " + num1.ToString();
      DbDataAdapter dataAdapter4 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter("SELECT DISTINCT ConnectionSettingsID FROM ConnectionSettings", newConnection);
      DataTable dataTable = new DataTable();
      dataAdapter4.Fill(dataTable);
      int num2 = 0;
      for (int index = 0; index < dataTable.Rows.Count; ++index)
      {
        int num3 = int.Parse(dataTable.Rows[index][0].ToString());
        if (!sortedList2.ContainsKey(num3))
        {
          ++num2;
          this.LostConnectionSettings.Add(new LostConnectionSetting(num3, num3.ToString()));
        }
      }
      this.TextBlockLostSettings.Text = "Number of Settings without reference: " + num2.ToString();
    }

    private void ContextMenuDeleteItem_Click(object sender, RoutedEventArgs e)
    {
      if (this.DataGridLostSetings.SelectedItem != null)
        ;
    }

    private void ButtonDeleteSelected_Click(object sender, RoutedEventArgs e)
    {
      bool flag = false;
      if (this.DataGridLostItems.SelectedItems != null)
      {
        List<int> itemIds = new List<int>();
        foreach (object selectedItem in (IEnumerable) this.DataGridLostItems.SelectedItems)
        {
          if (selectedItem is LostConnectionItem)
          {
            itemIds.Add(((LostConnectionItem) selectedItem).Id);
            flag = true;
          }
        }
        ReadoutConfigFunctions.DbData.DeleteConnectionItems(itemIds);
      }
      if (this.DataGridLostSetings.SelectedItems != null)
      {
        List<int> settingIds = new List<int>();
        foreach (object selectedItem in (IEnumerable) this.DataGridLostSetings.SelectedItems)
        {
          if (selectedItem is LostConnectionSetting)
          {
            settingIds.Add(((LostConnectionSetting) selectedItem).Id);
            flag = true;
          }
        }
        ReadoutConfigFunctions.DbData.DeleteSettings(settingIds);
      }
      if (!flag)
        return;
      this.LoadData();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/ReadoutConfiguration;component/lostitems.xaml", UriKind.Relative));
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
          ((MenuItem) target).Click += new RoutedEventHandler(this.ContextMenuDeleteItem_Click);
          break;
        case 3:
          this.gmmCorporateControl1 = (GmmCorporateControl) target;
          break;
        case 4:
          this.DockPanalLostProfiles = (DockPanel) target;
          break;
        case 5:
          this.TextBlockLostItems = (TextBlock) target;
          break;
        case 6:
          this.DataGridLostItems = (DataGrid) target;
          break;
        case 7:
          this.TextBlockLostSettings = (TextBlock) target;
          break;
        case 8:
          this.DataGridLostSetings = (DataGrid) target;
          break;
        case 9:
          this.ButtonDeleteSelected = (Button) target;
          this.ButtonDeleteSelected.Click += new RoutedEventHandler(this.ButtonDeleteSelected_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}

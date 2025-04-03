// Decompiled with JetBrains decompiler
// Type: HandlerLib.HardwareTypeEditor
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommonWPF;
using GmmDbLib;
using GmmDbLib.DataSets;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public class HardwareTypeEditor : Window, IComponentConnector
  {
    private HardwareTypeTables.HardwareOverviewDataTable HardwareOverviewTable;
    private HardwareTypeTables.ProgFilesDataTable ProgFilesTable;
    private SortedList<int, HardwareTypeTables.HardwareTypeRow> HardwareTypeRows;
    private SortedList<uint, List<HardwareTypeTables.HardwareTypeRow>> FirmwareVersionRows;
    private string[] HardwareNames;
    private uint? FirmwareVersionFromHandler;
    private uint? HardwareTypeFromHandler;
    private bool initFinished = false;
    private bool isChangeAllowed = false;
    private int? lastSelectedHardwareTypeID = new int?();
    private bool suppressTestChanged = false;
    private SortedList<int, HardwareTypeEditor.RowCompareState> CompareStates = new SortedList<int, HardwareTypeEditor.RowCompareState>();
    internal Menu menuMain;
    internal MenuItem MenuItemServiceFunctions;
    internal MenuItem MenuItemShowReleasedFirmwareForSapNumber;
    internal MenuItem MenuItemShowReleasedFirmwareForHardwareType;
    internal MenuItem MenuItemShowReleasedFirmwareForHardwareName;
    internal MenuItem MenuItemShowAllReleasedFirmwareForSapNumber;
    internal MenuItem MenuItemShowAllReleasedFirmwareForHardwareType;
    internal MenuItem MenuItemShowAllReleasedFirmwareForHardwareName;
    internal MenuItem MenuItemShowAllCompatibilities;
    internal StartupLib.GmmCorporateControl gmmCorporateControl1;
    internal StackPanel StackPanelButtons;
    internal Button ButtonShowCompatibleFirmwares;
    internal Button ButtonCreateHardwareType;
    internal Button ButtonChangeHardware;
    internal Button ButtonCompareSelected;
    internal Button ButtonCompareAll;
    internal Button ButtonCopySelected;
    internal TextBox TextBoxFirmwareFromHandlerHex;
    internal TextBox TextBoxFirmwareFromHandler;
    internal TextBox TextBoxHardwareVersionFromHandler;
    internal Button ButtonUseDataFromHandler;
    internal TextBox TextBoxInfo;
    internal ComboBox ComboBoxHardwareName;
    internal TextBox TextBoxFirmwareVersionHex;
    internal TextBox TextBoxFirmwareVersionStr;
    internal TextBox TextBoxFirmwareVersionDec;
    internal TextBox TextBoxHardwareVersion;
    internal TextBox TextBoxHardwareResource;
    internal TextBox TextBoxDescription;
    internal TextBox TextBoxTestinfo;
    internal TextBox TextBoxHardwareOptions;
    internal DataGrid DataGridOverview;
    private bool _contentLoaded;

    public HardwareTypeEditor(string hardwareName, uint? firmwareVersion = null, uint? hardwareType = null)
      : this(new string[1]{ hardwareName }, firmwareVersion, hardwareType)
    {
    }

    public HardwareTypeEditor(
      string[] HardwareNames,
      uint? firmwareVersion = null,
      uint? hardwareType = null,
      bool isChangingAllowed = false)
    {
      this.HardwareNames = HardwareNames;
      this.isChangeAllowed = isChangingAllowed;
      this.FirmwareVersionFromHandler = firmwareVersion;
      this.HardwareTypeFromHandler = hardwareType;
      if (firmwareVersion.HasValue && firmwareVersion.Value == 0U)
        this.FirmwareVersionFromHandler = new uint?();
      this.InitializeComponent();
      this.suppressTestChanged = true;
      this.ComboBoxHardwareName.ItemsSource = (IEnumerable) HardwareNames;
      this.ComboBoxHardwareName.SelectedIndex = 0;
      this.ButtonUseDataFromHandler.IsEnabled = false;
      uint num;
      if (this.FirmwareVersionFromHandler.HasValue)
      {
        this.TextBoxFirmwareFromHandler.Text = this.VersionString(this.FirmwareVersionFromHandler.Value);
        TextBox firmwareFromHandlerHex = this.TextBoxFirmwareFromHandlerHex;
        num = this.FirmwareVersionFromHandler.Value;
        string str = num.ToString("x08");
        firmwareFromHandlerHex.Text = str;
        this.TextBoxFirmwareVersionHex.Text = this.TextBoxFirmwareFromHandlerHex.Text;
        this.ButtonUseDataFromHandler.IsEnabled = true;
      }
      if (this.HardwareTypeFromHandler.HasValue)
      {
        TextBox versionFromHandler = this.TextBoxHardwareVersionFromHandler;
        num = this.HardwareTypeFromHandler.Value;
        string str = num.ToString("x08");
        versionFromHandler.Text = str;
        this.TextBoxHardwareVersion.Text = this.TextBoxHardwareVersionFromHandler.Text;
        this.ButtonUseDataFromHandler.IsEnabled = true;
      }
      this.suppressTestChanged = false;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      this.LoadDataFromDatabase();
      if (this.FirmwareVersionFromHandler.HasValue && this.FirmwareVersionRows.ContainsKey(this.FirmwareVersionFromHandler.Value))
      {
        string str = this.VersionString(this.FirmwareVersionFromHandler.Value);
        foreach (object obj in (IEnumerable) this.DataGridOverview.Items)
        {
          if (obj is DataRowView && ((HardwareTypeTables.HardwareOverviewRow) ((DataRowView) obj).Row).FirmwareVersion == str)
          {
            this.DataGridOverview.SelectedItem = obj;
            break;
          }
        }
      }
      if (this.DataGridOverview.SelectedIndex < 0)
      {
        if (this.DataGridOverview.Items.Count > 0)
        {
          this.DataGridOverview.SelectedIndex = 0;
        }
        else
        {
          if (this.FirmwareVersionFromHandler.HasValue)
            this.TextBoxFirmwareVersionHex.Text = this.TextBoxFirmwareFromHandlerHex.Text;
          if (this.HardwareTypeFromHandler.HasValue)
            this.TextBoxHardwareVersion.Text = this.TextBoxHardwareVersionFromHandler.Text;
        }
      }
      this.UpdateStatus();
    }

    private void LoadDataFromDatabase()
    {
      this.TextBoxHardwareOptions.Clear();
      this.TextBoxInfo.Clear();
      this.TextBoxTestinfo.Clear();
      this.TextBoxDescription.Clear();
      this.TextBoxHardwareVersion.Clear();
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append("SELECT * FROM HardwareType ");
          if (this.HardwareNames != null && this.HardwareNames.Length != 0)
          {
            if (this.HardwareNames.Length == 1)
            {
              stringBuilder.Append(" WHERE HardwareName = '" + this.HardwareNames[0] + "'");
              if (this.HardwareNames[0].Contains<char>('_'))
                stringBuilder.Append(" OR HardwareName = '" + this.HardwareNames[0].Replace('_', ' ') + "'");
            }
            else
            {
              stringBuilder.Append(" WHERE (HardwareName = '" + this.HardwareNames[0] + "'");
              if (this.HardwareNames[0].Contains<char>('_'))
                stringBuilder.Append(" OR HardwareName = '" + this.HardwareNames[0].Replace('_', ' ') + "'");
              for (int index = 1; index < this.HardwareNames.Length; ++index)
              {
                stringBuilder.Append(" OR HardwareName = '" + this.HardwareNames[index] + "'");
                if (this.HardwareNames[index].Contains<char>('_'))
                  stringBuilder.Append(" OR HardwareName = '" + this.HardwareNames[index].Replace('_', ' ') + "'");
              }
              stringBuilder.Append(")");
            }
            stringBuilder.Append(" ORDER BY HardwareTypeID DESC");
          }
          DbDataAdapter dataAdapter1 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection);
          HardwareTypeTables.HardwareTypeDataTable hardwareTypeDataTable = new HardwareTypeTables.HardwareTypeDataTable();
          dataAdapter1.Fill((DataTable) hardwareTypeDataTable);
          this.HardwareOverviewTable = new HardwareTypeTables.HardwareOverviewDataTable();
          this.FirmwareVersionRows = new SortedList<uint, List<HardwareTypeTables.HardwareTypeRow>>();
          HashSet<int> source = new HashSet<int>();
          this.HardwareTypeRows = new SortedList<int, HardwareTypeTables.HardwareTypeRow>();
          if (hardwareTypeDataTable.Count > 0)
          {
            for (int index1 = 0; index1 < hardwareTypeDataTable.Count; ++index1)
            {
              this.HardwareTypeRows.Add(hardwareTypeDataTable[index1].HardwareTypeID, hardwareTypeDataTable[index1]);
              if (!source.Contains(hardwareTypeDataTable[index1].MapID))
                source.Add(hardwareTypeDataTable[index1].MapID);
              HardwareTypeTables.HardwareOverviewRow row = this.HardwareOverviewTable.NewHardwareOverviewRow();
              row.HardwareName = hardwareTypeDataTable[index1].HardwareName;
              row.FirmwareVersion = this.VersionString((uint) hardwareTypeDataTable[index1].FirmwareVersion);
              row.HardwareTypeID = hardwareTypeDataTable[index1].HardwareTypeID;
              row.MapID = hardwareTypeDataTable[index1].MapID;
              if (!hardwareTypeDataTable[index1].IsHardwareResourceNull())
                row.HardwareResource = hardwareTypeDataTable[index1].HardwareResource;
              if (!hardwareTypeDataTable[index1].IsHardwareVersionNull())
                row.HardwareVersion = hardwareTypeDataTable[index1].HardwareVersion.ToString("x04");
              if (!hardwareTypeDataTable[index1].IsDescriptionNull())
                row.Description = hardwareTypeDataTable[index1].Description;
              if (!hardwareTypeDataTable[index1].IsTestinfoNull())
                row.Testinfo = hardwareTypeDataTable[index1].Testinfo;
              if (!hardwareTypeDataTable[index1].IsHardwareOptionsNull())
                row.HardwareOptions = hardwareTypeDataTable[index1].HardwareOptions;
              this.HardwareOverviewTable.AddHardwareOverviewRow(row);
              int index2 = this.FirmwareVersionRows.IndexOfKey((uint) hardwareTypeDataTable[index1].FirmwareVersion);
              if (index2 >= 0)
                this.FirmwareVersionRows.Values[index2].Add(hardwareTypeDataTable[index1]);
              else
                this.FirmwareVersionRows.Add((uint) hardwareTypeDataTable[index1].FirmwareVersion, new List<HardwareTypeTables.HardwareTypeRow>()
                {
                  hardwareTypeDataTable[index1]
                });
            }
            if (source.Count > 0)
            {
              stringBuilder.Clear();
              stringBuilder.Append("SELECT MapID, HardwareTypeMapID, FirmwareVersion, HardwareName FROM ProgFiles");
              stringBuilder.Append(" WHERE ( HardwareTypeMapID = " + source.ElementAt<int>(0).ToString());
              for (int index = 1; index < source.Count; ++index)
                stringBuilder.Append(" OR HardwareTypeMapID = " + source.ElementAt<int>(index).ToString());
              stringBuilder.Append(" )");
              DbDataAdapter dataAdapter2 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), newConnection);
              this.ProgFilesTable = new HardwareTypeTables.ProgFilesDataTable();
              dataAdapter2.Fill((DataTable) this.ProgFilesTable);
              foreach (HardwareTypeTables.HardwareOverviewRow hardwareOverviewRow in (TypedTableBase<HardwareTypeTables.HardwareOverviewRow>) this.HardwareOverviewTable)
              {
                HardwareTypeTables.ProgFilesRow[] progFilesRowArray = (HardwareTypeTables.ProgFilesRow[]) this.ProgFilesTable.Select("HardwareTypeMapID = " + hardwareOverviewRow.MapID.ToString());
                hardwareOverviewRow.CompatibleFirmwares = progFilesRowArray.Length;
              }
            }
          }
          else
            this.TextBoxInfo.Text = "No required HardwareName rows in Database. Create completely new HardwareType!";
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
        return;
      }
      this.DataGridOverview.ItemsSource = (IEnumerable) this.HardwareOverviewTable;
      this.initFinished = true;
      if (!this.lastSelectedHardwareTypeID.HasValue)
        return;
      foreach (object obj in (IEnumerable) this.DataGridOverview.Items)
      {
        if (((HardwareTypeTables.HardwareOverviewRow) ((DataRowView) obj).Row).HardwareTypeID == this.lastSelectedHardwareTypeID.Value)
        {
          this.DataGridOverview.SelectedItem = obj;
          break;
        }
      }
    }

    private void UpdateStatus()
    {
      if (!this.initFinished)
        return;
      this.ButtonShowCompatibleFirmwares.IsEnabled = false;
      this.ButtonChangeHardware.IsEnabled = false;
      this.ButtonCreateHardwareType.IsEnabled = false;
      this.MenuItemShowReleasedFirmwareForHardwareName.IsEnabled = false;
      this.MenuItemShowReleasedFirmwareForHardwareType.IsEnabled = false;
      this.ButtonCompareSelected.IsEnabled = false;
      this.ButtonCompareAll.IsEnabled = false;
      this.ButtonCopySelected.IsEnabled = false;
      uint? usedFirmware = this.GetUsedFirmware();
      if (!usedFirmware.HasValue)
      {
        this.TextBoxInfo.Text = "Not a valied firmware version";
      }
      else
      {
        StringBuilder stringBuilder = new StringBuilder();
        HardwareTypeTables.HardwareTypeRow selectedHardwareTypeRow = this.GetSelectedHardwareTypeRow();
        if (selectedHardwareTypeRow == null)
        {
          HardwareTypeTables.ProgFilesRow[] progFilesRowArray = this.ProgFilesTable != null ? (HardwareTypeTables.ProgFilesRow[]) this.ProgFilesTable.Select("FirmwareVersion = " + usedFirmware.Value.ToString()) : (HardwareTypeTables.ProgFilesRow[]) null;
          if (progFilesRowArray != null && progFilesRowArray.Length != 0)
          {
            stringBuilder.AppendLine("This firmware version is used in a compatible type. HardwareType MapID: " + progFilesRowArray[0].HardwareTypeMapID.ToString());
          }
          else
          {
            stringBuilder.AppendLine("Create new HardwareType and MapID prepared");
            this.ButtonCreateHardwareType.IsEnabled = true;
            stringBuilder.AppendLine("By creating a new HardwareType a new MapID will be created.");
          }
        }
        else
        {
          if ((long) selectedHardwareTypeRow.FirmwareVersion != (long) usedFirmware.Value)
            throw new Exception("Internal error. FirmwareVersionTextBox != selected firmware version");
          this.ButtonShowCompatibleFirmwares.IsEnabled = true;
          this.ButtonChangeHardware.IsEnabled = true;
          this.ButtonCreateHardwareType.IsEnabled = true;
          this.ButtonCompareSelected.IsEnabled = true;
          this.ButtonCompareAll.IsEnabled = true;
          if (this.CompareStates.ContainsKey(selectedHardwareTypeRow.HardwareTypeID) && this.CompareStates[selectedHardwareTypeRow.HardwareTypeID] != HardwareTypeEditor.RowCompareState.equal)
            this.ButtonCopySelected.IsEnabled = true;
          if (selectedHardwareTypeRow.MapID > 0)
          {
            this.MenuItemShowReleasedFirmwareForHardwareName.IsEnabled = true;
            this.MenuItemShowReleasedFirmwareForHardwareType.IsEnabled = true;
          }
          stringBuilder.AppendLine("The selected firmware version is: " + this.VersionString((uint) selectedHardwareTypeRow.FirmwareVersion));
          stringBuilder.AppendLine("By creating a new HardwareType the selected MapID will be used");
        }
        int num = 0;
        int index = this.FirmwareVersionRows.IndexOfKey(usedFirmware.Value);
        if (index >= 0)
          num = this.FirmwareVersionRows.Values[index].Count;
        if (num == 1)
          stringBuilder.AppendLine("Only one type includes the selected firmware.");
        else
          stringBuilder.AppendLine("The selected firmware is used in " + num.ToString() + " types");
        this.TextBoxInfo.Text = stringBuilder.ToString();
      }
    }

    private string VersionString(uint firmwareVersion)
    {
      return new FirmwareVersion(firmwareVersion).ToString();
    }

    private void ButtonShowCompatibleFirmwares_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        HardwareTypeTables.HardwareTypeRow selectedHardwareTypeRow = this.GetSelectedHardwareTypeRow();
        int hardwareTypeId = selectedHardwareTypeRow.HardwareTypeID;
        new CompatibleFirmwareWindow(selectedHardwareTypeRow, this.FirmwareVersionFromHandler).ShowDialog();
        this.LoadDataFromDatabase();
        foreach (object obj in (IEnumerable) this.DataGridOverview.Items)
        {
          if (obj is DataRowView && ((HardwareTypeTables.HardwareOverviewRow) ((DataRowView) obj).Row).HardwareTypeID == hardwareTypeId)
          {
            this.DataGridOverview.SelectedItem = obj;
            break;
          }
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonCreateHardwareType_Click(object sender, RoutedEventArgs e)
    {
      this.SaveHardwareType(false);
    }

    private void ButtonChangeHardware_Click(object sender, RoutedEventArgs e)
    {
      this.SaveHardwareType(true);
    }

    private HardwareTypeTables.HardwareTypeRow GetSelectedHardwareTypeRow()
    {
      return this.DataGridOverview.SelectedItem == null || !(this.DataGridOverview.SelectedItem is DataRowView) ? (HardwareTypeTables.HardwareTypeRow) null : this.HardwareTypeRows[((HardwareTypeTables.HardwareOverviewRow) ((DataRowView) this.DataGridOverview.SelectedItem).Row).HardwareTypeID];
    }

    private void DataGridOverview_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      HardwareTypeTables.HardwareTypeRow selectedHardwareTypeRow = this.GetSelectedHardwareTypeRow();
      if (selectedHardwareTypeRow == null)
        return;
      if (this.ComboBoxHardwareName.SelectedItem.ToString() != selectedHardwareTypeRow.HardwareName)
      {
        for (int index = 0; index < this.ComboBoxHardwareName.Items.Count; ++index)
        {
          if (this.ComboBoxHardwareName.Items[index].ToString() == selectedHardwareTypeRow.HardwareName)
          {
            this.ComboBoxHardwareName.SelectedIndex = index;
            break;
          }
        }
      }
      this.TextBoxFirmwareVersionHex.Text = selectedHardwareTypeRow.FirmwareVersion.ToString("x08");
      if (!selectedHardwareTypeRow.IsHardwareVersionNull())
        this.TextBoxHardwareVersion.Text = selectedHardwareTypeRow.HardwareVersion.ToString("x08");
      else
        this.TextBoxHardwareVersion.Clear();
      if (!selectedHardwareTypeRow.IsHardwareResourceNull())
        this.TextBoxHardwareResource.Text = selectedHardwareTypeRow.HardwareResource;
      else
        this.TextBoxHardwareResource.Clear();
      if (!selectedHardwareTypeRow.IsDescriptionNull())
        this.TextBoxDescription.Text = selectedHardwareTypeRow.Description;
      else
        this.TextBoxDescription.Clear();
      if (!selectedHardwareTypeRow.IsTestinfoNull())
        this.TextBoxTestinfo.Text = selectedHardwareTypeRow.Testinfo;
      else
        this.TextBoxTestinfo.Clear();
      if (!selectedHardwareTypeRow.IsHardwareOptionsNull())
        this.TextBoxHardwareOptions.Text = selectedHardwareTypeRow.HardwareOptions;
      else
        this.TextBoxHardwareOptions.Clear();
      this.UpdateStatus();
    }

    private void TextBoxFirmwareVersion_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.suppressTestChanged)
        return;
      try
      {
        uint result;
        if (this.TextBoxFirmwareVersionHex.Text.Length == 8 && uint.TryParse(this.TextBoxFirmwareVersionHex.Text, NumberStyles.HexNumber, (IFormatProvider) null, out result))
        {
          this.TextBoxFirmwareVersionStr.Text = this.VersionString(result);
          this.TextBoxFirmwareVersionDec.Text = result.ToString();
          string str = this.VersionString(result);
          bool flag = true;
          HardwareTypeTables.HardwareOverviewRow[] hardwareOverviewRowArray1 = (HardwareTypeTables.HardwareOverviewRow[]) this.HardwareOverviewTable.Select("FirmwareVersion = '" + str + "'");
          if (hardwareOverviewRowArray1.Length != 0)
          {
            if (this.DataGridOverview.SelectedItem != null)
            {
              foreach (HardwareTypeTables.HardwareOverviewRow hardwareOverviewRow in hardwareOverviewRowArray1)
              {
                if (hardwareOverviewRow == ((DataRowView) this.DataGridOverview.SelectedItem).Row)
                {
                  flag = false;
                  break;
                }
              }
            }
            if (flag)
              this.SelectViewRow(hardwareOverviewRowArray1[0].HardwareTypeID);
          }
          else if (this.ProgFilesTable != null)
          {
            HardwareTypeTables.ProgFilesRow[] progFilesRowArray = (HardwareTypeTables.ProgFilesRow[]) this.ProgFilesTable.Select("FirmwareVersion = " + result.ToString());
            if (progFilesRowArray.Length != 0)
            {
              HardwareTypeTables.HardwareOverviewRow[] hardwareOverviewRowArray2 = (HardwareTypeTables.HardwareOverviewRow[]) this.HardwareOverviewTable.Select("MapID = " + progFilesRowArray[0].HardwareTypeMapID.ToString());
              if (hardwareOverviewRowArray2.Length != 0)
                this.SelectViewRow(hardwareOverviewRowArray2[0].HardwareTypeID);
              else
                this.DataGridOverview.SelectedItem = (object) null;
            }
            else
              this.DataGridOverview.SelectedItem = (object) null;
          }
        }
        else
        {
          this.TextBoxFirmwareVersionStr.Text = "?";
          this.TextBoxFirmwareVersionDec.Text = "?";
          this.DataGridOverview.SelectedItem = (object) null;
        }
        this.UpdateStatus();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "FirmwareVersion error");
      }
    }

    private void SelectViewRow(int hardwareTypeID)
    {
      if (this.DataGridOverview.Items == null)
        return;
      foreach (DataRowView dataRowView in (IEnumerable) this.DataGridOverview.Items)
      {
        if (((HardwareTypeTables.HardwareOverviewRow) dataRowView.Row).HardwareTypeID == hardwareTypeID)
        {
          this.suppressTestChanged = true;
          this.DataGridOverview.SelectedItem = (object) dataRowView;
          this.suppressTestChanged = false;
          break;
        }
      }
    }

    private void SaveHardwareType(bool change)
    {
      DbBasis.PrimaryDB.BaseDbConnection.ExceptionIfNoSvnDatabase();
      this.CompareStates.Clear();
      HardwareTypeTables.HardwareTypeRow selectedHardwareTypeRow = this.GetSelectedHardwareTypeRow();
      StringBuilder stringBuilder = new StringBuilder();
      int num1;
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          newConnection.Open();
          DbTransaction theTransaction = newConnection.BeginTransaction();
          int index = this.FirmwareVersionRows.IndexOfKey((this.GetUsedFirmware() ?? throw new Exception("Illegal firmware")).Value);
          int mapID;
          if (index >= 0)
          {
            mapID = this.FirmwareVersionRows.Values[index][0].MapID;
          }
          else
          {
            mapID = DbBasis.PrimaryDB.BaseDbConnection.GetNewId("ProgFiles");
            stringBuilder.AppendLine("New MapID created: " + mapID.ToString());
          }
          if (!change)
          {
            num1 = this.CreateNewHardwareType(newConnection, theTransaction, mapID);
            stringBuilder.AppendLine("New Type created: " + num1.ToString());
          }
          else
          {
            num1 = selectedHardwareTypeRow.HardwareTypeID;
            this.ChangeHardwareType(newConnection, theTransaction, mapID);
            stringBuilder.AppendLine("Type changed: " + num1.ToString());
          }
          theTransaction.Commit();
          newConnection.Close();
        }
      }
      catch (Exception ex)
      {
        int num2 = (int) MessageBox.Show(ex.ToString());
        return;
      }
      int num3 = (int) MessageBox.Show(stringBuilder.ToString());
      this.LoadDataFromDatabase();
      foreach (object obj in (IEnumerable) this.DataGridOverview.Items)
      {
        if (obj is DataRowView && ((HardwareTypeTables.HardwareOverviewRow) ((DataRowView) obj).Row).HardwareTypeID == num1)
        {
          this.DataGridOverview.SelectedItem = obj;
          break;
        }
      }
      this.UpdateStatus();
    }

    private int CreateNewHardwareType(
      DbConnection theDbConnection,
      DbTransaction theTransaction,
      int mapID)
    {
      int newId = DbBasis.PrimaryDB.BaseDbConnection.GetNewId("HardwareType");
      string selectSql = "SELECT * FROM HardwareType WHERE HardwareTypeID = " + newId.ToString();
      DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql, theDbConnection, theTransaction, out DbCommandBuilder _);
      HardwareTypeTables.HardwareTypeDataTable hardwareTypeDataTable = new HardwareTypeTables.HardwareTypeDataTable();
      dataAdapter.Fill((DataTable) hardwareTypeDataTable);
      HardwareTypeTables.HardwareTypeRow hardwareTypeRow = hardwareTypeDataTable.Count <= 0 ? hardwareTypeDataTable.NewHardwareTypeRow() : throw new Exception("HardwareTypeID error. New generated ID exists = " + newId.ToString());
      hardwareTypeRow.HardwareTypeID = newId;
      hardwareTypeRow.MapID = mapID;
      this.InsertHardwareTypeData(hardwareTypeRow);
      hardwareTypeDataTable.AddHardwareTypeRow(hardwareTypeRow);
      dataAdapter.Update((DataTable) hardwareTypeDataTable);
      return newId;
    }

    private void ChangeHardwareType(
      DbConnection theDbConnection,
      DbTransaction theTransaction,
      int mapID)
    {
      HardwareTypeTables.HardwareTypeRow selectedHardwareTypeRow = this.GetSelectedHardwareTypeRow();
      string selectSql = "SELECT * FROM HardwareType WHERE HardwareTypeID = " + selectedHardwareTypeRow.HardwareTypeID.ToString();
      DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql, theDbConnection, theTransaction, out DbCommandBuilder _);
      HardwareTypeTables.HardwareTypeDataTable hardwareTypeDataTable = new HardwareTypeTables.HardwareTypeDataTable();
      dataAdapter.Fill((DataTable) hardwareTypeDataTable);
      if (hardwareTypeDataTable.Count != 1)
        throw new Exception("HardwareTypeID error. ID doesn't exists = " + selectedHardwareTypeRow.HardwareTypeID.ToString());
      this.InsertHardwareTypeData(hardwareTypeDataTable[0]);
      hardwareTypeDataTable[0].MapID = mapID;
      dataAdapter.Update((DataTable) hardwareTypeDataTable);
    }

    private uint? GetUsedFirmware()
    {
      if (this.TextBoxFirmwareVersionHex.Text.Length != 8)
        return new uint?();
      uint result;
      return uint.TryParse(this.TextBoxFirmwareVersionHex.Text, NumberStyles.HexNumber, (IFormatProvider) null, out result) ? new uint?(result) : new uint?();
    }

    private void InsertHardwareTypeData(
      HardwareTypeTables.HardwareTypeRow newHardwareTypeRow)
    {
      newHardwareTypeRow.FirmwareVersion = (int) (this.GetUsedFirmware() ?? throw new Exception("Illegal firmware")).Value;
      newHardwareTypeRow.HardwareName = this.HardwareNames[this.ComboBoxHardwareName.SelectedIndex];
      string s = this.TextBoxHardwareVersion.Text.Trim();
      if (s.Length > 0)
      {
        short result;
        if (!short.TryParse(s, NumberStyles.HexNumber, (IFormatProvider) null, out result))
          throw new Exception("HardwareVersion has wrong format");
        newHardwareTypeRow.HardwareVersion = (int) result;
      }
      else
        newHardwareTypeRow.SetHardwareVersionNull();
      string str1 = this.TextBoxHardwareResource.Text.Trim();
      if (!string.IsNullOrEmpty(str1))
        newHardwareTypeRow.HardwareResource = str1;
      else
        newHardwareTypeRow.SetHardwareResourceNull();
      string str2 = this.TextBoxDescription.Text.Trim();
      if (!string.IsNullOrEmpty(str2))
        newHardwareTypeRow.Description = str2;
      else
        newHardwareTypeRow.SetDescriptionNull();
      string str3 = this.TextBoxHardwareOptions.Text.Trim();
      if (!string.IsNullOrEmpty(str3))
        newHardwareTypeRow.HardwareOptions = str3;
      else
        newHardwareTypeRow.SetHardwareOptionsNull();
      string str4 = this.TextBoxTestinfo.Text.Trim();
      if (!string.IsNullOrEmpty(str4))
        newHardwareTypeRow.Testinfo = str4;
      else
        newHardwareTypeRow.SetTestinfoNull();
    }

    private void MenuItemShowReleasedFirmwareForSapNumber_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        string oneValue = EnterOneValue.GetOneValue("Define SAP number", Environment.NewLine + "Type in the requiremd SAP number");
        if (string.IsNullOrEmpty(oneValue))
          return;
        this.ShowFirmwareData(HardwareTypeSupport.GetReleasedFirmwareDataForSapNumber(oneValue), "Released firmware for SAP number: " + oneValue);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void MenuItemShowReleasedFirmwareForHardwareType_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        HardwareTypeTables.HardwareTypeRow selectedHardwareTypeRow = this.GetSelectedHardwareTypeRow();
        this.ShowFirmwareData(HardwareTypeSupport.GetReleasedFirmwareDataForHardwareType(selectedHardwareTypeRow.HardwareTypeID), "Released firmware for HardwareTypeID: " + selectedHardwareTypeRow.HardwareTypeID.ToString());
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void MenuItemShowReleasedFirmwareForHardwareName_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        HardwareTypeTables.HardwareTypeRow selectedHardwareTypeRow = this.GetSelectedHardwareTypeRow();
        this.ShowFirmwareData(HardwareTypeSupport.GetReleasedFirmwareDataForHardwareName(selectedHardwareTypeRow.HardwareName), "Released firmware for HardwareName: " + selectedHardwareTypeRow.HardwareName);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void MenuItemShowAllReleasedFirmwareForSapNumber_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        string oneValue = EnterOneValue.GetOneValue("Define SAP number", Environment.NewLine + "Type in the requiremd SAP number");
        if (string.IsNullOrEmpty(oneValue))
          return;
        this.ShowReleased(HardwareTypeSupport.GetAllReleasedFirmwareReleaseInfosForSapNumber(oneValue), "Released firmwares for SAP number: " + oneValue);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void MenuItemShowAllReleasedFirmwareForHardwareType_Click(
      object sender,
      RoutedEventArgs e)
    {
      try
      {
        HardwareTypeTables.HardwareTypeRow selectedHardwareTypeRow = this.GetSelectedHardwareTypeRow();
        this.ShowReleased(HardwareTypeSupport.GetAllReleasedFirmwareReleaseInfoForHardwareType(selectedHardwareTypeRow.HardwareTypeID), "Released firmwares for HardwareTypeID: " + selectedHardwareTypeRow.HardwareTypeID.ToString());
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void MenuItemShowAllReleasedFirmwareForHardwareName_Click(
      object sender,
      RoutedEventArgs e)
    {
      try
      {
        HardwareTypeTables.HardwareTypeRow selectedHardwareTypeRow = this.GetSelectedHardwareTypeRow();
        this.ShowReleased(HardwareTypeSupport.GetAllReleasedFirmwareReleaseInfosForHardwareName(selectedHardwareTypeRow.HardwareName), "Released firmwares for HardwareName: " + selectedHardwareTypeRow.HardwareName);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ShowReleased(List<FirmwareReleaseInfo> releaseInfo, string firstInfoLine)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(firstInfoLine);
      stringBuilder.AppendLine();
      if (releaseInfo.Count == 0)
      {
        stringBuilder.AppendLine("No released firmware");
      }
      else
      {
        foreach (FirmwareReleaseInfo firmwareReleaseInfo in releaseInfo)
        {
          stringBuilder.AppendLine("MapID: " + firmwareReleaseInfo.MapID.ToString());
          stringBuilder.AppendLine("FirmwareVersion: " + firmwareReleaseInfo.FirmwareVersionString);
          stringBuilder.AppendLine("Release text: " + firmwareReleaseInfo.ReleaseText);
          if (firmwareReleaseInfo.ReleaseDescription != null)
            stringBuilder.AppendLine("Release description: " + firmwareReleaseInfo.ReleaseDescription);
          stringBuilder.AppendLine();
        }
      }
      GmmMessage.Show_Ok(stringBuilder.ToString());
    }

    private void ShowFirmwareData(FirmwareData firmwareData, string firstInfoLine)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(firstInfoLine);
      stringBuilder.AppendLine();
      if (firmwareData == null)
        stringBuilder.AppendLine("No firmware found");
      else if (firmwareData.ProgrammerFileAsString == null)
      {
        stringBuilder.AppendLine("Firmware file not loaded to data base");
      }
      else
      {
        stringBuilder.AppendLine("Programmer file name: " + firmwareData.ProgFileName);
        stringBuilder.AppendLine("Programmer file bytes: " + firmwareData.ProgrammerFileAsString.Length.ToString());
      }
      GmmMessage.Show_Ok(stringBuilder.ToString());
    }

    private void MenuItemShowAllCompatibilities_Click(object sender, RoutedEventArgs e)
    {
      List<CompatibilityInfo> compatibilityInfos = HardwareTypeSupport.GetAllCompatibilityInfos();
      StringBuilder stringBuilder = new StringBuilder();
      int num = 0;
      for (int index = compatibilityInfos.Count - 1; index >= 0; --index)
      {
        CompatibilityInfo compatibilityInfo = compatibilityInfos[index];
        ++num;
        stringBuilder.AppendLine(num.ToString("d04") + ": " + compatibilityInfo.ToString());
      }
      GmmMessage.Show_Ok(stringBuilder.ToString(), "All compatibility infos");
    }

    private void ButtonCompareSelected_Click(object sender, RoutedEventArgs e)
    {
      StringBuilder message = new StringBuilder();
      try
      {
        if (this.DataGridOverview.SelectedItem == null)
          return;
        this.GuarantDatabasesAndGetInfo(message);
        message.AppendLine("*** Compare result ***");
        HardwareTypeTables.HardwareOverviewRow row = (HardwareTypeTables.HardwareOverviewRow) ((DataRowView) this.DataGridOverview.SelectedItem).Row;
        using (DbConnection newConnection1 = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          using (DbConnection newConnection2 = DbBasis.SecondaryDB.BaseDbConnection.GetNewConnection())
            this.GetCompareData(row, newConnection1, newConnection2, message);
        }
        this.DataGridOverview.ItemsSource = (IEnumerable) null;
        this.DataGridOverview.ItemsSource = (IEnumerable) this.HardwareOverviewTable;
        this.SelectViewRow(row.HardwareTypeID);
        GmmMessage.Show_Ok(message.ToString());
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonCompareAll_Click(object sender, RoutedEventArgs e)
    {
      StringBuilder message = new StringBuilder();
      try
      {
        HardwareTypeTables.HardwareOverviewRow hardwareOverviewRow = (HardwareTypeTables.HardwareOverviewRow) null;
        if (this.DataGridOverview.SelectedItem != null)
          hardwareOverviewRow = (HardwareTypeTables.HardwareOverviewRow) ((DataRowView) this.DataGridOverview.SelectedItem).Row;
        this.GuarantDatabasesAndGetInfo(message);
        message.AppendLine("*** Compare result ***");
        using (DbConnection newConnection1 = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          using (DbConnection newConnection2 = DbBasis.SecondaryDB.BaseDbConnection.GetNewConnection())
          {
            foreach (DataRowView dataRowView in (IEnumerable) this.DataGridOverview.Items)
            {
              HardwareTypeTables.HardwareOverviewRow row = (HardwareTypeTables.HardwareOverviewRow) dataRowView.Row;
              message.AppendLine("HardwareTypeID: " + row.HardwareTypeID.ToString());
              this.GetCompareData(row, newConnection1, newConnection2, message);
              message.AppendLine();
            }
          }
        }
        this.DataGridOverview.ItemsSource = (IEnumerable) null;
        this.DataGridOverview.ItemsSource = (IEnumerable) this.HardwareOverviewTable;
        if (hardwareOverviewRow != null)
          this.SelectViewRow(hardwareOverviewRow.HardwareTypeID);
        GmmMessage.Show_Ok(message.ToString());
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonCopySelected_Click(object sender, RoutedEventArgs e)
    {
      StringBuilder message = new StringBuilder();
      try
      {
        this.GuarantDatabasesAndGetInfo(message);
        HardwareTypeTables.HardwareOverviewRow row1 = (HardwareTypeTables.HardwareOverviewRow) ((DataRowView) this.DataGridOverview.SelectedItem).Row;
        using (DbConnection newConnection1 = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          using (DbConnection newConnection2 = DbBasis.SecondaryDB.BaseDbConnection.GetNewConnection())
          {
            string selectSql = "SELECT * FROM HardwareType WHERE HardwareTypeID = " + row1.HardwareTypeID.ToString();
            DbDataAdapter dataAdapter1 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql, newConnection1);
            HardwareTypeTables.HardwareTypeDataTable hardwareTypeDataTable1 = new HardwareTypeTables.HardwareTypeDataTable();
            dataAdapter1.Fill((DataTable) hardwareTypeDataTable1);
            DbDataAdapter dataAdapter2 = DbBasis.SecondaryDB.BaseDbConnection.GetDataAdapter(selectSql, newConnection2, out DbCommandBuilder _);
            HardwareTypeTables.HardwareTypeDataTable hardwareTypeDataTable2 = new HardwareTypeTables.HardwareTypeDataTable();
            dataAdapter2.Fill((DataTable) hardwareTypeDataTable2);
            HardwareTypeTables.HardwareTypeRow hardwareTypeRow = hardwareTypeDataTable1.Count == 1 ? hardwareTypeDataTable1[0] : throw new Exception("Could not load primary data");
            HardwareTypeTables.HardwareTypeRow row2;
            if (hardwareTypeDataTable2.Count == 1)
              row2 = hardwareTypeDataTable2[0];
            else
              row2 = hardwareTypeDataTable2.Count == 0 ? hardwareTypeDataTable2.NewHardwareTypeRow() : throw new Exception("Illegal nubers of secondary rows");
            foreach (DataColumn column in (InternalDataCollectionBase) hardwareTypeDataTable1.Columns)
              row2[column.ColumnName] = hardwareTypeRow[column.ColumnName];
            if (hardwareTypeDataTable2.Count == 0)
              hardwareTypeDataTable2.AddHardwareTypeRow(row2);
            dataAdapter2.Update((DataTable) hardwareTypeDataTable2);
            message.AppendLine("Copy done.");
          }
        }
        this.CompareStates[row1.HardwareTypeID] = HardwareTypeEditor.RowCompareState.equal;
        this.DataGridOverview.ItemsSource = (IEnumerable) null;
        this.DataGridOverview.ItemsSource = (IEnumerable) this.HardwareOverviewTable;
        this.SelectViewRow(row1.HardwareTypeID);
        GmmMessage.Show_Ok(message.ToString());
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void GuarantDatabasesAndGetInfo(StringBuilder message)
    {
      if (!PlugInLoader.InitSecundaryDatabase())
        throw new Exception("Error on loading secondary database");
      message.AppendLine("*** Primary database ***");
      message.AppendLine(DbBasis.PrimaryDB.BaseDbConnection.GetDatabaseInfo(">"));
      message.AppendLine("*** Secondary database ***");
      message.AppendLine(DbBasis.SecondaryDB.BaseDbConnection.GetDatabaseInfo(">"));
    }

    private void GetCompareData(
      HardwareTypeTables.HardwareOverviewRow hardwareOverviewRow,
      DbConnection primDbConnection,
      DbConnection secDbConnection,
      StringBuilder message)
    {
      if (!this.CompareStates.ContainsKey(hardwareOverviewRow.HardwareTypeID))
        this.CompareStates.Add(hardwareOverviewRow.HardwareTypeID, HardwareTypeEditor.RowCompareState.equal);
      else
        this.CompareStates[hardwareOverviewRow.HardwareTypeID] = HardwareTypeEditor.RowCompareState.equal;
      string selectSql = "SELECT * FROM HardwareType WHERE HardwareTypeID = " + hardwareOverviewRow.HardwareTypeID.ToString();
      DbDataAdapter dataAdapter1 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql, primDbConnection);
      HardwareTypeTables.HardwareTypeDataTable hardwareTypeDataTable1 = new HardwareTypeTables.HardwareTypeDataTable();
      dataAdapter1.Fill((DataTable) hardwareTypeDataTable1);
      DbDataAdapter dataAdapter2 = DbBasis.SecondaryDB.BaseDbConnection.GetDataAdapter(selectSql, secDbConnection);
      HardwareTypeTables.HardwareTypeDataTable hardwareTypeDataTable2 = new HardwareTypeTables.HardwareTypeDataTable();
      dataAdapter2.Fill((DataTable) hardwareTypeDataTable2);
      if (hardwareTypeDataTable1.Count != 1)
        throw new Exception("Could not load primary data");
      if (hardwareTypeDataTable2.Count != 1)
      {
        this.CompareStates[hardwareOverviewRow.HardwareTypeID] = HardwareTypeEditor.RowCompareState.notAvailable;
        message.AppendLine("Second data not available");
      }
      else
      {
        foreach (DataColumn column in (InternalDataCollectionBase) hardwareTypeDataTable1.Columns)
        {
          if (hardwareTypeDataTable1[0][column.ColumnName] == DBNull.Value)
          {
            if (hardwareTypeDataTable2[0][column.ColumnName] != DBNull.Value)
            {
              this.CompareStates[hardwareOverviewRow.HardwareTypeID] = HardwareTypeEditor.RowCompareState.different;
              message.AppendLine("Column: " + column.ColumnName + " -> PrimaryDB data not available");
            }
          }
          else if (hardwareTypeDataTable2[0][column.ColumnName] == DBNull.Value)
          {
            this.CompareStates[hardwareOverviewRow.HardwareTypeID] = HardwareTypeEditor.RowCompareState.different;
            message.AppendLine("Column: " + column.ColumnName + " -> SecondaryDB data not available");
          }
          else
          {
            string str1 = hardwareTypeDataTable1[0][column.ColumnName].ToString();
            string str2 = hardwareTypeDataTable2[0][column.ColumnName].ToString();
            if (str2 != str1)
            {
              this.CompareStates[hardwareOverviewRow.HardwareTypeID] = HardwareTypeEditor.RowCompareState.different;
              message.AppendLine("Column: " + column.ColumnName + " -> Data different.");
              if (str1.Length < 256 && str2.Length < 256)
              {
                message.AppendLine("   Primary: " + str1);
                message.AppendLine("   Secondary: " + str2);
              }
            }
          }
        }
      }
      if (this.CompareStates[hardwareOverviewRow.HardwareTypeID] != HardwareTypeEditor.RowCompareState.equal)
        return;
      message.AppendLine("Data are equal");
      this.CompareStates[hardwareOverviewRow.HardwareTypeID] = HardwareTypeEditor.RowCompareState.equal;
    }

    private void DataGridOverview_LoadingRow(object sender, DataGridRowEventArgs e)
    {
      HardwareTypeTables.HardwareOverviewRow row = (HardwareTypeTables.HardwareOverviewRow) ((DataRowView) e.Row.DataContext).Row;
      if (!this.CompareStates.ContainsKey(row.HardwareTypeID))
        return;
      switch (this.CompareStates[row.HardwareTypeID])
      {
        case HardwareTypeEditor.RowCompareState.checkError:
          e.Row.Background = (Brush) Brushes.Red;
          break;
        case HardwareTypeEditor.RowCompareState.equal:
          e.Row.Background = (Brush) Brushes.LightGreen;
          break;
        case HardwareTypeEditor.RowCompareState.different:
          e.Row.Background = (Brush) Brushes.Yellow;
          break;
        case HardwareTypeEditor.RowCompareState.notAvailable:
          e.Row.Background = (Brush) Brushes.Orange;
          break;
      }
    }

    private void ButtonUseDataFromHandler_Click(object sender, RoutedEventArgs e)
    {
      if (this.FirmwareVersionFromHandler.HasValue)
        this.TextBoxFirmwareVersionHex.Text = this.TextBoxFirmwareFromHandlerHex.Text;
      if (!this.HardwareTypeFromHandler.HasValue)
        return;
      this.TextBoxHardwareVersion.Text = this.TextBoxHardwareVersionFromHandler.Text;
    }

    private void DataGridOverview_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      this.ButtonShowCompatibleFirmwares_Click(sender, (RoutedEventArgs) null);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/hardwaremanagement/hardwaretypeeditor.xaml", UriKind.Relative));
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
          this.menuMain = (Menu) target;
          break;
        case 3:
          this.MenuItemServiceFunctions = (MenuItem) target;
          break;
        case 4:
          this.MenuItemShowReleasedFirmwareForSapNumber = (MenuItem) target;
          this.MenuItemShowReleasedFirmwareForSapNumber.Click += new RoutedEventHandler(this.MenuItemShowReleasedFirmwareForSapNumber_Click);
          break;
        case 5:
          this.MenuItemShowReleasedFirmwareForHardwareType = (MenuItem) target;
          this.MenuItemShowReleasedFirmwareForHardwareType.Click += new RoutedEventHandler(this.MenuItemShowReleasedFirmwareForHardwareType_Click);
          break;
        case 6:
          this.MenuItemShowReleasedFirmwareForHardwareName = (MenuItem) target;
          this.MenuItemShowReleasedFirmwareForHardwareName.Click += new RoutedEventHandler(this.MenuItemShowReleasedFirmwareForHardwareName_Click);
          break;
        case 7:
          this.MenuItemShowAllReleasedFirmwareForSapNumber = (MenuItem) target;
          this.MenuItemShowAllReleasedFirmwareForSapNumber.Click += new RoutedEventHandler(this.MenuItemShowAllReleasedFirmwareForSapNumber_Click);
          break;
        case 8:
          this.MenuItemShowAllReleasedFirmwareForHardwareType = (MenuItem) target;
          this.MenuItemShowAllReleasedFirmwareForHardwareType.Click += new RoutedEventHandler(this.MenuItemShowAllReleasedFirmwareForHardwareType_Click);
          break;
        case 9:
          this.MenuItemShowAllReleasedFirmwareForHardwareName = (MenuItem) target;
          this.MenuItemShowAllReleasedFirmwareForHardwareName.Click += new RoutedEventHandler(this.MenuItemShowAllReleasedFirmwareForHardwareName_Click);
          break;
        case 10:
          this.MenuItemShowAllCompatibilities = (MenuItem) target;
          this.MenuItemShowAllCompatibilities.Click += new RoutedEventHandler(this.MenuItemShowAllCompatibilities_Click);
          break;
        case 11:
          this.gmmCorporateControl1 = (StartupLib.GmmCorporateControl) target;
          break;
        case 12:
          this.StackPanelButtons = (StackPanel) target;
          break;
        case 13:
          this.ButtonShowCompatibleFirmwares = (Button) target;
          this.ButtonShowCompatibleFirmwares.Click += new RoutedEventHandler(this.ButtonShowCompatibleFirmwares_Click);
          break;
        case 14:
          this.ButtonCreateHardwareType = (Button) target;
          this.ButtonCreateHardwareType.Click += new RoutedEventHandler(this.ButtonCreateHardwareType_Click);
          break;
        case 15:
          this.ButtonChangeHardware = (Button) target;
          this.ButtonChangeHardware.Click += new RoutedEventHandler(this.ButtonChangeHardware_Click);
          break;
        case 16:
          this.ButtonCompareSelected = (Button) target;
          this.ButtonCompareSelected.Click += new RoutedEventHandler(this.ButtonCompareSelected_Click);
          break;
        case 17:
          this.ButtonCompareAll = (Button) target;
          this.ButtonCompareAll.Click += new RoutedEventHandler(this.ButtonCompareAll_Click);
          break;
        case 18:
          this.ButtonCopySelected = (Button) target;
          this.ButtonCopySelected.Click += new RoutedEventHandler(this.ButtonCopySelected_Click);
          break;
        case 19:
          this.TextBoxFirmwareFromHandlerHex = (TextBox) target;
          break;
        case 20:
          this.TextBoxFirmwareFromHandler = (TextBox) target;
          break;
        case 21:
          this.TextBoxHardwareVersionFromHandler = (TextBox) target;
          break;
        case 22:
          this.ButtonUseDataFromHandler = (Button) target;
          this.ButtonUseDataFromHandler.Click += new RoutedEventHandler(this.ButtonUseDataFromHandler_Click);
          break;
        case 23:
          this.TextBoxInfo = (TextBox) target;
          break;
        case 24:
          this.ComboBoxHardwareName = (ComboBox) target;
          break;
        case 25:
          this.TextBoxFirmwareVersionHex = (TextBox) target;
          this.TextBoxFirmwareVersionHex.TextChanged += new TextChangedEventHandler(this.TextBoxFirmwareVersion_TextChanged);
          break;
        case 26:
          this.TextBoxFirmwareVersionStr = (TextBox) target;
          break;
        case 27:
          this.TextBoxFirmwareVersionDec = (TextBox) target;
          break;
        case 28:
          this.TextBoxHardwareVersion = (TextBox) target;
          break;
        case 29:
          this.TextBoxHardwareResource = (TextBox) target;
          break;
        case 30:
          this.TextBoxDescription = (TextBox) target;
          break;
        case 31:
          this.TextBoxTestinfo = (TextBox) target;
          break;
        case 32:
          this.TextBoxHardwareOptions = (TextBox) target;
          break;
        case 33:
          this.DataGridOverview = (DataGrid) target;
          this.DataGridOverview.SelectionChanged += new SelectionChangedEventHandler(this.DataGridOverview_SelectionChanged);
          this.DataGridOverview.LoadingRow += new EventHandler<DataGridRowEventArgs>(this.DataGridOverview_LoadingRow);
          this.DataGridOverview.MouseDoubleClick += new MouseButtonEventHandler(this.DataGridOverview_MouseDoubleClick);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    private enum RowCompareState
    {
      checkError,
      equal,
      different,
      notAvailable,
    }
  }
}

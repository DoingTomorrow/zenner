// Decompiled with JetBrains decompiler
// Type: HandlerLib.CompatibleFirmwareWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommonWPF;
using GmmDbLib;
using GmmDbLib.DataSets;
using Microsoft.Win32;
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class CompatibleFirmwareWindow : Window, IComponentConnector
  {
    private uint? PreDefinedFirmwareVersion = new uint?();
    private uint? FirmwareVersionFromHandler = new uint?();
    private DbConnection myDbConnection;
    private DbDataAdapter ProgFilesDataAdapter;
    private DbCommandBuilder myComandBuilder;
    private string firmwareVersionUnchanged;
    private bool ignoreEditChanges = false;
    private bool newRowCreated;
    private int CommonMapID;
    private string HardwareName;
    private SortedList<int, CompatibleFirmwareWindow.RowCompareState> CompareStates = new SortedList<int, CompatibleFirmwareWindow.RowCompareState>();
    internal StackPanel StackPanelButtons;
    internal Button ButtonPrepareTable;
    internal Button ButtonImportProgrammerFile;
    internal Button ButtonExportProgrammerFile;
    internal Button ButtonTableToExcel;
    internal Button ButtonShowProgrammerFile;
    internal Button ButtonDeleteSelectedFirmware;
    internal Button ButtonDeleteSelectedFirmwareFile;
    internal Button ButtonAddFirmware;
    internal Button ButtonSaveChanges;
    internal Button ButtonCompareSelected;
    internal Button ButtonCompareAll;
    internal Button ButtonCopySelected;
    internal Border BorderVersionFromHandler;
    internal TextBox TextBoxFirmwareFromHandler;
    internal Button ButtonUseFirmwareFromHandler;
    internal StackPanel StackPanelBottom;
    internal Label LabelStatus;
    internal StackPanel StackPanelEditBoxes;
    internal TextBox TextBoxFirmwareVersion;
    internal TextBox TextBoxReleasedName;
    internal Button ButtonClearReleased;
    internal Button ButtonSetReleased;
    internal TextBox TextBoxCompatibleOverwriteGroups;
    internal CheckBox CheckBoxNoCompression;
    internal TextBox TextBoxOptions;
    internal TextBox TextBoxSourceInfo;
    internal TextBox TextBoxReleaseComments;
    internal Button ButtonFirmwareDependencies;
    internal CheckBox CheckBoxFirmwareDependenciesCleanUp;
    internal TextBox TextBoxFirmwareDependencies;
    internal DataGrid DataGridOverview;
    private bool _contentLoaded;

    public ObservableCollection<KeyValuePair<string, string>> PreDefinedInfos { get; set; }

    public HardwareTypeTables.ProgFilesDataTable ProgFilesTable { get; set; }

    public CompatibleFirmwareWindow(
      HardwareTypeTables.HardwareTypeRow hardwareTypeRow,
      uint? firmwareVersionFromHandler)
    {
      if (!hardwareTypeRow.IsFirmwareVersionNull())
        this.PreDefinedFirmwareVersion = new uint?((uint) hardwareTypeRow.FirmwareVersion);
      if (!this.PreDefinedFirmwareVersion.HasValue)
        this.PreDefinedFirmwareVersion = firmwareVersionFromHandler;
      this.FirmwareVersionFromHandler = firmwareVersionFromHandler;
      if (!this.FirmwareVersionFromHandler.HasValue)
        this.FirmwareVersionFromHandler = this.PreDefinedFirmwareVersion;
      this.CommonMapID = hardwareTypeRow.MapID;
      this.HardwareName = hardwareTypeRow.HardwareName;
      this.InitializeComponent();
      try
      {
        this.PreDefinedInfos = new ObservableCollection<KeyValuePair<string, string>>();
        if (!hardwareTypeRow.IsMapIDNull())
          this.PreDefinedInfos.Add(new KeyValuePair<string, string>("MapID", hardwareTypeRow.MapID.ToString()));
        this.PreDefinedInfos.Add(new KeyValuePair<string, string>("HardwareTypeID", hardwareTypeRow.HardwareTypeID.ToString()));
        if (!hardwareTypeRow.IsHardwareNameNull())
        {
          this.HardwareName = hardwareTypeRow.HardwareName;
          this.PreDefinedInfos.Add(new KeyValuePair<string, string>(nameof (HardwareName), hardwareTypeRow.HardwareName));
        }
        int num;
        if (!hardwareTypeRow.IsFirmwareVersionNull())
        {
          ObservableCollection<KeyValuePair<string, string>> preDefinedInfos = this.PreDefinedInfos;
          string[] strArray = new string[5]
          {
            hardwareTypeRow.FirmwareVersion.ToString(),
            " == 0x",
            null,
            null,
            null
          };
          num = hardwareTypeRow.FirmwareVersion;
          strArray[2] = num.ToString("x08");
          strArray[3] = " == ";
          strArray[4] = new FirmwareVersion((uint) hardwareTypeRow.FirmwareVersion).ToString();
          KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>("FirmwareVersion", string.Concat(strArray));
          preDefinedInfos.Add(keyValuePair);
        }
        if (!hardwareTypeRow.IsHardwareVersionNull())
        {
          ObservableCollection<KeyValuePair<string, string>> preDefinedInfos = this.PreDefinedInfos;
          num = hardwareTypeRow.HardwareVersion;
          string str1 = num.ToString();
          num = hardwareTypeRow.HardwareVersion;
          string str2 = num.ToString("x08");
          KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>("HardwareVersion", str1 + " == 0x" + str2);
          preDefinedInfos.Add(keyValuePair);
        }
        if (!hardwareTypeRow.IsHardwareResourceNull())
          this.PreDefinedInfos.Add(new KeyValuePair<string, string>("HardwareResource", hardwareTypeRow.HardwareResource));
        if (!hardwareTypeRow.IsHardwareOptionsNull())
          this.PreDefinedInfos.Add(new KeyValuePair<string, string>("HardwareOptions", hardwareTypeRow.HardwareOptions));
        if (!hardwareTypeRow.IsTestinfoNull())
          this.PreDefinedInfos.Add(new KeyValuePair<string, string>("Testinfo", hardwareTypeRow.Testinfo));
        if (!hardwareTypeRow.IsDescriptionNull())
          this.PreDefinedInfos.Add(new KeyValuePair<string, string>("Description", hardwareTypeRow.Description));
      }
      catch (Exception ex)
      {
        throw new Exception("Error on load HardwareType data", ex);
      }
      this.BaseConstructorFunction();
    }

    public CompatibleFirmwareWindow(
      int commonMapID,
      string hardwareName,
      uint? preDefinedFirmwareVersion)
    {
      if (string.IsNullOrEmpty(hardwareName))
        throw new ArgumentException("HardwareName not defined");
      this.CommonMapID = commonMapID;
      this.HardwareName = hardwareName;
      this.PreDefinedFirmwareVersion = preDefinedFirmwareVersion;
      this.InitializeComponent();
      try
      {
        this.PreDefinedInfos = new ObservableCollection<KeyValuePair<string, string>>();
        this.PreDefinedInfos.Add(new KeyValuePair<string, string>("Common MapID", this.CommonMapID.ToString()));
        this.PreDefinedInfos.Add(new KeyValuePair<string, string>(nameof (HardwareName), this.HardwareName));
        if (this.PreDefinedFirmwareVersion.HasValue)
        {
          ObservableCollection<KeyValuePair<string, string>> preDefinedInfos = this.PreDefinedInfos;
          string[] strArray = new string[5];
          uint num = this.PreDefinedFirmwareVersion.Value;
          strArray[0] = num.ToString();
          strArray[1] = " == 0x";
          num = this.PreDefinedFirmwareVersion.Value;
          strArray[2] = num.ToString("x08");
          strArray[3] = " == ";
          strArray[4] = new FirmwareVersion(this.PreDefinedFirmwareVersion.Value).ToString();
          KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>("Pre defined FirmwareVersion", string.Concat(strArray));
          preDefinedInfos.Add(keyValuePair);
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Error on load HardwareType data", ex);
      }
      this.BaseConstructorFunction();
    }

    private void BaseConstructorFunction()
    {
      this.myDbConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection();
      this.LoadData();
      if (this.FirmwareVersionFromHandler.HasValue)
      {
        this.BorderVersionFromHandler.Visibility = Visibility.Visible;
        this.TextBoxFirmwareFromHandler.Text = this.FirmwareVersionFromHandler.Value.ToString("x08");
      }
      else
        this.BorderVersionFromHandler.Visibility = Visibility.Collapsed;
      if (this.PreDefinedFirmwareVersion.HasValue)
        this.SetSelectedRowFromVersion((int) this.PreDefinedFirmwareVersion.Value);
      else
        this.DataGridOverview.SelectedIndex = 0;
    }

    private void SetSelectedRowFromVersion(int firmwareVersion)
    {
      int num = -1;
      for (int index = 0; index < this.DataGridOverview.Items.Count; ++index)
      {
        HardwareTypeTables.ProgFilesRow row = (HardwareTypeTables.ProgFilesRow) ((DataRowView) this.DataGridOverview.Items[index]).Row;
        if (!row.IsFirmwareVersionNull() && row.FirmwareVersion == firmwareVersion)
        {
          num = index;
          break;
        }
      }
      if (num >= 0)
        this.DataGridOverview.SelectedIndex = num;
      else
        this.DataGridOverview.SelectedIndex = 0;
    }

    private void LoadData()
    {
      try
      {
        this.ClearEditBoxes();
        this.LabelStatus.Content = (object) "";
        this.DataGridOverview.SelectedIndex = -1;
        this.ButtonAddFirmware.IsEnabled = false;
        this.ButtonSaveChanges.IsEnabled = false;
        this.ButtonDeleteSelectedFirmware.IsEnabled = false;
        this.ButtonDeleteSelectedFirmwareFile.IsEnabled = false;
        this.ButtonImportProgrammerFile.IsEnabled = false;
        this.ButtonExportProgrammerFile.IsEnabled = false;
        this.ButtonShowProgrammerFile.IsEnabled = false;
        this.newRowCreated = false;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("SELECT MapID, SourceInfo, FirmwareVersion, HardwareName, Options, ProgFileName, HardwareTypeMapID, CompatibleOverwriteGroups, ReleasedName, ReleaseComments, FirmwareDependencies");
        stringBuilder.Append(" FROM ProgFiles");
        stringBuilder.Append(" WHERE HardwareTypeMapID = " + this.CommonMapID.ToString());
        stringBuilder.Append(" OR MapID = " + this.CommonMapID.ToString());
        stringBuilder.Append(" ORDER BY MapID DESC");
        this.ProgFilesDataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), this.myDbConnection, out this.myComandBuilder);
        this.ProgFilesTable = new HardwareTypeTables.ProgFilesDataTable();
        this.ProgFilesDataAdapter.Fill((DataTable) this.ProgFilesTable);
        if (this.ProgFilesTable.Count == 0)
        {
          HardwareTypeTables.ProgFilesRow row = this.ProgFilesTable.NewProgFilesRow();
          row.MapID = this.CommonMapID;
          row.HardwareTypeMapID = this.CommonMapID;
          row.Options = ";Compression=ZIP;";
          if (this.PreDefinedFirmwareVersion.HasValue)
            row.FirmwareVersion = (int) this.PreDefinedFirmwareVersion.Value;
          if (this.HardwareName != null)
            row.HardwareName = this.HardwareName;
          this.ProgFilesTable.AddProgFilesRow(row);
          this.newRowCreated = true;
        }
        this.DataGridOverview.ItemsSource = (IEnumerable) this.ProgFilesTable;
      }
      catch (Exception ex)
      {
        throw new Exception("ProgFiles tabel error. Maybe old table format", ex);
      }
    }

    private void DataGridOverview_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      this.ignoreEditChanges = true;
      try
      {
        if (this.DataGridOverview.SelectedItem == null)
          return;
        this.ClearEditBoxes();
        HardwareTypeTables.ProgFilesRow row = (HardwareTypeTables.ProgFilesRow) ((DataRowView) this.DataGridOverview.SelectedItem).Row;
        this.firmwareVersionUnchanged = row.IsFirmwareVersionNull() ? (!this.PreDefinedFirmwareVersion.HasValue ? "" : this.PreDefinedFirmwareVersion.Value.ToString("x08")) : row.FirmwareVersion.ToString("x08");
        if (this.TextBoxFirmwareVersion.Text != this.firmwareVersionUnchanged)
          this.TextBoxFirmwareVersion.Text = this.firmwareVersionUnchanged;
        if (!row.IsReleasedNameNull())
          this.TextBoxReleasedName.Text = row.ReleasedName;
        if (!row.IsCompatibleOverwriteGroupsNull())
          this.TextBoxCompatibleOverwriteGroups.Text = row.CompatibleOverwriteGroups;
        if (!row.IsOptionsNull())
        {
          List<KeyValuePair<string, string>> keyValuePairList = DbUtil.KeyValueStringListToKeyValuePairList(row.Options);
          this.CheckBoxNoCompression.IsChecked = new bool?(keyValuePairList.FindIndex((Predicate<KeyValuePair<string, string>>) (x => x.Key == "Compression")) < 0);
          this.TextBoxOptions.Text = DbUtil.KeyValuePairListToKeyValueStringList(keyValuePairList);
        }
        if (!row.IsSourceInfoNull())
          this.TextBoxSourceInfo.Text = row.SourceInfo;
        if (!row.IsReleaseCommentsNull())
          this.TextBoxReleaseComments.Text = row.ReleaseComments;
        this.TextBoxFirmwareDependencies.Text = this.GetDependenciesText(row);
        this.StackPanelEditBoxes.IsEnabled = true;
        this.TextBoxReleaseComments.IsEnabled = true;
      }
      finally
      {
        this.CheckDataChanged(true);
        this.ignoreEditChanges = false;
      }
    }

    private string GetDependenciesText(HardwareTypeTables.ProgFilesRow progFilesRow)
    {
      string dependenciesText = "";
      if (!progFilesRow.IsFirmwareDependenciesNull())
      {
        if (progFilesRow.HardwareName != "Bootloader")
        {
          dependenciesText = progFilesRow.FirmwareDependencies;
        }
        else
        {
          StringBuilder stringBuilder = new StringBuilder();
          string firmwareDependencies = progFilesRow.FirmwareDependencies;
          char[] separator = new char[1]{ ';' };
          foreach (string versionString in firmwareDependencies.Split(separator, StringSplitOptions.RemoveEmptyEntries))
          {
            FirmwareVersion firmwareVersion = new FirmwareVersion(versionString);
            stringBuilder.Append(";" + firmwareVersion.ToString());
          }
          if (stringBuilder.Length > 0)
          {
            stringBuilder.Append(";");
            dependenciesText = stringBuilder.ToString();
          }
        }
      }
      return dependenciesText;
    }

    private void ClearEditBoxes()
    {
      this.TextBoxReleasedName.Clear();
      this.TextBoxCompatibleOverwriteGroups.Clear();
      this.TextBoxOptions.Clear();
      this.TextBoxSourceInfo.Clear();
      this.TextBoxReleaseComments.Clear();
    }

    private void ButtonSaveChanges_Click(object sender, RoutedEventArgs e)
    {
      DbBasis.PrimaryDB.BaseDbConnection.ExceptionIfNoSvnDatabase();
      try
      {
        this.CompareStates.Clear();
        if (this.DataGridOverview.SelectedItem == null)
          return;
        HardwareTypeTables.ProgFilesRow row = (HardwareTypeTables.ProgFilesRow) ((DataRowView) this.DataGridOverview.SelectedItem).Row;
        this.SetDataFromEditFields(row);
        this.ProgFilesDataAdapter.Update((DataTable) this.ProgFilesTable);
        this.LoadData();
        this.SetSelectedRowFromVersion(row.FirmwareVersion);
        int num = (int) MessageBox.Show("Data saved");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonDeleteSelectedFirmware_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.CompareStates.Clear();
        if (this.DataGridOverview.SelectedItem == null)
          return;
        ((DataRowView) this.DataGridOverview.SelectedItem).Row.Delete();
        this.ProgFilesDataAdapter.Update((DataTable) this.ProgFilesTable);
        this.LoadData();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonDeleteSelectedFirmwareFile_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.CompareStates.Clear();
        if (this.DataGridOverview.SelectedItem == null)
          return;
        HardwareTypeTables.ProgFilesRow row = (HardwareTypeTables.ProgFilesRow) ((DataRowView) this.DataGridOverview.SelectedItem).Row;
        row.SetProgFileNameNull();
        row.SetHexTextNull();
        this.ProgFilesDataAdapter.Update((DataTable) this.ProgFilesTable);
        this.LoadData();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonAddFirmware_Click(object sender, RoutedEventArgs e)
    {
      DbBasis.PrimaryDB.BaseDbConnection.ExceptionIfNoSvnDatabase();
      try
      {
        this.CompareStates.Clear();
        int newId = DbBasis.PrimaryDB.BaseDbConnection.GetNewId("ProgFiles");
        HardwareTypeTables.ProgFilesRow progFilesRow = this.ProgFilesTable.NewProgFilesRow();
        progFilesRow.MapID = newId;
        progFilesRow.HardwareTypeMapID = this.CommonMapID;
        progFilesRow.HardwareName = this.HardwareName;
        this.SetDataFromEditFields(progFilesRow);
        progFilesRow.SourceInfo = new FirmwareVersion((uint) progFilesRow.FirmwareVersion).ToString();
        this.ProgFilesTable.AddProgFilesRow(progFilesRow);
        this.ProgFilesDataAdapter.Update((DataTable) this.ProgFilesTable);
        this.LoadData();
        this.SetSelectedRowFromVersion(progFilesRow.FirmwareVersion);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void SetDataFromEditFields(HardwareTypeTables.ProgFilesRow theRow)
    {
      theRow.Options = string.IsNullOrEmpty(this.TextBoxOptions.Text) ? "" : this.GetOptionsString();
      theRow.FirmwareVersion = int.Parse(this.TextBoxFirmwareVersion.Text, NumberStyles.HexNumber);
      if (!string.IsNullOrEmpty(this.TextBoxReleasedName.Text))
        theRow.ReleasedName = this.TextBoxReleasedName.Text;
      else
        theRow.SetReleasedNameNull();
      if (!string.IsNullOrEmpty(this.TextBoxReleaseComments.Text))
        theRow.ReleaseComments = this.TextBoxReleaseComments.Text;
      else
        theRow.SetReleaseCommentsNull();
      if (!string.IsNullOrEmpty(this.TextBoxFirmwareDependencies.Text))
      {
        if (theRow.HardwareName == "Bootloader")
        {
          StringBuilder stringBuilder = new StringBuilder();
          string text = this.TextBoxFirmwareDependencies.Text;
          char[] separator = new char[1]{ ';' };
          foreach (string versionString in text.Split(separator, StringSplitOptions.RemoveEmptyEntries))
          {
            FirmwareVersion firmwareVersion = new FirmwareVersion(versionString);
            stringBuilder.Append(";" + firmwareVersion.Version.ToString("x08"));
          }
          if (stringBuilder.Length > 0)
            theRow.FirmwareDependencies = stringBuilder.ToString() + ";";
          else
            theRow.SetFirmwareDependenciesNull();
        }
        else
          theRow.FirmwareDependencies = this.TextBoxFirmwareDependencies.Text;
      }
      else
        theRow.SetFirmwareDependenciesNull();
      if (!string.IsNullOrEmpty(this.TextBoxCompatibleOverwriteGroups.Text))
        theRow.CompatibleOverwriteGroups = this.TextBoxCompatibleOverwriteGroups.Text;
      else
        theRow.SetCompatibleOverwriteGroupsNull();
    }

    private void ChangeSelectedIndex(int newIndex)
    {
      if (this.DataGridOverview.SelectedIndex == newIndex)
        return;
      this.ignoreEditChanges = true;
      this.DataGridOverview.SelectedIndex = newIndex;
      this.ignoreEditChanges = false;
    }

    private void CheckDataChanged(bool afterSelectionCheck = false)
    {
      string str = (string) null;
      bool flag = true;
      try
      {
        if (this.TextBoxFirmwareVersion.Text.Length != 8)
        {
          str = "Illegal firmware length. 8 digits required.";
          this.ChangeSelectedIndex(-1);
          goto label_40;
        }
        else
        {
          str = "Illegal firmware value";
          uint versionValue = uint.Parse(this.TextBoxFirmwareVersion.Text, NumberStyles.HexNumber);
          str = "Illegal new firmware struct";
          FirmwareVersion firmwareVersion1 = new FirmwareVersion(versionValue);
          str = "";
          flag = false;
          if (this.DataGridOverview.SelectedItem == null)
          {
            str = "New firmware";
            goto label_40;
          }
          else
          {
            str = "Illegal old firmware struct";
            FirmwareVersion firmwareVersion2 = new FirmwareVersion(uint.Parse(this.firmwareVersionUnchanged, NumberStyles.HexNumber));
            if (this.newRowCreated)
            {
              str = "Data created.";
              goto label_40;
            }
            else
            {
              int newIndex = -1;
              HardwareTypeTables.ProgFilesRow[] progFilesRowArray = (HardwareTypeTables.ProgFilesRow[]) this.ProgFilesTable.Select("FirmwareVersion = " + versionValue.ToString());
              if (progFilesRowArray.Length != 0)
              {
                for (int index = 0; index < progFilesRowArray.Length; ++index)
                {
                  if (((DataRowView) this.DataGridOverview.SelectedItem).Row == progFilesRowArray[index])
                  {
                    newIndex = this.DataGridOverview.SelectedIndex;
                    break;
                  }
                }
              }
              this.ChangeSelectedIndex(newIndex);
            }
          }
        }
      }
      catch (Exception ex)
      {
        str = str + "; " + ex.Message;
        goto label_40;
      }
      HardwareTypeTables.ProgFilesRow row1 = (HardwareTypeTables.ProgFilesRow) ((DataRowView) this.DataGridOverview.SelectedItem).Row;
      if (row1.IsOptionsNull())
      {
        if (this.TextBoxOptions.Text != "")
          goto label_40;
      }
      else if (!afterSelectionCheck && row1.Options != this.TextBoxOptions.Text)
        goto label_40;
      int result;
      if (this.TextBoxFirmwareVersion.Text.Length == 8 && int.TryParse(this.TextBoxFirmwareVersion.Text, NumberStyles.HexNumber, (IFormatProvider) null, out result) && !row1.IsFirmwareVersionNull() && result == row1.FirmwareVersion)
      {
        if (row1.IsReleasedNameNull())
        {
          if (this.TextBoxReleasedName.Text != "")
            goto label_40;
        }
        else if (row1.ReleasedName != this.TextBoxReleasedName.Text)
          goto label_40;
        if (row1.IsReleaseCommentsNull())
        {
          if (this.TextBoxReleaseComments.Text != "")
            goto label_40;
        }
        else if (row1.ReleaseComments != this.TextBoxReleaseComments.Text)
          goto label_40;
        if (!(this.GetDependenciesText(row1) != this.TextBoxFirmwareDependencies.Text))
        {
          if (row1.IsCompatibleOverwriteGroupsNull())
          {
            if (this.TextBoxCompatibleOverwriteGroups.Text != "")
              goto label_40;
          }
          else if (row1.CompatibleOverwriteGroups != this.TextBoxCompatibleOverwriteGroups.Text)
            goto label_40;
          this.ButtonSaveChanges.IsEnabled = false;
          this.ButtonAddFirmware.IsEnabled = false;
          this.ButtonImportProgrammerFile.IsEnabled = true;
          if (!row1.IsProgFileNameNull() && row1.ProgFileName.Length > 0)
          {
            this.ButtonExportProgrammerFile.IsEnabled = true;
            this.ButtonShowProgrammerFile.IsEnabled = true;
          }
          else
          {
            this.ButtonExportProgrammerFile.IsEnabled = false;
            this.ButtonShowProgrammerFile.IsEnabled = false;
          }
          this.ButtonDeleteSelectedFirmwareFile.IsEnabled = true;
          if (row1.IsHardwareTypeMapIDNull() || row1.HardwareTypeMapID == row1.MapID)
            this.ButtonDeleteSelectedFirmware.IsEnabled = false;
          else
            this.ButtonDeleteSelectedFirmware.IsEnabled = true;
          this.ButtonCompareSelected.IsEnabled = true;
          this.ButtonCompareAll.IsEnabled = true;
          if (!this.CompareStates.ContainsKey(row1.MapID) || this.CompareStates[row1.MapID] == CompatibleFirmwareWindow.RowCompareState.equal)
            this.ButtonCopySelected.IsEnabled = false;
          else
            this.ButtonCopySelected.IsEnabled = true;
          this.LabelStatus.Content = (object) "No changes";
          return;
        }
      }
label_40:
      this.ButtonCompareSelected.IsEnabled = false;
      this.ButtonCompareAll.IsEnabled = false;
      this.ButtonCopySelected.IsEnabled = false;
      if (flag)
      {
        this.ButtonSaveChanges.IsEnabled = false;
        this.ButtonAddFirmware.IsEnabled = false;
      }
      else if (this.newRowCreated)
      {
        this.ButtonSaveChanges.IsEnabled = true;
        this.ButtonAddFirmware.IsEnabled = false;
      }
      else if (this.DataGridOverview.SelectedItem != null)
      {
        HardwareTypeTables.ProgFilesRow row2 = (HardwareTypeTables.ProgFilesRow) ((DataRowView) this.DataGridOverview.SelectedItem).Row;
        if (row2.IsOptionsNull())
        {
          if (this.TextBoxOptions.Text.Trim().Length > 0)
            this.ButtonAddFirmware.IsEnabled = true;
          else
            this.ButtonAddFirmware.IsEnabled = false;
        }
        else if (row2.Options != this.TextBoxOptions.Text.Trim())
          this.ButtonAddFirmware.IsEnabled = true;
        else
          this.ButtonAddFirmware.IsEnabled = false;
        this.ButtonSaveChanges.IsEnabled = true;
      }
      else
      {
        this.ButtonSaveChanges.IsEnabled = false;
        this.ButtonAddFirmware.IsEnabled = true;
      }
      this.ButtonImportProgrammerFile.IsEnabled = false;
      this.ButtonExportProgrammerFile.IsEnabled = false;
      this.ButtonShowProgrammerFile.IsEnabled = false;
      this.ButtonDeleteSelectedFirmware.IsEnabled = false;
      this.ButtonDeleteSelectedFirmwareFile.IsEnabled = false;
      if (str != null)
        this.LabelStatus.Content = (object) str;
      else
        this.LabelStatus.Content = (object) "";
    }

    private string GetOptionsString()
    {
      List<KeyValuePair<string, string>> keyValuePairList = DbUtil.KeyValueStringListToKeyValuePairList(this.TextBoxOptions.Text);
      int index = keyValuePairList.FindIndex((Predicate<KeyValuePair<string, string>>) (x => x.Key == "Compression"));
      if (index >= 0)
        keyValuePairList.RemoveAt(index);
      if (!this.CheckBoxNoCompression.IsChecked.Value)
        keyValuePairList.Add(new KeyValuePair<string, string>("Compression", "ZIP"));
      return DbUtil.KeyValuePairListToKeyValueStringList(keyValuePairList);
    }

    private void ButtonImportProgrammerFile_Click(object sender, RoutedEventArgs e)
    {
      this.ChangeProgrammerFile();
      this.CompareStates.Clear();
    }

    private void ButtonExportProgrammerFile_Click(object sender, RoutedEventArgs e)
    {
      if (this.DataGridOverview.SelectedItem == null)
        return;
      try
      {
        FirmwareData firmwareData = HardwareTypeSupport.GetFirmwareData((uint) ((HardwareTypeTables.ProgFilesRow) ((DataRowView) this.DataGridOverview.SelectedItem).Row).MapID);
        if (firmwareData == null || firmwareData.ProgrammerFileAsString == null)
        {
          int num = (int) MessageBox.Show("No data in database");
        }
        else
        {
          SaveFileDialog saveFileDialog = new SaveFileDialog();
          saveFileDialog.Filter = "All files (*.*)|*.*";
          saveFileDialog.FilterIndex = 1;
          saveFileDialog.RestoreDirectory = true;
          if (!string.IsNullOrEmpty(firmwareData.ProgFileName))
            saveFileDialog.FileName = firmwareData.ProgFileName;
          if (!saveFileDialog.ShowDialog().Value)
            return;
          firmwareData.WriteToFile(saveFileDialog.FileName);
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Change programmer file error");
      }
    }

    private void ButtonShowProgrammerFile_Click(object sender, RoutedEventArgs e)
    {
      if (this.DataGridOverview.SelectedItem == null)
        return;
      try
      {
        FirmwareData firmwareData = HardwareTypeSupport.GetFirmwareData((uint) ((HardwareTypeTables.ProgFilesRow) ((DataRowView) this.DataGridOverview.SelectedItem).Row).MapID);
        if (firmwareData == null || firmwareData.ProgrammerFileAsString == null)
        {
          int num = (int) MessageBox.Show("No data in database");
        }
        else
          GmmMessage.Show_Ok(firmwareData.ProgrammerFileAsString);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Show programmer file error");
      }
    }

    private void ChangeProgrammerFile()
    {
      DbBasis.PrimaryDB.BaseDbConnection.ExceptionIfNoSvnDatabase();
      if (this.DataGridOverview.SelectedItem == null)
        return;
      HardwareTypeTables.ProgFilesRow row1 = (HardwareTypeTables.ProgFilesRow) ((DataRowView) this.DataGridOverview.SelectedItem).Row;
      try
      {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "All files (*.*)|*.*";
        openFileDialog.FilterIndex = 1;
        openFileDialog.RestoreDirectory = true;
        if (!openFileDialog.ShowDialog().Value)
          return;
        string end;
        using (StreamReader streamReader = new StreamReader(openFileDialog.FileName))
          end = streamReader.ReadToEnd();
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          List<KeyValuePair<string, string>> keyValuePairList = DbUtil.KeyValueStringListToKeyValuePairList(this.TextBoxOptions.Text);
          int index = keyValuePairList.FindIndex((Predicate<KeyValuePair<string, string>>) (x => x.Key == "Compression"));
          if (index >= 0)
            keyValuePairList.RemoveAt(index);
          bool? isChecked = this.CheckBoxNoCompression.IsChecked;
          if (!isChecked.Value)
            keyValuePairList.Add(new KeyValuePair<string, string>("Compression", "ZIP"));
          string keyValueStringList = DbUtil.KeyValuePairListToKeyValueStringList(keyValuePairList);
          string selectSql1 = "SELECT * FROM ProgFiles WHERE MapID = " + row1.MapID.ToString();
          DbCommandBuilder commandBuilder;
          DbDataAdapter dataAdapter1 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql1, newConnection, out commandBuilder);
          HardwareTypeTables.ProgFilesDataTable progFilesDataTable1 = new HardwareTypeTables.ProgFilesDataTable();
          dataAdapter1.Fill((DataTable) progFilesDataTable1);
          HardwareTypeTables.ProgFilesRow row2;
          if (progFilesDataTable1.Count > 0)
          {
            row2 = progFilesDataTable1[0];
            if (keyValueStringList != row2.Options)
            {
              if (MessageBox.Show("old Options:" + Environment.NewLine + row2.Options + Environment.NewLine + Environment.NewLine + "new Options:" + Environment.NewLine + keyValueStringList + Environment.NewLine + Environment.NewLine + "Change to new options?", "Change ProgFiles Options", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                return;
            }
            row2.Options = keyValueStringList;
          }
          else
          {
            row2 = progFilesDataTable1.NewProgFilesRow();
            row2.MapID = row1.MapID;
            row2.Options = keyValueStringList;
            progFilesDataTable1.AddProgFilesRow(row2);
          }
          isChecked = this.CheckBoxNoCompression.IsChecked;
          row2.HexText = isChecked.Value ? end : ZipUnzip.GetZipedStringFromString(end);
          string fileName = Path.GetFileName(openFileDialog.FileName);
          row2.ProgFileName = fileName;
          if (!row1.IsFirmwareVersionNull())
          {
            FirmwareVersion firmwareVersion = new FirmwareVersion((uint) row1.FirmwareVersion);
            row2.SourceInfo = firmwareVersion.ToString() + "; " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
          }
          dataAdapter1.Update((DataTable) progFilesDataTable1);
          string selectSql2 = "SELECT * FROM ProgFiles WHERE MapID = " + row1.MapID.ToString();
          DbDataAdapter dataAdapter2 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql2, newConnection, out commandBuilder);
          HardwareTypeTables.ProgFilesDataTable progFilesDataTable2 = new HardwareTypeTables.ProgFilesDataTable();
          dataAdapter2.Fill((DataTable) progFilesDataTable2);
          isChecked = this.CheckBoxNoCompression.IsChecked;
          if (isChecked.Value)
          {
            if (progFilesDataTable2[0].HexText != end)
              throw new Exception("VERIFY creates an error");
          }
          else if (ZipUnzip.GetStringFromZipedString(progFilesDataTable2[0].HexText) != end)
            throw new Exception("Zip-Unzip VERIFY creates an error");
          int num = (int) MessageBox.Show("Programer file changed");
          this.LoadData();
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Change programmer file error");
      }
    }

    private void ButtonUseFirmwareFromHandler_Click(object sender, RoutedEventArgs e)
    {
      this.TextBoxFirmwareVersion.Text = this.TextBoxFirmwareFromHandler.Text;
    }

    private void CheckBoxNoCompression_Checked(object sender, RoutedEventArgs e)
    {
      this.TextBoxOptions.Text = this.GetOptionsString();
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.ignoreEditChanges)
        return;
      this.CheckDataChanged();
    }

    private void ButtonPrepareTable_Click(object sender, RoutedEventArgs e) => this.PrepareTable();

    private void PrepareTable()
    {
      int num = 0;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("SELECT HardwareTypeID, MapID, FirmwareVersion, HardwareName, HardwareResource FROM HardwareType");
      DbDataAdapter dataAdapter1 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), this.myDbConnection);
      HardwareTypeTables.HardwareTypeDataTable hardwareTypeDataTable = new HardwareTypeTables.HardwareTypeDataTable();
      dataAdapter1.Fill((DataTable) hardwareTypeDataTable);
      stringBuilder.Clear();
      stringBuilder.Append("SELECT MapID, FirmwareVersion, HardwareName, Options, HardwareTypeMapID, CompatibleOverwriteGroups, ReleasedName, FirmwareDependencies");
      stringBuilder.Append(" FROM ProgFiles");
      DbDataAdapter dataAdapter2 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(stringBuilder.ToString(), this.myDbConnection, out DbCommandBuilder _);
      HardwareTypeTables.ProgFilesDataTable progFilesDataTable = new HardwareTypeTables.ProgFilesDataTable();
      dataAdapter2.Fill((DataTable) progFilesDataTable);
      foreach (HardwareTypeTables.ProgFilesRow progFilesRow in (TypedTableBase<HardwareTypeTables.ProgFilesRow>) progFilesDataTable)
      {
        if (progFilesRow.IsFirmwareVersionNull() || progFilesRow.IsHardwareNameNull())
        {
          HardwareTypeTables.HardwareTypeRow[] hardwareTypeRowArray = (HardwareTypeTables.HardwareTypeRow[]) hardwareTypeDataTable.Select("MapID = " + progFilesRow.MapID.ToString());
          if (hardwareTypeRowArray.Length != 0)
          {
            if (!hardwareTypeRowArray[0].IsFirmwareVersionNull())
            {
              progFilesRow.FirmwareVersion = hardwareTypeRowArray[0].FirmwareVersion;
              ++num;
            }
            if (!hardwareTypeRowArray[0].IsHardwareNameNull())
            {
              progFilesRow.HardwareName = hardwareTypeRowArray[0].HardwareName;
              ++num;
            }
          }
        }
        if (progFilesRow.IsHardwareTypeMapIDNull())
        {
          progFilesRow.HardwareTypeMapID = progFilesRow.MapID;
          ++num;
        }
        if (!progFilesRow.IsOptionsNull() && progFilesRow.IsReleasedNameNull() && progFilesRow.IsCompatibleOverwriteGroupsNull())
        {
          List<KeyValuePair<string, string>> keyValuePairList = DbUtil.KeyValueStringListToKeyValuePairList(progFilesRow.Options);
          string valueForKey1 = DbUtil.GetValueForKey("Released", keyValuePairList);
          if (valueForKey1 != null)
          {
            string str = valueForKey1.Replace(";", "");
            if (str.ToUpper() != "NO" && str.ToUpper() != "OLD")
            {
              progFilesRow.ReleasedName = str;
              ++num;
            }
          }
          string valueForKey2 = DbUtil.GetValueForKey("CompatibleMapId", keyValuePairList);
          if (valueForKey2 != null)
          {
            progFilesRow.CompatibleOverwriteGroups = valueForKey2;
            ++num;
          }
        }
        if (!progFilesRow.IsHardwareNameNull() && progFilesRow.HardwareName == "Bootloader" && progFilesRow.IsFirmwareDependenciesNull())
        {
          HardwareTypeTables.HardwareTypeRow[] hardwareTypeRowArray = (HardwareTypeTables.HardwareTypeRow[]) hardwareTypeDataTable.Select("MapID = " + progFilesRow.MapID.ToString());
          if (hardwareTypeRowArray.Length == 1 && !hardwareTypeRowArray[0].IsHardwareResourceNull())
          {
            progFilesRow.FirmwareDependencies = hardwareTypeRowArray[0].HardwareResource;
            ++num;
          }
        }
      }
      dataAdapter2.Update((DataTable) progFilesDataTable);
      GmmMessage.Show_Ok("Table prepared. Values copied: " + num.ToString());
      this.LoadData();
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
        HardwareTypeTables.ProgFilesRow row = (HardwareTypeTables.ProgFilesRow) ((DataRowView) this.DataGridOverview.SelectedItem).Row;
        using (DbConnection newConnection = DbBasis.SecondaryDB.BaseDbConnection.GetNewConnection())
          this.GetCompareData(row, newConnection, message);
        this.LoadData();
        this.SetSelectedRowFromVersion(row.FirmwareVersion);
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
        this.GuarantDatabasesAndGetInfo(message);
        message.AppendLine("*** Compare result ***");
        HardwareTypeTables.ProgFilesRow row1 = (HardwareTypeTables.ProgFilesRow) ((DataRowView) this.DataGridOverview.SelectedItem).Row;
        using (DbConnection newConnection = DbBasis.SecondaryDB.BaseDbConnection.GetNewConnection())
        {
          foreach (DataRowView dataRowView in (IEnumerable) this.DataGridOverview.Items)
          {
            HardwareTypeTables.ProgFilesRow row2 = (HardwareTypeTables.ProgFilesRow) dataRowView.Row;
            message.AppendLine("MapID: " + row2.MapID.ToString());
            this.GetCompareData(row2, newConnection, message);
            message.AppendLine();
          }
        }
        this.LoadData();
        this.SetSelectedRowFromVersion(row1.FirmwareVersion);
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
        HardwareTypeTables.ProgFilesRow row1 = (HardwareTypeTables.ProgFilesRow) ((DataRowView) this.DataGridOverview.SelectedItem).Row;
        using (DbConnection newConnection = DbBasis.SecondaryDB.BaseDbConnection.GetNewConnection())
        {
          string selectSql = "SELECT * FROM ProgFiles WHERE MapID = " + row1.MapID.ToString();
          DbDataAdapter dataAdapter1 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql, this.myDbConnection);
          HardwareTypeTables.ProgFilesDataTable progFilesDataTable1 = new HardwareTypeTables.ProgFilesDataTable();
          dataAdapter1.Fill((DataTable) progFilesDataTable1);
          DbDataAdapter dataAdapter2 = DbBasis.SecondaryDB.BaseDbConnection.GetDataAdapter(selectSql, newConnection, out DbCommandBuilder _);
          HardwareTypeTables.ProgFilesDataTable progFilesDataTable2 = new HardwareTypeTables.ProgFilesDataTable();
          dataAdapter2.Fill((DataTable) progFilesDataTable2);
          HardwareTypeTables.ProgFilesRow progFilesRow = progFilesDataTable1.Count == 1 ? progFilesDataTable1[0] : throw new Exception("Could not load primary data");
          HardwareTypeTables.ProgFilesRow row2;
          if (progFilesDataTable2.Count == 1)
            row2 = progFilesDataTable2[0];
          else
            row2 = progFilesDataTable2.Count == 0 ? progFilesDataTable2.NewProgFilesRow() : throw new Exception("Illegal nubers of secondary rows");
          foreach (DataColumn column in (InternalDataCollectionBase) row1.Table.Columns)
            row2[column.ColumnName] = progFilesRow[column.ColumnName];
          if (progFilesDataTable2.Count == 0)
            progFilesDataTable2.AddProgFilesRow(row2);
          dataAdapter2.Update((DataTable) progFilesDataTable2);
          message.AppendLine("Copy done.");
        }
        this.CompareStates[row1.MapID] = CompatibleFirmwareWindow.RowCompareState.equal;
        this.LoadData();
        this.SetSelectedRowFromVersion(row1.FirmwareVersion);
        GmmMessage.Show_Ok(message.ToString());
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void GetCompareData(
      HardwareTypeTables.ProgFilesRow progFilesRow,
      DbConnection secDbConnection,
      StringBuilder message)
    {
      if (!this.CompareStates.ContainsKey(progFilesRow.MapID))
        this.CompareStates.Add(progFilesRow.MapID, CompatibleFirmwareWindow.RowCompareState.equal);
      else
        this.CompareStates[progFilesRow.MapID] = CompatibleFirmwareWindow.RowCompareState.equal;
      string selectSql = "SELECT * FROM ProgFiles WHERE MapID = " + progFilesRow.MapID.ToString();
      DbDataAdapter dataAdapter1 = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql, this.myDbConnection);
      HardwareTypeTables.ProgFilesDataTable progFilesDataTable1 = new HardwareTypeTables.ProgFilesDataTable();
      dataAdapter1.Fill((DataTable) progFilesDataTable1);
      DbDataAdapter dataAdapter2 = DbBasis.SecondaryDB.BaseDbConnection.GetDataAdapter(selectSql, secDbConnection);
      HardwareTypeTables.ProgFilesDataTable progFilesDataTable2 = new HardwareTypeTables.ProgFilesDataTable();
      dataAdapter2.Fill((DataTable) progFilesDataTable2);
      if (progFilesDataTable1.Count != 1)
        throw new Exception("Could not load primary data");
      if (progFilesDataTable2.Count != 1)
      {
        this.CompareStates[progFilesRow.MapID] = CompatibleFirmwareWindow.RowCompareState.notAvailable;
        message.AppendLine("Second data not available");
      }
      else
      {
        foreach (DataColumn column in (InternalDataCollectionBase) progFilesRow.Table.Columns)
        {
          if (progFilesDataTable1[0][column.ColumnName] == DBNull.Value)
          {
            if (progFilesDataTable2[0][column.ColumnName] != DBNull.Value)
            {
              this.CompareStates[progFilesRow.MapID] = CompatibleFirmwareWindow.RowCompareState.different;
              message.AppendLine("Column: " + column.ColumnName + " -> PrimaryDB data not available");
            }
          }
          else if (progFilesDataTable2[0][column.ColumnName] == DBNull.Value)
          {
            this.CompareStates[progFilesRow.MapID] = CompatibleFirmwareWindow.RowCompareState.different;
            message.AppendLine("Column: " + column.ColumnName + " -> SecondaryDB data not available");
          }
          else
          {
            string str1 = progFilesDataTable1[0][column.ColumnName].ToString();
            string str2 = progFilesDataTable2[0][column.ColumnName].ToString();
            if (str2 != str1)
            {
              this.CompareStates[progFilesRow.MapID] = CompatibleFirmwareWindow.RowCompareState.different;
              message.AppendLine("Column: " + column.ColumnName + " -> Data different.");
              if (str1.Length < 100 && str2.Length < 100)
              {
                message.AppendLine("   Primary: " + str1);
                message.AppendLine("   Secondary: " + str2);
              }
            }
          }
        }
      }
      if (this.CompareStates[progFilesRow.MapID] != CompatibleFirmwareWindow.RowCompareState.equal)
        return;
      message.AppendLine("Data are equal");
      this.CompareStates[progFilesRow.MapID] = CompatibleFirmwareWindow.RowCompareState.equal;
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

    private void DataGridOverview_LoadingRow(object sender, DataGridRowEventArgs e)
    {
      HardwareTypeTables.ProgFilesRow row = (HardwareTypeTables.ProgFilesRow) ((DataRowView) e.Row.DataContext).Row;
      if (!this.CompareStates.ContainsKey(row.MapID))
        return;
      switch (this.CompareStates[row.MapID])
      {
        case CompatibleFirmwareWindow.RowCompareState.checkError:
          e.Row.Background = (Brush) Brushes.Red;
          break;
        case CompatibleFirmwareWindow.RowCompareState.equal:
          e.Row.Background = (Brush) Brushes.LightGreen;
          break;
        case CompatibleFirmwareWindow.RowCompareState.different:
          e.Row.Background = (Brush) Brushes.Yellow;
          break;
        case CompatibleFirmwareWindow.RowCompareState.notAvailable:
          e.Row.Background = (Brush) Brushes.Orange;
          break;
      }
    }

    private void ButtonClearReleased_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.DataGridOverview.SelectedItem == null)
          return;
        this.CompareStates.Clear();
        this.TextBoxReleasedName.Clear();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonSetReleased_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.DataGridOverview.SelectedItem == null)
          return;
        this.CompareStates.Clear();
        HardwareTypeTables.ProgFilesRow row = (HardwareTypeTables.ProgFilesRow) ((DataRowView) this.DataGridOverview.SelectedItem).Row;
        this.TextBoxReleasedName.Text = "Released for production";
        foreach (HardwareTypeTables.ProgFilesRow progFilesRow in (TypedTableBase<HardwareTypeTables.ProgFilesRow>) this.ProgFilesTable)
        {
          if (progFilesRow != row && !progFilesRow.IsReleasedNameNull() && progFilesRow.ReleasedName == "Released for production")
            progFilesRow.SetReleasedNameNull();
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void TextBoxFirmwareDependencies_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      this.doSelectFirmwareForBootloader();
    }

    private void ButtonFirmwareDependencies_Click(object sender, RoutedEventArgs e)
    {
      this.doSelectFirmwareForBootloader();
    }

    private void doSelectFirmwareForBootloader()
    {
      List<string> source = new List<string>();
      List<string> dependencysCleanUp = new List<string>();
      string str1 = string.Empty;
      bool flag = this.CheckBoxFirmwareDependenciesCleanUp.IsChecked.HasValue && this.CheckBoxFirmwareDependenciesCleanUp.IsChecked.Value;
      HardwareTypeTables.ProgFilesRow row1 = (HardwareTypeTables.ProgFilesRow) ((DataRowView) this.DataGridOverview.SelectedItem).Row;
      if (!row1.IsNull("FirmwareDependencies") && !string.IsNullOrEmpty(row1.Field<string>("FirmwareDependencies")))
      {
        str1 = row1.Field<string>("FirmwareDependencies").ToUpper();
        source = ((IEnumerable<string>) str1.Split(';')).ToList<string>();
      }
      string selectSql = "SELECT HardwareName, FirmwareVersion, FirmwareDependencies FROM ProgFiles WHERE HardwareName not like '%bootloader%' ORDER BY HardwareName, FirmwareVersion";
      DataTable dataTable = new DataTable();
      dataTable.Clear();
      dataTable.Columns.Add("HardwareName", typeof (string));
      dataTable.Columns.Add("FirmwareVersion", typeof (int));
      dataTable.Columns.Add("FirmwareDependencies", typeof (string));
      DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(selectSql, this.myDbConnection).Fill(dataTable);
      DataTable dependencyFW = new DataTable();
      dependencyFW.Clear();
      dependencyFW.Columns.Add("selected", typeof (bool));
      dependencyFW.Columns.Add("version", typeof (uint));
      dependencyFW.Columns.Add("version_X8", typeof (string));
      dependencyFW.Columns.Add("name", typeof (string));
      foreach (DataRow row2 in (InternalDataCollectionBase) dataTable.Rows)
      {
        uint num = row2.IsNull("FirmwareVersion") ? 0U : (uint) row2.Field<int>("FirmwareVersion");
        if (num > 0U)
        {
          string str2 = num.ToString("X8");
          DataRow row3 = dependencyFW.NewRow();
          row3["selected"] = (object) str1.ToUpper().Contains(str2.ToUpper());
          row3["version"] = (object) num;
          row3["version_X8"] = (object) str2;
          row3["name"] = (object) row2.Field<string>("HardwareName");
          dependencyFW.Rows.Add(row3);
        }
      }
      new CompatibleFirmwareSelectWindow((Window) this, ref dependencyFW).ShowDialog();
      dependencysCleanUp.Clear();
      foreach (DataRow row4 in (InternalDataCollectionBase) dependencyFW.Rows)
      {
        string upper = row4.Field<string>("version_X8").ToUpper();
        if (source.Contains(upper) && !row4.Field<bool>("selected"))
          source.Remove(upper);
        if (!source.Contains(upper) && row4.Field<bool>("selected"))
          source.Add(upper);
        dependencysCleanUp.Add(upper);
      }
      if (flag)
        source = source.Where<string>((System.Func<string, bool>) (x => dependencysCleanUp.Contains(x))).ToList<string>();
      string str3 = string.Empty;
      string str4 = string.Empty;
      foreach (string versionString in source)
      {
        if (!string.IsNullOrEmpty(versionString))
        {
          str3 = str3 + versionString + ";";
          FirmwareVersion firmwareVersion = new FirmwareVersion(versionString);
          str4 = str4 + firmwareVersion.ToString() + ";";
        }
      }
      row1["FirmwareDependencies"] = (object) str3;
      row1.AcceptChanges();
      this.TextBoxFirmwareDependencies.Text = str4;
      this.UpdateLayout();
    }

    private void ButtonTableToExcel_Click(object sender, RoutedEventArgs e)
    {
      ExcelConnect excelConnect = new ExcelConnect();
      excelConnect.AddTable((DataTable) this.ProgFilesTable, "IUW firmware versions", false, false);
      excelConnect.ShowWorkbook();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/hardwaremanagement/compatiblefirmwarewindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.StackPanelButtons = (StackPanel) target;
          break;
        case 2:
          this.ButtonPrepareTable = (Button) target;
          this.ButtonPrepareTable.Click += new RoutedEventHandler(this.ButtonPrepareTable_Click);
          break;
        case 3:
          this.ButtonImportProgrammerFile = (Button) target;
          this.ButtonImportProgrammerFile.Click += new RoutedEventHandler(this.ButtonImportProgrammerFile_Click);
          break;
        case 4:
          this.ButtonExportProgrammerFile = (Button) target;
          this.ButtonExportProgrammerFile.Click += new RoutedEventHandler(this.ButtonExportProgrammerFile_Click);
          break;
        case 5:
          this.ButtonTableToExcel = (Button) target;
          this.ButtonTableToExcel.Click += new RoutedEventHandler(this.ButtonTableToExcel_Click);
          break;
        case 6:
          this.ButtonShowProgrammerFile = (Button) target;
          this.ButtonShowProgrammerFile.Click += new RoutedEventHandler(this.ButtonShowProgrammerFile_Click);
          break;
        case 7:
          this.ButtonDeleteSelectedFirmware = (Button) target;
          this.ButtonDeleteSelectedFirmware.Click += new RoutedEventHandler(this.ButtonDeleteSelectedFirmware_Click);
          break;
        case 8:
          this.ButtonDeleteSelectedFirmwareFile = (Button) target;
          this.ButtonDeleteSelectedFirmwareFile.Click += new RoutedEventHandler(this.ButtonDeleteSelectedFirmwareFile_Click);
          break;
        case 9:
          this.ButtonAddFirmware = (Button) target;
          this.ButtonAddFirmware.Click += new RoutedEventHandler(this.ButtonAddFirmware_Click);
          break;
        case 10:
          this.ButtonSaveChanges = (Button) target;
          this.ButtonSaveChanges.Click += new RoutedEventHandler(this.ButtonSaveChanges_Click);
          break;
        case 11:
          this.ButtonCompareSelected = (Button) target;
          this.ButtonCompareSelected.Click += new RoutedEventHandler(this.ButtonCompareSelected_Click);
          break;
        case 12:
          this.ButtonCompareAll = (Button) target;
          this.ButtonCompareAll.Click += new RoutedEventHandler(this.ButtonCompareAll_Click);
          break;
        case 13:
          this.ButtonCopySelected = (Button) target;
          this.ButtonCopySelected.Click += new RoutedEventHandler(this.ButtonCopySelected_Click);
          break;
        case 14:
          this.BorderVersionFromHandler = (Border) target;
          break;
        case 15:
          this.TextBoxFirmwareFromHandler = (TextBox) target;
          break;
        case 16:
          this.ButtonUseFirmwareFromHandler = (Button) target;
          this.ButtonUseFirmwareFromHandler.Click += new RoutedEventHandler(this.ButtonUseFirmwareFromHandler_Click);
          break;
        case 17:
          this.StackPanelBottom = (StackPanel) target;
          break;
        case 18:
          this.LabelStatus = (Label) target;
          break;
        case 19:
          this.StackPanelEditBoxes = (StackPanel) target;
          break;
        case 20:
          this.TextBoxFirmwareVersion = (TextBox) target;
          this.TextBoxFirmwareVersion.TextChanged += new TextChangedEventHandler(this.TextBox_TextChanged);
          break;
        case 21:
          this.TextBoxReleasedName = (TextBox) target;
          this.TextBoxReleasedName.TextChanged += new TextChangedEventHandler(this.TextBox_TextChanged);
          break;
        case 22:
          this.ButtonClearReleased = (Button) target;
          this.ButtonClearReleased.Click += new RoutedEventHandler(this.ButtonClearReleased_Click);
          break;
        case 23:
          this.ButtonSetReleased = (Button) target;
          this.ButtonSetReleased.Click += new RoutedEventHandler(this.ButtonSetReleased_Click);
          break;
        case 24:
          this.TextBoxCompatibleOverwriteGroups = (TextBox) target;
          this.TextBoxCompatibleOverwriteGroups.TextChanged += new TextChangedEventHandler(this.TextBox_TextChanged);
          break;
        case 25:
          this.CheckBoxNoCompression = (CheckBox) target;
          this.CheckBoxNoCompression.Checked += new RoutedEventHandler(this.CheckBoxNoCompression_Checked);
          this.CheckBoxNoCompression.Unchecked += new RoutedEventHandler(this.CheckBoxNoCompression_Checked);
          break;
        case 26:
          this.TextBoxOptions = (TextBox) target;
          this.TextBoxOptions.TextChanged += new TextChangedEventHandler(this.TextBox_TextChanged);
          break;
        case 27:
          this.TextBoxSourceInfo = (TextBox) target;
          this.TextBoxSourceInfo.TextChanged += new TextChangedEventHandler(this.TextBox_TextChanged);
          break;
        case 28:
          this.TextBoxReleaseComments = (TextBox) target;
          this.TextBoxReleaseComments.TextChanged += new TextChangedEventHandler(this.TextBox_TextChanged);
          break;
        case 29:
          this.ButtonFirmwareDependencies = (Button) target;
          this.ButtonFirmwareDependencies.Click += new RoutedEventHandler(this.ButtonFirmwareDependencies_Click);
          break;
        case 30:
          this.CheckBoxFirmwareDependenciesCleanUp = (CheckBox) target;
          break;
        case 31:
          this.TextBoxFirmwareDependencies = (TextBox) target;
          this.TextBoxFirmwareDependencies.MouseDoubleClick += new MouseButtonEventHandler(this.TextBoxFirmwareDependencies_MouseDoubleClick);
          this.TextBoxFirmwareDependencies.TextChanged += new TextChangedEventHandler(this.TextBox_TextChanged);
          break;
        case 32:
          this.DataGridOverview = (DataGrid) target;
          this.DataGridOverview.SelectionChanged += new SelectionChangedEventHandler(this.DataGridOverview_SelectionChanged);
          this.DataGridOverview.LoadingRow += new EventHandler<DataGridRowEventArgs>(this.DataGridOverview_LoadingRow);
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

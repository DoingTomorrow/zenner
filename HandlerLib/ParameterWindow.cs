// Decompiled with JetBrains decompiler
// Type: HandlerLib.ParameterWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommonWPF;
using HandlerLib.MapManagement;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public class ParameterWindow : Window, IComponentConnector
  {
    private DeviceMemory actualDeviceMemory;
    private List<myDataRow> localDataSourceBACKUP;
    private List<myDataRow> localDataSourceSELECT;
    private SortedList<string, Parameter32bit> FirmwareParameter;
    private BaseMemoryAccess memoryAccess;
    private CancellationTokenSource cancelTokenSource;
    private ProgressHandler progress;
    private bool FirstInitData;
    private bool doSelection;
    private bool saveAllDataToDevice;
    private bool isDataSaved = false;
    private bool CancelButtonPressed;
    internal StartupLib.GmmCorporateControl gmmCorporateControl1;
    internal DataGrid DataGridParameterView;
    internal ContextMenu contextM1;
    internal MenuItem miReadValue;
    internal MenuItem miWriteValue;
    internal MenuItem miNone1;
    internal MenuItem miInt32;
    internal MenuItem miUInt32;
    internal MenuItem miInt16;
    internal MenuItem miUInt16;
    internal MenuItem miSByte;
    internal MenuItem miByte;
    internal MenuItem miBool;
    internal MenuItem miFloat;
    internal MenuItem miDouble;
    internal MenuItem miNone2;
    internal MenuItem miUpdatePTF;
    internal Button ButtonCancel;
    internal TextBox TextBoxSearchParameter;
    internal Button ButtonSearchForParameter;
    internal ProgressBar ProgressBar1;
    internal Button ButtonPrint;
    internal Button ButtonOK;
    internal Label labelParameterInfoAvail;
    internal Grid Walter;
    internal Canvas Sign1;
    internal Button ButtonLoadPrintFile;
    private bool _contentLoaded;

    public DeviceMemory saveDeviceMemory { get; set; }

    private ObservableCollection<myDataRow> localDataSource { get; set; }

    private FirmwareParameterManager FWParamMgr { get; set; }

    private Assembly HandlerAssembly { get; set; }

    private bool InitData { get; set; }

    private bool IsParameterFileAvail { get; set; }

    private bool IsParameterFileDeveloper { get; set; }

    private bool IsDeviceConnected { get; set; }

    private bool bWriteToDevice { get; set; }

    private myDataRow actualMDR { get; set; }

    public ParameterWindow(
      DeviceMemory deviceMemory,
      BaseMemoryAccess memoryAccess,
      Assembly handlerAssembly,
      bool isDeviceConnected = true,
      bool writeToDevice = true)
    {
      this.actualDeviceMemory = deviceMemory;
      this.saveDeviceMemory = new DeviceMemory(deviceMemory);
      this.memoryAccess = memoryAccess;
      this.localDataSource = new ObservableCollection<myDataRow>();
      this.localDataSourceBACKUP = new List<myDataRow>();
      this.localDataSourceSELECT = new List<myDataRow>();
      this.cancelTokenSource = new CancellationTokenSource();
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.FirmwareParameter = new SortedList<string, Parameter32bit>();
      this.FirmwareParameter = this.actualDeviceMemory.MapDef.GetAllParametersList();
      this.doSelection = false;
      this.saveAllDataToDevice = false;
      this.IsParameterFileAvail = false;
      this.HandlerAssembly = handlerAssembly;
      this.IsDeviceConnected = isDeviceConnected;
      this.bWriteToDevice = writeToDevice;
      this.InitData = true;
      this.FirstInitData = true;
      this.InitializeComponent();
      this.LoadFirmwareParameterInformation();
    }

    private void OnProgress(ProgressArg obj)
    {
      if (!this.CheckAccess())
        this.Dispatcher.Invoke((Action) (() => this.OnProgress(obj)));
      else
        this.ProgressBar1.Value = obj.ProgressPercentage;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      this.DataGridParameterView.DataContext = (object) this.localDataSource;
      this.DataGridParameterView.ItemsSource = (IEnumerable) this.localDataSource;
    }

    public void LoadFirmwareParameterInformation()
    {
      if (this.HandlerAssembly == (Assembly) null)
        throw new Exception("Assembly Name not plausible, check DeviceMemory call.");
      if (this.FWParamMgr == null)
        this.FWParamMgr = new FirmwareParameterManager(this.HandlerAssembly);
      this.FWParamMgr.ParameterInfos = FirmwareParameterManager.LoadParameterInfos();
    }

    private void DataGrid_Loaded(object sender, RoutedEventArgs e)
    {
      if (sender == null)
        this.InitData = true;
      this.localDataSource.Clear();
      DataGrid gridParameterView = this.DataGridParameterView;
      if (this.FWParamMgr != null && this.FWParamMgr.ParameterInfos != null)
      {
        this.IsParameterFileAvail = true;
        this.labelParameterInfoAvail.Content = (object) ("Definition file location: " + FirmwareParameterManager.FileNameWithPath);
      }
      else
      {
        this.IsParameterFileAvail = false;
        this.labelParameterInfoAvail.Content = (object) "Info --- <Bold>ParameterTypeInfo file not avail.</Bold> --- All changes to TYPES will not be saved.";
      }
      this.IsParameterFileDeveloper = this.FWParamMgr != null && this.FWParamMgr.isDeveloperVersion;
      this.labelParameterInfoAvail.Visibility = Visibility.Visible;
      foreach (MenuItem menuItem in (IEnumerable) this.contextM1.Items)
      {
        if (menuItem.Name.Equals("miUpdatePTF"))
          menuItem.Visibility = this.IsParameterFileDeveloper ? Visibility.Visible : Visibility.Collapsed;
        if (menuItem.Name.Equals("miReadValue"))
          menuItem.IsEnabled = this.IsDeviceConnected;
        if (menuItem.Name.Equals("miWriteValue"))
          menuItem.IsEnabled = this.IsDeviceConnected;
      }
      foreach (Parameter32bit parameter32bit in (IEnumerable<Parameter32bit>) this.FirmwareParameter.Values)
      {
        Parameter32bit p32Bit = parameter32bit;
        this.progress.Reset(this.FirmwareParameter.Values.Count);
        int num1 = 0;
        bool flag = this.localDataSourceSELECT.Count<myDataRow>((Func<myDataRow, bool>) (x => x.Name.Contains(p32Bit.Name))) > 0;
        if (!this.doSelection || flag)
        {
          uint[] source = new uint[3]{ 1U, 2U, 4U };
          Type type = MapReader.ConvertToRealType(p32Bit.Typ);
          if (this.IsParameterFileAvail)
          {
            FirmwareParameterInfo firmwareParameterInfo = this.FWParamMgr.ParameterInfos.SingleOrDefault<FirmwareParameterInfo>((Func<FirmwareParameterInfo, bool>) (kvp => kvp.ParameterName == p32Bit.Name));
            if (firmwareParameterInfo != null && p32Bit.Typ != firmwareParameterInfo.ParameterType.ParameterTypeSaved)
            {
              Type realType = MapReader.ConvertToRealType(firmwareParameterInfo.ParameterType.ParameterTypeSaved);
              num1 = firmwareParameterInfo.ParameterType.ParameterTypeColPreset;
              if (realType != (Type) null)
              {
                Parameter32bit.SetType(realType, p32Bit);
                type = MapReader.ConvertToRealType(p32Bit.Typ);
              }
              else if (type != (Type) null)
                Parameter32bit.SetType(type, p32Bit);
            }
            else if (firmwareParameterInfo == null)
              this.GenerateParameterTypeInfo();
          }
          if (p32Bit.Typ.Contains("UNKNOWN") && ((IEnumerable<uint>) source).Contains<uint>(p32Bit.Size))
          {
            type = MapReader.getDefaultTypeForSize(p32Bit.Size);
            Parameter32bit.SetType(type, p32Bit);
          }
          uint sizeOfType = Parameter32bit.GetSizeOfType(type);
          object localValue = (object) null;
          string str1 = "";
          byte[] numArray = new byte[0];
          if (this.saveDeviceMemory.AreDataAvailable(p32Bit.Address, p32Bit.Size))
          {
            if (type == (Type) null || sizeOfType < p32Bit.Size)
            {
              localValue = (object) Parameter32bit.GetValueByteArray(p32Bit.Address, p32Bit.Size, this.saveDeviceMemory);
            }
            else
            {
              localValue = Parameter32bit.GetValue(type, p32Bit.Address, this.saveDeviceMemory);
              str1 = Parameter32bit.GetValue(type, p32Bit.Address, this.saveDeviceMemory, true).ToString();
              if (localValue != null)
                numArray = ParameterArrayAssistant.TypeToByteArray(type, localValue);
            }
          }
          object obj1 = (int) p32Bit.Size == (int) sizeOfType ? localValue : (object) "[...]";
          DeviceMemoryType deviceMemoryType = DeviceMemoryType.DataRAM;
          string message = string.Empty;
          DeviceMemoryStorage memoryTypeForData = this.saveDeviceMemory.GetDeviceMemoryTypeForData(p32Bit.Address, p32Bit.Size, out message);
          deviceMemoryType = memoryTypeForData == null ? DeviceMemoryType.NotAvail : memoryTypeForData.MemoryType;
          string str2 = deviceMemoryType.Equals((object) DeviceMemoryType.NotAvail) || deviceMemoryType.Equals((object) DeviceMemoryType.Unknown) ? "" : deviceMemoryType.ToString();
          string section1 = p32Bit.Section.ToUpper().Contains("UNKNOWN") ? "" : p32Bit.Section;
          string name = p32Bit.Name;
          string section2 = section1;
          string memoryArea = str2;
          string typ = type == (Type) null ? "" : this.OriginTypeName(type);
          uint num2 = p32Bit.Size;
          string bytes = "0x" + num2.ToString("x4");
          num2 = p32Bit.Address;
          string address = "0x" + num2.ToString("x8");
          string valuehex = str1;
          object obj2 = obj1;
          Type valTyp = type;
          myDataRow myDataRow = new myDataRow(true, this, name, section2, memoryArea, typ, bytes, address, valuehex, obj2, valTyp)
          {
            OrigValue = localValue
          };
          myDataRow.IsMemoryAvail = myDataRow.IsMemoryAvail = string.IsNullOrEmpty(message);
          if (this.InitData)
          {
            myDataRow.IsChanged = false;
            int num3 = myDataRow.ValueHEX == null ? 0 : (!string.IsNullOrEmpty(myDataRow.ValueHEX) ? 1 : 0);
            myDataRow.IsInit = num3 != 0;
          }
          myDataRow.IsReading = false;
          myDataRow.savedColumns = num1;
          this.localDataSource.Add(myDataRow);
          if (this.FirstInitData)
            this.localDataSourceBACKUP.Add(myDataRow);
          this.progress.Report();
        }
      }
      try
      {
        gridParameterView.Items.Refresh();
      }
      catch (Exception ex)
      {
      }
      this.InitData = false;
      this.FirstInitData = false;
    }

    private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
    }

    private void DataGridParameterView_PreparingCellForEdit(
      object sender,
      DataGridPreparingCellForEditEventArgs e)
    {
      TextBox editingElement = e.EditingElement as TextBox;
      myDataRow myDataRow = e.Row.Item as myDataRow;
      if (editingElement.Text.Contains("[...]"))
      {
        MessageBoxResult messageBoxResult = MessageBoxResult.Yes;
        if (myDataRow.OrigValue == null)
          messageBoxResult = MessageBox.Show("No initial data avail for this parameter, please read data from device first.\nIf you continue without reading data you could damage your device.\nDo you want to continue? ", "WARNING - DAMAGE !!!", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No);
        if (messageBoxResult == MessageBoxResult.Yes)
          this.DataGridParameterView_MouseDoubleClick(this.DataGridParameterView);
        ((DataGrid) sender).CancelEdit();
      }
      else
      {
        if (myDataRow.OrigValue != null)
          return;
        int num = (int) MessageBox.Show("No data for this parameter read from device, please read first.\nRight-Mouse click for context menu.");
        ((DataGrid) sender).CancelEdit();
      }
    }

    private void DataGridParameterView_MouseDoubleClick(DataGrid grid)
    {
      if (grid.CurrentItem == null)
        return;
      grid.Items.IndexOf(grid.CurrentItem);
      grid.CurrentCell.Column.Header.ToString();
      myDataRow mdr = grid.CurrentItem as myDataRow;
      try
      {
        if (mdr.Value == null || !mdr.Value.ToString().Contains("..."))
          return;
        Type valType = mdr.ValType;
        ParameterArrayAssistant parameterArrayAssistant = new ParameterArrayAssistant(mdr);
        parameterArrayAssistant.ShowDialog();
        if (mdr.IsChanged && !parameterArrayAssistant.isCanceled)
        {
          uint address = uint.Parse(mdr.Address.Substring(2), NumberStyles.HexNumber);
          if (mdr.OrigValue != null)
            Parameter32bit.SetValue<byte[]>((byte[]) mdr.OrigValue, address, this.saveDeviceMemory);
          mdr.ValType = valType;
          string hexString = ZR_ClassLibrary.Util.ByteArrayToHexString((byte[]) mdr.OrigValue);
          mdr.ValueHEX = hexString;
        }
        if (this.IsParameterFileAvail)
        {
          this.FWParamMgr.ParameterInfos.Single<FirmwareParameterInfo>((Func<FirmwareParameterInfo, bool>) (x => x.ParameterName == mdr.Name)).ParameterType.ParameterTypeColPreset = mdr.savedColumns;
          FirmwareParameterManager.SaveParameterInfos(this.FWParamMgr.ParameterInfos);
        }
      }
      catch (Exception ex)
      {
        throw new Exception("ERROR: \n\n" + ex.Message);
      }
      finally
      {
      }
    }

    private async void DataGridReadValue_MouseClick(object sender, RoutedEventArgs e)
    {
      this.progress.Report("reading...");
      DataGrid grid = this.DataGridParameterView;
      try
      {
        if (grid == null || grid.SelectedItems == null || grid.SelectedItems.Count <= 0)
        {
          grid = (DataGrid) null;
        }
        else
        {
          IEnumerable<myDataRow> items = grid.SelectedItems.OfType<myDataRow>();
          this.progress.Reset(items.Count<myDataRow>());
          foreach (myDataRow mdr in items)
          {
            uint address = uint.Parse(mdr.Address.Substring(2), NumberStyles.HexNumber);
            uint bytes = uint.Parse(mdr.Bytes.Substring(2), NumberStyles.HexNumber);
            this.saveDeviceMemory.GarantMemoryAvailable(new AddressRange(address, bytes));
            await this.memoryAccess.ReadMemoryAsync(new AddressRange(address, bytes), this.saveDeviceMemory, this.progress, this.cancelTokenSource.Token);
            Type myType = MapReader.ConvertToRealType(mdr.Typ);
            uint TypeSize = Parameter32bit.GetSizeOfType(mdr.ValType);
            if ((int) bytes == (int) TypeSize)
            {
              mdr.IsInit = true;
              mdr.IsReading = true;
              object oldOrigValue = mdr.OrigValue;
              object result = Parameter32bit.GetValue(myType, address, this.saveDeviceMemory);
              mdr.OrigValue = result;
              mdr.Value = myType != (Type) null ? result : (object) "[...]";
              mdr.ValueHEX = this.getHexStringForObject(result.ToString(), myType);
              if (!mdr.IsMemoryAvail && !string.IsNullOrEmpty(mdr.MemoryArea))
              {
                mdr.IsMemoryAvail = true;
                mdr.MemoryArea = mdr.MemoryArea.Substring(0, mdr.MemoryArea.Contains("-") ? mdr.MemoryArea.IndexOf('-') : mdr.MemoryArea.Length).Trim();
              }
              mdr.IsChanged = false;
              mdr.IsReading = false;
              oldOrigValue = (object) null;
              result = (object) null;
            }
            else
            {
              mdr.IsInit = true;
              mdr.IsReading = true;
              string message = string.Empty;
              byte[] result = Parameter32bit.GetValueByteArray(address, bytes, this.saveDeviceMemory);
              this.saveDeviceMemory.GetDeviceMemoryTypeForData(address, bytes, out message);
              mdr.MemoryArea = mdr.MemoryArea.Substring(0, mdr.MemoryArea.Contains("-") ? mdr.MemoryArea.IndexOf('-') : mdr.MemoryArea.Length).Trim();
              mdr.IsMemoryAvail = true;
              mdr.OrigValue = (object) result;
              mdr.IsValueForced = true;
              mdr.Value = (object) "[...]";
              mdr.IsValueForced = false;
              string hexString = ZR_ClassLibrary.Util.ByteArrayToHexString(result);
              mdr.ValueHEX = hexString;
              mdr.IsChanged = false;
              mdr.IsReading = false;
              message = (string) null;
              result = (byte[]) null;
              hexString = (string) null;
            }
            this.progress.Report("Read 0x" + mdr.Address + " " + mdr.Bytes + " byte(s)");
            myType = (Type) null;
          }
          items = (IEnumerable<myDataRow>) null;
          grid = (DataGrid) null;
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error occurred:\n\nDataGridReadValueFromDevice Error...");
        grid = (DataGrid) null;
      }
    }

    private async void DataGridWriteValue_MouseClick(object sender, RoutedEventArgs e)
    {
      DataGrid grid;
      if (!this.bWriteToDevice)
      {
        int num = (int) MessageBox.Show("Write to device is disabled!");
        grid = (DataGrid) null;
      }
      else
      {
        grid = this.DataGridParameterView;
        try
        {
          if (grid == null || grid.SelectedItems == null || grid.SelectedItems.Count <= 0)
          {
            grid = (DataGrid) null;
          }
          else
          {
            IEnumerable<myDataRow> items = grid.SelectedItems.OfType<myDataRow>();
            this.progress.Reset(items.Count<myDataRow>());
            foreach (myDataRow mdr in grid.SelectedItems.OfType<myDataRow>())
            {
              this.progress.Report("Write 0x" + mdr.Address + " " + mdr.Bytes + " byte(s)");
              uint address = uint.Parse(mdr.Address.Substring(2), NumberStyles.HexNumber);
              uint size = uint.Parse(mdr.Bytes.Substring(2), NumberStyles.HexNumber);
              this.saveDeviceMemory.GarantMemoryAvailable(new AddressRange(address, size));
              await this.memoryAccess.WriteMemoryAsync(new AddressRange(address, size), this.saveDeviceMemory, this.progress, this.cancelTokenSource.Token);
              mdr.IsInit = true;
              mdr.IsChanged = false;
            }
            items = (IEnumerable<myDataRow>) null;
            grid = (DataGrid) null;
          }
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("Error occurred:\n\n" + ex.Message, "WriteValueToDeviceError ...");
          grid = (DataGrid) null;
        }
      }
    }

    private void WriteDataToMemoryOnly(DeviceMemory memory = null)
    {
      try
      {
        if (memory == null)
          memory = this.saveDeviceMemory;
        this.progress.Reset(this.DataGridParameterView.Items.Count);
        foreach (myDataRow myDataRow in this.DataGridParameterView.Items.OfType<myDataRow>())
        {
          if (myDataRow.IsChanged || myDataRow.IsSavedTemp || this.saveAllDataToDevice)
          {
            uint num = uint.Parse(myDataRow.Address.Substring(2), NumberStyles.HexNumber);
            uint length = uint.Parse(myDataRow.Bytes.Substring(2), NumberStyles.HexNumber);
            byte[] numArray = new byte[(int) length];
            byte[] data = !(myDataRow.ValType == (Type) null) && !(myDataRow.ValType == typeof (byte[])) && (!(myDataRow.ValType != typeof (double)) || length <= 4U) ? ParameterArrayAssistant.TypeToByteArray(myDataRow.ValType, myDataRow.OrigValue) : (byte[]) myDataRow.OrigValue;
            AddressRange setRange = new AddressRange(num, (uint) data.Length);
            memory.GarantMemoryAvailable(setRange);
            memory.SetData(num, data);
            this.isDataSaved = true;
            myDataRow.IsSavedTemp = true;
            if (myDataRow.IsChanged)
              myDataRow.IsChanged = false;
          }
          this.ProgressBar1.UpdateLayout();
        }
      }
      catch (Exception ex)
      {
        throw new Exception("An error occurred while writing data to memory.\nMessage:" + ex.Message);
      }
    }

    private void ButtonCancel_Click(object sender, RoutedEventArgs e)
    {
      this.CancelButtonPressed = true;
      this.Close();
    }

    private void ButtonOK_Click(object sender, RoutedEventArgs e)
    {
      this.CancelButtonPressed = false;
      this.WriteDataToMemoryOnly(this.actualDeviceMemory);
      this.Close();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      foreach (myDataRow myDataRow in this.DataGridParameterView.Items.OfType<myDataRow>())
      {
        if (myDataRow.IsChanged)
        {
          if (MessageBox.Show("There are changes in the Parameters.\nDo you want to discard the changes ? ", "Discard changes ?", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.No)
          {
            e.Cancel = true;
            break;
          }
          break;
        }
      }
      if (!this.CancelButtonPressed && !e.Cancel && this.isDataSaved)
        this.WriteDataToMemoryOnly(this.actualDeviceMemory);
      this.CancelButtonPressed = false;
      base.OnClosing(e);
    }

    private void ButtonSearchForParameter_Click(object sender, RoutedEventArgs e)
    {
      string lower = this.TextBoxSearchParameter.Text.ToLower();
      this.localDataSourceSELECT.Clear();
      if (!string.IsNullOrEmpty(lower))
      {
        this.doSelection = true;
        foreach (myDataRow myDataRow in this.localDataSourceBACKUP)
        {
          if (((((((((myDataRow.Name.ToLower().Contains(lower) ? 1 : 0) | (myDataRow.Value != null ? (myDataRow.Value.ToString().ToLower().Contains(lower) ? 1 : 0) : 0)) != 0 ? 1 : 0) | (myDataRow.ValueHEX != null ? (myDataRow.ValueHEX.ToLower().Contains(lower) ? 1 : 0) : 0)) != 0 | myDataRow.Typ.ToLower().Contains(lower) ? 1 : 0) | (myDataRow.Section != null ? (myDataRow.Section.ToLower().Contains(lower) ? 1 : 0) : 0)) != 0 | myDataRow.Address.ToLower().Contains(lower) | myDataRow.Bytes.ToLower().Contains(lower) ? 1 : 0) | (myDataRow.MemoryArea != null ? (myDataRow.MemoryArea.ToLower().Contains(lower) ? 1 : 0) : 0)) != 0)
            this.localDataSourceSELECT.Add(myDataRow);
        }
      }
      else
        this.doSelection = false;
      this.DataGrid_Loaded((object) null, (RoutedEventArgs) null);
    }

    private void DataGridSetNewType(Type myType)
    {
      try
      {
        if (this.IsParameterFileDeveloper && !this.IsParameterFileAvail)
          this.GenerateParameterTypeInfo();
        bool flag = false;
        DataGrid gridParameterView = this.DataGridParameterView;
        if (gridParameterView == null || gridParameterView.SelectedItems == null || gridParameterView.SelectedItems.Count <= 0)
          return;
        gridParameterView.SelectedItems.OfType<myDataRow>();
        foreach (myDataRow myDataRow in gridParameterView.SelectedItems.OfType<myDataRow>())
        {
          myDataRow mdr = myDataRow;
          uint address = uint.Parse(mdr.Address.Substring(2), NumberStyles.HexNumber);
          uint byteSize = uint.Parse(mdr.Bytes.Substring(2), NumberStyles.HexNumber);
          uint sizeOfType = Parameter32bit.GetSizeOfType(myType);
          if ((int) byteSize == (int) sizeOfType)
          {
            mdr.ValType = myType;
            mdr.Typ = this.OriginTypeName(myType);
            object origValue = mdr.OrigValue;
            if (mdr.OrigValue != null)
            {
              mdr.IsInit = true;
              mdr.IsReading = true;
              object obj = Parameter32bit.GetValue(myType, address, this.saveDeviceMemory);
              mdr.OrigValue = obj;
              mdr.Value = myType != (Type) null ? obj : (object) "[...]";
              mdr.ValueHEX = this.getHexStringForObject(obj.ToString(), myType);
              if (!mdr.IsMemoryAvail && !string.IsNullOrEmpty(mdr.MemoryArea))
              {
                mdr.IsMemoryAvail = true;
                mdr.MemoryArea = mdr.MemoryArea.Substring(0, mdr.MemoryArea.Contains("-") ? mdr.MemoryArea.IndexOf('-') : mdr.MemoryArea.Length).Trim();
              }
            }
            else if (!string.IsNullOrEmpty(mdr.Value.ToString()) && mdr.Value.ToString().Contains("[...]"))
            {
              mdr.IsValueForced = true;
              mdr.Value = (object) null;
              mdr.IsValueForced = false;
            }
            mdr.IsChanged = false;
            mdr.IsReading = false;
          }
          else if (byteSize > sizeOfType)
          {
            mdr.ValType = myType;
            mdr.Typ = this.OriginTypeName(myType);
            if (mdr.OrigValue != null)
            {
              mdr.IsInit = true;
              mdr.IsReading = true;
              string message = string.Empty;
              byte[] valueByteArray = Parameter32bit.GetValueByteArray(address, byteSize, this.saveDeviceMemory);
              this.saveDeviceMemory.GetDeviceMemoryTypeForData(address, byteSize, out message);
              mdr.MemoryArea = mdr.MemoryArea.Substring(0, mdr.MemoryArea.Contains("-") ? mdr.MemoryArea.IndexOf('-') : mdr.MemoryArea.Length).Trim();
              mdr.IsMemoryAvail = !string.IsNullOrEmpty(message) || !message.Contains("not avail");
              mdr.ResetOldValue();
              mdr.OrigValue = (object) valueByteArray;
              string hexString = ZR_ClassLibrary.Util.ByteArrayToHexString(valueByteArray);
              mdr.ValueHEX = hexString;
            }
            mdr.IsValueForced = true;
            mdr.Value = (object) "[...]";
            mdr.IsValueForced = false;
            mdr.IsChanged = false;
            mdr.IsReading = false;
          }
          else if (byteSize < sizeOfType)
            return;
          if (this.IsParameterFileAvail)
          {
            FirmwareParameterInfo firmwareParameterInfo = this.FWParamMgr.ParameterInfos.Single<FirmwareParameterInfo>((Func<FirmwareParameterInfo, bool>) (x => x.ParameterName == mdr.Name));
            firmwareParameterInfo.ParameterType.ParameterTypeSaved = myType.Name;
            firmwareParameterInfo.ParameterType.ParameterTypeColPreset = mdr.savedColumns;
            flag = true;
            mdr.IsTypeChanged = true;
          }
        }
        if (flag && this.IsParameterFileAvail)
          FirmwareParameterManager.SaveParameterInfos(this.FWParamMgr.ParameterInfos);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("ERROR:\n" + ex.Message);
      }
    }

    private void GenerateParameterTypeInfo()
    {
      if (this.FWParamMgr == null)
        return;
      FirmwareParameterManager fwParamMgr = this.FWParamMgr;
      FirmwareParameterManager.GenerateParameterInfo(this.actualDeviceMemory.MapDef, ref fwParamMgr);
      FirmwareParameterManager.SaveParameterInfos(this.FWParamMgr.ParameterInfos);
    }

    private string getValueStringFromHex(Type myType, string value)
    {
      string empty = string.Empty;
      if (myType == typeof (double) || myType == typeof (float))
        return double.Parse(value, NumberStyles.HexNumber).ToString();
      if (myType == typeof (byte))
        return byte.Parse(value, NumberStyles.HexNumber).ToString();
      if (myType == typeof (sbyte))
        return sbyte.Parse(value, NumberStyles.HexNumber).ToString();
      if (myType == typeof (short))
        return short.Parse(value, NumberStyles.HexNumber).ToString();
      if (myType == typeof (ushort))
        return ushort.Parse(value, NumberStyles.HexNumber).ToString();
      if (myType == typeof (int))
        return int.Parse(value, NumberStyles.HexNumber).ToString();
      if (myType == typeof (uint))
        return uint.Parse(value, NumberStyles.HexNumber).ToString();
      return myType == typeof (Decimal) ? Decimal.Parse(value, NumberStyles.HexNumber).ToString() : empty;
    }

    private void DataGridSetType_MouseClick(object sender, RoutedEventArgs e)
    {
      MenuItem menuItem = (MenuItem) sender;
      if (menuItem.Header.Equals((object) "uint"))
        this.DataGridSetNewType(typeof (uint));
      else if (menuItem.Header.Equals((object) "ushort"))
        this.DataGridSetNewType(typeof (ushort));
      else if (menuItem.Header.Equals((object) "double"))
        this.DataGridSetNewType(typeof (double));
      else if (menuItem.Header.Equals((object) "float"))
        this.DataGridSetNewType(typeof (float));
      else if (menuItem.Header.Equals((object) "byte"))
        this.DataGridSetNewType(typeof (byte));
      else if (menuItem.Header.Equals((object) "sbyte"))
        this.DataGridSetNewType(typeof (sbyte));
      else if (menuItem.Header.Equals((object) "int"))
        this.DataGridSetNewType(typeof (int));
      else if (menuItem.Header.Equals((object) "short"))
        this.DataGridSetNewType(typeof (short));
      else if (menuItem.Header.Equals((object) "Bool"))
        this.DataGridSetNewType(typeof (bool));
      else if (menuItem.Header.Equals((object) "byte[]"))
      {
        this.DataGridSetNewType(typeof (byte[]));
      }
      else
      {
        if (!menuItem.Header.Equals((object) "-none-"))
          return;
        this.DataGridSetNewType((Type) null);
      }
    }

    private string OriginTypeName(Type myType)
    {
      if (myType == typeof (uint))
        return "uint";
      if (myType == typeof (ushort))
        return "ushort";
      if (myType == typeof (int))
        return "int";
      if (myType == typeof (short))
        return "short";
      if (myType == typeof (sbyte))
        return "sbyte";
      if (myType == typeof (byte))
        return "byte";
      if (myType == typeof (float))
        return "float";
      if (myType == typeof (double))
        return "double";
      if (myType == typeof (byte[]))
        return "byte[]";
      if (myType == typeof (sbyte[]))
        return "sbyte[]";
      return myType == typeof (bool) ? "bool" : string.Empty;
    }

    private void DataGridUpdatePTF_MouseClick(object sender, RoutedEventArgs e)
    {
      this.GenerateParameterTypeInfo();
    }

    private string getHexStringForObject(string value, Type myType)
    {
      if (myType == typeof (byte))
        return byte.Parse(value).ToString("X2");
      if (myType == typeof (sbyte))
        return sbyte.Parse(value).ToString("X2");
      if (myType == typeof (short))
        return short.Parse(value).ToString("X4");
      if (myType == typeof (ushort))
        return ushort.Parse(value).ToString("X4");
      if (myType == typeof (int))
        return int.Parse(value).ToString("X8");
      if (myType == typeof (uint))
        return uint.Parse(value).ToString("X8");
      if (myType == typeof (float))
        return ParameterArrayAssistant.float2HEXString(float.Parse(value));
      if (myType == typeof (double))
        return ParameterArrayAssistant.double2HEXString(double.Parse(value));
      return myType == typeof (Decimal) ? Decimal.Parse(value).ToString("x8") : string.Empty;
    }

    private void ButtonPrint_Click(object sender, RoutedEventArgs e)
    {
      if (this.actualDeviceMemory == null)
        return;
      string title = new FirmwareVersion(this.actualDeviceMemory.FirmwareVersion).ToString() + " (" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ")";
      string parameterInfo = this.actualDeviceMemory.GetParameterInfo();
      if (string.IsNullOrEmpty(parameterInfo))
        return;
      NotepadHelper.ShowMessage(parameterInfo, title);
    }

    private void ButtonLoadPrintFile_Click(object sender, RoutedEventArgs e)
    {
      if (this.actualDeviceMemory == null)
        return;
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.DefaultExt = ".txt";
      openFileDialog.Filter = "Backup file (*.txt)|*.txt";
      bool? nullable = openFileDialog.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      try
      {
        string[] strArray = File.ReadAllText(openFileDialog.FileName).Split(new string[1]
        {
          Environment.NewLine
        }, StringSplitOptions.None);
        for (int index = 4; index < strArray.Length; ++index)
        {
          List<string> list = ((IEnumerable<string>) Regex.Split(strArray[index], "\\s+")).Where<string>((Func<string, bool>) (s => s != string.Empty)).Select<string, string>((Func<string, string>) (p => p.Trim())).ToList<string>();
          if (list.Count<string>() > 4)
            this.actualDeviceMemory.SetData(Convert.ToUInt32(list[0], 16), ZR_ClassLibrary.Util.HexStringToByteArray(list[4]));
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((Window) this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/parameterwindow.xaml", UriKind.Relative));
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
          this.gmmCorporateControl1 = (StartupLib.GmmCorporateControl) target;
          break;
        case 3:
          this.DataGridParameterView = (DataGrid) target;
          this.DataGridParameterView.Loaded += new RoutedEventHandler(this.DataGrid_Loaded);
          this.DataGridParameterView.PreparingCellForEdit += new EventHandler<DataGridPreparingCellForEditEventArgs>(this.DataGridParameterView_PreparingCellForEdit);
          this.DataGridParameterView.SelectionChanged += new SelectionChangedEventHandler(this.DataGrid_SelectionChanged);
          break;
        case 4:
          this.contextM1 = (ContextMenu) target;
          break;
        case 5:
          this.miReadValue = (MenuItem) target;
          this.miReadValue.Click += new RoutedEventHandler(this.DataGridReadValue_MouseClick);
          break;
        case 6:
          this.miWriteValue = (MenuItem) target;
          this.miWriteValue.Click += new RoutedEventHandler(this.DataGridWriteValue_MouseClick);
          break;
        case 7:
          this.miNone1 = (MenuItem) target;
          break;
        case 8:
          this.miInt32 = (MenuItem) target;
          this.miInt32.Click += new RoutedEventHandler(this.DataGridSetType_MouseClick);
          break;
        case 9:
          this.miUInt32 = (MenuItem) target;
          this.miUInt32.Click += new RoutedEventHandler(this.DataGridSetType_MouseClick);
          break;
        case 10:
          this.miInt16 = (MenuItem) target;
          this.miInt16.Click += new RoutedEventHandler(this.DataGridSetType_MouseClick);
          break;
        case 11:
          this.miUInt16 = (MenuItem) target;
          this.miUInt16.Click += new RoutedEventHandler(this.DataGridSetType_MouseClick);
          break;
        case 12:
          this.miSByte = (MenuItem) target;
          this.miSByte.Click += new RoutedEventHandler(this.DataGridSetType_MouseClick);
          break;
        case 13:
          this.miByte = (MenuItem) target;
          this.miByte.Click += new RoutedEventHandler(this.DataGridSetType_MouseClick);
          break;
        case 14:
          this.miBool = (MenuItem) target;
          this.miBool.Click += new RoutedEventHandler(this.DataGridSetType_MouseClick);
          break;
        case 15:
          this.miFloat = (MenuItem) target;
          this.miFloat.Click += new RoutedEventHandler(this.DataGridSetType_MouseClick);
          break;
        case 16:
          this.miDouble = (MenuItem) target;
          this.miDouble.Click += new RoutedEventHandler(this.DataGridSetType_MouseClick);
          break;
        case 17:
          this.miNone2 = (MenuItem) target;
          break;
        case 18:
          this.miUpdatePTF = (MenuItem) target;
          this.miUpdatePTF.Click += new RoutedEventHandler(this.DataGridUpdatePTF_MouseClick);
          break;
        case 19:
          this.ButtonCancel = (Button) target;
          this.ButtonCancel.Click += new RoutedEventHandler(this.ButtonCancel_Click);
          break;
        case 20:
          this.TextBoxSearchParameter = (TextBox) target;
          break;
        case 21:
          this.ButtonSearchForParameter = (Button) target;
          this.ButtonSearchForParameter.Click += new RoutedEventHandler(this.ButtonSearchForParameter_Click);
          break;
        case 22:
          this.ProgressBar1 = (ProgressBar) target;
          break;
        case 23:
          this.ButtonPrint = (Button) target;
          this.ButtonPrint.Click += new RoutedEventHandler(this.ButtonPrint_Click);
          break;
        case 24:
          this.ButtonOK = (Button) target;
          this.ButtonOK.Click += new RoutedEventHandler(this.ButtonOK_Click);
          break;
        case 25:
          this.labelParameterInfoAvail = (Label) target;
          break;
        case 26:
          this.Walter = (Grid) target;
          break;
        case 27:
          this.Sign1 = (Canvas) target;
          break;
        case 28:
          this.ButtonLoadPrintFile = (Button) target;
          this.ButtonLoadPrintFile.Click += new RoutedEventHandler(this.ButtonLoadPrintFile_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}

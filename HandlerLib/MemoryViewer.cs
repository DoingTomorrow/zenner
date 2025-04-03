// Decompiled with JetBrains decompiler
// Type: HandlerLib.MemoryViewer
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using Microsoft.Win32;
using StartupLib;
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
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public class MemoryViewer : Window, IComponentConnector
  {
    private DeviceMemory localDeviceMemory;
    private DeviceMemory localBackUpDeviceMemory;
    private BaseMemoryAccess memoryAccess;
    private CancellationTokenSource cancelTokenSource;
    private ProgressHandler progress;
    private ObservableCollection<memoryRow> localItems;
    private SortedList<uint, memoryRow> localItems_ALL;
    private string[] ba_actual;
    private uint actualStartAddress;
    private uint actualBytesToRead;
    private bool isDirty;
    private uint maxBytesAtOnce = 1024;
    internal GmmCorporateControl gmmCorporateControl1;
    internal DataGrid DataGridMemoryArray;
    internal Grid DataGridSettings;
    internal ProgressBar ProgressBar1;
    internal Label LabelProgress1;
    internal TextBox TextBoxAddressDevice;
    internal TextBox TextBoxSizeDevice;
    internal Button ButtonReadDataFromDevice;
    internal Button ButtonWriteDataToDevice;
    internal Button ButtonCopyValues;
    internal Button ButtonLoadDeviceMemory;
    internal Button ButtonUpdateDataGrid;
    internal Button ButtonPrev;
    internal Button ButtonNext;
    internal ComboBox ComboBoxAddress;
    internal TextBox TextBoxAddressMemory;
    internal Button ButtonGotoAddress;
    internal Button ButtonCancel;
    internal Button ButtonSaveDeviceMemoryToFile;
    internal TextBox TextBoxStopwatch;
    internal CheckBox CheckBoxShowHex;
    internal TextBox TextBoxSizeMemory;
    private bool _contentLoaded;

    public MemoryViewer(DeviceMemory meterMemory, BaseMemoryAccess memoryAccess)
    {
      this.InitializeComponent();
      this.memoryAccess = memoryAccess;
      this.localDeviceMemory = meterMemory;
      this.localBackUpDeviceMemory = new DeviceMemory(meterMemory);
      this.localItems = new ObservableCollection<memoryRow>();
      this.localItems_ALL = new SortedList<uint, memoryRow>();
      this.cancelTokenSource = new CancellationTokenSource();
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.DataContext = (object) this;
      this.actualStartAddress = 2147483648U;
      this.actualBytesToRead = 256U;
      List<DeviceMemoryStorage> list = meterMemory.MemoryBlockList.Values.OrderByDescending<DeviceMemoryStorage, uint>((Func<DeviceMemoryStorage, uint>) (kp => kp.StartAddress)).ToList<DeviceMemoryStorage>();
      bool flag = true;
      foreach (DeviceMemoryStorage newItem in list)
      {
        if (flag)
        {
          this.actualStartAddress = newItem.StartAddress;
          flag = false;
        }
        this.ComboBoxAddress.Items.Add((object) newItem);
      }
      if (WindowsIdentity.GetCurrent().Name.Contains("StollMa"))
      {
        this.ButtonLoadDeviceMemory.Visibility = Visibility.Visible;
        this.ButtonSaveDeviceMemoryToFile.Visibility = Visibility.Visible;
        this.TextBoxStopwatch.Visibility = Visibility.Visible;
      }
      else
      {
        this.ButtonLoadDeviceMemory.Visibility = Visibility.Hidden;
        this.ButtonSaveDeviceMemoryToFile.Visibility = Visibility.Hidden;
        this.TextBoxStopwatch.Visibility = Visibility.Hidden;
      }
      this.CheckBoxShowHex.IsChecked = new bool?(true);
    }

    private void OnProgress(ProgressArg obj)
    {
      if (!this.CheckAccess())
      {
        this.Dispatcher.Invoke((Action) (() => this.OnProgress(obj)));
      }
      else
      {
        this.ProgressBar1.Value = obj.ProgressPercentage;
        this.LabelProgress1.Content = (object) obj.Message;
      }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e) => this.setTextToAddressFields();

    private void setTextToAddressFields()
    {
      this.TextBoxAddressMemory.Text = !this.TextBoxAddressMemory.Text.Contains("0x") ? this.actualStartAddress.ToString("D") : "0x" + this.actualStartAddress.ToString("x8");
      if (this.TextBoxSizeMemory.Text.Contains("0x"))
        this.TextBoxSizeMemory.Text = "0x" + this.actualBytesToRead.ToString("x4");
      else
        this.TextBoxSizeMemory.Text = this.actualBytesToRead.ToString("D");
    }

    private void setActualAddresses(int step = 0, bool isReadFromDevice = false)
    {
      uint result1 = 0;
      uint result2 = this.actualBytesToRead;
      if (this.TextBoxAddressMemory.Text.Length > 2 && this.TextBoxAddressMemory.Text.Substring(0, 2).Equals("0x"))
        uint.TryParse(this.TextBoxAddressMemory.Text.Substring(2), NumberStyles.HexNumber, (IFormatProvider) null, out result1);
      else
        uint.TryParse(this.TextBoxAddressMemory.Text, out result1);
      if (this.TextBoxSizeMemory.Text.Length > 2 && this.TextBoxSizeMemory.Text.Substring(0, 2).Equals("0x"))
        uint.TryParse(this.TextBoxSizeMemory.Text.Substring(2), NumberStyles.HexNumber, (IFormatProvider) null, out result2);
      else
        uint.TryParse(this.TextBoxSizeMemory.Text, out result2);
      if (isReadFromDevice)
      {
        if (this.TextBoxAddressDevice.Text.Length > 2 && this.TextBoxAddressDevice.Text.Substring(0, 2).Equals("0x"))
          uint.TryParse(this.TextBoxAddressDevice.Text.Substring(2), NumberStyles.HexNumber, (IFormatProvider) null, out result1);
        else
          uint.TryParse(this.TextBoxAddressDevice.Text, out result1);
        if (this.TextBoxSizeDevice.Text.Length > 2 && this.TextBoxSizeDevice.Text.Substring(0, 2).Equals("0x"))
          uint.TryParse(this.TextBoxSizeDevice.Text.Substring(2), NumberStyles.HexNumber, (IFormatProvider) null, out result2);
        else
          uint.TryParse(this.TextBoxSizeDevice.Text, out result2);
      }
      this.actualStartAddress = (uint) ((ulong) result1 + (ulong) step);
      this.actualBytesToRead = result2;
    }

    private void copyLocalDataToOLD(ObservableCollection<memoryRow> oldMemRows)
    {
      foreach (memoryRow oldMemRow in (Collection<memoryRow>) oldMemRows)
      {
        if (this.localItems_ALL.ContainsKey(oldMemRow.address))
          this.localItems_ALL[oldMemRow.address] = oldMemRow;
        else
          this.localItems_ALL.Add(oldMemRow.address, oldMemRow);
      }
    }

    private void updateDataGrid(bool isNewData = false)
    {
      this.DataGridMemoryArray.ItemsSource = (IEnumerable) null;
      this.copyLocalDataToOLD(this.localItems);
      this.localItems.Clear();
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      int newParts = (int) this.actualBytesToRead / 16 + 1;
      uint actualStartAddress = this.actualStartAddress;
      uint length = this.actualBytesToRead > this.maxBytesAtOnce ? this.maxBytesAtOnce : this.actualBytesToRead;
      int num = (int) Math.Ceiling((double) this.actualBytesToRead / (double) length);
      this.ba_actual = new string[(long) length * (long) num];
      string[] strArray1 = new string[(long) length * (long) num];
      for (int index1 = 0; index1 < num; ++index1)
      {
        try
        {
          Array.Copy((Array) this.localDeviceMemory.GetDataAsHexStrings((uint) ((ulong) actualStartAddress + (ulong) index1 * (ulong) length), length), 0L, (Array) this.ba_actual, (long) index1 * (long) length, (long) length);
        }
        catch (Exception ex)
        {
          string[] strArray2 = new string[(int) length];
          for (int index2 = 0; (long) index2 < (long) length; ++index2)
            strArray2[index2] = "??";
        }
      }
      this.progress.Reset(newParts);
      do
      {
        for (int index = 0; index < newParts; ++index)
        {
          string[] strArray3 = new string[16];
          string[] strArray4 = new string[16];
          Array.Copy((Array) this.ba_actual, index * 16, (Array) strArray3, 0, this.ba_actual.Length - index * 16 < 16 ? this.ba_actual.Length - index * 16 : 16);
          memoryRow memoryRow = (memoryRow) null;
          if (this.localItems_ALL.ContainsKey(actualStartAddress))
            memoryRow = this.localItems_ALL[actualStartAddress];
          if (memoryRow == null)
          {
            memoryRow = new memoryRow();
            memoryRow.address = actualStartAddress;
            bool? isChecked = this.CheckBoxShowHex.IsChecked;
            bool flag = true;
            memoryRow.Address = !(isChecked.GetValueOrDefault() == flag & isChecked.HasValue) ? actualStartAddress.ToString() : actualStartAddress.ToString("x8");
            memoryRow.overWriteValuesOnly = false;
            memoryRow.byte_0 = strArray3[0];
            memoryRow.byte_1 = strArray3[1];
            memoryRow.byte_2 = strArray3[2];
            memoryRow.byte_3 = strArray3[3];
            memoryRow.byte_4 = strArray3[4];
            memoryRow.byte_5 = strArray3[5];
            memoryRow.byte_6 = strArray3[6];
            memoryRow.byte_7 = strArray3[7];
            memoryRow.byte_8 = strArray3[8];
            memoryRow.byte_9 = strArray3[9];
            memoryRow.byte_10 = strArray3[10];
            memoryRow.byte_11 = strArray3[11];
            memoryRow.byte_12 = strArray3[12];
            memoryRow.byte_13 = strArray3[13];
            memoryRow.byte_14 = strArray3[14];
            memoryRow.byte_15 = strArray3[15];
            memoryRow.IsChanged = false;
            memoryRow.ClearDataChanged();
          }
          else
          {
            memoryRow.overWriteValuesOnly = true;
            memoryRow.byte_0 = strArray3[0];
            memoryRow.byte_1 = strArray3[1];
            memoryRow.byte_2 = strArray3[2];
            memoryRow.byte_3 = strArray3[3];
            memoryRow.byte_4 = strArray3[4];
            memoryRow.byte_5 = strArray3[5];
            memoryRow.byte_6 = strArray3[6];
            memoryRow.byte_7 = strArray3[7];
            memoryRow.byte_8 = strArray3[8];
            memoryRow.byte_9 = strArray3[9];
            memoryRow.byte_10 = strArray3[10];
            memoryRow.byte_11 = strArray3[11];
            memoryRow.byte_12 = strArray3[12];
            memoryRow.byte_13 = strArray3[13];
            memoryRow.byte_14 = strArray3[14];
            memoryRow.byte_15 = strArray3[15];
            memoryRow.overWriteValuesOnly = false;
            if (isNewData)
            {
              memoryRow.IsChanged = false;
              memoryRow.ClearDataChanged();
            }
          }
          memoryRow.row = (uint) index;
          memoryRow.data = this.getReadAbleDataString(strArray3);
          this.localItems.Add(memoryRow);
          this.progress.Report("reading ... " + memoryRow.Address);
          actualStartAddress += 16U;
        }
      }
      while (false);
      stopwatch.Stop();
      this.TextBoxStopwatch.Text = stopwatch.ElapsedMilliseconds.ToString() + " ms.";
      this.DataGridMemoryArray.ItemsSource = (IEnumerable) this.localItems;
      this.progress.Report(" reading done! ");
    }

    private string getReadAbleDataString(byte[] bascii)
    {
      string readAbleDataString = string.Empty;
      foreach (byte num in bascii)
        readAbleDataString = num < (byte) 32 || num == (byte) 85 ? readAbleDataString + "." : readAbleDataString + ((char) num).ToString();
      return readAbleDataString;
    }

    private string getReadAbleDataString(string[] asciiViews)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string asciiView in asciiViews)
      {
        int result;
        if (!int.TryParse(asciiView, NumberStyles.HexNumber, (IFormatProvider) null, out result))
          stringBuilder.Append('.');
        else if (char.IsLetterOrDigit((char) result))
          stringBuilder.Append((char) result);
        else
          stringBuilder.Append('.');
      }
      return stringBuilder.ToString();
    }

    private void ButtonReadDataFromDevice_Click(object sender, RoutedEventArgs e)
    {
      this.setActualAddresses(isReadFromDevice: true);
      this.readDataFromDevice();
    }

    private void ButtonWriteDataToDevice_Click(object sender, RoutedEventArgs e)
    {
      this.writeDataToDevice();
    }

    private void ButonUpdateGRID_Click(object sender, RoutedEventArgs e) => this.updateDataGrid();

    private void ButtonPrev_Memory_Click(object sender, RoutedEventArgs e)
    {
      this.setActualAddresses((int) this.actualBytesToRead * -1);
      this.setTextToAddressFields();
      this.updateDataGrid();
    }

    private void ButtonNext_Memory_Click(object sender, RoutedEventArgs e)
    {
      this.setActualAddresses((int) this.actualBytesToRead);
      this.setTextToAddressFields();
      this.updateDataGrid();
    }

    private void ButtonLoadDeviceMemory_Click(object sender, RoutedEventArgs e)
    {
      this.loadDeviceMemFromFile();
    }

    private void ButtonSaveDeviceMemoryToFile_Click(object sender, RoutedEventArgs e)
    {
      this.writeMemoryDataToFile();
    }

    private void writeMemoryDataToFile()
    {
      byte[] compressedData = this.localDeviceMemory.GetCompressedData();
      File.WriteAllBytes("U:\\Device_" + this.localDeviceMemory.FirmwareVersion.ToString() + ".device", compressedData);
    }

    private async void writeDataToDevice()
    {
      try
      {
        uint writeStartAddress = 0;
        uint writeEndAddress = 0;
        foreach (memoryRow mr in (IEnumerable<memoryRow>) this.localItems_ALL.Values)
        {
          for (int i = 0; i < mr.dataChanged.Length; ++i)
          {
            if (mr.dataChanged[i])
            {
              writeStartAddress = writeStartAddress == 0U ? (uint) ((ulong) mr.address + (ulong) i) : writeStartAddress;
              writeEndAddress = writeEndAddress < (uint) ((ulong) mr.address + (ulong) i) ? (uint) ((ulong) mr.address + (ulong) i) : writeEndAddress;
              if (writeStartAddress >= 0U && writeEndAddress > 0U)
              {
                AddressRange aRange = new AddressRange(writeStartAddress, writeEndAddress + 1U - writeStartAddress);
                this.localDeviceMemory.GarantMemoryAvailable(aRange);
                await this.memoryAccess.WriteMemoryAsync(aRange, this.localDeviceMemory, this.progress, this.cancelTokenSource.Token);
                writeStartAddress = 0U;
                writeEndAddress = 0U;
                mr.dataChanged[i] = false;
                aRange = (AddressRange) null;
              }
            }
          }
          mr.ClearDataChanged();
          mr.IsChanged = false;
        }
        this.isDirty = false;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error occoured: \n\n" + ex.Message);
      }
    }

    private async void readDataFromDevice()
    {
      this.progress.Reset();
      if (this.isDirty && MessageBoxResult.No == MessageBox.Show("There are changes,reading from device will maybe overwrite them.\nDo you want to continue without saving the changes?", "SAVE Changes?", MessageBoxButton.YesNo))
        return;
      try
      {
        AddressRange readRange = new AddressRange(this.actualStartAddress, this.actualBytesToRead);
        this.localDeviceMemory.GarantMemoryAvailable(readRange);
        await this.memoryAccess.ReadMemoryAsync(readRange, this.localDeviceMemory, this.progress, this.cancelTokenSource.Token);
        readRange = (AddressRange) null;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error occoured: \n\n" + ex.Message);
      }
      this.localBackUpDeviceMemory = (DeviceMemory) null;
      this.localBackUpDeviceMemory = new DeviceMemory(this.localDeviceMemory);
      this.updateDataGrid(true);
    }

    private void ButtonGoto_Memory_Address_Click(object sender, RoutedEventArgs e)
    {
      this.setActualAddresses();
      this.updateDataGrid();
    }

    private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
    {
      string text = (e.EditingElement as TextBox).Text;
      string str = e.Column.Header.ToString();
      memoryRow memoryRow = e.Row.Item as memoryRow;
      try
      {
        int int32 = Convert.ToInt32(str);
        string name = "byte_" + int32.ToString();
        PropertyInfo property = memoryRow.GetType().GetProperty(name);
        if ((PropertyInfo) null != property && property.CanWrite)
        {
          byte[] numArray = new byte[1];
          byte[] byteArray = ZR_ClassLibrary.Util.HexStringToByteArray(text);
          property.SetValue((object) memoryRow, (object) text, (object[]) null);
          uint num = memoryRow.address + (uint) int32;
          this.localDeviceMemory.GarantMemoryAvailable(new AddressRange(num, (uint) byteArray.Length));
          this.localDeviceMemory.SetData(num, byteArray);
          memoryRow.IsChanged = true;
          this.isDirty = true;
        }
        this.updateDataGrid();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("There is an error in byte arra.\n" + ex.Message);
      }
    }

    private void ButtonCancel_Click(object sender, RoutedEventArgs e)
    {
      if (this.isDirty)
      {
        if (MessageBox.Show("There are changes that are not written to a device.\nDo you really want to quit and discard the changes?", "Changes pending ...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) != MessageBoxResult.Yes)
          return;
        this.isDirty = false;
        this.Close();
      }
      else
        this.Close();
    }

    private void Window_Closed(object sender, CancelEventArgs e)
    {
      if (!this.isDirty)
        return;
      if (MessageBox.Show("There are changes that are not written to a device.\nDo you really want to quit and discard the changes?", "Changes pending ...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No) == MessageBoxResult.Yes)
      {
        this.isDirty = false;
        e.Cancel = false;
      }
      else
        e.Cancel = true;
    }

    private void TextBoxes_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Return)
        this.ButtonGoto_Memory_Address_Click((object) null, (RoutedEventArgs) null);
      int num;
      if (e.Key != Key.D0 && e.Key != Key.D1 && e.Key != Key.D2 && e.Key != Key.D3 && e.Key != Key.D4 && e.Key != Key.D5 && e.Key != Key.D6 && e.Key != Key.D7 && e.Key != Key.D8 && e.Key != Key.D9)
      {
        bool? isChecked = this.CheckBoxShowHex.IsChecked;
        bool flag = true;
        num = !(isChecked.GetValueOrDefault() == flag & isChecked.HasValue) ? 0 : (e.Key != Key.X || e.Key != Key.A || e.Key != Key.B || e.Key != Key.C || e.Key != Key.D || e.Key != Key.E ? 1 : (e.Key != Key.F ? 1 : 0));
      }
      else
        num = 0;
      if (num == 0)
        ;
    }

    private void TextBoxes_GotFocus(object sender, RoutedEventArgs e)
    {
      ((TextBoxBase) sender).SelectAll();
    }

    private void ButtonSetValuesFromMemory_Click(object sender, RoutedEventArgs e)
    {
      this.TextBoxAddressDevice.Text = this.TextBoxAddressMemory.Text;
      this.TextBoxSizeDevice.Text = this.TextBoxSizeMemory.Text;
    }

    private void TextBoxes_LostFocus(object sender, RoutedEventArgs e) => this.setActualAddresses();

    private void CheckBoxShowHex_Click(object sender, RoutedEventArgs e)
    {
      this.setTextToAddressFields();
    }

    private void DataGridMemoryArray_KeyUp(object sender, KeyEventArgs e)
    {
    }

    private void DataGridMemoryArray_KeyDown(object sender, KeyEventArgs e)
    {
      if (this.isAnAllowedKey(e.Key))
        return;
      e.Handled = true;
    }

    private bool isAnAllowedKey(Key pressedOne)
    {
      return new List<Key>()
      {
        Key.D0,
        Key.D1,
        Key.D2,
        Key.D3,
        Key.D4,
        Key.D5,
        Key.D6,
        Key.D7,
        Key.D8,
        Key.D9,
        Key.A,
        Key.B,
        Key.C,
        Key.D,
        Key.E,
        Key.F,
        Key.Tab,
        Key.Back,
        Key.Up,
        Key.Down,
        Key.Left,
        Key.Right,
        Key.NumPad0,
        Key.NumPad1,
        Key.NumPad2,
        Key.NumPad3,
        Key.NumPad4,
        Key.NumPad5,
        Key.NumPad6,
        Key.NumPad7,
        Key.NumPad8,
        Key.NumPad9
      }.Contains<Key>(pressedOne);
    }

    private void DataGridMemoryArray_CurrentCellChanged(object sender, EventArgs e)
    {
    }

    private void ComboBoxAddress_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      DeviceMemoryStorage selectedItem = (DeviceMemoryStorage) this.ComboBoxAddress.SelectedItem;
      this.TextBoxAddressMemory.Text = "0x" + selectedItem.StartAddress.ToString("X8");
      this.TextBoxSizeMemory.Text = "0x" + selectedItem.ByteSize.ToString("X4");
      this.setActualAddresses();
      this.updateDataGrid();
    }

    private void loadDeviceMemFromFile()
    {
      this.progress.Reset(6);
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.FileName = "";
      openFileDialog.DefaultExt = ".device";
      openFileDialog.Filter = "DeviceMeter image (.device)|*.device";
      bool? nullable1 = openFileDialog.ShowDialog();
      this.progress.Report();
      bool? nullable2 = nullable1;
      bool flag = true;
      if (nullable2.GetValueOrDefault() == flag & nullable2.HasValue)
      {
        this.progress.Report();
        string fileName = openFileDialog.FileName;
        this.progress.Report();
        byte[] compressedData = File.ReadAllBytes(fileName);
        this.progress.Report();
        this.localDeviceMemory = new DeviceMemory(compressedData);
        this.localBackUpDeviceMemory = new DeviceMemory(compressedData);
        this.progress.Report();
        this.updateDataGrid(true);
      }
      this.progress.Report();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/memoryviewer.xaml", UriKind.Relative));
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
          ((Window) target).Closing += new CancelEventHandler(this.Window_Closed);
          break;
        case 2:
          this.gmmCorporateControl1 = (GmmCorporateControl) target;
          break;
        case 3:
          this.DataGridMemoryArray = (DataGrid) target;
          this.DataGridMemoryArray.CellEditEnding += new EventHandler<DataGridCellEditEndingEventArgs>(this.DataGrid_CellEditEnding);
          this.DataGridMemoryArray.CurrentCellChanged += new EventHandler<EventArgs>(this.DataGridMemoryArray_CurrentCellChanged);
          break;
        case 4:
          this.DataGridSettings = (Grid) target;
          break;
        case 5:
          this.ProgressBar1 = (ProgressBar) target;
          break;
        case 6:
          this.LabelProgress1 = (Label) target;
          break;
        case 7:
          this.TextBoxAddressDevice = (TextBox) target;
          this.TextBoxAddressDevice.KeyDown += new KeyEventHandler(this.TextBoxes_KeyDown);
          this.TextBoxAddressDevice.LostFocus += new RoutedEventHandler(this.TextBoxes_LostFocus);
          this.TextBoxAddressDevice.GotFocus += new RoutedEventHandler(this.TextBoxes_GotFocus);
          break;
        case 8:
          this.TextBoxSizeDevice = (TextBox) target;
          this.TextBoxSizeDevice.KeyDown += new KeyEventHandler(this.TextBoxes_KeyDown);
          this.TextBoxSizeDevice.LostFocus += new RoutedEventHandler(this.TextBoxes_LostFocus);
          this.TextBoxSizeDevice.GotFocus += new RoutedEventHandler(this.TextBoxes_GotFocus);
          break;
        case 9:
          this.ButtonReadDataFromDevice = (Button) target;
          this.ButtonReadDataFromDevice.Click += new RoutedEventHandler(this.ButtonReadDataFromDevice_Click);
          break;
        case 10:
          this.ButtonWriteDataToDevice = (Button) target;
          this.ButtonWriteDataToDevice.Click += new RoutedEventHandler(this.ButtonWriteDataToDevice_Click);
          break;
        case 11:
          this.ButtonCopyValues = (Button) target;
          this.ButtonCopyValues.Click += new RoutedEventHandler(this.ButtonSetValuesFromMemory_Click);
          break;
        case 12:
          this.ButtonLoadDeviceMemory = (Button) target;
          this.ButtonLoadDeviceMemory.Click += new RoutedEventHandler(this.ButtonLoadDeviceMemory_Click);
          break;
        case 13:
          this.ButtonUpdateDataGrid = (Button) target;
          this.ButtonUpdateDataGrid.Click += new RoutedEventHandler(this.ButonUpdateGRID_Click);
          break;
        case 14:
          this.ButtonPrev = (Button) target;
          this.ButtonPrev.Click += new RoutedEventHandler(this.ButtonPrev_Memory_Click);
          break;
        case 15:
          this.ButtonNext = (Button) target;
          this.ButtonNext.Click += new RoutedEventHandler(this.ButtonNext_Memory_Click);
          break;
        case 16:
          this.ComboBoxAddress = (ComboBox) target;
          this.ComboBoxAddress.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxAddress_SelectionChanged);
          break;
        case 17:
          this.TextBoxAddressMemory = (TextBox) target;
          this.TextBoxAddressMemory.KeyDown += new KeyEventHandler(this.TextBoxes_KeyDown);
          this.TextBoxAddressMemory.LostFocus += new RoutedEventHandler(this.TextBoxes_LostFocus);
          this.TextBoxAddressMemory.GotFocus += new RoutedEventHandler(this.TextBoxes_GotFocus);
          break;
        case 18:
          this.ButtonGotoAddress = (Button) target;
          this.ButtonGotoAddress.Click += new RoutedEventHandler(this.ButtonGoto_Memory_Address_Click);
          break;
        case 19:
          this.ButtonCancel = (Button) target;
          this.ButtonCancel.Click += new RoutedEventHandler(this.ButtonCancel_Click);
          break;
        case 20:
          this.ButtonSaveDeviceMemoryToFile = (Button) target;
          this.ButtonSaveDeviceMemoryToFile.Click += new RoutedEventHandler(this.ButtonSaveDeviceMemoryToFile_Click);
          break;
        case 21:
          this.TextBoxStopwatch = (TextBox) target;
          break;
        case 22:
          this.CheckBoxShowHex = (CheckBox) target;
          this.CheckBoxShowHex.Click += new RoutedEventHandler(this.CheckBoxShowHex_Click);
          break;
        case 23:
          this.TextBoxSizeMemory = (TextBox) target;
          this.TextBoxSizeMemory.KeyDown += new KeyEventHandler(this.TextBoxes_KeyDown);
          this.TextBoxSizeMemory.LostFocus += new RoutedEventHandler(this.TextBoxes_LostFocus);
          this.TextBoxSizeMemory.GotFocus += new RoutedEventHandler(this.TextBoxes_GotFocus);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}

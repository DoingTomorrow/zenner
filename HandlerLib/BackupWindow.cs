// Decompiled with JetBrains decompiler
// Type: HandlerLib.BackupWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using GmmDbLib;
using GmmDbLib.DataSets;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public class BackupWindow : Window, IComponentConnector, IStyleConnector
  {
    private FirmwareType[] fwType;
    private static BackupWindow window;
    private ICreateMeter handler;
    private string[] hardwareName;
    private ObservableCollection<BaseTables.MeterRow> listMeter;
    private ObservableCollection<BaseTables.MeterDataRow> listMeterData;
    private bool isV2;
    private bool useDataFilter;
    private bool useSecondDB;
    internal TextBox txtInfo;
    internal DatePicker DatePickerProductionStartDate;
    internal DatePicker DatePickerProductionEndDate;
    internal TimeControl txtProductionStartTime;
    internal TimeControl txtProductionEndTime;
    internal Button ButtonStartSearch;
    internal Button ButtonStop;
    internal TextBox txtSerialnumber;
    internal TextBox txtMeterID;
    internal TextBox txtOrderNumber;
    internal DatePicker DatePickerApprovalStartDate;
    internal DatePicker DatePickerApprovalEndDate;
    internal TimeControl txtApprovalStartTime;
    internal TimeControl txtApprovalEndTime;
    internal Label LabelCount;
    internal DataGrid DataGridMeter;
    internal DataGrid DataGridMeterData;
    internal Button ButtonOpen;
    internal ComboBox ComboBoxFwTypes;
    private bool _contentLoaded;

    private BackupWindow()
    {
      this.InitializeComponent();
      this.txtProductionEndTime.DateTimeValue = new DateTime?(new DateTime(2000, 1, 1, 23, 59, 0));
      this.txtApprovalEndTime.DateTimeValue = new DateTime?(new DateTime(2000, 1, 1, 23, 59, 0));
      this.listMeter = new ObservableCollection<BaseTables.MeterRow>();
      this.listMeterData = new ObservableCollection<BaseTables.MeterDataRow>();
      this.DataGridMeter.DataContext = (object) this.listMeter;
      this.DataGridMeterData.DataContext = (object) this.listMeterData;
      this.useDataFilter = true;
      this.useSecondDB = false;
    }

    private void BackupWindow_Loaded(object sender, RoutedEventArgs e)
    {
      this.ButtonStartSearch_Click(sender, e);
    }

    public static byte[] ShowDialog(
      Window owner,
      ICreateMeter handler,
      FirmwareType fwType,
      bool isV2,
      bool useSecondDB = false)
    {
      return BackupWindow.ShowDialog(owner, handler, new FirmwareType[1]
      {
        fwType
      }, (isV2 ? 1 : 0) != 0, (useSecondDB ? 1 : 0) != 0);
    }

    public static byte[] ShowDialog(
      Window owner,
      ICreateMeter handler,
      FirmwareType[] fwType,
      bool isV2,
      bool useSecondDB = false)
    {
      return BackupWindow.ShowDialog(owner, handler, (string[]) null, fwType, isV2, useSecondDB);
    }

    public static byte[] ShowDialog(
      Window owner,
      ICreateMeter handler,
      string hardwareName,
      bool isV2)
    {
      return BackupWindow.ShowDialog(owner, handler, new string[1]
      {
        hardwareName
      }, (isV2 ? 1 : 0) != 0);
    }

    public static byte[] ShowDialog(
      Window owner,
      ICreateMeter handler,
      string[] hardwareName,
      bool isV2)
    {
      return BackupWindow.ShowDialog(owner, handler, hardwareName, (FirmwareType[]) null, isV2);
    }

    public static byte[] ShowDialog(Window owner, uint? meterID)
    {
      BackupWindow.window = new BackupWindow();
      BackupWindow.window.Owner = owner;
      BackupWindow.window.handler = (ICreateMeter) null;
      BackupWindow.window.hardwareName = new string[1]
      {
        string.Empty
      };
      BackupWindow.window.fwType = new FirmwareType[1];
      BackupWindow.window.isV2 = false;
      BackupWindow.window.useDataFilter = false;
      Device.MeterFound += new EventHandler<BaseTables.MeterRow>(BackupWindow.Device_MeterFound);
      Device.SearchDone += new EventHandler(BackupWindow.Device_SearchDone);
      BackupWindow.window.txtMeterID.Text = meterID.HasValue ? meterID.ToString() : throw new ArgumentException("Invalid call!");
      BackupWindow.window.ComboBoxFwTypes.SelectedIndex = 0;
      BackupWindow.window.ButtonOpen.IsEnabled = true;
      BackupWindow.window.txtMeterID.IsEnabled = false;
      BackupWindow.window.txtOrderNumber.IsEnabled = false;
      BackupWindow.window.txtProductionEndTime.IsEnabled = false;
      BackupWindow.window.txtProductionStartTime.IsEnabled = false;
      BackupWindow.window.txtSerialnumber.IsEnabled = false;
      BackupWindow.window.DatePickerApprovalEndDate.IsEnabled = false;
      BackupWindow.window.DatePickerApprovalStartDate.IsEnabled = false;
      BackupWindow.window.DatePickerProductionEndDate.IsEnabled = false;
      BackupWindow.window.DatePickerProductionStartDate.IsEnabled = false;
      BackupWindow.window.txtInfo.Visibility = Visibility.Hidden;
      BackupWindow.window.Loaded += new RoutedEventHandler(BackupWindow.window.BackupWindow_Loaded);
      try
      {
        return BackupWindow.window.ShowDialog().Value && BackupWindow.window.DataGridMeterData != null && BackupWindow.window.DataGridMeterData.SelectedCells.Any<DataGridCellInfo>() && BackupWindow.window.DataGridMeterData.SelectedCells[0].Item is BaseTables.MeterDataRow meterDataRow && !meterDataRow.IsPValueBinaryNull() ? meterDataRow.PValueBinary : (byte[]) null;
      }
      finally
      {
        Device.MeterFound -= new EventHandler<BaseTables.MeterRow>(BackupWindow.Device_MeterFound);
        Device.SearchDone -= new EventHandler(BackupWindow.Device_SearchDone);
        BackupWindow.window.Loaded -= new RoutedEventHandler(BackupWindow.window.BackupWindow_Loaded);
      }
    }

    private static byte[] ShowDialog(
      Window owner,
      ICreateMeter handler,
      string[] hardwareName,
      FirmwareType[] fwType,
      bool isV2,
      bool useSecondDB = false)
    {
      if (handler == null)
        throw new ArgumentNullException(nameof (handler));
      if (useSecondDB && DbBasis.SecondaryDB == null)
        throw new Exception("Secondary data base not defined");
      BackupWindow.window = new BackupWindow();
      BackupWindow.window.Owner = owner;
      BackupWindow.window.handler = handler;
      BackupWindow.window.hardwareName = hardwareName;
      BackupWindow.window.fwType = fwType;
      BackupWindow.window.isV2 = isV2;
      BackupWindow.window.useSecondDB = useSecondDB;
      Device.MeterFound += new EventHandler<BaseTables.MeterRow>(BackupWindow.Device_MeterFound);
      Device.SearchDone += new EventHandler(BackupWindow.Device_SearchDone);
      if (fwType != null)
        BackupWindow.window.ComboBoxFwTypes.ItemsSource = (IEnumerable) fwType;
      else
        BackupWindow.window.ComboBoxFwTypes.ItemsSource = hardwareName != null ? (IEnumerable) hardwareName : throw new ArgumentException("Invalid call!");
      BackupWindow.window.ComboBoxFwTypes.SelectedIndex = 0;
      try
      {
        return BackupWindow.window.ShowDialog().Value && BackupWindow.window.DataGridMeterData != null && BackupWindow.window.DataGridMeterData.SelectedCells.Any<DataGridCellInfo>() && BackupWindow.window.DataGridMeterData.SelectedCells[0].Item is BaseTables.MeterDataRow meterDataRow && !meterDataRow.IsPValueBinaryNull() ? meterDataRow.PValueBinary : (byte[]) null;
      }
      finally
      {
        Device.MeterFound -= new EventHandler<BaseTables.MeterRow>(BackupWindow.Device_MeterFound);
        Device.SearchDone -= new EventHandler(BackupWindow.Device_SearchDone);
      }
    }

    private static void Device_SearchDone(object sender, EventArgs e)
    {
      if (!BackupWindow.window.CheckAccess())
      {
        BackupWindow.window.Dispatcher.Invoke((Action) (() => BackupWindow.Device_SearchDone(sender, e)));
      }
      else
      {
        BackupWindow.window.ButtonStop.IsEnabled = false;
        BackupWindow.window.ButtonStartSearch.IsEnabled = true;
        if (BackupWindow.window.listMeter.Count > 0)
        {
          DataGrid dataGridMeter = BackupWindow.window.DataGridMeter;
          dataGridMeter.CurrentCell = new DataGridCellInfo(dataGridMeter.Items[0], dataGridMeter.Columns[0]);
          dataGridMeter.SelectedCells.Add(dataGridMeter.CurrentCell);
        }
        else
        {
          BackupWindow.window.LabelCount.Content = (object) 0;
          int num = (int) MessageBox.Show((Window) BackupWindow.window, "No results!");
        }
      }
    }

    private static void Device_MeterFound(object sender, BaseTables.MeterRow e)
    {
      if (!BackupWindow.window.CheckAccess())
      {
        BackupWindow.window.Dispatcher.Invoke((Action) (() => BackupWindow.Device_MeterFound(sender, e)));
      }
      else
      {
        BackupWindow.window.listMeter.Add(e);
        BackupWindow.window.LabelCount.Content = (object) BackupWindow.window.listMeter.Count;
      }
    }

    private void ButtonStartSearch_Click(object sender, RoutedEventArgs e)
    {
      this.listMeter.Clear();
      this.txtInfo.Text = string.Empty;
      BackupWindow.window.ButtonStartSearch.IsEnabled = false;
      BackupWindow.window.ButtonStop.IsEnabled = true;
      string str1 = this.txtSerialnumber.Text;
      if (!string.IsNullOrEmpty(str1) && str1.IndexOf(" ") > -1)
        str1 = str1.Replace(" ", "").Trim();
      string str2 = this.txtMeterID.Text;
      if (!string.IsNullOrEmpty(str2) && str2.IndexOf(" ") > -1)
        str2 = str2.Replace(" ", "").Trim();
      DateTime? selectedDate1 = this.DatePickerProductionStartDate.SelectedDate;
      DateTime? selectedDate2 = this.DatePickerProductionEndDate.SelectedDate;
      DateTime? dateTimeValue1 = this.txtProductionStartTime.DateTimeValue;
      DateTime? dateTimeValue2 = this.txtProductionEndTime.DateTimeValue;
      DateTime? selectedDate3 = this.DatePickerApprovalStartDate.SelectedDate;
      DateTime? selectedDate4 = this.DatePickerApprovalEndDate.SelectedDate;
      DateTime? dateTimeValue3 = this.txtApprovalStartTime.DateTimeValue;
      DateTime? dateTimeValue4 = this.txtApprovalEndTime.DateTimeValue;
      string str3 = str1;
      string str4 = str2;
      string text = this.txtOrderNumber.Text;
      DateTime? nullable1 = BackupWindow.ConcatDateTime(selectedDate1, dateTimeValue1);
      DateTime? nullable2 = BackupWindow.ConcatDateTime(selectedDate2, dateTimeValue2);
      DateTime? nullable3 = BackupWindow.ConcatDateTime(selectedDate3, dateTimeValue3);
      DateTime? nullable4 = BackupWindow.ConcatDateTime(selectedDate4, dateTimeValue4);
      string empty = string.Empty;
      FirmwareType firmwareType = FirmwareType.None;
      if (BackupWindow.window.ComboBoxFwTypes.SelectedItem != null)
      {
        if (BackupWindow.window.ComboBoxFwTypes.SelectedItem is FirmwareType)
          firmwareType = (FirmwareType) BackupWindow.window.ComboBoxFwTypes.SelectedItem;
        else
          empty = BackupWindow.window.ComboBoxFwTypes.SelectedItem.ToString();
      }
      else
        empty = string.Empty;
      BaseDbConnection db = !this.useSecondDB ? DbBasis.PrimaryDB.BaseDbConnection : DbBasis.SecondaryDB.BaseDbConnection;
      DeviceSearchFilter filter = new DeviceSearchFilter();
      filter.ProductionStartDate = nullable1;
      filter.ProductionEndDate = nullable2;
      filter.ApprovalStartDate = nullable3;
      filter.ApprovalEndDate = nullable4;
      filter.Serialnumber = str3;
      filter.MeterID = str4;
      filter.OrderNumber = text;
      filter.HardwareName = empty;
      filter.FwType = firmwareType;
      filter.IsOldVersion = !this.isV2;
      int num = this.useDataFilter ? 1 : 0;
      Device.StartSearch(db, filter, num != 0);
    }

    private void ButtonStop_Click(object sender, RoutedEventArgs e)
    {
      Device.CancelSearch();
      BackupWindow.window.ButtonStop.IsEnabled = false;
      BackupWindow.window.ButtonStartSearch.IsEnabled = true;
    }

    private void ButtonOpen_Click(object sender, RoutedEventArgs e)
    {
      if (this.DataGridMeterData == null || !this.DataGridMeterData.SelectedCells.Any<DataGridCellInfo>() || !(this.DataGridMeterData.SelectedCells[0].Item is BaseTables.MeterDataRow))
        return;
      this.DialogResult = new bool?(true);
      this.Close();
    }

    private static DateTime? ConcatDateTime(DateTime? date, DateTime? time)
    {
      if (!date.HasValue || !time.HasValue)
        return new DateTime?();
      DateTime dateTime = date.Value;
      int year = dateTime.Year;
      dateTime = date.Value;
      int month = dateTime.Month;
      dateTime = date.Value;
      int day = dateTime.Day;
      dateTime = time.Value;
      int hour = dateTime.Hour;
      dateTime = time.Value;
      int minute = dateTime.Minute;
      return new DateTime?(new DateTime(year, month, day, hour, minute, 0));
    }

    private void DataGridMeter_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
    {
      this.listMeterData.Clear();
      if (this.DataGridMeter == null || !this.DataGridMeter.SelectedCells.Any<DataGridCellInfo>())
        return;
      BaseTables.MeterRow meterRow = this.DataGridMeter.SelectedCells[0].Item as BaseTables.MeterRow;
      List<BaseTables.MeterDataRow> source = MeterData.LoadMeterData(!this.useSecondDB ? DbBasis.PrimaryDB.BaseDbConnection : DbBasis.SecondaryDB.BaseDbConnection, meterRow.MeterID);
      if (source == null)
        return;
      foreach (BaseTables.MeterDataRow meterDataRow in source.OrderByDescending<BaseTables.MeterDataRow, DateTime>((Func<BaseTables.MeterDataRow, DateTime>) (o => o.TimePoint)).ToList<BaseTables.MeterDataRow>())
        this.listMeterData.Add(meterDataRow);
      DataGrid dataGridMeterData = this.DataGridMeterData;
      dataGridMeterData.CurrentCell = new DataGridCellInfo(dataGridMeterData.Items[0], dataGridMeterData.Columns[0]);
      dataGridMeterData.SelectedCells.Add(dataGridMeterData.CurrentCell);
    }

    private void DataGridMeterData_SelectedCellsChanged(
      object sender,
      SelectedCellsChangedEventArgs e)
    {
      this.txtInfo.Text = string.Empty;
      if (this.DataGridMeterData == null || !this.DataGridMeterData.SelectedCells.Any<DataGridCellInfo>())
        return;
      BaseTables.MeterDataRow meterData = this.DataGridMeterData.SelectedCells[0].Item as BaseTables.MeterDataRow;
      if (meterData.IsPValueBinaryNull())
        return;
      try
      {
        if (Enum.IsDefined(typeof (MeterData.Special), (object) meterData.PValueID))
        {
          this.txtInfo.Text = (this.handler as IMeterDataSpecial).Create(meterData).ToString();
        }
        else
        {
          if (this.handler == null)
            return;
          IMeter meter = this.handler.CreateMeter(meterData.PValueBinary);
          if (meter == null)
            return;
          this.txtInfo.Text = meter.GetInfo();
        }
      }
      catch (Exception ex)
      {
        this.txtInfo.Text = ex.Message;
      }
    }

    private void DatePickerProductionStartDate_SelectedDateChanged(
      object sender,
      SelectionChangedEventArgs e)
    {
      if (!this.DatePickerProductionStartDate.SelectedDate.HasValue || this.DatePickerProductionEndDate.SelectedDate.HasValue)
        return;
      this.DatePickerProductionEndDate.SelectedDate = this.DatePickerProductionStartDate.SelectedDate;
    }

    private void DatePickerApprovalStartDate_SelectedDateChanged(
      object sender,
      SelectionChangedEventArgs e)
    {
      if (!this.DatePickerApprovalStartDate.SelectedDate.HasValue || this.DatePickerApprovalEndDate.SelectedDate.HasValue)
        return;
      this.DatePickerApprovalEndDate.SelectedDate = this.DatePickerApprovalStartDate.SelectedDate;
    }

    private void Input_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return)
        return;
      this.ButtonStartSearch_Click((object) this, (RoutedEventArgs) null);
    }

    private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
    {
      this.ButtonOpen_Click(sender, (RoutedEventArgs) null);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/backupwindow.xaml", UriKind.Relative));
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
          this.txtInfo = (TextBox) target;
          break;
        case 2:
          this.DatePickerProductionStartDate = (DatePicker) target;
          this.DatePickerProductionStartDate.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(this.DatePickerProductionStartDate_SelectedDateChanged);
          break;
        case 3:
          this.DatePickerProductionEndDate = (DatePicker) target;
          break;
        case 4:
          this.txtProductionStartTime = (TimeControl) target;
          break;
        case 5:
          this.txtProductionEndTime = (TimeControl) target;
          break;
        case 6:
          this.ButtonStartSearch = (Button) target;
          this.ButtonStartSearch.Click += new RoutedEventHandler(this.ButtonStartSearch_Click);
          break;
        case 7:
          this.ButtonStop = (Button) target;
          this.ButtonStop.Click += new RoutedEventHandler(this.ButtonStop_Click);
          break;
        case 8:
          this.txtSerialnumber = (TextBox) target;
          this.txtSerialnumber.KeyDown += new KeyEventHandler(this.Input_KeyDown);
          break;
        case 9:
          this.txtMeterID = (TextBox) target;
          this.txtMeterID.KeyDown += new KeyEventHandler(this.Input_KeyDown);
          break;
        case 10:
          this.txtOrderNumber = (TextBox) target;
          this.txtOrderNumber.KeyDown += new KeyEventHandler(this.Input_KeyDown);
          break;
        case 11:
          this.DatePickerApprovalStartDate = (DatePicker) target;
          this.DatePickerApprovalStartDate.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(this.DatePickerApprovalStartDate_SelectedDateChanged);
          break;
        case 12:
          this.DatePickerApprovalEndDate = (DatePicker) target;
          break;
        case 13:
          this.txtApprovalStartTime = (TimeControl) target;
          break;
        case 14:
          this.txtApprovalEndTime = (TimeControl) target;
          break;
        case 15:
          this.LabelCount = (Label) target;
          break;
        case 16:
          this.DataGridMeter = (DataGrid) target;
          this.DataGridMeter.SelectedCellsChanged += new SelectedCellsChangedEventHandler(this.DataGridMeter_SelectedCellsChanged);
          break;
        case 17:
          this.DataGridMeterData = (DataGrid) target;
          this.DataGridMeterData.SelectedCellsChanged += new SelectedCellsChangedEventHandler(this.DataGridMeterData_SelectedCellsChanged);
          break;
        case 19:
          this.ButtonOpen = (Button) target;
          this.ButtonOpen.Click += new RoutedEventHandler(this.ButtonOpen_Click);
          break;
        case 20:
          this.ComboBoxFwTypes = (ComboBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IStyleConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 18)
        return;
      ((Style) target).Setters.Add((SetterBase) new EventSetter()
      {
        Event = Control.MouseDoubleClickEvent,
        Handler = (Delegate) new MouseButtonEventHandler(this.Row_DoubleClick)
      });
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: S3_Handler.OpenDevice
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZR_ClassLibrary;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace S3_Handler
{
  public class OpenDevice : Form
  {
    private static bool LastValuesAvailable;
    private static DateTime LastProductionDateFrom;
    private static DateTime LastProductionDateTo;
    private static bool LastUseProductionDateRange;
    private static DateTime LastApprovalDateFrom;
    private static DateTime LastApprovalDateTo;
    private static bool LastUseApprovalDateRange;
    private static string LastSerialNumber;
    private static string LastSapNumber;
    private static string LastMeterId;
    private static DateTime LastOptenedTime;
    private S3_HandlerFunctions MyFunctions;
    private DataTable MyMeterTabel;
    private int SelectedMeterRowIndex;
    internal DataTable SaveMeterDataTabel;
    internal int SelectedSaveMeterDataTableRowIndex;
    internal int MeterId;
    internal int HardwareTypeId;
    internal DateTime DataTimePoint;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private DataGridView dataGridViewMeters;
    private DataGridView dataGridViewMeterData;
    private Label label1;
    private Label label2;
    private TextBox textBoxSerialNumber;
    private Label label3;
    private Button buttonSearchSerialNumber;
    private Label label4;
    private TextBox textBoxMeterId;
    private Button buttonSearchMeterId;
    private Button buttonOpenDevice;
    private TextBox textBoxStateInfo;
    private Label label5;
    private GroupBox groupBoxDateRange;
    private DateTimePicker dateTimePickerProductionDateTo;
    private DateTimePicker dateTimePickerProductionDateFrom;
    private Label label7;
    private Label label6;
    private CheckBox checkBoxUseProductionDateRange;
    private GroupBox groupBoxDateRangeApprovalDate;
    private CheckBox checkBoxUseApprovalDateRange;
    private DateTimePicker dateTimePickerApprovalDateTo;
    private DateTimePicker dateTimePickerApprovalDateFrom;
    private Label label8;
    private Label label9;
    private GroupBox groupBox1;
    private Button buttonSearchSapNumber;
    private Label label10;
    private TextBox textBoxSapNumber;
    private Button buttonSearchHardwareTypeId;
    private Label label11;
    private TextBox textBoxHardwareTypeId;

    public OpenDevice(S3_HandlerFunctions MyFunctions)
    {
      this.MyFunctions = MyFunctions;
      this.InitializeComponent();
      if (!OpenDevice.LastValuesAvailable)
      {
        DateTimePicker productionDateFrom = this.dateTimePickerProductionDateFrom;
        DateTime dateTime1 = DateTime.Now;
        dateTime1 = dateTime1.Date;
        DateTime dateTime2 = dateTime1.AddDays(-7.0);
        productionDateFrom.Value = dateTime2;
        DateTimePicker productionDateTo = this.dateTimePickerProductionDateTo;
        dateTime1 = DateTime.Now;
        dateTime1 = dateTime1.AddDays(1.0);
        DateTime date1 = dateTime1.Date;
        productionDateTo.Value = date1;
        DateTimePicker approvalDateFrom = this.dateTimePickerApprovalDateFrom;
        dateTime1 = DateTime.Now;
        dateTime1 = dateTime1.Date;
        DateTime dateTime3 = dateTime1.AddDays(-7.0);
        approvalDateFrom.Value = dateTime3;
        DateTimePicker pickerApprovalDateTo = this.dateTimePickerApprovalDateTo;
        dateTime1 = DateTime.Now;
        dateTime1 = dateTime1.AddDays(1.0);
        DateTime date2 = dateTime1.Date;
        pickerApprovalDateTo.Value = date2;
      }
      else
      {
        this.dateTimePickerProductionDateFrom.Value = OpenDevice.LastProductionDateFrom;
        this.dateTimePickerProductionDateTo.Value = OpenDevice.LastProductionDateTo;
        this.checkBoxUseProductionDateRange.Checked = OpenDevice.LastUseProductionDateRange;
        this.dateTimePickerApprovalDateFrom.Value = OpenDevice.LastApprovalDateFrom;
        this.dateTimePickerApprovalDateTo.Value = OpenDevice.LastApprovalDateTo;
        this.checkBoxUseApprovalDateRange.Checked = OpenDevice.LastUseApprovalDateRange;
        this.textBoxSerialNumber.Text = OpenDevice.LastSerialNumber;
        this.textBoxSapNumber.Text = OpenDevice.LastSapNumber;
        this.textBoxMeterId.Text = OpenDevice.LastMeterId;
      }
    }

    private void SaveSettings()
    {
      OpenDevice.LastProductionDateFrom = this.dateTimePickerProductionDateFrom.Value;
      OpenDevice.LastProductionDateTo = this.dateTimePickerProductionDateTo.Value;
      OpenDevice.LastUseProductionDateRange = this.checkBoxUseProductionDateRange.Checked;
      OpenDevice.LastApprovalDateFrom = this.dateTimePickerApprovalDateFrom.Value;
      OpenDevice.LastApprovalDateTo = this.dateTimePickerApprovalDateTo.Value;
      OpenDevice.LastUseApprovalDateRange = this.checkBoxUseApprovalDateRange.Checked;
      OpenDevice.LastSerialNumber = this.textBoxSerialNumber.Text;
      OpenDevice.LastSapNumber = this.textBoxSapNumber.Text;
      OpenDevice.LastMeterId = this.textBoxMeterId.Text;
      OpenDevice.LastValuesAvailable = true;
    }

    private void OpenDevice_Load(object sender, EventArgs e) => this.textBoxSerialNumber.Select();

    private void buttonSearchSerialNumber_Click(object sender, EventArgs e)
    {
      this.SearchNumber(this.textBoxSerialNumber, OpenDevice.SearchNumberType.SerialNumber);
    }

    private void buttonSearchMeterId_Click(object sender, EventArgs e)
    {
      this.SearchNumber(this.textBoxMeterId, OpenDevice.SearchNumberType.MeterId);
    }

    private void buttonSearchSapNumber_Click(object sender, EventArgs e)
    {
      this.SearchNumber(this.textBoxSapNumber, OpenDevice.SearchNumberType.SapNumber);
    }

    private void textBoxHardwareTypeId_KeyPress(object sender, KeyPressEventArgs e)
    {
    }

    private void buttonSearchHardwareTypeId_Click(object sender, EventArgs e)
    {
      this.SearchNumber(this.textBoxHardwareTypeId, OpenDevice.SearchNumberType.HardwareTypeId);
    }

    private void SearchNumber(TextBox Selection, OpenDevice.SearchNumberType numberType)
    {
      this.textBoxStateInfo.Text = "Searching ...";
      this.Refresh();
      this.SaveSettings();
      if (numberType == OpenDevice.SearchNumberType.MeterId && !int.TryParse(Selection.Text, out int _))
      {
        Selection.Text = "0";
      }
      else
      {
        try
        {
          StringBuilder stringBuilder1 = new StringBuilder();
          int num;
          if (numberType != OpenDevice.SearchNumberType.HardwareTypeId)
          {
            stringBuilder1.Append("SELECT  Meter.SerialNr AS SerialNumber ,Meter.MeterID AS MeterId ,Meter.MeterInfoID AS MeterInfoID ,MeterInfo.PPSArtikelNr AS SAP_Number ,Meter.ProductionDate AS ProductionDate ,Meter.ApprovalDate AS ApprovalDate ,Meter.OrderNr AS OrderNr FROM Meter, MeterInfo, HardwareType WHERE Meter.MeterInfoID = MeterInfo.MeterInfoID AND MeterInfo.HardwareTypeID = HardwareType.HardwareTypeID AND HardwareType.HardwareResource LIKE ';Serie3;%'");
            if (numberType == OpenDevice.SearchNumberType.SerialNumber || numberType == OpenDevice.SearchNumberType.SapNumber)
            {
              if (this.checkBoxUseProductionDateRange.Checked)
                stringBuilder1.Append(" AND ProductionDate >= @FromProductionTime  AND ProductionDate <= @ToProductionTime");
              if (this.checkBoxUseApprovalDateRange.Checked)
                stringBuilder1.Append(" AND ApprovalDate >= @FromApprovalTime  AND ApprovalDate <= @ToApprovalTime");
              switch (numberType)
              {
                case OpenDevice.SearchNumberType.SerialNumber:
                  stringBuilder1.Append(" AND Meter.SerialNr LIKE '" + Selection.Text + "' ORDER BY Meter.SerialNr ASC");
                  break;
                case OpenDevice.SearchNumberType.SapNumber:
                  stringBuilder1.Append(" AND MeterInfo.PPSArtikelNr LIKE '" + Selection.Text + "' ORDER BY MeterInfo.PPSArtikelNr ASC");
                  break;
              }
            }
            else
              stringBuilder1.Append(" AND Meter.MeterId = " + Selection.Text + " ORDER BY Meter.MeterId ASC");
          }
          else
          {
            StringBuilder stringBuilder2 = stringBuilder1;
            num = 1;
            string str = "SELECT  DISTINCT Meter.MeterID ,Meter.MeterID AS MeterId ,Meter.SerialNr AS SerialNumber ,Meter.MeterInfoID AS MeterInfoID ,MeterInfo.PPSArtikelNr AS SAP_Number ,MeterInfo.Description AS TypeDescription ,Meter.ProductionDate AS ProductionDate ,Meter.ApprovalDate AS ApprovalDate ,Meter.OrderNr AS OrderNr FROM Meter, MeterInfo, HardwareType, MeterData WHERE Meter.MeterInfoID = MeterInfo.MeterInfoID AND MeterInfo.HardwareTypeID = HardwareType.HardwareTypeID AND HardwareType.HardwareResource LIKE ';Serie3;%' AND MeterData.MeterID = Meter.MeterID AND MeterData.PValueID = " + num.ToString() + " AND MeterData.PValue = " + Selection.Text;
            stringBuilder2.Append(str);
            if (this.checkBoxUseProductionDateRange.Checked)
              stringBuilder1.Append(" AND ProductionDate >= @FromProductionTime  AND ProductionDate <= @ToProductionTime");
            if (this.checkBoxUseApprovalDateRange.Checked)
              stringBuilder1.Append(" AND ApprovalDate >= @FromApprovalTime  AND ApprovalDate <= @ToApprovalTime");
            stringBuilder1.Append(" ORDER BY MeterInfo.PPSArtikelNr ASC");
          }
          DbDataAdapter dataAdapter = this.MyFunctions.MyDatabase.BaseDb.GetDataAdapter(stringBuilder1.ToString(), this.MyFunctions.MyDatabase.DbConn);
          DbCommand selectCommand = dataAdapter.SelectCommand;
          DateTime dateTime;
          if (this.checkBoxUseProductionDateRange.Checked)
          {
            IDbDataParameter parameter1 = (IDbDataParameter) selectCommand.CreateParameter();
            parameter1.DbType = DbType.DateTime;
            parameter1.ParameterName = "@FromProductionTime";
            IDbDataParameter dbDataParameter1 = parameter1;
            dateTime = this.dateTimePickerProductionDateFrom.Value;
            // ISSUE: variable of a boxed type
            __Boxed<DateTime> universalTime1 = (System.ValueType) dateTime.ToUniversalTime();
            dbDataParameter1.Value = (object) universalTime1;
            selectCommand.Parameters.Add((object) parameter1);
            IDbDataParameter parameter2 = (IDbDataParameter) selectCommand.CreateParameter();
            parameter2.DbType = DbType.DateTime;
            parameter2.ParameterName = "@ToProductionTime";
            IDbDataParameter dbDataParameter2 = parameter2;
            dateTime = this.dateTimePickerProductionDateTo.Value;
            dateTime = dateTime.AddDays(1.0);
            // ISSUE: variable of a boxed type
            __Boxed<DateTime> universalTime2 = (System.ValueType) dateTime.ToUniversalTime();
            dbDataParameter2.Value = (object) universalTime2;
            selectCommand.Parameters.Add((object) parameter2);
          }
          if (this.checkBoxUseApprovalDateRange.Checked)
          {
            IDbDataParameter parameter3 = (IDbDataParameter) selectCommand.CreateParameter();
            parameter3.DbType = DbType.DateTime;
            parameter3.ParameterName = "@FromApprovalTime";
            IDbDataParameter dbDataParameter3 = parameter3;
            dateTime = this.dateTimePickerApprovalDateFrom.Value;
            // ISSUE: variable of a boxed type
            __Boxed<DateTime> universalTime3 = (System.ValueType) dateTime.ToUniversalTime();
            dbDataParameter3.Value = (object) universalTime3;
            selectCommand.Parameters.Add((object) parameter3);
            IDbDataParameter parameter4 = (IDbDataParameter) selectCommand.CreateParameter();
            parameter4.DbType = DbType.DateTime;
            parameter4.ParameterName = "@ToApprovalTime";
            IDbDataParameter dbDataParameter4 = parameter4;
            dateTime = this.dateTimePickerApprovalDateTo.Value;
            dateTime = dateTime.AddDays(1.0);
            // ISSUE: variable of a boxed type
            __Boxed<DateTime> universalTime4 = (System.ValueType) dateTime.ToUniversalTime();
            dbDataParameter4.Value = (object) universalTime4;
            selectCommand.Parameters.Add((object) parameter4);
          }
          this.MyMeterTabel = new DataTable();
          dataAdapter.Fill(this.MyMeterTabel);
          if (this.MyMeterTabel.Rows.Count == 0)
          {
            this.textBoxStateInfo.Text = "No data";
          }
          else
          {
            TextBox textBoxStateInfo = this.textBoxStateInfo;
            num = this.MyMeterTabel.Rows.Count;
            string str = "Number of meters: " + num.ToString();
            textBoxStateInfo.Text = str;
          }
          this.dataGridViewMeters.DataSource = (object) this.MyMeterTabel;
        }
        catch (Exception ex)
        {
          int num = (int) GMM_MessageBox.ShowMessage("S3_Handler", ex.ToString());
        }
      }
    }

    private void dataGridViewMeters_SelectionChanged(object sender, EventArgs e)
    {
      try
      {
        if (this.dataGridViewMeters.SelectedCells.Count == 0)
        {
          this.dataGridViewMeterData.DataSource = (object) null;
        }
        else
        {
          if (this.dataGridViewMeterData.DataSource == null)
            this.SelectedMeterRowIndex = -1;
          int rowIndex1 = this.dataGridViewMeters.SelectedCells[0].RowIndex;
          if (rowIndex1 == this.SelectedMeterRowIndex)
            return;
          this.SelectedMeterRowIndex = rowIndex1;
          this.MeterId = int.Parse(this.dataGridViewMeters["MeterId", this.SelectedMeterRowIndex].Value.ToString());
          string SQLCommand = "SELECT TimePoint,PValue AS HardwareTypeId FROM MeterData WHERE MeterID = " + this.MeterId.ToString() + " AND PValueID = " + 1.ToString() + " ORDER BY TimePoint DESC";
          this.SaveMeterDataTabel = new DataTable();
          this.MyFunctions.MyDatabase.GetDataTableBySQLCommand(SQLCommand, this.SaveMeterDataTabel);
          this.dataGridViewMeterData.DataSource = (object) this.SaveMeterDataTabel;
          if (OpenDevice.LastValuesAvailable && OpenDevice.LastOptenedTime != DateTime.MinValue && !string.IsNullOrEmpty(OpenDevice.LastMeterId) && this.MeterId == int.Parse(OpenDevice.LastMeterId))
          {
            for (int rowIndex2 = 0; rowIndex2 < this.dataGridViewMeterData.RowCount; ++rowIndex2)
            {
              if ((DateTime) this.dataGridViewMeterData["TimePoint", rowIndex2].Value == OpenDevice.LastOptenedTime)
              {
                this.dataGridViewMeterData[0, rowIndex2].Selected = true;
                break;
              }
            }
          }
          this.dataGridViewMeterData.Columns[0].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm:ss";
          this.dataGridViewMeterData.Focus();
        }
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("S3_Handler", ex.ToString());
      }
    }

    private void dataGridViewMeterData_SelectionChanged(object sender, EventArgs e)
    {
      try
      {
        this.textBoxStateInfo.Clear();
        this.buttonOpenDevice.Enabled = false;
        this.SelectedSaveMeterDataTableRowIndex = -1;
        if (this.dataGridViewMeterData.SelectedCells.Count == 0)
          return;
        if (this.SelectedSaveMeterDataTableRowIndex < 0)
          this.SelectedSaveMeterDataTableRowIndex = this.dataGridViewMeterData.SelectedCells[0].RowIndex;
        this.DataTimePoint = (DateTime) this.dataGridViewMeterData["TimePoint", this.SelectedSaveMeterDataTableRowIndex].Value;
        this.HardwareTypeId = int.Parse((string) this.dataGridViewMeterData["HardwareTypeId", this.SelectedSaveMeterDataTableRowIndex].Value);
        string SQLCommand = "SELECT * FROM HardwareType WHERE HardwareTypeId = " + this.HardwareTypeId.ToString();
        Schema.HardwareTypeDataTable Table = new Schema.HardwareTypeDataTable();
        this.MyFunctions.MyDatabase.GetDataTableBySQLCommand(SQLCommand, (DataTable) Table);
        if (Table.Count == 0)
          this.textBoxStateInfo.Text = "Hardware not defined";
        else if (!Table[0].HardwareResource.Contains(";Serie3;"))
        {
          this.textBoxStateInfo.Text = "Not a series3 device";
        }
        else
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.AppendLine("*** Serie3 device ***");
          stringBuilder.AppendLine();
          stringBuilder.AppendLine("FirmwareVersion = " + ParameterService.GetVersionString((long) Table[0].FirmwareVersion, 8));
          stringBuilder.AppendLine("HardwareTypeId = " + Table[0].HardwareTypeID.ToString());
          stringBuilder.AppendLine("MapId = " + Table[0].MapID.ToString());
          if (!Table[0].IsDescriptionNull())
          {
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Description:");
            stringBuilder.AppendLine(Table[0].Description);
          }
          if (!Table[0].IsHardwareResourceNull())
          {
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("HardwareResource:");
            stringBuilder.AppendLine(Table[0].HardwareResource);
          }
          this.textBoxStateInfo.Text = stringBuilder.ToString();
          this.buttonOpenDevice.Enabled = true;
        }
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("S3_Handler", ex.ToString());
      }
    }

    private void textBoxSerialNumber_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\r')
        return;
      this.SearchNumber(this.textBoxSerialNumber, OpenDevice.SearchNumberType.SerialNumber);
      e.Handled = true;
    }

    private void textBoxMeterId_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\r')
        return;
      this.SearchNumber(this.textBoxMeterId, OpenDevice.SearchNumberType.MeterId);
      e.Handled = true;
    }

    private void textBoxSapNumber_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\r')
        return;
      this.SearchNumber(this.textBoxSapNumber, OpenDevice.SearchNumberType.SapNumber);
      e.Handled = true;
    }

    private void dataGridViewMeterData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex < 0)
        return;
      this.DataTimePoint = (DateTime) this.dataGridViewMeterData[0, e.RowIndex].Value;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void buttonOpenDevice_Click(object sender, EventArgs e) => this.LoadSelectedData();

    private void dataGridViewMeterData_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyValue != 13)
        return;
      this.LoadSelectedData();
      e.Handled = true;
    }

    private void LoadSelectedData()
    {
      OpenDevice.LastOptenedTime = this.DataTimePoint;
      if (this.buttonOpenDevice.Enabled)
        this.DialogResult = DialogResult.OK;
      else
        this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void dataGridViewMeters_CellFormatting(
      object sender,
      DataGridViewCellFormattingEventArgs e)
    {
      if (!(e.Value is DateTime))
        return;
      DateTime localTime = ((DateTime) e.Value).ToLocalTime();
      e.Value = (object) localTime.ToString();
      e.FormattingApplied = true;
    }

    private void dataGridViewMeterData_CellFormatting(
      object sender,
      DataGridViewCellFormattingEventArgs e)
    {
      if (!(e.Value is DateTime))
        return;
      DateTime localTime = ((DateTime) e.Value).ToLocalTime();
      e.Value = (object) localTime.ToString();
      e.FormattingApplied = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.dataGridViewMeters = new DataGridView();
      this.dataGridViewMeterData = new DataGridView();
      this.label1 = new Label();
      this.label2 = new Label();
      this.textBoxSerialNumber = new TextBox();
      this.label3 = new Label();
      this.buttonSearchSerialNumber = new Button();
      this.label4 = new Label();
      this.textBoxMeterId = new TextBox();
      this.buttonSearchMeterId = new Button();
      this.buttonOpenDevice = new Button();
      this.textBoxStateInfo = new TextBox();
      this.label5 = new Label();
      this.groupBoxDateRange = new GroupBox();
      this.checkBoxUseProductionDateRange = new CheckBox();
      this.dateTimePickerProductionDateTo = new DateTimePicker();
      this.dateTimePickerProductionDateFrom = new DateTimePicker();
      this.label7 = new Label();
      this.label6 = new Label();
      this.groupBoxDateRangeApprovalDate = new GroupBox();
      this.checkBoxUseApprovalDateRange = new CheckBox();
      this.dateTimePickerApprovalDateTo = new DateTimePicker();
      this.dateTimePickerApprovalDateFrom = new DateTimePicker();
      this.label8 = new Label();
      this.label9 = new Label();
      this.groupBox1 = new GroupBox();
      this.buttonSearchHardwareTypeId = new Button();
      this.buttonSearchSapNumber = new Button();
      this.label11 = new Label();
      this.label10 = new Label();
      this.textBoxHardwareTypeId = new TextBox();
      this.textBoxSapNumber = new TextBox();
      ((ISupportInitialize) this.dataGridViewMeters).BeginInit();
      ((ISupportInitialize) this.dataGridViewMeterData).BeginInit();
      this.groupBoxDateRange.SuspendLayout();
      this.groupBoxDateRangeApprovalDate.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(952, 105);
      this.zennerCoroprateDesign2.TabIndex = 16;
      this.dataGridViewMeters.AllowUserToAddRows = false;
      this.dataGridViewMeters.AllowUserToDeleteRows = false;
      this.dataGridViewMeters.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dataGridViewMeters.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.dataGridViewMeters.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewMeters.Location = new Point(12, 76);
      this.dataGridViewMeters.Name = "dataGridViewMeters";
      this.dataGridViewMeters.ReadOnly = true;
      this.dataGridViewMeters.Size = new Size(652, 417);
      this.dataGridViewMeters.TabIndex = 17;
      this.dataGridViewMeters.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dataGridViewMeters_CellFormatting);
      this.dataGridViewMeters.SelectionChanged += new System.EventHandler(this.dataGridViewMeters_SelectionChanged);
      this.dataGridViewMeterData.AllowUserToAddRows = false;
      this.dataGridViewMeterData.AllowUserToDeleteRows = false;
      this.dataGridViewMeterData.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dataGridViewMeterData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.dataGridViewMeterData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewMeterData.Location = new Point(13, 526);
      this.dataGridViewMeterData.MultiSelect = false;
      this.dataGridViewMeterData.Name = "dataGridViewMeterData";
      this.dataGridViewMeterData.ReadOnly = true;
      this.dataGridViewMeterData.Size = new Size(651, 148);
      this.dataGridViewMeterData.TabIndex = 18;
      this.dataGridViewMeterData.CellDoubleClick += new DataGridViewCellEventHandler(this.dataGridViewMeterData_CellDoubleClick);
      this.dataGridViewMeterData.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dataGridViewMeterData_CellFormatting);
      this.dataGridViewMeterData.SelectionChanged += new System.EventHandler(this.dataGridViewMeterData_SelectionChanged);
      this.dataGridViewMeterData.KeyDown += new KeyEventHandler(this.dataGridViewMeterData_KeyDown);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(13, 50);
      this.label1.Name = "label1";
      this.label1.Size = new Size(93, 13);
      this.label1.TabIndex = 19;
      this.label1.Text = "Searched devices";
      this.label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(13, 505);
      this.label2.Name = "label2";
      this.label2.Size = new Size(96, 13);
      this.label2.TabIndex = 19;
      this.label2.Text = "Available data sets";
      this.textBoxSerialNumber.Location = new Point(82, 230);
      this.textBoxSerialNumber.Name = "textBoxSerialNumber";
      this.textBoxSerialNumber.Size = new Size(101, 20);
      this.textBoxSerialNumber.TabIndex = 20;
      this.textBoxSerialNumber.Text = "%";
      this.textBoxSerialNumber.KeyPress += new KeyPressEventHandler(this.textBoxSerialNumber_KeyPress);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(5, 233);
      this.label3.Name = "label3";
      this.label3.Size = new Size(71, 13);
      this.label3.TabIndex = 19;
      this.label3.Text = "Serial number";
      this.buttonSearchSerialNumber.Location = new Point(189, 228);
      this.buttonSearchSerialNumber.Name = "buttonSearchSerialNumber";
      this.buttonSearchSerialNumber.Size = new Size(75, 23);
      this.buttonSearchSerialNumber.TabIndex = 21;
      this.buttonSearchSerialNumber.Text = "Search";
      this.buttonSearchSerialNumber.UseVisualStyleBackColor = true;
      this.buttonSearchSerialNumber.Click += new System.EventHandler(this.buttonSearchSerialNumber_Click);
      this.label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(675, 614);
      this.label4.Name = "label4";
      this.label4.Size = new Size(43, 13);
      this.label4.TabIndex = 19;
      this.label4.Text = "MeterId";
      this.textBoxMeterId.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.textBoxMeterId.Location = new Point(752, 611);
      this.textBoxMeterId.Name = "textBoxMeterId";
      this.textBoxMeterId.Size = new Size(101, 20);
      this.textBoxMeterId.TabIndex = 20;
      this.textBoxMeterId.KeyPress += new KeyPressEventHandler(this.textBoxMeterId_KeyPress);
      this.buttonSearchMeterId.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonSearchMeterId.Location = new Point(859, 609);
      this.buttonSearchMeterId.Name = "buttonSearchMeterId";
      this.buttonSearchMeterId.Size = new Size(75, 23);
      this.buttonSearchMeterId.TabIndex = 21;
      this.buttonSearchMeterId.Text = "Search";
      this.buttonSearchMeterId.UseVisualStyleBackColor = true;
      this.buttonSearchMeterId.Click += new System.EventHandler(this.buttonSearchMeterId_Click);
      this.buttonOpenDevice.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonOpenDevice.Location = new Point(758, 650);
      this.buttonOpenDevice.Name = "buttonOpenDevice";
      this.buttonOpenDevice.Size = new Size(182, 23);
      this.buttonOpenDevice.TabIndex = 22;
      this.buttonOpenDevice.Text = "Open selected meter";
      this.buttonOpenDevice.UseVisualStyleBackColor = true;
      this.buttonOpenDevice.Click += new System.EventHandler(this.buttonOpenDevice_Click);
      this.textBoxStateInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
      this.textBoxStateInfo.Location = new Point(670, 76);
      this.textBoxStateInfo.Multiline = true;
      this.textBoxStateInfo.Name = "textBoxStateInfo";
      this.textBoxStateInfo.Size = new Size(270, 184);
      this.textBoxStateInfo.TabIndex = 23;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(559, 50);
      this.label5.Name = "label5";
      this.label5.Size = new Size(67, 13);
      this.label5.TabIndex = 19;
      this.label5.Text = "Data set info";
      this.groupBoxDateRange.Controls.Add((Control) this.checkBoxUseProductionDateRange);
      this.groupBoxDateRange.Controls.Add((Control) this.dateTimePickerProductionDateTo);
      this.groupBoxDateRange.Controls.Add((Control) this.dateTimePickerProductionDateFrom);
      this.groupBoxDateRange.Controls.Add((Control) this.label7);
      this.groupBoxDateRange.Controls.Add((Control) this.label6);
      this.groupBoxDateRange.Location = new Point(8, 20);
      this.groupBoxDateRange.Name = "groupBoxDateRange";
      this.groupBoxDateRange.Size = new Size(256, 99);
      this.groupBoxDateRange.TabIndex = 24;
      this.groupBoxDateRange.TabStop = false;
      this.groupBoxDateRange.Text = "Date range production date";
      this.checkBoxUseProductionDateRange.AutoSize = true;
      this.checkBoxUseProductionDateRange.Checked = true;
      this.checkBoxUseProductionDateRange.CheckState = CheckState.Checked;
      this.checkBoxUseProductionDateRange.Location = new Point(89, 72);
      this.checkBoxUseProductionDateRange.Name = "checkBoxUseProductionDateRange";
      this.checkBoxUseProductionDateRange.RightToLeft = RightToLeft.Yes;
      this.checkBoxUseProductionDateRange.Size = new Size(152, 17);
      this.checkBoxUseProductionDateRange.TabIndex = 2;
      this.checkBoxUseProductionDateRange.Text = "Use production date range";
      this.checkBoxUseProductionDateRange.UseVisualStyleBackColor = true;
      this.dateTimePickerProductionDateTo.Format = DateTimePickerFormat.Short;
      this.dateTimePickerProductionDateTo.Location = new Point(74, 46);
      this.dateTimePickerProductionDateTo.Name = "dateTimePickerProductionDateTo";
      this.dateTimePickerProductionDateTo.Size = new Size(167, 20);
      this.dateTimePickerProductionDateTo.TabIndex = 1;
      this.dateTimePickerProductionDateFrom.Format = DateTimePickerFormat.Short;
      this.dateTimePickerProductionDateFrom.Location = new Point(74, 20);
      this.dateTimePickerProductionDateFrom.Name = "dateTimePickerProductionDateFrom";
      this.dateTimePickerProductionDateFrom.Size = new Size(167, 20);
      this.dateTimePickerProductionDateFrom.TabIndex = 1;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(7, 46);
      this.label7.Name = "label7";
      this.label7.Size = new Size(16, 13);
      this.label7.TabIndex = 0;
      this.label7.Text = "to";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(7, 20);
      this.label6.Name = "label6";
      this.label6.Size = new Size(27, 13);
      this.label6.TabIndex = 0;
      this.label6.Text = "from";
      this.groupBoxDateRangeApprovalDate.Controls.Add((Control) this.checkBoxUseApprovalDateRange);
      this.groupBoxDateRangeApprovalDate.Controls.Add((Control) this.dateTimePickerApprovalDateTo);
      this.groupBoxDateRangeApprovalDate.Controls.Add((Control) this.dateTimePickerApprovalDateFrom);
      this.groupBoxDateRangeApprovalDate.Controls.Add((Control) this.label8);
      this.groupBoxDateRangeApprovalDate.Controls.Add((Control) this.label9);
      this.groupBoxDateRangeApprovalDate.Location = new Point(8, 123);
      this.groupBoxDateRangeApprovalDate.Name = "groupBoxDateRangeApprovalDate";
      this.groupBoxDateRangeApprovalDate.Size = new Size(256, 99);
      this.groupBoxDateRangeApprovalDate.TabIndex = 24;
      this.groupBoxDateRangeApprovalDate.TabStop = false;
      this.groupBoxDateRangeApprovalDate.Text = "Date range approval date";
      this.checkBoxUseApprovalDateRange.AutoSize = true;
      this.checkBoxUseApprovalDateRange.Location = new Point(98, 72);
      this.checkBoxUseApprovalDateRange.Name = "checkBoxUseApprovalDateRange";
      this.checkBoxUseApprovalDateRange.RightToLeft = RightToLeft.Yes;
      this.checkBoxUseApprovalDateRange.Size = new Size(143, 17);
      this.checkBoxUseApprovalDateRange.TabIndex = 2;
      this.checkBoxUseApprovalDateRange.Text = "Use approval date range";
      this.checkBoxUseApprovalDateRange.UseVisualStyleBackColor = true;
      this.dateTimePickerApprovalDateTo.Format = DateTimePickerFormat.Short;
      this.dateTimePickerApprovalDateTo.Location = new Point(74, 46);
      this.dateTimePickerApprovalDateTo.Name = "dateTimePickerApprovalDateTo";
      this.dateTimePickerApprovalDateTo.Size = new Size(167, 20);
      this.dateTimePickerApprovalDateTo.TabIndex = 1;
      this.dateTimePickerApprovalDateFrom.Format = DateTimePickerFormat.Short;
      this.dateTimePickerApprovalDateFrom.Location = new Point(74, 20);
      this.dateTimePickerApprovalDateFrom.Name = "dateTimePickerApprovalDateFrom";
      this.dateTimePickerApprovalDateFrom.Size = new Size(167, 20);
      this.dateTimePickerApprovalDateFrom.TabIndex = 1;
      this.label8.AutoSize = true;
      this.label8.Location = new Point(7, 46);
      this.label8.Name = "label8";
      this.label8.Size = new Size(16, 13);
      this.label8.TabIndex = 0;
      this.label8.Text = "to";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(7, 20);
      this.label9.Name = "label9";
      this.label9.Size = new Size(27, 13);
      this.label9.TabIndex = 0;
      this.label9.Text = "from";
      this.groupBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.groupBox1.Controls.Add((Control) this.groupBoxDateRange);
      this.groupBox1.Controls.Add((Control) this.groupBoxDateRangeApprovalDate);
      this.groupBox1.Controls.Add((Control) this.buttonSearchHardwareTypeId);
      this.groupBox1.Controls.Add((Control) this.buttonSearchSapNumber);
      this.groupBox1.Controls.Add((Control) this.label11);
      this.groupBox1.Controls.Add((Control) this.buttonSearchSerialNumber);
      this.groupBox1.Controls.Add((Control) this.label10);
      this.groupBox1.Controls.Add((Control) this.textBoxHardwareTypeId);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.textBoxSapNumber);
      this.groupBox1.Controls.Add((Control) this.textBoxSerialNumber);
      this.groupBox1.Location = new Point(670, 285);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(270, 318);
      this.groupBox1.TabIndex = 25;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Time controlled";
      this.buttonSearchHardwareTypeId.Location = new Point(189, 280);
      this.buttonSearchHardwareTypeId.Name = "buttonSearchHardwareTypeId";
      this.buttonSearchHardwareTypeId.Size = new Size(75, 23);
      this.buttonSearchHardwareTypeId.TabIndex = 21;
      this.buttonSearchHardwareTypeId.Text = "Search";
      this.buttonSearchHardwareTypeId.UseVisualStyleBackColor = true;
      this.buttonSearchHardwareTypeId.Click += new System.EventHandler(this.buttonSearchHardwareTypeId_Click);
      this.buttonSearchSapNumber.Location = new Point(189, 254);
      this.buttonSearchSapNumber.Name = "buttonSearchSapNumber";
      this.buttonSearchSapNumber.Size = new Size(75, 23);
      this.buttonSearchSapNumber.TabIndex = 21;
      this.buttonSearchSapNumber.Text = "Search";
      this.buttonSearchSapNumber.UseVisualStyleBackColor = true;
      this.buttonSearchSapNumber.Click += new System.EventHandler(this.buttonSearchSapNumber_Click);
      this.label11.AutoSize = true;
      this.label11.Location = new Point(5, 285);
      this.label11.Name = "label11";
      this.label11.Size = new Size(56, 13);
      this.label11.TabIndex = 19;
      this.label11.Text = "HwTypeId";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(5, 259);
      this.label10.Name = "label10";
      this.label10.Size = new Size(66, 13);
      this.label10.TabIndex = 19;
      this.label10.Text = "SAP number";
      this.textBoxHardwareTypeId.Location = new Point(82, 282);
      this.textBoxHardwareTypeId.Name = "textBoxHardwareTypeId";
      this.textBoxHardwareTypeId.Size = new Size(101, 20);
      this.textBoxHardwareTypeId.TabIndex = 20;
      this.textBoxHardwareTypeId.KeyPress += new KeyPressEventHandler(this.textBoxHardwareTypeId_KeyPress);
      this.textBoxSapNumber.Location = new Point(82, 256);
      this.textBoxSapNumber.Name = "textBoxSapNumber";
      this.textBoxSapNumber.Size = new Size(101, 20);
      this.textBoxSapNumber.TabIndex = 20;
      this.textBoxSapNumber.KeyPress += new KeyPressEventHandler(this.textBoxSapNumber_KeyPress);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(952, 691);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.textBoxStateInfo);
      this.Controls.Add((Control) this.buttonOpenDevice);
      this.Controls.Add((Control) this.buttonSearchMeterId);
      this.Controls.Add((Control) this.textBoxMeterId);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.dataGridViewMeterData);
      this.Controls.Add((Control) this.dataGridViewMeters);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Name = nameof (OpenDevice);
      this.Text = nameof (OpenDevice);
      this.Load += new System.EventHandler(this.OpenDevice_Load);
      ((ISupportInitialize) this.dataGridViewMeters).EndInit();
      ((ISupportInitialize) this.dataGridViewMeterData).EndInit();
      this.groupBoxDateRange.ResumeLayout(false);
      this.groupBoxDateRange.PerformLayout();
      this.groupBoxDateRangeApprovalDate.ResumeLayout(false);
      this.groupBoxDateRangeApprovalDate.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private enum SearchNumberType
    {
      SerialNumber,
      SapNumber,
      HardwareTypeId,
      MeterId,
    }
  }
}

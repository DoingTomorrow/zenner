// Decompiled with JetBrains decompiler
// Type: PDC_Handler.LoggerEditor
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using CorporateDesign;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace PDC_Handler
{
  public class LoggerEditor : Form
  {
    private PDC_HandlerFunctions MyFunctions;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    internal Button btnPrint;
    private Button btnCancel;
    private DataGridView tableRamDataLogger;
    private Label label36;
    private ComboBox cboxHandlerObject;
    private TabControl tabs;
    private TabPage tabRamLogger;
    private TabPage tabFlashLogger;
    private TabPage tabEventLogger;
    private TabPage tabSystemLogger;
    private DataGridView tableFlashDataLogger;
    private TextBox txtLog_stichtag_address;
    private Label label4;
    private TextBox txtLog_fullmonth_address;
    private Label label5;
    private TextBox txtLog_halfmonth_address;
    private Label label6;
    private TextBox txtLOG_ADDRESS_STICHTAG_START;
    private Label label3;
    private TextBox txtLOG_ADDRESS_FULLMONTH_START;
    private Label label2;
    private TextBox txtLOG_ADDRESS_HALFMONTH_START;
    private Label label1;
    private TextBox txtHwSystemDate;
    private Label label11;
    private TextBox txtLogDate;
    private Label label7;
    private GroupBox gboxRamHeader;
    private TextBox txtRamHeaderCRC;
    private Label label18;
    private DateTimePicker txtRamHeaderLastDate;
    private Label label17;
    private ComboBox cboxRamHeaderFlags;
    private Label label16;
    private TextBox txtRamHeaderFifoEnd;
    private Label label15;
    private TextBox txtRamHeaderLength;
    private Label label14;
    private TextBox txtRamHeaderMaxLength;
    private Label label13;
    private TextBox txtRamHeaderAddress;
    private Label label12;
    private DataGridView tableRamDataLoggerValues;
    private ComboBox cboxRamLoggerType;
    private Label label19;
    private TabPage tabValues;
    private DataGridView tableValuesA;
    private DataGridView tableValuesB;
    private Label label21;
    private Label label20;

    private LoggerEditor() => this.InitializeComponent();

    internal static void ShowDialog(Form owner, PDC_HandlerFunctions MyFunctions)
    {
      if (MyFunctions == null)
        return;
      using (LoggerEditor loggerEditor = new LoggerEditor())
      {
        loggerEditor.MyFunctions = MyFunctions;
        int num = (int) loggerEditor.ShowDialog((IWin32Window) owner);
      }
    }

    private void LoggerEditor_Load(object sender, EventArgs e)
    {
      this.cboxHandlerObject.DataSource = (object) ZR_ClassLibrary.Util.GetNamesOfEnum(typeof (HandlerMeterType));
      this.cboxHandlerObject.SelectedItem = (object) HandlerMeterType.WorkMeter;
    }

    private void tabs_Selected(object sender, TabControlEventArgs e)
    {
      PDC_Meter handlerMeter = this.GetHandlerMeter();
      if (handlerMeter == null)
      {
        this.txtLOG_ADDRESS_HALFMONTH_START.Text = string.Empty;
        this.txtLOG_ADDRESS_FULLMONTH_START.Text = string.Empty;
        this.txtLOG_ADDRESS_STICHTAG_START.Text = string.Empty;
        this.txtLog_fullmonth_address.Text = string.Empty;
        this.txtLog_halfmonth_address.Text = string.Empty;
        this.txtLog_stichtag_address.Text = string.Empty;
        this.txtHwSystemDate.Text = string.Empty;
        this.txtLogDate.Text = string.Empty;
        this.tableFlashDataLogger.DataSource = (object) null;
      }
      else
      {
        if (!LoggerManager.IsLoggerDataAvailable(handlerMeter))
          return;
        this.txtHwSystemDate.Text = handlerMeter.GetSystemTime().ToString();
        if (e.TabPage == this.tabValues && e.Action == TabControlAction.Selected)
        {
          this.tableValuesA.DataSource = (object) ValueIdent.ToDataTable(this.MyFunctions.GetValues(1));
          this.tableValuesB.DataSource = (object) ValueIdent.ToDataTable(this.MyFunctions.GetValues(2));
        }
        else if (e.TabPage == this.tabFlashLogger && e.Action == TabControlAction.Selected)
        {
          DataTable flashLoggerTable = LoggerManager.CreateFlashLoggerTable(handlerMeter.Map.MemoryBytes);
          if (flashLoggerTable == null)
            return;
          ushort uint16_1 = BitConverter.ToUInt16(handlerMeter.Map.GetMemoryBytes(handlerMeter.GetParameter("log_halfmonth_address")), 0);
          ushort uint16_2 = BitConverter.ToUInt16(handlerMeter.Map.GetMemoryBytes(handlerMeter.GetParameter("log_fullmonth_address")), 0);
          ushort uint16_3 = BitConverter.ToUInt16(handlerMeter.Map.GetMemoryBytes(handlerMeter.GetParameter("log_stichtag_address")), 0);
          this.txtLOG_ADDRESS_HALFMONTH_START.Text = string.Format("0x{0:X4}", (object) (ushort) 32768);
          this.txtLOG_ADDRESS_FULLMONTH_START.Text = string.Format("0x{0:X4}", (object) (ushort) 35072);
          this.txtLOG_ADDRESS_STICHTAG_START.Text = string.Format("0x{0:X4}", (object) (ushort) 37376);
          this.txtLog_fullmonth_address.Text = string.Format("0x{0:X4}", (object) uint16_2);
          this.txtLog_halfmonth_address.Text = string.Format("0x{0:X4}", (object) uint16_1);
          this.txtLog_stichtag_address.Text = string.Format("0x{0:X4}", (object) uint16_3);
          this.txtLogDate.Text = handlerMeter.GetParameterValue<DateTime>("logDate").ToString();
          this.tableFlashDataLogger.DataSource = (object) flashLoggerTable;
        }
        else
        {
          if (e.TabPage != this.tabRamLogger || e.Action != TabControlAction.Selected)
            return;
          List<RamLogger> ramLogger = LoggerManager.ParseRamLogger(handlerMeter);
          if (ramLogger == null)
            return;
          this.cboxRamLoggerType.Tag = (object) ramLogger;
          this.cboxRamLoggerType.DataSource = (object) ZR_ClassLibrary.Util.GetNamesOfEnum(typeof (RamLoggerType));
          this.cboxRamLoggerType.SelectedItem = (object) RamLoggerType.QuarterHour.ToString();
          this.cboxRamHeaderFlags.DataSource = (object) ZR_ClassLibrary.Util.GetNamesOfEnum(typeof (RamLoggerFifoType));
        }
      }
    }

    private void cboxRamLoggerType_SelectedIndexChanged(object sender, EventArgs e)
    {
      RamLoggerType ramLoggerType = (RamLoggerType) Enum.Parse(typeof (RamLoggerType), this.cboxRamLoggerType.SelectedItem.ToString());
      this.gboxRamHeader.Visible = ramLoggerType != 0;
      this.tableRamDataLogger.Visible = ramLoggerType != 0;
      this.tableRamDataLoggerValues.Visible = ramLoggerType != 0;
      if (!(this.cboxRamLoggerType.Tag is List<RamLogger> tag))
        return;
      RamLogger ramLogger = tag.Find((Predicate<RamLogger>) (x => x.Type == ramLoggerType));
      if (ramLogger == null)
        return;
      try
      {
        this.txtRamHeaderAddress.Text = ramLogger.Header.Address.ToString("X4");
        this.txtRamHeaderFifoEnd.Text = ramLogger.Header.FifoEnd.ToString();
        TextBox ramHeaderMaxLength = this.txtRamHeaderMaxLength;
        byte num = ramLogger.Header.MaxLength;
        string str1 = num.ToString();
        ramHeaderMaxLength.Text = str1;
        TextBox txtRamHeaderLength = this.txtRamHeaderLength;
        num = ramLogger.Header.Length;
        string str2 = num.ToString();
        txtRamHeaderLength.Text = str2;
        this.cboxRamHeaderFlags.SelectedItem = (object) ramLogger.Header.Flags.ToString();
        this.txtRamHeaderLastDate.Value = ramLogger.Header.LastDate.HasValue ? ramLogger.Header.LastDate.Value : new DateTime(2000, 1, 1);
        this.txtRamHeaderCRC.Text = ramLogger.Header.CRC.ToString("X4");
        this.tableRamDataLogger.DataSource = (object) ramLogger.CreateTableRawData();
        this.tableRamDataLoggerValues.DataSource = (object) ramLogger.CreateTableValues();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, ex.Message, "RAM Header error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void cboxHandlerObject_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.tabs_Selected(sender, new TabControlEventArgs(this.tabs.SelectedTab, this.tabs.SelectedIndex, TabControlAction.Selected));
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
      PDC_Meter handlerMeter = this.GetHandlerMeter();
      if (handlerMeter == null)
        return;
      string str = handlerMeter.GetSerialnumberFull();
      if (string.IsNullOrEmpty(str))
        str = "???";
      if (this.tabs.SelectedTab != this.tabFlashLogger)
        return;
      PrintDataGridView.Print(this.tableFlashDataLogger, str + " FLASH DATA LOGGER");
    }

    private PDC_Meter GetHandlerMeter()
    {
      PDC_Meter handlerMeter;
      switch ((HandlerMeterType) Enum.Parse(typeof (HandlerMeterType), this.cboxHandlerObject.SelectedItem.ToString()))
      {
        case HandlerMeterType.WorkMeter:
          handlerMeter = this.MyFunctions.WorkMeter;
          break;
        case HandlerMeterType.TypeMeter:
          handlerMeter = this.MyFunctions.TypeMeter;
          break;
        case HandlerMeterType.BackupMeter:
          handlerMeter = this.MyFunctions.BackupMeter;
          break;
        case HandlerMeterType.ConnectedMeter:
          handlerMeter = this.MyFunctions.ConnectedMeter;
          break;
        default:
          throw new NotImplementedException();
      }
      return handlerMeter;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoggerEditor));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle6 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle7 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle8 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle9 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle10 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle11 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle12 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle13 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle14 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle15 = new DataGridViewCellStyle();
      this.btnPrint = new Button();
      this.btnCancel = new Button();
      this.tableRamDataLogger = new DataGridView();
      this.label36 = new Label();
      this.cboxHandlerObject = new ComboBox();
      this.tabs = new TabControl();
      this.tabValues = new TabPage();
      this.label21 = new Label();
      this.label20 = new Label();
      this.tableValuesB = new DataGridView();
      this.tableValuesA = new DataGridView();
      this.tabFlashLogger = new TabPage();
      this.txtLogDate = new TextBox();
      this.label7 = new Label();
      this.txtLog_stichtag_address = new TextBox();
      this.label4 = new Label();
      this.txtLog_fullmonth_address = new TextBox();
      this.label5 = new Label();
      this.txtLog_halfmonth_address = new TextBox();
      this.label6 = new Label();
      this.txtLOG_ADDRESS_STICHTAG_START = new TextBox();
      this.label3 = new Label();
      this.txtLOG_ADDRESS_FULLMONTH_START = new TextBox();
      this.label2 = new Label();
      this.txtLOG_ADDRESS_HALFMONTH_START = new TextBox();
      this.label1 = new Label();
      this.tableFlashDataLogger = new DataGridView();
      this.tabRamLogger = new TabPage();
      this.tableRamDataLoggerValues = new DataGridView();
      this.cboxRamLoggerType = new ComboBox();
      this.label19 = new Label();
      this.gboxRamHeader = new GroupBox();
      this.txtRamHeaderCRC = new TextBox();
      this.label18 = new Label();
      this.txtRamHeaderLastDate = new DateTimePicker();
      this.label17 = new Label();
      this.cboxRamHeaderFlags = new ComboBox();
      this.label16 = new Label();
      this.txtRamHeaderFifoEnd = new TextBox();
      this.label15 = new Label();
      this.txtRamHeaderLength = new TextBox();
      this.label14 = new Label();
      this.txtRamHeaderMaxLength = new TextBox();
      this.label13 = new Label();
      this.txtRamHeaderAddress = new TextBox();
      this.label12 = new Label();
      this.tabEventLogger = new TabPage();
      this.tabSystemLogger = new TabPage();
      this.txtHwSystemDate = new TextBox();
      this.label11 = new Label();
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      ((ISupportInitialize) this.tableRamDataLogger).BeginInit();
      this.tabs.SuspendLayout();
      this.tabValues.SuspendLayout();
      ((ISupportInitialize) this.tableValuesB).BeginInit();
      ((ISupportInitialize) this.tableValuesA).BeginInit();
      this.tabFlashLogger.SuspendLayout();
      ((ISupportInitialize) this.tableFlashDataLogger).BeginInit();
      this.tabRamLogger.SuspendLayout();
      ((ISupportInitialize) this.tableRamDataLoggerValues).BeginInit();
      this.gboxRamHeader.SuspendLayout();
      this.SuspendLayout();
      this.btnPrint.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnPrint.Image = (Image) componentResourceManager.GetObject("btnPrint.Image");
      this.btnPrint.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnPrint.Location = new Point(10, 521);
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(66, 29);
      this.btnPrint.TabIndex = 22;
      this.btnPrint.Text = "Print";
      this.btnPrint.TextAlign = ContentAlignment.MiddleRight;
      this.btnPrint.UseVisualStyleBackColor = true;
      this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Image = (Image) componentResourceManager.GetObject("btnCancel.Image");
      this.btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCancel.ImeMode = ImeMode.NoControl;
      this.btnCancel.Location = new Point(695, 521);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(77, 29);
      this.btnCancel.TabIndex = 21;
      this.btnCancel.Text = "Close";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      this.tableRamDataLogger.AllowUserToAddRows = false;
      this.tableRamDataLogger.AllowUserToDeleteRows = false;
      this.tableRamDataLogger.AllowUserToResizeRows = false;
      gridViewCellStyle1.BackColor = Color.FromArgb(228, 241, 244);
      this.tableRamDataLogger.AlternatingRowsDefaultCellStyle = gridViewCellStyle1;
      this.tableRamDataLogger.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.tableRamDataLogger.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.tableRamDataLogger.BackgroundColor = Color.White;
      this.tableRamDataLogger.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.True;
      this.tableRamDataLogger.DefaultCellStyle = gridViewCellStyle2;
      this.tableRamDataLogger.Location = new Point(212, 10);
      this.tableRamDataLogger.Name = "tableRamDataLogger";
      this.tableRamDataLogger.RowHeadersVisible = false;
      this.tableRamDataLogger.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
      gridViewCellStyle3.WrapMode = DataGridViewTriState.True;
      this.tableRamDataLogger.RowsDefaultCellStyle = gridViewCellStyle3;
      this.tableRamDataLogger.RowTemplate.Resizable = DataGridViewTriState.True;
      this.tableRamDataLogger.Size = new Size(291, 438);
      this.tableRamDataLogger.TabIndex = 23;
      this.label36.BackColor = Color.White;
      this.label36.Location = new Point(173, 7);
      this.label36.Name = "label36";
      this.label36.Size = new Size(84, 15);
      this.label36.TabIndex = 53;
      this.label36.Text = "Handler object:";
      this.label36.TextAlign = ContentAlignment.MiddleRight;
      this.cboxHandlerObject.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxHandlerObject.FormattingEnabled = true;
      this.cboxHandlerObject.Location = new Point(265, 5);
      this.cboxHandlerObject.Name = "cboxHandlerObject";
      this.cboxHandlerObject.Size = new Size(132, 21);
      this.cboxHandlerObject.TabIndex = 52;
      this.cboxHandlerObject.SelectedIndexChanged += new System.EventHandler(this.cboxHandlerObject_SelectedIndexChanged);
      this.tabs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tabs.Controls.Add((Control) this.tabValues);
      this.tabs.Controls.Add((Control) this.tabFlashLogger);
      this.tabs.Controls.Add((Control) this.tabRamLogger);
      this.tabs.Controls.Add((Control) this.tabEventLogger);
      this.tabs.Controls.Add((Control) this.tabSystemLogger);
      this.tabs.Location = new Point(2, 38);
      this.tabs.Name = "tabs";
      this.tabs.SelectedIndex = 0;
      this.tabs.Size = new Size(782, 477);
      this.tabs.TabIndex = 54;
      this.tabs.Selected += new TabControlEventHandler(this.tabs_Selected);
      this.tabValues.Controls.Add((Control) this.label21);
      this.tabValues.Controls.Add((Control) this.label20);
      this.tabValues.Controls.Add((Control) this.tableValuesB);
      this.tabValues.Controls.Add((Control) this.tableValuesA);
      this.tabValues.Location = new Point(4, 22);
      this.tabValues.Name = "tabValues";
      this.tabValues.Size = new Size(774, 451);
      this.tabValues.TabIndex = 4;
      this.tabValues.Text = "Values";
      this.tabValues.UseVisualStyleBackColor = true;
      this.label21.AutoSize = true;
      this.label21.Location = new Point(394, 5);
      this.label21.Name = "label21";
      this.label21.Size = new Size(41, 13);
      this.label21.TabIndex = 28;
      this.label21.Text = "Input B";
      this.label20.AutoSize = true;
      this.label20.Location = new Point(8, 5);
      this.label20.Name = "label20";
      this.label20.Size = new Size(41, 13);
      this.label20.TabIndex = 27;
      this.label20.Text = "Input A";
      this.tableValuesB.AllowUserToAddRows = false;
      this.tableValuesB.AllowUserToDeleteRows = false;
      this.tableValuesB.AllowUserToResizeRows = false;
      gridViewCellStyle4.BackColor = Color.FromArgb(228, 241, 244);
      this.tableValuesB.AlternatingRowsDefaultCellStyle = gridViewCellStyle4;
      this.tableValuesB.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tableValuesB.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.tableValuesB.BackgroundColor = Color.White;
      this.tableValuesB.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      gridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle5.BackColor = SystemColors.Window;
      gridViewCellStyle5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle5.ForeColor = SystemColors.ControlText;
      gridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle5.WrapMode = DataGridViewTriState.True;
      this.tableValuesB.DefaultCellStyle = gridViewCellStyle5;
      this.tableValuesB.Location = new Point(397, 23);
      this.tableValuesB.Name = "tableValuesB";
      this.tableValuesB.RowHeadersVisible = false;
      this.tableValuesB.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
      gridViewCellStyle6.WrapMode = DataGridViewTriState.True;
      this.tableValuesB.RowsDefaultCellStyle = gridViewCellStyle6;
      this.tableValuesB.RowTemplate.Resizable = DataGridViewTriState.True;
      this.tableValuesB.Size = new Size(374, 425);
      this.tableValuesB.TabIndex = 26;
      this.tableValuesA.AllowUserToAddRows = false;
      this.tableValuesA.AllowUserToDeleteRows = false;
      this.tableValuesA.AllowUserToResizeRows = false;
      gridViewCellStyle7.BackColor = Color.FromArgb(228, 241, 244);
      this.tableValuesA.AlternatingRowsDefaultCellStyle = gridViewCellStyle7;
      this.tableValuesA.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.tableValuesA.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.tableValuesA.BackgroundColor = Color.White;
      this.tableValuesA.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      gridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle8.BackColor = SystemColors.Window;
      gridViewCellStyle8.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle8.ForeColor = SystemColors.ControlText;
      gridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle8.WrapMode = DataGridViewTriState.True;
      this.tableValuesA.DefaultCellStyle = gridViewCellStyle8;
      this.tableValuesA.Location = new Point(3, 23);
      this.tableValuesA.Name = "tableValuesA";
      this.tableValuesA.RowHeadersVisible = false;
      this.tableValuesA.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
      gridViewCellStyle9.WrapMode = DataGridViewTriState.True;
      this.tableValuesA.RowsDefaultCellStyle = gridViewCellStyle9;
      this.tableValuesA.RowTemplate.Resizable = DataGridViewTriState.True;
      this.tableValuesA.Size = new Size(388, 425);
      this.tableValuesA.TabIndex = 25;
      this.tabFlashLogger.Controls.Add((Control) this.txtLogDate);
      this.tabFlashLogger.Controls.Add((Control) this.label7);
      this.tabFlashLogger.Controls.Add((Control) this.txtLog_stichtag_address);
      this.tabFlashLogger.Controls.Add((Control) this.label4);
      this.tabFlashLogger.Controls.Add((Control) this.txtLog_fullmonth_address);
      this.tabFlashLogger.Controls.Add((Control) this.label5);
      this.tabFlashLogger.Controls.Add((Control) this.txtLog_halfmonth_address);
      this.tabFlashLogger.Controls.Add((Control) this.label6);
      this.tabFlashLogger.Controls.Add((Control) this.txtLOG_ADDRESS_STICHTAG_START);
      this.tabFlashLogger.Controls.Add((Control) this.label3);
      this.tabFlashLogger.Controls.Add((Control) this.txtLOG_ADDRESS_FULLMONTH_START);
      this.tabFlashLogger.Controls.Add((Control) this.label2);
      this.tabFlashLogger.Controls.Add((Control) this.txtLOG_ADDRESS_HALFMONTH_START);
      this.tabFlashLogger.Controls.Add((Control) this.label1);
      this.tabFlashLogger.Controls.Add((Control) this.tableFlashDataLogger);
      this.tabFlashLogger.Location = new Point(4, 22);
      this.tabFlashLogger.Name = "tabFlashLogger";
      this.tabFlashLogger.Padding = new Padding(3);
      this.tabFlashLogger.Size = new Size(774, 451);
      this.tabFlashLogger.TabIndex = 1;
      this.tabFlashLogger.Text = "FLASH Data Logger";
      this.tabFlashLogger.UseVisualStyleBackColor = true;
      this.txtLogDate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.txtLogDate.Location = new Point(657, 379);
      this.txtLogDate.Name = "txtLogDate";
      this.txtLogDate.ReadOnly = true;
      this.txtLogDate.Size = new Size(109, 20);
      this.txtLogDate.TabIndex = 48;
      this.label7.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label7.Location = new Point(570, 382);
      this.label7.Name = "label7";
      this.label7.Size = new Size(83, 17);
      this.label7.TabIndex = 47;
      this.label7.Text = "logDate";
      this.label7.TextAlign = ContentAlignment.TopRight;
      this.txtLog_stichtag_address.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.txtLog_stichtag_address.Location = new Point(399, 422);
      this.txtLog_stichtag_address.Name = "txtLog_stichtag_address";
      this.txtLog_stichtag_address.ReadOnly = true;
      this.txtLog_stichtag_address.Size = new Size(56, 20);
      this.txtLog_stichtag_address.TabIndex = 36;
      this.label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label4.Location = new Point(272, 425);
      this.label4.Name = "label4";
      this.label4.Size = new Size(128, 17);
      this.label4.TabIndex = 35;
      this.label4.Text = "log_stichtag_address";
      this.label4.TextAlign = ContentAlignment.TopRight;
      this.txtLog_fullmonth_address.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.txtLog_fullmonth_address.Location = new Point(399, 400);
      this.txtLog_fullmonth_address.Name = "txtLog_fullmonth_address";
      this.txtLog_fullmonth_address.ReadOnly = true;
      this.txtLog_fullmonth_address.Size = new Size(56, 20);
      this.txtLog_fullmonth_address.TabIndex = 34;
      this.label5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label5.Location = new Point(272, 403);
      this.label5.Name = "label5";
      this.label5.Size = new Size(128, 17);
      this.label5.TabIndex = 33;
      this.label5.Text = "log_fullmonth_address";
      this.label5.TextAlign = ContentAlignment.TopRight;
      this.txtLog_halfmonth_address.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.txtLog_halfmonth_address.Location = new Point(399, 379);
      this.txtLog_halfmonth_address.Name = "txtLog_halfmonth_address";
      this.txtLog_halfmonth_address.ReadOnly = true;
      this.txtLog_halfmonth_address.Size = new Size(56, 20);
      this.txtLog_halfmonth_address.TabIndex = 32;
      this.label6.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label6.Location = new Point(272, 382);
      this.label6.Name = "label6";
      this.label6.Size = new Size(128, 17);
      this.label6.TabIndex = 31;
      this.label6.Text = "log_halfmonth_address";
      this.label6.TextAlign = ContentAlignment.TopRight;
      this.txtLOG_ADDRESS_STICHTAG_START.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.txtLOG_ADDRESS_STICHTAG_START.Location = new Point(215, 421);
      this.txtLOG_ADDRESS_STICHTAG_START.Name = "txtLOG_ADDRESS_STICHTAG_START";
      this.txtLOG_ADDRESS_STICHTAG_START.ReadOnly = true;
      this.txtLOG_ADDRESS_STICHTAG_START.Size = new Size(56, 20);
      this.txtLOG_ADDRESS_STICHTAG_START.TabIndex = 30;
      this.label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label3.Location = new Point(2, 424);
      this.label3.Name = "label3";
      this.label3.Size = new Size(210, 17);
      this.label3.TabIndex = 29;
      this.label3.Text = "LOG_ADDRESS_STICHTAG_START";
      this.label3.TextAlign = ContentAlignment.TopRight;
      this.txtLOG_ADDRESS_FULLMONTH_START.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.txtLOG_ADDRESS_FULLMONTH_START.Location = new Point(215, 400);
      this.txtLOG_ADDRESS_FULLMONTH_START.Name = "txtLOG_ADDRESS_FULLMONTH_START";
      this.txtLOG_ADDRESS_FULLMONTH_START.ReadOnly = true;
      this.txtLOG_ADDRESS_FULLMONTH_START.Size = new Size(56, 20);
      this.txtLOG_ADDRESS_FULLMONTH_START.TabIndex = 28;
      this.label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label2.Location = new Point(2, 403);
      this.label2.Name = "label2";
      this.label2.Size = new Size(210, 17);
      this.label2.TabIndex = 27;
      this.label2.Text = "LOG_ADDRESS_FULLMONTH_START";
      this.label2.TextAlign = ContentAlignment.TopRight;
      this.txtLOG_ADDRESS_HALFMONTH_START.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.txtLOG_ADDRESS_HALFMONTH_START.Location = new Point(215, 379);
      this.txtLOG_ADDRESS_HALFMONTH_START.Name = "txtLOG_ADDRESS_HALFMONTH_START";
      this.txtLOG_ADDRESS_HALFMONTH_START.ReadOnly = true;
      this.txtLOG_ADDRESS_HALFMONTH_START.Size = new Size(56, 20);
      this.txtLOG_ADDRESS_HALFMONTH_START.TabIndex = 26;
      this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label1.Location = new Point(2, 382);
      this.label1.Name = "label1";
      this.label1.Size = new Size(210, 17);
      this.label1.TabIndex = 25;
      this.label1.Text = "LOG_ADDRESS_HALFMONTH_START";
      this.label1.TextAlign = ContentAlignment.TopRight;
      this.tableFlashDataLogger.AllowUserToAddRows = false;
      this.tableFlashDataLogger.AllowUserToDeleteRows = false;
      this.tableFlashDataLogger.AllowUserToResizeRows = false;
      gridViewCellStyle10.BackColor = Color.FromArgb(228, 241, 244);
      this.tableFlashDataLogger.AlternatingRowsDefaultCellStyle = gridViewCellStyle10;
      this.tableFlashDataLogger.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tableFlashDataLogger.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.tableFlashDataLogger.BackgroundColor = Color.White;
      this.tableFlashDataLogger.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      gridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle11.BackColor = SystemColors.Window;
      gridViewCellStyle11.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle11.ForeColor = SystemColors.ControlText;
      gridViewCellStyle11.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle11.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle11.WrapMode = DataGridViewTriState.True;
      this.tableFlashDataLogger.DefaultCellStyle = gridViewCellStyle11;
      this.tableFlashDataLogger.Location = new Point(7, 6);
      this.tableFlashDataLogger.Name = "tableFlashDataLogger";
      this.tableFlashDataLogger.RowHeadersVisible = false;
      this.tableFlashDataLogger.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
      gridViewCellStyle12.WrapMode = DataGridViewTriState.True;
      this.tableFlashDataLogger.RowsDefaultCellStyle = gridViewCellStyle12;
      this.tableFlashDataLogger.RowTemplate.Resizable = DataGridViewTriState.True;
      this.tableFlashDataLogger.Size = new Size(761, 367);
      this.tableFlashDataLogger.TabIndex = 24;
      this.tabRamLogger.Controls.Add((Control) this.tableRamDataLoggerValues);
      this.tabRamLogger.Controls.Add((Control) this.cboxRamLoggerType);
      this.tabRamLogger.Controls.Add((Control) this.label19);
      this.tabRamLogger.Controls.Add((Control) this.gboxRamHeader);
      this.tabRamLogger.Controls.Add((Control) this.tableRamDataLogger);
      this.tabRamLogger.Location = new Point(4, 22);
      this.tabRamLogger.Name = "tabRamLogger";
      this.tabRamLogger.Padding = new Padding(3);
      this.tabRamLogger.Size = new Size(774, 451);
      this.tabRamLogger.TabIndex = 0;
      this.tabRamLogger.Text = "RAM Data Logger";
      this.tabRamLogger.UseVisualStyleBackColor = true;
      this.tableRamDataLoggerValues.AllowUserToAddRows = false;
      this.tableRamDataLoggerValues.AllowUserToDeleteRows = false;
      this.tableRamDataLoggerValues.AllowUserToResizeRows = false;
      gridViewCellStyle13.BackColor = Color.FromArgb(228, 241, 244);
      this.tableRamDataLoggerValues.AlternatingRowsDefaultCellStyle = gridViewCellStyle13;
      this.tableRamDataLoggerValues.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.tableRamDataLoggerValues.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.tableRamDataLoggerValues.BackgroundColor = Color.White;
      this.tableRamDataLoggerValues.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      gridViewCellStyle14.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle14.BackColor = SystemColors.Window;
      gridViewCellStyle14.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle14.ForeColor = SystemColors.ControlText;
      gridViewCellStyle14.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle14.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle14.WrapMode = DataGridViewTriState.True;
      this.tableRamDataLoggerValues.DefaultCellStyle = gridViewCellStyle14;
      this.tableRamDataLoggerValues.Location = new Point(509, 9);
      this.tableRamDataLoggerValues.Name = "tableRamDataLoggerValues";
      this.tableRamDataLoggerValues.RowHeadersVisible = false;
      this.tableRamDataLoggerValues.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
      gridViewCellStyle15.WrapMode = DataGridViewTriState.True;
      this.tableRamDataLoggerValues.RowsDefaultCellStyle = gridViewCellStyle15;
      this.tableRamDataLoggerValues.RowTemplate.Resizable = DataGridViewTriState.True;
      this.tableRamDataLoggerValues.Size = new Size(262, 439);
      this.tableRamDataLoggerValues.TabIndex = 36;
      this.cboxRamLoggerType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxRamLoggerType.FormattingEnabled = true;
      this.cboxRamLoggerType.Location = new Point(40, 10);
      this.cboxRamLoggerType.Name = "cboxRamLoggerType";
      this.cboxRamLoggerType.Size = new Size(160, 21);
      this.cboxRamLoggerType.TabIndex = 35;
      this.cboxRamLoggerType.SelectedIndexChanged += new System.EventHandler(this.cboxRamLoggerType_SelectedIndexChanged);
      this.label19.Location = new Point(4, 13);
      this.label19.Name = "label19";
      this.label19.Size = new Size(36, 15);
      this.label19.TabIndex = 34;
      this.label19.Text = "Type:";
      this.label19.TextAlign = ContentAlignment.TopRight;
      this.gboxRamHeader.Controls.Add((Control) this.txtRamHeaderCRC);
      this.gboxRamHeader.Controls.Add((Control) this.label18);
      this.gboxRamHeader.Controls.Add((Control) this.txtRamHeaderLastDate);
      this.gboxRamHeader.Controls.Add((Control) this.label17);
      this.gboxRamHeader.Controls.Add((Control) this.cboxRamHeaderFlags);
      this.gboxRamHeader.Controls.Add((Control) this.label16);
      this.gboxRamHeader.Controls.Add((Control) this.txtRamHeaderFifoEnd);
      this.gboxRamHeader.Controls.Add((Control) this.label15);
      this.gboxRamHeader.Controls.Add((Control) this.txtRamHeaderLength);
      this.gboxRamHeader.Controls.Add((Control) this.label14);
      this.gboxRamHeader.Controls.Add((Control) this.txtRamHeaderMaxLength);
      this.gboxRamHeader.Controls.Add((Control) this.label13);
      this.gboxRamHeader.Controls.Add((Control) this.txtRamHeaderAddress);
      this.gboxRamHeader.Controls.Add((Control) this.label12);
      this.gboxRamHeader.Location = new Point(4, 37);
      this.gboxRamHeader.Name = "gboxRamHeader";
      this.gboxRamHeader.Size = new Size(204, 176);
      this.gboxRamHeader.TabIndex = 25;
      this.gboxRamHeader.TabStop = false;
      this.gboxRamHeader.Text = "Header";
      this.txtRamHeaderCRC.Location = new Point(73, 145);
      this.txtRamHeaderCRC.Name = "txtRamHeaderCRC";
      this.txtRamHeaderCRC.Size = new Size(55, 20);
      this.txtRamHeaderCRC.TabIndex = 37;
      this.label18.Location = new Point(6, 148);
      this.label18.Name = "label18";
      this.label18.Size = new Size(65, 15);
      this.label18.TabIndex = 36;
      this.label18.Text = "CRC 0x:";
      this.label18.TextAlign = ContentAlignment.TopRight;
      this.txtRamHeaderLastDate.CustomFormat = "dd.MM.yyyy HH:mm:ss";
      this.txtRamHeaderLastDate.Format = DateTimePickerFormat.Custom;
      this.txtRamHeaderLastDate.Location = new Point(73, 123);
      this.txtRamHeaderLastDate.Name = "txtRamHeaderLastDate";
      this.txtRamHeaderLastDate.Size = new Size(122, 20);
      this.txtRamHeaderLastDate.TabIndex = 35;
      this.txtRamHeaderLastDate.Value = new DateTime(2014, 10, 14, 14, 52, 17, 0);
      this.label17.Location = new Point(6, 126);
      this.label17.Name = "label17";
      this.label17.Size = new Size(65, 15);
      this.label17.TabIndex = 34;
      this.label17.Text = "Last date:";
      this.label17.TextAlign = ContentAlignment.TopRight;
      this.cboxRamHeaderFlags.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxRamHeaderFlags.FormattingEnabled = true;
      this.cboxRamHeaderFlags.Location = new Point(73, 101);
      this.cboxRamHeaderFlags.Name = "cboxRamHeaderFlags";
      this.cboxRamHeaderFlags.Size = new Size(122, 21);
      this.cboxRamHeaderFlags.TabIndex = 33;
      this.label16.Location = new Point(6, 104);
      this.label16.Name = "label16";
      this.label16.Size = new Size(65, 15);
      this.label16.TabIndex = 32;
      this.label16.Text = "Flags:";
      this.label16.TextAlign = ContentAlignment.TopRight;
      this.txtRamHeaderFifoEnd.Location = new Point(73, 79);
      this.txtRamHeaderFifoEnd.Name = "txtRamHeaderFifoEnd";
      this.txtRamHeaderFifoEnd.Size = new Size(55, 20);
      this.txtRamHeaderFifoEnd.TabIndex = 31;
      this.label15.Location = new Point(6, 82);
      this.label15.Name = "label15";
      this.label15.Size = new Size(65, 15);
      this.label15.TabIndex = 30;
      this.label15.Text = "Fifo end:";
      this.label15.TextAlign = ContentAlignment.TopRight;
      this.txtRamHeaderLength.Location = new Point(73, 58);
      this.txtRamHeaderLength.Name = "txtRamHeaderLength";
      this.txtRamHeaderLength.Size = new Size(55, 20);
      this.txtRamHeaderLength.TabIndex = 29;
      this.label14.Location = new Point(6, 61);
      this.label14.Name = "label14";
      this.label14.Size = new Size(65, 15);
      this.label14.TabIndex = 28;
      this.label14.Text = "Length:";
      this.label14.TextAlign = ContentAlignment.TopRight;
      this.txtRamHeaderMaxLength.Location = new Point(73, 37);
      this.txtRamHeaderMaxLength.Name = "txtRamHeaderMaxLength";
      this.txtRamHeaderMaxLength.Size = new Size(55, 20);
      this.txtRamHeaderMaxLength.TabIndex = 27;
      this.label13.Location = new Point(6, 40);
      this.label13.Name = "label13";
      this.label13.Size = new Size(65, 15);
      this.label13.TabIndex = 26;
      this.label13.Text = "Max length:";
      this.label13.TextAlign = ContentAlignment.TopRight;
      this.txtRamHeaderAddress.Location = new Point(73, 16);
      this.txtRamHeaderAddress.Name = "txtRamHeaderAddress";
      this.txtRamHeaderAddress.Size = new Size(55, 20);
      this.txtRamHeaderAddress.TabIndex = 25;
      this.label12.Location = new Point(6, 19);
      this.label12.Name = "label12";
      this.label12.Size = new Size(65, 15);
      this.label12.TabIndex = 24;
      this.label12.Text = "Address: 0x";
      this.label12.TextAlign = ContentAlignment.TopRight;
      this.tabEventLogger.Location = new Point(4, 22);
      this.tabEventLogger.Name = "tabEventLogger";
      this.tabEventLogger.Padding = new Padding(3);
      this.tabEventLogger.Size = new Size(774, 451);
      this.tabEventLogger.TabIndex = 2;
      this.tabEventLogger.Text = "Event Logger";
      this.tabEventLogger.UseVisualStyleBackColor = true;
      this.tabSystemLogger.Location = new Point(4, 22);
      this.tabSystemLogger.Name = "tabSystemLogger";
      this.tabSystemLogger.Padding = new Padding(3);
      this.tabSystemLogger.Size = new Size(774, 451);
      this.tabSystemLogger.TabIndex = 3;
      this.tabSystemLogger.Text = "System Logger";
      this.tabSystemLogger.UseVisualStyleBackColor = true;
      this.txtHwSystemDate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.txtHwSystemDate.Location = new Point(209, 526);
      this.txtHwSystemDate.Name = "txtHwSystemDate";
      this.txtHwSystemDate.ReadOnly = true;
      this.txtHwSystemDate.Size = new Size(109, 20);
      this.txtHwSystemDate.TabIndex = 46;
      this.label11.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label11.Location = new Point(122, 529);
      this.label11.Name = "label11";
      this.label11.Size = new Size(83, 17);
      this.label11.TabIndex = 45;
      this.label11.Text = "hwSystemDate";
      this.label11.TextAlign = ContentAlignment.TopRight;
      this.zennerCoroprateDesign1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.zennerCoroprateDesign1.Location = new Point(0, 0);
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      this.zennerCoroprateDesign1.Size = new Size(784, 36);
      this.zennerCoroprateDesign1.TabIndex = 2;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(784, 562);
      this.Controls.Add((Control) this.tabs);
      this.Controls.Add((Control) this.label36);
      this.Controls.Add((Control) this.txtHwSystemDate);
      this.Controls.Add((Control) this.label11);
      this.Controls.Add((Control) this.cboxHandlerObject);
      this.Controls.Add((Control) this.btnPrint);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (LoggerEditor);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Logger Editor";
      this.Load += new System.EventHandler(this.LoggerEditor_Load);
      ((ISupportInitialize) this.tableRamDataLogger).EndInit();
      this.tabs.ResumeLayout(false);
      this.tabValues.ResumeLayout(false);
      this.tabValues.PerformLayout();
      ((ISupportInitialize) this.tableValuesB).EndInit();
      ((ISupportInitialize) this.tableValuesA).EndInit();
      this.tabFlashLogger.ResumeLayout(false);
      this.tabFlashLogger.PerformLayout();
      ((ISupportInitialize) this.tableFlashDataLogger).EndInit();
      this.tabRamLogger.ResumeLayout(false);
      ((ISupportInitialize) this.tableRamDataLoggerValues).EndInit();
      this.gboxRamHeader.ResumeLayout(false);
      this.gboxRamHeader.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

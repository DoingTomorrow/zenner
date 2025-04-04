// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.BackupEditor
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

using CorporateDesign;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace SmokeDetectorHandler
{
  public class BackupEditor : Form
  {
    private SmokeDetectorHandlerFunctions handler;
    private static BackupEditor dlgBackup;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private Button btnCancel;
    private Button btnLoad;
    private DataGridView tableMeter;
    private GroupBox groupBox1;
    private GroupBox groupBox2;
    private DataGridView tableMeterData;
    private GroupBox groupBox3;
    private TextBox txtInfo;
    private DataGridViewTextBoxColumn colMeterId;
    private DataGridViewTextBoxColumn colMeterInfoID;
    private DataGridViewTextBoxColumn colSerialnumber;
    private DataGridViewTextBoxColumn colProductionDate;
    private DataGridViewTextBoxColumn colApprovalDate;
    private DataGridViewTextBoxColumn colOrderNr;
    private DataGridViewTextBoxColumn colPos;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn colTimePoint;
    private DataGridViewTextBoxColumn colHardwareTypeID;
    private DataGridViewTextBoxColumn colData;
    private DataGridViewTextBoxColumn colSize;
    private Label label1;
    private GroupBox gboxFilter;
    private Label label2;
    private DateTimePicker txtProductionDateEnd;
    private DateTimePicker txtProductionDateStart;
    private Button btnReloadMeters;
    private Label lblInterval;
    private Button btnSearch;
    private ContextMenuStrip contextMenuMeter;
    private ToolStripMenuItem btnExportToCSV;
    private Label lblCountOfMeters;
    private GroupBox groupBox4;
    private Button btnStop;

    public BackupEditor()
    {
      this.InitializeComponent();
      this.gboxFilter.Enabled = true;
      this.txtProductionDateStart.Value = DateTime.Now.AddDays(-1.0);
      this.txtProductionDateEnd.Value = DateTime.Now;
    }

    internal static void ShowDialog(Form owner, SmokeDetectorHandlerFunctions handler)
    {
      if (handler == null)
        throw new ArgumentNullException(nameof (handler), "Can not show the dialog!");
      try
      {
        SmokeDetectorDatabase.OnMeterFound += new EventHandlerEx<Meter>(BackupEditor.SmokeDetectorDatabase_OnMeterFound);
        SmokeDetectorDatabase.OnDone += new System.EventHandler(BackupEditor.SmokeDetectorDatabase_OnDone);
        using (BackupEditor backupEditor = new BackupEditor())
        {
          backupEditor.handler = handler;
          BackupEditor.dlgBackup = backupEditor;
          int num = (int) backupEditor.ShowDialog((IWin32Window) owner);
        }
      }
      finally
      {
        SmokeDetectorDatabase.OnMeterFound -= new EventHandlerEx<Meter>(BackupEditor.SmokeDetectorDatabase_OnMeterFound);
        SmokeDetectorDatabase.OnDone -= new System.EventHandler(BackupEditor.SmokeDetectorDatabase_OnDone);
        BackupEditor.dlgBackup = (BackupEditor) null;
      }
    }

    private static void SmokeDetectorDatabase_OnDone(object sender, EventArgs e)
    {
      int num = (int) MessageBox.Show("Done!");
    }

    private void BackupEditor_Load(object sender, EventArgs e)
    {
    }

    private void txtProductionDate_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.btnReloadMeters_Click(sender, (EventArgs) e);
    }

    private void tableMeter_SelectionChanged(object sender, EventArgs e)
    {
      if (this.tableMeter.SelectedRows.Count != 1)
        return;
      List<MeterData> meterDataList = SmokeDetectorDatabase.LoadMeterData((int) Convert.ToUInt32(this.tableMeter.SelectedRows[0].Cells[this.colMeterId.Name].Value));
      if (meterDataList == null)
        return;
      this.tableMeterData.Rows.Clear();
      int num = 1;
      foreach (MeterData meterData in meterDataList)
        this.tableMeterData.Rows[this.tableMeterData.Rows.Add((object) num++, (object) meterData.MeterID, (object) meterData.TimePoint, (object) meterData.HardwareTypeID, (object) meterData.Buffer, (object) meterData.Buffer.Length)].Tag = (object) meterData;
      this.btnLoad.Enabled = this.tableMeterData.SelectedRows.Count == 1;
      if (this.tableMeterData.RowCount <= 0)
        return;
      this.tableMeterData.Rows[0].Selected = true;
      this.tableMeterData_SelectionChanged(sender, e);
    }

    private void tableMeterData_SelectionChanged(object sender, EventArgs e)
    {
      if (this.tableMeterData.SelectedRows.Count != 1 || this.tableMeterData.SelectedRows[0].Tag == null)
        return;
      this.txtInfo.Text = string.Empty;
      if (!(this.tableMeterData.SelectedRows[0].Tag is MeterData tag))
        return;
      try
      {
        MinoprotectIII minoprotectIii = MinoprotectIII.Unzip(tag.Buffer);
        if (minoprotectIii == null)
          return;
        this.txtInfo.Text = minoprotectIii.ToString();
      }
      catch (Exception ex)
      {
        this.txtInfo.Text = ex.Message;
      }
    }

    private void btnLoad_Click(object sender, EventArgs e)
    {
      if (this.tableMeterData.SelectedRows.Count != 1 || !(this.tableMeterData.SelectedRows[0].Tag is MeterData tag))
        return;
      this.handler.OpenDevice(tag);
      this.Close();
    }

    private void tableMeterData_DoubleClick(object sender, EventArgs e)
    {
      this.btnLoad_Click(sender, e);
    }

    private void ckboxShowOnlyEDC_CheckedChanged(object sender, EventArgs e) => this.ReloadMeter();

    private void btnReloadMeters_Click(object sender, EventArgs e) => this.ReloadMeter();

    private void txtProductionDate_ValueChanged(object sender, EventArgs e)
    {
      TimeSpan timeSpan = this.txtProductionDateEnd.Value - this.txtProductionDateStart.Value;
      if ((long) timeSpan.TotalDays > 0L)
        this.lblInterval.Text = "(" + ((long) timeSpan.TotalDays).ToString() + " days)";
      else
        this.lblInterval.Text = "(" + ((long) timeSpan.TotalHours).ToString() + " hours)";
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
      this.tableMeter.Rows.Clear();
      this.tableMeterData.Rows.Clear();
      this.txtInfo.Text = string.Empty;
      ExtendedSearch.ShowDialog((Form) this, new DateTime?(this.txtProductionDateStart.Value), new DateTime?(this.txtProductionDateEnd.Value));
    }

    private void btnStop_Click(object sender, EventArgs e) => SmokeDetectorDatabase.CancelSearch();

    private void btnExportToCSV_Click(object sender, EventArgs e)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
      saveFileDialog.FilterIndex = 2;
      saveFileDialog.RestoreDirectory = true;
      saveFileDialog.FileName = string.Format("{0}.csv", (object) DateTime.Now.ToShortDateString());
      if (saveFileDialog.ShowDialog() != DialogResult.OK)
        return;
      new CsvStyle().GenerateFile((DataTable) this.tableMeter.DataSource);
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        Stream stream = saveFileDialog.OpenFile();
        if (stream == null)
          return;
        using (stream)
        {
          byte[] bytes1 = Encoding.Default.GetBytes("MeterID;MeterInfoID;SerialNr;ProductionDate;ApprovalDate;OrderNr;" + Environment.NewLine);
          stream.Write(bytes1, 0, bytes1.Length);
          foreach (DataGridViewRow row in (IEnumerable) this.tableMeter.Rows)
          {
            byte[] bytes2 = Encoding.Default.GetBytes(string.Format("{0};{1};{2};{3};{4};{5};{6}", row.Cells[0].Value, row.Cells[1].Value, row.Cells[2].Value, row.Cells[3].Value == null ? (object) "" : (object) row.Cells[3].Value.ToString(), row.Cells[4].Value == null ? (object) "" : (object) row.Cells[4].Value.ToString(), row.Cells[5].Value, (object) Environment.NewLine));
            stream.Write(bytes2, 0, bytes2.Length);
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("Export", ex.Message);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void ReloadMeter()
    {
      this.tableMeter.SuspendLayout();
      this.tableMeter.Rows.Clear();
      this.tableMeterData.Rows.Clear();
      this.txtInfo.Text = string.Empty;
      this.lblCountOfMeters.Text = string.Empty;
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        if (this.handler.WorkMeter != null && this.handler.WorkMeter.Version != null)
        {
          Meter meter = SmokeDetectorDatabase.GetMeter(this.handler.WorkMeter.ManufacturingParameter.MeterID);
          if (meter != null)
          {
            this.tableMeter.Rows.Add((object) meter.MeterID, (object) meter.MeterInfoID, (object) meter.SerialNr, (object) meter.ProductionDate, (object) meter.ApprovalDate, (object) meter.OrderNr);
            return;
          }
        }
        List<Meter> meterList = SmokeDetectorDatabase.LoadMeter(new DateTime?(this.txtProductionDateStart.Value), new DateTime?(this.txtProductionDateEnd.Value));
        if (meterList == null)
        {
          ZR_ClassLibMessages.ShowAndClearErrors();
        }
        else
        {
          foreach (Meter meter in meterList)
            this.tableMeter.Rows.Add((object) meter.MeterID, (object) meter.MeterInfoID, (object) meter.SerialNr, (object) meter.ProductionDate, (object) meter.ApprovalDate, (object) meter.OrderNr);
          this.lblCountOfMeters.Text = this.tableMeter.Rows.Count.ToString();
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, "Unable to load meter! " + ex.Message, "Meter table", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
        this.tableMeter.ResumeLayout();
        Cursor.Current = Cursors.Default;
      }
    }

    private static void SmokeDetectorDatabase_OnMeterFound(object sender, Meter meter)
    {
      if (BackupEditor.dlgBackup.tableMeter.InvokeRequired)
      {
        BackupEditor.dlgBackup.tableMeter.Invoke((Delegate) new EventHandler<Meter>(BackupEditor.SmokeDetectorDatabase_OnMeterFound), sender, (object) meter);
      }
      else
      {
        BackupEditor.dlgBackup.tableMeter.Rows.Add((object) meter.MeterID, (object) meter.MeterInfoID, (object) meter.SerialNr, (object) meter.ProductionDate, (object) meter.ApprovalDate, (object) meter.OrderNr);
        BackupEditor.dlgBackup.lblCountOfMeters.Text = BackupEditor.dlgBackup.tableMeter.Rows.Count.ToString();
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (BackupEditor));
      DataGridViewCellStyle gridViewCellStyle1 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle2 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle3 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle4 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle5 = new DataGridViewCellStyle();
      DataGridViewCellStyle gridViewCellStyle6 = new DataGridViewCellStyle();
      this.btnCancel = new Button();
      this.btnLoad = new Button();
      this.tableMeter = new DataGridView();
      this.colMeterId = new DataGridViewTextBoxColumn();
      this.colMeterInfoID = new DataGridViewTextBoxColumn();
      this.colSerialnumber = new DataGridViewTextBoxColumn();
      this.colProductionDate = new DataGridViewTextBoxColumn();
      this.colApprovalDate = new DataGridViewTextBoxColumn();
      this.colOrderNr = new DataGridViewTextBoxColumn();
      this.contextMenuMeter = new ContextMenuStrip(this.components);
      this.btnExportToCSV = new ToolStripMenuItem();
      this.groupBox1 = new GroupBox();
      this.lblCountOfMeters = new Label();
      this.groupBox2 = new GroupBox();
      this.tableMeterData = new DataGridView();
      this.colPos = new DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
      this.colTimePoint = new DataGridViewTextBoxColumn();
      this.colHardwareTypeID = new DataGridViewTextBoxColumn();
      this.colData = new DataGridViewTextBoxColumn();
      this.colSize = new DataGridViewTextBoxColumn();
      this.groupBox3 = new GroupBox();
      this.txtInfo = new TextBox();
      this.label1 = new Label();
      this.gboxFilter = new GroupBox();
      this.groupBox4 = new GroupBox();
      this.btnStop = new Button();
      this.btnSearch = new Button();
      this.lblInterval = new Label();
      this.btnReloadMeters = new Button();
      this.label2 = new Label();
      this.txtProductionDateEnd = new DateTimePicker();
      this.txtProductionDateStart = new DateTimePicker();
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      ((ISupportInitialize) this.tableMeter).BeginInit();
      this.contextMenuMeter.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      ((ISupportInitialize) this.tableMeterData).BeginInit();
      this.groupBox3.SuspendLayout();
      this.gboxFilter.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Image = (Image) componentResourceManager.GetObject("btnCancel.Image");
      this.btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCancel.ImeMode = ImeMode.NoControl;
      this.btnCancel.Location = new Point(823, 521);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(77, 29);
      this.btnCancel.TabIndex = 23;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.TextAlign = ContentAlignment.MiddleRight;
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnLoad.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnLoad.Enabled = false;
      this.btnLoad.Image = (Image) componentResourceManager.GetObject("btnLoad.Image");
      this.btnLoad.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnLoad.ImeMode = ImeMode.NoControl;
      this.btnLoad.Location = new Point(723, 521);
      this.btnLoad.Name = "btnLoad";
      this.btnLoad.Size = new Size(77, 29);
      this.btnLoad.TabIndex = 22;
      this.btnLoad.Text = "Load";
      this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
      this.tableMeter.AllowUserToAddRows = false;
      this.tableMeter.AllowUserToDeleteRows = false;
      this.tableMeter.AllowUserToResizeRows = false;
      this.tableMeter.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.tableMeter.BackgroundColor = Color.White;
      gridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle1.BackColor = SystemColors.Control;
      gridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle1.ForeColor = SystemColors.WindowText;
      gridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle1.WrapMode = DataGridViewTriState.True;
      this.tableMeter.ColumnHeadersDefaultCellStyle = gridViewCellStyle1;
      this.tableMeter.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.tableMeter.Columns.AddRange((DataGridViewColumn) this.colMeterId, (DataGridViewColumn) this.colMeterInfoID, (DataGridViewColumn) this.colSerialnumber, (DataGridViewColumn) this.colProductionDate, (DataGridViewColumn) this.colApprovalDate, (DataGridViewColumn) this.colOrderNr);
      this.tableMeter.ContextMenuStrip = this.contextMenuMeter;
      gridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle2.BackColor = SystemColors.Window;
      gridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle2.ForeColor = SystemColors.ControlText;
      gridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle2.WrapMode = DataGridViewTriState.False;
      this.tableMeter.DefaultCellStyle = gridViewCellStyle2;
      this.tableMeter.Dock = DockStyle.Fill;
      this.tableMeter.Location = new Point(3, 16);
      this.tableMeter.MultiSelect = false;
      this.tableMeter.Name = "tableMeter";
      this.tableMeter.ReadOnly = true;
      gridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle3.BackColor = SystemColors.Control;
      gridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle3.ForeColor = SystemColors.WindowText;
      gridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle3.WrapMode = DataGridViewTriState.True;
      this.tableMeter.RowHeadersDefaultCellStyle = gridViewCellStyle3;
      this.tableMeter.RowHeadersVisible = false;
      this.tableMeter.ScrollBars = ScrollBars.Vertical;
      this.tableMeter.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.tableMeter.Size = new Size(511, 181);
      this.tableMeter.TabIndex = 24;
      this.tableMeter.SelectionChanged += new System.EventHandler(this.tableMeter_SelectionChanged);
      this.colMeterId.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      this.colMeterId.FillWeight = 60f;
      this.colMeterId.HeaderText = "MeterID";
      this.colMeterId.Name = "colMeterId";
      this.colMeterId.ReadOnly = true;
      this.colMeterId.Width = 70;
      this.colMeterInfoID.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      this.colMeterInfoID.FillWeight = 30f;
      this.colMeterInfoID.HeaderText = "Meter Info ID";
      this.colMeterInfoID.Name = "colMeterInfoID";
      this.colMeterInfoID.ReadOnly = true;
      this.colMeterInfoID.Width = 77;
      this.colSerialnumber.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      this.colSerialnumber.FillWeight = 72.30192f;
      this.colSerialnumber.HeaderText = "Serial number";
      this.colSerialnumber.Name = "colSerialnumber";
      this.colSerialnumber.ReadOnly = true;
      this.colSerialnumber.Width = 88;
      this.colProductionDate.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      this.colProductionDate.HeaderText = "Production date";
      this.colProductionDate.Name = "colProductionDate";
      this.colProductionDate.ReadOnly = true;
      this.colProductionDate.Width = 98;
      this.colApprovalDate.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      this.colApprovalDate.HeaderText = "Approval date";
      this.colApprovalDate.Name = "colApprovalDate";
      this.colApprovalDate.ReadOnly = true;
      this.colApprovalDate.Width = 90;
      this.colOrderNr.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      this.colOrderNr.FillWeight = 72.30192f;
      this.colOrderNr.HeaderText = "OrderNr";
      this.colOrderNr.Name = "colOrderNr";
      this.colOrderNr.ReadOnly = true;
      this.colOrderNr.Width = 69;
      this.contextMenuMeter.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.btnExportToCSV
      });
      this.contextMenuMeter.Name = "contextMenuMeter";
      this.contextMenuMeter.Size = new Size(146, 26);
      this.btnExportToCSV.Name = "btnExportToCSV";
      this.btnExportToCSV.Size = new Size(145, 22);
      this.btnExportToCSV.Text = "Export to CSV";
      this.btnExportToCSV.Click += new System.EventHandler(this.btnExportToCSV_Click);
      this.groupBox1.Controls.Add((Control) this.lblCountOfMeters);
      this.groupBox1.Controls.Add((Control) this.tableMeter);
      this.groupBox1.Location = new Point(3, 131);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(517, 200);
      this.groupBox1.TabIndex = 25;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Meter";
      this.lblCountOfMeters.AutoSize = true;
      this.lblCountOfMeters.Location = new Point(38, -1);
      this.lblCountOfMeters.Name = "lblCountOfMeters";
      this.lblCountOfMeters.Size = new Size(0, 13);
      this.lblCountOfMeters.TabIndex = 25;
      this.groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.groupBox2.Controls.Add((Control) this.tableMeterData);
      this.groupBox2.Location = new Point(3, 337);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(517, 178);
      this.groupBox2.TabIndex = 26;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Meter Data";
      this.tableMeterData.AllowUserToAddRows = false;
      this.tableMeterData.AllowUserToDeleteRows = false;
      this.tableMeterData.AllowUserToResizeRows = false;
      this.tableMeterData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
      this.tableMeterData.BackgroundColor = Color.White;
      gridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle4.BackColor = SystemColors.Control;
      gridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle4.ForeColor = SystemColors.WindowText;
      gridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle4.WrapMode = DataGridViewTriState.True;
      this.tableMeterData.ColumnHeadersDefaultCellStyle = gridViewCellStyle4;
      this.tableMeterData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.tableMeterData.Columns.AddRange((DataGridViewColumn) this.colPos, (DataGridViewColumn) this.dataGridViewTextBoxColumn1, (DataGridViewColumn) this.colTimePoint, (DataGridViewColumn) this.colHardwareTypeID, (DataGridViewColumn) this.colData, (DataGridViewColumn) this.colSize);
      gridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle5.BackColor = SystemColors.Window;
      gridViewCellStyle5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle5.ForeColor = SystemColors.ControlText;
      gridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle5.WrapMode = DataGridViewTriState.False;
      this.tableMeterData.DefaultCellStyle = gridViewCellStyle5;
      this.tableMeterData.Dock = DockStyle.Fill;
      this.tableMeterData.Location = new Point(3, 16);
      this.tableMeterData.MultiSelect = false;
      this.tableMeterData.Name = "tableMeterData";
      this.tableMeterData.ReadOnly = true;
      gridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
      gridViewCellStyle6.BackColor = SystemColors.Control;
      gridViewCellStyle6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      gridViewCellStyle6.ForeColor = SystemColors.WindowText;
      gridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
      gridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
      gridViewCellStyle6.WrapMode = DataGridViewTriState.True;
      this.tableMeterData.RowHeadersDefaultCellStyle = gridViewCellStyle6;
      this.tableMeterData.RowHeadersVisible = false;
      this.tableMeterData.ScrollBars = ScrollBars.Vertical;
      this.tableMeterData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.tableMeterData.Size = new Size(511, 159);
      this.tableMeterData.TabIndex = 24;
      this.tableMeterData.SelectionChanged += new System.EventHandler(this.tableMeterData_SelectionChanged);
      this.tableMeterData.DoubleClick += new System.EventHandler(this.tableMeterData_DoubleClick);
      this.colPos.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      this.colPos.HeaderText = "#";
      this.colPos.Name = "colPos";
      this.colPos.ReadOnly = true;
      this.colPos.Width = 39;
      this.dataGridViewTextBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      this.dataGridViewTextBoxColumn1.FillWeight = 57.84154f;
      this.dataGridViewTextBoxColumn1.HeaderText = "MeterID";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.ReadOnly = true;
      this.dataGridViewTextBoxColumn1.Width = 70;
      this.colTimePoint.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      this.colTimePoint.HeaderText = "Time Point";
      this.colTimePoint.Name = "colTimePoint";
      this.colTimePoint.ReadOnly = true;
      this.colTimePoint.Width = 82;
      this.colHardwareTypeID.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      this.colHardwareTypeID.FillWeight = 80f;
      this.colHardwareTypeID.HeaderText = "HardwareTypeID";
      this.colHardwareTypeID.Name = "colHardwareTypeID";
      this.colHardwareTypeID.ReadOnly = true;
      this.colHardwareTypeID.Width = 113;
      this.colData.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      this.colData.HeaderText = "Data";
      this.colData.Name = "colData";
      this.colData.ReadOnly = true;
      this.colData.Width = 55;
      this.colSize.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      this.colSize.HeaderText = "Size";
      this.colSize.Name = "colSize";
      this.colSize.ReadOnly = true;
      this.colSize.Width = 52;
      this.groupBox3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox3.Controls.Add((Control) this.txtInfo);
      this.groupBox3.Location = new Point(526, 42);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(377, 473);
      this.groupBox3.TabIndex = 27;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Info";
      this.txtInfo.Dock = DockStyle.Fill;
      this.txtInfo.Font = new Font("Consolas", 8.25f);
      this.txtInfo.Location = new Point(3, 16);
      this.txtInfo.Multiline = true;
      this.txtInfo.Name = "txtInfo";
      this.txtInfo.ScrollBars = ScrollBars.Vertical;
      this.txtInfo.Size = new Size(371, 454);
      this.txtInfo.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 15);
      this.label1.Name = "label1";
      this.label1.Size = new Size(85, 13);
      this.label1.TabIndex = 28;
      this.label1.Text = "Production date:";
      this.gboxFilter.Controls.Add((Control) this.groupBox4);
      this.gboxFilter.Controls.Add((Control) this.lblInterval);
      this.gboxFilter.Controls.Add((Control) this.btnReloadMeters);
      this.gboxFilter.Controls.Add((Control) this.label2);
      this.gboxFilter.Controls.Add((Control) this.txtProductionDateEnd);
      this.gboxFilter.Controls.Add((Control) this.txtProductionDateStart);
      this.gboxFilter.Controls.Add((Control) this.label1);
      this.gboxFilter.Location = new Point(6, 42);
      this.gboxFilter.Name = "gboxFilter";
      this.gboxFilter.Size = new Size(498, 85);
      this.gboxFilter.TabIndex = 29;
      this.gboxFilter.TabStop = false;
      this.gboxFilter.Text = "Filter";
      this.groupBox4.Controls.Add((Control) this.btnStop);
      this.groupBox4.Controls.Add((Control) this.btnSearch);
      this.groupBox4.Location = new Point(329, 34);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new Size(163, 45);
      this.groupBox4.TabIndex = 35;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Extended search";
      this.btnStop.Location = new Point(75, 15);
      this.btnStop.Name = "btnStop";
      this.btnStop.Size = new Size(75, 23);
      this.btnStop.TabIndex = 35;
      this.btnStop.Text = "Stop";
      this.btnStop.UseVisualStyleBackColor = true;
      this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
      this.btnSearch.Image = (Image) componentResourceManager.GetObject("btnSearch.Image");
      this.btnSearch.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnSearch.ImeMode = ImeMode.NoControl;
      this.btnSearch.Location = new Point(6, 15);
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new Size(63, 24);
      this.btnSearch.TabIndex = 34;
      this.btnSearch.Text = "Start";
      this.btnSearch.TextAlign = ContentAlignment.MiddleRight;
      this.btnSearch.UseVisualStyleBackColor = true;
      this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
      this.lblInterval.AutoSize = true;
      this.lblInterval.Location = new Point(377, 15);
      this.lblInterval.Name = "lblInterval";
      this.lblInterval.Size = new Size(50, 13);
      this.lblInterval.TabIndex = 33;
      this.lblInterval.Text = "{Interval}";
      this.btnReloadMeters.Image = (Image) componentResourceManager.GetObject("btnReloadMeters.Image");
      this.btnReloadMeters.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnReloadMeters.ImeMode = ImeMode.NoControl;
      this.btnReloadMeters.Location = new Point(12, 38);
      this.btnReloadMeters.Name = "btnReloadMeters";
      this.btnReloadMeters.Size = new Size(110, 26);
      this.btnReloadMeters.TabIndex = 32;
      this.btnReloadMeters.Text = "Reload meters";
      this.btnReloadMeters.TextAlign = ContentAlignment.MiddleRight;
      this.btnReloadMeters.UseVisualStyleBackColor = true;
      this.btnReloadMeters.Click += new System.EventHandler(this.btnReloadMeters_Click);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(229, 15);
      this.label2.Name = "label2";
      this.label2.Size = new Size(10, 13);
      this.label2.TabIndex = 31;
      this.label2.Text = "-";
      this.txtProductionDateEnd.CustomFormat = "dd.MM.yyyy HH:mm";
      this.txtProductionDateEnd.Format = DateTimePickerFormat.Custom;
      this.txtProductionDateEnd.Location = new Point(245, 12);
      this.txtProductionDateEnd.Name = "txtProductionDateEnd";
      this.txtProductionDateEnd.Size = new Size(125, 20);
      this.txtProductionDateEnd.TabIndex = 30;
      this.txtProductionDateEnd.ValueChanged += new System.EventHandler(this.txtProductionDate_ValueChanged);
      this.txtProductionDateEnd.KeyUp += new KeyEventHandler(this.txtProductionDate_KeyUp);
      this.txtProductionDateStart.CustomFormat = "dd.MM.yyyy HH:mm";
      this.txtProductionDateStart.Format = DateTimePickerFormat.Custom;
      this.txtProductionDateStart.Location = new Point(98, 12);
      this.txtProductionDateStart.Name = "txtProductionDateStart";
      this.txtProductionDateStart.Size = new Size(125, 20);
      this.txtProductionDateStart.TabIndex = 29;
      this.txtProductionDateStart.ValueChanged += new System.EventHandler(this.txtProductionDate_ValueChanged);
      this.txtProductionDateStart.KeyUp += new KeyEventHandler(this.txtProductionDate_KeyUp);
      this.zennerCoroprateDesign1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.zennerCoroprateDesign1.Location = new Point(0, 0);
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      this.zennerCoroprateDesign1.Size = new Size(912, 41);
      this.zennerCoroprateDesign1.TabIndex = 21;
      this.AcceptButton = (IButtonControl) this.btnLoad;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(912, 562);
      this.Controls.Add((Control) this.gboxFilter);
      this.Controls.Add((Control) this.groupBox3);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnLoad);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (BackupEditor);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Backups";
      this.Load += new System.EventHandler(this.BackupEditor_Load);
      ((ISupportInitialize) this.tableMeter).EndInit();
      this.contextMenuMeter.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      ((ISupportInitialize) this.tableMeterData).EndInit();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.gboxFilter.ResumeLayout(false);
      this.gboxFilter.PerformLayout();
      this.groupBox4.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}

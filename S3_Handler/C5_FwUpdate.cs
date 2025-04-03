// Decompiled with JetBrains decompiler
// Type: S3_Handler.C5_FwUpdate
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using HandlerLib;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ZR_ClassLibrary;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace S3_Handler
{
  public class C5_FwUpdate : Form
  {
    private S3_HandlerFunctions MyFunctions;
    private string FirmwareFile;
    private int MapId;
    private bool ignoreOriginalMeter;
    private bool firmwareOnly;
    private IContainer components = (IContainer) null;
    private Button btnC5FwUpDate;
    private ProgressBar progressBar1;
    private DataGridView dataGridViewC5Firmware;
    private Label lblSelectedFirmware;
    private Label lblProgressState;
    private CheckBox chBxIgnoreOriginalMeter;
    private CheckBox chkBx_firmwareOnly;
    private CheckBox ckBxUpdate2FW525;
    private Label lblConnectedController;

    public C5_FwUpdate(S3_HandlerFunctions MyFunctions)
    {
      this.MyFunctions = MyFunctions;
      this.ignoreOriginalMeter = false;
      this.firmwareOnly = false;
      this.InitializeComponent();
      try
      {
        Schema.ProgFilesDataTable Table = new Schema.ProgFilesDataTable();
        this.MyFunctions.MyDatabase.GetDataTableBySQLCommand("SELECT * FROM ProgFiles WHERE ProgFileName LIKE 'C5_%' OR ProgFileName LIKE '%_C5_%' OR ProgFileName LIKE '%_WR4_%' ORDER BY MapID", (DataTable) Table);
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add(new DataColumn(C5_FwUpdate.ProgFileTableRows.MapID.ToString(), typeof (int)));
        DataColumnCollection columns1 = dataTable.Columns;
        C5_FwUpdate.ProgFileTableRows progFileTableRows = C5_FwUpdate.ProgFileTableRows.ProgFileName;
        DataColumn column1 = new DataColumn(progFileTableRows.ToString(), typeof (string));
        columns1.Add(column1);
        DataColumnCollection columns2 = dataTable.Columns;
        progFileTableRows = C5_FwUpdate.ProgFileTableRows.Options;
        DataColumn column2 = new DataColumn(progFileTableRows.ToString(), typeof (string));
        columns2.Add(column2);
        DataColumnCollection columns3 = dataTable.Columns;
        progFileTableRows = C5_FwUpdate.ProgFileTableRows.SourceInfo;
        DataColumn column3 = new DataColumn(progFileTableRows.ToString(), typeof (string));
        columns3.Add(column3);
        foreach (Schema.ProgFilesRow progFilesRow in (TypedTableBase<Schema.ProgFilesRow>) Table)
        {
          DataRow row = dataTable.NewRow();
          row[0] = (object) progFilesRow.MapID;
          row[1] = (object) progFilesRow.ProgFileName;
          row[2] = (object) progFilesRow.Options;
          row[3] = (object) progFilesRow.SourceInfo;
          dataTable.Rows.Add(row);
        }
        this.dataGridViewC5Firmware.DataSource = (object) dataTable;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ex.ToString());
        ZR_ClassLibMessages.AddErrorDescriptionAndException(ZR_ClassLibMessages.LastErrors.IllegalData, "Read data from table MeterHardware");
      }
    }

    private void dataGridViewC5Firmware_RowMouseDoubleClick(
      object sender,
      DataGridViewCellMouseEventArgs e)
    {
      string str = this.dataGridViewC5Firmware.Rows[e.RowIndex].Cells[0].Value.ToString();
      if (str == string.Empty)
        return;
      this.MapId = (int) Convert.ToInt16(str);
      FirmwareData firmwareData = HardwareTypeSupport.GetFirmwareData((uint) this.MapId);
      this.FirmwareFile = firmwareData.ProgrammerFileAsString;
      this.lblSelectedFirmware.Text = "MapID: " + str + "   FirmWare: " + firmwareData.ProgFileName;
      this.lblSelectedFirmware.Visible = true;
      this.btnC5FwUpDate.Enabled = true;
    }

    private string C5_GetConnectedController()
    {
      this.MyFunctions.ConnectDevice();
      return "MSP430" + this.MyFunctions.ReadDeviceDescriptor().Chip.ToString();
    }

    private void btnC5FwUpDate_Click(object sender, EventArgs e)
    {
      this.MyFunctions.OnS3_HandlerMessage += new EventHandler<GMM_EventArgs>(this.C5_Update_OnProgress);
      this.lblConnectedController.Text = this.C5_GetConnectedController();
      try
      {
        if (this.ckBxUpdate2FW525.Checked)
          this.MyFunctions.C5_FirmwareUpdate_To_FW_5_2_5();
        else
          this.MyFunctions.C5_firmwareUpdate(this.MapId, this.FirmwareFile, this.ignoreOriginalMeter, this.firmwareOnly);
      }
      catch (HandlerMessageException ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("C5 FirmwareUpdate", ex.Message);
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("C5 FirmwareUpdate", ex.ToString());
      }
      this.MyFunctions.OnS3_HandlerMessage -= new EventHandler<GMM_EventArgs>(this.C5_Update_OnProgress);
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void C5_Update_OnProgress(object sender, GMM_EventArgs e)
    {
      if (e.TheMessageType != GMM_EventArgs.MessageType.C5_FirmwareUpdate)
        return;
      this.lblProgressState.Visible = true;
      this.lblProgressState.Text = e.EventMessage;
      this.progressBar1.Value = e.InfoNumber;
    }

    private void chBxIgnoreOriginalMeter_CheckedChanged(object sender, EventArgs e)
    {
      this.ignoreOriginalMeter = this.chBxIgnoreOriginalMeter.Checked;
    }

    private void chkBx_frimwareOnly_CheckedChanged(object sender, EventArgs e)
    {
      this.firmwareOnly = this.chkBx_firmwareOnly.Checked;
      if (this.firmwareOnly)
      {
        this.ignoreOriginalMeter = this.chBxIgnoreOriginalMeter.Checked = true;
        this.chBxIgnoreOriginalMeter.Enabled = false;
      }
      else
        this.chBxIgnoreOriginalMeter.Enabled = true;
    }

    private void ckBxUpdate2FW525_CheckedChanged(object sender, EventArgs e)
    {
      if (this.ckBxUpdate2FW525.Checked)
      {
        this.btnC5FwUpDate.Enabled = true;
        this.chBxIgnoreOriginalMeter.Enabled = false;
        this.chkBx_firmwareOnly.Enabled = false;
      }
      else
      {
        this.btnC5FwUpDate.Enabled = false;
        this.chBxIgnoreOriginalMeter.Enabled = true;
        this.chkBx_firmwareOnly.Enabled = true;
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
      this.btnC5FwUpDate = new Button();
      this.progressBar1 = new ProgressBar();
      this.dataGridViewC5Firmware = new DataGridView();
      this.lblSelectedFirmware = new Label();
      this.lblProgressState = new Label();
      this.chBxIgnoreOriginalMeter = new CheckBox();
      this.chkBx_firmwareOnly = new CheckBox();
      this.ckBxUpdate2FW525 = new CheckBox();
      this.lblConnectedController = new Label();
      ((ISupportInitialize) this.dataGridViewC5Firmware).BeginInit();
      this.SuspendLayout();
      this.btnC5FwUpDate.Anchor = AnchorStyles.Bottom;
      this.btnC5FwUpDate.Enabled = false;
      this.btnC5FwUpDate.Location = new Point(294, 389);
      this.btnC5FwUpDate.Name = "btnC5FwUpDate";
      this.btnC5FwUpDate.Size = new Size(129, 33);
      this.btnC5FwUpDate.TabIndex = 0;
      this.btnC5FwUpDate.Text = "Start UpDate";
      this.btnC5FwUpDate.UseVisualStyleBackColor = true;
      this.btnC5FwUpDate.Click += new System.EventHandler(this.btnC5FwUpDate_Click);
      this.progressBar1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.progressBar1.Location = new Point(15, 374);
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new Size(775, 9);
      this.progressBar1.TabIndex = 1;
      this.dataGridViewC5Firmware.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dataGridViewC5Firmware.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.dataGridViewC5Firmware.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewC5Firmware.Location = new Point(12, 12);
      this.dataGridViewC5Firmware.Name = "dataGridViewC5Firmware";
      this.dataGridViewC5Firmware.Size = new Size(775, 356);
      this.dataGridViewC5Firmware.TabIndex = 2;
      this.dataGridViewC5Firmware.RowHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dataGridViewC5Firmware_RowMouseDoubleClick);
      this.dataGridViewC5Firmware.RowHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(this.dataGridViewC5Firmware_RowMouseDoubleClick);
      this.lblSelectedFirmware.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblSelectedFirmware.AutoSize = true;
      this.lblSelectedFirmware.Location = new Point(12, 391);
      this.lblSelectedFirmware.Name = "lblSelectedFirmware";
      this.lblSelectedFirmware.Size = new Size(35, 13);
      this.lblSelectedFirmware.TabIndex = 3;
      this.lblSelectedFirmware.Text = "label1";
      this.lblSelectedFirmware.Visible = false;
      this.lblProgressState.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.lblProgressState.Location = new Point(650, 399);
      this.lblProgressState.Name = "lblProgressState";
      this.lblProgressState.Size = new Size(137, 13);
      this.lblProgressState.TabIndex = 4;
      this.lblProgressState.Text = "label2";
      this.lblProgressState.TextAlign = ContentAlignment.TopRight;
      this.lblProgressState.Visible = false;
      this.chBxIgnoreOriginalMeter.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.chBxIgnoreOriginalMeter.Cursor = Cursors.Default;
      this.chBxIgnoreOriginalMeter.Location = new Point(454, 387);
      this.chBxIgnoreOriginalMeter.Name = "chBxIgnoreOriginalMeter";
      this.chBxIgnoreOriginalMeter.Size = new Size(137, 23);
      this.chBxIgnoreOriginalMeter.TabIndex = 5;
      this.chBxIgnoreOriginalMeter.Text = "ignore original meter";
      this.chBxIgnoreOriginalMeter.UseVisualStyleBackColor = true;
      this.chBxIgnoreOriginalMeter.CheckedChanged += new System.EventHandler(this.chBxIgnoreOriginalMeter_CheckedChanged);
      this.chkBx_firmwareOnly.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.chkBx_firmwareOnly.Location = new Point(454, 406);
      this.chkBx_firmwareOnly.Name = "chkBx_firmwareOnly";
      this.chkBx_firmwareOnly.Size = new Size(120, 16);
      this.chkBx_firmwareOnly.TabIndex = 6;
      this.chkBx_firmwareOnly.Text = "firmware only";
      this.chkBx_firmwareOnly.UseVisualStyleBackColor = true;
      this.chkBx_firmwareOnly.CheckedChanged += new System.EventHandler(this.chkBx_frimwareOnly_CheckedChanged);
      this.ckBxUpdate2FW525.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.ckBxUpdate2FW525.AutoSize = true;
      this.ckBxUpdate2FW525.Location = new Point(580, 406);
      this.ckBxUpdate2FW525.Name = "ckBxUpdate2FW525";
      this.ckBxUpdate2FW525.Size = new Size(79, 17);
      this.ckBxUpdate2FW525.TabIndex = 7;
      this.ckBxUpdate2FW525.Text = "to FW5.2.5";
      this.ckBxUpdate2FW525.UseVisualStyleBackColor = true;
      this.ckBxUpdate2FW525.CheckedChanged += new System.EventHandler(this.ckBxUpdate2FW525_CheckedChanged);
      this.lblConnectedController.AutoSize = true;
      this.lblConnectedController.Location = new Point(12, 410);
      this.lblConnectedController.Name = "lblConnectedController";
      this.lblConnectedController.Size = new Size(38, 13);
      this.lblConnectedController.TabIndex = 8;
      this.lblConnectedController.Text = "Target";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(799, 434);
      this.Controls.Add((Control) this.lblConnectedController);
      this.Controls.Add((Control) this.ckBxUpdate2FW525);
      this.Controls.Add((Control) this.chkBx_firmwareOnly);
      this.Controls.Add((Control) this.chBxIgnoreOriginalMeter);
      this.Controls.Add((Control) this.lblProgressState);
      this.Controls.Add((Control) this.lblSelectedFirmware);
      this.Controls.Add((Control) this.dataGridViewC5Firmware);
      this.Controls.Add((Control) this.progressBar1);
      this.Controls.Add((Control) this.btnC5FwUpDate);
      this.Name = nameof (C5_FwUpdate);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "C5FwUpdate";
      ((ISupportInitialize) this.dataGridViewC5Firmware).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private enum ProgFileTableRows
    {
      MapID,
      ProgFileName,
      Options,
      SourceInfo,
      HexText,
    }
  }
}

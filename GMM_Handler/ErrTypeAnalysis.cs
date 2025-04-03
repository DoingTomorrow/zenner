// Decompiled with JetBrains decompiler
// Type: GMM_Handler.ErrTypeAnalysis
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  public class ErrTypeAnalysis : Form
  {
    private ZR_HandlerFunctions MyHandler;
    private DataSetAllErr8002Meters AllErrMeters;
    private string DataSetFileName;
    private IContainer components = (IContainer) null;
    private Button buttonGenerateList;
    private DataGridView dataGridView1;
    private TextBox textBoxStatus;
    private Button buttonSave;
    private Button buttonImport;
    private Button buttonSearcheSerialNumber;
    private TextBox textBoxSerialNumber;

    public ErrTypeAnalysis(ZR_HandlerFunctions MyHandler)
    {
      this.InitializeComponent();
      this.MyHandler = MyHandler;
      this.DataSetFileName = Path.Combine(SystemValues.LoggDataPath, "AllErr8002Meters.xml");
      if (!File.Exists(this.DataSetFileName))
        return;
      try
      {
        this.AllErrMeters = new DataSetAllErr8002Meters();
        int num = (int) this.AllErrMeters.ReadXml(this.DataSetFileName);
        this.dataGridView1.DataSource = (object) this.AllErrMeters.Err8002Meter;
      }
      catch (Exception ex)
      {
        int num = (int) GMM_MessageBox.ShowMessage("XLM File", "Fehler beim laden des Datenfiles" + ex.ToString());
      }
    }

    private void buttonGenerateList_Click(object sender, EventArgs e)
    {
      this.AllErrMeters = new DataSetAllErr8002Meters();
      if (!this.MyHandler.MyDataBaseAccess.LoadMeterListFromTypeID(this.AllErrMeters.Err8002Meter, 107))
      {
        this.textBoxStatus.Text = "Lesefehler";
      }
      else
      {
        string str = this.AllErrMeters.Err8002Meter.Rows.Count.ToString();
        this.textBoxStatus.Text = "Anzahl Geräte: " + str;
        for (int index = 0; index < this.AllErrMeters.Err8002Meter.Rows.Count; ++index)
        {
          DateTime lastProgDate = this.MyHandler.MyDataBaseAccess.GetLastProgDate(this.AllErrMeters.Err8002Meter[index].MeterID);
          if (!(lastProgDate == DateTime.MinValue))
          {
            this.AllErrMeters.Err8002Meter[index].LastProgDate = lastProgDate;
            if (index % 10 == 0)
            {
              this.textBoxStatus.Text = index.ToString() + "/" + str;
              this.Refresh();
            }
          }
        }
        this.dataGridView1.DataSource = (object) this.AllErrMeters.Err8002Meter;
        this.Refresh();
        this.AllErrMeters.WriteXml(this.DataSetFileName, XmlWriteMode.WriteSchema);
      }
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
      this.AllErrMeters.WriteXml(this.DataSetFileName, XmlWriteMode.WriteSchema);
    }

    private void buttonSearcheSerialNumber_Click(object sender, EventArgs e)
    {
      string str = this.textBoxSerialNumber.Text.Trim();
      for (int index = 0; index < this.dataGridView1.Rows.Count; ++index)
      {
        if (this.dataGridView1.Rows[index].Cells["SerialNr"].Value.ToString() == str)
        {
          this.dataGridView1.Rows[index].Selected = true;
          this.dataGridView1.FirstDisplayedScrollingRowIndex = index;
          return;
        }
      }
      int num = (int) GMM_MessageBox.ShowMessage("Search", "Not found");
    }

    private void buttonImport_Click(object sender, EventArgs e)
    {
      DataSetLogData dataSetLogData = new DataSetLogData();
      string str = Path.Combine(SystemValues.LoggDataPath, "Serie2LogData.xml");
      if (File.Exists(str))
      {
        try
        {
          int num = (int) dataSetLogData.ReadXml(str);
        }
        catch (Exception ex)
        {
          int num = (int) GMM_MessageBox.ShowMessage("XLM File", "Fehler beim laden des Datenfiles" + ex.ToString());
        }
      }
      int num1 = 0;
      for (int index1 = 0; index1 < dataSetLogData.GMM_Serie2DeviceLogData.Rows.Count; ++index1)
      {
        int deviceNumber = dataSetLogData.GMM_Serie2DeviceLogData[index1].DeviceNumber;
        DataSetAllErr8002Meters.Err8002MeterRow[] err8002MeterRowArray = (DataSetAllErr8002Meters.Err8002MeterRow[]) this.AllErrMeters.Err8002Meter.Select("SerialNr = " + deviceNumber.ToString());
        if (err8002MeterRowArray.Length == 1)
        {
          DataSetLogData.GMM_Serie2DeviceLogDataRow[] deviceLogDataRowArray = (DataSetLogData.GMM_Serie2DeviceLogDataRow[]) dataSetLogData.GMM_Serie2DeviceLogData.Select("DeviceNumber = " + deviceNumber.ToString());
          for (int index2 = 0; index2 < deviceLogDataRowArray.Length; ++index2)
          {
            if (deviceLogDataRowArray[index2].IsManuellGespeichertNull() || !deviceLogDataRowArray[index2].ManuellGespeichert)
            {
              if (!deviceLogDataRowArray[index2].IsFehlerBeseitigtNull() && deviceLogDataRowArray[index2].FehlerBeseitigt)
              {
                err8002MeterRowArray[0].FehlerBeseitigt = true;
                ++num1;
              }
              else if (!deviceLogDataRowArray[index2].IsFehlerGefundenNull())
              {
                if (err8002MeterRowArray[0].IsFehlerGefundenNull())
                  err8002MeterRowArray[0].FehlerGefunden = deviceLogDataRowArray[index2].FehlerGefunden;
                else if (deviceLogDataRowArray[index2].FehlerGefunden)
                  err8002MeterRowArray[0].FehlerGefunden = true;
              }
            }
          }
        }
      }
      int num2 = (int) GMM_MessageBox.ShowMessage("Statistik", "Anzahl beseitigter Fehler: " + num1.ToString());
      this.dataGridView1.Update();
      this.dataGridView1.Refresh();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.buttonGenerateList = new Button();
      this.dataGridView1 = new DataGridView();
      this.textBoxStatus = new TextBox();
      this.buttonSave = new Button();
      this.buttonImport = new Button();
      this.buttonSearcheSerialNumber = new Button();
      this.textBoxSerialNumber = new TextBox();
      ((ISupportInitialize) this.dataGridView1).BeginInit();
      this.SuspendLayout();
      this.buttonGenerateList.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonGenerateList.Enabled = false;
      this.buttonGenerateList.Location = new Point(564, 294);
      this.buttonGenerateList.Name = "buttonGenerateList";
      this.buttonGenerateList.Size = new Size(79, 23);
      this.buttonGenerateList.TabIndex = 0;
      this.buttonGenerateList.Text = "GenerateList";
      this.buttonGenerateList.UseVisualStyleBackColor = true;
      this.buttonGenerateList.Click += new System.EventHandler(this.buttonGenerateList_Click);
      this.dataGridView1.AllowUserToAddRows = false;
      this.dataGridView1.AllowUserToOrderColumns = true;
      this.dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dataGridView1.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
      this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Location = new Point(4, 2);
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      this.dataGridView1.Size = new Size(800, 275);
      this.dataGridView1.TabIndex = 1;
      this.textBoxStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.textBoxStatus.Location = new Point(4, 297);
      this.textBoxStatus.Name = "textBoxStatus";
      this.textBoxStatus.Size = new Size(275, 20);
      this.textBoxStatus.TabIndex = 2;
      this.buttonSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonSave.Location = new Point(729, 295);
      this.buttonSave.Name = "buttonSave";
      this.buttonSave.Size = new Size(75, 23);
      this.buttonSave.TabIndex = 0;
      this.buttonSave.Text = "Save";
      this.buttonSave.UseVisualStyleBackColor = true;
      this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
      this.buttonImport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonImport.Location = new Point(649, 295);
      this.buttonImport.Name = "buttonImport";
      this.buttonImport.Size = new Size(74, 23);
      this.buttonImport.TabIndex = 0;
      this.buttonImport.Text = "Import";
      this.buttonImport.UseVisualStyleBackColor = true;
      this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
      this.buttonSearcheSerialNumber.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonSearcheSerialNumber.Location = new Point(392, 295);
      this.buttonSearcheSerialNumber.Name = "buttonSearcheSerialNumber";
      this.buttonSearcheSerialNumber.Size = new Size(79, 23);
      this.buttonSearcheSerialNumber.TabIndex = 0;
      this.buttonSearcheSerialNumber.Text = "Search SNR";
      this.buttonSearcheSerialNumber.UseVisualStyleBackColor = true;
      this.buttonSearcheSerialNumber.Click += new System.EventHandler(this.buttonSearcheSerialNumber_Click);
      this.textBoxSerialNumber.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.textBoxSerialNumber.Location = new Point(477, 297);
      this.textBoxSerialNumber.Name = "textBoxSerialNumber";
      this.textBoxSerialNumber.Size = new Size(81, 20);
      this.textBoxSerialNumber.TabIndex = 3;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(809, 329);
      this.Controls.Add((Control) this.textBoxSerialNumber);
      this.Controls.Add((Control) this.textBoxStatus);
      this.Controls.Add((Control) this.dataGridView1);
      this.Controls.Add((Control) this.buttonImport);
      this.Controls.Add((Control) this.buttonSave);
      this.Controls.Add((Control) this.buttonSearcheSerialNumber);
      this.Controls.Add((Control) this.buttonGenerateList);
      this.Name = nameof (ErrTypeAnalysis);
      this.Text = nameof (ErrTypeAnalysis);
      ((ISupportInitialize) this.dataGridView1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

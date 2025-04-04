// Decompiled with JetBrains decompiler
// Type: EDC_Handler.FirmwareEditor
// Assembly: EDC_Handler, Version=2.4.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 42F089F4-0B6A-4F46-A83B-212735A4FCEC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDC_Handler.dll

using CorporateDesign;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace EDC_Handler
{
  public class FirmwareEditor : Form
  {
    private EDC_HandlerFunctions MyFunctions;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private Button btnCancel;
    private Button btnUpgrade;
    private ComboBox cboxFirmware;
    private RadioButton rbtnUseFirmwareFromDB;
    private RadioButton rbtnUseFirmwareFromFile;
    private Button btnBrowse;
    private ProgressBar progressBar;

    internal FirmwareEditor() => this.InitializeComponent();

    private void FirmwareEditor_Load(object sender, EventArgs e)
    {
      this.LoadDatabaseFirmwares();
      this.MyFunctions.OnProgress += new ValueEventHandler<int>(this.MyFunctions_OnProgress);
    }

    private void FirmwareEditor_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.MyFunctions.OnProgress -= new ValueEventHandler<int>(this.MyFunctions_OnProgress);
    }

    internal static void ShowDialog(Form owner, EDC_HandlerFunctions MyFunctions)
    {
      if (MyFunctions == null)
        return;
      using (FirmwareEditor firmwareEditor = new FirmwareEditor())
      {
        firmwareEditor.MyFunctions = MyFunctions;
        int num = (int) firmwareEditor.ShowDialog((IWin32Window) owner);
      }
    }

    private void btnUpgrade_Click(object sender, EventArgs e)
    {
      byte[] firmware = this.GetFirmware();
      if (firmware == null)
      {
        int num1 = (int) MessageBox.Show("Invalid source of firmware!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.progressBar.Visible = true;
        this.progressBar.Value = 0;
        this.Enabled = false;
        try
        {
          if (this.MyFunctions.UpgradeFirmware(firmware))
          {
            int num2 = (int) MessageBox.Show("Successfully upgraded the firmware!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          else
          {
            int num3 = (int) MessageBox.Show("Failed upgrade the firmware!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
        }
        finally
        {
          this.progressBar.Visible = false;
          this.Enabled = true;
        }
      }
    }

    private void rbtnUseFirmwareFromFile_CheckedChanged(object sender, EventArgs e)
    {
      bool flag = this.rbtnUseFirmwareFromFile.Checked;
      this.btnBrowse.Visible = flag;
      if (flag)
      {
        this.cboxFirmware.DropDownStyle = ComboBoxStyle.Simple;
        this.cboxFirmware.Text = string.Empty;
      }
      else
      {
        this.cboxFirmware.DropDownStyle = ComboBoxStyle.DropDownList;
        this.LoadDatabaseFirmwares();
      }
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.RestoreDirectory = true;
        openFileDialog.Title = "Please select a file to upgrade";
        int num = (int) openFileDialog.ShowDialog();
        this.cboxFirmware.Text = openFileDialog.FileName;
      }
    }

    private void LoadDatabaseFirmwares()
    {
      List<Firmware> firmwareList = new List<Firmware>();
      List<Firmware> collection1 = EDC_Database.LoadFirmware(new EDC_Hardware?(EDC_Hardware.EDC_Radio));
      if (collection1 != null)
        firmwareList.AddRange((IEnumerable<Firmware>) collection1);
      List<Firmware> collection2 = EDC_Database.LoadFirmware(new EDC_Hardware?(EDC_Hardware.EDC_mBus));
      if (collection2 != null)
        firmwareList.AddRange((IEnumerable<Firmware>) collection2);
      List<Firmware> collection3 = EDC_Database.LoadFirmware(new EDC_Hardware?(EDC_Hardware.EDC_ModBus));
      if (collection3 != null)
        firmwareList.AddRange((IEnumerable<Firmware>) collection3);
      List<Firmware> collection4 = EDC_Database.LoadFirmware(new EDC_Hardware?(EDC_Hardware.EDC_mBus_Modbus));
      if (collection4 != null)
        firmwareList.AddRange((IEnumerable<Firmware>) collection4);
      List<Firmware> collection5 = EDC_Database.LoadFirmware(new EDC_Hardware?(EDC_Hardware.EDC_mBus_CJ188));
      if (collection5 != null)
        firmwareList.AddRange((IEnumerable<Firmware>) collection5);
      List<Firmware> collection6 = EDC_Database.LoadFirmware(new EDC_Hardware?(EDC_Hardware.EDC_RS485_Modbus));
      if (collection6 != null)
        firmwareList.AddRange((IEnumerable<Firmware>) collection6);
      List<Firmware> collection7 = EDC_Database.LoadFirmware(new EDC_Hardware?(EDC_Hardware.EDC_RS485_CJ188));
      if (collection7 != null)
        firmwareList.AddRange((IEnumerable<Firmware>) collection7);
      this.cboxFirmware.DataSource = (object) firmwareList;
      this.cboxFirmware.DisplayMember = "Version";
      this.cboxFirmware.ValueMember = "FirmwareText";
    }

    private byte[] GetFirmware()
    {
      return this.rbtnUseFirmwareFromFile.Checked ? (string.IsNullOrEmpty(this.cboxFirmware.Text) ? (byte[]) null : FirmwareManager.ReadFirmwareFromFile(this.cboxFirmware.Text)) : (this.cboxFirmware.SelectedValue == null ? (byte[]) null : FirmwareManager.ReadFirmwareFromText(this.cboxFirmware.SelectedValue as string));
    }

    private void MyFunctions_OnProgress(object sender, int e)
    {
      if (e < 0 || e > 100)
        return;
      this.progressBar.Value = e;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FirmwareEditor));
      this.btnCancel = new Button();
      this.btnUpgrade = new Button();
      this.cboxFirmware = new ComboBox();
      this.rbtnUseFirmwareFromDB = new RadioButton();
      this.rbtnUseFirmwareFromFile = new RadioButton();
      this.btnBrowse = new Button();
      this.progressBar = new ProgressBar();
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Image = (Image) componentResourceManager.GetObject("btnCancel.Image");
      this.btnCancel.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnCancel.ImeMode = ImeMode.NoControl;
      this.btnCancel.Location = new Point(232, 131);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(79, 29);
      this.btnCancel.TabIndex = 21;
      this.btnCancel.Text = "Close";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnUpgrade.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnUpgrade.Image = (Image) componentResourceManager.GetObject("btnUpgrade.Image");
      this.btnUpgrade.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnUpgrade.ImeMode = ImeMode.NoControl;
      this.btnUpgrade.Location = new Point(147, 131);
      this.btnUpgrade.Name = "btnUpgrade";
      this.btnUpgrade.Size = new Size(79, 29);
      this.btnUpgrade.TabIndex = 20;
      this.btnUpgrade.Text = "Upgrade";
      this.btnUpgrade.TextAlign = ContentAlignment.MiddleRight;
      this.btnUpgrade.Click += new System.EventHandler(this.btnUpgrade_Click);
      this.cboxFirmware.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxFirmware.FormattingEnabled = true;
      this.cboxFirmware.Location = new Point(12, 70);
      this.cboxFirmware.Name = "cboxFirmware";
      this.cboxFirmware.Size = new Size(434, 21);
      this.cboxFirmware.TabIndex = 24;
      this.rbtnUseFirmwareFromDB.AutoSize = true;
      this.rbtnUseFirmwareFromDB.Checked = true;
      this.rbtnUseFirmwareFromDB.Location = new Point(16, 47);
      this.rbtnUseFirmwareFromDB.Name = "rbtnUseFirmwareFromDB";
      this.rbtnUseFirmwareFromDB.Size = new Size(156, 17);
      this.rbtnUseFirmwareFromDB.TabIndex = 25;
      this.rbtnUseFirmwareFromDB.TabStop = true;
      this.rbtnUseFirmwareFromDB.Text = "Use firmware from database";
      this.rbtnUseFirmwareFromDB.UseVisualStyleBackColor = true;
      this.rbtnUseFirmwareFromFile.AutoSize = true;
      this.rbtnUseFirmwareFromFile.Location = new Point(197, 47);
      this.rbtnUseFirmwareFromFile.Name = "rbtnUseFirmwareFromFile";
      this.rbtnUseFirmwareFromFile.Size = new Size(125, 17);
      this.rbtnUseFirmwareFromFile.TabIndex = 26;
      this.rbtnUseFirmwareFromFile.Text = "Use firmware from file";
      this.rbtnUseFirmwareFromFile.UseVisualStyleBackColor = true;
      this.rbtnUseFirmwareFromFile.CheckedChanged += new System.EventHandler(this.rbtnUseFirmwareFromFile_CheckedChanged);
      this.btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnBrowse.Location = new Point(370, 44);
      this.btnBrowse.Margin = new Padding(2, 3, 2, 3);
      this.btnBrowse.Name = "btnBrowse";
      this.btnBrowse.Size = new Size(77, 23);
      this.btnBrowse.TabIndex = 27;
      this.btnBrowse.Text = "Browse...";
      this.btnBrowse.UseVisualStyleBackColor = true;
      this.btnBrowse.Visible = false;
      this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
      this.progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.progressBar.Location = new Point(13, 97);
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new Size(434, 18);
      this.progressBar.TabIndex = 1;
      this.progressBar.Visible = false;
      this.zennerCoroprateDesign1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.zennerCoroprateDesign1.Location = new Point(0, 0);
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      this.zennerCoroprateDesign1.Size = new Size(458, 36);
      this.zennerCoroprateDesign1.TabIndex = 2;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(458, 172);
      this.Controls.Add((Control) this.btnBrowse);
      this.Controls.Add((Control) this.progressBar);
      this.Controls.Add((Control) this.rbtnUseFirmwareFromFile);
      this.Controls.Add((Control) this.rbtnUseFirmwareFromDB);
      this.Controls.Add((Control) this.cboxFirmware);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnUpgrade);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FirmwareEditor);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Firmware";
      this.FormClosing += new FormClosingEventHandler(this.FirmwareEditor_FormClosing);
      this.Load += new System.EventHandler(this.FirmwareEditor_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

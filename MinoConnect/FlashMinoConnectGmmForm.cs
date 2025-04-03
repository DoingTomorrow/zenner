// Decompiled with JetBrains decompiler
// Type: MinoConnect.FlashMinoConnectGmmForm
// Assembly: MinoConnect, Version=1.5.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E4D0ECC-943B-4E96-B8E2-CE02CEE9906B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinoConnect.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;

#nullable disable
namespace MinoConnect
{
  public class FlashMinoConnectGmmForm : Form
  {
    private FlashMinoConnect flasher;
    private IContainer components = (IContainer) null;
    private Button btnUpgrade;
    private Button btnClose;
    private Label label1;
    private Label label4;
    private Label label2;
    private TextBox txtPathToFirmware;
    private Button btnBrowse;
    private GroupBox groupBox2;
    private Label label6;
    private Label label8;
    private Label label7;
    private ProgressBar progressBar;

    public string FirmwareInitialDirectory { get; set; }

    public bool IsMinoConnectSuccessfulUpdated { get; private set; }

    public FlashMinoConnectGmmForm(SerialPort port)
    {
      this.InitializeComponent();
      this.flasher = new FlashMinoConnect(port);
      this.flasher.ProgressChanged += new EventHandler<ProgressChangedEventArgs>(this.Flasher_ProgressChanged);
      this.IsMinoConnectSuccessfulUpdated = false;
    }

    private void Flasher_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      this.progressBar.Value = e.ProgressPercentage;
    }

    private void btnUpgrade_Click(object sender, EventArgs e)
    {
      this.IsMinoConnectSuccessfulUpdated = false;
      if (!File.Exists(this.txtPathToFirmware.Text))
        return;
      try
      {
        this.Enabled = false;
        Application.DoEvents();
        Cursor.Current = Cursors.WaitCursor;
        if (this.flasher.Upgrade(this.txtPathToFirmware.Text))
        {
          this.IsMinoConnectSuccessfulUpdated = true;
          int num = (int) MessageBox.Show("Please turn on the MinoConnect!", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          int num1 = (int) MessageBox.Show("Upgrade are failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      catch (IOException ex)
      {
        int num = (int) MessageBox.Show("Can not connect to the MinoConnect! Please try again.", "MinoConnect", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
        this.Enabled = true;
        this.progressBar.Value = this.progressBar.Minimum;
      }
    }

    private void btnClose_Click(object sender, EventArgs e) => this.Close();

    private void btnBrowse_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.RestoreDirectory = true;
      openFileDialog.InitialDirectory = this.FirmwareInitialDirectory;
      openFileDialog.Title = "Please select a file to upgrade";
      int num = (int) openFileDialog.ShowDialog();
      this.txtPathToFirmware.Text = openFileDialog.FileName;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FlashMinoConnectGmmForm));
      this.btnUpgrade = new Button();
      this.btnClose = new Button();
      this.label1 = new Label();
      this.label4 = new Label();
      this.label2 = new Label();
      this.txtPathToFirmware = new TextBox();
      this.btnBrowse = new Button();
      this.groupBox2 = new GroupBox();
      this.label8 = new Label();
      this.label7 = new Label();
      this.progressBar = new ProgressBar();
      this.label6 = new Label();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      this.btnUpgrade.Location = new Point(183, 88);
      this.btnUpgrade.Margin = new Padding(2, 3, 2, 3);
      this.btnUpgrade.Name = "btnUpgrade";
      this.btnUpgrade.Size = new Size(74, 23);
      this.btnUpgrade.TabIndex = 5;
      this.btnUpgrade.Text = "Upgrade";
      this.btnUpgrade.UseVisualStyleBackColor = true;
      this.btnUpgrade.Click += new EventHandler(this.btnUpgrade_Click);
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.Location = new Point(376, 306);
      this.btnClose.Margin = new Padding(2, 3, 2, 3);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(74, 23);
      this.btnClose.TabIndex = 6;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.label1.BackColor = Color.White;
      this.label1.BorderStyle = BorderStyle.FixedSingle;
      this.label1.Dock = DockStyle.Top;
      this.label1.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.ForeColor = Color.FromArgb(64, 64, 64);
      this.label1.Location = new Point(0, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(456, 44);
      this.label1.TabIndex = 5;
      this.label1.Text = "MinoConnect Firmware Upgrade";
      this.label1.TextAlign = ContentAlignment.MiddleCenter;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 68);
      this.label4.Name = "label4";
      this.label4.Size = new Size(336, 13);
      this.label4.TabIndex = 10;
      this.label4.Text = "To upgrade the firmware on your MinoConnect follow the steps below.";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(6, 101);
      this.label2.Name = "label2";
      this.label2.Size = new Size(447, 13);
      this.label2.TabIndex = 12;
      this.label2.Text = "1. Click on the Browse... button to select the firmware file to be uploaded to the MinoConnect.";
      this.txtPathToFirmware.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtPathToFirmware.Location = new Point(9, 118);
      this.txtPathToFirmware.Name = "txtPathToFirmware";
      this.txtPathToFirmware.ReadOnly = true;
      this.txtPathToFirmware.Size = new Size(354, 20);
      this.txtPathToFirmware.TabIndex = 13;
      this.btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnBrowse.Location = new Point(368, 118);
      this.btnBrowse.Margin = new Padding(2, 3, 2, 3);
      this.btnBrowse.Name = "btnBrowse";
      this.btnBrowse.Size = new Size(77, 23);
      this.btnBrowse.TabIndex = 4;
      this.btnBrowse.Text = "Browse...";
      this.btnBrowse.UseVisualStyleBackColor = true;
      this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);
      this.groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox2.Controls.Add((Control) this.label8);
      this.groupBox2.Controls.Add((Control) this.label7);
      this.groupBox2.Controls.Add((Control) this.progressBar);
      this.groupBox2.Controls.Add((Control) this.btnUpgrade);
      this.groupBox2.Location = new Point(9, 173);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(440, 121);
      this.groupBox2.TabIndex = 15;
      this.groupBox2.TabStop = false;
      this.label8.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label8.Location = new Point(83, 15);
      this.label8.Name = "label8";
      this.label8.Size = new Size(274, 19);
      this.label8.TabIndex = 18;
      this.label8.Text = "W A R N I N G";
      this.label8.TextAlign = ContentAlignment.MiddleCenter;
      this.label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(6, 42);
      this.label7.Name = "label7";
      this.label7.Size = new Size(428, 27);
      this.label7.TabIndex = 17;
      this.label7.Text = "Upgrading firmware may take a few minutes.\r\nDo not turn off the power or press the reset button!";
      this.label7.TextAlign = ContentAlignment.MiddleCenter;
      this.progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.progressBar.Location = new Point(6, 72);
      this.progressBar.Name = "progressBar";
      this.progressBar.Size = new Size(428, 10);
      this.progressBar.TabIndex = 1;
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(5, 153);
      this.label6.Name = "label6";
      this.label6.Size = new Size(439, 13);
      this.label6.TabIndex = 16;
      this.label6.Text = "2. Click the Upgrade button to begin the upgrade process. Upgrade must not be interrupted.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(456, 341);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.btnBrowse);
      this.Controls.Add((Control) this.txtPathToFirmware);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnClose);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(2, 3, 2, 3);
      this.Name = nameof (FlashMinoConnectGmmForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.groupBox2.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

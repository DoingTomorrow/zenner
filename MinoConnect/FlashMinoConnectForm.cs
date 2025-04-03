// Decompiled with JetBrains decompiler
// Type: MinoConnect.FlashMinoConnectForm
// Assembly: MinoConnect, Version=1.5.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E4D0ECC-943B-4E96-B8E2-CE02CEE9906B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinoConnect.exe

using Microsoft.Win32;
using MinoConnect.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Security;
using System.Windows.Forms;

#nullable disable
namespace MinoConnect
{
  public class FlashMinoConnectForm : Form
  {
    private FlashMinoConnect flasher;
    private IContainer components = (IContainer) null;
    private Button btnUpgrade;
    private Button btnClose;
    private GroupBox groupBox1;
    private Label label1;
    private Button btnConnect;
    private ComboBox cboxPortNames;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label2;
    private TextBox txtPathToFirmware;
    private Button btnBrowse;
    private GroupBox groupBox2;
    private Label label6;
    private Label label8;
    private Label label7;
    private ProgressBar progressBar;
    private Button btnReloadComPorts;
    private TextBox txtVersion;
    private Button btnExitTransparentMode;

    public FlashMinoConnectForm()
    {
      this.InitializeComponent();
      Version version = Assembly.GetExecutingAssembly().GetName().Version;
      this.Text = string.Format("version {0}.{1}", (object) version.Major, (object) version.Minor);
      this.flasher = new FlashMinoConnect();
      this.flasher.ProgressChanged += new EventHandler<ProgressChangedEventArgs>(this.Flasher_ProgressChanged);
    }

    private void MainForm_Load(object sender, EventArgs e) => this.ReloadComPorts();

    private void Flasher_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      this.progressBar.Value = e.ProgressPercentage;
    }

    private void btnUpgrade_Click(object sender, EventArgs e)
    {
      if (!File.Exists(this.txtPathToFirmware.Text))
        return;
      try
      {
        this.Enabled = false;
        Application.DoEvents();
        Cursor.Current = Cursors.WaitCursor;
        if (this.flasher.Upgrade(this.txtPathToFirmware.Text, this.cboxPortNames.Text))
        {
          int num1 = (int) MessageBox.Show("Please turn on the MinoConnect and wait a few seconds before attempting to connect!", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          int num2 = (int) MessageBox.Show("Upgrade are failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

    private void btnConnect_Click(object sender, EventArgs e)
    {
      try
      {
        this.Enabled = false;
        Application.DoEvents();
        Cursor.Current = Cursors.WaitCursor;
        this.txtVersion.Text = this.flasher.ReadFirmwareVersion(this.cboxPortNames.Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Can not connect to the MinoConnect! Please try again. Error: " + ex.Message, "MinoConnect", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
        this.Enabled = true;
      }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      this.flasher.Dispose();
      this.Close();
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.RestoreDirectory = true;
      openFileDialog.Title = "Please select a file to upgrade";
      int num = (int) openFileDialog.ShowDialog();
      this.txtPathToFirmware.Text = openFileDialog.FileName;
    }

    private void btnReloadComPorts_Click(object sender, EventArgs e)
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        this.btnReloadComPorts.Enabled = false;
        this.ReloadComPorts();
      }
      finally
      {
        Cursor.Current = Cursors.Default;
        this.btnReloadComPorts.Enabled = true;
      }
    }

    private void ReloadComPorts()
    {
      this.cboxPortNames.Items.Clear();
      this.cboxPortNames.Items.AddRange((object[]) FlashMinoConnectForm.GetPortNames());
      if (this.cboxPortNames.Items.Count <= 0)
        return;
      this.cboxPortNames.SelectedIndex = 0;
    }

    private void btnExitTransparentMode_Click(object sender, EventArgs e)
    {
      try
      {
        this.Enabled = false;
        Application.DoEvents();
        Cursor.Current = Cursors.WaitCursor;
        this.txtVersion.Text = this.flasher.ExitFromTransparentMode(this.cboxPortNames.Text);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Can not connect to the MinoConnect! Please try again. Error: " + ex.Message, "MinoConnect", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
        this.Enabled = true;
      }
    }

    public static string[] GetPortNames()
    {
      List<string> portNames = new List<string>();
      FlashMinoConnectForm.SearchRegistryForPortNames("SYSTEM\\CurrentControlSet\\Enum", portNames, new List<string>((IEnumerable<string>) SerialPort.GetPortNames()));
      string[] array = portNames.ToArray();
      Array.Sort<string>(array, (IComparer<string>) new FlashMinoConnectForm.AlphanumComparator());
      return array;
    }

    private static void SearchRegistryForPortNames(
      string startKey,
      List<string> portNames,
      List<string> portNamesToMatch)
    {
      if (portNames.Count >= portNamesToMatch.Count)
        return;
      RegistryKey localMachine = Registry.LocalMachine;
      RegistryKey registryKey;
      try
      {
        registryKey = localMachine.OpenSubKey(startKey);
      }
      catch (SecurityException ex)
      {
        return;
      }
      List<string> stringList = new List<string>((IEnumerable<string>) registryKey.GetSubKeyNames());
      if (stringList.Contains("Device Parameters") && startKey != "SYSTEM\\CurrentControlSet\\Enum")
      {
        object obj = Registry.GetValue("HKEY_LOCAL_MACHINE\\" + startKey + "\\Device Parameters", "PortName", (object) null);
        if (obj == null)
          return;
        string str1 = obj.ToString();
        if (!str1.StartsWith("COM"))
          return;
        bool flag = false;
        foreach (string str2 in portNamesToMatch)
        {
          if (str2.StartsWith(str1.ToString()))
          {
            flag = true;
            break;
          }
        }
        if (!flag || portNames.Contains(str1))
          return;
        portNames.Add(str1);
      }
      else
      {
        foreach (string str in stringList)
          FlashMinoConnectForm.SearchRegistryForPortNames(startKey + "\\" + str, portNames, portNamesToMatch);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FlashMinoConnectForm));
      this.btnUpgrade = new Button();
      this.btnClose = new Button();
      this.groupBox1 = new GroupBox();
      this.txtVersion = new TextBox();
      this.label1 = new Label();
      this.cboxPortNames = new ComboBox();
      this.label3 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.label2 = new Label();
      this.txtPathToFirmware = new TextBox();
      this.btnBrowse = new Button();
      this.groupBox2 = new GroupBox();
      this.label8 = new Label();
      this.label7 = new Label();
      this.progressBar = new ProgressBar();
      this.label6 = new Label();
      this.btnReloadComPorts = new Button();
      this.btnConnect = new Button();
      this.btnExitTransparentMode = new Button();
      this.groupBox1.SuspendLayout();
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
      this.btnClose.Location = new Point(376, 472);
      this.btnClose.Margin = new Padding(2, 3, 2, 3);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(74, 23);
      this.btnClose.TabIndex = 6;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox1.Controls.Add((Control) this.txtVersion);
      this.groupBox1.Location = new Point(5, 150);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(445, 110);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Current firmware version";
      this.txtVersion.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtVersion.Location = new Point(5, 17);
      this.txtVersion.Multiline = true;
      this.txtVersion.Name = "txtVersion";
      this.txtVersion.Size = new Size(436, 87);
      this.txtVersion.TabIndex = 0;
      this.label1.BackColor = System.Drawing.Color.White;
      this.label1.BorderStyle = BorderStyle.FixedSingle;
      this.label1.Dock = DockStyle.Top;
      this.label1.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
      this.label1.Location = new Point(0, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(456, 44);
      this.label1.TabIndex = 5;
      this.label1.Text = "MinoConnect Firmware Upgrade";
      this.label1.TextAlign = ContentAlignment.MiddleCenter;
      this.cboxPortNames.FormattingEnabled = true;
      this.cboxPortNames.Location = new Point(39, 126);
      this.cboxPortNames.Name = "cboxPortNames";
      this.cboxPortNames.Size = new Size(71, 21);
      this.cboxPortNames.TabIndex = 1;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(6, 129);
      this.label3.Name = "label3";
      this.label3.Size = new Size(29, 13);
      this.label3.TabIndex = 8;
      this.label3.Text = "Port:";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 68);
      this.label4.Name = "label4";
      this.label4.Size = new Size(336, 13);
      this.label4.TabIndex = 10;
      this.label4.Text = "To upgrade the firmware on your MinoConnect follow the steps below.";
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(6, 105);
      this.label5.Name = "label5";
      this.label5.Size = new Size(429, 13);
      this.label5.TabIndex = 11;
      this.label5.Text = "1. Select the serial port to which your MinoConnect device is attached and click Connect.";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(6, 267);
      this.label2.Name = "label2";
      this.label2.Size = new Size(447, 13);
      this.label2.TabIndex = 12;
      this.label2.Text = "2. Click on the Browse... button to select the firmware file to be uploaded to the MinoConnect.";
      this.txtPathToFirmware.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtPathToFirmware.Location = new Point(9, 284);
      this.txtPathToFirmware.Name = "txtPathToFirmware";
      this.txtPathToFirmware.ReadOnly = true;
      this.txtPathToFirmware.Size = new Size(354, 20);
      this.txtPathToFirmware.TabIndex = 13;
      this.btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnBrowse.Location = new Point(368, 283);
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
      this.groupBox2.Location = new Point(9, 339);
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
      this.label6.Location = new Point(5, 319);
      this.label6.Name = "label6";
      this.label6.Size = new Size(439, 13);
      this.label6.TabIndex = 16;
      this.label6.Text = "3. Click the Upgrade button to begin the upgrade process. Upgrade must not be interrupted.";
      this.btnReloadComPorts.Image = (Image) Resources.refresh;
      this.btnReloadComPorts.Location = new Point(115, 125);
      this.btnReloadComPorts.Name = "btnReloadComPorts";
      this.btnReloadComPorts.Size = new Size(32, 23);
      this.btnReloadComPorts.TabIndex = 17;
      this.btnReloadComPorts.UseVisualStyleBackColor = true;
      this.btnReloadComPorts.Click += new EventHandler(this.btnReloadComPorts_Click);
      this.btnConnect.Location = new Point(175, 126);
      this.btnConnect.Margin = new Padding(2, 3, 2, 3);
      this.btnConnect.Name = "btnConnect";
      this.btnConnect.Size = new Size(77, 23);
      this.btnConnect.TabIndex = 2;
      this.btnConnect.Text = "Connect";
      this.btnConnect.UseVisualStyleBackColor = true;
      this.btnConnect.Click += new EventHandler(this.btnConnect_Click);
      this.btnExitTransparentMode.Location = new Point(292, 126);
      this.btnExitTransparentMode.Margin = new Padding(2, 3, 2, 3);
      this.btnExitTransparentMode.Name = "btnExitTransparentMode";
      this.btnExitTransparentMode.Size = new Size(157, 23);
      this.btnExitTransparentMode.TabIndex = 18;
      this.btnExitTransparentMode.Text = "Exit from transparent mode";
      this.btnExitTransparentMode.UseVisualStyleBackColor = true;
      this.btnExitTransparentMode.Click += new EventHandler(this.btnExitTransparentMode_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(456, 507);
      this.Controls.Add((Control) this.btnExitTransparentMode);
      this.Controls.Add((Control) this.btnReloadComPorts);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.btnBrowse);
      this.Controls.Add((Control) this.txtPathToFirmware);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.cboxPortNames);
      this.Controls.Add((Control) this.btnConnect);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.btnClose);
      this.FormBorderStyle = FormBorderStyle.Fixed3D;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(2, 3, 2, 3);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FlashMinoConnectForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Load += new EventHandler(this.MainForm_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public class AlphanumComparator : IComparer<string>
    {
      public int Compare(string s1, string s2)
      {
        if (s1 == null || s2 == null)
          return 0;
        int length1 = s1.Length;
        int length2 = s2.Length;
        int index1 = 0;
        int index2 = 0;
        while (index1 < length1 && index2 < length2)
        {
          char c1 = s1[index1];
          char c2 = s2[index2];
          char[] chArray1 = new char[length1];
          int num1 = 0;
          char[] chArray2 = new char[length2];
          int num2 = 0;
          do
          {
            chArray1[num1++] = c1;
            ++index1;
            if (index1 < length1)
              c1 = s1[index1];
            else
              break;
          }
          while (char.IsDigit(c1) == char.IsDigit(chArray1[0]));
          do
          {
            chArray2[num2++] = c2;
            ++index2;
            if (index2 < length2)
              c2 = s2[index2];
            else
              break;
          }
          while (char.IsDigit(c2) == char.IsDigit(chArray2[0]));
          string s = new string(chArray1);
          string str = new string(chArray2);
          int num3 = !char.IsDigit(chArray1[0]) || !char.IsDigit(chArray2[0]) ? s.CompareTo(str) : int.Parse(s).CompareTo(int.Parse(str));
          if (num3 != 0)
            return num3;
        }
        return length1 - length2;
      }
    }
  }
}

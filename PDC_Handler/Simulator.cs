// Decompiled with JetBrains decompiler
// Type: PDC_Handler.Simulator
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using NLog;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace PDC_Handler
{
  public class Simulator : Form
  {
    private static Logger logger = LogManager.GetLogger("LoggerSimulator");
    private PDC_HandlerFunctions handler;
    private bool isStopped;
    private const string TEST_READ = "Test: Read 1000 times meter values of input A";
    private const string TEST_READDEVICE_WITH_SLEEP = "Test: Read device with sleep time 22000";
    private const string TEST_READDEVICE = "Test: Read device without sleep";
    private IContainer components = (IContainer) null;
    private Button btnExecute;
    private TextBox txtStatus;
    private Button btnStop;
    private ComboBox cboxTestList;

    public Simulator() => this.InitializeComponent();

    private void Simulator_Load(object sender, EventArgs e)
    {
      this.cboxTestList.Items.Clear();
      this.cboxTestList.Items.Add((object) "Test: Read 1000 times meter values of input A");
      this.cboxTestList.Items.Add((object) "Test: Read device with sleep time 22000");
      this.cboxTestList.Items.Add((object) "Test: Read device without sleep");
      this.cboxTestList.SelectedIndex = 0;
    }

    internal static void ShowDialog(Form owner, PDC_HandlerFunctions MyFunctions)
    {
      if (MyFunctions == null)
        return;
      using (Simulator simulator = new Simulator())
      {
        simulator.handler = MyFunctions;
        int num = (int) simulator.ShowDialog((IWin32Window) owner);
      }
    }

    private void btnExecute_Click(object sender, EventArgs e)
    {
      if (this.cboxTestList.SelectedItem == null || this.cboxTestList.SelectedItem.ToString() == string.Empty)
        return;
      this.cboxTestList.Enabled = false;
      this.btnExecute.Enabled = false;
      this.btnStop.Enabled = true;
      this.isStopped = false;
      try
      {
        switch (this.cboxTestList.SelectedItem.ToString())
        {
          case "Test: Read 1000 times meter values of input A":
            this.TestRead();
            break;
          case "Test: Read device with sleep time 22000":
            this.TestReadDeviceWithSleep();
            break;
          case "Test: Read device without sleep":
            this.TestReadDeviceWith();
            break;
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, "Simulation failed! " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
        this.btnExecute.Enabled = true;
        this.btnStop.Enabled = false;
        this.cboxTestList.Enabled = true;
      }
    }

    private void btnStop_Click(object sender, EventArgs e) => this.isStopped = true;

    private void TestRead()
    {
      string newLine = Environment.NewLine;
      for (uint index = 1; !this.isStopped && index <= 1000U; ++index)
        this.txtStatus.AppendText("A: " + this.handler.ReadMeterValue((byte) 0).ToString() + newLine);
    }

    private void TestReadDeviceWithSleep()
    {
      string newLine = Environment.NewLine;
      for (uint index = 1; !this.isStopped && index <= 40U; ++index)
      {
        if (this.handler.ReadDevice())
        {
          this.txtStatus.AppendText("OK: 22000 ms sleep time" + newLine);
          Thread.Sleep(22000);
        }
        else
          this.txtStatus.AppendText("Failed no sleep" + newLine);
      }
    }

    private void TestReadDeviceWith()
    {
      string newLine = Environment.NewLine;
      for (uint index = 1; !this.isStopped && index <= 200U; ++index)
      {
        if (this.handler.ReadDevice())
          this.txtStatus.AppendText("OK: " + newLine);
        else
          this.txtStatus.AppendText("Failed " + newLine);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Simulator));
      this.btnExecute = new Button();
      this.txtStatus = new TextBox();
      this.btnStop = new Button();
      this.cboxTestList = new ComboBox();
      this.SuspendLayout();
      this.btnExecute.Location = new Point(12, 12);
      this.btnExecute.Name = "btnExecute";
      this.btnExecute.Size = new Size(75, 23);
      this.btnExecute.TabIndex = 0;
      this.btnExecute.Text = "Execute";
      this.btnExecute.UseVisualStyleBackColor = true;
      this.btnExecute.Click += new EventHandler(this.btnExecute_Click);
      this.txtStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtStatus.Location = new Point(12, 41);
      this.txtStatus.Multiline = true;
      this.txtStatus.Name = "txtStatus";
      this.txtStatus.ScrollBars = ScrollBars.Vertical;
      this.txtStatus.Size = new Size(535, 430);
      this.txtStatus.TabIndex = 1;
      this.btnStop.Enabled = false;
      this.btnStop.Location = new Point(93, 12);
      this.btnStop.Name = "btnStop";
      this.btnStop.Size = new Size(75, 23);
      this.btnStop.TabIndex = 2;
      this.btnStop.Text = "Stop";
      this.btnStop.UseVisualStyleBackColor = true;
      this.btnStop.Click += new EventHandler(this.btnStop_Click);
      this.cboxTestList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboxTestList.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxTestList.FormattingEnabled = true;
      this.cboxTestList.Location = new Point(195, 13);
      this.cboxTestList.Name = "cboxTestList";
      this.cboxTestList.Size = new Size(352, 21);
      this.cboxTestList.TabIndex = 3;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(559, 483);
      this.Controls.Add((Control) this.cboxTestList);
      this.Controls.Add((Control) this.btnStop);
      this.Controls.Add((Control) this.txtStatus);
      this.Controls.Add((Control) this.btnExecute);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (Simulator);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = nameof (Simulator);
      this.Load += new EventHandler(this.Simulator_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

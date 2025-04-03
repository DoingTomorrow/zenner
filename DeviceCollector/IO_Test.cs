// Decompiled with JetBrains decompiler
// Type: DeviceCollector.IO_Test
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using CorporateDesign;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace DeviceCollector
{
  public class IO_Test : Form
  {
    private DeviceCollectorFunctions MyBus;
    private bool IO_Run = false;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label6;
    private CheckBox checkBoxOutputState1;
    private CheckBox checkBoxOutputState2;
    private CheckBox checkBoxInputState1;
    private CheckBox checkBoxInputState2;
    private CheckBox checkBoxOutputSet1;
    private CheckBox checkBoxOutputSet2;
    private CheckBox checkBoxOutputMask1;
    private CheckBox checkBoxOutputMask2;
    private Label label7;
    private Label label8;
    private Label labelAccessCounter;
    private Label labelErrorCounter;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private Button buttonSingleAccess;
    private Button buttonStartCycle;
    private Button buttonStopCycle;
    private Button buttonExit;
    private Label label9;
    private CheckBox checkBoxOutputMask3;
    private CheckBox checkBoxOutputSet3;
    private CheckBox checkBoxInputState3;
    private CheckBox checkBoxOutputState3;
    private System.ComponentModel.Container components = (System.ComponentModel.Container) null;

    public IO_Test(DeviceCollectorFunctions MyBusIn)
    {
      this.InitializeComponent();
      this.MyBus = MyBusIn;
      if (this.MyBus.MyDeviceList.SelectedDevice is Serie2MBus)
        return;
      int num = (int) MessageBox.Show("The selected device has no I/O functions.");
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (IO_Test));
      this.checkBoxOutputState1 = new CheckBox();
      this.checkBoxOutputState2 = new CheckBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.checkBoxInputState1 = new CheckBox();
      this.checkBoxInputState2 = new CheckBox();
      this.checkBoxOutputSet1 = new CheckBox();
      this.checkBoxOutputSet2 = new CheckBox();
      this.checkBoxOutputMask1 = new CheckBox();
      this.label3 = new Label();
      this.checkBoxOutputMask2 = new CheckBox();
      this.label4 = new Label();
      this.label5 = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.label8 = new Label();
      this.labelAccessCounter = new Label();
      this.labelErrorCounter = new Label();
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.buttonSingleAccess = new Button();
      this.buttonStartCycle = new Button();
      this.buttonStopCycle = new Button();
      this.buttonExit = new Button();
      this.label9 = new Label();
      this.checkBoxOutputMask3 = new CheckBox();
      this.checkBoxOutputSet3 = new CheckBox();
      this.checkBoxInputState3 = new CheckBox();
      this.checkBoxOutputState3 = new CheckBox();
      this.SuspendLayout();
      this.checkBoxOutputState1.Enabled = false;
      this.checkBoxOutputState1.Location = new Point(140, 70);
      this.checkBoxOutputState1.Name = "checkBoxOutputState1";
      this.checkBoxOutputState1.Size = new Size(20, 20);
      this.checkBoxOutputState1.TabIndex = 0;
      this.checkBoxOutputState2.Enabled = false;
      this.checkBoxOutputState2.Location = new Point(140, 90);
      this.checkBoxOutputState2.Name = "checkBoxOutputState2";
      this.checkBoxOutputState2.Size = new Size(20, 20);
      this.checkBoxOutputState2.TabIndex = 0;
      this.label1.Location = new Point(110, 50);
      this.label1.Name = "label1";
      this.label1.Size = new Size(80, 20);
      this.label1.TabIndex = 1;
      this.label1.Text = "OutputState";
      this.label1.TextAlign = ContentAlignment.TopCenter;
      this.label2.Location = new Point(190, 50);
      this.label2.Name = "label2";
      this.label2.Size = new Size(80, 20);
      this.label2.TabIndex = 1;
      this.label2.Text = "InputState";
      this.label2.TextAlign = ContentAlignment.TopCenter;
      this.checkBoxInputState1.Enabled = false;
      this.checkBoxInputState1.Location = new Point(220, 70);
      this.checkBoxInputState1.Name = "checkBoxInputState1";
      this.checkBoxInputState1.Size = new Size(20, 20);
      this.checkBoxInputState1.TabIndex = 0;
      this.checkBoxInputState2.Enabled = false;
      this.checkBoxInputState2.Location = new Point(220, 90);
      this.checkBoxInputState2.Name = "checkBoxInputState2";
      this.checkBoxInputState2.Size = new Size(20, 20);
      this.checkBoxInputState2.TabIndex = 0;
      this.checkBoxOutputSet1.Location = new Point(380, 70);
      this.checkBoxOutputSet1.Name = "checkBoxOutputSet1";
      this.checkBoxOutputSet1.Size = new Size(20, 20);
      this.checkBoxOutputSet1.TabIndex = 0;
      this.checkBoxOutputSet2.Location = new Point(380, 90);
      this.checkBoxOutputSet2.Name = "checkBoxOutputSet2";
      this.checkBoxOutputSet2.Size = new Size(20, 20);
      this.checkBoxOutputSet2.TabIndex = 0;
      this.checkBoxOutputMask1.Location = new Point(300, 70);
      this.checkBoxOutputMask1.Name = "checkBoxOutputMask1";
      this.checkBoxOutputMask1.Size = new Size(20, 20);
      this.checkBoxOutputMask1.TabIndex = 0;
      this.label3.Location = new Point(350, 50);
      this.label3.Name = "label3";
      this.label3.Size = new Size(80, 20);
      this.label3.TabIndex = 1;
      this.label3.Text = "OutputSet";
      this.label3.TextAlign = ContentAlignment.TopCenter;
      this.checkBoxOutputMask2.Location = new Point(300, 90);
      this.checkBoxOutputMask2.Name = "checkBoxOutputMask2";
      this.checkBoxOutputMask2.Size = new Size(20, 20);
      this.checkBoxOutputMask2.TabIndex = 0;
      this.label4.Location = new Point(270, 50);
      this.label4.Name = "label4";
      this.label4.Size = new Size(80, 20);
      this.label4.TabIndex = 1;
      this.label4.Text = "OutputMask";
      this.label4.TextAlign = ContentAlignment.TopCenter;
      this.label5.Location = new Point(60, 70);
      this.label5.Name = "label5";
      this.label5.Size = new Size(40, 20);
      this.label5.TabIndex = 1;
      this.label5.Text = "I/O 1:";
      this.label5.TextAlign = ContentAlignment.MiddleRight;
      this.label6.Location = new Point(60, 90);
      this.label6.Name = "label6";
      this.label6.Size = new Size(40, 20);
      this.label6.TabIndex = 1;
      this.label6.Text = "I/O 2:";
      this.label6.TextAlign = ContentAlignment.MiddleRight;
      this.label7.Location = new Point(60, 170);
      this.label7.Name = "label7";
      this.label7.Size = new Size(110, 20);
      this.label7.TabIndex = 1;
      this.label7.Text = "Access counter:";
      this.label7.TextAlign = ContentAlignment.MiddleRight;
      this.label8.Location = new Point(60, 200);
      this.label8.Name = "label8";
      this.label8.Size = new Size(110, 20);
      this.label8.TabIndex = 1;
      this.label8.Text = "Error counter:";
      this.label8.TextAlign = ContentAlignment.MiddleRight;
      this.labelAccessCounter.BorderStyle = BorderStyle.Fixed3D;
      this.labelAccessCounter.Location = new Point(180, 170);
      this.labelAccessCounter.Name = "labelAccessCounter";
      this.labelAccessCounter.Size = new Size(90, 20);
      this.labelAccessCounter.TabIndex = 2;
      this.labelAccessCounter.Text = "0";
      this.labelAccessCounter.TextAlign = ContentAlignment.MiddleLeft;
      this.labelErrorCounter.BorderStyle = BorderStyle.Fixed3D;
      this.labelErrorCounter.Location = new Point(180, 200);
      this.labelErrorCounter.Name = "labelErrorCounter";
      this.labelErrorCounter.Size = new Size(90, 20);
      this.labelErrorCounter.TabIndex = 2;
      this.labelErrorCounter.Text = "0";
      this.labelErrorCounter.TextAlign = ContentAlignment.MiddleLeft;
      this.zennerCoroprateDesign1.Dock = DockStyle.Fill;
      this.zennerCoroprateDesign1.Location = new Point(0, 0);
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      this.zennerCoroprateDesign1.Size = new Size(592, 304);
      this.zennerCoroprateDesign1.TabIndex = 3;
      this.buttonSingleAccess.Location = new Point(460, 150);
      this.buttonSingleAccess.Name = "buttonSingleAccess";
      this.buttonSingleAccess.Size = new Size(110, 30);
      this.buttonSingleAccess.TabIndex = 4;
      this.buttonSingleAccess.Text = "Single access";
      this.buttonSingleAccess.Click += new EventHandler(this.buttonSingleAccess_Click);
      this.buttonStartCycle.Location = new Point(460, 60);
      this.buttonStartCycle.Name = "buttonStartCycle";
      this.buttonStartCycle.Size = new Size(110, 30);
      this.buttonStartCycle.TabIndex = 4;
      this.buttonStartCycle.Text = "Start cycle";
      this.buttonStartCycle.Click += new EventHandler(this.buttonStartCycle_Click);
      this.buttonStopCycle.Location = new Point(460, 100);
      this.buttonStopCycle.Name = "buttonStopCycle";
      this.buttonStopCycle.Size = new Size(110, 30);
      this.buttonStopCycle.TabIndex = 4;
      this.buttonStopCycle.Text = "Stop cycle";
      this.buttonStopCycle.Click += new EventHandler(this.buttonStopCycle_Click);
      this.buttonExit.Location = new Point(460, 190);
      this.buttonExit.Name = "buttonExit";
      this.buttonExit.Size = new Size(110, 30);
      this.buttonExit.TabIndex = 4;
      this.buttonExit.Text = "Exit";
      this.buttonExit.Click += new EventHandler(this.buttonExit_Click);
      this.label9.Location = new Point(60, 110);
      this.label9.Name = "label9";
      this.label9.Size = new Size(40, 20);
      this.label9.TabIndex = 1;
      this.label9.Text = "I/O 3:";
      this.label9.TextAlign = ContentAlignment.MiddleRight;
      this.checkBoxOutputMask3.Location = new Point(300, 110);
      this.checkBoxOutputMask3.Name = "checkBoxOutputMask3";
      this.checkBoxOutputMask3.Size = new Size(20, 20);
      this.checkBoxOutputMask3.TabIndex = 0;
      this.checkBoxOutputSet3.Location = new Point(380, 110);
      this.checkBoxOutputSet3.Name = "checkBoxOutputSet3";
      this.checkBoxOutputSet3.Size = new Size(20, 20);
      this.checkBoxOutputSet3.TabIndex = 0;
      this.checkBoxInputState3.Enabled = false;
      this.checkBoxInputState3.Location = new Point(220, 110);
      this.checkBoxInputState3.Name = "checkBoxInputState3";
      this.checkBoxInputState3.Size = new Size(20, 20);
      this.checkBoxInputState3.TabIndex = 0;
      this.checkBoxOutputState3.Enabled = false;
      this.checkBoxOutputState3.Location = new Point(140, 110);
      this.checkBoxOutputState3.Name = "checkBoxOutputState3";
      this.checkBoxOutputState3.Size = new Size(20, 20);
      this.checkBoxOutputState3.TabIndex = 0;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(592, 304);
      this.Controls.Add((Control) this.buttonExit);
      this.Controls.Add((Control) this.buttonStopCycle);
      this.Controls.Add((Control) this.buttonStartCycle);
      this.Controls.Add((Control) this.buttonSingleAccess);
      this.Controls.Add((Control) this.labelAccessCounter);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.checkBoxOutputState1);
      this.Controls.Add((Control) this.checkBoxOutputState3);
      this.Controls.Add((Control) this.checkBoxOutputState2);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.checkBoxInputState1);
      this.Controls.Add((Control) this.checkBoxInputState3);
      this.Controls.Add((Control) this.checkBoxInputState2);
      this.Controls.Add((Control) this.checkBoxOutputSet1);
      this.Controls.Add((Control) this.checkBoxOutputSet3);
      this.Controls.Add((Control) this.checkBoxOutputSet2);
      this.Controls.Add((Control) this.checkBoxOutputMask1);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.checkBoxOutputMask3);
      this.Controls.Add((Control) this.checkBoxOutputMask2);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.labelErrorCounter);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (IO_Test);
      this.Text = nameof (IO_Test);
      this.ResumeLayout(false);
    }

    private void IO_Cycle()
    {
      this.IO_Run = true;
      this.Cursor = Cursors.WaitCursor;
      while (this.IO_Run)
        this.WorkIO();
      this.Cursor = Cursors.Default;
    }

    private bool WorkIO()
    {
      uint NewOutputMask = 0;
      uint NewOutputState = 0;
      uint OldOutputState = 0;
      uint OldInputState = 0;
      if (this.checkBoxOutputMask1.Checked)
        NewOutputMask |= 1U;
      if (this.checkBoxOutputMask2.Checked)
        NewOutputMask |= 2U;
      if (this.checkBoxOutputMask3.Checked)
        NewOutputMask |= 4U;
      if (this.checkBoxOutputSet1.Checked)
        NewOutputState |= 1U;
      if (this.checkBoxOutputSet2.Checked)
        NewOutputState |= 2U;
      if (this.checkBoxOutputSet3.Checked)
        NewOutputState |= 4U;
      if (this.MyBus.DigitalInputsAndOutputs(NewOutputMask, NewOutputState, ref OldOutputState, ref OldInputState))
      {
        this.checkBoxOutputState1.Checked = (OldOutputState & 1U) > 0U;
        this.checkBoxOutputState2.Checked = (OldOutputState & 2U) > 0U;
        this.checkBoxOutputState3.Checked = (OldOutputState & 4U) > 0U;
        this.checkBoxInputState1.Checked = (OldInputState & 1U) > 0U;
        this.checkBoxInputState2.Checked = (OldInputState & 2U) > 0U;
        this.checkBoxInputState3.Checked = (OldInputState & 4U) > 0U;
        this.labelAccessCounter.Text = (int.Parse(this.labelAccessCounter.Text) + 1).ToString();
        return false;
      }
      this.labelAccessCounter.Text = (int.Parse(this.labelAccessCounter.Text) + 1).ToString();
      this.labelErrorCounter.Text = (int.Parse(this.labelErrorCounter.Text) + 1).ToString();
      return false;
    }

    private void buttonSingleAccess_Click(object sender, EventArgs e)
    {
      this.MyBus.BreakRequest = false;
      this.WorkIO();
      this.WorkIO();
    }

    private void buttonStartCycle_Click(object sender, EventArgs e) => this.IO_Cycle();

    private void buttonStopCycle_Click(object sender, EventArgs e) => this.IO_Run = false;

    private void buttonExit_Click(object sender, EventArgs e) => this.Hide();
  }
}

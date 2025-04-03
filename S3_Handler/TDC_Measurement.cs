// Decompiled with JetBrains decompiler
// Type: S3_Handler.TDC_Measurement
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using DeviceCollector;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public class TDC_Measurement : Form
  {
    private const string startText = "Start flying test";
    private const string stopText = "Stopp flying test";
    private S3_HandlerFunctions MyFunctions;
    private bool stopRunningTest;
    private DateTime StartTime;
    private DateTime StopTime;
    private string RunState = "";
    private int FlyingTestLoops;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private GroupBox groupBoxFlyingTestCommands;
    private Button buttonTestEndMesurement;
    private Button buttonTestDone;
    private Button buttonTestStart;
    private Button buttonTestStartMesurement;
    private SplitContainer splitContainer1;
    private TextBox textBoxInfos;
    private DataGridView dataGridViewInfos;
    private Button buttonTestReadVolume;
    private GroupBox groupBoxOpticalInterface;
    private Button buttonOptoActivateStatic;
    private Button buttonOptoReleaseStatic;
    private GroupBox groupBoxFlyingTestSimulation;
    private Label label1;
    private Button buttonRunTest;
    private TextBox textBoxTestSeconds;
    private CheckBox checkBoxShowState;
    private Button buttonClearWindow;
    private Button buttonLCD;
    private GroupBox groupBoxTDC;
    private CheckBox checkBoxRepeatFlyingTest;
    private TextBox textBoxWaitSeconds;
    private Label label3;
    private Button btnTDCmap;
    private Button buttonCheckIUF;
    private Button buttonTdcInternals;
    private GroupBox groupBoxTDC_Calibration;
    private TextBox textBoxErrorInPercent;
    private Label label2;
    private Button buttonRunCalibration;
    private GroupBox groupBoxVMCP;
    private Button buttonRead_VMCP_State;
    private TextBox textBoxStartWaitSeconds;
    private Label label4;
    private Button buttonReadVmcpVolume;
    private TextBox textBoxStatus;

    public TDC_Measurement(S3_HandlerFunctions MyFunctions)
    {
      this.MyFunctions = MyFunctions;
      this.InitializeComponent();
      VolumeGraphCalibration.GarantObjectAvailable(MyFunctions);
      if (this.MyFunctions.IsHandlerCompleteEnabled())
        this.buttonTdcInternals.Visible = true;
      if (this.MyFunctions.MyMeters.ConnectedMeter == null)
      {
        this.groupBoxTDC_Calibration.Enabled = false;
        this.checkBoxShowState.Enabled = false;
      }
      this.InsertTdcStateGridRows();
      this.buttonRunTest.Text = "Start flying test";
    }

    private void buttonTestStart_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.TestStart();
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private bool TestStart()
    {
      if (!this.checkBoxRepeatFlyingTest.Checked)
        this.textBoxInfos.AppendText("Send test start: ");
      bool ok = this.MyFunctions.SetTestMode(DeviceTestMode.Volume_Tests);
      if (!this.checkBoxRepeatFlyingTest.Checked)
        this.RegisterOkErr(ok);
      return ok;
    }

    private void buttonTestStartMesurement_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.MyFunctions.MyMeters.MeterJobStart(MeterJob.LocalTestFunction);
      this.TestStartMesurement();
      this.MyFunctions.MyMeters.MeterJobFinished(MeterJob.LocalTestFunction);
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private bool TestStartMesurement()
    {
      if (!this.checkBoxRepeatFlyingTest.Checked)
        this.textBoxInfos.AppendText(Environment.NewLine + "*********** Diverter start signal simulated ************" + Environment.NewLine);
      this.StartTime = DateTime.Now;
      return this.MyFunctions.MyCommands.FlyingTestStart();
    }

    private void ReadAndShowStatus()
    {
      if (!this.checkBoxShowState.Checked)
        return;
      try
      {
        this.dataGridViewInfos.Rows.Add(new object[1]
        {
          this.dataGridViewInfos.RowTemplate.Clone()
        });
        int index = this.dataGridViewInfos.Rows.Count - 1;
        if (this.MyFunctions.MyMeters.ConnectedMeter.MyIdentification.IsUltrasonic)
        {
          FlyingTestDiagnostic flyingTestDiagnostic = new FlyingTestDiagnostic(this.MyFunctions);
          flyingTestDiagnostic.ReadTdcStatusData();
          this.textBoxInfos.AppendText(flyingTestDiagnostic.GetTdcStatusReport());
        }
        FlyingTestData flyingTestData = this.MyFunctions.Read_FlyingTestDiagnostic();
        this.dataGridViewInfos.Rows[index].Cells["TimeStart_4ms"].Value = (object) flyingTestData.TimeStart_4ms.ToString();
        this.dataGridViewInfos.Rows[index].Cells["TimeStop_4ms"].Value = (object) flyingTestData.TimeStop_4ms.ToString();
        this.dataGridViewInfos.Rows[index].Cells["OutOfCycleTime"].Value = (object) flyingTestData.TimeInStartPlusStopCycle.ToString();
        this.dataGridViewInfos.Rows[index].Cells["InCycleTime"].Value = (object) flyingTestData.TimeInCompleteCycles.ToString();
        this.dataGridViewInfos.Rows[index].Cells["TimeStart_s"].Value = (object) flyingTestData.TimeStart_s.ToString();
        this.dataGridViewInfos.Rows[index].Cells["TimeStop_s"].Value = (object) flyingTestData.TimeStop_s.ToString();
        this.dataGridViewInfos.Rows[index].Cells["FlowStart"].Value = (object) flyingTestData.FlowStart.ToString();
        this.dataGridViewInfos.Rows[index].Cells["FlowStop"].Value = (object) flyingTestData.FlowStop.ToString();
        this.dataGridViewInfos.Rows[index].Cells["VolComplete"].Value = (object) flyingTestData.VolComplete.ToString();
        this.dataGridViewInfos.Rows[index].Cells["VolTimeVolUsed"].Value = (object) flyingTestData.VolTimeVolUsed.ToString();
        if (!this.checkBoxRepeatFlyingTest.Checked)
          this.textBoxInfos.AppendText(flyingTestData.ToString());
        else
          this.textBoxInfos.AppendText(flyingTestData.ToString("l"));
        if (this.MyFunctions.MyMeters.ConnectedMeter.MyIdentification.IsWR4)
        {
          if (!this.checkBoxRepeatFlyingTest.Checked)
            this.textBoxInfos.AppendText(this.MyFunctions.Read_VMCP_Diagnostic().ToString());
          else
            this.textBoxInfos.AppendText(this.MyFunctions.Read_VMCP_Diagnostic().ToString("l"));
        }
        this.textBoxInfos.AppendText(Environment.NewLine);
      }
      catch (Exception ex)
      {
        this.textBoxInfos.AppendText("Read statuserror exception" + Environment.NewLine + ex.ToString() + Environment.NewLine);
      }
    }

    private void buttonTestEndMesurement_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.MyFunctions.MyMeters.MeterJobStart(MeterJob.LocalTestFunction);
      this.TestEndMesurement();
      this.MyFunctions.MyMeters.MeterJobFinished(MeterJob.LocalTestFunction);
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private bool TestEndMesurement()
    {
      this.StopTime = DateTime.Now;
      bool flag = this.MyFunctions.MyCommands.FlyingTestStop();
      TimeSpan timeSpan = this.StopTime.Subtract(this.StartTime);
      if (!this.checkBoxRepeatFlyingTest.Checked)
      {
        this.textBoxInfos.AppendText(Environment.NewLine + "*********** Diverter stop signal simulated ************" + Environment.NewLine);
        this.textBoxInfos.AppendText("Test time: " + timeSpan.TotalSeconds.ToString() + Environment.NewLine);
      }
      return flag;
    }

    private void buttonTestReadVolume_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.TestReadVolume();
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private bool TestReadVolume()
    {
      if (!this.checkBoxRepeatFlyingTest.Checked)
        this.textBoxInfos.AppendText(Environment.NewLine + "*********** Read flying test volume ************" + Environment.NewLine);
      this.ReadAndShowStatus();
      float num;
      bool ok = this.MyFunctions.volumeGraphCalibration == null ? this.MyFunctions.MyCommands.FlyingTestReadVolume(out num, out MBusDeviceState _) : this.MyFunctions.GetTestVolume(out num);
      if (!this.checkBoxRepeatFlyingTest.Checked)
      {
        this.RegisterOkErr(ok);
        if (ok)
          this.textBoxInfos.AppendText("Test volume: " + num.ToString() + Environment.NewLine);
      }
      return ok;
    }

    private void buttonTestDone_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.TestDone();
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private bool TestDone()
    {
      if (!this.checkBoxRepeatFlyingTest.Checked)
        this.textBoxInfos.AppendText("Send test done: ");
      bool ok = this.MyFunctions.SetTestMode(DeviceTestMode.Off);
      if (!this.checkBoxRepeatFlyingTest.Checked)
        this.RegisterOkErr(ok);
      return ok;
    }

    private void buttonRunCalibration_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      float result;
      if (float.TryParse(this.textBoxErrorInPercent.Text, out result))
      {
        this.textBoxInfos.AppendText("Adjust error in %: " + result.ToString() + Environment.NewLine);
        if (this.MyFunctions.AdjustVolumeFactor(result))
          this.textBoxInfos.AppendText("Adjust done" + Environment.NewLine);
        else
          this.textBoxInfos.AppendText("Adjust error" + Environment.NewLine);
      }
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonOptoActivateStatic_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.OptoActivateStatic();
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private bool OptoActivateStatic()
    {
      this.textBoxInfos.AppendText("Opto activate static: ");
      bool ok = this.MyFunctions.SetOpticalInterfaceMode(OpticalInterfaceMode.Static);
      this.RegisterOkErr(ok);
      return ok;
    }

    private void buttonOptoReleaseStatic_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.OptoReleaseStatic();
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private bool OptoReleaseStatic()
    {
      this.textBoxInfos.AppendText("Opto release static: ");
      bool ok = this.MyFunctions.SetOpticalInterfaceMode(OpticalInterfaceMode.Wakeup);
      this.RegisterOkErr(ok);
      return ok;
    }

    private void RegisterOkErr(bool ok)
    {
      if (ok)
        this.textBoxInfos.AppendText("OK" + Environment.NewLine);
      else
        this.textBoxInfos.AppendText("Error" + Environment.NewLine);
    }

    private void buttonRunTest_Click(object sender, EventArgs e)
    {
      if (this.buttonRunTest.Text == "Stopp flying test")
      {
        this.buttonRunTest.Text = "Flying test stoping ...";
        this.buttonRunTest.BackColor = Color.Purple;
        this.stopRunningTest = true;
      }
      else if (this.buttonRunTest.Text != "Start flying test")
      {
        this.buttonRunTest.BackColor = Color.Purple;
        this.stopRunningTest = true;
      }
      else
      {
        this.buttonRunTest.Text = "Stopp flying test";
        this.buttonRunTest.BackColor = Color.LightYellow;
        this.stopRunningTest = false;
        ZR_ClassLibMessages.ClearErrors();
        this.MyFunctions.MyMeters.MeterJobStart(MeterJob.LocalTestFunction);
        try
        {
          int result1 = 0;
          if (!int.TryParse(this.textBoxStartWaitSeconds.Text, out result1) || result1 <= 1)
          {
            this.textBoxTestSeconds.Text = "3";
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal start wait time");
          }
          else
          {
            int result2 = 0;
            if (!int.TryParse(this.textBoxTestSeconds.Text, out result2) || result2 <= 1)
            {
              this.textBoxTestSeconds.Text = "30";
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal test time");
            }
            else
            {
              int result3 = 0;
              if (this.checkBoxRepeatFlyingTest.Checked && (!int.TryParse(this.textBoxWaitSeconds.Text, out result3) || result3 < 0))
              {
                this.textBoxTestSeconds.Text = "3";
                ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal wait time");
              }
              else
              {
                if (!this.OptoActivateStatic())
                {
                  ZR_ClassLibMessages.AddErrorDescription("Error on set optical interface static");
                }
                else
                {
                  if (this.checkBoxRepeatFlyingTest.Checked)
                  {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine(" ***** Result list apprevations *****");
                    stringBuilder.Append(FlyingTestData.GetListApprevations());
                    stringBuilder.Append(VMCP_Diagnostic.GetListApprevations());
                    stringBuilder.AppendLine();
                    stringBuilder.Append(FlyingTestData.GetListHeader());
                    stringBuilder.Append(VMCP_Diagnostic.GetListHeader());
                    stringBuilder.AppendLine();
                    this.textBoxInfos.AppendText(stringBuilder.ToString());
                    this.InsertTdcStateGridRows();
                  }
                  this.RunState = "";
                  this.FlyingTestLoops = 0;
                  while (!this.stopRunningTest)
                  {
                    Thread.Sleep(500);
                    if (!this.TestStart())
                    {
                      ZR_ClassLibMessages.AddErrorDescription("Error on test start");
                      break;
                    }
                    this.WaitSeconds("TMode=1", result1);
                    if (!this.stopRunningTest)
                    {
                      if (!this.TestStartMesurement())
                      {
                        ZR_ClassLibMessages.AddErrorDescription("Error on start mesurement");
                        break;
                      }
                      this.WaitSeconds("FT=1", result2);
                      if (!this.stopRunningTest)
                      {
                        if (!this.TestEndMesurement())
                        {
                          ZR_ClassLibMessages.AddErrorDescription("Error on stop mesurement");
                          break;
                        }
                        this.WaitSeconds("FT=0", 3);
                        if (!this.stopRunningTest)
                        {
                          if (!this.TestReadVolume())
                          {
                            ZR_ClassLibMessages.AddErrorDescription("Error on read volume");
                            break;
                          }
                          if (!this.TestDone())
                          {
                            ZR_ClassLibMessages.AddErrorDescription("Error on test done");
                            break;
                          }
                          if (this.checkBoxRepeatFlyingTest.Checked)
                          {
                            this.RunState = "";
                            this.WaitSeconds("TMode=0", result3);
                            if (!this.stopRunningTest)
                              ++this.FlyingTestLoops;
                            else
                              break;
                          }
                          else
                            break;
                        }
                        else
                          break;
                      }
                      else
                        break;
                    }
                    else
                      break;
                  }
                }
                Thread.Sleep(500);
                if (!this.OptoReleaseStatic())
                  ZR_ClassLibMessages.AddWarning("Error on reset optical interface from static mode.");
              }
            }
          }
          this.MyFunctions.MyMeters.MeterJobFinished(MeterJob.LocalTestFunction);
          ZR_ClassLibMessages.ShowAndClearErrors();
        }
        catch (Exception ex)
        {
          ZR_ClassLibMessages.AddException(ex);
          ZR_ClassLibMessages.ShowAndClearErrors();
          this.TestDone();
          Thread.Sleep(500);
          this.OptoReleaseStatic();
        }
        this.buttonRunTest.Text = "Start flying test";
        this.buttonRunTest.BackColor = Control.DefaultBackColor;
        this.textBoxStatus.Clear();
      }
    }

    private void InsertTdcStateGridRows()
    {
      this.dataGridViewInfos.Rows.Clear();
      this.dataGridViewInfos.Columns.Clear();
      string[] strArray = new string[10]
      {
        "TimeStart_4ms",
        "TimeStop_4ms",
        "OutOfCycleTime",
        "InCycleTime",
        "TimeStart_s",
        "TimeStop_s",
        "FlowStart",
        "FlowStop",
        "VolComplete",
        "VolTimeVolUsed"
      };
      foreach (string str in strArray)
      {
        DataGridViewColumn dataGridViewColumn = new DataGridViewColumn((DataGridViewCell) new DataGridViewTextBoxCell())
        {
          HeaderText = str.Trim()
        };
        dataGridViewColumn.Name = dataGridViewColumn.HeaderText;
        this.dataGridViewInfos.Columns.Add(dataGridViewColumn);
      }
    }

    private void WaitSeconds(string info, int Seconds)
    {
      for (int index = Seconds; index > 0 && !this.stopRunningTest; --index)
      {
        if (this.MyFunctions.MyMeters.ConnectedMeter.MyIdentification.IsWR4)
          this.textBoxStatus.Text = this.MyFunctions.Read_VMCP_VolumeString() + "; " + this.RunState + ": Waiting " + index.ToString() + " seconds";
        else
          this.textBoxStatus.Text = this.RunState + ": Waiting " + index.ToString() + " seconds";
        Application.DoEvents();
        Thread.Sleep(1000);
      }
    }

    private void buttonTdcInternals_Click(object sender, EventArgs e)
    {
      int num = (int) new TDC_Internals(this.MyFunctions).ShowDialog();
    }

    private void btnTDCmap_Click(object sender, EventArgs e)
    {
      int num = (int) new TDC_Map().ShowDialog();
    }

    private void buttonCheckIUF_Click(object sender, EventArgs e)
    {
      this.textBoxInfos.Text = "Checking ultrasonic volume meter ...";
      this.textBoxInfos.AppendText(Environment.NewLine);
      IufCheckResults checkResultes;
      if (this.MyFunctions.IsUltrasonicVolumeMeterReady(out checkResultes))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Puls width up: " + checkResultes.FirstPulsWidthUp.ToString());
        stringBuilder.AppendLine("Puls width down: " + checkResultes.FirstPulsWidthDown.ToString());
        stringBuilder.AppendLine("Time counter up: " + checkResultes.TimeCounterUp.ToString());
        stringBuilder.AppendLine("Time counter down: " + checkResultes.TimeCounterDown.ToString());
        this.textBoxInfos.AppendText("... ok ...");
        this.textBoxInfos.AppendText(Environment.NewLine);
        this.textBoxInfos.AppendText(Environment.NewLine);
        this.textBoxInfos.AppendText(stringBuilder.ToString());
      }
      else
        this.textBoxInfos.AppendText("!!! ERROR !!!");
    }

    private void buttonClearWindow_Click(object sender, EventArgs e)
    {
      this.textBoxInfos.Clear();
      this.dataGridViewInfos.Rows.Clear();
    }

    private void buttonLCD_Click(object sender, EventArgs e)
    {
      this.textBoxInfos.AppendText("LCD = " + this.MyFunctions.GetCheckupState().ToString("x02") + Environment.NewLine);
    }

    private void buttonRead_VMCP_State_Click(object sender, EventArgs e)
    {
      this.textBoxInfos.AppendText(this.MyFunctions.Read_VMCP_Diagnostic().ToString());
    }

    private void buttonReadVmcpVolume_Click(object sender, EventArgs e)
    {
      this.textBoxInfos.AppendText(Environment.NewLine + this.MyFunctions.Read_VMCP_VolumeString());
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupBoxFlyingTestCommands = new GroupBox();
      this.buttonLCD = new Button();
      this.checkBoxShowState = new CheckBox();
      this.buttonTestEndMesurement = new Button();
      this.buttonTestReadVolume = new Button();
      this.buttonTestDone = new Button();
      this.buttonTestStart = new Button();
      this.buttonTestStartMesurement = new Button();
      this.splitContainer1 = new SplitContainer();
      this.textBoxInfos = new TextBox();
      this.textBoxStatus = new TextBox();
      this.dataGridViewInfos = new DataGridView();
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.groupBoxOpticalInterface = new GroupBox();
      this.buttonOptoActivateStatic = new Button();
      this.buttonOptoReleaseStatic = new Button();
      this.groupBoxFlyingTestSimulation = new GroupBox();
      this.textBoxStartWaitSeconds = new TextBox();
      this.label4 = new Label();
      this.checkBoxRepeatFlyingTest = new CheckBox();
      this.textBoxWaitSeconds = new TextBox();
      this.label3 = new Label();
      this.textBoxTestSeconds = new TextBox();
      this.label1 = new Label();
      this.buttonRunTest = new Button();
      this.buttonClearWindow = new Button();
      this.groupBoxTDC = new GroupBox();
      this.btnTDCmap = new Button();
      this.buttonCheckIUF = new Button();
      this.buttonTdcInternals = new Button();
      this.groupBoxTDC_Calibration = new GroupBox();
      this.textBoxErrorInPercent = new TextBox();
      this.label2 = new Label();
      this.buttonRunCalibration = new Button();
      this.groupBoxVMCP = new GroupBox();
      this.buttonReadVmcpVolume = new Button();
      this.buttonRead_VMCP_State = new Button();
      this.groupBoxFlyingTestCommands.SuspendLayout();
      this.splitContainer1.BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      ((ISupportInitialize) this.dataGridViewInfos).BeginInit();
      this.groupBoxOpticalInterface.SuspendLayout();
      this.groupBoxFlyingTestSimulation.SuspendLayout();
      this.groupBoxTDC.SuspendLayout();
      this.groupBoxTDC_Calibration.SuspendLayout();
      this.groupBoxVMCP.SuspendLayout();
      this.SuspendLayout();
      this.groupBoxFlyingTestCommands.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.groupBoxFlyingTestCommands.Controls.Add((Control) this.buttonLCD);
      this.groupBoxFlyingTestCommands.Controls.Add((Control) this.checkBoxShowState);
      this.groupBoxFlyingTestCommands.Controls.Add((Control) this.buttonTestEndMesurement);
      this.groupBoxFlyingTestCommands.Controls.Add((Control) this.buttonTestReadVolume);
      this.groupBoxFlyingTestCommands.Controls.Add((Control) this.buttonTestDone);
      this.groupBoxFlyingTestCommands.Controls.Add((Control) this.buttonTestStart);
      this.groupBoxFlyingTestCommands.Controls.Add((Control) this.buttonTestStartMesurement);
      this.groupBoxFlyingTestCommands.Location = new Point(923, 108);
      this.groupBoxFlyingTestCommands.Name = "groupBoxFlyingTestCommands";
      this.groupBoxFlyingTestCommands.Size = new Size(175, 282);
      this.groupBoxFlyingTestCommands.TabIndex = 24;
      this.groupBoxFlyingTestCommands.TabStop = false;
      this.groupBoxFlyingTestCommands.Text = "Flying test commands";
      this.buttonLCD.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonLCD.Location = new Point(121, 46);
      this.buttonLCD.Name = "buttonLCD";
      this.buttonLCD.Size = new Size(48, 22);
      this.buttonLCD.TabIndex = 20;
      this.buttonLCD.Text = "LCD";
      this.buttonLCD.UseVisualStyleBackColor = true;
      this.buttonLCD.Click += new System.EventHandler(this.buttonLCD_Click);
      this.checkBoxShowState.AutoSize = true;
      this.checkBoxShowState.Location = new Point(9, 20);
      this.checkBoxShowState.Name = "checkBoxShowState";
      this.checkBoxShowState.Size = new Size(152, 17);
      this.checkBoxShowState.TabIndex = 19;
      this.checkBoxShowState.Text = "Read and show diagnostic";
      this.checkBoxShowState.UseVisualStyleBackColor = true;
      this.buttonTestEndMesurement.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonTestEndMesurement.Location = new Point(7, 99);
      this.buttonTestEndMesurement.Name = "buttonTestEndMesurement";
      this.buttonTestEndMesurement.Size = new Size(162, 23);
      this.buttonTestEndMesurement.TabIndex = 18;
      this.buttonTestEndMesurement.Text = "Send finish test mesurement";
      this.buttonTestEndMesurement.UseVisualStyleBackColor = true;
      this.buttonTestEndMesurement.Click += new System.EventHandler(this.buttonTestEndMesurement_Click);
      this.buttonTestReadVolume.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonTestReadVolume.Location = new Point(7, 128);
      this.buttonTestReadVolume.Name = "buttonTestReadVolume";
      this.buttonTestReadVolume.Size = new Size(162, 23);
      this.buttonTestReadVolume.TabIndex = 18;
      this.buttonTestReadVolume.Text = "Send read volume";
      this.buttonTestReadVolume.UseVisualStyleBackColor = true;
      this.buttonTestReadVolume.Click += new System.EventHandler(this.buttonTestReadVolume_Click);
      this.buttonTestDone.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonTestDone.Location = new Point(7, 157);
      this.buttonTestDone.Name = "buttonTestDone";
      this.buttonTestDone.Size = new Size(162, 23);
      this.buttonTestDone.TabIndex = 18;
      this.buttonTestDone.Text = "Send test done";
      this.buttonTestDone.UseVisualStyleBackColor = true;
      this.buttonTestDone.Click += new System.EventHandler(this.buttonTestDone_Click);
      this.buttonTestStart.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonTestStart.Location = new Point(7, 45);
      this.buttonTestStart.Name = "buttonTestStart";
      this.buttonTestStart.Size = new Size(108, 23);
      this.buttonTestStart.TabIndex = 18;
      this.buttonTestStart.Text = "Send test start";
      this.buttonTestStart.UseVisualStyleBackColor = true;
      this.buttonTestStart.Click += new System.EventHandler(this.buttonTestStart_Click);
      this.buttonTestStartMesurement.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonTestStartMesurement.Location = new Point(7, 70);
      this.buttonTestStartMesurement.Name = "buttonTestStartMesurement";
      this.buttonTestStartMesurement.Size = new Size(162, 23);
      this.buttonTestStartMesurement.TabIndex = 18;
      this.buttonTestStartMesurement.Text = "Send start test mesurement";
      this.buttonTestStartMesurement.UseVisualStyleBackColor = true;
      this.buttonTestStartMesurement.Click += new System.EventHandler(this.buttonTestStartMesurement_Click);
      this.splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.splitContainer1.Location = new Point(0, 46);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Orientation = Orientation.Horizontal;
      this.splitContainer1.Panel1.Controls.Add((Control) this.textBoxInfos);
      this.splitContainer1.Panel2.Controls.Add((Control) this.textBoxStatus);
      this.splitContainer1.Panel2.Controls.Add((Control) this.dataGridViewInfos);
      this.splitContainer1.Size = new Size(917, 623);
      this.splitContainer1.SplitterDistance = 422;
      this.splitContainer1.TabIndex = 22;
      this.textBoxInfos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxInfos.Font = new Font("Courier New", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.textBoxInfos.Location = new Point(10, 3);
      this.textBoxInfos.Multiline = true;
      this.textBoxInfos.Name = "textBoxInfos";
      this.textBoxInfos.ScrollBars = ScrollBars.Both;
      this.textBoxInfos.Size = new Size(904, 416);
      this.textBoxInfos.TabIndex = 0;
      this.textBoxStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxStatus.Location = new Point(13, 168);
      this.textBoxStatus.Name = "textBoxStatus";
      this.textBoxStatus.Size = new Size(901, 20);
      this.textBoxStatus.TabIndex = 1;
      this.dataGridViewInfos.AllowUserToAddRows = false;
      this.dataGridViewInfos.AllowUserToDeleteRows = false;
      this.dataGridViewInfos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.dataGridViewInfos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      this.dataGridViewInfos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewInfos.Location = new Point(10, 3);
      this.dataGridViewInfos.Name = "dataGridViewInfos";
      this.dataGridViewInfos.Size = new Size(907, 159);
      this.dataGridViewInfos.TabIndex = 0;
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(1278, 41);
      this.zennerCoroprateDesign2.TabIndex = 18;
      this.groupBoxOpticalInterface.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.groupBoxOpticalInterface.Controls.Add((Control) this.buttonOptoActivateStatic);
      this.groupBoxOpticalInterface.Controls.Add((Control) this.buttonOptoReleaseStatic);
      this.groupBoxOpticalInterface.Location = new Point(923, 49);
      this.groupBoxOpticalInterface.Name = "groupBoxOpticalInterface";
      this.groupBoxOpticalInterface.Size = new Size(344, 53);
      this.groupBoxOpticalInterface.TabIndex = 23;
      this.groupBoxOpticalInterface.TabStop = false;
      this.groupBoxOpticalInterface.Text = "Optical interface";
      this.buttonOptoActivateStatic.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonOptoActivateStatic.Location = new Point(7, 19);
      this.buttonOptoActivateStatic.Name = "buttonOptoActivateStatic";
      this.buttonOptoActivateStatic.Size = new Size(162, 23);
      this.buttonOptoActivateStatic.TabIndex = 18;
      this.buttonOptoActivateStatic.Text = "Activate static";
      this.buttonOptoActivateStatic.UseVisualStyleBackColor = true;
      this.buttonOptoActivateStatic.Click += new System.EventHandler(this.buttonOptoActivateStatic_Click);
      this.buttonOptoReleaseStatic.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonOptoReleaseStatic.Location = new Point(175, 19);
      this.buttonOptoReleaseStatic.Name = "buttonOptoReleaseStatic";
      this.buttonOptoReleaseStatic.Size = new Size(162, 23);
      this.buttonOptoReleaseStatic.TabIndex = 18;
      this.buttonOptoReleaseStatic.Text = "Release static";
      this.buttonOptoReleaseStatic.UseVisualStyleBackColor = true;
      this.buttonOptoReleaseStatic.Click += new System.EventHandler(this.buttonOptoReleaseStatic_Click);
      this.groupBoxFlyingTestSimulation.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.groupBoxFlyingTestSimulation.Controls.Add((Control) this.textBoxStartWaitSeconds);
      this.groupBoxFlyingTestSimulation.Controls.Add((Control) this.label4);
      this.groupBoxFlyingTestSimulation.Controls.Add((Control) this.checkBoxRepeatFlyingTest);
      this.groupBoxFlyingTestSimulation.Controls.Add((Control) this.textBoxWaitSeconds);
      this.groupBoxFlyingTestSimulation.Controls.Add((Control) this.label3);
      this.groupBoxFlyingTestSimulation.Controls.Add((Control) this.textBoxTestSeconds);
      this.groupBoxFlyingTestSimulation.Controls.Add((Control) this.label1);
      this.groupBoxFlyingTestSimulation.Controls.Add((Control) this.buttonRunTest);
      this.groupBoxFlyingTestSimulation.Location = new Point(1102, 108);
      this.groupBoxFlyingTestSimulation.Name = "groupBoxFlyingTestSimulation";
      this.groupBoxFlyingTestSimulation.Size = new Size(166, 282);
      this.groupBoxFlyingTestSimulation.TabIndex = 25;
      this.groupBoxFlyingTestSimulation.TabStop = false;
      this.groupBoxFlyingTestSimulation.Text = "Flying test simulation";
      this.textBoxStartWaitSeconds.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxStartWaitSeconds.Location = new Point(108, 18);
      this.textBoxStartWaitSeconds.Name = "textBoxStartWaitSeconds";
      this.textBoxStartWaitSeconds.Size = new Size(49, 20);
      this.textBoxStartWaitSeconds.TabIndex = 25;
      this.textBoxStartWaitSeconds.Text = "10";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 21);
      this.label4.Name = "label4";
      this.label4.Size = new Size(97, 13);
      this.label4.TabIndex = 24;
      this.label4.Text = "Start wait seconds:";
      this.checkBoxRepeatFlyingTest.AutoSize = true;
      this.checkBoxRepeatFlyingTest.Location = new Point(49, 93);
      this.checkBoxRepeatFlyingTest.Name = "checkBoxRepeatFlyingTest";
      this.checkBoxRepeatFlyingTest.RightToLeft = RightToLeft.Yes;
      this.checkBoxRepeatFlyingTest.Size = new Size(108, 17);
      this.checkBoxRepeatFlyingTest.TabIndex = 23;
      this.checkBoxRepeatFlyingTest.Text = "Repeat flying test";
      this.checkBoxRepeatFlyingTest.UseVisualStyleBackColor = true;
      this.textBoxWaitSeconds.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxWaitSeconds.Location = new Point(108, 67);
      this.textBoxWaitSeconds.Name = "textBoxWaitSeconds";
      this.textBoxWaitSeconds.Size = new Size(49, 20);
      this.textBoxWaitSeconds.TabIndex = 22;
      this.textBoxWaitSeconds.Text = "30";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(6, 70);
      this.label3.Name = "label3";
      this.label3.Size = new Size(75, 13);
      this.label3.TabIndex = 21;
      this.label3.Text = "Wait seconds:";
      this.textBoxTestSeconds.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.textBoxTestSeconds.Location = new Point(108, 43);
      this.textBoxTestSeconds.Name = "textBoxTestSeconds";
      this.textBoxTestSeconds.Size = new Size(49, 20);
      this.textBoxTestSeconds.TabIndex = 20;
      this.textBoxTestSeconds.Text = "30";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 46);
      this.label1.Name = "label1";
      this.label1.Size = new Size(74, 13);
      this.label1.TabIndex = 19;
      this.label1.Text = "Test seconds:";
      this.buttonRunTest.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.buttonRunTest.Location = new Point(7, 253);
      this.buttonRunTest.Name = "buttonRunTest";
      this.buttonRunTest.Size = new Size(153, 23);
      this.buttonRunTest.TabIndex = 18;
      this.buttonRunTest.Text = "Run test";
      this.buttonRunTest.UseVisualStyleBackColor = true;
      this.buttonRunTest.Click += new System.EventHandler(this.buttonRunTest_Click);
      this.buttonClearWindow.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonClearWindow.Location = new Point(1095, 629);
      this.buttonClearWindow.Name = "buttonClearWindow";
      this.buttonClearWindow.Size = new Size(173, 40);
      this.buttonClearWindow.TabIndex = 21;
      this.buttonClearWindow.Text = "Clear window";
      this.buttonClearWindow.UseVisualStyleBackColor = true;
      this.buttonClearWindow.Click += new System.EventHandler(this.buttonClearWindow_Click);
      this.groupBoxTDC.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.groupBoxTDC.Controls.Add((Control) this.btnTDCmap);
      this.groupBoxTDC.Controls.Add((Control) this.buttonCheckIUF);
      this.groupBoxTDC.Controls.Add((Control) this.buttonTdcInternals);
      this.groupBoxTDC.Controls.Add((Control) this.groupBoxTDC_Calibration);
      this.groupBoxTDC.Location = new Point(923, 396);
      this.groupBoxTDC.Name = "groupBoxTDC";
      this.groupBoxTDC.Size = new Size(345, 111);
      this.groupBoxTDC.TabIndex = 28;
      this.groupBoxTDC.TabStop = false;
      this.groupBoxTDC.Text = "TDC functions";
      this.btnTDCmap.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnTDCmap.Location = new Point(192, 77);
      this.btnTDCmap.Name = "btnTDCmap";
      this.btnTDCmap.Size = new Size(145, 23);
      this.btnTDCmap.TabIndex = 30;
      this.btnTDCmap.Text = "TDC map";
      this.btnTDCmap.UseVisualStyleBackColor = true;
      this.buttonCheckIUF.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonCheckIUF.Location = new Point(192, 19);
      this.buttonCheckIUF.Name = "buttonCheckIUF";
      this.buttonCheckIUF.Size = new Size(145, 23);
      this.buttonCheckIUF.TabIndex = 28;
      this.buttonCheckIUF.Text = "Check ultrasonic meter";
      this.buttonCheckIUF.UseVisualStyleBackColor = true;
      this.buttonCheckIUF.Click += new System.EventHandler(this.buttonCheckIUF_Click);
      this.buttonTdcInternals.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonTdcInternals.Location = new Point(192, 48);
      this.buttonTdcInternals.Name = "buttonTdcInternals";
      this.buttonTdcInternals.Size = new Size(145, 23);
      this.buttonTdcInternals.TabIndex = 29;
      this.buttonTdcInternals.Text = "TDC internals";
      this.buttonTdcInternals.UseVisualStyleBackColor = true;
      this.buttonTdcInternals.Visible = false;
      this.buttonTdcInternals.Click += new System.EventHandler(this.buttonTdcInternals_Click);
      this.groupBoxTDC_Calibration.Controls.Add((Control) this.textBoxErrorInPercent);
      this.groupBoxTDC_Calibration.Controls.Add((Control) this.label2);
      this.groupBoxTDC_Calibration.Controls.Add((Control) this.buttonRunCalibration);
      this.groupBoxTDC_Calibration.Location = new Point(9, 19);
      this.groupBoxTDC_Calibration.Name = "groupBoxTDC_Calibration";
      this.groupBoxTDC_Calibration.Size = new Size(175, 83);
      this.groupBoxTDC_Calibration.TabIndex = 24;
      this.groupBoxTDC_Calibration.TabStop = false;
      this.groupBoxTDC_Calibration.Text = "Calibration";
      this.textBoxErrorInPercent.Location = new Point(87, 19);
      this.textBoxErrorInPercent.Name = "textBoxErrorInPercent";
      this.textBoxErrorInPercent.Size = new Size(79, 20);
      this.textBoxErrorInPercent.TabIndex = 20;
      this.textBoxErrorInPercent.Text = "2";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 22);
      this.label2.Name = "label2";
      this.label2.Size = new Size(54, 13);
      this.label2.TabIndex = 19;
      this.label2.Text = "Error in %:";
      this.buttonRunCalibration.Location = new Point(5, 45);
      this.buttonRunCalibration.Name = "buttonRunCalibration";
      this.buttonRunCalibration.Size = new Size(162, 23);
      this.buttonRunCalibration.TabIndex = 18;
      this.buttonRunCalibration.Text = "Run calibration";
      this.buttonRunCalibration.UseVisualStyleBackColor = true;
      this.groupBoxVMCP.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.groupBoxVMCP.Controls.Add((Control) this.buttonReadVmcpVolume);
      this.groupBoxVMCP.Controls.Add((Control) this.buttonRead_VMCP_State);
      this.groupBoxVMCP.Location = new Point(923, 513);
      this.groupBoxVMCP.Name = "groupBoxVMCP";
      this.groupBoxVMCP.Size = new Size(343, 78);
      this.groupBoxVMCP.TabIndex = 29;
      this.groupBoxVMCP.TabStop = false;
      this.groupBoxVMCP.Text = "VMCP functions";
      this.buttonReadVmcpVolume.Location = new Point(7, 48);
      this.buttonReadVmcpVolume.Name = "buttonReadVmcpVolume";
      this.buttonReadVmcpVolume.Size = new Size(329, 23);
      this.buttonReadVmcpVolume.TabIndex = 0;
      this.buttonReadVmcpVolume.Text = "Read VMCP volume";
      this.buttonReadVmcpVolume.UseVisualStyleBackColor = true;
      this.buttonReadVmcpVolume.Click += new System.EventHandler(this.buttonReadVmcpVolume_Click);
      this.buttonRead_VMCP_State.Location = new Point(7, 19);
      this.buttonRead_VMCP_State.Name = "buttonRead_VMCP_State";
      this.buttonRead_VMCP_State.Size = new Size(329, 23);
      this.buttonRead_VMCP_State.TabIndex = 0;
      this.buttonRead_VMCP_State.Text = "Read VMCP state";
      this.buttonRead_VMCP_State.UseVisualStyleBackColor = true;
      this.buttonRead_VMCP_State.Click += new System.EventHandler(this.buttonRead_VMCP_State_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1278, 681);
      this.Controls.Add((Control) this.groupBoxVMCP);
      this.Controls.Add((Control) this.groupBoxTDC);
      this.Controls.Add((Control) this.groupBoxFlyingTestSimulation);
      this.Controls.Add((Control) this.groupBoxOpticalInterface);
      this.Controls.Add((Control) this.groupBoxFlyingTestCommands);
      this.Controls.Add((Control) this.splitContainer1);
      this.Controls.Add((Control) this.buttonClearWindow);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Name = nameof (TDC_Measurement);
      this.Text = "Volume measurement";
      this.groupBoxFlyingTestCommands.ResumeLayout(false);
      this.groupBoxFlyingTestCommands.PerformLayout();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel1.PerformLayout();
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.Panel2.PerformLayout();
      this.splitContainer1.EndInit();
      this.splitContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.dataGridViewInfos).EndInit();
      this.groupBoxOpticalInterface.ResumeLayout(false);
      this.groupBoxFlyingTestSimulation.ResumeLayout(false);
      this.groupBoxFlyingTestSimulation.PerformLayout();
      this.groupBoxTDC.ResumeLayout(false);
      this.groupBoxTDC_Calibration.ResumeLayout(false);
      this.groupBoxTDC_Calibration.PerformLayout();
      this.groupBoxVMCP.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}

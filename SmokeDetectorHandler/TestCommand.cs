// Decompiled with JetBrains decompiler
// Type: SmokeDetectorHandler.TestCommand
// Assembly: SmokeDetectorHandler, Version=2.20.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8E8970E7-4D1B-41F1-9589-E7C5C5D80A7B
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmokeDetectorHandler.dll

using CommunicationPort;
using CommunicationPort.Functions;
using DeviceCollector;
using HandlerLib;
using NLog;
using ReadoutConfiguration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Entities;
using ZR_ClassLibrary;

#nullable disable
namespace SmokeDetectorHandler
{
  public class TestCommand : Form
  {
    private static Logger logger = LogManager.GetLogger(nameof (TestCommand));
    private SmokeDetectorHandlerFunctions smokeHandler;
    private bool isCanceled;
    private Stopwatch stopwatch;
    private OleDbConnection cn;
    private OleDbCommand cmd;
    private IContainer components = (IContainer) null;
    private TextBox txtOutput;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel lblStatus;
    private ToolStripProgressBar progress;
    private ToolStripStatusLabel lblProgress;
    private ToolStripStatusLabel lblPerformance;
    private GroupBox groupBox2;
    private TabPage tabGenerally;
    private TabControl tabs;
    private Button btnTC_SetDeliveryState;
    private Button btnTC_ResetDevice;
    private Button btnTC_ButtonFunction;
    private GroupBox groupBox4;
    private Button btnStartFunctionTest;
    private ComboBox cboxFunctionTestMode;
    private Button btnStopFunctionTest;
    private Label label18;
    private GroupBox groupBox3;
    private Label label1;
    private ComboBox cboxRadioMode;
    private Button btnStartRadioTest;
    private Button btnStopRadioTest;
    private TabPage tabTestMode;
    private Button btnTC_EraseEEPROM;
    private Button btnTC_SurroundingAreaMonitoringCheckReceiverTestResult;
    private Button btnTC_SurroundingAreaMonitoringCheckReceiver;
    private Button btnTC_ClearTestRecordT1;
    private GroupBox groupBox1;
    private ComboBox txtLeds;
    private Button btnTC_SurroundingAreaMonitoringCheckTransmitter;
    private Button btnTC_ObstructionCheck;
    private Button btnTC_TestData;
    private Button btnTC_EepromState;
    private Button btnTC_ButtonTest;
    private Button btnTC_Check_LED_Battery_Voltage_TemperatureSensor_Piezo;
    private Button btnTC_PiezoTestLowSoundPressure;
    private Button btnTC_PiezoTestHighSoundPressure;
    private Button btnTC_TransmitterLedInSmokeChamberVoltageReferenceTL431;
    private Button btnTC_ExitTestMode;
    private Button btnTC_EnterTestMode;
    private Button btnTC_CauseTestAlarm;
    private NumericUpDown txtCValue;
    private Label label2;
    private Button btnTC_WriteSmokeDensityThreshold_C_Value;
    private Button btnTC_ReadSmokeDensityAndSensitivity;
    private Button btnTC_ReadSmokeDensityAndSensitivity_90_times;
    private Button btnTC_ReadSmokeDensityAndSensitivity_once;
    private Button btnRadioRadioTest;
    private Button WriteDevEUI;
    private TabPage tabDataAnalysis;
    private Button btnGetData;
    private Button btnSetPath;
    private Label labelPath;
    private Button btnSetPathOld;
    private Button btnWriteAesKey;
    private Button btnReadAesKey;
    private Button btnCheckLedFailure;
    private Button btnMbus_interval;
    private NumericUpDown txtMbus_interval;
    private Label label3;
    private Button btnMbus_radio_suppression_days;
    private NumericUpDown txtMbus_radio_suppression_days;
    private Label label4;
    private Button btnMbus_nighttime_start;
    private NumericUpDown txtMbus_nighttime_start;
    private Label label5;
    private Button btnMbus_nighttime_stop;
    private NumericUpDown txtMbus_nighttime_stop;
    private Label label6;
    private Button btnSet15Min;
    private Button btnSet_A_B;
    private Button btnSetTestIdentification;
    private Button btnTC_PiezoAdjustValueHighSound;
    private NumericUpDown txtPiezoHsDuration;
    private Button button1;
    private Button button2;
    private GroupBox groupBox5;
    private NumericUpDown txtNear6;
    private Label label12;
    private NumericUpDown txtNear5;
    private Label label11;
    private NumericUpDown txtNear4;
    private Label label10;
    private NumericUpDown txtNear3;
    private Label label9;
    private NumericUpDown txtNear2;
    private Label label8;
    private NumericUpDown txtNear1;
    private Label label7;

    public TestCommand()
    {
      this.InitializeComponent();
      this.stopwatch = new Stopwatch();
      this.isCanceled = false;
      this.ResetUI();
    }

    public TestCommand(SmokeDetectorHandlerFunctions MyFunctions)
      : this()
    {
      this.smokeHandler = MyFunctions;
    }

    internal static void ShowDialog(Form owner, SmokeDetectorHandlerFunctions smokeHandler)
    {
      if (smokeHandler == null)
        return;
      using (TestCommand testCommand = new TestCommand())
      {
        testCommand.smokeHandler = smokeHandler;
        int num = (int) testCommand.ShowDialog((IWin32Window) owner);
      }
    }

    private void TestCommand_Load(object sender, EventArgs e)
    {
      this.txtLeds.DataSource = (object) Util.GetNamesOfEnum(typeof (SmokeDetector.Check));
      this.cboxRadioMode.DataSource = (object) Util.GetNamesOfEnum(typeof (RadioMode));
      this.cboxFunctionTestMode.DataSource = (object) Util.GetNamesOfEnum(typeof (FunctionTestMode));
      this.smokeHandler.MyDeviceCollector.OnMessage += new EventHandler<GMM_EventArgs>(this.MyDeviceCollector_OnMessage);
      this.smokeHandler.OnSmokeDensityAndSensitivityValueReceived += new EventHandlerEx<SmokeDetector.SmokeDensityAndSensitivity>(this.SmokeHandler_OnSmokeDensityAndSensitivityValueReceived);
      this.btnGetData.Enabled = false;
    }

    private void TestCommand_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.smokeHandler.MyDeviceCollector.OnMessage -= new EventHandler<GMM_EventArgs>(this.MyDeviceCollector_OnMessage);
      this.smokeHandler.OnSmokeDensityAndSensitivityValueReceived -= new EventHandlerEx<SmokeDetector.SmokeDensityAndSensitivity>(this.SmokeHandler_OnSmokeDensityAndSensitivityValueReceived);
      if (this.cn == null)
        return;
      this.cn.Close();
    }

    private void MyDeviceCollector_OnMessage(object sender, GMM_EventArgs e)
    {
      if (this.InvokeRequired)
      {
        try
        {
          this.BeginInvoke((Delegate) new EventHandler<GMM_EventArgs>(this.MyDeviceCollector_OnMessage), sender, (object) e);
        }
        catch
        {
        }
      }
      else
      {
        e.Cancel = this.isCanceled;
        if (e.Cancel)
        {
          this.smokeHandler.MyDeviceCollector.BreakRequest = true;
        }
        else
        {
          this.progress.Visible = e.ProgressPercentage > 0;
          this.lblProgress.Visible = this.progress.Visible;
          if (this.progress.Visible)
          {
            this.progress.Value = e.ProgressPercentage;
            this.lblProgress.Text = string.Format("{0}%", (object) e.ProgressPercentage);
          }
          if (string.IsNullOrEmpty(e.EventMessage) || e.TheMessageType == GMM_EventArgs.MessageType.Alive)
            return;
          this.lblStatus.Text = e.EventMessage;
        }
      }
    }

    private void btnStartRadioTest_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<bool, RadioMode>(new TestCommand.ActionSimpleMethodWithParam<bool, RadioMode>(this.smokeHandler.StartRadioTest), (RadioMode) Enum.Parse(typeof (RadioMode), this.cboxRadioMode.SelectedItem.ToString()));
    }

    private void btnStopRadioTest_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<bool>(new TestCommand.ActionSimpleMethod<bool>(this.smokeHandler.StopRadioTest));
    }

    private void btnStartFunctionTest_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<bool, FunctionTestMode>(new TestCommand.ActionSimpleMethodWithParam<bool, FunctionTestMode>(this.smokeHandler.StartFunctionTest), (FunctionTestMode) Enum.Parse(typeof (FunctionTestMode), this.cboxFunctionTestMode.SelectedItem.ToString()));
    }

    private void btnStopFunctionTest_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<bool>(new TestCommand.ActionSimpleMethod<bool>(this.smokeHandler.StopFunctionTest));
    }

    private void btnTC_EnterTestMode_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<bool>(new TestCommand.ActionSimpleMethod<bool>(this.smokeHandler.TC_EnterTestMode));
    }

    private void btnTC_ExitTestMode_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<bool>(new TestCommand.ActionSimpleMethod<bool>(this.smokeHandler.TC_ExitTestMode));
    }

    private void btnTC_TransmitterLedInSmokeChamberVoltageReferenceTL431_Click(
      object sender,
      EventArgs e)
    {
      this.ExecuteSimpleMethod<bool>(new TestCommand.ActionSimpleMethod<bool>(this.smokeHandler.TC_TransmitterLedInSmokeChamberVoltageReferenceTL431));
    }

    private void btnTC_PiezoTestHighSoundPressure_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<bool, byte>(new TestCommand.ActionSimpleMethodWithParam<bool, byte>(this.smokeHandler.TC_PiezoTestHighSoundPressure), Convert.ToByte(this.txtPiezoHsDuration.Value));
    }

    private void btnTC_PiezoAdjustValueHighSound_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<byte>(new TestCommand.ActionSimpleMethod<byte>(this.smokeHandler.TC_PiezoAdjustValueHighSound));
    }

    private void btnTC_PiezoTestLowSoundPressure_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<bool>(new TestCommand.ActionSimpleMethod<bool>(this.smokeHandler.TC_PiezoTestLowSoundPressure));
    }

    private void btnTC_SetDeliveryState_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<bool>(new TestCommand.ActionSimpleMethod<bool>(this.smokeHandler.TC_SetDeliveryState));
    }

    private void btnTC_Check_LED_Battery_Voltage_TemperatureSensor_Piezo_Click(
      object sender,
      EventArgs e)
    {
      this.ExecuteSimpleMethod<SmokeDetector.HardwareState>(new TestCommand.ActionSimpleMethod<SmokeDetector.HardwareState>(this.smokeHandler.TC_Check_LED_Battery_Voltage_TemperatureSensor_Piezo));
    }

    private void btnTC_ButtonTest_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<bool>(new TestCommand.ActionSimpleMethod<bool>(this.smokeHandler.TC_ButtonTest));
    }

    private void btnTC_EepromState_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<SmokeDetector.EepromState?>(new TestCommand.ActionSimpleMethod<SmokeDetector.EepromState?>(this.smokeHandler.TC_EepromState));
    }

    private void btnTC_TestData_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<SmokeDetector.TestData>(new TestCommand.ActionSimpleMethod<SmokeDetector.TestData>(this.smokeHandler.TC_TestData));
    }

    private void btnTC_ObstructionCheck_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<SmokeDetector.ObstructionState>(new TestCommand.ActionSimpleMethod<SmokeDetector.ObstructionState>(this.smokeHandler.TC_ObstructionCheck));
    }

    private void btnTC_ObstructionCalibrationWrite(object sender, EventArgs e)
    {
      this.txtOutput.Text = this.smokeHandler.TC_ObstructionCalibrationWrite(Convert.ToUInt16(this.txtNear1.Text), Convert.ToUInt16(this.txtNear2.Text), Convert.ToUInt16(this.txtNear3.Text), Convert.ToUInt16(this.txtNear4.Text), Convert.ToUInt16(this.txtNear5.Text), Convert.ToUInt16(this.txtNear6.Text)) ? "OK" : "Not OK";
    }

    private void btnTC_ObstructionCalibrationRead(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<SmokeDetector.ObstructionState>(new TestCommand.ActionSimpleMethod<SmokeDetector.ObstructionState>(this.smokeHandler.TC_ObstructionCalibrationRead));
    }

    private void btnTC_SurroundingAreaMonitoringCheckReceiver_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<bool>(new TestCommand.ActionSimpleMethod<bool>(this.smokeHandler.TC_SurroundingAreaMonitoringCheckReceiver));
    }

    private void btnTC_SurroundingAreaMonitoringCheckReceiverTestResult_Click(
      object sender,
      EventArgs e)
    {
      this.ExecuteSimpleMethod<byte?>(new TestCommand.ActionSimpleMethod<byte?>(this.smokeHandler.TC_SurroundingAreaMonitoringCheckReceiverTestResult));
    }

    private void btnTC_SurroundingAreaMonitoringCheckTransmitter_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<bool, SmokeDetector.Check>(new TestCommand.ActionSimpleMethodWithParam<bool, SmokeDetector.Check>(this.smokeHandler.TC_SurroundingAreaMonitoringCheckTransmitter), (SmokeDetector.Check) Enum.Parse(typeof (SmokeDetector.Check), this.txtLeds.SelectedItem.ToString()));
    }

    private void btnTC_ClearTestRecordT1_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<DateTime?>(new TestCommand.ActionSimpleMethod<DateTime?>(this.smokeHandler.TC_ClearTestRecordT1));
    }

    private void btnTC_EraseEEPROM_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<DateTime?>(new TestCommand.ActionSimpleMethod<DateTime?>(this.smokeHandler.TC_EraseEEPROM));
    }

    private void btnTC_CauseTestAlarm_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<bool?>(new TestCommand.ActionSimpleMethod<bool?>(this.smokeHandler.TC_CauseTestAlarm));
    }

    private void btnTC_ButtonFunction_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<bool>(new TestCommand.ActionSimpleMethod<bool>(this.smokeHandler.TC_ButtonFunction));
    }

    private void btnTC_ResetDevice_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<bool>(new TestCommand.ActionSimpleMethod<bool>(this.smokeHandler.TC_ResetDevice));
    }

    private void btnTC_ReadSmokeDensityAndSensitivity_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<SmokeDetector.SmokeDensityAndSensitivity>(new TestCommand.ActionSimpleMethod<SmokeDetector.SmokeDensityAndSensitivity>(this.smokeHandler.TC_ReadSmokeDensityAndSensitivity));
    }

    private void btnTC_WriteSmokeDensityThreshold_C_Value_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<ushort?, byte>(new TestCommand.ActionSimpleMethodWithParam<ushort?, byte>(this.smokeHandler.TC_WriteSmokeDensityThreshold_C_Value), Convert.ToByte(this.txtCValue.Value));
    }

    private void btnTC_ReadSmokeDensityAndSensitivity_90_times_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<bool>(new TestCommand.ActionSimpleMethod<bool>(this.smokeHandler.TC_ReadSmokeDensityAndSensitivity_90_times));
    }

    private void btnSet_A_B_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<SmokeDetector.Smoke_A_B>(new TestCommand.ActionSimpleMethod<SmokeDetector.Smoke_A_B>(this.smokeHandler.TC_Set_A_B));
    }

    private void btnTC_ReadSmokeDensityAndSensitivity_once_Click(object sender, EventArgs e)
    {
      this.ExecuteSimpleMethod<SmokeDetector.SmokeDensityAndSensitivity>(new TestCommand.ActionSimpleMethod<SmokeDetector.SmokeDensityAndSensitivity>(this.smokeHandler.TC_ReadSmokeDensityAndSensitivity_once));
    }

    private void SmokeHandler_OnSmokeDensityAndSensitivityValueReceived(
      object sender,
      SmokeDetector.SmokeDensityAndSensitivity e)
    {
      if (e.A_EEPROM.HasValue)
        this.txtOutput.AppendText(string.Format("A: {0}, B: {1}, C: {2}, A_EEPROM: {3}, B_EEPROM: {4}, C_MEASURED: {5}", (object) e.A, (object) e.B, (object) e.C, (object) e.A_EEPROM, (object) e.B_EEPROM, (object) e.C_MEASURED));
      else
        this.txtOutput.AppendText(string.Format("A: {0}, B: {1}, C: {2}", (object) e.A, (object) e.B, (object) e.C));
      this.txtOutput.AppendText(Environment.NewLine);
    }

    private void ResetUI()
    {
      this.isCanceled = false;
      this.lblProgress.Text = string.Empty;
      this.lblStatus.Text = string.Empty;
      this.lblPerformance.Text = string.Empty;
      this.txtOutput.Text = string.Empty;
      this.stopwatch.Reset();
      this.stopwatch.Start();
    }

    private void ExecuteSimpleMethod<T>(TestCommand.ActionSimpleMethod<T> act)
    {
      this.ResetUI();
      string methodInfo = string.Format("{0}{1}.{2}", (object) act.Method.ReturnParameter, act.Target, (object) act.Method.Name);
      this.ShowResult<T>(act(), methodInfo);
    }

    private void ExecuteSimpleMethod<TReturn, TArg>(
      TestCommand.ActionSimpleMethodWithParam<TReturn, TArg> act,
      TArg arg)
    {
      this.ResetUI();
      string methodInfo = string.Format("{0}{1}.{2}", (object) act.Method.ReturnParameter, act.Target, (object) act.Method.Name);
      this.ShowResult<TReturn>(act(arg), methodInfo);
    }

    private void ShowResult<TReturn>(TReturn result, string methodInfo)
    {
      try
      {
        if ((object) result is bool)
        {
          bool boolean = Convert.ToBoolean((object) result);
          this.txtOutput.Text = boolean ? "OK" : "Not OK";
          if (!boolean)
          {
            string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
            TextBox txtOutput = this.txtOutput;
            txtOutput.Text = txtOutput.Text + Environment.NewLine + errorDescription;
          }
        }
        else if ((object) result != null)
          this.txtOutput.Text = result.ToString();
        else
          this.txtOutput.Text = "Failed to execute: " + methodInfo + Environment.NewLine + Environment.NewLine + ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
      }
      catch (Exception ex)
      {
        string errorDescription = ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription;
        int num = (int) MessageBox.Show((IWin32Window) this, "Failed to execute: " + methodInfo + Environment.NewLine + Environment.NewLine + errorDescription + Environment.NewLine + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
        this.stopwatch.Stop();
        this.lblPerformance.Text = string.Format("Time elapsed: {0}", (object) this.stopwatch.Elapsed);
      }
      this.lblStatus.Text = string.Empty;
      this.lblProgress.Text = string.Empty;
    }

    private void btnRadioRadioTest_Click(object sender, EventArgs e)
    {
      AsyncHelpers.RunSync((Func<Task>) (async () =>
      {
        ProgressHandler progress = new ProgressHandler((Action<ProgressArg>) (x => { }));
        CancellationToken cancelToken = new CancellationToken();
        this.txtOutput.Text = string.Empty;
        Stopwatch watch = Stopwatch.StartNew();
        SmokeDetectorHandlerFunctions handler = this.smokeHandler;
        ZR_ClassLibrary.RadioMode mode = ZR_ClassLibrary.RadioMode.Radio3_868_95;
        uint serialnumber = 11111111;
        byte[] arbitraryData = new byte[28];
        TestCommand.logger.Info("TEST: SD LoRa = > MiCon");
        ConnectionProfile profile = ReadoutConfigFunctions.Manager.GetConnectionProfile(32);
        profile.EquipmentModel.ChangeableParameters.Find((Predicate<ChangeableParameter>) (x => x.Key == "Port")).Value = "COM5";
        CommunicationPortFunctions port = new CommunicationPortFunctions();
        port.SetReadoutConfiguration(profile.GetSettingsList());
        TextBox txtOutput1 = this.txtOutput;
        txtOutput1.Text = txtOutput1.Text + "Open MiCon " + profile.EquipmentModel.ChangeableParameters.Find((Predicate<ChangeableParameter>) (x => x.Key == "Port")).Value + Environment.NewLine + Environment.NewLine;
        port.Open();
        await Task.Delay(500);
        CommunicationByMinoConnect micon = port.GetCommunicationByMinoConnect();
        try
        {
          TestCommand.logger.Info("TEST: MiCon => SD LoRa");
          TextBox txtOutput2 = this.txtOutput;
          txtOutput2.Text = txtOutput2.Text + Environment.NewLine + "Start SD LoRa receiver (" + (ushort) 10.ToString() + " sec)" + Environment.NewLine;
          TextBox txtOutput3 = this.txtOutput;
          txtOutput3.Text = txtOutput3.Text + "Set frequency to 868.95 MHz" + Environment.NewLine;
          await handler.SetCenterFrequencyAsync(progress, cancelToken, 868950000U);
          Task<double?> resultReceiver2 = handler.ReceiveTestPacketAsync(progress, cancelToken, (byte) 10, serialnumber, "D696");
          TextBox txtOutput4 = this.txtOutput;
          txtOutput4.Text = txtOutput4.Text + "Send test packet via MiCon" + Environment.NewLine;
          await Task.Run((Action) (() =>
          {
            while (!resultReceiver2.IsCompleted)
            {
              try
              {
                micon.SendTestPacket(serialnumber, mode, (byte) 7, "D696");
                Thread.Sleep(1500);
              }
              catch (Exception ex)
              {
                TestCommand.logger.Error(ex.Message);
              }
            }
          }));
          try
          {
            if (resultReceiver2.Result.HasValue)
            {
              TestCommand.logger.Info("Successfully send/receive a TEST packet from MinoConnect to SD LoRa. RSSI: " + resultReceiver2.Result.ToString() + " dBm");
              TextBox txtOutput5 = this.txtOutput;
              txtOutput5.Text = txtOutput5.Text + "MiCon = > SD LoRa OK RSSI: " + resultReceiver2.Result.ToString() + "dBm " + Environment.NewLine;
            }
            else
            {
              TestCommand.logger.Error("Failed send/receive a TEST packet from MinoConnect to SD LoRa");
              TextBox txtOutput6 = this.txtOutput;
              txtOutput6.Text = txtOutput6.Text + "MiCon = > SD LoRa Error" + Environment.NewLine;
            }
          }
          catch (Exception ex)
          {
            TestCommand.logger.Error("Failed send/receive a TEST packet from MinoConnect to SD LoRa");
            TextBox txtOutput7 = this.txtOutput;
            txtOutput7.Text = txtOutput7.Text + "MiCon = > SD LoRa Error: " + ex.Message + Environment.NewLine;
          }
          await Task.Delay(500);
          bool done = false;
          RadioTestResult resultReceiver = (RadioTestResult) null;
          TextBox txtOutput8 = this.txtOutput;
          txtOutput8.Text = txtOutput8.Text + "Start MiCon receiver (" + (ushort) 10.ToString() + " sec)" + Environment.NewLine;
          Task taskReceiver = Task.Run((Action) (() =>
          {
            try
            {
              resultReceiver = micon.ReceiveOnePacket(mode, (int) serialnumber, (ushort) 10, "D696");
            }
            catch (Exception ex)
            {
              TestCommand.logger.Error(ex.Message);
            }
            done = true;
          }));
          TextBox txtOutput9 = this.txtOutput;
          txtOutput9.Text = txtOutput9.Text + "Send test packet via SD LoRa" + Environment.NewLine;
          while (!done)
          {
            await Task.Delay(500);
            try
            {
              await handler.SendTestPacketAsync(progress, cancelToken, (ushort) 1, (ushort) 1, serialnumber, arbitraryData, "D696");
            }
            catch (Exception ex)
            {
              TestCommand.logger.Error(ex.Message);
            }
          }
          if (resultReceiver != null)
          {
            TestCommand.logger.Info("Successfully send/receive a TEST packet from SD LoRa to MinoConnect. RSSI: " + resultReceiver.RSSI.ToString() + " dBm");
            TextBox txtOutput10 = this.txtOutput;
            txtOutput10.Text = txtOutput10.Text + "SD LoRa = > MiCon OK. RSSI: " + resultReceiver.RSSI.ToString() + " dBm" + Environment.NewLine;
          }
          else
          {
            TestCommand.logger.Error("Failed send/receive a TEST packet from SD LoRa to MinoConnect");
            TextBox txtOutput11 = this.txtOutput;
            txtOutput11.Text = txtOutput11.Text + "SD LoRa = > MiCon Error" + Environment.NewLine;
          }
          TextBox txtOutput12 = this.txtOutput;
          txtOutput12.Text = txtOutput12.Text + "Set frequency to 868.30 MHz" + Environment.NewLine;
          await handler.SetCenterFrequencyAsync(progress, cancelToken, 868300000U);
          taskReceiver = (Task) null;
        }
        finally
        {
          TextBox txtOutput13 = this.txtOutput;
          txtOutput13.Text = txtOutput13.Text + Environment.NewLine + "Close MiCon " + profile.EquipmentModel.ChangeableParameters.Find((Predicate<ChangeableParameter>) (x => x.Key == "Port")).Value + Environment.NewLine;
          port.Close();
          watch.Stop();
          TextBox txtOutput14 = this.txtOutput;
          txtOutput14.Text = txtOutput14.Text + "Elapsed: " + Util.ElapsedToString(watch.Elapsed) + Environment.NewLine;
        }
        progress = (ProgressHandler) null;
        cancelToken = new CancellationToken();
        watch = (Stopwatch) null;
        handler = (SmokeDetectorHandlerFunctions) null;
        arbitraryData = (byte[]) null;
        profile = (ConnectionProfile) null;
        port = (CommunicationPortFunctions) null;
      }));
    }

    private void WriteDevEUI_Click(object sender, EventArgs e)
    {
      byte[] devEUI = Utility.HexStringToByteArray("04B6480B01119482");
      this.smokeHandler.ReadVersion();
      ProgressHandler progress = new ProgressHandler((Action<ProgressArg>) (x => { }));
      CancellationToken token = new CancellationTokenSource().Token;
      AsyncHelpers.RunSync((Func<Task>) (async () =>
      {
        DeviceVersionMBus deviceVersionMbus = await this.smokeHandler.Device.DeviceCMD.ReadVersionAsync(progress, token);
        await this.smokeHandler.LoRa.SetDevEUIAsync(devEUI, progress, token);
      }));
      this.txtOutput.Text = "OK";
    }

    private async void btnGetData_Click(object sender, EventArgs e)
    {
      this.txtOutput.BackColor = Color.White;
      this.txtOutput.Text = string.Empty;
      this.btnGetData.Enabled = false;
      this.btnSetPath.Enabled = false;
      this.btnSetPathOld.Enabled = false;
      TextBox textBox = this.txtOutput;
      string str = await this.GetAndWriteData();
      textBox.Text = str;
      textBox = (TextBox) null;
      str = (string) null;
      if (this.txtOutput.Text == "OK")
        this.txtOutput.BackColor = Color.LightGreen;
      else
        this.txtOutput.BackColor = Color.Red;
      this.btnGetData.Enabled = true;
      this.btnSetPath.Enabled = true;
      this.btnSetPathOld.Enabled = true;
    }

    private Task<string> GetAndWriteData()
    {
      return Task.Run<string>((Func<string>) (() =>
      {
        try
        {
          byte[] buffer = this.smokeHandler.ReadParameterBlock();
          if (buffer == null)
            return "Read parameter failed";
          SmokeDetectorVersion version = this.smokeHandler.ReadVersion();
          if (version == null)
            return "Read version failed";
          ManufacturingParameter mp = this.smokeHandler.ReadManufacturingParameter();
          if (mp == null)
            return "Read Manufacturing Parameter failed";
          List<MinoprotectIII_Events> list = new List<MinoprotectIII_Events>();
          for (ushort address = 17; address <= (ushort) 25; ++address)
          {
            List<MinoprotectIII_Events> collection = this.smokeHandler.ReadEventMemory(address);
            if (collection != null)
              list.AddRange((IEnumerable<MinoprotectIII_Events>) collection);
          }
          return !this.WriteDataintoDatasheet(buffer, version, mp, list) ? "Write data failed" : "OK";
        }
        catch (Exception ex)
        {
          throw new Exception(ex.ToString());
        }
      }));
    }

    private void btnSetPath_Click(object sender, EventArgs e)
    {
      string empty = string.Empty;
      string str1 = string.Empty;
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
        str1 = folderBrowserDialog.SelectedPath;
      string[] strArray = new string[8];
      strArray[0] = "SmokerData";
      int num1 = DateTime.Now.Year;
      strArray[1] = num1.ToString();
      DateTime now = DateTime.Now;
      num1 = now.Month;
      strArray[2] = num1.ToString();
      now = DateTime.Now;
      num1 = now.Day;
      strArray[3] = num1.ToString();
      now = DateTime.Now;
      num1 = now.Hour;
      strArray[4] = num1.ToString();
      now = DateTime.Now;
      num1 = now.Minute;
      strArray[5] = num1.ToString();
      now = DateTime.Now;
      num1 = now.Second;
      strArray[6] = num1.ToString();
      strArray[7] = ".xls";
      string str2 = string.Concat(strArray);
      string str3 = str1 + "\\" + str2;
      this.labelPath.Text = str3;
      if (!File.Exists(str3))
      {
        this.CreateExcelSheet(str3);
      }
      else
      {
        int num2 = (int) MessageBox.Show("Data table already exists.", "Remind", MessageBoxButtons.OK);
      }
      this.btnGetData.Enabled = true;
    }

    private void btnSetPathOld_Click(object sender, EventArgs e)
    {
      string str = string.Empty;
      OpenFileDialog openFileDialog1 = new OpenFileDialog();
      openFileDialog1.Filter = "Excel Files (*.xls)|*.xls";
      OpenFileDialog openFileDialog2 = openFileDialog1;
      if (openFileDialog2.ShowDialog() == DialogResult.OK)
        str = openFileDialog2.FileName;
      if (!File.Exists(str))
        return;
      if (this.CheckExcelSheet(str))
      {
        this.labelPath.Text = str;
        this.btnGetData.Enabled = true;
      }
      else
      {
        int num = (int) MessageBox.Show("Set path failed", "Remind", MessageBoxButtons.OK);
      }
    }

    private bool CheckExcelSheet(string savePath)
    {
      try
      {
        this.cn = new OleDbConnection("Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + savePath + ";Extended Properties=Excel 8.0;");
        this.cmd = new OleDbCommand("SELECT * FROM [TestSheet$A1:W1]", this.cn);
        this.cn.Open();
        OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(this.cmd);
        DataTable dataTable = new DataTable();
        oleDbDataAdapter.Fill(dataTable);
        if (dataTable == null || dataTable.Columns[0].ColumnName != "SerialNumber" || dataTable.Columns[22].ColumnName != "Bit15_undefined")
          return false;
        this.cn.Close();
        return true;
      }
      catch
      {
        return false;
      }
    }

    private void CreateExcelSheet(string savePath)
    {
      try
      {
        this.cn = new OleDbConnection("Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + savePath + ";Extended Properties=Excel 8.0;");
        this.cmd = new OleDbCommand("CREATE TABLE TestSheet ([SerialNumber] INTEGER,[MeterID] INTEGER, [FirmwareVersion] VarChar,[HardwareVersion] INTEGER, [NumberSmokeAlarms] INTEGER,[NumberTestAlarms] INTEGER,[Datetime] DateTime,\r\n                                    [BatteryForewarning] VarChar,[BatteryFault] VarChar,[BatteryWarningRadio] VarChar,[TestAlarmReleased] VarChar,[SmokeAlarmReleased] VarChar,\r\n                                    [SmokeChamberPollutionForewarning] VarChar,[SmokeChamberPollutionWarning] VarChar,[PushButtonFailure] VarChar,[HornFailure] VarChar,[RemovingDetection] VarChar,\r\n                                    [IngressAperturesObstructionDetected] VarChar,[ObjectInSurroundingAreaDetected] VarChar,[LED_Failure] VarChar,[Bit13_undefined] VarChar,[Bit14_undefined] VarChar,[Bit15_undefined] VarChar)", this.cn);
        this.cn.Open();
        this.cmd.ExecuteNonQuery();
        this.cn.Close();
      }
      catch (Exception ex)
      {
        throw new Exception(ex.ToString());
      }
    }

    private void InsertNewDataIntoDataSheet(OutputAnalyzeData data)
    {
      try
      {
        this.cmd.CommandText = "INSERT INTO TestSheet VALUES(" + data.SerialNum.ToString() + ",'" + data.MeterID.ToString() + "','" + data.FirmwareVer + "','" + data.HardwareVer.ToString() + "','" + data.NumSmokeAlarm.ToString() + "','" + data.NumTestAlarms.ToString() + "','" + data.OccurDatetime.ToString() + "','" + data.BatteryForewarning + "','" + data.BatteryFault + "','" + data.BatteryWarningRadio + "','" + data.TestAlarmReleased + "','" + data.SmokeAlarmReleased + "','" + data.SmokeChamberPollutionForewarning + "','" + data.SmokeChamberPollutionWarning + "','" + data.PushButtonFailure + "','" + data.HornFailure + "','" + data.RemovingDetection + "','" + data.IngressAperturesObstructionDetected + "','" + data.ObjectInSurroundingAreaDetected + "','" + data.LED_Failure + "','" + data.Bit13_undefined + "','" + data.Bit14_undefined + "','" + data.Bit15_undefined + "')";
        this.cmd.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        throw new Exception(ex.ToString());
      }
    }

    private string GetCountsAndValuesOfSpecifyEvent(
      List<MinoprotectIII_Events> eventsList,
      SmokeDetectorEvent specifyEvent)
    {
      try
      {
        int num = 0;
        string str = string.Empty;
        foreach (MinoprotectIII_Events events in eventsList)
        {
          if (events.EventIdentification == specifyEvent)
          {
            ++num;
            str = str + "/" + events.EventValue.ToString();
          }
        }
        return "Count:" + num.ToString() + " Values:" + str;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.ToString());
      }
    }

    private Dictionary<SmokeDetectorEvent, string> GetCountsAndValuesOfEventOccurOnOneDay(
      List<MinoprotectIII_Events> eventsList)
    {
      try
      {
        Dictionary<SmokeDetectorEvent, string> eventOccurOnOneDay = new Dictionary<SmokeDetectorEvent, string>();
        List<SmokeDetectorEvent> smokeDetectorEventList = new List<SmokeDetectorEvent>();
        foreach (MinoprotectIII_Events events in eventsList)
        {
          if (!smokeDetectorEventList.Contains(events.EventIdentification))
            smokeDetectorEventList.Add(events.EventIdentification);
        }
        foreach (SmokeDetectorEvent smokeDetectorEvent in smokeDetectorEventList)
          eventOccurOnOneDay.Add(smokeDetectorEvent, this.GetCountsAndValuesOfSpecifyEvent(eventsList, smokeDetectorEvent));
        return eventOccurOnOneDay;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.ToString());
      }
    }

    private Dictionary<DateTime, List<MinoprotectIII_Events>> CutEventsList(
      List<MinoprotectIII_Events> eventsList)
    {
      try
      {
        Dictionary<DateTime, List<MinoprotectIII_Events>> dictionary = new Dictionary<DateTime, List<MinoprotectIII_Events>>();
        List<DateTime> dateTimeList = new List<DateTime>();
        foreach (MinoprotectIII_Events events in eventsList)
        {
          if (!dateTimeList.Contains(Convert.ToDateTime((object) events.EventDate)))
            dateTimeList.Add(Convert.ToDateTime((object) events.EventDate));
        }
        foreach (DateTime key in dateTimeList)
        {
          List<MinoprotectIII_Events> minoprotectIiiEventsList = new List<MinoprotectIII_Events>();
          foreach (MinoprotectIII_Events events in eventsList)
          {
            DateTime? eventDate = events.EventDate;
            DateTime dateTime = key;
            if (eventDate.HasValue && (!eventDate.HasValue || eventDate.GetValueOrDefault() == dateTime))
              minoprotectIiiEventsList.Add(events);
          }
          dictionary.Add(key, minoprotectIiiEventsList);
        }
        return dictionary;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.ToString());
      }
    }

    private bool WriteDataintoDatasheet(
      byte[] buffer,
      SmokeDetectorVersion version,
      ManufacturingParameter mp,
      List<MinoprotectIII_Events> list)
    {
      try
      {
        this.cn.Open();
        this.AnalyzeData(new MinoprotectIII()
        {
          Parameter = MinoprotectIII_Parameter.Parse(buffer),
          Version = version,
          ManufacturingParameter = mp,
          EventMemory = list
        });
        this.cn.Close();
        return true;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.ToString());
      }
    }

    private bool AnalyzeData(MinoprotectIII s)
    {
      try
      {
        foreach (KeyValuePair<DateTime, List<MinoprotectIII_Events>> cutEvents in this.CutEventsList(s.EventMemory))
        {
          OutputAnalyzeData data = new OutputAnalyzeData();
          data.SerialNum = s.Version.Serialnumber;
          data.MeterID = s.ManufacturingParameter.MeterID;
          data.FirmwareVer = s.Version.VersionString;
          data.HardwareVer = s.Version.HardwareVersion;
          data.NumSmokeAlarm = s.Parameter.NumberSmokeAlarms;
          data.NumTestAlarms = s.Parameter.NumberTestAlarms;
          data.OccurDatetime = cutEvents.Key;
          foreach (KeyValuePair<SmokeDetectorEvent, string> keyValuePair in this.GetCountsAndValuesOfEventOccurOnOneDay(cutEvents.Value))
          {
            SmokeDetectorEvent key = keyValuePair.Key;
            if ((uint) key <= 256U)
            {
              if ((uint) key <= 16U)
              {
                switch (key)
                {
                  case SmokeDetectorEvent.BatteryForewarning:
                    data.BatteryForewarning = keyValuePair.Value;
                    break;
                  case SmokeDetectorEvent.BatteryFault:
                    data.BatteryFault = keyValuePair.Value;
                    break;
                  case SmokeDetectorEvent.BatteryWarningRadio:
                    data.BatteryWarningRadio = keyValuePair.Value;
                    break;
                  case SmokeDetectorEvent.SmokeChamberPollutionForewarning:
                    data.SmokeChamberPollutionForewarning = keyValuePair.Value;
                    break;
                  case SmokeDetectorEvent.SmokeChamberPollutionWarning:
                    data.SmokeChamberPollutionWarning = keyValuePair.Value;
                    break;
                }
              }
              else if ((uint) key <= 64U)
              {
                if (key != SmokeDetectorEvent.PushButtonFailure)
                {
                  if (key == SmokeDetectorEvent.HornFailure)
                    data.HornFailure = keyValuePair.Value;
                }
                else
                  data.PushButtonFailure = keyValuePair.Value;
              }
              else if (key != SmokeDetectorEvent.RemovingDetection)
              {
                if (key == SmokeDetectorEvent.TestAlarmReleased)
                  data.TestAlarmReleased = keyValuePair.Value;
              }
              else
                data.RemovingDetection = keyValuePair.Value;
            }
            else if ((uint) key <= 2048U)
            {
              switch (key)
              {
                case SmokeDetectorEvent.SmokeAlarmReleased:
                  data.SmokeAlarmReleased = keyValuePair.Value;
                  break;
                case SmokeDetectorEvent.IngressAperturesObstructionDetected:
                  data.IngressAperturesObstructionDetected = keyValuePair.Value;
                  break;
                case SmokeDetectorEvent.ObjectInSurroundingAreaDetected:
                  data.ObjectInSurroundingAreaDetected = keyValuePair.Value;
                  break;
              }
            }
            else if ((uint) key <= 8192U)
            {
              if (key != SmokeDetectorEvent.LED_Failure)
              {
                if (key == SmokeDetectorEvent.Bit13_undefined)
                  data.Bit13_undefined = keyValuePair.Value;
              }
              else
                data.LED_Failure = keyValuePair.Value;
            }
            else if (key != SmokeDetectorEvent.Bit14_undefined)
            {
              if (key == SmokeDetectorEvent.Bit15_undefined)
                data.Bit15_undefined = keyValuePair.Value;
            }
            else
              data.Bit14_undefined = keyValuePair.Value;
          }
          this.InsertNewDataIntoDataSheet(data);
        }
        return true;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.ToString());
      }
    }

    private void btnCheckLedFailure_Click(object sender, EventArgs e)
    {
      try
      {
        this.smokeHandler.StartFunctionTest(FunctionTestMode.LED_Measuring);
        this.smokeHandler.StopFunctionTest();
        Thread.Sleep(1000);
        if (!this.smokeHandler.ReadDevice())
        {
          int num1 = (int) MessageBox.Show("Failed to read device!");
        }
        else if (this.smokeHandler.WorkMeter.Parameter.CurrentStateOfEvents.HasFlag((Enum) SmokeDetectorEvent.LED_Failure))
        {
          int num2 = (int) MessageBox.Show("LED_Failure!");
        }
        else
        {
          int num3 = (int) MessageBox.Show("OK");
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void btnMbus_interval_Click(object sender, EventArgs e)
    {
      try
      {
        this.smokeHandler.Write_Mbus_interval(Convert.ToUInt16(this.txtMbus_interval.Value));
        this.smokeHandler.ResetDevice();
        this.txtOutput.Text = "OK";
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void btnMbus_radio_suppression_days_Click(object sender, EventArgs e)
    {
      try
      {
        this.smokeHandler.Write_Mbus_radio_suppression_days(Convert.ToByte(this.txtMbus_radio_suppression_days.Value));
        this.smokeHandler.ResetDevice();
        this.txtOutput.Text = "OK";
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void btnMbus_nighttime_start_Click(object sender, EventArgs e)
    {
      try
      {
        this.smokeHandler.Write_Mbus_nighttime_start(Convert.ToByte(this.txtMbus_nighttime_start.Value));
        this.smokeHandler.ResetDevice();
        this.txtOutput.Text = "OK";
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void btnMbus_nighttime_stop_Click(object sender, EventArgs e)
    {
      try
      {
        this.smokeHandler.Write_Mbus_nighttime_stop(Convert.ToByte(this.txtMbus_nighttime_stop.Value));
        this.smokeHandler.ResetDevice();
        this.txtOutput.Text = "OK";
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void btnSet15Min_Click(object sender, EventArgs e)
    {
      try
      {
        this.smokeHandler.Write_Mbus_interval((ushort) 900);
        this.smokeHandler.Write_Mbus_radio_suppression_days((byte) 0);
        this.smokeHandler.Write_Mbus_nighttime_start((byte) 0);
        this.smokeHandler.Write_Mbus_nighttime_stop((byte) 0);
        this.smokeHandler.ResetDevice();
        this.txtOutput.Text = "OK";
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void btnSetTestIdentification_Click(object sender, EventArgs e)
    {
      MBusChannelIdentification mci = new MBusChannelIdentification();
      mci.Channel = (byte) 0;
      mci.SerialNumber = 1123378L;
      mci.Manufacturer = "ZRI";
      mci.Generation = (byte) 13;
      mci.Medium = "SMOKE_DETECTOR";
      ProgressHandler progress = new ProgressHandler((Action<ProgressArg>) (x => { }));
      CancellationToken token = new CancellationTokenSource().Token;
      AsyncHelpers.RunSync((Func<Task>) (async () =>
      {
        DeviceVersionMBus deviceVersionMbus = await this.smokeHandler.Device.DeviceCMD.ReadVersionAsync(progress, token);
        await this.smokeHandler.MBus.SetChannelIdentificationAsync(mci, progress, token);
      }));
    }

    public async Task<bool> RunRadioPacketTest(
      ProgressHandler progress,
      CancellationToken cancelToken,
      string comPortOfSecondMicon)
    {
      this.txtOutput.Text = string.Empty;
      Stopwatch watch = Stopwatch.StartNew();
      ZR_ClassLibrary.RadioMode mode = ZR_ClassLibrary.RadioMode.Radio3_868_95;
      uint serialnumber = 11111111;
      byte[] arbitraryData = new byte[28];
      TestCommand.logger.Info("TEST: SD = > MiCon");
      ConnectionProfile profile = ReadoutConfigFunctions.Manager.GetConnectionProfile(32);
      profile.EquipmentModel.ChangeableParameters.Find((Predicate<ChangeableParameter>) (x => x.Key == "Port")).Value = comPortOfSecondMicon;
      CommunicationPortFunctions port = new CommunicationPortFunctions();
      port.SetReadoutConfiguration(profile.GetSettingsList());
      TextBox txtOutput1 = this.txtOutput;
      txtOutput1.Text = txtOutput1.Text + "Open MiCon " + profile.EquipmentModel.ChangeableParameters.Find((Predicate<ChangeableParameter>) (x => x.Key == "Port")).Value + Environment.NewLine + Environment.NewLine;
      port.Open();
      await Task.Delay(500);
      CommunicationByMinoConnect micon = port.GetCommunicationByMinoConnect();
      try
      {
        TextBox txtOutput2 = this.txtOutput;
        txtOutput2.Text = txtOutput2.Text + "Set frequency to 868.95 MHz" + Environment.NewLine;
        await this.smokeHandler.SetCenterFrequencyAsync(progress, cancelToken, 868950000U);
        TestCommand.logger.Info("TEST: MiCon => SD");
        TextBox txtOutput3 = this.txtOutput;
        txtOutput3.Text = txtOutput3.Text + Environment.NewLine + "Start SD receiver (" + (ushort) 10.ToString() + " sec)" + Environment.NewLine;
        Task<double?> resultReceiver2 = this.smokeHandler.ReceiveTestPacketAsync(progress, cancelToken, (byte) 10, serialnumber, "D696");
        TextBox txtOutput4 = this.txtOutput;
        txtOutput4.Text = txtOutput4.Text + "Send test packet via MiCon" + Environment.NewLine;
        int countOfSendPackets = 0;
        await Task.Run((Action) (() =>
        {
          while (!resultReceiver2.IsCompleted)
          {
            try
            {
              micon.SendTestPacket(serialnumber, mode, (byte) 7, "D696");
              ++countOfSendPackets;
              Thread.Sleep(1500);
            }
            catch (Exception ex)
            {
              TestCommand.logger.Error(ex.Message);
            }
          }
        }));
        TextBox txtOutput5 = this.txtOutput;
        txtOutput5.Text = txtOutput5.Text + countOfSendPackets.ToString() + " packets are sends form MiCon" + Environment.NewLine;
        try
        {
          if (resultReceiver2.Result.HasValue)
          {
            TestCommand.logger.Info("Successfully send/receive a TEST packet from MinoConnect to SD");
            TextBox txtOutput6 = this.txtOutput;
            txtOutput6.Text = txtOutput6.Text + "MiCon = > SD OK RSSI: " + resultReceiver2.Result.ToString() + Environment.NewLine;
          }
          else
          {
            TestCommand.logger.Error("Failed send/receive a TEST packet from MinoConnect to SD");
            TextBox txtOutput7 = this.txtOutput;
            txtOutput7.Text = txtOutput7.Text + "MiCon = > SD Error" + Environment.NewLine;
          }
        }
        catch (Exception ex)
        {
          TestCommand.logger.Error("Failed send/receive a TEST packet from MinoConnect to SD");
          TextBox txtOutput8 = this.txtOutput;
          txtOutput8.Text = txtOutput8.Text + "MiCon = > M8 Error: " + ex.Message + Environment.NewLine;
        }
        await Task.Delay(500);
        bool done = false;
        RadioTestResult resultReceiver = (RadioTestResult) null;
        TextBox txtOutput9 = this.txtOutput;
        txtOutput9.Text = txtOutput9.Text + "Start MiCon receiver (" + (ushort) 10.ToString() + " sec)" + Environment.NewLine;
        Task taskReceiver = Task.Run((Action) (() =>
        {
          resultReceiver = micon.ReceiveOnePacket(mode, (int) serialnumber, (ushort) 10, "D696");
          done = true;
        }));
        TextBox txtOutput10 = this.txtOutput;
        txtOutput10.Text = txtOutput10.Text + "Send test packet via SD" + Environment.NewLine;
        while (!done)
        {
          await Task.Delay(500);
          try
          {
            await this.smokeHandler.SendTestPacketAsync(progress, cancelToken, (ushort) 1, (ushort) 1, serialnumber, arbitraryData, "D696");
          }
          catch (Exception ex)
          {
            TestCommand.logger.Error(ex.Message);
          }
        }
        if (resultReceiver != null)
        {
          TestCommand.logger.Info("Successfully send/receive a TEST packet from M8 to MinoConnect. RSSI: " + resultReceiver.RSSI.ToString() + " dBm");
          TextBox txtOutput11 = this.txtOutput;
          txtOutput11.Text = txtOutput11.Text + "SD = > MiCon OK RSSI:" + resultReceiver.RSSI.ToString() + Environment.NewLine;
        }
        else
        {
          TestCommand.logger.Error("Failed send/receive a TEST packet from M8 to MinoConnect");
          TextBox txtOutput12 = this.txtOutput;
          txtOutput12.Text = txtOutput12.Text + "SD = > MiCon Error" + Environment.NewLine;
        }
        taskReceiver = (Task) null;
      }
      finally
      {
        TextBox txtOutput13 = this.txtOutput;
        txtOutput13.Text = txtOutput13.Text + Environment.NewLine + "Close MiCon " + profile.EquipmentModel.ChangeableParameters.Find((Predicate<ChangeableParameter>) (x => x.Key == "Port")).Value + Environment.NewLine;
        port.Close();
      }
      watch.Stop();
      TextBox txtOutput14 = this.txtOutput;
      txtOutput14.Text = txtOutput14.Text + "Elapsed: " + Util.ElapsedToString(watch.Elapsed) + Environment.NewLine;
      bool flag = true;
      watch = (Stopwatch) null;
      arbitraryData = (byte[]) null;
      profile = (ConnectionProfile) null;
      port = (CommunicationPortFunctions) null;
      return flag;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TestCommand));
      this.txtOutput = new TextBox();
      this.statusStrip = new StatusStrip();
      this.progress = new ToolStripProgressBar();
      this.lblStatus = new ToolStripStatusLabel();
      this.lblProgress = new ToolStripStatusLabel();
      this.lblPerformance = new ToolStripStatusLabel();
      this.groupBox2 = new GroupBox();
      this.tabGenerally = new TabPage();
      this.btnSet_A_B = new Button();
      this.btnSet15Min = new Button();
      this.btnMbus_nighttime_stop = new Button();
      this.txtMbus_nighttime_stop = new NumericUpDown();
      this.label6 = new Label();
      this.btnMbus_nighttime_start = new Button();
      this.txtMbus_nighttime_start = new NumericUpDown();
      this.label5 = new Label();
      this.btnMbus_radio_suppression_days = new Button();
      this.txtMbus_radio_suppression_days = new NumericUpDown();
      this.label4 = new Label();
      this.btnMbus_interval = new Button();
      this.txtMbus_interval = new NumericUpDown();
      this.label3 = new Label();
      this.WriteDevEUI = new Button();
      this.btnRadioRadioTest = new Button();
      this.btnTC_ReadSmokeDensityAndSensitivity_once = new Button();
      this.btnTC_ReadSmokeDensityAndSensitivity_90_times = new Button();
      this.txtCValue = new NumericUpDown();
      this.label2 = new Label();
      this.btnTC_WriteSmokeDensityThreshold_C_Value = new Button();
      this.groupBox4 = new GroupBox();
      this.btnStartFunctionTest = new Button();
      this.cboxFunctionTestMode = new ComboBox();
      this.btnStopFunctionTest = new Button();
      this.label18 = new Label();
      this.groupBox3 = new GroupBox();
      this.label1 = new Label();
      this.cboxRadioMode = new ComboBox();
      this.btnStartRadioTest = new Button();
      this.btnStopRadioTest = new Button();
      this.btnTC_ResetDevice = new Button();
      this.btnTC_ButtonFunction = new Button();
      this.btnTC_SetDeliveryState = new Button();
      this.tabs = new TabControl();
      this.tabTestMode = new TabPage();
      this.button2 = new Button();
      this.button1 = new Button();
      this.txtPiezoHsDuration = new NumericUpDown();
      this.btnTC_PiezoAdjustValueHighSound = new Button();
      this.btnTC_ReadSmokeDensityAndSensitivity = new Button();
      this.btnTC_CauseTestAlarm = new Button();
      this.btnTC_EraseEEPROM = new Button();
      this.btnTC_SurroundingAreaMonitoringCheckReceiverTestResult = new Button();
      this.btnTC_SurroundingAreaMonitoringCheckReceiver = new Button();
      this.btnTC_ClearTestRecordT1 = new Button();
      this.groupBox1 = new GroupBox();
      this.txtLeds = new ComboBox();
      this.btnTC_SurroundingAreaMonitoringCheckTransmitter = new Button();
      this.btnTC_ObstructionCheck = new Button();
      this.btnTC_TestData = new Button();
      this.btnTC_EepromState = new Button();
      this.btnTC_ButtonTest = new Button();
      this.btnTC_Check_LED_Battery_Voltage_TemperatureSensor_Piezo = new Button();
      this.btnTC_PiezoTestLowSoundPressure = new Button();
      this.btnTC_PiezoTestHighSoundPressure = new Button();
      this.btnTC_TransmitterLedInSmokeChamberVoltageReferenceTL431 = new Button();
      this.btnTC_ExitTestMode = new Button();
      this.btnTC_EnterTestMode = new Button();
      this.tabDataAnalysis = new TabPage();
      this.btnSetTestIdentification = new Button();
      this.btnCheckLedFailure = new Button();
      this.btnReadAesKey = new Button();
      this.btnWriteAesKey = new Button();
      this.btnSetPathOld = new Button();
      this.labelPath = new Label();
      this.btnSetPath = new Button();
      this.btnGetData = new Button();
      this.groupBox5 = new GroupBox();
      this.label7 = new Label();
      this.txtNear1 = new NumericUpDown();
      this.txtNear2 = new NumericUpDown();
      this.label8 = new Label();
      this.txtNear3 = new NumericUpDown();
      this.label9 = new Label();
      this.txtNear4 = new NumericUpDown();
      this.label10 = new Label();
      this.txtNear5 = new NumericUpDown();
      this.label11 = new Label();
      this.txtNear6 = new NumericUpDown();
      this.label12 = new Label();
      this.statusStrip.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.tabGenerally.SuspendLayout();
      this.txtMbus_nighttime_stop.BeginInit();
      this.txtMbus_nighttime_start.BeginInit();
      this.txtMbus_radio_suppression_days.BeginInit();
      this.txtMbus_interval.BeginInit();
      this.txtCValue.BeginInit();
      this.groupBox4.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.tabs.SuspendLayout();
      this.tabTestMode.SuspendLayout();
      this.txtPiezoHsDuration.BeginInit();
      this.groupBox1.SuspendLayout();
      this.tabDataAnalysis.SuspendLayout();
      this.groupBox5.SuspendLayout();
      this.txtNear1.BeginInit();
      this.txtNear2.BeginInit();
      this.txtNear3.BeginInit();
      this.txtNear4.BeginInit();
      this.txtNear5.BeginInit();
      this.txtNear6.BeginInit();
      this.SuspendLayout();
      this.txtOutput.Dock = DockStyle.Fill;
      this.txtOutput.Font = new Font("Consolas", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtOutput.Location = new Point(3, 16);
      this.txtOutput.Multiline = true;
      this.txtOutput.Name = "txtOutput";
      this.txtOutput.ScrollBars = ScrollBars.Vertical;
      this.txtOutput.Size = new Size(805, 158);
      this.txtOutput.TabIndex = 20;
      this.statusStrip.Items.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.progress,
        (ToolStripItem) this.lblStatus,
        (ToolStripItem) this.lblProgress,
        (ToolStripItem) this.lblPerformance
      });
      this.statusStrip.Location = new Point(0, 540);
      this.statusStrip.Name = "statusStrip";
      this.statusStrip.Size = new Size(822, 22);
      this.statusStrip.TabIndex = 21;
      this.statusStrip.Text = "statusStrip1";
      this.progress.Name = "progress";
      this.progress.Size = new Size(100, 16);
      this.progress.Step = 1;
      this.progress.Visible = false;
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new Size(46, 17);
      this.lblStatus.Text = "{status}";
      this.lblProgress.Name = "lblProgress";
      this.lblProgress.Size = new Size(60, 17);
      this.lblProgress.Text = "{progress}";
      this.lblPerformance.Name = "lblPerformance";
      this.lblPerformance.Size = new Size(83, 17);
      this.lblPerformance.Text = "{performance}";
      this.groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox2.Controls.Add((Control) this.txtOutput);
      this.groupBox2.Location = new Point(3, 361);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(811, 177);
      this.groupBox2.TabIndex = 32;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Result";
      this.tabGenerally.Controls.Add((Control) this.btnSet_A_B);
      this.tabGenerally.Controls.Add((Control) this.btnSet15Min);
      this.tabGenerally.Controls.Add((Control) this.btnMbus_nighttime_stop);
      this.tabGenerally.Controls.Add((Control) this.txtMbus_nighttime_stop);
      this.tabGenerally.Controls.Add((Control) this.label6);
      this.tabGenerally.Controls.Add((Control) this.btnMbus_nighttime_start);
      this.tabGenerally.Controls.Add((Control) this.txtMbus_nighttime_start);
      this.tabGenerally.Controls.Add((Control) this.label5);
      this.tabGenerally.Controls.Add((Control) this.btnMbus_radio_suppression_days);
      this.tabGenerally.Controls.Add((Control) this.txtMbus_radio_suppression_days);
      this.tabGenerally.Controls.Add((Control) this.label4);
      this.tabGenerally.Controls.Add((Control) this.btnMbus_interval);
      this.tabGenerally.Controls.Add((Control) this.txtMbus_interval);
      this.tabGenerally.Controls.Add((Control) this.label3);
      this.tabGenerally.Controls.Add((Control) this.WriteDevEUI);
      this.tabGenerally.Controls.Add((Control) this.btnRadioRadioTest);
      this.tabGenerally.Controls.Add((Control) this.btnTC_ReadSmokeDensityAndSensitivity_once);
      this.tabGenerally.Controls.Add((Control) this.btnTC_ReadSmokeDensityAndSensitivity_90_times);
      this.tabGenerally.Controls.Add((Control) this.txtCValue);
      this.tabGenerally.Controls.Add((Control) this.label2);
      this.tabGenerally.Controls.Add((Control) this.btnTC_WriteSmokeDensityThreshold_C_Value);
      this.tabGenerally.Controls.Add((Control) this.groupBox4);
      this.tabGenerally.Controls.Add((Control) this.groupBox3);
      this.tabGenerally.Controls.Add((Control) this.btnTC_ResetDevice);
      this.tabGenerally.Controls.Add((Control) this.btnTC_ButtonFunction);
      this.tabGenerally.Controls.Add((Control) this.btnTC_SetDeliveryState);
      this.tabGenerally.Location = new Point(4, 22);
      this.tabGenerally.Name = "tabGenerally";
      this.tabGenerally.Padding = new Padding(3);
      this.tabGenerally.Size = new Size(803, 326);
      this.tabGenerally.TabIndex = 0;
      this.tabGenerally.Text = "Generally";
      this.tabGenerally.UseVisualStyleBackColor = true;
      this.btnSet_A_B.Location = new Point(212, 93);
      this.btnSet_A_B.Name = "btnSet_A_B";
      this.btnSet_A_B.Size = new Size(94, 23);
      this.btnSet_A_B.TabIndex = 102;
      this.btnSet_A_B.Text = "Set A, B";
      this.btnSet_A_B.UseVisualStyleBackColor = true;
      this.btnSet_A_B.Click += new System.EventHandler(this.btnSet_A_B_Click);
      this.btnSet15Min.Location = new Point(574, 220);
      this.btnSet15Min.Name = "btnSet15Min";
      this.btnSet15Min.Size = new Size(161, 76);
      this.btnSet15Min.TabIndex = 101;
      this.btnSet15Min.Text = "Write '15 -min'";
      this.btnSet15Min.UseVisualStyleBackColor = true;
      this.btnSet15Min.Click += new System.EventHandler(this.btnSet15Min_Click);
      this.btnMbus_nighttime_stop.Location = new Point(708, 182);
      this.btnMbus_nighttime_stop.Name = "btnMbus_nighttime_stop";
      this.btnMbus_nighttime_stop.Size = new Size(47, 23);
      this.btnMbus_nighttime_stop.TabIndex = 100;
      this.btnMbus_nighttime_stop.Text = "Write";
      this.btnMbus_nighttime_stop.UseVisualStyleBackColor = true;
      this.btnMbus_nighttime_stop.Click += new System.EventHandler(this.btnMbus_nighttime_stop_Click);
      this.txtMbus_nighttime_stop.Location = new Point(644, 185);
      this.txtMbus_nighttime_stop.Maximum = new Decimal(new int[4]
      {
        23,
        0,
        0,
        0
      });
      this.txtMbus_nighttime_stop.Name = "txtMbus_nighttime_stop";
      this.txtMbus_nighttime_stop.Size = new Size(49, 20);
      this.txtMbus_nighttime_stop.TabIndex = 99;
      this.txtMbus_nighttime_stop.Value = new Decimal(new int[4]
      {
        5,
        0,
        0,
        0
      });
      this.label6.AutoSize = true;
      this.label6.Location = new Point(571, 167);
      this.label6.Name = "label6";
      this.label6.Size = new Size(110, 13);
      this.label6.TabIndex = 98;
      this.label6.Text = "Mbus_nighttime_stop:";
      this.btnMbus_nighttime_start.Location = new Point(708, 134);
      this.btnMbus_nighttime_start.Name = "btnMbus_nighttime_start";
      this.btnMbus_nighttime_start.Size = new Size(47, 23);
      this.btnMbus_nighttime_start.TabIndex = 97;
      this.btnMbus_nighttime_start.Text = "Write";
      this.btnMbus_nighttime_start.UseVisualStyleBackColor = true;
      this.btnMbus_nighttime_start.Click += new System.EventHandler(this.btnMbus_nighttime_start_Click);
      this.txtMbus_nighttime_start.Location = new Point(644, 137);
      this.txtMbus_nighttime_start.Maximum = new Decimal(new int[4]
      {
        23,
        0,
        0,
        0
      });
      this.txtMbus_nighttime_start.Name = "txtMbus_nighttime_start";
      this.txtMbus_nighttime_start.Size = new Size(49, 20);
      this.txtMbus_nighttime_start.TabIndex = 96;
      this.txtMbus_nighttime_start.Value = new Decimal(new int[4]
      {
        23,
        0,
        0,
        0
      });
      this.label5.AutoSize = true;
      this.label5.Location = new Point(571, 119);
      this.label5.Name = "label5";
      this.label5.Size = new Size(110, 13);
      this.label5.TabIndex = 95;
      this.label5.Text = "Mbus_nighttime_start:";
      this.btnMbus_radio_suppression_days.Location = new Point(708, 85);
      this.btnMbus_radio_suppression_days.Name = "btnMbus_radio_suppression_days";
      this.btnMbus_radio_suppression_days.Size = new Size(47, 23);
      this.btnMbus_radio_suppression_days.TabIndex = 94;
      this.btnMbus_radio_suppression_days.Text = "Write";
      this.btnMbus_radio_suppression_days.UseVisualStyleBackColor = true;
      this.btnMbus_radio_suppression_days.Click += new System.EventHandler(this.btnMbus_radio_suppression_days_Click);
      this.txtMbus_radio_suppression_days.Location = new Point(644, 85);
      this.txtMbus_radio_suppression_days.Maximum = new Decimal(new int[4]
      {
        (int) sbyte.MaxValue,
        0,
        0,
        0
      });
      this.txtMbus_radio_suppression_days.Name = "txtMbus_radio_suppression_days";
      this.txtMbus_radio_suppression_days.Size = new Size(49, 20);
      this.txtMbus_radio_suppression_days.TabIndex = 93;
      this.txtMbus_radio_suppression_days.Value = new Decimal(new int[4]
      {
        64,
        0,
        0,
        0
      });
      this.label4.AutoSize = true;
      this.label4.Location = new Point(571, 69);
      this.label4.Name = "label4";
      this.label4.Size = new Size(155, 13);
      this.label4.TabIndex = 92;
      this.label4.Text = "Mbus_radio_suppression_days:";
      this.btnMbus_interval.Location = new Point(708, 32);
      this.btnMbus_interval.Name = "btnMbus_interval";
      this.btnMbus_interval.Size = new Size(47, 23);
      this.btnMbus_interval.TabIndex = 91;
      this.btnMbus_interval.Text = "Write";
      this.btnMbus_interval.UseVisualStyleBackColor = true;
      this.btnMbus_interval.Click += new System.EventHandler(this.btnMbus_interval_Click);
      this.txtMbus_interval.Increment = new Decimal(new int[4]
      {
        2,
        0,
        0,
        0
      });
      this.txtMbus_interval.Location = new Point(644, 35);
      this.txtMbus_interval.Maximum = new Decimal(new int[4]
      {
        5000,
        0,
        0,
        0
      });
      this.txtMbus_interval.Minimum = new Decimal(new int[4]
      {
        10,
        0,
        0,
        0
      });
      this.txtMbus_interval.Name = "txtMbus_interval";
      this.txtMbus_interval.Size = new Size(49, 20);
      this.txtMbus_interval.TabIndex = 90;
      this.txtMbus_interval.Value = new Decimal(new int[4]
      {
        180,
        0,
        0,
        0
      });
      this.label3.AutoSize = true;
      this.label3.Location = new Point(571, 37);
      this.label3.Name = "label3";
      this.label3.Size = new Size(76, 13);
      this.label3.TabIndex = 89;
      this.label3.Text = "Mbus_interval:";
      this.WriteDevEUI.Location = new Point(294, 293);
      this.WriteDevEUI.Name = "WriteDevEUI";
      this.WriteDevEUI.Size = new Size(271, 23);
      this.WriteDevEUI.TabIndex = 88;
      this.WriteDevEUI.Text = "Write DevEUI 04B6480B01119481";
      this.WriteDevEUI.UseVisualStyleBackColor = true;
      this.WriteDevEUI.Click += new System.EventHandler(this.WriteDevEUI_Click);
      this.btnRadioRadioTest.Location = new Point(6, 293);
      this.btnRadioRadioTest.Name = "btnRadioRadioTest";
      this.btnRadioRadioTest.Size = new Size(282, 23);
      this.btnRadioRadioTest.TabIndex = 87;
      this.btnRadioRadioTest.Text = "Radio Test (MiCon->SDLoRa, SDLoRa->MiCon)";
      this.btnRadioRadioTest.UseVisualStyleBackColor = true;
      this.btnRadioRadioTest.Click += new System.EventHandler(this.btnRadioRadioTest_Click);
      this.btnTC_ReadSmokeDensityAndSensitivity_once.Location = new Point(212, 64);
      this.btnTC_ReadSmokeDensityAndSensitivity_once.Name = "btnTC_ReadSmokeDensityAndSensitivity_once";
      this.btnTC_ReadSmokeDensityAndSensitivity_once.Size = new Size(353, 23);
      this.btnTC_ReadSmokeDensityAndSensitivity_once.TabIndex = 86;
      this.btnTC_ReadSmokeDensityAndSensitivity_once.Text = "Read smoke density and sensitivity (A,B,C) once";
      this.btnTC_ReadSmokeDensityAndSensitivity_once.UseVisualStyleBackColor = true;
      this.btnTC_ReadSmokeDensityAndSensitivity_once.Click += new System.EventHandler(this.btnTC_ReadSmokeDensityAndSensitivity_once_Click);
      this.btnTC_ReadSmokeDensityAndSensitivity_90_times.Location = new Point(212, 36);
      this.btnTC_ReadSmokeDensityAndSensitivity_90_times.Name = "btnTC_ReadSmokeDensityAndSensitivity_90_times";
      this.btnTC_ReadSmokeDensityAndSensitivity_90_times.Size = new Size(353, 23);
      this.btnTC_ReadSmokeDensityAndSensitivity_90_times.TabIndex = 85;
      this.btnTC_ReadSmokeDensityAndSensitivity_90_times.Text = "Read smoke density and sensitivity (A,B,C) 90 times";
      this.btnTC_ReadSmokeDensityAndSensitivity_90_times.UseVisualStyleBackColor = true;
      this.btnTC_ReadSmokeDensityAndSensitivity_90_times.Click += new System.EventHandler(this.btnTC_ReadSmokeDensityAndSensitivity_90_times_Click);
      this.txtCValue.Location = new Point((int) byte.MaxValue, 9);
      this.txtCValue.Maximum = new Decimal(new int[4]
      {
        256,
        0,
        0,
        0
      });
      this.txtCValue.Name = "txtCValue";
      this.txtCValue.Size = new Size(49, 20);
      this.txtCValue.TabIndex = 84;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(209, 11);
      this.label2.Name = "label2";
      this.label2.Size = new Size(46, 13);
      this.label2.TabIndex = 83;
      this.label2.Text = "C value:";
      this.btnTC_WriteSmokeDensityThreshold_C_Value.Location = new Point(310, 8);
      this.btnTC_WriteSmokeDensityThreshold_C_Value.Name = "btnTC_WriteSmokeDensityThreshold_C_Value";
      this.btnTC_WriteSmokeDensityThreshold_C_Value.Size = new Size(150, 23);
      this.btnTC_WriteSmokeDensityThreshold_C_Value.TabIndex = 82;
      this.btnTC_WriteSmokeDensityThreshold_C_Value.Text = "Set C value (Calibration)";
      this.btnTC_WriteSmokeDensityThreshold_C_Value.UseVisualStyleBackColor = true;
      this.btnTC_WriteSmokeDensityThreshold_C_Value.Click += new System.EventHandler(this.btnTC_WriteSmokeDensityThreshold_C_Value_Click);
      this.groupBox4.Controls.Add((Control) this.btnStartFunctionTest);
      this.groupBox4.Controls.Add((Control) this.cboxFunctionTestMode);
      this.groupBox4.Controls.Add((Control) this.btnStopFunctionTest);
      this.groupBox4.Controls.Add((Control) this.label18);
      this.groupBox4.Location = new Point(6, 205);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new Size(559, 73);
      this.groupBox4.TabIndex = 81;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Function test";
      this.btnStartFunctionTest.Location = new Point(383, 30);
      this.btnStartFunctionTest.Name = "btnStartFunctionTest";
      this.btnStartFunctionTest.Size = new Size(75, 23);
      this.btnStartFunctionTest.TabIndex = 80;
      this.btnStartFunctionTest.Text = "Start";
      this.btnStartFunctionTest.UseVisualStyleBackColor = true;
      this.btnStartFunctionTest.Click += new System.EventHandler(this.btnStartFunctionTest_Click);
      this.cboxFunctionTestMode.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxFunctionTestMode.FormattingEnabled = true;
      this.cboxFunctionTestMode.Location = new Point(88, 31);
      this.cboxFunctionTestMode.Name = "cboxFunctionTestMode";
      this.cboxFunctionTestMode.Size = new Size(280, 21);
      this.cboxFunctionTestMode.TabIndex = 76;
      this.btnStopFunctionTest.Location = new Point(464, 30);
      this.btnStopFunctionTest.Name = "btnStopFunctionTest";
      this.btnStopFunctionTest.Size = new Size(75, 23);
      this.btnStopFunctionTest.TabIndex = 81;
      this.btnStopFunctionTest.Text = "Stop";
      this.btnStopFunctionTest.UseVisualStyleBackColor = true;
      this.btnStopFunctionTest.Click += new System.EventHandler(this.btnStopFunctionTest_Click);
      this.label18.Location = new Point(20, 35);
      this.label18.Name = "label18";
      this.label18.Size = new Size(63, 15);
      this.label18.TabIndex = 77;
      this.label18.Tag = (object) "";
      this.label18.Text = "Function:";
      this.label18.TextAlign = ContentAlignment.MiddleRight;
      this.groupBox3.Controls.Add((Control) this.label1);
      this.groupBox3.Controls.Add((Control) this.cboxRadioMode);
      this.groupBox3.Controls.Add((Control) this.btnStartRadioTest);
      this.groupBox3.Controls.Add((Control) this.btnStopRadioTest);
      this.groupBox3.Location = new Point(6, 125);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(559, 74);
      this.groupBox3.TabIndex = 80;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Radio test";
      this.label1.Location = new Point(23, 28);
      this.label1.Name = "label1";
      this.label1.Size = new Size(59, 15);
      this.label1.TabIndex = 79;
      this.label1.Tag = (object) "";
      this.label1.Text = "Mode:";
      this.label1.TextAlign = ContentAlignment.MiddleRight;
      this.cboxRadioMode.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboxRadioMode.FormattingEnabled = true;
      this.cboxRadioMode.Location = new Point(88, 28);
      this.cboxRadioMode.Name = "cboxRadioMode";
      this.cboxRadioMode.Size = new Size(280, 21);
      this.cboxRadioMode.TabIndex = 72;
      this.btnStartRadioTest.Location = new Point(383, 28);
      this.btnStartRadioTest.Name = "btnStartRadioTest";
      this.btnStartRadioTest.Size = new Size(75, 23);
      this.btnStartRadioTest.TabIndex = 74;
      this.btnStartRadioTest.Text = "Start";
      this.btnStartRadioTest.UseVisualStyleBackColor = true;
      this.btnStartRadioTest.Click += new System.EventHandler(this.btnStartRadioTest_Click);
      this.btnStopRadioTest.Location = new Point(464, 28);
      this.btnStopRadioTest.Name = "btnStopRadioTest";
      this.btnStopRadioTest.Size = new Size(75, 23);
      this.btnStopRadioTest.TabIndex = 75;
      this.btnStopRadioTest.Text = "Stop";
      this.btnStopRadioTest.UseVisualStyleBackColor = true;
      this.btnStopRadioTest.Click += new System.EventHandler(this.btnStopRadioTest_Click);
      this.btnTC_ResetDevice.Location = new Point(6, 64);
      this.btnTC_ResetDevice.Name = "btnTC_ResetDevice";
      this.btnTC_ResetDevice.Size = new Size(174, 23);
      this.btnTC_ResetDevice.TabIndex = 20;
      this.btnTC_ResetDevice.Text = "Reset device";
      this.btnTC_ResetDevice.UseVisualStyleBackColor = true;
      this.btnTC_ResetDevice.Click += new System.EventHandler(this.btnTC_ResetDevice_Click);
      this.btnTC_ButtonFunction.Location = new Point(6, 35);
      this.btnTC_ButtonFunction.Name = "btnTC_ButtonFunction";
      this.btnTC_ButtonFunction.Size = new Size(174, 23);
      this.btnTC_ButtonFunction.TabIndex = 19;
      this.btnTC_ButtonFunction.Text = "Button function";
      this.btnTC_ButtonFunction.UseVisualStyleBackColor = true;
      this.btnTC_ButtonFunction.Click += new System.EventHandler(this.btnTC_ButtonFunction_Click);
      this.btnTC_SetDeliveryState.Location = new Point(6, 5);
      this.btnTC_SetDeliveryState.Name = "btnTC_SetDeliveryState";
      this.btnTC_SetDeliveryState.Size = new Size(174, 23);
      this.btnTC_SetDeliveryState.TabIndex = 6;
      this.btnTC_SetDeliveryState.Text = "Set delivery state";
      this.btnTC_SetDeliveryState.UseVisualStyleBackColor = true;
      this.btnTC_SetDeliveryState.Click += new System.EventHandler(this.btnTC_SetDeliveryState_Click);
      this.tabs.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.tabs.Controls.Add((Control) this.tabGenerally);
      this.tabs.Controls.Add((Control) this.tabTestMode);
      this.tabs.Controls.Add((Control) this.tabDataAnalysis);
      this.tabs.Location = new Point(3, 3);
      this.tabs.Name = "tabs";
      this.tabs.SelectedIndex = 0;
      this.tabs.Size = new Size(811, 352);
      this.tabs.TabIndex = 33;
      this.tabTestMode.Controls.Add((Control) this.groupBox5);
      this.tabTestMode.Controls.Add((Control) this.txtPiezoHsDuration);
      this.tabTestMode.Controls.Add((Control) this.btnTC_PiezoAdjustValueHighSound);
      this.tabTestMode.Controls.Add((Control) this.btnTC_ReadSmokeDensityAndSensitivity);
      this.tabTestMode.Controls.Add((Control) this.btnTC_CauseTestAlarm);
      this.tabTestMode.Controls.Add((Control) this.btnTC_EraseEEPROM);
      this.tabTestMode.Controls.Add((Control) this.btnTC_SurroundingAreaMonitoringCheckReceiverTestResult);
      this.tabTestMode.Controls.Add((Control) this.btnTC_SurroundingAreaMonitoringCheckReceiver);
      this.tabTestMode.Controls.Add((Control) this.btnTC_ClearTestRecordT1);
      this.tabTestMode.Controls.Add((Control) this.groupBox1);
      this.tabTestMode.Controls.Add((Control) this.btnTC_ObstructionCheck);
      this.tabTestMode.Controls.Add((Control) this.btnTC_TestData);
      this.tabTestMode.Controls.Add((Control) this.btnTC_EepromState);
      this.tabTestMode.Controls.Add((Control) this.btnTC_ButtonTest);
      this.tabTestMode.Controls.Add((Control) this.btnTC_Check_LED_Battery_Voltage_TemperatureSensor_Piezo);
      this.tabTestMode.Controls.Add((Control) this.btnTC_PiezoTestLowSoundPressure);
      this.tabTestMode.Controls.Add((Control) this.btnTC_PiezoTestHighSoundPressure);
      this.tabTestMode.Controls.Add((Control) this.btnTC_TransmitterLedInSmokeChamberVoltageReferenceTL431);
      this.tabTestMode.Controls.Add((Control) this.btnTC_ExitTestMode);
      this.tabTestMode.Controls.Add((Control) this.btnTC_EnterTestMode);
      this.tabTestMode.Location = new Point(4, 22);
      this.tabTestMode.Name = "tabTestMode";
      this.tabTestMode.Size = new Size(803, 326);
      this.tabTestMode.TabIndex = 2;
      this.tabTestMode.Text = "Functions for Test Mode";
      this.tabTestMode.UseVisualStyleBackColor = true;
      this.button2.Location = new Point(401, 10);
      this.button2.Name = "button2";
      this.button2.Size = new Size(169, 23);
      this.button2.TabIndex = 30;
      this.button2.Text = "Obstruction Calibration Read";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.btnTC_ObstructionCalibrationRead);
      this.button1.Location = new Point(401, 37);
      this.button1.Name = "button1";
      this.button1.Size = new Size(169, 23);
      this.button1.TabIndex = 29;
      this.button1.Text = "Obstruction Calibration Write";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.btnTC_ObstructionCalibrationWrite);
      this.txtPiezoHsDuration.Location = new Point(13, 83);
      this.txtPiezoHsDuration.Maximum = new Decimal(new int[4]
      {
        256,
        0,
        0,
        0
      });
      this.txtPiezoHsDuration.Name = "txtPiezoHsDuration";
      this.txtPiezoHsDuration.Size = new Size(46, 20);
      this.txtPiezoHsDuration.TabIndex = 28;
      this.btnTC_PiezoAdjustValueHighSound.Location = new Point(239, 82);
      this.btnTC_PiezoAdjustValueHighSound.Name = "btnTC_PiezoAdjustValueHighSound";
      this.btnTC_PiezoAdjustValueHighSound.Size = new Size(168, 23);
      this.btnTC_PiezoAdjustValueHighSound.TabIndex = 27;
      this.btnTC_PiezoAdjustValueHighSound.Text = "Piezo (high sound) adjust value";
      this.btnTC_PiezoAdjustValueHighSound.UseVisualStyleBackColor = true;
      this.btnTC_PiezoAdjustValueHighSound.Click += new System.EventHandler(this.btnTC_PiezoAdjustValueHighSound_Click);
      this.btnTC_ReadSmokeDensityAndSensitivity.Location = new Point(426, 230);
      this.btnTC_ReadSmokeDensityAndSensitivity.Name = "btnTC_ReadSmokeDensityAndSensitivity";
      this.btnTC_ReadSmokeDensityAndSensitivity.Size = new Size(365, 23);
      this.btnTC_ReadSmokeDensityAndSensitivity.TabIndex = 26;
      this.btnTC_ReadSmokeDensityAndSensitivity.Text = "Read smoke density and sensitivity (A,B,C)";
      this.btnTC_ReadSmokeDensityAndSensitivity.UseVisualStyleBackColor = true;
      this.btnTC_ReadSmokeDensityAndSensitivity.Click += new System.EventHandler(this.btnTC_ReadSmokeDensityAndSensitivity_Click);
      this.btnTC_CauseTestAlarm.Location = new Point(13, 286);
      this.btnTC_CauseTestAlarm.Name = "btnTC_CauseTestAlarm";
      this.btnTC_CauseTestAlarm.Size = new Size(196, 23);
      this.btnTC_CauseTestAlarm.TabIndex = 25;
      this.btnTC_CauseTestAlarm.Text = "Cause a test alarm";
      this.btnTC_CauseTestAlarm.UseVisualStyleBackColor = true;
      this.btnTC_CauseTestAlarm.Click += new System.EventHandler(this.btnTC_CauseTestAlarm_Click);
      this.btnTC_EraseEEPROM.Location = new Point(11, 229);
      this.btnTC_EraseEEPROM.Name = "btnTC_EraseEEPROM";
      this.btnTC_EraseEEPROM.Size = new Size(198, 23);
      this.btnTC_EraseEEPROM.TabIndex = 24;
      this.btnTC_EraseEEPROM.Text = "Erase all events in the EEPROM";
      this.btnTC_EraseEEPROM.UseVisualStyleBackColor = true;
      this.btnTC_EraseEEPROM.Click += new System.EventHandler(this.btnTC_EraseEEPROM_Click);
      this.btnTC_SurroundingAreaMonitoringCheckReceiverTestResult.Location = new Point(426, 199);
      this.btnTC_SurroundingAreaMonitoringCheckReceiverTestResult.Name = "btnTC_SurroundingAreaMonitoringCheckReceiverTestResult";
      this.btnTC_SurroundingAreaMonitoringCheckReceiverTestResult.Size = new Size(365, 23);
      this.btnTC_SurroundingAreaMonitoringCheckReceiverTestResult.TabIndex = 23;
      this.btnTC_SurroundingAreaMonitoringCheckReceiverTestResult.Text = "Surrounding area monitoring check. Inquire the test result of the receiver.";
      this.btnTC_SurroundingAreaMonitoringCheckReceiverTestResult.UseVisualStyleBackColor = true;
      this.btnTC_SurroundingAreaMonitoringCheckReceiverTestResult.Click += new System.EventHandler(this.btnTC_SurroundingAreaMonitoringCheckReceiverTestResult_Click);
      this.btnTC_SurroundingAreaMonitoringCheckReceiver.Location = new Point(426, 171);
      this.btnTC_SurroundingAreaMonitoringCheckReceiver.Name = "btnTC_SurroundingAreaMonitoringCheckReceiver";
      this.btnTC_SurroundingAreaMonitoringCheckReceiver.Size = new Size(365, 23);
      this.btnTC_SurroundingAreaMonitoringCheckReceiver.TabIndex = 22;
      this.btnTC_SurroundingAreaMonitoringCheckReceiver.Text = "Surrounding area monitoring check receiver";
      this.btnTC_SurroundingAreaMonitoringCheckReceiver.UseVisualStyleBackColor = true;
      this.btnTC_SurroundingAreaMonitoringCheckReceiver.Click += new System.EventHandler(this.btnTC_SurroundingAreaMonitoringCheckReceiver_Click);
      this.btnTC_ClearTestRecordT1.Location = new Point(426, 83);
      this.btnTC_ClearTestRecordT1.Name = "btnTC_ClearTestRecordT1";
      this.btnTC_ClearTestRecordT1.Size = new Size(365, 23);
      this.btnTC_ClearTestRecordT1.TabIndex = 21;
      this.btnTC_ClearTestRecordT1.Text = "Clear test record T1";
      this.btnTC_ClearTestRecordT1.UseVisualStyleBackColor = true;
      this.btnTC_ClearTestRecordT1.Click += new System.EventHandler(this.btnTC_ClearTestRecordT1_Click);
      this.groupBox1.Controls.Add((Control) this.txtLeds);
      this.groupBox1.Controls.Add((Control) this.btnTC_SurroundingAreaMonitoringCheckTransmitter);
      this.groupBox1.Location = new Point(426, 114);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(365, 51);
      this.groupBox1.TabIndex = 20;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Surrounding area monitoring check";
      this.txtLeds.DropDownStyle = ComboBoxStyle.DropDownList;
      this.txtLeds.FormattingEnabled = true;
      this.txtLeds.Location = new Point(6, 19);
      this.txtLeds.Name = "txtLeds";
      this.txtLeds.Size = new Size(88, 21);
      this.txtLeds.TabIndex = 13;
      this.btnTC_SurroundingAreaMonitoringCheckTransmitter.Location = new Point(100, 17);
      this.btnTC_SurroundingAreaMonitoringCheckTransmitter.Name = "btnTC_SurroundingAreaMonitoringCheckTransmitter";
      this.btnTC_SurroundingAreaMonitoringCheckTransmitter.Size = new Size(227, 23);
      this.btnTC_SurroundingAreaMonitoringCheckTransmitter.TabIndex = 12;
      this.btnTC_SurroundingAreaMonitoringCheckTransmitter.Text = "Check transmitter";
      this.btnTC_SurroundingAreaMonitoringCheckTransmitter.UseVisualStyleBackColor = true;
      this.btnTC_SurroundingAreaMonitoringCheckTransmitter.Click += new System.EventHandler(this.btnTC_SurroundingAreaMonitoringCheckTransmitter_Click);
      this.btnTC_ObstructionCheck.Location = new Point(13, 257);
      this.btnTC_ObstructionCheck.Name = "btnTC_ObstructionCheck";
      this.btnTC_ObstructionCheck.Size = new Size(196, 23);
      this.btnTC_ObstructionCheck.TabIndex = 19;
      this.btnTC_ObstructionCheck.Text = "Obstruction check";
      this.btnTC_ObstructionCheck.UseVisualStyleBackColor = true;
      this.btnTC_ObstructionCheck.Click += new System.EventHandler(this.btnTC_ObstructionCheck_Click);
      this.btnTC_TestData.Location = new Point(426, 55);
      this.btnTC_TestData.Name = "btnTC_TestData";
      this.btnTC_TestData.Size = new Size(365, 23);
      this.btnTC_TestData.TabIndex = 18;
      this.btnTC_TestData.Text = "Read test record T1";
      this.btnTC_TestData.UseVisualStyleBackColor = true;
      this.btnTC_TestData.Click += new System.EventHandler(this.btnTC_TestData_Click);
      this.btnTC_EepromState.Location = new Point(13, 198);
      this.btnTC_EepromState.Name = "btnTC_EepromState";
      this.btnTC_EepromState.Size = new Size(196, 23);
      this.btnTC_EepromState.TabIndex = 17;
      this.btnTC_EepromState.Text = "EEPROM test";
      this.btnTC_EepromState.UseVisualStyleBackColor = true;
      this.btnTC_EepromState.Click += new System.EventHandler(this.btnTC_EepromState_Click);
      this.btnTC_ButtonTest.Location = new Point(13, 170);
      this.btnTC_ButtonTest.Name = "btnTC_ButtonTest";
      this.btnTC_ButtonTest.Size = new Size(394, 23);
      this.btnTC_ButtonTest.TabIndex = 16;
      this.btnTC_ButtonTest.Text = "Button test (Note: Push the button before sending the test command)";
      this.btnTC_ButtonTest.UseVisualStyleBackColor = true;
      this.btnTC_ButtonTest.Click += new System.EventHandler(this.btnTC_ButtonTest_Click);
      this.btnTC_Check_LED_Battery_Voltage_TemperatureSensor_Piezo.Location = new Point(13, 141);
      this.btnTC_Check_LED_Battery_Voltage_TemperatureSensor_Piezo.Name = "btnTC_Check_LED_Battery_Voltage_TemperatureSensor_Piezo";
      this.btnTC_Check_LED_Battery_Voltage_TemperatureSensor_Piezo.Size = new Size(394, 23);
      this.btnTC_Check_LED_Battery_Voltage_TemperatureSensor_Piezo.TabIndex = 15;
      this.btnTC_Check_LED_Battery_Voltage_TemperatureSensor_Piezo.Text = "Check LED, battery voltage , temperature sensor and piezo ";
      this.btnTC_Check_LED_Battery_Voltage_TemperatureSensor_Piezo.UseVisualStyleBackColor = true;
      this.btnTC_Check_LED_Battery_Voltage_TemperatureSensor_Piezo.Click += new System.EventHandler(this.btnTC_Check_LED_Battery_Voltage_TemperatureSensor_Piezo_Click);
      this.btnTC_PiezoTestLowSoundPressure.Location = new Point(13, 112);
      this.btnTC_PiezoTestLowSoundPressure.Name = "btnTC_PiezoTestLowSoundPressure";
      this.btnTC_PiezoTestLowSoundPressure.Size = new Size(394, 23);
      this.btnTC_PiezoTestLowSoundPressure.TabIndex = 14;
      this.btnTC_PiezoTestLowSoundPressure.Text = "Piezo test (low sound pressure)";
      this.btnTC_PiezoTestLowSoundPressure.UseVisualStyleBackColor = true;
      this.btnTC_PiezoTestLowSoundPressure.Click += new System.EventHandler(this.btnTC_PiezoTestLowSoundPressure_Click);
      this.btnTC_PiezoTestHighSoundPressure.Location = new Point(65, 82);
      this.btnTC_PiezoTestHighSoundPressure.Name = "btnTC_PiezoTestHighSoundPressure";
      this.btnTC_PiezoTestHighSoundPressure.Size = new Size(168, 23);
      this.btnTC_PiezoTestHighSoundPressure.TabIndex = 13;
      this.btnTC_PiezoTestHighSoundPressure.Text = "Piezo test (high sound pressure)";
      this.btnTC_PiezoTestHighSoundPressure.UseVisualStyleBackColor = true;
      this.btnTC_PiezoTestHighSoundPressure.Click += new System.EventHandler(this.btnTC_PiezoTestHighSoundPressure_Click);
      this.btnTC_TransmitterLedInSmokeChamberVoltageReferenceTL431.Location = new Point(13, 54);
      this.btnTC_TransmitterLedInSmokeChamberVoltageReferenceTL431.Name = "btnTC_TransmitterLedInSmokeChamberVoltageReferenceTL431";
      this.btnTC_TransmitterLedInSmokeChamberVoltageReferenceTL431.Size = new Size(394, 23);
      this.btnTC_TransmitterLedInSmokeChamberVoltageReferenceTL431.TabIndex = 12;
      this.btnTC_TransmitterLedInSmokeChamberVoltageReferenceTL431.Text = "Transmitter LED in smoke chamber works, voltage reference TL431";
      this.btnTC_TransmitterLedInSmokeChamberVoltageReferenceTL431.UseVisualStyleBackColor = true;
      this.btnTC_TransmitterLedInSmokeChamberVoltageReferenceTL431.Click += new System.EventHandler(this.btnTC_TransmitterLedInSmokeChamberVoltageReferenceTL431_Click);
      this.btnTC_ExitTestMode.Location = new Point(161, 14);
      this.btnTC_ExitTestMode.Name = "btnTC_ExitTestMode";
      this.btnTC_ExitTestMode.Size = new Size(140, 23);
      this.btnTC_ExitTestMode.TabIndex = 4;
      this.btnTC_ExitTestMode.Text = "Exit test mode";
      this.btnTC_ExitTestMode.UseVisualStyleBackColor = true;
      this.btnTC_ExitTestMode.Click += new System.EventHandler(this.btnTC_ExitTestMode_Click);
      this.btnTC_EnterTestMode.Location = new Point(5, 14);
      this.btnTC_EnterTestMode.Name = "btnTC_EnterTestMode";
      this.btnTC_EnterTestMode.Size = new Size(140, 23);
      this.btnTC_EnterTestMode.TabIndex = 3;
      this.btnTC_EnterTestMode.Text = "Enter test mode";
      this.btnTC_EnterTestMode.UseVisualStyleBackColor = true;
      this.btnTC_EnterTestMode.Click += new System.EventHandler(this.btnTC_EnterTestMode_Click);
      this.tabDataAnalysis.Controls.Add((Control) this.btnSetTestIdentification);
      this.tabDataAnalysis.Controls.Add((Control) this.btnCheckLedFailure);
      this.tabDataAnalysis.Controls.Add((Control) this.btnReadAesKey);
      this.tabDataAnalysis.Controls.Add((Control) this.btnWriteAesKey);
      this.tabDataAnalysis.Controls.Add((Control) this.btnSetPathOld);
      this.tabDataAnalysis.Controls.Add((Control) this.labelPath);
      this.tabDataAnalysis.Controls.Add((Control) this.btnSetPath);
      this.tabDataAnalysis.Controls.Add((Control) this.btnGetData);
      this.tabDataAnalysis.Location = new Point(4, 22);
      this.tabDataAnalysis.Name = "tabDataAnalysis";
      this.tabDataAnalysis.Size = new Size(803, 326);
      this.tabDataAnalysis.TabIndex = 3;
      this.tabDataAnalysis.Text = "Data Analysis";
      this.tabDataAnalysis.UseVisualStyleBackColor = true;
      this.btnSetTestIdentification.Location = new Point(333, 237);
      this.btnSetTestIdentification.Margin = new Padding(2);
      this.btnSetTestIdentification.Name = "btnSetTestIdentification";
      this.btnSetTestIdentification.Size = new Size(215, 19);
      this.btnSetTestIdentification.TabIndex = 9;
      this.btnSetTestIdentification.Text = "Set test identification";
      this.btnSetTestIdentification.UseVisualStyleBackColor = true;
      this.btnSetTestIdentification.Click += new System.EventHandler(this.btnSetTestIdentification_Click);
      this.btnCheckLedFailure.Location = new Point(8, 181);
      this.btnCheckLedFailure.Margin = new Padding(2);
      this.btnCheckLedFailure.Name = "btnCheckLedFailure";
      this.btnCheckLedFailure.Size = new Size(215, 19);
      this.btnCheckLedFailure.TabIndex = 6;
      this.btnCheckLedFailure.Text = "CheckLedFailure";
      this.btnCheckLedFailure.UseVisualStyleBackColor = true;
      this.btnCheckLedFailure.Click += new System.EventHandler(this.btnCheckLedFailure_Click);
      this.btnReadAesKey.Location = new Point(0, 0);
      this.btnReadAesKey.Margin = new Padding(2);
      this.btnReadAesKey.Name = "btnReadAesKey";
      this.btnReadAesKey.Size = new Size(56, 19);
      this.btnReadAesKey.TabIndex = 7;
      this.btnWriteAesKey.Location = new Point(0, 0);
      this.btnWriteAesKey.Margin = new Padding(2);
      this.btnWriteAesKey.Name = "btnWriteAesKey";
      this.btnWriteAesKey.Size = new Size(56, 19);
      this.btnWriteAesKey.TabIndex = 8;
      this.btnSetPathOld.Location = new Point(257, 34);
      this.btnSetPathOld.Name = "btnSetPathOld";
      this.btnSetPathOld.Size = new Size(240, 25);
      this.btnSetPathOld.TabIndex = 3;
      this.btnSetPathOld.Text = "Set output path to a old file";
      this.btnSetPathOld.UseVisualStyleBackColor = true;
      this.btnSetPathOld.Click += new System.EventHandler(this.btnSetPathOld_Click);
      this.labelPath.AutoSize = true;
      this.labelPath.Font = new Font("SimSun", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.labelPath.Location = new Point(5, 4);
      this.labelPath.Name = "labelPath";
      this.labelPath.Size = new Size(175, 14);
      this.labelPath.TabIndex = 2;
      this.labelPath.Text = "The Path Of Output Data ";
      this.btnSetPath.Location = new Point(8, 34);
      this.btnSetPath.Name = "btnSetPath";
      this.btnSetPath.Size = new Size(224, 25);
      this.btnSetPath.TabIndex = 1;
      this.btnSetPath.Text = "Set output path to a new file";
      this.btnSetPath.UseVisualStyleBackColor = true;
      this.btnSetPath.Click += new System.EventHandler(this.btnSetPath_Click);
      this.btnGetData.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.btnGetData.Location = new Point(8, 79);
      this.btnGetData.Name = "btnGetData";
      this.btnGetData.Size = new Size(291, 38);
      this.btnGetData.TabIndex = 0;
      this.btnGetData.Text = "Read Device and Write Datasheet";
      this.btnGetData.UseVisualStyleBackColor = true;
      this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
      this.groupBox5.Controls.Add((Control) this.txtNear6);
      this.groupBox5.Controls.Add((Control) this.label12);
      this.groupBox5.Controls.Add((Control) this.txtNear5);
      this.groupBox5.Controls.Add((Control) this.label11);
      this.groupBox5.Controls.Add((Control) this.txtNear4);
      this.groupBox5.Controls.Add((Control) this.label10);
      this.groupBox5.Controls.Add((Control) this.txtNear3);
      this.groupBox5.Controls.Add((Control) this.label9);
      this.groupBox5.Controls.Add((Control) this.txtNear2);
      this.groupBox5.Controls.Add((Control) this.label8);
      this.groupBox5.Controls.Add((Control) this.txtNear1);
      this.groupBox5.Controls.Add((Control) this.label7);
      this.groupBox5.Controls.Add((Control) this.button1);
      this.groupBox5.Controls.Add((Control) this.button2);
      this.groupBox5.Location = new Point(215, 257);
      this.groupBox5.Name = "groupBox5";
      this.groupBox5.Size = new Size(576, 66);
      this.groupBox5.TabIndex = 31;
      this.groupBox5.TabStop = false;
      this.groupBox5.Text = "Obstruction Calibration";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(16, 20);
      this.label7.Name = "label7";
      this.label7.Size = new Size(36, 13);
      this.label7.TabIndex = 31;
      this.label7.Text = "Near1";
      this.txtNear1.Location = new Point(16, 37);
      this.txtNear1.Maximum = new Decimal(new int[4]
      {
        4095,
        0,
        0,
        0
      });
      this.txtNear1.Name = "txtNear1";
      this.txtNear1.Size = new Size(49, 20);
      this.txtNear1.TabIndex = 32;
      this.txtNear2.Location = new Point(71, 37);
      this.txtNear2.Maximum = new Decimal(new int[4]
      {
        4095,
        0,
        0,
        0
      });
      this.txtNear2.Name = "txtNear2";
      this.txtNear2.Size = new Size(49, 20);
      this.txtNear2.TabIndex = 34;
      this.label8.AutoSize = true;
      this.label8.Location = new Point(71, 20);
      this.label8.Name = "label8";
      this.label8.Size = new Size(36, 13);
      this.label8.TabIndex = 33;
      this.label8.Text = "Near2";
      this.txtNear3.Location = new Point(126, 37);
      this.txtNear3.Maximum = new Decimal(new int[4]
      {
        4095,
        0,
        0,
        0
      });
      this.txtNear3.Name = "txtNear3";
      this.txtNear3.Size = new Size(49, 20);
      this.txtNear3.TabIndex = 36;
      this.label9.AutoSize = true;
      this.label9.Location = new Point(126, 20);
      this.label9.Name = "label9";
      this.label9.Size = new Size(36, 13);
      this.label9.TabIndex = 35;
      this.label9.Text = "Near3";
      this.txtNear4.Location = new Point(181, 37);
      this.txtNear4.Maximum = new Decimal(new int[4]
      {
        4095,
        0,
        0,
        0
      });
      this.txtNear4.Name = "txtNear4";
      this.txtNear4.Size = new Size(49, 20);
      this.txtNear4.TabIndex = 38;
      this.label10.AutoSize = true;
      this.label10.Location = new Point(181, 20);
      this.label10.Name = "label10";
      this.label10.Size = new Size(36, 13);
      this.label10.TabIndex = 37;
      this.label10.Text = "Near4";
      this.txtNear5.Location = new Point(236, 37);
      this.txtNear5.Maximum = new Decimal(new int[4]
      {
        4095,
        0,
        0,
        0
      });
      this.txtNear5.Name = "txtNear5";
      this.txtNear5.Size = new Size(49, 20);
      this.txtNear5.TabIndex = 40;
      this.label11.AutoSize = true;
      this.label11.Location = new Point(236, 20);
      this.label11.Name = "label11";
      this.label11.Size = new Size(36, 13);
      this.label11.TabIndex = 39;
      this.label11.Text = "Near5";
      this.txtNear6.Location = new Point(291, 37);
      this.txtNear6.Maximum = new Decimal(new int[4]
      {
        4095,
        0,
        0,
        0
      });
      this.txtNear6.Name = "txtNear6";
      this.txtNear6.Size = new Size(49, 20);
      this.txtNear6.TabIndex = 42;
      this.label12.AutoSize = true;
      this.label12.Location = new Point(291, 20);
      this.label12.Name = "label12";
      this.label12.Size = new Size(36, 13);
      this.label12.TabIndex = 41;
      this.label12.Text = "Near6";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(822, 562);
      this.Controls.Add((Control) this.tabs);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.statusStrip);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (TestCommand);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Test commands";
      this.FormClosing += new FormClosingEventHandler(this.TestCommand_FormClosing);
      this.Load += new System.EventHandler(this.TestCommand_Load);
      this.statusStrip.ResumeLayout(false);
      this.statusStrip.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.tabGenerally.ResumeLayout(false);
      this.tabGenerally.PerformLayout();
      this.txtMbus_nighttime_stop.EndInit();
      this.txtMbus_nighttime_start.EndInit();
      this.txtMbus_radio_suppression_days.EndInit();
      this.txtMbus_interval.EndInit();
      this.txtCValue.EndInit();
      this.groupBox4.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.tabs.ResumeLayout(false);
      this.tabTestMode.ResumeLayout(false);
      this.txtPiezoHsDuration.EndInit();
      this.groupBox1.ResumeLayout(false);
      this.tabDataAnalysis.ResumeLayout(false);
      this.tabDataAnalysis.PerformLayout();
      this.groupBox5.ResumeLayout(false);
      this.groupBox5.PerformLayout();
      this.txtNear1.EndInit();
      this.txtNear2.EndInit();
      this.txtNear3.EndInit();
      this.txtNear4.EndInit();
      this.txtNear5.EndInit();
      this.txtNear6.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private delegate TReturn ActionSimpleMethod<TReturn>();

    private delegate TReturn ActionSimpleMethodWithParam<TReturn, TArg>(TArg arg);
  }
}

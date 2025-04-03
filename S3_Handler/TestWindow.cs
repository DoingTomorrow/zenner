// Decompiled with JetBrains decompiler
// Type: S3_Handler.TestWindow
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CorporateDesign;
using DeviceCollector;
using HandlerLib;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  public class TestWindow : Form
  {
    private static Logger S3_LoRaTestLogger = LogManager.GetLogger(nameof (S3_LoRaTestLogger));
    private const string ClassMessageName = "S3_Handler Testwindow";
    private S3_HandlerFunctions MyFunctions;
    private S3_Meter MyMeter;
    private TestWindowCommunication windowCommunication;
    private TestScript testScript;
    private string scriptFilePath;
    private const string LoRaTimeFormat = "dd.MM.yyyy HH:mm:ss";
    private static DateTime NotDefinedTime = new DateTime(1980, 1, 1);
    private DateTime MeterTime;
    private DateTime NextLoRaScenarioEventTime;
    private DateTime NextLoRaDiagnosticEventTime;
    private ushort RadioCycleTimeCounter;
    private S3_Parameter S3_ValueParam_DeviceTime;
    private S3_Parameter S3_ValueParam_radioCycleTimeCounter;
    private S3_Parameter S3_ValueParam_lora_diag_nexttime;
    private DateTime LoRaAutoNextTime;
    private bool LoRaAutoMoveTime;
    private int PacketAddress;
    private LoRaPackets NextPacket;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private GroupBox groupBoxInterfaceCommands;
    private Button buttonOpto1hour;
    private Button buttonOptoStop;
    private Button buttonReset;
    private Button buttonBreakLoop;
    private Button buttonPulsProgramLoop;
    private GroupBox groupBoxTestCommands;
    private TextBox textBoxState;
    private Button buttonShowReadoutValues;
    private Button buttonShowConfigParam;
    private Button buttonRunIdTest;
    private Button buttonLoadAndRunTestScript;
    private GroupBox groupBoxSingleStep;
    private Button buttonSingleStepContinue;
    private CheckBox checkBoxSingleStepOn;
    private Button buttonShowLcdDisplayValues;
    private TextBox textBoxScriptName;
    private Button buttonGetVolumenHardwareCounterValue;
    private Button buttonGetLcdDisplay;
    private Button buttonShowIdentObject;
    private GroupBox groupBoxLoRaTest;
    private TextBox textBoxNextLoRaDiagnosticEventTime;
    private Label label4;
    private TextBox textBoxNextLoRaScenarioEventTime;
    private Label label3;
    private TextBox textBoxDeviceTime;
    private Label label2;
    private TextBox textBoxLoRaScenario;
    private Label label1;
    private Button buttonMoveToNextLoRaEvent;
    private Button buttonUpdateLoRaState;
    private System.Windows.Forms.Timer timerLoRa;
    private Button buttonAutoRunAllLoRaEvents;

    internal TestWindow(S3_HandlerFunctions MyFunctions, S3_Meter MyMeter)
    {
      this.MyFunctions = MyFunctions;
      this.MyMeter = MyMeter;
      this.InitializeComponent();
      if (this.MyMeter == null || !this.MyMeter.MyParameters.ParameterByName.ContainsKey(S3_ParameterNames.radioCycleTimeCounter.ToString()))
        this.groupBoxLoRaTest.Enabled = false;
      this.windowCommunication = new TestWindowCommunication(this.textBoxState);
    }

    private void buttonOptoStop_Click(object sender, EventArgs e)
    {
      this.MyFunctions.MyCommands.SetOptoTimeoutSeconds(0);
    }

    private void buttonOpto1hour_Click(object sender, EventArgs e)
    {
      this.MyFunctions.MyCommands.SetOptoTimeoutSeconds(3600);
    }

    private void buttonReset_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      if (this.MyFunctions.MyCommands.ResetDevice())
      {
        int num = (int) GMM_MessageBox.ShowMessage("S3_Handler Testwindow", "Reset ok");
        ZR_ClassLibMessages.ShowAndClearErrors();
      }
      else
        ZR_ClassLibMessages.ShowAndClearErrors("S3_Handler Testwindow", "Reset error");
    }

    private void buttonPulsProgramLoop_Click(object sender, EventArgs e)
    {
      Random random = new Random();
      this.LoopStateSet();
      float ErrorInPercent = 2f;
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      while (!this.windowCommunication.breakRequest)
      {
        ZR_ClassLibMessages.ClearErrors();
        this.textBoxState.Clear();
        this.textBoxState.AppendText("Loops: ....... " + num1.ToString() + Environment.NewLine);
        this.textBoxState.AppendText("Read errors: . " + num2.ToString() + Environment.NewLine);
        this.textBoxState.AppendText("Adjust errors: " + num3.ToString() + Environment.NewLine);
        this.textBoxState.AppendText("Write errors:  " + num4.ToString() + Environment.NewLine);
        this.textBoxState.AppendText("Read device... ");
        this.textBoxState.Update();
        Application.DoEvents();
        this.MyFunctions.Clear();
        if (this.MyFunctions.ReadConnectedDevice())
        {
          this.textBoxState.AppendText("done" + Environment.NewLine);
          this.textBoxState.AppendText("Adjust volume factor... ");
          this.textBoxState.Update();
          Application.DoEvents();
          ErrorInPercent = (double) ErrorInPercent <= 0.0 ? 2f : -2f;
          if (this.MyFunctions.AdjustVolumeFactor(ErrorInPercent))
          {
            this.textBoxState.AppendText("done; Factor: " + this.MyFunctions.MyMeters.WorkMeter.MyParameters.ParameterByName[S3_ParameterNames.Con_VolFactor.ToString()].GetFloatValue().ToString() + Environment.NewLine);
            this.textBoxState.AppendText("Write changes to device... ");
            this.textBoxState.Update();
            Application.DoEvents();
            if (this.MyFunctions.WriteChangesToConnectedDevice())
            {
              this.textBoxState.AppendText("done" + Environment.NewLine);
              this.textBoxState.Update();
              Application.DoEvents();
              int millisecondsTimeout = (int) (500.0 + 9500.0 * random.NextDouble());
              this.textBoxState.AppendText("Wait milli secounds: " + millisecondsTimeout.ToString());
              this.textBoxState.Update();
              Application.DoEvents();
              Thread.Sleep(millisecondsTimeout);
              ++num1;
            }
            else
            {
              ZR_ClassLibMessages.ShowAndClearErrors();
              int num5 = num4 + 1;
              break;
            }
          }
          else
          {
            ZR_ClassLibMessages.ShowAndClearErrors();
            int num6 = num3 + 1;
            break;
          }
        }
        else
        {
          ZR_ClassLibMessages.ShowAndClearErrors();
          ++num2;
          break;
        }
      }
      this.LoopStateReset();
    }

    private void buttonBreakLoop_Click(object sender, EventArgs e)
    {
      this.timerLoRa.Stop();
      this.LoRaAutoNextTime = DateTime.MinValue;
      this.groupBoxInterfaceCommands.Enabled = true;
      this.groupBoxTestCommands.Enabled = true;
      this.groupBoxSingleStep.Enabled = true;
      this.buttonUpdateLoRaState.Enabled = true;
      this.buttonMoveToNextLoRaEvent.Enabled = false;
      this.buttonAutoRunAllLoRaEvents.Enabled = false;
      this.windowCommunication.breakRequest = true;
      this.buttonBreakLoop.Enabled = false;
    }

    private void LoopStateSet()
    {
      this.windowCommunication.breakRequest = false;
      this.buttonBreakLoop.Enabled = true;
      this.groupBoxInterfaceCommands.Enabled = false;
      this.groupBoxTestCommands.Enabled = false;
      this.ControlBox = false;
    }

    private void LoopStateReset()
    {
      this.groupBoxInterfaceCommands.Enabled = true;
      this.groupBoxTestCommands.Enabled = true;
      this.buttonBreakLoop.Enabled = false;
      this.ControlBox = true;
    }

    private void buttonShowConfigParam_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.textBoxState.Clear();
      this.textBoxState.AppendText("*** All configuration parameters ***");
      this.textBoxState.AppendText(Environment.NewLine);
      this.textBoxState.AppendText("Update dynamic RAM values ...");
      this.textBoxState.AppendText(Environment.NewLine);
      this.textBoxState.AppendText(Environment.NewLine);
      if (!this.MyFunctions.RefreshDynamicRamData())
      {
        ZR_ClassLibMessages.ShowAndClearErrors();
        this.textBoxState.Clear();
      }
      else
      {
        SortedList<OverrideID, ConfigurationParameter> configurationParameters = this.MyFunctions.GetConfigurationParameters(0);
        for (int index = 0; index < configurationParameters.Count; ++index)
        {
          this.textBoxState.AppendText(configurationParameters.Keys[index].ToString() + ": ");
          this.textBoxState.AppendText(configurationParameters.Values[index].GetStringValueWin());
          this.textBoxState.AppendText(" " + configurationParameters.Values[index].Unit);
          this.textBoxState.AppendText(Environment.NewLine);
        }
      }
    }

    private void buttonShowReadoutValues_Click(object sender, EventArgs e)
    {
      ZR_ClassLibMessages.ClearErrors();
      this.textBoxState.Clear();
      this.textBoxState.AppendText("*** All read out values ***");
      this.textBoxState.AppendText(Environment.NewLine);
      this.textBoxState.AppendText("Update dynamic RAM values ...");
      if (!this.MyFunctions.RefreshDynamicRamData())
      {
        ZR_ClassLibMessages.ShowAndClearErrors();
        this.textBoxState.Clear();
      }
      else
      {
        SortedList<long, SortedList<DateTime, ReadingValue>> ValueList = (SortedList<long, SortedList<DateTime, ReadingValue>>) null;
        if (!this.MyFunctions.GetValues(ref ValueList, 0))
        {
          this.textBoxState.AppendText(Environment.NewLine);
          this.textBoxState.AppendText("Value list error");
        }
        else
        {
          for (int index1 = 0; index1 < ValueList.Count; ++index1)
          {
            this.textBoxState.AppendText(Environment.NewLine + Environment.NewLine);
            this.textBoxState.AppendText(ValueIdent.GetTranslatedValueNameForValueId(ValueList.Keys[index1], true));
            this.textBoxState.AppendText(":");
            SortedList<DateTime, ReadingValue> sortedList = ValueList.Values[index1];
            for (int index2 = 0; index2 < sortedList.Count; ++index2)
            {
              this.textBoxState.AppendText(Environment.NewLine);
              this.textBoxState.AppendText("   ");
              this.textBoxState.AppendText(sortedList.Keys[index2].ToShortDateString());
              this.textBoxState.AppendText(" ");
              this.textBoxState.AppendText(sortedList.Keys[index2].ToShortTimeString());
              this.textBoxState.AppendText(": ");
              this.textBoxState.AppendText(sortedList.Values[index2].value.ToString());
            }
          }
          this.textBoxState.AppendText(Environment.NewLine);
        }
      }
    }

    private void buttonRunIdTest_Click(object sender, EventArgs e)
    {
      this.textBoxState.Clear();
      ZR_ClassLibMessages.ClearErrors();
      this.textBoxState.Refresh();
      Application.DoEvents();
      byte[] numArray = this.MyFunctions.MyCommands.RunIoTest(IoTestFunctions.IoTest_ActivateIoTestMode);
      if (numArray != null)
        numArray = this.MyFunctions.MyCommands.RunIoTest(IoTestFunctions.IoTest_Run);
      if (numArray == null)
      {
        this.textBoxState.AppendText("Test error");
        ZR_ClassLibMessages.ShowAndClearErrors();
      }
      else
      {
        this.textBoxState.AppendText("Test done");
        this.textBoxState.AppendText(Environment.NewLine);
        for (int index = 0; index < numArray.Length; ++index)
        {
          this.textBoxState.AppendText(Environment.NewLine);
          this.textBoxState.AppendText("0x" + numArray[index].ToString("x02"));
          this.textBoxState.AppendText(" --> ");
          this.textBoxState.AppendText(this.GetIoTestInfoString(numArray[index]));
        }
        if (this.MyFunctions.MyCommands.RunIoTest(IoTestFunctions.IoTest_DeactivateIoTestMode) == null)
        {
          this.textBoxState.AppendText("Test error");
          ZR_ClassLibMessages.ShowAndClearErrors();
        }
      }
    }

    private string GetIoTestInfoString(byte infoByte)
    {
      StringBuilder stringBuilder = new StringBuilder(100);
      stringBuilder.Append("[Set/Read]");
      stringBuilder.Append(" Out1: ");
      if (((uint) infoByte & 16U) > 0U)
        stringBuilder.Append("1");
      else
        stringBuilder.Append("0");
      stringBuilder.Append("/");
      if (((uint) infoByte & 1U) > 0U)
        stringBuilder.Append("1");
      else
        stringBuilder.Append("0");
      stringBuilder.Append(" ; Out2: ");
      if (((uint) infoByte & 32U) > 0U)
        stringBuilder.Append("1");
      else
        stringBuilder.Append("0");
      stringBuilder.Append("/");
      if (((uint) infoByte & 2U) > 0U)
        stringBuilder.Append("1");
      else
        stringBuilder.Append("0");
      stringBuilder.Append(" ; Out3: ");
      if (((uint) infoByte & 64U) > 0U)
        stringBuilder.Append("1");
      else
        stringBuilder.Append("0");
      stringBuilder.Append("/");
      if (((uint) infoByte & 4U) > 0U)
        stringBuilder.Append("1");
      else
        stringBuilder.Append("0");
      return stringBuilder.ToString();
    }

    private void buttonLoadAndRunTestScript_Click(object sender, EventArgs e)
    {
      string str = SystemValues.LoggDataPath;
      string[] strArray = SystemValues.AppPath.Split(Path.DirectorySeparatorChar);
      if (strArray.Length > 4 && strArray[strArray.Length - 1] == "Debug" && strArray[strArray.Length - 2] == "x86" && strArray[strArray.Length - 3] == "bin" && strArray[strArray.Length - 4] == "GlobalMeterManager")
      {
        string path1 = strArray[0] + Path.DirectorySeparatorChar.ToString();
        for (int index = 1; index < strArray.Length - 4; ++index)
          path1 = Path.Combine(path1, strArray[index]);
        str = Path.Combine(Path.Combine(path1, "S3_Handler"), "TestScripts");
      }
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "Serie3 test script (*.S3TS)|*.S3TS| All files (*.*)|*.*";
      openFileDialog.FilterIndex = 1;
      openFileDialog.RestoreDirectory = true;
      openFileDialog.Title = "Load Serie3 test script";
      openFileDialog.CheckFileExists = true;
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      this.scriptFilePath = openFileDialog.FileName;
      this.buttonSingleStepContinue.Enabled = true;
      this.textBoxScriptName.Text = Path.GetFileNameWithoutExtension(this.scriptFilePath);
      this.textBoxState.Clear();
    }

    private void RunTestScript()
    {
      this.textBoxState.Clear();
      ZR_ClassLibMessages.ClearErrors();
      this.textBoxState.AppendText("*** Run Serie3 test script ***");
      this.textBoxState.AppendText(Environment.NewLine);
      this.textBoxState.AppendText("File: " + this.scriptFilePath);
      this.textBoxState.AppendText(Environment.NewLine);
      this.textBoxState.Refresh();
      Application.DoEvents();
      this.LoopStateSet();
      this.testScript = new TestScript(this.MyMeter, this.windowCommunication);
      this.buttonSingleStepContinue.Text = "Continue";
      this.testScript.RunScript(this.scriptFilePath);
      this.testScript = (TestScript) null;
      this.buttonSingleStepContinue.Text = "Run";
      this.LoopStateReset();
    }

    private void buttonSingleStepContinue_Click(object sender, EventArgs e)
    {
      if (this.testScript == null)
        this.RunTestScript();
      else
        this.windowCommunication.continueSingleStep = true;
    }

    private void buttonShowLcdDisplayValues_Click(object sender, EventArgs e)
    {
      this.textBoxState.Clear();
      ZR_ClassLibMessages.ClearErrors();
      StringBuilder stringBuilder = new StringBuilder();
      MeterDisplayValues displayValues = this.MyFunctions.GetDisplayValues();
      if (displayValues.energyIsAvailable)
      {
        stringBuilder.Append("Energy: ");
        stringBuilder.Append(displayValues.energy.ToString("d08"));
        if (displayValues.energyAfterPointDigits > (byte) 0)
          stringBuilder.Insert(stringBuilder.Length - (int) displayValues.energyAfterPointDigits, '.');
        stringBuilder.Append(" " + displayValues.energyUnitString);
        Decimal num = (Decimal) displayValues.energy * (Decimal) displayValues.energyFactorFromWh;
        stringBuilder.AppendLine(" = " + num.ToString() + " Wh");
      }
      else
        stringBuilder.AppendLine("Energy not available");
      stringBuilder.AppendLine();
      if (displayValues.cEnergyIsAvailable)
      {
        stringBuilder.Append("CEnergy: ");
        stringBuilder.Append(displayValues.cEnergy.ToString("d08"));
        if (displayValues.cEnergyAfterPointDigits > (byte) 0)
          stringBuilder.Insert(stringBuilder.Length - (int) displayValues.cEnergyAfterPointDigits, '.');
        stringBuilder.Append(" " + displayValues.cEnergyUnitString);
        Decimal num = (Decimal) displayValues.cEnergy * (Decimal) displayValues.cEnergyFactorFromWh;
        stringBuilder.AppendLine(" = " + num.ToString() + " Wh");
      }
      else
        stringBuilder.AppendLine("CEnergy not available");
      stringBuilder.AppendLine();
      if (displayValues.volumeIsAvailable)
      {
        stringBuilder.Append("Volume: ");
        stringBuilder.Append(displayValues.volume.ToString("d08"));
        if (displayValues.volumeAfterPointDigits > (byte) 0)
          stringBuilder.Insert(stringBuilder.Length - (int) displayValues.volumeAfterPointDigits, '.');
        stringBuilder.Append(" " + displayValues.volumeUnitString);
        Decimal num = (Decimal) displayValues.volume * (Decimal) displayValues.volumeFactorFromLiter;
        stringBuilder.AppendLine(" = " + num.ToString() + " Liter");
      }
      else
        stringBuilder.AppendLine("Volume not available");
      stringBuilder.AppendLine();
      if (displayValues.input0IsAvailable)
      {
        stringBuilder.Append("Input0: ");
        stringBuilder.Append(displayValues.input0Value.ToString("d08"));
        if (displayValues.input0AfterPointDigits > (byte) 0)
          stringBuilder.Insert(stringBuilder.Length - (int) displayValues.input0AfterPointDigits, '.');
        stringBuilder.AppendLine(" " + displayValues.input0UnitString);
      }
      else
        stringBuilder.AppendLine("Input 0 not available");
      stringBuilder.AppendLine();
      if (displayValues.input1IsAvailable)
      {
        stringBuilder.Append("Input1: ");
        stringBuilder.Append(displayValues.input1Value.ToString("d08"));
        if (displayValues.input1AfterPointDigits > (byte) 0)
          stringBuilder.Insert(stringBuilder.Length - (int) displayValues.input1AfterPointDigits, '.');
        stringBuilder.AppendLine(" " + displayValues.input1UnitString);
      }
      else
        stringBuilder.AppendLine("Input 1 not available");
      stringBuilder.AppendLine();
      if (displayValues.input2IsAvailable)
      {
        stringBuilder.Append("Input2: ");
        stringBuilder.Append(displayValues.input2Value.ToString("d08"));
        if (displayValues.input2AfterPointDigits > (byte) 0)
          stringBuilder.Insert(stringBuilder.Length - (int) displayValues.input2AfterPointDigits, '.');
        stringBuilder.AppendLine(" " + displayValues.input2UnitString);
      }
      else
        stringBuilder.AppendLine("Input 2 not available");
      this.textBoxState.Text = stringBuilder.ToString();
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void checkBoxSingleStepOn_CheckedChanged(object sender, EventArgs e)
    {
      this.windowCommunication.singleStepOn = this.checkBoxSingleStepOn.Checked;
    }

    private void buttonGetVolumenHardwareCounterValue_Click(object sender, EventArgs e)
    {
      this.textBoxState.Clear();
      ZR_ClassLibMessages.ClearErrors();
      byte volumeCounterValue;
      if (this.MyFunctions.GetVolumeCounterValue(out volumeCounterValue))
        this.textBoxState.Text = "Volume counter = " + volumeCounterValue.ToString();
      else
        this.textBoxState.Text = "Read error";
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonGetLcdDisplay_Click(object sender, EventArgs e)
    {
      this.textBoxState.Clear();
      ZR_ClassLibMessages.ClearErrors();
      LCD_Display theDisplay;
      if (this.MyFunctions.GetActiveLcdDisplay(out theDisplay))
        this.textBoxState.Text = "LCD display = " + theDisplay.lcdValue.ToString() + " " + theDisplay.lcdUnit;
      else
        this.textBoxState.Text = "Read error";
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonShowIdentObject_Click(object sender, EventArgs e)
    {
      S3_DeviceId deviceId = this.MyFunctions.GetDeviceId();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("IdentificationChecksum: " + deviceId.IdentificationCheckState.ToString());
      stringBuilder.AppendLine("FirmwareVersion: " + deviceId.FirmwareVersion.ToString());
      stringBuilder.AppendLine("FirmwareVersionString: " + deviceId.FirmwareVersionString.ToString());
      stringBuilder.AppendLine("HardwareMask: " + deviceId.HardwareMask.ToString());
      stringBuilder.AppendLine("HardwareTypeId: " + deviceId.HardwareTypeId.ToString());
      stringBuilder.AppendLine("MeterId.: " + deviceId.MeterId.ToString());
      stringBuilder.AppendLine("MeterInfoId: " + deviceId.MeterInfoId.ToString());
      stringBuilder.AppendLine("MeterTypeId: " + deviceId.MeterTypeId.ToString());
      stringBuilder.AppendLine("BaseTypeId: " + deviceId.BaseTypeId.ToString());
      stringBuilder.AppendLine("MapId: " + deviceId.MapId.ToString());
      stringBuilder.AppendLine("SAP_MaterialNumber: " + deviceId.SAP_MaterialNumber.ToString());
      stringBuilder.AppendLine("SAP_ProductionOrderNumber: " + deviceId.SAP_ProductionOrderNumber.ToString());
      stringBuilder.AppendLine("SerialNumber: " + deviceId.SerialNumber.ToString("x08"));
      stringBuilder.AppendLine("FullSerialNumber: " + deviceId.FullSerialNumber.ToString());
      stringBuilder.AppendLine("ApprovalRevison: " + deviceId.ApprovalRevison.ToString());
      stringBuilder.AppendLine("HardwareResources: ");
      for (int index = 0; index < deviceId.HardwareResources.Count; ++index)
        stringBuilder.AppendLine(" + " + deviceId.HardwareResources.Keys[index]);
      if (deviceId.HardwareOptions == null)
        stringBuilder.AppendLine("HardwareOptions: ---");
      else
        stringBuilder.AppendLine("HardwareOptions: " + deviceId.HardwareOptions.ToString());
      deviceId.SerialNumber += 286331153U;
      deviceId.SerialNumber &= 2004318071U;
      deviceId.FullSerialNumber = deviceId.FullSerialNumber.Substring(0, 6) + deviceId.SerialNumber.ToString("x08");
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("Change serial number to: " + deviceId.FullSerialNumber);
      this.textBoxState.Text = stringBuilder.ToString();
      this.MyFunctions.SetDeviceId(deviceId);
    }

    private void buttonUpdateLoRaState_Click(object sender, EventArgs e)
    {
      this.textBoxState.Clear();
      this.groupBoxInterfaceCommands.Enabled = false;
      this.groupBoxTestCommands.Enabled = false;
      this.groupBoxSingleStep.Enabled = false;
      this.buttonMoveToNextLoRaEvent.Enabled = true;
      this.buttonAutoRunAllLoRaEvents.Enabled = true;
      this.buttonBreakLoop.Enabled = true;
      this.LoRaAutoNextTime = DateTime.MinValue;
      this.S3_ValueParam_radioCycleTimeCounter = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.radioCycleTimeCounter.ToString()];
      this.S3_ValueParam_lora_diag_nexttime = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.lora_diag_nexttime.ToString()];
      this.S3_ValueParam_DeviceTime = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Bak_TimeBaseSecounds.ToString()];
      this.PacketAddress = this.MyMeter.theMap.First<KeyValuePair<string, int>>((Func<KeyValuePair<string, int>, bool>) (x => x.Key == "time_to_send_data")).Value + 16;
      byte transmissionScenario = ((S3_CommandsCOMPATIBLE) this.MyFunctions.MyCommands).myCommandsLoRa.GetTransmissionScenario((ProgressHandler) null, CancellationToken.None);
      switch (transmissionScenario)
      {
        case 1:
          this.textBoxLoRaScenario.Text = "1 -> Monthly";
          break;
        case 2:
          this.textBoxLoRaScenario.Text = "2 -> Daily";
          break;
        default:
          this.textBoxLoRaScenario.Text = transmissionScenario.ToString() + " -> ???";
          break;
      }
      TestWindow.S3_LoRaTestLogger.Trace("Scenario = " + this.textBoxLoRaScenario.Text);
      this.UpdateLoRaValues();
      this.ShowTimes();
      this.timerLoRa.Stop();
      this.timerLoRa.Interval = 1000;
      this.timerLoRa.Start();
    }

    private void UpdateLoRaValues()
    {
      uint uintValue1;
      ByteField MemoryData;
      uint uintValue2;
      do
      {
        this.S3_ValueParam_DeviceTime.ReadParameterFromConnectedDevice();
        uintValue1 = this.S3_ValueParam_DeviceTime.GetUintValue();
        this.S3_ValueParam_lora_diag_nexttime.ReadParameterFromConnectedDevice();
        this.S3_ValueParam_radioCycleTimeCounter.ReadParameterFromConnectedDevice();
        this.MyFunctions.MyCommands.ReadMemory(MemoryLocation.RAM, this.PacketAddress, 1, out MemoryData);
        this.S3_ValueParam_DeviceTime.ReadParameterFromConnectedDevice();
        uintValue2 = this.S3_ValueParam_DeviceTime.GetUintValue();
      }
      while ((int) uintValue1 != (int) uintValue2);
      this.NextPacket = (LoRaPackets) MemoryData.Data[0];
      this.MeterTime = ZR_Calendar.Cal_GetDateTime(uintValue1);
      this.NextLoRaDiagnosticEventTime = ZR_Calendar.Cal_GetDateTime(this.S3_ValueParam_lora_diag_nexttime.GetUintValue());
      this.RadioCycleTimeCounter = this.S3_ValueParam_radioCycleTimeCounter.GetUshortValue();
      this.NextLoRaScenarioEventTime = this.RadioCycleTimeCounter != (ushort) 0 ? this.MeterTime.AddSeconds((double) this.RadioCycleTimeCounter) : this.MeterTime.Date.AddDays(1.0);
      TestWindow.S3_LoRaTestLogger.Trace("Update: MeterTime=" + this.MeterTime.ToString("dd.MM.yyyy HH:mm:ss") + "; LoRaScenarioEventTime=" + this.NextLoRaScenarioEventTime.ToString("dd.MM.yyyy HH:mm:ss") + "; LoRaDiagnosticEventTime=" + this.GetNextDiagnosticTimeString());
    }

    private void MoveToNextLoRaEvent()
    {
      this.UpdateLoRaValues();
      DateTime dateTime = !(this.NextLoRaDiagnosticEventTime == TestWindow.NotDefinedTime) && !(this.NextLoRaScenarioEventTime < this.NextLoRaDiagnosticEventTime) ? this.NextLoRaDiagnosticEventTime : this.NextLoRaScenarioEventTime;
      int num = (int) dateTime.Subtract(this.MeterTime).TotalSeconds - 3;
      if (num < 5)
        return;
      this.S3_ValueParam_DeviceTime.SetUintValue(ZR_Calendar.Cal_GetMeterTime(dateTime.AddSeconds(-3.0)));
      this.S3_ValueParam_DeviceTime.WriteParameterToConnectedDevice();
      if ((int) this.RadioCycleTimeCounter > num)
      {
        this.RadioCycleTimeCounter -= (ushort) num;
        this.S3_ValueParam_radioCycleTimeCounter.SetUshortValue(this.RadioCycleTimeCounter);
        this.S3_ValueParam_radioCycleTimeCounter.WriteParameterToConnectedDevice();
      }
      TestWindow.S3_LoRaTestLogger.Trace("Move device time. Shift seconds: " + num.ToString());
      this.UpdateLoRaValues();
      this.ShowTimes();
      this.textBoxState.AppendText(Environment.NewLine + "PC_Time:" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "; EventTime:" + dateTime.ToString("dd.MM.yyyy HH:mm:ss") + "; Packet:" + this.NextPacket.ToString());
      if (this.textBoxState.Lines.Length <= 20)
        return;
      this.textBoxState.Select(0, this.textBoxState.Lines[0].Length + 2);
      this.textBoxState.SelectedText.Remove(0);
    }

    private void ShowTimes()
    {
      this.textBoxDeviceTime.Text = this.MeterTime.ToString("dd.MM.yyyy HH:mm:ss");
      this.textBoxNextLoRaScenarioEventTime.Text = this.NextLoRaScenarioEventTime.ToString("dd.MM.yyyy HH:mm:ss");
      this.textBoxNextLoRaDiagnosticEventTime.Text = this.GetNextDiagnosticTimeString();
    }

    private string GetNextDiagnosticTimeString()
    {
      return this.NextLoRaDiagnosticEventTime == TestWindow.NotDefinedTime ? "---" : this.NextLoRaDiagnosticEventTime.ToString("dd.MM.yyyy HH:mm:ss");
    }

    private void timerLoRa_Tick(object sender, EventArgs e)
    {
      this.MeterTime = this.MeterTime.AddSeconds(1.0);
      this.textBoxDeviceTime.Text = this.MeterTime.ToString("dd.MM.yyyy HH:mm:ss");
      if (!(this.LoRaAutoNextTime > DateTime.MinValue) || !(this.LoRaAutoNextTime <= DateTime.Now))
        return;
      if (this.LoRaAutoMoveTime)
      {
        this.MoveToNextLoRaEvent();
        this.LoRaAutoNextTime = DateTime.Now.AddSeconds(6.0);
        this.LoRaAutoMoveTime = false;
      }
      else
      {
        this.UpdateLoRaValues();
        this.ShowTimes();
        this.LoRaAutoNextTime = DateTime.Now.AddSeconds(3.0);
        this.LoRaAutoMoveTime = true;
      }
    }

    private void buttonMoveToNextLoRaEvent_Click(object sender, EventArgs e)
    {
      this.MoveToNextLoRaEvent();
    }

    private void TestWindow_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.timerLoRa.Stop();
    }

    private void buttonAutoRunAllLoRaEvents_Click(object sender, EventArgs e)
    {
      TestWindow.S3_LoRaTestLogger.Trace("Start AutoRunAllLoRaEvents");
      this.buttonUpdateLoRaState.Enabled = false;
      this.buttonMoveToNextLoRaEvent.Enabled = false;
      this.buttonAutoRunAllLoRaEvents.Enabled = false;
      this.buttonBreakLoop.Enabled = true;
      this.LoRaAutoNextTime = DateTime.Now.AddSeconds(3.0);
      this.LoRaAutoMoveTime = true;
      this.UpdateLoRaValues();
      this.ShowTimes();
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
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.groupBoxInterfaceCommands = new GroupBox();
      this.buttonOpto1hour = new Button();
      this.buttonOptoStop = new Button();
      this.buttonReset = new Button();
      this.buttonBreakLoop = new Button();
      this.buttonPulsProgramLoop = new Button();
      this.groupBoxTestCommands = new GroupBox();
      this.buttonShowIdentObject = new Button();
      this.buttonGetLcdDisplay = new Button();
      this.buttonGetVolumenHardwareCounterValue = new Button();
      this.buttonRunIdTest = new Button();
      this.buttonShowLcdDisplayValues = new Button();
      this.buttonShowReadoutValues = new Button();
      this.buttonShowConfigParam = new Button();
      this.buttonLoadAndRunTestScript = new Button();
      this.textBoxState = new TextBox();
      this.groupBoxSingleStep = new GroupBox();
      this.textBoxScriptName = new TextBox();
      this.buttonSingleStepContinue = new Button();
      this.checkBoxSingleStepOn = new CheckBox();
      this.groupBoxLoRaTest = new GroupBox();
      this.textBoxNextLoRaDiagnosticEventTime = new TextBox();
      this.label4 = new Label();
      this.textBoxNextLoRaScenarioEventTime = new TextBox();
      this.label3 = new Label();
      this.textBoxDeviceTime = new TextBox();
      this.label2 = new Label();
      this.textBoxLoRaScenario = new TextBox();
      this.label1 = new Label();
      this.buttonMoveToNextLoRaEvent = new Button();
      this.buttonUpdateLoRaState = new Button();
      this.timerLoRa = new System.Windows.Forms.Timer(this.components);
      this.buttonAutoRunAllLoRaEvents = new Button();
      this.groupBoxInterfaceCommands.SuspendLayout();
      this.groupBoxTestCommands.SuspendLayout();
      this.groupBoxSingleStep.SuspendLayout();
      this.groupBoxLoRaTest.SuspendLayout();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(1117, 45);
      this.zennerCoroprateDesign2.TabIndex = 17;
      this.groupBoxInterfaceCommands.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.groupBoxInterfaceCommands.Controls.Add((Control) this.buttonOpto1hour);
      this.groupBoxInterfaceCommands.Controls.Add((Control) this.buttonOptoStop);
      this.groupBoxInterfaceCommands.Location = new Point(707, 50);
      this.groupBoxInterfaceCommands.Name = "groupBoxInterfaceCommands";
      this.groupBoxInterfaceCommands.Size = new Size(213, 82);
      this.groupBoxInterfaceCommands.TabIndex = 18;
      this.groupBoxInterfaceCommands.TabStop = false;
      this.groupBoxInterfaceCommands.Text = "Optical interface commands";
      this.buttonOpto1hour.Location = new Point(6, 48);
      this.buttonOpto1hour.Name = "buttonOpto1hour";
      this.buttonOpto1hour.Size = new Size(201, 23);
      this.buttonOpto1hour.TabIndex = 19;
      this.buttonOpto1hour.Text = "Optical timeout 1 hour";
      this.buttonOpto1hour.UseVisualStyleBackColor = true;
      this.buttonOpto1hour.Click += new System.EventHandler(this.buttonOpto1hour_Click);
      this.buttonOptoStop.Location = new Point(6, 19);
      this.buttonOptoStop.Name = "buttonOptoStop";
      this.buttonOptoStop.Size = new Size(201, 23);
      this.buttonOptoStop.TabIndex = 19;
      this.buttonOptoStop.Text = "Optical interface stop";
      this.buttonOptoStop.UseVisualStyleBackColor = true;
      this.buttonOptoStop.Click += new System.EventHandler(this.buttonOptoStop_Click);
      this.buttonReset.Location = new Point(6, 19);
      this.buttonReset.Name = "buttonReset";
      this.buttonReset.Size = new Size(201, 23);
      this.buttonReset.TabIndex = 19;
      this.buttonReset.Text = "Reset device";
      this.buttonReset.UseVisualStyleBackColor = true;
      this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
      this.buttonBreakLoop.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonBreakLoop.Enabled = false;
      this.buttonBreakLoop.Location = new Point(946, 617);
      this.buttonBreakLoop.Name = "buttonBreakLoop";
      this.buttonBreakLoop.Size = new Size(163, 23);
      this.buttonBreakLoop.TabIndex = 21;
      this.buttonBreakLoop.Text = "break loop";
      this.buttonBreakLoop.UseVisualStyleBackColor = true;
      this.buttonBreakLoop.Click += new System.EventHandler(this.buttonBreakLoop_Click);
      this.buttonPulsProgramLoop.Location = new Point(6, 48);
      this.buttonPulsProgramLoop.Name = "buttonPulsProgramLoop";
      this.buttonPulsProgramLoop.Size = new Size(201, 23);
      this.buttonPulsProgramLoop.TabIndex = 19;
      this.buttonPulsProgramLoop.Text = "Pulse program loop";
      this.buttonPulsProgramLoop.UseVisualStyleBackColor = true;
      this.buttonPulsProgramLoop.Click += new System.EventHandler(this.buttonPulsProgramLoop_Click);
      this.groupBoxTestCommands.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
      this.groupBoxTestCommands.Controls.Add((Control) this.buttonShowIdentObject);
      this.groupBoxTestCommands.Controls.Add((Control) this.buttonGetLcdDisplay);
      this.groupBoxTestCommands.Controls.Add((Control) this.buttonGetVolumenHardwareCounterValue);
      this.groupBoxTestCommands.Controls.Add((Control) this.buttonRunIdTest);
      this.groupBoxTestCommands.Controls.Add((Control) this.buttonShowLcdDisplayValues);
      this.groupBoxTestCommands.Controls.Add((Control) this.buttonShowReadoutValues);
      this.groupBoxTestCommands.Controls.Add((Control) this.buttonShowConfigParam);
      this.groupBoxTestCommands.Controls.Add((Control) this.buttonReset);
      this.groupBoxTestCommands.Controls.Add((Control) this.buttonPulsProgramLoop);
      this.groupBoxTestCommands.Location = new Point(707, 138);
      this.groupBoxTestCommands.Name = "groupBoxTestCommands";
      this.groupBoxTestCommands.Size = new Size(213, 331);
      this.groupBoxTestCommands.TabIndex = 22;
      this.groupBoxTestCommands.TabStop = false;
      this.groupBoxTestCommands.Text = "Test commands";
      this.buttonShowIdentObject.Location = new Point(6, 291);
      this.buttonShowIdentObject.Name = "buttonShowIdentObject";
      this.buttonShowIdentObject.Size = new Size(201, 23);
      this.buttonShowIdentObject.TabIndex = 19;
      this.buttonShowIdentObject.Text = "Show ident object";
      this.buttonShowIdentObject.UseVisualStyleBackColor = true;
      this.buttonShowIdentObject.Click += new System.EventHandler(this.buttonShowIdentObject_Click);
      this.buttonGetLcdDisplay.Location = new Point(5, 251);
      this.buttonGetLcdDisplay.Name = "buttonGetLcdDisplay";
      this.buttonGetLcdDisplay.Size = new Size(201, 23);
      this.buttonGetLcdDisplay.TabIndex = 19;
      this.buttonGetLcdDisplay.Text = "Get LCD display";
      this.buttonGetLcdDisplay.UseVisualStyleBackColor = true;
      this.buttonGetLcdDisplay.Click += new System.EventHandler(this.buttonGetLcdDisplay_Click);
      this.buttonGetVolumenHardwareCounterValue.Location = new Point(5, 222);
      this.buttonGetVolumenHardwareCounterValue.Name = "buttonGetVolumenHardwareCounterValue";
      this.buttonGetVolumenHardwareCounterValue.Size = new Size(201, 23);
      this.buttonGetVolumenHardwareCounterValue.TabIndex = 19;
      this.buttonGetVolumenHardwareCounterValue.Text = "Get volume hardware counter";
      this.buttonGetVolumenHardwareCounterValue.UseVisualStyleBackColor = true;
      this.buttonGetVolumenHardwareCounterValue.Click += new System.EventHandler(this.buttonGetVolumenHardwareCounterValue_Click);
      this.buttonRunIdTest.Location = new Point(5, 193);
      this.buttonRunIdTest.Name = "buttonRunIdTest";
      this.buttonRunIdTest.Size = new Size(201, 23);
      this.buttonRunIdTest.TabIndex = 19;
      this.buttonRunIdTest.Text = "Run IO test";
      this.buttonRunIdTest.UseVisualStyleBackColor = true;
      this.buttonRunIdTest.Click += new System.EventHandler(this.buttonRunIdTest_Click);
      this.buttonShowLcdDisplayValues.Location = new Point(6, 148);
      this.buttonShowLcdDisplayValues.Name = "buttonShowLcdDisplayValues";
      this.buttonShowLcdDisplayValues.Size = new Size(201, 23);
      this.buttonShowLcdDisplayValues.TabIndex = 19;
      this.buttonShowLcdDisplayValues.Text = "Show LCD display values";
      this.buttonShowLcdDisplayValues.UseVisualStyleBackColor = true;
      this.buttonShowLcdDisplayValues.Click += new System.EventHandler(this.buttonShowLcdDisplayValues_Click);
      this.buttonShowReadoutValues.Location = new Point(6, 119);
      this.buttonShowReadoutValues.Name = "buttonShowReadoutValues";
      this.buttonShowReadoutValues.Size = new Size(201, 23);
      this.buttonShowReadoutValues.TabIndex = 19;
      this.buttonShowReadoutValues.Text = "Show readout values";
      this.buttonShowReadoutValues.UseVisualStyleBackColor = true;
      this.buttonShowReadoutValues.Click += new System.EventHandler(this.buttonShowReadoutValues_Click);
      this.buttonShowConfigParam.Location = new Point(6, 90);
      this.buttonShowConfigParam.Name = "buttonShowConfigParam";
      this.buttonShowConfigParam.Size = new Size(201, 23);
      this.buttonShowConfigParam.TabIndex = 19;
      this.buttonShowConfigParam.Text = "Show config parameters";
      this.buttonShowConfigParam.UseVisualStyleBackColor = true;
      this.buttonShowConfigParam.Click += new System.EventHandler(this.buttonShowConfigParam_Click);
      this.buttonLoadAndRunTestScript.Location = new Point(114, 41);
      this.buttonLoadAndRunTestScript.Name = "buttonLoadAndRunTestScript";
      this.buttonLoadAndRunTestScript.Size = new Size(93, 23);
      this.buttonLoadAndRunTestScript.TabIndex = 19;
      this.buttonLoadAndRunTestScript.Text = "Load";
      this.buttonLoadAndRunTestScript.UseVisualStyleBackColor = true;
      this.buttonLoadAndRunTestScript.Click += new System.EventHandler(this.buttonLoadAndRunTestScript_Click);
      this.textBoxState.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxState.Location = new Point(12, 50);
      this.textBoxState.Multiline = true;
      this.textBoxState.Name = "textBoxState";
      this.textBoxState.ScrollBars = ScrollBars.Both;
      this.textBoxState.Size = new Size(689, 590);
      this.textBoxState.TabIndex = 23;
      this.groupBoxSingleStep.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.groupBoxSingleStep.Controls.Add((Control) this.textBoxScriptName);
      this.groupBoxSingleStep.Controls.Add((Control) this.buttonLoadAndRunTestScript);
      this.groupBoxSingleStep.Controls.Add((Control) this.buttonSingleStepContinue);
      this.groupBoxSingleStep.Controls.Add((Control) this.checkBoxSingleStepOn);
      this.groupBoxSingleStep.Location = new Point(707, 475);
      this.groupBoxSingleStep.Name = "groupBoxSingleStep";
      this.groupBoxSingleStep.Size = new Size(213, 99);
      this.groupBoxSingleStep.TabIndex = 24;
      this.groupBoxSingleStep.TabStop = false;
      this.groupBoxSingleStep.Text = "Script";
      this.textBoxScriptName.Location = new Point(6, 15);
      this.textBoxScriptName.Name = "textBoxScriptName";
      this.textBoxScriptName.ReadOnly = true;
      this.textBoxScriptName.Size = new Size(200, 20);
      this.textBoxScriptName.TabIndex = 20;
      this.buttonSingleStepContinue.Location = new Point(114, 70);
      this.buttonSingleStepContinue.Name = "buttonSingleStepContinue";
      this.buttonSingleStepContinue.Size = new Size(92, 23);
      this.buttonSingleStepContinue.TabIndex = 1;
      this.buttonSingleStepContinue.Text = "Run";
      this.buttonSingleStepContinue.UseVisualStyleBackColor = true;
      this.buttonSingleStepContinue.Click += new System.EventHandler(this.buttonSingleStepContinue_Click);
      this.checkBoxSingleStepOn.AutoSize = true;
      this.checkBoxSingleStepOn.Location = new Point(6, 70);
      this.checkBoxSingleStepOn.Name = "checkBoxSingleStepOn";
      this.checkBoxSingleStepOn.Size = new Size(93, 17);
      this.checkBoxSingleStepOn.TabIndex = 0;
      this.checkBoxSingleStepOn.Text = "Single step on";
      this.checkBoxSingleStepOn.UseVisualStyleBackColor = true;
      this.checkBoxSingleStepOn.CheckedChanged += new System.EventHandler(this.checkBoxSingleStepOn_CheckedChanged);
      this.groupBoxLoRaTest.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.groupBoxLoRaTest.Controls.Add((Control) this.textBoxNextLoRaDiagnosticEventTime);
      this.groupBoxLoRaTest.Controls.Add((Control) this.label4);
      this.groupBoxLoRaTest.Controls.Add((Control) this.textBoxNextLoRaScenarioEventTime);
      this.groupBoxLoRaTest.Controls.Add((Control) this.label3);
      this.groupBoxLoRaTest.Controls.Add((Control) this.textBoxDeviceTime);
      this.groupBoxLoRaTest.Controls.Add((Control) this.label2);
      this.groupBoxLoRaTest.Controls.Add((Control) this.textBoxLoRaScenario);
      this.groupBoxLoRaTest.Controls.Add((Control) this.label1);
      this.groupBoxLoRaTest.Controls.Add((Control) this.buttonAutoRunAllLoRaEvents);
      this.groupBoxLoRaTest.Controls.Add((Control) this.buttonMoveToNextLoRaEvent);
      this.groupBoxLoRaTest.Controls.Add((Control) this.buttonUpdateLoRaState);
      this.groupBoxLoRaTest.Location = new Point(926, 50);
      this.groupBoxLoRaTest.Name = "groupBoxLoRaTest";
      this.groupBoxLoRaTest.Size = new Size(179, 287);
      this.groupBoxLoRaTest.TabIndex = 25;
      this.groupBoxLoRaTest.TabStop = false;
      this.groupBoxLoRaTest.Text = "LoRa Test";
      this.textBoxNextLoRaDiagnosticEventTime.Location = new Point(6, 164);
      this.textBoxNextLoRaDiagnosticEventTime.Name = "textBoxNextLoRaDiagnosticEventTime";
      this.textBoxNextLoRaDiagnosticEventTime.Size = new Size(167, 20);
      this.textBoxNextLoRaDiagnosticEventTime.TabIndex = 2;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 147);
      this.label4.Name = "label4";
      this.label4.Size = new Size(164, 13);
      this.label4.TabIndex = 1;
      this.label4.Text = "Next LoRa diagnostic event time:";
      this.textBoxNextLoRaScenarioEventTime.Location = new Point(6, 123);
      this.textBoxNextLoRaScenarioEventTime.Name = "textBoxNextLoRaScenarioEventTime";
      this.textBoxNextLoRaScenarioEventTime.Size = new Size(167, 20);
      this.textBoxNextLoRaScenarioEventTime.TabIndex = 2;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(6, 106);
      this.label3.Name = "label3";
      this.label3.Size = new Size(156, 13);
      this.label3.TabIndex = 1;
      this.label3.Text = "Next LoRa scenario event time:";
      this.textBoxDeviceTime.Location = new Point(6, 82);
      this.textBoxDeviceTime.Name = "textBoxDeviceTime";
      this.textBoxDeviceTime.Size = new Size(167, 20);
      this.textBoxDeviceTime.TabIndex = 2;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 65);
      this.label2.Name = "label2";
      this.label2.Size = new Size(66, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Device time:";
      this.textBoxLoRaScenario.Location = new Point(6, 41);
      this.textBoxLoRaScenario.Name = "textBoxLoRaScenario";
      this.textBoxLoRaScenario.Size = new Size(167, 20);
      this.textBoxLoRaScenario.TabIndex = 2;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 24);
      this.label1.Name = "label1";
      this.label1.Size = new Size(52, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Scenario:";
      this.buttonMoveToNextLoRaEvent.Enabled = false;
      this.buttonMoveToNextLoRaEvent.Location = new Point(6, 230);
      this.buttonMoveToNextLoRaEvent.Name = "buttonMoveToNextLoRaEvent";
      this.buttonMoveToNextLoRaEvent.Size = new Size(167, 23);
      this.buttonMoveToNextLoRaEvent.TabIndex = 0;
      this.buttonMoveToNextLoRaEvent.Text = "Move to next LoRa event";
      this.buttonMoveToNextLoRaEvent.UseVisualStyleBackColor = true;
      this.buttonMoveToNextLoRaEvent.Click += new System.EventHandler(this.buttonMoveToNextLoRaEvent_Click);
      this.buttonUpdateLoRaState.Location = new Point(6, 201);
      this.buttonUpdateLoRaState.Name = "buttonUpdateLoRaState";
      this.buttonUpdateLoRaState.Size = new Size(167, 23);
      this.buttonUpdateLoRaState.TabIndex = 0;
      this.buttonUpdateLoRaState.Text = "Update LoRa State";
      this.buttonUpdateLoRaState.UseVisualStyleBackColor = true;
      this.buttonUpdateLoRaState.Click += new System.EventHandler(this.buttonUpdateLoRaState_Click);
      this.timerLoRa.Tick += new System.EventHandler(this.timerLoRa_Tick);
      this.buttonAutoRunAllLoRaEvents.Enabled = false;
      this.buttonAutoRunAllLoRaEvents.Location = new Point(6, 258);
      this.buttonAutoRunAllLoRaEvents.Name = "buttonAutoRunAllLoRaEvents";
      this.buttonAutoRunAllLoRaEvents.Size = new Size(167, 23);
      this.buttonAutoRunAllLoRaEvents.TabIndex = 0;
      this.buttonAutoRunAllLoRaEvents.Text = "Auto run all LoRa events";
      this.buttonAutoRunAllLoRaEvents.UseVisualStyleBackColor = true;
      this.buttonAutoRunAllLoRaEvents.Click += new System.EventHandler(this.buttonAutoRunAllLoRaEvents_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1117, 652);
      this.Controls.Add((Control) this.groupBoxLoRaTest);
      this.Controls.Add((Control) this.groupBoxSingleStep);
      this.Controls.Add((Control) this.textBoxState);
      this.Controls.Add((Control) this.groupBoxTestCommands);
      this.Controls.Add((Control) this.buttonBreakLoop);
      this.Controls.Add((Control) this.groupBoxInterfaceCommands);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Name = nameof (TestWindow);
      this.Text = nameof (TestWindow);
      this.FormClosing += new FormClosingEventHandler(this.TestWindow_FormClosing);
      this.groupBoxInterfaceCommands.ResumeLayout(false);
      this.groupBoxTestCommands.ResumeLayout(false);
      this.groupBoxSingleStep.ResumeLayout(false);
      this.groupBoxSingleStep.PerformLayout();
      this.groupBoxLoRaTest.ResumeLayout(false);
      this.groupBoxLoRaTest.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: S3_Handler.MoreTests
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using CommonWPF;
using CorporateDesign;
using DeviceCollector;
using GmmDbLib;
using HandlerLib;
using HandlerLib.MapManagement;
using HandlerLib.View;
using MBusLib;
using StartupLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;
using ZR_ClassLibrary.Schema_Access;

#nullable disable
namespace S3_Handler
{
  public class MoreTests : Form
  {
    private const string ClassMessageName = "S3_Handler MoreTests";
    private S3_HandlerFunctions MyFunctions;
    private S3_Meter MyMeter;
    private bool breakLoop;
    private string breakText = "break volume input";
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign2;
    private GroupBox groupBoxRadioTests;
    private Button buttonRadioTestStop;
    private Button buttonRadio3_Packet;
    private Button buttonWMBus_T_PN9;
    private Button buttonWMBus_S_PN9;
    private Button buttonRadio3_PN9;
    private Button buttonWMBus_T_Center;
    private Button buttonRadio2_PN9;
    private Button buttonWMBus_S_Center;
    private Button buttonRadio3Center;
    private Button buttonRadio2Center;
    private TextBox textBoxMessages;
    private Button buttonReadDeviceState;
    private GroupBox groupBoxWriteProtection;
    private Button buttonWrProtSetKey;
    private Button buttonWrProtReset;
    private Button buttonWrProtSet;
    private Button buttonWrProtGet;
    private Label label1;
    private TextBox textBoxMeterKey;
    private Button buttonSetTestValues;
    private GroupBox groupBoxAccumulatedValues;
    private Button buttonAddBigTestValues;
    private Button buttonAddSmallTestValues;
    private Button buttonShowAccumulatedValues;
    private Button btnStartRTCcal;
    private GroupBox grBxRtcCalibration;
    private Button btnStopRTCcal;
    private Button buttonSetTestModeCapacityOff;
    private Button btnNFCTest;
    private Button buttonNewIdTest;
    private Button buttonCrateTestClass;
    private GroupBox groupBox1;
    private Button buttonAdjustFirmwarePointer;
    private Button buttonShowVolSimulator;
    private Button buttonHardwareTypeManager;
    private GroupBox groupBox2;
    private Button buttonStartStopVolumeInputStateLoop;
    private Button buttonSaveMapCompareFile;
    private Button buttonHardCodedSpecialTest;
    private Button buttonShowVmcpTool;
    private GroupBox groupBox3;
    private Button buttonOverwriteTempFromBackup;
    private GroupBox groupBox4;
    private Button ButtonReceiveTestPacket;
    private Button ButtonSendTestPacket;
    private Button ButtonSendTestPacketViaMiCon;
    private Button ButtonReceiveTestPacketViaMiCon;
    private GroupBox groupBox5;
    private Button buttonSetID_FromDatabase;
    private TextBox textBoxExistingMeterID;
    private Label label2;
    private Button buttonShow_IO_Test;
    private GroupBox groupBoxTransmitListsByCommand;
    private Button BtnTransmitlistsRead;
    private ComboBox cmb_encMode;
    private Label label5;
    private Label label3;
    private TextBox txtBxRadioCycletime;
    private Label label4;
    private ComboBox comboBoxRadioMode;
    private GroupBox groupBox7;
    private Button BtnTransmitlistSetRadio;
    private Button BtnTransmitlistSetMBus;
    private Button buttonSetRadioListAndParameters;
    private Label label7;
    private Label label6;
    private ComboBox comboBoxRadioListNumbers;
    private ComboBox comboBoxMBusListNumber;
    private Button buttonResetVolumeMeterPipe;
    private Button buttonShowVolumePipeSetup;
    private Button buttonReadDeviceDescriptor;
    private Button buttonRunVMCP_Simulator;

    internal MoreTests(S3_HandlerFunctions MyFunctions, S3_Meter MyMeter)
    {
      this.MyFunctions = MyFunctions;
      this.MyMeter = MyMeter;
      this.InitializeComponent();
      this.cmb_encMode.Items.AddRange((object[]) Enum.GetNames(typeof (AES_ENCRYPTION_MODE)));
      this.comboBoxRadioMode.Items.AddRange((object[]) Enum.GetNames(typeof (RADIO_MODE)));
    }

    private void buttonRadio2Center_Click(object sender, EventArgs e)
    {
      this.SetRadioTest(RadioTestMode.Radio2Center);
    }

    private void buttonRadio2_PN9_Click(object sender, EventArgs e)
    {
      this.SetRadioTest(RadioTestMode.Radio2_PN9);
    }

    private void buttonRadio3Center_Click(object sender, EventArgs e)
    {
      this.SetRadioTest(RadioTestMode.Radio3Center);
    }

    private void buttonRadio3_PN9_Click(object sender, EventArgs e)
    {
      this.SetRadioTest(RadioTestMode.Radio3_PN9);
    }

    private void buttonWMBus_S_Center_Click(object sender, EventArgs e)
    {
      this.SetRadioTest(RadioTestMode.WMBus_S_Center);
    }

    private void buttonWMBus_S_PN9_Click(object sender, EventArgs e)
    {
      this.SetRadioTest(RadioTestMode.WMBus_S_PN9);
    }

    private void buttonWMBus_T_Center_Click(object sender, EventArgs e)
    {
      this.SetRadioTest(RadioTestMode.WMBus_T_Center);
    }

    private void buttonWMBus_T_PN9_Click(object sender, EventArgs e)
    {
      this.SetRadioTest(RadioTestMode.WMBus_T_PN9);
    }

    private void buttonRadio3_Packet_Click(object sender, EventArgs e)
    {
      this.SetRadioTest(RadioTestMode.Radio3_Packet);
    }

    private void btnStartRTCcal_Click(object sender, EventArgs e)
    {
      this.TestStart("Start 512Hz Quarz calibration");
      this.TestEnd(this.MyFunctions.MyCommands.Start512HzRtcCalibration());
    }

    private void btnStopRTCcal_Click(object sender, EventArgs e)
    {
      this.TestStart("Stop Quarz Calibration");
      this.TestEnd(this.MyFunctions.MyCommands.TestDone(0L));
    }

    private void buttonRadioTestStop_Click(object sender, EventArgs e)
    {
      this.TestStart("Stop radio test");
      this.TestEnd(this.MyFunctions.MyCommands.TestDone(0L));
    }

    private void SetRadioTest(RadioTestMode radioTestMode)
    {
      this.TestStart(radioTestMode.ToString());
      this.TestEnd(this.MyFunctions.MyCommands.RadioTestActivate(radioTestMode));
    }

    private void TestStart(string testName)
    {
      this.Cursor = Cursors.WaitCursor;
      this.groupBoxRadioTests.Enabled = false;
      this.textBoxMessages.Text = "Function: " + testName;
      this.textBoxMessages.Refresh();
      System.Windows.Forms.Application.DoEvents();
      ZR_ClassLibMessages.ClearErrors();
    }

    private void TestEnd(bool funktionOK)
    {
      this.Cursor = Cursors.Default;
      this.groupBoxRadioTests.Enabled = true;
      this.textBoxMessages.AppendText(ZR_Constants.SystemNewLine);
      if (funktionOK)
        this.textBoxMessages.AppendText("--> ok");
      else
        this.textBoxMessages.AppendText("--> !!! error !!!");
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonReadDeviceState_Click(object sender, EventArgs e)
    {
      this.TestStart("Read device state");
      DeviceState deviceState = this.MyFunctions.GetDeviceState();
      bool funktionOK = false;
      if (deviceState != null)
      {
        this.textBoxMessages.AppendText(ZR_Constants.SystemNewLine);
        if (deviceState.buttonPressed)
          this.textBoxMessages.AppendText("bottom pressed" + ZR_Constants.SystemNewLine);
        else
          this.textBoxMessages.AppendText("bottom not pressed" + ZR_Constants.SystemNewLine);
        if (deviceState.unlockPressed)
          this.textBoxMessages.AppendText("unlock pressed" + ZR_Constants.SystemNewLine);
        else
          this.textBoxMessages.AppendText("unlock not pressed" + ZR_Constants.SystemNewLine);
        funktionOK = true;
      }
      this.TestEnd(funktionOK);
    }

    private void buttonWrProtSet_Click(object sender, EventArgs e)
    {
      this.TestStart("Set write protection");
      this.TestEnd(this.MyFunctions.MyCommands.DeviceProtectionSet());
    }

    private void buttonWrProtReset_Click(object sender, EventArgs e)
    {
      this.TestStart("Reset write protection");
      this.TestEnd(this.MyFunctions.MyCommands.DeviceProtectionReset(this.GetMeterKey()));
    }

    private void buttonWrProtGet_Click(object sender, EventArgs e)
    {
      this.TestStart("Get write protection");
      bool funktionOK = this.MyFunctions.MyCommands.DeviceProtectionGet();
      if (funktionOK)
      {
        this.textBoxMessages.AppendText(ZR_Constants.SystemNewLine);
        this.textBoxMessages.AppendText("--> Device is protected");
      }
      else
      {
        this.textBoxMessages.AppendText(ZR_Constants.SystemNewLine);
        this.textBoxMessages.AppendText("--> Device is NOT protected or communication error.");
      }
      this.TestEnd(funktionOK);
    }

    private void buttonWrProtSetKey_Click(object sender, EventArgs e)
    {
      this.TestStart("Get write protection");
      uint meterKey = this.GetMeterKey();
      bool funktionOK = this.MyFunctions.MyCommands.DeviceProtectionSetKey(meterKey);
      if (funktionOK)
      {
        this.textBoxMessages.AppendText(ZR_Constants.SystemNewLine);
        if (meterKey == uint.MaxValue)
          this.textBoxMessages.AppendText("--> Key is not defined");
        else
          this.textBoxMessages.AppendText("--> Key is defined");
      }
      else
      {
        this.textBoxMessages.AppendText(ZR_Constants.SystemNewLine);
        this.textBoxMessages.AppendText("--> Key is different defined or communication error.");
      }
      this.TestEnd(funktionOK);
    }

    private uint GetMeterKey()
    {
      try
      {
        if (this.textBoxMeterKey.TextLength == 0)
          return uint.MaxValue;
        string s = this.textBoxMeterKey.Text.Trim();
        return s.ToUpper().StartsWith("0X") ? uint.Parse(s.Substring(2), NumberStyles.HexNumber) : uint.Parse(s);
      }
      catch
      {
        this.textBoxMeterKey.Text = "";
        return uint.MaxValue;
      }
    }

    private void buttonShowAccumulatedValues_Click(object sender, EventArgs e)
    {
      StringBuilder stringBuilder = new StringBuilder();
      double doubleValue1 = this.MyMeter.MyParameters.ParameterByName["Bak_VolSum"].GetDoubleValue();
      stringBuilder.AppendLine("Bak_VolSum = " + doubleValue1.ToString());
      double doubleValue2 = this.MyMeter.MyParameters.ParameterByName["Bak_HeatEnergySum"].GetDoubleValue();
      stringBuilder.AppendLine("Bak_HeatEnergySum = " + doubleValue2.ToString());
      double doubleValue3 = this.MyMeter.MyParameters.ParameterByName["Bak_ColdEnergySum"].GetDoubleValue();
      stringBuilder.AppendLine("Bak_ColdEnergySum = " + doubleValue3.ToString());
      if (this.MyMeter.MyParameters.ParameterByName.IndexOfKey(S3_ParameterNames.Cal_DisplayInput_n_0.ToString()) >= 0)
      {
        uint uintValue = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Cal_DisplayInput_n_0.ToString()].GetUintValue();
        stringBuilder.AppendLine("Cal_DisplayInput_n_0 = " + uintValue.ToString());
      }
      if (this.MyMeter.MyParameters.ParameterByName.IndexOfKey(S3_ParameterNames.Cal_DisplayInput_n_1.ToString()) >= 0)
      {
        uint uintValue = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Cal_DisplayInput_n_1.ToString()].GetUintValue();
        stringBuilder.AppendLine("Cal_DisplayInput_n_1 = " + uintValue.ToString());
      }
      if (this.MyMeter.MyParameters.ParameterByName.IndexOfKey(S3_ParameterNames.Cal_DisplayInput_n_2.ToString()) >= 0)
      {
        uint uintValue = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Cal_DisplayInput_n_2.ToString()].GetUintValue();
        stringBuilder.AppendLine("Cal_DisplayInput_n_2 = " + uintValue.ToString());
      }
      double doubleValue4 = this.MyMeter.MyParameters.ParameterByName["Bak_Tariff0EnergySum"].GetDoubleValue();
      stringBuilder.AppendLine("Bak_Tariff0EnergySum = " + doubleValue4.ToString());
      doubleValue4 = this.MyMeter.MyParameters.ParameterByName["Bak_Tariff1EnergySum"].GetDoubleValue();
      stringBuilder.AppendLine("Bak_Tariff1EnergySum = " + doubleValue4.ToString());
      this.textBoxMessages.Text = stringBuilder.ToString();
    }

    private void buttonSetTestValues_Click(object sender, EventArgs e)
    {
      StringBuilder stringBuilder = new StringBuilder();
      S3_Parameter s3Parameter1 = this.MyMeter.MyParameters.ParameterByName["Bak_VolSum"];
      double NewValue1 = 10123.456;
      stringBuilder.AppendLine("Bak_VolSum = " + NewValue1.ToString());
      s3Parameter1.SetDoubleValue(NewValue1);
      S3_Parameter s3Parameter2 = this.MyMeter.MyParameters.ParameterByName["Bak_HeatEnergySum"];
      double NewValue2 = 20123.456;
      stringBuilder.AppendLine("Bak_HeatEnergySum = " + NewValue2.ToString());
      s3Parameter2.SetDoubleValue(NewValue2);
      S3_Parameter s3Parameter3 = this.MyMeter.MyParameters.ParameterByName["Bak_ColdEnergySum"];
      double NewValue3 = 30123.456;
      stringBuilder.AppendLine("Bak_ColdEnergySum = " + NewValue3.ToString());
      s3Parameter3.SetDoubleValue(NewValue3);
      if (this.MyMeter.MyParameters.ParameterByName.IndexOfKey(S3_ParameterNames.Cal_DisplayInput_n_0.ToString()) >= 0)
      {
        S3_Parameter s3Parameter4 = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Cal_DisplayInput_n_0.ToString()];
        uint NewValue4 = 40123;
        stringBuilder.AppendLine("Cal_DisplayInput_n_0 = " + NewValue4.ToString());
        s3Parameter4.SetUintValue(NewValue4);
      }
      if (this.MyMeter.MyParameters.ParameterByName.IndexOfKey(S3_ParameterNames.Cal_DisplayInput_n_1.ToString()) >= 0)
      {
        S3_Parameter s3Parameter5 = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Cal_DisplayInput_n_1.ToString()];
        uint NewValue5 = 50123;
        stringBuilder.AppendLine("Cal_DisplayInput_n_1 = " + NewValue5.ToString());
        s3Parameter5.SetUintValue(NewValue5);
      }
      if (this.MyMeter.MyParameters.ParameterByName.IndexOfKey(S3_ParameterNames.Cal_DisplayInput_n_2.ToString()) >= 0)
      {
        S3_Parameter s3Parameter6 = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Cal_DisplayInput_n_2.ToString()];
        uint NewValue6 = 60123;
        stringBuilder.AppendLine("Cal_DisplayInput_n_2 = " + NewValue6.ToString());
        s3Parameter6.SetUintValue(NewValue6);
      }
      S3_Parameter s3Parameter7 = this.MyMeter.MyParameters.ParameterByName["Bak_Tariff0EnergySum"];
      double NewValue7 = 70123.456;
      stringBuilder.AppendLine("Bak_Tariff0EnergySum = " + NewValue7.ToString());
      s3Parameter7.SetDoubleValue(NewValue7);
      S3_Parameter s3Parameter8 = this.MyMeter.MyParameters.ParameterByName["Bak_Tariff1EnergySum"];
      NewValue7 = 80123.456;
      stringBuilder.AppendLine("Bak_Tariff1EnergySum = " + NewValue7.ToString());
      s3Parameter8.SetDoubleValue(NewValue7);
      this.textBoxMessages.Text = stringBuilder.ToString();
      this.MyMeter.MyParameters.ReloadDisplyValues();
    }

    private void buttonAddSmallTestValues_Click(object sender, EventArgs e)
    {
      StringBuilder stringBuilder = new StringBuilder();
      S3_Parameter s3Parameter1 = this.MyMeter.MyParameters.ParameterByName["Bak_VolSum"];
      double doubleValue1 = s3Parameter1.GetDoubleValue();
      stringBuilder.Append("Bak_VolSum = " + doubleValue1.ToString());
      double NewValue1 = doubleValue1 + 0.1;
      stringBuilder.AppendLine(" ; changed to: " + NewValue1.ToString());
      s3Parameter1.SetDoubleValue(NewValue1);
      S3_Parameter s3Parameter2 = this.MyMeter.MyParameters.ParameterByName["Bak_HeatEnergySum"];
      double doubleValue2 = s3Parameter2.GetDoubleValue();
      stringBuilder.Append("Bak_HeatEnergySum = " + doubleValue2.ToString());
      double NewValue2 = doubleValue2 + 0.2;
      stringBuilder.AppendLine(" ; changed to: " + NewValue2.ToString());
      s3Parameter2.SetDoubleValue(NewValue2);
      S3_Parameter s3Parameter3 = this.MyMeter.MyParameters.ParameterByName["Bak_ColdEnergySum"];
      double doubleValue3 = s3Parameter3.GetDoubleValue();
      stringBuilder.Append("Bak_ColdEnergySum = " + doubleValue3.ToString());
      double NewValue3 = doubleValue3 + 0.3;
      stringBuilder.AppendLine(" ; changed to: " + NewValue3.ToString());
      s3Parameter3.SetDoubleValue(NewValue3);
      S3_Parameter s3Parameter4 = this.MyMeter.MyParameters.ParameterByName["Cal_DisplayInput_n_0"];
      uint uintValue1 = s3Parameter4.GetUintValue();
      stringBuilder.Append("Cal_DisplayInput_n_0 = " + uintValue1.ToString());
      uint NewValue4 = uintValue1 + 40U;
      stringBuilder.AppendLine(" ; changed to: " + NewValue4.ToString());
      s3Parameter4.SetUintValue(NewValue4);
      S3_Parameter s3Parameter5 = this.MyMeter.MyParameters.ParameterByName["Cal_DisplayInput_n_1"];
      uint uintValue2 = s3Parameter5.GetUintValue();
      stringBuilder.Append("Cal_DisplayInput_n_1 = " + uintValue2.ToString());
      uint NewValue5 = uintValue2 + 50U;
      stringBuilder.AppendLine(" ; changed to: " + NewValue5.ToString());
      s3Parameter5.SetUintValue(NewValue5);
      S3_Parameter s3Parameter6 = this.MyMeter.MyParameters.ParameterByName["Cal_DisplayInput_n_2"];
      uint uintValue3 = s3Parameter6.GetUintValue();
      stringBuilder.Append("Cal_DisplayInput_n_2 = " + uintValue3.ToString());
      uint NewValue6 = uintValue3 + 60U;
      stringBuilder.AppendLine(" ; changed to: " + NewValue6.ToString());
      s3Parameter6.SetUintValue(NewValue6);
      S3_Parameter s3Parameter7 = this.MyMeter.MyParameters.ParameterByName["Bak_Tariff0EnergySum"];
      double doubleValue4 = s3Parameter7.GetDoubleValue();
      stringBuilder.Append("Bak_Tariff0EnergySum = " + doubleValue4.ToString());
      double NewValue7 = doubleValue4 + 0.7;
      stringBuilder.AppendLine(" ; changed to: " + NewValue7.ToString());
      s3Parameter7.SetDoubleValue(NewValue7);
      S3_Parameter s3Parameter8 = this.MyMeter.MyParameters.ParameterByName["Bak_Tariff1EnergySum"];
      double doubleValue5 = s3Parameter8.GetDoubleValue();
      stringBuilder.Append("Bak_Tariff1EnergySum = " + doubleValue5.ToString());
      double NewValue8 = doubleValue5 + 0.8;
      stringBuilder.AppendLine(" ; changed to: " + NewValue8.ToString());
      s3Parameter8.SetDoubleValue(NewValue8);
      this.textBoxMessages.Text = stringBuilder.ToString();
      this.MyMeter.MyParameters.ReloadDisplyValues();
    }

    private void buttonAddBigTestValues_Click(object sender, EventArgs e)
    {
      StringBuilder stringBuilder = new StringBuilder();
      S3_Parameter s3Parameter1 = this.MyMeter.MyParameters.ParameterByName["Bak_VolSum"];
      double doubleValue1 = s3Parameter1.GetDoubleValue();
      stringBuilder.Append("Bak_VolSum = " + doubleValue1.ToString());
      double NewValue1 = doubleValue1 + 10.0;
      stringBuilder.AppendLine(" ; changed to: " + NewValue1.ToString());
      s3Parameter1.SetDoubleValue(NewValue1);
      S3_Parameter s3Parameter2 = this.MyMeter.MyParameters.ParameterByName["Bak_HeatEnergySum"];
      double doubleValue2 = s3Parameter2.GetDoubleValue();
      stringBuilder.Append("Bak_HeatEnergySum = " + doubleValue2.ToString());
      double NewValue2 = doubleValue2 + 20.0;
      stringBuilder.AppendLine(" ; changed to: " + NewValue2.ToString());
      s3Parameter2.SetDoubleValue(NewValue2);
      S3_Parameter s3Parameter3 = this.MyMeter.MyParameters.ParameterByName["Bak_ColdEnergySum"];
      double doubleValue3 = s3Parameter3.GetDoubleValue();
      stringBuilder.Append("Bak_ColdEnergySum = " + doubleValue3.ToString());
      double NewValue3 = doubleValue3 + 30.0;
      stringBuilder.AppendLine(" ; changed to: " + NewValue3.ToString());
      s3Parameter3.SetDoubleValue(NewValue3);
      if (this.MyMeter.MyParameters.ParameterByName.IndexOfKey(S3_ParameterNames.Cal_DisplayInput_n_0.ToString()) >= 0)
      {
        S3_Parameter s3Parameter4 = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Cal_DisplayInput_n_0.ToString()];
        uint uintValue = s3Parameter4.GetUintValue();
        stringBuilder.Append("Cal_DisplayInput_n_0 = " + uintValue.ToString());
        uint NewValue4 = uintValue + 4000U;
        stringBuilder.AppendLine(" ; changed to: " + NewValue4.ToString());
        s3Parameter4.SetUintValue(NewValue4);
      }
      if (this.MyMeter.MyParameters.ParameterByName.IndexOfKey(S3_ParameterNames.Cal_DisplayInput_n_1.ToString()) >= 0)
      {
        S3_Parameter s3Parameter5 = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Cal_DisplayInput_n_1.ToString()];
        uint uintValue = s3Parameter5.GetUintValue();
        stringBuilder.Append("Cal_DisplayInput_n_1 = " + uintValue.ToString());
        uint NewValue5 = uintValue + 5000U;
        stringBuilder.AppendLine(" ; changed to: " + NewValue5.ToString());
        s3Parameter5.SetUintValue(NewValue5);
      }
      if (this.MyMeter.MyParameters.ParameterByName.IndexOfKey(S3_ParameterNames.Cal_DisplayInput_n_2.ToString()) >= 0)
      {
        S3_Parameter s3Parameter6 = this.MyMeter.MyParameters.ParameterByName[S3_ParameterNames.Cal_DisplayInput_n_2.ToString()];
        uint uintValue = s3Parameter6.GetUintValue();
        stringBuilder.Append("Cal_DisplayInput_n_2 = " + uintValue.ToString());
        uint NewValue6 = uintValue + 6000U;
        stringBuilder.AppendLine(" ; changed to: " + NewValue6.ToString());
        s3Parameter6.SetUintValue(NewValue6);
      }
      S3_Parameter s3Parameter7 = this.MyMeter.MyParameters.ParameterByName["Bak_Tariff0EnergySum"];
      double doubleValue4 = s3Parameter7.GetDoubleValue();
      stringBuilder.Append("Bak_Tariff0EnergySum = " + doubleValue4.ToString());
      doubleValue4 += 70.0;
      stringBuilder.AppendLine(" ; changed to: " + doubleValue4.ToString());
      s3Parameter7.SetDoubleValue(doubleValue4);
      S3_Parameter s3Parameter8 = this.MyMeter.MyParameters.ParameterByName["Bak_Tariff1EnergySum"];
      doubleValue4 = s3Parameter8.GetDoubleValue();
      stringBuilder.Append("Bak_Tariff1EnergySum = " + doubleValue4.ToString());
      doubleValue4 += 80.0;
      stringBuilder.AppendLine(" ; changed to: " + doubleValue4.ToString());
      s3Parameter8.SetDoubleValue(doubleValue4);
      this.textBoxMessages.Text = stringBuilder.ToString();
      this.MyMeter.MyParameters.ReloadDisplyValues();
    }

    private void buttonSetTestModeCapacityOff_Click(object sender, EventArgs e)
    {
      this.textBoxMessages.Clear();
      if (!this.MyFunctions.MyCommands.CapacityOfTestActivate())
        this.textBoxMessages.Text = "**** Test activation error ****";
      else
        this.textBoxMessages.Text = "Test activated! Capacity is off!";
    }

    private void buttonNewIdTest_Click(object sender, EventArgs e)
    {
      this.TestWriteWithoutTransaction(3000, 1);
    }

    private void TestWriteWithoutTransaction(int waitSeconds, int testNumber)
    {
      new Thread(new ParameterizedThreadStart(this.WriteIdDataWithoutTransaction))
      {
        Name = ("TestWrite" + testNumber.ToString())
      }.Start((object) new MoreTests.CommunicationClass(waitSeconds, testNumber));
    }

    private void WriteIdDataWithoutTransaction(object stateInfo)
    {
      MoreTests.CommunicationClass communicationClass = (MoreTests.CommunicationClass) stateInfo;
      try
      {
        Schema.MeterDataTable meterDataTable = new Schema.MeterDataTable();
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter("SELECT * FROM Meter WHERE 1=0", newConnection);
          int newId = DbBasis.PrimaryDB.BaseDbConnection.GetNewId("Meter");
          Thread.Sleep(communicationClass.waitMilliSeconds);
          Schema.MeterRow row = meterDataTable.NewMeterRow();
          row.MeterID = newId;
          row.SerialNr = "test:" + communicationClass.testNumber.ToString();
          meterDataTable.AddMeterRow(row);
          dataAdapter.Update((DataTable) meterDataTable);
        }
      }
      catch (Exception ex)
      {
        Debugger.Break();
        throw ex;
      }
    }

    private void TestWrite2(int waitSeconds, int testNumber)
    {
      new Thread(new ParameterizedThreadStart(this.WriteIdData2))
      {
        Name = ("TestWrite" + testNumber.ToString())
      }.Start((object) new MoreTests.CommunicationClass(waitSeconds, testNumber));
    }

    private void WriteIdData2(object stateInfo)
    {
      MoreTests.CommunicationClass communicationClass = (MoreTests.CommunicationClass) stateInfo;
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          newConnection.Open();
          DbTransaction transaction = newConnection.BeginTransaction();
          Schema.MeterDataTable meterDataTable = new Schema.MeterDataTable();
          DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter("SELECT * FROM Meter WHERE 1=0", newConnection, transaction);
          dataAdapter.Fill((DataTable) meterDataTable);
          int newId = DbBasis.PrimaryDB.BaseDbConnection.GetNewId("Meter");
          Thread.Sleep(communicationClass.waitMilliSeconds);
          Schema.MeterRow row = meterDataTable.NewMeterRow();
          row.MeterID = newId;
          row.SerialNr = "test:" + communicationClass.testNumber.ToString();
          meterDataTable.AddMeterRow(row);
          dataAdapter.Update((DataTable) meterDataTable);
          transaction.Commit();
          newConnection.Close();
        }
      }
      catch (Exception ex)
      {
        Debugger.Break();
        throw ex;
      }
    }

    private void TestWrite3(int waitSeconds, int testNumber)
    {
      new Thread(new ParameterizedThreadStart(this.WriteIdData3))
      {
        Name = ("TestWrite" + testNumber.ToString())
      }.Start((object) new MoreTests.CommunicationClass(waitSeconds, testNumber));
    }

    private void WriteIdData3(object stateInfo)
    {
      MoreTests.CommunicationClass communicationClass = (MoreTests.CommunicationClass) stateInfo;
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          newConnection.Open();
          DbTransaction dbTransaction = newConnection.BeginTransaction();
          Schema.MeterDataTable meterDataTable = new Schema.MeterDataTable();
          DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter("SELECT * FROM Meter WHERE 1=0", newConnection);
          dataAdapter.Fill((DataTable) meterDataTable);
          int newId = DbBasis.PrimaryDB.BaseDbConnection.GetNewId("Meter");
          Thread.Sleep(communicationClass.waitMilliSeconds);
          Schema.MeterRow row = meterDataTable.NewMeterRow();
          row.MeterID = newId;
          row.SerialNr = "test:" + communicationClass.testNumber.ToString();
          meterDataTable.AddMeterRow(row);
          dataAdapter.Update((DataTable) meterDataTable);
          dbTransaction.Commit();
          newConnection.Close();
        }
      }
      catch (Exception ex)
      {
        Debugger.Break();
        throw ex;
      }
    }

    private void TestWrite4(int waitSeconds, int testNumber)
    {
      new Thread(new ParameterizedThreadStart(this.WriteIdData4))
      {
        Name = ("TestWrite" + testNumber.ToString())
      }.Start((object) new MoreTests.CommunicationClass(waitSeconds, testNumber));
    }

    private void WriteIdData4(object stateInfo)
    {
      DbCommand dbCommand = (DbCommand) null;
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          newConnection.Open();
          dbCommand = newConnection.CreateCommand();
          dbCommand.Transaction = newConnection.BeginTransaction();
          throw new Exception("test");
        }
      }
      catch (Exception ex)
      {
        if (dbCommand != null && dbCommand.Transaction != null)
          dbCommand.Transaction.Rollback();
        throw ex;
      }
    }

    private void buttonCrateTestClass_Click(object sender, EventArgs e)
    {
      new MapClassManager("S3_Handler", (MapReader) new MapReaderS3(), addressRanges: new List<AddressRange>()
      {
        new AddressRange(6144U, 512U),
        new AddressRange(7168U, 4096U),
        new AddressRange(16384U, 512U),
        new AddressRange(19456U, 10240U)
      }, pathToMapClasses: Path.GetDirectoryName(new StackTrace(true).GetFrame(1).GetFileName())).ShowDialog();
    }

    private void buttonSaveMapCompareFile_Click(object sender, EventArgs e)
    {
      string str = "";
      try
      {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "map class files (MapDefClass*.cs)|MapDefClass*.cs|All files (*.*)|*.*";
        openFileDialog.FilterIndex = 1;
        openFileDialog.RestoreDirectory = true;
        if (openFileDialog.ShowDialog() != DialogResult.OK)
          return;
        string withoutExtension = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
        byte[] fullByteList = ((MapDefClassBase) Activator.CreateInstance(Assembly.GetExecutingAssembly().GetType("S3_Handler." + withoutExtension))).FullByteList;
        SortedList<int, string> sortedList = new SortedList<int, string>();
        int readIndex = 0;
        if (LoadDataFromMapClass.GetByteForced(fullByteList, ref readIndex) > (byte) 0)
          throw new FormatException("Illegal MapDef class format");
        while (readIndex < fullByteList.Length)
        {
          str = LoadDataFromMapClass.GetStringForced(fullByteList, ref readIndex);
          int intForced = LoadDataFromMapClass.GetIntForced(fullByteList, ref readIndex);
          if (sortedList.ContainsKey(intForced))
            sortedList[intForced] = sortedList[intForced] + "; " + str;
          else
            sortedList.Add(intForced, str);
        }
        using (StreamWriter streamWriter = new StreamWriter(Path.Combine(Path.GetDirectoryName(openFileDialog.FileName), withoutExtension + ".compare")))
        {
          foreach (KeyValuePair<int, string> keyValuePair in sortedList)
            streamWriter.WriteLine(keyValuePair.Key.ToString("x04") + ": " + keyValuePair.Value);
        }
      }
      catch (Exception ex)
      {
        throw new FormatException("Illegal parameter config class. Error near parameter:" + str + Environment.NewLine + ex.Message);
      }
    }

    private void buttonAdjustFirmwarePointer_Click(object sender, EventArgs e)
    {
      try
      {
        SortedList<string, int> sortedList = new SortedList<string, int>();
        List<KeyValuePair<string, int>> map;
        if (this.MyFunctions.MyMeters.ConnectedMeter == null)
        {
          S3_DeviceId DeviceId;
          if (!this.MyFunctions.MyMeters.ReadHardwareIdentification(out DeviceId))
            throw new Exception("Connect device error");
          map = MapSelector.GetMap(DeviceId.FirmwareVersion, DeviceId.MapId, this.MyFunctions.MyDatabase);
        }
        else
        {
          S3_DeviceIdentification identification = this.MyFunctions.MyMeters.ConnectedMeter.MyIdentification;
          map = MapSelector.GetMap(identification.FirmwareVersion, identification.MapId, this.MyFunctions.MyDatabase);
        }
        foreach (KeyValuePair<string, int> keyValuePair in map)
        {
          if (!sortedList.ContainsKey(keyValuePair.Key))
            sortedList.Add(keyValuePair.Key, keyValuePair.Value);
        }
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "header files (*.h)|*.h|All files (*.*)|*.*";
        openFileDialog.FilterIndex = 1;
        openFileDialog.RestoreDirectory = true;
        if (openFileDialog.ShowDialog() != DialogResult.OK)
          return;
        string fullPath = Path.GetFullPath(openFileDialog.FileName);
        string directoryName = Path.GetDirectoryName(openFileDialog.FileName);
        string sourceFileName = Path.Combine(directoryName, "Serie3_ProtectedDisplayCodeNew.h");
        string str1 = Path.Combine(directoryName, "Serie3_ProtectedDisplayCodeBak.h");
        bool flag = false;
        int num1 = 0;
        using (StreamReader streamReader = new StreamReader(fullPath.ToString(), Encoding.Default))
        {
          using (StreamWriter streamWriter = new StreamWriter(sourceFileName.ToString(), false, Encoding.Default))
          {
            int num2 = 0;
            while (true)
            {
              string str2 = streamReader.ReadLine();
              if (str2 != null)
              {
                ++num2;
                if (flag)
                {
                  if (str2.StartsWith("//EndOfAddressing"))
                  {
                    flag = false;
                  }
                  else
                  {
                    int num3 = str2.IndexOf("0x");
                    if (num3 >= 0)
                    {
                      int num4 = str2.IndexOf("// &");
                      if (num4 >= 0)
                      {
                        string key = str2.Substring(num4 + 4).Trim();
                        if (sortedList.ContainsKey(key))
                        {
                          int num5 = sortedList[key];
                          str2 = str2.Substring(0, num3 + 2) + num5.ToString("x04") + " //->" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " // &" + key;
                          ++num1;
                        }
                      }
                    }
                  }
                }
                else if (str2.StartsWith("//StartOfAddressing"))
                  flag = true;
                streamWriter.WriteLine(str2);
              }
              else
                break;
            }
            streamWriter.Close();
          }
          streamReader.Close();
          File.Delete(str1);
          File.Move(fullPath, str1);
          File.Move(sourceFileName, fullPath);
          int num6 = (int) System.Windows.Forms.MessageBox.Show(num1.ToString() + " firmware pointers adjusted.");
        }
      }
      catch (Exception ex)
      {
        int num = (int) System.Windows.Forms.MessageBox.Show(ex.ToString());
      }
    }

    private void buttonShowVolSimulator_Click(object sender, EventArgs e)
    {
      VolumeSimulator.RunVolumeSimulator();
    }

    private void buttonHardwareTypeEditor_Click(object sender, EventArgs e)
    {
      string[] HardwareNames = new string[2]
      {
        "WR4",
        "zelsius C5"
      };
      if (this.MyFunctions.MyMeters.WorkMeter != null)
      {
        S3_DeviceId identification = (S3_DeviceId) this.MyFunctions.MyMeters.WorkMeter.MyIdentification;
        new HardwareTypeEditor(HardwareNames, new uint?(identification.FirmwareVersion), new uint?(identification.HardwareMask)).ShowDialog();
      }
      else
        new HardwareTypeEditor(HardwareNames, new uint?(0U)).ShowDialog();
    }

    private void buttonStartStopVolumeInputStateLoop_Click(object sender, EventArgs e)
    {
      S3_Meter checkedTestMeter = this.MyFunctions.checkedTestMeter;
      if (this.buttonStartStopVolumeInputStateLoop.Text == this.breakText)
      {
        this.breakLoop = true;
      }
      else
      {
        string text = this.buttonStartStopVolumeInputStateLoop.Text;
        try
        {
          this.buttonStartStopVolumeInputStateLoop.Text = this.breakText;
          this.MyFunctions.SetRemainingCycleTime((byte) 200);
          S3_Parameter s3Parameter = checkedTestMeter.MyParameters.ParameterByName[S3_ParameterNames.VolInputSetup.ToString()];
          if (checkedTestMeter.MyDeviceMemory.ReadDataFromConnectedDevice(s3Parameter.BlockStartAddress, s3Parameter.ByteSize))
          {
            string str = ((WR4_VOL_INPUT_SETUP) s3Parameter.GetByteValue()).ToString();
            this.breakLoop = false;
            StringBuilder stringBuilder = new StringBuilder();
            int num = 1;
            while (!this.breakLoop)
            {
              stringBuilder.Clear();
              stringBuilder.AppendLine("VolInputSetup: " + str);
              stringBuilder.AppendLine("Update: " + num.ToString());
              ++num;
              ImpulseInputCounters impulseInputCounters = this.MyFunctions.ReadImpulseInputCounters();
              if (impulseInputCounters.ImputState != null)
                stringBuilder.AppendLine("VolInputState: " + impulseInputCounters.ImputState);
              stringBuilder.AppendLine("VolumePulseCounter = " + impulseInputCounters.VolumePulseCounter.ToString() + " = 0x" + impulseInputCounters.VolumePulseCounter.ToString("x04"));
              stringBuilder.AppendLine("HardwareCounter = " + impulseInputCounters.HardwareCounter.ToString() + " = 0x" + impulseInputCounters.HardwareCounter.ToString("x04"));
              stringBuilder.AppendLine("Input0Counter      = " + impulseInputCounters.Input0Counter.ToString() + " = 0x" + impulseInputCounters.Input0Counter.ToString("x04"));
              stringBuilder.AppendLine("Input1Counter      = " + impulseInputCounters.Input1Counter.ToString() + " = 0x" + impulseInputCounters.Input1Counter.ToString("x04"));
              stringBuilder.AppendLine("Input2Counter      = " + impulseInputCounters.Input2Counter.ToString() + " = 0x" + impulseInputCounters.Input2Counter.ToString("x04"));
              this.textBoxMessages.Text = stringBuilder.ToString();
              System.Windows.Forms.Application.DoEvents();
              Thread.Sleep(1000);
            }
          }
        }
        catch (Exception ex)
        {
          this.textBoxMessages.Text = ex.ToString();
        }
        this.buttonStartStopVolumeInputStateLoop.Text = text;
      }
    }

    private void buttonHardCodedSpecialTest_Click(object sender, EventArgs e)
    {
      this.MyFunctions.MyCommands.RunBackup();
    }

    private void buttonShowVmcpTool_Click(object sender, EventArgs e)
    {
      new VMCP_Tool().ShowDialog();
    }

    private void buttonOverwriteTempFromBackup_Click(object sender, EventArgs e)
    {
      try
      {
        this.MyFunctions.OverwriteSrcToDest(HandlerMeterObjects.BackupMeter, HandlerMeterObjects.WorkMeter, new CommonOverwriteGroups[2]
        {
          CommonOverwriteGroups.TemperatureSettings,
          CommonOverwriteGroups.TemperatureLimits
        });
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private async void ButtonSendTestPacket_Click(object sender, EventArgs e)
    {
      byte[] arbitraryData = new byte[28];
      for (int i = 0; i < arbitraryData.Length; ++i)
        arbitraryData[i] = (byte) 85;
      try
      {
        await this.MyFunctions.MyCommands.SendTestPacketAsync((ushort) 1, (ushort) 5, 11111111U, arbitraryData, "0FF0");
        arbitraryData = (byte[]) null;
      }
      catch (Exception ex)
      {
        ErrorMessageBox.ShowDialog((Window) null, ex.Message, ex);
        arbitraryData = (byte[]) null;
      }
    }

    private async void ButtonReceiveTestPacket_Click(object sender, EventArgs e)
    {
      try
      {
        double? nullable = await this.MyFunctions.MyCommands.ReceiveTestPacketAsync((byte) 10, 11111111U, "0FF0");
        double? rssi_dBm = nullable;
        nullable = new double?();
        if (rssi_dBm.HasValue)
        {
          int num1 = (int) System.Windows.Forms.MessageBox.Show("OK. RSSI: " + rssi_dBm.ToString() + " dBm");
        }
        else
        {
          int num2 = (int) System.Windows.Forms.MessageBox.Show("Failed.");
        }
        rssi_dBm = new double?();
      }
      catch (Exception ex)
      {
        ErrorMessageBox.ShowDialog((Window) null, ex.Message, ex);
      }
    }

    private void ButtonSendTestPacketViaMiCon_Click(object sender, EventArgs e)
    {
      SendTestPacketMiConWindow.Show((Window) null);
    }

    private void ButtonReceiveTestPacketViaMiCon_Click(object sender, EventArgs e)
    {
      ReceiveTestPacketMiConWindow.Show((Window) null);
    }

    private void buttonSetID_FromDatabase_Click(object sender, EventArgs e)
    {
      try
      {
        using (DbConnection newConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
        {
          bool flag = false;
          string s = this.textBoxExistingMeterID.Text.Trim();
          if (!string.IsNullOrEmpty(s))
            flag = true;
          int result;
          if (flag)
          {
            if (!int.TryParse(s, out result))
              throw new Exception("Illegal existing MeterID");
          }
          else
            result = DbBasis.PrimaryDB.BaseDbConnection.GetNewId("Meter");
          ConfigurationParameter.ChangeOverValues changeoverSetup = this.MyMeter.GetChangeoverSetup();
          Schema.MeterDataTable meterDataTable = new Schema.MeterDataTable();
          DbDataAdapter dataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter("SELECT * FROM Meter WHERE MeterID = " + result.ToString(), newConnection, out DbCommandBuilder _);
          dataAdapter.Fill((DataTable) meterDataTable);
          int num;
          if (flag)
          {
            if (meterDataTable.Count != 1)
              throw new Exception("Existing MeterID not found");
            num = !meterDataTable[0].IsSerialNrNull() && meterDataTable[0].SerialNr.Length == 14 ? int.Parse(meterDataTable[0].SerialNr.Substring(6)) : DbBasis.PrimaryDB.BaseDbConnection.GetNewId("SerialNumberC5");
          }
          else
          {
            if (meterDataTable.Count > 0)
              throw new Exception("Illegal existing MeterID generated");
            num = DbBasis.PrimaryDB.BaseDbConnection.GetNewId("SerialNumberC5");
          }
          string str = num.ToString("d08");
          string fullSerialNumber;
          switch (changeoverSetup)
          {
            case ConfigurationParameter.ChangeOverValues.Heating:
              fullSerialNumber = "6ZRI89" + str;
              break;
            case ConfigurationParameter.ChangeOverValues.ChangeOver:
              fullSerialNumber = "6ZRI8B" + str;
              break;
            case ConfigurationParameter.ChangeOverValues.Cooling:
              fullSerialNumber = "5ZRI8A" + str;
              break;
            default:
              throw new Exception("Illegal change over setting");
          }
          ulong asDevEuiValue = new IdentificationMapping(fullSerialNumber).GetAsDevEUI_Value();
          if (meterDataTable.Count == 0)
          {
            Schema.MeterRow row = meterDataTable.NewMeterRow();
            row.MeterID = result;
            row.SerialNr = fullSerialNumber;
            meterDataTable.AddMeterRow(row);
          }
          else
            meterDataTable[0].SerialNr = fullSerialNumber;
          DeviceIdentification deviceIdentification = this.MyFunctions.GetDeviceIdentification();
          deviceIdentification.MeterID = new uint?((uint) result);
          deviceIdentification.FullSerialNumber = fullSerialNumber;
          deviceIdentification.PrintedSerialNumberAsString = fullSerialNumber;
          deviceIdentification.LoRa_DevEUI = new ulong?(asDevEuiValue);
          dataAdapter.Update((DataTable) meterDataTable);
          this.textBoxMessages.Text = "MeterID = " + result.ToString() + Environment.NewLine + "PrintedSerialNumber = " + fullSerialNumber;
        }
      }
      catch (Exception ex)
      {
        Debugger.Break();
        throw ex;
      }
    }

    private void buttonShow_IO_Test_Click(object sender, EventArgs e)
    {
      if (!(this.MyFunctions.MyCommands is S3_CommandsCOMPATIBLE))
      {
        int num = (int) System.Windows.Forms.MessageBox.Show("Not supported for selected communication");
      }
      else
        new MBus_IO_Management(((S3_CommandsCOMPATIBLE) this.MyFunctions.MyCommands).myCommands16Bit).ShowDialog();
    }

    private async void BtnTransmitlistsRead_Click(object sender, EventArgs e)
    {
      try
      {
        this.groupBoxTransmitListsByCommand.BackColor = Color.LightSalmon;
        await Task.Delay(100);
        this.BtnTransmitlistSetMBus.Enabled = false;
        this.BtnTransmitlistSetRadio.Enabled = false;
        this.buttonSetRadioListAndParameters.Enabled = false;
        ParameterListInfo Info = ((S3_CommandsCOMPATIBLE) this.MyFunctions.MyCommands).GetTransmitList();
        string[] MBusListNumbers = new string[Info.S3_Device.MaxList];
        for (int i = 0; i < MBusListNumbers.Length; ++i)
          MBusListNumbers[i] = i.ToString();
        this.comboBoxMBusListNumber.Items.Clear();
        this.comboBoxMBusListNumber.Items.AddRange((object[]) MBusListNumbers);
        this.comboBoxMBusListNumber.SelectedIndex = Info.S3_Device.SelectedList;
        string[] radioListNumbers = new string[Info.Radio.MaxList];
        for (int i = 0; i < radioListNumbers.Length; ++i)
          radioListNumbers[i] = i.ToString();
        this.comboBoxRadioListNumbers.Items.Clear();
        this.comboBoxRadioListNumbers.Items.AddRange((object[]) radioListNumbers);
        this.comboBoxRadioListNumbers.SelectedIndex = Info.Radio.SelectedList;
        this.cmb_encMode.SelectedItem = (object) Info.Radio.AES_EncMode.ToString();
        int? cycletime = Info.Radio.Cycletime;
        if (!cycletime.HasValue)
        {
          this.txtBxRadioCycletime.Clear();
        }
        else
        {
          TextBox bxRadioCycletime = this.txtBxRadioCycletime;
          cycletime = Info.Radio.Cycletime;
          string str = cycletime.Value.ToString();
          bxRadioCycletime.Text = str;
        }
        RADIO_MODE? radioMode = Info.Radio.RadioMode;
        if (!radioMode.HasValue)
        {
          this.comboBoxRadioMode.SelectedIndex = -1;
        }
        else
        {
          ComboBox comboBoxRadioMode = this.comboBoxRadioMode;
          radioMode = Info.Radio.RadioMode;
          string str = radioMode.ToString();
          comboBoxRadioMode.SelectedItem = (object) str;
        }
        if (Info.S3_Device.MaxList > 0)
          this.BtnTransmitlistSetMBus.Enabled = true;
        if (Info.Radio.MaxList > 0)
        {
          this.BtnTransmitlistSetRadio.Enabled = true;
          cycletime = Info.Radio.Cycletime;
          if (cycletime.HasValue)
            this.buttonSetRadioListAndParameters.Enabled = true;
        }
        this.groupBoxTransmitListsByCommand.BackColor = Color.LightGreen;
        Info = (ParameterListInfo) null;
        MBusListNumbers = (string[]) null;
        radioListNumbers = (string[]) null;
      }
      catch (Exception ex)
      {
        this.groupBoxTransmitListsByCommand.BackColor = Color.OrangeRed;
        ExceptionViewer.Show(ex);
      }
    }

    private void BtnTransmitlistSetMBus_Click(object sender, EventArgs e)
    {
      try
      {
        this.groupBoxTransmitListsByCommand.BackColor = Color.LightSalmon;
        ByteField DataBlock = new ByteField();
        if (((S3_CommandsCOMPATIBLE) this.MyFunctions.MyCommands).SetTransmitList(ushort.Parse(this.comboBoxMBusListNumber.Text), false, (ushort) this.cmb_encMode.SelectedIndex, out DataBlock))
          DataBlock.GetByteArray();
        this.groupBoxTransmitListsByCommand.BackColor = Color.LightGreen;
      }
      catch (Exception ex)
      {
        this.groupBoxTransmitListsByCommand.BackColor = Color.OrangeRed;
        ExceptionViewer.Show(ex);
      }
    }

    private void BtnTransmitlistSetRadio_Click(object sender, EventArgs e)
    {
      try
      {
        ByteField DataBlock = new ByteField();
        if (((S3_CommandsCOMPATIBLE) this.MyFunctions.MyCommands).SetTransmitList(ushort.Parse(this.comboBoxRadioListNumbers.Text), true, (ushort) 0, (ushort) 0, out DataBlock))
          DataBlock.GetByteArray();
        this.groupBoxTransmitListsByCommand.BackColor = Color.LightGreen;
      }
      catch (Exception ex)
      {
        this.groupBoxTransmitListsByCommand.BackColor = Color.OrangeRed;
        ExceptionViewer.Show(ex);
      }
    }

    private async void buttonSetRadioListAndParameters_Click(object sender, EventArgs e)
    {
      try
      {
        byte listNumber = byte.Parse(this.comboBoxRadioListNumbers.Text);
        RADIO_MODE radioMode;
        if (!Enum.TryParse<RADIO_MODE>(this.comboBoxRadioMode.SelectedItem.ToString(), out radioMode))
          throw new Exception("Illegal radio mode");
        AES_ENCRYPTION_MODE AES_Encryption;
        if (!Enum.TryParse<AES_ENCRYPTION_MODE>(this.cmb_encMode.SelectedItem.ToString(), out AES_Encryption))
          throw new Exception("Illegal encryption");
        ushort intervallSeconds;
        if (!ushort.TryParse(this.txtBxRadioCycletime.Text, out intervallSeconds))
          throw new Exception("Illegal radio cycle time");
        await ((S3_CommandsCOMPATIBLE) this.MyFunctions.MyCommands).SetRadioParametersAsync(listNumber, radioMode, AES_Encryption, intervallSeconds);
        this.groupBoxTransmitListsByCommand.BackColor = Color.LightGreen;
      }
      catch (Exception ex)
      {
        this.groupBoxTransmitListsByCommand.BackColor = Color.OrangeRed;
        ExceptionViewer.Show(ex);
      }
    }

    private void buttonResetVolumeMeterPipe_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.MyFunctions.MyMeters.ConnectedMeter == null)
          throw new Exception("ConnectedMeter not defined");
        this.textBoxMessages.Text = "Reset volume meter pipe setup inside the connected device.";
        System.Windows.Forms.Application.DoEvents();
        S3_Parameter s3Parameter = this.MyFunctions.MyMeters.WorkMeter.MyParameters.ParameterByName[S3_ParameterNames.Device_Setup_2.ToString()];
        ushort NewValue = (ushort) ((uint) s3Parameter.GetUshortValue() | (uint) S3P_Device_Setup_2.DEVICE_SETUP2_FLOW_LINE | (uint) S3P_Device_Setup_2.DEVICE_SETUP2_FLOW_LINE_NOT_SELECTED);
        s3Parameter.SetUshortValue(NewValue);
        this.MyFunctions.MyMeters.WriteChangesToConnectedDevice();
        this.textBoxMessages.Text = "Reset volume meter pipe setup: DONE";
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void buttonShowVolumePipeSetup_Click(object sender, EventArgs e)
    {
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Volume meter pipe setup");
        stringBuilder.AppendLine();
        ushort ushortValue = this.MyFunctions.MyMeters.WorkMeter.MyParameters.ParameterByName[S3_ParameterNames.Device_Setup_2.ToString()].GetUshortValue();
        bool flag1 = ((uint) ushortValue & (uint) S3P_Device_Setup_2.DEVICE_SETUP2_FLOW_LINE) > 0U;
        if (flag1)
          stringBuilder.AppendLine("Volume meter mounting position 'Flow line' selected: inlet");
        else
          stringBuilder.AppendLine("Volume meter mounting position 'Return line' selected: outlet");
        bool flag2 = ((uint) ushortValue & (uint) S3P_Device_Setup_2.DEVICE_SETUP2_FLOW_LINE_NOT_SELECTED) > 0U;
        if (flag2)
        {
          stringBuilder.AppendLine("Volume meter mounting position not finally defined.");
          stringBuilder.AppendLine("   -> Need definition by device wakeup from SLEEP");
        }
        else
          stringBuilder.AppendLine("Volume meter mounting position defined.");
        stringBuilder.AppendLine();
        bool flag3 = ((uint) ushortValue & (uint) S3P_Device_Setup_2.DEVICE_SETUP2_CHANGE_TEMP_IF_FLOW_LINE_SET) > 0U;
        if (flag3)
        {
          stringBuilder.AppendLine("Xchange temperature values if 'Flow line' selected.");
          stringBuilder.AppendLine("   -> Return sensor mounted in volume meter");
          stringBuilder.AppendLine("   -> Asymmetrical temperature measurement");
        }
        else
        {
          stringBuilder.AppendLine("Dont xchange temperature values if flow line selected");
          stringBuilder.AppendLine("   -> Return sensor not mounted in volume meter");
          stringBuilder.AppendLine("   -> Mounting of both temperature sensors by installation");
          stringBuilder.AppendLine("   -> Symmetrical temperature measurement");
        }
        if (!flag2 && flag1 & flag3)
          stringBuilder.AppendLine("!!! Flow and return temperature values are xchanged !!!");
        stringBuilder.AppendLine();
        int index1 = this.MyFunctions.MyMeters.WorkMeter.MyParameters.ParameterByName.IndexOfKey(S3_ParameterNames.Heap_SelectTime.ToString());
        if (index1 >= 0)
        {
          uint uintValue = this.MyFunctions.MyMeters.WorkMeter.MyParameters.ParameterByName.Values[index1].GetUintValue();
          if (uintValue == 0U)
            stringBuilder.AppendLine("Flow line select time not stored.");
          else
            stringBuilder.AppendLine("Flow line select time: " + ZR_Calendar.Cal_GetDateTime(uintValue).ToString("yyyy.MM.dd HH:mm"));
        }
        else
          stringBuilder.AppendLine("Flow line select time not available.");
        stringBuilder.AppendLine();
        int index2 = this.MyFunctions.MyMeters.WorkMeter.MyParameters.ParameterByName.IndexOfKey(S3_ParameterNames.SerDev0_Medium_Generation.ToString());
        if (index2 >= 0)
        {
          string medium = MBusUtil.GetMedium((byte) ((uint) this.MyFunctions.MyMeters.WorkMeter.MyParameters.ParameterByName.Values[index2].GetUshortValue() >> 8));
          stringBuilder.AppendLine("Medium: " + medium);
        }
        this.textBoxMessages.Text = stringBuilder.ToString();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void buttonReadDeviceDescriptor_Click(object sender, EventArgs e)
    {
      try
      {
        S3_DeviceDescriptor deviceDescriptor = this.MyFunctions.ReadDeviceDescriptor();
        this.textBoxMessages.Text = "Device descriptor:" + Environment.NewLine + "Chip: " + deviceDescriptor.Chip.ToString() + Environment.NewLine + "Lot: 0x" + deviceDescriptor.Lot.ToString("x08") + " = " + deviceDescriptor.Lot.ToString();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void buttonRunVMCP_Simulator_Click(object sender, EventArgs e)
    {
      try
      {
        string str = Path.Combine(SystemValues.AppPath, "VMCP_Simulator", "VMCP_Simulator.exe");
        new Process() { StartInfo = { FileName = str } }.Start();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
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
      this.zennerCoroprateDesign2 = new ZennerCoroprateDesign();
      this.groupBoxRadioTests = new GroupBox();
      this.buttonRadioTestStop = new Button();
      this.buttonRadio3_Packet = new Button();
      this.buttonWMBus_T_PN9 = new Button();
      this.buttonWMBus_S_PN9 = new Button();
      this.buttonRadio3_PN9 = new Button();
      this.buttonWMBus_T_Center = new Button();
      this.buttonRadio2_PN9 = new Button();
      this.buttonWMBus_S_Center = new Button();
      this.buttonRadio3Center = new Button();
      this.buttonRadio2Center = new Button();
      this.textBoxMessages = new TextBox();
      this.buttonReadDeviceState = new Button();
      this.groupBoxWriteProtection = new GroupBox();
      this.buttonWrProtSetKey = new Button();
      this.buttonWrProtReset = new Button();
      this.buttonWrProtSet = new Button();
      this.buttonWrProtGet = new Button();
      this.label1 = new Label();
      this.textBoxMeterKey = new TextBox();
      this.buttonSetTestValues = new Button();
      this.groupBoxAccumulatedValues = new GroupBox();
      this.buttonAddBigTestValues = new Button();
      this.buttonAddSmallTestValues = new Button();
      this.buttonShowAccumulatedValues = new Button();
      this.btnStartRTCcal = new Button();
      this.grBxRtcCalibration = new GroupBox();
      this.btnStopRTCcal = new Button();
      this.buttonSetTestModeCapacityOff = new Button();
      this.btnNFCTest = new Button();
      this.buttonNewIdTest = new Button();
      this.buttonCrateTestClass = new Button();
      this.groupBox1 = new GroupBox();
      this.buttonHardwareTypeManager = new Button();
      this.buttonAdjustFirmwarePointer = new Button();
      this.buttonSaveMapCompareFile = new Button();
      this.buttonShowVolSimulator = new Button();
      this.groupBox2 = new GroupBox();
      this.buttonStartStopVolumeInputStateLoop = new Button();
      this.buttonHardCodedSpecialTest = new Button();
      this.buttonShowVmcpTool = new Button();
      this.groupBox3 = new GroupBox();
      this.buttonOverwriteTempFromBackup = new Button();
      this.groupBox4 = new GroupBox();
      this.ButtonReceiveTestPacketViaMiCon = new Button();
      this.ButtonSendTestPacketViaMiCon = new Button();
      this.ButtonReceiveTestPacket = new Button();
      this.ButtonSendTestPacket = new Button();
      this.groupBox5 = new GroupBox();
      this.textBoxExistingMeterID = new TextBox();
      this.label2 = new Label();
      this.buttonSetID_FromDatabase = new Button();
      this.buttonShow_IO_Test = new Button();
      this.groupBoxTransmitListsByCommand = new GroupBox();
      this.groupBox7 = new GroupBox();
      this.BtnTransmitlistSetRadio = new Button();
      this.BtnTransmitlistSetMBus = new Button();
      this.label3 = new Label();
      this.txtBxRadioCycletime = new TextBox();
      this.label7 = new Label();
      this.label6 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.comboBoxRadioListNumbers = new ComboBox();
      this.comboBoxMBusListNumber = new ComboBox();
      this.comboBoxRadioMode = new ComboBox();
      this.cmb_encMode = new ComboBox();
      this.buttonSetRadioListAndParameters = new Button();
      this.BtnTransmitlistsRead = new Button();
      this.buttonResetVolumeMeterPipe = new Button();
      this.buttonShowVolumePipeSetup = new Button();
      this.buttonReadDeviceDescriptor = new Button();
      this.buttonRunVMCP_Simulator = new Button();
      this.groupBoxRadioTests.SuspendLayout();
      this.groupBoxWriteProtection.SuspendLayout();
      this.groupBoxAccumulatedValues.SuspendLayout();
      this.grBxRtcCalibration.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.groupBox5.SuspendLayout();
      this.groupBoxTransmitListsByCommand.SuspendLayout();
      this.groupBox7.SuspendLayout();
      this.SuspendLayout();
      this.zennerCoroprateDesign2.Dock = DockStyle.Top;
      this.zennerCoroprateDesign2.Location = new Point(0, 0);
      this.zennerCoroprateDesign2.Margin = new Padding(2);
      this.zennerCoroprateDesign2.Name = "zennerCoroprateDesign2";
      this.zennerCoroprateDesign2.Size = new Size(973, 45);
      this.zennerCoroprateDesign2.TabIndex = 18;
      this.groupBoxRadioTests.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.groupBoxRadioTests.Controls.Add((Control) this.buttonRadioTestStop);
      this.groupBoxRadioTests.Controls.Add((Control) this.buttonRadio3_Packet);
      this.groupBoxRadioTests.Controls.Add((Control) this.buttonWMBus_T_PN9);
      this.groupBoxRadioTests.Controls.Add((Control) this.buttonWMBus_S_PN9);
      this.groupBoxRadioTests.Controls.Add((Control) this.buttonRadio3_PN9);
      this.groupBoxRadioTests.Controls.Add((Control) this.buttonWMBus_T_Center);
      this.groupBoxRadioTests.Controls.Add((Control) this.buttonRadio2_PN9);
      this.groupBoxRadioTests.Controls.Add((Control) this.buttonWMBus_S_Center);
      this.groupBoxRadioTests.Controls.Add((Control) this.buttonRadio3Center);
      this.groupBoxRadioTests.Controls.Add((Control) this.buttonRadio2Center);
      this.groupBoxRadioTests.Location = new Point(212, 266);
      this.groupBoxRadioTests.Name = "groupBoxRadioTests";
      this.groupBoxRadioTests.Size = new Size(140, 327);
      this.groupBoxRadioTests.TabIndex = 19;
      this.groupBoxRadioTests.TabStop = false;
      this.groupBoxRadioTests.Text = "Radio tests";
      this.buttonRadioTestStop.Location = new Point(6, 289);
      this.buttonRadioTestStop.Name = "buttonRadioTestStop";
      this.buttonRadioTestStop.Size = new Size(125, 23);
      this.buttonRadioTestStop.TabIndex = 0;
      this.buttonRadioTestStop.Text = "Radio test stop";
      this.buttonRadioTestStop.UseVisualStyleBackColor = true;
      this.buttonRadioTestStop.Click += new System.EventHandler(this.buttonRadioTestStop_Click);
      this.buttonRadio3_Packet.Location = new Point(6, 251);
      this.buttonRadio3_Packet.Name = "buttonRadio3_Packet";
      this.buttonRadio3_Packet.Size = new Size(125, 23);
      this.buttonRadio3_Packet.TabIndex = 0;
      this.buttonRadio3_Packet.Text = "Radio3 packet";
      this.buttonRadio3_Packet.UseVisualStyleBackColor = true;
      this.buttonRadio3_Packet.Click += new System.EventHandler(this.buttonRadio3_Packet_Click);
      this.buttonWMBus_T_PN9.Location = new Point(6, 222);
      this.buttonWMBus_T_PN9.Name = "buttonWMBus_T_PN9";
      this.buttonWMBus_T_PN9.Size = new Size(125, 23);
      this.buttonWMBus_T_PN9.TabIndex = 0;
      this.buttonWMBus_T_PN9.Text = "WMBus T PN9";
      this.buttonWMBus_T_PN9.UseVisualStyleBackColor = true;
      this.buttonWMBus_T_PN9.Click += new System.EventHandler(this.buttonWMBus_T_PN9_Click);
      this.buttonWMBus_S_PN9.Location = new Point(6, 165);
      this.buttonWMBus_S_PN9.Name = "buttonWMBus_S_PN9";
      this.buttonWMBus_S_PN9.Size = new Size(125, 23);
      this.buttonWMBus_S_PN9.TabIndex = 0;
      this.buttonWMBus_S_PN9.Text = "WMBus S PN9";
      this.buttonWMBus_S_PN9.UseVisualStyleBackColor = true;
      this.buttonWMBus_S_PN9.Click += new System.EventHandler(this.buttonWMBus_S_PN9_Click);
      this.buttonRadio3_PN9.Location = new Point(6, 107);
      this.buttonRadio3_PN9.Name = "buttonRadio3_PN9";
      this.buttonRadio3_PN9.Size = new Size(125, 23);
      this.buttonRadio3_PN9.TabIndex = 0;
      this.buttonRadio3_PN9.Text = "Radio3 PN9";
      this.buttonRadio3_PN9.UseVisualStyleBackColor = true;
      this.buttonRadio3_PN9.Click += new System.EventHandler(this.buttonRadio3_PN9_Click);
      this.buttonWMBus_T_Center.Location = new Point(7, 193);
      this.buttonWMBus_T_Center.Name = "buttonWMBus_T_Center";
      this.buttonWMBus_T_Center.Size = new Size(124, 23);
      this.buttonWMBus_T_Center.TabIndex = 0;
      this.buttonWMBus_T_Center.Text = "WMBus T center";
      this.buttonWMBus_T_Center.UseVisualStyleBackColor = true;
      this.buttonWMBus_T_Center.Click += new System.EventHandler(this.buttonWMBus_T_Center_Click);
      this.buttonRadio2_PN9.Location = new Point(6, 49);
      this.buttonRadio2_PN9.Name = "buttonRadio2_PN9";
      this.buttonRadio2_PN9.Size = new Size(125, 23);
      this.buttonRadio2_PN9.TabIndex = 0;
      this.buttonRadio2_PN9.Text = "Radio2 PN9";
      this.buttonRadio2_PN9.UseVisualStyleBackColor = true;
      this.buttonRadio2_PN9.Click += new System.EventHandler(this.buttonRadio2_PN9_Click);
      this.buttonWMBus_S_Center.Location = new Point(7, 136);
      this.buttonWMBus_S_Center.Name = "buttonWMBus_S_Center";
      this.buttonWMBus_S_Center.Size = new Size(124, 23);
      this.buttonWMBus_S_Center.TabIndex = 0;
      this.buttonWMBus_S_Center.Text = "WMBus S center";
      this.buttonWMBus_S_Center.UseVisualStyleBackColor = true;
      this.buttonWMBus_S_Center.Click += new System.EventHandler(this.buttonWMBus_S_Center_Click);
      this.buttonRadio3Center.Location = new Point(7, 78);
      this.buttonRadio3Center.Name = "buttonRadio3Center";
      this.buttonRadio3Center.Size = new Size(124, 23);
      this.buttonRadio3Center.TabIndex = 0;
      this.buttonRadio3Center.Text = "Radio3 center";
      this.buttonRadio3Center.UseVisualStyleBackColor = true;
      this.buttonRadio3Center.Click += new System.EventHandler(this.buttonRadio3Center_Click);
      this.buttonRadio2Center.Location = new Point(7, 20);
      this.buttonRadio2Center.Name = "buttonRadio2Center";
      this.buttonRadio2Center.Size = new Size(124, 23);
      this.buttonRadio2Center.TabIndex = 0;
      this.buttonRadio2Center.Text = "Radio2 center";
      this.buttonRadio2Center.UseVisualStyleBackColor = true;
      this.buttonRadio2Center.Click += new System.EventHandler(this.buttonRadio2Center_Click);
      this.textBoxMessages.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxMessages.Location = new Point(12, 51);
      this.textBoxMessages.Multiline = true;
      this.textBoxMessages.Name = "textBoxMessages";
      this.textBoxMessages.ReadOnly = true;
      this.textBoxMessages.Size = new Size(949, 209);
      this.textBoxMessages.TabIndex = 20;
      this.buttonReadDeviceState.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.buttonReadDeviceState.Location = new Point(358, 382);
      this.buttonReadDeviceState.Name = "buttonReadDeviceState";
      this.buttonReadDeviceState.Size = new Size(141, 23);
      this.buttonReadDeviceState.TabIndex = 21;
      this.buttonReadDeviceState.Text = "Read device state";
      this.buttonReadDeviceState.UseVisualStyleBackColor = true;
      this.buttonReadDeviceState.Click += new System.EventHandler(this.buttonReadDeviceState_Click);
      this.groupBoxWriteProtection.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.groupBoxWriteProtection.Controls.Add((Control) this.buttonWrProtSetKey);
      this.groupBoxWriteProtection.Controls.Add((Control) this.buttonWrProtReset);
      this.groupBoxWriteProtection.Controls.Add((Control) this.buttonWrProtSet);
      this.groupBoxWriteProtection.Controls.Add((Control) this.buttonWrProtGet);
      this.groupBoxWriteProtection.Controls.Add((Control) this.label1);
      this.groupBoxWriteProtection.Controls.Add((Control) this.textBoxMeterKey);
      this.groupBoxWriteProtection.Location = new Point(12, 419);
      this.groupBoxWriteProtection.Name = "groupBoxWriteProtection";
      this.groupBoxWriteProtection.Size = new Size(182, 139);
      this.groupBoxWriteProtection.TabIndex = 22;
      this.groupBoxWriteProtection.TabStop = false;
      this.groupBoxWriteProtection.Text = "Write protection";
      this.buttonWrProtSetKey.Location = new Point(6, 103);
      this.buttonWrProtSetKey.Name = "buttonWrProtSetKey";
      this.buttonWrProtSetKey.Size = new Size(170, 23);
      this.buttonWrProtSetKey.TabIndex = 2;
      this.buttonWrProtSetKey.Text = "Set meter key";
      this.buttonWrProtSetKey.UseVisualStyleBackColor = true;
      this.buttonWrProtSetKey.Click += new System.EventHandler(this.buttonWrProtSetKey_Click);
      this.buttonWrProtReset.Location = new Point(90, 45);
      this.buttonWrProtReset.Name = "buttonWrProtReset";
      this.buttonWrProtReset.Size = new Size(86, 23);
      this.buttonWrProtReset.TabIndex = 2;
      this.buttonWrProtReset.Text = "Reset";
      this.buttonWrProtReset.UseVisualStyleBackColor = true;
      this.buttonWrProtReset.Click += new System.EventHandler(this.buttonWrProtReset_Click);
      this.buttonWrProtSet.Location = new Point(6, 45);
      this.buttonWrProtSet.Name = "buttonWrProtSet";
      this.buttonWrProtSet.Size = new Size(75, 23);
      this.buttonWrProtSet.TabIndex = 2;
      this.buttonWrProtSet.Text = "Set";
      this.buttonWrProtSet.UseVisualStyleBackColor = true;
      this.buttonWrProtSet.Click += new System.EventHandler(this.buttonWrProtSet_Click);
      this.buttonWrProtGet.Location = new Point(6, 74);
      this.buttonWrProtGet.Name = "buttonWrProtGet";
      this.buttonWrProtGet.Size = new Size(170, 23);
      this.buttonWrProtGet.TabIndex = 2;
      this.buttonWrProtGet.Text = "Get protection state";
      this.buttonWrProtGet.UseVisualStyleBackColor = true;
      this.buttonWrProtGet.Click += new System.EventHandler(this.buttonWrProtGet_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(15, 22);
      this.label1.Name = "label1";
      this.label1.Size = new Size(28, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Key:";
      this.textBoxMeterKey.Location = new Point(49, 19);
      this.textBoxMeterKey.Name = "textBoxMeterKey";
      this.textBoxMeterKey.Size = new Size((int) sbyte.MaxValue, 20);
      this.textBoxMeterKey.TabIndex = 0;
      this.buttonSetTestValues.Location = new Point(6, 49);
      this.buttonSetTestValues.Name = "buttonSetTestValues";
      this.buttonSetTestValues.Size = new Size(170, 23);
      this.buttonSetTestValues.TabIndex = 21;
      this.buttonSetTestValues.Text = "Set test values";
      this.buttonSetTestValues.UseVisualStyleBackColor = true;
      this.buttonSetTestValues.Click += new System.EventHandler(this.buttonSetTestValues_Click);
      this.groupBoxAccumulatedValues.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.groupBoxAccumulatedValues.Controls.Add((Control) this.buttonAddBigTestValues);
      this.groupBoxAccumulatedValues.Controls.Add((Control) this.buttonAddSmallTestValues);
      this.groupBoxAccumulatedValues.Controls.Add((Control) this.buttonShowAccumulatedValues);
      this.groupBoxAccumulatedValues.Controls.Add((Control) this.buttonSetTestValues);
      this.groupBoxAccumulatedValues.Location = new Point(12, 266);
      this.groupBoxAccumulatedValues.Name = "groupBoxAccumulatedValues";
      this.groupBoxAccumulatedValues.Size = new Size(182, 147);
      this.groupBoxAccumulatedValues.TabIndex = 23;
      this.groupBoxAccumulatedValues.TabStop = false;
      this.groupBoxAccumulatedValues.Text = "Accumulated values";
      this.buttonAddBigTestValues.Location = new Point(6, 107);
      this.buttonAddBigTestValues.Name = "buttonAddBigTestValues";
      this.buttonAddBigTestValues.Size = new Size(170, 23);
      this.buttonAddBigTestValues.TabIndex = 21;
      this.buttonAddBigTestValues.Text = "Add big test values";
      this.buttonAddBigTestValues.UseVisualStyleBackColor = true;
      this.buttonAddBigTestValues.Click += new System.EventHandler(this.buttonAddBigTestValues_Click);
      this.buttonAddSmallTestValues.Location = new Point(6, 78);
      this.buttonAddSmallTestValues.Name = "buttonAddSmallTestValues";
      this.buttonAddSmallTestValues.Size = new Size(170, 23);
      this.buttonAddSmallTestValues.TabIndex = 21;
      this.buttonAddSmallTestValues.Text = "Add small test values";
      this.buttonAddSmallTestValues.UseVisualStyleBackColor = true;
      this.buttonAddSmallTestValues.Click += new System.EventHandler(this.buttonAddSmallTestValues_Click);
      this.buttonShowAccumulatedValues.Location = new Point(6, 20);
      this.buttonShowAccumulatedValues.Name = "buttonShowAccumulatedValues";
      this.buttonShowAccumulatedValues.Size = new Size(170, 23);
      this.buttonShowAccumulatedValues.TabIndex = 21;
      this.buttonShowAccumulatedValues.Text = "Show values";
      this.buttonShowAccumulatedValues.UseVisualStyleBackColor = true;
      this.buttonShowAccumulatedValues.Click += new System.EventHandler(this.buttonShowAccumulatedValues_Click);
      this.btnStartRTCcal.Location = new Point(6, 20);
      this.btnStartRTCcal.Name = "btnStartRTCcal";
      this.btnStartRTCcal.Size = new Size(124, 23);
      this.btnStartRTCcal.TabIndex = 1;
      this.btnStartRTCcal.Text = "Start 512Hz";
      this.btnStartRTCcal.UseVisualStyleBackColor = true;
      this.btnStartRTCcal.Click += new System.EventHandler(this.btnStartRTCcal_Click);
      this.grBxRtcCalibration.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.grBxRtcCalibration.Controls.Add((Control) this.btnStopRTCcal);
      this.grBxRtcCalibration.Controls.Add((Control) this.btnStartRTCcal);
      this.grBxRtcCalibration.Location = new Point(358, 263);
      this.grBxRtcCalibration.Name = "grBxRtcCalibration";
      this.grBxRtcCalibration.Size = new Size(141, 83);
      this.grBxRtcCalibration.TabIndex = 24;
      this.grBxRtcCalibration.TabStop = false;
      this.grBxRtcCalibration.Text = "Quarz calibration";
      this.btnStopRTCcal.Location = new Point(5, 49);
      this.btnStopRTCcal.Name = "btnStopRTCcal";
      this.btnStopRTCcal.Size = new Size(125, 23);
      this.btnStopRTCcal.TabIndex = 1;
      this.btnStopRTCcal.Text = "Stop";
      this.btnStopRTCcal.UseVisualStyleBackColor = true;
      this.btnStopRTCcal.Click += new System.EventHandler(this.btnStopRTCcal_Click);
      this.buttonSetTestModeCapacityOff.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.buttonSetTestModeCapacityOff.Location = new Point(358, 353);
      this.buttonSetTestModeCapacityOff.Name = "buttonSetTestModeCapacityOff";
      this.buttonSetTestModeCapacityOff.Size = new Size(141, 23);
      this.buttonSetTestModeCapacityOff.TabIndex = 21;
      this.buttonSetTestModeCapacityOff.Text = "Test capacity off";
      this.buttonSetTestModeCapacityOff.UseVisualStyleBackColor = true;
      this.buttonSetTestModeCapacityOff.Click += new System.EventHandler(this.buttonSetTestModeCapacityOff_Click);
      this.btnNFCTest.Location = new Point(0, 0);
      this.btnNFCTest.Name = "btnNFCTest";
      this.btnNFCTest.Size = new Size(75, 23);
      this.btnNFCTest.TabIndex = 28;
      this.buttonNewIdTest.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonNewIdTest.Location = new Point(505, 441);
      this.buttonNewIdTest.Name = "buttonNewIdTest";
      this.buttonNewIdTest.Size = new Size(157, 23);
      this.buttonNewIdTest.TabIndex = 26;
      this.buttonNewIdTest.Text = "NewIdTest";
      this.buttonNewIdTest.UseVisualStyleBackColor = true;
      this.buttonNewIdTest.Click += new System.EventHandler(this.buttonNewIdTest_Click);
      this.buttonCrateTestClass.Location = new Point(6, 19);
      this.buttonCrateTestClass.Name = "buttonCrateTestClass";
      this.buttonCrateTestClass.Size = new Size(185, 23);
      this.buttonCrateTestClass.TabIndex = 21;
      this.buttonCrateTestClass.Text = "Create map class";
      this.buttonCrateTestClass.UseVisualStyleBackColor = true;
      this.buttonCrateTestClass.Click += new System.EventHandler(this.buttonCrateTestClass_Click);
      this.groupBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.groupBox1.Controls.Add((Control) this.buttonHardwareTypeManager);
      this.groupBox1.Controls.Add((Control) this.buttonAdjustFirmwarePointer);
      this.groupBox1.Controls.Add((Control) this.buttonSaveMapCompareFile);
      this.groupBox1.Controls.Add((Control) this.buttonCrateTestClass);
      this.groupBox1.Location = new Point(761, 352);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(200, 135);
      this.groupBox1.TabIndex = 27;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Map management";
      this.buttonHardwareTypeManager.Location = new Point(6, 106);
      this.buttonHardwareTypeManager.Name = "buttonHardwareTypeManager";
      this.buttonHardwareTypeManager.Size = new Size(185, 23);
      this.buttonHardwareTypeManager.TabIndex = 21;
      this.buttonHardwareTypeManager.Text = "Hardware Type Editor";
      this.buttonHardwareTypeManager.UseVisualStyleBackColor = true;
      this.buttonHardwareTypeManager.Click += new System.EventHandler(this.buttonHardwareTypeEditor_Click);
      this.buttonAdjustFirmwarePointer.Location = new Point(6, 77);
      this.buttonAdjustFirmwarePointer.Name = "buttonAdjustFirmwarePointer";
      this.buttonAdjustFirmwarePointer.Size = new Size(185, 23);
      this.buttonAdjustFirmwarePointer.TabIndex = 21;
      this.buttonAdjustFirmwarePointer.Text = "Adjust firmware pointer";
      this.buttonAdjustFirmwarePointer.UseVisualStyleBackColor = true;
      this.buttonAdjustFirmwarePointer.Click += new System.EventHandler(this.buttonAdjustFirmwarePointer_Click);
      this.buttonSaveMapCompareFile.Location = new Point(6, 48);
      this.buttonSaveMapCompareFile.Name = "buttonSaveMapCompareFile";
      this.buttonSaveMapCompareFile.Size = new Size(185, 23);
      this.buttonSaveMapCompareFile.TabIndex = 21;
      this.buttonSaveMapCompareFile.Text = "Save map compare file";
      this.buttonSaveMapCompareFile.UseVisualStyleBackColor = true;
      this.buttonSaveMapCompareFile.Click += new System.EventHandler(this.buttonSaveMapCompareFile_Click);
      this.buttonShowVolSimulator.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonShowVolSimulator.Location = new Point(761, 668);
      this.buttonShowVolSimulator.Name = "buttonShowVolSimulator";
      this.buttonShowVolSimulator.Size = new Size(200, 23);
      this.buttonShowVolSimulator.TabIndex = 21;
      this.buttonShowVolSimulator.Text = "Show volume simulator";
      this.buttonShowVolSimulator.UseVisualStyleBackColor = true;
      this.buttonShowVolSimulator.Click += new System.EventHandler(this.buttonShowVolSimulator_Click);
      this.groupBox2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.groupBox2.Controls.Add((Control) this.buttonStartStopVolumeInputStateLoop);
      this.groupBox2.Location = new Point(358, 411);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(141, 57);
      this.groupBox2.TabIndex = 24;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Volume input state";
      this.buttonStartStopVolumeInputStateLoop.Location = new Point(6, 20);
      this.buttonStartStopVolumeInputStateLoop.Name = "buttonStartStopVolumeInputStateLoop";
      this.buttonStartStopVolumeInputStateLoop.Size = new Size(124, 23);
      this.buttonStartStopVolumeInputStateLoop.TabIndex = 1;
      this.buttonStartStopVolumeInputStateLoop.Text = "Start read loop";
      this.buttonStartStopVolumeInputStateLoop.UseVisualStyleBackColor = true;
      this.buttonStartStopVolumeInputStateLoop.Click += new System.EventHandler(this.buttonStartStopVolumeInputStateLoop_Click);
      this.buttonHardCodedSpecialTest.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonHardCodedSpecialTest.Location = new Point(505, 471);
      this.buttonHardCodedSpecialTest.Name = "buttonHardCodedSpecialTest";
      this.buttonHardCodedSpecialTest.Size = new Size(157, 23);
      this.buttonHardCodedSpecialTest.TabIndex = 26;
      this.buttonHardCodedSpecialTest.Text = "Hard coded special test";
      this.buttonHardCodedSpecialTest.UseVisualStyleBackColor = true;
      this.buttonHardCodedSpecialTest.Click += new System.EventHandler(this.buttonHardCodedSpecialTest_Click);
      this.buttonShowVmcpTool.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonShowVmcpTool.Location = new Point(761, 696);
      this.buttonShowVmcpTool.Name = "buttonShowVmcpTool";
      this.buttonShowVmcpTool.Size = new Size(200, 23);
      this.buttonShowVmcpTool.TabIndex = 21;
      this.buttonShowVmcpTool.Text = "Show VMCP tool";
      this.buttonShowVmcpTool.UseVisualStyleBackColor = true;
      this.buttonShowVmcpTool.Click += new System.EventHandler(this.buttonShowVmcpTool_Click);
      this.groupBox3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.groupBox3.Controls.Add((Control) this.buttonOverwriteTempFromBackup);
      this.groupBox3.Location = new Point(358, 476);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new Size(141, 57);
      this.groupBox3.TabIndex = 24;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Special overwrite";
      this.buttonOverwriteTempFromBackup.Location = new Point(6, 20);
      this.buttonOverwriteTempFromBackup.Name = "buttonOverwriteTempFromBackup";
      this.buttonOverwriteTempFromBackup.Size = new Size(124, 23);
      this.buttonOverwriteTempFromBackup.TabIndex = 1;
      this.buttonOverwriteTempFromBackup.Text = "Temp. from Backup";
      this.buttonOverwriteTempFromBackup.UseVisualStyleBackColor = true;
      this.buttonOverwriteTempFromBackup.Click += new System.EventHandler(this.buttonOverwriteTempFromBackup_Click);
      this.groupBox4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.groupBox4.Controls.Add((Control) this.ButtonReceiveTestPacketViaMiCon);
      this.groupBox4.Controls.Add((Control) this.ButtonSendTestPacketViaMiCon);
      this.groupBox4.Controls.Add((Control) this.ButtonReceiveTestPacket);
      this.groupBox4.Controls.Add((Control) this.ButtonSendTestPacket);
      this.groupBox4.Location = new Point(505, 263);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new Size(157, 173);
      this.groupBox4.TabIndex = 29;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Radio tests 2";
      this.ButtonReceiveTestPacketViaMiCon.Location = new Point(6, 130);
      this.ButtonReceiveTestPacketViaMiCon.Name = "ButtonReceiveTestPacketViaMiCon";
      this.ButtonReceiveTestPacketViaMiCon.Size = new Size(147, 34);
      this.ButtonReceiveTestPacketViaMiCon.TabIndex = 3;
      this.ButtonReceiveTestPacketViaMiCon.Text = "Receive Test Packet Via MiCon";
      this.ButtonReceiveTestPacketViaMiCon.UseVisualStyleBackColor = true;
      this.ButtonReceiveTestPacketViaMiCon.Click += new System.EventHandler(this.ButtonReceiveTestPacketViaMiCon_Click);
      this.ButtonSendTestPacketViaMiCon.Location = new Point(5, 89);
      this.ButtonSendTestPacketViaMiCon.Name = "ButtonSendTestPacketViaMiCon";
      this.ButtonSendTestPacketViaMiCon.Size = new Size(147, 34);
      this.ButtonSendTestPacketViaMiCon.TabIndex = 2;
      this.ButtonSendTestPacketViaMiCon.Text = "Send Test Packet Via MiCon";
      this.ButtonSendTestPacketViaMiCon.UseVisualStyleBackColor = true;
      this.ButtonSendTestPacketViaMiCon.Click += new System.EventHandler(this.ButtonSendTestPacketViaMiCon_Click);
      this.ButtonReceiveTestPacket.Location = new Point(5, 49);
      this.ButtonReceiveTestPacket.Name = "ButtonReceiveTestPacket";
      this.ButtonReceiveTestPacket.Size = new Size(147, 34);
      this.ButtonReceiveTestPacket.TabIndex = 1;
      this.ButtonReceiveTestPacket.Text = "Receive Test Packet";
      this.ButtonReceiveTestPacket.UseVisualStyleBackColor = true;
      this.ButtonReceiveTestPacket.Click += new System.EventHandler(this.ButtonReceiveTestPacket_Click);
      this.ButtonSendTestPacket.Location = new Point(6, 20);
      this.ButtonSendTestPacket.Name = "ButtonSendTestPacket";
      this.ButtonSendTestPacket.Size = new Size(146, 23);
      this.ButtonSendTestPacket.TabIndex = 1;
      this.ButtonSendTestPacket.Text = "Send Test Packet";
      this.ButtonSendTestPacket.UseVisualStyleBackColor = true;
      this.ButtonSendTestPacket.Click += new System.EventHandler(this.ButtonSendTestPacket_Click);
      this.groupBox5.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.groupBox5.Controls.Add((Control) this.textBoxExistingMeterID);
      this.groupBox5.Controls.Add((Control) this.label2);
      this.groupBox5.Controls.Add((Control) this.buttonSetID_FromDatabase);
      this.groupBox5.Location = new Point(761, 266);
      this.groupBox5.Name = "groupBox5";
      this.groupBox5.Size = new Size(200, 80);
      this.groupBox5.TabIndex = 27;
      this.groupBox5.TabStop = false;
      this.groupBox5.Text = "ID management";
      this.textBoxExistingMeterID.Location = new Point(100, 17);
      this.textBoxExistingMeterID.Name = "textBoxExistingMeterID";
      this.textBoxExistingMeterID.Size = new Size(91, 20);
      this.textBoxExistingMeterID.TabIndex = 2;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 20);
      this.label2.Name = "label2";
      this.label2.Size = new Size(87, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Existing MeterID:";
      this.buttonSetID_FromDatabase.Location = new Point(6, 46);
      this.buttonSetID_FromDatabase.Name = "buttonSetID_FromDatabase";
      this.buttonSetID_FromDatabase.Size = new Size(185, 23);
      this.buttonSetID_FromDatabase.TabIndex = 0;
      this.buttonSetID_FromDatabase.Text = "Set ID's from database";
      this.buttonSetID_FromDatabase.UseVisualStyleBackColor = true;
      this.buttonSetID_FromDatabase.Click += new System.EventHandler(this.buttonSetID_FromDatabase_Click);
      this.buttonShow_IO_Test.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonShow_IO_Test.Location = new Point(761, 639);
      this.buttonShow_IO_Test.Name = "buttonShow_IO_Test";
      this.buttonShow_IO_Test.Size = new Size(200, 23);
      this.buttonShow_IO_Test.TabIndex = 21;
      this.buttonShow_IO_Test.Text = "Show IO test";
      this.buttonShow_IO_Test.UseVisualStyleBackColor = true;
      this.buttonShow_IO_Test.Click += new System.EventHandler(this.buttonShow_IO_Test_Click);
      this.groupBoxTransmitListsByCommand.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.groupBoxTransmitListsByCommand.Controls.Add((Control) this.groupBox7);
      this.groupBoxTransmitListsByCommand.Controls.Add((Control) this.label3);
      this.groupBoxTransmitListsByCommand.Controls.Add((Control) this.txtBxRadioCycletime);
      this.groupBoxTransmitListsByCommand.Controls.Add((Control) this.label7);
      this.groupBoxTransmitListsByCommand.Controls.Add((Control) this.label6);
      this.groupBoxTransmitListsByCommand.Controls.Add((Control) this.label4);
      this.groupBoxTransmitListsByCommand.Controls.Add((Control) this.label5);
      this.groupBoxTransmitListsByCommand.Controls.Add((Control) this.comboBoxRadioListNumbers);
      this.groupBoxTransmitListsByCommand.Controls.Add((Control) this.comboBoxMBusListNumber);
      this.groupBoxTransmitListsByCommand.Controls.Add((Control) this.comboBoxRadioMode);
      this.groupBoxTransmitListsByCommand.Controls.Add((Control) this.cmb_encMode);
      this.groupBoxTransmitListsByCommand.Controls.Add((Control) this.buttonSetRadioListAndParameters);
      this.groupBoxTransmitListsByCommand.Controls.Add((Control) this.BtnTransmitlistsRead);
      this.groupBoxTransmitListsByCommand.Location = new Point(12, 598);
      this.groupBoxTransmitListsByCommand.Name = "groupBoxTransmitListsByCommand";
      this.groupBoxTransmitListsByCommand.Size = new Size(465, 145);
      this.groupBoxTransmitListsByCommand.TabIndex = 30;
      this.groupBoxTransmitListsByCommand.TabStop = false;
      this.groupBoxTransmitListsByCommand.Text = "Transmit lists by command";
      this.groupBox7.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.groupBox7.Controls.Add((Control) this.BtnTransmitlistSetRadio);
      this.groupBox7.Controls.Add((Control) this.BtnTransmitlistSetMBus);
      this.groupBox7.Location = new Point(265, 16);
      this.groupBox7.Margin = new Padding(2);
      this.groupBox7.Name = "groupBox7";
      this.groupBox7.Padding = new Padding(2);
      this.groupBox7.Size = new Size(187, 68);
      this.groupBox7.TabIndex = 37;
      this.groupBox7.TabStop = false;
      this.groupBox7.Text = "MBus list select commands";
      this.BtnTransmitlistSetRadio.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.BtnTransmitlistSetRadio.Enabled = false;
      this.BtnTransmitlistSetRadio.Location = new Point(9, 42);
      this.BtnTransmitlistSetRadio.Name = "BtnTransmitlistSetRadio";
      this.BtnTransmitlistSetRadio.Size = new Size(170, 23);
      this.BtnTransmitlistSetRadio.TabIndex = 35;
      this.BtnTransmitlistSetRadio.Text = "Set Radio list";
      this.BtnTransmitlistSetRadio.UseVisualStyleBackColor = true;
      this.BtnTransmitlistSetRadio.Click += new System.EventHandler(this.BtnTransmitlistSetRadio_Click);
      this.BtnTransmitlistSetMBus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.BtnTransmitlistSetMBus.Enabled = false;
      this.BtnTransmitlistSetMBus.Location = new Point(9, 19);
      this.BtnTransmitlistSetMBus.Name = "BtnTransmitlistSetMBus";
      this.BtnTransmitlistSetMBus.Size = new Size(170, 23);
      this.BtnTransmitlistSetMBus.TabIndex = 34;
      this.BtnTransmitlistSetMBus.Text = "Set MBus list";
      this.BtnTransmitlistSetMBus.UseVisualStyleBackColor = true;
      this.BtnTransmitlistSetMBus.Click += new System.EventHandler(this.BtnTransmitlistSetMBus_Click);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(15, 116);
      this.label3.Name = "label3";
      this.label3.Size = new Size(94, 13);
      this.label3.TabIndex = 31;
      this.label3.Text = "CycletimeSeconds";
      this.txtBxRadioCycletime.Location = new Point(137, 116);
      this.txtBxRadioCycletime.Name = "txtBxRadioCycletime";
      this.txtBxRadioCycletime.Size = new Size(107, 20);
      this.txtBxRadioCycletime.TabIndex = 31;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(13, 93);
      this.label7.Name = "label7";
      this.label7.Size = new Size(88, 13);
      this.label7.TabIndex = 36;
      this.label7.Text = "Radio list number";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(13, 70);
      this.label6.Name = "label6";
      this.label6.Size = new Size(87, 13);
      this.label6.TabIndex = 36;
      this.label6.Text = "MBus list number";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(13, 47);
      this.label4.Name = "label4";
      this.label4.Size = new Size(65, 13);
      this.label4.TabIndex = 36;
      this.label4.Text = "Radio Mode";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(13, 25);
      this.label5.Name = "label5";
      this.label5.Size = new Size(111, 13);
      this.label5.TabIndex = 36;
      this.label5.Text = "AES_EncryptionMode";
      this.label5.TextAlign = ContentAlignment.BottomRight;
      this.comboBoxRadioListNumbers.FormattingEnabled = true;
      this.comboBoxRadioListNumbers.Location = new Point(137, 92);
      this.comboBoxRadioListNumbers.Name = "comboBoxRadioListNumbers";
      this.comboBoxRadioListNumbers.Size = new Size(107, 21);
      this.comboBoxRadioListNumbers.TabIndex = 31;
      this.comboBoxMBusListNumber.FormattingEnabled = true;
      this.comboBoxMBusListNumber.Location = new Point(137, 69);
      this.comboBoxMBusListNumber.Name = "comboBoxMBusListNumber";
      this.comboBoxMBusListNumber.Size = new Size(107, 21);
      this.comboBoxMBusListNumber.TabIndex = 31;
      this.comboBoxRadioMode.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxRadioMode.FormattingEnabled = true;
      this.comboBoxRadioMode.Location = new Point(137, 46);
      this.comboBoxRadioMode.Name = "comboBoxRadioMode";
      this.comboBoxRadioMode.Size = new Size(107, 21);
      this.comboBoxRadioMode.TabIndex = 31;
      this.cmb_encMode.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmb_encMode.FormattingEnabled = true;
      this.cmb_encMode.Location = new Point(137, 22);
      this.cmb_encMode.Name = "cmb_encMode";
      this.cmb_encMode.Size = new Size(107, 21);
      this.cmb_encMode.TabIndex = 31;
      this.buttonSetRadioListAndParameters.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.buttonSetRadioListAndParameters.Enabled = false;
      this.buttonSetRadioListAndParameters.Location = new Point(274, 87);
      this.buttonSetRadioListAndParameters.Name = "buttonSetRadioListAndParameters";
      this.buttonSetRadioListAndParameters.Size = new Size(170, 23);
      this.buttonSetRadioListAndParameters.TabIndex = 31;
      this.buttonSetRadioListAndParameters.Text = "Set radio list and parameters";
      this.buttonSetRadioListAndParameters.UseVisualStyleBackColor = true;
      this.buttonSetRadioListAndParameters.Click += new System.EventHandler(this.buttonSetRadioListAndParameters_Click);
      this.BtnTransmitlistsRead.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.BtnTransmitlistsRead.Location = new Point(265, 120);
      this.BtnTransmitlistsRead.Name = "BtnTransmitlistsRead";
      this.BtnTransmitlistsRead.Size = new Size(187, 23);
      this.BtnTransmitlistsRead.TabIndex = 31;
      this.BtnTransmitlistsRead.Text = "Read";
      this.BtnTransmitlistsRead.UseVisualStyleBackColor = true;
      this.BtnTransmitlistsRead.Click += new System.EventHandler(this.BtnTransmitlistsRead_Click);
      this.buttonResetVolumeMeterPipe.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonResetVolumeMeterPipe.Location = new Point(505, 500);
      this.buttonResetVolumeMeterPipe.Name = "buttonResetVolumeMeterPipe";
      this.buttonResetVolumeMeterPipe.Size = new Size(157, 23);
      this.buttonResetVolumeMeterPipe.TabIndex = 26;
      this.buttonResetVolumeMeterPipe.Text = "Reset volume meter pipe";
      this.buttonResetVolumeMeterPipe.UseVisualStyleBackColor = true;
      this.buttonResetVolumeMeterPipe.Click += new System.EventHandler(this.buttonResetVolumeMeterPipe_Click);
      this.buttonShowVolumePipeSetup.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonShowVolumePipeSetup.Location = new Point(505, 529);
      this.buttonShowVolumePipeSetup.Name = "buttonShowVolumePipeSetup";
      this.buttonShowVolumePipeSetup.Size = new Size(157, 23);
      this.buttonShowVolumePipeSetup.TabIndex = 26;
      this.buttonShowVolumePipeSetup.Text = "Show volume pipe setup";
      this.buttonShowVolumePipeSetup.UseVisualStyleBackColor = true;
      this.buttonShowVolumePipeSetup.Click += new System.EventHandler(this.buttonShowVolumePipeSetup_Click);
      this.buttonReadDeviceDescriptor.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonReadDeviceDescriptor.Location = new Point(505, 558);
      this.buttonReadDeviceDescriptor.Name = "buttonReadDeviceDescriptor";
      this.buttonReadDeviceDescriptor.Size = new Size(157, 23);
      this.buttonReadDeviceDescriptor.TabIndex = 26;
      this.buttonReadDeviceDescriptor.Text = "Read device descriptor";
      this.buttonReadDeviceDescriptor.UseVisualStyleBackColor = true;
      this.buttonReadDeviceDescriptor.Click += new System.EventHandler(this.buttonReadDeviceDescriptor_Click);
      this.buttonRunVMCP_Simulator.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonRunVMCP_Simulator.Location = new Point(761, 725);
      this.buttonRunVMCP_Simulator.Name = "buttonRunVMCP_Simulator";
      this.buttonRunVMCP_Simulator.Size = new Size(200, 23);
      this.buttonRunVMCP_Simulator.TabIndex = 21;
      this.buttonRunVMCP_Simulator.Text = "Run VMCP Simulator";
      this.buttonRunVMCP_Simulator.UseVisualStyleBackColor = true;
      this.buttonRunVMCP_Simulator.Click += new System.EventHandler(this.buttonRunVMCP_Simulator_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(973, 752);
      this.Controls.Add((Control) this.groupBoxTransmitListsByCommand);
      this.Controls.Add((Control) this.groupBox4);
      this.Controls.Add((Control) this.buttonSetTestModeCapacityOff);
      this.Controls.Add((Control) this.groupBox5);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.buttonReadDeviceDescriptor);
      this.Controls.Add((Control) this.buttonShowVolumePipeSetup);
      this.Controls.Add((Control) this.buttonResetVolumeMeterPipe);
      this.Controls.Add((Control) this.buttonHardCodedSpecialTest);
      this.Controls.Add((Control) this.buttonNewIdTest);
      this.Controls.Add((Control) this.btnNFCTest);
      this.Controls.Add((Control) this.groupBox3);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.grBxRtcCalibration);
      this.Controls.Add((Control) this.groupBoxAccumulatedValues);
      this.Controls.Add((Control) this.groupBoxWriteProtection);
      this.Controls.Add((Control) this.buttonRunVMCP_Simulator);
      this.Controls.Add((Control) this.buttonShowVmcpTool);
      this.Controls.Add((Control) this.buttonShow_IO_Test);
      this.Controls.Add((Control) this.buttonShowVolSimulator);
      this.Controls.Add((Control) this.buttonReadDeviceState);
      this.Controls.Add((Control) this.textBoxMessages);
      this.Controls.Add((Control) this.groupBoxRadioTests);
      this.Controls.Add((Control) this.zennerCoroprateDesign2);
      this.Name = nameof (MoreTests);
      this.Text = nameof (MoreTests);
      this.groupBoxRadioTests.ResumeLayout(false);
      this.groupBoxWriteProtection.ResumeLayout(false);
      this.groupBoxWriteProtection.PerformLayout();
      this.groupBoxAccumulatedValues.ResumeLayout(false);
      this.grBxRtcCalibration.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox2.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox4.ResumeLayout(false);
      this.groupBox5.ResumeLayout(false);
      this.groupBox5.PerformLayout();
      this.groupBoxTransmitListsByCommand.ResumeLayout(false);
      this.groupBoxTransmitListsByCommand.PerformLayout();
      this.groupBox7.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    internal class CommunicationClass
    {
      internal int waitMilliSeconds;
      internal int testNumber;

      internal CommunicationClass(int waitSeconds, int testNumber)
      {
        this.waitMilliSeconds = waitSeconds;
        this.testNumber = testNumber;
      }
    }
  }
}

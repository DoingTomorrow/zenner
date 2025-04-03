// Decompiled with JetBrains decompiler
// Type: DeviceCollector.MBusConverterDiagnostic
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using CorporateDesign;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class MBusConverterDiagnostic : Form
  {
    protected static Logger MBusConverterLogger = LogManager.GetLogger("MBusConverterCommands");
    private MBusDevice MBusConverterDevice;
    private MBusDevice SelectedDevice;
    private DeviceCollectorFunctions MyFunctions;
    private double[] DiagnositcValues = new double[512];
    private int[] DiagnositcValuesInt = new int[512];
    private int DiagnositcValuesOffset;
    private MBusConverterDiagnostic.ConverterCommands typeOfData;
    private int Grundpegel;
    private int Triggergrenze;
    private List<MBusConverterDiagnostic.mA_TableEntry> mA_Table;
    private double scaleFactor;
    private bool use_mA_Table;
    private IContainer components = (IContainer) null;
    private ZennerCoroprateDesign zennerCoroprateDesign1;
    private TextBox textBoxMBusConverterData;
    private Button buttonIdentifyReceiveCurrent;
    private Button buttonIdentifyReceiveCurrentFilterd;
    private Button buttonIdentifyTransmitCurrent;
    private Button buttonIdentifyTransmitVoltage;
    private Button buttonReadConverterData;
    private Button buttonIdentifyStandbyCurrent;
    private Button buttonIdentifyPowerOnCurrent;
    private Button buttonReadLastData;
    private Button buttonReadErrorLogger;
    private Button buttonIdentifyStandbyCurrentSpectrum;
    private Chart chart1;

    public MBusConverterDiagnostic(DeviceCollectorFunctions MyFunctions)
    {
      this.InitializeComponent();
      this.MyFunctions = MyFunctions;
      this.mA_Table = new List<MBusConverterDiagnostic.mA_TableEntry>();
      this.mA_Table.Add(new MBusConverterDiagnostic.mA_TableEntry(34, 0.0353));
      this.mA_Table.Add(new MBusConverterDiagnostic.mA_TableEntry(44, 0.0545));
      this.mA_Table.Add(new MBusConverterDiagnostic.mA_TableEntry(51, 0.07));
      this.mA_Table.Add(new MBusConverterDiagnostic.mA_TableEntry(61, 0.08));
      this.mA_Table.Add(new MBusConverterDiagnostic.mA_TableEntry(76, 0.088));
      this.mA_Table.Add(new MBusConverterDiagnostic.mA_TableEntry(95, 0.097));
      this.mA_Table.Add(new MBusConverterDiagnostic.mA_TableEntry(124, 0.106));
      this.mA_Table.Add(new MBusConverterDiagnostic.mA_TableEntry(543, 0.129));
      this.mA_Table.Add(new MBusConverterDiagnostic.mA_TableEntry(1256, 0.133));
      this.mA_Table.Add(new MBusConverterDiagnostic.mA_TableEntry(9999999, 0.133));
      this.textBoxMBusConverterData.Clear();
      for (int index = 0; index < this.MyFunctions.MyDeviceList.bus.Count; ++index)
      {
        if (this.MyFunctions.MyDeviceList.bus[index] is MBusDevice)
        {
          MBusDevice bu = (MBusDevice) this.MyFunctions.MyDeviceList.bus[index];
          if (bu.PrimaryAddressKnown && bu.PrimaryDeviceAddress == (byte) 251)
          {
            this.MBusConverterDevice = bu;
            break;
          }
        }
      }
      for (int xValue = 0; xValue < this.DiagnositcValues.Length; ++xValue)
        this.chart1.Series[0].Points.AddXY((double) xValue, this.DiagnositcValues[xValue]);
    }

    private void MBusConverterDiagnostic_Activated(object sender, EventArgs e)
    {
      this.textBoxMBusConverterData.Clear();
      if (this.MBusConverterDevice == null)
      {
        this.textBoxMBusConverterData.AppendText("No mbus converter available");
        this.buttonReadConverterData.Enabled = false;
        this.buttonIdentifyReceiveCurrent.Enabled = false;
        this.buttonIdentifyReceiveCurrentFilterd.Enabled = false;
        this.buttonIdentifyTransmitCurrent.Enabled = false;
        this.buttonIdentifyTransmitVoltage.Enabled = false;
        this.buttonIdentifyStandbyCurrent.Enabled = false;
        this.buttonIdentifyPowerOnCurrent.Enabled = false;
        this.buttonIdentifyStandbyCurrentSpectrum.Enabled = false;
        this.buttonReadErrorLogger.Enabled = false;
        this.buttonReadLastData.Enabled = false;
      }
      else
      {
        this.buttonReadConverterData.Enabled = true;
        this.buttonIdentifyTransmitCurrent.Enabled = true;
        this.buttonIdentifyTransmitVoltage.Enabled = true;
        this.buttonIdentifyStandbyCurrent.Enabled = true;
        this.buttonIdentifyPowerOnCurrent.Enabled = true;
        this.buttonIdentifyStandbyCurrentSpectrum.Enabled = true;
        this.buttonReadErrorLogger.Enabled = true;
        this.buttonReadLastData.Enabled = true;
        if (this.MyFunctions.MyDeviceList.SelectedDevice == null || !(this.MyFunctions.MyDeviceList.SelectedDevice is MBusDevice) || this.MyFunctions.MyDeviceList.SelectedDevice == this.MBusConverterDevice)
        {
          this.textBoxMBusConverterData.AppendText("Test device not selected");
          this.buttonIdentifyReceiveCurrent.Enabled = false;
          this.buttonIdentifyReceiveCurrentFilterd.Enabled = false;
        }
        else
        {
          this.SelectedDevice = (MBusDevice) this.MyFunctions.MyDeviceList.SelectedDevice;
          this.buttonIdentifyReceiveCurrent.Enabled = true;
          this.buttonIdentifyReceiveCurrentFilterd.Enabled = true;
        }
      }
    }

    private void buttonReadConverterData_Click(object sender, EventArgs e)
    {
      this.textBoxMBusConverterData.Clear();
      this.chart1.Series[0].Points.Clear();
      ZR_ClassLibMessages.ClearErrors();
      this.ReadConverterMBusData();
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonIdentifyReceiveCurrent_Click(object sender, EventArgs e)
    {
      this.TestFunction(MBusConverterDiagnostic.ConverterCommands.SetReceiveCurrent);
    }

    private void buttonIdentifyReceiveCurrentFilterd_Click(object sender, EventArgs e)
    {
      this.TestFunction(MBusConverterDiagnostic.ConverterCommands.SetReceiveCurrentFilterd);
    }

    private void buttonIdentifyTransmitCurrent_Click(object sender, EventArgs e)
    {
      this.TestFunction(MBusConverterDiagnostic.ConverterCommands.SetReceiveCurrentOnTransmit);
    }

    private void buttonIdentifyTransmitVoltage_Click(object sender, EventArgs e)
    {
      this.TestFunction(MBusConverterDiagnostic.ConverterCommands.SetTransmitVoltage);
    }

    private void buttonIdentifyStandbyCurrent_Click(object sender, EventArgs e)
    {
      this.textBoxMBusConverterData.Clear();
      ZR_ClassLibMessages.ClearErrors();
      if (this.ActivateDiagnostic(MBusConverterDiagnostic.ConverterCommands.SetStandbyCurrent))
        this.ReadDiagnostic();
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonIdentifyPowerOnCurrent_Click(object sender, EventArgs e)
    {
      this.textBoxMBusConverterData.Clear();
      ZR_ClassLibMessages.ClearErrors();
      if (this.ActivateDiagnostic(MBusConverterDiagnostic.ConverterCommands.SetPowerOnCurrent))
        this.ReadDiagnostic();
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonIdentifyStandbyCurrentSpectrum_Click(object sender, EventArgs e)
    {
      this.textBoxMBusConverterData.Clear();
      ZR_ClassLibMessages.ClearErrors();
      if (this.ActivateDiagnostic(MBusConverterDiagnostic.ConverterCommands.SetStandbyCurrentSpectrum))
        this.ReadDiagnostic();
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonReadErrorLogger_Click(object sender, EventArgs e)
    {
      this.textBoxMBusConverterData.Clear();
      ZR_ClassLibMessages.ClearErrors();
      if (this.ActivateDiagnostic(MBusConverterDiagnostic.ConverterCommands.SetReadErrorLogger))
        this.ReadErrorLogger();
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void buttonReadLastData_Click(object sender, EventArgs e)
    {
      this.textBoxMBusConverterData.Clear();
      ZR_ClassLibMessages.ClearErrors();
      this.ReadDiagnostic();
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private void TestFunction(
      MBusConverterDiagnostic.ConverterCommands theCommand)
    {
      this.textBoxMBusConverterData.Clear();
      ZR_ClassLibMessages.ClearErrors();
      if (this.ActivateDiagnostic(theCommand))
      {
        this.ReadDeviceMBusData();
        this.ReadDiagnostic();
      }
      ZR_ClassLibMessages.ShowAndClearErrors();
    }

    private bool ReadConverterMBusData()
    {
      this.MyFunctions.BreakRequest = false;
      if (!this.MBusConverterDevice.REQ_UD2() || !this.MBusConverterDevice.GenerateParameterList(true))
        return false;
      StringBuilder stringBuilder = new StringBuilder();
      string zdfParameterString = this.MBusConverterDevice.Info.GetZDFParameterString();
      stringBuilder.AppendLine(zdfParameterString);
      stringBuilder.AppendLine();
      Dictionary<string, string> parametersAsList = ParameterService.GetAllParametersAsList(zdfParameterString, ';');
      if (parametersAsList == null)
        return false;
      foreach (KeyValuePair<string, string> keyValuePair in parametersAsList)
      {
        try
        {
          string empty = string.Empty;
          string str = !(keyValuePair.Key == "A") ? keyValuePair.Key + ": " + keyValuePair.Value : keyValuePair.Key + ": " + keyValuePair.Value + "[MBus current in mA]";
          stringBuilder.AppendLine(str);
          if (keyValuePair.Key == "ERR")
          {
            int int32 = Convert.ToInt32(keyValuePair.Value, 16);
            if ((int32 & 1) != 0)
              stringBuilder.AppendLine("--> *** short-circuit on MBus lines ***");
            else
              stringBuilder.AppendLine("--> MBus current ok");
            if ((int32 & 2) != 0)
              stringBuilder.AppendLine("--> *** MBus+ line connected to earth ***");
            else
              stringBuilder.AppendLine("--> MBus+ line ok");
            if ((int32 & 4) != 0)
              stringBuilder.AppendLine("--> *** MBus- line connected to earth ***");
            else
              stringBuilder.AppendLine("--> MBus- line ok");
            if ((int32 & 8) != 0)
              stringBuilder.AppendLine("--> *** Receiver: Parity error found ***");
            else
              stringBuilder.AppendLine("--> No parity error found");
            if ((int32 & 16) != 0)
              stringBuilder.AppendLine("--> *** Receiver: Stop bit error found ***");
            else
              stringBuilder.AppendLine("--> No stop bit error found");
            if ((int32 & 32) != 0)
              stringBuilder.AppendLine("--> *** MBus over-current detected ***");
            else
              stringBuilder.AppendLine("--> MBus current ok");
          }
        }
        catch
        {
        }
      }
      this.textBoxMBusConverterData.Text = stringBuilder.ToString();
      return true;
    }

    private void ReadDeviceMBusData()
    {
      if (!this.SelectedDevice.GarantAddressingPossible())
      {
        this.textBoxMBusConverterData.AppendText("--- Device address not valied ---");
      }
      else
      {
        this.textBoxMBusConverterData.AppendText("--> Request device data ...");
        this.textBoxMBusConverterData.AppendText(Environment.NewLine);
        this.MyFunctions.MyCom.ClearCom();
        this.MyFunctions.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.REQ_UD2);
        this.SelectedDevice.GenerateREQ_UD2();
        this.MyFunctions.MyCom.TransmitBlock(ref this.SelectedDevice.TransmitBuffer);
        Thread.Sleep(1000);
        this.MyFunctions.MyCom.ClearCom();
        this.MyFunctions.MyCom.ResetEarliestTransmitTime();
        ZR_ClassLibMessages.ClearErrors();
      }
    }

    private bool ActivateDiagnostic(
      MBusConverterDiagnostic.ConverterCommands theCommand)
    {
      if (!this.MyFunctions.MyCom.Open())
        return false;
      this.textBoxMBusConverterData.Clear();
      int millisecondsTimeout;
      switch (theCommand)
      {
        case MBusConverterDiagnostic.ConverterCommands.SetReceiveCurrent:
          millisecondsTimeout = 100;
          this.textBoxMBusConverterData.AppendText("*** Identify receive current ***");
          break;
        case MBusConverterDiagnostic.ConverterCommands.SetReceiveCurrentFilterd:
          millisecondsTimeout = 100;
          this.textBoxMBusConverterData.AppendText("*** Identify receive current filterd ***");
          break;
        case MBusConverterDiagnostic.ConverterCommands.SetReceiveCurrentOnTransmit:
          millisecondsTimeout = 100;
          this.textBoxMBusConverterData.AppendText("*** Identify transmit current ***");
          break;
        case MBusConverterDiagnostic.ConverterCommands.SetTransmitVoltage:
          millisecondsTimeout = 100;
          this.textBoxMBusConverterData.AppendText("*** Identify transmit voltage ***");
          break;
        case MBusConverterDiagnostic.ConverterCommands.SetStandbyCurrent:
          millisecondsTimeout = 100;
          this.textBoxMBusConverterData.AppendText("*** Identify standby current ***");
          break;
        case MBusConverterDiagnostic.ConverterCommands.SetPowerOnCurrent:
          millisecondsTimeout = 1000;
          this.textBoxMBusConverterData.AppendText("*** Identify power on current ***");
          break;
        case MBusConverterDiagnostic.ConverterCommands.SetReadErrorLogger:
          millisecondsTimeout = 50;
          this.textBoxMBusConverterData.AppendText("*** Show error logger ***");
          break;
        case MBusConverterDiagnostic.ConverterCommands.SetStandbyCurrentSpectrum:
          millisecondsTimeout = 1000;
          this.textBoxMBusConverterData.AppendText("*** Identify standby current spectrum ***");
          break;
        default:
          this.textBoxMBusConverterData.AppendText("*** Identify command error ***");
          return false;
      }
      this.textBoxMBusConverterData.AppendText(Environment.NewLine);
      this.MyFunctions.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.MBusConverterCommand);
      this.MBusConverterDevice.GenerateSendDataHeader();
      this.MBusConverterDevice.TransmitBuffer.Add(15);
      this.MBusConverterDevice.TransmitBuffer.Add((byte) theCommand);
      this.MBusConverterDevice.FinishLongFrame();
      bool flag = false;
      while (this.MyFunctions.BusState.TestRepeatCounter(this.MyFunctions.MaxRequestRepeat))
      {
        this.MyFunctions.MyCom.TransmitBlock(ref this.MBusConverterDevice.TransmitBuffer);
        this.MyFunctions.BusState.IncrementTransmitBlockCounter();
        if (!this.MBusConverterDevice.ReceiveOkNok())
        {
          if (MBusConverterDiagnostic.MBusConverterLogger.IsWarnEnabled && this.MyFunctions.BusState.RepeadCounter < this.MyFunctions.MaxRequestRepeat)
            MBusConverterDiagnostic.MBusConverterLogger.Warn("--> Activate diagnostic error. Repeat command!");
        }
        else
        {
          flag = true;
          break;
        }
      }
      if (flag)
      {
        MBusConverterDiagnostic.MBusConverterLogger.Debug("--> Activate diagnostic done");
        Thread.Sleep(millisecondsTimeout);
      }
      else
        MBusConverterDiagnostic.MBusConverterLogger.Error("--> Activate diagnostic error");
      return flag;
    }

    private bool ReadErrorLogger()
    {
      this.textBoxMBusConverterData.AppendText("--> Diagnostics");
      this.textBoxMBusConverterData.AppendText(Environment.NewLine);
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      this.DiagnositcValuesOffset = 0;
      for (int PacketNumber = 10; PacketNumber <= 14; ++PacketNumber)
      {
        this.textBoxMBusConverterData.AppendText("--> Read block " + PacketNumber.ToString());
        this.textBoxMBusConverterData.AppendText(Environment.NewLine);
        if (this.ReadCurrentDiagnosticBlock(PacketNumber))
        {
          if (PacketNumber == 10)
          {
            num1 = this.DiagnositcValuesInt[0] | this.DiagnositcValuesInt[1] << 16;
            num2 = this.DiagnositcValuesInt[2];
            num3 = (num2 * 6 + 2) / 230;
          }
          if (num3 <= 0)
            break;
        }
        else
          break;
      }
      StringBuilder stringBuilder = new StringBuilder(3000);
      stringBuilder.AppendLine("--> Diagnostic type: '" + this.typeOfData.ToString() + "'");
      stringBuilder.AppendLine("--> Number of errors: '" + num2.ToString() + "'");
      int index1 = 3;
      for (int index2 = 0; index2 < num2; ++index2)
      {
        int num4 = this.DiagnositcValuesInt[index1] | this.DiagnositcValuesInt[index1 + 1] << 16;
        int num5 = this.DiagnositcValuesInt[index1 + 2];
        index1 += 3;
        string str = "illegal time: ";
        int num6 = num1 - num4;
        if (num6 >= 0)
        {
          DateTime dateTime = SystemValues.DateTimeNow.AddSeconds((double) num6 * -1.0);
          str = dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
        }
        stringBuilder.AppendLine();
        stringBuilder.Append(str + ": ");
        stringBuilder.Append(((MBusConverterDiagnostic.MBusConverterReadErrors) num5).ToString());
      }
      this.textBoxMBusConverterData.AppendText(stringBuilder.ToString());
      this.chart1.Series[0].Points.Clear();
      for (int xValue = 0; xValue < this.DiagnositcValues.Length; ++xValue)
        this.chart1.Series[0].Points.AddXY((double) xValue, this.DiagnositcValues[xValue]);
      this.textBoxMBusConverterData.Select(0, 10);
      this.textBoxMBusConverterData.ScrollToCaret();
      this.textBoxMBusConverterData.DeselectAll();
      return true;
    }

    private bool ReadDiagnostic()
    {
      this.textBoxMBusConverterData.AppendText("--> Diagnostics");
      this.textBoxMBusConverterData.AppendText(Environment.NewLine);
      bool flag = false;
      this.DiagnositcValuesOffset = 0;
      for (int PacketNumber = 10; PacketNumber <= 14; ++PacketNumber)
      {
        this.textBoxMBusConverterData.AppendText("--> Read block " + PacketNumber.ToString());
        this.textBoxMBusConverterData.AppendText(Environment.NewLine);
        flag = this.ReadCurrentDiagnosticBlock(PacketNumber);
        if (flag)
        {
          if (PacketNumber == 10)
          {
            this.textBoxMBusConverterData.AppendText("--> Diagnostic type: '" + this.typeOfData.ToString() + "'");
            this.textBoxMBusConverterData.AppendText(Environment.NewLine);
          }
        }
        else
          break;
      }
      StringBuilder stringBuilder = new StringBuilder(3000);
      for (int index = 0; index < this.DiagnositcValuesOffset; ++index)
      {
        stringBuilder.AppendLine();
        stringBuilder.Append(index.ToString("d04"));
        stringBuilder.Append(":");
        stringBuilder.Append(" ");
        stringBuilder.Append(this.DiagnositcValues[index].ToString());
      }
      this.textBoxMBusConverterData.AppendText(stringBuilder.ToString());
      this.chart1.Series[0].Points.Clear();
      for (int xValue = 0; xValue < this.DiagnositcValues.Length; ++xValue)
        this.chart1.Series[0].Points.AddXY((double) xValue, this.DiagnositcValues[xValue]);
      this.textBoxMBusConverterData.Select(0, 10);
      this.textBoxMBusConverterData.ScrollToCaret();
      this.textBoxMBusConverterData.DeselectAll();
      return flag;
    }

    private bool ReadCurrentDiagnosticBlock(int PacketNumber)
    {
      if (!this.MyFunctions.MyCom.Open())
        return false;
      this.MyFunctions.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.MBusConverterCommand);
      this.MBusConverterDevice.GenerateSendDataHeader();
      this.MBusConverterDevice.TransmitBuffer.Add(15);
      this.MBusConverterDevice.TransmitBuffer.Add((byte) PacketNumber);
      this.MBusConverterDevice.FinishLongFrame();
      bool flag = false;
      while (this.MyFunctions.BusState.TestRepeatCounter(this.MyFunctions.MaxRequestRepeat))
      {
        this.MyFunctions.MyCom.TransmitBlock(ref this.MBusConverterDevice.TransmitBuffer);
        this.MyFunctions.BusState.IncrementTransmitBlockCounter();
        if (!this.MBusConverterDevice.ReceiveHeader())
          MBusConverterDiagnostic.MBusConverterLogger.Debug("--> !!! Receive header error !!!");
        else if (!this.MBusConverterDevice.ReceiveLongframeEnd())
          MBusConverterDiagnostic.MBusConverterLogger.Debug("--> !!! Receive longframe end error !!!");
        else if (this.MBusConverterDevice.ReceiveBuffer.Count <= 10)
          MBusConverterDiagnostic.MBusConverterLogger.Debug("--> !!! Wron block size received !!!");
        else if (this.MBusConverterDevice.ReceiveBuffer.Data[0] != (byte) 15)
        {
          MBusConverterDiagnostic.MBusConverterLogger.Debug("--> !!! Wron block received !!!");
        }
        else
        {
          int num1 = 1;
          if (PacketNumber == 10)
          {
            this.use_mA_Table = false;
            this.scaleFactor = 0.0;
            this.typeOfData = (MBusConverterDiagnostic.ConverterCommands) this.MBusConverterDevice.ReceiveBuffer.Data[num1++];
            if (this.typeOfData == MBusConverterDiagnostic.ConverterCommands.SetReceiveCurrent)
              this.use_mA_Table = true;
            else if (this.typeOfData == MBusConverterDiagnostic.ConverterCommands.SetReceiveCurrentFilterd)
              this.use_mA_Table = true;
            else if (this.typeOfData == MBusConverterDiagnostic.ConverterCommands.SetReceiveCurrentOnTransmit)
              this.use_mA_Table = true;
            else if (this.typeOfData == MBusConverterDiagnostic.ConverterCommands.SetTransmitVoltage)
              this.scaleFactor = 0.0106;
            else if (this.typeOfData == MBusConverterDiagnostic.ConverterCommands.SetStandbyCurrent)
              this.use_mA_Table = true;
            else if (this.typeOfData == MBusConverterDiagnostic.ConverterCommands.SetPowerOnCurrent)
              this.use_mA_Table = true;
            else if (this.typeOfData != MBusConverterDiagnostic.ConverterCommands.SetReadErrorLogger && this.typeOfData == MBusConverterDiagnostic.ConverterCommands.SetStandbyCurrentSpectrum)
            {
              this.scaleFactor = 1.0;
              byte[] data1 = this.MBusConverterDevice.ReceiveBuffer.Data;
              int index1 = num1;
              int num2 = index1 + 1;
              this.Grundpegel = (int) data1[index1];
              int grundpegel = this.Grundpegel;
              byte[] data2 = this.MBusConverterDevice.ReceiveBuffer.Data;
              int index2 = num2;
              int num3 = index2 + 1;
              int num4 = (int) data2[index2] << 8;
              this.Grundpegel = grundpegel | num4;
              byte[] data3 = this.MBusConverterDevice.ReceiveBuffer.Data;
              int index3 = num3;
              int num5 = index3 + 1;
              this.Triggergrenze = (int) data3[index3];
              int triggergrenze = this.Triggergrenze;
              byte[] data4 = this.MBusConverterDevice.ReceiveBuffer.Data;
              int index4 = num5;
              num1 = index4 + 1;
              int num6 = (int) data4[index4] << 8;
              this.Triggergrenze = triggergrenze | num6;
              this.textBoxMBusConverterData.AppendText("--> Base level: " + this.Grundpegel.ToString());
              this.textBoxMBusConverterData.AppendText(Environment.NewLine);
              this.textBoxMBusConverterData.AppendText("--> Trigger level: " + this.Triggergrenze.ToString());
              this.textBoxMBusConverterData.AppendText(Environment.NewLine);
            }
          }
          while (num1 < this.MBusConverterDevice.ReceiveBuffer.Count - 2)
          {
            if (this.DiagnositcValuesOffset >= this.DiagnositcValues.Length)
              break;
            byte[] data5 = this.MBusConverterDevice.ReceiveBuffer.Data;
            int index5 = num1;
            int num7 = index5 + 1;
            int num8 = (int) data5[index5];
            byte[] data6 = this.MBusConverterDevice.ReceiveBuffer.Data;
            int index6 = num7;
            num1 = index6 + 1;
            int num9 = (int) data6[index6] << 8;
            int num10 = num8 | num9;
            if (this.use_mA_Table)
            {
              for (int index7 = 0; index7 < this.mA_Table.Count; ++index7)
              {
                if (this.mA_Table[index7].ADC_Value > num10)
                {
                  this.scaleFactor = this.mA_Table[index7].factor;
                  break;
                }
              }
            }
            this.DiagnositcValuesInt[this.DiagnositcValuesOffset] = num10;
            this.DiagnositcValues[this.DiagnositcValuesOffset++] = (double) num10 * this.scaleFactor;
          }
          flag = true;
          break;
        }
      }
      if (flag)
      {
        MBusConverterDiagnostic.MBusConverterLogger.Debug("--> Activate diagnostic done");
      }
      else
      {
        MBusConverterDiagnostic.MBusConverterLogger.Error("--> Activate diagnostic error");
        this.textBoxMBusConverterData.AppendText(Environment.NewLine);
        this.textBoxMBusConverterData.AppendText("--> Read block error");
      }
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
      ChartArea chartArea = new ChartArea();
      Legend legend = new Legend();
      Series series = new Series();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MBusConverterDiagnostic));
      this.zennerCoroprateDesign1 = new ZennerCoroprateDesign();
      this.textBoxMBusConverterData = new TextBox();
      this.buttonIdentifyReceiveCurrent = new Button();
      this.buttonIdentifyReceiveCurrentFilterd = new Button();
      this.buttonIdentifyTransmitCurrent = new Button();
      this.buttonIdentifyTransmitVoltage = new Button();
      this.buttonReadConverterData = new Button();
      this.buttonIdentifyStandbyCurrent = new Button();
      this.buttonIdentifyPowerOnCurrent = new Button();
      this.buttonReadLastData = new Button();
      this.buttonReadErrorLogger = new Button();
      this.buttonIdentifyStandbyCurrentSpectrum = new Button();
      this.chart1 = new Chart();
      this.chart1.BeginInit();
      this.SuspendLayout();
      this.zennerCoroprateDesign1.Dock = DockStyle.Top;
      this.zennerCoroprateDesign1.Location = new Point(0, 0);
      this.zennerCoroprateDesign1.Name = "zennerCoroprateDesign1";
      this.zennerCoroprateDesign1.Size = new Size(1040, 40);
      this.zennerCoroprateDesign1.TabIndex = 0;
      this.textBoxMBusConverterData.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxMBusConverterData.Font = new Font("Courier New", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.textBoxMBusConverterData.Location = new Point(12, 461);
      this.textBoxMBusConverterData.Multiline = true;
      this.textBoxMBusConverterData.Name = "textBoxMBusConverterData";
      this.textBoxMBusConverterData.ScrollBars = ScrollBars.Both;
      this.textBoxMBusConverterData.Size = new Size(697, 277);
      this.textBoxMBusConverterData.TabIndex = 1;
      this.buttonIdentifyReceiveCurrent.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonIdentifyReceiveCurrent.Enabled = false;
      this.buttonIdentifyReceiveCurrent.Location = new Point(715, 508);
      this.buttonIdentifyReceiveCurrent.Name = "buttonIdentifyReceiveCurrent";
      this.buttonIdentifyReceiveCurrent.Size = new Size(313, 23);
      this.buttonIdentifyReceiveCurrent.TabIndex = 2;
      this.buttonIdentifyReceiveCurrent.Text = "Identify receive current";
      this.buttonIdentifyReceiveCurrent.UseVisualStyleBackColor = true;
      this.buttonIdentifyReceiveCurrent.Click += new System.EventHandler(this.buttonIdentifyReceiveCurrent_Click);
      this.buttonIdentifyReceiveCurrentFilterd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonIdentifyReceiveCurrentFilterd.Enabled = false;
      this.buttonIdentifyReceiveCurrentFilterd.Location = new Point(715, 531);
      this.buttonIdentifyReceiveCurrentFilterd.Name = "buttonIdentifyReceiveCurrentFilterd";
      this.buttonIdentifyReceiveCurrentFilterd.Size = new Size(313, 23);
      this.buttonIdentifyReceiveCurrentFilterd.TabIndex = 2;
      this.buttonIdentifyReceiveCurrentFilterd.Text = "Identify receive current filterd";
      this.buttonIdentifyReceiveCurrentFilterd.UseVisualStyleBackColor = true;
      this.buttonIdentifyReceiveCurrentFilterd.Click += new System.EventHandler(this.buttonIdentifyReceiveCurrentFilterd_Click);
      this.buttonIdentifyTransmitCurrent.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonIdentifyTransmitCurrent.Enabled = false;
      this.buttonIdentifyTransmitCurrent.Location = new Point(715, 554);
      this.buttonIdentifyTransmitCurrent.Name = "buttonIdentifyTransmitCurrent";
      this.buttonIdentifyTransmitCurrent.Size = new Size(313, 23);
      this.buttonIdentifyTransmitCurrent.TabIndex = 2;
      this.buttonIdentifyTransmitCurrent.Text = "Identify transmit current";
      this.buttonIdentifyTransmitCurrent.UseVisualStyleBackColor = true;
      this.buttonIdentifyTransmitCurrent.Click += new System.EventHandler(this.buttonIdentifyTransmitCurrent_Click);
      this.buttonIdentifyTransmitVoltage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonIdentifyTransmitVoltage.Enabled = false;
      this.buttonIdentifyTransmitVoltage.Location = new Point(715, 577);
      this.buttonIdentifyTransmitVoltage.Name = "buttonIdentifyTransmitVoltage";
      this.buttonIdentifyTransmitVoltage.Size = new Size(313, 23);
      this.buttonIdentifyTransmitVoltage.TabIndex = 2;
      this.buttonIdentifyTransmitVoltage.Text = "Identify transmit voltage";
      this.buttonIdentifyTransmitVoltage.UseVisualStyleBackColor = true;
      this.buttonIdentifyTransmitVoltage.Click += new System.EventHandler(this.buttonIdentifyTransmitVoltage_Click);
      this.buttonReadConverterData.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonReadConverterData.Enabled = false;
      this.buttonReadConverterData.Location = new Point(715, 461);
      this.buttonReadConverterData.Name = "buttonReadConverterData";
      this.buttonReadConverterData.Size = new Size(313, 23);
      this.buttonReadConverterData.TabIndex = 2;
      this.buttonReadConverterData.Text = "Read converter data";
      this.buttonReadConverterData.UseVisualStyleBackColor = true;
      this.buttonReadConverterData.Click += new System.EventHandler(this.buttonReadConverterData_Click);
      this.buttonIdentifyStandbyCurrent.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonIdentifyStandbyCurrent.Enabled = false;
      this.buttonIdentifyStandbyCurrent.Location = new Point(715, 600);
      this.buttonIdentifyStandbyCurrent.Name = "buttonIdentifyStandbyCurrent";
      this.buttonIdentifyStandbyCurrent.Size = new Size(313, 23);
      this.buttonIdentifyStandbyCurrent.TabIndex = 2;
      this.buttonIdentifyStandbyCurrent.Text = "Identify standby current";
      this.buttonIdentifyStandbyCurrent.UseVisualStyleBackColor = true;
      this.buttonIdentifyStandbyCurrent.Click += new System.EventHandler(this.buttonIdentifyStandbyCurrent_Click);
      this.buttonIdentifyPowerOnCurrent.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonIdentifyPowerOnCurrent.Enabled = false;
      this.buttonIdentifyPowerOnCurrent.Location = new Point(715, 623);
      this.buttonIdentifyPowerOnCurrent.Name = "buttonIdentifyPowerOnCurrent";
      this.buttonIdentifyPowerOnCurrent.Size = new Size(313, 23);
      this.buttonIdentifyPowerOnCurrent.TabIndex = 2;
      this.buttonIdentifyPowerOnCurrent.Text = "Identify power on current";
      this.buttonIdentifyPowerOnCurrent.UseVisualStyleBackColor = true;
      this.buttonIdentifyPowerOnCurrent.Click += new System.EventHandler(this.buttonIdentifyPowerOnCurrent_Click);
      this.buttonReadLastData.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonReadLastData.Enabled = false;
      this.buttonReadLastData.Location = new Point(715, 715);
      this.buttonReadLastData.Name = "buttonReadLastData";
      this.buttonReadLastData.Size = new Size(313, 23);
      this.buttonReadLastData.TabIndex = 2;
      this.buttonReadLastData.Text = "Read last data";
      this.buttonReadLastData.UseVisualStyleBackColor = true;
      this.buttonReadLastData.Click += new System.EventHandler(this.buttonReadLastData_Click);
      this.buttonReadErrorLogger.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonReadErrorLogger.Enabled = false;
      this.buttonReadErrorLogger.Location = new Point(715, 669);
      this.buttonReadErrorLogger.Name = "buttonReadErrorLogger";
      this.buttonReadErrorLogger.Size = new Size(313, 23);
      this.buttonReadErrorLogger.TabIndex = 2;
      this.buttonReadErrorLogger.Text = "Read error logger";
      this.buttonReadErrorLogger.UseVisualStyleBackColor = true;
      this.buttonReadErrorLogger.Click += new System.EventHandler(this.buttonReadErrorLogger_Click);
      this.buttonIdentifyStandbyCurrentSpectrum.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonIdentifyStandbyCurrentSpectrum.Enabled = false;
      this.buttonIdentifyStandbyCurrentSpectrum.Location = new Point(715, 646);
      this.buttonIdentifyStandbyCurrentSpectrum.Name = "buttonIdentifyStandbyCurrentSpectrum";
      this.buttonIdentifyStandbyCurrentSpectrum.Size = new Size(313, 23);
      this.buttonIdentifyStandbyCurrentSpectrum.TabIndex = 2;
      this.buttonIdentifyStandbyCurrentSpectrum.Text = "Identify standby current spectrum";
      this.buttonIdentifyStandbyCurrentSpectrum.UseVisualStyleBackColor = true;
      this.buttonIdentifyStandbyCurrentSpectrum.Click += new System.EventHandler(this.buttonIdentifyStandbyCurrentSpectrum_Click);
      this.chart1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      chartArea.Name = "ChartArea1";
      this.chart1.ChartAreas.Add(chartArea);
      legend.Name = "Legend1";
      this.chart1.Legends.Add(legend);
      this.chart1.Location = new Point(12, 46);
      this.chart1.Name = "chart1";
      series.ChartArea = "ChartArea1";
      series.ChartType = SeriesChartType.Line;
      series.Legend = "Legend1";
      series.Name = "Daten";
      this.chart1.Series.Add(series);
      this.chart1.Size = new Size(1016, 409);
      this.chart1.TabIndex = 3;
      this.chart1.Text = "chart1";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1040, 750);
      this.Controls.Add((Control) this.chart1);
      this.Controls.Add((Control) this.buttonReadLastData);
      this.Controls.Add((Control) this.buttonIdentifyStandbyCurrentSpectrum);
      this.Controls.Add((Control) this.buttonReadErrorLogger);
      this.Controls.Add((Control) this.buttonIdentifyPowerOnCurrent);
      this.Controls.Add((Control) this.buttonIdentifyStandbyCurrent);
      this.Controls.Add((Control) this.buttonIdentifyTransmitVoltage);
      this.Controls.Add((Control) this.buttonIdentifyTransmitCurrent);
      this.Controls.Add((Control) this.buttonIdentifyReceiveCurrentFilterd);
      this.Controls.Add((Control) this.buttonReadConverterData);
      this.Controls.Add((Control) this.buttonIdentifyReceiveCurrent);
      this.Controls.Add((Control) this.textBoxMBusConverterData);
      this.Controls.Add((Control) this.zennerCoroprateDesign1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (MBusConverterDiagnostic);
      this.Text = "MBusConverter Diagnostic";
      this.Activated += new System.EventHandler(this.MBusConverterDiagnostic_Activated);
      this.chart1.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private enum ConverterCommands
    {
      SetReceiveCurrent = 0,
      SetReceiveCurrentFilterd = 1,
      SetReceiveCurrentOnTransmit = 2,
      SetTransmitVoltage = 3,
      SetStandbyCurrent = 4,
      SetPowerOnCurrent = 5,
      SetReadErrorLogger = 6,
      SetStandbyCurrentSpectrum = 7,
      RecDataBlock0 = 10, // 0x0000000A
      RecDataBlock1 = 11, // 0x0000000B
      RecDataBlock2 = 12, // 0x0000000C
      RecDataBlock3 = 13, // 0x0000000D
      RecDataBlock4 = 14, // 0x0000000E
    }

    internal class mA_TableEntry
    {
      internal int ADC_Value;
      internal double factor;

      internal mA_TableEntry(int ADC_Value, double factor)
      {
        this.ADC_Value = ADC_Value;
        this.factor = factor;
      }
    }

    private enum MBusConverterReadErrors
    {
      noError,
      parityError,
      stopbitError,
      dataCollision,
      lastErrorRepeated,
    }
  }
}

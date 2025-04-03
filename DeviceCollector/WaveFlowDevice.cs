// Decompiled with JetBrains decompiler
// Type: DeviceCollector.WaveFlowDevice
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using NS.Plugin.Wavenis;
using StartupLib;
using System;
using System.Collections;
using System.Threading;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class WaveFlowDevice : BusDevice
  {
    private string[] Repeaters;
    private bool RepeatersAreActivated;
    private int IndexA;
    private int IndexB;
    private int IndexC;
    private int IndexD;
    private int NbCounters;
    private WaveFlow.OperationMode.DatalogMode LoggingMode;
    private bool WireCutDetect;
    private bool LowLeakDetect;
    private bool HighLeakDetect;
    private bool ReedDetect;
    private bool BatteryLife;
    private bool WireCutA;
    private bool WireCutB;
    private bool LowLeak;
    private bool HighLeak;
    private bool ReedFaultAORWireCutC;
    private bool ReedFaultBORWireCutD;
    private bool ReverseLeak;
    private byte PulseValueA;
    private WaveFlow.PulseWeight.PulseUnit PulseUnitA;
    private byte PulseValueB;
    private WaveFlow.PulseWeight.PulseUnit PulseUnitB;
    private byte PulseValueC;
    private WaveFlow.PulseWeight.PulseUnit PulseUnitC;
    private byte PulseValueD;
    private WaveFlow.PulseWeight.PulseUnit PulseUnitD;
    private string Firmware;
    private byte RSSI;
    private DateTime DeviceTime;
    private string LastErrorString;

    public WaveFlowDevice(DeviceCollectorFunctions TheBus)
      : base(TheBus)
    {
      if (this.MyBus.MyWavePort == null)
        this.MyBus.MyWavePort = new WavePortConnector(TheBus);
      this.DeviceType = DeviceTypes.WaveFlowDevice;
      this.Info.Manufacturer = "COR";
      this.Info.ManufacturerCode = (short) 3570;
      this.Info.Medium = (byte) 7;
      this.LastErrorString = string.Empty;
      this.Repeaters = new string[0];
      this.RepeatersAreActivated = false;
    }

    private string[] GetConvertedRepeaters()
    {
      if (!this.RepeatersAreActivated || this.Repeaters.Length == 0)
        return (string[]) null;
      string[] convertedRepeaters = new string[this.Repeaters.Length];
      for (int index = 0; index < this.Repeaters.Length; ++index)
        convertedRepeaters[index] = this.GetConvertedSN(this.FillSerialNumber(this.Repeaters[index]));
      return convertedRepeaters;
    }

    internal override bool SetRepeaters(string[] SerialNumbers, out string Fehlerstring)
    {
      Fehlerstring = string.Empty;
      if (SerialNumbers.Length < 1 || SerialNumbers.Length > 3)
      {
        this.LastErrorString = "Wrong number of SerialNumbers! (Min = 1, Max = 3)!";
        Fehlerstring = this.LastErrorString;
        return false;
      }
      for (int index = 0; index < SerialNumbers.Length; ++index)
      {
        if (this.GetConvertedSN(this.FillSerialNumber(SerialNumbers[index])) == string.Empty)
        {
          this.LastErrorString = "Wrong SerialNumber! (Pos: " + index.ToString() + ")!";
          Fehlerstring = this.LastErrorString;
          return false;
        }
      }
      this.Repeaters = new string[SerialNumbers.Length];
      for (int index = 0; index < SerialNumbers.Length; ++index)
        this.Repeaters[index] = this.FillSerialNumber(SerialNumbers[index]);
      return true;
    }

    internal override void ActivateRepeaters() => this.RepeatersAreActivated = true;

    internal override void DeactivateRepeaters() => this.RepeatersAreActivated = false;

    internal override string[] GetRepeaters() => this.Repeaters;

    internal override bool GetRepeatersAreActivated() => this.RepeatersAreActivated;

    private void ClearParameters()
    {
      this.IndexA = 0;
      this.IndexB = 0;
      this.IndexC = 0;
      this.IndexD = 0;
      this.NbCounters = 0;
      this.LoggingMode = (WaveFlow.OperationMode.DatalogMode) 0;
      this.WireCutDetect = false;
      this.LowLeakDetect = false;
      this.HighLeakDetect = false;
      this.ReedDetect = false;
      this.BatteryLife = false;
      this.WireCutA = false;
      this.WireCutB = false;
      this.LowLeak = false;
      this.HighLeak = false;
      this.ReedFaultAORWireCutC = false;
      this.ReedFaultBORWireCutD = false;
      this.ReverseLeak = false;
      this.PulseValueA = (byte) 0;
      this.PulseUnitA = (WaveFlow.PulseWeight.PulseUnit) 0;
      this.PulseValueB = (byte) 0;
      this.PulseUnitB = (WaveFlow.PulseWeight.PulseUnit) 0;
      this.PulseValueC = (byte) 0;
      this.PulseUnitC = (WaveFlow.PulseWeight.PulseUnit) 0;
      this.PulseValueD = (byte) 0;
      this.PulseUnitD = (WaveFlow.PulseWeight.PulseUnit) 0;
      this.Firmware = string.Empty;
      this.RSSI = (byte) 0;
      this.DeviceTime = new DateTime();
    }

    internal string GetParameterString()
    {
      return "IndexA = " + this.IndexA.ToString() + ZR_Constants.SystemNewLine + "IndexB = " + this.IndexB.ToString() + ZR_Constants.SystemNewLine + "IndexC = " + this.IndexC.ToString() + ZR_Constants.SystemNewLine + "IndexD = " + this.IndexD.ToString() + ZR_Constants.SystemNewLine + "NbCounters = " + this.NbCounters.ToString() + ZR_Constants.SystemNewLine + "LoggingMode = " + this.LoggingMode.ToString() + ZR_Constants.SystemNewLine + "WireCutDetect = " + this.WireCutDetect.ToString() + ZR_Constants.SystemNewLine + "LowLeakDetect = " + this.LowLeakDetect.ToString() + ZR_Constants.SystemNewLine + "HighLeakDetect = " + this.HighLeakDetect.ToString() + ZR_Constants.SystemNewLine + "ReedDetect = " + this.ReedDetect.ToString() + ZR_Constants.SystemNewLine + "BatteryLife = " + this.BatteryLife.ToString() + ZR_Constants.SystemNewLine + "WireCutA = " + this.WireCutA.ToString() + ZR_Constants.SystemNewLine + "WireCutB = " + this.WireCutB.ToString() + ZR_Constants.SystemNewLine + "LowLeak = " + this.LowLeak.ToString() + ZR_Constants.SystemNewLine + "HighLeak = " + this.HighLeak.ToString() + ZR_Constants.SystemNewLine + "ReedFaultAORWireCutC = " + this.ReedFaultAORWireCutC.ToString() + ZR_Constants.SystemNewLine + "ReedFaultBORWireCutD = " + this.ReedFaultBORWireCutD.ToString() + ZR_Constants.SystemNewLine + "ReverseLeak = " + this.ReverseLeak.ToString() + ZR_Constants.SystemNewLine + "PulseValueA = " + this.PulseValueA.ToString() + ZR_Constants.SystemNewLine + "PulseUnitA = " + this.PulseUnitA.ToString() + ZR_Constants.SystemNewLine + "PulseValueB = " + this.PulseValueB.ToString() + ZR_Constants.SystemNewLine + "PulseUnitB = " + this.PulseUnitB.ToString() + ZR_Constants.SystemNewLine + "PulseValueC = " + this.PulseValueC.ToString() + ZR_Constants.SystemNewLine + "PulseUnitC = " + this.PulseUnitC.ToString() + ZR_Constants.SystemNewLine + "PulseValueD = " + this.PulseValueD.ToString() + ZR_Constants.SystemNewLine + "PulseUnitD = " + this.PulseUnitD.ToString() + ZR_Constants.SystemNewLine + "Firmware = " + this.Firmware + ZR_Constants.SystemNewLine + "RSSI = " + this.RSSI.ToString() + ZR_Constants.SystemNewLine + "DeviceTime = " + this.DeviceTime.ToString() + ZR_Constants.SystemNewLine;
    }

    internal bool ReadParameters()
    {
      if (!UserManager.CheckPermission(UserRights.Rights.Waveflow))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for Waveflow!");
        return false;
      }
      this.ClearParameters();
      this.MyBus.BreakRequest = false;
      this.Info.ParameterOk = false;
      this.Info.ParameterList.Clear();
      this.Info.LastReadingDate = ParameterService.GetNow();
      this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("RTIME", this.Info.LastReadingDate.ToString("dd.MM.yyyy HH:mm:ss")));
      this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("SID", this.Info.MeterNumber));
      object ParameterData1;
      if (!this.ReadParameterGroup(ParameterGroups.Indices, 3, out ParameterData1))
      {
        this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("R_ERR", "0"));
        return false;
      }
      try
      {
        SortedList sortedList1 = (SortedList) ParameterData1;
        this.NbCounters = (int) sortedList1[(object) WaveFlowDevice.ParameterNames.NbCounters];
        this.IndexA = (int) sortedList1[(object) WaveFlowDevice.ParameterNames.IndexA];
        if (this.NbCounters > 1)
          this.IndexB = (int) sortedList1[(object) WaveFlowDevice.ParameterNames.IndexB];
        if (this.NbCounters > 2)
          this.IndexC = (int) sortedList1[(object) WaveFlowDevice.ParameterNames.IndexC];
        if (this.NbCounters > 3)
          this.IndexD = (int) sortedList1[(object) WaveFlowDevice.ParameterNames.IndexD];
        this.LoggingMode = (WaveFlow.OperationMode.DatalogMode) sortedList1[(object) WaveFlowDevice.ParameterNames.LoggingMode];
        this.WireCutDetect = (bool) sortedList1[(object) WaveFlowDevice.ParameterNames.WireCutDetect];
        this.LowLeakDetect = (bool) sortedList1[(object) WaveFlowDevice.ParameterNames.LowLeakDetect];
        this.HighLeakDetect = (bool) sortedList1[(object) WaveFlowDevice.ParameterNames.HighLeakDetect];
        this.ReedDetect = (bool) sortedList1[(object) WaveFlowDevice.ParameterNames.ReedDetect];
        this.BatteryLife = (bool) sortedList1[(object) WaveFlowDevice.ParameterNames.BatteryLife];
        this.WireCutA = (bool) sortedList1[(object) WaveFlowDevice.ParameterNames.WireCutA];
        this.WireCutB = (bool) sortedList1[(object) WaveFlowDevice.ParameterNames.WireCutB];
        this.LowLeak = (bool) sortedList1[(object) WaveFlowDevice.ParameterNames.LowLeak];
        this.HighLeak = (bool) sortedList1[(object) WaveFlowDevice.ParameterNames.HighLeak];
        this.ReedFaultAORWireCutC = (bool) sortedList1[(object) WaveFlowDevice.ParameterNames.ReedFaultAORWireCutC];
        this.ReedFaultBORWireCutD = (bool) sortedList1[(object) WaveFlowDevice.ParameterNames.ReedFaultBORWireCutD];
        this.ReverseLeak = (bool) sortedList1[(object) WaveFlowDevice.ParameterNames.ReverseLeak];
        object ParameterData2 = new object();
        if (!this.ReadParameterGroup(ParameterGroups.PulseWeights, 3, out ParameterData2))
          return false;
        Application.DoEvents();
        if (this.MyBus.BreakRequest)
          return false;
        SortedList sortedList2 = (SortedList) ParameterData2;
        if (this.NbCounters > 0)
        {
          this.PulseValueA = (byte) sortedList2[(object) WaveFlowDevice.ParameterNames.PulseValueA];
          this.PulseUnitA = (WaveFlow.PulseWeight.PulseUnit) sortedList2[(object) WaveFlowDevice.ParameterNames.PulseUnitA];
        }
        if (this.NbCounters > 1)
        {
          this.PulseValueB = (byte) sortedList2[(object) WaveFlowDevice.ParameterNames.PulseValueB];
          this.PulseUnitB = (WaveFlow.PulseWeight.PulseUnit) sortedList2[(object) WaveFlowDevice.ParameterNames.PulseUnitB];
        }
        if (this.NbCounters > 2)
        {
          this.PulseValueC = (byte) sortedList2[(object) WaveFlowDevice.ParameterNames.PulseValueC];
          this.PulseUnitC = (WaveFlow.PulseWeight.PulseUnit) sortedList2[(object) WaveFlowDevice.ParameterNames.PulseUnitC];
        }
        if (this.NbCounters > 3)
        {
          this.PulseValueD = (byte) sortedList2[(object) WaveFlowDevice.ParameterNames.PulseValueD];
          this.PulseUnitD = (WaveFlow.PulseWeight.PulseUnit) sortedList2[(object) WaveFlowDevice.ParameterNames.PulseUnitD];
        }
        object ParameterData3;
        if (!this.ReadParameterGroup(ParameterGroups.Firmware, 3, out ParameterData3))
          return false;
        Application.DoEvents();
        if (this.MyBus.BreakRequest)
          return false;
        this.Firmware = (string) ((SortedList) ParameterData3)[(object) WaveFlowDevice.ParameterNames.Firmware];
        if (!this.ReadParameterGroup(ParameterGroups.RSSI, 3, out ParameterData3))
          return false;
        Application.DoEvents();
        if (this.MyBus.BreakRequest)
          return false;
        this.RSSI = (byte) ((SortedList) ParameterData3)[(object) WaveFlowDevice.ParameterNames.RSSI];
        if (!this.ReadParameterGroup(ParameterGroups.Date, 3, out ParameterData3))
          return false;
        Application.DoEvents();
        if (this.MyBus.BreakRequest)
          return false;
        this.DeviceTime = (DateTime) ((SortedList) ParameterData3)[(object) WaveFlowDevice.ParameterNames.Date];
        this.Info.Version = (byte) 1;
        this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("MAN", this.Info.Manufacturer));
        this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("MED", MBusDevice.GetMediaString(this.Info.Medium)));
        this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("GEN", this.Info.Version.ToString()));
        this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("RCL", this.RSSI.ToString()));
        this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("TIMP", this.DeviceTime.ToString("dd.MM.yyyy HH:mm:ss")));
        this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("NbCounters", this.NbCounters.ToString()));
        if (this.NbCounters > 0)
        {
          string QmIndexString;
          if (!this.GetQMIndexString(this.IndexA, this.PulseUnitA, this.PulseValueA, out QmIndexString))
            return false;
          this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("QM", QmIndexString));
        }
        if (this.NbCounters > 1)
        {
          string QmIndexString;
          if (!this.GetQMIndexString(this.IndexB, this.PulseUnitB, this.PulseValueB, out QmIndexString))
            return false;
          this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("QM[1]", QmIndexString));
        }
        if (this.NbCounters > 2)
        {
          string QmIndexString;
          if (!this.GetQMIndexString(this.IndexC, this.PulseUnitC, this.PulseValueC, out QmIndexString))
            return false;
          this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("QM[2]", QmIndexString));
        }
        if (this.NbCounters > 3)
        {
          string QmIndexString;
          if (!this.GetQMIndexString(this.IndexD, this.PulseUnitD, this.PulseValueD, out QmIndexString))
            return false;
          this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("QM[3]", QmIndexString));
        }
        this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("LoggingMode", this.LoggingMode.ToString()));
        if (this.WireCutDetect)
        {
          if (this.NbCounters > 0 && this.WireCutA)
            this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("WireCutA", "1"));
          if (this.NbCounters > 1 && this.WireCutB)
            this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("WireCutB", "1"));
        }
        if (this.BatteryLife)
          this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("BatteryLife", "1"));
        if (this.LowLeakDetect && this.LowLeak)
          this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("LowLeak", "1"));
        if (this.HighLeakDetect && this.HighLeak)
          this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("HighLeak", "1"));
        this.Info.ParameterOk = true;
        return true;
      }
      catch (Exception ex)
      {
        this.LastErrorString = ex.ToString();
        return false;
      }
    }

    internal override bool ReadParameterGroup(
      ParameterGroups TheParameterGroup,
      out object ParameterData)
    {
      return this.ReadParameterGroup(TheParameterGroup, 1, out ParameterData);
    }

    internal override bool ReadParameterGroup(
      ParameterGroups TheParameterGroup,
      int Retries,
      out object ParameterData)
    {
      this.LastErrorString = string.Empty;
      ParameterData = (object) null;
      string convertedSn = this.GetConvertedSN(this.GetSerialNumber());
      int num = Retries;
      bool flag = false;
      SortedList TheList = new SortedList();
      if (!this.MyBus.ComOpen())
      {
        this.LastErrorString = "Can't open COM!";
        return false;
      }
      while (num-- > 0 && !flag)
      {
        switch (TheParameterGroup)
        {
          case ParameterGroups.RSSI:
            if (this.WF_GetRSSI(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.Date:
            if (this.WF_GetDate(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.BatteryLifeCounter:
            if (this.WF_GetBatteryLifeCounter(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.DateForEndOfBatteryLifeDetection:
            if (this.WF_GetDateForEndOfBatteryLifeDetection(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.Firmware:
            if (this.WF_GetFirmware(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.GroupNumber:
            if (this.WF_GetGroupNumber(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.OperationModes:
            if (this.WF_GetOperationModes(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.Status:
            if (this.WF_GetStatus(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.PulseWeights:
            if (this.WF_GetPulseWeights(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.DateForReedFailure:
            if (this.WF_GetDateForReedFailure(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.DateForWireCutDetection:
            if (this.WF_GetDateForeWireCutDetection(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.Indices:
            if (this.WF_GetIndices(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.ExtendedIndex:
            if (this.WF_GetExtendedIndex(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.WakeUpMode:
            if (this.WF_GetWakeUpMode(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.DatalogPeriod:
            if (this.WF_GetDatalogPeriod(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.WeeklyDatalog:
            if (this.WF_GetWeeklyDatalog(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.MonthlyDatalog:
            if (this.WF_GetMonthlyDatalog(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.DatalogAllInputs:
            if (this.WF_GetDatalogAllInputs(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.Datalog:
            if (this.WF_GetDatalog(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.ExtendedDatalog:
            if (this.WF_GetExtendedDatalog(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.LowLeak:
            if (this.WF_GetLowLeak(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.HighLeak:
            if (this.WF_GetHighLeak(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.LeakHistory:
            if (this.WF_GetLeakHistory(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.BackFlow:
            if (this.WF_GetBackFlow(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.BackflowHistory:
            if (this.WF_GetBackflowHistory(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.SpecialBackFlow:
            if (this.WF_GetSpecialBackFlow(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.AlarmConfiguration:
            if (this.WF_GetAlarmConfiguration(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.FactoryConfiguration:
            if (this.WF_GetFactoryConfiguration(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          case ParameterGroups.SolenoidState:
            if (this.WF_GetSolenoidState(convertedSn, ref TheList))
            {
              flag = true;
              break;
            }
            break;
          default:
            this.LastErrorString = "No read function available for this parametergroup" + TheParameterGroup.ToString() + " !";
            flag = false;
            break;
        }
        if (!flag)
          Thread.Sleep(500);
      }
      ParameterData = (object) TheList;
      if (flag)
        return true;
      this.LastErrorString = string.Format("Readparametergroup '{0}' readout failed {1} times.", (object) TheParameterGroup, (object) num);
      return false;
    }

    internal override bool WriteParameterGroup(
      ParameterGroups TheParameterGroup,
      object ParameterData)
    {
      this.LastErrorString = string.Empty;
      string convertedSn = this.GetConvertedSN(this.GetSerialNumber());
      if (!this.MyBus.ComOpen())
      {
        this.LastErrorString = "Can't open COM!";
        return false;
      }
      switch (TheParameterGroup)
      {
        case ParameterGroups.Date:
          if (!this.WF_SetDate(convertedSn, ParameterData))
            return false;
          break;
        case ParameterGroups.GroupNumber:
          if (!this.WF_SetGroupNumber(convertedSn, ParameterData))
            return false;
          break;
        case ParameterGroups.OperationModes:
          if (!this.WF_SetOperationModes(convertedSn, ParameterData))
            return false;
          break;
        case ParameterGroups.Status:
          if (!this.WF_SetStatus(convertedSn, ParameterData))
            return false;
          break;
        case ParameterGroups.PulseWeights:
          if (!this.WF_SetPulseWeights(convertedSn, ParameterData))
            return false;
          break;
        case ParameterGroups.Indices:
          if (!this.WF_SetIndices(convertedSn, ParameterData))
            return false;
          break;
        case ParameterGroups.WakeUpMode:
          if (!this.WF_SetWakeUpMode(convertedSn, ParameterData))
            return false;
          break;
        case ParameterGroups.DatalogPeriod:
          if (!this.WF_SetDatalogPeriod(convertedSn, ParameterData))
            return false;
          break;
        case ParameterGroups.WeeklyDatalog:
          if (!this.WF_SetWeeklyDatalog(convertedSn, ParameterData))
            return false;
          break;
        case ParameterGroups.MonthlyDatalog:
          if (!this.WF_SetMonthlyDatalog(convertedSn, ParameterData))
            return false;
          break;
        case ParameterGroups.LowLeak:
          if (!this.WF_SetLowLeak(convertedSn, ParameterData))
            return false;
          break;
        case ParameterGroups.HighLeak:
          if (!this.WF_SetHighLeak(convertedSn, ParameterData))
            return false;
          break;
        case ParameterGroups.BackFlow:
          if (!this.WF_SetBackFlow(convertedSn, ParameterData))
            return false;
          break;
        case ParameterGroups.SpecialBackFlow:
          if (!this.WF_SetSpecialBackFlow(convertedSn, ParameterData))
            return false;
          break;
        case ParameterGroups.AlarmConfiguration:
          if (!this.WF_SetAlarmConfiguration(convertedSn, ParameterData))
            return false;
          break;
        case ParameterGroups.SolenoidState:
          if (!this.WF_SetSolenoidState(convertedSn, ParameterData))
            return false;
          break;
        default:
          this.LastErrorString = "No write function available for this parametergroup" + TheParameterGroup.ToString() + " !";
          return false;
      }
      return true;
    }

    internal override bool ResetParameterGroup(ParameterGroups TheParameterGroup)
    {
      this.LastErrorString = string.Empty;
      string convertedSn = this.GetConvertedSN(this.GetSerialNumber());
      if (!this.MyBus.ComOpen())
      {
        this.LastErrorString = "Can't open COM!";
        return false;
      }
      switch (TheParameterGroup)
      {
        case ParameterGroups.DateForEndOfBatteryLifeDetection:
          if (!this.WF_ResetDateForEndOfBatteryLifeDetection(convertedSn))
            return false;
          break;
        case ParameterGroups.DateForReedFailure:
          if (!this.WF_ResetDateForReedFailure(convertedSn))
            return false;
          break;
        case ParameterGroups.DateForWireCutDetection:
          if (!this.WF_ResetDateForeWireCutDetection(convertedSn))
            return false;
          break;
        default:
          this.LastErrorString = "No reset function available for this parametergroup" + TheParameterGroup.ToString() + " !";
          return false;
      }
      return true;
    }

    internal string GetSerialNumber() => this.FillSerialNumber(this.Info.MeterNumber);

    internal string FillSerialNumber(string SerialNumber)
    {
      SerialNumber = SerialNumber.Trim();
      for (int length = SerialNumber.Length; length < 15; ++length)
        SerialNumber = "0" + SerialNumber;
      return SerialNumber;
    }

    internal string GetLastErrorString() => this.LastErrorString;

    internal override bool ReadAnswerString(string RequestString, out string AnswerString)
    {
      string convertedSn = this.GetConvertedSN(this.GetSerialNumber());
      AnswerString = string.Empty;
      if (!this.MyBus.ComOpen())
      {
        this.LastErrorString = "Can't open COM!";
        return false;
      }
      try
      {
        if (RequestString.Trim().Substring(0, 2) == "0x")
        {
          if (!this.MyBus.MyWavePort.StartRequest(convertedSn, this.GetConvertedRepeaters(), RequestString.Substring(2), out AnswerString, out this.LastErrorString))
            return false;
        }
        else
          AnswerString = "Noch nicht implementiert!";
        return true;
      }
      catch (Exception ex)
      {
        this.LastErrorString = ex.ToString();
        return false;
      }
    }

    private bool WF_GetRSSI(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      byte num;
      WaveCardRequest.Errors rssi = ((WaveBase) this.MyBus.MyWavePort.MyWaveFlow).GetRSSI(SerialNumber, this.GetConvertedRepeaters(), ref num);
      if (rssi > 0)
      {
        this.LastErrorString = rssi.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      TheList[(object) WaveFlowDevice.ParameterNames.RSSI] = (object) num;
      return true;
    }

    private bool WF_GetDate(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      DateTime dateTime;
      WaveCardRequest.Errors date = ((WaveBase) this.MyBus.MyWavePort.MyWaveFlow).GetDate(SerialNumber, this.GetConvertedRepeaters(), ref dateTime);
      if (date > 0)
      {
        this.LastErrorString = date.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      TheList[(object) WaveFlowDevice.ParameterNames.Date] = (object) dateTime;
      return true;
    }

    private bool WF_GetBatteryLifeCounter(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      ushort num;
      WaveCardRequest.Errors batteryLifeCounter = this.MyBus.MyWavePort.MyWaveFlow.GetBatteryLifeCounter(SerialNumber, this.GetConvertedRepeaters(), ref num);
      if (batteryLifeCounter > 0)
      {
        this.LastErrorString = batteryLifeCounter.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      TheList[(object) WaveFlowDevice.ParameterNames.BatteryLifeCounter] = (object) num;
      return true;
    }

    private bool WF_GetDateForEndOfBatteryLifeDetection(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      DateTime dateTime;
      WaveCardRequest.Errors batteryLifeDetection = this.MyBus.MyWavePort.MyWaveFlow.GetDateForEndOfBatteryLifeDetection(SerialNumber, this.GetConvertedRepeaters(), ref dateTime);
      if (batteryLifeDetection > 0)
      {
        this.LastErrorString = batteryLifeDetection.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      TheList[(object) WaveFlowDevice.ParameterNames.EndOfBatteryLifeDetection] = (object) dateTime;
      return true;
    }

    private bool WF_GetFirmware(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      string str;
      WaveCardRequest.Errors firmware = ((WaveBase) this.MyBus.MyWavePort.MyWaveFlow).GetFirmware(SerialNumber, this.GetConvertedRepeaters(), ref str);
      if (firmware > 0)
      {
        this.LastErrorString = firmware.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      TheList[(object) WaveFlowDevice.ParameterNames.Firmware] = (object) str;
      return true;
    }

    private bool WF_GetGroupNumber(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      try
      {
        byte num;
        WaveCardRequest.Errors groupNumber = this.MyBus.MyWavePort.MyWaveFlow.GetGroupNumber(SerialNumber, this.GetConvertedRepeaters(), ref num);
        if (groupNumber > 0)
        {
          this.LastErrorString = groupNumber.ToString();
          TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
          return false;
        }
        TheList[(object) WaveFlowDevice.ParameterNames.GroupNumberForPolls] = (object) num;
        return true;
      }
      catch (Exception ex)
      {
        this.LastErrorString = ex.ToString();
        return false;
      }
    }

    private bool WF_GetOperationModes(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      WaveFlow.OperationMode operationMode1;
      WaveCardRequest.Errors operationMode2 = this.MyBus.MyWavePort.MyWaveFlow.GetOperationMode(SerialNumber, this.GetConvertedRepeaters(), ref operationMode1);
      if (operationMode2 > 0)
      {
        this.LastErrorString = operationMode2.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      TheList[(object) WaveFlowDevice.ParameterNames.NbCounters] = (object) operationMode1.NbCounters;
      TheList[(object) WaveFlowDevice.ParameterNames.LoggingMode] = (object) operationMode1.LoggingMode;
      TheList[(object) WaveFlowDevice.ParameterNames.WireCutDetect] = (object) operationMode1.WireCutDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.LowLeakDetect] = (object) operationMode1.LowLeakDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.HighLeakDetect] = (object) operationMode1.HighLeakDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.ReedDetect] = (object) operationMode1.ReedDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.BackflowDetectionMethod] = (object) operationMode1.BackflowDetection;
      return true;
    }

    private bool WF_GetStatus(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      WaveFlow.Status status1;
      WaveCardRequest.Errors status2 = this.MyBus.MyWavePort.MyWaveFlow.GetStatus(SerialNumber, this.GetConvertedRepeaters(), ref status1);
      if (status2 > 0)
      {
        this.LastErrorString = status2.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      TheList[(object) WaveFlowDevice.ParameterNames.BatteryLife] = (object) status1.BatteryLife;
      TheList[(object) WaveFlowDevice.ParameterNames.WireCutA] = (object) status1.WireCutA;
      TheList[(object) WaveFlowDevice.ParameterNames.WireCutB] = (object) status1.WireCutB;
      TheList[(object) WaveFlowDevice.ParameterNames.LowLeak] = (object) status1.LowLeak;
      TheList[(object) WaveFlowDevice.ParameterNames.HighLeak] = (object) status1.HighLeak;
      TheList[(object) WaveFlowDevice.ParameterNames.ReedFaultAORWireCutC] = (object) status1.ReedFaultAORWireCutC;
      TheList[(object) WaveFlowDevice.ParameterNames.ReedFaultBORWireCutD] = (object) status1.ReedFaultBORWireCutD;
      TheList[(object) WaveFlowDevice.ParameterNames.ReverseLeak] = (object) status1.ReverseLeak;
      return true;
    }

    private bool WF_GetPulseWeights(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      WaveFlow.PulseWeight[] pulseWeightArray;
      WaveCardRequest.Errors pulseWeights = this.MyBus.MyWavePort.MyWaveFlow.GetPulseWeights(SerialNumber, this.GetConvertedRepeaters(), ref pulseWeightArray);
      if (pulseWeights > 0)
      {
        this.LastErrorString = pulseWeights.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      for (int index = 0; index < pulseWeightArray.Length; ++index)
      {
        switch (index)
        {
          case 0:
            TheList[(object) WaveFlowDevice.ParameterNames.PulseValueA] = (object) pulseWeightArray[index].Coefficient;
            TheList[(object) WaveFlowDevice.ParameterNames.PulseUnitA] = (object) pulseWeightArray[index].Unit;
            break;
          case 1:
            TheList[(object) WaveFlowDevice.ParameterNames.PulseValueB] = (object) pulseWeightArray[index].Coefficient;
            TheList[(object) WaveFlowDevice.ParameterNames.PulseUnitB] = (object) pulseWeightArray[index].Unit;
            break;
          case 2:
            TheList[(object) WaveFlowDevice.ParameterNames.PulseValueC] = (object) pulseWeightArray[index].Coefficient;
            TheList[(object) WaveFlowDevice.ParameterNames.PulseUnitC] = (object) pulseWeightArray[index].Unit;
            break;
          case 3:
            TheList[(object) WaveFlowDevice.ParameterNames.PulseValueD] = (object) pulseWeightArray[index].Coefficient;
            TheList[(object) WaveFlowDevice.ParameterNames.PulseUnitD] = (object) pulseWeightArray[index].Unit;
            break;
        }
      }
      return true;
    }

    private bool WF_GetDateForReedFailure(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      DateTime[] dateTimeArray;
      WaveCardRequest.Errors dateForReedFailure = this.MyBus.MyWavePort.MyWaveFlow.GetDateForReedFailure(SerialNumber, this.GetConvertedRepeaters(), ref dateTimeArray);
      if (dateForReedFailure > 0)
      {
        this.LastErrorString = dateForReedFailure.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      for (int index = 0; index < dateTimeArray.Length; ++index)
      {
        switch (index)
        {
          case 0:
            TheList[(object) WaveFlowDevice.ParameterNames.ReedFaultADate] = (object) dateTimeArray[index];
            break;
          case 1:
            TheList[(object) WaveFlowDevice.ParameterNames.ReedFaultBDate] = (object) dateTimeArray[index];
            break;
        }
      }
      return true;
    }

    private bool WF_GetDateForeWireCutDetection(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      DateTime[] dateTimeArray;
      WaveCardRequest.Errors wireCutDetection = this.MyBus.MyWavePort.MyWaveFlow.GetDateForWireCutDetection(SerialNumber, this.GetConvertedRepeaters(), ref dateTimeArray);
      if (wireCutDetection > 0)
      {
        this.LastErrorString = wireCutDetection.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      for (int index = 0; index < dateTimeArray.Length; ++index)
      {
        switch (index)
        {
          case 0:
            TheList[(object) WaveFlowDevice.ParameterNames.WireCutADate] = (object) dateTimeArray[index];
            break;
          case 1:
            TheList[(object) WaveFlowDevice.ParameterNames.WireCutBDate] = (object) dateTimeArray[index];
            break;
          case 2:
            TheList[(object) WaveFlowDevice.ParameterNames.WireCutCDate] = (object) dateTimeArray[index];
            break;
          case 3:
            TheList[(object) WaveFlowDevice.ParameterNames.WireCutDDate] = (object) dateTimeArray[index];
            break;
        }
      }
      return true;
    }

    private bool WF_GetIndices(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      WaveFlow.OperationMode operationMode;
      WaveFlow.Status status;
      int[] numArray;
      WaveCardRequest.Errors index = this.MyBus.MyWavePort.MyWaveFlow.GetIndex(SerialNumber, this.GetConvertedRepeaters(), ref operationMode, ref status, ref numArray);
      if (index > 0)
      {
        this.LastErrorString = index.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      TheList[(object) WaveFlowDevice.ParameterNames.IndexA] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.IndexB] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.IndexC] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.IndexD] = (object) null;
      if (numArray.Length != 0)
        TheList[(object) WaveFlowDevice.ParameterNames.IndexA] = (object) numArray[0];
      if (numArray.Length > 1)
        TheList[(object) WaveFlowDevice.ParameterNames.IndexB] = (object) numArray[1];
      if (numArray.Length > 2)
        TheList[(object) WaveFlowDevice.ParameterNames.IndexC] = (object) numArray[2];
      if (numArray.Length > 3)
        TheList[(object) WaveFlowDevice.ParameterNames.IndexD] = (object) numArray[3];
      TheList[(object) WaveFlowDevice.ParameterNames.NbCounters] = (object) operationMode.NbCounters;
      TheList[(object) WaveFlowDevice.ParameterNames.LoggingMode] = (object) operationMode.LoggingMode;
      TheList[(object) WaveFlowDevice.ParameterNames.WireCutDetect] = (object) operationMode.WireCutDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.LowLeakDetect] = (object) operationMode.LowLeakDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.HighLeakDetect] = (object) operationMode.HighLeakDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.ReedDetect] = (object) operationMode.ReedDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.BackflowDetectionMethod] = (object) operationMode.BackflowDetection;
      TheList[(object) WaveFlowDevice.ParameterNames.BatteryLife] = (object) status.BatteryLife;
      TheList[(object) WaveFlowDevice.ParameterNames.WireCutA] = (object) status.WireCutA;
      TheList[(object) WaveFlowDevice.ParameterNames.WireCutB] = (object) status.WireCutB;
      TheList[(object) WaveFlowDevice.ParameterNames.LowLeak] = (object) status.LowLeak;
      TheList[(object) WaveFlowDevice.ParameterNames.HighLeak] = (object) status.HighLeak;
      TheList[(object) WaveFlowDevice.ParameterNames.ReedFaultAORWireCutC] = (object) status.ReedFaultAORWireCutC;
      TheList[(object) WaveFlowDevice.ParameterNames.ReedFaultBORWireCutD] = (object) status.ReedFaultBORWireCutD;
      TheList[(object) WaveFlowDevice.ParameterNames.ReverseLeak] = (object) status.ReverseLeak;
      return true;
    }

    private bool WF_GetExtendedIndex(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      WaveFlow.OperationMode operationMode;
      WaveFlow.Status status;
      int[] numArray1;
      int[] numArray2;
      int[][] numArray3;
      DateTime dateTime;
      WaveBase.MeasurePeriod measurePeriod;
      WaveCardRequest.Errors extendedIndex = this.MyBus.MyWavePort.MyWaveFlow.GetExtendedIndex(SerialNumber, this.GetConvertedRepeaters(), ref operationMode, ref status, ref numArray1, ref numArray2, ref numArray3, ref dateTime, ref measurePeriod);
      if (extendedIndex > 0)
      {
        this.LastErrorString = extendedIndex.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      for (int index = 0; index < 4; ++index)
      {
        switch (index)
        {
          case 0:
            TheList[(object) WaveFlowDevice.ParameterNames.IndexA] = (object) -1;
            TheList[(object) WaveFlowDevice.ParameterNames.IndexLMEA] = (object) -1;
            TheList[(object) WaveFlowDevice.ParameterNames.IndexA00] = (object) -1;
            TheList[(object) WaveFlowDevice.ParameterNames.IndexA01] = (object) -1;
            TheList[(object) WaveFlowDevice.ParameterNames.IndexA02] = (object) -1;
            TheList[(object) WaveFlowDevice.ParameterNames.IndexA03] = (object) -1;
            break;
          case 1:
            TheList[(object) WaveFlowDevice.ParameterNames.IndexB] = (object) -1;
            TheList[(object) WaveFlowDevice.ParameterNames.IndexLMEB] = (object) -1;
            TheList[(object) WaveFlowDevice.ParameterNames.IndexB00] = (object) -1;
            TheList[(object) WaveFlowDevice.ParameterNames.IndexB01] = (object) -1;
            TheList[(object) WaveFlowDevice.ParameterNames.IndexB02] = (object) -1;
            TheList[(object) WaveFlowDevice.ParameterNames.IndexB03] = (object) -1;
            break;
          case 2:
            TheList[(object) WaveFlowDevice.ParameterNames.IndexC] = (object) -1;
            TheList[(object) WaveFlowDevice.ParameterNames.IndexLMEC] = (object) -1;
            TheList[(object) WaveFlowDevice.ParameterNames.IndexC00] = (object) -1;
            TheList[(object) WaveFlowDevice.ParameterNames.IndexC01] = (object) -1;
            TheList[(object) WaveFlowDevice.ParameterNames.IndexC02] = (object) -1;
            TheList[(object) WaveFlowDevice.ParameterNames.IndexC03] = (object) -1;
            break;
          case 3:
            TheList[(object) WaveFlowDevice.ParameterNames.IndexD] = (object) -1;
            TheList[(object) WaveFlowDevice.ParameterNames.IndexLMED] = (object) -1;
            TheList[(object) WaveFlowDevice.ParameterNames.IndexD00] = (object) -1;
            TheList[(object) WaveFlowDevice.ParameterNames.IndexD01] = (object) -1;
            TheList[(object) WaveFlowDevice.ParameterNames.IndexD02] = (object) -1;
            TheList[(object) WaveFlowDevice.ParameterNames.IndexD03] = (object) -1;
            break;
        }
      }
      for (int index = 0; index < numArray1.Length; ++index)
      {
        switch (index)
        {
          case 0:
            TheList[(object) WaveFlowDevice.ParameterNames.IndexA] = (object) numArray1[index];
            TheList[(object) WaveFlowDevice.ParameterNames.IndexLMEA] = (object) numArray2[index];
            TheList[(object) WaveFlowDevice.ParameterNames.IndexA00] = (object) numArray3[index][0];
            TheList[(object) WaveFlowDevice.ParameterNames.IndexA01] = (object) numArray3[index][1];
            TheList[(object) WaveFlowDevice.ParameterNames.IndexA02] = (object) numArray3[index][2];
            TheList[(object) WaveFlowDevice.ParameterNames.IndexA03] = (object) numArray3[index][3];
            break;
          case 1:
            TheList[(object) WaveFlowDevice.ParameterNames.IndexB] = (object) numArray1[index];
            TheList[(object) WaveFlowDevice.ParameterNames.IndexLMEB] = (object) numArray2[index];
            TheList[(object) WaveFlowDevice.ParameterNames.IndexB00] = (object) numArray3[index][0];
            TheList[(object) WaveFlowDevice.ParameterNames.IndexB01] = (object) numArray3[index][1];
            TheList[(object) WaveFlowDevice.ParameterNames.IndexB02] = (object) numArray3[index][2];
            TheList[(object) WaveFlowDevice.ParameterNames.IndexB03] = (object) numArray3[index][3];
            break;
          case 2:
            TheList[(object) WaveFlowDevice.ParameterNames.IndexC] = (object) numArray1[index];
            TheList[(object) WaveFlowDevice.ParameterNames.IndexLMEC] = (object) numArray2[index];
            TheList[(object) WaveFlowDevice.ParameterNames.IndexC00] = (object) numArray3[index][0];
            TheList[(object) WaveFlowDevice.ParameterNames.IndexC01] = (object) numArray3[index][1];
            TheList[(object) WaveFlowDevice.ParameterNames.IndexC02] = (object) numArray3[index][2];
            TheList[(object) WaveFlowDevice.ParameterNames.IndexC03] = (object) numArray3[index][3];
            break;
          case 3:
            TheList[(object) WaveFlowDevice.ParameterNames.IndexD] = (object) numArray1[index];
            TheList[(object) WaveFlowDevice.ParameterNames.IndexLMED] = (object) numArray2[index];
            TheList[(object) WaveFlowDevice.ParameterNames.IndexD00] = (object) numArray3[index][0];
            TheList[(object) WaveFlowDevice.ParameterNames.IndexD01] = (object) numArray3[index][1];
            TheList[(object) WaveFlowDevice.ParameterNames.IndexD02] = (object) numArray3[index][2];
            TheList[(object) WaveFlowDevice.ParameterNames.IndexD03] = (object) numArray3[index][3];
            break;
        }
      }
      TheList[(object) WaveFlowDevice.ParameterNames.LoggerDate] = (object) dateTime;
      TheList[(object) WaveFlowDevice.ParameterNames.DatalogMeasurePeriodUnit] = (object) measurePeriod.Unit;
      TheList[(object) WaveFlowDevice.ParameterNames.DatalogMeasurePeriodCoefficient] = (object) measurePeriod.Coefficient;
      TheList[(object) WaveFlowDevice.ParameterNames.NbCounters] = (object) operationMode.NbCounters;
      TheList[(object) WaveFlowDevice.ParameterNames.LoggingMode] = (object) operationMode.LoggingMode;
      TheList[(object) WaveFlowDevice.ParameterNames.WireCutDetect] = (object) operationMode.WireCutDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.LowLeakDetect] = (object) operationMode.LowLeakDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.HighLeakDetect] = (object) operationMode.HighLeakDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.ReedDetect] = (object) operationMode.ReedDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.BackflowDetectionMethod] = (object) operationMode.BackflowDetection;
      TheList[(object) WaveFlowDevice.ParameterNames.BatteryLife] = (object) status.BatteryLife;
      TheList[(object) WaveFlowDevice.ParameterNames.WireCutA] = (object) status.WireCutA;
      TheList[(object) WaveFlowDevice.ParameterNames.WireCutB] = (object) status.WireCutB;
      TheList[(object) WaveFlowDevice.ParameterNames.LowLeak] = (object) status.LowLeak;
      TheList[(object) WaveFlowDevice.ParameterNames.HighLeak] = (object) status.HighLeak;
      TheList[(object) WaveFlowDevice.ParameterNames.ReedFaultAORWireCutC] = (object) status.ReedFaultAORWireCutC;
      TheList[(object) WaveFlowDevice.ParameterNames.ReedFaultBORWireCutD] = (object) status.ReedFaultBORWireCutD;
      TheList[(object) WaveFlowDevice.ParameterNames.ReverseLeak] = (object) status.ReverseLeak;
      return true;
    }

    private bool WF_GetWakeUpMode(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      TheList.Clear();
      WakeUpMode wakeUpMode1;
      byte num1;
      byte num2;
      byte num3;
      byte num4;
      byte num5;
      DayOfWeek dayOfWeek1;
      DayOfWeek dayOfWeek2;
      WaveCardRequest.Errors wakeUpMode2 = ((WaveBase) this.MyBus.MyWavePort.MyWaveFlow).GetWakeUpMode(SerialNumber, this.GetConvertedRepeaters(), ref wakeUpMode1, ref num1, ref num2, ref num3, ref num4, ref num5, ref dayOfWeek1, ref dayOfWeek2);
      if (wakeUpMode2 > 0)
      {
        this.LastErrorString = wakeUpMode2.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      try
      {
        TheList[(object) WaveFlowDevice.ParameterNames.WakeUpMode] = (object) wakeUpMode1;
        TheList[(object) WaveFlowDevice.ParameterNames.DefaultWakeupDuration] = (object) num1;
        TheList[(object) WaveFlowDevice.ParameterNames.HourFirstTimeFrameStarts] = (object) num2;
        TheList[(object) WaveFlowDevice.ParameterNames.FirstTimeFrameWakeupDuration] = (object) num3;
        TheList[(object) WaveFlowDevice.ParameterNames.HourSecondTimeFrameStarts] = (object) num4;
        TheList[(object) WaveFlowDevice.ParameterNames.SecondTimeFrameWakeupDuration] = (object) num5;
        TheList[(object) WaveFlowDevice.ParameterNames.TimeFramesEnabledOnMonday] = (object) ((dayOfWeek1 & 1) == 1);
        TheList[(object) WaveFlowDevice.ParameterNames.TimeFramesEnabledOnTuesday] = (object) ((dayOfWeek1 & 2) == 2);
        TheList[(object) WaveFlowDevice.ParameterNames.TimeFramesEnabledOnWednesday] = (object) ((dayOfWeek1 & 4) == 4);
        TheList[(object) WaveFlowDevice.ParameterNames.TimeFramesEnabledOnThursday] = (object) ((dayOfWeek1 & 8) == 8);
        TheList[(object) WaveFlowDevice.ParameterNames.TimeFramesEnabledOnFriday] = (object) ((dayOfWeek1 & 16) == 16);
        TheList[(object) WaveFlowDevice.ParameterNames.TimeFramesEnabledOnSaturday] = (object) ((dayOfWeek1 & 32) == 32);
        TheList[(object) WaveFlowDevice.ParameterNames.TimeFramesEnabledOnSunday] = (object) ((dayOfWeek1 & 64) == 64);
        TheList[(object) WaveFlowDevice.ParameterNames.WakeupDisabledOnMonday] = (object) ((dayOfWeek2 & 1) == 1);
        TheList[(object) WaveFlowDevice.ParameterNames.WakeupDisabledOnTuesday] = (object) ((dayOfWeek2 & 2) == 2);
        TheList[(object) WaveFlowDevice.ParameterNames.WakeupDisabledOnWednesday] = (object) ((dayOfWeek2 & 4) == 4);
        TheList[(object) WaveFlowDevice.ParameterNames.WakeupDisabledOnThursday] = (object) ((dayOfWeek2 & 8) == 8);
        TheList[(object) WaveFlowDevice.ParameterNames.WakeupDisabledOnFriday] = (object) ((dayOfWeek2 & 16) == 16);
        TheList[(object) WaveFlowDevice.ParameterNames.WakeupDisabledOnSaturday] = (object) ((dayOfWeek2 & 32) == 32);
        TheList[(object) WaveFlowDevice.ParameterNames.WakeupDisabledOnSunday] = (object) ((dayOfWeek2 & 64) == 64);
        return true;
      }
      catch (Exception ex)
      {
        this.LastErrorString = ex.ToString();
        return false;
      }
    }

    private bool WF_GetDatalogPeriod(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      byte num;
      WaveBase.MeasurePeriod measurePeriod;
      WaveCardRequest.Errors datalogPeriod = this.MyBus.MyWavePort.MyWaveFlow.GetDatalogPeriod(SerialNumber, this.GetConvertedRepeaters(), ref num, ref measurePeriod);
      if (datalogPeriod > 0)
      {
        this.LastErrorString = datalogPeriod.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      TheList[(object) WaveFlowDevice.ParameterNames.DatalogStart] = (object) num;
      TheList[(object) WaveFlowDevice.ParameterNames.DatalogMeasurePeriodUnit] = (object) measurePeriod.Unit;
      TheList[(object) WaveFlowDevice.ParameterNames.DatalogMeasurePeriodCoefficient] = (object) measurePeriod.Coefficient;
      return true;
    }

    private bool WF_GetWeeklyDatalog(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      DayOfWeek dayOfWeek;
      byte num;
      WaveCardRequest.Errors weeklyDatalog = this.MyBus.MyWavePort.MyWaveFlow.GetWeeklyDatalog(SerialNumber, this.GetConvertedRepeaters(), ref dayOfWeek, ref num);
      if (weeklyDatalog > 0)
      {
        this.LastErrorString = weeklyDatalog.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      TheList[(object) WaveFlowDevice.ParameterNames.TimeOfDatalogIf1PerMonthOrWeek] = (object) num;
      TheList[(object) WaveFlowDevice.ParameterNames.DayOfWeekOrMonth] = (object) (byte) dayOfWeek;
      return true;
    }

    private bool WF_GetMonthlyDatalog(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      byte num1;
      byte num2;
      WaveCardRequest.Errors monthlyDatalog = this.MyBus.MyWavePort.MyWaveFlow.GetMonthlyDatalog(SerialNumber, this.GetConvertedRepeaters(), ref num1, ref num2);
      if (monthlyDatalog > 0)
      {
        this.LastErrorString = monthlyDatalog.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      TheList[(object) WaveFlowDevice.ParameterNames.TimeOfDatalogIf1PerMonthOrWeek] = (object) num2;
      TheList[(object) WaveFlowDevice.ParameterNames.DayOfWeekOrMonth] = (object) num1;
      return true;
    }

    private bool WF_GetDatalogAllInputs(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      int[][] numArray;
      WaveFlow.Status status;
      WaveFlow.OperationMode operationMode;
      DateTime dateTime;
      WaveBase.MeasurePeriod measurePeriod;
      WaveCardRequest.Errors datalogAllInputs = this.MyBus.MyWavePort.MyWaveFlow.GetDatalogAllInputs(SerialNumber, this.GetConvertedRepeaters(), ref numArray, ref status, ref operationMode, ref dateTime, ref measurePeriod);
      if (datalogAllInputs > 0)
      {
        this.LastErrorString = datalogAllInputs.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      int num1 = -1;
      for (int index = 0; index < 24; ++index)
      {
        ++num1;
        WaveFlowDevice.ParameterNames key = (WaveFlowDevice.ParameterNames) Enum.Parse(typeof (WaveFlowDevice.ParameterNames), "IndexA" + num1.ToString("00"), false);
        TheList[(object) key] = (object) null;
      }
      int num2 = -1;
      for (int index = 0; index < 12; ++index)
      {
        ++num2;
        WaveFlowDevice.ParameterNames key = (WaveFlowDevice.ParameterNames) Enum.Parse(typeof (WaveFlowDevice.ParameterNames), "IndexB" + num2.ToString("00"), false);
        TheList[(object) key] = (object) null;
      }
      int num3 = -1;
      for (int index = 0; index < 12; ++index)
      {
        ++num3;
        WaveFlowDevice.ParameterNames key = (WaveFlowDevice.ParameterNames) Enum.Parse(typeof (WaveFlowDevice.ParameterNames), "IndexC" + num3.ToString("00"), false);
        TheList[(object) key] = (object) null;
      }
      int num4 = -1;
      for (int index = 0; index < 12; ++index)
      {
        ++num4;
        WaveFlowDevice.ParameterNames key = (WaveFlowDevice.ParameterNames) Enum.Parse(typeof (WaveFlowDevice.ParameterNames), "IndexD" + num4.ToString("00"), false);
        TheList[(object) key] = (object) null;
      }
      for (int index1 = 0; index1 < numArray.Length; ++index1)
      {
        string str;
        switch (index1)
        {
          case 0:
            str = "A";
            break;
          case 1:
            str = "B";
            break;
          case 2:
            str = "C";
            break;
          case 3:
            str = "D";
            break;
          default:
            this.LastErrorString = "Wrong Index!";
            return false;
        }
        int num5 = -1;
        for (int index2 = 0; index2 < numArray[index1].Length; ++index2)
        {
          ++num5;
          WaveFlowDevice.ParameterNames key = (WaveFlowDevice.ParameterNames) Enum.Parse(typeof (WaveFlowDevice.ParameterNames), "Index" + str + num5.ToString("00"), false);
          TheList[(object) key] = (object) numArray[index1][index2];
        }
      }
      TheList[(object) WaveFlowDevice.ParameterNames.LoggerDate] = (object) dateTime;
      TheList[(object) WaveFlowDevice.ParameterNames.DatalogMeasurePeriodUnit] = (object) measurePeriod.Unit;
      TheList[(object) WaveFlowDevice.ParameterNames.DatalogMeasurePeriodCoefficient] = (object) measurePeriod.Coefficient;
      TheList[(object) WaveFlowDevice.ParameterNames.NbCounters] = (object) operationMode.NbCounters;
      TheList[(object) WaveFlowDevice.ParameterNames.LoggingMode] = (object) operationMode.LoggingMode;
      TheList[(object) WaveFlowDevice.ParameterNames.WireCutDetect] = (object) operationMode.WireCutDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.LowLeakDetect] = (object) operationMode.LowLeakDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.HighLeakDetect] = (object) operationMode.HighLeakDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.ReedDetect] = (object) operationMode.ReedDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.BackflowDetectionMethod] = (object) operationMode.BackflowDetection;
      TheList[(object) WaveFlowDevice.ParameterNames.BatteryLife] = (object) status.BatteryLife;
      TheList[(object) WaveFlowDevice.ParameterNames.WireCutA] = (object) status.WireCutA;
      TheList[(object) WaveFlowDevice.ParameterNames.WireCutB] = (object) status.WireCutB;
      TheList[(object) WaveFlowDevice.ParameterNames.LowLeak] = (object) status.LowLeak;
      TheList[(object) WaveFlowDevice.ParameterNames.HighLeak] = (object) status.HighLeak;
      TheList[(object) WaveFlowDevice.ParameterNames.ReedFaultAORWireCutC] = (object) status.ReedFaultAORWireCutC;
      TheList[(object) WaveFlowDevice.ParameterNames.ReedFaultBORWireCutD] = (object) status.ReedFaultBORWireCutD;
      TheList[(object) WaveFlowDevice.ParameterNames.ReverseLeak] = (object) status.ReverseLeak;
      return true;
    }

    private bool WF_GetDatalog(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      int[][] numArray;
      WaveFlow.Status status;
      WaveFlow.OperationMode operationMode;
      DateTime dateTime;
      WaveBase.MeasurePeriod measurePeriod;
      WaveCardRequest.Errors datalog = this.MyBus.MyWavePort.MyWaveFlow.GetDatalog(SerialNumber, this.GetConvertedRepeaters(), ref numArray, ref status, ref operationMode, ref dateTime, ref measurePeriod);
      if (datalog > 0)
      {
        this.LastErrorString = datalog.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      int num1 = -1;
      for (int index = 0; index < 24; ++index)
      {
        ++num1;
        WaveFlowDevice.ParameterNames key = (WaveFlowDevice.ParameterNames) Enum.Parse(typeof (WaveFlowDevice.ParameterNames), "IndexA" + num1.ToString("00"), false);
        TheList[(object) key] = (object) null;
      }
      int num2 = -1;
      for (int index = 0; index < 12; ++index)
      {
        ++num2;
        WaveFlowDevice.ParameterNames key = (WaveFlowDevice.ParameterNames) Enum.Parse(typeof (WaveFlowDevice.ParameterNames), "IndexB" + num2.ToString("00"), false);
        TheList[(object) key] = (object) null;
      }
      for (int index1 = 0; index1 < numArray.Length; ++index1)
      {
        string str;
        switch (index1)
        {
          case 0:
            str = "A";
            break;
          case 1:
            str = "B";
            break;
          default:
            this.LastErrorString = "Wrong Index!";
            return false;
        }
        int num3 = -1;
        for (int index2 = 0; index2 < numArray[index1].Length; ++index2)
        {
          ++num3;
          WaveFlowDevice.ParameterNames key = (WaveFlowDevice.ParameterNames) Enum.Parse(typeof (WaveFlowDevice.ParameterNames), "Index" + str + num3.ToString("00"), false);
          TheList[(object) key] = (object) numArray[index1][index2];
        }
      }
      TheList[(object) WaveFlowDevice.ParameterNames.LoggerDate] = (object) dateTime;
      TheList[(object) WaveFlowDevice.ParameterNames.DatalogMeasurePeriodUnit] = (object) measurePeriod.Unit;
      TheList[(object) WaveFlowDevice.ParameterNames.DatalogMeasurePeriodCoefficient] = (object) measurePeriod.Coefficient;
      TheList[(object) WaveFlowDevice.ParameterNames.NbCounters] = (object) operationMode.NbCounters;
      TheList[(object) WaveFlowDevice.ParameterNames.LoggingMode] = (object) operationMode.LoggingMode;
      TheList[(object) WaveFlowDevice.ParameterNames.WireCutDetect] = (object) operationMode.WireCutDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.LowLeakDetect] = (object) operationMode.LowLeakDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.HighLeakDetect] = (object) operationMode.HighLeakDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.ReedDetect] = (object) operationMode.ReedDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.BackflowDetectionMethod] = (object) operationMode.BackflowDetection;
      TheList[(object) WaveFlowDevice.ParameterNames.BatteryLife] = (object) status.BatteryLife;
      TheList[(object) WaveFlowDevice.ParameterNames.WireCutA] = (object) status.WireCutA;
      TheList[(object) WaveFlowDevice.ParameterNames.WireCutB] = (object) status.WireCutB;
      TheList[(object) WaveFlowDevice.ParameterNames.LowLeak] = (object) status.LowLeak;
      TheList[(object) WaveFlowDevice.ParameterNames.HighLeak] = (object) status.HighLeak;
      TheList[(object) WaveFlowDevice.ParameterNames.ReedFaultAORWireCutC] = (object) status.ReedFaultAORWireCutC;
      TheList[(object) WaveFlowDevice.ParameterNames.ReedFaultBORWireCutD] = (object) status.ReedFaultBORWireCutD;
      TheList[(object) WaveFlowDevice.ParameterNames.ReverseLeak] = (object) status.ReverseLeak;
      return true;
    }

    private bool WF_GetExtendedDatalog(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      WaveFlow.OperationMode operationMode;
      WaveFlow.Status status;
      DateTime dateTime;
      int[] numArray1;
      int[] numArray2;
      WaveBase.MeasurePeriod measurePeriod;
      WaveCardRequest.Errors extendedDatalog = this.MyBus.MyWavePort.MyWaveFlow.GetExtendedDatalog(SerialNumber, this.GetConvertedRepeaters(), ref operationMode, ref status, ref dateTime, ref numArray1, ref numArray2, ref measurePeriod);
      if (extendedDatalog > 0)
      {
        this.LastErrorString = extendedDatalog.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      int num1 = -1;
      for (int index = 0; index < 12; ++index)
      {
        ++num1;
        WaveFlowDevice.ParameterNames key = (WaveFlowDevice.ParameterNames) Enum.Parse(typeof (WaveFlowDevice.ParameterNames), "IndexC" + num1.ToString("00"), false);
        if (index < numArray1.Length)
          TheList[(object) key] = (object) numArray1[index];
        else
          TheList[(object) key] = (object) null;
      }
      int num2 = -1;
      for (int index = 0; index < 12; ++index)
      {
        ++num2;
        WaveFlowDevice.ParameterNames key = (WaveFlowDevice.ParameterNames) Enum.Parse(typeof (WaveFlowDevice.ParameterNames), "IndexD" + num2.ToString("00"), false);
        if (index < numArray2.Length)
          TheList[(object) key] = (object) numArray2[index];
        else
          TheList[(object) key] = (object) null;
      }
      TheList[(object) WaveFlowDevice.ParameterNames.LoggerDate] = (object) dateTime;
      TheList[(object) WaveFlowDevice.ParameterNames.DatalogMeasurePeriodUnit] = (object) measurePeriod.Unit;
      TheList[(object) WaveFlowDevice.ParameterNames.DatalogMeasurePeriodCoefficient] = (object) measurePeriod.Coefficient;
      TheList[(object) WaveFlowDevice.ParameterNames.NbCounters] = (object) operationMode.NbCounters;
      TheList[(object) WaveFlowDevice.ParameterNames.LoggingMode] = (object) operationMode.LoggingMode;
      TheList[(object) WaveFlowDevice.ParameterNames.WireCutDetect] = (object) operationMode.WireCutDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.LowLeakDetect] = (object) operationMode.LowLeakDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.HighLeakDetect] = (object) operationMode.HighLeakDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.ReedDetect] = (object) operationMode.ReedDetect;
      TheList[(object) WaveFlowDevice.ParameterNames.BackflowDetectionMethod] = (object) operationMode.BackflowDetection;
      TheList[(object) WaveFlowDevice.ParameterNames.BatteryLife] = (object) status.BatteryLife;
      TheList[(object) WaveFlowDevice.ParameterNames.WireCutA] = (object) status.WireCutA;
      TheList[(object) WaveFlowDevice.ParameterNames.WireCutB] = (object) status.WireCutB;
      TheList[(object) WaveFlowDevice.ParameterNames.LowLeak] = (object) status.LowLeak;
      TheList[(object) WaveFlowDevice.ParameterNames.HighLeak] = (object) status.HighLeak;
      TheList[(object) WaveFlowDevice.ParameterNames.ReedFaultAORWireCutC] = (object) status.ReedFaultAORWireCutC;
      TheList[(object) WaveFlowDevice.ParameterNames.ReedFaultBORWireCutD] = (object) status.ReedFaultBORWireCutD;
      TheList[(object) WaveFlowDevice.ParameterNames.ReverseLeak] = (object) status.ReverseLeak;
      return true;
    }

    private bool WF_GetLowLeak(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      WaveFlow.LeakSettings[] leakSettingsArray;
      byte num;
      WaveCardRequest.Errors lowLeak = this.MyBus.MyWavePort.MyWaveFlow.GetLowLeak(SerialNumber, this.GetConvertedRepeaters(), ref leakSettingsArray, ref num);
      if (lowLeak > 0)
      {
        this.LastErrorString = lowLeak.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      TheList[(object) WaveFlowDevice.ParameterNames.LowLeakThresholdA] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LowLeakDurationA] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LowLeakThresholdB] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LowLeakDurationB] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LowLeakThresholdC] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LowLeakDurationC] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LowLeakThresholdD] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LowLeakDurationD] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.TimeBetweenMeasures] = (object) null;
      for (int index = 0; index < leakSettingsArray.Length; ++index)
      {
        switch (index)
        {
          case 0:
            TheList[(object) WaveFlowDevice.ParameterNames.LowLeakThresholdA] = (object) leakSettingsArray[index].Threshold;
            TheList[(object) WaveFlowDevice.ParameterNames.LowLeakDurationA] = (object) leakSettingsArray[index].Duration;
            break;
          case 1:
            TheList[(object) WaveFlowDevice.ParameterNames.LowLeakThresholdB] = (object) leakSettingsArray[index].Threshold;
            TheList[(object) WaveFlowDevice.ParameterNames.LowLeakDurationB] = (object) leakSettingsArray[index].Duration;
            break;
          case 2:
            TheList[(object) WaveFlowDevice.ParameterNames.LowLeakThresholdC] = (object) leakSettingsArray[index].Threshold;
            TheList[(object) WaveFlowDevice.ParameterNames.LowLeakDurationC] = (object) leakSettingsArray[index].Duration;
            break;
          case 3:
            TheList[(object) WaveFlowDevice.ParameterNames.LowLeakThresholdD] = (object) leakSettingsArray[index].Threshold;
            TheList[(object) WaveFlowDevice.ParameterNames.LowLeakDurationD] = (object) leakSettingsArray[index].Duration;
            break;
          default:
            this.LastErrorString = "Wrong Index!";
            return false;
        }
      }
      TheList[(object) WaveFlowDevice.ParameterNames.TimeBetweenMeasures] = (object) num;
      return true;
    }

    private bool WF_GetHighLeak(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      WaveFlow.LeakSettings[] leakSettingsArray;
      byte num;
      WaveCardRequest.Errors highLeak = this.MyBus.MyWavePort.MyWaveFlow.GetHighLeak(SerialNumber, this.GetConvertedRepeaters(), ref leakSettingsArray, ref num);
      if (highLeak > 0)
      {
        this.LastErrorString = highLeak.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      TheList[(object) WaveFlowDevice.ParameterNames.HighLeakThresholdA] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.HighLeakDurationA] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.HighLeakThresholdB] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.HighLeakDurationB] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.HighLeakThresholdC] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.HighLeakDurationC] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.HighLeakThresholdD] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.HighLeakDurationD] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.TimeBetweenMeasures] = (object) null;
      for (int index = 0; index < leakSettingsArray.Length; ++index)
      {
        switch (index)
        {
          case 0:
            TheList[(object) WaveFlowDevice.ParameterNames.HighLeakThresholdA] = (object) leakSettingsArray[index].Threshold;
            TheList[(object) WaveFlowDevice.ParameterNames.HighLeakDurationA] = (object) leakSettingsArray[index].Duration;
            break;
          case 1:
            TheList[(object) WaveFlowDevice.ParameterNames.HighLeakThresholdB] = (object) leakSettingsArray[index].Threshold;
            TheList[(object) WaveFlowDevice.ParameterNames.HighLeakDurationB] = (object) leakSettingsArray[index].Duration;
            break;
          case 2:
            TheList[(object) WaveFlowDevice.ParameterNames.HighLeakThresholdC] = (object) leakSettingsArray[index].Threshold;
            TheList[(object) WaveFlowDevice.ParameterNames.HighLeakDurationC] = (object) leakSettingsArray[index].Duration;
            break;
          case 3:
            TheList[(object) WaveFlowDevice.ParameterNames.HighLeakThresholdD] = (object) leakSettingsArray[index].Threshold;
            TheList[(object) WaveFlowDevice.ParameterNames.HighLeakDurationD] = (object) leakSettingsArray[index].Duration;
            break;
          default:
            this.LastErrorString = "Wrong Index!";
            return false;
        }
      }
      TheList[(object) WaveFlowDevice.ParameterNames.TimeBetweenMeasures] = (object) num;
      return true;
    }

    private bool WF_GetLeakHistory(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      WaveFlow.Leak[] leakArray;
      WaveCardRequest.Errors leakHistory = this.MyBus.MyWavePort.MyWaveFlow.GetLeakHistory(SerialNumber, this.GetConvertedRepeaters(), ref leakArray);
      if (leakHistory > 0)
      {
        this.LastErrorString = leakHistory.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryAppearance0] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryDisappearance0] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryHighLeak0] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryLowLeak0] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryDateTime0] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryInput0] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryFlow0] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryAppearance1] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryDisappearance1] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryHighLeak1] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryLowLeak1] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryDateTime1] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryInput1] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryFlow1] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryAppearance2] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryDisappearance2] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryHighLeak2] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryLowLeak2] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryDateTime2] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryInput2] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryFlow2] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryAppearance3] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryDisappearance3] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryHighLeak3] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryLowLeak3] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryDateTime3] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryInput3] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryFlow3] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryAppearance4] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryDisappearance4] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryHighLeak4] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryLowLeak4] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryDateTime4] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryInput4] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryFlow4] = (object) null;
      for (int index = 0; index < leakArray.Length; ++index)
      {
        switch (index)
        {
          case 0:
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryAppearance0] = (object) leakArray[0].Appearance;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryDisappearance0] = (object) leakArray[0].Disappearance;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryHighLeak0] = (object) leakArray[0].HighLeak;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryLowLeak0] = (object) leakArray[0].LowLeak;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryDateTime0] = (object) leakArray[0].DateTime;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryInput0] = (object) leakArray[0].Input;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryFlow0] = (object) leakArray[0].Flow;
            break;
          case 1:
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryAppearance1] = (object) leakArray[1].Appearance;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryDisappearance1] = (object) leakArray[1].Disappearance;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryHighLeak1] = (object) leakArray[1].HighLeak;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryLowLeak1] = (object) leakArray[1].LowLeak;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryDateTime1] = (object) leakArray[1].DateTime;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryInput1] = (object) leakArray[1].Input;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryFlow1] = (object) leakArray[1].Flow;
            break;
          case 2:
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryAppearance2] = (object) leakArray[2].Appearance;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryDisappearance2] = (object) leakArray[2].Disappearance;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryHighLeak2] = (object) leakArray[2].HighLeak;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryLowLeak2] = (object) leakArray[2].LowLeak;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryDateTime2] = (object) leakArray[2].DateTime;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryInput2] = (object) leakArray[2].Input;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryFlow2] = (object) leakArray[2].Flow;
            break;
          case 3:
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryAppearance3] = (object) leakArray[3].Appearance;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryDisappearance3] = (object) leakArray[3].Disappearance;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryHighLeak3] = (object) leakArray[3].HighLeak;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryLowLeak3] = (object) leakArray[3].LowLeak;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryDateTime3] = (object) leakArray[3].DateTime;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryInput3] = (object) leakArray[3].Input;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryFlow3] = (object) leakArray[3].Flow;
            break;
          case 4:
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryAppearance4] = (object) leakArray[4].Appearance;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryDisappearance4] = (object) leakArray[4].Disappearance;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryHighLeak4] = (object) leakArray[4].HighLeak;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryLowLeak4] = (object) leakArray[4].LowLeak;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryDateTime4] = (object) leakArray[4].DateTime;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryInput4] = (object) leakArray[4].Input;
            TheList[(object) WaveFlowDevice.ParameterNames.LeakHistoryFlow4] = (object) leakArray[4].Flow;
            break;
        }
      }
      return true;
    }

    private bool WF_GetBackFlow(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      WaveFlow.LeakSettings[] leakSettingsArray;
      WaveFlow.OperationMode.BackflowDetectionMethod backflowDetectionMethod;
      WaveCardRequest.Errors backFlow = this.MyBus.MyWavePort.MyWaveFlow.GetBackFlow(SerialNumber, this.GetConvertedRepeaters(), ref leakSettingsArray, ref backflowDetectionMethod);
      if (backFlow > 0)
      {
        this.LastErrorString = backFlow.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      for (int index = 0; index < leakSettingsArray.Length; ++index)
      {
        switch (index)
        {
          case 0:
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowDurationA] = (object) leakSettingsArray[index].Duration;
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowThresholdA] = (object) leakSettingsArray[index].Threshold;
            break;
          case 1:
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowDurationB] = (object) leakSettingsArray[index].Duration;
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowThresholdB] = (object) leakSettingsArray[index].Threshold;
            break;
        }
        TheList[(object) WaveFlowDevice.ParameterNames.BackflowDetectionMethod] = (object) backflowDetectionMethod;
      }
      return true;
    }

    private bool WF_GetSpecialBackFlow(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      WaveFlow.LeakSettings[] leakSettingsArray;
      ushort[] numArray;
      WaveCardRequest.Errors specialBackFlow = this.MyBus.MyWavePort.MyWaveFlow.GetSpecialBackFlow(SerialNumber, this.GetConvertedRepeaters(), ref leakSettingsArray, ref numArray);
      if (specialBackFlow > 0)
      {
        this.LastErrorString = specialBackFlow.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      for (int index = 0; index < leakSettingsArray.Length; ++index)
      {
        switch (index)
        {
          case 0:
            TheList[(object) WaveFlowDevice.ParameterNames.SpecialBackFlowDurationA] = (object) leakSettingsArray[index].Duration;
            TheList[(object) WaveFlowDevice.ParameterNames.SpecialBackFlowThresholdA] = (object) leakSettingsArray[index].Threshold;
            break;
          case 1:
            TheList[(object) WaveFlowDevice.ParameterNames.SpecialBackFlowDurationB] = (object) leakSettingsArray[index].Duration;
            TheList[(object) WaveFlowDevice.ParameterNames.SpecialBackFlowThresholdB] = (object) leakSettingsArray[index].Threshold;
            break;
        }
      }
      for (int index = 0; index < numArray.Length; ++index)
      {
        switch (index)
        {
          case 0:
            TheList[(object) WaveFlowDevice.ParameterNames.MonthlyFlagsOnReverseFlowA] = (object) numArray[index];
            break;
          case 1:
            TheList[(object) WaveFlowDevice.ParameterNames.MonthlyFlagsOnReverseFlowB] = (object) numArray[index];
            break;
        }
      }
      return true;
    }

    private bool WF_GetBackflowHistory(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      WaveFlow.BackFlow[] backFlowArray;
      WaveCardRequest.Errors backFlowHistory = this.MyBus.MyWavePort.MyWaveFlow.GetBackFlowHistory(SerialNumber, this.GetConvertedRepeaters(), ref backFlowArray);
      if (backFlowHistory > 0)
      {
        this.LastErrorString = backFlowHistory.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryBackFlowPresenceDuration0] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDateTime0] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDateTimeEnded0] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDetectionDuration0] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryFlow0] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryInput0] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryBackFlowPresenceDuration1] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDateTime1] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDateTimeEnded1] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDetectionDuration1] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryFlow1] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryInput1] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryBackFlowPresenceDuration3] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDateTime3] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDateTimeEnded3] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDetectionDuration3] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryFlow3] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryInput3] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryBackFlowPresenceDuration3] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDateTime3] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDateTimeEnded3] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDetectionDuration3] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryFlow3] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryInput3] = (object) null;
      for (int index = 0; index < backFlowArray.Length; ++index)
      {
        switch (index)
        {
          case 0:
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryBackFlowPresenceDuration0] = (object) backFlowArray[0].BackFlowPresenceDuration;
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDateTime0] = (object) backFlowArray[0].DateTime;
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDateTimeEnded0] = (object) backFlowArray[0].DateTimeEnded;
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDetectionDuration0] = (object) backFlowArray[0].DetectionDuration;
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryFlow0] = (object) backFlowArray[0].Flow;
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryInput0] = (object) backFlowArray[0].Input;
            break;
          case 1:
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryBackFlowPresenceDuration1] = (object) backFlowArray[1].BackFlowPresenceDuration;
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDateTime1] = (object) backFlowArray[1].DateTime;
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDateTimeEnded1] = (object) backFlowArray[1].DateTimeEnded;
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDetectionDuration1] = (object) backFlowArray[1].DetectionDuration;
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryFlow1] = (object) backFlowArray[1].Flow;
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryInput1] = (object) backFlowArray[1].Input;
            break;
          case 2:
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryBackFlowPresenceDuration2] = (object) backFlowArray[2].BackFlowPresenceDuration;
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDateTime2] = (object) backFlowArray[2].DateTime;
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDateTimeEnded2] = (object) backFlowArray[2].DateTimeEnded;
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDetectionDuration2] = (object) backFlowArray[2].DetectionDuration;
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryFlow2] = (object) backFlowArray[2].Flow;
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryInput2] = (object) backFlowArray[2].Input;
            break;
          case 3:
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryBackFlowPresenceDuration3] = (object) backFlowArray[3].BackFlowPresenceDuration;
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDateTime3] = (object) backFlowArray[3].DateTime;
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDateTimeEnded3] = (object) backFlowArray[3].DateTimeEnded;
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryDetectionDuration3] = (object) backFlowArray[3].DetectionDuration;
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryFlow3] = (object) backFlowArray[3].Flow;
            TheList[(object) WaveFlowDevice.ParameterNames.BackFlowHistoryInput3] = (object) backFlowArray[3].Input;
            break;
        }
      }
      return true;
    }

    private bool WF_GetAlarmConfiguration(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      WaveFlow.AlarmConfiguration alarmConfiguration1;
      string str;
      string[] strArray;
      WaveCardRequest.Errors alarmConfiguration2 = this.MyBus.MyWavePort.MyWaveFlow.GetAlarmConfiguration(SerialNumber, this.GetConvertedRepeaters(), ref alarmConfiguration1, ref str, ref strArray);
      if (alarmConfiguration2 > 0)
      {
        this.LastErrorString = alarmConfiguration2.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      TheList[(object) WaveFlowDevice.ParameterNames.AlarmWireCutAndReedFault] = (object) ((alarmConfiguration1 & 1) == 1);
      TheList[(object) WaveFlowDevice.ParameterNames.AlarmBatteryLife] = (object) ((alarmConfiguration1 & 2) == 2);
      TheList[(object) WaveFlowDevice.ParameterNames.AlarmLowLeak] = (object) ((alarmConfiguration1 & 4) == 4);
      TheList[(object) WaveFlowDevice.ParameterNames.AlarmHighLeak] = (object) ((alarmConfiguration1 & 8) == 8);
      TheList[(object) WaveFlowDevice.ParameterNames.AlarmReverseLeak] = (object) ((alarmConfiguration1 & 16) == 16);
      if (strArray.Length != 0)
        TheList[(object) WaveFlowDevice.ParameterNames.AlarmRouter1] = (object) strArray[0];
      else
        TheList[(object) WaveFlowDevice.ParameterNames.AlarmRouter1] = (object) null;
      if (strArray.Length > 2)
        TheList[(object) WaveFlowDevice.ParameterNames.AlarmRouter2] = (object) strArray[1];
      else
        TheList[(object) WaveFlowDevice.ParameterNames.AlarmRouter2] = (object) null;
      if (strArray.Length > 2)
        TheList[(object) WaveFlowDevice.ParameterNames.AlarmRouter3] = (object) strArray[2];
      else
        TheList[(object) WaveFlowDevice.ParameterNames.AlarmRouter3] = (object) null;
      TheList[(object) WaveFlowDevice.ParameterNames.AlarmDestination] = (object) str;
      return true;
    }

    private bool WF_GetFactoryConfiguration(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      WaveFlow.FactoryConfiguration factoryConfiguration1;
      WaveCardRequest.Errors factoryConfiguration2 = this.MyBus.MyWavePort.MyWaveFlow.GetFactoryConfiguration(SerialNumber, this.GetConvertedRepeaters(), ref factoryConfiguration1);
      if (factoryConfiguration2 > 0)
      {
        this.LastErrorString = factoryConfiguration2.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      TheList[(object) WaveFlowDevice.ParameterNames.BackflowDetectionType] = (object) factoryConfiguration1.BackFlowDetectionType;
      TheList[(object) WaveFlowDevice.ParameterNames.IsFourInputs] = (object) factoryConfiguration1.IsFourInputs;
      TheList[(object) WaveFlowDevice.ParameterNames.SensorType] = (object) factoryConfiguration1.SensorType;
      TheList[(object) WaveFlowDevice.ParameterNames.Valve] = (object) factoryConfiguration1.Valve;
      return true;
    }

    private bool WF_GetSolenoidState(string SerialNumber, ref SortedList TheList)
    {
      TheList.Clear();
      WaveFlow.SolenoidState solenoidState1;
      WaveCardRequest.Errors solenoidState2 = this.MyBus.MyWavePort.MyWaveFlow.GetSolenoidState(SerialNumber, this.GetConvertedRepeaters(), ref solenoidState1);
      if (solenoidState2 > 0)
      {
        this.LastErrorString = solenoidState2.ToString();
        TheList[(object) WaveFlowDevice.ParameterNames.Error] = (object) this.LastErrorString;
        return false;
      }
      TheList[(object) WaveFlowDevice.ParameterNames.SolenoidState] = (object) solenoidState1;
      return true;
    }

    private bool WF_SetDate(string SerialNumber, object ParameterData)
    {
      SortedList sortedList = (SortedList) ParameterData;
      try
      {
        DateTime dateTime = (DateTime) sortedList[(object) WaveFlowDevice.ParameterNames.Date];
        WaveCardRequest.Errors errors = ((WaveBase) this.MyBus.MyWavePort.MyWaveFlow).SetDate(SerialNumber, this.GetConvertedRepeaters(), dateTime);
        if (errors <= 0)
          return true;
        this.LastErrorString = errors.ToString();
        return false;
      }
      catch (Exception ex)
      {
        this.LastErrorString = ex.ToString();
        return false;
      }
    }

    private bool WF_SetGroupNumber(string SerialNumber, object ParameterData)
    {
      try
      {
        byte num = (byte) ((SortedList) ParameterData)[(object) WaveFlowDevice.ParameterNames.GroupNumberForPolls];
        WaveCardRequest.Errors errors = this.MyBus.MyWavePort.MyWaveFlow.SetGroupNumber(SerialNumber, this.GetConvertedRepeaters(), num);
        if (errors <= 0)
          return true;
        this.LastErrorString = errors.ToString();
        return false;
      }
      catch (Exception ex)
      {
        this.LastErrorString = ex.ToString();
        return false;
      }
    }

    private bool WF_SetOperationModes(string SerialNumber, object ParameterData)
    {
      SortedList sortedList = (SortedList) ParameterData;
      WaveFlow.OperationMode operationMode = new WaveFlow.OperationMode((byte) 0);
      try
      {
        operationMode.NbCounters = (int) sortedList[(object) WaveFlowDevice.ParameterNames.NbCounters];
        operationMode.LoggingMode = (WaveFlow.OperationMode.DatalogMode) sortedList[(object) WaveFlowDevice.ParameterNames.LoggingMode];
        operationMode.WireCutDetect = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.WireCutDetect];
        operationMode.LowLeakDetect = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.LowLeakDetect];
        operationMode.HighLeakDetect = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.HighLeakDetect];
        operationMode.ReedDetect = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.ReedDetect];
        operationMode.BackflowDetection = (WaveFlow.OperationMode.BackflowDetectionMethod) sortedList[(object) WaveFlowDevice.ParameterNames.BackflowDetectionMethod];
        WaveCardRequest.Errors errors = this.MyBus.MyWavePort.MyWaveFlow.SetOperationMode(SerialNumber, this.GetConvertedRepeaters(), operationMode);
        if (errors <= 0)
          return true;
        this.LastErrorString = errors.ToString();
        return false;
      }
      catch (Exception ex)
      {
        this.LastErrorString = ex.ToString();
        return false;
      }
    }

    private bool WF_SetStatus(string SerialNumber, object ParameterData)
    {
      SortedList sortedList = (SortedList) ParameterData;
      WaveFlow.Status status = new WaveFlow.Status((byte) 0);
      try
      {
        status.BatteryLife = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.BatteryLife];
        status.HighLeak = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.HighLeak];
        status.LowLeak = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.LowLeak];
        status.ReedFaultAORWireCutC = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.ReedFaultAORWireCutC];
        status.ReedFaultBORWireCutD = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.ReedFaultBORWireCutD];
        status.ReverseLeak = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.ReverseLeak];
        status.WireCutA = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.WireCutA];
        status.WireCutB = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.WireCutB];
        WaveCardRequest.Errors errors = this.MyBus.MyWavePort.MyWaveFlow.SetStatus(SerialNumber, this.GetConvertedRepeaters(), status);
        if (errors <= 0)
          return true;
        this.LastErrorString = errors.ToString();
        return false;
      }
      catch (Exception ex)
      {
        this.LastErrorString = ex.ToString();
        return false;
      }
    }

    private bool WF_SetPulseWeights(string SerialNumber, object ParameterData)
    {
      SortedList sortedList = (SortedList) ParameterData;
      int length = 0;
      try
      {
        if (sortedList[(object) WaveFlowDevice.ParameterNames.PulseValueA] != null && sortedList[(object) WaveFlowDevice.ParameterNames.PulseUnitA] != null)
          ++length;
        if (sortedList[(object) WaveFlowDevice.ParameterNames.PulseValueB] != null && sortedList[(object) WaveFlowDevice.ParameterNames.PulseUnitB] != null)
          ++length;
        if (sortedList[(object) WaveFlowDevice.ParameterNames.PulseValueC] != null && sortedList[(object) WaveFlowDevice.ParameterNames.PulseUnitC] != null)
          ++length;
        if (sortedList[(object) WaveFlowDevice.ParameterNames.PulseValueD] != null && sortedList[(object) WaveFlowDevice.ParameterNames.PulseUnitD] != null)
          ++length;
        WaveFlow.PulseWeight[] pulseWeightArray = new WaveFlow.PulseWeight[length];
        if (length > 0)
          pulseWeightArray[0] = new WaveFlow.PulseWeight((byte) sortedList[(object) WaveFlowDevice.ParameterNames.PulseValueA], (WaveFlow.PulseWeight.PulseUnit) sortedList[(object) WaveFlowDevice.ParameterNames.PulseUnitA]);
        if (length > 1)
          pulseWeightArray[1] = new WaveFlow.PulseWeight((byte) sortedList[(object) WaveFlowDevice.ParameterNames.PulseValueB], (WaveFlow.PulseWeight.PulseUnit) sortedList[(object) WaveFlowDevice.ParameterNames.PulseUnitB]);
        if (length > 2)
          pulseWeightArray[2] = new WaveFlow.PulseWeight((byte) sortedList[(object) WaveFlowDevice.ParameterNames.PulseValueC], (WaveFlow.PulseWeight.PulseUnit) sortedList[(object) WaveFlowDevice.ParameterNames.PulseUnitC]);
        if (length > 3)
          pulseWeightArray[3] = new WaveFlow.PulseWeight((byte) sortedList[(object) WaveFlowDevice.ParameterNames.PulseValueD], (WaveFlow.PulseWeight.PulseUnit) sortedList[(object) WaveFlowDevice.ParameterNames.PulseUnitD]);
        WaveCardRequest.Errors errors = this.MyBus.MyWavePort.MyWaveFlow.SetPulseWeights(SerialNumber, this.GetConvertedRepeaters(), pulseWeightArray);
        if (errors <= 0)
          return true;
        this.LastErrorString = errors.ToString();
        return false;
      }
      catch (Exception ex)
      {
        this.LastErrorString = ex.ToString();
        return false;
      }
    }

    private bool WF_SetIndices(string SerialNumber, object ParameterData)
    {
      SortedList sortedList = (SortedList) ParameterData;
      int[] numArray = new int[4];
      bool[] flagArray = new bool[4];
      try
      {
        if (sortedList[(object) WaveFlowDevice.ParameterNames.IndexA] == null)
        {
          numArray[0] = 0;
          flagArray[0] = false;
        }
        else
        {
          numArray[0] = (int) sortedList[(object) WaveFlowDevice.ParameterNames.IndexA];
          flagArray[0] = true;
        }
        if (sortedList[(object) WaveFlowDevice.ParameterNames.IndexB] == null)
        {
          numArray[1] = 0;
          flagArray[1] = false;
        }
        else
        {
          numArray[1] = (int) sortedList[(object) WaveFlowDevice.ParameterNames.IndexB];
          flagArray[1] = true;
        }
        if (sortedList[(object) WaveFlowDevice.ParameterNames.IndexC] == null)
        {
          numArray[2] = 0;
          flagArray[2] = false;
        }
        else
        {
          numArray[2] = (int) sortedList[(object) WaveFlowDevice.ParameterNames.IndexC];
          flagArray[2] = true;
        }
        if (sortedList[(object) WaveFlowDevice.ParameterNames.IndexD] == null)
        {
          numArray[3] = 0;
          flagArray[3] = false;
        }
        else
        {
          numArray[3] = (int) sortedList[(object) WaveFlowDevice.ParameterNames.IndexD];
          flagArray[3] = true;
        }
        WaveCardRequest.Errors errors = this.MyBus.MyWavePort.MyWaveFlow.SetIndex(SerialNumber, this.GetConvertedRepeaters(), numArray, flagArray);
        if (errors <= 0)
          return true;
        this.LastErrorString = errors.ToString();
        return false;
      }
      catch (Exception ex)
      {
        this.LastErrorString = ex.ToString();
        return false;
      }
    }

    private bool WF_SetWakeUpMode(string SerialNumber, object ParameterData)
    {
      SortedList sortedList = (SortedList) ParameterData;
      try
      {
        WakeUpMode wakeUpMode = (WakeUpMode) sortedList[(object) WaveFlowDevice.ParameterNames.WakeUpMode];
        byte num1 = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.DefaultWakeupDuration];
        byte num2 = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.HourFirstTimeFrameStarts];
        byte num3 = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.FirstTimeFrameWakeupDuration];
        byte num4 = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.HourSecondTimeFrameStarts];
        byte num5 = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.SecondTimeFrameWakeupDuration];
        bool flag1 = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.TimeFramesEnabledOnMonday];
        bool flag2 = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.TimeFramesEnabledOnTuesday];
        bool flag3 = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.TimeFramesEnabledOnWednesday];
        bool flag4 = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.TimeFramesEnabledOnThursday];
        bool flag5 = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.TimeFramesEnabledOnFriday];
        bool flag6 = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.TimeFramesEnabledOnSaturday];
        bool flag7 = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.TimeFramesEnabledOnSunday];
        bool flag8 = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.WakeupDisabledOnMonday];
        bool flag9 = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.WakeupDisabledOnTuesday];
        bool flag10 = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.WakeupDisabledOnWednesday];
        bool flag11 = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.WakeupDisabledOnThursday];
        bool flag12 = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.WakeupDisabledOnFriday];
        bool flag13 = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.WakeupDisabledOnSaturday];
        bool flag14 = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.WakeupDisabledOnSunday];
        DayOfWeek dayOfWeek1 = (DayOfWeek) 0;
        if (flag1)
          dayOfWeek1 = (DayOfWeek) (dayOfWeek1 | 1);
        if (flag2)
          dayOfWeek1 = (DayOfWeek) (dayOfWeek1 | 2);
        if (flag3)
          dayOfWeek1 = (DayOfWeek) (dayOfWeek1 | 4);
        if (flag4)
          dayOfWeek1 = (DayOfWeek) (dayOfWeek1 | 8);
        if (flag5)
          dayOfWeek1 = (DayOfWeek) (dayOfWeek1 | 16);
        if (flag6)
          dayOfWeek1 = (DayOfWeek) (dayOfWeek1 | 32);
        if (flag7)
          dayOfWeek1 = (DayOfWeek) (dayOfWeek1 | 64);
        DayOfWeek dayOfWeek2 = (DayOfWeek) 0;
        if (flag8)
          dayOfWeek2 = (DayOfWeek) (dayOfWeek2 | 1);
        if (flag9)
          dayOfWeek2 = (DayOfWeek) (dayOfWeek2 | 2);
        if (flag10)
          dayOfWeek2 = (DayOfWeek) (dayOfWeek2 | 4);
        if (flag11)
          dayOfWeek2 = (DayOfWeek) (dayOfWeek2 | 8);
        if (flag12)
          dayOfWeek2 = (DayOfWeek) (dayOfWeek2 | 16);
        if (flag13)
          dayOfWeek2 = (DayOfWeek) (dayOfWeek2 | 32);
        if (flag14)
          dayOfWeek2 = (DayOfWeek) (dayOfWeek2 | 64);
        WaveCardRequest.Errors errors = ((WaveBase) this.MyBus.MyWavePort.MyWaveFlow).SetWakeUpMode(SerialNumber, this.GetConvertedRepeaters(), wakeUpMode, num1, num2, num3, num4, num5, dayOfWeek1, dayOfWeek2);
        if (errors <= 0)
          return true;
        this.LastErrorString = errors.ToString();
        return false;
      }
      catch (Exception ex)
      {
        this.LastErrorString = ex.ToString();
        return false;
      }
    }

    private bool WF_SetDatalogPeriod(string SerialNumber, object ParameterData)
    {
      SortedList sortedList = (SortedList) ParameterData;
      bool[] flagArray = new bool[2];
      try
      {
        byte num = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.DatalogStart];
        WaveCardRequest.Errors errors = this.MyBus.MyWavePort.MyWaveFlow.SetDatalogPeriod(SerialNumber, this.GetConvertedRepeaters(), num, new WaveBase.MeasurePeriod((byte) 0)
        {
          Coefficient = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.DatalogMeasurePeriodCoefficient],
          Unit = (WaveBase.MeasurePeriod.Units) sortedList[(object) WaveFlowDevice.ParameterNames.DatalogMeasurePeriodUnit]
        });
        if (errors <= 0)
          return true;
        this.LastErrorString = errors.ToString();
        return false;
      }
      catch (Exception ex)
      {
        this.LastErrorString = ex.ToString();
        return false;
      }
    }

    private bool WF_SetWeeklyDatalog(string SerialNumber, object ParameterData)
    {
      SortedList sortedList = (SortedList) ParameterData;
      try
      {
        byte num = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.TimeOfDatalogIf1PerMonthOrWeek];
        DayOfWeek dayOfWeek = (DayOfWeek) (byte) sortedList[(object) WaveFlowDevice.ParameterNames.DayOfWeekOrMonth];
        WaveCardRequest.Errors errors = this.MyBus.MyWavePort.MyWaveFlow.SetWeeklyDatalog(SerialNumber, this.GetConvertedRepeaters(), dayOfWeek, num);
        if (errors <= 0)
          return true;
        this.LastErrorString = errors.ToString();
        return false;
      }
      catch (Exception ex)
      {
        this.LastErrorString = ex.ToString();
        return false;
      }
    }

    private bool WF_SetMonthlyDatalog(string SerialNumber, object ParameterData)
    {
      SortedList sortedList = (SortedList) ParameterData;
      try
      {
        byte num1 = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.TimeOfDatalogIf1PerMonthOrWeek];
        byte num2 = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.DayOfWeekOrMonth];
        WaveCardRequest.Errors errors = this.MyBus.MyWavePort.MyWaveFlow.SetMonthlyDatalog(SerialNumber, this.GetConvertedRepeaters(), num2, num1);
        if (errors <= 0)
          return true;
        this.LastErrorString = errors.ToString();
        return false;
      }
      catch (Exception ex)
      {
        this.LastErrorString = ex.ToString();
        return false;
      }
    }

    private bool WF_SetLowLeak(string SerialNumber, object ParameterData)
    {
      SortedList sortedList = (SortedList) ParameterData;
      int length = 0;
      try
      {
        if (sortedList[(object) WaveFlowDevice.ParameterNames.LowLeakThresholdA] != null)
          ++length;
        if (sortedList[(object) WaveFlowDevice.ParameterNames.LowLeakThresholdB] != null)
          ++length;
        if (sortedList[(object) WaveFlowDevice.ParameterNames.LowLeakThresholdC] != null)
          ++length;
        if (sortedList[(object) WaveFlowDevice.ParameterNames.LowLeakThresholdD] != null)
          ++length;
        WaveFlow.LeakSettings[] leakSettingsArray = new WaveFlow.LeakSettings[length];
        for (int index = 0; index < length; ++index)
        {
          switch (index)
          {
            case 0:
              ushort num1 = (ushort) sortedList[(object) WaveFlowDevice.ParameterNames.LowLeakThresholdA];
              byte num2 = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.LowLeakDurationA];
              leakSettingsArray[index] = new WaveFlow.LeakSettings(num1, num2);
              break;
            case 1:
              ushort num3 = (ushort) sortedList[(object) WaveFlowDevice.ParameterNames.LowLeakThresholdB];
              byte num4 = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.LowLeakDurationB];
              leakSettingsArray[index] = new WaveFlow.LeakSettings(num3, num4);
              break;
            case 2:
              ushort num5 = (ushort) sortedList[(object) WaveFlowDevice.ParameterNames.LowLeakThresholdC];
              byte num6 = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.LowLeakDurationC];
              leakSettingsArray[index] = new WaveFlow.LeakSettings(num5, num6);
              break;
            case 3:
              ushort num7 = (ushort) sortedList[(object) WaveFlowDevice.ParameterNames.LowLeakThresholdD];
              byte num8 = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.LowLeakDurationD];
              leakSettingsArray[index] = new WaveFlow.LeakSettings(num7, num8);
              break;
          }
        }
        byte num = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.TimeBetweenMeasures];
        WaveCardRequest.Errors errors = this.MyBus.MyWavePort.MyWaveFlow.SetLowLeak(SerialNumber, this.GetConvertedRepeaters(), leakSettingsArray, num);
        if (errors <= 0)
          return true;
        this.LastErrorString = errors.ToString();
        return false;
      }
      catch (Exception ex)
      {
        this.LastErrorString = ex.ToString();
        return false;
      }
    }

    private bool WF_SetHighLeak(string SerialNumber, object ParameterData)
    {
      SortedList sortedList = (SortedList) ParameterData;
      int length = 0;
      try
      {
        if (sortedList[(object) WaveFlowDevice.ParameterNames.HighLeakThresholdA] != null)
          ++length;
        if (sortedList[(object) WaveFlowDevice.ParameterNames.HighLeakThresholdB] != null)
          ++length;
        if (sortedList[(object) WaveFlowDevice.ParameterNames.HighLeakThresholdC] != null)
          ++length;
        if (sortedList[(object) WaveFlowDevice.ParameterNames.HighLeakThresholdD] != null)
          ++length;
        WaveFlow.LeakSettings[] leakSettingsArray = new WaveFlow.LeakSettings[length];
        for (int index = 0; index < length; ++index)
        {
          switch (index)
          {
            case 0:
              ushort num1 = (ushort) sortedList[(object) WaveFlowDevice.ParameterNames.HighLeakThresholdA];
              byte num2 = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.HighLeakDurationA];
              leakSettingsArray[index] = new WaveFlow.LeakSettings(num1, num2);
              break;
            case 1:
              ushort num3 = (ushort) sortedList[(object) WaveFlowDevice.ParameterNames.HighLeakThresholdB];
              byte num4 = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.HighLeakDurationB];
              leakSettingsArray[index] = new WaveFlow.LeakSettings(num3, num4);
              break;
            case 2:
              ushort num5 = (ushort) sortedList[(object) WaveFlowDevice.ParameterNames.HighLeakThresholdC];
              byte num6 = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.HighLeakDurationC];
              leakSettingsArray[index] = new WaveFlow.LeakSettings(num5, num6);
              break;
            case 3:
              ushort num7 = (ushort) sortedList[(object) WaveFlowDevice.ParameterNames.HighLeakThresholdD];
              byte num8 = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.HighLeakDurationD];
              leakSettingsArray[index] = new WaveFlow.LeakSettings(num7, num8);
              break;
          }
        }
        byte num = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.TimeBetweenMeasures];
        WaveCardRequest.Errors errors = this.MyBus.MyWavePort.MyWaveFlow.SetHighLeak(SerialNumber, this.GetConvertedRepeaters(), leakSettingsArray, num);
        if (errors <= 0)
          return true;
        this.LastErrorString = errors.ToString();
        return false;
      }
      catch (Exception ex)
      {
        this.LastErrorString = ex.ToString();
        return false;
      }
    }

    private bool WF_SetBackFlow(string SerialNumber, object ParameterData)
    {
      SortedList sortedList = (SortedList) ParameterData;
      try
      {
        WaveFlow.LeakSettings[] leakSettingsArray = new WaveFlow.LeakSettings[2];
        ushort num1 = (ushort) sortedList[(object) WaveFlowDevice.ParameterNames.BackFlowThresholdA];
        byte num2 = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.BackFlowDurationA];
        leakSettingsArray[0] = new WaveFlow.LeakSettings(num1, num2);
        ushort num3 = (ushort) sortedList[(object) WaveFlowDevice.ParameterNames.BackFlowThresholdB];
        byte num4 = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.BackFlowDurationB];
        leakSettingsArray[1] = new WaveFlow.LeakSettings(num3, num4);
        WaveFlow.OperationMode.BackflowDetectionMethod backflowDetectionMethod = (WaveFlow.OperationMode.BackflowDetectionMethod) sortedList[(object) WaveFlowDevice.ParameterNames.BackflowDetectionMethod];
        WaveCardRequest.Errors errors = this.MyBus.MyWavePort.MyWaveFlow.SetBackFlow(SerialNumber, this.GetConvertedRepeaters(), leakSettingsArray, backflowDetectionMethod);
        if (errors <= 0)
          return true;
        this.LastErrorString = errors.ToString();
        return false;
      }
      catch (Exception ex)
      {
        this.LastErrorString = ex.ToString();
        return false;
      }
    }

    private bool WF_SetSpecialBackFlow(string SerialNumber, object ParameterData)
    {
      SortedList sortedList = (SortedList) ParameterData;
      try
      {
        WaveFlow.LeakSettings[] leakSettingsArray = new WaveFlow.LeakSettings[2];
        ushort[] numArray = new ushort[2];
        ushort num1 = (ushort) sortedList[(object) WaveFlowDevice.ParameterNames.SpecialBackFlowThresholdA];
        byte num2 = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.SpecialBackFlowDurationA];
        leakSettingsArray[0] = new WaveFlow.LeakSettings(num1, num2);
        ushort num3 = (ushort) sortedList[(object) WaveFlowDevice.ParameterNames.SpecialBackFlowThresholdB];
        byte num4 = (byte) sortedList[(object) WaveFlowDevice.ParameterNames.SpecialBackFlowDurationB];
        leakSettingsArray[1] = new WaveFlow.LeakSettings(num3, num4);
        numArray[0] = (ushort) sortedList[(object) WaveFlowDevice.ParameterNames.MonthlyFlagsOnReverseFlowA];
        numArray[1] = (ushort) sortedList[(object) WaveFlowDevice.ParameterNames.MonthlyFlagsOnReverseFlowB];
        WaveCardRequest.Errors errors = this.MyBus.MyWavePort.MyWaveFlow.SetSpecialBackFlow(SerialNumber, this.GetConvertedRepeaters(), leakSettingsArray, numArray);
        if (errors <= 0)
          return true;
        this.LastErrorString = errors.ToString();
        return false;
      }
      catch (Exception ex)
      {
        this.LastErrorString = ex.ToString();
        return false;
      }
    }

    private bool WF_SetAlarmConfiguration(string SerialNumber, object ParameterData)
    {
      SortedList sortedList = (SortedList) ParameterData;
      WaveFlow.Status status = new WaveFlow.Status((byte) 0);
      try
      {
        bool flag1 = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.AlarmWireCutAndReedFault];
        bool flag2 = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.AlarmBatteryLife];
        bool flag3 = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.AlarmLowLeak];
        bool flag4 = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.AlarmHighLeak];
        bool flag5 = (bool) sortedList[(object) WaveFlowDevice.ParameterNames.AlarmReverseLeak];
        WaveFlow.AlarmConfiguration alarmConfiguration = (WaveFlow.AlarmConfiguration) 0;
        if (flag1)
          alarmConfiguration = (WaveFlow.AlarmConfiguration) (alarmConfiguration | 1);
        if (flag2)
          alarmConfiguration = (WaveFlow.AlarmConfiguration) (alarmConfiguration | 2);
        if (flag3)
          alarmConfiguration = (WaveFlow.AlarmConfiguration) (alarmConfiguration | 4);
        if (flag4)
          alarmConfiguration = (WaveFlow.AlarmConfiguration) (alarmConfiguration | 8);
        if (flag5)
          alarmConfiguration = (WaveFlow.AlarmConfiguration) (alarmConfiguration | 16);
        string str1 = (string) sortedList[(object) WaveFlowDevice.ParameterNames.AlarmDestination];
        string str2 = ((string) sortedList[(object) WaveFlowDevice.ParameterNames.AlarmRouter1]).Trim();
        string str3 = ((string) sortedList[(object) WaveFlowDevice.ParameterNames.AlarmRouter2]).Trim();
        string str4 = ((string) sortedList[(object) WaveFlowDevice.ParameterNames.AlarmRouter3]).Trim();
        int length = 0;
        if (str2 != string.Empty)
        {
          ++length;
          if (str3 != string.Empty)
          {
            ++length;
            if (str4 != string.Empty)
              ++length;
          }
        }
        string[] strArray = new string[length];
        if (length > 0)
        {
          strArray[0] = str2;
          if (length > 1)
          {
            strArray[1] = str3;
            if (length > 2)
              strArray[2] = str4;
          }
        }
        WaveCardRequest.Errors errors = this.MyBus.MyWavePort.MyWaveFlow.SetAlarmConfiguration(SerialNumber, this.GetConvertedRepeaters(), alarmConfiguration, str1, strArray);
        if (errors <= 0)
          return true;
        this.LastErrorString = errors.ToString();
        return false;
      }
      catch (Exception ex)
      {
        this.LastErrorString = ex.ToString();
        return false;
      }
    }

    private bool WF_SetSolenoidState(string SerialNumber, object ParameterData)
    {
      SortedList sortedList = (SortedList) ParameterData;
      try
      {
        WaveFlow.SolenoidState solenoidState = (WaveFlow.SolenoidState) sortedList[(object) WaveFlowDevice.ParameterNames.SolenoidState];
        WaveCardRequest.Errors errors = this.MyBus.MyWavePort.MyWaveFlow.SetSolenoidState(SerialNumber, this.GetConvertedRepeaters(), solenoidState);
        if (errors <= 0)
          return true;
        this.LastErrorString = errors.ToString();
        return false;
      }
      catch (Exception ex)
      {
        this.LastErrorString = ex.ToString();
        return false;
      }
    }

    private bool WF_ResetDateForEndOfBatteryLifeDetection(string SerialNumber)
    {
      WaveCardRequest.Errors errors = this.MyBus.MyWavePort.MyWaveFlow.ResetDateForEndOfBatteryLifeDetection(SerialNumber, this.GetConvertedRepeaters());
      if (errors <= 0)
        return true;
      this.LastErrorString = errors.ToString();
      return false;
    }

    private bool WF_ResetDateForReedFailure(string SerialNumber)
    {
      WaveCardRequest.Errors errors = this.MyBus.MyWavePort.MyWaveFlow.ResetDateForReedFailure(SerialNumber, this.GetConvertedRepeaters());
      if (errors <= 0)
        return true;
      this.LastErrorString = errors.ToString();
      return false;
    }

    private bool WF_ResetDateForeWireCutDetection(string SerialNumber)
    {
      WaveCardRequest.Errors errors = this.MyBus.MyWavePort.MyWaveFlow.ResetDateForWireCutDetection(SerialNumber, this.GetConvertedRepeaters());
      if (errors <= 0)
        return true;
      this.LastErrorString = errors.ToString();
      return false;
    }

    private bool GetQMIndexString(
      int Index,
      WaveFlow.PulseWeight.PulseUnit Unit,
      byte Value,
      out string QmIndexString)
    {
      QmIndexString = string.Empty;
      try
      {
        double num = (double) Index * (double) Value * Math.Pow(10.0, (double) Unit) / 1000000.0;
        QmIndexString = num.ToString((IFormatProvider) FixedFormates.TheFormates.NumberFormat);
        return true;
      }
      catch (Exception ex)
      {
        this.LastErrorString = ex.ToString();
        return false;
      }
    }

    private string GetConvertedSN(string TheSerialNumber)
    {
      try
      {
        long num1 = long.Parse(TheSerialNumber.Substring(0, 5));
        long num2 = long.Parse(TheSerialNumber.Substring(5, 2));
        long num3 = long.Parse(TheSerialNumber.Substring(7, 8));
        return num1.ToString("x4") + num2.ToString("x2") + num3.ToString("x6");
      }
      catch
      {
        return string.Empty;
      }
    }

    public enum ParameterNames
    {
      Error = 0,
      RSSI = 1,
      Date = 2,
      BatteryLifeCounter = 3,
      EndOfBatteryLifeDetection = 4,
      NbCounters = 5,
      LoggingMode = 6,
      WireCutDetect = 7,
      HighLeakDetect = 8,
      LowLeakDetect = 9,
      ReedDetect = 10, // 0x0000000A
      BackflowDetectionMethod = 11, // 0x0000000B
      Firmware = 12, // 0x0000000C
      GroupNumberForPolls = 13, // 0x0000000D
      BatteryLife = 14, // 0x0000000E
      WireCutA = 15, // 0x0000000F
      WireCutB = 16, // 0x00000010
      LowLeak = 17, // 0x00000011
      HighLeak = 18, // 0x00000012
      ReedFaultAORWireCutC = 19, // 0x00000013
      ReedFaultBORWireCutD = 20, // 0x00000014
      ReverseLeak = 21, // 0x00000015
      PulseValueA = 22, // 0x00000016
      PulseValueB = 23, // 0x00000017
      PulseValueC = 24, // 0x00000018
      PulseValueD = 25, // 0x00000019
      PulseUnitA = 26, // 0x0000001A
      PulseUnitB = 27, // 0x0000001B
      PulseUnitC = 28, // 0x0000001C
      PulseUnitD = 29, // 0x0000001D
      ReedFaultADate = 30, // 0x0000001E
      ReedFaultBDate = 31, // 0x0000001F
      WireCutADate = 32, // 0x00000020
      WireCutBDate = 33, // 0x00000021
      WireCutCDate = 34, // 0x00000022
      WireCutDDate = 35, // 0x00000023
      IndexA = 44, // 0x0000002C
      IndexB = 45, // 0x0000002D
      IndexC = 46, // 0x0000002E
      IndexD = 47, // 0x0000002F
      IndexA00 = 48, // 0x00000030
      IndexA01 = 49, // 0x00000031
      IndexA02 = 50, // 0x00000032
      IndexA03 = 51, // 0x00000033
      IndexA04 = 52, // 0x00000034
      IndexA05 = 53, // 0x00000035
      IndexA06 = 54, // 0x00000036
      IndexA07 = 55, // 0x00000037
      IndexA08 = 56, // 0x00000038
      IndexA09 = 57, // 0x00000039
      IndexA10 = 58, // 0x0000003A
      IndexA11 = 59, // 0x0000003B
      IndexA12 = 60, // 0x0000003C
      IndexA13 = 61, // 0x0000003D
      IndexA14 = 62, // 0x0000003E
      IndexA15 = 63, // 0x0000003F
      IndexA16 = 64, // 0x00000040
      IndexA17 = 65, // 0x00000041
      IndexA18 = 66, // 0x00000042
      IndexA19 = 67, // 0x00000043
      IndexA20 = 68, // 0x00000044
      IndexA21 = 69, // 0x00000045
      IndexA22 = 70, // 0x00000046
      IndexA23 = 71, // 0x00000047
      IndexB00 = 72, // 0x00000048
      IndexB01 = 73, // 0x00000049
      IndexB02 = 74, // 0x0000004A
      IndexB03 = 75, // 0x0000004B
      IndexB04 = 76, // 0x0000004C
      IndexB05 = 77, // 0x0000004D
      IndexB06 = 78, // 0x0000004E
      IndexB07 = 79, // 0x0000004F
      IndexB08 = 80, // 0x00000050
      IndexB09 = 81, // 0x00000051
      IndexB10 = 82, // 0x00000052
      IndexB11 = 83, // 0x00000053
      IndexC00 = 84, // 0x00000054
      IndexC01 = 85, // 0x00000055
      IndexC02 = 86, // 0x00000056
      IndexC03 = 87, // 0x00000057
      IndexC04 = 88, // 0x00000058
      IndexC05 = 89, // 0x00000059
      IndexC06 = 90, // 0x0000005A
      IndexC07 = 91, // 0x0000005B
      IndexC08 = 92, // 0x0000005C
      IndexC09 = 93, // 0x0000005D
      IndexC10 = 94, // 0x0000005E
      IndexC11 = 95, // 0x0000005F
      IndexD00 = 96, // 0x00000060
      IndexD01 = 97, // 0x00000061
      IndexD02 = 98, // 0x00000062
      IndexD03 = 99, // 0x00000063
      IndexD04 = 100, // 0x00000064
      IndexD05 = 101, // 0x00000065
      IndexD06 = 102, // 0x00000066
      IndexD07 = 103, // 0x00000067
      IndexD08 = 104, // 0x00000068
      IndexD09 = 105, // 0x00000069
      IndexD10 = 106, // 0x0000006A
      IndexD11 = 107, // 0x0000006B
      LoggerDate = 108, // 0x0000006C
      WakeUpMode = 109, // 0x0000006D
      DefaultWakeupDuration = 110, // 0x0000006E
      HourFirstTimeFrameStarts = 111, // 0x0000006F
      FirstTimeFrameWakeupDuration = 112, // 0x00000070
      HourSecondTimeFrameStarts = 113, // 0x00000071
      SecondTimeFrameWakeupDuration = 114, // 0x00000072
      TimeFramesEnabledOnMonday = 115, // 0x00000073
      TimeFramesEnabledOnTuesday = 116, // 0x00000074
      TimeFramesEnabledOnWednesday = 117, // 0x00000075
      TimeFramesEnabledOnThursday = 118, // 0x00000076
      TimeFramesEnabledOnFriday = 119, // 0x00000077
      TimeFramesEnabledOnSaturday = 120, // 0x00000078
      TimeFramesEnabledOnSunday = 121, // 0x00000079
      WakeupDisabledOnMonday = 122, // 0x0000007A
      WakeupDisabledOnTuesday = 123, // 0x0000007B
      WakeupDisabledOnWednesday = 124, // 0x0000007C
      WakeupDisabledOnThursday = 125, // 0x0000007D
      WakeupDisabledOnFriday = 126, // 0x0000007E
      WakeupDisabledOnSaturday = 127, // 0x0000007F
      WakeupDisabledOnSunday = 128, // 0x00000080
      DatalogStart = 129, // 0x00000081
      DatalogMeasurePeriodUnit = 130, // 0x00000082
      DatalogMeasurePeriodCoefficient = 131, // 0x00000083
      TimeOfDatalogIf1PerMonthOrWeek = 132, // 0x00000084
      DayOfWeekOrMonth = 133, // 0x00000085
      LowLeakThresholdA = 134, // 0x00000086
      LowLeakDurationA = 135, // 0x00000087
      LowLeakThresholdB = 136, // 0x00000088
      LowLeakDurationB = 137, // 0x00000089
      LowLeakThresholdC = 138, // 0x0000008A
      LowLeakDurationC = 139, // 0x0000008B
      LowLeakThresholdD = 140, // 0x0000008C
      LowLeakDurationD = 141, // 0x0000008D
      HighLeakThresholdA = 142, // 0x0000008E
      HighLeakDurationA = 143, // 0x0000008F
      HighLeakThresholdB = 144, // 0x00000090
      HighLeakDurationB = 145, // 0x00000091
      HighLeakThresholdC = 146, // 0x00000092
      HighLeakDurationC = 147, // 0x00000093
      HighLeakThresholdD = 148, // 0x00000094
      HighLeakDurationD = 149, // 0x00000095
      TimeBetweenMeasures = 150, // 0x00000096
      BackFlowDurationA = 151, // 0x00000097
      BackFlowThresholdA = 152, // 0x00000098
      BackFlowDurationB = 153, // 0x00000099
      BackFlowThresholdB = 154, // 0x0000009A
      SpecialBackFlowDurationA = 155, // 0x0000009B
      SpecialBackFlowThresholdA = 156, // 0x0000009C
      SpecialBackFlowDurationB = 157, // 0x0000009D
      SpecialBackFlowThresholdB = 158, // 0x0000009E
      MonthlyFlagsOnReverseFlowA = 159, // 0x0000009F
      MonthlyFlagsOnReverseFlowB = 160, // 0x000000A0
      AlarmWireCutAndReedFault = 161, // 0x000000A1
      AlarmBatteryLife = 162, // 0x000000A2
      AlarmLowLeak = 163, // 0x000000A3
      AlarmHighLeak = 164, // 0x000000A4
      AlarmReverseLeak = 165, // 0x000000A5
      AlarmDestination = 166, // 0x000000A6
      AlarmRouter1 = 167, // 0x000000A7
      AlarmRouter2 = 168, // 0x000000A8
      AlarmRouter3 = 169, // 0x000000A9
      IndexLMEA = 170, // 0x000000AA
      IndexLMEB = 171, // 0x000000AB
      IndexLMEC = 172, // 0x000000AC
      IndexLMED = 173, // 0x000000AD
      LeakHistoryAppearance0 = 174, // 0x000000AE
      LeakHistoryDisappearance0 = 175, // 0x000000AF
      LeakHistoryHighLeak0 = 176, // 0x000000B0
      LeakHistoryLowLeak0 = 177, // 0x000000B1
      LeakHistoryDateTime0 = 178, // 0x000000B2
      LeakHistoryInput0 = 179, // 0x000000B3
      LeakHistoryFlow0 = 180, // 0x000000B4
      LeakHistoryAppearance1 = 181, // 0x000000B5
      LeakHistoryDisappearance1 = 182, // 0x000000B6
      LeakHistoryHighLeak1 = 183, // 0x000000B7
      LeakHistoryLowLeak1 = 184, // 0x000000B8
      LeakHistoryDateTime1 = 185, // 0x000000B9
      LeakHistoryInput1 = 186, // 0x000000BA
      LeakHistoryFlow1 = 187, // 0x000000BB
      LeakHistoryAppearance2 = 188, // 0x000000BC
      LeakHistoryDisappearance2 = 189, // 0x000000BD
      LeakHistoryHighLeak2 = 190, // 0x000000BE
      LeakHistoryLowLeak2 = 191, // 0x000000BF
      LeakHistoryDateTime2 = 192, // 0x000000C0
      LeakHistoryInput2 = 193, // 0x000000C1
      LeakHistoryFlow2 = 194, // 0x000000C2
      LeakHistoryAppearance3 = 195, // 0x000000C3
      LeakHistoryDisappearance3 = 196, // 0x000000C4
      LeakHistoryHighLeak3 = 197, // 0x000000C5
      LeakHistoryLowLeak3 = 198, // 0x000000C6
      LeakHistoryDateTime3 = 199, // 0x000000C7
      LeakHistoryInput3 = 200, // 0x000000C8
      LeakHistoryFlow3 = 201, // 0x000000C9
      LeakHistoryAppearance4 = 202, // 0x000000CA
      LeakHistoryDisappearance4 = 203, // 0x000000CB
      LeakHistoryHighLeak4 = 204, // 0x000000CC
      LeakHistoryLowLeak4 = 205, // 0x000000CD
      LeakHistoryDateTime4 = 206, // 0x000000CE
      LeakHistoryInput4 = 207, // 0x000000CF
      LeakHistoryFlow4 = 208, // 0x000000D0
      BackFlowHistoryBackFlowPresenceDuration0 = 209, // 0x000000D1
      BackFlowHistoryDateTime0 = 210, // 0x000000D2
      BackFlowHistoryDateTimeEnded0 = 211, // 0x000000D3
      BackFlowHistoryDetectionDuration0 = 212, // 0x000000D4
      BackFlowHistoryFlow0 = 213, // 0x000000D5
      BackFlowHistoryInput0 = 214, // 0x000000D6
      BackFlowHistoryBackFlowPresenceDuration1 = 215, // 0x000000D7
      BackFlowHistoryDateTime1 = 216, // 0x000000D8
      BackFlowHistoryDateTimeEnded1 = 217, // 0x000000D9
      BackFlowHistoryDetectionDuration1 = 218, // 0x000000DA
      BackFlowHistoryFlow1 = 219, // 0x000000DB
      BackFlowHistoryInput1 = 220, // 0x000000DC
      BackFlowHistoryBackFlowPresenceDuration2 = 221, // 0x000000DD
      BackFlowHistoryDateTime2 = 222, // 0x000000DE
      BackFlowHistoryDateTimeEnded2 = 223, // 0x000000DF
      BackFlowHistoryDetectionDuration2 = 224, // 0x000000E0
      BackFlowHistoryFlow2 = 225, // 0x000000E1
      BackFlowHistoryInput2 = 226, // 0x000000E2
      BackFlowHistoryBackFlowPresenceDuration3 = 227, // 0x000000E3
      BackFlowHistoryDateTime3 = 228, // 0x000000E4
      BackFlowHistoryDateTimeEnded3 = 229, // 0x000000E5
      BackFlowHistoryDetectionDuration3 = 230, // 0x000000E6
      BackFlowHistoryFlow3 = 231, // 0x000000E7
      BackFlowHistoryInput3 = 232, // 0x000000E8
      IsFourInputs = 233, // 0x000000E9
      SensorType = 234, // 0x000000EA
      BackflowDetectionType = 235, // 0x000000EB
      Valve = 236, // 0x000000EC
      SolenoidState = 237, // 0x000000ED
    }
  }
}

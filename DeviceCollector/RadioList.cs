// Decompiled with JetBrains decompiler
// Type: DeviceCollector.RadioList
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  internal class RadioList : DeviceList
  {
    private static Logger logger = LogManager.GetLogger(nameof (RadioList));
    public List<long> ExpectedDevices;
    private int countOfFoundExpectedDevices;
    private int countOfFoundUnexpectedDevices;

    public bool HasAllExpectedDevicesFound
    {
      get
      {
        return this.ExpectedDevices.Count > 0 && this.countOfFoundExpectedDevices == this.ExpectedDevices.Count;
      }
    }

    public DeviceInfo DeviceInfoOfLastReceivedPacket { get; set; }

    public Dictionary<long, RadioDataSet> ReceivedData { get; private set; }

    public RadioList(DeviceCollectorFunctions busDevice)
    {
      this.MyBus = busDevice;
      this.bus = new ArrayList();
      this.FaultyDevices = new List<MBusDevice>();
      this.ExpectedDevices = new List<long>();
      this.ReceivedData = new Dictionary<long, RadioDataSet>();
    }

    public List<GlobalDeviceId> GetGlobalDeviceIdList()
    {
      List<GlobalDeviceId> globalDeviceIdList = new List<GlobalDeviceId>();
      foreach (KeyValuePair<long, RadioDataSet> keyValuePair in this.ReceivedData)
      {
        GlobalDeviceId globalDeviceId = new GlobalDeviceId();
        globalDeviceId.Serialnumber = keyValuePair.Key.ToString();
        if (keyValuePair.Value.LastRadioPacket != null)
        {
          globalDeviceId.DeviceTypeName = keyValuePair.Value.LastRadioPacket.DeviceType.ToString();
          globalDeviceId.MeterType = ValueIdent.ConvertToMeterType(keyValuePair.Value.LastRadioPacket.DeviceType);
        }
        globalDeviceIdList.Add(globalDeviceId);
      }
      return globalDeviceIdList;
    }

    internal override bool SelectDeviceBySerialNumber(string SerialNumber)
    {
      long funkId = Util.ToLong((object) SerialNumber);
      if (this.bus.Count == 0)
        return false;
      this.SelectedDevice = (BusDevice) this.Find(funkId);
      return this.SelectedDevice != null;
    }

    internal override bool DeleteSelectedDevice()
    {
      if (this.SelectedDevice == null)
        return false;
      long funkId = ((RadioDevice) this.SelectedDevice).Device.FunkId;
      int index;
      if (this.Find(funkId, out index) != null && index != -1)
      {
        this.bus.RemoveAt(index);
        this.ReceivedData.Remove(funkId);
      }
      return true;
    }

    internal override string GetAllParameters()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (object bu in this.bus)
      {
        foreach (DeviceInfo deviceInfo in (bu as RadioDevice).DeviceInfoList)
          stringBuilder.Append(deviceInfo.GetZDFParameterString()).Append(ZR_Constants.SystemNewLine);
      }
      return stringBuilder.ToString();
    }

    internal override bool AddDevice(DeviceTypes t, bool select)
    {
      this.bus.Add((object) new RadioDevice(this.MyBus));
      return true;
    }

    internal bool ReadRadio()
    {
      if (this.MyBus.RadioReader == null)
        return false;
      if (this.MyBus.MyBusMode == BusMode.MinomatRadioTest)
      {
        if (this.MyBus.DaKonId == null || string.IsNullOrEmpty(this.MyBus.DaKonId.Trim()))
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Invalid NetworkID!", RadioList.logger);
        long funkId;
        try
        {
          funkId = long.Parse(this.MyBus.DaKonId);
        }
        catch
        {
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Invalid NetworkID!", RadioList.logger);
        }
        this.ClearExpectedDevices();
        if (!this.AddExpectedDevice(funkId))
          return false;
      }
      this.ClearReceivedPackets();
      this.MyBus.RadioReader.OnPacketReceived += new EventHandler<RadioPacket>(this.RadioReader_OnPacketReceived);
      try
      {
        if (this.MyBus.RadioReader.Read() == null)
          return false;
      }
      finally
      {
        this.MyBus.RadioReader.OnPacketReceived -= new EventHandler<RadioPacket>(this.RadioReader_OnPacketReceived);
      }
      return true;
    }

    private void RadioReader_OnPacketReceived(object sender, RadioPacket e)
    {
      this.AddRadioDevice(e);
    }

    internal void AddPacket(RadioPacket packet)
    {
      if (!this.ReceivedData.ContainsKey(packet.FunkId))
      {
        this.ReceivedData.Add(packet.FunkId, new RadioDataSet());
        this.AddRadioDevice(packet);
        if (this.ExpectedDevices.Contains(packet.FunkId))
          ++this.countOfFoundExpectedDevices;
        else
          ++this.countOfFoundUnexpectedDevices;
        if (this.ExpectedDevices.Count > 0)
        {
          GMM_EventArgs e = new GMM_EventArgs(GMM_EventArgs.MessageType.MessageAndProgressPercentage);
          e.EventMessage = "Found new " + packet.DeviceType.ToString() + " device! ID: " + packet.FunkId.ToString();
          e.deviceListStatus.FoundExpectedDevices = this.countOfFoundExpectedDevices;
          e.deviceListStatus.FoundUnexpectedDevices = this.countOfFoundUnexpectedDevices;
          e.deviceListStatus.DevicesMissing = this.ExpectedDevices.Count - this.countOfFoundExpectedDevices;
          e.ProgressPercentage = this.GetProgress();
          this.MyBus.SendMessage(e);
          if (RadioList.logger.IsInfoEnabled)
          {
            Logger logger = RadioList.logger;
            string[] strArray = new string[11];
            int num = e.ProgressPercentage;
            strArray[0] = num.ToString();
            strArray[1] = "%\tExpected(";
            strArray[2] = this.countOfFoundExpectedDevices.ToString();
            strArray[3] = "/";
            num = this.ExpectedDevices.Count;
            strArray[4] = num.ToString();
            strArray[5] = ")\tUnexpected(";
            strArray[6] = this.countOfFoundUnexpectedDevices.ToString();
            strArray[7] = ")\t";
            strArray[8] = packet.FunkId.ToString();
            strArray[9] = "\t";
            strArray[10] = packet.DeviceType.ToString();
            string message = string.Concat(strArray);
            logger.Info(message);
          }
        }
      }
      this.ReceivedData[packet.FunkId].UpdateData(packet);
    }

    public int GetProgress() => this.countOfFoundExpectedDevices * 100 / this.ExpectedDevices.Count;

    public void AddRadioDevice(RadioPacket packet)
    {
      DeviceInfo deviceInfo = new DeviceInfo();
      deviceInfo.Manufacturer = "MINOL";
      deviceInfo.LastReadingDate = packet.ReceivedAt;
      deviceInfo.DeviceType = packet.DeviceType;
      deviceInfo.MeterNumber = packet.FunkId.ToString("00000000");
      if (this.MyBus.MyBusMode == BusMode.Radio2)
      {
        RadioPacketRadio2 radioPacketRadio2 = packet as RadioPacketRadio2;
        if (!radioPacketRadio2.IsCrcOk)
          return;
        List<DeviceInfo.MBusParamStruct> parameterList1 = deviceInfo.ParameterList;
        DateTime dateTime = SystemValues.DateTimeNow;
        DeviceInfo.MBusParamStruct mbusParamStruct1 = new DeviceInfo.MBusParamStruct("RTIME", dateTime.ToString(FixedFormates.TheFormates.DateTimeFormat.FullDateTimePattern));
        parameterList1.Add(mbusParamStruct1);
        deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("SID", radioPacketRadio2.FunkId.ToString()));
        deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("MAN", deviceInfo.Manufacturer));
        deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("GEN", "2"));
        deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("MED", radioPacketRadio2.DeviceType.ToString()));
        int? rssiDBm = radioPacketRadio2.RSSI_dBm;
        if (rssiDBm.HasValue)
        {
          List<DeviceInfo.MBusParamStruct> parameterList2 = deviceInfo.ParameterList;
          rssiDBm = radioPacketRadio2.RSSI_dBm;
          DeviceInfo.MBusParamStruct mbusParamStruct2 = new DeviceInfo.MBusParamStruct("RSSI_dBm", rssiDBm.Value.ToString((IFormatProvider) FixedFormates.TheFormates.NumberFormat));
          parameterList2.Add(mbusParamStruct2);
        }
        else
          deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("RSSI_dBm", "NULL"));
        List<DeviceInfo.MBusParamStruct> parameterList3 = deviceInfo.ParameterList;
        dateTime = radioPacketRadio2.TimePoint;
        DeviceInfo.MBusParamStruct mbusParamStruct3 = new DeviceInfo.MBusParamStruct("TimePoint", dateTime.ToString(FixedFormates.TheFormates.DateTimeFormat.FullDateTimePattern));
        parameterList3.Add(mbusParamStruct3);
        Decimal num;
        if (radioPacketRadio2.CurrentValue.HasValue)
        {
          List<DeviceInfo.MBusParamStruct> parameterList4 = deviceInfo.ParameterList;
          num = radioPacketRadio2.CurrentValue.Value;
          DeviceInfo.MBusParamStruct mbusParamStruct4 = new DeviceInfo.MBusParamStruct("CurrentValue", num.ToString((IFormatProvider) FixedFormates.TheFormates.NumberFormat));
          parameterList4.Add(mbusParamStruct4);
        }
        else
          deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("CurrentValue", "NULL"));
        if (radioPacketRadio2.DueDate != DateTime.MinValue)
        {
          List<DeviceInfo.MBusParamStruct> parameterList5 = deviceInfo.ParameterList;
          dateTime = radioPacketRadio2.DueDate;
          DeviceInfo.MBusParamStruct mbusParamStruct5 = new DeviceInfo.MBusParamStruct("DueDate", dateTime.ToString(FixedFormates.TheFormates.DateTimeFormat.FullDateTimePattern));
          parameterList5.Add(mbusParamStruct5);
        }
        else
          deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("DueDate", "NULL"));
        if (radioPacketRadio2.DueDateValue.HasValue)
        {
          List<DeviceInfo.MBusParamStruct> parameterList6 = deviceInfo.ParameterList;
          num = radioPacketRadio2.DueDateValue.Value;
          DeviceInfo.MBusParamStruct mbusParamStruct6 = new DeviceInfo.MBusParamStruct("DueDateValue", num.ToString((IFormatProvider) FixedFormates.TheFormates.NumberFormat));
          parameterList6.Add(mbusParamStruct6);
        }
        else
          deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("DueDateValue", "NULL"));
        deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("MonthIndex", radioPacketRadio2.MonthIndex.ToString()));
        Decimal? dynamicValue = radioPacketRadio2.DynamicValue;
        if (dynamicValue.HasValue)
        {
          List<DeviceInfo.MBusParamStruct> parameterList7 = deviceInfo.ParameterList;
          dateTime = radioPacketRadio2.DynamicDate;
          DeviceInfo.MBusParamStruct mbusParamStruct7 = new DeviceInfo.MBusParamStruct("DynamicDate", dateTime.ToString(FixedFormates.TheFormates.DateTimeFormat.FullDateTimePattern));
          parameterList7.Add(mbusParamStruct7);
          List<DeviceInfo.MBusParamStruct> parameterList8 = deviceInfo.ParameterList;
          dynamicValue = radioPacketRadio2.DynamicValue;
          num = dynamicValue.Value;
          DeviceInfo.MBusParamStruct mbusParamStruct8 = new DeviceInfo.MBusParamStruct("DynamicValue", num.ToString((IFormatProvider) FixedFormates.TheFormates.NumberFormat));
          parameterList8.Add(mbusParamStruct8);
        }
        if (radioPacketRadio2.IsHeatCostAllocator)
        {
          deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("SensorMode", radioPacketRadio2.SensorMode.ToString()));
          List<DeviceInfo.MBusParamStruct> parameterList9 = deviceInfo.ParameterList;
          num = radioPacketRadio2.Exponent2F;
          DeviceInfo.MBusParamStruct mbusParamStruct9 = new DeviceInfo.MBusParamStruct("Exponent2F", num.ToString((IFormatProvider) FixedFormates.TheFormates.NumberFormat));
          parameterList9.Add(mbusParamStruct9);
          List<DeviceInfo.MBusParamStruct> parameterList10 = deviceInfo.ParameterList;
          num = (Decimal) radioPacketRadio2.K / 1000M;
          DeviceInfo.MBusParamStruct mbusParamStruct10 = new DeviceInfo.MBusParamStruct("HCA_Factor_Weighting", num.ToString((IFormatProvider) FixedFormates.TheFormates.NumberFormat));
          parameterList10.Add(mbusParamStruct10);
        }
        else
        {
          deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("ScaleFactor", radioPacketRadio2.ScaleFactor.ToString((IFormatProvider) FixedFormates.TheFormates.NumberFormat)));
          deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("Unit", radioPacketRadio2.Unit.ToString()));
        }
        deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("IsDeviceError", radioPacketRadio2.IsDeviceError.ToString()));
        deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("IsManipulated", radioPacketRadio2.IsManipulated.ToString()));
        deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("IsMeasurementEnabled", radioPacketRadio2.IsMeasurementEnabled.ToString()));
        deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("IsSummerTime", radioPacketRadio2.IsSummerTime.ToString()));
        deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("MeasurementError", radioPacketRadio2.MeasurementError.ToString()));
        deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("MCT", radioPacketRadio2.MCT.ToString()));
      }
      else if (this.MyBus.MyBusMode == BusMode.Radio4)
      {
        RadioPacketRadio3 radioPacketRadio3 = packet as RadioPacketRadio3;
        deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("RTIME", SystemValues.DateTimeNow.ToString(FixedFormates.TheFormates.DateTimeFormat.FullDateTimePattern)));
        deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("SID", radioPacketRadio3.FunkId.ToString()));
      }
      else if (this.MyBus.MyBusMode == BusMode.Radio3 || this.MyBus.MyBusMode == BusMode.Radio3_868_95_RUSSIA)
      {
        RadioPacketRadio3 radioPacketRadio3 = packet as RadioPacketRadio3;
        if (!radioPacketRadio3.IsCrcOk)
          return;
        List<DeviceInfo.MBusParamStruct> parameterList11 = deviceInfo.ParameterList;
        DateTime dateTime = SystemValues.DateTimeNow;
        DeviceInfo.MBusParamStruct mbusParamStruct11 = new DeviceInfo.MBusParamStruct("RTIME", dateTime.ToString(FixedFormates.TheFormates.DateTimeFormat.FullDateTimePattern));
        parameterList11.Add(mbusParamStruct11);
        deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("SID", radioPacketRadio3.FunkId.ToString()));
        deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("MAN", deviceInfo.Manufacturer));
        deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("GEN", "3"));
        deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("MED", radioPacketRadio3.DeviceType.ToString()));
        int? nullable = radioPacketRadio3.RSSI_dBm;
        int num1;
        if (nullable.HasValue)
        {
          List<DeviceInfo.MBusParamStruct> parameterList12 = deviceInfo.ParameterList;
          nullable = radioPacketRadio3.RSSI_dBm;
          num1 = nullable.Value;
          DeviceInfo.MBusParamStruct mbusParamStruct12 = new DeviceInfo.MBusParamStruct("RSSI_dBm", num1.ToString((IFormatProvider) FixedFormates.TheFormates.NumberFormat));
          parameterList12.Add(mbusParamStruct12);
        }
        else
          deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("RSSI_dBm", "NULL"));
        if (radioPacketRadio3.RadioTestPacket != null)
        {
          deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("TestPacketVersion", radioPacketRadio3.RadioTestPacket.TestPacketVersion.ToString()));
          deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("MeterID", radioPacketRadio3.RadioTestPacket.MeterID.ToString()));
          deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("SapNumber", radioPacketRadio3.RadioTestPacket.SapNumber.ToString()));
        }
        else
        {
          List<DeviceInfo.MBusParamStruct> parameterList13 = deviceInfo.ParameterList;
          dateTime = radioPacketRadio3.TimePoint;
          DeviceInfo.MBusParamStruct mbusParamStruct13 = new DeviceInfo.MBusParamStruct("TimePoint", dateTime.ToString(FixedFormates.TheFormates.DateTimeFormat.FullDateTimePattern));
          parameterList13.Add(mbusParamStruct13);
          if (radioPacketRadio3.DeviceType == DeviceTypes.SmokeDetector)
          {
            DateTime? ofFirstActivation = radioPacketRadio3.SmokeDetector.DateOfFirstActivation;
            if (ofFirstActivation.HasValue)
            {
              List<DeviceInfo.MBusParamStruct> parameterList14 = deviceInfo.ParameterList;
              ofFirstActivation = radioPacketRadio3.SmokeDetector.DateOfFirstActivation;
              dateTime = ofFirstActivation.Value;
              DeviceInfo.MBusParamStruct mbusParamStruct14 = new DeviceInfo.MBusParamStruct("DateOfFirstActivation", dateTime.ToString(FixedFormates.TheFormates.DateTimeFormat.FullDateTimePattern));
              parameterList14.Add(mbusParamStruct14);
            }
            else
              deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("DateOfFirstActivation", "NULL"));
            deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("IsDeviceError", radioPacketRadio3.IsDeviceError.ToString()));
            deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("IsManipulated", radioPacketRadio3.IsManipulated.ToString()));
            deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("IsAccuDefect", radioPacketRadio3.IsAccuDefect.ToString()));
            deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("PacketNr", radioPacketRadio3.PacketNr.ToString()));
            SmokeDetectorEvent?[] monthlyEvents = radioPacketRadio3.SmokeDetector.MonthlyEvents;
            for (int index = 0; index < monthlyEvents.Length; ++index)
            {
              if (monthlyEvents[index].HasValue)
                deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("M[" + index.ToString() + "]", monthlyEvents[index].Value.ToString()));
            }
            SmokeDetectorEvent?[] dailyEvents = radioPacketRadio3.SmokeDetector.DailyEvents;
            for (int index = 0; index < dailyEvents.Length; ++index)
            {
              if (dailyEvents[index].HasValue)
                deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("D[" + index.ToString() + "]", dailyEvents[index].Value.ToString()));
            }
          }
          else if (radioPacketRadio3.DeviceType == DeviceTypes.TemperatureSensor || radioPacketRadio3.DeviceType == DeviceTypes.HumiditySensor)
          {
            if (radioPacketRadio3.DeviceType == DeviceTypes.HumiditySensor)
              deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("Unit", "% "));
            else
              deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("Unit", radioPacketRadio3.TemperaturUnit.ToString()));
            Decimal num2;
            if (radioPacketRadio3.CurrentValue.HasValue)
            {
              List<DeviceInfo.MBusParamStruct> parameterList15 = deviceInfo.ParameterList;
              num2 = radioPacketRadio3.CurrentValue.Value;
              DeviceInfo.MBusParamStruct mbusParamStruct15 = new DeviceInfo.MBusParamStruct("CurrentValue", num2.ToString((IFormatProvider) FixedFormates.TheFormates.NumberFormat));
              parameterList15.Add(mbusParamStruct15);
            }
            else
              deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("CurrentValue", "NULL"));
            if (radioPacketRadio3.DeviceErrorDate.HasValue)
            {
              List<DeviceInfo.MBusParamStruct> parameterList16 = deviceInfo.ParameterList;
              dateTime = radioPacketRadio3.DeviceErrorDate.Value;
              DeviceInfo.MBusParamStruct mbusParamStruct16 = new DeviceInfo.MBusParamStruct("DeviceErrorDate", dateTime.ToString(FixedFormates.TheFormates.DateTimeFormat.FullDateTimePattern));
              parameterList16.Add(mbusParamStruct16);
            }
            if (radioPacketRadio3.ManipulationDate.HasValue)
            {
              List<DeviceInfo.MBusParamStruct> parameterList17 = deviceInfo.ParameterList;
              dateTime = radioPacketRadio3.ManipulationDate.Value;
              DeviceInfo.MBusParamStruct mbusParamStruct17 = new DeviceInfo.MBusParamStruct("ManipulationDate", dateTime.ToString(FixedFormates.TheFormates.DateTimeFormat.FullDateTimePattern));
              parameterList17.Add(mbusParamStruct17);
            }
            deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("PacketNr", radioPacketRadio3.PacketNr.ToString()));
            deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("IsDeviceError", radioPacketRadio3.IsDeviceError.ToString()));
            deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("IsManipulated", radioPacketRadio3.IsManipulated.ToString()));
            deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("IsAccuDefect", radioPacketRadio3.IsAccuDefect.ToString()));
            byte? scenarioNr = radioPacketRadio3.ScenarioNr;
            if (scenarioNr.HasValue)
            {
              List<DeviceInfo.MBusParamStruct> parameterList18 = deviceInfo.ParameterList;
              scenarioNr = radioPacketRadio3.ScenarioNr;
              DeviceInfo.MBusParamStruct mbusParamStruct18 = new DeviceInfo.MBusParamStruct("ScenarioNr", scenarioNr.ToString());
              parameterList18.Add(mbusParamStruct18);
            }
            else
              deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("ScenarioNr", "NULL"));
            if (radioPacketRadio3.Months != null)
            {
              foreach (KeyValuePair<int, RadioPacketRadio3.MonthValue> month in (Dictionary<int, RadioPacketRadio3.MonthValue>) radioPacketRadio3.Months)
              {
                if (month.Value.Value.HasValue)
                {
                  List<DeviceInfo.MBusParamStruct> parameterList19 = deviceInfo.ParameterList;
                  num1 = month.Key;
                  string DefStr1 = "MonthDate[" + num1.ToString() + "]";
                  dateTime = month.Value.TimePoint;
                  string ValStr1 = dateTime.ToString(FixedFormates.TheFormates.DateTimeFormat.FullDateTimePattern);
                  DeviceInfo.MBusParamStruct mbusParamStruct19 = new DeviceInfo.MBusParamStruct(DefStr1, ValStr1);
                  parameterList19.Add(mbusParamStruct19);
                  List<DeviceInfo.MBusParamStruct> parameterList20 = deviceInfo.ParameterList;
                  num1 = month.Key;
                  string DefStr2 = "MonthValue[" + num1.ToString() + "]";
                  num2 = month.Value.Value.Value;
                  string ValStr2 = num2.ToString((IFormatProvider) FixedFormates.TheFormates.NumberFormat);
                  DeviceInfo.MBusParamStruct mbusParamStruct20 = new DeviceInfo.MBusParamStruct(DefStr2, ValStr2);
                  parameterList20.Add(mbusParamStruct20);
                }
              }
            }
          }
          else
          {
            byte? scenarioNr = radioPacketRadio3.ScenarioNr;
            nullable = scenarioNr.HasValue ? new int?((int) scenarioNr.GetValueOrDefault()) : new int?();
            num1 = 5;
            int num3;
            if (!(nullable.GetValueOrDefault() == num1 & nullable.HasValue))
            {
              scenarioNr = radioPacketRadio3.ScenarioNr;
              nullable = scenarioNr.HasValue ? new int?((int) scenarioNr.GetValueOrDefault()) : new int?();
              num1 = 6;
              num3 = nullable.GetValueOrDefault() == num1 & nullable.HasValue ? 1 : 0;
            }
            else
              num3 = 1;
            Decimal num4;
            if (num3 != 0)
            {
              if (radioPacketRadio3.IsHeatCostAllocator)
              {
                deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("Scale", radioPacketRadio3.Scale.ToString()));
                deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("SensorMode", radioPacketRadio3.SensorMode.ToString()));
                if (radioPacketRadio3.K.HasValue)
                {
                  List<DeviceInfo.MBusParamStruct> parameterList21 = deviceInfo.ParameterList;
                  num4 = (Decimal) radioPacketRadio3.K.Value / 1000M;
                  DeviceInfo.MBusParamStruct mbusParamStruct21 = new DeviceInfo.MBusParamStruct("K", num4.ToString((IFormatProvider) FixedFormates.TheFormates.NumberFormat));
                  parameterList21.Add(mbusParamStruct21);
                }
              }
              else if (radioPacketRadio3.K.HasValue)
              {
                deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("ScaleFactor", radioPacketRadio3.ScaleFactor.Value.ToString((IFormatProvider) FixedFormates.TheFormates.NumberFormat)));
                deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("Unit", radioPacketRadio3.Unit.ToString()));
              }
              if (radioPacketRadio3.CurrentValue.HasValue)
              {
                List<DeviceInfo.MBusParamStruct> parameterList22 = deviceInfo.ParameterList;
                num4 = radioPacketRadio3.CurrentValue.Value;
                DeviceInfo.MBusParamStruct mbusParamStruct22 = new DeviceInfo.MBusParamStruct("CurrentValue", num4.ToString((IFormatProvider) FixedFormates.TheFormates.NumberFormat));
                parameterList22.Add(mbusParamStruct22);
              }
              else
                deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("CurrentValue", "NULL"));
              if (radioPacketRadio3.ResetDate.HasValue)
              {
                List<DeviceInfo.MBusParamStruct> parameterList23 = deviceInfo.ParameterList;
                dateTime = radioPacketRadio3.ResetDate.Value;
                DeviceInfo.MBusParamStruct mbusParamStruct23 = new DeviceInfo.MBusParamStruct("ResetDate", dateTime.ToString(FixedFormates.TheFormates.DateTimeFormat.FullDateTimePattern));
                parameterList23.Add(mbusParamStruct23);
              }
            }
            else
            {
              if (radioPacketRadio3.DueDate.HasValue)
              {
                List<DeviceInfo.MBusParamStruct> parameterList24 = deviceInfo.ParameterList;
                dateTime = radioPacketRadio3.DueDate.Value;
                DeviceInfo.MBusParamStruct mbusParamStruct24 = new DeviceInfo.MBusParamStruct("DueDate", dateTime.ToString(FixedFormates.TheFormates.DateTimeFormat.FullDateTimePattern));
                parameterList24.Add(mbusParamStruct24);
              }
              else
                deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("DueDate", "NULL"));
              if (radioPacketRadio3.DueDateValue.HasValue)
              {
                List<DeviceInfo.MBusParamStruct> parameterList25 = deviceInfo.ParameterList;
                num4 = radioPacketRadio3.DueDateValue.Value;
                DeviceInfo.MBusParamStruct mbusParamStruct25 = new DeviceInfo.MBusParamStruct("DueDateValue", num4.ToString((IFormatProvider) FixedFormates.TheFormates.NumberFormat));
                parameterList25.Add(mbusParamStruct25);
              }
              else
                deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("DueDateValue", "NULL"));
            }
            if (radioPacketRadio3.DeviceErrorDate.HasValue)
            {
              List<DeviceInfo.MBusParamStruct> parameterList26 = deviceInfo.ParameterList;
              dateTime = radioPacketRadio3.DeviceErrorDate.Value;
              DeviceInfo.MBusParamStruct mbusParamStruct26 = new DeviceInfo.MBusParamStruct("DeviceErrorDate", dateTime.ToString(FixedFormates.TheFormates.DateTimeFormat.FullDateTimePattern));
              parameterList26.Add(mbusParamStruct26);
            }
            if (radioPacketRadio3.ManipulationDate.HasValue)
            {
              List<DeviceInfo.MBusParamStruct> parameterList27 = deviceInfo.ParameterList;
              dateTime = radioPacketRadio3.ManipulationDate.Value;
              DeviceInfo.MBusParamStruct mbusParamStruct27 = new DeviceInfo.MBusParamStruct("ManipulationDate", dateTime.ToString(FixedFormates.TheFormates.DateTimeFormat.FullDateTimePattern));
              parameterList27.Add(mbusParamStruct27);
            }
            deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("IsDeviceError", radioPacketRadio3.IsDeviceError.ToString()));
            deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("IsManipulated", radioPacketRadio3.IsManipulated.ToString()));
            deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("IsAccuDefect", radioPacketRadio3.IsAccuDefect.ToString()));
            if (radioPacketRadio3.Months != null)
            {
              foreach (KeyValuePair<int, RadioPacketRadio3.MonthValue> month in (Dictionary<int, RadioPacketRadio3.MonthValue>) radioPacketRadio3.Months)
              {
                if (month.Value.Value.HasValue)
                {
                  List<DeviceInfo.MBusParamStruct> parameterList28 = deviceInfo.ParameterList;
                  num1 = month.Key;
                  string DefStr3 = "MonthDate[" + num1.ToString() + "]";
                  dateTime = month.Value.TimePoint;
                  string ValStr3 = dateTime.ToString(FixedFormates.TheFormates.DateTimeFormat.FullDateTimePattern);
                  DeviceInfo.MBusParamStruct mbusParamStruct28 = new DeviceInfo.MBusParamStruct(DefStr3, ValStr3);
                  parameterList28.Add(mbusParamStruct28);
                  List<DeviceInfo.MBusParamStruct> parameterList29 = deviceInfo.ParameterList;
                  num1 = month.Key;
                  string DefStr4 = "MonthValue[" + num1.ToString() + "]";
                  num4 = month.Value.Value.Value;
                  string ValStr4 = num4.ToString((IFormatProvider) FixedFormates.TheFormates.NumberFormat);
                  DeviceInfo.MBusParamStruct mbusParamStruct29 = new DeviceInfo.MBusParamStruct(DefStr4, ValStr4);
                  parameterList29.Add(mbusParamStruct29);
                }
              }
            }
            scenarioNr = radioPacketRadio3.ScenarioNr;
            if (scenarioNr.HasValue)
            {
              List<DeviceInfo.MBusParamStruct> parameterList30 = deviceInfo.ParameterList;
              scenarioNr = radioPacketRadio3.ScenarioNr;
              DeviceInfo.MBusParamStruct mbusParamStruct30 = new DeviceInfo.MBusParamStruct("ScenarioNr", scenarioNr.ToString());
              parameterList30.Add(mbusParamStruct30);
            }
            else
              deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("ScenarioNr", "NULL"));
          }
        }
        deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("MCT", radioPacketRadio3.MCT.ToString()));
      }
      else if (this.MyBus.MyBusMode == BusMode.wMBusC1A || this.MyBus.MyBusMode == BusMode.wMBusC1B || this.MyBus.MyBusMode == BusMode.wMBusS1 || this.MyBus.MyBusMode == BusMode.wMBusS1M || this.MyBus.MyBusMode == BusMode.wMBusS2 || this.MyBus.MyBusMode == BusMode.wMBusT1 || this.MyBus.MyBusMode == BusMode.wMBusT2_meter || this.MyBus.MyBusMode == BusMode.wMBusT2_other)
      {
        RadioPacketWirelessMBus packetWirelessMbus = packet as RadioPacketWirelessMBus;
        if (!packetWirelessMbus.IsCrcOk)
          return;
        deviceInfo.Manufacturer = packetWirelessMbus.Manufacturer;
        deviceInfo.Medium = packetWirelessMbus.Medium;
        Dictionary<string, string> parametersAsList = ParameterService.GetAllParametersAsList(packetWirelessMbus.ZDF, ';');
        if (parametersAsList != null)
        {
          foreach (KeyValuePair<string, string> keyValuePair in parametersAsList)
            deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct(keyValuePair.Key, keyValuePair.Value));
        }
        deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("MCT", packetWirelessMbus.MCT.ToString()));
      }
      else
      {
        if (this.MyBus.MyBusMode != BusMode.MinomatRadioTest && this.MyBus.MyBusMode != BusMode.RadioMS)
          throw new ArgumentException("MyBus.MyBusMode");
        RadioPacketMinomatV4 radioPacketMinomatV4 = packet as RadioPacketMinomatV4;
        if (radioPacketMinomatV4.Header != null && radioPacketMinomatV4.Data != null)
        {
          deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("RTIME", SystemValues.DateTimeNow.ToString(FixedFormates.TheFormates.DateTimeFormat.FullDateTimePattern)));
          deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("NetworkID", radioPacketMinomatV4.FunkId.ToString()));
          deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("Type", radioPacketMinomatV4.Header.Type.ToString()));
          deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("Number", radioPacketMinomatV4.Header.Number.ToString()));
          deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("Parent", radioPacketMinomatV4.Data.Parent.ToString()));
          deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("Hops", radioPacketMinomatV4.Data.Hops.ToString()));
          List<DeviceInfo.MBusParamStruct> parameterList31 = deviceInfo.ParameterList;
          byte num5 = radioPacketMinomatV4.Data.Islot;
          DeviceInfo.MBusParamStruct mbusParamStruct31 = new DeviceInfo.MBusParamStruct("Islot", num5.ToString());
          parameterList31.Add(mbusParamStruct31);
          List<DeviceInfo.MBusParamStruct> parameterList32 = deviceInfo.ParameterList;
          ushort num6 = radioPacketMinomatV4.Data.Offset;
          DeviceInfo.MBusParamStruct mbusParamStruct32 = new DeviceInfo.MBusParamStruct("Offset", num6.ToString());
          parameterList32.Add(mbusParamStruct32);
          deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("Secs", radioPacketMinomatV4.Data.Secs.ToString()));
          List<DeviceInfo.MBusParamStruct> parameterList33 = deviceInfo.ParameterList;
          num6 = radioPacketMinomatV4.Data.Millis;
          DeviceInfo.MBusParamStruct mbusParamStruct33 = new DeviceInfo.MBusParamStruct("Millis", num6.ToString());
          parameterList33.Add(mbusParamStruct33);
          List<DeviceInfo.MBusParamStruct> parameterList34 = deviceInfo.ParameterList;
          num6 = radioPacketMinomatV4.Data.FrameCounter;
          DeviceInfo.MBusParamStruct mbusParamStruct34 = new DeviceInfo.MBusParamStruct("FrameCounter", num6.ToString());
          parameterList34.Add(mbusParamStruct34);
          List<DeviceInfo.MBusParamStruct> parameterList35 = deviceInfo.ParameterList;
          num5 = radioPacketMinomatV4.Data.SetupCounter;
          DeviceInfo.MBusParamStruct mbusParamStruct35 = new DeviceInfo.MBusParamStruct("SetupCounter", num5.ToString());
          parameterList35.Add(mbusParamStruct35);
          List<DeviceInfo.MBusParamStruct> parameterList36 = deviceInfo.ParameterList;
          num5 = radioPacketMinomatV4.Data.SubTreeId;
          DeviceInfo.MBusParamStruct mbusParamStruct36 = new DeviceInfo.MBusParamStruct("SubTreeId", num5.ToString());
          parameterList36.Add(mbusParamStruct36);
          List<DeviceInfo.MBusParamStruct> parameterList37 = deviceInfo.ParameterList;
          num5 = radioPacketMinomatV4.Data.InquiryRssi.Count;
          DeviceInfo.MBusParamStruct mbusParamStruct37 = new DeviceInfo.MBusParamStruct("Rssi.Count", num5.ToString());
          parameterList37.Add(mbusParamStruct37);
          List<DeviceInfo.MBusParamStruct> parameterList38 = deviceInfo.ParameterList;
          num5 = radioPacketMinomatV4.Data.InquiryRssi.Remainder;
          DeviceInfo.MBusParamStruct mbusParamStruct38 = new DeviceInfo.MBusParamStruct("Rssi.Remainder", num5.ToString());
          parameterList38.Add(mbusParamStruct38);
          List<DeviceInfo.MBusParamStruct> parameterList39 = deviceInfo.ParameterList;
          num5 = radioPacketMinomatV4.Data.InquiryRssi.Value;
          DeviceInfo.MBusParamStruct mbusParamStruct39 = new DeviceInfo.MBusParamStruct("Rssi.Value", num5.ToString());
          parameterList39.Add(mbusParamStruct39);
          deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("Minomat RSSI_dBm", radioPacketMinomatV4.Data.InquiryRssi.RSSI_dBm.ToString((IFormatProvider) FixedFormates.TheFormates.NumberFormat)));
          if (radioPacketMinomatV4.RSSI_dBm.HasValue)
            deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("MinoConnect RSSI_dBm", radioPacketMinomatV4.RSSI_dBm.Value.ToString((IFormatProvider) FixedFormates.TheFormates.NumberFormat)));
          else
            deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("MinoConnect RSSI_dBm", "NULL"));
          deviceInfo.ParameterList.Add(new DeviceInfo.MBusParamStruct("MCT", radioPacketMinomatV4.MCT.ToString()));
        }
      }
      RadioDevice radioDevice = this.Find(packet.FunkId);
      if (radioDevice == null)
      {
        RadioDevice e = new RadioDevice(this.MyBus, packet);
        e.Info = deviceInfo;
        e.DeviceInfoList.Add(deviceInfo);
        this.bus.Add((object) e);
        this.MyBus.RaiseEventOnDeviceListChanged((DeviceList) this, (BusDevice) e);
      }
      else
      {
        radioDevice.Device = packet;
        radioDevice.Info = deviceInfo;
        radioDevice.DeviceInfoList.Add(deviceInfo);
      }
      this.DeviceInfoOfLastReceivedPacket = deviceInfo;
    }

    private RadioDevice Find(long funkId) => this.Find(funkId, out int _);

    private RadioDevice Find(long funkId, out int index)
    {
      index = -1;
      for (int index1 = 0; index1 < this.bus.Count; ++index1)
      {
        if (((RadioDevice) this.bus[index1]).Device.FunkId == funkId)
        {
          index = index1;
          return (RadioDevice) this.bus[index1];
        }
      }
      return (RadioDevice) null;
    }

    internal void ClearReceivedPackets()
    {
      this.countOfFoundExpectedDevices = 0;
      this.countOfFoundUnexpectedDevices = 0;
      this.ReceivedData.Clear();
      this.bus.Clear();
      this.MyBus.RaiseEventOnDeviceListChanged((DeviceList) this, (BusDevice) null);
    }

    internal override void DeleteBusList()
    {
      RadioList.logger.Trace("Clear bus list and expected devices");
      this.ExpectedDevices.Clear();
      this.ClearReceivedPackets();
      base.DeleteBusList();
    }

    internal bool AddExpectedDevice(long funkId)
    {
      if (!this.ExpectedDevices.Contains(funkId))
      {
        this.ExpectedDevices.Add(funkId);
        return true;
      }
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "This number already exists! Value: " + funkId.ToString());
      return false;
    }

    internal void ClearExpectedDevices()
    {
      if (this.ExpectedDevices == null)
        return;
      this.ExpectedDevices.Clear();
    }

    internal override bool WorkBusAddresses() => true;
  }
}

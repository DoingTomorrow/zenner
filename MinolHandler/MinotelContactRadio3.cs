// Decompiled with JetBrains decompiler
// Type: MinolHandler.MinotelContactRadio3
// Assembly: MinolHandler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: A1A42975-0CFC-4FCB-838E-3BA18C5EABDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinolHandler.dll

using NLog;
using StartupLib;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace MinolHandler
{
  internal class MinotelContactRadio3 : MinolDevice
  {
    private static Logger logger = LogManager.GetLogger(nameof (MinotelContactRadio3));
    private static string[] ReadingNameExtention = new string[3]
    {
      "",
      "_A",
      "_B"
    };
    private long input1_PulseValue = -1;
    private long input2_PulseValue = -1;

    internal MinotelContactRadio3(MinolHandlerFunctions Functions)
      : base(Functions)
    {
      this.DeviceType = DeviceTypes.MinotelContactRadio3;
    }

    internal override int GetMaxSizeOfRequestBuffer() => 80;

    internal override bool CreateDeviceData()
    {
      if (this.MyFunctions.MyDevices.BitAccessByBitName["RAM;ERROR"].GetBitValue(this.Map))
        this.DeviceState = ReadingValueState.error;
      else
        this.DeviceState = ReadingValueState.ok;
      return true;
    }

    internal override object GetReadValue(string parameterKey)
    {
      if (this.MyFunctions.MyDevices.BitAccessByBitName.ContainsKey(parameterKey))
        return (object) this.MyFunctions.MyDevices.BitAccessByBitName[parameterKey].GetBitValue(this.Map);
      switch (parameterKey)
      {
        case "RAM;K_A_Unit":
          return (object) DeviceCollector.MinolDevice.ConvertMinolUnitToInputUnitsIndex((byte) Util.ToLong(this.MyFunctions.MyDevices.ParameterAccessByName["RAM;K_A"].GetReadValue(this.Map)));
        case "RAM;K_B_Unit":
          return (object) DeviceCollector.MinolDevice.ConvertMinolUnitToInputUnitsIndex((byte) Util.ToLong(this.MyFunctions.MyDevices.ParameterAccessByName["RAM;K_B"].GetReadValue(this.Map)));
        case "RAM;K_A_Value":
          return (object) Util.ToDouble((object) (Util.ToLong(this.MyFunctions.MyDevices.ParameterAccessByName["RAM;K_A"].GetReadValue(this.Map)) >> 4));
        case "RAM;K_B_Value":
          return (object) Util.ToDouble((object) (Util.ToLong(this.MyFunctions.MyDevices.ParameterAccessByName["RAM;K_B"].GetReadValue(this.Map)) >> 4));
        case "RAM;DateToday":
          DateTime dateTime = ((DateTime) this.MyFunctions.MyDevices.ParameterAccessByName["RAM;DateToday"].GetReadValue(this.Map)).AddMinutes((double) (long) this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Minute"].GetReadValue(this.Map));
          long readValue1 = (long) this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Hour"].GetReadValue(this.Map);
          dateTime = dateTime.AddHours((double) readValue1);
          long readValue2 = (long) this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Second"].GetReadValue(this.Map);
          return (object) dateTime.AddSeconds((double) readValue2);
        case "INFO;Radio2_Preload":
          return (object) (Util.ToInteger(this.MyFunctions.MyDevices.ParameterAccessByName["INFO;Radio2_Preload"].GetReadValue(this.Map)) * 2);
        case "INFO;Radio3_Preload":
          return (object) (Util.ToInteger(this.MyFunctions.MyDevices.ParameterAccessByName["INFO;Radio3_Preload"].GetReadValue(this.Map)) * 2);
        case "INFO;Radio3_Preload_B":
          return (object) (Util.ToInteger(this.MyFunctions.MyDevices.ParameterAccessByName["INFO;Radio3_Preload_B"].GetReadValue(this.Map)) * 2);
        case "INFO;RadioEpsilonOffsetEnabled":
          int int32 = Convert.ToInt32(this.MyFunctions.MyDevices.ParameterAccessByName["INFO;RFID_A"].GetReadValue(this.Map));
          int integer = Util.ToInteger(this.MyFunctions.MyDevices.ParameterAccessByName["INFO;Radio3_Epsilon"].GetReadValue(this.Map));
          int radio3EpsilonValue = MinolDevice.CalculateRadio3EpsilonValue(int32);
          if (radio3EpsilonValue == integer)
            return (object) true;
          if (integer != 0 && integer != (int) ushort.MaxValue)
            MinotelContactRadio3.logger.Warn<int, int>("Wrong radio 3 epsilon value! Expected=0x{0:X4}, Actual=0x{1:X4}", radio3EpsilonValue, integer);
          return (object) false;
        case "RAM;RadioProtocol":
          bool boolean1 = Convert.ToBoolean(this.MyFunctions.MyDevices.BitAccessByBitName["RAM;CFG_HH"].GetBitValue(this.Map));
          bool boolean2 = Convert.ToBoolean(this.MyFunctions.MyDevices.BitAccessByBitName["RAM;CFG_Radio3"].GetBitValue(this.Map));
          byte num = Convert.ToByte(this.MyFunctions.MyDevices.ParameterAccessByName["RAM;RF_Type_Info"].GetReadValue(this.Map));
          bool flag1 = (24 & (int) num) == 0;
          bool flag2 = (16 & (int) num) == 0 && (8 & (int) num) == 8;
          if (flag1)
            MinotelContactRadio3.logger.Error("Wrong coding type detected. RF_Type_Info= 0x" + num.ToString("X2"));
          else if (!flag2)
            MinotelContactRadio3.logger.Error("Unknown coding type detected. RF_Type_Info= 0x" + num.ToString("X2"));
          if (!boolean2)
            return (object) RadioProtocol.Scenario0;
          if ((224 & (int) num) == 0 && !boolean1)
            return (object) RadioProtocol.Scenario1;
          if ((224 & (int) num) == 32 && !boolean1)
            return (object) RadioProtocol.Scenario2;
          if ((224 & (int) num) == 0 & boolean1)
            return (object) RadioProtocol.Scenario3;
          if ((224 & (int) num) == 64)
          {
            switch (this.GetRadioFrequence())
            {
              case RadioFrequence.Europa_868_3_MHz:
                return (object) RadioProtocol.Scenario6;
              case RadioFrequence.Russia_868_95_MHz:
                return (object) RadioProtocol.Scenario5;
            }
          }
          MinotelContactRadio3.logger.Error("Unknown scenario detected. RF_Type_Info= 0x" + num.ToString("X2") + " and CFG_HH=" + boolean1.ToString());
          return (object) RadioProtocol.Undefined;
        default:
          if (!this.MyFunctions.MyDevices.ParameterAccessByName.ContainsKey(parameterKey))
            throw new ArgumentException("Unknown parameter! Key: " + parameterKey);
          return this.MyFunctions.MyDevices.ParameterAccessByName[parameterKey].GetReadValue(this.Map);
      }
    }

    internal override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.Radio3))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for radio3!");
        return false;
      }
      if (this.SelectedDevice != null)
      {
        object parameterValue1 = this.GetConfigurationParameters(ConfigurationParameter.ValueType.Ident, 1)[OverrideID.SerialNumber].ParameterValue;
        if (parameterValue1 != null && parameterValue1.ToString() == this.SelectedDevice.Serialnumber)
          return this.GetValues(ref ValueList, 1);
        object parameterValue2 = this.GetConfigurationParameters(ConfigurationParameter.ValueType.Ident, 2)[OverrideID.SerialNumber].ParameterValue;
        if (parameterValue2 != null && parameterValue2.ToString() == this.SelectedDevice.Serialnumber)
          return this.GetValues(ref ValueList, 2);
      }
      return this.GetValues(ref ValueList, 0);
    }

    internal override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      int SubDevice)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.Radio3))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for radio3!");
        return false;
      }
      if (SubDevice < 0 || SubDevice > 2)
        throw new Exception("MinotelContact: Illegal sub device number");
      if (SubDevice == 0)
        return true;
      List<long> filter = (List<long>) null;
      if (ValueList == null)
        ValueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      else if (ValueList.Count > 0)
      {
        filter = new List<long>();
        filter.AddRange((IEnumerable<long>) ValueList.Keys);
      }
      SortedList<DeviceCollectorSettings, object> collectorSettings = this.MyFunctions.MyBus.GetDeviceCollectorSettings();
      DateTime dateTime1 = Convert.ToDateTime(collectorSettings[DeviceCollectorSettings.FromTime]);
      DateTime dateTime2 = Convert.ToDateTime(collectorSettings[DeviceCollectorSettings.ToTime]);
      long num1 = 272700106;
      if (ValueIdent.IsExpectedValueIdent(filter, num1))
      {
        object readValue1 = this.GetReadValue("RAM;DateToday");
        if (readValue1 != null)
        {
          DateTime timePoint = (DateTime) readValue1;
          if (timePoint >= dateTime1 && timePoint <= dateTime2)
          {
            object readValue2 = this.GetReadValue("RAM;Reading" + MinotelContactRadio3.ReadingNameExtention[SubDevice]);
            if (readValue2 != null)
              this.AddValue(ref ValueList, timePoint, num1, readValue2);
          }
        }
      }
      long num2 = 281088714;
      if (ValueIdent.IsExpectedValueIdent(filter, num2))
      {
        for (int index = 0; index < 2; ++index)
        {
          object readValue3 = this.GetReadValue("LOGM" + MinotelContactRadio3.ReadingNameExtention[SubDevice] + ";DateStampY" + index.ToString("00"));
          if (readValue3 != null)
          {
            DateTime timePoint = (DateTime) readValue3;
            if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
            {
              object readValue4 = this.GetReadValue("LOGM" + MinotelContactRadio3.ReadingNameExtention[SubDevice] + ";ReadingY" + index.ToString("00") + MinotelContactRadio3.ReadingNameExtention[SubDevice]);
              if (readValue4 != null)
                this.AddValue(ref ValueList, timePoint, num2, readValue4);
            }
          }
        }
      }
      long num3 = 289477322;
      if (ValueIdent.IsExpectedValueIdent(filter, num3))
      {
        for (int index = 1; index <= 18; ++index)
        {
          object readValue5 = this.GetReadValue("LOGM" + MinotelContactRadio3.ReadingNameExtention[SubDevice] + ";DateStampM" + index.ToString("00"));
          if (readValue5 != null)
          {
            DateTime timePoint = (DateTime) readValue5;
            if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
            {
              object readValue6 = this.GetReadValue("LOGM" + MinotelContactRadio3.ReadingNameExtention[SubDevice] + ";ReadingM" + index.ToString("00") + MinotelContactRadio3.ReadingNameExtention[SubDevice]);
              if (readValue6 != null)
                this.AddValue(ref ValueList, timePoint, num3, readValue6);
            }
          }
        }
      }
      long num4 = 293671626;
      if (ValueIdent.IsExpectedValueIdent(filter, num4))
      {
        object readValue7 = this.GetReadValue("LOGM" + MinotelContactRadio3.ReadingNameExtention[SubDevice] + ";DateStampM00");
        if (readValue7 != null)
        {
          DateTime timePoint = (DateTime) readValue7;
          if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
          {
            object readValue8 = this.GetReadValue("LOGM" + MinotelContactRadio3.ReadingNameExtention[SubDevice] + ";ReadingM00" + MinotelContactRadio3.ReadingNameExtention[SubDevice]);
            if (readValue8 != null)
              this.AddValue(ref ValueList, timePoint, num4, readValue8);
          }
        }
      }
      long num5 = 302060234;
      if (ValueIdent.IsExpectedValueIdent(filter, num5))
      {
        for (int index = 0; index <= 31; ++index)
        {
          object readValue9 = this.GetReadValue("LOGD" + MinotelContactRadio3.ReadingNameExtention[SubDevice] + ";DateStampD" + index.ToString("00"));
          if (readValue9 != null)
          {
            DateTime timePoint = (DateTime) readValue9;
            if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
            {
              object readValue10 = this.GetReadValue("LOGD" + MinotelContactRadio3.ReadingNameExtention[SubDevice] + ";ReadingD" + index.ToString("00") + MinotelContactRadio3.ReadingNameExtention[SubDevice]);
              if (readValue10 != null)
                this.AddValue(ref ValueList, timePoint, num5, readValue10);
            }
          }
        }
      }
      object readValue11 = this.GetReadValue("RAM;ERROR");
      if (readValue11 != null && Convert.ToBoolean(readValue11))
      {
        object readValue12 = this.GetReadValue("RAM;DateError");
        DateTime dateTime3;
        if (readValue12 != null && Util.TryParseToDateTime(readValue12.ToString(), out dateTime3))
        {
          this.AddErrorValue(ValueList, ValueIdent.ValueIdPart_MeterType.PulseCounter, ValueIdent.ValueIdentError.DeviceError, new DateTime?(dateTime3));
        }
        else
        {
          DateTime now = DateTime.Now;
          this.AddErrorValue(ValueList, ValueIdent.ValueIdPart_MeterType.PulseCounter, ValueIdent.ValueIdentError.DeviceError, new DateTime?(new DateTime(now.Year, now.Month, now.Day)));
        }
      }
      return true;
    }

    internal override GlobalDeviceId GetGlobalDeviceId()
    {
      if (!UserManager.CheckPermission(UserRights.Rights.Radio3))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for radio3!");
        return (GlobalDeviceId) null;
      }
      GlobalDeviceId globalDeviceId1 = new GlobalDeviceId()
      {
        Serialnumber = this.SerialNumber.ToString(),
        MeterType = ValueIdent.ValueIdPart_MeterType.PulseCounter
      };
      globalDeviceId1.DeviceTypeName = globalDeviceId1.MeterType.ToString();
      globalDeviceId1.Manufacturer = "MINOL";
      globalDeviceId1.MeterNumber = this.SerialNumber.ToString();
      globalDeviceId1.FirmwareVersion = this.FirmwareVersion.ToString();
      globalDeviceId1.Generation = "3";
      globalDeviceId1.SubDevices = new List<GlobalDeviceId>();
      SortedList<OverrideID, ConfigurationParameter> configurationParameters1 = this.GetConfigurationParameters(ConfigurationParameter.ValueType.Ident, 1);
      if (configurationParameters1 != null)
      {
        GlobalDeviceId globalDeviceId2 = new GlobalDeviceId();
        object parameterValue = configurationParameters1[OverrideID.SerialNumber].ParameterValue;
        if (parameterValue != null)
          globalDeviceId2.Serialnumber = parameterValue.ToString();
        globalDeviceId2.MeterType = ValueIdent.ValueIdPart_MeterType.PulseCounter;
        globalDeviceId2.DeviceTypeName = ValueIdent.GetTranslatedValueNameForPartOfValueId((Enum) globalDeviceId2.MeterType);
        globalDeviceId2.Manufacturer = "MINOL";
        globalDeviceId2.MeterNumber = string.Empty;
        globalDeviceId2.FirmwareVersion = this.FirmwareVersion.ToString();
        globalDeviceId2.Generation = "3";
        globalDeviceId1.SubDevices.Add(globalDeviceId2);
      }
      SortedList<OverrideID, ConfigurationParameter> configurationParameters2 = this.GetConfigurationParameters(ConfigurationParameter.ValueType.Ident, 2);
      if (configurationParameters2 != null)
      {
        GlobalDeviceId globalDeviceId3 = new GlobalDeviceId();
        object parameterValue = configurationParameters2[OverrideID.SerialNumber].ParameterValue;
        if (parameterValue != null)
          globalDeviceId3.Serialnumber = parameterValue.ToString();
        globalDeviceId3.MeterType = ValueIdent.ValueIdPart_MeterType.PulseCounter;
        globalDeviceId3.DeviceTypeName = ValueIdent.GetTranslatedValueNameForPartOfValueId((Enum) globalDeviceId3.MeterType);
        globalDeviceId3.Manufacturer = "MINOL";
        globalDeviceId3.MeterNumber = string.Empty;
        globalDeviceId3.FirmwareVersion = this.FirmwareVersion.ToString();
        globalDeviceId3.Generation = "3";
        globalDeviceId1.SubDevices.Add(globalDeviceId3);
      }
      return globalDeviceId1;
    }

    internal override bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> ParameterList,
      int SubDevice)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.Radio3))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for radio3!");
        return false;
      }
      if (SubDevice > 2)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "MinotelContact can have a maximum 2 sub devices!");
        return false;
      }
      foreach (KeyValuePair<OverrideID, ConfigurationParameter> parameter in ParameterList)
      {
        string parameterKey = parameter.Value.ParameterKey;
        object parameterValue = parameter.Value.ParameterValue;
        if (parameterKey == "RAM;DateToday" || parameter.Value.ParameterID == OverrideID.DeviceClock)
        {
          if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change the device clock!");
            continue;
          }
          ParameterAccess parameterAccess1 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Minute"];
          this.SetConfigurationParameters(parameterAccess1.TelegramPara.Address, parameterAccess1.GetValueAsByteArray(parameterValue));
          ParameterAccess parameterAccess2 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Hour"];
          this.SetConfigurationParameters(parameterAccess2.TelegramPara.Address, parameterAccess2.GetValueAsByteArray(parameterValue));
          ParameterAccess parameterAccess3 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Second"];
          this.SetConfigurationParameters(parameterAccess3.TelegramPara.Address, parameterAccess3.GetValueAsByteArray(parameterValue));
          ParameterAccess parameterAccess4 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;DateToday"];
          this.SetConfigurationParameters(parameterAccess4.TelegramPara.Address, parameterAccess4.GetValueAsByteArray(parameterValue));
        }
        else
        {
          int num;
          switch (parameterKey)
          {
            case "RAM;K_A_Value":
              ParameterAccess parameterAccess5 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;K_A"];
              int address1 = parameterAccess5.TelegramPara.Address;
              if (this.input1_PulseValue == -1L)
                this.input1_PulseValue = Util.ToLong(parameterAccess5.GetReadValue(this.Map));
              this.input1_PulseValue = Util.ToLong(parameterValue) << 4 | this.input1_PulseValue & 15L;
              byte[] byteArray1 = Util.ConvertLongToByteArray(this.input1_PulseValue, parameterAccess5.TelegramPara.ByteLength);
              this.SetConfigurationParameters(address1, byteArray1);
              goto label_53;
            case "RAM;K_B_Value":
              ParameterAccess parameterAccess6 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;K_B"];
              int address2 = parameterAccess6.TelegramPara.Address;
              if (this.input2_PulseValue == -1L)
                this.input2_PulseValue = Util.ToLong(parameterAccess6.GetReadValue(this.Map));
              this.input2_PulseValue = Util.ToLong(parameterValue) << 4 | this.input2_PulseValue & 15L;
              byte[] byteArray2 = Util.ConvertLongToByteArray(this.input2_PulseValue, parameterAccess6.TelegramPara.ByteLength);
              this.SetConfigurationParameters(address2, byteArray2);
              goto label_53;
            case "RAM;K_A_Unit":
              ParameterAccess parameterAccess7 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;K_A"];
              int address3 = parameterAccess7.TelegramPara.Address;
              if (this.input1_PulseValue == -1L)
                this.input1_PulseValue = Util.ToLong(parameterAccess7.GetReadValue(this.Map));
              this.input1_PulseValue = this.input1_PulseValue & 65520L | Util.ToLong((long) this.ConvertInputUnitsIndexToMinolUnit(parameterValue.ToString()));
              byte[] byteArray3 = Util.ConvertLongToByteArray(this.input1_PulseValue, parameterAccess7.TelegramPara.ByteLength);
              this.SetConfigurationParameters(address3, byteArray3);
              goto label_53;
            case "RAM;K_B_Unit":
              ParameterAccess parameterAccess8 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;K_B"];
              int address4 = parameterAccess8.TelegramPara.Address;
              if (this.input2_PulseValue == -1L)
                this.input2_PulseValue = Util.ToLong(parameterAccess8.GetReadValue(this.Map));
              this.input2_PulseValue = this.input2_PulseValue & 65520L | Util.ToLong((long) this.ConvertInputUnitsIndexToMinolUnit(parameterValue.ToString()));
              byte[] byteArray4 = Util.ConvertLongToByteArray(this.input2_PulseValue, parameterAccess8.TelegramPara.ByteLength);
              this.SetConfigurationParameters(address4, byteArray4);
              goto label_53;
            case "RAM;INP_SampleTime":
              this.SetReadValue("RAM;INP_SampleTime", (object) Convert.ToByte(parameterValue));
              goto label_53;
            case "INFO;Radio2_Preload":
              this.SetReadValue("INFO;Radio2_Preload", (object) (Convert.ToInt32(parameterValue) / 2));
              goto label_53;
            case "INFO;Radio3_Preload":
              this.SetReadValue("INFO;Radio3_Preload", (object) (Convert.ToInt32(parameterValue) / 2));
              goto label_53;
            case "INFO;Radio3_Preload_B":
              this.SetReadValue("INFO;Radio3_Preload_B", (object) (Convert.ToInt32(parameterValue) / 2));
              goto label_53;
            case "INFO;RadioEpsilonOffsetEnabled":
              num = 1;
              break;
            default:
              num = parameter.Value.ParameterID == OverrideID.RadioEpsilonOffsetEnabled ? 1 : 0;
              break;
          }
          if (num != 0)
          {
            if (!(parameterValue is bool flag))
              flag = bool.Parse(parameterValue.ToString());
            if (flag)
              this.SetReadValue("INFO;Radio3_Epsilon", (object) MinolDevice.CalculateRadio3EpsilonValue(Convert.ToInt32(this.MyFunctions.MyDevices.ParameterAccessByName["INFO;RFID_A"].GetReadValue(this.Map))));
            else
              this.SetReadValue("INFO;Radio3_Epsilon", (object) (int) ushort.MaxValue);
          }
          else if (parameterKey == "RAM;RadioProtocol" || parameter.Value.ParameterID == OverrideID.RadioProtocol)
          {
            if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet3))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change the radio protocol!");
              continue;
            }
            if (!Enum.IsDefined(typeof (RadioProtocol), parameterValue))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Unknown scenario! Value: " + parameterValue?.ToString());
              continue;
            }
            switch ((RadioProtocol) Enum.Parse(typeof (RadioProtocol), parameterValue.ToString(), true))
            {
              case RadioProtocol.Scenario0:
                this.SetReadValue("RAM;CFG_HH", (object) false);
                this.SetReadValue("RAM;CFG_Radio3", (object) false);
                this.SetReadValue("RAM;RF_Type_Info", (object) 8);
                this.SetReadValue("RAM;TIMER_RF", (object) 10);
                break;
              case RadioProtocol.Scenario1:
                this.SetReadValue("RAM;CFG_HH", (object) false);
                this.SetReadValue("RAM;CFG_Radio3", (object) true);
                this.SetReadValue("RAM;RF_Type_Info", (object) 8);
                this.SetReadValue("RAM;TIMER_RF", (object) 10);
                this.ClearSavedLoggerValues();
                break;
              case RadioProtocol.Scenario2:
                this.SetReadValue("RAM;CFG_HH", (object) false);
                this.SetReadValue("RAM;CFG_Radio3", (object) true);
                this.SetReadValue("RAM;RF_Type_Info", (object) 40);
                this.SetReadValue("RAM;TIMER_RF", (object) 10);
                this.ClearSavedLoggerValues();
                break;
              case RadioProtocol.Scenario3:
                this.SetReadValue("RAM;CFG_HH", (object) true);
                this.SetReadValue("RAM;CFG_Radio3", (object) true);
                this.SetReadValue("RAM;RF_Type_Info", (object) 8);
                this.SetReadValue("RAM;TIMER_RF", (object) 10);
                break;
              case RadioProtocol.Undefined:
                break;
              default:
                MinotelContactRadio3.logger.Fatal("Unknown RadioMode=" + ((RadioProtocol) parameterValue).ToString());
                break;
            }
          }
          else if (parameterKey == "RAM;SLEEP" || parameter.Value.ParameterID == OverrideID.SleepMode)
          {
            if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet4))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change sleep parameter of the device!");
              continue;
            }
            if (Convert.ToBoolean(parameterValue))
              this.SetReadValue("RAM;DateOn", (object) DateTime.MinValue);
            this.SetReadValue(parameterKey, parameterValue);
          }
          else
            this.SetReadValue(parameterKey, parameterValue);
        }
label_53:;
      }
      return true;
    }

    protected void ClearSavedLoggerValues()
    {
      for (int index = 0; index <= 31; ++index)
      {
        this.SetReadValue("LOGD_A;DateStampD" + index.ToString("00"), (object) DateTime.MinValue);
        this.SetReadValue("LOGD_A;ReadingD" + index.ToString("00") + "_A", (object) uint.MaxValue);
        this.SetReadValue("LOGD_B;DateStampD" + index.ToString("00"), (object) DateTime.MinValue);
        this.SetReadValue("LOGD_B;ReadingD" + index.ToString("00") + "_B", (object) uint.MaxValue);
      }
      for (int index = 0; index <= 31; ++index)
      {
        this.SetReadValue("LOGH0;DateStamp" + index.ToString("00"), (object) DateTime.MinValue);
        this.SetReadValue("LOGH0;Reading" + index.ToString("00"), (object) uint.MaxValue);
      }
      for (int index = 32; index <= 63; ++index)
      {
        this.SetReadValue("LOGH1;DateStamp" + index.ToString("00"), (object) DateTime.MinValue);
        this.SetReadValue("LOGH1;Reading" + index.ToString("00"), (object) uint.MaxValue);
      }
      for (int index = 64; index <= 95; ++index)
      {
        this.SetReadValue("LOGH2;DateStamp" + index.ToString("00"), (object) DateTime.MinValue);
        this.SetReadValue("LOGH2;Reading" + index.ToString("00"), (object) uint.MaxValue);
      }
    }

    internal override SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType,
      int SubDevice)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.Radio3))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for radio3!");
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      }
      if (SubDevice > 2)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "MinotelContact can have a maximum 2 sub devices!");
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      }
      try
      {
        this.ParameterList = new SortedList<OverrideID, ConfigurationParameter>();
        bool hasWritePermission1 = UserManager.CheckPermission(UserRights.Rights.MConfigSet1);
        bool hasWritePermission2 = UserManager.CheckPermission(UserRights.Rights.MConfigSet2);
        bool hasWritePermission3 = UserManager.CheckPermission(UserRights.Rights.MConfigSet3);
        bool hasWritePermission4 = UserManager.CheckPermission(UserRights.Rights.MConfigSet4);
        RadioProtocol readValue = (RadioProtocol) this.GetReadValue("RAM;RadioProtocol");
        bool boolean = Convert.ToBoolean(this.MyFunctions.MyDevices.BitAccessByBitName["RAM;CFG_Radio3"].GetBitValue(this.Map));
        switch (SubDevice)
        {
          case 0:
            this.AddParameter(0, OverrideID.DeviceName);
            this.AddParameter(0, OverrideID.FirmwareVersion);
            this.AddParameter(0, OverrideID.Signature);
            this.AddParameter(0, OverrideID.Manufacturer);
            this.AddParameter(false, 0, OverrideID.SerialNumber, "INFO;SerNo");
            if (ConfigurationType == ConfigurationParameter.ValueType.Direct || ConfigurationType == ConfigurationParameter.ValueType.Complete)
            {
              this.AddParameter(true, 0, OverrideID.DueDate, "RAM;DateKey", hasWritePermission1);
              this.AddParameter(true, 0, OverrideID.DeviceClock, "RAM;DateToday", hasWritePermission1);
              this.AddParameter(true, 0, OverrideID.DeviceHasError, "RAM;ERROR", hasWritePermission4);
              this.AddParameter(true, 0, OverrideID.ErrorDate, "RAM;DateError", hasWritePermission4);
              this.AddParameter(true, 0, OverrideID.RadioProtocol, "RAM;RadioProtocol", hasWritePermission3);
              this.AddParameter(true, 0, OverrideID.RadioEpsilonOffsetEnabled, "INFO;RadioEpsilonOffsetEnabled", hasWritePermission4);
              this.AddParameter(true, 0, OverrideID.SleepMode, "RAM;SLEEP", hasWritePermission4);
              this.AddParameter(true, 0, OverrideID.InputSampleTime, "RAM;INP_SampleTime", hasWritePermission4);
              this.AddParameter(false, OverrideID.RadioFrequence, new ConfigurationParameter(OverrideID.RadioFrequence, (object) this.GetRadioFrequence())
              {
                HasWritePermission = false
              });
              this.AddParameter(false, OverrideID.NumberOfSubDevices, new ConfigurationParameter(OverrideID.NumberOfSubDevices)
              {
                ParameterValue = (object) 2UL
              });
              string str = string.Format("ID:{0} Manufacturer:0x{1:X4} Status:0x{2} Signature:0x{3:X4} CFG_RAM:0x{4} Flags:0x{5} CFG_Flags:0x{6}", (object) this.SerialNumber, (object) this.Manufacturer, (object) this.GetParameterDataHexString("RAM;Status"), (object) this.Signature, (object) this.GetParameterDataHexString("RAM;CFG_RAM"), (object) this.GetParameterDataHexString("RAM;Flags"), (object) this.GetParameterDataHexString("INFO;CFG_Flags"));
              this.AddParameter(false, OverrideID.DiagnosticString, new ConfigurationParameter(OverrideID.DiagnosticString)
              {
                ParameterValue = (object) str,
                HasWritePermission = false,
                IsFunction = false
              });
              break;
            }
            break;
          case 1:
            this.AddParameter(false, 1, OverrideID.SerialNumber, "INFO;RFID_A");
            this.AddParameter(true, 1, OverrideID.InputResolution, "RAM;K_A_Unit", hasWritePermission2);
            this.AddParameter(true, 1, OverrideID.InputPulsValue, "RAM;K_A_Value", hasWritePermission2);
            string physicalUnit1 = this.ParameterList[OverrideID.InputResolution].ParameterValue.ToString();
            this.AddParameter(true, 1, OverrideID.InputActualValue, "RAM;Reading_A", hasWritePermission2, physicalUnit1);
            if (boolean)
            {
              this.AddParameter(true, 1, OverrideID.RadioSendInterval, "INFO;Radio3_Preload", hasWritePermission4, "s", (string[]) null, (object) 16, (object) 131070);
              break;
            }
            this.AddParameter(true, 1, OverrideID.RadioSendInterval, "INFO;Radio2_Preload", hasWritePermission4, "s", (string[]) null, (object) 300, (object) 300);
            break;
          case 2:
            this.AddParameter(false, 2, OverrideID.SerialNumber, "INFO;RFID_B");
            this.AddParameter(true, 2, OverrideID.InputResolution, "RAM;K_B_Unit", hasWritePermission2);
            this.AddParameter(true, 2, OverrideID.InputPulsValue, "RAM;K_B_Value", hasWritePermission2);
            this.AddParameter(true, 2, OverrideID.RadioEnabled, "RAM;CFG_RF_B", hasWritePermission2);
            string physicalUnit2 = this.ParameterList[OverrideID.InputResolution].ParameterValue.ToString();
            this.AddParameter(true, 2, OverrideID.InputActualValue, "RAM;Reading_B", hasWritePermission2, physicalUnit2);
            if (boolean)
              this.AddParameter(true, 2, OverrideID.RadioSendOffset, "INFO;Radio3_Preload_B", hasWritePermission4, "s", (string[]) null, (object) 2, (object) 20);
            break;
        }
        this.input1_PulseValue = -1L;
        this.input2_PulseValue = -1L;
        return this.ParameterList;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Parameter generation error. " + ex.Message);
        return this.ParameterList;
      }
    }

    private RadioFrequence GetRadioFrequence()
    {
      long num1 = !Convert.ToBoolean(this.MyFunctions.MyDevices.BitAccessByBitName["RAM;CFG_Radio3"].GetBitValue(this.Map)) ? (long) this.GetReadValue("INFO;Radio2_ADF7012_N") : (long) this.GetReadValue("INFO;Radio3_ADF7012_N");
      int num2;
      switch (num1)
      {
        case 1929557:
          num2 = 1;
          break;
        case 1931001:
          return RadioFrequence.Russia_868_95_MHz;
        default:
          num2 = num1 == 6122613L ? 1 : 0;
          break;
      }
      if (num2 != 0)
        return RadioFrequence.Europa_868_3_MHz;
      throw new ArgumentException("Unknown radio frequence detected: Value: 0x" + num1.ToString("X8"));
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: MinolHandler.AquaMicroRadio3
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
  internal class AquaMicroRadio3 : MinolDevice
  {
    private static Logger logger = LogManager.GetLogger(nameof (AquaMicroRadio3));
    private long complexPulseValue = -1;

    internal AquaMicroRadio3(MinolHandlerFunctions Functions)
      : base(Functions)
    {
      this.DeviceType = DeviceTypes.AquaMicroRadio3;
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
      if (!UserManager.CheckPermission(UserRights.Rights.Radio3))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for radio3!");
        return (object) null;
      }
      if (this.MyFunctions.MyDevices.BitAccessByBitName.ContainsKey(parameterKey))
        return (object) this.MyFunctions.MyDevices.BitAccessByBitName[parameterKey].GetBitValue(this.Map);
      switch (parameterKey)
      {
        case "RAM;DateToday":
          DateTime dateTime = (DateTime) this.MyFunctions.MyDevices.ParameterAccessByName["RAM;DateToday"].GetReadValue(this.Map);
          long readValue1 = (long) this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Minute"].GetReadValue(this.Map);
          dateTime = dateTime.AddMinutes((double) readValue1);
          long readValue2 = (long) this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Hour"].GetReadValue(this.Map);
          return (object) dateTime.AddHours((double) readValue2).AddSeconds((double) (long) this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Second"].GetReadValue(this.Map));
        case "INFO;K_Unit":
          return (object) DeviceCollector.MinolDevice.ConvertMinolUnitToInputUnitsIndex((byte) Util.ToLong(this.MyFunctions.MyDevices.ParameterAccessByName["INFO;K"].GetReadValue(this.Map)));
        case "INFO;K_Value":
          return (object) Util.ToDouble((object) (Util.ToLong(this.MyFunctions.MyDevices.ParameterAccessByName["INFO;K"].GetReadValue(this.Map)) >> 4));
        case "INFO;Radio2_Preload":
          return (object) (Util.ToInteger(this.MyFunctions.MyDevices.ParameterAccessByName["INFO;Radio2_Preload"].GetReadValue(this.Map)) * 2);
        case "INFO;Radio3_Preload":
          return (object) (Util.ToInteger(this.MyFunctions.MyDevices.ParameterAccessByName["INFO;Radio3_Preload"].GetReadValue(this.Map)) * 2);
        case "INFO;RadioEpsilonOffsetEnabled":
          int integer = Util.ToInteger(this.MyFunctions.MyDevices.ParameterAccessByName["INFO;Radio3_Epsilon"].GetReadValue(this.Map));
          int radio3EpsilonValue = MinolDevice.CalculateRadio3EpsilonValue(this.SerialNumber);
          if (radio3EpsilonValue == integer)
            return (object) true;
          if (integer != 0 && integer != (int) ushort.MaxValue)
            AquaMicroRadio3.logger.Warn<int, int, int>("Wrong radio 3 epsilon value! FunkID={0}, Expected=0x{1:X4}, Actual=0x{2:X4}", this.SerialNumber, radio3EpsilonValue, integer);
          return (object) false;
        case "RAM;RadioProtocol":
          bool boolean1 = Convert.ToBoolean(this.MyFunctions.MyDevices.BitAccessByBitName["RAM;CFG_HH"].GetBitValue(this.Map));
          bool boolean2 = Convert.ToBoolean(this.MyFunctions.MyDevices.BitAccessByBitName["RAM;CFG_Radio3"].GetBitValue(this.Map));
          byte num = Convert.ToByte(this.MyFunctions.MyDevices.ParameterAccessByName["RAM;RF_Type_Info"].GetReadValue(this.Map));
          bool flag1 = (24 & (int) num) == 0;
          bool flag2 = (16 & (int) num) == 0 && (8 & (int) num) == 8;
          if (flag1)
            AquaMicroRadio3.logger.Error("Wrong coding type detected. RF_Type_Info= 0x" + num.ToString("X2"));
          else if (!flag2)
            AquaMicroRadio3.logger.Error("Unknown coding type detected. RF_Type_Info= 0x" + num.ToString("X2"));
          if (!boolean2)
            return (object) RadioProtocol.Scenario0;
          if ((224 & (int) num) == 0 && !boolean1)
            return (object) RadioProtocol.Scenario1;
          if ((192 & (int) num) == 0 && (32 & (int) num) == 32 && !boolean1)
            return (object) RadioProtocol.Scenario2;
          if ((224 & (int) num) == 0 & boolean1)
            return (object) RadioProtocol.Scenario3;
          if ((64 & (int) num) == 64)
          {
            switch (this.GetRadioFrequence())
            {
              case RadioFrequence.Europa_868_3_MHz:
                return (object) RadioProtocol.Scenario6;
              case RadioFrequence.Russia_868_95_MHz:
                return (object) RadioProtocol.Scenario5;
            }
          }
          AquaMicroRadio3.logger.Error("Unknown scenario detected. RF_Type_Info= 0x" + num.ToString("X2") + " and CFG_HH=" + boolean1.ToString());
          return (object) RadioProtocol.Undefined;
        default:
          if (!this.MyFunctions.MyDevices.ParameterAccessByName.ContainsKey(parameterKey))
            throw new ArgumentException("Unknown parameter! Key: " + parameterKey);
          return this.MyFunctions.MyDevices.ParameterAccessByName[parameterKey].GetReadValue(this.Map);
      }
    }

    internal override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      int SubDevice)
    {
      if (SubDevice != 0)
        throw new ArgumentOutOfRangeException(nameof (SubDevice), "AquaMicroRadio3 has no sub devices!");
      return this.GetValues(ref ValueList);
    }

    internal override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.Developer) && !UserManager.CheckPermission(UserRights.Rights.Radio3))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for radio3!");
        return false;
      }
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
      Decimal num1 = Convert.ToDecimal(this.GetReadValue("INFO;K_Value"));
      long num2 = 272699457;
      if (ValueIdent.IsExpectedValueIdent(filter, num2))
      {
        object readValue1 = this.GetReadValue("RAM;DateToday");
        if (readValue1 != null)
        {
          DateTime timePoint = (DateTime) readValue1;
          if (timePoint >= dateTime1 && timePoint <= dateTime2)
          {
            object readValue2 = this.GetReadValue("RAM;Reading");
            if (readValue2 != null)
            {
              Decimal num3 = Convert.ToDecimal(readValue2);
              this.AddValue(ref ValueList, timePoint, num2, (object) (num3 * num1 / 1000M));
            }
          }
        }
      }
      long num4 = 281088065;
      if (ValueIdent.IsExpectedValueIdent(filter, num4))
      {
        for (int index = 0; index < 2; ++index)
        {
          object readValue3 = this.GetReadValue("LOGM;DateStampY" + index.ToString("00"));
          if (readValue3 != null)
          {
            DateTime timePoint = (DateTime) readValue3;
            if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
            {
              object readValue4 = this.GetReadValue("LOGM;ReadingY" + index.ToString("00"));
              if (readValue4 != null)
              {
                Decimal num5 = Convert.ToDecimal(readValue4);
                this.AddValue(ref ValueList, timePoint, num4, (object) (num5 * num1 / 1000M));
              }
            }
          }
        }
      }
      long num6 = 289476673;
      if (ValueIdent.IsExpectedValueIdent(filter, num6))
      {
        for (int index = 1; index <= 18; ++index)
        {
          object readValue5 = this.GetReadValue("LOGM;DateStampM" + index.ToString("00"));
          if (readValue5 != null)
          {
            DateTime timePoint = (DateTime) readValue5;
            if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
            {
              object readValue6 = this.GetReadValue("LOGM;ReadingM" + index.ToString("00"));
              if (readValue6 != null)
              {
                Decimal num7 = Convert.ToDecimal(readValue6);
                this.AddValue(ref ValueList, timePoint, num6, (object) (num7 * num1 / 1000M));
              }
            }
          }
        }
      }
      long num8 = 293670977;
      if (ValueIdent.IsExpectedValueIdent(filter, num8))
      {
        object readValue7 = this.GetReadValue("LOGM;DateStampM00");
        if (readValue7 != null)
        {
          DateTime timePoint = (DateTime) readValue7;
          if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
          {
            object readValue8 = this.GetReadValue("LOGM;ReadingM00");
            if (readValue8 != null)
            {
              Decimal num9 = Convert.ToDecimal(readValue8);
              this.AddValue(ref ValueList, timePoint, num8, (object) (num9 * num1 / 1000M));
            }
          }
        }
      }
      long num10 = 302059585;
      if (ValueIdent.IsExpectedValueIdent(filter, num10))
      {
        for (int index = 0; index <= 31; ++index)
        {
          object readValue9 = this.GetReadValue("LOGD;DateStampD" + index.ToString("00"));
          if (readValue9 != null)
          {
            DateTime timePoint = (DateTime) readValue9;
            if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
            {
              object readValue10 = this.GetReadValue("LOGD;ReadingD" + index.ToString("00"));
              if (readValue10 != null)
              {
                Decimal num11 = Convert.ToDecimal(readValue10);
                this.AddValue(ref ValueList, timePoint, num10, (object) (num11 * num1 / 1000M));
              }
            }
          }
        }
      }
      long num12 = 272700106;
      if (ValueIdent.IsExpectedValueIdent(filter, num12))
      {
        object readValue11 = this.GetReadValue("RAM;DateToday");
        if (readValue11 != null)
        {
          DateTime timePoint = (DateTime) readValue11;
          if (timePoint >= dateTime1 && timePoint <= dateTime2)
          {
            object readValue12 = this.GetReadValue("RAM;Reading");
            if (readValue12 != null)
              this.AddValue(ref ValueList, timePoint, num12, readValue12);
          }
        }
      }
      long num13 = 281088714;
      if (ValueIdent.IsExpectedValueIdent(filter, num13))
      {
        for (int index = 0; index < 2; ++index)
        {
          object readValue13 = this.GetReadValue("LOGM;DateStampY" + index.ToString("00"));
          if (readValue13 != null)
          {
            DateTime timePoint = (DateTime) readValue13;
            if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
            {
              object readValue14 = this.GetReadValue("LOGM;ReadingY" + index.ToString("00"));
              if (readValue14 != null)
                this.AddValue(ref ValueList, timePoint, num13, readValue14);
            }
          }
        }
      }
      long num14 = 289477322;
      if (ValueIdent.IsExpectedValueIdent(filter, num14))
      {
        for (int index = 1; index <= 18; ++index)
        {
          object readValue15 = this.GetReadValue("LOGM;DateStampM" + index.ToString("00"));
          if (readValue15 != null)
          {
            DateTime timePoint = (DateTime) readValue15;
            if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
            {
              object readValue16 = this.GetReadValue("LOGM;ReadingM" + index.ToString("00"));
              if (readValue16 != null)
                this.AddValue(ref ValueList, timePoint, num14, readValue16);
            }
          }
        }
      }
      long num15 = 293671626;
      if (ValueIdent.IsExpectedValueIdent(filter, num15))
      {
        object readValue17 = this.GetReadValue("LOGM;DateStampM00");
        if (readValue17 != null)
        {
          DateTime timePoint = (DateTime) readValue17;
          if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
          {
            object readValue18 = this.GetReadValue("LOGM;ReadingM00");
            if (readValue18 != null)
              this.AddValue(ref ValueList, timePoint, num15, readValue18);
          }
        }
      }
      long num16 = 302060234;
      if (ValueIdent.IsExpectedValueIdent(filter, num16))
      {
        for (int index = 0; index <= 31; ++index)
        {
          object readValue19 = this.GetReadValue("LOGD;DateStampD" + index.ToString("00"));
          if (readValue19 != null)
          {
            DateTime timePoint = (DateTime) readValue19;
            if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
            {
              object readValue20 = this.GetReadValue("LOGD;ReadingD" + index.ToString("00"));
              if (readValue20 != null)
                this.AddValue(ref ValueList, timePoint, num16, readValue20);
            }
          }
        }
      }
      object readValue21 = this.GetReadValue("RAM;ERROR");
      if (readValue21 != null && Convert.ToBoolean(readValue21))
      {
        object readValue22 = this.GetReadValue("RAM;DateError");
        DateTime dateTime3;
        if (readValue22 != null && Util.TryParseToDateTime(readValue22.ToString(), out dateTime3))
        {
          this.AddErrorValue(ValueList, ValueIdent.ValueIdPart_MeterType.Water, ValueIdent.ValueIdentError.DeviceError, new DateTime?(dateTime3));
        }
        else
        {
          DateTime now = DateTime.Now;
          this.AddErrorValue(ValueList, ValueIdent.ValueIdPart_MeterType.Water, ValueIdent.ValueIdentError.DeviceError, new DateTime?(new DateTime(now.Year, now.Month, now.Day)));
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
      GlobalDeviceId globalDeviceId = new GlobalDeviceId()
      {
        Serialnumber = this.SerialNumber.ToString(),
        MeterType = ValueIdent.ValueIdPart_MeterType.Water
      };
      globalDeviceId.DeviceTypeName = globalDeviceId.MeterType.ToString();
      globalDeviceId.Manufacturer = "MINOL";
      globalDeviceId.MeterNumber = this.SerialNumber.ToString();
      globalDeviceId.FirmwareVersion = this.FirmwareVersion.ToString();
      globalDeviceId.Generation = "3";
      return globalDeviceId;
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
      if (SubDevice != 0)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Aqua micro device has no sub devices!");
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
        else if (parameterKey == "INFO;K_Value" || parameter.Value.ParameterID == OverrideID.InputPulsValue)
        {
          if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet3))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change the puls value!");
            continue;
          }
          ParameterAccess parameterAccess = this.MyFunctions.MyDevices.ParameterAccessByName["INFO;K"];
          int address = parameterAccess.TelegramPara.Address;
          if (this.complexPulseValue == -1L)
            this.complexPulseValue = Util.ToLong(parameterAccess.GetReadValue(this.Map));
          this.complexPulseValue = Util.ToLong(parameterValue) << 4 | this.complexPulseValue & 15L;
          byte[] byteArray = Util.ConvertLongToByteArray(this.complexPulseValue, parameterAccess.TelegramPara.ByteLength);
          this.SetConfigurationParameters(address, byteArray);
        }
        else if (parameterKey == "INFO;K_Unit" || parameter.Value.ParameterID == OverrideID.InputResolution)
        {
          if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet3))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change the puls resolution!");
            continue;
          }
          ParameterAccess parameterAccess = this.MyFunctions.MyDevices.ParameterAccessByName["INFO;K"];
          int address = parameterAccess.TelegramPara.Address;
          if (this.complexPulseValue == -1L)
            this.complexPulseValue = Util.ToLong(parameterAccess.GetReadValue(this.Map));
          this.complexPulseValue = this.complexPulseValue & 65520L | Util.ToLong((long) this.ConvertInputUnitsIndexToMinolUnit(parameterValue.ToString()));
          byte[] byteArray = Util.ConvertLongToByteArray(this.complexPulseValue, parameterAccess.TelegramPara.ByteLength);
          this.SetConfigurationParameters(address, byteArray);
        }
        else
        {
          int num;
          switch (parameterKey)
          {
            case "INFO;Radio2_Preload":
              if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet4))
              {
                ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change the radio send interval!");
                continue;
              }
              this.SetReadValue("INFO;Radio2_Preload", (object) (Convert.ToInt32(parameterValue) / 2));
              goto label_78;
            case "INFO;Radio3_Preload":
              if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet4))
              {
                ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change the radio send interval!");
                continue;
              }
              this.SetReadValue("INFO;Radio3_Preload", (object) (Convert.ToInt32(parameterValue) / 2));
              goto label_78;
            case "INFO;RadioEpsilonOffsetEnabled":
              num = 1;
              break;
            default:
              num = parameter.Value.ParameterID == OverrideID.RadioEpsilonOffsetEnabled ? 1 : 0;
              break;
          }
          if (num != 0)
          {
            if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet4))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change the 'RadioEpsilonOffsetEnabled' parameter!");
              continue;
            }
            if (!(parameterValue is bool flag))
              flag = bool.Parse(parameterValue.ToString());
            if (flag)
              this.SetReadValue("INFO;Radio3_Epsilon", (object) MinolDevice.CalculateRadio3EpsilonValue(this.SerialNumber));
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
                this.ClearSavedLoggerValuesOfRadio3Device();
                break;
              case RadioProtocol.Scenario2:
                this.SetReadValue("RAM;CFG_HH", (object) false);
                this.SetReadValue("RAM;CFG_Radio3", (object) true);
                this.SetReadValue("RAM;RF_Type_Info", (object) 40);
                this.SetReadValue("RAM;TIMER_RF", (object) 10);
                this.ClearSavedLoggerValuesOfRadio3Device();
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
                AquaMicroRadio3.logger.Fatal("Unknown RadioMode=" + ((RadioProtocol) parameterValue).ToString());
                break;
            }
          }
          else if (parameterKey == "RAM;Reading" || parameter.Value.ParameterID == OverrideID.InputActualValue)
          {
            if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet4))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change actual measurement value!");
              continue;
            }
            this.SetReadValue(parameterKey, parameterValue);
          }
          else if (parameterKey == "RAM;DateError" || parameter.Value.ParameterID == OverrideID.ErrorDate)
          {
            if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet4))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change error date of the device!");
              continue;
            }
            if (!UserManager.CheckPermission(UserRights.Rights.Developer) && Convert.ToDateTime(parameterValue) != new DateTime(2000, 1, 1))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "Is not allowed to set the error date! Only reset is allowed (01/01/2000).");
              continue;
            }
            this.SetReadValue(parameterKey, parameterValue);
          }
          else if (parameterKey == "RAM;ERROR" || parameter.Value.ParameterID == OverrideID.DeviceHasError)
          {
            if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet4))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change error flag of the device!");
              continue;
            }
            if (!UserManager.CheckPermission(UserRights.Rights.Developer) && Convert.ToBoolean(parameterValue))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "Is not allowed to set the error flag! Only reset is allowed.");
              continue;
            }
            this.SetReadValue(parameterKey, parameterValue);
          }
          else if (parameterKey == "RAM;DateKey" || parameter.Value.ParameterID == OverrideID.DueDate)
          {
            if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change due date of the device!");
              continue;
            }
            this.SetReadValue(parameterKey, parameterValue);
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
label_78:;
      }
      return true;
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
      if (SubDevice != 0)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Aqua micro device has no sub devices!");
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      }
      try
      {
        this.ParameterList = new SortedList<OverrideID, ConfigurationParameter>();
        bool hasWritePermission1 = UserManager.CheckPermission(UserRights.Rights.MConfigSet1);
        bool hasWritePermission2 = UserManager.CheckPermission(UserRights.Rights.MConfigSet3);
        bool hasWritePermission3 = UserManager.CheckPermission(UserRights.Rights.MConfigSet4);
        this.AddParameter(0, OverrideID.DeviceName);
        this.AddParameter(0, OverrideID.SerialNumber);
        this.AddParameter(0, OverrideID.FirmwareVersion);
        this.AddParameter(0, OverrideID.Signature);
        this.AddParameter(0, OverrideID.Manufacturer);
        if (ConfigurationType == ConfigurationParameter.ValueType.Direct || ConfigurationType == ConfigurationParameter.ValueType.Complete)
        {
          this.AddParameter(true, 0, OverrideID.DueDate, "RAM;DateKey", hasWritePermission1);
          this.AddParameter(true, 0, OverrideID.DeviceClock, "RAM;DateToday", hasWritePermission1);
          this.AddParameter(true, 0, OverrideID.DeviceHasError, "RAM;ERROR", hasWritePermission3);
          this.AddParameter(true, 0, OverrideID.ErrorDate, "RAM;DateError", hasWritePermission3);
          this.AddParameter(true, 0, OverrideID.RadioProtocol, "RAM;RadioProtocol", hasWritePermission2);
          this.AddParameter(true, 0, OverrideID.RadioEpsilonOffsetEnabled, "INFO;RadioEpsilonOffsetEnabled", hasWritePermission3);
          this.AddParameter(true, 0, OverrideID.SleepMode, "RAM;SLEEP", hasWritePermission3);
          this.AddParameter(false, 0, OverrideID.InputResolution, "INFO;K_Unit", false);
          this.AddParameter(true, 0, OverrideID.InputPulsValue, "INFO;K_Value", hasWritePermission3);
          this.AddParameter(true, 0, OverrideID.InputActualValue, "RAM;Reading", hasWritePermission3);
          this.AddParameter(false, OverrideID.RadioFrequence, new ConfigurationParameter(OverrideID.RadioFrequence, (object) this.GetRadioFrequence())
          {
            HasWritePermission = false
          });
          if (Convert.ToBoolean(this.MyFunctions.MyDevices.BitAccessByBitName["RAM;CFG_Radio3"].GetBitValue(this.Map)))
            this.AddParameter(true, 0, OverrideID.RadioSendInterval, "INFO;Radio3_Preload", hasWritePermission3, "s", (string[]) null, (object) 16, (object) 131070);
          else
            this.AddParameter(true, 0, OverrideID.RadioSendInterval, "INFO;Radio2_Preload", hasWritePermission3, "s", (string[]) null, (object) 300, (object) 300);
          string str = string.Format("ID:{0} Manufacturer:0x{1:X4} Status:0x{2} Signature:0x{3:X4} CFG_Flags:0x{4}", (object) this.SerialNumber, (object) this.Manufacturer, (object) this.GetParameterDataHexString("RAM;Status"), (object) this.Signature, (object) this.GetParameterDataHexString("INFO;CFG_Flags"));
          this.AddParameter(false, OverrideID.DiagnosticString, new ConfigurationParameter(OverrideID.DiagnosticString)
          {
            ParameterValue = (object) str,
            HasWritePermission = false,
            IsFunction = false
          });
        }
        return this.ParameterList;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Parameter generation error " + ex.Message);
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

// Decompiled with JetBrains decompiler
// Type: MinolHandler.AquaMicro
// Assembly: MinolHandler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: A1A42975-0CFC-4FCB-838E-3BA18C5EABDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinolHandler.dll

using StartupLib;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace MinolHandler
{
  internal class AquaMicro : MinolDevice
  {
    private long complexPulseValue = -1;

    internal AquaMicro(MinolHandlerFunctions Functions)
      : base(Functions)
    {
      this.DeviceType = DeviceTypes.AquaMicro;
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
      if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for Minomess micro radio2!");
        return (object) null;
      }
      if (this.MyFunctions.MyDevices.BitAccessByBitName.ContainsKey(parameterKey))
        return (object) this.MyFunctions.MyDevices.BitAccessByBitName[parameterKey].GetBitValue(this.Map);
      switch (parameterKey)
      {
        case "RAM;Ticker":
          ParameterAccess parameterAccess = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;DateToday"];
          if (parameterAccess == null)
            return (object) null;
          object readValue1 = parameterAccess.GetReadValue(this.Map);
          if (readValue1 == null || readValue1.GetType() != typeof (DateTime))
            return (object) null;
          object readValue2 = this.MyFunctions.MyDevices.ParameterAccessByName[parameterKey].GetReadValue(this.Map);
          return readValue2 == null || readValue2.GetType() != typeof (long) ? (object) null : (object) ((DateTime) readValue1).AddSeconds((double) (long) readValue2);
        case "INFO;K_Unit":
          return (object) DeviceCollector.MinolDevice.ConvertMinolUnitToInputUnitsIndex((byte) Util.ToLong(this.MyFunctions.MyDevices.ParameterAccessByName["INFO;K"].GetReadValue(this.Map)));
        case "INFO;K_Value":
          return (object) Util.ToDouble((object) (Util.ToLong(this.MyFunctions.MyDevices.ParameterAccessByName["INFO;K"].GetReadValue(this.Map)) >> 4));
        case "INFO;RF_Preload":
          return (object) (Util.ToInteger(this.MyFunctions.MyDevices.ParameterAccessByName[parameterKey].GetReadValue(this.Map)) * 2);
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
        throw new ArgumentOutOfRangeException(nameof (SubDevice), "AquaMicro has no sub devices!");
      return this.GetValues(ref ValueList);
    }

    internal override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for Minomess micro radio2!");
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
      DateTime readValue1 = (DateTime) this.GetReadValue("RAM;Ticker");
      SortedList<DeviceCollectorSettings, object> collectorSettings = this.MyFunctions.MyBus.GetDeviceCollectorSettings();
      DateTime dateTime1 = Convert.ToDateTime(collectorSettings[DeviceCollectorSettings.FromTime]);
      DateTime dateTime2 = Convert.ToDateTime(collectorSettings[DeviceCollectorSettings.ToTime]);
      Decimal num1 = Convert.ToDecimal(this.GetReadValue("INFO;K_Value"));
      long num2 = 272699457;
      if (ValueIdent.IsExpectedValueIdent(filter, num2) && readValue1 >= dateTime1 && readValue1 <= dateTime2)
      {
        object readValue2 = this.GetReadValue("RAM;Reading");
        if (readValue2 != null)
        {
          Decimal num3 = Convert.ToDecimal(readValue2);
          this.AddValue(ref ValueList, readValue1, num2, (object) (num3 * num1 / 1000M));
        }
      }
      long num4 = 281088065;
      if (ValueIdent.IsExpectedValueIdent(filter, num4))
      {
        for (int index = 1; index <= 2; ++index)
        {
          object readValue3 = this.GetReadValue("RAM;DateStampY" + index.ToString("00"));
          if (readValue3 != null)
          {
            DateTime timePoint = (DateTime) readValue3;
            if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
            {
              object readValue4 = this.GetReadValue("RAM;ReadingY" + index.ToString("00"));
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
          DateTime timePoint = new DateTime(readValue1.Year, readValue1.Month, 1);
          timePoint = timePoint.AddMonths((index - 1) * -1);
          if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
          {
            object readValue5 = this.GetReadValue("RAM;ReadingM" + index.ToString("00"));
            if (readValue5 != null)
            {
              Decimal num7 = Convert.ToDecimal(readValue5);
              this.AddValue(ref ValueList, timePoint, num6, (object) (num7 * num1 / 1000M));
            }
          }
        }
      }
      long num8 = 293670977;
      if (ValueIdent.IsExpectedValueIdent(filter, num8))
      {
        DateTime timePoint = new DateTime(readValue1.Year, readValue1.Month, 16);
        if (readValue1.Day < 16)
          timePoint = timePoint.AddMonths(-1);
        if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
        {
          object readValue6 = this.GetReadValue("RAM;ReadingM00");
          if (readValue6 != null)
          {
            Decimal num9 = Convert.ToDecimal(readValue6);
            this.AddValue(ref ValueList, timePoint, num8, (object) (num9 * num1 / 1000M));
          }
        }
      }
      long num10 = 272700106;
      if (ValueIdent.IsExpectedValueIdent(filter, num10) && readValue1 >= dateTime1 && readValue1 <= dateTime2)
      {
        object readValue7 = this.GetReadValue("RAM;Reading");
        if (readValue7 != null)
          this.AddValue(ref ValueList, readValue1, num10, readValue7);
      }
      long num11 = 281088714;
      if (ValueIdent.IsExpectedValueIdent(filter, num11))
      {
        for (int index = 1; index <= 2; ++index)
        {
          object readValue8 = this.GetReadValue("RAM;DateStampY" + index.ToString("00"));
          if (readValue8 != null)
          {
            DateTime timePoint = (DateTime) readValue8;
            if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
            {
              object readValue9 = this.GetReadValue("RAM;ReadingY" + index.ToString("00"));
              if (readValue9 != null)
                this.AddValue(ref ValueList, timePoint, num11, readValue9);
            }
          }
        }
      }
      long num12 = 289477322;
      if (ValueIdent.IsExpectedValueIdent(filter, num12))
      {
        for (int index = 1; index <= 18; ++index)
        {
          DateTime timePoint = new DateTime(readValue1.Year, readValue1.Month, 1);
          timePoint = timePoint.AddMonths((index - 1) * -1);
          if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
          {
            object readValue10 = this.GetReadValue("RAM;ReadingM" + index.ToString("00"));
            if (readValue10 != null)
              this.AddValue(ref ValueList, timePoint, num12, readValue10);
          }
        }
      }
      long num13 = 293671626;
      if (ValueIdent.IsExpectedValueIdent(filter, num13))
      {
        DateTime timePoint = new DateTime(readValue1.Year, readValue1.Month, 16);
        if (readValue1.Day < 16)
          timePoint = timePoint.AddMonths(-1);
        if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
        {
          object readValue11 = this.GetReadValue("RAM;ReadingM00");
          if (readValue11 != null)
            this.AddValue(ref ValueList, timePoint, num13, readValue11);
        }
      }
      object readValue12 = this.GetReadValue("RAM;ERROR");
      if (readValue12 != null && Convert.ToBoolean(readValue12))
      {
        object readValue13 = this.GetReadValue("RAM;DateError");
        DateTime dateTime3;
        if (readValue13 != null && Util.TryParseToDateTime(readValue13.ToString(), out dateTime3))
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
      if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for Minomess micro radio2!");
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
      globalDeviceId.Generation = "2";
      return globalDeviceId;
    }

    internal override bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> ParameterList,
      int SubDevice)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for Minomess micro radio2!");
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
        object parameterValue1 = parameter.Value.ParameterValue;
        if (parameterKey == "RAM;Ticker" || parameter.Value.ParameterID == OverrideID.DeviceClock)
        {
          if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change the device clock!");
            continue;
          }
          ParameterAccess parameterAccess1 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Ticker"];
          this.SetConfigurationParameters(parameterAccess1.TelegramPara.Address, parameterAccess1.GetValueAsByteArray(parameterValue1));
          ParameterAccess parameterAccess2 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;DateToday"];
          this.SetConfigurationParameters(parameterAccess2.TelegramPara.Address, parameterAccess2.GetValueAsByteArray(parameterValue1));
        }
        else
        {
          int num;
          switch (parameterKey)
          {
            case "INFO;RF_Preload":
              int parameterValue2 = Convert.ToInt32(parameterValue1) / 2;
              this.SetReadValue(parameterKey, (object) parameterValue2);
              goto label_27;
            case "INFO;K_Value":
              ParameterAccess parameterAccess3 = this.MyFunctions.MyDevices.ParameterAccessByName["INFO;K"];
              int address1 = parameterAccess3.TelegramPara.Address;
              if (this.complexPulseValue == -1L)
                this.complexPulseValue = Util.ToLong(parameterAccess3.GetReadValue(this.Map));
              this.complexPulseValue = Util.ToLong(parameterValue1) << 4 | this.complexPulseValue & 15L;
              byte[] byteArray1 = Util.ConvertLongToByteArray(this.complexPulseValue, parameterAccess3.TelegramPara.ByteLength);
              this.SetConfigurationParameters(address1, byteArray1);
              goto label_27;
            case "INFO;K_Unit":
              ParameterAccess parameterAccess4 = this.MyFunctions.MyDevices.ParameterAccessByName["INFO;K"];
              int address2 = parameterAccess4.TelegramPara.Address;
              if (this.complexPulseValue == -1L)
                this.complexPulseValue = Util.ToLong(parameterAccess4.GetReadValue(this.Map));
              this.complexPulseValue = this.complexPulseValue & 65520L | Util.ToLong((long) this.ConvertInputUnitsIndexToMinolUnit(parameterValue1.ToString()));
              byte[] byteArray2 = Util.ConvertLongToByteArray(this.complexPulseValue, parameterAccess4.TelegramPara.ByteLength);
              this.SetConfigurationParameters(address2, byteArray2);
              goto label_27;
            case "RAM;SLEEP":
              num = 1;
              break;
            default:
              num = parameter.Value.ParameterID == OverrideID.SleepMode ? 1 : 0;
              break;
          }
          if (num != 0)
          {
            if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet4))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change sleep parameter of the device!");
              continue;
            }
            if (Convert.ToBoolean(parameterValue1))
              this.SetReadValue("RAM;DateOn", (object) DateTime.MinValue);
            this.SetReadValue(parameterKey, parameterValue1);
          }
          else
            this.SetReadValue(parameterKey, parameterValue1);
        }
label_27:;
      }
      return true;
    }

    internal override SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType,
      int SubDevice)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for Minomess micro radio2!");
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
        bool hasWritePermission2 = UserManager.CheckPermission(UserRights.Rights.MConfigSet4);
        this.AddParameter(0, OverrideID.DeviceName);
        this.AddParameter(0, OverrideID.SerialNumber);
        this.AddParameter(0, OverrideID.FirmwareVersion);
        this.AddParameter(0, OverrideID.Signature);
        this.AddParameter(0, OverrideID.Manufacturer);
        if (ConfigurationType == ConfigurationParameter.ValueType.Direct || ConfigurationType == ConfigurationParameter.ValueType.Complete)
        {
          this.AddParameter(true, 0, OverrideID.DueDate, "RAM;DateKey", hasWritePermission1);
          this.AddParameter(true, 0, OverrideID.DeviceClock, "RAM;Ticker", hasWritePermission1);
          this.AddParameter(true, 0, OverrideID.DeviceHasError, "RAM;ERROR", hasWritePermission2);
          this.AddParameter(true, 0, OverrideID.ErrorDate, "RAM;DateError", hasWritePermission2);
          this.AddParameter(true, 0, OverrideID.RadioSendInterval, "INFO;RF_Preload", hasWritePermission2, "s", (string[]) null, (object) 16, (object) 131070);
          this.AddParameter(false, 0, OverrideID.InputResolution, "INFO;K_Unit", false);
          this.AddParameter(true, 0, OverrideID.InputPulsValue, "INFO;K_Value", hasWritePermission2);
          this.AddParameter(true, 0, OverrideID.InputActualValue, "RAM;Reading", hasWritePermission2);
          this.AddParameter(true, 0, OverrideID.SleepMode, "RAM;SLEEP", hasWritePermission2);
          string str = string.Format("ID:{0} Manufacturer:0x{1:X4} Status:0x{2} Signature:0x{3:X4} CFG_Flags:0x{4}", (object) this.SerialNumber, (object) this.Manufacturer, (object) this.GetParameterDataHexString("RAM;Status"), (object) this.Signature, (object) this.GetParameterDataHexString("INFO;CFG_Flags"));
          this.AddParameter(false, OverrideID.DiagnosticString, new ConfigurationParameter(OverrideID.DiagnosticString)
          {
            ParameterValue = (object) str,
            HasWritePermission = false,
            IsFunction = false
          });
          this.complexPulseValue = -1L;
        }
        return this.ParameterList;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Parameter generation error " + ex.Message);
        return this.ParameterList;
      }
    }
  }
}

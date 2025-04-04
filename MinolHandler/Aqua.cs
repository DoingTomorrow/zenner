// Decompiled with JetBrains decompiler
// Type: MinolHandler.Aqua
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
  internal class Aqua : MinolDevice
  {
    private static Logger logger = LogManager.GetLogger(nameof (Aqua));
    private long complexPulseValue = -1;

    internal Aqua(MinolHandlerFunctions Functions)
      : base(Functions)
    {
      this.DeviceType = DeviceTypes.Aqua;
    }

    internal override int GetMaxSizeOfRequestBuffer() => 64;

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
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for Minomess radio2!");
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
          try
          {
            object readValue1 = parameterAccess.GetReadValue(this.Map);
            if (readValue1 == null)
              return (object) null;
            DateTime dateTime = DateTime.Parse(readValue1.ToString());
            object readValue2 = this.MyFunctions.MyDevices.ParameterAccessByName[parameterKey].GetReadValue(this.Map);
            if (readValue2 == null)
              return (object) null;
            long num = long.Parse(readValue2.ToString());
            return num < 0L ? (object) null : (object) dateTime.AddSeconds((double) num);
          }
          catch (Exception ex)
          {
            string message = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
            Aqua.logger.ErrorException(message, ex);
            return (object) null;
          }
        case "RAM;K_Unit":
          return (object) DeviceCollector.MinolDevice.ConvertMinolUnitToInputUnitsIndex((byte) Util.ToLong(this.MyFunctions.MyDevices.ParameterAccessByName["RAM;K"].GetReadValue(this.Map)));
        case "RAM;K_Value":
          return (object) Util.ToDouble((object) (Util.ToLong(this.MyFunctions.MyDevices.ParameterAccessByName["RAM;K"].GetReadValue(this.Map)) >> 4));
        case "RAM;RF_Preload":
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
        throw new ArgumentOutOfRangeException(nameof (SubDevice), "Aqua has no sub devices!");
      return this.GetValues(ref ValueList);
    }

    internal override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for Minomess radio2!");
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
      object readValue1 = this.GetReadValue("RAM;Ticker");
      if (readValue1 == null)
        return true;
      DateTime timePoint1 = (DateTime) readValue1;
      long num1 = 272699457;
      if (ValueIdent.IsExpectedValueIdent(filter, num1) && timePoint1 >= dateTime1 && timePoint1 <= dateTime2)
      {
        object readValue2 = this.GetReadValue("RAM;Reading");
        if (readValue2 != null)
          this.AddValue(ref ValueList, timePoint1, num1, (object) (Convert.ToDecimal(readValue2) / 1000M));
      }
      long num2 = 281088065;
      if (ValueIdent.IsExpectedValueIdent(filter, num2))
      {
        for (int index = 1; index <= 2; ++index)
        {
          object readValue3 = this.GetReadValue("RAM;DateStampY" + index.ToString("00"));
          if (readValue3 != null)
          {
            DateTime timePoint2 = (DateTime) readValue3;
            if (timePoint2 >= dateTime1 && timePoint2 <= dateTime2 && timePoint2 != DateTime.MinValue)
            {
              object readValue4 = this.GetReadValue("RAM;ReadingY" + index.ToString("00"));
              if (readValue4 != null)
                this.AddValue(ref ValueList, timePoint2, num2, (object) (Convert.ToDecimal(readValue4) / 1000M));
            }
          }
        }
      }
      long num3 = 289476673;
      if (ValueIdent.IsExpectedValueIdent(filter, num3))
      {
        for (int index = 1; index <= 18; ++index)
        {
          DateTime timePoint3 = new DateTime(timePoint1.Year, timePoint1.Month, 1);
          timePoint3 = timePoint3.AddMonths((index - 1) * -1);
          if (timePoint3 >= dateTime1 && timePoint3 <= dateTime2 && timePoint3 != DateTime.MinValue)
          {
            object readValue5 = this.GetReadValue("RAM;ReadingM" + index.ToString("00"));
            if (readValue5 != null)
              this.AddValue(ref ValueList, timePoint3, num3, (object) (Convert.ToDecimal(readValue5) / 1000M));
          }
        }
      }
      long num4 = 293670977;
      if (ValueIdent.IsExpectedValueIdent(filter, num4))
      {
        DateTime timePoint4 = new DateTime(timePoint1.Year, timePoint1.Month, 16);
        if (timePoint1.Day < 16)
          timePoint4 = timePoint4.AddMonths(-1);
        if (timePoint4 >= dateTime1 && timePoint4 <= dateTime2 && timePoint4 != DateTime.MinValue)
        {
          object readValue6 = this.GetReadValue("RAM;ReadingM00");
          if (readValue6 != null)
            this.AddValue(ref ValueList, timePoint4, num4, (object) (Convert.ToDecimal(readValue6) / 1000M));
        }
      }
      long num5 = 272700106;
      if (ValueIdent.IsExpectedValueIdent(filter, num5) && timePoint1 >= dateTime1 && timePoint1 <= dateTime2)
      {
        object readValue7 = this.GetReadValue("RAM;Reading");
        if (readValue7 != null)
          this.AddValue(ref ValueList, timePoint1, num5, readValue7);
      }
      long num6 = 281088714;
      if (ValueIdent.IsExpectedValueIdent(filter, num6))
      {
        for (int index = 1; index <= 2; ++index)
        {
          object readValue8 = this.GetReadValue("RAM;DateStampY" + index.ToString("00"));
          if (readValue8 != null)
          {
            DateTime timePoint5 = (DateTime) readValue8;
            if (timePoint5 >= dateTime1 && timePoint5 <= dateTime2 && timePoint5 != DateTime.MinValue)
            {
              object readValue9 = this.GetReadValue("RAM;ReadingY" + index.ToString("00"));
              if (readValue9 != null)
                this.AddValue(ref ValueList, timePoint5, num6, readValue9);
            }
          }
        }
      }
      long num7 = 289477322;
      if (ValueIdent.IsExpectedValueIdent(filter, num7))
      {
        for (int index = 1; index <= 18; ++index)
        {
          DateTime timePoint6 = new DateTime(timePoint1.Year, timePoint1.Month, 1);
          timePoint6 = timePoint6.AddMonths((index - 1) * -1);
          if (timePoint6 >= dateTime1 && timePoint6 <= dateTime2 && timePoint6 != DateTime.MinValue)
          {
            object readValue10 = this.GetReadValue("RAM;ReadingM" + index.ToString("00"));
            if (readValue10 != null)
              this.AddValue(ref ValueList, timePoint6, num7, readValue10);
          }
        }
      }
      long num8 = 293671626;
      if (ValueIdent.IsExpectedValueIdent(filter, num8))
      {
        DateTime timePoint7 = new DateTime(timePoint1.Year, timePoint1.Month, 16);
        if (timePoint1.Day < 16)
          timePoint7 = timePoint7.AddMonths(-1);
        if (timePoint7 >= dateTime1 && timePoint7 <= dateTime2 && timePoint7 != DateTime.MinValue)
        {
          object readValue11 = this.GetReadValue("RAM;ReadingM00");
          if (readValue11 != null)
            this.AddValue(ref ValueList, timePoint7, num8, readValue11);
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
      object readValue14 = this.GetReadValue("RAM;MANIPULATION");
      if (readValue14 != null && Convert.ToBoolean(readValue14))
      {
        object readValue15 = this.GetReadValue("RAM;DateManipulation");
        DateTime dateTime4;
        if (readValue15 != null && Util.TryParseToDateTime(readValue15.ToString(), out dateTime4))
        {
          this.AddErrorValue(ValueList, ValueIdent.ValueIdPart_MeterType.Water, ValueIdent.ValueIdentError.Manipulation, new DateTime?(dateTime4));
        }
        else
        {
          DateTime now = DateTime.Now;
          this.AddErrorValue(ValueList, ValueIdent.ValueIdPart_MeterType.Water, ValueIdent.ValueIdentError.Manipulation, new DateTime?(new DateTime(now.Year, now.Month, now.Day)));
        }
      }
      return true;
    }

    internal override bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> ParameterList,
      int SubDevice)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for Minomess radio2!");
        return false;
      }
      if (SubDevice != 0)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Aqua device has no sub devices!");
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
            case "RAM;RF_Preload":
              int parameterValue2 = Convert.ToInt32(parameterValue1) / 2;
              this.SetReadValue(parameterKey, (object) parameterValue2);
              goto label_27;
            case "RAM;K_Value":
              ParameterAccess parameterAccess3 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;K"];
              int address1 = parameterAccess3.TelegramPara.Address;
              if (this.complexPulseValue == -1L)
                this.complexPulseValue = Util.ToLong(parameterAccess3.GetReadValue(this.Map));
              this.complexPulseValue = Util.ToLong(parameterValue1) << 4 | this.complexPulseValue & 15L;
              byte[] byteArray1 = Util.ConvertLongToByteArray(this.complexPulseValue, parameterAccess3.TelegramPara.ByteLength);
              this.SetConfigurationParameters(address1, byteArray1);
              goto label_27;
            case "RAM;K_Unit":
              ParameterAccess parameterAccess4 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;K"];
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

    internal override GlobalDeviceId GetGlobalDeviceId()
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for Minomess radio2!");
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

    internal override SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType,
      int SubDevice)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for Minomess radio2!");
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      }
      if (SubDevice != 0)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Aqua device has no sub devices!");
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      }
      try
      {
        this.ParameterList = new SortedList<OverrideID, ConfigurationParameter>();
        bool hasWritePermission1 = UserManager.CheckPermission(UserRights.Rights.MConfigSet1);
        bool hasWritePermission2 = UserManager.CheckPermission(UserRights.Rights.MConfigSet2);
        bool hasWritePermission3 = UserManager.CheckPermission(UserRights.Rights.MConfigSet4);
        this.AddParameter(0, OverrideID.DeviceName);
        this.AddParameter(0, OverrideID.SerialNumber);
        this.AddParameter(0, OverrideID.FirmwareVersion);
        this.AddParameter(0, OverrideID.Signature);
        this.AddParameter(0, OverrideID.Manufacturer);
        if (ConfigurationType == ConfigurationParameter.ValueType.Direct || ConfigurationType == ConfigurationParameter.ValueType.Complete)
        {
          this.AddParameter(true, 0, OverrideID.DueDate, "RAM;DateKey", hasWritePermission1);
          this.AddParameter(true, 0, OverrideID.DeviceClock, "RAM;Ticker", hasWritePermission1);
          this.AddParameter(false, 0, OverrideID.InputResolution, "RAM;K_Unit", false);
          this.AddParameter(false, 0, OverrideID.InputPulsValue, "RAM;K_Value", false);
          this.AddParameter(true, 0, OverrideID.InputActualValue, "RAM;Reading", hasWritePermission3);
          this.AddParameter(true, 0, OverrideID.Manipulation, "RAM;MANIPULATION", hasWritePermission2);
          this.AddParameter(true, 0, OverrideID.ManipulationDate, "RAM;DateManipulation", hasWritePermission2);
          this.AddParameter(true, 0, OverrideID.DeviceHasError, "RAM;ERROR", hasWritePermission3);
          this.AddParameter(true, 0, OverrideID.ErrorDate, "RAM;DateError", hasWritePermission3);
          this.AddParameter(true, 0, OverrideID.RadioSendInterval, "RAM;RF_Preload", hasWritePermission3, "s", (string[]) null, (object) 16, (object) 131070);
          this.AddParameter(true, 0, OverrideID.SleepMode, "RAM;SLEEP", hasWritePermission3);
          string str = string.Format("ID:{0} Manufacturer:0x{1:X4} Status:0x{2} Signature:0x{3:X4} CfgFlags1:0x{4} CfgFlags3:0x{5}", (object) this.SerialNumber, (object) this.Manufacturer, (object) this.GetParameterDataHexString("RAM;Status"), (object) this.Signature, (object) this.GetParameterDataHexString("RAM;CfgFlags1"), (object) this.GetParameterDataHexString("RAM;CfgFlags3"));
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
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Parameter generation error. " + ex.Message);
        return this.ParameterList;
      }
    }
  }
}

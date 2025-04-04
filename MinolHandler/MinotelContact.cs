// Decompiled with JetBrains decompiler
// Type: MinolHandler.MinotelContact
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
  internal class MinotelContact : MinolDevice
  {
    private static string[] ReadingNameExtention = new string[3]
    {
      "",
      "_A",
      "_B"
    };
    private long input1_PulseValue = -1;
    private long input2_PulseValue = -1;

    internal MinotelContact(MinolHandlerFunctions Functions)
      : base(Functions)
    {
      this.DeviceType = DeviceTypes.MinotelContact;
    }

    internal override int GetMaxSizeOfRequestBuffer() => 24;

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
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for MinotelContact radio2!");
        return (object) null;
      }
      if (this.MyFunctions.MyDevices.BitAccessByBitName.ContainsKey(parameterKey))
        return (object) this.MyFunctions.MyDevices.BitAccessByBitName[parameterKey].GetBitValue(this.Map);
      switch (parameterKey)
      {
        case "RAM;K_A_Unit":
          return (object) this.ConvertMinotelContactUnitToInputUnitsIndex((byte) Util.ToLong(this.MyFunctions.MyDevices.ParameterAccessByName["RAM;K_A"].GetReadValue(this.Map)));
        case "RAM;K_B_Unit":
          return (object) this.ConvertMinotelContactUnitToInputUnitsIndex((byte) Util.ToLong(this.MyFunctions.MyDevices.ParameterAccessByName["RAM;K_B"].GetReadValue(this.Map)));
        case "RAM;K_A_Value":
          return (object) Util.ToDouble((object) (Util.ToLong(this.MyFunctions.MyDevices.ParameterAccessByName["RAM;K_A"].GetReadValue(this.Map)) >> 4));
        case "RAM;K_B_Value":
          return (object) Util.ToDouble((object) (Util.ToLong(this.MyFunctions.MyDevices.ParameterAccessByName["RAM;K_B"].GetReadValue(this.Map)) >> 4));
        case "RAM;Ticker":
          ParameterAccess parameterAccess1 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Ticker"];
          ParameterAccess parameterAccess2 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;DateToday"];
          if (parameterAccess2 == null)
            return (object) null;
          object readValue1 = parameterAccess2.GetReadValue(this.Map);
          if (readValue1 == null || readValue1.GetType() != typeof (DateTime))
            return (object) null;
          object readValue2 = parameterAccess1.GetReadValue(this.Map);
          return readValue2 == null || readValue2.GetType() != typeof (long) ? (object) null : (object) ((DateTime) readValue1).AddSeconds((double) (long) readValue2);
        default:
          if (!this.MyFunctions.MyDevices.ParameterAccessByName.ContainsKey(parameterKey))
            throw new ArgumentException("Unknown parameter! Key: " + parameterKey);
          return this.MyFunctions.MyDevices.ParameterAccessByName[parameterKey].GetReadValue(this.Map);
      }
    }

    internal override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList)
    {
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
      if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for MinotelContact radio2!");
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
      DateTime readValue1 = (DateTime) this.GetReadValue("RAM;Ticker");
      SortedList<DeviceCollectorSettings, object> collectorSettings = this.MyFunctions.MyBus.GetDeviceCollectorSettings();
      DateTime dateTime1 = Convert.ToDateTime(collectorSettings[DeviceCollectorSettings.FromTime]);
      DateTime dateTime2 = Convert.ToDateTime(collectorSettings[DeviceCollectorSettings.ToTime]);
      long num1 = 272700106;
      if (ValueIdent.IsExpectedValueIdent(filter, num1) && readValue1 >= dateTime1 && readValue1 <= dateTime2)
      {
        object readValue2 = this.GetReadValue("RAM;Reading" + MinotelContact.ReadingNameExtention[SubDevice]);
        if (readValue2 != null)
          this.AddValue(ref ValueList, readValue1, num1, readValue2);
      }
      long num2 = 281088714;
      if (ValueIdent.IsExpectedValueIdent(filter, num2))
      {
        for (int index = 1; index <= 2; ++index)
        {
          object readValue3 = this.GetReadValue("RAM;DateStampY" + index.ToString("00"));
          if (readValue3 != null)
          {
            DateTime timePoint = (DateTime) readValue3;
            if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
            {
              object readValue4 = this.GetReadValue("RAM;ReadingY" + index.ToString("00") + MinotelContact.ReadingNameExtention[SubDevice]);
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
          DateTime timePoint = new DateTime(readValue1.Year, readValue1.Month, 1);
          timePoint = timePoint.AddMonths((index - 1) * -1);
          if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
          {
            object readValue5 = this.GetReadValue("RAM;ReadingM" + index.ToString("00") + MinotelContact.ReadingNameExtention[SubDevice]);
            if (readValue5 != null)
              this.AddValue(ref ValueList, timePoint, num3, readValue5);
          }
        }
      }
      long num4 = 293671626;
      if (ValueIdent.IsExpectedValueIdent(filter, num4))
      {
        DateTime timePoint = new DateTime(readValue1.Year, readValue1.Month, 16);
        if (readValue1.Day < 16)
          timePoint = timePoint.AddMonths(-1);
        if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
        {
          object readValue6 = this.GetReadValue("RAM;ReadingM00" + MinotelContact.ReadingNameExtention[SubDevice]);
          if (readValue6 != null)
            this.AddValue(ref ValueList, timePoint, num4, readValue6);
        }
      }
      object readValue7 = this.GetReadValue("RAM;ERROR");
      if (readValue7 != null && Convert.ToBoolean(readValue7))
      {
        object readValue8 = this.GetReadValue("RAM;DateError");
        DateTime dateTime3;
        if (readValue8 != null && Util.TryParseToDateTime(readValue8.ToString(), out dateTime3))
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
      if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for MinotelContact radio2!");
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
      globalDeviceId1.Generation = "2";
      globalDeviceId1.SubDevices = new List<GlobalDeviceId>();
      SortedList<OverrideID, ConfigurationParameter> configurationParameters1 = this.GetConfigurationParameters(ConfigurationParameter.ValueType.Ident, 1);
      GlobalDeviceId globalDeviceId2 = new GlobalDeviceId()
      {
        Serialnumber = configurationParameters1[OverrideID.SerialNumber].ParameterValue.ToString(),
        MeterType = ValueIdent.ValueIdPart_MeterType.PulseCounter
      };
      globalDeviceId2.DeviceTypeName = ValueIdent.GetTranslatedValueNameForPartOfValueId((Enum) globalDeviceId2.MeterType);
      globalDeviceId2.Manufacturer = "MINOL";
      globalDeviceId2.MeterNumber = configurationParameters1[OverrideID.SerialNumber].ParameterValue.ToString();
      globalDeviceId2.FirmwareVersion = this.FirmwareVersion.ToString();
      globalDeviceId2.Generation = "2";
      globalDeviceId1.SubDevices.Add(globalDeviceId2);
      SortedList<OverrideID, ConfigurationParameter> configurationParameters2 = this.GetConfigurationParameters(ConfigurationParameter.ValueType.Ident, 2);
      GlobalDeviceId globalDeviceId3 = new GlobalDeviceId()
      {
        Serialnumber = configurationParameters2[OverrideID.SerialNumber].ParameterValue.ToString(),
        MeterType = ValueIdent.ValueIdPart_MeterType.PulseCounter
      };
      globalDeviceId3.DeviceTypeName = ValueIdent.GetTranslatedValueNameForPartOfValueId((Enum) globalDeviceId3.MeterType);
      globalDeviceId3.Manufacturer = "MINOL";
      globalDeviceId3.MeterNumber = configurationParameters2[OverrideID.SerialNumber].ParameterValue.ToString();
      globalDeviceId3.FirmwareVersion = this.FirmwareVersion.ToString();
      globalDeviceId3.Generation = "2";
      globalDeviceId1.SubDevices.Add(globalDeviceId3);
      return globalDeviceId1;
    }

    internal override bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> ParameterList,
      int SubDevice)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for MinotelContact radio2!");
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
        if (parameterKey == "RAM;Ticker" || parameter.Value.ParameterID == OverrideID.DeviceClock)
        {
          if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change the device clock!");
            return false;
          }
          ParameterAccess parameterAccess1 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Ticker"];
          this.SetConfigurationParameters(parameterAccess1.TelegramPara.Address, parameterAccess1.GetValueAsByteArray(parameterValue));
          ParameterAccess parameterAccess2 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;DateToday"];
          this.SetConfigurationParameters(parameterAccess2.TelegramPara.Address, parameterAccess2.GetValueAsByteArray(parameterValue));
        }
        else
        {
          int num;
          switch (parameterKey)
          {
            case "RAM;K_A_Value":
              ParameterAccess parameterAccess3 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;K_A"];
              int address1 = parameterAccess3.TelegramPara.Address;
              if (this.input1_PulseValue == -1L)
                this.input1_PulseValue = Util.ToLong(parameterAccess3.GetReadValue(this.Map));
              this.input1_PulseValue = Util.ToLong(parameterValue) << 4 | this.input1_PulseValue & 15L;
              byte[] byteArray1 = Util.ConvertLongToByteArray(this.input1_PulseValue, parameterAccess3.TelegramPara.ByteLength);
              this.SetConfigurationParameters(address1, byteArray1);
              goto label_32;
            case "RAM;K_B_Value":
              ParameterAccess parameterAccess4 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;K_B"];
              int address2 = parameterAccess4.TelegramPara.Address;
              if (this.input2_PulseValue == -1L)
                this.input2_PulseValue = Util.ToLong(parameterAccess4.GetReadValue(this.Map));
              this.input2_PulseValue = Util.ToLong(parameterValue) << 4 | this.input2_PulseValue & 15L;
              byte[] byteArray2 = Util.ConvertLongToByteArray(this.input2_PulseValue, parameterAccess4.TelegramPara.ByteLength);
              this.SetConfigurationParameters(address2, byteArray2);
              goto label_32;
            case "RAM;K_A_Unit":
              ParameterAccess parameterAccess5 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;K_A"];
              int address3 = parameterAccess5.TelegramPara.Address;
              if (this.input1_PulseValue == -1L)
                this.input1_PulseValue = Util.ToLong(parameterAccess5.GetReadValue(this.Map));
              this.input1_PulseValue = this.input1_PulseValue & 65520L | Util.ToLong((long) this.ConvertInputUnitsIndexToMinotelContactUnit(parameterValue.ToString()));
              byte[] byteArray3 = Util.ConvertLongToByteArray(this.input1_PulseValue, parameterAccess5.TelegramPara.ByteLength);
              this.SetConfigurationParameters(address3, byteArray3);
              goto label_32;
            case "RAM;K_B_Unit":
              ParameterAccess parameterAccess6 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;K_B"];
              int address4 = parameterAccess6.TelegramPara.Address;
              if (this.input2_PulseValue == -1L)
                this.input2_PulseValue = Util.ToLong(parameterAccess6.GetReadValue(this.Map));
              this.input2_PulseValue = this.input2_PulseValue & 65520L | Util.ToLong((long) this.ConvertInputUnitsIndexToMinotelContactUnit(parameterValue.ToString()));
              byte[] byteArray4 = Util.ConvertLongToByteArray(this.input2_PulseValue, parameterAccess6.TelegramPara.ByteLength);
              this.SetConfigurationParameters(address4, byteArray4);
              goto label_32;
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
            if (Convert.ToBoolean(parameterValue))
              this.SetReadValue("RAM;DateOn", (object) DateTime.MinValue);
            this.SetReadValue(parameterKey, parameterValue);
          }
          else
            this.SetReadValue(parameterKey, parameterValue);
        }
label_32:;
      }
      return true;
    }

    internal override SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType,
      int SubDevice)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for MinotelContact radio2!");
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
        bool hasWritePermission2 = UserManager.CheckPermission(UserRights.Rights.MConfigSet3);
        bool hasWritePermission3 = UserManager.CheckPermission(UserRights.Rights.MConfigSet4);
        switch (SubDevice)
        {
          case 0:
            this.AddParameter(false, 0, OverrideID.SerialNumber, "INFO;SerNo");
            this.AddParameter(0, OverrideID.DeviceName);
            this.AddParameter(0, OverrideID.FirmwareVersion);
            this.AddParameter(0, OverrideID.Signature);
            this.AddParameter(0, OverrideID.Manufacturer);
            if (ConfigurationType == ConfigurationParameter.ValueType.Direct || ConfigurationType == ConfigurationParameter.ValueType.Complete)
            {
              this.AddParameter(true, 0, OverrideID.DueDate, "RAM;DateKey", hasWritePermission1);
              this.AddParameter(true, 0, OverrideID.DeviceClock, "RAM;Ticker", hasWritePermission1);
              this.AddParameter(true, 0, OverrideID.DeviceHasError, "RAM;ERROR", hasWritePermission3);
              this.AddParameter(true, 0, OverrideID.ErrorDate, "INFO;DateError", hasWritePermission3);
              this.AddParameter(true, 0, OverrideID.SleepMode, "RAM;SLEEP", hasWritePermission3);
              this.AddParameter(false, OverrideID.NumberOfSubDevices, new ConfigurationParameter(OverrideID.NumberOfSubDevices)
              {
                ParameterValue = (object) 2UL
              });
              string str = string.Format("ID:{0} Manufacturer:0x{1:X4} Status:0x{2} Signature:0x{3:X4} CfgFlags:0x{4}", (object) this.SerialNumber, (object) this.Manufacturer, (object) this.GetParameterDataHexString("RAM;Status"), (object) this.Signature, (object) this.GetParameterDataHexString("INFO;CfgFlags"));
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
            this.AddParameter(true, 1, OverrideID.InputActualValue, "RAM;Reading_A", hasWritePermission2);
            break;
          case 2:
            this.AddParameter(false, 2, OverrideID.SerialNumber, "INFO;RFID_B");
            this.AddParameter(true, 2, OverrideID.InputResolution, "RAM;K_B_Unit", hasWritePermission2);
            this.AddParameter(true, 2, OverrideID.InputPulsValue, "RAM;K_B_Value", hasWritePermission2);
            this.AddParameter(true, 2, OverrideID.InputActualValue, "RAM;Reading_B", hasWritePermission2);
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

    private byte ConvertInputUnitsIndexToMinotelContactUnit(string inputUnitsIndexAsString)
    {
      return this.ConvertInputUnitsIndexToMinotelContactUnit((InputUnitsIndex) Enum.Parse(typeof (InputUnitsIndex), inputUnitsIndexAsString, true));
    }

    private byte ConvertInputUnitsIndexToMinotelContactUnit(InputUnitsIndex inputUnitsIndex)
    {
      switch (inputUnitsIndex)
      {
        case InputUnitsIndex.ImpUnit_0Wh:
          return 3;
        case InputUnitsIndex.ImpUnit_0kWh:
          return 4;
        case InputUnitsIndex.ImpUnit_0MWh:
          return 5;
        case InputUnitsIndex.ImpUnit_0J:
          return 6;
        case InputUnitsIndex.ImpUnit_0kJ:
          return 7;
        case InputUnitsIndex.ImpUnit_0MJ:
          return 8;
        case InputUnitsIndex.ImpUnit_0GJ:
          return 9;
        case InputUnitsIndex.ImpUnit_0L:
          return 1;
        case InputUnitsIndex.ImpUnit_0qm:
          return 2;
        default:
          return 0;
      }
    }

    private InputUnitsIndex ConvertMinotelContactUnitToInputUnitsIndex(byte minotelContactIndex)
    {
      minotelContactIndex &= (byte) 15;
      switch (minotelContactIndex)
      {
        case 1:
          return InputUnitsIndex.ImpUnit_0L;
        case 2:
          return InputUnitsIndex.ImpUnit_0qm;
        case 3:
          return InputUnitsIndex.ImpUnit_0Wh;
        case 4:
          return InputUnitsIndex.ImpUnit_0kWh;
        case 5:
          return InputUnitsIndex.ImpUnit_0MWh;
        case 6:
          return InputUnitsIndex.ImpUnit_0J;
        case 7:
          return InputUnitsIndex.ImpUnit_0kJ;
        case 8:
          return InputUnitsIndex.ImpUnit_0MJ;
        case 9:
          return InputUnitsIndex.ImpUnit_0GJ;
        default:
          return InputUnitsIndex.ImpUnit_0;
      }
    }
  }
}

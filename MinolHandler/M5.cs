// Decompiled with JetBrains decompiler
// Type: MinolHandler.M5
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
  internal class M5 : MinolDevice
  {
    internal M5(MinolHandlerFunctions Functions)
      : base(Functions)
    {
      this.DeviceType = DeviceTypes.EHCA_M5;
    }

    internal override int GetMaxSizeOfRequestBuffer() => 56;

    internal override GlobalDeviceId GetGlobalDeviceId()
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for M5 radio2!");
        return (GlobalDeviceId) null;
      }
      GlobalDeviceId globalDeviceId = new GlobalDeviceId()
      {
        Serialnumber = this.SerialNumber.ToString(),
        MeterType = ValueIdent.ValueIdPart_MeterType.HeatCostAllocator
      };
      globalDeviceId.DeviceTypeName = globalDeviceId.MeterType.ToString();
      globalDeviceId.Manufacturer = "MINOL";
      globalDeviceId.MeterNumber = this.SerialNumber.ToString();
      globalDeviceId.FirmwareVersion = this.FirmwareVersion.ToString();
      globalDeviceId.Generation = "2";
      return globalDeviceId;
    }

    internal override bool CreateDeviceData()
    {
      ParameterAccess parameterAccess = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;KQ"];
      if (this.MyFunctions.MyDevices.BitAccessByBitName["RAM;PermError"].GetBitValue(this.Map))
        this.DeviceState = ReadingValueState.error;
      else
        this.DeviceState = ReadingValueState.ok;
      return true;
    }

    internal override object GetReadValue(string parameterKey)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for M5 radio2!");
        return (object) false;
      }
      if (this.MyFunctions.MyDevices.ParameterAccessByName.ContainsKey(parameterKey))
      {
        ParameterAccess parameterAccess = this.MyFunctions.MyDevices.ParameterAccessByName[parameterKey];
        switch (parameterKey)
        {
          case "RAM;Date":
            DateTime dateTime = (DateTime) this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Date"].GetReadValue(this.Map);
            long readValue1 = (long) this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Minute"].GetReadValue(this.Map);
            dateTime = dateTime.AddMinutes((double) readValue1);
            long readValue2 = (long) this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Hour"].GetReadValue(this.Map);
            return (object) dateTime.AddHours((double) readValue2).AddSeconds((double) (long) this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Second"].GetReadValue(this.Map));
          case "RAM;c_H":
            return (object) Math.Round(Convert.ToDecimal(parameterAccess.GetReadValue(this.Map)), 3);
          case "RAM;c_HR":
            return (object) Math.Round(Convert.ToDecimal(parameterAccess.GetReadValue(this.Map)), 3);
          default:
            return parameterAccess.GetReadValue(this.Map);
        }
      }
      else
      {
        if (!this.MyFunctions.MyDevices.BitAccessByBitName.ContainsKey(parameterKey))
          throw new ArgumentException("Unknown parameter! Key: " + parameterKey);
        bool bitValue = this.MyFunctions.MyDevices.BitAccessByBitName[parameterKey].GetBitValue(this.Map);
        switch (parameterKey)
        {
          case "RAM;ProductScale":
            return bitValue ? (object) HCA_Scale.Product : (object) HCA_Scale.Uniform;
          case "RAM;DoubleSensor":
            return bitValue ? (object) HCA_SensorMode.Double : (object) HCA_SensorMode.Single;
          default:
            return (object) bitValue;
        }
      }
    }

    internal override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      int SubDevice)
    {
      if (SubDevice != 0)
        throw new ArgumentOutOfRangeException(nameof (SubDevice), "M5 has no sub devices!");
      return this.GetValues(ref ValueList);
    }

    internal override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for M5 radio2!");
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
      DateTime readValue1 = (DateTime) this.GetReadValue("RAM;Date");
      SortedList<DeviceCollectorSettings, object> collectorSettings = this.MyFunctions.MyBus.GetDeviceCollectorSettings();
      DateTime dateTime1 = Convert.ToDateTime(collectorSettings[DeviceCollectorSettings.FromTime]);
      DateTime dateTime2 = Convert.ToDateTime(collectorSettings[DeviceCollectorSettings.ToTime]);
      long num1 = 272700040;
      if (ValueIdent.IsExpectedValueIdent(filter, num1) && readValue1 >= dateTime1 && readValue1 <= dateTime2)
      {
        object readValue2 = this.GetReadValue("RAM;Reading0");
        if (readValue2 != null)
        {
          ParameterAccess parameterAccess = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Reading0"];
          this.AddValue(ref ValueList, readValue1, num1, readValue2);
        }
      }
      long num2 = 281088648;
      if (ValueIdent.IsExpectedValueIdent(filter, num2))
      {
        for (int index = 3; index <= 4; ++index)
        {
          object readValue3 = this.GetReadValue("RAM;DateStamp" + index.ToString());
          if (readValue3 != null)
          {
            DateTime timePoint = (DateTime) readValue3;
            if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
            {
              object readValue4 = this.GetReadValue("RAM;Reading" + index.ToString());
              if (readValue4 != null)
              {
                ParameterAccess parameterAccess = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Reading" + index.ToString()];
                this.AddValue(ref ValueList, timePoint, num2, readValue4);
              }
            }
          }
        }
      }
      long num3 = 289477256;
      if (ValueIdent.IsExpectedValueIdent(filter, num3))
      {
        for (int index = 1; index <= 2; ++index)
        {
          object readValue5 = this.GetReadValue("RAM;DateStamp" + index.ToString());
          if (readValue5 != null)
          {
            DateTime timePoint = (DateTime) readValue5;
            if (timePoint.Day == 1 && timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
            {
              object readValue6 = this.GetReadValue("RAM;Reading" + index.ToString());
              if (readValue6 != null)
              {
                ParameterAccess parameterAccess = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Reading" + index.ToString()];
                this.AddValue(ref ValueList, timePoint, num3, readValue6);
              }
            }
          }
        }
      }
      long num4 = 293671560;
      if (ValueIdent.IsExpectedValueIdent(filter, num4))
      {
        for (int index = 1; index <= 2; ++index)
        {
          object readValue7 = this.GetReadValue("RAM;DateStamp" + index.ToString());
          if (readValue7 != null)
          {
            DateTime timePoint = (DateTime) readValue7;
            if (timePoint.Day == 16 && timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
            {
              object readValue8 = this.GetReadValue("RAM;Reading" + index.ToString());
              if (readValue8 != null)
              {
                ParameterAccess parameterAccess = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Reading" + index.ToString()];
                this.AddValue(ref ValueList, timePoint, num4, readValue8);
              }
            }
          }
        }
      }
      object readValue9 = this.GetReadValue("RAM;PermError");
      if (readValue9 != null && Convert.ToBoolean(readValue9))
      {
        object readValue10 = this.GetReadValue("RAM;ErrDate");
        DateTime dateTime3;
        if (readValue10 != null && Util.TryParseToDateTime(readValue10.ToString(), out dateTime3))
        {
          this.AddErrorValue(ValueList, ValueIdent.ValueIdPart_MeterType.HeatCostAllocator, ValueIdent.ValueIdentError.DeviceError, new DateTime?(dateTime3));
        }
        else
        {
          DateTime now = DateTime.Now;
          this.AddErrorValue(ValueList, ValueIdent.ValueIdPart_MeterType.HeatCostAllocator, ValueIdent.ValueIdentError.DeviceError, new DateTime?(new DateTime(now.Year, now.Month, now.Day)));
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
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for M5 radio2!");
        return false;
      }
      if (SubDevice != 0)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "M5 device has no sub devices!");
        return false;
      }
      foreach (KeyValuePair<OverrideID, ConfigurationParameter> parameter in ParameterList)
      {
        string parameterKey = parameter.Value.ParameterKey;
        object parameterValue = parameter.Value.ParameterValue;
        if (parameterKey == "RAM;Date" || parameter.Value.ParameterID == OverrideID.DeviceClock)
        {
          if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change the device clock!");
          }
          else
          {
            ParameterAccess parameterAccess1 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Date"];
            this.SetConfigurationParameters(parameterAccess1.TelegramPara.Address, parameterAccess1.GetValueAsByteArray(parameterValue));
            ParameterAccess parameterAccess2 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Minute"];
            this.SetConfigurationParameters(parameterAccess2.TelegramPara.Address, parameterAccess2.GetValueAsByteArray(parameterValue));
            ParameterAccess parameterAccess3 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Hour"];
            this.SetConfigurationParameters(parameterAccess3.TelegramPara.Address, parameterAccess3.GetValueAsByteArray(parameterValue));
            ParameterAccess parameterAccess4 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Second"];
            this.SetConfigurationParameters(parameterAccess4.TelegramPara.Address, parameterAccess4.GetValueAsByteArray(parameterValue));
          }
        }
        else if (parameterKey == "RAM;ProductScale" || parameter.Value.ParameterID == OverrideID.HCA_Scale)
        {
          if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet2))
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change the scale of HCA!");
          else if ((HCA_Scale) Enum.Parse(typeof (HCA_Scale), parameterValue.ToString(), true) == HCA_Scale.Product)
            this.SetReadValue(parameterKey, (object) 1);
          else
            this.SetReadValue(parameterKey, (object) 0);
        }
        else if (parameterKey == "RAM;DoubleSensor" || parameter.Value.ParameterID == OverrideID.HCA_SensorMode)
        {
          if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet4))
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change the sensor mode of HCA!");
          else if ((HCA_SensorMode) Enum.Parse(typeof (HCA_SensorMode), parameterValue.ToString(), true) == HCA_SensorMode.Double)
            this.SetReadValue(parameterKey, (object) 1);
          else
            this.SetReadValue(parameterKey, (object) 0);
        }
        else if (parameterKey == "RAM;SleepMode" || parameter.Value.ParameterID == OverrideID.SleepMode)
        {
          if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet4))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change sleep parameter of the device!");
          }
          else
          {
            if (Convert.ToBoolean(parameterValue))
              this.SetReadValue("RAM;OnDate", (object) DateTime.MinValue);
            this.SetReadValue(parameterKey, parameterValue);
          }
        }
        else
          this.SetReadValue(parameterKey, parameterValue);
      }
      return true;
    }

    internal override SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType,
      int SubDevice)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for M5 radio2!");
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      }
      if (SubDevice != 0)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "M5 device has no sub devices!");
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      }
      this.ParameterList = new SortedList<OverrideID, ConfigurationParameter>();
      bool hasWritePermission1 = UserManager.CheckPermission(UserRights.Rights.MConfigSet1);
      bool hasWritePermission2 = UserManager.CheckPermission(UserRights.Rights.MConfigSet2);
      bool hasWritePermission3 = UserManager.CheckPermission(UserRights.Rights.MConfigSet4);
      this.AddParameter(0, OverrideID.DeviceName);
      this.AddParameter(0, OverrideID.FirmwareVersion);
      this.AddParameter(0, OverrideID.Signature);
      this.AddParameter(0, OverrideID.Manufacturer);
      this.AddParameter(false, 0, OverrideID.SerialNumber, "RAM;SerNo");
      if (ConfigurationType == ConfigurationParameter.ValueType.Direct || ConfigurationType == ConfigurationParameter.ValueType.Complete)
      {
        this.AddParameter(true, 0, OverrideID.DeviceHasError, "RAM;PermError", hasWritePermission3);
        this.AddParameter(true, 0, OverrideID.ErrorDate, "RAM;ErrDate", hasWritePermission3);
        this.AddParameter(true, 0, OverrideID.HCA_ActualValue, "RAM;Reading0", hasWritePermission3);
        this.AddParameter(true, 0, OverrideID.DeviceClock, "RAM;Date", hasWritePermission1);
        this.AddParameter(true, 0, OverrideID.DueDate, "RAM;KeyDate", hasWritePermission1);
        this.AddParameter(true, 0, OverrideID.HCA_Scale, "RAM;ProductScale", hasWritePermission2);
        this.AddParameter(true, 0, OverrideID.HCA_SensorMode, "RAM;DoubleSensor", hasWritePermission3);
        this.AddParameter(true, 0, OverrideID.HCA_Factor_Weighting, "RAM;KQ", hasWritePermission2);
        this.AddParameter(true, 0, OverrideID.HCA_Factor_CH, "RAM;c_H", hasWritePermission3);
        this.AddParameter(true, 0, OverrideID.HCA_Factor_CHR, "RAM;c_HR", hasWritePermission3);
        this.AddParameter(true, 0, OverrideID.SleepMode, "RAM;SleepMode", hasWritePermission3);
        string str = string.Format("ID:{0} Manufacturer:0x{1:X4} Status:0x{2} Signature:0x{3:X4} Flags:0x{4} Config:0x{5}", (object) this.SerialNumber, (object) this.Manufacturer, (object) this.GetParameterDataHexString("RAM;Status"), (object) this.Signature, (object) this.GetParameterDataHexString("RAM;Flags"), (object) this.GetParameterDataHexString("RAM;Config"));
        this.AddParameter(false, OverrideID.DiagnosticString, new ConfigurationParameter(OverrideID.DiagnosticString)
        {
          ParameterValue = (object) str,
          HasWritePermission = false,
          IsFunction = false
        });
      }
      return this.ParameterList;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: MinolHandler.ISF
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
  internal class ISF : MinolDevice
  {
    internal ISF(MinolHandlerFunctions Functions)
      : base(Functions)
    {
      this.DeviceType = DeviceTypes.ISF;
    }

    internal override int GetMaxSizeOfRequestBuffer() => 32;

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
      if (!UserManager.CheckPermission(UserRights.Rights.ISF))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for ISF!");
        return (object) null;
      }
      if (this.MyFunctions.MyDevices.ParameterAccessByName.ContainsKey(parameterKey))
      {
        ParameterAccess parameterAccess = this.MyFunctions.MyDevices.ParameterAccessByName[parameterKey];
        return parameterAccess.Name == "INFO;TotalFlowAdj" ? (object) Math.Round(Util.ToDecimal(parameterAccess.GetReadValue(this.Map, true)), 3) : parameterAccess.GetReadValue(this.Map);
      }
      if (!this.MyFunctions.MyDevices.BitAccessByBitName.ContainsKey(parameterKey))
        throw new ArgumentException("Unknown parameter! Key: " + parameterKey);
      ParameterAccess parameterAccess1 = this.MyFunctions.MyDevices.BitAccessByBitName[parameterKey];
      return this.MyFunctions.MyDevices.ParameterAccessByName[parameterAccess1.RangeName + ";" + parameterAccess1.TelegramPara.Parent].IsValueNull(this.Map) ? (object) false : (object) parameterAccess1.GetBitValue(this.Map);
    }

    internal override bool SetConfigurationParameters(
      SortedList<OverrideID, ConfigurationParameter> ParameterList,
      int SubDevice)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.ISF))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for ISF!");
        return false;
      }
      if (SubDevice != 0)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "ISF device has no sub devices!");
        return false;
      }
      foreach (KeyValuePair<OverrideID, ConfigurationParameter> parameter in ParameterList)
      {
        string parameterKey = parameter.Value.ParameterKey;
        object parameterValue = parameter.Value.ParameterValue;
        if (parameter.Key == OverrideID.VolumePulsValue)
        {
          double num = Convert.ToDouble(parameterValue);
          if (num == 0.0)
            throw new ArgumentException("The parameter 'VolumePulsValue' can not be 0!");
          this.SetReadValue("INFO;VPreLoad", (object) (1.0 / num));
        }
        else if (parameter.Key == OverrideID.StartCalibration)
        {
          this.SetReadValue("RAM;DAC1_min", (object) 0);
          this.SetReadValue("RAM;DAC1_cal", (object) 0);
          this.SetReadValue("RAM;DAC0_max", (object) 0);
          this.SetReadValue("RAM;DAC0_cal", (object) 0);
          this.SetReadValue("RAM;Status", (object) (byte) ((uint) Convert.ToByte(this.GetReadValue("RAM;Status")) & 4294967167U));
          this.SetReadValue("RAM;REQ_Timer", (object) true);
        }
        else if (parameter.Key != OverrideID.MaxFlow)
        {
          if (parameter.Key == OverrideID.PulseCorrectionEnabled)
          {
            bool boolean = Convert.ToBoolean(parameterValue);
            this.SetReadValue(parameterKey, (object) !boolean);
          }
          else
            this.SetReadValue(parameterKey, parameterValue);
        }
      }
      foreach (KeyValuePair<OverrideID, ConfigurationParameter> parameter in ParameterList)
      {
        if (parameter.Key == OverrideID.MaxFlow)
        {
          ushort uint16 = Convert.ToUInt16(this.GetReadValue("INFO;VPreLoad"));
          double num = Convert.ToDouble(parameter.Value.ParameterValue);
          if (uint16 == (ushort) 200 && num == 1200.0)
          {
            this.SetReadValue("INFO;SampleRate", (object) 9);
            this.SetReadValue("INFO;SampleRate2", (object) 17);
          }
          else if (uint16 == (ushort) 200 && num == 3000.0)
          {
            this.SetReadValue("INFO;SampleRate", (object) 19);
            this.SetReadValue("INFO;SampleRate2", (object) 24);
          }
          else if (uint16 == (ushort) 120 && num == 5000.0)
          {
            this.SetReadValue("INFO;SampleRate", (object) 19);
            this.SetReadValue("INFO;SampleRate2", (object) 24);
          }
          else if (uint16 == (ushort) 230 && num == 1200.0)
          {
            this.SetReadValue("INFO;SampleRate", (object) 11);
            this.SetReadValue("INFO;SampleRate2", (object) 19);
          }
          else if (uint16 == (ushort) 230 && num == 3000.0)
          {
            this.SetReadValue("INFO;SampleRate", (object) 21);
            this.SetReadValue("INFO;SampleRate2", (object) 25);
          }
          else
          {
            if (uint16 != (ushort) 135 || num != 5000.0)
              throw new NotSupportedException("This value of 'MaxFlow' is not supported! Value: " + num.ToString());
            this.SetReadValue("INFO;SampleRate", (object) 21);
            this.SetReadValue("INFO;SampleRate2", (object) 25);
          }
        }
      }
      return true;
    }

    internal override GlobalDeviceId GetGlobalDeviceId()
    {
      if (!UserManager.CheckPermission(UserRights.Rights.ISF))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for ISF!");
        return (GlobalDeviceId) null;
      }
      GlobalDeviceId globalDeviceId = new GlobalDeviceId()
      {
        Serialnumber = this.SerialNumber.ToString(),
        MeterType = ValueIdent.ValueIdPart_MeterType.VolumeMeter
      };
      globalDeviceId.DeviceTypeName = globalDeviceId.MeterType.ToString();
      globalDeviceId.Manufacturer = "MINOL";
      globalDeviceId.MeterNumber = this.SerialNumber.ToString();
      globalDeviceId.FirmwareVersion = this.FirmwareVersion.ToString();
      globalDeviceId.Generation = "2";
      return globalDeviceId;
    }

    internal override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      int SubDevice)
    {
      if (SubDevice != 0)
        throw new ArgumentOutOfRangeException(nameof (SubDevice), "ISF has no sub devices!");
      return this.GetValues(ref ValueList);
    }

    internal override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.ISF))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for ISF!");
        return false;
      }
      if (ValueList == null)
        ValueList = new SortedList<long, SortedList<DateTime, ReadingValue>>();
      else if (ValueList.Count > 0)
        new List<long>().AddRange((IEnumerable<long>) ValueList.Keys);
      return true;
    }

    internal override SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType,
      int SubDevice)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.ISF))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for ISF!");
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      }
      if (SubDevice != 0)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "ISF device has no sub devices!");
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      }
      try
      {
        this.ParameterList = new SortedList<OverrideID, ConfigurationParameter>();
        this.AddParameter(true, 0, OverrideID.TotalTestPulses, "RAM;TotalTPulses", true);
        this.AddParameter(true, 0, OverrideID.TotalVolumePulses, "RAM;TotalVPulses", true);
        this.AddParameter(true, 0, OverrideID.PulseCorrectionValue, "INFO;TotalFlowAdj", true, "%", (string[]) null, (object) -25.0, (object) 24.998);
        bool boolean = Convert.ToBoolean(this.GetReadValue("INFO;CFG_DisFlowCorr"));
        this.AddParameter(true, OverrideID.PulseCorrectionEnabled, new ConfigurationParameter(OverrideID.PulseCorrectionEnabled)
        {
          ParameterKey = "INFO;CFG_DisFlowCorr",
          ParameterValue = (object) !boolean,
          HasWritePermission = true,
          IsFunction = false
        });
        this.AddParameter(true, 0, OverrideID.SerialNumber, "INFO;SerNo", true);
        this.AddParameter(true, 0, OverrideID.DeviceHasError, "RAM;ERROR", true);
        this.AddParameter(true, 0, OverrideID.Protected, "RAM;Protected", true);
        this.AddParameter(false, 0, OverrideID.Calibrated, "RAM;Calibrated", false);
        this.AddParameter(true, OverrideID.StartCalibration, new ConfigurationParameter(OverrideID.StartCalibration)
        {
          ParameterValue = (object) false,
          HasWritePermission = true,
          IsFunction = true
        });
        ushort uint16 = Convert.ToUInt16(this.GetReadValue("INFO;VPreLoad"));
        byte num1 = Convert.ToByte(this.GetReadValue("INFO;SampleRate"));
        byte num2 = Convert.ToByte(this.GetReadValue("INFO;SampleRate2"));
        this.AddParameter(true, OverrideID.VolumePulsValue, new ConfigurationParameter(OverrideID.VolumePulsValue)
        {
          ParameterValue = (object) (uint16 > (ushort) 0 ? 1.0 / Convert.ToDouble(uint16) : 0.0),
          HasWritePermission = true,
          IsFunction = false
        });
        double? nullable = new double?();
        if (uint16 == (ushort) 200 && num1 == (byte) 19 && num2 == (byte) 24)
          nullable = new double?(3000.0);
        else if (uint16 == (ushort) 200 && num1 == (byte) 9 && num2 == (byte) 17)
          nullable = new double?(1200.0);
        else if (uint16 == (ushort) 120 && num1 == (byte) 19 && num2 == (byte) 24)
          nullable = new double?(5000.0);
        this.AddParameter(true, OverrideID.MaxFlow, new ConfigurationParameter(OverrideID.MaxFlow)
        {
          ParameterValue = (object) nullable,
          HasWritePermission = true,
          IsFunction = false,
          Unit = "l/h"
        });
        this.AddParameter(0, OverrideID.DeviceName);
        this.AddParameter(0, OverrideID.FirmwareVersion);
        this.AddParameter(0, OverrideID.Signature);
        this.AddParameter(0, OverrideID.Manufacturer);
        string str1 = string.Format("ID:{0} Manufacturer:0x{1:X4} Status:0x{2} Signature:0x{3:X4} CFG_Flags:0x{4}", (object) this.SerialNumber, (object) this.Manufacturer, (object) this.GetParameterDataHexString("RAM;Status"), (object) this.Signature, (object) this.GetParameterDataHexString("INFO;CFG_Flags"));
        this.AddParameter(false, OverrideID.DiagnosticString, new ConfigurationParameter(OverrideID.DiagnosticString)
        {
          ParameterValue = (object) str1,
          HasWritePermission = false,
          IsFunction = false
        });
        string str2 = string.Format("DAC1_min={0}; DAC1_cal={1}; DAC0_max={2}; DAC0_cal={3}", this.GetReadValue("RAM;DAC1_min"), this.GetReadValue("RAM;DAC1_cal"), this.GetReadValue("RAM;DAC0_max"), this.GetReadValue("RAM;DAC0_cal"));
        this.AddParameter(false, OverrideID.CalibrationValues, new ConfigurationParameter(OverrideID.CalibrationValues)
        {
          ParameterValue = (object) str2,
          HasWritePermission = false,
          IsFunction = false
        });
        return this.ParameterList;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Parameter generation error " + ex.Message);
        return this.ParameterList;
      }
    }

    internal override RangeSet<int> GetAddresses(AddressRange range)
    {
      RangeSet<int> addresses = new RangeSet<int>();
      if (range != AddressRange.BaseType)
        throw new NotSupportedException();
      addresses.Add(4132, 4159);
      return addresses;
    }
  }
}

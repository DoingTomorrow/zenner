// Decompiled with JetBrains decompiler
// Type: MinolHandler.M5p
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
  internal class M5p : MinolDevice
  {
    private static Logger logger = LogManager.GetLogger(nameof (M5p));
    private Decimal K_Factor = 0M;

    internal M5p(MinolHandlerFunctions Functions)
      : base(Functions)
    {
      this.DeviceType = DeviceTypes.EHCA_M5p;
    }

    internal override int GetMaxSizeOfRequestBuffer() => 80;

    internal override bool CreateDeviceData()
    {
      ParameterAccess parameterAccess = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;K"];
      if (parameterAccess != null)
        this.K_Factor = Util.ToDecimal(parameterAccess.GetReadValue(this.Map));
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
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for M5+ radio2!");
        return (object) false;
      }
      if (this.MyFunctions.MyDevices.ParameterAccessByName.ContainsKey(parameterKey))
      {
        ParameterAccess parameterAccess = this.MyFunctions.MyDevices.ParameterAccessByName[parameterKey];
        if (parameterAccess.Name == "RAM;Ticker")
        {
          long readValue1 = (long) this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Year"].GetReadValue(this.Map);
          long year = readValue1 >= 80L ? readValue1 + 1900L : readValue1 + 2000L;
          long readValue2 = (long) this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Month"].GetReadValue(this.Map);
          long readValue3 = (long) this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Day"].GetReadValue(this.Map);
          try
          {
            return (object) new DateTime((int) year, (int) readValue2, (int) readValue3).AddSeconds((double) (long) parameterAccess.GetReadValue(this.Map));
          }
          catch (Exception ex)
          {
            string str = string.Format("The device values (RAM;Year={0}, RAM;Month={1}, RAM;Day={2}) are invalid! Error: {3}", (object) year, (object) readValue2, (object) readValue3, (object) ex);
            M5p.logger.ErrorException(str, ex);
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, str);
            return (object) null;
          }
        }
        else
        {
          if (parameterAccess.Name == "RAM;Reading")
            return (object) Math.Round(Util.ToDecimal(parameterAccess.GetReadValue(this.Map)), 3);
          if (!(parameterAccess.Name == "RAM;SerNo"))
            return parameterAccess.GetReadValue(this.Map);
          object readValue = parameterAccess.GetReadValue(this.Map);
          return readValue != null ? (object) Convert.ToInt64("8" + Convert.ToInt64(readValue).ToString().PadLeft(8, '0')) : readValue;
        }
      }
      else
      {
        if (!this.MyFunctions.MyDevices.BitAccessByBitName.ContainsKey(parameterKey))
          throw new ArgumentException("Unknown parameter! Key: " + parameterKey);
        bool bitValue = this.MyFunctions.MyDevices.BitAccessByBitName[parameterKey].GetBitValue(this.Map);
        switch (parameterKey)
        {
          case "RAM;CFG_PROD":
            return bitValue ? (object) HCA_Scale.Product : (object) HCA_Scale.Uniform;
          case "RAM;MOD_1F":
            return bitValue ? (object) HCA_SensorMode.Single : (object) HCA_SensorMode.Double;
          default:
            return (object) bitValue;
        }
      }
    }

    internal override GlobalDeviceId GetGlobalDeviceId()
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for M5+ radio2!");
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
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for M5+ radio2!");
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
      long num1 = 272700040;
      if (ValueIdent.IsExpectedValueIdent(filter, num1) && readValue1 >= dateTime1 && readValue1 <= dateTime2)
      {
        object obj = this.GetReadValue("RAM;Reading");
        if (obj != null)
        {
          if (this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Reading"].TelegramPara.UseK)
            obj = (object) (Util.ToDouble(obj) * Util.ToDouble((object) this.K_Factor));
          this.AddValue(ref ValueList, readValue1, num1, obj);
        }
      }
      long num2 = 281088648;
      if (ValueIdent.IsExpectedValueIdent(filter, num2))
      {
        for (int index = 1; index <= 2; ++index)
        {
          object readValue2 = this.GetReadValue("RAM;DateStampY" + index.ToString("00"));
          if (readValue2 != null)
          {
            DateTime timePoint = (DateTime) readValue2;
            if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
            {
              object obj = this.GetReadValue("RAM;ReadingY" + index.ToString("00"));
              if (obj != null)
              {
                if (this.MyFunctions.MyDevices.ParameterAccessByName["RAM;ReadingY" + index.ToString("00")].TelegramPara.UseK)
                  obj = (object) (Util.ToDouble(obj) * Util.ToDouble((object) this.K_Factor));
                this.AddValue(ref ValueList, timePoint, num2, obj);
              }
            }
          }
        }
      }
      long num3 = 289477256;
      if (ValueIdent.IsExpectedValueIdent(filter, num3))
      {
        for (int index = 1; index <= 18; ++index)
        {
          DateTime timePoint = new DateTime(readValue1.Year, readValue1.Month, 1);
          timePoint = timePoint.AddMonths((index - 1) * -1);
          if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
          {
            object obj = this.GetReadValue("RAM;ReadingM" + index.ToString("00"));
            if (obj != null)
            {
              if (this.MyFunctions.MyDevices.ParameterAccessByName["RAM;ReadingM" + index.ToString("00")].TelegramPara.UseK)
                obj = (object) (Util.ToDouble(obj) * Util.ToDouble((object) this.K_Factor));
              this.AddValue(ref ValueList, timePoint, num3, obj);
            }
          }
        }
      }
      object readValue3 = this.GetReadValue("RAM;ERROR");
      if (readValue3 != null && Convert.ToBoolean(readValue3))
      {
        object readValue4 = this.GetReadValue("RAM;DateError");
        DateTime dateTime3;
        if (readValue4 != null && Util.TryParseToDateTime(readValue4.ToString(), out dateTime3))
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
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for M5+ radio2!");
        return false;
      }
      if (SubDevice != 0)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "M5-plus device has no sub devices!");
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
          }
          else
          {
            ParameterAccess parameterAccess1 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Ticker"];
            this.SetConfigurationParameters(parameterAccess1.TelegramPara.Address, parameterAccess1.GetValueAsByteArray(parameterValue1));
            ParameterAccess parameterAccess2 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Year"];
            this.SetConfigurationParameters(parameterAccess2.TelegramPara.Address, parameterAccess2.GetValueAsByteArray(parameterValue1));
            ParameterAccess parameterAccess3 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Month"];
            this.SetConfigurationParameters(parameterAccess3.TelegramPara.Address, parameterAccess3.GetValueAsByteArray(parameterValue1));
            ParameterAccess parameterAccess4 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Day"];
            this.SetConfigurationParameters(parameterAccess4.TelegramPara.Address, parameterAccess4.GetValueAsByteArray(parameterValue1));
          }
        }
        else if (parameterKey == "RAM;CFG_PROD" || parameter.Value.ParameterID == OverrideID.HCA_Scale)
        {
          if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet2))
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change the scale of HCA!");
          else if ((HCA_Scale) Enum.Parse(typeof (HCA_Scale), parameterValue1.ToString(), true) == HCA_Scale.Product)
          {
            this.SetReadValue(parameterKey, (object) 1);
          }
          else
          {
            this.SetReadValue("RAM;K", (object) 1);
            this.SetReadValue(parameterKey, (object) 0);
          }
        }
        else if (parameterKey == "RAM;MOD_1F" || parameter.Value.ParameterID == OverrideID.HCA_SensorMode)
        {
          if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet4))
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change the sensor mode of HCA!");
          else if ((HCA_SensorMode) Enum.Parse(typeof (HCA_SensorMode), parameterValue1.ToString(), true) == HCA_SensorMode.Single)
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented, "Can not set device from 2F (double sensor) in 1F (single sensor) mode! This function is not supported!");
          else if (!Convert.ToBoolean(this.MyFunctions.MyDevices.BitAccessByBitName["RAM;CFG_2F"].GetBitValue(this.Map)))
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! It is 1F (single sensor) device.");
          else if (Convert.ToBoolean(this.MyFunctions.MyDevices.BitAccessByBitName["RAM;CFG_FF"].GetBitValue(this.Map)))
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! It is no compact device.");
          else if (!Convert.ToBoolean(this.MyFunctions.MyDevices.BitAccessByBitName["RAM;CFG_2F_Algo"].GetBitValue(this.Map)))
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! 2F (double sensor) functionality is disabled for this device. (CFG_2F_Algo = 0)");
          else if (Convert.ToBoolean(this.MyFunctions.MyDevices.BitAccessByBitName["RAM;ERROR"].GetBitValue(this.Map)))
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! Device has errors.");
          else if (Convert.ToBoolean(this.MyFunctions.MyDevices.BitAccessByBitName["RAM;MOD_1F"].GetBitValue(this.Map)))
          {
            object readValue1 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;T_HS"].GetReadValue(this.Map);
            if (readValue1 == null)
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! Device has errors. T_HS = null");
            }
            else
            {
              object readValue2 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;T_RS"].GetReadValue(this.Map);
              if (readValue2 == null)
                ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! Device has errors. T_RS = null");
              else if (!this.SetReadValue("RAM;CNT_AVL", (object) 1000))
              {
                ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! Internal error while sets CNT_AVL to 1000.");
              }
              else
              {
                double parameterValue2 = 20.0;
                double num1 = Convert.ToDouble(readValue1);
                double num2 = Convert.ToDouble(readValue2);
                if (Math.Abs(num1 - num2) < 1.5)
                  parameterValue2 = (num1 + num2) / 2.0;
                else if (num2 <= 20.0)
                  parameterValue2 = num2;
                else if (num1 > num2 + 1.5 && num2 >= 20.0)
                  parameterValue2 = 20.0;
                double num3;
                if (!this.SetReadValue("RAM;T_L", (object) parameterValue2))
                  ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! Internal error while sets T_L to " + parameterValue2.ToString());
                else if (!this.SetReadValue("RAM;c_HR", (object) 0.53))
                {
                  num3 = 0.53;
                  ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! Internal error while sets c_HR to " + num3.ToString());
                }
                else if (!this.SetReadValue("RAM;c_HR_AV", (object) 0.53))
                {
                  num3 = 0.53;
                  ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! Internal error while sets c_HR_AV to " + num3.ToString());
                }
                else if (!this.SetReadValue("RAM;c_HR_AVL", (object) 0.53))
                {
                  num3 = 0.53;
                  ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! Internal error while sets c_HR_AVL to " + num3.ToString());
                }
                else if (!this.SetReadValue("RAM;CNT_TOTALMEAS", (object) 120))
                  ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! Internal error while sets CNT_TOTALMEAS to 120.");
                else
                  this.SetReadValue(parameterKey, (object) 0);
              }
            }
          }
        }
        else if (parameterKey == "RAM;DateStart" || parameter.Value.ParameterID == OverrideID.StartDate)
        {
          if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet4))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change start date of the device!");
          }
          else
          {
            if (parameterValue1 != null && parameterValue1 is DateTime dateTime && dateTime != DateTime.MinValue)
              this.SetReadValue("RAM;MEAS_STDBY", (object) true);
            else
              this.SetReadValue("RAM;MEAS_STDBY", (object) false);
            this.SetReadValue(parameterKey, parameterValue1);
          }
        }
        else if (parameterKey == "RAM;SLEEP" || parameter.Value.ParameterID == OverrideID.SleepMode)
        {
          if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet4))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change sleep parameter of the device!");
          }
          else
          {
            if (Convert.ToBoolean(parameterValue1))
              this.SetReadValue("RAM;DateOn", (object) DateTime.MinValue);
            this.SetReadValue(parameterKey, parameterValue1);
          }
        }
        else
          this.SetReadValue(parameterKey, parameterValue1);
      }
      return true;
    }

    internal override SortedList<OverrideID, ConfigurationParameter> GetConfigurationParameters(
      ConfigurationParameter.ValueType ConfigurationType,
      int SubDevice)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for M5+ radio2!");
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      }
      if (SubDevice != 0)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "M5+ device has no sub devices!");
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
        this.AddParameter(true, 0, OverrideID.DueDate, "RAM;DateKey", hasWritePermission1);
        this.AddParameter(true, 0, OverrideID.StartDate, "RAM;DateStart", hasWritePermission3);
        this.AddParameter(true, 0, OverrideID.DeviceClock, "RAM;Ticker", hasWritePermission1);
        this.AddParameter(true, 0, OverrideID.HCA_ActualValue, "RAM;Reading", hasWritePermission3);
        this.AddParameter(true, 0, OverrideID.DeviceHasError, "RAM;ERROR", hasWritePermission3);
        this.AddParameter(true, 0, OverrideID.ErrorDate, "RAM;DateError", hasWritePermission3);
        this.AddParameter(true, 0, OverrideID.HCA_Scale, "RAM;CFG_PROD", hasWritePermission2);
        this.AddParameter(true, 0, OverrideID.HCA_SensorMode, "RAM;MOD_1F", hasWritePermission3);
        this.AddParameter(true, 0, OverrideID.HCA_Factor_Weighting, "RAM;K", hasWritePermission2);
        this.AddParameter(true, 0, OverrideID.HCA_Factor_CHR, "RAM;c_HR", hasWritePermission3);
        this.AddParameter(true, 0, OverrideID.SleepMode, "RAM;SLEEP", hasWritePermission3);
        string str = string.Format("ID:{0} Manufacturer:0x{1:X4} Status:0x{2} Signature:0x{3:X4} CfgFlags1:0x{4} CfgFlags2:0x{5}", (object) this.SerialNumber, (object) this.Manufacturer, (object) this.GetParameterDataHexString("RAM;Status"), (object) this.Signature, (object) this.GetParameterDataHexString("RAM;CfgFlags1"), (object) this.GetParameterDataHexString("RAM;CfgFlags2"));
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

// Decompiled with JetBrains decompiler
// Type: MinolHandler.M6Radio3
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
  internal class M6Radio3 : MinolDevice
  {
    private static Logger logger = LogManager.GetLogger(nameof (M6Radio3));

    internal M6Radio3(MinolHandlerFunctions Functions)
      : base(Functions)
    {
      this.DeviceType = DeviceTypes.EHCA_M6_Radio3;
    }

    internal override int GetMaxSizeOfRequestBuffer() => 40;

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
      {
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
      else
      {
        switch (parameterKey)
        {
          case "RAM;DateToday":
            object readValue1 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;DateToday"].GetReadValue(this.Map);
            if (readValue1 == null)
              return (object) DateTime.MinValue;
            DateTime dateTime = (DateTime) readValue1;
            long readValue2 = (long) this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Minute"].GetReadValue(this.Map);
            dateTime = dateTime.AddMinutes((double) readValue2);
            long readValue3 = (long) this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Hour"].GetReadValue(this.Map);
            return (object) dateTime.AddHours((double) readValue3).AddSeconds((double) (long) this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Second"].GetReadValue(this.Map));
          case "RAM;Reading":
            return (object) Math.Round(Util.ToDecimal(this.MyFunctions.MyDevices.ParameterAccessByName[parameterKey].GetReadValue(this.Map)), 3);
          case "INFO;Radio2_Preload":
            return (object) (Util.ToInteger(this.MyFunctions.MyDevices.ParameterAccessByName["INFO;Radio2_Preload"].GetReadValue(this.Map)) * 2);
          case "INFO;Radio3_Preload":
            return (object) (Util.ToInteger(this.MyFunctions.MyDevices.ParameterAccessByName["INFO;Radio3_Preload"].GetReadValue(this.Map)) * 2);
          case "RAM;T_HS":
            return (object) Math.Round(Convert.ToDouble(this.MyFunctions.MyDevices.ParameterAccessByName["RAM;T_HS"].GetReadValue(this.Map)), 2);
          case "RAM;T_RS":
            return (object) Math.Round(Convert.ToDouble(this.MyFunctions.MyDevices.ParameterAccessByName["RAM;T_RS"].GetReadValue(this.Map)), 2);
          case "INFO;RadioEpsilonOffsetEnabled":
            int integer = Util.ToInteger(this.MyFunctions.MyDevices.ParameterAccessByName["INFO;Radio3_Epsilon"].GetReadValue(this.Map));
            int radio3EpsilonValue = MinolDevice.CalculateRadio3EpsilonValue(this.SerialNumber);
            if (radio3EpsilonValue == integer)
              return (object) true;
            if (integer != 0 && integer != (int) ushort.MaxValue)
              M6Radio3.logger.Warn<int, int>("Wrong radio 3 epsilon value! Expected=0x{0:X4}, Actual=0x{1:X4}", radio3EpsilonValue, integer);
            return (object) false;
          case "RAM;RadioProtocol":
            bool boolean1 = Convert.ToBoolean(this.MyFunctions.MyDevices.BitAccessByBitName["RAM;CFG_HH"].GetBitValue(this.Map));
            bool boolean2 = Convert.ToBoolean(this.MyFunctions.MyDevices.BitAccessByBitName["RAM;CFG_Radio3"].GetBitValue(this.Map));
            byte num = Convert.ToByte(this.MyFunctions.MyDevices.ParameterAccessByName["RAM;RF_Type_Info"].GetReadValue(this.Map));
            bool flag1 = (24 & (int) num) == 0;
            bool flag2 = (16 & (int) num) == 0 && (8 & (int) num) == 8;
            if (!boolean2)
              return (object) RadioProtocol.Scenario0;
            if (!(flag1 | flag2))
              M6Radio3.logger.Error("Unknown coding type detected. RF_Type_Info= 0x" + num.ToString("X2"));
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
            M6Radio3.logger.Error("Unknown scenario detected. RF_Type_Info= 0x" + num.ToString("X2") + " and CFG_HH=" + boolean1.ToString());
            return (object) RadioProtocol.Undefined;
          default:
            if (!this.MyFunctions.MyDevices.ParameterAccessByName.ContainsKey(parameterKey))
              throw new ArgumentException("Unknown parameter! Key: " + parameterKey);
            return this.MyFunctions.MyDevices.ParameterAccessByName[parameterKey].GetReadValue(this.Map);
        }
      }
    }

    internal override GlobalDeviceId GetGlobalDeviceId()
    {
      GlobalDeviceId globalDeviceId = new GlobalDeviceId()
      {
        Serialnumber = this.SerialNumber.ToString(),
        MeterType = ValueIdent.ValueIdPart_MeterType.HeatCostAllocator
      };
      globalDeviceId.DeviceTypeName = globalDeviceId.MeterType.ToString();
      globalDeviceId.Manufacturer = "MINOL";
      globalDeviceId.MeterNumber = this.SerialNumber.ToString();
      globalDeviceId.FirmwareVersion = this.FirmwareVersion.ToString();
      globalDeviceId.Generation = "3";
      return globalDeviceId;
    }

    internal override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList,
      int SubDevice)
    {
      if (SubDevice != 0)
        throw new ArgumentOutOfRangeException(nameof (SubDevice), "M7 has no sub devices!");
      return this.GetValues(ref ValueList);
    }

    internal override bool GetValues(
      ref SortedList<long, SortedList<DateTime, ReadingValue>> ValueList)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.Radio3))
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
      long num1 = 272700040;
      if (ValueIdent.IsExpectedValueIdent(filter, num1))
      {
        object readValue = this.GetReadValue("RAM;DateToday");
        if (readValue != null)
        {
          DateTime timePoint = (DateTime) readValue;
          if (timePoint >= dateTime1 && timePoint <= dateTime2)
          {
            Decimal num2 = Util.ToDecimal(this.GetReadValue("RAM;Reading"));
            this.AddValue(ref ValueList, timePoint, num1, (object) num2);
          }
        }
      }
      long num3 = 281088648;
      if (ValueIdent.IsExpectedValueIdent(filter, num3))
      {
        for (int index = 0; index < 2; ++index)
        {
          object readValue1 = this.GetReadValue("LOGM;DateStampY" + index.ToString("00"));
          if (readValue1 != null)
          {
            DateTime timePoint = (DateTime) readValue1;
            if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
            {
              object readValue2 = this.GetReadValue("LOGM;ReadingY" + index.ToString("00"));
              if (readValue2 != null)
              {
                Decimal num4 = Util.ToDecimal(readValue2);
                this.AddValue(ref ValueList, timePoint, num3, (object) num4);
              }
            }
          }
        }
      }
      long num5 = 289477256;
      if (ValueIdent.IsExpectedValueIdent(filter, num5))
      {
        for (int index = 1; index <= 18; ++index)
        {
          object readValue3 = this.GetReadValue("LOGM;DateStampM" + index.ToString("00"));
          if (readValue3 != null)
          {
            DateTime timePoint = (DateTime) readValue3;
            if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
            {
              object readValue4 = this.GetReadValue("LOGM;ReadingM" + index.ToString("00"));
              if (readValue4 != null)
              {
                Decimal num6 = Util.ToDecimal(readValue4);
                this.AddValue(ref ValueList, timePoint, num5, (object) num6);
              }
            }
          }
        }
      }
      long num7 = 293671560;
      if (ValueIdent.IsExpectedValueIdent(filter, num7))
      {
        object readValue5 = this.GetReadValue("LOGM;DateStampM00");
        if (readValue5 != null)
        {
          DateTime timePoint = (DateTime) readValue5;
          if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
          {
            object readValue6 = this.GetReadValue("LOGM;ReadingM00");
            if (readValue6 != null)
            {
              Decimal num8 = Util.ToDecimal(readValue6);
              this.AddValue(ref ValueList, timePoint, num7, (object) num8);
            }
          }
        }
      }
      long num9 = 302060168;
      if (ValueIdent.IsExpectedValueIdent(filter, num9))
      {
        for (int index = 1; index <= 18; ++index)
        {
          object readValue7 = this.GetReadValue("LOGD;DateStampD" + index.ToString("00"));
          if (readValue7 != null)
          {
            DateTime timePoint = (DateTime) readValue7;
            if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
            {
              object readValue8 = this.GetReadValue("LOGD;ReadingD" + index.ToString("00"));
              if (readValue8 != null)
              {
                Decimal num10 = Util.ToDecimal(readValue8);
                this.AddValue(ref ValueList, timePoint, num9, (object) num10);
              }
            }
          }
        }
      }
      Decimal num11 = Convert.ToDecimal(this.GetReadValue("RAM;K"));
      long num12 = 272700041;
      if (ValueIdent.IsExpectedValueIdent(filter, num12))
      {
        object readValue = this.GetReadValue("RAM;DateToday");
        if (readValue != null)
        {
          DateTime timePoint = (DateTime) readValue;
          if (timePoint >= dateTime1 && timePoint <= dateTime2)
          {
            Decimal num13 = Util.ToDecimal(this.GetReadValue("RAM;Reading"));
            this.AddValue(ref ValueList, timePoint, num12, (object) (num13 * num11));
          }
        }
      }
      long num14 = 281088649;
      if (ValueIdent.IsExpectedValueIdent(filter, num14))
      {
        for (int index = 0; index < 2; ++index)
        {
          object readValue9 = this.GetReadValue("LOGM;DateStampY" + index.ToString("00"));
          if (readValue9 != null)
          {
            DateTime timePoint = (DateTime) readValue9;
            if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
            {
              object readValue10 = this.GetReadValue("LOGM;ReadingY" + index.ToString("00"));
              if (readValue10 != null)
              {
                Decimal num15 = Util.ToDecimal(readValue10);
                this.AddValue(ref ValueList, timePoint, num14, (object) (num15 * num11));
              }
            }
          }
        }
      }
      long num16 = 289477257;
      if (ValueIdent.IsExpectedValueIdent(filter, num16))
      {
        for (int index = 1; index <= 18; ++index)
        {
          object readValue11 = this.GetReadValue("LOGM;DateStampM" + index.ToString("00"));
          if (readValue11 != null)
          {
            DateTime timePoint = (DateTime) readValue11;
            if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
            {
              object readValue12 = this.GetReadValue("LOGM;ReadingM" + index.ToString("00"));
              if (readValue12 != null)
              {
                Decimal num17 = Util.ToDecimal(readValue12);
                this.AddValue(ref ValueList, timePoint, num16, (object) (num17 * num11));
              }
            }
          }
        }
      }
      long num18 = 293671561;
      if (ValueIdent.IsExpectedValueIdent(filter, num18))
      {
        object readValue13 = this.GetReadValue("LOGM;DateStampM00");
        if (readValue13 != null)
        {
          DateTime timePoint = (DateTime) readValue13;
          if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
          {
            object readValue14 = this.GetReadValue("LOGM;ReadingM00");
            if (readValue14 != null)
            {
              Decimal num19 = Util.ToDecimal(readValue14);
              this.AddValue(ref ValueList, timePoint, num18, (object) (num19 * num11));
            }
          }
        }
      }
      long num20 = 302060169;
      if (ValueIdent.IsExpectedValueIdent(filter, num20))
      {
        for (int index = 1; index <= 18; ++index)
        {
          object readValue15 = this.GetReadValue("LOGD;DateStampD" + index.ToString("00"));
          if (readValue15 != null)
          {
            DateTime timePoint = (DateTime) readValue15;
            if (timePoint >= dateTime1 && timePoint <= dateTime2 && timePoint != DateTime.MinValue)
            {
              object readValue16 = this.GetReadValue("LOGD;ReadingD" + index.ToString("00"));
              if (readValue16 != null)
              {
                Decimal num21 = Util.ToDecimal(readValue16);
                this.AddValue(ref ValueList, timePoint, num20, (object) (num21 * num11));
              }
            }
          }
        }
      }
      object readValue17 = this.GetReadValue("RAM;ERROR");
      if (readValue17 != null && Convert.ToBoolean(readValue17))
      {
        object readValue18 = this.GetReadValue("RAM;DateError");
        DateTime dateTime3;
        if (readValue18 != null && Util.TryParseToDateTime(readValue18.ToString(), out dateTime3))
        {
          this.AddErrorValue(ValueList, ValueIdent.ValueIdPart_MeterType.HeatCostAllocator, ValueIdent.ValueIdentError.DeviceError, new DateTime?(dateTime3));
        }
        else
        {
          DateTime now = DateTime.Now;
          this.AddErrorValue(ValueList, ValueIdent.ValueIdPart_MeterType.HeatCostAllocator, ValueIdent.ValueIdentError.DeviceError, new DateTime?(new DateTime(now.Year, now.Month, now.Day)));
        }
      }
      object readValue19 = this.GetReadValue("RAM;MANIPULATION");
      if (readValue19 != null && Convert.ToBoolean(readValue19))
      {
        object readValue20 = this.GetReadValue("RAM;DateManipulation");
        DateTime dateTime4;
        if (readValue20 != null && Util.TryParseToDateTime(readValue20.ToString(), out dateTime4))
        {
          this.AddErrorValue(ValueList, ValueIdent.ValueIdPart_MeterType.HeatCostAllocator, ValueIdent.ValueIdentError.Manipulation, new DateTime?(dateTime4));
        }
        else
        {
          DateTime now = DateTime.Now;
          this.AddErrorValue(ValueList, ValueIdent.ValueIdPart_MeterType.HeatCostAllocator, ValueIdent.ValueIdentError.Manipulation, new DateTime?(new DateTime(now.Year, now.Month, now.Day)));
        }
      }
      return true;
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
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "M6 radio3 device has no sub devices!");
        return false;
      }
      foreach (KeyValuePair<OverrideID, ConfigurationParameter> parameter in ParameterList)
      {
        string parameterKey = parameter.Value.ParameterKey;
        object parameterValue1 = parameter.Value.ParameterValue;
        if (parameterKey == "RAM;DateToday" || parameter.Value.ParameterID == OverrideID.DeviceClock)
        {
          if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change the device clock!");
            continue;
          }
          ParameterAccess parameterAccess1 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Minute"];
          this.SetConfigurationParameters(parameterAccess1.TelegramPara.Address, parameterAccess1.GetValueAsByteArray(parameterValue1));
          ParameterAccess parameterAccess2 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Hour"];
          this.SetConfigurationParameters(parameterAccess2.TelegramPara.Address, parameterAccess2.GetValueAsByteArray(parameterValue1));
          ParameterAccess parameterAccess3 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;Second"];
          this.SetConfigurationParameters(parameterAccess3.TelegramPara.Address, parameterAccess3.GetValueAsByteArray(parameterValue1));
          ParameterAccess parameterAccess4 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;DateToday"];
          this.SetConfigurationParameters(parameterAccess4.TelegramPara.Address, parameterAccess4.GetValueAsByteArray(parameterValue1));
        }
        else if (parameterKey == "RAM;CFG_PROD" || parameter.Value.ParameterID == OverrideID.HCA_Scale)
        {
          if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet2))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change the scale of HCA!");
            continue;
          }
          if ((HCA_Scale) Enum.Parse(typeof (HCA_Scale), parameterValue1.ToString(), true) == HCA_Scale.Product)
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
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change the sensor mode of HCA!");
            continue;
          }
          if ((HCA_SensorMode) Enum.Parse(typeof (HCA_SensorMode), parameterValue1.ToString(), true) == HCA_SensorMode.Single)
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FunctionNotImplemented, "Can not set device from 2F (double sensor) in 1F (single sensor) mode! This function is not supported!");
            continue;
          }
          if (!Convert.ToBoolean(this.MyFunctions.MyDevices.BitAccessByBitName["RAM;HKVE_Type2F"].GetBitValue(this.Map)))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! It is 1F (single sensor) device.");
            continue;
          }
          if (Convert.ToBoolean(this.MyFunctions.MyDevices.BitAccessByBitName["INFO;CFG_FF"].GetBitValue(this.Map)))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! It is no compact device.");
            continue;
          }
          if (!Convert.ToBoolean(this.MyFunctions.MyDevices.BitAccessByBitName["INFO;CFG_2F_Algo"].GetBitValue(this.Map)))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! 2F (double sensor) functionality is disabled for this device. (CFG_2F_Algo = 0)");
            continue;
          }
          if (Convert.ToBoolean(this.MyFunctions.MyDevices.BitAccessByBitName["RAM;ERROR"].GetBitValue(this.Map)))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! Device has errors.");
            continue;
          }
          if (Convert.ToBoolean(this.MyFunctions.MyDevices.BitAccessByBitName["RAM;MOD_1F"].GetBitValue(this.Map)))
          {
            object readValue1 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;T_HS"].GetReadValue(this.Map);
            if (readValue1 == null)
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! Device has errors. T_HS = null");
              continue;
            }
            object readValue2 = this.MyFunctions.MyDevices.ParameterAccessByName["RAM;T_RS"].GetReadValue(this.Map);
            if (readValue2 == null)
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! Device has errors. T_RS = null");
              continue;
            }
            if (!this.SetReadValue("RAM;CNT_TL_AV", (object) 1000))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! Internal error while sets CNT_TL_AV to 1000.");
              continue;
            }
            double parameterValue2 = 20.0;
            double num1 = Convert.ToDouble(readValue1);
            double num2 = Convert.ToDouble(readValue2);
            if (Math.Abs(num1 - num2) < 1.5)
              parameterValue2 = (num1 + num2) / 2.0;
            else if (num2 <= 20.0)
              parameterValue2 = num2;
            else if (num1 > num2 + 1.5 && num2 >= 20.0)
              parameterValue2 = 20.0;
            if (!this.SetReadValue("RAM;T_L", (object) parameterValue2))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! Internal error while sets T_L to " + parameterValue2.ToString());
              continue;
            }
            double num3;
            if (!this.SetReadValue("RAM;c_HR", (object) 0.53))
            {
              num3 = 0.53;
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! Internal error while sets c_HR to " + num3.ToString());
              continue;
            }
            if (!this.SetReadValue("RAM;c_HR_AV", (object) 0.53))
            {
              num3 = 0.53;
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! Internal error while sets c_HR_AV to " + num3.ToString());
              continue;
            }
            if (!this.SetReadValue("RAM;c_HR_AVL", (object) 0.53))
            {
              num3 = 0.53;
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! Internal error while sets c_HR_AVL to " + num3.ToString());
              continue;
            }
            if (!this.SetReadValue("RAM;CNT_MEAS", (object) 120))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Can not set device to 2F (double sensor) mode! Internal error while sets CNT_MEAS to 120.");
              continue;
            }
            this.SetReadValue(parameterKey, (object) 0);
          }
          else
            continue;
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
              this.SetReadValue("INFO;Radio2_Preload", (object) (Convert.ToInt32(parameterValue1) / 2));
              goto label_142;
            case "INFO;Radio3_Preload":
              if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet4))
              {
                ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change the radio send interval!");
                continue;
              }
              this.SetReadValue("INFO;Radio3_Preload", (object) (Convert.ToInt32(parameterValue1) / 2));
              goto label_142;
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
            if (!(parameterValue1 is bool flag))
              flag = bool.Parse(parameterValue1.ToString());
            if (flag)
              this.SetReadValue("INFO;Radio3_Epsilon", (object) MinolDevice.CalculateRadio3EpsilonValue(this.SerialNumber));
            else
              this.SetReadValue("INFO;Radio3_Epsilon", (object) (int) ushort.MaxValue);
          }
          else if (parameterKey == "RAM;RadioProtocol" || parameter.Value.ParameterID == OverrideID.RadioProtocol)
          {
            if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet3))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change the radio protocol of HCA!");
              continue;
            }
            if (!Enum.IsDefined(typeof (RadioProtocol), parameterValue1))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Unknown scenario! Value: " + parameterValue1?.ToString());
              continue;
            }
            switch ((RadioProtocol) Enum.Parse(typeof (RadioProtocol), parameterValue1.ToString(), true))
            {
              case RadioProtocol.Scenario0:
                this.SetReadValue("RAM;CFG_HH", (object) false);
                this.SetReadValue("RAM;CFG_Radio3", (object) false);
                this.SetReadValue("RAM;RF_Type_Info", (object) 0);
                this.SetReadValue("RAM;TIMER_RF", (object) 10);
                break;
              case RadioProtocol.Scenario1:
                this.SetReadValue("RAM;CFG_HH", (object) false);
                this.SetReadValue("RAM;CFG_Radio3", (object) true);
                this.SetReadValue("RAM;RF_Type_Info", (object) 0);
                this.SetReadValue("RAM;TIMER_RF", (object) 10);
                this.ClearSavedLoggerValuesOfRadio3Device();
                break;
              case RadioProtocol.Scenario2:
                this.SetReadValue("RAM;CFG_HH", (object) false);
                this.SetReadValue("RAM;CFG_Radio3", (object) true);
                this.SetReadValue("RAM;RF_Type_Info", (object) 32);
                this.SetReadValue("RAM;TIMER_RF", (object) 10);
                this.ClearSavedLoggerValuesOfRadio3Device();
                break;
              case RadioProtocol.Scenario3:
                this.SetReadValue("RAM;CFG_HH", (object) true);
                this.SetReadValue("RAM;CFG_Radio3", (object) true);
                this.SetReadValue("RAM;RF_Type_Info", (object) 0);
                this.SetReadValue("RAM;TIMER_RF", (object) 10);
                break;
              case RadioProtocol.Undefined:
                break;
              default:
                M6Radio3.logger.Fatal("Unknown RadioMode=" + ((RadioProtocol) parameterValue1).ToString());
                break;
            }
          }
          else if (parameterKey == "RAM;MANIPULATION" || parameter.Value.ParameterID == OverrideID.Manipulation)
          {
            if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to reset manipulation flag!");
              continue;
            }
            if (Convert.ToBoolean(parameterValue1) && !UserManager.CheckPermission(UserRights.Rights.Developer))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "Is not allowed to set the manipulation flag! Only reset is allowed.");
              continue;
            }
            if (!Convert.ToBoolean(parameterValue1))
            {
              this.SetReadValue("RAM;DateManipulation", (object) new DateTime(2000, 1, 1));
              this.SetReadValue(parameterKey, (object) false);
              this.SetReadValue("RAM;Manip_Active", (object) false);
              this.SetReadValue("RAM;TIMER_MANIP", this.GetReadValue("INFO;Manip_Preload1"));
            }
            else
              this.SetReadValue(parameterKey, (object) true);
          }
          else if (parameterKey == "RAM;DateManipulation" || parameter.Value.ParameterID == OverrideID.ManipulationDate)
          {
            if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to reset manipulation date!");
              continue;
            }
            if (Convert.ToDateTime(parameterValue1) != new DateTime(2000, 1, 1) && !UserManager.CheckPermission(UserRights.Rights.Developer))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "Is not allowed to set the manipulation date! Only reset is allowed (01/01/2000).");
              continue;
            }
            this.SetReadValue(parameterKey, parameterValue1);
          }
          else if (parameterKey == "RAM;Reading" || parameter.Value.ParameterID == OverrideID.HCA_ActualValue)
          {
            if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet4))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change actual measurement value!");
              continue;
            }
            this.SetReadValue(parameterKey, parameterValue1);
          }
          else if (parameterKey == "RAM;DateError" || parameter.Value.ParameterID == OverrideID.ErrorDate)
          {
            if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet4))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change error date of the device!");
              continue;
            }
            if (Convert.ToDateTime(parameterValue1) != new DateTime(2000, 1, 1) && !UserManager.CheckPermission(UserRights.Rights.Developer))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "Is not allowed to set the error date! Only reset is allowed (01/01/2000).");
              continue;
            }
            this.SetReadValue(parameterKey, parameterValue1);
          }
          else if (parameterKey == "RAM;ERROR" || parameter.Value.ParameterID == OverrideID.DeviceHasError)
          {
            if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet4))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change error flag of the device!");
              continue;
            }
            if (Convert.ToBoolean(parameterValue1) && !UserManager.CheckPermission(UserRights.Rights.Developer))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "Is not allowed to set the error flag! Only reset is allowed.");
              continue;
            }
            this.SetReadValue(parameterKey, parameterValue1);
          }
          else if (parameterKey == "RAM;DateKey" || parameter.Value.ParameterID == OverrideID.DueDate)
          {
            if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet1))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change due date of the device!");
              continue;
            }
            this.SetReadValue(parameterKey, parameterValue1);
          }
          else if (parameterKey == "RAM;DateStart" || parameter.Value.ParameterID == OverrideID.StartDate)
          {
            if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet4))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change start date of the device!");
              continue;
            }
            if (parameterValue1 != null && parameterValue1 is DateTime dateTime && dateTime != DateTime.MinValue)
              this.SetReadValue("RAM;MEAS_STDBY", (object) true);
            else
              this.SetReadValue("RAM;MEAS_STDBY", (object) false);
            this.SetReadValue(parameterKey, parameterValue1);
          }
          else if (parameterKey == "RAM;MEAS_STDBY" || parameter.Value.ParameterID == OverrideID.Standby)
          {
            if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet4))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change standby property of the device!");
              continue;
            }
            if (parameterValue1 != null && Convert.ToBoolean(parameterValue1))
              this.SetReadValue("RAM;MEAS_STDBY", (object) true);
            else
              this.SetReadValue("RAM;MEAS_STDBY", (object) false);
          }
          else if (parameterKey == "RAM;K" || parameter.Value.ParameterID == OverrideID.HCA_Factor_Weighting)
          {
            if (!UserManager.CheckPermission(UserRights.Rights.MConfigSet2))
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission to change factor weighting of the device!");
              continue;
            }
            this.SetReadValue(parameterKey, parameterValue1);
          }
          else if (parameterKey == "RAM;SLEEP" || parameter.Value.ParameterID == OverrideID.SleepMode)
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
          else if (parameterKey == "RAM;REQ_Exit_SLEEP" || parameter.Value.ParameterID == OverrideID.ExitSleep)
            this.SetReadValue(parameterKey, parameterValue1);
          else
            this.SetReadValue(parameterKey, parameterValue1);
        }
label_142:;
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
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "M6 radio3 device has no sub devices!");
        return (SortedList<OverrideID, ConfigurationParameter>) null;
      }
      try
      {
        this.ParameterList = new SortedList<OverrideID, ConfigurationParameter>();
        bool hasWritePermission1 = UserManager.CheckPermission(UserRights.Rights.MConfigSet1);
        bool hasWritePermission2 = UserManager.CheckPermission(UserRights.Rights.MConfigSet2);
        bool hasWritePermission3 = UserManager.CheckPermission(UserRights.Rights.MConfigSet3);
        bool hasWritePermission4 = UserManager.CheckPermission(UserRights.Rights.MConfigSet4);
        this.AddParameter(0, OverrideID.DeviceName);
        this.AddParameter(false, 0, OverrideID.SerialNumber, "INFO;SerNo");
        this.AddParameter(0, OverrideID.FirmwareVersion);
        this.AddParameter(0, OverrideID.Signature);
        this.AddParameter(0, OverrideID.Manufacturer);
        if (ConfigurationType == ConfigurationParameter.ValueType.Direct || ConfigurationType == ConfigurationParameter.ValueType.Complete)
        {
          this.AddParameter(true, 0, OverrideID.DueDate, "RAM;DateKey", hasWritePermission1);
          this.AddParameter(true, 0, OverrideID.StartDate, "RAM;DateStart", hasWritePermission4);
          this.AddParameter(true, 0, OverrideID.DeviceClock, "RAM;DateToday", hasWritePermission1);
          this.AddParameter(true, 0, OverrideID.HCA_ActualValue, "RAM;Reading", hasWritePermission4);
          this.AddParameter(true, 0, OverrideID.Manipulation, "RAM;MANIPULATION", hasWritePermission1);
          this.AddParameter(true, 0, OverrideID.ManipulationDate, "RAM;DateManipulation", hasWritePermission1);
          this.AddParameter(true, 0, OverrideID.DeviceHasError, "RAM;ERROR", hasWritePermission4);
          this.AddParameter(true, 0, OverrideID.ErrorDate, "RAM;DateError", hasWritePermission4);
          this.AddParameter(true, 0, OverrideID.HCA_SensorMode, "RAM;MOD_1F", hasWritePermission4);
          this.AddParameter(true, 0, OverrideID.HCA_Scale, "RAM;CFG_PROD", hasWritePermission2);
          this.AddParameter(true, 0, OverrideID.HCA_Factor_Weighting, "RAM;K", hasWritePermission2);
          this.AddParameter(true, 0, OverrideID.RadioProtocol, "RAM;RadioProtocol", hasWritePermission3);
          this.AddParameter(true, 0, OverrideID.RadioEpsilonOffsetEnabled, "INFO;RadioEpsilonOffsetEnabled", hasWritePermission4);
          this.AddParameter(true, 0, OverrideID.SleepMode, "RAM;SLEEP", hasWritePermission4);
          this.AddParameter(true, 0, OverrideID.ExitSleep, "RAM;REQ_Exit_SLEEP", hasWritePermission4);
          this.AddParameter(true, 0, OverrideID.Standby, "RAM;MEAS_STDBY", hasWritePermission4);
          this.AddParameter(false, 0, OverrideID.TemperaturRadiator, "RAM;T_HS");
          this.AddParameter(false, 0, OverrideID.TemperaturRoom, "RAM;T_RS");
          this.AddParameter(false, OverrideID.RadioFrequence, new ConfigurationParameter(OverrideID.RadioFrequence, (object) this.GetRadioFrequence())
          {
            HasWritePermission = false
          });
          if (Convert.ToBoolean(this.MyFunctions.MyDevices.BitAccessByBitName["RAM;CFG_Radio3"].GetBitValue(this.Map)))
            this.AddParameter(true, 0, OverrideID.RadioSendInterval, "INFO;Radio3_Preload", hasWritePermission4, "s", (string[]) null, (object) 16, (object) 131070);
          else
            this.AddParameter(true, 0, OverrideID.RadioSendInterval, "INFO;Radio2_Preload", hasWritePermission4, "s", (string[]) null, (object) 300, (object) 300);
          string str = string.Format("ID:{0} Manufacturer:0x{1:X4} Status:0x{2} Signature:0x{3:X4} CfgFlags_RAM:0x{4} Flags:0x{5} CFG_Flags:0x{6}", (object) this.SerialNumber, (object) this.Manufacturer, (object) this.GetParameterDataHexString("RAM;Status"), (object) this.Signature, (object) this.GetParameterDataHexString("RAM;CfgFlags_RAM"), (object) this.GetParameterDataHexString("RAM;Flags"), (object) this.GetParameterDataHexString("INFO;CFG_Flags"));
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

    internal override bool SetTestParameter(TestParameter testParameter)
    {
      try
      {
        this.SetReadValue("RAM;Minute", (object) testParameter.DeviceDateTime);
        this.SetReadValue("RAM;Hour", (object) testParameter.DeviceDateTime);
        this.SetReadValue("RAM;Second", (object) testParameter.DeviceDateTime);
        this.SetReadValue("RAM;DateToday", (object) testParameter.DeviceDateTime);
        this.SetConfigurationParameters(513, new byte[1]
        {
          (byte) 192
        });
        this.SetConfigurationParameters(520, new byte[2]
        {
          (byte) 1,
          (byte) 1
        });
        this.SetConfigurationParameters(522, new byte[2]
        {
          (byte) 1,
          (byte) 6
        });
        this.SetConfigurationParameters(524, new byte[2]
        {
          (byte) 1,
          (byte) 9
        });
        this.SetConfigurationParameters(526, new byte[2]
        {
          (byte) 97,
          (byte) 17
        });
        this.SetConfigurationParameters(528, new byte[2]
        {
          (byte) 97,
          (byte) 17
        });
        this.SetConfigurationParameters(530, new byte[2]);
        this.SetConfigurationParameters(532, new byte[2]);
        this.SetConfigurationParameters(534, new byte[2]);
        this.SetConfigurationParameters(537, new byte[1]
        {
          (byte) 8
        });
        this.SetReadValue("RAM;Reading", (object) testParameter.DeviceNumber);
        this.SetConfigurationParameters(544, new byte[4]
        {
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue
        });
        this.SetConfigurationParameters(572, new byte[2]
        {
          (byte) 232,
          (byte) 3
        });
        this.SetConfigurationParameters(587, new byte[1]
        {
          (byte) 3
        });
        this.SetConfigurationParameters(588, new byte[1]);
        this.SetConfigurationParameters(618, new byte[2]
        {
          (byte) 160,
          (byte) 5
        });
        this.SetConfigurationParameters(620, new byte[2]
        {
          (byte) 16,
          (byte) 0
        });
        this.SetConfigurationParameters(628, new byte[1]);
        this.SetConfigurationParameters(768, new byte[1]
        {
          (byte) 17
        });
        DateTime deviceDateTime = testParameter.DeviceDateTime;
        this.SetConfigurationParameters(770, Util.ConvertLongToByteArray(ParameterAccess.ConvertDateTimeTo_YYYYMMMMYYYDDDDD(new DateTime(deviceDateTime.Year, 1, 1)), 2));
        deviceDateTime = testParameter.DeviceDateTime;
        this.SetConfigurationParameters(772, Util.ConvertLongToByteArray(ParameterAccess.ConvertDateTimeTo_YYYYMMMMYYYDDDDD(new DateTime(deviceDateTime.Year - 1, 1, 1)), 2));
        deviceDateTime = testParameter.DeviceDateTime;
        int year1 = deviceDateTime.Year;
        deviceDateTime = testParameter.DeviceDateTime;
        int month1 = deviceDateTime.Month;
        this.SetConfigurationParameters(774, Util.ConvertLongToByteArray(ParameterAccess.ConvertDateTimeTo_YYYYMMMMYYYDDDDD(new DateTime(year1, month1, 16)), 2));
        deviceDateTime = testParameter.DeviceDateTime;
        int year2 = deviceDateTime.Year;
        deviceDateTime = testParameter.DeviceDateTime;
        int month2 = deviceDateTime.Month;
        DateTime dateTime = new DateTime(year2, month2, 1);
        int num1 = 1;
        int address1 = 776;
        while (num1 <= 18)
        {
          byte[] byteArray = Util.ConvertLongToByteArray(ParameterAccess.ConvertDateTimeTo_YYYYMMMMYYYDDDDD(dateTime.AddMonths(num1 - 18)), 2);
          this.SetConfigurationParameters(address1, byteArray);
          ++num1;
          address1 += 2;
        }
        this.SetConfigurationParameters(1196, Util.ConvertLongToByteArray((long) int.Parse(string.Format("{0:00}010", (object) testParameter.DeviceNumber)), 2));
        this.SetConfigurationParameters(1198, Util.ConvertLongToByteArray((long) int.Parse(string.Format("{0:00}020", (object) testParameter.DeviceNumber)), 2));
        this.SetConfigurationParameters(1200, Util.ConvertLongToByteArray((long) int.Parse(string.Format("{0:00}030", (object) testParameter.DeviceNumber)), 2));
        int num2 = 1;
        int address2 = 1202;
        while (num2 <= 18)
        {
          byte[] byteArray = Util.ConvertLongToByteArray((long) int.Parse(string.Format("{0:00}{1:00}0", (object) testParameter.DeviceNumber, (object) (10 + num2))), 2);
          this.SetConfigurationParameters(address2, byteArray);
          ++num2;
          address2 += 2;
        }
        this.SetConfigurationParameters(4134, new byte[2]
        {
          (byte) 90,
          (byte) 0
        });
        this.SetConfigurationParameters(4166, new byte[2]
        {
          byte.MaxValue,
          byte.MaxValue
        });
        this.SetConfigurationParameters(4176, new byte[2]
        {
          (byte) 160,
          (byte) 5
        });
        this.SetConfigurationParameters(4178, new byte[2]
        {
          (byte) 160,
          (byte) 5
        });
      }
      catch (Exception ex)
      {
        M6Radio3.logger.Error(ex.Message, ex);
        return false;
      }
      return true;
    }
  }
}

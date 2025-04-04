// Decompiled with JetBrains decompiler
// Type: TH_Handler.TH_Meter
// Assembly: TH_Handler, Version=1.3.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 02D62764-6653-46F8-9117-1BC5233AD061
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\TH_Handler.dll

using HandlerLib;
using NLog;
using System;
using System.Globalization;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace TH_Handler
{
  public sealed class TH_Meter : IMeter
  {
    private static Logger logger = LogManager.GetLogger(nameof (TH_Meter));
    public static readonly DateTime DateTimeNull = new DateTime(2000, 1, 1);

    public TH_MemoryMap Map { get; private set; }

    public TH_Meter(TH_MemoryMap map) => this.Map = map;

    public TH_Meter DeepCopy() => new TH_Meter(this.Map.DeepCopy());

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      int totalWidth = 60;
      int num = 37;
      if (this.Map == null)
        return string.Empty;
      try
      {
        stringBuilder.AppendLine(this.Map.Version.ToString(num));
        stringBuilder.AppendLine("Device Identification ".PadRight(totalWidth, '-'));
        TH_DeviceIdentification deviceIdentification = this.GetDeviceIdentification();
        stringBuilder.AppendLine(deviceIdentification != null ? deviceIdentification.ToString(num) : "not defined");
        stringBuilder.AppendLine("Parameter ".PadRight(totalWidth, '-'));
        foreach (Parameter parameter in this.Map.Parameters)
        {
          if (parameter.Size != 0 && !(parameter.Segment == "SEGMENT"))
          {
            stringBuilder.Append("0x" + parameter.Address.ToString("X4") + "   ");
            if (!this.Map.AreBytesDefined(parameter.Address, parameter.Size))
            {
              stringBuilder.Append(parameter.Name.PadRight(num, ' ')).AppendLine("!!!!!! NOT AVAILABLE !!!!!!");
            }
            else
            {
              byte[] parameterValue = this.GetParameterValue<byte[]>(parameter.Name);
              if (parameterValue != null)
              {
                stringBuilder.Append(parameter.Name.PadRight(num, ' '));
                for (int index = parameterValue.Length - 1; index >= 0; --index)
                  stringBuilder.Append(parameterValue[index].ToString("X2"));
                stringBuilder.AppendLine();
              }
              else
                stringBuilder.Append(parameter.Name.PadRight(num, ' ')).AppendLine(" ERROR ");
            }
          }
        }
        stringBuilder.Append("Read ").Append(this.Map.GetDefinedBytesCount().ToString()).AppendLine(" bytes.");
      }
      catch (Exception ex)
      {
        stringBuilder.AppendLine("INTERNAL ERROR: ").AppendLine(ex.Message);
      }
      return stringBuilder.ToString();
    }

    private T GetParameterValue<T>(string parameterName)
    {
      Parameter p = !string.IsNullOrEmpty(parameterName) ? this.Map.GetParameter(parameterName) : throw new ArgumentException(nameof (parameterName));
      if (p == null)
        throw new ArgumentException("Access of an unknown parameter! Name: " + parameterName);
      if (!this.Map.AreBytesDefined(p.Address, p.Size))
        throw new ArgumentException("No bytes are available at this address! Parameter name: " + parameterName);
      if (typeof (T) == typeof (short))
        return (T) (System.ValueType) BitConverter.ToInt16(this.Map.GetMemoryBytes(p), 0);
      if (typeof (T) == typeof (ushort))
        return (T) (System.ValueType) BitConverter.ToUInt16(this.Map.GetMemoryBytes(p), 0);
      if (typeof (T) == typeof (byte))
        return (T) (System.ValueType) this.Map.GetMemoryBytes(p)[0];
      if (typeof (T) == typeof (sbyte))
        return (T) (System.ValueType) (sbyte) this.Map.GetMemoryBytes(p)[0];
      if (typeof (T) == typeof (byte[]))
        return (T) this.Map.GetMemoryBytes(p);
      if (typeof (T) == typeof (uint))
        return (T) (System.ValueType) BitConverter.ToUInt32(this.Map.GetMemoryBytes(p), 0);
      if (typeof (T) == typeof (int))
        return (T) (System.ValueType) BitConverter.ToInt32(this.Map.GetMemoryBytes(p), 0);
      if (typeof (T) == typeof (ulong))
        return (T) (System.ValueType) BitConverter.ToUInt64(this.Map.GetMemoryBytes(p), 0);
      if (typeof (T) == typeof (long))
        return (T) (System.ValueType) BitConverter.ToInt64(this.Map.GetMemoryBytes(p), 0);
      if (!(typeof (T) == typeof (DateTime)))
        throw new NotImplementedException("INTERNAL ERROR: Can not cast the Value. The type is not implemented. Type: " + typeof (T)?.ToString());
      byte[] memoryBytes = this.Map.GetMemoryBytes(p);
      byte num = memoryBytes[0];
      byte month = memoryBytes[1];
      byte day = memoryBytes[2];
      byte hour = memoryBytes[3];
      byte minute = memoryBytes[4];
      byte second = memoryBytes[5];
      if (num == byte.MaxValue || month == byte.MaxValue || day == byte.MaxValue || hour == byte.MaxValue || minute == byte.MaxValue || second == byte.MaxValue)
        return (T) (System.ValueType) TH_Meter.DateTimeNull;
      if (num > byte.MaxValue || month > (byte) 12 || day > (byte) 31 || minute > (byte) 59 || second > (byte) 59)
        return (T) (System.ValueType) TH_Meter.DateTimeNull;
      try
      {
        return (T) (System.ValueType) new DateTime(2000 + (int) num, (int) month, (int) day, (int) hour, (int) minute, (int) second);
      }
      catch
      {
        return (T) (System.ValueType) TH_Meter.DateTimeNull;
      }
    }

    private bool SetParameterValue<T>(string parameterName, T newValue)
    {
      CultureInfo invariantCulture = CultureInfo.InvariantCulture;
      Parameter parameter = this.Map.GetParameter(parameterName);
      if (parameter == null)
        throw new ArgumentException("Access of an unknown parameter! Name: " + parameterName);
      if (!this.Map.AreBytesDefined(parameter.Address, parameter.Size))
        throw new ArgumentException("No bytes are available at this address! Parameter name: " + parameterName);
      if (typeof (T) == typeof (short))
      {
        byte[] bytes = BitConverter.GetBytes((short) Convert.ChangeType((object) newValue, typeof (T), (IFormatProvider) invariantCulture));
        return this.Map.SetMemoryBytes(parameter.Address, bytes);
      }
      if (typeof (T) == typeof (ushort))
      {
        byte[] bytes = BitConverter.GetBytes((ushort) Convert.ChangeType((object) newValue, typeof (T), (IFormatProvider) invariantCulture));
        return this.Map.SetMemoryBytes(parameter.Address, bytes);
      }
      if (typeof (T) == typeof (int))
      {
        byte[] bytes = BitConverter.GetBytes((int) Convert.ChangeType((object) newValue, typeof (T), (IFormatProvider) invariantCulture));
        return this.Map.SetMemoryBytes(parameter.Address, bytes);
      }
      if (typeof (T) == typeof (uint))
      {
        byte[] bytes = BitConverter.GetBytes((uint) Convert.ChangeType((object) newValue, typeof (T), (IFormatProvider) invariantCulture));
        return this.Map.SetMemoryBytes(parameter.Address, bytes);
      }
      if (typeof (T) == typeof (byte))
      {
        byte num = (byte) Convert.ChangeType((object) newValue, typeof (T), (IFormatProvider) invariantCulture);
        return this.Map.SetMemoryBytes(parameter.Address, new byte[1]
        {
          num
        });
      }
      if (typeof (T) == typeof (sbyte))
      {
        sbyte num = (sbyte) Convert.ChangeType((object) newValue, typeof (T), (IFormatProvider) invariantCulture);
        return this.Map.SetMemoryBytes(parameter.Address, new byte[1]
        {
          (byte) num
        });
      }
      if (typeof (T) == typeof (byte[]))
      {
        byte[] buffer = (byte[]) Convert.ChangeType((object) newValue, typeof (T), (IFormatProvider) invariantCulture);
        return this.Map.SetMemoryBytes(parameter.Address, buffer);
      }
      if (typeof (T) == typeof (long))
      {
        byte[] bytes = BitConverter.GetBytes((long) Convert.ChangeType((object) newValue, typeof (T), (IFormatProvider) invariantCulture));
        return this.Map.SetMemoryBytes(parameter.Address, bytes);
      }
      if (typeof (T) == typeof (ulong))
      {
        byte[] bytes = BitConverter.GetBytes((ulong) Convert.ChangeType((object) newValue, typeof (T), (IFormatProvider) invariantCulture));
        return this.Map.SetMemoryBytes(parameter.Address, bytes);
      }
      if (!(typeof (T) == typeof (DateTime)))
        throw new NotImplementedException("INTERNAL ERROR: Can not convert value to byte array. The type is not implemented. Type: " + typeof (T)?.ToString());
      DateTime dateTime = (DateTime) Convert.ChangeType((object) newValue, typeof (T), (IFormatProvider) invariantCulture);
      if (dateTime.Year < 2000)
        throw new ArgumentOutOfRangeException("Invalid date time! The year should be greater or equal to 2000. Value: " + dateTime.ToString("G"));
      if (dateTime.Year > 2255)
        throw new ArgumentOutOfRangeException("Invalid date time! The year should be smaller as 2255. Value: " + dateTime.ToString("G"));
      byte[] buffer1 = new byte[6]
      {
        (byte) (dateTime.Year - 2000),
        (byte) dateTime.Month,
        (byte) dateTime.Day,
        (byte) dateTime.Hour,
        (byte) dateTime.Minute,
        (byte) dateTime.Second
      };
      return this.Map.SetMemoryBytes(parameter.Address, buffer1);
    }

    private bool SetBit<T>(string parameterName, T mask) where T : struct
    {
      return this.ChangeBit(parameterName, true, this.GetBytes<T>(mask));
    }

    private bool ClearBit<T>(string parameterName, T mask) where T : struct
    {
      return this.ChangeBit(parameterName, false, this.GetBytes<T>(mask));
    }

    private bool ChangeBit(string parameterName, bool isSet, byte[] mask)
    {
      Parameter parameter = this.Map.GetParameter(parameterName);
      if (parameter == null)
        throw new ArgumentException("Access of an unknown parameter! Name: " + parameterName);
      if (parameter.Size != mask.Length)
        throw new ArgumentException("Invalid size! Name: " + parameterName);
      byte[] memoryBytes = this.Map.GetMemoryBytes(parameter);
      if (memoryBytes == null)
        return false;
      for (int index = 0; index < memoryBytes.Length; ++index)
      {
        if (isSet)
          memoryBytes[index] |= mask[index];
        else
          memoryBytes[index] &= ~mask[index];
      }
      this.Map.SetMemoryBytes(parameter.Address, memoryBytes);
      return true;
    }

    private bool GetBit<T>(string parameterName, T mask) where T : struct
    {
      return this.GetBit(parameterName, this.GetBytes<T>(mask));
    }

    private bool GetBit(string parameterName, byte[] mask)
    {
      Parameter parameter = this.Map.GetParameter(parameterName);
      if (parameter == null)
        throw new ArgumentException("Access of an unknown parameter! Name: " + parameterName);
      if (parameter.Size != mask.Length)
        throw new ArgumentException("Invalid size! Name: " + parameterName);
      byte[] memoryBytes = this.Map.GetMemoryBytes(parameter);
      if (memoryBytes == null)
        return false;
      for (int index = 0; index < memoryBytes.Length; ++index)
      {
        if (mask[index] != (byte) 0 && (int) (byte) ((uint) memoryBytes[index] & (uint) mask[index]) == (int) mask[index])
          return true;
      }
      return false;
    }

    private byte[] GetBytes<T>(T type) where T : struct
    {
      if (typeof (T) == typeof (byte))
        return new byte[1]{ Convert.ToByte((object) type) };
      if (typeof (T) == typeof (ushort))
        return BitConverter.GetBytes(Convert.ToUInt16((object) type));
      throw new NotImplementedException("INTERNAL ERROR: Can not get bytes of the generic type. The type is not implemented. Type: " + typeof (T)?.ToString());
    }

    public static TH_Meter Unzip(byte[] buffer)
    {
      TH_MemoryMap map = buffer != null ? TH_MemoryMap.Unzip(buffer) : throw new ArgumentNullException("The parameter 'buffer' can not be null!");
      return map == null ? (TH_Meter) null : new TH_Meter(map);
    }

    public TH_DeviceIdentification GetDeviceIdentification()
    {
      return TH_DeviceIdentification.Parse(this.GetParameterValue<uint>("Con_MeterId"), this.GetParameterValue<uint>("Con_HardwareTypeId"), this.GetParameterValue<uint>("Con_MeterInfoId"), this.GetParameterValue<uint>("Con_BaseTypeId"), this.GetParameterValue<uint>("Con_MeterTypeId"), this.GetParameterValue<uint>("Con_SAP_MaterialNumber"), this.GetParameterValue<uint>("Con_SAP_ProductionOrderNumber"), this.GetParameterValue<byte[]>("Con_fullserialnumber"), this.GetParameterValue<byte[]>("Con_fullserialnumberA"), this.GetParameterValue<byte[]>("Con_fullserialnumberB"), this.GetParameterValue<ushort>("Con_IdentificationChecksum"));
    }

    public void SetDeviceIdentification(TH_DeviceIdentification identBlock)
    {
      if (identBlock == null)
        throw new ArgumentException(nameof (identBlock));
      this.SetParameterValue<uint>("Con_MeterId", identBlock.MeterID.HasValue ? identBlock.MeterID.Value : uint.MaxValue);
      this.SetParameterValue<uint>("Con_HardwareTypeId", identBlock.HardwareTypeID.HasValue ? identBlock.HardwareTypeID.Value : uint.MaxValue);
      this.SetParameterValue<uint>("Con_MeterInfoId", identBlock.MeterInfoID.HasValue ? identBlock.MeterInfoID.Value : uint.MaxValue);
      this.SetParameterValue<uint>("Con_BaseTypeId", identBlock.BaseTypeID.HasValue ? identBlock.BaseTypeID.Value : uint.MaxValue);
      this.SetParameterValue<uint>("Con_MeterTypeId", identBlock.MeterTypeID.HasValue ? identBlock.MeterTypeID.Value : uint.MaxValue);
      this.SetParameterValue<uint>("Con_SAP_MaterialNumber", identBlock.SapMaterialNumber.HasValue ? identBlock.SapMaterialNumber.Value : uint.MaxValue);
      this.SetParameterValue<uint>("Con_SAP_ProductionOrderNumber", identBlock.SapProductionOrderNumber.HasValue ? identBlock.SapProductionOrderNumber.Value : uint.MaxValue);
      byte[] newValue1;
      if (identBlock.Con_fullserialnumber == null)
        newValue1 = new byte[8]
        {
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue
        };
      else
        newValue1 = identBlock.Con_fullserialnumber;
      this.SetParameterValue<byte[]>("Con_fullserialnumber", newValue1);
      byte[] newValue2;
      if (identBlock.Con_fullserialnumberA == null)
        newValue2 = new byte[8]
        {
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue
        };
      else
        newValue2 = identBlock.Con_fullserialnumberA;
      this.SetParameterValue<byte[]>("Con_fullserialnumberA", newValue2);
      byte[] newValue3;
      if (identBlock.Con_fullserialnumberB == null)
        newValue3 = new byte[8]
        {
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue,
          byte.MaxValue
        };
      else
        newValue3 = identBlock.Con_fullserialnumberB;
      this.SetParameterValue<byte[]>("Con_fullserialnumberB", newValue3);
      this.SetParameterValue<ushort>("Con_IdentificationChecksum", identBlock.Con_IdentificationChecksum);
    }

    public bool SetVersion(byte value) => this.SetParameterValue<byte>("cfg_version", value);

    public byte GetVersion() => this.GetParameterValue<byte>("cfg_version");

    public bool SetSerial(uint value)
    {
      return this.SetParameterValue<uint>("cfg_serial", Util.ConvertUnt32ToBcdUInt32(value));
    }

    public uint GetSerial()
    {
      return Util.ConvertBcdUInt32ToUInt32(this.GetParameterValue<uint>("cfg_serial"));
    }

    public bool SetConfigFlags(ConfigFlags value)
    {
      return this.SetParameterValue<ushort>("cfg_config_flags", (ushort) value);
    }

    public ConfigFlags GetConfigFlags()
    {
      return (ConfigFlags) this.GetParameterValue<ushort>("cfg_config_flags");
    }

    public bool SetTactileSwCycle(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_tactile_sw_cycle", value);
    }

    public ushort GetTactileSwCycle() => this.GetParameterValue<ushort>("cfg_tactile_sw_cycle");

    public bool SetTactileSwInstall(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_tactile_sw_install", value);
    }

    public ushort GetTactileSwInstall() => this.GetParameterValue<ushort>("cfg_tactile_sw_install");

    public bool SetTactileSwRemoval(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_tactile_sw_removal", value);
    }

    public ushort GetTactileSwRemoval() => this.GetParameterValue<ushort>("cfg_tactile_sw_removal");

    public bool SetRadioFlags(RadioFlags value)
    {
      return this.SetParameterValue<byte>("cfg_radio_flags", (byte) value);
    }

    public RadioFlags GetRadioFlags()
    {
      return (RadioFlags) this.GetParameterValue<byte>("cfg_radio_flags");
    }

    public bool SetRadioMode(RadioMode value)
    {
      return this.SetParameterValue<byte>("cfg_radio_mode", (byte) value);
    }

    public RadioMode GetRadioMode() => (RadioMode) this.GetParameterValue<byte>("cfg_radio_mode");

    public bool SetRadioScenario(RadioScenario value)
    {
      return this.SetParameterValue<byte>("cfg_radio_scenario", (byte) value);
    }

    public RadioScenario GetRadioScenario()
    {
      return (RadioScenario) this.GetParameterValue<byte>("cfg_radio_scenario");
    }

    public bool SetRadioInstallCount(byte value)
    {
      return this.SetParameterValue<byte>("cfg_radio_install_count", value);
    }

    public byte GetRadioInstallCount() => this.GetParameterValue<byte>("cfg_radio_install_count");

    public bool SetRadioTimeBias(short value)
    {
      return this.SetParameterValue<short>("cfg_radio_time_bias", value);
    }

    public short GetRadioTimeBias() => this.GetParameterValue<short>("cfg_radio_time_bias");

    public bool SetRadioNormalBasetime(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_radio_normal_basetime", value);
    }

    public ushort GetRadioNormalBasetime()
    {
      return this.GetParameterValue<ushort>("cfg_radio_normal_basetime");
    }

    public bool SetRadioInstallBasetime(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_radio_install_basetime", value);
    }

    public ushort GetRadioInstallBasetime()
    {
      return this.GetParameterValue<ushort>("cfg_radio_install_basetime");
    }

    public bool SetRadioPacketBOffset(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_radio_packetb_offset", value);
    }

    public ushort GetRadioPacketBOffset()
    {
      return this.GetParameterValue<ushort>("cfg_radio_packetb_offset");
    }

    public bool SetRadioPower(RadioPower value)
    {
      return this.SetParameterValue<byte>("cfg_radio_power", (byte) value);
    }

    public RadioPower GetRadioPower()
    {
      return (RadioPower) this.GetParameterValue<byte>("cfg_radio_power");
    }

    public bool SetRadioFreqOffset(short value)
    {
      return this.SetParameterValue<short>("cfg_radio_freq_offset", value);
    }

    public short GetRadioFreqOffset() => this.GetParameterValue<short>("cfg_radio_freq_offset");

    public bool SetHumidityThreshold(short value)
    {
      return this.SetParameterValue<short>("cfg_humidity_threshold", value);
    }

    public short GetHumidityThreshold() => this.GetParameterValue<short>("cfg_humidity_threshold");

    public bool SetSensorCycle(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_sensor_cycle", value);
    }

    public ushort GetSensorCycle() => this.GetParameterValue<ushort>("cfg_sensor_cycle");

    public bool SetLCDTestTiming(ushort value)
    {
      return this.SetParameterValue<ushort>("cfg_LCD_test_timing", value);
    }

    public ushort GetLCDTestTiming() => this.GetParameterValue<ushort>("cfg_LCD_test_timing");

    public bool SetLCDSegmentTest(LcdTest value)
    {
      return this.SetParameterValue<byte>("cfg_LCD_segment_test", (byte) value);
    }

    public LcdTest GetLCDSegmentTest()
    {
      return (LcdTest) this.GetParameterValue<byte>("cfg_LCD_segment_test");
    }

    public bool SetLCDBlinkingCycle(byte value)
    {
      return this.SetParameterValue<byte>("cfg_LCD_blinking_cycle", value);
    }

    public byte GetLCDBlinkingCycle() => this.GetParameterValue<byte>("cfg_LCD_blinking_cycle");

    public double GetHumidityValue()
    {
      return (double) this.GetParameterValue<short>("meterValueRH") / 10.0;
    }

    public double GetTemperatureValue()
    {
      return (double) this.GetParameterValue<short>("meterValueT") / 10.0;
    }

    public bool SetLowBatt(DateTime? value)
    {
      if (!value.HasValue)
        return this.SetParameterValue<byte>("cfg_lowbatt_year", byte.MaxValue) && this.SetParameterValue<byte>("cfg_lowbatt_month", byte.MaxValue) && this.SetParameterValue<byte>("cfg_lowbatt_day", byte.MaxValue);
      if (value.Value.Year < 2000 || value.Value.Year > 2255)
        throw new ArgumentException("Can not set BatteryEndDate! The year of new value is invalid (Valid are: 2000-2255). Value: " + value.ToString());
      DateTime dateTime = value.Value;
      int num;
      if (this.SetParameterValue<byte>("cfg_lowbatt_year", (byte) (dateTime.Year - 2000)))
      {
        dateTime = value.Value;
        if (this.SetParameterValue<byte>("cfg_lowbatt_month", (byte) dateTime.Month))
        {
          dateTime = value.Value;
          num = this.SetParameterValue<byte>("cfg_lowbatt_day", (byte) dateTime.Day) ? 1 : 0;
          goto label_8;
        }
      }
      num = 0;
label_8:
      return num != 0;
    }

    public DateTime? GetLowBatt()
    {
      byte parameterValue1 = this.GetParameterValue<byte>("cfg_lowbatt_year");
      byte parameterValue2 = this.GetParameterValue<byte>("cfg_lowbatt_month");
      byte parameterValue3 = this.GetParameterValue<byte>("cfg_lowbatt_day");
      if (parameterValue1 == byte.MaxValue || parameterValue2 == byte.MaxValue || parameterValue3 == byte.MaxValue)
        return new DateTime?();
      if (parameterValue1 > byte.MaxValue || parameterValue2 > (byte) 12 || parameterValue3 > (byte) 31)
        return new DateTime?();
      try
      {
        return new DateTime?(new DateTime(2000 + (int) parameterValue1, (int) parameterValue2, (int) parameterValue3));
      }
      catch
      {
        return new DateTime?();
      }
    }

    public bool SetTimeZone(int valueInQuarterHours)
    {
      if (valueInQuarterHours > 56)
        throw new ArgumentOutOfRangeException("Invalid value of 'Timezone'! Too big. Max. UTC+14:00 (14*4=56), Min. UTC-12:00 (-12*4=-48), Actual value is: " + valueInQuarterHours.ToString());
      return valueInQuarterHours >= -48 ? this.SetParameterValue<byte>("Bak_TimeZoneInQuarterHours", (byte) valueInQuarterHours) : throw new ArgumentOutOfRangeException("Invalid value of 'Timezone'! Too small. Max. UTC+14:00 (14*4=56), Min. UTC-12:00 (-12*4=-48), Actual value is: " + valueInQuarterHours.ToString());
    }

    public int GetTimeZone()
    {
      byte parameterValue = this.GetParameterValue<byte>("Bak_TimeZoneInQuarterHours");
      int num = ((int) parameterValue & 128) != 128 ? (int) parameterValue : (int) parameterValue | -256;
      return num > 56 || num < -48 ? 0 : num;
    }

    public string GetInfo() => this.ToString();
  }
}

// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ConfigurationParameter
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#nullable disable
namespace ZR_ClassLibrary
{
  public class ConfigurationParameter : IComparable<ConfigurationParameter>
  {
    private static Logger ConfigParamLogger = LogManager.GetLogger(nameof (ConfigurationParameter));
    public static ConfigurationLevel ActiveConfigurationLevel = ConfigurationLevel.Native;
    public readonly OverrideID ParameterID;
    private object _parameterValue;
    public object MinParameterValue = (object) null;
    public object MaxParameterValue = (object) null;
    public int SubDevice;
    protected Decimal TrueDivisor;
    private ConfigurationParameter.MeterStringFormater MyStringFormater;
    public static SortedList<OverrideID, ConfigurationParameter.ConPaInfo> ConfigParametersByOverrideID = new SortedList<OverrideID, ConfigurationParameter.ConPaInfo>();
    private static ConfigurationParameter.StringMSF StringMSF_fun = new ConfigurationParameter.StringMSF();
    private static ConfigurationParameter.BoolMSF BoolMSF_fun = new ConfigurationParameter.BoolMSF();
    private static ConfigurationParameter.UlongMSF UlongMSF_fun = new ConfigurationParameter.UlongMSF();
    private static ConfigurationParameter.IntMSF intMSF_fun = new ConfigurationParameter.IntMSF();
    private static ConfigurationParameter.UIntMSF uintMSF_fun = new ConfigurationParameter.UIntMSF();
    private static ConfigurationParameter.doubleMSF doubleMSF_fun = new ConfigurationParameter.doubleMSF();
    private static ConfigurationParameter.DecimalMSF DecimalMSF_fun = new ConfigurationParameter.DecimalMSF();
    private static ConfigurationParameter.DateTimeMSF DateTimeMSF_fun = new ConfigurationParameter.DateTimeMSF();
    private static ConfigurationParameter.YearMSF YearMSF_fun = new ConfigurationParameter.YearMSF();
    private static ConfigurationParameter.TemperatureMSF TemperatureMSF_fun = new ConfigurationParameter.TemperatureMSF();
    private static ConfigurationParameter.MinolMeterDeviceListMSF MinolMeterDeviceListMSF_fun = new ConfigurationParameter.MinolMeterDeviceListMSF();
    private static ConfigurationParameter.UShortMSF UshortMSF_fun = new ConfigurationParameter.UShortMSF();
    private static ConfigurationParameter.ShortMSF ShortMSF_fun = new ConfigurationParameter.ShortMSF();
    private static ConfigurationParameter.ByteMSF byteMSF_fun = new ConfigurationParameter.ByteMSF();
    private static ConfigurationParameter.FloatMSF floatMSF_fun = new ConfigurationParameter.FloatMSF();
    private static ConfigurationParameter.UintHexKey8 uintHexKey8_fun = new ConfigurationParameter.UintHexKey8();
    private static ConfigurationParameter.UlongHexKey16 ulongHexKey16_fun = new ConfigurationParameter.UlongHexKey16();
    private static ConfigurationParameter.ByteArrayHexKey32 byteArrayHexKey32_fun = new ConfigurationParameter.ByteArrayHexKey32();
    private static ConfigurationParameter.Enum_MSF Enum_MSF_fun = new ConfigurationParameter.Enum_MSF();
    private static ConfigurationParameter.SelectListMSF SelectListMSF_fun = new ConfigurationParameter.SelectListMSF();

    public string ParameterKey { get; set; }

    public object ParameterValue
    {
      get => this._parameterValue;
      set
      {
        try
        {
          object defaultValue = this.ParameterInfo.DefaultValue;
          if (value != null)
          {
            Type type = defaultValue.GetType();
            if (value.GetType() != type)
            {
              string message = "Illegal type for ConfigurationParameter " + this.ParameterID.ToString() + Environment.NewLine + "Required Type: " + ConfigurationParameter.ConfigParametersByOverrideID[this.ParameterID].DefaultValue.GetType().ToString() + Environment.NewLine + "Used Type: " + value.GetType().ToString();
              if (this.ParameterInfo.FormatControlled)
                throw new ArgumentException(message);
              ConfigurationParameter.ConfigParamLogger.Trace(message);
            }
            else if (type == typeof (byte[]) && ((byte[]) defaultValue).Length != ((byte[]) value).Length)
            {
              string[] strArray = new string[8]
              {
                "Illegal byte[] length for ConfigurationParameter ",
                this.ParameterID.ToString(),
                Environment.NewLine,
                "Required byte[] length: ",
                null,
                null,
                null,
                null
              };
              int length = ((byte[]) defaultValue).Length;
              strArray[4] = length.ToString();
              strArray[5] = Environment.NewLine;
              strArray[6] = "Used byte[] length: : ";
              length = ((byte[]) value).Length;
              strArray[7] = length.ToString();
              string message = string.Concat(strArray);
              if (this.ParameterInfo.FormatControlled)
                throw new ArgumentException(message);
              ConfigurationParameter.ConfigParamLogger.Trace(message);
            }
          }
        }
        catch (Exception ex)
        {
          throw new ArgumentException("ParameterValue check error on ConfigurationParameter " + this.ParameterID.ToString() + Environment.NewLine + ex.Message);
        }
        this._parameterValue = value;
      }
    }

    public Type ParameterType => this.ParameterInfo.DefaultValue.GetType();

    public string Unit { get; set; }

    public string Format { get; set; }

    public bool HasWritePermission { get; set; }

    public bool IsEditable { get; set; }

    public bool IsFunction { get; set; }

    public string[] AllowedValues { get; set; }

    public ConfigurationParameter.ConPaInfo ParameterInfo { get; private set; }

    public override string ToString() => this.GetStringValueWin();

    public void Pars(string StringValue) => this.SetValueFromStringWin(StringValue);

    public string GetStringValueDb() => this.MyStringFormater.GetStringValueDb(this);

    public virtual string GetStringValueWin() => this.MyStringFormater.GetStringValueWin(this);

    public void SetValueFromStringDb(string StringValue)
    {
      this.MyStringFormater.SetValueFromStringDb(StringValue, this);
    }

    public virtual void SetValueFromStringWin(string StringValue)
    {
      this.MyStringFormater.SetValueFromStringWin(StringValue, this);
    }

    static ConfigurationParameter()
    {
      ConfigurationParameter.AddCoPaInfo(OverrideID.PrintedSerialNumber, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.DeviceInformation, 1, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MBusAddress, true, OverrideID.Unknown, 0, false, (object) 0UL, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UlongMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.SerialNumber, false, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.DeviceInformation, 2, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MeterID, true, OverrideID.Unknown, 0, false, (object) 0UL, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UlongMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 3, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.BaseTypeID, true, OverrideID.Unknown, 0, false, (object) 0UL, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UlongMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.NominalFlow, true, OverrideID.Unknown, 0, false, (object) 0.0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Flow, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.DeviceName, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.DeviceInformation, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Medium, true, OverrideID.Unknown, 0, false, (object) MBusDeviceType.UNKNOWN, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.Enum_MBusMediumMSF(), ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Manufacturer, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CustomID, true, OverrideID.Unknown, 0, false, (object) 0UL, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UlongMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MBusIdentificationNo, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.FactoryTypeID, true, OverrideID.Unknown, 0, false, (object) 0UL, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UlongMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.SerialNumberFull, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.SapNumber, true, OverrideID.Unknown, 0, false, (object) 0U, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.uintMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.OrderNumber, true, OverrideID.Unknown, 0, false, (object) 0L, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UlongMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MBusGeneration, true, OverrideID.Unknown, 0, false, (object) (byte) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.byteMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.SerialNumberSecondary, true, OverrideID.Unknown, 0, false, (object) 0U, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.uintMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.FirmwareVersion, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.LoRaWanVersion, true, OverrideID.Unknown, 0, true, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.LoRaVersion, true, OverrideID.Unknown, 0, true, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RadioVersion, true, OverrideID.Unknown, 0, true, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Signature, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.ModuleType, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Protected, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.NumberOfSubDevices, true, OverrideID.Unknown, 0, false, (object) 0UL, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UlongMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.ErrorDate, true, OverrideID.Unknown, 0, false, (object) DateTime.MinValue, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DateTimeMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.DiagnosticString, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.DeviceHasError, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.LastErrorDate, true, OverrideID.Unknown, 0, false, (object) DateTime.MinValue, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DateTimeMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Manipulation, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.ManipulationDate, true, OverrideID.Unknown, 0, false, (object) DateTime.MinValue, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DateTimeMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.OperatingHours, true, OverrideID.Unknown, 0, false, (object) 0L, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UlongMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.OperatingHours, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RadioTechnology, true, OverrideID.Unknown, 0, false, (object) RadioTechnology.None, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.Enum_MSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Radio, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RadioFrequence, true, OverrideID.Unknown, 0, false, (object) (ushort) 0, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.Enum_RadioFrequence_MSF(), ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Frequence, true, OverrideID.Unknown, 0, false, (object) 0.0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RadioSendInterval, true, OverrideID.Unknown, 0, false, (object) 0UL, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UlongMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RadioSendOffset, true, OverrideID.Unknown, 0, false, (object) 0UL, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UlongMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RadioProtocol, true, OverrideID.Unknown, 0, false, (object) RadioProtocol.Undefined, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.Enum_RadioMode_MSF(), ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RadioEnabled, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RadioEpsilonOffsetEnabled, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RadioActiveMonday, true, OverrideID.Unknown, 0, false, (object) true, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Radio, 1, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RadioActiveTuesday, true, OverrideID.Unknown, 0, false, (object) true, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Radio, 2, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RadioActiveWednesday, true, OverrideID.Unknown, 0, false, (object) true, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Radio, 3, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RadioActiveThursday, true, OverrideID.Unknown, 0, false, (object) true, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Radio, 4, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RadioActiveFriday, true, OverrideID.Unknown, 0, false, (object) true, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Radio, 5, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RadioActiveSaturday, true, OverrideID.Unknown, 0, false, (object) true, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Radio, 6, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RadioActiveSunday, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Radio, 7, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RadioActiveStartTime, true, OverrideID.Unknown, 0, false, (object) (byte) 5, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.byteMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Radio, 8, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RadioActiveStopTime, true, OverrideID.Unknown, 0, false, (object) (byte) 23, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.byteMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Radio, 9, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.JoinEUI, true, OverrideID.Unknown, 0, false, (object) 0UL, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.ulongHexKey16_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.DevEUI, true, OverrideID.Unknown, 0, false, (object) 0UL, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.ulongHexKey16_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.AppKey, true, OverrideID.Unknown, 0, false, (object) new byte[16], (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.byteArrayHexKey32_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.NwkSKey, true, OverrideID.Unknown, 0, false, (object) new byte[16], (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.byteArrayHexKey32_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.AppSKey, true, OverrideID.Unknown, 0, false, (object) new byte[16], (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.byteArrayHexKey32_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.DevAddr, true, OverrideID.Unknown, 0, false, (object) 0U, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.uintHexKey8_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.NetID, true, OverrideID.Unknown, 0, false, (object) 0U, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.uintHexKey8_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.TransmissionScenario, true, OverrideID.Unknown, 0, false, (object) (byte) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.byteMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RadioScenario, true, OverrideID.Unknown, 0, false, (object) RadioScenario.Scenario_201_LoRaMonthly, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.Enum_MSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CommunicationScenario, true, OverrideID.Unknown, 0, false, (object) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.intMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CommunicationScenarioLoRa, true, OverrideID.Unknown, 0, false, (object) (ushort) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UshortMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CommunicationScenarioWmbus, true, OverrideID.Unknown, 0, false, (object) (ushort) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UshortMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.ADR, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Region, true, OverrideID.Unknown, 0, false, (object) Region.EU_863_870, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.Enum_Region_MSF(), ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CalVolMaxFlowLiterPerHour, true, OverrideID.Unknown, 0, false, (object) double.NaN, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Flow, ConfigurationGroup.VolumeCalibration, 1000, ConfigurationLevel.Native | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CalVolMaxErrorPercent, true, OverrideID.Unknown, 0, false, (object) double.NaN, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Percent, ConfigurationGroup.VolumeCalibration, 1010, ConfigurationLevel.Native | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CalVolNominalFlowLiterPerHour, true, OverrideID.Unknown, 0, false, (object) double.NaN, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Flow, ConfigurationGroup.VolumeCalibration, 2000, ConfigurationLevel.Native | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CalVolNominalErrorPercent, true, OverrideID.Unknown, 0, false, (object) double.NaN, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Percent, ConfigurationGroup.VolumeCalibration, 2010, ConfigurationLevel.Native | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CalVolMinFlowLiterPerHour, true, OverrideID.Unknown, 0, false, (object) double.NaN, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Flow, ConfigurationGroup.VolumeCalibration, 3000, ConfigurationLevel.Native | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CalVolMinErrorPercent, true, OverrideID.Unknown, 0, false, (object) double.NaN, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Percent, ConfigurationGroup.VolumeCalibration, 3010, ConfigurationLevel.Native | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CalFlowTempMinGrad, true, OverrideID.Unknown, 0, false, (object) double.NaN, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Temperature, ConfigurationGroup.TemperatureCalibration, 1000, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CalFlowTempMinErrorPercent, true, OverrideID.Unknown, 0, false, (object) double.NaN, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Percent, ConfigurationGroup.TemperatureCalibration, 1010, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CalFlowTempMiddleGrad, true, OverrideID.Unknown, 0, false, (object) double.NaN, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Temperature, ConfigurationGroup.TemperatureCalibration, 1020, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CalFlowTempMiddleErrorPercent, true, OverrideID.Unknown, 0, false, (object) double.NaN, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Percent, ConfigurationGroup.TemperatureCalibration, 1030, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CalFlowTempMaxGrad, true, OverrideID.Unknown, 0, false, (object) double.NaN, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Temperature, ConfigurationGroup.TemperatureCalibration, 1040, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CalFlowTempMaxErrorPercent, true, OverrideID.Unknown, 0, false, (object) double.NaN, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Percent, ConfigurationGroup.TemperatureCalibration, 1050, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CalReturnTempMinGrad, true, OverrideID.Unknown, 0, false, (object) double.NaN, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Temperature, ConfigurationGroup.TemperatureCalibration, 2000, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CalReturnTempMinErrorPercent, true, OverrideID.Unknown, 0, false, (object) double.NaN, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Percent, ConfigurationGroup.TemperatureCalibration, 2010, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CalReturnTempMiddleGrad, true, OverrideID.Unknown, 0, false, (object) double.NaN, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Temperature, ConfigurationGroup.TemperatureCalibration, 2020, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CalReturnTempMiddleErrorPercent, true, OverrideID.Unknown, 0, false, (object) double.NaN, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Percent, ConfigurationGroup.TemperatureCalibration, 2030, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CalReturnTempMaxGrad, true, OverrideID.Unknown, 0, false, (object) double.NaN, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Temperature, ConfigurationGroup.TemperatureCalibration, 2040, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CalReturnTempMaxErrorPercent, true, OverrideID.Unknown, 0, false, (object) double.NaN, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Percent, ConfigurationGroup.TemperatureCalibration, 2050, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.TestVolumeSimulation, false, OverrideID.Unknown, 0, false, (object) double.NaN, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Volume, ConfigurationGroup.TemperatureCalibration, 2050, ConfigurationLevel.Native | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView01, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 1000, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView02, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 2000, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView03, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 3000, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView04, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 4000, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView05, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 5000, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView06, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 6000, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView07, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 7000, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView08, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 8000, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView09, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 9000, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView10, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 10000, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView01_Sel, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 1010, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView02_Sel, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 2010, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView03_Sel, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 3010, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView04_Sel, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 4010, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView05_Sel, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 5010, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView06_Sel, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 6010, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView07_Sel, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 7010, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView08_Sel, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 8010, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView09_Sel, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 9010, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView10_Sel, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 10010, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView01_Time, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 1020, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView02_Time, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 2020, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView03_Time, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 3020, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView04_Time, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 4020, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView05_Time, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 5020, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView06_Time, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 6020, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView07_Time, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 7020, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView08_Time, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 8020, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView09_Time, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 9020, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MenuView10_Time, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MenuDefinition, 10020, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.DisplayMenu, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.VolumeResolution, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.VolumePulsValue, true, OverrideID.Unknown, 0, false, (object) 0.0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.LiterPerImpuls, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.PulseMultiplier, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.InputSampleTime, true, OverrideID.Unknown, 0, false, (object) 0U, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.uintMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.InputMode, true, OverrideID.Unknown, 0, false, (object) VolumeInputModes.Impulse_10Hz, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.Enum_VolumeInputModes_MSF(), ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.IO_Functions, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.PulseBlockLimit, true, OverrideID.Unknown, 0, false, (object) (ushort) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UshortMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.PulseLeakLimit, true, OverrideID.Unknown, 0, false, (object) (ushort) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UshortMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.PulseUnleakLimit, true, OverrideID.Unknown, 0, false, (object) (ushort) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UshortMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.PulseLeakLower, true, OverrideID.Unknown, 0, false, (object) (short) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.ShortMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.PulseLeakUpper, true, OverrideID.Unknown, 0, false, (object) (short) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.ShortMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.PulseBackLimit, true, OverrideID.Unknown, 0, false, (object) (ushort) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UshortMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.PulseUnbackLimit, true, OverrideID.Unknown, 0, false, (object) (ushort) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UshortMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.InputResolution, true, OverrideID.Unknown, 0, false, (object) InputUnitsIndex.ImpUnit_0, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.Enum_InputUnit_MSF(), ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.InputPulsValue, true, OverrideID.Unknown, 0, false, (object) 0.0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.LiterPerImpuls, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.InputActualValue, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.InputDueDateValue, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.InputDueDateLastValue, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.InputDeviceType, true, OverrideID.Unknown, 0, false, (object) MBusDeviceType.WATER, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.Enum_MBusDeviceType_MSF(), ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.InputOutputFunction, true, OverrideID.Unknown, 0, false, (object) InputOutputFunctions.BusControlled, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.Enum_InputOutputFunctions_MSF(), ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.InputResolutionStr, true, OverrideID.Unknown, 0, false, (object) "m\u00B3", (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.UniversalUnit_MSF(), ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.PulseoutMode, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.PulseoutWidth, true, OverrideID.Unknown, 0, false, (object) 0.0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.PulseoutResolution, true, OverrideID.Unknown, 0, false, (object) (short) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.ShortMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.TimeZone, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.DeviceClock, true, OverrideID.Unknown, 0, false, (object) DateTime.MinValue, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DateTimeMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.DeviceInformation, 50, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CycleTimeFast, true, OverrideID.Unknown, 0, false, (object) 0UL, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UlongMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CycleTimeStandard, true, OverrideID.Unknown, 0, false, (object) 0UL, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UlongMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CycleTimeDynamic, true, OverrideID.Unknown, 0, false, (object) CycleTimeChangeMethode.OFF, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.Enum_CycleTimeChangeMethode_MSF(), ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.DueDate, true, OverrideID.Unknown, 0, false, (object) DateTime.MinValue, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DateTimeMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.ReadingDate, true, OverrideID.Unknown, 0, false, (object) DateTime.MinValue, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DateTimeMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.DueDateMonth, true, OverrideID.Unknown, 0, false, (object) 1L, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UlongMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.SummerOff, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.WinterStart, true, OverrideID.Unknown, 0, false, (object) DateTime.MinValue, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DateTimeMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.SummerStart, true, OverrideID.Unknown, 0, false, (object) DateTime.MinValue, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DateTimeMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.EndOfBattery, true, OverrideID.Unknown, 0, false, (object) new DateTime(1980, 1, 1), (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DateTimeMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Battery, 1, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true).SpecialDisplayFormat = "d";
      ConfigurationParameter.AddCoPaInfo(OverrideID.EndOfCalibration, true, OverrideID.Unknown, 0, false, (object) DateTime.MinValue, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.YearMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Battery, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.EndOfBatteryDate, true, OverrideID.Unknown, 0, false, (object) DateTime.MinValue, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DateTimeMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Battery, 2, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MaxEndOfBatteryDate, true, OverrideID.Unknown, 0, false, (object) DateTime.MinValue, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DateTimeMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Battery, 3, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true).SpecialDisplayFormat = "d";
      ConfigurationParameter.ConPaInfo conPaInfo = ConfigurationParameter.AddCoPaInfo(OverrideID.BatteryDurabilityMonths, true, OverrideID.Unknown, 0, false, (object) 0U, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.uintMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Battery, 3, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      conPaInfo = ConfigurationParameter.AddCoPaInfo(OverrideID.BatteryPreWarningMonths, true, OverrideID.Unknown, 0, false, (object) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.intMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Battery, 3, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.BatteryCapacity_mAh, true, OverrideID.Unknown, 0, false, (object) 0.0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Battery, 4, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RemainingDiagnosticMessages, true, OverrideID.Unknown, 0, false, (object) 0U, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.uintMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Battery, 5, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.PossibleHourDiagnosticYears, true, OverrideID.Unknown, 0, false, (object) 0.0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Battery, 6, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true).SpecialDisplayFormat = "F2";
      ConfigurationParameter.AddCoPaInfo(OverrideID.WarmerPipe, true, OverrideID.Unknown, 0, false, (object) ConfigurationParameter.WormerPipeValues.FLOW, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.Enum_WormerPipeValues_MSF(), ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.EnergyResolution, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.ChangeOver, true, OverrideID.Unknown, 0, false, (object) ConfigurationParameter.ChangeOverValues.Heating, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.Enum_ChangeOverValues_MSF(), ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.BaseConfig, true, OverrideID.Unknown, 0, false, (object) ConfigurationParameter.BaseConfigSettings.HSrL, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.Enum_MSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CycleTimeVolume, true, OverrideID.Unknown, 0, false, (object) 0UL, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UlongMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.ShowGCAL, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Glycol, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.EnergyActualValue, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Energy, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.EnergyDueDateValue, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Energy, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.EnergyDueDateLastValue, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Energy, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.VolumeActualValue, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Volume, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.VolumeDueDateValue, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Volume, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.VolumeDueDateLastValue, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Volume, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CEnergyActualValue, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.CoolingEnergy, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CEnergyDueDateValue, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.CoolingEnergy, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CEnergyDueDateLastValue, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.CoolingEnergy, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.TotalVolumePulses, true, OverrideID.Unknown, 0, false, (object) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.intMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.TotalTestPulses, true, OverrideID.Unknown, 0, false, (object) 0UL, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UlongMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.TarifFunction, true, OverrideID.Unknown, 0, false, (object) TarifSetup.OFF, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.Enum_TarifSetup_MSF(), ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.TarifRefTemp, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.TemperatureMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.TarifEnergy0, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.TarifEnergy1, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.HeatThresholdTemp, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.TemperatureMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Temperature, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.HCA_Factor_Weighting, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.HCA_SensorMode, true, OverrideID.Unknown, 0, false, (object) HCA_SensorMode.Single, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.Enum_HCA_SensorMode_MSF(), ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.HCA_Scale, true, OverrideID.Unknown, 0, false, (object) HCA_Scale.Uniform, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.Enum_HCA_Scale_MSF(), ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.HCA_Factor_CH, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.HCA_Factor_CHR, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.FixedTempSetup, true, OverrideID.Unknown, 0, false, (object) FixedTempSetup.OFF, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.Enum_FixedTempSetup_MSF(), ValueIdent.ValueIdPart_PhysicalQuantity.Temperature, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.FixedTempValue, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.TemperatureMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Temperature, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MimTempDiffSetup, true, OverrideID.Unknown, 0, false, (object) MinimalTempDiffSetup.OFF, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.Enum_MinimalTempDiffSetup_MSF(), ValueIdent.ValueIdPart_PhysicalQuantity.TempDiff, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MinTempDiffPlusTemp, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.MinTempDiffPlusTemp_MSF(), ValueIdent.ValueIdPart_PhysicalQuantity.TempDiff, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MinTempDiffMinusTemp, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.MinTempDiffMinusTemp_MSF(), ValueIdent.ValueIdPart_PhysicalQuantity.TempDiff, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Baudrate, true, OverrideID.Unknown, 0, false, (object) 0UL, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UlongMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.HCA_ActualValue, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.SleepMode, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.ExitSleep, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.DeviceUnit, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.DaKonSerialNumber, true, OverrideID.Unknown, 0, false, (object) 0UL, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UlongMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.DaKonRegisterNumber, true, OverrideID.Unknown, 0, false, (object) 0UL, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UlongMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.InitDevice, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.StartHKVEReceptionWindow, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RegisterHKVE, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.DeregisterHKVE, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RegisteredHKVE, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.UnregisteredHKVE, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.PulseCorrectionEnabled, true, OverrideID.Unknown, 0, false, (object) true, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.PulseCorrectionValue, true, OverrideID.Unknown, 0, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Coefficient, true, OverrideID.Unknown, 0, false, (object) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.intMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.StartDate, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DateTimeMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.VolMeterFlowPosition, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MBusThirdPartySupport, true, OverrideID.Unknown, 0, false, (object) true, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.ClearNotProtectedValues, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.ClearProtectedValues, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.SetSleepMode, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.SetWriteProtection, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.ResetAllValues, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.ClearAllLoggers, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.SetPcTime, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.SetTimeForTimeZoneFromPcTime, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.SetMbusPrimAdrFromSerialNumber, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.VirtualDeviceOff, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CompactMBusList, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MinolSerialNumber, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RegisterDigits, true, OverrideID.Unknown, 0, false, (object) 0UL, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UlongMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.LongHeader, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Encryption, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.OversizeDiff, true, OverrideID.Unknown, 0, false, (object) (ushort) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UshortMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.OversizeLimit, true, OverrideID.Unknown, 0, false, (object) (ushort) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UshortMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.UndersizeDiff, true, OverrideID.Unknown, 0, false, (object) (ushort) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UshortMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.UndersizeLimit, true, OverrideID.Unknown, 0, false, (object) (ushort) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UshortMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.BurstDiff, true, OverrideID.Unknown, 0, false, (object) (ushort) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UshortMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.BurstLimit, true, OverrideID.Unknown, 0, false, (object) (ushort) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UshortMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.ShowVolumeAsMass, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.TempRangeUpperLimit, true, OverrideID.Unknown, 0, false, (object) 110M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.TemperatureMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Temperature, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.TempRangeLowerLimit, true, OverrideID.Unknown, 0, false, (object) -0.5M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.TemperatureMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Temperature, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.ErrorCode, true, OverrideID.Unknown, 0, false, (object) (ushort) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UshortMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.ErrorNumber, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Standby, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RadioMode, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.ListType, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.ClearWarnings, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.WarningInfo, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.SelectedRadioList, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.AESKey, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.ShowEnergyChecker, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CycleTimeRadio, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.ClearManipulation, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MediumSecondary, true, OverrideID.Unknown, 0, false, (object) MBusDeviceType.UNKNOWN, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.Enum_MBusMediumMSF(), ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.ManufacturerSecondary, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.PulseEnabled, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.TotalPulse, true, OverrideID.Unknown, 0, false, (object) 0U, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.uintMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MaxFlow, true, OverrideID.Unknown, 0, false, (object) 0.0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.StartCalibration, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CalibrationValues, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Calibrated, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.TemperaturRadiator, true, OverrideID.Unknown, 0, false, (object) 0.0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.TemperaturRoom, true, OverrideID.Unknown, 0, false, (object) 0.0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Radio3RussianMode, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.VolMeterFlowPositionByUser, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MeasurementSetup, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.OutletTempSensorInVolumeMeter, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.MeasurementSetup, 0, ConfigurationLevel.Native | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.SetToDelivery, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.DateOfFirstActivation, true, OverrideID.Unknown, 0, false, (object) DateTime.MinValue, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DateTimeMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.CurrentEvents, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.PushButtonError, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.HornDriveLevel, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.RemovingDetection, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.NumberSmokeAlarms, true, OverrideID.Unknown, 0, false, (object) (ushort) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UshortMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.NumberTestAlarms, true, OverrideID.Unknown, 0, false, (object) (ushort) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.UshortMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.ObstructionDetection, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.SurroundingProximity, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.LedFailure, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.StatusOfInterlinkedDevices, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.TotalVolumePulsesNegativ, true, OverrideID.Unknown, 0, false, (object) 0.0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.DeviceMode, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.SendJoinRequest, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.SetOperatingMode, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Activation, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.HCA_Metrology, true, OverrideID.Unknown, 0, true, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.LeakDetectionOn, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.BackflowDetectionOn, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.BurstDetectionOn, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.StandstillDetectionOn, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.UndersizeDetectionOn, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.OversizeDetectionOn, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.SmartFunctions, true, OverrideID.Unknown, 0, false, (object) new string[0], (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.SelectListMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.SmartFunctions, 0, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.ActiveSmartFunctions, true, OverrideID.Unknown, 0, false, (object) new string[0], (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.SelectListMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.SmartFunctions, 0, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.SmartFunctionConfig, true, OverrideID.Unknown, 0, false, (object) new string[0], (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.SelectListMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.SmartFunctions, 0, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.SmartFunctionGroup, true, OverrideID.Unknown, 0, false, (object) new string[0], (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.SelectListMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.SmartFunctions, 0, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.MinimumFlowQ1, false, OverrideID.Unknown, 0, false, (object) 0.0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Flow, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.PermanentFlowQ3, false, OverrideID.Unknown, 0, false, (object) 0.0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Flow, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Input1PulsValue, true, OverrideID.VolumePulsValue, 1, false, (object) 0.0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Input1Unit, true, OverrideID.InputPulsValue, 1, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Input2PulsValue, true, OverrideID.VolumePulsValue, 2, false, (object) 0.0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.doubleMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Input2Unit, true, OverrideID.InputPulsValue, 2, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Output1Function, true, OverrideID.Unknown, 0, false, (object) ConfigurationParameter.OutputFunctions.KEINE, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.Enum_OutputFunctions_MSF(), ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Output2Function, true, OverrideID.Unknown, 0, false, (object) ConfigurationParameter.OutputFunctions.KEINE, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.Enum_OutputFunctions_MSF(), ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Input1ActualValue, true, OverrideID.InputActualValue, 1, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Input1DueDateValue, true, OverrideID.InputDueDateValue, 1, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Input1DueDateLastValue, true, OverrideID.InputDueDateLastValue, 1, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Input2ActualValue, true, OverrideID.InputActualValue, 2, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Input2DueDateValue, true, OverrideID.InputDueDateValue, 2, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Input2DueDateLastValue, true, OverrideID.InputDueDateLastValue, 2, false, (object) 0M, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.DecimalMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Input1IdNumber, true, OverrideID.SerialNumber, 1, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Input2IdNumber, true, OverrideID.SerialNumber, 2, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Input1Type, true, OverrideID.InputDeviceType, 1, false, (object) MBusDeviceType.WATER, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.Enum_MBusDeviceType_MSF(), ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Input2Type, true, OverrideID.InputDeviceType, 2, false, (object) MBusDeviceType.WATER, (ConfigurationParameter.MeterStringFormater) new ConfigurationParameter.Enum_MBusDeviceType_MSF(), ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      conPaInfo = ConfigurationParameter.AddCoPaInfo(OverrideID.MAC_Address, false, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 1, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      conPaInfo = ConfigurationParameter.AddCoPaInfo(OverrideID.CCID, false, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 1, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      conPaInfo = ConfigurationParameter.AddCoPaInfo(OverrideID.IMEI, false, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 1, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      conPaInfo = ConfigurationParameter.AddCoPaInfo(OverrideID.IMSI, false, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 1, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      conPaInfo = ConfigurationParameter.AddCoPaInfo(OverrideID.FirmwareTimestamp, false, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 1, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      conPaInfo = ConfigurationParameter.AddCoPaInfo(OverrideID.LteModemVersion, false, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 1, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      conPaInfo = ConfigurationParameter.AddCoPaInfo(OverrideID.LteModemModel, false, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 1, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      conPaInfo = ConfigurationParameter.AddCoPaInfo(OverrideID.CpuID, false, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 1, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      conPaInfo = ConfigurationParameter.AddCoPaInfo(OverrideID.LteAPN, false, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 1, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, true);
      ConfigurationParameter.AddCoPaInfo(OverrideID.ActivateSmartFunctions, true, OverrideID.Unknown, 0, true, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.SmartFunctionsActivated, true, OverrideID.Unknown, 0, false, (object) false, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.BoolMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.VIF, true, OverrideID.Unknown, 0, false, (object) (byte) 0, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.byteMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
      ConfigurationParameter.AddCoPaInfo(OverrideID.Unknown, true, OverrideID.Unknown, 0, false, (object) string.Empty, (ConfigurationParameter.MeterStringFormater) ConfigurationParameter.StringMSF_fun, ValueIdent.ValueIdPart_PhysicalQuantity.Any, ConfigurationGroup.Default, 0, ConfigurationLevel.Native | ConfigurationLevel.Standard | ConfigurationLevel.Advanced | ConfigurationLevel.Huge, false);
    }

    private static ConfigurationParameter.ConPaInfo AddCoPaInfo(
      OverrideID TheOverrideID,
      bool EnabledAsDefaultInOldLicense,
      OverrideID NewOverrideId,
      int SubdeviceNumber,
      bool IsFunction,
      object DefaultValue,
      ConfigurationParameter.MeterStringFormater StringFormater,
      ValueIdent.ValueIdPart_PhysicalQuantity CorrespondetValieIdent,
      ConfigurationGroup configGroup,
      int configOrder,
      ConfigurationLevel defaultConfigurationLevels,
      bool formatControlled)
    {
      ConfigurationParameter.ConPaInfo conPaInfo = new ConfigurationParameter.ConPaInfo(TheOverrideID, NewOverrideId, SubdeviceNumber, IsFunction, DefaultValue, StringFormater, CorrespondetValieIdent, EnabledAsDefaultInOldLicense, configGroup, configOrder, defaultConfigurationLevels, formatControlled);
      ConfigurationParameter.ConfigParametersByOverrideID.Add(TheOverrideID, conPaInfo);
      return conPaInfo;
    }

    public ConfigurationParameter(OverrideID TheID)
    {
      this.ParameterID = TheID;
      ConfigurationParameter.ConPaInfo conPaInfo = ConfigurationParameter.ConfigParametersByOverrideID[TheID];
      this.ParameterInfo = conPaInfo;
      this.MyStringFormater = conPaInfo.StringFormater;
      this.Format = conPaInfo.SpecialDisplayFormat;
      this.Unit = this.GetUnitStringFromOverrideId(TheID);
      this.IsFunction = conPaInfo.IsFunction;
      this.IsEditable = false;
      this.ParameterValue = conPaInfo.DefaultValue;
    }

    public ConfigurationParameter(OverrideID TheID, object ParameterValue)
      : this(TheID)
    {
      this.ParameterValue = ParameterValue;
    }

    public ConfigurationParameter(OverrideID TheID, string StringValue, bool DbString)
      : this(TheID)
    {
      if (DbString)
        this.SetValueFromStringDb(StringValue);
      else
        this.SetValueFromStringWin(StringValue);
    }

    public ConfigurationParameter(ConfigurationParameter BaseObject)
    {
      this.ParameterID = BaseObject.ParameterID;
      this.ParameterInfo = BaseObject.ParameterInfo;
      this.ParameterValue = this.GetParameterValueClone(BaseObject.ParameterValue);
      this.Format = BaseObject.Format;
      this.HasWritePermission = BaseObject.HasWritePermission;
      this.MinParameterValue = BaseObject.MinParameterValue;
      this.MaxParameterValue = BaseObject.MaxParameterValue;
      this.AllowedValues = BaseObject.AllowedValues;
      this.TrueDivisor = BaseObject.TrueDivisor;
      this.MyStringFormater = BaseObject.MyStringFormater;
      this.SubDevice = BaseObject.SubDevice;
      this.Unit = BaseObject.Unit;
      this.ParameterKey = BaseObject.ParameterKey;
      this.IsEditable = BaseObject.IsEditable;
    }

    public ConfigurationParameter Clone()
    {
      return new ConfigurationParameter(this.ParameterID)
      {
        ParameterValue = this.GetParameterValueClone(this.ParameterValue),
        Format = this.Format,
        HasWritePermission = this.HasWritePermission,
        MinParameterValue = this.MinParameterValue,
        MaxParameterValue = this.MaxParameterValue,
        AllowedValues = this.AllowedValues,
        TrueDivisor = this.TrueDivisor,
        MyStringFormater = this.MyStringFormater,
        SubDevice = this.SubDevice,
        ParameterKey = this.ParameterKey,
        Unit = this.Unit,
        IsEditable = this.IsEditable,
        ParameterInfo = this.ParameterInfo
      };
    }

    public ConfigurationParameter CloneWithNewId(OverrideID NewId)
    {
      return new ConfigurationParameter(NewId)
      {
        ParameterValue = this.GetParameterValueClone(this.ParameterValue),
        Format = this.Format,
        HasWritePermission = this.HasWritePermission,
        MinParameterValue = this.MinParameterValue,
        MaxParameterValue = this.MaxParameterValue,
        AllowedValues = this.AllowedValues,
        TrueDivisor = this.TrueDivisor,
        MyStringFormater = this.MyStringFormater,
        SubDevice = this.SubDevice,
        ParameterKey = this.ParameterKey,
        Unit = this.GetUnitStringFromOverrideId(NewId),
        IsEditable = this.IsEditable
      };
    }

    private object GetParameterValueClone(object parameterValue)
    {
      if (parameterValue == null)
        return (object) null;
      if (!(parameterValue.GetType() == typeof (byte[])))
        return parameterValue;
      byte[] parameterValueClone = new byte[((byte[]) parameterValue).Length];
      ((Array) parameterValue).CopyTo((Array) parameterValueClone, 0);
      return (object) parameterValueClone;
    }

    public string GetUnitStringFromOverrideId(OverrideID theId)
    {
      ValueIdent.ValueIdPart_PhysicalQuantity correspondetValueIdent = ConfigurationParameter.ConfigParametersByOverrideID[theId].CorrespondetValueIdent;
      return correspondetValueIdent != 0 ? ValueIdent.GetUnit(correspondetValueIdent) : string.Empty;
    }

    public static SortedList GetListClone(SortedList ConfigList)
    {
      SortedList listClone = new SortedList();
      for (int index = 0; index < ConfigList.Count; ++index)
      {
        ConfigurationParameter configurationParameter = ((ConfigurationParameter) ConfigList.GetByIndex(index)).Clone();
        listClone.Add((object) configurationParameter.ParameterID, (object) configurationParameter);
      }
      return listClone;
    }

    public bool IsParameterValueEqual(ConfigurationParameter compareParameter)
    {
      if (compareParameter == null || this.ParameterValue == null || compareParameter.ParameterValue == null || this.ParameterValue.GetType() != compareParameter.ParameterValue.GetType())
        return false;
      if (this.ParameterType == typeof (byte[]))
      {
        if (((byte[]) this.ParameterValue).Length != ((byte[]) compareParameter.ParameterValue).Length)
          return false;
        for (int index = 0; index < ((byte[]) this.ParameterValue).Length; ++index)
        {
          if ((int) ((byte[]) this.ParameterValue)[index] != (int) ((byte[]) compareParameter.ParameterValue)[index])
            return false;
        }
        return true;
      }
      return this.ParameterValue == compareParameter.ParameterValue;
    }

    public int CompareTo(ConfigurationParameter compareParameter)
    {
      int num1 = this.IsFunction.CompareTo(compareParameter.IsFunction);
      if (num1 != 0)
        return num1;
      int num2 = this.HasWritePermission.CompareTo(compareParameter.HasWritePermission);
      if (num2 != 0)
        return num2;
      int num3 = this.ParameterInfo.ConfigurationGroup.CompareTo((object) compareParameter.ParameterInfo.ConfigurationGroup);
      if (num3 != 0)
        return num3;
      int num4 = this.ParameterInfo.ConfigurationOrder.CompareTo(compareParameter.ParameterInfo.ConfigurationOrder);
      if (num4 != 0)
        return num4;
      OverrideID parameterId = this.ParameterID;
      string str = parameterId.ToString();
      parameterId = compareParameter.ParameterID;
      string strB = parameterId.ToString();
      return str.CompareTo(strB);
    }

    public static List<ConfigurationParameter> GetOrderdList(
      SortedList<OverrideID, ConfigurationParameter> ConfigList)
    {
      List<ConfigurationParameter> orderdList = new List<ConfigurationParameter>((IEnumerable<ConfigurationParameter>) ConfigList.Values);
      orderdList.Sort();
      return orderdList;
    }

    public static void ChangeOrAddOverrideParameter(
      SortedList TheList,
      ConfigurationParameter TheConfigParameter)
    {
      int index = TheList.IndexOfKey((object) TheConfigParameter.ParameterID);
      if (index < 0)
        TheList.Add((object) TheConfigParameter.ParameterID, (object) TheConfigParameter);
      else
        TheList.SetByIndex(index, (object) TheConfigParameter);
    }

    public static void DeleteConfigParameter(SortedList TheList, OverrideID TheId)
    {
      int index = TheList.IndexOfKey((object) TheId);
      if (index < 0)
        return;
      TheList.RemoveAt(index);
    }

    public class ConPaInfo
    {
      public string SpecialDisplayFormat;
      public bool FormatControlled;
      public OverrideID NewOverrideId;
      public int SubdeviceNumber;
      public bool EnabledAsDefaultInOldLicense;

      public OverrideID OverrideId { get; internal set; }

      public bool IsFunction { get; internal set; }

      public object DefaultValue { get; internal set; }

      public ConfigurationParameter.MeterStringFormater StringFormater { get; internal set; }

      public ValueIdent.ValueIdPart_PhysicalQuantity CorrespondetValueIdent { get; internal set; }

      public ConfigurationGroup ConfigurationGroup { get; internal set; }

      public int ConfigurationOrder { get; internal set; }

      public ConfigurationLevel DefaultConfigurationLevels { get; internal set; }

      public ConPaInfo(
        OverrideID OverrideId,
        OverrideID NewOverrideId,
        int SubdeviceNumber,
        bool IsFunction,
        object DefaultValue,
        ConfigurationParameter.MeterStringFormater StringFormater,
        ValueIdent.ValueIdPart_PhysicalQuantity CorrespondetValueIdent,
        bool EnabledAsDefaultInOldLicense,
        ConfigurationGroup ConfigurationGroup,
        int ConfigurationOrder,
        ConfigurationLevel defaultConfigurationLevels,
        bool FormatControlled)
      {
        this.OverrideId = OverrideId;
        this.NewOverrideId = NewOverrideId;
        this.SubdeviceNumber = SubdeviceNumber;
        this.IsFunction = IsFunction;
        this.DefaultValue = DefaultValue;
        this.StringFormater = StringFormater;
        this.CorrespondetValueIdent = CorrespondetValueIdent;
        this.EnabledAsDefaultInOldLicense = EnabledAsDefaultInOldLicense;
        this.ConfigurationGroup = ConfigurationGroup;
        this.ConfigurationOrder = ConfigurationOrder;
        this.DefaultConfigurationLevels = defaultConfigurationLevels;
        this.FormatControlled = FormatControlled;
      }
    }

    [Flags]
    public enum ValueType
    {
      Ident = 0,
      Direct = 1,
      Complete = 2,
      Factory = Complete | Direct, // 0x00000003
    }

    public enum OutputFunctions
    {
      KEINE,
      ENERGIE,
      VOLUMEN,
      KÄLTE,
    }

    public enum WormerPipeValues
    {
      RETURN,
      FLOW,
    }

    public enum ChangeOverValues
    {
      Heating,
      ChangeOver,
      Cooling,
      None,
    }

    public enum BaseConfigSettings
    {
      HSrL,
      HSrH,
      HdrL,
      HEnL,
      CSrL,
      CSrH,
      CdrL,
      CEnL,
      OSrL,
      OSrH,
      OdrL,
      OEnL,
      FSrL,
      FSrH,
      FdrL,
      FEnL,
    }

    public abstract class MeterStringFormater
    {
      public abstract string GetStringValueDb(ConfigurationParameter TheParameter);

      public abstract string GetStringValueWin(ConfigurationParameter TheParameter);

      public abstract void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter);

      public abstract void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter);
    }

    private class StringMSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return Util.ToString(TheParameter.ParameterValue);
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return Util.ToString(TheParameter.ParameterValue);
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) StringValue;
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) StringValue;
      }
    }

    private class BoolMSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return TheParameter.ParameterValue.ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return TheParameter.ParameterValue.ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) bool.Parse(StringValue);
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) bool.Parse(StringValue);
      }
    }

    private class UlongMSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return TheParameter.ParameterValue.ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return TheParameter.ParameterValue.ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) ulong.Parse(StringValue);
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) ulong.Parse(StringValue);
      }
    }

    private class IntMSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return TheParameter.ParameterValue.ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return TheParameter.ParameterValue.ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) int.Parse(StringValue);
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) int.Parse(StringValue);
      }
    }

    private class UIntMSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return TheParameter.ParameterValue.ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return TheParameter.ParameterValue.ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) uint.Parse(StringValue);
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) uint.Parse(StringValue);
      }
    }

    private class ByteMSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return TheParameter.ParameterValue.ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return TheParameter.ParameterValue.ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) byte.Parse(StringValue);
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) byte.Parse(StringValue);
      }
    }

    private class UShortMSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return this.GetStringValueWin(TheParameter);
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return !string.IsNullOrEmpty(TheParameter.Format) ? ((ushort) TheParameter.ParameterValue).ToString(TheParameter.Format) : TheParameter.ParameterValue.ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) ushort.Parse(StringValue);
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) ushort.Parse(StringValue);
      }
    }

    private class ShortMSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return TheParameter.ParameterValue.ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return TheParameter.ParameterValue.ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) short.Parse(StringValue);
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) short.Parse(StringValue);
      }
    }

    private class doubleMSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return ((double) TheParameter.ParameterValue).ToString((IFormatProvider) FixedFormates.TheFormates.NumberFormat);
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        if (TheParameter.ParameterValue == null)
          return string.Empty;
        return string.IsNullOrEmpty(TheParameter.Format) ? Convert.ToDouble(TheParameter.ParameterValue).ToString() : Convert.ToDouble(TheParameter.ParameterValue).ToString(TheParameter.Format);
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) double.Parse(StringValue, (IFormatProvider) FixedFormates.TheFormates.NumberFormat);
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) double.Parse(StringValue);
      }
    }

    private class DecimalMSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return ((Decimal) TheParameter.ParameterValue).ToString((IFormatProvider) FixedFormates.TheFormates.NumberFormat);
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return string.IsNullOrEmpty(TheParameter.Format) ? ((Decimal) TheParameter.ParameterValue).ToString() : ((Decimal) TheParameter.ParameterValue).ToString(TheParameter.Format);
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        if (string.IsNullOrEmpty(StringValue))
          return;
        TheParameter.ParameterValue = (object) Decimal.Parse(StringValue, (IFormatProvider) FixedFormates.TheFormates.NumberFormat);
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        if (string.IsNullOrEmpty(StringValue))
          return;
        TheParameter.ParameterValue = (object) Decimal.Parse(StringValue);
      }
    }

    private class FloatMSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return ((float) TheParameter.ParameterValue).ToString((IFormatProvider) FixedFormates.TheFormates.NumberFormat);
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return string.IsNullOrEmpty(TheParameter.Format) ? ((float) TheParameter.ParameterValue).ToString() : ((float) TheParameter.ParameterValue).ToString(TheParameter.Format);
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) float.Parse(StringValue, (IFormatProvider) FixedFormates.TheFormates.NumberFormat);
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) float.Parse(StringValue);
      }
    }

    private class UintHexKey8 : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return !(TheParameter.ParameterValue.GetType() != typeof (uint)) ? ((uint) TheParameter.ParameterValue).ToString("x08") : throw new ArgumentException("Illegal data type for ParameterValue");
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return this.GetStringValueDb(TheParameter);
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        if (string.IsNullOrEmpty(StringValue))
          throw new ArgumentException("Value not defined. Exact 8 hex digits required");
        if (StringValue.Trim().Length != 8)
          throw new ArgumentException("Illegal number of characters. Exact 8 hex digits required");
        uint result;
        if (!uint.TryParse(StringValue, NumberStyles.HexNumber, (IFormatProvider) null, out result))
          throw new ArgumentException("Illegal hex value");
        TheParameter.ParameterValue = (object) result;
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class UlongHexKey16 : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        if (TheParameter.ParameterValue == null)
          return string.Empty;
        return !(TheParameter.ParameterValue.GetType() != typeof (ulong)) ? ((ulong) TheParameter.ParameterValue).ToString("x016") : throw new ArgumentException("Illegal data type for ParameterValue");
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return this.GetStringValueDb(TheParameter);
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        if (string.IsNullOrEmpty(StringValue))
          throw new ArgumentException("Value not defined. Exact 16 hex digits required");
        if (StringValue.Trim().Length != 16)
          throw new ArgumentException("Illegal number of characters. Exact 16 hex digits required");
        ulong result;
        if (!ulong.TryParse(StringValue, NumberStyles.HexNumber, (IFormatProvider) null, out result))
          throw new ArgumentException("Illegal hex value");
        TheParameter.ParameterValue = (object) result;
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class ByteArrayHexKey32 : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        if (TheParameter.ParameterValue == null)
          return string.Empty;
        return !(TheParameter.ParameterValue.GetType() != typeof (byte[])) ? Util.ByteArrayToHexString((byte[]) TheParameter.ParameterValue) : throw new ArgumentException("Illegal data type for ParameterValue");
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return this.GetStringValueDb(TheParameter);
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        string hex = !string.IsNullOrEmpty(StringValue) ? StringValue.Trim() : throw new ArgumentException("Value not defined. Exact 32 hex digits required");
        byte[] numArray = hex.Length == 32 ? Util.HexStringToByteArray(hex) : throw new ArgumentException("Illegal number of characters. Exact 32 hex digits required");
        TheParameter.ParameterValue = (object) numArray;
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class DateTimeMSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return ((DateTime) TheParameter.ParameterValue).ToString((IFormatProvider) FixedFormates.TheFormates.DateTimeFormat);
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        if (TheParameter.ParameterValue == null)
          return string.Empty;
        return string.IsNullOrEmpty(TheParameter.Format) ? ((DateTime) TheParameter.ParameterValue).ToString() : ((DateTime) TheParameter.ParameterValue).ToString(TheParameter.Format);
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) DateTime.Parse(StringValue, (IFormatProvider) FixedFormates.TheFormates.DateTimeFormat);
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        if (string.IsNullOrEmpty(TheParameter.Format))
        {
          TheParameter.ParameterValue = (object) DateTime.Parse(StringValue);
        }
        else
        {
          try
          {
            TheParameter.ParameterValue = (object) DateTime.ParseExact(StringValue, TheParameter.Format, (IFormatProvider) null);
          }
          catch
          {
            TheParameter.ParameterValue = (object) DateTime.Parse(StringValue);
          }
        }
      }
    }

    private class YearMSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return TheParameter.ParameterValue.ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return TheParameter.ParameterValue.ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) this.YearValue(StringValue);
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        TheParameter.ParameterValue = (object) this.YearValue(StringValue);
      }

      private int YearValue(string StringValue)
      {
        int num = int.Parse(StringValue);
        if (num > 0 && num < 20)
          return num + DateTime.Now.Year;
        return num >= 1980 && num < 2050 ? num : throw new ArgumentException("End of date");
      }
    }

    private class MinolMeterDeviceListMSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter) => string.Empty;

      public override string GetStringValueWin(ConfigurationParameter TheParameter) => string.Empty;

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
      }
    }

    private class SelectListMSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (string str in (string[]) TheParameter._parameterValue)
        {
          if (stringBuilder.Length > 0)
            stringBuilder.Append(';');
          stringBuilder.Append(str);
        }
        return stringBuilder.ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return this.GetStringValueDb(TheParameter);
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        string[] strArray = StringValue.Split(new char[1]
        {
          ';'
        }, StringSplitOptions.RemoveEmptyEntries);
        ConfigurationParameter.ConfigParametersByOverrideID[TheParameter.ParameterID].DefaultValue.GetType();
        TheParameter.ParameterValue = (object) strArray;
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class Enum_MSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return TheParameter.ParameterValue.ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return TheParameter.ParameterValue.ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        Type type = ConfigurationParameter.ConfigParametersByOverrideID[TheParameter.ParameterID].DefaultValue.GetType();
        TheParameter.ParameterValue = Enum.Parse(type, StringValue, true);
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class Enum_WormerPipeValues_MSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return (bool) TheParameter.ParameterValue ? ConfigurationParameter.WormerPipeValues.FLOW.ToString() : ConfigurationParameter.WormerPipeValues.RETURN.ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return this.GetStringValueDb(TheParameter);
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        if (StringValue == ConfigurationParameter.WormerPipeValues.RETURN.ToString())
        {
          TheParameter.ParameterValue = (object) false;
        }
        else
        {
          if (!(StringValue == ConfigurationParameter.WormerPipeValues.FLOW.ToString()))
            throw new ArgumentException("Illegal WarmerPipe value");
          TheParameter.ParameterValue = (object) true;
        }
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class Enum_ChangeOverValues_MSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return ((ConfigurationParameter.ChangeOverValues) TheParameter.ParameterValue).ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return this.GetStringValueDb(TheParameter);
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          TheParameter.ParameterValue = (object) (ConfigurationParameter.ChangeOverValues) Enum.Parse(typeof (ConfigurationParameter.ChangeOverValues), StringValue, true);
        }
        catch
        {
          ZR_ClassLibMessages.AddErrorDescriptionAndException(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal ChangeOver value");
        }
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class Enum_OutputFunctions_MSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return ((ConfigurationParameter.OutputFunctions) TheParameter.ParameterValue).ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return ((ConfigurationParameter.OutputFunctions) TheParameter.ParameterValue).ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          TheParameter.ParameterValue = (object) (ConfigurationParameter.OutputFunctions) Enum.Parse(typeof (ConfigurationParameter.OutputFunctions), StringValue, true);
        }
        catch
        {
          throw new ArgumentException("Illegal output function");
        }
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class Enum_MinimalTempDiffSetup_MSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return ((MinimalTempDiffSetup) TheParameter.ParameterValue).ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return ((MinimalTempDiffSetup) TheParameter.ParameterValue).ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          TheParameter.ParameterValue = (object) (MinimalTempDiffSetup) Enum.Parse(typeof (MinimalTempDiffSetup), StringValue, false);
        }
        catch
        {
          throw new ArgumentException("Illegal MinimalTempDiffSetup parameter");
        }
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class Enum_FixedTempSetup_MSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return ((FixedTempSetup) TheParameter.ParameterValue).ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return ((FixedTempSetup) TheParameter.ParameterValue).ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          TheParameter.ParameterValue = (object) (FixedTempSetup) Enum.Parse(typeof (FixedTempSetup), StringValue, false);
        }
        catch
        {
          throw new ArgumentException("Illegal FixedTempSetup parameter");
        }
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class Enum_CycleTimeChangeMethode_MSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return ((CycleTimeChangeMethode) TheParameter.ParameterValue).ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return ((CycleTimeChangeMethode) TheParameter.ParameterValue).ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          TheParameter.ParameterValue = (object) (CycleTimeChangeMethode) Enum.Parse(typeof (CycleTimeChangeMethode), StringValue, false);
        }
        catch
        {
          throw new ArgumentException("Illegal CycleTimeChangeMethode parameter");
        }
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class Enum_TarifSetup_MSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return ((TarifSetup) TheParameter.ParameterValue).ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return ((TarifSetup) TheParameter.ParameterValue).ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          TheParameter.ParameterValue = (object) (TarifSetup) Enum.Parse(typeof (TarifSetup), StringValue, false);
        }
        catch
        {
          throw new ArgumentException("Illegal TarifSetup parameter");
        }
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class Enum_HCA_SensorMode_MSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return ((HCA_SensorMode) TheParameter.ParameterValue).ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return ((HCA_SensorMode) TheParameter.ParameterValue).ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          TheParameter.ParameterValue = (object) (HCA_SensorMode) Enum.Parse(typeof (HCA_SensorMode), StringValue, false);
        }
        catch
        {
          throw new ArgumentException("Illegal HCA_SensorMode parameter");
        }
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class Enum_MBusMediumMSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return this.GetStringValueWin(TheParameter);
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        if (TheParameter == null)
          throw new ArgumentNullException("ConfigurationParameter MBusMedium not defined");
        return (TheParameter.ParameterValue != null && Enum.IsDefined(typeof (MBusDeviceType), TheParameter.ParameterValue) ? (MBusDeviceType) Enum.Parse(typeof (MBusDeviceType), TheParameter.ParameterValue.ToString(), true) : (MBusDeviceType) ConfigurationParameter.ConfigParametersByOverrideID[TheParameter.ParameterID].DefaultValue).ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          TheParameter.ParameterValue = (object) (MBusDeviceType) Enum.Parse(typeof (MBusDeviceType), StringValue, false);
        }
        catch
        {
          throw new ArgumentException("Illegal MBusDeviceType parameter");
        }
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class Enum_HCA_Scale_MSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return ((HCA_Scale) TheParameter.ParameterValue).ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return ((HCA_Scale) TheParameter.ParameterValue).ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          TheParameter.ParameterValue = (object) (HCA_Scale) Enum.Parse(typeof (HCA_Scale), StringValue, false);
        }
        catch
        {
          throw new ArgumentException("Illegal HCA_Scale parameter");
        }
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class Enum_InputUnit_MSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return ((InputUnitsIndex) TheParameter.ParameterValue).ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return ((InputUnitsIndex) TheParameter.ParameterValue).ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          TheParameter.ParameterValue = (object) (InputUnitsIndex) Enum.Parse(typeof (InputUnitsIndex), StringValue, false);
        }
        catch
        {
          throw new ArgumentException("Illegal input unit parameter");
        }
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class Enum_RadioMode_MSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return ((RadioProtocol) TheParameter.ParameterValue).ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return ((RadioProtocol) TheParameter.ParameterValue).ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          TheParameter.ParameterValue = (object) (RadioProtocol) Enum.Parse(typeof (RadioProtocol), StringValue, false);
        }
        catch
        {
          throw new ArgumentException("Illegal RadioMode parameter");
        }
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class Enum_Region_MSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return ((Region) TheParameter.ParameterValue).ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return ((Region) TheParameter.ParameterValue).ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          TheParameter.ParameterValue = (object) (Region) Enum.Parse(typeof (Region), StringValue, false);
        }
        catch
        {
          throw new ArgumentException("Illegal Region parameter");
        }
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class Enum_InputOutputFunctions_MSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return this.GetStringValueWin(TheParameter);
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return TheParameter == null || TheParameter.ParameterValue == null || !Enum.IsDefined(typeof (InputOutputFunctions), TheParameter.ParameterValue) ? (string) null : Enum.Parse(typeof (InputOutputFunctions), TheParameter.ParameterValue.ToString(), true).ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          TheParameter.ParameterValue = (object) (InputOutputFunctions) Enum.Parse(typeof (InputOutputFunctions), StringValue, false);
        }
        catch (Exception ex)
        {
          ZR_ClassLibMessages.AddErrorDescription("Exception: " + ex.ToString(), ConfigurationParameter.ConfigParamLogger);
          ZR_ClassLibMessages.AddErrorDescriptionAndException(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal InputOutputFunctions parameter", ConfigurationParameter.ConfigParamLogger);
        }
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class UniversalUnit_MSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return (string) TheParameter.ParameterValue;
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return (string) TheParameter.ParameterValue;
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          if (TheParameter.AllowedValues != null)
          {
            int index = 0;
            while (index < TheParameter.AllowedValues.Length && !(TheParameter.AllowedValues[index] == StringValue))
              ++index;
            if (index >= TheParameter.AllowedValues.Length)
              ZR_ClassLibMessages.AddErrorDescriptionAndException(ZR_ClassLibMessages.LastErrors.IllegalData, "Parameter unit not available", ConfigurationParameter.ConfigParamLogger);
          }
          TheParameter.ParameterValue = (object) StringValue;
        }
        catch (Exception ex)
        {
          ZR_ClassLibMessages.AddErrorDescription("Exception: " + ex.ToString(), ConfigurationParameter.ConfigParamLogger);
          ZR_ClassLibMessages.AddErrorDescriptionAndException(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal unit parameter", ConfigurationParameter.ConfigParamLogger);
        }
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class Enum_MBusDeviceType_MSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return ((MBusDeviceType) (ulong) TheParameter.ParameterValue).ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return ((MBusDeviceType) (ulong) TheParameter.ParameterValue).ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          TheParameter.ParameterValue = (object) (ulong) (MBusDeviceType) Enum.Parse(typeof (MBusDeviceType), StringValue, false);
        }
        catch
        {
          throw new ArgumentException("Illegal MBusDeviceType parameter");
        }
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class Enum_RadioFrequence_MSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return ((RadioFrequence) TheParameter.ParameterValue).ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return ((RadioFrequence) TheParameter.ParameterValue).ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          TheParameter.ParameterValue = (object) (RadioFrequence) Enum.Parse(typeof (RadioFrequence), StringValue, false);
        }
        catch
        {
          throw new ArgumentException("Illegal RadioFrequence parameter");
        }
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class Enum_VolumeInputModes_MSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return ((VolumeInputModes) TheParameter.ParameterValue).ToString();
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return ((VolumeInputModes) TheParameter.ParameterValue).ToString();
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          TheParameter.ParameterValue = (object) (VolumeInputModes) Enum.Parse(typeof (VolumeInputModes), StringValue, false);
        }
        catch
        {
          throw new ArgumentException("Illegal VolumeInputModes parameter");
        }
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        this.SetValueFromStringDb(StringValue, TheParameter);
      }
    }

    private class TemperatureMSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return ((Decimal) TheParameter.ParameterValue).ToString("0.00", (IFormatProvider) FixedFormates.TheFormates.NumberFormat);
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return ((Decimal) TheParameter.ParameterValue).ToString("0.00");
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          Decimal num = Decimal.Parse(StringValue, (IFormatProvider) FixedFormates.TheFormates.NumberFormat);
          TheParameter.ParameterValue = !(num > 300M) && !(num < -30M) ? (object) num : throw new ArgumentException("Illegal temperature");
        }
        catch
        {
          throw new ArgumentException("Illegal temperature");
        }
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          Decimal num = Decimal.Parse(StringValue);
          TheParameter.ParameterValue = !(num > 300M) && !(num < -30M) ? (object) num : throw new ArgumentException("Illegal temperature");
        }
        catch
        {
          throw new ArgumentException("Illegal temperature");
        }
      }
    }

    private class MinTempDiffMinusTemp_MSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return ((Decimal) TheParameter.ParameterValue).ToString("0.00", (IFormatProvider) FixedFormates.TheFormates.NumberFormat);
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return ((Decimal) TheParameter.ParameterValue).ToString("0.00");
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          Decimal num = Decimal.Parse(StringValue, (IFormatProvider) FixedFormates.TheFormates.NumberFormat);
          if (num > 0M)
            num *= -1M;
          TheParameter.ParameterValue = !(num < -300M) ? (object) num : throw new ArgumentException("Illegal temperature");
        }
        catch
        {
          throw new ArgumentException("Illegal temperature");
        }
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          Decimal num = Decimal.Parse(StringValue);
          if (num > 0M)
            num *= -1M;
          TheParameter.ParameterValue = !(num < -300M) ? (object) num : throw new ArgumentException("Illegal temperature");
        }
        catch
        {
          throw new ArgumentException("Illegal temperature");
        }
      }
    }

    private class MinTempDiffPlusTemp_MSF : ConfigurationParameter.MeterStringFormater
    {
      public override string GetStringValueDb(ConfigurationParameter TheParameter)
      {
        return ((Decimal) TheParameter.ParameterValue).ToString("0.00", (IFormatProvider) FixedFormates.TheFormates.NumberFormat);
      }

      public override string GetStringValueWin(ConfigurationParameter TheParameter)
      {
        return ((Decimal) TheParameter.ParameterValue).ToString("0.00");
      }

      public override void SetValueFromStringDb(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          Decimal num = Decimal.Parse(StringValue, (IFormatProvider) FixedFormates.TheFormates.NumberFormat);
          if (num < 0M)
            num *= -1M;
          TheParameter.ParameterValue = !(num > 300M) ? (object) num : throw new ArgumentException("Illegal temperature");
        }
        catch
        {
          throw new ArgumentException("Illegal temperature");
        }
      }

      public override void SetValueFromStringWin(
        string StringValue,
        ConfigurationParameter TheParameter)
      {
        try
        {
          Decimal num = Decimal.Parse(StringValue);
          if (num < 0M)
            num *= -1M;
          TheParameter.ParameterValue = !(num > 300M) ? (object) num : throw new ArgumentException("Illegal temperature");
        }
        catch
        {
          throw new ArgumentException("Illegal temperature");
        }
      }
    }
  }
}

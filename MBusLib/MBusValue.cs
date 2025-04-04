// Decompiled with JetBrains decompiler
// Type: MBusLib.MBusValue
// Assembly: MBusLib, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4AF58B7C-ADEB-4130-ADB4-1CAE79AA8266
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MBusLib.dll

using MBusLib.Utility;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace MBusLib
{
  public sealed class MBusValue
  {
    private const byte n = 1;
    private const byte nn = 3;
    private const byte nnn = 7;
    private const byte nnnn = 15;
    private VariableDataBlock vdb;

    public string Description { get; private set; }

    public object Value { get; private set; }

    public string Unit { get; private set; }

    public long StorageNumber { get; private set; }

    public int SubUnit { get; private set; }

    public long Tariff { get; private set; }

    public string Zdf { get; private set; }

    public string ZdfValue { get; private set; }

    private MBusValue()
    {
    }

    public MBusValue(VariableDataBlock vdb)
    {
      this.vdb = vdb != null ? vdb : throw new ArgumentNullException(nameof (vdb));
      this.SetTariff();
      this.SetSubUnit();
      this.SetStorageNumber();
      this.SetValue();
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.Description).Append(' ');
      stringBuilder.Append(this.Value).Append(' ');
      stringBuilder.Append(this.Unit).Append(' ');
      if (this.vdb.DIF.FunctionField != 0)
        stringBuilder.Append((object) this.vdb.DIF.FunctionField).Append(' ');
      if (this.SubUnit > 0)
        stringBuilder.Append("SubUnit:").Append(this.SubUnit).Append(' ');
      if (this.StorageNumber > 0L)
        stringBuilder.Append("Storage:").Append(this.StorageNumber).Append(' ');
      if (this.Tariff > 0L)
        stringBuilder.Append("Tariff:").Append(this.Tariff).Append(' ');
      stringBuilder.Append("ZDF:").Append(this.Zdf).Append(' ');
      stringBuilder.Append("ZDF Value:").Append(this.ZdfValue).Append(' ');
      return stringBuilder.ToString().TrimEnd();
    }

    private void SetValue(string description)
    {
      this.SetValue(description.Replace(' ', '_'), 1.0, description, string.Empty, 0, 0, 0);
    }

    private void SetValue(string zdf, double zdfMultiplicator, string description, string unit = "")
    {
      this.SetValue(zdf, zdfMultiplicator, description, unit, 0, 0, 0);
    }

    private void SetValue(
      string zdf,
      double zdfMultiplicator,
      string description,
      string unit,
      int nMask)
    {
      this.SetValue(zdf, zdfMultiplicator, description, unit, nMask, 0, 0);
    }

    private void SetValue(
      string zdf,
      double zdfMultiplicator,
      string description,
      string unit,
      int nMask,
      int exp,
      int decimals = 3)
    {
      if (nMask > 0)
      {
        double multiplier = Math.Pow(10.0, (double) (((int) this.vdb.VIF.Value & nMask) + exp));
        object numericValue = this.GetNumericValue(this.vdb);
        this.ScaleValue(zdfMultiplicator, decimals, multiplier, numericValue);
      }
      else
      {
        this.Value = this.GetNumericValue(this.vdb);
        if (this.Value != null)
          this.ZdfValue = this.Value.ToString();
      }
      this.Description = description;
      this.Unit = unit;
      this.Zdf = zdf;
    }

    private void ScaleValue(
      double zdfMultiplicator,
      int decimals,
      double multiplier,
      object rawValue)
    {
      if (rawValue == null)
        return;
      if (this.vdb.DIF.DataField == DataField.Real32bit)
      {
        float d = Convert.ToSingle(rawValue) * (float) multiplier;
        if (decimals == 0)
        {
          double num = Math.Truncate((double) d);
          this.Value = (object) num;
          this.ZdfValue = (num * zdfMultiplicator).ToString();
        }
        else
        {
          double num = Math.Round((double) d, decimals);
          this.Value = (object) num;
          this.ZdfValue = (num * zdfMultiplicator).ToString();
        }
      }
      else
      {
        long int64 = Convert.ToInt64(rawValue);
        if (decimals == 0)
        {
          double num = Convert.ToDouble((double) int64 * multiplier);
          this.Value = (object) num;
          this.ZdfValue = (num * zdfMultiplicator).ToString();
        }
        else
        {
          double num = Math.Round((double) int64 * multiplier, decimals);
          this.Value = (object) num;
          this.ZdfValue = (num * zdfMultiplicator).ToString();
        }
      }
    }

    private void SetValue()
    {
      if (this.vdb.DIF.Value == (byte) 15 && this.vdb.VIF == null)
      {
        this.Value = (object) this.vdb.Data;
        this.Description = "Manufacture specific data";
        this.Zdf = "ManSpec";
        if (this.vdb.Data == null)
          return;
        this.ZdfValue = Util.ByteArrayToHexString((IEnumerable<byte>) this.vdb.Data);
      }
      else
      {
        switch (this.vdb.VIF.Value)
        {
          case 239:
            this.SetValue("Reserved");
            break;
          case 251:
            this.SetValueVIF_FB(this.vdb);
            break;
          case 253:
            this.SetValueVIF_FD(this.vdb);
            break;
          default:
            byte unitAndMultiplier = this.vdb.VIF.UnitAndMultiplier;
            switch (unitAndMultiplier)
            {
              case 0:
              case 1:
              case 2:
              case 3:
              case 4:
              case 5:
              case 6:
              case 7:
                this.SetValue("MWh", 1E-06, "Energy", "Wh", 7, -3);
                break;
              case 8:
              case 9:
              case 10:
              case 11:
              case 12:
              case 13:
              case 14:
              case 15:
                this.SetValue("GJ", 1E-09, "Energy", "J", 7);
                break;
              case 16:
              case 17:
              case 18:
              case 19:
              case 20:
              case 21:
              case 22:
              case 23:
                this.SetValue("QM", 1.0, "Volume", "m^3", 7, -6);
                break;
              case 24:
              case 25:
              case 26:
              case 27:
              case 28:
              case 29:
              case 30:
              case 31:
                this.SetValue("Kg", 1.0, "Mass", "kg", 7, -3);
                break;
              case 32:
              case 33:
              case 34:
              case 35:
              case 36:
              case 37:
              case 38:
              case 39:
              case 112:
              case 113:
              case 114:
              case 115:
              case 116:
              case 117:
              case 118:
              case 119:
                string empty1 = string.Empty;
                string empty2 = string.Empty;
                string description;
                string str;
                if (((int) unitAndMultiplier & 124) == 32)
                {
                  description = "On time";
                  str = "On";
                }
                else if (((int) unitAndMultiplier & 124) == 36)
                {
                  description = "Operating time";
                  str = "Op";
                }
                else if (((int) unitAndMultiplier & 124) == 112)
                {
                  description = "Averaging Duration";
                  str = "AvD";
                }
                else
                {
                  description = "Actuality Duration";
                  str = "AcD";
                }
                switch ((int) unitAndMultiplier & 3)
                {
                  case 0:
                    this.SetValue(str + "Secs", 1.0, description, "seconds", 3);
                    break;
                  case 1:
                    this.SetValue(str + "Mins", 1.0, description, "minutes", 3);
                    break;
                  case 2:
                    this.SetValue(str + "Hours", 1.0, description, "hours", 3);
                    break;
                  case 3:
                    this.SetValue(str + "Days", 1.0, description, "days", 3);
                    break;
                }
                break;
              case 40:
              case 41:
              case 42:
              case 43:
              case 44:
              case 45:
              case 46:
              case 47:
                this.SetValue("kW", 0.001, "Power", "W", 7, -3);
                break;
              case 48:
              case 49:
              case 50:
              case 51:
              case 52:
              case 53:
              case 54:
              case 55:
                this.SetValue("GJ/h", 1E-09, "Power", "J/h", 7);
                break;
              case 56:
              case 57:
              case 58:
              case 59:
              case 60:
              case 61:
              case 62:
              case 63:
                this.SetValue("QMPH", 1.0, "Volume flow", "m^3/h", 7, -6);
                break;
              case 64:
              case 65:
              case 66:
              case 67:
              case 68:
              case 69:
              case 70:
              case 71:
                this.SetValue("QMPM", 1.0, "Volume flow", "m^3/min", 7, -7, 4);
                break;
              case 72:
              case 73:
              case 74:
              case 75:
              case 76:
              case 77:
              case 78:
              case 79:
                this.SetValue("QMPS", 1.0, "Volume flow", "m^3/s", 7, -9);
                break;
              case 80:
              case 81:
              case 82:
              case 83:
              case 84:
              case 85:
              case 86:
              case 87:
                this.SetValue("KGPH", 1.0, "Mass flow", "kg/h", 7, -3);
                break;
              case 88:
              case 89:
              case 90:
              case 91:
                this.SetValue("TF", 1.0, "Flow temperature", "°C", 3, -3);
                break;
              case 92:
              case 93:
              case 94:
              case 95:
                this.SetValue("TR", 1.0, "Return temperature", "°C", 3, -3);
                break;
              case 96:
              case 97:
              case 98:
              case 99:
                this.SetValue("TD", 1.0, "Temperature Difference", "K", 3, -3);
                break;
              case 100:
              case 101:
              case 102:
              case 103:
                this.SetValue("TX", 1.0, "External temperature", "°C", 3, -3);
                break;
              case 104:
              case 105:
              case 106:
              case 107:
                this.SetValue("HePas", 1.0, "Pressure", "bar", 3, -3, 0);
                break;
              case 108:
              case 109:
                this.Description = "Time Point";
                this.Zdf = "RTIME";
                if (((int) unitAndMultiplier & 1) == 1)
                {
                  DateTime? timeMbusCp32TypeF = MBusUtil.ConvertToDateTime_MBus_CP32_TypeF(this.vdb.Data);
                  this.Unit = "time & date";
                  this.Value = (object) timeMbusCp32TypeF;
                  if (timeMbusCp32TypeF.HasValue)
                  {
                    this.ZdfValue = timeMbusCp32TypeF.Value.ToString("dd.MM.yyyy HH:mm:ss");
                    break;
                  }
                  break;
                }
                DateTime? dateMbusCp16TypeG = MBusUtil.ConvertToDate_MBus_CP16_TypeG(this.vdb.Data);
                this.Unit = "date";
                this.Value = (object) dateMbusCp16TypeG;
                if (dateMbusCp16TypeG.HasValue)
                  this.ZdfValue = dateMbusCp16TypeG.Value.ToString("dd.MM.yyyy");
                break;
              case 110:
                this.SetValue("Units for H.C.A.");
                break;
              case 111:
                this.SetValue("Reserved");
                break;
              case 120:
                this.SetValue("FAB", 1.0, "Fabrication number");
                break;
              case 121:
                this.SetValue("CID", 1.0, "(Enhanced) Identification");
                break;
              case 122:
                this.SetValue("ADR", 1.0, "Bus Address");
                break;
              case 124:
                this.SetValue(this.vdb.VIF_0xFC);
                break;
              case 127:
                this.SetValue("Manuf", 1.0, "Manufacturer specific");
                break;
              default:
                this.Description = "Unknown VIF";
                break;
            }
            this.SetOrthogonal();
            this.FinishZDF();
            break;
        }
      }
    }

    private object GetNumericValue(VariableDataBlock vdb)
    {
      if (vdb.Data == null)
        return (object) null;
      switch (vdb.DIF.DataField)
      {
        case DataField.NoData:
        case DataField.SelectionForReadout:
        case DataField.VariableLength:
          return (object) null;
        case DataField.Integer8bit:
          return (object) vdb.Data[0];
        case DataField.Integer16bit:
          return MBusUtil.IsIntegerValid(vdb.Data) ? (object) BitConverter.ToInt16(vdb.Data, 0) : (object) null;
        case DataField.Integer24bit:
          return MBusUtil.IsIntegerValid(vdb.Data) ? (object) ((int) vdb.Data[0] + ((int) vdb.Data[1] << 8) + ((int) vdb.Data[2] << 16)) : (object) null;
        case DataField.Integer32bit:
          return MBusUtil.IsIntegerValid(vdb.Data) ? (object) BitConverter.ToInt32(vdb.Data, 0) : (object) null;
        case DataField.Real32bit:
          return (object) BitConverter.ToSingle(vdb.Data, 0);
        case DataField.Integer48bit:
          return MBusUtil.IsIntegerValid(vdb.Data) ? (object) ((int) vdb.Data[0] + ((int) vdb.Data[1] << 8) + ((int) vdb.Data[2] << 16) + ((int) vdb.Data[3] << 24) + (int) vdb.Data[4] + ((int) vdb.Data[5] << 8)) : (object) null;
        case DataField.Integer64bit:
          return MBusUtil.IsIntegerValid(vdb.Data) ? (object) BitConverter.ToInt64(vdb.Data, 0) : (object) null;
        case DataField.Bcd2Digit:
          return MBusUtil.IsBcdValid(vdb.Data) ? (object) Util.DecodeBcd(vdb.Data, 1) : (object) null;
        case DataField.Bcd4Digit:
          return MBusUtil.IsBcdValid(vdb.Data) ? (object) Util.DecodeBcd(vdb.Data, 2) : (object) null;
        case DataField.Bcd6Digit:
          return MBusUtil.IsBcdValid(vdb.Data) ? (object) Util.DecodeBcd(vdb.Data, 3) : (object) null;
        case DataField.Bcd8Digit:
          return MBusUtil.IsBcdValid(vdb.Data) ? (object) Util.DecodeBcd(vdb.Data, 4) : (object) null;
        case DataField.Bcd12Digit:
          return MBusUtil.IsBcdValid(vdb.Data) ? (object) Util.DecodeBcd(vdb.Data, 6) : (object) null;
        default:
          throw new Exception("Illegal data field");
      }
    }

    private void SetValueVIF_FB(VariableDataBlock vdb)
    {
      switch (vdb.VIFE[0].UnitAndMultiplier)
      {
        case 0:
        case 1:
          this.SetValue("MWh", 1.0, "Energy", "MWh", 1, -1, 1);
          break;
        case 2:
        case 3:
        case 4:
        case 5:
        case 6:
        case 7:
          this.SetValue("Reserved");
          break;
        case 8:
        case 9:
          this.SetValue("GJ", 1.0, "Energy", "GJ", 1, -1, 1);
          break;
        case 10:
        case 11:
        case 12:
        case 13:
        case 14:
        case 15:
          this.SetValue("Reserved");
          break;
        case 16:
        case 17:
          this.SetValue("QM", 1.0, "Volume", "m^3", 1, 2);
          break;
        case 18:
        case 19:
        case 20:
        case 21:
        case 22:
        case 23:
          this.SetValue("Reserved");
          break;
        case 24:
        case 25:
          this.SetValue("MassT", 1.0, "Mass", "t", 1, 2);
          break;
        case 26:
        case 27:
        case 28:
        case 29:
        case 30:
        case 31:
        case 32:
          this.SetValue("Reserved");
          break;
        case 33:
          this.SetValue("VolumeFeet^3", 1.0, "Volume", "feet^3", 2, -1, 1);
          break;
        case 34:
          this.SetValue("VolumeAmericanGallon", 1.0, "Volume", "0.1 american gallon", 2, -1, 1);
          break;
        case 35:
          this.SetValue("VolumeAmericanGallon", 1.0, "Volume", "american gallon");
          break;
        case 36:
          this.SetValue("VolumeFlowAmericanGallonPerMin", 1.0, "Volume flow", "american gallon/min", 2, -3);
          break;
        case 37:
          this.SetValue("VolumeFlowAmericanGallonPerMin", 1.0, "Volume flow", "american gallon/min");
          break;
        case 38:
          this.SetValue("VolumeFlowAmericanGallonPerHour", 1.0, "Volume flow", "american gallon/h");
          break;
        case 39:
          this.SetValue("Reserved");
          break;
        case 40:
        case 41:
          this.SetValue("kW", 1000.0, "Power", "MW", 1, -1, 1);
          break;
        case 42:
        case 43:
        case 44:
        case 45:
        case 46:
        case 47:
          this.SetValue("Reserved");
          break;
        case 48:
        case 49:
          this.SetValue("GJ/h", 1.0, "Power", "GJ/h", 1, -1, 1);
          break;
        case 50:
        case 51:
        case 52:
        case 53:
        case 54:
        case 55:
        case 56:
        case 57:
        case 58:
        case 59:
        case 60:
        case 61:
        case 62:
        case 63:
        case 64:
        case 65:
        case 66:
        case 67:
        case 68:
        case 69:
        case 70:
        case 71:
        case 72:
        case 73:
        case 74:
        case 75:
        case 76:
        case 77:
        case 78:
        case 79:
        case 80:
        case 81:
        case 82:
        case 83:
        case 84:
        case 85:
        case 86:
        case 87:
          this.SetValue("Reserved");
          break;
        case 88:
        case 89:
        case 90:
        case 91:
          this.SetValue("FlowTemperature", 1.0, "Flow Temperature", "°F", 3, -3);
          break;
        case 92:
        case 93:
        case 94:
        case 95:
          this.SetValue("ReturnTemperature", 1.0, "Return Temperature", "°F", 3, -3);
          break;
        case 96:
        case 97:
        case 98:
        case 99:
          this.SetValue("TemperatureDifference", 1.0, "Temperature Difference", "°F", 3, -3);
          break;
        case 100:
        case 101:
        case 102:
        case 103:
          this.SetValue("ExternalTemperature", 1.0, "External Temperature", "°F", 3, -3);
          break;
        case 104:
        case 105:
        case 106:
        case 107:
        case 108:
        case 109:
        case 110:
        case 111:
          this.SetValue("Reserved");
          break;
        case 112:
        case 113:
        case 114:
        case 115:
          this.SetValue("ColdWarmTemperatureLimitF", 1.0, "Cold / Warm Temperature Limit", "°F", 3, -3);
          break;
        case 116:
        case 117:
        case 118:
        case 119:
          this.SetValue("ColdWarmTemperatureLimitC", 1.0, "Cold / Warm Temperature Limit", "°C", 3, -3);
          break;
        case 120:
        case 121:
        case 122:
        case 123:
        case 124:
        case 125:
        case 126:
        case 127:
          this.SetValue("CumulCountMaxPower", 1.0, "cumul. count max power", "W", 7, -3);
          break;
        default:
          this.Description = "Unknown VIF=FBh";
          break;
      }
      this.SetOrthogonal();
      this.FinishZDF();
    }

    private void SetValueVIF_FD(VariableDataBlock vdb)
    {
      switch (vdb.VIFE[0].UnitAndMultiplier)
      {
        case 0:
        case 1:
        case 2:
        case 3:
          this.SetValue("CurrencyUnitsCredit", 1.0, "Credit", "Currency units", 3, -3);
          break;
        case 4:
        case 5:
        case 6:
        case 7:
          this.SetValue("CurrencyUnitsDebit", 1.0, "Debit", "Currency units", 3, -3);
          break;
        case 8:
          this.SetValue("Access Number (transmission count)");
          break;
        case 9:
          this.SetValue("Device type");
          break;
        case 10:
          this.SetValue("Manufacturer");
          break;
        case 11:
          this.SetValue("Parameter set identification");
          break;
        case 12:
          this.SetValue("Model");
          break;
        case 13:
          this.SetValue("Hardware version");
          break;
        case 14:
          this.SetValue("Firmware version");
          break;
        case 15:
          this.SetValue("Software version");
          break;
        case 16:
          this.SetValue("Customer location");
          break;
        case 17:
          this.SetValue("Customer");
          break;
        case 18:
          this.SetValue("Access Code User");
          break;
        case 19:
          this.SetValue("Access Code Operator");
          break;
        case 20:
          this.SetValue("Access Code System Operator");
          break;
        case 21:
          this.SetValue("Access Code Developer");
          break;
        case 22:
          this.SetValue("Password");
          break;
        case 23:
          this.SetValue("Error flags");
          break;
        case 24:
          this.SetValue("Error mask");
          break;
        case 25:
          this.SetValue("Reserved");
          break;
        case 26:
          this.SetValue("Digital Output");
          break;
        case 27:
          this.SetValue("Digital Input");
          break;
        case 28:
          this.SetValue("Baudrate", 1.0, "Baudrate", "Baud");
          break;
        case 29:
          this.SetValue("ResponseDelayTime", 1.0, "Response delay time", "Bittimes");
          break;
        case 30:
          this.SetValue("Retry");
          break;
        case 31:
          this.SetValue("Reserved");
          break;
        case 32:
          this.SetValue("First storage # for cyclic storage");
          break;
        case 33:
          this.SetValue("Last storage # for cyclic storage");
          break;
        case 34:
          this.SetValue("Size of storage block");
          break;
        case 35:
          this.SetValue("Reserved");
          break;
        case 36:
          this.SetValue("StorageIntervalSec", 1.0, "Storage interval", "seconds");
          break;
        case 37:
          this.SetValue("StorageIntervalMin", 1.0, "Storage interval", "minutes");
          break;
        case 38:
          this.SetValue("StorageIntervalHour", 1.0, "Storage interval", "hours");
          break;
        case 39:
          this.SetValue("StorageIntervalDay", 1.0, "Storage interval", "days");
          break;
        case 40:
          this.SetValue("StorageIntervalMonth", 1.0, "Storage interval", "months");
          break;
        case 41:
          this.SetValue("StorageIntervalYear", 1.0, "Storage interval", "years");
          break;
        case 42:
          this.SetValue("Reserved");
          break;
        case 43:
          this.SetValue("TimeSec", 1.0, "Time", "seconds");
          break;
        case 44:
          this.SetValue("DurationSinceLastReadoutSec", 1.0, "Duration since last readout", "seconds");
          break;
        case 45:
          this.SetValue("DurationSinceLastReadoutMin", 1.0, "Duration since last readout", "minutes");
          break;
        case 46:
          this.SetValue("DurationSinceLastReadoutHour", 1.0, "Duration since last readout", "hours");
          break;
        case 47:
          this.SetValue("DurationSinceLastReadoutDay", 1.0, "Duration since last readout", "days");
          break;
        case 48:
          this.SetDateTimeValue("Start of tariff");
          break;
        case 49:
          this.SetValue("TariffStorageIntervalMin", 1.0, "Duration of tariff", "minutes");
          break;
        case 50:
          this.SetValue("TariffStorageIntervalHour", 1.0, "Duration of tariff", "hours");
          break;
        case 51:
          this.SetValue("TariffStorageIntervalDay", 1.0, "Duration of tariff", "days");
          break;
        case 52:
          this.SetValue("PeriodOfTariffSec", 1.0, "Period of tariff", "seconds");
          break;
        case 53:
          this.SetValue("PeriodOfTariffMin", 1.0, "Period of tariff", "minutes");
          break;
        case 54:
          this.SetValue("PeriodOfTariffHour", 1.0, "Period of tariff", "hours");
          break;
        case 55:
          this.SetValue("PeriodOfTariffDay", 1.0, "Period of tariff", "days");
          break;
        case 56:
          this.SetValue("PeriodOfTariffMonth", 1.0, "Period of tariff", "months");
          break;
        case 57:
          this.SetValue("PeriodOfTariffYear", 1.0, "Period of tariff", "years");
          break;
        case 58:
          this.SetValue("Dimensionless");
          break;
        case 59:
          this.SetValue("wM-Bus data");
          break;
        case 60:
          this.SetValue("SendIntervalSec", 1.0, "Send interval", "seconds");
          break;
        case 61:
          this.SetValue("SendIntervalMin", 1.0, "Send interval", "minutes");
          break;
        case 62:
          this.SetValue("SendIntervalHour", 1.0, "Send interval", "hours");
          break;
        case 63:
          this.SetValue("SendIntervalDay", 1.0, "Send interval", "days");
          break;
        case 64:
        case 65:
        case 66:
        case 67:
        case 68:
        case 69:
        case 70:
        case 71:
        case 72:
        case 73:
        case 74:
        case 75:
        case 76:
        case 77:
        case 78:
        case 79:
          this.SetValue("V", 1.0, "Voltage", "V", 15, -9);
          break;
        case 80:
        case 81:
        case 82:
        case 83:
        case 84:
        case 85:
        case 86:
        case 87:
        case 88:
        case 89:
        case 90:
        case 91:
        case 92:
        case 93:
        case 94:
        case 95:
          this.SetValue("A", 1.0, "Current", "A", 15, -12);
          break;
        case 96:
          this.SetValue("Reset counter");
          break;
        case 97:
          this.SetValue("Cumulation counter");
          break;
        case 98:
          this.SetValue("Control signal");
          break;
        case 99:
          this.SetValue("Day of week");
          break;
        case 100:
          this.SetValue("Week number");
          break;
        case 101:
          this.SetValue("Time point of day change");
          break;
        case 102:
          this.SetValue("State of parameter activation");
          break;
        case 103:
          this.SetValue("Special supplier information");
          break;
        case 104:
          this.SetValue("DurationSinceLastCumulationHour", 1.0, "Duration since last cumulation", "hours");
          break;
        case 105:
          this.SetValue("DurationSinceLastCumulationDay", 1.0, "Duration since last cumulation", "days");
          break;
        case 106:
          this.SetValue("DurationSinceLastCumulationMonth", 1.0, "Duration since last cumulation", "months");
          break;
        case 107:
          this.SetValue("DurationSinceLastCumulationYear", 1.0, "Duration since last cumulation", "years");
          break;
        case 108:
          this.SetValue("OperatingTimeBatteryHour", 1.0, "Operating time battery", "hours");
          break;
        case 109:
          this.SetValue("OperatingTimeBatteryDay", 1.0, "Operating time battery", "days");
          break;
        case 110:
          this.SetValue("OperatingTimeBatteryMonth", 1.0, "Operating time battery", "months");
          break;
        case 111:
          this.SetValue("OperatingTimeBatteryYear", 1.0, "Operating time battery", "years");
          break;
        case 112:
          this.SetDateTimeValue("Date and time of battery change");
          break;
        case 113:
          this.SetDateTimeValue("Receive level");
          break;
        case 114:
        case 115:
        case 116:
        case 117:
        case 118:
        case 119:
        case 120:
        case 121:
        case 122:
        case 123:
        case 124:
        case 125:
        case 126:
        case 127:
          this.SetValue("Reserved");
          break;
        default:
          this.Description = "Unknown VIF=FDh";
          break;
      }
      this.SetOrthogonal();
      this.FinishZDF();
    }

    private void SetDateTimeValue(string description)
    {
      this.Description = description;
      switch (this.vdb.DIF.DataField)
      {
        case DataField.Integer16bit:
          this.Unit = "date";
          this.Value = (object) MBusUtil.ConvertToDate_MBus_CP16_TypeG(this.vdb.Data);
          break;
        case DataField.Integer24bit:
          this.Unit = "time";
          this.Value = (object) null;
          break;
        case DataField.Integer32bit:
          this.Unit = "time & date";
          this.Value = (object) MBusUtil.ConvertToDateTime_MBus_CP32_TypeF(this.vdb.Data);
          break;
      }
    }

    private void SetOrthogonal()
    {
      if (this.vdb.VIFE == null || this.vdb.VIFE.Count == 0)
        return;
      byte unitAndMultiplier = this.vdb.VIFE[0].UnitAndMultiplier;
      switch (unitAndMultiplier)
      {
        case 31:
          this.SetOrthogonal("compact profile without register", "_CPWR");
          break;
        case 32:
          this.SetOrthogonal("per second", "_PerSecond", "/second");
          break;
        case 33:
          this.SetOrthogonal("per minute", "_PerMinute", "/minute");
          break;
        case 34:
          this.SetOrthogonal("per hour", "_PerHour", "/h");
          break;
        case 35:
          this.SetOrthogonal("per day", "_PerDay", "/day");
          break;
        case 36:
          this.SetOrthogonal("per week", "_PerWeek", "/week");
          break;
        case 37:
          this.SetOrthogonal("per month", "_PerMonth", "/month");
          break;
        case 38:
          this.SetOrthogonal("per year", "_PerYear", "/year");
          break;
        case 39:
          this.SetOrthogonal("per revolution / measurement", "_PerCycle", "/cycle");
          break;
        case 40:
          this.SetOrthogonal("increment per input pulse on input channel #0", "_IncInp0");
          break;
        case 41:
          this.SetOrthogonal("increment per input pulse on input channel #1", "_IncInp1");
          break;
        case 42:
          this.SetOrthogonal("increment per output pulse on output channel #0", "_IncOut0");
          break;
        case 43:
          this.SetOrthogonal("increment per output pulse on output channel #1", "_IncOut1");
          break;
        case 44:
          this.SetOrthogonal("per liter", "_PerLiter", "/l");
          break;
        case 45:
          this.SetOrthogonal("per m\u00B3", "_PerQM", "/m\u00B3");
          break;
        case 46:
          this.SetOrthogonal("per kg", "_PerKg", "/kg");
          break;
        case 47:
          this.SetOrthogonal("per K (Kelvin)", "_PerK", "/K");
          break;
        case 48:
          this.SetOrthogonal("per kWh", "_PerkWh", "/kWh");
          break;
        case 49:
          this.SetOrthogonal("per GJ", "_PerGJ", "/GJ");
          break;
        case 50:
          this.SetOrthogonal("per kW", "_PerkW", "/kW");
          break;
        case 51:
          this.SetOrthogonal("per (K*l) (Kelvin*liter)", "_PerK*l", "/K*l");
          break;
        case 52:
          this.SetOrthogonal("per V (Volt)", "_PerV", "/V");
          break;
        case 53:
          this.SetOrthogonal("per A (Ampere)", "_PerA", "/A");
          break;
        case 54:
          this.SetOrthogonal("multiplied by sek", "_*s", "*s");
          break;
        case 55:
          this.SetOrthogonal("multiplied by sek/V", "_*s/V", "*s/V");
          break;
        case 56:
          this.SetOrthogonal("multiplied by sek/A", "_*s/A", "*s/A");
          break;
        case 57:
          this.SetOrthogonal("start date(/time) of", "_STime");
          break;
        case 58:
          this.SetOrthogonal("VIF contains uncorrected unit instead of corrected unit", "_RAW");
          break;
        case 59:
          this.SetOrthogonal("Accumulation only if positive contributions", "+");
          break;
        case 60:
          this.SetOrthogonal("Accumulation of abs value only if negative contributions", "-");
          break;
        case 64:
          this.SetOrthogonal("Lower limit value", "_LowerLimit");
          break;
        case 65:
          this.SetOrthogonal("# of exceeds of lower limit", "_OfExceedsOfLower");
          break;
        case 66:
          this.SetOrthogonal("Date (/time) of begin of first lower limit exceed", "_TimeOfBeginFirstLower");
          break;
        case 67:
          this.SetOrthogonal("Date (/time) of end of first lower limit exceed", "_TimeOfEndFirstLower");
          break;
        case 70:
          this.SetOrthogonal("Date (/time) of begin of last lower limit exceed", "_TimeOfBeginLastLower");
          break;
        case 71:
          this.SetOrthogonal("Date (/time) of end of last lower limit exceed", "_TimeOfEndLastLower");
          break;
        case 72:
          this.SetOrthogonal("Upper limit value", "_UpperLimit");
          break;
        case 73:
          this.SetOrthogonal("# of exceeds of upper limit", "_OfExceedsOfUpper");
          break;
        case 74:
          this.SetOrthogonal("Date (/time) of begin of first upper limit exceed", "_TimeOfBeginFirstUpper");
          break;
        case 75:
          this.SetOrthogonal("Date (/time) of end of first upper limit exceed", "_TimeOfEndFirstUpper");
          break;
        case 78:
          this.SetOrthogonal("Date (/time) of begin of last upper limit exceed", "_TimeOfBeginLastUpper");
          break;
        case 79:
          this.SetOrthogonal("Date (/time) of end of last upper limit exceed", "_TimeOfEndLastUpper");
          break;
        case 80:
        case 81:
        case 82:
        case 83:
        case 84:
        case 85:
        case 86:
        case 87:
        case 88:
        case 89:
        case 90:
        case 91:
        case 92:
        case 93:
        case 94:
        case 95:
        case 96:
        case 97:
        case 98:
        case 99:
        case 100:
        case 101:
        case 102:
        case 103:
          string empty1 = string.Empty;
          string empty2 = string.Empty;
          string str1;
          string str2;
          if (((int) unitAndMultiplier & 124) == 80)
          {
            str1 = "Duration of first lower limit exceed";
            str2 = "DurationOfFirstLowerLimitExceed";
          }
          else if (((int) unitAndMultiplier & 124) == 84)
          {
            str1 = "Duration of last lower limit exceed";
            str2 = "DurationOfLastLowerLimitExceed";
          }
          else if (((int) unitAndMultiplier & 124) == 88)
          {
            str1 = "Duration of first upper limit exceed";
            str2 = "DurationOfFirstUpperLimitExceed ";
          }
          else if (((int) unitAndMultiplier & 124) == 92)
          {
            str1 = "Duration of last upper limit exceed";
            str2 = "DurationOfLastUpperLimitExceed";
          }
          else if (((int) unitAndMultiplier & 124) == 96)
          {
            str1 = "Duration of first";
            str2 = "DurationOfFirst";
          }
          else
          {
            str1 = "Duration of last";
            str2 = "DurationOfLast";
          }
          switch ((int) unitAndMultiplier & 3)
          {
            case 0:
              this.SetOrthogonal(str1 + " seconds", str2 + "Secs");
              return;
            case 1:
              this.SetOrthogonal(str1 + " minutes", str2 + "Mins");
              return;
            case 2:
              this.SetOrthogonal(str1 + " hours", str2 + "Hours");
              return;
            case 3:
              this.SetOrthogonal(str1 + " days", str2 + "Days");
              return;
            default:
              return;
          }
        case 106:
          this.SetOrthogonal("Date (/time) of begin of first", "_DateBeginFirst");
          break;
        case 107:
          this.SetOrthogonal("Date (/time) of end of first", "_DateEndFirst");
          break;
        case 108:
          this.SetOrthogonal("Leakage", "_Leakage");
          break;
        case 109:
          this.SetOrthogonal("Overflow", "_Overflow");
          break;
        case 110:
          this.SetOrthogonal("Date (/time) of begin of last", "_DateBeginLast");
          break;
        case 111:
          this.SetOrthogonal("Date (/time) of end of last", "_DateEndLast");
          break;
        case 112:
        case 113:
        case 114:
        case 115:
        case 116:
        case 117:
        case 118:
        case 119:
        case 120:
        case 121:
          this.ScaleValue(1.0, 3, Math.Pow(10.0, (double) (((int) this.vdb.VIF.Value & 7) - 6)), this.Value);
          break;
      }
    }

    private void SetOrthogonal(string desc, string zdf, string unit = "")
    {
      this.Description = this.Description + " " + desc;
      this.Zdf += zdf;
      this.Unit += unit;
    }

    private void SetStorageNumber()
    {
      int num = 1;
      this.StorageNumber = this.vdb.DIF.LsbOfStorageNumber;
      if (this.vdb.DIFE == null)
        return;
      foreach (DIFE dife in this.vdb.DIFE)
      {
        this.StorageNumber |= dife.StorageNumber << num;
        num += 4;
      }
    }

    private void SetSubUnit()
    {
      int num = 0;
      this.SubUnit = 0;
      if (this.vdb.DIFE == null)
        return;
      foreach (DIFE dife in this.vdb.DIFE)
      {
        this.SubUnit |= dife.SubUnit << num;
        ++num;
      }
    }

    private void SetTariff()
    {
      int num = 0;
      this.Tariff = 0L;
      if (this.vdb.DIFE == null)
        return;
      foreach (DIFE dife in this.vdb.DIFE)
      {
        this.Tariff |= dife.Tariff << num;
        num += 2;
      }
    }

    private void FinishZDF()
    {
      if (this.StorageNumber > 0L || this.SubUnit > 0)
        this.Zdf += string.Format("[{0}]", (object) this.StorageNumber);
      if (this.SubUnit > 0)
        this.Zdf += string.Format("[{0}]", (object) this.SubUnit);
      if (this.vdb.DIF.FunctionField == FunctionField.Max)
        this.Zdf += "_MAX";
      else if (this.vdb.DIF.FunctionField == FunctionField.Min)
        this.Zdf += "_MIN";
      else if (this.vdb.DIF.FunctionField == FunctionField.ValueDuringErrorState)
        this.Zdf += "_ERR";
      if (this.Tariff <= 0L)
        return;
      this.Zdf += string.Format("_TAR[{0}]", (object) this.Tariff);
    }
  }
}

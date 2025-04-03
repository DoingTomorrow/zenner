// Decompiled with JetBrains decompiler
// Type: DeviceCollector.MBusDevice
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using HandlerLib;
using NLog;
using StartupLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class MBusDevice : BusDevice
  {
    protected static Logger MBusDeviceLogger = LogManager.GetLogger(nameof (MBusDevice));
    private static int[] DifLengthTable = new int[16]
    {
      0,
      1,
      2,
      3,
      4,
      4,
      6,
      8,
      0,
      1,
      2,
      3,
      4,
      4,
      6,
      8
    };
    internal static MBusDevice.ParamCode[] DifParamCodeTable = new MBusDevice.ParamCode[16]
    {
      MBusDevice.ParamCode.None,
      MBusDevice.ParamCode.Int,
      MBusDevice.ParamCode.Int,
      MBusDevice.ParamCode.Int,
      MBusDevice.ParamCode.Int,
      MBusDevice.ParamCode.Real,
      MBusDevice.ParamCode.Int,
      MBusDevice.ParamCode.Int,
      MBusDevice.ParamCode.Readout,
      MBusDevice.ParamCode.BCD,
      MBusDevice.ParamCode.BCD,
      MBusDevice.ParamCode.BCD,
      MBusDevice.ParamCode.BCD,
      MBusDevice.ParamCode.Variable,
      MBusDevice.ParamCode.BCD,
      MBusDevice.ParamCode.Special
    };
    public static MBusDevice.VifStruct[] VifList = new MBusDevice.VifStruct[128]
    {
      new MBusDevice.VifStruct("MWH", -9, 0, "MWh"),
      new MBusDevice.VifStruct("MWH", -8, 0, "MWh"),
      new MBusDevice.VifStruct("MWH", -7, 0, "MWh"),
      new MBusDevice.VifStruct("MWH", -6, 0, "MWh"),
      new MBusDevice.VifStruct("MWH", -5, 0, "MWh"),
      new MBusDevice.VifStruct("MWH", -4, 0, "MWh"),
      new MBusDevice.VifStruct("MWH", -3, 0, "MWh"),
      new MBusDevice.VifStruct("MWH", -2, 0, "MWh"),
      new MBusDevice.VifStruct("GJ", -9, 0, "GJ"),
      new MBusDevice.VifStruct("GJ", -8, 0, "GJ"),
      new MBusDevice.VifStruct("GJ", -7, 0, "GJ"),
      new MBusDevice.VifStruct("GJ", -6, 0, "GJ"),
      new MBusDevice.VifStruct("GJ", -5, 0, "GJ"),
      new MBusDevice.VifStruct("GJ", -4, 0, "GJ"),
      new MBusDevice.VifStruct("GJ", -3, 0, "GJ"),
      new MBusDevice.VifStruct("GJ", -2, 0, "GJ"),
      new MBusDevice.VifStruct("QM", -6, 0, "m\u00B3"),
      new MBusDevice.VifStruct("QM", -5, 0, "m\u00B3"),
      new MBusDevice.VifStruct("QM", -4, 0, "m\u00B3"),
      new MBusDevice.VifStruct("QM", -3, 0, "m\u00B3"),
      new MBusDevice.VifStruct("QM", -2, 0, "m\u00B3"),
      new MBusDevice.VifStruct("QM", -1, 0, "m\u00B3"),
      new MBusDevice.VifStruct("QM", 0, 0, "m\u00B3"),
      new MBusDevice.VifStruct("QM", 1, 0, "m\u00B3"),
      new MBusDevice.VifStruct("Kg", -3, 0, "Kg"),
      new MBusDevice.VifStruct("Kg", -3, 0, "Kg"),
      new MBusDevice.VifStruct("Kg", -3, 0, "Kg"),
      new MBusDevice.VifStruct("Kg", -3, 0, "Kg"),
      new MBusDevice.VifStruct("Kg", -3, 0, "Kg"),
      new MBusDevice.VifStruct("Kg", -3, 0, "Kg"),
      new MBusDevice.VifStruct("Kg", -3, 0, "Kg"),
      new MBusDevice.VifStruct("Kg", -3, 0, "Kg"),
      new MBusDevice.VifStruct("OnSecs", 0, 0, "s"),
      new MBusDevice.VifStruct("OnMins", 0, 0, "s"),
      new MBusDevice.VifStruct("OnHours", 0, 0, "s"),
      new MBusDevice.VifStruct("OnDays", 0, 0, "s"),
      new MBusDevice.VifStruct("OpSecs", 0, 0, "s"),
      new MBusDevice.VifStruct("OpMins", 0, 0, "s"),
      new MBusDevice.VifStruct("OpHours", 0, 0, "s"),
      new MBusDevice.VifStruct("OpDays", 0, 0, "s"),
      new MBusDevice.VifStruct("kW", -6, 0, "kW"),
      new MBusDevice.VifStruct("kW", -5, 0, "kW"),
      new MBusDevice.VifStruct("kW", -4, 0, "kW"),
      new MBusDevice.VifStruct("kW", -3, 0, "kW"),
      new MBusDevice.VifStruct("kW", -2, 0, "kW"),
      new MBusDevice.VifStruct("kW", -1, 0, "kW"),
      new MBusDevice.VifStruct("kW", 0, 0, "kW"),
      new MBusDevice.VifStruct("kW", 1, 0, "kW"),
      new MBusDevice.VifStruct("GJ/h", -9, 0, "GJ/h"),
      new MBusDevice.VifStruct("GJ/h", -8, 0, "GJ/h"),
      new MBusDevice.VifStruct("GJ/h", -7, 0, "GJ/h"),
      new MBusDevice.VifStruct("GJ/h", -6, 0, "GJ/h"),
      new MBusDevice.VifStruct("GJ/h", -5, 0, "GJ/h"),
      new MBusDevice.VifStruct("GJ/h", -4, 0, "GJ/h"),
      new MBusDevice.VifStruct("GJ/h", -3, 0, "GJ/h"),
      new MBusDevice.VifStruct("GJ/h", -2, 0, "GJ/h"),
      new MBusDevice.VifStruct("QMPH", -6, 0, "m\u00B3/h"),
      new MBusDevice.VifStruct("QMPH", -5, 0, "m\u00B3/h"),
      new MBusDevice.VifStruct("QMPH", -4, 0, "m\u00B3/h"),
      new MBusDevice.VifStruct("QMPH", -3, 0, "m\u00B3/h"),
      new MBusDevice.VifStruct("QMPH", -2, 0, "m\u00B3/h"),
      new MBusDevice.VifStruct("QMPH", -1, 0, "m\u00B3/h"),
      new MBusDevice.VifStruct("QMPH", 0, 0, "m\u00B3/h"),
      new MBusDevice.VifStruct("QMPH", 1, 0, "m\u00B3/h"),
      new MBusDevice.VifStruct("QMPM", -7, 0, "m\u00B3/min"),
      new MBusDevice.VifStruct("QMPM", -6, 0, "m\u00B3/min"),
      new MBusDevice.VifStruct("QMPM", -5, 0, "m\u00B3/min"),
      new MBusDevice.VifStruct("QMPM", -4, 0, "m\u00B3/min"),
      new MBusDevice.VifStruct("QMPM", -3, 0, "m\u00B3/min"),
      new MBusDevice.VifStruct("QMPM", -2, 0, "m\u00B3/min"),
      new MBusDevice.VifStruct("QMPM", -1, 0, "m\u00B3/min"),
      new MBusDevice.VifStruct("QMPM", 0, 0, "m\u00B3/min"),
      new MBusDevice.VifStruct("QMPS", -9, 0, "m\u00B3/s"),
      new MBusDevice.VifStruct("QMPS", -8, 0, "m\u00B3/s"),
      new MBusDevice.VifStruct("QMPS", -7, 0, "m\u00B3/s"),
      new MBusDevice.VifStruct("QMPS", -6, 0, "m\u00B3/s"),
      new MBusDevice.VifStruct("QMPS", -5, 0, "m\u00B3/s"),
      new MBusDevice.VifStruct("QMPS", -4, 0, "m\u00B3/s"),
      new MBusDevice.VifStruct("QMPS", -3, 0, "m\u00B3/s"),
      new MBusDevice.VifStruct("QMPS", -2, 0, "m\u00B3/s"),
      new MBusDevice.VifStruct("KGPH", -3, 0, "kg/h"),
      new MBusDevice.VifStruct("KGPH", -2, 0, "kg/h"),
      new MBusDevice.VifStruct("KGPH", -1, 0, "kg/h"),
      new MBusDevice.VifStruct("KGPH", 0, 0, "kg/h"),
      new MBusDevice.VifStruct("KGPH", 1, 0, "kg/h"),
      new MBusDevice.VifStruct("KGPH", 2, 0, "kg/h"),
      new MBusDevice.VifStruct("KGPH", 3, 0, "kg/h"),
      new MBusDevice.VifStruct("KGPH", 4, 0, "kg/h"),
      new MBusDevice.VifStruct("TF", -3, 0, "°C"),
      new MBusDevice.VifStruct("TF", -2, 0, "°C"),
      new MBusDevice.VifStruct("TF", -1, 0, "°C"),
      new MBusDevice.VifStruct("TF", 0, 0, "°C"),
      new MBusDevice.VifStruct("TR", -3, 0, "°C"),
      new MBusDevice.VifStruct("TR", -2, 0, "°C"),
      new MBusDevice.VifStruct("TR", -1, 0, "°C"),
      new MBusDevice.VifStruct("TR", 0, 0, "°C"),
      new MBusDevice.VifStruct("TD", -3, 0, "K"),
      new MBusDevice.VifStruct("TD", -2, 0, "K"),
      new MBusDevice.VifStruct("TD", -1, 0, "K"),
      new MBusDevice.VifStruct("TD", 0, 0, "K"),
      new MBusDevice.VifStruct("TX", -3, 0, "°C"),
      new MBusDevice.VifStruct("TX", -2, 0, "°C"),
      new MBusDevice.VifStruct("TX", -1, 0, "°C"),
      new MBusDevice.VifStruct("TX", 0, 0, "°C"),
      new MBusDevice.VifStruct("HePas", -3, 0, "bar"),
      new MBusDevice.VifStruct("HePas", -2, 0, "bar"),
      new MBusDevice.VifStruct("HePas", -1, 0, "bar"),
      new MBusDevice.VifStruct("HePas", 0, 0, "bar"),
      new MBusDevice.VifStruct("TIMP", 0, 0, "-"),
      new MBusDevice.VifStruct("TIMP", 0, 0, "-"),
      new MBusDevice.VifStruct("HCA", 0, 0, "-"),
      new MBusDevice.VifStruct("Reserved_VifList_0x6f", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("AvDSecs", 0, 0, "sec"),
      new MBusDevice.VifStruct("AvDMins", 0, 0, "min"),
      new MBusDevice.VifStruct("AvDHours", 0, 0, "hour"),
      new MBusDevice.VifStruct("AvDDays", 0, 0, "day"),
      new MBusDevice.VifStruct("AcDSecs", 0, 0, "sec"),
      new MBusDevice.VifStruct("AcDMins", 0, 0, "min"),
      new MBusDevice.VifStruct("AcDHours", 0, 0, "hour"),
      new MBusDevice.VifStruct("AcDDays", 0, 0, "day"),
      new MBusDevice.VifStruct("FAB", 0, 0, "-"),
      new MBusDevice.VifStruct("CID", 0, 0, "-"),
      new MBusDevice.VifStruct("ADR", 0, 0, "-"),
      new MBusDevice.VifStruct("??_VifList_0x7b", 0, 0, "??"),
      new MBusDevice.VifStruct("UserDefinableVIF", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifList_0x7d", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifList_0x7e", 0, 0, "??"),
      new MBusDevice.VifStruct("Manuf", 0, 0, "-")
    };
    public static MBusDevice.VifStruct[] VifEList_0xFD = new MBusDevice.VifStruct[128]
    {
      new MBusDevice.VifStruct("CurrencyUnitsCredit", -3, 0, "Credit"),
      new MBusDevice.VifStruct("CurrencyUnitsCredit", -2, 0, "Credit"),
      new MBusDevice.VifStruct("CurrencyUnitsCredit", -1, 0, "Credit"),
      new MBusDevice.VifStruct("CurrencyUnitsCredit", 0, 0, "Credit"),
      new MBusDevice.VifStruct("CurrencyUnitsDebit", -3, 0, "Debit"),
      new MBusDevice.VifStruct("CurrencyUnitsDebit", -2, 0, "Debit"),
      new MBusDevice.VifStruct("CurrencyUnitsDebit", -1, 0, "Debit"),
      new MBusDevice.VifStruct("CurrencyUnitsDebit", 0, 0, "Debit"),
      new MBusDevice.VifStruct("AccessNumber", 0, 0, "-"),
      new MBusDevice.VifStruct("TYPE", 0, 0, "-"),
      new MBusDevice.VifStruct("Manufacturer", 0, 0, "-"),
      new MBusDevice.VifStruct("ParaSet", 0, 0, "-"),
      new MBusDevice.VifStruct("Model", 0, 0, "-"),
      new MBusDevice.VifStruct("Hardware", 0, 0, "-"),
      new MBusDevice.VifStruct("Firmware", 0, 0, "-"),
      new MBusDevice.VifStruct("Software", 0, 0, "-"),
      new MBusDevice.VifStruct("CustomerLocation", 0, 0, "-"),
      new MBusDevice.VifStruct("Customer", 0, 0, "-"),
      new MBusDevice.VifStruct("AccessCodeUser", 0, 0, "-"),
      new MBusDevice.VifStruct("AccessCodeOperator", 0, 0, "-"),
      new MBusDevice.VifStruct("AccessCodeSystemOperator", 0, 0, "-"),
      new MBusDevice.VifStruct("AccessCodeDeveloper", 0, 0, "-"),
      new MBusDevice.VifStruct("Password", 0, 0, "-"),
      new MBusDevice.VifStruct("ERR", 0, 8, "-", true),
      new MBusDevice.VifStruct("ERR_MASK", 0, 0, "-"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x19", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("DIGI_OUT", 0, 8, "-"),
      new MBusDevice.VifStruct("DIGI_IN", 0, 8, "-"),
      new MBusDevice.VifStruct("Baudrate", 0, 0, "-"),
      new MBusDevice.VifStruct("ResponseDelayTime", 0, 0, "bittimes"),
      new MBusDevice.VifStruct("Retry", 0, 0, "-"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x1f", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("FirstStorageNrForCyclicStorage", 0, 0, "-"),
      new MBusDevice.VifStruct("LastStorageNrForCyclicStorage", 0, 0, "-"),
      new MBusDevice.VifStruct("SizeOfStorageBlock", 0, 0, "-"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x23", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("StorageIntervalSec", 0, 0, "sec"),
      new MBusDevice.VifStruct("StorageIntervalMin", 0, 0, "min"),
      new MBusDevice.VifStruct("StorageIntervalHour", 0, 0, "hour"),
      new MBusDevice.VifStruct("StorageIntervalDay", 0, 0, "day"),
      new MBusDevice.VifStruct("StorageIntervalMonth", 0, 0, "month"),
      new MBusDevice.VifStruct("StorageIntervalYear", 0, 0, "year"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x2a", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x2b", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("DurationSinceLastReadoutSec", 0, 0, "sec"),
      new MBusDevice.VifStruct("DurationSinceLastReadoutMin", 0, 0, "min"),
      new MBusDevice.VifStruct("DurationSinceLastReadoutHour", 0, 0, "hour"),
      new MBusDevice.VifStruct("DurationSinceLastReadoutDay", 0, 0, "day"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x30", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("TariffStorageIntervalMin", 0, 0, "min"),
      new MBusDevice.VifStruct("TariffStorageIntervalHour", 0, 0, "hour"),
      new MBusDevice.VifStruct("TariffStorageIntervalDay", 0, 0, "day"),
      new MBusDevice.VifStruct("PeriodOfTariffSec", 0, 0, "sec"),
      new MBusDevice.VifStruct("PeriodOfTariffMin", 0, 0, "min"),
      new MBusDevice.VifStruct("PeriodOfTariffHour", 0, 0, "hour"),
      new MBusDevice.VifStruct("PeriodOfTariffDay", 0, 0, "day"),
      new MBusDevice.VifStruct("PeriodOfTariffMonth", 0, 0, "month"),
      new MBusDevice.VifStruct("PeriodOfTariffYear", 0, 0, "year"),
      new MBusDevice.VifStruct("Dimensionless", 0, 0, "-"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x3b", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x3c", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x3d", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x3e", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x3f", 1, 0, "Reserved"),
      new MBusDevice.VifStruct("V", -9, 0, "V"),
      new MBusDevice.VifStruct("V", -8, 0, "V"),
      new MBusDevice.VifStruct("V", -7, 0, "V"),
      new MBusDevice.VifStruct("V", -6, 0, "V"),
      new MBusDevice.VifStruct("V", -5, 0, "V"),
      new MBusDevice.VifStruct("V", -4, 0, "V"),
      new MBusDevice.VifStruct("V", -3, 0, "V"),
      new MBusDevice.VifStruct("V", -2, 0, "V"),
      new MBusDevice.VifStruct("V", -1, 0, "V"),
      new MBusDevice.VifStruct("V", 0, 0, "V"),
      new MBusDevice.VifStruct("V", 1, 0, "V"),
      new MBusDevice.VifStruct("V", 2, 0, "V"),
      new MBusDevice.VifStruct("V", 3, 0, "V"),
      new MBusDevice.VifStruct("V", 4, 0, "V"),
      new MBusDevice.VifStruct("V", 5, 0, "V"),
      new MBusDevice.VifStruct("V", 6, 0, "V"),
      new MBusDevice.VifStruct("A", -12, 0, "A"),
      new MBusDevice.VifStruct("A", -11, 0, "A"),
      new MBusDevice.VifStruct("A", -10, 0, "A"),
      new MBusDevice.VifStruct("A", -9, 0, "A"),
      new MBusDevice.VifStruct("A", -8, 0, "A"),
      new MBusDevice.VifStruct("A", -7, 0, "A"),
      new MBusDevice.VifStruct("A", -6, 0, "A"),
      new MBusDevice.VifStruct("A", -5, 0, "A"),
      new MBusDevice.VifStruct("A", -4, 0, "A"),
      new MBusDevice.VifStruct("A", -3, 0, "A"),
      new MBusDevice.VifStruct("A", -2, 0, "A"),
      new MBusDevice.VifStruct("A", -1, 0, "A"),
      new MBusDevice.VifStruct("A", 0, 0, "A"),
      new MBusDevice.VifStruct("A", 1, 0, "A"),
      new MBusDevice.VifStruct("A", 2, 0, "A"),
      new MBusDevice.VifStruct("A", 3, 0, "A"),
      new MBusDevice.VifStruct("RES0000", 0, 0, "-"),
      new MBusDevice.VifStruct("CumulationCounter", 0, 0, "-"),
      new MBusDevice.VifStruct("ControlSignal", 0, 0, "-"),
      new MBusDevice.VifStruct("DayOfWeek", 0, 0, "-"),
      new MBusDevice.VifStruct("WeekNumber", 0, 0, "-"),
      new MBusDevice.VifStruct("TimePointOfDayChange", 0, 0, "-"),
      new MBusDevice.VifStruct("StateOfParameterActivation", 0, 0, "-"),
      new MBusDevice.VifStruct("SPEC_SUP_INF", 0, 0, "-"),
      new MBusDevice.VifStruct("DurationSinceLastCumulationHour", 0, 0, "hour"),
      new MBusDevice.VifStruct("DurationSinceLastCumulationDay", 0, 0, "day"),
      new MBusDevice.VifStruct("DurationSinceLastCumulationMonth", 0, 0, "month"),
      new MBusDevice.VifStruct("DurationSinceLastCumulationYear", 0, 0, "year"),
      new MBusDevice.VifStruct("OperatingTimeBatteryHour", 0, 0, "hour"),
      new MBusDevice.VifStruct("OperatingTimeBatteryDay", 0, 0, "day"),
      new MBusDevice.VifStruct("OperatingTimeBatteryMonth", 0, 0, "month"),
      new MBusDevice.VifStruct("OperatingTimeBatteryYear", 0, 0, "year"),
      new MBusDevice.VifStruct("DateAndTimeOfBatteryChange", 0, 0, "-"),
      new MBusDevice.VifStruct("RSSI", 0, 0, "dBm"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x72", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x73", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x74", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x75", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x76", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x77", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x78", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x79", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x7a", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x7b", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x7c", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x7d", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x7e", 0, 0, "Reserved"),
      new MBusDevice.VifStruct("Reserved_VifEList_0xFD_0x7f", 0, 0, "Reserved")
    };
    public static MBusDevice.VifStruct[] VifEList_0xFB = new MBusDevice.VifStruct[128]
    {
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 8, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 8, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 8, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 1, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("Temperature_Limit", 0, 0, "°C"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("", 0, 0, ""),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??"),
      new MBusDevice.VifStruct("??_VifEList_0xFB", 0, 0, "??")
    };
    public static MBusDevice.OrtoVifStruct[] VifEListOrto = new MBusDevice.OrtoVifStruct[128]
    {
      new MBusDevice.OrtoVifStruct("", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x01", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x02", 0, false, ""),
      new MBusDevice.OrtoVifStruct("", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x04", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x05", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x06", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x07", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x08", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x09", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x0a", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x0b", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x0c", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x0d", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x0e", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x0f", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x10", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x11", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x12", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x13", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x14", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x15", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x16", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x17", 0, false, ""),
      new MBusDevice.OrtoVifStruct("", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x19", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x1a", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x1b", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x1c", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x1d", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x1e", 0, false, ""),
      new MBusDevice.OrtoVifStruct("_CPWR", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x20", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x21", 0, false, ""),
      new MBusDevice.OrtoVifStruct("_PerHour", 0, false, "/h"),
      new MBusDevice.OrtoVifStruct("_PerDay", 0, false, "/day"),
      new MBusDevice.OrtoVifStruct("_PerWeek", 0, false, "/week"),
      new MBusDevice.OrtoVifStruct("_PerMonth", 0, false, "/month"),
      new MBusDevice.OrtoVifStruct("_PerYear", 0, false, "/year"),
      new MBusDevice.OrtoVifStruct("_PerCycle", 0, false, "/cycle"),
      new MBusDevice.OrtoVifStruct("_IncInp0", 0, false, ""),
      new MBusDevice.OrtoVifStruct("_IncInp1", 0, false, ""),
      new MBusDevice.OrtoVifStruct("_IncOut0", 0, false, ""),
      new MBusDevice.OrtoVifStruct("_IncOut1", 0, false, ""),
      new MBusDevice.OrtoVifStruct("_PerLiter", 0, false, "/l"),
      new MBusDevice.OrtoVifStruct("_PerQM", 0, false, "/m\u00B3"),
      new MBusDevice.OrtoVifStruct("_PerKg", 0, false, "/kg"),
      new MBusDevice.OrtoVifStruct("_PerK", 0, false, "/K"),
      new MBusDevice.OrtoVifStruct("_PerkWh", 0, false, "/kWh"),
      new MBusDevice.OrtoVifStruct("_PerGJ", 0, false, "/GJ"),
      new MBusDevice.OrtoVifStruct("_PerkW", 0, false, "/kW"),
      new MBusDevice.OrtoVifStruct("_PerK*l", 0, false, "/K*l"),
      new MBusDevice.OrtoVifStruct("_PerV", 0, false, "/V"),
      new MBusDevice.OrtoVifStruct("_PerA", 0, false, "/A"),
      new MBusDevice.OrtoVifStruct("_*s", 0, false, "*s"),
      new MBusDevice.OrtoVifStruct("_*s/V", 0, false, "*s/V"),
      new MBusDevice.OrtoVifStruct("_*s/A", 0, false, "*s/A"),
      new MBusDevice.OrtoVifStruct("_STime", 0, false, ""),
      new MBusDevice.OrtoVifStruct("_RAW", 0, false, ""),
      new MBusDevice.OrtoVifStruct("+", 0, false, ""),
      new MBusDevice.OrtoVifStruct("-", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x3d", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x3e", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x3f", 0, false, ""),
      new MBusDevice.OrtoVifStruct("_LowerLimit", 0, false, ""),
      new MBusDevice.OrtoVifStruct("_OfExceedsOfLower", 0, false, ""),
      new MBusDevice.OrtoVifStruct("_TimeOfBeginFirstLower", 0, true, ""),
      new MBusDevice.OrtoVifStruct("_TimeOfEndFirstLower", 0, true, ""),
      new MBusDevice.OrtoVifStruct("_VifEListOrto_0x44", 0, false, ""),
      new MBusDevice.OrtoVifStruct("_VifEListOrto_0x45", 0, false, ""),
      new MBusDevice.OrtoVifStruct("_TimeOfBeginLastLower", 0, true, ""),
      new MBusDevice.OrtoVifStruct("_TimeOfEndLastLower", 0, true, ""),
      new MBusDevice.OrtoVifStruct("_UpperLimit", 0, false, ""),
      new MBusDevice.OrtoVifStruct("_OfExceedsOfUpper", 0, false, ""),
      new MBusDevice.OrtoVifStruct("_TimeOfBeginFirstUpper", 0, true, ""),
      new MBusDevice.OrtoVifStruct("_TimeOfEndFirstUpper", 0, true, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x4c", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x4d", 0, false, ""),
      new MBusDevice.OrtoVifStruct("_TimeOfBeginLastUpper", 0, true, ""),
      new MBusDevice.OrtoVifStruct("_TimeOfEndLastUpper", 0, true, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x50", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x51", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x52", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x53", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x54", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x55", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x56", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x57", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x58", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x59", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x5a", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x5b", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x5c", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x5d", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x5e", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x5f", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x60", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x61", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x62", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x63", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x64", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x65", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x66", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x67", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x68", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x69", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x6a", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x6b", 0, false, ""),
      new MBusDevice.OrtoVifStruct("_Leakage", 0, false, ""),
      new MBusDevice.OrtoVifStruct("_Overflow", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x6e", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x6f", 0, false, ""),
      new MBusDevice.OrtoVifStruct("", -6, false, ""),
      new MBusDevice.OrtoVifStruct("", -5, false, ""),
      new MBusDevice.OrtoVifStruct("", -4, false, ""),
      new MBusDevice.OrtoVifStruct("", -3, false, ""),
      new MBusDevice.OrtoVifStruct("", -2, false, ""),
      new MBusDevice.OrtoVifStruct("", -1, false, ""),
      new MBusDevice.OrtoVifStruct("", 0, false, ""),
      new MBusDevice.OrtoVifStruct("", 1, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x78", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x79", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x7a", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x7b", 0, false, ""),
      new MBusDevice.OrtoVifStruct("??_VifEListOrto_0x7c", 0, false, ""),
      new MBusDevice.OrtoVifStruct("", 3, false, ""),
      new MBusDevice.OrtoVifStruct("_Future", 0, false, ""),
      new MBusDevice.OrtoVifStruct("_Man", 0, false, "")
    };
    private const int LongFrameStartLen = 4;
    private const int LongFrameHeaderLen = 19;
    private const int LongFrameStopLen = 2;
    private const int MaxFrameEndLen = 242;
    protected int FrameEndLen;
    private byte TempChecksum;
    public byte PrimaryDeviceAddress;
    public bool PrimaryAddressOk;
    public bool PrimaryAddressKnown;
    public bool IsSelectedOnBus;
    public string DeviceInfoText = string.Empty;
    public bool followingTelegrammAnnounced = false;
    public bool followingTelegrammTransmit_FCB_Odd = false;
    private MBusDevice.ParamScannerValues scannerValues = new MBusDevice.ParamScannerValues();

    public static string GetMeterTypeDescription(MBusDeviceType TheType)
    {
      return DeviceCollectorFunctions.SerialBusMessage.GetString(TheType.ToString()) ?? DeviceCollectorFunctions.SerialBusMessage.GetString(MBusDeviceType.UNKNOWN.ToString());
    }

    public MBusDevice()
    {
      this.DeviceType = DeviceTypes.MBus;
      this.PrimaryAddressOk = false;
      this.PrimaryAddressKnown = false;
      this.IsSelectedOnBus = false;
    }

    public MBusDevice(DeviceCollectorFunctions TheBus)
      : base(TheBus)
    {
      this.DeviceType = DeviceTypes.MBus;
      this.PrimaryAddressOk = false;
      this.PrimaryAddressKnown = false;
      this.IsSelectedOnBus = false;
    }

    public byte GetPrimaryAddress()
    {
      this.GarantAddressingPossible();
      if (this.MyBus.MyBusMode == BusMode.MBusPointToPoint || this.MyBus.MyBusMode == BusMode.SmokeDetector)
        return 254;
      if (!this.MyBus.OnlySecondaryAddressing && this.PrimaryAddressOk)
        return this.PrimaryDeviceAddress;
      if (this.IsSelectedOnBus)
        return 253;
      MBusDevice.MBusDeviceLogger.Error("MBus telegram generation without valid address.");
      return 251;
    }

    private bool AddressingPossible()
    {
      return this.MyBus.MyBusMode == BusMode.MBusPointToPoint || this.MyBus.MyBusMode == BusMode.SmokeDetector || !this.MyBus.OnlySecondaryAddressing && this.PrimaryAddressOk || this.IsSelectedOnBus;
    }

    public void GenerateREQ_UD2()
    {
      this.TransmitBuffer = new ByteField(5);
      byte primaryAddress = this.GetPrimaryAddress();
      MBusDevice.REQ_UD2_Type reqUd2Type = this.MyBus.UseREQ_UD2_5B ? MBusDevice.REQ_UD2_Type.REQ_UD2_5B : MBusDevice.REQ_UD2_Type.REQ_UD2_7B;
      if (this.MyBus.IsMultiTelegrammEnabled)
        reqUd2Type = !this.followingTelegrammTransmit_FCB_Odd ? MBusDevice.REQ_UD2_Type.REQ_UD2_5B : MBusDevice.REQ_UD2_Type.REQ_UD2_7B;
      this.TransmitBuffer.Add(16);
      this.TransmitBuffer.Add((byte) reqUd2Type);
      this.TransmitBuffer.Add(primaryAddress);
      this.TransmitBuffer.Add((byte) (reqUd2Type + primaryAddress));
      this.TransmitBuffer.Add(22);
    }

    internal void GenerateLongframeStart()
    {
      this.TransmitBuffer = new ByteField(270);
      this.TransmitBuffer.Add(104);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(104);
    }

    public void GenerateSendDataHeader()
    {
      byte primaryAddress = this.GetPrimaryAddress();
      this.GenerateLongframeStart();
      this.TransmitBuffer.Add(83);
      this.TransmitBuffer.Add(primaryAddress);
      this.TransmitBuffer.Add(81);
    }

    public void FinishLongFrame()
    {
      byte num = (byte) (this.TransmitBuffer.Count - 4);
      this.TransmitBuffer.Data[1] = num;
      this.TransmitBuffer.Data[2] = num;
      byte Byte = 0;
      for (int index = 4; index < this.TransmitBuffer.Count; ++index)
        Byte += this.TransmitBuffer.Data[index];
      this.TransmitBuffer.Add(Byte);
      this.TransmitBuffer.Add(22);
    }

    internal bool ReceiveLongframeStart()
    {
      try
      {
        this.MyBus.TempDeviceInfo = new DeviceInfo();
        this.ReceiveBuffer = new ByteField(4);
        MBusDevice.MBusDeviceLogger.Debug("Receive longframe");
        if (!this.MyBus.MyCom.ReceiveBlock(ref this.ReceiveBuffer, 4, true))
        {
          MBusDevice.MBusDeviceLogger.Error("... Error on receive longframe header");
          return false;
        }
        MBusDevice.MBusDeviceLogger.Debug("... Longframe bytes received");
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusWorkHeader);
        if (this.ReceiveBuffer.Data[0] == (byte) 104 && this.ReceiveBuffer.Data[3] == (byte) 104)
        {
          int num = (int) this.ReceiveBuffer.Data[1];
          if ((int) this.ReceiveBuffer.Data[2] == num)
          {
            this.FrameEndLen = num + 2;
            this.TempChecksum = (byte) 0;
            return true;
          }
        }
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.FramingError, "Received error data.");
        ++this.MyBus.BusState.TotalErrorCounter;
        return false;
      }
      catch (TimeoutException ex)
      {
        return false;
      }
      catch (FramingErrorException ex)
      {
        return false;
      }
    }

    internal bool ReceiveHeader()
    {
      try
      {
        if (!this.MyBus.MyCom.IsOpen)
          return false;
        this.MyBus.TempDeviceInfo = new DeviceInfo();
        this.TotalReceiveBuffer.Clear();
        this.ReceiveBuffer = new ByteField(19);
        MBusDevice.MBusDeviceLogger.Trace("Start receive header");
        if (!this.MyBus.MyCom.ReceiveBlock(ref this.ReceiveBuffer, 19, true))
        {
          ZR_ClassLibMessages.LastErrors lastError = ZR_ClassLibMessages.GetLastError();
          MBusDevice.MBusDeviceLogger.Warn("Failed receive M-Bus header! Reason: " + lastError.ToString());
          this.MyBus.MyCom.ClearWakeup();
          ++this.MyBus.BusState.TotalErrorCounter;
          return false;
        }
        this.TotalReceiveBuffer.AddRange((IEnumerable<byte>) this.ReceiveBuffer.Data);
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusWorkHeader);
        if (this.ReceiveBuffer.Data[0] == (byte) 104 && this.ReceiveBuffer.Data[3] == (byte) 104)
        {
          int num = (int) this.ReceiveBuffer.Data[1];
          if (num != 0 && (int) this.ReceiveBuffer.Data[2] == num)
          {
            this.MyBus.TempDeviceInfo.C_Field = this.ReceiveBuffer.Data[4];
            this.MyBus.TempDeviceInfo.A_Field = this.ReceiveBuffer.Data[5];
            this.MyBus.TempDeviceInfo.CI_Field = this.ReceiveBuffer.Data[6];
            long InValue = (long) this.ReceiveBuffer.Data[7] + ((long) this.ReceiveBuffer.Data[8] << 8) + ((long) this.ReceiveBuffer.Data[9] << 16) + ((long) this.ReceiveBuffer.Data[10] << 24);
            this.MyBus.TempDeviceInfo.MeterNumberOriginal = Convert.ToUInt32(InValue);
            this.MyBus.TempDeviceInfo.MeterNumber = MBusDevice.TranslateBcdToBin(InValue).ToString();
            this.MyBus.TempDeviceInfo.ManufacturerCode = (short) this.ReceiveBuffer.Data[11];
            this.MyBus.TempDeviceInfo.ManufacturerCode += (short) ((int) this.ReceiveBuffer.Data[12] << 8);
            this.MyBus.TempDeviceInfo.Manufacturer = MBusDevice.GetManufacturer(this.MyBus.TempDeviceInfo.ManufacturerCode);
            this.MyBus.TempDeviceInfo.Version = this.ReceiveBuffer.Data[13];
            this.MyBus.TempDeviceInfo.Medium = this.ReceiveBuffer.Data[14];
            this.MyBus.TempDeviceInfo.AccessNb = this.ReceiveBuffer.Data[15];
            this.MyBus.TempDeviceInfo.Status = this.ReceiveBuffer.Data[16];
            this.MyBus.TempDeviceInfo.Signature = (int) this.ReceiveBuffer.Data[17];
            this.MyBus.TempDeviceInfo.Signature += (int) this.ReceiveBuffer.Data[18] << 8;
            this.TempChecksum = (byte) 0;
            for (int index = 4; index < 19; ++index)
              this.TempChecksum += this.ReceiveBuffer.Data[index];
            this.FrameEndLen = num - 19 + 4 + 2;
            return true;
          }
        }
        string str = "Invalid M-Bus header! Buffer: " + ZR_ClassLibrary.Util.ByteArrayToHexString(this.ReceiveBuffer.Data);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, str);
        ++this.MyBus.BusState.TotalErrorCounter;
        MBusDevice.MBusDeviceLogger.Error(str);
        return false;
      }
      catch (TimeoutException ex)
      {
        return false;
      }
      catch (FramingErrorException ex)
      {
        return false;
      }
    }

    internal bool ReceiveLongframeEnd() => this.ReceiveLongframeEnd(ParameterService.GetNow());

    internal bool ReceiveLongframeEnd(DateTime LastReadingDate)
    {
      try
      {
        if (this.FrameEndLen <= 0)
          return false;
        MBusDevice.MBusDeviceLogger.Trace("Start receive longframe end. It is expected {0} bytes", this.FrameEndLen);
        this.ReceiveBuffer = new ByteField(this.FrameEndLen);
        if (this.FrameEndLen < 2 || this.FrameEndLen > 242)
        {
          string str = "Illegal mbus length. Value: " + this.FrameEndLen.ToString();
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, str);
          this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusReceiveDataError);
          MBusDevice.MBusDeviceLogger.Error(str);
        }
        else if (this.MyBus.MyCom.ReceiveBlock(ref this.ReceiveBuffer, this.FrameEndLen, true))
        {
          this.TotalReceiveBuffer.AddRange((IEnumerable<byte>) this.ReceiveBuffer.Data);
          this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusWorkData);
          for (int index = 0; index < this.FrameEndLen - 2; ++index)
            this.TempChecksum += this.ReceiveBuffer.Data[index];
          if ((int) this.TempChecksum != (int) this.ReceiveBuffer.Data[this.FrameEndLen - 2])
          {
            MBusDevice.MBusDeviceLogger.Error("Checksum error!");
            this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusChecksumError);
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "MBus checksum error");
          }
          else
          {
            this.MyBus.TempDeviceInfo.DeviceType = this.Info.DeviceType;
            this.MyBus.TempDeviceInfo.LastReadingDate = LastReadingDate;
            this.Info = this.MyBus.TempDeviceInfo;
            this.MyBus.BusState.IncrementReceiveBlockCounter();
            this.MyBus.MyCom.ResetEarliestTransmitTime();
            return true;
          }
        }
        ZR_ClassLibMessages.LastErrors lastError = ZR_ClassLibMessages.GetLastError();
        MBusDevice.MBusDeviceLogger.Error("Failed to receive end of M-Bus long frame! Reason: " + lastError.ToString());
        if (MBusDevice.MBusDeviceLogger.IsTraceEnabled)
        {
          MBusDevice.MBusDeviceLogger.Trace("Received data: {0}", ZR_ClassLibrary.Util.ByteArrayToHexString(this.ReceiveBuffer.Data, 0, this.ReceiveBuffer.Count));
          MBusDevice.MBusDeviceLogger.Trace("Received size: {0}", this.ReceiveBuffer.Count);
          MBusDevice.MBusDeviceLogger.Trace("Required size: {0}", this.FrameEndLen);
        }
        ++this.MyBus.BusState.TotalErrorCounter;
        return false;
      }
      catch (TimeoutException ex)
      {
        return false;
      }
      catch (FramingErrorException ex)
      {
        return false;
      }
    }

    internal bool DeselectDevice()
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.SEND_NKE);
      ByteField DataBlock = new ByteField(5);
      DataBlock.Add(16);
      DataBlock.Add(64);
      DataBlock.Add(253);
      DataBlock.Add((byte) 61);
      DataBlock.Add(22);
      if (!this.MyBus.MyCom.Open())
        return false;
      int maxRequestRepeat = this.MyBus.MaxRequestRepeat;
      while (maxRequestRepeat > 0 && !this.MyBus.BreakRequest)
      {
        MBusDevice.MBusDeviceLogger.Info<int, int>("Try deselect M-Bus device. Repeats {0} of {1}", this.MyBus.MaxRequestRepeat - maxRequestRepeat + 1, this.MyBus.MaxRequestRepeat);
        this.MyBus.MyCom.WaitToEarliestTransmitTime();
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusSendSND_NKE);
        if (MBusDevice.MBusDeviceLogger.IsTraceEnabled)
          MBusDevice.MBusDeviceLogger.Trace<string, int>("DESELECT M-BUS DEVICE : {0}, Size: {1}", ZR_ClassLibrary.Util.ByteArrayToHexString(DataBlock.Data, 0, DataBlock.Count), DataBlock.Count);
        this.MyBus.MyCom.TransmitBlock(ref DataBlock);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (this.ReceiveOkNok())
        {
          if (this.TryDisposeDataFromInputBufferAndKeepErrorMessage())
          {
            MBusDevice.MBusDeviceLogger.Warn("Failed deselected the device! Unexpected data received!");
            ZR_ClassLibMessages.ClearErrors();
            return false;
          }
          MBusDevice.MBusDeviceLogger.Info("Successfully deselected the device!");
          ZR_ClassLibMessages.ClearErrors();
          return true;
        }
        ZR_ClassLibMessages.LastErrors lastError = ZR_ClassLibMessages.GetLastError();
        MBusDevice.MBusDeviceLogger.Warn("Failed deselected the device! Reason: " + lastError.ToString());
        switch (lastError)
        {
          case ZR_ClassLibMessages.LastErrors.NoError:
          case ZR_ClassLibMessages.LastErrors.Timeout:
            ZR_ClassLibMessages.ClearErrors();
            return true;
          case ZR_ClassLibMessages.LastErrors.FramingError:
          case ZR_ClassLibMessages.LastErrors.IllegalData:
            this.TryDisposeDataFromInputBufferAndKeepErrorMessage();
            --maxRequestRepeat;
            if (MBusDevice.MBusDeviceLogger.IsDebugEnabled)
              MBusDevice.MBusDeviceLogger.Debug("Application.DoEvents() -> DeselectDevice");
            Application.DoEvents();
            continue;
          default:
            return false;
        }
      }
      ZR_ClassLibMessages.ClearErrors();
      return true;
    }

    internal bool ReceiveOkNok()
    {
      try
      {
        ByteField DataBlock = new ByteField(1);
        ZR_ClassLibMessages.ClearErrors();
        if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
        {
          MBusDevice.MBusDeviceLogger.Trace("NACK (Timeout or frame error)");
          ++this.MyBus.BusState.TotalErrorCounter;
          return false;
        }
        if (DataBlock.Data[0] == (byte) 229)
        {
          this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusReceiveOK);
          MBusDevice.MBusDeviceLogger.Trace("ACK received Buffer: E5");
          this.MyBus.BusState.IncrementReceiveBlockCounter();
          this.MyBus.MyCom.ResetEarliestTransmitTime();
          return true;
        }
        if (DataBlock.Data[0] == (byte) 26)
        {
          this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusReceiveNOK);
          MBusDevice.MBusDeviceLogger.Trace("NACK received Buffer: 1A");
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NAK_Received);
          ++this.MyBus.BusState.TotalErrorCounter;
          return false;
        }
        MBusDevice.MBusDeviceLogger.Trace("NACK (Data received but not ACK) Buffer: " + DataBlock.Data[0].ToString("X02"));
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "OK expectad but not received");
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusReceiveNOK);
        ++this.MyBus.BusState.TotalErrorCounter;
        return false;
      }
      catch (TimeoutException ex)
      {
        return false;
      }
      catch (FramingErrorException ex)
      {
        return false;
      }
    }

    public static string GetManufacturer(short ManufacturerCode)
    {
      return "" + ((char) (((int) ManufacturerCode >> 10 & 31) + 64)).ToString() + ((char) (((int) ManufacturerCode >> 5 & 31) + 64)).ToString() + ((char) (((int) ManufacturerCode & 31) + 64)).ToString();
    }

    public static ushort GetManufacturerCode(string Manufacturer)
    {
      return (ushort) ((uint) (ushort) ((uint) (ushort) (0U + (uint) (ushort) ((uint) (byte) Manufacturer[2] - 64U)) + (uint) (ushort) ((uint) (ushort) ((uint) (byte) Manufacturer[1] - 64U) << 5)) + (uint) (ushort) ((uint) (ushort) ((uint) (byte) Manufacturer[0] - 64U) << 10));
    }

    internal static bool StringToMBusSerialNumber(string SerialNumberIn, out uint SerialNumberOut)
    {
      SerialNumberIn.Trim();
      SerialNumberIn = SerialNumberIn.ToUpper();
      SerialNumberOut = 0U;
      while (SerialNumberIn.Length > 0)
      {
        SerialNumberOut <<= 4;
        uint num = (uint) SerialNumberIn[0] - 48U;
        if (num < 0U || num > 9U)
        {
          if (num != 22U)
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal serial number for secundare addressing", MBusDevice.MBusDeviceLogger);
          num = 15U;
        }
        SerialNumberIn = SerialNumberIn.Remove(0, 1);
        SerialNumberOut += num;
      }
      return true;
    }

    public bool SerchBaudrate()
    {
      ArrayList ParameterList = new ArrayList();
      this.MyBus.MyCom.GetCommParameter(ref ParameterList);
      int index;
      for (index = 0; index < ParameterList.Count; index += 2)
      {
        if ((string) ParameterList[index] == "Baudrate")
        {
          ++index;
          break;
        }
      }
      if ((index & 1) != 1)
        return false;
      string str = (string) ParameterList[index];
      int maxRequestRepeat = this.MyBus.MaxRequestRepeat;
      this.MyBus.MaxRequestRepeat = 1;
      bool flag = this.REQ_UD2();
      if (!flag)
      {
        ParameterList[index] = !(str == "2400") ? (object) "2400" : (object) "9600";
        this.MyBus.MyCom.SetCommParameter(ParameterList);
        flag = this.REQ_UD2();
      }
      this.MyBus.MaxRequestRepeat = maxRequestRepeat;
      if (flag)
        return true;
      ParameterList[index] = (object) str;
      this.MyBus.MyCom.SetCommParameter(ParameterList);
      return false;
    }

    internal bool GarantAddressingPossible()
    {
      if (!this.AddressingPossible())
      {
        uint SerialNumberOut;
        if (!MBusDevice.StringToMBusSerialNumber(this.Info.MeterNumber, out SerialNumberOut))
          return false;
        if (!this.MyBus.FastSecondaryAddressing)
          this.DeselectDevice();
        if (!this.SelectDeviceOnBus(SerialNumberOut, ushort.MaxValue, byte.MaxValue, byte.MaxValue))
          return false;
      }
      return true;
    }

    public bool REQ_UD2() => this.REQ_UD2(ParameterService.GetNow());

    public bool REQ_UD2(DateTime timePoint)
    {
      if (!UserManager.CheckPermission(UserRights.Rights.MBus))
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.NoPermission, "No permission for M-Bus!");
        return false;
      }
      if (this.MyBus.MyCom.Open())
      {
        this.Info.LastReadingDate = timePoint;
        if (this.GarantAddressingPossible())
        {
          this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.REQ_UD2);
          this.GenerateREQ_UD2();
          for (int maxRequestRepeat = this.MyBus.MaxRequestRepeat; maxRequestRepeat > 0 && !this.MyBus.BreakRequest; --maxRequestRepeat)
          {
            MBusDevice.MBusDeviceLogger.Info<int, int>("Try REQ_UD2. Repeats {0} of {1}", this.MyBus.MaxRequestRepeat - maxRequestRepeat + 1, this.MyBus.MaxRequestRepeat);
            this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusSendREQ_UD2);
            if (MBusDevice.MBusDeviceLogger.IsTraceEnabled)
              MBusDevice.MBusDeviceLogger.Trace("REQ_UD2: {0}", ZR_ClassLibrary.Util.ByteArrayToHexString(this.TransmitBuffer.Data, 0, this.TransmitBuffer.Count));
            this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
            this.MyBus.BusState.IncrementTransmitBlockCounter();
            if (this.ReceiveHeader())
            {
              if (this.ReceiveLongframeEnd(timePoint))
              {
                this.GenerateParameterList(true);
                MBusDevice.MBusDeviceLogger.Info("M-Bus device found! -------------------------------------> Serialnumber: {0}", this.Info.MeterNumber);
                this.Info.ParameterOk = true;
                ZR_ClassLibMessages.ClearErrorText();
                if (this.Info.Manufacturer == "ZRI" && this.Info.Version == (byte) 2)
                  this.DeviceType = DeviceTypes.EDC;
                return true;
              }
              this.TryDisposeDataFromInputBufferAndKeepErrorMessage();
            }
            else
              this.TryDisposeDataFromInputBufferAndKeepErrorMessage();
          }
        }
      }
      this.Info.ParameterOk = false;
      this.GenerateParameterList(false);
      return false;
    }

    private bool TryDisposeDataFromInputBufferAndKeepErrorMessage()
    {
      ZR_ClassLibMessages.LastErrors lastError = ZR_ClassLibMessages.GetLastError();
      if (lastError != ZR_ClassLibMessages.LastErrors.FramingError && lastError != ZR_ClassLibMessages.LastErrors.IllegalData)
        return false;
      ByteField DataBlock = new ByteField(1);
      if (this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
      {
        byte[] buffer;
        this.MyBus.MyCom.TryReceiveBlock(out buffer);
        string message = "Dispose data from input buffer! Buffer: " + DataBlock.Data[0].ToString("X2") + ZR_ClassLibrary.Util.ByteArrayToHexString(buffer);
        MBusDevice.MBusDeviceLogger.Error(message);
      }
      ZR_ClassLibMessages.AddErrorDescription(lastError);
      return true;
    }

    public bool MeterApplicationReset()
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.ApplicationReset);
      this.TransmitBuffer = new ByteField(270);
      byte primaryAddress = this.GetPrimaryAddress();
      this.TransmitBuffer.Add(104);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(104);
      this.TransmitBuffer.Add(83);
      this.TransmitBuffer.Add(primaryAddress);
      this.TransmitBuffer.Add(80);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        MBusDevice.MBusDeviceLogger.Debug("Send application reset");
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusApplicationReset);
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (this.ReceiveOkNok())
        {
          ZR_ClassLibMessages.ClearErrorText();
          return true;
        }
      }
      return false;
    }

    public bool SelectAllParameter()
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.SelectAllParameter);
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add((int) sbyte.MaxValue);
      this.TransmitBuffer.Add(126);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        MBusDevice.MBusDeviceLogger.Debug("Select all parameter");
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusSetAllParameters);
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (this.ReceiveOkNok())
        {
          ZR_ClassLibMessages.ClearErrorText();
          return true;
        }
      }
      return false;
    }

    internal override bool WriteDueDateMonth(ushort month)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.WriteDueDateMonth);
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add((int) month);
      this.TransmitBuffer.Add(33);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        MBusDevice.MBusDeviceLogger.Debug("Write DueDateMonth");
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.WriteDueDateMonth);
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (this.ReceiveOkNok())
        {
          ZR_ClassLibMessages.ClearErrorText();
          return true;
        }
        if (ZR_ClassLibMessages.GetLastError() != ZR_ClassLibMessages.LastErrors.NAK_Received)
          ;
      }
      return false;
    }

    internal override bool SelectParameterList(int ListNumber, int function)
    {
      return this.SelectParameterListWork(ListNumber, function);
    }

    internal bool SelectParameterListWork(int ListNumber, int function)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.SelectParameterList);
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(ParameterListInfo.GetCommandPayload((ushort) ListNumber, function > 0));
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        MBusDevice.MBusDeviceLogger.Debug("Select parameter list");
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusSetAllParameters);
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (this.ReceiveOkNok())
        {
          ZR_ClassLibMessages.ClearErrorText();
          return true;
        }
        if (ZR_ClassLibMessages.GetLastError() != ZR_ClassLibMessages.LastErrors.NAK_Received)
          ;
      }
      return false;
    }

    public bool SetBaudrate(int Baudrate)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.SetBaudrate);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      switch (Baudrate)
      {
        case 300:
          this.TransmitBuffer.Data[6] = (byte) 184;
          break;
        case 600:
          this.TransmitBuffer.Data[6] = (byte) 185;
          break;
        case 1200:
          this.TransmitBuffer.Data[6] = (byte) 186;
          break;
        case 2400:
          this.TransmitBuffer.Data[6] = (byte) 187;
          break;
        case 4800:
          this.TransmitBuffer.Data[6] = (byte) 188;
          break;
        case 9600:
          this.TransmitBuffer.Data[6] = (byte) 189;
          break;
        case 19200:
          this.TransmitBuffer.Data[6] = (byte) 190;
          break;
        case 38400:
          this.TransmitBuffer.Data[6] = (byte) 191;
          break;
        default:
          return false;
      }
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        MBusDevice.MBusDeviceLogger.Debug("Set baudrate");
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusSendREQ_UD2);
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (this.ReceiveOkNok())
        {
          ZR_ClassLibMessages.ClearErrorText();
          return true;
        }
      }
      return false;
    }

    public bool SelectDeviceOnBus(
      uint BCD_SerialNr,
      ushort Manufacturer,
      byte Generation,
      byte Medium)
    {
      MBusDevice mbusDevice = this;
      for (int index = 0; index < this.MyBus.MyDeviceList.bus.Count; ++index)
      {
        MBusDevice bu = (MBusDevice) this.MyBus.MyDeviceList.bus[index];
        if (bu.IsSelectedOnBus)
        {
          mbusDevice = bu;
          bu.IsSelectedOnBus = false;
        }
      }
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.SelectDevice);
      this.TransmitBuffer = new ByteField(270);
      this.TransmitBuffer.Add(104);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(104);
      this.TransmitBuffer.Add(83);
      this.TransmitBuffer.Add(253);
      this.TransmitBuffer.Add(82);
      this.TransmitBuffer.Add((int) (byte) BCD_SerialNr & (int) byte.MaxValue);
      this.TransmitBuffer.Add((int) (byte) (BCD_SerialNr >> 8) & (int) byte.MaxValue);
      this.TransmitBuffer.Add((int) (byte) (BCD_SerialNr >> 16) & (int) byte.MaxValue);
      this.TransmitBuffer.Add((int) (byte) (BCD_SerialNr >> 24) & (int) byte.MaxValue);
      this.TransmitBuffer.Add((int) (byte) Manufacturer & (int) byte.MaxValue);
      this.TransmitBuffer.Add((int) (byte) ((uint) Manufacturer >> 8) & (int) byte.MaxValue);
      this.TransmitBuffer.Add(Generation);
      this.TransmitBuffer.Add(Medium);
      this.FinishLongFrame();
      for (int maxRequestRepeat = this.MyBus.MaxRequestRepeat; maxRequestRepeat > 0 && !this.MyBus.BreakRequest; --maxRequestRepeat)
      {
        MBusDevice.MBusDeviceLogger.Info<int, int>("Try select device. Repeats {0} of {1}", this.MyBus.MaxRequestRepeat - maxRequestRepeat + 1, this.MyBus.MaxRequestRepeat);
        if (!this.MyBus.MyCom.Open())
          return false;
        this.MyBus.MyCom.WaitToEarliestTransmitTime();
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusSelectDevice);
        if (MBusDevice.MBusDeviceLogger.IsTraceEnabled)
          MBusDevice.MBusDeviceLogger.Trace("SELECT MBUS DEVICE: {0}", ZR_ClassLibrary.Util.ByteArrayToHexString(this.TransmitBuffer.Data, 0, this.TransmitBuffer.Count));
        if (maxRequestRepeat != this.MyBus.MaxRequestRepeat)
          this.MyBus.SendMessage(new GMM_EventArgs(string.Format("{0} Attempt {1} of {2}", (object) BCD_SerialNr.ToString("X08"), (object) (this.MyBus.MaxRequestRepeat - maxRequestRepeat), (object) this.MyBus.MaxRequestRepeat)));
        if (!this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer))
          return false;
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (this.ReceiveOkNok())
        {
          if (this.MyBus.BusState.GlobalFunctionTask != BusStatusClass.GlobalFunctionTasks.ScanSecundary)
          {
            ZR_ClassLibMessages.ClearErrors();
            this.IsSelectedOnBus = true;
            return true;
          }
          ByteField DataBlock = new ByteField(1);
          if (this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
          {
            MBusDevice.MBusDeviceLogger.Error("Disposed a byte after selection! Byte: 0x{0}", DataBlock.Data[0].ToString("X2"));
            this.TryDisposeDataFromInputBufferAndKeepErrorMessage();
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData);
            this.IsSelectedOnBus = false;
            return false;
          }
          switch (ZR_ClassLibMessages.GetLastError())
          {
            case ZR_ClassLibMessages.LastErrors.NoError:
            case ZR_ClassLibMessages.LastErrors.Timeout:
              ZR_ClassLibMessages.ClearErrors();
              this.IsSelectedOnBus = true;
              MBusDevice.MBusDeviceLogger.Info("Successfull selected the device!");
              return true;
            default:
              this.IsSelectedOnBus = false;
              this.TryDisposeDataFromInputBufferAndKeepErrorMessage();
              return false;
          }
        }
        else
        {
          this.IsSelectedOnBus = false;
          ZR_ClassLibMessages.LastErrors lastError = ZR_ClassLibMessages.GetLastError();
          MBusDevice.MBusDeviceLogger.Warn("Failed select the device on bus! Reason: " + lastError.ToString());
          if (lastError != ZR_ClassLibMessages.LastErrors.Timeout)
          {
            this.IsSelectedOnBus = false;
            this.TryDisposeDataFromInputBufferAndKeepErrorMessage();
            bool flag1 = BCD_SerialNr.ToString("X08").StartsWith("F");
            bool flag2 = BCD_SerialNr.ToString("X08").EndsWith("F");
            if (flag1 || flag2)
              return false;
          }
        }
      }
      return this.IsSelectedOnBus;
    }

    public bool SetPrimaryAddress(int PrimaryAddress)
    {
      if (!this.MyBus.MyCom.Open() || !this.GarantAddressingPossible())
        return false;
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.SetPrimaryAddress);
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(1);
      this.TransmitBuffer.Add(122);
      this.TransmitBuffer.Add(PrimaryAddress);
      this.FinishLongFrame();
      this.MyBus.MyCom.SetAnswerOffsetTime(500);
      bool flag = false;
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        MBusDevice.MBusDeviceLogger.Debug("Select primary address.");
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusSendREQ_UD2);
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (this.ReceiveOkNok())
        {
          this.PrimaryAddressKnown = true;
          this.PrimaryDeviceAddress = (byte) PrimaryAddress;
          this.Info.EraseParameter("RADR");
          this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("RADR", this.PrimaryDeviceAddress.ToString()));
          flag = true;
          ZR_ClassLibMessages.ClearErrorText();
          break;
        }
      }
      this.MyBus.MyCom.SetAnswerOffsetTime(0);
      return flag;
    }

    public static bool GetZR_MBusLoggerDivVif(ref long DifVif, ref short DivVifSize)
    {
      byte num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      long num5 = 0;
      bool flag1 = true;
      bool flag2 = true;
      byte num6 = 1;
      byte num7 = 0;
      byte num8 = 0;
      byte num9 = 0;
      if (DivVifSize < (short) 2)
        return false;
      while (DivVifSize > (short) 0)
      {
        byte num10 = (byte) ((ulong) DifVif & (ulong) byte.MaxValue);
        if (flag2)
        {
          flag2 = false;
          if (((int) num10 & 128) == 0)
            flag1 = false;
          if (((int) num10 & 64) == 64)
            num2 = 1;
          num1 = (byte) ((uint) num10 & 63U);
        }
        else if (flag1)
        {
          if (((int) num10 & 128) == 0)
            flag1 = false;
          int num11 = ((int) num10 & 15) << (int) num6;
          byte num12 = (byte) ((uint) num6 + 4U);
          num2 |= num11;
          int num13 = ((int) num10 & 48) >> 4 << (int) num7;
          num6 = (byte) ((uint) num12 + 2U);
          num3 |= num13;
          int num14 = ((int) num10 & 48) >> 6 << (int) num8;
          ++num8;
          num4 |= num14;
        }
        else
        {
          num5 = DifVif;
          num9 = (byte) DivVifSize;
          break;
        }
        DifVif >>= 8;
        --DivVifSize;
      }
      if (num2 > 2 || num4 > 0 || num5 == 122L || num5 == 121L)
        return false;
      int num15 = num2;
      int num16 = 32;
      byte num17 = 0;
      DifVif = 0L;
      DivVifSize = (short) 0;
      while (true)
      {
        if (num17 == (byte) 0)
        {
          DifVif = (long) (128 | (int) num1);
          num17 = (byte) 1;
          num16 >>= 1;
          num5 <<= 8;
          ++DivVifSize;
        }
        else if (num15 > 0 || num16 > 0 || num3 > 0)
        {
          long num18 = (long) (num16 & 15 | (num3 & 3) << 4 | (num15 & 1) << 6);
          num16 >>= 4;
          num3 >>= 1;
          num15 >>= 1;
          if (num15 > 0 || num16 > 0 || num3 > 0)
            num18 |= 128L;
          DifVif |= num18 << (int) DivVifSize * 8;
          num5 <<= 8;
          ++DivVifSize;
        }
        else
          break;
      }
      DifVif |= num5;
      DivVifSize += (short) num9;
      return true;
    }

    public static bool GetZR_MBusParameterID(
      long DifVifs,
      short DifVifSize,
      out string ZR_MBusParameterID,
      out string ParamUnit,
      out int UnitExponent)
    {
      byte[] DifVifList = new byte[(int) DifVifSize];
      for (int index = 0; index < (int) DifVifSize; ++index)
      {
        DifVifList[index] = (byte) ((ulong) DifVifs & (ulong) byte.MaxValue);
        DifVifs >>= 8;
      }
      return MBusDevice.GetZR_MBusParameterID(DifVifList, out ZR_MBusParameterID, out ParamUnit, out UnitExponent);
    }

    public static string GetZR_MBusParameterID(byte[] DifVifList)
    {
      if (DifVifList == null || DifVifList.Length == 0)
        return string.Empty;
      string ZR_MBusParameterID;
      MBusDevice.GetZR_MBusParameterID(DifVifList, out ZR_MBusParameterID, out string _, out int _);
      return ZR_MBusParameterID;
    }

    private static bool GetZR_MBusParameterID(
      byte[] DifVifList,
      out string ZR_MBusParameterID,
      out string ParamUnit,
      out int UnitExponent)
    {
      MBusDevice.ParamScannerValues s = new MBusDevice.ParamScannerValues();
      s.readBuffer = DifVifList;
      s.usedBytesInBuffer = DifVifList.Length;
      s.Clear();
      bool zrMbusParameterId = MBusDevice.ScanDif(ref s);
      if (zrMbusParameterId && !s.NoVIF)
        zrMbusParameterId = MBusDevice.ScanVif(ref s);
      if (s.paramError)
        s.ZDF_StringBuilder.Append("_ParamErr");
      ZR_MBusParameterID = s.ZDF_String;
      ParamUnit = s.unitText;
      UnitExponent = s.unitExponent;
      return zrMbusParameterId;
    }

    internal bool GenerateParameterList(bool OkList)
    {
      bool parameterList = false;
      this.Info.ParameterList.Clear();
      this.Info.ParameterListWithoutValues.Clear();
      this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("RTIME", this.Info.LastReadingDate.ToString(FixedFormates.TheFormates.DateTimeFormat.FullDateTimePattern)));
      this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("SID", this.Info.MeterNumber));
      this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("MAN", this.Info.Manufacturer));
      this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("GEN", this.Info.Version.ToString()));
      this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("MED", this.Info.MediumString));
      this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("RADR", this.Info.A_Field.ToString()));
      if (!OkList)
      {
        this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct("R_ERR", this.Info.A_Field.ToString()));
      }
      else
      {
        this.followingTelegrammAnnounced = false;
        this.scannerValues.readBuffer = this.ReceiveBuffer.Data;
        this.scannerValues.usedBytesInBuffer = this.ReceiveBuffer.Count - 2;
        this.scannerValues.followingTelegrammAnnounced = false;
        this.scannerValues.breakParameterLoop = false;
        this.scannerValues.readBufferOffset = 0;
        while (this.scannerValues.readBufferOffset < this.scannerValues.usedBytesInBuffer && !this.scannerValues.breakParameterLoop)
        {
          if (this.scannerValues.readBuffer[this.scannerValues.readBufferOffset] == (byte) 47)
            ++this.scannerValues.readBufferOffset;
          else if (MBusDevice.ScanMBusParameter(ref this.scannerValues, this.Info.ParameterList))
          {
            if (this.scannerValues.ZDF_StringBuilder.Length > 0)
            {
              if (this.scannerValues.isType)
                this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct(this.scannerValues.ZDF_String, MBusDevice.GetMediaString(byte.Parse(this.scannerValues.PValue.ToString()))));
              else
                this.Info.ParameterList.Add(new DeviceInfo.MBusParamStruct(this.scannerValues.ZDF_String, this.scannerValues.PValue.ToString()));
              this.Info.ParameterListWithoutValues.Add(this.scannerValues.ZDF_String.Replace("_ERR", string.Empty));
            }
          }
          else
            break;
        }
        if (this.scannerValues.followingTelegrammAnnounced)
          this.followingTelegrammAnnounced = true;
        parameterList = true;
      }
      return parameterList;
    }

    internal static bool ScanMBusParameter(
      ref MBusDevice.ParamScannerValues s,
      List<DeviceInfo.MBusParamStruct> parameter)
    {
      s.Clear();
      bool flag = MBusDevice.ScanDif(ref s);
      if (flag && !s.NoVIF)
        flag = MBusDevice.ScanVif(ref s);
      if (flag && !s.NoValue)
        MBusDevice.ScanValue(ref s, parameter);
      int num = -1;
      if (s.paramError)
      {
        s.ZDF_StringBuilder.Append("_ParamErr");
        num = s.readBufferStartOffset;
      }
      if (s.addAdditionalData)
        num = s.readBufferOffset;
      if (num >= 0)
      {
        s.PValue.Length = 0;
        s.readBufferOffset = num;
        while (s.readBufferOffset < s.usedBytesInBuffer)
        {
          if (!s.GetNextByte())
            return false;
          s.PValue.Append(s.actualByte.ToString("x02") + " ");
        }
      }
      return true;
    }

    private static bool ScanDif(ref MBusDevice.ParamScannerValues s)
    {
      int num = 0;
      s.readBufferStartOffset = s.readBufferOffset;
      while (s.GetNextByte())
      {
        if (num == 0)
        {
          if (s.actualByte == (byte) 15 || s.actualByte == (byte) 31)
          {
            if (s.readBufferOffset < s.usedBytesInBuffer)
            {
              s.ZDF_StringBuilder.Append("ManSpec");
              s.addAdditionalData = true;
            }
            s.NoVIF = true;
            s.NoValue = true;
            s.breakParameterLoop = true;
            if (s.actualByte == (byte) 31)
              s.followingTelegrammAnnounced = true;
            return true;
          }
          s.storageNumber = ((int) s.actualByte & 64) >> 6;
          s.functionCode = (MBusDevice.FunctionCode) (((int) s.actualByte & 48) >> 4);
          s.parameterCoding = MBusDevice.DifParamCodeTable[(int) s.actualByte & 15];
          s.parameterLength = MBusDevice.DifLengthTable[(int) s.actualByte & 15];
        }
        else
        {
          if (num > 8)
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal number of DIV values", MBusDevice.MBusDeviceLogger);
          s.storageNumber += ((int) s.actualByte & 15) << num * 4 - 3;
          s.tarifNumber += ((int) s.actualByte & 48) >> 4 << num * 2 - 2;
          s.unitNumber += ((int) s.actualByte & 64) >> 6 << num - 1;
        }
        ++num;
        if (((uint) s.actualByte & 128U) <= 0U)
          return true;
      }
      return false;
    }

    private static bool ScanVif(ref MBusDevice.ParamScannerValues s)
    {
      if (!s.GetNextByte())
        return false;
      if (s.actualByte == (byte) 253)
      {
        if (!s.GetNextByte())
          return false;
        int index = (int) s.actualByte & (int) sbyte.MaxValue;
        s.ZDF_StringBuilder.Append(MBusDevice.VifEList_0xFD[index].VifString);
        s.unitText = MBusDevice.VifEList_0xFD[index].VifUnit;
        s.unitExponent = MBusDevice.VifEList_0xFD[index].VifExponent;
        s.fixStringSize = MBusDevice.VifEList_0xFD[index].FixStringSize;
        s.isHexValue = MBusDevice.VifEList_0xFD[index].IsHexValue;
        if (!MBusDevice.ScanOrtogonalVif(ref s))
          return false;
      }
      else if (s.actualByte == (byte) 251)
      {
        if (!s.GetNextByte())
          return false;
        int index = (int) s.actualByte & (int) sbyte.MaxValue;
        s.ZDF_StringBuilder.Append(MBusDevice.VifEList_0xFB[index].VifString);
        s.unitText = MBusDevice.VifEList_0xFB[index].VifUnit;
        s.unitExponent = MBusDevice.VifEList_0xFB[index].VifExponent;
        s.fixStringSize = MBusDevice.VifEList_0xFB[index].FixStringSize;
        if (!MBusDevice.ScanOrtogonalVif(ref s))
          return false;
      }
      else if (s.actualByte == (byte) 252 || s.actualByte == (byte) 124)
      {
        byte actualByte1 = s.actualByte;
        int index = (int) actualByte1 & (int) sbyte.MaxValue;
        s.ZDF_StringBuilder.Append(MBusDevice.VifEList_0xFB[index].VifString);
        s.unitText = MBusDevice.VifEList_0xFB[index].VifUnit;
        s.unitExponent = MBusDevice.VifEList_0xFB[index].VifExponent;
        s.fixStringSize = MBusDevice.VifEList_0xFB[index].FixStringSize;
        if (!s.GetNextByte())
          return false;
        byte actualByte2 = s.actualByte;
        s.ZDF_StringBuilder.Append(ZR_ClassLibrary.Util.ReverseString(Encoding.ASCII.GetString(s.readBuffer, s.readBufferOffset, (int) actualByte2)));
        s.readBufferOffset += (int) actualByte2;
        s.actualByte = actualByte1;
        if (!MBusDevice.ScanOrtogonalVif(ref s))
          return false;
      }
      else
      {
        int index = (int) s.actualByte & (int) sbyte.MaxValue;
        s.ZDF_StringBuilder.Append(MBusDevice.VifList[index].VifString);
        s.unitExponent = MBusDevice.VifList[index].VifExponent;
        s.fixStringSize = MBusDevice.VifList[index].FixStringSize;
        if (index == (int) sbyte.MaxValue)
        {
          s.ZDF_StringBuilder.Append("ManSpec");
          s.addAdditionalData = true;
          s.NoValue = true;
          s.breakParameterLoop = true;
        }
        else if (!MBusDevice.ScanOrtogonalVif(ref s))
          return false;
      }
      if (s.ZDF_String == "TYPE")
        s.isType = true;
      if (s.storageNumber > 0 || s.unitNumber > 0)
        s.ZDF_StringBuilder.Append("[" + s.storageNumber.ToString() + "]");
      if (s.unitNumber > 0)
        s.ZDF_StringBuilder.Append("[" + s.unitNumber.ToString() + "]");
      if (s.functionCode == MBusDevice.FunctionCode.MaximumValue)
        s.ZDF_StringBuilder.Append("_MAX");
      else if (s.functionCode == MBusDevice.FunctionCode.MinimumValue)
        s.ZDF_StringBuilder.Append("_MIN");
      else if (s.functionCode == MBusDevice.FunctionCode.ValueDuringError)
        s.ZDF_StringBuilder.Append("_ERR");
      if (s.tarifNumber > 0)
        s.ZDF_StringBuilder.Append("_TAR[" + s.tarifNumber.ToString() + "]");
      return true;
    }

    private static bool ScanOrtogonalVif(ref MBusDevice.ParamScannerValues s)
    {
      s.isDateTime = s.ZDF_String.StartsWith("TIMP");
      bool flag = false;
      while (((uint) s.actualByte & 128U) > 0U)
      {
        if (!s.GetNextByte())
          return false;
        s.lastVIFE = s.actualByte;
        int index = (int) s.actualByte & (int) sbyte.MaxValue;
        if (flag)
        {
          s.ZDF_StringBuilder.Append(index.ToString("x02"));
        }
        else
        {
          s.ZDF_StringBuilder.Append(MBusDevice.VifEListOrto[index].VifStringToAdd);
          s.unitExponent += MBusDevice.VifEListOrto[index].VifExponentToAdd;
          if (MBusDevice.VifEListOrto[index].IsDateTimeValue)
            s.isDateTime = true;
          if (index == (int) sbyte.MaxValue)
            flag = true;
        }
      }
      return true;
    }

    private static bool ScanValue(
      ref MBusDevice.ParamScannerValues s,
      List<DeviceInfo.MBusParamStruct> parameter)
    {
      switch (s.parameterCoding)
      {
        case MBusDevice.ParamCode.Int:
          if (s.isDateTime)
          {
            if (s.parameterLength == 2)
            {
              if (!s.GetNextByte())
                return false;
              int actualByte = (int) s.actualByte;
              if (!s.GetNextByte())
                return false;
              int num1 = actualByte + ((int) s.actualByte << 8);
              if (num1 != (int) ushort.MaxValue)
              {
                StringBuilder pvalue1 = s.PValue;
                int num2 = num1 & 31;
                string str1 = num2.ToString("d02") + ".";
                pvalue1.Append(str1);
                StringBuilder pvalue2 = s.PValue;
                num2 = (num1 & 3840) >> 8;
                string str2 = num2.ToString("d02") + ".";
                pvalue2.Append(str2);
                int num3 = (num1 & 224) >> 5 | (num1 & 61440) >> 9;
                int num4 = num3 < 80 ? num3 + 2000 : num3 + 1900;
                s.PValue.Append(num4.ToString("d04"));
                break;
              }
              break;
            }
            if (s.parameterLength == 4)
            {
              if (!s.GetNextByte())
                return false;
              int actualByte1 = (int) s.actualByte;
              if (!s.GetNextByte())
                return false;
              int num5 = actualByte1 + ((int) s.actualByte << 8);
              if ((num5 & 128) != 0)
              {
                s.readBufferOffset += 2;
                s.PValue.Append("01.01.1980 00:00");
                break;
              }
              s.PValue.Append(((num5 & 7936) >> 8).ToString("d02") + ":");
              StringBuilder pvalue3 = s.PValue;
              int num6 = num5 & 63;
              string str3 = num6.ToString("d02");
              pvalue3.Append(str3);
              if (!s.GetNextByte())
                return false;
              int actualByte2 = (int) s.actualByte;
              if (!s.GetNextByte())
                return false;
              int num7 = actualByte2 + ((int) s.actualByte << 8);
              int num8 = (num7 & 224) >> 5 | (num7 & 61440) >> 9;
              int num9 = num8 < 80 ? num8 + 2000 : num8 + 1900;
              s.PValue.Insert(0, num9.ToString("d04") + " ");
              StringBuilder pvalue4 = s.PValue;
              num6 = (num7 & 3840) >> 8;
              string str4 = num6.ToString("d02") + ".";
              pvalue4.Insert(0, str4);
              StringBuilder pvalue5 = s.PValue;
              num6 = num7 & 31;
              string str5 = num6.ToString("d02") + ".";
              pvalue5.Insert(0, str5);
              break;
            }
            if (s.parameterLength == 6)
            {
              if (!s.GetNextByte())
                return false;
              int actualByte3 = (int) s.actualByte;
              if (!s.GetNextByte())
                return false;
              int num10 = actualByte3 + ((int) s.actualByte << 8);
              if (!s.GetNextByte())
                return false;
              int num11 = num10 + ((int) s.actualByte << 16);
              if ((num11 & 32768) != 0)
              {
                s.readBufferOffset += 3;
                s.PValue.Append("01.01.1980 00:00");
                break;
              }
              s.PValue.Append(((num11 & 2031616) >> 16).ToString("d02") + ":");
              s.PValue.Append(((num11 & 16128) >> 8).ToString("d02") + ":");
              StringBuilder pvalue6 = s.PValue;
              int num12 = num11 & 63;
              string str6 = num12.ToString("d02");
              pvalue6.Append(str6);
              if (!s.GetNextByte())
                return false;
              int actualByte4 = (int) s.actualByte;
              if (!s.GetNextByte())
                return false;
              int num13 = actualByte4 + ((int) s.actualByte << 8);
              int num14 = (num13 & 224) >> 5 | (num13 & 61440) >> 9;
              int num15 = num14 < 80 ? num14 + 2000 : num14 + 1900;
              s.PValue.Insert(0, num15.ToString("d04") + " ");
              StringBuilder pvalue7 = s.PValue;
              num12 = (num13 & 3840) >> 8;
              string str7 = num12.ToString("d02") + ".";
              pvalue7.Insert(0, str7);
              StringBuilder pvalue8 = s.PValue;
              num12 = num13 & 31;
              string str8 = num12.ToString("d02") + ".";
              pvalue8.Insert(0, str8);
              ++s.readBufferOffset;
              break;
            }
            s.paramError = true;
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal date parameter byte size", MBusDevice.MBusDeviceLogger);
          }
          if (s.parameterLength != 1 && s.parameterLength != 2 && s.parameterLength != 3 && s.parameterLength != 4 && s.parameterLength != 6 && s.parameterLength != 8)
            return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Invalid parameter length detected! Value: " + s.parameterLength.ToString(), MBusDevice.MBusDeviceLogger);
          switch (s.parameterLength)
          {
            case 1:
              s.PValue.Append(s.readBuffer[s.readBufferOffset].ToString((IFormatProvider) FixedFormates.TheFormates));
              break;
            case 2:
              if (s.isHexValue)
              {
                s.PValue.Append(s.readBuffer[s.readBufferOffset + 1].ToString("X2"));
                s.PValue.Append(s.readBuffer[s.readBufferOffset].ToString("X2"));
                break;
              }
              short int16 = BitConverter.ToInt16(s.readBuffer, s.readBufferOffset);
              s.PValue.Append(int16.ToString((IFormatProvider) FixedFormates.TheFormates));
              break;
            case 3:
              int num16 = BitConverter.ToInt32(new byte[4]
              {
                (byte) 0,
                s.readBuffer[s.readBufferOffset],
                s.readBuffer[s.readBufferOffset + 1],
                s.readBuffer[s.readBufferOffset + 2]
              }, 0) >> 8;
              s.PValue.Append(num16.ToString((IFormatProvider) FixedFormates.TheFormates));
              break;
            case 4:
              if (s.readBuffer[s.readBufferOffset] == (byte) 0 && s.readBuffer[s.readBufferOffset + 1] == (byte) 0 && s.readBuffer[s.readBufferOffset + 2] == (byte) 0 && s.readBuffer[s.readBufferOffset + 3] == (byte) 128)
              {
                s.PValue.Length = 0;
                break;
              }
              int int32_1 = BitConverter.ToInt32(s.readBuffer, s.readBufferOffset);
              s.PValue.Append(int32_1.ToString((IFormatProvider) FixedFormates.TheFormates));
              break;
            case 6:
              long num17 = BitConverter.ToInt64(new byte[8]
              {
                (byte) 0,
                (byte) 0,
                s.readBuffer[s.readBufferOffset],
                s.readBuffer[s.readBufferOffset + 1],
                s.readBuffer[s.readBufferOffset + 2],
                s.readBuffer[s.readBufferOffset + 3],
                s.readBuffer[s.readBufferOffset + 4],
                s.readBuffer[s.readBufferOffset + 5]
              }, 0) >> 16;
              s.PValue.Append(num17.ToString((IFormatProvider) FixedFormates.TheFormates));
              break;
            case 8:
              long int64 = BitConverter.ToInt64(s.readBuffer, s.readBufferOffset);
              s.PValue.Append(int64.ToString((IFormatProvider) FixedFormates.TheFormates));
              break;
            default:
              throw new NotSupportedException("Invalid parameter length = " + s.parameterLength.ToString());
          }
          for (int index = 0; index < s.parameterLength; ++index)
          {
            if (!s.GetNextByte())
              return false;
          }
          break;
        case MBusDevice.ParamCode.Real:
          float single = BitConverter.ToSingle(s.readBuffer, s.readBufferOffset);
          s.readBufferOffset += 4;
          string str9 = single.ToString("F50", (IFormatProvider) FixedFormates.TheFormates);
          if (str9.IndexOf(FixedFormates.TheFormates.NumberFormat.NumberDecimalSeparator) >= 0)
            str9 = str9.TrimEnd('0');
          string str10 = str9.TrimEnd('.');
          s.PValue.Append(str10);
          break;
        case MBusDevice.ParamCode.BCD:
          for (int index1 = 0; index1 < s.parameterLength; ++index1)
          {
            if (!s.GetNextByte())
              return false;
            for (int index2 = 0; index2 < 2; ++index2)
            {
              byte num18 = index2 != 0 ? (byte) ((uint) s.actualByte >> 4) : (byte) ((uint) s.actualByte & 15U);
              if (num18 > (byte) 9)
              {
                if (index1 == s.parameterLength - 1 && index2 == 1 && num18 == (byte) 15)
                {
                  while (s.PValue[0] == '0' && s.PValue.Length > 0)
                    s.PValue = s.PValue.Remove(0, 1);
                  s.PValue.Insert(0, "-");
                }
                else if (num18 < (byte) 15 && num18 > (byte) 9)
                {
                  s.PValue.Insert(0, "!");
                }
                else
                {
                  for (++index1; index1 < s.parameterLength; ++index1)
                  {
                    if (!s.GetNextByte())
                      return false;
                  }
                  break;
                }
              }
              else
                s.PValue.Insert(0, ((char) ((uint) num18 + 48U)).ToString() ?? "");
            }
          }
          break;
        case MBusDevice.ParamCode.Variable:
          if (!s.GetNextByte())
            return false;
          int actualByte5 = (int) s.actualByte;
          if (s.lastVIFE == (byte) 31 && s.storageNumber == 8)
          {
            if (!s.GetNextByte())
              return false;
            MBusDevice.SpacingControl spacingControl = new MBusDevice.SpacingControl(s.actualByte);
            if (!s.GetNextByte())
              return false;
            byte actualByte6 = s.actualByte;
            if (spacingControl.SpaceUnit == MBusDevice.SpaceUnit.DaysOrMonths && actualByte6 == (byte) 254)
            {
              int num19 = (actualByte5 - 2) / spacingControl.Length;
              if (spacingControl.DataField == MBusDevice.ParamCode.BCD)
              {
                s.ZDF_StringBuilder.Replace("_CPWR", "");
                string referenceValueKey = s.ZDF_String;
                DeviceInfo.MBusParamStruct mbusParamStruct1 = parameter.Find((Predicate<DeviceInfo.MBusParamStruct>) (x => x.DefineString == referenceValueKey));
                string str11 = mbusParamStruct1 != null ? mbusParamStruct1.ValueString : string.Empty;
                string referenceTimeKey = "TIMP" + s.ZDF_String.Substring(s.ZDF_String.IndexOf('['));
                DeviceInfo.MBusParamStruct mbusParamStruct2 = parameter.Find((Predicate<DeviceInfo.MBusParamStruct>) (x => x.DefineString == referenceTimeKey));
                string str12 = mbusParamStruct2 != null ? mbusParamStruct2.ValueString : string.Empty;
                int length = referenceValueKey.IndexOf('[');
                int num20 = referenceValueKey.IndexOf(']');
                int int32_2 = Convert.ToInt32(referenceValueKey.Substring(length + 1, num20 - length - 1));
                string str13 = referenceValueKey.Substring(0, length);
                if (string.IsNullOrEmpty(str11))
                {
                  for (int index3 = 0; index3 < num19; ++index3)
                  {
                    for (int index4 = 0; index4 < spacingControl.Length; ++index4)
                    {
                      if (!s.GetNextByte())
                        return false;
                    }
                    List<DeviceInfo.MBusParamStruct> mbusParamStructList1 = parameter;
                    int num21 = int32_2 + index3 + 1;
                    DeviceInfo.MBusParamStruct mbusParamStruct3 = new DeviceInfo.MBusParamStruct("TIMP[" + num21.ToString() + "]", "");
                    mbusParamStructList1.Add(mbusParamStruct3);
                    List<DeviceInfo.MBusParamStruct> mbusParamStructList2 = parameter;
                    string str14 = str13;
                    num21 = int32_2 + index3 + 1;
                    string str15 = num21.ToString();
                    DeviceInfo.MBusParamStruct mbusParamStruct4 = new DeviceInfo.MBusParamStruct(str14 + "[" + str15 + "]", "");
                    mbusParamStructList2.Add(mbusParamStruct4);
                  }
                  s.ZDF_StringBuilder.Length = 0;
                  s.PValue.Length = 0;
                }
                else
                {
                  double num22 = Convert.ToDouble(str11, (IFormatProvider) FixedFormates.TheFormates);
                  DateTime dateTime = Convert.ToDateTime(str12, (IFormatProvider) FixedFormates.TheFormates);
                  if (spacingControl.IncrementMode == MBusDevice.IncrementMode.Diff)
                  {
                    List<double?> nullableList = new List<double?>();
                    double? nullable1;
                    for (int index5 = 0; index5 < num19; ++index5)
                    {
                      int? nullable2 = new int?(0);
                      byte[] numArray = new byte[spacingControl.Length];
                      for (int index6 = 0; index6 < spacingControl.Length; ++index6)
                      {
                        if (!s.GetNextByte())
                          return false;
                        numArray[index6] = s.actualByte;
                      }
                      bool flag = false;
                      for (int index7 = 0; index7 < numArray.Length; ++index7)
                      {
                        if (numArray[index7] != byte.MaxValue)
                          flag = true;
                        int num23 = (int) numArray[index7] << index7 * 8;
                        int? nullable3 = nullable2;
                        nullable2 = nullable3.HasValue ? new int?(num23 | nullable3.GetValueOrDefault()) : new int?();
                      }
                      if (flag)
                      {
                        nullable2 = new int?(ZR_ClassLibrary.Util.ConvertBcdInt32ToInt32(nullable2.Value));
                        s.PValue.Length = 0;
                        s.PValue.Append((object) nullable2);
                        MBusDevice.SetStringExpo(ref s.PValue, s.unitExponent);
                        parameter.Add(new DeviceInfo.MBusParamStruct("CP_DIFF[" + (int32_2 + index5 + 1).ToString() + "]", s.PValue.ToString()));
                        double num24 = Convert.ToDouble(s.PValue.ToString(), (IFormatProvider) FixedFormates.TheFormates);
                        if (nullableList.Count == 0)
                        {
                          double num25 = num22 + num24;
                          nullableList.Add(new double?(num25));
                        }
                        else
                        {
                          nullable1 = nullableList[nullableList.Count - 1];
                          double num26 = num24;
                          double? nullable4 = nullable1.HasValue ? new double?(nullable1.GetValueOrDefault() + num26) : new double?();
                          nullableList.Add(nullable4);
                        }
                      }
                      else
                      {
                        int? nullable5 = new int?();
                        break;
                      }
                    }
                    int num27 = str11.IndexOf(FixedFormates.TheFormates.NumberFormat.NumberDecimalSeparator);
                    int num28 = 0;
                    if (num27 > 0)
                      num28 = str11.Length - num27 - 1;
                    for (int index = 0; index < nullableList.Count; ++index)
                    {
                      nullable1 = nullableList[index];
                      if (nullable1.HasValue)
                      {
                        nullable1 = nullableList[index];
                        string str16 = nullable1.Value.ToString("F50", (IFormatProvider) FixedFormates.TheFormates);
                        if (str16.IndexOf(FixedFormates.TheFormates.NumberFormat.NumberDecimalSeparator) >= 0)
                          str16 = str16.TrimEnd('0');
                        string ValStr = str16.TrimEnd('.');
                        parameter.Add(new DeviceInfo.MBusParamStruct("TIMP[" + (int32_2 + index + 1).ToString() + "]", dateTime.AddMonths(index + 1).ToString("d", (IFormatProvider) FixedFormates.TheFormates)));
                        parameter.Add(new DeviceInfo.MBusParamStruct(str13 + "[" + (int32_2 + index + 1).ToString() + "]", ValStr));
                      }
                      else
                        break;
                    }
                    s.ZDF_StringBuilder.Length = 0;
                    s.PValue.Length = 0;
                  }
                }
              }
              break;
            }
            break;
          }
          if (actualByte5 <= 191)
          {
            for (int index = 0; index < actualByte5 && s.readBufferOffset < s.usedBytesInBuffer; ++index)
            {
              if (!s.GetNextByte())
                return false;
              s.PValue.Insert(0, new char[1]
              {
                (char) s.actualByte
              });
            }
            break;
          }
          s.paramError = true;
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Unknown parameter code", MBusDevice.MBusDeviceLogger);
        default:
          s.paramError = true;
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Unknown parameter coding. Buffer: " + ZR_ClassLibrary.Util.ByteArrayToHexString(s.readBuffer) + " Offset: " + s.readBufferOffset.ToString() + " Unknown ParameterCoding: " + s.parameterCoding.ToString() + " of " + s.ZDF_String, MBusDevice.MBusDeviceLogger);
      }
      MBusDevice.SetStringExpo(ref s.PValue, s.unitExponent);
      while (s.PValue.Length < s.fixStringSize)
        s.PValue.Insert(0, "0");
      if (s.isHexValue)
        s.PValue.Insert(0, "0x");
      return true;
    }

    private int SkipThisParameter(byte ActualByte, int InParamOffset, int ParameterLength)
    {
      while (((uint) ActualByte & 128U) > 0U)
        ActualByte = this.ReceiveBuffer.Data[InParamOffset++];
      while (ParameterLength > 0)
      {
        --ParameterLength;
        ++InParamOffset;
      }
      return InParamOffset;
    }

    public static string GetMediaString(byte Media)
    {
      try
      {
        return ((MBusDeviceType) Media).ToString();
      }
      catch
      {
      }
      return "UNKNOWN (0x" + Media.ToString("x02") + ")";
    }

    internal static long TranslateBcdToBin(long InValue)
    {
      string str = InValue.ToString("X16");
      if (str.Contains("F") || str.Contains("E") || str.Contains("D") || str.Contains("C") || str.Contains("B") || str.Contains("A"))
      {
        MBusDevice.MBusDeviceLogger.Warn("Invalid BCD value detected. Value: 0x" + InValue.ToString("X8"));
        return 0;
      }
      long num = 1;
      long bin = 0;
      for (; InValue > 0L; InValue >>= 4)
      {
        bin += (InValue & 15L) * num;
        num *= 10L;
      }
      return bin;
    }

    internal static void SetStringExpo(ref StringBuilder PValue, int UnitExponent)
    {
      string s = PValue.ToString().Trim();
      if (string.IsNullOrEmpty(s))
        return;
      if (UnitExponent != 0)
      {
        try
        {
          double num = double.Parse(s, (IFormatProvider) FixedFormates.TheFormates) * Math.Pow(10.0, (double) UnitExponent);
          if (num != 0.0)
          {
            string str1 = num.ToString("F50", (IFormatProvider) FixedFormates.TheFormates);
            if (str1.IndexOf(FixedFormates.TheFormates.NumberFormat.NumberDecimalSeparator) >= 0)
              str1 = str1.TrimEnd('0');
            string str2 = str1.TrimEnd('.');
            PValue.Length = 0;
            PValue.Append(str2);
          }
        }
        catch (Exception ex)
        {
          MBusDevice.MBusDeviceLogger.Fatal("Can not scale the PValue: '" + s + " UnitExponent: " + UnitExponent.ToString() + " ' Error: " + ex.Message);
          return;
        }
      }
      while (PValue.Length > 1)
      {
        if (PValue.ToString().Contains(".") && PValue[PValue.Length - 1] == '0' && UnitExponent < 0)
        {
          PValue.Remove(PValue.Length - 1, 1);
          ++UnitExponent;
        }
        else if (PValue[PValue.Length - 1] == '.')
        {
          PValue.Remove(PValue.Length - 1, 1);
        }
        else
        {
          if (PValue[0] != '0' || PValue[1] == '.')
            break;
          PValue.Remove(0, 1);
        }
      }
    }

    public static string ParseMBusDifVif(byte[] data)
    {
      if (data == null)
        return string.Empty;
      List<byte> byteList = new List<byte>((IEnumerable<byte>) data);
      byteList.Add((byte) 0);
      byteList.Add((byte) 0);
      MBusDevice mbusDevice = new MBusDevice();
      mbusDevice.ReceiveBuffer = new ByteField(byteList.ToArray());
      if (!mbusDevice.GenerateParameterList(true))
        return string.Empty;
      mbusDevice.Info.ParameterList.RemoveRange(0, 6);
      string zdfParameterString = mbusDevice.Info.GetZDFParameterString();
      if (zdfParameterString == "NoParameter")
        return string.Empty;
      return zdfParameterString.EndsWith(";") ? zdfParameterString : zdfParameterString + ";";
    }

    internal void SendMeterApplicationResetAsBroadcast()
    {
      if (!this.MyBus.MyCom.Open())
        return;
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.ApplicationReset);
      this.TransmitBuffer = new ByteField(9);
      this.TransmitBuffer.Add(104);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(104);
      this.TransmitBuffer.Add(115);
      this.TransmitBuffer.Add((int) byte.MaxValue);
      this.TransmitBuffer.Add(80);
      this.FinishLongFrame();
      MBusDevice.MBusDeviceLogger.Debug("Send application reset as broadcast");
      this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusApplicationReset);
      this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
      this.MyBus.BusState.IncrementTransmitBlockCounter();
    }

    internal void SND_NKE_Broadcast()
    {
      if (!this.MyBus.MyCom.Open())
        return;
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.SEND_NKE);
      this.TransmitBuffer = new ByteField(5);
      this.TransmitBuffer.Add(16);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add((int) byte.MaxValue);
      this.TransmitBuffer.Add(63);
      this.TransmitBuffer.Add(22);
      MBusDevice.MBusDeviceLogger.Debug("Send SEND_NKE");
      this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusSendSND_NKE);
      this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
      this.MyBus.BusState.IncrementTransmitBlockCounter();
    }

    internal void SendSynchronizeAction()
    {
      if (!this.MyBus.MyCom.Open())
        return;
      byte primaryAddress = this.GetPrimaryAddress();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.SynchronizeAction);
      this.TransmitBuffer = new ByteField(9);
      this.TransmitBuffer.Add(104);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(104);
      this.TransmitBuffer.Add(115);
      this.TransmitBuffer.Add(primaryAddress);
      this.TransmitBuffer.Add(92);
      this.FinishLongFrame();
      MBusDevice.MBusDeviceLogger.Debug("Send Synchronize Action");
      this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusSendSynchronizeAction);
      this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
      this.MyBus.BusState.IncrementTransmitBlockCounter();
      Thread.Sleep(32);
    }

    internal enum ParamCode
    {
      Int,
      Real,
      BCD,
      None,
      Special,
      Readout,
      Variable,
    }

    internal enum FunctionCode
    {
      InstantaneousValue,
      MaximumValue,
      MinimumValue,
      ValueDuringError,
    }

    private sealed class SpacingControl
    {
      private byte value;

      public MBusDevice.IncrementMode IncrementMode
      {
        get => (MBusDevice.IncrementMode) ((int) this.value >> 6);
      }

      public MBusDevice.SpaceUnit SpaceUnit
      {
        get => (MBusDevice.SpaceUnit) (((int) this.value & 48) >> 4);
      }

      public MBusDevice.ParamCode DataField => MBusDevice.DifParamCodeTable[(int) this.value & 15];

      public int Length => MBusDevice.DifLengthTable[(int) this.value & 15];

      public SpacingControl(byte value) => this.value = value;
    }

    private enum IncrementMode
    {
      Abs,
      Inc,
      Dec,
      Diff,
    }

    private enum SpaceUnit
    {
      Secounds,
      Minutes,
      Hours,
      DaysOrMonths,
    }

    public struct VifStruct
    {
      public string VifString;
      public int VifExponent;
      public int FixStringSize;
      public string VifUnit;
      public bool IsHexValue;

      public VifStruct(string Str, int Expo, int FixStringS, string UnitIn)
      {
        this.VifString = Str;
        this.VifExponent = Expo;
        this.FixStringSize = FixStringS;
        this.VifUnit = UnitIn;
        this.IsHexValue = false;
      }

      public VifStruct(string Str, int Expo, int FixStringS, string UnitIn, bool isBinary)
      {
        this.VifString = Str;
        this.VifExponent = Expo;
        this.FixStringSize = FixStringS;
        this.VifUnit = UnitIn;
        this.IsHexValue = isBinary;
      }
    }

    public struct OrtoVifStruct(
      string VifStringToAdd,
      int VifExponentToAdd,
      bool IsDateTimeValue,
      string VifUnitToAdd)
    {
      public string VifStringToAdd = VifStringToAdd;
      public int VifExponentToAdd = VifExponentToAdd;
      public bool IsDateTimeValue = IsDateTimeValue;
      public string VifUnitToAdd = VifUnitToAdd;
    }

    public enum REQ_UD2_Type : byte
    {
      REQ_UD1_5A = 90, // 0x5A
      REQ_UD2_5B = 91, // 0x5B
      REQ_UD1_7A = 122, // 0x7A
      REQ_UD2_7B = 123, // 0x7B
    }

    internal class ParamScannerValues
    {
      internal byte[] readBuffer;
      internal int usedBytesInBuffer;
      internal int readBufferStartOffset;
      internal int readBufferOffset;
      internal byte actualByte;
      internal StringBuilder ZDF_StringBuilder = new StringBuilder();
      internal byte lastVIFE;
      internal int storageNumber;
      internal int tarifNumber;
      internal int unitNumber;
      internal string unitText;
      internal MBusDevice.FunctionCode functionCode;
      internal MBusDevice.ParamCode parameterCoding;
      internal int parameterLength;
      internal bool followingTelegrammAnnounced;
      internal bool NoVIF;
      internal bool NoValue;
      internal bool breakParameterLoop;
      internal bool paramError;
      internal bool addAdditionalData;
      internal bool isType;
      internal int unitExponent;
      internal int fixStringSize;
      internal bool isDateTime;
      internal bool isHexValue;
      internal StringBuilder PValue = new StringBuilder();

      internal string ZDF_String => this.ZDF_StringBuilder.ToString();

      internal void Clear()
      {
        this.ZDF_StringBuilder.Length = 0;
        this.lastVIFE = (byte) 0;
        this.storageNumber = 0;
        this.tarifNumber = 0;
        this.unitNumber = 0;
        this.unitText = string.Empty;
        this.functionCode = MBusDevice.FunctionCode.InstantaneousValue;
        this.parameterCoding = MBusDevice.ParamCode.None;
        this.parameterLength = 0;
        this.NoVIF = false;
        this.NoValue = false;
        this.breakParameterLoop = false;
        this.paramError = false;
        this.addAdditionalData = false;
        this.isType = false;
        this.unitExponent = 0;
        this.fixStringSize = 0;
        this.isDateTime = false;
        this.isHexValue = false;
        this.PValue.Length = 0;
      }

      internal bool GetNextByte()
      {
        if (this.readBufferOffset >= this.usedBytesInBuffer)
          return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal data length by MBus parameter scanning", MBusDevice.MBusDeviceLogger);
        this.actualByte = this.readBuffer[this.readBufferOffset++];
        return true;
      }
    }
  }
}

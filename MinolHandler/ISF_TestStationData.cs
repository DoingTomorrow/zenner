// Decompiled with JetBrains decompiler
// Type: MinolHandler.ISF_TestStationData
// Assembly: MinolHandler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: A1A42975-0CFC-4FCB-838E-3BA18C5EABDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinolHandler.dll

using System;
using ZR_ClassLibrary;

#nullable disable
namespace MinolHandler
{
  public sealed class ISF_TestStationData
  {
    public const int BLOCK_START_ADDRESS = 4102;
    public const int BLOCK_LENGTH = 30;
    public const int ADDRESS_OF_CHECKSUM = 4160;
    public const int ADDRESS_OF_METER_ID = 4106;

    public byte[] Buffer { get; set; }

    public long? HardwareSerialnumber { get; set; }

    public DateTime? DateOfMeasuring { get; set; }

    public byte? CountOfHardwareTests { get; set; }

    public byte? StatusByte { get; set; }

    public TestStationDataStatus Status
    {
      get
      {
        return this.StatusByte.HasValue ? (TestStationDataStatus) Enum.ToObject(typeof (TestStationDataStatus), (object) this.StatusByte) : TestStationDataStatus.Undefined;
      }
    }

    public byte? PulseTestStationNumber { get; set; }

    public int? TesterSWVersion { get; set; }

    public int? VibrationLevelUndamped { get; set; }

    public int? VibrationLevelDamped { get; set; }

    public Decimal? CurrentConsumption { get; set; }

    public int? MeterID { get; set; }

    public byte[] Reserve { get; set; }

    internal static ISF_TestStationData GetTestStationData(short[] map)
    {
      if ((byte) map[4101] != byte.MaxValue || (byte) map[4100] != byte.MaxValue)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Der Datensatz von Löt- und Pulsprüfplatz (L&P) hat an der Stelle INFO:TabFlowX0 einen falschen Wert!");
        return (ISF_TestStationData) null;
      }
      ISF_TestStationData testStationData = new ISF_TestStationData();
      int num1 = 4102;
      short[] numArray1 = map;
      int index1 = num1;
      int num2 = index1 + 1;
      long num3 = (long) (byte) numArray1[index1];
      short[] numArray2 = map;
      int index2 = num2;
      int num4 = index2 + 1;
      long num5 = (long) (byte) numArray2[index2] << 8;
      long num6 = num3 | num5;
      short[] numArray3 = map;
      int index3 = num4;
      int num7 = index3 + 1;
      long num8 = (long) (byte) numArray3[index3] << 16;
      long num9 = num6 | num8;
      short[] numArray4 = map;
      int index4 = num7;
      int num10 = index4 + 1;
      long num11 = (long) (byte) numArray4[index4] << 24;
      long bcd = num9 | num11;
      if (bcd != (long) uint.MaxValue)
        testStationData.HardwareSerialnumber = new long?(Util.ConvertBcdInt64ToInt64(bcd));
      short[] numArray5 = map;
      int index5 = num10;
      int num12 = index5 + 1;
      int num13 = (int) (byte) numArray5[index5];
      short[] numArray6 = map;
      int index6 = num12;
      int num14 = index6 + 1;
      int num15 = (int) (byte) numArray6[index6] << 8;
      int num16 = num13 | num15;
      short[] numArray7 = map;
      int index7 = num14;
      int num17 = index7 + 1;
      int num18 = (int) (byte) numArray7[index7] << 16;
      int num19 = num16 | num18;
      short[] numArray8 = map;
      int index8 = num17;
      int num20 = index8 + 1;
      int num21 = (int) (byte) numArray8[index8] << 24;
      int num22 = num19 | num21;
      if (num22 != (int) ushort.MaxValue && num22 > 0)
        testStationData.MeterID = new int?(num22);
      short[] numArray9 = map;
      int index9 = num20;
      int num23 = index9 + 1;
      long num24 = (long) (byte) numArray9[index9];
      short[] numArray10 = map;
      int index10 = num23;
      int num25 = index10 + 1;
      long num26 = (long) (byte) numArray10[index10] << 8;
      long timeValue = num24 | num26;
      short[] numArray11 = map;
      int index11 = num25;
      int num27 = index11 + 1;
      byte minute = (byte) numArray11[index11];
      short[] numArray12 = map;
      int index12 = num27;
      int num28 = index12 + 1;
      byte hour = (byte) numArray12[index12];
      if (timeValue != (long) ushort.MaxValue && hour != byte.MaxValue && minute != byte.MaxValue)
      {
        DateTime dateTime = ParameterAccess.ConvertInt64_YYYYMMMMYYYDDDDD_ToDateTime(timeValue);
        if (dateTime != DateTime.MinValue)
          testStationData.DateOfMeasuring = new DateTime?(new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, (int) hour, (int) minute, 0));
      }
      short[] numArray13 = map;
      int index13 = num28;
      int num29 = index13 + 1;
      byte num30 = (byte) numArray13[index13];
      if (num30 != byte.MaxValue)
        testStationData.CountOfHardwareTests = new byte?(num30);
      short[] numArray14 = map;
      int index14 = num29;
      int num31 = index14 + 1;
      byte num32 = (byte) numArray14[index14];
      if (num32 != byte.MaxValue)
        testStationData.StatusByte = new byte?(num32);
      short[] numArray15 = map;
      int index15 = num31;
      int num33 = index15 + 1;
      byte num34 = (byte) numArray15[index15];
      if (num34 != byte.MaxValue)
        testStationData.PulseTestStationNumber = new byte?(num34);
      ISF_TestStationData isfTestStationData1 = testStationData;
      short[] numArray16 = map;
      int index16 = num33;
      int num35 = index16 + 1;
      int? nullable1 = new int?((int) (byte) numArray16[index16]);
      isfTestStationData1.TesterSWVersion = nullable1;
      ISF_TestStationData isfTestStationData2 = testStationData;
      int? nullable2 = isfTestStationData2.TesterSWVersion;
      short[] numArray17 = map;
      int index17 = num35;
      int num36 = index17 + 1;
      int num37 = (int) (byte) numArray17[index17] << 8;
      isfTestStationData2.TesterSWVersion = nullable2.HasValue ? new int?(nullable2.GetValueOrDefault() | num37) : new int?();
      nullable2 = testStationData.TesterSWVersion;
      int maxValue1 = (int) ushort.MaxValue;
      if (nullable2.GetValueOrDefault() == maxValue1 & nullable2.HasValue)
      {
        ISF_TestStationData isfTestStationData3 = testStationData;
        nullable2 = new int?();
        int? nullable3 = nullable2;
        isfTestStationData3.TesterSWVersion = nullable3;
      }
      ISF_TestStationData isfTestStationData4 = testStationData;
      short[] numArray18 = map;
      int index18 = num36;
      int num38 = index18 + 1;
      int? nullable4 = new int?((int) (byte) numArray18[index18]);
      isfTestStationData4.VibrationLevelUndamped = nullable4;
      ISF_TestStationData isfTestStationData5 = testStationData;
      nullable2 = isfTestStationData5.VibrationLevelUndamped;
      short[] numArray19 = map;
      int index19 = num38;
      int num39 = index19 + 1;
      int num40 = (int) (byte) numArray19[index19] << 8;
      isfTestStationData5.VibrationLevelUndamped = nullable2.HasValue ? new int?(nullable2.GetValueOrDefault() | num40) : new int?();
      nullable2 = testStationData.VibrationLevelUndamped;
      int maxValue2 = (int) ushort.MaxValue;
      if (nullable2.GetValueOrDefault() == maxValue2 & nullable2.HasValue)
      {
        ISF_TestStationData isfTestStationData6 = testStationData;
        nullable2 = new int?();
        int? nullable5 = nullable2;
        isfTestStationData6.VibrationLevelUndamped = nullable5;
      }
      else
      {
        ISF_TestStationData isfTestStationData7 = testStationData;
        nullable2 = testStationData.VibrationLevelUndamped;
        int? nullable6 = new int?(Convert.ToInt32(2.932551319648093841642228739M * (Decimal) nullable2.Value));
        isfTestStationData7.VibrationLevelUndamped = nullable6;
      }
      ISF_TestStationData isfTestStationData8 = testStationData;
      short[] numArray20 = map;
      int index20 = num39;
      int num41 = index20 + 1;
      int? nullable7 = new int?((int) (byte) numArray20[index20]);
      isfTestStationData8.VibrationLevelDamped = nullable7;
      ISF_TestStationData isfTestStationData9 = testStationData;
      nullable2 = isfTestStationData9.VibrationLevelDamped;
      short[] numArray21 = map;
      int index21 = num41;
      int num42 = index21 + 1;
      int num43 = (int) (byte) numArray21[index21] << 8;
      isfTestStationData9.VibrationLevelDamped = nullable2.HasValue ? new int?(nullable2.GetValueOrDefault() | num43) : new int?();
      nullable2 = testStationData.VibrationLevelDamped;
      int maxValue3 = (int) ushort.MaxValue;
      if (nullable2.GetValueOrDefault() == maxValue3 & nullable2.HasValue)
      {
        ISF_TestStationData isfTestStationData10 = testStationData;
        nullable2 = new int?();
        int? nullable8 = nullable2;
        isfTestStationData10.VibrationLevelDamped = nullable8;
      }
      else
      {
        ISF_TestStationData isfTestStationData11 = testStationData;
        nullable2 = testStationData.VibrationLevelDamped;
        int? nullable9 = new int?(Convert.ToInt32(2.932551319648093841642228739M * (Decimal) nullable2.Value));
        isfTestStationData11.VibrationLevelDamped = nullable9;
      }
      short[] numArray22 = map;
      int index22 = num42;
      int num44 = index22 + 1;
      int num45 = (int) (byte) numArray22[index22];
      short[] numArray23 = map;
      int index23 = num44;
      int num46 = index23 + 1;
      int num47 = (int) (byte) numArray23[index23] << 8;
      int num48 = num45 | num47;
      testStationData.CurrentConsumption = num48 != (int) ushort.MaxValue ? new Decimal?((Decimal) num48 / 10M) : new Decimal?();
      testStationData.Reserve = new byte[7];
      byte[] reserve1 = testStationData.Reserve;
      short[] numArray24 = map;
      int index24 = num46;
      int num49 = index24 + 1;
      int num50 = (int) (byte) numArray24[index24];
      reserve1[0] = (byte) num50;
      byte[] reserve2 = testStationData.Reserve;
      short[] numArray25 = map;
      int index25 = num49;
      int num51 = index25 + 1;
      int num52 = (int) (byte) numArray25[index25];
      reserve2[1] = (byte) num52;
      byte[] reserve3 = testStationData.Reserve;
      short[] numArray26 = map;
      int index26 = num51;
      int num53 = index26 + 1;
      int num54 = (int) (byte) numArray26[index26];
      reserve3[2] = (byte) num54;
      byte[] reserve4 = testStationData.Reserve;
      short[] numArray27 = map;
      int index27 = num53;
      int num55 = index27 + 1;
      int num56 = (int) (byte) numArray27[index27];
      reserve4[3] = (byte) num56;
      byte[] reserve5 = testStationData.Reserve;
      short[] numArray28 = map;
      int index28 = num55;
      int num57 = index28 + 1;
      int num58 = (int) (byte) numArray28[index28];
      reserve5[4] = (byte) num58;
      byte[] reserve6 = testStationData.Reserve;
      short[] numArray29 = map;
      int index29 = num57;
      int num59 = index29 + 1;
      int num60 = (int) (byte) numArray29[index29];
      reserve6[5] = (byte) num60;
      byte[] reserve7 = testStationData.Reserve;
      short[] numArray30 = map;
      int index30 = num59;
      int num61 = index30 + 1;
      int num62 = (int) (byte) numArray30[index30];
      reserve7[6] = (byte) num62;
      byte[] numArray31 = new byte[30];
      for (int index31 = 0; index31 < 30; ++index31)
        numArray31[index31] = (byte) map[4102 + index31];
      testStationData.Buffer = numArray31;
      int num63 = 4096;
      int num64 = 4160;
      byte[] adr = new byte[num64 - num63];
      int index32 = 0;
      int index33 = num63;
      while (index33 < num64)
      {
        adr[index32] = (byte) map[index33];
        ++index33;
        ++index32;
      }
      if ((int) CRC.calculateChecksumReversed(adr, (uint) adr.Length, 0U) == ((int) (byte) map[4160] | (int) (byte) map[4161] << 8))
        return testStationData;
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Checksum error!");
      return (ISF_TestStationData) null;
    }

    internal static void SetMeterIDToISF(MinolDevice minolDevice)
    {
      ISF_TestStationData.GetTestStationData(minolDevice.Map);
      byte[] buffer = new byte[4]
      {
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue
      };
      if (minolDevice.MeterID.HasValue)
        buffer = Util.ConvertLongToByteArray((long) minolDevice.MeterID.Value, 4);
      minolDevice.SetMapValue(4106, buffer);
    }

    internal static void SetTestStationData(MinolDevice minolDevice, ISF_TestStationData data)
    {
      minolDevice.SetMapValue(4101, byte.MaxValue);
      minolDevice.SetMapValue(4100, byte.MaxValue);
      int num1 = 4102;
      int num2;
      if (data.HardwareSerialnumber.HasValue)
      {
        long bcdInt64 = Util.ConvertInt64ToBcdInt64(data.HardwareSerialnumber.Value);
        MinolDevice minolDevice1 = minolDevice;
        int address1 = num1;
        int num3 = address1 + 1;
        int newValue1 = (int) (byte) bcdInt64;
        minolDevice1.SetMapValue(address1, (byte) newValue1);
        MinolDevice minolDevice2 = minolDevice;
        int address2 = num3;
        int num4 = address2 + 1;
        int newValue2 = (int) (byte) (bcdInt64 >> 8);
        minolDevice2.SetMapValue(address2, (byte) newValue2);
        MinolDevice minolDevice3 = minolDevice;
        int address3 = num4;
        int num5 = address3 + 1;
        int newValue3 = (int) (byte) (bcdInt64 >> 16);
        minolDevice3.SetMapValue(address3, (byte) newValue3);
        MinolDevice minolDevice4 = minolDevice;
        int address4 = num5;
        num2 = address4 + 1;
        int newValue4 = (int) (byte) (bcdInt64 >> 24);
        minolDevice4.SetMapValue(address4, (byte) newValue4);
      }
      else
      {
        MinolDevice minolDevice5 = minolDevice;
        int address5 = num1;
        int num6 = address5 + 1;
        minolDevice5.SetMapValue(address5, byte.MaxValue);
        MinolDevice minolDevice6 = minolDevice;
        int address6 = num6;
        int num7 = address6 + 1;
        minolDevice6.SetMapValue(address6, byte.MaxValue);
        MinolDevice minolDevice7 = minolDevice;
        int address7 = num7;
        int num8 = address7 + 1;
        minolDevice7.SetMapValue(address7, byte.MaxValue);
        MinolDevice minolDevice8 = minolDevice;
        int address8 = num8;
        num2 = address8 + 1;
        minolDevice8.SetMapValue(address8, byte.MaxValue);
      }
      int? nullable1;
      int num9;
      if (data.MeterID.HasValue)
      {
        MinolDevice minolDevice9 = minolDevice;
        int address9 = num2;
        int num10 = address9 + 1;
        nullable1 = data.MeterID;
        int newValue5 = (int) (byte) nullable1.Value;
        minolDevice9.SetMapValue(address9, (byte) newValue5);
        MinolDevice minolDevice10 = minolDevice;
        int address10 = num10;
        int num11 = address10 + 1;
        nullable1 = data.MeterID;
        int newValue6 = (int) (byte) (nullable1.Value >> 8);
        minolDevice10.SetMapValue(address10, (byte) newValue6);
        MinolDevice minolDevice11 = minolDevice;
        int address11 = num11;
        int num12 = address11 + 1;
        nullable1 = data.MeterID;
        int newValue7 = (int) (byte) (nullable1.Value >> 16);
        minolDevice11.SetMapValue(address11, (byte) newValue7);
        MinolDevice minolDevice12 = minolDevice;
        int address12 = num12;
        num9 = address12 + 1;
        nullable1 = data.MeterID;
        int newValue8 = (int) (byte) (nullable1.Value >> 24);
        minolDevice12.SetMapValue(address12, (byte) newValue8);
      }
      else
      {
        MinolDevice minolDevice13 = minolDevice;
        int address13 = num2;
        int num13 = address13 + 1;
        minolDevice13.SetMapValue(address13, byte.MaxValue);
        MinolDevice minolDevice14 = minolDevice;
        int address14 = num13;
        int num14 = address14 + 1;
        minolDevice14.SetMapValue(address14, byte.MaxValue);
        MinolDevice minolDevice15 = minolDevice;
        int address15 = num14;
        int num15 = address15 + 1;
        minolDevice15.SetMapValue(address15, byte.MaxValue);
        MinolDevice minolDevice16 = minolDevice;
        int address16 = num15;
        num9 = address16 + 1;
        minolDevice16.SetMapValue(address16, byte.MaxValue);
        minolDevice.MeterID = new int?();
      }
      int num16;
      if (data.DateOfMeasuring.HasValue)
      {
        DateTime? dateOfMeasuring = data.DateOfMeasuring;
        long yyyymmmmyyyddddd = ParameterAccess.ConvertDateTimeTo_YYYYMMMMYYYDDDDD(dateOfMeasuring.Value);
        MinolDevice minolDevice17 = minolDevice;
        int address17 = num9;
        int num17 = address17 + 1;
        int newValue9 = (int) (byte) yyyymmmmyyyddddd;
        minolDevice17.SetMapValue(address17, (byte) newValue9);
        MinolDevice minolDevice18 = minolDevice;
        int address18 = num17;
        int num18 = address18 + 1;
        int newValue10 = (int) (byte) (yyyymmmmyyyddddd >> 8);
        minolDevice18.SetMapValue(address18, (byte) newValue10);
        MinolDevice minolDevice19 = minolDevice;
        int address19 = num18;
        int num19 = address19 + 1;
        dateOfMeasuring = data.DateOfMeasuring;
        int minute = (int) (byte) dateOfMeasuring.Value.Minute;
        minolDevice19.SetMapValue(address19, (byte) minute);
        MinolDevice minolDevice20 = minolDevice;
        int address20 = num19;
        num16 = address20 + 1;
        dateOfMeasuring = data.DateOfMeasuring;
        int hour = (int) (byte) dateOfMeasuring.Value.Hour;
        minolDevice20.SetMapValue(address20, (byte) hour);
      }
      else
      {
        MinolDevice minolDevice21 = minolDevice;
        int address21 = num9;
        int num20 = address21 + 1;
        minolDevice21.SetMapValue(address21, byte.MaxValue);
        MinolDevice minolDevice22 = minolDevice;
        int address22 = num20;
        int num21 = address22 + 1;
        minolDevice22.SetMapValue(address22, byte.MaxValue);
        MinolDevice minolDevice23 = minolDevice;
        int address23 = num21;
        int num22 = address23 + 1;
        minolDevice23.SetMapValue(address23, byte.MaxValue);
        MinolDevice minolDevice24 = minolDevice;
        int address24 = num22;
        num16 = address24 + 1;
        minolDevice24.SetMapValue(address24, byte.MaxValue);
      }
      int num23;
      byte? nullable2;
      if (data.CountOfHardwareTests.HasValue)
      {
        MinolDevice minolDevice25 = minolDevice;
        int address = num16;
        num23 = address + 1;
        nullable2 = data.CountOfHardwareTests;
        int newValue = (int) nullable2.Value;
        minolDevice25.SetMapValue(address, (byte) newValue);
      }
      else
      {
        MinolDevice minolDevice26 = minolDevice;
        int address = num16;
        num23 = address + 1;
        minolDevice26.SetMapValue(address, byte.MaxValue);
      }
      nullable2 = data.StatusByte;
      int num24;
      if (nullable2.HasValue)
      {
        MinolDevice minolDevice27 = minolDevice;
        int address = num23;
        num24 = address + 1;
        nullable2 = data.StatusByte;
        int newValue = (int) nullable2.Value;
        minolDevice27.SetMapValue(address, (byte) newValue);
      }
      else
      {
        MinolDevice minolDevice28 = minolDevice;
        int address = num23;
        num24 = address + 1;
        minolDevice28.SetMapValue(address, byte.MaxValue);
      }
      nullable2 = data.PulseTestStationNumber;
      int num25;
      if (nullable2.HasValue)
      {
        MinolDevice minolDevice29 = minolDevice;
        int address = num24;
        num25 = address + 1;
        nullable2 = data.PulseTestStationNumber;
        int newValue = (int) nullable2.Value;
        minolDevice29.SetMapValue(address, (byte) newValue);
      }
      else
      {
        MinolDevice minolDevice30 = minolDevice;
        int address = num24;
        num25 = address + 1;
        minolDevice30.SetMapValue(address, byte.MaxValue);
      }
      nullable1 = data.TesterSWVersion;
      int num26;
      if (nullable1.HasValue)
      {
        MinolDevice minolDevice31 = minolDevice;
        int address25 = num25;
        int num27 = address25 + 1;
        nullable1 = data.TesterSWVersion;
        int newValue11 = (int) (byte) nullable1.Value;
        minolDevice31.SetMapValue(address25, (byte) newValue11);
        MinolDevice minolDevice32 = minolDevice;
        int address26 = num27;
        num26 = address26 + 1;
        nullable1 = data.TesterSWVersion;
        int newValue12 = (int) (byte) (nullable1.Value >> 8);
        minolDevice32.SetMapValue(address26, (byte) newValue12);
      }
      else
      {
        MinolDevice minolDevice33 = minolDevice;
        int address27 = num25;
        int num28 = address27 + 1;
        minolDevice33.SetMapValue(address27, byte.MaxValue);
        MinolDevice minolDevice34 = minolDevice;
        int address28 = num28;
        num26 = address28 + 1;
        minolDevice34.SetMapValue(address28, byte.MaxValue);
      }
      nullable1 = data.VibrationLevelUndamped;
      int num29;
      if (nullable1.HasValue)
      {
        nullable1 = data.VibrationLevelUndamped;
        int num30 = (int) Math.Round((Decimal) nullable1.Value * 1023M / 3000M);
        MinolDevice minolDevice35 = minolDevice;
        int address29 = num26;
        int num31 = address29 + 1;
        int newValue13 = (int) (byte) num30;
        minolDevice35.SetMapValue(address29, (byte) newValue13);
        MinolDevice minolDevice36 = minolDevice;
        int address30 = num31;
        num29 = address30 + 1;
        int newValue14 = (int) (byte) (num30 >> 8);
        minolDevice36.SetMapValue(address30, (byte) newValue14);
      }
      else
      {
        MinolDevice minolDevice37 = minolDevice;
        int address31 = num26;
        int num32 = address31 + 1;
        minolDevice37.SetMapValue(address31, byte.MaxValue);
        MinolDevice minolDevice38 = minolDevice;
        int address32 = num32;
        num29 = address32 + 1;
        minolDevice38.SetMapValue(address32, byte.MaxValue);
      }
      nullable1 = data.VibrationLevelDamped;
      int num33;
      if (nullable1.HasValue)
      {
        nullable1 = data.VibrationLevelDamped;
        int num34 = (int) Math.Round((Decimal) nullable1.Value * 1023M / 3000M);
        MinolDevice minolDevice39 = minolDevice;
        int address33 = num29;
        int num35 = address33 + 1;
        int newValue15 = (int) (byte) num34;
        minolDevice39.SetMapValue(address33, (byte) newValue15);
        MinolDevice minolDevice40 = minolDevice;
        int address34 = num35;
        num33 = address34 + 1;
        int newValue16 = (int) (byte) (num34 >> 8);
        minolDevice40.SetMapValue(address34, (byte) newValue16);
      }
      else
      {
        MinolDevice minolDevice41 = minolDevice;
        int address35 = num29;
        int num36 = address35 + 1;
        minolDevice41.SetMapValue(address35, byte.MaxValue);
        MinolDevice minolDevice42 = minolDevice;
        int address36 = num36;
        num33 = address36 + 1;
        minolDevice42.SetMapValue(address36, byte.MaxValue);
      }
      int num37;
      if (data.CurrentConsumption.HasValue)
      {
        int num38 = (int) (data.CurrentConsumption.Value * 10M);
        MinolDevice minolDevice43 = minolDevice;
        int address37 = num33;
        int num39 = address37 + 1;
        int newValue17 = (int) (byte) num38;
        minolDevice43.SetMapValue(address37, (byte) newValue17);
        MinolDevice minolDevice44 = minolDevice;
        int address38 = num39;
        num37 = address38 + 1;
        int newValue18 = (int) (byte) (num38 >> 8);
        minolDevice44.SetMapValue(address38, (byte) newValue18);
      }
      else
      {
        MinolDevice minolDevice45 = minolDevice;
        int address39 = num33;
        int num40 = address39 + 1;
        minolDevice45.SetMapValue(address39, byte.MaxValue);
        MinolDevice minolDevice46 = minolDevice;
        int address40 = num40;
        num37 = address40 + 1;
        minolDevice46.SetMapValue(address40, byte.MaxValue);
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_TDC_Internals
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.Functions
{
  internal class S4_TDC_Internals
  {
    private static Logger IUW_UltrasonicErrorLogger = LogManager.GetLogger(nameof (IUW_UltrasonicErrorLogger));
    private static Logger IUW_UltrasonicLogger = LogManager.GetLogger(nameof (IUW_UltrasonicLogger));
    private static SortedList<string, string> HeaderLineAbbreviations;
    internal string LogFilePath;
    internal const int originalStructSize = 60;
    internal const int expandedStructureSize = 80;
    internal const int highSampledStructSize = 128;
    private uint structSize;
    private uint uint1_address;
    private uint uint2_address;
    internal AddressRange tdcAddressRange1Chanals;
    internal AddressRange tdcAddressRange2Chanals;
    internal AddressRange unitsStorageTimeAddressRange;
    internal S4_Meter connectedMeter;
    internal S4_DeviceMemory meterMemory;
    private S4_DeviceCommandsNFC nfcCMD;
    internal tdcData tdcUnit_1;
    internal tdcData tdcUnit_2;
    private DateTime unitStorageTime;
    private Parameter32bit ConfigFlagRegister;
    private ushort ConfigFlagRegisterValue;
    private static int[] columnCharacters = new int[9]
    {
      10,
      11,
      13,
      9,
      15,
      15,
      15,
      15,
      15
    };
    internal uint RefCycles;
    internal bool EmcRefFinished;
    internal bool EmcTestActive;
    internal bool EmcTestFinished;
    private double refFlow = 0.0;
    private EmcTestData FirstRefData;
    private EmcTestData LastRefData;
    private EmcTestData FirstTestData;
    private EmcTestData LastTestData;
    private EmcTestData LastReadData = (EmcTestData) null;
    internal List<tdcData> tdc1Log;
    internal List<tdcData> tdc2Log;

    private bool twoUsChanelsUsed => ((int) this.ConfigFlagRegisterValue & 1) == 0;

    private bool onlyChanel1Active => ((uint) this.ConfigFlagRegisterValue & 4096U) > 0U;

    private bool onlyChanel2Active => ((uint) this.ConfigFlagRegisterValue & 8192U) > 0U;

    private bool parallel_TOF_evel => ((uint) this.ConfigFlagRegisterValue & 16U) > 0U;

    static S4_TDC_Internals()
    {
      S4_TDC_Internals.HeaderLineAbbreviations = new SortedList<string, string>();
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.flowWithoutCor.ToString(), "flow");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.flowWithCor.ToString(), "flowCor");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.speedOfSound.ToString(), "speed");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.tofDiffAvg.ToString(), "tofDiff");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.calibration.ToString(), "calib");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.usedTofDiffAvg.ToString(), "U_tofDiff");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.avgDn.ToString(), "avgDn");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.avgUp.ToString(), "avgUp");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.t1Avg.ToString(), "t1Avg");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.t3Avg.ToString(), "t3Avg");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.intStatusReg.ToString(), "StatReg");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.temperature.ToString(), "temp");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.errorFlags.ToString(), "err");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.wvrDnReal.ToString(), "wvrDn");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.wvrDnIdeal.ToString(), "wvrDnI");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.wvrUpReal.ToString(), "wvrDu");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.wvrUpIdeal.ToString(), "wvrDuI");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.tofCycles.ToString(), "tofCyc");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.tempCycles.ToString(), "tempCyc");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.cOffsetUp.ToString(), "cOffUp");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.cOffsetDn.ToString(), "cOffDn");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.isUnit2.ToString(), "isUnit2");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.wvrDnUsed.ToString(), "wvrDnU");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.wvrUpUsed.ToString(), "wvrUpU");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.latestTofCycles.ToString(), "laCyc");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.latestTofDiffValue.ToString(), "laDifV");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.latestTofDiffSum.ToString(), "laDifS");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.latestTofDiffMean.ToString(), "laDifM");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.latestTofDiffAvgNotUsed.ToString(), "laDifNU");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.latestUsedCycles.ToString(), "usedCy");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.highSampledNextValue.ToString(), "highSampledNextValue");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.latestDummy2.ToString(), "latestDummy2");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.latestDummy3.ToString(), "latestDummy3");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.highSampledTofDiffList.ToString(), "highSampledTofDiffList");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.highSampledWvrUpList.ToString(), "highSampledWvrUpList");
      S4_TDC_Internals.HeaderLineAbbreviations.Add(TDC_VALUENAMES.highSampledWvrDnList.ToString(), "highSampledWvrDnList");
    }

    internal S4_TDC_Internals(S4_Meter connectedMeter, S4_DeviceCommandsNFC nfcCMD)
    {
      this.connectedMeter = connectedMeter;
      this.meterMemory = connectedMeter.meterMemory;
      this.SetLogFilePathByExtension();
      this.nfcCMD = nfcCMD;
      this.uint1_address = this.meterMemory.GetParameterAddress(S4_Params.unit1);
      this.uint2_address = this.meterMemory.GetParameterAddress(S4_Params.unit2);
      this.structSize = this.uint2_address - this.uint1_address;
      if (this.structSize < 60U)
        throw new Exception("Illegal tdcData struct size.");
      if (this.meterMemory.IsParameterInMap(S4_Params.unitsStorageTime))
      {
        this.unitsStorageTimeAddressRange = this.meterMemory.GetAddressRange(S4_Params.unitsStorageTime.ToString());
        this.tdcAddressRange1Chanals = new AddressRange(this.uint2_address, this.structSize + this.unitsStorageTimeAddressRange.ByteSize);
        this.tdcAddressRange2Chanals = new AddressRange(this.uint1_address, this.structSize * 2U + this.unitsStorageTimeAddressRange.ByteSize);
      }
      else
      {
        this.tdcAddressRange1Chanals = new AddressRange(this.uint2_address, this.structSize);
        this.tdcAddressRange2Chanals = new AddressRange(this.uint1_address, this.structSize * 2U);
      }
    }

    private void SetLogFilePathByExtension(string fileExtension = null)
    {
      string str = "LogTdc_" + this.connectedMeter.deviceIdentification.MeterID.ToString();
      if (fileExtension != null && fileExtension != string.Empty)
        str = str + "_" + fileExtension;
      string path2 = Path.Combine("ZENNER", "GMM", "LoggData", str + ".csv");
      this.LogFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), path2);
    }

    public string HeaderToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Time;  " + TDC_VALUENAMES.flowWithoutCor.ToString() + "; 2; " + TDC_VALUENAMES.flowWithCor.ToString() + "; 2; " + TDC_VALUENAMES.speedOfSound.ToString() + "; 2; " + TDC_VALUENAMES.tofDiffAvg.ToString() + "; 2; " + TDC_VALUENAMES.calibration.ToString() + "; 2; " + TDC_VALUENAMES.usedTofDiffAvg.ToString() + "; 2; " + TDC_VALUENAMES.avgDn.ToString() + "; 2; " + TDC_VALUENAMES.avgUp.ToString() + "; 2; " + TDC_VALUENAMES.t1Avg.ToString() + "; 2; " + TDC_VALUENAMES.t3Avg.ToString() + "; 2; " + TDC_VALUENAMES.intStatusReg.ToString() + "; 2; " + TDC_VALUENAMES.temperature.ToString() + "; 2; " + TDC_VALUENAMES.errorFlags.ToString() + "; 2; " + TDC_VALUENAMES.wvrDnReal.ToString() + "; 2; " + TDC_VALUENAMES.wvrDnIdeal.ToString() + "; 2; " + TDC_VALUENAMES.wvrUpReal.ToString() + "; 2; " + TDC_VALUENAMES.wvrUpIdeal.ToString() + "; 2; " + TDC_VALUENAMES.tofCycles.ToString() + "; 2; " + TDC_VALUENAMES.tempCycles.ToString() + "; 2; " + TDC_VALUENAMES.cOffsetUp.ToString() + "; 2; " + TDC_VALUENAMES.cOffsetDn.ToString() + "; 2; " + TDC_VALUENAMES.isUnit2.ToString() + "; 2; " + TDC_VALUENAMES.wvrDnUsed.ToString() + "; 2; " + TDC_VALUENAMES.wvrUpUsed.ToString() + "; 2; ");
      if (this.structSize > 60U)
        stringBuilder.Append(TDC_VALUENAMES.latestTofCycles.ToString() + "; 2; " + TDC_VALUENAMES.latestTofDiffValue.ToString() + "; 2; " + TDC_VALUENAMES.latestTofDiffSum.ToString() + "; 2; " + TDC_VALUENAMES.latestTofDiffMean.ToString() + "; 2; " + TDC_VALUENAMES.latestTofDiffAvgNotUsed.ToString() + "; 2; " + TDC_VALUENAMES.latestUsedCycles.ToString() + "; 2; ");
      stringBuilder.AppendLine();
      return stringBuilder.ToString();
    }

    public void BuildHeaderLine(TextLineBuilder lineBuilder)
    {
      lineBuilder.AddFieldValue("Time");
      this.BuildSingleHeaderLine(lineBuilder);
      if (!this.twoUsChanelsUsed)
        return;
      lineBuilder.AddField(" -2- ");
      this.BuildSingleHeaderLine(lineBuilder);
    }

    private void BuildSingleHeaderLine(TextLineBuilder lineBuilder)
    {
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.flowWithoutCor.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.flowWithCor.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.speedOfSound.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.tofDiffAvg.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.calibration.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.usedTofDiffAvg.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.avgDn.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.avgUp.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.t1Avg.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.t3Avg.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.intStatusReg.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.temperature.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.errorFlags.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.wvrDnReal.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.wvrDnIdeal.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.wvrUpReal.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.wvrUpIdeal.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.tofCycles.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.tempCycles.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.cOffsetUp.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.cOffsetDn.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.isUnit2.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.wvrDnUsed.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.wvrUpUsed.ToString()]);
      if (this.structSize > 60U)
      {
        lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.latestTofCycles.ToString()]);
        lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.latestTofDiffValue.ToString()]);
        lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.latestTofDiffSum.ToString()]);
        lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.latestTofDiffMean.ToString()]);
        lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.latestTofDiffAvgNotUsed.ToString()]);
        lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.latestUsedCycles.ToString()]);
      }
      if (this.structSize != 128U)
        return;
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.highSampledNextValue.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.latestDummy2.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.latestDummy3.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.highSampledTofDiffList.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.highSampledWvrUpList.ToString()]);
      lineBuilder.AddFieldValue(S4_TDC_Internals.HeaderLineAbbreviations[TDC_VALUENAMES.highSampledWvrDnList.ToString()]);
    }

    public override string ToString()
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.Append(DateTime.Now.ToString("HH:mm:ss.fff") + ";   ");
      StringBuilder stringBuilder2 = stringBuilder1;
      float flowWithoutCor = this.tdcUnit_1.flowWithoutCor;
      string str1 = flowWithoutCor.ToString();
      flowWithoutCor = this.tdcUnit_2.flowWithoutCor;
      string str2 = flowWithoutCor.ToString();
      string str3 = str1 + "; " + str2 + ";   ";
      stringBuilder2.Append(str3);
      StringBuilder stringBuilder3 = stringBuilder1;
      float flowWithCor = this.tdcUnit_1.flowWithCor;
      string str4 = flowWithCor.ToString();
      flowWithCor = this.tdcUnit_2.flowWithCor;
      string str5 = flowWithCor.ToString();
      string str6 = str4 + "; " + str5 + ";   ";
      stringBuilder3.Append(str6);
      StringBuilder stringBuilder4 = stringBuilder1;
      float speedOfSound = this.tdcUnit_1.speedOfSound;
      string str7 = speedOfSound.ToString();
      speedOfSound = this.tdcUnit_2.speedOfSound;
      string str8 = speedOfSound.ToString();
      string str9 = str7 + "; " + str8 + ";   ";
      stringBuilder4.Append(str9);
      StringBuilder stringBuilder5 = stringBuilder1;
      int tofDiffAvg = this.tdcUnit_1.tofDiffAvg;
      string str10 = tofDiffAvg.ToString();
      tofDiffAvg = this.tdcUnit_2.tofDiffAvg;
      string str11 = tofDiffAvg.ToString();
      string str12 = str10 + "; " + str11 + ";   ";
      stringBuilder5.Append(str12);
      StringBuilder stringBuilder6 = stringBuilder1;
      uint calibration = this.tdcUnit_1.calibration;
      string str13 = calibration.ToString();
      calibration = this.tdcUnit_2.calibration;
      string str14 = calibration.ToString();
      string str15 = str13 + "; " + str14 + ";   ";
      stringBuilder6.Append(str15);
      StringBuilder stringBuilder7 = stringBuilder1;
      int usedTofDiffAvg = this.tdcUnit_1.usedTofDiffAvg;
      string str16 = usedTofDiffAvg.ToString();
      usedTofDiffAvg = this.tdcUnit_2.usedTofDiffAvg;
      string str17 = usedTofDiffAvg.ToString();
      string str18 = str16 + "; " + str17 + ";   ";
      stringBuilder7.Append(str18);
      StringBuilder stringBuilder8 = stringBuilder1;
      uint avgDn = this.tdcUnit_1.avgDn;
      string str19 = avgDn.ToString();
      avgDn = this.tdcUnit_2.avgDn;
      string str20 = avgDn.ToString();
      string str21 = str19 + "; " + str20 + ";   ";
      stringBuilder8.Append(str21);
      StringBuilder stringBuilder9 = stringBuilder1;
      uint avgUp = this.tdcUnit_1.avgUp;
      string str22 = avgUp.ToString();
      avgUp = this.tdcUnit_2.avgUp;
      string str23 = avgUp.ToString();
      string str24 = str22 + "; " + str23 + ";   ";
      stringBuilder9.Append(str24);
      StringBuilder stringBuilder10 = stringBuilder1;
      uint t1Avg = this.tdcUnit_1.t1Avg;
      string str25 = t1Avg.ToString();
      t1Avg = this.tdcUnit_2.t1Avg;
      string str26 = t1Avg.ToString();
      string str27 = str25 + "; " + str26 + ";   ";
      stringBuilder10.Append(str27);
      StringBuilder stringBuilder11 = stringBuilder1;
      uint t3Avg = this.tdcUnit_1.t3Avg;
      string str28 = t3Avg.ToString();
      t3Avg = this.tdcUnit_2.t3Avg;
      string str29 = t3Avg.ToString();
      string str30 = str28 + "; " + str29 + ";   ";
      stringBuilder11.Append(str30);
      StringBuilder stringBuilder12 = stringBuilder1;
      ushort intStatusReg = this.tdcUnit_1.intStatusReg;
      string str31 = intStatusReg.ToString();
      intStatusReg = this.tdcUnit_2.intStatusReg;
      string str32 = intStatusReg.ToString();
      string str33 = str31 + "; " + str32 + ";   ";
      stringBuilder12.Append(str33);
      StringBuilder stringBuilder13 = stringBuilder1;
      short temperature = this.tdcUnit_1.temperature;
      string str34 = temperature.ToString();
      temperature = this.tdcUnit_2.temperature;
      string str35 = temperature.ToString();
      string str36 = str34 + "; " + str35 + ";   ";
      stringBuilder13.Append(str36);
      StringBuilder stringBuilder14 = stringBuilder1;
      ushort errorFlags = this.tdcUnit_1.errorFlags;
      string str37 = errorFlags.ToString();
      errorFlags = this.tdcUnit_2.errorFlags;
      string str38 = errorFlags.ToString();
      string str39 = str37 + "; " + str38 + ";   ";
      stringBuilder14.Append(str39);
      StringBuilder stringBuilder15 = stringBuilder1;
      byte wvrDnReal = this.tdcUnit_1.wvrDnReal;
      string str40 = wvrDnReal.ToString();
      wvrDnReal = this.tdcUnit_2.wvrDnReal;
      string str41 = wvrDnReal.ToString();
      string str42 = str40 + "; " + str41 + ";   ";
      stringBuilder15.Append(str42);
      StringBuilder stringBuilder16 = stringBuilder1;
      byte wvrDnIdeal = this.tdcUnit_1.wvrDnIdeal;
      string str43 = wvrDnIdeal.ToString();
      wvrDnIdeal = this.tdcUnit_2.wvrDnIdeal;
      string str44 = wvrDnIdeal.ToString();
      string str45 = str43 + "; " + str44 + ";   ";
      stringBuilder16.Append(str45);
      StringBuilder stringBuilder17 = stringBuilder1;
      byte wvrUpReal = this.tdcUnit_1.wvrUpReal;
      string str46 = wvrUpReal.ToString();
      wvrUpReal = this.tdcUnit_2.wvrUpReal;
      string str47 = wvrUpReal.ToString();
      string str48 = str46 + "; " + str47 + ";   ";
      stringBuilder17.Append(str48);
      StringBuilder stringBuilder18 = stringBuilder1;
      byte wvrUpIdeal = this.tdcUnit_1.wvrUpIdeal;
      string str49 = wvrUpIdeal.ToString();
      wvrUpIdeal = this.tdcUnit_2.wvrUpIdeal;
      string str50 = wvrUpIdeal.ToString();
      string str51 = str49 + "; " + str50 + ";   ";
      stringBuilder18.Append(str51);
      StringBuilder stringBuilder19 = stringBuilder1;
      byte tofCycles = this.tdcUnit_1.tofCycles;
      string str52 = tofCycles.ToString();
      tofCycles = this.tdcUnit_2.tofCycles;
      string str53 = tofCycles.ToString();
      string str54 = str52 + "; " + str53 + ";   ";
      stringBuilder19.Append(str54);
      StringBuilder stringBuilder20 = stringBuilder1;
      byte tempCycles = this.tdcUnit_1.tempCycles;
      string str55 = tempCycles.ToString();
      tempCycles = this.tdcUnit_2.tempCycles;
      string str56 = tempCycles.ToString();
      string str57 = str55 + "; " + str56 + ";   ";
      stringBuilder20.Append(str57);
      StringBuilder stringBuilder21 = stringBuilder1;
      ushort cOffsetUp = this.tdcUnit_1.cOffsetUp;
      string str58 = cOffsetUp.ToString();
      cOffsetUp = this.tdcUnit_2.cOffsetUp;
      string str59 = cOffsetUp.ToString();
      string str60 = str58 + "; " + str59 + ";   ";
      stringBuilder21.Append(str60);
      StringBuilder stringBuilder22 = stringBuilder1;
      ushort cOffsetDn = this.tdcUnit_1.cOffsetDn;
      string str61 = cOffsetDn.ToString();
      cOffsetDn = this.tdcUnit_2.cOffsetDn;
      string str62 = cOffsetDn.ToString();
      string str63 = str61 + "; " + str62 + ";   ";
      stringBuilder22.Append(str63);
      StringBuilder stringBuilder23 = stringBuilder1;
      byte isUnit2 = this.tdcUnit_1.isUnit2;
      string str64 = isUnit2.ToString();
      isUnit2 = this.tdcUnit_2.isUnit2;
      string str65 = isUnit2.ToString();
      string str66 = str64 + "; " + str65 + ";   ";
      stringBuilder23.Append(str66);
      StringBuilder stringBuilder24 = stringBuilder1;
      byte wvrDnUsed = this.tdcUnit_1.wvrDnUsed;
      string str67 = wvrDnUsed.ToString();
      wvrDnUsed = this.tdcUnit_2.wvrDnUsed;
      string str68 = wvrDnUsed.ToString();
      string str69 = str67 + "; " + str68 + ";   ";
      stringBuilder24.Append(str69);
      StringBuilder stringBuilder25 = stringBuilder1;
      byte wvrUpUsed = this.tdcUnit_1.wvrUpUsed;
      string str70 = wvrUpUsed.ToString();
      wvrUpUsed = this.tdcUnit_2.wvrUpUsed;
      string str71 = wvrUpUsed.ToString();
      string str72 = str70 + "; " + str71 + ";   ";
      stringBuilder25.Append(str72);
      if (this.structSize > 60U)
      {
        stringBuilder1.Append(this.tdcUnit_1.latestTofCycles.ToString() + "; " + this.tdcUnit_2.latestTofCycles.ToString() + ";   ");
        StringBuilder stringBuilder26 = stringBuilder1;
        string str73 = this.tdcUnit_1.latestTofDiffValue.ToString();
        int num = this.tdcUnit_2.latestTofDiffValue;
        string str74 = num.ToString();
        string str75 = str73 + "; " + str74 + ";   ";
        stringBuilder26.Append(str75);
        StringBuilder stringBuilder27 = stringBuilder1;
        num = this.tdcUnit_1.latestTofDiffSum;
        string str76 = num.ToString();
        num = this.tdcUnit_2.latestTofDiffSum;
        string str77 = num.ToString();
        string str78 = str76 + "; " + str77 + ";   ";
        stringBuilder27.Append(str78);
        StringBuilder stringBuilder28 = stringBuilder1;
        num = this.tdcUnit_1.latestTofDiffMean;
        string str79 = num.ToString();
        num = this.tdcUnit_2.latestTofDiffMean;
        string str80 = num.ToString();
        string str81 = str79 + "; " + str80 + ";   ";
        stringBuilder28.Append(str81);
        StringBuilder stringBuilder29 = stringBuilder1;
        num = this.tdcUnit_1.latestTofDiffAvgNotUsed;
        string str82 = num.ToString();
        num = this.tdcUnit_2.latestTofDiffAvgNotUsed;
        string str83 = num.ToString();
        string str84 = str82 + "; " + str83 + ";   ";
        stringBuilder29.Append(str84);
        stringBuilder1.Append(this.tdcUnit_1.latestUsedCycles.ToString() + "; " + this.tdcUnit_2.latestUsedCycles.ToString() + ";   ");
      }
      stringBuilder1.AppendLine();
      return stringBuilder1.ToString();
    }

    public void BuildValueLine(TextLineBuilder lineBuilder)
    {
      lineBuilder.AddFieldValue(DateTime.Now.ToString("HH:mm:ss.fff"));
      this.tdcUnit_2.BuildValueLine(lineBuilder);
      if (!this.twoUsChanelsUsed)
        return;
      lineBuilder.AddField(" -2- ");
      this.tdcUnit_1.BuildValueLine(lineBuilder);
    }

    internal bool IsUltrasonicOk()
    {
      return this.tdcUnit_2.wvrDnReal != byte.MaxValue && this.tdcUnit_2.tofCycles == (byte) 8 && (!this.twoUsChanelsUsed || this.tdcUnit_1.wvrDnReal != byte.MaxValue && this.tdcUnit_1.tofCycles == (byte) 8);
    }

    internal UltrasonicState GetUltrasonicState()
    {
      UltrasonicState ultrasonicState = new UltrasonicState();
      if (this.twoUsChanelsUsed)
      {
        ultrasonicState.TransducerPair2State = this.GetTransducerPairState(this.tdcUnit_2);
        ultrasonicState.TransducerPair1State = this.GetTransducerPairState(this.tdcUnit_1);
      }
      else
        ultrasonicState.TransducerPair1State = this.GetTransducerPairState(this.tdcUnit_2);
      return ultrasonicState;
    }

    private TransducerPairState GetTransducerPairState(tdcData tdcUnit)
    {
      TransducerPairState transducerPairState = new TransducerPairState();
      transducerPairState.SuccessfulCycles = (uint) tdcUnit.tofCycles;
      transducerPairState.UpCounts = tdcUnit.avgUp != uint.MaxValue ? tdcUnit.avgUp : 0U;
      transducerPairState.DownCounts = tdcUnit.avgDn != uint.MaxValue ? tdcUnit.avgDn : 0U;
      int num = transducerPairState.UpCounts == 0U ? 1 : (transducerPairState.DownCounts == 0U ? 1 : 0);
      transducerPairState.DiffCounts = num == 0 ? tdcUnit.usedTofDiffAvg : 0;
      if (tdcUnit.wvrDnReal == byte.MaxValue)
      {
        transducerPairState.CycleErrorsInPercent = 100.0;
      }
      else
      {
        transducerPairState.CycleErrorsInPercent = (double) ((8 - (int) tdcUnit.tofCycles) * 100 / 8);
        if (transducerPairState.CycleErrorsInPercent == 0.0)
          transducerPairState.Perfect = true;
      }
      return transducerPairState;
    }

    public async void TDC_ChangeDirection(ProgressHandler progress, CancellationToken cancelToken)
    {
      this.ConfigFlagRegister = this.meterMemory.UsedParametersByName[S4_Params.ConfigFlagRegister.ToString()];
      await this.nfcCMD.CommonNfcCommands.ReadMemoryAsync(this.ConfigFlagRegister.AddressRange, (DeviceMemory) this.meterMemory, progress, cancelToken);
      this.ConfigFlagRegisterValue = this.meterMemory.GetParameterValue<ushort>(S4_Params.ConfigFlagRegister);
      this.ConfigFlagRegisterValue ^= (ushort) 8;
      this.meterMemory.SetParameterValue<ushort>(S4_Params.ConfigFlagRegister, this.ConfigFlagRegisterValue);
      await this.nfcCMD.CommonNfcCommands.WriteMemoryAsync(this.ConfigFlagRegister.AddressRange, (DeviceMemory) this.meterMemory, progress, cancelToken);
    }

    public async Task ReadConfig(ProgressHandler progress, CancellationToken cancelToken)
    {
      if (this.ConfigFlagRegister != null)
        return;
      this.ConfigFlagRegister = this.meterMemory.UsedParametersByName[S4_Params.ConfigFlagRegister.ToString()];
      await this.nfcCMD.CommonNfcCommands.ReadMemoryAsync(this.ConfigFlagRegister.AddressRange, (DeviceMemory) this.meterMemory, progress, cancelToken);
      this.ConfigFlagRegisterValue = this.meterMemory.GetParameterValue<ushort>(S4_Params.ConfigFlagRegister);
      this.tdcUnit_1 = new tdcData((DeviceMemory) this.meterMemory, this.uint1_address, this.structSize);
      this.tdcUnit_2 = new tdcData((DeviceMemory) this.meterMemory, this.uint2_address, this.structSize);
    }

    public async Task<UltrasonicState> ReadAndGetUltrasonicStateAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await this.ReadConfig(progress, cancelToken);
      if (this.twoUsChanelsUsed)
      {
        await this.nfcCMD.CommonNfcCommands.ReadMemoryAsync(this.tdcAddressRange2Chanals, (DeviceMemory) this.meterMemory, progress, cancelToken);
        this.tdcUnit_1 = new tdcData((DeviceMemory) this.meterMemory, this.uint1_address, this.structSize);
        this.tdcUnit_2 = new tdcData((DeviceMemory) this.meterMemory, this.uint2_address, this.structSize);
        this.tdcUnit_1.RefreshData();
        this.tdcUnit_2.RefreshData();
      }
      else
      {
        await this.nfcCMD.CommonNfcCommands.ReadMemoryAsync(this.tdcAddressRange1Chanals, (DeviceMemory) this.meterMemory, progress, cancelToken);
        this.tdcUnit_2 = new tdcData((DeviceMemory) this.meterMemory, this.uint2_address, this.structSize);
        this.tdcUnit_2.RefreshData();
      }
      return this.GetUltrasonicState();
    }

    public async Task<EmcTestData> ReadAndGetEmcTestDataAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      int parameterIndex = this.meterMemory.UsedParametersByName.IndexOfKey(S4_Params.emcTestData.ToString());
      if (parameterIndex < 0)
        throw new Exception("This firmware doesn't support the EMC test");
      Parameter32bit emcTestData = this.meterMemory.UsedParametersByName.Values[parameterIndex];
      await this.nfcCMD.CommonNfcCommands.ReadMemoryAsync(emcTestData.AddressRange, (DeviceMemory) this.meterMemory, progress, cancelToken);
      EmcTestData result = new EmcTestData();
      uint parameterAddress = emcTestData.AddressRange.StartAddress;
      result.VolQmSum = this.meterMemory.GetValue<double>(parameterAddress);
      parameterAddress += 8U;
      result.Flow = this.meterMemory.GetValue<float>(parameterAddress);
      parameterAddress += 4U;
      result.CycleTime = this.meterMemory.GetValue<float>(parameterAddress);
      parameterAddress += 4U;
      result.CycleCounter = this.meterMemory.GetValue<uint>(parameterAddress);
      parameterAddress += 4U;
      result.TimeStamp = FirmwareDateTimeSupport.GetDateTimeFromMemoryBCD(parameterAddress, (DeviceMemory) this.meterMemory);
      EmcTestData emcTestDataAsync = result;
      emcTestData = (Parameter32bit) null;
      result = (EmcTestData) null;
      return emcTestDataAsync;
    }

    private async Task ReadLevelTestDataAsync(
      UltrasonicTestResults ur,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      SortedList<string, Parameter32bit> parametersByName1 = this.meterMemory.UsedParametersByName;
      S4_Params s4Params = S4_Params.US_Leveltest_Offset;
      string key1 = s4Params.ToString();
      if (parametersByName1.IndexOfKey(key1) < 0)
        return;
      SortedList<string, Parameter32bit> parametersByName2 = this.meterMemory.UsedParametersByName;
      s4Params = S4_Params.US_Leveltest_Offset;
      string key2 = s4Params.ToString();
      Parameter32bit US_Leveltest_Offset = parametersByName2[key2];
      AddressRange cRange = US_Leveltest_Offset.AddressRange.Clone();
      cRange.ByteSize = 12U;
      await this.nfcCMD.CommonNfcCommands.ReadMemoryAsync(cRange, (DeviceMemory) this.meterMemory, progress, cancelToken);
      uint workAdr = cRange.StartAddress;
      byte[] offsetData = this.meterMemory.GetData(cRange);
      for (int i = 0; i < 6; ++i)
        ur.LevelOffsets[i] = BitConverter.ToUInt16(offsetData, 2 * i);
      SortedList<string, Parameter32bit> parametersByName3 = this.meterMemory.UsedParametersByName;
      s4Params = S4_Params.tdcLevelTestUnit1;
      string key3 = s4Params.ToString();
      if (parametersByName3.IndexOfKey(key3) >= 0)
      {
        SortedList<string, Parameter32bit> parametersByName4 = this.meterMemory.UsedParametersByName;
        s4Params = S4_Params.tdcLevelTestUnit1;
        string key4 = s4Params.ToString();
        Parameter32bit tdcLevelTestUnit1Start = parametersByName4[key4];
        SortedList<string, Parameter32bit> parametersByName5 = this.meterMemory.UsedParametersByName;
        s4Params = S4_Params.tdcLevelTestUnit2;
        string key5 = s4Params.ToString();
        AddressRange levelAddressRange;
        byte[] levelTestData;
        int offset;
        if (parametersByName5.IndexOfKey(key5) >= 0)
        {
          SortedList<string, Parameter32bit> parametersByName6 = this.meterMemory.UsedParametersByName;
          s4Params = S4_Params.tdcLevelTestUnit2;
          string key6 = s4Params.ToString();
          Parameter32bit tdcLevelTestUnit2Start = parametersByName6[key6];
          if (tdcLevelTestUnit1Start.Address > tdcLevelTestUnit2Start.Address)
            throw new Exception("Illegal map order for tdcLevelTestUnit parameters");
          levelAddressRange = new AddressRange(tdcLevelTestUnit1Start.Address, 128U);
          await this.nfcCMD.CommonNfcCommands.ReadMemoryAsync(levelAddressRange, (DeviceMemory) this.meterMemory, progress, cancelToken);
          levelTestData = this.meterMemory.GetData(levelAddressRange);
          ur.SUS_LevelData = new TdcLevelTestData();
          offset = 64;
          Buffer.BlockCopy((Array) levelTestData, offset, (Array) ur.SUS_LevelData.cntDn, 0, 24);
          offset += 24;
          Buffer.BlockCopy((Array) levelTestData, offset, (Array) ur.SUS_LevelData.cntUp, 0, 24);
          offset += 24;
          Buffer.BlockCopy((Array) levelTestData, offset, (Array) ur.SUS_LevelData.wvrUp, 0, 6);
          offset += 6;
          Buffer.BlockCopy((Array) levelTestData, offset, (Array) ur.SUS_LevelData.wvrDn, 0, 6);
          offset += 6;
          ur.SUS_LevelData.measCounter = levelTestData[offset++];
          ur.SUS_LevelData.offsetOldUp = levelTestData[offset++];
          ur.SUS_LevelData.offsetOldDn = levelTestData[offset++];
          tdcLevelTestUnit2Start = (Parameter32bit) null;
        }
        else
        {
          ur.SUS_LevelData = (TdcLevelTestData) null;
          levelAddressRange = new AddressRange(tdcLevelTestUnit1Start.Address, 64U);
          this.meterMemory.GarantMemoryAvailable(levelAddressRange);
          await this.nfcCMD.CommonNfcCommands.ReadMemoryAsync(levelAddressRange, (DeviceMemory) this.meterMemory, progress, cancelToken);
          levelTestData = this.meterMemory.GetData(levelAddressRange);
        }
        ur.LevelData = new TdcLevelTestData();
        offset = 0;
        Buffer.BlockCopy((Array) levelTestData, offset, (Array) ur.LevelData.cntDn, 0, 24);
        offset += 24;
        Buffer.BlockCopy((Array) levelTestData, offset, (Array) ur.LevelData.cntUp, 0, 24);
        offset += 24;
        Buffer.BlockCopy((Array) levelTestData, offset, (Array) ur.LevelData.wvrUp, 0, 6);
        offset += 6;
        Buffer.BlockCopy((Array) levelTestData, offset, (Array) ur.LevelData.wvrDn, 0, 6);
        offset += 6;
        ur.LevelData.measCounter = levelTestData[offset++];
        ur.LevelData.offsetOldUp = levelTestData[offset++];
        ur.LevelData.offsetOldDn = levelTestData[offset++];
        tdcLevelTestUnit1Start = (Parameter32bit) null;
        levelAddressRange = (AddressRange) null;
        levelTestData = (byte[]) null;
      }
      ur.Receiver1Amplitude = 0.0;
      ur.Receiver2Amplitude = 0.0;
      ur.SUS_Receiver1Amplitude = 0.0;
      ur.SUS_Receiver2Amplitude = 0.0;
      for (int i = 0; i < 6; ++i)
      {
        double amp = (double) S4_TDC_Internals.GetLevelOffset(ur.LevelOffsets[i], (uint) ur.LevelData.wvrUp[i]);
        if (amp != 0.0)
          ur.Receiver1Amplitude = amp;
        amp = (double) S4_TDC_Internals.GetLevelOffset(ur.LevelOffsets[i], (uint) ur.LevelData.wvrDn[i]);
        if (amp != 0.0)
          ur.Receiver2Amplitude = amp;
        if (ur.SUS_LevelData != null)
        {
          amp = (double) S4_TDC_Internals.GetLevelOffset(ur.LevelOffsets[i], (uint) ur.SUS_LevelData.wvrUp[i]);
          if (amp != 0.0)
            ur.SUS_Receiver1Amplitude = amp;
          amp = (double) S4_TDC_Internals.GetLevelOffset(ur.LevelOffsets[i], (uint) ur.SUS_LevelData.wvrDn[i]);
          if (amp != 0.0)
            ur.SUS_Receiver2Amplitude = amp;
        }
      }
      US_Leveltest_Offset = (Parameter32bit) null;
      cRange = (AddressRange) null;
      offsetData = (byte[]) null;
    }

    private async Task ReadLevelTestData2Async(
      UltrasonicTestResults ur,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      SortedList<string, Parameter32bit> parametersByName1 = this.meterMemory.UsedParametersByName;
      S4_Params s4Params = S4_Params.tdcUsltUnit1;
      string key1 = s4Params.ToString();
      if (parametersByName1.IndexOfKey(key1) < 0)
        return;
      SortedList<string, Parameter32bit> parametersByName2 = this.meterMemory.UsedParametersByName;
      s4Params = S4_Params.tdcUsltUnit1;
      string key2 = s4Params.ToString();
      Parameter32bit tdcUsltUnit1 = parametersByName2[key2];
      AddressRange cRange = tdcUsltUnit1.AddressRange.Clone();
      if (cRange.ByteSize != 20U)
        throw new Exception("Not expected tdcUsltUnit1 ByteSize: " + tdcUsltUnit1.AddressRange.ByteSize.ToString());
      cRange.ByteSize *= 2U;
      await this.nfcCMD.CommonNfcCommands.ReadMemoryAsync(cRange, (DeviceMemory) this.meterMemory, progress, cancelToken);
      uint workAdr = cRange.StartAddress;
      byte[] tdcUsltUnitData = this.meterMemory.GetData(cRange);
      await this.nfcCMD.CommonNfcCommands.ReadMemoryAsync(this.meterMemory.GetParameterAddressRange(S4_Params.Uslt_Test_Numbers), (DeviceMemory) this.meterMemory, progress, cancelToken);
      await this.nfcCMD.CommonNfcCommands.ReadMemoryAsync(this.meterMemory.GetParameterAddressRange(S4_Params.Uslt_Offset), (DeviceMemory) this.meterMemory, progress, cancelToken);
      byte Uslt_Test_Numbers = this.meterMemory.GetParameterValue<byte>(S4_Params.Uslt_Test_Numbers);
      byte Uslt_Offset = this.meterMemory.GetParameterValue<byte>(S4_Params.Uslt_Offset);
      S4_TDC_Internals.tdcUsltData tdcUsltUnit2struct = new S4_TDC_Internals.tdcUsltData(tdcUsltUnitData, 0);
      S4_TDC_Internals.tdcUsltData tdcUsltUnit1struct = new S4_TDC_Internals.tdcUsltData(tdcUsltUnitData, 20);
      ur.LevelData = new TdcLevelTestData();
      ur.SUS_LevelData = new TdcLevelTestData();
      ur.Receiver1Amplitude = 0.0;
      ur.Receiver2Amplitude = 0.0;
      ur.SUS_Receiver1Amplitude = 0.0;
      ur.SUS_Receiver2Amplitude = 0.0;
      for (int i = 0; i < (int) Uslt_Test_Numbers; ++i)
      {
        ur.LevelOffsets[i] = (ushort) Uslt_Offset;
        ur.LevelData.wvrUp = tdcUsltUnit1struct.wvrUp;
        ur.LevelData.wvrDn = tdcUsltUnit1struct.wvrDn;
        ur.SUS_LevelData.wvrUp = tdcUsltUnit2struct.wvrUp;
        ur.SUS_LevelData.wvrDn = tdcUsltUnit2struct.wvrDn;
        double ReceiverAmplitude = (double) S4_TDC_Internals.GetLevelOffset(ur.LevelOffsets[i], (uint) ur.LevelData.wvrUp[i]);
        if (ReceiverAmplitude > ur.Receiver1Amplitude)
          ur.Receiver1Amplitude = ReceiverAmplitude;
        ReceiverAmplitude = (double) S4_TDC_Internals.GetLevelOffset(ur.LevelOffsets[i], (uint) ur.LevelData.wvrDn[i]);
        if (ReceiverAmplitude > ur.Receiver2Amplitude)
          ur.Receiver2Amplitude = ReceiverAmplitude;
        ReceiverAmplitude = (double) S4_TDC_Internals.GetLevelOffset(ur.LevelOffsets[i], (uint) ur.SUS_LevelData.wvrUp[i]);
        if (ReceiverAmplitude > ur.SUS_Receiver1Amplitude)
          ur.SUS_Receiver1Amplitude = ReceiverAmplitude;
        ReceiverAmplitude = (double) S4_TDC_Internals.GetLevelOffset(ur.LevelOffsets[i], (uint) ur.SUS_LevelData.wvrDn[i]);
        if (ReceiverAmplitude > ur.SUS_Receiver2Amplitude)
          ur.SUS_Receiver2Amplitude = ReceiverAmplitude;
      }
      ur.LevelData.measCounter = tdcUsltUnit1struct.measCounter;
      ur.LevelData.offsetOldUp = tdcUsltUnit1struct.offsetOldUp;
      ur.LevelData.offsetOldDn = tdcUsltUnit1struct.offsetOldDn;
      ur.SUS_LevelData.measCounter = tdcUsltUnit2struct.measCounter;
      ur.SUS_LevelData.offsetOldUp = tdcUsltUnit2struct.offsetOldUp;
      ur.SUS_LevelData.offsetOldDn = tdcUsltUnit2struct.offsetOldDn;
      tdcUsltUnit1 = (Parameter32bit) null;
      cRange = (AddressRange) null;
      tdcUsltUnitData = (byte[]) null;
      tdcUsltUnit2struct = (S4_TDC_Internals.tdcUsltData) null;
      tdcUsltUnit1struct = (S4_TDC_Internals.tdcUsltData) null;
    }

    internal static int GetLevelOffset(ushort levelOffsetValue, uint countValue)
    {
      if (countValue > 128U)
        return 0;
      if (countValue < 5U)
        return (int) levelOffsetValue;
      double d = Math.PI / 2.0 * (double) countValue / 128.0;
      double num1 = d / Math.PI * 180.0;
      double num2 = Math.Cos(d);
      return (int) ((double) levelOffsetValue / num2);
    }

    public async Task<UltrasonicTestResults> RunUltrasonicTestAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      UltrasonicState us = await this.ReadAndGetUltrasonicStateAsync(progress, cancelToken);
      UltrasonicTestResults ur = new UltrasonicTestResults(us);
      if (new FirmwareVersion(this.meterMemory.FirmwareVersion) >= (object) "1.2.2 IUW")
      {
        await this.nfcCMD.CommonNfcCommands.SetModeAsync(S4_DeviceModes.TestModePrepared, progress, cancelToken);
        await this.nfcCMD.CommonNfcCommands.SetModeAsync(S4_DeviceModes.TdcLevelTest, progress, cancelToken, new byte[1]
        {
          (byte) 1
        });
        DateTime timeoutTime = DateTime.Now.AddSeconds(20.0);
        for (int i = 1; i < 6; ++i)
        {
          await Task.Delay(1000, cancelToken);
          progress.Report("Wait for test finished: " + i.ToString());
        }
        int i1 = 1;
        while (true)
        {
          S4_SystemState deviceState = await this.nfcCMD.GetDeviceStatesAsync(progress, cancelToken);
          progress.Report("Device state polling: " + i1.ToString());
          if (deviceState.TestState != Test_State.TEST_STATE_STOPPED)
          {
            if (!(timeoutTime < DateTime.Now))
            {
              await Task.Delay(1000, cancelToken);
              deviceState = (S4_SystemState) null;
              ++i1;
            }
            else
              break;
          }
          else
            goto label_12;
        }
        throw new TimeoutException("Level test timeout");
label_12:
        await this.nfcCMD.CommonNfcCommands.SetModeAsync(S4_DeviceModes.OperationMode, progress, cancelToken);
        await this.ReadLevelTestData2Async(ur, progress, cancelToken);
        ur.ResonatorCalibration = (double) this.tdcUnit_2.calibration / 65536.0;
        ur.Temperature = (double) this.tdcUnit_2.temperature / 100.0;
        if (((int) this.tdcUnit_2.errorFlags & 240) == 0)
          ur.TemperatureMeasuringOK = true;
        ur.TemperatureReferenceCounts = this.tdcUnit_2.t3Avg;
        ur.TemperatureSensorCounts = this.tdcUnit_2.t1Avg;
        if (this.twoUsChanelsUsed)
          ur.SUS_ResonatorCalibration = (double) this.tdcUnit_1.calibration / 65536.0;
      }
      else if (new FirmwareVersion(this.meterMemory.FirmwareVersion) >= (object) "0.3.1 IUW")
      {
        await this.nfcCMD.CommonNfcCommands.SetModeAsync(S4_DeviceModes.TestModePrepared, progress, cancelToken);
        await this.nfcCMD.CommonNfcCommands.SetModeAsync(S4_DeviceModes.TdcLevelTest, progress, cancelToken);
        DateTime timeoutTime = DateTime.Now.AddSeconds(20.0);
        await Task.Delay(12000, cancelToken);
        int i = 1;
        while (true)
        {
          S4_SystemState deviceState = await this.nfcCMD.GetDeviceStatesAsync(progress, cancelToken);
          progress.Report("Device state polling: " + i.ToString());
          if (deviceState.TestState != Test_State.TEST_STATE_STOPPED)
          {
            if (!(timeoutTime < DateTime.Now))
            {
              await Task.Delay(1000, cancelToken);
              deviceState = (S4_SystemState) null;
              ++i;
            }
            else
              break;
          }
          else
            goto label_26;
        }
        throw new TimeoutException("Level test timeout");
label_26:
        await this.nfcCMD.CommonNfcCommands.SetModeAsync(S4_DeviceModes.OperationMode, progress, cancelToken);
        await this.ReadLevelTestDataAsync(ur, progress, cancelToken);
        ur.ResonatorCalibration = (double) this.tdcUnit_2.calibration / 65536.0;
        ur.Temperature = (double) this.tdcUnit_2.temperature / 100.0;
        if (((int) this.tdcUnit_2.errorFlags & 240) == 0)
          ur.TemperatureMeasuringOK = true;
        ur.TemperatureReferenceCounts = this.tdcUnit_2.t3Avg;
        ur.TemperatureSensorCounts = this.tdcUnit_2.t1Avg;
        if (this.twoUsChanelsUsed)
          ur.SUS_ResonatorCalibration = (double) this.tdcUnit_1.calibration / 65536.0;
      }
      UltrasonicTestResults ultrasonicTestResults = ur;
      us = (UltrasonicState) null;
      ur = (UltrasonicTestResults) null;
      return ultrasonicTestResults;
    }

    public async Task PrepareEmcTestAsync(
      int cycleSetup,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.EmcTestActive = true;
      this.FirstRefData = (EmcTestData) null;
      this.LastRefData = (EmcTestData) null;
      this.EmcRefFinished = false;
      this.EmcTestFinished = false;
      if (cycleSetup < 1 || cycleSetup > 100)
        throw new Exception("Illegal calculation cycles. [1..100]");
      NLogSupport.GarantLogFileAndLoggerRule("Ultrasonic.txt", "IUW_UltrasonicLogger", NLog.LogLevel.Info);
      StringBuilder testResult = new StringBuilder();
      progress.BaseMessage = "Run EMC test: ";
      string header = "Count".PadLeft(S4_TDC_Internals.columnCharacters[0]) + "Date".PadLeft(S4_TDC_Internals.columnCharacters[1]) + "DeviceTime".PadLeft(S4_TDC_Internals.columnCharacters[2]) + "TimeDiff".PadLeft(S4_TDC_Internals.columnCharacters[3]) + "Flow".PadLeft(S4_TDC_Internals.columnCharacters[4]) + "Volume".PadLeft(S4_TDC_Internals.columnCharacters[5]) + "StartVol".PadLeft(S4_TDC_Internals.columnCharacters[6]) + "VolDiff".PadLeft(S4_TDC_Internals.columnCharacters[7]) + "Flow".PadLeft(S4_TDC_Internals.columnCharacters[8]);
      progress.Report(tag: (object) header);
      S4_TDC_Internals.IUW_UltrasonicLogger.Info(header);
      string startInfo = "******** EMC test. Prepare reference data ... *********";
      progress.Report(tag: (object) startInfo);
      S4_TDC_Internals.IUW_UltrasonicLogger.Info(startInfo);
      while (!cancelToken.IsCancellationRequested && !this.EmcRefFinished)
      {
        try
        {
          EmcTestData emcTestData = await this.ReadAndGetEmcTestDataAsync(progress, cancelToken);
          this.LastReadData = emcTestData;
          emcTestData = (EmcTestData) null;
          if (this.FirstRefData == null)
          {
            this.FirstRefData = this.LastReadData;
            this.LastRefData = this.LastReadData;
          }
          else if ((int) this.LastRefData.CycleCounter != (int) this.LastReadData.CycleCounter)
          {
            this.LastRefData = this.LastReadData;
            this.RefCycles = this.LastRefData.CycleCounter - this.FirstRefData.CycleCounter;
            S4_TDC_Internals.CalcValues cv = this.PrepareTestResultAndGetFlow(testResult, this.LastRefData, this.FirstRefData, progress);
            this.refFlow = cv.flow;
            cv = (S4_TDC_Internals.CalcValues) null;
          }
        }
        catch (TimeoutException ex)
        {
          S4_TDC_Internals.IUW_UltrasonicErrorLogger.Error("Timeout");
        }
        catch (TaskCanceledException ex)
        {
          progress.Report("Canceled");
          testResult = (StringBuilder) null;
          header = (string) null;
          startInfo = (string) null;
          return;
        }
        catch (Exception ex)
        {
          S4_TDC_Internals.IUW_UltrasonicErrorLogger.Error("Exception:" + ex.ToString());
          throw new Exception("PrepareEmcTest", ex);
        }
        await Task.Delay(1500, cancelToken);
      }
      this.EmcTestActive = false;
      testResult = (StringBuilder) null;
      header = (string) null;
      startInfo = (string) null;
    }

    public async Task RunEmcTestAsync(ProgressHandler progress, CancellationToken cancelToken)
    {
      this.EmcTestActive = true;
      this.FirstTestData = (EmcTestData) null;
      this.LastTestData = (EmcTestData) null;
      StringBuilder testResult = new StringBuilder();
      string startInfo = "**************** EMC test started ********************";
      progress.Report(tag: (object) startInfo);
      S4_TDC_Internals.IUW_UltrasonicLogger.Info(startInfo);
      while (!cancelToken.IsCancellationRequested)
      {
        try
        {
          EmcTestData emcTestData = await this.ReadAndGetEmcTestDataAsync(progress, cancelToken);
          this.LastReadData = emcTestData;
          emcTestData = (EmcTestData) null;
          if (this.FirstTestData == null)
          {
            this.FirstTestData = this.LastReadData;
            this.LastTestData = this.LastReadData;
          }
          else if ((int) this.LastTestData.CycleCounter != (int) this.LastReadData.CycleCounter)
          {
            this.LastTestData = this.LastReadData;
            S4_TDC_Internals.CalcValues cv = this.PrepareTestResultAndGetFlow(testResult, this.LastTestData, this.FirstTestData, progress);
            if (this.EmcTestFinished)
            {
              startInfo = "**************** EMC test finished ********************";
              progress.Report(tag: (object) startInfo);
              S4_TDC_Internals.IUW_UltrasonicLogger.Info(startInfo);
              testResult.Clear();
              string outData;
              if (cv.volDiff == 0.0)
              {
                testResult.Append("No result");
              }
              else
              {
                testResult.Append("StartVoluume: " + cv.startVol.ToString("f6"));
                testResult.Append("; EndVoluume: " + this.LastTestData.VolQmSum.ToString("f6"));
                testResult.Append("; VolDiff: " + cv.volDiff.ToString("f6"));
                testResult.Append("; StartTime: " + this.FirstTestData.TimeStamp.ToShortDateString());
                testResult.Append(" " + this.FirstTestData.TimeStamp.ToString("HH:mm.ss"));
                testResult.Append("; EndTime: " + this.LastTestData.TimeStamp.ToShortDateString());
                testResult.Append(" " + this.LastTestData.TimeStamp.ToString("HH:mm.ss"));
                testResult.Append("; DiffHours " + cv.diffHours.ToString("f6"));
                outData = testResult.ToString();
                progress.Report("EMC test result", (object) outData);
                S4_TDC_Internals.IUW_UltrasonicLogger.Info(outData);
                double refVolDiff = (this.LastRefData.VolQmSum - this.FirstRefData.VolQmSum) / this.LastRefData.TimeStamp.Subtract(this.FirstRefData.TimeStamp).TotalHours * this.LastTestData.TimeStamp.Subtract(this.FirstTestData.TimeStamp).TotalHours;
                testResult.Clear();
                testResult.Append("; RefFlow: " + this.refFlow.ToString("f6"));
                testResult.Append("; Flow: " + cv.flow.ToString("f6"));
                testResult.Append("; RefVolDiff: " + refVolDiff.ToString("f6"));
                testResult.Append("; VolDiff: " + cv.volDiff.ToString("f6"));
                double error = (cv.flow - this.refFlow) / this.refFlow * 100.0;
                testResult.Append("; TestError[flow]: " + error.ToString("f6") + "%");
                double errorv = (cv.volDiff - refVolDiff) / refVolDiff * 100.0;
                testResult.Append("; TestError[volDiff]: " + errorv.ToString("f6") + "%");
              }
              outData = testResult.ToString();
              progress.Report("EMC test result", (object) outData);
              S4_TDC_Internals.IUW_UltrasonicLogger.Info(outData);
              break;
            }
            cv = (S4_TDC_Internals.CalcValues) null;
          }
        }
        catch (TimeoutException ex)
        {
          S4_TDC_Internals.IUW_UltrasonicErrorLogger.Error("Timeout");
        }
        catch (TaskCanceledException ex)
        {
          progress.Report("Canceled");
          testResult = (StringBuilder) null;
          startInfo = (string) null;
          return;
        }
        catch (Exception ex)
        {
          S4_TDC_Internals.IUW_UltrasonicErrorLogger.Error("Exception:" + ex.ToString());
          throw new Exception("RunEmcTest", ex);
        }
        await Task.Delay(1500, cancelToken);
      }
      this.EmcTestActive = false;
      testResult = (StringBuilder) null;
      startInfo = (string) null;
    }

    public async Task BeforeEmcTestAsync(ProgressHandler progress, CancellationToken cancelToken)
    {
      this.EmcTestActive = true;
      this.FirstTestData = (EmcTestData) null;
      this.LastTestData = (EmcTestData) null;
      StringBuilder testResult = new StringBuilder();
      string startInfo = "**************** Before EMC test ********************";
      progress.Report(tag: (object) startInfo);
      S4_TDC_Internals.IUW_UltrasonicLogger.Info(startInfo);
      try
      {
        EmcTestData emcTestData = await this.ReadAndGetEmcTestDataAsync(progress, cancelToken);
        this.LastReadData = emcTestData;
        emcTestData = (EmcTestData) null;
        this.FirstTestData = this.LastReadData;
        this.LastTestData = this.LastReadData;
        this.PrepareTestResultAndGetFlow(testResult, this.LastTestData, this.FirstTestData, progress);
      }
      catch (TimeoutException ex)
      {
        S4_TDC_Internals.IUW_UltrasonicErrorLogger.Error("Timeout");
      }
      catch (TaskCanceledException ex)
      {
        progress.Report("Canceled");
        testResult = (StringBuilder) null;
        startInfo = (string) null;
        return;
      }
      catch (Exception ex)
      {
        S4_TDC_Internals.IUW_UltrasonicErrorLogger.Error("Exception:" + ex.ToString());
        throw new Exception("RunEmcTest", ex);
      }
      this.EmcTestActive = false;
      testResult = (StringBuilder) null;
      startInfo = (string) null;
    }

    public async Task AfterEmcTestAsync(ProgressHandler progress, CancellationToken cancelToken)
    {
      this.EmcTestActive = true;
      StringBuilder testResult = new StringBuilder();
      string startInfo = "**************** After EMC test ********************";
      progress.Report(tag: (object) startInfo);
      S4_TDC_Internals.IUW_UltrasonicLogger.Info(startInfo);
      try
      {
        EmcTestData emcTestData = await this.ReadAndGetEmcTestDataAsync(progress, cancelToken);
        this.LastReadData = emcTestData;
        emcTestData = (EmcTestData) null;
        this.LastTestData = this.LastReadData;
        S4_TDC_Internals.CalcValues cv = this.PrepareTestResultAndGetFlow(testResult, this.LastTestData, this.FirstTestData, progress);
        startInfo = "**************** EMC test finished ********************";
        progress.Report(tag: (object) startInfo);
        S4_TDC_Internals.IUW_UltrasonicLogger.Info(startInfo);
        testResult.Clear();
        string outData;
        if (cv.volDiff == 0.0)
        {
          testResult.Append("No result");
        }
        else
        {
          testResult.Append("StartVoluume: " + cv.startVol.ToString("f6"));
          testResult.Append("; EndVoluume: " + this.LastTestData.VolQmSum.ToString("f6"));
          testResult.Append("; VolDiff: " + cv.volDiff.ToString("f6"));
          testResult.Append("; StartTime: " + this.FirstTestData.TimeStamp.ToShortDateString());
          testResult.Append(" " + this.FirstTestData.TimeStamp.ToString("HH:mm.ss"));
          testResult.Append("; EndTime: " + this.LastTestData.TimeStamp.ToShortDateString());
          testResult.Append(" " + this.LastTestData.TimeStamp.ToString("HH:mm.ss"));
          testResult.Append("; DiffHours " + cv.diffHours.ToString("f6"));
          outData = testResult.ToString();
          progress.Report("EMC test result", (object) outData);
          S4_TDC_Internals.IUW_UltrasonicLogger.Info(outData);
          double refVolDiff = (this.LastRefData.VolQmSum - this.FirstRefData.VolQmSum) / this.LastRefData.TimeStamp.Subtract(this.FirstRefData.TimeStamp).TotalHours * this.LastTestData.TimeStamp.Subtract(this.FirstTestData.TimeStamp).TotalHours;
          testResult.Clear();
          testResult.Append("; RefFlow: " + this.refFlow.ToString("f6"));
          testResult.Append("; Flow: " + cv.flow.ToString("f6"));
          testResult.Append("; RefVolDiff: " + refVolDiff.ToString("f6"));
          testResult.Append("; VolDiff: " + cv.volDiff.ToString("f6"));
          double error = (cv.flow - this.refFlow) / this.refFlow * 100.0;
          testResult.Append("; TestError[flow]: " + error.ToString("f6") + "%");
          double errorv = (cv.volDiff - refVolDiff) / refVolDiff * 100.0;
          testResult.Append("; TestError[volDiff]: " + errorv.ToString("f6") + "%");
          outData = testResult.ToString();
          progress.Report("EMC test result", (object) outData);
          S4_TDC_Internals.IUW_UltrasonicLogger.Info(outData);
        }
        cv = (S4_TDC_Internals.CalcValues) null;
        outData = (string) null;
      }
      catch (TimeoutException ex)
      {
        S4_TDC_Internals.IUW_UltrasonicErrorLogger.Error("Timeout");
      }
      catch (TaskCanceledException ex)
      {
        progress.Report("Canceled");
        testResult = (StringBuilder) null;
        startInfo = (string) null;
        return;
      }
      catch (Exception ex)
      {
        S4_TDC_Internals.IUW_UltrasonicErrorLogger.Error("Exception:" + ex.ToString());
        throw new Exception("RunEmcTest", ex);
      }
      this.EmcTestActive = false;
      testResult = (StringBuilder) null;
      startInfo = (string) null;
    }

    private S4_TDC_Internals.CalcValues PrepareTestResultAndGetFlow(
      StringBuilder testResult,
      EmcTestData testData,
      EmcTestData refData,
      ProgressHandler progress)
    {
      S4_TDC_Internals.CalcValues flow = new S4_TDC_Internals.CalcValues();
      testResult.Clear();
      testResult.Append(testData.CycleCounter.ToString().PadLeft(S4_TDC_Internals.columnCharacters[0]));
      testResult.Append(testData.TimeStamp.ToShortDateString().PadLeft(S4_TDC_Internals.columnCharacters[1]));
      testResult.Append(testData.TimeStamp.ToString("HH:mm:ss.fff").PadLeft(S4_TDC_Internals.columnCharacters[2]));
      testResult.Append(testData.CycleTime.ToString().PadLeft(S4_TDC_Internals.columnCharacters[3]));
      testResult.Append(testData.Flow.ToString("f6").PadLeft(S4_TDC_Internals.columnCharacters[4]));
      testResult.Append(testData.VolQmSum.ToString("f6").PadLeft(S4_TDC_Internals.columnCharacters[5]));
      flow.startVol = refData.VolQmSum;
      flow.volDiff = testData.VolQmSum - flow.startVol;
      flow.diffHours = testData.TimeStamp.Subtract(refData.TimeStamp).TotalHours;
      flow.flow = flow.volDiff / flow.diffHours;
      testResult.Append(flow.startVol.ToString("f6").PadLeft(S4_TDC_Internals.columnCharacters[6]));
      testResult.Append(flow.volDiff.ToString("f6").PadLeft(S4_TDC_Internals.columnCharacters[7]));
      testResult.Append(flow.flow.ToString("f6").PadLeft(S4_TDC_Internals.columnCharacters[8]));
      string str = testResult.ToString();
      progress.Report("EMC data received", (object) str);
      S4_TDC_Internals.IUW_UltrasonicLogger.Info(str);
      return flow;
    }

    internal async Task LogTdcInternals(
      int cycleTime,
      string fileExtension,
      EventHandler<string> tdcEvent,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.tdc1Log = new List<tdcData>();
      this.tdc2Log = new List<tdcData>();
      if (fileExtension != null)
        this.SetLogFilePathByExtension(fileExtension);
      try
      {
        DateTime nextTime = DateTime.Now;
        DateTime dateTime;
        TimeSpan timeSpan;
        if (this.unitsStorageTimeAddressRange != null)
        {
          AddressRange sysDateTimeRange = this.connectedMeter.meterMemory.GetAddressRange(S4_Params.sysDateTime.ToString());
          byte[] unitStorageTimeBytes = await this.nfcCMD.CommonNfcCommands.ReadMemoryAsync(progress, cancelToken, this.unitsStorageTimeAddressRange.StartAddress, this.unitsStorageTimeAddressRange.ByteSize);
          DateTime unitStorageTime = FirmwareDateTimeSupport.ToDateTimeFromBCD(unitStorageTimeBytes);
          DateTime unitStorageTime2 = DateTime.MinValue;
          while (unitStorageTime2 <= unitStorageTime)
          {
            await Task.Delay(500);
            dateTime = DateTime.Now;
            timeSpan = dateTime.Subtract(nextTime);
            if (timeSpan.TotalSeconds > 10.0)
              throw new Exception("Cycle syncronisation timeout");
            byte[] unitStorageTimeBytes2 = await this.nfcCMD.CommonNfcCommands.ReadMemoryAsync(progress, cancelToken, this.unitsStorageTimeAddressRange.StartAddress, this.unitsStorageTimeAddressRange.ByteSize);
            unitStorageTime2 = FirmwareDateTimeSupport.ToDateTimeFromBCD(unitStorageTimeBytes2);
            unitStorageTimeBytes2 = (byte[]) null;
          }
          byte[] sysDateTimeBytes = await this.nfcCMD.CommonNfcCommands.ReadMemoryAsync(progress, cancelToken, sysDateTimeRange.StartAddress, sysDateTimeRange.ByteSize);
          DateTime pcReadTime = DateTime.Now;
          dateTime = FirmwareDateTimeSupport.ToDateTimeFromBCD(sysDateTimeBytes);
          DateTime deviceTime = dateTime.AddMilliseconds(-100.0);
          TimeSpan CycleTime = unitStorageTime2 - unitStorageTime;
          int detectedCycle_ms = (int) CycleTime.TotalMilliseconds;
          int rounding = detectedCycle_ms % 400;
          detectedCycle_ms = detectedCycle_ms / 400 * 400;
          if (rounding > 200)
            detectedCycle_ms += 400;
          cycleTime = detectedCycle_ms;
          if (tdcEvent != null)
            tdcEvent((object) this, string.Format("Cycle TDC syncronised and set to {0}ms" + Environment.NewLine, (object) cycleTime));
          timeSpan = deviceTime.Subtract(unitStorageTime2);
          int TdcDeviceTimeDif_ms = (int) timeSpan.TotalMilliseconds;
          nextTime = pcReadTime.AddMilliseconds((double) (cycleTime + cycleTime / 2 - TdcDeviceTimeDif_ms));
          sysDateTimeRange = (AddressRange) null;
          unitStorageTimeBytes = (byte[]) null;
          sysDateTimeBytes = (byte[]) null;
          CycleTime = new TimeSpan();
        }
        await this.ReadConfig(progress, cancelToken);
        TextLineBuilder lineBuilder = new TextLineBuilder();
        this.BuildHeaderLine(lineBuilder);
        File.AppendAllText(this.LogFilePath, lineBuilder.ToCondencedString());
        int LineNumber = 0;
        do
        {
          try
          {
            timeSpan = nextTime - DateTime.Now;
            int delay = (int) timeSpan.TotalMilliseconds;
            if (delay > 10)
              await Task.Delay(delay, cancelToken);
            else if (delay < -100)
            {
              dateTime = DateTime.Now;
              nextTime = dateTime.AddMilliseconds((double) cycleTime);
            }
            byte[] numArray1 = await this.nfcCMD.CommonNfcCommands.mySubunitCommands.SetRfOnAsync(progress, cancelToken);
            UltrasonicState ultrasonicStateAsync = await this.ReadAndGetUltrasonicStateAsync(progress, cancelToken);
            byte[] numArray2 = await this.nfcCMD.CommonNfcCommands.mySubunitCommands.SetRfOffAsync(progress, cancelToken);
            this.tdc1Log.Add(this.tdcUnit_1);
            this.tdc2Log.Add(this.tdcUnit_2);
            if (this.tdc1Log.Count > 3600)
            {
              this.tdc1Log.RemoveRange(0, 300);
              this.tdc2Log.RemoveRange(0, 300);
            }
            if (LineNumber % 20 == 0)
            {
              if (LineNumber == 0)
              {
                lineBuilder.ClearLine();
                this.BuildValueLine(lineBuilder);
              }
              lineBuilder.ClearLine();
              this.BuildHeaderLine(lineBuilder);
              string headerLine = lineBuilder.ToString();
              if (tdcEvent != null)
                tdcEvent((object) this, headerLine);
              headerLine = (string) null;
            }
            ++LineNumber;
            lineBuilder.ClearLine();
            this.BuildValueLine(lineBuilder);
            File.AppendAllText(this.LogFilePath, lineBuilder.ToCondencedString());
            if (tdcEvent != null)
              tdcEvent((object) this, lineBuilder.ToString());
            nextTime = nextTime.AddMilliseconds((double) cycleTime);
          }
          catch (TaskCanceledException ex)
          {
            return;
          }
          catch (Exception ex)
          {
            string msg = ex.Message + "\r\n";
            if (this.LogFilePath != string.Empty)
              File.AppendAllText(this.LogFilePath, msg);
            if (tdcEvent != null)
              tdcEvent((object) this, msg);
            msg = (string) null;
          }
        }
        while (!cancelToken.IsCancellationRequested);
        lineBuilder = (TextLineBuilder) null;
      }
      catch (Exception ex)
      {
        string msg = ex.Message + "\r\n";
        if (this.LogFilePath != string.Empty)
          File.AppendAllText(this.LogFilePath, msg);
        if (tdcEvent != null)
          tdcEvent((object) this, msg);
        msg = (string) null;
      }
      byte[] numArray = await this.nfcCMD.CommonNfcCommands.mySubunitCommands.SetRfOffAsync(progress, cancelToken);
    }

    internal class tdcUsltData
    {
      internal const int tdcUsltDataLength = 20;
      internal ushort delayOld;
      internal ushort delayNew;
      internal byte[] wvrUp;
      internal byte[] wvrDn;
      internal byte measCounter;
      internal byte offsetOldUp;
      internal byte offsetOldDn;

      internal tdcUsltData(byte[] readData, int startOffset)
      {
        this.delayOld = readData.Length == 40 ? BitConverter.ToUInt16(readData, startOffset) : throw new Exception("Illegal data length for tdcUsltData data: " + readData.Length.ToString());
        startOffset += 2;
        this.delayNew = BitConverter.ToUInt16(readData, startOffset);
        startOffset += 2;
        this.wvrUp = new byte[6];
        this.wvrDn = new byte[6];
        Buffer.BlockCopy((Array) readData, startOffset, (Array) this.wvrUp, 0, 6);
        startOffset += 6;
        Buffer.BlockCopy((Array) readData, startOffset, (Array) this.wvrDn, 0, 6);
        startOffset += 6;
        this.measCounter = readData[startOffset];
        ++startOffset;
        this.offsetOldUp = readData[startOffset];
        ++startOffset;
        this.offsetOldDn = readData[startOffset];
        ++startOffset;
      }
    }

    private class CalcValues
    {
      internal double startVol;
      internal double volDiff;
      internal double diffHours;
      internal double flow;
    }
  }
}

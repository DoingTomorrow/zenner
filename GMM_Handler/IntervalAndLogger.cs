// Decompiled with JetBrains decompiler
// Type: GMM_Handler.IntervalAndLogger
// Assembly: GMM_Handler, Version=4.4.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 45504820-369B-4484-B911-CB82C9D368B6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GMM_Handler.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace GMM_Handler
{
  public class IntervalAndLogger : CodeBlock
  {
    internal const int YearSecounds = 31622400;
    internal const int MonthSecounds = 2678400;
    public static IntervalAndLogger.IntervallCycleInfos[] CodeIntervalls = new IntervalAndLogger.IntervallCycleInfos[18]
    {
      new IntervalAndLogger.IntervallCycleInfos(31622400U, "year", (byte) 87, "RUI_CODE_Interval RUI_TIME_Year"),
      new IntervalAndLogger.IntervallCycleInfos(0U, "6 month", (byte) 95, "RUI_CODE_Interval RUI_TIME_Month6"),
      new IntervalAndLogger.IntervallCycleInfos(0U, "3 month", (byte) 103, "RUI_CODE_Interval RUI_TIME_Month3"),
      new IntervalAndLogger.IntervallCycleInfos(2678400U, "month", (byte) 111, "RUI_CODE_Interval RUI_TIME_Month"),
      new IntervalAndLogger.IntervallCycleInfos(604800U, "week", (byte) 7, "RUI_CODE_Interval RUI_TIME_Week"),
      new IntervalAndLogger.IntervallCycleInfos(86400U, "day", (byte) 15, "RUI_CODE_Interval RUI_TIME_Day"),
      new IntervalAndLogger.IntervallCycleInfos(43200U, "12 hours", (byte) 23, "RUI_CODE_Interval RUI_TIME_Hours12"),
      new IntervalAndLogger.IntervallCycleInfos(21600U, "6 hours", (byte) 31, "RUI_CODE_Interval RUI_TIME_Hours6"),
      new IntervalAndLogger.IntervallCycleInfos(7200U, "2 hours", (byte) 39, "RUI_CODE_Interval RUI_TIME_Hours2"),
      new IntervalAndLogger.IntervallCycleInfos(3600U, "hour", (byte) 47, "RUI_CODE_Interval RUI_TIME_Hour"),
      new IntervalAndLogger.IntervallCycleInfos(1800U, "30 minutes", (byte) 55, "RUI_CODE_Interval RUI_TIME_Minutes30"),
      new IntervalAndLogger.IntervallCycleInfos(900U, "15 minutes", (byte) 63, "RUI_CODE_Interval RUI_TIME_Minutes15"),
      new IntervalAndLogger.IntervallCycleInfos(600U, "10 minutes", (byte) 71, "RUI_CODE_Interval RUI_TIME_Minutes10"),
      new IntervalAndLogger.IntervallCycleInfos(300U, "5 minutes", (byte) 79, "RUI_CODE_Interval RUI_TIME_Minutes5"),
      new IntervalAndLogger.IntervallCycleInfos(120U, "2 minutes", (byte) 119, ""),
      new IntervalAndLogger.IntervallCycleInfos(60U, "1 minutes", (byte) 119, ""),
      new IntervalAndLogger.IntervallCycleInfos(30U, "30 secounds", (byte) 119, ""),
      new IntervalAndLogger.IntervallCycleInfos(10U, "10 secounds", (byte) 119, "")
    };
    internal Meter MyMeter;
    public List<Parameter> LoggerParameter;
    public LoggerTypes Type;
    internal Parameter P_NextTimePoint;
    internal Parameter P_Intervall;
    internal Parameter P_StartAddress;
    internal Parameter P_EndAddress;
    internal Parameter P_WriteAddress;
    internal Parameter P_Flags;
    internal int MaxEntries;
    internal int EntrySize;
    internal int MBusParameterLength = 0;
    internal int DifVifEntrySize = 0;
    internal byte MBusMaxEntries;
    internal bool EpromDataAvailable = false;
    internal uint Interval;
    internal DateTime LastEventDateTime;
    internal DateTime NextEventDateTime;
    internal LoggerInfo MyLoggerInfo;
    internal bool LetLoggerUnchanged = false;

    internal IntervalAndLogger(
      CodeBlock.CodeSequenceTypes TheType,
      FrameTypes TheFrameType,
      int FunctionNumberIn)
      : base(TheType, TheFrameType, FunctionNumberIn)
    {
    }

    internal IntervalAndLogger Clone(
      Meter MyMeterIn,
      CodeBlock.CodeSequenceTypes TheType,
      FrameTypes TheFrameType,
      int FunctionNumberIn,
      ArrayList FunctionParameterList)
    {
      IntervalAndLogger intervalAndLogger = new IntervalAndLogger(TheType, TheFrameType, FunctionNumberIn);
      intervalAndLogger.MyMeter = MyMeterIn;
      intervalAndLogger.MaxEntries = this.MaxEntries;
      intervalAndLogger.EntrySize = this.EntrySize;
      intervalAndLogger.Type = this.Type;
      intervalAndLogger.MBusMaxEntries = this.MBusMaxEntries;
      intervalAndLogger.Interval = this.Interval;
      intervalAndLogger.MBusParameterLength = 0;
      intervalAndLogger.DifVifEntrySize = 0;
      foreach (Parameter functionParameter in FunctionParameterList)
      {
        if (functionParameter.BlockMark == LinkBlockTypes.LoggerStore)
        {
          if (intervalAndLogger.LoggerParameter == null)
            intervalAndLogger.LoggerParameter = new List<Parameter>();
          intervalAndLogger.LoggerParameter.Add(functionParameter);
          intervalAndLogger.MBusParameterLength += functionParameter.MBusParameterLength;
          intervalAndLogger.DifVifEntrySize += (int) functionParameter.DifVifSize;
        }
        else if (functionParameter.Name.EndsWith("_0T"))
        {
          intervalAndLogger.P_NextTimePoint = functionParameter;
          intervalAndLogger.InitialiseTimeVariablesFromMeterTime(DateTime.Now);
        }
        else if (functionParameter.Name.EndsWith("_1I"))
          intervalAndLogger.P_Intervall = functionParameter;
        else if (functionParameter.Name.EndsWith("_2S"))
          intervalAndLogger.P_StartAddress = functionParameter;
        else if (functionParameter.Name.EndsWith("_3E"))
          intervalAndLogger.P_EndAddress = functionParameter;
        else if (functionParameter.Name.EndsWith("_4W"))
          intervalAndLogger.P_WriteAddress = functionParameter;
        else if (functionParameter.Name.EndsWith("_5F"))
          intervalAndLogger.P_Flags = functionParameter;
      }
      return intervalAndLogger;
    }

    internal void ClearLogger()
    {
      if (this.LetLoggerUnchanged)
        return;
      if (this.P_WriteAddress != null)
      {
        this.P_WriteAddress.ValueEprom = this.P_StartAddress.ValueEprom;
        this.P_WriteAddress.UpdateByteList();
      }
      if (this.P_Flags != null)
      {
        this.P_Flags.ValueEprom = 0L;
        this.P_Flags.UpdateByteList();
      }
    }

    internal bool SetIntervalFromOpcode() => true;

    internal bool InitialiseTimeVariablesFromMeterTime(DateTime ClockTime)
    {
      if (this.Interval == 0U)
        return true;
      Parameter pNextTimePoint = this.P_NextTimePoint;
      DateTime dateTime = ZR_Calendar.Cal_GetDateTime((uint) pNextTimePoint.ValueEprom);
      this.NextEventDateTime = ClockTime;
      this.SetTimeToNextInterval(dateTime, (int) this.P_NextTimePoint.MinValue, ref this.NextEventDateTime);
      this.LastEventDateTime = this.NextEventDateTime;
      this.GetElapsedTimePoint(1, ref this.LastEventDateTime);
      uint num = this.Type != LoggerTypes.FixedLoggerFuture ? ZR_Calendar.Cal_GetMeterTime(this.LastEventDateTime) : ZR_Calendar.Cal_GetMeterTime(this.NextEventDateTime);
      pNextTimePoint.ValueEprom = (long) num;
      pNextTimePoint.UpdateByteList();
      if (this.P_Intervall != null)
      {
        this.P_Intervall.ValueEprom = (long) this.Interval;
        this.P_Intervall.UpdateByteList();
      }
      return true;
    }

    internal void SetTimeToNextInterval(
      DateTime NearIntervallTime,
      int FixOffsetMinutes,
      ref DateTime TimeToSet)
    {
      if (NearIntervallTime.Second > 50)
        NearIntervallTime = NearIntervallTime.AddSeconds((double) (60 - NearIntervallTime.Second));
      if (!this.MyMeter.MyHandler.IgnoreIntervalMinutesRaster)
      {
        if (FixOffsetMinutes > 0)
        {
          int num = (FixOffsetMinutes != 60 ? FixOffsetMinutes : 0) - NearIntervallTime.Minute;
          if (num != 0)
            NearIntervallTime = NearIntervallTime.AddMinutes((double) num);
        }
      }
      else if (this.Interval >= 3600U)
        NearIntervallTime = NearIntervallTime.AddMinutes((double) (NearIntervallTime.Minute * -1));
      TimeToSet = new DateTime(TimeToSet.Year, TimeToSet.Month, TimeToSet.Day, TimeToSet.Hour, TimeToSet.Minute, TimeToSet.Second);
      if (this.Interval == 31622400U)
      {
        int num = NearIntervallTime.Day - 1;
        NearIntervallTime = new DateTime(TimeToSet.Year, NearIntervallTime.Month, 1, NearIntervallTime.Hour, NearIntervallTime.Minute, NearIntervallTime.Second);
        NearIntervallTime = NearIntervallTime.AddDays((double) num);
        while (NearIntervallTime > TimeToSet)
          NearIntervallTime = NearIntervallTime.AddYears(-1);
        while (NearIntervallTime < TimeToSet)
          NearIntervallTime = NearIntervallTime.AddYears(1);
        TimeToSet = NearIntervallTime;
      }
      else if (this.Interval == 2678400U)
      {
        int num = NearIntervallTime.Day - 1;
        NearIntervallTime = new DateTime(TimeToSet.Year, NearIntervallTime.Month, 1, NearIntervallTime.Hour, NearIntervallTime.Minute, NearIntervallTime.Second);
        NearIntervallTime = NearIntervallTime.AddDays((double) num);
        while (NearIntervallTime > TimeToSet)
          NearIntervallTime = NearIntervallTime.AddMonths(-1);
        while (NearIntervallTime < TimeToSet)
          NearIntervallTime = NearIntervallTime.AddMonths(1);
        TimeToSet = NearIntervallTime;
      }
      else
      {
        long meterTime = (long) ZR_Calendar.Cal_GetMeterTime(NearIntervallTime);
        long num1 = ((long) ZR_Calendar.Cal_GetMeterTime(TimeToSet) - meterTime) / (long) this.Interval * (long) this.Interval;
        NearIntervallTime.AddSeconds((double) num1);
        long num2 = (long) this.Interval * -1L;
        while (NearIntervallTime > TimeToSet)
          NearIntervallTime = NearIntervallTime.AddSeconds((double) num2);
        while (NearIntervallTime < TimeToSet)
          NearIntervallTime = NearIntervallTime.AddSeconds((double) this.Interval);
        TimeToSet = NearIntervallTime;
        DateTime dateTime1 = new DateTime(2000, 1, 1);
        double num3 = Math.Truncate(TimeToSet.Subtract(dateTime1).TotalSeconds / (double) this.Interval) * (double) this.Interval;
        DateTime dateTime2 = dateTime1.AddSeconds(num3);
        TimeSpan timeSpan = dateTime2.Subtract(TimeToSet);
        double totalSeconds = timeSpan.TotalSeconds;
        while (Math.Abs(totalSeconds) >= 10.0)
        {
          dateTime2 = dateTime2.AddSeconds((double) this.Interval);
          timeSpan = dateTime2.Subtract(TimeToSet);
          totalSeconds = timeSpan.TotalSeconds;
          if (totalSeconds >= 10.0)
            goto label_33;
        }
        TimeToSet = dateTime2;
label_33:;
      }
    }

    internal void InitialiseNextTimePoint()
    {
      this.SetTimeToIntervalBoundary();
      if (this.Type == LoggerTypes.FixedLoggerFuture)
        this.P_NextTimePoint.ValueEprom = (long) ZR_Calendar.Cal_GetMeterTime(this.NextEventDateTime);
      else
        this.P_NextTimePoint.ValueEprom = (long) ZR_Calendar.Cal_GetMeterTime(this.LastEventDateTime);
    }

    internal void SetTimeToIntervalBoundary()
    {
      DateTime now = DateTime.Now;
      if (this.Interval == 31622400U)
      {
        this.LastEventDateTime = new DateTime(now.Year, 1, 1);
        this.NextEventDateTime = this.LastEventDateTime.AddYears(1);
      }
      else if (this.Interval == 2678400U)
      {
        this.LastEventDateTime = new DateTime(now.Year, now.Month, 1);
        this.NextEventDateTime = this.LastEventDateTime.AddMonths(1);
      }
      else
      {
        this.LastEventDateTime = new DateTime(now.Ticks / 10000000L / (long) this.Interval * (long) this.Interval * 10000000L);
        this.NextEventDateTime = this.LastEventDateTime.AddSeconds((double) this.Interval);
      }
    }

    internal void SetTimeToLastInterval(DateTime NearIntervallTime, ref DateTime TimeToSet)
    {
      DateTime dateTime = TimeToSet;
      this.SetTimeToNextInterval(NearIntervallTime, 0, ref TimeToSet);
      if (!(TimeToSet > dateTime))
        return;
      this.GetElapsedTimePoint(1, ref TimeToSet);
    }

    internal void GetElapsedTimePoint(int BackIntervalls, ref DateTime TimeToSet)
    {
      if (TimeToSet == DateTime.MinValue || BackIntervalls <= 0)
        return;
      if (this.Interval == 31622400U)
      {
        for (int index = 0; index < BackIntervalls; ++index)
          TimeToSet = TimeToSet.AddYears(-1);
      }
      else if (this.Interval == 2678400U)
      {
        for (int index = 0; index < BackIntervalls; ++index)
          TimeToSet = TimeToSet.AddMonths(-1);
      }
      else
        TimeToSet = TimeToSet.Subtract(new TimeSpan(0, 0, (int) this.Interval * BackIntervalls));
    }

    internal bool CompleteLoggerData()
    {
      try
      {
        byte codeValueCompiled1 = (byte) ((CodeObject) this.CodeList[0]).CodeValueCompiled;
        ushort codeValueCompiled2 = (ushort) ((CodeObject) this.CodeList[1]).CodeValueCompiled;
        if (codeValueCompiled1 == (byte) 119)
        {
          this.Interval = (uint) this.P_Intervall.ValueEprom;
        }
        else
        {
          for (int index = 0; index < IntervalAndLogger.CodeIntervalls.Length; ++index)
          {
            if ((int) IntervalAndLogger.CodeIntervalls[index].RuntimeCode == (int) codeValueCompiled1)
            {
              this.Interval = IntervalAndLogger.CodeIntervalls[index].Secounds;
              break;
            }
          }
          if (this.Interval == 0U)
            return false;
        }
        int num1 = (int) (byte) ((CodeObject) this.CodeList[2]).CodeValueCompiled - 1;
        int num2 = 0;
        this.LoggerParameter = new List<Parameter>();
        int num3 = 3;
        this.EntrySize = 0;
        do
        {
          ArrayList codeList1 = this.CodeList;
          int index1 = num3;
          int num4 = index1 + 1;
          byte codeValueCompiled3 = (byte) ((CodeObject) codeList1[index1]).CodeValueCompiled;
          int num5 = num2 + 1;
          int num6;
          Parameter parameter;
          switch (codeValueCompiled3)
          {
            case 48:
              goto label_36;
            case 51:
              num6 = 4;
              parameter = (Parameter) this.MyMeter.AllParameters[(object) "DefaultFunction.Sta_Secounds"];
              break;
            case 80:
              return true;
            case 83:
              num6 = 4;
              parameter = (Parameter) this.MyMeter.AllParameters[(object) "Energ_WaermeEnergDisplay"];
              break;
            case 120:
              return true;
            case 128:
              num6 = 1;
              num5 += 2;
              parameter = this.GetPtrParameter((CodeObject) this.CodeList[num4++]);
              break;
            case 139:
              num6 = 2;
              num5 += 2;
              parameter = this.GetPtrParameter((CodeObject) this.CodeList[num4++]);
              break;
            case 147:
              num6 = 4;
              parameter = (Parameter) this.MyMeter.AllParameters[(object) "Vol_VolumenDisplay"];
              break;
            case 155:
              num6 = 4;
              num5 += 2;
              parameter = this.GetPtrParameter((CodeObject) this.CodeList[num4++]);
              break;
            case 160:
              num6 = 4;
              parameter = (Parameter) this.MyMeter.AllParameters[(object) "Energ_KaelteEnergDisplay"];
              break;
            case 195:
              num6 = 1;
              num5 += 2;
              parameter = this.GetPtrParameter((CodeObject) this.CodeList[num4++]);
              break;
            case 203:
              num6 = 2;
              num5 += 2;
              parameter = this.GetPtrParameter((CodeObject) this.CodeList[num4++]);
              break;
            case 219:
              num6 = 4;
              num5 += 2;
              parameter = this.GetPtrParameter((CodeObject) this.CodeList[num4++]);
              break;
            default:
              throw new ArgumentOutOfRangeException("Illegal logger parameter load code at logger " + ((Function) this.MyMeter.MyHandler.MyLoadedFunctions.LoadedFunctionHeaders[(object) (ushort) this.FunctionNumber]).Name);
          }
          if (num6 != parameter.Size)
            throw new ArgumentOutOfRangeException("Illegal logger parameter size at logger " + ((Function) this.MyMeter.MyHandler.MyLoadedFunctions.LoadedFunctionHeaders[(object) (ushort) this.FunctionNumber]).Name);
          ArrayList codeList2 = this.CodeList;
          int index2 = num4;
          num3 = index2 + 1;
          byte codeValueCompiled4 = (byte) ((CodeObject) codeList2[index2]).CodeValueCompiled;
          num2 = num5 + 1;
          switch (codeValueCompiled4)
          {
            case 35:
              ++this.EntrySize;
              break;
            case 43:
              this.EntrySize += 2;
              break;
            case 59:
              this.EntrySize += 4;
              break;
            default:
              if (this.EntrySize == 0)
                return true;
              throw new ArgumentOutOfRangeException("Illegal logger store code at logger " + ((Function) this.MyMeter.MyHandler.MyLoadedFunctions.LoadedFunctionHeaders[(object) (ushort) this.FunctionNumber]).Name);
          }
          this.LoggerParameter.Add(parameter);
        }
        while (num2 < num1);
      }
      catch (Exception ex)
      {
        throw new ArgumentOutOfRangeException("Illegal logger store code at logger " + ((Function) this.MyMeter.MyHandler.MyLoadedFunctions.LoadedFunctionHeaders[(object) (ushort) this.FunctionNumber]).Name + ZR_Constants.SystemNewLine + ex.ToString());
      }
label_36:
      if (this.P_EndAddress != null)
        this.MaxEntries = (int) ((this.P_EndAddress.ValueEprom - this.P_StartAddress.ValueEprom) / (long) this.EntrySize);
      return true;
    }

    internal bool ReadDataToEpromArray()
    {
      if (this.EpromDataAvailable || this.P_EndAddress == null)
        return true;
      int num = ((ulong) this.P_Flags.ValueEprom & 128UL) <= 0UL ? (int) (this.P_WriteAddress.ValueEprom - 1L) : (int) (this.P_EndAddress.ValueEprom - 1L);
      int valueEprom = (int) this.P_StartAddress.ValueEprom;
      int NumberOfBytes = num - valueEprom + 1;
      ByteField MemoryData;
      if (!this.MyMeter.MyHandler.SerBus.ReadMemory(MemoryLocation.EEPROM, valueEprom, NumberOfBytes, out MemoryData))
      {
        ZR_ClassLibMessages.AddErrorDescription("Read logger data error");
        return false;
      }
      for (int index = 0; index < MemoryData.Count; ++index)
        this.MyMeter.Eprom[valueEprom + index] = MemoryData.Data[index];
      if (this.MyMeter.UsedEpromSize <= num)
        this.MyMeter.UsedEpromSize = num + 1;
      return true;
    }

    private Parameter GetPtrParameter(CodeObject TheCodeObj)
    {
      string codeValue = TheCodeObj.CodeValue;
      if (codeValue.StartsWith("LateLink:0x"))
      {
        int key = int.Parse(codeValue.Substring(11), NumberStyles.HexNumber);
        if (TheCodeObj.CodeType == CodeObject.CodeTypes.iPTR)
        {
          TheCodeObj.CodeValue = ((Parameter) this.MyMeter.AllRamParametersByAddress[(object) key]).FullName;
        }
        else
        {
          if (TheCodeObj.CodeType != CodeObject.CodeTypes.ePTR)
            throw new ArgumentOutOfRangeException("External logger time interval not supported");
          TheCodeObj.CodeValue = ((Parameter) this.MyMeter.AllEpromParametersByAddress[(object) key]).FullName;
        }
      }
      return (Parameter) this.MyMeter.AllParameters[(object) TheCodeObj.CodeValue];
    }

    internal bool AdjustFunction()
    {
      int CodeIndex1 = 0;
      CodeObject loggerCodeObject1 = this.GetNextLoggerCodeObject(ref CodeIndex1);
      int address = loggerCodeObject1.Address;
      byte[] eprom1 = this.MyMeter.Eprom;
      int index1 = address;
      int num1 = index1 + 1;
      byte num2 = eprom1[index1];
      this.MyMeter.MyCompiler.CompileCodeObject(loggerCodeObject1);
      byte[] eprom2 = this.MyMeter.Eprom;
      int index2 = num1;
      int num3 = index2 + 1;
      int num4 = (int) eprom2[index2];
      byte[] eprom3 = this.MyMeter.Eprom;
      int index3 = num3;
      int num5 = index3 + 1;
      int num6 = (int) eprom3[index3] << 8;
      int num7 = num4 + num6;
      int CodeIndex2 = CodeIndex1 + 1;
      Parameter pNextTimePoint = this.P_NextTimePoint;
      byte[] eprom4 = this.MyMeter.Eprom;
      int index4 = num7;
      int num8 = index4 + 1;
      int num9 = (int) eprom4[index4];
      byte[] eprom5 = this.MyMeter.Eprom;
      int index5 = num8;
      int num10 = index5 + 1;
      int num11 = (int) eprom5[index5] << 8;
      int num12 = num9 + num11;
      byte[] eprom6 = this.MyMeter.Eprom;
      int index6 = num10;
      int num13 = index6 + 1;
      int num14 = (int) eprom6[index6] << 16;
      int num15 = num12 + num14;
      byte[] eprom7 = this.MyMeter.Eprom;
      int index7 = num13;
      int num16 = index7 + 1;
      int num17 = (int) eprom7[index7] << 24;
      long num18 = (long) (uint) (num15 + num17);
      pNextTimePoint.ValueEprom = num18;
      if (num2 == (byte) 119)
      {
        if (this.Type != LoggerTypes.ShortCycleLogger)
          throw new ArgumentOutOfRangeException("Illegal logger type code");
        byte[] eprom8 = this.MyMeter.Eprom;
        int index8 = num16;
        int num19 = index8 + 1;
        int num20 = (int) eprom8[index8];
        byte[] eprom9 = this.MyMeter.Eprom;
        int index9 = num19;
        int num21 = index9 + 1;
        int num22 = (int) eprom9[index9] << 8;
        int num23 = num20 + num22;
        byte[] eprom10 = this.MyMeter.Eprom;
        int index10 = num21;
        int num24 = index10 + 1;
        int num25 = (int) eprom10[index10] << 16;
        int num26 = num23 + num25;
        byte[] eprom11 = this.MyMeter.Eprom;
        int index11 = num24;
        num16 = index11 + 1;
        int num27 = (int) eprom11[index11] << 24;
        this.Interval = (uint) (num26 + num27);
        this.P_Intervall.ValueEprom = (long) this.Interval;
      }
      else
      {
        for (int index12 = 0; index12 < IntervalAndLogger.CodeIntervalls.Length; ++index12)
        {
          if ((int) IntervalAndLogger.CodeIntervalls[index12].RuntimeCode == (int) num2)
          {
            this.Interval = IntervalAndLogger.CodeIntervalls[index12].Secounds;
            loggerCodeObject1.CodeValue = IntervalAndLogger.CodeIntervalls[index12].RuntimeCodeString;
            break;
          }
        }
      }
      if (this.Interval == 0U)
        throw new ArgumentOutOfRangeException("Illegal logger cycle code");
      Parameter pStartAddress = this.P_StartAddress;
      byte[] eprom12 = this.MyMeter.Eprom;
      int index13 = num16;
      int num28 = index13 + 1;
      int num29 = (int) eprom12[index13];
      byte[] eprom13 = this.MyMeter.Eprom;
      int index14 = num28;
      int num30 = index14 + 1;
      int num31 = (int) eprom13[index14] << 8;
      long num32 = (long) (uint) (num29 + num31);
      pStartAddress.ValueEprom = num32;
      Parameter pEndAddress = this.P_EndAddress;
      byte[] eprom14 = this.MyMeter.Eprom;
      int index15 = num30;
      int num33 = index15 + 1;
      int num34 = (int) eprom14[index15];
      byte[] eprom15 = this.MyMeter.Eprom;
      int index16 = num33;
      int num35 = index16 + 1;
      int num36 = (int) eprom15[index16] << 8;
      long num37 = (long) (uint) (num34 + num36);
      pEndAddress.ValueEprom = num37;
      byte[] eprom16 = this.MyMeter.Eprom;
      int index17 = num35;
      int num38 = index17 + 1;
      int num39 = (int) eprom16[index17];
      byte[] eprom17 = this.MyMeter.Eprom;
      int index18 = num38;
      int num40 = index18 + 1;
      int num41 = (int) eprom17[index18] << 8;
      int num42 = num39 + num41;
      byte[] eprom18 = this.MyMeter.Eprom;
      int index19 = num40;
      int num43 = index19 + 1;
      byte num44 = eprom18[index19];
      byte[] eprom19 = this.MyMeter.Eprom;
      int index20 = num5;
      int num45 = index20 + 1;
      byte num46 = eprom19[index20];
      CodeObject loggerCodeObject2 = this.GetNextLoggerCodeObject(ref CodeIndex2);
      loggerCodeObject2.CodeValue = num46.ToString();
      this.MyMeter.MyCompiler.CompileCodeObject(loggerCodeObject2);
      int num47 = num45 + (int) num46 - 1;
      this.EntrySize = 0;
      while (num45 < num47)
      {
        int num48 = 4;
        int num49 = 0;
        CodeObject.CodeTypes codeTypes = CodeObject.CodeTypes.BYTE;
        byte[] eprom20 = this.MyMeter.Eprom;
        int index21 = num45;
        int num50 = index21 + 1;
        string str1;
        switch (eprom20[index21])
        {
          case 51:
            str1 = "RUI_CODE_Load_SysTime";
            break;
          case 83:
            str1 = "RUI_CODE_Load_Energie";
            break;
          case 128:
            codeTypes = CodeObject.CodeTypes.iPTR;
            num48 = 1;
            byte[] eprom21 = this.MyMeter.Eprom;
            int index22 = num50;
            int num51 = index22 + 1;
            int num52 = (int) eprom21[index22];
            byte[] eprom22 = this.MyMeter.Eprom;
            int index23 = num51;
            num50 = index23 + 1;
            int num53 = (int) eprom22[index23] << 8;
            num49 = num52 + num53;
            str1 = "RUI_CODE_Load RUI_VAR_1BYTE";
            break;
          case 139:
            codeTypes = CodeObject.CodeTypes.iPTR;
            num48 = 2;
            byte[] eprom23 = this.MyMeter.Eprom;
            int index24 = num50;
            int num54 = index24 + 1;
            int num55 = (int) eprom23[index24];
            byte[] eprom24 = this.MyMeter.Eprom;
            int index25 = num54;
            num50 = index25 + 1;
            int num56 = (int) eprom24[index25] << 8;
            num49 = num55 + num56;
            str1 = "RUI_CODE_Load RUI_VAR_2BYTE";
            break;
          case 147:
            str1 = "RUI_CODE_Load_Volume";
            break;
          case 155:
            codeTypes = CodeObject.CodeTypes.iPTR;
            byte[] eprom25 = this.MyMeter.Eprom;
            int index26 = num50;
            int num57 = index26 + 1;
            int num58 = (int) eprom25[index26];
            byte[] eprom26 = this.MyMeter.Eprom;
            int index27 = num57;
            num50 = index27 + 1;
            int num59 = (int) eprom26[index27] << 8;
            num49 = num58 + num59;
            str1 = "RUI_CODE_Load RUI_VAR_4BYTE";
            break;
          case 160:
            str1 = "RUI_CODE_Load_ColdEnergie";
            break;
          case 195:
            codeTypes = CodeObject.CodeTypes.ePTR;
            num48 = 1;
            byte[] eprom27 = this.MyMeter.Eprom;
            int index28 = num50;
            int num60 = index28 + 1;
            int num61 = (int) eprom27[index28];
            byte[] eprom28 = this.MyMeter.Eprom;
            int index29 = num60;
            num50 = index29 + 1;
            int num62 = (int) eprom28[index29] << 8;
            num49 = num61 + num62;
            str1 = "RUI_CODE_Load RUI_VAR_EEPROM RUI_VAR_1BYTE";
            break;
          case 203:
            codeTypes = CodeObject.CodeTypes.ePTR;
            num48 = 2;
            byte[] eprom29 = this.MyMeter.Eprom;
            int index30 = num50;
            int num63 = index30 + 1;
            int num64 = (int) eprom29[index30];
            byte[] eprom30 = this.MyMeter.Eprom;
            int index31 = num63;
            num50 = index31 + 1;
            int num65 = (int) eprom30[index31] << 8;
            num49 = num64 + num65;
            str1 = "RUI_CODE_Load RUI_VAR_EEPROM RUI_VAR_2BYTE";
            break;
          case 219:
            codeTypes = CodeObject.CodeTypes.ePTR;
            byte[] eprom31 = this.MyMeter.Eprom;
            int index32 = num50;
            int num66 = index32 + 1;
            int num67 = (int) eprom31[index32];
            byte[] eprom32 = this.MyMeter.Eprom;
            int index33 = num66;
            num50 = index33 + 1;
            int num68 = (int) eprom32[index33] << 8;
            num49 = num67 + num68;
            str1 = "RUI_CODE_Load RUI_VAR_EEPROM RUI_VAR_4BYTE";
            break;
          default:
            throw new ArgumentOutOfRangeException("Unknown logger parameter load code");
        }
        byte[] eprom33 = this.MyMeter.Eprom;
        int index34 = num50;
        num45 = index34 + 1;
        string str2;
        switch (eprom33[index34])
        {
          case 35:
            str2 = "RUI_CODE_LOGGER_STORE_1BYTE";
            break;
          case 43:
            str2 = "RUI_CODE_LOGGER_STORE_2BYTE";
            break;
          case 59:
            str2 = "RUI_CODE_LOGGER_STORE_4BYTE";
            break;
          default:
            throw new ArgumentOutOfRangeException("Unknown logger parameter store code");
        }
        CodeObject loggerCodeObject3 = this.GetNextLoggerCodeObject(ref CodeIndex2);
        loggerCodeObject3.CodeType = CodeObject.CodeTypes.BYTE;
        loggerCodeObject3.Size = 1;
        loggerCodeObject3.CodeValue = str1;
        if (codeTypes != 0)
        {
          CodeObject loggerCodeObject4 = this.GetNextLoggerCodeObject(ref CodeIndex2);
          loggerCodeObject4.CodeType = codeTypes;
          loggerCodeObject4.Size = 2;
          loggerCodeObject4.CodeValue = "LateLink:0x" + num49.ToString("x04");
        }
        CodeObject loggerCodeObject5 = this.GetNextLoggerCodeObject(ref CodeIndex2);
        loggerCodeObject5.CodeType = CodeObject.CodeTypes.BYTE;
        loggerCodeObject5.Size = 1;
        loggerCodeObject5.CodeValue = str2;
        this.EntrySize += num48;
      }
      if (num45 != num47)
        throw new ArgumentOutOfRangeException("Illegal logger parameter list");
      this.MaxEntries = (int) ((this.P_EndAddress.ValueEprom - this.P_StartAddress.ValueEprom) / (long) this.EntrySize);
      if (this.MyMeter.ConfigLoggers == null)
        this.MyMeter.ConfigLoggers = new SortedList<uint, Function>();
      if (this.MyMeter.ConfigLoggers.IndexOfKey((uint) (ushort) this.FunctionNumber) < 0)
        this.MyMeter.ConfigLoggers.Add((uint) (ushort) this.FunctionNumber, (Function) this.MyMeter.MyFunctionTable.FunctionListByNumber[(object) (ushort) this.FunctionNumber]);
      return true;
    }

    private void CreateAllPotentialLoggerParameters()
    {
      if (this.MyMeter.PotentialLoggerParameters != null)
        return;
      this.MyMeter.PotentialLoggerParameters = new SortedList<string, Parameter>();
      foreach (Parameter BaseParameter in (IEnumerable) this.MyMeter.MyMBusList.AllMBusParameters.Values)
      {
        LoggerParameterData loggerParameterData = new LoggerParameterData(BaseParameter, this.MyMeter);
        BaseParameter.LoggerData = loggerParameterData;
        if (!(loggerParameterData.ZDF_ParameterID == "NoZDF_Param"))
          this.MyMeter.PotentialLoggerParameters.Add(BaseParameter.FullName, BaseParameter);
      }
    }

    private CodeObject GetNextLoggerCodeObject(ref int CodeIndex)
    {
      CodeObject loggerCodeObject;
      if (this.CodeList.Count > CodeIndex)
      {
        loggerCodeObject = (CodeObject) this.CodeList[CodeIndex];
      }
      else
      {
        loggerCodeObject = new CodeObject((int) (ushort) this.FunctionNumber);
        this.CodeList.Add((object) loggerCodeObject);
      }
      ++CodeIndex;
      return loggerCodeObject;
    }

    public LoggerInfo GetLoggerInfo(out int LoggerMemorySize)
    {
      this.CreateAllPotentialLoggerParameters();
      LoggerMemorySize = this.MyMeter.MyIdent.extEEPSize - this.MyMeter.MyLoggerStore.BlockStartAddress;
      if (this.MyLoggerInfo != null)
        return this.MyLoggerInfo;
      if (this.Type != LoggerTypes.FixedLogger && this.Type != LoggerTypes.FixedLoggerFuture && this.Type != LoggerTypes.ConfigLogger && this.Type != LoggerTypes.ShortCycleLogger || this.EntrySize == 0)
        return (LoggerInfo) null;
      this.MyLoggerInfo = new LoggerInfo(this);
      this.MyLoggerInfo.AllPotentialParameters = new LoggerParameterData[this.MyMeter.PotentialLoggerParameters.Count];
      for (int index = 0; index < this.MyMeter.PotentialLoggerParameters.Count; ++index)
      {
        Parameter parameter = this.MyMeter.PotentialLoggerParameters.Values[index];
        this.MyLoggerInfo.AllPotentialParameters[index] = parameter.LoggerData;
      }
      this.MyLoggerInfo.LoggerName = ((Function) this.MyMeter.MyFunctionTable.FunctionListByNumber[(object) (ushort) this.FunctionNumber]).Name;
      this.MyLoggerInfo.EntrySize = this.EntrySize;
      this.MyLoggerInfo.MaxEntries = this.MaxEntries;
      this.MyLoggerInfo.StartAddress = (int) this.P_StartAddress.ValueEprom;
      this.MyLoggerInfo.EndAddress = (int) this.P_EndAddress.ValueEprom;
      this.MyLoggerInfo.WriteAddress = (int) this.P_WriteAddress.ValueEprom;
      this.MyLoggerInfo.MaxRamBytes = this.MyMeter.MyIdent.extEEPSize - this.MyMeter.MyLoggerStore.StartAddressOfNextBlock + this.EntrySize * this.MaxEntries;
      if (((ulong) this.P_Flags.ValueEprom & 128UL) > 0UL)
      {
        this.MyLoggerInfo.AvailableEntries = this.MaxEntries;
        this.MyLoggerInfo.LoggerFull = true;
      }
      else
      {
        this.MyLoggerInfo.AvailableEntries = (int) ((this.P_WriteAddress.ValueEprom - this.P_StartAddress.ValueEprom) / (long) this.EntrySize);
        this.MyLoggerInfo.LoggerFull = false;
      }
      for (int index = 0; index < IntervalAndLogger.CodeIntervalls.Length; ++index)
      {
        if ((int) IntervalAndLogger.CodeIntervalls[index].Secounds == (int) this.Interval)
        {
          this.MyLoggerInfo.Intervall = IntervalAndLogger.CodeIntervalls[index].Description;
          this.MyLoggerInfo.TimeListIndex = index;
          break;
        }
      }
      if (this.LastEventDateTime == DateTime.MinValue)
        this.SetTimeToIntervalBoundary();
      this.MyLoggerInfo.LastTimePoint = this.LastEventDateTime;
      this.MyLoggerInfo.FirstTimePoint = this.MyLoggerInfo.LastTimePoint;
      this.GetElapsedTimePoint(this.MyLoggerInfo.AvailableEntries - 1, ref this.MyLoggerInfo.FirstTimePoint);
      this.MyLoggerInfo.LastStoredTimePoint = this.MyLoggerInfo.LastTimePoint.ToShortDateString() + " " + this.MyLoggerInfo.LastTimePoint.ToShortTimeString();
      this.MyLoggerInfo.FirstStoredTimePoint = this.MyLoggerInfo.FirstTimePoint.ToShortDateString() + " " + this.MyLoggerInfo.FirstTimePoint.ToShortTimeString();
      this.MyLoggerInfo.EstimatedReadoutTime = (this.MyLoggerInfo.AvailableEntries * this.EntrySize * 11 * 2 / 2400 + 1).ToString() + " Sec.";
      this.MyLoggerInfo.AllParameters = new LoggerParameterData[this.LoggerParameter.Count];
      this.MyLoggerInfo.IndexOfTimeParameter = -1;
      int index1 = 0;
      foreach (Parameter BaseParameter in this.LoggerParameter)
      {
        if (BaseParameter.LoggerData == null)
          BaseParameter.LoggerData = new LoggerParameterData(BaseParameter, this.MyMeter);
        this.MyLoggerInfo.AllParameters[index1] = BaseParameter.LoggerData;
        if (BaseParameter.FullName == "DefaultFunction.Sta_Secounds")
          this.MyLoggerInfo.IndexOfTimeParameter = index1;
        ++index1;
      }
      return this.MyLoggerInfo;
    }

    internal bool ReprogramLoggerData(LoggerInfo OldLogger)
    {
      int[] numArray = new int[this.LoggerParameter.Count];
      bool flag = false;
      for (int index1 = 0; index1 < this.LoggerParameter.Count; ++index1)
      {
        numArray[index1] = -1;
        for (int index2 = 0; index2 < OldLogger.MyLogger.LoggerParameter.Count; ++index2)
        {
          if (this.LoggerParameter[index1].FullName == OldLogger.MyLogger.LoggerParameter[index2].FullName && this.LoggerParameter[index1].DifVifs == OldLogger.MyLogger.LoggerParameter[index2].DifVifs)
          {
            numArray[index1] = index2;
            if (this.LoggerParameter[index1].FullName != "DefaultFunction.Sta_Secounds")
            {
              flag = true;
              break;
            }
            break;
          }
        }
      }
      if (!flag)
      {
        ZR_ClassLibMessages.AddErrorDescription("No logger data usable from logger: " + OldLogger.LoggerName);
        return true;
      }
      int num1 = OldLogger.LoggerData.Count;
      if (num1 > this.MaxEntries)
        num1 = this.MaxEntries;
      ByteField LoggerData = new ByteField(this.EntrySize * num1);
      for (int index3 = OldLogger.LoggerData.Count - num1; index3 < OldLogger.LoggerData.Count; ++index3)
      {
        for (int index4 = 0; index4 < this.LoggerParameter.Count; ++index4)
        {
          int size = this.LoggerParameter[index4].Size;
          long num2;
          if (this.LoggerParameter[index4].FullName == "DefaultFunction.Sta_Secounds")
            num2 = (long) ZR_Calendar.Cal_GetMeterTime(OldLogger.LoggerData.Keys[index3]);
          else if (numArray[index4] < 0)
          {
            num2 = 0L;
          }
          else
          {
            StringBuilder PValue = new StringBuilder(OldLogger.LoggerData.Values[index3][numArray[index4]]);
            int unitExponent = OldLogger.AllParameters[numArray[index4]].UnitExponent;
            ParameterService.ResetStringExpo(ref PValue, unitExponent * -1);
            num2 = long.Parse(PValue.ToString());
          }
          for (int index5 = 0; index5 < size; ++index5)
          {
            byte Byte = (byte) ((ulong) num2 & (ulong) byte.MaxValue);
            LoggerData.Add(Byte);
            num2 >>= 8;
          }
        }
      }
      return this.WriteDataToLogger(LoggerData);
    }

    internal bool WriteDataToLogger(ByteField LoggerData)
    {
      if (!this.MyMeter.MyCommunication.MyBus.WriteMemory(MemoryLocation.EEPROM, (int) this.P_StartAddress.ValueEprom, LoggerData))
        return false;
      this.P_WriteAddress.ValueEprom = this.P_StartAddress.ValueEprom + (long) LoggerData.Count;
      if (this.P_WriteAddress.ValueEprom >= this.P_EndAddress.ValueEprom)
      {
        this.P_WriteAddress.ValueEprom = this.P_StartAddress.ValueEprom;
        this.P_Flags.ValueEprom &= 128L;
      }
      else
        this.P_Flags.ValueEprom = 0L;
      if (ZR_Calendar.Cal_GetDateTime((uint) this.P_NextTimePoint.ValueEprom) <= DateTime.Now)
      {
        this.P_NextTimePoint.ValueEprom = (long) ZR_Calendar.Cal_GetMeterTime(this.NextEventDateTime);
        this.P_NextTimePoint.UpdateByteList();
        LoggerData = new ByteField(this.P_NextTimePoint.LinkByteList);
        if (!this.MyMeter.MyCommunication.MyBus.WriteMemory(MemoryLocation.EEPROM, this.P_NextTimePoint.Address, LoggerData))
          return false;
      }
      this.P_Flags.UpdateByteList();
      LoggerData = new ByteField(this.P_Flags.LinkByteList);
      if (!this.MyMeter.MyCommunication.MyBus.WriteMemory(MemoryLocation.EEPROM, this.P_Flags.Address, LoggerData))
        return false;
      this.P_WriteAddress.UpdateByteList();
      LoggerData = new ByteField(this.P_WriteAddress.LinkByteList);
      return this.MyMeter.MyCommunication.MyBus.WriteMemory(MemoryLocation.EEPROM, this.P_WriteAddress.Address, LoggerData);
    }

    public class IntervallCycleInfos
    {
      public uint Secounds;
      public string Description;
      public byte RuntimeCode;
      internal string RuntimeCodeString;

      internal IntervallCycleInfos(
        uint SecoundsIn,
        string DescriptionIn,
        byte RuntimeCodeIn,
        string RuntimeCodeStringIn)
      {
        this.Secounds = SecoundsIn;
        this.Description = DescriptionIn;
        this.RuntimeCode = RuntimeCodeIn;
        this.RuntimeCodeString = RuntimeCodeStringIn;
      }
    }
  }
}

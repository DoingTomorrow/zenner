// Decompiled with JetBrains decompiler
// Type: S3_Handler.LoggerConfiguration
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class LoggerConfiguration : S3_MemoryBlock
  {
    internal const uint IntervalConstMonth = 2629800;
    internal const uint IntervalConstHalfMonth = 1314900;
    internal const uint IntervalConstYear = 31557600;
    internal uint startTimeSeconds;
    internal uint intervallSeconds;
    internal ushort flags;
    internal ushort maxEntries;
    internal int numberOfChanals;

    public bool IsYearLogger => 31557600U == this.intervallSeconds;

    public bool IsMonthLogger => 2629800U == this.intervallSeconds;

    internal bool isRamLogger
    {
      get => ((uint) this.flags & 1U) > 0U;
      set
      {
        if (value)
          this.flags |= (ushort) 1;
        else
          this.flags &= (ushort) 65534;
      }
    }

    internal LoggerConfiguration(S3_Meter MyMeter, S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, S3_MemorySegment.LoggerHeader, parentMemoryBlock)
    {
    }

    internal LoggerConfiguration(
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      int insertIndex)
      : base(MyMeter, S3_MemorySegment.LoggerHeader, parentMemoryBlock, insertIndex)
    {
    }

    internal LoggerConfiguration(
      S3_Meter MyMeter,
      S3_MemoryBlock parentMemoryBlock,
      S3_MemoryBlock sourceMemoryBlock)
      : base(MyMeter, parentMemoryBlock, sourceMemoryBlock)
    {
    }

    internal bool Clone(S3_Meter cloneMeter)
    {
      LoggerConfiguration cloneLoggerConfig = new LoggerConfiguration(cloneMeter, cloneMeter.MyDeviceMemory.BlockLoggerTable, (S3_MemoryBlock) this);
      cloneLoggerConfig.startTimeSeconds = this.startTimeSeconds;
      cloneLoggerConfig.intervallSeconds = this.intervallSeconds;
      cloneLoggerConfig.flags = this.flags;
      cloneLoggerConfig.maxEntries = this.maxEntries != (ushort) 0 ? this.maxEntries : (ushort) 12;
      cloneLoggerConfig.numberOfChanals = this.numberOfChanals;
      if (this.sourceMemoryBlock != null)
        cloneLoggerConfig.sourceMemoryBlock = this.sourceMemoryBlock;
      foreach (LoggerChanal childMemoryBlock in this.childMemoryBlocks)
        childMemoryBlock.Clone(cloneMeter, cloneLoggerConfig);
      return true;
    }

    internal bool InsertLoggerChanal(
      int insertIndex,
      S3_Parameter chanalParameter,
      out LoggerChanal newChanal)
    {
      newChanal = new LoggerChanal(this.MyMeter, this, insertIndex, chanalParameter);
      return true;
    }

    internal bool CreateLoggerFromMemory(ref int nextLoggerAddress)
    {
      this.BlockStartAddress = nextLoggerAddress;
      this.StartAddressOfNextBlock = this.BlockStartAddress + 16;
      this.firstChildMemoryBlockOffset = this.ByteSize;
      this.startTimeSeconds = this.memStartTimeSeconds;
      this.intervallSeconds = this.memIntervallSeconds;
      this.flags = this.memFlags;
      this.maxEntries = this.memMaxEntries;
      if (LoggerManager.LoggerManagerLogger.IsInfoEnabled)
      {
        DateTime dateTime = ZR_Calendar.Cal_GetDateTime(this.startTimeSeconds);
        string intervalInfo = ZR_Calendar.Cal_GetIntervalInfo(this.intervallSeconds);
        LoggerManager.LoggerManagerLogger.Info("Create logger! Start time: " + dateTime.ToString("dd.MM.yyyy HH:mm") + "; Intervall: " + intervalInfo);
      }
      nextLoggerAddress = (int) this.memNextLoggerPtr;
      ushort chanalDescriptinPtr = this.memChanalDescriptinPtr;
      this.numberOfChanals = 0;
      while (chanalDescriptinPtr > (ushort) 0)
      {
        if (!new LoggerChanal(this.MyMeter, this).CreateLoggerChanalFromMemory(ref chanalDescriptinPtr))
          return false;
        ++this.numberOfChanals;
      }
      return true;
    }

    internal bool CrateLoggerFromBaseMeter(LoggerConfiguration baseLogger)
    {
      this.firstChildMemoryBlockOffset = baseLogger.firstChildMemoryBlockOffset;
      this.startTimeSeconds = baseLogger.startTimeSeconds;
      this.intervallSeconds = baseLogger.intervallSeconds;
      this.flags = baseLogger.flags;
      this.maxEntries = baseLogger.maxEntries;
      this.numberOfChanals = baseLogger.numberOfChanals;
      if (LoggerManager.LoggerManagerLogger.IsInfoEnabled)
      {
        DateTime dateTime = ZR_Calendar.Cal_GetDateTime(this.startTimeSeconds);
        string intervalInfo = ZR_Calendar.Cal_GetIntervalInfo(this.intervallSeconds);
        LoggerManager.LoggerManagerLogger.Info("Create logger from base meter! Start time: " + dateTime.ToString("dd.MM.yyyy HH:mm") + "; Intervall: " + intervalInfo);
      }
      foreach (LoggerChanal childMemoryBlock in baseLogger.childMemoryBlocks)
      {
        if (!new LoggerChanal(this.MyMeter, this).CreateLoggerChanalFromBaseMeter(childMemoryBlock))
          return false;
      }
      this.ByteSize = baseLogger.ByteSize;
      return true;
    }

    internal bool InsertLoggerData()
    {
      this.memStartTimeSeconds = this.startTimeSeconds;
      this.memIntervallSeconds = this.intervallSeconds;
      this.memFlags = this.flags;
      this.memMaxEntries = this.maxEntries;
      if (LoggerManager.LoggerManagerLogger.IsInfoEnabled)
      {
        DateTime dateTime = ZR_Calendar.Cal_GetDateTime(this.startTimeSeconds);
        string intervalInfo = ZR_Calendar.Cal_GetIntervalInfo(this.intervallSeconds);
        LoggerManager.LoggerManagerLogger.Info("Insert logger data! Start time: " + dateTime.ToString("dd.MM.yyyy HH:mm") + "; Intervall: " + intervalInfo);
      }
      foreach (LoggerChanal childMemoryBlock in this.childMemoryBlocks)
      {
        if (!childMemoryBlock.InsertLoggerData())
          return false;
      }
      return true;
    }

    internal uint memStartTimeSeconds
    {
      get => this.MyMeter.MyDeviceMemory.GetUintValue(this.BlockStartAddress);
      set
      {
        this.startTimeSeconds = value;
        this.MyMeter.MyDeviceMemory.SetUintValue(this.BlockStartAddress, value);
      }
    }

    internal uint memIntervallSeconds
    {
      get => this.MyMeter.MyDeviceMemory.GetUintValue(this.BlockStartAddress + 4);
      set
      {
        this.intervallSeconds = value;
        this.MyMeter.MyDeviceMemory.SetUintValue(this.BlockStartAddress + 4, value);
      }
    }

    internal ushort memFlags
    {
      get => this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress + 8);
      set
      {
        this.flags = value;
        this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress + 8, value);
      }
    }

    internal ushort memMaxEntries
    {
      get => this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress + 10);
      set
      {
        this.maxEntries = value;
        this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress + 10, value);
      }
    }

    internal ushort memChanalDescriptinPtr
    {
      get => this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress + 12);
      set => this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress + 12, value);
    }

    internal ushort memNextLoggerPtr
    {
      get => this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress + 14);
      set => this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress + 14, value);
    }

    internal bool RemoveLoggerChanalsByResources()
    {
      for (int index = this.childMemoryBlocks.Count - 1; index >= 0; --index)
      {
        LoggerChanal childMemoryBlock = (LoggerChanal) this.childMemoryBlocks[index];
        if (childMemoryBlock.chanalParameter != null && !this.MyMeter.MyResources.IsResourceAvailable(childMemoryBlock.chanalParameter.Statics.NeedResource, 0))
        {
          this.MyMeter.MyResources.ResourceEventDeleteObjectByMissedResource("Delete logger chanal. Parameter: " + childMemoryBlock.chanalParameter.Name, childMemoryBlock.chanalParameter.Statics.NeedResource);
          if (!this.RemoveLoggerChanal(childMemoryBlock))
            return false;
        }
      }
      return true;
    }

    internal bool RemoveLoggerChanal(LoggerChanal chanalToDelete)
    {
      if (LoggerManager.LoggerManagerLogger.IsInfoEnabled)
      {
        string str = "no! -> event";
        if (chanalToDelete.chanalParameter != null)
          str = chanalToDelete.chanalParameter.Name;
        LoggerManager.LoggerManagerLogger.Info("Remove logger chanal. Parameter: " + str);
      }
      return chanalToDelete.RemoveChanalData() && this.childMemoryBlocks.Remove((S3_MemoryBlock) chanalToDelete);
    }

    internal bool LinkLoggerChanals()
    {
      this.memChanalDescriptinPtr = (ushort) 0;
      LoggerChanal loggerChanal = (LoggerChanal) null;
      foreach (LoggerChanal childMemoryBlock in this.childMemoryBlocks)
      {
        if (loggerChanal != null)
          loggerChanal.memNextChanalDescriptionAdr = (ushort) childMemoryBlock.BlockStartAddress;
        else
          this.memChanalDescriptinPtr = (ushort) childMemoryBlock.BlockStartAddress;
        childMemoryBlock.memNextChanalDescriptionAdr = (ushort) 0;
        childMemoryBlock.memLoggerHeaderAdr = (ushort) this.BlockStartAddress;
        if (childMemoryBlock.chanalParameter != null)
          childMemoryBlock.memValuePtr = (ushort) childMemoryBlock.chanalParameter.BlockStartAddress;
        if (childMemoryBlock.chanalData != null)
          childMemoryBlock.memDataStartPtr = (ushort) childMemoryBlock.chanalData.BlockStartAddress;
        loggerChanal = childMemoryBlock;
      }
      return true;
    }

    internal bool ResizeLoggerCanalDataBlocks()
    {
      foreach (LoggerChanal childMemoryBlock in this.childMemoryBlocks)
      {
        if (!childMemoryBlock.ResizeLoggerCanalDataBlock())
          return false;
      }
      return true;
    }

    internal DateTime GetLastTime()
    {
      DateTime dateTime = ZR_Calendar.Cal_GetDateTime(this.startTimeSeconds);
      return this.intervallSeconds != 31557600U ? (this.intervallSeconds != 2629800U ? dateTime.AddSeconds((double) ((long) ((int) this.maxEntries - 1) * (long) this.intervallSeconds)) : dateTime.AddMonths((int) this.maxEntries - 1)) : dateTime.AddYears((int) this.maxEntries - 1);
    }

    internal LoggerActualData GetTimeData(uint RefTime)
    {
      LoggerActualData timeData = new LoggerActualData();
      timeData.RefTime = RefTime;
      if (timeData.RefTime < this.startTimeSeconds)
      {
        timeData.TimeLE_RefTime = 0U;
        timeData.TimeGT_RefTime = this.startTimeSeconds;
        timeData.VirtualWriteIndex = -1;
      }
      else if (this.intervallSeconds < 1314900U)
      {
        uint num = (timeData.RefTime - this.startTimeSeconds) / this.intervallSeconds;
        timeData.TimeLE_RefTime = this.startTimeSeconds + num * this.intervallSeconds;
        timeData.TimeGT_RefTime = timeData.TimeLE_RefTime + this.intervallSeconds;
        timeData.VirtualWriteIndex = (int) timeData.TimeLE_RefTime != (int) timeData.RefTime ? (int) num + 1 : (int) num;
      }
      else
      {
        DateTime dateTime = ZR_Calendar.Cal_GetDateTime(this.startTimeSeconds);
        DateTime date = dateTime.Date;
        uint num1 = this.startTimeSeconds - ZR_Calendar.Cal_GetMeterTime(date);
        dateTime = ZR_Calendar.Cal_GetDateTime(timeData.RefTime);
        DateTime TheTime = dateTime.Date;
        TheTime = this.intervallSeconds != 31557600U ? (this.intervallSeconds != 1314900U ? new DateTime(TheTime.Year, TheTime.Month, date.Day) : (TheTime.Day < 16 ? new DateTime(TheTime.Year, TheTime.Month, 1) : new DateTime(TheTime.Year, TheTime.Month, 16))) : new DateTime(TheTime.Year, date.Month, date.Day);
        uint num2 = ZR_Calendar.Cal_GetMeterTime(TheTime) + num1;
        if (num2 <= timeData.RefTime)
        {
          timeData.TimeLE_RefTime = num2;
          if (this.intervallSeconds == 31557600U)
            TheTime = TheTime.AddYears(1);
          else if (this.intervallSeconds == 1314900U)
          {
            if (TheTime.Day >= 16)
            {
              TheTime = new DateTime(TheTime.Year, TheTime.Month, 1);
              TheTime = TheTime.AddMonths(1);
            }
            else
              TheTime = new DateTime(TheTime.Year, TheTime.Month, 16);
          }
          else
            TheTime = TheTime.AddMonths(1);
          uint meterTime = ZR_Calendar.Cal_GetMeterTime(TheTime);
          timeData.TimeGT_RefTime = meterTime + num1;
        }
        else
        {
          timeData.TimeGT_RefTime = num2;
          if (this.intervallSeconds == 31557600U)
            TheTime = TheTime.AddYears(-1);
          else if (this.intervallSeconds == 1314900U)
          {
            if (TheTime.Day >= 16)
            {
              TheTime = new DateTime(TheTime.Year, TheTime.Month, 1);
            }
            else
            {
              TheTime = new DateTime(TheTime.Year, TheTime.Month, 16);
              TheTime = TheTime.AddMonths(-1);
            }
          }
          else
            TheTime = TheTime.AddMonths(-1);
          uint meterTime = ZR_Calendar.Cal_GetMeterTime(TheTime);
          timeData.TimeLE_RefTime = meterTime + num1;
        }
        uint num3 = timeData.RefTime <= timeData.TimeLE_RefTime ? timeData.TimeLE_RefTime : timeData.TimeGT_RefTime;
        timeData.VirtualWriteIndex = ((int) num3 - (int) this.startTimeSeconds + (int) this.intervallSeconds / 2) / (int) this.intervallSeconds;
      }
      if (timeData.VirtualWriteIndex >= (int) this.maxEntries)
      {
        int num = !this.isRamLogger ? 0 : (this.maxEntries > (ushort) 0 ? 1 : 0);
        timeData.WriteIndex = num == 0 ? -1 : timeData.VirtualWriteIndex % (int) this.maxEntries;
      }
      else
        timeData.WriteIndex = timeData.VirtualWriteIndex;
      if (timeData.VirtualWriteIndex < 1)
      {
        timeData.ReadIndex = -1;
        timeData.VirtualReadIndex = -1;
      }
      else if (timeData.WriteIndex == -1)
      {
        timeData.ReadIndex = (int) this.maxEntries - 1;
        timeData.VirtualReadIndex = timeData.ReadIndex;
      }
      else
      {
        timeData.ReadIndex = timeData.WriteIndex - 1;
        timeData.VirtualReadIndex = timeData.VirtualWriteIndex - 1;
        if (timeData.ReadIndex < 0)
          timeData.ReadIndex += (int) this.maxEntries;
      }
      timeData.BuildDateTimeValues();
      return timeData;
    }

    internal bool ResetStartTime(DateTime timeNow)
    {
      DateTime TheTime1 = timeNow.AddSeconds(-1.0);
      DateTime TheTime2 = ZR_Calendar.Cal_GetDateTime(this.startTimeSeconds);
      if (TheTime1 < TheTime2)
      {
        while (timeNow <= TheTime2)
          TheTime2 = TheTime2.AddYears(-1);
        this.startTimeSeconds = ZR_Calendar.Cal_GetMeterTime(TheTime2);
      }
      this.startTimeSeconds = this.GetTimeData(ZR_Calendar.Cal_GetMeterTime(TheTime1)).TimeLE_RefTime;
      return this.MyMeter.MyDeviceMemory.SetUintValue(this.BlockStartAddress, this.startTimeSeconds);
    }

    internal bool SetChanalToMaximalValueLogger(LoggerChanal theChanal, int ramInsertIndex)
    {
      theChanal.RemoveChanalData();
      theChanal.ByteSize += 2;
      theChanal.EntryBytes += 4;
      theChanal.isReducedRamChanal = true;
      theChanal.isCleareSource = true;
      theChanal.chanalMaxEntries = (ushort) 12;
      theChanal.hasTimeStamp = true;
      theChanal.chanalData = (LoggerChanalDataBase) new LoggerChanalData(this.MyMeter, theChanal, this.MyMeter.MyDeviceMemory.BlockLoggerRamData, ramInsertIndex);
      return true;
    }

    internal bool SetChanalToMaximalValueGenerator(LoggerChanal theChanal)
    {
      S3_MemoryBlock insertBlock;
      int insertIndex;
      theChanal.GetChanalDataInsertInfo(out insertBlock, out insertIndex);
      theChanal.isMaxValues = true;
      theChanal.RemoveChanalData();
      theChanal.chanalData = (LoggerChanalDataBase) new LoggerMaxBlockChanalData(this.MyMeter, theChanal, insertBlock, insertIndex);
      return true;
    }

    internal bool SetStartTime(DateTime timeToSet)
    {
      this.startTimeSeconds = ZR_Calendar.Cal_GetMeterTime(timeToSet);
      return this.MyMeter.MyDeviceMemory.SetUintValue(this.BlockStartAddress, this.startTimeSeconds);
    }

    private string GetIntervalString(uint interval)
    {
      switch (interval)
      {
        case 3600:
          return "hour";
        case 86400:
          return "day";
        case 604800:
          return "week";
        case 1314900:
          return "half_month";
        case 2629800:
          return "month";
        case 31557600:
          return "year";
        default:
          return "seconds(" + interval.ToString() + ")";
      }
    }

    public string IntervalString => this.GetIntervalString(this.intervallSeconds);

    public string Name
    {
      get
      {
        string name;
        switch (this.intervallSeconds)
        {
          case 3600:
            name = "LC_Hour_";
            break;
          case 86400:
            name = "LC_Day_";
            break;
          case 604800:
            name = "LC_Week_";
            break;
          case 1314900:
          case 2629800:
            name = "LC_Month_";
            break;
          case 31557600:
            name = "LC_Year_";
            break;
          default:
            throw new NotImplementedException();
        }
        int num = 0;
        foreach (LoggerConfiguration childMemoryBlock in this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks)
        {
          if (childMemoryBlock != this)
          {
            if ((int) childMemoryBlock.intervallSeconds == (int) this.intervallSeconds)
              ++num;
          }
          else
            break;
        }
        if (num > 0)
          name = name + "[" + num.ToString() + "]_";
        return name;
      }
    }

    internal void PrintLogger(StringBuilder printText)
    {
      string str1 = "   ";
      string startString = str1 + "   ";
      string str2 = this.intervallSeconds != 2629800U ? (this.intervallSeconds != 1314900U ? (this.intervallSeconds != 31557600U ? (this.intervallSeconds != 86400U ? (this.intervallSeconds != 3600U ? this.intervallSeconds.ToString() + "sec" : "Hour") : "Day") : "Year") : "Half month") : "Month";
      if (this.isRamLogger)
        printText.AppendLine(str1 + "Ram logger. Intervall: " + str2 + " ; Maximal number of entries: " + this.maxEntries.ToString());
      else
        printText.AppendLine(str1 + "Flash logger. Intervall: " + str2 + " ; Maximal number of entries: " + this.maxEntries.ToString());
      foreach (LoggerChanal childMemoryBlock in this.childMemoryBlocks)
        childMemoryBlock.PrintChanal(printText, startString);
    }

    internal enum LoggerFlags
    {
      IsRamLogger = 1,
    }
  }
}

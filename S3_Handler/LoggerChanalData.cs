// Decompiled with JetBrains decompiler
// Type: S3_Handler.LoggerChanalData
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using System.Data;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class LoggerChanalData : LoggerChanalDataBase
  {
    internal LoggerChanal myChanalInfo;
    internal int[] chanalAdr;
    internal ulong[] chanalData;
    internal uint[] chanalTimeStamp;
    private DataTable chanalDataTable;

    internal int BytesPerEntry
    {
      get
      {
        if (this.myChanalInfo.chanalParameter == null)
          return 0;
        int byteSize = this.myChanalInfo.chanalParameter.ByteSize;
        if (this.myChanalInfo.hasTimeStamp)
          byteSize += 4;
        return byteSize;
      }
    }

    internal LoggerChanalData(
      S3_Meter MyMeter,
      LoggerChanal myChanalInfo,
      S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, S3_MemorySegment.LoggerChanalData, parentMemoryBlock)
    {
      this.myChanalInfo = myChanalInfo;
      this.ByteSize = (int) this.myChanalInfo.chanalMaxEntries * this.BytesPerEntry;
    }

    internal LoggerChanalData(
      S3_Meter MyMeter,
      LoggerChanal myChanalInfo,
      S3_MemoryBlock parentMemoryBlock,
      int insertIndex)
      : base(MyMeter, S3_MemorySegment.LoggerChanalData, parentMemoryBlock, insertIndex)
    {
      this.myChanalInfo = myChanalInfo;
      this.ByteSize = (int) this.myChanalInfo.chanalMaxEntries * this.BytesPerEntry;
    }

    internal LoggerChanalData(
      S3_Meter MyMeter,
      LoggerChanal myChanalInfo,
      S3_MemoryBlock parentMemoryBlock,
      S3_MemoryBlock sourceMemoryBlock)
      : base(MyMeter, parentMemoryBlock, sourceMemoryBlock)
    {
      this.myChanalInfo = myChanalInfo;
      this.ByteSize = (int) this.myChanalInfo.chanalMaxEntries * this.BytesPerEntry;
    }

    internal override void Clone(
      S3_Meter cloneMeter,
      LoggerChanal cloneChanalInfo,
      S3_MemoryBlock cloneParentMemoryBlock)
    {
      LoggerChanalData loggerChanalData = new LoggerChanalData(cloneMeter, cloneChanalInfo, cloneParentMemoryBlock, (S3_MemoryBlock) this);
      if (this.chanalAdr != null)
      {
        loggerChanalData.chanalAdr = (int[]) this.chanalAdr.Clone();
        loggerChanalData.chanalData = (ulong[]) this.chanalData.Clone();
        if (this.chanalTimeStamp != null)
          loggerChanalData.chanalTimeStamp = (uint[]) this.chanalTimeStamp.Clone();
      }
      cloneChanalInfo.chanalData = (LoggerChanalDataBase) loggerChanalData;
      if (this.sourceMemoryBlock == null)
        return;
      cloneChanalInfo.chanalData.sourceMemoryBlock = this.sourceMemoryBlock;
    }

    internal override bool RemoveChanalDataObjects() => true;

    internal override bool CacheChanalData()
    {
      int blockStartAddress = this.BlockStartAddress;
      if (this.myChanalInfo.chanalParameter.ByteSize > 0 && this.myChanalInfo.chanalMaxEntries > (ushort) 0)
      {
        this.chanalAdr = new int[(int) this.myChanalInfo.chanalMaxEntries];
        this.chanalData = new ulong[(int) this.myChanalInfo.chanalMaxEntries];
        if (this.myChanalInfo.hasTimeStamp)
          this.chanalTimeStamp = new uint[(int) this.myChanalInfo.chanalMaxEntries];
        for (int index = 0; index < (int) this.myChanalInfo.chanalMaxEntries; ++index)
        {
          this.chanalAdr[index] = blockStartAddress;
          this.chanalData[index] = this.MyMeter.MyDeviceMemory.GetUlongValue(blockStartAddress, this.myChanalInfo.chanalParameter.ByteSize);
          blockStartAddress += this.myChanalInfo.chanalParameter.ByteSize;
          if (this.myChanalInfo.hasTimeStamp)
          {
            this.chanalTimeStamp[index] = this.MyMeter.MyDeviceMemory.GetUintValue(blockStartAddress);
            blockStartAddress += 4;
          }
        }
      }
      return true;
    }

    internal override DataTable GetChanalData(bool physicalView)
    {
      this.chanalDataTable = new DataTable("chanal data");
      this.chanalDataTable.Columns.Add(new DataColumn("adr", typeof (uint)));
      this.chanalDataTable.Columns.Add(new DataColumn("time", typeof (DateTime)));
      this.chanalDataTable.Columns.Add(new DataColumn("hex", typeof (ulong)));
      this.chanalDataTable.Columns.Add(new DataColumn("dec", typeof (Decimal)));
      this.chanalDataTable.Columns.Add(new DataColumn("unit", typeof (string)));
      if (this.myChanalInfo.hasTimeStamp)
        this.chanalDataTable.Columns.Add(new DataColumn("timeStamp", typeof (DateTime)));
      LoggerActualData timeData = this.myChanalInfo.myLoggerConfig.GetTimeData(this.MyMeter.MeterTimeAsSeconds1980);
      int readIndex = timeData.ReadIndex;
      int virtualReadIndex = timeData.VirtualReadIndex;
      if (readIndex >= (int) this.myChanalInfo.chanalMaxEntries)
        readIndex %= (int) this.myChanalInfo.chanalMaxEntries;
      Decimal num = 1M;
      string str = "-";
      if (this.myChanalInfo.chanalParameter != null)
      {
        ResolutionData resolution = this.myChanalInfo.chanalParameter.GetResolution();
        if (resolution != null)
        {
          num = 1M / resolution.displayFactor;
          str = resolution.resolutionString;
        }
      }
      if (!physicalView)
      {
        DateTime dateTime = ZR_Calendar.Cal_GetDateTime(this.myChanalInfo.myLoggerConfig.startTimeSeconds);
        for (int index = 0; index < (int) this.myChanalInfo.chanalMaxEntries; ++index)
        {
          if (readIndex < 0)
          {
            if (virtualReadIndex >= 0)
              readIndex += (int) this.myChanalInfo.chanalMaxEntries;
            else
              break;
          }
          DateTime indexTime = this.GetIndexTime(virtualReadIndex);
          if (!(indexTime < dateTime))
          {
            if (this.chanalAdr == null)
              this.CacheChanalData();
            DataRow row = this.chanalDataTable.NewRow();
            row[0] = (object) this.chanalAdr[readIndex];
            row[1] = (object) indexTime;
            row[2] = (object) this.chanalData[readIndex];
            row[3] = (object) ((Decimal) this.chanalData[readIndex] * num);
            row[4] = (object) str;
            if (this.myChanalInfo.hasTimeStamp)
              row[5] = (object) ZR_Calendar.Cal_GetDateTime(this.chanalTimeStamp[readIndex]);
            this.chanalDataTable.Rows.Add(row);
            --virtualReadIndex;
            --readIndex;
          }
          else
            break;
        }
      }
      else
      {
        DateTime dateTime1 = ZR_Calendar.Cal_GetDateTime(this.myChanalInfo.myLoggerConfig.startTimeSeconds);
        DateTime dateTime2 = virtualReadIndex >= 0 ? this.GetIndexTime(virtualReadIndex - readIndex) : dateTime1;
        for (int index = 0; index < (int) this.myChanalInfo.chanalMaxEntries; ++index)
        {
          DataRow row = this.chanalDataTable.NewRow();
          row[0] = (object) this.chanalAdr[index];
          row[1] = (object) dateTime2;
          row[2] = (object) this.chanalData[index];
          row[3] = (object) ((Decimal) this.chanalData[index] * num);
          row[4] = (object) str;
          if (this.myChanalInfo.hasTimeStamp)
            row[5] = (object) ZR_Calendar.Cal_GetDateTime(this.chanalTimeStamp[index]);
          this.chanalDataTable.Rows.Add(row);
          bool flag = true;
          if (index == readIndex && this.myChanalInfo.isRamChanal)
          {
            DateTime indexTime = this.GetIndexTime(virtualReadIndex - (int) this.myChanalInfo.chanalMaxEntries + 1);
            if (indexTime >= dateTime1)
            {
              dateTime2 = indexTime;
              flag = false;
            }
          }
          if (flag)
          {
            switch (this.myChanalInfo.myLoggerConfig.intervallSeconds)
            {
              case 1314900:
                if (dateTime2.Day >= 16)
                {
                  dateTime2 = new DateTime(dateTime2.Year, dateTime2.Month, 1);
                  dateTime2 = dateTime2.AddMonths(1);
                  break;
                }
                dateTime2 = new DateTime(dateTime2.Year, dateTime2.Month, 16);
                break;
              case 2629800:
                dateTime2 = dateTime2.AddMonths(1);
                break;
              case 31557600:
                dateTime2 = dateTime2.AddYears(1);
                break;
              default:
                dateTime2 = dateTime2.AddSeconds((double) this.myChanalInfo.myLoggerConfig.intervallSeconds);
                break;
            }
          }
        }
      }
      return this.chanalDataTable;
    }

    internal override bool ChangeChanalData()
    {
      int byteSize = this.myChanalInfo.chanalParameter.ByteSize;
      if (this.myChanalInfo.hasTimeStamp)
        byteSize += 4;
      for (int index = 0; index < (int) this.myChanalInfo.chanalMaxEntries; ++index)
      {
        ulong NewValue = (ulong) ((Decimal) this.chanalDataTable.Rows[index][3] * 1000M);
        if ((long) this.chanalData[index] != (long) NewValue)
        {
          this.chanalData[index] = NewValue;
          if (!this.MyMeter.MyDeviceMemory.SetUlongValue(this.BlockStartAddress + index * byteSize, byteSize, NewValue))
            return false;
        }
      }
      return true;
    }

    internal override bool InsertChanalData()
    {
      if (this.sourceMemoryBlock != null)
      {
        if (this.sourceMemoryBlock == this)
          return true;
        if (this.sourceMemoryBlock == S3_MemoryBlock.NoSource)
          return this.ResetChanalData();
        LoggerChanalData sourceMemoryBlock = (LoggerChanalData) this.sourceMemoryBlock;
        if (this.sourceMemoryBlock.ByteSize == this.ByteSize && sourceMemoryBlock.myChanalInfo.ChanalName == this.myChanalInfo.ChanalName && sourceMemoryBlock.myChanalInfo.chanalParameter.GetUnit() == this.myChanalInfo.chanalParameter.GetUnit())
        {
          this.InsertDataFromSourceMemoryBlockIncludeChildBlocks();
          LoggerManager.LoggerManagerLogger.Trace("Insert old chanal data. Chanal: " + this.myChanalInfo.ChanalName + " ; copy from 0x" + sourceMemoryBlock.BlockStartAddress.ToString("x04") + " to 0x" + this.BlockStartAddress.ToString("x04"));
        }
        else
        {
          LoggerManager.LoggerManagerLogger.Trace("Old chanal data not compatible. Reset chanal data: " + this.myChanalInfo.ChanalName);
          this.ResetChanalData();
        }
      }
      else
      {
        LoggerManager.LoggerManagerLogger.Trace("Old chanal data not available. Reset chanal data: " + this.myChanalInfo.ChanalName);
        this.ResetChanalData();
      }
      return true;
    }

    internal override bool ResetChanalData()
    {
      int byteSize = this.myChanalInfo.chanalParameter.ByteSize;
      if (this.myChanalInfo.hasTimeStamp)
        byteSize += 4;
      int blockStartAddress = this.BlockStartAddress;
      for (int index = 0; index < (int) this.myChanalInfo.chanalMaxEntries; ++index)
      {
        if (this.myChanalInfo.isRamChanal)
        {
          if (!this.MyMeter.MyDeviceMemory.FillMemory(blockStartAddress, byteSize, (byte) 0))
            return false;
        }
        else if (!this.MyMeter.MyDeviceMemory.FillMemory(blockStartAddress, byteSize, byte.MaxValue))
          return false;
        blockStartAddress += byteSize;
      }
      this.sourceMemoryBlock = (S3_MemoryBlock) this;
      return true;
    }

    internal override bool ResetChanalDataToOneStoredValue()
    {
      if (!this.ResetChanalData())
        return false;
      if (this.myChanalInfo.chanalParameter != null)
      {
        ulong ulongValue = this.MyMeter.MyDeviceMemory.GetUlongValue(this.myChanalInfo.chanalParameter.BlockStartAddress, this.myChanalInfo.chanalParameter.ByteSize);
        if (!this.myChanalInfo.IsYearLogger && !this.MyMeter.MyDeviceMemory.SetUlongValue(this.BlockStartAddress, this.myChanalInfo.chanalParameter.ByteSize, ulongValue) || this.myChanalInfo.hasTimeStamp && !this.MyMeter.MyDeviceMemory.SetUintValue(this.BlockStartAddress + this.myChanalInfo.chanalParameter.ByteSize, this.myChanalInfo.myLoggerConfig.startTimeSeconds))
          return false;
      }
      if (!this.CacheChanalData())
        return false;
      this.sourceMemoryBlock = (S3_MemoryBlock) this;
      return true;
    }

    internal override bool FillChanalTestData(ref ulong testValue)
    {
      int byteSize = this.myChanalInfo.chanalParameter.ByteSize;
      int blockStartAddress = this.BlockStartAddress;
      int num = (int) testValue / 1000;
      DateTime TheTime = new DateTime(num + 2000, 1, 1, num % 24, 0, 0);
      for (int index = 0; index < (int) this.myChanalInfo.chanalMaxEntries; ++index)
      {
        if (!this.MyMeter.MyDeviceMemory.SetUlongValue(blockStartAddress, byteSize, testValue))
          return false;
        blockStartAddress += byteSize;
        ++testValue;
        if (this.myChanalInfo.hasTimeStamp)
        {
          if (!this.MyMeter.MyDeviceMemory.SetUintValue(blockStartAddress, ZR_Calendar.Cal_GetMeterTime(TheTime)))
            return false;
          blockStartAddress += 4;
          TheTime = TheTime.AddMonths(1);
          TheTime = TheTime.AddMinutes(1.0);
        }
        if (testValue % 1000UL == 0UL)
          testValue -= 1000UL;
      }
      testValue -= testValue % 1000UL;
      testValue += 1000UL;
      this.sourceMemoryBlock = (S3_MemoryBlock) this;
      return true;
    }

    internal DateTime GetIndexTime(int virtualIndex)
    {
      DateTime dateTime = ZR_Calendar.Cal_GetDateTime(this.myChanalInfo.myLoggerConfig.startTimeSeconds);
      DateTime indexTime;
      if (this.myChanalInfo.myLoggerConfig.intervallSeconds == 31557600U)
        indexTime = dateTime.AddYears(virtualIndex);
      else if (this.myChanalInfo.myLoggerConfig.intervallSeconds == 2629800U)
        indexTime = dateTime.AddMonths(virtualIndex);
      else if (this.myChanalInfo.myLoggerConfig.intervallSeconds == 1314900U)
      {
        indexTime = dateTime.AddMonths(virtualIndex >> 1);
        if (dateTime.Day < 16)
        {
          if (virtualIndex % 2 != 0)
            indexTime = new DateTime(indexTime.Year, indexTime.Month, 16);
        }
        else if (virtualIndex % 2 != 0)
        {
          indexTime = indexTime.AddMonths(1);
          indexTime = new DateTime(indexTime.Year, indexTime.Month, 1);
        }
      }
      else
        indexTime = dateTime.AddSeconds((double) ((long) this.myChanalInfo.myLoggerConfig.intervallSeconds * (long) virtualIndex));
      return indexTime;
    }
  }
}

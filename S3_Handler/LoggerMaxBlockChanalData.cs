// Decompiled with JetBrains decompiler
// Type: S3_Handler.LoggerMaxBlockChanalData
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System;
using System.Collections.Generic;
using System.Data;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class LoggerMaxBlockChanalData : LoggerChanalDataBase
  {
    internal LoggerChanal myChanalInfo;
    internal string[] canalParameterNames = new string[5];
    internal uint maxValueAbs;
    internal uint maxValueAbsTimePoint;
    internal uint maxValueMonth;
    internal uint maxValueMonthTimePoint;
    internal uint lastValue;
    private DataTable chanalDataTable;

    internal LoggerMaxBlockChanalData(
      S3_Meter MyMeter,
      LoggerChanal myChanalInfo,
      S3_MemoryBlock parentMemoryBlock)
      : base(MyMeter, S3_MemorySegment.LoggerChanalData, parentMemoryBlock)
    {
      this.myChanalInfo = myChanalInfo;
      this.CreateLoggerHeapParameters();
    }

    internal LoggerMaxBlockChanalData(
      S3_Meter MyMeter,
      LoggerChanal myChanalInfo,
      S3_MemoryBlock parentMemoryBlock,
      S3_MemoryBlock sourceMemoryBlock)
      : base(MyMeter, S3_MemorySegment.LoggerChanalData, parentMemoryBlock)
    {
      this.sourceMemoryBlock = sourceMemoryBlock;
      this.myChanalInfo = myChanalInfo;
      this.CreateLoggerHeapParameters();
    }

    internal LoggerMaxBlockChanalData(
      S3_Meter MyMeter,
      LoggerChanal myChanalInfo,
      S3_MemoryBlock parentMemoryBlock,
      int insertIndex)
      : base(MyMeter, S3_MemorySegment.LoggerChanalData, parentMemoryBlock, insertIndex)
    {
      this.myChanalInfo = myChanalInfo;
      this.CreateLoggerHeapParameters();
    }

    internal bool CreateLoggerHeapParameters()
    {
      this.canalParameterNames[0] = this.myChanalInfo.chanalParameter.Name + "_MaxValueAbs";
      this.canalParameterNames[1] = this.myChanalInfo.chanalParameter.Name + "_MaxValueAbsTimePoint";
      this.canalParameterNames[2] = this.myChanalInfo.chanalParameter.Name + "_MaxValueMonth";
      this.canalParameterNames[3] = this.myChanalInfo.chanalParameter.Name + "_MaxValueMonthTimePoint";
      this.canalParameterNames[4] = this.myChanalInfo.chanalParameter.Name + "_LastSavedValue";
      if (this.MyMeter.MyParameters.ParameterByName.ContainsKey(this.canalParameterNames[0]))
        return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "LoggerMaxBlockChanalData: parameter not deleted!");
      for (int index = 0; index < this.canalParameterNames.Length; ++index)
      {
        if (this.sourceMemoryBlock == null)
        {
          if (this.MyMeter.MyParameters.AddNewHeapParameterByName(this.canalParameterNames[index], (S3_MemoryBlock) this) == null)
            return false;
        }
        else if (this.MyMeter.MyParameters.AddNewHeapParameterByName(this.canalParameterNames[index], (S3_MemoryBlock) this, this.sourceMemoryBlock.MyMeter) == null)
          return false;
      }
      return true;
    }

    internal LoggerMaxBlockChanalData(
      S3_Meter MyMeter,
      LoggerChanal myChanalInfo,
      S3_MemoryBlock parentMemoryBlock,
      LoggerMaxBlockChanalData sourceMaxChanalDataBlock)
      : base(MyMeter, parentMemoryBlock, (S3_MemoryBlock) sourceMaxChanalDataBlock)
    {
      this.myChanalInfo = myChanalInfo;
      this.canalParameterNames = sourceMaxChanalDataBlock.canalParameterNames;
      SortedList<string, S3_Parameter> parameterByName = sourceMaxChanalDataBlock.MyMeter.MyParameters.ParameterByName;
      for (int index = 0; index < this.canalParameterNames.Length; ++index)
      {
        S3_Parameter s3Parameter = parameterByName[this.canalParameterNames[index]];
        S3_Parameter TheParameter = s3Parameter.Clone(this.MyMeter, (S3_MemoryBlock) this);
        TheParameter.sourceMemoryBlock = (S3_MemoryBlock) s3Parameter;
        this.MyMeter.MyParameters.AddParameter(TheParameter);
      }
    }

    internal override void Clone(
      S3_Meter cloneMeter,
      LoggerChanal cloneChanalInfo,
      S3_MemoryBlock cloneParentMemoryBlock)
    {
      LoggerMaxBlockChanalData maxBlockChanalData = new LoggerMaxBlockChanalData(cloneMeter, cloneChanalInfo, cloneParentMemoryBlock, this);
      maxBlockChanalData.maxValueAbs = this.maxValueAbs;
      maxBlockChanalData.maxValueAbsTimePoint = this.maxValueAbsTimePoint;
      maxBlockChanalData.maxValueMonth = this.maxValueMonth;
      maxBlockChanalData.maxValueMonthTimePoint = this.maxValueMonthTimePoint;
      maxBlockChanalData.lastValue = this.lastValue;
      if (this.sourceMemoryBlock != null)
        maxBlockChanalData.sourceMemoryBlock = this.sourceMemoryBlock;
      cloneChanalInfo.chanalData = (LoggerChanalDataBase) maxBlockChanalData;
    }

    internal override bool RemoveChanalDataObjects()
    {
      for (int index = 0; index < this.canalParameterNames.Length; ++index)
      {
        if (!this.MyMeter.MyParameters.RemoveHeapParameterByName(this.canalParameterNames[index]))
          return false;
      }
      return true;
    }

    internal override bool CacheChanalData()
    {
      int blockStartAddress = this.BlockStartAddress;
      this.maxValueAbs = this.MyMeter.MyDeviceMemory.GetUintValue(blockStartAddress);
      int Address1 = blockStartAddress + 4;
      this.maxValueAbsTimePoint = this.MyMeter.MyDeviceMemory.GetUintValue(Address1);
      int Address2 = Address1 + 4;
      this.maxValueMonth = this.MyMeter.MyDeviceMemory.GetUintValue(Address2);
      int Address3 = Address2 + 4;
      this.maxValueMonthTimePoint = this.MyMeter.MyDeviceMemory.GetUintValue(Address3);
      this.lastValue = this.MyMeter.MyDeviceMemory.GetUintValue(Address3 + 4);
      return true;
    }

    internal override DataTable GetChanalData(bool physicalView)
    {
      this.chanalDataTable = new DataTable("chanal data");
      this.chanalDataTable.Columns.Add(new DataColumn("adr", typeof (uint)));
      this.chanalDataTable.Columns.Add(new DataColumn("time", typeof (DateTime)));
      this.chanalDataTable.Columns.Add(new DataColumn("hex", typeof (ulong)));
      this.chanalDataTable.Columns.Add(new DataColumn("dec", typeof (Decimal)));
      this.chanalDataTable.Columns.Add(new DataColumn("name", typeof (string)));
      LoggerActualData timeData = this.myChanalInfo.myLoggerConfig.GetTimeData(this.MyMeter.MeterTimeAsSeconds1980);
      Decimal num = 1M;
      if (this.myChanalInfo.chanalParameter != null)
        num = 1M / this.myChanalInfo.chanalParameter.GetResolution().displayFactor;
      DataRow row1 = this.chanalDataTable.NewRow();
      row1[0] = (object) this.BlockStartAddress;
      row1[1] = (object) ZR_Calendar.Cal_GetDateTime(this.maxValueAbsTimePoint);
      row1[2] = (object) this.maxValueAbs;
      row1[3] = (object) ((Decimal) this.maxValueAbs * num);
      row1[4] = (object) "maxValAbs";
      this.chanalDataTable.Rows.Add(row1);
      DataRow row2 = this.chanalDataTable.NewRow();
      row2[0] = (object) (this.BlockStartAddress + 8);
      row2[1] = (object) ZR_Calendar.Cal_GetDateTime(this.maxValueMonthTimePoint);
      row2[2] = (object) this.maxValueMonth;
      row2[3] = (object) ((Decimal) this.maxValueMonth * num);
      row2[4] = (object) "maxValMonth";
      this.chanalDataTable.Rows.Add(row2);
      DataRow row3 = this.chanalDataTable.NewRow();
      row3[0] = (object) (this.BlockStartAddress + 16);
      row3[1] = (object) timeData.timeLE_RefTime.Date.AddHours((double) timeData.timeLE_RefTime.Hour);
      row3[2] = (object) this.lastValue;
      row3[3] = (object) ((Decimal) this.lastValue * num);
      row3[4] = (object) "lastVal";
      this.chanalDataTable.Rows.Add(row3);
      return this.chanalDataTable;
    }

    internal override bool ChangeChanalData() => true;

    internal override bool InsertChanalData()
    {
      if (this.sourceMemoryBlock != null)
      {
        if (this.sourceMemoryBlock == this)
          return true;
        if (this.sourceMemoryBlock == S3_MemoryBlock.NoSource)
          return this.ResetChanalData();
        if (this.sourceMemoryBlock.ByteSize == this.ByteSize && ((LoggerMaxBlockChanalData) this.sourceMemoryBlock).myChanalInfo.ChanalName == this.myChanalInfo.ChanalName && ((LoggerMaxBlockChanalData) this.sourceMemoryBlock).myChanalInfo.chanalParameter.GetUnit() == this.myChanalInfo.chanalParameter.GetUnit())
        {
          this.InsertDataFromSourceMemoryBlockIncludeChildBlocks();
          LoggerManager.LoggerManagerLogger.Trace("Insert old chanal data. Chanal: " + this.myChanalInfo.ChanalName);
        }
        else
        {
          LoggerManager.LoggerManagerLogger.Trace("Old chanal data not compatible. Chanal: " + this.myChanalInfo.ChanalName);
          this.ResetChanalData();
        }
      }
      this.CacheChanalData();
      return true;
    }

    internal override bool ResetChanalData()
    {
      this.MyMeter.MyDeviceMemory.SetUintValue(this.BlockStartAddress, 0U);
      this.MyMeter.MyDeviceMemory.SetUintValue(this.BlockStartAddress + 4, this.myChanalInfo.myLoggerConfig.startTimeSeconds);
      this.MyMeter.MyDeviceMemory.SetUintValue(this.BlockStartAddress + 8, 0U);
      this.MyMeter.MyDeviceMemory.SetUintValue(this.BlockStartAddress + 12, this.myChanalInfo.myLoggerConfig.startTimeSeconds);
      if (!this.MyMeter.MyDeviceMemory.SetUlongValue(this.BlockStartAddress + 16, this.myChanalInfo.chanalParameter.ByteSize, this.MyMeter.MyDeviceMemory.GetUlongValue(this.myChanalInfo.chanalParameter.BlockStartAddress, this.myChanalInfo.chanalParameter.ByteSize)))
        return false;
      this.sourceMemoryBlock = (S3_MemoryBlock) this;
      this.CacheChanalData();
      return true;
    }

    internal override bool ResetChanalDataToOneStoredValue() => this.ResetChanalData();

    internal override bool FillChanalTestData(ref ulong testValue)
    {
      int blockStartAddress = this.BlockStartAddress;
      DateTime TheTime = new DateTime((int) testValue / 1000 + 2000, 1, 1);
      this.MyMeter.MyDeviceMemory.SetUintValue(blockStartAddress, (uint) testValue);
      ++testValue;
      int Address1 = blockStartAddress + 4;
      this.MyMeter.MyDeviceMemory.SetUintValue(Address1, ZR_Calendar.Cal_GetMeterTime(TheTime));
      TheTime = TheTime.AddMonths(1);
      int Address2 = Address1 + 4;
      this.MyMeter.MyDeviceMemory.SetUintValue(Address2, (uint) testValue);
      ++testValue;
      int Address3 = Address2 + 4;
      this.MyMeter.MyDeviceMemory.SetUintValue(Address3, ZR_Calendar.Cal_GetMeterTime(TheTime));
      TheTime = TheTime.AddMonths(1);
      this.MyMeter.MyDeviceMemory.SetUintValue(Address3 + 4, (uint) testValue);
      testValue -= testValue % 1000UL;
      testValue += 1000UL;
      this.sourceMemoryBlock = (S3_MemoryBlock) this;
      return true;
    }
  }
}

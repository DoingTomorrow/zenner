// Decompiled with JetBrains decompiler
// Type: S3_Handler.LoggerChanal
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class LoggerChanal : S3_MemoryBlock
  {
    internal ushort chanalFlags;
    internal ushort loggerHeaderAdr;
    internal ushort dataStartPtr;
    internal ushort chanalMaxEntries;
    internal LoggerConfiguration myLoggerConfig;
    internal LoggerChanalDataBase chanalData;
    internal S3_Parameter chanalParameter;

    internal int EntryBytes
    {
      get => (int) this.chanalFlags & 15;
      set
      {
        if (value > 15)
          ZR_ClassLibMessages.AddErrorDescriptionAndException(ZR_ClassLibMessages.LastErrors.IllegalData, "Logger entry size to much.", LoggerManager.LoggerManagerLogger);
        this.chanalFlags = (ushort) ((int) this.chanalFlags & -16 | value);
      }
    }

    internal bool isGenerateMidnightEvent => ((uint) this.chanalFlags & 256U) > 0U;

    internal bool isStartRuntimeFunction => ((uint) this.chanalFlags & 512U) > 0U;

    internal bool isMaxValues
    {
      get => ((uint) this.chanalFlags & 1024U) > 0U;
      set
      {
        if (value)
          this.chanalFlags |= (ushort) 1024;
        else
          this.chanalFlags &= (ushort) 64511;
      }
    }

    internal bool isCleareSource
    {
      get => ((uint) this.chanalFlags & 4096U) > 0U;
      set
      {
        if (value)
          this.chanalFlags |= (ushort) 4096;
        else
          this.chanalFlags &= (ushort) 61439;
      }
    }

    internal bool hasTimeStamp
    {
      get => ((uint) this.chanalFlags & 8192U) > 0U;
      set
      {
        if (value)
          this.chanalFlags |= (ushort) 8192;
        else
          this.chanalFlags &= (ushort) 57343;
      }
    }

    internal bool isReducedRamChanal
    {
      get => ((uint) this.chanalFlags & 16384U) > 0U;
      set
      {
        if (value)
          this.chanalFlags |= (ushort) 16384;
        else
          this.chanalFlags &= (ushort) 49151;
      }
    }

    internal bool hasChanalData => !this.isGenerateMidnightEvent && !this.isStartRuntimeFunction;

    internal bool isRamChanal => this.isReducedRamChanal || this.myLoggerConfig.isRamLogger;

    internal int ByteSizeOfDataBlock => this.chanalData == null ? 0 : this.chanalData.ByteSize;

    internal string ChanalName
    {
      get
      {
        if (this.chanalParameter != null)
        {
          if (this.isMaxValues)
            return this.myLoggerConfig.Name + "Max_" + this.chanalParameter.Name;
          return this.isCleareSource ? this.myLoggerConfig.Name + "Res_" + this.chanalParameter.Name : this.myLoggerConfig.Name + this.chanalParameter.Name;
        }
        if (this.isGenerateMidnightEvent)
          return this.myLoggerConfig.Name + "MidnightEvent";
        return this.isStartRuntimeFunction ? this.myLoggerConfig.Name + "StartRuntime" : this.myLoggerConfig.Name + "Unknown";
      }
    }

    internal LoggerChanal(S3_Meter MyMeter, LoggerConfiguration myLoggerConfig)
      : base(MyMeter, S3_MemorySegment.LoggerChanal, (S3_MemoryBlock) myLoggerConfig)
    {
      this.myLoggerConfig = myLoggerConfig;
    }

    internal LoggerChanal(
      S3_Meter MyMeter,
      LoggerConfiguration myLoggerConfig,
      int insertIndex,
      S3_Parameter chanalParameter)
      : base(MyMeter, S3_MemorySegment.LoggerChanal, (S3_MemoryBlock) myLoggerConfig, insertIndex)
    {
      this.myLoggerConfig = myLoggerConfig;
      this.chanalParameter = chanalParameter;
      if (!this.hasChanalData)
      {
        if (this.isStartRuntimeFunction)
          this.ByteSize = 8;
        else
          this.ByteSize = 6;
      }
      else
      {
        this.chanalMaxEntries = myLoggerConfig.maxEntries;
        if (myLoggerConfig.isRamLogger)
          this.ByteSize = 10;
        else if (this.isReducedRamChanal)
          this.ByteSize = 12;
        else
          this.ByteSize = 10;
      }
      if (!this.isGenerateMidnightEvent && !this.isMaxValues)
      {
        int byteSize = chanalParameter.ByteSize;
        int num;
        switch (byteSize)
        {
          case 1:
          case 2:
          case 4:
            num = 1;
            break;
          default:
            num = byteSize == 8 ? 1 : 0;
            break;
        }
        if (num != 0)
          this.chanalFlags |= (ushort) byteSize;
        else
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Parameter " + chanalParameter.Name + " has invalid size! Value: " + byteSize.ToString(), S3_Meter.S3_MeterLogger);
      }
      S3_MemoryBlock insertBlock;
      int insertIndex1;
      this.MyMeter.MyLoggerManager.GetDataBlockInsertInfo(this, out insertBlock, out insertIndex1);
      this.chanalData = (LoggerChanalDataBase) new LoggerChanalData(this.MyMeter, this, insertBlock, insertIndex1);
    }

    internal LoggerChanal(
      S3_Meter MyMeter,
      LoggerConfiguration myLoggerConfig,
      S3_MemoryBlock sourceMemoryBlock)
      : base(MyMeter, (S3_MemoryBlock) myLoggerConfig, sourceMemoryBlock)
    {
      this.myLoggerConfig = myLoggerConfig;
    }

    internal LoggerChanal Clone(S3_Meter cloneMeter, LoggerConfiguration cloneLoggerConfig)
    {
      LoggerChanal cloneChanalInfo = new LoggerChanal(cloneMeter, cloneLoggerConfig, (S3_MemoryBlock) this);
      cloneChanalInfo.chanalFlags = this.chanalFlags;
      cloneChanalInfo.loggerHeaderAdr = this.loggerHeaderAdr;
      cloneChanalInfo.dataStartPtr = this.dataStartPtr;
      cloneChanalInfo.chanalMaxEntries = this.chanalMaxEntries;
      if (this.sourceMemoryBlock != null)
        cloneChanalInfo.sourceMemoryBlock = this.sourceMemoryBlock;
      if (this.chanalParameter != null)
      {
        int index = cloneMeter.MyParameters.ParameterByName.IndexOfKey(this.chanalParameter.Name);
        if (index >= 0)
          cloneChanalInfo.chanalParameter = cloneMeter.MyParameters.ParameterByName.Values[index];
      }
      if (this.chanalData != null)
      {
        if (this.isRamChanal)
          this.chanalData.Clone(cloneMeter, cloneChanalInfo, cloneMeter.MyDeviceMemory.BlockLoggerRamData);
        else
          this.chanalData.Clone(cloneMeter, cloneChanalInfo, cloneMeter.MyDeviceMemory.BlockLoggerData);
      }
      return cloneChanalInfo;
    }

    internal LoggerChanalFunctions GetChanalFunction()
    {
      if (this.isStartRuntimeFunction)
        return LoggerChanalFunctions.RuntimeFunction;
      if (this.isGenerateMidnightEvent)
        return LoggerChanalFunctions.MidnightEvent;
      if (this.myLoggerConfig.isRamLogger)
        return this.chanalData is LoggerMaxBlockChanalData ? LoggerChanalFunctions.MaximalValueGenerator : LoggerChanalFunctions.RamChanal;
      if (!this.isRamChanal)
        return LoggerChanalFunctions.SingleWriteFlashChanal;
      return this.isCleareSource ? LoggerChanalFunctions.MaximalValueLogger : LoggerChanalFunctions.ReducedRamChanal;
    }

    internal bool RemoveChanalData()
    {
      if (this.chanalData == null)
        return true;
      return this.chanalData.RemoveChanalDataObjects() && this.chanalData.RemoveFromParentMemoryBlock();
    }

    internal bool ResizeLoggerCanalDataBlock()
    {
      if (this.chanalData != null && !(this.chanalData is LoggerMaxBlockChanalData) && (!this.isReducedRamChanal || (int) this.chanalMaxEntries > (int) this.myLoggerConfig.maxEntries) && (int) this.chanalMaxEntries != (int) this.myLoggerConfig.maxEntries)
      {
        this.chanalMaxEntries = this.myLoggerConfig.maxEntries;
        this.chanalData.ByteSize = (int) this.chanalMaxEntries * ((LoggerChanalData) this.chanalData).BytesPerEntry;
      }
      return true;
    }

    internal void GetChanalDataInsertInfo(out S3_MemoryBlock insertBlock, out int insertIndex)
    {
      insertBlock = this.chanalData.parentMemoryBlock;
      insertIndex = this.chanalData.parentMemoryBlock.childMemoryBlocks.IndexOf((S3_MemoryBlock) this.chanalData);
    }

    internal ushort memChanalFlags
    {
      get => this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress);
      set
      {
        this.chanalFlags = value;
        this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress, value);
      }
    }

    internal ushort memNextChanalDescriptionAdr
    {
      get => this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress + 2);
      set => this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress + 2, value);
    }

    internal ushort memLoggerHeaderAdr
    {
      get => this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress + 4);
      set
      {
        this.loggerHeaderAdr = value;
        this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress + 4, value);
      }
    }

    internal ushort memValuePtr
    {
      get => this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress + 6);
      set => this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress + 6, value);
    }

    internal ushort memDataStartPtr
    {
      get => this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress + 8);
      set
      {
        this.dataStartPtr = value;
        this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress + 8, value);
      }
    }

    internal ushort memChanalMaxEntries
    {
      get => this.MyMeter.MyDeviceMemory.GetUshortValue(this.BlockStartAddress + 10);
      set
      {
        this.chanalMaxEntries = value;
        this.MyMeter.MyDeviceMemory.SetUshortValue(this.BlockStartAddress + 10, value);
      }
    }

    internal bool CreateLoggerChanalFromMemory(ref ushort nextLoggerChanalAdr)
    {
      this.BlockStartAddress = (int) nextLoggerChanalAdr;
      this.chanalMaxEntries = this.myLoggerConfig.maxEntries;
      this.chanalFlags = this.memChanalFlags;
      this.loggerHeaderAdr = this.memLoggerHeaderAdr;
      nextLoggerChanalAdr = this.memNextChanalDescriptionAdr;
      if (this.hasChanalData)
      {
        this.AddLoggerChanalParameterFromMemory();
        this.dataStartPtr = this.memDataStartPtr;
        int memValuePtr = (int) this.memValuePtr;
        if (!this.isMaxValues)
        {
          if (this.isReducedRamChanal)
          {
            this.ByteSize = 12;
            this.chanalMaxEntries = this.memChanalMaxEntries;
          }
          else
            this.ByteSize = 10;
          if (this.isRamChanal)
          {
            this.chanalData = (LoggerChanalDataBase) new LoggerChanalData(this.MyMeter, this, this.MyMeter.MyDeviceMemory.BlockLoggerRamData);
          }
          else
          {
            if (this.MyMeter.MyDeviceMemory.BlockLoggerData.ByteSize == 0)
              this.MyMeter.MyDeviceMemory.BlockLoggerData.BlockStartAddress = (int) this.dataStartPtr;
            this.chanalData = (LoggerChanalDataBase) new LoggerChanalData(this.MyMeter, this, this.MyMeter.MyDeviceMemory.BlockLoggerData);
          }
        }
        else
        {
          this.ByteSize = 10;
          if (this.MyMeter.MyDeviceMemory.BlockLoggerRamData.childMemoryBlocks == null || this.MyMeter.MyDeviceMemory.BlockLoggerRamData.childMemoryBlocks.Count == 0)
            this.MyMeter.MyDeviceMemory.BlockLoggerRamData.BlockStartAddress = (int) this.memDataStartPtr;
          this.myLoggerConfig.isRamLogger = true;
          this.chanalData = (LoggerChanalDataBase) new LoggerMaxBlockChanalData(this.MyMeter, this, this.MyMeter.MyDeviceMemory.BlockLoggerRamData);
        }
        this.chanalData.BlockStartAddress = (int) this.dataStartPtr;
      }
      else
      {
        LoggerManager.LoggerManagerLogger.Info("Create logger chanal without data! (=Event)");
        this.ByteSize = 6;
      }
      return true;
    }

    internal bool AddLoggerChanalParameterFromMemory()
    {
      int memValuePtr = (int) this.memValuePtr;
      int index = this.MyMeter.MyParameters.ParameterByAddress.IndexOfKey(memValuePtr);
      if (index >= 0)
      {
        this.chanalParameter = this.MyMeter.MyParameters.ParameterByAddress.Values[index];
        if (LoggerManager.LoggerManagerLogger.IsInfoEnabled)
          LoggerManager.LoggerManagerLogger.Info("Create logger chanal. Saved parameter: " + this.chanalParameter.Name);
        return true;
      }
      LoggerManager.LoggerManagerLogger.Error("Create logger chanal: Chanal parameter not found");
      return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Chanal parameter not found at address: 0x" + memValuePtr.ToString("x04"), LoggerManager.LoggerManagerLogger);
    }

    internal bool CreateLoggerHeapParameters()
    {
      return !(this.chanalData is LoggerMaxBlockChanalData) || ((LoggerMaxBlockChanalData) this.chanalData).CreateLoggerHeapParameters();
    }

    internal bool CreateLoggerChanalFromBaseMeter(LoggerChanal baseLoggerChanal)
    {
      this.ByteSize = baseLoggerChanal.ByteSize;
      this.chanalFlags = baseLoggerChanal.chanalFlags;
      this.chanalMaxEntries = baseLoggerChanal.chanalMaxEntries;
      if (baseLoggerChanal.chanalParameter != null)
      {
        this.chanalParameter = this.MyMeter.MyParameters.ParameterByName[baseLoggerChanal.chanalParameter.Name];
        if (LoggerManager.LoggerManagerLogger.IsInfoEnabled)
          LoggerManager.LoggerManagerLogger.Info("Create logger chanal from base meter. Parameter: " + this.chanalParameter.Name);
      }
      else
        LoggerManager.LoggerManagerLogger.Info("Create logger chanal from base meter. Parameter: no! -> event");
      if (baseLoggerChanal.chanalData != null)
      {
        this.chanalData = baseLoggerChanal.isMaxValues ? (LoggerChanalDataBase) new LoggerMaxBlockChanalData(this.MyMeter, this, this.MyMeter.MyDeviceMemory.BlockLoggerRamData, (S3_MemoryBlock) baseLoggerChanal) : (!baseLoggerChanal.isRamChanal ? (LoggerChanalDataBase) new LoggerChanalData(this.MyMeter, this, this.MyMeter.MyDeviceMemory.BlockLoggerData) : (LoggerChanalDataBase) new LoggerChanalData(this.MyMeter, this, this.MyMeter.MyDeviceMemory.BlockLoggerRamData));
        this.chanalData.sourceMemoryBlock = S3_MemoryBlock.NoSource;
      }
      return true;
    }

    internal bool InsertLoggerData()
    {
      this.memChanalFlags = this.chanalFlags;
      if (this.ByteSize > 10)
        this.memChanalMaxEntries = this.chanalMaxEntries;
      if (this.chanalData != null)
        this.chanalData.InsertChanalData();
      return true;
    }

    internal bool ResetChanalDataToOneStoredValue()
    {
      return this.chanalData == null || this.chanalData.ResetChanalDataToOneStoredValue();
    }

    internal bool FillChanalTestData(ref ulong testValue)
    {
      return this.chanalData == null || this.chanalData.FillChanalTestData(ref testValue) && this.chanalData.CacheChanalData();
    }

    public bool IsYearLogger => this.myLoggerConfig != null && this.myLoggerConfig.IsYearLogger;

    public bool IsMonthLogger => this.myLoggerConfig != null && this.myLoggerConfig.IsMonthLogger;

    internal void PrintChanal(StringBuilder printText, string startString)
    {
      if (this.isGenerateMidnightEvent)
        printText.AppendLine(startString + "Midnight event");
      else if (this.chanalParameter != null)
      {
        string parameterNameByName = S3_Parameter.GetTranslatedParameterNameByName(this.chanalParameter.Name);
        if (this.isReducedRamChanal)
          printText.Append(startString + "Reduced ram chanal. Entries:" + this.chanalMaxEntries.ToString() + " ; ");
        else
          printText.Append(startString);
        if (this.isMaxValues)
          printText.AppendLine("Max value of parameter: " + parameterNameByName);
        else
          printText.AppendLine("Saved parameter: " + parameterNameByName);
      }
      else
        printText.AppendLine(startString + "test");
    }

    internal enum LoggerChanalFlags
    {
      EntrySizeSelection = 15, // 0x0000000F
      GenerateMidnightEvent = 256, // 0x00000100
      StartRuntimeFunction = 512, // 0x00000200
      MaxValues = 1024, // 0x00000400
      CleareSource = 4096, // 0x00001000
      HasTimeStamp = 8192, // 0x00002000
      ReducedRamChanal = 16384, // 0x00004000
    }
  }
}

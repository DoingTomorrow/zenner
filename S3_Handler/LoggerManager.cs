// Decompiled with JetBrains decompiler
// Type: S3_Handler.LoggerManager
// Assembly: S3_Handler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 8B802F4D-47E2-4B0E-BB63-F326B3D7B0D1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S3_Handler.dll

using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using ZR_ClassLibrary;

#nullable disable
namespace S3_Handler
{
  internal class LoggerManager
  {
    internal static Logger LoggerManagerLogger = LogManager.GetLogger("S3_LoggerManager");
    private S3_HandlerFunctions MyFunctions;
    private S3_Meter MyMeter;
    internal int UnusedRAM = 0;
    internal int UnusedFlash = 0;

    internal LoggerManager(S3_HandlerFunctions MyFunctions, S3_Meter MyMeter)
    {
      this.MyFunctions = MyFunctions;
      this.MyMeter = MyMeter;
    }

    internal bool CreateLoggersFromMemory()
    {
      LoggerManager.LoggerManagerLogger.Info("Create all logger from memory");
      S3_Parameter s3Parameter = this.MyMeter.MyParameters.ParameterByName["Con_FirstLoggerAdr"];
      bool loggersFromMemory = false;
      int shortValue = (int) s3Parameter.GetShortValue();
      while (shortValue > 0)
      {
        loggersFromMemory = new LoggerConfiguration(this.MyMeter, this.MyMeter.MyDeviceMemory.BlockLoggerTable).CreateLoggerFromMemory(ref shortValue);
        if (!loggersFromMemory)
          break;
      }
      this.MyMeter.MyDeviceMemory.BlockFlashBlock3.BlockStartAddress = this.MyMeter.MyDeviceMemory.BlockLoggerData.BlockStartAddress;
      this.MyMeter.MyDeviceMemory.BlockFlashBlock3.StartAddressOfNextBlock = this.MyMeter.MyDeviceMemory.BlockLoggerData.StartAddressOfNextBlock;
      this.MyMeter.MyDeviceMemory.BlockFlashBlock3.parentMemoryBlock.StartAddressOfNextBlock = this.MyMeter.MyDeviceMemory.BlockFlashBlock3.StartAddressOfNextBlock;
      return loggersFromMemory;
    }

    internal bool CrateLoggersFromBaseMeter(S3_Meter baseMeter)
    {
      try
      {
        S3_MemoryBlock blockLoggerTable = this.MyMeter.MyDeviceMemory.BlockLoggerTable;
        DeviceMemory deviceMemory = this.MyMeter.MyDeviceMemory;
        LoggerManager.LoggerManagerLogger.Info("Create loggers from base meter -> remove old logger objects");
        for (int index = blockLoggerTable.childMemoryBlocks.Count - 1; index >= 0; --index)
        {
          if (!this.RemoveLogger((LoggerConfiguration) blockLoggerTable.childMemoryBlocks[index]))
            return false;
        }
        bool flag = false;
        foreach (LoggerConfiguration childMemoryBlock1 in baseMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks)
        {
          LoggerConfiguration loggerConfiguration = new LoggerConfiguration(this.MyMeter, blockLoggerTable);
          flag = loggerConfiguration.CrateLoggerFromBaseMeter(childMemoryBlock1);
          if (flag)
          {
            if (blockLoggerTable.sourceMemoryBlock != null)
            {
              foreach (LoggerConfiguration childMemoryBlock2 in blockLoggerTable.sourceMemoryBlock.childMemoryBlocks)
              {
                if ((int) loggerConfiguration.intervallSeconds == (int) childMemoryBlock2.intervallSeconds && (int) loggerConfiguration.maxEntries == (int) childMemoryBlock2.maxEntries && (int) loggerConfiguration.flags == (int) childMemoryBlock2.flags)
                {
                  loggerConfiguration.sourceMemoryBlock = (S3_MemoryBlock) childMemoryBlock2;
                  loggerConfiguration.startTimeSeconds = childMemoryBlock2.startTimeSeconds;
                  loggerConfiguration.intervallSeconds = childMemoryBlock2.intervallSeconds;
                  loggerConfiguration.flags = childMemoryBlock2.flags;
                  loggerConfiguration.maxEntries = childMemoryBlock2.maxEntries;
                  if (LoggerManager.LoggerManagerLogger.IsInfoEnabled)
                  {
                    DateTime dateTime = ZR_Calendar.Cal_GetDateTime(loggerConfiguration.startTimeSeconds);
                    string intervalInfo = ZR_Calendar.Cal_GetIntervalInfo(loggerConfiguration.intervallSeconds);
                    LoggerManager.LoggerManagerLogger.Info("Set logger data references! Start time: " + dateTime.ToString("dd.MM.yyyy HH:mm") + "; Intervall: " + intervalInfo);
                  }
                  if (childMemoryBlock2.sourceMemoryBlock != null)
                  {
                    LoggerManager.LoggerManagerLogger.Trace("Logger source memory block available.");
                    using (List<S3_MemoryBlock>.Enumerator enumerator = childMemoryBlock2.sourceMemoryBlock.childMemoryBlocks.GetEnumerator())
                    {
                      while (enumerator.MoveNext())
                      {
                        LoggerChanal current = (LoggerChanal) enumerator.Current;
                        LoggerManager.LoggerManagerLogger.Trace("Search source chanal data for chanal: " + current.ChanalName);
                        foreach (LoggerChanal childMemoryBlock3 in loggerConfiguration.childMemoryBlocks)
                        {
                          if (childMemoryBlock3.ChanalName == current.ChanalName && (int) childMemoryBlock3.chanalMaxEntries == (int) current.chanalMaxEntries)
                          {
                            LoggerManager.LoggerManagerLogger.Trace("Source found: Add data reference for insert chanal configuration");
                            childMemoryBlock3.sourceMemoryBlock = (S3_MemoryBlock) current;
                            if (childMemoryBlock3.chanalData != null && current.chanalData != null && childMemoryBlock3.chanalData.ByteSize == current.chanalData.ByteSize)
                            {
                              LoggerManager.LoggerManagerLogger.Trace("Add source chanal data reference for insert chanal data. Source address = 0x" + current.chanalData.BlockStartAddress.ToString("x04"));
                              childMemoryBlock3.chanalData.sourceMemoryBlock = (S3_MemoryBlock) current.chanalData;
                              if (current.chanalData.childMemoryBlocks != null)
                              {
                                LoggerManager.LoggerManagerLogger.Trace("MaxValueGenerator: Add source parameter references for insert parameter data");
                                for (int index = 0; index < current.chanalData.childMemoryBlocks.Count; ++index)
                                  childMemoryBlock3.chanalData.childMemoryBlocks[index].sourceMemoryBlock = current.chanalData.childMemoryBlocks[index];
                                break;
                              }
                              break;
                            }
                            LoggerManager.LoggerManagerLogger.Trace("Source chanal data not available");
                            break;
                          }
                        }
                      }
                      break;
                    }
                  }
                  else
                  {
                    LoggerManager.LoggerManagerLogger.Info("Source memory block not available");
                    break;
                  }
                }
              }
            }
          }
          else
            break;
        }
        this.MyMeter.MyDeviceMemory.BlockFlashBlock3.BlockStartAddress = blockLoggerTable.BlockStartAddress;
        this.MyMeter.MyDeviceMemory.BlockFlashBlock3.StartAddressOfNextBlock = blockLoggerTable.StartAddressOfNextBlock;
        return flag;
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.Exception, ex.ToString());
        return false;
      }
    }

    internal bool RemoveLogger(LoggerConfiguration theLogger)
    {
      LoggerManager.LoggerManagerLogger.Info("Remove logger");
      for (int index = theLogger.childMemoryBlocks.Count - 1; index >= 0; --index)
      {
        LoggerChanal childMemoryBlock = (LoggerChanal) theLogger.childMemoryBlocks[index];
        if (!theLogger.RemoveLoggerChanal(childMemoryBlock))
          return false;
      }
      this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks.Remove((S3_MemoryBlock) theLogger);
      return true;
    }

    internal LoggerChanal GetLoggerChanal(int chanalAddress)
    {
      foreach (S3_MemoryBlock childMemoryBlock1 in this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks)
      {
        foreach (LoggerChanal childMemoryBlock2 in childMemoryBlock1.childMemoryBlocks)
        {
          if (childMemoryBlock2.BlockStartAddress == chanalAddress)
            return childMemoryBlock2;
        }
      }
      return (LoggerChanal) null;
    }

    internal LoggerChanal GetLoggerChanal(string chanalName)
    {
      foreach (S3_MemoryBlock childMemoryBlock1 in this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks)
      {
        foreach (LoggerChanal childMemoryBlock2 in childMemoryBlock1.childMemoryBlocks)
        {
          if (childMemoryBlock2.ChanalName == chanalName)
            return childMemoryBlock2;
        }
      }
      return (LoggerChanal) null;
    }

    internal void GetDataBlockInsertInfo(
      LoggerChanal theChanal,
      out S3_MemoryBlock insertBlock,
      out int insertIndex)
    {
      insertIndex = 0;
      insertBlock = !theChanal.isRamChanal ? this.MyMeter.MyDeviceMemory.BlockLoggerData : this.MyMeter.MyDeviceMemory.BlockLoggerRamData;
      foreach (LoggerConfiguration childMemoryBlock1 in this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks)
      {
        if (childMemoryBlock1.childMemoryBlocks == null)
          break;
        foreach (LoggerChanal childMemoryBlock2 in childMemoryBlock1.childMemoryBlocks)
        {
          if (theChanal == childMemoryBlock2)
            return;
          if (childMemoryBlock2.isRamChanal == theChanal.isRamChanal && childMemoryBlock2.hasChanalData)
            ++insertIndex;
        }
      }
    }

    internal bool CreateStructureObjects()
    {
      if (this.MyMeter.MyDeviceMemory.BlockLoggerData.childMemoryBlocks != null)
      {
        foreach (S3_MemoryBlock childMemoryBlock in this.MyMeter.MyDeviceMemory.BlockLoggerData.childMemoryBlocks)
        {
          if (childMemoryBlock is LoggerChanalData)
            ((LoggerChanalDataBase) childMemoryBlock).CacheChanalData();
        }
      }
      if (this.MyMeter.MyDeviceMemory.BlockLoggerRamData.childMemoryBlocks != null)
      {
        foreach (S3_MemoryBlock childMemoryBlock in this.MyMeter.MyDeviceMemory.BlockLoggerRamData.childMemoryBlocks)
        {
          if (childMemoryBlock is LoggerChanalData)
            ((LoggerChanalDataBase) childMemoryBlock).CacheChanalData();
          else if (childMemoryBlock is LoggerMaxBlockChanalData)
            ((LoggerChanalDataBase) childMemoryBlock).CacheChanalData();
        }
      }
      return true;
    }

    internal LoggerManager Clone(S3_Meter theCloneMeter)
    {
      LoggerManager loggerManager = new LoggerManager(this.MyFunctions, theCloneMeter);
      if (this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks != null)
      {
        foreach (LoggerConfiguration childMemoryBlock in this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks)
          childMemoryBlock.Clone(theCloneMeter);
      }
      loggerManager.UnusedRAM = this.UnusedRAM;
      loggerManager.UnusedFlash = this.UnusedFlash;
      return loggerManager;
    }

    internal LoggerConfiguration InsertNewLogger(
      LoggerConfiguration loggerConfigurationAfterNewLogger)
    {
      LoggerConfiguration loggerConfiguration = loggerConfigurationAfterNewLogger != null ? new LoggerConfiguration(this.MyMeter, this.MyMeter.MyDeviceMemory.BlockLoggerTable, this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks.IndexOf((S3_MemoryBlock) loggerConfigurationAfterNewLogger)) : new LoggerConfiguration(this.MyMeter, this.MyMeter.MyDeviceMemory.BlockLoggerTable);
      loggerConfiguration.firstChildMemoryBlockOffset = 16;
      loggerConfiguration.ByteSize = 16;
      loggerConfiguration.intervallSeconds = 2629800U;
      loggerConfiguration.maxEntries = (ushort) 2;
      loggerConfiguration.ResetStartTime(DateTime.Now);
      return loggerConfiguration;
    }

    internal bool SetChanalToMaximalValueLogger(LoggerChanal theChanal)
    {
      int ramInsertIndex = 0;
      foreach (S3_MemoryBlock childMemoryBlock1 in this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks)
      {
        foreach (LoggerChanal childMemoryBlock2 in childMemoryBlock1.childMemoryBlocks)
        {
          if (childMemoryBlock2 != theChanal)
          {
            if (childMemoryBlock2.chanalData != null && childMemoryBlock2.isRamChanal)
              ++ramInsertIndex;
          }
          else
            goto label_12;
        }
      }
label_12:
      return theChanal.myLoggerConfig.SetChanalToMaximalValueLogger(theChanal, ramInsertIndex) && this.MyMeter.MyLinker.Link(this.MyMeter.MyDeviceMemory.meterMemory) && this.MyMeter.MyLinker.ResetLabels() && this.MyMeter.MyParameters.RecreateParameterByAddress();
    }

    internal bool ResetAndClearAllLoggers()
    {
      try
      {
        foreach (LoggerConfiguration childMemoryBlock1 in this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks)
        {
          childMemoryBlock1.sourceMemoryBlock = (S3_MemoryBlock) null;
          if (!childMemoryBlock1.ResetStartTime(ZR_Calendar.Cal_GetDateTime(this.MyMeter.MeterTimeAsSeconds1980)))
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "loggerConfig.ResetStartTime");
            return false;
          }
          foreach (LoggerChanal childMemoryBlock2 in childMemoryBlock1.childMemoryBlocks)
          {
            childMemoryBlock2.sourceMemoryBlock = (S3_MemoryBlock) null;
            if (!childMemoryBlock2.ResetChanalDataToOneStoredValue())
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "loggerChanal.ResetChanalData");
              return false;
            }
          }
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription("Exception: " + ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, nameof (ResetAndClearAllLoggers));
        return false;
      }
      return true;
    }

    internal bool FillTestValues()
    {
      ulong testValue = 1000;
      foreach (S3_MemoryBlock childMemoryBlock1 in this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks)
      {
        foreach (LoggerChanal childMemoryBlock2 in childMemoryBlock1.childMemoryBlocks)
        {
          if (!childMemoryBlock2.FillChanalTestData(ref testValue))
            return false;
        }
      }
      return true;
    }

    internal string GetLoggerName(ushort address)
    {
      List<S3_MemoryBlock> childMemoryBlocks = this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks;
      if (childMemoryBlocks == null || childMemoryBlocks.Count == 0)
        return (string) null;
      foreach (S3_MemoryBlock s3MemoryBlock in childMemoryBlocks)
      {
        foreach (LoggerChanal childMemoryBlock in s3MemoryBlock.childMemoryBlocks)
        {
          if ((int) (ushort) childMemoryBlock.BlockStartAddress == (int) address)
            return childMemoryBlock.ChanalName;
        }
      }
      return (string) null;
    }

    internal Dictionary<ushort, string> GetLoggerChanalsByAddresses()
    {
      Dictionary<ushort, string> chanalsByAddresses = new Dictionary<ushort, string>();
      List<S3_MemoryBlock> childMemoryBlocks = this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks;
      if (childMemoryBlocks == null || childMemoryBlocks.Count == 0)
        return chanalsByAddresses;
      foreach (S3_MemoryBlock s3MemoryBlock in childMemoryBlocks)
      {
        foreach (LoggerChanal childMemoryBlock in s3MemoryBlock.childMemoryBlocks)
        {
          if (chanalsByAddresses.ContainsKey((ushort) childMemoryBlock.BlockStartAddress))
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, childMemoryBlock.ChanalName + " -> Logger chanals at same address: 0x" + childMemoryBlock.BlockStartAddress.ToString("x04"));
          else
            chanalsByAddresses.Add((ushort) childMemoryBlock.BlockStartAddress, childMemoryBlock.ChanalName);
        }
      }
      return chanalsByAddresses;
    }

    internal SortedList<string, int> GetLoggerChanalsByNames()
    {
      SortedList<string, int> loggerChanalsByNames = new SortedList<string, int>();
      List<S3_MemoryBlock> childMemoryBlocks = this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks;
      if (childMemoryBlocks == null || childMemoryBlocks.Count == 0)
        return loggerChanalsByNames;
      foreach (S3_MemoryBlock s3MemoryBlock in childMemoryBlocks)
      {
        foreach (LoggerChanal childMemoryBlock in s3MemoryBlock.childMemoryBlocks)
        {
          string chanalName = childMemoryBlock.ChanalName;
          if (loggerChanalsByNames.ContainsKey(chanalName))
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, chanalName + " -> Logger chanals that use same name: " + chanalName);
          else
            loggerChanalsByNames.Add(chanalName, childMemoryBlock.BlockStartAddress);
        }
      }
      return loggerChanalsByNames;
    }

    internal bool RemoveByResources()
    {
      for (int index = this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks.Count - 1; index >= 0; --index)
      {
        LoggerConfiguration childMemoryBlock = (LoggerConfiguration) this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks[index];
        if (!childMemoryBlock.RemoveLoggerChanalsByResources())
          return false;
        if (childMemoryBlock.childMemoryBlocks == null || childMemoryBlock.childMemoryBlocks.Count == 0)
        {
          this.MyMeter.MyResources.ResourceEventDeleteObjectByMissedResource("Delete logger (no chanals)", "");
          this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks.RemoveAt(index);
        }
      }
      return true;
    }

    internal bool InsertData()
    {
      if (this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks == null)
        return true;
      foreach (LoggerConfiguration childMemoryBlock in this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks)
      {
        if (!childMemoryBlock.InsertLoggerData())
          return false;
      }
      this.UnusedRAM = 0;
      int addressLable1 = this.MyFunctions.MyMeters.WorkMeter.MyParameters.AddressLables["CSTACK"];
      if (this.MyMeter.MyDeviceMemory.BlockConfiguratorHeap.childMemoryBlocks != null && this.MyMeter.MyDeviceMemory.BlockConfiguratorHeap.childMemoryBlocks.Count > 0)
      {
        int index = this.MyMeter.MyDeviceMemory.BlockConfiguratorHeap.childMemoryBlocks.Count - 1;
        this.UnusedRAM = addressLable1 - this.MyMeter.MyDeviceMemory.BlockConfiguratorHeap.childMemoryBlocks[index].StartAddressOfNextBlock;
      }
      else
        this.UnusedRAM = addressLable1 - this.MyMeter.MyDeviceMemory.BlockConfiguratorHeap.BlockStartAddress;
      this.UnusedFlash = 0;
      int addressLable2 = this.MyFunctions.MyMeters.WorkMeter.MyParameters.AddressLables["CODE_CONSTANTS"];
      if (this.MyMeter.MyDeviceMemory.BlockLoggerData.childMemoryBlocks != null && this.MyMeter.MyDeviceMemory.BlockLoggerData.childMemoryBlocks.Count > 0)
      {
        int index = this.MyMeter.MyDeviceMemory.BlockLoggerData.childMemoryBlocks.Count - 1;
        this.UnusedFlash = addressLable2 - this.MyMeter.MyDeviceMemory.BlockLoggerData.childMemoryBlocks[index].StartAddressOfNextBlock;
      }
      else
        this.UnusedFlash = addressLable2 - this.MyMeter.MyDeviceMemory.BlockLoggerData.BlockStartAddress;
      return true;
    }

    internal bool LinkPointer()
    {
      if (this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks == null)
        return true;
      try
      {
        S3_Parameter s3Parameter = this.MyMeter.MyParameters.ParameterByName["Con_FirstLoggerAdr"];
        s3Parameter.SetUshortValue((ushort) 0);
        LoggerConfiguration loggerConfiguration = (LoggerConfiguration) null;
        foreach (LoggerConfiguration childMemoryBlock in this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks)
        {
          if (loggerConfiguration != null)
            loggerConfiguration.memNextLoggerPtr = (ushort) childMemoryBlock.BlockStartAddress;
          else
            s3Parameter.SetUshortValue((ushort) childMemoryBlock.BlockStartAddress);
          childMemoryBlock.memNextLoggerPtr = (ushort) 0;
          if (!childMemoryBlock.LinkLoggerChanals())
            return false;
          loggerConfiguration = childMemoryBlock;
        }
      }
      catch (Exception ex)
      {
        ZR_ClassLibMessages.AddErrorDescription("Exception: " + ex.ToString());
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "LoggerManager: LinkPointer");
        return false;
      }
      return true;
    }

    internal void PrintAllLoggers(StringBuilder printText)
    {
      foreach (LoggerConfiguration childMemoryBlock in this.MyMeter.MyDeviceMemory.BlockLoggerTable.childMemoryBlocks)
        childMemoryBlock.PrintLogger(printText);
    }
  }
}

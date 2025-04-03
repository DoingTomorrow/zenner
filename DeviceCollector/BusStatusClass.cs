// Decompiled with JetBrains decompiler
// Type: DeviceCollector.BusStatusClass
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

#nullable disable
namespace DeviceCollector
{
  public class BusStatusClass
  {
    internal BusStatusClass.GlobalFunctionTasks GlobalFunctionTask;
    internal BusStatusClass.BusFunctionTasks BusFunctionTask;
    internal int RepeadCounter;
    internal int TransmitBlockCounter;
    internal int ReceiveBlockCounter;
    internal int ByteCounter;
    internal int TotalJobCounter;
    internal int TotalErrorCounter;
    internal int TotalTransmitBlockCounter;
    internal int TotalReceiveBlockCounter;
    private DeviceCollectorFunctions MyFunctions;

    public BusStatusClass(DeviceCollectorFunctions TheFunctions) => this.MyFunctions = TheFunctions;

    internal void StartGlobalFunctionTask(
      BusStatusClass.GlobalFunctionTasks NewGlobalFunctionTask)
    {
      this.GlobalFunctionTask = NewGlobalFunctionTask;
      this.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Off);
      this.TotalErrorCounter = 0;
      this.TotalJobCounter = 0;
      this.TotalTransmitBlockCounter = 0;
      this.TotalReceiveBlockCounter = 0;
    }

    internal void StartBusFunctionTask(BusStatusClass.BusFunctionTasks NewBusFunctionTask)
    {
      ++this.TotalJobCounter;
      this.BusFunctionTask = NewBusFunctionTask;
      this.RepeadCounter = 0;
      this.TransmitBlockCounter = 0;
      this.ReceiveBlockCounter = 0;
      this.ByteCounter = 0;
    }

    internal void IncrementTransmitBlockCounter()
    {
      ++this.TransmitBlockCounter;
      ++this.TotalTransmitBlockCounter;
    }

    internal void IncrementReceiveBlockCounter()
    {
      ++this.ReceiveBlockCounter;
      ++this.TotalReceiveBlockCounter;
    }

    internal bool TestRepeat1() => this.RepeadCounter <= 1;

    internal bool TestRepeatCounter(int MaxRepeatNumber)
    {
      if (this.MyFunctions.BreakRequest || this.RepeadCounter >= MaxRepeatNumber)
        return false;
      if (this.RepeadCounter > 0)
        ++this.TotalErrorCounter;
      ++this.RepeadCounter;
      return true;
    }

    internal enum GlobalFunctionTasks
    {
      Off,
      ReadParameter,
      ScanPrimary,
      ScanSecundary,
      TestloopReadEEProm,
      TestloopWriteReadEEProm,
    }

    internal enum BusFunctionTasks
    {
      Off,
      SEND_NKE,
      REQ_UD2,
      ApplicationReset,
      SelectAllParameter,
      SetBaudrate,
      SelectDevice,
      SetPrimaryAddress,
      ResetDevice,
      ResetDeviceBaudChange,
      RunBackup,
      SetEmergencyMode,
      DeleteMeterKey,
      ReadMemoryBlock,
      WriteMemoryBlock,
      WriteBitField,
      DigitalInputsAndOutputs,
      ReadVersion,
      SetNewPin,
      TransmitRadioFrame,
      WriteBit,
      WriteNibble,
      WriteByte,
      SelectParameterList,
      Serie3Command,
      MBusConverterCommand,
      PulseDisable,
      PulseEnable,
      RadioDisable,
      RadioNormal,
      StartVolumeMonitor,
      StopVolumeMonitor,
      RadioOOK,
      RadioPN9,
      SynchronizeAction,
      WritePulseoutQueue,
      StartDepassivation,
      RadioReceive,
      UpdateMode,
      WriteDueDateMonth,
      EventLogClear,
      SystemLogClear,
      RemovalFlagClear,
      TamperFlagClear,
      BackflowFlagClear,
      LeakFlagClear,
      BlockFlagClear,
      OversizeFlagClear,
      UndersizeFlagClear,
      BurstFlagClear,
      LogClearAndDisableLog,
      ReadMeterValue,
      WriteMeterValue,
      LogEnable,
      LogDisable,
      Dummy,
    }
  }
}

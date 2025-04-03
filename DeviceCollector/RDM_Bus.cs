// Decompiled with JetBrains decompiler
// Type: DeviceCollector.RDM_Bus
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class RDM_Bus : MBusDevice
  {
    private const int MaxReadBlockSize = 32;
    private const int MaxWriteBlockSize = 8;

    public RDM_Bus(DeviceCollectorFunctions TheBus)
      : base(TheBus)
    {
      this.DeviceType = DeviceTypes.ZR_RDM;
    }

    public RDM_Bus(MBusDevice TheMBusDevice)
      : base(TheMBusDevice.MyBus)
    {
      this.Info = TheMBusDevice.Info;
      this.DeviceType = DeviceTypes.ZR_RDM;
    }

    internal bool ReadVersion(ref long Version)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.ReadVersion);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateLongframeStart();
      this.TransmitBuffer.Add(6);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusSendREQ_Version);
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (this.ReceiveLongframeStart() && this.ReceiveLongframeEnd())
        {
          Version = ((long) this.ReceiveBuffer.Data[0] << 16) + ((long) this.ReceiveBuffer.Data[1] << 24);
          return true;
        }
      }
      return false;
    }

    internal bool DeleteMeterKey(int MeterKey)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.DeleteMeterKey);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateLongframeStart();
      this.TransmitBuffer.Add(7);
      this.TransmitBuffer.Add((byte) (MeterKey & (int) byte.MaxValue));
      this.TransmitBuffer.Add((byte) (MeterKey >> 8 & (int) byte.MaxValue));
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (this.ReceiveOkNok())
          return true;
      }
      return false;
    }

    internal bool SetNewPin(int Pin)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.SetNewPin);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateLongframeStart();
      this.TransmitBuffer.Add(8);
      this.TransmitBuffer.Add((byte) (Pin & (int) byte.MaxValue));
      this.TransmitBuffer.Add((byte) (Pin >> 8 & (int) byte.MaxValue));
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (this.ReceiveOkNok())
          return true;
      }
      return false;
    }

    internal bool TransmitRadioFrame()
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.TransmitRadioFrame);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateLongframeStart();
      this.TransmitBuffer.Add(5);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (this.ReceiveOkNok())
          return true;
      }
      return false;
    }

    internal bool ReadMemory(int StartAddress, int NumberOfBytes, out ByteField OutData)
    {
      int num = StartAddress;
      this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusStartReadMemory);
      OutData = new ByteField(NumberOfBytes);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.MyBus.BreakRequest = false;
      do
      {
        int BlockSize = NumberOfBytes - (num - StartAddress);
        if (BlockSize > 32)
          BlockSize = 32;
        this.MyBus.SendMessage(num, GMM_EventArgs.MessageType.PrimaryAddressMessage);
        if (this.MyBus.BreakRequest)
        {
          this.MyBus.SendMessage(num, GMM_EventArgs.MessageType.EndMessage);
          return false;
        }
        if (!this.ReadMemoryBlock(num, BlockSize))
        {
          this.MyBus.SendMessage(num, GMM_EventArgs.MessageType.EndMessage);
          return false;
        }
        for (int index = 0; index < this.ReceiveBuffer.Count - 2; ++index)
          OutData.Add(this.ReceiveBuffer.Data[index]);
        num += BlockSize;
      }
      while (num - StartAddress < NumberOfBytes);
      this.MyBus.SendMessage(num, GMM_EventArgs.MessageType.EndMessage);
      return true;
    }

    private bool ReadMemoryBlock(int BlockStartAddress, int BlockSize)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.ReadMemoryBlock);
      this.GenerateLongframeStart();
      this.TransmitBuffer.Add(1);
      this.TransmitBuffer.Add((byte) (BlockStartAddress & (int) byte.MaxValue));
      this.TransmitBuffer.Add((byte) (BlockStartAddress >> 8 & (int) byte.MaxValue));
      this.TransmitBuffer.Add((byte) BlockSize);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusStartReadBlock);
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (this.ReceiveLongframeStart() && this.ReceiveLongframeEnd())
        {
          if (this.ReceiveBuffer.Count - 2 == BlockSize)
            return true;
          this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusReadWrongBlockLength);
        }
      }
      return false;
    }

    internal bool WriteMemory(int StartAddress, ref ByteField WriteData)
    {
      if (!this.MyBus.MyCom.Open())
        return false;
      int num = StartAddress;
      int DataStartOffset = 0;
      this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusStartWriteMemory);
      this.MyBus.BreakRequest = false;
      do
      {
        int BlockSize = WriteData.Count - (num - StartAddress);
        if (BlockSize > 8)
          BlockSize = 8;
        this.MyBus.SendMessage(num, GMM_EventArgs.MessageType.PrimaryAddressMessage);
        if (this.MyBus.BreakRequest)
        {
          this.MyBus.SendMessage(num, GMM_EventArgs.MessageType.EndMessage);
          return false;
        }
        if (!this.WriteMemoryBlock(num, BlockSize, ref DataStartOffset, ref WriteData))
        {
          this.MyBus.SendMessage(num, GMM_EventArgs.MessageType.EndMessage);
          return false;
        }
        num += BlockSize;
      }
      while (num - StartAddress < WriteData.Count);
      this.MyBus.SendMessage(num, GMM_EventArgs.MessageType.EndMessage);
      return true;
    }

    private bool WriteMemoryBlock(
      int BlockStartAddress,
      int BlockSize,
      ref int DataStartOffset,
      ref ByteField WriteData)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.WriteMemoryBlock);
      this.GenerateLongframeStart();
      this.TransmitBuffer.Add(2);
      this.TransmitBuffer.Add((byte) (BlockStartAddress & (int) byte.MaxValue));
      this.TransmitBuffer.Add((byte) (BlockStartAddress >> 8 & (int) byte.MaxValue));
      this.TransmitBuffer.Add((byte) BlockSize);
      while (BlockSize-- > 0)
        this.TransmitBuffer.Add(WriteData.Data[DataStartOffset++]);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusStartWriteBlock);
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (this.ReceiveOkNok())
          return true;
      }
      return false;
    }
  }
}

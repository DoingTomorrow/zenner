// Decompiled with JetBrains decompiler
// Type: DeviceCollector.EHCA_MBus
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class EHCA_MBus : MBusDevice
  {
    private const int MaxReadBlockSize = 200;
    private const int MaxWriteBlockSize = 8;
    internal int StartAddress;
    internal int NumberOfBytes;
    internal ByteField DataBuffer;

    public EHCA_MBus(DeviceCollectorFunctions TheBus)
      : base(TheBus)
    {
      this.DeviceType = DeviceTypes.ZR_EHCA;
    }

    public EHCA_MBus(MBusDevice TheMBusDevice)
      : base(TheMBusDevice.MyBus)
    {
      this.Info = TheMBusDevice.Info;
      this.PrimaryAddressKnown = TheMBusDevice.PrimaryAddressKnown;
      this.PrimaryAddressOk = TheMBusDevice.PrimaryAddressOk;
      this.PrimaryDeviceAddress = TheMBusDevice.PrimaryDeviceAddress;
      this.DeviceType = DeviceTypes.ZR_EHCA;
    }

    internal bool ReadMemory()
    {
      if (!this.MyBus.MyCom.Open())
        return false;
      int startAddress = this.StartAddress;
      this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusStartReadMemory);
      this.DataBuffer = new ByteField(this.NumberOfBytes);
      do
      {
        int BlockSize = this.NumberOfBytes - (startAddress - this.StartAddress);
        if (BlockSize > 200)
          BlockSize = 200;
        if (!this.ReadMemoryBlock(startAddress, BlockSize))
          return false;
        for (int index = 1; index < this.ReceiveBuffer.Count - 2; ++index)
          this.DataBuffer.Add(this.ReceiveBuffer.Data[index]);
        startAddress += BlockSize;
      }
      while (startAddress - this.StartAddress < this.NumberOfBytes);
      return true;
    }

    private bool ReadMemoryBlock(int BlockStartAddress, int BlockSize)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.ReadMemoryBlock);
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add((byte) (BlockStartAddress & (int) byte.MaxValue));
      this.TransmitBuffer.Add((byte) (BlockStartAddress >> 8 & (int) byte.MaxValue));
      this.TransmitBuffer.Add((int) (byte) BlockSize - 1);
      this.TransmitBuffer.Add(2);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusStartReadBlock);
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (this.ReceiveHeader() && this.ReceiveLongframeEnd())
        {
          if (this.ReceiveBuffer.Count - 3 == BlockSize)
            return true;
          this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusReadWrongBlockLength);
        }
      }
      return false;
    }

    internal bool WriteMemory()
    {
      if (!this.MyBus.MyCom.Open())
        return false;
      int startAddress = this.StartAddress;
      int DataStartOffset = 0;
      this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusStartWriteMemory);
      do
      {
        int BlockSize = this.DataBuffer.Count - (startAddress - this.StartAddress);
        if (BlockSize > 8)
          BlockSize = 8;
        if (!this.WriteMemoryBlock(startAddress, BlockSize, ref DataStartOffset))
          return false;
        startAddress += BlockSize;
      }
      while (startAddress - this.StartAddress < this.DataBuffer.Count);
      return true;
    }

    private bool WriteMemoryBlock(int BlockStartAddress, int BlockSize, ref int DataStartOffset)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.WriteMemoryBlock);
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add((byte) (BlockStartAddress & (int) byte.MaxValue));
      this.TransmitBuffer.Add((byte) (BlockStartAddress >> 8 & (int) byte.MaxValue));
      this.TransmitBuffer.Add((byte) BlockSize);
      this.TransmitBuffer.Add(3);
      while (BlockSize-- > 0)
        this.TransmitBuffer.Add(this.DataBuffer.Data[DataStartOffset++]);
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

    internal bool WriteBitfield(uint AndMask, uint OrMask)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.WriteBitField);
      int num = 4;
      for (uint index = 4278190080; index > 0U && ((int) AndMask & (int) index) == (int) index && (OrMask & index) <= 0U; index >>= 8)
        --num;
      if (num == 0)
        return true;
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add((byte) (this.StartAddress & (int) byte.MaxValue));
      this.TransmitBuffer.Add((byte) (this.StartAddress >> 8 & (int) byte.MaxValue));
      this.TransmitBuffer.Add((byte) (num * 2));
      this.TransmitBuffer.Add(13);
      for (byte index = 0; (int) index < num; ++index)
      {
        this.TransmitBuffer.Add((byte) (AndMask >> (int) index * 8 & (uint) byte.MaxValue));
        this.TransmitBuffer.Add((byte) (OrMask >> (int) index * 8 & (uint) byte.MaxValue));
      }
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusStartWriteBit);
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (this.ReceiveOkNok())
          return true;
      }
      return false;
    }
  }
}

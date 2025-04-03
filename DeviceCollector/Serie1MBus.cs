// Decompiled with JetBrains decompiler
// Type: DeviceCollector.Serie1MBus
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class Serie1MBus : MBusDevice
  {
    private const long S1B_F_O_OutPu1 = 32509943;
    private const long S1B_F_O_OutPu2 = 15732727;

    public Serie1MBus(DeviceCollectorFunctions TheBus)
      : base(TheBus)
    {
      this.DeviceType = DeviceTypes.ZR_Serie1;
    }

    public Serie1MBus(MBusDevice TheMBusDevice)
      : base(TheMBusDevice.MyBus)
    {
      this.Info = TheMBusDevice.Info;
      this.PrimaryAddressKnown = TheMBusDevice.PrimaryAddressKnown;
      this.PrimaryAddressOk = TheMBusDevice.PrimaryAddressOk;
      this.PrimaryDeviceAddress = TheMBusDevice.PrimaryDeviceAddress;
      this.DeviceType = DeviceTypes.ZR_Serie1;
    }

    internal override bool SelectParameterList(int ListNumber, int function) => false;

    public void ReadVersion()
    {
    }

    internal bool WriteBit(long Address, bool SetBitTo1)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.WriteBit);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(6);
      this.TransmitBuffer.Add((int) sbyte.MaxValue);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(5);
      this.TransmitBuffer.Add((byte) ((ulong) Address & (ulong) byte.MaxValue));
      this.TransmitBuffer.Add((byte) ((ulong) (Address >> 8) & (ulong) byte.MaxValue));
      this.TransmitBuffer.Add((byte) ((ulong) (Address >> 16) & (ulong) byte.MaxValue));
      byte Byte = (byte) ((ulong) (Address >> 24) & 15UL);
      if (SetBitTo1)
        Byte |= (byte) 16;
      this.TransmitBuffer.Add(Byte);
      this.WorkTransparentChecksum();
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

    internal bool WriteNibble(long Address, byte NibbleData)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.WriteNibble);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(6);
      this.TransmitBuffer.Add((int) sbyte.MaxValue);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(6);
      this.TransmitBuffer.Add((byte) ((ulong) Address & (ulong) byte.MaxValue));
      this.TransmitBuffer.Add((byte) ((ulong) (Address >> 8) & (ulong) byte.MaxValue));
      this.TransmitBuffer.Add((byte) ((ulong) (Address >> 16) & (ulong) byte.MaxValue));
      this.TransmitBuffer.Add((byte) ((uint) (byte) ((ulong) (Address >> 24) & 15UL) | (uint) (byte) (((int) NibbleData & 15) << 4)));
      this.WorkTransparentChecksum();
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

    internal bool WriteByte(long Address, byte ByteData)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.WriteByte);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(6);
      this.TransmitBuffer.Add((int) sbyte.MaxValue);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(7);
      this.TransmitBuffer.Add((byte) ((ulong) Address & (ulong) byte.MaxValue));
      this.TransmitBuffer.Add((byte) ((ulong) (Address >> 8) & (ulong) byte.MaxValue));
      this.TransmitBuffer.Add((byte) ((ulong) (Address >> 16) & (ulong) byte.MaxValue));
      this.TransmitBuffer.Add(ByteData);
      this.WorkTransparentChecksum();
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

    private void WorkTransparentChecksum()
    {
      byte num = 0;
      for (int index = 10; index <= 14; ++index)
        num = (byte) ((uint) (byte) ((uint) num + (uint) (byte) ((uint) this.TransmitBuffer.Data[index] & 15U)) + (uint) (byte) ((int) this.TransmitBuffer.Data[index] >> 4 & 15));
      this.TransmitBuffer.Data[10] |= (byte) (((int) num & 15) << 4);
    }

    internal bool ReadMemory(long FullStartAddress, int NumberOfBytes, ref ByteField MemoryData)
    {
      return this.ReadMemoryBlock(FullStartAddress, NumberOfBytes, ref MemoryData);
    }

    private bool ReadMemoryBlock(long StartAddress, int NumberOfBytes, ref ByteField MemoryData)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.WriteMemoryBlock);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(6);
      this.TransmitBuffer.Add((int) sbyte.MaxValue);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(4);
      this.TransmitBuffer.Add((byte) ((ulong) StartAddress & (ulong) byte.MaxValue));
      this.TransmitBuffer.Add((byte) ((ulong) (StartAddress >> 8) & (ulong) byte.MaxValue));
      this.TransmitBuffer.Add((byte) ((ulong) (StartAddress >> 16) & (ulong) byte.MaxValue));
      this.TransmitBuffer.Add(NumberOfBytes);
      this.WorkTransparentChecksum();
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        this.MyBus.SendMessage((int) (StartAddress & (long) ushort.MaxValue), GMM_EventArgs.MessageType.PrimaryAddressMessage);
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusStartWriteBlock);
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (this.ReceiveOkNok())
        {
          this.GenerateREQ_UD2();
          this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
          this.MyBus.BusState.IncrementTransmitBlockCounter();
          if (this.ReceiveHeader() && this.ReceiveLongframeEnd())
          {
            this.MyBus.SendMessage((int) (StartAddress & (long) ((int) ushort.MaxValue + NumberOfBytes)), GMM_EventArgs.MessageType.PrimaryAddressMessage);
            if (this.ReceiveBuffer.Data[0] == (byte) 15 && this.ReceiveBuffer.Count == NumberOfBytes + 5)
            {
              for (int index = 1; index <= NumberOfBytes; ++index)
                MemoryData.Add(this.ReceiveBuffer.Data[index]);
              this.MyBus.SendMessage(0, GMM_EventArgs.MessageType.EndMessage);
              return true;
            }
          }
        }
      }
      this.MyBus.SendMessage(0, GMM_EventArgs.MessageType.EndMessage);
      return false;
    }

    internal bool SetOutput(int Port, bool State)
    {
      bool SetBitTo1 = !State;
      if (Port == 1)
        return this.WriteBit(32509943L, SetBitTo1);
      return Port == 2 && this.WriteBit(15732727L, SetBitTo1);
    }

    internal bool GetInput(int Port, out bool State)
    {
      State = false;
      if (!this.REQ_UD2())
        return false;
      for (int index = 0; index < this.Info.ParameterList.Count; ++index)
      {
        if (this.Info.ParameterList[index].DefineString == "DIGI_IN")
        {
          if (this.Info.ParameterList[index].ValueString.Length != 8)
            return false;
          switch (Port)
          {
            case 1:
              if (this.Info.ParameterList[index].ValueString[1] == '0')
              {
                State = true;
                break;
              }
              break;
            case 2:
              if (this.Info.ParameterList[index].ValueString[2] == '0')
                State = true;
              break;
          }
          return true;
        }
      }
      return false;
    }

    internal new virtual bool ResetDevice()
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.ResetDevice);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.MyBus.MyCom.SetAnswerOffsetTime(1500);
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(6);
      this.TransmitBuffer.Add((int) sbyte.MaxValue);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(2);
      this.TransmitBuffer.Add(165);
      this.TransmitBuffer.Add(90);
      this.TransmitBuffer.Add(175);
      this.TransmitBuffer.Add(254);
      this.WorkTransparentChecksum();
      this.FinishLongFrame();
      bool flag = false;
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (this.ReceiveOkNok())
        {
          flag = true;
          break;
        }
      }
      this.MyBus.MyCom.SetAnswerOffsetTime(0);
      return flag;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: DeviceCollector.EDC
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using AsyncCom;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class EDC : Serie3MBus
  {
    private static Logger logger = LogManager.GetLogger(nameof (EDC));
    private const int CMD_ENTERS_UPDATE_MODE = 1;
    private const int CMD_EVENT_LOG_CLEAR = 3;
    private const int CMD_SYSTEM_LOG_CLEAR = 5;
    private const int CMD_PULSE_DISABLE = 16;
    private const int CMD_PULSE_ENABLE = 17;
    private const int CMD_RADIO_DISABLE = 6;
    private const int CMD_RADIO_NORMAL = 9;
    private const int CMD_COIL_TEST_MODE = 18;
    private const int CMD_RADIO_OOK = 12;
    private const int CMD_RADIO_PN9 = 13;
    private const int CMD_TIME = 14;
    private const int CMD_DEPASSIVATION = 21;
    private const int CMD_PULSEOUT_QUEUE = 24;
    private const int CMD_RADIO_RECEIVE = 10;
    private const int CMD_METER_VALUE = 32;
    private const int CMD_REMOVAL_FLAG_CLEAR = 37;
    private const int CMD_TAMPER_FLAG_CLEAR = 38;
    private const int CMD_BACKFLOW_FLAG_CLEAR = 39;
    private const int CMD_LEAK_FLAG_CLEAR = 40;
    private const int CMD_BLOCK_FLAG_CLEAR = 41;
    private const int CMD_OVERSIZE_FLAG_CLEAR = 42;
    private const int CMD_UNDERSIZE_FLAG_CLEAR = 43;
    private const int CMD_BURST_FLAG_CLEAR = 44;
    private const int CMD_CONFIG_FLAGS = 45;
    private const int CMD_LOG_ENABLE = 47;
    private const int CMD_LOG_DISABLE = 48;
    private const int CMD_LOG_CLEAR_AND_DISABLE_LOG = 49;
    private const int CMD_SERIAL = 52;
    private const int CMD_ADDRESS = 53;
    private const int CMD_GENERATION = 54;
    private const int CMD_MEDIUM = 55;
    private const int CMD_MANUFACTURER = 56;
    private const int CMD_OBIS = 57;

    public EDC(DeviceCollectorFunctions TheBus)
      : base(TheBus)
    {
      this.BaseConstructor();
    }

    public EDC(MBusDevice TheMBusDevice)
      : base(TheMBusDevice)
    {
      this.BaseConstructor();
    }

    private void BaseConstructor() => this.DeviceType = DeviceTypes.EDC;

    public int ReadTimeout_RecTime_OffsetPerBlock
    {
      get
      {
        if (this.MyBus.AsyncCom == null)
          return 800;
        SortedList<AsyncComSettings, object> asyncComSettings = this.MyBus.AsyncCom.GetAsyncComSettings();
        return !asyncComSettings.ContainsKey(AsyncComSettings.RecTime_OffsetPerBlock) || string.IsNullOrEmpty(asyncComSettings[AsyncComSettings.RecTime_OffsetPerBlock].ToString()) ? 800 : Convert.ToInt32(asyncComSettings[AsyncComSettings.RecTime_OffsetPerBlock]);
      }
      set
      {
        if (this.MyBus.AsyncCom == null)
          return;
        this.MyBus.AsyncCom.SingleParameter(CommParameter.RecTime_OffsetPerBlock, value.ToString());
      }
    }

    public new bool ReadVersion(out ReadVersionData versionData)
    {
      return base.ReadVersion(out versionData);
    }

    public bool ReadMemory(ushort startAddress, int size, out byte[] buffer)
    {
      buffer = (byte[]) null;
      this.StartAddress = (int) startAddress;
      this.NumberOfBytes = size;
      if (!this.ReadMemory())
      {
        EDC.logger.Error<ushort, int>("Read memory error at address: 0x{0:X4}, Size: {1}", startAddress, size);
        return false;
      }
      buffer = this.DataBuffer.Data;
      return true;
    }

    public bool RunRAMBackup()
    {
      EDC.logger.Info(nameof (RunRAMBackup));
      return this.RunBackup();
    }

    public new bool ResetDevice()
    {
      int timeOffsetPerBlock = this.ReadTimeout_RecTime_OffsetPerBlock;
      if (timeOffsetPerBlock < 2000)
        this.ReadTimeout_RecTime_OffsetPerBlock = 2000;
      try
      {
        this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.ResetDevice);
        if (!this.MyBus.MyCom.Open())
          return false;
        this.GenerateSendDataHeader();
        this.TransmitBuffer.Add(15);
        this.TransmitBuffer.Add(0);
        this.TransmitBuffer.Add(0);
        this.TransmitBuffer.Add(0);
        this.TransmitBuffer.Add(4);
        this.FinishLongFrame();
        while (!this.MyBus.BreakRequest && this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
        {
          EDC.logger.Info(nameof (ResetDevice));
          this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusDeviceReset);
          if (!this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer))
            return false;
          this.MyBus.BusState.IncrementTransmitBlockCounter();
          if (!this.ReceiveOkNok())
          {
            if (EDC.logger.IsWarnEnabled && this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
              EDC.logger.Warn("--> ResetDevice error. Repeat command!");
          }
          else
          {
            Thread.Sleep(300);
            return true;
          }
        }
        return false;
      }
      finally
      {
        this.ReadTimeout_RecTime_OffsetPerBlock = timeOffsetPerBlock;
      }
    }

    public bool UpdateModeEnter()
    {
      BusDevice.CheckReadOnlyRight();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.UpdateMode);
      if (!this.MyBus.MyCom.Open())
        return false;
      if (this.UpdateModeReadFlash(38400U, (byte) 128) != null)
        return true;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(1);
      this.FinishLongFrame();
      EDC.logger.Debug("Enters the update mode");
      this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
      this.MyBus.BusState.IncrementTransmitBlockCounter();
      ByteField DataBlock = new ByteField(1);
      if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
      {
        Thread.Sleep(200);
        this.MyBus.MyCom.ResetLastTransmitEndTime();
        if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
        {
          Thread.Sleep(200);
          this.MyBus.MyCom.ResetLastTransmitEndTime();
          if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
            return false;
        }
      }
      return DataBlock != null && DataBlock.Count == 1 && DataBlock.Data[0] == (byte) 229;
    }

    public bool UpdateModeExit()
    {
      List<byte> buffer = new List<byte>();
      buffer.Add((byte) 5);
      buffer.Add((byte) 0);
      ushort num = Util.CalculatesCRC16_CC430(buffer);
      buffer.AddRange((IEnumerable<byte>) BitConverter.GetBytes(num));
      byte[] stuffedBuffer = this.GetStuffedBuffer(buffer);
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 170);
      byteList.AddRange((IEnumerable<byte>) stuffedBuffer);
      byteList.Add((byte) 205);
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.UpdateMode);
      if (!this.MyBus.MyCom.Open())
        return false;
      bool wakeupTemporaryOff = this.MyBus.AsyncCom.WakeupTemporaryOff;
      try
      {
        EDC.logger.Debug("Exit the update mode");
        this.MyBus.AsyncCom.WakeupTemporaryOff = true;
        this.MyBus.MyCom.TransmitBlock(byteList.ToArray());
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        Thread.Sleep(2000);
        this.MyBus.MyCom.ResetLastTransmitEndTime();
        ByteField DataBlock = new ByteField(1);
        if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
        {
          Thread.Sleep(100);
          this.MyBus.MyCom.ResetLastTransmitEndTime();
          if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
          {
            Thread.Sleep(300);
            this.MyBus.MyCom.ResetLastTransmitEndTime();
            if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
              return false;
          }
        }
        this.MyBus.MyCom.ClearCom();
        return DataBlock != null && DataBlock.Count == 1 && DataBlock.Data[0] == (byte) 229;
      }
      finally
      {
        this.MyBus.AsyncCom.WakeupTemporaryOff = wakeupTemporaryOff;
      }
    }

    public bool UpdateModeEraseFlash(uint address)
    {
      byte[] bytes = BitConverter.GetBytes(address);
      List<byte> buffer1 = new List<byte>();
      buffer1.Add((byte) 7);
      buffer1.Add((byte) 0);
      buffer1.AddRange((IEnumerable<byte>) bytes);
      ushort num1 = Util.CalculatesCRC16_CC430(buffer1);
      buffer1.AddRange((IEnumerable<byte>) BitConverter.GetBytes(num1));
      byte[] stuffedBuffer = this.GetStuffedBuffer(buffer1);
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 170);
      byteList.AddRange((IEnumerable<byte>) stuffedBuffer);
      byteList.Add((byte) 205);
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.UpdateMode);
      if (!this.MyBus.MyCom.Open())
        return false;
      bool wakeupTemporaryOff = this.MyBus.AsyncCom.WakeupTemporaryOff;
      try
      {
        EDC.logger.Debug("Erase 512 bytes flash at address 0x" + address.ToString("X4"));
        this.MyBus.AsyncCom.WakeupTemporaryOff = true;
        this.MyBus.MyCom.TransmitBlock(byteList.ToArray());
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        List<byte> buffer2 = new List<byte>();
        ByteField DataBlock = new ByteField(1);
        if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
        {
          Thread.Sleep(300);
          this.MyBus.MyCom.ResetLastTransmitEndTime();
          if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
          {
            Thread.Sleep(300);
            this.MyBus.MyCom.ResetLastTransmitEndTime();
            if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
            {
              Thread.Sleep(300);
              this.MyBus.MyCom.ResetLastTransmitEndTime();
              if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
                return false;
            }
          }
        }
        byte num2 = DataBlock.Data[0];
        buffer2.Add(num2);
        if (num2 != (byte) 170)
          return false;
        byte num3;
        do
        {
          if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
          {
            Thread.Sleep(100);
            this.MyBus.MyCom.ResetLastTransmitEndTime();
            if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
            {
              Thread.Sleep(300);
              this.MyBus.MyCom.ResetLastTransmitEndTime();
              if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
              {
                Thread.Sleep(300);
                this.MyBus.MyCom.ResetLastTransmitEndTime();
                if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
                  return false;
              }
            }
          }
          num3 = DataBlock.Data[0];
          buffer2.Add(num3);
        }
        while (num3 != (byte) 205);
        byte[] unstuffedBuffer = this.GetUnstuffedBuffer(buffer2);
        if (unstuffedBuffer.Length < 10)
          return false;
        byte[] numArray = new byte[unstuffedBuffer.Length - 4];
        Buffer.BlockCopy((Array) unstuffedBuffer, 1, (Array) numArray, 0, numArray.Length);
        ushort num4 = Util.CalculatesCRC16_CC430(new List<byte>((IEnumerable<byte>) numArray));
        ushort uint16 = BitConverter.ToUInt16(unstuffedBuffer, 7);
        return unstuffedBuffer != null && unstuffedBuffer[1] == (byte) 135 && unstuffedBuffer[2] == (byte) 0 && (int) unstuffedBuffer[3] == (int) bytes[0] && (int) unstuffedBuffer[4] == (int) bytes[1] && (int) unstuffedBuffer[5] == (int) bytes[2] && (int) unstuffedBuffer[6] == (int) bytes[3] && (int) uint16 == (int) num4;
      }
      finally
      {
        this.MyBus.AsyncCom.WakeupTemporaryOff = wakeupTemporaryOff;
      }
    }

    public bool UpdateModeWriteFlash(uint address, byte[] memory_128byte)
    {
      byte[] bytes = BitConverter.GetBytes(address);
      List<byte> buffer1 = new List<byte>();
      buffer1.Add((byte) 6);
      buffer1.Add((byte) 0);
      buffer1.AddRange((IEnumerable<byte>) bytes);
      buffer1.AddRange((IEnumerable<byte>) memory_128byte);
      ushort num1 = Util.CalculatesCRC16_CC430(buffer1);
      buffer1.AddRange((IEnumerable<byte>) BitConverter.GetBytes(num1));
      byte[] stuffedBuffer = this.GetStuffedBuffer(buffer1);
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 170);
      byteList.AddRange((IEnumerable<byte>) stuffedBuffer);
      byteList.Add((byte) 205);
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.UpdateMode);
      if (!this.MyBus.MyCom.Open())
        return false;
      bool wakeupTemporaryOff = this.MyBus.AsyncCom.WakeupTemporaryOff;
      try
      {
        EDC.logger.Debug("Write 128 bytes flash at address 0x" + address.ToString("X4"));
        this.MyBus.AsyncCom.WakeupTemporaryOff = true;
        this.MyBus.MyCom.TransmitBlock(byteList.ToArray());
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        this.MyBus.MyCom.ResetLastTransmitEndTime();
        List<byte> buffer2 = new List<byte>();
        ByteField DataBlock = new ByteField(1);
        if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
        {
          Thread.Sleep(200);
          this.MyBus.MyCom.ResetLastTransmitEndTime();
          if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
          {
            Thread.Sleep(200);
            this.MyBus.MyCom.ResetLastTransmitEndTime();
            if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
            {
              Thread.Sleep(200);
              this.MyBus.MyCom.ResetLastTransmitEndTime();
              if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
                return false;
            }
          }
        }
        byte num2 = DataBlock.Data[0];
        buffer2.Add(num2);
        if (num2 != (byte) 170)
          return false;
        byte num3;
        do
        {
          if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
          {
            Thread.Sleep(200);
            this.MyBus.MyCom.ResetLastTransmitEndTime();
            if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
            {
              Thread.Sleep(200);
              this.MyBus.MyCom.ResetLastTransmitEndTime();
              if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
              {
                Thread.Sleep(200);
                this.MyBus.MyCom.ResetLastTransmitEndTime();
                if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
                  return false;
              }
            }
          }
          num3 = DataBlock.Data[0];
          buffer2.Add(num3);
        }
        while (num3 != (byte) 205);
        byte[] unstuffedBuffer = this.GetUnstuffedBuffer(buffer2);
        if (unstuffedBuffer.Length < 10)
          return false;
        byte[] numArray = new byte[unstuffedBuffer.Length - 4];
        Buffer.BlockCopy((Array) unstuffedBuffer, 1, (Array) numArray, 0, numArray.Length);
        ushort num4 = Util.CalculatesCRC16_CC430(new List<byte>((IEnumerable<byte>) numArray));
        ushort uint16 = BitConverter.ToUInt16(unstuffedBuffer, 7);
        return unstuffedBuffer != null && unstuffedBuffer[1] == (byte) 134 && unstuffedBuffer[2] == (byte) 0 && (int) unstuffedBuffer[3] == (int) bytes[0] && (int) unstuffedBuffer[4] == (int) bytes[1] && (int) unstuffedBuffer[5] == (int) bytes[2] && (int) unstuffedBuffer[6] == (int) bytes[3] && (int) uint16 == (int) num4;
      }
      finally
      {
        this.MyBus.AsyncCom.WakeupTemporaryOff = wakeupTemporaryOff;
      }
    }

    public byte[] UpdateModeReadFlash(uint address, byte count)
    {
      byte[] bytes = BitConverter.GetBytes(address);
      List<byte> buffer1 = new List<byte>();
      buffer1.Add((byte) 8);
      buffer1.Add((byte) 0);
      buffer1.AddRange((IEnumerable<byte>) bytes);
      buffer1.Add(count);
      ushort num1 = Util.CalculatesCRC16_CC430(buffer1);
      buffer1.AddRange((IEnumerable<byte>) BitConverter.GetBytes(num1));
      byte[] stuffedBuffer = this.GetStuffedBuffer(buffer1);
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 170);
      byteList.AddRange((IEnumerable<byte>) stuffedBuffer);
      byteList.Add((byte) 205);
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.UpdateMode);
      if (!this.MyBus.MyCom.Open())
        return (byte[]) null;
      bool wakeupTemporaryOff = this.MyBus.AsyncCom.WakeupTemporaryOff;
      try
      {
        EDC.logger.Debug("Read " + count.ToString() + " bytes flash at address 0x" + address.ToString("X4"));
        this.MyBus.AsyncCom.WakeupTemporaryOff = true;
        this.MyBus.MyCom.TransmitBlock(byteList.ToArray());
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        List<byte> buffer2 = new List<byte>();
        ByteField DataBlock = new ByteField(1);
        if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
        {
          Thread.Sleep(200);
          this.MyBus.MyCom.ResetLastTransmitEndTime();
          if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
          {
            Thread.Sleep(400);
            this.MyBus.MyCom.ResetLastTransmitEndTime();
            if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
              return (byte[]) null;
          }
        }
        buffer2.AddRange((IEnumerable<byte>) DataBlock.Data);
        if (buffer2[0] != (byte) 170)
          return (byte[]) null;
        while (buffer2[buffer2.Count - 1] != (byte) 205)
        {
          if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
          {
            Thread.Sleep(200);
            this.MyBus.MyCom.ResetLastTransmitEndTime();
            if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
            {
              Thread.Sleep(200);
              this.MyBus.MyCom.ResetLastTransmitEndTime();
              if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
                return (byte[]) null;
            }
          }
          buffer2.Add(DataBlock.Data[0]);
        }
        byte[] unstuffedBuffer = this.GetUnstuffedBuffer(buffer2);
        byte[] numArray = new byte[unstuffedBuffer.Length - 4];
        Buffer.BlockCopy((Array) unstuffedBuffer, 1, (Array) numArray, 0, numArray.Length);
        ushort num2 = Util.CalculatesCRC16_CC430(new List<byte>((IEnumerable<byte>) numArray));
        ushort uint16 = BitConverter.ToUInt16(unstuffedBuffer, unstuffedBuffer.Length - 3);
        if (unstuffedBuffer == null || unstuffedBuffer[1] != (byte) 136 || unstuffedBuffer[2] != (byte) 0 || (int) unstuffedBuffer[3] != (int) bytes[0] || (int) unstuffedBuffer[4] != (int) bytes[1] || (int) unstuffedBuffer[5] != (int) bytes[2] || (int) unstuffedBuffer[6] != (int) bytes[3] || (int) uint16 != (int) num2)
          return (byte[]) null;
        byte[] dst = new byte[(int) count];
        Buffer.BlockCopy((Array) unstuffedBuffer, 7, (Array) dst, 0, dst.Length);
        return dst;
      }
      finally
      {
        this.MyBus.AsyncCom.WakeupTemporaryOff = wakeupTemporaryOff;
      }
    }

    private byte[] GetStuffedBuffer(List<byte> buffer)
    {
      List<byte> byteList = new List<byte>();
      foreach (byte num1 in buffer)
      {
        int num2;
        switch (num1)
        {
          case 170:
          case 205:
            num2 = 1;
            break;
          default:
            num2 = num1 == (byte) 92 ? 1 : 0;
            break;
        }
        if (num2 != 0)
        {
          byteList.Add((byte) 92);
          byteList.Add(~num1);
        }
        else
          byteList.Add(num1);
      }
      return byteList.ToArray();
    }

    private byte[] GetUnstuffedBuffer(List<byte> buffer)
    {
      List<byte> byteList = new List<byte>();
      for (int index = 0; index < buffer.Count; ++index)
      {
        if (buffer[index] == (byte) 92)
        {
          ++index;
          byteList.Add(~buffer[index]);
        }
        else
          byteList.Add(buffer[index]);
      }
      return byteList.ToArray();
    }

    public bool PulseDisable()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks.PulseDisable, (byte) 16) && this.ReceiveACK_NACK((byte) 16);
    }

    public bool PulseEnable()
    {
      return this.ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks.PulseEnable, (byte) 17) && this.ReceiveACK_NACK((byte) 17);
    }

    public bool RadioDisable()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks.RadioDisable, (byte) 6) && this.ReceiveACK_NACK((byte) 6);
    }

    public bool RadioNormal()
    {
      return this.ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks.RadioNormal, (byte) 9) && this.ReceiveACK_NACK((byte) 9);
    }

    private bool ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks task, byte cmd)
    {
      this.MyBus.BusState.StartBusFunctionTask(task);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(cmd);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        EDC.logger.Debug("Send cmd: " + task.ToString());
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn(" ... repeat run " + task.ToString());
        }
        else
        {
          if (this.ReceiveLongframeEnd())
            return true;
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn(" ... repeat run " + task.ToString());
        }
      }
      return false;
    }

    public bool StartVolumeMonitor()
    {
      BusDevice.CheckReadOnlyRight();
      this.MyBus.MyCom.BreakRequest = false;
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.StartVolumeMonitor);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(18);
      this.FinishLongFrame();
      EDC.logger.Info("Send cmd: StartVolumeMonitor");
      this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
      this.MyBus.BusState.IncrementTransmitBlockCounter();
      Thread.Sleep(1200);
      this.MyBus.MyCom.ResetLastTransmitEndTime();
      if (this.MyBus.AsyncCom.InputBufferLength > 0L)
      {
        EDC.logger.Info("ok");
        return true;
      }
      EDC.logger.Error("Failed to start the volume monitor.");
      return false;
    }

    public bool StopVolumeMonitor_SendE5()
    {
      this.MyBus.MyCom.BreakRequest = false;
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.StopVolumeMonitor);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.MyBus.MyCom.WakeupTemporaryOff = true;
      ByteField DataBlock = new ByteField(new byte[1]
      {
        (byte) 229
      });
      EDC.logger.Info("Send cmd: StopVolumeMonitor_SendE5");
      this.MyBus.MyCom.TransmitBlock(ref DataBlock);
      this.MyBus.MyCom.TransmitBlock(ref DataBlock);
      this.MyBus.MyCom.TransmitBlock(ref DataBlock);
      this.MyBus.BusState.IncrementTransmitBlockCounter();
      Thread.Sleep(600);
      this.MyBus.MyCom.ResetLastTransmitEndTime();
      LogManager.DisableLogging();
      try
      {
        this.MyBus.MyCom.ClearCom();
      }
      finally
      {
        LogManager.EnableLogging();
      }
      ZR_ClassLibMessages.ClearErrorText();
      return true;
    }

    public bool WriteRAM(ushort address, byte[] buffer)
    {
      BusDevice.CheckReadOnlyRight();
      int timeOffsetPerBlock = this.ReadTimeout_RecTime_OffsetPerBlock;
      if (timeOffsetPerBlock < 1000)
        this.ReadTimeout_RecTime_OffsetPerBlock = 1000;
      try
      {
        bool flag = this.Write(MemoryLocation.RAM, address, buffer);
        if (flag)
          Thread.Sleep(100);
        return flag;
      }
      finally
      {
        this.ReadTimeout_RecTime_OffsetPerBlock = timeOffsetPerBlock;
      }
    }

    public bool WriteFLASH(ushort address, byte[] buffer)
    {
      BusDevice.CheckReadOnlyRight();
      int timeOffsetPerBlock = this.ReadTimeout_RecTime_OffsetPerBlock;
      if (timeOffsetPerBlock < 1000)
        this.ReadTimeout_RecTime_OffsetPerBlock = 1000;
      try
      {
        bool flag = this.Write(MemoryLocation.FLASH, address, buffer);
        if (flag)
          Thread.Sleep(200);
        return flag;
      }
      finally
      {
        this.ReadTimeout_RecTime_OffsetPerBlock = timeOffsetPerBlock;
      }
    }

    private bool Write(MemoryLocation location, ushort address, byte[] buffer)
    {
      BusDevice.CheckReadOnlyRight();
      if (location != MemoryLocation.FLASH && location != MemoryLocation.RAM)
        return false;
      if (buffer == null || buffer.Length == 0)
        return true;
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.WriteMemoryBlock);
      if (!this.MyBus.MyCom.Open())
        return false;
      int IntToByte = 3;
      if (location == MemoryLocation.FLASH)
      {
        IntToByte = 1;
        if (buffer.Length % 4 != 0)
          throw new Exception("Internally EDC Handler Bug: FLASH write. Write buffer is not multiple of 4!");
      }
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(BitConverter.GetBytes(address));
      this.TransmitBuffer.Add((byte) buffer.Length);
      this.TransmitBuffer.Add(IntToByte);
      this.TransmitBuffer.Add(buffer);
      this.FinishLongFrame();
      bool flag = false;
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        EDC.logger.Info("Write " + location.ToString() + " 0x" + address.ToString("X4"));
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveOkNok())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn(" ... repeat run write " + location.ToString());
        }
        else
        {
          ZR_ClassLibMessages.ClearErrorText();
          return true;
        }
      }
      return flag;
    }

    public bool SendSND_NKE()
    {
      this.SND_NKE_Broadcast();
      return true;
    }

    public bool RadioOOK()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks.RadioOOK, (byte) 12) && this.ReceiveACK_NACK((byte) 12);
    }

    public bool RadioOOK(RadioMode mode, short offset, ushort timeoutInSeconds)
    {
      return this.RadioTest(BusStatusClass.BusFunctionTasks.RadioOOK, (byte) 12, mode, offset, timeoutInSeconds);
    }

    public bool RadioPN9()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks.RadioPN9, (byte) 13) && this.ReceiveACK_NACK((byte) 13);
    }

    public bool RadioPN9(RadioMode mode, short offset, ushort timeoutInSeconds)
    {
      return this.RadioTest(BusStatusClass.BusFunctionTasks.RadioPN9, (byte) 13, mode, offset, timeoutInSeconds);
    }

    private bool RadioTest(
      BusStatusClass.BusFunctionTasks task,
      byte cmd,
      RadioMode mode,
      short offset,
      ushort timeoutInSeconds)
    {
      BusDevice.CheckReadOnlyRight();
      this.MyBus.BusState.StartBusFunctionTask(task);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(cmd);
      this.TransmitBuffer.Add((byte) mode);
      this.TransmitBuffer.Add(BitConverter.GetBytes(offset));
      this.TransmitBuffer.Add(BitConverter.GetBytes(timeoutInSeconds));
      this.FinishLongFrame();
      bool flag = false;
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        EDC.logger.Debug("Send cmd: " + task.ToString());
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn(" ... repeat run " + task.ToString());
        }
        else
        {
          if (this.ReceiveLongframeEnd())
            return this.ReceiveACK_NACK(cmd);
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn(" ... repeat run " + task.ToString());
        }
      }
      return flag;
    }

    public bool StartDepassivation()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks.StartDepassivation, (byte) 21) && this.ReceiveACK_NACK((byte) 21);
    }

    public DateTime? ReadSystemTime()
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.ReadMemoryBlock);
      if (!this.MyBus.MyCom.Open())
        return new DateTime?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(14);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        EDC.logger.Debug("Send cmd: ReadSystemTime");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn(" ... repeat run ReadSystemTime");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn(" ... repeat run ReadSystemTime");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 12 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 64 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown EDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          byte num = this.ReceiveBuffer.Data[6];
          byte month = this.ReceiveBuffer.Data[7];
          byte day = this.ReceiveBuffer.Data[8];
          byte hour = this.ReceiveBuffer.Data[9];
          byte minute = this.ReceiveBuffer.Data[10];
          byte second = this.ReceiveBuffer.Data[11];
          if (month > (byte) 12 || day > (byte) 31 || hour > (byte) 23 || minute > (byte) 59 || second > (byte) 59)
            return new DateTime?(new DateTime(2000, 1, 1));
          if (month == (byte) 0 || day == (byte) 0)
            return new DateTime?(new DateTime(2000, 1, 1));
          try
          {
            return new DateTime?(new DateTime((int) num + 2000, (int) month, (int) day, (int) hour, (int) minute, (int) second));
          }
          catch
          {
            return new DateTime?(new DateTime(2000, 1, 1));
          }
        }
      }
      return new DateTime?();
    }

    public bool WriteSystemTime(DateTime value)
    {
      BusDevice.CheckReadOnlyRight();
      if (value.Year < 2000)
        throw new ArgumentOutOfRangeException("Can not write system time! The year should be greater or equal to 2000. Value: " + value.ToLongDateString());
      if (value.Year > 2255)
        throw new ArgumentOutOfRangeException("Can not write system time! The year should be smaller as 2255. Value: " + value.ToLongDateString());
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.ReadMemoryBlock);
      byte Byte = (byte) (value.Year - 2000);
      byte month = (byte) value.Month;
      byte day = (byte) value.Day;
      byte hour = (byte) value.Hour;
      byte minute = (byte) value.Minute;
      byte second = (byte) value.Second;
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(14);
      this.TransmitBuffer.Add(Byte);
      this.TransmitBuffer.Add(month);
      this.TransmitBuffer.Add(day);
      this.TransmitBuffer.Add(hour);
      this.TransmitBuffer.Add(minute);
      this.TransmitBuffer.Add(second);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        EDC.logger.Info("WriteSystemTime(" + value.ToString("g") + ")");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn(" ... repeat run ReadSystemTime");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn(" ... repeat run ReadSystemTime");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 12 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 64 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown EDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return (int) Byte == (int) this.ReceiveBuffer.Data[6] && (int) month == (int) this.ReceiveBuffer.Data[7] && (int) day == (int) this.ReceiveBuffer.Data[8] && (int) hour == (int) this.ReceiveBuffer.Data[9] && (int) minute == (int) this.ReceiveBuffer.Data[10];
        }
      }
      return false;
    }

    internal bool ReceiveACK_NACK(byte cmd)
    {
      ZR_ClassLibMessages.ClearErrors();
      if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 64 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
        throw new Exception("Unknown EDC response detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
      if (this.ReceiveBuffer.Data[5] == byte.MaxValue)
        throw new Exception("Invalid command was send to EDC device! CMD: 0x" + cmd.ToString("X2"));
      if (this.ReceiveBuffer.Data[5] == (byte) 251 || this.ReceiveBuffer.Data[5] == (byte) 252 || this.ReceiveBuffer.Data[5] == (byte) 253 || this.ReceiveBuffer.Data[5] == (byte) 254)
        throw new Exception("Invalid response was received! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
      if (this.ReceiveBuffer.Data[5] == (byte) 250 && (int) this.ReceiveBuffer.Data[6] == (int) cmd)
      {
        EDC.logger.Debug("... successful ");
        return true;
      }
      if ((int) this.ReceiveBuffer.Data[5] != ((int) cmd | 128))
        return false;
      EDC.logger.Debug("... successful ");
      return true;
    }

    public bool EraseFLASHSegment(ushort address)
    {
      BusDevice.CheckReadOnlyRight();
      EDC.logger.Info("Erase FLASH 0x" + address.ToString("X4"));
      int timeOffsetPerBlock = this.ReadTimeout_RecTime_OffsetPerBlock;
      if (timeOffsetPerBlock < 1000)
        this.ReadTimeout_RecTime_OffsetPerBlock = 1000;
      try
      {
        bool flag = this.EraseFlash((int) address, 0);
        if (flag)
          Thread.Sleep(200);
        return flag;
      }
      finally
      {
        this.ReadTimeout_RecTime_OffsetPerBlock = timeOffsetPerBlock;
      }
    }

    public bool WritePulseoutQueue(short value, bool clearQueue)
    {
      BusDevice.CheckReadOnlyRight();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.WritePulseoutQueue);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(24);
      this.TransmitBuffer.Add(BitConverter.GetBytes(value));
      this.TransmitBuffer.Add(BitConverter.GetBytes(clearQueue));
      this.FinishLongFrame();
      bool flag = false;
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        EDC.logger.Debug("Send cmd: " + BusStatusClass.BusFunctionTasks.WritePulseoutQueue.ToString());
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn(" ... repeat run " + BusStatusClass.BusFunctionTasks.WritePulseoutQueue.ToString());
        }
        else
        {
          if (this.ReceiveLongframeEnd())
            return this.ReceiveACK_NACK((byte) 24);
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn(" ... repeat run " + BusStatusClass.BusFunctionTasks.WritePulseoutQueue.ToString());
        }
      }
      return flag;
    }

    public bool RadioReceive(
      out RadioPacket packet,
      out byte[] buffer,
      out int rssi_dBm,
      out int lqi,
      uint timeout)
    {
      BusDevice.CheckReadOnlyRight();
      packet = (RadioPacket) null;
      buffer = (byte[]) null;
      rssi_dBm = 0;
      lqi = 0;
      DateTime now1 = DateTime.Now;
      try
      {
        if (!this.PulseDisable())
          throw new Exception("Can not disable radio!");
        if (!this.ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks.RadioReceive, (byte) 10))
          throw new Exception("Can not start radio receiver!");
        if (!this.ReceiveACK_NACK((byte) 10))
          throw new Exception("Missing OK after start radio receiver!");
        DateTime now2 = DateTime.Now;
        while ((DateTime.Now - now2).TotalMilliseconds < (double) timeout && this.MyBus.MyCom.InputBufferLength < 19L)
        {
          if (this.MyBus.BreakRequest)
            throw new Exception("Function was canceled!");
          EDC.logger.Trace("Wait 100 ms");
          Thread.Sleep(100);
          this.MyBus.MyCom.ResetLastTransmitEndTime();
        }
        if ((DateTime.Now - now2).TotalMilliseconds > (double) timeout)
        {
          string TheDescription = "Timeout " + timeout.ToString() + " ms expired. The timeout period elapsed prior to completion of the operation or the device is not responding.";
          if (this.MyBus.MyCom.InputBufferLength > 0L)
            TheDescription = TheDescription + "Invalid buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data, 0);
          ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.Timeout, TheDescription);
          return false;
        }
        this.MyBus.MyCom.ResetLastTransmitEndTime();
        if (!this.ReceiveHeader())
          throw new Exception("Can not encode header!");
        if (!this.ReceiveLongframeEnd())
          throw new Exception("Can not encode M-Bus long frame!");
        if (!this.ReceiveACK_NACK((byte) 10))
          throw new Exception("Can not receive OK after sucessfull radio receive!");
        if (this.ReceiveBuffer.Data.Length < 7 || this.ReceiveBuffer.Data[6] > (byte) 0)
          throw new Exception("Faled to recive radio packet! Error number: " + this.ReceiveBuffer.Data[6].ToString());
        buffer = new byte[this.ReceiveBuffer.Data.Length - 7 - 2];
        Buffer.BlockCopy((Array) this.ReceiveBuffer.Data, 7, (Array) buffer, 0, buffer.Length);
        packet = RadioPacket.Parse(buffer, true);
        rssi_dBm = Util.RssiToRssi_dBm(buffer[buffer.Length - 2]);
        lqi = (int) buffer[buffer.Length - 1];
        return true;
      }
      finally
      {
        ZR_ClassLibMessages.LastErrors lastError = ZR_ClassLibMessages.GetLastError();
        this.RadioDisable();
        if (lastError != 0)
          ZR_ClassLibMessages.AddErrorDescription(lastError);
      }
    }

    public bool StartRadioReceiver()
    {
      BusDevice.CheckReadOnlyRight();
      if (!this.PulseDisable())
        throw new Exception("Failed to start the radio receiver! Can not disable the coil sampling.");
      if (!this.ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks.RadioReceive, (byte) 10))
        throw new Exception("Can not start radio receiver!");
      if (!this.ReceiveACK_NACK((byte) 10))
        throw new Exception("Missing OK after start radio receiver!");
      return true;
    }

    public bool EventLogClear()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks.EventLogClear, (byte) 3) && this.ReceiveACK_NACK((byte) 3);
    }

    public bool SystemLogClear()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks.SystemLogClear, (byte) 5) && this.ReceiveACK_NACK((byte) 5);
    }

    public bool RemovalFlagClear()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks.RemovalFlagClear, (byte) 37) && this.ReceiveACK_NACK((byte) 37);
    }

    public bool TamperFlagClear()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks.TamperFlagClear, (byte) 38) && this.ReceiveACK_NACK((byte) 38);
    }

    public bool BackflowFlagClear()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks.BackflowFlagClear, (byte) 39) && this.ReceiveACK_NACK((byte) 39);
    }

    public bool LeakFlagClear()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks.LeakFlagClear, (byte) 40) && this.ReceiveACK_NACK((byte) 40);
    }

    public bool BlockFlagClear()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks.BlockFlagClear, (byte) 41) && this.ReceiveACK_NACK((byte) 41);
    }

    public bool OversizeFlagClear()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks.OversizeFlagClear, (byte) 42) && this.ReceiveACK_NACK((byte) 42);
    }

    public bool UndersizeFlagClear()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks.UndersizeFlagClear, (byte) 43) && this.ReceiveACK_NACK((byte) 43);
    }

    public bool BurstFlagClear()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks.BurstFlagClear, (byte) 44) && this.ReceiveACK_NACK((byte) 44);
    }

    public bool LogEnable()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks.LogEnable, (byte) 47) && this.ReceiveACK_NACK((byte) 47);
    }

    public bool LogDisable()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks.LogDisable, (byte) 48) && this.ReceiveACK_NACK((byte) 48);
    }

    public bool LogClearAndDisableLog()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand(BusStatusClass.BusFunctionTasks.LogClearAndDisableLog, (byte) 49) && this.ReceiveACK_NACK((byte) 49);
    }

    public bool WriteMeterValue(uint value)
    {
      BusDevice.CheckReadOnlyRight();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.WriteMeterValue);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(32);
      this.TransmitBuffer.Add(BitConverter.GetBytes(value));
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        EDC.logger.Info("WriteMeterValue(" + value.ToString() + ")");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn(" ... repeat run WriteMeterValue");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn(" ... repeat run WriteMeterValue");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 64 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown EDC response detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return (long) BitConverter.ToInt32(this.ReceiveBuffer.Data, 6) == (long) value;
        }
      }
      return false;
    }

    public uint? ReadMeterValue()
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.ReadMeterValue);
      if (!this.MyBus.MyCom.Open())
        return new uint?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(32);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        EDC.logger.Debug("Send cmd: ReadMeterValue");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn(" ... repeat run ReadMeterValue");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn(" ... repeat run ReadMeterValue");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 64 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown EDC response detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new uint?(BitConverter.ToUInt32(this.ReceiveBuffer.Data, 6));
        }
      }
      return new uint?();
    }

    public ushort? ReadConfigFlags()
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      if (!this.MyBus.MyCom.Open())
        return new ushort?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(45);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        EDC.logger.Debug(nameof (ReadConfigFlags));
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn(" ... repeat run ReadConfigFlags");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn(" ... repeat run ReadConfigFlags");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 64 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown EDC response detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new ushort?(BitConverter.ToUInt16(this.ReceiveBuffer.Data, 6));
        }
      }
      return new ushort?();
    }

    public bool WriteConfigFlags(ushort value)
    {
      BusDevice.CheckReadOnlyRight();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(45);
      this.TransmitBuffer.Add(BitConverter.GetBytes(value));
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        EDC.logger.Info("WriteConfigFlags(" + value.ToString() + ")");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn(" ... repeat run WriteConfigFlags");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn(" ... repeat run WriteConfigFlags");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 64 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown EDC response detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return (int) BitConverter.ToUInt16(this.ReceiveBuffer.Data, 6) == (int) value;
        }
      }
      return false;
    }

    public uint? ReadSerialnumber(byte index)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      if (!this.MyBus.MyCom.Open())
        return new uint?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(52);
      this.TransmitBuffer.Add(index);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        EDC.logger.Debug("ReadSerialnumber(" + index.ToString() + ")");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 13 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 64 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) index)
            throw new Exception("Unknown EDC response detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new uint?(Util.ConvertBcdUInt32ToUInt32(BitConverter.ToUInt32(this.ReceiveBuffer.Data, 7)));
        }
      }
      return new uint?();
    }

    public bool WriteSerialnumber(byte index, uint serial)
    {
      BusDevice.CheckReadOnlyRight();
      uint bcdUint32 = Util.ConvertUnt32ToBcdUInt32(serial);
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(52);
      this.TransmitBuffer.Add(index);
      this.TransmitBuffer.Add(BitConverter.GetBytes(bcdUint32));
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        EDC.logger.Info("WriteSerialnumber(" + index.ToString() + ", " + serial.ToString() + "=>BCD: 0x" + bcdUint32.ToString("X8") + ")");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 13 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 64 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) index)
            throw new Exception("Unknown EDC response detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return (int) BitConverter.ToUInt32(this.ReceiveBuffer.Data, 7) == (int) bcdUint32;
        }
      }
      return false;
    }

    public byte? ReadAddress(byte index)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      if (!this.MyBus.MyCom.Open())
        return new byte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(53);
      this.TransmitBuffer.Add(index);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        EDC.logger.Debug("ReadAddress(" + index.ToString() + ")");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 64 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) index)
            throw new Exception("Unknown EDC response detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new byte?(this.ReceiveBuffer.Data[7]);
        }
      }
      return new byte?();
    }

    public bool WriteAddress(byte index, byte address)
    {
      BusDevice.CheckReadOnlyRight();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(53);
      this.TransmitBuffer.Add(index);
      this.TransmitBuffer.Add(address);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        EDC.logger.Info("WriteAddress(" + address.ToString() + ")");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 64 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) index)
            throw new Exception("Unknown EDC response detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return (int) this.ReceiveBuffer.Data[7] == (int) address;
        }
      }
      return false;
    }

    public byte? ReadGeneration(byte index)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      if (!this.MyBus.MyCom.Open())
        return new byte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(54);
      this.TransmitBuffer.Add(index);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        EDC.logger.Debug("ReadGeneration(" + index.ToString() + ")");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 64 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) index)
            throw new Exception("Unknown EDC response detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new byte?(this.ReceiveBuffer.Data[7]);
        }
      }
      return new byte?();
    }

    public bool WriteGeneration(byte index, byte generation)
    {
      BusDevice.CheckReadOnlyRight();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(54);
      this.TransmitBuffer.Add(index);
      this.TransmitBuffer.Add(generation);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        EDC.logger.Info("WriteGeneration(" + generation.ToString() + ")");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 64 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) index)
            throw new Exception("Unknown EDC response detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return (int) this.ReceiveBuffer.Data[7] == (int) generation;
        }
      }
      return false;
    }

    public byte? ReadMedium(byte index)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      if (!this.MyBus.MyCom.Open())
        return new byte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(55);
      this.TransmitBuffer.Add(index);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        EDC.logger.Debug("ReadMedium(" + index.ToString() + ")");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 64 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) index)
            throw new Exception("Unknown EDC response detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new byte?(this.ReceiveBuffer.Data[7]);
        }
      }
      return new byte?();
    }

    public bool WriteMedium(byte index, byte medium)
    {
      BusDevice.CheckReadOnlyRight();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(55);
      this.TransmitBuffer.Add(index);
      this.TransmitBuffer.Add(medium);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        EDC.logger.Info("WriteMedium(" + medium.ToString() + ")");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 64 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) index)
            throw new Exception("Unknown EDC response detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return (int) this.ReceiveBuffer.Data[7] == (int) medium;
        }
      }
      return false;
    }

    public ushort? ReadManufacturer(byte index)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      if (!this.MyBus.MyCom.Open())
        return new ushort?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(56);
      this.TransmitBuffer.Add(index);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        EDC.logger.Debug("ReadManufacturer(" + index.ToString() + ")");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 64 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) index)
            throw new Exception("Unknown EDC response detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new ushort?(BitConverter.ToUInt16(this.ReceiveBuffer.Data, 7));
        }
      }
      return new ushort?();
    }

    public bool WriteManufacturer(byte index, ushort manufacturer)
    {
      BusDevice.CheckReadOnlyRight();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(56);
      this.TransmitBuffer.Add(index);
      this.TransmitBuffer.Add(BitConverter.GetBytes(manufacturer));
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        EDC.logger.Info("WriteManufacturer(" + manufacturer.ToString() + ")");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 64 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) index)
            throw new Exception("Unknown EDC response detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return (int) BitConverter.ToUInt16(this.ReceiveBuffer.Data, 7) == (int) manufacturer;
        }
      }
      return false;
    }

    public byte? ReadObis(byte index)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      if (!this.MyBus.MyCom.Open())
        return new byte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(57);
      this.TransmitBuffer.Add(index);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        EDC.logger.Debug("ReadObis(" + index.ToString() + ")");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 64 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) index)
            throw new Exception("Unknown EDC response detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new byte?(this.ReceiveBuffer.Data[7]);
        }
      }
      return new byte?();
    }

    public bool WriteObis(byte index, byte obis)
    {
      BusDevice.CheckReadOnlyRight();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(64);
      this.TransmitBuffer.Add(57);
      this.TransmitBuffer.Add(index);
      this.TransmitBuffer.Add(obis);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        EDC.logger.Info("WriteObis(" + obis.ToString() + ")");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            EDC.logger.Warn("repeat");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 64 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) index)
            throw new Exception("Unknown EDC response detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return (int) this.ReceiveBuffer.Data[7] == (int) obis;
        }
      }
      return false;
    }
  }
}

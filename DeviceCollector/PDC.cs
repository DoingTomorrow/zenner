// Decompiled with JetBrains decompiler
// Type: DeviceCollector.PDC
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
  public class PDC : Serie3MBus
  {
    private static Logger logger = LogManager.GetLogger(nameof (PDC));
    private const byte CommIDVersionPDC = 0;
    private const byte CommIDUpdate = 1;
    private const byte CommIDEventLogRead = 2;
    private const byte CommIDEventLogClear = 3;
    private const byte CommIDSystemLogRead = 4;
    private const byte CommIDSystemLogClear = 5;
    private const byte CommIDRadioDisable = 6;
    private const byte CommIDRadioMode = 7;
    private const byte CommIDRadioBaseTime = 8;
    private const byte CommIDRadioNormal = 9;
    private const byte CommIDRadioReceive = 10;
    private const byte CommIDRadioTransmit = 11;
    private const byte CommIDRadioOOK = 12;
    private const byte CommIDRadioPN9 = 13;
    private const byte CommIDTime = 14;
    private const byte CommIDPulseSettings = 15;
    private const byte CommIDPulseDisable = 16;
    private const byte CommIDPulseEnable = 17;
    private const byte CommIDPulseTest = 18;
    private const byte CommIDPowerCheck = 19;
    private const byte CommIDLogHeaders = 20;
    private const byte CommIDListQuery = 21;
    private const byte CommIDRadioList = 22;
    private const byte CommIDConfigFlags = 23;
    private const byte CommIDRadioFlags = 24;
    private const byte CommIDKeyDate = 25;
    private const byte CommIDResetToDelivery = 26;
    private const byte CommIDStatusFlags = 32;
    private const byte CommIDMeterValue = 33;
    private const byte CommIDVif = 34;
    private const byte CommIDExponent = 35;
    private const byte CommIDMantissa = 36;
    private const byte CommIDFlowBlock = 37;
    private const byte CommIDFlowLeak = 38;
    private const byte CommIDFlowBurst = 39;
    private const byte CommIDFlowOversize = 40;
    private const byte CommIDFlowUndersize = 41;
    private const byte CommIDSerial = 48;
    private const byte CommIDMBusAddress = 49;
    private const byte CommIDMBusVersion = 50;
    private const byte CommIDMBusMedium = 51;
    private const byte CommIDMBusManid = 52;
    private const byte CommIDObisCode = 53;
    private const byte CommIDDepassivate = 64;
    private const byte CommIDDepassSettings = 65;
    private const byte CommIDMBusBreak = 66;
    private const byte CommIDMBusStatus = 67;

    public PDC(DeviceCollectorFunctions TheBus)
      : base(TheBus)
    {
      this.BaseConstructor();
    }

    public PDC(MBusDevice TheMBusDevice)
      : base(TheMBusDevice)
    {
      this.BaseConstructor();
    }

    private void BaseConstructor() => this.DeviceType = DeviceTypes.PDC;

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
        PDC.logger.Error<ushort, int>("Read memory error at address: 0x{0:X4}, Size: {1}", startAddress, size);
        return false;
      }
      buffer = this.DataBuffer.Data;
      return true;
    }

    public bool RunRAMBackup()
    {
      PDC.logger.Info(nameof (RunRAMBackup));
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
          PDC.logger.Info(nameof (ResetDevice));
          this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusDeviceReset);
          if (!this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer))
            return false;
          this.MyBus.BusState.IncrementTransmitBlockCounter();
          if (!this.ReceiveOkNok())
          {
            if (PDC.logger.IsWarnEnabled && this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
              PDC.logger.Warn("--> ResetDevice error. Repeat command!");
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
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 1);
      this.FinishLongFrame();
      PDC.logger.Debug("Enters the update mode");
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
        PDC.logger.Debug("Exit the update mode");
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
        PDC.logger.Debug("Erase 512 bytes flash at address 0x" + address.ToString("X4"));
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
        PDC.logger.Debug("Write 128 bytes flash at address 0x" + address.ToString("X4"));
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
        PDC.logger.Debug("Read " + count.ToString() + " bytes flash at address 0x" + address.ToString("X4"));
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
      return this.ExecuteSimpleCommand((byte) 16) && this.ReceiveACK_NACK((byte) 16);
    }

    public bool PulseEnable()
    {
      return this.ExecuteSimpleCommand((byte) 17) && this.ReceiveACK_NACK((byte) 17);
    }

    public uint? PulseSettingsRead()
    {
      if (!this.MyBus.MyCom.Open())
        return new uint?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 15);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: PulseSettingsRead");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run PulseSettingsRead");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run PulseSettingsRead");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new uint?(BitConverter.ToUInt32(this.ReceiveBuffer.Data, 6));
        }
      }
      return new uint?();
    }

    public uint? PulseSettingsWrite(ushort period, byte ontime)
    {
      BusDevice.CheckReadOnlyRight();
      if (!this.MyBus.MyCom.Open())
        return new uint?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 15);
      this.TransmitBuffer.Add((int) period);
      this.TransmitBuffer.Add(ontime);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: PulseSettingsWrite");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run PulseSettingsWrite");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run PulseSettingsWrite");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new uint?(BitConverter.ToUInt32(this.ReceiveBuffer.Data, 6));
        }
      }
      return new uint?();
    }

    public bool RadioDisable()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand((byte) 6) && this.ReceiveACK_NACK((byte) 6);
    }

    public bool RadioNormal()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand((byte) 9) && this.ReceiveACK_NACK((byte) 9);
    }

    private bool ExecuteSimpleCommand(byte cmd)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add(cmd);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: 0x" + cmd.ToString("X2"));
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run 0x" + cmd.ToString("X2"));
        }
        else
        {
          if (this.ReceiveLongframeEnd())
            return true;
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run 0x" + cmd.ToString("X2"));
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
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 18);
      this.FinishLongFrame();
      PDC.logger.Info("Send cmd: StartVolumeMonitor");
      this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
      this.MyBus.BusState.IncrementTransmitBlockCounter();
      Thread.Sleep(1200);
      this.MyBus.MyCom.ResetLastTransmitEndTime();
      if (this.MyBus.AsyncCom.InputBufferLength > 0L)
      {
        PDC.logger.Info("ok");
        return true;
      }
      PDC.logger.Error("Failed to start the volume monitor.");
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
      PDC.logger.Info("Send cmd: StopVolumeMonitor_SendE5");
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
          throw new Exception("Internally PDC Handler Bug: FLASH write. Write buffer is not multiple of 4!");
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
        PDC.logger.Info("Write " + location.ToString() + " 0x" + address.ToString("X4"));
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveOkNok())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run write " + location.ToString());
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
      return this.ExecuteSimpleCommand((byte) 12) && this.ReceiveACK_NACK((byte) 12);
    }

    public bool RadioOOK(RadioMode mode, short offset, ushort timeoutInSeconds)
    {
      return this.RadioTest((byte) 12, mode, offset, timeoutInSeconds);
    }

    public bool RadioPN9()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand((byte) 13) && this.ReceiveACK_NACK((byte) 13);
    }

    public bool RadioPN9(RadioMode mode, short offset, ushort timeoutInSeconds)
    {
      return this.RadioTest((byte) 13, mode, offset, timeoutInSeconds);
    }

    private bool RadioTest(byte cmd, RadioMode mode, short offset, ushort timeoutInSeconds)
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
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add(cmd);
      this.TransmitBuffer.Add((byte) mode);
      this.TransmitBuffer.Add(BitConverter.GetBytes(offset));
      this.TransmitBuffer.Add(BitConverter.GetBytes(timeoutInSeconds));
      this.FinishLongFrame();
      bool flag = false;
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: 0x" + cmd.ToString("X2"));
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run 0x" + cmd.ToString("X2"));
        }
        else
        {
          if (this.ReceiveLongframeEnd())
            return this.ReceiveACK_NACK(cmd);
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run 0x" + cmd.ToString("X2"));
        }
      }
      return flag;
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
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 14);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: ReadSystemTime");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadSystemTime");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadSystemTime");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 12 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
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
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 14);
      this.TransmitBuffer.Add(Byte);
      this.TransmitBuffer.Add(month);
      this.TransmitBuffer.Add(day);
      this.TransmitBuffer.Add(hour);
      this.TransmitBuffer.Add(minute);
      this.TransmitBuffer.Add(second);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Info("WriteSystemTime(" + value.ToString("g") + ")");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadSystemTime");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadSystemTime");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 12 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown EDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return (int) Byte == (int) this.ReceiveBuffer.Data[6] && (int) month == (int) this.ReceiveBuffer.Data[7] && (int) day == (int) this.ReceiveBuffer.Data[8] && (int) hour == (int) this.ReceiveBuffer.Data[9] && (int) minute == (int) this.ReceiveBuffer.Data[10];
        }
      }
      return false;
    }

    internal bool ReceiveACK_NACK(byte cmd)
    {
      ZR_ClassLibMessages.ClearErrors();
      if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
        throw new Exception("Unknown EDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
      if (this.ReceiveBuffer.Data[5] == byte.MaxValue)
        throw new Exception("Invalid command was send to EDC device! CMD: " + this.ReceiveBuffer.Data[6].ToString());
      if (this.ReceiveBuffer.Data[5] == (byte) 251 || this.ReceiveBuffer.Data[5] == (byte) 252 || this.ReceiveBuffer.Data[5] == (byte) 253 || this.ReceiveBuffer.Data[5] == (byte) 254)
        throw new Exception("Invalid responce was received! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
      if (this.ReceiveBuffer.Data[5] == (byte) 250 && (int) this.ReceiveBuffer.Data[6] == (int) cmd)
      {
        PDC.logger.Debug("... successful ");
        return true;
      }
      if ((int) this.ReceiveBuffer.Data[5] != ((int) cmd | 128))
        return false;
      PDC.logger.Debug("... successful ");
      return true;
    }

    public bool EraseFLASHSegment(ushort address)
    {
      BusDevice.CheckReadOnlyRight();
      PDC.logger.Info("Erase FLASH 0x" + address.ToString("X4"));
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
        if (!this.ExecuteSimpleCommand((byte) 10))
          throw new Exception("Can not start radio receiver!");
        if (!this.ReceiveACK_NACK((byte) 10))
          throw new Exception("Missing OK after start radio receiver!");
        DateTime now2 = DateTime.Now;
        while ((DateTime.Now - now2).TotalMilliseconds < (double) timeout && this.MyBus.MyCom.InputBufferLength < 19L)
        {
          if (this.MyBus.BreakRequest)
            throw new Exception("Function was canceled!");
          PDC.logger.Trace("Wait 100 ms");
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
          throw new Exception("Can not receive OK after successfully radio receive!");
        if (this.ReceiveBuffer.Data.Length < 7 || this.ReceiveBuffer.Data[6] > (byte) 0)
          throw new Exception("Failed to receive radio packet! Error number: " + this.ReceiveBuffer.Data[6].ToString());
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
      if (!this.ExecuteSimpleCommand((byte) 10))
        throw new Exception("Can not start radio receiver!");
      if (!this.ReceiveACK_NACK((byte) 10))
        throw new Exception("Missing OK after start radio receiver!");
      return true;
    }

    public bool EventLogClear()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand((byte) 3) && this.ReceiveACK_NACK((byte) 3);
    }

    public bool SystemLogClear()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand((byte) 5) && this.ReceiveACK_NACK((byte) 5);
    }

    public bool WriteMeterValue(byte channel, uint value)
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
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 33);
      this.TransmitBuffer.Add(channel);
      this.TransmitBuffer.Add(BitConverter.GetBytes(value));
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Info("WriteMeterValue(" + value.ToString() + ")");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteMeterValue");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteMeterValue");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown EDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return (long) BitConverter.ToInt32(this.ReceiveBuffer.Data, 7) == (long) value;
        }
      }
      return false;
    }

    public int? ReadMeterValue(byte channel)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.ReadMeterValue);
      if (!this.MyBus.MyCom.Open())
        return new int?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 33);
      this.TransmitBuffer.Add(channel);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: ReadMeterValue");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadMeterValue");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadMeterValue");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown EDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new int?(BitConverter.ToInt32(this.ReceiveBuffer.Data, 7));
        }
      }
      return new int?();
    }

    public bool DeliveryState()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand((byte) 26) && this.ReceiveACK_NACK((byte) 26);
    }

    public uint? ListQuery()
    {
      if (!this.MyBus.MyCom.Open())
        return new uint?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 21);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: ListQuery");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ListQuery");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ListQuery");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new uint?(BitConverter.ToUInt32(this.ReceiveBuffer.Data, 6));
        }
      }
      return new uint?();
    }

    public byte? ReadRadioList()
    {
      if (!this.MyBus.MyCom.Open())
        return new byte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 22);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: Read RadioList");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run Read RadioList");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run Read RadioList");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown PDC response detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new byte?(this.ReceiveBuffer.Data[6]);
        }
      }
      return new byte?();
    }

    public byte? WriteRadioList(byte list)
    {
      if (!this.MyBus.MyCom.Open())
        return new byte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 22);
      this.TransmitBuffer.Add(list);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: Read RadioList");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run Read RadioList");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run Read RadioList");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown PDC response detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new byte?(this.ReceiveBuffer.Data[6]);
        }
      }
      return new byte?();
    }

    public byte? ReadVIF(byte channel)
    {
      if (!this.MyBus.MyCom.Open())
        return new byte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 34);
      this.TransmitBuffer.Add(channel);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: ReadVIF");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadVIF");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadVIF");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new byte?(this.ReceiveBuffer.Data[7]);
        }
      }
      return new byte?();
    }

    public byte? WriteVIF(byte channel, byte code)
    {
      BusDevice.CheckReadOnlyRight();
      if (!this.MyBus.MyCom.Open())
        return new byte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 34);
      this.TransmitBuffer.Add(channel);
      this.TransmitBuffer.Add(code);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: WriteVIF");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteVIF");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteVIF");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new byte?(this.ReceiveBuffer.Data[7]);
        }
      }
      return new byte?();
    }

    public sbyte? ReadExponent(byte channel)
    {
      if (!this.MyBus.MyCom.Open())
        return new sbyte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 35);
      this.TransmitBuffer.Add(channel);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: ReadExponent");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadExponent");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadExponent");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new sbyte?((sbyte) this.ReceiveBuffer.Data[7]);
        }
      }
      return new sbyte?();
    }

    public sbyte? WriteExponent(byte channel, sbyte code)
    {
      BusDevice.CheckReadOnlyRight();
      if (!this.MyBus.MyCom.Open())
        return new sbyte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 35);
      this.TransmitBuffer.Add(channel);
      this.TransmitBuffer.Add(BitConverter.GetBytes((short) code));
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: WriteExponent");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteExponent");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteExponent");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new sbyte?((sbyte) this.ReceiveBuffer.Data[7]);
        }
      }
      return new sbyte?();
    }

    public ushort? ReadMantissa(byte channel)
    {
      if (!this.MyBus.MyCom.Open())
        return new ushort?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 36);
      this.TransmitBuffer.Add(channel);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: ReadMantissa");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadMantissa");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadMantissa");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new ushort?(BitConverter.ToUInt16(this.ReceiveBuffer.Data, 7));
        }
      }
      return new ushort?();
    }

    public ushort? WriteMantissa(byte channel, ushort code)
    {
      BusDevice.CheckReadOnlyRight();
      if (!this.MyBus.MyCom.Open())
        return new ushort?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 36);
      this.TransmitBuffer.Add(channel);
      this.TransmitBuffer.Add(BitConverter.GetBytes(code));
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: WriteMantissa");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteMantissa");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteMantissa");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new ushort?(BitConverter.ToUInt16(this.ReceiveBuffer.Data, 7));
        }
      }
      return new ushort?();
    }

    public ushort? ReadFlowBlock(byte channel)
    {
      if (!this.MyBus.MyCom.Open())
        return new ushort?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 37);
      this.TransmitBuffer.Add(channel);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: ReadFlowBlock");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadFlowBlock");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadFlowBlock");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new ushort?(BitConverter.ToUInt16(this.ReceiveBuffer.Data, 7));
        }
      }
      return new ushort?();
    }

    public ushort? WriteFlowBlock(byte channel, ushort code)
    {
      BusDevice.CheckReadOnlyRight();
      if (!this.MyBus.MyCom.Open())
        return new ushort?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 37);
      this.TransmitBuffer.Add(channel);
      this.TransmitBuffer.Add(BitConverter.GetBytes(code));
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: WriteFlowBlock");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteFlowBlock");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteFlowBlock");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new ushort?(BitConverter.ToUInt16(this.ReceiveBuffer.Data, 7));
        }
      }
      return new ushort?();
    }

    public ulong? ReadFlowLeak(byte channel)
    {
      if (!this.MyBus.MyCom.Open())
        return new ulong?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 38);
      this.TransmitBuffer.Add(channel);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: ReadFlowLeak");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadFlowLeak");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadFlowLeak");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new ulong?(BitConverter.ToUInt64(this.ReceiveBuffer.Data, 7));
        }
      }
      return new ulong?();
    }

    public ulong? WriteFlowLeak(
      byte channel,
      ushort leak,
      ushort unleak,
      ushort upper,
      ushort lower)
    {
      BusDevice.CheckReadOnlyRight();
      if (!this.MyBus.MyCom.Open())
        return new ulong?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 38);
      this.TransmitBuffer.Add(channel);
      this.TransmitBuffer.Add(BitConverter.GetBytes(leak));
      this.TransmitBuffer.Add(BitConverter.GetBytes(unleak));
      this.TransmitBuffer.Add(BitConverter.GetBytes(upper));
      this.TransmitBuffer.Add(BitConverter.GetBytes(lower));
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: WriteFlowLeak");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteFlowLeak");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteFlowLeak");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new ulong?(BitConverter.ToUInt64(this.ReceiveBuffer.Data, 7));
        }
      }
      return new ulong?();
    }

    public uint? ReadFlowBurst(byte channel)
    {
      if (!this.MyBus.MyCom.Open())
        return new uint?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 39);
      this.TransmitBuffer.Add(channel);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: ReadFlowBurst");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadFlowBurst");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadFlowBurst");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new uint?(BitConverter.ToUInt32(this.ReceiveBuffer.Data, 7));
        }
      }
      return new uint?();
    }

    public uint? WriteFlowBurst(byte channel, ushort diff, ushort limit)
    {
      BusDevice.CheckReadOnlyRight();
      if (!this.MyBus.MyCom.Open())
        return new uint?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 39);
      this.TransmitBuffer.Add(channel);
      this.TransmitBuffer.Add(BitConverter.GetBytes(diff));
      this.TransmitBuffer.Add(BitConverter.GetBytes(limit));
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: WriteFlowBurst");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteFlowBurst");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteFlowBurst");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new uint?(BitConverter.ToUInt32(this.ReceiveBuffer.Data, 7));
        }
      }
      return new uint?();
    }

    public uint? ReadFlowOversize(byte channel)
    {
      if (!this.MyBus.MyCom.Open())
        return new uint?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 40);
      this.TransmitBuffer.Add(channel);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: ReadFlowOversize");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadFlowOversize");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadFlowOversize");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new uint?(BitConverter.ToUInt32(this.ReceiveBuffer.Data, 7));
        }
      }
      return new uint?();
    }

    public uint? WriteFlowOversize(byte channel, ushort diff, ushort limit)
    {
      BusDevice.CheckReadOnlyRight();
      if (!this.MyBus.MyCom.Open())
        return new uint?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 40);
      this.TransmitBuffer.Add(channel);
      this.TransmitBuffer.Add(BitConverter.GetBytes(diff));
      this.TransmitBuffer.Add(BitConverter.GetBytes(limit));
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: WriteFlowOversize");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteFlowOversize");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteFlowOversize");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new uint?(BitConverter.ToUInt32(this.ReceiveBuffer.Data, 7));
        }
      }
      return new uint?();
    }

    public uint? ReadFlowUndersize(byte channel)
    {
      if (!this.MyBus.MyCom.Open())
        return new uint?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 41);
      this.TransmitBuffer.Add(channel);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: ReadFlowUndersize");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadFlowUndersize");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadFlowUndersize");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new uint?(BitConverter.ToUInt32(this.ReceiveBuffer.Data, 7));
        }
      }
      return new uint?();
    }

    public uint? WriteFlowUndersize(byte channel, ushort diff, ushort limit)
    {
      BusDevice.CheckReadOnlyRight();
      if (!this.MyBus.MyCom.Open())
        return new uint?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 41);
      this.TransmitBuffer.Add(channel);
      this.TransmitBuffer.Add(BitConverter.GetBytes(diff));
      this.TransmitBuffer.Add(BitConverter.GetBytes(limit));
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: WriteFlowUndersize");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteFlowUndersize");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteFlowUndersize");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new uint?(BitConverter.ToUInt32(this.ReceiveBuffer.Data, 7));
        }
      }
      return new uint?();
    }

    public ushort? ReadConfigFlags()
    {
      if (!this.MyBus.MyCom.Open())
        return new ushort?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 23);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: ReadConfigFlags");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadConfigFlags");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadConfigFlags");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new ushort?(BitConverter.ToUInt16(this.ReceiveBuffer.Data, 6));
        }
      }
      return new ushort?();
    }

    public ushort? WriteConfigFlags(ushort flags)
    {
      BusDevice.CheckReadOnlyRight();
      if (!this.MyBus.MyCom.Open())
        return new ushort?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 23);
      this.TransmitBuffer.Add((int) flags & (int) byte.MaxValue);
      this.TransmitBuffer.Add((int) flags >> 8 & (int) byte.MaxValue);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: WriteConfigFlags");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteConfigFlags");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteConfigFlags");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new ushort?(BitConverter.ToUInt16(this.ReceiveBuffer.Data, 6));
        }
      }
      return new ushort?();
    }

    public ushort? ModifyConfigFlags(ushort flagsSet, ushort flagsReset)
    {
      if (!this.MyBus.MyCom.Open())
        return new ushort?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 23);
      this.TransmitBuffer.Add((int) flagsSet & (int) byte.MaxValue);
      this.TransmitBuffer.Add((int) flagsSet >> 8 & (int) byte.MaxValue);
      this.TransmitBuffer.Add((int) flagsReset & (int) byte.MaxValue);
      this.TransmitBuffer.Add((int) flagsReset >> 8 & (int) byte.MaxValue);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: ModifyConfigFlags");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ModifyConfigFlags");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ModifyConfigFlags");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new ushort?(BitConverter.ToUInt16(this.ReceiveBuffer.Data, 6));
        }
      }
      return new ushort?();
    }

    public byte? ReadRadioFlags()
    {
      if (!this.MyBus.MyCom.Open())
        return new byte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 24);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: ReadRadioFlags");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadRadioFlags");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadRadioFlags");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new byte?(this.ReceiveBuffer.Data[6]);
        }
      }
      return new byte?();
    }

    public byte? WriteRadioFlags(byte flags)
    {
      BusDevice.CheckReadOnlyRight();
      if (!this.MyBus.MyCom.Open())
        return new byte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 24);
      this.TransmitBuffer.Add(flags);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: WriteRadioFlags");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteRadioFlags");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteRadioFlags");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new byte?(this.ReceiveBuffer.Data[6]);
        }
      }
      return new byte?();
    }

    public byte? ModifyRadioFlags(byte flagsSet, byte flagsReset)
    {
      if (!this.MyBus.MyCom.Open())
        return new byte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 24);
      this.TransmitBuffer.Add(flagsSet);
      this.TransmitBuffer.Add(flagsReset);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: ModifyRadioFlags");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ModifyRadioFlags");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ModifyRadioFlags");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new byte?(this.ReceiveBuffer.Data[6]);
        }
      }
      return new byte?();
    }

    public ushort? StatusFlagsRead(byte channel)
    {
      if (!this.MyBus.MyCom.Open())
        return new ushort?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 32);
      this.TransmitBuffer.Add(channel);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: StatusFlags");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run StatusFlags");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run StatusFlags");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new ushort?(BitConverter.ToUInt16(this.ReceiveBuffer.Data, 7));
        }
      }
      return new ushort?();
    }

    public ushort? StatusFlagsClear(byte channel, ushort flags)
    {
      BusDevice.CheckReadOnlyRight();
      if (!this.MyBus.MyCom.Open())
        return new ushort?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 32);
      this.TransmitBuffer.Add(channel);
      this.TransmitBuffer.Add(BitConverter.GetBytes(flags));
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: StatusFlags");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run StatusFlags");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run StatusFlags");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new ushort?(BitConverter.ToUInt16(this.ReceiveBuffer.Data, 7));
        }
      }
      return new ushort?();
    }

    public ushort? ReadKeyDate()
    {
      if (!this.MyBus.MyCom.Open())
        return new ushort?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 25);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: KeyDate");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run KeyDate");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run KeyDate");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new ushort?(BitConverter.ToUInt16(this.ReceiveBuffer.Data, 6));
        }
      }
      return new ushort?();
    }

    public ushort? WriteKeyDate(byte month, byte day)
    {
      BusDevice.CheckReadOnlyRight();
      if (!this.MyBus.MyCom.Open())
        return new ushort?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 25);
      this.TransmitBuffer.Add(month);
      this.TransmitBuffer.Add(day);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: WriteKeyDate");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteKeyDate");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteKeyDate");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new ushort?(BitConverter.ToUInt16(this.ReceiveBuffer.Data, 6));
        }
      }
      return new ushort?();
    }

    public uint? ReadSerial(byte channel)
    {
      if (!this.MyBus.MyCom.Open())
        return new uint?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 48);
      this.TransmitBuffer.Add(channel);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: ReadSerial");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadSerial");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadSerial");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new uint?(BitConverter.ToUInt32(this.ReceiveBuffer.Data, 7));
        }
      }
      return new uint?();
    }

    public uint? WriteSerial(byte channel, uint serial)
    {
      BusDevice.CheckReadOnlyRight();
      if (!this.MyBus.MyCom.Open())
        return new uint?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 48);
      this.TransmitBuffer.Add(channel);
      this.TransmitBuffer.Add((byte) (serial & (uint) byte.MaxValue));
      this.TransmitBuffer.Add((byte) (serial >> 8 & (uint) byte.MaxValue));
      this.TransmitBuffer.Add((byte) (serial >> 16 & (uint) byte.MaxValue));
      this.TransmitBuffer.Add((byte) (serial >> 24 & (uint) byte.MaxValue));
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: WriteSerial");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteSerial");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteSerial");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new uint?(BitConverter.ToUInt32(this.ReceiveBuffer.Data, 7));
        }
      }
      return new uint?();
    }

    public byte? ReadMBusAddress(byte channel)
    {
      if (!this.MyBus.MyCom.Open())
        return new byte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 49);
      this.TransmitBuffer.Add(channel);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: ReadMBusAddress");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadMBusAddress");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadMBusAddress");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new byte?(this.ReceiveBuffer.Data[7]);
        }
      }
      return new byte?();
    }

    public byte? WriteMBusAddress(byte channel, byte address)
    {
      BusDevice.CheckReadOnlyRight();
      if (!this.MyBus.MyCom.Open())
        return new byte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 49);
      this.TransmitBuffer.Add(channel);
      this.TransmitBuffer.Add(address);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: WriteMBusAddress");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteMBusAddress");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteMBusAddress");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new byte?(this.ReceiveBuffer.Data[7]);
        }
      }
      return new byte?();
    }

    public byte? ReadMBusVersion(byte channel)
    {
      if (!this.MyBus.MyCom.Open())
        return new byte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 50);
      this.TransmitBuffer.Add(channel);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: ReadMBusVersion");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadMBusVersion");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadMBusVersion");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new byte?(this.ReceiveBuffer.Data[7]);
        }
      }
      return new byte?();
    }

    public byte? WriteMBusVersion(byte channel, byte version)
    {
      BusDevice.CheckReadOnlyRight();
      if (!this.MyBus.MyCom.Open())
        return new byte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 50);
      this.TransmitBuffer.Add(channel);
      this.TransmitBuffer.Add(version);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: WriteMBusVersion");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteMBusVersion");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteMBusVersion");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new byte?(this.ReceiveBuffer.Data[7]);
        }
      }
      return new byte?();
    }

    public byte? ReadMBusMedium(byte channel)
    {
      if (!this.MyBus.MyCom.Open())
        return new byte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 51);
      this.TransmitBuffer.Add(channel);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: CommIDMBusMedium");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run CommIDMBusMedium");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run CommIDMBusMedium");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new byte?(this.ReceiveBuffer.Data[7]);
        }
      }
      return new byte?();
    }

    public byte? WriteMBusMedium(byte channel, byte medium)
    {
      BusDevice.CheckReadOnlyRight();
      if (!this.MyBus.MyCom.Open())
        return new byte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 51);
      this.TransmitBuffer.Add(channel);
      this.TransmitBuffer.Add(medium);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: WriteMBusMedium");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteMBusMedium");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteMBusMedium");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new byte?(this.ReceiveBuffer.Data[7]);
        }
      }
      return new byte?();
    }

    public ushort? ReadMBusManid(byte channel)
    {
      if (!this.MyBus.MyCom.Open())
        return new ushort?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 52);
      this.TransmitBuffer.Add(channel);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: ReadMBusManid");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadMBusManid");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadMBusManid");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new ushort?(BitConverter.ToUInt16(this.ReceiveBuffer.Data, 7));
        }
      }
      return new ushort?();
    }

    public ushort? WriteMBusManid(byte channel, ushort manid)
    {
      BusDevice.CheckReadOnlyRight();
      if (!this.MyBus.MyCom.Open())
        return new ushort?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 52);
      this.TransmitBuffer.Add(channel);
      this.TransmitBuffer.Add((int) manid & (int) byte.MaxValue);
      this.TransmitBuffer.Add((int) manid >> 8 & (int) byte.MaxValue);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: WriteMBusManid");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteMBusManid");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteMBusManid");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 10 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new ushort?(BitConverter.ToUInt16(this.ReceiveBuffer.Data, 7));
        }
      }
      return new ushort?();
    }

    public byte? ReadObisCode(byte channel)
    {
      if (!this.MyBus.MyCom.Open())
        return new byte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 53);
      this.TransmitBuffer.Add(channel);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: ReadObisCode");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadObisCode");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadObisCode");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128 || (int) this.ReceiveBuffer.Data[6] != (int) channel)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new byte?(this.ReceiveBuffer.Data[7]);
        }
      }
      return new byte?();
    }

    public byte? WriteObisCode(byte channel, byte obis)
    {
      BusDevice.CheckReadOnlyRight();
      if (!this.MyBus.MyCom.Open())
        return new byte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 53);
      this.TransmitBuffer.Add(channel);
      this.TransmitBuffer.Add(obis);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: WriteObisCode");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteObisCode");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteObisCode");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new byte?(this.ReceiveBuffer.Data[7]);
        }
      }
      return new byte?();
    }

    public bool Depassivate()
    {
      BusDevice.CheckReadOnlyRight();
      return this.ExecuteSimpleCommand((byte) 64) && this.ReceiveACK_NACK((byte) 64);
    }

    public uint? ReadDepass()
    {
      if (!this.MyBus.MyCom.Open())
        return new uint?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 65);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: ReadDepass");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadDepass");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadDepass");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new uint?(BitConverter.ToUInt32(this.ReceiveBuffer.Data, 6));
        }
      }
      return new uint?();
    }

    public uint? WriteDepass(ushort timeout, ushort period)
    {
      BusDevice.CheckReadOnlyRight();
      if (!this.MyBus.MyCom.Open())
        return new uint?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 65);
      this.TransmitBuffer.Add(BitConverter.GetBytes(timeout));
      this.TransmitBuffer.Add(BitConverter.GetBytes(period));
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: WriteDepass");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteDepass");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteDepass");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new uint?(BitConverter.ToUInt32(this.ReceiveBuffer.Data, 6));
        }
      }
      return new uint?();
    }

    public bool? MBusBreak(ushort count)
    {
      if (!this.MyBus.MyCom.Open())
        return new bool?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 66);
      this.TransmitBuffer.Add(BitConverter.GetBytes(count));
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      return new bool?(true);
    }

    public byte? ReadMBusStatus()
    {
      if (!this.MyBus.MyCom.Open())
        return new byte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 67);
      this.FinishLongFrame();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: ReadMBusStatus");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadMBusStatus");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run ReadMBusStatus");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new byte?(this.ReceiveBuffer.Data[6]);
        }
      }
      return new byte?();
    }

    public byte? WriteMBusStatus(byte state)
    {
      BusDevice.CheckReadOnlyRight();
      if (!this.MyBus.MyCom.Open())
        return new byte?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(65);
      this.TransmitBuffer.Add((byte) 67);
      this.TransmitBuffer.Add(state);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        PDC.logger.Debug("Send cmd: WriteMBusStatus");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteMBusStatus");
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            PDC.logger.Warn(" ... repeat run WriteMBusStatus");
        }
        else
        {
          if (this.ReceiveBuffer.Count < 8 || this.ReceiveBuffer.Data[0] != (byte) 15 || this.ReceiveBuffer.Data[1] != (byte) 0 || this.ReceiveBuffer.Data[2] != (byte) 0 || this.ReceiveBuffer.Data[3] != (byte) 0 || this.ReceiveBuffer.Data[4] != (byte) 65 || ((int) this.ReceiveBuffer.Data[5] & 128) != 128)
            throw new Exception("Unknown PDC responce detected! Buffer: " + Util.ByteArrayToHexString(this.ReceiveBuffer.Data));
          return new byte?(this.ReceiveBuffer.Data[6]);
        }
      }
      return new byte?();
    }
  }
}

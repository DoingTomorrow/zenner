// Decompiled with JetBrains decompiler
// Type: DeviceCollector.SmokeDetector
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using AsyncCom;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class SmokeDetector : MBusDevice
  {
    private static Logger logger = LogManager.GetLogger(nameof (SmokeDetector));
    private byte lastAccessNumber = 0;
    private uint lastSerialAsBCD = 0;

    public SmokeDetector(DeviceCollectorFunctions TheBus)
      : base(TheBus)
    {
      this.DeviceType = DeviceTypes.SmokeDetector;
    }

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

    public bool ReadVersion(
      out uint serialnumber,
      out string manufacturer,
      out byte generation,
      out byte medium,
      out byte status,
      out byte[] buffer)
    {
      SmokeDetector.logger.Info("Read version");
      serialnumber = 0U;
      manufacturer = (string) null;
      generation = (byte) 0;
      medium = (byte) 0;
      status = (byte) 0;
      buffer = (byte[]) null;
      if (!this.SND_UD((ushort) 0, (byte) 10, (byte) 6) || this.ReceiveBuffer.Count != 13)
        return false;
      byte[] dst = new byte[10];
      Buffer.BlockCopy((Array) this.ReceiveBuffer.Data, 1, (Array) dst, 0, dst.Length);
      serialnumber = Convert.ToUInt32(this.Info.MeterNumber);
      manufacturer = this.Info.Manufacturer;
      generation = this.Info.Version;
      medium = this.Info.Medium;
      status = this.Info.Status;
      buffer = dst;
      return true;
    }

    public byte[] ReadParameter(ushort address)
    {
      SmokeDetector.logger.Info("Read parameter at address: 0x" + address.ToString("X4"));
      if (!this.SND_UD(address, (byte) 29, (byte) 48) || this.ReceiveBuffer.Count <= 0)
        return (byte[]) null;
      byte[] dst = new byte[this.ReceiveBuffer.Count - 3];
      Buffer.BlockCopy((Array) this.ReceiveBuffer.Data, 1, (Array) dst, 0, dst.Length);
      return dst;
    }

    public byte[] ReadEventMemory(ushort address)
    {
      SmokeDetector.logger.Info("Read event memory at address: 0x" + address.ToString("X4"));
      if (!this.SND_UD(address, (byte) 166, (byte) 48) || this.ReceiveBuffer.Count <= 0)
        return (byte[]) null;
      byte[] dst = new byte[this.ReceiveBuffer.Count - 3];
      Buffer.BlockCopy((Array) this.ReceiveBuffer.Data, 1, (Array) dst, 0, dst.Length);
      return dst;
    }

    private bool SND_UD(ushort address, byte count, byte cmd)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      if (!this.MyBus.MyCom.Open())
        return false;
      byte[] bytes = BitConverter.GetBytes(address);
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(bytes[0]);
      this.TransmitBuffer.Add(bytes[1]);
      this.TransmitBuffer.Add(count);
      this.TransmitBuffer.Add(cmd);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            SmokeDetector.logger.Warn(" ... repeat run");
          this.MyBus.AsyncCom.ClearWakeup();
        }
        else if (!this.ReceiveLongframeEnd())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            SmokeDetector.logger.Warn(" ... repeat run");
        }
        else
        {
          this.lastAccessNumber = this.Info.AccessNb;
          this.lastSerialAsBCD = this.Info.MeterNumberOriginal;
          return true;
        }
      }
      return false;
    }

    public byte[] Read(int numberOfBytesToReceive)
    {
      byte[] buffer;
      return this.MyBus.MyCom.TryReceiveBlock(out buffer, numberOfBytesToReceive) ? buffer : (byte[]) null;
    }

    private byte[] Send(ushort address, byte cmd, byte[] buffer = null)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      if (!this.MyBus.MyCom.Open())
        return (byte[]) null;
      byte[] bytes = BitConverter.GetBytes(address);
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(bytes[0]);
      this.TransmitBuffer.Add(bytes[1]);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(cmd);
      if (buffer != null && buffer.Length != 0)
        this.TransmitBuffer.Add(buffer);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        ByteField DataBlock1 = new ByteField(4);
        if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock1, 4, true))
        {
          this.MyBus.MyCom.ClearWakeup();
          ++this.MyBus.BusState.TotalErrorCounter;
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            SmokeDetector.logger.Warn("Repeat to read M-Bus header! Reason: No response.");
        }
        else if (DataBlock1.Data[0] != (byte) 104 || DataBlock1.Data[3] != (byte) 104 || (int) DataBlock1.Data[1] != (int) DataBlock1.Data[2] || DataBlock1.Data[1] < (byte) 3)
        {
          ++this.MyBus.BusState.TotalErrorCounter;
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            SmokeDetector.logger.Warn("Repeat to read M-Bus header! Reason: Invalid M-Bus header.");
        }
        else
        {
          byte index1 = DataBlock1.Data[1];
          ByteField DataBlock2 = new ByteField((int) index1 + 2);
          if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock2, (int) index1 + 2, false))
          {
            ++this.MyBus.BusState.TotalErrorCounter;
            if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
              SmokeDetector.logger.Warn("Repeat to read M-Bus data!");
          }
          else
          {
            byte num1 = DataBlock2.Data[(int) index1];
            byte num2 = 0;
            for (int index2 = 0; index2 < (int) index1; ++index2)
              num2 += DataBlock2.Data[index2];
            if ((int) num1 != (int) num2)
            {
              ++this.MyBus.BusState.TotalErrorCounter;
              if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
                SmokeDetector.logger.Warn("Repeat to read M-Bus header! Reason: Invalid M-Bus checksum.");
            }
            else
            {
              byte[] dst = new byte[(int) index1 - 3];
              Buffer.BlockCopy((Array) DataBlock2.Data, 3, (Array) dst, 0, dst.Length);
              ZR_ClassLibMessages.ClearErrors();
              return dst;
            }
          }
        }
      }
      return (byte[]) null;
    }

    private ushort CalculatePSW()
    {
      return (ushort) ((int) (ushort) (this.lastSerialAsBCD >> 16) ^ (int) (ushort) this.lastSerialAsBCD ^ ((int) this.lastAccessNumber << 8) + (int) this.lastAccessNumber ^ new int[8]
      {
        18463,
        21893,
        3673,
        27814,
        18154,
        29022,
        9735,
        15054
      }[(int) this.lastAccessNumber & 7]);
    }

    public bool WriteDevice(int address, byte[] buffer)
    {
      BusDevice.CheckReadOnlyRight();
      if (buffer == null)
        throw new ArgumentNullException("The 'buffer' can not be null!");
      if (buffer.Length == 0)
        throw new ArgumentNullException("The length of the 'buffer' can not be 0!");
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.WriteMemoryBlock);
      if (!this.MyBus.MyCom.Open() || !this.ReadVersion(out uint _, out string _, out byte _, out byte _, out byte _, out byte[] _))
        return false;
      byte[] bytes1 = BitConverter.GetBytes(address);
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(bytes1[0]);
      this.TransmitBuffer.Add(bytes1[1]);
      this.TransmitBuffer.Add(buffer.Length);
      this.TransmitBuffer.Add(49);
      byte[] bytes2 = BitConverter.GetBytes(this.CalculatePSW());
      this.TransmitBuffer.Add(bytes2[0]);
      this.TransmitBuffer.Add(bytes2[1]);
      foreach (byte Byte in buffer)
        this.TransmitBuffer.Add(Byte);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        ByteField DataBlock = new ByteField(1);
        if (this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 1, true))
          return DataBlock != null && DataBlock.Count == 1 && DataBlock.Data[0] == (byte) 229;
        SmokeDetector.logger.Warn(" ... repeat run");
        this.MyBus.MyCom.ClearWakeup();
      }
      return false;
    }

    private bool ExecuteSimpleCommandAndCheck_E5(ushort address, byte cmd)
    {
      return this.ExecuteSimpleCommandAndCheck_E5(address, cmd, (byte[]) null);
    }

    private bool ExecuteSimpleCommandAndCheck_E5(ushort address, byte cmd, byte[] buffer)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      if (!this.MyBus.MyCom.Open())
        return false;
      byte[] bytes = BitConverter.GetBytes(address);
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(bytes[0]);
      this.TransmitBuffer.Add(bytes[1]);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(cmd);
      if (buffer != null && buffer.Length != 0)
        this.TransmitBuffer.Add(buffer);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (this.ReceiveOkNok())
          return true;
        if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
          SmokeDetector.logger.Warn("Repeat command!");
        this.MyBus.AsyncCom.ClearWakeup();
      }
      return false;
    }

    public bool TC_EnterTestMode()
    {
      BusDevice.CheckReadOnlyRight();
      SmokeDetector.logger.Info("(Test command) EnterTestMode");
      return this.ExecuteSimpleCommandAndCheck_E5((ushort) 192, (byte) 48);
    }

    public bool TC_ExitTestMode()
    {
      SmokeDetector.logger.Info("(Test command) ExitTestMode");
      return this.ExecuteSimpleCommandAndCheck_E5((ushort) 203, (byte) 48);
    }

    public bool TC_TransmitterLedInSmokeChamberVoltageReferenceTL431()
    {
      SmokeDetector.logger.Info("(Test command) TC_TransmitterLedInSmokeChamberVoltageReferenceTL431");
      return this.ExecuteSimpleCommandAndCheck_E5((ushort) 194, (byte) 48);
    }

    public bool TC_PiezoTestHighSoundPressure(byte duration = 0)
    {
      SmokeDetector.logger.Info("(Test command) TC_PiezoTestHighSoundPressure, Duration: " + duration.ToString());
      return duration != (byte) 1 ? this.ExecuteSimpleCommandAndCheck_E5(BitConverter.ToUInt16(new byte[2]
      {
        (byte) 195,
        duration
      }, 0), (byte) 48) : throw new Exception("Duration is not allowed. Please use TC_PiezoAdjustValueHighSound");
    }

    public byte TC_PiezoAdjustValueHighSound()
    {
      SmokeDetector.logger.Info("(Test command) TC_PiezoAdjustValueHighSound");
      return this.Send((ushort) 451, (byte) 48)[0];
    }

    public bool TC_PiezoTestLowSoundPressure()
    {
      SmokeDetector.logger.Info("(Test command) TC_PiezoTestLowSoundPressure");
      return this.ExecuteSimpleCommandAndCheck_E5((ushort) 196, (byte) 48);
    }

    public bool TC_SetDeliveryState()
    {
      SmokeDetector.logger.Info("(Test command) TC_SetDeliveryState");
      bool flag = this.ExecuteSimpleCommandAndCheck_E5(ushort.MaxValue, (byte) 50);
      if (flag)
      {
        Thread.Sleep(3000);
        this.MyBus.MyCom.ClearCom();
      }
      return flag;
    }

    public bool TC_ButtonFunction()
    {
      SmokeDetector.logger.Info("(Test command) TC_ButtonFunction");
      return this.ExecuteSimpleCommandAndCheck_E5((ushort) byte.MaxValue, (byte) 51);
    }

    public bool TC_ResetDevice()
    {
      SmokeDetector.logger.Info("(Test command) TC_ResetDevice");
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      if (!this.MyBus.MyCom.Open())
        return false;
      byte[] bytes = BitConverter.GetBytes((int) byte.MaxValue);
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(bytes[0]);
      this.TransmitBuffer.Add(bytes[1]);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(4);
      this.FinishLongFrame();
      bool flag = this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
      if (flag)
      {
        Thread.Sleep(3000);
        this.MyBus.MyCom.ClearCom();
      }
      return flag;
    }

    public SmokeDetector.HardwareState TC_Check_LED_Battery_Voltage_TemperatureSensor_Piezo()
    {
      SmokeDetector.logger.Info("(Test command) TC_Check_LED_Battery_Voltage_TemperatureSensor_Piezo");
      byte[] buffer = this.Send((ushort) 449, (byte) 48);
      return buffer == null ? (SmokeDetector.HardwareState) null : SmokeDetector.HardwareState.Parse(buffer);
    }

    public bool TC_ButtonTest()
    {
      SmokeDetector.logger.Info("(Test command) TC_ButtonTest");
      byte[] buffer = this.Send((ushort) 197, (byte) 48);
      if (buffer == null)
        return false;
      if (buffer == null)
        throw new ArgumentNullException("Can not parse! The buffer is null.");
      if (buffer.Length != 1)
        throw new ArgumentOutOfRangeException("Can not parse! Unknown size of buffer. Expected 1 byte. Buffer: " + Util.ByteArrayToHexString(buffer));
      if (buffer[0] != (byte) 0 && buffer[0] != (byte) 1)
        throw new ArgumentException("Can not parse! Invalid value of 'Button test' result. Buffer: " + Util.ByteArrayToHexString(buffer));
      return !Convert.ToBoolean(buffer[0]);
    }

    public SmokeDetector.EepromState? TC_EepromState()
    {
      SmokeDetector.logger.Info("(Test command) TC_EepromState");
      byte[] buffer = this.Send((ushort) 198, (byte) 48);
      if (buffer == null)
        return new SmokeDetector.EepromState?();
      if (buffer == null)
        throw new ArgumentNullException("Can not parse! The buffer is null.");
      if (buffer.Length != 1)
        throw new ArgumentOutOfRangeException("Can not parse! Unknown size of buffer. Expected 1 byte. Buffer: " + Util.ByteArrayToHexString(buffer));
      return buffer[0] <= (byte) 7 ? new SmokeDetector.EepromState?((SmokeDetector.EepromState) buffer[0]) : throw new ArgumentException("Can not parse! Invalid value of 'EEPROM test' result. Buffer: " + Util.ByteArrayToHexString(buffer));
    }

    public SmokeDetector.TestData TC_TestData()
    {
      SmokeDetector.logger.Info("(Test command) TC_TestData");
      byte[] buffer = this.Send((ushort) 200, (byte) 48);
      return buffer == null ? (SmokeDetector.TestData) null : SmokeDetector.TestData.Parse(buffer);
    }

    public SmokeDetector.ObstructionState TC_ObstructionCheck()
    {
      SmokeDetector.logger.Info("(Test command) TC_ObstructionCheck");
      byte[] buffer = this.Send((ushort) 204, (byte) 48);
      return buffer == null ? (SmokeDetector.ObstructionState) null : SmokeDetector.ObstructionState.Parse(buffer);
    }

    public bool TC_ObstructionCalibrationWrite(SmokeDetector.ObstructionState state)
    {
      SmokeDetector.logger.Info("TC_ObstructionCalibrationWrite: " + state?.ToString());
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 0);
      byteList.Add((byte) 0);
      byteList.Add((byte) 0);
      byteList.AddRange(state.ToByteArray());
      return this.ExecuteSimpleCommandAndCheck_E5((ushort) 204, (byte) 49, byteList.ToArray());
    }

    public SmokeDetector.ObstructionState TC_ObstructionCalibrationRead()
    {
      SmokeDetector.logger.Info(nameof (TC_ObstructionCalibrationRead));
      byte[] buffer = this.Send((ushort) 204, (byte) 49, new byte[3]
      {
        (byte) 0,
        (byte) 0,
        (byte) 1
      });
      return buffer == null ? (SmokeDetector.ObstructionState) null : SmokeDetector.ObstructionState.Parse(buffer);
    }

    public bool TC_SurroundingAreaMonitoringCheckTransmitter(SmokeDetector.Check led)
    {
      SmokeDetector.logger.Info("(Test command) TC_SurroundingAreaMonitoringCheckTransmitter(" + led.ToString() + ")");
      return this.ExecuteSimpleCommandAndCheck_E5((ushort) 205, (byte) 49, new byte[3]
      {
        (byte) 0,
        (byte) 0,
        (byte) led
      });
    }

    public bool TC_SurroundingAreaMonitoringCheckReceiver()
    {
      SmokeDetector.logger.Info("(Test command) TC_SurroundingAreaMonitoringCheckReceiver");
      return this.ExecuteSimpleCommandAndCheck_E5((ushort) 461, (byte) 48);
    }

    public byte? TC_SurroundingAreaMonitoringCheckReceiverTestResult()
    {
      SmokeDetector.logger.Info("(Test command) TC_SurroundingAreaMonitoringCheckReceiverTestResult");
      byte[] buffer = this.Send((ushort) 717, (byte) 48);
      if (buffer == null)
        return new byte?();
      if (buffer == null)
        throw new ArgumentNullException("Can not parse! The buffer is null.");
      return buffer.Length == 1 ? new byte?(buffer[0]) : throw new ArgumentOutOfRangeException("Can not parse! Unknown size of buffer. Expected 1 byte. Buffer: " + Util.ByteArrayToHexString(buffer));
    }

    public DateTime? TC_ClearTestRecordT1()
    {
      SmokeDetector.logger.Info("(Test command) TC_ClearTestRecordT1");
      byte[] buffer = this.Send((ushort) 202, (byte) 48);
      if (buffer == null)
        return new DateTime?();
      byte second = buffer.Length == 6 ? buffer[0] : throw new ArgumentOutOfRangeException("Can not parse! Unknown size of buffer. Expected 6 bytes. Buffer: " + Util.ByteArrayToHexString(buffer));
      byte minute = buffer[1];
      byte hour = buffer[2];
      byte day = buffer[3];
      byte month = buffer[4];
      byte num = buffer[5];
      try
      {
        return new DateTime?(new DateTime(2000 + (int) num, (int) month, (int) day, (int) hour, (int) minute, (int) second));
      }
      catch
      {
        return new DateTime?();
      }
    }

    public DateTime? TC_EraseEEPROM()
    {
      SmokeDetector.logger.Info("(Test command) TC_EraseEEPROM");
      int timeOffsetPerBlock = this.ReadTimeout_RecTime_OffsetPerBlock;
      if (timeOffsetPerBlock < 10000)
        this.ReadTimeout_RecTime_OffsetPerBlock = 10000;
      byte[] buffer = (byte[]) null;
      try
      {
        buffer = this.Send((ushort) 458, (byte) 48);
      }
      finally
      {
        this.ReadTimeout_RecTime_OffsetPerBlock = timeOffsetPerBlock;
      }
      if (buffer == null)
        return new DateTime?();
      byte second = buffer.Length == 6 ? buffer[0] : throw new ArgumentOutOfRangeException("Can not parse! Unknown size of buffer. Expected 6 bytes. Buffer: " + Util.ByteArrayToHexString(buffer));
      byte minute = buffer[1];
      byte hour = buffer[2];
      byte day = buffer[3];
      byte month = buffer[4];
      byte num = buffer[5];
      try
      {
        return new DateTime?(new DateTime(2000 + (int) num, (int) month, (int) day, (int) hour, (int) minute, (int) second));
      }
      catch
      {
        return new DateTime?();
      }
    }

    public bool? TC_CauseTestAlarm()
    {
      SmokeDetector.logger.Info("(Test command) TC_CauseTestAlarm");
      return new bool?(this.ExecuteSimpleCommandAndCheck_E5((ushort) 162, (byte) 48));
    }

    public SmokeDetector.SmokeDensityAndSensitivity TC_ReadSmokeDensityAndSensitivity()
    {
      SmokeDetector.logger.Info("(Test command) TC_ReadSmokeDensityAndSensitivity");
      byte[] buffer = this.Send((ushort) 193, (byte) 48);
      return buffer == null ? (SmokeDetector.SmokeDensityAndSensitivity) null : SmokeDetector.SmokeDensityAndSensitivity.Parse(buffer);
    }

    public ushort? TC_WriteSmokeDensityThreshold_C_Value(byte value)
    {
      SmokeDetector.logger.Info("(Test command) TC_WriteSmokeDensityThreshold_C_Value");
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Dummy);
      if (!this.MyBus.MyCom.Open())
        return new ushort?();
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(160);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(49);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(value);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        ByteField DataBlock = new ByteField(2);
        if (!this.MyBus.MyCom.ReceiveBlock(ref DataBlock, 2, true))
        {
          this.MyBus.MyCom.ClearWakeup();
          ++this.MyBus.BusState.TotalErrorCounter;
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            SmokeDetector.logger.Warn("Repeat to read C value! Reason: No response.");
        }
        else
        {
          ZR_ClassLibMessages.ClearErrors();
          this.MyBus.MyCom.ClearWakeup();
          return new ushort?(BitConverter.ToUInt16(new byte[2]
          {
            DataBlock.Data[1],
            DataBlock.Data[0]
          }, 0));
        }
      }
      return new ushort?();
    }

    public bool TC_ReadSmokeDensityAndSensitivity_90_times()
    {
      SmokeDetector.logger.Info("(Test command) TC_ReadSmokeDensityAndSensitivity_90_times()");
      return this.ExecuteSimpleCommandAndCheck_E5((ushort) 161, (byte) 48);
    }

    public byte[] TC_Set_A_B()
    {
      SmokeDetector.logger.Info("(Test command) TC_Set_A_B()");
      return this.Send((ushort) 161, (byte) 49);
    }

    public sealed class HardwareState
    {
      public ushort RedLED { get; set; }

      public ushort YellowLED { get; set; }

      public ushort BatteryVoltage { get; set; }

      public byte TemperatureSensor { get; set; }

      public bool HasPiezoError { get; set; }

      public static SmokeDetector.HardwareState Parse(byte[] buffer)
      {
        if (buffer == null)
          throw new ArgumentNullException("Can not parse! The buffer is null.");
        if (buffer.Length != 8)
          throw new ArgumentOutOfRangeException("Can not parse! Unknown size of buffer. Expected 8 bytes. Buffer: " + Util.ByteArrayToHexString(buffer));
        return buffer[7] == (byte) 0 || buffer[7] == (byte) 1 ? new SmokeDetector.HardwareState()
        {
          RedLED = BitConverter.ToUInt16(new byte[2]
          {
            buffer[1],
            buffer[0]
          }, 0),
          YellowLED = BitConverter.ToUInt16(new byte[2]
          {
            buffer[3],
            buffer[2]
          }, 0),
          BatteryVoltage = BitConverter.ToUInt16(new byte[2]
          {
            buffer[5],
            buffer[4]
          }, 0),
          TemperatureSensor = buffer[6],
          HasPiezoError = Convert.ToBoolean(buffer[7])
        } : throw new ArgumentException("Can not parse! Invalid value of piezo state. Buffer: " + Util.ByteArrayToHexString(buffer));
      }

      public override string ToString()
      {
        string newLine = Environment.NewLine;
        return string.Format("RedLED={0},{1}YellowLED={2},{3}BatteryVoltage={4},{5}TemperatureSensor={6},{7}HasPiezoError={8}", (object) this.RedLED, (object) newLine, (object) this.YellowLED, (object) newLine, (object) this.BatteryVoltage, (object) newLine, (object) this.TemperatureSensor, (object) newLine, (object) this.HasPiezoError);
      }
    }

    public enum EepromState : byte
    {
      NoError,
      HeadPartBroken,
      MiddlePartBroken,
      HeadAndMiddlePartBroken,
      EndPartBroken,
      HeadAndEndPartBroken,
      MiddleAndEndPartBroken,
      AllPartsBroken,
    }

    [Flags]
    public enum OPE : byte
    {
      None = 0,
      Radio = 1,
      ObstructionDetection = 2,
      SurroundingAreaMonitoring = 4,
      IrDa = 8,
      Piezo = 16, // 0x10
      RTC = 32, // 0x20
      EEPROM = 64, // 0x40
      Button = 128, // 0x80
    }

    public enum WinsoeCode : byte
    {
      M01041 = 1,
      M01042 = 2,
      M01043 = 3,
      M01048 = 4,
      M01049 = 5,
      M01050 = 6,
    }

    public sealed class SerialNumberInfo
    {
      public SmokeDetector.WinsoeCode WinsoeCode { get; set; }

      public int FactoryYear { get; set; }

      public int FactoryWeek { get; set; }

      public ushort SerialNumber { get; set; }

      internal static SmokeDetector.SerialNumberInfo Parse(uint serialNumber)
      {
        byte[] bytes = BitConverter.GetBytes(serialNumber);
        uint num = BitConverter.ToUInt32(bytes, 0) & 4095U;
        return new SmokeDetector.SerialNumberInfo()
        {
          WinsoeCode = (SmokeDetector.WinsoeCode) Enum.ToObject(typeof (SmokeDetector.WinsoeCode), (int) bytes[0] >> 4),
          FactoryYear = (int) (num / 100U) + 2000,
          FactoryWeek = (int) num - (int) (num / 100U) * 100,
          SerialNumber = BitConverter.ToUInt16(bytes, 2)
        };
      }

      public override string ToString()
      {
        StringBuilder stringBuilder = new StringBuilder();
        int totalWidth = 25;
        stringBuilder.Append("WinsoeCode: ".PadRight(totalWidth)).AppendLine(this.WinsoeCode.ToString());
        stringBuilder.Append("FactoryYear: ".PadRight(totalWidth)).AppendLine(this.FactoryYear.ToString());
        stringBuilder.Append("FactoryWeek: ".PadRight(totalWidth)).AppendLine(this.FactoryWeek.ToString());
        stringBuilder.Append("Serial number: ".PadRight(totalWidth)).AppendLine(this.SerialNumber.ToString());
        return stringBuilder.ToString();
      }
    }

    public sealed class SmokeDensityAndSensitivity
    {
      public ushort A { get; set; }

      public ushort B { get; set; }

      public ushort C { get; set; }

      public ushort? A_EEPROM { get; set; }

      public ushort? B_EEPROM { get; set; }

      public ushort? C_MEASURED { get; set; }

      public static SmokeDetector.SmokeDensityAndSensitivity Parse(byte[] buffer)
      {
        if (buffer == null)
          throw new ArgumentNullException("Can not parse! The buffer is null.");
        SmokeDetector.SmokeDensityAndSensitivity densityAndSensitivity = new SmokeDetector.SmokeDensityAndSensitivity();
        if (buffer.Length == 6)
        {
          densityAndSensitivity.C = BitConverter.ToUInt16(new byte[2]
          {
            buffer[1],
            buffer[0]
          }, 0);
          densityAndSensitivity.A = BitConverter.ToUInt16(new byte[2]
          {
            buffer[3],
            buffer[2]
          }, 0);
          densityAndSensitivity.B = BitConverter.ToUInt16(new byte[2]
          {
            buffer[5],
            buffer[4]
          }, 0);
        }
        else if (buffer.Length == 17)
        {
          densityAndSensitivity.C = BitConverter.ToUInt16(new byte[2]
          {
            buffer[8],
            buffer[7]
          }, 0);
          densityAndSensitivity.A = BitConverter.ToUInt16(new byte[2]
          {
            buffer[10],
            buffer[9]
          }, 0);
          densityAndSensitivity.B = BitConverter.ToUInt16(new byte[2]
          {
            buffer[12],
            buffer[11]
          }, 0);
        }
        else
        {
          densityAndSensitivity.C = buffer.Length == 21 ? BitConverter.ToUInt16(new byte[2]
          {
            buffer[8],
            buffer[7]
          }, 0) : throw new ArgumentOutOfRangeException("Can not parse! Unknown size of buffer. Buffer: " + Util.ByteArrayToHexString(buffer));
          densityAndSensitivity.A = BitConverter.ToUInt16(new byte[2]
          {
            buffer[10],
            buffer[9]
          }, 0);
          densityAndSensitivity.B = BitConverter.ToUInt16(new byte[2]
          {
            buffer[12],
            buffer[11]
          }, 0);
          densityAndSensitivity.C_MEASURED = new ushort?(BitConverter.ToUInt16(new byte[2]
          {
            buffer[14],
            buffer[13]
          }, 0));
          densityAndSensitivity.A_EEPROM = new ushort?(BitConverter.ToUInt16(new byte[2]
          {
            buffer[16],
            buffer[15]
          }, 0));
          densityAndSensitivity.B_EEPROM = new ushort?(BitConverter.ToUInt16(new byte[2]
          {
            buffer[18],
            buffer[17]
          }, 0));
        }
        return densityAndSensitivity;
      }

      public override string ToString()
      {
        StringBuilder stringBuilder = new StringBuilder();
        int totalWidth = 10;
        stringBuilder.Append("A: ".PadRight(totalWidth)).AppendLine(this.A.ToString());
        stringBuilder.Append("B: ".PadRight(totalWidth)).AppendLine(this.B.ToString());
        stringBuilder.Append("C: ".PadRight(totalWidth)).AppendLine(this.C.ToString());
        if (this.C_MEASURED.HasValue)
        {
          stringBuilder.Append("A_EEPROM: ".PadRight(totalWidth)).AppendLine(this.A_EEPROM.ToString());
          stringBuilder.Append("B_EEPROM: ".PadRight(totalWidth)).AppendLine(this.B_EEPROM.ToString());
          stringBuilder.Append("C_EEPROM: ".PadRight(totalWidth)).AppendLine(this.C_MEASURED.ToString());
        }
        return stringBuilder.ToString();
      }
    }

    public sealed class Smoke_A_B
    {
      public ushort A { get; set; }

      public ushort B { get; set; }

      public static SmokeDetector.Smoke_A_B Parse(byte[] buffer)
      {
        if (buffer == null)
          throw new ArgumentNullException("Can not parse! The buffer is null.");
        SmokeDetector.Smoke_A_B smokeAB = new SmokeDetector.Smoke_A_B();
        smokeAB.A = buffer.Length == 4 ? BitConverter.ToUInt16(new byte[2]
        {
          buffer[1],
          buffer[0]
        }, 0) : throw new ArgumentOutOfRangeException("Can not parse! Unknown size of buffer. Buffer: " + Util.ByteArrayToHexString(buffer));
        smokeAB.B = BitConverter.ToUInt16(new byte[2]
        {
          buffer[3],
          buffer[2]
        }, 0);
        return smokeAB;
      }

      public override string ToString()
      {
        StringBuilder stringBuilder = new StringBuilder();
        int totalWidth = 10;
        stringBuilder.Append("A: ".PadRight(totalWidth)).AppendLine(this.A.ToString());
        stringBuilder.Append("B: ".PadRight(totalWidth)).AppendLine(this.B.ToString());
        return stringBuilder.ToString();
      }
    }

    public sealed class TestData
    {
      public uint SerialNumber { get; set; }

      public SmokeDetector.SerialNumberInfo SerialNumberInfo
      {
        get => SmokeDetector.SerialNumberInfo.Parse(this.SerialNumber);
      }

      public byte A { get; set; }

      public byte B { get; set; }

      public byte C { get; set; }

      public ushort RedLED { get; set; }

      public ushort YellowLED { get; set; }

      public ushort BatteryVoltage { get; set; }

      public byte TemperatureSensor { get; set; }

      public byte CurrentConsumption { get; set; }

      public byte TP_1_VoltageOfTestPoint { get; set; }

      public byte TL431 { get; set; }

      public SmokeDetector.OPE OPE { get; set; }

      public byte TP_2_VoltageOfPiezo { get; set; }

      public byte? Reserved { get; set; }

      public byte? P_IrDA { get; set; }

      public byte? P_LED { get; set; }

      public byte? P_AMP { get; set; }

      public byte? P_POWER { get; set; }

      public byte? P_BATT { get; set; }

      public byte? Basis { get; set; }

      public byte? Radio { get; set; }

      public static SmokeDetector.TestData Parse(byte[] buffer)
      {
        if (buffer == null)
          throw new ArgumentNullException("Can not parse! The buffer is null.");
        if (buffer.Length < 19)
          throw new ArgumentOutOfRangeException("Can not parse! Unknown size of buffer. Buffer: " + Util.ByteArrayToHexString(buffer));
        SmokeDetector.TestData testData = new SmokeDetector.TestData();
        testData.SerialNumber = BitConverter.ToUInt32(buffer, 0);
        testData.A = buffer[4];
        testData.B = buffer[5];
        testData.C = buffer[6];
        testData.RedLED = BitConverter.ToUInt16(buffer, 7);
        testData.YellowLED = BitConverter.ToUInt16(buffer, 9);
        testData.BatteryVoltage = BitConverter.ToUInt16(buffer, 11);
        testData.TemperatureSensor = buffer[13];
        testData.CurrentConsumption = buffer[14];
        testData.TP_1_VoltageOfTestPoint = buffer[15];
        testData.TL431 = buffer[16];
        testData.OPE = (SmokeDetector.OPE) buffer[17];
        testData.TP_2_VoltageOfPiezo = buffer[18];
        if (buffer.Length == 20)
        {
          testData.Reserved = new byte?(buffer[19]);
        }
        else
        {
          testData.P_IrDA = buffer.Length == 26 ? new byte?(buffer[19]) : throw new ArgumentOutOfRangeException("Can not parse! Unknown size of buffer. Buffer: " + Util.ByteArrayToHexString(buffer));
          testData.P_LED = new byte?(buffer[20]);
          testData.P_AMP = new byte?(buffer[21]);
          testData.P_POWER = new byte?(buffer[22]);
          testData.P_BATT = new byte?(buffer[23]);
          testData.Basis = new byte?(buffer[24]);
          testData.Radio = new byte?(buffer[25]);
        }
        return testData;
      }

      public override string ToString()
      {
        StringBuilder stringBuilder = new StringBuilder();
        int totalWidth = 25;
        stringBuilder.Append("Serial number (raw): ".PadRight(totalWidth)).AppendLine(this.SerialNumber.ToString());
        stringBuilder.Append(this.SerialNumberInfo.ToString());
        stringBuilder.Append("A: ".PadRight(totalWidth)).AppendLine(this.A.ToString());
        stringBuilder.Append("B: ".PadRight(totalWidth)).AppendLine(this.B.ToString());
        stringBuilder.Append("C: ".PadRight(totalWidth)).AppendLine(this.C.ToString());
        stringBuilder.Append("RedLED: ".PadRight(totalWidth)).AppendLine(this.RedLED.ToString());
        stringBuilder.Append("YellowLED: ".PadRight(totalWidth)).AppendLine(this.YellowLED.ToString());
        stringBuilder.Append("BatteryVoltage: ".PadRight(totalWidth)).AppendLine(this.BatteryVoltage.ToString());
        stringBuilder.Append("TemperatureSensor: ".PadRight(totalWidth)).AppendLine(this.TemperatureSensor.ToString());
        stringBuilder.Append("CurrentConsumption: ".PadRight(totalWidth)).AppendLine(this.CurrentConsumption.ToString());
        stringBuilder.Append("TP_1_VoltageOfTestPoint: ".PadRight(totalWidth)).AppendLine(this.TP_1_VoltageOfTestPoint.ToString());
        stringBuilder.Append("TL431: ".PadRight(totalWidth)).AppendLine(this.TL431.ToString());
        stringBuilder.Append("OPE: ".PadRight(totalWidth)).AppendLine(this.OPE.ToString());
        stringBuilder.Append("TP_2_VoltageOfPiezo: ".PadRight(totalWidth)).AppendLine(this.TP_2_VoltageOfPiezo.ToString());
        if (this.Reserved.HasValue)
          stringBuilder.Append("Reserved: ".PadRight(totalWidth)).AppendLine(this.Reserved.ToString());
        if (this.P_IrDA.HasValue)
          stringBuilder.Append("P_IrDA : ".PadRight(totalWidth)).AppendLine(this.P_IrDA.ToString());
        if (this.P_LED.HasValue)
          stringBuilder.Append("P_LED  : ".PadRight(totalWidth)).AppendLine(this.P_LED.ToString());
        if (this.P_AMP.HasValue)
          stringBuilder.Append("P_AMP  : ".PadRight(totalWidth)).AppendLine(this.P_AMP.ToString());
        if (this.P_POWER.HasValue)
          stringBuilder.Append("P_POWER: ".PadRight(totalWidth)).AppendLine(this.P_POWER.ToString());
        if (this.P_BATT.HasValue)
          stringBuilder.Append("P_BATT : ".PadRight(totalWidth)).AppendLine(this.P_BATT.ToString());
        if (this.Basis.HasValue)
          stringBuilder.Append("Basis  : ".PadRight(totalWidth)).AppendLine(this.Basis.ToString());
        if (this.Radio.HasValue)
          stringBuilder.Append("Radio  : ".PadRight(totalWidth)).AppendLine(this.Radio.ToString());
        return stringBuilder.ToString();
      }
    }

    public sealed class ObstructionState
    {
      public ushort Near1 { get; set; }

      public ushort Near2 { get; set; }

      public ushort Near3 { get; set; }

      public ushort Near4 { get; set; }

      public ushort Near5 { get; set; }

      public ushort Near6 { get; set; }

      public static SmokeDetector.ObstructionState Parse(byte[] buffer)
      {
        if (buffer == null)
          throw new ArgumentNullException("Can not parse! The buffer is null.");
        if (buffer.Length != 8)
          throw new ArgumentOutOfRangeException("Can not parse! Unknown size of buffer. Expected 8 bytes. Buffer: " + Util.ByteArrayToHexString(buffer));
        SmokeDetector.ObstructionState obstructionState = new SmokeDetector.ObstructionState();
        obstructionState.Near1 = (ushort) buffer[0];
        obstructionState.Near2 = (ushort) buffer[1];
        obstructionState.Near3 = (ushort) buffer[2];
        obstructionState.Near4 = (ushort) buffer[3];
        obstructionState.Near5 = (ushort) buffer[4];
        obstructionState.Near6 = (ushort) buffer[5];
        byte num1 = buffer[6];
        byte num2 = buffer[7];
        obstructionState.Near1 |= (ushort) (((int) num1 & 3) << 8);
        obstructionState.Near2 |= (ushort) (((int) num1 & 12) << 6);
        obstructionState.Near3 |= (ushort) (((int) num1 & 48) << 4);
        obstructionState.Near4 |= (ushort) (((int) num1 & 192) << 2);
        obstructionState.Near5 |= (ushort) (((int) num2 & 3) << 8);
        obstructionState.Near6 |= (ushort) (((int) num2 & 12) << 6);
        return obstructionState;
      }

      public override string ToString()
      {
        StringBuilder stringBuilder = new StringBuilder();
        int totalWidth = 8;
        stringBuilder.Append("Near1: ".PadRight(totalWidth)).AppendLine(this.Near1.ToString());
        stringBuilder.Append("Near2: ".PadRight(totalWidth)).AppendLine(this.Near2.ToString());
        stringBuilder.Append("Near3: ".PadRight(totalWidth)).AppendLine(this.Near3.ToString());
        stringBuilder.Append("Near4: ".PadRight(totalWidth)).AppendLine(this.Near4.ToString());
        stringBuilder.Append("Near5: ".PadRight(totalWidth)).AppendLine(this.Near5.ToString());
        stringBuilder.Append("Near6: ".PadRight(totalWidth)).AppendLine(this.Near6.ToString());
        return stringBuilder.ToString();
      }

      internal IEnumerable<byte> ToByteArray()
      {
        return (IEnumerable<byte>) new byte[8]
        {
          (byte) this.Near1,
          (byte) this.Near2,
          (byte) this.Near3,
          (byte) this.Near4,
          (byte) this.Near5,
          (byte) this.Near6,
          (byte) ((int) this.Near1 >> 8 & 3 | ((int) this.Near2 >> 8 & 3) << 2 | ((int) this.Near3 >> 8 & 3) << 4 | ((int) this.Near4 >> 8 & 3) << 6),
          (byte) ((int) this.Near5 >> 8 & 3 | ((int) this.Near6 >> 8 & 3) << 2)
        };
      }
    }

    public enum Check : byte
    {
      LED1_EAST = 1,
      LED2_WEST = 2,
      LED3_NORTH = 3,
      LED4_SOUTH = 4,
      LED5_MIDDLE = 5,
      LED6_EWSN = 6,
    }
  }
}

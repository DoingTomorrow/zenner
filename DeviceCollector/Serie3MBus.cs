// Decompiled with JetBrains decompiler
// Type: DeviceCollector.Serie3MBus
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using AsyncCom;
using HandlerLib;
using System;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class Serie3MBus : Serie2MBus
  {
    public Serie3MBus(DeviceCollectorFunctions TheBus)
      : base(TheBus)
    {
      this.BaseConstructor();
    }

    public Serie3MBus(MBusDevice TheMBusDevice)
      : base(TheMBusDevice)
    {
      this.BaseConstructor();
    }

    private void BaseConstructor() => this.DeviceType = DeviceTypes.ZR_Serie3;

    internal override bool ResetDevice() => this.ResetDevice(false);

    internal override bool ReadVersion(out ReadVersionData versionData)
    {
      versionData = new ReadVersionData();
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Serie3Command);
      string str = this.MyBus.MyCom.SingleParameter(CommParameter.Baudrate, "");
      if (!this.MyBus.MyCom.Open())
        return false;
      this.MyBus.BreakRequest = false;
      this.MaxWriteBlockSize = 16;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(6);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        if (this.MyBus.BusState.RepeadCounter > 1)
          Serie2MBus.Serie2_3_Logger.Warn("Repeat read version");
        else
          Serie2MBus.Serie2_3_Logger.Debug("Read version");
        if (this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer))
        {
          this.MyBus.BusState.IncrementTransmitBlockCounter();
          if (!this.ReceiveHeader() || !this.ReceiveLongframeEnd())
          {
            Serie2MBus.Serie2_3_Logger.Trace("Read version NOK");
          }
          else
          {
            Serie2MBus.Serie2_3_Logger.Trace("Read version OK");
            versionData.MBusManufacturer = this.Info.ManufacturerCode;
            versionData.MBusMedium = this.Info.Medium;
            versionData.MBusGeneration = this.Info.Version;
            versionData.MBusSerialNr = uint.Parse(this.Info.MeterNumber);
            versionData.PacketSizeOfResponceByGetVersionCommand = new int?(this.ReceiveBuffer.Count);
            if (this.ReceiveBuffer.Count == 5)
            {
              versionData.Version = (uint) this.ReceiveBuffer.Data[1] << 16;
              versionData.Version += (uint) this.ReceiveBuffer.Data[2] << 24;
            }
            else if (this.ReceiveBuffer.Count >= 7 && this.ReceiveBuffer.Count < 22)
            {
              uint num = (uint) this.ReceiveBuffer.Data[1] + ((uint) this.ReceiveBuffer.Data[2] << 8) + ((uint) this.ReceiveBuffer.Data[3] << 16) + ((uint) this.ReceiveBuffer.Data[4] << 24);
              versionData.Version = num;
              if (num >= 17104897U)
                this.MaxWriteBlockSize = 32;
              if (this.ReceiveBuffer.Count == 11 || this.ReceiveBuffer.Count == 13 || this.ReceiveBuffer.Count == 21)
              {
                if (str != "115200")
                  this.MaxWriteBlockSize = 16;
                else
                  this.MaxWriteBlockSize = 150;
                if (this.ReceiveBuffer.Count == 11 || this.ReceiveBuffer.Count == 13)
                {
                  versionData.BuildRevision = (uint) this.ReceiveBuffer.Data[5];
                  versionData.BuildRevision += (uint) this.ReceiveBuffer.Data[6] << 8;
                  versionData.BuildRevision += (uint) this.ReceiveBuffer.Data[7] << 16;
                  versionData.BuildRevision += (uint) this.ReceiveBuffer.Data[8] << 24;
                  if (this.ReceiveBuffer.Count == 13)
                  {
                    versionData.HardwareIdentification = (uint) this.ReceiveBuffer.Data[9];
                    versionData.HardwareIdentification += (uint) this.ReceiveBuffer.Data[10] << 8;
                  }
                }
                else if (this.ReceiveBuffer.Count == 21)
                {
                  versionData.HardwareIdentification = (uint) this.ReceiveBuffer.Data[5];
                  versionData.HardwareIdentification += (uint) this.ReceiveBuffer.Data[6] << 8;
                  versionData.HardwareIdentification += (uint) this.ReceiveBuffer.Data[7] << 16;
                  versionData.HardwareIdentification += (uint) this.ReceiveBuffer.Data[8] << 24;
                  versionData.BuildRevision = (uint) this.ReceiveBuffer.Data[9];
                  versionData.BuildRevision += (uint) this.ReceiveBuffer.Data[10] << 8;
                  versionData.BuildRevision += (uint) this.ReceiveBuffer.Data[11] << 16;
                  versionData.BuildRevision += (uint) this.ReceiveBuffer.Data[12] << 24;
                  DateTime? mbusDateTime = MBusDifVif.GetMBusDateTime(this.ReceiveBuffer.Data, 13);
                  if (mbusDateTime.HasValue)
                    versionData.BuildTime = mbusDateTime.Value;
                  versionData.FirmwareSignature = (ushort) this.ReceiveBuffer.Data[17];
                  versionData.FirmwareSignature += (ushort) ((uint) this.ReceiveBuffer.Data[18] << 8);
                }
              }
            }
            else
            {
              if (this.ReceiveBuffer.Count != 22)
                return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Read version with wrong number of bytes.", Serie2MBus.Serie2_3_Logger);
              uint num = (uint) this.ReceiveBuffer.Data[2] + ((uint) this.ReceiveBuffer.Data[3] << 8) + ((uint) this.ReceiveBuffer.Data[4] << 16) + ((uint) this.ReceiveBuffer.Data[5] << 24);
              versionData.Version = num;
              versionData.HardwareIdentification = (uint) this.ReceiveBuffer.Data[6];
              versionData.HardwareIdentification += (uint) this.ReceiveBuffer.Data[7] << 8;
              versionData.HardwareIdentification += (uint) this.ReceiveBuffer.Data[8] << 16;
              versionData.HardwareIdentification += (uint) this.ReceiveBuffer.Data[9] << 24;
              versionData.BuildRevision = (uint) this.ReceiveBuffer.Data[10];
              versionData.BuildRevision += (uint) this.ReceiveBuffer.Data[11] << 8;
              versionData.BuildRevision += (uint) this.ReceiveBuffer.Data[12] << 16;
              versionData.BuildRevision += (uint) this.ReceiveBuffer.Data[13] << 24;
              DateTime? mbusDateTime = MBusDifVif.GetMBusDateTime(this.ReceiveBuffer.Data, 14);
              if (mbusDateTime.HasValue)
                versionData.BuildTime = mbusDateTime.Value;
              versionData.FirmwareSignature = (ushort) this.ReceiveBuffer.Data[18];
              versionData.FirmwareSignature += (ushort) ((uint) this.ReceiveBuffer.Data[19] << 8);
            }
            ZR_ClassLibMessages.ClearErrorText();
            return true;
          }
        }
      }
      return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read version error.", Serie2MBus.Serie2_3_Logger);
    }

    internal override bool ResetDevice(bool loadBackup)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.ResetDevice);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.MyBus.MyCom.SetAnswerOffsetTime(500);
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      if (loadBackup)
      {
        this.TransmitBuffer.Add(1);
        this.TransmitBuffer.Add(4);
        this.TransmitBuffer.Add(0);
      }
      else
      {
        this.TransmitBuffer.Add(0);
        this.TransmitBuffer.Add(4);
      }
      this.FinishLongFrame();
      bool flag = false;
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        Serie2MBus.Serie2_3_Logger.Info("Send ResetDevice");
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusDeviceReset);
        if (!this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer))
          return false;
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        this.MyBus.MyCom.ClearWakeup();
        if (!this.ReceiveOkNok())
        {
          if (Serie2MBus.Serie2_3_Logger.IsWarnEnabled && this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            Serie2MBus.Serie2_3_Logger.Warn("--> ResetDevice error. Repeat command!");
        }
        else
        {
          flag = true;
          break;
        }
      }
      if (flag)
      {
        this.IsSelectedOnBus = false;
        Serie2MBus.Serie2_3_Logger.Debug("--> ResetDevice done");
      }
      else
        Serie2MBus.Serie2_3_Logger.Error("--> ResetDevice error");
      this.MyBus.MyCom.SetAnswerOffsetTime(0);
      return flag;
    }

    internal override byte[] RunIoTest(IoTestFunctions theFunction)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Serie3Command);
      if (!this.MyBus.MyCom.Open())
        return (byte[]) null;
      if (theFunction == IoTestFunctions.IoTest_Run)
        this.MyBus.MyCom.SetAnswerOffsetTime(500);
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add((byte) theFunction);
      this.TransmitBuffer.Add(21);
      this.FinishLongFrame();
      bool flag = false;
      byte[] numArray = new byte[1];
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        Serie2MBus.Serie2_3_Logger.Info("Send IoTest" + theFunction.ToString());
        if (!this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer))
        {
          flag = false;
          break;
        }
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (theFunction == IoTestFunctions.IoTest_Run)
        {
          if (!this.ReceiveHeader())
          {
            Serie2MBus.Serie2_3_Logger.Debug("--> !!! Receive header error !!!");
            continue;
          }
          if (!this.ReceiveLongframeEnd())
          {
            Serie2MBus.Serie2_3_Logger.Debug("--> !!! Receive longframe end error !!!");
            continue;
          }
          if (this.ReceiveBuffer.Count != 7)
          {
            Serie2MBus.Serie2_3_Logger.Debug("--> !!! Wrong block size received !!!");
            continue;
          }
          numArray = new byte[4];
          for (int index = 0; index < 4; ++index)
            numArray[index] = this.ReceiveBuffer.Data[index + 1];
        }
        else if (!this.ReceiveOkNok())
        {
          if (Serie2MBus.Serie2_3_Logger.IsWarnEnabled && this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
          {
            Serie2MBus.Serie2_3_Logger.Warn("--> Send IoTest: ACK not received. Repeat command!");
            continue;
          }
          continue;
        }
        flag = true;
        break;
      }
      this.MyBus.MyCom.SetAnswerOffsetTime(0);
      if (flag)
      {
        Serie2MBus.Serie2_3_Logger.Debug("--> Read Ok");
        return numArray;
      }
      Serie2MBus.Serie2_3_Logger.Error("--> Read error");
      return (byte[]) null;
    }

    internal override bool EraseFlash(int StartAddress, int NumberOfBytes)
    {
      int num1 = NumberOfBytes / 128;
      if ((StartAddress & 63) != 0)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal erase flash start address");
        return false;
      }
      if ((NumberOfBytes & 63) != 0 || num1 > (int) byte.MaxValue)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal number of bytes by erase flash");
        return false;
      }
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Serie3Command);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add((byte) (StartAddress & (int) byte.MaxValue));
      this.TransmitBuffer.Add((byte) (StartAddress >> 8 & (int) byte.MaxValue));
      this.TransmitBuffer.Add((byte) num1);
      this.TransmitBuffer.Add(14);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusStartWriteBlock);
        Serie2MBus.Serie2_3_Logger.Debug("Erase flash");
        if (Serie2MBus.Serie2_3_Logger.IsInfoEnabled)
        {
          int num2 = StartAddress + num1 * 128 - 1;
          Serie2MBus.Serie2_3_Logger.Debug("Send EraseFlash. Adr.from-to: 0x" + StartAddress.ToString("x04") + " - 0x" + num2.ToString("x04"));
        }
        if (!this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer))
          return false;
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveOkNok())
        {
          if (this.MyBus.BusState.RepeadCounter < this.MyBus.MaxRequestRepeat)
            Serie2MBus.Serie2_3_Logger.Warn("--> Erase flash error. Repeat command!");
        }
        else
        {
          Serie2MBus.Serie2_3_Logger.Debug("--> Erase flash done");
          return true;
        }
      }
      Serie2MBus.Serie2_3_Logger.Error("--> Erase flash error");
      return false;
    }

    internal override bool ReadMemory()
    {
      this.Location = 1;
      return base.ReadMemory();
    }

    internal override bool WriteMemory() => this.CheckMemoryLocation() && base.WriteMemory();

    private bool CheckMemoryLocation()
    {
      if (this.Location == 1 || this.Location == 3)
        return true;
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal memory location");
      return false;
    }

    internal override bool SelectParameterList(int ListNumber, int function)
    {
      return this.SelectParameterListWork(ListNumber, function);
    }

    internal override ParameterListInfo ReadParameterList()
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Serie3Command);
      if (!this.MyBus.MyCom.Open())
        return (ParameterListInfo) null;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(37);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        Serie2MBus.Serie2_3_Logger.Debug("Read ParameterList Information");
        if (!this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer))
          return (ParameterListInfo) null;
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
          Serie2MBus.Serie2_3_Logger.Debug("--> !!! Receive header error !!!");
        else if (!this.ReceiveLongframeEnd())
        {
          Serie2MBus.Serie2_3_Logger.Debug("--> !!! Receive longframe end error !!!");
        }
        else
        {
          Serie2MBus.Serie2_3_Logger.Debug("--> Read Ok");
          return ParameterListInfo.Parse(this.TotalReceiveBuffer.ToArray());
        }
      }
      return (ParameterListInfo) null;
    }

    internal ImpulseInputCounters ReadInputCounters()
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Serie3Command);
      if (!this.MyBus.MyCom.Open())
        return (ImpulseInputCounters) null;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(41);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        Serie2MBus.Serie2_3_Logger.Debug(nameof (ReadInputCounters));
        if (!this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer))
          return (ImpulseInputCounters) null;
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
          Serie2MBus.Serie2_3_Logger.Debug("--> !!! Receive header error !!!");
        else if (!this.ReceiveLongframeEnd())
        {
          Serie2MBus.Serie2_3_Logger.Debug("--> !!! Receive longframe end error !!!");
        }
        else
        {
          Serie2MBus.Serie2_3_Logger.Debug("--> Read Ok");
          if (this.ReceiveBuffer.Count != 14)
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal number of bytes by ReadInputCounters");
            return (ImpulseInputCounters) null;
          }
          return new ImpulseInputCounters()
          {
            ImputState = ((WR4_VOL_INPUT_STATE) this.ReceiveBuffer.Data[1]).ToString(),
            HardwareCounter = BitConverter.ToUInt16(this.ReceiveBuffer.Data, 2),
            VolumePulseCounter = BitConverter.ToInt16(this.ReceiveBuffer.Data, 4),
            Input0Counter = BitConverter.ToUInt16(this.ReceiveBuffer.Data, 6),
            Input1Counter = BitConverter.ToUInt16(this.ReceiveBuffer.Data, 8),
            Input2Counter = BitConverter.ToUInt16(this.ReceiveBuffer.Data, 10)
          };
        }
      }
      return (ImpulseInputCounters) null;
    }

    internal override bool SetOptoTimeoutSeconds(int Seconds)
    {
      uint OptionByte = (uint) (Seconds / 16 + 3);
      if (OptionByte < 4U)
        OptionByte = 0U;
      if (OptionByte > (uint) byte.MaxValue)
        OptionByte = (uint) byte.MaxValue;
      bool flag = this.S3Command((byte) 13, (byte) OptionByte, "Send opto timeout seconds");
      if (flag && OptionByte > 4U)
        this.MyBus.MyCom.WakeupTemporaryOff = true;
      else
        this.MyBus.MyCom.ClearWakeup();
      return flag;
    }

    internal override bool FlyingTestActivate()
    {
      return this.S3Command((byte) 17, "Send Flying test activate");
    }

    internal override bool FlyingTestStart()
    {
      return this.S3CommandNoAnswer((byte) 18, "Send Flying test start");
    }

    internal override bool FlyingTestStop()
    {
      return this.S3CommandNoAnswer((byte) 19, "Send Flying test stop");
    }

    internal override bool FlyingTestReadVolume(out float volume, out MBusDeviceState state)
    {
      volume = 0.0f;
      state = MBusDeviceState.AnyError;
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Serie3Command);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(20);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        Serie2MBus.Serie2_3_Logger.Debug("Send Flying test read volume");
        if (!this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer))
          return false;
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveHeader())
          Serie2MBus.Serie2_3_Logger.Debug("--> !!! Receive header error !!!");
        else if (!this.ReceiveLongframeEnd())
          Serie2MBus.Serie2_3_Logger.Debug("--> !!! Receive longframe end error !!!");
        else if (this.ReceiveBuffer.Count != 6)
        {
          Serie2MBus.Serie2_3_Logger.Debug("--> !!! Wron block size received !!!");
        }
        else
        {
          state = (MBusDeviceState) this.Info.Status;
          volume = BitConverter.ToSingle(this.ReceiveBuffer.Data, 0);
          Serie2MBus.Serie2_3_Logger.Debug("--> Read Ok");
          return true;
        }
      }
      return false;
    }

    internal override bool AdcTestActivate() => this.S3Command((byte) 16, "Send ADC test activate");

    internal override bool CapacityOfTestActivate()
    {
      return this.S3Command((byte) 38, "Send capacity off test activate");
    }

    internal override bool RadioTest(RadioTestMode testMode)
    {
      return this.S3Command((byte) testMode, "Set radio test mode: " + testMode.ToString());
    }

    internal override bool Start512HzRtcCalibration()
    {
      return this.S3Command((byte) 36, "STart RTC 512Hz calibration mode");
    }

    internal override bool AdcTestCycleWithSimulatedVolume(float simulationVolume)
    {
      byte[] bytes = BitConverter.GetBytes(simulationVolume);
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Serie3Command);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(4);
      this.TransmitBuffer.Add(11);
      for (int index = 0; index < bytes.Length; ++index)
        this.TransmitBuffer.Add(bytes[index]);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        Serie2MBus.Serie2_3_Logger.Debug("Send simmulated volume + ADC cycle");
        if (!this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer))
          return false;
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveOkNok())
        {
          Serie2MBus.Serie2_3_Logger.Debug("--> !!! ACK not received !!!");
        }
        else
        {
          Serie2MBus.Serie2_3_Logger.Debug("--> ACK received");
          return true;
        }
      }
      return false;
    }

    internal override bool TestDone(long dispValueId)
    {
      switch (dispValueId)
      {
        case -1:
          return this.S3Command((byte) 15, (byte) 3, "Send test done. LCD: Sleep");
        case 272769346:
          return this.S3Command((byte) 15, (byte) 0, "Send test done. LCD: HeadEnergy");
        case 272769355:
          return this.S3Command((byte) 15, (byte) 1, "Send test done. LCD: CoolingEnergy");
        default:
          return this.S3Command((byte) 15, (byte) 2, "Send test done. LCD: SegmentTest");
      }
    }

    internal bool S3Command(byte CommandByte, string NlogString)
    {
      return this.S3Command(CommandByte, (byte) 0, NlogString);
    }

    internal bool S3Command(byte CommandByte, byte OptionByte, string NlogString)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Serie3Command);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(OptionByte);
      this.TransmitBuffer.Add(CommandByte);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        Serie2MBus.Serie2_3_Logger.Debug(NlogString);
        if (!this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer))
          return false;
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveOkNok())
        {
          Serie2MBus.Serie2_3_Logger.Debug("--> !!! ACK not received !!!");
        }
        else
        {
          Serie2MBus.Serie2_3_Logger.Debug("--> ACK received");
          return true;
        }
      }
      return false;
    }

    internal bool S3CommandNoAnswer(byte CommandByte, string NlogString)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Serie3Command);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(CommandByte);
      this.FinishLongFrame();
      Serie2MBus.Serie2_3_Logger.Debug(NlogString);
      if (!this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer))
        return false;
      this.MyBus.MyCom.ResetEarliestTransmitTime();
      return true;
    }

    internal override bool GetMeterMonitorData(out ByteField Buffer)
    {
      Buffer = new ByteField(504);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Serie3Command);
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(5);
      this.FinishLongFrame();
      this.MyBus.MyCom.SetAnswerOffsetTime(2000);
      bool meterMonitorData = false;
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        Serie2MBus.Serie2_3_Logger.Debug("Send read volume monitor data");
        if (!this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer))
          return false;
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.MyBus.MyCom.ReceiveBlock(ref Buffer, 504, true))
        {
          ZR_ClassLibMessages.LastErrors lastError = ZR_ClassLibMessages.GetLastError();
          Serie2MBus.Serie2_3_Logger.Error("Failed receive volume monitor data: " + lastError.ToString());
          if (MBusDevice.MBusDeviceLogger.IsTraceEnabled)
          {
            MBusDevice.MBusDeviceLogger.Trace("Received data: {0}", Util.ByteArrayToHexString(Buffer.Data, 0, Buffer.Count));
            MBusDevice.MBusDeviceLogger.Trace("Received size: {0}", Buffer.Count);
            MBusDevice.MBusDeviceLogger.Trace("Required size: {0}", 504);
          }
          this.MyBus.MyCom.ClearWakeup();
          ++this.MyBus.BusState.TotalErrorCounter;
        }
        else
        {
          meterMonitorData = true;
          break;
        }
      }
      this.MyBus.MyCom.SetAnswerOffsetTime(0);
      if (meterMonitorData)
      {
        Serie2MBus.Serie2_3_Logger.Debug("--> Read Ok");
        this.MyBus.MyCom.ResetEarliestTransmitTime();
      }
      else
        Serie2MBus.Serie2_3_Logger.Error("--> Read volume monitor data error");
      return meterMonitorData;
    }

    internal override bool DeviceProtectionGet()
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Serie3Command);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add((byte) 0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(7);
      this.FinishLongFrame();
      Serie2MBus.Serie2_3_Logger.Debug("Get device protection");
      if (!this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer))
        return false;
      if (!this.ReceiveOkNok())
      {
        this.MyBus.MyCom.ClearWakeup();
        return false;
      }
      Serie2MBus.Serie2_3_Logger.Debug("--> ACK received");
      return true;
    }

    internal override bool DeviceProtectionSet()
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Serie3Command);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add((byte) 1);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(7);
      this.FinishLongFrame();
      Serie2MBus.Serie2_3_Logger.Debug("Set device protection");
      if (!this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer))
        return false;
      if (!this.ReceiveOkNok())
      {
        this.MyBus.MyCom.ClearWakeup();
        return false;
      }
      Serie2MBus.Serie2_3_Logger.Debug("--> ACK received");
      return true;
    }

    internal override bool DeviceProtectionReset(uint meterKey)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Serie3Command);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add((byte) 2);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(4);
      this.TransmitBuffer.Add(7);
      for (int index = 0; index < 4; ++index)
      {
        this.TransmitBuffer.Add((byte) meterKey);
        meterKey >>= 8;
      }
      this.FinishLongFrame();
      Serie2MBus.Serie2_3_Logger.Debug("Reset device protection");
      if (!this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer))
        return false;
      if (!this.ReceiveOkNok())
      {
        this.MyBus.MyCom.ClearWakeup();
        return false;
      }
      Serie2MBus.Serie2_3_Logger.Debug("--> ACK received");
      return true;
    }

    internal override bool DeviceProtectionSetKey(uint meterKey)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.Serie3Command);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add((byte) 3);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(4);
      this.TransmitBuffer.Add(7);
      for (int index = 0; index < 4; ++index)
      {
        this.TransmitBuffer.Add((byte) meterKey);
        meterKey >>= 8;
      }
      this.FinishLongFrame();
      Serie2MBus.Serie2_3_Logger.Debug("Set protection key");
      if (!this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer))
        return false;
      if (!this.ReceiveOkNok())
      {
        this.MyBus.MyCom.ClearWakeup();
        return false;
      }
      Serie2MBus.Serie2_3_Logger.Debug("--> ACK received");
      return true;
    }

    private enum DeviceProtectionCommands
    {
      IsProtected,
      SetProtection,
      ResetProtection,
      SetKey,
    }
  }
}

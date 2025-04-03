// Decompiled with JetBrains decompiler
// Type: DeviceCollector.Serie2MBus
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using AsyncCom;
using NLog;
using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class Serie2MBus : MBusDevice
  {
    protected static Logger Serie2_3_Logger = LogManager.GetLogger("Serie2_3_MBus");
    internal long DeviceVersion;
    internal int HardwareTypeId;
    internal int HardwareMask;
    internal int SubversionRevision;
    internal DateTime BuildTime;
    public const int ZrMaxReadBlockSize = 200;
    internal const int MaxWriteBlockSizeLow = 16;
    internal const long HighBlockSizeVersion = 17104897;
    internal const int MaxWriteBlockSizeHigh = 32;
    internal const int MaxWriteBlockSizeSerie3 = 150;
    internal const int MaxWriteBlockSizeSerie3LowBaud = 16;
    internal const long Baud38400Version = 17104897;
    public static int[] AllBaudrates = new int[5]
    {
      2400,
      4800,
      9600,
      38400,
      300
    };
    public static int[] All_C2_Baudrates = new int[2]
    {
      2400,
      4800
    };
    public static int[] All_WR3_Baudrates = new int[4]
    {
      2400,
      9600,
      38400,
      300
    };
    internal int MaxWriteBlockSize;
    internal int Location = 0;
    internal int StartAddress = 0;
    internal int NumberOfBytes = 0;
    internal byte AndMask = 0;
    internal byte OrMask = 0;
    internal ByteField DataBuffer;
    internal ByteField OldDataBuffer;

    public Serie2MBus(DeviceCollectorFunctions TheBus)
      : base(TheBus)
    {
      this.BaseConstructor();
    }

    public Serie2MBus(MBusDevice TheMBusDevice)
      : base(TheMBusDevice.MyBus)
    {
      this.Info = TheMBusDevice.Info;
      this.PrimaryAddressKnown = TheMBusDevice.PrimaryAddressKnown;
      this.PrimaryAddressOk = TheMBusDevice.PrimaryAddressOk;
      this.PrimaryDeviceAddress = TheMBusDevice.PrimaryDeviceAddress;
      this.IsSelectedOnBus = TheMBusDevice.IsSelectedOnBus;
      this.BaseConstructor();
    }

    private void BaseConstructor()
    {
      this.DeviceType = DeviceTypes.ZR_Serie2;
      this.MaxWriteBlockSize = 16;
    }

    internal override bool SelectParameterList(int ListNumber, int function) => false;

    internal bool ReadVersion(int[] Baudrates)
    {
      this.MyBus.BreakRequest = false;
      this.HardwareTypeId = 0;
      this.HardwareMask = 0;
      string[] strArray;
      if (Baudrates == null)
      {
        strArray = new string[1]
        {
          this.MyBus.MyCom.SingleParameter(CommParameter.Baudrate, "")
        };
      }
      else
      {
        strArray = new string[Baudrates.Length];
        for (int index = 0; index < Baudrates.Length; ++index)
          strArray[index] = Baudrates[index].ToString();
      }
      this.MaxWriteBlockSize = 16;
      int index1 = 0;
      string ParameterValue = this.MyBus.MyCom.SingleParameter(CommParameter.Baudrate, "");
      for (int index2 = 0; index2 < strArray.Length; ++index2)
      {
        if (strArray[index2] == ParameterValue)
        {
          index1 = index2;
          break;
        }
      }
      ByteField data = new ByteField(3);
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(6);
      this.FinishLongFrame();
      if (!this.MyBus.MyCom.Open())
        return false;
      int num1 = this.MyBus.MaxRequestRepeat * strArray.Length;
      int num2 = 1;
      bool flag = true;
      while (num1-- > 0 && !this.MyBus.BreakRequest)
      {
        if (!flag)
          Serie2MBus.Serie2_3_Logger.Warn("Repeate read version");
        else
          flag = false;
        string str1 = "Read version! Try:" + num2.ToString() + "  Baud:";
        Serie2MBus.Serie2_3_Logger.Debug(str1);
        this.MyBus.SendMessage(str1, int.Parse(strArray[index1]), GMM_EventArgs.MessageType.StandardMessage);
        data.Count = 0;
        string str2 = strArray[index1].PadLeft(6, '0');
        data.Add(byte.Parse(str2.Substring(0, 2), NumberStyles.HexNumber));
        data.Add(byte.Parse(str2.Substring(2, 2), NumberStyles.HexNumber));
        data.Add(byte.Parse(str2.Substring(4, 2), NumberStyles.HexNumber));
        this.MyBus.MyCom.ComWriteLoggerData(EventLogger.LoggerEvent.BusSendREQ_Version, ref data);
        ++this.MyBus.BusState.TransmitBlockCounter;
        if (this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer))
        {
          if (!this.ReceiveHeader() || !this.ReceiveLongframeEnd())
          {
            Serie2MBus.Serie2_3_Logger.Trace("Read version NOK");
            ++index1;
            if (index1 >= strArray.Length)
            {
              index1 = 0;
              ++num2;
            }
            this.MyBus.MyCom.SingleParameter(CommParameter.Baudrate, strArray[index1]);
          }
          else
          {
            Serie2MBus.Serie2_3_Logger.Trace("Read version OK");
            if (this.ReceiveBuffer.Count == 5)
            {
              this.DeviceVersion = (long) this.ReceiveBuffer.Data[1] << 16;
              this.DeviceVersion += (long) this.ReceiveBuffer.Data[2] << 24;
            }
            else if (this.ReceiveBuffer.Count >= 7)
            {
              this.DeviceVersion = (long) this.ReceiveBuffer.Data[1];
              this.DeviceVersion += (long) this.ReceiveBuffer.Data[2] << 8;
              this.DeviceVersion += (long) this.ReceiveBuffer.Data[3] << 16;
              this.DeviceVersion += (long) this.ReceiveBuffer.Data[4] << 24;
              if (this.DeviceVersion >= 17104897L)
                this.MaxWriteBlockSize = 32;
              if (this.ReceiveBuffer.Count == 11 || this.ReceiveBuffer.Count == 13)
              {
                this.HardwareTypeId = (int) this.ReceiveBuffer.Data[5];
                this.HardwareTypeId += (int) this.ReceiveBuffer.Data[6] << 8;
                this.HardwareTypeId += (int) this.ReceiveBuffer.Data[7] << 16;
                this.HardwareTypeId += (int) this.ReceiveBuffer.Data[8] << 24;
                this.MaxWriteBlockSize = !(str2 != "115200") ? 150 : 16;
                if (this.ReceiveBuffer.Count == 13)
                {
                  this.HardwareMask = (int) this.ReceiveBuffer.Data[9];
                  this.HardwareMask += (int) this.ReceiveBuffer.Data[10] << 8;
                }
              }
              if (this.UseMaxBaudrate)
              {
                string str3 = this.DeviceVersion < 17104897L ? "4800" : "38400";
                if (strArray[index1] != str3)
                {
                  Serie2MBus.Serie2_3_Logger.Debug("Read version send change baudrate");
                  if (this.SetBaudrate(int.Parse(str3)))
                    this.MyBus.MyCom.SingleParameter(CommParameter.Baudrate, str3);
                  else
                    break;
                }
              }
            }
            else
              continue;
            ZR_ClassLibMessages.ClearErrorText();
            return true;
          }
        }
      }
      this.MyBus.MyCom.SingleParameter(CommParameter.Baudrate, ParameterValue);
      return false;
    }

    internal override bool ResetDevice(int AfterResetBaudrate)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.ResetDeviceBaudChange);
      string str = this.MyBus.MyCom.SingleParameter(CommParameter.Baudrate, "");
      string ParameterValue = AfterResetBaudrate.ToString();
      if (str == ParameterValue)
        return this.ResetDevice();
      if (!this.MyBus.MyCom.Open())
        return false;
      this.MyBus.MyCom.SetAnswerOffsetTime(5000);
      bool flag = false;
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        this.GenerateSendDataHeader();
        this.TransmitBuffer.Add(15);
        this.TransmitBuffer.Add(0);
        this.TransmitBuffer.Add(0);
        this.TransmitBuffer.Add(0);
        this.TransmitBuffer.Add(4);
        this.FinishLongFrame();
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusDeviceReset);
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        Thread.Sleep(80);
        this.MyBus.MyCom.SingleParameter(CommParameter.Baudrate, ParameterValue);
        if (!this.ReceiveOkNok())
        {
          if (this.ReadVersion((int[]) null))
          {
            if (this.MyBus.MyCom.SingleParameter(CommParameter.Baudrate, "") == ParameterValue)
            {
              flag = true;
              break;
            }
          }
          else
            break;
        }
        else
        {
          flag = true;
          ZR_ClassLibMessages.ClearErrorText();
          break;
        }
      }
      if (this.MyBus.BusState.RepeadCounter == 1)
        Thread.Sleep(500);
      this.MyBus.MyCom.SetAnswerOffsetTime(0);
      return flag;
    }

    internal override bool ResetDevice()
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.ResetDevice);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.MyBus.MyCom.SetAnswerOffsetTime(5000);
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(4);
      this.FinishLongFrame();
      bool flag = false;
      if (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusDeviceReset);
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveOkNok())
        {
          this.MyBus.MyCom.SetAnswerOffsetTime(0);
          if (!this.ReadVersion((int[]) null))
            goto label_6;
        }
        flag = true;
        ZR_ClassLibMessages.ClearErrorText();
      }
label_6:
      if (this.MyBus.BusState.RepeadCounter == 1)
        Thread.Sleep(500);
      this.MyBus.MyCom.SetAnswerOffsetTime(0);
      return flag;
    }

    internal bool RunBackup()
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.RunBackup);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.MyBus.MyCom.SetAnswerOffsetTime(1000);
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(8);
      this.FinishLongFrame();
      bool flag = false;
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        Serie2MBus.Serie2_3_Logger.Debug("Send run backup");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveOkNok())
        {
          Serie2MBus.Serie2_3_Logger.Warn(" ... repeat run backup ");
        }
        else
        {
          Serie2MBus.Serie2_3_Logger.Debug("... run backup done");
          flag = true;
          ZR_ClassLibMessages.ClearErrorText();
          break;
        }
      }
      this.MyBus.MyCom.SetAnswerOffsetTime(0);
      Application.DoEvents();
      Thread.Sleep(100);
      Application.DoEvents();
      Thread.Sleep(100);
      return flag;
    }

    internal bool SetEmergencyMode()
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.SetEmergencyMode);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(10);
      this.FinishLongFrame();
      bool flag = false;
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        Serie2MBus.Serie2_3_Logger.Info("Send set emergency mode");
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveOkNok())
        {
          Serie2MBus.Serie2_3_Logger.Warn("--> Repeat: Set emergency mode");
        }
        else
        {
          flag = true;
          ZR_ClassLibMessages.ClearErrorText();
          break;
        }
      }
      if (flag)
        Serie2MBus.Serie2_3_Logger.Info("--> Set emergency mode done");
      else
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Set emergency mode error", Serie2MBus.Serie2_3_Logger);
      this.MyBus.MyCom.SetAnswerOffsetTime(0);
      return flag;
    }

    internal bool DeleteMeterKey(int MeterKey)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.DeleteMeterKey);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(7);
      this.TransmitBuffer.Add(MeterKey & (int) byte.MaxValue);
      this.TransmitBuffer.Add(MeterKey >> 8 & (int) byte.MaxValue);
      this.TransmitBuffer.Add(MeterKey >> 16 & (int) byte.MaxValue);
      this.TransmitBuffer.Add(MeterKey >> 24 & (int) byte.MaxValue);
      this.FinishLongFrame();
      bool flag = false;
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (this.ReceiveOkNok())
        {
          flag = true;
          ZR_ClassLibMessages.ClearErrorText();
          break;
        }
      }
      this.MyBus.MyCom.SetAnswerOffsetTime(0);
      return flag;
    }

    internal virtual bool ReadMemory()
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
        this.MyBus.SendMessage(startAddress, GMM_EventArgs.MessageType.PrimaryAddressMessage);
        if (this.MyBus.BreakRequest)
        {
          this.MyBus.SendMessage(startAddress, GMM_EventArgs.MessageType.EndMessage);
          return false;
        }
        if (!this.ReadMemoryBlock(startAddress, BlockSize))
        {
          this.MyBus.SendMessage(startAddress, GMM_EventArgs.MessageType.EndMessage);
          return false;
        }
        for (int index = 1; index < this.ReceiveBuffer.Count - 2; ++index)
          this.DataBuffer.Add(this.ReceiveBuffer.Data[index]);
        startAddress += BlockSize;
      }
      while (startAddress - this.StartAddress < this.NumberOfBytes);
      this.MyBus.SendMessage(startAddress, GMM_EventArgs.MessageType.EndMessage);
      return true;
    }

    private bool ReadMemoryBlock(int BlockStartAddress, int BlockSize)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.ReadMemoryBlock);
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add((byte) (BlockStartAddress & (int) byte.MaxValue));
      this.TransmitBuffer.Add((byte) (BlockStartAddress >> 8 & (int) byte.MaxValue));
      this.TransmitBuffer.Add((byte) BlockSize);
      if (this.Location == 0)
        this.TransmitBuffer.Add(0);
      else
        this.TransmitBuffer.Add(2);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        if (Serie2MBus.Serie2_3_Logger.IsDebugEnabled)
          Serie2MBus.Serie2_3_Logger.Debug("Read memory 0x" + BlockStartAddress.ToString("X4") + " Size: " + BlockSize.ToString());
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (this.ReceiveHeader() && this.ReceiveLongframeEnd())
        {
          if (this.ReceiveBuffer.Count - 3 != BlockSize)
          {
            Serie2MBus.Serie2_3_Logger.Error("--> !!! Wrong block size received !!!");
          }
          else
          {
            ZR_ClassLibMessages.ClearErrorText();
            return true;
          }
        }
      }
      return ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read memory block error", Serie2MBus.Serie2_3_Logger);
    }

    internal bool UpdateMemory()
    {
      if (!this.MyBus.MyCom.Open())
        return false;
      int num = this.StartAddress;
      int DataStartOffset = 0;
      this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusStartUpdateMemory);
      while (true)
      {
        while (DataStartOffset >= this.DataBuffer.Count || (int) this.DataBuffer.Data[DataStartOffset] != (int) this.OldDataBuffer.Data[DataStartOffset])
        {
          if (DataStartOffset < this.DataBuffer.Count)
          {
            int BlockSize = this.DataBuffer.Count - DataStartOffset;
            if (BlockSize > this.MaxWriteBlockSize)
              BlockSize = this.MaxWriteBlockSize;
            for (; BlockSize > 1; --BlockSize)
            {
              int index = DataStartOffset + BlockSize - 1;
              if ((int) this.DataBuffer.Data[index] != (int) this.OldDataBuffer.Data[index])
                break;
            }
            num = this.StartAddress + DataStartOffset;
            this.MyBus.SendMessage(num, GMM_EventArgs.MessageType.PrimaryAddressMessage);
            if (this.MyBus.BreakRequest)
            {
              this.MyBus.SendMessage(num, GMM_EventArgs.MessageType.EndMessage);
              return false;
            }
            if (!this.WriteMemoryBlock(num, BlockSize, ref DataStartOffset))
            {
              this.MyBus.SendMessage(num, GMM_EventArgs.MessageType.EndMessage);
              return false;
            }
            if (DataStartOffset < this.DataBuffer.Count)
              continue;
          }
          this.MyBus.SendMessage(num, GMM_EventArgs.MessageType.EndMessage);
          ZR_ClassLibMessages.ClearErrorText();
          return true;
        }
        ++DataStartOffset;
      }
    }

    internal virtual bool WriteMemory()
    {
      if (!this.MyBus.MyCom.Open())
        return false;
      int startAddress = this.StartAddress;
      int DataStartOffset = 0;
      DeviceCollectorFunctions.logger.Trace("Write memory. Address: 0x" + this.StartAddress.ToString("x04"));
      this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusStartWriteMemory);
      do
      {
        int BlockSize = this.DataBuffer.Count - (startAddress - this.StartAddress);
        if (BlockSize > this.MaxWriteBlockSize)
          BlockSize = this.MaxWriteBlockSize;
        this.MyBus.SendMessage(startAddress, GMM_EventArgs.MessageType.PrimaryAddressMessage);
        if (this.MyBus.BreakRequest)
        {
          this.MyBus.SendMessage(startAddress, GMM_EventArgs.MessageType.EndMessage);
          return false;
        }
        if (!this.WriteMemoryBlock(startAddress, BlockSize, ref DataStartOffset))
        {
          this.MyBus.SendMessage(startAddress, GMM_EventArgs.MessageType.EndMessage);
          return false;
        }
        startAddress += BlockSize;
        Serie2MBus.Serie2_3_Logger.Debug("write block at " + startAddress.ToString("x4") + " - bytes " + BlockSize.ToString("x4"));
      }
      while (startAddress - this.StartAddress < this.DataBuffer.Count);
      this.MyBus.SendMessage(startAddress, GMM_EventArgs.MessageType.EndMessage);
      ZR_ClassLibMessages.ClearErrorText();
      return true;
    }

    private bool WriteMemoryBlock(int BlockStartAddress, int BlockSize, ref int DataStartOffset)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.WriteMemoryBlock);
      if (this.WatchNumberOfBytes > 0)
      {
        bool flag = false;
        if (this.WatchMemoryLocation == (MemoryLocation) this.Location)
        {
          int num1 = this.WatchStartAddress + this.WatchNumberOfBytes - 1;
          int num2 = BlockStartAddress + BlockSize - 1;
          if (this.WatchStartAddress >= BlockStartAddress)
          {
            if (num2 >= this.WatchStartAddress)
              flag = true;
          }
          else if (num1 >= BlockStartAddress)
            flag = true;
          if (flag)
          {
            byte[] data = new byte[BlockSize];
            for (int index = 0; index < BlockSize; ++index)
              data[index] = this.DataBuffer.Data[DataStartOffset + index];
            this.MyBus.MemoryWriteWatch(BlockStartAddress, ref data);
          }
        }
      }
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add((byte) (BlockStartAddress & (int) byte.MaxValue));
      this.TransmitBuffer.Add((byte) (BlockStartAddress >> 8 & (int) byte.MaxValue));
      this.TransmitBuffer.Add((byte) BlockSize);
      if (this.Location == 0)
        this.TransmitBuffer.Add(1);
      else if (this.Location == 1)
        this.TransmitBuffer.Add(3);
      else if (this.Location == 3)
      {
        this.TransmitBuffer.Add(1);
      }
      else
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal memory location");
        return false;
      }
      for (int index = 0; index < BlockSize; ++index)
        this.TransmitBuffer.Add(this.DataBuffer.Data[DataStartOffset++]);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        if (Serie2MBus.Serie2_3_Logger.IsDebugEnabled)
        {
          Serie2MBus.Serie2_3_Logger.Debug("Write memory block. Address: 0x" + BlockStartAddress.ToString("x04") + " Bytes: 0x" + BlockSize.ToString("x04"));
          Serie2MBus.Serie2_3_Logger.Debug("--> " + BitConverter.ToString(this.TransmitBuffer.GetByteArray()));
        }
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (!this.ReceiveOkNok())
        {
          Serie2MBus.Serie2_3_Logger.Debug("--> Write memory error");
        }
        else
        {
          Serie2MBus.Serie2_3_Logger.Debug("--> Write memory done");
          ZR_ClassLibMessages.ClearErrorText();
          return true;
        }
      }
      return false;
    }

    internal bool WriteBitfield()
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.WriteBitField);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add((byte) (this.StartAddress & (int) byte.MaxValue));
      this.TransmitBuffer.Add((byte) (this.StartAddress >> 8 & (int) byte.MaxValue));
      this.TransmitBuffer.Add(1);
      this.TransmitBuffer.Add(9);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(this.AndMask);
      this.TransmitBuffer.Add(this.OrMask);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusStartWriteBlock);
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        this.MyBus.BusState.IncrementTransmitBlockCounter();
        if (this.ReceiveOkNok())
        {
          ZR_ClassLibMessages.ClearErrorText();
          return true;
        }
      }
      return false;
    }

    internal bool DigitalInputsAndOutputs(
      uint NewOutputMask,
      uint NewOutputState,
      ref uint OldOutputState,
      ref uint OldInputState)
    {
      this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.DigitalInputsAndOutputs);
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add((byte) NewOutputMask);
      this.TransmitBuffer.Add((byte) NewOutputState);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(12);
      this.FinishLongFrame();
      while (this.MyBus.BusState.TestRepeatCounter(this.MyBus.MaxRequestRepeat))
      {
        this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusSendREQ_Version);
        this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
        ++this.MyBus.BusState.TransmitBlockCounter;
        if (this.ReceiveHeader() && this.ReceiveLongframeEnd())
        {
          if (this.ReceiveBuffer.Count - 3 != 2)
          {
            this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusReadWrongBlockLength);
          }
          else
          {
            OldOutputState = (uint) this.ReceiveBuffer.Data[1];
            OldInputState = (uint) this.ReceiveBuffer.Data[2];
            ZR_ClassLibMessages.ClearErrorText();
            return true;
          }
        }
      }
      return false;
    }

    internal bool StartMeterMonitor(int SampleTime)
    {
      if (!this.MyBus.MyCom.Open())
        return false;
      this.GenerateSendDataHeader();
      this.TransmitBuffer.Add(15);
      this.TransmitBuffer.Add((byte) SampleTime);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(0);
      this.TransmitBuffer.Add(5);
      this.FinishLongFrame();
      this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer);
      return true;
    }

    internal override bool GetMeterMonitorData(out ByteField Buffer)
    {
      Buffer = new ByteField(200);
      return this.MyBus.MyCom.ReceiveBlock(ref Buffer);
    }
  }
}

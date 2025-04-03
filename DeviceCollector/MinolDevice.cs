// Decompiled with JetBrains decompiler
// Type: DeviceCollector.MinolDevice
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using NLog;
using System;
using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public sealed class MinolDevice : MBusDevice
  {
    private const int LONG_WACKEUP_SEQUENCE = 1920;
    private const int SHORT_WACKEUP_SEQUENCE = 64;
    private static Logger logger = LogManager.GetLogger("DeviceCollector.MinolDevice");
    private byte[] savedReceivedProtocol;
    private int lengthOfWakeUpSequence;
    private int[] CodeTable = new int[8]
    {
      18463,
      21893,
      3673,
      27814,
      18154,
      29022,
      9735,
      15054
    };

    public MinolDevice(DeviceCollectorFunctions TheBus)
      : base(TheBus)
    {
      this.DeviceType = DeviceTypes.Minol_Device;
      this.Info.Manufacturer = "MINOL";
      this.Info.ParameterOk = true;
      this.lengthOfWakeUpSequence = 1920;
      this.UseOnlyLongWakeUpSequence = false;
    }

    public bool UseOnlyLongWakeUpSequence { get; set; }

    internal bool ReadMemory(
      MemoryLocation Location,
      int mbusAddress,
      int NumberOfBytes,
      out ByteField MemoryData)
    {
      MemoryData = (ByteField) null;
      if (this.MyBus.BreakRequest)
        return false;
      try
      {
        this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.ReadMemoryBlock);
        this.lengthOfWakeUpSequence = !this.UseOnlyLongWakeUpSequence ? 64 : 1920;
        int num = 0;
        int maxRequestRepeat = this.MyBus.MaxRequestRepeat;
        if (maxRequestRepeat == 1)
          ++maxRequestRepeat;
        while (this.MyBus.BusState.TestRepeatCounter(maxRequestRepeat))
        {
          ++num;
          if (MinolDevice.logger.IsTraceEnabled)
            MinolDevice.logger.Trace("Read {0} address {1} wake-up {2} IrDaPuls {3} attempt {4}/{5}", new object[6]
            {
              (object) Location,
              (object) mbusAddress,
              (object) this.lengthOfWakeUpSequence,
              (object) this.MyBus.MyCom.MinoConnectIrDaPulseLength,
              (object) num,
              (object) maxRequestRepeat
            });
          if (this.MyBus.MyCom.Transceiver == TransceiverDevice.MinoHead)
          {
            this.TransmitBuffer = new ByteField(5);
            this.TransmitBuffer.Add(0);
            this.TransmitBuffer.Add((byte) 12);
            this.TransmitBuffer.Add((byte) this.lengthOfWakeUpSequence);
            this.TransmitBuffer.Add((byte) ((this.lengthOfWakeUpSequence & 65280) >> 8));
            this.TransmitBuffer.Add((byte) mbusAddress);
          }
          else
          {
            this.TransmitBuffer = new ByteField(this.lengthOfWakeUpSequence + 5);
            for (int index = 0; index < this.lengthOfWakeUpSequence; ++index)
              this.TransmitBuffer.Add(85);
            byte Byte = (byte) mbusAddress;
            this.TransmitBuffer.Add(16);
            this.TransmitBuffer.Add(75);
            this.TransmitBuffer.Add(Byte);
            this.TransmitBuffer.Add((byte) (75U + (uint) Byte));
            this.TransmitBuffer.Add(22);
          }
          string message = "Read address " + mbusAddress.ToString();
          if (num > 2)
            message = message + " Attemp: " + num.ToString() + " of " + maxRequestRepeat.ToString();
          this.MyBus.SendMessage(new GMM_EventArgs(message));
          if (this.MyBus.BreakRequest)
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled);
            return false;
          }
          LogManager.DisableLogging();
          if (!this.MyBus.MyCom.TransmitBlock(ref this.TransmitBuffer))
            return false;
          LogManager.EnableLogging();
          if (!this.ReceiveHeader())
          {
            if (ZR_ClassLibMessages.GetLastError() == ZR_ClassLibMessages.LastErrors.Timeout)
              this.lengthOfWakeUpSequence = 1920;
          }
          else
          {
            MemoryData = new ByteField(this.ReceiveBuffer.Count + this.FrameEndLen);
            for (int index = 0; index < this.ReceiveBuffer.Count; ++index)
              MemoryData.Add(this.ReceiveBuffer.Data[index]);
            if (this.MyBus.BreakRequest)
            {
              ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled);
              return false;
            }
            if (!this.ReceiveLongframeEnd())
            {
              this.MyBus.MyCom.ClearCom();
              if (this.MyBus.MyCom.Transceiver == TransceiverDevice.MinoConnect)
                this.MyBus.MyCom.ResetEarliestTransmitTime();
              if (ZR_ClassLibMessages.GetLastError() == ZR_ClassLibMessages.LastErrors.Timeout)
                this.lengthOfWakeUpSequence = 1920;
            }
            else
            {
              for (int index = 0; index < this.ReceiveBuffer.Count; ++index)
                MemoryData.Add(this.ReceiveBuffer.Data[index]);
              this.savedReceivedProtocol = MemoryData.Data;
              ZR_ClassLibMessages.ClearErrors();
              return true;
            }
          }
        }
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Read memory failed! The number of attempts: " + num.ToString());
        return false;
      }
      catch (Exception ex)
      {
        string str = "Read memory failed! Error:" + ex.Message + " Trace: " + ex.StackTrace;
        MinolDevice.logger.Error(ex, str);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, str);
        return false;
      }
    }

    internal bool WriteMemory(MemoryLocation Location, int StartAddress, ByteField WriteData)
    {
      BusDevice.CheckReadOnlyRight();
      try
      {
        if (!this.MyBus.MyCom.IsOpen && this.MyBus.MyCom.Open())
          return false;
        if (MinolDevice.logger.IsDebugEnabled)
          MinolDevice.logger.Debug("Write: " + Location.ToString() + " Address: " + StartAddress.ToString() + " Buffer: " + Util.ByteArrayToHexString(WriteData.Data));
        this.MyBus.BusState.StartBusFunctionTask(BusStatusClass.BusFunctionTasks.WriteMemoryBlock);
        int num = 0;
        int maxRequestRepeat = this.MyBus.MaxRequestRepeat;
        if (maxRequestRepeat == 1)
          ++maxRequestRepeat;
        while (num < maxRequestRepeat)
        {
          ++num;
          if (!this.ReadMemory(MemoryLocation.RAM, 0, 0, out ByteField _))
            return false;
          this.lengthOfWakeUpSequence = num <= 1 ? 64 : 1920;
          int writePassword = this.GetWritePassword();
          byte Byte = Location == MemoryLocation.RAM ? (byte) 0 : (byte) 1;
          this.TransmitBuffer = new ByteField(14 + WriteData.Count);
          this.TransmitBuffer.Add(104);
          this.TransmitBuffer.Add(0);
          this.TransmitBuffer.Add(0);
          this.TransmitBuffer.Add(104);
          this.TransmitBuffer.Add(67);
          this.TransmitBuffer.Add(Byte);
          this.TransmitBuffer.Add(178);
          this.TransmitBuffer.Add((byte) writePassword);
          this.TransmitBuffer.Add((byte) (writePassword >> 8));
          this.TransmitBuffer.Add((byte) WriteData.Count);
          this.TransmitBuffer.Add((byte) StartAddress);
          this.TransmitBuffer.Add((byte) (StartAddress >> 8));
          for (int index = 0; index < WriteData.Count; ++index)
            this.TransmitBuffer.Add(WriteData.Data[index]);
          this.FinishLongFrame();
          string message = "Write at address " + Byte.ToString();
          if (this.MyBus.BusState.RepeadCounter > 1)
            message = message + " Attemp: " + num.ToString() + " of " + this.MyBus.MaxRequestRepeat.ToString();
          this.MyBus.SendMessage(new GMM_EventArgs(message));
          if (this.MyBus.BreakRequest)
          {
            ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.OperationCancelled);
            return false;
          }
          if (this.MyBus.MyCom.Transceiver == TransceiverDevice.MinoHead)
          {
            this.lengthOfWakeUpSequence = 1920;
            ByteField DataBlock1 = new ByteField(4);
            DataBlock1.Add(0);
            DataBlock1.Add((byte) 4);
            DataBlock1.Add((byte) this.lengthOfWakeUpSequence);
            DataBlock1.Add((byte) ((this.lengthOfWakeUpSequence & 65280) >> 8));
            MinolDevice.logger.Trace("Set MinoHead lp2SetWakeupParams: {0}", this.lengthOfWakeUpSequence);
            this.MyBus.MyCom.TransmitBlock(ref DataBlock1);
            Util.Wait(200L, "while MinoHead send command.", (ICancelable) this.MyBus, MinolDevice.logger);
            if (this.MyBus.BreakRequest)
              return false;
            this.MyBus.MyCom.ClearCom();
            List<byte> byteList = new List<byte>();
            byteList.Add((byte) 0);
            byteList.Add((byte) 8);
            byteList.AddRange((IEnumerable<byte>) this.TransmitBuffer.Data);
            ByteField DataBlock2 = new ByteField(byteList.ToArray());
            this.MyBus.MyCom.TransmitBlock(ref DataBlock2);
            this.MyBus.BusState.IncrementTransmitBlockCounter();
            if (!Util.Wait(700L, "while MinoHead send command.", (ICancelable) this.MyBus, MinolDevice.logger))
              return false;
            this.MyBus.MyCom.ClearCom();
            ZR_ClassLibMessages.ClearErrors();
            return true;
          }
          ByteField DataBlock3 = new ByteField(14 + WriteData.Count + this.lengthOfWakeUpSequence);
          for (int index = 0; index < this.lengthOfWakeUpSequence; ++index)
            DataBlock3.Add(85);
          DataBlock3.Add(this.TransmitBuffer);
          this.MyBus.MyCom.ComWriteLoggerEvent(EventLogger.LoggerEvent.BusStartWriteMemory);
          this.MyBus.MyCom.ResetEarliestTransmitTime();
          if (!this.MyBus.MyCom.TransmitBlock(ref DataBlock3))
            return false;
          this.MyBus.BusState.IncrementTransmitBlockCounter();
          ByteField DataBlock4 = new ByteField(1);
          bool block = this.MyBus.MyCom.ReceiveBlock(ref DataBlock4, 1, true);
          if (block && DataBlock4.Count == 1 && DataBlock4.Data[0] == (byte) 229)
          {
            ZR_ClassLibMessages.ClearErrors();
            return true;
          }
          if (block)
          {
            MinolDevice.logger.Error("Received wrong byte! Expected 0xE5, Actual 0x" + DataBlock4.Data[0].ToString("X2"));
            if (!Util.Wait(600L, "before clear input buffer", (ICancelable) this.MyBus, MinolDevice.logger))
              return false;
            this.MyBus.MyCom.ClearCom();
          }
          else if (ZR_ClassLibMessages.GetLastError() == ZR_ClassLibMessages.LastErrors.Timeout)
            this.lengthOfWakeUpSequence = 1920;
        }
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Write memory failed! The number of attempts: " + num.ToString());
        return false;
      }
      catch (Exception ex)
      {
        string str = "Write memory failed! Error: " + ex.Message;
        MinolDevice.logger.Error(ex, str);
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, str);
        return false;
      }
    }

    internal override bool EraseFlash(int StartAddress, int NumberOfBytes)
    {
      BusDevice.CheckReadOnlyRight();
      MinolDevice.logger.Debug("Start EraseFlash");
      bool flag = this.WriteMemory(MemoryLocation.FLASH, 4096, new ByteField(0));
      if (!flag)
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.CommunicationError, "Failed to erase the FLASH memory!");
      MinolDevice.logger.Debug("End EraseFlash");
      return flag;
    }

    private int GetWritePassword()
    {
      if (this.savedReceivedProtocol == null && this.savedReceivedProtocol.Length > 15)
        return -1;
      int StartOffset = 7;
      int num1 = (int) this.savedReceivedProtocol[1];
      int num2 = (int) this.savedReceivedProtocol[13];
      int num3 = (int) this.savedReceivedProtocol[17] + ((int) this.savedReceivedProtocol[18] << 8);
      if (num1 == 175 && num2 == 1 && num3 == 0)
        StartOffset = 163;
      short fromByteArrayShort1 = ParameterService.GetFromByteArray_short(this.savedReceivedProtocol, ref StartOffset);
      short fromByteArrayShort2 = ParameterService.GetFromByteArray_short(this.savedReceivedProtocol, ref StartOffset);
      byte num4 = this.savedReceivedProtocol[15];
      return (int) fromByteArrayShort2 ^ (int) fromByteArrayShort1 ^ ((int) num4 << 8) + (int) num4 ^ this.CodeTable[(int) num4 & 7];
    }

    public static InputUnitsIndex ConvertMinolUnitToInputUnitsIndex(byte minotelContactIndex)
    {
      minotelContactIndex &= (byte) 15;
      switch (minotelContactIndex)
      {
        case 1:
          return InputUnitsIndex.ImpUnit_0L;
        case 2:
          return InputUnitsIndex.ImpUnit_0qm;
        case 3:
          return InputUnitsIndex.ImpUnit_0Wh;
        case 4:
          return InputUnitsIndex.ImpUnit_0kWh;
        case 5:
          return InputUnitsIndex.ImpUnit_0MWh;
        case 6:
          return InputUnitsIndex.ImpUnit_0J;
        case 7:
          return InputUnitsIndex.ImpUnit_0kJ;
        case 8:
          return InputUnitsIndex.ImpUnit_0MJ;
        case 9:
          return InputUnitsIndex.ImpUnit_0GJ;
        default:
          return InputUnitsIndex.ImpUnit_0;
      }
    }
  }
}

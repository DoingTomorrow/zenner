// Decompiled with JetBrains decompiler
// Type: TH_Handler.TH
// Assembly: TH_Handler, Version=1.3.4.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 02D62764-6653-46F8-9117-1BC5233AD061
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\TH_Handler.dll

using AsyncCom;
using DeviceCollector;
using GmmDbLib;
using HandlerLib;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;
using ZR_ClassLibrary;

#nullable disable
namespace TH_Handler
{
  public sealed class TH
  {
    private static Logger logger = LogManager.GetLogger(nameof (TH));
    private DeviceCollectorFunctions deviceCollector;

    public TH(DeviceCollectorFunctions deviceCollector) => this.deviceCollector = deviceCollector;

    private TimeSpan RetryInterval
    {
      get
      {
        return this.deviceCollector == null ? TimeSpan.MinValue : TimeSpan.FromMilliseconds((double) ((AsyncFunctions) this.deviceCollector.MyCom).WaitBeforeRepeatTime);
      }
    }

    private int RetryCount
    {
      get => this.deviceCollector == null ? 0 : this.deviceCollector.MaxRequestRepeat;
    }

    public TH_Version ReadVersion()
    {
      return Retry.Do<TH_Version>((Func<TH_Version>) (() => this.ReadVersionOnce()), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    public bool RadioOOK(RadioMode mode, short offset, ushort timeoutInSeconds)
    {
      return Retry.Do<bool>((Func<bool>) (() => this.RadioOOKOnce(mode, offset, timeoutInSeconds)), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    public bool RadioPN9(RadioMode mode, short offset, ushort timeoutInSeconds)
    {
      return Retry.Do<bool>((Func<bool>) (() => this.RadioPN9Once(mode, offset, timeoutInSeconds)), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    public bool ResetToDelivery()
    {
      return Retry.Do<bool>((Func<bool>) (() => this.ResetToDeliveryOnce()), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    public TactileSwitchState TactileSwitch()
    {
      return Retry.Do<TactileSwitchState>((Func<TactileSwitchState>) (() => this.TactileSwitchOnce()), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    public DateTime ReadDateTime()
    {
      return Retry.Do<DateTime>((Func<DateTime>) (() => this.ReadDateTimeOnce()), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    public bool WriteDateTime(DateTime value)
    {
      return Retry.Do<bool>((Func<bool>) (() => this.WriteDateTimeOnce(value)), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    public bool ResetDevice()
    {
      return Retry.Do<bool>((Func<bool>) (() => this.ResetDeviceOnce()), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    public bool LCDDisable()
    {
      return Retry.Do<bool>((Func<bool>) (() => this.LCDDisableOnce()), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    public bool LCDEnable()
    {
      return Retry.Do<bool>((Func<bool>) (() => this.LCDEnableOnce()), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    public bool LCDTest(LcdTest value)
    {
      return Retry.Do<bool>((Func<bool>) (() => this.LCDTestOnce(value)), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    public bool RadioDisable()
    {
      return Retry.Do<bool>((Func<bool>) (() => this.RadioDisableOnce()), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    public bool SaveConfig()
    {
      return Retry.Do<bool>((Func<bool>) (() => this.SaveConfigOnce()), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    public bool RadioEnable()
    {
      return Retry.Do<bool>((Func<bool>) (() => this.RadioEnableOnce()), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    public bool RadioTransmit()
    {
      return Retry.Do<bool>((Func<bool>) (() => this.RadioTransmitOnce()), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    public uint ReadSerial()
    {
      return Retry.Do<uint>((Func<uint>) (() => this.ReadSerialOnce()), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    public double ReadTemperature()
    {
      return Retry.Do<double>((Func<double>) (() => this.ReadTemperatureOnce()), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    public double ReadHumidity()
    {
      return Retry.Do<double>((Func<double>) (() => this.ReadHumidityOnce()), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    public bool Sleep()
    {
      return Retry.Do<bool>((Func<bool>) (() => this.SleepOnce()), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    public bool WakeUp()
    {
      return Retry.Do<bool>((Func<bool>) (() => this.WakeUpOnce()), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    public bool SND_NKE()
    {
      TH.logger.Debug(nameof (SND_NKE));
      return this.Write("1040FF3F16");
    }

    internal TH_Version ManageIrDaWakeUpAndReadVersion()
    {
      this.deviceCollector.AsyncCom.WakeupTemporaryOff = true;
      try
      {
        TH_Version thVersion = this.ReadVersionOnce();
        if (thVersion != null)
          return thVersion;
        this.deviceCollector.AsyncCom.ClearWakeup();
        return this.ReadVersion() ?? throw new Exception(ZR_ClassLibMessages.GetLastErrorAndClearError().LastErrorDescription);
      }
      finally
      {
        ZR_ClassLibMessages.ClearErrors();
      }
    }

    internal byte[] ReadMemory(ushort startAddress, int size)
    {
      return Retry.Do<byte[]>((Func<byte[]>) (() => this.ReadMemoryOnce(startAddress, size)), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    internal bool EraseFLASHSegment(ushort startAddress)
    {
      return Retry.Do<bool>((Func<bool>) (() => this.EraseFLASHSegmentOnce(startAddress)), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    internal bool WriteFLASH(ushort address, byte[] buffer)
    {
      return this.WriteMemory(MemoryLocation.FLASH, address, buffer);
    }

    internal bool WriteRAM(ushort address, byte[] buffer)
    {
      return this.WriteMemory(MemoryLocation.RAM, address, buffer);
    }

    private List<byte> CreateHeader()
    {
      return new List<byte>((IEnumerable<byte>) new byte[8]
      {
        (byte) 104,
        (byte) 0,
        (byte) 0,
        (byte) 104,
        (byte) 83,
        (byte) 254,
        (byte) 81,
        (byte) 15
      });
    }

    private void FinishFrame(List<byte> buffer)
    {
      byte num = 0;
      for (int index = 4; index < buffer.Count; ++index)
        num += buffer[index];
      buffer[1] = (byte) (buffer.Count - 4);
      buffer[2] = buffer[1];
      buffer.Add(num);
      buffer.Add((byte) 22);
    }

    private bool Write(string hex) => this.Write(Util.HexStringToByteArray(hex));

    private bool Write(byte[] buffer)
    {
      this.deviceCollector.BreakRequest = false;
      return this.deviceCollector.AsyncCom.TransmitBlock(buffer);
    }

    private byte[] Read()
    {
      ByteField DataBlock1 = new ByteField(4);
      if (!this.deviceCollector.AsyncCom.ReceiveBlock(ref DataBlock1, 4, true))
        return (byte[]) null;
      byte[] byteArray1 = DataBlock1.GetByteArray();
      if (byteArray1[0] != (byte) 104 || (int) byteArray1[0] != (int) byteArray1[3] || (int) byteArray1[1] != (int) byteArray1[2])
        throw new Exception(Ot.Gtm(Tg.HandlerLogic, "InvalidMBusHeader", "Invalid M-Bus header!"));
      int num = (int) byteArray1[1] + 2;
      ByteField DataBlock2 = new ByteField(num);
      if (!this.deviceCollector.AsyncCom.ReceiveBlock(ref DataBlock2, num, true))
        return (byte[]) null;
      byte[] byteArray2 = DataBlock2.GetByteArray();
      byte[] dst = new byte[byteArray1.Length + byteArray2.Length];
      Buffer.BlockCopy((Array) byteArray1, 0, (Array) dst, 0, byteArray1.Length);
      Buffer.BlockCopy((Array) byteArray2, 0, (Array) dst, byteArray1.Length, byteArray2.Length);
      return dst;
    }

    private byte? ReadByte()
    {
      ByteField DataBlock = new ByteField(1);
      if (!this.deviceCollector.AsyncCom.ReceiveBlock(ref DataBlock, 1, true))
        return new byte?();
      byte[] byteArray = DataBlock.GetByteArray();
      if (byteArray.Length == 0)
        return new byte?();
      return byteArray.Length == 1 ? new byte?(byteArray[0]) : throw new Exception(Ot.Gtm(Tg.HandlerLogic, "InvalidResponce", "Invalid response: ") + Util.ByteArrayToHexString(byteArray));
    }

    private static bool CheckResponce(byte[] responce)
    {
      if (responce == null)
      {
        ZR_ClassLibMessages.LastErrorInfo errorAndClearError = ZR_ClassLibMessages.GetLastErrorAndClearError();
        if (errorAndClearError.LastError == ZR_ClassLibMessages.LastErrors.Timeout)
          throw new TimeoutException(errorAndClearError.LastErrorDescription);
        throw new Exception(errorAndClearError.LastErrorDescription);
      }
      if (responce.Length < 8 || responce[19] != (byte) 15 || responce[23] != (byte) 66 || ((int) responce[24] & 128) != 128)
      {
        string message = Ot.Gtm(Tg.HandlerLogic, "InvalidResponce", "Invalid response");
        TH.logger.Error(message + " " + Util.ByteArrayToHexString(responce));
        throw new Exception(message);
      }
      return true;
    }

    private TH_Version ReadVersionOnce()
    {
      TH.logger.Debug("ReadVersion");
      this.Write("6808086853FE510F00000006B716");
      byte[] buffer = this.Read();
      return buffer == null ? (TH_Version) null : TH_Version.Parse(buffer, 20);
    }

    private byte[] ReadMemoryOnce(ushort startAddress, int size)
    {
      TH.logger.Debug("ReadMemory 0x" + startAddress.ToString("X4") + ", " + size.ToString() + " byte(s)");
      List<byte> header = this.CreateHeader();
      header.AddRange((IEnumerable<byte>) BitConverter.GetBytes(startAddress));
      header.Add((byte) size);
      header.Add((byte) 2);
      this.FinishFrame(header);
      if (!this.Write(header.ToArray()))
        return (byte[]) null;
      byte[] src = this.Read();
      if (src == null || src.Length < 22)
        return (byte[]) null;
      byte[] dst = new byte[src.Length - 22];
      Buffer.BlockCopy((Array) src, 20, (Array) dst, 0, dst.Length);
      return dst;
    }

    private bool EraseFLASHSegmentOnce(ushort startAddress)
    {
      TH.logger.Debug("EraseFLASHSegment 0x" + startAddress.ToString("X4"));
      List<byte> header = this.CreateHeader();
      header.AddRange((IEnumerable<byte>) BitConverter.GetBytes(startAddress));
      header.Add((byte) 0);
      header.Add((byte) 14);
      this.FinishFrame(header);
      this.Write(header.ToArray());
      Thread.Sleep(200);
      byte? nullable = this.ReadByte();
      return nullable.HasValue && nullable.Value == (byte) 229;
    }

    private bool ResetDeviceOnce()
    {
      TH.logger.Debug("ResetDevice");
      List<byte> header = this.CreateHeader();
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 4);
      this.FinishFrame(header);
      this.Write(header.ToArray());
      byte? nullable = this.ReadByte();
      return nullable.HasValue && nullable.Value == (byte) 229;
    }

    private bool SaveConfigOnce()
    {
      TH.logger.Debug("Save Config");
      List<byte> header = this.CreateHeader();
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 8);
      this.FinishFrame(header);
      this.Write(header.ToArray());
      byte? nullable = this.ReadByte();
      return nullable.HasValue && nullable.Value == (byte) 229;
    }

    private bool WriteMemory(MemoryLocation location, ushort address, byte[] buffer)
    {
      return Retry.Do<bool>((Func<bool>) (() => this.WriteMemoryOnce(location, address, buffer)), this.RetryInterval, this.RetryCount, new Action(this.deviceCollector.AsyncCom.ClearWakeup));
    }

    private bool WriteMemoryOnce(MemoryLocation location, ushort address, byte[] buffer)
    {
      if (location != MemoryLocation.FLASH && location != MemoryLocation.RAM)
        return false;
      if (buffer == null || buffer.Length == 0)
        return true;
      TH.logger.Debug("WriteMemory " + location.ToString() + ", 0x" + address.ToString("X4") + ", " + buffer.Length.ToString() + " byte(s)");
      byte num = 3;
      if (location == MemoryLocation.FLASH)
      {
        num = (byte) 1;
        if (buffer.Length % 4 != 0)
          throw new Exception("Internally T&H Handler Bug: FLASH write. Write buffer is not multiple of 4!");
      }
      List<byte> header = this.CreateHeader();
      header.AddRange((IEnumerable<byte>) BitConverter.GetBytes(address));
      header.Add((byte) buffer.Length);
      header.Add(num);
      header.AddRange((IEnumerable<byte>) buffer);
      this.FinishFrame(header);
      this.Write(header.ToArray());
      Thread.Sleep(100);
      byte? nullable = this.ReadByte();
      return nullable.HasValue && nullable.Value == (byte) 229;
    }

    private bool RadioOOKOnce(RadioMode mode, short offset, ushort timeoutInSeconds)
    {
      TH.logger.Debug("RadioOOK");
      List<byte> header = this.CreateHeader();
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 66);
      header.Add((byte) 12);
      header.Add((byte) mode);
      header.AddRange((IEnumerable<byte>) BitConverter.GetBytes(offset));
      header.AddRange((IEnumerable<byte>) BitConverter.GetBytes(timeoutInSeconds));
      this.FinishFrame(header);
      this.Write(header.ToArray());
      return TH.CheckResponce(this.Read());
    }

    private bool RadioPN9Once(RadioMode mode, short offset, ushort timeoutInSeconds)
    {
      TH.logger.Debug("RadioPN9");
      List<byte> header = this.CreateHeader();
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 66);
      header.Add((byte) 13);
      header.Add((byte) mode);
      header.AddRange((IEnumerable<byte>) BitConverter.GetBytes(offset));
      header.AddRange((IEnumerable<byte>) BitConverter.GetBytes(timeoutInSeconds));
      this.FinishFrame(header);
      this.Write(header.ToArray());
      return TH.CheckResponce(this.Read());
    }

    private bool ResetToDeliveryOnce()
    {
      TH.logger.Debug("ResetToDelivery");
      List<byte> header = this.CreateHeader();
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 66);
      header.Add((byte) 26);
      this.FinishFrame(header);
      this.Write(header.ToArray());
      Thread.Sleep(100);
      return TH.CheckResponce(this.Read());
    }

    private TactileSwitchState TactileSwitchOnce()
    {
      TH.logger.Debug("TactileSwitch");
      List<byte> header = this.CreateHeader();
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 66);
      header.Add((byte) 20);
      this.FinishFrame(header);
      this.Write(header.ToArray());
      byte[] responce = this.Read();
      TH.CheckResponce(responce);
      return (TactileSwitchState) responce[25];
    }

    private DateTime ReadDateTimeOnce()
    {
      TH.logger.Debug("ReadDateTime");
      List<byte> header = this.CreateHeader();
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 66);
      header.Add((byte) 14);
      this.FinishFrame(header);
      this.Write(header.ToArray());
      byte[] responce = this.Read();
      TH.CheckResponce(responce);
      byte num = responce[25];
      byte month = responce[26];
      byte day = responce[27];
      byte hour = responce[28];
      byte minute = responce[29];
      byte second = responce[30];
      DateTime dateTime = new DateTime(2000, 1, 1);
      if (month > (byte) 12 || day > (byte) 31 || hour > (byte) 23 || minute > (byte) 59 || second > (byte) 59)
        return dateTime;
      if (month == (byte) 0 || day == (byte) 0)
        return dateTime;
      try
      {
        return new DateTime((int) num + 2000, (int) month, (int) day, (int) hour, (int) minute, (int) second);
      }
      catch
      {
        return dateTime;
      }
    }

    private bool WriteDateTimeOnce(DateTime value)
    {
      if (value.Year < 2000)
        throw new ArgumentOutOfRangeException(Ot.Gtm(Tg.Handler_UI, "SystemTimeInvalidMin", "Invalid system date. The year should be greater or equal to 2000.") + " " + value.ToLongDateString());
      if (value.Year > 2255)
        throw new ArgumentOutOfRangeException(Ot.Gtm(Tg.Handler_UI, "SystemTimeInvalidMax", "Invalid system date. The year should be smaller as 2255.") + " " + value.ToLongDateString());
      TH.logger.Debug("WriteDateTime(" + value.ToString() + ")");
      byte num = (byte) (value.Year - 2000);
      byte month = (byte) value.Month;
      byte day = (byte) value.Day;
      byte hour = (byte) value.Hour;
      byte minute = (byte) value.Minute;
      byte second = (byte) value.Second;
      List<byte> header = this.CreateHeader();
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 66);
      header.Add((byte) 14);
      header.Add(num);
      header.Add(month);
      header.Add(day);
      header.Add(hour);
      header.Add(minute);
      header.Add(second);
      this.FinishFrame(header);
      this.Write(header.ToArray());
      byte[] responce = this.Read();
      TH.CheckResponce(responce);
      return (int) num == (int) responce[25] && (int) month == (int) responce[26] && (int) day == (int) responce[27] && (int) hour == (int) responce[28] && (int) minute == (int) responce[29] && (int) second == (int) responce[30];
    }

    private bool LCDDisableOnce()
    {
      TH.logger.Debug("LCD Disable");
      List<byte> header = this.CreateHeader();
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 66);
      header.Add((byte) 16);
      this.FinishFrame(header);
      this.Write(header.ToArray());
      return TH.CheckResponce(this.Read());
    }

    private bool LCDEnableOnce()
    {
      TH.logger.Debug("LCD Enable");
      List<byte> header = this.CreateHeader();
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 66);
      header.Add((byte) 17);
      this.FinishFrame(header);
      this.Write(header.ToArray());
      return TH.CheckResponce(this.Read());
    }

    private bool LCDTestOnce(LcdTest value)
    {
      TH.logger.Debug("LCD Test");
      List<byte> header = this.CreateHeader();
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 66);
      header.Add((byte) 18);
      header.Add((byte) value);
      this.FinishFrame(header);
      this.Write(header.ToArray());
      return this.ReadByte().HasValue;
    }

    private bool RadioDisableOnce()
    {
      TH.logger.Debug("Radio Disable");
      List<byte> header = this.CreateHeader();
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 66);
      header.Add((byte) 6);
      this.FinishFrame(header);
      this.Write(header.ToArray());
      return TH.CheckResponce(this.Read());
    }

    private bool RadioEnableOnce()
    {
      TH.logger.Debug("Radio Enable");
      List<byte> header = this.CreateHeader();
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 66);
      header.Add((byte) 9);
      this.FinishFrame(header);
      this.Write(header.ToArray());
      return TH.CheckResponce(this.Read());
    }

    private uint ReadSerialOnce()
    {
      TH.logger.Debug("Read serial");
      List<byte> header = this.CreateHeader();
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 66);
      header.Add((byte) 48);
      header.Add((byte) 0);
      this.FinishFrame(header);
      this.Write(header.ToArray());
      byte[] responce = this.Read();
      TH.CheckResponce(responce);
      return Util.ConvertBcdUInt32ToUInt32(BitConverter.ToUInt32(responce, 26));
    }

    private bool RadioTransmitOnce()
    {
      if (!this.RadioEnable())
        return false;
      TH.logger.Debug("Radio Transmit");
      uint num = Util.SwapBytes(Util.ConvertUnt32ToBcdUInt32(this.ReadSerial()));
      byte[] byteArray = Util.HexStringToByteArray("223000000200092F230E000000ED7FFF7FFF7FFF7FFF7FFF7FFF7FFF7FFF7FFF7FFF7FFF7FFF7FFF7FFF7FFF");
      Buffer.BlockCopy((Array) BitConverter.GetBytes(num), 0, (Array) byteArray, 1, 4);
      List<byte> header = this.CreateHeader();
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 66);
      header.Add((byte) 11);
      header.Add((byte) byteArray.Length);
      header.AddRange((IEnumerable<byte>) byteArray);
      this.FinishFrame(header);
      this.Write(header.ToArray());
      return TH.CheckResponce(this.Read());
    }

    private double ReadTemperatureOnce()
    {
      TH.logger.Debug("Read Temperature");
      List<byte> header = this.CreateHeader();
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 66);
      header.Add((byte) 33);
      header.Add((byte) 0);
      this.FinishFrame(header);
      this.Write(header.ToArray());
      byte[] responce = this.Read();
      TH.CheckResponce(responce);
      return (double) BitConverter.ToUInt16(responce, 26) / 10.0;
    }

    private double ReadHumidityOnce()
    {
      TH.logger.Debug("Read Humidity");
      List<byte> header = this.CreateHeader();
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 66);
      header.Add((byte) 33);
      header.Add((byte) 1);
      this.FinishFrame(header);
      this.Write(header.ToArray());
      byte[] responce = this.Read();
      TH.CheckResponce(responce);
      return (double) BitConverter.ToUInt16(responce, 26) / 10.0;
    }

    private bool SleepOnce()
    {
      TH.logger.Debug("Sleep");
      List<byte> header = this.CreateHeader();
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 66);
      header.Add((byte) 15);
      header.Add((byte) 1);
      this.FinishFrame(header);
      this.Write(header.ToArray());
      byte[] responce = this.Read();
      TH.CheckResponce(responce);
      return responce[25] == byte.MaxValue;
    }

    private bool WakeUpOnce()
    {
      TH.logger.Debug("Wake up");
      List<byte> header = this.CreateHeader();
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 0);
      header.Add((byte) 66);
      header.Add((byte) 15);
      header.Add((byte) 0);
      this.FinishFrame(header);
      this.Write(header.ToArray());
      Thread.Sleep(9000);
      byte[] responce = this.Read();
      TH.CheckResponce(responce);
      return responce[25] == (byte) 0;
    }
  }
}

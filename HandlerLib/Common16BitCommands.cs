// Decompiled with JetBrains decompiler
// Type: HandlerLib.Common16BitCommands
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using HandlerLib.Interfaces;
using MBusLib;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public sealed class Common16BitCommands : IHandlerMemoryAccess
  {
    private static Logger Common16BitLogger = LogManager.GetLogger("S3_CommandsConnectionPort");
    private DeviceCommandsMBus deviceCMD;

    public Common16BitCommands(DeviceCommandsMBus deviceCMD) => this.deviceCMD = deviceCMD;

    public async Task ReadMemoryAsync(
      AddressRange addressRange,
      DeviceMemory deviceMemory,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] readData = await this.ReadMemoryAsync(progress, cancelToken, addressRange, (byte) 90);
      deviceMemory.SetData(addressRange.StartAddress, readData);
      readData = (byte[]) null;
    }

    public async Task WriteMemoryAsync(
      AddressRange addressRange,
      DeviceMemory deviceMemory,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] writeData = deviceMemory.GetData(addressRange);
      uint num = addressRange.ByteSize;
      byte cmd = byte.Parse(num.ToString());
      num = addressRange.StartAddress;
      ushort address = ushort.Parse(num.ToString());
      byte maxBytes = byte.Parse("192");
      await Task.Run((Action) (() => this.WriteMemory(progress, cancelToken, cmd, address, writeData, maxBytes)));
    }

    public async Task<byte[]> ReadMemoryAsync(
      ProgressHandler progress,
      CancellationToken token,
      ushort address,
      byte count)
    {
      byte[] numArray = await Task.Run<byte[]>((Func<byte[]>) (() => this.ReadMemory(progress, token, address, count)), token);
      return numArray;
    }

    public async Task<byte[]> ReadMemoryAsync(
      ProgressHandler progress,
      CancellationToken token,
      AddressRange addressRange,
      byte maxBytesPerPacket)
    {
      if (addressRange == null)
        throw new NullReferenceException(nameof (addressRange));
      if (addressRange.StartAddress > (uint) ushort.MaxValue)
        throw new ArgumentOutOfRangeException("AddressRange.StartAddress can not be greater as " + ushort.MaxValue.ToString());
      byte[] numArray = await Task.Run<byte[]>((Func<byte[]>) (() => this.ReadMemory(progress, token, (ushort) addressRange.StartAddress, addressRange.ByteSize, maxBytesPerPacket)), token);
      return numArray;
    }

    public async Task<byte[]> ReadMemoryAsync(
      ProgressHandler progress,
      CancellationToken token,
      ushort address,
      uint count,
      byte maxBytesPerPacket)
    {
      byte[] numArray = await Task.Run<byte[]>((Func<byte[]>) (() => this.ReadMemory(progress, token, address, count, maxBytesPerPacket)), token);
      return numArray;
    }

    public byte[] ReadMemory(
      ProgressHandler progress,
      CancellationToken token,
      ushort address,
      byte count)
    {
      if (progress == null)
        throw new ArgumentNullException(nameof (progress));
      if (count <= (byte) 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      token.ThrowIfCancellationRequested();
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 15);
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(address));
      byteList.Add(count);
      byteList.Add((byte) 2);
      Common16BitCommands.Common16BitLogger.Debug("Read memory. Address: 0x" + address.ToString("X4") + " 0x" + count.ToString("x4") + " byte(s)");
      VariableDataStructure variableDataStructure = VariableDataStructure.Parse(this.deviceCMD.MBus.Repeater.GetResultFrame(new MBusFrame(byteList.ToArray()), progress, token));
      if (variableDataStructure.MfgData.Length != (int) count)
        throw new Exception("Invalid response by read the memory! Expected: " + count.ToString() + " bytes but receive: " + variableDataStructure.MfgData.Length.ToString() + " byte(s)");
      return variableDataStructure.MfgData;
    }

    public byte[] ReadMemory(
      ProgressHandler progress,
      CancellationToken token,
      ushort address,
      uint count,
      byte maxBytesPerPacket)
    {
      if (count < 0U)
        throw new ArgumentOutOfRangeException(nameof (count));
      int num1 = maxBytesPerPacket >= (byte) 0 ? Convert.ToInt32(count / (uint) maxBytesPerPacket) : throw new ArgumentOutOfRangeException(nameof (maxBytesPerPacket));
      if (num1 <= 1 && count <= (uint) maxBytesPerPacket)
        return this.ReadMemory(progress, token, address, (byte) count);
      byte num2 = (byte) (count % (uint) maxBytesPerPacket);
      if (num2 > (byte) 0)
        ++num1;
      List<byte> byteList = new List<byte>((int) count);
      ushort address1 = address;
      byte count1 = maxBytesPerPacket;
      progress.Split(num1 + 1);
      progress.Report("Read: 0x" + address.ToString("X4"));
      for (int index = 1; index <= num1; ++index)
      {
        if (index > 1)
          address1 += (ushort) count1;
        count1 = (long) (index * (int) maxBytesPerPacket) >= (long) count ? (num2 > (byte) 0 ? num2 : maxBytesPerPacket) : maxBytesPerPacket;
        byte[] collection = this.ReadMemory(progress, token, address1, count1);
        byteList.AddRange((IEnumerable<byte>) collection);
      }
      return byteList.ToArray();
    }

    public async Task WriteRAMAsync(
      ProgressHandler progress,
      CancellationToken token,
      ushort address,
      byte[] buffer)
    {
      await Task.Run((Action) (() => this.WriteRAM(progress, token, address, buffer)), token);
    }

    public async Task WriteRAMAsync(
      ProgressHandler progress,
      CancellationToken token,
      AddressRange addressRange,
      byte[] buffer,
      byte maxBytesPerPacket)
    {
      if (addressRange == null)
        throw new NullReferenceException(nameof (addressRange));
      if (addressRange.StartAddress > (uint) ushort.MaxValue)
        throw new ArgumentOutOfRangeException("AddressRange.StartAddress can not be greater as " + ushort.MaxValue.ToString());
      await Task.Run((Action) (() => this.WriteRAM(progress, token, (ushort) addressRange.StartAddress, buffer, maxBytesPerPacket)), token);
    }

    public async Task WriteRAMAsync(
      ProgressHandler progress,
      CancellationToken token,
      ushort address,
      byte[] buffer,
      byte maxBytesPerPacket)
    {
      await Task.Run((Action) (() => this.WriteRAM(progress, token, address, buffer, maxBytesPerPacket)), token);
    }

    public void WriteRAM(
      ProgressHandler progress,
      CancellationToken token,
      ushort address,
      byte[] buffer)
    {
      this.WriteMemory(progress, token, (byte) 3, address, buffer);
    }

    public void WriteRAM(
      ProgressHandler progress,
      CancellationToken token,
      ushort address,
      byte[] buffer,
      byte maxBytesPerPacket)
    {
      this.WriteMemory(progress, token, (byte) 3, address, buffer, maxBytesPerPacket);
    }

    public async Task WriteFLASHAsync(
      ProgressHandler progress,
      CancellationToken token,
      ushort address,
      byte[] buffer)
    {
      await Task.Run((Action) (() => this.WriteFLASH(progress, token, address, buffer)), token);
    }

    public async Task WriteFLASHAsync(
      ProgressHandler progress,
      CancellationToken token,
      ushort address,
      byte[] buffer,
      byte maxBytesPerPacket)
    {
      await Task.Run((Action) (() => this.WriteFLASH(progress, token, address, buffer, maxBytesPerPacket)), token);
    }

    public void WriteFLASH(
      ProgressHandler progress,
      CancellationToken token,
      ushort address,
      byte[] buffer)
    {
      this.WriteMemory(progress, token, (byte) 1, address, buffer);
    }

    public void WriteFLASH(
      ProgressHandler progress,
      CancellationToken token,
      ushort address,
      byte[] buffer,
      byte maxBytesPerPacket)
    {
      this.WriteMemory(progress, token, (byte) 1, address, buffer, maxBytesPerPacket);
    }

    public async Task EraseFLASHAsync(
      ProgressHandler progress,
      CancellationToken token,
      ushort address)
    {
      await Task.Run((Action) (() => this.EraseFLASH(progress, token, address)), token);
    }

    public void EraseFLASH(ProgressHandler progress, CancellationToken token, ushort address)
    {
      if (progress == null)
        throw new ArgumentNullException(nameof (progress));
      if (address <= (ushort) 0)
        throw new ArgumentException(nameof (address));
      token.ThrowIfCancellationRequested();
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 15);
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(address));
      byteList.Add((byte) 0);
      byteList.Add((byte) 14);
      progress.Split(new double[2]{ 10.0, 90.0 });
      progress.Report("Erase FLASH");
      Common16BitCommands.Common16BitLogger.Debug("EraseFLASH: " + address.ToString("x4"));
      MBusFrame resultFrame = this.deviceCMD.MBus.Repeater.GetResultFrame(new MBusFrame(byteList.ToArray()), progress, token);
      if (resultFrame.Type != 0)
        throw new Exception("Invalid response by erase the memory! Expected: ACK, but receive: " + resultFrame.Type.ToString());
    }

    public void EraseFLASH(
      ProgressHandler progress,
      CancellationToken token,
      ushort StartAddress,
      int NumberOfBytes)
    {
      int num = NumberOfBytes / 128;
      if (((uint) StartAddress & 63U) > 0U)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal erase flash start address");
        throw new Exception("Illegal erase flash start address");
      }
      if ((NumberOfBytes & 63) != 0 || num > (int) byte.MaxValue)
      {
        ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.IllegalData, "Illegal number of bytes by erase flash");
        throw new Exception("Illegal number of bytes by erase flash");
      }
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 15);
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(StartAddress));
      byteList.Add((byte) num);
      byteList.Add((byte) 14);
      MBusFrame resultFrame = this.deviceCMD.MBus.Repeater.GetResultFrame(new MBusFrame(byteList.ToArray()), progress, token);
      Common16BitCommands.Common16BitLogger.Debug("EraseFlash. Address: 0x" + StartAddress.ToString("X4") + " " + NumberOfBytes.ToString("x4") + " byte(s)");
      if (resultFrame.Type != 0)
        throw new Exception("Invalid response by erase the memory! Expected: ACK, but receive: " + resultFrame.Type.ToString());
    }

    public async Task SetEmergencyModeAsync(ProgressHandler progress, CancellationToken token)
    {
      int num = await Task.Run<bool>((Func<bool>) (() => this.SetEmergencyMode(progress, token)), token) ? 1 : 0;
    }

    public bool SetEmergencyMode(ProgressHandler progress, CancellationToken token)
    {
      MBusFrame resultFrame = this.deviceCMD.MBus.Repeater.GetResultFrame(new MBusFrame(new List<byte>()
      {
        (byte) 15,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 10
      }.ToArray()), progress, token);
      Common16BitCommands.Common16BitLogger.Debug("calling SetEmergencyMode ... done. ");
      if (resultFrame.Type != 0)
        throw new Exception("Invalid response while setting emergency mode! Expected: ACK, but receive: " + resultFrame.Type.ToString());
      return true;
    }

    public async Task BackupDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      await Task.Run((Action) (() => this.BackupDevice(progress, token)), token);
    }

    public void BackupDevice(ProgressHandler progress, CancellationToken token)
    {
      if (progress == null)
        throw new ArgumentNullException(nameof (progress));
      token.ThrowIfCancellationRequested();
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 15);
      byteList.Add((byte) 0);
      byteList.Add((byte) 0);
      byteList.Add((byte) 0);
      byteList.Add((byte) 8);
      progress.Split(new double[2]{ 10.0, 90.0 });
      progress.Report("Backup device");
      MBusFrame resultFrame = this.deviceCMD.MBus.Repeater.GetResultFrame(new MBusFrame(byteList.ToArray()), progress, token);
      if (resultFrame.Type != 0)
        throw new Exception("Invalid response by backup device! Expected: ACK, but receive: " + resultFrame.Type.ToString());
    }

    public async Task ResetDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      await Task.Run((Action) (() => this.ResetDevice(progress, token)), token);
    }

    public void ResetDevice(ProgressHandler progress, CancellationToken token)
    {
      if (progress == null)
        throw new ArgumentNullException(nameof (progress));
      token.ThrowIfCancellationRequested();
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 15);
      byteList.Add((byte) 0);
      byteList.Add((byte) 0);
      byteList.Add((byte) 0);
      byteList.Add((byte) 4);
      progress.Split(new double[2]{ 10.0, 90.0 });
      progress.Report("Reset device");
      MBusFrame resultFrame = this.deviceCMD.MBus.Repeater.GetResultFrame(new MBusFrame(byteList.ToArray()), progress, token);
      if (resultFrame.Type != 0)
        throw new Exception("Invalid response by reset device! Expected: ACK, but receive: " + resultFrame.Type.ToString());
      this.deviceCMD.MBus.Repeater.Port.ForceWakeup();
    }

    private void WriteMemory(
      ProgressHandler progress,
      CancellationToken token,
      byte cmd,
      ushort address,
      byte[] buffer)
    {
      if (progress == null)
        throw new ArgumentNullException(nameof (progress));
      if (address == (ushort) 0)
        throw new ArgumentException(nameof (address));
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      token.ThrowIfCancellationRequested();
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 15);
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(address));
      byteList.Add((byte) buffer.Length);
      byteList.Add(cmd);
      byteList.AddRange((IEnumerable<byte>) buffer);
      progress.Split(new double[2]{ 10.0, 90.0 });
      progress.Report("Write: 0x" + address.ToString("X4") + " " + buffer.Length.ToString() + " byte(s)");
      MBusFrame frame = new MBusFrame(byteList.ToArray());
      MBusFrame resultFrame = this.deviceCMD.MBus.Repeater.GetResultFrame(frame, progress, token);
      if (resultFrame.Type != 0)
      {
        Common16BitCommands.Common16BitLogger.Debug("--> Write memory error!!!");
        throw new Exception("Invalid response by write the memory! Expected: ACK, but receive: " + resultFrame.Type.ToString());
      }
      Common16BitCommands.Common16BitLogger.Debug("Write memory block. Address: 0x" + address.ToString("X4") + " " + buffer.Length.ToString("x4") + " byte(s)");
      Common16BitCommands.Common16BitLogger.Debug("--> " + BitConverter.ToString(frame.ToByteArray()));
      Common16BitCommands.Common16BitLogger.Debug("--> Write memory done");
    }

    public void WriteMemory(
      ProgressHandler progress,
      CancellationToken token,
      byte cmd,
      ushort address,
      byte[] buffer,
      byte maxBytesPerPacket)
    {
      if (buffer == null || buffer.Length == 0)
        throw new ArgumentException(nameof (buffer));
      if (maxBytesPerPacket < (byte) 0)
        throw new ArgumentOutOfRangeException(nameof (maxBytesPerPacket));
      int int32 = Convert.ToInt32(buffer.Length / (int) maxBytesPerPacket);
      if (int32 <= 1 && buffer.Length <= (int) maxBytesPerPacket)
      {
        this.WriteMemory(progress, token, cmd, address, buffer);
      }
      else
      {
        byte num = (byte) ((uint) buffer.Length % (uint) maxBytesPerPacket);
        if (num > (byte) 0)
          ++int32;
        ushort address1 = address;
        byte length = maxBytesPerPacket;
        int srcOffset = 0;
        progress.Split(int32 * 2 + 1);
        for (int index = 1; index <= int32; ++index)
        {
          progress.Report("Write: 0x" + address1.ToString("X4") + " " + length.ToString() + " byte(s)");
          if (index > 1)
            address1 += (ushort) length;
          length = index * (int) maxBytesPerPacket >= buffer.Length ? (num > (byte) 0 ? num : maxBytesPerPacket) : maxBytesPerPacket;
          byte[] numArray = new byte[(int) length];
          Buffer.BlockCopy((Array) buffer, srcOffset, (Array) numArray, 0, numArray.Length);
          this.WriteMemory(progress, token, cmd, address1, numArray);
          srcOffset += numArray.Length;
        }
        if (srcOffset != buffer.Length)
          throw new Exception("Write memory failed! Written number of bytes is incorrect. Expected: " + buffer.Length.ToString() + ", Actual: " + srcOffset.ToString());
        progress.Report("Write: 0x" + address.ToString("X4") + " " + srcOffset.ToString() + " byte(s)");
      }
    }

    public async Task DeviceProtectionGetAsync(ProgressHandler progress, CancellationToken token)
    {
      int num = await Task.Run<bool>((Func<bool>) (() => this.DeviceProtectionGet(progress, token)), token) ? 1 : 0;
    }

    public bool DeviceProtectionGet(ProgressHandler progress, CancellationToken token)
    {
      MBusFrame resultFrame = this.deviceCMD.MBus.Repeater.GetResultFrame(new MBusFrame(new List<byte>()
      {
        (byte) 15,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 7
      }.ToArray()), progress, token);
      if (resultFrame.Type != 0)
        throw new Exception("Invalid response while getting device protection! Expected: ACK, but receive: " + resultFrame.Type.ToString());
      return true;
    }

    public async Task DeviceProtectionSetAsync(ProgressHandler progress, CancellationToken token)
    {
      int num = await Task.Run<bool>((Func<bool>) (() => this.DeviceProtectionSet(progress, token)), token) ? 1 : 0;
    }

    public bool DeviceProtectionSet(ProgressHandler progress, CancellationToken token)
    {
      MBusFrame resultFrame = this.deviceCMD.MBus.Repeater.GetResultFrame(new MBusFrame(new List<byte>()
      {
        (byte) 15,
        (byte) 1,
        (byte) 0,
        (byte) 0,
        (byte) 7
      }.ToArray()), progress, token);
      if (resultFrame.Type != 0)
        throw new Exception("Invalid response while setting device protection! Expected: ACK, but receive: " + resultFrame.Type.ToString());
      return true;
    }

    public async Task DeviceProtectionResetAsync(
      ProgressHandler progress,
      CancellationToken token,
      uint meterKey)
    {
      int num = await Task.Run<bool>((Func<bool>) (() => this.DeviceProtectionReset(progress, token, meterKey)), token) ? 1 : 0;
    }

    public bool DeviceProtectionReset(
      ProgressHandler progress,
      CancellationToken token,
      uint meterKey)
    {
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 15);
      byteList.Add((byte) 2);
      byteList.Add((byte) 0);
      byteList.Add((byte) 4);
      byteList.Add((byte) 7);
      for (int index = 0; index < 4; ++index)
      {
        byteList.Add((byte) meterKey);
        meterKey >>= 8;
      }
      MBusFrame resultFrame = this.deviceCMD.MBus.Repeater.GetResultFrame(new MBusFrame(byteList.ToArray()), progress, token);
      if (resultFrame.Type != 0)
        throw new Exception("Invalid response while device protection reset! Expected: ACK, but receive: " + resultFrame.Type.ToString());
      return true;
    }

    public async Task DeviceProtectionSetKeyAsync(
      ProgressHandler progress,
      CancellationToken token,
      uint meterKey)
    {
      int num = await Task.Run<bool>((Func<bool>) (() => this.DeviceProtectionSetKey(progress, token, meterKey)), token) ? 1 : 0;
    }

    public bool DeviceProtectionSetKey(
      ProgressHandler progress,
      CancellationToken token,
      uint meterKey)
    {
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 15);
      byteList.Add((byte) 3);
      byteList.Add((byte) 0);
      byteList.Add((byte) 4);
      byteList.Add((byte) 7);
      for (int index = 0; index < 4; ++index)
      {
        byteList.Add((byte) meterKey);
        meterKey >>= 8;
      }
      MBusFrame resultFrame = this.deviceCMD.MBus.Repeater.GetResultFrame(new MBusFrame(byteList.ToArray()), progress, token);
      if (resultFrame.Type != 0)
        throw new Exception("Invalid response while setting device protection key! Expected: ACK, but receive: " + resultFrame.Type.ToString());
      return true;
    }

    public async Task SetOptoTimeoutSecondsAsync(
      ProgressHandler progress,
      CancellationToken token,
      int seconds)
    {
      int num = await Task.Run<bool>((Func<bool>) (() => this.SetOptoTimeoutSeconds(progress, token, seconds)), token) ? 1 : 0;
    }

    public bool SetOptoTimeoutSeconds(
      ProgressHandler progress,
      CancellationToken token,
      int Seconds)
    {
      uint OptionByte = (uint) (Seconds / 16 + 3);
      if (OptionByte < 4U)
        OptionByte = 0U;
      if (OptionByte > (uint) byte.MaxValue)
        OptionByte = (uint) byte.MaxValue;
      bool flag = this.S3Command(progress, token, (byte) 13, (byte) OptionByte, "Send opto timeout seconds");
      if (flag && OptionByte < 4U)
        this.deviceCMD.MBus.Repeater.Port.ForceWakeup();
      return flag;
    }

    public async Task FlyingTestActivateAsync(ProgressHandler progress, CancellationToken token)
    {
      int num = await Task.Run<bool>((Func<bool>) (() => this.FlyingTestActivate(progress, token)), token) ? 1 : 0;
    }

    public bool FlyingTestActivate(ProgressHandler progress, CancellationToken token)
    {
      return this.S3Command(progress, token, (byte) 17, "Send Flying test activate");
    }

    public async Task AdcTestActivateAsync(ProgressHandler progress, CancellationToken token)
    {
      int num = await Task.Run<bool>((Func<bool>) (() => this.AdcTestActivate(progress, token)), token) ? 1 : 0;
    }

    public bool AdcTestActivate(ProgressHandler progress, CancellationToken token)
    {
      return this.S3Command(progress, token, (byte) 16, "Send ADC test activate");
    }

    public async Task FlyingTestStartAsync(ProgressHandler progress, CancellationToken token)
    {
      await Task.Run((Action) (() => this.FlyingTestStart(progress, token)), token);
    }

    public void FlyingTestStart(ProgressHandler progress, CancellationToken token)
    {
      this.S3CommandNoAnswer((byte) 18, "Send Flying test start");
    }

    public async Task FlyingTestStopAsync(ProgressHandler progress, CancellationToken token)
    {
      await Task.Run((Action) (() => this.FlyingTestStop(progress, token)), token);
    }

    public void FlyingTestStop(ProgressHandler progress, CancellationToken token)
    {
      this.S3CommandNoAnswer((byte) 19, "Send Flying test stop");
    }

    public async Task CapacityOfTestActivateAsync(ProgressHandler progress, CancellationToken token)
    {
      int num = await Task.Run<bool>((Func<bool>) (() => this.CapacityOfTestActivate(progress, token)), token) ? 1 : 0;
    }

    public bool CapacityOfTestActivate(ProgressHandler progress, CancellationToken token)
    {
      return this.S3Command(progress, token, (byte) 38, "Send capacity off test activate");
    }

    public async Task Start512HzRtcCalibrationAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      int num = await Task.Run<bool>((Func<bool>) (() => this.Start512HzRtcCalibration(progress, token)), token) ? 1 : 0;
    }

    public bool Start512HzRtcCalibration(ProgressHandler progress, CancellationToken token)
    {
      return this.S3Command(progress, token, (byte) 36, "STart RTC 512Hz calibration mode");
    }

    public bool TestDone(ProgressHandler progress, CancellationToken token, long dispValueId)
    {
      switch (dispValueId)
      {
        case -1:
          return this.S3Command(progress, token, (byte) 15, (byte) 3, "Send test done. LCD: Sleep");
        case 272769346:
          return this.S3Command(progress, token, (byte) 15, (byte) 0, "Send test done. LCD: HeadEnergy");
        case 272769355:
          return this.S3Command(progress, token, (byte) 15, (byte) 1, "Send test done. LCD: CoolingEnergy");
        default:
          return this.S3Command(progress, token, (byte) 15, (byte) 2, "Send test done. LCD: SegmentTest");
      }
    }

    internal bool S3Command(
      ProgressHandler progress,
      CancellationToken token,
      byte CommandByte,
      string NlogString)
    {
      return this.S3Command(progress, token, CommandByte, (byte) 0, NlogString);
    }

    internal bool S3Command(
      ProgressHandler progress,
      CancellationToken token,
      byte CommandByte,
      byte OptionByte,
      string NlogString)
    {
      MBusFrame resultFrame = this.deviceCMD.MBus.Repeater.GetResultFrame(new MBusFrame(new List<byte>()
      {
        (byte) 15,
        (byte) 0,
        (byte) 0,
        OptionByte,
        CommandByte
      }.ToArray()), progress, token);
      if (resultFrame.Type != 0)
        throw new Exception("Invalid response while " + NlogString + "! Expected: ACK, but receive: " + resultFrame.Type.ToString());
      return true;
    }

    internal byte[] S3CommandData(
      ProgressHandler progress,
      CancellationToken token,
      byte CommandByte,
      string NlogString)
    {
      return this.S3CommandData(progress, token, CommandByte, (byte) 0, NlogString);
    }

    internal byte[] S3CommandData(
      ProgressHandler progress,
      CancellationToken token,
      byte CommandByte,
      byte OptionByte,
      string NlogString)
    {
      return this.deviceCMD.MBus.Repeater.GetResultData(new MBusFrame(new List<byte>()
      {
        (byte) 15,
        (byte) 0,
        (byte) 0,
        OptionByte,
        CommandByte
      }.ToArray()), 504, progress, token);
    }

    internal void S3CommandNoAnswer(byte CommandByte, string NlogString)
    {
      this.deviceCMD.MBus.Repeater.TransmitMBusFrame(new MBusFrame(new List<byte>()
      {
        (byte) 15,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        CommandByte
      }.ToArray()));
    }

    public void FlyingTestReadVolume(
      ProgressHandler progress,
      CancellationToken token,
      out float volume,
      out byte state)
    {
      volume = 0.0f;
      MBusFrame resultFrame = this.deviceCMD.MBus.Repeater.GetResultFrame(new MBusFrame(new List<byte>()
      {
        (byte) 15,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 20
      }.ToArray()), progress, token);
      FixedDataHeader fixedDataHeader = resultFrame.Type == FrameType.LongFrame ? FixedDataHeader.Parse(resultFrame.UserData) : throw new Exception("Invalid response by FlyingTestReadVolume! Expected: ACK, but receive: " + resultFrame.Type.ToString());
      state = fixedDataHeader.Status;
      volume = BitConverter.ToSingle(resultFrame.UserData, 12);
    }

    public bool AdcTestCycleWithSimulatedVolume(
      ProgressHandler progress,
      CancellationToken token,
      float simulationVolume)
    {
      byte[] bytes = BitConverter.GetBytes(simulationVolume);
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 15);
      byteList.Add((byte) 0);
      byteList.Add((byte) 0);
      byteList.Add((byte) 4);
      byteList.Add((byte) 11);
      for (int index = 0; index < bytes.Length; ++index)
        byteList.Add(bytes[index]);
      MBusFrame resultFrame = this.deviceCMD.MBus.Repeater.GetResultFrame(new MBusFrame(byteList.ToArray()), progress, token);
      if (resultFrame.Type != 0)
        throw new Exception("Invalid response by AdcTestCycleWithSimulatedVolume! Expected: ACK, but receive: " + resultFrame.Type.ToString());
      return true;
    }

    public ParameterListInfo GetTransmitListInfo(ProgressHandler progress, CancellationToken token)
    {
      MBusFrame resultFrame = this.deviceCMD.MBus.Repeater.GetResultFrame(new MBusFrame(new List<byte>()
      {
        (byte) 15,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 37
      }.ToArray()), progress, token);
      return resultFrame.IsVariableDataStructure ? ParameterListInfo.Parse(VariableDataStructure.Parse(resultFrame).MfgData) : throw new ArgumentException("response does not include variable data structure");
    }

    public async Task SetRadioParametersAsync(
      ProgressHandler progress,
      CancellationToken token,
      byte listNumber,
      RADIO_MODE radioMode,
      AES_ENCRYPTION_MODE AES_Encryption,
      ushort intervallSeconds)
    {
      byte radioAndEncMode = (byte) (((int) radioMode << 4) + AES_Encryption);
      List<byte> TransmitBuffer = new List<byte>();
      TransmitBuffer.Add((byte) 15);
      TransmitBuffer.Add((byte) 0);
      TransmitBuffer.Add((byte) 0);
      TransmitBuffer.Add((byte) 4);
      TransmitBuffer.Add((byte) 42);
      TransmitBuffer.Add(listNumber);
      TransmitBuffer.Add(radioAndEncMode);
      TransmitBuffer.AddRange((IEnumerable<byte>) BitConverter.GetBytes(intervallSeconds));
      MBusFrame request = new MBusFrame(TransmitBuffer.ToArray());
      MBusFrame response = await this.deviceCMD.MBus.Repeater.GetResultFrameAsync(request, progress, token);
      if (response.Type != 0)
        throw new Exception("Invalid response while setting device protection! Expected: ACK, but receive: " + response.Type.ToString());
      TransmitBuffer = (List<byte>) null;
      request = (MBusFrame) null;
      response = (MBusFrame) null;
    }

    public byte[] SetTransmitList(
      ProgressHandler progress,
      CancellationToken token,
      ushort list,
      bool isRadio,
      ushort enc_mode,
      ushort intervallseconds)
    {
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) ParameterListInfo.GetCommandPayload(list, isRadio, enc_mode));
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(intervallseconds));
      return this.deviceCMD.MBus.Repeater.GetResultData(new MBusFrame(byteList.ToArray()), 1, progress, token);
    }

    public byte[] SetTransmitList(
      ProgressHandler progress,
      CancellationToken token,
      ushort list,
      bool isRadio,
      ushort enc_mode)
    {
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) ParameterListInfo.GetCommandPayload(list, isRadio, enc_mode));
      return this.deviceCMD.MBus.Repeater.GetResultData(new MBusFrame(byteList.ToArray()), 1, progress, token);
    }

    public byte[] GetMeterMonitorData(ProgressHandler progress, CancellationToken token)
    {
      return this.deviceCMD.MBus.Repeater.GetResultData(new MBusFrame(new List<byte>()
      {
        (byte) 15,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 5
      }.ToArray()), 504, progress, token);
    }

    public cImpulseInputCounters ReadInputCounters(
      ProgressHandler progress,
      CancellationToken token)
    {
      MBusFrame resultFrame = this.deviceCMD.MBus.Repeater.GetResultFrame(new MBusFrame(new List<byte>()
      {
        (byte) 15,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 41
      }.ToArray()), progress, token);
      if (resultFrame.Type != FrameType.LongFrame)
        throw new Exception("Invalid response by ReadInputCounters! Expected: ACK, but receive: " + resultFrame.Type.ToString());
      int length = 12;
      int count = 12;
      byte[] numArray = new byte[count];
      byte[] dst = new byte[length];
      Buffer.BlockCopy((Array) resultFrame.UserData, 0, (Array) dst, 0, length);
      Buffer.BlockCopy((Array) resultFrame.UserData, length, (Array) numArray, 0, count);
      ByteField byteField = new ByteField(numArray);
      if (byteField.Count != count)
        throw new Exception("Illegal number of bytes by ReadInputCounters");
      return new cImpulseInputCounters()
      {
        ImputState = ((eWR4_VOL_INPUT_STATE) byteField.Data[1]).ToString(),
        HardwareCounter = BitConverter.ToUInt16(byteField.Data, 2),
        VolumePulseCounter = BitConverter.ToInt16(byteField.Data, 4),
        Input0Counter = BitConverter.ToUInt16(byteField.Data, 6),
        Input1Counter = BitConverter.ToUInt16(byteField.Data, 8),
        Input2Counter = BitConverter.ToUInt16(byteField.Data, 10)
      };
    }

    public byte[] RunIoTest(
      ProgressHandler progress,
      CancellationToken token,
      eIoTestFunctions theFunction)
    {
      List<byte> byteList = new List<byte>();
      byteList.Add((byte) 15);
      byteList.Add((byte) 0);
      byteList.Add((byte) 0);
      byteList.Add((byte) theFunction);
      byteList.Add((byte) 21);
      byte[] numArray1 = new byte[1];
      MBusFrame resultFrame = this.deviceCMD.MBus.Repeater.GetResultFrame(new MBusFrame(byteList.ToArray()), progress, token);
      if (resultFrame.Type != FrameType.ACK && resultFrame.Type != FrameType.LongFrame)
        throw new Exception("Invalid response by RunIoTest! Expected: ACK, but receive: " + resultFrame.Type.ToString());
      if (theFunction != eIoTestFunctions.IoTest_Run)
        return numArray1;
      int length = 4;
      int num = resultFrame.UserData.Length - length;
      byte[] numArray2 = new byte[length];
      for (int index = 0; index < length; ++index)
        numArray2[index] = resultFrame.UserData[index + num];
      return numArray2;
    }

    public void DigitalInputsAndOutputs(
      ProgressHandler progress,
      CancellationToken token,
      uint NewOutputMask,
      uint NewOutputState,
      ref uint OldOutputState,
      ref uint OldInputState)
    {
      MBusFrame resultFrame = this.deviceCMD.MBus.Repeater.GetResultFrame(new MBusFrame(new List<byte>()
      {
        (byte) 15,
        (byte) NewOutputMask,
        (byte) NewOutputState,
        (byte) 0,
        (byte) 12
      }.ToArray()), progress, token);
      OldOutputState = (uint) resultFrame.UserData[13];
      OldInputState = (uint) resultFrame.UserData[14];
    }

    public void RadioTest(ProgressHandler progress, CancellationToken token, byte testMode)
    {
      MBusFrame resultFrame = this.deviceCMD.MBus.Repeater.GetResultFrame(new MBusFrame(new List<byte>()
      {
        (byte) 15,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        testMode
      }.ToArray()), progress, token);
      if (resultFrame.Type != FrameType.ACK && resultFrame.Type != FrameType.LongFrame)
        throw new Exception("Invalid response by RadioTest! Expected: ACK, but receive: " + resultFrame.Type.ToString());
    }

    public bool TransmitBlock(ProgressHandler progress, CancellationToken token, ref byte[] buffer)
    {
      try
      {
        Common16BitCommands.Common16BitLogger.Debug("TransmitBlock: " + Util.ByteArrayToHexString(buffer));
        this.deviceCMD.MBus.Repeater.Port.Write(buffer);
        return true;
      }
      catch (Exception ex)
      {
        throw new Exception("Invalid response by TransmitBlock! " + ex.Message);
      }
    }

    public bool SendBlock(ProgressHandler progress, CancellationToken token, ref byte[] buffer)
    {
      try
      {
        Common16BitCommands.Common16BitLogger.Debug("SendBlock: " + Util.ByteArrayToHexString(buffer));
        this.deviceCMD.MBus.Repeater.Port.Write(buffer);
        return true;
      }
      catch (Exception ex)
      {
        throw new Exception("Invalid response by SendBlock! " + ex.Message);
      }
    }

    public byte[] ReceiveBlock(
      ProgressHandler progress,
      CancellationToken token,
      int MinByteNb,
      bool first)
    {
      byte[] numArray = new byte[0];
      byte[] buffer = !first ? this.deviceCMD.MBus.Repeater.Port.ReadEnd(MinByteNb) : this.deviceCMD.MBus.Repeater.Port.ReadHeader(MinByteNb);
      Common16BitCommands.Common16BitLogger.Debug("ReceiveBlock: " + Util.ByteArrayToHexString(buffer));
      return buffer;
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

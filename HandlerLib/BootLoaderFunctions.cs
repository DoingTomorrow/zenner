// Decompiled with JetBrains decompiler
// Type: HandlerLib.BootLoaderFunctions
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommunicationPort.Functions;
using ReadoutConfiguration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Exceptions;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class BootLoaderFunctions
  {
    private CommunicationPortFunctions myPort = (CommunicationPortFunctions) null;
    private FirmwareUpdateToolDeviceCommands DeviceCMD = (FirmwareUpdateToolDeviceCommands) null;
    public byte[] response;
    public bool isIUW = false;
    private Stopwatch watch;

    public BootLoaderFunctions(
      CommunicationPortFunctions port,
      FirmwareUpdateToolDeviceCommands fwUpDevCMD = null)
    {
      this.myPort = port;
      this.myPort.OnResponse += new EventHandler<byte[]>(this.myPort_OnResponse);
      this.isIUW = false;
      ConfigList readoutConfiguration = this.myPort.GetReadoutConfiguration();
      bool flag = Enum.IsDefined(typeof (BTL_NON_NFC_Devices), (object) new ConnectionProfileIdentification(readoutConfiguration.ConnectionProfileID).DeviceModelID);
      if (readoutConfiguration.BusMode != null)
        this.isIUW = readoutConfiguration.BusMode.Contains("NFC") && !flag;
      this.DeviceCMD = fwUpDevCMD;
      this.watch = new Stopwatch();
    }

    private void myPort_OnResponse(object sender, byte[] e)
    {
      this.response = new byte[e.Length];
      if (e.Length == 0)
        return;
      Buffer.BlockCopy((Array) e, 0, (Array) this.response, 0, e.Length);
    }

    public async Task writeMemoryAsync(
      ProgressHandler progress,
      CancellationTokenSource cancel,
      uint address,
      byte[] data)
    {
      if (this.isIUW)
      {
        if (address >= 134742016U)
          this.DeviceCMD.SetBlockMode(false, 64U);
        else
          this.DeviceCMD.SetBlockMode();
        await this.DeviceCMD.WriteMemoryAsync(progress, cancel.Token, address, data);
      }
      else
        await Task.Run((Action) (() => this.writeMemory(progress, cancel, address, data)));
    }

    private void writeMemory(
      ProgressHandler progress,
      CancellationTokenSource cancel,
      uint address,
      byte[] data)
    {
      if (progress == null)
        throw new ArgumentNullException(nameof (progress));
      if (cancel == null)
        throw new ArgumentNullException("token");
      if (address == 0U)
        throw new ArgumentException(nameof (address));
      if (data == null)
        throw new ArgumentNullException("buffer");
      cancel.Token.ThrowIfCancellationRequested();
      byte[] numArray = new byte[3];
      Buffer.BlockCopy((Array) BitConverter.GetBytes(data.Length), 0, (Array) numArray, 0, 3);
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(address));
      byteList.AddRange((IEnumerable<byte>) numArray);
      byteList.AddRange((IEnumerable<byte>) data);
      this.TransmitAndCheckAck(BootLoader_FC.WRITE_MEMORY, byteList.ToArray(), progress, cancel.Token, BTL_MEM_ERROR.ERROR_ADR);
    }

    public async Task<byte[]> readMemoryAsync(
      ProgressHandler progress,
      CancellationTokenSource cancel,
      uint address,
      uint count,
      bool overrideIUW = true)
    {
      if (overrideIUW && this.isIUW)
      {
        this.DeviceCMD.SetBlockMode();
        byte[] numArray = await this.DeviceCMD.ReadMemoryAsync(progress, cancel.Token, address, count);
        return numArray;
      }
      byte[] numArray1 = await Task.Run<byte[]>((Func<byte[]>) (() => this.readMemory(progress, cancel, address, count)));
      return numArray1;
    }

    internal byte[] readMemory(
      ProgressHandler progress,
      CancellationTokenSource cancel,
      uint address,
      uint count)
    {
      if (progress == null)
        throw new ArgumentNullException(nameof (progress));
      if (cancel == null)
        throw new ArgumentNullException("token");
      if (count <= 0U)
        throw new ArgumentOutOfRangeException(nameof (count));
      cancel.Token.ThrowIfCancellationRequested();
      byte[] numArray = new byte[3];
      Buffer.BlockCopy((Array) BitConverter.GetBytes(count), 0, (Array) numArray, 0, 3);
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(address));
      byteList.AddRange((IEnumerable<byte>) numArray);
      byte[] data = this.TransmitAndGetData(BootLoader_FC.READ_MEMORY, byteList.ToArray(), false, progress, cancel.Token);
      byte[] dst = new byte[(int) count];
      Buffer.BlockCopy((Array) data, 7, (Array) dst, 0, (int) count);
      if ((long) dst.Length != (long) count)
        throw new Exception("Invalid response by read the memory! Expected: " + count.ToString() + " bytes but received: " + data.Length.ToString() + " byte(s)");
      return dst;
    }

    internal uint verifyMemory(uint startAddress, uint endAddress)
    {
      if (startAddress <= 0U)
        throw new ArgumentOutOfRangeException(nameof (startAddress));
      if (endAddress <= 0U)
        throw new ArgumentOutOfRangeException(nameof (endAddress));
      if (!this.isIUW)
        return 0;
      this.DeviceCMD.SetBlockMode();
      return (uint) this.DeviceCMD.VerifyMemory(startAddress, endAddress);
    }

    internal async Task<uint> verifyMemoryAsync(
      ProgressHandler progress,
      CancellationTokenSource cancel,
      uint startAddress,
      uint endAddress)
    {
      if (progress == null)
        throw new ArgumentNullException(nameof (progress));
      if (cancel == null)
        throw new ArgumentNullException("token");
      if (startAddress <= 0U)
        throw new ArgumentOutOfRangeException(nameof (startAddress));
      if (endAddress <= 0U)
        throw new ArgumentOutOfRangeException(nameof (endAddress));
      cancel.Token.ThrowIfCancellationRequested();
      if (!this.isIUW)
        return 0;
      this.DeviceCMD.SetBlockMode();
      ushort num = await this.DeviceCMD.VerifyMemoryAsync(progress, cancel.Token, startAddress, endAddress);
      return (uint) num;
    }

    public async Task eraseMemoryAsync(
      ProgressHandler progress,
      CancellationTokenSource cancel,
      uint address,
      uint count)
    {
      if (this.isIUW)
        ;
      else
        await Task.Run((Action) (() => this.eraseMemory(progress, cancel, address, count)));
    }

    private void eraseMemory(
      ProgressHandler progress,
      CancellationTokenSource cancel,
      uint address,
      uint count)
    {
      if (progress == null)
        throw new ArgumentNullException(nameof (progress));
      if (cancel == null)
        throw new ArgumentNullException("token");
      if (count <= 0U)
        throw new ArgumentOutOfRangeException(nameof (count));
      cancel.Token.ThrowIfCancellationRequested();
      byte[] numArray = new byte[3];
      Buffer.BlockCopy((Array) BitConverter.GetBytes(count), 0, (Array) numArray, 0, 3);
      List<byte> byteList = new List<byte>();
      byteList.AddRange((IEnumerable<byte>) BitConverter.GetBytes(address));
      byteList.AddRange((IEnumerable<byte>) numArray);
      this.TransmitAndCheckAck(BootLoader_FC.ERASE_MEMORY, byteList.ToArray(), progress, cancel.Token, BTL_MEM_ERROR.ERROR_ADR);
    }

    public async Task<byte[]> getVersionAsync(
      ProgressHandler progress,
      CancellationTokenSource cancel)
    {
      if (this.isIUW)
      {
        byte[] versionAsync = await this.DeviceCMD.getVersionAsync(progress, cancel.Token);
        return versionAsync;
      }
      byte[] versionAsync1 = await Task.Run<byte[]>((Func<byte[]>) (() => this.getVersion(progress, cancel)));
      return versionAsync1;
    }

    private byte[] getVersion(ProgressHandler progress, CancellationTokenSource cancel)
    {
      int num = 3;
      if (progress == null)
        throw new ArgumentNullException(nameof (progress));
      if (cancel == null)
        throw new ArgumentNullException("token");
      cancel.Token.ThrowIfCancellationRequested();
      progress.Split(num + 1);
      for (int index = 0; index < num; ++index)
      {
        progress.Report("Try to connect ... " + (index + 1).ToString());
        byte[] data = this.TransmitAndGetData(BootLoader_FC.GET_VERSION, (byte[]) null, false, progress, cancel.Token);
        if (data.Length != 0)
          return data;
        Thread.Sleep(500);
      }
      throw new Exception("No answer after " + num.ToString() + " attempts, please check connection to BOOTLOADER ...");
    }

    public async Task GO_Async(ProgressHandler progress, CancellationTokenSource cancel)
    {
      if (this.isIUW)
        await this.DeviceCMD.ResetDeviceAsync(progress, cancel.Token);
      else
        await Task.Run((Action) (() => this.GO(progress, cancel)));
    }

    private void GO(ProgressHandler progress, CancellationTokenSource cancel)
    {
      if (progress == null)
        throw new ArgumentNullException(nameof (progress));
      if (cancel == null)
        throw new ArgumentNullException("token");
      cancel.Token.ThrowIfCancellationRequested();
      this.TransmitAndCheckAck(BootLoader_FC.GO, (byte[]) null, progress, cancel.Token);
    }

    public async Task TransmitAndCheckAckAsync(
      BootLoader_FC FC,
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken,
      BTL_MEM_ERROR typ = BTL_MEM_ERROR.ERROR_NO)
    {
      await Task.Run((Action) (() => this.TransmitAndCheckAck(FC, data, progress, cancelToken, typ)));
    }

    private void TransmitAndCheckAck(
      BootLoader_FC FC,
      byte[] data,
      ProgressHandler progress,
      CancellationToken cancelToken,
      BTL_MEM_ERROR typ = BTL_MEM_ERROR.ERROR_NO)
    {
      byte[] data1 = this.TransmitAndGetData(FC, data, true, progress, cancelToken);
      if (data1 == null || data1.Length > 1)
        throw new Exception("Illegal ACK data received.");
      if (data1.Length > 2)
        throw new Exception("Too much ACK data.");
      if (data1[0] != (byte) 229 && (BTL_MEM_ERROR) data1[0] != typ)
      {
        foreach (byte num in Enum.GetValues(typeof (BTL_MEM_ERROR)))
        {
          if ((int) data1[0] == (int) num)
            throw new Exception("Error received from device. Type: " + Enum.GetName(typeof (BTL_MEM_ERROR), (object) num));
        }
        throw new Exception("Error while incorrect return error typ from device.");
      }
    }

    public async Task<byte[]> TransmitAndGetDataAsync(
      BootLoader_FC FC,
      byte[] parameter,
      bool ACK_allowed,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] dataAsync = await Task.Run<byte[]>((Func<byte[]>) (() => this.TransmitAndGetData(FC, parameter, false, progress, cancelToken)));
      return dataAsync;
    }

    private byte[] TransmitAndGetData(
      BootLoader_FC FC,
      byte[] parameterBytes,
      bool ACK_allowed,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      int num1 = 0;
      if (parameterBytes != null)
        num1 = parameterBytes.Length;
      int dstOffset = 0;
      int num2 = 0;
      int length = num1 + 4;
      byte[] numArray1 = new byte[length];
      byte[] numArray2 = new byte[4]
      {
        (byte) 104,
        BitConverter.GetBytes(length)[0],
        BitConverter.GetBytes(length)[1],
        (byte) 104
      };
      byte[] numArray3 = new byte[length + numArray2.Length];
      Buffer.BlockCopy((Array) numArray2, 0, (Array) numArray3, dstOffset, numArray2.Length);
      int num3 = dstOffset + numArray2.Length;
      byte[] numArray4 = numArray1;
      int index1 = num2;
      int num4 = index1 + 1;
      int num5 = (int) FC;
      numArray4[index1] = (byte) num5;
      if (parameterBytes != null)
      {
        for (int index2 = 0; index2 < num1; ++index2)
          numArray1[num4++] = parameterBytes[index2];
      }
      byte[] numArray5 = new byte[length - 3];
      Buffer.BlockCopy((Array) numArray1, 0, (Array) numArray5, 0, numArray5.Length);
      byte[] bytes = BitConverter.GetBytes(CRC.CRC_CCITT(numArray5));
      byte[] numArray6 = numArray1;
      int index3 = num4;
      int num6 = index3 + 1;
      int num7 = (int) bytes[0];
      numArray6[index3] = (byte) num7;
      byte[] numArray7 = numArray1;
      int index4 = num6;
      int num8 = index4 + 1;
      int num9 = (int) bytes[1];
      numArray7[index4] = (byte) num9;
      byte[] numArray8 = numArray1;
      int index5 = num8;
      int num10 = index5 + 1;
      numArray8[index5] = (byte) 22;
      if (numArray1 != null)
      {
        for (int index6 = 0; index6 < numArray1.Length; ++index6)
          numArray3[num3++] = numArray1[index6];
      }
      this.response = new byte[0];
      if (!this.myPort.IsOpen)
        this.myPort.Open();
      this.myPort.Write(numArray3);
      this.response = this.myPort.ReadHeader(1);
      if (ACK_allowed && this.response[0] != (byte) 104)
        return this.response;
      if (this.response[0] == (byte) 26)
        throw new NACK_Exception("(NACK)Not acknowledged command or data received, please check ...");
      List<byte> source = new List<byte>();
      source.AddRange((IEnumerable<byte>) this.response);
      source.AddRange((IEnumerable<byte>) this.myPort.ReadEnd(3));
      this.response = source.ToArray<byte>();
      ushort uint16 = BitConverter.ToUInt16(this.response, 1);
      if (uint16 > (ushort) 0)
      {
        this.response = this.myPort.ReadEnd((int) uint16);
        if (this.response.Length != 0)
        {
          if (ACK_allowed && (BootLoader_FC) (this.response[0] ^= (byte) 128) != FC)
            throw new NACK_Exception("COMMAND error ...\nPlease check command.");
          byte[] dst = new byte[(int) uint16 - 4];
          Buffer.BlockCopy((Array) this.response, 1, (Array) dst, 0, dst.Length);
          return dst;
        }
      }
      progress.Report("Error writing bytes to device ...");
      throw new Exception("Error while writing bytes to device.\nNO or wrong answer from device.\nHead: " + Util.ByteArrayToString(numArray2) + "\nPayLoad: " + Util.ByteArrayToString(numArray1) + "\nResponse:" + Util.ByteArrayToString(this.response));
    }
  }
}

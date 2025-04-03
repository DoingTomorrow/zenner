// Decompiled with JetBrains decompiler
// Type: HandlerLib.NFC.NfcSubunitCommands
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommunicationPort.Functions;
using NLog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib.NFC
{
  public class NfcSubunitCommands
  {
    protected static Logger MiConConnectorLogger = LogManager.GetLogger("MiConConnector");
    private NfcRepeater myRepeater;
    public string MiConConnectorIdentification;

    public NfcSubunitCommands(CommunicationPortFunctions port)
    {
      this.myRepeater = new NfcRepeater(port);
    }

    private async Task<byte[]> WorkSubunitCommand(
      SubunitCommands command,
      ProgressHandler progress,
      CancellationToken cancelToken,
      byte?[] data = null)
    {
      byte[] cmd;
      if (data == null)
      {
        cmd = new byte[1]{ (byte) command };
      }
      else
      {
        cmd = new byte[data.Length + 1];
        for (int i = 0; i < data.Length; ++i)
          cmd[i + 1] = data[i].Value;
        cmd[0] = (byte) command;
      }
      string str = command.ToString();
      NfcFrame nfcFrame = new NfcFrame(NfcCommands.SubUnitCommand, cmd, this.myRepeater.myConfig.ReadingChannelIdentification);
      NfcSubunitCommands.MiConConnectorLogger.Trace("Send " + str + " frame: " + Util.ByteArrayToHexStringFormated(nfcFrame.NfcRequestFrame));
      await this.myRepeater.GetResultFrameAsync(nfcFrame, progress, cancelToken);
      byte[] nfcResponseFrame = nfcFrame.NfcResponseFrame;
      cmd = (byte[]) null;
      str = (string) null;
      nfcFrame = (NfcFrame) null;
      return nfcResponseFrame;
    }

    public async Task<string> GetEchoAsync(ProgressHandler progress, CancellationToken cancelToken)
    {
      byte[] NfcResponseFrame = await this.WorkSubunitCommand(SubunitCommands.Coup_Echo, progress, cancelToken);
      string hexString = Util.ByteArrayToHexString(NfcResponseFrame);
      NfcResponseFrame = (byte[]) null;
      return hexString;
    }

    public async Task<string> ReadIdentificationAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] NfcResponseFrame = await this.WorkSubunitCommand(SubunitCommands.Con_GetIdent, progress, cancelToken);
      this.MiConConnectorIdentification = Util.ByteArrayToHexString(NfcResponseFrame);
      string connectorIdentification = this.MiConConnectorIdentification;
      NfcResponseFrame = (byte[]) null;
      return connectorIdentification;
    }

    public async Task<MiConConnectorVersion> ReadMiConIdentificationAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] NfcResponseFrame = await this.WorkSubunitCommand(SubunitCommands.Con_GetIdent, progress, cancelToken);
      MiConConnectorVersion nfcDI = new MiConConnectorVersion(NfcResponseFrame);
      MiConConnectorVersion connectorVersion = nfcDI;
      NfcResponseFrame = (byte[]) null;
      nfcDI = (MiConConnectorVersion) null;
      return connectorVersion;
    }

    public async Task WriteMiConIdentificationAsync(
      byte?[] ident,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] NfcResponseFrame = await this.WorkSubunitCommand(SubunitCommands.Con_SetIdent, progress, cancelToken, ident);
      NfcResponseFrame = (byte[]) null;
    }

    public async Task WriteMiConMeterIDAsync(
      uint meterID,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await Task.Delay(1);
      throw new Exception("Not Implemented YET !!!");
    }

    public async Task<string> ReadCouplerIdentificationAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] NfcResponseFrame = await this.WorkSubunitCommand(SubunitCommands.Coup_Ident, progress, cancelToken);
      this.MiConConnectorIdentification = Util.ByteArrayToHexString(NfcResponseFrame);
      string str = Util.ByteArrayToString(Util.HexStringToByteArray(this.MiConConnectorIdentification)).Substring(7, 8);
      NfcResponseFrame = (byte[]) null;
      return str;
    }

    public async Task<NdcMiConModuleHardwareIds> ReadNdcMiConModuleHardwareIds(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      NdcMiConModuleHardwareIds theIds = new NdcMiConModuleHardwareIds();
      NdcMiConModuleHardwareIds moduleHardwareIds1 = theIds;
      MiConConnectorVersion connectorVersion = await this.ReadMiConIdentificationAsync(progress, cancelToken);
      moduleHardwareIds1.MiConConnectorID = Util.ByteArrayToHexString(connectorVersion.Unique_ID);
      moduleHardwareIds1 = (NdcMiConModuleHardwareIds) null;
      connectorVersion = (MiConConnectorVersion) null;
      NdcMiConModuleHardwareIds moduleHardwareIds2 = theIds;
      string str = await this.ReadCouplerIdentificationAsync(progress, cancelToken);
      moduleHardwareIds2.NfcCouplerID = str;
      moduleHardwareIds2 = (NdcMiConModuleHardwareIds) null;
      str = (string) null;
      NdcMiConModuleHardwareIds moduleHardwareIds = theIds;
      theIds = (NdcMiConModuleHardwareIds) null;
      return moduleHardwareIds;
    }

    public async Task<byte[]> SetRfOnAsync(ProgressHandler progress, CancellationToken cancelToken)
    {
      byte?[] rfOn = new byte?[1]{ new byte?((byte) 2) };
      byte[] numArray = await this.WorkSubunitCommand(SubunitCommands.Coup_SetRF, progress, cancelToken, rfOn);
      rfOn = (byte?[]) null;
      return numArray;
    }

    public async Task<byte[]> SetRfOffAsync(ProgressHandler progress, CancellationToken cancelToken)
    {
      byte?[] rfOff = new byte?[1]{ new byte?((byte) 0) };
      byte[] numArray = await this.WorkSubunitCommand(SubunitCommands.Coup_SetRF, progress, cancelToken, rfOff);
      rfOff = (byte?[]) null;
      return numArray;
    }

    public async Task<byte[]> NFC_AnticollisionAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] numArray = await this.WorkSubunitCommand(SubunitCommands.NFC_Anticollision, progress, cancelToken);
      return numArray;
    }

    public async Task<byte[]> NFC_GetTagIdentAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] tagIdentAsync = await this.WorkSubunitCommand(SubunitCommands.NFC_GetTagIdent, progress, cancelToken);
      return tagIdentAsync;
    }

    public async Task<byte[]> NFC_GetTagStatusAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte?[] getStatus = new byte?[2]
      {
        new byte?((byte) 248),
        new byte?((byte) 3)
      };
      byte[] tagStatusAsync = await this.WorkSubunitCommand(SubunitCommands.NFC_GetTagStatus, progress, cancelToken, getStatus);
      getStatus = (byte?[]) null;
      return tagStatusAsync;
    }

    public async Task<byte[]> SetTestOff(ProgressHandler progress, CancellationToken cancelToken)
    {
      byte?[] testOff = new byte?[1]{ new byte?((byte) 0) };
      byte[] numArray = await this.WorkSubunitCommand(SubunitCommands.Con_SetTest, progress, cancelToken, testOff);
      testOff = (byte?[]) null;
      return numArray;
    }

    public async Task<byte[]> SetTestStart(
      byte[] NFC_Frame,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte?[] testStart = new byte?[NFC_Frame.Length + 1];
      for (int i = 0; i < NFC_Frame.Length; ++i)
        testStart[i + 1] = new byte?(NFC_Frame[i]);
      testStart[0] = new byte?((byte) 1);
      byte[] numArray = await this.WorkSubunitCommand(SubunitCommands.Con_SetTest, progress, cancelToken, testStart);
      testStart = (byte?[]) null;
      return numArray;
    }

    public async Task<byte[]> SetTestStop(
      byte[] NFC_Frame,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte?[] testStop = new byte?[NFC_Frame.Length + 1];
      for (int i = 0; i < NFC_Frame.Length; ++i)
        testStop[i + 1] = new byte?(NFC_Frame[i]);
      testStop[0] = new byte?((byte) 2);
      byte[] numArray = await this.WorkSubunitCommand(SubunitCommands.Con_SetTest, progress, cancelToken, testStop);
      testStop = (byte?[]) null;
      return numArray;
    }

    public async Task<byte[]> StartCouplerCurrentMeasurement(
      ushort cycleTime,
      ushort startCycleOffset,
      ushort logCycles,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte?[] startCurrMeas = new byte?[6]
      {
        new byte?((byte) cycleTime),
        new byte?((byte) ((uint) cycleTime >> 8)),
        new byte?((byte) startCycleOffset),
        new byte?((byte) ((uint) startCycleOffset >> 8)),
        new byte?((byte) logCycles),
        new byte?((byte) ((uint) logCycles >> 8))
      };
      byte[] numArray = await this.WorkSubunitCommand(SubunitCommands.Con_MeasCouplerCurrent, progress, cancelToken, startCurrMeas);
      startCurrMeas = (byte?[]) null;
      return numArray;
    }

    public async Task<ushort[]> GetCouplerCurrentValues(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] resultBytes = await this.WorkSubunitCommand(SubunitCommands.Con_GetCurrentValues, progress, cancelToken);
      ushort offset = 3;
      ushort size = (ushort) ((resultBytes.Length - 5) / 2);
      ushort[] currentSamples = new ushort[(int) size];
      for (ushort i = 0; (int) size - (int) i > 0; ++i)
        currentSamples[(int) i] = (ushort) ((uint) resultBytes[(int) offset + (2 * (int) i + 1)] << 8 | (uint) resultBytes[(int) offset + 2 * (int) i]);
      ushort[] couplerCurrentValues = currentSamples;
      resultBytes = (byte[]) null;
      currentSamples = (ushort[]) null;
      return couplerCurrentValues;
    }

    public async Task MiConConnector_Reset(ProgressHandler progress, CancellationToken cancelToken)
    {
      byte[] numArray = await this.WorkSubunitCommand(SubunitCommands.Con_ResetDevice, progress, cancelToken);
    }

    public async Task<byte[]> ReadNdcMemory_Async(
      uint startAddress,
      uint byteSize,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (byteSize <= 0U)
        throw new Exception("Please Check MemoryAddresses. The Endaddress is lower or equal to Startaddress!!!");
      int count = (int) (byteSize / 55U);
      if (byteSize % 55U > 0U)
        ++count;
      progress.Split(count);
      uint readAddress = startAddress;
      byte[] ReceivedMemory = new byte[(int) byteSize];
      do
      {
        uint blockSize = byteSize - (readAddress - startAddress);
        if (blockSize > 55U)
          blockSize = 55U;
        byte[] NfcResponseFrame = await this.ReadNdcMemoryBlock_Async(readAddress, blockSize, progress, cancelToken);
        Buffer.BlockCopy((Array) NfcResponseFrame, 7, (Array) ReceivedMemory, (int) readAddress - (int) startAddress, (int) blockSize);
        readAddress += blockSize;
        NfcResponseFrame = (byte[]) null;
      }
      while (readAddress < startAddress + byteSize);
      byte[] numArray = ReceivedMemory;
      ReceivedMemory = (byte[]) null;
      return numArray;
    }

    private async Task<byte[]> ReadNdcMemoryBlock_Async(
      uint readAddress,
      uint blockSize,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] addr = BitConverter.GetBytes(readAddress);
      byte[] size = BitConverter.GetBytes(blockSize);
      byte[] frameData = new byte[6];
      Buffer.BlockCopy((Array) addr, 0, (Array) frameData, 0, 4);
      Buffer.BlockCopy((Array) size, 0, (Array) frameData, 4, 2);
      byte[] numArray = await this.WorkSubunitCommand(SubunitCommands.Con_ReadMemory, progress, cancelToken, Enumerable.Cast<byte?>(frameData).ToArray<byte?>());
      addr = (byte[]) null;
      size = (byte[]) null;
      frameData = (byte[]) null;
      return numArray;
    }

    public async Task<byte[]> SubUnit_WriteMemory_Async(
      uint startAddress,
      byte[] WriteMemory,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (WriteMemory == null || WriteMemory.Length == 0)
        throw new Exception("No data defined for write operation");
      uint writeAddress = startAddress;
      byte[] NfcResponseFrame = new byte[64];
      byte[] MemoryToTransmit = new byte[64];
      do
      {
        uint blockSize = (uint) (WriteMemory.Length - ((int) writeAddress - (int) startAddress));
        if (blockSize > 50U)
          blockSize = 50U;
        Buffer.BlockCopy((Array) WriteMemory, (int) writeAddress - (int) startAddress, (Array) MemoryToTransmit, 0, (int) blockSize);
        NfcResponseFrame = await this.SubUnit_WriteMemoryBlock_Async(writeAddress, MemoryToTransmit, blockSize, progress, cancelToken);
        int lenData = (int) NfcResponseFrame[0];
        if (lenData != 8)
          throw new Exception("Write failed!!!");
        writeAddress += blockSize;
      }
      while ((long) writeAddress < (long) startAddress + (long) WriteMemory.Length);
      byte[] numArray = NfcResponseFrame;
      NfcResponseFrame = (byte[]) null;
      MemoryToTransmit = (byte[]) null;
      return numArray;
    }

    private async Task<byte[]> SubUnit_WriteMemoryBlock_Async(
      uint writeAddress,
      byte[] MemoryToTransmit,
      uint blockSize,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] addr = BitConverter.GetBytes(writeAddress);
      byte[] frameData = new byte[(int) blockSize + 4];
      Buffer.BlockCopy((Array) addr, 0, (Array) frameData, 0, 4);
      Buffer.BlockCopy((Array) MemoryToTransmit, 0, (Array) frameData, 4, (int) blockSize);
      byte[] numArray = await this.WorkSubunitCommand(SubunitCommands.Con_WriteMemory, progress, cancelToken, Enumerable.Cast<byte?>(frameData).ToArray<byte?>());
      addr = (byte[]) null;
      frameData = (byte[]) null;
      return numArray;
    }

    public async Task<byte[]> NDC_USB_StartBootloader(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] numArray = await this.WorkSubunitCommand(SubunitCommands.Con_NDC_USB_StartBootloader, progress, cancelToken);
      return numArray;
    }
  }
}

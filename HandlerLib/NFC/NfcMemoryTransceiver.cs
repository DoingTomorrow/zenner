// Decompiled with JetBrains decompiler
// Type: HandlerLib.NFC.NfcMemoryTransceiver
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib.NFC
{
  public class NfcMemoryTransceiver
  {
    private NfcRepeater myRepeater;
    private uint maxBufferSize = 64;

    public uint MaxBufferSize
    {
      get => this.maxBufferSize;
      set
      {
        this.maxBufferSize = value >= 64U && value <= 512U ? value : throw new Exception("Illegal MaxBufferSize: " + value.ToString());
      }
    }

    private uint MaxBlockSize
    {
      get => this.MaxBufferSize <= 64U ? this.MaxBufferSize - 9U : this.MaxBufferSize - 11U;
    }

    public NfcMemoryTransceiver(NfcRepeater myRepeater) => this.myRepeater = myRepeater;

    public async Task<byte[]> ReadMemoryAsync(
      uint startAddress,
      uint byteSize,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (byteSize == 0U)
        return new byte[0];
      int count = (int) (byteSize / this.MaxBlockSize);
      if (byteSize % this.MaxBlockSize > 0U)
        ++count;
      progress.Split(count);
      uint readAddress = startAddress;
      byte[] ReceivedMemory = new byte[(int) byteSize];
      do
      {
        uint blockSize = byteSize - (readAddress - startAddress);
        if (blockSize > this.MaxBlockSize)
          blockSize = this.MaxBlockSize;
        byte[] NfcResponseFrame = await this.ReadMemoryBlockAsync(readAddress, blockSize, progress, cancelToken);
        if ((long) NfcResponseFrame.Length == (long) (blockSize + 8U))
          Buffer.BlockCopy((Array) NfcResponseFrame, 6, (Array) ReceivedMemory, (int) readAddress - (int) startAddress, (int) blockSize);
        else if ((long) NfcResponseFrame.Length == (long) (blockSize + 10U))
        {
          if (NfcResponseFrame[0] != byte.MaxValue)
          {
            string ErrorText = "Read memory block failed. Long frame data length without 16 bit address coding";
            this.myRepeater.NfcRepeaterLogger.Error(ErrorText);
            throw new Exception(ErrorText);
          }
          Buffer.BlockCopy((Array) NfcResponseFrame, 8, (Array) ReceivedMemory, (int) readAddress - (int) startAddress, (int) blockSize);
        }
        else
        {
          string ErrorText = "Read memory block failed. Illegal result data length.";
          this.myRepeater.NfcRepeaterLogger.Error(ErrorText);
          throw new Exception(ErrorText);
        }
        readAddress += blockSize;
        NfcResponseFrame = (byte[]) null;
      }
      while (readAddress < startAddress + byteSize);
      return ReceivedMemory;
    }

    private async Task<byte[]> ReadMemoryBlockAsync(
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
      NfcFrame nfcFrame = new NfcFrame(NfcCommands.ReadMemory, frameData, this.myRepeater.myConfig.ReadingChannelIdentification, this.myRepeater.CrcInitValue);
      nfcFrame.ProgressParameter = " 0x" + readAddress.ToString("x08");
      int readTimeOffset = ((int) blockSize - 64) * 2;
      await this.myRepeater.GetResultFrameAsync(nfcFrame, progress, cancelToken, readTimeOffset);
      byte[] nfcResponseFrame = nfcFrame.NfcResponseFrame;
      addr = (byte[]) null;
      size = (byte[]) null;
      frameData = (byte[]) null;
      nfcFrame = (NfcFrame) null;
      return nfcResponseFrame;
    }

    public async Task<byte[]> WriteMemoryAsync(
      uint startAddress,
      byte[] WriteMemory,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      if (WriteMemory == null || WriteMemory.Length == 0)
        throw new Exception("No data defined for write operation");
      if (startAddress > 134742016U)
        this.maxBufferSize = 64U;
      uint writeAddress = startAddress;
      byte[] NfcResponseFrame = new byte[(int) this.MaxBufferSize];
      byte[] MemoryToTransmit = new byte[(int) this.MaxBufferSize];
      do
      {
        uint blockSize = (uint) (WriteMemory.Length - ((int) writeAddress - (int) startAddress));
        if (blockSize > this.MaxBlockSize)
          blockSize = this.MaxBlockSize;
        Buffer.BlockCopy((Array) WriteMemory, (int) writeAddress - (int) startAddress, (Array) MemoryToTransmit, 0, (int) blockSize);
        NfcResponseFrame = await this.WriteMemoryBlockAsync(writeAddress, MemoryToTransmit, blockSize, progress, cancelToken);
        int lenData = (int) NfcResponseFrame[0];
        if (lenData != 7)
          throw new Exception("Write failed!!!");
        writeAddress += blockSize;
      }
      while ((long) writeAddress < (long) startAddress + (long) WriteMemory.Length);
      byte[] numArray = NfcResponseFrame;
      NfcResponseFrame = (byte[]) null;
      MemoryToTransmit = (byte[]) null;
      return numArray;
    }

    private async Task<byte[]> WriteMemoryBlockAsync(
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
      NfcFrame nfcFrame = new NfcFrame(NfcCommands.WriteMemory, frameData, this.myRepeater.myConfig.ReadingChannelIdentification, this.myRepeater.CrcInitValue);
      nfcFrame.ProgressParameter = " 0x" + writeAddress.ToString("x08");
      await this.myRepeater.GetResultFrameAsync(nfcFrame, progress, cancelToken);
      byte[] nfcResponseFrame = nfcFrame.NfcResponseFrame;
      addr = (byte[]) null;
      frameData = (byte[]) null;
      nfcFrame = (NfcFrame) null;
      return nfcResponseFrame;
    }
  }
}

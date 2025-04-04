// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_ModuleCommands
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.Functions
{
  internal class S4_ModuleCommands : BaseMemoryAccess
  {
    public static Logger Base_ModuleCommandsLogger = LogManager.GetLogger(nameof (S4_ModuleCommands));
    public ChannelLogger ModuleCommandsLogger;
    private NfcDeviceCommands CommonNfcCommands;
    private BusModuleInfo ModuleInfo;
    private const int MaxBlockSize = 500;

    internal S4_ModuleCommands(S4_DeviceCommandsNFC myDeviceCommands, BusModuleInfo moduleInfo)
    {
      this.CommonNfcCommands = myDeviceCommands.CommonNfcCommands;
      this.ModuleInfo = moduleInfo;
      this.ModuleCommandsLogger = new ChannelLogger(S4_ModuleCommands.Base_ModuleCommandsLogger, this.CommonNfcCommands.myNfcRepeater.myConfig);
    }

    internal async Task GetMemoryRangesAsync(
      DeviceMemory theMemory,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.ModuleCommandsLogger.Debug("GetMemoryRanges");
      byte[] transmitData = new byte[1]{ (byte) 9 };
      byte[] result = await this.CommonNfcCommands.SendTransparentToModuleAsync(progress, cancelToken, this.ModuleInfo, BusModuleCommand.BUS_ASYNC_TRANSPARENT_TO_MODULE, transmitData);
      int resultOffset = 0;
      while (result.Length - resultOffset < 7)
      {
        uint startAddress = ByteArrayScanner.ScanUInt32(result, ref resultOffset);
        uint byteSize = (uint) ByteArrayScanner.ScanUInt16(result, ref resultOffset);
        ReadPartsSelection readSelection = (ReadPartsSelection) ByteArrayScanner.ScanUInt32(result, ref resultOffset);
        theMemory.AddMemoryBlock(readSelection, startAddress, byteSize);
      }
      transmitData = (byte[]) null;
      result = (byte[]) null;
    }

    public override async Task ReadMemoryAsync(
      AddressRange addressRange,
      DeviceMemory deviceMemory,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] ReceivedMemory;
      if (addressRange.ByteSize == 0U)
      {
        ReceivedMemory = (byte[]) null;
      }
      else
      {
        int count = (int) (addressRange.ByteSize / 500U);
        if (addressRange.ByteSize % 500U > 0U)
          ++count;
        progress.Split(count);
        uint readAddress = addressRange.StartAddress;
        ReceivedMemory = new byte[(int) addressRange.ByteSize];
        do
        {
          uint rangeOffset = readAddress - addressRange.StartAddress;
          uint blockSize = addressRange.ByteSize - rangeOffset;
          if (blockSize > 500U)
            blockSize = 500U;
          byte[] receivedBytes = await this.ReadMemoryBlockAsync(readAddress, blockSize, progress, cancelToken);
          receivedBytes.CopyTo((Array) ReceivedMemory, (long) rangeOffset);
          readAddress += blockSize;
          receivedBytes = (byte[]) null;
        }
        while (readAddress < addressRange.StartAddress + addressRange.ByteSize);
        deviceMemory.SetData(addressRange, ReceivedMemory);
        ReceivedMemory = (byte[]) null;
      }
    }

    private async Task<byte[]> ReadMemoryBlockAsync(
      uint readAddress,
      uint blockSize,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      this.ModuleCommandsLogger.Debug(nameof (ReadMemoryBlockAsync));
      List<byte> commandBytes = new List<byte>();
      commandBytes.Add((byte) 10);
      commandBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes(readAddress));
      commandBytes.AddRange((IEnumerable<byte>) BitConverter.GetBytes((ushort) blockSize));
      byte[] transmitData = commandBytes.ToArray();
      byte[] result = await this.CommonNfcCommands.SendTransparentToModuleAsync(progress, cancelToken, this.ModuleInfo, BusModuleCommand.BUS_ASYNC_TRANSPARENT_TO_MODULE, transmitData);
      byte[] numArray = result;
      commandBytes = (List<byte>) null;
      transmitData = (byte[]) null;
      result = (byte[]) null;
      return numArray;
    }

    public override async Task WriteMemoryAsync(
      AddressRange addressRange,
      DeviceMemory deviceMemory,
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      await Task.Delay(0);
      throw new NotImplementedException(nameof (WriteMemoryAsync));
    }

    internal async Task<byte[]> SendTransparentToModuleAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      byte moduleCommand,
      byte[] transparentData = null)
    {
      this.ModuleCommandsLogger.Debug(nameof (SendTransparentToModuleAsync));
      byte[] transmitData;
      if (transparentData == null)
      {
        transmitData = new byte[1];
      }
      else
      {
        transmitData = new byte[transparentData.Length + 1];
        transparentData.CopyTo((Array) transmitData, 1);
      }
      transmitData[0] = moduleCommand;
      byte[] result = await this.CommonNfcCommands.SendTransparentToModuleAsync(progress, cancelToken, this.ModuleInfo, BusModuleCommand.BUS_ASYNC_TRANSPARENT_TO_MODULE, transmitData);
      byte[] moduleAsync = result;
      transmitData = (byte[]) null;
      result = (byte[]) null;
      return moduleAsync;
    }

    public enum ASYNC_MODULE_COMMANDS : byte
    {
      BUS_ASYNC_NO_COMMAND,
      BUS_ASYNC_MODULE_INITIALISATION,
      BUS_ASYNC_CHANGE_STATE,
      BUS_ASYNC_SAVE_SETUP_TO_MASTER,
      BUS_ASYNC_LOAD_SETUP_FROM_MASTER,
      BUS_ASYNC_TRANSPARENT_TO_MODULE,
      BUS_ASYNC_TRANSPARENT_FROM_MODULE,
      BUS_ASYNC_WAIT_FOR_NEXT_CYCLE,
      BUS_ASYNC_GET_LAST_CYCLE_ANSWER,
      BUS_ASYNC_GET_MEMORY_RANGES,
      BUS_ASYNC_READ_MEMORY,
      BUS_ASYNC_WRITE_MEMORY,
    }
  }
}

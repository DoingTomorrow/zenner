// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_ModuleObjects
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using HandlerLib;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace S4_Handler.Functions
{
  internal class S4_ModuleObjects
  {
    internal S4_Module WorkObject;
    internal S4_Module TypeObject;
    internal S4_Module ConnectedObject;
    internal S4_Module BackupObject;
    private S4_ModuleCommands ModuleCommands;
    internal BusModuleInfo ModuleInfo;

    internal S4_ModuleObjects(S4_ModuleCommands moduleCommands, BusModuleInfo moduleInfo)
    {
      this.ModuleCommands = moduleCommands;
      this.ModuleInfo = moduleInfo;
    }

    internal async Task ConnectModule(ProgressHandler progress, CancellationToken cancelToken)
    {
      S4_ModuleMemory moduleMemory = new S4_ModuleMemory(this.ModuleInfo.FirmwareVersion);
      await this.ModuleCommands.GetMemoryRangesAsync((DeviceMemory) moduleMemory, progress, cancelToken);
      this.WorkObject = (S4_Module) null;
      this.ConnectedObject = new S4_Module(this.ModuleInfo, moduleMemory);
      moduleMemory = (S4_ModuleMemory) null;
    }

    internal async Task ReadModule(
      ProgressHandler progress,
      CancellationToken cancelToken,
      ReadPartsSelection readSelection)
    {
      S4_ModuleMemory moduleMemory = new S4_ModuleMemory(this.ModuleInfo.FirmwareVersion);
      await this.ModuleCommands.GetMemoryRangesAsync((DeviceMemory) moduleMemory, progress, cancelToken);
      foreach (DeviceMemoryStorage theStorage in (IEnumerable<DeviceMemoryStorage>) moduleMemory.MemoryBlockList.Values)
      {
        if ((theStorage.ReadSelection & readSelection) > ~(ReadPartsSelection.Dump | ReadPartsSelection.ProtocolOnlyMode))
          await this.ModuleCommands.ReadMemoryAsync(theStorage.GetAddressRange(), (DeviceMemory) moduleMemory, progress, cancelToken);
      }
      this.WorkObject = (S4_Module) null;
      this.ConnectedObject = new S4_Module(this.ModuleInfo, moduleMemory);
      moduleMemory = (S4_ModuleMemory) null;
    }

    internal async Task<float> ReadImpulseValue(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      byte[] result = await this.ModuleCommands.SendTransparentToModuleAsync(progress, cancelToken, (byte) 128);
      float single = BitConverter.ToSingle(result, 4);
      result = (byte[]) null;
      return single;
    }

    internal async Task WriteImpulseValue(
      ProgressHandler progress,
      CancellationToken cancelToken,
      float ImpulseValue)
    {
      byte[] valueBytes = BitConverter.GetBytes(ImpulseValue);
      byte[] result = await this.ModuleCommands.SendTransparentToModuleAsync(progress, cancelToken, (byte) 129, valueBytes);
      valueBytes = (byte[]) null;
      result = (byte[]) null;
    }
  }
}

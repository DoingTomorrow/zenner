// Decompiled with JetBrains decompiler
// Type: HandlerLib.FirmwareUpdateToolDeviceCommands
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using NLog;
using System;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public abstract class FirmwareUpdateToolDeviceCommands
  {
    internal static Logger FirmwareUpdateTool_DeviceCommandsLogger = LogManager.GetLogger("FirmwareUpdateTool_DeviceCommands");
    private string messageNotImplemented = "Method not implemented for actual connection type!!!";
    public bool usingSubUnitCommands = false;

    public DeviceIdentification ConnectedDeviceVersion => this.GetDeviceVersionObject();

    protected abstract DeviceIdentification GetDeviceVersionObject();

    public bool IsDeviceIdentified => this.ConnectedDeviceVersion != null;

    public virtual async Task<byte[]> ReadMemoryAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      uint startAdress,
      uint size)
    {
      throw new Exception(this.messageNotImplemented);
    }

    public virtual async Task<byte[]> ReadMemoryAsync(
      ProgressHandler progress,
      CancellationToken token,
      uint address,
      uint count,
      byte maxBytesPerPacket)
    {
      throw new Exception(this.messageNotImplemented);
    }

    public virtual async Task WriteMemoryAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      uint startAdress,
      byte[] bytes)
    {
      throw new Exception(this.messageNotImplemented);
    }

    internal virtual void SetIdentificationLikeInFirmware(
      uint serialNumberBCD,
      ushort manufacturerCode,
      byte generation,
      byte mediumCode)
    {
      throw new Exception(this.messageNotImplemented);
    }

    internal virtual Task<byte[]> getVersion() => throw new Exception(this.messageNotImplemented);

    public virtual async Task<byte[]> getVersionAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      throw new Exception(this.messageNotImplemented);
    }

    public virtual async Task<DeviceIdentification> ReadVersionAsync(
      ProgressHandler progress,
      CancellationToken token)
    {
      throw new Exception(this.messageNotImplemented);
    }

    public virtual async Task<int> ReadDeviceAsync(
      ProgressHandler progress,
      CancellationToken token,
      ReadPartsSelection readPartsSelection)
    {
      throw new Exception(this.messageNotImplemented);
    }

    public virtual async Task ResetDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      throw new Exception(this.messageNotImplemented);
    }

    public virtual void SetBlockMode(bool modeOn = true, uint size = 512)
    {
      throw new Exception(this.messageNotImplemented);
    }

    public virtual void SetParameter(string parameterName, uint parameterValue)
    {
      throw new Exception(this.messageNotImplemented);
    }

    public virtual async Task<ushort> VerifyMemoryAsync(
      ProgressHandler progress,
      CancellationToken token,
      uint startAddress,
      uint endAddress)
    {
      throw new Exception(this.messageNotImplemented);
    }

    public virtual ushort VerifyMemory(uint startAddress, uint endAddress)
    {
      throw new Exception(this.messageNotImplemented);
    }
  }
}

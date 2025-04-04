// Decompiled with JetBrains decompiler
// Type: EDCL_Handler.UI.FirmwareUpdateToolDeviceCommands_MBus
// Assembly: EDCL_Handler, Version=2.2.10.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F3010E47-8885-4BE8-8551-D37B09710D3C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDCL_Handler.dll

using CommunicationPort.Functions;
using HandlerLib;
using NLog;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace EDCL_Handler.UI
{
  public class FirmwareUpdateToolDeviceCommands_MBus : FirmwareUpdateToolDeviceCommands
  {
    internal new static Logger FirmwareUpdateTool_DeviceCommandsLogger = LogManager.GetLogger("FirmwareUpdateTool_DeviceCommands_MBus");
    private DeviceCommandsMBus devCMD = (DeviceCommandsMBus) null;

    internal CommonMBusCommands CMDs_MBus { get; private set; }

    internal Common32BitCommands CMDs_Device { get; private set; }

    internal FirmwareUpdateToolDeviceCommands_MBus(CommunicationPortFunctions myPort)
    {
      this.devCMD = new DeviceCommandsMBus((IPort) myPort);
      this.CMDs_Device = new Common32BitCommands(this.devCMD);
      this.CMDs_MBus = new CommonMBusCommands(this.CMDs_Device);
    }

    protected override DeviceIdentification GetDeviceVersionObject()
    {
      return (DeviceIdentification) this.devCMD.ConnectedDeviceVersion;
    }

    public override async Task<DeviceIdentification> ReadVersionAsync(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      DeviceVersionMBus deviceVersionMbus = await this.devCMD.ReadVersionAsync(progress, cancelToken);
      return (DeviceIdentification) deviceVersionMbus;
    }

    public override async Task<byte[]> ReadMemoryAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      uint startAdress,
      uint size,
      byte maxBytePerPacket)
    {
      byte[] numArray = await this.CMDs_Device.ReadMemoryAsync(progress, cancelToken, startAdress, size, maxBytePerPacket);
      return numArray;
    }

    public override async Task<byte[]> ReadMemoryAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      uint startAdress,
      uint size)
    {
      byte[] numArray = await this.CMDs_Device.ReadMemoryAsync(progress, cancelToken, startAdress, size, (byte) 128);
      return numArray;
    }

    public override async Task WriteMemoryAsync(
      ProgressHandler progress,
      CancellationToken cancelToken,
      uint startAdress,
      byte[] bytes)
    {
      await this.CMDs_Device.WriteMemoryAsync(progress, cancelToken, startAdress, bytes);
    }

    public override async Task ResetDeviceAsync(ProgressHandler progress, CancellationToken token)
    {
      await this.CMDs_Device.ResetDeviceAsync(progress, token);
    }
  }
}

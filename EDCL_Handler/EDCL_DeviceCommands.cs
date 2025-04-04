// Decompiled with JetBrains decompiler
// Type: EDCL_Handler.EDCL_DeviceCommands
// Assembly: EDCL_Handler, Version=2.2.10.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F3010E47-8885-4BE8-8551-D37B09710D3C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EDCL_Handler.dll

using CommunicationPort.Functions;
using HandlerLib;
using MBusLib;
using System.Threading;
using ZENNER.CommonLibrary;

#nullable disable
namespace EDCL_Handler
{
  public class EDCL_DeviceCommands : DeviceCommandsMBus
  {
    private CommunicationPortFunctions port;

    public CommonNBIoTCommands NBIoT { get; private set; }

    public CommonLoRaCommands LoRa { get; private set; }

    public CommonMBusCommands MBusCmd { get; private set; }

    public CommonRadioCommands Radio { get; private set; }

    public Common32BitCommands Device { get; private set; }

    public SpecialCommands Special { get; private set; }

    internal EDCL_DeviceCommands(CommunicationPortFunctions port)
      : base((IPort) port)
    {
      this.port = port;
      this.Device = new Common32BitCommands((DeviceCommandsMBus) this);
      this.MBusCmd = new CommonMBusCommands(this.Device);
      this.Radio = new CommonRadioCommands(this.Device);
      this.LoRa = new CommonLoRaCommands(this.Device);
      this.NBIoT = new CommonNBIoTCommands(this.Device);
      this.Special = new SpecialCommands(this.Device);
    }

    internal void StartVolumeMonitor()
    {
      this.port.Write(new MBusFrame(new byte[5]
      {
        (byte) 15,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 64
      }).ToByteArray());
      this.port.ForceWakeup();
    }

    internal void StopVolumeMonitor()
    {
      for (int index = 0; index < 10; ++index)
      {
        this.port.WriteWithoutDiscardInputBuffer(new byte[34]
        {
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85,
          (byte) 85
        });
        this.port.DiscardCurrentInBuffer();
        Thread.Sleep(100);
      }
      this.port.ForceWakeup();
    }

    internal bool GetCurrentInputBuffer(out byte[] buffer)
    {
      buffer = this.port.ReadExisting();
      return buffer != null;
    }
  }
}

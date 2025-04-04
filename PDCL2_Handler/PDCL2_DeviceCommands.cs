// Decompiled with JetBrains decompiler
// Type: PDCL2_Handler.PDCL2_DeviceCommands
// Assembly: PDCL2_Handler, Version=2.22.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 03BA4C2D-69FE-4DA6-9C3F-B3D5471C4058
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDCL2_Handler.dll

using CommunicationPort.Functions;
using HandlerLib;
using MBusLib;
using System.Threading;
using ZENNER.CommonLibrary;

#nullable disable
namespace PDCL2_Handler
{
  public class PDCL2_DeviceCommands : DeviceCommandsMBus
  {
    private CommunicationPortFunctions port;

    internal CommonLoRaCommands LoRa { get; private set; }

    internal CommonMBusCommands MBusCmd { get; private set; }

    internal CommonRadioCommands Radio { get; private set; }

    internal Common32BitCommands Device { get; private set; }

    internal SpecialCommands Special { get; private set; }

    internal PDCL2_DeviceCommands(CommunicationPortFunctions port)
      : base((IPort) port)
    {
      this.port = port;
      this.Device = new Common32BitCommands((DeviceCommandsMBus) this);
      this.MBusCmd = new CommonMBusCommands(this.Device);
      this.Radio = new CommonRadioCommands(this.Device);
      this.LoRa = new CommonLoRaCommands(this.Device);
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

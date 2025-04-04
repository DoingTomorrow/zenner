// Decompiled with JetBrains decompiler
// Type: THL_Handler.THL_DeviceCommands
// Assembly: THL_Handler, Version=1.0.5.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: C9669406-A704-45DE-B726-D8A41F27FFB8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\THL_Handler.dll

using CommunicationPort.Functions;
using HandlerLib;
using ZENNER.CommonLibrary;

#nullable disable
namespace THL_Handler
{
  public class THL_DeviceCommands : DeviceCommandsMBus
  {
    internal CommonLoRaCommands LoRa { get; private set; }

    internal CommonMBusCommands MBusCmd { get; private set; }

    internal CommonRadioCommands Radio { get; private set; }

    internal Common32BitCommands Device { get; private set; }

    internal SpecialCommands Special { get; private set; }

    internal THL_DeviceCommands(CommunicationPortFunctions port)
      : base((IPort) port)
    {
      this.Device = new Common32BitCommands((DeviceCommandsMBus) this);
      this.MBusCmd = new CommonMBusCommands(this.Device);
      this.Radio = new CommonRadioCommands(this.Device);
      this.LoRa = new CommonLoRaCommands(this.Device);
      this.Special = new SpecialCommands(this.Device);
    }
  }
}

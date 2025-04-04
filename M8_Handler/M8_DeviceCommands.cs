// Decompiled with JetBrains decompiler
// Type: M8_Handler.M8_DeviceCommands
// Assembly: M8_Handler, Version=2.0.6.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 582F1296-F274-42DF-B72B-4C0B4D92AA72
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\M8_Handler.dll

using CommunicationPort.Functions;
using HandlerLib;
using ZENNER.CommonLibrary;

#nullable disable
namespace M8_Handler
{
  public class M8_DeviceCommands : DeviceCommandsMBus
  {
    internal CommonLoRaCommands LoRa { get; private set; }

    internal CommonMBusCommands MBusCmd { get; private set; }

    internal CommonRadioCommands Radio { get; private set; }

    internal Common32BitCommands Device { get; private set; }

    internal SpecialCommands Special { get; private set; }

    internal M8_DeviceCommands(CommunicationPortFunctions port)
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

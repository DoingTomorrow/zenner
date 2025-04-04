// Decompiled with JetBrains decompiler
// Type: NFCL_Handler.NFCL_DeviceCommands
// Assembly: NFCL_Handler, Version=2.3.2.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 887E21A2-7448-48CC-AF3E-C39E4C7B3AFD
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NFCL_Handler.dll

using CommunicationPort.Functions;
using HandlerLib;
using ZENNER.CommonLibrary;

#nullable disable
namespace NFCL_Handler
{
  public class NFCL_DeviceCommands : DeviceCommandsMBus
  {
    internal CommonLoRaCommands LoRa { get; private set; }

    internal CommonMBusCommands MBusCmd { get; private set; }

    internal CommonRadioCommands Radio { get; private set; }

    internal Common32BitCommands Device { get; private set; }

    internal SpecialCommands Special { get; private set; }

    internal NFCL_DeviceCommands(CommunicationPortFunctions port)
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

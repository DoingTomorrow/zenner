// Decompiled with JetBrains decompiler
// Type: PDC_Handler.Firmware
// Assembly: PDC_Handler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FFD3ACC-6945-4315-9101-00D149CAC985
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\PDC_Handler.dll

using ZR_ClassLibrary;

#nullable disable
namespace PDC_Handler
{
  public sealed class Firmware
  {
    public DeviceVersion Version { get; set; }

    public ProgFiles FirmwareFile { get; set; }

    public string FirmwareText
    {
      get => this.FirmwareFile != null ? this.FirmwareFile.HexText : string.Empty;
    }
  }
}

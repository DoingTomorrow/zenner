// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.DeliveryNote.Device_DeliveryNote
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System.Collections.Generic;

#nullable disable
namespace ZENNER.CommonLibrary.DeliveryNote
{
  public class Device_DeliveryNote
  {
    private List<Paramter_Device> parameters = new List<Paramter_Device>();

    public HardwareTypeHardwareName TypeOfDevice { get; set; }

    public List<Paramter_Device> Parameters
    {
      get => this.parameters;
      set => this.parameters = value;
    }

    public Device_DeliveryNote()
    {
    }

    public Device_DeliveryNote(HardwareTypeHardwareName type) => this.TypeOfDevice = type;
  }
}

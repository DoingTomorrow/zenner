// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.DeliveryNote.Paramter_Device
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

#nullable disable
namespace ZENNER.CommonLibrary.DeliveryNote
{
  public class Paramter_Device
  {
    public string Value { get; set; }

    public Converter.ParameterNaming NameOfParameter { get; set; }

    public Paramter_Device()
    {
    }

    public Paramter_Device(Converter.ParameterNaming key, string value)
    {
      this.NameOfParameter = key;
      this.Value = value;
    }
  }
}

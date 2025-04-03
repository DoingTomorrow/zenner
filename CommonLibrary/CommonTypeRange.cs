// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.CommonTypeRange
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

#nullable disable
namespace ZENNER.CommonLibrary
{
  public class CommonTypeRange
  {
    internal ulong RangeMin;
    internal ulong RangeMax;
    internal char DIN_Sparte;
    internal LoRa_ProtocolType ProtocolType;

    internal CommonTypeRange(
      ulong rangeMin,
      ulong rangeMax,
      char din_Sparte,
      LoRa_ProtocolType protocolType)
    {
      this.RangeMin = rangeMin;
      this.RangeMax = rangeMax;
      this.DIN_Sparte = din_Sparte;
      this.ProtocolType = protocolType;
    }
  }
}

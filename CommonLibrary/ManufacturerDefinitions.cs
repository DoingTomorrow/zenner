// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.ManufacturerDefinitions
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System.Collections.Generic;

#nullable disable
namespace ZENNER.CommonLibrary
{
  public class ManufacturerDefinitions
  {
    public string Manufacturer;
    public byte[] OIU;
    public SortedList<ulong, CommonTypeRange> TypeRanges;

    public ManufacturerDefinitions(string manufacturer, byte[] oui)
    {
      this.Manufacturer = manufacturer;
      this.OIU = oui;
      this.TypeRanges = new SortedList<ulong, CommonTypeRange>();
    }

    public void AddTypeRange(
      ulong rangeMin,
      ulong rangeMax,
      char din_Sparte,
      LoRa_ProtocolType deviceType)
    {
      this.TypeRanges.Add(rangeMin, new CommonTypeRange(rangeMin, rangeMax, din_Sparte, deviceType));
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.TranslationRule
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;

#nullable disable
namespace ZR_ClassLibrary
{
  [Serializable]
  public sealed class TranslationRule : IEquatable<TranslationRule>
  {
    public string Manufacturer { get; set; }

    public string Medium { get; set; }

    public int VersionMin { get; set; }

    public int VersionMax { get; set; }

    public string MBusZDF { get; set; }

    public long ValueIdent { get; set; }

    public int RuleOrder { get; set; }

    public double Multiplier { get; set; }

    public SpecialTranslation SpecialTranslation { get; set; }

    public int SubDeviceIndex { get; set; }

    public string StorageTimeParam { get; set; }

    public SpecialStorageTimeTranslation StorageTimeTranslation { get; set; }

    public string SubDeviceAttributeIdentifier { get; set; }

    public bool Equals(TranslationRule other)
    {
      return other != null && other.Manufacturer == this.Manufacturer && other.Medium == this.Medium && other.VersionMin == this.VersionMin && other.VersionMax == this.VersionMax && other.MBusZDF == this.MBusZDF && other.ValueIdent == this.ValueIdent && other.RuleOrder == this.RuleOrder && other.StorageTimeParam == this.StorageTimeParam && Util.AreEqual(other.Multiplier, this.Multiplier) && other.SpecialTranslation == this.SpecialTranslation && other.SubDeviceIndex == this.SubDeviceIndex && other.StorageTimeTranslation == this.StorageTimeTranslation && other.SubDeviceAttributeIdentifier == this.SubDeviceAttributeIdentifier;
    }

    public bool IsValidManufacturer()
    {
      return !string.IsNullOrEmpty(this.Manufacturer) && this.Manufacturer.Length < 8;
    }

    public bool IsValidMedium() => !string.IsNullOrEmpty(this.Medium);

    public bool IsValidMBusZDF() => !string.IsNullOrEmpty(this.MBusZDF);

    public bool IsValidVersionMin()
    {
      return this.VersionMin >= -1 && this.VersionMin <= 256 && this.VersionMin <= this.VersionMax;
    }

    public bool IsValidVersionMax()
    {
      return this.VersionMax >= -1 && this.VersionMax <= 256 && this.VersionMax >= this.VersionMin && this.VersionMax != 0;
    }

    public bool IsValidValueIdent() => this.ValueIdent != 0L && ZR_ClassLibrary.ValueIdent.IsValid(this.ValueIdent);

    public bool IsValidMultiplier() => this.Multiplier > 0.0;

    public bool IsValidStorageTimeParam() => this.StorageTimeParam != null;

    public bool IsValidSubDeviceAttributeIdentifier()
    {
      return this.SpecialTranslation == SpecialTranslation.None ? !string.IsNullOrEmpty(this.SubDeviceAttributeIdentifier) : string.IsNullOrEmpty(this.SubDeviceAttributeIdentifier);
    }

    public bool IsValid()
    {
      return this.SubDeviceIndex > 0 ? this.IsValidManufacturer() && this.IsValidMBusZDF() && this.IsValidMedium() && this.IsValidVersionMin() && this.IsValidVersionMax() && this.IsValidSubDeviceAttributeIdentifier() : this.IsValidManufacturer() && this.IsValidMedium() && this.IsValidMBusZDF() && this.IsValidVersionMin() && this.IsValidVersionMax() && this.IsValidValueIdent() && this.IsValidMultiplier() && this.IsValidStorageTimeParam();
    }

    public static string CorrectMedium(string medium)
    {
      if (string.IsNullOrEmpty(medium))
        return (string) null;
      if (medium == "HEAT_OUTLET" || medium == "HEAT_INLET")
        return "HEAT";
      return medium == "COOL_OUTLET" || medium == "COOL_INLET" ? "COOL" : medium;
    }

    public override string ToString()
    {
      if (string.IsNullOrEmpty(this.Manufacturer) || string.IsNullOrEmpty(this.Medium) || string.IsNullOrEmpty(this.MBusZDF))
        return string.Empty;
      return string.Format("Manufacturer: {0}, Medium: {1}, ZDF: {2}, VersionMin: {3}, VersionMax: {4}", (object) this.Manufacturer, (object) this.Medium, (object) this.MBusZDF, (object) this.VersionMin, (object) this.VersionMax);
    }
  }
}

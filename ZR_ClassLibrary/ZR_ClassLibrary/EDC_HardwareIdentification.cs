// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.EDC_HardwareIdentification
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class EDC_HardwareIdentification
  {
    public KOMMUNIKATION_SCHNITTSTELLE Type { get; set; }

    public MONTAGE_AM_ZAEHLER Mounting { get; set; }

    public BATTERIE_TYP Battery { get; set; }

    public COMPLETION Completion { get; set; }

    public string CreateKey()
    {
      return EDC_HardwareIdentification.CreateKey(this.Type.ToString(), this.Mounting.ToString(), this.Battery.ToString(), this.Completion);
    }

    private static string CreateKey(
      string type,
      string mounting,
      string battery,
      COMPLETION completion)
    {
      return string.Format("EDC;{0};{1};{2};{3}", (object) type, (object) mounting, (object) battery, (object) completion);
    }

    public static EDC_HardwareIdentification ParseKey(
      string type,
      string mounting,
      string battery,
      COMPLETION completion)
    {
      return string.IsNullOrEmpty(type) || string.IsNullOrEmpty(mounting) || string.IsNullOrEmpty(battery) ? (EDC_HardwareIdentification) null : EDC_HardwareIdentification.ParseKey(EDC_HardwareIdentification.CreateKey(type, mounting, battery, completion));
    }

    public static EDC_HardwareIdentification ParseKey(string key)
    {
      string[] strArray = !string.IsNullOrEmpty(key) ? key.Split(';') : throw new ArgumentException(nameof (key));
      if (strArray.Length < 1)
        throw new ArgumentException(nameof (key));
      if (strArray[0] != "EDC")
        throw new ArgumentException(nameof (key));
      if (strArray.Length != 5)
        return (EDC_HardwareIdentification) null;
      return new EDC_HardwareIdentification()
      {
        Type = (KOMMUNIKATION_SCHNITTSTELLE) Enum.Parse(typeof (KOMMUNIKATION_SCHNITTSTELLE), strArray[1], true),
        Mounting = (MONTAGE_AM_ZAEHLER) Enum.Parse(typeof (MONTAGE_AM_ZAEHLER), strArray[2], true),
        Battery = (BATTERIE_TYP) Enum.Parse(typeof (BATTERIE_TYP), strArray[3], true),
        Completion = (COMPLETION) Enum.Parse(typeof (COMPLETION), strArray[4], true)
      };
    }
  }
}

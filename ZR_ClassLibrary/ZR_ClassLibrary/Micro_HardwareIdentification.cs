// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.Micro_HardwareIdentification
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class Micro_HardwareIdentification
  {
    public KOMMUNIKATION_SCHNITTSTELLE Type { get; set; }

    public BATTERIE_TYP Battery { get; set; }

    public COMPLETION Completion { get; set; }

    public string CreateKey()
    {
      return Micro_HardwareIdentification.CreateKey(this.Type.ToString(), this.Battery.ToString(), this.Completion);
    }

    private static string CreateKey(string type, string battery, COMPLETION completion)
    {
      return string.Format("Micro;{0};{1};{2}", (object) type, (object) battery, (object) completion);
    }

    public static Micro_HardwareIdentification ParseKey(
      string type,
      string battery,
      COMPLETION completion)
    {
      return string.IsNullOrEmpty(type) || string.IsNullOrEmpty(battery) ? (Micro_HardwareIdentification) null : Micro_HardwareIdentification.ParseKey(Micro_HardwareIdentification.CreateKey(type, battery, completion));
    }

    public static Micro_HardwareIdentification ParseKey(string key)
    {
      string[] strArray = !string.IsNullOrEmpty(key) ? key.Split(';') : throw new ArgumentException(nameof (key));
      if (strArray.Length < 1)
        throw new ArgumentException(nameof (key));
      if (strArray[0] != "Micro")
        throw new ArgumentException(nameof (key));
      if (strArray.Length != 5)
        return (Micro_HardwareIdentification) null;
      return new Micro_HardwareIdentification()
      {
        Type = (KOMMUNIKATION_SCHNITTSTELLE) Enum.Parse(typeof (KOMMUNIKATION_SCHNITTSTELLE), strArray[1], true),
        Battery = (BATTERIE_TYP) Enum.Parse(typeof (BATTERIE_TYP), strArray[2], true),
        Completion = (COMPLETION) Enum.Parse(typeof (COMPLETION), strArray[3], true)
      };
    }
  }
}

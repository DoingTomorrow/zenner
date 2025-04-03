// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.PDC_HardwareIdentification
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class PDC_HardwareIdentification
  {
    public KOMMUNIKATION_SCHNITTSTELLE_PDC Type { get; set; }

    public InputSettings_PDC InputConfig { get; set; }

    public BATTERIE_TYP_PDC Battery { get; set; }

    public string CreateKey()
    {
      return PDC_HardwareIdentification.CreateKey(this.Type.ToString(), this.Battery.ToString());
    }

    private static string CreateKey(string communicationInterface, string batteryType)
    {
      return string.Format("PDC;{0};{1}", (object) communicationInterface, (object) batteryType);
    }

    public static PDC_HardwareIdentification ParseKey(
      string communicationInterface,
      string batteryType)
    {
      if (string.IsNullOrEmpty(communicationInterface))
        throw new ArgumentNullException(nameof (communicationInterface));
      if (string.IsNullOrEmpty(batteryType))
        throw new ArgumentNullException(nameof (batteryType));
      if (!Enum.IsDefined(typeof (KOMMUNIKATION_SCHNITTSTELLE_PDC), (object) communicationInterface))
        throw new ArgumentNullException(nameof (communicationInterface));
      return Enum.IsDefined(typeof (BATTERIE_TYP_PDC), (object) batteryType) ? PDC_HardwareIdentification.ParseKey(PDC_HardwareIdentification.CreateKey(communicationInterface, batteryType)) : throw new ArgumentNullException(nameof (batteryType));
    }

    public static PDC_HardwareIdentification ParseKey(string key)
    {
      string[] strArray = !string.IsNullOrEmpty(key) ? key.Split(';') : throw new NullReferenceException(nameof (key));
      if (strArray.Length < 1)
        throw new ArgumentException(nameof (key));
      if (strArray[0] != "PDC")
        throw new ArgumentException(nameof (key));
      if (strArray.Length != 3)
        return (PDC_HardwareIdentification) null;
      return new PDC_HardwareIdentification()
      {
        Type = (KOMMUNIKATION_SCHNITTSTELLE_PDC) Enum.Parse(typeof (KOMMUNIKATION_SCHNITTSTELLE_PDC), strArray[1], true),
        Battery = (BATTERIE_TYP_PDC) Enum.Parse(typeof (BATTERIE_TYP_PDC), strArray[2], true),
        InputConfig = InputSettings_PDC.NONE
      };
    }

    public override string ToString() => this.CreateKey();
  }
}

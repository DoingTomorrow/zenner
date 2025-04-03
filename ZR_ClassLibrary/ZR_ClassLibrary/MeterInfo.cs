// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.MeterInfo
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Text;

#nullable disable
namespace ZR_ClassLibrary
{
  [Serializable]
  public sealed class MeterInfo
  {
    public int MeterInfoID { get; set; }

    public int MeterHardwareID { get; set; }

    public int MeterTypeID { get; set; }

    public string PPSArtikelNr { get; set; }

    public string DefaultFunctionNr { get; set; }

    public string Description { get; set; }

    public int HardwareTypeID { get; set; }

    public override string ToString() => this.MeterInfoID.ToString() + " " + this.Description;

    public string ToString(int spaces)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      if (!string.IsNullOrEmpty(this.Description))
        stringBuilder1.Append("MeterInfoDescription: ".PadRight(spaces)).AppendLine(this.Description);
      StringBuilder stringBuilder2 = stringBuilder1.Append("HardwareTypeID: ".PadRight(spaces));
      int num = this.HardwareTypeID;
      string str1 = num.ToString();
      stringBuilder2.AppendLine(str1);
      StringBuilder stringBuilder3 = stringBuilder1.Append("MeterInfoID: ".PadRight(spaces));
      num = this.MeterInfoID;
      string str2 = num.ToString();
      stringBuilder3.AppendLine(str2);
      StringBuilder stringBuilder4 = stringBuilder1.Append("MeterHardwareID: ".PadRight(spaces));
      num = this.MeterHardwareID;
      string str3 = num.ToString();
      stringBuilder4.AppendLine(str3);
      StringBuilder stringBuilder5 = stringBuilder1.Append("MeterTypeID: ".PadRight(spaces));
      num = this.MeterTypeID;
      string str4 = num.ToString();
      stringBuilder5.AppendLine(str4);
      if (!string.IsNullOrEmpty(this.PPSArtikelNr))
        stringBuilder1.Append("PPSArtikelNr: ".PadRight(spaces)).AppendLine(this.PPSArtikelNr);
      return stringBuilder1.ToString();
    }

    public MeterInfo DeepCopy() => this.MemberwiseClone() as MeterInfo;
  }
}

// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ReadoutType
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using GmmDbLib;

#nullable disable
namespace ZR_ClassLibrary
{
  public sealed class ReadoutType
  {
    public int ReadoutDeviceTypeID { get; set; }

    public int ReadoutSettingsID { get; set; }

    public string ImageIdList { get; set; }

    public string ReadoutDeviceType
    {
      get
      {
        return Ot.GetTranslatedLanguageText("ReadoutDeviceTypeID", this.ReadoutDeviceTypeID.ToString());
      }
    }

    public string ReadoutSettings
    {
      get => Ot.GetTranslatedLanguageText("ReadoutSettingsID", this.ReadoutSettingsID.ToString());
    }

    public override string ToString()
    {
      return Ot.GetTranslatedLanguageText("ReadoutDeviceTypeID", this.ReadoutDeviceTypeID.ToString()) + " " + Ot.GetTranslatedLanguageText("ReadoutSettingsID", this.ReadoutSettingsID.ToString());
    }
  }
}

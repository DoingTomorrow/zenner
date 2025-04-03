// Decompiled with JetBrains decompiler
// Type: GmmDbLib.EnumTranslator
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using System;

#nullable disable
namespace GmmDbLib
{
  public static class EnumTranslator
  {
    public static string GetTranslatedEnumName(object EnumObject)
    {
      string str = EnumObject.GetType().ToString();
      string defaultText = EnumObject.ToString();
      return Ot.GetTranslatedLanguageText("EnumName_", str + "_" + defaultText, defaultText);
    }

    public static string GetTranslatedEnumName(object EnumObject, string defaultName)
    {
      return Ot.GetTranslatedLanguageText("EnumName_", EnumObject.GetType().ToString() + "_" + EnumObject.ToString(), defaultName);
    }

    public static string GetTranslatedEnumDescription(object EnumObject)
    {
      string str = EnumObject.GetType().ToString();
      string defaultText = EnumObject.ToString();
      return Ot.GetTranslatedLanguageText("EnumDesc_", str + "_" + defaultText, defaultText);
    }

    public static string[] GetTranslatedEnumNames(Type EnumType)
    {
      string[] names = Enum.GetNames(EnumType);
      string str = EnumType.ToString();
      string[] translatedEnumNames = new string[names.Length];
      for (int index = 0; index < names.Length; ++index)
      {
        string defaultText = names[index];
        translatedEnumNames[index] = Ot.GetTranslatedLanguageText("EnumName_", str + "_" + defaultText, defaultText);
      }
      return translatedEnumNames;
    }
  }
}

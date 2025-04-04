// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.Kodierung
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Globalization;

#nullable disable
namespace ZR_ClassLibrary
{
  public class Kodierung
  {
    public static string LoginPersonalName = string.Empty;
    public static string LoginPermission = string.Empty;
    public static long LoginPersonalNr = 0;

    public bool GetGlobalPermission(
      out int Personalnummer,
      out string DBPersonalName,
      out string DBPermission)
    {
      Personalnummer = 0;
      DBPersonalName = string.Empty;
      DBPermission = string.Empty;
      if (Kodierung.LoginPersonalName.Length == 0 || Kodierung.LoginPermission.Length == 0 || Kodierung.LoginPersonalNr == 0L)
        return false;
      Personalnummer = (int) Kodierung.LoginPersonalNr;
      DBPersonalName = Kodierung.LoginPersonalName;
      DBPermission = Kodierung.LoginPermission;
      return true;
    }

    public bool EncodePassword(
      string Passwort,
      int Personalnummer,
      out string KodiertesPasswort,
      out string Fehlerstring)
    {
      string str1 = "";
      string str2 = "";
      Fehlerstring = "";
      try
      {
        Passwort = Passwort.Trim();
        int length = Passwort.Length;
        if (length < 5 || length > 25)
          throw new ApplicationException("Passwort muss mindestens 5 und maximal 25 Stellen haben!");
        int index1 = 0;
        for (int index2 = 0; index2 < Personalnummer; ++index2)
        {
          ++index1;
          if (index1 == length)
            index1 = 0;
        }
        char ch;
        for (int index3 = 0; index3 < length; ++index3)
        {
          string str3 = str1;
          ch = Passwort[index1];
          string str4 = ch.ToString();
          str1 = str3 + str4;
          ++index1;
          if (index1 == length)
            index1 = 0;
        }
        int num1 = 0;
        for (int index4 = 0; index4 < length; ++index4)
        {
          int num2 = (int) str1[index4];
          num1 += num2;
        }
        for (int index5 = 0; index5 < length; ++index5)
        {
          int num3 = ((int) str1[index5] + (index5 + 1) * 7 + Personalnummer + num1) % (int) byte.MaxValue;
          string str5 = str2;
          ch = (char) num3;
          string str6 = ch.ToString();
          str2 = str5 + str6;
        }
        string str7 = "";
        for (int index6 = 0; index6 < length; ++index6)
        {
          int num4 = (int) str2[index6];
          str7 += num4.ToString("X");
        }
        KodiertesPasswort = str7;
        return true;
      }
      catch (Exception ex)
      {
        Fehlerstring = ex.ToString();
        KodiertesPasswort = "";
        return false;
      }
    }

    public int Get32BitRandomValue()
    {
      Random random = new Random();
      int num;
      do
      {
        num = random.Next();
      }
      while (num == 0);
      return num;
    }

    public string GetPruefstellenPasswort() => "A12BCBBCDDDAA";

    public int Get32BitPasswort(string VerschluesseltesPasswort)
    {
      int num1 = 0;
      VerschluesseltesPasswort = VerschluesseltesPasswort.Trim();
      for (int startIndex = 0; startIndex < VerschluesseltesPasswort.Length; ++startIndex)
      {
        int num2 = int.Parse(VerschluesseltesPasswort.Substring(startIndex, 1), NumberStyles.AllowHexSpecifier);
        num1 += num2;
      }
      return num1;
    }

    public int Get32BitXORKey(int RandomValue, int IntPasswort) => RandomValue ^ IntPasswort;
  }
}

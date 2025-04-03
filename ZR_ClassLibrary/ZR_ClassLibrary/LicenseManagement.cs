// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.LicenseManagement
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace ZR_ClassLibrary
{
  public class LicenseManagement
  {
    internal static byte LizenzCodeVersion = 0;
    internal static byte MobileDeviceVersion = 16;
    private static int PackageNumber = -1;
    private static int OptionNumber = -1;
    public static string PackageName = string.Empty;
    public static string OptionPackageName = string.Empty;
    private static bool lizok = false;
    private static string aktCode = "LZC.";
    private static bool[] liz = new bool[4];
    internal static string[] PackageNames = new string[6]
    {
      "Demo",
      "ServiceManager",
      "ConfigurationManager",
      "RadioManager",
      "SystemManager",
      "GlobalMeterManager"
    };
    internal static string[] OptionPackageNames = new string[1]
    {
      "NoOptions"
    };
    private static char[] CodeTranslatorTable = new char[32]
    {
      'M',
      '1',
      'Z',
      '3',
      'C',
      '4',
      '7',
      '8',
      'R',
      'P',
      'L',
      'H',
      'A',
      '2',
      'B',
      'Y',
      'K',
      'W',
      'E',
      'F',
      'V',
      'G',
      'D',
      'N',
      'T',
      '9',
      'U',
      '5',
      'Q',
      'S',
      '6',
      'X'
    };

    public static bool CheckLicenseKey(string LicenseKey)
    {
      LicenseManagement.aktCode = LicenseKey;
      LicenseManagement.CheckLiz(ref LicenseManagement.liz);
      return !LicenseManagement.liz[1] && LicenseManagement.liz[0] && LicenseManagement.liz[2];
    }

    private static bool CheckLiz(ref bool[] liz)
    {
      if (!liz[1])
      {
        try
        {
          LicenseManagement.aktCode = LicenseManagement.decodeZennerCode(LicenseManagement.aktCode);
          if (LicenseManagement.localCustomScrable(LicenseManagement.GetDeviceID()) == LicenseManagement.aktCode)
          {
            liz[3] = false;
            liz[0] = true;
            liz[2] = true;
          }
          else
            liz[3] = true;
        }
        catch
        {
          liz[3] = false;
        }
      }
      return LicenseManagement.lizok;
    }

    private static string localCustomScrable(string intext)
    {
      uint[] numArray1 = new uint[intext.Length];
      uint[] numArray2 = new uint[13];
      LicenseManagement.lizok = true;
      for (int index = 0; index < intext.Length; ++index)
        numArray1[index] = (uint) intext[index];
      int index1 = 7;
      int index2 = 0;
      for (int index3 = 0; index3 < 133; ++index3)
      {
        if (index2 >= numArray1.Length)
          index2 = 0;
        if (index1 >= numArray2.Length)
          index1 -= numArray2.Length;
        numArray2[index1] += numArray1[index2];
        numArray2[index1] += (uint) LicenseManagement.PackageNumber;
        numArray2[index1] += (uint) LicenseManagement.OptionNumber;
        numArray2[index1] += (uint) index2;
        ++index2;
        index1 += 3;
      }
      StringBuilder stringBuilder = new StringBuilder(30);
      for (int index4 = 0; index4 < numArray2.Length; ++index4)
      {
        uint num = numArray2[index4];
        uint InChar = 0;
        for (; num > 0U; num >>= 5)
          InChar ^= num & 31U;
        stringBuilder.Append(LicenseManagement.GetCharacterCode((int) InChar));
      }
      return stringBuilder.ToString();
    }

    private static string decodeZennerCode(string intext)
    {
      if (intext.Length == 0)
        return "";
      if (intext.Length != 24)
      {
        int num = (int) MessageBox.Show(ZR_ClassLibMessages.ZR_ClassMessage.GetString("2"), ZR_ClassLibMessages.ZR_ClassMessage.GetString("1"));
        return "";
      }
      intext = intext.Replace("-", "");
      if (intext.Length != 20)
      {
        int num = (int) MessageBox.Show(ZR_ClassLibMessages.ZR_ClassMessage.GetString("3"), ZR_ClassLibMessages.ZR_ClassMessage.GetString("1"));
        return "";
      }
      if ((LicenseManagement.GetIntFromCharacterCode(intext[0]) & 15) != (int) LicenseManagement.LizenzCodeVersion)
      {
        int num = (int) MessageBox.Show(ZR_ClassLibMessages.ZR_ClassMessage.GetString("4"), ZR_ClassLibMessages.ZR_ClassMessage.GetString("1"));
        return "";
      }
      if ((LicenseManagement.GetIntFromCharacterCode(intext[0]) & 16) != (int) LicenseManagement.MobileDeviceVersion)
      {
        int num = (int) MessageBox.Show(ZR_ClassLibMessages.ZR_ClassMessage.GetString("11"), ZR_ClassLibMessages.ZR_ClassMessage.GetString("1"));
        return "";
      }
      if (LicenseManagement.GetStringCS(intext.Substring(0, 18)) != intext.Substring(18))
      {
        int num = (int) MessageBox.Show(ZR_ClassLibMessages.ZR_ClassMessage.GetString("5"), ZR_ClassLibMessages.ZR_ClassMessage.GetString("1"));
        return "";
      }
      LicenseManagement.PackageNumber = 0;
      LicenseManagement.PackageNumber += LicenseManagement.GetIntFromCharacterCode(intext[1]);
      LicenseManagement.PackageNumber += LicenseManagement.GetIntFromCharacterCode(intext[2]) << 5;
      LicenseManagement.OptionNumber = 0;
      LicenseManagement.OptionNumber += LicenseManagement.GetIntFromCharacterCode(intext[3]);
      LicenseManagement.OptionNumber += LicenseManagement.GetIntFromCharacterCode(intext[4]) << 5;
      if (LicenseManagement.PackageNumber < 0 || LicenseManagement.PackageNumber >= LicenseManagement.PackageNames.Length || LicenseManagement.OptionNumber < 0 || LicenseManagement.OptionNumber >= LicenseManagement.OptionPackageNames.Length)
      {
        LicenseManagement.PackageNumber = -1;
        LicenseManagement.OptionNumber = -1;
        int num = (int) MessageBox.Show(ZR_ClassLibMessages.ZR_ClassMessage.GetString("6"), ZR_ClassLibMessages.ZR_ClassMessage.GetString("1"));
        return "";
      }
      LicenseManagement.PackageName = LicenseManagement.PackageNames[LicenseManagement.PackageNumber];
      LicenseManagement.OptionPackageName = LicenseManagement.OptionPackageNames[LicenseManagement.OptionNumber];
      return intext.Substring(5, 13);
    }

    public static string GetDeviceKey()
    {
      string deviceId = LicenseManagement.GetDeviceID();
      char characterCode = LicenseManagement.GetCharacterCode((int) LicenseManagement.LizenzCodeVersion | 16);
      string str1 = characterCode.ToString();
      string str2 = deviceId;
      characterCode = LicenseManagement.GetCharacterCode((int) LicenseManagement.LizenzCodeVersion | 16);
      string stringCs = LicenseManagement.GetStringCS(characterCode.ToString() + deviceId);
      return LicenseManagement.GetSeparatedString(str1 + str2 + stringCs);
    }

    public static string GetDeviceID()
    {
      string deviceId1 = API_Access.GetDeviceID();
      byte[] hash = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(deviceId1));
      StringBuilder stringBuilder = new StringBuilder();
      string deviceId2 = "";
      for (int InChar = 0; InChar < 17; ++InChar)
        deviceId2 = InChar <= hash.Length - 1 ? deviceId2 + LicenseManagement.GetCharacterCode((int) hash[InChar]).ToString() : deviceId2 + LicenseManagement.GetCharacterCode(InChar).ToString();
      return deviceId2;
    }

    internal static char GetCharacterCode(int InChar)
    {
      return LicenseManagement.CodeTranslatorTable[InChar & 31];
    }

    internal static int GetIntFromCharacterCode(char InChar)
    {
      for (int fromCharacterCode = 0; fromCharacterCode < LicenseManagement.CodeTranslatorTable.Length; ++fromCharacterCode)
      {
        if ((int) LicenseManagement.CodeTranslatorTable[fromCharacterCode] == (int) InChar)
          return fromCharacterCode;
      }
      return -1;
    }

    internal static string GetStringCS(string InputString)
    {
      int InChar = 0;
      for (int index = 0; index < InputString.Length; ++index)
        InChar = InChar + index + (InputString.Length - index) + (int) InputString[index];
      char characterCode = LicenseManagement.GetCharacterCode(InChar);
      string str1 = "" + characterCode.ToString();
      characterCode = LicenseManagement.GetCharacterCode(InChar >> 6);
      string str2 = characterCode.ToString();
      return str1 + str2;
    }

    internal static string GetSeparatedString(string InputString)
    {
      StringBuilder stringBuilder = new StringBuilder(30);
      for (int index = 0; index < InputString.Length; ++index)
      {
        if (index % 4 == 0 && index > 0)
          stringBuilder.Append('-');
        stringBuilder.Append(InputString[index]);
      }
      return stringBuilder.ToString();
    }
  }
}

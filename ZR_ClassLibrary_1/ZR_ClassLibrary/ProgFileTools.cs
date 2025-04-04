// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ProgFileTools
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace ZR_ClassLibrary
{
  public class ProgFileTools
  {
    public SortedDictionary<uint, byte[]> parseTextfile(string firmwareFileString)
    {
      SortedDictionary<uint, byte[]> textfile = new SortedDictionary<uint, byte[]>();
      string[] strArray = firmwareFileString.Split('\n');
      string empty1 = string.Empty;
      uint key1 = 0;
      foreach (string str1 in strArray)
      {
        if (str1.Contains("@"))
        {
          uint num = key1;
          key1 = uint.Parse(str1.Substring(1), NumberStyles.HexNumber);
          uint key2 = num == 0U ? key1 : num;
          byte[] numArray = new byte[0];
          if (empty1 != string.Empty)
          {
            numArray = Util.HexStringToByteArray(empty1);
            empty1 = string.Empty;
          }
          if (!textfile.ContainsKey(key2))
            textfile.Add(key2, numArray);
          else
            textfile[key2] = numArray;
        }
        else if (str1.Contains("q"))
        {
          byte[] numArray = new byte[0];
          if (empty1 != string.Empty)
          {
            numArray = Util.HexStringToByteArray(empty1);
            empty1 = string.Empty;
          }
          textfile.Add(key1, numArray);
        }
        else
        {
          string empty2 = string.Empty;
          string str2 = str1.Replace(" ", "").Replace("\r", "").Replace("\n", "");
          empty1 += str2;
        }
      }
      return textfile;
    }

    public static SortedDictionary<uint, byte[]> parseINTEL_HEX_file(string firmwareFileString)
    {
      SortedDictionary<uint, byte[]> intelHexFile = new SortedDictionary<uint, byte[]>();
      string[] strArray = firmwareFileString.Split('\r');
      string empty1 = string.Empty;
      sbyte chkSUM = 0;
      uint num1 = 0;
      string empty2 = string.Empty;
      string str1 = string.Empty;
      string str2 = string.Empty;
      foreach (string str3 in strArray)
      {
        string empty3 = string.Empty;
        if (str3.Contains(":"))
        {
          string str4 = str3.Replace("\n", "");
          byte[] numArray = new byte[0];
          uint length = uint.Parse(str4.Substring(1, 2), NumberStyles.HexNumber) * 2U;
          string s1 = str4.Substring(3, 4);
          uint num2 = uint.Parse(str4.Substring(7, 2), NumberStyles.HexNumber);
          if (num2 == 0U)
          {
            numArray = Util.HexStringToByteArray(str4.Substring(9, (int) length));
            chkSUM = sbyte.Parse(str4.Substring(9 + (int) length, 2), NumberStyles.HexNumber);
          }
          if (num2 == 1U)
          {
            chkSUM = sbyte.Parse(str4.Substring(9), NumberStyles.HexNumber);
            if (chkSUM == (sbyte) -1)
              break;
          }
          if (num2 == 2U)
          {
            num1 = uint.Parse(str4.Substring(9, 4), NumberStyles.HexNumber) * 16U;
            chkSUM = sbyte.Parse(str4.Substring(13, 2), NumberStyles.HexNumber);
          }
          if (num2 == 3U)
          {
            uint num3 = uint.Parse(str4.Substring(9, 4), NumberStyles.HexNumber);
            uint num4 = uint.Parse(str4.Substring(13, 4), NumberStyles.HexNumber);
            chkSUM = sbyte.Parse(str4.Substring(17, 2), NumberStyles.HexNumber);
            num1 = num3 * 16U + num4;
          }
          if (num2 == 4U)
          {
            str1 = str4.Substring(9, 4);
            chkSUM = sbyte.Parse(str4.Substring(13, 2), NumberStyles.HexNumber);
          }
          if (num2 == 5U)
          {
            str2 = str4.Substring(9, 8);
            chkSUM = sbyte.Parse(str4.Substring(17, 2), NumberStyles.HexNumber);
          }
          if (!ProgFileTools.chkSumTEST(Util.HexStringToByteArray(str4.Substring(1, str4.Length - 3)), chkSUM))
            throw new Exception("CHECKSUM ERROR in firmware file !!!\nAdress: " + s1 + "\nPlease check the firmware file and replace with working file.\n");
          string s2 = string.IsNullOrEmpty(str2) ? str1 + s1 : str2;
          uint key = num1 != 0U ? num1 + uint.Parse(s1, NumberStyles.HexNumber) : uint.Parse(s2, NumberStyles.HexNumber);
          if (num2 == 0U && !intelHexFile.ContainsKey(key + num1))
            intelHexFile.Add(key, numArray);
        }
      }
      return intelHexFile;
    }

    public static bool chkSumTEST(byte[] chkBA, sbyte chkSUM)
    {
      byte num1 = 0;
      foreach (byte num2 in chkBA)
        num1 += num2;
      sbyte num3 = (sbyte) (((int) (sbyte) ((int) num1 & (int) byte.MaxValue) ^ (int) byte.MaxValue) + 1);
      return (int) chkSUM == (int) num3;
    }
  }
}

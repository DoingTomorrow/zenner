// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.ParameterService
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace ZR_ClassLibrary
{
  public class ParameterService
  {
    private static ParameterService.S3_ValueTypeTranslation[] S3_ValueTypeCodes = new ParameterService.S3_ValueTypeTranslation[14]
    {
      new ParameterService.S3_ValueTypeTranslation(S3_VariableTypes.INT8, typeof (short), 1),
      new ParameterService.S3_ValueTypeTranslation(S3_VariableTypes.UINT8, typeof (byte), 1),
      new ParameterService.S3_ValueTypeTranslation(S3_VariableTypes.UINT16, typeof (ushort), 2),
      new ParameterService.S3_ValueTypeTranslation(S3_VariableTypes.INT16, typeof (short), 2),
      new ParameterService.S3_ValueTypeTranslation(S3_VariableTypes.UINT32, typeof (uint), 4),
      new ParameterService.S3_ValueTypeTranslation(S3_VariableTypes.INT32, typeof (int), 4),
      new ParameterService.S3_ValueTypeTranslation(S3_VariableTypes.REAL32, typeof (float), 4),
      new ParameterService.S3_ValueTypeTranslation(S3_VariableTypes.INT64, typeof (long), 8),
      new ParameterService.S3_ValueTypeTranslation(S3_VariableTypes.UINT64, typeof (ulong), 8),
      new ParameterService.S3_ValueTypeTranslation(S3_VariableTypes.REAL64, typeof (double), 8),
      new ParameterService.S3_ValueTypeTranslation(S3_VariableTypes.MeterTime1980, typeof (uint), 4),
      new ParameterService.S3_ValueTypeTranslation(S3_VariableTypes.Address, typeof (uint), 0),
      new ParameterService.S3_ValueTypeTranslation(S3_VariableTypes.ByteArray, typeof (byte[]), 1),
      new ParameterService.S3_ValueTypeTranslation(S3_VariableTypes.TDC_Matrix, typeof (float), 1)
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

    public static int GetNumberOfBytes(S3_VariableTypes VarType)
    {
      return ParameterService.S3_ValueTypeCodes[(int) VarType].Bytes;
    }

    public static SortedList<string, string> NamedParamSE_GetAsSortedList(string ParameterString)
    {
      SortedList<string, string> asSortedList = new SortedList<string, string>();
      string str1 = ParameterString;
      char[] chArray1 = new char[1]{ ';' };
      foreach (string str2 in str1.Split(chArray1))
      {
        char[] chArray2 = new char[1]{ '=' };
        string[] strArray = str2.Split(chArray2);
        if (strArray.Length > 1)
        {
          string key = strArray[0].Trim();
          string str3 = strArray[1].Trim();
          if (!asSortedList.ContainsKey(key))
            asSortedList.Add(key, str3);
        }
      }
      return asSortedList;
    }

    public static Dictionary<long, string> NamedParamSE_GetAsDictionary_Long_String(
      string ParameterString)
    {
      Dictionary<long, string> dictionaryLongString = new Dictionary<long, string>();
      string str1 = ParameterString;
      char[] chArray1 = new char[1]{ ';' };
      foreach (string str2 in str1.Split(chArray1))
      {
        char[] chArray2 = new char[1]{ '=' };
        string[] strArray = str2.Split(chArray2);
        if (strArray.Length > 1)
        {
          long int64 = Convert.ToInt64(strArray[0].Trim());
          string str3 = strArray[1].Trim();
          if (!dictionaryLongString.ContainsKey(int64))
            dictionaryLongString.Add(int64, str3);
        }
      }
      return dictionaryLongString;
    }

    public static string NamedParamSE_GetAsString(SortedList<string, string> ParameterList)
    {
      StringBuilder stringBuilder = new StringBuilder(500);
      if (ParameterList != null)
      {
        for (int index = 0; index < ParameterList.Count; ++index)
        {
          if (index != 0)
            stringBuilder.Append(';');
          stringBuilder.Append(ParameterList.Keys[index]);
          stringBuilder.Append('=');
          stringBuilder.Append(ParameterList.Values[index]);
        }
      }
      return stringBuilder.ToString();
    }

    public static string NamedParamSE_GetAsString(Dictionary<long, string> ParameterList)
    {
      StringBuilder stringBuilder = new StringBuilder(500);
      if (ParameterList != null && ParameterList.Count > 0)
      {
        foreach (KeyValuePair<long, string> parameter in ParameterList)
        {
          stringBuilder.Append(parameter.Key);
          stringBuilder.Append('=');
          stringBuilder.Append(parameter.Value);
          stringBuilder.Append(';');
        }
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
      }
      return stringBuilder.ToString();
    }

    private static bool IsValuePart(string Instring)
    {
      try
      {
        if (Instring == ".")
          return true;
        int.Parse(Instring);
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static void GetValueAndUnit(
      string Instring,
      out string SValue,
      out string SUnit,
      out int decimalCounts)
    {
      string empty = string.Empty;
      SValue = string.Empty;
      SUnit = string.Empty;
      int num = -1;
      decimalCounts = -1;
      Instring = Instring.Trim();
      for (int startIndex = 0; startIndex < Instring.Length; ++startIndex)
      {
        string Instring1 = Instring.Substring(startIndex, 1);
        if (Instring1 == ".")
          num = startIndex;
        if (ParameterService.IsValuePart(Instring1))
        {
          SValue += Instring1;
          if (num > 0)
            decimalCounts = startIndex - num;
        }
        else
          SUnit += Instring1;
      }
    }

    public static string AddParameter(
      string ParameterString,
      string behind,
      string ParameterName,
      string ParameterValue)
    {
      if (ParameterName == null)
        return string.Empty;
      if (ParameterString == null)
        ParameterString = string.Empty;
      ParameterString.Trim();
      ParameterName.Trim();
      ParameterValue.Trim();
      if (ParameterName.Length == 0 || ParameterValue.Length == 0)
        return ParameterString;
      string str = ParameterName + ";" + ParameterValue;
      if (behind == "")
        return ParameterString.Length == 0 ? str : str + ";" + ParameterString;
      int startIndex = ParameterString.IndexOf(behind);
      if (startIndex >= 0)
      {
        int num = ParameterString.IndexOf(';', startIndex);
        if (num >= 0)
          return ParameterString.Substring(0, num) + ";" + str + ParameterString.Substring(num);
      }
      return ParameterString + ";" + str;
    }

    public static string AddOrUpdateParameter(string settings, string key, string value)
    {
      if (ParameterService.GetParameter(settings, key) == value && settings.IndexOf(key + ";") == settings.LastIndexOf(key + ";"))
        return settings;
      ParameterService.DeleteParameter(ref settings, key);
      return ParameterService.AddParameter(settings, "", key, value);
    }

    public static bool DeleteParameter(ref string ParameterString, string ParameterName)
    {
      if (string.IsNullOrEmpty(ParameterString))
        return false;
      string[] strArray = ParameterString.Split(';');
      if (strArray == null || strArray.Length == 0)
        return false;
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index].Trim() == ParameterName.Trim())
        {
          ++index;
        }
        else
        {
          if (stringBuilder.Length != 0)
            stringBuilder.Append(';');
          stringBuilder.Append(strArray[index].Trim());
        }
      }
      ParameterString = stringBuilder.ToString();
      return true;
    }

    public static string GetParameter(string ParameterString, string ParameterName)
    {
      string parameter = ParameterService.GetParameter(ParameterString, ParameterName, ';');
      if (parameter == "")
        parameter = ParameterService.GetParameter(ParameterString, ParameterName, ',');
      return parameter;
    }

    public static string GetParameter(string ParameterString, string ParameterName, char Sep)
    {
      if (string.IsNullOrEmpty(ParameterString))
        return string.Empty;
      string[] strArray = ParameterString.Split(Sep);
      string parameter = "";
      for (int index = 0; index < strArray.Length; index += 2)
      {
        if ((string) strArray.GetValue(index) == ParameterName)
        {
          parameter = (string) strArray.GetValue(index + 1);
          break;
        }
      }
      return parameter;
    }

    public static string GetParameter(
      string ParameterString,
      string ParameterName,
      char NameValueSep,
      char PairSep)
    {
      string parameter = "";
      string str1 = PairSep.ToString() + ParameterString;
      int num = str1.IndexOf(PairSep.ToString() + ParameterName + NameValueSep.ToString());
      if (num >= 0)
      {
        int startIndex = num + (ParameterName.Length + 2);
        string str2 = str1.Substring(startIndex);
        int length = str2.IndexOf(PairSep);
        if (length < 0)
        {
          if (str2.Length > 0)
            parameter = str2;
        }
        else
          parameter = str2.Substring(0, length);
      }
      return parameter;
    }

    public static Dictionary<string, string> GetAllParametersAsList(
      string ParameterString,
      char PairSep)
    {
      Dictionary<string, string> parametersAsList = new Dictionary<string, string>();
      if (string.IsNullOrEmpty(ParameterString))
        return parametersAsList;
      ParameterString = ParameterString.TrimStart(';');
      string[] strArray = ParameterString.Split(PairSep);
      int num = 0;
      for (int index = 0; index < strArray.Length; index += 2)
      {
        string key = (string) strArray.GetValue(index);
        if (!string.IsNullOrEmpty(key))
        {
          if (parametersAsList.ContainsKey(key))
            key += num.ToString();
          parametersAsList.Add(key, (string) strArray.GetValue(index + 1));
        }
      }
      return parametersAsList;
    }

    public static double DoubleParseWithCheck(string TheInput)
    {
      return TheInput.IndexOf(SystemValues.ZRNumberGroupSeperator) < 0 ? double.Parse(TheInput) : throw new FormatException();
    }

    public static Decimal DecimalParseWithCheck(string TheInput)
    {
      return TheInput.IndexOf(SystemValues.ZRNumberGroupSeperator) < 0 ? Decimal.Parse(TheInput) : throw new FormatException();
    }

    public static float FloatParseWithCheck(string TheInput)
    {
      return !TheInput.Contains(SystemValues.ZRNumberGroupSeperator) ? float.Parse(TheInput) : throw new FormatException();
    }

    public static string GetCheckedDecimalString(string InputString)
    {
      bool flag = false;
      StringBuilder stringBuilder = new StringBuilder(InputString.Trim());
      for (int index = 0; index < stringBuilder.Length; ++index)
      {
        char c = stringBuilder[index];
        if (!char.IsDigit(c) && (c != '-' || index != 0))
        {
          if (c == '.' && !flag)
            flag = true;
          else if (c == ',' && !flag)
          {
            stringBuilder[index] = '.';
            flag = true;
          }
          else
          {
            stringBuilder.Remove(index, 1);
            ++index;
          }
        }
      }
      return stringBuilder.ToString();
    }

    public static string GetIntegerPartFromDecimalString(string InputString)
    {
      int length = InputString.IndexOf('.');
      return length < 0 ? InputString : InputString.Substring(0, length);
    }

    public static long GetIntegerPartFromDecimalStringAsLong(string InputString)
    {
      return long.Parse(ParameterService.GetIntegerPartFromDecimalString(InputString));
    }

    public static string GetFractionPartFromDecimalString(string InputString)
    {
      int num = InputString.IndexOf('.');
      return num < 0 || num == InputString.Length - 1 ? "0" : InputString.Substring(num + 1);
    }

    public static long GetFractionPartFromDecimalStringAsLong(string InputString)
    {
      return long.Parse(ParameterService.GetFractionPartFromDecimalString(InputString));
    }

    public static string FormatStringNumber(
      string InputString,
      int FeldWhite,
      int AfterPoint,
      bool FillWith_0,
      int ShiftExpo)
    {
      StringBuilder stringBuilder = new StringBuilder(InputString.Trim(), 30);
      int num = stringBuilder.ToString().IndexOf('.');
      if (AfterPoint > 0)
      {
        if (num < 0)
        {
          stringBuilder.Append('.');
          num = stringBuilder.Length - 1;
        }
        while (AfterPoint >= stringBuilder.Length - num)
          stringBuilder.Append('0');
        stringBuilder.Length = num + 1 + AfterPoint;
      }
      else if (num >= 0)
        stringBuilder.Length = num;
      while (stringBuilder.Length < FeldWhite)
      {
        if (FillWith_0)
          stringBuilder.Insert(0, "0");
        else
          stringBuilder.Insert(0, " ");
      }
      return stringBuilder.ToString();
    }

    public static void SetStringExpo(ref StringBuilder PValue, int UnitExponent)
    {
      for (int index = 0; index < UnitExponent; ++index)
        PValue.Append('0');
      if (UnitExponent < 0)
      {
        while (PValue.Length <= UnitExponent * -1)
          PValue.Insert(0, "0");
        PValue.Insert(PValue.Length + UnitExponent, ".");
      }
      while (PValue.Length > 1)
      {
        if (PValue[PValue.Length - 1] == '0' && UnitExponent < 0)
        {
          PValue.Remove(PValue.Length - 1, 1);
          ++UnitExponent;
        }
        else if (PValue[PValue.Length - 1] == '.')
        {
          PValue.Remove(PValue.Length - 1, 1);
        }
        else
        {
          if (PValue[0] != '0' || PValue[1] == '.')
            break;
          PValue.Remove(0, 1);
        }
      }
    }

    public static void ResetStringExpo(ref StringBuilder PValue, int UnitExponent)
    {
      int index = PValue.ToString().IndexOf('.');
      for (; UnitExponent > 0; --UnitExponent)
      {
        if (index < 0)
          PValue.Append('0');
        else if (index == PValue.Length - 1)
        {
          PValue[index] = '0';
          index = -1;
        }
        else
        {
          char ch = PValue[index + 1];
          PValue[index + 1] = '.';
          PValue[index] = ch;
          ++index;
        }
      }
      for (; UnitExponent < 0; ++UnitExponent)
      {
        if (index == 0 || PValue.Length < 2)
        {
          PValue[0] = '0';
          PValue.Length = 1;
          return;
        }
        if (index < 0)
        {
          --PValue.Length;
        }
        else
        {
          char ch = PValue[index - 1];
          PValue[index - 1] = '.';
          PValue[index] = ch;
          --index;
        }
      }
      if (index == 0)
      {
        PValue[0] = '0';
        PValue.Length = 1;
      }
      else if (index > 0)
        PValue.Length = index;
      while (PValue.Length > 1 && PValue[0] == '0')
        PValue.Remove(0, 1);
    }

    public static DateTime GetNow()
    {
      DateTime dateTime = !DateTime.Now.IsDaylightSavingTime() ? DateTime.Now : DateTime.Now.AddHours(-1.0);
      return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, 0);
    }

    [Obsolete("Please use GetVersionString(long version, int packetSizeOfResponceByGetVersionCommand)")]
    public static string GetVersionString(long version)
    {
      return ParameterService.GetVersionString(version, 7);
    }

    public static string GetVersionString(long version, int packetSizeOfResponceByGetVersionCommand)
    {
      if (version == 0L)
        return "not defined";
      long num1 = version >> 24 & (long) byte.MaxValue;
      string str1 = num1.ToString() + ".";
      num1 = version >> 16 & (long) byte.MaxValue;
      string str2 = num1.ToString();
      string str3 = str1 + str2 + ".";
      string versionString;
      if (version < 33554432L && packetSizeOfResponceByGetVersionCommand <= 7)
      {
        string str4 = str3;
        num1 = version & (long) ushort.MaxValue;
        string str5 = num1.ToString();
        string str6 = str4 + str5;
        versionString = version >= 17039360L ? str6 + ":C2" : str6 + ":C2e";
      }
      else
      {
        string str7 = str3;
        num1 = (version & 61440L) >> 12;
        string str8 = num1.ToString();
        string str9 = str7 + str8;
        long num2 = version & 4095L;
        long num3 = num2 - 1L;
        if ((ulong) num3 <= 46UL)
        {
          switch ((uint) num3)
          {
            case 0:
              versionString = str9 + ":C2";
              goto label_46;
            case 1:
              versionString = str9 + ":C2a";
              goto label_46;
            case 2:
              versionString = str9 + ":Puls";
              goto label_46;
            case 3:
              versionString = str9 + ":C3";
              goto label_46;
            case 4:
              versionString = str9 + ":C5";
              goto label_46;
            case 5:
              versionString = str9 + ":IUW";
              goto label_46;
            case 6:
            case 9:
            case 10:
            case 11:
            case 12:
            case 13:
            case 14:
            case 18:
            case 19:
            case 20:
            case 21:
            case 22:
            case 23:
            case 26:
            case 27:
            case 32:
            case 33:
            case 37:
            case 39:
            case 40:
            case 42:
            case 44:
              goto label_45;
            case 7:
              versionString = str9 + ":WR3";
              goto label_46;
            case 8:
              versionString = str9 + ":WR4";
              goto label_46;
            case 15:
              versionString = str9 + ":C4";
              goto label_46;
            case 16:
              versionString = str9 + ":EDC_Radio";
              goto label_46;
            case 17:
              versionString = str9 + ":EDC_mBus";
              goto label_46;
            case 24:
              versionString = str9 + ":EDC_LoRa";
              goto label_46;
            case 25:
              versionString = str9 + ":PDC_LoRa";
              goto label_46;
            case 28:
              versionString = str9 + ":Micro_LoRa";
              goto label_46;
            case 29:
              versionString = str9 + ":Micro_wMBus";
              goto label_46;
            case 30:
              versionString = str9 + ":EDC_wMBus_ST";
              goto label_46;
            case 31:
              versionString = str9 + ":EDC_LoRa_470 ";
              goto label_46;
            case 34:
              versionString = str9 + ":C5_LoRa";
              goto label_46;
            case 35:
              versionString = str9 + ":WR4_LoRa";
              goto label_46;
            case 36:
              versionString = str9 + ":Micro_LoRa_LL";
              goto label_46;
            case 38:
              versionString = str9 + ":NFC_LoRa";
              goto label_46;
            case 41:
              versionString = str9 + ":EDC_NBIoT";
              goto label_46;
            case 43:
              versionString = str9 + ":Micro_wMBus_LL";
              goto label_46;
            case 45:
              versionString = str9 + ":PDC_LoRa_915MHz";
              goto label_46;
            case 46:
              versionString = str9 + ":UDC_LoRa_915MHz";
              goto label_46;
          }
        }
        long num4 = num2 - 54L;
        if ((ulong) num4 <= 22UL)
        {
          switch ((uint) num4)
          {
            case 0:
              versionString = str9 + ":EDC_NBIoT_LCSW";
              goto label_46;
            case 1:
              versionString = str9 + ":NFC_wMBus_Connector";
              goto label_46;
            case 3:
              versionString = str9 + ":EDC_NBIoT_YJSW";
              goto label_46;
            case 4:
              versionString = str9 + ":PDC_LoRa_868MHz_SD";
              goto label_46;
            case 7:
              versionString = str9 + ":EDC_NBIoT_FSNH";
              goto label_46;
            case 8:
              versionString = str9 + ":EDC_NBIoT_XM";
              goto label_46;
            case 14:
              versionString = str9 + ":EDC_NBIoT_Israel";
              goto label_46;
            case 15:
              versionString = str9 + ":EDC_NBIoT_TaiWan";
              goto label_46;
            case 16:
              versionString = str9 + ":PDC_GAS";
              goto label_46;
            case 20:
              versionString = str9 + ":EDC_LoRa_868_v3";
              goto label_46;
            case 21:
              versionString = str9 + ":EDC_LoRa_915_v2_US";
              goto label_46;
            case 22:
              versionString = str9 + ":EDC_LoRa_915_v2_BR";
              goto label_46;
          }
        }
label_45:
        versionString = str9 + ":??";
label_46:;
      }
      return versionString;
    }

    public static string GetVersionNumberString(long version)
    {
      if (version == 0L)
        return "not defined";
      long num = version >> 24 & (long) byte.MaxValue;
      string str1 = num.ToString() + ".";
      num = version >> 16 & (long) byte.MaxValue;
      string str2 = num.ToString();
      string str3 = str1 + str2 + ".";
      num = (version & 61440L) >> 12;
      string str4 = num.ToString();
      return str3 + str4;
    }

    public static string GetHardwareString(uint HardwareMask)
    {
      if (HardwareMask == 0U)
        return "not defined";
      bool flag = false;
      StringBuilder stringBuilder1 = new StringBuilder(100);
      switch (HardwareMask & 3840U)
      {
        case 256:
          stringBuilder1.Append(ParameterService.HardwareMaskElements.UndefDev1.ToString());
          break;
        case 512:
          flag = true;
          stringBuilder1.Append(ParameterService.HardwareMaskElements.WR4.ToString());
          break;
        case 1024:
          stringBuilder1.Append(ParameterService.HardwareMaskElements.UndefDev2.ToString());
          break;
        case 2048:
          stringBuilder1.Append(ParameterService.HardwareMaskElements.C5.ToString());
          break;
        default:
          stringBuilder1.Append("IllegalDevice");
          break;
      }
      ParameterService.HardwareMaskElements hardwareMaskElements;
      switch (HardwareMask & 15U)
      {
        case 1:
          if (!flag)
          {
            StringBuilder stringBuilder2 = stringBuilder1;
            hardwareMaskElements = ParameterService.HardwareMaskElements.MBus;
            string str = ";" + hardwareMaskElements.ToString();
            stringBuilder2.Append(str);
            break;
          }
          break;
        case 2:
          StringBuilder stringBuilder3 = stringBuilder1;
          hardwareMaskElements = ParameterService.HardwareMaskElements.Radio;
          string str1 = ";" + hardwareMaskElements.ToString();
          stringBuilder3.Append(str1);
          break;
        case 4:
          StringBuilder stringBuilder4 = stringBuilder1;
          hardwareMaskElements = ParameterService.HardwareMaskElements.NoCom;
          string str2 = ";" + hardwareMaskElements.ToString();
          stringBuilder4.Append(str2);
          break;
        case 8:
          StringBuilder stringBuilder5 = stringBuilder1;
          hardwareMaskElements = ParameterService.HardwareMaskElements.LoRa;
          string str3 = ";" + hardwareMaskElements.ToString();
          stringBuilder5.Append(str3);
          break;
        default:
          StringBuilder stringBuilder6 = stringBuilder1;
          hardwareMaskElements = ParameterService.HardwareMaskElements.IllegalCom;
          string str4 = ";" + hardwareMaskElements.ToString();
          stringBuilder6.Append(str4);
          break;
      }
      if (!flag)
      {
        switch (HardwareMask & 240U)
        {
          case 16:
            StringBuilder stringBuilder7 = stringBuilder1;
            hardwareMaskElements = ParameterService.HardwareMaskElements.Compact;
            string str5 = ";" + hardwareMaskElements.ToString();
            stringBuilder7.Append(str5);
            break;
          case 32:
            StringBuilder stringBuilder8 = stringBuilder1;
            hardwareMaskElements = ParameterService.HardwareMaskElements.Combi;
            string str6 = ";" + hardwareMaskElements.ToString();
            stringBuilder8.Append(str6);
            break;
          case 64:
            StringBuilder stringBuilder9 = stringBuilder1;
            hardwareMaskElements = ParameterService.HardwareMaskElements.Ultrasonic;
            string str7 = ";" + hardwareMaskElements.ToString();
            stringBuilder9.Append(str7);
            break;
          case 128:
            StringBuilder stringBuilder10 = stringBuilder1;
            hardwareMaskElements = ParameterService.HardwareMaskElements.UltrasonicDirect;
            string str8 = ";" + hardwareMaskElements.ToString();
            stringBuilder10.Append(str8);
            break;
          default:
            stringBuilder1.Append(";IllegalVol");
            break;
        }
      }
      else
      {
        switch (HardwareMask & 240U)
        {
          case 16:
            stringBuilder1.Append("; PT100");
            break;
          case 32:
            stringBuilder1.Append("; PT500");
            break;
          case 64:
            stringBuilder1.Append("; PT1000");
            break;
          default:
            stringBuilder1.Append(";Illegal_PT_sensor");
            break;
        }
      }
      return stringBuilder1.ToString();
    }

    public static ParameterService.HardwareMaskElements GetVolumeHardware(uint HardwareMask)
    {
      return (ParameterService.HardwareMaskElements) ((int) HardwareMask & 240);
    }

    public static bool GetAssamblyVersionParts(
      string FullName,
      out string Name,
      out int Version,
      out string VersionString,
      out int Revision,
      out DateTime BuildTime,
      out int Hashcode)
    {
      Name = "";
      Version = 0;
      Revision = 0;
      VersionString = "";
      BuildTime = new DateTime(2000, 1, 1);
      Hashcode = 0;
      try
      {
        string[] strArray1 = FullName.Split(',');
        if (strArray1.Length != 0)
          Name = strArray1[0];
        if (strArray1.Length > 1)
        {
          string[] strArray2 = strArray1[1].Split('=');
          VersionString = strArray2[1].Trim();
          string[] strArray3 = VersionString.Split('.');
          Version = int.Parse(strArray3[0]);
          Revision = int.Parse(strArray3[1]);
          BuildTime = BuildTime.AddDays((double) int.Parse(strArray3[2]));
          BuildTime = BuildTime.AddSeconds((double) (int.Parse(strArray3[3]) * 2));
        }
        if (strArray1.Length > 4)
        {
          string[] strArray4 = strArray1[4].Split('=');
          if (strArray4.Length == 2 && strArray4[0].Trim() == nameof (Hashcode))
            Hashcode = int.Parse(strArray4[1]);
        }
        return true;
      }
      catch
      {
        return false;
      }
    }

    public static ushort GetFromByteArray_ushort(byte[] TheArray, ref int StartOffset)
    {
      return (ushort) ((int) (ushort) TheArray[StartOffset++] + (int) (ushort) ((uint) TheArray[StartOffset++] << 8));
    }

    public static uint GetFromByteArray_uint(byte[] TheArray, ref int StartOffset)
    {
      return (uint) ((int) TheArray[StartOffset++] + ((int) TheArray[StartOffset++] << 8) + ((int) TheArray[StartOffset++] << 16) + ((int) TheArray[StartOffset++] << 24));
    }

    public static ulong GetFromByteArray_ulong(byte[] TheArray, ref int StartOffset)
    {
      return (ulong) ((long) TheArray[StartOffset++] + ((long) TheArray[StartOffset++] << 8) + ((long) TheArray[StartOffset++] << 16) + ((long) TheArray[StartOffset++] << 24) + ((long) TheArray[StartOffset++] << 32) + ((long) TheArray[StartOffset++] << 40) + ((long) TheArray[StartOffset++] << 48) + ((long) TheArray[StartOffset++] << 56));
    }

    public static short GetFromByteArray_short(byte[] TheArray, ref int StartOffset)
    {
      return (short) ((int) (short) TheArray[StartOffset++] + (int) (short) ((int) TheArray[StartOffset++] << 8));
    }

    public static int GetFromByteArray_int(byte[] TheArray, ref int StartOffset)
    {
      return (int) TheArray[StartOffset++] + ((int) TheArray[StartOffset++] << 8) + ((int) TheArray[StartOffset++] << 16) + ((int) TheArray[StartOffset++] << 24);
    }

    public static long GetFromByteArray_long(byte[] TheArray, ref int StartOffset)
    {
      return (long) TheArray[StartOffset++] + ((long) TheArray[StartOffset++] << 8) + ((long) TheArray[StartOffset++] << 16) + ((long) TheArray[StartOffset++] << 24) + ((long) TheArray[StartOffset++] << 32) + ((long) TheArray[StartOffset++] << 40) + ((long) TheArray[StartOffset++] << 48) + ((long) TheArray[StartOffset++] << 56);
    }

    public static ulong GetFromByteArray_ulong(byte[] TheArray, ushort Size, ref int StartOffset)
    {
      ulong fromByteArrayUlong = 0;
      for (int index = 0; index < (int) Size; ++index)
        fromByteArrayUlong += (ulong) TheArray[StartOffset++] << 8 * index;
      return fromByteArrayUlong;
    }

    public static int GetFromByteArray_int_from_BCD(byte[] TheArray, ref int StartOffset)
    {
      uint fromByteArrayUint = ParameterService.GetFromByteArray_uint(TheArray, ref StartOffset);
      uint byteArrayIntFromBcd = 0;
      uint num = 1;
      for (int index = 0; index < 8; ++index)
      {
        byteArrayIntFromBcd += (fromByteArrayUint & 15U) * num;
        num *= 10U;
        fromByteArrayUint >>= 4;
      }
      return (int) byteArrayIntFromBcd;
    }

    public static void SetToByteArray_ushort(
      ushort Value,
      ref byte[] TheArray,
      ref int StartOffset)
    {
      TheArray[StartOffset++] = (byte) ((uint) Value & (uint) byte.MaxValue);
      TheArray[StartOffset++] = (byte) ((int) Value >> 8 & (int) byte.MaxValue);
    }

    public static void SetToByteArray_uint(uint Value, ref byte[] TheArray, ref int StartOffset)
    {
      TheArray[StartOffset++] = (byte) (Value & (uint) byte.MaxValue);
      TheArray[StartOffset++] = (byte) (Value >> 8 & (uint) byte.MaxValue);
      TheArray[StartOffset++] = (byte) (Value >> 16 & (uint) byte.MaxValue);
      TheArray[StartOffset++] = (byte) (Value >> 24 & (uint) byte.MaxValue);
    }

    public static void SetToByteArray_ulong(ulong Value, ref byte[] TheArray, ref int StartOffset)
    {
      TheArray[StartOffset++] = (byte) (Value & (ulong) byte.MaxValue);
      TheArray[StartOffset++] = (byte) (Value >> 8 & (ulong) byte.MaxValue);
      TheArray[StartOffset++] = (byte) (Value >> 16 & (ulong) byte.MaxValue);
      TheArray[StartOffset++] = (byte) (Value >> 24 & (ulong) byte.MaxValue);
      TheArray[StartOffset++] = (byte) (Value >> 32 & (ulong) byte.MaxValue);
      TheArray[StartOffset++] = (byte) (Value >> 40 & (ulong) byte.MaxValue);
      TheArray[StartOffset++] = (byte) (Value >> 48 & (ulong) byte.MaxValue);
      TheArray[StartOffset++] = (byte) (Value >> 56 & (ulong) byte.MaxValue);
    }

    public static void SetToByteArray_short(short Value, ref byte[] TheArray, ref int StartOffset)
    {
      TheArray[StartOffset++] = (byte) ((uint) Value & (uint) byte.MaxValue);
      TheArray[StartOffset++] = (byte) ((int) Value >> 8 & (int) byte.MaxValue);
    }

    public static void SetToByteArray_int(int Value, ref byte[] TheArray, ref int StartOffset)
    {
      TheArray[StartOffset++] = (byte) (Value & (int) byte.MaxValue);
      TheArray[StartOffset++] = (byte) (Value >> 8 & (int) byte.MaxValue);
      TheArray[StartOffset++] = (byte) (Value >> 16 & (int) byte.MaxValue);
      TheArray[StartOffset++] = (byte) (Value >> 24 & (int) byte.MaxValue);
    }

    public static void SetToByteArray_long(long Value, ref byte[] TheArray, ref int StartOffset)
    {
      TheArray[StartOffset++] = (byte) ((ulong) Value & (ulong) byte.MaxValue);
      TheArray[StartOffset++] = (byte) ((ulong) (Value >> 8) & (ulong) byte.MaxValue);
      TheArray[StartOffset++] = (byte) ((ulong) (Value >> 16) & (ulong) byte.MaxValue);
      TheArray[StartOffset++] = (byte) ((ulong) (Value >> 24) & (ulong) byte.MaxValue);
      TheArray[StartOffset++] = (byte) ((ulong) (Value >> 32) & (ulong) byte.MaxValue);
      TheArray[StartOffset++] = (byte) ((ulong) (Value >> 40) & (ulong) byte.MaxValue);
      TheArray[StartOffset++] = (byte) ((ulong) (Value >> 48) & (ulong) byte.MaxValue);
      TheArray[StartOffset++] = (byte) ((ulong) (Value >> 56) & (ulong) byte.MaxValue);
    }

    public static char GetCharacterCode(int InChar)
    {
      return ParameterService.CodeTranslatorTable[InChar & 31];
    }

    public static int GetIntFromCharacterCode(char InChar)
    {
      for (int fromCharacterCode = 0; fromCharacterCode < ParameterService.CodeTranslatorTable.Length; ++fromCharacterCode)
      {
        if ((int) ParameterService.CodeTranslatorTable[fromCharacterCode] == (int) InChar)
          return fromCharacterCode;
      }
      return -1;
    }

    public static int GetIntFromCharacterCodeString(string InCharString)
    {
      int characterCodeString = -1;
      int num = 0;
      for (int index = InCharString.Length - 1; index >= 0; --index)
      {
        char InChar = InCharString[index];
        if (InChar != ' ')
        {
          int fromCharacterCode = ParameterService.GetIntFromCharacterCode(InChar);
          if (fromCharacterCode < 0)
            return -1;
          if (num == 0)
            characterCodeString = fromCharacterCode;
          else
            characterCodeString |= fromCharacterCode << num;
          num += 5;
        }
      }
      return characterCodeString;
    }

    public static string GetCharacterCodeStringFromInt(int IntValue, int MinStringLen)
    {
      StringBuilder stringBuilder;
      for (stringBuilder = new StringBuilder(20); IntValue > 0 || stringBuilder.Length < MinStringLen; IntValue >>= 5)
        stringBuilder.Insert(0, ParameterService.GetCharacterCode(IntValue));
      return stringBuilder.ToString();
    }

    public static string GetHexStringFromByteArray(byte[] byteArray, int startOffset)
    {
      return ParameterService.GetHexStringFromByteArray(byteArray, startOffset, byteArray.Length - startOffset - 1);
    }

    public static string GetHexStringFromByteArray(
      byte[] byteArray,
      int startOffset,
      int endOffset)
    {
      if (byteArray == null)
        throw new NullReferenceException(nameof (byteArray));
      if (startOffset < 0 || startOffset >= byteArray.Length)
        throw new IndexOutOfRangeException(nameof (startOffset));
      if (endOffset >= byteArray.Length)
        throw new IndexOutOfRangeException(nameof (endOffset));
      StringBuilder stringBuilder = endOffset >= startOffset ? new StringBuilder(endOffset - startOffset + 1) : throw new Exception("endOffset < startOffset");
      int index = startOffset;
      while (true)
      {
        stringBuilder.Append(byteArray[index].ToString("x02"));
        if (index != endOffset)
        {
          stringBuilder.Append(" ");
          ++index;
        }
        else
          break;
      }
      return stringBuilder.ToString();
    }

    public static string GetHexStringFromByteArray(byte[] ByteArray)
    {
      StringBuilder stringBuilder = new StringBuilder(100);
      for (int index = 0; index < ByteArray.Length; ++index)
        stringBuilder.Append(" " + ByteArray[index].ToString("x02"));
      return stringBuilder.ToString();
    }

    public static string GetStringFromByteArray(byte[] ByteArray)
    {
      StringBuilder stringBuilder = new StringBuilder(100);
      for (int index = 0; index < ByteArray.Length; ++index)
        stringBuilder.Append((char) ByteArray[index]);
      return ParameterService.GetExpandesString(stringBuilder.ToString());
    }

    public static string GetExpandesString(string TheString)
    {
      TheString = TheString.Replace("\r", "\\r");
      TheString = TheString.Replace("\n", "\\n");
      return TheString;
    }

    public static List<string> GetKeys(string zdf)
    {
      List<string> keys = new List<string>();
      string[] strArray1 = zdf.Split(';');
      int num;
      for (int index1 = 0; index1 < strArray1.Length; index1 = num + 1)
      {
        List<string> stringList = keys;
        string[] strArray2 = strArray1;
        int index2 = index1;
        num = index2 + 1;
        string str = strArray2[index2];
        stringList.Add(str);
      }
      return keys;
    }

    public static List<string> GetValues(string zdf)
    {
      List<string> values = new List<string>();
      string[] strArray1 = zdf.Split(';');
      int num;
      for (int index1 = 1; index1 < strArray1.Length; index1 = num + 1)
      {
        List<string> stringList = values;
        string[] strArray2 = strArray1;
        int index2 = index1;
        num = index2 + 1;
        string str = strArray2[index2];
        stringList.Add(str);
      }
      return values;
    }

    public static string ConvertInt32ToHexString(int value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      return string.Format("{0}{1}{2}{3}", (object) bytes[3].ToString("X2"), (object) bytes[2].ToString("X2"), (object) bytes[1].ToString("X2"), (object) bytes[0].ToString("X2"));
    }

    public static uint ConvertHexStringToUInt32(string value)
    {
      if (string.IsNullOrEmpty(value))
        throw new ArgumentNullException(nameof (value));
      string str = value.Length <= 8 ? value.PadLeft(8, '0') : throw new ArgumentException("Invalid size of value!");
      byte[] numArray = new byte[4]
      {
        (byte) 0,
        (byte) 0,
        (byte) 0,
        Convert.ToByte(str.Substring(0, 2), 16)
      };
      numArray[2] = Convert.ToByte(str.Substring(2, 2), 16);
      numArray[1] = Convert.ToByte(str.Substring(4, 2), 16);
      numArray[0] = Convert.ToByte(str.Substring(6, 2), 16);
      return BitConverter.ToUInt32(numArray, 0);
    }

    public static string[] GetMemoryInfo(ByteField MemoryData, int BufferAddressOffset)
    {
      int num1 = BufferAddressOffset;
      int num2 = num1 & -16;
      int num3 = BufferAddressOffset + MemoryData.Count - 1;
      int num4 = num3 | 15;
      string[] memoryInfo = new string[(num4 + 1 - num2) / 16];
      StringBuilder stringBuilder = new StringBuilder();
      int num5 = 0;
      for (; num2 <= num4; ++num2)
      {
        if ((num2 & 15) == 0)
        {
          stringBuilder.Length = 0;
          stringBuilder.Append(num2.ToString("x04") + ": ");
        }
        stringBuilder.Append(" ");
        if (num2 < num1 || num2 > num3)
          stringBuilder.Append("..");
        else
          stringBuilder.Append(MemoryData.Data[num2 - BufferAddressOffset].ToString("x02"));
        if ((num2 & 15) == 15)
        {
          memoryInfo[num5++] = stringBuilder.ToString();
          stringBuilder.Length = 0;
        }
      }
      return memoryInfo;
    }

    public static string DeleteAllAsynComSettings(string settings)
    {
      string[] namesOfEnum = Util.GetNamesOfEnum(typeof (AsyncComSettings));
      if (namesOfEnum == null)
        return settings;
      foreach (string ParameterName in namesOfEnum)
        ParameterService.DeleteParameter(ref settings, ParameterName);
      return settings;
    }

    public static string DeleteAllDeviceCollectorSettings(string settings)
    {
      string[] namesOfEnum = Util.GetNamesOfEnum(typeof (DeviceCollectorSettings));
      if (namesOfEnum == null)
        return settings;
      foreach (string ParameterName in namesOfEnum)
        ParameterService.DeleteParameter(ref settings, ParameterName);
      return settings;
    }

    internal struct S3_ValueTypeTranslation
    {
      internal S3_VariableTypes S3_Type;
      internal Type NET_Type;
      internal int Bytes;

      internal S3_ValueTypeTranslation(S3_VariableTypes S3_Type, Type NET_Type, int Bytes)
      {
        this.S3_Type = S3_Type;
        this.NET_Type = NET_Type;
        this.Bytes = Bytes;
      }
    }

    public enum HardwareMaskElements
    {
      MBus = 1,
      Radio = 2,
      NoCom = 4,
      LoRa = 8,
      IllegalCom = 9,
      ComMask = 15, // 0x0000000F
      Compact = 16, // 0x00000010
      Combi = 32, // 0x00000020
      Ultrasonic = 64, // 0x00000040
      UltrasonicDirect = 128, // 0x00000080
      VolTypeMask = 240, // 0x000000F0
      UndefDev1 = 256, // 0x00000100
      WR4 = 512, // 0x00000200
      UndefDev2 = 1024, // 0x00000400
      C5 = 2048, // 0x00000800
      VolDeviceTypeMask = 3840, // 0x00000F00
    }

    public enum HardwareMaskElementsWR4
    {
      PT_100 = 16, // 0x00000010
      PT_500 = 32, // 0x00000020
      PT_1000 = 64, // 0x00000040
      WR4_PT_Mask = 240, // 0x000000F0
    }
  }
}

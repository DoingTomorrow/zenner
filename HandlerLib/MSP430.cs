// Decompiled with JetBrains decompiler
// Type: HandlerLib.MSP430
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;

#nullable disable
namespace HandlerLib
{
  public static class MSP430
  {
    public static List<Parameter> LoadParameter(ResourceManager resourceManager, uint version)
    {
      return MSP430.LoadParameter(resourceManager, version, (string[]) null, (string[]) null);
    }

    public static List<Parameter> LoadParameter(
      ResourceManager resourceManager,
      uint version,
      string[] ignoreSegments)
    {
      return MSP430.LoadParameter(resourceManager, version, ignoreSegments, (string[]) null);
    }

    public static List<Parameter> LoadParameter(
      ResourceManager resourceManager,
      uint version,
      string[] ignoreSegments,
      string[] ignorePrefixes)
    {
      if (resourceManager == null)
        throw new NullReferenceException(nameof (resourceManager));
      foreach (DictionaryEntry resource in resourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true))
      {
        string str = resource.Key.ToString();
        uint result;
        if (str.Length == 11 && str.StartsWith("_0x") && uint.TryParse(str.Substring(3), NumberStyles.HexNumber, (IFormatProvider) null, out result) && (int) result == (int) version)
          return MSP430.LoadParameter(Encoding.ASCII.GetString(resource.Value as byte[]), ignoreSegments, ignorePrefixes);
      }
      return (List<Parameter>) null;
    }

    public static List<Parameter> LoadParameter(string mapContent)
    {
      return MSP430.LoadParameter(mapContent, (string[]) null, (string[]) null);
    }

    public static List<Parameter> LoadParameter(string mapContent, string[] ignoreSegments)
    {
      return MSP430.LoadParameter(mapContent, ignoreSegments, (string[]) null);
    }

    public static List<Parameter> LoadParameter(
      string mapContent,
      string[] ignoreSegments,
      string[] ignorePrefixes)
    {
      string[] map = !string.IsNullOrEmpty(mapContent) ? mapContent.Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.RemoveEmptyEntries) : throw new NullReferenceException(nameof (mapContent));
      List<Parameter> result = new List<Parameter>();
      List<string> stringList1 = new List<string>()
      {
        "DATA16_AN",
        "DATA16_AC",
        "DATA16_ID",
        "CSTART",
        "RESET",
        "?FILL",
        "CODE",
        "CHECKSUM"
      };
      List<string> stringList2 = new List<string>();
      if (ignoreSegments != null)
        stringList1.AddRange((IEnumerable<string>) ignoreSegments);
      if (ignorePrefixes != null)
        stringList2.AddRange((IEnumerable<string>) ignorePrefixes);
      for (int i = 0; i < map.Length; ++i)
      {
        string str1 = map[i];
        if (str1.Contains("Relative segment, address:"))
        {
          string segment = map[i - 1];
          if (!stringList1.Exists((Predicate<string>) (x => segment.Contains(x))))
          {
            string[] strArray1 = str1.Split(' ');
            if (strArray1.Length == 12)
            {
              ushort address = ushort.Parse(strArray1[5], NumberStyles.HexNumber);
              ushort num1 = ushort.Parse(strArray1[7], NumberStyles.HexNumber);
              string str2;
              do
              {
                str2 = map[++i];
                if (str2.Contains("*************************************************************************") || str2.Contains("-------------------------------------------------------------------------"))
                  goto label_27;
              }
              while (!str2.Contains("===== "));
              string str3 = MSP430.TrimSpaces(map[++i]);
              string[] strArray2 = str3.Split(' ');
              string name = strArray2[0];
              ushort num2 = 0;
              if (strArray2.Length >= 2)
                num2 = ushort.Parse(strArray2[1], NumberStyles.HexNumber);
              else if (strArray2.Length == 1)
              {
                str3 = MSP430.TrimSpaces(map[++i]);
                num2 = ushort.Parse(str3.Split(' ')[0], NumberStyles.HexNumber);
              }
              if ((int) num2 != (int) address)
                throw new Exception("Internal error while parse the map file! Wrong address detected. " + str3);
              if (result.Exists((Predicate<Parameter>) (x => x.Name == name)))
                throw new Exception("Internal error while parse the map file! The parameter already exists. " + str3);
              if (stringList2.Count <= 0 || !stringList2.Exists((Predicate<string>) (x => name.StartsWith(x))))
                result.Add(new Parameter(segment, name, address, (int) (ushort) ((int) num1 - (int) address + 1)));
            }
            else if (strArray1.Length != 8)
              throw new Exception("Internal error while parse the map file! Unknown line detected: " + str1);
          }
          else
            continue;
        }
        else if (str1.StartsWith("SEGMENT") && str1.Contains("START ADDRESS"))
          MSP430.ParseSegments(map, i, result);
label_27:;
      }
      return result;
    }

    private static void ParseSegments(string[] map, int i, List<Parameter> result)
    {
      string line;
      while (true)
      {
        do
        {
          line = map[++i];
        }
        while (line.StartsWith("=======") || line.StartsWith("DATA16_AN") || line.StartsWith(" "));
        if (!line.StartsWith("CSTART"))
        {
          string[] strArray = MSP430.TrimSpaces(line).Split(' ');
          if (strArray.Length == 1)
          {
            line += map[++i];
            strArray = MSP430.TrimSpaces(line).Split(' ');
          }
          string name = strArray[0];
          if (strArray.Length == 7)
          {
            ushort address = ushort.Parse(strArray[1], NumberStyles.HexNumber);
            ushort size = ushort.Parse(strArray[4], NumberStyles.HexNumber);
            if (name.StartsWith("?FILL"))
            {
              Parameter parameter = result[result.Count - 1];
              result.RemoveAt(result.Count - 1);
              result.Add(new Parameter("SEGMENT", parameter.Name, parameter.Address, parameter.Size + (int) size));
            }
            else if (!result.Exists((Predicate<Parameter>) (x => x.Name == name)))
              result.Add(new Parameter("SEGMENT", name, address, (int) size));
            else
              goto label_8;
          }
          else if (strArray.Length == 4)
          {
            ushort address = ushort.Parse(strArray[1], NumberStyles.HexNumber);
            if (!result.Exists((Predicate<Parameter>) (x => x.Name == name)))
              result.Add(new Parameter("SEGMENT", name, address, 0));
            else
              goto label_13;
          }
          else
            goto label_15;
        }
        else
          break;
      }
      return;
label_8:
      throw new Exception("Internal error while parse the map file! The parameter already exists. " + line);
label_13:
      throw new Exception("Internal error while parse the map file! The parameter already exists. " + line);
label_15:
      throw new Exception("Internal error while parse the map file! Unknown segment structure. " + line);
    }

    private static string TrimSpaces(string line)
    {
      StringBuilder stringBuilder = new StringBuilder();
      char[] charArray = line.ToCharArray();
      bool flag = false;
      foreach (char c in charArray)
      {
        if (char.IsWhiteSpace(c))
        {
          if (flag)
          {
            stringBuilder.Append(c);
            flag = false;
          }
        }
        else
        {
          stringBuilder.Append(c);
          flag = true;
        }
      }
      return stringBuilder.ToString().TrimEnd();
    }
  }
}

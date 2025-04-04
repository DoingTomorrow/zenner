// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.Utils.AddressUtility
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Text.RegularExpressions;

#nullable disable
namespace OfficeOpenXml.Utils
{
  public static class AddressUtility
  {
    public static string ParseEntireColumnSelections(string address)
    {
      string address1 = address;
      foreach (Match match in Regex.Matches(address, "[A-Z]+:[A-Z]+"))
        AddressUtility.AddRowNumbersToEntireColumnRange(ref address1, match.Value);
      return address1;
    }

    private static void AddRowNumbersToEntireColumnRange(ref string address, string range)
    {
      string[] strArray = string.Format("{0}{1}", (object) range, (object) 1048576).Split(':');
      address = address.Replace(range, string.Format("{0}1:{1}", (object) strArray[0], (object) strArray[1]));
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExcelUtilities.WildCardValueMatcher
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Text.RegularExpressions;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExcelUtilities
{
  public class WildCardValueMatcher : ValueMatcher
  {
    protected override int CompareStringToString(string s1, string s2)
    {
      if (s1.Contains("*") || s1.Contains("?"))
      {
        string pattern = string.Format("^{0}$", (object) Regex.Escape(s1)).Replace("\\*", ".*").Replace("\\?", ".");
        if (Regex.IsMatch(s2, pattern))
          return 0;
      }
      return base.CompareStringToString(s1, s2);
    }
  }
}

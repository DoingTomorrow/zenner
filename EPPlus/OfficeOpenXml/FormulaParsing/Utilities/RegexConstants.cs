// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Utilities.RegexConstants
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Utilities
{
  public static class RegexConstants
  {
    public const string SingleCellAddress = "^(('[^/\\\\?*\\[\\]]{1,31}'|[A-Za-z_]{1,31})!)?[A-Z]{1,3}[1-9]{1}[0-9]{0,7}$";
    public const string ExcelAddress = "^(('[^/\\\\?*\\[\\]]{1,31}'|[A-Za-z_]{1,31})!)?[\\$]{0,1}([A-Z]|[A-Z]{1,3}[\\$]{0,1}[1-9]{1}[0-9]{0,7})(\\:({0,1}[A-Z]|[A-Z]{1,3}[\\$]{0,1}[1-9]{1}[0-9]{0,7})){0,1}$";
    public const string Boolean = "^(true|false)$";
    public const string Decimal = "^[0-9]+\\.[0-9]+$";
    public const string Integer = "^[0-9]+$";
  }
}

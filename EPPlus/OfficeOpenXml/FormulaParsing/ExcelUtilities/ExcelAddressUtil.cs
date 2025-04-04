// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExcelUtilities.ExcelAddressUtil
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExcelUtilities
{
  public static class ExcelAddressUtil
  {
    private static char[] SheetNameInvalidChars = new char[5]
    {
      '?',
      ':',
      '*',
      '/',
      '\\'
    };

    public static bool IsValidAddress(string token)
    {
      if (token[0] == '\'')
      {
        int num = token.LastIndexOf('\'');
        if (num <= 0 || num >= token.Length - 1 || token[num + 1] != '!' || token.IndexOfAny(ExcelAddressUtil.SheetNameInvalidChars, 1, num - 1) > 0)
          return false;
        token = token.Substring(num + 2);
      }
      else if (token.IndexOf('!') > 1)
      {
        if (token.IndexOfAny(ExcelAddressUtil.SheetNameInvalidChars, 0, token.IndexOf('!')) > 0)
          return false;
        token = token.Substring(token.IndexOf('!') + 1);
      }
      return ExcelCellBase.IsValidAddress(token);
    }
  }
}

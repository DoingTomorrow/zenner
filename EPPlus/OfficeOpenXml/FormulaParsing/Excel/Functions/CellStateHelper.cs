// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.CellStateHelper
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions
{
  internal static class CellStateHelper
  {
    private static bool IsSubTotal(ExcelDataProvider.ICellInfo c)
    {
      return c.Tokens != null && c.Tokens.Any<Token>((Func<Token, bool>) (token => token.TokenType == OfficeOpenXml.FormulaParsing.LexicalAnalysis.TokenType.Function && token.Value.Equals("SUBTOTAL", StringComparison.InvariantCultureIgnoreCase)));
    }

    internal static bool ShouldIgnore(
      bool ignoreHiddenValues,
      ExcelDataProvider.ICellInfo c,
      ParsingContext context)
    {
      if (ignoreHiddenValues && c.IsHiddenRow)
        return true;
      return context.Scopes.Current.IsSubtotal && CellStateHelper.IsSubTotal(c);
    }

    internal static bool ShouldIgnore(
      bool ignoreHiddenValues,
      FunctionArgument arg,
      ParsingContext context)
    {
      return ignoreHiddenValues && arg.ExcelStateFlagIsSet(ExcelCellState.HiddenCell);
    }
  }
}

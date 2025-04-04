// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExcelUtilities.CellReferenceProvider
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExcelUtilities
{
  public class CellReferenceProvider
  {
    public virtual IEnumerable<string> GetReferencedAddresses(
      string cellFormula,
      ParsingContext context)
    {
      List<string> referencedAddresses = new List<string>();
      foreach (Token token in context.Configuration.Lexer.Tokenize(cellFormula).Where<Token>((Func<Token, bool>) (x => x.TokenType == OfficeOpenXml.FormulaParsing.LexicalAnalysis.TokenType.ExcelAddress)))
      {
        RangeAddress rangeAddress = context.RangeAddressFactory.Create(token.Value);
        List<string> collection = new List<string>();
        if (rangeAddress.FromRow < rangeAddress.ToRow || rangeAddress.FromCol < rangeAddress.ToCol)
        {
          for (int fromCol = rangeAddress.FromCol; fromCol <= rangeAddress.ToCol; ++fromCol)
          {
            for (int fromRow = rangeAddress.FromRow; fromRow <= rangeAddress.ToRow; ++fromRow)
              referencedAddresses.Add(context.RangeAddressFactory.Create(fromCol, fromRow).Address);
          }
        }
        else
          collection.Add(token.Value);
        referencedAddresses.AddRange((IEnumerable<string>) collection);
      }
      return (IEnumerable<string>) referencedAddresses;
    }
  }
}

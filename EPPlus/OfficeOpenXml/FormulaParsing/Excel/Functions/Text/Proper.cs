// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Text.Proper
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Text
{
  public class Proper : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 1);
      string lower = this.ArgToString(arguments, 0).ToLower();
      StringBuilder stringBuilder = new StringBuilder();
      char c = '.';
      foreach (char ch in lower)
      {
        if (!char.IsLetter(c))
          stringBuilder.Append(ch.ToString((IFormatProvider) CultureInfo.InvariantCulture).ToUpperInvariant());
        else
          stringBuilder.Append(ch);
        c = ch;
      }
      return this.CreateResult((object) stringBuilder.ToString(), DataType.String);
    }
  }
}

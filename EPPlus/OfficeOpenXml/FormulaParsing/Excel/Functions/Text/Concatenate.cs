// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Text.Concatenate
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Text
{
  public class Concatenate : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      if (arguments == null)
        return this.CreateResult((object) string.Empty, DataType.String);
      StringBuilder stringBuilder = new StringBuilder();
      foreach (FunctionArgument functionArgument in arguments)
      {
        object valueFirst = functionArgument.ValueFirst;
        if (valueFirst != null)
          stringBuilder.Append(valueFirst.ToString());
      }
      return this.CreateResult((object) stringBuilder.ToString(), DataType.String);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime.Day
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime
{
  public class Day : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 1);
      object firstValue = this.GetFirstValue(arguments);
      System.DateTime dateTime = System.DateTime.MinValue;
      if (firstValue is double d)
        dateTime = System.DateTime.FromOADate(d);
      if (firstValue is string)
        dateTime = System.DateTime.Parse(firstValue.ToString());
      return this.CreateResult((object) dateTime.Day, DataType.Integer);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime.Year
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime
{
  public class Year : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 1);
      object obj = arguments.ElementAt<FunctionArgument>(0).Value;
      System.DateTime dateTime = System.DateTime.MinValue;
      if (obj is double d)
        dateTime = System.DateTime.FromOADate(d);
      if (obj is string)
        dateTime = System.DateTime.Parse(obj.ToString());
      return this.CreateResult((object) dateTime.Year, DataType.Integer);
    }
  }
}

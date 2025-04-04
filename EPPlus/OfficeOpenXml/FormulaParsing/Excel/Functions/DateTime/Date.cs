// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime.Date
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime
{
  public class Date : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 3);
      int year = this.ArgToInt(arguments, 0);
      int num1 = this.ArgToInt(arguments, 1);
      int num2 = this.ArgToInt(arguments, 2);
      System.DateTime dateTime = new System.DateTime(year, 1, 1);
      int months = num1 - 1;
      dateTime = dateTime.AddMonths(months);
      dateTime = dateTime.AddDays((double) (num2 - 1));
      return this.CreateResult((object) dateTime.ToOADate(), DataType.Date);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime.Eomonth
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime
{
  public class Eomonth : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 2);
      System.DateTime dateTime = System.DateTime.FromOADate(this.ArgToDecimal(arguments, 0));
      int num = this.ArgToInt(arguments, 1);
      return this.CreateResult((object) new System.DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(num + 1).AddDays(-1.0).ToOADate(), DataType.Date);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime.Edate
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime
{
  public class Edate : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 2, eErrorType.Value);
      return this.CreateResult((object) System.DateTime.FromOADate(this.ArgToDecimal(arguments, 0)).AddMonths(this.ArgToInt(arguments, 1)).ToOADate(), DataType.Date);
    }
  }
}

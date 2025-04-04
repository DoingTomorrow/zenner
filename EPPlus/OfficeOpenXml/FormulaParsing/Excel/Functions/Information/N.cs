// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Information.N
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Exceptions;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using OfficeOpenXml.Utils;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Information
{
  public class N : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 1);
      object firstValue = this.GetFirstValue(arguments);
      if (firstValue is bool flag)
        return this.CreateResult((object) (flag ? 1.0 : 0.0), DataType.Decimal);
      if (this.IsNumeric(firstValue))
        return this.CreateResult((object) ConvertUtil.GetValueDouble(firstValue), DataType.Decimal);
      switch (firstValue)
      {
        case string _:
          return this.CreateResult((object) 0.0, DataType.Decimal);
        case ExcelErrorValue _:
          return this.CreateResult(firstValue, DataType.ExcelError);
        default:
          throw new ExcelErrorValueException(eErrorType.Value);
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Information.IsEven
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using OfficeOpenXml.Utils;
using System;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Information
{
  public class IsEven : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 1);
      object firstValue = this.GetFirstValue(arguments);
      if (!ConvertUtil.IsNumeric(firstValue))
        this.ThrowExcelErrorValueException(eErrorType.Value);
      return this.CreateResult((object) ((int) Math.Floor(ConvertUtil.GetValueDouble(firstValue)) % 2 == 0), DataType.Boolean);
    }
  }
}

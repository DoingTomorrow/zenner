// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Text.Find
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Exceptions;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Text
{
  public class Find : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      if (!(arguments is FunctionArgument[] functionArgumentArray1))
        functionArgumentArray1 = arguments.ToArray<FunctionArgument>();
      FunctionArgument[] functionArgumentArray2 = functionArgumentArray1;
      this.ValidateArguments((IEnumerable<FunctionArgument>) functionArgumentArray2, 2);
      string str1 = this.ArgToString((IEnumerable<FunctionArgument>) functionArgumentArray2, 0);
      string str2 = this.ArgToString((IEnumerable<FunctionArgument>) functionArgumentArray2, 1);
      int startIndex = 0;
      if (((IEnumerable<FunctionArgument>) functionArgumentArray2).Count<FunctionArgument>() > 2)
        startIndex = this.ArgToInt((IEnumerable<FunctionArgument>) functionArgumentArray2, 2);
      int num = str2.IndexOf(str1, startIndex, StringComparison.Ordinal);
      return num != -1 ? this.CreateResult((object) (num + 1), DataType.Integer) : throw new ExcelErrorValueException(ExcelErrorValue.Create(eErrorType.Value));
    }
  }
}

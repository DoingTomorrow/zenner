// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup.Index
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Exceptions;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup
{
  public class Index : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 2);
      FunctionArgument functionArgument1 = arguments.ElementAt<FunctionArgument>(0);
      IEnumerable<FunctionArgument> source = functionArgument1.Value as IEnumerable<FunctionArgument>;
      CompileResultFactory compileResultFactory = new CompileResultFactory();
      if (source != null)
      {
        int num = this.ArgToInt(arguments, 1);
        FunctionArgument functionArgument2 = num <= source.Count<FunctionArgument>() ? source.ElementAt<FunctionArgument>(num - 1) : throw new ExcelErrorValueException(eErrorType.Ref);
        return compileResultFactory.Create(functionArgument2.Value);
      }
      if (!functionArgument1.IsExcelRange)
        throw new NotImplementedException();
      int num1 = this.ArgToInt(arguments, 1);
      int num2 = arguments.Count<FunctionArgument>() > 2 ? this.ArgToInt(arguments, 2) : 1;
      ExcelDataProvider.IRangeInfo valueAsRangeInfo = functionArgument1.ValueAsRangeInfo;
      if (num1 > valueAsRangeInfo.Address._toRow - valueAsRangeInfo.Address._fromRow + 1 || num2 > valueAsRangeInfo.Address._toCol - valueAsRangeInfo.Address._fromCol + 1)
        this.ThrowExcelErrorValueException(eErrorType.Ref);
      object offset = valueAsRangeInfo.GetOffset(num1 - 1, num2 - 1);
      return compileResultFactory.Create(offset);
    }
  }
}

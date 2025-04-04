// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers.DefaultCompiler
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Excel;
using OfficeOpenXml.FormulaParsing.Excel.Functions;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers
{
  public class DefaultCompiler(ExcelFunction function) : FunctionCompiler(function)
  {
    public override CompileResult Compile(IEnumerable<Expression> children, ParsingContext context)
    {
      List<FunctionArgument> functionArgumentList = new List<FunctionArgument>();
      this.Function.BeforeInvoke(context);
      foreach (Expression child in children)
      {
        CompileResult compileResult = child.Compile();
        if (compileResult.IsResultOfSubtotal)
        {
          FunctionArgument functionArgument = new FunctionArgument(compileResult.Result);
          functionArgument.SetExcelStateFlag(ExcelCellState.IsResultOfSubtotal);
          functionArgumentList.Add(functionArgument);
        }
        else
          this.BuildFunctionArguments(compileResult.Result, functionArgumentList);
      }
      return this.Function.Execute((IEnumerable<FunctionArgument>) functionArgumentList, context);
    }
  }
}

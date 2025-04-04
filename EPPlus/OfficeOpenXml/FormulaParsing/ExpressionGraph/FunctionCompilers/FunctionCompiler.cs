// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers.FunctionCompiler
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Excel.Functions;
using OfficeOpenXml.FormulaParsing.Utilities;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers
{
  public abstract class FunctionCompiler
  {
    protected ExcelFunction Function { get; private set; }

    public FunctionCompiler(ExcelFunction function)
    {
      Require.That<ExcelFunction>(function).Named(nameof (function)).IsNotNull<ExcelFunction>();
      this.Function = function;
    }

    protected void BuildFunctionArguments(object result, List<FunctionArgument> args)
    {
      if (result is IEnumerable<object> && !(result is ExcelDataProvider.IRangeInfo))
      {
        List<FunctionArgument> functionArgumentList = new List<FunctionArgument>();
        foreach (object result1 in result as IEnumerable<object>)
          this.BuildFunctionArguments(result1, functionArgumentList);
        args.Add(new FunctionArgument((object) functionArgumentList));
      }
      else
        args.Add(new FunctionArgument(result));
    }

    public abstract CompileResult Compile(IEnumerable<Expression> children, ParsingContext context);
  }
}

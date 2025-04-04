// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers.ErrorHandlingFunctionCompiler
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Excel.Functions;
using OfficeOpenXml.FormulaParsing.Exceptions;
using System;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers
{
  public class ErrorHandlingFunctionCompiler(ExcelFunction function) : FunctionCompiler(function)
  {
    public override CompileResult Compile(IEnumerable<Expression> children, ParsingContext context)
    {
      List<FunctionArgument> functionArgumentList = new List<FunctionArgument>();
      this.Function.BeforeInvoke(context);
      foreach (Expression child in children)
      {
        try
        {
          this.BuildFunctionArguments(child.Compile()?.Result, functionArgumentList);
        }
        catch (ExcelErrorValueException ex)
        {
          return ((ErrorHandlingFunction) this.Function).HandleError(ex.ErrorValue.ToString());
        }
        catch (Exception ex)
        {
          return ((ErrorHandlingFunction) this.Function).HandleError("#VALUE!");
        }
      }
      return this.Function.Execute((IEnumerable<FunctionArgument>) functionArgumentList, context);
    }
  }
}

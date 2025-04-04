// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers.IfFunctionCompiler
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Excel.Functions;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Exceptions;
using OfficeOpenXml.FormulaParsing.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers
{
  public class IfFunctionCompiler : FunctionCompiler
  {
    public IfFunctionCompiler(ExcelFunction function)
      : base(function)
    {
      Require.That<ExcelFunction>(function).Named(nameof (function)).IsNotNull<ExcelFunction>();
      if (!(function is If))
        throw new ArgumentException("function must be of type If");
    }

    public override CompileResult Compile(IEnumerable<Expression> children, ParsingContext context)
    {
      if (children.Count<Expression>() < 3)
        throw new ExcelErrorValueException(eErrorType.Value);
      List<FunctionArgument> arguments = new List<FunctionArgument>();
      this.Function.BeforeInvoke(context);
      bool result1 = (bool) children.ElementAt<Expression>(0).Compile().Result;
      arguments.Add(new FunctionArgument((object) result1));
      if (result1)
      {
        object result2 = children.ElementAt<Expression>(1).Compile().Result;
        arguments.Add(new FunctionArgument(result2));
        arguments.Add(new FunctionArgument((object) null));
      }
      else
      {
        object result3 = children.ElementAt<Expression>(2).Compile().Result;
        arguments.Add(new FunctionArgument((object) null));
        arguments.Add(new FunctionArgument(result3));
      }
      return this.Function.Execute((IEnumerable<FunctionArgument>) arguments, context);
    }
  }
}

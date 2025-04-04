// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.ExpressionConverter
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Exceptions;
using System;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph
{
  public class ExpressionConverter : IExpressionConverter
  {
    private static IExpressionConverter _instance;

    public StringExpression ToStringExpression(Expression expression)
    {
      StringExpression stringExpression = new StringExpression(expression.Compile().Result.ToString());
      stringExpression.Operator = expression.Operator;
      return stringExpression;
    }

    public Expression FromCompileResult(CompileResult compileResult)
    {
      switch (compileResult.DataType)
      {
        case DataType.Integer:
          return !(compileResult.Result is string) ? (Expression) new IntegerExpression(Convert.ToDouble(compileResult.Result)) : (Expression) new IntegerExpression(compileResult.Result.ToString());
        case DataType.Decimal:
          return !(compileResult.Result is string) ? (Expression) new DecimalExpression((double) compileResult.Result) : (Expression) new DecimalExpression(compileResult.Result.ToString());
        case DataType.String:
          return (Expression) new StringExpression(compileResult.Result.ToString());
        case DataType.Boolean:
          return !(compileResult.Result is string) ? (Expression) new BooleanExpression((bool) compileResult.Result) : (Expression) new BooleanExpression(compileResult.Result.ToString());
        case DataType.ExcelError:
          throw new ExcelErrorValueException((ExcelErrorValue) compileResult.Result);
        case DataType.Empty:
          return (Expression) new IntegerExpression(0.0);
        default:
          return (Expression) null;
      }
    }

    public static IExpressionConverter Instance
    {
      get
      {
        if (ExpressionConverter._instance == null)
          ExpressionConverter._instance = (IExpressionConverter) new ExpressionConverter();
        return ExpressionConverter._instance;
      }
    }
  }
}

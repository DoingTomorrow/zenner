// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Math.AverageIf
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExcelUtilities;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using OfficeOpenXml.FormulaParsing.Utilities;
using OfficeOpenXml.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Math
{
  public class AverageIf : HiddenValuesHandlingFunction
  {
    private readonly NumericExpressionEvaluator _numericExpressionEvaluator;
    private readonly WildCardValueMatcher _wildCardValueMatcher;

    public AverageIf()
      : this(new NumericExpressionEvaluator(), new WildCardValueMatcher())
    {
    }

    public AverageIf(
      NumericExpressionEvaluator evaluator,
      WildCardValueMatcher wildCardValueMatcher)
    {
      OfficeOpenXml.FormulaParsing.Utilities.Require.That<NumericExpressionEvaluator>(evaluator).Named(nameof (evaluator)).IsNotNull<NumericExpressionEvaluator>();
      OfficeOpenXml.FormulaParsing.Utilities.Require.That<NumericExpressionEvaluator>(evaluator).Named(nameof (wildCardValueMatcher)).IsNotNull<NumericExpressionEvaluator>();
      this._numericExpressionEvaluator = evaluator;
      this._wildCardValueMatcher = wildCardValueMatcher;
    }

    private bool Evaluate(object obj, string expression)
    {
      double? nullable = new double?();
      if (this.IsNumeric(obj))
        nullable = new double?(ConvertUtil.GetValueDouble(obj));
      return nullable.HasValue ? this._numericExpressionEvaluator.Evaluate((object) nullable.Value, expression) : this._wildCardValueMatcher.IsMatch((object) expression, (object) obj.ToString()) == 0;
    }

    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 2);
      FunctionArgument functionArgument1 = arguments.ElementAt<FunctionArgument>(0);
      if (!(functionArgument1.Value is IEnumerable<FunctionArgument> functionArguments) && functionArgument1.IsExcelRange)
        functionArguments = (IEnumerable<FunctionArgument>) new List<FunctionArgument>()
        {
          functionArgument1
        };
      object criteria = arguments.ElementAt<FunctionArgument>(1).Value;
      this.ThrowExcelErrorValueExceptionIf((Func<bool>) (() => criteria == null || criteria.ToString().Length > (int) byte.MaxValue), eErrorType.Value);
      double result;
      if (arguments.Count<FunctionArgument>() > 2)
      {
        FunctionArgument functionArgument2 = arguments.ElementAt<FunctionArgument>(2);
        if (!(functionArgument2.Value is IEnumerable<FunctionArgument> sumRange) && functionArgument2.IsExcelRange)
          sumRange = (IEnumerable<FunctionArgument>) new List<FunctionArgument>()
          {
            functionArgument2
          };
        result = this.CalculateWithLookupRange(functionArguments, criteria.ToString(), sumRange, context);
      }
      else
        result = this.CalculateSingleRange(functionArguments, criteria.ToString(), context);
      return this.CreateResult((object) result, DataType.Decimal);
    }

    private double CalculateWithLookupRange(
      IEnumerable<FunctionArgument> range,
      string criteria,
      IEnumerable<FunctionArgument> sumRange,
      ParsingContext context)
    {
      double num1 = 0.0;
      int num2 = 0;
      IEnumerable<object> objectEnumerable = this.ArgsToObjectEnumerable(false, range, context);
      IEnumerable<double> doubleEnumerable = this.ArgsToDoubleEnumerable(sumRange, context);
      for (int index = 0; index < objectEnumerable.Count<object>(); ++index)
      {
        double num3 = doubleEnumerable.ElementAt<double>(index);
        if (this.Evaluate(objectEnumerable.ElementAt<object>(index), criteria))
        {
          ++num2;
          num1 += num3;
        }
      }
      return num1 / (double) num2;
    }

    private double CalculateSingleRange(
      IEnumerable<FunctionArgument> args,
      string expression,
      ParsingContext context)
    {
      double num1 = 0.0;
      int num2 = 0;
      IEnumerable<double> doubleEnumerable = this.ArgsToDoubleEnumerable(args, context);
      if (!(doubleEnumerable is double[] numArray))
        numArray = doubleEnumerable.ToArray<double>();
      foreach (double num3 in numArray)
      {
        if (this.Evaluate((object) num3, expression))
        {
          num1 += num3;
          ++num2;
        }
      }
      return num1 / (double) num2;
    }

    private double Calculate(FunctionArgument arg, string expression)
    {
      double num = 0.0;
      if (this.ShouldIgnore(arg) || !this._numericExpressionEvaluator.Evaluate(arg.Value, expression))
        return num;
      if (this.IsNumeric(arg.Value))
        num += ConvertUtil.GetValueDouble(arg.Value);
      else if (arg.Value is IEnumerable<FunctionArgument>)
      {
        foreach (FunctionArgument functionArgument in (IEnumerable<FunctionArgument>) arg.Value)
          num += this.Calculate(functionArgument, expression);
      }
      return num;
    }
  }
}

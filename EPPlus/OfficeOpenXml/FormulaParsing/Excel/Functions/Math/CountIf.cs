// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Math.CountIf
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExcelUtilities;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using OfficeOpenXml.FormulaParsing.Utilities;
using OfficeOpenXml.Utils;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Math
{
  public class CountIf : ExcelFunction
  {
    private readonly NumericExpressionEvaluator _numericExpressionEvaluator;
    private readonly WildCardValueMatcher _wildCardValueMatcher;

    public CountIf()
      : this(new NumericExpressionEvaluator(), new WildCardValueMatcher())
    {
    }

    public CountIf(NumericExpressionEvaluator evaluator, WildCardValueMatcher wildCardValueMatcher)
    {
      OfficeOpenXml.FormulaParsing.Utilities.Require.That<NumericExpressionEvaluator>(evaluator).Named(nameof (evaluator)).IsNotNull<NumericExpressionEvaluator>();
      OfficeOpenXml.FormulaParsing.Utilities.Require.That<WildCardValueMatcher>(wildCardValueMatcher).Named(nameof (wildCardValueMatcher)).IsNotNull<WildCardValueMatcher>();
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
      if (!(arguments is FunctionArgument[] functionArgumentArray1))
        functionArgumentArray1 = arguments.ToArray<FunctionArgument>();
      FunctionArgument[] functionArgumentArray2 = functionArgumentArray1;
      this.ValidateArguments((IEnumerable<FunctionArgument>) functionArgumentArray2, 2);
      FunctionArgument functionArgument1 = ((IEnumerable<FunctionArgument>) functionArgumentArray2).ElementAt<FunctionArgument>(0);
      string expression = this.ArgToString((IEnumerable<FunctionArgument>) functionArgumentArray2, 1);
      double result = 0.0;
      if (functionArgument1.IsExcelRange)
      {
        foreach (ExcelDataProvider.ICellInfo cellInfo in (IEnumerable<ExcelDataProvider.ICellInfo>) functionArgument1.ValueAsRangeInfo)
        {
          if (this.Evaluate(cellInfo.Value, expression))
            ++result;
        }
      }
      else if (functionArgument1.Value is IEnumerable<FunctionArgument>)
      {
        foreach (FunctionArgument functionArgument2 in (IEnumerable<FunctionArgument>) functionArgument1.Value)
        {
          if (this.Evaluate(functionArgument2.Value, expression))
            ++result;
        }
      }
      else if (this.Evaluate(functionArgument1.Value, expression))
        ++result;
      return this.CreateResult((object) result, DataType.Integer);
    }
  }
}

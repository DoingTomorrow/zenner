// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExcelUtilities.NumericExpressionEvaluator
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Excel.Operators;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Text.RegularExpressions;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExcelUtilities
{
  public class NumericExpressionEvaluator
  {
    private ValueMatcher _valueMatcher;
    private CompileResultFactory _compileResultFactory;

    public NumericExpressionEvaluator()
      : this(new ValueMatcher(), new CompileResultFactory())
    {
    }

    public NumericExpressionEvaluator(
      ValueMatcher valueMatcher,
      CompileResultFactory compileResultFactory)
    {
      this._valueMatcher = valueMatcher;
      this._compileResultFactory = compileResultFactory;
    }

    private string GetNonNumericStartChars(string expression)
    {
      if (!string.IsNullOrEmpty(expression))
      {
        if (Regex.IsMatch(expression, "^([^\\d]{2})"))
          return expression.Substring(0, 2);
        if (Regex.IsMatch(expression, "^([^\\d]{1})"))
          return expression.Substring(0, 1);
      }
      return (string) null;
    }

    public double? OperandAsDouble(object op)
    {
      if (op is double || op is int)
        return new double?(Convert.ToDouble(op));
      double result;
      return op != null && double.TryParse(op.ToString(), out result) ? new double?(result) : new double?();
    }

    public bool Evaluate(object left, string expression)
    {
      string numericStartChars = this.GetNonNumericStartChars(expression);
      double? nullable = this.OperandAsDouble(left);
      IOperator @operator;
      double result;
      if (string.IsNullOrEmpty(numericStartChars) || !nullable.HasValue || !OperatorsDict.Instance.TryGetValue(numericStartChars, out @operator) || !double.TryParse(expression.Replace(numericStartChars, string.Empty), out result))
        return this._valueMatcher.IsMatch(left, (object) expression) == 0;
      CompileResult left1 = this._compileResultFactory.Create((object) nullable);
      CompileResult right = this._compileResultFactory.Create((object) result);
      CompileResult compileResult = @operator.Apply(left1, right);
      return compileResult.DataType == DataType.Boolean ? (bool) compileResult.Result : throw new ArgumentException("Illegal operator in expression");
    }
  }
}

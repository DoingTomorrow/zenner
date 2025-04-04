// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Operators.Operator
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Exceptions;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using OfficeOpenXml.Utils;
using System;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Operators
{
  public class Operator : IOperator
  {
    private const int PrecedencePercent = 2;
    private const int PrecedenceExp = 4;
    private const int PrecedenceMultiplyDevide = 6;
    private const int PrecedenceIntegerDivision = 8;
    private const int PrecedenceModulus = 10;
    private const int PrecedenceAddSubtract = 12;
    private const int PrecedenceConcat = 15;
    private const int PrecedenceComparison = 25;
    private readonly Func<CompileResult, CompileResult, CompileResult> _implementation;
    private readonly int _precedence;
    private readonly OfficeOpenXml.FormulaParsing.Excel.Operators.Operators _operator;
    private static IOperator _percent;

    private Operator()
    {
    }

    private Operator(
      OfficeOpenXml.FormulaParsing.Excel.Operators.Operators @operator,
      int precedence,
      Func<CompileResult, CompileResult, CompileResult> implementation)
    {
      this._implementation = implementation;
      this._precedence = precedence;
      this._operator = @operator;
    }

    int IOperator.Precedence => this._precedence;

    OfficeOpenXml.FormulaParsing.Excel.Operators.Operators IOperator.Operator => this._operator;

    public CompileResult Apply(CompileResult left, CompileResult right)
    {
      if (left.Result is ExcelErrorValue)
        throw new ExcelErrorValueException((ExcelErrorValue) left.Result);
      if (right.Result is ExcelErrorValue)
        throw new ExcelErrorValueException((ExcelErrorValue) right.Result);
      return this._implementation(left, right);
    }

    public override string ToString() => "Operator: " + (object) this._operator;

    public static IOperator Plus
    {
      get
      {
        return (IOperator) new Operator(OfficeOpenXml.FormulaParsing.Excel.Operators.Operators.Plus, 12, (Func<CompileResult, CompileResult, CompileResult>) ((l, r) =>
        {
          l = l == null || l.Result == null ? new CompileResult((object) 0, DataType.Integer) : l;
          r = r == null || r.Result == null ? new CompileResult((object) 0, DataType.Integer) : r;
          Operator.CheckForErrors(l, r);
          if (l.DataType == DataType.Integer && r.DataType == DataType.Integer)
            return new CompileResult((object) (l.ResultNumeric + r.ResultNumeric), DataType.Integer);
          if ((l.IsNumeric || l.IsNumericString || l.Result is ExcelDataProvider.IRangeInfo) && (r.IsNumeric || r.IsNumericString || r.Result is ExcelDataProvider.IRangeInfo))
            return new CompileResult((object) (l.ResultNumeric + r.ResultNumeric), DataType.Decimal);
          throw new ExcelErrorValueException(eErrorType.Value);
        }));
      }
    }

    public static IOperator Minus
    {
      get
      {
        return (IOperator) new Operator(OfficeOpenXml.FormulaParsing.Excel.Operators.Operators.Minus, 12, (Func<CompileResult, CompileResult, CompileResult>) ((l, r) =>
        {
          l = l == null || l.Result == null ? new CompileResult((object) 0, DataType.Integer) : l;
          r = r == null || r.Result == null ? new CompileResult((object) 0, DataType.Integer) : r;
          if (l.DataType == DataType.Integer && r.DataType == DataType.Integer)
            return new CompileResult((object) (l.ResultNumeric - r.ResultNumeric), DataType.Integer);
          if ((l.IsNumeric || l.IsNumericString || l.Result is ExcelDataProvider.IRangeInfo) && (r.IsNumeric || r.IsNumericString || r.Result is ExcelDataProvider.IRangeInfo))
            return new CompileResult((object) (l.ResultNumeric - r.ResultNumeric), DataType.Decimal);
          throw new ExcelErrorValueException(eErrorType.Value);
        }));
      }
    }

    public static IOperator Multiply
    {
      get
      {
        return (IOperator) new Operator(OfficeOpenXml.FormulaParsing.Excel.Operators.Operators.Multiply, 6, (Func<CompileResult, CompileResult, CompileResult>) ((l, r) =>
        {
          l = l ?? new CompileResult((object) 0, DataType.Integer);
          r = r ?? new CompileResult((object) 0, DataType.Integer);
          if (l.DataType == DataType.Integer && r.DataType == DataType.Integer)
            return new CompileResult((object) (l.ResultNumeric * r.ResultNumeric), DataType.Integer);
          if ((l.IsNumeric || l.IsNumericString || l.Result is ExcelDataProvider.IRangeInfo) && (r.IsNumeric || r.IsNumericString || r.Result is ExcelDataProvider.IRangeInfo))
            return new CompileResult((object) (l.ResultNumeric * r.ResultNumeric), DataType.Decimal);
          throw new ExcelErrorValueException(eErrorType.Value);
        }));
      }
    }

    public static IOperator Divide
    {
      get
      {
        return (IOperator) new Operator(OfficeOpenXml.FormulaParsing.Excel.Operators.Operators.Divide, 6, (Func<CompileResult, CompileResult, CompileResult>) ((l, r) =>
        {
          if (!l.IsNumeric && !l.IsNumericString && !(l.Result is ExcelDataProvider.IRangeInfo) || !r.IsNumeric && !r.IsNumericString && !(r.Result is ExcelDataProvider.IRangeInfo))
            throw new ExcelErrorValueException(eErrorType.Value);
          double resultNumeric1 = l.ResultNumeric;
          double resultNumeric2 = r.ResultNumeric;
          if (Math.Abs(resultNumeric2 - 0.0) < double.Epsilon)
            throw new ExcelErrorValueException(eErrorType.Div0);
          if ((l.IsNumeric || l.IsNumericString || l.Result is ExcelDataProvider.IRangeInfo) && (r.IsNumeric || r.IsNumericString || r.Result is ExcelDataProvider.IRangeInfo))
            return new CompileResult((object) (resultNumeric1 / resultNumeric2), DataType.Decimal);
          throw new ExcelErrorValueException(eErrorType.Value);
        }));
      }
    }

    public static IOperator Exp
    {
      get
      {
        return (IOperator) new Operator(OfficeOpenXml.FormulaParsing.Excel.Operators.Operators.Exponentiation, 4, (Func<CompileResult, CompileResult, CompileResult>) ((l, r) =>
        {
          if (l == null && r == null)
            throw new ExcelErrorValueException(eErrorType.Value);
          l = l ?? new CompileResult((object) 0, DataType.Integer);
          r = r ?? new CompileResult((object) 0, DataType.Integer);
          return (l.IsNumeric || l.Result is ExcelDataProvider.IRangeInfo) && (r.IsNumeric || r.Result is ExcelDataProvider.IRangeInfo) ? new CompileResult((object) Math.Pow(l.ResultNumeric, r.ResultNumeric), DataType.Decimal) : new CompileResult((object) 0.0, DataType.Decimal);
        }));
      }
    }

    public static IOperator Concat
    {
      get
      {
        return (IOperator) new Operator(OfficeOpenXml.FormulaParsing.Excel.Operators.Operators.Concat, 15, (Func<CompileResult, CompileResult, CompileResult>) ((l, r) =>
        {
          l = l ?? new CompileResult((object) string.Empty, DataType.String);
          r = r ?? new CompileResult((object) string.Empty, DataType.String);
          return new CompileResult((object) ((l.Result != null ? l.ResultValue.ToString() : string.Empty) + (r.Result != null ? r.ResultValue.ToString() : string.Empty)), DataType.String);
        }));
      }
    }

    public static IOperator GreaterThan
    {
      get
      {
        return (IOperator) new Operator(OfficeOpenXml.FormulaParsing.Excel.Operators.Operators.GreaterThan, 25, (Func<CompileResult, CompileResult, CompileResult>) ((l, r) => new CompileResult((object) (Operator.Compare(l, r) > 0), DataType.Boolean)));
      }
    }

    public static IOperator Eq
    {
      get
      {
        return (IOperator) new Operator(OfficeOpenXml.FormulaParsing.Excel.Operators.Operators.Equals, 25, (Func<CompileResult, CompileResult, CompileResult>) ((l, r) => new CompileResult((object) (Operator.Compare(l, r) == 0), DataType.Boolean)));
      }
    }

    public static IOperator NotEqualsTo
    {
      get
      {
        return (IOperator) new Operator(OfficeOpenXml.FormulaParsing.Excel.Operators.Operators.NotEqualTo, 25, (Func<CompileResult, CompileResult, CompileResult>) ((l, r) => new CompileResult((object) (Operator.Compare(l, r) != 0), DataType.Boolean)));
      }
    }

    public static IOperator GreaterThanOrEqual
    {
      get
      {
        return (IOperator) new Operator(OfficeOpenXml.FormulaParsing.Excel.Operators.Operators.GreaterThanOrEqual, 25, (Func<CompileResult, CompileResult, CompileResult>) ((l, r) => new CompileResult((object) (Operator.Compare(l, r) >= 0), DataType.Boolean)));
      }
    }

    public static IOperator LessThan
    {
      get
      {
        return (IOperator) new Operator(OfficeOpenXml.FormulaParsing.Excel.Operators.Operators.LessThan, 25, (Func<CompileResult, CompileResult, CompileResult>) ((l, r) => new CompileResult((object) (Operator.Compare(l, r) < 0), DataType.Boolean)));
      }
    }

    public static IOperator LessThanOrEqual
    {
      get
      {
        return (IOperator) new Operator(OfficeOpenXml.FormulaParsing.Excel.Operators.Operators.LessThanOrEqual, 25, (Func<CompileResult, CompileResult, CompileResult>) ((l, r) => new CompileResult((object) (Operator.Compare(l, r) <= 0), DataType.Boolean)));
      }
    }

    public static IOperator Percent
    {
      get
      {
        if (Operator._percent == null)
          Operator._percent = (IOperator) new Operator(OfficeOpenXml.FormulaParsing.Excel.Operators.Operators.Percent, 2, (Func<CompileResult, CompileResult, CompileResult>) ((l, r) =>
          {
            l = l ?? new CompileResult((object) 0, DataType.Integer);
            r = r ?? new CompileResult((object) 0, DataType.Integer);
            if (l.DataType == DataType.Integer && r.DataType == DataType.Integer)
              return new CompileResult((object) (l.ResultNumeric * r.ResultNumeric), DataType.Integer);
            if ((l.IsNumeric || l.Result is ExcelDataProvider.IRangeInfo) && (r.IsNumeric || r.Result is ExcelDataProvider.IRangeInfo))
              return new CompileResult((object) (l.ResultNumeric * r.ResultNumeric), DataType.Decimal);
            throw new ExcelErrorValueException(eErrorType.Value);
          }));
        return Operator._percent;
      }
    }

    private static object GetObjFromOther(CompileResult obj, CompileResult other)
    {
      if (obj.Result != null)
        return obj.ResultValue;
      return other.DataType == DataType.String ? (object) string.Empty : (object) 0.0;
    }

    private static int Compare(CompileResult l, CompileResult r)
    {
      Operator.CheckForErrors(l, r);
      object objFromOther1 = Operator.GetObjFromOther(l, r);
      object objFromOther2 = Operator.GetObjFromOther(r, l);
      if (!ConvertUtil.IsNumeric(objFromOther1) || !ConvertUtil.IsNumeric(objFromOther2))
        return Operator.CompareString(l.Result, r.Result);
      double valueDouble1 = ConvertUtil.GetValueDouble(objFromOther1);
      double valueDouble2 = ConvertUtil.GetValueDouble(objFromOther2);
      return Math.Abs(valueDouble1 - valueDouble2) < double.Epsilon ? 0 : valueDouble1.CompareTo(valueDouble2);
    }

    private static int CompareString(object l, object r)
    {
      return string.Compare((l ?? (object) "").ToString(), (r ?? (object) "").ToString(), StringComparison.Ordinal);
    }

    private static void CheckForErrors(CompileResult l, CompileResult r)
    {
      if (l.DataType == DataType.ExcelError)
        throw new ExcelErrorValueException((ExcelErrorValue) l.Result);
      if (r.DataType == DataType.ExcelError)
        throw new ExcelErrorValueException((ExcelErrorValue) r.Result);
    }
  }
}

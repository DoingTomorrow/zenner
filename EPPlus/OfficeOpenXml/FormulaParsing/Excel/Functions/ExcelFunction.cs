// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.ExcelFunction
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Exceptions;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using OfficeOpenXml.FormulaParsing.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions
{
  public abstract class ExcelFunction
  {
    private readonly ArgumentCollectionUtil _argumentCollectionUtil;
    private readonly ArgumentParsers _argumentParsers;
    private readonly CompileResultValidators _compileResultValidators;

    public ExcelFunction()
      : this(new ArgumentCollectionUtil(), new ArgumentParsers(), new CompileResultValidators())
    {
    }

    public ExcelFunction(
      ArgumentCollectionUtil argumentCollectionUtil,
      ArgumentParsers argumentParsers,
      CompileResultValidators compileResultValidators)
    {
      this._argumentCollectionUtil = argumentCollectionUtil;
      this._argumentParsers = argumentParsers;
      this._compileResultValidators = compileResultValidators;
    }

    public abstract CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context);

    public virtual void BeforeInvoke(ParsingContext context)
    {
    }

    public virtual bool IsLookupFuction => false;

    public virtual bool IsErrorHandlingFunction => false;

    public bool SkipArgumentEvaluation { get; set; }

    protected object GetFirstValue(IEnumerable<FunctionArgument> val)
    {
      FunctionArgument functionArgument = val.FirstOrDefault<FunctionArgument>();
      if (functionArgument.Value is ExcelDataProvider.IRangeInfo)
      {
        ExcelDataProvider.IRangeInfo valueAsRangeInfo = functionArgument.ValueAsRangeInfo;
        return valueAsRangeInfo.GetValue(valueAsRangeInfo.Address._fromRow, valueAsRangeInfo.Address._fromCol);
      }
      return functionArgument?.Value;
    }

    protected void ValidateArguments(
      IEnumerable<FunctionArgument> arguments,
      int minLength,
      eErrorType errorTypeToThrow)
    {
      Require.That<IEnumerable<FunctionArgument>>(arguments).Named(nameof (arguments)).IsNotNull<IEnumerable<FunctionArgument>>();
      this.ThrowExcelErrorValueExceptionIf((Func<bool>) (() =>
      {
        int num = 0;
        if (arguments.Any<FunctionArgument>())
        {
          foreach (FunctionArgument functionArgument in arguments)
          {
            ++num;
            if (num >= minLength)
              return false;
            if (functionArgument.IsExcelRange)
            {
              num += functionArgument.ValueAsRangeInfo.GetNCells();
              if (num >= minLength)
                return false;
            }
          }
        }
        return true;
      }), errorTypeToThrow);
    }

    protected void ValidateArguments(IEnumerable<FunctionArgument> arguments, int minLength)
    {
      Require.That<IEnumerable<FunctionArgument>>(arguments).Named(nameof (arguments)).IsNotNull<IEnumerable<FunctionArgument>>();
      this.ThrowArgumentExceptionIf((Func<bool>) (() =>
      {
        int num = 0;
        if (arguments.Any<FunctionArgument>())
        {
          foreach (FunctionArgument functionArgument in arguments)
          {
            ++num;
            if (num >= minLength)
              return false;
            if (functionArgument.IsExcelRange)
            {
              num += functionArgument.ValueAsRangeInfo.GetNCells();
              if (num >= minLength)
                return false;
            }
          }
        }
        return true;
      }), "Expecting at least {0} arguments", (object) minLength.ToString());
    }

    protected int ArgToInt(IEnumerable<FunctionArgument> arguments, int index)
    {
      object valueFirst = arguments.ElementAt<FunctionArgument>(index).ValueFirst;
      return (int) this._argumentParsers.GetParser(DataType.Integer).Parse(valueFirst);
    }

    protected string ArgToString(IEnumerable<FunctionArgument> arguments, int index)
    {
      object valueFirst = arguments.ElementAt<FunctionArgument>(index).ValueFirst;
      return valueFirst == null ? string.Empty : valueFirst.ToString();
    }

    protected double ArgToDecimal(object obj)
    {
      return (double) this._argumentParsers.GetParser(DataType.Decimal).Parse(obj);
    }

    protected double ArgToDecimal(IEnumerable<FunctionArgument> arguments, int index)
    {
      return this.ArgToDecimal(arguments.ElementAt<FunctionArgument>(index).Value);
    }

    protected bool IsNumericString(object value)
    {
      return value != null && !string.IsNullOrEmpty(value.ToString()) && Regex.IsMatch(value.ToString(), "^[\\d]+(\\,[\\d])?");
    }

    protected bool ArgToBool(IEnumerable<FunctionArgument> arguments, int index)
    {
      object obj = arguments.ElementAt<FunctionArgument>(index).Value ?? (object) string.Empty;
      return (bool) this._argumentParsers.GetParser(DataType.Boolean).Parse(obj);
    }

    protected void ThrowArgumentExceptionIf(Func<bool> condition, string message)
    {
      if (condition())
        throw new ArgumentException(message);
    }

    protected void ThrowArgumentExceptionIf(
      Func<bool> condition,
      string message,
      params object[] formats)
    {
      message = string.Format(message, formats);
      this.ThrowArgumentExceptionIf(condition, message);
    }

    protected void ThrowExcelErrorValueException(eErrorType errorType)
    {
      throw new ExcelErrorValueException("An excel function error occurred", ExcelErrorValue.Create(errorType));
    }

    protected void ThrowExcelErrorValueExceptionIf(Func<bool> condition, eErrorType errorType)
    {
      if (condition())
        throw new ExcelErrorValueException("An excel function error occurred", ExcelErrorValue.Create(errorType));
    }

    protected bool IsNumeric(object val)
    {
      if (val == null)
        return false;
      if (!val.GetType().IsPrimitive)
      {
        switch (val)
        {
          case double _:
          case Decimal _:
          case DateTime _:
            break;
          default:
            return val is TimeSpan;
        }
      }
      return true;
    }

    protected bool AreEqual(double d1, double d2) => Math.Abs(d1 - d2) < double.Epsilon;

    protected virtual IEnumerable<double> ArgsToDoubleEnumerable(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      return this.ArgsToDoubleEnumerable(false, arguments, context);
    }

    protected virtual IEnumerable<double> ArgsToDoubleEnumerable(
      bool ignoreHiddenCells,
      bool ignoreErrors,
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      return this._argumentCollectionUtil.ArgsToDoubleEnumerable(ignoreHiddenCells, ignoreErrors, arguments, context);
    }

    protected virtual IEnumerable<double> ArgsToDoubleEnumerable(
      bool ignoreHiddenCells,
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      return this.ArgsToDoubleEnumerable(ignoreHiddenCells, true, arguments, context);
    }

    protected virtual IEnumerable<object> ArgsToObjectEnumerable(
      bool ignoreHiddenCells,
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      return this._argumentCollectionUtil.ArgsToObjectEnumerable(ignoreHiddenCells, arguments, context);
    }

    protected CompileResult CreateResult(object result, DataType dataType)
    {
      this._compileResultValidators.GetValidator(dataType).Validate(result);
      return new CompileResult(result, dataType);
    }

    protected virtual double CalculateCollection(
      IEnumerable<FunctionArgument> collection,
      double result,
      Func<FunctionArgument, double, double> action)
    {
      return this._argumentCollectionUtil.CalculateCollection(collection, result, action);
    }

    protected void CheckForAndHandleExcelError(FunctionArgument arg)
    {
      if (arg.ValueIsExcelError)
        throw new ExcelErrorValueException(arg.ValueAsExcelErrorValue);
    }

    protected void CheckForAndHandleExcelError(ExcelDataProvider.ICellInfo cell)
    {
      if (cell.IsExcelError)
        throw new ExcelErrorValueException(ExcelErrorValue.Parse(cell.Value.ToString()));
    }
  }
}

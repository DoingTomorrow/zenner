// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Math.Product
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Math
{
  public class Product : HiddenValuesHandlingFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 1);
      double num1 = 0.0;
      int count = 0;
      while (this.AreEqual(num1, 0.0) && count < arguments.Count<FunctionArgument>())
        num1 = this.CalculateFirstItem(arguments, count++, context);
      return this.CreateResult((object) this.CalculateCollection(arguments.Skip<FunctionArgument>(count), num1, (Func<FunctionArgument, double, double>) ((arg, current) =>
      {
        if (this.ShouldIgnore(arg))
          return current;
        if (arg.ValueIsExcelError)
          this.ThrowExcelErrorValueException(arg.ValueAsExcelErrorValue.Type);
        if (arg.IsExcelRange)
        {
          foreach (ExcelDataProvider.ICellInfo c in (IEnumerable<ExcelDataProvider.ICellInfo>) arg.ValueAsRangeInfo)
          {
            if (this.ShouldIgnore(c, context))
              return current;
            current *= c.ValueDouble;
          }
          return current;
        }
        object val = arg.Value;
        if (val != null && this.IsNumeric(val))
        {
          double num2 = Convert.ToDouble(val);
          current *= num2;
        }
        return current;
      })), DataType.Decimal);
    }

    private double CalculateFirstItem(
      IEnumerable<FunctionArgument> arguments,
      int index,
      ParsingContext context)
    {
      IEnumerable<double> doubleEnumerable = this.ArgsToDoubleEnumerable(false, false, (IEnumerable<FunctionArgument>) new List<FunctionArgument>()
      {
        arguments.ElementAt<FunctionArgument>(index)
      }, context);
      double firstItem = 0.0;
      foreach (double num in doubleEnumerable)
      {
        if (firstItem == 0.0 && num > 0.0)
          firstItem = num;
        else
          firstItem *= num;
      }
      return firstItem;
    }
  }
}

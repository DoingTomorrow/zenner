// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.DoubleEnumerableArgConverter
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Exceptions;
using OfficeOpenXml.Utils;
using System;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions
{
  public class DoubleEnumerableArgConverter : CollectionFlattener<double>
  {
    public virtual IEnumerable<double> ConvertArgs(
      bool ignoreHidden,
      bool ignoreErrors,
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      return this.FuncArgsToFlatEnumerable(arguments, (Action<FunctionArgument, IList<double>>) ((arg, argList) =>
      {
        if (arg.IsExcelRange)
        {
          foreach (ExcelDataProvider.ICellInfo c in (IEnumerable<ExcelDataProvider.ICellInfo>) arg.ValueAsRangeInfo)
          {
            if (!ignoreErrors && c.IsExcelError)
              throw new ExcelErrorValueException(ExcelErrorValue.Parse(c.Value.ToString()));
            if (!CellStateHelper.ShouldIgnore(ignoreHidden, c, context) && ConvertUtil.IsNumeric(c.Value))
              argList.Add(c.ValueDouble);
          }
        }
        else
        {
          if (!ignoreErrors && arg.ValueIsExcelError)
            throw new ExcelErrorValueException(arg.ValueAsExcelErrorValue);
          if (!ConvertUtil.IsNumeric(arg.Value) || CellStateHelper.ShouldIgnore(ignoreHidden, arg, context))
            return;
          argList.Add(ConvertUtil.GetValueDouble(arg.Value));
        }
      }));
    }

    public virtual IEnumerable<double> ConvertArgsIncludingOtherTypes(
      IEnumerable<FunctionArgument> arguments)
    {
      return this.FuncArgsToFlatEnumerable(arguments, (Action<FunctionArgument, IList<double>>) ((arg, argList) =>
      {
        if (arg.Value is ExcelDataProvider.IRangeInfo)
        {
          foreach (ExcelDataProvider.ICellInfo cellInfo in (IEnumerable<ExcelDataProvider.ICellInfo>) arg.Value)
            argList.Add(cellInfo.ValueDoubleLogical);
        }
        else if (arg.Value is double || arg.Value is int || arg.Value is bool)
        {
          argList.Add(Convert.ToDouble(arg.Value));
        }
        else
        {
          if (!(arg.Value is string))
            return;
          argList.Add(0.0);
        }
      }));
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.CompileResult
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph
{
  public class CompileResult
  {
    private static CompileResult _empty = new CompileResult((object) null, DataType.Empty);
    private object _result;

    public static CompileResult Empty => CompileResult._empty;

    public CompileResult(object result, DataType dataType)
    {
      this.Result = result;
      this.DataType = dataType;
    }

    public object Result { get; private set; }

    public object ResultValue
    {
      get
      {
        return !(this.Result is ExcelDataProvider.IRangeInfo result) ? this.Result : result.GetValue(result.Address._fromRow, result.Address._fromCol);
      }
    }

    public double ResultNumeric
    {
      get
      {
        if (this.IsNumeric)
          return this.Result != null ? Convert.ToDouble(this.Result) : 0.0;
        if (this.Result is DateTime)
          return ((DateTime) this.Result).ToOADate();
        if (this.Result is TimeSpan)
          return new DateTime(((TimeSpan) this.Result).Ticks).ToOADate();
        if (this.IsNumericString)
          return double.Parse(this.Result.ToString(), NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture);
        if (!(this.Result is ExcelDataProvider.IRangeInfo))
          return 0.0;
        ExcelDataProvider.ICellInfo cellInfo = ((IEnumerable<ExcelDataProvider.ICellInfo>) this.Result).FirstOrDefault<ExcelDataProvider.ICellInfo>();
        return cellInfo == null ? 0.0 : cellInfo.ValueDoubleLogical;
      }
    }

    public DataType DataType { get; private set; }

    public bool IsNumeric
    {
      get
      {
        return this.DataType == DataType.Decimal || this.DataType == DataType.Integer || this.DataType == DataType.Empty || this.DataType == DataType.Boolean;
      }
    }

    public bool IsNumericString
    {
      get => this.DataType == DataType.String && ConvertUtil.IsNumericString(this.Result);
    }

    public bool IsResultOfSubtotal { get; set; }

    public bool IsHiddenCell { get; set; }
  }
}

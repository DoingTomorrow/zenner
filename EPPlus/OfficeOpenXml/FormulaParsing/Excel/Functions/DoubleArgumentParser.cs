// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.DoubleArgumentParser
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Exceptions;
using OfficeOpenXml.FormulaParsing.Utilities;
using OfficeOpenXml.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions
{
  public class DoubleArgumentParser : ArgumentParser
  {
    public override object Parse(object obj)
    {
      OfficeOpenXml.FormulaParsing.Utilities.Require.That<object>(obj).Named("argument").IsNotNull<object>();
      if (obj is ExcelDataProvider.IRangeInfo)
      {
        ExcelDataProvider.ICellInfo cellInfo = ((IEnumerable<ExcelDataProvider.ICellInfo>) obj).FirstOrDefault<ExcelDataProvider.ICellInfo>();
        return (object) (cellInfo == null ? 0.0 : cellInfo.ValueDouble);
      }
      if (obj is double)
        return obj;
      if (obj.IsNumeric())
        return (object) ConvertUtil.GetValueDouble(obj);
      string s = obj != null ? obj.ToString() : string.Empty;
      try
      {
        return (object) double.Parse(s, (IFormatProvider) CultureInfo.InvariantCulture);
      }
      catch (Exception ex)
      {
        throw new ExcelErrorValueException(ExcelErrorValue.Create(eErrorType.Value));
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.IntArgumentParser
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Exceptions;
using OfficeOpenXml.FormulaParsing.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions
{
  public class IntArgumentParser : ArgumentParser
  {
    public override object Parse(object obj)
    {
      Require.That<object>(obj).Named("argument").IsNotNull<object>();
      if (obj is ExcelDataProvider.IRangeInfo)
      {
        ExcelDataProvider.ICellInfo cellInfo = ((IEnumerable<ExcelDataProvider.ICellInfo>) obj).FirstOrDefault<ExcelDataProvider.ICellInfo>();
        return (object) (cellInfo == null ? 0 : Convert.ToInt32(cellInfo.ValueDouble));
      }
      Type type = obj.GetType();
      if (type == typeof (int))
        return (object) (int) obj;
      if (type == typeof (double) || type == typeof (Decimal))
        return (object) Convert.ToInt32(obj);
      int result;
      if (!int.TryParse(obj.ToString(), out result))
        throw new ExcelErrorValueException(ExcelErrorValue.Create(eErrorType.Value));
      return (object) result;
    }
  }
}

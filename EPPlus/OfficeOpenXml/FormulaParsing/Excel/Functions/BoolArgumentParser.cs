// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.BoolArgumentParser
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions
{
  public class BoolArgumentParser : ArgumentParser
  {
    public override object Parse(object obj)
    {
      if (obj is ExcelDataProvider.IRangeInfo)
      {
        ExcelDataProvider.ICellInfo cellInfo = ((IEnumerable<ExcelDataProvider.ICellInfo>) obj).FirstOrDefault<ExcelDataProvider.ICellInfo>();
        obj = cellInfo == null ? (object) null : cellInfo.Value;
      }
      if (obj == null)
        return (object) false;
      if (obj is bool flag)
        return (object) flag;
      if (obj.IsNumeric())
        return (object) Convert.ToBoolean(obj);
      bool result;
      return bool.TryParse(obj.ToString(), out result) ? (object) result : (object) result;
    }
  }
}

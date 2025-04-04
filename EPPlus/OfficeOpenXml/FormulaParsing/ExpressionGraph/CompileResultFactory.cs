// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.CompileResultFactory
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph
{
  public class CompileResultFactory
  {
    public virtual CompileResult Create(object obj)
    {
      if (obj == null)
        return new CompileResult((object) null, DataType.Empty);
      if (obj.GetType().Equals(typeof (string)))
        return new CompileResult(obj, DataType.String);
      if (obj.GetType().Equals(typeof (double)) || obj is Decimal)
        return new CompileResult(obj, DataType.Decimal);
      if (obj.GetType().Equals(typeof (int)) || obj is long || obj is short)
        return new CompileResult(obj, DataType.Integer);
      if (obj.GetType().Equals(typeof (bool)))
        return new CompileResult(obj, DataType.Boolean);
      if (obj.GetType().Equals(typeof (ExcelErrorValue)))
        return new CompileResult(obj, DataType.ExcelError);
      return obj.GetType().Equals(typeof (DateTime)) ? new CompileResult((object) ((DateTime) obj).ToOADate(), DataType.Date) : throw new ArgumentException("Non supported type " + obj.GetType().FullName);
    }
  }
}

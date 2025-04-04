// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Exceptions.ExcelErrorValueException
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Exceptions
{
  public class ExcelErrorValueException : Exception
  {
    public ExcelErrorValueException(ExcelErrorValue error)
      : this(error.ToString(), error)
    {
    }

    public ExcelErrorValueException(string message, ExcelErrorValue error)
      : base(message)
    {
      this.ErrorValue = error;
    }

    public ExcelErrorValueException(eErrorType errorType)
      : this(ExcelErrorValue.Create(errorType))
    {
    }

    public ExcelErrorValue ErrorValue { get; private set; }
  }
}

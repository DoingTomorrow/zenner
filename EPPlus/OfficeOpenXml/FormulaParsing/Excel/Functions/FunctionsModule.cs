// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.FunctionsModule
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions
{
  public abstract class FunctionsModule : IFunctionModule
  {
    private readonly Dictionary<string, ExcelFunction> _functions = new Dictionary<string, ExcelFunction>();

    public IDictionary<string, ExcelFunction> Functions
    {
      get => (IDictionary<string, ExcelFunction>) this._functions;
    }
  }
}

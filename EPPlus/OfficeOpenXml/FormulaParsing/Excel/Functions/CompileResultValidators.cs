// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.CompileResultValidators
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions
{
  public class CompileResultValidators
  {
    private readonly Dictionary<DataType, CompileResultValidator> _validators = new Dictionary<DataType, CompileResultValidator>();

    private CompileResultValidator CreateOrGet(DataType dataType)
    {
      if (this._validators.ContainsKey(dataType))
        return this._validators[dataType];
      return dataType == DataType.Decimal ? (this._validators[DataType.Decimal] = (CompileResultValidator) new DecimalCompileResultValidator()) : CompileResultValidator.Empty;
    }

    public CompileResultValidator GetValidator(DataType dataType) => this.CreateOrGet(dataType);
  }
}

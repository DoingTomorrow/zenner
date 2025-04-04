// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup.Indirect
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup
{
  public class Indirect : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 1);
      string address = this.ArgToString(arguments, 0);
      ExcelAddress excelAddress = new ExcelAddress(address);
      string worksheetName = excelAddress.WorkSheet;
      if (string.IsNullOrEmpty(worksheetName))
        worksheetName = context.Scopes.Current.Address.Worksheet;
      ExcelDataProvider.IRangeInfo range = context.ExcelDataProvider.GetRange(worksheetName, excelAddress._fromRow, excelAddress._fromCol, address);
      return range.IsEmpty ? CompileResult.Empty : new CompileResult((object) range, DataType.Enumerable);
    }
  }
}

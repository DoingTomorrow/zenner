// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup.Address
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExcelUtilities;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup
{
  public class Address : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 2);
      int row = this.ArgToInt(arguments, 0);
      int col = this.ArgToInt(arguments, 1);
      this.ThrowExcelErrorValueExceptionIf((Func<bool>) (() => row < 0 && col < 0), eErrorType.Value);
      ExcelReferenceType referenceType = ExcelReferenceType.AbsoluteRowAndColumn;
      string str = string.Empty;
      if (arguments.Count<FunctionArgument>() > 2)
      {
        int arg3 = this.ArgToInt(arguments, 2);
        this.ThrowExcelErrorValueExceptionIf((Func<bool>) (() => arg3 < 1 || arg3 > 4), eErrorType.Value);
        referenceType = (ExcelReferenceType) this.ArgToInt(arguments, 2);
      }
      if (arguments.Count<FunctionArgument>() > 3)
      {
        object obj = arguments.ElementAt<FunctionArgument>(3).Value;
        if (obj.GetType().Equals(typeof (bool)) && !(bool) obj)
          throw new InvalidOperationException("Excelformulaparser does not support the R1C1 format!");
        if (obj.GetType().Equals(typeof (string)))
          str = obj.ToString() + "!";
      }
      IndexToAddressTranslator addressTranslator = new IndexToAddressTranslator(context.ExcelDataProvider, referenceType);
      return this.CreateResult((object) (str + addressTranslator.ToAddress(col, row)), DataType.ExcelAddress);
    }
  }
}

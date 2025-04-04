// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup.Rows
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
  public class Rows : LookupFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 1);
      ExcelDataProvider.IRangeInfo valueAsRangeInfo = arguments.ElementAt<FunctionArgument>(0).ValueAsRangeInfo;
      if (valueAsRangeInfo != null)
        return this.CreateResult((object) (valueAsRangeInfo.Address._toRow - valueAsRangeInfo.Address._fromRow + 1), DataType.Integer);
      string str = this.ArgToString(arguments, 0);
      if (!ExcelAddressUtil.IsValidAddress(str))
        throw new ArgumentException("Invalid range supplied");
      RangeAddress rangeAddress = new RangeAddressFactory(context.ExcelDataProvider).Create(str);
      return this.CreateResult((object) (rangeAddress.ToRow - rangeAddress.FromRow + 1), DataType.Integer);
    }
  }
}

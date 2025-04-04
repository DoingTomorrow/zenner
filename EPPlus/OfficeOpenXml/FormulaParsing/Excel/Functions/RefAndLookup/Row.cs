// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup.Row
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
  public class Row : LookupFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      if (arguments == null || arguments.Count<FunctionArgument>() == 0)
        return this.CreateResult((object) context.Scopes.Current.Address.FromRow, DataType.Integer);
      ExcelDataProvider.IRangeInfo valueAsRangeInfo = arguments.ElementAt<FunctionArgument>(0).ValueAsRangeInfo;
      if (valueAsRangeInfo != null)
        return this.CreateResult((object) valueAsRangeInfo.Address._fromRow, DataType.Integer);
      string str = this.ArgToString(arguments, 0);
      if (ExcelAddressUtil.IsValidAddress(str))
        return this.CreateResult((object) new RangeAddressFactory(context.ExcelDataProvider).Create(str).FromRow, DataType.Integer);
      throw new ArgumentException("An invalid argument was supplied");
    }
  }
}

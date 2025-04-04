// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup.HLookup
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup
{
  public class HLookup : LookupFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 3);
      LookupArguments lookupArgs = new LookupArguments(arguments);
      this.ThrowExcelErrorValueExceptionIf((Func<bool>) (() => lookupArgs.LookupIndex < 1), eErrorType.Value);
      return this.Lookup(LookupNavigatorFactory.Create(LookupDirection.Horizontal, lookupArgs, context), lookupArgs);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Text.Exact
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Text
{
  public class Exact : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 2);
      object valueFirst1 = arguments.ElementAt<FunctionArgument>(0).ValueFirst;
      object valueFirst2 = arguments.ElementAt<FunctionArgument>(1).ValueFirst;
      if (valueFirst1 == null && valueFirst2 == null)
        return this.CreateResult((object) true, DataType.Boolean);
      return valueFirst1 == null && valueFirst2 != null || valueFirst1 != null && valueFirst2 == null ? this.CreateResult((object) false, DataType.Boolean) : this.CreateResult((object) (string.Compare(valueFirst1.ToString(), valueFirst2.ToString(), StringComparison.InvariantCulture) == 0), DataType.Boolean);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.HiddenValuesHandlingFunction
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions
{
  public abstract class HiddenValuesHandlingFunction : ExcelFunction
  {
    public bool IgnoreHiddenValues { get; set; }

    protected override IEnumerable<double> ArgsToDoubleEnumerable(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      return this.ArgsToDoubleEnumerable(arguments, context, true);
    }

    protected IEnumerable<double> ArgsToDoubleEnumerable(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context,
      bool ignoreErrors)
    {
      if (!arguments.Any<FunctionArgument>())
        return Enumerable.Empty<double>();
      return this.IgnoreHiddenValues ? this.ArgsToDoubleEnumerable(this.IgnoreHiddenValues, arguments.Where<FunctionArgument>((Func<FunctionArgument, bool>) (x => !x.ExcelStateFlagIsSet(ExcelCellState.HiddenCell))), context) : this.ArgsToDoubleEnumerable(this.IgnoreHiddenValues, ignoreErrors, arguments, context);
    }

    protected bool ShouldIgnore(ExcelDataProvider.ICellInfo c, ParsingContext context)
    {
      return CellStateHelper.ShouldIgnore(this.IgnoreHiddenValues, c, context);
    }

    protected bool ShouldIgnore(FunctionArgument arg)
    {
      return this.IgnoreHiddenValues && arg.ExcelStateFlagIsSet(ExcelCellState.HiddenCell);
    }
  }
}

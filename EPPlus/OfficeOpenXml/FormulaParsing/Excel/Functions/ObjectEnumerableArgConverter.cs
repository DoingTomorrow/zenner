// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.ObjectEnumerableArgConverter
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions
{
  public class ObjectEnumerableArgConverter : CollectionFlattener<object>
  {
    public virtual IEnumerable<object> ConvertArgs(
      bool ignoreHidden,
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      return this.FuncArgsToFlatEnumerable(arguments, (Action<FunctionArgument, IList<object>>) ((arg, argList) =>
      {
        if (arg.Value is ExcelDataProvider.IRangeInfo)
        {
          foreach (ExcelDataProvider.ICellInfo c in (IEnumerable<ExcelDataProvider.ICellInfo>) arg.Value)
          {
            if (!CellStateHelper.ShouldIgnore(ignoreHidden, c, context))
              argList.Add(c.Value);
          }
        }
        else
          argList.Add(arg.Value);
      }));
    }
  }
}

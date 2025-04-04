// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup.Choose
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup
{
  public class Choose : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 2);
      int index1 = this.ArgToInt(arguments, 0);
      List<string> stringList = new List<string>();
      for (int index2 = 0; index2 < arguments.Count<FunctionArgument>(); ++index2)
        stringList.Add(arguments.ElementAt<FunctionArgument>(index2).ValueFirst.ToString());
      return this.CreateResult((object) stringList[index1], DataType.String);
    }
  }
}

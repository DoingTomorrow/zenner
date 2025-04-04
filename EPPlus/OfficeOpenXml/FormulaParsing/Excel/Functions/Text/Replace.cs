// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Text.Replace
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Text
{
  public class Replace : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 4);
      string text = this.ArgToString(arguments, 0);
      int startPos = this.ArgToInt(arguments, 1);
      int nCharactersToReplace = this.ArgToInt(arguments, 2);
      string str = this.ArgToString(arguments, 3);
      string firstPart = this.GetFirstPart(text, startPos);
      string lastPart = this.GetLastPart(text, startPos, nCharactersToReplace);
      return this.CreateResult((object) (firstPart + str + lastPart), DataType.String);
    }

    private string GetFirstPart(string text, int startPos) => text.Substring(0, startPos - 1);

    private string GetLastPart(string text, int startPos, int nCharactersToReplace)
    {
      int startIndex = startPos - 1 + nCharactersToReplace;
      return text.Substring(startIndex, text.Length - startIndex);
    }
  }
}

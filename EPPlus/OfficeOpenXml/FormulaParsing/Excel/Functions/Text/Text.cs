// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Text.Text
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Text
{
  public class Text : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 2);
      object valueFirst = arguments.First<FunctionArgument>().ValueFirst;
      string format = this.ArgToString(arguments, 1).Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".").Replace(CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator.Replace(' ', ' '), ",");
      return this.CreateResult((object) context.ExcelDataProvider.GetFormat(valueFirst, format), DataType.String);
    }
  }
}

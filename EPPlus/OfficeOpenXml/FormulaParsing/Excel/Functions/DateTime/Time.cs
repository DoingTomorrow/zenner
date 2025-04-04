// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime.Time
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime
{
  public class Time : TimeBaseFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 1);
      string input = arguments.ElementAt<FunctionArgument>(0).Value.ToString();
      if (arguments.Count<FunctionArgument>() == 1 && this.TimeStringParser.CanParse(input))
        return new CompileResult((object) this.TimeStringParser.Parse(input), DataType.Time);
      this.ValidateArguments(arguments, 3);
      int hour = this.ArgToInt(arguments, 0);
      int min = this.ArgToInt(arguments, 1);
      int sec = this.ArgToInt(arguments, 2);
      this.ThrowArgumentExceptionIf((Func<bool>) (() => sec < 0 || sec > 59), "Invalid second: " + (object) sec);
      this.ThrowArgumentExceptionIf((Func<bool>) (() => min < 0 || min > 59), "Invalid minute: " + (object) min);
      this.ThrowArgumentExceptionIf((Func<bool>) (() => min < 0 || hour > 23), "Invalid hour: " + (object) hour);
      return this.CreateResult((object) this.GetTimeSerialNumber((double) (hour * 60 * 60 + min * 60 + sec)), DataType.Time);
    }
  }
}

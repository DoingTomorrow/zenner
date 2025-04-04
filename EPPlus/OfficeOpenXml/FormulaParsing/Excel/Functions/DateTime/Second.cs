// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime.Second
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
  public class Second : TimeBaseFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 1);
      string input = arguments.ElementAt<FunctionArgument>(0).Value.ToString();
      if (arguments.Count<FunctionArgument>() == 1 && this.TimeStringParser.CanParse(input))
        return this.CreateResult((object) this.GetSecondFromSerialNumber(this.TimeStringParser.Parse(input)), DataType.Integer);
      this.ValidateAndInitSerialNumber(arguments);
      return this.CreateResult((object) this.GetSecondFromSerialNumber(this.SerialNumber), DataType.Integer);
    }

    private int GetSecondFromSerialNumber(double serialNumber)
    {
      return (int) Math.Round(this.GetSecond(serialNumber), 0);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime.Weekday
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Exceptions;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime
{
  public class Weekday : ExcelFunction
  {
    private static List<int> _oneBasedStartOnSunday = new List<int>()
    {
      1,
      2,
      3,
      4,
      5,
      6,
      7
    };
    private static List<int> _oneBasedStartOnMonday = new List<int>()
    {
      7,
      1,
      2,
      3,
      4,
      5,
      6
    };
    private static List<int> _zeroBasedStartOnSunday = new List<int>()
    {
      6,
      0,
      1,
      2,
      3,
      4,
      5
    };

    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 1);
      double d = this.ArgToDecimal(arguments, 0);
      int returnType = arguments.Count<FunctionArgument>() > 1 ? this.ArgToInt(arguments, 1) : 1;
      return this.CreateResult((object) this.CalculateDayOfWeek(System.DateTime.FromOADate(d), returnType), DataType.Integer);
    }

    private int CalculateDayOfWeek(System.DateTime dateTime, int returnType)
    {
      int dayOfWeek = (int) dateTime.DayOfWeek;
      switch (returnType)
      {
        case 1:
          return Weekday._oneBasedStartOnSunday[dayOfWeek];
        case 2:
          return Weekday._oneBasedStartOnMonday[dayOfWeek];
        case 3:
          return Weekday._zeroBasedStartOnSunday[dayOfWeek];
        default:
          throw new ExcelErrorValueException(eErrorType.Num);
      }
    }
  }
}

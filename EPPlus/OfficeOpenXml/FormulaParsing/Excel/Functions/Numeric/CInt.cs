// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric.CInt
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric
{
  public class CInt : ExcelFunction
  {
    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 1);
      return this.CreateResult(this.ToInteger(arguments.ElementAt<FunctionArgument>(0).ValueFirst), DataType.Integer);
    }

    private object ToInteger(object obj)
    {
      Type type = obj.GetType();
      if (type == typeof (double))
        return (object) (int) Math.Floor((double) obj);
      if (type == typeof (Decimal))
        return (object) (int) Math.Floor((Decimal) obj);
      double result;
      if (double.TryParse(this.HandleDecimalSeparator(obj), out result))
        return (object) (int) Math.Floor(result);
      throw new ArgumentException("Could not cast supplied argument to integer");
    }

    private string HandleDecimalSeparator(object obj)
    {
      string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
      string str = obj != null ? obj.ToString() : string.Empty;
      if (decimalSeparator == ",")
        str = str.Replace(".", ",");
      return str;
    }
  }
}

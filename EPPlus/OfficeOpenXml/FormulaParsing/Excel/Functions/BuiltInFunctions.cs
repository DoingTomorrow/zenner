// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.BuiltInFunctions
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions
{
  public class BuiltInFunctions : FunctionsModule
  {
    public BuiltInFunctions()
    {
      this.Functions["len"] = (ExcelFunction) new Len();
      this.Functions["lower"] = (ExcelFunction) new Lower();
      this.Functions["upper"] = (ExcelFunction) new Upper();
      this.Functions["left"] = (ExcelFunction) new Left();
      this.Functions["right"] = (ExcelFunction) new Right();
      this.Functions["mid"] = (ExcelFunction) new Mid();
      this.Functions["replace"] = (ExcelFunction) new Replace();
      this.Functions["substitute"] = (ExcelFunction) new Substitute();
      this.Functions["concatenate"] = (ExcelFunction) new Concatenate();
      this.Functions["exact"] = (ExcelFunction) new Exact();
      this.Functions["find"] = (ExcelFunction) new Find();
      this.Functions["proper"] = (ExcelFunction) new Proper();
      this.Functions["text"] = (ExcelFunction) new OfficeOpenXml.FormulaParsing.Excel.Functions.Text.Text();
      this.Functions["t"] = (ExcelFunction) new T();
      this.Functions["int"] = (ExcelFunction) new CInt();
      this.Functions["abs"] = (ExcelFunction) new Abs();
      this.Functions["cos"] = (ExcelFunction) new Cos();
      this.Functions["cosh"] = (ExcelFunction) new Cosh();
      this.Functions["power"] = (ExcelFunction) new Power();
      this.Functions["sign"] = (ExcelFunction) new Sign();
      this.Functions["sqrt"] = (ExcelFunction) new Sqrt();
      this.Functions["sqrtpi"] = (ExcelFunction) new SqrtPi();
      this.Functions["pi"] = (ExcelFunction) new Pi();
      this.Functions["product"] = (ExcelFunction) new Product();
      this.Functions["ceiling"] = (ExcelFunction) new Ceiling();
      this.Functions["count"] = (ExcelFunction) new Count();
      this.Functions["counta"] = (ExcelFunction) new CountA();
      this.Functions["countif"] = (ExcelFunction) new CountIf();
      this.Functions["fact"] = (ExcelFunction) new Fact();
      this.Functions["floor"] = (ExcelFunction) new Floor();
      this.Functions["sin"] = (ExcelFunction) new Sin();
      this.Functions["sinh"] = (ExcelFunction) new Sinh();
      this.Functions["sum"] = (ExcelFunction) new Sum();
      this.Functions["sumif"] = (ExcelFunction) new SumIf();
      this.Functions["sumproduct"] = (ExcelFunction) new SumProduct();
      this.Functions["sumsq"] = (ExcelFunction) new Sumsq();
      this.Functions["stdev"] = (ExcelFunction) new Stdev();
      this.Functions["stdevp"] = (ExcelFunction) new StdevP();
      this.Functions["stdev.s"] = (ExcelFunction) new Stdev();
      this.Functions["stdev.p"] = (ExcelFunction) new StdevP();
      this.Functions["subtotal"] = (ExcelFunction) new Subtotal();
      this.Functions["exp"] = (ExcelFunction) new Exp();
      this.Functions["log"] = (ExcelFunction) new Log();
      this.Functions["log10"] = (ExcelFunction) new Log10();
      this.Functions["ln"] = (ExcelFunction) new Ln();
      this.Functions["max"] = (ExcelFunction) new Max();
      this.Functions["maxa"] = (ExcelFunction) new Maxa();
      this.Functions["min"] = (ExcelFunction) new Min();
      this.Functions["mod"] = (ExcelFunction) new Mod();
      this.Functions["average"] = (ExcelFunction) new Average();
      this.Functions["averagea"] = (ExcelFunction) new AverageA();
      this.Functions["averageif"] = (ExcelFunction) new AverageIf();
      this.Functions["round"] = (ExcelFunction) new Round();
      this.Functions["rounddown"] = (ExcelFunction) new Rounddown();
      this.Functions["roundup"] = (ExcelFunction) new Roundup();
      this.Functions["rand"] = (ExcelFunction) new Rand();
      this.Functions["randbetween"] = (ExcelFunction) new RandBetween();
      this.Functions["quotient"] = (ExcelFunction) new Quotient();
      this.Functions["trunc"] = (ExcelFunction) new Trunc();
      this.Functions["tan"] = (ExcelFunction) new Tan();
      this.Functions["tanh"] = (ExcelFunction) new Tanh();
      this.Functions["atan"] = (ExcelFunction) new Atan();
      this.Functions["atan2"] = (ExcelFunction) new Atan2();
      this.Functions["var"] = (ExcelFunction) new Var();
      this.Functions["varp"] = (ExcelFunction) new VarP();
      this.Functions["isblank"] = (ExcelFunction) new IsBlank();
      this.Functions["isnumber"] = (ExcelFunction) new IsNumber();
      this.Functions["istext"] = (ExcelFunction) new IsText();
      this.Functions["iserror"] = (ExcelFunction) new IsError();
      this.Functions["iserr"] = (ExcelFunction) new IsErr();
      this.Functions["iseven"] = (ExcelFunction) new IsEven();
      this.Functions["isodd"] = (ExcelFunction) new IsOdd();
      this.Functions["islogical"] = (ExcelFunction) new IsLogical();
      this.Functions["isna"] = (ExcelFunction) new IsNa();
      this.Functions["na"] = (ExcelFunction) new Na();
      this.Functions["n"] = (ExcelFunction) new N();
      this.Functions["if"] = (ExcelFunction) new If();
      this.Functions["not"] = (ExcelFunction) new Not();
      this.Functions["and"] = (ExcelFunction) new And();
      this.Functions["or"] = (ExcelFunction) new Or();
      this.Functions["true"] = (ExcelFunction) new True();
      this.Functions["address"] = (ExcelFunction) new Address();
      this.Functions["hlookup"] = (ExcelFunction) new HLookup();
      this.Functions["vlookup"] = (ExcelFunction) new VLookup();
      this.Functions["lookup"] = (ExcelFunction) new Lookup();
      this.Functions["match"] = (ExcelFunction) new Match();
      this.Functions["row"] = (ExcelFunction) new Row();
      IDictionary<string, ExcelFunction> functions1 = this.Functions;
      Rows rows1 = new Rows();
      rows1.SkipArgumentEvaluation = true;
      Rows rows2 = rows1;
      functions1["rows"] = (ExcelFunction) rows2;
      this.Functions["column"] = (ExcelFunction) new Column();
      IDictionary<string, ExcelFunction> functions2 = this.Functions;
      Columns columns1 = new Columns();
      columns1.SkipArgumentEvaluation = true;
      Columns columns2 = columns1;
      functions2["columns"] = (ExcelFunction) columns2;
      this.Functions["choose"] = (ExcelFunction) new Choose();
      this.Functions["index"] = (ExcelFunction) new Index();
      this.Functions["indirect"] = (ExcelFunction) new Indirect();
      this.Functions["date"] = (ExcelFunction) new Date();
      this.Functions["today"] = (ExcelFunction) new Today();
      this.Functions["now"] = (ExcelFunction) new Now();
      this.Functions["day"] = (ExcelFunction) new Day();
      this.Functions["month"] = (ExcelFunction) new Month();
      this.Functions["year"] = (ExcelFunction) new Year();
      this.Functions["time"] = (ExcelFunction) new Time();
      this.Functions["hour"] = (ExcelFunction) new Hour();
      this.Functions["minute"] = (ExcelFunction) new Minute();
      this.Functions["second"] = (ExcelFunction) new Second();
      this.Functions["weeknum"] = (ExcelFunction) new Weeknum();
      this.Functions["weekday"] = (ExcelFunction) new Weekday();
      this.Functions["days360"] = (ExcelFunction) new Days360();
      this.Functions["yearfrac"] = (ExcelFunction) new Yearfrac();
      this.Functions["edate"] = (ExcelFunction) new Edate();
      this.Functions["eomonth"] = (ExcelFunction) new Eomonth();
      this.Functions["isoweeknum"] = (ExcelFunction) new IsoWeekNum();
      this.Functions["workday"] = (ExcelFunction) new Workday();
    }
  }
}

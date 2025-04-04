// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.Excel.Functions.Math.Maxa
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using OfficeOpenXml.FormulaParsing.Utilities;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.Excel.Functions.Math
{
  public class Maxa : ExcelFunction
  {
    private readonly DoubleEnumerableArgConverter _argConverter;

    public Maxa()
      : this(new DoubleEnumerableArgConverter())
    {
    }

    public Maxa(DoubleEnumerableArgConverter argConverter)
    {
      Require.That<DoubleEnumerableArgConverter>(argConverter).Named(nameof (argConverter)).IsNotNull<DoubleEnumerableArgConverter>();
      this._argConverter = argConverter;
    }

    public override CompileResult Execute(
      IEnumerable<FunctionArgument> arguments,
      ParsingContext context)
    {
      this.ValidateArguments(arguments, 1);
      return this.CreateResult((object) this._argConverter.ConvertArgsIncludingOtherTypes(arguments).Max(), DataType.Decimal);
    }
  }
}

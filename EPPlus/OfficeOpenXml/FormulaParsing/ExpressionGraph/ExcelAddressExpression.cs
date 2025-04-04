// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ExpressionGraph.ExcelAddressExpression
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExcelUtilities;
using OfficeOpenXml.FormulaParsing.Utilities;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.ExpressionGraph
{
  public class ExcelAddressExpression : AtomicExpression
  {
    private readonly ExcelDataProvider _excelDataProvider;
    private readonly ParsingContext _parsingContext;
    private readonly RangeAddressFactory _rangeAddressFactory;
    private readonly bool _negate;

    public ExcelAddressExpression(
      string expression,
      ExcelDataProvider excelDataProvider,
      ParsingContext parsingContext)
      : this(expression, excelDataProvider, parsingContext, new RangeAddressFactory(excelDataProvider), false)
    {
    }

    public ExcelAddressExpression(
      string expression,
      ExcelDataProvider excelDataProvider,
      ParsingContext parsingContext,
      bool negate)
      : this(expression, excelDataProvider, parsingContext, new RangeAddressFactory(excelDataProvider), negate)
    {
    }

    public ExcelAddressExpression(
      string expression,
      ExcelDataProvider excelDataProvider,
      ParsingContext parsingContext,
      RangeAddressFactory rangeAddressFactory,
      bool negate)
      : base(expression)
    {
      Require.That<ExcelDataProvider>(excelDataProvider).Named(nameof (excelDataProvider)).IsNotNull<ExcelDataProvider>();
      Require.That<ParsingContext>(parsingContext).Named(nameof (parsingContext)).IsNotNull<ParsingContext>();
      Require.That<RangeAddressFactory>(rangeAddressFactory).Named(nameof (rangeAddressFactory)).IsNotNull<RangeAddressFactory>();
      this._excelDataProvider = excelDataProvider;
      this._parsingContext = parsingContext;
      this._rangeAddressFactory = rangeAddressFactory;
      this._negate = negate;
    }

    public override bool IsGroupedExpression => false;

    public override CompileResult Compile()
    {
      return this.ParentIsLookupFunction ? new CompileResult((object) this.ExpressionString, DataType.ExcelAddress) : this.CompileRangeValues();
    }

    private CompileResult CompileRangeValues()
    {
      ParsingScope current = this._parsingContext.Scopes.Current;
      ExcelDataProvider.IRangeInfo range = this._excelDataProvider.GetRange(current.Address.Worksheet, current.Address.FromRow, current.Address.FromCol, this.ExpressionString);
      return range == null ? CompileResult.Empty : new CompileResult((object) range, DataType.Enumerable);
    }

    private CompileResult CompileSingleCell(ExcelDataProvider.IRangeInfo result)
    {
      ExcelDataProvider.ICellInfo cellInfo = result.First<ExcelDataProvider.ICellInfo>();
      CompileResult compileResult = new CompileResultFactory().Create(cellInfo.Value);
      if (this._negate && compileResult.IsNumeric)
        compileResult = new CompileResult((object) (compileResult.ResultNumeric * -1.0), compileResult.DataType);
      compileResult.IsHiddenCell = cellInfo.IsHiddenRow;
      return compileResult;
    }
  }
}

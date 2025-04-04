// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.FormulaParser
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Excel.Functions;
using OfficeOpenXml.FormulaParsing.ExcelUtilities;
using OfficeOpenXml.FormulaParsing.Exceptions;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using OfficeOpenXml.FormulaParsing.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing
{
  public class FormulaParser
  {
    private readonly ParsingContext _parsingContext;
    private readonly ExcelDataProvider _excelDataProvider;
    private ILexer _lexer;
    private IExpressionGraphBuilder _graphBuilder;
    private IExpressionCompiler _compiler;

    public FormulaParser(ExcelDataProvider excelDataProvider)
      : this(excelDataProvider, ParsingContext.Create())
    {
    }

    public FormulaParser(ExcelDataProvider excelDataProvider, ParsingContext parsingContext)
    {
      FormulaParser formulaParser = this;
      parsingContext.Parser = this;
      parsingContext.ExcelDataProvider = excelDataProvider;
      parsingContext.NameValueProvider = (INameValueProvider) new EpplusNameValueProvider(excelDataProvider);
      parsingContext.RangeAddressFactory = new RangeAddressFactory(excelDataProvider);
      this._parsingContext = parsingContext;
      this._excelDataProvider = excelDataProvider;
      this.Configure((Action<ParsingConfiguration>) (configuration => configuration.SetLexer((ILexer) new OfficeOpenXml.FormulaParsing.LexicalAnalysis.Lexer(formulaParser._parsingContext.Configuration.FunctionRepository, formulaParser._parsingContext.NameValueProvider)).SetGraphBuilder((IExpressionGraphBuilder) new ExpressionGraphBuilder(excelDataProvider, formulaParser._parsingContext)).SetExpresionCompiler((IExpressionCompiler) new ExpressionCompiler()).SetIdProvider((IdProvider) new IntegerIdProvider()).FunctionRepository.LoadModule((IFunctionModule) new BuiltInFunctions())));
    }

    public void Configure(Action<ParsingConfiguration> configMethod)
    {
      configMethod(this._parsingContext.Configuration);
      this._lexer = this._parsingContext.Configuration.Lexer ?? this._lexer;
      this._graphBuilder = this._parsingContext.Configuration.GraphBuilder ?? this._graphBuilder;
      this._compiler = this._parsingContext.Configuration.ExpressionCompiler ?? this._compiler;
    }

    public ILexer Lexer => this._lexer;

    public IEnumerable<string> FunctionNames
    {
      get => this._parsingContext.Configuration.FunctionRepository.FunctionNames;
    }

    internal virtual object Parse(string formula, RangeAddress rangeAddress)
    {
      using (this._parsingContext.Scopes.NewScope(rangeAddress))
      {
        OfficeOpenXml.FormulaParsing.ExpressionGraph.ExpressionGraph expressionGraph = this._graphBuilder.Build(this._lexer.Tokenize(formula));
        return expressionGraph.Expressions.Count<Expression>() == 0 ? (object) null : this._compiler.Compile(expressionGraph.Expressions).Result;
      }
    }

    internal virtual object Parse(IEnumerable<Token> tokens, string worksheet, string address)
    {
      using (this._parsingContext.Scopes.NewScope(this._parsingContext.RangeAddressFactory.Create(address)))
      {
        OfficeOpenXml.FormulaParsing.ExpressionGraph.ExpressionGraph expressionGraph = this._graphBuilder.Build(tokens);
        return expressionGraph.Expressions.Count<Expression>() == 0 ? (object) null : this._compiler.Compile(expressionGraph.Expressions).Result;
      }
    }

    internal virtual object ParseCell(
      IEnumerable<Token> tokens,
      string worksheet,
      int row,
      int column)
    {
      using (this._parsingContext.Scopes.NewScope(this._parsingContext.RangeAddressFactory.Create(worksheet, column, row)))
      {
        OfficeOpenXml.FormulaParsing.ExpressionGraph.ExpressionGraph expressionGraph = this._graphBuilder.Build(tokens);
        if (expressionGraph.Expressions.Count<Expression>() == 0)
          return (object) 0.0;
        try
        {
          CompileResult compileResult = this._compiler.Compile(expressionGraph.Expressions);
          if (!(compileResult.Result is ExcelDataProvider.IRangeInfo result))
            return compileResult.Result ?? (object) 0.0;
          if (result.IsEmpty)
            return (object) 0.0;
          if (!result.IsMulti)
            return result.First<ExcelDataProvider.ICellInfo>().Value ?? (object) 0.0;
          throw new ExcelErrorValueException(eErrorType.Value);
        }
        catch (ExcelErrorValueException ex)
        {
          return (object) ex.ErrorValue;
        }
      }
    }

    public virtual object Parse(string formula, string address)
    {
      return this.Parse(formula, this._parsingContext.RangeAddressFactory.Create(address));
    }

    public virtual object Parse(string formula) => this.Parse(formula, RangeAddress.Empty);

    public virtual object ParseAt(string address)
    {
      Require.That<string>(address).Named(nameof (address)).IsNotNullOrEmpty();
      RangeAddress rangeAddress = this._parsingContext.RangeAddressFactory.Create(address);
      return this.ParseAt(rangeAddress.Worksheet, rangeAddress.FromRow, rangeAddress.FromCol);
    }

    public virtual object ParseAt(string worksheetName, int row, int col)
    {
      string rangeFormula = this._excelDataProvider.GetRangeFormula(worksheetName, row, col);
      return string.IsNullOrEmpty(rangeFormula) ? this._excelDataProvider.GetRangeValue(worksheetName, row, col) : this.Parse(rangeFormula, this._parsingContext.RangeAddressFactory.Create(worksheetName, col, row));
    }
  }
}

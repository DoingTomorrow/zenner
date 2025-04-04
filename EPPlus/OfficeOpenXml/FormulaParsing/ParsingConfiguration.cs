// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ParsingConfiguration
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Excel.Functions;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using OfficeOpenXml.FormulaParsing.Utilities;

#nullable disable
namespace OfficeOpenXml.FormulaParsing
{
  public class ParsingConfiguration
  {
    public virtual ILexer Lexer { get; private set; }

    public virtual IdProvider IdProvider { get; private set; }

    public IExpressionGraphBuilder GraphBuilder { get; private set; }

    public IExpressionCompiler ExpressionCompiler { get; private set; }

    public FunctionRepository FunctionRepository { get; private set; }

    private ParsingConfiguration() => this.FunctionRepository = FunctionRepository.Create();

    internal static ParsingConfiguration Create() => new ParsingConfiguration();

    public ParsingConfiguration SetIdProvider(IdProvider idProvider)
    {
      this.IdProvider = idProvider;
      return this;
    }

    public ParsingConfiguration SetLexer(ILexer lexer)
    {
      this.Lexer = lexer;
      return this;
    }

    public ParsingConfiguration SetGraphBuilder(IExpressionGraphBuilder graphBuilder)
    {
      this.GraphBuilder = graphBuilder;
      return this;
    }

    public ParsingConfiguration SetExpresionCompiler(IExpressionCompiler expressionCompiler)
    {
      this.ExpressionCompiler = expressionCompiler;
      return this;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.LexicalAnalysis.Lexer
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Excel.Functions;
using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.LexicalAnalysis
{
  public class Lexer : ILexer
  {
    private readonly ISourceCodeTokenizer _tokenizer;
    private readonly ISyntacticAnalyzer _analyzer;

    public Lexer(FunctionRepository functionRepository, INameValueProvider nameValueProvider)
      : this((ISourceCodeTokenizer) new SourceCodeTokenizer((IFunctionNameProvider) functionRepository, nameValueProvider), (ISyntacticAnalyzer) new SyntacticAnalyzer())
    {
    }

    public Lexer(ISourceCodeTokenizer tokenizer, ISyntacticAnalyzer analyzer)
    {
      this._tokenizer = tokenizer;
      this._analyzer = analyzer;
    }

    public IEnumerable<Token> Tokenize(string input)
    {
      IEnumerable<Token> tokens = this._tokenizer.Tokenize(input);
      this._analyzer.Analyze(tokens);
      return tokens;
    }
  }
}

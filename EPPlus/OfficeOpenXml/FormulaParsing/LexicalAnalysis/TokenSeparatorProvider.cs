// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.LexicalAnalysis.TokenSeparatorProvider
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Collections.Generic;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.LexicalAnalysis
{
  public class TokenSeparatorProvider : ITokenSeparatorProvider
  {
    private bool _isInitialized;
    private static Dictionary<string, Token> _tokens = new Dictionary<string, Token>();

    public TokenSeparatorProvider()
    {
      if (this._isInitialized)
        return;
      TokenSeparatorProvider.Init();
    }

    private static void Init()
    {
      TokenSeparatorProvider._tokens.Clear();
      TokenSeparatorProvider._tokens.Add("+", new Token("+", TokenType.Operator));
      TokenSeparatorProvider._tokens.Add("-", new Token("-", TokenType.Operator));
      TokenSeparatorProvider._tokens.Add("*", new Token("*", TokenType.Operator));
      TokenSeparatorProvider._tokens.Add("/", new Token("/", TokenType.Operator));
      TokenSeparatorProvider._tokens.Add("^", new Token("^", TokenType.Operator));
      TokenSeparatorProvider._tokens.Add("&", new Token("&", TokenType.Operator));
      TokenSeparatorProvider._tokens.Add(">", new Token(">", TokenType.Operator));
      TokenSeparatorProvider._tokens.Add("<", new Token("<", TokenType.Operator));
      TokenSeparatorProvider._tokens.Add("=", new Token("=", TokenType.Operator));
      TokenSeparatorProvider._tokens.Add("<=", new Token("<=", TokenType.Operator));
      TokenSeparatorProvider._tokens.Add(">=", new Token(">=", TokenType.Operator));
      TokenSeparatorProvider._tokens.Add("(", new Token("(", TokenType.OpeningParenthesis));
      TokenSeparatorProvider._tokens.Add(")", new Token(")", TokenType.ClosingParenthesis));
      TokenSeparatorProvider._tokens.Add("{", new Token("{", TokenType.OpeningEnumerable));
      TokenSeparatorProvider._tokens.Add("}", new Token("}", TokenType.ClosingEnumerable));
      TokenSeparatorProvider._tokens.Add("'", new Token("'", TokenType.String));
      TokenSeparatorProvider._tokens.Add("\"", new Token("\"", TokenType.String));
      TokenSeparatorProvider._tokens.Add(",", new Token(",", TokenType.Comma));
      TokenSeparatorProvider._tokens.Add(";", new Token(";", TokenType.SemiColon));
      TokenSeparatorProvider._tokens.Add("[", new Token("[", TokenType.OpeningBracket));
      TokenSeparatorProvider._tokens.Add("]", new Token("]", TokenType.ClosingBracket));
      TokenSeparatorProvider._tokens.Add("%", new Token("%", TokenType.Percent));
    }

    IDictionary<string, Token> ITokenSeparatorProvider.Tokens
    {
      get => (IDictionary<string, Token>) TokenSeparatorProvider._tokens;
    }

    public bool IsOperator(string item)
    {
      return TokenSeparatorProvider._tokens.ContainsKey(item) && TokenSeparatorProvider._tokens[item].TokenType == TokenType.Operator;
    }

    public bool IsPossibleLastPartOfMultipleCharOperator(string part) => part == "=";
  }
}

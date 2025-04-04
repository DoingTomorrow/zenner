// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.LexicalAnalysis.SourceCodeTokenizer
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Excel.Functions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.LexicalAnalysis
{
  public class SourceCodeTokenizer : ISourceCodeTokenizer
  {
    private readonly ITokenSeparatorProvider _tokenProvider;
    private readonly ITokenFactory _tokenFactory;

    public static ISourceCodeTokenizer Default
    {
      get
      {
        return (ISourceCodeTokenizer) new SourceCodeTokenizer((IFunctionNameProvider) FunctionNameProvider.Empty, NameValueProvider.Empty);
      }
    }

    public SourceCodeTokenizer(
      IFunctionNameProvider functionRepository,
      INameValueProvider nameValueProvider)
      : this((ITokenFactory) new TokenFactory(functionRepository, nameValueProvider), (ITokenSeparatorProvider) new TokenSeparatorProvider())
    {
    }

    public SourceCodeTokenizer(ITokenFactory tokenFactory, ITokenSeparatorProvider tokenProvider)
    {
      this._tokenFactory = tokenFactory;
      this._tokenProvider = tokenProvider;
    }

    public IEnumerable<Token> Tokenize(string input)
    {
      if (string.IsNullOrEmpty(input))
        return Enumerable.Empty<Token>();
      input = input.TrimStart('+');
      TokenizerContext context = new TokenizerContext(input);
      for (int index = 0; index < context.FormulaChars.Length; ++index)
      {
        char formulaChar = context.FormulaChars[index];
        Token token1;
        if (this.CharIsTokenSeparator(formulaChar, out token1))
        {
          if (context.IsInString)
          {
            if (token1.TokenType == TokenType.String && index + 1 < context.FormulaChars.Length && context.FormulaChars[index + 1] == '\'')
            {
              ++index;
              context.AppendToCurrentToken(formulaChar);
              continue;
            }
            if (token1.TokenType != TokenType.String)
            {
              context.AppendToCurrentToken(formulaChar);
              continue;
            }
          }
          if (token1.TokenType == TokenType.OpeningBracket)
          {
            context.AppendToCurrentToken(formulaChar);
            ++context.BracketCount;
          }
          else if (token1.TokenType == TokenType.ClosingBracket)
          {
            context.AppendToCurrentToken(formulaChar);
            --context.BracketCount;
          }
          else if (context.BracketCount > 0)
            context.AppendToCurrentToken(formulaChar);
          else if (this.IsPartOfMultipleCharSeparator(context, formulaChar))
          {
            Token token2 = this._tokenProvider.Tokens[context.LastToken.Value + formulaChar.ToString((IFormatProvider) CultureInfo.InvariantCulture)];
            context.ReplaceLastToken(token2);
            context.NewToken();
          }
          else
          {
            if (token1.TokenType == TokenType.String)
            {
              if (context.LastToken != null && context.LastToken.TokenType == TokenType.OpeningEnumerable)
              {
                context.AppendToCurrentToken(formulaChar);
                context.ToggleIsInString();
                continue;
              }
              if (context.LastToken != null && context.LastToken.TokenType == TokenType.String && !context.CurrentTokenHasValue)
                context.AddToken(new Token(string.Empty, TokenType.StringContent));
              context.ToggleIsInString();
            }
            if (context.CurrentTokenHasValue)
            {
              context.AddToken(this.CreateToken(context));
              if (token1.TokenType == TokenType.OpeningParenthesis && (context.LastToken.TokenType == TokenType.ExcelAddress || context.LastToken.TokenType == TokenType.NameValue))
                context.LastToken.TokenType = TokenType.Function;
            }
            if (token1.Value == "-" && SourceCodeTokenizer.TokenIsNegator(context))
            {
              context.AddToken(new Token("-", TokenType.Negator));
            }
            else
            {
              context.AddToken(token1);
              context.NewToken();
            }
          }
        }
        else
          context.AppendToCurrentToken(formulaChar);
      }
      if (context.CurrentTokenHasValue)
        context.AddToken(this.CreateToken(context));
      SourceCodeTokenizer.CleanupTokens(context);
      return (IEnumerable<Token>) context.Result;
    }

    private void FixOperators(TokenizerContext context)
    {
    }

    private static void CleanupTokens(TokenizerContext context)
    {
      for (int index = 0; index < context.Result.Count; ++index)
      {
        Token token1 = context.Result[index];
        if (token1.TokenType == TokenType.Unrecognized)
          token1.TokenType = index >= context.Result.Count - 1 ? TokenType.NameValue : (context.Result[index + 1].TokenType != TokenType.OpeningParenthesis ? TokenType.NameValue : TokenType.Function);
        else if ((token1.TokenType == TokenType.Operator || token1.TokenType == TokenType.Negator) && index < context.Result.Count - 1 && (token1.Value == "+" || token1.Value == "-"))
        {
          if (index > 0 && token1.Value == "+" && context.Result[index - 1].TokenType == TokenType.OpeningParenthesis)
          {
            context.Result.RemoveAt(index);
            SourceCodeTokenizer.SetNegatorOperator(context, index);
            --index;
          }
          else
          {
            Token token2 = context.Result[index + 1];
            if (token2.TokenType == TokenType.Operator || token2.TokenType == TokenType.Negator)
            {
              if (token1.Value == "+" && (token2.Value == "+" || token2.Value == "-"))
              {
                context.Result.RemoveAt(index);
                SourceCodeTokenizer.SetNegatorOperator(context, index);
                --index;
              }
              else if (token1.Value == "-" && token2.Value == "+")
              {
                context.Result.RemoveAt(index + 1);
                SourceCodeTokenizer.SetNegatorOperator(context, index);
                --index;
              }
              else if (token1.Value == "-" && token2.Value == "-")
              {
                context.Result.RemoveAt(index);
                if (index == 0)
                {
                  context.Result.RemoveAt(index + 1);
                  index += 2;
                }
                else
                {
                  context.Result[index].TokenType = TokenType.Operator;
                  context.Result[index].Value = "+";
                  SourceCodeTokenizer.SetNegatorOperator(context, index);
                  --index;
                }
              }
            }
          }
        }
      }
    }

    private static void SetNegatorOperator(TokenizerContext context, int i)
    {
      if (!(context.Result[i].Value == "-") || i <= 0 || context.Result[i].TokenType != TokenType.Operator && context.Result[i].TokenType != TokenType.Negator)
        return;
      if (SourceCodeTokenizer.TokenIsNegator(context.Result[i - 1]))
        context.Result[i].TokenType = TokenType.Negator;
      else
        context.Result[i].TokenType = TokenType.Operator;
    }

    private static bool TokenIsNegator(TokenizerContext context)
    {
      return SourceCodeTokenizer.TokenIsNegator(context.LastToken);
    }

    private static bool TokenIsNegator(Token t)
    {
      return t == null || t.TokenType == TokenType.Operator || t.TokenType == TokenType.OpeningParenthesis || t.TokenType == TokenType.Comma || t.TokenType == TokenType.SemiColon || t.TokenType == TokenType.OpeningEnumerable;
    }

    private bool IsPartOfMultipleCharSeparator(TokenizerContext context, char c)
    {
      return this._tokenProvider.IsOperator(context.LastToken != null ? context.LastToken.Value : string.Empty) && this._tokenProvider.IsPossibleLastPartOfMultipleCharOperator(c.ToString((IFormatProvider) CultureInfo.InvariantCulture)) && !context.CurrentTokenHasValue;
    }

    private Token CreateToken(TokenizerContext context)
    {
      return context.CurrentToken == "-" && context.LastToken == null && context.LastToken.TokenType == TokenType.Operator ? new Token("-", TokenType.Negator) : this._tokenFactory.Create((IEnumerable<Token>) context.Result, context.CurrentToken);
    }

    private bool CharIsTokenSeparator(char c, out Token token)
    {
      bool flag = this._tokenProvider.Tokens.ContainsKey(c.ToString());
      token = flag ? (token = this._tokenProvider.Tokens[c.ToString()]) : (Token) null;
      return flag;
    }
  }
}

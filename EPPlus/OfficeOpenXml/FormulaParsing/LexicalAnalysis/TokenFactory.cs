// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.LexicalAnalysis.TokenFactory
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.Excel.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.LexicalAnalysis
{
  public class TokenFactory : ITokenFactory
  {
    private readonly ITokenSeparatorProvider _tokenSeparatorProvider;
    private readonly IFunctionNameProvider _functionNameProvider;
    private readonly INameValueProvider _nameValueProvider;

    public TokenFactory(
      IFunctionNameProvider functionRepository,
      INameValueProvider nameValueProvider)
      : this((ITokenSeparatorProvider) new TokenSeparatorProvider(), nameValueProvider, functionRepository)
    {
    }

    public TokenFactory(
      ITokenSeparatorProvider tokenSeparatorProvider,
      INameValueProvider nameValueProvider,
      IFunctionNameProvider functionNameProvider)
    {
      this._tokenSeparatorProvider = tokenSeparatorProvider;
      this._functionNameProvider = functionNameProvider;
      this._nameValueProvider = nameValueProvider;
    }

    public Token Create(IEnumerable<Token> tokens, string token)
    {
      Token token1 = (Token) null;
      if (this._tokenSeparatorProvider.Tokens.TryGetValue(token, out token1))
        return token1;
      IList<Token> tokenList = (IList<Token>) tokens;
      if (token.StartsWith("!") && tokenList[tokenList.Count - 1].TokenType == TokenType.String)
      {
        int index = tokenList.Count - 2;
        if (index <= 0)
          throw new ArgumentException(string.Format("Invalid formula token sequence near {0}", (object) token));
        if (tokenList[index].TokenType != TokenType.StringContent)
          throw new ArgumentException(string.Format("Invalid formula token sequence near {0}", (object) token));
        string str = "'" + tokenList[index].Value.Replace("'", "''") + "'";
        tokenList.RemoveAt(tokenList.Count - 1);
        tokenList.RemoveAt(tokenList.Count - 1);
        tokenList.RemoveAt(tokenList.Count - 1);
        return new Token(str + token, TokenType.ExcelAddress);
      }
      if (tokens.Any<Token>() && tokens.Last<Token>().TokenType == TokenType.String)
        return new Token(token, TokenType.StringContent);
      if (!string.IsNullOrEmpty(token))
        token = token.Trim();
      if (Regex.IsMatch(token, "^[0-9]+\\.[0-9]+$"))
        return new Token(token, TokenType.Decimal);
      if (Regex.IsMatch(token, "^[0-9]+$"))
        return new Token(token, TokenType.Integer);
      if (Regex.IsMatch(token, "^(true|false)$", RegexOptions.IgnoreCase))
        return new Token(token, TokenType.Boolean);
      if (token.ToUpper().Contains("#REF!"))
        return new Token(token, TokenType.InvalidReference);
      if (token.ToUpper() == "#NUM!")
        return new Token(token, TokenType.NumericError);
      if (token.ToUpper() == "#VALUE!")
        return new Token(token, TokenType.ValueDataTypeError);
      if (token.ToUpper() == "#NULL!")
        return new Token(token, TokenType.Null);
      if (this._nameValueProvider != null && this._nameValueProvider.IsNamedValue(token))
        return new Token(token, TokenType.NameValue);
      if (this._functionNameProvider.IsFunctionName(token))
        return new Token(token, TokenType.Function);
      if (tokenList.Count > 0 && tokenList[tokenList.Count - 1].TokenType == TokenType.OpeningEnumerable)
        return new Token(token, TokenType.Enumerable);
      return ExcelAddressBase.IsValid(token) == ExcelAddressBase.AddressType.InternalAddress ? new Token(token.ToUpper(), TokenType.ExcelAddress) : new Token(token, TokenType.Unrecognized);
    }
  }
}

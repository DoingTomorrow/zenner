// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.LexicalAnalysis.TokenizerContext
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace OfficeOpenXml.FormulaParsing.LexicalAnalysis
{
  public class TokenizerContext
  {
    private char[] _chars;
    private List<Token> _result;
    private StringBuilder _currentToken;

    public TokenizerContext(string formula)
    {
      if (!string.IsNullOrEmpty(formula))
        this._chars = formula.ToArray<char>();
      this._result = new List<Token>();
      this._currentToken = new StringBuilder();
    }

    public char[] FormulaChars => this._chars;

    public IList<Token> Result => (IList<Token>) this._result;

    public bool IsInString { get; private set; }

    public void ToggleIsInString() => this.IsInString = !this.IsInString;

    internal int BracketCount { get; set; }

    public string CurrentToken => this._currentToken.ToString();

    public bool CurrentTokenHasValue => !string.IsNullOrEmpty(this.CurrentToken.Trim());

    public void NewToken() => this._currentToken = new StringBuilder();

    public void AddToken(Token token) => this._result.Add(token);

    public void AppendToCurrentToken(char c) => this._currentToken.Append(c.ToString());

    public void AppendToLastToken(string stringToAppend)
    {
      this._result.Last<Token>().Append(stringToAppend);
    }

    public void ReplaceLastToken(Token newToken)
    {
      if (this._result.Count > 0)
        this._result.RemoveAt(this._result.Count - 1);
      this._result.Add(newToken);
    }

    public Token LastToken => this._result.Count <= 0 ? (Token) null : this._result.Last<Token>();
  }
}

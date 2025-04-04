// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.LexicalAnalysis.Token
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.FormulaParsing.LexicalAnalysis
{
  public class Token
  {
    public Token(string token, TokenType tokenType)
    {
      this.Value = token;
      this.TokenType = tokenType;
    }

    public string Value { get; internal set; }

    public TokenType TokenType { get; internal set; }

    public void Append(string stringToAppend) => this.Value += stringToAppend;

    public bool IsNegated { get; private set; }

    public void Negate()
    {
      if (this.TokenType != TokenType.Decimal && this.TokenType != TokenType.Integer && this.TokenType != TokenType.ExcelAddress)
        return;
      this.IsNegated = true;
    }

    public override string ToString() => this.TokenType.ToString() + ", " + this.Value;
  }
}

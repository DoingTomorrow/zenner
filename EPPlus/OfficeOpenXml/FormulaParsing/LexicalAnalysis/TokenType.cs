// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.LexicalAnalysis.TokenType
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace OfficeOpenXml.FormulaParsing.LexicalAnalysis
{
  public enum TokenType
  {
    Operator,
    Negator,
    OpeningParenthesis,
    ClosingParenthesis,
    OpeningEnumerable,
    ClosingEnumerable,
    OpeningBracket,
    ClosingBracket,
    Enumerable,
    Comma,
    SemiColon,
    String,
    StringContent,
    Integer,
    Boolean,
    Decimal,
    Percent,
    Function,
    ExcelAddress,
    NameValue,
    InvalidReference,
    NumericError,
    ValueDataTypeError,
    Null,
    Unrecognized,
  }
}

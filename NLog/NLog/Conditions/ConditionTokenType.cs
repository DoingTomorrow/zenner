// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionTokenType
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

#nullable disable
namespace NLog.Conditions
{
  internal enum ConditionTokenType
  {
    EndOfInput,
    BeginningOfInput,
    Number,
    String,
    Keyword,
    Whitespace,
    FirstPunct,
    LessThan,
    GreaterThan,
    LessThanOrEqualTo,
    GreaterThanOrEqualTo,
    EqualTo,
    NotEqual,
    LeftParen,
    RightParen,
    Dot,
    Comma,
    Not,
    And,
    Or,
    Minus,
    LastPunct,
    Invalid,
    ClosingCurlyBrace,
    Colon,
    Exclamation,
    Ampersand,
    Pipe,
  }
}

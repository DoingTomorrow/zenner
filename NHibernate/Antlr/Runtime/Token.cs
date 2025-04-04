// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Token
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace Antlr.Runtime
{
  internal static class Token
  {
    public const int EOR_TOKEN_TYPE = 1;
    public const int DOWN = 2;
    public const int UP = 3;
    public const int INVALID_TOKEN_TYPE = 0;
    public const int DEFAULT_CHANNEL = 0;
    public const int HIDDEN_CHANNEL = 99;
    public static readonly int MIN_TOKEN_TYPE = 4;
    public static readonly int EOF = -1;
    public static readonly IToken EOF_TOKEN = (IToken) new CommonToken(Token.EOF);
    public static readonly IToken INVALID_TOKEN = (IToken) new CommonToken(0);
    public static readonly IToken SKIP_TOKEN = (IToken) new CommonToken(0);
  }
}

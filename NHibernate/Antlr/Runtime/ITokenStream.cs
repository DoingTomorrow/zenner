// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.ITokenStream
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace Antlr.Runtime
{
  internal interface ITokenStream : IIntStream
  {
    IToken LT(int k);

    IToken Get(int i);

    ITokenSource TokenSource { get; }

    string ToString(int start, int stop);

    string ToString(IToken start, IToken stop);
  }
}

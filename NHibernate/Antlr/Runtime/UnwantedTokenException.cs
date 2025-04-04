// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.UnwantedTokenException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace Antlr.Runtime
{
  internal class UnwantedTokenException : MismatchedTokenException
  {
    public UnwantedTokenException()
    {
    }

    public UnwantedTokenException(int expecting, IIntStream input)
      : base(expecting, input)
    {
    }

    public IToken UnexpectedToken => this.token;

    public override string ToString()
    {
      string str = ", expected " + (object) this.Expecting;
      if (this.Expecting == 0)
        str = string.Empty;
      return this.token == null ? "UnwantedTokenException(found=" + (string) null + str + ")" : "UnwantedTokenException(found=" + this.token.Text + str + ")";
    }
  }
}

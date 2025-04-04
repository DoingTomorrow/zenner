// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.MismatchedTokenException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  internal class MismatchedTokenException : RecognitionException
  {
    private int expecting;

    public MismatchedTokenException()
    {
    }

    public MismatchedTokenException(int expecting, IIntStream input)
      : base(input)
    {
      this.expecting = expecting;
    }

    public int Expecting
    {
      get => this.expecting;
      set => this.expecting = value;
    }

    public override string ToString()
    {
      return "MismatchedTokenException(" + (object) this.UnexpectedType + "!=" + (object) this.expecting + ")";
    }
  }
}

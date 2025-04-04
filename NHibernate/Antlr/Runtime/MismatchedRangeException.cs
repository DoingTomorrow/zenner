// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.MismatchedRangeException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  internal class MismatchedRangeException : RecognitionException
  {
    private int a;
    private int b;

    public MismatchedRangeException()
    {
    }

    public MismatchedRangeException(int a, int b, IIntStream input)
      : base(input)
    {
      this.a = a;
      this.b = b;
    }

    public int A
    {
      get => this.a;
      set => this.a = value;
    }

    public int B
    {
      get => this.b;
      set => this.b = value;
    }

    public override string ToString()
    {
      return "MismatchedNotSetException(" + (object) this.UnexpectedType + " not in [" + (object) this.a + "," + (object) this.b + "])";
    }
  }
}

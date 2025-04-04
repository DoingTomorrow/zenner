// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.MismatchedNotSetException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  internal class MismatchedNotSetException : MismatchedSetException
  {
    public MismatchedNotSetException()
    {
    }

    public MismatchedNotSetException(BitSet expecting, IIntStream input)
      : base(expecting, input)
    {
    }

    public override string ToString()
    {
      return "MismatchedNotSetException(" + (object) this.UnexpectedType + "!=" + (object) this.expecting + ")";
    }
  }
}

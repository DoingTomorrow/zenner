// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.MismatchedTreeNodeException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime.Tree;
using System;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  internal class MismatchedTreeNodeException : RecognitionException
  {
    public int expecting;

    public MismatchedTreeNodeException()
    {
    }

    public MismatchedTreeNodeException(int expecting, ITreeNodeStream input)
      : base((IIntStream) input)
    {
      this.expecting = expecting;
    }

    public override string ToString()
    {
      return "MismatchedTreeNodeException(" + (object) this.UnexpectedType + "!=" + (object) this.expecting + ")";
    }
  }
}

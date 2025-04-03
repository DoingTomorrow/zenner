// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.MismatchedTreeNodeException
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using Antlr.Runtime.Tree;
using System;
using System.Runtime.Serialization;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  public class MismatchedTreeNodeException : RecognitionException
  {
    private readonly int _expecting;

    public MismatchedTreeNodeException()
    {
    }

    public MismatchedTreeNodeException(string message)
      : base(message)
    {
    }

    public MismatchedTreeNodeException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public MismatchedTreeNodeException(int expecting, ITreeNodeStream input)
      : base((IIntStream) input)
    {
      this._expecting = expecting;
    }

    public MismatchedTreeNodeException(string message, int expecting, ITreeNodeStream input)
      : base(message, (IIntStream) input)
    {
      this._expecting = expecting;
    }

    public MismatchedTreeNodeException(
      string message,
      int expecting,
      ITreeNodeStream input,
      Exception innerException)
      : base(message, (IIntStream) input, innerException)
    {
      this._expecting = expecting;
    }

    protected MismatchedTreeNodeException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this._expecting = info != null ? info.GetInt32(nameof (Expecting)) : throw new ArgumentNullException(nameof (info));
    }

    public int Expecting => this._expecting;

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException(nameof (info));
      base.GetObjectData(info, context);
      info.AddValue("Expecting", this._expecting);
    }

    public override string ToString()
    {
      return "MismatchedTreeNodeException(" + (object) this.UnexpectedType + "!=" + (object) this.Expecting + ")";
    }
  }
}

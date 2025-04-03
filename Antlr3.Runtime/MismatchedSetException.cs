// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.MismatchedSetException
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  public class MismatchedSetException : RecognitionException
  {
    private readonly BitSet _expecting;

    public MismatchedSetException()
    {
    }

    public MismatchedSetException(string message)
      : base(message)
    {
    }

    public MismatchedSetException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public MismatchedSetException(BitSet expecting, IIntStream input)
      : base(input)
    {
      this._expecting = expecting;
    }

    public MismatchedSetException(string message, BitSet expecting, IIntStream input)
      : base(message, input)
    {
      this._expecting = expecting;
    }

    public MismatchedSetException(
      string message,
      BitSet expecting,
      IIntStream input,
      Exception innerException)
      : base(message, input, innerException)
    {
      this._expecting = expecting;
    }

    protected MismatchedSetException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this._expecting = info != null ? (BitSet) info.GetValue(nameof (Expecting), typeof (BitSet)) : throw new ArgumentNullException(nameof (info));
    }

    public BitSet Expecting => this._expecting;

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException(nameof (info));
      base.GetObjectData(info, context);
      info.AddValue("Expecting", (object) this._expecting);
    }

    public override string ToString()
    {
      return "MismatchedSetException(" + (object) this.UnexpectedType + "!=" + (object) this.Expecting + ")";
    }
  }
}

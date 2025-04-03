// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.MismatchedNotSetException
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  public class MismatchedNotSetException : MismatchedSetException
  {
    public MismatchedNotSetException()
    {
    }

    public MismatchedNotSetException(string message)
      : base(message)
    {
    }

    public MismatchedNotSetException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public MismatchedNotSetException(BitSet expecting, IIntStream input)
      : base(expecting, input)
    {
    }

    public MismatchedNotSetException(string message, BitSet expecting, IIntStream input)
      : base(message, expecting, input)
    {
    }

    public MismatchedNotSetException(
      string message,
      BitSet expecting,
      IIntStream input,
      Exception innerException)
      : base(message, expecting, input, innerException)
    {
    }

    protected MismatchedNotSetException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public override string ToString()
    {
      return "MismatchedNotSetException(" + (object) this.UnexpectedType + "!=" + (object) this.Expecting + ")";
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.MissingTokenException
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  public class MissingTokenException : MismatchedTokenException
  {
    private readonly object _inserted;

    public MissingTokenException()
    {
    }

    public MissingTokenException(string message)
      : base(message)
    {
    }

    public MissingTokenException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public MissingTokenException(int expecting, IIntStream input, object inserted)
      : this(expecting, input, inserted, (IList<string>) null)
    {
    }

    public MissingTokenException(
      int expecting,
      IIntStream input,
      object inserted,
      IList<string> tokenNames)
      : base(expecting, input, tokenNames)
    {
      this._inserted = inserted;
    }

    public MissingTokenException(
      string message,
      int expecting,
      IIntStream input,
      object inserted,
      IList<string> tokenNames)
      : base(message, expecting, input, tokenNames)
    {
      this._inserted = inserted;
    }

    public MissingTokenException(
      string message,
      int expecting,
      IIntStream input,
      object inserted,
      IList<string> tokenNames,
      Exception innerException)
      : base(message, expecting, input, tokenNames, innerException)
    {
      this._inserted = inserted;
    }

    protected MissingTokenException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public virtual int MissingType => this.Expecting;

    public override string ToString()
    {
      return this._inserted != null && this.Token != null ? "MissingTokenException(inserted " + this._inserted + " at " + this.Token.Text + ")" : (this.Token != null ? "MissingTokenException(at " + this.Token.Text + ")" : nameof (MissingTokenException));
    }
  }
}

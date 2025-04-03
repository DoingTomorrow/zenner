// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.MismatchedRangeException
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  public class MismatchedRangeException : RecognitionException
  {
    private readonly int _a;
    private readonly int _b;

    public MismatchedRangeException()
    {
    }

    public MismatchedRangeException(string message)
      : base(message)
    {
    }

    public MismatchedRangeException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public MismatchedRangeException(int a, int b, IIntStream input)
      : base(input)
    {
      this._a = a;
      this._b = b;
    }

    public MismatchedRangeException(string message, int a, int b, IIntStream input)
      : base(message, input)
    {
      this._a = a;
      this._b = b;
    }

    public MismatchedRangeException(
      string message,
      int a,
      int b,
      IIntStream input,
      Exception innerException)
      : base(message, input, innerException)
    {
      this._a = a;
      this._b = b;
    }

    protected MismatchedRangeException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this._a = info != null ? info.GetInt32(nameof (A)) : throw new ArgumentNullException(nameof (info));
      this._b = info.GetInt32(nameof (B));
    }

    public int A => this._a;

    public int B => this._b;

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException(nameof (info));
      base.GetObjectData(info, context);
      info.AddValue("A", this._a);
      info.AddValue("B", this._b);
    }

    public override string ToString()
    {
      return "MismatchedRangeException(" + (object) this.UnexpectedType + " not in [" + (object) this.A + "," + (object) this.B + "])";
    }
  }
}

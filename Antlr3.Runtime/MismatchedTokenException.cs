// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.MismatchedTokenException
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  public class MismatchedTokenException : RecognitionException
  {
    private readonly int _expecting;
    private readonly ReadOnlyCollection<string> _tokenNames;

    public MismatchedTokenException()
    {
    }

    public MismatchedTokenException(string message)
      : base(message)
    {
    }

    public MismatchedTokenException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public MismatchedTokenException(int expecting, IIntStream input)
      : this(expecting, input, (IList<string>) null)
    {
    }

    public MismatchedTokenException(int expecting, IIntStream input, IList<string> tokenNames)
      : base(input)
    {
      this._expecting = expecting;
      if (tokenNames == null)
        return;
      this._tokenNames = new List<string>((IEnumerable<string>) tokenNames).AsReadOnly();
    }

    public MismatchedTokenException(
      string message,
      int expecting,
      IIntStream input,
      IList<string> tokenNames)
      : base(message, input)
    {
      this._expecting = expecting;
      if (tokenNames == null)
        return;
      this._tokenNames = new List<string>((IEnumerable<string>) tokenNames).AsReadOnly();
    }

    public MismatchedTokenException(
      string message,
      int expecting,
      IIntStream input,
      IList<string> tokenNames,
      Exception innerException)
      : base(message, input, innerException)
    {
      this._expecting = expecting;
      if (tokenNames == null)
        return;
      this._tokenNames = new List<string>((IEnumerable<string>) tokenNames).AsReadOnly();
    }

    protected MismatchedTokenException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this._expecting = info != null ? info.GetInt32(nameof (Expecting)) : throw new ArgumentNullException(nameof (info));
      this._tokenNames = new ReadOnlyCollection<string>((IList<string>) (string[]) info.GetValue(nameof (TokenNames), typeof (string[])));
    }

    public int Expecting => this._expecting;

    public ReadOnlyCollection<string> TokenNames => this._tokenNames;

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException(nameof (info));
      base.GetObjectData(info, context);
      info.AddValue("Expecting", this._expecting);
      info.AddValue("TokenNames", this._tokenNames != null ? (object) new List<string>((IEnumerable<string>) this._tokenNames).ToArray() : (object) (string[]) null);
    }

    public override string ToString()
    {
      int unexpectedType = this.UnexpectedType;
      return "MismatchedTokenException(" + (this.TokenNames == null || unexpectedType < 0 || unexpectedType >= this.TokenNames.Count ? unexpectedType.ToString() : this.TokenNames[unexpectedType]) + "!=" + (this.TokenNames == null || this.Expecting < 0 || this.Expecting >= this.TokenNames.Count ? this.Expecting.ToString() : this.TokenNames[this.Expecting]) + ")";
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.UnwantedTokenException
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
  public class UnwantedTokenException : MismatchedTokenException
  {
    public UnwantedTokenException()
    {
    }

    public UnwantedTokenException(string message)
      : base(message)
    {
    }

    public UnwantedTokenException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public UnwantedTokenException(int expecting, IIntStream input)
      : base(expecting, input)
    {
    }

    public UnwantedTokenException(int expecting, IIntStream input, IList<string> tokenNames)
      : base(expecting, input, tokenNames)
    {
    }

    public UnwantedTokenException(
      string message,
      int expecting,
      IIntStream input,
      IList<string> tokenNames)
      : base(message, expecting, input, tokenNames)
    {
    }

    public UnwantedTokenException(
      string message,
      int expecting,
      IIntStream input,
      IList<string> tokenNames,
      Exception innerException)
      : base(message, expecting, input, tokenNames, innerException)
    {
    }

    protected UnwantedTokenException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    public virtual IToken UnexpectedToken => this.Token;

    public override string ToString()
    {
      string str = ", expected " + (this.TokenNames == null || this.Expecting < 0 || this.Expecting >= this.TokenNames.Count ? this.Expecting.ToString() : this.TokenNames[this.Expecting]);
      if (this.Expecting == 0)
        str = "";
      return this.Token == null ? "UnwantedTokenException(found=" + str + ")" : "UnwantedTokenException(found=" + this.Token.Text + str + ")";
    }
  }
}

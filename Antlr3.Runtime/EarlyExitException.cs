// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.EarlyExitException
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  public class EarlyExitException : RecognitionException
  {
    private readonly int _decisionNumber;

    public EarlyExitException()
    {
    }

    public EarlyExitException(string message)
      : base(message)
    {
    }

    public EarlyExitException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public EarlyExitException(int decisionNumber, IIntStream input)
      : base(input)
    {
      this._decisionNumber = decisionNumber;
    }

    public EarlyExitException(string message, int decisionNumber, IIntStream input)
      : base(message, input)
    {
      this._decisionNumber = decisionNumber;
    }

    public EarlyExitException(
      string message,
      int decisionNumber,
      IIntStream input,
      Exception innerException)
      : base(message, input, innerException)
    {
      this._decisionNumber = decisionNumber;
    }

    protected EarlyExitException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this._decisionNumber = info != null ? info.GetInt32(nameof (DecisionNumber)) : throw new ArgumentNullException(nameof (info));
    }

    public int DecisionNumber => this._decisionNumber;

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException(nameof (info));
      base.GetObjectData(info, context);
      info.AddValue("DecisionNumber", this.DecisionNumber);
    }
  }
}

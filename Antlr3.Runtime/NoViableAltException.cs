// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.NoViableAltException
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  public class NoViableAltException : RecognitionException
  {
    private readonly string _grammarDecisionDescription;
    private readonly int _decisionNumber;
    private readonly int _stateNumber;

    public NoViableAltException()
    {
    }

    public NoViableAltException(string grammarDecisionDescription)
    {
      this._grammarDecisionDescription = grammarDecisionDescription;
    }

    public NoViableAltException(string message, string grammarDecisionDescription)
      : base(message)
    {
      this._grammarDecisionDescription = grammarDecisionDescription;
    }

    public NoViableAltException(
      string message,
      string grammarDecisionDescription,
      Exception innerException)
      : base(message, innerException)
    {
      this._grammarDecisionDescription = grammarDecisionDescription;
    }

    public NoViableAltException(
      string grammarDecisionDescription,
      int decisionNumber,
      int stateNumber,
      IIntStream input)
      : this(grammarDecisionDescription, decisionNumber, stateNumber, input, 1)
    {
    }

    public NoViableAltException(
      string grammarDecisionDescription,
      int decisionNumber,
      int stateNumber,
      IIntStream input,
      int k)
      : base(input, k)
    {
      this._grammarDecisionDescription = grammarDecisionDescription;
      this._decisionNumber = decisionNumber;
      this._stateNumber = stateNumber;
    }

    public NoViableAltException(
      string message,
      string grammarDecisionDescription,
      int decisionNumber,
      int stateNumber,
      IIntStream input)
      : this(message, grammarDecisionDescription, decisionNumber, stateNumber, input, 1)
    {
    }

    public NoViableAltException(
      string message,
      string grammarDecisionDescription,
      int decisionNumber,
      int stateNumber,
      IIntStream input,
      int k)
      : base(message, input, k)
    {
      this._grammarDecisionDescription = grammarDecisionDescription;
      this._decisionNumber = decisionNumber;
      this._stateNumber = stateNumber;
    }

    public NoViableAltException(
      string message,
      string grammarDecisionDescription,
      int decisionNumber,
      int stateNumber,
      IIntStream input,
      Exception innerException)
      : this(message, grammarDecisionDescription, decisionNumber, stateNumber, input, 1, innerException)
    {
    }

    public NoViableAltException(
      string message,
      string grammarDecisionDescription,
      int decisionNumber,
      int stateNumber,
      IIntStream input,
      int k,
      Exception innerException)
      : base(message, input, k, innerException)
    {
      this._grammarDecisionDescription = grammarDecisionDescription;
      this._decisionNumber = decisionNumber;
      this._stateNumber = stateNumber;
    }

    protected NoViableAltException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this._grammarDecisionDescription = info != null ? info.GetString(nameof (GrammarDecisionDescription)) : throw new ArgumentNullException(nameof (info));
      this._decisionNumber = info.GetInt32(nameof (DecisionNumber));
      this._stateNumber = info.GetInt32(nameof (StateNumber));
    }

    public int DecisionNumber => this._decisionNumber;

    public string GrammarDecisionDescription => this._grammarDecisionDescription;

    public int StateNumber => this._stateNumber;

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException(nameof (info));
      base.GetObjectData(info, context);
      info.AddValue("GrammarDecisionDescription", (object) this._grammarDecisionDescription);
      info.AddValue("DecisionNumber", this._decisionNumber);
      info.AddValue("StateNumber", this._stateNumber);
    }

    public override string ToString()
    {
      return this.Input is ICharStream ? "NoViableAltException('" + (object) (char) this.UnexpectedType + "'@[" + this.GrammarDecisionDescription + "])" : "NoViableAltException(" + (object) this.UnexpectedType + "@[" + this.GrammarDecisionDescription + "])";
    }
  }
}

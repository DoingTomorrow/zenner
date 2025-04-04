// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.NoViableAltException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  internal class NoViableAltException : RecognitionException
  {
    public string grammarDecisionDescription;
    public int decisionNumber;
    public int stateNumber;

    public NoViableAltException()
    {
    }

    public NoViableAltException(
      string grammarDecisionDescription,
      int decisionNumber,
      int stateNumber,
      IIntStream input)
      : base(input)
    {
      this.grammarDecisionDescription = grammarDecisionDescription;
      this.decisionNumber = decisionNumber;
      this.stateNumber = stateNumber;
    }

    public override string ToString()
    {
      return this.input is ICharStream ? "NoViableAltException('" + (object) (char) this.UnexpectedType + "'@[" + this.grammarDecisionDescription + "])" : "NoViableAltException(" + (object) this.UnexpectedType + "@[" + this.grammarDecisionDescription + "])";
    }
  }
}

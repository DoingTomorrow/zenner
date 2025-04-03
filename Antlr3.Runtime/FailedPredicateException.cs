// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.FailedPredicateException
// Assembly: Antlr3.Runtime, Version=3.4.1.9004, Culture=neutral, PublicKeyToken=eb42632606e9261f
// MVID: 770B825D-AB58-454E-B162-B363E6A4CCD6
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Antlr3.Runtime.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Antlr.Runtime
{
  [Serializable]
  public class FailedPredicateException : RecognitionException
  {
    private readonly string _ruleName;
    private readonly string _predicateText;

    public FailedPredicateException()
    {
    }

    public FailedPredicateException(string message)
      : base(message)
    {
    }

    public FailedPredicateException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    public FailedPredicateException(IIntStream input, string ruleName, string predicateText)
      : base(input)
    {
      this._ruleName = ruleName;
      this._predicateText = predicateText;
    }

    public FailedPredicateException(
      string message,
      IIntStream input,
      string ruleName,
      string predicateText)
      : base(message, input)
    {
      this._ruleName = ruleName;
      this._predicateText = predicateText;
    }

    public FailedPredicateException(
      string message,
      IIntStream input,
      string ruleName,
      string predicateText,
      Exception innerException)
      : base(message, input, innerException)
    {
      this._ruleName = ruleName;
      this._predicateText = predicateText;
    }

    protected FailedPredicateException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this._ruleName = info != null ? info.GetString(nameof (RuleName)) : throw new ArgumentNullException(nameof (info));
      this._predicateText = info.GetString(nameof (PredicateText));
    }

    public string RuleName => this._ruleName;

    public string PredicateText => this._predicateText;

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        throw new ArgumentNullException(nameof (info));
      base.GetObjectData(info, context);
      info.AddValue("RuleName", (object) this._ruleName);
      info.AddValue("PredicateText", (object) this._predicateText);
    }

    public override string ToString()
    {
      return "FailedPredicateException(" + this.RuleName + ",{" + this.PredicateText + "}?)";
    }
  }
}

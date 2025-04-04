// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionOrExpression
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

#nullable disable
namespace NLog.Conditions
{
  internal sealed class ConditionOrExpression : ConditionExpression
  {
    private static readonly object boxedFalse = (object) false;
    private static readonly object boxedTrue = (object) true;

    public ConditionOrExpression(ConditionExpression left, ConditionExpression right)
    {
      this.LeftExpression = left;
      this.RightExpression = right;
    }

    public ConditionExpression LeftExpression { get; private set; }

    public ConditionExpression RightExpression { get; private set; }

    public override string ToString()
    {
      return string.Format("({0} or {1})", (object) this.LeftExpression, (object) this.RightExpression);
    }

    protected override object EvaluateNode(LogEventInfo context)
    {
      return (bool) this.LeftExpression.Evaluate(context) || (bool) this.RightExpression.Evaluate(context) ? ConditionOrExpression.boxedTrue : ConditionOrExpression.boxedFalse;
    }
  }
}

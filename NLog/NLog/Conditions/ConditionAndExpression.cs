// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionAndExpression
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

#nullable disable
namespace NLog.Conditions
{
  internal sealed class ConditionAndExpression : ConditionExpression
  {
    private static readonly object boxedFalse = (object) false;
    private static readonly object boxedTrue = (object) true;

    public ConditionAndExpression(ConditionExpression left, ConditionExpression right)
    {
      this.Left = left;
      this.Right = right;
    }

    public ConditionExpression Left { get; private set; }

    public ConditionExpression Right { get; private set; }

    public override string ToString()
    {
      return string.Format("({0} and {1})", (object) this.Left, (object) this.Right);
    }

    protected override object EvaluateNode(LogEventInfo context)
    {
      return !(bool) this.Left.Evaluate(context) || !(bool) this.Right.Evaluate(context) ? ConditionAndExpression.boxedFalse : ConditionAndExpression.boxedTrue;
    }
  }
}

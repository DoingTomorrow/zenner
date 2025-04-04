// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionMessageExpression
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

#nullable disable
namespace NLog.Conditions
{
  internal sealed class ConditionMessageExpression : ConditionExpression
  {
    public override string ToString() => "message";

    protected override object EvaluateNode(LogEventInfo context)
    {
      return (object) context.FormattedMessage;
    }
  }
}

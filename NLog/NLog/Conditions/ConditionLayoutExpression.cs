// Decompiled with JetBrains decompiler
// Type: NLog.Conditions.ConditionLayoutExpression
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Layouts;

#nullable disable
namespace NLog.Conditions
{
  internal sealed class ConditionLayoutExpression : ConditionExpression
  {
    public ConditionLayoutExpression(Layout layout) => this.Layout = layout;

    public Layout Layout { get; private set; }

    public override string ToString() => this.Layout.ToString();

    protected override object EvaluateNode(LogEventInfo context)
    {
      return (object) this.Layout.Render(context);
    }
  }
}

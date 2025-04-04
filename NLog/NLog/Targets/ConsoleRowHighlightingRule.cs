// Decompiled with JetBrains decompiler
// Type: NLog.Targets.ConsoleRowHighlightingRule
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Conditions;
using NLog.Config;
using System.ComponentModel;

#nullable disable
namespace NLog.Targets
{
  [NLogConfigurationItem]
  public class ConsoleRowHighlightingRule
  {
    static ConsoleRowHighlightingRule()
    {
      ConsoleRowHighlightingRule.Default = new ConsoleRowHighlightingRule((ConditionExpression) null, ConsoleOutputColor.NoChange, ConsoleOutputColor.NoChange);
    }

    public ConsoleRowHighlightingRule()
      : this((ConditionExpression) null, ConsoleOutputColor.NoChange, ConsoleOutputColor.NoChange)
    {
    }

    public ConsoleRowHighlightingRule(
      ConditionExpression condition,
      ConsoleOutputColor foregroundColor,
      ConsoleOutputColor backgroundColor)
    {
      this.Condition = condition;
      this.ForegroundColor = foregroundColor;
      this.BackgroundColor = backgroundColor;
    }

    public static ConsoleRowHighlightingRule Default { get; private set; }

    [RequiredParameter]
    public ConditionExpression Condition { get; set; }

    [DefaultValue("NoChange")]
    public ConsoleOutputColor ForegroundColor { get; set; }

    [DefaultValue("NoChange")]
    public ConsoleOutputColor BackgroundColor { get; set; }

    public bool CheckCondition(LogEventInfo logEvent)
    {
      return this.Condition == null || true.Equals(this.Condition.Evaluate(logEvent));
    }
  }
}

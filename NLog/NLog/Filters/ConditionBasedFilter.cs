// Decompiled with JetBrains decompiler
// Type: NLog.Filters.ConditionBasedFilter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Conditions;
using NLog.Config;

#nullable disable
namespace NLog.Filters
{
  [Filter("when")]
  public class ConditionBasedFilter : Filter
  {
    private static readonly object boxedTrue = (object) true;

    [RequiredParameter]
    public ConditionExpression Condition { get; set; }

    protected override FilterResult Check(LogEventInfo logEvent)
    {
      object obj = this.Condition.Evaluate(logEvent);
      return ConditionBasedFilter.boxedTrue.Equals(obj) ? this.Action : FilterResult.Neutral;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.FilteringRule
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Conditions;
using NLog.Config;

#nullable disable
namespace NLog.Targets.Wrappers
{
  [NLogConfigurationItem]
  public class FilteringRule
  {
    public FilteringRule()
      : this((ConditionExpression) null, (ConditionExpression) null)
    {
    }

    public FilteringRule(
      ConditionExpression whenExistsExpression,
      ConditionExpression filterToApply)
    {
      this.Exists = whenExistsExpression;
      this.Filter = filterToApply;
    }

    [RequiredParameter]
    public ConditionExpression Exists { get; set; }

    [RequiredParameter]
    public ConditionExpression Filter { get; set; }
  }
}

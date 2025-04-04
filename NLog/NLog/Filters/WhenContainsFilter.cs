// Decompiled with JetBrains decompiler
// Type: NLog.Filters.WhenContainsFilter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using System;
using System.ComponentModel;

#nullable disable
namespace NLog.Filters
{
  [Filter("whenContains")]
  public class WhenContainsFilter : LayoutBasedFilter
  {
    [DefaultValue(false)]
    public bool IgnoreCase { get; set; }

    [RequiredParameter]
    public string Substring { get; set; }

    protected override FilterResult Check(LogEventInfo logEvent)
    {
      StringComparison comparisonType = this.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
      return this.Layout.Render(logEvent).IndexOf(this.Substring, comparisonType) >= 0 ? this.Action : FilterResult.Neutral;
    }
  }
}

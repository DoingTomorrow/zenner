// Decompiled with JetBrains decompiler
// Type: NLog.Filters.Filter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;

#nullable disable
namespace NLog.Filters
{
  [NLogConfigurationItem]
  public abstract class Filter
  {
    protected Filter() => this.Action = FilterResult.Neutral;

    [RequiredParameter]
    public FilterResult Action { get; set; }

    internal FilterResult GetFilterResult(LogEventInfo logEvent) => this.Check(logEvent);

    protected abstract FilterResult Check(LogEventInfo logEvent);
  }
}

// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.EnvironmentLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Internal;
using NLog.Layouts;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("environment")]
  [ThreadSafe]
  public class EnvironmentLayoutRenderer : LayoutRenderer
  {
    private KeyValuePair<string, SimpleLayout> _cachedValue;

    [RequiredParameter]
    [DefaultParameter]
    public string Variable { get; set; }

    public string Default { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      if (this.Variable == null)
        return;
      string environmentVariable = EnvironmentHelper.GetSafeEnvironmentVariable(this.Variable);
      if (string.IsNullOrEmpty(environmentVariable))
        environmentVariable = this.Default;
      if (string.IsNullOrEmpty(environmentVariable))
        return;
      if (string.CompareOrdinal(this._cachedValue.Key, environmentVariable) != 0)
        this._cachedValue = new KeyValuePair<string, SimpleLayout>(environmentVariable, new SimpleLayout(environmentVariable));
      this._cachedValue.Value.RenderAppendBuilder(logEvent, builder);
    }
  }
}

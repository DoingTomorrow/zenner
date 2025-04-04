// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.MessageLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Internal;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("message")]
  [ThreadAgnostic]
  [ThreadSafe]
  public class MessageLayoutRenderer : LayoutRenderer
  {
    public MessageLayoutRenderer() => this.ExceptionSeparator = EnvironmentHelper.NewLine;

    public bool WithException { get; set; }

    public string ExceptionSeparator { get; set; }

    public bool Raw { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      if (this.Raw)
        builder.Append(logEvent.Message);
      else if (logEvent.MessageFormatter == LogMessageTemplateFormatter.DefaultAutoSingleTarget.MessageFormatter)
        logEvent.AppendFormattedMessage((ILogMessageFormatter) LogMessageTemplateFormatter.DefaultAutoSingleTarget, builder);
      else
        builder.Append(logEvent.FormattedMessage);
      if (!this.WithException || logEvent.Exception == null)
        return;
      builder.Append(this.ExceptionSeparator);
      builder.Append(logEvent.Exception.ToString());
    }
  }
}

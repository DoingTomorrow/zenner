// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.CallSiteFileNameLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Internal;
using System.ComponentModel;
using System.IO;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("callsite-filename")]
  [ThreadAgnostic]
  [ThreadSafe]
  public class CallSiteFileNameLayoutRenderer : LayoutRenderer, IUsesStackTrace
  {
    public CallSiteFileNameLayoutRenderer() => this.IncludeSourcePath = true;

    [DefaultValue(true)]
    public bool IncludeSourcePath { get; set; }

    [DefaultValue(0)]
    public int SkipFrames { get; set; }

    StackTraceUsage IUsesStackTrace.StackTraceUsage => StackTraceUsage.WithSource;

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      if (logEvent.CallSiteInformation == null)
        return;
      string callerFilePath = logEvent.CallSiteInformation.GetCallerFilePath(this.SkipFrames);
      if (string.IsNullOrEmpty(callerFilePath))
        return;
      if (this.IncludeSourcePath)
        builder.Append(callerFilePath);
      else
        builder.Append(Path.GetFileName(callerFilePath));
    }
  }
}

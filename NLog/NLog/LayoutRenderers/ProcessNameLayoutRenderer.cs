// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.ProcessNameLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Config;
using NLog.Internal;
using System.ComponentModel;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("processname")]
  [AppDomainFixedOutput]
  [ThreadAgnostic]
  [ThreadSafe]
  public class ProcessNameLayoutRenderer : LayoutRenderer
  {
    [DefaultValue(false)]
    public bool FullName { get; set; }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      string str = this.FullName ? ThreadIDHelper.Instance.CurrentProcessName : ThreadIDHelper.Instance.CurrentProcessBaseName;
      builder.Append(str);
    }
  }
}

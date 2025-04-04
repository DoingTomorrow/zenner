// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.MachineNameLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers
{
  [LayoutRenderer("machinename")]
  [AppDomainFixedOutput]
  [ThreadAgnostic]
  [ThreadSafe]
  public class MachineNameLayoutRenderer : LayoutRenderer
  {
    internal string MachineName { get; private set; }

    protected override void InitializeLayoutRenderer()
    {
      base.InitializeLayoutRenderer();
      try
      {
        this.MachineName = EnvironmentHelper.GetMachineName();
        if (!string.IsNullOrEmpty(this.MachineName))
          return;
        InternalLogger.Info("MachineName is not available.");
      }
      catch (Exception ex)
      {
        InternalLogger.Error(ex, "Error getting machine name.");
        if (ex.MustBeRethrown())
          throw;
        else
          this.MachineName = string.Empty;
      }
    }

    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
      builder.Append(this.MachineName);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: NLog.Config.LoggingConfigurationChangedEventArgs
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;

#nullable disable
namespace NLog.Config
{
  public class LoggingConfigurationChangedEventArgs : EventArgs
  {
    public LoggingConfigurationChangedEventArgs(
      LoggingConfiguration activatedConfiguration,
      LoggingConfiguration deactivatedConfiguration)
    {
      this.ActivatedConfiguration = activatedConfiguration;
      this.DeactivatedConfiguration = deactivatedConfiguration;
    }

    public LoggingConfiguration DeactivatedConfiguration { get; private set; }

    public LoggingConfiguration ActivatedConfiguration { get; private set; }

    [Obsolete("This option will be removed in NLog 5. Marked obsolete on NLog 4.5")]
    public LoggingConfiguration OldConfiguration => this.ActivatedConfiguration;

    [Obsolete("This option will be removed in NLog 5. Marked obsolete on NLog 4.5")]
    public LoggingConfiguration NewConfiguration => this.DeactivatedConfiguration;
  }
}

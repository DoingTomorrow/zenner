// Decompiled with JetBrains decompiler
// Type: NLog.Config.LoggingConfigurationReloadedEventArgs
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;

#nullable disable
namespace NLog.Config
{
  public class LoggingConfigurationReloadedEventArgs : EventArgs
  {
    public LoggingConfigurationReloadedEventArgs(bool succeeded) => this.Succeeded = succeeded;

    public LoggingConfigurationReloadedEventArgs(bool succeeded, Exception exception)
    {
      this.Succeeded = succeeded;
      this.Exception = exception;
    }

    public bool Succeeded { get; private set; }

    public Exception Exception { get; private set; }
  }
}

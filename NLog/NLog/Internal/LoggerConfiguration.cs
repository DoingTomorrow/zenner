// Decompiled with JetBrains decompiler
// Type: NLog.Internal.LoggerConfiguration
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;

#nullable disable
namespace NLog.Internal
{
  internal class LoggerConfiguration
  {
    private readonly TargetWithFilterChain[] _targetsByLevel;

    public LoggerConfiguration(
      TargetWithFilterChain[] targetsByLevel,
      bool exceptionLoggingOldStyle = false)
    {
      this._targetsByLevel = targetsByLevel;
      this.ExceptionLoggingOldStyle = exceptionLoggingOldStyle;
    }

    [Obsolete("This property marked obsolete before v4.3.11 and it will be removed in NLog 5.")]
    public bool ExceptionLoggingOldStyle { get; private set; }

    public TargetWithFilterChain GetTargetsForLevel(NLog.LogLevel level)
    {
      return level == NLog.LogLevel.Off ? (TargetWithFilterChain) null : this._targetsByLevel[level.Ordinal];
    }

    public bool IsEnabled(NLog.LogLevel level)
    {
      return !(level == NLog.LogLevel.Off) && this._targetsByLevel[level.Ordinal] != null;
    }
  }
}

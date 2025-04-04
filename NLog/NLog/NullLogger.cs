// Decompiled with JetBrains decompiler
// Type: NLog.NullLogger
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using NLog.Internal;
using System;

#nullable disable
namespace NLog
{
  public sealed class NullLogger : Logger
  {
    private NullLogger()
    {
    }

    public NullLogger(LogFactory factory)
    {
      if (factory == null)
        throw new ArgumentNullException(nameof (factory));
      TargetWithFilterChain[] targetsByLevel = new TargetWithFilterChain[LogLevel.MaxLevel.Ordinal + 1];
      this.Initialize(string.Empty, new LoggerConfiguration(targetsByLevel), factory);
    }
  }
}

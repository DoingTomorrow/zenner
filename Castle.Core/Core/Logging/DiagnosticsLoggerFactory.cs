// Decompiled with JetBrains decompiler
// Type: Castle.Core.Logging.DiagnosticsLoggerFactory
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;

#nullable disable
namespace Castle.Core.Logging
{
  [Serializable]
  public class DiagnosticsLoggerFactory : AbstractLoggerFactory
  {
    private const string DefaultLogName = "CastleDefaultLogger";

    public override ILogger Create(string name)
    {
      return (ILogger) new DiagnosticsLogger("CastleDefaultLogger", name);
    }

    public override ILogger Create(string name, LoggerLevel level)
    {
      DiagnosticsLogger diagnosticsLogger = new DiagnosticsLogger("CastleDefaultLogger", name);
      diagnosticsLogger.Level = level;
      return (ILogger) diagnosticsLogger;
    }
  }
}

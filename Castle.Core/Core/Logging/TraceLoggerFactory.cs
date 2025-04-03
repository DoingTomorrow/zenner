// Decompiled with JetBrains decompiler
// Type: Castle.Core.Logging.TraceLoggerFactory
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

#nullable disable
namespace Castle.Core.Logging
{
  public class TraceLoggerFactory : AbstractLoggerFactory
  {
    public override ILogger Create(string name) => this.InternalCreate(name);

    private ILogger InternalCreate(string name) => (ILogger) new TraceLogger(name);

    public override ILogger Create(string name, LoggerLevel level)
    {
      return this.InternalCreate(name, level);
    }

    private ILogger InternalCreate(string name, LoggerLevel level)
    {
      return (ILogger) new TraceLogger(name, level);
    }
  }
}

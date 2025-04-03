// Decompiled with JetBrains decompiler
// Type: Castle.Core.Logging.ConsoleFactory
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;

#nullable disable
namespace Castle.Core.Logging
{
  [Serializable]
  public class ConsoleFactory : MarshalByRefObject, ILoggerFactory
  {
    public ILogger Create(Type type) => (ILogger) new ConsoleLogger(type.FullName);

    public ILogger Create(string name) => (ILogger) new ConsoleLogger(name);

    public ILogger Create(Type type, LoggerLevel level)
    {
      return (ILogger) new ConsoleLogger(type.Name, level);
    }

    public ILogger Create(string name, LoggerLevel level)
    {
      return (ILogger) new ConsoleLogger(name, level);
    }
  }
}

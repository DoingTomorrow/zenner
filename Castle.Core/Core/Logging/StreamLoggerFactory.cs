// Decompiled with JetBrains decompiler
// Type: Castle.Core.Logging.StreamLoggerFactory
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Castle.Core.Logging
{
  [Serializable]
  public class StreamLoggerFactory : AbstractLoggerFactory
  {
    public override ILogger Create(string name)
    {
      return (ILogger) new StreamLogger(name, (Stream) new FileStream(name + ".log", FileMode.Append, FileAccess.Write), Encoding.Default);
    }

    public override ILogger Create(string name, LoggerLevel level)
    {
      StreamLogger streamLogger = new StreamLogger(name, (Stream) new FileStream(name + ".log", FileMode.Append, FileAccess.Write), Encoding.Default);
      streamLogger.Level = level;
      return (ILogger) streamLogger;
    }
  }
}

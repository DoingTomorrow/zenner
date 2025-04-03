// Decompiled with JetBrains decompiler
// Type: Castle.Core.Logging.StreamLogger
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
  public class StreamLogger : LevelFilteredLogger, IDisposable
  {
    private StreamWriter writer;

    public StreamLogger(string name, Stream stream)
      : this(name, new StreamWriter(stream))
    {
    }

    public StreamLogger(string name, Stream stream, Encoding encoding)
      : this(name, new StreamWriter(stream, encoding))
    {
    }

    public StreamLogger(string name, Stream stream, Encoding encoding, int bufferSize)
      : this(name, new StreamWriter(stream, encoding, bufferSize))
    {
    }

    ~StreamLogger() => this.Dispose(false);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing || this.writer == null)
        return;
      this.writer.Close();
      this.writer = (StreamWriter) null;
    }

    protected StreamLogger(string name, StreamWriter writer)
      : base(name, LoggerLevel.Debug)
    {
      this.writer = writer;
      writer.AutoFlush = true;
    }

    protected override void Log(
      LoggerLevel loggerLevel,
      string loggerName,
      string message,
      Exception exception)
    {
      if (this.writer == null)
        return;
      this.writer.WriteLine("[{0}] '{1}' {2}", (object) loggerLevel.ToString(), (object) loggerName, (object) message);
      if (exception == null)
        return;
      this.writer.WriteLine("[{0}] '{1}' {2}: {3} {4}", (object) loggerLevel.ToString(), (object) loggerName, (object) exception.GetType().FullName, (object) exception.Message, (object) exception.StackTrace);
    }

    public override ILogger CreateChildLogger(string loggerName)
    {
      throw new NotSupportedException("A streamlogger does not support child loggers");
    }
  }
}

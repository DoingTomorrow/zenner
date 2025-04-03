// Decompiled with JetBrains decompiler
// Type: Castle.Core.Logging.NullLogger
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;

#nullable disable
namespace Castle.Core.Logging
{
  public class NullLogger : IExtendedLogger, ILogger
  {
    public static readonly NullLogger Instance = new NullLogger();

    public void Debug(string message)
    {
    }

    public void Debug(string message, Exception exception)
    {
    }

    public void Debug(string format, params object[] args)
    {
    }

    public void DebugFormat(string format, params object[] args)
    {
    }

    public void DebugFormat(Exception exception, string format, params object[] args)
    {
    }

    public void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
    {
    }

    public void DebugFormat(
      Exception exception,
      IFormatProvider formatProvider,
      string format,
      params object[] args)
    {
    }

    public bool IsDebugEnabled => false;

    public void Info(string message)
    {
    }

    public void Info(string message, Exception exception)
    {
    }

    public void Info(string format, params object[] args)
    {
    }

    public void InfoFormat(string format, params object[] args)
    {
    }

    public void InfoFormat(Exception exception, string format, params object[] args)
    {
    }

    public void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
    {
    }

    public void InfoFormat(
      Exception exception,
      IFormatProvider formatProvider,
      string format,
      params object[] args)
    {
    }

    public bool IsInfoEnabled => false;

    public void Warn(string message)
    {
    }

    public void Warn(string message, Exception exception)
    {
    }

    public void Warn(string format, params object[] args)
    {
    }

    public void WarnFormat(string format, params object[] args)
    {
    }

    public void WarnFormat(Exception exception, string format, params object[] args)
    {
    }

    public void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
    {
    }

    public void WarnFormat(
      Exception exception,
      IFormatProvider formatProvider,
      string format,
      params object[] args)
    {
    }

    public bool IsWarnEnabled => false;

    public void Error(string message)
    {
    }

    public void Error(string message, Exception exception)
    {
    }

    public void Error(string format, params object[] args)
    {
    }

    public void ErrorFormat(string format, params object[] args)
    {
    }

    public void ErrorFormat(Exception exception, string format, params object[] args)
    {
    }

    public void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
    {
    }

    public void ErrorFormat(
      Exception exception,
      IFormatProvider formatProvider,
      string format,
      params object[] args)
    {
    }

    public bool IsErrorEnabled => false;

    public void Fatal(string message)
    {
    }

    public void Fatal(string message, Exception exception)
    {
    }

    public void Fatal(string format, params object[] args)
    {
    }

    public void FatalFormat(string format, params object[] args)
    {
    }

    public void FatalFormat(Exception exception, string format, params object[] args)
    {
    }

    public void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
    {
    }

    public void FatalFormat(
      Exception exception,
      IFormatProvider formatProvider,
      string format,
      params object[] args)
    {
    }

    public bool IsFatalEnabled => false;

    [Obsolete("Use Fatal instead")]
    public void FatalError(string message)
    {
    }

    [Obsolete("Use Fatal instead")]
    public void FatalError(string message, Exception exception)
    {
    }

    [Obsolete("Use Fatal or FatalFormat instead")]
    public void FatalError(string format, params object[] args)
    {
    }

    [Obsolete("Use IsFatalEnabled instead")]
    public bool IsFatalErrorEnabled => false;

    public ILogger CreateChildLogger(string loggerName) => (ILogger) this;

    public IContextProperties GlobalProperties
    {
      get => (IContextProperties) NullLogger.NullContextProperties.Instance;
    }

    public IContextProperties ThreadProperties
    {
      get => (IContextProperties) NullLogger.NullContextProperties.Instance;
    }

    public IContextStacks ThreadStacks => (IContextStacks) NullLogger.NullContextStacks.Instance;

    private class NullContextProperties : IContextProperties
    {
      public static readonly NullLogger.NullContextProperties Instance = new NullLogger.NullContextProperties();

      public object this[string key]
      {
        get => (object) null;
        set
        {
        }
      }
    }

    private class NullContextStack : IContextStack, IDisposable
    {
      public static readonly NullLogger.NullContextStack Instance = new NullLogger.NullContextStack();

      public int Count => 0;

      public void Clear()
      {
      }

      public string Pop() => (string) null;

      public IDisposable Push(string message) => (IDisposable) this;

      public void Dispose() => GC.SuppressFinalize((object) this);
    }

    private class NullContextStacks : IContextStacks
    {
      public static readonly NullLogger.NullContextStacks Instance = new NullLogger.NullContextStacks();

      public IContextStack this[string key] => (IContextStack) NullLogger.NullContextStack.Instance;
    }
  }
}

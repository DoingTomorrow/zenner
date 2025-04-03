// Decompiled with JetBrains decompiler
// Type: Castle.Core.Logging.ILogger
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;

#nullable disable
namespace Castle.Core.Logging
{
  public interface ILogger
  {
    void Debug(string message);

    void Debug(string message, Exception exception);

    void Debug(string format, params object[] args);

    void DebugFormat(string format, params object[] args);

    void DebugFormat(Exception exception, string format, params object[] args);

    void DebugFormat(IFormatProvider formatProvider, string format, params object[] args);

    void DebugFormat(
      Exception exception,
      IFormatProvider formatProvider,
      string format,
      params object[] args);

    bool IsDebugEnabled { get; }

    void Info(string message);

    void Info(string message, Exception exception);

    void Info(string format, params object[] args);

    void InfoFormat(string format, params object[] args);

    void InfoFormat(Exception exception, string format, params object[] args);

    void InfoFormat(IFormatProvider formatProvider, string format, params object[] args);

    void InfoFormat(
      Exception exception,
      IFormatProvider formatProvider,
      string format,
      params object[] args);

    bool IsInfoEnabled { get; }

    void Warn(string message);

    void Warn(string message, Exception exception);

    void Warn(string format, params object[] args);

    void WarnFormat(string format, params object[] args);

    void WarnFormat(Exception exception, string format, params object[] args);

    void WarnFormat(IFormatProvider formatProvider, string format, params object[] args);

    void WarnFormat(
      Exception exception,
      IFormatProvider formatProvider,
      string format,
      params object[] args);

    bool IsWarnEnabled { get; }

    void Error(string message);

    void Error(string message, Exception exception);

    void Error(string format, params object[] args);

    void ErrorFormat(string format, params object[] args);

    void ErrorFormat(Exception exception, string format, params object[] args);

    void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args);

    void ErrorFormat(
      Exception exception,
      IFormatProvider formatProvider,
      string format,
      params object[] args);

    bool IsErrorEnabled { get; }

    void Fatal(string message);

    void Fatal(string message, Exception exception);

    void Fatal(string format, params object[] args);

    void FatalFormat(string format, params object[] args);

    void FatalFormat(Exception exception, string format, params object[] args);

    void FatalFormat(IFormatProvider formatProvider, string format, params object[] args);

    void FatalFormat(
      Exception exception,
      IFormatProvider formatProvider,
      string format,
      params object[] args);

    bool IsFatalEnabled { get; }

    [Obsolete("Use Fatal instead")]
    void FatalError(string message);

    [Obsolete("Use Fatal instead")]
    void FatalError(string message, Exception exception);

    [Obsolete("Use Fatal or FatalFormat instead")]
    void FatalError(string format, params object[] args);

    [Obsolete("Use IsFatalEnabled instead")]
    bool IsFatalErrorEnabled { get; }

    ILogger CreateChildLogger(string loggerName);
  }
}

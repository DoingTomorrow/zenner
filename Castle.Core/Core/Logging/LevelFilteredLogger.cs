// Decompiled with JetBrains decompiler
// Type: Castle.Core.Logging.LevelFilteredLogger
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Globalization;
using System.Security.Permissions;

#nullable disable
namespace Castle.Core.Logging
{
  [Serializable]
  public abstract class LevelFilteredLogger : MarshalByRefObject, ILogger
  {
    private LoggerLevel level;
    private string name = "unnamed";

    protected LevelFilteredLogger()
    {
    }

    protected LevelFilteredLogger(string name) => this.ChangeName(name);

    protected LevelFilteredLogger(LoggerLevel loggerLevel) => this.level = loggerLevel;

    protected LevelFilteredLogger(string loggerName, LoggerLevel loggerLevel)
      : this(loggerLevel)
    {
      this.ChangeName(loggerName);
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
    public override object InitializeLifetimeService() => (object) null;

    public abstract ILogger CreateChildLogger(string loggerName);

    public LoggerLevel Level
    {
      get => this.level;
      set => this.level = value;
    }

    public string Name => this.name;

    public void Debug(string message)
    {
      if (!this.IsDebugEnabled)
        return;
      this.Log(LoggerLevel.Debug, message, (Exception) null);
    }

    public void Debug(string message, Exception exception)
    {
      if (!this.IsDebugEnabled)
        return;
      this.Log(LoggerLevel.Debug, message, exception);
    }

    public void DebugFormat(string format, params object[] args)
    {
      if (!this.IsDebugEnabled)
        return;
      this.Log(LoggerLevel.Debug, string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, args), (Exception) null);
    }

    public void DebugFormat(Exception exception, string format, params object[] args)
    {
      if (!this.IsDebugEnabled)
        return;
      this.Log(LoggerLevel.Debug, string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, args), exception);
    }

    public void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
    {
      if (!this.IsDebugEnabled)
        return;
      this.Log(LoggerLevel.Debug, string.Format(formatProvider, format, args), (Exception) null);
    }

    public void DebugFormat(
      Exception exception,
      IFormatProvider formatProvider,
      string format,
      params object[] args)
    {
      if (!this.IsDebugEnabled)
        return;
      this.Log(LoggerLevel.Debug, string.Format(formatProvider, format, args), exception);
    }

    public void Debug(string format, params object[] args)
    {
      if (!this.IsDebugEnabled)
        return;
      this.Log(LoggerLevel.Debug, string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, args), (Exception) null);
    }

    public void Info(string message)
    {
      if (!this.IsInfoEnabled)
        return;
      this.Log(LoggerLevel.Info, message, (Exception) null);
    }

    public void Info(string message, Exception exception)
    {
      if (!this.IsInfoEnabled)
        return;
      this.Log(LoggerLevel.Info, message, exception);
    }

    public void InfoFormat(string format, params object[] args)
    {
      if (!this.IsInfoEnabled)
        return;
      this.Log(LoggerLevel.Info, string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, args), (Exception) null);
    }

    public void InfoFormat(Exception exception, string format, params object[] args)
    {
      if (!this.IsInfoEnabled)
        return;
      this.Log(LoggerLevel.Info, string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, args), exception);
    }

    public void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
    {
      if (!this.IsInfoEnabled)
        return;
      this.Log(LoggerLevel.Info, string.Format(formatProvider, format, args), (Exception) null);
    }

    public void InfoFormat(
      Exception exception,
      IFormatProvider formatProvider,
      string format,
      params object[] args)
    {
      if (!this.IsInfoEnabled)
        return;
      this.Log(LoggerLevel.Info, string.Format(formatProvider, format, args), exception);
    }

    public void Info(string format, params object[] args)
    {
      if (!this.IsInfoEnabled)
        return;
      this.Log(LoggerLevel.Info, string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, args), (Exception) null);
    }

    public void Warn(string message)
    {
      if (!this.IsWarnEnabled)
        return;
      this.Log(LoggerLevel.Warn, message, (Exception) null);
    }

    public void Warn(string message, Exception exception)
    {
      if (!this.IsWarnEnabled)
        return;
      this.Log(LoggerLevel.Warn, message, exception);
    }

    public void WarnFormat(string format, params object[] args)
    {
      if (!this.IsWarnEnabled)
        return;
      this.Log(LoggerLevel.Warn, string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, args), (Exception) null);
    }

    public void WarnFormat(Exception exception, string format, params object[] args)
    {
      if (!this.IsWarnEnabled)
        return;
      this.Log(LoggerLevel.Warn, string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, args), exception);
    }

    public void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
    {
      if (!this.IsWarnEnabled)
        return;
      this.Log(LoggerLevel.Warn, string.Format(formatProvider, format, args), (Exception) null);
    }

    public void WarnFormat(
      Exception exception,
      IFormatProvider formatProvider,
      string format,
      params object[] args)
    {
      if (!this.IsWarnEnabled)
        return;
      this.Log(LoggerLevel.Warn, string.Format(formatProvider, format, args), exception);
    }

    public void Warn(string format, params object[] args)
    {
      if (!this.IsWarnEnabled)
        return;
      this.Log(LoggerLevel.Warn, string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, args), (Exception) null);
    }

    public void Error(string message)
    {
      if (!this.IsErrorEnabled)
        return;
      this.Log(LoggerLevel.Error, message, (Exception) null);
    }

    public void Error(string message, Exception exception)
    {
      if (!this.IsErrorEnabled)
        return;
      this.Log(LoggerLevel.Error, message, exception);
    }

    public void ErrorFormat(string format, params object[] args)
    {
      if (!this.IsErrorEnabled)
        return;
      this.Log(LoggerLevel.Error, string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, args), (Exception) null);
    }

    public void ErrorFormat(Exception exception, string format, params object[] args)
    {
      if (!this.IsErrorEnabled)
        return;
      this.Log(LoggerLevel.Error, string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, args), exception);
    }

    public void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
    {
      if (!this.IsErrorEnabled)
        return;
      this.Log(LoggerLevel.Error, string.Format(formatProvider, format, args), (Exception) null);
    }

    public void ErrorFormat(
      Exception exception,
      IFormatProvider formatProvider,
      string format,
      params object[] args)
    {
      if (!this.IsErrorEnabled)
        return;
      this.Log(LoggerLevel.Error, string.Format(formatProvider, format, args), exception);
    }

    public void Error(string format, params object[] args)
    {
      if (!this.IsErrorEnabled)
        return;
      this.Log(LoggerLevel.Error, string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, args), (Exception) null);
    }

    public void Fatal(string message)
    {
      if (!this.IsFatalEnabled)
        return;
      this.Log(LoggerLevel.Fatal, message, (Exception) null);
    }

    public void Fatal(string message, Exception exception)
    {
      if (!this.IsFatalEnabled)
        return;
      this.Log(LoggerLevel.Fatal, message, exception);
    }

    public void FatalFormat(string format, params object[] args)
    {
      if (!this.IsFatalEnabled)
        return;
      this.Log(LoggerLevel.Fatal, string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, args), (Exception) null);
    }

    public void FatalFormat(Exception exception, string format, params object[] args)
    {
      if (!this.IsFatalEnabled)
        return;
      this.Log(LoggerLevel.Fatal, string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, args), exception);
    }

    public void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
    {
      if (!this.IsFatalEnabled)
        return;
      this.Log(LoggerLevel.Fatal, string.Format(formatProvider, format, args), (Exception) null);
    }

    public void FatalFormat(
      Exception exception,
      IFormatProvider formatProvider,
      string format,
      params object[] args)
    {
      if (!this.IsFatalEnabled)
        return;
      this.Log(LoggerLevel.Fatal, string.Format(formatProvider, format, args), exception);
    }

    public void Fatal(string format, params object[] args)
    {
      if (!this.IsFatalEnabled)
        return;
      this.Log(LoggerLevel.Fatal, string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, args), (Exception) null);
    }

    [Obsolete("Use Fatal instead")]
    public void FatalError(string message)
    {
      if (!this.IsFatalEnabled)
        return;
      this.Log(LoggerLevel.Fatal, message, (Exception) null);
    }

    [Obsolete("Use Fatal instead")]
    public void FatalError(string message, Exception exception)
    {
      if (!this.IsFatalEnabled)
        return;
      this.Log(LoggerLevel.Fatal, message, exception);
    }

    [Obsolete("Use Fatal or FatalFormat instead")]
    public void FatalError(string format, params object[] args)
    {
      if (!this.IsFatalEnabled)
        return;
      this.Log(LoggerLevel.Fatal, string.Format((IFormatProvider) CultureInfo.CurrentCulture, format, args), (Exception) null);
    }

    public bool IsDebugEnabled => this.Level >= LoggerLevel.Debug;

    public bool IsInfoEnabled => this.Level >= LoggerLevel.Info;

    public bool IsWarnEnabled => this.Level >= LoggerLevel.Warn;

    public bool IsErrorEnabled => this.Level >= LoggerLevel.Error;

    public bool IsFatalEnabled => this.Level >= LoggerLevel.Fatal;

    [Obsolete("Use IsFatalEnabled instead")]
    public bool IsFatalErrorEnabled => this.Level >= LoggerLevel.Fatal;

    protected abstract void Log(
      LoggerLevel loggerLevel,
      string loggerName,
      string message,
      Exception exception);

    protected void ChangeName(string newName)
    {
      this.name = newName != null ? newName : throw new ArgumentNullException(nameof (newName));
    }

    private void Log(LoggerLevel loggerLevel, string message, Exception exception)
    {
      this.Log(loggerLevel, this.Name, message, exception);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.NLogLogger
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using NHibernate;
using NHibernate.Impl;
using NLog;
using System;

#nullable disable
namespace MSS.Business.Utils
{
  public class NLogLogger : IInternalLogger
  {
    private readonly Logger logger;

    public NLogLogger(Logger logger) => this.logger = logger;

    public bool IsDebugEnabled => this.logger.IsDebugEnabled;

    public bool IsErrorEnabled => this.logger.IsErrorEnabled;

    public bool IsFatalEnabled => this.logger.IsFatalEnabled;

    public bool IsInfoEnabled => this.logger.IsInfoEnabled;

    public bool IsWarnEnabled => this.logger.IsWarnEnabled;

    public void Debug(object message, Exception exception)
    {
      this.logger.Debug("SessionId: " + new NLogLogger.SessionIdCapturer().ToString() + "; " + message.ToString(), exception);
    }

    public void Debug(object message)
    {
      this.logger.Debug("SessionId: " + new NLogLogger.SessionIdCapturer().ToString() + "; " + message.ToString());
    }

    public void DebugFormat(string format, params object[] args)
    {
      this.logger.Debug("SessionId: " + new NLogLogger.SessionIdCapturer().ToString() + "; " + string.Format(format, args));
    }

    public void Error(object message, Exception exception)
    {
      this.logger.Error("SessionId: " + new NLogLogger.SessionIdCapturer().ToString() + "; " + message.ToString(), exception);
    }

    public void Error(object message)
    {
      this.logger.Error("SessionId: " + new NLogLogger.SessionIdCapturer().ToString() + "; " + message.ToString());
    }

    public void ErrorFormat(string format, params object[] args)
    {
      this.logger.Error("SessionId: " + new NLogLogger.SessionIdCapturer().ToString() + "; " + string.Format(format, args));
    }

    public void Fatal(object message, Exception exception)
    {
      this.logger.Fatal("SessionId: " + new NLogLogger.SessionIdCapturer().ToString() + "; " + message.ToString(), exception);
    }

    public void Fatal(object message)
    {
      this.logger.Fatal("SessionId: " + new NLogLogger.SessionIdCapturer().ToString() + "; " + message.ToString());
    }

    public void Info(object message, Exception exception)
    {
      this.logger.Info(message.ToString(), exception);
    }

    public void Info(object message) => this.logger.Info(message.ToString());

    public void InfoFormat(string format, params object[] args)
    {
      this.logger.Info(string.Format(format, args));
    }

    public void Warn(object message, Exception exception)
    {
      this.logger.Warn(message.ToString(), exception);
    }

    public void Warn(object message) => this.logger.Warn(message.ToString());

    public void WarnFormat(string format, params object[] args)
    {
      this.logger.Warn(string.Format(format, args));
    }

    public class SessionIdCapturer
    {
      public override string ToString() => SessionIdLoggingContext.SessionId.ToString();
    }
  }
}

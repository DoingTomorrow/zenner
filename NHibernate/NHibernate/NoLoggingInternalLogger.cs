// Decompiled with JetBrains decompiler
// Type: NHibernate.NoLoggingInternalLogger
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate
{
  public class NoLoggingInternalLogger : IInternalLogger
  {
    public bool IsErrorEnabled => false;

    public bool IsFatalEnabled => false;

    public bool IsDebugEnabled => false;

    public bool IsInfoEnabled => false;

    public bool IsWarnEnabled => false;

    public void Error(object message)
    {
    }

    public void Error(object message, Exception exception)
    {
    }

    public void ErrorFormat(string format, params object[] args)
    {
    }

    public void Fatal(object message)
    {
    }

    public void Fatal(object message, Exception exception)
    {
    }

    public void Debug(object message)
    {
    }

    public void Debug(object message, Exception exception)
    {
    }

    public void DebugFormat(string format, params object[] args)
    {
    }

    public void Info(object message)
    {
    }

    public void Info(object message, Exception exception)
    {
    }

    public void InfoFormat(string format, params object[] args)
    {
    }

    public void Warn(object message)
    {
    }

    public void Warn(object message, Exception exception)
    {
    }

    public void WarnFormat(string format, params object[] args)
    {
    }
  }
}

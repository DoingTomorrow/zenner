// Decompiled with JetBrains decompiler
// Type: NHibernate.IInternalLogger
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate
{
  public interface IInternalLogger
  {
    bool IsErrorEnabled { get; }

    bool IsFatalEnabled { get; }

    bool IsDebugEnabled { get; }

    bool IsInfoEnabled { get; }

    bool IsWarnEnabled { get; }

    void Error(object message);

    void Error(object message, Exception exception);

    void ErrorFormat(string format, params object[] args);

    void Fatal(object message);

    void Fatal(object message, Exception exception);

    void Debug(object message);

    void Debug(object message, Exception exception);

    void DebugFormat(string format, params object[] args);

    void Info(object message);

    void Info(object message, Exception exception);

    void InfoFormat(string format, params object[] args);

    void Warn(object message);

    void Warn(object message, Exception exception);

    void WarnFormat(string format, params object[] args);
  }
}

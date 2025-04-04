// Decompiled with JetBrains decompiler
// Type: NHibernate.Log4NetLogger
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate
{
  public class Log4NetLogger : IInternalLogger
  {
    private static readonly Type ILogType = Type.GetType("log4net.ILog, log4net");
    private static readonly Func<object, bool> IsErrorEnabledDelegate = Log4NetLogger.GetPropertyGetter(nameof (IsErrorEnabled));
    private static readonly Func<object, bool> IsFatalEnabledDelegate = Log4NetLogger.GetPropertyGetter(nameof (IsFatalEnabled));
    private static readonly Func<object, bool> IsDebugEnabledDelegate = Log4NetLogger.GetPropertyGetter(nameof (IsDebugEnabled));
    private static readonly Func<object, bool> IsInfoEnabledDelegate = Log4NetLogger.GetPropertyGetter(nameof (IsInfoEnabled));
    private static readonly Func<object, bool> IsWarnEnabledDelegate = Log4NetLogger.GetPropertyGetter(nameof (IsWarnEnabled));
    private static readonly Action<object, object> ErrorDelegate = Log4NetLogger.GetMethodCallForMessage("Error");
    private static readonly Action<object, object, Exception> ErrorExceptionDelegate = Log4NetLogger.GetMethodCallForMessageException("Error");
    private static readonly Action<object, string, object[]> ErrorFormatDelegate = Log4NetLogger.GetMethodCallForMessageFormat("ErrorFormat");
    private static readonly Action<object, object> FatalDelegate = Log4NetLogger.GetMethodCallForMessage("Fatal");
    private static readonly Action<object, object, Exception> FatalExceptionDelegate = Log4NetLogger.GetMethodCallForMessageException("Fatal");
    private static readonly Action<object, object> DebugDelegate = Log4NetLogger.GetMethodCallForMessage("Debug");
    private static readonly Action<object, object, Exception> DebugExceptionDelegate = Log4NetLogger.GetMethodCallForMessageException("Debug");
    private static readonly Action<object, string, object[]> DebugFormatDelegate = Log4NetLogger.GetMethodCallForMessageFormat("DebugFormat");
    private static readonly Action<object, object> InfoDelegate = Log4NetLogger.GetMethodCallForMessage("Info");
    private static readonly Action<object, object, Exception> InfoExceptionDelegate = Log4NetLogger.GetMethodCallForMessageException("Info");
    private static readonly Action<object, string, object[]> InfoFormatDelegate = Log4NetLogger.GetMethodCallForMessageFormat("InfoFormat");
    private static readonly Action<object, object> WarnDelegate = Log4NetLogger.GetMethodCallForMessage("Warn");
    private static readonly Action<object, object, Exception> WarnExceptionDelegate = Log4NetLogger.GetMethodCallForMessageException("Warn");
    private static readonly Action<object, string, object[]> WarnFormatDelegate = Log4NetLogger.GetMethodCallForMessageFormat("WarnFormat");
    private readonly object logger;

    private static Func<object, bool> GetPropertyGetter(string propertyName)
    {
      ParameterExpression parameterExpression = Expression.Parameter(typeof (object), "l");
      return (Func<object, bool>) Expression.Lambda((Expression) Expression.Property((Expression) Expression.Convert((Expression) parameterExpression, Log4NetLogger.ILogType), propertyName), parameterExpression).Compile();
    }

    private static Action<object, object> GetMethodCallForMessage(string methodName)
    {
      ParameterExpression parameterExpression1 = Expression.Parameter(typeof (object), "l");
      ParameterExpression parameterExpression2 = Expression.Parameter(typeof (object), "o");
      return (Action<object, object>) Expression.Lambda((Expression) Expression.Call((Expression) Expression.Convert((Expression) parameterExpression1, Log4NetLogger.ILogType), Log4NetLogger.ILogType.GetMethod(methodName, new Type[1]
      {
        typeof (object)
      }), (Expression) parameterExpression2), parameterExpression1, parameterExpression2).Compile();
    }

    private static Action<object, object, Exception> GetMethodCallForMessageException(
      string methodName)
    {
      ParameterExpression parameterExpression1 = Expression.Parameter(typeof (object), "l");
      ParameterExpression parameterExpression2 = Expression.Parameter(typeof (object), "o");
      ParameterExpression parameterExpression3 = Expression.Parameter(typeof (Exception), "e");
      return (Action<object, object, Exception>) Expression.Lambda((Expression) Expression.Call((Expression) Expression.Convert((Expression) parameterExpression1, Log4NetLogger.ILogType), Log4NetLogger.ILogType.GetMethod(methodName, new Type[2]
      {
        typeof (object),
        typeof (Exception)
      }), (Expression) parameterExpression2, (Expression) parameterExpression3), parameterExpression1, parameterExpression2, parameterExpression3).Compile();
    }

    private static Action<object, string, object[]> GetMethodCallForMessageFormat(string methodName)
    {
      ParameterExpression parameterExpression1 = Expression.Parameter(typeof (object), "l");
      ParameterExpression parameterExpression2 = Expression.Parameter(typeof (string), "f");
      ParameterExpression parameterExpression3 = Expression.Parameter(typeof (object[]), "p");
      return (Action<object, string, object[]>) Expression.Lambda((Expression) Expression.Call((Expression) Expression.Convert((Expression) parameterExpression1, Log4NetLogger.ILogType), Log4NetLogger.ILogType.GetMethod(methodName, new Type[2]
      {
        typeof (string),
        typeof (object[])
      }), (Expression) parameterExpression2, (Expression) parameterExpression3), parameterExpression1, parameterExpression2, parameterExpression3).Compile();
    }

    public Log4NetLogger(object logger) => this.logger = logger;

    public bool IsErrorEnabled => Log4NetLogger.IsErrorEnabledDelegate(this.logger);

    public bool IsFatalEnabled => Log4NetLogger.IsFatalEnabledDelegate(this.logger);

    public bool IsDebugEnabled => Log4NetLogger.IsDebugEnabledDelegate(this.logger);

    public bool IsInfoEnabled => Log4NetLogger.IsInfoEnabledDelegate(this.logger);

    public bool IsWarnEnabled => Log4NetLogger.IsWarnEnabledDelegate(this.logger);

    public void Error(object message)
    {
      if (!this.IsErrorEnabled)
        return;
      Log4NetLogger.ErrorDelegate(this.logger, message);
    }

    public void Error(object message, Exception exception)
    {
      if (!this.IsErrorEnabled)
        return;
      Log4NetLogger.ErrorExceptionDelegate(this.logger, message, exception);
    }

    public void ErrorFormat(string format, params object[] args)
    {
      if (!this.IsErrorEnabled)
        return;
      Log4NetLogger.ErrorFormatDelegate(this.logger, format, args);
    }

    public void Fatal(object message)
    {
      if (!this.IsFatalEnabled)
        return;
      Log4NetLogger.FatalDelegate(this.logger, message);
    }

    public void Fatal(object message, Exception exception)
    {
      if (!this.IsFatalEnabled)
        return;
      Log4NetLogger.FatalExceptionDelegate(this.logger, message, exception);
    }

    public void Debug(object message)
    {
      if (!this.IsDebugEnabled)
        return;
      Log4NetLogger.DebugDelegate(this.logger, message);
    }

    public void Debug(object message, Exception exception)
    {
      if (!this.IsDebugEnabled)
        return;
      Log4NetLogger.DebugExceptionDelegate(this.logger, message, exception);
    }

    public void DebugFormat(string format, params object[] args)
    {
      if (!this.IsDebugEnabled)
        return;
      Log4NetLogger.DebugFormatDelegate(this.logger, format, args);
    }

    public void Info(object message)
    {
      if (!this.IsInfoEnabled)
        return;
      Log4NetLogger.InfoDelegate(this.logger, message);
    }

    public void Info(object message, Exception exception)
    {
      if (!this.IsInfoEnabled)
        return;
      Log4NetLogger.InfoExceptionDelegate(this.logger, message, exception);
    }

    public void InfoFormat(string format, params object[] args)
    {
      if (!this.IsInfoEnabled)
        return;
      Log4NetLogger.InfoFormatDelegate(this.logger, format, args);
    }

    public void Warn(object message)
    {
      if (!this.IsWarnEnabled)
        return;
      Log4NetLogger.WarnDelegate(this.logger, message);
    }

    public void Warn(object message, Exception exception)
    {
      if (!this.IsWarnEnabled)
        return;
      Log4NetLogger.WarnExceptionDelegate(this.logger, message, exception);
    }

    public void WarnFormat(string format, params object[] args)
    {
      if (!this.IsWarnEnabled)
        return;
      Log4NetLogger.WarnFormatDelegate(this.logger, format, args);
    }
  }
}

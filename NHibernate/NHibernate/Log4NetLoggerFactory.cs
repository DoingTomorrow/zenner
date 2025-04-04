// Decompiled with JetBrains decompiler
// Type: NHibernate.Log4NetLoggerFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate
{
  public class Log4NetLoggerFactory : ILoggerFactory
  {
    private static readonly Type LogManagerType = Type.GetType("log4net.LogManager, log4net");
    private static readonly Func<string, object> GetLoggerByNameDelegate = Log4NetLoggerFactory.GetGetLoggerMethodCall<string>();
    private static readonly Func<Type, object> GetLoggerByTypeDelegate = Log4NetLoggerFactory.GetGetLoggerMethodCall<Type>();

    public IInternalLogger LoggerFor(string keyName)
    {
      return (IInternalLogger) new Log4NetLogger(Log4NetLoggerFactory.GetLoggerByNameDelegate(keyName));
    }

    public IInternalLogger LoggerFor(Type type)
    {
      return (IInternalLogger) new Log4NetLogger(Log4NetLoggerFactory.GetLoggerByTypeDelegate(type));
    }

    private static Func<TParameter, object> GetGetLoggerMethodCall<TParameter>()
    {
      MethodInfo method = Log4NetLoggerFactory.LogManagerType.GetMethod("GetLogger", new Type[1]
      {
        typeof (TParameter)
      });
      ParameterExpression parameterExpression1 = Expression.Parameter(typeof (TParameter), "key");
      ParameterExpression parameterExpression2;
      return Expression.Lambda<Func<TParameter, object>>((Expression) Expression.Call((Expression) null, method, (Expression) (parameterExpression2 = parameterExpression1)), parameterExpression2).Compile();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.DefaultLazyInitializer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Proxy.DynamicProxy;
using NHibernate.Proxy.Poco;
using NHibernate.Type;
using System;
using System.Reflection;

#nullable disable
namespace NHibernate.Proxy
{
  [Serializable]
  public class DefaultLazyInitializer(
    string entityName,
    System.Type persistentClass,
    object id,
    MethodInfo getIdentifierMethod,
    MethodInfo setIdentifierMethod,
    IAbstractComponentType componentIdType,
    ISessionImplementor session) : BasicLazyInitializer(entityName, persistentClass, id, getIdentifierMethod, setIdentifierMethod, componentIdType, session), NHibernate.Proxy.DynamicProxy.IInterceptor
  {
    [NonSerialized]
    private static readonly MethodInfo exceptionInternalPreserveStackTrace = typeof (Exception).GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);

    public object Intercept(InvocationInfo info)
    {
      try
      {
        object obj = this.Invoke(info.TargetMethod, info.Arguments, info.Target);
        return obj != AbstractLazyInitializer.InvokeImplementation ? obj : info.TargetMethod.Invoke(this.GetImplementation(), info.Arguments);
      }
      catch (TargetInvocationException ex)
      {
        DefaultLazyInitializer.exceptionInternalPreserveStackTrace.Invoke((object) ex.InnerException, new object[0]);
        throw ex.InnerException;
      }
    }
  }
}

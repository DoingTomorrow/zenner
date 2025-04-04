// Decompiled with JetBrains decompiler
// Type: NHibernate.ByteCode.Castle.LazyInitializer
// Assembly: NHibernate.ByteCode.Castle, Version=3.1.0.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: 1144BD3F-E8FD-45B0-9AA0-77466B846AAB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.ByteCode.Castle.dll

using Castle.DynamicProxy;
using NHibernate.Engine;
using NHibernate.Proxy;
using NHibernate.Proxy.Poco;
using NHibernate.Type;
using System;
using System.Reflection;

#nullable disable
namespace NHibernate.ByteCode.Castle
{
  [CLSCompliant(false)]
  [Serializable]
  public class LazyInitializer(
    string entityName,
    System.Type persistentClass,
    object id,
    MethodInfo getIdentifierMethod,
    MethodInfo setIdentifierMethod,
    IAbstractComponentType componentIdType,
    ISessionImplementor session) : BasicLazyInitializer(entityName, persistentClass, id, getIdentifierMethod, setIdentifierMethod, componentIdType, session), Castle.DynamicProxy.IInterceptor
  {
    private static readonly MethodInfo Exception_InternalPreserveStackTrace = typeof (Exception).GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);
    public bool _constructed;

    public virtual void Intercept(IInvocation invocation)
    {
      try
      {
        if (!this._constructed)
          return;
        invocation.ReturnValue = this.Invoke(invocation.Method, invocation.Arguments, invocation.Proxy);
        if (invocation.ReturnValue != AbstractLazyInitializer.InvokeImplementation)
          return;
        invocation.ReturnValue = invocation.Method.Invoke(this.GetImplementation(), invocation.Arguments);
      }
      catch (TargetInvocationException ex)
      {
        LazyInitializer.Exception_InternalPreserveStackTrace.Invoke((object) ex.InnerException, new object[0]);
        throw ex.InnerException;
      }
    }
  }
}

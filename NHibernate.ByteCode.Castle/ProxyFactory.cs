// Decompiled with JetBrains decompiler
// Type: NHibernate.ByteCode.Castle.ProxyFactory
// Assembly: NHibernate.ByteCode.Castle, Version=3.1.0.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: 1144BD3F-E8FD-45B0-9AA0-77466B846AAB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.ByteCode.Castle.dll

using Castle.DynamicProxy;
using NHibernate.Engine;
using NHibernate.Proxy;
using System;

#nullable disable
namespace NHibernate.ByteCode.Castle
{
  public class ProxyFactory : AbstractProxyFactory
  {
    protected static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (ProxyFactory));
    private static readonly ProxyGenerator ProxyGenerator = new ProxyGenerator();

    protected static ProxyGenerator DefaultProxyGenerator => ProxyFactory.ProxyGenerator;

    public override INHibernateProxy GetProxy(object id, ISessionImplementor session)
    {
      try
      {
        LazyInitializer lazyInitializer = new LazyInitializer(this.EntityName, this.PersistentClass, id, this.GetIdentifierMethod, this.SetIdentifierMethod, this.ComponentIdType, session);
        object obj;
        if (!this.IsClassProxy)
          obj = ProxyFactory.ProxyGenerator.CreateInterfaceProxyWithoutTarget(this.Interfaces[0], this.Interfaces, (Castle.DynamicProxy.IInterceptor) lazyInitializer);
        else
          obj = ProxyFactory.ProxyGenerator.CreateClassProxy(this.PersistentClass, this.Interfaces, (Castle.DynamicProxy.IInterceptor) lazyInitializer);
        object proxy = obj;
        lazyInitializer._constructed = true;
        return (INHibernateProxy) proxy;
      }
      catch (Exception ex)
      {
        ProxyFactory.log.Error((object) "Creating a proxy instance failed", ex);
        throw new HibernateException("Creating a proxy instance failed", ex);
      }
    }

    public virtual object GetFieldInterceptionProxy()
    {
      ProxyGenerationOptions options = new ProxyGenerationOptions();
      LazyFieldInterceptor instance = new LazyFieldInterceptor();
      options.AddMixinInstance((object) instance);
      return ProxyFactory.ProxyGenerator.CreateClassProxy(this.PersistentClass, options, (Castle.DynamicProxy.IInterceptor) instance);
    }
  }
}

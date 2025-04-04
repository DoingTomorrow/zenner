// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.DefaultProxyFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Intercept;
using NHibernate.Proxy.DynamicProxy;
using System;

#nullable disable
namespace NHibernate.Proxy
{
  public class DefaultProxyFactory : AbstractProxyFactory
  {
    private readonly ProxyFactory factory = new ProxyFactory();
    protected static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (DefaultProxyFactory));

    public override INHibernateProxy GetProxy(object id, ISessionImplementor session)
    {
      try
      {
        DefaultLazyInitializer defaultLazyInitializer = new DefaultLazyInitializer(this.EntityName, this.PersistentClass, id, this.GetIdentifierMethod, this.SetIdentifierMethod, this.ComponentIdType, session);
        return this.IsClassProxy ? (INHibernateProxy) this.factory.CreateProxy(this.PersistentClass, (NHibernate.Proxy.DynamicProxy.IInterceptor) defaultLazyInitializer, this.Interfaces) : (INHibernateProxy) this.factory.CreateProxy(this.Interfaces[0], (NHibernate.Proxy.DynamicProxy.IInterceptor) defaultLazyInitializer, this.Interfaces);
      }
      catch (Exception ex)
      {
        DefaultProxyFactory.log.Error((object) "Creating a proxy instance failed", ex);
        throw new HibernateException("Creating a proxy instance failed", ex);
      }
    }

    public override object GetFieldInterceptionProxy(object instanceToWrap)
    {
      return this.factory.CreateProxy(this.PersistentClass, (NHibernate.Proxy.DynamicProxy.IInterceptor) new DefaultDynamicLazyFieldInterceptor(instanceToWrap), typeof (IFieldInterceptorAccessor));
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.NHibernateProxyHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Intercept;
using System;

#nullable disable
namespace NHibernate.Proxy
{
  public static class NHibernateProxyHelper
  {
    public static Type GetClassWithoutInitializingProxy(object obj)
    {
      return obj.IsProxy() ? (obj as INHibernateProxy).HibernateLazyInitializer.PersistentClass : obj.GetType();
    }

    public static Type GuessClass(object entity)
    {
      if (entity.IsProxy())
      {
        ILazyInitializer hibernateLazyInitializer = (entity as INHibernateProxy).HibernateLazyInitializer;
        if (hibernateLazyInitializer.IsUninitialized)
          return hibernateLazyInitializer.PersistentClass;
        entity = hibernateLazyInitializer.GetImplementation();
      }
      return entity is IFieldInterceptorAccessor interceptorAccessor ? interceptorAccessor.FieldInterceptor.MappedClass : entity.GetType();
    }

    public static bool IsProxy(this object entity)
    {
      return NHibernate.Cfg.Environment.BytecodeProvider.ProxyFactoryFactory.IsProxy(entity);
    }
  }
}

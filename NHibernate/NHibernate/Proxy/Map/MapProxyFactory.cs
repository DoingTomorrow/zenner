// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.Map.MapProxyFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Type;
using System;
using System.Reflection;

#nullable disable
namespace NHibernate.Proxy.Map
{
  public class MapProxyFactory : IProxyFactory
  {
    private string entityName;

    public void PostInstantiate(
      string entityName,
      System.Type persistentClass,
      ISet<System.Type> interfaces,
      MethodInfo getIdentifierMethod,
      MethodInfo setIdentifierMethod,
      IAbstractComponentType componentIdType)
    {
      this.entityName = entityName;
    }

    public INHibernateProxy GetProxy(object id, ISessionImplementor session)
    {
      return (INHibernateProxy) new MapProxy(new MapLazyInitializer(this.entityName, id, session));
    }

    public object GetFieldInterceptionProxy(object getInstance)
    {
      throw new NotSupportedException();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.AbstractProxyFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Type;
using System;
using System.Reflection;

#nullable disable
namespace NHibernate.Proxy
{
  public abstract class AbstractProxyFactory : IProxyFactory
  {
    protected virtual string EntityName { get; private set; }

    protected virtual System.Type PersistentClass { get; private set; }

    protected virtual System.Type[] Interfaces { get; private set; }

    protected virtual MethodInfo GetIdentifierMethod { get; private set; }

    protected virtual MethodInfo SetIdentifierMethod { get; private set; }

    protected virtual IAbstractComponentType ComponentIdType { get; private set; }

    protected bool IsClassProxy => this.Interfaces.Length == 1;

    public virtual void PostInstantiate(
      string entityName,
      System.Type persistentClass,
      ISet<System.Type> interfaces,
      MethodInfo getIdentifierMethod,
      MethodInfo setIdentifierMethod,
      IAbstractComponentType componentIdType)
    {
      this.EntityName = entityName;
      this.PersistentClass = persistentClass;
      this.Interfaces = new System.Type[interfaces.Count];
      if (interfaces.Count > 0)
        interfaces.CopyTo(this.Interfaces, 0);
      this.GetIdentifierMethod = getIdentifierMethod;
      this.SetIdentifierMethod = setIdentifierMethod;
      this.ComponentIdType = componentIdType;
    }

    public abstract INHibernateProxy GetProxy(object id, ISessionImplementor session);

    public virtual object GetFieldInterceptionProxy(object instanceToWrap)
    {
      throw new NotSupportedException();
    }
  }
}

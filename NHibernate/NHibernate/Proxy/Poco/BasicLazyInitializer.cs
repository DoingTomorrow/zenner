// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.Poco.BasicLazyInitializer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate.Proxy.Poco
{
  [Serializable]
  public abstract class BasicLazyInitializer : AbstractLazyInitializer
  {
    private static readonly IEqualityComparer IdentityEqualityComparer = (IEqualityComparer) new NHibernate.IdentityEqualityComparer();
    internal System.Type persistentClass;
    protected internal MethodInfo getIdentifierMethod;
    protected internal MethodInfo setIdentifierMethod;
    protected internal bool overridesEquals;
    protected internal IAbstractComponentType componentIdType;

    protected internal BasicLazyInitializer(
      string entityName,
      System.Type persistentClass,
      object id,
      MethodInfo getIdentifierMethod,
      MethodInfo setIdentifierMethod,
      IAbstractComponentType componentIdType,
      ISessionImplementor session)
      : base(entityName, id, session)
    {
      this.persistentClass = persistentClass;
      this.getIdentifierMethod = getIdentifierMethod;
      this.setIdentifierMethod = setIdentifierMethod;
      this.componentIdType = componentIdType;
      this.overridesEquals = ReflectHelper.OverridesEquals(persistentClass);
    }

    protected virtual void AddSerializationInfo(SerializationInfo info, StreamingContext context)
    {
    }

    public override System.Type PersistentClass => this.persistentClass;

    public virtual object Invoke(MethodInfo method, object[] args, object proxy)
    {
      string name = method.Name;
      switch (method.GetParameters().Length)
      {
        case 0:
          if (!this.overridesEquals && name == "GetHashCode")
            return (object) BasicLazyInitializer.IdentityEqualityComparer.GetHashCode(proxy);
          if (this.IsUninitialized && this.IsEqualToIdentifierMethod(method))
            return this.Identifier;
          if (name == "Dispose")
            return (object) null;
          if ("get_HibernateLazyInitializer".Equals(name))
            return (object) this;
          break;
        case 1:
          if (!this.overridesEquals && name == "Equals")
            return (object) BasicLazyInitializer.IdentityEqualityComparer.Equals(args[0], proxy);
          if (this.setIdentifierMethod != null && method.Equals((object) this.setIdentifierMethod))
          {
            this.Initialize();
            this.Identifier = args[0];
            return AbstractLazyInitializer.InvokeImplementation;
          }
          break;
        case 2:
          if (name == "GetObjectData")
          {
            SerializationInfo info = (SerializationInfo) args[0];
            StreamingContext context = (StreamingContext) args[1];
            if (this.Target == null & this.Session != null)
            {
              object entity = this.Session.PersistenceContext.GetEntity(new EntityKey(this.Identifier, this.Session.Factory.GetEntityPersister(this.EntityName), this.Session.EntityMode));
              if (entity != null)
                this.SetImplementation(entity);
            }
            this.AddSerializationInfo(info, context);
            return (object) null;
          }
          break;
      }
      return this.componentIdType != null && this.componentIdType.IsMethodOf((MethodBase) method) ? method.Invoke(this.Identifier, args) : AbstractLazyInitializer.InvokeImplementation;
    }

    private bool IsEqualToIdentifierMethod(MethodInfo method)
    {
      return this.getIdentifierMethod != null && method.Name.Equals(this.getIdentifierMethod.Name) && method.ReturnType.Equals(this.getIdentifierMethod.ReturnType);
    }
  }
}

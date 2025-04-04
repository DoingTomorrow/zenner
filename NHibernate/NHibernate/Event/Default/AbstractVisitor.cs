// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.AbstractVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Intercept;
using NHibernate.Persister.Entity;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Event.Default
{
  public abstract class AbstractVisitor
  {
    private readonly IEventSource session;

    public AbstractVisitor(IEventSource session) => this.session = session;

    public IEventSource Session => this.session;

    internal void ProcessValues(object[] values, IType[] types)
    {
      for (int i = 0; i < types.Length; ++i)
      {
        if (this.IncludeProperty(values, i))
          this.ProcessValue(i, values, types);
      }
    }

    internal virtual void ProcessValue(int i, object[] values, IType[] types)
    {
      this.ProcessValue(values[i], types[i]);
    }

    internal object ProcessValue(object value, IType type)
    {
      if (type.IsCollectionType)
        return this.ProcessCollection(value, (CollectionType) type);
      if (type.IsEntityType)
        return this.ProcessEntity(value, (EntityType) type);
      return type.IsComponentType ? this.ProcessComponent(value, (IAbstractComponentType) type) : (object) null;
    }

    internal virtual object ProcessComponent(object component, IAbstractComponentType componentType)
    {
      if (component != null)
        this.ProcessValues(componentType.GetPropertyValues(component, (ISessionImplementor) this.session), componentType.Subtypes);
      return (object) null;
    }

    internal virtual object ProcessEntity(object value, EntityType entityType) => (object) null;

    internal virtual object ProcessCollection(object value, CollectionType collectionType)
    {
      return (object) null;
    }

    internal virtual void Process(object obj, IEntityPersister persister)
    {
      this.ProcessEntityPropertyValues(persister.GetPropertyValues(obj, this.Session.EntityMode), persister.PropertyTypes);
    }

    public void ProcessEntityPropertyValues(object[] values, IType[] types)
    {
      for (int i = 0; i < types.Length; ++i)
      {
        if (this.IncludeEntityProperty(values, i))
          this.ProcessValue(i, values, types);
      }
    }

    internal virtual bool IncludeEntityProperty(object[] values, int i)
    {
      return this.IncludeProperty(values, i);
    }

    internal bool IncludeProperty(object[] values, int i)
    {
      return !object.Equals(LazyPropertyInitializer.UnfetchedProperty, values[i]);
    }
  }
}

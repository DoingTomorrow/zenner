// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.ForeignKeys
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Id;
using NHibernate.Intercept;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Engine
{
  public static class ForeignKeys
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (ForeignKeys));

    public static bool IsNotTransient(
      string entityName,
      object entity,
      bool? assumed,
      ISessionImplementor session)
    {
      return entity.IsProxy() || session.PersistenceContext.IsEntryFor(entity) || !ForeignKeys.IsTransient(entityName, entity, assumed, session);
    }

    public static bool IsTransient(
      string entityName,
      object entity,
      bool? assumed,
      ISessionImplementor session)
    {
      if (object.Equals(LazyPropertyInitializer.UnfetchedProperty, entity))
        return false;
      bool? nullable = session.Interceptor.IsTransient(entity);
      if (nullable.HasValue)
        return nullable.Value;
      IEntityPersister entityPersister = session.GetEntityPersister(entityName, entity);
      nullable = entityPersister.IsTransient(entity, session);
      if (nullable.HasValue)
        return nullable.Value;
      if (assumed.HasValue)
        return assumed.Value;
      if (entityPersister.IdentifierGenerator is Assigned)
        ForeignKeys.log.Warn((object) ("Unable to determine if " + entity.ToString() + " with assigned identifier " + entityPersister.GetIdentifier(entity, session.EntityMode) + " is transient or detached; querying the database. Use explicit Save() or Update() in session to prevent this."));
      return session.PersistenceContext.GetDatabaseSnapshot(entityPersister.GetIdentifier(entity, session.EntityMode), entityPersister) == null;
    }

    public static object GetEntityIdentifierIfNotUnsaved(
      string entityName,
      object entity,
      ISessionImplementor session)
    {
      if (entity == null)
        return (object) null;
      object identifierIfNotUnsaved = session.GetContextEntityIdentifier(entity);
      if (identifierIfNotUnsaved == null)
      {
        if (entity.GetType().IsPrimitive)
          return entity;
        if (ForeignKeys.IsTransient(entityName, entity, new bool?(false), session))
        {
          EntityEntry entry = session.PersistenceContext.GetEntry(entity);
          if (entry != null)
            return entry.Id;
          entityName = entityName ?? session.GuessEntityName(entity);
          string str = entity.ToString();
          throw new TransientObjectException(string.Format("object references an unsaved transient instance - save the transient instance before flushing or set cascade action for the property to something that would make it autosave. Type: {0}, Entity: {1}", (object) entityName, (object) str));
        }
        identifierIfNotUnsaved = session.GetEntityPersister(entityName, entity).GetIdentifier(entity, session.EntityMode);
      }
      return identifierIfNotUnsaved;
    }

    public class Nullifier
    {
      private readonly bool isDelete;
      private readonly bool isEarlyInsert;
      private readonly ISessionImplementor session;
      private readonly object self;

      public Nullifier(
        object self,
        bool isDelete,
        bool isEarlyInsert,
        ISessionImplementor session)
      {
        this.isDelete = isDelete;
        this.isEarlyInsert = isEarlyInsert;
        this.session = session;
        this.self = self;
      }

      public void NullifyTransientReferences(object[] values, IType[] types)
      {
        for (int index = 0; index < types.Length; ++index)
          values[index] = this.NullifyTransientReferences(values[index], types[index]);
      }

      private object NullifyTransientReferences(object value, IType type)
      {
        if (value == null)
          return (object) null;
        if (type.IsEntityType)
        {
          EntityType entityType = (EntityType) type;
          return entityType.IsOneToOne || !this.IsNullifiable(entityType.GetAssociatedEntityName(), value) ? value : (object) null;
        }
        if (type.IsAnyType)
          return !this.IsNullifiable((string) null, value) ? value : (object) null;
        if (!type.IsComponentType)
          return value;
        IAbstractComponentType abstractComponentType = (IAbstractComponentType) type;
        object[] propertyValues = abstractComponentType.GetPropertyValues(value, this.session);
        IType[] subtypes = abstractComponentType.Subtypes;
        bool flag = false;
        for (int index = 0; index < propertyValues.Length; ++index)
        {
          object obj = this.NullifyTransientReferences(propertyValues[index], subtypes[index]);
          if (obj != propertyValues[index])
          {
            flag = true;
            propertyValues[index] = obj;
          }
        }
        if (flag)
          abstractComponentType.SetPropertyValues(value, propertyValues, this.session.EntityMode);
        return value;
      }

      private bool IsNullifiable(string entityName, object obj)
      {
        if (obj.IsProxy())
        {
          ILazyInitializer hibernateLazyInitializer = (obj as INHibernateProxy).HibernateLazyInitializer;
          if (hibernateLazyInitializer.GetImplementation(this.session) == null)
            return false;
          obj = hibernateLazyInitializer.GetImplementation();
        }
        if (obj == this.self)
          return this.isEarlyInsert || this.isDelete;
        EntityEntry entry = this.session.PersistenceContext.GetEntry(obj);
        return entry == null ? ForeignKeys.IsTransient(entityName, obj, new bool?(), this.session) : entry.IsNullifiable(this.isEarlyInsert, this.session);
      }
    }
  }
}

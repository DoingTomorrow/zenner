// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.CollectionType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.SqlTypes;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Xml;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public abstract class CollectionType : AbstractType, IAssociationType, IType, ICacheAssembler
  {
    private static readonly object NotNullCollection = new object();
    public static readonly object UnfetchedCollection = new object();
    private readonly string role;
    private readonly string foreignKeyPropertyName;
    private readonly bool isEmbeddedInXML;
    private static readonly SqlType[] NoSqlTypes = new SqlType[0];

    protected CollectionType(string role, string foreignKeyPropertyName, bool isEmbeddedInXML)
    {
      this.role = role;
      this.isEmbeddedInXML = isEmbeddedInXML;
      this.foreignKeyPropertyName = foreignKeyPropertyName;
    }

    public virtual string Role => this.role;

    public override bool IsCollectionType => true;

    public override bool IsEqual(object x, object y, EntityMode entityMode)
    {
      if (x == y || x is IPersistentCollection && ((IPersistentCollection) x).IsWrapper(y))
        return true;
      return y is IPersistentCollection && ((IPersistentCollection) y).IsWrapper(x);
    }

    public override int GetHashCode(object x, EntityMode entityMode)
    {
      throw new InvalidOperationException("cannot perform lookups on collections");
    }

    public abstract IPersistentCollection Instantiate(
      ISessionImplementor session,
      ICollectionPersister persister,
      object key);

    public override object NullSafeGet(
      IDataReader rs,
      string name,
      ISessionImplementor session,
      object owner)
    {
      return this.NullSafeGet(rs, new string[1]{ name }, session, owner);
    }

    public override object NullSafeGet(
      IDataReader rs,
      string[] name,
      ISessionImplementor session,
      object owner)
    {
      return this.ResolveIdentifier((object) null, session, owner);
    }

    public override void NullSafeSet(
      IDbCommand st,
      object value,
      int index,
      bool[] settable,
      ISessionImplementor session)
    {
    }

    public override void NullSafeSet(
      IDbCommand cmd,
      object value,
      int index,
      ISessionImplementor session)
    {
    }

    public override SqlType[] SqlTypes(IMapping session) => CollectionType.NoSqlTypes;

    public override int GetColumnSpan(IMapping session) => 0;

    public override string ToLoggableString(object value, ISessionFactoryImplementor factory)
    {
      if (value == null)
        return "null";
      return !NHibernateUtil.IsInitialized(value) ? "<uninitialized>" : this.RenderLoggableString(value, factory);
    }

    public override object DeepCopy(
      object value,
      EntityMode entityMode,
      ISessionFactoryImplementor factory)
    {
      return value;
    }

    public override string Name
    {
      get => this.ReturnedClass.FullName + (object) '(' + this.Role + (object) ')';
    }

    public override bool IsMutable => false;

    public override object Disassemble(object value, ISessionImplementor session, object owner)
    {
      object keyOfOwner = this.GetKeyOfOwner(owner, session);
      return keyOfOwner == null ? (object) null : this.GetPersister(session).KeyType.Disassemble(keyOfOwner, session, owner);
    }

    public override object Assemble(object cached, ISessionImplementor session, object owner)
    {
      return cached == null ? (object) null : this.ResolveKey(this.GetPersister(session).KeyType.Assemble(cached, session, owner), session, owner);
    }

    private bool IsOwnerVersioned(ISessionImplementor session)
    {
      return this.GetPersister(session).OwnerEntityPersister.IsVersioned;
    }

    private ICollectionPersister GetPersister(ISessionImplementor session)
    {
      return session.Factory.GetCollectionPersister(this.role);
    }

    public override bool IsDirty(object old, object current, ISessionImplementor session)
    {
      return this.IsOwnerVersioned(session) && base.IsDirty(old, current, session);
    }

    public abstract IPersistentCollection Wrap(ISessionImplementor session, object collection);

    public override bool IsAssociationType => true;

    public virtual ForeignKeyDirection ForeignKeyDirection
    {
      get => ForeignKeyDirection.ForeignKeyToParent;
    }

    public override object Hydrate(
      IDataReader rs,
      string[] name,
      ISessionImplementor session,
      object owner)
    {
      return CollectionType.NotNullCollection;
    }

    public override object ResolveIdentifier(object key, ISessionImplementor session, object owner)
    {
      return this.ResolveKey(this.GetKeyOfOwner(owner, session), session, owner);
    }

    private object ResolveKey(object key, ISessionImplementor session, object owner)
    {
      return key != null ? this.GetCollection(key, session, owner) : (object) null;
    }

    public object GetCollection(object key, ISessionImplementor session, object owner)
    {
      ICollectionPersister persister = this.GetPersister(session);
      IPersistenceContext persistenceContext = session.PersistenceContext;
      EntityMode entityMode = session.EntityMode;
      if (entityMode == EntityMode.Xml && !this.isEmbeddedInXML)
        return CollectionType.UnfetchedCollection;
      IPersistentCollection persistentCollection = persistenceContext.LoadContexts.LocateLoadingCollection(persister, key);
      if (persistentCollection == null)
      {
        persistentCollection = persistenceContext.UseUnownedCollection(new CollectionKey(persister, key, entityMode));
        if (persistentCollection == null)
        {
          persistentCollection = this.Instantiate(session, persister, key);
          persistentCollection.Owner = owner;
          persistenceContext.AddUninitializedCollection(persister, persistentCollection, key);
          if (this.InitializeImmediately(entityMode))
            session.InitializeCollection(persistentCollection, false);
          else if (!persister.IsLazy)
            persistenceContext.AddNonLazyCollection(persistentCollection);
          if (this.HasHolder(entityMode))
            session.PersistenceContext.AddCollectionHolder(persistentCollection);
        }
      }
      persistentCollection.Owner = owner;
      return persistentCollection.GetValue();
    }

    public override object SemiResolve(object value, ISessionImplementor session, object owner)
    {
      throw new NotSupportedException("collection mappings may not form part of a property-ref");
    }

    public virtual bool IsArrayType => false;

    public bool UseLHSPrimaryKey => string.IsNullOrEmpty(this.foreignKeyPropertyName);

    public IJoinable GetAssociatedJoinable(ISessionFactoryImplementor factory)
    {
      return (IJoinable) factory.GetCollectionPersister(this.role);
    }

    public string[] GetReferencedColumns(ISessionFactoryImplementor factory)
    {
      return this.GetAssociatedJoinable(factory).KeyColumnNames;
    }

    public string GetAssociatedEntityName(ISessionFactoryImplementor factory)
    {
      try
      {
        IQueryableCollection collectionPersister = (IQueryableCollection) factory.GetCollectionPersister(this.role);
        if (!collectionPersister.ElementType.IsEntityType)
          throw new MappingException("collection was not an association: " + collectionPersister.Role);
        return collectionPersister.ElementPersister.EntityName;
      }
      catch (InvalidCastException ex)
      {
        throw new MappingException("collection role is not queryable " + this.role, (Exception) ex);
      }
    }

    public virtual object InstantiateResult(object original) => this.Instantiate(-1);

    public override object Replace(
      object original,
      object target,
      ISessionImplementor session,
      object owner,
      IDictionary copyCache)
    {
      if (original == null)
        return (object) null;
      if (!NHibernateUtil.IsInitialized(original))
        return target;
      object target1 = target == null || target == original ? this.InstantiateResult(original) : target;
      object original1 = this.ReplaceElements(original, target1, owner, copyCache, session);
      if (original == target)
      {
        this.ReplaceElements(original1, target, owner, copyCache, session);
        original1 = target;
      }
      return original1;
    }

    public virtual object ReplaceElements(
      object original,
      object target,
      object owner,
      IDictionary copyCache,
      ISessionImplementor session)
    {
      object collection = target;
      this.Clear(collection);
      IType elementType = this.GetElementType(session.Factory);
      foreach (object original1 in (IEnumerable) original)
        this.Add(collection, elementType.Replace(original1, (object) null, session, owner, copyCache));
      IPersistentCollection persistentCollection1 = original as IPersistentCollection;
      IPersistentCollection persistentCollection2 = collection as IPersistentCollection;
      if (persistentCollection1 != null && persistentCollection2 != null && !persistentCollection1.IsDirty)
        persistentCollection2.ClearDirty();
      return collection;
    }

    public IType GetElementType(ISessionFactoryImplementor factory)
    {
      return factory.GetCollectionPersister(this.Role).ElementType;
    }

    public override string ToString()
    {
      return this.GetType().FullName + (object) '(' + this.Role + (object) ')';
    }

    protected virtual void Clear(object collection)
    {
      throw new NotImplementedException("CollectionType.Clear was not overridden for type " + this.GetType().FullName);
    }

    protected virtual void Add(object collection, object element)
    {
      throw new NotImplementedException("CollectionType.Add was not overridden for type " + this.GetType().FullName);
    }

    public string LHSPropertyName => this.foreignKeyPropertyName;

    public string RHSUniqueKeyPropertyName => (string) null;

    public bool IsAlwaysDirtyChecked => true;

    public bool IsEmbeddedInXML => this.isEmbeddedInXML;

    public override bool IsDirty(
      object old,
      object current,
      bool[] checkable,
      ISessionImplementor session)
    {
      return this.IsDirty(old, current, session);
    }

    public override bool IsModified(
      object oldHydratedState,
      object currentState,
      bool[] checkable,
      ISessionImplementor session)
    {
      return false;
    }

    public object GetKeyOfOwner(object owner, ISessionImplementor session)
    {
      EntityEntry entry = session.PersistenceContext.GetEntry(owner);
      if (entry == null)
        return (object) null;
      if (this.foreignKeyPropertyName == null)
        return entry.Id;
      object o = entry.LoadedState == null ? entry.Persister.GetPropertyValue(owner, this.foreignKeyPropertyName, session.EntityMode) : entry.GetLoadedValue(this.foreignKeyPropertyName);
      IType keyType = this.GetPersister(session).KeyType;
      if (!keyType.ReturnedClass.IsInstanceOfType(o))
        o = keyType.SemiResolve(entry.GetLoadedValue(this.foreignKeyPropertyName), session, owner);
      return o;
    }

    public virtual object GetIdOfOwnerOrNull(object key, ISessionImplementor session)
    {
      object idOfOwnerOrNull = (object) null;
      if (this.foreignKeyPropertyName == null)
      {
        idOfOwnerOrNull = key;
      }
      else
      {
        IType keyType = this.GetPersister(session).KeyType;
        IEntityPersister ownerEntityPersister = this.GetPersister(session).OwnerEntityPersister;
        if (ownerEntityPersister.GetMappedClass(session.EntityMode).IsAssignableFrom(keyType.ReturnedClass) && keyType.ReturnedClass.IsInstanceOfType(key))
          idOfOwnerOrNull = ownerEntityPersister.GetIdentifier(key, session.EntityMode);
      }
      return idOfOwnerOrNull;
    }

    public abstract object Instantiate(int anticipatedSize);

    public string GetOnCondition(
      string alias,
      ISessionFactoryImplementor factory,
      IDictionary<string, NHibernate.IFilter> enabledFilters)
    {
      return this.GetAssociatedJoinable(factory).FilterFragment(alias, enabledFilters);
    }

    public override object FromXMLNode(XmlNode xml, IMapping factory) => (object) xml;

    public override void SetToXMLNode(
      XmlNode node,
      object value,
      ISessionFactoryImplementor factory)
    {
      if (!this.isEmbeddedInXML)
        return;
      AbstractType.ReplaceNode(node, (XmlNode) value);
    }

    public override bool[] ToColumnNullness(object value, IMapping mapping)
    {
      return ArrayHelper.EmptyBoolArray;
    }

    public override int Compare(object x, object y, EntityMode? entityMode) => 0;

    public virtual bool Contains(
      object collection,
      object childObject,
      ISessionImplementor session)
    {
      foreach (object obj in this.GetElementsIterator(collection, session))
      {
        object entity = obj;
        if (entity.IsProxy())
        {
          ILazyInitializer hibernateLazyInitializer = (entity as INHibernateProxy).HibernateLazyInitializer;
          if (!hibernateLazyInitializer.IsUninitialized)
            entity = hibernateLazyInitializer.GetImplementation();
        }
        if (entity == childObject)
          return true;
      }
      return false;
    }

    public virtual IEnumerable GetElementsIterator(object collection, ISessionImplementor session)
    {
      return this.GetElementsIterator(collection);
    }

    public virtual IEnumerable GetElementsIterator(object collection) => (IEnumerable) collection;

    public virtual bool HasHolder(EntityMode entityMode) => false;

    protected internal virtual bool InitializeImmediately(EntityMode entityMode) => false;

    public virtual object IndexOf(object collection, object element)
    {
      throw new NotSupportedException("generic collections don't have indexes");
    }

    protected internal virtual string RenderLoggableString(
      object value,
      ISessionFactoryImplementor factory)
    {
      IList list = (IList) new ArrayList();
      IType elementType = this.GetElementType(factory);
      foreach (object obj in this.GetElementsIterator(value))
        list.Add((object) elementType.ToLoggableString(obj, factory));
      return CollectionPrinter.ToString(list);
    }
  }
}

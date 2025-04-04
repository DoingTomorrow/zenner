// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Collection.ICollectionPersister
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Metadata;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Persister.Collection
{
  public interface ICollectionPersister
  {
    ICacheConcurrencyStrategy Cache { get; }

    ICacheEntryStructure CacheEntryStructure { get; }

    CollectionType CollectionType { get; }

    IType KeyType { get; }

    IType IndexType { get; }

    IType ElementType { get; }

    System.Type ElementClass { get; }

    bool IsPrimitiveArray { get; }

    bool IsArray { get; }

    bool IsOneToMany { get; }

    bool IsManyToMany { get; }

    bool IsLazy { get; }

    bool IsInverse { get; }

    string Role { get; }

    IEntityPersister OwnerEntityPersister { get; }

    IIdentifierGenerator IdentifierGenerator { get; }

    IType IdentifierType { get; }

    string[] CollectionSpaces { get; }

    ICollectionMetadata CollectionMetadata { get; }

    bool CascadeDeleteEnabled { get; }

    bool IsVersioned { get; }

    bool IsMutable { get; }

    string NodeName { get; }

    string ElementNodeName { get; }

    string IndexNodeName { get; }

    ISessionFactoryImplementor Factory { get; }

    bool IsExtraLazy { get; }

    void Initialize(object key, ISessionImplementor session);

    bool HasCache { get; }

    object ReadKey(IDataReader rs, string[] keyAliases, ISessionImplementor session);

    object ReadElement(
      IDataReader rs,
      object owner,
      string[] columnAliases,
      ISessionImplementor session);

    object ReadIndex(IDataReader rs, string[] columnAliases, ISessionImplementor session);

    object ReadIdentifier(IDataReader rs, string columnAlias, ISessionImplementor session);

    string GetManyToManyFilterFragment(string alias, IDictionary<string, NHibernate.IFilter> enabledFilters);

    bool HasIndex { get; }

    void Remove(object id, ISessionImplementor session);

    void Recreate(IPersistentCollection collection, object key, ISessionImplementor session);

    void DeleteRows(IPersistentCollection collection, object key, ISessionImplementor session);

    void UpdateRows(IPersistentCollection collection, object key, ISessionImplementor session);

    void InsertRows(IPersistentCollection collection, object key, ISessionImplementor session);

    bool HasOrphanDelete { get; }

    bool HasOrdering { get; }

    bool HasManyToManyOrdering { get; }

    void PostInstantiate();

    bool IsAffectedByEnabledFilters(ISessionImplementor session);

    string[] GetKeyColumnAliases(string suffix);

    string[] GetIndexColumnAliases(string suffix);

    string[] GetElementColumnAliases(string suffix);

    string GetIdentifierColumnAlias(string suffix);

    int GetSize(object key, ISessionImplementor session);

    bool IndexExists(object key, object index, ISessionImplementor session);

    bool ElementExists(object key, object element, ISessionImplementor session);

    object GetElementByIndex(object key, object index, ISessionImplementor session, object owner);

    object NotFoundObject { get; }
  }
}

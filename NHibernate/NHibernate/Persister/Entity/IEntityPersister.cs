// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Entity.IEntityPersister
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Metadata;
using NHibernate.Tuple.Entity;
using NHibernate.Type;
using System.Collections;

#nullable disable
namespace NHibernate.Persister.Entity
{
  public interface IEntityPersister : IOptimisticCacheSource
  {
    ISessionFactoryImplementor Factory { get; }

    string RootEntityName { get; }

    string EntityName { get; }

    EntityMetamodel EntityMetamodel { get; }

    string[] PropertySpaces { get; }

    string[] QuerySpaces { get; }

    bool IsMutable { get; }

    bool IsInherited { get; }

    bool IsIdentifierAssignedByInsert { get; }

    new bool IsVersioned { get; }

    IVersionType VersionType { get; }

    int VersionProperty { get; }

    int[] NaturalIdentifierProperties { get; }

    IIdentifierGenerator IdentifierGenerator { get; }

    IType[] PropertyTypes { get; }

    string[] PropertyNames { get; }

    bool[] PropertyInsertability { get; }

    ValueInclusion[] PropertyInsertGenerationInclusions { get; }

    ValueInclusion[] PropertyUpdateGenerationInclusions { get; }

    bool[] PropertyCheckability { get; }

    bool[] PropertyNullability { get; }

    bool[] PropertyVersionability { get; }

    bool[] PropertyLaziness { get; }

    CascadeStyle[] PropertyCascadeStyles { get; }

    IType IdentifierType { get; }

    string IdentifierPropertyName { get; }

    bool IsCacheInvalidationRequired { get; }

    bool IsLazyPropertiesCacheable { get; }

    ICacheConcurrencyStrategy Cache { get; }

    ICacheEntryStructure CacheEntryStructure { get; }

    IClassMetadata ClassMetadata { get; }

    bool IsBatchLoadable { get; }

    bool IsSelectBeforeUpdateRequired { get; }

    bool IsVersionPropertyGenerated { get; }

    void PostInstantiate();

    bool IsSubclassEntityName(string entityName);

    bool HasProxy { get; }

    bool HasCollections { get; }

    bool HasMutableProperties { get; }

    bool HasSubselectLoadableCollections { get; }

    bool HasCascades { get; }

    IType GetPropertyType(string propertyName);

    int[] FindDirty(
      object[] currentState,
      object[] previousState,
      object entity,
      ISessionImplementor session);

    int[] FindModified(object[] old, object[] current, object entity, ISessionImplementor session);

    bool HasIdentifierProperty { get; }

    bool CanExtractIdOutOfEntity { get; }

    bool HasNaturalIdentifier { get; }

    object[] GetNaturalIdentifierSnapshot(object id, ISessionImplementor session);

    bool HasLazyProperties { get; }

    object Load(object id, object optionalObject, LockMode lockMode, ISessionImplementor session);

    void Lock(
      object id,
      object version,
      object obj,
      LockMode lockMode,
      ISessionImplementor session);

    void Insert(object id, object[] fields, object obj, ISessionImplementor session);

    object Insert(object[] fields, object obj, ISessionImplementor session);

    void Delete(object id, object version, object obj, ISessionImplementor session);

    void Update(
      object id,
      object[] fields,
      int[] dirtyFields,
      bool hasDirtyCollection,
      object[] oldFields,
      object oldVersion,
      object obj,
      object rowId,
      ISessionImplementor session);

    bool[] PropertyUpdateability { get; }

    bool HasCache { get; }

    object[] GetDatabaseSnapshot(object id, ISessionImplementor session);

    object GetCurrentVersion(object id, ISessionImplementor session);

    object ForceVersionIncrement(object id, object currentVersion, ISessionImplementor session);

    EntityMode? GuessEntityMode(object obj);

    bool IsInstrumented(EntityMode entityMode);

    bool HasInsertGeneratedProperties { get; }

    bool HasUpdateGeneratedProperties { get; }

    void AfterInitialize(
      object entity,
      bool lazyPropertiesAreUnfetched,
      ISessionImplementor session);

    void AfterReassociate(object entity, ISessionImplementor session);

    object CreateProxy(object id, ISessionImplementor session);

    bool? IsTransient(object obj, ISessionImplementor session);

    object[] GetPropertyValuesToInsert(
      object obj,
      IDictionary mergeMap,
      ISessionImplementor session);

    void ProcessInsertGeneratedProperties(
      object id,
      object entity,
      object[] state,
      ISessionImplementor session);

    void ProcessUpdateGeneratedProperties(
      object id,
      object entity,
      object[] state,
      ISessionImplementor session);

    System.Type GetMappedClass(EntityMode entityMode);

    bool ImplementsLifecycle(EntityMode entityMode);

    bool ImplementsValidatable(EntityMode entityMode);

    System.Type GetConcreteProxyClass(EntityMode entityMode);

    void SetPropertyValues(object obj, object[] values, EntityMode entityMode);

    void SetPropertyValue(object obj, int i, object value, EntityMode entityMode);

    object[] GetPropertyValues(object obj, EntityMode entityMode);

    object GetPropertyValue(object obj, int i, EntityMode entityMode);

    object GetPropertyValue(object obj, string name, EntityMode entityMode);

    object GetIdentifier(object obj, EntityMode entityMode);

    void SetIdentifier(object obj, object id, EntityMode entityMode);

    object GetVersion(object obj, EntityMode entityMode);

    object Instantiate(object id, EntityMode entityMode);

    bool IsInstance(object entity, EntityMode entityMode);

    bool HasUninitializedLazyProperties(object obj, EntityMode entityMode);

    void ResetIdentifier(
      object entity,
      object currentId,
      object currentVersion,
      EntityMode entityMode);

    IEntityPersister GetSubclassEntityPersister(
      object instance,
      ISessionFactoryImplementor factory,
      EntityMode entityMode);

    bool? IsUnsavedVersion(object version);
  }
}

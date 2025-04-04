// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Loader
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using Iesi.Collections.Generic;
using NHibernate.AdoNet;
using NHibernate.Cache;
using NHibernate.Driver;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Exceptions;
using NHibernate.Hql.Util;
using NHibernate.Impl;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

#nullable disable
namespace NHibernate.Loader
{
  public abstract class Loader
  {
    private static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof (NHibernate.Loader.Loader));
    private readonly ISessionFactoryImplementor _factory;
    private readonly SessionFactoryHelper _helper;
    private ColumnNameCache _columnNameCache;

    protected Loader(ISessionFactoryImplementor factory)
    {
      this._factory = factory;
      this._helper = new SessionFactoryHelper(factory);
    }

    protected SessionFactoryHelper Helper => this._helper;

    protected virtual bool[] EntityEagerPropertyFetches => (bool[]) null;

    protected virtual int[] Owners => (int[]) null;

    protected virtual EntityType[] OwnerAssociationTypes => (EntityType[]) null;

    protected virtual int[] CollectionOwners => (int[]) null;

    protected virtual bool IsSingleRowLoader => false;

    public virtual bool IsSubselectLoadingEnabled => false;

    protected abstract IEntityAliases[] EntityAliases { get; }

    protected abstract ICollectionAliases[] CollectionAliases { get; }

    public ISessionFactoryImplementor Factory => this._factory;

    public abstract SqlString SqlString { get; }

    public abstract ILoadable[] EntityPersisters { get; }

    protected virtual ICollectionPersister[] CollectionPersisters => (ICollectionPersister[]) null;

    public abstract LockMode[] GetLockModes(IDictionary<string, LockMode> lockModes);

    protected virtual SqlString ApplyLocks(
      SqlString sql,
      IDictionary<string, LockMode> lockModes,
      NHibernate.Dialect.Dialect dialect)
    {
      return sql;
    }

    protected virtual bool UpgradeLocks() => false;

    protected virtual string[] Aliases => (string[]) null;

    protected virtual SqlString PreprocessSQL(
      SqlString sql,
      QueryParameters parameters,
      NHibernate.Dialect.Dialect dialect)
    {
      sql = this.ApplyLocks(sql, parameters.LockModes, dialect);
      return !this.Factory.Settings.IsCommentsEnabled ? sql : NHibernate.Loader.Loader.PrependComment(sql, parameters);
    }

    private static SqlString PrependComment(SqlString sql, QueryParameters parameters)
    {
      string comment = parameters.Comment;
      return string.IsNullOrEmpty(comment) ? sql : sql.Insert(0, "/* " + comment + " */");
    }

    private IList DoQueryAndInitializeNonLazyCollections(
      ISessionImplementor session,
      QueryParameters queryParameters,
      bool returnProxies)
    {
      IPersistenceContext persistenceContext = session.PersistenceContext;
      bool defaultReadOnly = persistenceContext.DefaultReadOnly;
      if (queryParameters.IsReadOnlyInitialized)
        persistenceContext.DefaultReadOnly = queryParameters.ReadOnly;
      else
        queryParameters.ReadOnly = persistenceContext.DefaultReadOnly;
      persistenceContext.BeforeLoad();
      IList list;
      try
      {
        try
        {
          list = this.DoQuery(session, queryParameters, returnProxies);
        }
        finally
        {
          persistenceContext.AfterLoad();
        }
        persistenceContext.InitializeNonLazyCollections();
      }
      finally
      {
        persistenceContext.DefaultReadOnly = defaultReadOnly;
      }
      return list;
    }

    protected object LoadSingleRow(
      IDataReader resultSet,
      ISessionImplementor session,
      QueryParameters queryParameters,
      bool returnProxies)
    {
      int length = this.EntityPersisters.Length;
      IList hydratedObjects = length == 0 ? (IList) null : (IList) new System.Collections.Generic.List<object>(length);
      object rowFromResultSet;
      try
      {
        rowFromResultSet = this.GetRowFromResultSet(resultSet, session, queryParameters, this.GetLockModes(queryParameters.LockModes), (EntityKey) null, hydratedObjects, new EntityKey[length], returnProxies);
      }
      catch (HibernateException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, ex, "could not read next row of results", this.SqlString, queryParameters.PositionalParameterValues, queryParameters.NamedParameters);
      }
      this.InitializeEntitiesAndCollections(hydratedObjects, (object) resultSet, session, queryParameters.IsReadOnly(session));
      session.PersistenceContext.InitializeNonLazyCollections();
      return rowFromResultSet;
    }

    internal static EntityKey GetOptionalObjectKey(
      QueryParameters queryParameters,
      ISessionImplementor session)
    {
      object optionalObject = queryParameters.OptionalObject;
      object optionalId = queryParameters.OptionalId;
      string optionalEntityName = queryParameters.OptionalEntityName;
      return optionalObject != null && !string.IsNullOrEmpty(optionalEntityName) ? new EntityKey(optionalId, session.GetEntityPersister(optionalEntityName, optionalObject), session.EntityMode) : (EntityKey) null;
    }

    internal object GetRowFromResultSet(
      IDataReader resultSet,
      ISessionImplementor session,
      QueryParameters queryParameters,
      LockMode[] lockModeArray,
      EntityKey optionalObjectKey,
      IList hydratedObjects,
      EntityKey[] keys,
      bool returnProxies)
    {
      ILoadable[] entityPersisters = this.EntityPersisters;
      int length = entityPersisters.Length;
      for (int i = 0; i < length; ++i)
        keys[i] = this.GetKeyFromResultSet(i, (IEntityPersister) entityPersisters[i], i == length - 1 ? queryParameters.OptionalId : (object) null, resultSet, session);
      this.RegisterNonExists(keys, session);
      object[] row = this.GetRow(resultSet, entityPersisters, keys, queryParameters.OptionalObject, optionalObjectKey, lockModeArray, hydratedObjects, session);
      this.ReadCollectionElements(row, resultSet, session);
      if (returnProxies)
      {
        for (int index = 0; index < length; ++index)
        {
          object obj1 = row[index];
          object obj2 = session.PersistenceContext.ProxyFor((IEntityPersister) entityPersisters[index], keys[index], obj1);
          if (obj1 != obj2)
          {
            ((INHibernateProxy) obj2).HibernateLazyInitializer.SetImplementation(obj1);
            row[index] = obj2;
          }
        }
      }
      return this.GetResultColumnOrRow(row, queryParameters.ResultTransformer, resultSet, session);
    }

    private void ReadCollectionElements(
      object[] row,
      IDataReader resultSet,
      ISessionImplementor session)
    {
      ICollectionPersister[] collectionPersisters = this.CollectionPersisters;
      if (collectionPersisters == null)
        return;
      ICollectionAliases[] collectionAliases = this.CollectionAliases;
      int[] collectionOwners = this.CollectionOwners;
      for (int index = 0; index < collectionPersisters.Length; ++index)
      {
        object obj = collectionOwners != null && collectionOwners[index] > -1 ? row[collectionOwners[index]] : (object) null;
        ICollectionPersister persister = collectionPersisters[index];
        object keyOfOwner = obj != null ? persister.CollectionType.GetKeyOfOwner(obj, session) : (object) null;
        NHibernate.Loader.Loader.ReadCollectionElement(obj, keyOfOwner, persister, collectionAliases[index], resultSet, session);
      }
    }

    private IList DoQuery(
      ISessionImplementor session,
      QueryParameters queryParameters,
      bool returnProxies)
    {
      RowSelection rowSelection = queryParameters.RowSelection;
      int num1 = NHibernate.Loader.Loader.HasMaxRows(rowSelection) ? rowSelection.MaxRows : int.MaxValue;
      int length = this.EntityPersisters.Length;
      System.Collections.Generic.List<object> hydratedObjects = length == 0 ? (System.Collections.Generic.List<object>) null : new System.Collections.Generic.List<object>(length * 10);
      IDbCommand dbCommand = this.PrepareQueryCommand(queryParameters, false, session);
      IDataReader resultSet = this.GetResultSet(dbCommand, queryParameters.HasAutoDiscoverScalarTypes, queryParameters.Callable, rowSelection, session);
      LockMode[] lockModes = this.GetLockModes(queryParameters.LockModes);
      EntityKey optionalObjectKey = NHibernate.Loader.Loader.GetOptionalObjectKey(queryParameters, session);
      bool subselectLoadingEnabled = this.IsSubselectLoadingEnabled;
      System.Collections.Generic.List<EntityKey[]> keys1 = subselectLoadingEnabled ? new System.Collections.Generic.List<EntityKey[]>() : (System.Collections.Generic.List<EntityKey[]>) null;
      IList list = (IList) new System.Collections.Generic.List<object>();
      try
      {
        this.HandleEmptyCollections(queryParameters.CollectionKeys, (object) resultSet, session);
        EntityKey[] keys2 = new EntityKey[length];
        if (NHibernate.Loader.Loader.Log.IsDebugEnabled)
          NHibernate.Loader.Loader.Log.Debug((object) "processing result set");
        int num2;
        for (num2 = 0; num2 < num1 && resultSet.Read(); ++num2)
        {
          if (NHibernate.Loader.Loader.Log.IsDebugEnabled)
            NHibernate.Loader.Loader.Log.Debug((object) ("result set row: " + (object) num2));
          object rowFromResultSet = this.GetRowFromResultSet(resultSet, session, queryParameters, lockModes, optionalObjectKey, (IList) hydratedObjects, keys2, returnProxies);
          list.Add(rowFromResultSet);
          if (subselectLoadingEnabled)
          {
            keys1.Add(keys2);
            keys2 = new EntityKey[length];
          }
        }
        if (NHibernate.Loader.Loader.Log.IsDebugEnabled)
          NHibernate.Loader.Loader.Log.Debug((object) string.Format("done processing result set ({0} rows)", (object) num2));
      }
      catch (Exception ex)
      {
        ex.Data[(object) "actual-sql-query"] = (object) dbCommand.CommandText;
        throw;
      }
      finally
      {
        session.Batcher.CloseCommand(dbCommand, resultSet);
      }
      this.InitializeEntitiesAndCollections((IList) hydratedObjects, (object) resultSet, session, queryParameters.IsReadOnly(session));
      if (subselectLoadingEnabled)
        this.CreateSubselects((IList<EntityKey[]>) keys1, queryParameters, session);
      return list;
    }

    protected bool HasSubselectLoadableCollections()
    {
      foreach (IEntityPersister entityPersister in this.EntityPersisters)
      {
        if (entityPersister.HasSubselectLoadableCollections)
          return true;
      }
      return false;
    }

    private static ISet<EntityKey>[] Transpose(IList<EntityKey[]> keys)
    {
      ISet<EntityKey>[] setArray = new ISet<EntityKey>[keys[0].Length];
      for (int index1 = 0; index1 < setArray.Length; ++index1)
      {
        setArray[index1] = (ISet<EntityKey>) new HashedSet<EntityKey>();
        for (int index2 = 0; index2 < keys.Count; ++index2)
        {
          EntityKey o = keys[index2][index1];
          if (o != null)
            setArray[index1].Add(o);
        }
      }
      return setArray;
    }

    internal void CreateSubselects(
      IList<EntityKey[]> keys,
      QueryParameters queryParameters,
      ISessionImplementor session)
    {
      if (keys.Count <= 1)
        return;
      SubselectFetch[] array = this.CreateSubselects(keys, queryParameters).ToArray<SubselectFetch>();
      foreach (EntityKey[] key in (IEnumerable<EntityKey[]>) keys)
      {
        for (int index = 0; index < key.Length; ++index)
        {
          if (key[index] != null && array[index] != null)
            session.PersistenceContext.BatchFetchQueue.AddSubselect(key[index], array[index]);
        }
      }
    }

    private IEnumerable<SubselectFetch> CreateSubselects(
      IList<EntityKey[]> keys,
      QueryParameters queryParameters)
    {
      ISet<EntityKey>[] keySets = NHibernate.Loader.Loader.Transpose(keys);
      ILoadable[] loadables = this.EntityPersisters;
      string[] aliases = this.Aliases;
      for (int i = 0; i < loadables.Length; ++i)
      {
        if (loadables[i].HasSubselectLoadableCollections)
          yield return new SubselectFetch(aliases[i], loadables[i], queryParameters, keySets[i]);
        else
          yield return (SubselectFetch) null;
      }
    }

    internal void InitializeEntitiesAndCollections(
      IList hydratedObjects,
      object resultSetId,
      ISessionImplementor session,
      bool readOnly)
    {
      ICollectionPersister[] collectionPersisters = this.CollectionPersisters;
      if (collectionPersisters != null)
      {
        for (int index = 0; index < collectionPersisters.Length; ++index)
        {
          if (collectionPersisters[index].IsArray)
            NHibernate.Loader.Loader.EndCollectionLoad(resultSetId, session, collectionPersisters[index]);
        }
      }
      PreLoadEvent preLoadEvent;
      PostLoadEvent postLoadEvent;
      if (session.IsEventSource)
      {
        IEventSource source = (IEventSource) session;
        preLoadEvent = new PreLoadEvent(source);
        postLoadEvent = new PostLoadEvent(source);
      }
      else
      {
        preLoadEvent = (PreLoadEvent) null;
        postLoadEvent = (PostLoadEvent) null;
      }
      if (hydratedObjects != null)
      {
        int count = hydratedObjects.Count;
        if (NHibernate.Loader.Loader.Log.IsDebugEnabled)
          NHibernate.Loader.Loader.Log.Debug((object) string.Format("total objects hydrated: {0}", (object) count));
        for (int index = 0; index < count; ++index)
          TwoPhaseLoad.InitializeEntity(hydratedObjects[index], readOnly, session, preLoadEvent, postLoadEvent);
      }
      if (collectionPersisters == null)
        return;
      for (int index = 0; index < collectionPersisters.Length; ++index)
      {
        if (!collectionPersisters[index].IsArray)
          NHibernate.Loader.Loader.EndCollectionLoad(resultSetId, session, collectionPersisters[index]);
      }
    }

    private static void EndCollectionLoad(
      object resultSetId,
      ISessionImplementor session,
      ICollectionPersister collectionPersister)
    {
      session.PersistenceContext.LoadContexts.GetCollectionLoadContext((IDataReader) resultSetId).EndLoadingCollections(collectionPersister);
    }

    public virtual IList GetResultList(IList results, IResultTransformer resultTransformer)
    {
      return results;
    }

    protected virtual object GetResultColumnOrRow(
      object[] row,
      IResultTransformer resultTransformer,
      IDataReader rs,
      ISessionImplementor session)
    {
      return (object) row;
    }

    private void RegisterNonExists(EntityKey[] keys, ISessionImplementor session)
    {
      int[] owners = this.Owners;
      if (owners == null)
        return;
      EntityType[] associationTypes = this.OwnerAssociationTypes;
      for (int index1 = 0; index1 < keys.Length; ++index1)
      {
        int index2 = owners[index1];
        if (index2 > -1)
        {
          EntityKey key = keys[index2];
          if (keys[index1] == null && key != null && associationTypes != null && associationTypes[index1] != null && associationTypes[index1].IsOneToOne)
            session.PersistenceContext.AddNullProperty(key, associationTypes[index1].PropertyName);
        }
      }
    }

    private static void ReadCollectionElement(
      object optionalOwner,
      object optionalKey,
      ICollectionPersister persister,
      ICollectionAliases descriptor,
      IDataReader rs,
      ISessionImplementor session)
    {
      IPersistenceContext persistenceContext = session.PersistenceContext;
      object obj = persister.ReadKey(rs, descriptor.SuffixedKeyAliases, session);
      if (obj != null)
      {
        if (NHibernate.Loader.Loader.Log.IsDebugEnabled)
          NHibernate.Loader.Loader.Log.Debug((object) ("found row of collection: " + MessageHelper.InfoString(persister, obj)));
        object owner = optionalOwner;
        if (owner == null)
          owner = persistenceContext.GetCollectionOwner(obj, persister);
        persistenceContext.LoadContexts.GetCollectionLoadContext(rs).GetLoadingCollection(persister, obj)?.ReadFrom(rs, persister, descriptor, owner);
      }
      else
      {
        if (optionalKey == null)
          return;
        if (NHibernate.Loader.Loader.Log.IsDebugEnabled)
          NHibernate.Loader.Loader.Log.Debug((object) ("result set contains (possibly empty) collection: " + MessageHelper.InfoString(persister, optionalKey)));
        persistenceContext.LoadContexts.GetCollectionLoadContext(rs).GetLoadingCollection(persister, optionalKey);
      }
    }

    internal void HandleEmptyCollections(
      object[] keys,
      object resultSetId,
      ISessionImplementor session)
    {
      if (keys == null)
        return;
      ICollectionPersister[] collectionPersisters = this.CollectionPersisters;
      for (int index1 = 0; index1 < collectionPersisters.Length; ++index1)
      {
        for (int index2 = 0; index2 < keys.Length; ++index2)
        {
          if (NHibernate.Loader.Loader.Log.IsDebugEnabled)
            NHibernate.Loader.Loader.Log.Debug((object) ("result set contains (possibly empty) collection: " + MessageHelper.InfoString(collectionPersisters[index1], keys[index2])));
          session.PersistenceContext.LoadContexts.GetCollectionLoadContext((IDataReader) resultSetId).GetLoadingCollection(collectionPersisters[index1], keys[index2]);
        }
      }
    }

    private EntityKey GetKeyFromResultSet(
      int i,
      IEntityPersister persister,
      object id,
      IDataReader rs,
      ISessionImplementor session)
    {
      object obj;
      if (this.IsSingleRowLoader && id != null)
      {
        obj = id;
      }
      else
      {
        IType identifierType = persister.IdentifierType;
        obj = identifierType.NullSafeGet(rs, this.EntityAliases[i].SuffixedKeyAliases, session, (object) null);
        if (id != null && obj != null && identifierType.IsEqual(id, obj, session.EntityMode, this._factory))
          obj = id;
      }
      return obj != null ? new EntityKey(obj, persister, session.EntityMode) : (EntityKey) null;
    }

    private void CheckVersion(
      int i,
      IEntityPersister persister,
      object id,
      object entity,
      IDataReader rs,
      ISessionImplementor session)
    {
      object version = session.PersistenceContext.GetEntry(entity).Version;
      if (version == null)
        return;
      IVersionType versionType = persister.VersionType;
      object y = versionType.NullSafeGet(rs, this.EntityAliases[i].SuffixedVersionAliases, session, (object) null);
      if (!versionType.IsEqual(version, y))
      {
        if (session.Factory.Statistics.IsStatisticsEnabled)
          session.Factory.StatisticsImplementor.OptimisticFailure(persister.EntityName);
        throw new StaleObjectStateException(persister.EntityName, id);
      }
    }

    private object[] GetRow(
      IDataReader rs,
      ILoadable[] persisters,
      EntityKey[] keys,
      object optionalObject,
      EntityKey optionalObjectKey,
      LockMode[] lockModes,
      IList hydratedObjects,
      ISessionImplementor session)
    {
      int length = persisters.Length;
      IEntityAliases[] entityAliases = this.EntityAliases;
      if (NHibernate.Loader.Loader.Log.IsDebugEnabled)
        NHibernate.Loader.Loader.Log.Debug((object) ("result row: " + StringHelper.ToString((object[]) keys)));
      object[] row = new object[length];
      for (int i = 0; i < length; ++i)
      {
        object obj = (object) null;
        EntityKey key = keys[i];
        if (keys[i] != null)
        {
          obj = session.GetEntityUsingInterceptor(key);
          if (obj != null)
            this.InstanceAlreadyLoaded(rs, i, (IEntityPersister) persisters[i], key, obj, lockModes[i], session);
          else
            obj = this.InstanceNotYetLoaded(rs, i, persisters[i], key, lockModes[i], entityAliases[i].RowIdAlias, optionalObjectKey, optionalObject, hydratedObjects, session);
        }
        row[i] = obj;
      }
      return row;
    }

    private void InstanceAlreadyLoaded(
      IDataReader rs,
      int i,
      IEntityPersister persister,
      EntityKey key,
      object obj,
      LockMode lockMode,
      ISessionImplementor session)
    {
      if (!persister.IsInstance(obj, session.EntityMode))
        throw new WrongClassException(string.Format("loading object was of wrong class [{0}]", (object) obj.GetType().FullName), key.Identifier, persister.EntityName);
      if (LockMode.None == lockMode || !this.UpgradeLocks())
        return;
      EntityEntry entry = session.PersistenceContext.GetEntry(obj);
      if (!persister.IsVersioned || !entry.LockMode.LessThan(lockMode))
        return;
      this.CheckVersion(i, persister, key.Identifier, obj, rs, session);
      entry.LockMode = lockMode;
    }

    private object InstanceNotYetLoaded(
      IDataReader dr,
      int i,
      ILoadable persister,
      EntityKey key,
      LockMode lockMode,
      string rowIdAlias,
      EntityKey optionalObjectKey,
      object optionalObject,
      IList hydratedObjects,
      ISessionImplementor session)
    {
      string instanceClass = this.GetInstanceClass(dr, i, persister, key.Identifier, session);
      object obj = optionalObjectKey == null || !key.Equals((object) optionalObjectKey) ? session.Instantiate(instanceClass, key.Identifier) : optionalObject;
      LockMode lockMode1 = lockMode == LockMode.None ? LockMode.Read : lockMode;
      this.LoadFromResultSet(dr, i, obj, instanceClass, key, rowIdAlias, lockMode1, persister, session);
      hydratedObjects.Add(obj);
      return obj;
    }

    private bool IsEagerPropertyFetchEnabled(int i)
    {
      bool[] eagerPropertyFetches = this.EntityEagerPropertyFetches;
      return eagerPropertyFetches != null && eagerPropertyFetches[i];
    }

    private void LoadFromResultSet(
      IDataReader rs,
      int i,
      object obj,
      string instanceClass,
      EntityKey key,
      string rowIdAlias,
      LockMode lockMode,
      ILoadable rootPersister,
      ISessionImplementor session)
    {
      object identifier = key.Identifier;
      ILoadable entityPersister = (ILoadable) this.Factory.GetEntityPersister(instanceClass);
      if (NHibernate.Loader.Loader.Log.IsDebugEnabled)
        NHibernate.Loader.Loader.Log.Debug((object) ("Initializing object from DataReader: " + MessageHelper.InfoString((IEntityPersister) entityPersister, identifier)));
      bool allProperties = this.IsEagerPropertyFetchEnabled(i);
      TwoPhaseLoad.AddUninitializedEntity(key, obj, (IEntityPersister) entityPersister, lockMode, !allProperties, session);
      string[][] suffixedPropertyColumns = entityPersister == rootPersister ? this.EntityAliases[i].SuffixedPropertyAliases : this.EntityAliases[i].GetSuffixedPropertyAliases(entityPersister);
      object[] values = entityPersister.Hydrate(rs, identifier, obj, rootPersister, suffixedPropertyColumns, allProperties, session);
      object r = entityPersister.HasRowId ? rs[rowIdAlias] : (object) null;
      IAssociationType[] associationTypes = (IAssociationType[]) this.OwnerAssociationTypes;
      if (associationTypes != null && associationTypes[i] != null)
      {
        string uniqueKeyPropertyName = associationTypes[i].RHSUniqueKeyPropertyName;
        if (uniqueKeyPropertyName != null)
        {
          int propertyIndex = ((IUniqueKeyLoadable) entityPersister).GetPropertyIndex(uniqueKeyPropertyName);
          IType propertyType = entityPersister.PropertyTypes[propertyIndex];
          EntityUniqueKey euk = new EntityUniqueKey(rootPersister.EntityName, uniqueKeyPropertyName, propertyType.SemiResolve(values[propertyIndex], session, obj), propertyType, session.EntityMode, session.Factory);
          session.PersistenceContext.AddEntity(euk, obj);
        }
      }
      TwoPhaseLoad.PostHydrate((IEntityPersister) entityPersister, identifier, values, r, obj, lockMode, !allProperties, session);
    }

    private string GetInstanceClass(
      IDataReader rs,
      int i,
      ILoadable persister,
      object id,
      ISessionImplementor session)
    {
      if (!persister.HasSubclasses)
        return persister.EntityName;
      object obj = persister.DiscriminatorType.NullSafeGet(rs, this.EntityAliases[i].SuffixedDiscriminatorAlias, session, (object) null);
      return persister.GetSubclassForDiscriminatorValue(obj) ?? throw new WrongClassException(string.Format("Discriminator was: '{0}'", obj), id, persister.EntityName);
    }

    internal static void Advance(IDataReader rs, RowSelection selection)
    {
      int firstRow = NHibernate.Loader.Loader.GetFirstRow(selection);
      if (firstRow == 0)
        return;
      for (int index = 0; index < firstRow; ++index)
        rs.Read();
    }

    internal static bool HasMaxRows(RowSelection selection)
    {
      return selection != null && selection.MaxRows != RowSelection.NoValue;
    }

    private static bool HasOffset(RowSelection selection)
    {
      return selection != null && selection.FirstRow != RowSelection.NoValue;
    }

    internal static int GetFirstRow(RowSelection selection)
    {
      return selection == null || !selection.DefinesLimits || selection.FirstRow <= 0 ? 0 : selection.FirstRow;
    }

    internal bool UseLimit(RowSelection selection, NHibernate.Dialect.Dialect dialect)
    {
      if (!dialect.SupportsLimit)
        return false;
      return NHibernate.Loader.Loader.HasMaxRows(selection) || NHibernate.Loader.Loader.HasOffset(selection);
    }

    internal static int? GetOffsetUsingDialect(RowSelection selection, NHibernate.Dialect.Dialect dialect)
    {
      int firstRow = NHibernate.Loader.Loader.GetFirstRow(selection);
      return firstRow == 0 ? new int?() : new int?(dialect.GetOffsetValue(firstRow));
    }

    internal static int? GetLimitUsingDialect(RowSelection selection, NHibernate.Dialect.Dialect dialect)
    {
      return selection == null || selection.MaxRows == RowSelection.NoValue ? new int?() : new int?(dialect.GetLimitValue(NHibernate.Loader.Loader.GetFirstRow(selection), selection.MaxRows));
    }

    protected internal virtual IDbCommand PrepareQueryCommand(
      QueryParameters queryParameters,
      bool scroll,
      ISessionImplementor session)
    {
      ISqlCommand sqlCommand = this.CreateSqlCommand(queryParameters, session);
      SqlString query = sqlCommand.Query;
      sqlCommand.ResetParametersIndexesForTheCommand(0);
      IDbCommand dbCommand = session.Batcher.PrepareQueryCommand(CommandType.Text, query, sqlCommand.ParameterTypes);
      try
      {
        RowSelection rowSelection = queryParameters.RowSelection;
        if (rowSelection != null && rowSelection.Timeout != RowSelection.NoValue)
          dbCommand.CommandTimeout = rowSelection.Timeout;
        sqlCommand.Bind(dbCommand, session);
        IDriver driver = this._factory.ConnectionProvider.Driver;
        driver.RemoveUnusedCommandParameters(dbCommand, query);
        driver.ExpandQueryParameters(dbCommand, query);
      }
      catch (HibernateException ex)
      {
        session.Batcher.CloseCommand(dbCommand, (IDataReader) null);
        throw;
      }
      catch (Exception ex)
      {
        session.Batcher.CloseCommand(dbCommand, (IDataReader) null);
        ADOExceptionReporter.LogExceptions(ex);
        throw;
      }
      return dbCommand;
    }

    internal static int GetMaxOrLimit(NHibernate.Dialect.Dialect dialect, RowSelection selection)
    {
      int firstRow = NHibernate.Loader.Loader.GetFirstRow(selection);
      int maxRows = selection.MaxRows;
      return maxRows == RowSelection.NoValue ? int.MaxValue : dialect.GetLimitValue(firstRow, maxRows);
    }

    protected IDataReader GetResultSet(
      IDbCommand st,
      bool autoDiscoverTypes,
      bool callable,
      RowSelection selection,
      ISessionImplementor session)
    {
      IDataReader resultSet = (IDataReader) null;
      try
      {
        NHibernate.Loader.Loader.Log.Info((object) st.CommandText);
        resultSet = session.Batcher.ExecuteReader(st);
        if (session.Factory.Settings.IsWrapResultSetsEnabled)
          resultSet = this.WrapResultSet(resultSet);
        NHibernate.Dialect.Dialect dialect = session.Factory.Dialect;
        if (!dialect.SupportsLimitOffset || !this.UseLimit(selection, dialect))
          NHibernate.Loader.Loader.Advance(resultSet, selection);
        if (autoDiscoverTypes)
          this.AutoDiscoverTypes(resultSet);
        return resultSet;
      }
      catch (Exception ex)
      {
        ADOExceptionReporter.LogExceptions(ex);
        session.Batcher.CloseCommand(st, resultSet);
        throw;
      }
    }

    protected virtual void AutoDiscoverTypes(IDataReader rs)
    {
      throw new AssertionFailure("Auto discover types not supported in this loader");
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    private IDataReader WrapResultSet(IDataReader rs)
    {
      try
      {
        NHibernate.Loader.Loader.Log.Debug((object) ("Wrapping result set [" + (object) rs + "]"));
        return (IDataReader) new ResultSetWrapper(rs, this.RetreiveColumnNameToIndexCache(rs));
      }
      catch (Exception ex)
      {
        NHibernate.Loader.Loader.Log.Info((object) "Error wrapping result set", ex);
        return rs;
      }
    }

    private ColumnNameCache RetreiveColumnNameToIndexCache(IDataReader rs)
    {
      if (this._columnNameCache == null)
      {
        NHibernate.Loader.Loader.Log.Debug((object) "Building columnName->columnIndex cache");
        this._columnNameCache = new ColumnNameCache(rs.GetSchemaTable().Rows.Count);
      }
      return this._columnNameCache;
    }

    protected IList LoadEntity(
      ISessionImplementor session,
      object id,
      IType identifierType,
      object optionalObject,
      string optionalEntityName,
      object optionalIdentifier,
      IEntityPersister persister)
    {
      if (NHibernate.Loader.Loader.Log.IsDebugEnabled)
        NHibernate.Loader.Loader.Log.Debug((object) ("loading entity: " + MessageHelper.InfoString(persister, id, identifierType, this.Factory)));
      IList list;
      try
      {
        QueryParameters queryParameters = new QueryParameters(new IType[1]
        {
          identifierType
        }, new object[1]{ id }, optionalObject, optionalEntityName, optionalIdentifier);
        list = this.DoQueryAndInitializeNonLazyCollections(session, queryParameters, false);
      }
      catch (HibernateException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        ILoadable[] entityPersisters = this.EntityPersisters;
        throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, ex, "could not load an entity: " + MessageHelper.InfoString((IEntityPersister) entityPersisters[entityPersisters.Length - 1], id, identifierType, this.Factory), this.SqlString);
      }
      NHibernate.Loader.Loader.Log.Debug((object) "done entity load");
      return list;
    }

    protected IList LoadEntity(
      ISessionImplementor session,
      object key,
      object index,
      IType keyType,
      IType indexType,
      IEntityPersister persister)
    {
      NHibernate.Loader.Loader.Log.Debug((object) "loading collection element by index");
      IList list;
      try
      {
        list = this.DoQueryAndInitializeNonLazyCollections(session, new QueryParameters(new IType[2]
        {
          keyType,
          indexType
        }, new object[2]{ key, index }), false);
      }
      catch (Exception ex)
      {
        throw ADOExceptionHelper.Convert(this._factory.SQLExceptionConverter, ex, "could not collection element by index", this.SqlString);
      }
      NHibernate.Loader.Loader.Log.Debug((object) "done entity load");
      return list;
    }

    protected internal IList LoadEntityBatch(
      ISessionImplementor session,
      object[] ids,
      IType idType,
      object optionalObject,
      string optionalEntityName,
      object optionalId,
      IEntityPersister persister)
    {
      if (NHibernate.Loader.Loader.Log.IsDebugEnabled)
        NHibernate.Loader.Loader.Log.Debug((object) ("batch loading entity: " + MessageHelper.InfoString(persister, ids, this.Factory)));
      IType[] typeArray = new IType[ids.Length];
      ArrayHelper.Fill<IType>(typeArray, idType);
      IList list;
      try
      {
        list = this.DoQueryAndInitializeNonLazyCollections(session, new QueryParameters(typeArray, ids, optionalObject, optionalEntityName, optionalId), false);
      }
      catch (HibernateException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, ex, "could not load an entity batch: " + MessageHelper.InfoString(persister, ids, this.Factory), this.SqlString);
      }
      NHibernate.Loader.Loader.Log.Debug((object) "done entity batch load");
      return list;
    }

    public void LoadCollection(ISessionImplementor session, object id, IType type)
    {
      if (NHibernate.Loader.Loader.Log.IsDebugEnabled)
        NHibernate.Loader.Loader.Log.Debug((object) ("loading collection: " + MessageHelper.InfoString(this.CollectionPersisters[0], id)));
      object[] objArray = new object[1]{ id };
      try
      {
        this.DoQueryAndInitializeNonLazyCollections(session, new QueryParameters(new IType[1]
        {
          type
        }, objArray, objArray), true);
      }
      catch (HibernateException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, ex, "could not initialize a collection: " + MessageHelper.InfoString(this.CollectionPersisters[0], id), this.SqlString);
      }
      NHibernate.Loader.Loader.Log.Debug((object) "done loading collection");
    }

    public void LoadCollectionBatch(ISessionImplementor session, object[] ids, IType type)
    {
      if (NHibernate.Loader.Loader.Log.IsDebugEnabled)
        NHibernate.Loader.Loader.Log.Debug((object) ("batch loading collection: " + MessageHelper.InfoString(this.CollectionPersisters[0], (object) ids)));
      IType[] typeArray = new IType[ids.Length];
      ArrayHelper.Fill<IType>(typeArray, type);
      try
      {
        this.DoQueryAndInitializeNonLazyCollections(session, new QueryParameters(typeArray, ids, ids), true);
      }
      catch (HibernateException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, ex, "could not initialize a collection batch: " + MessageHelper.InfoString(this.CollectionPersisters[0], (object) ids), this.SqlString);
      }
      NHibernate.Loader.Loader.Log.Debug((object) "done batch load");
    }

    protected void LoadCollectionSubselect(
      ISessionImplementor session,
      object[] ids,
      object[] parameterValues,
      IType[] parameterTypes,
      IDictionary<string, TypedValue> namedParameters,
      IType type)
    {
      try
      {
        this.DoQueryAndInitializeNonLazyCollections(session, new QueryParameters(parameterTypes, parameterValues, namedParameters, ids), true);
      }
      catch (HibernateException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, ex, "could not load collection by subselect: " + MessageHelper.InfoString(this.CollectionPersisters[0], (object) ids), this.SqlString, parameterValues, namedParameters);
      }
    }

    protected IList List(
      ISessionImplementor session,
      QueryParameters queryParameters,
      ISet<string> querySpaces,
      IType[] resultTypes)
    {
      return this._factory.Settings.IsQueryCacheEnabled && queryParameters.Cacheable ? this.ListUsingQueryCache(session, queryParameters, querySpaces, resultTypes) : this.ListIgnoreQueryCache(session, queryParameters);
    }

    private IList ListIgnoreQueryCache(ISessionImplementor session, QueryParameters queryParameters)
    {
      return this.GetResultList(this.DoList(session, queryParameters), queryParameters.ResultTransformer);
    }

    private IList ListUsingQueryCache(
      ISessionImplementor session,
      QueryParameters queryParameters,
      ISet<string> querySpaces,
      IType[] resultTypes)
    {
      IQueryCache queryCache = this._factory.GetQueryCache(queryParameters.CacheRegion);
      ISet filterKeys = FilterKey.CreateFilterKeys(session.EnabledFilters, session.EntityMode);
      QueryKey key = new QueryKey(this.Factory, this.SqlString, queryParameters, filterKeys);
      IList list = this.GetResultFromQueryCache(session, queryParameters, querySpaces, resultTypes, queryCache, key);
      if (list == null)
      {
        list = this.DoList(session, queryParameters);
        this.PutResultInQueryCache(session, queryParameters, resultTypes, queryCache, key, list);
      }
      return this.GetResultList(list, queryParameters.ResultTransformer);
    }

    private IList GetResultFromQueryCache(
      ISessionImplementor session,
      QueryParameters queryParameters,
      ISet<string> querySpaces,
      IType[] resultTypes,
      IQueryCache queryCache,
      QueryKey key)
    {
      IList resultFromQueryCache = (IList) null;
      if (!queryParameters.ForceCacheRefresh && (session.CacheMode & CacheMode.Get) == CacheMode.Get)
      {
        IPersistenceContext persistenceContext = session.PersistenceContext;
        bool defaultReadOnly = persistenceContext.DefaultReadOnly;
        if (queryParameters.IsReadOnlyInitialized)
          persistenceContext.DefaultReadOnly = queryParameters.ReadOnly;
        else
          queryParameters.ReadOnly = persistenceContext.DefaultReadOnly;
        try
        {
          resultFromQueryCache = queryCache.Get(key, (ICacheAssembler[]) resultTypes, queryParameters.NaturalKeyLookup, querySpaces, session);
          if (this._factory.Statistics.IsStatisticsEnabled)
          {
            if (resultFromQueryCache == null)
              this._factory.StatisticsImplementor.QueryCacheMiss(this.QueryIdentifier, queryCache.RegionName);
            else
              this._factory.StatisticsImplementor.QueryCacheHit(this.QueryIdentifier, queryCache.RegionName);
          }
        }
        finally
        {
          persistenceContext.DefaultReadOnly = defaultReadOnly;
        }
      }
      return resultFromQueryCache;
    }

    private void PutResultInQueryCache(
      ISessionImplementor session,
      QueryParameters queryParameters,
      IType[] resultTypes,
      IQueryCache queryCache,
      QueryKey key,
      IList result)
    {
      if ((session.CacheMode & CacheMode.Put) != CacheMode.Put || !queryCache.Put(key, (ICacheAssembler[]) resultTypes, result, queryParameters.NaturalKeyLookup, session) || !this._factory.Statistics.IsStatisticsEnabled)
        return;
      this._factory.StatisticsImplementor.QueryCachePut(this.QueryIdentifier, queryCache.RegionName);
    }

    protected IList DoList(ISessionImplementor session, QueryParameters queryParameters)
    {
      bool statisticsEnabled = this.Factory.Statistics.IsStatisticsEnabled;
      Stopwatch stopwatch = new Stopwatch();
      if (statisticsEnabled)
        stopwatch.Start();
      IList list;
      try
      {
        list = this.DoQueryAndInitializeNonLazyCollections(session, queryParameters, true);
      }
      catch (HibernateException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw ADOExceptionHelper.Convert(this.Factory.SQLExceptionConverter, ex, "could not execute query", this.SqlString, queryParameters.PositionalParameterValues, queryParameters.NamedParameters);
      }
      if (statisticsEnabled)
      {
        stopwatch.Stop();
        this.Factory.StatisticsImplementor.QueryExecuted(this.QueryIdentifier, list.Count, stopwatch.Elapsed);
      }
      return list;
    }

    protected virtual void PostInstantiate()
    {
    }

    public virtual string QueryIdentifier => (string) null;

    public override string ToString()
    {
      return this.GetType().FullName + (object) '(' + (object) this.SqlString + (object) ')';
    }

    public virtual ISqlCommand CreateSqlCommand(
      QueryParameters queryParameters,
      ISessionImplementor session)
    {
      HashSet<IParameterSpecification> parameterSpecificationSet = new HashSet<IParameterSpecification>(this.GetParameterSpecifications());
      SqlString sqlString = this.ExpandDynamicFilterParameters(this.SqlString.Copy(), (ICollection<IParameterSpecification>) parameterSpecificationSet, session);
      this.AdjustQueryParametersForSubSelectFetching(sqlString, (IEnumerable<IParameterSpecification>) parameterSpecificationSet, queryParameters);
      SqlString query = this.PreprocessSQL(this.AddLimitsParametersIfNeeded(sqlString, (ICollection<IParameterSpecification>) parameterSpecificationSet, queryParameters, session), queryParameters, session.Factory.Dialect);
      this.ResetEffectiveExpectedType((IEnumerable<IParameterSpecification>) parameterSpecificationSet, queryParameters);
      return (ISqlCommand) new SqlCommandImpl(query, (ICollection<IParameterSpecification>) parameterSpecificationSet, queryParameters, session.Factory);
    }

    protected virtual void ResetEffectiveExpectedType(
      IEnumerable<IParameterSpecification> parameterSpecs,
      QueryParameters queryParameters)
    {
    }

    protected abstract IEnumerable<IParameterSpecification> GetParameterSpecifications();

    protected void AdjustQueryParametersForSubSelectFetching(
      SqlString filteredSqlString,
      IEnumerable<IParameterSpecification> parameterSpecsWithFilters,
      QueryParameters queryParameters)
    {
      queryParameters.ProcessedSql = filteredSqlString;
      queryParameters.ProcessedSqlParameters = (IEnumerable<IParameterSpecification>) parameterSpecsWithFilters.ToList<IParameterSpecification>();
      if (queryParameters.RowSelection == null)
        return;
      queryParameters.ProcessedRowSelection = new RowSelection()
      {
        FirstRow = queryParameters.RowSelection.FirstRow,
        MaxRows = queryParameters.RowSelection.MaxRows
      };
    }

    protected SqlString ExpandDynamicFilterParameters(
      SqlString sqlString,
      ICollection<IParameterSpecification> parameterSpecs,
      ISessionImplementor session)
    {
      IDictionary<string, NHibernate.IFilter> enabledFilters = session.EnabledFilters;
      if (enabledFilters.Count == 0 || sqlString.ToString().IndexOf(":") < 0)
        return sqlString;
      NHibernate.Dialect.Dialect dialect = session.Factory.Dialect;
      string delim = " \n\r\f\t,()=<>&|+-=/*'^![]#~\\;" + (object) dialect.OpenQuote + (object) dialect.CloseQuote;
      SqlString sqlString1 = sqlString.Compact();
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      foreach (object part in (IEnumerable) sqlString1.Parts)
      {
        Parameter parameter1 = part as Parameter;
        if (parameter1 != (Parameter) null)
        {
          sqlStringBuilder.Add(parameter1);
        }
        else
        {
          foreach (string sql1 in new StringTokenizer(part.ToString(), delim, true))
          {
            if (sql1.StartsWith(":"))
            {
              string[] filterParameterName = StringHelper.ParseFilterParameterName(sql1.Substring(1));
              string str1 = filterParameterName[0];
              string str2 = filterParameterName[1];
              FilterImpl filterImpl = (FilterImpl) enabledFilters[str1];
              object parameter2 = filterImpl.GetParameter(str2);
              IType parameterType = filterImpl.FilterDefinition.GetParameterType(str2);
              int columnSpan = parameterType.GetColumnSpan((IMapping) session.Factory);
              ICollection collection = parameter2 as ICollection;
              int? collectionSpan = new int?();
              string element = string.Join(", ", Enumerable.Repeat<string>("?", columnSpan).ToArray<string>());
              string sql2;
              if (collection != null && !parameterType.ReturnedClass.IsArray)
              {
                collectionSpan = new int?(collection.Count);
                sql2 = string.Join(", ", Enumerable.Repeat<string>(element, collection.Count).ToArray<string>());
              }
              else
                sql2 = element;
              SqlString sqlString2 = SqlString.Parse(sql2);
              DynamicFilterParameterSpecification parameterSpecification = new DynamicFilterParameterSpecification(str1, str2, parameterType, collectionSpan);
              Parameter[] array = sqlString2.GetParameters().ToArray<Parameter>();
              int num = 0;
              foreach (string str3 in parameterSpecification.GetIdsForBackTrack((IMapping) session.Factory))
                array[num++].BackTrack = (object) str3;
              parameterSpecs.Add((IParameterSpecification) parameterSpecification);
              sqlStringBuilder.Add(sqlString2);
            }
            else
              sqlStringBuilder.Add(sql1);
          }
        }
      }
      return sqlStringBuilder.ToSqlString().Compact();
    }

    protected SqlString AddLimitsParametersIfNeeded(
      SqlString sqlString,
      ICollection<IParameterSpecification> parameterSpecs,
      QueryParameters queryParameters,
      ISessionImplementor session)
    {
      ISessionFactoryImplementor factory = session.Factory;
      NHibernate.Dialect.Dialect dialect = factory.Dialect;
      RowSelection rowSelection = queryParameters.RowSelection;
      if (!this.UseLimit(rowSelection, dialect))
        return sqlString;
      bool flag = NHibernate.Loader.Loader.GetFirstRow(rowSelection) > 0 && dialect.SupportsLimitOffset;
      int maxOrLimit = NHibernate.Loader.Loader.GetMaxOrLimit(dialect, rowSelection);
      int? offset = flag ? new int?(dialect.GetOffsetValue(NHibernate.Loader.Loader.GetFirstRow(rowSelection))) : new int?();
      int? limit = maxOrLimit != int.MaxValue ? new int?(maxOrLimit) : new int?();
      Parameter offsetParameter = (Parameter) null;
      Parameter limitParameter = (Parameter) null;
      if (offset.HasValue)
      {
        QuerySkipParameterSpecification parameterSpecification = new QuerySkipParameterSpecification();
        offsetParameter = Parameter.Placeholder;
        offsetParameter.BackTrack = parameterSpecification.GetIdsForBackTrack((IMapping) factory).First();
        parameterSpecs.Add((IParameterSpecification) parameterSpecification);
      }
      if (limit.HasValue)
      {
        QueryTakeParameterSpecification parameterSpecification = new QueryTakeParameterSpecification();
        limitParameter = Parameter.Placeholder;
        limitParameter.BackTrack = parameterSpecification.GetIdsForBackTrack((IMapping) factory).First();
        parameterSpecs.Add((IParameterSpecification) parameterSpecification);
      }
      return dialect.GetLimitString(sqlString, offset, limit, offsetParameter, limitParameter);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.SessionFactoryImpl
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Cache;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Context;
using NHibernate.Dialect.Function;
using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Engine.Query.Sql;
using NHibernate.Event;
using NHibernate.Exceptions;
using NHibernate.Id;
using NHibernate.Mapping;
using NHibernate.Metadata;
using NHibernate.Persister;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Stat;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Transaction;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

#nullable disable
namespace NHibernate.Impl
{
  [Serializable]
  public sealed class SessionFactoryImpl : 
    ISessionFactoryImplementor,
    IMapping,
    ISessionFactory,
    IDisposable,
    IObjectReference
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (SessionFactoryImpl));
    private static readonly IIdentifierGenerator UuidGenerator = (IIdentifierGenerator) new UUIDHexGenerator();
    [NonSerialized]
    private readonly ThreadSafeDictionary<string, ICache> allCacheRegions = new ThreadSafeDictionary<string, ICache>((IDictionary<string, ICache>) new Dictionary<string, ICache>());
    [NonSerialized]
    private readonly IDictionary<string, IClassMetadata> classMetadata;
    [NonSerialized]
    private readonly IDictionary<string, ICollectionMetadata> collectionMetadata;
    [NonSerialized]
    private readonly Dictionary<string, ICollectionPersister> collectionPersisters;
    [NonSerialized]
    private readonly IDictionary<string, ISet<string>> collectionRolesByEntityParticipant;
    [NonSerialized]
    private readonly ICurrentSessionContext currentSessionContext;
    [NonSerialized]
    private readonly IEntityNotFoundDelegate entityNotFoundDelegate;
    [NonSerialized]
    private readonly IDictionary<string, IEntityPersister> entityPersisters;
    [NonSerialized]
    private readonly IDictionary<System.Type, string> implementorToEntityName;
    [NonSerialized]
    private readonly EventListeners eventListeners;
    [NonSerialized]
    private readonly Dictionary<string, FilterDefinition> filters;
    [NonSerialized]
    private readonly Dictionary<string, IIdentifierGenerator> identifierGenerators;
    [NonSerialized]
    private readonly Dictionary<string, string> imports;
    [NonSerialized]
    private readonly IInterceptor interceptor;
    private readonly string name;
    [NonSerialized]
    private readonly Dictionary<string, NamedQueryDefinition> namedQueries;
    [NonSerialized]
    private readonly Dictionary<string, NamedSQLQueryDefinition> namedSqlQueries;
    [NonSerialized]
    private readonly IDictionary<string, string> properties;
    [NonSerialized]
    private readonly IQueryCache queryCache;
    [NonSerialized]
    private readonly IDictionary<string, IQueryCache> queryCaches;
    [NonSerialized]
    private readonly SchemaExport schemaExport;
    [NonSerialized]
    private readonly Settings settings;
    [NonSerialized]
    private readonly SQLFunctionRegistry sqlFunctionRegistry;
    [NonSerialized]
    private readonly Dictionary<string, ResultSetMappingDefinition> sqlResultSetMappings;
    [NonSerialized]
    private readonly UpdateTimestampsCache updateTimestampsCache;
    [NonSerialized]
    private readonly IDictionary<string, string[]> entityNameImplementorsMap = (IDictionary<string, string[]>) new ThreadSafeDictionary<string, string[]>((IDictionary<string, string[]>) new Dictionary<string, string[]>(100));
    private readonly string uuid;
    private bool disposed;
    [NonSerialized]
    private bool isClosed;
    private QueryPlanCache queryPlanCache;
    [NonSerialized]
    private StatisticsImpl statistics;

    public SessionFactoryImpl(
      Configuration cfg,
      IMapping mapping,
      Settings settings,
      EventListeners listeners)
    {
      this.Init();
      SessionFactoryImpl.log.Info((object) "building session factory");
      this.properties = (IDictionary<string, string>) new Dictionary<string, string>(cfg.Properties);
      this.interceptor = cfg.Interceptor;
      this.settings = settings;
      this.sqlFunctionRegistry = new SQLFunctionRegistry(settings.Dialect, cfg.SqlFunctions);
      this.eventListeners = listeners;
      this.filters = new Dictionary<string, FilterDefinition>(cfg.FilterDefinitions);
      if (SessionFactoryImpl.log.IsDebugEnabled)
        SessionFactoryImpl.log.Debug((object) ("Session factory constructed with filter configurations : " + CollectionPrinter.ToString((IDictionary) this.filters)));
      if (SessionFactoryImpl.log.IsDebugEnabled)
        SessionFactoryImpl.log.Debug((object) ("instantiating session factory with properties: " + CollectionPrinter.ToString(this.properties)));
      try
      {
        if (settings.IsKeywordsImportEnabled)
          SchemaMetadataUpdater.Update((ISessionFactory) this);
        if (settings.IsAutoQuoteEnabled)
          SchemaMetadataUpdater.QuoteTableAndColumns(cfg);
      }
      catch (NotSupportedException ex)
      {
      }
      settings.CacheProvider.Start(this.properties);
      this.identifierGenerators = new Dictionary<string, IIdentifierGenerator>();
      foreach (PersistentClass classMapping in (IEnumerable<PersistentClass>) cfg.ClassMappings)
      {
        if (!classMapping.IsInherited)
        {
          IIdentifierGenerator identifierGenerator = classMapping.Identifier.CreateIdentifierGenerator(settings.Dialect, settings.DefaultCatalogName, settings.DefaultSchemaName, (RootClass) classMapping);
          this.identifierGenerators[classMapping.EntityName] = identifierGenerator;
        }
      }
      Dictionary<string, ICacheConcurrencyStrategy> dictionary1 = new Dictionary<string, ICacheConcurrencyStrategy>();
      this.entityPersisters = (IDictionary<string, IEntityPersister>) new Dictionary<string, IEntityPersister>();
      this.implementorToEntityName = (IDictionary<System.Type, string>) new Dictionary<System.Type, string>();
      Dictionary<string, IClassMetadata> dictionary2 = new Dictionary<string, IClassMetadata>();
      foreach (PersistentClass classMapping in (IEnumerable<PersistentClass>) cfg.ClassMappings)
      {
        classMapping.PrepareTemporaryTables(mapping, settings.Dialect);
        string cacheRegionName = classMapping.RootClazz.CacheRegionName;
        ICacheConcurrencyStrategy cache;
        if (!dictionary1.TryGetValue(cacheRegionName, out cache))
        {
          cache = CacheFactory.CreateCache(classMapping.CacheConcurrencyStrategy, cacheRegionName, classMapping.IsMutable, settings, this.properties);
          if (cache != null)
          {
            dictionary1.Add(cacheRegionName, cache);
            this.allCacheRegions.Add(cache.RegionName, cache.Cache);
          }
        }
        IEntityPersister classPersister = PersisterFactory.CreateClassPersister(classMapping, cache, (ISessionFactoryImplementor) this, mapping);
        this.entityPersisters[classMapping.EntityName] = classPersister;
        dictionary2[classMapping.EntityName] = classPersister.ClassMetadata;
        if (classMapping.HasPocoRepresentation)
          this.implementorToEntityName[classMapping.MappedClass] = classMapping.EntityName;
      }
      this.classMetadata = (IDictionary<string, IClassMetadata>) new UnmodifiableDictionary<string, IClassMetadata>((IDictionary<string, IClassMetadata>) dictionary2);
      Dictionary<string, ISet<string>> dictionary3 = new Dictionary<string, ISet<string>>();
      this.collectionPersisters = new Dictionary<string, ICollectionPersister>();
      foreach (NHibernate.Mapping.Collection collectionMapping in (IEnumerable<NHibernate.Mapping.Collection>) cfg.CollectionMappings)
      {
        ICacheConcurrencyStrategy cache = CacheFactory.CreateCache(collectionMapping.CacheConcurrencyStrategy, collectionMapping.CacheRegionName, collectionMapping.Owner.IsMutable, settings, this.properties);
        if (cache != null)
          this.allCacheRegions[cache.RegionName] = cache.Cache;
        ICollectionPersister collectionPersister = PersisterFactory.CreateCollectionPersister(cfg, collectionMapping, cache, (ISessionFactoryImplementor) this);
        this.collectionPersisters[collectionMapping.Role] = collectionPersister;
        IType indexType = collectionPersister.IndexType;
        if (indexType != null && indexType.IsAssociationType && !indexType.IsAnyType)
        {
          string associatedEntityName = ((IAssociationType) indexType).GetAssociatedEntityName((ISessionFactoryImplementor) this);
          ISet<string> set;
          if (!dictionary3.TryGetValue(associatedEntityName, out set))
          {
            set = (ISet<string>) new HashedSet<string>();
            dictionary3[associatedEntityName] = set;
          }
          set.Add(collectionPersister.Role);
        }
        IType elementType = collectionPersister.ElementType;
        if (elementType.IsAssociationType && !elementType.IsAnyType)
        {
          string associatedEntityName = ((IAssociationType) elementType).GetAssociatedEntityName((ISessionFactoryImplementor) this);
          ISet<string> set;
          if (!dictionary3.TryGetValue(associatedEntityName, out set))
          {
            set = (ISet<string>) new HashedSet<string>();
            dictionary3[associatedEntityName] = set;
          }
          set.Add(collectionPersister.Role);
        }
      }
      Dictionary<string, ICollectionMetadata> dictionary4 = new Dictionary<string, ICollectionMetadata>(this.collectionPersisters.Count);
      foreach (KeyValuePair<string, ICollectionPersister> collectionPersister in this.collectionPersisters)
        dictionary4.Add(collectionPersister.Key, collectionPersister.Value.CollectionMetadata);
      this.collectionMetadata = (IDictionary<string, ICollectionMetadata>) new UnmodifiableDictionary<string, ICollectionMetadata>((IDictionary<string, ICollectionMetadata>) dictionary4);
      this.collectionRolesByEntityParticipant = (IDictionary<string, ISet<string>>) new UnmodifiableDictionary<string, ISet<string>>((IDictionary<string, ISet<string>>) dictionary3);
      this.namedQueries = new Dictionary<string, NamedQueryDefinition>(cfg.NamedQueries);
      this.namedSqlQueries = new Dictionary<string, NamedSQLQueryDefinition>(cfg.NamedSQLQueries);
      this.sqlResultSetMappings = new Dictionary<string, ResultSetMappingDefinition>(cfg.SqlResultSetMappings);
      this.imports = new Dictionary<string, string>(cfg.Imports);
      foreach (IEntityPersister entityPersister in (IEnumerable<IEntityPersister>) this.entityPersisters.Values)
        entityPersister.PostInstantiate();
      foreach (ICollectionPersister collectionPersister in this.collectionPersisters.Values)
        collectionPersister.PostInstantiate();
      this.name = settings.SessionFactoryName;
      try
      {
        this.uuid = (string) SessionFactoryImpl.UuidGenerator.Generate((ISessionImplementor) null, (object) null);
      }
      catch (Exception ex)
      {
        throw new AssertionFailure("Could not generate UUID");
      }
      SessionFactoryObjectFactory.AddInstance(this.uuid, this.name, (ISessionFactory) this, this.properties);
      SessionFactoryImpl.log.Debug((object) "Instantiated session factory");
      if (settings.IsAutoCreateSchema)
        new SchemaExport(cfg).Create(false, true);
      if (settings.IsAutoUpdateSchema)
        new SchemaUpdate(cfg).Execute(false, true);
      if (settings.IsAutoValidateSchema)
        new SchemaValidator(cfg, settings).Validate();
      if (settings.IsAutoDropSchema)
        this.schemaExport = new SchemaExport(cfg);
      this.currentSessionContext = this.BuildCurrentSessionContext();
      if (settings.IsQueryCacheEnabled)
      {
        this.updateTimestampsCache = new UpdateTimestampsCache(settings, this.properties);
        this.queryCache = settings.QueryCacheFactory.GetQueryCache((string) null, this.updateTimestampsCache, settings, this.properties);
        this.queryCaches = (IDictionary<string, IQueryCache>) new ThreadSafeDictionary<string, IQueryCache>((IDictionary<string, IQueryCache>) new Dictionary<string, IQueryCache>());
      }
      else
      {
        this.updateTimestampsCache = (UpdateTimestampsCache) null;
        this.queryCache = (IQueryCache) null;
        this.queryCaches = (IDictionary<string, IQueryCache>) null;
      }
      if (settings.IsNamedQueryStartupCheckingEnabled)
      {
        IDictionary<string, HibernateException> dictionary5 = this.CheckNamedQueries();
        if (dictionary5.Count > 0)
        {
          StringBuilder stringBuilder = new StringBuilder("Errors in named queries: ");
          foreach (KeyValuePair<string, HibernateException> keyValuePair in (IEnumerable<KeyValuePair<string, HibernateException>>) dictionary5)
          {
            stringBuilder.Append('{').Append(keyValuePair.Key).Append('}');
            SessionFactoryImpl.log.Error((object) ("Error in named query: " + keyValuePair.Key), (Exception) keyValuePair.Value);
          }
          throw new HibernateException(stringBuilder.ToString());
        }
      }
      this.Statistics.IsStatisticsEnabled = settings.IsStatisticsEnabled;
      this.entityNotFoundDelegate = cfg.EntityNotFoundDelegate ?? (IEntityNotFoundDelegate) new SessionFactoryImpl.DefaultEntityNotFoundDelegate();
    }

    public EventListeners EventListeners => this.eventListeners;

    public object GetRealObject(StreamingContext context)
    {
      SessionFactoryImpl.log.Debug((object) "Resolving serialized SessionFactory");
      ISessionFactory realObject = SessionFactoryObjectFactory.GetInstance(this.uuid);
      if (realObject == null)
      {
        realObject = SessionFactoryObjectFactory.GetNamedInstance(this.name);
        if (realObject == null)
          throw new NullReferenceException("Could not find a SessionFactory named " + this.name + " or identified by uuid " + this.uuid);
        SessionFactoryImpl.log.Debug((object) "resolved SessionFactory by name");
      }
      else
        SessionFactoryImpl.log.Debug((object) "resolved SessionFactory by uuid");
      return (object) realObject;
    }

    public ISession OpenSession() => this.OpenSession(this.interceptor);

    public ISession OpenSession(IDbConnection connection)
    {
      return this.OpenSession(connection, this.interceptor);
    }

    public ISession OpenSession(IDbConnection connection, IInterceptor sessionLocalInterceptor)
    {
      return sessionLocalInterceptor != null ? (ISession) this.OpenSession(connection, false, long.MinValue, sessionLocalInterceptor) : throw new ArgumentNullException(nameof (sessionLocalInterceptor));
    }

    public ISession OpenSession(IInterceptor sessionLocalInterceptor)
    {
      if (sessionLocalInterceptor == null)
        throw new ArgumentNullException(nameof (sessionLocalInterceptor));
      return (ISession) this.OpenSession((IDbConnection) null, true, this.settings.CacheProvider.NextTimestamp(), sessionLocalInterceptor);
    }

    public ISession OpenSession(
      IDbConnection connection,
      bool flushBeforeCompletionEnabled,
      bool autoCloseSessionEnabled,
      ConnectionReleaseMode connectionReleaseMode)
    {
      return (ISession) new SessionImpl(connection, this, true, this.settings.CacheProvider.NextTimestamp(), this.interceptor, this.settings.DefaultEntityMode, flushBeforeCompletionEnabled, autoCloseSessionEnabled, connectionReleaseMode);
    }

    public IEntityPersister GetEntityPersister(string entityName)
    {
      IEntityPersister entityPersister;
      if (!this.entityPersisters.TryGetValue(entityName, out entityPersister))
        throw new MappingException("No persister for: " + entityName);
      return entityPersister;
    }

    public IEntityPersister TryGetEntityPersister(string entityName)
    {
      IEntityPersister entityPersister;
      this.entityPersisters.TryGetValue(entityName, out entityPersister);
      return entityPersister;
    }

    public ICollectionPersister GetCollectionPersister(string role)
    {
      ICollectionPersister collectionPersister;
      if (!this.collectionPersisters.TryGetValue(role, out collectionPersister))
        throw new MappingException("Unknown collection role: " + role);
      return collectionPersister;
    }

    public ISet<string> GetCollectionRolesByEntityParticipant(string entityName)
    {
      ISet<string> entityParticipant;
      this.collectionRolesByEntityParticipant.TryGetValue(entityName, out entityParticipant);
      return entityParticipant;
    }

    public NHibernate.Dialect.Dialect Dialect => this.settings.Dialect;

    public IInterceptor Interceptor => this.interceptor;

    public ITransactionFactory TransactionFactory => this.settings.TransactionFactory;

    public ISQLExceptionConverter SQLExceptionConverter => this.settings.SqlExceptionConverter;

    public NamedQueryDefinition GetNamedQuery(string queryName)
    {
      NamedQueryDefinition namedQuery;
      this.namedQueries.TryGetValue(queryName, out namedQuery);
      return namedQuery;
    }

    public NamedSQLQueryDefinition GetNamedSQLQuery(string queryName)
    {
      NamedSQLQueryDefinition namedSqlQuery;
      this.namedSqlQueries.TryGetValue(queryName, out namedSqlQuery);
      return namedSqlQuery;
    }

    public IType GetIdentifierType(string className)
    {
      return this.GetEntityPersister(className).IdentifierType;
    }

    public string GetIdentifierPropertyName(string className)
    {
      return this.GetEntityPersister(className).IdentifierPropertyName;
    }

    public IType[] GetReturnTypes(string queryString)
    {
      return this.queryPlanCache.GetHQLQueryPlan(queryString, false, (IDictionary<string, NHibernate.IFilter>) new CollectionHelper.EmptyMapClass<string, NHibernate.IFilter>()).ReturnMetadata.ReturnTypes;
    }

    public string[] GetReturnAliases(string queryString)
    {
      return this.queryPlanCache.GetHQLQueryPlan(queryString, false, (IDictionary<string, NHibernate.IFilter>) new CollectionHelper.EmptyMapClass<string, NHibernate.IFilter>()).ReturnMetadata.ReturnAliases;
    }

    public IClassMetadata GetClassMetadata(System.Type persistentClass)
    {
      return this.GetClassMetadata(persistentClass.FullName);
    }

    public IClassMetadata GetClassMetadata(string entityName)
    {
      IClassMetadata classMetadata;
      this.classMetadata.TryGetValue(entityName, out classMetadata);
      return classMetadata;
    }

    public ICollectionMetadata GetCollectionMetadata(string roleName)
    {
      ICollectionMetadata collectionMetadata;
      this.collectionMetadata.TryGetValue(roleName, out collectionMetadata);
      return collectionMetadata;
    }

    public string[] GetImplementors(string entityOrClassName)
    {
      string[] implementors1;
      if (this.entityNameImplementorsMap.TryGetValue(entityOrClassName, out implementors1))
        return implementors1;
      System.Type entityClass = (System.Type) null;
      if (entityOrClassName.IndexOf('.') > 0)
      {
        IEntityPersister entityPersister;
        if (this.entityPersisters.TryGetValue(entityOrClassName, out entityPersister))
        {
          if (!entityPersister.EntityMetamodel.HasPocoRepresentation)
          {
            string[] implementors2 = new string[1]
            {
              entityOrClassName
            };
            this.entityNameImplementorsMap[entityOrClassName] = implementors2;
            return implementors2;
          }
          entityClass = entityPersister.GetMappedClass(EntityMode.Poco);
        }
        if (entityClass == null)
        {
          try
          {
            entityClass = ReflectHelper.ClassForFullNameOrNull(entityOrClassName);
          }
          catch (Exception ex)
          {
            entityClass = (System.Type) null;
          }
        }
      }
      if (entityClass == null)
      {
        string importedClassName = this.GetImportedClassName(entityOrClassName);
        if (importedClassName != null)
          entityClass = System.Type.GetType(importedClassName, false);
      }
      if (entityClass == null)
      {
        string[] implementors3 = new string[1]
        {
          entityOrClassName
        };
        this.entityNameImplementorsMap[entityOrClassName] = implementors3;
        return implementors3;
      }
      List<string> stringList = new List<string>();
      foreach (NHibernate.Persister.Entity.IQueryable implementor in this.entityPersisters.Values.OfType<NHibernate.Persister.Entity.IQueryable>())
      {
        string entityName = implementor.EntityName;
        bool flag1 = entityOrClassName.Equals(entityName) || entityClass.FullName.Equals(entityName);
        if (implementor.IsExplicitPolymorphism)
        {
          if (flag1)
          {
            string[] implementors4 = new string[1]
            {
              entityName
            };
            this.entityNameImplementorsMap[entityOrClassName] = implementors4;
            return implementors4;
          }
        }
        else if (flag1)
          stringList.Add(entityName);
        else if (SessionFactoryImpl.IsMatchingImplementor(entityOrClassName, entityClass, implementor))
        {
          bool flag2;
          if (implementor.IsInherited)
          {
            System.Type mappedClass = this.GetEntityPersister(implementor.MappedSuperclass).GetMappedClass(EntityMode.Poco);
            flag2 = entityClass.IsAssignableFrom(mappedClass);
          }
          else
            flag2 = false;
          if (!flag2)
            stringList.Add(entityName);
        }
      }
      string[] array = stringList.ToArray();
      this.entityNameImplementorsMap[entityOrClassName] = array;
      return array;
    }

    private static bool IsMatchingImplementor(
      string entityOrClassName,
      System.Type entityClass,
      NHibernate.Persister.Entity.IQueryable implementor)
    {
      System.Type mappedClass = implementor.GetMappedClass(EntityMode.Poco);
      if (mappedClass == null)
        return false;
      if (!entityClass.Equals(mappedClass))
        return entityClass.IsAssignableFrom(mappedClass);
      return entityOrClassName.Equals(entityClass.FullName) || entityOrClassName.Equals(implementor.EntityName);
    }

    public string GetImportedClassName(string className)
    {
      string str;
      return className != null && this.imports.TryGetValue(className, out str) ? str : className;
    }

    public IDictionary<string, IClassMetadata> GetAllClassMetadata() => this.classMetadata;

    public IDictionary<string, ICollectionMetadata> GetAllCollectionMetadata()
    {
      return this.collectionMetadata;
    }

    public void Dispose()
    {
      if (this.disposed)
        return;
      this.disposed = true;
      this.Close();
    }

    public void Close()
    {
      SessionFactoryImpl.log.Info((object) "Closing");
      this.isClosed = true;
      foreach (IEntityPersister entityPersister in (IEnumerable<IEntityPersister>) this.entityPersisters.Values)
      {
        if (entityPersister.HasCache)
          entityPersister.Cache.Destroy();
      }
      foreach (ICollectionPersister collectionPersister in this.collectionPersisters.Values)
      {
        if (collectionPersister.HasCache)
          collectionPersister.Cache.Destroy();
      }
      if (this.settings.IsQueryCacheEnabled)
      {
        this.queryCache.Destroy();
        foreach (IQueryCache queryCache in (IEnumerable<IQueryCache>) this.queryCaches.Values)
          queryCache.Destroy();
        this.updateTimestampsCache.Destroy();
      }
      this.settings.CacheProvider.Stop();
      try
      {
        this.settings.ConnectionProvider.Dispose();
      }
      finally
      {
        SessionFactoryObjectFactory.RemoveInstance(this.uuid, this.name, this.properties);
      }
      if (this.settings.IsAutoDropSchema)
        this.schemaExport.Drop(false, true);
      this.eventListeners.DestroyListeners();
    }

    public void Evict(System.Type persistentClass, object id)
    {
      IEntityPersister entityPersister = this.GetEntityPersister(persistentClass.FullName);
      if (!entityPersister.HasCache)
        return;
      if (SessionFactoryImpl.log.IsDebugEnabled)
        SessionFactoryImpl.log.Debug((object) ("evicting second-level cache: " + MessageHelper.InfoString(entityPersister, id)));
      CacheKey key = new CacheKey(id, entityPersister.IdentifierType, entityPersister.RootEntityName, EntityMode.Poco, (ISessionFactoryImplementor) this);
      entityPersister.Cache.Remove(key);
    }

    public void Evict(System.Type persistentClass)
    {
      IEntityPersister entityPersister = this.GetEntityPersister(persistentClass.FullName);
      if (!entityPersister.HasCache)
        return;
      if (SessionFactoryImpl.log.IsDebugEnabled)
        SessionFactoryImpl.log.Debug((object) ("evicting second-level cache: " + entityPersister.EntityName));
      entityPersister.Cache.Clear();
    }

    public void EvictEntity(string entityName)
    {
      IEntityPersister entityPersister = this.GetEntityPersister(entityName);
      if (!entityPersister.HasCache)
        return;
      if (SessionFactoryImpl.log.IsDebugEnabled)
        SessionFactoryImpl.log.Debug((object) ("evicting second-level cache: " + entityPersister.EntityName));
      entityPersister.Cache.Clear();
    }

    public void EvictEntity(string entityName, object id)
    {
      IEntityPersister entityPersister = this.GetEntityPersister(entityName);
      if (!entityPersister.HasCache)
        return;
      if (SessionFactoryImpl.log.IsDebugEnabled)
        SessionFactoryImpl.log.Debug((object) ("evicting second-level cache: " + MessageHelper.InfoString(entityPersister, id, (ISessionFactoryImplementor) this)));
      CacheKey key = new CacheKey(id, entityPersister.IdentifierType, entityPersister.RootEntityName, EntityMode.Poco, (ISessionFactoryImplementor) this);
      entityPersister.Cache.Remove(key);
    }

    public void EvictCollection(string roleName, object id)
    {
      ICollectionPersister collectionPersister = this.GetCollectionPersister(roleName);
      if (!collectionPersister.HasCache)
        return;
      if (SessionFactoryImpl.log.IsDebugEnabled)
        SessionFactoryImpl.log.Debug((object) ("evicting second-level cache: " + MessageHelper.InfoString(collectionPersister, id)));
      CacheKey key = new CacheKey(id, collectionPersister.KeyType, collectionPersister.Role, EntityMode.Poco, (ISessionFactoryImplementor) this);
      collectionPersister.Cache.Remove(key);
    }

    public void EvictCollection(string roleName)
    {
      ICollectionPersister collectionPersister = this.GetCollectionPersister(roleName);
      if (!collectionPersister.HasCache)
        return;
      if (SessionFactoryImpl.log.IsDebugEnabled)
        SessionFactoryImpl.log.Debug((object) ("evicting second-level cache: " + collectionPersister.Role));
      collectionPersister.Cache.Clear();
    }

    public IType GetReferencedPropertyType(string className, string propertyName)
    {
      return this.GetEntityPersister(className).GetPropertyType(propertyName);
    }

    public bool HasNonIdentifierPropertyNamedId(string className)
    {
      return "id".Equals(this.GetIdentifierPropertyName(className));
    }

    public IConnectionProvider ConnectionProvider => this.settings.ConnectionProvider;

    public bool IsClosed => this.isClosed;

    public UpdateTimestampsCache UpdateTimestampsCache => this.updateTimestampsCache;

    public IDictionary<string, ICache> GetAllSecondLevelCacheRegions()
    {
      lock (this.allCacheRegions.SyncRoot)
        return (IDictionary<string, ICache>) new Dictionary<string, ICache>((IDictionary<string, ICache>) this.allCacheRegions);
    }

    public ICache GetSecondLevelCacheRegion(string regionName)
    {
      ICache levelCacheRegion;
      this.allCacheRegions.TryGetValue(regionName, out levelCacheRegion);
      return levelCacheRegion;
    }

    public IStatisticsImplementor StatisticsImplementor => (IStatisticsImplementor) this.statistics;

    public IQueryCache QueryCache => this.queryCache;

    public IQueryCache GetQueryCache(string cacheRegion)
    {
      if (cacheRegion == null)
        return this.QueryCache;
      if (!this.settings.IsQueryCacheEnabled)
        return (IQueryCache) null;
      lock (this.allCacheRegions.SyncRoot)
      {
        IQueryCache queryCache;
        if (!this.queryCaches.TryGetValue(cacheRegion, out queryCache))
        {
          queryCache = this.settings.QueryCacheFactory.GetQueryCache(cacheRegion, this.updateTimestampsCache, this.settings, this.properties);
          this.queryCaches[cacheRegion] = queryCache;
          this.allCacheRegions[queryCache.RegionName] = queryCache.Cache;
        }
        return queryCache;
      }
    }

    public void EvictQueries()
    {
      if (this.queryCache == null)
        return;
      this.queryCache.Clear();
      if (this.queryCaches.Count != 0)
        return;
      this.updateTimestampsCache.Clear();
    }

    public void EvictQueries(string cacheRegion)
    {
      if (string.IsNullOrEmpty(cacheRegion))
        throw new ArgumentNullException(nameof (cacheRegion), "use the zero-argument form to evict the default query cache");
      IQueryCache queryCache;
      if (!this.settings.IsQueryCacheEnabled || !this.queryCaches.TryGetValue(cacheRegion, out queryCache))
        return;
      queryCache.Clear();
    }

    public IIdentifierGenerator GetIdentifierGenerator(string rootEntityName)
    {
      IIdentifierGenerator identifierGenerator;
      this.identifierGenerators.TryGetValue(rootEntityName, out identifierGenerator);
      return identifierGenerator;
    }

    public ResultSetMappingDefinition GetResultSetMapping(string resultSetName)
    {
      ResultSetMappingDefinition resultSetMapping;
      this.sqlResultSetMappings.TryGetValue(resultSetName, out resultSetMapping);
      return resultSetMapping;
    }

    public FilterDefinition GetFilterDefinition(string filterName)
    {
      FilterDefinition filterDefinition;
      if (!this.filters.TryGetValue(filterName, out filterDefinition))
        throw new HibernateException("No such filter configured [" + filterName + "]");
      return filterDefinition;
    }

    public ICollection<string> DefinedFilterNames => (ICollection<string>) this.filters.Keys;

    public Settings Settings => this.settings;

    public ISession GetCurrentSession()
    {
      return this.currentSessionContext != null ? this.currentSessionContext.CurrentSession() : throw new HibernateException("No CurrentSessionContext configured (set the property current_session_context_class)!");
    }

    public IStatelessSession OpenStatelessSession()
    {
      return (IStatelessSession) new StatelessSessionImpl((IDbConnection) null, this);
    }

    public IStatelessSession OpenStatelessSession(IDbConnection connection)
    {
      return (IStatelessSession) new StatelessSessionImpl(connection, this);
    }

    public IStatistics Statistics => (IStatistics) this.statistics;

    public ICurrentSessionContext CurrentSessionContext => this.currentSessionContext;

    public SQLFunctionRegistry SQLFunctionRegistry => this.sqlFunctionRegistry;

    public IEntityNotFoundDelegate EntityNotFoundDelegate => this.entityNotFoundDelegate;

    public QueryPlanCache QueryPlanCache => this.queryPlanCache;

    private void Init()
    {
      this.statistics = new StatisticsImpl((ISessionFactoryImplementor) this);
      this.queryPlanCache = new QueryPlanCache((ISessionFactoryImplementor) this);
    }

    private IDictionary<string, HibernateException> CheckNamedQueries()
    {
      IDictionary<string, HibernateException> dictionary = (IDictionary<string, HibernateException>) new Dictionary<string, HibernateException>();
      SessionFactoryImpl.log.Debug((object) ("Checking " + (object) this.namedQueries.Count + " named HQL queries"));
      foreach (KeyValuePair<string, NamedQueryDefinition> namedQuery in this.namedQueries)
      {
        string key = namedQuery.Key;
        NamedQueryDefinition namedQueryDefinition = namedQuery.Value;
        try
        {
          SessionFactoryImpl.log.Debug((object) ("Checking named query: " + key));
          this.queryPlanCache.GetHQLQueryPlan(namedQueryDefinition.QueryString, false, (IDictionary<string, NHibernate.IFilter>) new CollectionHelper.EmptyMapClass<string, NHibernate.IFilter>());
        }
        catch (QueryException ex)
        {
          dictionary[key] = (HibernateException) ex;
        }
        catch (MappingException ex)
        {
          dictionary[key] = (HibernateException) ex;
        }
      }
      SessionFactoryImpl.log.Debug((object) ("Checking " + (object) this.namedSqlQueries.Count + " named SQL queries"));
      foreach (KeyValuePair<string, NamedSQLQueryDefinition> namedSqlQuery in this.namedSqlQueries)
      {
        string key = namedSqlQuery.Key;
        NamedSQLQueryDefinition sqlQueryDefinition = namedSqlQuery.Value;
        try
        {
          SessionFactoryImpl.log.Debug((object) ("Checking named SQL query: " + key));
          NativeSQLQuerySpecification spec;
          if (sqlQueryDefinition.ResultSetRef != null)
            spec = new NativeSQLQuerySpecification(sqlQueryDefinition.QueryString, (this.sqlResultSetMappings[sqlQueryDefinition.ResultSetRef] ?? throw new MappingException("Unable to find resultset-ref definition: " + sqlQueryDefinition.ResultSetRef)).GetQueryReturns(), (ICollection<string>) sqlQueryDefinition.QuerySpaces);
          else
            spec = new NativeSQLQuerySpecification(sqlQueryDefinition.QueryString, sqlQueryDefinition.QueryReturns, (ICollection<string>) sqlQueryDefinition.QuerySpaces);
          this.queryPlanCache.GetNativeSQLQueryPlan(spec);
        }
        catch (QueryException ex)
        {
          dictionary[key] = (HibernateException) ex;
        }
        catch (MappingException ex)
        {
          dictionary[key] = (HibernateException) ex;
        }
      }
      return dictionary;
    }

    private SessionImpl OpenSession(
      IDbConnection connection,
      bool autoClose,
      long timestamp,
      IInterceptor sessionLocalInterceptor)
    {
      SessionImpl sessionImpl = new SessionImpl(connection, this, autoClose, timestamp, sessionLocalInterceptor ?? this.interceptor, this.settings.DefaultEntityMode, this.settings.IsFlushBeforeCompletionEnabled, this.settings.IsAutoCloseSessionEnabled, this.settings.ConnectionReleaseMode);
      sessionLocalInterceptor?.SetSession((ISession) sessionImpl);
      return sessionImpl;
    }

    private ICurrentSessionContext BuildCurrentSessionContext()
    {
      string name = PropertiesHelper.GetString("current_session_context_class", this.properties, (string) null);
      switch (name)
      {
        case null:
          return (ICurrentSessionContext) null;
        case "call":
          return (ICurrentSessionContext) new CallSessionContext((ISessionFactoryImplementor) this);
        case "thread_static":
          return (ICurrentSessionContext) new ThreadStaticSessionContext((ISessionFactoryImplementor) this);
        case "web":
          return (ICurrentSessionContext) new WebSessionContext((ISessionFactoryImplementor) this);
        case "managed_web":
          return (ICurrentSessionContext) new ManagedWebSessionContext((ISessionFactoryImplementor) this);
        case "wcf_operation":
          return (ICurrentSessionContext) new WcfOperationSessionContext((ISessionFactoryImplementor) this);
        default:
          try
          {
            return (ICurrentSessionContext) NHibernate.Cfg.Environment.BytecodeProvider.ObjectsFactory.CreateInstance(ReflectHelper.ClassForName(name), (object) this);
          }
          catch (Exception ex)
          {
            SessionFactoryImpl.log.Error((object) ("Unable to construct current session context [" + name + "]"), ex);
            return (ICurrentSessionContext) null;
          }
      }
    }

    public string TryGetGuessEntityName(System.Type implementor)
    {
      string guessEntityName;
      this.implementorToEntityName.TryGetValue(implementor, out guessEntityName);
      return guessEntityName;
    }

    public string Name => this.name;

    public string Uuid => this.uuid;

    private class DefaultEntityNotFoundDelegate : IEntityNotFoundDelegate
    {
      public void HandleEntityNotFound(string entityName, object id)
      {
        throw new ObjectNotFoundException(id, entityName);
      }
    }
  }
}

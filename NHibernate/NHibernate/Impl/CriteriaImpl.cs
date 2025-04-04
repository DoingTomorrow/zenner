// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.CriteriaImpl
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Impl
{
  [Serializable]
  public class CriteriaImpl : ICriteria, ICloneable
  {
    private readonly Type persistentClass;
    private readonly System.Collections.Generic.List<CriteriaImpl.CriterionEntry> criteria = new System.Collections.Generic.List<CriteriaImpl.CriterionEntry>();
    private readonly System.Collections.Generic.List<CriteriaImpl.OrderEntry> orderEntries = new System.Collections.Generic.List<CriteriaImpl.OrderEntry>(10);
    private readonly Dictionary<string, FetchMode> fetchModes = new Dictionary<string, FetchMode>();
    private readonly Dictionary<string, LockMode> lockModes = new Dictionary<string, LockMode>();
    private int maxResults = RowSelection.NoValue;
    private int firstResult;
    private int timeout = RowSelection.NoValue;
    private int fetchSize = RowSelection.NoValue;
    private ISessionImplementor session;
    private IResultTransformer resultTransformer;
    private bool cacheable;
    private string cacheRegion;
    private CacheMode? cacheMode;
    private CacheMode? sessionCacheMode;
    private string comment;
    private FlushMode? flushMode;
    private FlushMode? sessionFlushMode;
    private bool? readOnly;
    private readonly System.Collections.Generic.List<CriteriaImpl.Subcriteria> subcriteriaList = new System.Collections.Generic.List<CriteriaImpl.Subcriteria>();
    private readonly string rootAlias;
    private readonly Dictionary<string, ICriteria> subcriteriaByPath = new Dictionary<string, ICriteria>();
    private readonly Dictionary<string, ICriteria> subcriteriaByAlias = new Dictionary<string, ICriteria>();
    private readonly string entityOrClassName;
    private IProjection projection;
    private ICriteria projectionCriteria;

    public CriteriaImpl(Type persistentClass, ISessionImplementor session)
      : this(persistentClass.FullName, CriteriaSpecification.RootAlias, session)
    {
      this.persistentClass = persistentClass;
    }

    public CriteriaImpl(Type persistentClass, string alias, ISessionImplementor session)
      : this(persistentClass.FullName, alias, session)
    {
      this.persistentClass = persistentClass;
    }

    public CriteriaImpl(string entityOrClassName, ISessionImplementor session)
      : this(entityOrClassName, CriteriaSpecification.RootAlias, session)
    {
    }

    public CriteriaImpl(string entityOrClassName, string alias, ISessionImplementor session)
    {
      this.session = session;
      this.entityOrClassName = entityOrClassName;
      this.cacheable = false;
      this.rootAlias = alias;
      this.subcriteriaByAlias[alias] = (ICriteria) this;
    }

    public ISessionImplementor Session
    {
      get => this.session;
      set => this.session = value;
    }

    public string EntityOrClassName => this.entityOrClassName;

    public IDictionary<string, LockMode> LockModes
    {
      get => (IDictionary<string, LockMode>) this.lockModes;
    }

    public ICriteria ProjectionCriteria => this.projectionCriteria;

    public bool LookupByNaturalKey
    {
      get
      {
        return this.projection == null && this.subcriteriaList.Count <= 0 && this.criteria.Count == 1 && this.criteria[0].Criterion is NaturalIdentifier;
      }
    }

    public string Alias => this.rootAlias;

    public IProjection Projection => this.projection;

    public bool IsReadOnlyInitialized => this.readOnly.HasValue;

    public bool IsReadOnly
    {
      get
      {
        if (!this.IsReadOnlyInitialized && this.Session == null)
          throw new InvalidOperationException("cannot determine readOnly/modifiable setting when it is not initialized and is not initialized and Session == null");
        return !this.IsReadOnlyInitialized ? this.Session.PersistenceContext.DefaultReadOnly : this.readOnly.Value;
      }
    }

    public FetchMode GetFetchMode(string path)
    {
      FetchMode fetchMode;
      if (!this.fetchModes.TryGetValue(path, out fetchMode))
        fetchMode = FetchMode.Default;
      return fetchMode;
    }

    public IResultTransformer ResultTransformer => this.resultTransformer;

    public int MaxResults => this.maxResults;

    public int FirstResult => this.firstResult;

    public int FetchSize => this.fetchSize;

    public int Timeout => this.timeout;

    public bool Cacheable => this.cacheable;

    public string CacheRegion => this.cacheRegion;

    public string Comment => this.comment;

    protected internal void Before()
    {
      if (this.flushMode.HasValue)
      {
        this.sessionFlushMode = new FlushMode?(this.Session.FlushMode);
        this.Session.FlushMode = this.flushMode.Value;
      }
      if (!this.cacheMode.HasValue)
        return;
      this.sessionCacheMode = new CacheMode?(this.Session.CacheMode);
      this.Session.CacheMode = this.cacheMode.Value;
    }

    protected internal void After()
    {
      if (this.sessionFlushMode.HasValue)
      {
        this.Session.FlushMode = this.sessionFlushMode.Value;
        this.sessionFlushMode = new FlushMode?();
      }
      if (!this.sessionCacheMode.HasValue)
        return;
      this.Session.CacheMode = this.sessionCacheMode.Value;
      this.sessionCacheMode = new CacheMode?();
    }

    public ICriteria SetMaxResults(int maxResults)
    {
      this.maxResults = maxResults;
      return (ICriteria) this;
    }

    public ICriteria SetFirstResult(int firstResult)
    {
      this.firstResult = firstResult;
      return (ICriteria) this;
    }

    public ICriteria SetTimeout(int timeout)
    {
      this.timeout = timeout;
      return (ICriteria) this;
    }

    public ICriteria SetFetchSize(int fetchSize)
    {
      this.fetchSize = fetchSize;
      return (ICriteria) this;
    }

    public ICriteria Add(ICriterion expression)
    {
      this.Add((ICriteria) this, expression);
      return (ICriteria) this;
    }

    public IList List()
    {
      ArrayList results = new ArrayList();
      this.List((IList) results);
      return (IList) results;
    }

    public void List(IList results)
    {
      this.Before();
      try
      {
        this.session.List(this, results);
      }
      finally
      {
        this.After();
      }
    }

    public IList<T> List<T>()
    {
      System.Collections.Generic.List<T> results = new System.Collections.Generic.List<T>();
      this.List((IList) results);
      return (IList<T>) results;
    }

    public T UniqueResult<T>()
    {
      object obj = this.UniqueResult();
      return obj == null && typeof (T).IsValueType ? default (T) : (T) obj;
    }

    public void ClearOrders() => this.orderEntries.Clear();

    public IEnumerable<CriteriaImpl.CriterionEntry> IterateExpressionEntries()
    {
      return (IEnumerable<CriteriaImpl.CriterionEntry>) this.criteria;
    }

    public IEnumerable<CriteriaImpl.OrderEntry> IterateOrderings()
    {
      return (IEnumerable<CriteriaImpl.OrderEntry>) this.orderEntries;
    }

    public IEnumerable<CriteriaImpl.Subcriteria> IterateSubcriteria()
    {
      return (IEnumerable<CriteriaImpl.Subcriteria>) this.subcriteriaList;
    }

    public override string ToString()
    {
      bool flag1 = true;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (CriteriaImpl.CriterionEntry criterion in this.criteria)
      {
        if (!flag1)
          stringBuilder.Append(" and ");
        stringBuilder.Append(criterion.ToString());
        flag1 = false;
      }
      if (this.orderEntries.Count != 0)
        stringBuilder.Append(Environment.NewLine);
      bool flag2 = true;
      foreach (CriteriaImpl.OrderEntry orderEntry in this.orderEntries)
      {
        if (!flag2)
          stringBuilder.Append(" and ");
        stringBuilder.Append(orderEntry.ToString());
        flag2 = false;
      }
      return stringBuilder.ToString();
    }

    public ICriteria AddOrder(Order ordering)
    {
      this.orderEntries.Add(new CriteriaImpl.OrderEntry(ordering, (ICriteria) this));
      return (ICriteria) this;
    }

    public ICriteria SetFetchMode(string associationPath, FetchMode mode)
    {
      this.fetchModes[associationPath] = mode;
      return (ICriteria) this;
    }

    public ICriteria CreateAlias(string associationPath, string alias)
    {
      this.CreateAlias(associationPath, alias, JoinType.InnerJoin);
      return (ICriteria) this;
    }

    public ICriteria CreateAlias(string associationPath, string alias, JoinType joinType)
    {
      CriteriaImpl.Subcriteria subcriteria = new CriteriaImpl.Subcriteria(this, (ICriteria) this, associationPath, alias, joinType);
      return (ICriteria) this;
    }

    public ICriteria CreateAlias(
      string associationPath,
      string alias,
      JoinType joinType,
      ICriterion withClause)
    {
      CriteriaImpl.Subcriteria subcriteria = new CriteriaImpl.Subcriteria(this, (ICriteria) this, associationPath, alias, joinType, withClause);
      return (ICriteria) this;
    }

    public ICriteria Add(ICriteria criteriaInst, ICriterion expression)
    {
      this.criteria.Add(new CriteriaImpl.CriterionEntry(expression, criteriaInst));
      return (ICriteria) this;
    }

    public ICriteria CreateCriteria(string associationPath)
    {
      return this.CreateCriteria(associationPath, JoinType.InnerJoin);
    }

    public ICriteria CreateCriteria(string associationPath, JoinType joinType)
    {
      return (ICriteria) new CriteriaImpl.Subcriteria(this, (ICriteria) this, associationPath, joinType);
    }

    public ICriteria CreateCriteria(string associationPath, string alias)
    {
      return this.CreateCriteria(associationPath, alias, JoinType.InnerJoin);
    }

    public ICriteria CreateCriteria(string associationPath, string alias, JoinType joinType)
    {
      return (ICriteria) new CriteriaImpl.Subcriteria(this, (ICriteria) this, associationPath, alias, joinType);
    }

    public ICriteria CreateCriteria(
      string associationPath,
      string alias,
      JoinType joinType,
      ICriterion withClause)
    {
      return (ICriteria) new CriteriaImpl.Subcriteria(this, (ICriteria) this, associationPath, alias, joinType, withClause);
    }

    public IFutureValue<T> FutureValue<T>()
    {
      if (!this.session.Factory.ConnectionProvider.Driver.SupportsMultipleQueries)
        return (IFutureValue<T>) new NHibernate.Impl.FutureValue<T>(new NHibernate.Impl.FutureValue<T>.GetResult(this.List<T>));
      this.session.FutureCriteriaBatch.Add<T>((ICriteria) this);
      return this.session.FutureCriteriaBatch.GetFutureValue<T>();
    }

    public IEnumerable<T> Future<T>()
    {
      if (!this.session.Factory.ConnectionProvider.Driver.SupportsMultipleQueries)
        return (IEnumerable<T>) this.List<T>();
      this.session.FutureCriteriaBatch.Add<T>((ICriteria) this);
      return this.session.FutureCriteriaBatch.GetEnumerator<T>();
    }

    public object UniqueResult() => AbstractQueryImpl.UniqueElement(this.List());

    public ICriteria SetLockMode(LockMode lockMode)
    {
      return this.SetLockMode(CriteriaSpecification.RootAlias, lockMode);
    }

    public ICriteria SetLockMode(string alias, LockMode lockMode)
    {
      this.lockModes[alias] = lockMode;
      return (ICriteria) this;
    }

    public ICriteria SetResultTransformer(IResultTransformer tupleMapper)
    {
      this.resultTransformer = tupleMapper;
      return (ICriteria) this;
    }

    public ICriteria SetCacheable(bool cacheable)
    {
      this.cacheable = cacheable;
      return (ICriteria) this;
    }

    public ICriteria SetCacheRegion(string cacheRegion)
    {
      this.cacheRegion = cacheRegion.Trim();
      return (ICriteria) this;
    }

    public ICriteria SetComment(string comment)
    {
      this.comment = comment;
      return (ICriteria) this;
    }

    public ICriteria SetFlushMode(FlushMode flushMode)
    {
      this.flushMode = new FlushMode?(flushMode);
      return (ICriteria) this;
    }

    public ICriteria SetProjection(params IProjection[] projections)
    {
      if (projections == null)
        throw new ArgumentNullException(nameof (projections));
      if (projections.Length == 0)
        throw new ArgumentException("projections must contain a least one projection");
      if (projections.Length == 1)
      {
        this.projection = projections[0];
      }
      else
      {
        ProjectionList projectionList = new ProjectionList();
        foreach (IProjection projection in projections)
          projectionList.Add(projection);
        this.projection = (IProjection) projectionList;
      }
      if (this.projection != null)
      {
        this.projectionCriteria = (ICriteria) this;
        this.SetResultTransformer(CriteriaSpecification.Projection);
      }
      return (ICriteria) this;
    }

    public ICriteria SetReadOnly(bool readOnly)
    {
      this.readOnly = new bool?(readOnly);
      return (ICriteria) this;
    }

    public ICriteria SetCacheMode(CacheMode cacheMode)
    {
      this.cacheMode = new CacheMode?(cacheMode);
      return (ICriteria) this;
    }

    public object Clone()
    {
      CriteriaImpl clone = this.persistentClass == null ? new CriteriaImpl(this.entityOrClassName, this.Alias, this.Session) : new CriteriaImpl(this.persistentClass, this.Alias, this.Session);
      this.CloneSubcriteria(clone);
      foreach (KeyValuePair<string, FetchMode> fetchMode in this.fetchModes)
        clone.fetchModes.Add(fetchMode.Key, fetchMode.Value);
      foreach (KeyValuePair<string, LockMode> lockMode in this.lockModes)
        clone.lockModes.Add(lockMode.Key, lockMode.Value);
      clone.maxResults = this.maxResults;
      clone.firstResult = this.firstResult;
      clone.timeout = this.timeout;
      clone.fetchSize = this.fetchSize;
      clone.cacheable = this.cacheable;
      clone.cacheRegion = this.cacheRegion;
      clone.SetProjection(new IProjection[1]
      {
        this.projection
      });
      this.CloneProjectCrtieria(clone);
      clone.SetResultTransformer(this.resultTransformer);
      clone.comment = this.comment;
      if (this.flushMode.HasValue)
        clone.SetFlushMode(this.flushMode.Value);
      if (this.cacheMode.HasValue)
        clone.SetCacheMode(this.cacheMode.Value);
      return (object) clone;
    }

    private void CloneProjectCrtieria(CriteriaImpl clone)
    {
      if (this.projectionCriteria == null)
        return;
      if (this.projectionCriteria == this)
      {
        clone.projectionCriteria = (ICriteria) clone;
      }
      else
      {
        ICriteria criteria = (ICriteria) this.projectionCriteria.Clone();
        clone.projectionCriteria = criteria;
      }
    }

    private void CloneSubcriteria(CriteriaImpl clone)
    {
      Dictionary<ICriteria, ICriteria> dictionary = new Dictionary<ICriteria, ICriteria>();
      dictionary[(ICriteria) this] = (ICriteria) clone;
      foreach (CriteriaImpl.Subcriteria key in this.IterateSubcriteria())
      {
        ICriteria parent;
        if (!dictionary.TryGetValue(key.Parent, out parent))
          throw new AssertionFailure("Could not find parent for subcriteria in the previous subcriteria. If you see this error, it is a bug");
        CriteriaImpl.Subcriteria subcriteria = new CriteriaImpl.Subcriteria(clone, parent, key.Path, key.Alias, key.JoinType, key.WithClause);
        subcriteria.SetLockMode(key.LockMode);
        dictionary[(ICriteria) key] = (ICriteria) subcriteria;
      }
      foreach (CriteriaImpl.OrderEntry iterateOrdering in this.IterateOrderings())
      {
        ICriteria criteria;
        if (!dictionary.TryGetValue(iterateOrdering.Criteria, out criteria))
          throw new AssertionFailure("Could not find parent for order in the previous criteria. If you see this error, it is a bug");
        criteria.AddOrder(iterateOrdering.Order);
      }
      foreach (CriteriaImpl.CriterionEntry criterion in this.criteria)
      {
        ICriteria criteria;
        if (!dictionary.TryGetValue(criterion.Criteria, out criteria))
          throw new AssertionFailure("Could not find parent for restriction in the previous criteria. If you see this error, it is a bug.");
        criteria.Add(criterion.Criterion);
      }
    }

    public ICriteria GetCriteriaByPath(string path)
    {
      ICriteria criteriaByPath;
      this.subcriteriaByPath.TryGetValue(path, out criteriaByPath);
      return criteriaByPath;
    }

    public ICriteria GetCriteriaByAlias(string alias)
    {
      ICriteria criteriaByAlias;
      this.subcriteriaByAlias.TryGetValue(alias, out criteriaByAlias);
      return criteriaByAlias;
    }

    public Type GetRootEntityTypeIfAvailable()
    {
      return this.persistentClass != null ? this.persistentClass : throw new HibernateException("Cannot provide root entity type because this criteria was initialized with an entity name.");
    }

    [Serializable]
    public sealed class Subcriteria : ICriteria, ICloneable
    {
      private readonly CriteriaImpl root;
      private readonly ICriteria parent;
      private string alias;
      private readonly string path;
      private LockMode lockMode;
      private readonly JoinType joinType;
      private ICriterion withClause;

      internal Subcriteria(
        CriteriaImpl root,
        ICriteria parent,
        string path,
        string alias,
        JoinType joinType,
        ICriterion withClause)
      {
        this.root = root;
        this.parent = parent;
        this.alias = alias;
        this.path = path;
        this.joinType = joinType;
        this.withClause = withClause;
        root.subcriteriaList.Add(this);
        root.subcriteriaByPath[path] = (ICriteria) this;
        this.SetAlias(alias);
      }

      internal Subcriteria(
        CriteriaImpl root,
        ICriteria parent,
        string path,
        string alias,
        JoinType joinType)
        : this(root, parent, path, alias, joinType, (ICriterion) null)
      {
      }

      internal Subcriteria(CriteriaImpl root, ICriteria parent, string path, JoinType joinType)
        : this(root, parent, path, (string) null, joinType)
      {
      }

      public ICriterion WithClause => this.withClause;

      public string Path => this.path;

      public ICriteria Parent => this.parent;

      public JoinType JoinType => this.joinType;

      public string Alias
      {
        get => this.alias;
        set => this.SetAlias(value);
      }

      public LockMode LockMode => this.lockMode;

      public bool IsReadOnlyInitialized => this.root.IsReadOnlyInitialized;

      public bool IsReadOnly => this.root.IsReadOnly;

      public ICriteria SetLockMode(LockMode lockMode)
      {
        this.lockMode = lockMode;
        return (ICriteria) this;
      }

      public ICriteria Add(ICriterion expression)
      {
        this.root.Add((ICriteria) this, expression);
        return (ICriteria) this;
      }

      public ICriteria AddOrder(Order order)
      {
        this.root.orderEntries.Add(new CriteriaImpl.OrderEntry(order, (ICriteria) this));
        return (ICriteria) this;
      }

      public ICriteria CreateAlias(string associationPath, string alias)
      {
        return this.CreateAlias(associationPath, alias, JoinType.InnerJoin);
      }

      public ICriteria CreateAlias(string associationPath, string alias, JoinType joinType)
      {
        CriteriaImpl.Subcriteria subcriteria = new CriteriaImpl.Subcriteria(this.root, (ICriteria) this, associationPath, alias, joinType);
        return (ICriteria) this;
      }

      public ICriteria CreateAlias(
        string associationPath,
        string alias,
        JoinType joinType,
        ICriterion withClause)
      {
        CriteriaImpl.Subcriteria subcriteria = new CriteriaImpl.Subcriteria(this.root, (ICriteria) this, associationPath, alias, joinType, withClause);
        return (ICriteria) this;
      }

      public ICriteria CreateCriteria(string associationPath)
      {
        return this.CreateCriteria(associationPath, JoinType.InnerJoin);
      }

      public ICriteria CreateCriteria(string associationPath, JoinType joinType)
      {
        return (ICriteria) new CriteriaImpl.Subcriteria(this.root, (ICriteria) this, associationPath, joinType);
      }

      public ICriteria CreateCriteria(string associationPath, string alias)
      {
        return this.CreateCriteria(associationPath, alias, JoinType.InnerJoin);
      }

      public ICriteria CreateCriteria(string associationPath, string alias, JoinType joinType)
      {
        return (ICriteria) new CriteriaImpl.Subcriteria(this.root, (ICriteria) this, associationPath, alias, joinType);
      }

      public ICriteria CreateCriteria(
        string associationPath,
        string alias,
        JoinType joinType,
        ICriterion withClause)
      {
        return (ICriteria) new CriteriaImpl.Subcriteria(this.root, (ICriteria) this, associationPath, alias, joinType, withClause);
      }

      public ICriteria SetCacheable(bool cacheable)
      {
        this.root.SetCacheable(cacheable);
        return (ICriteria) this;
      }

      public ICriteria SetCacheRegion(string cacheRegion)
      {
        this.root.SetCacheRegion(cacheRegion);
        return (ICriteria) this;
      }

      public IList List() => this.root.List();

      public IFutureValue<T> FutureValue<T>() => this.root.FutureValue<T>();

      public IEnumerable<T> Future<T>() => this.root.Future<T>();

      public void List(IList results) => this.root.List(results);

      public IList<T> List<T>() => this.root.List<T>();

      public T UniqueResult<T>()
      {
        object obj = this.UniqueResult();
        return obj != null || !typeof (T).IsValueType ? (T) obj : throw new InvalidCastException("UniqueResult<T>() cannot cast null result to value type. Call UniqueResult<T?>() instead");
      }

      public void ClearOrders() => this.root.ClearOrders();

      public object UniqueResult() => this.root.UniqueResult();

      public ICriteria SetFetchMode(string associationPath, FetchMode mode)
      {
        this.root.SetFetchMode(StringHelper.Qualify(this.path, associationPath), mode);
        return (ICriteria) this;
      }

      public ICriteria SetFlushMode(FlushMode flushMode)
      {
        this.root.SetFlushMode(flushMode);
        return (ICriteria) this;
      }

      public ICriteria SetCacheMode(CacheMode cacheMode)
      {
        this.root.SetCacheMode(cacheMode);
        return (ICriteria) this;
      }

      public ICriteria SetFirstResult(int firstResult)
      {
        this.root.SetFirstResult(firstResult);
        return (ICriteria) this;
      }

      public ICriteria SetMaxResults(int maxResults)
      {
        this.root.SetMaxResults(maxResults);
        return (ICriteria) this;
      }

      public ICriteria SetTimeout(int timeout)
      {
        this.root.SetTimeout(timeout);
        return (ICriteria) this;
      }

      public ICriteria SetFetchSize(int fetchSize)
      {
        this.root.SetFetchSize(fetchSize);
        return (ICriteria) this;
      }

      public ICriteria SetLockMode(string alias, LockMode lockMode)
      {
        this.root.SetLockMode(alias, lockMode);
        return (ICriteria) this;
      }

      public ICriteria SetResultTransformer(IResultTransformer resultProcessor)
      {
        this.root.SetResultTransformer(resultProcessor);
        return (ICriteria) this;
      }

      public ICriteria SetComment(string comment)
      {
        this.root.SetComment(comment);
        return (ICriteria) this;
      }

      public ICriteria SetProjection(params IProjection[] projections)
      {
        this.root.SetProjection(projections);
        return (ICriteria) this;
      }

      public ICriteria SetReadOnly(bool readOnly)
      {
        this.root.SetReadOnly(readOnly);
        return (ICriteria) this;
      }

      public ICriteria GetCriteriaByPath(string path) => this.root.GetCriteriaByPath(path);

      public ICriteria GetCriteriaByAlias(string alias) => this.root.GetCriteriaByAlias(alias);

      public Type GetRootEntityTypeIfAvailable() => this.root.GetRootEntityTypeIfAvailable();

      public object Clone() => this.root.Clone();

      private void SetAlias(string newAlias)
      {
        if (this.alias != null)
          this.root.subcriteriaByAlias.Remove(this.alias);
        if (newAlias != null)
          this.root.subcriteriaByAlias[newAlias] = (ICriteria) this;
        this.alias = newAlias;
      }
    }

    [Serializable]
    public sealed class CriterionEntry
    {
      private readonly ICriterion criterion;
      private readonly ICriteria criteria;

      internal CriterionEntry(ICriterion criterion, ICriteria criteria)
      {
        this.criterion = criterion;
        this.criteria = criteria;
      }

      public ICriterion Criterion => this.criterion;

      public ICriteria Criteria => this.criteria;

      public override string ToString() => this.criterion.ToString();
    }

    [Serializable]
    public sealed class OrderEntry
    {
      private readonly Order order;
      private readonly ICriteria criteria;

      internal OrderEntry(Order order, ICriteria criteria)
      {
        this.order = order;
        this.criteria = criteria;
      }

      public Order Order => this.order;

      public ICriteria Criteria => this.criteria;

      public override string ToString() => this.order.ToString();
    }
  }
}

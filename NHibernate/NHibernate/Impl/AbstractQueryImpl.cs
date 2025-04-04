// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.AbstractQueryImpl
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Properties;
using NHibernate.Proxy;
using NHibernate.Transform;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Impl
{
  public abstract class AbstractQueryImpl : IQuery
  {
    private readonly string queryString;
    private readonly ISessionImplementor session;
    protected internal ParameterMetadata parameterMetadata;
    private readonly RowSelection selection;
    private readonly ArrayList values = new ArrayList(4);
    private readonly System.Collections.Generic.List<IType> types = new System.Collections.Generic.List<IType>(4);
    private readonly Dictionary<string, TypedValue> namedParameters = new Dictionary<string, TypedValue>(4);
    protected readonly Dictionary<string, TypedValue> namedParameterLists = new Dictionary<string, TypedValue>(4);
    private bool cacheable;
    private string cacheRegion;
    private bool? readOnly;
    private static readonly object UNSET_PARAMETER = new object();
    private static readonly IType UNSET_TYPE = (IType) null;
    private object optionalId;
    private object optionalObject;
    private string optionalEntityName;
    private FlushMode flushMode = FlushMode.Unspecified;
    private FlushMode sessionFlushMode = FlushMode.Unspecified;
    private object collectionKey;
    private IResultTransformer resultTransformer;
    private bool shouldIgnoredUnknownNamedParameters;
    private CacheMode? cacheMode;
    private CacheMode? sessionCacheMode;
    private string comment;

    protected AbstractQueryImpl(
      string queryString,
      FlushMode flushMode,
      ISessionImplementor session,
      ParameterMetadata parameterMetadata)
    {
      this.session = session;
      this.queryString = queryString;
      this.selection = new RowSelection();
      this.flushMode = flushMode;
      this.cacheMode = new CacheMode?();
      this.parameterMetadata = parameterMetadata;
    }

    public bool Cacheable => this.cacheable;

    public string CacheRegion => this.cacheRegion;

    public bool HasNamedParameters => this.parameterMetadata.NamedParameterNames.Count > 0;

    protected internal virtual void VerifyParameters() => this.VerifyParameters(false);

    protected internal virtual void VerifyParameters(bool reserveFirstParameter)
    {
      if (this.parameterMetadata.NamedParameterNames.Count != this.namedParameters.Count + this.namedParameterLists.Count)
      {
        HashedSet<string> hashedSet = new HashedSet<string>(this.parameterMetadata.NamedParameterNames);
        hashedSet.RemoveAll((ICollection<string>) this.namedParameterLists.Keys);
        hashedSet.RemoveAll((ICollection<string>) this.namedParameters.Keys);
        throw new QueryException("Not all named parameters have been set: " + CollectionPrinter.ToString((ISet) hashedSet), this.QueryString);
      }
      int num = 0;
      for (int index = 0; index < this.values.Count; ++index)
      {
        object type = (object) this.types[index];
        if (this.values[index] == AbstractQueryImpl.UNSET_PARAMETER || type == AbstractQueryImpl.UNSET_TYPE)
        {
          if (!reserveFirstParameter || index != 0)
            throw new QueryException("Unset positional parameter at position: " + (object) index, this.QueryString);
        }
        else
          ++num;
      }
      if (this.parameterMetadata.OrdinalParameterCount == num)
        return;
      if (reserveFirstParameter && this.parameterMetadata.OrdinalParameterCount - 1 != num)
        throw new QueryException("Expected positional parameter count: " + (object) (this.parameterMetadata.OrdinalParameterCount - 1) + ", actual parameters: " + CollectionPrinter.ToString((IList) this.values), this.QueryString);
      if (!reserveFirstParameter)
        throw new QueryException("Expected positional parameter count: " + (object) this.parameterMetadata.OrdinalParameterCount + ", actual parameters: " + CollectionPrinter.ToString((IList) this.values), this.QueryString);
    }

    protected internal virtual IType DetermineType(
      int paramPosition,
      object paramValue,
      IType defaultType)
    {
      return this.parameterMetadata.GetOrdinalParameterExpectedType(paramPosition + 1) ?? defaultType;
    }

    protected internal virtual IType DetermineType(int paramPosition, object paramValue)
    {
      return this.parameterMetadata.GetOrdinalParameterExpectedType(paramPosition + 1) ?? this.GuessType(paramValue);
    }

    protected internal virtual IType DetermineType(
      string paramName,
      object paramValue,
      IType defaultType)
    {
      return this.parameterMetadata.GetNamedParameterExpectedType(paramName) ?? defaultType;
    }

    protected internal virtual IType DetermineType(string paramName, object paramValue)
    {
      return this.parameterMetadata.GetNamedParameterExpectedType(paramName) ?? this.GuessType(paramValue);
    }

    protected internal virtual IType DetermineType(string paramName, System.Type clazz)
    {
      return this.parameterMetadata.GetNamedParameterExpectedType(paramName) ?? this.GuessType(clazz);
    }

    private IType GuessType(object param)
    {
      return param != null ? this.GuessType(NHibernateProxyHelper.GetClassWithoutInitializingProxy(param)) : throw new ArgumentNullException(nameof (param), "The IType can not be guessed for a null value.");
    }

    private IType GuessType(System.Type clazz)
    {
      string typeName = clazz != null ? clazz.AssemblyQualifiedName : throw new ArgumentNullException(nameof (clazz), "The IType can not be guessed for a null value.");
      IType type = TypeFactory.HeuristicType(typeName);
      bool flag = type != null && type is SerializableType;
      if (type != null)
      {
        if (!flag)
          return type;
      }
      try
      {
        this.session.Factory.GetEntityPersister(clazz.FullName);
      }
      catch (MappingException ex)
      {
        if (flag)
          return type;
        throw new HibernateException("Could not determine a type for class: " + typeName);
      }
      return NHibernateUtil.Entity(clazz);
    }

    protected internal virtual string ExpandParameterLists(
      IDictionary<string, TypedValue> namedParamsCopy)
    {
      string query = this.queryString;
      foreach (KeyValuePair<string, TypedValue> namedParameterList in this.namedParameterLists)
        query = this.ExpandParameterList(query, namedParameterList.Key, namedParameterList.Value, namedParamsCopy);
      return query;
    }

    private string ExpandParameterList(
      string query,
      string name,
      TypedValue typedList,
      IDictionary<string, TypedValue> namedParamsCopy)
    {
      ICollection collection = (ICollection) typedList.Value;
      IType type = typedList.Type;
      if (collection.Count == 1)
      {
        IEnumerator enumerator = collection.GetEnumerator();
        enumerator.MoveNext();
        namedParamsCopy[name] = new TypedValue(type, enumerator.Current, this.session.EntityMode);
        return query;
      }
      StringBuilder stringBuilder = new StringBuilder(16);
      int num = 0;
      bool jpaStyle = this.parameterMetadata.GetNamedParameterDescriptor(name).JpaStyle;
      foreach (object obj in (IEnumerable) collection)
      {
        if (num > 0)
          stringBuilder.Append(", ");
        string key = (jpaStyle ? (object) ('x'.ToString() + name) : (object) (name + (object) '_')).ToString() + (object) num++ + (object) '_';
        namedParamsCopy[key] = new TypedValue(type, obj, this.session.EntityMode);
        stringBuilder.Append(":").Append(key);
      }
      string str = jpaStyle ? "?" : ":";
      return StringHelper.Replace(query, str + name, stringBuilder.ToString(), true);
    }

    public IQuery SetParameter(int position, object val, IType type)
    {
      this.CheckPositionalParameter(position);
      int count = this.values.Count;
      if (position < count)
      {
        this.values[position] = val;
        this.types[position] = type;
      }
      else
      {
        for (int index = 0; index < position - count; ++index)
        {
          this.values.Add(AbstractQueryImpl.UNSET_PARAMETER);
          this.types.Add(AbstractQueryImpl.UNSET_TYPE);
        }
        this.values.Add(val);
        this.types.Add(type);
      }
      return (IQuery) this;
    }

    public IQuery SetParameter(string name, object val, IType type)
    {
      if (!this.parameterMetadata.NamedParameterNames.Contains(name))
      {
        if (this.shouldIgnoredUnknownNamedParameters)
          return (IQuery) this;
        throw new ArgumentException("Parameter " + name + " does not exist as a named parameter in [" + this.QueryString + "]");
      }
      this.namedParameters[name] = new TypedValue(type, val, this.session.EntityMode);
      return (IQuery) this;
    }

    public IQuery SetParameter<T>(int position, T val)
    {
      this.CheckPositionalParameter(position);
      return this.SetParameter(position, (object) val, this.parameterMetadata.GetOrdinalParameterExpectedType(position + 1) ?? this.GuessType(typeof (T)));
    }

    private void CheckPositionalParameter(int position)
    {
      if (this.parameterMetadata.OrdinalParameterCount == 0)
        throw new ArgumentException("No positional parameters in query: " + this.QueryString);
      if (position < 0 || position > this.parameterMetadata.OrdinalParameterCount - 1)
        throw new ArgumentException("Positional parameter does not exist: " + (object) position + " in query: " + this.QueryString);
    }

    public IQuery SetParameter<T>(string name, T val)
    {
      return this.SetParameter(name, (object) val, this.parameterMetadata.GetNamedParameterExpectedType(name) ?? this.GuessType(typeof (T)));
    }

    public IQuery SetParameter(string name, object val)
    {
      if (!this.parameterMetadata.NamedParameterNames.Contains(name) && this.shouldIgnoredUnknownNamedParameters)
        return (IQuery) this;
      if (val == null)
        this.SetParameter(name, val, this.parameterMetadata.GetNamedParameterExpectedType(name) ?? throw new ArgumentNullException(nameof (val), "A type specific Set(name, val) should be called because the Type can not be guessed from a null value."));
      else
        this.SetParameter(name, val, this.DetermineType(name, val));
      return (IQuery) this;
    }

    public IQuery SetParameter(int position, object val)
    {
      if (val == null)
        throw new ArgumentNullException(nameof (val), "A type specific Set(position, val) should be called because the Type can not be guessed from a null value.");
      this.SetParameter(position, val, this.DetermineType(position, val));
      return (IQuery) this;
    }

    public IQuery SetAnsiString(int position, string val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.AnsiString);
      return (IQuery) this;
    }

    public IQuery SetString(int position, string val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.String);
      return (IQuery) this;
    }

    public IQuery SetCharacter(int position, char val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Character);
      return (IQuery) this;
    }

    public IQuery SetBoolean(int position, bool val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Boolean);
      return (IQuery) this;
    }

    public IQuery SetByte(int position, byte val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Byte);
      return (IQuery) this;
    }

    public IQuery SetInt16(int position, short val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Int16);
      return (IQuery) this;
    }

    public IQuery SetInt32(int position, int val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Int32);
      return (IQuery) this;
    }

    public IQuery SetInt64(int position, long val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Int64);
      return (IQuery) this;
    }

    public IQuery SetSingle(int position, float val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Single);
      return (IQuery) this;
    }

    public IQuery SetDouble(int position, double val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Double);
      return (IQuery) this;
    }

    public IQuery SetBinary(int position, byte[] val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Binary);
      return (IQuery) this;
    }

    public IQuery SetDateTimeOffset(string name, DateTimeOffset val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.DateTimeOffset);
      return (IQuery) this;
    }

    public IQuery SetDecimal(int position, Decimal val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Decimal);
      return (IQuery) this;
    }

    public IQuery SetDateTime(int position, DateTime val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.DateTime);
      return (IQuery) this;
    }

    public IQuery SetTime(int position, DateTime val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Time);
      return (IQuery) this;
    }

    public IQuery SetTimestamp(int position, DateTime val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Timestamp);
      return (IQuery) this;
    }

    public IQuery SetEntity(int position, object val)
    {
      this.SetParameter(position, val, NHibernateUtil.Entity(NHibernateProxyHelper.GuessClass(val)));
      return (IQuery) this;
    }

    public IQuery SetEnum(int position, Enum val)
    {
      this.SetParameter(position, (object) val, NHibernateUtil.Enum(val.GetType()));
      return (IQuery) this;
    }

    public IQuery SetAnsiString(string name, string val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.AnsiString);
      return (IQuery) this;
    }

    public IQuery SetString(string name, string val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.String);
      return (IQuery) this;
    }

    public IQuery SetCharacter(string name, char val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Character);
      return (IQuery) this;
    }

    public IQuery SetBoolean(string name, bool val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Boolean);
      return (IQuery) this;
    }

    public IQuery SetByte(string name, byte val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Byte);
      return (IQuery) this;
    }

    public IQuery SetInt16(string name, short val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Int16);
      return (IQuery) this;
    }

    public IQuery SetInt32(string name, int val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Int32);
      return (IQuery) this;
    }

    public IQuery SetInt64(string name, long val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Int64);
      return (IQuery) this;
    }

    public IQuery SetSingle(string name, float val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Single);
      return (IQuery) this;
    }

    public IQuery SetDouble(string name, double val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Double);
      return (IQuery) this;
    }

    public IQuery SetBinary(string name, byte[] val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Binary);
      return (IQuery) this;
    }

    public IQuery SetDecimal(string name, Decimal val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Decimal);
      return (IQuery) this;
    }

    public IQuery SetDateTime(string name, DateTime val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.DateTime);
      return (IQuery) this;
    }

    public IQuery SetDateTime2(int position, DateTime val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.DateTime2);
      return (IQuery) this;
    }

    public IQuery SetDateTime2(string name, DateTime val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.DateTime2);
      return (IQuery) this;
    }

    public IQuery SetTimeSpan(int position, TimeSpan val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.TimeSpan);
      return (IQuery) this;
    }

    public IQuery SetTimeSpan(string name, TimeSpan val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.TimeSpan);
      return (IQuery) this;
    }

    public IQuery SetTimeAsTimeSpan(int position, TimeSpan val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.TimeAsTimeSpan);
      return (IQuery) this;
    }

    public IQuery SetTimeAsTimeSpan(string name, TimeSpan val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.TimeAsTimeSpan);
      return (IQuery) this;
    }

    public IQuery SetDateTimeOffset(int position, DateTimeOffset val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.DateTimeOffset);
      return (IQuery) this;
    }

    public IQuery SetTime(string name, DateTime val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Time);
      return (IQuery) this;
    }

    public IQuery SetTimestamp(string name, DateTime val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Timestamp);
      return (IQuery) this;
    }

    public IQuery SetGuid(string name, Guid val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Guid);
      return (IQuery) this;
    }

    public IQuery SetGuid(int position, Guid val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Guid);
      return (IQuery) this;
    }

    public IQuery SetEntity(string name, object val)
    {
      this.SetParameter(name, val, NHibernateUtil.Entity(NHibernateProxyHelper.GuessClass(val)));
      return (IQuery) this;
    }

    public IQuery SetEnum(string name, Enum val)
    {
      this.SetParameter(name, (object) val, NHibernateUtil.Enum(val.GetType()));
      return (IQuery) this;
    }

    public IQuery SetProperties(IDictionary map)
    {
      foreach (string namedParameter in this.NamedParameters)
      {
        object obj = map[(object) namedParameter];
        if (obj != null)
        {
          System.Type type = obj.GetType();
          if (typeof (ICollection).IsAssignableFrom(type))
            this.SetParameterList(namedParameter, (IEnumerable) obj);
          else if (type.IsArray)
            this.SetParameterList(namedParameter, (IEnumerable) (object[]) obj);
          else
            this.SetParameter(namedParameter, obj, this.DetermineType(namedParameter, type));
        }
      }
      return (IQuery) this;
    }

    public IQuery SetProperties(object bean)
    {
      System.Type type = bean.GetType();
      foreach (string namedParameter in this.NamedParameters)
      {
        try
        {
          IGetter getter = ReflectHelper.GetGetter(type, namedParameter, "property");
          System.Type returnType = getter.ReturnType;
          object obj = getter.Get(bean);
          if (typeof (ICollection).IsAssignableFrom(returnType))
            this.SetParameterList(namedParameter, (IEnumerable) obj);
          else if (returnType.IsArray)
            this.SetParameterList(namedParameter, (IEnumerable) (object[]) obj);
          else
            this.SetParameter(namedParameter, obj, this.DetermineType(namedParameter, returnType));
        }
        catch (PropertyNotFoundException ex)
        {
        }
      }
      return (IQuery) this;
    }

    public IQuery SetParameterList(string name, IEnumerable vals, IType type)
    {
      if (!this.parameterMetadata.NamedParameterNames.Contains(name))
      {
        if (this.shouldIgnoredUnknownNamedParameters)
          return (IQuery) this;
        throw new ArgumentException("Parameter " + name + " does not exist as a named parameter in [" + this.QueryString + "]");
      }
      if (type == null)
        throw new ArgumentNullException(nameof (type), "Can't determine the type of parameter-list elements.");
      if (!vals.Any())
        throw new QueryException(string.Format("An empty parameter-list generate wrong SQL; parameter name '{0}'", (object) name));
      this.namedParameterLists[name] = new TypedValue(type, (object) vals, this.session.EntityMode);
      return (IQuery) this;
    }

    public IQuery SetParameterList(string name, IEnumerable vals)
    {
      if (vals == null)
        throw new ArgumentNullException(nameof (vals));
      if (!this.parameterMetadata.NamedParameterNames.Contains(name) && this.shouldIgnoredUnknownNamedParameters)
        return (IQuery) this;
      object paramValue = vals.FirstOrNull();
      this.SetParameterList(name, vals, paramValue == null ? this.GuessType(vals.GetCollectionElementType()) : this.DetermineType(name, paramValue));
      return (IQuery) this;
    }

    public string QueryString => this.queryString;

    protected internal IDictionary<string, TypedValue> NamedParams
    {
      get
      {
        return (IDictionary<string, TypedValue>) new Dictionary<string, TypedValue>((IDictionary<string, TypedValue>) this.namedParameters);
      }
    }

    protected IDictionary NamedParameterLists => (IDictionary) this.namedParameterLists;

    protected IList Values => (IList) this.values;

    protected IList<IType> Types => (IList<IType>) this.types;

    public virtual IType[] ReturnTypes => this.session.Factory.GetReturnTypes(this.queryString);

    public virtual string[] ReturnAliases
    {
      get => this.session.Factory.GetReturnAliases(this.queryString);
    }

    public RowSelection Selection => this.selection;

    public IQuery SetMaxResults(int maxResults)
    {
      this.selection.MaxRows = maxResults;
      return (IQuery) this;
    }

    public IQuery SetTimeout(int timeout)
    {
      this.selection.Timeout = timeout;
      return (IQuery) this;
    }

    public IQuery SetFetchSize(int fetchSize)
    {
      this.selection.FetchSize = fetchSize;
      return (IQuery) this;
    }

    public IQuery SetFirstResult(int firstResult)
    {
      this.selection.FirstRow = firstResult;
      return (IQuery) this;
    }

    public string[] NamedParameters
    {
      get => ArrayHelper.ToStringArray(this.parameterMetadata.NamedParameterNames);
    }

    public abstract IQuery SetLockMode(string alias, LockMode lockMode);

    public IQuery SetComment(string comment)
    {
      this.comment = comment;
      return (IQuery) this;
    }

    protected internal ISessionImplementor Session => this.session;

    protected RowSelection RowSelection => this.selection;

    public IQuery SetCacheable(bool cacheable)
    {
      this.cacheable = cacheable;
      return (IQuery) this;
    }

    public IQuery SetCacheRegion(string cacheRegion)
    {
      if (cacheRegion != null)
        this.cacheRegion = cacheRegion.Trim();
      return (IQuery) this;
    }

    public bool IsReadOnly
    {
      get
      {
        return this.readOnly.HasValue ? this.readOnly.Value : this.Session.PersistenceContext.DefaultReadOnly;
      }
    }

    public IQuery SetReadOnly(bool readOnly)
    {
      this.readOnly = new bool?(readOnly);
      return (IQuery) this;
    }

    public void SetOptionalId(object optionalId) => this.optionalId = optionalId;

    public void SetOptionalObject(object optionalObject) => this.optionalObject = optionalObject;

    public void SetOptionalEntityName(string optionalEntityName)
    {
      this.optionalEntityName = optionalEntityName;
    }

    public IQuery SetFlushMode(FlushMode flushMode)
    {
      this.flushMode = flushMode;
      return (IQuery) this;
    }

    public IQuery SetCollectionKey(object collectionKey)
    {
      this.collectionKey = collectionKey;
      return (IQuery) this;
    }

    public IQuery SetResultTransformer(IResultTransformer transformer)
    {
      this.resultTransformer = transformer;
      return (IQuery) this;
    }

    public IEnumerable<T> Future<T>()
    {
      if (!this.session.Factory.ConnectionProvider.Driver.SupportsMultipleQueries)
        return (IEnumerable<T>) this.List<T>();
      this.session.FutureQueryBatch.Add<T>((IQuery) this);
      return this.session.FutureQueryBatch.GetEnumerator<T>();
    }

    public IFutureValue<T> FutureValue<T>()
    {
      if (!this.session.Factory.ConnectionProvider.Driver.SupportsMultipleQueries)
        return (IFutureValue<T>) new NHibernate.Impl.FutureValue<T>(new NHibernate.Impl.FutureValue<T>.GetResult(this.List<T>));
      this.session.FutureQueryBatch.Add<T>((IQuery) this);
      return this.session.FutureQueryBatch.GetFutureValue<T>();
    }

    public IQuery SetCacheMode(CacheMode cacheMode)
    {
      this.cacheMode = new CacheMode?(cacheMode);
      return (IQuery) this;
    }

    public IQuery SetIgnoreUknownNamedParameters(bool ignoredUnknownNamedParameters)
    {
      this.shouldIgnoredUnknownNamedParameters = ignoredUnknownNamedParameters;
      return (IQuery) this;
    }

    protected internal abstract IDictionary<string, LockMode> LockModes { get; }

    public abstract int ExecuteUpdate();

    public abstract IEnumerable Enumerable();

    public abstract IEnumerable<T> Enumerable<T>();

    public abstract IList List();

    public abstract void List(IList results);

    public abstract IList<T> List<T>();

    public T UniqueResult<T>()
    {
      object obj = this.UniqueResult();
      return obj == null && typeof (T).IsValueType ? default (T) : (T) obj;
    }

    public object UniqueResult() => AbstractQueryImpl.UniqueElement(this.List());

    internal static object UniqueElement(IList list)
    {
      int count = list.Count;
      if (count == 0)
        return (object) null;
      object obj = list[0];
      for (int index = 1; index < count; ++index)
      {
        if (list[index] != obj)
          throw new NonUniqueResultException(count);
      }
      return obj;
    }

    public virtual IType[] TypeArray() => this.types.ToArray();

    public virtual object[] ValueArray() => (object[]) this.values.ToArray(typeof (object));

    public virtual QueryParameters GetQueryParameters()
    {
      return this.GetQueryParameters(this.NamedParams);
    }

    public virtual QueryParameters GetQueryParameters(IDictionary<string, TypedValue> namedParams)
    {
      IType[] positionalParameterTypes = this.TypeArray();
      object[] positionalParameterValues = this.ValueArray();
      IDictionary<string, TypedValue> namedParameters = namedParams;
      IDictionary<string, LockMode> lockModes = this.LockModes;
      RowSelection selection = this.Selection;
      int num1 = this.IsReadOnly ? 1 : 0;
      int num2 = this.cacheable ? 1 : 0;
      string cacheRegion = this.cacheRegion;
      string comment = this.comment;
      object[] collectionKeys;
      if (this.collectionKey != null)
        collectionKeys = new object[1]{ this.collectionKey };
      else
        collectionKeys = (object[]) null;
      object optionalObject = this.optionalObject;
      string optionalEntityName = this.optionalEntityName;
      object optionalId = this.optionalId;
      IResultTransformer resultTransformer = this.resultTransformer;
      return new QueryParameters(positionalParameterTypes, positionalParameterValues, namedParameters, lockModes, selection, true, num1 != 0, num2 != 0, cacheRegion, comment, collectionKeys, optionalObject, optionalEntityName, optionalId, resultTransformer);
    }

    protected void Before()
    {
      if (this.flushMode != FlushMode.Unspecified)
      {
        this.sessionFlushMode = this.Session.FlushMode;
        this.Session.FlushMode = this.flushMode;
      }
      if (!this.cacheMode.HasValue)
        return;
      this.sessionCacheMode = new CacheMode?(this.Session.CacheMode);
      this.Session.CacheMode = this.cacheMode.Value;
    }

    protected void After()
    {
      if (this.sessionFlushMode != FlushMode.Unspecified)
      {
        this.Session.FlushMode = this.sessionFlushMode;
        this.sessionFlushMode = FlushMode.Unspecified;
      }
      if (!this.sessionCacheMode.HasValue)
        return;
      this.Session.CacheMode = this.sessionCacheMode.Value;
      this.sessionCacheMode = new CacheMode?();
    }

    public override string ToString() => this.queryString;
  }
}

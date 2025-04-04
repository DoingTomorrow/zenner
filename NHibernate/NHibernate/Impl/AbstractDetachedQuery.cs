// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.AbstractDetachedQuery
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Proxy;
using NHibernate.Transform;
using NHibernate.Type;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Impl
{
  [Serializable]
  public abstract class AbstractDetachedQuery : IDetachedQuery, IDetachedQueryImplementor
  {
    protected readonly Dictionary<int, object> posUntypeParams = new Dictionary<int, object>(4);
    protected readonly Dictionary<string, object> namedUntypeParams = new Dictionary<string, object>();
    protected readonly Dictionary<string, ICollection> namedUntypeListParams = new Dictionary<string, ICollection>(2);
    protected readonly IList optionalUntypeParams = (IList) new ArrayList(2);
    protected readonly Dictionary<int, TypedValue> posParams = new Dictionary<int, TypedValue>(4);
    protected readonly Dictionary<string, TypedValue> namedParams = new Dictionary<string, TypedValue>();
    protected readonly Dictionary<string, TypedValue> namedListParams = new Dictionary<string, TypedValue>(2);
    protected readonly Dictionary<string, LockMode> lockModes = new Dictionary<string, LockMode>(2);
    protected readonly RowSelection selection = new RowSelection();
    protected bool cacheable;
    protected string cacheRegion;
    protected bool readOnly;
    protected FlushMode flushMode = FlushMode.Unspecified;
    protected IResultTransformer resultTransformer;
    protected bool shouldIgnoredUnknownNamedParameters;
    protected CacheMode? cacheMode;
    protected string comment;

    public abstract IQuery GetExecutableQuery(ISession session);

    public IDetachedQuery SetMaxResults(int maxResults)
    {
      this.selection.MaxRows = maxResults;
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetFirstResult(int firstResult)
    {
      this.selection.FirstRow = firstResult;
      return (IDetachedQuery) this;
    }

    public virtual IDetachedQuery SetComment(string comment)
    {
      this.comment = comment;
      return (IDetachedQuery) this;
    }

    public virtual IDetachedQuery SetCacheable(bool cacheable)
    {
      this.cacheable = cacheable;
      return (IDetachedQuery) this;
    }

    public virtual IDetachedQuery SetCacheRegion(string cacheRegion)
    {
      this.cacheRegion = cacheRegion;
      return (IDetachedQuery) this;
    }

    public virtual IDetachedQuery SetReadOnly(bool readOnly)
    {
      this.readOnly = readOnly;
      return (IDetachedQuery) this;
    }

    public virtual IDetachedQuery SetTimeout(int timeout)
    {
      this.selection.Timeout = timeout;
      return (IDetachedQuery) this;
    }

    public virtual IDetachedQuery SetFetchSize(int fetchSize)
    {
      this.selection.FetchSize = fetchSize;
      return (IDetachedQuery) this;
    }

    public void SetLockMode(string alias, LockMode lockMode)
    {
      if (string.IsNullOrEmpty(alias))
        throw new ArgumentNullException(nameof (alias), "Is null or empty.");
      this.lockModes[alias] = lockMode;
    }

    public IDetachedQuery SetParameter(int position, object val, IType type)
    {
      this.posParams[position] = new TypedValue(type, val, EntityMode.Poco);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetParameter(string name, object val, IType type)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentNullException(nameof (name), "Is null or empty.");
      this.namedParams[name] = new TypedValue(type, val, EntityMode.Poco);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetParameter(int position, object val)
    {
      this.posUntypeParams[position] = val;
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetParameter(string name, object val)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentNullException(nameof (name), "Is null or empty.");
      this.namedUntypeParams[name] = val;
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetParameterList(string name, ICollection vals, IType type)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentNullException(nameof (name), "Is null or empty.");
      this.namedListParams[name] = new TypedValue(type, (object) vals, EntityMode.Poco);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetParameterList(string name, ICollection vals)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentNullException(nameof (name), "Is null or empty.");
      this.namedUntypeListParams[name] = vals;
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetProperties(object obj)
    {
      if (obj == null)
        throw new ArgumentNullException(nameof (obj));
      this.optionalUntypeParams.Add(obj);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetAnsiString(int position, string val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.AnsiString);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetAnsiString(string name, string val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.AnsiString);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetBinary(int position, byte[] val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Binary);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetBinary(string name, byte[] val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Binary);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetBoolean(int position, bool val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Boolean);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetBoolean(string name, bool val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Boolean);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetByte(int position, byte val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Byte);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetByte(string name, byte val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Byte);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetCharacter(int position, char val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Character);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetCharacter(string name, char val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Character);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetDateTime(int position, DateTime val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.DateTime);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetDateTime(string name, DateTime val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.DateTime);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetDecimal(int position, Decimal val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Decimal);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetDecimal(string name, Decimal val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Decimal);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetDouble(int position, double val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Double);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetDouble(string name, double val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Double);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetEntity(int position, object val)
    {
      this.SetParameter(position, val, NHibernateUtil.Entity(NHibernateProxyHelper.GuessClass(val)));
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetEntity(string name, object val)
    {
      this.SetParameter(name, val, NHibernateUtil.Entity(NHibernateProxyHelper.GuessClass(val)));
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetEnum(int position, Enum val)
    {
      this.SetParameter(position, (object) val, NHibernateUtil.Enum(val.GetType()));
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetEnum(string name, Enum val)
    {
      this.SetParameter(name, (object) val, NHibernateUtil.Enum(val.GetType()));
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetInt16(int position, short val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Int16);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetInt16(string name, short val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Int16);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetInt32(int position, int val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Int32);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetInt32(string name, int val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Int32);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetInt64(int position, long val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Int64);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetInt64(string name, long val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Int64);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetSingle(int position, float val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Single);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetSingle(string name, float val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Single);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetString(int position, string val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.String);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetString(string name, string val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.String);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetTime(int position, DateTime val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Time);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetTime(string name, DateTime val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Time);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetTimestamp(int position, DateTime val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Timestamp);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetTimestamp(string name, DateTime val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Timestamp);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetGuid(int position, Guid val)
    {
      this.SetParameter(position, (object) val, (IType) NHibernateUtil.Guid);
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetGuid(string name, Guid val)
    {
      this.SetParameter(name, (object) val, (IType) NHibernateUtil.Guid);
      return (IDetachedQuery) this;
    }

    public virtual IDetachedQuery SetFlushMode(FlushMode flushMode)
    {
      this.flushMode = flushMode;
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetResultTransformer(IResultTransformer resultTransformer)
    {
      this.resultTransformer = resultTransformer;
      return (IDetachedQuery) this;
    }

    public IDetachedQuery SetIgnoreUknownNamedParameters(bool ignoredUnknownNamedParameters)
    {
      this.shouldIgnoredUnknownNamedParameters = ignoredUnknownNamedParameters;
      return (IDetachedQuery) this;
    }

    public virtual IDetachedQuery SetCacheMode(CacheMode cacheMode)
    {
      this.cacheMode = new CacheMode?(cacheMode);
      return (IDetachedQuery) this;
    }

    protected void SetQueryProperties(IQuery q)
    {
      q.SetMaxResults(this.selection.MaxRows).SetFirstResult(this.selection.FirstRow).SetCacheable(this.cacheable).SetReadOnly(this.readOnly).SetTimeout(this.selection.Timeout).SetFlushMode(this.flushMode).SetFetchSize(this.selection.FetchSize);
      if (!string.IsNullOrEmpty(this.comment))
        q.SetComment(this.comment);
      if (!string.IsNullOrEmpty(this.cacheRegion))
        q.SetCacheRegion(this.cacheRegion);
      if (this.resultTransformer != null)
        q.SetResultTransformer(this.resultTransformer);
      if (this.cacheMode.HasValue)
        q.SetCacheMode(this.cacheMode.Value);
      foreach (KeyValuePair<string, LockMode> lockMode in this.lockModes)
        q.SetLockMode(lockMode.Key, lockMode.Value);
      if (q is AbstractQueryImpl abstractQueryImpl)
        abstractQueryImpl.SetIgnoreUknownNamedParameters(this.shouldIgnoredUnknownNamedParameters);
      foreach (object optionalUntypeParam in (IEnumerable) this.optionalUntypeParams)
        q.SetProperties(optionalUntypeParam);
      foreach (KeyValuePair<int, object> posUntypeParam in this.posUntypeParams)
        q.SetParameter(posUntypeParam.Key, posUntypeParam.Value);
      foreach (KeyValuePair<string, object> namedUntypeParam in this.namedUntypeParams)
        q.SetParameter(namedUntypeParam.Key, namedUntypeParam.Value);
      foreach (KeyValuePair<string, ICollection> namedUntypeListParam in this.namedUntypeListParams)
        q.SetParameterList(namedUntypeListParam.Key, (IEnumerable) namedUntypeListParam.Value);
      foreach (KeyValuePair<int, TypedValue> posParam in this.posParams)
        q.SetParameter(posParam.Key, posParam.Value.Value, posParam.Value.Type);
      foreach (KeyValuePair<string, TypedValue> namedParam in this.namedParams)
        q.SetParameter(namedParam.Key, namedParam.Value.Value, namedParam.Value.Type);
      foreach (KeyValuePair<string, TypedValue> namedListParam in this.namedListParams)
        q.SetParameterList(namedListParam.Key, (IEnumerable) namedListParam.Value.Value, namedListParam.Value.Type);
    }

    private void Reset()
    {
      this.ClearParameters();
      this.lockModes.Clear();
      this.selection.FirstRow = RowSelection.NoValue;
      this.selection.MaxRows = RowSelection.NoValue;
      this.selection.Timeout = RowSelection.NoValue;
      this.selection.FetchSize = RowSelection.NoValue;
      this.cacheable = false;
      this.cacheRegion = (string) null;
      this.cacheMode = new CacheMode?();
      this.readOnly = false;
      this.flushMode = FlushMode.Unspecified;
      this.resultTransformer = (IResultTransformer) null;
      this.shouldIgnoredUnknownNamedParameters = false;
      this.comment = (string) null;
    }

    private void ClearParameters()
    {
      this.posUntypeParams.Clear();
      this.namedUntypeParams.Clear();
      this.namedUntypeListParams.Clear();
      this.optionalUntypeParams.Clear();
      this.posParams.Clear();
      this.namedParams.Clear();
      this.namedListParams.Clear();
    }

    public void CopyTo(IDetachedQuery destination)
    {
      destination.SetMaxResults(this.selection.MaxRows).SetFirstResult(this.selection.FirstRow).SetCacheable(this.cacheable).SetReadOnly(this.readOnly).SetTimeout(this.selection.Timeout).SetFlushMode(this.flushMode).SetFetchSize(this.selection.FetchSize);
      if (!string.IsNullOrEmpty(this.comment))
        destination.SetComment(this.comment);
      if (!string.IsNullOrEmpty(this.cacheRegion))
        destination.SetCacheRegion(this.cacheRegion);
      if (this.cacheMode.HasValue)
        destination.SetCacheMode(this.cacheMode.Value);
      if (this.resultTransformer != null)
        destination.SetResultTransformer(this.resultTransformer);
      foreach (KeyValuePair<string, LockMode> lockMode in this.lockModes)
        destination.SetLockMode(lockMode.Key, lockMode.Value);
      this.SetParametersTo(destination);
    }

    public void SetParametersTo(IDetachedQuery destination)
    {
      foreach (object optionalUntypeParam in (IEnumerable) this.optionalUntypeParams)
        destination.SetProperties(optionalUntypeParam);
      foreach (KeyValuePair<int, object> posUntypeParam in this.posUntypeParams)
        destination.SetParameter(posUntypeParam.Key, posUntypeParam.Value);
      foreach (KeyValuePair<string, object> namedUntypeParam in this.namedUntypeParams)
        destination.SetParameter(namedUntypeParam.Key, namedUntypeParam.Value);
      foreach (KeyValuePair<string, ICollection> namedUntypeListParam in this.namedUntypeListParams)
        destination.SetParameterList(namedUntypeListParam.Key, namedUntypeListParam.Value);
      foreach (KeyValuePair<int, TypedValue> posParam in this.posParams)
        destination.SetParameter(posParam.Key, posParam.Value.Value, posParam.Value.Type);
      foreach (KeyValuePair<string, TypedValue> namedParam in this.namedParams)
        destination.SetParameter(namedParam.Key, namedParam.Value.Value, namedParam.Value.Type);
      foreach (KeyValuePair<string, TypedValue> namedListParam in this.namedListParams)
        destination.SetParameterList(namedListParam.Key, (ICollection) namedListParam.Value.Value, namedListParam.Value.Type);
    }

    void IDetachedQueryImplementor.OverrideInfoFrom(IDetachedQueryImplementor origin)
    {
      this.Reset();
      origin.CopyTo((IDetachedQuery) this);
    }

    void IDetachedQueryImplementor.OverrideParametersFrom(IDetachedQueryImplementor origin)
    {
      this.ClearParameters();
      origin.SetParametersTo((IDetachedQuery) this);
    }

    public IDetachedQuery CopyParametersFrom(IDetachedQueryImplementor origin)
    {
      if (origin == null)
        throw new ArgumentNullException(nameof (origin));
      ((IDetachedQueryImplementor) this).OverrideParametersFrom(origin);
      return (IDetachedQuery) this;
    }
  }
}

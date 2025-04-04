// Decompiled with JetBrains decompiler
// Type: NHibernate.Impl.SqlQueryImpl
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Engine.Query.Sql;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Impl
{
  public class SqlQueryImpl : AbstractQueryImpl, ISQLQuery, IQuery
  {
    private readonly IList<INativeSQLQueryReturn> queryReturns;
    private readonly ICollection<string> querySpaces;
    private readonly bool callable;
    private bool autoDiscoverTypes;

    internal SqlQueryImpl(
      NamedSQLQueryDefinition queryDef,
      ISessionImplementor session,
      ParameterMetadata parameterMetadata)
      : base(queryDef.QueryString, queryDef.FlushMode, session, parameterMetadata)
    {
      if (!string.IsNullOrEmpty(queryDef.ResultSetRef))
        this.queryReturns = (IList<INativeSQLQueryReturn>) new System.Collections.Generic.List<INativeSQLQueryReturn>((IEnumerable<INativeSQLQueryReturn>) (session.Factory.GetResultSetMapping(queryDef.ResultSetRef) ?? throw new MappingException("Unable to find resultset-ref definition: " + queryDef.ResultSetRef)).GetQueryReturns());
      else
        this.queryReturns = (IList<INativeSQLQueryReturn>) new System.Collections.Generic.List<INativeSQLQueryReturn>((IEnumerable<INativeSQLQueryReturn>) queryDef.QueryReturns);
      this.querySpaces = (ICollection<string>) queryDef.QuerySpaces;
      this.callable = queryDef.IsCallable;
    }

    internal SqlQueryImpl(
      string sql,
      IList<INativeSQLQueryReturn> queryReturns,
      ICollection<string> querySpaces,
      FlushMode flushMode,
      bool callable,
      ISessionImplementor session,
      ParameterMetadata parameterMetadata)
      : base(sql, flushMode, session, parameterMetadata)
    {
      this.queryReturns = queryReturns;
      this.querySpaces = querySpaces;
      this.callable = callable;
    }

    internal SqlQueryImpl(
      string sql,
      string[] returnAliases,
      System.Type[] returnClasses,
      LockMode[] lockModes,
      ISessionImplementor session,
      ICollection<string> querySpaces,
      FlushMode flushMode,
      ParameterMetadata parameterMetadata)
      : base(sql, flushMode, session, parameterMetadata)
    {
      this.queryReturns = (IList<INativeSQLQueryReturn>) new System.Collections.Generic.List<INativeSQLQueryReturn>(returnAliases.Length);
      for (int index = 0; index < returnAliases.Length; ++index)
        this.queryReturns.Add((INativeSQLQueryReturn) new NativeSQLQueryRootReturn(returnAliases[index], returnClasses[index].FullName, lockModes == null ? LockMode.None : lockModes[index]));
      this.querySpaces = querySpaces;
      this.callable = false;
    }

    internal SqlQueryImpl(
      string sql,
      string[] returnAliases,
      System.Type[] returnClasses,
      ISessionImplementor session,
      ParameterMetadata parameterMetadata)
      : this(sql, returnAliases, returnClasses, (LockMode[]) null, session, (ICollection<string>) null, FlushMode.Unspecified, parameterMetadata)
    {
    }

    internal SqlQueryImpl(
      string sql,
      ISessionImplementor session,
      ParameterMetadata parameterMetadata)
      : base(sql, FlushMode.Unspecified, session, parameterMetadata)
    {
      this.queryReturns = (IList<INativeSQLQueryReturn>) new System.Collections.Generic.List<INativeSQLQueryReturn>();
      this.querySpaces = (ICollection<string>) null;
      this.callable = false;
    }

    protected internal override IDictionary<string, LockMode> LockModes
    {
      get => (IDictionary<string, LockMode>) new CollectionHelper.EmptyMapClass<string, LockMode>();
    }

    private INativeSQLQueryReturn[] GetQueryReturns()
    {
      INativeSQLQueryReturn[] array = new INativeSQLQueryReturn[this.queryReturns.Count];
      this.queryReturns.CopyTo(array, 0);
      return array;
    }

    public override string[] ReturnAliases
    {
      get
      {
        throw new NotSupportedException("SQL queries do not currently support returning aliases");
      }
    }

    public override IType[] ReturnTypes
    {
      get => throw new NotSupportedException("not yet implemented for SQL queries");
    }

    public override IList List()
    {
      this.VerifyParameters();
      IDictionary<string, TypedValue> namedParams = this.NamedParams;
      NativeSQLQuerySpecification querySpecification = this.GenerateQuerySpecification(namedParams);
      QueryParameters queryParameters = this.GetQueryParameters(namedParams);
      this.Before();
      try
      {
        return this.Session.List(querySpecification, queryParameters);
      }
      finally
      {
        this.After();
      }
    }

    public override void List(IList results)
    {
      this.VerifyParameters();
      IDictionary<string, TypedValue> namedParams = this.NamedParams;
      NativeSQLQuerySpecification querySpecification = this.GenerateQuerySpecification(namedParams);
      QueryParameters queryParameters = this.GetQueryParameters(namedParams);
      this.Before();
      try
      {
        this.Session.List(querySpecification, queryParameters, results);
      }
      finally
      {
        this.After();
      }
    }

    public override IList<T> List<T>()
    {
      this.VerifyParameters();
      IDictionary<string, TypedValue> namedParams = this.NamedParams;
      NativeSQLQuerySpecification querySpecification = this.GenerateQuerySpecification(namedParams);
      QueryParameters queryParameters = this.GetQueryParameters(namedParams);
      this.Before();
      try
      {
        return this.Session.List<T>(querySpecification, queryParameters);
      }
      finally
      {
        this.After();
      }
    }

    public NativeSQLQuerySpecification GenerateQuerySpecification(
      IDictionary<string, TypedValue> parameters)
    {
      return new NativeSQLQuerySpecification(this.ExpandParameterLists(parameters), this.GetQueryReturns(), this.querySpaces);
    }

    public override QueryParameters GetQueryParameters(IDictionary<string, TypedValue> namedParams)
    {
      QueryParameters queryParameters = base.GetQueryParameters(namedParams);
      queryParameters.Callable = this.callable;
      queryParameters.HasAutoDiscoverScalarTypes = this.autoDiscoverTypes;
      return queryParameters;
    }

    public override IEnumerable Enumerable()
    {
      throw new NotSupportedException("SQL queries do not currently support enumeration");
    }

    public override IEnumerable<T> Enumerable<T>()
    {
      throw new NotSupportedException("SQL queries do not currently support enumeration");
    }

    public ISQLQuery AddScalar(string columnAlias, IType type)
    {
      this.autoDiscoverTypes = true;
      this.queryReturns.Add((INativeSQLQueryReturn) new NativeSQLQueryScalarReturn(columnAlias, type));
      return (ISQLQuery) this;
    }

    public ISQLQuery AddJoin(string alias, string path) => this.AddJoin(alias, path, LockMode.Read);

    public ISQLQuery AddEntity(System.Type entityClass)
    {
      return this.AddEntity(entityClass.Name, entityClass);
    }

    public ISQLQuery AddEntity(string entityName)
    {
      return this.AddEntity(StringHelper.Unqualify(entityName), entityName);
    }

    public ISQLQuery AddEntity(string alias, string entityName)
    {
      return this.AddEntity(alias, entityName, LockMode.Read);
    }

    public ISQLQuery AddEntity(string alias, System.Type entityClass)
    {
      return this.AddEntity(alias, entityClass.FullName);
    }

    public ISQLQuery AddJoin(string alias, string path, LockMode lockMode)
    {
      int length = path.IndexOf('.');
      string ownerAlias = length >= 0 ? path.Substring(0, length) : throw new QueryException("not a property path: " + path);
      string ownerProperty = path.Substring(length + 1);
      this.queryReturns.Add((INativeSQLQueryReturn) new NativeSQLQueryJoinReturn(alias, ownerAlias, ownerProperty, (IDictionary<string, string[]>) new CollectionHelper.EmptyMapClass<string, string[]>(), lockMode));
      return (ISQLQuery) this;
    }

    public ISQLQuery AddEntity(string alias, string entityName, LockMode lockMode)
    {
      this.queryReturns.Add((INativeSQLQueryReturn) new NativeSQLQueryRootReturn(alias, entityName, lockMode));
      return (ISQLQuery) this;
    }

    public ISQLQuery AddEntity(string alias, System.Type entityClass, LockMode lockMode)
    {
      return this.AddEntity(alias, entityClass.FullName, lockMode);
    }

    public ISQLQuery SetResultSetMapping(string name)
    {
      INativeSQLQueryReturn[] queryReturns = (this.Session.Factory.GetResultSetMapping(name) ?? throw new MappingException("Unknown SqlResultSetMapping [" + name + "]")).GetQueryReturns();
      int length = queryReturns.Length;
      for (int index = 0; index < length; ++index)
        this.queryReturns.Add(queryReturns[index]);
      return (ISQLQuery) this;
    }

    protected internal override void VerifyParameters()
    {
      base.VerifyParameters();
      bool flag = this.queryReturns == null || this.queryReturns.Count == 0;
      if (flag)
      {
        this.autoDiscoverTypes = flag;
      }
      else
      {
        foreach (INativeSQLQueryReturn queryReturn in (IEnumerable<INativeSQLQueryReturn>) this.queryReturns)
        {
          if (queryReturn is NativeSQLQueryScalarReturn && ((NativeSQLQueryScalarReturn) queryReturn).Type == null)
          {
            this.autoDiscoverTypes = true;
            break;
          }
        }
      }
    }

    public override IQuery SetLockMode(string alias, LockMode lockMode)
    {
      throw new NotSupportedException("cannot set the lock mode for a native SQL query");
    }

    public override int ExecuteUpdate()
    {
      IDictionary<string, TypedValue> namedParams = this.NamedParams;
      this.Before();
      try
      {
        return this.Session.ExecuteNativeUpdate(this.GenerateQuerySpecification(namedParams), this.GetQueryParameters(namedParams));
      }
      finally
      {
        this.After();
      }
    }
  }
}

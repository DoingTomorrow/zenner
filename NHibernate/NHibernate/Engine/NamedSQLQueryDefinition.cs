// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.NamedSQLQueryDefinition
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine.Query.Sql;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Engine
{
  [Serializable]
  public class NamedSQLQueryDefinition : NamedQueryDefinition
  {
    private readonly INativeSQLQueryReturn[] queryReturns;
    private readonly IList<string> querySpaces;
    private readonly bool callable;
    private readonly string resultSetRef;

    public NamedSQLQueryDefinition(
      string query,
      INativeSQLQueryReturn[] queryReturns,
      IList<string> querySpaces,
      bool cacheable,
      string cacheRegion,
      int timeout,
      int fetchSize,
      FlushMode flushMode,
      NHibernate.CacheMode? cacheMode,
      bool readOnly,
      string comment,
      IDictionary<string, string> parameterTypes,
      bool callable)
      : base(query.Trim(), cacheable, cacheRegion, timeout, fetchSize, flushMode, cacheMode, readOnly, comment, parameterTypes)
    {
      this.queryReturns = queryReturns;
      this.querySpaces = querySpaces;
      this.callable = callable;
    }

    public NamedSQLQueryDefinition(
      string query,
      string resultSetRef,
      IList<string> querySpaces,
      bool cacheable,
      string cacheRegion,
      int timeout,
      int fetchSize,
      FlushMode flushMode,
      NHibernate.CacheMode? cacheMode,
      bool readOnly,
      string comment,
      IDictionary<string, string> parameterTypes,
      bool callable)
      : base(query.Trim(), cacheable, cacheRegion, timeout, fetchSize, flushMode, cacheMode, readOnly, comment, parameterTypes)
    {
      this.resultSetRef = resultSetRef;
      this.querySpaces = querySpaces;
      this.callable = callable;
    }

    public INativeSQLQueryReturn[] QueryReturns => this.queryReturns;

    public IList<string> QuerySpaces => this.querySpaces;

    public bool IsCallable => this.callable;

    public string ResultSetRef => this.resultSetRef;
  }
}

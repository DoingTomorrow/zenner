// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.NamedQueryDefinition
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Engine
{
  [Serializable]
  public class NamedQueryDefinition
  {
    private readonly string query;
    private readonly bool cacheable;
    private readonly string cacheRegion;
    private readonly int timeout = -1;
    private readonly int fetchSize = -1;
    private readonly FlushMode flushMode = FlushMode.Unspecified;
    private readonly IDictionary<string, string> parameterTypes;
    private readonly NHibernate.CacheMode? cacheMode;
    private readonly bool readOnly;
    private readonly string comment;

    public NamedQueryDefinition(
      string query,
      bool cacheable,
      string cacheRegion,
      int timeout,
      int fetchSize,
      FlushMode flushMode,
      bool readOnly,
      string comment,
      IDictionary<string, string> parameterTypes)
      : this(query, cacheable, cacheRegion, timeout, fetchSize, flushMode, new NHibernate.CacheMode?(), readOnly, comment, parameterTypes)
    {
    }

    public NamedQueryDefinition(
      string query,
      bool cacheable,
      string cacheRegion,
      int timeout,
      int fetchSize,
      FlushMode flushMode,
      NHibernate.CacheMode? cacheMode,
      bool readOnly,
      string comment,
      IDictionary<string, string> parameterTypes)
    {
      this.query = query;
      this.cacheable = cacheable;
      this.cacheRegion = cacheRegion;
      this.timeout = timeout;
      this.fetchSize = fetchSize;
      this.flushMode = flushMode;
      this.parameterTypes = parameterTypes;
      this.cacheMode = cacheMode;
      this.readOnly = readOnly;
      this.comment = comment;
    }

    public string QueryString => this.query;

    public bool IsCacheable => this.cacheable;

    public string CacheRegion => this.cacheRegion;

    public int FetchSize => this.fetchSize;

    public int Timeout => this.timeout;

    public FlushMode FlushMode => this.flushMode;

    public override string ToString()
    {
      return this.GetType().FullName + (object) '(' + this.query + (object) ')';
    }

    public IDictionary<string, string> ParameterTypes => this.parameterTypes;

    public string Query => this.query;

    public bool IsReadOnly => this.readOnly;

    public string Comment => this.comment;

    public NHibernate.CacheMode? CacheMode => this.cacheMode;
  }
}

// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.NamedQueryDefinitionBuilder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cfg.Loquacious
{
  internal class NamedQueryDefinitionBuilder : INamedQueryDefinitionBuilder
  {
    private int fetchSize = -1;
    private int timeout = -1;

    public NamedQueryDefinitionBuilder() => this.FlushMode = FlushMode.Unspecified;

    public bool IsCacheable { get; set; }

    public string CacheRegion { get; set; }

    public int FetchSize
    {
      get => this.fetchSize;
      set
      {
        if (value > 0)
          this.fetchSize = value;
        else
          this.fetchSize = -1;
      }
    }

    public int Timeout
    {
      get => this.timeout;
      set
      {
        if (value > 0)
          this.timeout = value;
        else
          this.timeout = -1;
      }
    }

    public FlushMode FlushMode { get; set; }

    public string Query { get; set; }

    public bool IsReadOnly { get; set; }

    public string Comment { get; set; }

    public NHibernate.CacheMode? CacheMode { get; set; }

    public NamedQueryDefinition Build()
    {
      return new NamedQueryDefinition(this.Query, this.IsCacheable, this.CacheRegion, this.Timeout, this.FetchSize, this.FlushMode, this.CacheMode, this.IsReadOnly, this.Comment, (IDictionary<string, string>) new Dictionary<string, string>(1));
    }
  }
}

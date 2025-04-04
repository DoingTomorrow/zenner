// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.CacheInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class CacheInstance : CacheInspector, ICacheInstance, ICacheInspector, IInspector
  {
    private const int layer = 1;
    private readonly CacheMapping mapping;

    public CacheInstance(CacheMapping mapping)
      : base(mapping)
    {
      this.mapping = mapping;
    }

    public void ReadWrite()
    {
      this.mapping.Set<string>((Expression<Func<CacheMapping, string>>) (x => x.Usage), 1, "read-write");
    }

    public void NonStrictReadWrite()
    {
      this.mapping.Set<string>((Expression<Func<CacheMapping, string>>) (x => x.Usage), 1, "nonstrict-read-write");
    }

    public void ReadOnly()
    {
      this.mapping.Set<string>((Expression<Func<CacheMapping, string>>) (x => x.Usage), 1, "read-only");
    }

    public void Transactional()
    {
      this.mapping.Set<string>((Expression<Func<CacheMapping, string>>) (x => x.Usage), 1, "transactional");
    }

    public void IncludeAll()
    {
      this.mapping.Set<string>((Expression<Func<CacheMapping, string>>) (x => x.Include), 1, "all");
    }

    public void IncludeNonLazy()
    {
      this.mapping.Set<string>((Expression<Func<CacheMapping, string>>) (x => x.Include), 1, "non-lazy");
    }

    public void CustomInclude(string include)
    {
      this.mapping.Set<string>((Expression<Func<CacheMapping, string>>) (x => x.Include), 1, include);
    }

    public void CustomUsage(string custom)
    {
      this.mapping.Set<string>((Expression<Func<CacheMapping, string>>) (x => x.Usage), 1, custom);
    }

    public void Region(string name)
    {
      this.mapping.Set<string>((Expression<Func<CacheMapping, string>>) (x => x.Region), 1, name);
    }
  }
}

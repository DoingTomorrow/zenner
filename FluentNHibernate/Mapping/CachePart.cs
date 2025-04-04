// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.CachePart
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using System;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class CachePart : ICacheMappingProvider
  {
    private readonly Type entityType;
    private readonly AttributeStore attributes = new AttributeStore();

    public CachePart(Type entityType) => this.entityType = entityType;

    public CachePart ReadWrite()
    {
      this.attributes.Set("Usage", 2, (object) "read-write");
      return this;
    }

    public CachePart NonStrictReadWrite()
    {
      this.attributes.Set("Usage", 2, (object) "nonstrict-read-write");
      return this;
    }

    public CachePart ReadOnly()
    {
      this.attributes.Set("Usage", 2, (object) "read-only");
      return this;
    }

    public CachePart Transactional()
    {
      this.attributes.Set("Usage", 2, (object) "transactional");
      return this;
    }

    public CachePart CustomUsage(string custom)
    {
      this.attributes.Set("Usage", 2, (object) custom);
      return this;
    }

    public CachePart Region(string name)
    {
      this.attributes.Set(nameof (Region), 2, (object) name);
      return this;
    }

    public CachePart IncludeAll()
    {
      this.attributes.Set("Include", 2, (object) "all");
      return this;
    }

    public CachePart IncludeNonLazy()
    {
      this.attributes.Set("Include", 2, (object) "non-lazy");
      return this;
    }

    public CachePart CustomInclude(string custom)
    {
      this.attributes.Set("Include", 2, (object) custom);
      return this;
    }

    internal bool IsDirty
    {
      get
      {
        return this.attributes.IsSpecified("Region") || this.attributes.IsSpecified("Usage") || this.attributes.IsSpecified("Include");
      }
    }

    CacheMapping ICacheMappingProvider.GetCacheMapping()
    {
      return new CacheMapping(this.attributes.Clone())
      {
        ContainedEntityType = this.entityType
      };
    }
  }
}

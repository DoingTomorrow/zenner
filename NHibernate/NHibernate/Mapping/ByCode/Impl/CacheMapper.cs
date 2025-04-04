// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.CacheMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class CacheMapper : ICacheMapper
  {
    private readonly HbmCache cacheMapping;

    public CacheMapper(HbmCache cacheMapping)
    {
      this.cacheMapping = cacheMapping != null ? cacheMapping : throw new ArgumentNullException(nameof (cacheMapping));
      this.Usage(CacheUsage.Transactional);
    }

    public void Usage(CacheUsage cacheUsage) => this.cacheMapping.usage = cacheUsage.ToHbm();

    public void Region(string regionName) => this.cacheMapping.region = regionName;

    public void Include(CacheInclude cacheInclude)
    {
      this.cacheMapping.include = cacheInclude.ToHbm();
    }
  }
}

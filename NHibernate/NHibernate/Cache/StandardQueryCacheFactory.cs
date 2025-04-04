// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.StandardQueryCacheFactory
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cache
{
  public class StandardQueryCacheFactory : IQueryCacheFactory
  {
    public IQueryCache GetQueryCache(
      string regionName,
      UpdateTimestampsCache updateTimestampsCache,
      Settings settings,
      IDictionary<string, string> props)
    {
      return (IQueryCache) new StandardQueryCache(settings, props, updateTimestampsCache, regionName);
    }
  }
}

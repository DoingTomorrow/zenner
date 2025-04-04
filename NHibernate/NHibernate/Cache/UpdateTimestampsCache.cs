// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.UpdateTimestampsCache
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace NHibernate.Cache
{
  public class UpdateTimestampsCache
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (UpdateTimestampsCache));
    private ICache updateTimestamps;
    private readonly string regionName = typeof (UpdateTimestampsCache).Name;

    public void Clear() => this.updateTimestamps.Clear();

    public UpdateTimestampsCache(Settings settings, IDictionary<string, string> props)
    {
      string cacheRegionPrefix = settings.CacheRegionPrefix;
      this.regionName = cacheRegionPrefix == null ? this.regionName : cacheRegionPrefix + (object) '.' + this.regionName;
      UpdateTimestampsCache.log.Info((object) ("starting update timestamps cache at region: " + this.regionName));
      this.updateTimestamps = settings.CacheProvider.BuildCache(this.regionName, props);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void PreInvalidate(object[] spaces)
    {
      long num = this.updateTimestamps.NextTimestamp() + (long) this.updateTimestamps.Timeout;
      for (int index = 0; index < spaces.Length; ++index)
        this.updateTimestamps.Put(spaces[index], (object) num);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Invalidate(object[] spaces)
    {
      long num = this.updateTimestamps.NextTimestamp();
      for (int index = 0; index < spaces.Length; ++index)
      {
        UpdateTimestampsCache.log.Debug((object) string.Format("Invalidating space [{0}]", spaces[index]));
        this.updateTimestamps.Put(spaces[index], (object) num);
      }
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public bool IsUpToDate(ISet<string> spaces, long timestamp)
    {
      foreach (object space in (IEnumerable<string>) spaces)
      {
        object obj = this.updateTimestamps.Get(space);
        if (obj != null && (long) obj >= timestamp)
          return false;
      }
      return true;
    }

    public void Destroy()
    {
      try
      {
        this.updateTimestamps.Destroy();
      }
      catch (Exception ex)
      {
        UpdateTimestampsCache.log.Warn((object) "could not destroy UpdateTimestamps cache", ex);
      }
    }
  }
}

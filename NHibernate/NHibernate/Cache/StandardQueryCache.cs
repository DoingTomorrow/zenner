// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.StandardQueryCache
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cache
{
  public class StandardQueryCache : IQueryCache
  {
    private static readonly IInternalLogger Log = LoggerProvider.LoggerFor(typeof (StandardQueryCache));
    private readonly ICache _queryCache;
    private readonly string _regionName;
    private readonly UpdateTimestampsCache _updateTimestampsCache;

    public StandardQueryCache(
      Settings settings,
      IDictionary<string, string> props,
      UpdateTimestampsCache updateTimestampsCache,
      string regionName)
    {
      if (regionName == null)
        regionName = typeof (StandardQueryCache).FullName;
      string cacheRegionPrefix = settings.CacheRegionPrefix;
      if (!string.IsNullOrEmpty(cacheRegionPrefix))
        regionName = cacheRegionPrefix + (object) '.' + regionName;
      StandardQueryCache.Log.Info((object) ("starting query cache at region: " + regionName));
      this._queryCache = settings.CacheProvider.BuildCache(regionName, props);
      this._updateTimestampsCache = updateTimestampsCache;
      this._regionName = regionName;
    }

    public ICache Cache => this._queryCache;

    public string RegionName => this._regionName;

    public void Clear() => this._queryCache.Clear();

    public bool Put(
      QueryKey key,
      ICacheAssembler[] returnTypes,
      IList result,
      bool isNaturalKeyLookup,
      ISessionImplementor session)
    {
      if (isNaturalKeyLookup && result.Count == 0)
        return false;
      long timestamp = session.Timestamp;
      if (StandardQueryCache.Log.IsDebugEnabled)
        StandardQueryCache.Log.DebugFormat("caching query results in region: '{0}'; {1}", (object) this._regionName, (object) key);
      IList list = (IList) new List<object>(result.Count + 1)
      {
        (object) timestamp
      };
      for (int index = 0; index < result.Count; ++index)
      {
        if (returnTypes.Length == 1 && !key.HasResultTransformer)
          list.Add(returnTypes[0].Disassemble(result[index], session, (object) null));
        else
          list.Add((object) TypeHelper.Disassemble((object[]) result[index], returnTypes, (bool[]) null, session, (object) null));
      }
      this._queryCache.Put((object) key, (object) list);
      return true;
    }

    public IList Get(
      QueryKey key,
      ICacheAssembler[] returnTypes,
      bool isNaturalKeyLookup,
      ISet<string> spaces,
      ISessionImplementor session)
    {
      if (StandardQueryCache.Log.IsDebugEnabled)
        StandardQueryCache.Log.DebugFormat("checking cached query results in region: '{0}'; {1}", (object) this._regionName, (object) key);
      IList list1 = (IList) this._queryCache.Get((object) key);
      if (list1 == null)
      {
        StandardQueryCache.Log.DebugFormat("query results were not found in cache: {0}", (object) key);
        return (IList) null;
      }
      long timestamp = (long) list1[0];
      if (StandardQueryCache.Log.IsDebugEnabled)
        StandardQueryCache.Log.DebugFormat("Checking query spaces for up-to-dateness [{0}]", (object) StringHelper.CollectionToString((ICollection) spaces));
      if (!isNaturalKeyLookup && !this.IsUpToDate(spaces, timestamp))
      {
        StandardQueryCache.Log.DebugFormat("cached query results were not up to date for: {0}", (object) key);
        return (IList) null;
      }
      StandardQueryCache.Log.DebugFormat("returning cached query results for: {0}", (object) key);
      for (int index = 1; index < list1.Count; ++index)
      {
        if (returnTypes.Length == 1 && !key.HasResultTransformer)
          returnTypes[0].BeforeAssemble(list1[index], session);
        else
          TypeHelper.BeforeAssemble((object[]) list1[index], returnTypes, session);
      }
      IList list2 = (IList) new List<object>(list1.Count - 1);
      for (int index = 1; index < list1.Count; ++index)
      {
        try
        {
          if (returnTypes.Length == 1 && !key.HasResultTransformer)
            list2.Add(returnTypes[0].Assemble(list1[index], session, (object) null));
          else
            list2.Add((object) TypeHelper.Assemble((object[]) list1[index], returnTypes, session, (object) null));
        }
        catch (UnresolvableObjectException ex)
        {
          if (isNaturalKeyLookup)
          {
            StandardQueryCache.Log.Debug((object) "could not reassemble cached result set");
            this._queryCache.Remove((object) key);
            return (IList) null;
          }
          throw;
        }
      }
      return list2;
    }

    public void Destroy()
    {
      try
      {
        this._queryCache.Destroy();
      }
      catch (Exception ex)
      {
        StandardQueryCache.Log.Warn((object) ("could not destroy query cache: " + this._regionName), ex);
      }
    }

    protected virtual bool IsUpToDate(ISet<string> spaces, long timestamp)
    {
      return this._updateTimestampsCache.IsUpToDate(spaces, timestamp);
    }
  }
}

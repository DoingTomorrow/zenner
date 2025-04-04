// Decompiled with JetBrains decompiler
// Type: NHibernate.Stat.SecondLevelCacheStatistics
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;
using System;
using System.Collections;
using System.Text;

#nullable disable
namespace NHibernate.Stat
{
  [Serializable]
  public class SecondLevelCacheStatistics : CategorizedStatistics
  {
    [NonSerialized]
    private readonly ICache cache;
    internal long hitCount;
    internal long missCount;
    internal long putCount;

    public SecondLevelCacheStatistics(ICache cache)
      : base(cache.RegionName)
    {
      this.cache = cache;
    }

    public long HitCount => this.hitCount;

    public long MissCount => this.missCount;

    public long PutCount => this.putCount;

    public long ElementCountInMemory => -1;

    public long ElementCountOnDisk => -1;

    public long SizeInMemory => -1;

    public IDictionary Entries => (IDictionary) new Hashtable();

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder().Append("SecondLevelCacheStatistics[").Append("hitCount=").Append(this.hitCount).Append(",missCount=").Append(this.missCount).Append(",putCount=").Append(this.putCount);
      if (this.cache != null)
        stringBuilder.Append(",elementCountInMemory=").Append(this.ElementCountInMemory).Append(",elementCountOnDisk=").Append(this.ElementCountOnDisk).Append(",sizeInMemory=").Append(this.SizeInMemory);
      stringBuilder.Append(']');
      return stringBuilder.ToString();
    }
  }
}

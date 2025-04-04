// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.CachedItem
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections;

#nullable disable
namespace NHibernate.Cache
{
  [Serializable]
  public class CachedItem : ReadWriteCache.ILockable
  {
    private readonly long freshTimestamp;
    private readonly object value;
    private readonly object version;

    public CachedItem(object value, long currentTimestamp, object version)
    {
      this.value = value;
      this.freshTimestamp = currentTimestamp;
      this.version = version;
    }

    public long FreshTimestamp => this.freshTimestamp;

    public object Value => this.value;

    public CacheLock Lock(long timeout, int id) => new CacheLock(timeout, id, this.version);

    public bool IsLock => false;

    public bool IsGettable(long txTimestamp) => this.freshTimestamp < txTimestamp;

    public bool IsPuttable(long txTimestamp, object newVersion, IComparer comparator)
    {
      return this.version != null && comparator.Compare(this.version, newVersion) < 0;
    }

    public override string ToString()
    {
      return "Item{version=" + this.version + ",freshTimestamp=" + (object) this.freshTimestamp + "}";
    }
  }
}

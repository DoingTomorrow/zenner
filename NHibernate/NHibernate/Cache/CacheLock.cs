// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.CacheLock
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache.Access;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Cache
{
  [Serializable]
  public class CacheLock : ReadWriteCache.ILockable, ISoftLock
  {
    private long unlockTimestamp = -1;
    private int multiplicity = 1;
    private bool concurrentLock;
    private long timeout;
    private readonly int id;
    private readonly object version;

    public CacheLock(long timeout, int id, object version)
    {
      this.timeout = timeout;
      this.id = id;
      this.version = version;
    }

    public CacheLock Lock(long timeout, int id)
    {
      this.concurrentLock = true;
      ++this.multiplicity;
      this.timeout = timeout;
      return this;
    }

    public void Unlock(long currentTimestamp)
    {
      if (--this.multiplicity != 0)
        return;
      this.unlockTimestamp = currentTimestamp;
    }

    public bool IsPuttable(long txTimestamp, object newVersion, IComparer comparator)
    {
      if (this.timeout < txTimestamp)
        return true;
      if (this.multiplicity > 0)
        return false;
      return this.version != null ? comparator.Compare(this.version, newVersion) < 0 : this.unlockTimestamp < txTimestamp;
    }

    public bool WasLockedConcurrently => this.concurrentLock;

    public bool IsLock => true;

    public bool IsGettable(long txTimestamp) => false;

    public int Id => this.id;

    public override string ToString()
    {
      return "CacheLock{id=" + (object) this.id + ",version=" + this.version + ",multiplicity=" + (object) this.multiplicity + ",unlockTimestamp=" + (object) this.unlockTimestamp + "}";
    }
  }
}

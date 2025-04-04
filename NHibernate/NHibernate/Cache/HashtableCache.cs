// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.HashtableCache
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections;

#nullable disable
namespace NHibernate.Cache
{
  public class HashtableCache : ICache
  {
    private IDictionary hashtable = (IDictionary) new Hashtable();
    private readonly string regionName;

    public HashtableCache(string regionName) => this.regionName = regionName;

    public object Get(object key) => this.hashtable[key];

    public void Put(object key, object value) => this.hashtable[key] = value;

    public void Remove(object key) => this.hashtable.Remove(key);

    public void Clear() => this.hashtable.Clear();

    public void Destroy()
    {
    }

    public void Lock(object key)
    {
    }

    public void Unlock(object key)
    {
    }

    public long NextTimestamp() => Timestamper.Next();

    public int Timeout => 245760000;

    public string RegionName => this.regionName;
  }
}

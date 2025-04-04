// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.FakeCache
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Cache
{
  public class FakeCache : ICache
  {
    public FakeCache(string regionName) => this.RegionName = regionName;

    public object Get(object key) => (object) null;

    public void Put(object key, object value)
    {
    }

    public void Remove(object key)
    {
    }

    public void Clear()
    {
    }

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

    public int Timeout { get; private set; }

    public string RegionName { get; private set; }
  }
}

// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.HashtableCacheProvider
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cache
{
  public class HashtableCacheProvider : ICacheProvider
  {
    public ICache BuildCache(string regionName, IDictionary<string, string> properties)
    {
      return (ICache) new HashtableCache(regionName);
    }

    public long NextTimestamp() => Timestamper.Next();

    public void Start(IDictionary<string, string> properties)
    {
    }

    public void Stop()
    {
    }
  }
}

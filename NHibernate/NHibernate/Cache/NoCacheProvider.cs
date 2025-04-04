// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.NoCacheProvider
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cache
{
  public class NoCacheProvider : ICacheProvider
  {
    public const string WarnMessage = "Second-level cache is enabled in a class, but no cache provider was selected. Fake cache used.";
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (NoCacheProvider));

    public ICache BuildCache(string regionName, IDictionary<string, string> properties)
    {
      NoCacheProvider.log.Warn((object) "Second-level cache is enabled in a class, but no cache provider was selected. Fake cache used.");
      return (ICache) new FakeCache(regionName);
    }

    public long NextTimestamp() => DateTime.Now.Ticks / 1000000L;

    public void Start(IDictionary<string, string> properties)
    {
    }

    public void Stop()
    {
    }
  }
}

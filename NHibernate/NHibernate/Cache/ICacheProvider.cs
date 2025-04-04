// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.ICacheProvider
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cache
{
  public interface ICacheProvider
  {
    ICache BuildCache(string regionName, IDictionary<string, string> properties);

    long NextTimestamp();

    void Start(IDictionary<string, string> properties);

    void Stop();
  }
}

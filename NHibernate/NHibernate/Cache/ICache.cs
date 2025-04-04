// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.ICache
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Cache
{
  public interface ICache
  {
    object Get(object key);

    void Put(object key, object value);

    void Remove(object key);

    void Clear();

    void Destroy();

    void Lock(object key);

    void Unlock(object key);

    long NextTimestamp();

    int Timeout { get; }

    string RegionName { get; }
  }
}

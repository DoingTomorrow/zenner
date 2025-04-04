// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.Entry.StructuredCacheEntry
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Entity;
using System.Collections;

#nullable disable
namespace NHibernate.Cache.Entry
{
  public class StructuredCacheEntry : ICacheEntryStructure
  {
    private readonly IEntityPersister persister;

    public StructuredCacheEntry(IEntityPersister persister) => this.persister = persister;

    public object Destructure(object item, ISessionFactoryImplementor factory)
    {
      IDictionary dictionary = (IDictionary) item;
      bool unfetched = (bool) dictionary[(object) "_lazyPropertiesUnfetched"];
      string str = (string) dictionary[(object) "_subclass"];
      object version = dictionary[(object) "_version"];
      string[] propertyNames = factory.GetEntityPersister(str).PropertyNames;
      object[] state = new object[propertyNames.Length];
      for (int index = 0; index < propertyNames.Length; ++index)
        state[index] = dictionary[(object) propertyNames[index]];
      return (object) new CacheEntry(state, str, unfetched, version);
    }

    public object Structure(object item)
    {
      CacheEntry cacheEntry = (CacheEntry) item;
      string[] propertyNames = this.persister.PropertyNames;
      IDictionary dictionary = (IDictionary) new Hashtable(propertyNames.Length + 2);
      dictionary[(object) "_subclass"] = (object) cacheEntry.Subclass;
      dictionary[(object) "_version"] = cacheEntry.Version;
      dictionary[(object) "_lazyPropertiesUnfetched"] = (object) cacheEntry.AreLazyPropertiesUnfetched;
      for (int index = 0; index < propertyNames.Length; ++index)
        dictionary[(object) propertyNames[index]] = cacheEntry.DisassembledState[index];
      return (object) dictionary;
    }
  }
}

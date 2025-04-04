// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.Entry.StructuredCollectionCacheEntry
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System.Collections;

#nullable disable
namespace NHibernate.Cache.Entry
{
  public class StructuredCollectionCacheEntry : ICacheEntryStructure
  {
    public virtual object Structure(object item)
    {
      return (object) new ArrayList((ICollection) ((CollectionCacheEntry) item).State);
    }

    public virtual object Destructure(object item, ISessionFactoryImplementor factory)
    {
      return (object) new CollectionCacheEntry((object) new ArrayList((ICollection) item).ToArray());
    }
  }
}

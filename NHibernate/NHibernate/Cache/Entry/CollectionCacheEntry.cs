// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.Entry.CollectionCacheEntry
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Persister.Collection;
using NHibernate.Util;
using System;

#nullable disable
namespace NHibernate.Cache.Entry
{
  [Serializable]
  public class CollectionCacheEntry
  {
    private readonly object state;

    public CollectionCacheEntry(IPersistentCollection collection, ICollectionPersister persister)
    {
      this.state = collection.Disassemble(persister);
    }

    internal CollectionCacheEntry(object state) => this.state = state;

    public virtual object[] State => (object[]) this.state;

    public virtual void Assemble(
      IPersistentCollection collection,
      ICollectionPersister persister,
      object owner)
    {
      collection.InitializeFromCache(persister, this.state, owner);
      collection.AfterInitialize(persister);
    }

    public override string ToString()
    {
      return nameof (CollectionCacheEntry) + ArrayHelper.ToString(this.State);
    }
  }
}

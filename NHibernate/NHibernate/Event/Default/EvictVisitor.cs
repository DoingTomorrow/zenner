// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.EvictVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Event.Default
{
  public class EvictVisitor(IEventSource session) : AbstractVisitor(session)
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (EvictVisitor));

    internal override object ProcessCollection(object collection, CollectionType type)
    {
      if (collection != null)
        this.EvictCollection(collection, type);
      return (object) null;
    }

    public virtual void EvictCollection(object value, CollectionType type)
    {
      IPersistentCollection persistentCollection;
      if (type.IsArrayType)
      {
        persistentCollection = this.Session.PersistenceContext.RemoveCollectionHolder(value);
      }
      else
      {
        if (!(value is IPersistentCollection))
          return;
        persistentCollection = (IPersistentCollection) value;
      }
      IPersistentCollection collection = persistentCollection;
      if (!collection.UnsetSession((ISessionImplementor) this.Session))
        return;
      this.EvictCollection(collection);
    }

    private void EvictCollection(IPersistentCollection collection)
    {
      CollectionEntry collectionEntry = (CollectionEntry) this.Session.PersistenceContext.CollectionEntries[(object) collection];
      this.Session.PersistenceContext.CollectionEntries.Remove((object) collection);
      if (EvictVisitor.log.IsDebugEnabled)
        EvictVisitor.log.Debug((object) ("evicting collection: " + MessageHelper.InfoString(collectionEntry.LoadedPersister, collectionEntry.LoadedKey, this.Session.Factory)));
      if (collectionEntry.LoadedPersister == null || collectionEntry.LoadedKey == null)
        return;
      this.Session.PersistenceContext.CollectionsByKey.Remove(new CollectionKey(collectionEntry.LoadedPersister, collectionEntry.LoadedKey, this.Session.EntityMode));
    }
  }
}

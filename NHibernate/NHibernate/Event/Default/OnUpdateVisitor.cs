// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.OnUpdateVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Event.Default
{
  public class OnUpdateVisitor(IEventSource session, object ownerIdentifier, object owner) : 
    ReattachVisitor(session, ownerIdentifier, owner)
  {
    internal override object ProcessCollection(object collection, CollectionType type)
    {
      if (collection == CollectionType.UnfetchedCollection)
        return (object) null;
      IEventSource session = this.Session;
      ICollectionPersister collectionPersister = session.Factory.GetCollectionPersister(type.Role);
      object collectionKeyFromOwner = this.ExtractCollectionKeyFromOwner(collectionPersister);
      if (collection is IPersistentCollection persistentCollection)
      {
        if (persistentCollection.SetCurrentSession((ISessionImplementor) session))
        {
          if (!ProxyVisitor.IsOwnerUnchanged(persistentCollection, collectionPersister, collectionKeyFromOwner))
            this.RemoveCollection(collectionPersister, collectionKeyFromOwner, session);
          this.ReattachCollection(persistentCollection, type);
        }
        else
          this.RemoveCollection(collectionPersister, collectionKeyFromOwner, session);
      }
      else
        this.RemoveCollection(collectionPersister, collectionKeyFromOwner, session);
      return (object) null;
    }
  }
}

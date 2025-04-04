// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.OnLockVisitor
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
  public class OnLockVisitor(IEventSource session, object ownerIdentifier, object owner) : 
    ReattachVisitor(session, ownerIdentifier, owner)
  {
    internal override object ProcessCollection(object collection, CollectionType type)
    {
      ISessionImplementor session = (ISessionImplementor) this.Session;
      ICollectionPersister collectionPersister = session.Factory.GetCollectionPersister(type.Role);
      if (collection != null)
      {
        if (!(collection is IPersistentCollection persistentCollection))
          throw new HibernateException("reassociated object has dirty collection reference (or an array)");
        if (!persistentCollection.SetCurrentSession(session))
          throw new HibernateException("reassociated object has dirty collection reference: " + persistentCollection.Role);
        if (!ProxyVisitor.IsOwnerUnchanged(persistentCollection, collectionPersister, this.ExtractCollectionKeyFromOwner(collectionPersister)))
          throw new HibernateException("reassociated object has dirty collection reference: " + persistentCollection.Role);
        if (persistentCollection.IsDirty)
          throw new HibernateException("reassociated object has dirty collection: " + persistentCollection.Role);
        this.ReattachCollection(persistentCollection, type);
      }
      return (object) null;
    }
  }
}

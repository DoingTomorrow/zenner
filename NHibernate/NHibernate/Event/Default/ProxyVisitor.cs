// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.ProxyVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Persister.Collection;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Event.Default
{
  public abstract class ProxyVisitor : AbstractVisitor
  {
    public ProxyVisitor(IEventSource session)
      : base(session)
    {
    }

    internal override object ProcessEntity(object value, EntityType entityType)
    {
      if (value != null)
        this.Session.PersistenceContext.ReassociateIfUninitializedProxy(value);
      return (object) null;
    }

    protected internal static bool IsOwnerUnchanged(
      IPersistentCollection snapshot,
      ICollectionPersister persister,
      object id)
    {
      return ProxyVisitor.IsCollectionSnapshotValid(snapshot) && persister.Role.Equals(snapshot.Role) && id.Equals(snapshot.Key);
    }

    private static bool IsCollectionSnapshotValid(IPersistentCollection snapshot)
    {
      return snapshot != null && snapshot.Role != null && snapshot.Key != null;
    }

    protected internal void ReattachCollection(
      IPersistentCollection collection,
      CollectionType type)
    {
      if (collection.WasInitialized)
      {
        this.Session.PersistenceContext.AddInitializedDetachedCollection(this.Session.Factory.GetCollectionPersister(type.Role), collection);
      }
      else
      {
        if (!ProxyVisitor.IsCollectionSnapshotValid(collection))
          throw new HibernateException("could not reassociate uninitialized transient collection");
        this.Session.PersistenceContext.AddUninitializedDetachedCollection(this.Session.Factory.GetCollectionPersister(collection.Role), collection);
      }
    }
  }
}

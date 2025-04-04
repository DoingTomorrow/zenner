// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.OnReplicateVisitor
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
  public class OnReplicateVisitor : ReattachVisitor
  {
    private readonly bool isUpdate;

    public OnReplicateVisitor(
      IEventSource session,
      object ownerIdentifier,
      object owner,
      bool isUpdate)
      : base(session, ownerIdentifier, owner)
    {
      this.isUpdate = isUpdate;
    }

    internal override object ProcessCollection(object collection, CollectionType type)
    {
      if (collection == CollectionType.UnfetchedCollection)
        return (object) null;
      IEventSource session = this.Session;
      ICollectionPersister collectionPersister = session.Factory.GetCollectionPersister(type.Role);
      if (this.isUpdate)
        this.RemoveCollection(collectionPersister, this.ExtractCollectionKeyFromOwner(collectionPersister), session);
      if (collection is IPersistentCollection collection1)
      {
        collection1.SetCurrentSession((ISessionImplementor) session);
        if (collection1.WasInitialized)
          session.PersistenceContext.AddNewCollection(collectionPersister, collection1);
        else
          this.ReattachCollection(collection1, type);
      }
      return (object) null;
    }
  }
}

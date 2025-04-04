// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.FlushVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Event.Default
{
  public class FlushVisitor : AbstractVisitor
  {
    private readonly object owner;

    public FlushVisitor(IEventSource session, object owner)
      : base(session)
    {
      this.owner = owner;
    }

    internal override object ProcessCollection(object collection, CollectionType type)
    {
      if (collection == CollectionType.UnfetchedCollection)
        return (object) null;
      if (collection != null)
        Collections.ProcessReachableCollection(!type.IsArrayType ? (IPersistentCollection) collection : this.Session.PersistenceContext.GetCollectionHolder(collection), type, this.owner, (ISessionImplementor) this.Session);
      return (object) null;
    }
  }
}

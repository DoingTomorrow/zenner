// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.DirtyCollectionSearchVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Event.Default
{
  public class DirtyCollectionSearchVisitor : AbstractVisitor
  {
    private readonly bool[] propertyVersionability;
    private bool dirty;

    public DirtyCollectionSearchVisitor(IEventSource session, bool[] propertyVersionability)
      : base(session)
    {
      this.propertyVersionability = propertyVersionability;
    }

    public bool WasDirtyCollectionFound => this.dirty;

    internal override object ProcessCollection(object collection, CollectionType type)
    {
      if (collection != null)
      {
        ISessionImplementor session = (ISessionImplementor) this.Session;
        if ((!type.IsArrayType ? (IPersistentCollection) collection : session.PersistenceContext.GetCollectionHolder(collection)).IsDirty)
        {
          this.dirty = true;
          return (object) null;
        }
      }
      return (object) null;
    }

    internal override bool IncludeEntityProperty(object[] values, int i)
    {
      return this.propertyVersionability[i] && base.IncludeEntityProperty(values, i);
    }
  }
}

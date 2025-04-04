// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.ReattachVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Action;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Collection;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Event.Default
{
  public abstract class ReattachVisitor : ProxyVisitor
  {
    private readonly object ownerIdentifier;
    private readonly object owner;
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (AbstractFlushingEventListener));

    protected ReattachVisitor(IEventSource session, object ownerIdentifier, object owner)
      : base(session)
    {
      this.ownerIdentifier = ownerIdentifier;
      this.owner = owner;
    }

    public object OwnerIdentifier => this.ownerIdentifier;

    public object Owner => this.owner;

    internal override object ProcessComponent(
      object component,
      IAbstractComponentType componentType)
    {
      IType[] subtypes = componentType.Subtypes;
      if (component == null)
        this.ProcessValues(new object[subtypes.Length], subtypes);
      else
        base.ProcessComponent(component, componentType);
      return (object) null;
    }

    internal void RemoveCollection(
      ICollectionPersister role,
      object collectionKey,
      IEventSource source)
    {
      if (ReattachVisitor.log.IsDebugEnabled)
        ReattachVisitor.log.Debug((object) ("collection dereferenced while transient " + MessageHelper.InfoString(role, this.ownerIdentifier, source.Factory)));
      source.ActionQueue.AddAction(new CollectionRemoveAction(this.owner, role, collectionKey, false, (ISessionImplementor) source));
    }

    internal object ExtractCollectionKeyFromOwner(ICollectionPersister role)
    {
      return role.CollectionType.UseLHSPrimaryKey ? this.ownerIdentifier : role.OwnerEntityPersister.GetPropertyValue(this.owner, role.CollectionType.LHSPropertyName, this.Session.EntityMode);
    }
  }
}

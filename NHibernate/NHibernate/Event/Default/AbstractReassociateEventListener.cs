// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.AbstractReassociateEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Event.Default
{
  [Serializable]
  public class AbstractReassociateEventListener
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (AbstractReassociateEventListener));

    protected EntityEntry Reassociate(
      AbstractEvent @event,
      object entity,
      object id,
      IEntityPersister persister)
    {
      if (AbstractReassociateEventListener.log.IsDebugEnabled)
        AbstractReassociateEventListener.log.Debug((object) ("Reassociating transient instance: " + MessageHelper.InfoString(persister, id, @event.Session.Factory)));
      IEventSource session = @event.Session;
      EntityKey entityKey = new EntityKey(id, persister, session.EntityMode);
      session.PersistenceContext.CheckUniqueness(entityKey, entity);
      object[] propertyValues = persister.GetPropertyValues(entity, session.EntityMode);
      TypeHelper.DeepCopy(propertyValues, persister.PropertyTypes, persister.PropertyUpdateability, propertyValues, (ISessionImplementor) session);
      object version = Versioning.GetVersion(propertyValues, persister);
      EntityEntry entityEntry = session.PersistenceContext.AddEntity(entity, persister.IsMutable ? Status.Loaded : Status.ReadOnly, propertyValues, entityKey, version, LockMode.None, true, persister, false, true);
      new OnLockVisitor(session, id, entity).Process(entity, persister);
      persister.AfterReassociate(entity, (ISessionImplementor) session);
      return entityEntry;
    }
  }
}

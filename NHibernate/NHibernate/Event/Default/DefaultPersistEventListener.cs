// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.DefaultPersistEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Util;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Event.Default
{
  [Serializable]
  public class DefaultPersistEventListener : AbstractSaveEventListener, IPersistEventListener
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (DefaultPersistEventListener));

    protected override CascadingAction CascadeAction => CascadingAction.Persist;

    protected override bool? AssumedUnsaved => new bool?(true);

    public virtual void OnPersist(PersistEvent @event)
    {
      this.OnPersist(@event, IdentityMap.Instantiate(10));
    }

    public virtual void OnPersist(PersistEvent @event, IDictionary createdAlready)
    {
      ISessionImplementor session = (ISessionImplementor) @event.Session;
      object entity1 = @event.Entity;
      object entity2;
      if (entity1.IsProxy())
      {
        ILazyInitializer hibernateLazyInitializer = ((INHibernateProxy) entity1).HibernateLazyInitializer;
        if (hibernateLazyInitializer.IsUninitialized)
        {
          if (hibernateLazyInitializer.Session == session)
            return;
          throw new PersistentObjectException("uninitialized proxy passed to persist()");
        }
        entity2 = hibernateLazyInitializer.GetImplementation();
      }
      else
        entity2 = entity1;
      switch (this.GetEntityState(entity2, @event.EntityName, session.PersistenceContext.GetEntry(entity2), session))
      {
        case EntityState.Persistent:
          this.EntityIsPersistent(@event, createdAlready);
          break;
        case EntityState.Transient:
          this.EntityIsTransient(@event, createdAlready);
          break;
        case EntityState.Detached:
          throw new PersistentObjectException("detached entity passed to persist: " + this.GetLoggableName(@event.EntityName, entity2));
        default:
          throw new ObjectDeletedException("deleted instance passed to merge", (object) null, this.GetLoggableName(@event.EntityName, entity2));
      }
    }

    protected virtual void EntityIsPersistent(PersistEvent @event, IDictionary createCache)
    {
      DefaultPersistEventListener.log.Debug((object) "ignoring persistent instance");
      IEventSource session = @event.Session;
      object obj1 = session.PersistenceContext.Unproxy(@event.Entity);
      if (@event.EntityName == null)
        @event.EntityName = session.BestGuessEntityName(obj1);
      IEntityPersister entityPersister = session.GetEntityPersister(@event.EntityName, obj1);
      object obj2 = createCache[obj1];
      createCache[obj1] = obj1;
      if (obj2 != null)
        return;
      this.CascadeBeforeSave(session, entityPersister, obj1, (object) createCache);
      this.CascadeAfterSave(session, entityPersister, obj1, (object) createCache);
    }

    protected virtual void EntityIsTransient(PersistEvent @event, IDictionary createCache)
    {
      DefaultPersistEventListener.log.Debug((object) "saving transient instance");
      IEventSource session = @event.Session;
      object obj1 = session.PersistenceContext.Unproxy(@event.Entity);
      object obj2 = createCache[obj1];
      createCache[obj1] = obj1;
      if (obj2 != null)
        return;
      this.SaveWithGeneratedId(obj1, @event.EntityName, (object) createCache, session, false);
    }
  }
}

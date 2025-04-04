// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.DefaultEvictEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using System;

#nullable disable
namespace NHibernate.Event.Default
{
  [Serializable]
  public class DefaultEvictEventListener : IEvictEventListener
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (DefaultEvictEventListener));

    public virtual void OnEvict(EvictEvent @event)
    {
      IEventSource session = @event.Session;
      object entity1 = @event.Entity;
      IPersistenceContext persistenceContext = session.PersistenceContext;
      if (entity1.IsProxy())
      {
        ILazyInitializer hibernateLazyInitializer = ((INHibernateProxy) entity1).HibernateLazyInitializer;
        object identifier = hibernateLazyInitializer.Identifier;
        IEntityPersister entityPersister = session.Factory.GetEntityPersister(hibernateLazyInitializer.EntityName);
        if (identifier == null)
          throw new ArgumentException("null identifier");
        EntityKey key = new EntityKey(identifier, entityPersister, session.EntityMode);
        persistenceContext.RemoveProxy(key);
        if (!hibernateLazyInitializer.IsUninitialized)
        {
          object entity2 = persistenceContext.RemoveEntity(key);
          if (entity2 != null)
          {
            EntityEntry entityEntry = @event.Session.PersistenceContext.RemoveEntry(entity2);
            this.DoEvict(entity2, key, entityEntry.Persister, @event.Session);
          }
        }
        hibernateLazyInitializer.UnsetSession();
      }
      else
      {
        EntityEntry entityEntry = persistenceContext.RemoveEntry(entity1);
        if (entityEntry == null)
          return;
        persistenceContext.RemoveEntity(entityEntry.EntityKey);
        this.DoEvict(entity1, entityEntry.EntityKey, entityEntry.Persister, session);
      }
    }

    protected virtual void DoEvict(
      object obj,
      EntityKey key,
      IEntityPersister persister,
      IEventSource session)
    {
      if (DefaultEvictEventListener.log.IsDebugEnabled)
        DefaultEvictEventListener.log.Debug((object) ("evicting " + MessageHelper.InfoString(persister)));
      if (persister.HasCollections)
        new EvictVisitor(session).Process(obj, persister);
      new Cascade(CascadingAction.Evict, CascadePoint.AfterUpdate, session).CascadeOn(persister, obj);
    }
  }
}

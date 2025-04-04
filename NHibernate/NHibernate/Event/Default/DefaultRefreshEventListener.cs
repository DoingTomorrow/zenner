// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.DefaultRefreshEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Event.Default
{
  [Serializable]
  public class DefaultRefreshEventListener : IRefreshEventListener
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (DefaultRefreshEventListener));

    public virtual void OnRefresh(RefreshEvent @event)
    {
      this.OnRefresh(@event, IdentityMap.Instantiate(10));
    }

    public virtual void OnRefresh(RefreshEvent @event, IDictionary refreshedAlready)
    {
      IEventSource session = @event.Session;
      bool flag = !session.Contains(@event.Entity);
      if (session.PersistenceContext.ReassociateIfUninitializedProxy(@event.Entity))
      {
        if (!flag)
          return;
        session.SetReadOnly(@event.Entity, session.DefaultReadOnly);
      }
      else
      {
        object obj1 = session.PersistenceContext.UnproxyAndReassociate(@event.Entity);
        if (refreshedAlready.Contains(obj1))
        {
          DefaultRefreshEventListener.log.Debug((object) "already refreshed");
        }
        else
        {
          EntityEntry entry = session.PersistenceContext.GetEntry(obj1);
          IEntityPersister persister;
          object id;
          if (entry == null)
          {
            persister = session.GetEntityPersister((string) null, obj1);
            id = persister.GetIdentifier(obj1, session.EntityMode);
            if (DefaultRefreshEventListener.log.IsDebugEnabled)
              DefaultRefreshEventListener.log.Debug((object) ("refreshing transient " + MessageHelper.InfoString(persister, id, session.Factory)));
            EntityKey entity = new EntityKey(id, persister, session.EntityMode);
            if (session.PersistenceContext.GetEntry((object) entity) != null)
              throw new PersistentObjectException("attempted to refresh transient instance when persistent instance was already associated with the Session: " + MessageHelper.InfoString(persister, id, session.Factory));
          }
          else
          {
            if (DefaultRefreshEventListener.log.IsDebugEnabled)
              DefaultRefreshEventListener.log.Debug((object) ("refreshing " + MessageHelper.InfoString(entry.Persister, entry.Id, session.Factory)));
            persister = entry.ExistsInDatabase ? entry.Persister : throw new HibernateException("this instance does not yet exist as a row in the database");
            id = entry.Id;
          }
          refreshedAlready[obj1] = obj1;
          new Cascade(CascadingAction.Refresh, CascadePoint.AfterUpdate, session).CascadeOn(persister, obj1, (object) refreshedAlready);
          if (entry != null)
          {
            EntityKey key = new EntityKey(id, persister, session.EntityMode);
            session.PersistenceContext.RemoveEntity(key);
            if (persister.HasCollections)
              new EvictVisitor(session).Process(obj1, persister);
          }
          if (persister.HasCache)
          {
            CacheKey key = new CacheKey(id, persister.IdentifierType, persister.RootEntityName, session.EntityMode, session.Factory);
            persister.Cache.Remove(key);
          }
          this.EvictCachedCollections(persister, id, session.Factory);
          new WrapVisitor(session).Process(obj1, persister);
          string fetchProfile = session.FetchProfile;
          session.FetchProfile = "refresh";
          object obj2 = persister.Load(id, obj1, @event.LockMode, (ISessionImplementor) session);
          if (obj2 != null)
          {
            if (!persister.IsMutable)
              session.SetReadOnly(obj2, true);
            else
              session.SetReadOnly(obj2, entry == null ? session.DefaultReadOnly : entry.IsReadOnly);
          }
          session.FetchProfile = fetchProfile;
          if (ForeignKeys.IsTransient(persister.EntityName, obj1, new bool?(obj2 == null), (ISessionImplementor) @event.Session))
            return;
          UnresolvableObjectException.ThrowIfNull(obj2, id, persister.EntityName);
        }
      }
    }

    private void EvictCachedCollections(
      IEntityPersister persister,
      object id,
      ISessionFactoryImplementor factory)
    {
      this.EvictCachedCollections(persister.PropertyTypes, id, factory);
    }

    private void EvictCachedCollections(
      IType[] types,
      object id,
      ISessionFactoryImplementor factory)
    {
      for (int index = 0; index < types.Length; ++index)
      {
        if (types[index].IsCollectionType)
          factory.EvictCollection(((CollectionType) types[index]).Role, id);
        else if (types[index].IsComponentType)
          this.EvictCachedCollections(((IAbstractComponentType) types[index]).Subtypes, id, factory);
      }
    }
  }
}

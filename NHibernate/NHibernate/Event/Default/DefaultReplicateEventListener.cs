// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.DefaultReplicateEventListener
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
  public class DefaultReplicateEventListener : AbstractSaveEventListener, IReplicateEventListener
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (DefaultReplicateEventListener));

    public virtual void OnReplicate(ReplicateEvent @event)
    {
      IEventSource session = @event.Session;
      if (session.PersistenceContext.ReassociateIfUninitializedProxy(@event.Entity))
      {
        DefaultReplicateEventListener.log.Debug((object) "uninitialized proxy passed to replicate()");
      }
      else
      {
        object entity = session.PersistenceContext.UnproxyAndReassociate(@event.Entity);
        if (session.PersistenceContext.IsEntryFor(entity))
        {
          DefaultReplicateEventListener.log.Debug((object) "ignoring persistent instance passed to replicate()");
        }
        else
        {
          IEntityPersister entityPersister = session.GetEntityPersister(@event.EntityName, entity);
          object identifier = entityPersister.GetIdentifier(entity, session.EntityMode);
          if (identifier == null)
            throw new TransientObjectException("instance with null id passed to replicate()");
          ReplicationMode replicationMode = @event.ReplicationMode;
          object currentVersion = replicationMode != ReplicationMode.Exception ? entityPersister.GetCurrentVersion(identifier, (ISessionImplementor) session) : (object) null;
          if (currentVersion != null)
          {
            if (DefaultReplicateEventListener.log.IsDebugEnabled)
              DefaultReplicateEventListener.log.Debug((object) ("found existing row for " + MessageHelper.InfoString(entityPersister, identifier, session.Factory)));
            object obj = entityPersister.IsVersioned ? currentVersion : (object) null;
            if (replicationMode.ShouldOverwriteCurrentVersion(entity, obj, entityPersister.GetVersion(entity, session.EntityMode), entityPersister.VersionType))
              this.PerformReplication(entity, identifier, obj, entityPersister, replicationMode, session);
            else
              DefaultReplicateEventListener.log.Debug((object) "no need to replicate");
          }
          else
          {
            if (DefaultReplicateEventListener.log.IsDebugEnabled)
              DefaultReplicateEventListener.log.Debug((object) ("no existing row, replicating new instance " + MessageHelper.InfoString(entityPersister, identifier, session.Factory)));
            bool assignedByInsert = entityPersister.IsIdentifierAssignedByInsert;
            EntityKey key = assignedByInsert ? (EntityKey) null : new EntityKey(identifier, entityPersister, session.EntityMode);
            this.PerformSaveOrReplicate(entity, key, entityPersister, assignedByInsert, (object) replicationMode, session, true);
          }
        }
      }
    }

    private void PerformReplication(
      object entity,
      object id,
      object version,
      IEntityPersister persister,
      ReplicationMode replicationMode,
      IEventSource source)
    {
      if (DefaultReplicateEventListener.log.IsDebugEnabled)
        DefaultReplicateEventListener.log.Debug((object) ("replicating changes to " + MessageHelper.InfoString(persister, id, source.Factory)));
      new OnReplicateVisitor(source, id, entity, true).Process(entity, persister);
      source.PersistenceContext.AddEntity(entity, persister.IsMutable ? Status.Loaded : Status.ReadOnly, (object[]) null, new EntityKey(id, persister, source.EntityMode), version, LockMode.None, true, persister, true, false);
      this.CascadeAfterReplicate(entity, persister, replicationMode, source);
    }

    private void CascadeAfterReplicate(
      object entity,
      IEntityPersister persister,
      ReplicationMode replicationMode,
      IEventSource source)
    {
      source.PersistenceContext.IncrementCascadeLevel();
      try
      {
        new Cascade(CascadingAction.Replicate, CascadePoint.AfterUpdate, source).CascadeOn(persister, entity, (object) replicationMode);
      }
      finally
      {
        source.PersistenceContext.DecrementCascadeLevel();
      }
    }

    protected override bool VersionIncrementDisabled => true;

    protected override CascadingAction CascadeAction => CascadingAction.Replicate;

    protected override bool SubstituteValuesIfNecessary(
      object entity,
      object id,
      object[] values,
      IEntityPersister persister,
      ISessionImplementor source)
    {
      return false;
    }

    protected override bool VisitCollectionsBeforeSave(
      object entity,
      object id,
      object[] values,
      IType[] types,
      IEventSource source)
    {
      new OnReplicateVisitor(source, id, entity, false).ProcessEntityPropertyValues(values, types);
      return base.VisitCollectionsBeforeSave(entity, id, values, types, source);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.DefaultUpdateEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Entity;
using System;

#nullable disable
namespace NHibernate.Event.Default
{
  [Serializable]
  public class DefaultUpdateEventListener : DefaultSaveOrUpdateEventListener
  {
    protected override object PerformSaveOrUpdate(SaveOrUpdateEvent @event)
    {
      EntityEntry entry = @event.Session.PersistenceContext.GetEntry(@event.Entity);
      if (entry != null)
      {
        if (entry.Status == Status.Deleted)
          throw new ObjectDeletedException("deleted instance passed to update()", (object) null, @event.EntityName);
        return this.EntityIsPersistent(@event);
      }
      this.EntityIsDetached(@event);
      return (object) null;
    }

    protected override object GetUpdateId(
      object entity,
      IEntityPersister persister,
      object requestedId,
      EntityMode entityMode)
    {
      if (requestedId == null)
        return base.GetUpdateId(entity, persister, requestedId, entityMode);
      persister.SetIdentifier(entity, requestedId, entityMode);
      return requestedId;
    }
  }
}

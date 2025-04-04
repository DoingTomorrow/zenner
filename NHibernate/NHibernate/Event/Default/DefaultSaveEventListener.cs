// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.DefaultSaveEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;

#nullable disable
namespace NHibernate.Event.Default
{
  [Serializable]
  public class DefaultSaveEventListener : DefaultSaveOrUpdateEventListener
  {
    protected override object PerformSaveOrUpdate(SaveOrUpdateEvent @event)
    {
      EntityEntry entry = @event.Session.PersistenceContext.GetEntry(@event.Entity);
      return entry != null && entry.Status != Status.Deleted ? this.EntityIsPersistent(@event) : this.EntityIsTransient(@event);
    }

    protected override object SaveWithGeneratedOrRequestedId(SaveOrUpdateEvent @event)
    {
      return @event.RequestedId == null ? base.SaveWithGeneratedOrRequestedId(@event) : this.SaveWithRequestedId(@event.Entity, @event.RequestedId, @event.EntityName, (object) null, @event.Session);
    }

    protected override bool ReassociateIfUninitializedProxy(object obj, ISessionImplementor source)
    {
      if (!NHibernateUtil.IsInitialized(obj))
        throw new PersistentObjectException("Uninitialized proxy passed to save(). Object: " + obj.ToString());
      return false;
    }
  }
}

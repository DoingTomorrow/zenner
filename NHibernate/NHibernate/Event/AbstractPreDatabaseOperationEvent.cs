// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.AbstractPreDatabaseOperationEvent
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;
using System;

#nullable disable
namespace NHibernate.Event
{
  [Serializable]
  public abstract class AbstractPreDatabaseOperationEvent : 
    AbstractEvent,
    IPreDatabaseOperationEventArgs,
    IDatabaseEventArgs
  {
    protected AbstractPreDatabaseOperationEvent(
      IEventSource source,
      object entity,
      object id,
      IEntityPersister persister)
      : base(source)
    {
      this.Entity = entity;
      this.Id = id;
      this.Persister = persister;
    }

    public object Entity { get; private set; }

    public object Id { get; private set; }

    public IEntityPersister Persister { get; private set; }
  }
}

// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.PostDeleteEvent
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;
using System;

#nullable disable
namespace NHibernate.Event
{
  [Serializable]
  public class PostDeleteEvent : AbstractPostDatabaseOperationEvent
  {
    public PostDeleteEvent(
      object entity,
      object id,
      object[] deletedState,
      IEntityPersister persister,
      IEventSource source)
      : base(source, entity, id, persister)
    {
      this.DeletedState = deletedState;
    }

    public object[] DeletedState { get; private set; }
  }
}

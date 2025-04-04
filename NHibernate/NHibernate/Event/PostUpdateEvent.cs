// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.PostUpdateEvent
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;
using System;

#nullable disable
namespace NHibernate.Event
{
  [Serializable]
  public class PostUpdateEvent : AbstractPostDatabaseOperationEvent
  {
    public PostUpdateEvent(
      object entity,
      object id,
      object[] state,
      object[] oldState,
      IEntityPersister persister,
      IEventSource source)
      : base(source, entity, id, persister)
    {
      this.State = state;
      this.OldState = oldState;
    }

    public object[] State { get; private set; }

    public object[] OldState { get; private set; }
  }
}

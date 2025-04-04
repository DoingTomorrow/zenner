// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.PostInsertEvent
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;
using System;

#nullable disable
namespace NHibernate.Event
{
  [Serializable]
  public class PostInsertEvent : AbstractPostDatabaseOperationEvent
  {
    public PostInsertEvent(
      object entity,
      object id,
      object[] state,
      IEntityPersister persister,
      IEventSource source)
      : base(source, entity, id, persister)
    {
      this.State = state;
    }

    public object[] State { get; private set; }
  }
}

// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.PreInsertEvent
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;

#nullable disable
namespace NHibernate.Event
{
  public class PreInsertEvent : AbstractPreDatabaseOperationEvent
  {
    public PreInsertEvent(
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

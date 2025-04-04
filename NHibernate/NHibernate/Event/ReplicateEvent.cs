// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.ReplicateEvent
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Event
{
  [Serializable]
  public class ReplicateEvent : AbstractEvent
  {
    private string entityName;
    private object entity;
    private ReplicationMode replicationMode;

    public ReplicateEvent(object entity, ReplicationMode replicationMode, IEventSource source)
      : this((string) null, entity, replicationMode, source)
    {
    }

    public ReplicateEvent(
      string entityName,
      object entity,
      ReplicationMode replicationMode,
      IEventSource source)
      : base(source)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof (entity), "attempt to create replication strategy with null entity");
      if (replicationMode == null)
        throw new ArgumentNullException(nameof (replicationMode), "attempt to create replication strategy with null replication mode");
      this.entityName = entityName;
      this.entity = entity;
      this.replicationMode = replicationMode;
    }

    public string EntityName
    {
      get => this.entityName;
      set => this.entityName = value;
    }

    public object Entity
    {
      get => this.entity;
      set => this.entity = value;
    }

    public ReplicationMode ReplicationMode
    {
      get => this.replicationMode;
      set => this.replicationMode = value;
    }
  }
}

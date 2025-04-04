// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.DeleteEvent
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Event
{
  [Serializable]
  public class DeleteEvent : AbstractEvent
  {
    private readonly string entityName;
    private readonly object entity;
    private readonly bool cascadeDeleteEnabled;

    public DeleteEvent(object entity, IEventSource source)
      : base(source)
    {
      this.entity = entity != null ? entity : throw new ArgumentNullException(nameof (entity), "Attempt to create delete event with null entity");
    }

    public DeleteEvent(string entityName, object entity, IEventSource source)
      : this(entity, source)
    {
      this.entityName = entityName;
    }

    public DeleteEvent(
      string entityName,
      object entity,
      bool isCascadeDeleteEnabled,
      IEventSource source)
      : this(entityName, entity, source)
    {
      this.cascadeDeleteEnabled = isCascadeDeleteEnabled;
    }

    public string EntityName => this.entityName;

    public object Entity => this.entity;

    public bool CascadeDeleteEnabled => this.cascadeDeleteEnabled;
  }
}

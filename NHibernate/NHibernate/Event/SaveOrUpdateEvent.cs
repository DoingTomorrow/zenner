// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.SaveOrUpdateEvent
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;

#nullable disable
namespace NHibernate.Event
{
  [Serializable]
  public class SaveOrUpdateEvent : AbstractEvent
  {
    private object entity;
    private string entityName;
    private object requestedId;
    private object resultEntity;
    private EntityEntry entry;
    private object resultId;

    public SaveOrUpdateEvent(object entity, IEventSource source)
      : base(source)
    {
      this.entity = entity != null ? entity : throw new ArgumentNullException(nameof (entity), "attempt to create saveOrUpdate event with null entity");
    }

    public SaveOrUpdateEvent(string entityName, object original, IEventSource source)
      : this(original, source)
    {
      this.entityName = entityName;
    }

    public SaveOrUpdateEvent(string entityName, object original, object id, IEventSource source)
      : this(entityName, original, source)
    {
      this.requestedId = id != null ? id : throw new ArgumentNullException(nameof (id), "attempt to create saveOrUpdate event with null identifier");
    }

    public object Entity
    {
      get => this.entity;
      set => this.entity = value;
    }

    public string EntityName
    {
      get => this.entityName;
      set => this.entityName = value;
    }

    public object RequestedId
    {
      get => this.requestedId;
      set => this.requestedId = value;
    }

    public object ResultEntity
    {
      get => this.resultEntity;
      set => this.resultEntity = value;
    }

    public EntityEntry Entry
    {
      get => this.entry;
      set => this.entry = value;
    }

    public object ResultId
    {
      get => this.resultId;
      set => this.resultId = value;
    }
  }
}

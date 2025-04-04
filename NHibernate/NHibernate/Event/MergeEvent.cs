// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.MergeEvent
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Event
{
  [Serializable]
  public class MergeEvent : AbstractEvent
  {
    private object original;
    private string entityName;
    private object requestedId;
    private object entity;
    private object result;

    public MergeEvent(object entity, IEventSource source)
      : base(source)
    {
      this.Original = entity != null ? entity : throw new ArgumentNullException(nameof (entity), "attempt to create merge event with null entity");
    }

    public MergeEvent(string entityName, object original, IEventSource source)
      : this(original, source)
    {
      this.EntityName = entityName;
    }

    public MergeEvent(string entityName, object original, object id, IEventSource source)
      : this(entityName, original, source)
    {
      this.RequestedId = id != null ? id : throw new ArgumentNullException(nameof (id), "attempt to create merge event with null identifier");
    }

    public object Original
    {
      get => this.original;
      set => this.original = value;
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

    public object Entity
    {
      get => this.entity;
      set => this.entity = value;
    }

    public object Result
    {
      get => this.result;
      set => this.result = value;
    }
  }
}

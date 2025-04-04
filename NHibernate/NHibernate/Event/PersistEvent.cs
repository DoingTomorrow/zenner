// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.PersistEvent
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Event
{
  public class PersistEvent : AbstractEvent
  {
    private object entity;
    private string entityName;

    public PersistEvent(object entity, IEventSource source)
      : base(source)
    {
      this.entity = entity != null ? entity : throw new ArgumentNullException(nameof (entity), "Attempt to create create event with null entity");
    }

    public PersistEvent(string entityName, object original, IEventSource source)
      : this(original, source)
    {
      this.entityName = entityName;
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
  }
}

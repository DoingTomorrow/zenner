// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.LockEvent
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Event
{
  [Serializable]
  public class LockEvent : AbstractEvent
  {
    private string entityName;
    private object entity;
    private LockMode lockMode;

    public LockEvent(object entity, LockMode lockMode, IEventSource source)
      : base(source)
    {
      this.Entity = entity;
      this.LockMode = lockMode;
    }

    public LockEvent(string entityName, object original, LockMode lockMode, IEventSource source)
      : this(original, lockMode, source)
    {
      this.EntityName = entityName;
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

    public LockMode LockMode
    {
      get => this.lockMode;
      set => this.lockMode = value;
    }
  }
}

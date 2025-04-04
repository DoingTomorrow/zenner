// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.RefreshEvent
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Event
{
  [Serializable]
  public class RefreshEvent : AbstractEvent
  {
    private readonly LockMode lockMode = LockMode.Read;
    private readonly object entity;

    public RefreshEvent(object entity, IEventSource source)
      : base(source)
    {
      this.entity = entity != null ? entity : throw new ArgumentNullException(nameof (entity), "Attempt to generate refresh event with null object");
    }

    public RefreshEvent(object entity, LockMode lockMode, IEventSource source)
      : this(entity, source)
    {
      this.lockMode = lockMode != null ? lockMode : throw new ArgumentNullException(nameof (lockMode), "Attempt to generate refresh event with null lock mode");
    }

    public object Entity => this.entity;

    public LockMode LockMode => this.lockMode;
  }
}

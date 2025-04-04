// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.LoadEvent
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Event
{
  [Serializable]
  public class LoadEvent : AbstractEvent
  {
    public static readonly LockMode DefaultLockMode = LockMode.None;
    private object entityId;
    private string entityClassName;
    private object instanceToLoad;
    private LockMode lockMode;
    private readonly bool isAssociationFetch;
    private object result;

    private LoadEvent(
      object entityId,
      string entityClassName,
      object instanceToLoad,
      LockMode lockMode,
      bool isAssociationFetch,
      IEventSource source)
      : base(source)
    {
      if (entityId == null)
        throw new ArgumentNullException(nameof (entityId), "id to load is required for loading");
      if (lockMode == LockMode.Write)
        throw new ArgumentOutOfRangeException(nameof (lockMode), "Invalid lock mode for loading");
      if (lockMode == null)
        lockMode = LoadEvent.DefaultLockMode;
      this.entityId = entityId;
      this.entityClassName = entityClassName;
      this.instanceToLoad = instanceToLoad;
      this.lockMode = lockMode;
      this.isAssociationFetch = isAssociationFetch;
    }

    public LoadEvent(object entityId, object instanceToLoad, IEventSource source)
      : this(entityId, (string) null, instanceToLoad, (LockMode) null, false, source)
    {
    }

    public LoadEvent(
      object entityId,
      string entityClassName,
      LockMode lockMode,
      IEventSource source)
      : this(entityId, entityClassName, (object) null, lockMode, false, source)
    {
    }

    public LoadEvent(
      object entityId,
      string entityClassName,
      bool isAssociationFetch,
      IEventSource source)
      : this(entityId, entityClassName, (object) null, (LockMode) null, isAssociationFetch, source)
    {
    }

    public bool IsAssociationFetch => this.isAssociationFetch;

    public object EntityId
    {
      get => this.entityId;
      set
      {
        this.entityId = value != null ? value : throw new InvalidOperationException("id to load is required for loading");
      }
    }

    public string EntityClassName
    {
      get => this.entityClassName;
      set => this.entityClassName = value;
    }

    public object InstanceToLoad
    {
      get => this.instanceToLoad;
      set => this.instanceToLoad = value;
    }

    public LockMode LockMode
    {
      get => this.lockMode;
      set
      {
        if (value == LockMode.Write)
          throw new InvalidOperationException("Invalid lock mode for loading");
        if (value == null)
          this.lockMode = LoadEvent.DefaultLockMode;
        this.lockMode = value;
      }
    }

    public object Result
    {
      get => this.result;
      set => this.result = value;
    }
  }
}

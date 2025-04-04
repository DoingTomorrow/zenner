// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.FlushEntityEvent
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;

#nullable disable
namespace NHibernate.Event
{
  [Serializable]
  public class FlushEntityEvent : AbstractEvent
  {
    private readonly object entity;
    private readonly EntityEntry entityEntry;
    private object[] propertyValues;
    private object[] databaseSnapshot;
    private int[] dirtyProperties;
    private bool hasDirtyCollection;
    private bool dirtyCheckPossible;
    private bool dirtyCheckHandledByInterceptor;

    public FlushEntityEvent(IEventSource source, object entity, EntityEntry entry)
      : base(source)
    {
      this.entity = entity;
      this.entityEntry = entry;
    }

    public object Entity => this.entity;

    public EntityEntry EntityEntry => this.entityEntry;

    public object[] PropertyValues
    {
      get => this.propertyValues;
      set => this.propertyValues = value;
    }

    public object[] DatabaseSnapshot
    {
      get => this.databaseSnapshot;
      set => this.databaseSnapshot = value;
    }

    public int[] DirtyProperties
    {
      get => this.dirtyProperties;
      set => this.dirtyProperties = value;
    }

    public bool HasDirtyCollection
    {
      get => this.hasDirtyCollection;
      set => this.hasDirtyCollection = value;
    }

    public bool DirtyCheckPossible
    {
      get => this.dirtyCheckPossible;
      set => this.dirtyCheckPossible = value;
    }

    public bool DirtyCheckHandledByInterceptor
    {
      get => this.dirtyCheckHandledByInterceptor;
      set => this.dirtyCheckHandledByInterceptor = value;
    }

    public bool HasDatabaseSnapshot => this.databaseSnapshot != null;
  }
}

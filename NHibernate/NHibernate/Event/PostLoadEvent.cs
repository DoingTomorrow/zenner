// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.PostLoadEvent
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;
using System;

#nullable disable
namespace NHibernate.Event
{
  [Serializable]
  public class PostLoadEvent(IEventSource source) : 
    AbstractEvent(source),
    IPostDatabaseOperationEventArgs,
    IDatabaseEventArgs
  {
    private object entity;
    private object id;
    private IEntityPersister persister;

    public object Entity
    {
      get => this.entity;
      set => this.entity = value;
    }

    public object Id
    {
      get => this.id;
      set => this.id = value;
    }

    public IEntityPersister Persister
    {
      get => this.persister;
      set => this.persister = value;
    }
  }
}

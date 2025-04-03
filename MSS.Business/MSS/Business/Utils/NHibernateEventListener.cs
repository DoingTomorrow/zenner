// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.NHibernateEventListener
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Business.Events;
using MSS.Interfaces;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using System;

#nullable disable
namespace MSS.Business.Utils
{
  public class NHibernateEventListener : 
    IPreInsertEventListener,
    IPreUpdateEventListener,
    IPostInsertEventListener,
    IPostUpdateEventListener,
    IPostDeleteEventListener
  {
    private string[] preinsertPropertyNamesToUpdate = new string[3]
    {
      "CreatedOn",
      "LastChangedOn",
      "LastUpdatedOn"
    };
    private string[] preupdatePropertyNamesToUpdate = new string[2]
    {
      "LastChangedOn",
      "LastUpdatedOn"
    };

    private bool IsSynchronizationObject(object obj) => obj is ISynchronizableEntity;

    public bool OnPreInsert(PreInsertEvent @event)
    {
      DateTime now = DateTime.Now;
      foreach (string propertyName in this.preinsertPropertyNamesToUpdate)
        this.SetEntityAndProperty(@event.Persister, @event.State, propertyName, (object) now);
      return false;
    }

    public bool OnPreUpdate(PreUpdateEvent @event)
    {
      DateTime now = DateTime.Now;
      foreach (string propertyName in this.preupdatePropertyNamesToUpdate)
        this.SetEntityAndProperty(@event.Persister, @event.State, propertyName, (object) now);
      return false;
    }

    private void SetEntityAndProperty(
      IEntityPersister persister,
      object[] state,
      string propertyName,
      object value)
    {
      int index = Array.IndexOf<string>(persister.PropertyNames, propertyName);
      if (index == -1)
        return;
      state[index] = value;
    }

    public void OnPostInsert(PostInsertEvent @event)
    {
      if (!this.IsSynchronizationObject(@event.Entity))
        return;
      EventPublisher.Publish<LocalDatabaseModified>(new LocalDatabaseModified()
      {
        IsChanged = true
      }, (object) this);
    }

    public void OnPostUpdate(PostUpdateEvent @event)
    {
      if (!this.IsSynchronizationObject(@event.Entity))
        return;
      EventPublisher.Publish<LocalDatabaseModified>(new LocalDatabaseModified()
      {
        IsChanged = true
      }, (object) this);
    }

    public void OnPostDelete(PostDeleteEvent @event)
    {
      if (!this.IsSynchronizationObject(@event.Entity))
        return;
      EventPublisher.Publish<LocalDatabaseModified>(new LocalDatabaseModified()
      {
        IsChanged = true
      }, (object) this);
    }
  }
}

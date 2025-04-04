// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.DefaultPreLoadEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;
using System;

#nullable disable
namespace NHibernate.Event.Default
{
  [Serializable]
  public class DefaultPreLoadEventListener : IPreLoadEventListener
  {
    public virtual void OnPreLoad(PreLoadEvent @event)
    {
      IEntityPersister persister = @event.Persister;
      @event.Session.Interceptor.OnLoad(@event.Entity, @event.Id, @event.State, persister.PropertyNames, persister.PropertyTypes);
    }
  }
}

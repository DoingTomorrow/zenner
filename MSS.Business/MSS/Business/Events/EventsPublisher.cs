// Decompiled with JetBrains decompiler
// Type: MSS.Business.Events.EventsPublisher
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Interfaces;
using System;
using System.Collections.Generic;

#nullable disable
namespace MSS.Business.Events
{
  public class EventsPublisher : IEventPublisher
  {
    private static readonly IDictionary<Type, List<Action<object>>> Subscribers = (IDictionary<Type, List<Action<object>>>) new Dictionary<Type, List<Action<object>>>();

    public void Publish<T>(T eventToPublish, IViewModel publishedBy)
    {
      EventPublisher.Publish<T>(eventToPublish, publishedBy);
    }

    public void Publish<T>(T eventToPublish, object publishedBy)
    {
      EventPublisher.Publish<T>(eventToPublish, publishedBy);
    }

    public void Register<T>(Action<T> action) => EventPublisher.Register<T>(action);
  }
}

// Decompiled with JetBrains decompiler
// Type: MSS.Business.Events.EventPublisher
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using MSS.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

#nullable disable
namespace MSS.Business.Events
{
  public class EventPublisher
  {
    private static readonly IDictionary<Type, List<Action<object>>> Subscribers = (IDictionary<Type, List<Action<object>>>) new Dictionary<Type, List<Action<object>>>();

    public static void Publish<T>(T eventToPublish, IViewModel publishedBy)
    {
      List<Action<object>> actionList;
      lock (EventPublisher.Subscribers)
      {
        if (!EventPublisher.Subscribers.TryGetValue(typeof (T), out actionList))
          return;
      }
      foreach (Action<object> action in actionList)
      {
        if (action.Target != publishedBy)
          action((object) eventToPublish);
      }
    }

    public static void Publish<T>(T eventToPublish, object publishedBy)
    {
      List<Action<object>> actionList;
      lock (EventPublisher.Subscribers)
      {
        if (!EventPublisher.Subscribers.TryGetValue(typeof (T), out actionList))
          return;
      }
      foreach (Action<object> action in actionList)
      {
        if (action.Target != publishedBy)
          action((object) eventToPublish);
      }
    }

    public static void Register<T>(Action<T> action)
    {
      lock (EventPublisher.Subscribers)
      {
        List<Action<object>> actionList;
        if (!EventPublisher.Subscribers.TryGetValue(typeof (T), out actionList) && !EventPublisher.Subscribers.TryGetValue(typeof (T), out actionList))
          EventPublisher.Subscribers[typeof (T)] = actionList = new List<Action<object>>();
        Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
        Debug.Assert(dispatcher != null);
        Action<object> item = (Action<object>) (o =>
        {
          if (dispatcher != Dispatcher.CurrentDispatcher)
            dispatcher.Invoke((Action) (() => action((T) o)));
          else
            action((T) o);
        });
        if (action != null && action.Target is IViewModel)
          ((IViewModel) action.Target).Disposed += (Action) (() => EventPublisher.Unregister<T>(item));
        else if (action.Target is Window)
          ((Window) action.Target).Closed += (EventHandler) delegate
          {
            EventPublisher.Unregister<T>(item);
          };
        else if (action.Target is UserControl)
          ((FrameworkElement) action.Target).Unloaded += (RoutedEventHandler) delegate
          {
            EventPublisher.Unregister<T>(item);
          };
        actionList.Add(item);
      }
    }

    private static void Unregister<T>(Action<object> action)
    {
      lock (EventPublisher.Subscribers)
      {
        List<Action<object>> actionList;
        if (!EventPublisher.Subscribers.TryGetValue(typeof (T), out actionList))
          return;
        actionList.Remove(action);
      }
    }
  }
}

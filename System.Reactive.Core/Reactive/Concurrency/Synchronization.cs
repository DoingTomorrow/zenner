// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.Synchronization
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.ComponentModel;
using System.Reactive.Disposables;
using System.Threading;

#nullable disable
namespace System.Reactive.Concurrency
{
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  public static class Synchronization
  {
    public static IObservable<TSource> SubscribeOn<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return (IObservable<TSource>) new AnonymousObservable<TSource>((Func<IObserver<TSource>, IDisposable>) (observer =>
      {
        SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
        SerialDisposable d = new SerialDisposable();
        d.Disposable = (IDisposable) assignmentDisposable;
        assignmentDisposable.Disposable = scheduler.Schedule((Action) (() => d.Disposable = (IDisposable) new ScheduledDisposable(scheduler, source.SubscribeSafe<TSource>(observer))));
        return (IDisposable) d;
      }));
    }

    public static IObservable<TSource> SubscribeOn<TSource>(
      IObservable<TSource> source,
      SynchronizationContext context)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (context == null)
        throw new ArgumentNullException(nameof (context));
      return (IObservable<TSource>) new AnonymousObservable<TSource>((Func<IObserver<TSource>, IDisposable>) (observer =>
      {
        SingleAssignmentDisposable subscription = new SingleAssignmentDisposable();
        context.PostWithStartComplete((Action) (() =>
        {
          if (subscription.IsDisposed)
            return;
          subscription.Disposable = (IDisposable) new ContextDisposable(context, source.SubscribeSafe<TSource>(observer));
        }));
        return (IDisposable) subscription;
      }));
    }

    public static IObservable<TSource> ObserveOn<TSource>(
      IObservable<TSource> source,
      IScheduler scheduler)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return scheduler != null ? (IObservable<TSource>) new System.Reactive.Concurrency.ObserveOn<TSource>(source, scheduler) : throw new ArgumentNullException(nameof (scheduler));
    }

    public static IObservable<TSource> ObserveOn<TSource>(
      IObservable<TSource> source,
      SynchronizationContext context)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return context != null ? (IObservable<TSource>) new System.Reactive.Concurrency.ObserveOn<TSource>(source, context) : throw new ArgumentNullException(nameof (context));
    }

    public static IObservable<TSource> Synchronize<TSource>(IObservable<TSource> source)
    {
      return source != null ? (IObservable<TSource>) new System.Reactive.Concurrency.Synchronize<TSource>(source) : throw new ArgumentNullException(nameof (source));
    }

    public static IObservable<TSource> Synchronize<TSource>(
      IObservable<TSource> source,
      object gate)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return gate != null ? (IObservable<TSource>) new System.Reactive.Concurrency.Synchronize<TSource>(source, gate) : throw new ArgumentNullException(nameof (gate));
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: System.ObservableExtensions
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.ComponentModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading;

#nullable disable
namespace System
{
  public static class ObservableExtensions
  {
    public static IDisposable Subscribe<T>(this IObservable<T> source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      return source.Subscribe((IObserver<T>) new AnonymousObserver<T>(Stubs<T>.Ignore, Stubs.Throw, Stubs.Nop));
    }

    public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      return source.Subscribe((IObserver<T>) new AnonymousObserver<T>(onNext, Stubs.Throw, Stubs.Nop));
    }

    public static IDisposable Subscribe<T>(
      this IObservable<T> source,
      Action<T> onNext,
      Action<Exception> onError)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      if (onError == null)
        throw new ArgumentNullException(nameof (onError));
      return source.Subscribe((IObserver<T>) new AnonymousObserver<T>(onNext, onError, Stubs.Nop));
    }

    public static IDisposable Subscribe<T>(
      this IObservable<T> source,
      Action<T> onNext,
      Action onCompleted)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      if (onCompleted == null)
        throw new ArgumentNullException(nameof (onCompleted));
      return source.Subscribe((IObserver<T>) new AnonymousObserver<T>(onNext, Stubs.Throw, onCompleted));
    }

    public static IDisposable Subscribe<T>(
      this IObservable<T> source,
      Action<T> onNext,
      Action<Exception> onError,
      Action onCompleted)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      if (onError == null)
        throw new ArgumentNullException(nameof (onError));
      if (onCompleted == null)
        throw new ArgumentNullException(nameof (onCompleted));
      return source.Subscribe((IObserver<T>) new AnonymousObserver<T>(onNext, onError, onCompleted));
    }

    public static void Subscribe<T>(
      this IObservable<T> source,
      IObserver<T> observer,
      CancellationToken token)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (observer == null)
        throw new ArgumentNullException(nameof (observer));
      source.Subscribe_<T>(observer, token);
    }

    public static void Subscribe<T>(this IObservable<T> source, CancellationToken token)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      source.Subscribe_<T>((IObserver<T>) new AnonymousObserver<T>(Stubs<T>.Ignore, Stubs.Throw, Stubs.Nop), token);
    }

    public static void Subscribe<T>(
      this IObservable<T> source,
      Action<T> onNext,
      CancellationToken token)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      source.Subscribe_<T>((IObserver<T>) new AnonymousObserver<T>(onNext, Stubs.Throw, Stubs.Nop), token);
    }

    public static void Subscribe<T>(
      this IObservable<T> source,
      Action<T> onNext,
      Action<Exception> onError,
      CancellationToken token)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      if (onError == null)
        throw new ArgumentNullException(nameof (onError));
      source.Subscribe_<T>((IObserver<T>) new AnonymousObserver<T>(onNext, onError, Stubs.Nop), token);
    }

    public static void Subscribe<T>(
      this IObservable<T> source,
      Action<T> onNext,
      Action onCompleted,
      CancellationToken token)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      if (onCompleted == null)
        throw new ArgumentNullException(nameof (onCompleted));
      source.Subscribe_<T>((IObserver<T>) new AnonymousObserver<T>(onNext, Stubs.Throw, onCompleted), token);
    }

    public static void Subscribe<T>(
      this IObservable<T> source,
      Action<T> onNext,
      Action<Exception> onError,
      Action onCompleted,
      CancellationToken token)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (onNext == null)
        throw new ArgumentNullException(nameof (onNext));
      if (onError == null)
        throw new ArgumentNullException(nameof (onError));
      if (onCompleted == null)
        throw new ArgumentNullException(nameof (onCompleted));
      source.Subscribe_<T>((IObserver<T>) new AnonymousObserver<T>(onNext, onError, onCompleted), token);
    }

    private static void Subscribe_<T>(
      this IObservable<T> source,
      IObserver<T> observer,
      CancellationToken token)
    {
      if (token.CanBeCanceled)
      {
        if (token.IsCancellationRequested)
          return;
        SingleAssignmentDisposable r = new SingleAssignmentDisposable();
        IDisposable disposable = source.Subscribe<T>(new Action<T>(observer.OnNext), (Action<Exception>) (ex =>
        {
          using (r)
            observer.OnError(ex);
        }), (Action) (() =>
        {
          using (r)
            observer.OnCompleted();
        }));
        r.Disposable = (IDisposable) token.Register(new Action(disposable.Dispose));
      }
      else
        source.Subscribe(observer);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static IDisposable SubscribeSafe<T>(this IObservable<T> source, IObserver<T> observer)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (observer == null)
        throw new ArgumentNullException(nameof (observer));
      switch (source)
      {
        case ObservableBase<T> _:
          return source.Subscribe(observer);
        case IProducer<T> producer:
          return producer.SubscribeRaw(observer, false);
        default:
          IDisposable disposable = Disposable.Empty;
          try
          {
            disposable = source.Subscribe(observer);
          }
          catch (Exception ex)
          {
            observer.OnError(ex);
          }
          return disposable;
      }
    }
  }
}

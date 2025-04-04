// Decompiled with JetBrains decompiler
// Type: System.Reactive.Subjects.Subject
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Linq;

#nullable disable
namespace System.Reactive.Subjects
{
  public static class Subject
  {
    public static ISubject<TSource, TResult> Create<TSource, TResult>(
      IObserver<TSource> observer,
      IObservable<TResult> observable)
    {
      if (observer == null)
        throw new ArgumentNullException(nameof (observer));
      return observable != null ? (ISubject<TSource, TResult>) new Subject.AnonymousSubject<TSource, TResult>(observer, observable) : throw new ArgumentNullException(nameof (observable));
    }

    public static ISubject<T> Create<T>(IObserver<T> observer, IObservable<T> observable)
    {
      if (observer == null)
        throw new ArgumentNullException(nameof (observer));
      return observable != null ? (ISubject<T>) new Subject.AnonymousSubject<T>(observer, observable) : throw new ArgumentNullException(nameof (observable));
    }

    public static ISubject<TSource, TResult> Synchronize<TSource, TResult>(
      ISubject<TSource, TResult> subject)
    {
      return subject != null ? (ISubject<TSource, TResult>) new Subject.AnonymousSubject<TSource, TResult>(Observer.Synchronize<TSource>((IObserver<TSource>) subject), (IObservable<TResult>) subject) : throw new ArgumentNullException(nameof (subject));
    }

    public static ISubject<TSource> Synchronize<TSource>(ISubject<TSource> subject)
    {
      return subject != null ? (ISubject<TSource>) new Subject.AnonymousSubject<TSource>(Observer.Synchronize<TSource>((IObserver<TSource>) subject), (IObservable<TSource>) subject) : throw new ArgumentNullException(nameof (subject));
    }

    public static ISubject<TSource, TResult> Synchronize<TSource, TResult>(
      ISubject<TSource, TResult> subject,
      IScheduler scheduler)
    {
      if (subject == null)
        throw new ArgumentNullException(nameof (subject));
      return scheduler != null ? (ISubject<TSource, TResult>) new Subject.AnonymousSubject<TSource, TResult>(Observer.Synchronize<TSource>((IObserver<TSource>) subject), subject.ObserveOn<TResult>(scheduler)) : throw new ArgumentNullException(nameof (scheduler));
    }

    public static ISubject<TSource> Synchronize<TSource>(
      ISubject<TSource> subject,
      IScheduler scheduler)
    {
      if (subject == null)
        throw new ArgumentNullException(nameof (subject));
      return scheduler != null ? (ISubject<TSource>) new Subject.AnonymousSubject<TSource>(Observer.Synchronize<TSource>((IObserver<TSource>) subject), subject.ObserveOn<TSource>(scheduler)) : throw new ArgumentNullException(nameof (scheduler));
    }

    private class AnonymousSubject<T, U> : ISubject<T, U>, IObserver<T>, IObservable<U>
    {
      private readonly IObserver<T> _observer;
      private readonly IObservable<U> _observable;

      public AnonymousSubject(IObserver<T> observer, IObservable<U> observable)
      {
        this._observer = observer;
        this._observable = observable;
      }

      public void OnCompleted() => this._observer.OnCompleted();

      public void OnError(Exception error)
      {
        if (error == null)
          throw new ArgumentNullException(nameof (error));
        this._observer.OnError(error);
      }

      public void OnNext(T value) => this._observer.OnNext(value);

      public IDisposable Subscribe(IObserver<U> observer)
      {
        return observer != null ? this._observable.Subscribe(observer) : throw new ArgumentNullException(nameof (observer));
      }
    }

    private class AnonymousSubject<T>(IObserver<T> observer, IObservable<T> observable) : 
      Subject.AnonymousSubject<T, T>(observer, observable),
      ISubject<T>,
      ISubject<T, T>,
      IObserver<T>,
      IObservable<T>
    {
    }
  }
}

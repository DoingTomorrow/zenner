// Decompiled with JetBrains decompiler
// Type: System.Reactive.TailRecursiveSink`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive
{
  internal abstract class TailRecursiveSink<TSource> : Sink<TSource>, IObserver<TSource>
  {
    private bool _isDisposed;
    private SerialDisposable _subscription;
    private AsyncLock _gate;
    private Stack<IEnumerator<IObservable<TSource>>> _stack;
    private Stack<int?> _length;
    protected Action _recurse;

    public TailRecursiveSink(IObserver<TSource> observer, IDisposable cancel)
      : base(observer, cancel)
    {
    }

    public IDisposable Run(IEnumerable<IObservable<TSource>> sources)
    {
      this._isDisposed = false;
      this._subscription = new SerialDisposable();
      this._gate = new AsyncLock();
      this._stack = new Stack<IEnumerator<IObservable<TSource>>>();
      this._length = new Stack<int?>();
      IEnumerator<IObservable<TSource>> result = (IEnumerator<IObservable<TSource>>) null;
      if (!this.TryGetEnumerator(sources, out result))
        return Disposable.Empty;
      this._stack.Push(result);
      this._length.Push(Helpers.GetLength<IObservable<TSource>>(sources));
      return (IDisposable) StableCompositeDisposable.Create((IDisposable) this._subscription, SchedulerDefaults.TailRecursion.Schedule((Action<Action>) (self =>
      {
        this._recurse = self;
        this._gate.Wait(new Action(this.MoveNext));
      })), Disposable.Create((Action) (() => this._gate.Wait(new Action(this.Dispose)))));
    }

    protected abstract IEnumerable<IObservable<TSource>> Extract(IObservable<TSource> source);

    private void MoveNext()
    {
      bool flag = false;
      IObservable<TSource> source1 = (IObservable<TSource>) null;
      while (this._stack.Count != 0)
      {
        if (this._isDisposed)
          return;
        IEnumerator<IObservable<TSource>> enumerator = this._stack.Peek();
        int? nullable1 = this._length.Peek();
        IObservable<TSource> source2 = (IObservable<TSource>) null;
        try
        {
          flag = enumerator.MoveNext();
          if (flag)
            source2 = enumerator.Current;
        }
        catch (Exception ex)
        {
          enumerator.Dispose();
          this._observer.OnError(ex);
          base.Dispose();
          return;
        }
        if (!flag)
        {
          enumerator.Dispose();
          this._stack.Pop();
          this._length.Pop();
        }
        else
        {
          int? nullable2 = nullable1;
          int? nullable3 = nullable2.HasValue ? new int?(nullable2.GetValueOrDefault() - 1) : new int?();
          this._length.Pop();
          this._length.Push(nullable3);
          try
          {
            source1 = Helpers.Unpack<TSource>(source2);
          }
          catch (Exception ex)
          {
            if (this.Fail(ex))
              return;
            enumerator.Dispose();
            return;
          }
          int? nullable4 = nullable3;
          int num = 0;
          if ((nullable4.GetValueOrDefault() == num ? (nullable4.HasValue ? 1 : 0) : 0) != 0)
          {
            enumerator.Dispose();
            this._stack.Pop();
            this._length.Pop();
          }
          IEnumerable<IObservable<TSource>> observables = this.Extract(source1);
          if (observables != null)
          {
            IEnumerator<IObservable<TSource>> result = (IEnumerator<IObservable<TSource>>) null;
            if (!this.TryGetEnumerator(observables, out result))
              return;
            this._stack.Push(result);
            this._length.Push(Helpers.GetLength<IObservable<TSource>>(observables));
            flag = false;
          }
        }
        if (flag)
          break;
      }
      if (!flag)
      {
        this.Done();
      }
      else
      {
        SingleAssignmentDisposable assignmentDisposable = new SingleAssignmentDisposable();
        this._subscription.Disposable = (IDisposable) assignmentDisposable;
        assignmentDisposable.Disposable = source1.SubscribeSafe<TSource>((IObserver<TSource>) this);
      }
    }

    private new void Dispose()
    {
      while (this._stack.Count > 0)
      {
        IEnumerator<IObservable<TSource>> enumerator = this._stack.Pop();
        this._length.Pop();
        enumerator.Dispose();
      }
      this._isDisposed = true;
    }

    private bool TryGetEnumerator(
      IEnumerable<IObservable<TSource>> sources,
      out IEnumerator<IObservable<TSource>> result)
    {
      try
      {
        result = sources.GetEnumerator();
        return true;
      }
      catch (Exception ex)
      {
        this._observer.OnError(ex);
        base.Dispose();
        result = (IEnumerator<IObservable<TSource>>) null;
        return false;
      }
    }

    public abstract void OnCompleted();

    public abstract void OnError(Exception error);

    public abstract void OnNext(TSource value);

    protected virtual void Done()
    {
      this._observer.OnCompleted();
      base.Dispose();
    }

    protected virtual bool Fail(Exception error)
    {
      this._observer.OnError(error);
      base.Dispose();
      return false;
    }
  }
}

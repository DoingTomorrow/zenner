// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.ForEach`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Threading;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class ForEach<TSource>
  {
    public class _ : IObserver<TSource>
    {
      private readonly Action<TSource> _onNext;
      private readonly Action _done;
      private Exception _exception;
      private int _stopped;

      public _(Action<TSource> onNext, Action done)
      {
        this._onNext = onNext;
        this._done = done;
        this._stopped = 0;
      }

      public Exception Error => this._exception;

      public void OnNext(TSource value)
      {
        if (this._stopped != 0)
          return;
        try
        {
          this._onNext(value);
        }
        catch (Exception ex)
        {
          this.OnError(ex);
        }
      }

      public void OnError(Exception error)
      {
        if (Interlocked.Exchange(ref this._stopped, 1) != 0)
          return;
        this._exception = error;
        this._done();
      }

      public void OnCompleted()
      {
        if (Interlocked.Exchange(ref this._stopped, 1) != 0)
          return;
        this._done();
      }
    }

    public class ForEachImpl : IObserver<TSource>
    {
      private readonly Action<TSource, int> _onNext;
      private readonly Action _done;
      private int _index;
      private Exception _exception;
      private int _stopped;

      public ForEachImpl(Action<TSource, int> onNext, Action done)
      {
        this._onNext = onNext;
        this._done = done;
        this._index = 0;
        this._stopped = 0;
      }

      public Exception Error => this._exception;

      public void OnNext(TSource value)
      {
        if (this._stopped != 0)
          return;
        try
        {
          this._onNext(value, checked (this._index++));
        }
        catch (Exception ex)
        {
          this.OnError(ex);
        }
      }

      public void OnError(Exception error)
      {
        if (Interlocked.Exchange(ref this._stopped, 1) != 0)
          return;
        this._exception = error;
        this._done();
      }

      public void OnCompleted()
      {
        if (Interlocked.Exchange(ref this._stopped, 1) != 0)
          return;
        this._done();
      }
    }
  }
}

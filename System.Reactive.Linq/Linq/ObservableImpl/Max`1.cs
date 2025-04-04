// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Max`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Max<TSource> : Producer<TSource>
  {
    private readonly IObservable<TSource> _source;
    private readonly IComparer<TSource> _comparer;

    public Max(IObservable<TSource> source, IComparer<TSource> comparer)
    {
      this._source = source;
      this._comparer = comparer;
    }

    protected override IDisposable Run(
      IObserver<TSource> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      if ((object) default (TSource) == null)
      {
        Max<TSource>._ obj = new Max<TSource>._(this, observer, cancel);
        setSink((IDisposable) obj);
        return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
      }
      Max<TSource>.Delta delta = new Max<TSource>.Delta(this, observer, cancel);
      setSink((IDisposable) delta);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) delta);
    }

    private class Delta : Sink<TSource>, IObserver<TSource>
    {
      private readonly Max<TSource> _parent;
      private bool _hasValue;
      private TSource _lastValue;

      public Delta(Max<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._hasValue = false;
        this._lastValue = default (TSource);
      }

      public void OnNext(TSource value)
      {
        if (this._hasValue)
        {
          int num;
          try
          {
            num = this._parent._comparer.Compare(value, this._lastValue);
          }
          catch (Exception ex)
          {
            this._observer.OnError(ex);
            this.Dispose();
            return;
          }
          if (num <= 0)
            return;
          this._lastValue = value;
        }
        else
        {
          this._hasValue = true;
          this._lastValue = value;
        }
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        if (!this._hasValue)
        {
          this._observer.OnError((Exception) new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
        }
        else
        {
          this._observer.OnNext(this._lastValue);
          this._observer.OnCompleted();
        }
        this.Dispose();
      }
    }

    private class _ : Sink<TSource>, IObserver<TSource>
    {
      private readonly Max<TSource> _parent;
      private TSource _lastValue;

      public _(Max<TSource> parent, IObserver<TSource> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
        this._lastValue = default (TSource);
      }

      public void OnNext(TSource value)
      {
        if ((object) value == null)
          return;
        if ((object) this._lastValue == null)
        {
          this._lastValue = value;
        }
        else
        {
          int num;
          try
          {
            num = this._parent._comparer.Compare(value, this._lastValue);
          }
          catch (Exception ex)
          {
            this._observer.OnError(ex);
            this.Dispose();
            return;
          }
          if (num <= 0)
            return;
          this._lastValue = value;
        }
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext(this._lastValue);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Contains`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Contains<TSource> : Producer<bool>
  {
    private readonly IObservable<TSource> _source;
    private readonly TSource _value;
    private readonly IEqualityComparer<TSource> _comparer;

    public Contains(
      IObservable<TSource> source,
      TSource value,
      IEqualityComparer<TSource> comparer)
    {
      this._source = source;
      this._value = value;
      this._comparer = comparer;
    }

    protected override IDisposable Run(
      IObserver<bool> observer,
      IDisposable cancel,
      Action<IDisposable> setSink)
    {
      Contains<TSource>._ obj = new Contains<TSource>._(this, observer, cancel);
      setSink((IDisposable) obj);
      return this._source.SubscribeSafe<TSource>((IObserver<TSource>) obj);
    }

    private class _ : Sink<bool>, IObserver<TSource>
    {
      private readonly Contains<TSource> _parent;

      public _(Contains<TSource> parent, IObserver<bool> observer, IDisposable cancel)
        : base(observer, cancel)
      {
        this._parent = parent;
      }

      public void OnNext(TSource value)
      {
        bool flag;
        try
        {
          flag = this._parent._comparer.Equals(value, this._parent._value);
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        if (!flag)
          return;
        this._observer.OnNext(true);
        this._observer.OnCompleted();
        this.Dispose();
      }

      public void OnError(Exception error)
      {
        this._observer.OnError(error);
        this.Dispose();
      }

      public void OnCompleted()
      {
        this._observer.OnNext(false);
        this._observer.OnCompleted();
        this.Dispose();
      }
    }
  }
}

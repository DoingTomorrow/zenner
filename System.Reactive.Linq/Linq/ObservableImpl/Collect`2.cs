// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.Collect`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class Collect<TSource, TResult> : PushToPullAdapter<TSource, TResult>
  {
    private readonly Func<TResult> _getInitialCollector;
    private readonly Func<TResult, TSource, TResult> _merge;
    private readonly Func<TResult, TResult> _getNewCollector;

    public Collect(
      IObservable<TSource> source,
      Func<TResult> getInitialCollector,
      Func<TResult, TSource, TResult> merge,
      Func<TResult, TResult> getNewCollector)
      : base(source)
    {
      this._getInitialCollector = getInitialCollector;
      this._merge = merge;
      this._getNewCollector = getNewCollector;
    }

    protected override PushToPullSink<TSource, TResult> Run(IDisposable subscription)
    {
      Collect<TSource, TResult>._ obj = new Collect<TSource, TResult>._(this, subscription);
      obj.Initialize();
      return (PushToPullSink<TSource, TResult>) obj;
    }

    private class _ : PushToPullSink<TSource, TResult>
    {
      private readonly Collect<TSource, TResult> _parent;
      private object _gate;
      private TResult _collector;
      private bool _hasFailed;
      private Exception _error;
      private bool _hasCompleted;
      private bool _done;

      public _(Collect<TSource, TResult> parent, IDisposable subscription)
        : base(subscription)
      {
        this._parent = parent;
      }

      public void Initialize()
      {
        this._gate = new object();
        this._collector = this._parent._getInitialCollector();
      }

      public override void OnNext(TSource value)
      {
        lock (this._gate)
        {
          try
          {
            this._collector = this._parent._merge(this._collector, value);
          }
          catch (Exception ex)
          {
            this._error = ex;
            this._hasFailed = true;
            this.Dispose();
          }
        }
      }

      public override void OnError(Exception error)
      {
        this.Dispose();
        lock (this._gate)
        {
          this._error = error;
          this._hasFailed = true;
        }
      }

      public override void OnCompleted()
      {
        this.Dispose();
        lock (this._gate)
          this._hasCompleted = true;
      }

      public override bool TryMoveNext(out TResult current)
      {
        lock (this._gate)
        {
          if (this._hasFailed)
          {
            current = default (TResult);
            this._error.Throw();
          }
          else if (this._hasCompleted)
          {
            if (this._done)
            {
              current = default (TResult);
              return false;
            }
            current = this._collector;
            this._done = true;
          }
          else
          {
            current = this._collector;
            try
            {
              this._collector = this._parent._getNewCollector(current);
            }
            catch
            {
              this.Dispose();
              throw;
            }
          }
          return true;
        }
      }
    }
  }
}

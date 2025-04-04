// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.CombineLatestSink`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal abstract class CombineLatestSink<TResult> : Sink<TResult>, ICombineLatest
  {
    protected readonly object _gate;
    private bool _hasValueAll;
    private readonly bool[] _hasValue;
    private readonly bool[] _isDone;

    public CombineLatestSink(int arity, IObserver<TResult> observer, IDisposable cancel)
      : base(observer, cancel)
    {
      this._gate = new object();
      this._hasValue = new bool[arity];
      this._isDone = new bool[arity];
    }

    public void Next(int index)
    {
      if (!this._hasValueAll)
      {
        this._hasValue[index] = true;
        bool flag1 = true;
        foreach (bool flag2 in this._hasValue)
        {
          if (!flag2)
          {
            flag1 = false;
            break;
          }
        }
        this._hasValueAll = flag1;
      }
      if (this._hasValueAll)
      {
        TResult result1 = default (TResult);
        TResult result2;
        try
        {
          result2 = this.GetResult();
        }
        catch (Exception ex)
        {
          this._observer.OnError(ex);
          this.Dispose();
          return;
        }
        this._observer.OnNext(result2);
      }
      else
      {
        bool flag = true;
        for (int index1 = 0; index1 < this._isDone.Length; ++index1)
        {
          if (index1 != index && !this._isDone[index1])
          {
            flag = false;
            break;
          }
        }
        if (!flag)
          return;
        this._observer.OnCompleted();
        this.Dispose();
      }
    }

    protected abstract TResult GetResult();

    public void Fail(Exception error)
    {
      this._observer.OnError(error);
      this.Dispose();
    }

    public void Done(int index)
    {
      this._isDone[index] = true;
      bool flag1 = true;
      foreach (bool flag2 in this._isDone)
      {
        if (!flag2)
        {
          flag1 = false;
          break;
        }
      }
      if (!flag1)
        return;
      this._observer.OnCompleted();
      this.Dispose();
    }
  }
}

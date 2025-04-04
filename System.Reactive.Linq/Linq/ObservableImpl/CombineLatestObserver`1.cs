// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.CombineLatestObserver`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class CombineLatestObserver<T> : IObserver<T>
  {
    private readonly object _gate;
    private readonly ICombineLatest _parent;
    private readonly int _index;
    private readonly IDisposable _self;
    private T _value;

    public CombineLatestObserver(object gate, ICombineLatest parent, int index, IDisposable self)
    {
      this._gate = gate;
      this._parent = parent;
      this._index = index;
      this._self = self;
    }

    public T Value => this._value;

    public void OnNext(T value)
    {
      lock (this._gate)
      {
        this._value = value;
        this._parent.Next(this._index);
      }
    }

    public void OnError(Exception error)
    {
      this._self.Dispose();
      lock (this._gate)
        this._parent.Fail(error);
    }

    public void OnCompleted()
    {
      this._self.Dispose();
      lock (this._gate)
        this._parent.Done(this._index);
    }
  }
}

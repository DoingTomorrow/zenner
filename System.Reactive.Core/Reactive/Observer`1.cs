// Decompiled with JetBrains decompiler
// Type: System.Reactive.Observer`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

#nullable disable
namespace System.Reactive
{
  internal class Observer<T> : IObserver<T>
  {
    private readonly ImmutableList<IObserver<T>> _observers;

    public Observer(ImmutableList<IObserver<T>> observers) => this._observers = observers;

    public void OnCompleted()
    {
      foreach (IObserver<T> observer in this._observers.Data)
        observer.OnCompleted();
    }

    public void OnError(Exception error)
    {
      foreach (IObserver<T> observer in this._observers.Data)
        observer.OnError(error);
    }

    public void OnNext(T value)
    {
      foreach (IObserver<T> observer in this._observers.Data)
        observer.OnNext(value);
    }

    internal IObserver<T> Add(IObserver<T> observer)
    {
      return (IObserver<T>) new Observer<T>(this._observers.Add(observer));
    }

    internal IObserver<T> Remove(IObserver<T> observer)
    {
      int num = Array.IndexOf<IObserver<T>>(this._observers.Data, observer);
      if (num < 0)
        return (IObserver<T>) this;
      return this._observers.Data.Length == 2 ? this._observers.Data[1 - num] : (IObserver<T>) new Observer<T>(this._observers.Remove(observer));
    }
  }
}

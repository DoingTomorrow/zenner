// Decompiled with JetBrains decompiler
// Type: Ninject.Infrastructure.Future`1
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using System;

#nullable disable
namespace Ninject.Infrastructure
{
  public class Future<T>
  {
    private bool _hasValue;
    private T _value;

    public T Value
    {
      get
      {
        if (!this._hasValue)
        {
          this._value = this.Callback();
          this._hasValue = true;
        }
        return this._value;
      }
    }

    public Func<T> Callback { get; private set; }

    public Future(Func<T> callback) => this.Callback = callback;

    public static implicit operator T(Future<T> future) => future.Value;
  }
}

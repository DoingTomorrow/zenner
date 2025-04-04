// Decompiled with JetBrains decompiler
// Type: System.Reactive.EventSource`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;

#nullable disable
namespace System.Reactive
{
  internal class EventSource<T> : IEventSource<T>
  {
    private readonly IObservable<T> _source;
    private readonly Dictionary<Delegate, Stack<IDisposable>> _subscriptions;
    private readonly Action<Action<T>, T> _invokeHandler;

    public EventSource(IObservable<T> source, Action<Action<T>, T> invokeHandler)
    {
      this._source = source;
      this._invokeHandler = invokeHandler;
      this._subscriptions = new Dictionary<Delegate, Stack<IDisposable>>();
    }

    public event Action<T> OnNext
    {
      add
      {
        object gate = new object();
        bool isAdded = false;
        bool isDone = false;
        Action remove = (Action) (() =>
        {
          lock (gate)
          {
            if (isAdded)
              this.Remove((Delegate) value);
            else
              isDone = true;
          }
        });
        IDisposable disposable = this._source.Subscribe<T>((Action<T>) (x => this._invokeHandler(value, x)), (Action<Exception>) (ex =>
        {
          remove();
          ex.Throw();
        }), (Action) (() => remove()));
        lock (gate)
        {
          if (isDone)
            return;
          this.Add((Delegate) value, disposable);
          isAdded = true;
        }
      }
      remove => this.Remove((Delegate) value);
    }

    private void Add(Delegate handler, IDisposable disposable)
    {
      lock (this._subscriptions)
      {
        Stack<IDisposable> disposableStack = new Stack<IDisposable>();
        if (!this._subscriptions.TryGetValue(handler, out disposableStack))
          this._subscriptions[handler] = disposableStack = new Stack<IDisposable>();
        disposableStack.Push(disposable);
      }
    }

    private void Remove(Delegate handler)
    {
      IDisposable disposable = (IDisposable) null;
      lock (this._subscriptions)
      {
        Stack<IDisposable> disposableStack = new Stack<IDisposable>();
        if (this._subscriptions.TryGetValue(handler, out disposableStack))
        {
          disposable = disposableStack.Pop();
          if (disposableStack.Count == 0)
            this._subscriptions.Remove(handler);
        }
      }
      disposable?.Dispose();
    }
  }
}

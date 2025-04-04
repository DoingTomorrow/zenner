// Decompiled with JetBrains decompiler
// Type: System.Reactive.Subjects.ConnectableObservable`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Linq;

#nullable disable
namespace System.Reactive.Subjects
{
  internal class ConnectableObservable<TSource, TResult> : 
    IConnectableObservable<TResult>,
    IObservable<TResult>
  {
    private readonly ISubject<TSource, TResult> _subject;
    private readonly IObservable<TSource> _source;
    private readonly object _gate;
    private ConnectableObservable<TSource, TResult>.Connection _connection;

    public ConnectableObservable(IObservable<TSource> source, ISubject<TSource, TResult> subject)
    {
      this._subject = subject;
      this._source = source.AsObservable<TSource>();
      this._gate = new object();
    }

    public IDisposable Connect()
    {
      lock (this._gate)
      {
        if (this._connection == null)
          this._connection = new ConnectableObservable<TSource, TResult>.Connection(this, this._source.SubscribeSafe<TSource>((IObserver<TSource>) this._subject));
        return (IDisposable) this._connection;
      }
    }

    public IDisposable Subscribe(IObserver<TResult> observer)
    {
      return observer != null ? this._subject.SubscribeSafe<TResult>(observer) : throw new ArgumentNullException(nameof (observer));
    }

    private class Connection : IDisposable
    {
      private readonly ConnectableObservable<TSource, TResult> _parent;
      private IDisposable _subscription;

      public Connection(ConnectableObservable<TSource, TResult> parent, IDisposable subscription)
      {
        this._parent = parent;
        this._subscription = subscription;
      }

      public void Dispose()
      {
        lock (this._parent._gate)
        {
          if (this._subscription == null)
            return;
          this._subscription.Dispose();
          this._subscription = (IDisposable) null;
          this._parent._connection = (ConnectableObservable<TSource, TResult>.Connection) null;
        }
      }
    }
  }
}

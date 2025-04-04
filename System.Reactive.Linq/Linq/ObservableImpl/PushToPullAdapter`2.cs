// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.PushToPullAdapter`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections;
using System.Collections.Generic;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal abstract class PushToPullAdapter<TSource, TResult> : IEnumerable<TResult>, IEnumerable
  {
    private readonly IObservable<TSource> _source;

    public PushToPullAdapter(IObservable<TSource> source) => this._source = source;

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public IEnumerator<TResult> GetEnumerator()
    {
      SingleAssignmentDisposable subscription = new SingleAssignmentDisposable();
      PushToPullSink<TSource, TResult> enumerator = this.Run((IDisposable) subscription);
      subscription.Disposable = this._source.SubscribeSafe<TSource>((IObserver<TSource>) enumerator);
      return (IEnumerator<TResult>) enumerator;
    }

    protected abstract PushToPullSink<TSource, TResult> Run(IDisposable subscription);
  }
}

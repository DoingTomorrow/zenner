// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.GroupedObservable`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Disposables;
using System.Reactive.Subjects;

#nullable disable
namespace System.Reactive.Linq
{
  internal class GroupedObservable<TKey, TElement> : 
    ObservableBase<TElement>,
    IGroupedObservable<TKey, TElement>,
    IObservable<TElement>
  {
    private readonly TKey _key;
    private readonly IObservable<TElement> _subject;
    private readonly RefCountDisposable _refCount;

    public GroupedObservable(TKey key, ISubject<TElement> subject, RefCountDisposable refCount)
    {
      this._key = key;
      this._subject = (IObservable<TElement>) subject;
      this._refCount = refCount;
    }

    public GroupedObservable(TKey key, ISubject<TElement> subject)
    {
      this._key = key;
      this._subject = (IObservable<TElement>) subject;
    }

    public TKey Key => this._key;

    protected override IDisposable SubscribeCore(IObserver<TElement> observer)
    {
      return this._refCount != null ? (IDisposable) StableCompositeDisposable.Create(this._refCount.GetDisposable(), this._subject.Subscribe(observer)) : this._subject.Subscribe(observer);
    }
  }
}

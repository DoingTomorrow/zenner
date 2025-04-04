// Decompiled with JetBrains decompiler
// Type: System.Reactive.ConcatSink`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;

#nullable disable
namespace System.Reactive
{
  internal abstract class ConcatSink<TSource> : TailRecursiveSink<TSource>
  {
    public ConcatSink(IObserver<TSource> observer, IDisposable cancel)
      : base(observer, cancel)
    {
    }

    protected override IEnumerable<IObservable<TSource>> Extract(IObservable<TSource> source)
    {
      return source is IConcatenatable<TSource> concatenatable ? concatenatable.GetSources() : (IEnumerable<IObservable<TSource>>) null;
    }

    public override void OnCompleted() => this._recurse();
  }
}

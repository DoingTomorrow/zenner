// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.WindowObservable`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class WindowObservable<TSource>(IObservable<TSource> source, RefCountDisposable refCount) : 
    AddRef<TSource>(source, refCount)
  {
  }
}

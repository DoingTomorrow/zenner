// Decompiled with JetBrains decompiler
// Type: System.Reactive.Subjects.SubjectBase`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Subjects
{
  public abstract class SubjectBase<T> : 
    ISubject<T>,
    ISubject<T, T>,
    IObserver<T>,
    IObservable<T>,
    IDisposable
  {
    public abstract bool HasObservers { get; }

    public abstract bool IsDisposed { get; }

    public abstract void Dispose();

    public abstract void OnCompleted();

    public abstract void OnError(Exception error);

    public abstract void OnNext(T value);

    public abstract IDisposable Subscribe(IObserver<T> observer);
  }
}

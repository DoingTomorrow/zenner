// Decompiled with JetBrains decompiler
// Type: System.Reactive.DoneObserver`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

#nullable disable
namespace System.Reactive
{
  internal class DoneObserver<T> : IObserver<T>
  {
    public static readonly IObserver<T> Completed = (IObserver<T>) new DoneObserver<T>();

    public Exception Exception { get; set; }

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(T value)
    {
    }
  }
}

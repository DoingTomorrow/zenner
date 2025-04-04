// Decompiled with JetBrains decompiler
// Type: System.Reactive.IObserver`2
// Assembly: System.Reactive.Interfaces, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: F75E00F4-A403-435F-9B50-2B7670A5231F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Interfaces.dll

#nullable disable
namespace System.Reactive
{
  public interface IObserver<in TValue, out TResult>
  {
    TResult OnNext(TValue value);

    TResult OnError(Exception exception);

    TResult OnCompleted();
  }
}

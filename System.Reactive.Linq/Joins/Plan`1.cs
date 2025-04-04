// Decompiled with JetBrains decompiler
// Type: System.Reactive.Joins.Plan`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;

#nullable disable
namespace System.Reactive.Joins
{
  public abstract class Plan<TResult>
  {
    internal Plan()
    {
    }

    internal abstract ActivePlan Activate(
      Dictionary<object, IJoinObserver> externalSubscriptions,
      IObserver<TResult> observer,
      Action<ActivePlan> deactivate);

    internal static JoinObserver<TSource> CreateObserver<TSource>(
      Dictionary<object, IJoinObserver> externalSubscriptions,
      IObservable<TSource> observable,
      Action<Exception> onError)
    {
      IJoinObserver joinObserver = (IJoinObserver) null;
      JoinObserver<TSource> observer;
      if (!externalSubscriptions.TryGetValue((object) observable, out joinObserver))
      {
        observer = new JoinObserver<TSource>(observable, onError);
        externalSubscriptions.Add((object) observable, (IJoinObserver) observer);
      }
      else
        observer = (JoinObserver<TSource>) joinObserver;
      return observer;
    }
  }
}

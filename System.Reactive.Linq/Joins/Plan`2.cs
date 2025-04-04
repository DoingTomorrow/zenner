// Decompiled with JetBrains decompiler
// Type: System.Reactive.Joins.Plan`2
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;

#nullable disable
namespace System.Reactive.Joins
{
  internal class Plan<T1, TResult> : Plan<TResult>
  {
    internal Pattern<T1> Expression { get; private set; }

    internal Func<T1, TResult> Selector { get; private set; }

    internal Plan(Pattern<T1> expression, Func<T1, TResult> selector)
    {
      this.Expression = expression;
      this.Selector = selector;
    }

    internal override ActivePlan Activate(
      Dictionary<object, IJoinObserver> externalSubscriptions,
      IObserver<TResult> observer,
      Action<ActivePlan> deactivate)
    {
      Action<Exception> onError = new Action<Exception>(observer.OnError);
      JoinObserver<T1> firstJoinObserver = Plan<TResult>.CreateObserver<T1>(externalSubscriptions, this.Expression.First, onError);
      ActivePlan<T1> activePlan = (ActivePlan<T1>) null;
      activePlan = new ActivePlan<T1>(firstJoinObserver, (Action<T1>) (first =>
      {
        TResult result1 = default (TResult);
        TResult result2;
        try
        {
          result2 = this.Selector(first);
        }
        catch (Exception ex)
        {
          observer.OnError(ex);
          return;
        }
        observer.OnNext(result2);
      }), (Action) (() =>
      {
        firstJoinObserver.RemoveActivePlan((ActivePlan) activePlan);
        deactivate((ActivePlan) activePlan);
      }));
      firstJoinObserver.AddActivePlan((ActivePlan) activePlan);
      return (ActivePlan) activePlan;
    }
  }
}

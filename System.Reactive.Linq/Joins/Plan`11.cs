// Decompiled with JetBrains decompiler
// Type: System.Reactive.Joins.Plan`11
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;

#nullable disable
namespace System.Reactive.Joins
{
  internal class Plan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : Plan<TResult>
  {
    internal Pattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Expression { get; private set; }

    internal Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Selector { get; private set; }

    internal Plan(
      Pattern<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> expression,
      Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> selector)
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
      JoinObserver<T2> secondJoinObserver = Plan<TResult>.CreateObserver<T2>(externalSubscriptions, this.Expression.Second, onError);
      JoinObserver<T3> thirdJoinObserver = Plan<TResult>.CreateObserver<T3>(externalSubscriptions, this.Expression.Third, onError);
      JoinObserver<T4> fourthJoinObserver = Plan<TResult>.CreateObserver<T4>(externalSubscriptions, this.Expression.Fourth, onError);
      JoinObserver<T5> fifthJoinObserver = Plan<TResult>.CreateObserver<T5>(externalSubscriptions, this.Expression.Fifth, onError);
      JoinObserver<T6> sixthJoinObserver = Plan<TResult>.CreateObserver<T6>(externalSubscriptions, this.Expression.Sixth, onError);
      JoinObserver<T7> seventhJoinObserver = Plan<TResult>.CreateObserver<T7>(externalSubscriptions, this.Expression.Seventh, onError);
      JoinObserver<T8> eighthJoinObserver = Plan<TResult>.CreateObserver<T8>(externalSubscriptions, this.Expression.Eighth, onError);
      JoinObserver<T9> ninthJoinObserver = Plan<TResult>.CreateObserver<T9>(externalSubscriptions, this.Expression.Ninth, onError);
      JoinObserver<T10> tenthJoinObserver = Plan<TResult>.CreateObserver<T10>(externalSubscriptions, this.Expression.Tenth, onError);
      ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> activePlan = (ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>) null;
      activePlan = new ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(firstJoinObserver, secondJoinObserver, thirdJoinObserver, fourthJoinObserver, fifthJoinObserver, sixthJoinObserver, seventhJoinObserver, eighthJoinObserver, ninthJoinObserver, tenthJoinObserver, (Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>) ((first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth) =>
      {
        TResult result1 = default (TResult);
        TResult result2;
        try
        {
          result2 = this.Selector(first, second, third, fourth, fifth, sixth, seventh, eighth, ninth, tenth);
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
        secondJoinObserver.RemoveActivePlan((ActivePlan) activePlan);
        thirdJoinObserver.RemoveActivePlan((ActivePlan) activePlan);
        fourthJoinObserver.RemoveActivePlan((ActivePlan) activePlan);
        fifthJoinObserver.RemoveActivePlan((ActivePlan) activePlan);
        sixthJoinObserver.RemoveActivePlan((ActivePlan) activePlan);
        seventhJoinObserver.RemoveActivePlan((ActivePlan) activePlan);
        eighthJoinObserver.RemoveActivePlan((ActivePlan) activePlan);
        ninthJoinObserver.RemoveActivePlan((ActivePlan) activePlan);
        tenthJoinObserver.RemoveActivePlan((ActivePlan) activePlan);
        deactivate((ActivePlan) activePlan);
      }));
      firstJoinObserver.AddActivePlan((ActivePlan) activePlan);
      secondJoinObserver.AddActivePlan((ActivePlan) activePlan);
      thirdJoinObserver.AddActivePlan((ActivePlan) activePlan);
      fourthJoinObserver.AddActivePlan((ActivePlan) activePlan);
      fifthJoinObserver.AddActivePlan((ActivePlan) activePlan);
      sixthJoinObserver.AddActivePlan((ActivePlan) activePlan);
      seventhJoinObserver.AddActivePlan((ActivePlan) activePlan);
      eighthJoinObserver.AddActivePlan((ActivePlan) activePlan);
      ninthJoinObserver.AddActivePlan((ActivePlan) activePlan);
      tenthJoinObserver.AddActivePlan((ActivePlan) activePlan);
      return (ActivePlan) activePlan;
    }
  }
}

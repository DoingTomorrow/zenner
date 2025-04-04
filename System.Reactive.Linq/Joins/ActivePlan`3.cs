// Decompiled with JetBrains decompiler
// Type: System.Reactive.Joins.ActivePlan`3
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Joins
{
  internal class ActivePlan<T1, T2, T3> : ActivePlan
  {
    private readonly Action<T1, T2, T3> onNext;
    private readonly Action onCompleted;
    private readonly JoinObserver<T1> first;
    private readonly JoinObserver<T2> second;
    private readonly JoinObserver<T3> third;

    internal ActivePlan(
      JoinObserver<T1> first,
      JoinObserver<T2> second,
      JoinObserver<T3> third,
      Action<T1, T2, T3> onNext,
      Action onCompleted)
    {
      this.onNext = onNext;
      this.onCompleted = onCompleted;
      this.first = first;
      this.second = second;
      this.third = third;
      this.AddJoinObserver((IJoinObserver) first);
      this.AddJoinObserver((IJoinObserver) second);
      this.AddJoinObserver((IJoinObserver) third);
    }

    internal override void Match()
    {
      if (this.first.Queue.Count <= 0 || this.second.Queue.Count <= 0 || this.third.Queue.Count <= 0)
        return;
      Notification<T1> notification1 = this.first.Queue.Peek();
      Notification<T2> notification2 = this.second.Queue.Peek();
      Notification<T3> notification3 = this.third.Queue.Peek();
      if (notification1.Kind == NotificationKind.OnCompleted || notification2.Kind == NotificationKind.OnCompleted || notification3.Kind == NotificationKind.OnCompleted)
      {
        this.onCompleted();
      }
      else
      {
        this.Dequeue();
        this.onNext(notification1.Value, notification2.Value, notification3.Value);
      }
    }
  }
}

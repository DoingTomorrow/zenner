// Decompiled with JetBrains decompiler
// Type: System.Reactive.Joins.ActivePlan`7
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Joins
{
  internal class ActivePlan<T1, T2, T3, T4, T5, T6, T7> : ActivePlan
  {
    private readonly Action<T1, T2, T3, T4, T5, T6, T7> onNext;
    private readonly Action onCompleted;
    private readonly JoinObserver<T1> first;
    private readonly JoinObserver<T2> second;
    private readonly JoinObserver<T3> third;
    private readonly JoinObserver<T4> fourth;
    private readonly JoinObserver<T5> fifth;
    private readonly JoinObserver<T6> sixth;
    private readonly JoinObserver<T7> seventh;

    internal ActivePlan(
      JoinObserver<T1> first,
      JoinObserver<T2> second,
      JoinObserver<T3> third,
      JoinObserver<T4> fourth,
      JoinObserver<T5> fifth,
      JoinObserver<T6> sixth,
      JoinObserver<T7> seventh,
      Action<T1, T2, T3, T4, T5, T6, T7> onNext,
      Action onCompleted)
    {
      this.onNext = onNext;
      this.onCompleted = onCompleted;
      this.first = first;
      this.second = second;
      this.third = third;
      this.fourth = fourth;
      this.fifth = fifth;
      this.sixth = sixth;
      this.seventh = seventh;
      this.AddJoinObserver((IJoinObserver) first);
      this.AddJoinObserver((IJoinObserver) second);
      this.AddJoinObserver((IJoinObserver) third);
      this.AddJoinObserver((IJoinObserver) fourth);
      this.AddJoinObserver((IJoinObserver) fifth);
      this.AddJoinObserver((IJoinObserver) sixth);
      this.AddJoinObserver((IJoinObserver) seventh);
    }

    internal override void Match()
    {
      if (this.first.Queue.Count <= 0 || this.second.Queue.Count <= 0 || this.third.Queue.Count <= 0 || this.fourth.Queue.Count <= 0 || this.fifth.Queue.Count <= 0 || this.sixth.Queue.Count <= 0 || this.seventh.Queue.Count <= 0)
        return;
      Notification<T1> notification1 = this.first.Queue.Peek();
      Notification<T2> notification2 = this.second.Queue.Peek();
      Notification<T3> notification3 = this.third.Queue.Peek();
      Notification<T4> notification4 = this.fourth.Queue.Peek();
      Notification<T5> notification5 = this.fifth.Queue.Peek();
      Notification<T6> notification6 = this.sixth.Queue.Peek();
      Notification<T7> notification7 = this.seventh.Queue.Peek();
      if (notification1.Kind == NotificationKind.OnCompleted || notification2.Kind == NotificationKind.OnCompleted || notification3.Kind == NotificationKind.OnCompleted || notification4.Kind == NotificationKind.OnCompleted || notification5.Kind == NotificationKind.OnCompleted || notification6.Kind == NotificationKind.OnCompleted || notification7.Kind == NotificationKind.OnCompleted)
      {
        this.onCompleted();
      }
      else
      {
        this.Dequeue();
        this.onNext(notification1.Value, notification2.Value, notification3.Value, notification4.Value, notification5.Value, notification6.Value, notification7.Value);
      }
    }
  }
}

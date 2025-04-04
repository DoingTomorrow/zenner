// Decompiled with JetBrains decompiler
// Type: System.Reactive.Joins.ActivePlan`15
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Joins
{
  internal class ActivePlan<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : 
    ActivePlan
  {
    private readonly Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> onNext;
    private readonly Action onCompleted;
    private readonly JoinObserver<T1> first;
    private readonly JoinObserver<T2> second;
    private readonly JoinObserver<T3> third;
    private readonly JoinObserver<T4> fourth;
    private readonly JoinObserver<T5> fifth;
    private readonly JoinObserver<T6> sixth;
    private readonly JoinObserver<T7> seventh;
    private readonly JoinObserver<T8> eighth;
    private readonly JoinObserver<T9> ninth;
    private readonly JoinObserver<T10> tenth;
    private readonly JoinObserver<T11> eleventh;
    private readonly JoinObserver<T12> twelfth;
    private readonly JoinObserver<T13> thirteenth;
    private readonly JoinObserver<T14> fourteenth;
    private readonly JoinObserver<T15> fifteenth;

    internal ActivePlan(
      JoinObserver<T1> first,
      JoinObserver<T2> second,
      JoinObserver<T3> third,
      JoinObserver<T4> fourth,
      JoinObserver<T5> fifth,
      JoinObserver<T6> sixth,
      JoinObserver<T7> seventh,
      JoinObserver<T8> eighth,
      JoinObserver<T9> ninth,
      JoinObserver<T10> tenth,
      JoinObserver<T11> eleventh,
      JoinObserver<T12> twelfth,
      JoinObserver<T13> thirteenth,
      JoinObserver<T14> fourteenth,
      JoinObserver<T15> fifteenth,
      Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> onNext,
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
      this.eighth = eighth;
      this.ninth = ninth;
      this.tenth = tenth;
      this.eleventh = eleventh;
      this.twelfth = twelfth;
      this.thirteenth = thirteenth;
      this.fourteenth = fourteenth;
      this.fifteenth = fifteenth;
      this.AddJoinObserver((IJoinObserver) first);
      this.AddJoinObserver((IJoinObserver) second);
      this.AddJoinObserver((IJoinObserver) third);
      this.AddJoinObserver((IJoinObserver) fourth);
      this.AddJoinObserver((IJoinObserver) fifth);
      this.AddJoinObserver((IJoinObserver) sixth);
      this.AddJoinObserver((IJoinObserver) seventh);
      this.AddJoinObserver((IJoinObserver) eighth);
      this.AddJoinObserver((IJoinObserver) ninth);
      this.AddJoinObserver((IJoinObserver) tenth);
      this.AddJoinObserver((IJoinObserver) eleventh);
      this.AddJoinObserver((IJoinObserver) twelfth);
      this.AddJoinObserver((IJoinObserver) thirteenth);
      this.AddJoinObserver((IJoinObserver) fourteenth);
      this.AddJoinObserver((IJoinObserver) fifteenth);
    }

    internal override void Match()
    {
      if (this.first.Queue.Count <= 0 || this.second.Queue.Count <= 0 || this.third.Queue.Count <= 0 || this.fourth.Queue.Count <= 0 || this.fifth.Queue.Count <= 0 || this.sixth.Queue.Count <= 0 || this.seventh.Queue.Count <= 0 || this.eighth.Queue.Count <= 0 || this.ninth.Queue.Count <= 0 || this.tenth.Queue.Count <= 0 || this.eleventh.Queue.Count <= 0 || this.twelfth.Queue.Count <= 0 || this.thirteenth.Queue.Count <= 0 || this.fourteenth.Queue.Count <= 0 || this.fifteenth.Queue.Count <= 0)
        return;
      Notification<T1> notification1 = this.first.Queue.Peek();
      Notification<T2> notification2 = this.second.Queue.Peek();
      Notification<T3> notification3 = this.third.Queue.Peek();
      Notification<T4> notification4 = this.fourth.Queue.Peek();
      Notification<T5> notification5 = this.fifth.Queue.Peek();
      Notification<T6> notification6 = this.sixth.Queue.Peek();
      Notification<T7> notification7 = this.seventh.Queue.Peek();
      Notification<T8> notification8 = this.eighth.Queue.Peek();
      Notification<T9> notification9 = this.ninth.Queue.Peek();
      Notification<T10> notification10 = this.tenth.Queue.Peek();
      Notification<T11> notification11 = this.eleventh.Queue.Peek();
      Notification<T12> notification12 = this.twelfth.Queue.Peek();
      Notification<T13> notification13 = this.thirteenth.Queue.Peek();
      Notification<T14> notification14 = this.fourteenth.Queue.Peek();
      Notification<T15> notification15 = this.fifteenth.Queue.Peek();
      if (notification1.Kind == NotificationKind.OnCompleted || notification2.Kind == NotificationKind.OnCompleted || notification3.Kind == NotificationKind.OnCompleted || notification4.Kind == NotificationKind.OnCompleted || notification5.Kind == NotificationKind.OnCompleted || notification6.Kind == NotificationKind.OnCompleted || notification7.Kind == NotificationKind.OnCompleted || notification8.Kind == NotificationKind.OnCompleted || notification9.Kind == NotificationKind.OnCompleted || notification10.Kind == NotificationKind.OnCompleted || notification11.Kind == NotificationKind.OnCompleted || notification12.Kind == NotificationKind.OnCompleted || notification13.Kind == NotificationKind.OnCompleted || notification14.Kind == NotificationKind.OnCompleted || notification15.Kind == NotificationKind.OnCompleted)
      {
        this.onCompleted();
      }
      else
      {
        this.Dequeue();
        this.onNext(notification1.Value, notification2.Value, notification3.Value, notification4.Value, notification5.Value, notification6.Value, notification7.Value, notification8.Value, notification9.Value, notification10.Value, notification11.Value, notification12.Value, notification13.Value, notification14.Value, notification15.Value);
      }
    }
  }
}

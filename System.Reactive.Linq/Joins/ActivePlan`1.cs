// Decompiled with JetBrains decompiler
// Type: System.Reactive.Joins.ActivePlan`1
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

#nullable disable
namespace System.Reactive.Joins
{
  internal class ActivePlan<T1> : ActivePlan
  {
    private readonly Action<T1> onNext;
    private readonly Action onCompleted;
    private readonly JoinObserver<T1> first;

    internal ActivePlan(JoinObserver<T1> first, Action<T1> onNext, Action onCompleted)
    {
      this.onNext = onNext;
      this.onCompleted = onCompleted;
      this.first = first;
      this.AddJoinObserver((IJoinObserver) first);
    }

    internal override void Match()
    {
      if (this.first.Queue.Count <= 0)
        return;
      Notification<T1> notification = this.first.Queue.Peek();
      if (notification.Kind == NotificationKind.OnCompleted)
      {
        this.onCompleted();
      }
      else
      {
        this.Dequeue();
        this.onNext(notification.Value);
      }
    }
  }
}

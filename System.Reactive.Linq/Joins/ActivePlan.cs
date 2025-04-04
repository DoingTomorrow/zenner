// Decompiled with JetBrains decompiler
// Type: System.Reactive.Joins.ActivePlan
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Collections.Generic;

#nullable disable
namespace System.Reactive.Joins
{
  internal abstract class ActivePlan
  {
    private Dictionary<IJoinObserver, IJoinObserver> joinObservers = new Dictionary<IJoinObserver, IJoinObserver>();

    internal abstract void Match();

    protected void AddJoinObserver(IJoinObserver joinObserver)
    {
      this.joinObservers.Add(joinObserver, joinObserver);
    }

    protected void Dequeue()
    {
      foreach (IJoinObserver joinObserver in this.joinObservers.Values)
        joinObserver.Dequeue();
    }
  }
}

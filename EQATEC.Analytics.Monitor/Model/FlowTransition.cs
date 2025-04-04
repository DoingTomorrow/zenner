// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Model.FlowTransition
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;

#nullable disable
namespace EQATEC.Analytics.Monitor.Model
{
  internal class FlowTransition
  {
    public Waypoint LastWayPoint { get; private set; }

    public Waypoint ThisWayPoint { get; private set; }

    public long TotalTransitionTimeInMilliseconds { get; private set; }

    public int TotalTransitions { get; private set; }

    public int TransitionsSynced { get; set; }

    public bool Synced => this.TotalTransitions == this.TransitionsSynced;

    public FlowTransition(Waypoint lastWayPoint, Waypoint thisWayPoint)
    {
      this.LastWayPoint = lastWayPoint;
      this.ThisWayPoint = thisWayPoint;
    }

    public FlowTransition(
      Waypoint lastWayPoint,
      Waypoint thisWayPoint,
      int count,
      int sync,
      long timeInMilliseconds)
      : this(lastWayPoint, thisWayPoint)
    {
      this.TotalTransitions = count;
      this.TransitionsSynced = sync;
      this.TotalTransitionTimeInMilliseconds = timeInMilliseconds;
    }

    public void AddTransition(TimeSpan timeSpanForTransition)
    {
      ++this.TotalTransitions;
      this.TotalTransitionTimeInMilliseconds += (long) timeSpanForTransition.TotalMilliseconds;
    }

    internal bool Equals(FlowTransition other)
    {
      return other != null && this.LastWayPoint.Equals(other.LastWayPoint) && this.ThisWayPoint.Equals(other.ThisWayPoint) && this.TotalTransitions == other.TotalTransitions && this.TotalTransitionTimeInMilliseconds == other.TotalTransitionTimeInMilliseconds && this.TransitionsSynced == other.TransitionsSynced;
    }

    internal FlowTransition Copy()
    {
      return new FlowTransition(this.LastWayPoint, this.ThisWayPoint, this.TotalTransitions, this.TransitionsSynced, this.TotalTransitionTimeInMilliseconds);
    }
  }
}

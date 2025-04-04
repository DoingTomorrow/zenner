// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Model.Flow
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using EQATEC.Analytics.Monitor.Policy;
using System;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace EQATEC.Analytics.Monitor.Model
{
  internal class Flow
  {
    private Waypoint m_lastWaypoint;
    private TimeSpan m_lastWaypointTime = TimeSpan.Zero;
    private static int m_waypointIds;
    private readonly SettingsRestrictions m_restrictions;
    private readonly CircularQueue<WaypointWithRuntime> m_waypointQueue;

    public string FlowName { get; private set; }

    internal Dictionary<string, Waypoint> Waypoints { get; private set; }

    internal Dictionary<string, FlowTransition> Transitions { get; private set; }

    internal List<TrackedGoal> Goals { get; private set; }

    internal bool IsSynced
    {
      get
      {
        foreach (KeyValuePair<string, FlowTransition> transition in this.Transitions)
        {
          if (!transition.Value.Synced)
            return false;
        }
        return true;
      }
    }

    public Flow(string name, TimeSpan startRuntime)
      : this(name, startRuntime, (SettingsRestrictions) null)
    {
    }

    public Flow(string name, TimeSpan startRuntime, SettingsRestrictions restrictions)
    {
      this.FlowName = name;
      this.m_lastWaypointTime = startRuntime;
      this.m_restrictions = restrictions;
      this.Transitions = new Dictionary<string, FlowTransition>();
      this.Waypoints = new Dictionary<string, Waypoint>();
      this.Goals = new List<TrackedGoal>();
      this.m_waypointQueue = new CircularQueue<WaypointWithRuntime>(restrictions == null ? 5 : restrictions.MaxFlowQueueLength.Value);
    }

    internal void AddWayPoint(string wayPoint, TimeSpan runtime)
    {
      if (this.m_lastWaypoint == null && !this.Waypoints.TryGetValue("", out this.m_lastWaypoint))
      {
        this.m_lastWaypoint = new Waypoint("", Interlocked.Increment(ref Flow.m_waypointIds));
        this.Waypoints.Add(this.m_lastWaypoint.Name, this.m_lastWaypoint);
      }
      Waypoint waypoint;
      if (!this.Waypoints.TryGetValue(wayPoint, out waypoint))
      {
        waypoint = new Waypoint(wayPoint, Interlocked.Increment(ref Flow.m_waypointIds));
        this.Waypoints.Add(wayPoint, waypoint);
      }
      string transitionKey = Flow.GetTransitionKey(this.m_lastWaypoint, waypoint);
      FlowTransition flowTransition;
      if (!this.Transitions.TryGetValue(transitionKey, out flowTransition) && (this.m_restrictions == null || this.Transitions.Count < this.m_restrictions.MaxTransitionsInFlow.Value))
      {
        flowTransition = new FlowTransition(this.m_lastWaypoint, waypoint);
        this.Transitions.Add(transitionKey, flowTransition);
      }
      TimeSpan timeSpanForTransition = TimeSpan.FromMilliseconds(Math.Max(0.0, runtime.Subtract(this.m_lastWaypointTime).TotalMilliseconds));
      this.m_lastWaypoint = waypoint;
      this.m_lastWaypointTime = runtime;
      flowTransition?.AddTransition(timeSpanForTransition);
      this.m_waypointQueue.Add(new WaypointWithRuntime(waypoint, runtime));
    }

    internal static string GetTransitionKey(Waypoint lastWayPoint, Waypoint thisWaypoint)
    {
      return string.Format("{0}_{1}", lastWayPoint == null ? (object) "" : (object) lastWayPoint.Name, thisWaypoint == null ? (object) "" : (object) thisWaypoint.Name);
    }

    internal void AddGoal(string name, TimeSpan runtime)
    {
      List<WaypointWithRuntime> waypointWithRuntimeList = new List<WaypointWithRuntime>();
      TimeSpan timeSpan = runtime;
      foreach (WaypointWithRuntime waypointWithRuntime in this.m_waypointQueue.Enumerate())
      {
        long val2 = timeSpan.Ticks - waypointWithRuntime.Runtime.Ticks;
        waypointWithRuntimeList.Add(new WaypointWithRuntime(waypointWithRuntime.Waypoint, new TimeSpan(Math.Max(0L, val2))));
        timeSpan = waypointWithRuntime.Runtime;
      }
      if (waypointWithRuntimeList.Count <= 0 || this.m_restrictions == null || this.Goals.Count >= this.m_restrictions.MaxGoalsInFlow.Value)
        return;
      this.Goals.Add(new TrackedGoal(name, waypointWithRuntimeList.ToArray()));
    }

    internal void Reset(TimeSpan runtime)
    {
      this.m_lastWaypoint = (Waypoint) null;
      this.m_lastWaypointTime = runtime;
      this.m_waypointQueue.Reset();
    }

    internal bool Equals(Flow other)
    {
      return other != null && other.FlowName.Equals(this.FlowName) && this.Matches(other.Transitions);
    }

    private bool Matches(Dictionary<string, FlowTransition> transitions)
    {
      if (this.Transitions.Count != transitions.Count)
        return false;
      foreach (KeyValuePair<string, FlowTransition> transition in this.Transitions)
      {
        FlowTransition other;
        if (!transitions.TryGetValue(transition.Key, out other) || !transition.Value.Equals(other))
          return false;
      }
      return true;
    }

    internal Flow Copy()
    {
      Flow flow = new Flow(this.FlowName, this.m_lastWaypointTime);
      flow.m_lastWaypoint = this.m_lastWaypoint;
      foreach (TrackedGoal goal in this.Goals)
        flow.Goals.Add(goal);
      foreach (KeyValuePair<string, FlowTransition> transition in this.Transitions)
        flow.Transitions.Add(transition.Key, transition.Value.Copy());
      return flow;
    }

    internal void SubtractCopy(Flow flow)
    {
      foreach (KeyValuePair<string, FlowTransition> transition in flow.Transitions)
      {
        FlowTransition flowTransition;
        if (this.Transitions.TryGetValue(transition.Key, out flowTransition))
          flowTransition.TransitionsSynced = transition.Value.TotalTransitions;
      }
      foreach (TrackedGoal goal in flow.Goals)
        this.Goals.Remove(goal);
    }
  }
}

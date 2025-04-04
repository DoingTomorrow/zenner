// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Model.TrackedGoal
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;

#nullable disable
namespace EQATEC.Analytics.Monitor.Model
{
  internal class TrackedGoal
  {
    public Guid ID { get; set; }

    public string Name { get; set; }

    public WaypointWithRuntime[] Waypoints { get; set; }

    public TrackedGoal(string name, WaypointWithRuntime[] waypoints)
    {
      this.ID = Guid.NewGuid();
      this.Name = name;
      this.Waypoints = waypoints;
    }

    public TrackedGoal Copy()
    {
      return new TrackedGoal(this.Name, this.Waypoints)
      {
        ID = this.ID
      };
    }

    public bool Equals(TrackedGoal other) => other != null && this.ID == other.ID;
  }
}

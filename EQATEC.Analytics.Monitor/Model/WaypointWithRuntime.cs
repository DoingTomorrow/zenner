// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Model.WaypointWithRuntime
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;

#nullable disable
namespace EQATEC.Analytics.Monitor.Model
{
  internal class WaypointWithRuntime
  {
    public TimeSpan Runtime { get; private set; }

    public Waypoint Waypoint { get; private set; }

    public WaypointWithRuntime(Waypoint waypoint, TimeSpan runtime)
    {
      this.Runtime = runtime;
      this.Waypoint = waypoint;
    }

    public override string ToString()
    {
      return this.Waypoint.ToString() + "(" + (object) this.Runtime.TotalMilliseconds + "ms)";
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Model.Waypoint
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

#nullable disable
namespace EQATEC.Analytics.Monitor.Model
{
  internal class Waypoint
  {
    public string Name { get; private set; }

    public int ID { get; private set; }

    internal Waypoint(string name, int id)
    {
      this.Name = name;
      this.ID = id;
    }

    public bool Equals(Waypoint other)
    {
      return other != null && this.Name == other.Name && this.ID == other.ID;
    }

    public override string ToString() => this.Name;
  }
}

// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Policy.MonitorPolicy
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;

#nullable disable
namespace EQATEC.Analytics.Monitor.Policy
{
  internal class MonitorPolicy : IEquatable<MonitorPolicy>
  {
    internal DataTypeRestrictions DataTypeRestrictions { get; private set; }

    internal SettingsRestrictions SettingsRestrictions { get; private set; }

    internal MonitorInfo Info { get; private set; }

    internal RuntimeStatus RuntimeStatus { get; private set; }

    internal BlockingRestriction TransmissionBlocking { get; private set; }

    internal MonitorPolicy()
    {
      this.DataTypeRestrictions = new DataTypeRestrictions();
      this.SettingsRestrictions = new SettingsRestrictions();
      this.Info = new MonitorInfo();
      this.RuntimeStatus = new RuntimeStatus();
      this.TransmissionBlocking = new BlockingRestriction();
    }

    internal virtual void RaiseChangedEvent() => this.Changed((object) this, EventArgs.Empty);

    public bool Equals(MonitorPolicy other)
    {
      return other != null && this.DataTypeRestrictions.Equals(other.DataTypeRestrictions) && this.SettingsRestrictions.Equals(other.SettingsRestrictions) && this.Info.Equals(other.Info) && this.TransmissionBlocking.Equals(other.TransmissionBlocking);
    }

    public MonitorPolicy Copy()
    {
      return new MonitorPolicy()
      {
        Info = this.Info.Copy(),
        RuntimeStatus = this.RuntimeStatus.Copy(),
        DataTypeRestrictions = this.DataTypeRestrictions.Copy(),
        SettingsRestrictions = this.SettingsRestrictions.Copy(),
        TransmissionBlocking = this.TransmissionBlocking.Copy()
      };
    }

    internal event EventHandler Changed = (param0, param1) => { };
  }
}

// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Policy.DataTypeRestrictions
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;

#nullable disable
namespace EQATEC.Analytics.Monitor.Policy
{
  internal class DataTypeRestrictions : IEquatable<DataTypeRestrictions>
  {
    internal int RestrictionsVersion { get; set; }

    internal BlockingRestriction Sessions { get; private set; }

    internal BlockingRestriction FeatureUsages { get; private set; }

    internal BlockingRestriction FeatureValues { get; private set; }

    internal BlockingRestriction FeatureTiming { get; private set; }

    internal BlockingRestriction Exceptions { get; private set; }

    internal BlockingRestriction Flows { get; private set; }

    internal BlockingRestriction Goals { get; private set; }

    internal DataTypeRestrictions()
    {
      this.RestrictionsVersion = 0;
      this.Sessions = new BlockingRestriction();
      this.FeatureUsages = new BlockingRestriction();
      this.FeatureValues = new BlockingRestriction();
      this.FeatureTiming = new BlockingRestriction();
      this.Exceptions = new BlockingRestriction();
      this.Flows = new BlockingRestriction();
      this.Goals = new BlockingRestriction();
    }

    public bool Equals(DataTypeRestrictions other)
    {
      return other != null && this.RestrictionsVersion == other.RestrictionsVersion && this.Sessions.Equals(other.Sessions) && this.FeatureUsages.Equals(other.FeatureUsages) && this.FeatureValues.Equals(other.FeatureValues) && this.FeatureTiming.Equals(other.FeatureTiming) && this.Exceptions.Equals(other.Exceptions) && this.Flows.Equals(other.Flows) && this.Goals.Equals(other.Goals);
    }

    public DataTypeRestrictions Copy()
    {
      return new DataTypeRestrictions()
      {
        RestrictionsVersion = this.RestrictionsVersion,
        Sessions = this.Sessions.Copy(),
        Exceptions = this.Exceptions.Copy(),
        FeatureUsages = this.FeatureUsages.Copy(),
        FeatureValues = this.FeatureValues.Copy(),
        FeatureTiming = this.FeatureTiming.Copy(),
        Flows = this.Flows.Copy(),
        Goals = this.Goals.Copy()
      };
    }
  }
}

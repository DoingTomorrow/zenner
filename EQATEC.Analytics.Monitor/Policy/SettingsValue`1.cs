// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Policy.SettingsValue`1
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

#nullable disable
namespace EQATEC.Analytics.Monitor.Policy
{
  internal class SettingsValue<T> where T : struct
  {
    internal T Default { get; private set; }

    internal T Value { get; private set; }

    internal T? ExplicitValue { get; private set; }

    internal bool IsExplicit { get; private set; }

    internal SettingsValue(T defaultValue)
    {
      this.Default = defaultValue;
      this.Value = defaultValue;
      this.ExplicitValue = new T?();
      this.IsExplicit = false;
    }

    public override int GetHashCode() => base.GetHashCode();

    public override bool Equals(object obj)
    {
      return obj is SettingsValue<T> && this.Value.Equals((object) ((SettingsValue<T>) obj).Value);
    }

    internal void SetExplictValue(T explicitValue)
    {
      this.ExplicitValue = new T?(explicitValue);
      this.Value = explicitValue;
      this.IsExplicit = true;
    }

    internal void SetValue(T value)
    {
      this.Value = value;
      this.IsExplicit = false;
    }

    internal void Reset()
    {
      if (this.ExplicitValue.HasValue)
      {
        this.Value = this.ExplicitValue.Value;
        this.IsExplicit = true;
      }
      else
      {
        this.Value = this.Default;
        this.IsExplicit = false;
      }
    }

    public override string ToString() => this.Value.ToString();
  }
}

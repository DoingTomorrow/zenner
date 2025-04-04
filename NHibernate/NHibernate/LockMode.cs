// Decompiled with JetBrains decompiler
// Type: NHibernate.LockMode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate
{
  [Serializable]
  public sealed class LockMode
  {
    private readonly int level;
    private readonly string name;
    private readonly int hashcode;
    public static LockMode None = new LockMode(0, nameof (None));
    public static LockMode Read = new LockMode(5, nameof (Read));
    public static LockMode Upgrade = new LockMode(10, nameof (Upgrade));
    public static LockMode UpgradeNoWait = new LockMode(10, nameof (UpgradeNoWait));
    public static LockMode Write = new LockMode(10, nameof (Write));
    public static readonly LockMode Force = new LockMode(15, nameof (Force));

    private LockMode(int level, string name)
    {
      this.level = level;
      this.name = name;
      this.hashcode = level * 37 ^ (name != null ? name.GetHashCode() : 0);
    }

    public override string ToString() => this.name;

    public bool GreaterThan(LockMode mode) => this.level > mode.level;

    public bool LessThan(LockMode mode) => this.level < mode.level;

    public override bool Equals(object obj) => this.Equals(obj as LockMode);

    public bool Equals(LockMode other)
    {
      if (other == null)
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return other.level == this.level && object.Equals((object) other.name, (object) this.name);
    }

    public override int GetHashCode() => this.hashcode;
  }
}

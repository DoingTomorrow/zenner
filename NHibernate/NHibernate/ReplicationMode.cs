// Decompiled with JetBrains decompiler
// Type: NHibernate.ReplicationMode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;

#nullable disable
namespace NHibernate
{
  public abstract class ReplicationMode
  {
    public static readonly ReplicationMode Exception = (ReplicationMode) new ReplicationMode.ExceptionReplicationMode("EXCEPTION");
    public static readonly ReplicationMode Ignore = (ReplicationMode) new ReplicationMode.IgnoreReplicationMode("IGNORE");
    public static readonly ReplicationMode LatestVersion = (ReplicationMode) new ReplicationMode.LatestVersionReplicationMode("LATEST_VERSION");
    public static readonly ReplicationMode Overwrite = (ReplicationMode) new ReplicationMode.OverwriteReplicationMode("OVERWRITE");
    private readonly string name;

    protected ReplicationMode(string name) => this.name = name;

    public override string ToString() => this.name;

    public abstract bool ShouldOverwriteCurrentVersion(
      object entity,
      object currentVersion,
      object newVersion,
      IVersionType versionType);

    private sealed class ExceptionReplicationMode(string name) : ReplicationMode(name)
    {
      public override bool ShouldOverwriteCurrentVersion(
        object entity,
        object currentVersion,
        object newVersion,
        IVersionType versionType)
      {
        throw new AssertionFailure("should not be called");
      }
    }

    private sealed class IgnoreReplicationMode(string name) : ReplicationMode(name)
    {
      public override bool ShouldOverwriteCurrentVersion(
        object entity,
        object currentVersion,
        object newVersion,
        IVersionType versionType)
      {
        return false;
      }
    }

    private sealed class LatestVersionReplicationMode(string name) : ReplicationMode(name)
    {
      public override bool ShouldOverwriteCurrentVersion(
        object entity,
        object currentVersion,
        object newVersion,
        IVersionType versionType)
      {
        return versionType == null || versionType.Comparator.Compare(currentVersion, newVersion) <= 0;
      }
    }

    private sealed class OverwriteReplicationMode(string name) : ReplicationMode(name)
    {
      public override bool ShouldOverwriteCurrentVersion(
        object entity,
        object currentVersion,
        object newVersion,
        IVersionType versionType)
      {
        return true;
      }
    }
  }
}

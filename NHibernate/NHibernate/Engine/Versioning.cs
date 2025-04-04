// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.Versioning
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Persister.Entity;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Engine
{
  public class Versioning
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (Versioning));

    public static object Increment(
      object version,
      IVersionType versionType,
      ISessionImplementor session)
    {
      object obj = versionType.Next(version, session);
      if (Versioning.log.IsDebugEnabled)
        Versioning.log.Debug((object) string.Format("Incrementing: {0} to {1}", (object) versionType.ToLoggableString(version, session.Factory), (object) versionType.ToLoggableString(obj, session.Factory)));
      return obj;
    }

    public static object Seed(IVersionType versionType, ISessionImplementor session)
    {
      object obj = versionType.Seed(session);
      if (Versioning.log.IsDebugEnabled)
        Versioning.log.Debug((object) ("Seeding: " + obj));
      return obj;
    }

    public static bool SeedVersion(
      object[] fields,
      int versionProperty,
      IVersionType versionType,
      bool? force,
      ISessionImplementor session)
    {
      object field = fields[versionProperty];
      if (field == null || !force.HasValue || force.Value)
      {
        fields[versionProperty] = Versioning.Seed(versionType, session);
        return true;
      }
      if (Versioning.log.IsDebugEnabled)
        Versioning.log.Debug((object) ("using initial version: " + field));
      return false;
    }

    public static void SetVersion(object[] fields, object version, IEntityPersister persister)
    {
      if (!persister.IsVersioned)
        return;
      fields[persister.VersionProperty] = version;
    }

    public static object GetVersion(object[] fields, IEntityPersister persister)
    {
      return !persister.IsVersioned ? (object) null : fields[persister.VersionProperty];
    }

    public static bool IsVersionIncrementRequired(
      int[] dirtyProperties,
      bool hasDirtyCollections,
      bool[] propertyVersionability)
    {
      if (hasDirtyCollections)
        return true;
      for (int index = 0; index < dirtyProperties.Length; ++index)
      {
        if (propertyVersionability[dirtyProperties[index]])
          return true;
      }
      return false;
    }

    public enum OptimisticLock
    {
      None = -1, // 0xFFFFFFFF
      Version = 0,
      Dirty = 1,
      All = 2,
    }
  }
}

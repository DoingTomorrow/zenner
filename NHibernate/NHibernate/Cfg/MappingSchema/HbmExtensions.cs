// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.MappingSchema.HbmExtensions
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System;

#nullable disable
namespace NHibernate.Cfg.MappingSchema
{
  public static class HbmExtensions
  {
    public static Versioning.OptimisticLock ToOptimisticLock(
      this HbmOptimisticLockMode hbmOptimisticLockMode)
    {
      switch (hbmOptimisticLockMode)
      {
        case HbmOptimisticLockMode.None:
          return Versioning.OptimisticLock.None;
        case HbmOptimisticLockMode.Version:
          return Versioning.OptimisticLock.Version;
        case HbmOptimisticLockMode.Dirty:
          return Versioning.OptimisticLock.Dirty;
        case HbmOptimisticLockMode.All:
          return Versioning.OptimisticLock.All;
        default:
          return Versioning.OptimisticLock.Version;
      }
    }

    public static string ToNullValue(this HbmUnsavedValueType unsavedValueType)
    {
      switch (unsavedValueType)
      {
        case HbmUnsavedValueType.Undefined:
          return "undefined";
        case HbmUnsavedValueType.Any:
          return "any";
        case HbmUnsavedValueType.None:
          return "none";
        default:
          throw new ArgumentOutOfRangeException(nameof (unsavedValueType));
      }
    }

    public static string ToCacheConcurrencyStrategy(this HbmCacheUsage cacheUsage)
    {
      switch (cacheUsage)
      {
        case HbmCacheUsage.ReadOnly:
          return "read-only";
        case HbmCacheUsage.ReadWrite:
          return "read-write";
        case HbmCacheUsage.NonstrictReadWrite:
          return "nonstrict-read-write";
        case HbmCacheUsage.Transactional:
          return "transactional";
        default:
          throw new ArgumentOutOfRangeException(nameof (cacheUsage));
      }
    }

    public static CacheMode? ToCacheMode(this HbmCacheMode cacheMode)
    {
      switch (cacheMode)
      {
        case HbmCacheMode.Get:
          return new CacheMode?(CacheMode.Get);
        case HbmCacheMode.Ignore:
          return new CacheMode?(CacheMode.Ignore);
        case HbmCacheMode.Normal:
          return new CacheMode?(CacheMode.Normal);
        case HbmCacheMode.Put:
          return new CacheMode?(CacheMode.Put);
        case HbmCacheMode.Refresh:
          return new CacheMode?(CacheMode.Refresh);
        default:
          throw new ArgumentOutOfRangeException(nameof (cacheMode));
      }
    }

    public static string JoinString(this string[] source)
    {
      if (source == null)
        return (string) null;
      string str = string.Join(System.Environment.NewLine, source).Trim();
      return str.Length != 0 ? str : (string) null;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.CacheInclude
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public abstract class CacheInclude
  {
    public static CacheInclude All = (CacheInclude) new CacheInclude.AllCacheInclude();
    public static CacheInclude NonLazy = (CacheInclude) new CacheInclude.NonLazyCacheInclude();

    internal abstract HbmCacheInclude ToHbm();

    public class AllCacheInclude : CacheInclude
    {
      internal override HbmCacheInclude ToHbm() => HbmCacheInclude.All;
    }

    public class NonLazyCacheInclude : CacheInclude
    {
      internal override HbmCacheInclude ToHbm() => HbmCacheInclude.NonLazy;
    }
  }
}

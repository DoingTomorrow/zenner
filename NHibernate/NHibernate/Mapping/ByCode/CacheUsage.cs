// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.CacheUsage
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public abstract class CacheUsage
  {
    public static CacheUsage ReadOnly = (CacheUsage) new CacheUsage.ReadOnlyUsage();
    public static CacheUsage ReadWrite = (CacheUsage) new CacheUsage.ReadWriteUsage();
    public static CacheUsage NonstrictReadWrite = (CacheUsage) new CacheUsage.NonstrictReadWriteUsage();
    public static CacheUsage Transactional = (CacheUsage) new CacheUsage.TransactionalUsage();

    internal abstract HbmCacheUsage ToHbm();

    private class NonstrictReadWriteUsage : CacheUsage
    {
      internal override HbmCacheUsage ToHbm() => HbmCacheUsage.NonstrictReadWrite;
    }

    private class ReadOnlyUsage : CacheUsage
    {
      internal override HbmCacheUsage ToHbm() => HbmCacheUsage.ReadOnly;
    }

    private class ReadWriteUsage : CacheUsage
    {
      internal override HbmCacheUsage ToHbm() => HbmCacheUsage.ReadWrite;
    }

    private class TransactionalUsage : CacheUsage
    {
      internal override HbmCacheUsage ToHbm() => HbmCacheUsage.Transactional;
    }
  }
}

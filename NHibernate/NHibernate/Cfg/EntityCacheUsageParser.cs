// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.EntityCacheUsageParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Cfg
{
  public static class EntityCacheUsageParser
  {
    private const string ReadOnlyXmlValue = "read-only";
    private const string ReadWriteXmlValue = "read-write";
    private const string NonstrictReadWriteXmlValue = "nonstrict-read-write";
    private const string TransactionalXmlValue = "transactional";

    public static string ToString(EntityCacheUsage value)
    {
      switch (value)
      {
        case EntityCacheUsage.Readonly:
          return "read-only";
        case EntityCacheUsage.ReadWrite:
          return "read-write";
        case EntityCacheUsage.NonStrictReadWrite:
          return "nonstrict-read-write";
        case EntityCacheUsage.Transactional:
          return "transactional";
        default:
          return string.Empty;
      }
    }

    public static EntityCacheUsage Parse(string value)
    {
      switch (value)
      {
        case "read-only":
          return EntityCacheUsage.Readonly;
        case "read-write":
          return EntityCacheUsage.ReadWrite;
        case "nonstrict-read-write":
          return EntityCacheUsage.NonStrictReadWrite;
        case "transactional":
          return EntityCacheUsage.Transactional;
        default:
          throw new HibernateConfigException(string.Format("Invalid EntityCacheUsage value:{0}", (object) value));
      }
    }
  }
}

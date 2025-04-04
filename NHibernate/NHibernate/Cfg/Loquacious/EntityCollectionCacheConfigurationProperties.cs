// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.EntityCollectionCacheConfigurationProperties
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Cfg.Loquacious
{
  internal class EntityCollectionCacheConfigurationProperties : 
    IEntityCollectionCacheConfigurationProperties
  {
    public EntityCollectionCacheConfigurationProperties()
    {
      this.Strategy = EntityCacheUsage.ReadWrite;
    }

    public EntityCacheUsage Strategy { get; set; }

    public string RegionName { get; set; }
  }
}

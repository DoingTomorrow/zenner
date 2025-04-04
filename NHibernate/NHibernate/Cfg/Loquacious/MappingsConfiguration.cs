// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.MappingsConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Cfg.Loquacious
{
  internal class MappingsConfiguration : IMappingsConfiguration
  {
    private readonly FluentSessionFactoryConfiguration fc;

    public MappingsConfiguration(FluentSessionFactoryConfiguration parent) => this.fc = parent;

    public IMappingsConfiguration UsingDefaultCatalog(string defaultCatalogName)
    {
      this.fc.Configuration.SetProperty("default_catalog", defaultCatalogName);
      return (IMappingsConfiguration) this;
    }

    public IFluentSessionFactoryConfiguration UsingDefaultSchema(string defaultSchemaName)
    {
      this.fc.Configuration.SetProperty("default_schema", defaultSchemaName);
      return (IFluentSessionFactoryConfiguration) this.fc;
    }
  }
}

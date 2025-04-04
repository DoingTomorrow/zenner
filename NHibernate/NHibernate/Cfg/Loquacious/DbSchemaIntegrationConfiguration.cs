// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Loquacious.DbSchemaIntegrationConfiguration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Cfg.Loquacious
{
  internal class DbSchemaIntegrationConfiguration : IDbSchemaIntegrationConfiguration
  {
    private readonly DbIntegrationConfiguration dbc;

    public DbSchemaIntegrationConfiguration(DbIntegrationConfiguration dbc) => this.dbc = dbc;

    public IDbIntegrationConfiguration Recreating()
    {
      this.dbc.Configuration.SetProperty("hbm2ddl.auto", SchemaAutoAction.Recreate.ToString());
      return (IDbIntegrationConfiguration) this.dbc;
    }

    public IDbIntegrationConfiguration Creating()
    {
      this.dbc.Configuration.SetProperty("hbm2ddl.auto", SchemaAutoAction.Create.ToString());
      return (IDbIntegrationConfiguration) this.dbc;
    }

    public IDbIntegrationConfiguration Updating()
    {
      this.dbc.Configuration.SetProperty("hbm2ddl.auto", SchemaAutoAction.Update.ToString());
      return (IDbIntegrationConfiguration) this.dbc;
    }

    public IDbIntegrationConfiguration Validating()
    {
      this.dbc.Configuration.SetProperty("hbm2ddl.auto", SchemaAutoAction.Validate.ToString());
      return (IDbIntegrationConfiguration) this.dbc;
    }
  }
}

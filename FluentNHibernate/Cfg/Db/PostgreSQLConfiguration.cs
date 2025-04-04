// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.PostgreSQLConfiguration
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using NHibernate.Dialect;
using NHibernate.Driver;

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class PostgreSQLConfiguration : 
    PersistenceConfiguration<PostgreSQLConfiguration, PostgreSQLConnectionStringBuilder>
  {
    protected PostgreSQLConfiguration() => this.Driver<NpgsqlDriver>();

    public static PostgreSQLConfiguration Standard
    {
      get => new PostgreSQLConfiguration().Dialect<PostgreSQLDialect>();
    }

    public static PostgreSQLConfiguration PostgreSQL81
    {
      get => new PostgreSQLConfiguration().Dialect<PostgreSQL81Dialect>();
    }

    public static PostgreSQLConfiguration PostgreSQL82
    {
      get => new PostgreSQLConfiguration().Dialect<PostgreSQL82Dialect>();
    }
  }
}

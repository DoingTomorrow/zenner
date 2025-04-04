// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.OracleDataClientConfiguration
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using NHibernate.Dialect;
using NHibernate.Driver;

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class OracleDataClientConfiguration : 
    PersistenceConfiguration<OracleDataClientConfiguration, OracleConnectionStringBuilder>
  {
    protected OracleDataClientConfiguration() => this.Driver<OracleDataClientDriver>();

    public static OracleDataClientConfiguration Oracle9
    {
      get => new OracleDataClientConfiguration().Dialect<Oracle9iDialect>();
    }

    public static OracleDataClientConfiguration Oracle10
    {
      get => new OracleDataClientConfiguration().Dialect<Oracle10gDialect>();
    }
  }
}

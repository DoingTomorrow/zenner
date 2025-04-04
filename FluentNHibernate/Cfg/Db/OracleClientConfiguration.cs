// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.OracleClientConfiguration
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using NHibernate.Dialect;
using NHibernate.Driver;

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class OracleClientConfiguration : 
    PersistenceConfiguration<OracleClientConfiguration, OracleConnectionStringBuilder>
  {
    protected OracleClientConfiguration() => this.Driver<OracleClientDriver>();

    public static OracleClientConfiguration Oracle9
    {
      get => new OracleClientConfiguration().Dialect<Oracle9iDialect>();
    }

    public static OracleClientConfiguration Oracle10
    {
      get => new OracleClientConfiguration().Dialect<Oracle10gDialect>();
    }
  }
}

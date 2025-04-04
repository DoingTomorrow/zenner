// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.OracleConfiguration
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using NHibernate.Dialect;
using NHibernate.Driver;
using System;

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  [Obsolete("Replaced by OracleDataClientConfiguration. Also, for System.Data.OracleClient, use OracleClientConfiguration.")]
  public class OracleConfiguration : 
    PersistenceConfiguration<OracleConfiguration, OracleConnectionStringBuilder>
  {
    protected OracleConfiguration() => this.Driver<OracleDataClientDriver>();

    public static OracleConfiguration Oracle8
    {
      get => new OracleConfiguration().Dialect<Oracle8iDialect>();
    }

    public static OracleConfiguration Oracle9
    {
      get => new OracleConfiguration().Dialect<Oracle9iDialect>();
    }

    public static OracleConfiguration Oracle10
    {
      get => new OracleConfiguration().Dialect<Oracle10gDialect>();
    }
  }
}

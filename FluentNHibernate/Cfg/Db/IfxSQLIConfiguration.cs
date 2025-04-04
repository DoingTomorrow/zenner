// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.IfxSQLIConfiguration
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using NHibernate.Dialect;
using NHibernate.Driver;

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class IfxSQLIConfiguration : 
    PersistenceConfiguration<IfxSQLIConfiguration, IfxSQLIConnectionStringBuilder>
  {
    protected IfxSQLIConfiguration() => this.Driver<IfxDriver>();

    public static IfxSQLIConfiguration Informix
    {
      get => new IfxSQLIConfiguration().Dialect<InformixDialect>();
    }

    public static IfxSQLIConfiguration Informix0940
    {
      get => new IfxSQLIConfiguration().Dialect<InformixDialect0940>();
    }

    public static IfxSQLIConfiguration Informix1000
    {
      get => new IfxSQLIConfiguration().Dialect<InformixDialect1000>();
    }
  }
}

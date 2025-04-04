// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.IfxOdbcConfiguration
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using NHibernate.Dialect;
using NHibernate.Driver;

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class IfxOdbcConfiguration : 
    PersistenceConfiguration<IfxOdbcConfiguration, OdbcConnectionStringBuilder>
  {
    protected IfxOdbcConfiguration() => this.Driver<OdbcDriver>();

    public static IfxOdbcConfiguration Informix
    {
      get => new IfxOdbcConfiguration().Dialect<InformixDialect>();
    }

    public static IfxOdbcConfiguration Informix0940
    {
      get => new IfxOdbcConfiguration().Dialect<InformixDialect0940>();
    }

    public static IfxOdbcConfiguration Informix1000
    {
      get => new IfxOdbcConfiguration().Dialect<InformixDialect1000>();
    }
  }
}

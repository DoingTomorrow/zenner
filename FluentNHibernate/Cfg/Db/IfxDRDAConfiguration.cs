// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.IfxDRDAConfiguration
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using NHibernate.Dialect;
using NHibernate.Driver;

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class IfxDRDAConfiguration : 
    PersistenceConfiguration<IfxDRDAConfiguration, IfxDRDAConnectionStringBuilder>
  {
    protected IfxDRDAConfiguration() => this.Driver<IfxDriver>();

    public static IfxDRDAConfiguration Informix
    {
      get => new IfxDRDAConfiguration().Dialect<InformixDialect>();
    }

    public static IfxDRDAConfiguration Informix0940
    {
      get => new IfxDRDAConfiguration().Dialect<InformixDialect0940>();
    }

    public static IfxDRDAConfiguration Informix1000
    {
      get => new IfxDRDAConfiguration().Dialect<InformixDialect1000>();
    }
  }
}

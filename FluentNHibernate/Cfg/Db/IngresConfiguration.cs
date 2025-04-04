// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.IngresConfiguration
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using NHibernate.Dialect;
using NHibernate.Driver;

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class IngresConfiguration : 
    PersistenceConfiguration<IngresConfiguration, IngresConnectionStringBuilder>
  {
    protected IngresConfiguration() => this.Driver<IngresDriver>();

    public static IngresConfiguration Standard
    {
      get => new IngresConfiguration().Dialect<IngresDialect>();
    }
  }
}

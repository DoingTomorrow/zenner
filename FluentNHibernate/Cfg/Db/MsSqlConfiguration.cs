// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.MsSqlConfiguration
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using NHibernate.Dialect;
using NHibernate.Driver;

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class MsSqlConfiguration : 
    PersistenceConfiguration<MsSqlConfiguration, MsSqlConnectionStringBuilder>
  {
    protected MsSqlConfiguration() => this.Driver<SqlClientDriver>();

    public static MsSqlConfiguration MsSql7 => new MsSqlConfiguration().Dialect<MsSql7Dialect>();

    public static MsSqlConfiguration MsSql2000
    {
      get => new MsSqlConfiguration().Dialect<MsSql2000Dialect>();
    }

    public static MsSqlConfiguration MsSql2005
    {
      get => new MsSqlConfiguration().Dialect<MsSql2005Dialect>();
    }

    public static MsSqlConfiguration MsSql2008
    {
      get => new MsSqlConfiguration().Dialect<MsSql2008Dialect>();
    }
  }
}

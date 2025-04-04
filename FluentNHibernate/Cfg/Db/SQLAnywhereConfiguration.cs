// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.SQLAnywhereConfiguration
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using NHibernate.Dialect;
using NHibernate.Driver;

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class SQLAnywhereConfiguration : 
    PersistenceConfiguration<SQLAnywhereConfiguration, SybaseSQLAnywhereConnectionStringBuilder>
  {
    protected SQLAnywhereConfiguration() => this.Driver<SybaseSQLAnywhereDriver>();

    public static SQLAnywhereConfiguration SQLAnywhere9
    {
      get => new SQLAnywhereConfiguration().Dialect<SybaseASA9Dialect>();
    }

    public static SQLAnywhereConfiguration SQLAnywhere10
    {
      get => new SQLAnywhereConfiguration().Dialect<SybaseSQLAnywhere10Dialect>();
    }

    public static SQLAnywhereConfiguration SQLAnywhere11
    {
      get => new SQLAnywhereConfiguration().Dialect<SybaseSQLAnywhere11Dialect>();
    }
  }
}

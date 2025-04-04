// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.SQLiteConfiguration
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using NHibernate.Dialect;
using NHibernate.Driver;
using System;

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class SQLiteConfiguration : PersistenceConfiguration<SQLiteConfiguration>
  {
    public static SQLiteConfiguration Standard => new SQLiteConfiguration();

    public SQLiteConfiguration()
    {
      this.Driver<SQLite20Driver>();
      this.Dialect<SQLiteDialect>();
      this.Raw("query.substitutions", "true=1;false=0");
    }

    public SQLiteConfiguration InMemory()
    {
      this.Raw("connection.release_mode", "on_close");
      return this.ConnectionString((Action<ConnectionStringBuilder>) (c => c.Is("Data Source=:memory:;Version=3;New=True;")));
    }

    public SQLiteConfiguration UsingFile(string fileName)
    {
      return this.ConnectionString((Action<ConnectionStringBuilder>) (c => c.Is(string.Format("Data Source={0};Version=3;New=True;", (object) fileName))));
    }

    public SQLiteConfiguration UsingFileWithPassword(string fileName, string password)
    {
      return this.ConnectionString((Action<ConnectionStringBuilder>) (c => c.Is(string.Format("Data Source={0};Version=3;New=True;Password={1};", (object) fileName, (object) password))));
    }
  }
}

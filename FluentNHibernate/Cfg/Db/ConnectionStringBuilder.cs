// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.ConnectionStringBuilder
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System.Configuration;

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class ConnectionStringBuilder
  {
    private string connectionString;

    public ConnectionStringBuilder FromAppSetting(string appSettingKey)
    {
      this.connectionString = ConfigurationManager.AppSettings[appSettingKey];
      this.IsDirty = true;
      return this;
    }

    public ConnectionStringBuilder FromConnectionStringWithKey(string connectionStringKey)
    {
      this.connectionString = ConfigurationManager.ConnectionStrings[connectionStringKey].ConnectionString;
      this.IsDirty = true;
      return this;
    }

    public ConnectionStringBuilder Is(string rawConnectionString)
    {
      this.connectionString = rawConnectionString;
      this.IsDirty = true;
      return this;
    }

    protected internal bool IsDirty { get; set; }

    protected internal virtual string Create() => this.connectionString;
  }
}

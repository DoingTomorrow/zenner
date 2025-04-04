// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.PostgreSQLConnectionStringBuilder
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System.Text;

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class PostgreSQLConnectionStringBuilder : ConnectionStringBuilder
  {
    private string host;
    private int port;
    private string database;
    private string username;
    private string password;

    public PostgreSQLConnectionStringBuilder Host(string host)
    {
      this.host = host;
      this.IsDirty = true;
      return this;
    }

    public PostgreSQLConnectionStringBuilder Port(int port)
    {
      this.port = port;
      this.IsDirty = true;
      return this;
    }

    public PostgreSQLConnectionStringBuilder Database(string database)
    {
      this.database = database;
      this.IsDirty = true;
      return this;
    }

    public PostgreSQLConnectionStringBuilder Username(string username)
    {
      this.username = username;
      this.IsDirty = true;
      return this;
    }

    public PostgreSQLConnectionStringBuilder Password(string password)
    {
      this.password = password;
      this.IsDirty = true;
      return this;
    }

    protected internal override string Create()
    {
      string str = base.Create();
      if (!string.IsNullOrEmpty(str))
        return str;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("User Id={0};Password={1};Host={2};Port={3};Database={4};", (object) this.username, (object) this.password, (object) this.host, (object) this.port, (object) this.database);
      return stringBuilder.ToString();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.IngresConnectionStringBuilder
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System.Text;

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class IngresConnectionStringBuilder : ConnectionStringBuilder
  {
    private string server;
    private string database;
    private string port;
    private string username;
    private string password;

    public IngresConnectionStringBuilder Server(string server)
    {
      this.server = server;
      this.IsDirty = true;
      return this;
    }

    public IngresConnectionStringBuilder Database(string database)
    {
      this.database = database;
      this.IsDirty = true;
      return this;
    }

    public IngresConnectionStringBuilder Port(string port)
    {
      this.port = port;
      this.IsDirty = true;
      return this;
    }

    public IngresConnectionStringBuilder Username(string username)
    {
      this.username = username;
      this.IsDirty = true;
      return this;
    }

    public IngresConnectionStringBuilder Password(string password)
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
      stringBuilder.AppendFormat("Host={0};Database={1};Port={2};User ID={3};PWD={4}", (object) this.server, (object) this.database, (object) this.port, (object) this.username, (object) this.password);
      return stringBuilder.ToString();
    }
  }
}

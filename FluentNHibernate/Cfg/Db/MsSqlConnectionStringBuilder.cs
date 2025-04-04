// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.MsSqlConnectionStringBuilder
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System.Data.SqlClient;

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class MsSqlConnectionStringBuilder : ConnectionStringBuilder
  {
    private string server;
    private string database;
    private string username;
    private string password;
    private bool trustedConnection;

    public MsSqlConnectionStringBuilder Server(string server)
    {
      this.server = server;
      this.IsDirty = true;
      return this;
    }

    public MsSqlConnectionStringBuilder Database(string database)
    {
      this.database = database;
      this.IsDirty = true;
      return this;
    }

    public MsSqlConnectionStringBuilder Username(string username)
    {
      this.username = username;
      this.IsDirty = true;
      return this;
    }

    public MsSqlConnectionStringBuilder Password(string password)
    {
      this.password = password;
      this.IsDirty = true;
      return this;
    }

    public MsSqlConnectionStringBuilder TrustedConnection()
    {
      this.trustedConnection = true;
      this.IsDirty = true;
      return this;
    }

    protected internal override string Create()
    {
      string connectionString = base.Create();
      if (!string.IsNullOrEmpty(connectionString))
        return connectionString;
      SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
      {
        DataSource = this.server,
        InitialCatalog = this.database,
        IntegratedSecurity = this.trustedConnection
      };
      if (!this.trustedConnection)
      {
        connectionStringBuilder.UserID = this.username;
        connectionStringBuilder.Password = this.password;
      }
      return connectionStringBuilder.ToString();
    }
  }
}

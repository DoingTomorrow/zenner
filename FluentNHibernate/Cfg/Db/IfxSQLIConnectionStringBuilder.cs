// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.IfxSQLIConnectionStringBuilder
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class IfxSQLIConnectionStringBuilder : ConnectionStringBuilder
  {
    private string clientLocale = "";
    private string database = "";
    private string databaseLocale = "";
    private bool delimident = true;
    private string host = "";
    private string maxPoolSize = "";
    private string minPoolSize = "";
    private string password = "";
    private string pooling = "";
    private string server = "";
    private string service = "";
    private string username = "";
    private string otherOptions = "";

    public IfxSQLIConnectionStringBuilder ClientLocale(string clientLocale)
    {
      this.clientLocale = clientLocale;
      this.IsDirty = true;
      return this;
    }

    public IfxSQLIConnectionStringBuilder Database(string database)
    {
      this.database = database;
      this.IsDirty = true;
      return this;
    }

    public IfxSQLIConnectionStringBuilder DatabaseLocale(string databaseLocale)
    {
      this.databaseLocale = databaseLocale;
      this.IsDirty = true;
      return this;
    }

    public IfxSQLIConnectionStringBuilder Delimident(bool delimident)
    {
      this.delimident = delimident;
      this.IsDirty = true;
      return this;
    }

    public IfxSQLIConnectionStringBuilder Host(string host)
    {
      this.host = host;
      this.IsDirty = true;
      return this;
    }

    public IfxSQLIConnectionStringBuilder MaxPoolSize(string maxPoolSize)
    {
      this.maxPoolSize = maxPoolSize;
      this.IsDirty = true;
      return this;
    }

    public IfxSQLIConnectionStringBuilder MinPoolSize(string minPoolSize)
    {
      this.minPoolSize = minPoolSize;
      this.IsDirty = true;
      return this;
    }

    public IfxSQLIConnectionStringBuilder Password(string password)
    {
      this.password = password;
      this.IsDirty = true;
      return this;
    }

    public IfxSQLIConnectionStringBuilder Pooling(string pooling)
    {
      this.pooling = pooling;
      this.IsDirty = true;
      return this;
    }

    public IfxSQLIConnectionStringBuilder Server(string server)
    {
      this.server = server;
      this.IsDirty = true;
      return this;
    }

    public IfxSQLIConnectionStringBuilder Service(string service)
    {
      this.service = service;
      this.IsDirty = true;
      return this;
    }

    public IfxSQLIConnectionStringBuilder Username(string username)
    {
      this.username = username;
      this.IsDirty = true;
      return this;
    }

    public IfxSQLIConnectionStringBuilder OtherOptions(string otherOptions)
    {
      this.otherOptions = otherOptions;
      this.IsDirty = true;
      return this;
    }

    protected internal override string Create()
    {
      string str = base.Create();
      if (!string.IsNullOrEmpty(str))
        return str;
      if (this.clientLocale.Length > 0)
        str += string.Format("Client_Locale={0};", (object) this.clientLocale);
      if (this.database.Length > 0)
        str += string.Format("DB={0};", (object) this.database);
      if (this.databaseLocale.Length > 0)
        str += string.Format("DB_LOCALE={0};", (object) this.databaseLocale);
      if (!this.delimident)
        str += "DELIMIDENT='n'";
      if (this.host.Length > 0)
        str += string.Format("Host={0};", (object) this.host);
      if (this.maxPoolSize.Length > 0)
        str += string.Format("Max Pool Size={0};", (object) this.maxPoolSize);
      if (this.minPoolSize.Length > 0)
        str += string.Format("Min Pool Size={0};", (object) this.minPoolSize);
      if (this.password.Length > 0)
        str += string.Format("PWD={0};", (object) this.password);
      if (this.pooling.Length > 0)
        str += string.Format("Pooling={0};", (object) this.pooling);
      if (this.server.Length > 0)
        str += string.Format("Server={0};", (object) this.server);
      if (this.service.Length > 0)
        str += string.Format("Service={0};", (object) this.service);
      if (this.username.Length > 0)
        str += string.Format("UID={0};", (object) this.username);
      if (this.otherOptions.Length > 0)
        str += this.otherOptions;
      return str;
    }
  }
}

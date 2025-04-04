// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.OracleConnectionStringBuilder
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class OracleConnectionStringBuilder : ConnectionStringBuilder
  {
    private string instance;
    private string otherOptions;
    private string password;
    private int port;
    private string server;
    private string username;
    private bool pooling;
    private string statementCacheSize;

    public OracleConnectionStringBuilder() => this.port = 1521;

    public OracleConnectionStringBuilder Server(string server)
    {
      this.server = server;
      this.IsDirty = true;
      return this;
    }

    public OracleConnectionStringBuilder Instance(string instance)
    {
      this.instance = instance;
      this.IsDirty = true;
      return this;
    }

    public OracleConnectionStringBuilder Username(string username)
    {
      this.username = username;
      this.IsDirty = true;
      return this;
    }

    public OracleConnectionStringBuilder Password(string password)
    {
      this.password = password;
      this.IsDirty = true;
      return this;
    }

    public OracleConnectionStringBuilder Port(int port)
    {
      this.port = port;
      this.IsDirty = true;
      return this;
    }

    public OracleConnectionStringBuilder Pooling(bool pooling)
    {
      this.pooling = pooling;
      this.IsDirty = true;
      return this;
    }

    public OracleConnectionStringBuilder StatementCacheSize(int cacheSize)
    {
      this.statementCacheSize = string.Format("Statement Cache Size={0};", (object) cacheSize);
      this.IsDirty = true;
      return this;
    }

    public OracleConnectionStringBuilder OtherOptions(string otherOptions)
    {
      this.otherOptions = string.Format("{0};", (object) otherOptions);
      this.IsDirty = true;
      return this;
    }

    protected internal override string Create()
    {
      string str = base.Create();
      if (!string.IsNullOrEmpty(str))
        return str;
      return string.Format("User Id={0};Password={1};Pooling={2};{3}{4}Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={5})(PORT={6})))(CONNECT_DATA=(SERVICE_NAME={7})))", (object) this.username, (object) this.password, (object) this.pooling, (object) this.statementCacheSize, (object) this.otherOptions, (object) this.server, (object) this.port, (object) this.instance);
    }
  }
}

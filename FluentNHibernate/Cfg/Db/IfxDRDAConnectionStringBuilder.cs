// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.IfxDRDAConnectionStringBuilder
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class IfxDRDAConnectionStringBuilder : ConnectionStringBuilder
  {
    private string authentication = "";
    private string database = "";
    private string hostVarParameter = "";
    private string isolationLevel = "";
    private string maxPoolSize = "";
    private string minPoolSize = "";
    private string password = "";
    private string pooling = "";
    private string server = "";
    private string username = "";
    private string otherOptions = "";

    public IfxDRDAConnectionStringBuilder Authentication(string authentication)
    {
      this.authentication = authentication;
      this.IsDirty = true;
      return this;
    }

    public IfxDRDAConnectionStringBuilder Database(string database)
    {
      this.database = database;
      this.IsDirty = true;
      return this;
    }

    public IfxDRDAConnectionStringBuilder HostVarParameter(string hostVarParameter)
    {
      this.hostVarParameter = hostVarParameter;
      this.IsDirty = true;
      return this;
    }

    public IfxDRDAConnectionStringBuilder IsolationLevel(string isolationLevel)
    {
      this.isolationLevel = isolationLevel;
      this.IsDirty = true;
      return this;
    }

    public IfxDRDAConnectionStringBuilder MaxPoolSize(string maxPoolSize)
    {
      this.maxPoolSize = maxPoolSize;
      this.IsDirty = true;
      return this;
    }

    public IfxDRDAConnectionStringBuilder MinPoolSize(string minPoolSize)
    {
      this.minPoolSize = minPoolSize;
      this.IsDirty = true;
      return this;
    }

    public IfxDRDAConnectionStringBuilder Password(string password)
    {
      this.password = password;
      this.IsDirty = true;
      return this;
    }

    public IfxDRDAConnectionStringBuilder Pooling(string pooling)
    {
      this.pooling = pooling;
      this.IsDirty = true;
      return this;
    }

    public IfxDRDAConnectionStringBuilder Server(string server)
    {
      this.server = server;
      this.IsDirty = true;
      return this;
    }

    public IfxDRDAConnectionStringBuilder Username(string username)
    {
      this.username = username;
      this.IsDirty = true;
      return this;
    }

    public IfxDRDAConnectionStringBuilder OtherOptions(string otherOptions)
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
      if (this.authentication.Length > 0)
        str += string.Format("Authentication={0};", (object) this.authentication);
      if (this.database.Length > 0)
        str += string.Format("Database={0};", (object) this.database);
      if (this.hostVarParameter.Length > 0)
        str += string.Format("HostVarParameter={0};", (object) this.hostVarParameter);
      if (this.isolationLevel.Length > 0)
        str += string.Format("IsolationLevel={0};", (object) this.isolationLevel);
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
      if (this.username.Length > 0)
        str += string.Format("UID={0};", (object) this.username);
      if (this.otherOptions.Length > 0)
        str += this.otherOptions;
      return str;
    }
  }
}

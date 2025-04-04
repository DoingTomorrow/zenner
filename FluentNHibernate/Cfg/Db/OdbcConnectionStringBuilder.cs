// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.OdbcConnectionStringBuilder
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class OdbcConnectionStringBuilder : ConnectionStringBuilder
  {
    private string dsn = "";
    private string username = "";
    private string password = "";
    private string otherOptions = "";

    public OdbcConnectionStringBuilder Dsn(string dsn)
    {
      this.dsn = dsn;
      this.IsDirty = true;
      return this;
    }

    public OdbcConnectionStringBuilder Username(string username)
    {
      this.username = username;
      this.IsDirty = true;
      return this;
    }

    public OdbcConnectionStringBuilder Password(string password)
    {
      this.password = password;
      this.IsDirty = true;
      return this;
    }

    public OdbcConnectionStringBuilder OtherOptions(string otherOptions)
    {
      this.otherOptions = otherOptions;
      this.IsDirty = true;
      return this;
    }

    protected internal override string Create()
    {
      string str1 = base.Create();
      if (!string.IsNullOrEmpty(str1))
        return str1;
      string str2;
      if (this.username.Length > 0)
        str2 = string.Format("Uid={0};Pwd={1};DSN={2};{3}", (object) this.username, (object) this.password, (object) this.dsn, (object) this.otherOptions);
      else
        str2 = string.Format("DSN={0};{1}", (object) this.dsn, (object) this.otherOptions);
      return str2;
    }
  }
}

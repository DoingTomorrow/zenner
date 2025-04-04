// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.JetDriverConnectionStringBuilder
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System.Text;

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class JetDriverConnectionStringBuilder : ConnectionStringBuilder
  {
    private string databaseFile;
    private string provider;
    private string username;
    private string password;

    public JetDriverConnectionStringBuilder() => this.provider = "Microsoft.Jet.OLEDB.4.0";

    public JetDriverConnectionStringBuilder Provider(string provider)
    {
      this.provider = provider;
      this.IsDirty = true;
      return this;
    }

    public JetDriverConnectionStringBuilder DatabaseFile(string databaseFile)
    {
      this.databaseFile = databaseFile;
      this.IsDirty = true;
      return this;
    }

    public JetDriverConnectionStringBuilder Username(string username)
    {
      this.username = username;
      this.IsDirty = true;
      return this;
    }

    public JetDriverConnectionStringBuilder Password(string password)
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
      stringBuilder.AppendFormat("Provider={0};Data Source={1};User Id={2};Password={3};", (object) this.provider, (object) this.databaseFile, (object) this.username, (object) this.password);
      return stringBuilder.ToString();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.SybaseSQLAnywhereConnectionStringBuilder
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System.Text;

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class SybaseSQLAnywhereConnectionStringBuilder : ConnectionStringBuilder
  {
    private string server;
    private string links;
    private string username;
    private string password;

    public SybaseSQLAnywhereConnectionStringBuilder Server(string server)
    {
      this.server = server;
      this.IsDirty = true;
      return this;
    }

    public SybaseSQLAnywhereConnectionStringBuilder Links(string links)
    {
      this.links = links;
      this.IsDirty = true;
      return this;
    }

    public SybaseSQLAnywhereConnectionStringBuilder Username(string username)
    {
      this.username = username;
      this.IsDirty = true;
      return this;
    }

    public SybaseSQLAnywhereConnectionStringBuilder Password(string password)
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
      stringBuilder.AppendFormat("uid={0};pwd={1};ServerName={2};links={3};", (object) this.username, (object) this.password, (object) this.server, (object) this.links);
      return stringBuilder.ToString();
    }
  }
}

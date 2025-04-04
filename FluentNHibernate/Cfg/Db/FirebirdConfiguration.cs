// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.FirebirdConfiguration
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using NHibernate.Dialect;
using NHibernate.Driver;

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class FirebirdConfiguration : PersistenceConfiguration<FirebirdConfiguration>
  {
    public FirebirdConfiguration()
    {
      this.Driver<FirebirdClientDriver>();
      this.Dialect<FirebirdDialect>();
      this.QuerySubstitutions("true 1, false 0, yes 1, no 0");
      this.Raw("connection.isolation", "ReadCommitted");
      this.Raw("command_timeout", "444");
      this.UseOuterJoin();
    }
  }
}

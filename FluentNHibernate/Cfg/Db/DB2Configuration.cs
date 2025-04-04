// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.Db.DB2Configuration
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using NHibernate.Dialect;
using NHibernate.Driver;

#nullable disable
namespace FluentNHibernate.Cfg.Db
{
  public class DB2Configuration : 
    PersistenceConfiguration<DB2Configuration, DB2ConnectionStringBuilder>
  {
    protected DB2Configuration() => this.Driver<DB2Driver>();

    public static DB2Configuration Standard => new DB2Configuration().Dialect<DB2Dialect>();
  }
}

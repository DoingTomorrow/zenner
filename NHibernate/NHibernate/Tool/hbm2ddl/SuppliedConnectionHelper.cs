// Decompiled with JetBrains decompiler
// Type: NHibernate.Tool.hbm2ddl.SuppliedConnectionHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Data.Common;

#nullable disable
namespace NHibernate.Tool.hbm2ddl
{
  public class SuppliedConnectionHelper : IConnectionHelper
  {
    private DbConnection connection;

    public SuppliedConnectionHelper(DbConnection connection) => this.connection = connection;

    public void Prepare()
    {
    }

    public DbConnection Connection => this.connection;

    public void Release() => this.connection = (DbConnection) null;
  }
}

// Decompiled with JetBrains decompiler
// Type: NHibernate.Tool.hbm2ddl.SuppliedConnectionProviderConnectionHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Connection;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Tool.hbm2ddl
{
  public class SuppliedConnectionProviderConnectionHelper : IConnectionHelper
  {
    private readonly IConnectionProvider provider;
    private DbConnection connection;

    public SuppliedConnectionProviderConnectionHelper(IConnectionProvider provider)
    {
      this.provider = provider;
    }

    public void Prepare() => this.connection = (DbConnection) this.provider.GetConnection();

    public DbConnection Connection => this.connection;

    public void Release()
    {
      if (this.connection == null)
        return;
      this.provider.CloseConnection((IDbConnection) this.connection);
    }
  }
}

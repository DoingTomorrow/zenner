// Decompiled with JetBrains decompiler
// Type: NHibernate.Tool.hbm2ddl.ManagedProviderConnectionHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Connection;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Tool.hbm2ddl
{
  public class ManagedProviderConnectionHelper : IConnectionHelper
  {
    private readonly IDictionary<string, string> cfgProperties;
    private IConnectionProvider connectionProvider;
    private DbConnection connection;

    public ManagedProviderConnectionHelper(IDictionary<string, string> cfgProperties)
    {
      this.cfgProperties = cfgProperties;
    }

    public void Prepare()
    {
      this.connectionProvider = ConnectionProviderFactory.NewConnectionProvider(this.cfgProperties);
      this.connection = (DbConnection) this.connectionProvider.GetConnection();
    }

    public DbConnection Connection => this.connection;

    public void Release()
    {
      if (this.connection != null)
        this.connectionProvider.CloseConnection((IDbConnection) this.connection);
      this.connection = (DbConnection) null;
    }
  }
}

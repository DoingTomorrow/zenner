// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.DbProviderFactoryDriveConnectionCommandProvider
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Driver
{
  public class DbProviderFactoryDriveConnectionCommandProvider : IDriveConnectionCommandProvider
  {
    private readonly DbProviderFactory dbProviderFactory;

    public DbProviderFactoryDriveConnectionCommandProvider(DbProviderFactory dbProviderFactory)
    {
      this.dbProviderFactory = dbProviderFactory != null ? dbProviderFactory : throw new ArgumentNullException(nameof (dbProviderFactory));
    }

    public IDbConnection CreateConnection()
    {
      return (IDbConnection) this.dbProviderFactory.CreateConnection();
    }

    public IDbCommand CreateCommand() => (IDbCommand) this.dbProviderFactory.CreateCommand();
  }
}

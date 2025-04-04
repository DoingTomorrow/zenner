// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.SqlCeProviderFactory
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

#nullable disable
namespace System.Data.SqlServerCe
{
  public sealed class SqlCeProviderFactory : DbProviderFactory, IServiceProvider
  {
    [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
    public static readonly SqlCeProviderFactory Instance = new SqlCeProviderFactory();

    public override DbCommand CreateCommand() => (DbCommand) new SqlCeCommand();

    public override DbCommandBuilder CreateCommandBuilder()
    {
      return (DbCommandBuilder) new SqlCeCommandBuilder();
    }

    public override DbConnection CreateConnection() => (DbConnection) new SqlCeConnection();

    public override DbDataAdapter CreateDataAdapter() => (DbDataAdapter) new SqlCeDataAdapter();

    public override DbParameter CreateParameter() => (DbParameter) new SqlCeParameter();

    object IServiceProvider.GetService(Type serviceType)
    {
      object service = (object) null;
      if (serviceType == ExtensionMethods.SystemDataCommonDbProviderServices_Type)
        service = ExtensionMethods.SystemDataSqlServerCeSqlCeProviderServices_Instance();
      return service;
    }
  }
}

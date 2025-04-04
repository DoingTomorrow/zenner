// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.SqlClientDriver
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.AdoNet;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System;
using System.Data;
using System.Data.SqlClient;

#nullable disable
namespace NHibernate.Driver
{
  public class SqlClientDriver : DriverBase, IEmbeddedBatcherFactoryProvider
  {
    public const int MaxSizeForAnsiClob = 2147483647;
    public const int MaxSizeForClob = 1073741823;
    public const int MaxSizeForBlob = 2147483647;
    public const int MaxSizeForLengthLimitedAnsiString = 8000;
    public const int MaxSizeForLengthLimitedString = 4000;
    public const int MaxSizeForLengthLimitedBinary = 8000;
    public const byte MaxPrecision = 28;
    public const byte MaxScale = 5;
    public const byte MaxDateTime2 = 8;
    public const byte MaxDateTimeOffset = 10;

    public override IDbConnection CreateConnection() => (IDbConnection) new SqlConnection();

    public override IDbCommand CreateCommand() => (IDbCommand) new System.Data.SqlClient.SqlCommand();

    public override bool UseNamedPrefixInSql => true;

    public override bool UseNamedPrefixInParameter => true;

    public override string NamedPrefix => "@";

    public override bool SupportsMultipleOpenReaders => false;

    public override IDbCommand GenerateCommand(
      CommandType type,
      SqlString sqlString,
      SqlType[] parameterTypes)
    {
      IDbCommand command = base.GenerateCommand(type, sqlString, parameterTypes);
      SqlClientDriver.SetParameterSizes(command.Parameters, parameterTypes);
      return command;
    }

    public static void SetParameterSizes(
      IDataParameterCollection parameters,
      SqlType[] parameterTypes)
    {
      for (int index = 0; index < parameters.Count; ++index)
        SqlClientDriver.SetVariableLengthParameterSize((IDbDataParameter) parameters[index], parameterTypes[index]);
    }

    protected static void SetVariableLengthParameterSize(IDbDataParameter dbParam, SqlType sqlType)
    {
      SqlClientDriver.SetDefaultParameterSize(dbParam, sqlType);
      if (!sqlType.PrecisionDefined)
        return;
      dbParam.Precision = sqlType.Precision;
      dbParam.Scale = sqlType.Scale;
    }

    protected static void SetDefaultParameterSize(IDbDataParameter dbParam, SqlType sqlType)
    {
      switch (dbParam.DbType)
      {
        case DbType.AnsiString:
        case DbType.AnsiStringFixedLength:
          dbParam.Size = 8000;
          break;
        case DbType.Binary:
          dbParam.Size = SqlClientDriver.IsBlob(dbParam, sqlType) ? int.MaxValue : 8000;
          break;
        case DbType.Decimal:
          dbParam.Precision = (byte) 28;
          dbParam.Scale = (byte) 5;
          break;
        case DbType.String:
        case DbType.StringFixedLength:
          dbParam.Size = SqlClientDriver.IsText(dbParam, sqlType) ? 1073741823 : 4000;
          break;
        case DbType.DateTime2:
          dbParam.Size = 8;
          break;
        case DbType.DateTimeOffset:
          dbParam.Size = 10;
          break;
      }
    }

    protected static bool IsText(IDbDataParameter dbParam, SqlType sqlType)
    {
      if (sqlType is StringClobSqlType)
        return true;
      return (DbType.String == dbParam.DbType || DbType.StringFixedLength == dbParam.DbType) && sqlType.LengthDefined && sqlType.Length > 4000;
    }

    protected static bool IsBlob(IDbDataParameter dbParam, SqlType sqlType)
    {
      if (sqlType is BinaryBlobSqlType)
        return true;
      return DbType.Binary == dbParam.DbType && sqlType.LengthDefined && sqlType.Length > 8000;
    }

    Type IEmbeddedBatcherFactoryProvider.BatcherFactoryClass
    {
      get => typeof (SqlClientBatchingBatcherFactory);
    }

    public override IResultSetsCommand GetResultSetsCommand(ISessionImplementor session)
    {
      return (IResultSetsCommand) new BasicResultSetsCommand(session);
    }

    public override bool SupportsMultipleQueries => true;
  }
}

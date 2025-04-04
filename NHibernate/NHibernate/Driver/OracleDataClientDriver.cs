// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.OracleDataClientDriver
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.AdoNet;
using NHibernate.Engine.Query;
using NHibernate.SqlTypes;
using NHibernate.Util;
using System;
using System.Data;
using System.Reflection;

#nullable disable
namespace NHibernate.Driver
{
  public class OracleDataClientDriver : ReflectionBasedDriver, IEmbeddedBatcherFactoryProvider
  {
    private const string driverAssemblyName = "Oracle.DataAccess";
    private const string connectionTypeName = "Oracle.DataAccess.Client.OracleConnection";
    private const string commandTypeName = "Oracle.DataAccess.Client.OracleCommand";
    private static readonly SqlType GuidSqlType = new SqlType(DbType.Binary, 16);
    private readonly PropertyInfo oracleCommandBindByName;
    private readonly PropertyInfo oracleDbType;
    private readonly object oracleDbTypeRefCursor;

    public OracleDataClientDriver()
      : base("Oracle.DataAccess.Client", "Oracle.DataAccess", "Oracle.DataAccess.Client.OracleConnection", "Oracle.DataAccess.Client.OracleCommand")
    {
      this.oracleCommandBindByName = ReflectHelper.TypeFromAssembly("Oracle.DataAccess.Client.OracleCommand", "Oracle.DataAccess", false).GetProperty("BindByName");
      this.oracleDbType = ReflectHelper.TypeFromAssembly("Oracle.DataAccess.Client.OracleParameter", "Oracle.DataAccess", false).GetProperty("OracleDbType");
      this.oracleDbTypeRefCursor = Enum.Parse(ReflectHelper.TypeFromAssembly("Oracle.DataAccess.Client.OracleDbType", "Oracle.DataAccess", false), "RefCursor");
    }

    public override bool UseNamedPrefixInSql => true;

    public override bool UseNamedPrefixInParameter => true;

    public override string NamedPrefix => ":";

    protected override void InitializeParameter(
      IDbDataParameter dbParam,
      string name,
      SqlType sqlType)
    {
      switch (sqlType.DbType)
      {
        case DbType.Boolean:
          base.InitializeParameter(dbParam, name, SqlTypeFactory.Int16);
          break;
        case DbType.Guid:
          base.InitializeParameter(dbParam, name, OracleDataClientDriver.GuidSqlType);
          break;
        default:
          base.InitializeParameter(dbParam, name, sqlType);
          break;
      }
    }

    protected override void OnBeforePrepare(IDbCommand command)
    {
      base.OnBeforePrepare(command);
      this.oracleCommandBindByName.SetValue((object) command, (object) true, (object[]) null);
      CallableParser.Detail detail = CallableParser.Parse(command.CommandText);
      if (!detail.IsCallable)
        return;
      command.CommandType = CommandType.StoredProcedure;
      command.CommandText = detail.FunctionName;
      this.oracleCommandBindByName.SetValue((object) command, (object) false, (object[]) null);
      IDbDataParameter parameter = command.CreateParameter();
      this.oracleDbType.SetValue((object) parameter, this.oracleDbTypeRefCursor, (object[]) null);
      parameter.Direction = detail.HasReturn ? ParameterDirection.ReturnValue : ParameterDirection.Output;
      command.Parameters.Insert(0, (object) parameter);
    }

    Type IEmbeddedBatcherFactoryProvider.BatcherFactoryClass
    {
      get => typeof (OracleDataClientBatchingBatcherFactory);
    }
  }
}

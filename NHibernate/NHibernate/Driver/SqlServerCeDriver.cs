// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.SqlServerCeDriver
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Util;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

#nullable disable
namespace NHibernate.Driver
{
  public class SqlServerCeDriver : ReflectionBasedDriver
  {
    private bool prepareSql;
    private PropertyInfo dbParamSqlDbTypeProperty;

    public SqlServerCeDriver()
      : base("System.Data.SqlServerCe", "System.Data.SqlServerCe.SqlCeConnection", "System.Data.SqlServerCe.SqlCeCommand")
    {
    }

    public override void Configure(IDictionary<string, string> settings)
    {
      base.Configure(settings);
      this.prepareSql = PropertiesHelper.GetBoolean("prepare_sql", settings, false);
      using (IDbCommand command = this.CreateCommand())
        this.dbParamSqlDbTypeProperty = command.CreateParameter().GetType().GetProperty("SqlDbType");
    }

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
      if (this.prepareSql)
        SqlClientDriver.SetParameterSizes(command.Parameters, parameterTypes);
      return command;
    }

    public override IResultSetsCommand GetResultSetsCommand(ISessionImplementor session)
    {
      return (IResultSetsCommand) new BasicResultSetsCommand(session);
    }

    public override bool SupportsMultipleQueries => true;

    protected override void InitializeParameter(
      IDbDataParameter dbParam,
      string name,
      SqlType sqlType)
    {
      base.InitializeParameter(dbParam, name, sqlType);
      this.AdjustDbParamTypeForLargeObjects(dbParam, sqlType);
    }

    private void AdjustDbParamTypeForLargeObjects(IDbDataParameter dbParam, SqlType sqlType)
    {
      switch (sqlType)
      {
        case BinaryBlobSqlType _:
          this.dbParamSqlDbTypeProperty.SetValue((object) dbParam, (object) SqlDbType.Image, (object[]) null);
          break;
        case StringClobSqlType _:
          this.dbParamSqlDbTypeProperty.SetValue((object) dbParam, (object) SqlDbType.NText, (object[]) null);
          break;
      }
    }
  }
}

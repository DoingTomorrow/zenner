// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.OracleClientDriver
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine.Query;
using NHibernate.SqlTypes;
using System;
using System.Data;

#nullable disable
namespace NHibernate.Driver
{
  public class OracleClientDriver : ReflectionBasedDriver
  {
    private static readonly SqlType GuidSqlType = new SqlType(DbType.Binary, 16);

    public OracleClientDriver()
      : base("System.Data.OracleClient", "System.Data.OracleClient, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", "System.Data.OracleClient.OracleConnection", "System.Data.OracleClient.OracleCommand")
    {
    }

    public override bool UseNamedPrefixInSql => true;

    public override bool UseNamedPrefixInParameter => true;

    public override string NamedPrefix => ":";

    protected override void InitializeParameter(
      IDbDataParameter dbParam,
      string name,
      SqlType sqlType)
    {
      if (sqlType.DbType == DbType.Guid)
        base.InitializeParameter(dbParam, name, OracleClientDriver.GuidSqlType);
      else
        base.InitializeParameter(dbParam, name, sqlType);
    }

    protected override void OnBeforePrepare(IDbCommand command)
    {
      base.OnBeforePrepare(command);
      if (CallableParser.Parse(command.CommandText).IsCallable)
        throw new NotImplementedException(this.GetType().Name + " does not support CallableStatement syntax (stored procedures). Consider using OracleDataClientDriver instead.");
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.OdbcDriver
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System.Data;
using System.Data.Odbc;

#nullable disable
namespace NHibernate.Driver
{
  public class OdbcDriver : DriverBase
  {
    public override IDbConnection CreateConnection() => (IDbConnection) new OdbcConnection();

    public override IDbCommand CreateCommand() => (IDbCommand) new OdbcCommand();

    public override bool UseNamedPrefixInSql => false;

    public override bool UseNamedPrefixInParameter => false;

    public override string NamedPrefix => string.Empty;

    protected static void SetVariableLengthParameterSize(IDbDataParameter dbParam, SqlType sqlType)
    {
      if (sqlType.LengthDefined)
        dbParam.Size = sqlType.Length;
      if (!sqlType.PrecisionDefined)
        return;
      dbParam.Precision = sqlType.Precision;
      dbParam.Scale = sqlType.Scale;
    }

    public static void SetParameterSizes(
      IDataParameterCollection parameters,
      SqlType[] parameterTypes)
    {
      for (int index = 0; index < parameters.Count; ++index)
        OdbcDriver.SetVariableLengthParameterSize((IDbDataParameter) parameters[index], parameterTypes[index]);
    }

    public override IDbCommand GenerateCommand(
      CommandType type,
      SqlString sqlString,
      SqlType[] parameterTypes)
    {
      IDbCommand command = base.GenerateCommand(type, sqlString, parameterTypes);
      OdbcDriver.SetParameterSizes(command.Parameters, parameterTypes);
      return command;
    }
  }
}

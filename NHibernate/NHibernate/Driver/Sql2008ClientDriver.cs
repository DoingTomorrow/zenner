// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.Sql2008ClientDriver
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlTypes;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

#nullable disable
namespace NHibernate.Driver
{
  public class Sql2008ClientDriver : SqlClientDriver
  {
    protected override void InitializeParameter(
      IDbDataParameter dbParam,
      string name,
      SqlType sqlType)
    {
      base.InitializeParameter(dbParam, name, sqlType);
      if (sqlType.DbType != DbType.Time)
        return;
      ((SqlParameter) dbParam).SqlDbType = SqlDbType.Time;
    }

    public override void AdjustCommand(IDbCommand command)
    {
      foreach (SqlParameter sqlParameter in command.Parameters.Cast<SqlParameter>().Where<SqlParameter>((System.Func<SqlParameter, bool>) (x => x.SqlDbType == SqlDbType.Time && x.Value is DateTime)))
      {
        DateTime dateTime = (DateTime) sqlParameter.Value;
        sqlParameter.Value = (object) dateTime.TimeOfDay;
      }
    }
  }
}

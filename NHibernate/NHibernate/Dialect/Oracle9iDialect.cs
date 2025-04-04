// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Oracle9iDialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System.Data;

#nullable disable
namespace NHibernate.Dialect
{
  public class Oracle9iDialect : Oracle8iDialect
  {
    public override string CurrentTimestampSelectString => "select systimestamp from dual";

    public override string CurrentTimestampSQLFunctionName => "current_timestamp";

    protected override void RegisterDateTimeTypeMappings()
    {
      this.RegisterColumnType(DbType.Date, "DATE");
      this.RegisterColumnType(DbType.DateTime, "TIMESTAMP(4)");
      this.RegisterColumnType(DbType.Time, "TIMESTAMP(4)");
    }

    public override long TimestampResolutionInTicks => 1000;

    public override string GetSelectClauseNullString(SqlType sqlType)
    {
      return this.GetBasicSelectClauseNullString(sqlType);
    }

    public override CaseFragment CreateCaseFragment()
    {
      return (CaseFragment) new ANSICaseFragment((NHibernate.Dialect.Dialect) this);
    }
  }
}

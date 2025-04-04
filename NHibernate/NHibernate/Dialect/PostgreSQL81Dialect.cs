// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.PostgreSQL81Dialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlCommand;
using System.Data;

#nullable disable
namespace NHibernate.Dialect
{
  public class PostgreSQL81Dialect : PostgreSQLDialect
  {
    public override string ForUpdateNowaitString => " for update nowait";

    public override string GetForUpdateNowaitString(string aliases)
    {
      return " for update of " + aliases + " nowait";
    }

    public override bool SupportsIdentityColumns => true;

    public override bool HasDataTypeInIdentityColumn => false;

    public override string GetIdentityColumnString(DbType type)
    {
      return type != DbType.Int64 ? "serial" : "bigserial";
    }

    public override string NoColumnsInsertString => "default values";

    public override string IdentitySelectString => "select lastval()";

    public override SqlString AppendIdentitySelectToInsert(SqlString insertSql)
    {
      return insertSql.Append("; " + this.IdentitySelectString);
    }

    public override bool SupportsInsertSelectIdentity => true;
  }
}

// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.MsSql2008Dialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;
using NHibernate.Driver;
using NHibernate.Type;
using System.Data;

#nullable disable
namespace NHibernate.Dialect
{
  public class MsSql2008Dialect : MsSql2005Dialect
  {
    protected override void RegisterDateTimeTypeMappings()
    {
      base.RegisterDateTimeTypeMappings();
      this.RegisterColumnType(DbType.DateTime2, "DATETIME2");
      this.RegisterColumnType(DbType.DateTimeOffset, "DATETIMEOFFSET");
      this.RegisterColumnType(DbType.Date, "DATE");
      this.RegisterColumnType(DbType.Time, "TIME");
    }

    protected override void RegisterFunctions()
    {
      base.RegisterFunctions();
      this.RegisterFunction("current_timestamp", (ISQLFunction) new NoArgSQLFunction("sysdatetime", (IType) NHibernateUtil.DateTime2, true));
      this.RegisterFunction("current_timestamp_offset", (ISQLFunction) new NoArgSQLFunction("sysdatetimeoffset", (IType) NHibernateUtil.DateTimeOffset, true));
    }

    protected override void RegisterKeywords()
    {
      base.RegisterKeywords();
      this.RegisterKeyword("date");
      this.RegisterKeyword("datetimeoffset");
      this.RegisterKeyword("datetime2");
      this.RegisterKeyword("time");
      this.RegisterKeyword("hierarchyid");
    }

    protected override void RegisterDefaultProperties()
    {
      base.RegisterDefaultProperties();
      this.DefaultProperties["connection.driver_class"] = typeof (Sql2008ClientDriver).AssemblyQualifiedName;
    }
  }
}

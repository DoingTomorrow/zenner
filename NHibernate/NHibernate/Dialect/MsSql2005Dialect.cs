// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.MsSql2005Dialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Mapping;
using NHibernate.SqlCommand;
using System.Data;

#nullable disable
namespace NHibernate.Dialect
{
  public class MsSql2005Dialect : MsSql2000Dialect
  {
    public MsSql2005Dialect() => this.RegisterColumnType(DbType.Xml, "XML");

    protected override void RegisterCharacterTypeMappings()
    {
      base.RegisterCharacterTypeMappings();
      this.RegisterColumnType(DbType.String, 1073741823, "NVARCHAR(MAX)");
      this.RegisterColumnType(DbType.AnsiString, int.MaxValue, "VARCHAR(MAX)");
    }

    protected override void RegisterLargeObjectTypeMappings()
    {
      base.RegisterLargeObjectTypeMappings();
      this.RegisterColumnType(DbType.Binary, "VARBINARY(MAX)");
      this.RegisterColumnType(DbType.Binary, 8000, "VARBINARY($l)");
      this.RegisterColumnType(DbType.Binary, int.MaxValue, "VARBINARY(MAX)");
    }

    protected override void RegisterKeywords()
    {
      base.RegisterKeywords();
      this.RegisterKeyword("xml");
    }

    public override SqlString GetLimitString(
      SqlString queryString,
      SqlString offset,
      SqlString limit)
    {
      return new MsSql2005DialectQueryPager(queryString).PageBy(offset, limit);
    }

    public override bool SupportsLimit => true;

    public override bool SupportsLimitOffset => true;

    public override bool SupportsVariableLimit => true;

    protected override string GetSelectExistingObject(string name, Table table)
    {
      string quotedSchemaName = table.GetQuotedSchemaName((NHibernate.Dialect.Dialect) this);
      if (quotedSchemaName != null)
        quotedSchemaName += ".";
      return string.Format("select 1 from sys.objects where object_id = OBJECT_ID(N'{0}') AND parent_object_id = OBJECT_ID('{1}')", (object) string.Format("{0}{1}", (object) quotedSchemaName, (object) this.Quote(name)), (object) string.Format("{0}{1}", (object) quotedSchemaName, (object) table.GetQuotedName((NHibernate.Dialect.Dialect) this)));
    }

    public override bool UseMaxForLimit => false;

    public override string AppendLockHint(LockMode lockMode, string tableName)
    {
      if (!this.NeedsLockHint(lockMode))
        return tableName;
      return lockMode == LockMode.UpgradeNoWait ? tableName + " with (updlock, rowlock, nowait)" : tableName + " with (updlock, rowlock)";
    }
  }
}

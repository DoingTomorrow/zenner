// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Schema.OracleTableMetadata
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Data;

#nullable disable
namespace NHibernate.Dialect.Schema
{
  public class OracleTableMetadata(DataRow rs, IDataBaseSchema meta, bool extras) : 
    AbstractTableMetadata(rs, meta, extras)
  {
    protected override void ParseTableInfo(DataRow rs)
    {
      this.Catalog = (string) null;
      this.Schema = Convert.ToString(rs["OWNER"]);
      if (string.IsNullOrEmpty(this.Schema))
        this.Schema = (string) null;
      this.Name = Convert.ToString(rs["TABLE_NAME"]);
    }

    protected override string GetConstraintName(DataRow rs)
    {
      return Convert.ToString(rs["FOREIGN_KEY_CONSTRAINT_NAME"]);
    }

    protected override string GetColumnName(DataRow rs) => Convert.ToString(rs["COLUMN_NAME"]);

    protected override string GetIndexName(DataRow rs) => Convert.ToString(rs["INDEX_NAME"]);

    protected override IColumnMetadata GetColumnMetadata(DataRow rs)
    {
      return (IColumnMetadata) new OracleColumnMetadata(rs);
    }

    protected override IForeignKeyMetadata GetForeignKeyMetadata(DataRow rs)
    {
      return (IForeignKeyMetadata) new OracleForeignKeyMetadata(rs);
    }

    protected override IIndexMetadata GetIndexMetadata(DataRow rs)
    {
      return (IIndexMetadata) new OracleIndexMetadata(rs);
    }
  }
}

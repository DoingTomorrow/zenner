// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Schema.SybaseAnywhereTableMetaData
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Data;

#nullable disable
namespace NHibernate.Dialect.Schema
{
  public class SybaseAnywhereTableMetaData(DataRow rs, IDataBaseSchema meta, bool extras) : 
    AbstractTableMetadata(rs, meta, extras)
  {
    protected override IColumnMetadata GetColumnMetadata(DataRow rs)
    {
      return (IColumnMetadata) new SybaseAnywhereColumnMetaData(rs);
    }

    protected override string GetColumnName(DataRow rs) => Convert.ToString(rs["COLUMN_NAME"]);

    protected override string GetConstraintName(DataRow rs) => Convert.ToString(rs["COLUMN_NAME"]);

    protected override IForeignKeyMetadata GetForeignKeyMetadata(DataRow rs)
    {
      return (IForeignKeyMetadata) new SybaseAnywhereForeignKeyMetaData(rs);
    }

    protected override IIndexMetadata GetIndexMetadata(DataRow rs)
    {
      return (IIndexMetadata) new SybaseAnywhereIndexMetaData(rs);
    }

    protected override string GetIndexName(DataRow rs) => (string) rs["INDEX_NAME"];

    protected override void ParseTableInfo(DataRow rs)
    {
      this.Catalog = (string) null;
      this.Schema = Convert.ToString(rs["TABLE_SCHEMA"]);
      if (string.IsNullOrEmpty(this.Schema))
        this.Schema = (string) null;
      this.Name = Convert.ToString(rs["TABLE_NAME"]);
    }
  }
}

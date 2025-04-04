// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Schema.MsSqlColumnMetadata
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Data;

#nullable disable
namespace NHibernate.Dialect.Schema
{
  public class MsSqlColumnMetadata : AbstractColumnMetaData
  {
    public MsSqlColumnMetadata(DataRow rs)
      : base(rs)
    {
      this.Name = Convert.ToString(rs["COLUMN_NAME"]);
      this.SetColumnSize(SchemaHelper.GetValue(rs, "CHARACTER_MAXIMUM_LENGTH", "COLUMN_SIZE"));
      this.SetNumericalPrecision(SchemaHelper.GetValue(rs, "NUMERIC_PRECISION", "COLUMN_SIZE"));
      this.Nullable = Convert.ToString(rs["IS_NULLABLE"]);
      this.TypeName = SchemaHelper.GetString(rs, "TYPE_NAME", "DATA_TYPE");
    }
  }
}

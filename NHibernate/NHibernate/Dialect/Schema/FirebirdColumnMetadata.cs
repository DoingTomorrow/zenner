// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Schema.FirebirdColumnMetadata
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Data;

#nullable disable
namespace NHibernate.Dialect.Schema
{
  public class FirebirdColumnMetadata : AbstractColumnMetaData
  {
    public FirebirdColumnMetadata(DataRow rs)
      : base(rs)
    {
      this.Name = Convert.ToString(rs["COLUMN_NAME"]);
      this.SetColumnSize(rs["COLUMN_SIZE"]);
      this.SetNumericalPrecision(rs["NUMERIC_PRECISION"]);
      this.Nullable = Convert.ToString(rs["IS_NULLABLE"]);
      this.TypeName = Convert.ToString(rs["COLUMN_DATA_TYPE"]);
    }
  }
}

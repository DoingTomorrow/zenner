// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Schema.OracleColumnMetadata
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Data;

#nullable disable
namespace NHibernate.Dialect.Schema
{
  public class OracleColumnMetadata : AbstractColumnMetaData
  {
    public OracleColumnMetadata(DataRow rs)
      : base(rs)
    {
      this.Name = Convert.ToString(rs["COLUMN_NAME"]);
      this.SetColumnSize(rs["LENGTH"]);
      this.SetNumericalPrecision(rs["PRECISION"]);
      this.Nullable = Convert.ToString(rs["NULLABLE"]);
      this.TypeName = Convert.ToString(rs["DATATYPE"]);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Schema.MsSqlCeDataBaseSchema
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Data;
using System.Data.Common;

#nullable disable
namespace NHibernate.Dialect.Schema
{
  public class MsSqlCeDataBaseSchema(DbConnection connection) : AbstractDataBaseSchema(connection)
  {
    public override ITableMetadata GetTableMetadata(DataRow rs, bool extras)
    {
      return (ITableMetadata) new MsSqlCeTableMetadata(rs, (IDataBaseSchema) this, extras);
    }
  }
}

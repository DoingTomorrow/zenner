// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.SybaseASA10Dialect
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Data;

#nullable disable
namespace NHibernate.Dialect
{
  [Obsolete("Please use SybaseSQLAnywhere10Dialect instead. This dialect will be removed in a future release.")]
  public class SybaseASA10Dialect : SybaseASA9Dialect
  {
    public SybaseASA10Dialect()
    {
      this.RegisterColumnType(DbType.StringFixedLength, (int) byte.MaxValue, "NCHAR($l)");
      this.RegisterColumnType(DbType.String, 1073741823, "LONG NVARCHAR");
      this.RegisterColumnType(DbType.String, (int) byte.MaxValue, "NVARCHAR($l)");
      this.RegisterColumnType(DbType.String, "LONG NVARCHAR");
    }
  }
}

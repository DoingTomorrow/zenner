// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlTypes.SqlType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Data;
using System.Text;

#nullable disable
namespace NHibernate.SqlTypes
{
  [Serializable]
  public class SqlType
  {
    private readonly DbType dbType;
    private readonly int length;
    private readonly byte precision;
    private readonly byte scale;
    private readonly bool lengthDefined;
    private readonly bool precisionDefined;

    public SqlType(DbType dbType) => this.dbType = dbType;

    public SqlType(DbType dbType, int length)
    {
      this.dbType = dbType;
      this.length = length;
      this.lengthDefined = true;
    }

    public SqlType(DbType dbType, byte precision, byte scale)
    {
      this.dbType = dbType;
      this.precision = precision;
      this.scale = scale;
      this.precisionDefined = true;
    }

    public DbType DbType => this.dbType;

    public int Length => this.length;

    public byte Precision => this.precision;

    public byte Scale => this.scale;

    public bool LengthDefined => this.lengthDefined;

    public bool PrecisionDefined => this.precisionDefined;

    public override int GetHashCode()
    {
      return !this.LengthDefined ? (!this.PrecisionDefined ? this.DbType.GetHashCode() : this.DbType.GetHashCode() / 3 + this.Precision.GetHashCode() / 3 + this.Scale.GetHashCode() / 3) : this.DbType.GetHashCode() / 2 + this.Length.GetHashCode() / 2;
    }

    public override bool Equals(object obj) => obj == this || this.Equals(obj as SqlType);

    public bool Equals(SqlType rhsSqlType)
    {
      if (rhsSqlType == null)
        return false;
      if (this.LengthDefined)
        return this.DbType.Equals((object) rhsSqlType.DbType) && this.Length == rhsSqlType.Length;
      if (!this.PrecisionDefined)
        return this.DbType.Equals((object) rhsSqlType.DbType);
      return this.DbType.Equals((object) rhsSqlType.DbType) && (int) this.Precision == (int) rhsSqlType.Precision && (int) this.Scale == (int) rhsSqlType.Scale;
    }

    public override string ToString()
    {
      if (!this.LengthDefined && !this.PrecisionDefined)
        return this.DbType.ToString();
      StringBuilder stringBuilder = new StringBuilder(this.DbType.ToString());
      if (this.LengthDefined)
        stringBuilder.Append("(Length=").Append(this.Length).Append(')');
      if (this.PrecisionDefined)
        stringBuilder.Append("(Precision=").Append(this.Precision).Append(", ").Append("Scale=").Append(this.Scale).Append(')');
      return stringBuilder.ToString();
    }
  }
}

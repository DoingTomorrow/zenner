// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Schema.AbstractColumnMetaData
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Data;

#nullable disable
namespace NHibernate.Dialect.Schema
{
  public abstract class AbstractColumnMetaData : IColumnMetadata
  {
    private string name;
    private string typeName;
    private int columnSize;
    private int numericalPrecision;
    private string isNullable;

    public AbstractColumnMetaData(DataRow rs)
    {
    }

    public string Name
    {
      get => this.name;
      protected set => this.name = value;
    }

    public string TypeName
    {
      get => this.typeName;
      protected set => this.typeName = value;
    }

    public int ColumnSize
    {
      get => this.columnSize;
      protected set => this.columnSize = value;
    }

    public int NumericalPrecision
    {
      get => this.numericalPrecision;
      protected set => this.numericalPrecision = value;
    }

    public string Nullable
    {
      get => this.isNullable;
      protected set => this.isNullable = value;
    }

    public override string ToString() => "ColumnMetadata(" + this.name + (object) ')';

    protected void SetColumnSize(object columnSizeValue)
    {
      if (columnSizeValue == DBNull.Value)
        return;
      this.columnSize = (int) Math.Min((long) int.MaxValue, Convert.ToInt64(columnSizeValue));
    }

    protected void SetNumericalPrecision(object numericalPrecisionValue)
    {
      if (numericalPrecisionValue == DBNull.Value)
        return;
      this.NumericalPrecision = Convert.ToInt32(numericalPrecisionValue);
    }
  }
}

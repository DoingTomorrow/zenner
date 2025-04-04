// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.ColumnMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class ColumnMapper : IColumnMapper
  {
    private readonly HbmColumn mapping;

    public ColumnMapper(HbmColumn mapping, string memberName)
    {
      if (mapping == null)
        throw new ArgumentNullException(nameof (mapping));
      if (memberName == null)
        throw new ArgumentNullException(nameof (memberName));
      if (string.Empty == memberName.Trim())
        throw new ArgumentNullException(nameof (memberName), "The column name should be a valid not empty name.");
      this.mapping = mapping;
      if (!string.IsNullOrEmpty(mapping.name))
        return;
      mapping.name = memberName;
    }

    public void Name(string name) => this.mapping.name = name;

    public void Length(int length)
    {
      this.mapping.length = length > 0 ? length.ToString() : throw new ArgumentOutOfRangeException(nameof (length), "The length should be positive value");
    }

    public void Precision(short precision)
    {
      this.mapping.precision = precision > (short) 0 ? precision.ToString() : throw new ArgumentOutOfRangeException(nameof (precision), "The precision should be positive value");
    }

    public void Scale(short scale)
    {
      this.mapping.scale = scale >= (short) 0 ? scale.ToString() : throw new ArgumentOutOfRangeException(nameof (scale), "The scale should be positive value");
    }

    public void NotNullable(bool notnull)
    {
      this.mapping.notnull = this.mapping.notnullSpecified = notnull;
    }

    public void Unique(bool unique) => this.mapping.unique = this.mapping.uniqueSpecified = unique;

    public void UniqueKey(string uniquekeyName) => this.mapping.uniquekey = uniquekeyName;

    public void SqlType(string sqltype) => this.mapping.sqltype = sqltype;

    public void Index(string indexName) => this.mapping.index = indexName;

    public void Check(string checkConstraint) => this.mapping.check = checkConstraint;

    public void Default(object defaultValue)
    {
      FormatterConverter formatterConverter = new FormatterConverter();
      this.mapping.@default = defaultValue == null ? "null" : formatterConverter.ToString(defaultValue);
    }
  }
}

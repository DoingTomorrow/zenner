// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.Column
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Util;
using System;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class Column : ISelectable, ICloneable
  {
    public const int DefaultLength = 255;
    public const int DefaultPrecision = 19;
    public const int DefaultScale = 2;
    private int? length;
    private int? precision;
    private int? scale;
    private IValue _value;
    private int typeIndex;
    private string name;
    private bool nullable = true;
    private bool unique;
    private string sqlType;
    private NHibernate.SqlTypes.SqlType sqlTypeCode;
    private bool quoted;
    internal int uniqueInteger;
    private string checkConstraint;
    private string comment;
    private string defaultValue;

    public Column()
    {
    }

    public Column(string columnName) => this.Name = columnName;

    public int Length
    {
      get => this.length.GetValueOrDefault((int) byte.MaxValue);
      set => this.length = new int?(value);
    }

    public string Name
    {
      get => this.name;
      set
      {
        if (value[0] == '`')
        {
          this.quoted = true;
          this.name = value.Substring(1, value.Length - 2);
        }
        else
          this.name = value;
      }
    }

    public string CanonicalName => !this.quoted ? this.name.ToLowerInvariant() : this.name;

    public string GetQuotedName(NHibernate.Dialect.Dialect d)
    {
      return !this.IsQuoted ? this.name : d.QuoteForColumnName(this.name);
    }

    public string GetAlias(NHibernate.Dialect.Dialect dialect)
    {
      string str1 = this.name;
      string str2 = this.uniqueInteger.ToString() + (object) '_';
      int num = StringHelper.LastIndexOfLetter(this.name);
      if (num == -1)
        str1 = "column";
      else if (num < this.name.Length - 1)
        str1 = this.name.Substring(0, num + 1);
      if (str1.Length > dialect.MaxAliasLength)
        str1 = str1.Substring(0, dialect.MaxAliasLength - str2.Length);
      return this.name.Equals(str1) && !this.quoted && !StringHelper.EqualsCaseInsensitive(this.name, "rowid") ? str1 : str1 + str2;
    }

    public string GetAlias(NHibernate.Dialect.Dialect dialect, Table table)
    {
      return this.GetAlias(dialect) + (object) table.UniqueInteger + (object) '_';
    }

    public bool IsNullable
    {
      get => this.nullable;
      set => this.nullable = value;
    }

    public int TypeIndex
    {
      get => this.typeIndex;
      set => this.typeIndex = value;
    }

    public bool IsUnique
    {
      get => this.unique;
      set => this.unique = value;
    }

    public string GetSqlType(NHibernate.Dialect.Dialect dialect, IMapping mapping)
    {
      return this.sqlType ?? this.GetDialectTypeName(dialect, mapping);
    }

    private string GetDialectTypeName(NHibernate.Dialect.Dialect dialect, IMapping mapping)
    {
      return this.IsCaracteristicsDefined() ? dialect.GetTypeName(this.GetSqlTypeCode(mapping), !this.IsPrecisionDefined() ? this.Length : 0, this.Precision, this.Scale) : dialect.GetTypeName(this.GetSqlTypeCode(mapping));
    }

    public override bool Equals(object obj) => obj is Column column && this.Equals(column);

    public bool Equals(Column column)
    {
      if (column == null)
        return false;
      if (this == column)
        return true;
      return !this.IsQuoted ? this.name.ToLowerInvariant().Equals(column.name.ToLowerInvariant()) : this.name.Equals(column.name);
    }

    public override int GetHashCode()
    {
      return !this.IsQuoted ? this.name.ToLowerInvariant().GetHashCode() : this.name.GetHashCode();
    }

    public string SqlType
    {
      get => this.sqlType;
      set => this.sqlType = value;
    }

    public bool IsQuoted
    {
      get => this.quoted;
      set => this.quoted = value;
    }

    public bool Unique
    {
      get => this.unique;
      set => this.unique = value;
    }

    public string CheckConstraint
    {
      get => this.checkConstraint;
      set => this.checkConstraint = value;
    }

    public bool HasCheckConstraint => !string.IsNullOrEmpty(this.checkConstraint);

    public string Text => this.Name;

    public string GetText(NHibernate.Dialect.Dialect dialect) => this.GetQuotedName(dialect);

    public bool IsFormula => false;

    public int Precision
    {
      get => this.precision.GetValueOrDefault(19);
      set => this.precision = new int?(value);
    }

    public int Scale
    {
      get => this.scale.GetValueOrDefault(2);
      set => this.scale = new int?(value);
    }

    public IValue Value
    {
      get => this._value;
      set => this._value = value;
    }

    public NHibernate.SqlTypes.SqlType SqlTypeCode
    {
      get => this.sqlTypeCode;
      set => this.sqlTypeCode = value;
    }

    public string Comment
    {
      get => this.comment;
      set => this.comment = value;
    }

    public string DefaultValue
    {
      get => this.defaultValue;
      set => this.defaultValue = value;
    }

    public string GetTemplate(NHibernate.Dialect.Dialect dialect, SQLFunctionRegistry functionRegistry)
    {
      return this.GetQuotedName(dialect);
    }

    public override string ToString()
    {
      return string.Format("{0}({1})", (object) this.GetType().FullName, (object) this.name);
    }

    public NHibernate.SqlTypes.SqlType GetSqlTypeCode(IMapping mapping)
    {
      IType type = this.Value.Type;
      try
      {
        NHibernate.SqlTypes.SqlType sqlType = type.SqlTypes(mapping)[this.TypeIndex];
        return this.SqlTypeCode == null || this.SqlTypeCode == sqlType ? sqlType : throw new MappingException(string.Format("SQLType code's does not match. mapped as {0} but is {1}", (object) sqlType, (object) this.SqlTypeCode));
      }
      catch (Exception ex)
      {
        throw new MappingException(string.Format("Could not determine type for column {0} of type {1}: {2}", (object) this.name, (object) type.GetType().FullName, (object) ex.GetType().FullName), ex);
      }
    }

    public string GetQuotedName()
    {
      return !this.quoted ? this.name : '`'.ToString() + this.name + (object) '`';
    }

    public bool IsCaracteristicsDefined() => this.IsLengthDefined() || this.IsPrecisionDefined();

    public bool IsPrecisionDefined() => this.precision.HasValue || this.scale.HasValue;

    public bool IsLengthDefined() => this.length.HasValue;

    public object Clone()
    {
      Column column = new Column();
      if (this.length.HasValue)
        column.Length = this.Length;
      if (this.precision.HasValue)
        column.Precision = this.Precision;
      if (this.scale.HasValue)
        column.Scale = this.Scale;
      column.Value = this._value;
      column.TypeIndex = this.typeIndex;
      column.Name = this.GetQuotedName();
      column.IsNullable = this.nullable;
      column.Unique = this.unique;
      column.SqlType = this.sqlType;
      column.SqlTypeCode = this.sqlTypeCode;
      column.uniqueInteger = this.uniqueInteger;
      column.CheckConstraint = this.checkConstraint;
      column.Comment = this.comment;
      column.DefaultValue = this.defaultValue;
      return (object) column;
    }
  }
}

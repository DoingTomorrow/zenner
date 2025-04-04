// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.ColumnsBinder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cfg.XmlHbmBinding
{
  public class ColumnsBinder : Binder
  {
    private readonly SimpleValue value;

    public ColumnsBinder(SimpleValue value, Mappings mappings)
      : base(mappings)
    {
      this.value = value;
    }

    public void Bind(HbmColumn column, bool isNullable)
    {
      this.BindColumn(column, this.value.Table, isNullable);
    }

    public void Bind(
      IEnumerable<HbmColumn> columns,
      bool isNullable,
      Func<HbmColumn> defaultColumnDelegate)
    {
      Table table = this.value.Table;
      foreach (HbmColumn column in columns)
        this.BindColumn(column, table, isNullable);
      if (this.value.ColumnSpan != 0 || defaultColumnDelegate == null)
        return;
      this.BindColumn(defaultColumnDelegate(), table, isNullable);
    }

    private void BindColumn(HbmColumn hbmColumn, Table table, bool isNullable)
    {
      Column column = new Column()
      {
        Value = (IValue) this.value
      };
      this.BindColumn(hbmColumn, column, isNullable);
      table?.AddColumn(column);
      this.value.AddColumn(column);
      ColumnsBinder.BindIndex(hbmColumn.index, table, column);
      ColumnsBinder.BindUniqueKey(hbmColumn.uniquekey, table, column);
    }

    private void BindColumn(HbmColumn columnMapping, Column column, bool isNullable)
    {
      column.Name = this.mappings.NamingStrategy.ColumnName(columnMapping.name);
      if (columnMapping.length != null)
        column.Length = int.Parse(columnMapping.length);
      if (columnMapping.scale != null)
        column.Scale = int.Parse(columnMapping.scale);
      if (columnMapping.precision != null)
        column.Precision = int.Parse(columnMapping.precision);
      column.IsNullable = columnMapping.notnullSpecified ? !columnMapping.notnull : isNullable;
      column.IsUnique = columnMapping.uniqueSpecified && columnMapping.unique;
      column.CheckConstraint = columnMapping.check ?? string.Empty;
      column.SqlType = columnMapping.sqltype;
      column.DefaultValue = columnMapping.@default;
      if (columnMapping.comment == null)
        return;
      column.Comment = columnMapping.comment.Text.LinesToString().Trim();
    }

    private static void BindIndex(string indexAttribute, Table table, Column column)
    {
      if (indexAttribute == null || table == null)
        return;
      System.Array.ForEach<string>(indexAttribute.Split(','), (Action<string>) (t => table.GetOrCreateIndex(t.Trim()).AddColumn(column)));
    }

    private static void BindUniqueKey(string uniqueKeyAttribute, Table table, Column column)
    {
      if (uniqueKeyAttribute == null || table == null)
        return;
      System.Array.ForEach<string>(uniqueKeyAttribute.Split(','), (Action<string>) (t => table.GetOrCreateUniqueKey(t.Trim()).AddColumn(column)));
    }
  }
}

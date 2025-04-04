// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.Index
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class Index : IRelationalModel
  {
    private Table table;
    private readonly List<Column> columns = new List<Column>();
    private string name;

    public static string BuildSqlCreateIndexString(
      NHibernate.Dialect.Dialect dialect,
      string name,
      Table table,
      IEnumerable<Column> columns,
      bool unique,
      string defaultCatalog,
      string defaultSchema)
    {
      StringBuilder stringBuilder = new StringBuilder("create").Append(unique ? " unique" : "").Append(" index ").Append(dialect.QualifyIndexName ? name : StringHelper.Unqualify(name)).Append(" on ").Append(table.GetQualifiedName(dialect, defaultCatalog, defaultSchema)).Append(" (");
      bool flag = false;
      foreach (Column column in columns)
      {
        if (flag)
          stringBuilder.Append(", ");
        flag = true;
        stringBuilder.Append(column.GetQuotedName(dialect));
      }
      stringBuilder.Append(")");
      return stringBuilder.ToString();
    }

    public static string BuildSqlDropIndexString(
      NHibernate.Dialect.Dialect dialect,
      Table table,
      string name,
      string defaultCatalog,
      string defaultSchema)
    {
      return dialect.GetIfExistsDropConstraint(table, name) + Environment.NewLine + string.Format("drop index {0}", (object) StringHelper.Qualify(table.GetQualifiedName(dialect, defaultCatalog, defaultSchema), name)) + Environment.NewLine + dialect.GetIfExistsDropConstraintEnd(table, name);
    }

    public string SqlCreateString(
      NHibernate.Dialect.Dialect dialect,
      IMapping p,
      string defaultCatalog,
      string defaultSchema)
    {
      return Index.BuildSqlCreateIndexString(dialect, this.Name, this.Table, this.ColumnIterator, false, defaultCatalog, defaultSchema);
    }

    public string SqlDropString(NHibernate.Dialect.Dialect dialect, string defaultCatalog, string defaultSchema)
    {
      return Index.BuildSqlDropIndexString(dialect, this.Table, this.Name, defaultCatalog, defaultSchema);
    }

    public Table Table
    {
      get => this.table;
      set => this.table = value;
    }

    public IEnumerable<Column> ColumnIterator => (IEnumerable<Column>) this.columns;

    public int ColumnSpan => this.columns.Count;

    public void AddColumn(Column column)
    {
      if (this.columns.Contains(column))
        return;
      this.columns.Add(column);
    }

    public void AddColumns(IEnumerable<Column> extraColumns)
    {
      foreach (Column extraColumn in extraColumns)
        this.AddColumn(extraColumn);
    }

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public bool ContainsColumn(Column column) => this.columns.Contains(column);

    public override string ToString() => this.GetType().FullName + "(" + this.Name + ")";
  }
}

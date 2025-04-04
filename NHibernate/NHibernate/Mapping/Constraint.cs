// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.Constraint
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public abstract class Constraint : IRelationalModel
  {
    private string name;
    private readonly List<Column> columns = new List<Column>();
    private Table table;

    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public IEnumerable<Column> ColumnIterator => (IEnumerable<Column>) this.columns;

    public void AddColumn(Column column)
    {
      if (this.columns.Contains(column))
        return;
      this.columns.Add(column);
    }

    public void AddColumns(IEnumerable<Column> columnIterator)
    {
      foreach (Column column in columnIterator)
      {
        if (!column.IsFormula)
          this.AddColumn(column);
      }
    }

    public int ColumnSpan => this.columns.Count;

    public IList<Column> Columns => (IList<Column>) this.columns;

    public Table Table
    {
      get => this.table;
      set => this.table = value;
    }

    public virtual string SqlDropString(
      NHibernate.Dialect.Dialect dialect,
      string defaultCatalog,
      string defaultSchema)
    {
      if (!this.IsGenerated(dialect))
        return (string) null;
      return dialect.GetIfExistsDropConstraint(this.Table, this.Name) + Environment.NewLine + string.Format("alter table {0} drop constraint {1}", (object) this.Table.GetQualifiedName(dialect, defaultCatalog, defaultSchema), (object) this.Name) + Environment.NewLine + dialect.GetIfExistsDropConstraintEnd(this.Table, this.Name);
    }

    public virtual string SqlCreateString(
      NHibernate.Dialect.Dialect dialect,
      IMapping p,
      string defaultCatalog,
      string defaultSchema)
    {
      if (!this.IsGenerated(dialect))
        return (string) null;
      string str = this.SqlConstraintString(dialect, this.Name, defaultCatalog, defaultSchema);
      return new StringBuilder("alter table ").Append(this.Table.GetQualifiedName(dialect, defaultCatalog, defaultSchema)).Append(str).ToString();
    }

    public abstract string SqlConstraintString(
      NHibernate.Dialect.Dialect d,
      string constraintName,
      string defaultCatalog,
      string defaultSchema);

    public virtual bool IsGenerated(NHibernate.Dialect.Dialect dialect) => true;

    public override string ToString()
    {
      return string.Format("{0}({1}{2}) as {3}", (object) this.GetType().FullName, (object) this.Table.Name, (object) StringHelper.CollectionToString((ICollection) this.Columns), (object) this.name);
    }
  }
}

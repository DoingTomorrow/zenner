// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ForeignKey
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class ForeignKey : Constraint
  {
    private Table referencedTable;
    private string referencedEntityName;
    private bool cascadeDeleteEnabled;
    private readonly List<Column> referencedColumns = new List<Column>();

    public override string SqlConstraintString(
      NHibernate.Dialect.Dialect d,
      string constraintName,
      string defaultCatalog,
      string defaultSchema)
    {
      string[] foreignKey = new string[this.ColumnSpan];
      string[] primaryKey = new string[this.ColumnSpan];
      int index1 = 0;
      IEnumerable<Column> columns = !this.IsReferenceToPrimaryKey ? (IEnumerable<Column>) this.referencedColumns : this.referencedTable.PrimaryKey.ColumnIterator;
      foreach (Column column in this.ColumnIterator)
      {
        foreignKey[index1] = column.GetQuotedName(d);
        ++index1;
      }
      int index2 = 0;
      foreach (Column column in columns)
      {
        primaryKey[index2] = column.GetQuotedName(d);
        ++index2;
      }
      string constraintString = d.GetAddForeignKeyConstraintString(constraintName, foreignKey, this.referencedTable.GetQualifiedName(d, defaultCatalog, defaultSchema), primaryKey, this.IsReferenceToPrimaryKey);
      return !this.cascadeDeleteEnabled || !d.SupportsCascadeDelete ? constraintString : constraintString + " on delete cascade";
    }

    public Table ReferencedTable
    {
      get => this.referencedTable;
      set => this.referencedTable = value;
    }

    public bool CascadeDeleteEnabled
    {
      get => this.cascadeDeleteEnabled;
      set => this.cascadeDeleteEnabled = value;
    }

    public override string SqlDropString(
      NHibernate.Dialect.Dialect dialect,
      string defaultCatalog,
      string defaultSchema)
    {
      return dialect.GetIfExistsDropConstraint(this.Table, this.Name) + Environment.NewLine + string.Format("alter table {0} {1}", (object) this.Table.GetQualifiedName(dialect, defaultCatalog, defaultSchema), (object) dialect.GetDropForeignKeyConstraintString(this.Name)) + Environment.NewLine + dialect.GetIfExistsDropConstraintEnd(this.Table, this.Name);
    }

    public void AlignColumns()
    {
      if (!this.IsReferenceToPrimaryKey)
        return;
      this.AlignColumns(this.referencedTable);
    }

    private void AlignColumns(Table referencedTable)
    {
      if (referencedTable.PrimaryKey.ColumnSpan != this.ColumnSpan)
      {
        StringBuilder buf = new StringBuilder();
        buf.Append("Foreign key (").Append(this.Name + ":").Append(this.Table.Name).Append(" [");
        ForeignKey.AppendColumns(buf, this.ColumnIterator);
        buf.Append("])").Append(") must have same number of columns as the referenced primary key (").Append(referencedTable.Name).Append(" [");
        ForeignKey.AppendColumns(buf, referencedTable.PrimaryKey.ColumnIterator);
        buf.Append("])");
        throw new FKUnmatchingColumnsException(buf.ToString());
      }
      IEnumerator<Column> enumerator1 = this.ColumnIterator.GetEnumerator();
      IEnumerator<Column> enumerator2 = referencedTable.PrimaryKey.ColumnIterator.GetEnumerator();
      while (enumerator1.MoveNext() && enumerator2.MoveNext())
        enumerator1.Current.Length = enumerator2.Current.Length;
    }

    private static void AppendColumns(StringBuilder buf, IEnumerable<Column> columns)
    {
      bool flag = false;
      foreach (Column column in columns)
      {
        if (flag)
          buf.Append(", ");
        flag = true;
        buf.Append(column.Name);
      }
    }

    public virtual void AddReferencedColumns(IEnumerable<Column> referencedColumnsIterator)
    {
      foreach (Column column in referencedColumnsIterator)
      {
        if (!column.IsFormula)
          this.AddReferencedColumn(column);
      }
    }

    private void AddReferencedColumn(Column column)
    {
      if (this.referencedColumns.Contains(column))
        return;
      this.referencedColumns.Add(column);
    }

    internal void AddReferencedTable(PersistentClass referencedClass)
    {
      if (this.referencedColumns.Count > 0)
        this.referencedTable = this.referencedColumns[0].Value.Table;
      else
        this.referencedTable = referencedClass.Table;
    }

    public override string ToString()
    {
      if (this.IsReferenceToPrimaryKey)
        return base.ToString();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(this.GetType().FullName).Append('(').Append(this.Table.Name).Append(StringHelper.Join(", ", (IEnumerable) this.Columns)).Append(" ref-columns:").Append('(').Append(StringHelper.Join(", ", (IEnumerable) this.ReferencedColumns)).Append(") as ").Append(this.Name);
      return stringBuilder.ToString();
    }

    public bool HasPhysicalConstraint
    {
      get
      {
        return this.referencedTable.IsPhysicalTable && this.Table.IsPhysicalTable && !this.referencedTable.HasDenormalizedTables;
      }
    }

    public IList<Column> ReferencedColumns => (IList<Column>) this.referencedColumns;

    public string ReferencedEntityName
    {
      get => this.referencedEntityName;
      set => this.referencedEntityName = value;
    }

    public bool IsReferenceToPrimaryKey => this.referencedColumns.Count == 0;
  }
}

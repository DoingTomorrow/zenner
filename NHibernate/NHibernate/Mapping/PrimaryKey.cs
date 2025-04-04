// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.PrimaryKey
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Text;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class PrimaryKey : Constraint
  {
    public string SqlConstraintString(NHibernate.Dialect.Dialect d, string defaultSchema)
    {
      StringBuilder stringBuilder = new StringBuilder(d.PrimaryKeyString + " (");
      int num = 0;
      foreach (Column column in this.ColumnIterator)
      {
        stringBuilder.Append(column.GetQuotedName(d));
        if (num < this.ColumnSpan - 1)
          stringBuilder.Append(", ");
        ++num;
      }
      return stringBuilder.Append(")").ToString();
    }

    public override string SqlConstraintString(
      NHibernate.Dialect.Dialect d,
      string constraintName,
      string defaultCatalog,
      string defaultSchema)
    {
      StringBuilder stringBuilder = new StringBuilder(d.GetAddPrimaryKeyConstraintString(constraintName)).Append('(');
      int num = 0;
      foreach (Column column in this.ColumnIterator)
      {
        stringBuilder.Append(column.GetQuotedName(d));
        if (num < this.ColumnSpan - 1)
          stringBuilder.Append(", ");
        ++num;
      }
      return stringBuilder.Append(")").ToString();
    }

    public override string SqlDropString(
      NHibernate.Dialect.Dialect dialect,
      string defaultCatalog,
      string defaultSchema)
    {
      return dialect.GetIfExistsDropConstraint(this.Table, this.Name) + Environment.NewLine + string.Format("alter table {0}{1}", (object) this.Table.GetQualifiedName(dialect, defaultCatalog, defaultSchema), (object) dialect.GetDropPrimaryKeyConstraintString(this.Name)) + Environment.NewLine + dialect.GetIfExistsDropConstraintEnd(this.Table, this.Name);
    }
  }
}

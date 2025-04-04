// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.UniqueKey
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Util;
using System;
using System.Text;

#nullable disable
namespace NHibernate.Mapping
{
  [Serializable]
  public class UniqueKey : Constraint
  {
    public string SqlConstraintString(NHibernate.Dialect.Dialect dialect)
    {
      StringBuilder stringBuilder = new StringBuilder("unique (");
      bool flag1 = false;
      bool flag2 = false;
      foreach (Column column in this.ColumnIterator)
      {
        if (!flag2 && column.IsNullable)
          flag2 = true;
        if (flag1)
          stringBuilder.Append(", ");
        flag1 = true;
        stringBuilder.Append(column.GetQuotedName(dialect));
      }
      return flag2 && !dialect.SupportsNotNullUnique ? (string) null : stringBuilder.Append(")").ToString();
    }

    public override string SqlConstraintString(
      NHibernate.Dialect.Dialect dialect,
      string constraintName,
      string defaultCatalog,
      string defaultSchema)
    {
      StringBuilder stringBuilder = new StringBuilder(dialect.GetAddPrimaryKeyConstraintString(constraintName)).Append("(");
      bool flag1 = false;
      bool flag2 = false;
      foreach (Column column in this.ColumnIterator)
      {
        if (!flag2 && column.IsNullable)
          flag2 = true;
        if (flag1)
          stringBuilder.Append(", ");
        flag1 = true;
        stringBuilder.Append(column.GetQuotedName(dialect));
      }
      return flag2 && !dialect.SupportsNotNullUnique ? (string) null : StringHelper.Replace(stringBuilder.Append(")").ToString(), "primary key", "unique");
    }

    public override string SqlCreateString(
      NHibernate.Dialect.Dialect dialect,
      IMapping p,
      string defaultCatalog,
      string defaultSchema)
    {
      return dialect.SupportsUniqueConstraintInCreateAlterTable ? base.SqlCreateString(dialect, p, defaultCatalog, defaultSchema) : Index.BuildSqlCreateIndexString(dialect, this.Name, this.Table, this.ColumnIterator, true, defaultCatalog, defaultSchema);
    }

    public override string SqlDropString(
      NHibernate.Dialect.Dialect dialect,
      string defaultCatalog,
      string defaultSchema)
    {
      return dialect.SupportsUniqueConstraintInCreateAlterTable ? base.SqlDropString(dialect, defaultCatalog, defaultSchema) : Index.BuildSqlDropIndexString(dialect, this.Table, this.Name, defaultCatalog, defaultSchema);
    }

    public override bool IsGenerated(NHibernate.Dialect.Dialect dialect)
    {
      if (dialect.SupportsNotNullUnique)
        return true;
      foreach (Column column in this.ColumnIterator)
      {
        if (column.IsNullable)
          return false;
      }
      return true;
    }
  }
}

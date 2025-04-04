// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.InFragment
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.SqlCommand
{
  public class InFragment
  {
    public static readonly string NotNull = "not null";
    public static readonly string Null = "null";
    private readonly ArrayList values = new ArrayList();
    private string columnName;

    public InFragment AddValue(object value)
    {
      this.values.Add(value);
      return this;
    }

    public InFragment SetColumn(string colName)
    {
      this.columnName = colName;
      return this;
    }

    public InFragment SetColumn(string alias, string colName)
    {
      this.columnName = alias + (object) '.' + colName;
      return this.SetColumn(this.columnName);
    }

    public InFragment SetFormula(string alias, string formulaTemplate)
    {
      this.columnName = StringHelper.Replace(formulaTemplate, Template.Placeholder, alias);
      return this.SetColumn(this.columnName);
    }

    public SqlString ToFragmentString()
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder(this.values.Count * 5);
      sqlStringBuilder.Add(this.columnName);
      if (this.values.Count > 1)
      {
        bool flag1 = false;
        bool flag2 = false;
        sqlStringBuilder.Add(" in (");
        for (int index = 0; index < this.values.Count; ++index)
        {
          object sql = this.values[index];
          if (InFragment.Null.Equals(sql))
          {
            flag2 = true;
          }
          else
          {
            if (InFragment.NotNull.Equals(sql))
              throw new NotSupportedException(string.Format("not null makes no sense for in expression (column:{0})", (object) this.columnName));
            if (flag1)
              sqlStringBuilder.Add(", ");
            if ((object) (sql as Parameter) != null)
              sqlStringBuilder.Add((Parameter) sql);
            else
              sqlStringBuilder.Add((string) sql);
            flag1 = true;
          }
        }
        sqlStringBuilder.Add(")");
        if (flag2)
          sqlStringBuilder.Insert(0, " is null or ").Insert(0, this.columnName).Insert(0, "(").Add(")");
      }
      else
      {
        object obj = this.values.Count != 0 ? this.values[0] : throw new NotSupportedException(string.Format("Attempting to parse a null value into an sql string (column:{0}).", (object) this.columnName));
        if (InFragment.Null.Equals(obj))
          sqlStringBuilder.Add(" is null");
        else if (InFragment.NotNull.Equals(obj))
          sqlStringBuilder.Add(" is not null ");
        else
          sqlStringBuilder.Add("=").AddObject(this.values[0]);
      }
      return sqlStringBuilder.ToSqlString();
    }
  }
}

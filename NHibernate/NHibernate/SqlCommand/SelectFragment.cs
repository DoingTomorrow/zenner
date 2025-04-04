// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.SelectFragment
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using NHibernate.Util;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.SqlCommand
{
  public class SelectFragment
  {
    private string suffix;
    private IList<string> columns = (IList<string>) new List<string>();
    private IList<string> columnAliases = (IList<string>) new List<string>();
    private NHibernate.Dialect.Dialect dialect;
    private string[] usedAliases;
    private string extraSelectList;

    public SelectFragment(NHibernate.Dialect.Dialect d) => this.dialect = d;

    public SelectFragment SetUsedAliases(string[] usedAliases)
    {
      this.usedAliases = usedAliases;
      return this;
    }

    public SelectFragment SetSuffix(string suffix)
    {
      this.suffix = suffix;
      return this;
    }

    public SelectFragment AddColumn(string columnName)
    {
      this.AddColumn((string) null, columnName);
      return this;
    }

    public SelectFragment AddColumns(string[] columnNames)
    {
      for (int index = 0; index < columnNames.Length; ++index)
        this.AddColumn(columnNames[index]);
      return this;
    }

    public SelectFragment AddColumn(string tableAlias, string columnName)
    {
      return this.AddColumn(tableAlias, columnName, columnName);
    }

    public SelectFragment AddColumn(string tableAlias, string columnName, string columnAlias)
    {
      if (string.IsNullOrEmpty(tableAlias))
        this.columns.Add(columnName);
      else
        this.columns.Add(tableAlias + (object) '.' + columnName);
      this.columnAliases.Add(columnAlias);
      return this;
    }

    public SelectFragment AddColumns(string tableAlias, string[] columnNames)
    {
      for (int index = 0; index < columnNames.Length; ++index)
      {
        if (columnNames[index] != null)
          this.AddColumn(tableAlias, columnNames[index]);
      }
      return this;
    }

    public SelectFragment AddColumns(
      string tableAlias,
      string[] columnNames,
      string[] columnAliases)
    {
      for (int index = 0; index < columnNames.Length; ++index)
      {
        if (columnNames[index] != null)
          this.AddColumn(tableAlias, columnNames[index], columnAliases[index]);
      }
      return this;
    }

    public SelectFragment AddFormulas(
      string tableAlias,
      string[] formulas,
      string[] formulaAliases)
    {
      for (int index = 0; index < formulas.Length; ++index)
      {
        if (formulas[index] != null)
          this.AddFormula(tableAlias, formulas[index], formulaAliases[index]);
      }
      return this;
    }

    public SelectFragment AddFormula(string tableAlias, string formula, string formulaAlias)
    {
      this.AddColumn((string) null, StringHelper.Replace(formula, Template.Placeholder, tableAlias), formulaAlias);
      return this;
    }

    public string ToFragmentString() => this.ToSqlStringFragment();

    public string ToSqlStringFragment() => this.ToSqlStringFragment(true);

    public string ToSqlStringFragment(bool includeLeadingComma)
    {
      StringBuilder stringBuilder = new StringBuilder(this.columns.Count * 10);
      HashedSet hashedSet = new HashedSet();
      if (this.usedAliases != null)
        hashedSet.AddAll((ICollection) this.usedAliases);
      bool flag = false;
      for (int index = 0; index < this.columns.Count; ++index)
      {
        string column = this.columns[index];
        string columnAlias = this.columnAliases[index];
        if (hashedSet.Add((object) columnAlias))
        {
          if (flag || includeLeadingComma)
            stringBuilder.Append(", ");
          stringBuilder.Append(column).Append(" as ").Append(new Alias(this.suffix).ToAliasString(columnAlias, this.dialect));
          flag = true;
        }
      }
      if (this.extraSelectList != null)
      {
        if (flag || includeLeadingComma)
          stringBuilder.Append(", ");
        stringBuilder.Append(this.extraSelectList);
      }
      return stringBuilder.ToString();
    }

    public SelectFragment SetExtraSelectList(string extraSelectList)
    {
      this.extraSelectList = extraSelectList;
      return this;
    }

    public SelectFragment SetExtraSelectList(CaseFragment caseFragment, string fragmentAlias)
    {
      this.SetExtraSelectList(caseFragment.SetReturnColumnName(fragmentAlias, this.suffix).ToSqlStringFragment());
      return this;
    }
  }
}

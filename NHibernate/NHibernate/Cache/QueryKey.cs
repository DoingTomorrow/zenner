// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.QueryKey
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Cache
{
  [Serializable]
  public class QueryKey
  {
    private readonly ISessionFactoryImplementor factory;
    private readonly SqlString sqlQueryString;
    private readonly IType[] types;
    private readonly object[] values;
    private readonly int firstRow = RowSelection.NoValue;
    private readonly int maxRows = RowSelection.NoValue;
    private readonly IDictionary<string, TypedValue> namedParameters;
    private readonly ISet filters;
    private readonly IResultTransformer customTransformer;
    private readonly int hashCode;
    private int[] multiQueriesFirstRows;
    private int[] multiQueriesMaxRows;

    public QueryKey(
      ISessionFactoryImplementor factory,
      SqlString queryString,
      QueryParameters queryParameters,
      ISet filters)
    {
      this.factory = factory;
      this.sqlQueryString = queryString;
      this.types = queryParameters.PositionalParameterTypes;
      this.values = queryParameters.PositionalParameterValues;
      RowSelection rowSelection = queryParameters.RowSelection;
      if (rowSelection != null)
      {
        this.firstRow = rowSelection.FirstRow;
        this.maxRows = rowSelection.MaxRows;
      }
      else
      {
        this.firstRow = RowSelection.NoValue;
        this.maxRows = RowSelection.NoValue;
      }
      this.namedParameters = queryParameters.NamedParameters;
      this.filters = filters;
      this.customTransformer = queryParameters.ResultTransformer;
      this.hashCode = this.ComputeHashCode();
    }

    public bool HasResultTransformer => this.customTransformer != null;

    public QueryKey SetFirstRows(int[] firstRows)
    {
      this.multiQueriesFirstRows = firstRows;
      return this;
    }

    public QueryKey SetMaxRows(int[] maxRows)
    {
      this.multiQueriesMaxRows = maxRows;
      return this;
    }

    public override bool Equals(object other)
    {
      QueryKey queryKey = (QueryKey) other;
      if (!this.sqlQueryString.Equals((object) queryKey.sqlQueryString) || this.firstRow != queryKey.firstRow || this.maxRows != queryKey.maxRows || !object.Equals((object) this.customTransformer, (object) queryKey.customTransformer))
        return false;
      if (this.types == null)
      {
        if (queryKey.types != null)
          return false;
      }
      else
      {
        if (queryKey.types == null || this.types.Length != queryKey.types.Length)
          return false;
        for (int index = 0; index < this.types.Length; ++index)
        {
          if (!this.types[index].Equals((object) queryKey.types[index]) || !object.Equals(this.values[index], queryKey.values[index]))
            return false;
        }
      }
      return CollectionHelper.SetEquals(this.filters, queryKey.filters) && CollectionHelper.DictionaryEquals<string, TypedValue>(this.namedParameters, queryKey.namedParameters) && CollectionHelper.CollectionEquals<int>((ICollection<int>) this.multiQueriesFirstRows, (ICollection<int>) queryKey.multiQueriesFirstRows) && CollectionHelper.CollectionEquals<int>((ICollection<int>) this.multiQueriesMaxRows, (ICollection<int>) queryKey.multiQueriesMaxRows);
    }

    public override int GetHashCode() => this.hashCode;

    public int ComputeHashCode()
    {
      int num = 37 * (37 * (37 * 13 + this.firstRow.GetHashCode()) + this.maxRows.GetHashCode()) + (this.namedParameters == null ? 0 : CollectionHelper.GetHashCode<KeyValuePair<string, TypedValue>>((IEnumerable<KeyValuePair<string, TypedValue>>) this.namedParameters));
      for (int index = 0; index < this.types.Length; ++index)
        num = 37 * num + (this.types[index] == null ? 0 : this.types[index].GetHashCode());
      for (int index = 0; index < this.values.Length; ++index)
        num = 37 * num + (this.values[index] == null ? 0 : this.values[index].GetHashCode());
      if (this.multiQueriesFirstRows != null)
      {
        foreach (int multiQueriesFirstRow in this.multiQueriesFirstRows)
          num = 37 * num + multiQueriesFirstRow;
      }
      if (this.multiQueriesMaxRows != null)
      {
        foreach (int multiQueriesMaxRow in this.multiQueriesMaxRows)
          num = 37 * num + multiQueriesMaxRow;
      }
      if (this.filters != null)
      {
        foreach (object filter in (IEnumerable) this.filters)
          num = 37 * num + filter.GetHashCode();
      }
      return 37 * (37 * num + (this.customTransformer == null ? 0 : this.customTransformer.GetHashCode())) + this.sqlQueryString.GetHashCode();
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder().Append("sql: ").Append((object) this.sqlQueryString);
      Printer printer = new Printer(this.factory);
      if (this.values != null)
        stringBuilder.Append("; parameters: ").Append(printer.ToString(this.types, this.values));
      if (this.namedParameters != null)
        stringBuilder.Append("; named parameters: ").Append(printer.ToString(this.namedParameters));
      if (this.filters != null)
        stringBuilder.Append("; filters: ").Append(CollectionPrinter.ToString(this.filters));
      if (this.firstRow != RowSelection.NoValue)
        stringBuilder.Append("; first row: ").Append(this.firstRow);
      if (this.maxRows != RowSelection.NoValue)
        stringBuilder.Append("; max rows: ").Append(this.maxRows);
      if (this.multiQueriesFirstRows != null)
      {
        stringBuilder.Append("; multi queries - first rows: ");
        for (int index = 0; index < this.multiQueriesFirstRows.Length; ++index)
          stringBuilder.Append("#").Append(index).Append("=").Append(this.multiQueriesFirstRows[index]);
        stringBuilder.Append("; ");
      }
      if (this.multiQueriesMaxRows != null)
      {
        stringBuilder.Append("; multi queries - max rows: ");
        for (int index = 0; index < this.multiQueriesMaxRows.Length; ++index)
          stringBuilder.Append("#").Append(index).Append("=").Append(this.multiQueriesMaxRows[index]);
        stringBuilder.Append("; ");
      }
      return stringBuilder.ToString();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.FilterHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Dialect.Function;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Util
{
  public sealed class FilterHelper
  {
    private readonly string[] filterNames;
    private readonly string[] filterConditions;

    public FilterHelper(
      IDictionary<string, string> filters,
      NHibernate.Dialect.Dialect dialect,
      SQLFunctionRegistry sqlFunctionRegistry)
    {
      int count = filters.Count;
      this.filterNames = new string[count];
      this.filterConditions = new string[count];
      int index = 0;
      foreach (KeyValuePair<string, string> filter in (IEnumerable<KeyValuePair<string, string>>) filters)
      {
        this.filterNames[index] = filter.Key;
        this.filterConditions[index] = Template.RenderWhereStringTemplate(filter.Value, FilterImpl.MARKER, dialect, sqlFunctionRegistry);
        this.filterConditions[index] = StringHelper.Replace(this.filterConditions[index], ":", ":" + this.filterNames[index] + ".");
        ++index;
      }
    }

    public bool IsAffectedBy(IDictionary<string, IFilter> enabledFilters)
    {
      int index = 0;
      for (int length = this.filterNames.Length; index < length; ++index)
      {
        if (enabledFilters.ContainsKey(this.filterNames[index]))
          return true;
      }
      return false;
    }

    public string Render(string alias, IDictionary<string, IFilter> enabledFilters)
    {
      StringBuilder buffer = new StringBuilder();
      this.Render(buffer, alias, enabledFilters);
      return buffer.ToString();
    }

    public void Render(
      StringBuilder buffer,
      string alias,
      IDictionary<string, IFilter> enabledFilters)
    {
      if (this.filterNames == null || this.filterNames.Length <= 0)
        return;
      int index = 0;
      for (int length = this.filterNames.Length; index < length; ++index)
      {
        if (enabledFilters.ContainsKey(this.filterNames[index]))
        {
          string filterCondition = this.filterConditions[index];
          if (StringHelper.IsNotEmpty(filterCondition))
          {
            buffer.Append(" and ");
            buffer.Append(StringHelper.Replace(filterCondition, FilterImpl.MARKER, alias));
          }
        }
      }
    }

    public static IDictionary<string, IFilter> GetEnabledForManyToOne(
      IDictionary<string, IFilter> enabledFilters)
    {
      Dictionary<string, IFilter> enabledForManyToOne = new Dictionary<string, IFilter>();
      foreach (KeyValuePair<string, IFilter> enabledFilter in (IEnumerable<KeyValuePair<string, IFilter>>) enabledFilters)
      {
        if (enabledFilter.Value.FilterDefinition.UseInManyToOne)
          enabledForManyToOne.Add(enabledFilter.Key, enabledFilter.Value);
      }
      return (IDictionary<string, IFilter>) enabledForManyToOne;
    }
  }
}

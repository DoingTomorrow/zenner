// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.FilterMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class FilterMapper : IFilterMapper
  {
    private readonly HbmFilter filter;

    public FilterMapper(string filterName, HbmFilter filter)
    {
      if (filterName == null)
        throw new ArgumentNullException(nameof (filterName));
      if (string.Empty.Equals(filterName.Trim()))
        throw new ArgumentOutOfRangeException(nameof (filterName), "Invalid filter-name: the name should contain no blank characters.");
      this.filter = filter != null ? filter : throw new ArgumentNullException(nameof (filter));
      this.filter.name = filterName;
    }

    public void Condition(string sqlCondition)
    {
      if (sqlCondition == null || string.Empty.Equals(sqlCondition) || string.Empty.Equals(sqlCondition.Trim()))
      {
        this.filter.condition = (string) null;
        this.filter.Text = (string[]) null;
      }
      else
      {
        string[] strArray = sqlCondition.Split(new string[1]
        {
          Environment.NewLine
        }, StringSplitOptions.None);
        if (strArray.Length > 1)
        {
          this.filter.Text = strArray;
          this.filter.condition = (string) null;
        }
        else
        {
          this.filter.condition = sqlCondition;
          this.filter.Text = (string[]) null;
        }
      }
    }
  }
}

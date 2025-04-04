// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.FiltersBinder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cfg.XmlHbmBinding
{
  public class FiltersBinder : Binder
  {
    private readonly IFilterable filterable;

    public FiltersBinder(IFilterable filterable, Mappings mappings)
      : base(mappings)
    {
      this.filterable = filterable;
    }

    public void Bind(IEnumerable<HbmFilter> filters)
    {
      this.Bind(filters, (Action<string, string>) ((name, condition) => this.filterable.AddFilter(name, condition)));
    }

    public void Bind(IEnumerable<HbmFilter> filters, Action<string, string> addFilterDelegate)
    {
      if (filters == null)
        return;
      foreach (HbmFilter filter in filters)
      {
        string name = filter.name;
        if (name.IndexOf('.') > -1)
          throw new MappingException("Filter name can't contain the character '.'(point): " + name);
        string condition = (filter.Text.LinesToString() ?? string.Empty).Trim();
        if (string.IsNullOrEmpty(condition))
          condition = filter.condition;
        if (string.IsNullOrEmpty(condition))
        {
          FilterDefinition filterDefinition = this.mappings.GetFilterDefinition(name);
          if (filterDefinition != null)
            condition = filterDefinition.DefaultFilterCondition;
        }
        this.mappings.ExpectedFilterDefinition(this.filterable, name, condition);
        Binder.log.Debug((object) string.Format("Applying filter [{0}] as [{1}]", (object) name, (object) condition));
        addFilterDelegate(name, condition);
      }
    }
  }
}

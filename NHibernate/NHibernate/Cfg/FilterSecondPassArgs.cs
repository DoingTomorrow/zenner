// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.FilterSecondPassArgs
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Mapping;
using System;

#nullable disable
namespace NHibernate.Cfg
{
  [Serializable]
  public class FilterSecondPassArgs
  {
    public FilterSecondPassArgs(IFilterable filterable, string filterName)
    {
      if (filterable == null)
        throw new ArgumentNullException(nameof (filterable));
      if (string.IsNullOrEmpty(filterName))
        throw new ArgumentNullException(nameof (filterName));
      this.Filterable = filterable;
      this.FilterName = filterName;
    }

    public IFilterable Filterable { get; private set; }

    public string FilterName { get; private set; }
  }
}

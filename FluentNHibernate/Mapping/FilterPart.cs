// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.FilterPart
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class FilterPart : IFilter, IFilterMappingProvider
  {
    private readonly string filterName;
    private readonly string condition;
    private readonly AttributeStore attributes = new AttributeStore();

    public FilterPart(string name)
      : this(name, (string) null)
    {
    }

    public FilterPart(string name, string condition)
    {
      this.filterName = name;
      this.condition = condition;
    }

    public string Condition => this.condition;

    public string Name => this.filterName;

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (FilterPart) && this.Equals((FilterPart) obj);
    }

    FilterMapping IFilterMappingProvider.GetFilterMapping()
    {
      FilterMapping filterMapping = new FilterMapping();
      filterMapping.Set<string>((Expression<Func<FilterMapping, string>>) (x => x.Name), 0, this.Name);
      filterMapping.Set<string>((Expression<Func<FilterMapping, string>>) (x => x.Condition), 0, this.Condition);
      return filterMapping;
    }

    public bool Equals(FilterPart other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return object.Equals((object) other.filterName, (object) this.filterName) && object.Equals((object) other.condition, (object) this.condition) && object.Equals((object) other.attributes, (object) this.attributes);
    }

    public override int GetHashCode()
    {
      return ((this.filterName != null ? this.filterName.GetHashCode() : 0) * 397 ^ (this.condition != null ? this.condition.GetHashCode() : 0)) * 397 ^ (this.attributes != null ? this.attributes.GetHashCode() : 0);
    }
  }
}

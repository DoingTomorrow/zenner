// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.FilterDefinition
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public abstract class FilterDefinition : IFilterDefinition
  {
    private string filterName;
    private string filterCondition;
    private readonly IDictionary<string, IType> parameters;

    protected FilterDefinition()
    {
      this.parameters = (IDictionary<string, IType>) new Dictionary<string, IType>();
    }

    public string Name => this.filterName;

    public IEnumerable<KeyValuePair<string, IType>> Parameters
    {
      get => (IEnumerable<KeyValuePair<string, IType>>) this.parameters;
    }

    public FilterDefinition WithName(string name)
    {
      this.filterName = name;
      return this;
    }

    public FilterDefinition WithCondition(string condition)
    {
      this.filterCondition = condition;
      return this;
    }

    public FilterDefinition AddParameter(string name, IType type)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentException("The name is mandatory", nameof (name));
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      this.parameters.Add(name, type);
      return this;
    }

    FilterDefinitionMapping IFilterDefinition.GetFilterMapping()
    {
      FilterDefinitionMapping filterMapping = new FilterDefinitionMapping();
      filterMapping.Set<string>((Expression<Func<FilterDefinitionMapping, string>>) (x => x.Name), 0, this.filterName);
      filterMapping.Set<string>((Expression<Func<FilterDefinitionMapping, string>>) (x => x.Condition), 0, this.filterCondition);
      foreach (KeyValuePair<string, IType> parameter in this.Parameters)
        filterMapping.Parameters.Add(parameter);
      return filterMapping;
    }

    HibernateMapping IFilterDefinition.GetHibernateMapping() => new HibernateMapping();
  }
}

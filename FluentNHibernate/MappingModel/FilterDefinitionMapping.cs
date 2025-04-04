// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.FilterDefinitionMapping
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.MappingModel
{
  [Serializable]
  public class FilterDefinitionMapping : MappingBase
  {
    private readonly AttributeStore attributes;
    private readonly IDictionary<string, IType> parameters;

    public FilterDefinitionMapping()
      : this(new AttributeStore())
    {
    }

    public FilterDefinitionMapping(AttributeStore attributes)
    {
      this.attributes = attributes;
      this.parameters = (IDictionary<string, IType>) new Dictionary<string, IType>();
    }

    public IDictionary<string, IType> Parameters => this.parameters;

    public string Name => this.attributes.GetOrDefault<string>(nameof (Name));

    public string Condition => this.attributes.GetOrDefault<string>(nameof (Condition));

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessFilterDefinition(this);
    }

    public bool Equals(FilterDefinitionMapping other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return object.Equals((object) other.attributes, (object) this.attributes) && other.parameters.ContentEquals<string, IType>(this.parameters);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (FilterDefinitionMapping) && this.Equals((FilterDefinitionMapping) obj);
    }

    public override int GetHashCode()
    {
      return (this.attributes != null ? this.attributes.GetHashCode() : 0) * 397 ^ (this.parameters != null ? this.parameters.GetHashCode() : 0);
    }

    public void Set<T>(
      Expression<Func<FilterDefinitionMapping, T>> expression,
      int layer,
      T value)
    {
      this.Set(expression.ToMember<FilterDefinitionMapping, T>().Name, layer, (object) value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);
  }
}

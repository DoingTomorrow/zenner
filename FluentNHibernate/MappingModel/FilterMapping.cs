// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.FilterMapping
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.MappingModel
{
  [Serializable]
  public class FilterMapping : IMapping
  {
    private readonly AttributeStore attributes;

    public FilterMapping()
      : this(new AttributeStore())
    {
    }

    public FilterMapping(AttributeStore attributes) => this.attributes = attributes;

    public string Name => this.attributes.GetOrDefault<string>(nameof (Name));

    public string Condition => this.attributes.GetOrDefault<string>(nameof (Condition));

    public void AcceptVisitor(IMappingModelVisitor visitor) => visitor.ProcessFilter(this);

    public bool IsSpecified(string property) => this.attributes.IsSpecified(property);

    public bool Equals(FilterMapping other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      return object.ReferenceEquals((object) this, (object) other) || object.Equals((object) other.attributes, (object) this.attributes);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (FilterMapping) && this.Equals((FilterMapping) obj);
    }

    public override int GetHashCode()
    {
      return this.attributes == null ? 0 : this.attributes.GetHashCode();
    }

    public void Set<T>(Expression<Func<FilterMapping, T>> expression, int layer, T value)
    {
      this.Set(expression.ToMember<FilterMapping, T>().Name, layer, (object) value);
    }

    public void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.OneToOneMapping
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
  public class OneToOneMapping : MappingBase
  {
    private readonly AttributeStore attributes;

    public OneToOneMapping()
      : this(new AttributeStore())
    {
    }

    public OneToOneMapping(AttributeStore attributes) => this.attributes = attributes;

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessOneToOne(this);
    }

    public string Name => this.attributes.GetOrDefault<string>(nameof (Name));

    public string Access => this.attributes.GetOrDefault<string>(nameof (Access));

    public TypeReference Class => this.attributes.GetOrDefault<TypeReference>(nameof (Class));

    public string Cascade => this.attributes.GetOrDefault<string>(nameof (Cascade));

    public bool Constrained => this.attributes.GetOrDefault<bool>(nameof (Constrained));

    public string Fetch => this.attributes.GetOrDefault<string>(nameof (Fetch));

    public string ForeignKey => this.attributes.GetOrDefault<string>(nameof (ForeignKey));

    public string PropertyRef => this.attributes.GetOrDefault<string>(nameof (PropertyRef));

    public string Lazy => this.attributes.GetOrDefault<string>(nameof (Lazy));

    public string EntityName => this.attributes.GetOrDefault<string>(nameof (EntityName));

    public Type ContainingEntityType { get; set; }

    public bool Equals(OneToOneMapping other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return object.Equals((object) other.attributes, (object) this.attributes) && object.Equals((object) other.ContainingEntityType, (object) this.ContainingEntityType);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (OneToOneMapping) && this.Equals((OneToOneMapping) obj);
    }

    public override int GetHashCode()
    {
      return (this.attributes != null ? this.attributes.GetHashCode() : 0) * 397 ^ (this.ContainingEntityType != null ? this.ContainingEntityType.GetHashCode() : 0);
    }

    public void Set<T>(Expression<Func<OneToOneMapping, T>> expression, int layer, T value)
    {
      this.Set(expression.ToMember<OneToOneMapping, T>().Name, layer, (object) value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);
  }
}

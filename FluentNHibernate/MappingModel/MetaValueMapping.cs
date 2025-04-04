// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.MetaValueMapping
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
  public class MetaValueMapping : MappingBase
  {
    private readonly AttributeStore attributes;

    public MetaValueMapping()
      : this(new AttributeStore())
    {
    }

    protected MetaValueMapping(AttributeStore attributes) => this.attributes = attributes;

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessMetaValue(this);
    }

    public string Value => this.attributes.GetOrDefault<string>(nameof (Value));

    public TypeReference Class => this.attributes.GetOrDefault<TypeReference>(nameof (Class));

    public Type ContainingEntityType { get; set; }

    public bool Equals(MetaValueMapping other)
    {
      return object.Equals((object) other.attributes, (object) this.attributes) && object.Equals((object) other.ContainingEntityType, (object) this.ContainingEntityType);
    }

    public override bool Equals(object obj)
    {
      return obj.GetType() == typeof (MetaValueMapping) && this.Equals((MetaValueMapping) obj);
    }

    public override int GetHashCode()
    {
      return (this.attributes != null ? this.attributes.GetHashCode() : 0) * 397 ^ (this.ContainingEntityType != null ? this.ContainingEntityType.GetHashCode() : 0);
    }

    public void Set<T>(Expression<Func<MetaValueMapping, T>> expression, int layer, T value)
    {
      this.Set(expression.ToMember<MetaValueMapping, T>().Name, layer, (object) value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);
  }
}

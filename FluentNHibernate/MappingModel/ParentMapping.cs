// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.ParentMapping
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
  public class ParentMapping : MappingBase
  {
    private readonly AttributeStore attributes;

    public ParentMapping()
      : this(new AttributeStore())
    {
    }

    protected ParentMapping(AttributeStore attributes) => this.attributes = attributes;

    public override void AcceptVisitor(IMappingModelVisitor visitor) => visitor.ProcessParent(this);

    public string Name => this.attributes.GetOrDefault<string>(nameof (Name));

    public Type ContainingEntityType { get; set; }

    public override bool Equals(object obj)
    {
      return obj.GetType() == typeof (ParentMapping) && this.Equals((ParentMapping) obj);
    }

    public bool Equals(ParentMapping other)
    {
      return object.Equals((object) other.attributes, (object) this.attributes) && object.Equals((object) other.ContainingEntityType, (object) this.ContainingEntityType);
    }

    public override int GetHashCode()
    {
      return (this.attributes != null ? this.attributes.GetHashCode() : 0) * 397 ^ (this.ContainingEntityType != null ? this.ContainingEntityType.GetHashCode() : 0);
    }

    public void Set<T>(Expression<Func<ParentMapping, T>> expression, int layer, T value)
    {
      this.Set(expression.ToMember<ParentMapping, T>().Name, layer, (object) value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);
  }
}

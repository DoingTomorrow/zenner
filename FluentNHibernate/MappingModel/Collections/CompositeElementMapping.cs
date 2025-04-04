// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Collections.CompositeElementMapping
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.MappingModel.Collections
{
  [Serializable]
  public class CompositeElementMapping : MappingBase
  {
    private readonly MappedMembers mappedMembers;
    private readonly List<NestedCompositeElementMapping> compositeElements = new List<NestedCompositeElementMapping>();
    private readonly AttributeStore attributes;

    public CompositeElementMapping()
      : this(new AttributeStore())
    {
    }

    public CompositeElementMapping(AttributeStore attributes)
    {
      this.attributes = attributes;
      this.mappedMembers = new MappedMembers();
    }

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessCompositeElement(this);
      if (this.Parent != null)
        visitor.Visit(this.Parent);
      foreach (NestedCompositeElementMapping compositeElement in this.CompositeElements)
        visitor.Visit((CompositeElementMapping) compositeElement);
      this.mappedMembers.AcceptVisitor(visitor);
    }

    public TypeReference Class => this.attributes.GetOrDefault<TypeReference>(nameof (Class));

    public ParentMapping Parent => this.attributes.GetOrDefault<ParentMapping>(nameof (Parent));

    public IEnumerable<PropertyMapping> Properties => this.mappedMembers.Properties;

    public void AddProperty(PropertyMapping property) => this.mappedMembers.AddProperty(property);

    public IEnumerable<ManyToOneMapping> References => this.mappedMembers.References;

    public IEnumerable<NestedCompositeElementMapping> CompositeElements
    {
      get => (IEnumerable<NestedCompositeElementMapping>) this.compositeElements;
    }

    public Type ContainingEntityType { get; set; }

    public void AddReference(ManyToOneMapping manyToOne)
    {
      this.mappedMembers.AddReference(manyToOne);
    }

    public void AddCompositeElement(NestedCompositeElementMapping compositeElement)
    {
      this.compositeElements.Add(compositeElement);
    }

    public bool Equals(CompositeElementMapping other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return object.Equals((object) other.mappedMembers, (object) this.mappedMembers) && object.Equals((object) other.attributes, (object) this.attributes) && object.Equals((object) other.ContainingEntityType, (object) this.ContainingEntityType);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (CompositeElementMapping) && this.Equals((CompositeElementMapping) obj);
    }

    public override int GetHashCode()
    {
      return ((this.mappedMembers != null ? this.mappedMembers.GetHashCode() : 0) * 397 ^ (this.attributes != null ? this.attributes.GetHashCode() : 0)) * 397 ^ (this.ContainingEntityType != null ? this.ContainingEntityType.GetHashCode() : 0);
    }

    public void Set<T>(
      Expression<Func<CompositeElementMapping, T>> expression,
      int layer,
      T value)
    {
      this.Set(expression.ToMember<CompositeElementMapping, T>().Name, layer, (object) value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);
  }
}

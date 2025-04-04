// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.ClassBased.ComponentMapping
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.MappingModel.ClassBased
{
  [Serializable]
  public class ComponentMapping : ComponentMappingBase, IComponentMapping, IMapping
  {
    private readonly AttributeStore attributes;

    public ComponentType ComponentType { get; set; }

    public ComponentMapping(ComponentType componentType)
      : this(componentType, new AttributeStore())
    {
    }

    public ComponentMapping(ComponentType componentType, AttributeStore attributes)
      : base(attributes)
    {
      this.ComponentType = componentType;
      this.attributes = attributes;
    }

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessComponent(this);
      base.AcceptVisitor(visitor);
    }

    public bool HasColumnPrefix => !string.IsNullOrEmpty(this.ColumnPrefix);

    public string ColumnPrefix { get; set; }

    public override string Name => this.attributes.GetOrDefault<string>(nameof (Name));

    public override Type Type => this.attributes.GetOrDefault<Type>(nameof (Type));

    public TypeReference Class => this.attributes.GetOrDefault<TypeReference>(nameof (Class));

    public bool Lazy => this.attributes.GetOrDefault<bool>(nameof (Lazy));

    public bool Equals(ComponentMapping other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return this.Equals((ComponentMappingBase) other) && object.Equals((object) other.attributes, (object) this.attributes);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      return object.ReferenceEquals((object) this, obj) || this.Equals(obj as ComponentMapping);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() * 397 ^ (this.attributes != null ? this.attributes.GetHashCode() : 0);
    }

    public void Set<T>(Expression<Func<ComponentMapping, T>> expression, int layer, T value)
    {
      this.Set(expression.ToMember<ComponentMapping, T>().Name, layer, (object) value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);
  }
}

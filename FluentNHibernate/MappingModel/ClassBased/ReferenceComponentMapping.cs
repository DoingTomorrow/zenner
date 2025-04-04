// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.ClassBased.ReferenceComponentMapping
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Visitors;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.MappingModel.ClassBased
{
  [Serializable]
  public class ReferenceComponentMapping : IComponentMapping, IMapping
  {
    private readonly Member property;
    private readonly Type componentType;
    private ExternalComponentMapping mergedComponent;
    private Type containingEntityType;

    public ComponentType ComponentType { get; set; }

    public ReferenceComponentMapping(
      ComponentType componentType,
      Member property,
      Type componentEntityType,
      Type containingEntityType,
      string columnPrefix)
    {
      this.ComponentType = componentType;
      this.property = property;
      this.componentType = componentEntityType;
      this.containingEntityType = containingEntityType;
      this.ColumnPrefix = columnPrefix;
    }

    public void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessComponent(this);
      if (this.mergedComponent == null)
        return;
      this.mergedComponent.AcceptVisitor(visitor);
    }

    public bool IsSpecified(string name)
    {
      return this.IsAssociated && this.mergedComponent.IsSpecified(name);
    }

    public void Set(string attribute, int layer, object value)
    {
      ((IMapping) this.mergedComponent).Set(attribute, layer, value);
    }

    public virtual void AssociateExternalMapping(ExternalComponentMapping mapping)
    {
      this.mergedComponent = mapping;
      this.mergedComponent.Member = this.property;
      this.mergedComponent.Set<string>((Expression<Func<ComponentMapping, string>>) (x => x.Name), 0, this.property.Name);
      this.mergedComponent.Set<TypeReference>((Expression<Func<ComponentMapping, TypeReference>>) (x => x.Class), 0, new TypeReference(this.componentType));
      this.mergedComponent.Set<Type>((Expression<Func<ComponentMapping, Type>>) (x => x.Type), 0, this.componentType);
    }

    public IEnumerable<ManyToOneMapping> References => this.mergedComponent.References;

    public IEnumerable<CollectionMapping> Collections => this.mergedComponent.Collections;

    public IEnumerable<PropertyMapping> Properties => this.mergedComponent.Properties;

    public IEnumerable<IComponentMapping> Components => this.mergedComponent.Components;

    public IEnumerable<OneToOneMapping> OneToOnes => this.mergedComponent.OneToOnes;

    public IEnumerable<AnyMapping> Anys => this.mergedComponent.Anys;

    public void AddProperty(PropertyMapping property) => this.mergedComponent.AddProperty(property);

    public void AddCollection(CollectionMapping collection)
    {
      this.mergedComponent.AddCollection(collection);
    }

    public void AddReference(ManyToOneMapping manyToOne)
    {
      this.mergedComponent.AddReference(manyToOne);
    }

    public void AddComponent(IComponentMapping componentMapping)
    {
      this.mergedComponent.AddComponent(componentMapping);
    }

    public void AddOneToOne(OneToOneMapping mapping) => this.mergedComponent.AddOneToOne(mapping);

    public void AddAny(AnyMapping mapping) => this.mergedComponent.AddAny(mapping);

    public Type ContainingEntityType
    {
      get => this.containingEntityType;
      set => this.containingEntityType = value;
    }

    public Member Member
    {
      get => this.mergedComponent != null ? this.mergedComponent.Member : this.property;
    }

    public ParentMapping Parent => this.mergedComponent.Parent;

    public bool Unique => this.mergedComponent.Unique;

    public bool HasColumnPrefix => !string.IsNullOrEmpty(this.ColumnPrefix);

    public string ColumnPrefix { get; set; }

    public bool Insert => this.mergedComponent.Insert;

    public bool Update => this.mergedComponent.Update;

    public string Access => this.mergedComponent.Access;

    public bool OptimisticLock => this.mergedComponent.OptimisticLock;

    public string Name
    {
      get => this.mergedComponent != null ? this.mergedComponent.Name : this.property.Name;
    }

    public Type Type
    {
      get => this.mergedComponent != null ? this.mergedComponent.Type : this.componentType;
    }

    public TypeReference Class => this.mergedComponent.Class;

    public bool Lazy => this.mergedComponent.Lazy;

    public bool IsAssociated => this.mergedComponent != null;

    public ComponentMapping MergedModel => (ComponentMapping) this.mergedComponent;

    public bool Equals(ReferenceComponentMapping other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return object.Equals((object) other.property, (object) this.property) && object.Equals((object) other.componentType, (object) this.componentType) && object.Equals((object) other.mergedComponent, (object) this.mergedComponent) && object.Equals((object) other.containingEntityType, (object) this.containingEntityType);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (ReferenceComponentMapping) && this.Equals((ReferenceComponentMapping) obj);
    }

    public override int GetHashCode()
    {
      return (((this.property != (Member) null ? this.property.GetHashCode() : 0) * 397 ^ (this.componentType != null ? this.componentType.GetHashCode() : 0)) * 397 ^ (this.mergedComponent != null ? this.mergedComponent.GetHashCode() : 0)) * 397 ^ (this.containingEntityType != null ? this.containingEntityType.GetHashCode() : 0);
    }
  }
}

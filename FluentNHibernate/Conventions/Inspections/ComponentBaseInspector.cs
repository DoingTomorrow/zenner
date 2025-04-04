// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.ComponentBaseInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public abstract class ComponentBaseInspector : 
    IComponentBaseInspector,
    IAccessInspector,
    IExposedThroughPropertyInspector,
    IInspector
  {
    private readonly ComponentMapping mapping;

    public ComponentBaseInspector(ComponentMapping mapping) => this.mapping = mapping;

    public Access Access => Access.FromString(this.mapping.Access);

    public Type EntityType => this.mapping.ContainingEntityType;

    public string StringIdentifierForModel => this.mapping.Name;

    public abstract bool IsSet(Member property);

    public Member Property => this.mapping.Member;

    public IParentInspector Parent
    {
      get
      {
        return this.mapping.Parent == null ? (IParentInspector) new ParentInspector(new ParentMapping()) : (IParentInspector) new ParentInspector(this.mapping.Parent);
      }
    }

    public bool Insert => this.mapping.Insert;

    public bool Update => this.mapping.Update;

    public IEnumerable<IAnyInspector> Anys
    {
      get
      {
        return this.mapping.Anys.Select<AnyMapping, AnyInspector>((Func<AnyMapping, AnyInspector>) (x => new AnyInspector(x))).Cast<IAnyInspector>();
      }
    }

    public IEnumerable<ICollectionInspector> Collections
    {
      get
      {
        return this.mapping.Collections.Select<CollectionMapping, CollectionInspector>((Func<CollectionMapping, CollectionInspector>) (x => new CollectionInspector(x))).Cast<ICollectionInspector>();
      }
    }

    public IEnumerable<IComponentBaseInspector> Components
    {
      get
      {
        return this.mapping.Components.Select<IComponentMapping, IComponentBaseInspector>((Func<IComponentMapping, IComponentBaseInspector>) (x => x.ComponentType == ComponentType.Component ? (IComponentBaseInspector) new ComponentInspector((ComponentMapping) x) : (IComponentBaseInspector) new DynamicComponentInspector((ComponentMapping) x)));
      }
    }

    public string Name => this.mapping.Name;

    public bool OptimisticLock => this.mapping.OptimisticLock;

    public bool Unique => this.mapping.Unique;

    public TypeReference Class => this.mapping == null ? (TypeReference) null : this.mapping.Class;

    public IEnumerable<IOneToOneInspector> OneToOnes
    {
      get
      {
        return this.mapping.OneToOnes.Select<OneToOneMapping, OneToOneInspector>((Func<OneToOneMapping, OneToOneInspector>) (x => new OneToOneInspector(x))).Cast<IOneToOneInspector>();
      }
    }

    public IEnumerable<IPropertyInspector> Properties
    {
      get
      {
        return this.mapping.Properties.Select<PropertyMapping, PropertyInspector>((Func<PropertyMapping, PropertyInspector>) (x => new PropertyInspector(x))).Cast<IPropertyInspector>();
      }
    }

    public IEnumerable<IManyToOneInspector> References
    {
      get
      {
        return this.mapping.References.Select<ManyToOneMapping, ManyToOneInspector>((Func<ManyToOneMapping, ManyToOneInspector>) (x => new ManyToOneInspector(x))).Cast<IManyToOneInspector>();
      }
    }

    public Type Type => this.mapping.Type;
  }
}

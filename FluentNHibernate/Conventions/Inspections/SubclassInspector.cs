// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.SubclassInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class SubclassInspector : ISubclassInspector, ISubclassInspectorBase, IInspector
  {
    private readonly InspectorModelMapper<ISubclassInspector, SubclassMapping> mappedProperties = new InspectorModelMapper<ISubclassInspector, SubclassMapping>();
    private readonly SubclassMapping mapping;

    public SubclassInspector(SubclassMapping mapping)
    {
      this.mapping = mapping;
      this.mappedProperties.Map((Expression<Func<ISubclassInspector, object>>) (x => (object) x.LazyLoad), (Expression<Func<SubclassMapping, object>>) (x => (object) x.Lazy));
    }

    public Type EntityType => this.mapping.Type;

    public string StringIdentifierForModel => this.mapping.Name;

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.mappedProperties.Get(property));
    }

    public bool Abstract => this.mapping.Abstract;

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

    public object DiscriminatorValue => this.mapping.DiscriminatorValue;

    public bool DynamicInsert => this.mapping.DynamicInsert;

    public bool DynamicUpdate => this.mapping.DynamicUpdate;

    public Type Extends => this.mapping.Extends;

    public IEnumerable<IJoinInspector> Joins
    {
      get
      {
        return this.mapping.Joins.Select<JoinMapping, JoinInspector>((Func<JoinMapping, JoinInspector>) (x => new JoinInspector(x))).Cast<IJoinInspector>();
      }
    }

    public bool LazyLoad => this.mapping.Lazy;

    public string Name => this.mapping.Name;

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

    public string Proxy => this.mapping.Proxy;

    public IEnumerable<IManyToOneInspector> References
    {
      get
      {
        return this.mapping.References.Select<ManyToOneMapping, ManyToOneInspector>((Func<ManyToOneMapping, ManyToOneInspector>) (x => new ManyToOneInspector(x))).Cast<IManyToOneInspector>();
      }
    }

    public bool SelectBeforeUpdate => this.mapping.SelectBeforeUpdate;

    public IEnumerable<ISubclassInspector> Subclasses
    {
      get
      {
        return this.mapping.Subclasses.Select<SubclassMapping, SubclassInspector>((Func<SubclassMapping, SubclassInspector>) (x => new SubclassInspector(x))).Cast<ISubclassInspector>();
      }
    }

    IEnumerable<ISubclassInspectorBase> ISubclassInspectorBase.Subclasses
    {
      get => this.Subclasses.Cast<ISubclassInspectorBase>();
    }

    public Type Type => this.mapping.Type;
  }
}

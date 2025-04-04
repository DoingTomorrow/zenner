// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.ClassInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class ClassInspector : IClassInspector, ILazyLoadInspector, IReadOnlyInspector, IInspector
  {
    private readonly ClassMapping mapping;
    private readonly InspectorModelMapper<IClassInspector, ClassMapping> propertyMappings = new InspectorModelMapper<IClassInspector, ClassMapping>();

    public ClassInspector(ClassMapping mapping)
    {
      this.mapping = mapping;
      this.propertyMappings.Map((Expression<Func<IClassInspector, object>>) (x => (object) x.LazyLoad), (Expression<Func<ClassMapping, object>>) (x => (object) x.Lazy));
      this.propertyMappings.Map((Expression<Func<IClassInspector, object>>) (x => (object) x.ReadOnly), (Expression<Func<ClassMapping, object>>) (x => (object) x.Mutable));
      this.propertyMappings.Map((Expression<Func<IClassInspector, object>>) (x => x.EntityType), (Expression<Func<ClassMapping, object>>) (x => x.Type));
    }

    public Type EntityType => this.mapping.Type;

    public string StringIdentifierForModel => this.mapping.Name;

    public bool LazyLoad => this.mapping.Lazy;

    public bool ReadOnly => !this.mapping.Mutable;

    public string TableName => this.mapping.TableName;

    ICacheInspector IClassInspector.Cache => (ICacheInspector) this.Cache;

    public ICacheInstance Cache
    {
      get
      {
        if (this.mapping.Cache == null)
          this.mapping.Set<CacheMapping>((Expression<Func<ClassMapping, CacheMapping>>) (x => x.Cache), 1, new CacheMapping());
        return (ICacheInstance) new CacheInstance(this.mapping.Cache);
      }
    }

    public OptimisticLock OptimisticLock => OptimisticLock.FromString(this.mapping.OptimisticLock);

    public SchemaAction SchemaAction => SchemaAction.FromString(this.mapping.SchemaAction);

    public string Schema => this.mapping.Schema;

    public bool DynamicUpdate => this.mapping.DynamicUpdate;

    public bool DynamicInsert => this.mapping.DynamicInsert;

    public int BatchSize => this.mapping.BatchSize;

    public bool Abstract => this.mapping.Abstract;

    public IVersionInspector Version
    {
      get
      {
        return this.mapping.Version == null ? (IVersionInspector) new VersionInspector(new VersionMapping()) : (IVersionInspector) new VersionInspector(this.mapping.Version);
      }
    }

    public IEnumerable<IAnyInspector> Anys
    {
      get
      {
        return this.mapping.Anys.Select<AnyMapping, AnyInspector>((Func<AnyMapping, AnyInspector>) (x => new AnyInspector(x))).Cast<IAnyInspector>();
      }
    }

    public string Check => this.mapping.Check;

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

    public IEnumerable<IJoinInspector> Joins
    {
      get
      {
        return this.mapping.Joins.Select<JoinMapping, JoinInspector>((Func<JoinMapping, JoinInspector>) (x => new JoinInspector(x))).Cast<IJoinInspector>();
      }
    }

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

    public IEnumerable<ISubclassInspectorBase> Subclasses
    {
      get
      {
        return this.mapping.Subclasses.Where<SubclassMapping>((Func<SubclassMapping, bool>) (x => x.SubclassType == SubclassType.Subclass)).Select<SubclassMapping, ISubclassInspectorBase>((Func<SubclassMapping, ISubclassInspectorBase>) (x => (ISubclassInspectorBase) new SubclassInspector(x)));
      }
    }

    public IDiscriminatorInspector Discriminator
    {
      get
      {
        return this.mapping.Discriminator == null ? (IDiscriminatorInspector) new DiscriminatorInspector(new DiscriminatorMapping()) : (IDiscriminatorInspector) new DiscriminatorInspector(this.mapping.Discriminator);
      }
    }

    public object DiscriminatorValue => this.mapping.DiscriminatorValue;

    public string Name => this.mapping.Name;

    public string Persister => this.mapping.Persister;

    public Polymorphism Polymorphism => Polymorphism.FromString(this.mapping.Polymorphism);

    public string Proxy => this.mapping.Proxy;

    public string Where => this.mapping.Where;

    public string Subselect => this.mapping.Subselect;

    public bool SelectBeforeUpdate => this.mapping.SelectBeforeUpdate;

    public IIdentityInspectorBase Id
    {
      get
      {
        if (this.mapping.Id == null)
          return (IIdentityInspectorBase) new IdentityInspector(new IdMapping());
        return this.mapping.Id is CompositeIdMapping ? (IIdentityInspectorBase) new CompositeIdentityInspector((CompositeIdMapping) this.mapping.Id) : (IIdentityInspectorBase) new IdentityInspector((IdMapping) this.mapping.Id);
      }
    }

    public bool IsSet(Member property)
    {
      return this.mapping.IsSpecified(this.propertyMappings.Get(property));
    }
  }
}

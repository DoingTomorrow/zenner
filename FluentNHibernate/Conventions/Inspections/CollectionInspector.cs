// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Inspections.CollectionInspector
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace FluentNHibernate.Conventions.Inspections
{
  public class CollectionInspector : 
    IArrayInspector,
    IBagInspector,
    IListInspector,
    IMapInspector,
    ISetInspector,
    ICollectionInspector,
    IInspector
  {
    private InspectorModelMapper<ICollectionInspector, CollectionMapping> propertyMappings = new InspectorModelMapper<ICollectionInspector, CollectionMapping>();
    private CollectionMapping mapping;

    public CollectionInspector(CollectionMapping mapping)
    {
      this.mapping = mapping;
      this.propertyMappings.Map((Expression<Func<ICollectionInspector, object>>) (x => (object) x.LazyLoad), (Expression<Func<CollectionMapping, object>>) (x => (object) x.Lazy));
    }

    public Type EntityType => this.mapping.ContainingEntityType;

    public string StringIdentifierForModel => this.mapping.Name;

    Collection ICollectionInspector.Collection => this.mapping.Collection;

    public bool IsSet(FluentNHibernate.Member property)
    {
      return this.mapping.IsSpecified(this.propertyMappings.Get(property));
    }

    public IKeyInspector Key
    {
      get
      {
        return this.mapping.Key == null ? (IKeyInspector) new KeyInspector(new KeyMapping()) : (IKeyInspector) new KeyInspector(this.mapping.Key);
      }
    }

    public string TableName => this.mapping.TableName;

    public bool IsMethodAccess => this.mapping.Member.IsMethod;

    public MemberInfo Member => this.mapping.Member.MemberInfo;

    public IRelationshipInspector Relationship
    {
      get
      {
        return this.mapping.Relationship is ManyToManyMapping ? (IRelationshipInspector) new ManyToManyInspector((ManyToManyMapping) this.mapping.Relationship) : (IRelationshipInspector) new OneToManyInspector((OneToManyMapping) this.mapping.Relationship);
      }
    }

    public Cascade Cascade => Cascade.FromString(this.mapping.Cascade);

    public Fetch Fetch => Fetch.FromString(this.mapping.Fetch);

    public bool OptimisticLock => this.mapping.OptimisticLock;

    public bool Generic => this.mapping.Generic;

    public bool Inverse => this.mapping.Inverse;

    public Access Access => Access.FromString(this.mapping.Access);

    public int BatchSize => this.mapping.BatchSize;

    public ICacheInspector Cache
    {
      get
      {
        return this.mapping.Cache == null ? (ICacheInspector) new CacheInspector(new CacheMapping()) : (ICacheInspector) new CacheInspector(this.mapping.Cache);
      }
    }

    public string Check => this.mapping.Check;

    public Type ChildType => this.mapping.ChildType;

    public TypeReference CollectionType => this.mapping.CollectionType;

    public ICompositeElementInspector CompositeElement
    {
      get
      {
        return this.mapping.CompositeElement == null ? (ICompositeElementInspector) new CompositeElementInspector(new CompositeElementMapping()) : (ICompositeElementInspector) new CompositeElementInspector(this.mapping.CompositeElement);
      }
    }

    public IElementInspector Element
    {
      get
      {
        return this.mapping.Element == null ? (IElementInspector) new ElementInspector(new ElementMapping()) : (IElementInspector) new ElementInspector(this.mapping.Element);
      }
    }

    public Lazy LazyLoad => this.mapping.Lazy;

    public string Name => this.mapping.Name;

    public TypeReference Persister => this.mapping.Persister;

    public string Schema => this.mapping.Schema;

    public string Where => this.mapping.Where;

    public string OrderBy => this.mapping.OrderBy;

    public string Sort => this.mapping.Sort;

    public IIndexInspectorBase Index
    {
      get
      {
        if (this.mapping.Index == null)
          return (IIndexInspectorBase) new IndexInspector(new IndexMapping());
        if (this.mapping.Index is IndexMapping)
          return (IIndexInspectorBase) new IndexInspector(this.mapping.Index as IndexMapping);
        return this.mapping.Index is IndexManyToManyMapping ? (IIndexInspectorBase) new IndexManyToManyInspector(this.mapping.Index as IndexManyToManyMapping) : throw new InvalidOperationException("This IIndexMapping is not a valid type for inspecting");
      }
    }

    public virtual void ExtraLazyLoad() => throw new NotImplementedException();
  }
}

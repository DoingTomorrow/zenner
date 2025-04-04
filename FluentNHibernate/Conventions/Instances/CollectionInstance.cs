// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.CollectionInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class CollectionInstance : 
    CollectionInspector,
    IArrayInstance,
    IArrayInspector,
    IBagInstance,
    IBagInspector,
    IListInstance,
    IListInspector,
    IMapInstance,
    IMapInspector,
    ISetInstance,
    ISetInspector,
    ICollectionInstance,
    ICollectionInspector,
    IInspector
  {
    private readonly CollectionMapping mapping;
    protected bool nextBool = true;

    public CollectionInstance(CollectionMapping mapping)
      : base(mapping)
    {
      this.mapping = mapping;
    }

    public IRelationshipInstance Relationship
    {
      get
      {
        return this.mapping.Relationship is ManyToManyMapping ? (IRelationshipInstance) new ManyToManyInstance((ManyToManyMapping) this.mapping.Relationship) : (IRelationshipInstance) new OneToManyInstance((OneToManyMapping) this.mapping.Relationship);
      }
    }

    public ICollectionCascadeInstance Cascade
    {
      get
      {
        return (ICollectionCascadeInstance) new CollectionCascadeInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.Cascade), 1, value)));
      }
    }

    public IFetchInstance Fetch
    {
      get
      {
        return (IFetchInstance) new FetchInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.Fetch), 1, value)));
      }
    }

    public void OptimisticLock()
    {
      this.mapping.Set<bool>((Expression<Func<CollectionMapping, bool>>) (x => x.OptimisticLock), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void Check(string constraint)
    {
      this.mapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.Check), 1, constraint);
    }

    public void CollectionType<T>() => this.CollectionType(typeof (T));

    public void CollectionType(string type)
    {
      this.mapping.Set<TypeReference>((Expression<Func<CollectionMapping, TypeReference>>) (x => x.CollectionType), 1, new TypeReference(type));
    }

    public void CollectionType(Type type)
    {
      this.mapping.Set<TypeReference>((Expression<Func<CollectionMapping, TypeReference>>) (x => x.CollectionType), 1, new TypeReference(type));
    }

    public void Generic()
    {
      this.mapping.Set<bool>((Expression<Func<CollectionMapping, bool>>) (x => x.Generic), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void Inverse()
    {
      this.mapping.Set<bool>((Expression<Func<CollectionMapping, bool>>) (x => x.Inverse), 1, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void Persister<T>()
    {
      this.mapping.Set<TypeReference>((Expression<Func<CollectionMapping, TypeReference>>) (x => x.Persister), 1, new TypeReference(typeof (T)));
    }

    public void Where(string whereClause)
    {
      this.mapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.Where), 1, whereClause);
    }

    public IIndexInstanceBase Index
    {
      get
      {
        if (this.mapping.Index == null)
          return (IIndexInstanceBase) new IndexInstance(new IndexMapping());
        if (this.mapping.Index is IndexMapping)
          return (IIndexInstanceBase) new IndexInstance(this.mapping.Index as IndexMapping);
        return this.mapping.Index is IndexManyToManyMapping ? (IIndexInstanceBase) new IndexManyToManyInstance(this.mapping.Index as IndexManyToManyMapping) : throw new InvalidOperationException("This IIndexMapping is not a valid type for inspecting");
      }
    }

    public void OrderBy(string orderBy)
    {
      this.mapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.OrderBy), 1, orderBy);
    }

    public void Sort(string sort)
    {
      this.mapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.Sort), 1, sort);
    }

    public void Subselect(string subselect)
    {
      this.mapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.Subselect), 1, subselect);
    }

    public void KeyNullable()
    {
      this.mapping.Key.Set<bool>((Expression<Func<KeyMapping, bool>>) (x => x.NotNull), 1, (!this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    public void Table(string tableName)
    {
      this.mapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.TableName), 1, tableName);
    }

    public void Name(string name)
    {
      this.mapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.Name), 1, name);
    }

    public void Schema(string schema)
    {
      this.mapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.Schema), 1, schema);
    }

    public void LazyLoad()
    {
      this.mapping.Set<Lazy>((Expression<Func<CollectionMapping, Lazy>>) (x => x.Lazy), 1, (Lazy) (this.nextBool ? 1 : 0));
      this.nextBool = true;
    }

    public override void ExtraLazyLoad()
    {
      this.mapping.Set<Lazy>((Expression<Func<CollectionMapping, Lazy>>) (x => x.Lazy), 1, (Lazy) (this.nextBool ? 2 : 1));
      this.nextBool = true;
    }

    public void BatchSize(int batchSize)
    {
      this.mapping.Set<int>((Expression<Func<CollectionMapping, int>>) (x => x.BatchSize), 1, batchSize);
    }

    public void ReadOnly()
    {
      this.mapping.Set<bool>((Expression<Func<CollectionMapping, bool>>) (x => x.Mutable), 1, (!this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
    }

    void ICollectionInstance.AsArray() => this.mapping.Collection = Collection.Array;

    void ICollectionInstance.AsBag() => this.mapping.Collection = Collection.Bag;

    void ICollectionInstance.AsList()
    {
      this.mapping.Collection = Collection.List;
      if (this.mapping.Index != null)
        return;
      IndexMapping indexMapping = new IndexMapping();
      ColumnMapping mapping = new ColumnMapping();
      mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, "Index");
      indexMapping.AddColumn(0, mapping);
      this.mapping.Set<IIndexMapping>((Expression<Func<CollectionMapping, IIndexMapping>>) (x => x.Index), 0, (IIndexMapping) indexMapping);
    }

    void ICollectionInstance.AsMap() => this.mapping.Collection = Collection.Map;

    void ICollectionInstance.AsSet() => this.mapping.Collection = Collection.Set;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ICollectionInstance Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return (ICollectionInstance) this;
      }
    }

    public ICacheInstance Cache
    {
      get
      {
        if (this.mapping.Cache == null)
          this.mapping.Set<CacheMapping>((Expression<Func<CollectionMapping, CacheMapping>>) (x => x.Cache), 1, new CacheMapping());
        return (ICacheInstance) new CacheInstance(this.mapping.Cache);
      }
    }

    public void SetOrderBy(string orderBy)
    {
      this.mapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.OrderBy), 1, orderBy);
    }

    public IAccessInstance Access
    {
      get
      {
        return (IAccessInstance) new AccessInstance((Action<string>) (value => this.mapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.Access), 1, value)));
      }
    }

    public IKeyInstance Key => (IKeyInstance) new KeyInstance(this.mapping.Key);

    public IElementInstance Element
    {
      get
      {
        if (this.mapping.Element == null)
          this.mapping.Set<ElementMapping>((Expression<Func<CollectionMapping, ElementMapping>>) (x => x.Element), 1, new ElementMapping());
        return (IElementInstance) new ElementInstance(this.mapping.Element);
      }
    }

    public void ApplyFilter(string name, string condition)
    {
      FilterMapping mapping = new FilterMapping();
      mapping.Set<string>((Expression<Func<FilterMapping, string>>) (x => x.Name), 1, name);
      mapping.Set<string>((Expression<Func<FilterMapping, string>>) (x => x.Condition), 1, condition);
      this.mapping.AddFilter(mapping);
    }

    public void ApplyFilter(string name) => this.ApplyFilter(name, (string) null);

    public void ApplyFilter<TFilter>(string condition) where TFilter : FilterDefinition, new()
    {
      this.ApplyFilter(new TFilter().Name, condition);
    }

    public void ApplyFilter<TFilter>() where TFilter : FilterDefinition, new()
    {
      this.ApplyFilter<TFilter>((string) null);
    }
  }
}

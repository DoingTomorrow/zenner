// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.ToManyBase`2
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public abstract class ToManyBase<T, TChild> : ICollectionMappingProvider where T : ToManyBase<T, TChild>, ICollectionMappingProvider
  {
    private readonly AccessStrategyBuilder<T> access;
    private readonly FetchTypeExpression<T> fetch;
    private readonly OptimisticLockBuilder<T> optimisticLock;
    private readonly CollectionCascadeExpression<T> cascade;
    protected ElementPart elementPart;
    protected ICompositeElementMappingProvider componentMapping;
    protected bool nextBool = true;
    protected readonly AttributeStore collectionAttributes = new AttributeStore();
    protected readonly KeyMapping keyMapping = new KeyMapping();
    protected readonly AttributeStore relationshipAttributes = new AttributeStore();
    private readonly IList<IFilterMappingProvider> filters = (IList<IFilterMappingProvider>) new List<IFilterMappingProvider>();
    private Func<AttributeStore, CollectionMapping> collectionBuilder;
    private IndexMapping indexMapping;
    protected Member member;
    private readonly Type entity;

    protected ToManyBase(Type entity, Member member, Type type)
    {
      this.entity = entity;
      this.member = member;
      this.AsBag();
      this.access = new AccessStrategyBuilder<T>((T) this, (Action<string>) (value => this.collectionAttributes.Set(nameof (Access), 2, (object) value)));
      this.fetch = new FetchTypeExpression<T>((T) this, (Action<string>) (value => this.collectionAttributes.Set(nameof (Fetch), 2, (object) value)));
      this.optimisticLock = new OptimisticLockBuilder<T>((T) this, (Action<string>) (value => this.collectionAttributes.Set("OptimisticLock", 2, (object) value)));
      this.cascade = new CollectionCascadeExpression<T>((T) this, (Action<string>) (value => this.collectionAttributes.Set(nameof (Cascade), 2, (object) value)));
      this.SetDefaultCollectionType();
      this.SetCustomCollectionType(type);
      this.SetDefaultAccess();
      this.Cache = new CachePart(entity);
      this.collectionAttributes.Set("Name", 0, (object) member.Name);
      this.relationshipAttributes.Set("Class", 0, (object) new TypeReference(typeof (TChild)));
    }

    public Type EntityType => this.entity;

    public T PropertyRef(string propertyRef)
    {
      this.keyMapping.Set<string>((Expression<Func<KeyMapping, string>>) (x => x.PropertyRef), 2, propertyRef);
      return (T) this;
    }

    public CachePart Cache { get; private set; }

    public T LazyLoad()
    {
      this.collectionAttributes.Set("Lazy", 2, (object) (Lazy) (this.nextBool ? 1 : 0));
      this.nextBool = true;
      return (T) this;
    }

    public T ExtraLazyLoad()
    {
      this.collectionAttributes.Set("Lazy", 2, (object) (Lazy) (this.nextBool ? 2 : 1));
      this.nextBool = true;
      return (T) this;
    }

    public T Inverse()
    {
      this.collectionAttributes.Set(nameof (Inverse), 2, (object) this.nextBool);
      this.nextBool = true;
      return (T) this;
    }

    public CollectionCascadeExpression<T> Cascade => this.cascade;

    public T AsSet()
    {
      this.collectionBuilder = (Func<AttributeStore, CollectionMapping>) (attrs => CollectionMapping.Set(attrs));
      return (T) this;
    }

    public T AsSet(SortType sort)
    {
      this.collectionBuilder = (Func<AttributeStore, CollectionMapping>) (attrs =>
      {
        CollectionMapping collectionMapping = CollectionMapping.Set(attrs);
        collectionMapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.Sort), 2, sort.ToLowerInvariantString());
        return collectionMapping;
      });
      return (T) this;
    }

    public T AsSet<TComparer>() where TComparer : IComparer<TChild>
    {
      this.collectionBuilder = (Func<AttributeStore, CollectionMapping>) (attrs =>
      {
        CollectionMapping collectionMapping = CollectionMapping.Set(attrs);
        collectionMapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.Sort), 2, typeof (TComparer).AssemblyQualifiedName);
        return collectionMapping;
      });
      return (T) this;
    }

    public T AsBag()
    {
      this.collectionBuilder = (Func<AttributeStore, CollectionMapping>) (attrs => CollectionMapping.Bag(attrs));
      return (T) this;
    }

    public T AsList()
    {
      this.collectionBuilder = (Func<AttributeStore, CollectionMapping>) (attrs => CollectionMapping.List(attrs));
      this.CreateIndexMapping((Action<IndexPart>) null);
      if (this.indexMapping.Columns.IsEmpty<ColumnMapping>())
      {
        ColumnMapping mapping = new ColumnMapping();
        mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, "Index");
        this.indexMapping.AddColumn(0, mapping);
      }
      return (T) this;
    }

    public T AsList(Action<ListIndexPart> customIndexMapping)
    {
      this.collectionBuilder = (Func<AttributeStore, CollectionMapping>) (attrs => CollectionMapping.List(attrs));
      this.CreateListIndexMapping(customIndexMapping);
      if (this.indexMapping.Columns.IsEmpty<ColumnMapping>())
      {
        ColumnMapping mapping = new ColumnMapping();
        mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, "Index");
        this.indexMapping.AddColumn(0, mapping);
      }
      return (T) this;
    }

    public T AsMap<TIndex>(Expression<Func<TChild, TIndex>> indexSelector)
    {
      return this.AsMap<TIndex>(indexSelector, (Action<IndexPart>) null);
    }

    public T AsMap<TIndex>(Expression<Func<TChild, TIndex>> indexSelector, SortType sort)
    {
      return this.AsMap<TIndex>(indexSelector, (Action<IndexPart>) null, sort);
    }

    public T AsMap(string indexColumnName)
    {
      this.collectionBuilder = (Func<AttributeStore, CollectionMapping>) (attrs => CollectionMapping.Map(attrs));
      this.AsIndexedCollection<int>(indexColumnName, (Action<IndexPart>) null);
      return (T) this;
    }

    public T AsMap(string indexColumnName, SortType sort)
    {
      this.collectionBuilder = (Func<AttributeStore, CollectionMapping>) (attrs =>
      {
        CollectionMapping collectionMapping = CollectionMapping.Map(attrs);
        collectionMapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.Sort), 2, sort.ToLowerInvariantString());
        return collectionMapping;
      });
      this.AsIndexedCollection<int>(indexColumnName, (Action<IndexPart>) null);
      return (T) this;
    }

    public T AsMap<TIndex>(string indexColumnName)
    {
      this.collectionBuilder = (Func<AttributeStore, CollectionMapping>) (attrs => CollectionMapping.Map(attrs));
      this.AsIndexedCollection<TIndex>(indexColumnName, (Action<IndexPart>) null);
      return (T) this;
    }

    public T AsMap<TIndex>(string indexColumnName, SortType sort)
    {
      this.collectionBuilder = (Func<AttributeStore, CollectionMapping>) (attrs =>
      {
        CollectionMapping collectionMapping = CollectionMapping.Map(attrs);
        collectionMapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.Sort), 2, sort.ToLowerInvariantString());
        return collectionMapping;
      });
      this.AsIndexedCollection<TIndex>(indexColumnName, (Action<IndexPart>) null);
      return (T) this;
    }

    public T AsMap<TIndex, TComparer>(string indexColumnName) where TComparer : IComparer<TChild>
    {
      this.collectionBuilder = (Func<AttributeStore, CollectionMapping>) (attrs =>
      {
        CollectionMapping collectionMapping = CollectionMapping.Map(attrs);
        collectionMapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.Sort), 2, typeof (TComparer).AssemblyQualifiedName);
        return collectionMapping;
      });
      this.AsIndexedCollection<TIndex>(indexColumnName, (Action<IndexPart>) null);
      return (T) this;
    }

    public T AsMap<TIndex>(
      Expression<Func<TChild, TIndex>> indexSelector,
      Action<IndexPart> customIndexMapping)
    {
      this.collectionBuilder = (Func<AttributeStore, CollectionMapping>) (attrs => CollectionMapping.Map(attrs));
      return this.AsIndexedCollection<TIndex>(indexSelector, customIndexMapping);
    }

    public T AsMap<TIndex>(
      Expression<Func<TChild, TIndex>> indexSelector,
      Action<IndexPart> customIndexMapping,
      SortType sort)
    {
      this.collectionBuilder = (Func<AttributeStore, CollectionMapping>) (attrs =>
      {
        CollectionMapping collectionMapping = CollectionMapping.Map(attrs);
        collectionMapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.Sort), 2, sort.ToLowerInvariantString());
        return collectionMapping;
      });
      return this.AsIndexedCollection<TIndex>(indexSelector, customIndexMapping);
    }

    public T AsMap<TIndex>(
      Action<IndexPart> customIndexMapping,
      Action<ElementPart> customElementMapping)
    {
      this.collectionBuilder = (Func<AttributeStore, CollectionMapping>) (attrs => CollectionMapping.Map(attrs));
      this.AsIndexedCollection<TIndex>(string.Empty, customIndexMapping);
      this.Element(string.Empty);
      customElementMapping(this.elementPart);
      return (T) this;
    }

    public T AsArray<TIndex>(Expression<Func<TChild, TIndex>> indexSelector)
    {
      return this.AsArray<TIndex>(indexSelector, (Action<IndexPart>) null);
    }

    public T AsArray<TIndex>(
      Expression<Func<TChild, TIndex>> indexSelector,
      Action<IndexPart> customIndexMapping)
    {
      this.collectionBuilder = (Func<AttributeStore, CollectionMapping>) (attrs => CollectionMapping.Array(attrs));
      return this.AsIndexedCollection<TIndex>(indexSelector, customIndexMapping);
    }

    public T AsIndexedCollection<TIndex>(
      Expression<Func<TChild, TIndex>> indexSelector,
      Action<IndexPart> customIndexMapping)
    {
      return this.AsIndexedCollection<TIndex>(indexSelector.ToMember<TChild, TIndex>().Name, customIndexMapping);
    }

    public T AsIndexedCollection<TIndex>(string indexColumn, Action<IndexPart> customIndexMapping)
    {
      this.CreateIndexMapping(customIndexMapping);
      this.indexMapping.Set<TypeReference>((Expression<Func<IndexMapping, TypeReference>>) (x => x.Type), 0, new TypeReference(typeof (TIndex)));
      if (this.indexMapping.Columns.IsEmpty<ColumnMapping>())
      {
        ColumnMapping mapping = new ColumnMapping();
        mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, indexColumn);
        this.indexMapping.AddColumn(0, mapping);
      }
      return (T) this;
    }

    private void CreateIndexMapping(Action<IndexPart> customIndex)
    {
      IndexPart indexPart = new IndexPart(typeof (T));
      if (customIndex != null)
        customIndex(indexPart);
      this.indexMapping = indexPart.GetIndexMapping();
    }

    private void CreateListIndexMapping(Action<ListIndexPart> customIndex)
    {
      this.indexMapping = new IndexMapping();
      ListIndexPart listIndexPart = new ListIndexPart(this.indexMapping);
      if (customIndex == null)
        return;
      customIndex(listIndexPart);
    }

    public T Element(string columnName)
    {
      this.elementPart = new ElementPart(typeof (T));
      this.elementPart.Type<TChild>();
      if (!string.IsNullOrEmpty(columnName))
        this.elementPart.Column(columnName);
      return (T) this;
    }

    public T Element(string columnName, Action<ElementPart> customElementMapping)
    {
      this.Element(columnName);
      if (customElementMapping != null)
        customElementMapping(this.elementPart);
      return (T) this;
    }

    public T Component(Action<CompositeElementPart<TChild>> action)
    {
      CompositeElementPart<TChild> compositeElementPart = new CompositeElementPart<TChild>(typeof (T));
      action(compositeElementPart);
      this.componentMapping = (ICompositeElementMappingProvider) compositeElementPart;
      return (T) this;
    }

    public T Table(string name)
    {
      this.collectionAttributes.Set("TableName", 2, (object) name);
      return (T) this;
    }

    public T ForeignKeyCascadeOnDelete()
    {
      this.keyMapping.Set<string>((Expression<Func<KeyMapping, string>>) (x => x.OnDelete), 0, "cascade");
      return (T) this;
    }

    public FetchTypeExpression<T> Fetch => this.fetch;

    public AccessStrategyBuilder<T> Access => this.access;

    public T OptimisticLock()
    {
      this.collectionAttributes.Set(nameof (OptimisticLock), 2, (object) this.nextBool);
      this.nextBool = true;
      return (T) this;
    }

    public T Persister<TPersister>()
    {
      this.collectionAttributes.Set(nameof (Persister), 2, (object) new TypeReference(typeof (TPersister)));
      return (T) this;
    }

    public T Check(string constraintName)
    {
      this.collectionAttributes.Set(nameof (Check), 2, (object) constraintName);
      return (T) this;
    }

    public T Generic()
    {
      this.collectionAttributes.Set(nameof (Generic), 2, (object) this.nextBool);
      this.nextBool = true;
      return (T) this;
    }

    public T Where(string where)
    {
      this.collectionAttributes.Set(nameof (Where), 2, (object) where);
      return (T) this;
    }

    public T BatchSize(int size)
    {
      this.collectionAttributes.Set(nameof (BatchSize), 2, (object) size);
      return (T) this;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public T Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return (T) this;
      }
    }

    public T CollectionType<TCollection>() => this.CollectionType(typeof (TCollection));

    public T CollectionType(Type type) => this.CollectionType(new TypeReference(type));

    public T CollectionType(string type) => this.CollectionType(new TypeReference(type));

    public T CollectionType(TypeReference type)
    {
      this.collectionAttributes.Set(nameof (CollectionType), 2, (object) type);
      return (T) this;
    }

    public T Schema(string schema)
    {
      this.collectionAttributes.Set(nameof (Schema), 2, (object) schema);
      return (T) this;
    }

    public T EntityName(string entityName)
    {
      this.relationshipAttributes.Set(nameof (EntityName), 2, (object) entityName);
      return (T) this;
    }

    public T ApplyFilter(string name, string condition)
    {
      this.filters.Add((IFilterMappingProvider) new FilterPart(name, condition));
      return (T) this;
    }

    public T ApplyFilter(string name) => this.ApplyFilter(name, (string) null);

    public T ApplyFilter<TFilter>(string condition) where TFilter : FilterDefinition, new()
    {
      return this.ApplyFilter(new TFilter().Name, condition);
    }

    public T ApplyFilter<TFilter>() where TFilter : FilterDefinition, new()
    {
      return this.ApplyFilter<TFilter>((string) null);
    }

    protected IList<IFilterMappingProvider> Filters => this.filters;

    private void SetDefaultCollectionType()
    {
      switch (CollectionTypeResolver.Resolve(this.member))
      {
        case Collection.Bag:
          this.AsBag();
          break;
        case Collection.Set:
          this.AsSet();
          break;
      }
    }

    private void SetDefaultAccess()
    {
      FluentNHibernate.Mapping.Access access = MemberAccessResolver.Resolve(this.member);
      if (access == FluentNHibernate.Mapping.Access.Property || access == FluentNHibernate.Mapping.Access.Unset)
        return;
      this.collectionAttributes.Set("Access", 0, (object) access.ToString());
    }

    private void SetCustomCollectionType(Type type)
    {
      if (type.Namespace.StartsWith("Iesi") || type.Namespace.StartsWith("System") || type.IsArray)
        return;
      this.collectionAttributes.Set("CollectionType", 0, (object) new TypeReference(type));
    }

    CollectionMapping ICollectionMappingProvider.GetCollectionMapping()
    {
      return this.GetCollectionMapping();
    }

    protected virtual CollectionMapping GetCollectionMapping()
    {
      CollectionMapping collectionMapping = this.collectionBuilder(this.collectionAttributes.Clone());
      collectionMapping.ContainingEntityType = this.entity;
      collectionMapping.Member = this.member;
      collectionMapping.Set<string>((Expression<Func<CollectionMapping, string>>) (x => x.Name), 0, this.GetDefaultName());
      collectionMapping.Set<Type>((Expression<Func<CollectionMapping, Type>>) (x => x.ChildType), 0, typeof (TChild));
      collectionMapping.Set<KeyMapping>((Expression<Func<CollectionMapping, KeyMapping>>) (x => x.Key), 0, this.keyMapping);
      collectionMapping.Set<ICollectionRelationshipMapping>((Expression<Func<CollectionMapping, ICollectionRelationshipMapping>>) (x => x.Relationship), 0, this.GetRelationship());
      collectionMapping.Key.ContainingEntityType = this.entity;
      if (this.Cache.IsDirty)
        collectionMapping.Set<CacheMapping>((Expression<Func<CollectionMapping, CacheMapping>>) (x => x.Cache), 0, ((ICacheMappingProvider) this.Cache).GetCacheMapping());
      if (this.componentMapping != null)
      {
        collectionMapping.Set<CompositeElementMapping>((Expression<Func<CollectionMapping, CompositeElementMapping>>) (x => x.CompositeElement), 0, this.componentMapping.GetCompositeElementMapping());
        collectionMapping.Set<ICollectionRelationshipMapping>((Expression<Func<CollectionMapping, ICollectionRelationshipMapping>>) (x => x.Relationship), 0, (ICollectionRelationshipMapping) null);
      }
      if (collectionMapping.Collection == Collection.Array || collectionMapping.Collection == Collection.List || collectionMapping.Collection == Collection.Map)
        collectionMapping.Set<IIndexMapping>((Expression<Func<CollectionMapping, IIndexMapping>>) (x => x.Index), 0, (IIndexMapping) this.indexMapping);
      if (this.elementPart != null)
      {
        collectionMapping.Set<ElementMapping>((Expression<Func<CollectionMapping, ElementMapping>>) (x => x.Element), 0, ((IElementMappingProvider) this.elementPart).GetElementMapping());
        collectionMapping.Set<ICollectionRelationshipMapping>((Expression<Func<CollectionMapping, ICollectionRelationshipMapping>>) (x => x.Relationship), 0, (ICollectionRelationshipMapping) null);
      }
      foreach (IFilterMappingProvider filter in (IEnumerable<IFilterMappingProvider>) this.Filters)
        collectionMapping.AddFilter(filter.GetFilterMapping());
      return collectionMapping;
    }

    private string GetDefaultName()
    {
      if (this.member.IsMethod)
      {
        Member backingField;
        if (this.member.TryGetBackingField(out backingField))
          return backingField.Name;
        if (this.member.Name.StartsWith("Get"))
        {
          string defaultName = this.member.Name.Substring(3);
          if (char.IsUpper(defaultName[0]))
            defaultName = char.ToLower(defaultName[0]).ToString() + defaultName.Substring(1);
          return defaultName;
        }
      }
      return this.member.Name;
    }

    protected abstract ICollectionRelationshipMapping GetRelationship();
  }
}

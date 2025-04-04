// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.Collections.CollectionMapping
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
  public class CollectionMapping : MappingBase, IRelationship
  {
    private readonly AttributeStore attributes;
    private readonly IList<FilterMapping> filters = (IList<FilterMapping>) new System.Collections.Generic.List<FilterMapping>();

    public Type ContainingEntityType { get; set; }

    public Member Member { get; set; }

    private CollectionMapping(AttributeStore attributes)
    {
      this.Collection = Collection.Bag;
      this.attributes = attributes;
    }

    public IEnumerable<FilterMapping> Filters => (IEnumerable<FilterMapping>) this.filters;

    public void AddFilter(FilterMapping mapping) => this.filters.Add(mapping);

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessCollection(this);
      if (this.Key != null)
        visitor.Visit(this.Key);
      if (this.Index != null && (this.Collection == Collection.Array || this.Collection == Collection.List || this.Collection == Collection.Map))
        visitor.Visit(this.Index);
      if (this.Element != null)
        visitor.Visit(this.Element);
      if (this.CompositeElement != null)
        visitor.Visit(this.CompositeElement);
      if (this.Relationship != null)
        visitor.Visit(this.Relationship);
      foreach (FilterMapping filter in this.Filters)
        visitor.Visit(filter);
      if (this.Cache == null)
        return;
      visitor.Visit(this.Cache);
    }

    public Type ChildType => this.attributes.GetOrDefault<Type>(nameof (ChildType));

    public IRelationship OtherSide { get; set; }

    public KeyMapping Key => this.attributes.GetOrDefault<KeyMapping>(nameof (Key));

    public ElementMapping Element => this.attributes.GetOrDefault<ElementMapping>(nameof (Element));

    public CompositeElementMapping CompositeElement
    {
      get => this.attributes.GetOrDefault<CompositeElementMapping>(nameof (CompositeElement));
    }

    public CacheMapping Cache => this.attributes.GetOrDefault<CacheMapping>(nameof (Cache));

    public ICollectionRelationshipMapping Relationship
    {
      get => this.attributes.GetOrDefault<ICollectionRelationshipMapping>(nameof (Relationship));
    }

    public bool Generic => this.attributes.GetOrDefault<bool>(nameof (Generic));

    public Lazy Lazy => this.attributes.GetOrDefault<Lazy>(nameof (Lazy));

    public bool Inverse => this.attributes.GetOrDefault<bool>(nameof (Inverse));

    public string Name => this.attributes.GetOrDefault<string>(nameof (Name));

    public string Access => this.attributes.GetOrDefault<string>(nameof (Access));

    public string TableName => this.attributes.GetOrDefault<string>(nameof (TableName));

    public string Schema => this.attributes.GetOrDefault<string>(nameof (Schema));

    public string Fetch => this.attributes.GetOrDefault<string>(nameof (Fetch));

    public string Cascade => this.attributes.GetOrDefault<string>(nameof (Cascade));

    public string Where => this.attributes.GetOrDefault<string>(nameof (Where));

    public bool Mutable => this.attributes.GetOrDefault<bool>(nameof (Mutable));

    public string Subselect => this.attributes.GetOrDefault<string>(nameof (Subselect));

    public TypeReference Persister
    {
      get => this.attributes.GetOrDefault<TypeReference>(nameof (Persister));
    }

    public int BatchSize => this.attributes.GetOrDefault<int>(nameof (BatchSize));

    public string Check => this.attributes.GetOrDefault<string>(nameof (Check));

    public TypeReference CollectionType
    {
      get => this.attributes.GetOrDefault<TypeReference>(nameof (CollectionType));
    }

    public bool OptimisticLock => this.attributes.GetOrDefault<bool>(nameof (OptimisticLock));

    public string OrderBy => this.attributes.GetOrDefault<string>(nameof (OrderBy));

    public Collection Collection { get; set; }

    public string Sort => this.attributes.GetOrDefault<string>(nameof (Sort));

    public IIndexMapping Index => this.attributes.GetOrDefault<IIndexMapping>(nameof (Index));

    public bool Equals(CollectionMapping other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return object.Equals((object) other.attributes, (object) this.attributes) && other.filters.ContentEquals<FilterMapping>((IEnumerable<FilterMapping>) this.filters) && object.Equals((object) other.ContainingEntityType, (object) this.ContainingEntityType) && object.Equals((object) other.Member, (object) this.Member);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (CollectionMapping) && this.Equals((CollectionMapping) obj);
    }

    public override int GetHashCode()
    {
      return (((this.attributes != null ? this.attributes.GetHashCode() : 0) * 397 ^ (this.filters != null ? this.filters.GetHashCode() : 0)) * 397 ^ (this.ContainingEntityType != null ? this.ContainingEntityType.GetHashCode() : 0)) * 397 ^ (this.Member != (Member) null ? this.Member.GetHashCode() : 0);
    }

    public void Set<T>(Expression<Func<CollectionMapping, T>> expression, int layer, T value)
    {
      this.Set(expression.ToMember<CollectionMapping, T>().Name, layer, (object) value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);

    public static CollectionMapping Array() => CollectionMapping.Array(new AttributeStore());

    public static CollectionMapping Array(AttributeStore underlyingStore)
    {
      return CollectionMapping.For(Collection.Array, underlyingStore);
    }

    public static CollectionMapping Bag() => CollectionMapping.Bag(new AttributeStore());

    public static CollectionMapping Bag(AttributeStore underlyingStore)
    {
      return CollectionMapping.For(Collection.Bag, underlyingStore);
    }

    public static CollectionMapping List() => CollectionMapping.List(new AttributeStore());

    public static CollectionMapping List(AttributeStore underlyingStore)
    {
      return CollectionMapping.For(Collection.List, underlyingStore);
    }

    public static CollectionMapping Map() => CollectionMapping.Map(new AttributeStore());

    public static CollectionMapping Map(AttributeStore underlyingStore)
    {
      return CollectionMapping.For(Collection.Map, underlyingStore);
    }

    public static CollectionMapping Set() => CollectionMapping.Set(new AttributeStore());

    public static CollectionMapping Set(AttributeStore underlyingStore)
    {
      return CollectionMapping.For(Collection.Set, underlyingStore);
    }

    public static CollectionMapping For(Collection collectionType)
    {
      return CollectionMapping.For(collectionType, new AttributeStore());
    }

    public static CollectionMapping For(Collection collectionType, AttributeStore underlyingStore)
    {
      return new CollectionMapping(underlyingStore)
      {
        Collection = collectionType
      };
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.ManyToManyPart`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class ManyToManyPart<TChild> : ToManyBase<ManyToManyPart<TChild>, TChild>
  {
    private readonly IList<IFilterMappingProvider> childFilters = (IList<IFilterMappingProvider>) new List<IFilterMappingProvider>();
    private readonly FetchTypeExpression<ManyToManyPart<TChild>> fetch;
    private readonly NotFoundExpression<ManyToManyPart<TChild>> notFound;
    private IndexManyToManyPart manyToManyIndex;
    private IndexPart index;
    private readonly ColumnMappingCollection<ManyToManyPart<TChild>> childKeyColumns;
    private readonly ColumnMappingCollection<ManyToManyPart<TChild>> parentKeyColumns;
    private readonly Type childType;
    private Type valueType;
    private bool isTernary;

    public ManyToManyPart(Type entity, Member property)
      : this(entity, property, property.PropertyType)
    {
    }

    protected ManyToManyPart(Type entity, Member member, Type collectionType)
      : base(entity, member, collectionType)
    {
      this.childType = collectionType;
      this.fetch = new FetchTypeExpression<ManyToManyPart<TChild>>(this, (Action<string>) (value => this.collectionAttributes.Set("Fetch", 2, (object) value)));
      this.notFound = new NotFoundExpression<ManyToManyPart<TChild>>(this, (Action<string>) (value => this.relationshipAttributes.Set(nameof (NotFound), 2, (object) value)));
      this.childKeyColumns = new ColumnMappingCollection<ManyToManyPart<TChild>>(this);
      this.parentKeyColumns = new ColumnMappingCollection<ManyToManyPart<TChild>>(this);
    }

    public ManyToManyPart<TChild> ChildKeyColumn(string childKeyColumn)
    {
      this.childKeyColumns.Clear();
      this.childKeyColumns.Add(childKeyColumn);
      return this;
    }

    public ManyToManyPart<TChild> ParentKeyColumn(string parentKeyColumn)
    {
      this.parentKeyColumns.Clear();
      this.parentKeyColumns.Add(parentKeyColumn);
      return this;
    }

    public ColumnMappingCollection<ManyToManyPart<TChild>> ChildKeyColumns => this.childKeyColumns;

    public ColumnMappingCollection<ManyToManyPart<TChild>> ParentKeyColumns
    {
      get => this.parentKeyColumns;
    }

    public ManyToManyPart<TChild> ForeignKeyConstraintNames(
      string parentForeignKeyName,
      string childForeignKeyName)
    {
      this.keyMapping.Set<string>((Expression<Func<KeyMapping, string>>) (x => x.ForeignKey), 2, parentForeignKeyName);
      this.relationshipAttributes.Set("ForeignKey", 2, (object) childForeignKeyName);
      return this;
    }

    public ManyToManyPart<TChild> ChildPropertyRef(string childPropertyRef)
    {
      this.relationshipAttributes.Set(nameof (ChildPropertyRef), 2, (object) childPropertyRef);
      return this;
    }

    public FetchTypeExpression<ManyToManyPart<TChild>> FetchType => this.fetch;

    private void EnsureDictionary()
    {
      if (!typeof (IDictionary).IsAssignableFrom(this.childType))
        throw new ArgumentException(this.member.Name + " must be of type IDictionary to be used in a non-generic ternary association. Type was: " + (object) this.childType);
    }

    private void EnsureGenericDictionary()
    {
      if (!this.childType.IsGenericType || this.childType.GetGenericTypeDefinition() != typeof (IDictionary<,>))
        throw new ArgumentException(this.member.Name + " must be of type IDictionary<> to be used in a ternary assocation. Type was: " + (object) this.childType);
    }

    public ManyToManyPart<TChild> AsTernaryAssociation()
    {
      this.EnsureGenericDictionary();
      return this.AsTernaryAssociation(typeof (TChild).GetGenericArguments()[0].Name + "_id", typeof (TChild).GetGenericArguments()[1].Name + "_id");
    }

    public ManyToManyPart<TChild> AsTernaryAssociation(string indexColumn, string valueColumn)
    {
      return this.AsTernaryAssociation(indexColumn, valueColumn, (Action<IndexManyToManyPart>) (x => { }));
    }

    public ManyToManyPart<TChild> AsTernaryAssociation(
      string indexColumn,
      string valueColumn,
      Action<IndexManyToManyPart> indexAction)
    {
      this.EnsureGenericDictionary();
      Type genericArgument1 = typeof (TChild).GetGenericArguments()[0];
      Type genericArgument2 = typeof (TChild).GetGenericArguments()[1];
      this.manyToManyIndex = new IndexManyToManyPart(typeof (ManyToManyPart<TChild>));
      this.manyToManyIndex.Column(indexColumn);
      this.manyToManyIndex.Type(genericArgument1);
      if (indexAction != null)
        indexAction(this.manyToManyIndex);
      this.ChildKeyColumn(valueColumn);
      this.valueType = genericArgument2;
      this.isTernary = true;
      return this;
    }

    public ManyToManyPart<TChild> AsTernaryAssociation(Type indexType, Type typeOfValue)
    {
      return this.AsTernaryAssociation(indexType, indexType.Name + "_id", typeOfValue, typeOfValue.Name + "_id");
    }

    public ManyToManyPart<TChild> AsTernaryAssociation(
      Type indexType,
      string indexColumn,
      Type typeOfValue,
      string valueColumn)
    {
      return this.AsTernaryAssociation(indexType, indexColumn, typeOfValue, valueColumn, (Action<IndexManyToManyPart>) (x => { }));
    }

    public ManyToManyPart<TChild> AsTernaryAssociation(
      Type indexType,
      string indexColumn,
      Type typeOfValue,
      string valueColumn,
      Action<IndexManyToManyPart> indexAction)
    {
      this.EnsureDictionary();
      this.manyToManyIndex = new IndexManyToManyPart(typeof (ManyToManyPart<TChild>));
      this.manyToManyIndex.Column(indexColumn);
      this.manyToManyIndex.Type(indexType);
      if (indexAction != null)
        indexAction(this.manyToManyIndex);
      this.ChildKeyColumn(valueColumn);
      this.valueType = typeOfValue;
      this.isTernary = true;
      return this;
    }

    public ManyToManyPart<TChild> AsSimpleAssociation()
    {
      this.EnsureGenericDictionary();
      return this.AsSimpleAssociation(typeof (TChild).GetGenericArguments()[0].Name + "_id", typeof (TChild).GetGenericArguments()[1].Name + "_id");
    }

    public ManyToManyPart<TChild> AsSimpleAssociation(string indexColumn, string valueColumn)
    {
      this.EnsureGenericDictionary();
      Type genericArgument1 = typeof (TChild).GetGenericArguments()[0];
      Type genericArgument2 = typeof (TChild).GetGenericArguments()[1];
      this.index = new IndexPart(genericArgument1);
      this.index.Column(indexColumn);
      this.index.Type(genericArgument1);
      this.ChildKeyColumn(valueColumn);
      this.valueType = genericArgument2;
      this.isTernary = true;
      return this;
    }

    public ManyToManyPart<TChild> AsEntityMap() => this.AsMap((string) null).AsTernaryAssociation();

    public ManyToManyPart<TChild> AsEntityMap(string indexColumn, string valueColumn)
    {
      return this.AsMap((string) null).AsTernaryAssociation(indexColumn, valueColumn);
    }

    public Type ChildType => typeof (TChild);

    public NotFoundExpression<ManyToManyPart<TChild>> NotFound => this.notFound;

    protected override ICollectionRelationshipMapping GetRelationship()
    {
      ManyToManyMapping relationship = new ManyToManyMapping(this.relationshipAttributes.Clone())
      {
        ContainingEntityType = this.EntityType
      };
      if (this.isTernary && this.valueType != null)
        relationship.Set<TypeReference>((Expression<Func<ManyToManyMapping, TypeReference>>) (x => x.Class), 0, new TypeReference(this.valueType));
      foreach (IFilterMappingProvider childFilter in (IEnumerable<IFilterMappingProvider>) this.childFilters)
        relationship.ChildFilters.Add(childFilter.GetFilterMapping());
      return (ICollectionRelationshipMapping) relationship;
    }

    public ManyToManyPart<TChild> OrderBy(string orderBy)
    {
      this.collectionAttributes.Set(nameof (OrderBy), 2, (object) orderBy);
      return this;
    }

    public ManyToManyPart<TChild> ChildOrderBy(string orderBy)
    {
      this.relationshipAttributes.Set("OrderBy", 2, (object) orderBy);
      return this;
    }

    public ManyToManyPart<TChild> ReadOnly()
    {
      this.collectionAttributes.Set("Mutable", 2, (object) !this.nextBool);
      this.nextBool = true;
      return this;
    }

    public ManyToManyPart<TChild> Subselect(string subselect)
    {
      this.collectionAttributes.Set(nameof (Subselect), 2, (object) subselect);
      return this;
    }

    public ManyToManyPart<TChild> ApplyChildFilter(string name, string condition)
    {
      this.childFilters.Add((IFilterMappingProvider) new FilterPart(name, condition));
      return this;
    }

    public ManyToManyPart<TChild> ApplyChildFilter(string name)
    {
      return this.ApplyChildFilter(name, (string) null);
    }

    public ManyToManyPart<TChild> ApplyChildFilter<TFilter>(string condition) where TFilter : FilterDefinition, new()
    {
      this.childFilters.Add((IFilterMappingProvider) new FilterPart(new TFilter().Name, condition));
      return this;
    }

    public ManyToManyPart<TChild> ApplyChildFilter<TFilter>() where TFilter : FilterDefinition, new()
    {
      return this.ApplyChildFilter<TFilter>((string) null);
    }

    public ManyToManyPart<TChild> ChildWhere(string where)
    {
      this.relationshipAttributes.Set("Where", 2, (object) where);
      return this;
    }

    public ManyToManyPart<TChild> ChildWhere(Expression<Func<TChild, bool>> where)
    {
      return this.ChildWhere(ExpressionToSql.Convert<TChild>(where));
    }

    protected override CollectionMapping GetCollectionMapping()
    {
      CollectionMapping collectionMapping = base.GetCollectionMapping();
      if (this.parentKeyColumns.Count == 0)
        collectionMapping.Key.AddColumn(0, new ColumnMapping(this.EntityType.Name + "_id"));
      foreach (ColumnMapping parentKeyColumn in this.parentKeyColumns)
        collectionMapping.Key.AddColumn(2, parentKeyColumn);
      if (collectionMapping.Relationship != null)
      {
        if (this.childKeyColumns.Count == 0)
          ((ManyToManyMapping) collectionMapping.Relationship).AddColumn(0, new ColumnMapping(typeof (TChild).Name + "_id"));
        foreach (ColumnMapping childKeyColumn in this.childKeyColumns)
          ((ManyToManyMapping) collectionMapping.Relationship).AddColumn(2, childKeyColumn);
      }
      if (this.index != null)
        collectionMapping.Set<IIndexMapping>((Expression<Func<CollectionMapping, IIndexMapping>>) (x => x.Index), 0, (IIndexMapping) this.index.GetIndexMapping());
      if (this.manyToManyIndex != null && collectionMapping.Collection == Collection.Map)
        collectionMapping.Set<IIndexMapping>((Expression<Func<CollectionMapping, IIndexMapping>>) (x => x.Index), 0, (IIndexMapping) this.manyToManyIndex.GetIndexMapping());
      return collectionMapping;
    }
  }
}

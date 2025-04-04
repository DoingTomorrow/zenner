// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.OneToManyPart`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class OneToManyPart<TChild> : ToManyBase<OneToManyPart<TChild>, TChild>
  {
    private readonly Type entity;
    private readonly ColumnMappingCollection<OneToManyPart<TChild>> keyColumns;
    private readonly CollectionCascadeExpression<OneToManyPart<TChild>> cascade;
    private readonly NotFoundExpression<OneToManyPart<TChild>> notFound;
    private IndexManyToManyPart manyToManyIndex;
    private readonly Type childType;
    private Type valueType;
    private bool isTernary;

    public OneToManyPart(Type entity, Member property)
      : this(entity, property, property.PropertyType)
    {
    }

    protected OneToManyPart(Type entity, Member member, Type collectionType)
      : base(entity, member, collectionType)
    {
      this.entity = entity;
      this.childType = collectionType;
      this.keyColumns = new ColumnMappingCollection<OneToManyPart<TChild>>(this);
      this.cascade = new CollectionCascadeExpression<OneToManyPart<TChild>>(this, (Action<string>) (value => this.collectionAttributes.Set(nameof (Cascade), 2, (object) value)));
      this.notFound = new NotFoundExpression<OneToManyPart<TChild>>(this, (Action<string>) (value => this.relationshipAttributes.Set(nameof (NotFound), 2, (object) value)));
      this.collectionAttributes.Set("Name", 0, (object) member.Name);
    }

    public NotFoundExpression<OneToManyPart<TChild>> NotFound => this.notFound;

    public new CollectionCascadeExpression<OneToManyPart<TChild>> Cascade => this.cascade;

    public OneToManyPart<TChild> AsTernaryAssociation()
    {
      return this.AsTernaryAssociation(this.childType.GetGenericArguments()[0].Name + "_id");
    }

    public OneToManyPart<TChild> AsTernaryAssociation(string indexColumnName)
    {
      this.EnsureGenericDictionary();
      Type genericArgument1 = this.childType.GetGenericArguments()[0];
      Type genericArgument2 = this.childType.GetGenericArguments()[1];
      this.manyToManyIndex = new IndexManyToManyPart(typeof (ManyToManyPart<TChild>));
      this.manyToManyIndex.Column(indexColumnName);
      this.manyToManyIndex.Type(genericArgument1);
      this.valueType = genericArgument2;
      this.isTernary = true;
      return this;
    }

    public OneToManyPart<TChild> AsEntityMap() => this.AsMap((string) null).AsTernaryAssociation();

    public OneToManyPart<TChild> AsEntityMap(string indexColumnName)
    {
      return this.AsMap((string) null).AsTernaryAssociation(indexColumnName);
    }

    public OneToManyPart<TChild> KeyColumn(string columnName)
    {
      this.KeyColumns.Clear();
      this.KeyColumns.Add(columnName);
      return this;
    }

    public ColumnMappingCollection<OneToManyPart<TChild>> KeyColumns => this.keyColumns;

    public OneToManyPart<TChild> ForeignKeyConstraintName(string foreignKeyName)
    {
      this.keyMapping.Set<string>((Expression<Func<KeyMapping, string>>) (x => x.ForeignKey), 2, foreignKeyName);
      return this;
    }

    public OneToManyPart<TChild> OrderBy(string orderBy)
    {
      this.collectionAttributes.Set(nameof (OrderBy), 2, (object) orderBy);
      return this;
    }

    public OneToManyPart<TChild> ReadOnly()
    {
      this.collectionAttributes.Set("Mutable", 2, (object) !this.nextBool);
      this.nextBool = true;
      return this;
    }

    public OneToManyPart<TChild> Subselect(string subselect)
    {
      this.collectionAttributes.Set(nameof (Subselect), 2, (object) subselect);
      return this;
    }

    public OneToManyPart<TChild> KeyUpdate()
    {
      this.keyMapping.Set<bool>((Expression<Func<KeyMapping, bool>>) (x => x.Update), 2, (this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
      return this;
    }

    public OneToManyPart<TChild> KeyNullable()
    {
      this.keyMapping.Set<bool>((Expression<Func<KeyMapping, bool>>) (x => x.NotNull), 2, (!this.nextBool ? 1 : 0) != 0);
      this.nextBool = true;
      return this;
    }

    protected override CollectionMapping GetCollectionMapping()
    {
      CollectionMapping collectionMapping = base.GetCollectionMapping();
      if (this.keyColumns.Count<ColumnMapping>() == 0)
        collectionMapping.Key.AddColumn(0, new ColumnMapping(this.entity.Name + "_id"));
      foreach (ColumnMapping keyColumn in this.keyColumns)
        collectionMapping.Key.AddColumn(2, keyColumn);
      if (this.manyToManyIndex != null && collectionMapping.Collection == Collection.Map)
        collectionMapping.Set<IIndexMapping>((Expression<Func<CollectionMapping, IIndexMapping>>) (x => x.Index), 0, (IIndexMapping) this.manyToManyIndex.GetIndexMapping());
      return collectionMapping;
    }

    protected override ICollectionRelationshipMapping GetRelationship()
    {
      OneToManyMapping relationship = new OneToManyMapping(this.relationshipAttributes.Clone())
      {
        ContainingEntityType = this.entity
      };
      if (this.isTernary && this.valueType != null)
        relationship.Set<TypeReference>((Expression<Func<OneToManyMapping, TypeReference>>) (x => x.Class), 0, new TypeReference(this.valueType));
      return (ICollectionRelationshipMapping) relationship;
    }

    private void EnsureGenericDictionary()
    {
      if (!this.childType.IsGenericType || this.childType.GetGenericTypeDefinition() != typeof (IDictionary<,>))
        throw new ArgumentException(this.member.Name + " must be of type IDictionary<> to be used in a ternary assocation. Type was: " + (object) this.childType);
    }

    public OneToManyPart<TChild> Where(Expression<Func<TChild, bool>> where)
    {
      return this.Where(ExpressionToSql.Convert<TChild>(where));
    }
  }
}

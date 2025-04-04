// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.JoinedSubClassPart`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  [Obsolete("REMOVE ME")]
  public class JoinedSubClassPart<TSubclass> : ClasslikeMapBase<TSubclass>, ISubclassMappingProvider
  {
    private readonly MappingProviderStore providers;
    private readonly ColumnMappingCollection<JoinedSubClassPart<TSubclass>> columns;
    private readonly List<SubclassMapping> subclassMappings = new List<SubclassMapping>();
    private readonly AttributeStore attributes;
    private bool nextBool = true;

    public JoinedSubClassPart(string keyColumn)
      : this(keyColumn, new AttributeStore(), new MappingProviderStore())
    {
    }

    protected JoinedSubClassPart(
      string keyColumn,
      AttributeStore attributes,
      MappingProviderStore providers)
      : base(providers)
    {
      this.providers = providers;
      this.attributes = attributes;
      this.columns = new ColumnMappingCollection<JoinedSubClassPart<TSubclass>>(this)
      {
        keyColumn
      };
    }

    public virtual void JoinedSubClass<TNextSubclass>(
      string keyColumn,
      Action<JoinedSubClassPart<TNextSubclass>> action)
    {
      JoinedSubClassPart<TNextSubclass> joinedSubClassPart = new JoinedSubClassPart<TNextSubclass>(keyColumn);
      action(joinedSubClassPart);
      this.providers.Subclasses[typeof (TNextSubclass)] = (ISubclassMappingProvider) joinedSubClassPart;
      this.subclassMappings.Add(((ISubclassMappingProvider) joinedSubClassPart).GetSubclassMapping());
    }

    public ColumnMappingCollection<JoinedSubClassPart<TSubclass>> KeyColumns => this.columns;

    public JoinedSubClassPart<TSubclass> Table(string tableName)
    {
      this.attributes.Set("TableName", 2, (object) tableName);
      return this;
    }

    public JoinedSubClassPart<TSubclass> Schema(string schema)
    {
      this.attributes.Set(nameof (Schema), 2, (object) schema);
      return this;
    }

    public JoinedSubClassPart<TSubclass> CheckConstraint(string constraintName)
    {
      this.attributes.Set("Check", 2, (object) constraintName);
      return this;
    }

    public JoinedSubClassPart<TSubclass> Proxy(Type type)
    {
      this.attributes.Set(nameof (Proxy), 2, (object) type.AssemblyQualifiedName);
      return this;
    }

    public JoinedSubClassPart<TSubclass> Proxy<T>() => this.Proxy(typeof (T));

    public JoinedSubClassPart<TSubclass> LazyLoad()
    {
      this.attributes.Set("Lazy", 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public JoinedSubClassPart<TSubclass> DynamicUpdate()
    {
      this.attributes.Set(nameof (DynamicUpdate), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public JoinedSubClassPart<TSubclass> DynamicInsert()
    {
      this.attributes.Set(nameof (DynamicInsert), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public JoinedSubClassPart<TSubclass> SelectBeforeUpdate()
    {
      this.attributes.Set(nameof (SelectBeforeUpdate), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public JoinedSubClassPart<TSubclass> Abstract()
    {
      this.attributes.Set(nameof (Abstract), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public JoinedSubClassPart<TSubclass> EntityName(string entityName)
    {
      this.attributes.Set(nameof (EntityName), 2, (object) entityName);
      return this;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public JoinedSubClassPart<TSubclass> Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return this;
      }
    }

    SubclassMapping ISubclassMappingProvider.GetSubclassMapping()
    {
      SubclassMapping subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass, this.attributes.Clone());
      subclassMapping.Set<KeyMapping>((Expression<Func<SubclassMapping, KeyMapping>>) (x => x.Key), 0, new KeyMapping()
      {
        ContainingEntityType = typeof (TSubclass)
      });
      subclassMapping.Set<string>((Expression<Func<SubclassMapping, string>>) (x => x.Name), 0, typeof (TSubclass).AssemblyQualifiedName);
      subclassMapping.Set<Type>((Expression<Func<SubclassMapping, Type>>) (x => x.Type), 0, typeof (TSubclass));
      foreach (ColumnMapping column in this.columns)
        subclassMapping.Key.AddColumn(0, column);
      foreach (IPropertyMappingProvider property in (IEnumerable<IPropertyMappingProvider>) this.providers.Properties)
        subclassMapping.AddProperty(property.GetPropertyMapping());
      foreach (IComponentMappingProvider component in (IEnumerable<IComponentMappingProvider>) this.providers.Components)
        subclassMapping.AddComponent(component.GetComponentMapping());
      foreach (IOneToOneMappingProvider oneToOne in (IEnumerable<IOneToOneMappingProvider>) this.providers.OneToOnes)
        subclassMapping.AddOneToOne(oneToOne.GetOneToOneMapping());
      foreach (ICollectionMappingProvider collection in (IEnumerable<ICollectionMappingProvider>) this.providers.Collections)
        subclassMapping.AddCollection(collection.GetCollectionMapping());
      foreach (IManyToOneMappingProvider reference in (IEnumerable<IManyToOneMappingProvider>) this.providers.References)
        subclassMapping.AddReference(reference.GetManyToOneMapping());
      foreach (IAnyMappingProvider any in (IEnumerable<IAnyMappingProvider>) this.providers.Anys)
        subclassMapping.AddAny(any.GetAnyMapping());
      return subclassMapping;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.SubclassMap`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class SubclassMap<T> : ClasslikeMapBase<T>, IIndeterminateSubclassMappingProvider
  {
    private readonly MappingProviderStore providers;
    private readonly AttributeStore attributes = new AttributeStore();
    private readonly IDictionary<Type, IIndeterminateSubclassMappingProvider> indetermineateSubclasses = (IDictionary<Type, IIndeterminateSubclassMappingProvider>) new Dictionary<Type, IIndeterminateSubclassMappingProvider>();
    private bool nextBool = true;
    private readonly IList<JoinMapping> joins = (IList<JoinMapping>) new List<JoinMapping>();

    public SubclassMap()
      : this(new MappingProviderStore())
    {
    }

    protected SubclassMap(MappingProviderStore providers)
      : base(providers)
    {
      this.providers = providers;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public SubclassMap<T> Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return this;
      }
    }

    public void Abstract()
    {
      this.attributes.Set(nameof (Abstract), 2, (object) this.nextBool);
      this.nextBool = true;
    }

    public void DynamicInsert()
    {
      this.attributes.Set(nameof (DynamicInsert), 2, (object) this.nextBool);
      this.nextBool = true;
    }

    public void DynamicUpdate()
    {
      this.attributes.Set(nameof (DynamicUpdate), 2, (object) this.nextBool);
      this.nextBool = true;
    }

    public void LazyLoad()
    {
      this.attributes.Set("Lazy", 2, (object) this.nextBool);
      this.nextBool = true;
    }

    public void Proxy<TProxy>() => this.Proxy(typeof (TProxy));

    public void Proxy(Type proxyType)
    {
      this.attributes.Set(nameof (Proxy), 2, (object) proxyType.AssemblyQualifiedName);
    }

    public void SelectBeforeUpdate()
    {
      this.attributes.Set(nameof (SelectBeforeUpdate), 2, (object) this.nextBool);
      this.nextBool = true;
    }

    [Obsolete("Use a new SubclassMap")]
    public void Subclass<TSubclass>(Action<SubclassMap<TSubclass>> subclassDefinition)
    {
      SubclassMap<TSubclass> subclassMap = new SubclassMap<TSubclass>();
      subclassDefinition(subclassMap);
      this.indetermineateSubclasses[typeof (TSubclass)] = (IIndeterminateSubclassMappingProvider) subclassMap;
    }

    public void DiscriminatorValue(object discriminatorValue)
    {
      this.attributes.Set(nameof (DiscriminatorValue), 2, discriminatorValue);
    }

    public void Table(string table) => this.attributes.Set("TableName", 2, (object) table);

    public void Schema(string schema) => this.attributes.Set(nameof (Schema), 2, (object) schema);

    public void Check(string constraint)
    {
      this.attributes.Set(nameof (Check), 2, (object) constraint);
    }

    public void KeyColumn(string column)
    {
      KeyMapping keyMapping = !this.attributes.IsSpecified("Key") ? new KeyMapping() : this.attributes.GetOrDefault<KeyMapping>("Key");
      keyMapping.AddColumn(2, new ColumnMapping(column));
      this.attributes.Set("Key", 2, (object) keyMapping);
    }

    public void Subselect(string subselect)
    {
      this.attributes.Set(nameof (Subselect), 2, (object) subselect);
    }

    public void Persister<TPersister>()
    {
      this.attributes.Set(nameof (Persister), 2, (object) new TypeReference(typeof (TPersister)));
    }

    public void Persister(Type type)
    {
      this.attributes.Set(nameof (Persister), 2, (object) new TypeReference(type));
    }

    public void Persister(string type)
    {
      this.attributes.Set(nameof (Persister), 2, (object) new TypeReference(type));
    }

    public void BatchSize(int batchSize)
    {
      this.attributes.Set(nameof (BatchSize), 2, (object) batchSize);
    }

    public void EntityName(string entityname)
    {
      this.attributes.Set(nameof (EntityName), 2, (object) entityname);
    }

    public void Join(string tableName, Action<JoinPart<T>> action)
    {
      JoinPart<T> joinPart = new JoinPart<T>(tableName);
      action(joinPart);
      this.joins.Add(((IJoinMappingProvider) joinPart).GetJoinMapping());
    }

    public void Extends<TOther>() => this.Extends(typeof (TOther));

    public void Extends(Type type) => this.attributes.Set(nameof (Extends), 2, (object) type);

    SubclassMapping IIndeterminateSubclassMappingProvider.GetSubclassMapping(SubclassType type)
    {
      SubclassMapping mapping = new SubclassMapping(type);
      this.GenerateNestedSubclasses(mapping);
      this.attributes.Set("Type", 0, (object) typeof (T));
      this.attributes.Set("Name", 0, (object) typeof (T).AssemblyQualifiedName);
      this.attributes.Set("DiscriminatorValue", 0, (object) typeof (T).Name);
      KeyMapping keyMapping = new KeyMapping();
      keyMapping.AddColumn(0, new ColumnMapping(typeof (T).BaseType.Name + "_id"));
      this.attributes.Set("TableName", 0, (object) this.GetDefaultTableName());
      this.attributes.Set("Key", 0, (object) keyMapping);
      mapping.OverrideAttributes(this.attributes.Clone());
      foreach (JoinMapping join in (IEnumerable<JoinMapping>) this.joins)
        mapping.AddJoin(join);
      foreach (IPropertyMappingProvider property in (IEnumerable<IPropertyMappingProvider>) this.providers.Properties)
        mapping.AddProperty(property.GetPropertyMapping());
      foreach (IComponentMappingProvider component in (IEnumerable<IComponentMappingProvider>) this.providers.Components)
        mapping.AddComponent(component.GetComponentMapping());
      foreach (IOneToOneMappingProvider oneToOne in (IEnumerable<IOneToOneMappingProvider>) this.providers.OneToOnes)
        mapping.AddOneToOne(oneToOne.GetOneToOneMapping());
      foreach (ICollectionMappingProvider collection in (IEnumerable<ICollectionMappingProvider>) this.providers.Collections)
        mapping.AddCollection(collection.GetCollectionMapping());
      foreach (IManyToOneMappingProvider reference in (IEnumerable<IManyToOneMappingProvider>) this.providers.References)
        mapping.AddReference(reference.GetManyToOneMapping());
      foreach (IAnyMappingProvider any in (IEnumerable<IAnyMappingProvider>) this.providers.Anys)
        mapping.AddAny(any.GetAnyMapping());
      return mapping.DeepClone<SubclassMapping>();
    }

    Type IIndeterminateSubclassMappingProvider.EntityType => this.EntityType;

    Type IIndeterminateSubclassMappingProvider.Extends
    {
      get => this.attributes.GetOrDefault<Type>("Extends");
    }

    private void GenerateNestedSubclasses(SubclassMapping mapping)
    {
      foreach (Type key in (IEnumerable<Type>) this.indetermineateSubclasses.Keys)
      {
        SubclassMapping subclassMapping = this.indetermineateSubclasses[key].GetSubclassMapping(mapping.SubclassType);
        mapping.AddSubclass(subclassMapping);
      }
    }

    private string GetDefaultTableName()
    {
      string str = this.EntityType.Name;
      if (this.EntityType.IsGenericType)
      {
        str = this.EntityType.Name.Substring(0, this.EntityType.Name.IndexOf('`'));
        foreach (Type genericArgument in this.EntityType.GetGenericArguments())
          str = str + "_" + genericArgument.Name;
      }
      return "`" + str + "`";
    }
  }
}

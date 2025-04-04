// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.ClassMap`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using NHibernate.Persister.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class ClassMap<T> : ClasslikeMapBase<T>, IMappingProvider
  {
    protected readonly AttributeStore attributes;
    private readonly MappingProviderStore providers;
    private readonly OptimisticLockBuilder<ClassMap<T>> optimisticLock;
    private readonly IList<ImportPart> imports = (IList<ImportPart>) new List<ImportPart>();
    private bool nextBool = true;
    private readonly HibernateMappingPart hibernateMappingPart = new HibernateMappingPart();
    private readonly PolymorphismBuilder<ClassMap<T>> polymorphism;
    private readonly SchemaActionBuilder<ClassMap<T>> schemaAction;

    public ClassMap()
      : this(new AttributeStore(), new MappingProviderStore())
    {
    }

    protected ClassMap(AttributeStore attributes, MappingProviderStore providers)
      : base(providers)
    {
      this.attributes = attributes;
      this.providers = providers;
      this.optimisticLock = new OptimisticLockBuilder<ClassMap<T>>(this, (Action<string>) (value => attributes.Set(nameof (OptimisticLock), 2, (object) value)));
      this.polymorphism = new PolymorphismBuilder<ClassMap<T>>(this, (Action<string>) (value => attributes.Set(nameof (Polymorphism), 2, (object) value)));
      this.schemaAction = new SchemaActionBuilder<ClassMap<T>>(this, (Action<string>) (value => attributes.Set(nameof (SchemaAction), 2, (object) value)));
      this.Cache = new CachePart(typeof (T));
    }

    public CachePart Cache { get; private set; }

    public HibernateMappingPart HibernateMapping => this.hibernateMappingPart;

    public IdentityPart Id(Expression<Func<T, object>> memberExpression)
    {
      return this.Id(memberExpression, (string) null);
    }

    public IdentityPart Id(Expression<Func<T, object>> memberExpression, string column)
    {
      Member member = memberExpression.ToMember<T, object>();
      this.OnMemberMapped(member);
      IdentityPart identityPart = new IdentityPart(this.EntityType, member);
      if (column != null)
        identityPart.Column(column);
      this.providers.Id = (IIdentityMappingProvider) identityPart;
      return identityPart;
    }

    public IdentityPart Id() => this.Id<int>((string) null).GeneratedBy.Increment();

    public IdentityPart Id<TId>() => this.Id<TId>((string) null);

    public IdentityPart Id<TId>(string column)
    {
      IdentityPart identityPart = new IdentityPart(typeof (T), typeof (TId));
      if (column != null)
      {
        identityPart.SetName(column);
        identityPart.Column(column);
      }
      this.providers.Id = (IIdentityMappingProvider) identityPart;
      return identityPart;
    }

    public NaturalIdPart<T> NaturalId()
    {
      NaturalIdPart<T> naturalIdPart = new NaturalIdPart<T>();
      this.providers.NaturalId = (INaturalIdMappingProvider) naturalIdPart;
      return naturalIdPart;
    }

    public CompositeIdentityPart<T> CompositeId()
    {
      CompositeIdentityPart<T> compositeIdentityPart = new CompositeIdentityPart<T>(new Action<Member>(((ClasslikeMapBase<T>) this).OnMemberMapped));
      this.providers.CompositeId = (ICompositeIdMappingProvider) compositeIdentityPart;
      return compositeIdentityPart;
    }

    public CompositeIdentityPart<TId> CompositeId<TId>(Expression<Func<T, TId>> memberExpression)
    {
      Member member = memberExpression.ToMember<T, TId>();
      this.OnMemberMapped(member);
      CompositeIdentityPart<TId> compositeIdentityPart = new CompositeIdentityPart<TId>(member.Name, new Action<Member>(((ClasslikeMapBase<T>) this).OnMemberMapped));
      this.providers.CompositeId = (ICompositeIdMappingProvider) compositeIdentityPart;
      return compositeIdentityPart;
    }

    public VersionPart Version(Expression<Func<T, object>> memberExpression)
    {
      return this.Version(memberExpression.ToMember<T, object>());
    }

    private VersionPart Version(Member member)
    {
      this.OnMemberMapped(member);
      VersionPart versionPart = new VersionPart(typeof (T), member);
      this.providers.Version = (IVersionMappingProvider) versionPart;
      return versionPart;
    }

    public DiscriminatorPart DiscriminateSubClassesOnColumn<TDiscriminator>(
      string columnName,
      TDiscriminator baseClassDiscriminator)
    {
      DiscriminatorPart discriminatorPart = new DiscriminatorPart(columnName, typeof (T), new Action<Type, ISubclassMappingProvider>(this.providers.Subclasses.Add), new TypeReference(typeof (TDiscriminator)));
      this.providers.Discriminator = (IDiscriminatorMappingProvider) discriminatorPart;
      this.attributes.Set("DiscriminatorValue", 2, (object) baseClassDiscriminator);
      return discriminatorPart;
    }

    public DiscriminatorPart DiscriminateSubClassesOnColumn<TDiscriminator>(string columnName)
    {
      DiscriminatorPart discriminatorPart = new DiscriminatorPart(columnName, typeof (T), new Action<Type, ISubclassMappingProvider>(this.providers.Subclasses.Add), new TypeReference(typeof (TDiscriminator)));
      this.providers.Discriminator = (IDiscriminatorMappingProvider) discriminatorPart;
      return discriminatorPart;
    }

    public DiscriminatorPart DiscriminateSubClassesOnColumn(string columnName)
    {
      return this.DiscriminateSubClassesOnColumn<string>(columnName);
    }

    public void UseUnionSubclassForInheritanceMapping()
    {
      this.attributes.Set("Abstract", 2, (object) true);
      this.attributes.Set("IsUnionSubclass", 2, (object) true);
    }

    [Obsolete("Inline definitions of subclasses are depreciated. Please create a derived class from SubclassMap in the same way you do with ClassMap.")]
    public void JoinedSubClass<TSubclass>(
      string keyColumn,
      Action<JoinedSubClassPart<TSubclass>> action)
      where TSubclass : T
    {
      JoinedSubClassPart<TSubclass> joinedSubClassPart = new JoinedSubClassPart<TSubclass>(keyColumn);
      action(joinedSubClassPart);
      this.providers.Subclasses[typeof (TSubclass)] = (ISubclassMappingProvider) joinedSubClassPart;
    }

    public void Schema(string schema) => this.attributes.Set(nameof (Schema), 2, (object) schema);

    public void Table(string tableName) => this.attributes.Set("TableName", 2, (object) tableName);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ClassMap<T> Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return this;
      }
    }

    public void LazyLoad()
    {
      this.attributes.Set("Lazy", 2, (object) this.nextBool);
      this.nextBool = true;
    }

    public void Join(string tableName, Action<JoinPart<T>> action)
    {
      JoinPart<T> joinPart = new JoinPart<T>(tableName);
      action(joinPart);
      this.providers.Joins.Add((IJoinMappingProvider) joinPart);
    }

    public ImportPart ImportType<TImport>()
    {
      ImportPart importPart = new ImportPart(typeof (TImport));
      this.imports.Add(importPart);
      return importPart;
    }

    public void ReadOnly()
    {
      this.attributes.Set("Mutable", 2, (object) !this.nextBool);
      this.nextBool = true;
    }

    public void DynamicUpdate()
    {
      this.attributes.Set(nameof (DynamicUpdate), 2, (object) this.nextBool);
      this.nextBool = true;
    }

    public void DynamicInsert()
    {
      this.attributes.Set(nameof (DynamicInsert), 2, (object) this.nextBool);
      this.nextBool = true;
    }

    public ClassMap<T> BatchSize(int size)
    {
      this.attributes.Set(nameof (BatchSize), 2, (object) size);
      return this;
    }

    public OptimisticLockBuilder<ClassMap<T>> OptimisticLock => this.optimisticLock;

    public PolymorphismBuilder<ClassMap<T>> Polymorphism => this.polymorphism;

    public SchemaActionBuilder<ClassMap<T>> SchemaAction => this.schemaAction;

    public void CheckConstraint(string constraint)
    {
      this.attributes.Set("Check", 2, (object) constraint);
    }

    public void Persister<TPersister>() where TPersister : IEntityPersister
    {
      this.Persister(typeof (TPersister));
    }

    private void Persister(Type type) => this.Persister(type.AssemblyQualifiedName);

    public void Persister(string type) => this.attributes.Set(nameof (Persister), 2, (object) type);

    public void Proxy<TProxy>() => this.Proxy(typeof (TProxy));

    public void Proxy(Type type) => this.Proxy(type.AssemblyQualifiedName);

    public void Proxy(string type) => this.attributes.Set(nameof (Proxy), 2, (object) type);

    public void SelectBeforeUpdate()
    {
      this.attributes.Set(nameof (SelectBeforeUpdate), 2, (object) this.nextBool);
      this.nextBool = true;
    }

    public void Where(string where) => this.attributes.Set(nameof (Where), 2, (object) where);

    public void Subselect(string subselectSql)
    {
      this.attributes.Set(nameof (Subselect), 2, (object) subselectSql);
    }

    public void EntityName(string entityName)
    {
      this.attributes.Set(nameof (EntityName), 2, (object) entityName);
    }

    public ClassMap<T> ApplyFilter(string name, string condition)
    {
      this.providers.Filters.Add((IFilterMappingProvider) new FilterPart(name, condition));
      return this;
    }

    public ClassMap<T> ApplyFilter(string name) => this.ApplyFilter(name, (string) null);

    public ClassMap<T> ApplyFilter<TFilter>(string condition) where TFilter : FilterDefinition, new()
    {
      return this.ApplyFilter(new TFilter().Name, condition);
    }

    public ClassMap<T> ApplyFilter<TFilter>() where TFilter : FilterDefinition, new()
    {
      return this.ApplyFilter<TFilter>((string) null);
    }

    public TuplizerPart Tuplizer(TuplizerMode mode, Type tuplizerType)
    {
      this.providers.TuplizerMapping = new TuplizerMapping();
      this.providers.TuplizerMapping.Set<TuplizerMode>((Expression<Func<TuplizerMapping, TuplizerMode>>) (x => x.Mode), 2, mode);
      this.providers.TuplizerMapping.Set<TypeReference>((Expression<Func<TuplizerMapping, TypeReference>>) (x => x.Type), 2, new TypeReference(tuplizerType));
      return new TuplizerPart(this.providers.TuplizerMapping).Type(tuplizerType).Mode(mode);
    }

    ClassMapping IMappingProvider.GetClassMapping()
    {
      ClassMapping classMapping = new ClassMapping(this.attributes.Clone());
      classMapping.Set<Type>((Expression<Func<ClassMapping, Type>>) (x => x.Type), 0, typeof (T));
      classMapping.Set<string>((Expression<Func<ClassMapping, string>>) (x => x.Name), 0, typeof (T).AssemblyQualifiedName);
      foreach (IPropertyMappingProvider property in (IEnumerable<IPropertyMappingProvider>) this.providers.Properties)
        classMapping.AddProperty(property.GetPropertyMapping());
      foreach (IComponentMappingProvider component in (IEnumerable<IComponentMappingProvider>) this.providers.Components)
        classMapping.AddComponent(component.GetComponentMapping());
      if (this.providers.Version != null)
        classMapping.Set<VersionMapping>((Expression<Func<ClassMapping, VersionMapping>>) (x => x.Version), 0, this.providers.Version.GetVersionMapping());
      foreach (IOneToOneMappingProvider oneToOne in (IEnumerable<IOneToOneMappingProvider>) this.providers.OneToOnes)
        classMapping.AddOneToOne(oneToOne.GetOneToOneMapping());
      foreach (ICollectionMappingProvider collection in (IEnumerable<ICollectionMappingProvider>) this.providers.Collections)
        classMapping.AddCollection(collection.GetCollectionMapping());
      foreach (IManyToOneMappingProvider reference in (IEnumerable<IManyToOneMappingProvider>) this.providers.References)
        classMapping.AddReference(reference.GetManyToOneMapping());
      foreach (IAnyMappingProvider any in (IEnumerable<IAnyMappingProvider>) this.providers.Anys)
        classMapping.AddAny(any.GetAnyMapping());
      foreach (ISubclassMappingProvider subclassMappingProvider in this.providers.Subclasses.Values)
        classMapping.AddSubclass(subclassMappingProvider.GetSubclassMapping());
      foreach (IJoinMappingProvider join in (IEnumerable<IJoinMappingProvider>) this.providers.Joins)
        classMapping.AddJoin(join.GetJoinMapping());
      if (this.providers.Discriminator != null)
        classMapping.Set<DiscriminatorMapping>((Expression<Func<ClassMapping, DiscriminatorMapping>>) (x => x.Discriminator), 0, this.providers.Discriminator.GetDiscriminatorMapping());
      if (this.Cache.IsDirty)
        classMapping.Set<CacheMapping>((Expression<Func<ClassMapping, CacheMapping>>) (x => x.Cache), 0, ((ICacheMappingProvider) this.Cache).GetCacheMapping());
      if (this.providers.Id != null)
        classMapping.Set<IIdentityMapping>((Expression<Func<ClassMapping, IIdentityMapping>>) (x => x.Id), 0, (IIdentityMapping) this.providers.Id.GetIdentityMapping());
      if (this.providers.CompositeId != null)
        classMapping.Set<IIdentityMapping>((Expression<Func<ClassMapping, IIdentityMapping>>) (x => x.Id), 0, (IIdentityMapping) this.providers.CompositeId.GetCompositeIdMapping());
      if (this.providers.NaturalId != null)
        classMapping.Set<NaturalIdMapping>((Expression<Func<ClassMapping, NaturalIdMapping>>) (x => x.NaturalId), 0, this.providers.NaturalId.GetNaturalIdMapping());
      classMapping.Set<string>((Expression<Func<ClassMapping, string>>) (x => x.TableName), 0, this.GetDefaultTableName());
      foreach (IFilterMappingProvider filter in (IEnumerable<IFilterMappingProvider>) this.providers.Filters)
        classMapping.AddFilter(filter.GetFilterMapping());
      foreach (IStoredProcedureMappingProvider storedProcedure in (IEnumerable<IStoredProcedureMappingProvider>) this.providers.StoredProcedures)
        classMapping.AddStoredProcedure(storedProcedure.GetStoredProcedureMapping());
      classMapping.Set<TuplizerMapping>((Expression<Func<ClassMapping, TuplizerMapping>>) (x => x.Tuplizer), 0, this.providers.TuplizerMapping);
      return classMapping;
    }

    FluentNHibernate.MappingModel.HibernateMapping IMappingProvider.GetHibernateMapping()
    {
      FluentNHibernate.MappingModel.HibernateMapping hibernateMapping = ((IHibernateMappingProvider) this.hibernateMappingPart).GetHibernateMapping();
      foreach (ImportPart import in (IEnumerable<ImportPart>) this.imports)
        hibernateMapping.AddImport(import.GetImportMapping());
      return hibernateMapping;
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

    IEnumerable<Member> IMappingProvider.GetIgnoredProperties()
    {
      return (IEnumerable<Member>) new Member[0];
    }
  }
}

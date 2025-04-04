// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.ExplicitDeclarationsHolder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class ExplicitDeclarationsHolder : IModelExplicitDeclarationsHolder
  {
    private readonly HashSet<MemberInfo> any = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> arrays = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> bags = new HashSet<MemberInfo>();
    private readonly HashSet<Type> components = new HashSet<Type>();
    private readonly HashSet<MemberInfo> dictionaries = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> idBags = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> lists = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> manyToManyRelations = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> manyToOneRelations = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> manyToAnyRelations = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> naturalIds = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> oneToManyRelations = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> oneToOneRelations = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> poids = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> composedIds = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> properties = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> dynamicComponents = new HashSet<MemberInfo>();
    private readonly Dictionary<MemberInfo, Type> dynamicComponentTemplates = new Dictionary<MemberInfo, Type>();
    private readonly HashSet<MemberInfo> persistentMembers = new HashSet<MemberInfo>();
    private readonly HashSet<Type> rootEntities = new HashSet<Type>();
    private readonly HashSet<MemberInfo> sets = new HashSet<MemberInfo>();
    private readonly HashSet<SplitDefinition> splitDefinitions = new HashSet<SplitDefinition>();
    private readonly HashSet<Type> tablePerClassEntities = new HashSet<Type>();
    private readonly HashSet<Type> tablePerClassHierarchyEntities = new HashSet<Type>();
    private readonly HashSet<Type> tablePerConcreteClassEntities = new HashSet<Type>();
    private readonly HashSet<MemberInfo> versionProperties = new HashSet<MemberInfo>();

    public IEnumerable<Type> RootEntities => (IEnumerable<Type>) this.rootEntities;

    public IEnumerable<Type> Components => (IEnumerable<Type>) this.components;

    public IEnumerable<Type> TablePerClassEntities
    {
      get => (IEnumerable<Type>) this.tablePerClassEntities;
    }

    public IEnumerable<Type> TablePerClassHierarchyEntities
    {
      get => (IEnumerable<Type>) this.tablePerClassHierarchyEntities;
    }

    public IEnumerable<Type> TablePerConcreteClassEntities
    {
      get => (IEnumerable<Type>) this.tablePerConcreteClassEntities;
    }

    public IEnumerable<MemberInfo> OneToOneRelations
    {
      get => (IEnumerable<MemberInfo>) this.oneToOneRelations;
    }

    public IEnumerable<MemberInfo> ManyToOneRelations
    {
      get => (IEnumerable<MemberInfo>) this.manyToOneRelations;
    }

    public IEnumerable<MemberInfo> ManyToManyRelations
    {
      get => (IEnumerable<MemberInfo>) this.manyToManyRelations;
    }

    public IEnumerable<MemberInfo> OneToManyRelations
    {
      get => (IEnumerable<MemberInfo>) this.oneToManyRelations;
    }

    public IEnumerable<MemberInfo> ManyToAnyRelations
    {
      get => (IEnumerable<MemberInfo>) this.manyToAnyRelations;
    }

    public IEnumerable<MemberInfo> Any => (IEnumerable<MemberInfo>) this.any;

    public IEnumerable<MemberInfo> Poids => (IEnumerable<MemberInfo>) this.poids;

    public IEnumerable<MemberInfo> ComposedIds => (IEnumerable<MemberInfo>) this.composedIds;

    public IEnumerable<MemberInfo> VersionProperties
    {
      get => (IEnumerable<MemberInfo>) this.versionProperties;
    }

    public IEnumerable<MemberInfo> NaturalIds => (IEnumerable<MemberInfo>) this.naturalIds;

    public IEnumerable<MemberInfo> Sets => (IEnumerable<MemberInfo>) this.sets;

    public IEnumerable<MemberInfo> Bags => (IEnumerable<MemberInfo>) this.bags;

    public IEnumerable<MemberInfo> IdBags => (IEnumerable<MemberInfo>) this.idBags;

    public IEnumerable<MemberInfo> Lists => (IEnumerable<MemberInfo>) this.lists;

    public IEnumerable<MemberInfo> Arrays => (IEnumerable<MemberInfo>) this.arrays;

    public IEnumerable<MemberInfo> Dictionaries => (IEnumerable<MemberInfo>) this.dictionaries;

    public IEnumerable<MemberInfo> Properties => (IEnumerable<MemberInfo>) this.properties;

    public IEnumerable<MemberInfo> DynamicComponents
    {
      get => (IEnumerable<MemberInfo>) this.dynamicComponents;
    }

    public IEnumerable<MemberInfo> PersistentMembers
    {
      get => (IEnumerable<MemberInfo>) this.persistentMembers;
    }

    public IEnumerable<SplitDefinition> SplitDefinitions
    {
      get => (IEnumerable<SplitDefinition>) this.splitDefinitions;
    }

    public IEnumerable<string> GetSplitGroupsFor(Type type) => Enumerable.Empty<string>();

    public string GetSplitGroupFor(MemberInfo member) => (string) null;

    public Type GetDynamicComponentTemplate(MemberInfo member)
    {
      Type type;
      this.dynamicComponentTemplates.TryGetValue(member, out type);
      return type ?? typeof (object);
    }

    public void AddAsRootEntity(Type type) => this.rootEntities.Add(type);

    public void AddAsComponent(Type type) => this.components.Add(type);

    public void AddAsTablePerClassEntity(Type type) => this.tablePerClassEntities.Add(type);

    public void AddAsTablePerClassHierarchyEntity(Type type)
    {
      this.tablePerClassHierarchyEntities.Add(type);
    }

    public void AddAsTablePerConcreteClassEntity(Type type)
    {
      this.tablePerConcreteClassEntities.Add(type);
    }

    public void AddAsOneToOneRelation(MemberInfo member) => this.oneToOneRelations.Add(member);

    public void AddAsManyToOneRelation(MemberInfo member) => this.manyToOneRelations.Add(member);

    public void AddAsManyToManyRelation(MemberInfo member) => this.manyToManyRelations.Add(member);

    public void AddAsOneToManyRelation(MemberInfo member) => this.oneToManyRelations.Add(member);

    public void AddAsManyToAnyRelation(MemberInfo member) => this.manyToAnyRelations.Add(member);

    public void AddAsAny(MemberInfo member) => this.any.Add(member);

    public void AddAsPoid(MemberInfo member) => this.poids.Add(member);

    public void AddAsPartOfComposedId(MemberInfo member) => this.composedIds.Add(member);

    public void AddAsVersionProperty(MemberInfo member) => this.versionProperties.Add(member);

    public void AddAsNaturalId(MemberInfo member) => this.naturalIds.Add(member);

    public void AddAsSet(MemberInfo member) => this.sets.Add(member);

    public void AddAsBag(MemberInfo member) => this.bags.Add(member);

    public void AddAsIdBag(MemberInfo member) => this.idBags.Add(member);

    public void AddAsList(MemberInfo member) => this.lists.Add(member);

    public void AddAsArray(MemberInfo member) => this.arrays.Add(member);

    public void AddAsMap(MemberInfo member) => this.dictionaries.Add(member);

    public void AddAsProperty(MemberInfo member) => this.properties.Add(member);

    public void AddAsPersistentMember(MemberInfo member) => this.persistentMembers.Add(member);

    public void AddAsPropertySplit(SplitDefinition definition)
    {
      this.splitDefinitions.Add(definition);
    }

    public void AddAsDynamicComponent(MemberInfo member, Type componentTemplate)
    {
      this.dynamicComponents.Add(member);
      this.dynamicComponentTemplates[member] = componentTemplate;
    }
  }
}

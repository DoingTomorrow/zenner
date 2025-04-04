// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.FakeModelExplicitDeclarationsHolder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public class FakeModelExplicitDeclarationsHolder : IModelExplicitDeclarationsHolder
  {
    private readonly IEnumerable<MemberInfo> any = Enumerable.Empty<MemberInfo>();
    private readonly IEnumerable<MemberInfo> arrays = Enumerable.Empty<MemberInfo>();
    private readonly IEnumerable<MemberInfo> bags = Enumerable.Empty<MemberInfo>();
    private readonly IEnumerable<Type> components = Enumerable.Empty<Type>();
    private readonly IEnumerable<MemberInfo> dictionaries = Enumerable.Empty<MemberInfo>();
    private readonly IEnumerable<MemberInfo> idBags = Enumerable.Empty<MemberInfo>();
    private readonly IEnumerable<MemberInfo> lists = Enumerable.Empty<MemberInfo>();
    private readonly IEnumerable<MemberInfo> manyToManyRelations = Enumerable.Empty<MemberInfo>();
    private readonly IEnumerable<MemberInfo> manyToOneRelations = Enumerable.Empty<MemberInfo>();
    private readonly IEnumerable<MemberInfo> manyToAnyRelations = Enumerable.Empty<MemberInfo>();
    private readonly IEnumerable<MemberInfo> naturalIds = Enumerable.Empty<MemberInfo>();
    private readonly IEnumerable<MemberInfo> oneToManyRelations = Enumerable.Empty<MemberInfo>();
    private readonly IEnumerable<MemberInfo> oneToOneRelations = Enumerable.Empty<MemberInfo>();
    private readonly IEnumerable<MemberInfo> poids = Enumerable.Empty<MemberInfo>();
    private readonly IEnumerable<MemberInfo> composedIds = Enumerable.Empty<MemberInfo>();
    private readonly IEnumerable<MemberInfo> properties = Enumerable.Empty<MemberInfo>();
    private readonly IEnumerable<MemberInfo> dynamicComponents = Enumerable.Empty<MemberInfo>();
    private readonly IEnumerable<MemberInfo> persistentMembers = (IEnumerable<MemberInfo>) new HashSet<MemberInfo>();
    private readonly IEnumerable<Type> rootEntities = Enumerable.Empty<Type>();
    private readonly IEnumerable<MemberInfo> sets = Enumerable.Empty<MemberInfo>();
    private readonly IEnumerable<Type> tablePerClassEntities = Enumerable.Empty<Type>();
    private readonly IEnumerable<Type> tablePerClassHierarchyEntities = Enumerable.Empty<Type>();
    private readonly IEnumerable<Type> tablePerClassHierarchyJoinEntities = Enumerable.Empty<Type>();
    private readonly IEnumerable<Type> tablePerConcreteClassEntities = Enumerable.Empty<Type>();
    private readonly IEnumerable<MemberInfo> versionProperties = Enumerable.Empty<MemberInfo>();
    private readonly IEnumerable<SplitDefinition> splitDefinitions = Enumerable.Empty<SplitDefinition>();

    public IEnumerable<Type> RootEntities => this.rootEntities;

    public IEnumerable<Type> Components => this.components;

    public IEnumerable<Type> TablePerClassEntities => this.tablePerClassEntities;

    public IEnumerable<Type> TablePerClassHierarchyEntities => this.tablePerClassHierarchyEntities;

    public IEnumerable<Type> TablePerClassHierarchyJoinEntities
    {
      get => this.tablePerClassHierarchyJoinEntities;
    }

    public IEnumerable<Type> TablePerConcreteClassEntities => this.tablePerConcreteClassEntities;

    public IEnumerable<MemberInfo> OneToOneRelations => this.oneToOneRelations;

    public IEnumerable<MemberInfo> ManyToOneRelations => this.manyToOneRelations;

    public IEnumerable<MemberInfo> ManyToManyRelations => this.manyToManyRelations;

    public IEnumerable<MemberInfo> OneToManyRelations => this.oneToManyRelations;

    public IEnumerable<MemberInfo> ManyToAnyRelations => this.manyToAnyRelations;

    public IEnumerable<MemberInfo> Any => this.any;

    public IEnumerable<MemberInfo> Poids => this.poids;

    public IEnumerable<MemberInfo> ComposedIds => this.composedIds;

    public IEnumerable<MemberInfo> VersionProperties => this.versionProperties;

    public IEnumerable<MemberInfo> NaturalIds => this.naturalIds;

    public IEnumerable<MemberInfo> Sets => this.sets;

    public IEnumerable<MemberInfo> Bags => this.bags;

    public IEnumerable<MemberInfo> IdBags => this.idBags;

    public IEnumerable<MemberInfo> Lists => this.lists;

    public IEnumerable<MemberInfo> Arrays => this.arrays;

    public IEnumerable<MemberInfo> Dictionaries => this.dictionaries;

    public IEnumerable<MemberInfo> Properties => this.properties;

    public IEnumerable<MemberInfo> DynamicComponents => this.dynamicComponents;

    public IEnumerable<MemberInfo> PersistentMembers => this.persistentMembers;

    public IEnumerable<SplitDefinition> SplitDefinitions => this.splitDefinitions;

    public IEnumerable<string> GetSplitGroupsFor(Type type) => Enumerable.Empty<string>();

    public string GetSplitGroupFor(MemberInfo member) => (string) null;

    public Type GetDynamicComponentTemplate(MemberInfo member) => typeof (object);

    public void AddAsRootEntity(Type type)
    {
    }

    public void AddAsComponent(Type type)
    {
    }

    public void AddAsTablePerClassEntity(Type type)
    {
    }

    public void AddAsTablePerClassHierarchyEntity(Type type)
    {
    }

    public void AddAsTablePerConcreteClassEntity(Type type)
    {
    }

    public void AddAsOneToOneRelation(MemberInfo member)
    {
    }

    public void AddAsManyToOneRelation(MemberInfo member)
    {
    }

    public void AddAsManyToManyRelation(MemberInfo member)
    {
    }

    public void AddAsOneToManyRelation(MemberInfo member)
    {
    }

    public void AddAsManyToAnyRelation(MemberInfo member)
    {
    }

    public void AddAsAny(MemberInfo member)
    {
    }

    public void AddAsPoid(MemberInfo member)
    {
    }

    public void AddAsPartOfComposedId(MemberInfo member)
    {
    }

    public void AddAsVersionProperty(MemberInfo member)
    {
    }

    public void AddAsNaturalId(MemberInfo member)
    {
    }

    public void AddAsSet(MemberInfo member)
    {
    }

    public void AddAsBag(MemberInfo member)
    {
    }

    public void AddAsIdBag(MemberInfo member)
    {
    }

    public void AddAsList(MemberInfo member)
    {
    }

    public void AddAsArray(MemberInfo member)
    {
    }

    public void AddAsMap(MemberInfo member)
    {
    }

    public void AddAsProperty(MemberInfo member)
    {
    }

    public void AddAsPersistentMember(MemberInfo member)
    {
    }

    public void AddAsPropertySplit(SplitDefinition definition)
    {
    }

    public void AddAsDynamicComponent(MemberInfo member, Type componentTemplate)
    {
    }
  }
}

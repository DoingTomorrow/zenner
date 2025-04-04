// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.IModelExplicitDeclarationsHolder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface IModelExplicitDeclarationsHolder
  {
    IEnumerable<Type> RootEntities { get; }

    IEnumerable<Type> Components { get; }

    IEnumerable<Type> TablePerClassEntities { get; }

    IEnumerable<Type> TablePerClassHierarchyEntities { get; }

    IEnumerable<Type> TablePerConcreteClassEntities { get; }

    IEnumerable<MemberInfo> OneToOneRelations { get; }

    IEnumerable<MemberInfo> ManyToOneRelations { get; }

    IEnumerable<MemberInfo> ManyToManyRelations { get; }

    IEnumerable<MemberInfo> OneToManyRelations { get; }

    IEnumerable<MemberInfo> ManyToAnyRelations { get; }

    IEnumerable<MemberInfo> Any { get; }

    IEnumerable<MemberInfo> Poids { get; }

    IEnumerable<MemberInfo> ComposedIds { get; }

    IEnumerable<MemberInfo> VersionProperties { get; }

    IEnumerable<MemberInfo> NaturalIds { get; }

    IEnumerable<MemberInfo> Sets { get; }

    IEnumerable<MemberInfo> Bags { get; }

    IEnumerable<MemberInfo> IdBags { get; }

    IEnumerable<MemberInfo> Lists { get; }

    IEnumerable<MemberInfo> Arrays { get; }

    IEnumerable<MemberInfo> Dictionaries { get; }

    IEnumerable<MemberInfo> Properties { get; }

    IEnumerable<MemberInfo> DynamicComponents { get; }

    IEnumerable<MemberInfo> PersistentMembers { get; }

    IEnumerable<SplitDefinition> SplitDefinitions { get; }

    IEnumerable<string> GetSplitGroupsFor(Type type);

    string GetSplitGroupFor(MemberInfo member);

    Type GetDynamicComponentTemplate(MemberInfo member);

    void AddAsRootEntity(Type type);

    void AddAsComponent(Type type);

    void AddAsTablePerClassEntity(Type type);

    void AddAsTablePerClassHierarchyEntity(Type type);

    void AddAsTablePerConcreteClassEntity(Type type);

    void AddAsOneToOneRelation(MemberInfo member);

    void AddAsManyToOneRelation(MemberInfo member);

    void AddAsManyToManyRelation(MemberInfo member);

    void AddAsOneToManyRelation(MemberInfo member);

    void AddAsManyToAnyRelation(MemberInfo member);

    void AddAsAny(MemberInfo member);

    void AddAsPoid(MemberInfo member);

    void AddAsPartOfComposedId(MemberInfo member);

    void AddAsVersionProperty(MemberInfo member);

    void AddAsNaturalId(MemberInfo member);

    void AddAsSet(MemberInfo member);

    void AddAsBag(MemberInfo member);

    void AddAsIdBag(MemberInfo member);

    void AddAsList(MemberInfo member);

    void AddAsArray(MemberInfo member);

    void AddAsMap(MemberInfo member);

    void AddAsProperty(MemberInfo member);

    void AddAsPersistentMember(MemberInfo member);

    void AddAsPropertySplit(SplitDefinition definition);

    void AddAsDynamicComponent(MemberInfo member, Type componentTemplate);
  }
}

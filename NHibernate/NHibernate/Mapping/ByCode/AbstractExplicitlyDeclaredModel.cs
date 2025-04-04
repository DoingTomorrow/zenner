// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.AbstractExplicitlyDeclaredModel
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public abstract class AbstractExplicitlyDeclaredModel : IModelExplicitDeclarationsHolder
  {
    private readonly HashSet<MemberInfo> any = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> arrays = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> bags = new HashSet<MemberInfo>();
    private readonly HashSet<Type> components = new HashSet<Type>();
    private readonly HashSet<MemberInfo> dictionaries = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> idBags = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> lists = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> manyToManyRelations = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> manyToAnyRelations = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> manyToOneRelations = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> naturalIds = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> composedIds = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> oneToManyRelations = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> oneToOneRelations = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> poids = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> properties = new HashSet<MemberInfo>();
    private readonly HashSet<MemberInfo> dynamicComponents = new HashSet<MemberInfo>();
    private readonly Dictionary<MemberInfo, Type> dynamicComponentTemplates = new Dictionary<MemberInfo, Type>();
    private readonly HashSet<MemberInfo> persistentMembers = new HashSet<MemberInfo>();
    private readonly HashSet<Type> rootEntities = new HashSet<Type>();
    private readonly HashSet<MemberInfo> sets = new HashSet<MemberInfo>();
    private readonly HashSet<Type> tablePerClassEntities = new HashSet<Type>();
    private readonly HashSet<Type> tablePerClassHierarchyEntities = new HashSet<Type>();
    private readonly HashSet<Type> tablePerConcreteClassEntities = new HashSet<Type>();
    private readonly HashSet<MemberInfo> versionProperties = new HashSet<MemberInfo>();
    private readonly Dictionary<Type, HashSet<string>> typeSplitGroups = new Dictionary<Type, HashSet<string>>();
    private readonly Dictionary<MemberInfo, string> memberSplitGroup = new Dictionary<MemberInfo, string>();
    private readonly HashSet<SplitDefinition> splitDefinitions = new HashSet<SplitDefinition>();
    private readonly Queue<Action> delayedRootEntityRegistrations = new Queue<Action>();
    private readonly Dictionary<Type, List<Action<Type>>> delayedEntityRegistrations = new Dictionary<Type, List<Action<Type>>>();

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

    public IEnumerable<string> GetSplitGroupsFor(Type type)
    {
      HashSet<string> stringSet;
      return this.typeSplitGroups.TryGetValue(type, out stringSet) ? (IEnumerable<string>) stringSet : Enumerable.Empty<string>();
    }

    public string GetSplitGroupFor(MemberInfo member)
    {
      string str;
      return this.memberSplitGroup.TryGetValue(member.GetMemberFromDeclaringType(), out str) ? str : (string) null;
    }

    public void AddAsRootEntity(Type type)
    {
      this.rootEntities.Add(type);
      if (this.IsComponent(type))
        throw new MappingException(string.Format("Ambiguous mapping of {0}. It was registered as entity and as component", (object) type.FullName));
    }

    public abstract bool IsComponent(Type type);

    public void AddAsComponent(Type type)
    {
      this.components.Add(type);
      if (this.GetSingleRootEntityOrNull(type) != null)
        throw new MappingException(string.Format("Ambiguous mapping of {0}. It was registered as entity and as component", (object) type.FullName));
    }

    public void AddAsTablePerClassEntity(Type type) => this.AddAsTablePerClassEntity(type, false);

    protected virtual void AddAsTablePerClassEntity(Type type, bool rootEntityMustExists)
    {
      if (!rootEntityMustExists)
      {
        this.delayedRootEntityRegistrations.Enqueue((Action) (() => System.Array.ForEach<Type>(this.GetRootEntitiesOf(type).ToArray<Type>(), (Action<Type>) (root => this.tablePerClassEntities.Add(root)))));
        this.EnlistTypeRegistration(type, (Action<Type>) (t => this.AddAsTablePerClassEntity(t, true)));
      }
      else
      {
        Type type1 = !this.IsComponent(type) ? this.GetSingleRootEntityOrNull(type) : throw new MappingException(string.Format("Ambiguous mapping of {0}. It was registered as entity and as component", (object) type.FullName));
        if (type1 == null)
          throw new MappingException(string.Format("The root entity for {0} was never registered", (object) type.FullName));
        if (type1.Equals(type))
          throw new MappingException(string.Format("Ambiguous mapping of {0}. It was registered as root-entity and as subclass for table-per-class strategy", (object) type.FullName));
        if (this.IsMappedFor((ICollection<Type>) this.tablePerClassHierarchyEntities, type) || this.IsMappedFor((ICollection<Type>) this.tablePerConcreteClassEntities, type))
          throw new MappingException(string.Format("Ambiguous mapping of {0}. It was registered with more than one class-hierarchy strategy", (object) type.FullName));
      }
    }

    public void AddAsTablePerClassHierarchyEntity(Type type)
    {
      this.AddAsTablePerClassHierarchyEntity(type, false);
    }

    protected virtual void AddAsTablePerClassHierarchyEntity(Type type, bool rootEntityMustExists)
    {
      if (!rootEntityMustExists)
      {
        this.delayedRootEntityRegistrations.Enqueue((Action) (() => System.Array.ForEach<Type>(this.GetRootEntitiesOf(type).ToArray<Type>(), (Action<Type>) (root => this.tablePerClassHierarchyEntities.Add(root)))));
        this.EnlistTypeRegistration(type, (Action<Type>) (t => this.AddAsTablePerClassHierarchyEntity(t, true)));
      }
      else
      {
        Type type1 = !this.IsComponent(type) ? this.GetSingleRootEntityOrNull(type) : throw new MappingException(string.Format("Ambiguous mapping of {0}. It was registered as entity and as component", (object) type.FullName));
        if (type1 == null)
          throw new MappingException(string.Format("The root entity for {0} was never registered", (object) type.FullName));
        if (type1.Equals(type))
          throw new MappingException(string.Format("Ambiguous mapping of {0}. It was registered as root-entity and as subclass for table-per-class-hierarchy strategy", (object) type.FullName));
        if (this.IsMappedFor((ICollection<Type>) this.tablePerClassEntities, type) || this.IsMappedFor((ICollection<Type>) this.tablePerConcreteClassEntities, type))
          throw new MappingException(string.Format("Ambiguous mapping of {0}. It was registered with more than one class-hierarchy strategy", (object) type.FullName));
      }
    }

    public void AddAsTablePerConcreteClassEntity(Type type)
    {
      this.AddAsTablePerConcreteClassEntity(type, false);
    }

    protected virtual void AddAsTablePerConcreteClassEntity(Type type, bool rootEntityMustExists)
    {
      if (!rootEntityMustExists)
      {
        this.delayedRootEntityRegistrations.Enqueue((Action) (() => System.Array.ForEach<Type>(this.GetRootEntitiesOf(type).ToArray<Type>(), (Action<Type>) (root => this.tablePerConcreteClassEntities.Add(root)))));
        this.EnlistTypeRegistration(type, (Action<Type>) (t => this.AddAsTablePerConcreteClassEntity(t, true)));
      }
      else
      {
        Type type1 = !this.IsComponent(type) ? this.GetSingleRootEntityOrNull(type) : throw new MappingException(string.Format("Ambiguous mapping of {0}. It was registered as entity and as component", (object) type.FullName));
        if (type1 == null)
          throw new MappingException(string.Format("The root entity for {0} was never registered", (object) type.FullName));
        if (type1.Equals(type))
          throw new MappingException(string.Format("Ambiguous mapping of {0}. It was registered as root-entity and as subclass for table-per-concrete-class strategy", (object) type.FullName));
        if (this.IsMappedFor((ICollection<Type>) this.tablePerClassEntities, type) || this.IsMappedFor((ICollection<Type>) this.tablePerClassHierarchyEntities, type))
          throw new MappingException(string.Format("Ambiguous mapping of {0}. It was registered with more than one class-hierarchy strategy", (object) type.FullName));
      }
    }

    public void AddAsOneToOneRelation(MemberInfo member)
    {
      this.persistentMembers.Add(member);
      this.oneToOneRelations.Add(member);
    }

    public void AddAsManyToOneRelation(MemberInfo member)
    {
      this.persistentMembers.Add(member);
      this.manyToOneRelations.Add(member);
    }

    public void AddAsManyToManyRelation(MemberInfo member)
    {
      this.persistentMembers.Add(member);
      this.manyToManyRelations.Add(member);
    }

    public void AddAsOneToManyRelation(MemberInfo member)
    {
      this.persistentMembers.Add(member);
      this.oneToManyRelations.Add(member);
    }

    public void AddAsManyToAnyRelation(MemberInfo member)
    {
      this.persistentMembers.Add(member);
      this.manyToAnyRelations.Add(member);
    }

    public void AddAsAny(MemberInfo member)
    {
      this.persistentMembers.Add(member);
      this.any.Add(member);
    }

    public void AddAsPoid(MemberInfo member)
    {
      this.persistentMembers.Add(member);
      this.poids.Add(member);
    }

    public void AddAsPartOfComposedId(MemberInfo member)
    {
      this.persistentMembers.Add(member);
      this.composedIds.Add(member);
    }

    public void AddAsVersionProperty(MemberInfo member)
    {
      this.persistentMembers.Add(member);
      this.versionProperties.Add(member);
    }

    public void AddAsNaturalId(MemberInfo member)
    {
      this.persistentMembers.Add(member);
      this.naturalIds.Add(member);
    }

    public void AddAsSet(MemberInfo member)
    {
      this.persistentMembers.Add(member);
      this.sets.Add(member);
    }

    public void AddAsBag(MemberInfo member)
    {
      this.persistentMembers.Add(member);
      this.bags.Add(member);
    }

    public void AddAsIdBag(MemberInfo member)
    {
      this.persistentMembers.Add(member);
      this.idBags.Add(member);
    }

    public void AddAsList(MemberInfo member)
    {
      this.persistentMembers.Add(member);
      this.lists.Add(member);
    }

    public void AddAsArray(MemberInfo member)
    {
      this.persistentMembers.Add(member);
      this.arrays.Add(member);
    }

    public void AddAsMap(MemberInfo member)
    {
      this.persistentMembers.Add(member);
      this.dictionaries.Add(member);
    }

    public void AddAsProperty(MemberInfo member)
    {
      this.persistentMembers.Add(member);
      this.properties.Add(member);
    }

    public void AddAsPersistentMember(MemberInfo member) => this.persistentMembers.Add(member);

    public void AddAsPropertySplit(SplitDefinition definition)
    {
      if (definition == null)
        return;
      Type on = definition.On;
      string groupId = definition.GroupId;
      MemberInfo fromDeclaringType = definition.Member.GetMemberFromDeclaringType();
      if (!this.memberSplitGroup.TryGetValue(fromDeclaringType, out string _))
      {
        this.AddTypeSplits(on, groupId);
        this.memberSplitGroup[fromDeclaringType] = groupId;
      }
      this.splitDefinitions.Add(definition);
    }

    public void AddAsDynamicComponent(MemberInfo member, Type componentTemplate)
    {
      this.persistentMembers.Add(member);
      this.dynamicComponents.Add(member);
      this.dynamicComponentTemplates[member] = componentTemplate;
    }

    private void AddTypeSplits(Type propertyContainer, string splitGroupId)
    {
      HashSet<string> stringSet;
      this.typeSplitGroups.TryGetValue(propertyContainer, out stringSet);
      if (stringSet == null)
      {
        stringSet = new HashSet<string>();
        this.typeSplitGroups[propertyContainer] = stringSet;
      }
      stringSet.Add(splitGroupId);
    }

    public virtual Type GetDynamicComponentTemplate(MemberInfo member)
    {
      Type type;
      this.dynamicComponentTemplates.TryGetValue(member, out type);
      return type ?? typeof (object);
    }

    protected Type GetSingleRootEntityOrNull(Type entityType)
    {
      List<Type> list = this.GetRootEntitiesOf(entityType).ToList<Type>();
      if (list.Count > 1)
      {
        StringBuilder stringBuilder = new StringBuilder(1024);
        stringBuilder.AppendLine(string.Format("Ambiguous mapping for {0}. More than one root entities was found:", (object) entityType.FullName));
        foreach (Type type in list.AsEnumerable<Type>().Reverse<Type>())
          stringBuilder.AppendLine(type.FullName);
        stringBuilder.AppendLine("Possible solutions:");
        stringBuilder.AppendLine("- Merge the mappings of root entity in the one is representing the real root of the hierarchy.");
        stringBuilder.AppendLine("- Inject a IModelInspector with a logic to discover the real root-entity.");
        throw new MappingException(stringBuilder.ToString());
      }
      return list.SingleOrDefault<Type>(new Func<Type, bool>(this.IsRootEntity));
    }

    protected IEnumerable<Type> GetRootEntitiesOf(Type entityType)
    {
      if (entityType != null)
      {
        if (this.IsRootEntity(entityType))
          yield return entityType;
        foreach (Type type in entityType.GetBaseTypes().Where<Type>(new Func<Type, bool>(this.IsRootEntity)))
          yield return type;
      }
    }

    public abstract bool IsRootEntity(Type entityType);

    protected bool IsMappedForTablePerClassEntities(Type type)
    {
      return this.IsMappedFor((ICollection<Type>) this.tablePerClassEntities, type);
    }

    protected bool IsMappedForTablePerClassHierarchyEntities(Type type)
    {
      return this.IsMappedFor((ICollection<Type>) this.tablePerClassHierarchyEntities, type);
    }

    protected bool IsMappedForTablePerConcreteClassEntities(Type type)
    {
      return this.IsMappedFor((ICollection<Type>) this.tablePerConcreteClassEntities, type);
    }

    private bool IsMappedFor(ICollection<Type> explicitMappedEntities, Type type)
    {
      bool flag1 = explicitMappedEntities.Contains(type);
      bool flag2 = false;
      if (!flag1)
      {
        flag2 = type.GetBaseTypes().Any<Type>(new Func<Type, bool>(explicitMappedEntities.Contains));
        if (flag2)
          explicitMappedEntities.Add(type);
      }
      return flag1 || flag2;
    }

    protected void EnlistTypeRegistration(Type type, Action<Type> registration)
    {
      List<Action<Type>> actionList;
      if (!this.delayedEntityRegistrations.TryGetValue(type, out actionList))
      {
        actionList = new List<Action<Type>>();
        this.delayedEntityRegistrations[type] = actionList;
      }
      actionList.Add(registration);
    }

    protected void ExecuteDelayedTypeRegistration(Type type)
    {
      this.ExecuteDelayedRootEntitiesRegistrations();
      List<Action<Type>> actionList;
      if (!this.delayedEntityRegistrations.TryGetValue(type, out actionList))
        return;
      foreach (Action<Type> action in actionList)
        action(type);
    }

    protected void ExecuteDelayedRootEntitiesRegistrations()
    {
      while (this.delayedRootEntityRegistrations.Count > 0)
        this.delayedRootEntityRegistrations.Dequeue()();
    }

    protected bool HasDelayedEntityRegistration(Type type)
    {
      return this.delayedEntityRegistrations.ContainsKey(type);
    }
  }
}

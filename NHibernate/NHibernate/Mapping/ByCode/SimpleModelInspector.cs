// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.SimpleModelInspector
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using Iesi.Collections.Generic;
using NHibernate.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public class SimpleModelInspector : IModelInspector, IModelExplicitDeclarationsHolder
  {
    private readonly SimpleModelInspector.MixinDeclaredModel declaredModel;
    private Func<Type, bool, bool> isEntity = (Func<Type, bool, bool>) ((t, declared) => declared);
    private Func<Type, bool, bool> isRootEntity;
    private Func<Type, bool, bool> isTablePerClass;
    private Func<SplitDefinition, bool, bool> isTablePerClassSplit = (Func<SplitDefinition, bool, bool>) ((sd, declared) => declared);
    private Func<Type, bool, bool> isTablePerClassHierarchy = (Func<Type, bool, bool>) ((t, declared) => declared);
    private Func<Type, bool, bool> isTablePerConcreteClass = (Func<Type, bool, bool>) ((t, declared) => declared);
    private Func<Type, IEnumerable<string>, IEnumerable<string>> splitsForType = (Func<Type, IEnumerable<string>, IEnumerable<string>>) ((t, declared) => declared);
    private Func<Type, bool, bool> isComponent;
    private Func<MemberInfo, bool, bool> isPersistentId;
    private Func<MemberInfo, bool, bool> isPersistentProperty;
    private Func<MemberInfo, bool, bool> isVersion = (Func<MemberInfo, bool, bool>) ((m, declared) => declared);
    private Func<MemberInfo, bool, bool> isProperty = (Func<MemberInfo, bool, bool>) ((m, declared) => declared);
    private Func<MemberInfo, bool, bool> isDynamicComponent = (Func<MemberInfo, bool, bool>) ((m, declared) => declared);
    private Func<MemberInfo, bool, bool> isAny = (Func<MemberInfo, bool, bool>) ((m, declared) => declared);
    private Func<MemberInfo, bool, bool> isManyToMany = (Func<MemberInfo, bool, bool>) ((m, declared) => declared);
    private Func<MemberInfo, bool, bool> isManyToAny = (Func<MemberInfo, bool, bool>) ((m, declared) => declared);
    private Func<MemberInfo, bool, bool> isManyToOne;
    private Func<MemberInfo, bool, bool> isMemberOfNaturalId = (Func<MemberInfo, bool, bool>) ((m, declared) => declared);
    private Func<MemberInfo, bool, bool> isOneToMany;
    private Func<MemberInfo, bool, bool> isOneToOne = (Func<MemberInfo, bool, bool>) ((m, declared) => declared);
    private Func<MemberInfo, bool, bool> isSet;
    private Func<MemberInfo, bool, bool> isArray;
    private Func<MemberInfo, bool, bool> isBag;
    private Func<MemberInfo, bool, bool> isDictionary;
    private Func<MemberInfo, bool, bool> isIdBag = (Func<MemberInfo, bool, bool>) ((m, declared) => declared);
    private Func<MemberInfo, bool, bool> isList = (Func<MemberInfo, bool, bool>) ((m, declared) => declared);

    public SimpleModelInspector()
    {
      this.isEntity = (Func<Type, bool, bool>) ((t, declared) => declared || this.MatchEntity(t));
      this.isRootEntity = (Func<Type, bool, bool>) ((t, declared) => declared || this.MatchRootEntity(t));
      this.isTablePerClass = (Func<Type, bool, bool>) ((t, declared) => declared || this.MatchTablePerClass(t));
      this.isPersistentId = (Func<MemberInfo, bool, bool>) ((m, declared) => declared || this.MatchPoIdPattern(m));
      this.isComponent = (Func<Type, bool, bool>) ((t, declared) => declared || this.MatchComponentPattern(t));
      this.isPersistentProperty = (Func<MemberInfo, bool, bool>) ((m, declared) =>
      {
        if (declared)
          return true;
        return m is PropertyInfo && this.MatchNoReadOnlyPropertyPattern(m);
      });
      this.isSet = (Func<MemberInfo, bool, bool>) ((m, declared) => declared || this.MatchCollection(m, new Predicate<MemberInfo>(this.MatchSetMember)));
      this.isArray = (Func<MemberInfo, bool, bool>) ((m, declared) => declared || this.MatchCollection(m, new Predicate<MemberInfo>(this.MatchArrayMember)));
      this.isBag = (Func<MemberInfo, bool, bool>) ((m, declared) => declared || this.MatchCollection(m, new Predicate<MemberInfo>(this.MatchBagMember)));
      this.isDictionary = (Func<MemberInfo, bool, bool>) ((m, declared) => declared || this.MatchCollection(m, new Predicate<MemberInfo>(this.MatchDictionaryMember)));
      this.isManyToOne = (Func<MemberInfo, bool, bool>) ((m, declared) => declared || this.MatchManyToOne(m));
      this.isOneToMany = (Func<MemberInfo, bool, bool>) ((m, declared) => declared || this.MatchOneToMany(m));
      this.declaredModel = new SimpleModelInspector.MixinDeclaredModel((IModelInspector) this);
    }

    private bool MatchRootEntity(Type type)
    {
      return type.IsClass && typeof (object).Equals(type.BaseType) && ((IModelInspector) this).IsEntity(type);
    }

    private bool MatchTablePerClass(Type type)
    {
      return !this.declaredModel.IsTablePerClassHierarchy(type) && !this.declaredModel.IsTablePerConcreteClass(type);
    }

    private bool MatchOneToMany(MemberInfo memberInfo)
    {
      IModelInspector modelInspector = (IModelInspector) this;
      Type reflectedType = memberInfo.ReflectedType;
      Type dictionaryValueType = memberInfo.GetPropertyOrFieldType().DetermineCollectionElementOrDictionaryValueType();
      if (dictionaryValueType == null)
        return false;
      bool flag1 = modelInspector.IsEntity(reflectedType) && modelInspector.IsEntity(dictionaryValueType);
      bool flag2 = modelInspector.IsComponent(reflectedType) && modelInspector.IsEntity(dictionaryValueType);
      if (this.declaredModel.IsManyToMany(memberInfo))
        return false;
      return flag1 || flag2;
    }

    private bool MatchManyToOne(MemberInfo memberInfo)
    {
      IModelInspector modelInspector = (IModelInspector) this;
      Type reflectedType = memberInfo.ReflectedType;
      Type propertyOrFieldType = memberInfo.GetPropertyOrFieldType();
      bool flag = modelInspector.IsEntity(reflectedType) && modelInspector.IsEntity(propertyOrFieldType);
      if (modelInspector.IsComponent(reflectedType) && modelInspector.IsEntity(propertyOrFieldType))
        return true;
      return flag && !modelInspector.IsOneToOne(memberInfo);
    }

    protected bool MatchArrayMember(MemberInfo subject)
    {
      Type propertyOrFieldType = subject.GetPropertyOrFieldType();
      return propertyOrFieldType.IsArray && propertyOrFieldType.GetElementType() != typeof (byte);
    }

    protected bool MatchDictionaryMember(MemberInfo subject)
    {
      Type propertyOrFieldType = subject.GetPropertyOrFieldType();
      if (typeof (IDictionary).IsAssignableFrom(propertyOrFieldType))
        return true;
      return propertyOrFieldType.IsGenericType && propertyOrFieldType.GetGenericInterfaceTypeDefinitions().Contains<Type>(typeof (IDictionary<,>));
    }

    protected bool MatchBagMember(MemberInfo subject)
    {
      Type propertyOrFieldType = subject.GetPropertyOrFieldType();
      return typeof (IEnumerable).IsAssignableFrom(propertyOrFieldType) && propertyOrFieldType != typeof (string) && propertyOrFieldType != typeof (byte[]);
    }

    protected bool MatchCollection(
      MemberInfo subject,
      Predicate<MemberInfo> specificCollectionPredicate)
    {
      if (specificCollectionPredicate(subject))
        return true;
      PropertyInfo pi = subject as PropertyInfo;
      if (pi != null)
      {
        FieldInfo fieldInfo = PropertyToField.DefaultStrategies.Values.Select(ps => new
        {
          ps = ps,
          fi = subject.DeclaringType.GetField(ps.GetFieldName(pi.Name), BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        }).Where(_param0 => _param0.fi != null).Select(_param0 => _param0.fi).FirstOrDefault<FieldInfo>();
        if (fieldInfo != null)
          return specificCollectionPredicate((MemberInfo) fieldInfo);
      }
      return false;
    }

    protected bool MatchSetMember(MemberInfo subject)
    {
      Type propertyOrFieldType = subject.GetPropertyOrFieldType();
      if (typeof (ISet).IsAssignableFrom(propertyOrFieldType))
        return true;
      return propertyOrFieldType.IsGenericType && propertyOrFieldType.GetGenericInterfaceTypeDefinitions().Contains<Type>(typeof (ISet<>));
    }

    protected bool MatchNoReadOnlyPropertyPattern(MemberInfo subject)
    {
      return !this.IsReadOnlyProperty(subject);
    }

    protected bool IsReadOnlyProperty(MemberInfo subject)
    {
      PropertyInfo property = subject as PropertyInfo;
      if (property == null || !this.CanReadCantWriteInsideType(property) && !this.CanReadCantWriteInBaseType(property))
        return false;
      return !PropertyToField.DefaultStrategies.Values.Any<IFieldNamingStrategy>((Func<IFieldNamingStrategy, bool>) (s => subject.DeclaringType.GetField(s.GetFieldName(property.Name), BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) != null)) || this.IsAutoproperty(property);
    }

    protected bool IsAutoproperty(PropertyInfo property)
    {
      return ((IEnumerable<FieldInfo>) property.ReflectedType.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Any<FieldInfo>((Func<FieldInfo, bool>) (pi => pi.Name == "<" + property.Name + ">k__BackingField"));
    }

    protected bool CanReadCantWriteInsideType(PropertyInfo property)
    {
      return !property.CanWrite && property.CanRead && property.DeclaringType == property.ReflectedType;
    }

    protected bool CanReadCantWriteInBaseType(PropertyInfo property)
    {
      if (property.DeclaringType == property.ReflectedType)
        return false;
      PropertyInfo propertyInfo = ((IEnumerable<PropertyInfo>) property.DeclaringType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).SingleOrDefault<PropertyInfo>((Func<PropertyInfo, bool>) (pi => pi.Name == property.Name));
      return propertyInfo != null && !propertyInfo.CanWrite && propertyInfo.CanRead;
    }

    protected bool MatchPoIdPattern(MemberInfo subject)
    {
      string name = subject.Name;
      if (name.Equals("id", StringComparison.InvariantCultureIgnoreCase) || name.Equals("poid", StringComparison.InvariantCultureIgnoreCase) || name.Equals("oid", StringComparison.InvariantCultureIgnoreCase))
        return true;
      return name.StartsWith(subject.DeclaringType.Name) && name.Equals(subject.DeclaringType.Name + "id", StringComparison.InvariantCultureIgnoreCase);
    }

    protected bool MatchComponentPattern(Type subject)
    {
      if (this.declaredModel.IsEntity(subject))
        return false;
      IModelInspector modelInspector = (IModelInspector) this;
      return !subject.IsEnum && (subject.Namespace == null || !subject.Namespace.StartsWith("System")) && !modelInspector.IsEntity(subject) && !subject.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).Cast<MemberInfo>().Concat<MemberInfo>((IEnumerable<MemberInfo>) subject.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)).Any<MemberInfo>((Func<MemberInfo, bool>) (m => modelInspector.IsPersistentId(m)));
    }

    protected bool MatchEntity(Type subject)
    {
      if (this.declaredModel.Components.Contains<Type>(subject))
        return false;
      IModelInspector modelInspector = (IModelInspector) this;
      return subject.IsClass && subject.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).Cast<MemberInfo>().Concat<MemberInfo>((IEnumerable<MemberInfo>) subject.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)).Any<MemberInfo>((Func<MemberInfo, bool>) (m => modelInspector.IsPersistentId(m)));
    }

    IEnumerable<Type> IModelExplicitDeclarationsHolder.RootEntities
    {
      get => this.declaredModel.RootEntities;
    }

    IEnumerable<Type> IModelExplicitDeclarationsHolder.Components => this.declaredModel.Components;

    IEnumerable<Type> IModelExplicitDeclarationsHolder.TablePerClassEntities
    {
      get => this.declaredModel.TablePerClassEntities;
    }

    IEnumerable<Type> IModelExplicitDeclarationsHolder.TablePerClassHierarchyEntities
    {
      get => this.declaredModel.TablePerClassHierarchyEntities;
    }

    IEnumerable<Type> IModelExplicitDeclarationsHolder.TablePerConcreteClassEntities
    {
      get => this.declaredModel.TablePerConcreteClassEntities;
    }

    IEnumerable<MemberInfo> IModelExplicitDeclarationsHolder.OneToOneRelations
    {
      get => this.declaredModel.OneToOneRelations;
    }

    IEnumerable<MemberInfo> IModelExplicitDeclarationsHolder.ManyToOneRelations
    {
      get => this.declaredModel.ManyToManyRelations;
    }

    IEnumerable<MemberInfo> IModelExplicitDeclarationsHolder.ManyToManyRelations
    {
      get => this.declaredModel.ManyToManyRelations;
    }

    IEnumerable<MemberInfo> IModelExplicitDeclarationsHolder.OneToManyRelations
    {
      get => this.declaredModel.OneToManyRelations;
    }

    IEnumerable<MemberInfo> IModelExplicitDeclarationsHolder.ManyToAnyRelations
    {
      get => this.declaredModel.ManyToAnyRelations;
    }

    IEnumerable<MemberInfo> IModelExplicitDeclarationsHolder.Any => this.declaredModel.Any;

    IEnumerable<MemberInfo> IModelExplicitDeclarationsHolder.Poids => this.declaredModel.Poids;

    IEnumerable<MemberInfo> IModelExplicitDeclarationsHolder.VersionProperties
    {
      get => this.declaredModel.VersionProperties;
    }

    IEnumerable<MemberInfo> IModelExplicitDeclarationsHolder.NaturalIds
    {
      get => this.declaredModel.NaturalIds;
    }

    IEnumerable<MemberInfo> IModelExplicitDeclarationsHolder.Sets => this.declaredModel.Sets;

    IEnumerable<MemberInfo> IModelExplicitDeclarationsHolder.Bags => this.declaredModel.Bags;

    IEnumerable<MemberInfo> IModelExplicitDeclarationsHolder.IdBags => this.declaredModel.IdBags;

    IEnumerable<MemberInfo> IModelExplicitDeclarationsHolder.Lists => this.declaredModel.Lists;

    IEnumerable<MemberInfo> IModelExplicitDeclarationsHolder.Arrays => this.declaredModel.Arrays;

    IEnumerable<MemberInfo> IModelExplicitDeclarationsHolder.Dictionaries
    {
      get => this.declaredModel.Dictionaries;
    }

    IEnumerable<MemberInfo> IModelExplicitDeclarationsHolder.Properties
    {
      get => this.declaredModel.Properties;
    }

    IEnumerable<MemberInfo> IModelExplicitDeclarationsHolder.PersistentMembers
    {
      get => this.declaredModel.PersistentMembers;
    }

    IEnumerable<SplitDefinition> IModelExplicitDeclarationsHolder.SplitDefinitions
    {
      get => this.declaredModel.SplitDefinitions;
    }

    IEnumerable<MemberInfo> IModelExplicitDeclarationsHolder.DynamicComponents
    {
      get => this.declaredModel.DynamicComponents;
    }

    IEnumerable<string> IModelExplicitDeclarationsHolder.GetSplitGroupsFor(Type type)
    {
      return this.declaredModel.GetSplitGroupsFor(type);
    }

    string IModelExplicitDeclarationsHolder.GetSplitGroupFor(MemberInfo member)
    {
      return this.declaredModel.GetSplitGroupFor(member);
    }

    void IModelExplicitDeclarationsHolder.AddAsRootEntity(Type type)
    {
      this.declaredModel.AddAsRootEntity(type);
    }

    void IModelExplicitDeclarationsHolder.AddAsComponent(Type type)
    {
      this.declaredModel.AddAsComponent(type);
    }

    void IModelExplicitDeclarationsHolder.AddAsTablePerClassEntity(Type type)
    {
      this.declaredModel.AddAsTablePerClassEntity(type);
    }

    void IModelExplicitDeclarationsHolder.AddAsTablePerClassHierarchyEntity(Type type)
    {
      this.declaredModel.AddAsTablePerClassHierarchyEntity(type);
    }

    void IModelExplicitDeclarationsHolder.AddAsTablePerConcreteClassEntity(Type type)
    {
      this.declaredModel.AddAsTablePerConcreteClassEntity(type);
    }

    void IModelExplicitDeclarationsHolder.AddAsOneToOneRelation(MemberInfo member)
    {
      this.declaredModel.AddAsOneToOneRelation(member);
    }

    void IModelExplicitDeclarationsHolder.AddAsManyToOneRelation(MemberInfo member)
    {
      this.declaredModel.AddAsManyToOneRelation(member);
    }

    void IModelExplicitDeclarationsHolder.AddAsManyToManyRelation(MemberInfo member)
    {
      this.declaredModel.AddAsManyToManyRelation(member);
    }

    void IModelExplicitDeclarationsHolder.AddAsOneToManyRelation(MemberInfo member)
    {
      this.declaredModel.AddAsOneToManyRelation(member);
    }

    void IModelExplicitDeclarationsHolder.AddAsManyToAnyRelation(MemberInfo member)
    {
      this.declaredModel.AddAsManyToAnyRelation(member);
    }

    void IModelExplicitDeclarationsHolder.AddAsAny(MemberInfo member)
    {
      this.declaredModel.AddAsAny(member);
    }

    void IModelExplicitDeclarationsHolder.AddAsPoid(MemberInfo member)
    {
      this.declaredModel.AddAsPoid(member);
    }

    void IModelExplicitDeclarationsHolder.AddAsVersionProperty(MemberInfo member)
    {
      this.declaredModel.AddAsVersionProperty(member);
    }

    void IModelExplicitDeclarationsHolder.AddAsNaturalId(MemberInfo member)
    {
      this.declaredModel.AddAsNaturalId(member);
    }

    void IModelExplicitDeclarationsHolder.AddAsSet(MemberInfo member)
    {
      this.declaredModel.AddAsSet(member);
    }

    void IModelExplicitDeclarationsHolder.AddAsBag(MemberInfo member)
    {
      this.declaredModel.AddAsBag(member);
    }

    void IModelExplicitDeclarationsHolder.AddAsIdBag(MemberInfo member)
    {
      this.declaredModel.AddAsIdBag(member);
    }

    void IModelExplicitDeclarationsHolder.AddAsList(MemberInfo member)
    {
      this.declaredModel.AddAsList(member);
    }

    void IModelExplicitDeclarationsHolder.AddAsArray(MemberInfo member)
    {
      this.declaredModel.AddAsArray(member);
    }

    void IModelExplicitDeclarationsHolder.AddAsMap(MemberInfo member)
    {
      this.declaredModel.AddAsMap(member);
    }

    void IModelExplicitDeclarationsHolder.AddAsProperty(MemberInfo member)
    {
      this.declaredModel.AddAsProperty(member);
    }

    void IModelExplicitDeclarationsHolder.AddAsPersistentMember(MemberInfo member)
    {
      this.declaredModel.AddAsPersistentMember(member);
    }

    void IModelExplicitDeclarationsHolder.AddAsPropertySplit(SplitDefinition definition)
    {
      this.declaredModel.AddAsPropertySplit(definition);
    }

    void IModelExplicitDeclarationsHolder.AddAsDynamicComponent(
      MemberInfo member,
      Type componentTemplate)
    {
      this.declaredModel.AddAsDynamicComponent(member, componentTemplate);
    }

    IEnumerable<MemberInfo> IModelExplicitDeclarationsHolder.ComposedIds
    {
      get => this.declaredModel.ComposedIds;
    }

    void IModelExplicitDeclarationsHolder.AddAsPartOfComposedId(MemberInfo member)
    {
      this.declaredModel.AddAsPartOfComposedId(member);
    }

    bool IModelInspector.IsRootEntity(Type type)
    {
      bool flag = this.declaredModel.RootEntities.Contains<Type>(type);
      return this.isRootEntity(type, flag);
    }

    bool IModelInspector.IsComponent(Type type)
    {
      bool flag = this.declaredModel.Components.Contains<Type>(type);
      return this.isComponent(type, flag);
    }

    bool IModelInspector.IsEntity(Type type)
    {
      bool flag = this.declaredModel.IsEntity(type);
      return this.isEntity(type, flag);
    }

    bool IModelInspector.IsTablePerClass(Type type)
    {
      bool flag = this.declaredModel.IsTablePerClass(type);
      return this.isTablePerClass(type, flag);
    }

    bool IModelInspector.IsTablePerClassSplit(Type type, object splitGroupId, MemberInfo member)
    {
      bool flag = this.declaredModel.IsTablePerClassSplit(type, splitGroupId, member);
      return this.isTablePerClassSplit(new SplitDefinition(type, splitGroupId.ToString(), member), flag);
    }

    bool IModelInspector.IsTablePerClassHierarchy(Type type)
    {
      bool flag = this.declaredModel.IsTablePerClassHierarchy(type);
      return this.isTablePerClassHierarchy(type, flag);
    }

    bool IModelInspector.IsTablePerConcreteClass(Type type)
    {
      bool flag = this.declaredModel.IsTablePerConcreteClass(type);
      return this.isTablePerConcreteClass(type, flag);
    }

    bool IModelInspector.IsOneToOne(MemberInfo member)
    {
      bool flag = this.DeclaredPolymorphicMatch(member, (Func<MemberInfo, bool>) (m => this.declaredModel.IsOneToOne(m)));
      return this.isOneToOne(member, flag);
    }

    bool IModelInspector.IsManyToOne(MemberInfo member)
    {
      bool flag = this.DeclaredPolymorphicMatch(member, (Func<MemberInfo, bool>) (m => this.declaredModel.IsManyToOne(m)));
      return this.isManyToOne(member, flag);
    }

    bool IModelInspector.IsManyToMany(MemberInfo member)
    {
      bool flag = this.DeclaredPolymorphicMatch(member, (Func<MemberInfo, bool>) (m => this.declaredModel.IsManyToMany(m)));
      return this.isManyToMany(member, flag);
    }

    bool IModelInspector.IsOneToMany(MemberInfo member)
    {
      bool flag = this.DeclaredPolymorphicMatch(member, (Func<MemberInfo, bool>) (m => this.declaredModel.IsOneToMany(m)));
      return this.isOneToMany(member, flag);
    }

    bool IModelInspector.IsManyToAny(MemberInfo member)
    {
      bool flag = this.DeclaredPolymorphicMatch(member, (Func<MemberInfo, bool>) (m => this.declaredModel.IsManyToAny(m)));
      return this.isManyToAny(member, flag);
    }

    bool IModelInspector.IsAny(MemberInfo member)
    {
      bool flag = this.DeclaredPolymorphicMatch(member, (Func<MemberInfo, bool>) (m => this.declaredModel.IsAny(m)));
      return this.isAny(member, flag);
    }

    bool IModelInspector.IsPersistentId(MemberInfo member)
    {
      bool flag = this.DeclaredPolymorphicMatch(member, (Func<MemberInfo, bool>) (m => this.declaredModel.IsPersistentId(m)));
      return this.isPersistentId(member, flag);
    }

    bool IModelInspector.IsMemberOfComposedId(MemberInfo member)
    {
      return this.declaredModel.IsMemberOfComposedId(member);
    }

    bool IModelInspector.IsVersion(MemberInfo member)
    {
      bool flag = this.DeclaredPolymorphicMatch(member, (Func<MemberInfo, bool>) (m => this.declaredModel.IsVersion(m)));
      return this.isVersion(member, flag);
    }

    bool IModelInspector.IsMemberOfNaturalId(MemberInfo member)
    {
      bool flag = this.DeclaredPolymorphicMatch(member, (Func<MemberInfo, bool>) (m => this.declaredModel.IsMemberOfNaturalId(m)));
      return this.isMemberOfNaturalId(member, flag);
    }

    bool IModelInspector.IsPersistentProperty(MemberInfo member)
    {
      bool flag = this.DeclaredPolymorphicMatch(member, (Func<MemberInfo, bool>) (m => this.declaredModel.IsPersistentProperty(m)));
      return this.isPersistentProperty(member, flag);
    }

    bool IModelInspector.IsSet(MemberInfo role)
    {
      bool flag = this.DeclaredPolymorphicMatch(role, (Func<MemberInfo, bool>) (m => this.declaredModel.IsSet(m)));
      return this.isSet(role, flag);
    }

    bool IModelInspector.IsBag(MemberInfo role)
    {
      bool flag = this.DeclaredPolymorphicMatch(role, (Func<MemberInfo, bool>) (m => this.declaredModel.IsBag(m)));
      return this.isBag(role, flag);
    }

    bool IModelInspector.IsIdBag(MemberInfo role)
    {
      bool flag = this.DeclaredPolymorphicMatch(role, (Func<MemberInfo, bool>) (m => this.declaredModel.IsIdBag(m)));
      return this.isIdBag(role, flag);
    }

    bool IModelInspector.IsList(MemberInfo role)
    {
      bool flag = this.DeclaredPolymorphicMatch(role, (Func<MemberInfo, bool>) (m => this.declaredModel.IsList(m)));
      return this.isList(role, flag);
    }

    bool IModelInspector.IsArray(MemberInfo role)
    {
      bool flag = this.DeclaredPolymorphicMatch(role, (Func<MemberInfo, bool>) (m => this.declaredModel.IsArray(m)));
      return this.isArray(role, flag);
    }

    bool IModelInspector.IsDictionary(MemberInfo role)
    {
      bool flag = this.DeclaredPolymorphicMatch(role, (Func<MemberInfo, bool>) (m => this.declaredModel.IsDictionary(m)));
      return this.isDictionary(role, flag);
    }

    bool IModelInspector.IsProperty(MemberInfo member)
    {
      bool flag = this.DeclaredPolymorphicMatch(member, (Func<MemberInfo, bool>) (m => this.declaredModel.IsProperty(m)));
      return this.isProperty(member, flag);
    }

    bool IModelInspector.IsDynamicComponent(MemberInfo member)
    {
      bool flag = this.DeclaredPolymorphicMatch(member, (Func<MemberInfo, bool>) (m => this.declaredModel.IsDynamicComponent(m)));
      return this.isDynamicComponent(member, flag);
    }

    Type IModelInspector.GetDynamicComponentTemplate(MemberInfo member)
    {
      return this.declaredModel.GetDynamicComponentTemplate(member);
    }

    Type IModelExplicitDeclarationsHolder.GetDynamicComponentTemplate(MemberInfo member)
    {
      return this.declaredModel.GetDynamicComponentTemplate(member);
    }

    IEnumerable<string> IModelInspector.GetPropertiesSplits(Type type)
    {
      IEnumerable<string> propertiesSplits = this.declaredModel.GetPropertiesSplits(type);
      return this.splitsForType(type, propertiesSplits);
    }

    protected virtual bool DeclaredPolymorphicMatch(
      MemberInfo member,
      Func<MemberInfo, bool> declaredMatch)
    {
      return declaredMatch(member) || member.GetMemberFromDeclaringClasses().Any<MemberInfo>(declaredMatch) || member.GetPropertyFromInterfaces().Any<MemberInfo>(declaredMatch);
    }

    public void IsRootEntity(Func<Type, bool, bool> match)
    {
      if (match == null)
        return;
      this.isRootEntity = match;
    }

    public void IsComponent(Func<Type, bool, bool> match)
    {
      if (match == null)
        return;
      this.isComponent = match;
    }

    public void IsEntity(Func<Type, bool, bool> match)
    {
      if (match == null)
        return;
      this.isEntity = match;
    }

    public void IsTablePerClass(Func<Type, bool, bool> match)
    {
      if (match == null)
        return;
      this.isTablePerClass = match;
    }

    public void IsTablePerClassHierarchy(Func<Type, bool, bool> match)
    {
      if (match == null)
        return;
      this.isTablePerClassHierarchy = match;
    }

    public void IsTablePerConcreteClass(Func<Type, bool, bool> match)
    {
      if (match == null)
        return;
      this.isTablePerConcreteClass = match;
    }

    public void IsOneToOne(Func<MemberInfo, bool, bool> match)
    {
      if (match == null)
        return;
      this.isOneToOne = match;
    }

    public void IsManyToOne(Func<MemberInfo, bool, bool> match)
    {
      if (match == null)
        return;
      this.isManyToOne = match;
    }

    public void IsManyToMany(Func<MemberInfo, bool, bool> match)
    {
      if (match == null)
        return;
      this.isManyToMany = match;
    }

    public void IsOneToMany(Func<MemberInfo, bool, bool> match)
    {
      if (match == null)
        return;
      this.isOneToMany = match;
    }

    public void IsManyToAny(Func<MemberInfo, bool, bool> match)
    {
      if (match == null)
        return;
      this.isManyToAny = match;
    }

    public void IsAny(Func<MemberInfo, bool, bool> match)
    {
      if (match == null)
        return;
      this.isAny = match;
    }

    public void IsPersistentId(Func<MemberInfo, bool, bool> match)
    {
      if (match == null)
        return;
      this.isPersistentId = match;
    }

    public void IsVersion(Func<MemberInfo, bool, bool> match)
    {
      if (match == null)
        return;
      this.isVersion = match;
    }

    public void IsMemberOfNaturalId(Func<MemberInfo, bool, bool> match)
    {
      if (match == null)
        return;
      this.isMemberOfNaturalId = match;
    }

    public void IsPersistentProperty(Func<MemberInfo, bool, bool> match)
    {
      if (match == null)
        return;
      this.isPersistentProperty = match;
    }

    public void IsSet(Func<MemberInfo, bool, bool> match)
    {
      if (match == null)
        return;
      this.isSet = match;
    }

    public void IsBag(Func<MemberInfo, bool, bool> match)
    {
      if (match == null)
        return;
      this.isBag = match;
    }

    public void IsIdBag(Func<MemberInfo, bool, bool> match)
    {
      if (match == null)
        return;
      this.isIdBag = match;
    }

    public void IsList(Func<MemberInfo, bool, bool> match)
    {
      if (match == null)
        return;
      this.isList = match;
    }

    public void IsArray(Func<MemberInfo, bool, bool> match)
    {
      if (match == null)
        return;
      this.isArray = match;
    }

    public void IsDictionary(Func<MemberInfo, bool, bool> match)
    {
      if (match == null)
        return;
      this.isDictionary = match;
    }

    public void IsProperty(Func<MemberInfo, bool, bool> match)
    {
      if (match == null)
        return;
      this.isProperty = match;
    }

    public void SplitsFor(
      Func<Type, IEnumerable<string>, IEnumerable<string>> getPropertiesSplitsId)
    {
      if (getPropertiesSplitsId == null)
        return;
      this.splitsForType = getPropertiesSplitsId;
    }

    public void IsTablePerClassSplit(Func<SplitDefinition, bool, bool> match)
    {
      if (match == null)
        return;
      this.isTablePerClassSplit = match;
    }

    public void IsDynamicComponent(Func<MemberInfo, bool, bool> match)
    {
      if (match == null)
        return;
      this.isDynamicComponent = match;
    }

    private class MixinDeclaredModel : AbstractExplicitlyDeclaredModel
    {
      private readonly IModelInspector inspector;

      public MixinDeclaredModel(IModelInspector inspector) => this.inspector = inspector;

      public override bool IsComponent(Type type) => this.Components.Contains<Type>(type);

      public override bool IsRootEntity(Type entityType) => this.inspector.IsRootEntity(entityType);

      public bool IsEntity(Type type)
      {
        return this.RootEntities.Contains<Type>(type) || type.GetBaseTypes().Any<Type>((Func<Type, bool>) (t => this.RootEntities.Contains<Type>(t))) || this.HasDelayedEntityRegistration(type);
      }

      public bool IsTablePerClass(Type type)
      {
        this.ExecuteDelayedTypeRegistration(type);
        return this.IsMappedForTablePerClassEntities(type);
      }

      public bool IsTablePerClassSplit(Type type, object splitGroupId, MemberInfo member)
      {
        return object.Equals(splitGroupId, (object) this.GetSplitGroupFor(member));
      }

      public bool IsTablePerClassHierarchy(Type type)
      {
        this.ExecuteDelayedTypeRegistration(type);
        return this.IsMappedForTablePerClassHierarchyEntities(type);
      }

      public bool IsTablePerConcreteClass(Type type)
      {
        this.ExecuteDelayedTypeRegistration(type);
        return this.IsMappedForTablePerConcreteClassEntities(type);
      }

      public bool IsOneToOne(MemberInfo member)
      {
        return this.OneToOneRelations.Contains<MemberInfo>(member);
      }

      public bool IsManyToOne(MemberInfo member)
      {
        return this.ManyToOneRelations.Contains<MemberInfo>(member);
      }

      public bool IsManyToMany(MemberInfo member)
      {
        return this.ManyToManyRelations.Contains<MemberInfo>(member);
      }

      public bool IsOneToMany(MemberInfo member)
      {
        return this.OneToManyRelations.Contains<MemberInfo>(member);
      }

      public bool IsManyToAny(MemberInfo member)
      {
        return this.ManyToAnyRelations.Contains<MemberInfo>(member);
      }

      public bool IsAny(MemberInfo member) => this.Any.Contains<MemberInfo>(member);

      public bool IsPersistentId(MemberInfo member) => this.Poids.Contains<MemberInfo>(member);

      public bool IsMemberOfComposedId(MemberInfo member)
      {
        return this.ComposedIds.Contains<MemberInfo>(member);
      }

      public bool IsVersion(MemberInfo member)
      {
        return this.VersionProperties.Contains<MemberInfo>(member);
      }

      public bool IsMemberOfNaturalId(MemberInfo member)
      {
        return this.NaturalIds.Contains<MemberInfo>(member);
      }

      public bool IsPersistentProperty(MemberInfo member)
      {
        return this.PersistentMembers.Contains<MemberInfo>(member);
      }

      public bool IsSet(MemberInfo role) => this.Sets.Contains<MemberInfo>(role);

      public bool IsBag(MemberInfo role) => this.Bags.Contains<MemberInfo>(role);

      public bool IsIdBag(MemberInfo role) => this.IdBags.Contains<MemberInfo>(role);

      public bool IsList(MemberInfo role) => this.Lists.Contains<MemberInfo>(role);

      public bool IsArray(MemberInfo role) => this.Arrays.Contains<MemberInfo>(role);

      public bool IsDictionary(MemberInfo role) => this.Dictionaries.Contains<MemberInfo>(role);

      public bool IsProperty(MemberInfo member) => this.Properties.Contains<MemberInfo>(member);

      public bool IsDynamicComponent(MemberInfo member)
      {
        return this.DynamicComponents.Contains<MemberInfo>(member);
      }

      public IEnumerable<string> GetPropertiesSplits(Type type) => this.GetSplitGroupsFor(type);
    }
  }
}

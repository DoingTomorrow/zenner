// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.ConventionModelMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Mapping.ByCode.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public class ConventionModelMapper : ModelMapper
  {
    public ConventionModelMapper()
      : base((IModelInspector) new SimpleModelInspector())
    {
      this.AppendDefaultEvents();
    }

    protected virtual void AppendDefaultEvents()
    {
      this.BeforeMapClass += new RootClassMappingHandler(this.NoPoidGuid);
      this.BeforeMapClass += new RootClassMappingHandler(this.NoSetterPoidToField);
      this.BeforeMapProperty += new PropertyMappingHandler(this.MemberToFieldAccessor);
      this.BeforeMapProperty += new PropertyMappingHandler(this.MemberNoSetterToField);
      this.BeforeMapProperty += new PropertyMappingHandler(this.MemberReadOnlyAccessor);
      this.BeforeMapComponent += new ComponentMappingHandler(this.MemberToFieldAccessor);
      this.BeforeMapComponent += new ComponentMappingHandler(this.MemberNoSetterToField);
      this.BeforeMapComponent += new ComponentMappingHandler(this.MemberReadOnlyAccessor);
      this.BeforeMapComponent += new ComponentMappingHandler(this.ComponentParentToFieldAccessor);
      this.BeforeMapComponent += new ComponentMappingHandler(this.ComponentParentNoSetterToField);
      this.BeforeMapBag += new BagMappingHandler(this.MemberToFieldAccessor);
      this.BeforeMapIdBag += new IdBagMappingHandler(this.MemberToFieldAccessor);
      this.BeforeMapSet += new SetMappingHandler(this.MemberToFieldAccessor);
      this.BeforeMapMap += new MapMappingHandler(this.MemberToFieldAccessor);
      this.BeforeMapList += new ListMappingHandler(this.MemberToFieldAccessor);
      this.BeforeMapBag += new BagMappingHandler(this.MemberNoSetterToField);
      this.BeforeMapIdBag += new IdBagMappingHandler(this.MemberNoSetterToField);
      this.BeforeMapSet += new SetMappingHandler(this.MemberNoSetterToField);
      this.BeforeMapMap += new MapMappingHandler(this.MemberNoSetterToField);
      this.BeforeMapList += new ListMappingHandler(this.MemberNoSetterToField);
      this.BeforeMapBag += new BagMappingHandler(this.MemberReadOnlyAccessor);
      this.BeforeMapIdBag += new IdBagMappingHandler(this.MemberReadOnlyAccessor);
      this.BeforeMapSet += new SetMappingHandler(this.MemberReadOnlyAccessor);
      this.BeforeMapMap += new MapMappingHandler(this.MemberReadOnlyAccessor);
      this.BeforeMapList += new ListMappingHandler(this.MemberReadOnlyAccessor);
      this.BeforeMapManyToOne += new ManyToOneMappingHandler(this.MemberToFieldAccessor);
      this.BeforeMapOneToOne += new OneToOneMappingHandler(this.MemberToFieldAccessor);
      this.BeforeMapAny += new AnyMappingHandler(this.MemberToFieldAccessor);
      this.BeforeMapManyToOne += new ManyToOneMappingHandler(this.MemberNoSetterToField);
      this.BeforeMapOneToOne += new OneToOneMappingHandler(this.MemberNoSetterToField);
      this.BeforeMapAny += new AnyMappingHandler(this.MemberNoSetterToField);
      this.BeforeMapManyToOne += new ManyToOneMappingHandler(this.MemberReadOnlyAccessor);
      this.BeforeMapOneToOne += new OneToOneMappingHandler(this.MemberReadOnlyAccessor);
      this.BeforeMapAny += new AnyMappingHandler(this.MemberReadOnlyAccessor);
    }

    protected virtual void ComponentParentToFieldAccessor(
      IModelInspector modelInspector,
      PropertyPath member,
      IComponentAttributesMapper componentMapper)
    {
      MemberInfo referenceProperty = this.GetComponentParentReferenceProperty(this.MembersProvider.GetComponentMembers(member.LocalMember.GetPropertyOrFieldType()).Where<MemberInfo>((Func<MemberInfo, bool>) (p => this.ModelInspector.IsPersistentProperty(p))), member.LocalMember.ReflectedType);
      if (referenceProperty == null || !this.MatchPropertyToField(referenceProperty))
        return;
      componentMapper.Parent(referenceProperty, (Action<IComponentParentMapper>) (cp => cp.Access(Accessor.Field)));
    }

    protected virtual void ComponentParentNoSetterToField(
      IModelInspector modelInspector,
      PropertyPath member,
      IComponentAttributesMapper componentMapper)
    {
      MemberInfo referenceProperty = this.GetComponentParentReferenceProperty(this.MembersProvider.GetComponentMembers(member.LocalMember.GetPropertyOrFieldType()).Where<MemberInfo>((Func<MemberInfo, bool>) (p => this.ModelInspector.IsPersistentProperty(p))), member.LocalMember.ReflectedType);
      if (referenceProperty == null || !this.MatchNoSetterProperty(referenceProperty))
        return;
      componentMapper.Parent(referenceProperty, (Action<IComponentParentMapper>) (cp => cp.Access(Accessor.NoSetter)));
    }

    protected virtual void MemberReadOnlyAccessor(
      IModelInspector modelInspector,
      PropertyPath member,
      IAccessorPropertyMapper propertyCustomizer)
    {
      if (!this.MatchReadOnlyProperty(member.LocalMember))
        return;
      propertyCustomizer.Access(Accessor.ReadOnly);
    }

    protected bool MatchReadOnlyProperty(MemberInfo subject)
    {
      return subject is PropertyInfo propertyInfo && (this.CanReadCantWriteInsideType(propertyInfo) || this.CanReadCantWriteInBaseType(propertyInfo)) && PropertyToField.GetBackFieldInfo(propertyInfo) == null;
    }

    private bool CanReadCantWriteInsideType(PropertyInfo property)
    {
      return !property.CanWrite && property.CanRead && property.DeclaringType == property.ReflectedType;
    }

    private bool CanReadCantWriteInBaseType(PropertyInfo property)
    {
      if (property.DeclaringType == property.ReflectedType)
        return false;
      PropertyInfo propertyInfo = ((IEnumerable<PropertyInfo>) property.DeclaringType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).SingleOrDefault<PropertyInfo>((Func<PropertyInfo, bool>) (pi => pi.Name == property.Name));
      return propertyInfo != null && !propertyInfo.CanWrite && propertyInfo.CanRead;
    }

    protected virtual void MemberNoSetterToField(
      IModelInspector modelInspector,
      PropertyPath member,
      IAccessorPropertyMapper propertyCustomizer)
    {
      if (!this.MatchNoSetterProperty(member.LocalMember))
        return;
      propertyCustomizer.Access(Accessor.NoSetter);
    }

    protected virtual void MemberToFieldAccessor(
      IModelInspector modelInspector,
      PropertyPath member,
      IAccessorPropertyMapper propertyCustomizer)
    {
      if (!this.MatchPropertyToField(member.LocalMember))
        return;
      propertyCustomizer.Access(Accessor.Field);
    }

    protected bool MatchPropertyToField(MemberInfo subject)
    {
      if (!(subject is PropertyInfo subject1))
        return false;
      FieldInfo backFieldInfo = PropertyToField.GetBackFieldInfo(subject1);
      return backFieldInfo != null && backFieldInfo.FieldType != subject1.PropertyType;
    }

    protected virtual void NoSetterPoidToField(
      IModelInspector modelInspector,
      Type type,
      IClassAttributesMapper classCustomizer)
    {
      if (!this.MatchNoSetterProperty(this.MembersProvider.GetEntityMembersForPoid(type).FirstOrDefault<MemberInfo>(new Func<MemberInfo, bool>(modelInspector.IsPersistentId))))
        return;
      classCustomizer.Id((Action<IIdMapper>) (idm => idm.Access(Accessor.NoSetter)));
    }

    protected bool MatchNoSetterProperty(MemberInfo subject)
    {
      if (!(subject is PropertyInfo subject1) || subject1.CanWrite || !subject1.CanRead)
        return false;
      FieldInfo backFieldInfo = PropertyToField.GetBackFieldInfo(subject1);
      return backFieldInfo != null && backFieldInfo.FieldType == subject1.PropertyType;
    }

    protected virtual void NoPoidGuid(
      IModelInspector modelInspector,
      Type type,
      IClassAttributesMapper classCustomizer)
    {
      if (!object.ReferenceEquals((object) null, (object) this.MembersProvider.GetEntityMembersForPoid(type).FirstOrDefault<MemberInfo>((Func<MemberInfo, bool>) (mi => modelInspector.IsPersistentId(mi)))))
        return;
      classCustomizer.Id((MemberInfo) null, (Action<IIdMapper>) (idm => idm.Generator(Generators.Guid)));
    }

    protected SimpleModelInspector SimpleModelInspector
    {
      get => (SimpleModelInspector) this.ModelInspector;
    }

    public void IsRootEntity(Func<Type, bool, bool> match)
    {
      this.SimpleModelInspector.IsRootEntity(match);
    }

    public void IsComponent(Func<Type, bool, bool> match)
    {
      this.SimpleModelInspector.IsComponent(match);
    }

    public void IsEntity(Func<Type, bool, bool> match) => this.SimpleModelInspector.IsEntity(match);

    public void IsTablePerClass(Func<Type, bool, bool> match)
    {
      this.SimpleModelInspector.IsTablePerClass(match);
    }

    public void IsTablePerClassHierarchy(Func<Type, bool, bool> match)
    {
      this.SimpleModelInspector.IsTablePerClassHierarchy(match);
    }

    public void IsTablePerConcreteClass(Func<Type, bool, bool> match)
    {
      this.SimpleModelInspector.IsTablePerConcreteClass(match);
    }

    public void IsOneToOne(Func<MemberInfo, bool, bool> match)
    {
      this.SimpleModelInspector.IsOneToOne(match);
    }

    public void IsManyToOne(Func<MemberInfo, bool, bool> match)
    {
      this.SimpleModelInspector.IsManyToOne(match);
    }

    public void IsManyToMany(Func<MemberInfo, bool, bool> match)
    {
      this.SimpleModelInspector.IsManyToMany(match);
    }

    public void IsOneToMany(Func<MemberInfo, bool, bool> match)
    {
      this.SimpleModelInspector.IsOneToMany(match);
    }

    public void IsAny(Func<MemberInfo, bool, bool> match) => this.SimpleModelInspector.IsAny(match);

    public void IsPersistentId(Func<MemberInfo, bool, bool> match)
    {
      this.SimpleModelInspector.IsPersistentId(match);
    }

    public void IsVersion(Func<MemberInfo, bool, bool> match)
    {
      this.SimpleModelInspector.IsVersion(match);
    }

    public void IsMemberOfNaturalId(Func<MemberInfo, bool, bool> match)
    {
      this.SimpleModelInspector.IsMemberOfNaturalId(match);
    }

    public void IsPersistentProperty(Func<MemberInfo, bool, bool> match)
    {
      this.SimpleModelInspector.IsPersistentProperty(match);
    }

    public void IsSet(Func<MemberInfo, bool, bool> match) => this.SimpleModelInspector.IsSet(match);

    public void IsBag(Func<MemberInfo, bool, bool> match) => this.SimpleModelInspector.IsBag(match);

    public void IsIdBag(Func<MemberInfo, bool, bool> match)
    {
      this.SimpleModelInspector.IsIdBag(match);
    }

    public void IsList(Func<MemberInfo, bool, bool> match)
    {
      this.SimpleModelInspector.IsList(match);
    }

    public void IsArray(Func<MemberInfo, bool, bool> match)
    {
      this.SimpleModelInspector.IsArray(match);
    }

    public void IsDictionary(Func<MemberInfo, bool, bool> match)
    {
      this.SimpleModelInspector.IsDictionary(match);
    }

    public void IsProperty(Func<MemberInfo, bool, bool> match)
    {
      this.SimpleModelInspector.IsProperty(match);
    }

    public void SplitsFor(
      Func<Type, IEnumerable<string>, IEnumerable<string>> getPropertiesSplitsId)
    {
      this.SimpleModelInspector.SplitsFor(getPropertiesSplitsId);
    }

    public void IsTablePerClassSplit(Func<SplitDefinition, bool, bool> match)
    {
      this.SimpleModelInspector.IsTablePerClassSplit(match);
    }
  }
}

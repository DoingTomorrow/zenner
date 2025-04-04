// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.ExplicitlyDeclaredModel
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
  public class ExplicitlyDeclaredModel : AbstractExplicitlyDeclaredModel, IModelInspector
  {
    public override bool IsRootEntity(Type type) => this.RootEntities.Contains<Type>(type);

    public override bool IsComponent(Type type) => this.Components.Contains<Type>(type);

    public virtual bool IsEntity(Type type)
    {
      return this.RootEntities.Contains<Type>(type) || type.GetBaseTypes().Any<Type>((Func<Type, bool>) (t => this.RootEntities.Contains<Type>(t))) || this.HasDelayedEntityRegistration(type);
    }

    public virtual bool IsTablePerClass(Type type)
    {
      this.ExecuteDelayedTypeRegistration(type);
      return this.IsMappedForTablePerClassEntities(type);
    }

    public virtual bool IsTablePerClassSplit(Type type, object splitGroupId, MemberInfo member)
    {
      return object.Equals(splitGroupId, (object) this.GetSplitGroupFor(member));
    }

    public virtual bool IsTablePerClassHierarchy(Type type)
    {
      this.ExecuteDelayedTypeRegistration(type);
      return this.IsMappedForTablePerClassHierarchyEntities(type);
    }

    public virtual bool IsTablePerConcreteClass(Type type)
    {
      this.ExecuteDelayedTypeRegistration(type);
      return this.IsMappedForTablePerConcreteClassEntities(type);
    }

    public virtual bool IsOneToOne(MemberInfo member)
    {
      return this.OneToOneRelations.Contains<MemberInfo>(member);
    }

    public virtual bool IsManyToOne(MemberInfo member)
    {
      return this.ManyToOneRelations.Contains<MemberInfo>(member);
    }

    public virtual bool IsManyToMany(MemberInfo member)
    {
      return this.ManyToManyRelations.Contains<MemberInfo>(member);
    }

    public virtual bool IsOneToMany(MemberInfo member)
    {
      return this.OneToManyRelations.Contains<MemberInfo>(member);
    }

    public bool IsManyToAny(MemberInfo member)
    {
      return this.ManyToAnyRelations.Contains<MemberInfo>(member);
    }

    public virtual bool IsAny(MemberInfo member) => this.Any.Contains<MemberInfo>(member);

    public virtual bool IsPersistentId(MemberInfo member)
    {
      return this.Poids.Contains<MemberInfo>(member);
    }

    public bool IsMemberOfComposedId(MemberInfo member)
    {
      return this.ComposedIds.Contains<MemberInfo>(member);
    }

    public virtual bool IsVersion(MemberInfo member)
    {
      return this.VersionProperties.Contains<MemberInfo>(member);
    }

    public virtual bool IsMemberOfNaturalId(MemberInfo member)
    {
      return this.NaturalIds.Contains<MemberInfo>(member);
    }

    public virtual bool IsPersistentProperty(MemberInfo member)
    {
      return this.PersistentMembers.Contains<MemberInfo>(member);
    }

    public virtual bool IsSet(MemberInfo role) => this.Sets.Contains<MemberInfo>(role);

    public virtual bool IsBag(MemberInfo role) => this.Bags.Contains<MemberInfo>(role);

    public virtual bool IsIdBag(MemberInfo role) => this.IdBags.Contains<MemberInfo>(role);

    public virtual bool IsList(MemberInfo role) => this.Lists.Contains<MemberInfo>(role);

    public virtual bool IsArray(MemberInfo role) => this.Arrays.Contains<MemberInfo>(role);

    public virtual bool IsDictionary(MemberInfo role)
    {
      return this.Dictionaries.Contains<MemberInfo>(role);
    }

    public virtual bool IsProperty(MemberInfo member)
    {
      return this.Properties.Contains<MemberInfo>(member);
    }

    public virtual bool IsDynamicComponent(MemberInfo member)
    {
      return this.DynamicComponents.Contains<MemberInfo>(member);
    }

    public virtual IEnumerable<string> GetPropertiesSplits(Type type)
    {
      return this.GetSplitGroupsFor(type);
    }
  }
}

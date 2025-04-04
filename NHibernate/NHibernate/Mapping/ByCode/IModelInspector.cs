// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.IModelInspector
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface IModelInspector
  {
    bool IsRootEntity(Type type);

    bool IsComponent(Type type);

    bool IsEntity(Type type);

    bool IsTablePerClass(Type type);

    bool IsTablePerClassSplit(Type type, object splitGroupId, MemberInfo member);

    bool IsTablePerClassHierarchy(Type type);

    bool IsTablePerConcreteClass(Type type);

    bool IsOneToOne(MemberInfo member);

    bool IsManyToOne(MemberInfo member);

    bool IsManyToMany(MemberInfo member);

    bool IsOneToMany(MemberInfo member);

    bool IsManyToAny(MemberInfo member);

    bool IsAny(MemberInfo member);

    bool IsPersistentId(MemberInfo member);

    bool IsMemberOfComposedId(MemberInfo member);

    bool IsVersion(MemberInfo member);

    bool IsMemberOfNaturalId(MemberInfo member);

    bool IsPersistentProperty(MemberInfo member);

    bool IsSet(MemberInfo role);

    bool IsBag(MemberInfo role);

    bool IsIdBag(MemberInfo role);

    bool IsList(MemberInfo role);

    bool IsArray(MemberInfo role);

    bool IsDictionary(MemberInfo role);

    bool IsProperty(MemberInfo member);

    bool IsDynamicComponent(MemberInfo member);

    Type GetDynamicComponentTemplate(MemberInfo member);

    IEnumerable<string> GetPropertiesSplits(Type type);
  }
}

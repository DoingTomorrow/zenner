// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.DefaultCandidatePersistentMembersProvider
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
  public class DefaultCandidatePersistentMembersProvider : ICandidatePersistentMembersProvider
  {
    internal const BindingFlags SubClassPropertiesBindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    internal const BindingFlags RootClassPropertiesBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
    internal const BindingFlags ComponentPropertiesBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
    internal const BindingFlags ClassFieldsBindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    public IEnumerable<MemberInfo> GetEntityMembersForPoid(Type entityClass)
    {
      return !entityClass.IsInterface ? entityClass.GetPropertiesOfHierarchy().Concat<MemberInfo>(this.GetFieldsOfHierarchy(entityClass)) : entityClass.GetInterfaceProperties();
    }

    public IEnumerable<MemberInfo> GetRootEntityMembers(Type entityClass)
    {
      return this.GetCandidatePersistentProperties(entityClass, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).Concat<MemberInfo>(this.GetUserDeclaredFields(entityClass).Cast<MemberInfo>());
    }

    public IEnumerable<MemberInfo> GetSubEntityMembers(Type entityClass, Type entitySuperclass)
    {
      return !entitySuperclass.Equals(entityClass.BaseType) ? this.GetCandidatePersistentProperties(entityClass, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).Except<MemberInfo>(this.GetCandidatePersistentProperties(entitySuperclass, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy), (IEqualityComparer<MemberInfo>) new DefaultCandidatePersistentMembersProvider.PropertyNameEqualityComparer()).Concat<MemberInfo>(this.GetUserDeclaredFields(entityClass).Cast<MemberInfo>()) : this.GetCandidatePersistentProperties(entityClass, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Concat<MemberInfo>(this.GetUserDeclaredFields(entityClass).Cast<MemberInfo>());
    }

    protected IEnumerable<FieldInfo> GetUserDeclaredFields(Type type)
    {
      return ((IEnumerable<FieldInfo>) type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Where<FieldInfo>((Func<FieldInfo, bool>) (x => !x.Name.StartsWith("<")));
    }

    public IEnumerable<MemberInfo> GetComponentMembers(Type componentClass)
    {
      return this.GetCandidatePersistentProperties(componentClass, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).Concat<MemberInfo>(this.GetUserDeclaredFields(componentClass).Cast<MemberInfo>());
    }

    private IEnumerable<MemberInfo> GetFieldsOfHierarchy(Type type)
    {
      for (Type analizing = type; analizing != null && analizing != typeof (object); analizing = analizing.BaseType)
      {
        foreach (FieldInfo fieldInfo in this.GetUserDeclaredFields(analizing))
          yield return (MemberInfo) fieldInfo;
      }
    }

    private IEnumerable<MemberInfo> GetCandidatePersistentProperties(
      Type type,
      BindingFlags propertiesBindingFlags)
    {
      return !type.IsInterface ? (IEnumerable<MemberInfo>) type.GetProperties(propertiesBindingFlags) : type.GetInterfaceProperties();
    }

    private class PropertyNameEqualityComparer : IEqualityComparer<MemberInfo>
    {
      public bool Equals(MemberInfo x, MemberInfo y) => x.Name == y.Name;

      public int GetHashCode(MemberInfo obj) => obj.Name.GetHashCode();
    }
  }
}

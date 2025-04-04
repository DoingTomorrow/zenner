// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.ICandidatePersistentMembersProvider
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public interface ICandidatePersistentMembersProvider
  {
    IEnumerable<MemberInfo> GetEntityMembersForPoid(Type entityClass);

    IEnumerable<MemberInfo> GetRootEntityMembers(Type entityClass);

    IEnumerable<MemberInfo> GetSubEntityMembers(Type entityClass, Type entitySuperclass);

    IEnumerable<MemberInfo> GetComponentMembers(Type componentClass);
  }
}

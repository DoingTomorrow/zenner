// Decompiled with JetBrains decompiler
// Type: Ninject.Infrastructure.Language.ExtensionsForICustomAttributeProvider
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using System;
using System.Reflection;

#nullable disable
namespace Ninject.Infrastructure.Language
{
  internal static class ExtensionsForICustomAttributeProvider
  {
    public static bool HasAttribute(this ICustomAttributeProvider member, Type type)
    {
      MemberInfo member1 = member as MemberInfo;
      return member1 != (MemberInfo) null ? ExtensionsForMemberInfo.HasAttribute(member1, type) : member.IsDefined(type, true);
    }

    public static object[] GetCustomAttributesExtended(
      this ICustomAttributeProvider member,
      Type attributeType,
      bool inherit)
    {
      MemberInfo member1 = member as MemberInfo;
      return member1 != (MemberInfo) null ? ExtensionsForMemberInfo.GetCustomAttributesExtended(member1, attributeType, inherit) : member.GetCustomAttributes(attributeType, inherit);
    }
  }
}

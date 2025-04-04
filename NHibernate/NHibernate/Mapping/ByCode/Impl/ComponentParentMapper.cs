// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.Impl.ComponentParentMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using System;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode.Impl
{
  public class ComponentParentMapper : IComponentParentMapper, IAccessorPropertyMapper
  {
    private readonly AccessorPropertyMapper accessorPropertyMapper;

    public ComponentParentMapper(HbmParent parent, MemberInfo member)
    {
      parent.name = member != null ? member.Name : throw new ArgumentNullException(nameof (member));
      this.accessorPropertyMapper = new AccessorPropertyMapper(member.DeclaringType, member.Name, (Action<string>) (x => parent.access = x));
    }

    public void Access(Accessor accessor) => this.accessorPropertyMapper.Access(accessor);

    public void Access(Type accessorType) => this.accessorPropertyMapper.Access(accessorType);
  }
}

// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.IComponentElementMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface IComponentElementMapper : 
    IComponentAttributesMapper,
    IEntityPropertyMapper,
    IAccessorPropertyMapper
  {
    void Property(MemberInfo property, Action<IPropertyMapper> mapping);

    void Component(MemberInfo property, Action<IComponentElementMapper> mapping);

    void ManyToOne(MemberInfo property, Action<IManyToOneMapper> mapping);
  }
}

// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.IOneToOneMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface IOneToOneMapper : IEntityPropertyMapper, IAccessorPropertyMapper
  {
    void Cascade(NHibernate.Mapping.ByCode.Cascade cascadeStyle);

    void Lazy(LazyRelation lazyRelation);

    void Constrained(bool value);

    void PropertyReference(MemberInfo propertyInTheOtherSide);

    void Formula(string formula);

    void ForeignKey(string foreignKeyName);
  }
}

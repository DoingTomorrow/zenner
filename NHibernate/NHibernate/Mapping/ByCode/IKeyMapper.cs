// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.IKeyMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface IKeyMapper : IColumnsMapper
  {
    void OnDelete(OnDeleteAction deleteAction);

    void PropertyRef(MemberInfo property);

    void Update(bool consideredInUpdateQuery);

    void NotNullable(bool notnull);

    void Unique(bool unique);

    void ForeignKey(string foreignKeyName);
  }
}

// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.IMapKeyMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface IMapKeyMapper : IColumnsMapper
  {
    void Type(IType persistentType);

    void Type<TPersistentType>();

    void Type(System.Type persistentType);

    void Length(int length);

    void Formula(string formula);
  }
}

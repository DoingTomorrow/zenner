// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.IPropertyMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface IPropertyMapper : IEntityPropertyMapper, IAccessorPropertyMapper, IColumnsMapper
  {
    void Type(IType persistentType);

    void Type<TPersistentType>();

    void Type<TPersistentType>(object parameters);

    void Type(System.Type persistentType, object parameters);

    void Length(int length);

    void Precision(short precision);

    void Scale(short scale);

    void NotNullable(bool notnull);

    void Unique(bool unique);

    void UniqueKey(string uniquekeyName);

    void Index(string indexName);

    void Formula(string formula);

    void Update(bool consideredInUpdateQuery);

    void Insert(bool consideredInInsertQuery);

    void Lazy(bool isLazy);

    void Generated(PropertyGeneration generation);
  }
}

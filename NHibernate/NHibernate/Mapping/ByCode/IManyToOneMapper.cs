// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.IManyToOneMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface IManyToOneMapper : IEntityPropertyMapper, IAccessorPropertyMapper, IColumnsMapper
  {
    void Class(Type entityType);

    void Cascade(NHibernate.Mapping.ByCode.Cascade cascadeStyle);

    void NotNullable(bool notnull);

    void Unique(bool unique);

    void UniqueKey(string uniquekeyName);

    void Index(string indexName);

    void Fetch(FetchKind fetchMode);

    void Formula(string formula);

    void Lazy(LazyRelation lazyRelation);

    void Update(bool consideredInUpdateQuery);

    void Insert(bool consideredInInsertQuery);

    void ForeignKey(string foreignKeyName);

    void PropertyRef(string propertyReferencedName);

    void NotFound(NotFoundMode mode);
  }
}

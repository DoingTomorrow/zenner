// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.IClassAttributesMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Reflection;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface IClassAttributesMapper : IEntityAttributesMapper, IEntitySqlsMapper
  {
    void Id(Action<IIdMapper> idMapper);

    void Id(MemberInfo idProperty, Action<IIdMapper> idMapper);

    void ComponentAsId(MemberInfo idProperty, Action<IComponentAsIdMapper> idMapper);

    void ComposedId(Action<IComposedIdMapper> idPropertiesMapping);

    void Discriminator(Action<IDiscriminatorMapper> discriminatorMapping);

    void DiscriminatorValue(object value);

    void Table(string tableName);

    void Catalog(string catalogName);

    void Schema(string schemaName);

    void Mutable(bool isMutable);

    void Version(MemberInfo versionProperty, Action<IVersionMapper> versionMapping);

    void NaturalId(Action<INaturalIdMapper> naturalIdMapping);

    void Cache(Action<ICacheMapper> cacheMapping);

    void Filter(string filterName, Action<IFilterMapper> filterMapping);

    void Where(string whereClause);

    void SchemaAction(NHibernate.Mapping.ByCode.SchemaAction action);
  }
}

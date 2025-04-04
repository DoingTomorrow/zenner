// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.IClassAttributesMapper`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface IClassAttributesMapper<TEntity> : IEntityAttributesMapper, IEntitySqlsMapper where TEntity : class
  {
    void Id<TProperty>(Expression<Func<TEntity, TProperty>> idProperty);

    void Id<TProperty>(Expression<Func<TEntity, TProperty>> idProperty, Action<IIdMapper> idMapper);

    void Id(string notVisiblePropertyOrFieldName, Action<IIdMapper> idMapper);

    void ComponentAsId<TComponent>(Expression<Func<TEntity, TComponent>> idProperty) where TComponent : class;

    void ComponentAsId<TComponent>(
      Expression<Func<TEntity, TComponent>> idProperty,
      Action<IComponentAsIdMapper<TComponent>> idMapper)
      where TComponent : class;

    void ComponentAsId<TComponent>(string notVisiblePropertyOrFieldName) where TComponent : class;

    void ComponentAsId<TComponent>(
      string notVisiblePropertyOrFieldName,
      Action<IComponentAsIdMapper<TComponent>> idMapper)
      where TComponent : class;

    void ComposedId(
      Action<IComposedIdMapper<TEntity>> idPropertiesMapping);

    void Discriminator(Action<IDiscriminatorMapper> discriminatorMapping);

    void DiscriminatorValue(object value);

    void Table(string tableName);

    void Catalog(string catalogName);

    void Schema(string schemaName);

    void Mutable(bool isMutable);

    void Version<TProperty>(
      Expression<Func<TEntity, TProperty>> versionProperty,
      Action<IVersionMapper> versionMapping);

    void Version(string notVisiblePropertyOrFieldName, Action<IVersionMapper> versionMapping);

    void NaturalId(
      Action<IBasePlainPropertyContainerMapper<TEntity>> naturalIdPropertiesMapping,
      Action<INaturalIdAttributesMapper> naturalIdMapping);

    void NaturalId(
      Action<IBasePlainPropertyContainerMapper<TEntity>> naturalIdPropertiesMapping);

    void Cache(Action<ICacheMapper> cacheMapping);

    void Filter(string filterName, Action<IFilterMapper> filterMapping);

    void Where(string whereClause);

    void SchemaAction(NHibernate.Mapping.ByCode.SchemaAction action);
  }
}

// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.RootClassBinder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Cfg.XmlHbmBinding
{
  public class RootClassBinder(Mappings mappings, NHibernate.Dialect.Dialect dialect) : ClassBinder(mappings, dialect)
  {
    public void Bind(HbmClass classSchema, IDictionary<string, MetaAttribute> inheritedMetas)
    {
      RootClass rootClass = new RootClass();
      this.BindClass((IEntityMapping) classSchema, (PersistentClass) rootClass, inheritedMetas);
      rootClass.OptimisticLockMode = classSchema.optimisticlock.ToOptimisticLock();
      inheritedMetas = Binder.GetMetas((IDecoratable) classSchema, inheritedMetas, true);
      string schema = classSchema.schema ?? this.mappings.SchemaName;
      string catalog = classSchema.catalog ?? this.mappings.CatalogName;
      string classTableName = this.GetClassTableName((PersistentClass) rootClass, classSchema);
      if (string.IsNullOrEmpty(classTableName))
        throw new MappingException(string.Format("Could not determine the name of the table for entity '{0}'; remove the 'table' attribute or assign a value to it.", (object) rootClass.EntityName));
      Table table = this.mappings.AddTable(schema, catalog, classTableName, classSchema.Subselect, rootClass.IsAbstract.GetValueOrDefault(), classSchema.schemaaction);
      ((ITableOwner) rootClass).Table = table;
      Binder.log.InfoFormat("Mapping class: {0} -> {1}", (object) rootClass.EntityName, (object) rootClass.Table.Name);
      rootClass.IsMutable = classSchema.mutable;
      rootClass.Where = classSchema.where ?? rootClass.Where;
      if (classSchema.check != null)
        table.AddCheckConstraint(classSchema.check);
      rootClass.IsExplicitPolymorphism = classSchema.polymorphism == HbmPolymorphismType.Explicit;
      RootClassBinder.BindCache(classSchema.cache, rootClass);
      new ClassIdBinder((ClassBinder) this).BindId(classSchema.Id, (PersistentClass) rootClass, table);
      new ClassCompositeIdBinder((ClassBinder) this).BindCompositeId(classSchema.CompositeId, (PersistentClass) rootClass);
      new ClassDiscriminatorBinder((PersistentClass) rootClass, this.Mappings).BindDiscriminator(classSchema.discriminator, table);
      this.BindTimestamp(classSchema.Timestamp, (PersistentClass) rootClass, table, inheritedMetas);
      this.BindVersion(classSchema.Version, (PersistentClass) rootClass, table, inheritedMetas);
      rootClass.CreatePrimaryKey(this.dialect);
      this.BindNaturalId(classSchema.naturalid, (PersistentClass) rootClass, inheritedMetas);
      new PropertiesBinder(this.mappings, (PersistentClass) rootClass, this.dialect).Bind(classSchema.Properties, inheritedMetas);
      this.BindJoins(classSchema.Joins, (PersistentClass) rootClass, inheritedMetas);
      this.BindSubclasses(classSchema.Subclasses, (PersistentClass) rootClass, inheritedMetas);
      this.BindJoinedSubclasses(classSchema.JoinedSubclasses, (PersistentClass) rootClass, inheritedMetas);
      this.BindUnionSubclasses(classSchema.UnionSubclasses, (PersistentClass) rootClass, inheritedMetas);
      new FiltersBinder((IFilterable) rootClass, this.Mappings).Bind((IEnumerable<HbmFilter>) classSchema.filter);
      this.mappings.AddClass((PersistentClass) rootClass);
    }

    private void BindNaturalId(
      HbmNaturalId naturalid,
      PersistentClass rootClass,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      if (naturalid == null)
        return;
      PropertiesBinder propertiesBinder = new PropertiesBinder(this.mappings, rootClass, this.dialect);
      UniqueKey uniqueKey = new UniqueKey();
      uniqueKey.Name = "_UniqueKey";
      uniqueKey.Table = rootClass.Table;
      UniqueKey uk = uniqueKey;
      propertiesBinder.Bind(naturalid.Properties, inheritedMetas, (Action<Property>) (property =>
      {
        if (!naturalid.mutable)
          property.IsUpdateable = false;
        property.IsNaturalIdentifier = true;
        uk.AddColumns(property.ColumnIterator.OfType<Column>());
      }));
      rootClass.Table.AddUniqueKey(uk);
    }

    private string GetClassTableName(PersistentClass model, HbmClass classSchema)
    {
      return classSchema.table == null ? this.mappings.NamingStrategy.ClassToTableName(model.EntityName) : this.mappings.NamingStrategy.TableName(classSchema.table.Trim());
    }

    private void BindTimestamp(
      HbmTimestamp timestampSchema,
      PersistentClass rootClass,
      Table table,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      if (timestampSchema == null)
        return;
      string propertyName = timestampSchema.name;
      SimpleValue propertyValue = new SimpleValue(table);
      new ColumnsBinder(propertyValue, this.Mappings).Bind(timestampSchema.Columns, false, (Func<HbmColumn>) (() => new HbmColumn()
      {
        name = this.mappings.NamingStrategy.PropertyToColumnName(propertyName)
      }));
      if (!propertyValue.IsTypeSpecified)
      {
        switch (timestampSchema.source)
        {
          case HbmTimestampSource.Vm:
            propertyValue.TypeName = NHibernateUtil.Timestamp.Name;
            break;
          case HbmTimestampSource.Db:
            propertyValue.TypeName = NHibernateUtil.DbTimestamp.Name;
            break;
          default:
            propertyValue.TypeName = NHibernateUtil.Timestamp.Name;
            break;
        }
      }
      Property property = new Property((IValue) propertyValue);
      this.BindProperty(timestampSchema, property, inheritedMetas);
      if (property.Generation == PropertyGeneration.Insert)
        throw new MappingException("'generated' attribute cannot be 'insert' for versioning property");
      propertyValue.NullValue = timestampSchema.unsavedvalue == HbmTimestampUnsavedvalue.Null ? (string) null : "undefined";
      rootClass.Version = property;
      rootClass.AddProperty(property);
    }

    private void BindProperty(
      HbmTimestamp timestampSchema,
      Property property,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      property.Name = timestampSchema.name;
      if (property.Value.Type == null)
        throw new MappingException("could not determine a property type for: " + property.Name);
      property.PropertyAccessorName = timestampSchema.access ?? this.mappings.DefaultAccess;
      property.Cascade = this.mappings.DefaultCascade;
      property.IsUpdateable = true;
      property.IsInsertable = true;
      property.IsOptimisticLocked = true;
      property.Generation = RootClassBinder.Convert(timestampSchema.generated);
      if (property.Generation == PropertyGeneration.Always || property.Generation == PropertyGeneration.Insert)
      {
        if (property.IsInsertable)
          property.IsInsertable = false;
        if (property.IsUpdateable && property.Generation == PropertyGeneration.Always)
          property.IsUpdateable = false;
      }
      property.MetaAttributes = Binder.GetMetas((IDecoratable) timestampSchema, inheritedMetas);
      property.LogMapped(Binder.log);
    }

    private static PropertyGeneration Convert(HbmVersionGeneration versionGeneration)
    {
      switch (versionGeneration)
      {
        case HbmVersionGeneration.Never:
          return PropertyGeneration.Never;
        case HbmVersionGeneration.Always:
          return PropertyGeneration.Always;
        default:
          throw new ArgumentOutOfRangeException(nameof (versionGeneration));
      }
    }

    private void BindVersion(
      HbmVersion versionSchema,
      PersistentClass rootClass,
      Table table,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      if (versionSchema == null)
        return;
      string propertyName = versionSchema.name;
      SimpleValue propertyValue = new SimpleValue(table);
      new TypeBinder(propertyValue, this.Mappings).Bind(versionSchema.type);
      new ColumnsBinder(propertyValue, this.Mappings).Bind(versionSchema.Columns, false, (Func<HbmColumn>) (() => new HbmColumn()
      {
        name = this.mappings.NamingStrategy.PropertyToColumnName(propertyName)
      }));
      if (!propertyValue.IsTypeSpecified)
        propertyValue.TypeName = NHibernateUtil.Int32.Name;
      Property property = new Property((IValue) propertyValue);
      this.BindProperty(versionSchema, property, inheritedMetas);
      if (property.Generation == PropertyGeneration.Insert)
        throw new MappingException("'generated' attribute cannot be 'insert' for versioning property");
      propertyValue.NullValue = versionSchema.unsavedvalue;
      rootClass.Version = property;
      rootClass.AddProperty(property);
    }

    private void BindProperty(
      HbmVersion versionSchema,
      Property property,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      property.Name = versionSchema.name;
      if (property.Value.Type == null)
        throw new MappingException("could not determine a property type for: " + property.Name);
      property.PropertyAccessorName = versionSchema.access ?? this.mappings.DefaultAccess;
      property.Cascade = this.mappings.DefaultCascade;
      property.IsUpdateable = true;
      property.IsInsertable = true;
      property.IsOptimisticLocked = true;
      property.Generation = RootClassBinder.Convert(versionSchema.generated);
      if (property.Generation == PropertyGeneration.Always || property.Generation == PropertyGeneration.Insert)
      {
        if (property.IsInsertable)
          property.IsInsertable = false;
        if (property.IsUpdateable && property.Generation == PropertyGeneration.Always)
          property.IsUpdateable = false;
      }
      property.MetaAttributes = Binder.GetMetas((IDecoratable) versionSchema, inheritedMetas);
      property.LogMapped(Binder.log);
    }

    private static void BindCache(HbmCache cacheSchema, RootClass rootClass)
    {
      if (cacheSchema == null)
        return;
      rootClass.CacheConcurrencyStrategy = cacheSchema.usage.ToCacheConcurrencyStrategy();
      rootClass.CacheRegionName = cacheSchema.region;
    }
  }
}

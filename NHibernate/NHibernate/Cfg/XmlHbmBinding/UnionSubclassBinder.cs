// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.UnionSubclassBinder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping;
using NHibernate.Persister.Entity;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cfg.XmlHbmBinding
{
  public class UnionSubclassBinder : ClassBinder
  {
    public UnionSubclassBinder(Mappings mappings, NHibernate.Dialect.Dialect dialect)
      : base(mappings, dialect)
    {
    }

    public UnionSubclassBinder(ClassBinder parent)
      : base(parent)
    {
    }

    public void Bind(
      HbmUnionSubclass unionSubclassMapping,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      this.HandleUnionSubclass(this.GetSuperclass(unionSubclassMapping.extends), unionSubclassMapping, inheritedMetas);
    }

    public void HandleUnionSubclass(
      PersistentClass model,
      HbmUnionSubclass unionSubclassMapping,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      UnionSubclass model1 = new UnionSubclass(model);
      this.BindClass((IEntityMapping) unionSubclassMapping, (PersistentClass) model1, inheritedMetas);
      inheritedMetas = Binder.GetMetas((IDecoratable) unionSubclassMapping, inheritedMetas, true);
      if (model1.EntityPersisterClass == null)
        model1.RootClazz.EntityPersisterClass = typeof (UnionSubclassEntityPersister);
      string schema = unionSubclassMapping.schema ?? this.mappings.SchemaName;
      string catalog = unionSubclassMapping.catalog ?? this.mappings.CatalogName;
      Table table1 = model1.Superclass.Table;
      Table table2 = this.mappings.AddDenormalizedTable(schema, catalog, this.GetClassTableName((PersistentClass) model1, unionSubclassMapping.table), model1.IsAbstract.GetValueOrDefault(), unionSubclassMapping.Subselect, table1);
      ((ITableOwner) model1).Table = table2;
      Binder.log.InfoFormat("Mapping union-subclass: {0} -> {1}", (object) model1.EntityName, (object) model1.Table.Name);
      new PropertiesBinder(this.mappings, (PersistentClass) model1, this.dialect).Bind(unionSubclassMapping.Properties, inheritedMetas);
      this.BindUnionSubclasses(unionSubclassMapping.UnionSubclasses, (PersistentClass) model1, inheritedMetas);
      model.AddSubclass((Subclass) model1);
      this.mappings.AddClass((PersistentClass) model1);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.JoinedSubclassBinder
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
  public class JoinedSubclassBinder : ClassBinder
  {
    public JoinedSubclassBinder(Mappings mappings, NHibernate.Dialect.Dialect dialect)
      : base(mappings, dialect)
    {
    }

    public JoinedSubclassBinder(ClassBinder parent)
      : base(parent)
    {
    }

    public void Bind(
      HbmJoinedSubclass joinedSubclassMapping,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      this.HandleJoinedSubclass(this.GetSuperclass(joinedSubclassMapping.extends), joinedSubclassMapping, inheritedMetas);
    }

    public void HandleJoinedSubclass(
      PersistentClass model,
      HbmJoinedSubclass joinedSubclassMapping,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      JoinedSubclass joinedSubclass = new JoinedSubclass(model);
      this.BindClass((IEntityMapping) joinedSubclassMapping, (PersistentClass) joinedSubclass, inheritedMetas);
      inheritedMetas = Binder.GetMetas((IDecoratable) joinedSubclassMapping, inheritedMetas, true);
      if (joinedSubclass.EntityPersisterClass == null)
        joinedSubclass.RootClazz.EntityPersisterClass = typeof (JoinedSubclassEntityPersister);
      Table mytable = this.mappings.AddTable(joinedSubclassMapping.schema ?? this.mappings.SchemaName, joinedSubclassMapping.catalog ?? this.mappings.CatalogName, this.GetClassTableName((PersistentClass) joinedSubclass, joinedSubclassMapping.table), joinedSubclassMapping.Subselect, false, joinedSubclassMapping.schemaaction);
      ((ITableOwner) joinedSubclass).Table = mytable;
      Binder.log.InfoFormat("Mapping joined-subclass: {0} -> {1}", (object) joinedSubclass.EntityName, (object) joinedSubclass.Table.Name);
      this.BindKey(joinedSubclass, joinedSubclassMapping.key, mytable);
      joinedSubclass.CreatePrimaryKey(this.dialect);
      if (!joinedSubclass.IsJoinedSubclass)
        throw new MappingException("Cannot map joined-subclass " + joinedSubclass.EntityName + " to table " + joinedSubclass.Table.Name + ", the same table as its base class.");
      joinedSubclass.CreateForeignKey();
      mytable.AddCheckConstraint(joinedSubclassMapping.check);
      new PropertiesBinder(this.mappings, (PersistentClass) joinedSubclass, this.dialect).Bind(joinedSubclassMapping.Properties, inheritedMetas);
      this.BindJoinedSubclasses(joinedSubclassMapping.JoinedSubclasses, (PersistentClass) joinedSubclass, inheritedMetas);
      model.AddSubclass((Subclass) joinedSubclass);
      this.mappings.AddClass((PersistentClass) joinedSubclass);
    }

    private void BindKey(JoinedSubclass subclass, HbmKey keyMapping, Table mytable)
    {
      SimpleValue simpleValue = (SimpleValue) new DependantValue(mytable, subclass.Identifier);
      subclass.Key = (IKeyValue) simpleValue;
      simpleValue.IsCascadeDeleteEnabled = keyMapping.ondelete == HbmOndelete.Cascade;
      simpleValue.ForeignKeyName = keyMapping.foreignkey;
      new ValuePropertyBinder(simpleValue, this.Mappings).BindSimpleValue(keyMapping, subclass.EntityName, false);
    }
  }
}

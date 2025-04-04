// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.SubclassBinder
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
  public class SubclassBinder : ClassBinder
  {
    public SubclassBinder(Binder parent, NHibernate.Dialect.Dialect dialect)
      : base(parent.Mappings, dialect)
    {
    }

    public SubclassBinder(ClassBinder parent)
      : base(parent)
    {
    }

    public void Bind(HbmSubclass subClassMapping, IDictionary<string, MetaAttribute> inheritedMetas)
    {
      this.HandleSubclass(this.GetSuperclass(subClassMapping.extends), subClassMapping, inheritedMetas);
    }

    public void HandleSubclass(
      PersistentClass model,
      HbmSubclass subClassMapping,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      Subclass subclass = (Subclass) new SingleTableSubclass(model);
      this.BindClass((IEntityMapping) subClassMapping, (PersistentClass) subclass, inheritedMetas);
      inheritedMetas = Binder.GetMetas((IDecoratable) subClassMapping, inheritedMetas, true);
      if (subclass.EntityPersisterClass == null)
        subclass.RootClazz.EntityPersisterClass = typeof (SingleTableEntityPersister);
      Binder.log.InfoFormat("Mapping subclass: {0} -> {1}", (object) subclass.EntityName, (object) subclass.Table.Name);
      new PropertiesBinder(this.mappings, (PersistentClass) subclass, this.dialect).Bind(subClassMapping.Properties, inheritedMetas);
      this.BindJoins(subClassMapping.Joins, (PersistentClass) subclass, inheritedMetas);
      this.BindSubclasses(subClassMapping.Subclasses, (PersistentClass) subclass, inheritedMetas);
      model.AddSubclass(subclass);
      this.mappings.AddClass((PersistentClass) subclass);
    }
  }
}

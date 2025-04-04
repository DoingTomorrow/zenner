// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.ClassIdBinder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping;
using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Cfg.XmlHbmBinding
{
  public class ClassIdBinder(ClassBinder parent) : ClassBinder(parent)
  {
    public void BindId(HbmId idSchema, PersistentClass rootClass, Table table)
    {
      if (idSchema == null)
        return;
      SimpleValue simpleValue = new SimpleValue(table);
      new TypeBinder(simpleValue, this.Mappings).Bind(idSchema.Type);
      rootClass.Identifier = (IKeyValue) simpleValue;
      Func<HbmColumn> defaultColumnDelegate = (Func<HbmColumn>) (() => new HbmColumn()
      {
        name = idSchema.name ?? "id",
        length = idSchema.length
      });
      new ColumnsBinder(simpleValue, this.Mappings).Bind(idSchema.Columns, false, defaultColumnDelegate);
      this.CreateIdentifierProperty(idSchema, rootClass, simpleValue);
      ClassIdBinder.VerifiyIdTypeIsValid(simpleValue.Type, rootClass.EntityName);
      new IdGeneratorBinder(this.Mappings).BindGenerator(simpleValue, this.GetIdGenerator(idSchema));
      simpleValue.Table.SetIdentifierValue(simpleValue);
      ClassIdBinder.BindUnsavedValue(idSchema, simpleValue);
    }

    private void CreateIdentifierProperty(
      HbmId idSchema,
      PersistentClass rootClass,
      SimpleValue id)
    {
      if (idSchema.name == null)
        return;
      string accesorName = idSchema.access ?? this.mappings.DefaultAccess;
      id.SetTypeUsingReflection(rootClass.MappedClass == null ? (string) null : rootClass.MappedClass.AssemblyQualifiedName, idSchema.name, accesorName);
      Property property = new Property((IValue) id)
      {
        Name = idSchema.name
      };
      if (property.Value.Type == null)
        throw new MappingException("could not determine a property type for: " + property.Name);
      property.PropertyAccessorName = idSchema.access ?? this.mappings.DefaultAccess;
      property.Cascade = this.mappings.DefaultCascade;
      property.IsUpdateable = true;
      property.IsInsertable = true;
      property.IsOptimisticLocked = true;
      property.Generation = PropertyGeneration.Never;
      property.MetaAttributes = Binder.GetMetas((IDecoratable) idSchema, Binder.EmptyMeta);
      rootClass.IdentifierProperty = property;
      property.LogMapped(Binder.log);
    }

    private HbmGenerator GetIdGenerator(HbmId idSchema)
    {
      if (string.IsNullOrEmpty(idSchema.generator1))
        return idSchema.generator;
      return new HbmGenerator()
      {
        @class = idSchema.generator1
      };
    }

    private static void VerifiyIdTypeIsValid(IType idType, string className)
    {
      if (idType == null)
        throw new MappingException(string.Format("Must specify an identifier type: {0}.", (object) className));
      if (idType.ReturnedClass.IsArray)
        throw new MappingException("Illegal use of an array as an identifier (arrays don't reimplement equals).");
    }

    private static void BindUnsavedValue(HbmId idSchema, SimpleValue id)
    {
      id.NullValue = idSchema.unsavedvalue ?? (id.IdentifierGeneratorStrategy == "assigned" ? "undefined" : (string) null);
    }
  }
}

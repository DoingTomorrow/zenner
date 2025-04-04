// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.PropertiesBinder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Cfg.XmlHbmBinding
{
  public class PropertiesBinder : ClassBinder
  {
    private readonly PersistentClass persistentClass;
    private readonly Component component;
    private readonly string entityName;
    private readonly Type mappedClass;
    private readonly string className;
    private readonly bool componetDefaultNullable;
    private readonly string propertyBasePath;

    public PropertiesBinder(Mappings mappings, PersistentClass persistentClass, NHibernate.Dialect.Dialect dialect)
      : base(mappings, dialect)
    {
      this.persistentClass = persistentClass;
      this.entityName = persistentClass.EntityName;
      this.propertyBasePath = this.entityName;
      this.className = persistentClass.ClassName;
      this.mappedClass = persistentClass.MappedClass;
      this.componetDefaultNullable = true;
      this.component = (Component) null;
    }

    public PropertiesBinder(
      Mappings mappings,
      Component component,
      string className,
      string path,
      bool isNullable,
      NHibernate.Dialect.Dialect dialect)
      : base(mappings, dialect)
    {
      this.persistentClass = component.Owner;
      this.component = component;
      this.entityName = className;
      this.className = component.ComponentClassName;
      this.mappedClass = component.ComponentClass;
      this.propertyBasePath = path;
      this.componetDefaultNullable = isNullable;
    }

    public void Bind(
      IEnumerable<IEntityPropertyMapping> properties,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      this.Bind(properties, inheritedMetas, (Action<Property>) (p => { }));
    }

    public void Bind(
      IEnumerable<IEntityPropertyMapping> properties,
      IDictionary<string, MetaAttribute> inheritedMetas,
      Action<Property> modifier)
    {
      this.Bind(properties, this.persistentClass.Table, inheritedMetas, modifier, (Action<Property>) (p => this.persistentClass.AddProperty(p)));
    }

    public void Bind(
      IEnumerable<IEntityPropertyMapping> properties,
      Table table,
      IDictionary<string, MetaAttribute> inheritedMetas,
      Action<Property> modifier,
      Action<Property> addToModelAction)
    {
      if (table == null)
        throw new ArgumentNullException(nameof (table));
      if (modifier == null)
        throw new ArgumentNullException(nameof (modifier));
      if (addToModelAction == null)
        throw new ArgumentNullException(nameof (addToModelAction));
      foreach (IEntityPropertyMapping property1 in properties)
      {
        Property property2 = (Property) null;
        string name = property1.Name;
        switch (property1)
        {
          case HbmProperty propertyMapping:
            SimpleValue simpleValue1 = new SimpleValue(table);
            new ValuePropertyBinder(simpleValue1, this.Mappings).BindSimpleValue(propertyMapping, name, true);
            property2 = this.CreateProperty(property1, this.className, (IValue) simpleValue1, inheritedMetas);
            this.BindValueProperty(propertyMapping, property2);
            break;
          case ICollectionPropertiesMapping propertiesMapping1:
            CollectionBinder collectionBinder = new CollectionBinder(this.Mappings, this.dialect);
            string propertyFullPath = name == null ? (string) null : StringHelper.Qualify(this.propertyBasePath, name);
            Collection collection = collectionBinder.Create(propertiesMapping1, this.entityName, propertyFullPath, this.persistentClass, this.mappedClass, inheritedMetas);
            this.mappings.AddCollection(collection);
            property2 = this.CreateProperty((IEntityPropertyMapping) propertiesMapping1, this.className, (IValue) collection, inheritedMetas);
            this.BindCollectionProperty(propertiesMapping1, property2);
            break;
          case HbmProperties propertiesMapping2:
            string path1 = name == null ? (string) null : StringHelper.Qualify(this.propertyBasePath, name);
            Component newComponent1 = this.CreateNewComponent(table);
            this.BindComponent((IComponentMapping) propertiesMapping2, newComponent1, (Type) null, this.entityName, path1, this.componetDefaultNullable, inheritedMetas);
            property2 = this.CreateProperty(property1, this.className, (IValue) newComponent1, inheritedMetas);
            this.BindComponentProperty(propertiesMapping2, property2, newComponent1);
            break;
          case HbmManyToOne manyToOneMapping:
            ManyToOne model1 = new ManyToOne(table);
            this.BindManyToOne(manyToOneMapping, model1, name, true);
            property2 = this.CreateProperty(property1, this.className, (IValue) model1, inheritedMetas);
            this.BindManyToOneProperty(manyToOneMapping, property2);
            break;
          case HbmComponent componentMapping:
            string path2 = name == null ? (string) null : StringHelper.Qualify(this.propertyBasePath, name);
            Component newComponent2 = this.CreateNewComponent(table);
            Type propertyType1 = this.mappedClass == null ? (Type) null : this.GetPropertyType(componentMapping.Class, this.mappedClass, name, componentMapping.Access);
            this.BindComponent((IComponentMapping) componentMapping, newComponent2, propertyType1, this.entityName, path2, this.componetDefaultNullable, inheritedMetas);
            property2 = this.CreateProperty(property1, this.className, (IValue) newComponent2, inheritedMetas);
            this.BindComponentProperty(componentMapping, property2, newComponent2);
            break;
          case HbmOneToOne oneToOneMapping:
            OneToOne model2 = new OneToOne(table, this.persistentClass);
            this.BindOneToOne(oneToOneMapping, model2);
            property2 = this.CreateProperty(property1, this.className, (IValue) model2, inheritedMetas);
            this.BindOneToOneProperty(oneToOneMapping, property2);
            break;
          case HbmDynamicComponent dynamicComponentMapping:
            string path3 = name == null ? (string) null : StringHelper.Qualify(this.propertyBasePath, name);
            Component newComponent3 = this.CreateNewComponent(table);
            Type propertyType2 = this.mappedClass == null ? (Type) null : this.GetPropertyType(dynamicComponentMapping.Class, this.mappedClass, name, dynamicComponentMapping.Access);
            this.BindComponent((IComponentMapping) dynamicComponentMapping, newComponent3, propertyType2, this.entityName, path3, this.componetDefaultNullable, inheritedMetas);
            property2 = this.CreateProperty(property1, this.className, (IValue) newComponent3, inheritedMetas);
            this.BindComponentProperty(dynamicComponentMapping, property2, newComponent3);
            break;
          case HbmAny hbmAny:
            Any model3 = new Any(table);
            this.BindAny(hbmAny, model3, true);
            property2 = this.CreateProperty(property1, this.className, (IValue) model3, inheritedMetas);
            this.BindAnyProperty(hbmAny, property2);
            break;
          case HbmNestedCompositeElement compositeElement:
            if (this.component == null)
              throw new AssertionFailure("Nested Composite Element without a owner component.");
            string path4 = name == null ? (string) null : StringHelper.Qualify(this.propertyBasePath, name);
            Component newComponent4 = this.CreateNewComponent(table);
            Type propertyType3 = this.mappedClass == null ? (Type) null : this.GetPropertyType(compositeElement.Class, this.mappedClass, name, compositeElement.access);
            this.BindComponent((IComponentMapping) compositeElement, newComponent4, propertyType3, this.entityName, path4, this.componetDefaultNullable, inheritedMetas);
            property2 = this.CreateProperty(property1, this.className, (IValue) newComponent4, inheritedMetas);
            break;
          case HbmKeyProperty mapKeyManyToManyMapping:
            SimpleValue simpleValue2 = new SimpleValue(table);
            new ValuePropertyBinder(simpleValue2, this.Mappings).BindSimpleValue(mapKeyManyToManyMapping, name, this.componetDefaultNullable);
            property2 = this.CreateProperty(property1, this.className, (IValue) simpleValue2, inheritedMetas);
            break;
          case HbmKeyManyToOne keyManyToOneMapping:
            ManyToOne model4 = new ManyToOne(table);
            this.BindKeyManyToOne(keyManyToOneMapping, model4, name, this.componetDefaultNullable);
            property2 = this.CreateProperty(property1, this.className, (IValue) model4, inheritedMetas);
            break;
        }
        if (property2 != null)
        {
          modifier(property2);
          property2.LogMapped(Binder.log);
          addToModelAction(property2);
        }
      }
    }

    private Component CreateNewComponent(Table table)
    {
      return this.component == null ? new Component(table, this.persistentClass) : new Component(this.component);
    }

    private Type GetPropertyType(
      string classMapping,
      Type containingType,
      string propertyName,
      string propertyAccess)
    {
      if (!string.IsNullOrEmpty(classMapping))
        return Binder.ClassForNameChecked(classMapping, this.mappings, "could not find class: {0}");
      if (containingType == null)
        return (Type) null;
      string propertyAccessorName = this.GetPropertyAccessorName(propertyAccess);
      return ReflectHelper.ReflectedPropertyClass(containingType, propertyName, propertyAccessorName);
    }

    private void BindKeyManyToOne(
      HbmKeyManyToOne keyManyToOneMapping,
      ManyToOne model,
      string defaultColumnName,
      bool isNullable)
    {
      new ValuePropertyBinder((SimpleValue) model, this.Mappings).BindSimpleValue(keyManyToOneMapping, defaultColumnName, isNullable);
      ClassBinder.InitLaziness(keyManyToOneMapping.Lazy, (ToOne) model, true);
      model.ReferencedEntityName = ClassBinder.GetEntityName((IRelationship) keyManyToOneMapping, this.mappings);
      model.IsIgnoreNotFound = keyManyToOneMapping.NotFoundMode == HbmNotFoundMode.Ignore;
      this.BindForeignKey(keyManyToOneMapping.foreignkey, (SimpleValue) model);
    }

    private void BindManyToOne(
      HbmManyToOne manyToOneMapping,
      ManyToOne model,
      string defaultColumnName,
      bool isNullable)
    {
      new ValuePropertyBinder((SimpleValue) model, this.Mappings).BindSimpleValue(manyToOneMapping, defaultColumnName, isNullable);
      this.InitOuterJoinFetchSetting(manyToOneMapping, model);
      ClassBinder.InitLaziness(manyToOneMapping.Lazy, (ToOne) model, true);
      string propertyref = !string.IsNullOrEmpty(manyToOneMapping.propertyref) ? manyToOneMapping.propertyref : (string) null;
      if (propertyref != null)
        model.ReferencedPropertyName = propertyref;
      model.ReferencedEntityName = ClassBinder.GetEntityName((IRelationship) manyToOneMapping, this.mappings);
      model.IsIgnoreNotFound = manyToOneMapping.NotFoundMode == HbmNotFoundMode.Ignore;
      if (propertyref != null && !model.IsIgnoreNotFound)
        this.AddManyToOneSecondPass(model);
      this.BindForeignKey(manyToOneMapping.foreignkey, (SimpleValue) model);
    }

    private void InitOuterJoinFetchSetting(HbmManyToOne manyToOneMapping, ManyToOne model)
    {
      FetchMode fetchMode = !manyToOneMapping.fetchSpecified ? (!manyToOneMapping.outerjoinSpecified ? FetchMode.Default : this.GetFetchStyle(manyToOneMapping.outerjoin)) : this.GetFetchStyle(manyToOneMapping.fetch);
      model.FetchMode = fetchMode;
    }

    private void AddManyToOneSecondPass(ManyToOne manyToOne)
    {
      this.mappings.AddSecondPass(new SecondPassCommand(manyToOne.CreatePropertyRefConstraints));
    }

    private void BindValueProperty(HbmProperty propertyMapping, Property property)
    {
      property.IsUpdateable = !propertyMapping.updateSpecified || propertyMapping.update;
      property.IsInsertable = !propertyMapping.insertSpecified || propertyMapping.insert;
      PropertyGeneration propertyGeneration;
      switch (propertyMapping.generated)
      {
        case HbmPropertyGeneration.Never:
          propertyGeneration = PropertyGeneration.Never;
          break;
        case HbmPropertyGeneration.Insert:
          propertyGeneration = PropertyGeneration.Insert;
          break;
        case HbmPropertyGeneration.Always:
          propertyGeneration = PropertyGeneration.Always;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
      property.Generation = propertyGeneration;
      if (propertyGeneration != PropertyGeneration.Always && propertyGeneration != PropertyGeneration.Insert)
        return;
      if (propertyMapping.insertSpecified && property.IsInsertable)
        throw new MappingException("cannot specify both insert=\"true\" and generated=\"" + (object) propertyGeneration + "\" for property: " + propertyMapping.Name);
      property.IsInsertable = false;
      if (propertyMapping.updateSpecified && property.IsUpdateable && propertyGeneration == PropertyGeneration.Always)
        throw new MappingException("cannot specify both update=\"true\" and generated=\"" + (object) propertyGeneration + "\" for property: " + propertyMapping.Name);
      property.IsUpdateable = false;
    }

    private void BindAnyProperty(HbmAny anyMapping, Property property)
    {
      property.Cascade = anyMapping.cascade ?? this.mappings.DefaultCascade;
      property.IsUpdateable = anyMapping.update;
      property.IsInsertable = anyMapping.insert;
    }

    private void BindOneToOneProperty(HbmOneToOne oneToOneMapping, Property property)
    {
      property.Cascade = oneToOneMapping.cascade ?? this.mappings.DefaultCascade;
      Property property1 = property;
      HbmLaziness? lazy = oneToOneMapping.Lazy;
      int num = lazy.GetValueOrDefault() != HbmLaziness.NoProxy ? 0 : (lazy.HasValue ? 1 : 0);
      property1.UnwrapProxy = num != 0;
      if (!(property.Value is ToOne toOne))
        return;
      string referencedPropertyName = toOne.ReferencedPropertyName;
      if (referencedPropertyName != null)
        this.mappings.AddUniquePropertyReference(toOne.ReferencedEntityName, referencedPropertyName);
      toOne.CreateForeignKey();
    }

    private void BindComponentProperty(
      HbmDynamicComponent dynamicComponentMapping,
      Property property,
      Component model)
    {
      property.IsUpdateable = dynamicComponentMapping.update;
      property.IsInsertable = dynamicComponentMapping.insert;
      if (!dynamicComponentMapping.unique)
        return;
      model.Owner.Table.CreateUniqueKey((IList<Column>) model.ColumnIterator.OfType<Column>().ToList<Column>());
    }

    private void BindComponentProperty(
      HbmProperties propertiesMapping,
      Property property,
      Component model)
    {
      property.IsUpdateable = propertiesMapping.update;
      property.IsInsertable = propertiesMapping.insert;
      if (!propertiesMapping.unique)
        return;
      model.Owner.Table.CreateUniqueKey((IList<Column>) model.ColumnIterator.OfType<Column>().ToList<Column>());
    }

    private void BindComponentProperty(
      HbmComponent componentMapping,
      Property property,
      Component model)
    {
      property.IsUpdateable = componentMapping.update;
      property.IsInsertable = componentMapping.insert;
      if (componentMapping.unique)
        model.Owner.Table.CreateUniqueKey((IList<Column>) model.ColumnIterator.OfType<Column>().ToList<Column>());
      HbmTuplizer[] tuplizer1 = componentMapping.tuplizer;
      if (tuplizer1 == null)
        return;
      System.Array.ForEach(((IEnumerable<HbmTuplizer>) tuplizer1).Select(tuplizer => new
      {
        TuplizerClassName = Binder.FullQualifiedClassName(tuplizer.@class, this.mappings),
        Mode = tuplizer.entitymode.ToEntityMode()
      }).ToArray(), x => model.AddTuplizer(x.Mode, x.TuplizerClassName));
    }

    private void BindManyToOneProperty(HbmManyToOne manyToOneMapping, Property property)
    {
      property.Cascade = manyToOneMapping.cascade ?? this.mappings.DefaultCascade;
      Property property1 = property;
      HbmLaziness? lazy = manyToOneMapping.Lazy;
      int num = lazy.GetValueOrDefault() != HbmLaziness.NoProxy ? 0 : (lazy.HasValue ? 1 : 0);
      property1.UnwrapProxy = num != 0;
      property.IsUpdateable = manyToOneMapping.update;
      property.IsInsertable = manyToOneMapping.insert;
      if (!(property.Value is ToOne toOne))
        return;
      string referencedPropertyName = toOne.ReferencedPropertyName;
      if (referencedPropertyName != null)
        this.mappings.AddUniquePropertyReference(toOne.ReferencedEntityName, referencedPropertyName);
      toOne.CreateForeignKey();
    }

    private void BindCollectionProperty(
      ICollectionPropertiesMapping collectionMapping,
      Property property)
    {
      property.Cascade = collectionMapping.Cascade ?? this.mappings.DefaultCascade;
    }

    private Property CreateProperty(
      IEntityPropertyMapping propertyMapping,
      string propertyOwnerClassName,
      IValue value,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      string accesorName = !string.IsNullOrEmpty(propertyMapping.Name) ? this.GetPropertyAccessorName(propertyMapping.Access) : throw new MappingException("A property mapping must define the name attribute [" + propertyOwnerClassName + "]");
      if (!string.IsNullOrEmpty(propertyOwnerClassName) && value.IsSimpleValue)
        value.SetTypeUsingReflection(propertyOwnerClassName, propertyMapping.Name, accesorName);
      return new Property()
      {
        Name = propertyMapping.Name,
        PropertyAccessorName = accesorName,
        Value = value,
        IsLazy = propertyMapping.IsLazyProperty,
        IsOptimisticLocked = propertyMapping.OptimisticLock,
        MetaAttributes = Binder.GetMetas((IDecoratable) propertyMapping, inheritedMetas)
      };
    }

    private string GetPropertyAccessorName(string propertyMappedAccessor)
    {
      return propertyMappedAccessor ?? this.Mappings.DefaultAccess;
    }
  }
}

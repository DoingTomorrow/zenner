// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.ClassCompositeIdBinder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Cfg.XmlHbmBinding
{
  public class ClassCompositeIdBinder(ClassBinder parent) : ClassBinder(parent)
  {
    private Component compositeId;

    public void BindCompositeId(HbmCompositeId idSchema, PersistentClass rootClass)
    {
      if (idSchema == null)
        return;
      this.compositeId = new Component(rootClass);
      this.compositeId.IsKey = true;
      rootClass.Identifier = (IKeyValue) this.compositeId;
      if (idSchema.name == null)
      {
        this.BindComponent((Type) null, "id", idSchema);
        rootClass.HasEmbeddedIdentifier = this.compositeId.IsEmbedded;
      }
      else
      {
        this.BindComponent(this.GetPropertyType(rootClass.MappedClass, idSchema.name, idSchema), idSchema.name, idSchema);
        Property property = new Property((IValue) this.compositeId);
        this.BindProperty(property, idSchema);
        rootClass.IdentifierProperty = property;
      }
      this.compositeId.Table.SetIdentifierValue((SimpleValue) this.compositeId);
      this.compositeId.NullValue = idSchema.unsavedvalue.ToNullValue();
      if (this.compositeId.IsDynamic)
        return;
      this.CheckEqualsAndGetHashCodeOverride();
    }

    private void CheckEqualsAndGetHashCodeOverride()
    {
      Type componentClass = this.compositeId.ComponentClass;
      if (!ReflectHelper.OverridesEquals(componentClass))
        throw new MappingException("composite-id class must override Equals(): " + componentClass.FullName);
      if (!ReflectHelper.OverridesGetHashCode(componentClass))
        throw new MappingException("composite-id class must override GetHashCode(): " + componentClass.FullName);
    }

    private void BindComponent(Type reflectedClass, string path, HbmCompositeId idSchema)
    {
      if (idSchema.@class != null)
      {
        this.compositeId.ComponentClass = Binder.ClassForNameChecked(idSchema.@class, this.mappings, "component class not found: {0}");
        this.compositeId.IsEmbedded = false;
      }
      else if (reflectedClass != null)
      {
        this.compositeId.ComponentClass = reflectedClass;
        this.compositeId.IsEmbedded = false;
      }
      else if (this.compositeId.Owner.HasPocoRepresentation)
      {
        this.compositeId.ComponentClass = this.compositeId.Owner.MappedClass;
        this.compositeId.IsEmbedded = true;
      }
      else
        this.compositeId.IsDynamic = true;
      foreach (object obj in idSchema.Items ?? new object[0])
      {
        HbmKeyManyToOne keyManyToOneSchema = obj as HbmKeyManyToOne;
        HbmKeyProperty keyPropertySchema = obj as HbmKeyProperty;
        if (keyManyToOneSchema != null)
        {
          ManyToOne manyToOne = new ManyToOne(this.compositeId.Table);
          string defaultColumnName = keyManyToOneSchema.name == null ? (string) null : StringHelper.Qualify(path, keyManyToOneSchema.name);
          this.BindManyToOne(keyManyToOneSchema, manyToOne, defaultColumnName, false);
          this.compositeId.AddProperty(this.CreateProperty((ToOne) manyToOne, keyManyToOneSchema.name, this.compositeId.ComponentClass, keyManyToOneSchema));
        }
        else if (keyPropertySchema != null)
        {
          SimpleValue model = new SimpleValue(this.compositeId.Table);
          string path1 = keyPropertySchema.name == null ? (string) null : StringHelper.Qualify(path, keyPropertySchema.name);
          this.BindSimpleValue(keyPropertySchema, model, false, path1);
          this.compositeId.AddProperty(this.CreateProperty(model, keyPropertySchema.name, this.compositeId.ComponentClass, keyPropertySchema));
        }
      }
    }

    private void BindProperty(Property property, HbmCompositeId idSchema)
    {
      property.Name = idSchema.name;
      if (property.Value.Type == null)
        throw new MappingException("could not determine a property type for: " + property.Name);
      property.PropertyAccessorName = idSchema.access ?? this.mappings.DefaultAccess;
      property.Cascade = this.mappings.DefaultCascade;
      property.IsUpdateable = true;
      property.IsInsertable = true;
      property.IsOptimisticLocked = true;
      property.Generation = PropertyGeneration.Never;
      property.MetaAttributes = (IDictionary<string, MetaAttribute>) new Dictionary<string, MetaAttribute>();
      property.LogMapped(Binder.log);
    }

    private Type GetPropertyType(Type containingType, string propertyName, HbmCompositeId idSchema)
    {
      if (idSchema.@class != null)
        return Binder.ClassForNameChecked(idSchema.@class, this.mappings, "could not find class: {0}");
      if (containingType == null)
        return (Type) null;
      string access = idSchema.access ?? this.mappings.DefaultAccess;
      return ReflectHelper.ReflectedPropertyClass(containingType, propertyName, access);
    }

    private void BindManyToOne(
      HbmKeyManyToOne keyManyToOneSchema,
      ManyToOne manyToOne,
      string defaultColumnName,
      bool isNullable)
    {
      new ColumnsBinder((SimpleValue) manyToOne, this.mappings).Bind(keyManyToOneSchema.Columns, isNullable, (Func<HbmColumn>) (() => new HbmColumn()
      {
        name = this.mappings.NamingStrategy.PropertyToColumnName(defaultColumnName)
      }));
      manyToOne.FetchMode = FetchMode.Default;
      manyToOne.IsLazy = !keyManyToOneSchema.lazySpecified ? manyToOne.IsLazy : keyManyToOneSchema.lazy == HbmRestrictedLaziness.Proxy;
      string unqualifiedName = keyManyToOneSchema.@class;
      if (unqualifiedName != null)
        manyToOne.ReferencedEntityName = Binder.GetClassName(unqualifiedName, this.mappings);
      else
        manyToOne.ReferencedEntityName = (string) null;
      manyToOne.IsIgnoreNotFound = false;
      if (keyManyToOneSchema.foreignkey == null)
        return;
      manyToOne.ForeignKeyName = keyManyToOneSchema.foreignkey;
    }

    private Property CreateProperty(
      ToOne value,
      string propertyName,
      Type parentClass,
      HbmKeyManyToOne keyManyToOneSchema)
    {
      if (parentClass != null && value.IsSimpleValue)
        value.SetTypeUsingReflection(parentClass.AssemblyQualifiedName, propertyName, keyManyToOneSchema.access ?? this.mappings.DefaultAccess);
      string referencedPropertyName = value.ReferencedPropertyName;
      if (referencedPropertyName != null)
        this.mappings.AddUniquePropertyReference(value.ReferencedEntityName, referencedPropertyName);
      value.CreateForeignKey();
      Property property = new Property()
      {
        Value = (IValue) value
      };
      this.BindProperty(keyManyToOneSchema, property);
      return property;
    }

    private void BindProperty(HbmKeyManyToOne keyManyToOneSchema, Property property)
    {
      property.Name = keyManyToOneSchema.name;
      if (property.Value.Type == null)
        throw new MappingException("could not determine a property type for: " + property.Name);
      property.PropertyAccessorName = keyManyToOneSchema.access ?? this.mappings.DefaultAccess;
      property.Cascade = this.mappings.DefaultCascade;
      property.IsUpdateable = true;
      property.IsInsertable = true;
      property.IsOptimisticLocked = true;
      property.Generation = PropertyGeneration.Never;
      property.MetaAttributes = (IDictionary<string, MetaAttribute>) new Dictionary<string, MetaAttribute>();
      property.LogMapped(Binder.log);
    }

    private void BindSimpleValue(
      HbmKeyProperty keyPropertySchema,
      SimpleValue model,
      bool isNullable,
      string path)
    {
      if (keyPropertySchema.type1 != null)
        model.TypeName = keyPropertySchema.type1;
      new ColumnsBinder(model, this.Mappings).Bind(keyPropertySchema.Columns, isNullable, (Func<HbmColumn>) (() => new HbmColumn()
      {
        name = this.mappings.NamingStrategy.PropertyToColumnName(path),
        length = keyPropertySchema.length
      }));
    }

    private Property CreateProperty(
      SimpleValue value,
      string propertyName,
      Type parentClass,
      HbmKeyProperty keyPropertySchema)
    {
      if (parentClass != null && value.IsSimpleValue)
        value.SetTypeUsingReflection(parentClass.AssemblyQualifiedName, propertyName, keyPropertySchema.access ?? this.mappings.DefaultAccess);
      if (value is ToOne toOne)
      {
        string referencedPropertyName = toOne.ReferencedPropertyName;
        if (referencedPropertyName != null)
          this.mappings.AddUniquePropertyReference(toOne.ReferencedEntityName, referencedPropertyName);
      }
      value.CreateForeignKey();
      Property property = new Property()
      {
        Value = (IValue) value
      };
      this.BindProperty(keyPropertySchema, property);
      return property;
    }

    private void BindProperty(HbmKeyProperty keyPropertySchema, Property property)
    {
      property.Name = keyPropertySchema.name;
      if (property.Value.Type == null)
        throw new MappingException("could not determine a property type for: " + property.Name);
      property.PropertyAccessorName = keyPropertySchema.access ?? this.mappings.DefaultAccess;
      property.Cascade = this.mappings.DefaultCascade;
      property.IsUpdateable = true;
      property.IsInsertable = true;
      property.IsOptimisticLocked = true;
      property.Generation = PropertyGeneration.Never;
      property.MetaAttributes = (IDictionary<string, MetaAttribute>) new Dictionary<string, MetaAttribute>();
      property.LogMapped(Binder.log);
    }
  }
}

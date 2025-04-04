// Decompiled with JetBrains decompiler
// Type: NHibernate.Tuple.Entity.EntityMetamodel
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Intercept;
using NHibernate.Mapping;
using NHibernate.Properties;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace NHibernate.Tuple.Entity
{
  [Serializable]
  public class EntityMetamodel
  {
    private const int NoVersionIndex = -66;
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (EntityMetamodel));
    private readonly ISessionFactoryImplementor sessionFactory;
    private readonly string name;
    private readonly string rootName;
    private readonly System.Type type;
    private readonly System.Type rootType;
    private readonly string rootTypeAssemblyQualifiedName;
    private readonly EntityType entityType;
    private readonly IdentifierProperty identifierProperty;
    private readonly bool versioned;
    private readonly int propertySpan;
    private readonly int versionPropertyIndex;
    private readonly StandardProperty[] properties;
    private readonly string[] propertyNames;
    private readonly IType[] propertyTypes;
    private readonly bool[] propertyLaziness;
    private readonly bool[] propertyUpdateability;
    private readonly bool[] nonlazyPropertyUpdateability;
    private readonly bool[] propertyCheckability;
    private readonly bool[] propertyInsertability;
    private readonly ValueInclusion[] insertInclusions;
    private readonly ValueInclusion[] updateInclusions;
    private readonly bool[] propertyNullability;
    private readonly bool[] propertyVersionability;
    private readonly CascadeStyle[] cascadeStyles;
    private readonly IDictionary<string, int?> propertyIndexes = (IDictionary<string, int?>) new Dictionary<string, int?>();
    private readonly bool hasCollections;
    private readonly bool hasMutableProperties;
    private readonly bool hasLazyProperties;
    private readonly int[] naturalIdPropertyNumbers;
    private bool lazy;
    private readonly bool hasCascades;
    private readonly bool hasNonIdentifierPropertyNamedId;
    private readonly bool mutable;
    private readonly bool isAbstract;
    private readonly bool selectBeforeUpdate;
    private readonly bool dynamicUpdate;
    private readonly bool dynamicInsert;
    private readonly Versioning.OptimisticLock optimisticLockMode;
    private readonly bool polymorphic;
    private readonly string superclass;
    private readonly System.Type superclassType;
    private readonly bool explicitPolymorphism;
    private readonly bool inherited;
    private readonly bool hasSubclasses;
    private readonly HashedSet<string> subclassEntityNames = new HashedSet<string>();
    private readonly bool hasInsertGeneratedValues;
    private readonly bool hasUpdateGeneratedValues;
    private readonly EntityEntityModeToTuplizerMapping tuplizerMapping;
    private bool hasUnwrapProxyForProperties;

    public EntityMetamodel(
      PersistentClass persistentClass,
      ISessionFactoryImplementor sessionFactory)
    {
      this.sessionFactory = sessionFactory;
      this.name = persistentClass.EntityName;
      this.rootName = persistentClass.RootClazz.EntityName;
      this.entityType = TypeFactory.ManyToOne(this.name);
      this.type = persistentClass.MappedClass;
      this.rootType = persistentClass.RootClazz.MappedClass;
      this.rootTypeAssemblyQualifiedName = this.rootType == null ? (string) null : this.rootType.AssemblyQualifiedName;
      this.identifierProperty = PropertyFactory.BuildIdentifierProperty(persistentClass, sessionFactory.GetIdentifierGenerator(this.rootName));
      this.versioned = persistentClass.IsVersioned;
      bool flag1 = persistentClass.HasPocoRepresentation && FieldInterceptionHelper.IsInstrumented(persistentClass.MappedClass);
      bool flag2 = false;
      this.propertySpan = persistentClass.PropertyClosureSpan;
      this.properties = new StandardProperty[this.propertySpan];
      List<int> intList = new List<int>();
      this.propertyNames = new string[this.propertySpan];
      this.propertyTypes = new IType[this.propertySpan];
      this.propertyUpdateability = new bool[this.propertySpan];
      this.propertyInsertability = new bool[this.propertySpan];
      this.insertInclusions = new ValueInclusion[this.propertySpan];
      this.updateInclusions = new ValueInclusion[this.propertySpan];
      this.nonlazyPropertyUpdateability = new bool[this.propertySpan];
      this.propertyCheckability = new bool[this.propertySpan];
      this.propertyNullability = new bool[this.propertySpan];
      this.propertyVersionability = new bool[this.propertySpan];
      this.propertyLaziness = new bool[this.propertySpan];
      this.cascadeStyles = new CascadeStyle[this.propertySpan];
      int i = 0;
      int num = -66;
      bool flag3 = false;
      bool flag4 = false;
      bool flag5 = false;
      bool flag6 = false;
      bool flag7 = false;
      bool flag8 = false;
      this.HasPocoRepresentation = persistentClass.HasPocoRepresentation;
      this.lazy = persistentClass.IsLazy && (!persistentClass.HasPocoRepresentation || !ReflectHelper.IsFinalClass(persistentClass.ProxyInterface));
      bool flag9 = flag1 & this.lazy;
      bool flag10 = false;
      bool flag11 = false;
      foreach (NHibernate.Mapping.Property property in persistentClass.PropertyClosureIterator)
      {
        if (property.IsLazy)
          flag10 = true;
        if (property.UnwrapProxy)
          flag11 = true;
        bool lazyAvailable = property.IsLazy && flag9 && (!property.IsEntityRelation || property.UnwrapProxy);
        bool flag12 = property.UnwrapProxy && flag9;
        if (lazyAvailable || flag12)
        {
          IGetter getter = property.GetGetter(persistentClass.MappedClass);
          if (getter.Method == null || !getter.Method.IsDefined(typeof (CompilerGeneratedAttribute), false))
            EntityMetamodel.log.ErrorFormat("Lazy or no-proxy property {0}.{1} is not an auto property, which may result in uninitialized property access", (object) persistentClass.EntityName, (object) property.Name);
        }
        if (property == persistentClass.Version)
        {
          num = i;
          this.properties[i] = (StandardProperty) PropertyFactory.BuildVersionProperty(property, lazyAvailable);
        }
        else
          this.properties[i] = PropertyFactory.BuildStandardProperty(property, lazyAvailable);
        if (property.IsNaturalIdentifier)
          intList.Add(i);
        if ("id".Equals(property.Name))
          flag8 = true;
        if (lazyAvailable)
          flag2 = true;
        if (flag12)
          this.hasUnwrapProxyForProperties = true;
        this.propertyLaziness[i] = lazyAvailable;
        this.propertyNames[i] = this.properties[i].Name;
        this.propertyTypes[i] = this.properties[i].Type;
        this.propertyNullability[i] = this.properties[i].IsNullable;
        this.propertyUpdateability[i] = this.properties[i].IsUpdateable;
        this.propertyInsertability[i] = this.properties[i].IsInsertable;
        this.insertInclusions[i] = this.DetermineInsertValueGenerationType(property, this.properties[i]);
        this.updateInclusions[i] = this.DetermineUpdateValueGenerationType(property, this.properties[i]);
        this.propertyVersionability[i] = this.properties[i].IsVersionable;
        this.nonlazyPropertyUpdateability[i] = this.properties[i].IsUpdateable && !lazyAvailable;
        this.propertyCheckability[i] = this.propertyUpdateability[i] || this.propertyTypes[i].IsAssociationType && ((IAssociationType) this.propertyTypes[i]).IsAlwaysDirtyChecked;
        this.cascadeStyles[i] = this.properties[i].CascadeStyle;
        if (this.properties[i].IsLazy)
          flag2 = true;
        if (this.properties[i].CascadeStyle != CascadeStyle.None)
          flag3 = true;
        if (this.IndicatesCollection(this.properties[i].Type))
          flag4 = true;
        if (this.propertyTypes[i].IsMutable && this.propertyCheckability[i])
          flag5 = true;
        if (this.insertInclusions[i] != ValueInclusion.None)
          flag6 = true;
        if (this.updateInclusions[i] != ValueInclusion.None)
          flag7 = true;
        this.MapPropertyToIndex(property, i);
        ++i;
      }
      this.naturalIdPropertyNumbers = intList.Count != 0 ? intList.ToArray() : (int[]) null;
      this.hasCascades = flag3;
      this.hasInsertGeneratedValues = flag6;
      this.hasUpdateGeneratedValues = flag7;
      this.hasNonIdentifierPropertyNamedId = flag8;
      this.versionPropertyIndex = num;
      this.hasLazyProperties = flag2;
      if (flag10 && !flag2)
        EntityMetamodel.log.WarnFormat("Disabled lazy property fetching for {0} because it does not support lazy at the entity level", (object) this.name);
      if (flag2)
        EntityMetamodel.log.Info((object) ("lazy property fetching available for: " + this.name));
      if (flag11 && !this.hasUnwrapProxyForProperties)
        EntityMetamodel.log.WarnFormat("Disabled ghost property fetching for {0} because it does not support lazy at the entity level", (object) this.name);
      if (this.hasUnwrapProxyForProperties)
        EntityMetamodel.log.Info((object) ("no-proxy property fetching available for: " + this.name));
      this.mutable = persistentClass.IsMutable;
      if (!persistentClass.IsAbstract.HasValue)
      {
        this.isAbstract = persistentClass.HasPocoRepresentation && ReflectHelper.IsAbstractClass(persistentClass.MappedClass);
      }
      else
      {
        this.isAbstract = persistentClass.IsAbstract.Value;
        if (!this.isAbstract && persistentClass.HasPocoRepresentation && ReflectHelper.IsAbstractClass(persistentClass.MappedClass))
          EntityMetamodel.log.Warn((object) ("entity [" + this.type.FullName + "] is abstract-class/interface explicitly mapped as non-abstract; be sure to supply entity-names"));
      }
      this.selectBeforeUpdate = persistentClass.SelectBeforeUpdate;
      this.dynamicUpdate = persistentClass.DynamicUpdate;
      this.dynamicInsert = persistentClass.DynamicInsert;
      this.polymorphic = persistentClass.IsPolymorphic;
      this.explicitPolymorphism = persistentClass.IsExplicitPolymorphism;
      this.inherited = persistentClass.IsInherited;
      this.superclass = this.inherited ? persistentClass.Superclass.EntityName : (string) null;
      this.superclassType = this.inherited ? persistentClass.Superclass.MappedClass : (System.Type) null;
      this.hasSubclasses = persistentClass.HasSubclasses;
      this.optimisticLockMode = persistentClass.OptimisticLockMode;
      if (this.optimisticLockMode > Versioning.OptimisticLock.Version && !this.dynamicUpdate)
        throw new MappingException("optimistic-lock setting requires dynamic-update=\"true\": " + this.type.FullName);
      this.hasCollections = flag4;
      this.hasMutableProperties = flag5;
      foreach (PersistentClass persistentClass1 in persistentClass.SubclassIterator)
        this.subclassEntityNames.Add(persistentClass1.EntityName);
      this.subclassEntityNames.Add(this.name);
      this.tuplizerMapping = new EntityEntityModeToTuplizerMapping(persistentClass, this);
    }

    public bool HasPocoRepresentation { get; private set; }

    private ValueInclusion DetermineInsertValueGenerationType(
      NHibernate.Mapping.Property mappingProperty,
      StandardProperty runtimeProperty)
    {
      if (runtimeProperty.IsInsertGenerated)
        return ValueInclusion.Full;
      return mappingProperty.Value is Component && this.HasPartialInsertComponentGeneration((Component) mappingProperty.Value) ? ValueInclusion.Partial : ValueInclusion.None;
    }

    private bool HasPartialInsertComponentGeneration(Component component)
    {
      foreach (NHibernate.Mapping.Property property in component.PropertyIterator)
      {
        if (property.Generation == PropertyGeneration.Always || property.Generation == PropertyGeneration.Insert || property.Value is Component && this.HasPartialInsertComponentGeneration((Component) property.Value))
          return true;
      }
      return false;
    }

    private ValueInclusion DetermineUpdateValueGenerationType(
      NHibernate.Mapping.Property mappingProperty,
      StandardProperty runtimeProperty)
    {
      if (runtimeProperty.IsUpdateGenerated)
        return ValueInclusion.Full;
      return mappingProperty.Value is Component && this.HasPartialUpdateComponentGeneration((Component) mappingProperty.Value) ? ValueInclusion.Partial : ValueInclusion.None;
    }

    private bool HasPartialUpdateComponentGeneration(Component component)
    {
      foreach (NHibernate.Mapping.Property property in component.PropertyIterator)
      {
        if (property.Generation == PropertyGeneration.Always || property.Value is Component && this.HasPartialUpdateComponentGeneration((Component) property.Value))
          return true;
      }
      return false;
    }

    private void MapPropertyToIndex(NHibernate.Mapping.Property prop, int i)
    {
      this.propertyIndexes[prop.Name] = new int?(i);
      if (!(prop.Value is Component component))
        return;
      foreach (NHibernate.Mapping.Property property in component.PropertyIterator)
        this.propertyIndexes[prop.Name + (object) '.' + property.Name] = new int?(i);
    }

    public ISet<string> SubclassEntityNames => (ISet<string>) this.subclassEntityNames;

    private bool IndicatesCollection(IType type)
    {
      if (type.IsCollectionType)
        return true;
      if (type.IsComponentType)
      {
        foreach (IType subtype in ((IAbstractComponentType) type).Subtypes)
        {
          if (this.IndicatesCollection(subtype))
            return true;
        }
      }
      return false;
    }

    public ISessionFactoryImplementor SessionFactory => this.sessionFactory;

    public System.Type Type => this.type;

    public System.Type RootType => this.rootType;

    public string RootTypeAssemblyQualifiedName => this.rootTypeAssemblyQualifiedName;

    public string Name => this.name;

    public string RootName => this.rootName;

    public EntityType EntityType => this.entityType;

    public IdentifierProperty IdentifierProperty => this.identifierProperty;

    public int PropertySpan => this.propertySpan;

    public int VersionPropertyIndex => this.versionPropertyIndex;

    public VersionProperty VersionProperty
    {
      get
      {
        return -66 == this.versionPropertyIndex ? (VersionProperty) null : (VersionProperty) this.properties[this.versionPropertyIndex];
      }
    }

    public StandardProperty[] Properties => this.properties;

    public int GetPropertyIndex(string propertyName)
    {
      return (this.GetPropertyIndexOrNull(propertyName) ?? throw new HibernateException("Unable to resolve property: " + propertyName)).Value;
    }

    public int? GetPropertyIndexOrNull(string propertyName)
    {
      int? nullable;
      return this.propertyIndexes.TryGetValue(propertyName, out nullable) ? nullable : new int?();
    }

    public bool HasCollections => this.hasCollections;

    public bool HasMutableProperties => this.hasMutableProperties;

    public bool HasLazyProperties => this.hasLazyProperties;

    public bool HasCascades => this.hasCascades;

    public bool IsMutable => this.mutable;

    public bool IsSelectBeforeUpdate => this.selectBeforeUpdate;

    public bool IsDynamicUpdate => this.dynamicUpdate;

    public bool IsDynamicInsert => this.dynamicInsert;

    public Versioning.OptimisticLock OptimisticLockMode => this.optimisticLockMode;

    public bool IsPolymorphic => this.polymorphic;

    public string Superclass => this.superclass;

    public System.Type SuperclassType => this.superclassType;

    public bool IsExplicitPolymorphism => this.explicitPolymorphism;

    public bool IsInherited => this.inherited;

    public bool HasSubclasses => this.hasSubclasses;

    public bool IsLazy
    {
      get => this.lazy;
      set => this.lazy = value;
    }

    public bool IsVersioned => this.versioned;

    public bool IsAbstract => this.isAbstract;

    public override string ToString()
    {
      return "EntityMetamodel(" + this.type.FullName + (object) ':' + ArrayHelper.ToString((object[]) this.properties) + (object) ')';
    }

    public string[] PropertyNames => this.propertyNames;

    public IType[] PropertyTypes => this.propertyTypes;

    public bool[] PropertyLaziness => this.propertyLaziness;

    public bool[] PropertyUpdateability => this.propertyUpdateability;

    public bool[] PropertyCheckability => this.propertyCheckability;

    public bool[] NonlazyPropertyUpdateability => this.nonlazyPropertyUpdateability;

    public bool[] PropertyInsertability => this.propertyInsertability;

    public bool[] PropertyNullability => this.propertyNullability;

    public bool[] PropertyVersionability => this.propertyVersionability;

    public CascadeStyle[] CascadeStyles => this.cascadeStyles;

    public ValueInclusion[] PropertyInsertGenerationInclusions => this.insertInclusions;

    public ValueInclusion[] PropertyUpdateGenerationInclusions => this.updateInclusions;

    public bool HasInsertGeneratedValues => this.hasInsertGeneratedValues;

    public bool HasUpdateGeneratedValues => this.hasUpdateGeneratedValues;

    public IEntityTuplizer GetTuplizer(EntityMode entityMode)
    {
      return (IEntityTuplizer) this.tuplizerMapping.GetTuplizer(entityMode);
    }

    public IEntityTuplizer GetTuplizerOrNull(EntityMode entityMode)
    {
      return (IEntityTuplizer) this.tuplizerMapping.GetTuplizerOrNull(entityMode);
    }

    public EntityMode? GuessEntityMode(object obj) => this.tuplizerMapping.GuessEntityMode(obj);

    public bool HasNaturalIdentifier => this.naturalIdPropertyNumbers != null;

    public bool HasUnwrapProxyForProperties => this.hasUnwrapProxyForProperties;

    public bool HasNonIdentifierPropertyNamedId => this.hasNonIdentifierPropertyNamedId;

    public int[] NaturalIdentifierProperties => this.naturalIdPropertyNumbers;
  }
}

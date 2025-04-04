// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.ClassBinder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Cfg.XmlHbmBinding
{
  public abstract class ClassBinder : Binder
  {
    protected readonly NHibernate.Dialect.Dialect dialect;

    protected ClassBinder(Mappings mappings, NHibernate.Dialect.Dialect dialect)
      : base(mappings)
    {
      this.dialect = dialect;
    }

    protected ClassBinder(ClassBinder parent)
      : base(parent.Mappings)
    {
      this.dialect = parent.dialect;
    }

    protected void BindClass(
      IEntityMapping classMapping,
      PersistentClass model,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      model.IsLazy = classMapping.UseLazy.HasValue ? classMapping.UseLazy.Value : this.mappings.DefaultLazy;
      model.EntityName = (classMapping.EntityName ?? Binder.ClassForNameChecked(classMapping.Name, this.mappings, "persistent class {0} not found").FullName) ?? throw new MappingException("Unable to determine entity name");
      this.BindPocoRepresentation(classMapping, model);
      this.BindXmlRepresentation(classMapping, model);
      this.BindMapRepresentation(classMapping, model);
      this.BindPersistentClassCommonValues(classMapping, model, inheritedMetas);
    }

    protected void BindUnionSubclasses(
      IEnumerable<HbmUnionSubclass> unionSubclasses,
      PersistentClass persistentClass,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      foreach (HbmUnionSubclass unionSubclass in unionSubclasses)
        new UnionSubclassBinder(this).HandleUnionSubclass(persistentClass, unionSubclass, inheritedMetas);
    }

    protected void BindJoinedSubclasses(
      IEnumerable<HbmJoinedSubclass> joinedSubclasses,
      PersistentClass persistentClass,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      foreach (HbmJoinedSubclass joinedSubclass in joinedSubclasses)
        new JoinedSubclassBinder(this).HandleJoinedSubclass(persistentClass, joinedSubclass, inheritedMetas);
    }

    protected void BindSubclasses(
      IEnumerable<HbmSubclass> subclasses,
      PersistentClass persistentClass,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      foreach (HbmSubclass subclass in subclasses)
        new SubclassBinder(this).HandleSubclass(persistentClass, subclass, inheritedMetas);
    }

    private void BindPersistentClassCommonValues(
      IEntityMapping classMapping,
      PersistentClass model,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      IEntityDiscriminableMapping discriminableMapping = classMapping as IEntityDiscriminableMapping;
      model.DiscriminatorValue = discriminableMapping == null || string.IsNullOrEmpty(discriminableMapping.DiscriminatorValue) ? model.EntityName : discriminableMapping.DiscriminatorValue;
      model.DynamicUpdate = classMapping.DynamicUpdate;
      model.DynamicInsert = classMapping.DynamicInsert;
      string className = model.MappedClass == null ? model.EntityName : model.MappedClass.AssemblyQualifiedName;
      this.mappings.AddImport(className, model.EntityName);
      if (this.mappings.IsAutoImport && model.EntityName.IndexOf('.') > 0)
        this.mappings.AddImport(className, StringHelper.Unqualify(model.EntityName));
      if (classMapping.BatchSize.HasValue)
        model.BatchSize = new int?(classMapping.BatchSize.Value);
      model.SelectBeforeUpdate = classMapping.SelectBeforeUpdate;
      model.MetaAttributes = Binder.GetMetas((IDecoratable) classMapping, inheritedMetas);
      if (!string.IsNullOrEmpty(classMapping.Persister))
        model.EntityPersisterClass = Binder.ClassForNameChecked(classMapping.Persister, this.mappings, "could not instantiate persister class: {0}");
      this.HandleCustomSQL((IEntitySqlsMapping) classMapping, (ISqlCustomizable) model);
      if (classMapping.SqlLoader != null)
        model.LoaderName = classMapping.SqlLoader.queryref;
      foreach (HbmSynchronize hbmSynchronize in classMapping.Synchronize)
        model.AddSynchronizedTable(hbmSynchronize.table);
      model.IsAbstract = classMapping.IsAbstract;
    }

    private void BindMapRepresentation(IEntityMapping classMapping, PersistentClass entity)
    {
      HbmTuplizer hbmTuplizer = ((IEnumerable<HbmTuplizer>) classMapping.Tuplizers).FirstOrDefault<HbmTuplizer>((Func<HbmTuplizer, bool>) (tp => tp.entitymode == HbmTuplizerEntitymode.DynamicMap));
      if (hbmTuplizer == null)
        return;
      string implClass = Binder.FullQualifiedClassName(hbmTuplizer.@class, this.mappings);
      entity.AddTuplizer(EntityMode.Map, implClass);
    }

    private void BindXmlRepresentation(IEntityMapping classMapping, PersistentClass entity)
    {
      entity.NodeName = string.IsNullOrEmpty(classMapping.Node) ? StringHelper.Unqualify(entity.EntityName) : classMapping.Node;
      HbmTuplizer hbmTuplizer = ((IEnumerable<HbmTuplizer>) classMapping.Tuplizers).FirstOrDefault<HbmTuplizer>((Func<HbmTuplizer, bool>) (tp => tp.entitymode == HbmTuplizerEntitymode.Xml));
      if (hbmTuplizer == null)
        return;
      string implClass = Binder.FullQualifiedClassName(hbmTuplizer.@class, this.mappings);
      entity.AddTuplizer(EntityMode.Xml, implClass);
    }

    private void BindPocoRepresentation(IEntityMapping classMapping, PersistentClass entity)
    {
      string assemblyQualifiedName = classMapping.Name == null ? (string) null : Binder.ClassForNameChecked(classMapping.Name, this.mappings, "persistent class {0} not found").AssemblyQualifiedName;
      entity.ClassName = assemblyQualifiedName;
      if (!string.IsNullOrEmpty(classMapping.Proxy))
      {
        entity.ProxyInterfaceName = Binder.ClassForNameChecked(classMapping.Proxy, this.mappings, "proxy class not found: {0}").AssemblyQualifiedName;
        entity.IsLazy = true;
      }
      else if (entity.IsLazy)
        entity.ProxyInterfaceName = assemblyQualifiedName;
      HbmTuplizer hbmTuplizer = ((IEnumerable<HbmTuplizer>) classMapping.Tuplizers).FirstOrDefault<HbmTuplizer>((Func<HbmTuplizer, bool>) (tp => tp.entitymode == HbmTuplizerEntitymode.Poco));
      if (hbmTuplizer == null)
        return;
      string implClass = Binder.FullQualifiedClassName(hbmTuplizer.@class, this.mappings);
      entity.AddTuplizer(EntityMode.Poco, implClass);
    }

    protected void BindJoins(
      IEnumerable<HbmJoin> joins,
      PersistentClass persistentClass,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      foreach (HbmJoin join1 in joins)
      {
        NHibernate.Mapping.Join join2 = new NHibernate.Mapping.Join()
        {
          PersistentClass = persistentClass
        };
        this.BindJoin(join1, join2, inheritedMetas);
        persistentClass.AddJoin(join2);
      }
    }

    private void BindJoin(
      HbmJoin joinMapping,
      NHibernate.Mapping.Join join,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      PersistentClass persistentClass = join.PersistentClass;
      string schema = joinMapping.schema ?? this.mappings.SchemaName;
      string catalog = joinMapping.catalog ?? this.mappings.CatalogName;
      string schemaAction = "all";
      string table1 = joinMapping.table;
      Table table2 = this.mappings.AddTable(schema, catalog, this.GetClassTableName(persistentClass, table1), joinMapping.Subselect, false, schemaAction);
      join.Table = table2;
      join.IsSequentialSelect = joinMapping.fetch == HbmJoinFetch.Select;
      join.IsInverse = joinMapping.inverse;
      join.IsOptional = joinMapping.optional;
      Binder.log.InfoFormat("Mapping class join: {0} -> {1}", (object) persistentClass.EntityName, (object) join.Table.Name);
      SimpleValue simpleValue = (SimpleValue) new DependantValue(table2, persistentClass.Identifier);
      simpleValue.ForeignKeyName = joinMapping.key.foreignkey;
      join.Key = (IKeyValue) simpleValue;
      simpleValue.IsCascadeDeleteEnabled = joinMapping.key.ondelete == HbmOndelete.Cascade;
      new ValuePropertyBinder(simpleValue, this.Mappings).BindSimpleValue(joinMapping.key, persistentClass.EntityName, false);
      join.CreatePrimaryKey(this.dialect);
      join.CreateForeignKey();
      new PropertiesBinder(this.Mappings, persistentClass, this.dialect).Bind(joinMapping.Properties, join.Table, inheritedMetas, (Action<Property>) (p => { }), new Action<Property>(join.AddProperty));
      this.HandleCustomSQL((IEntitySqlsMapping) joinMapping, (ISqlCustomizable) join);
    }

    private void HandleCustomSQL(IEntitySqlsMapping sqlsMapping, ISqlCustomizable model)
    {
      HbmCustomSQL sqlInsert = sqlsMapping.SqlInsert;
      if (sqlInsert != null)
      {
        bool callable = sqlInsert.callableSpecified && sqlInsert.callable;
        model.SetCustomSQLInsert(sqlInsert.Text.LinesToString(), callable, ClassBinder.GetResultCheckStyle(sqlInsert));
      }
      HbmCustomSQL sqlDelete = sqlsMapping.SqlDelete;
      if (sqlDelete != null)
      {
        bool callable = sqlDelete.callableSpecified && sqlDelete.callable;
        model.SetCustomSQLDelete(sqlDelete.Text.LinesToString(), callable, ClassBinder.GetResultCheckStyle(sqlDelete));
      }
      HbmCustomSQL sqlUpdate = sqlsMapping.SqlUpdate;
      if (sqlUpdate == null)
        return;
      bool callable1 = sqlUpdate.callableSpecified && sqlUpdate.callable;
      model.SetCustomSQLUpdate(sqlUpdate.Text.LinesToString(), callable1, ClassBinder.GetResultCheckStyle(sqlUpdate));
    }

    protected PersistentClass GetSuperclass(string extendsName)
    {
      if (string.IsNullOrEmpty(extendsName))
        throw new MappingException("'extends' attribute is not found or is empty.");
      return (this.mappings.GetClass(extendsName) ?? this.mappings.GetClass(Binder.FullClassName(extendsName, this.mappings))) ?? throw new MappingException("Cannot extend unmapped class: " + extendsName);
    }

    protected string GetClassTableName(PersistentClass model, string mappedTableName)
    {
      return !string.IsNullOrEmpty(mappedTableName) ? this.mappings.NamingStrategy.TableName(mappedTableName) : this.mappings.NamingStrategy.ClassToTableName(model.EntityName);
    }

    protected void BindComponent(
      IComponentMapping componentMapping,
      Component model,
      System.Type reflectedClass,
      string className,
      string path,
      bool isNullable,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      model.RoleName = path;
      inheritedMetas = Binder.GetMetas(componentMapping as IDecoratable, inheritedMetas);
      model.MetaAttributes = inheritedMetas;
      string str1 = componentMapping.Class;
      if (!string.IsNullOrEmpty(str1))
      {
        model.ComponentClass = Binder.ClassForNameChecked(str1, this.mappings, "component class not found: {0}");
        model.ComponentClassName = Binder.FullQualifiedClassName(str1, this.mappings);
        model.IsEmbedded = false;
      }
      else if (componentMapping is HbmDynamicComponent)
      {
        model.IsEmbedded = false;
        model.IsDynamic = true;
      }
      else if (reflectedClass != null)
      {
        model.ComponentClass = reflectedClass;
        model.IsEmbedded = false;
      }
      else
      {
        model.IsEmbedded = true;
        if (model.Owner.HasPocoRepresentation)
          model.ComponentClass = model.Owner.MappedClass;
        else
          model.IsDynamic = true;
      }
      string str2 = !string.IsNullOrEmpty(componentMapping.EmbeddedNode) ? componentMapping.EmbeddedNode : (!string.IsNullOrEmpty(componentMapping.Name) ? componentMapping.Name : model.Owner.NodeName);
      model.NodeName = str2;
      if (componentMapping.Parent != null && !string.IsNullOrEmpty(componentMapping.Parent.name))
        model.ParentProperty = new Property()
        {
          Name = componentMapping.Parent.name,
          PropertyAccessorName = componentMapping.Parent.access ?? this.mappings.DefaultAccess
        };
      new PropertiesBinder(this.Mappings, model, className, path, isNullable, this.Mappings.Dialect).Bind(componentMapping.Properties, model.Table, inheritedMetas, (Action<Property>) (p => { }), new Action<Property>(model.AddProperty));
    }

    protected void BindForeignKey(string foreignKey, SimpleValue value)
    {
      if (string.IsNullOrEmpty(foreignKey))
        return;
      value.ForeignKeyName = foreignKey;
    }

    protected void BindAny(HbmAny node, Any model, bool isNullable)
    {
      model.IdentifierTypeName = node.idtype;
      new TypeBinder((SimpleValue) model, this.Mappings).Bind(node.idtype);
      this.BindAnyMeta((IAnyMapping) node, model);
      new ColumnsBinder((SimpleValue) model, this.Mappings).Bind(node.Columns, isNullable, (Func<HbmColumn>) null);
    }

    protected void BindAnyMeta(IAnyMapping anyMapping, Any model)
    {
      if (string.IsNullOrEmpty(anyMapping.MetaType))
        return;
      model.MetaType = anyMapping.MetaType;
      ICollection<HbmMetaValue> metaValues = anyMapping.MetaValues;
      if (metaValues.Count == 0)
        return;
      IDictionary<object, string> dictionary = (IDictionary<object, string>) new Dictionary<object, string>();
      IType type = TypeFactory.HeuristicType(model.MetaType);
      foreach (HbmMetaValue hbmMetaValue in (IEnumerable<HbmMetaValue>) metaValues)
      {
        try
        {
          object key = ((IIdentifierType) type).StringToObject(hbmMetaValue.value);
          string className = Binder.GetClassName(hbmMetaValue.@class, this.mappings);
          dictionary[key] = className;
        }
        catch (InvalidCastException ex)
        {
          throw new MappingException("meta-type was not an IDiscriminatorType: " + type.Name);
        }
        catch (HibernateException ex)
        {
          throw new MappingException("could not interpret meta-value", (Exception) ex);
        }
        catch (TypeLoadException ex)
        {
          throw new MappingException("meta-value class not found", (Exception) ex);
        }
      }
      model.MetaValues = dictionary.Count > 0 ? dictionary : (IDictionary<object, string>) null;
    }

    protected void BindOneToOne(HbmOneToOne oneToOneMapping, OneToOne model)
    {
      model.IsConstrained = oneToOneMapping.constrained;
      model.ForeignKeyType = oneToOneMapping.constrained ? ForeignKeyDirection.ForeignKeyFromParent : ForeignKeyDirection.ForeignKeyToParent;
      this.InitOuterJoinFetchSetting(oneToOneMapping, model);
      ClassBinder.InitLaziness(oneToOneMapping.Lazy, (ToOne) model, true);
      this.BindForeignKey(oneToOneMapping.foreignkey, (SimpleValue) model);
      model.ReferencedPropertyName = oneToOneMapping.propertyref;
      model.ReferencedEntityName = ClassBinder.GetEntityName((IRelationship) oneToOneMapping, this.mappings);
      model.PropertyName = oneToOneMapping.Name;
    }

    protected static ExecuteUpdateResultCheckStyle GetResultCheckStyle(HbmCustomSQL customSQL)
    {
      if (customSQL != null)
      {
        if (!customSQL.checkSpecified)
          return ExecuteUpdateResultCheckStyle.Count;
        switch (customSQL.check)
        {
          case HbmCustomSQLCheck.None:
            return ExecuteUpdateResultCheckStyle.None;
          case HbmCustomSQLCheck.Rowcount:
            return ExecuteUpdateResultCheckStyle.Count;
          case HbmCustomSQLCheck.Param:
            return (ExecuteUpdateResultCheckStyle) null;
        }
      }
      return (ExecuteUpdateResultCheckStyle) null;
    }

    protected static void InitLaziness(
      HbmRestrictedLaziness? restrictedLaziness,
      ToOne fetchable,
      bool defaultLazy)
    {
      int num;
      if (restrictedLaziness.HasValue)
      {
        HbmRestrictedLaziness? nullable = restrictedLaziness;
        num = nullable.GetValueOrDefault() != HbmRestrictedLaziness.Proxy ? 0 : (nullable.HasValue ? 1 : 0);
      }
      else
        num = !defaultLazy ? 0 : (fetchable.IsLazy ? 1 : 0);
      bool flag = num != 0;
      fetchable.IsLazy = flag;
    }

    protected static void InitLaziness(HbmLaziness? laziness, ToOne fetchable, bool defaultLazy)
    {
      int num1;
      if (laziness.HasValue)
      {
        HbmLaziness? nullable1 = laziness;
        if ((nullable1.GetValueOrDefault() != HbmLaziness.Proxy ? 0 : (nullable1.HasValue ? 1 : 0)) == 0)
        {
          HbmLaziness? nullable2 = laziness;
          num1 = nullable2.GetValueOrDefault() != HbmLaziness.NoProxy ? 0 : (nullable2.HasValue ? 1 : 0);
        }
        else
          num1 = 1;
      }
      else
        num1 = !defaultLazy ? 0 : (fetchable.IsLazy ? 1 : 0);
      bool flag = num1 != 0;
      fetchable.IsLazy = flag;
      ToOne toOne = fetchable;
      HbmLaziness? nullable = laziness;
      int num2 = nullable.GetValueOrDefault() != HbmLaziness.NoProxy ? 0 : (nullable.HasValue ? 1 : 0);
      toOne.UnwrapProxy = num2 != 0;
    }

    protected void InitOuterJoinFetchSetting(HbmManyToMany manyToMany, IFetchable model)
    {
      bool flag = true;
      FetchMode fetchMode;
      if (!manyToMany.fetchSpecified)
      {
        if (!manyToMany.outerjoinSpecified)
        {
          flag = false;
          fetchMode = FetchMode.Join;
        }
        else
          fetchMode = this.GetFetchStyle(manyToMany.outerjoin);
      }
      else
        fetchMode = this.GetFetchStyle(manyToMany.fetch);
      model.FetchMode = fetchMode;
      model.IsLazy = flag;
    }

    protected FetchMode GetFetchStyle(HbmFetchMode fetchModeMapping)
    {
      switch (fetchModeMapping)
      {
        case HbmFetchMode.Select:
          return FetchMode.Select;
        case HbmFetchMode.Join:
          return FetchMode.Join;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    protected FetchMode GetFetchStyle(HbmOuterJoinStrategy outerJoinStrategyMapping)
    {
      switch (outerJoinStrategyMapping)
      {
        case HbmOuterJoinStrategy.Auto:
          return FetchMode.Default;
        case HbmOuterJoinStrategy.True:
          return FetchMode.Join;
        case HbmOuterJoinStrategy.False:
          return FetchMode.Select;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    protected void InitOuterJoinFetchSetting(HbmOneToOne oneToOne, OneToOne model)
    {
      bool flag = true;
      FetchMode fetchMode;
      if (!oneToOne.fetchSpecified)
      {
        if (!oneToOne.outerjoinSpecified)
        {
          flag = model.IsConstrained;
          fetchMode = flag ? FetchMode.Default : FetchMode.Join;
        }
        else
          fetchMode = this.GetFetchStyle(oneToOne.outerjoin);
      }
      else
        fetchMode = this.GetFetchStyle(oneToOne.fetch);
      model.FetchMode = fetchMode;
      model.IsLazy = flag;
    }

    protected static string GetEntityName(IRelationship relationship, Mappings mappings)
    {
      string entityName = string.IsNullOrEmpty(relationship.EntityName) ? (string) null : relationship.EntityName;
      string className = string.IsNullOrEmpty(relationship.Class) ? (string) null : relationship.Class;
      return entityName ?? (className == null ? (string) null : StringHelper.GetFullClassname(Binder.FullQualifiedClassName(className, mappings)));
    }
  }
}

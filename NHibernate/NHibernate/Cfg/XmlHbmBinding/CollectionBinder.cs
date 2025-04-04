// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.CollectionBinder
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Cfg.XmlHbmBinding
{
  public class CollectionBinder(Mappings mappings, NHibernate.Dialect.Dialect dialect) : ClassBinder(mappings, dialect)
  {
    public Collection Create(
      ICollectionPropertiesMapping collectionMapping,
      string className,
      string propertyFullPath,
      PersistentClass owner,
      System.Type containingType,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      System.Type type = collectionMapping.GetType();
      if (type == typeof (HbmBag))
        return this.CreateBag((HbmBag) collectionMapping, className, propertyFullPath, owner, containingType, inheritedMetas);
      if (type == typeof (HbmSet))
        return this.CreateSet((HbmSet) collectionMapping, className, propertyFullPath, owner, containingType, inheritedMetas);
      if (type == typeof (HbmList))
        return this.CreateList((HbmList) collectionMapping, className, propertyFullPath, owner, containingType, inheritedMetas);
      if (type == typeof (HbmMap))
        return this.CreateMap((HbmMap) collectionMapping, className, propertyFullPath, owner, containingType, inheritedMetas);
      if (type == typeof (HbmIdbag))
        return this.CreateIdentifierBag((HbmIdbag) collectionMapping, className, propertyFullPath, owner, containingType, inheritedMetas);
      if (type == typeof (HbmArray))
        return this.CreateArray((HbmArray) collectionMapping, className, propertyFullPath, owner, containingType, inheritedMetas);
      if (type == typeof (HbmPrimitiveArray))
        return this.CreatePrimitiveArray((HbmPrimitiveArray) collectionMapping, className, propertyFullPath, owner, containingType, inheritedMetas);
      throw new MappingException("Not supported collection mapping element:" + (object) type);
    }

    private Collection CreateMap(
      HbmMap mapMapping,
      string prefix,
      string path,
      PersistentClass owner,
      System.Type containingType,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      Map model = new Map(owner);
      this.BindCollection((ICollectionPropertiesMapping) mapMapping, (Collection) model, prefix, path, containingType, inheritedMetas);
      this.AddMapSecondPass(mapMapping, model, inheritedMetas);
      return (Collection) model;
    }

    private Collection CreateSet(
      HbmSet setMapping,
      string prefix,
      string path,
      PersistentClass owner,
      System.Type containingType,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      Set model = new Set(owner);
      this.BindCollection((ICollectionPropertiesMapping) setMapping, (Collection) model, prefix, path, containingType, inheritedMetas);
      this.AddSetSecondPass(setMapping, model, inheritedMetas);
      return (Collection) model;
    }

    private Collection CreateList(
      HbmList listMapping,
      string prefix,
      string path,
      PersistentClass owner,
      System.Type containingType,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      List model = new List(owner);
      this.BindCollection((ICollectionPropertiesMapping) listMapping, (Collection) model, prefix, path, containingType, inheritedMetas);
      this.AddListSecondPass(listMapping, model, inheritedMetas);
      return (Collection) model;
    }

    private Collection CreateBag(
      HbmBag bagMapping,
      string prefix,
      string path,
      PersistentClass owner,
      System.Type containingType,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      Bag model = new Bag(owner);
      this.BindCollection((ICollectionPropertiesMapping) bagMapping, (Collection) model, prefix, path, containingType, inheritedMetas);
      this.AddCollectionSecondPass((ICollectionPropertiesMapping) bagMapping, (Collection) model, inheritedMetas);
      return (Collection) model;
    }

    private Collection CreateIdentifierBag(
      HbmIdbag idbagMapping,
      string prefix,
      string path,
      PersistentClass owner,
      System.Type containingType,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      IdentifierBag model = new IdentifierBag(owner);
      this.BindCollection((ICollectionPropertiesMapping) idbagMapping, (Collection) model, prefix, path, containingType, inheritedMetas);
      this.AddIdentifierCollectionSecondPass(idbagMapping, (IdentifierCollection) model, inheritedMetas);
      return (Collection) model;
    }

    private Collection CreateArray(
      HbmArray arrayMapping,
      string prefix,
      string path,
      PersistentClass owner,
      System.Type containingType,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      NHibernate.Mapping.Array model = new NHibernate.Mapping.Array(owner);
      this.BindArray(arrayMapping, model, prefix, path, containingType, inheritedMetas);
      this.AddArraySecondPass(arrayMapping, model, inheritedMetas);
      return (Collection) model;
    }

    private Collection CreatePrimitiveArray(
      HbmPrimitiveArray primitiveArrayMapping,
      string prefix,
      string path,
      PersistentClass owner,
      System.Type containingType,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      PrimitiveArray model = new PrimitiveArray(owner);
      this.BindPrimitiveArray(primitiveArrayMapping, model, prefix, path, containingType, inheritedMetas);
      this.AddPrimitiveArraySecondPass(primitiveArrayMapping, (NHibernate.Mapping.Array) model, inheritedMetas);
      return (Collection) model;
    }

    private void BindPrimitiveArray(
      HbmPrimitiveArray arrayMapping,
      PrimitiveArray model,
      string prefix,
      string path,
      System.Type containingType,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      this.BindCollection((ICollectionPropertiesMapping) arrayMapping, (Collection) model, prefix, path, containingType, inheritedMetas);
      if (!(arrayMapping.ElementRelationship is HbmElement elementRelationship))
        return;
      string name = (elementRelationship.Type ?? throw new MappingException("type for <element> was not defined")).name;
      model.ElementClassName = (TypeFactory.HeuristicType(name, (IDictionary<string, string>) null) ?? throw new MappingException("could not interpret type: " + name)).ReturnedClass.AssemblyQualifiedName;
    }

    private void BindCollection(
      ICollectionPropertiesMapping collectionMapping,
      Collection model,
      string className,
      string path,
      System.Type containingType,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      model.Role = path;
      model.IsInverse = collectionMapping.Inverse;
      model.IsMutable = collectionMapping.Mutable;
      model.IsOptimisticLocked = collectionMapping.OptimisticLock;
      model.OrderBy = collectionMapping.OrderBy;
      model.Where = collectionMapping.Where;
      if (collectionMapping.BatchSize.HasValue)
        model.BatchSize = collectionMapping.BatchSize.Value;
      if (!string.IsNullOrEmpty(collectionMapping.PersisterQualifiedName))
        model.CollectionPersisterClass = Binder.ClassForNameChecked(collectionMapping.PersisterQualifiedName, this.mappings, "could not instantiate collection persister class: {0}");
      if (!string.IsNullOrEmpty(collectionMapping.CollectionType))
      {
        TypeDef typeDef = this.mappings.GetTypeDef(collectionMapping.CollectionType);
        if (typeDef != null)
        {
          model.TypeName = typeDef.TypeClass;
          model.TypeParameters = typeDef.Parameters;
        }
        else
          model.TypeName = Binder.FullQualifiedClassName(collectionMapping.CollectionType, this.mappings);
      }
      this.InitOuterJoinFetchSetting(collectionMapping, model);
      this.InitLaziness(collectionMapping, model);
      if (collectionMapping.ElementRelationship is HbmOneToMany elementRelationship)
      {
        OneToMany model1 = new OneToMany(model.Owner);
        model.Element = (IValue) model1;
        this.BindOneToMany(elementRelationship, model1);
      }
      else
      {
        string name = !string.IsNullOrEmpty(collectionMapping.Table) ? this.mappings.NamingStrategy.TableName(collectionMapping.Table) : this.mappings.NamingStrategy.PropertyToTableName(className, path);
        string schema = string.IsNullOrEmpty(collectionMapping.Schema) ? this.mappings.SchemaName : collectionMapping.Schema;
        string catalog = string.IsNullOrEmpty(collectionMapping.Catalog) ? this.mappings.CatalogName : collectionMapping.Catalog;
        model.CollectionTable = this.mappings.AddTable(schema, catalog, name, collectionMapping.Subselect, false, "all");
        Binder.log.InfoFormat("Mapping collection: {0} -> {1}", (object) model.Role, (object) model.CollectionTable.Name);
      }
      string sort = collectionMapping.Sort;
      if (string.IsNullOrEmpty(sort) || sort.Equals("unsorted"))
      {
        model.IsSorted = false;
      }
      else
      {
        model.IsSorted = true;
        if (!sort.Equals("natural"))
        {
          string str = Binder.FullQualifiedClassName(sort, this.mappings);
          model.ComparerClassName = str;
        }
      }
      string cascade = collectionMapping.Cascade;
      if (!string.IsNullOrEmpty(cascade) && cascade.IndexOf("delete-orphan") >= 0)
        model.HasOrphanDelete = true;
      bool? nullable = collectionMapping.Generic;
      System.Type type = (System.Type) null;
      if (!nullable.HasValue && containingType != null)
      {
        type = this.GetPropertyType(containingType, collectionMapping.Name, collectionMapping.Access);
        nullable = new bool?(type.IsGenericType);
      }
      model.IsGeneric = ((int) nullable ?? 0) != 0;
      if (model.IsGeneric)
      {
        if (type == null)
          type = this.GetPropertyType(containingType, collectionMapping.Name, collectionMapping.Access);
        System.Type[] genericArguments = type.GetGenericArguments();
        model.GenericArguments = genericArguments;
      }
      this.HandleCustomSQL((ICollectionSqlsMapping) collectionMapping, model);
      if (collectionMapping.SqlLoader != null)
        model.LoaderName = collectionMapping.SqlLoader.queryref;
      new FiltersBinder((IFilterable) model, this.Mappings).Bind(collectionMapping.Filters);
      HbmKey key = collectionMapping.Key;
      if (key == null)
        return;
      model.ReferencedPropertyName = key.propertyref;
    }

    private void InitLaziness(ICollectionPropertiesMapping collectionMapping, Collection fetchable)
    {
      HbmCollectionLazy? lazy = collectionMapping.Lazy;
      if (!lazy.HasValue)
      {
        fetchable.IsLazy = this.mappings.DefaultLazy;
        fetchable.ExtraLazy = false;
      }
      else
      {
        switch (lazy.Value)
        {
          case HbmCollectionLazy.True:
            fetchable.IsLazy = true;
            break;
          case HbmCollectionLazy.False:
            fetchable.IsLazy = false;
            fetchable.ExtraLazy = false;
            break;
          case HbmCollectionLazy.Extra:
            fetchable.IsLazy = true;
            fetchable.ExtraLazy = true;
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    private void InitOuterJoinFetchSetting(
      ICollectionPropertiesMapping collectionMapping,
      Collection model)
    {
      FetchMode fetchMode = FetchMode.Default;
      if (!collectionMapping.FetchMode.HasValue)
      {
        if (collectionMapping.OuterJoin.HasValue)
        {
          switch (collectionMapping.OuterJoin.Value)
          {
            case HbmOuterJoinStrategy.Auto:
              fetchMode = FetchMode.Default;
              break;
            case HbmOuterJoinStrategy.True:
              fetchMode = FetchMode.Join;
              break;
            case HbmOuterJoinStrategy.False:
              fetchMode = FetchMode.Select;
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }
      }
      else
      {
        switch (collectionMapping.FetchMode.Value)
        {
          case HbmCollectionFetchMode.Select:
            fetchMode = FetchMode.Select;
            break;
          case HbmCollectionFetchMode.Join:
            fetchMode = FetchMode.Join;
            break;
          case HbmCollectionFetchMode.Subselect:
            fetchMode = FetchMode.Select;
            model.IsSubselectLoadable = true;
            model.Owner.HasSubselectLoadableCollections = true;
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      model.FetchMode = fetchMode;
    }

    private void BindArray(
      HbmArray arrayMapping,
      NHibernate.Mapping.Array model,
      string prefix,
      string path,
      System.Type containingType,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      this.BindCollection((ICollectionPropertiesMapping) arrayMapping, (Collection) model, prefix, path, containingType, inheritedMetas);
      string elementclass = arrayMapping.elementclass;
      if (elementclass != null)
        model.ElementClassName = Binder.GetQualifiedClassName(elementclass, this.mappings);
      else if (arrayMapping.ElementRelationship is HbmElement elementRelationship3)
      {
        string name = (elementRelationship3.Type ?? throw new MappingException("type for <element> was not defined")).name;
        model.ElementClassName = (TypeFactory.HeuristicType(name, (IDictionary<string, string>) null) ?? throw new MappingException("could not interpret type: " + name)).ReturnedClass.AssemblyQualifiedName;
      }
      else if (arrayMapping.ElementRelationship is HbmOneToMany elementRelationship2)
        model.ElementClassName = Binder.GetQualifiedClassName(elementRelationship2.@class, this.mappings);
      else if (arrayMapping.ElementRelationship is HbmManyToMany elementRelationship1)
      {
        model.ElementClassName = Binder.GetQualifiedClassName(elementRelationship1.@class, this.mappings);
      }
      else
      {
        if (!(arrayMapping.ElementRelationship is HbmCompositeElement elementRelationship))
          return;
        model.ElementClassName = Binder.GetQualifiedClassName(elementRelationship.@class, this.mappings);
      }
    }

    private void AddListSecondPass(
      HbmList listMapping,
      List model,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      this.mappings.AddSecondPass((SecondPassCommand) (persistentClasses =>
      {
        CollectionBinder.PreCollectionSecondPass((Collection) model);
        this.BindListSecondPass(listMapping, model, persistentClasses, inheritedMetas);
        CollectionBinder.PostCollectionSecondPass((Collection) model);
      }));
    }

    private void AddArraySecondPass(
      HbmArray arrayMapping,
      NHibernate.Mapping.Array model,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      this.mappings.AddSecondPass((SecondPassCommand) (persistentClasses =>
      {
        CollectionBinder.PreCollectionSecondPass((Collection) model);
        this.BindArraySecondPass(arrayMapping, (List) model, persistentClasses, inheritedMetas);
        CollectionBinder.PostCollectionSecondPass((Collection) model);
      }));
    }

    private void AddPrimitiveArraySecondPass(
      HbmPrimitiveArray primitiveArrayMapping,
      NHibernate.Mapping.Array model,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      this.mappings.AddSecondPass((SecondPassCommand) (persistentClasses =>
      {
        CollectionBinder.PreCollectionSecondPass((Collection) model);
        this.BindPrimitiveArraySecondPass(primitiveArrayMapping, (List) model, persistentClasses, inheritedMetas);
        CollectionBinder.PostCollectionSecondPass((Collection) model);
      }));
    }

    private void AddMapSecondPass(
      HbmMap mapMapping,
      Map model,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      this.mappings.AddSecondPass((SecondPassCommand) (persistentClasses =>
      {
        CollectionBinder.PreCollectionSecondPass((Collection) model);
        this.BindMapSecondPass(mapMapping, model, persistentClasses, inheritedMetas);
        CollectionBinder.PostCollectionSecondPass((Collection) model);
      }));
    }

    private void AddSetSecondPass(
      HbmSet setMapping,
      Set model,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      this.mappings.AddSecondPass((SecondPassCommand) (persistentClasses =>
      {
        CollectionBinder.PreCollectionSecondPass((Collection) model);
        this.BindSetSecondPass(setMapping, model, persistentClasses, inheritedMetas);
        CollectionBinder.PostCollectionSecondPass((Collection) model);
      }));
    }

    private void AddIdentifierCollectionSecondPass(
      HbmIdbag idbagMapping,
      IdentifierCollection model,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      this.mappings.AddSecondPass((SecondPassCommand) (persistentClasses =>
      {
        CollectionBinder.PreCollectionSecondPass((Collection) model);
        this.BindIdentifierCollectionSecondPass(idbagMapping, model, persistentClasses, inheritedMetas);
        CollectionBinder.PostCollectionSecondPass((Collection) model);
      }));
    }

    private void AddCollectionSecondPass(
      ICollectionPropertiesMapping collectionMapping,
      Collection model,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      this.mappings.AddSecondPass((SecondPassCommand) (persistentClasses =>
      {
        CollectionBinder.PreCollectionSecondPass(model);
        this.BindCollectionSecondPass(collectionMapping, model, persistentClasses, inheritedMetas);
        CollectionBinder.PostCollectionSecondPass(model);
      }));
    }

    private void HandleCustomSQL(ICollectionSqlsMapping collection, Collection model)
    {
      HbmCustomSQL sqlInsert = collection.SqlInsert;
      if (sqlInsert != null)
      {
        bool callable = sqlInsert.callableSpecified && sqlInsert.callable;
        model.SetCustomSQLInsert(sqlInsert.Text.LinesToString(), callable, ClassBinder.GetResultCheckStyle(sqlInsert));
      }
      HbmCustomSQL sqlDelete = collection.SqlDelete;
      if (sqlDelete != null)
      {
        bool callable = sqlDelete.callableSpecified && sqlDelete.callable;
        model.SetCustomSQLDelete(sqlDelete.Text.LinesToString(), callable, ClassBinder.GetResultCheckStyle(sqlDelete));
      }
      HbmCustomSQL sqlUpdate = collection.SqlUpdate;
      if (sqlUpdate != null)
      {
        bool callable = sqlUpdate.callableSpecified && sqlUpdate.callable;
        model.SetCustomSQLUpdate(sqlUpdate.Text.LinesToString(), callable, ClassBinder.GetResultCheckStyle(sqlUpdate));
      }
      HbmCustomSQL sqlDeleteAll = collection.SqlDeleteAll;
      if (sqlDeleteAll == null)
        return;
      bool callable1 = sqlDeleteAll.callableSpecified && sqlDeleteAll.callable;
      model.SetCustomSQLDeleteAll(sqlDeleteAll.Text.LinesToString(), callable1, ClassBinder.GetResultCheckStyle(sqlDeleteAll));
    }

    private static void PreCollectionSecondPass(Collection collection)
    {
      if (!Binder.log.IsDebugEnabled)
        return;
      Binder.log.Debug((object) ("Second pass for collection: " + collection.Role));
    }

    private static void PostCollectionSecondPass(Collection collection)
    {
      collection.CreateAllKeys();
      if (!Binder.log.IsDebugEnabled)
        return;
      string str = "Mapped collection key: " + string.Join(",", collection.Key.ColumnIterator.Select<ISelectable, string>((Func<ISelectable, string>) (c => c.Text)).ToArray<string>());
      if (collection.IsIndexed)
        str = str + ", index: " + string.Join(",", ((IndexedCollection) collection).Index.ColumnIterator.Select<ISelectable, string>((Func<ISelectable, string>) (c => c.Text)).ToArray<string>());
      string message = !collection.IsOneToMany ? str + ", element: " + string.Join(",", collection.Element.ColumnIterator.Select<ISelectable, string>((Func<ISelectable, string>) (c => c.Text)).ToArray<string>()) + ", type: " + collection.Element.Type.Name : str + ", one-to-many: " + collection.Element.Type.Name;
      Binder.log.Debug((object) message);
    }

    private void BindListSecondPass(
      HbmList listMapping,
      List model,
      IDictionary<string, PersistentClass> persistentClasses,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      this.BindCollectionSecondPass((ICollectionPropertiesMapping) listMapping, (Collection) model, persistentClasses, inheritedMetas);
      this.BindCollectionIndex((IIndexedCollectionMapping) listMapping, (IndexedCollection) model);
      if (listMapping.ListIndex != null && !string.IsNullOrEmpty(listMapping.ListIndex.@base))
        model.BaseIndex = Convert.ToInt32(listMapping.ListIndex.@base);
      if (!model.IsOneToMany || model.Key.IsNullable || model.IsInverse)
        return;
      PersistentClass persistentClass = this.mappings.GetClass(((OneToMany) model.Element).ReferencedEntityName);
      IndexBackref p = new IndexBackref();
      p.Name = '_'.ToString() + model.OwnerEntityName + "." + listMapping.Name + "IndexBackref";
      p.IsUpdateable = false;
      p.IsSelectable = false;
      p.CollectionRole = model.Role;
      p.EntityName = model.Owner.EntityName;
      p.Value = (IValue) model.Index;
      persistentClass.AddProperty((Property) p);
    }

    private void BindArraySecondPass(
      HbmArray arrayMapping,
      List model,
      IDictionary<string, PersistentClass> persistentClasses,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      this.BindCollectionSecondPass((ICollectionPropertiesMapping) arrayMapping, (Collection) model, persistentClasses, inheritedMetas);
      this.BindCollectionIndex((IIndexedCollectionMapping) arrayMapping, (IndexedCollection) model);
      if (arrayMapping.ListIndex == null || string.IsNullOrEmpty(arrayMapping.ListIndex.@base))
        return;
      model.BaseIndex = Convert.ToInt32(arrayMapping.ListIndex.@base);
    }

    private void BindPrimitiveArraySecondPass(
      HbmPrimitiveArray primitiveArrayMapping,
      List model,
      IDictionary<string, PersistentClass> persistentClasses,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      this.BindCollectionSecondPass((ICollectionPropertiesMapping) primitiveArrayMapping, (Collection) model, persistentClasses, inheritedMetas);
      this.BindCollectionIndex((IIndexedCollectionMapping) primitiveArrayMapping, (IndexedCollection) model);
      if (primitiveArrayMapping.ListIndex == null || string.IsNullOrEmpty(primitiveArrayMapping.ListIndex.@base))
        return;
      model.BaseIndex = Convert.ToInt32(primitiveArrayMapping.ListIndex.@base);
    }

    private void BindCollectionIndex(IIndexedCollectionMapping listMapping, IndexedCollection model)
    {
      SimpleValue simpleValue = (SimpleValue) null;
      if (listMapping.ListIndex != null)
      {
        simpleValue = new SimpleValue(model.CollectionTable);
        new ValuePropertyBinder(simpleValue, this.Mappings).BindSimpleValue(listMapping.ListIndex, "idx", model.IsOneToMany);
      }
      else if (listMapping.Index != null)
      {
        simpleValue = new SimpleValue(model.CollectionTable);
        listMapping.Index.type = NHibernateUtil.Int32.Name;
        new ValuePropertyBinder(simpleValue, this.Mappings).BindSimpleValue(listMapping.Index, "idx", model.IsOneToMany);
      }
      if (simpleValue == null)
        return;
      if (simpleValue.ColumnSpan > 1)
        Binder.log.Error((object) "This shouldn't happen, check BindIntegerValue");
      model.Index = simpleValue;
    }

    private void BindOneToMany(HbmOneToMany oneToManyMapping, OneToMany model)
    {
      model.ReferencedEntityName = ClassBinder.GetEntityName((IRelationship) oneToManyMapping, this.mappings);
      model.IsIgnoreNotFound = oneToManyMapping.NotFoundMode == HbmNotFoundMode.Ignore;
    }

    private void BindIdentifierCollectionSecondPass(
      HbmIdbag idbagMapping,
      IdentifierCollection model,
      IDictionary<string, PersistentClass> persitentClasses,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      this.BindCollectionSecondPass((ICollectionPropertiesMapping) idbagMapping, (Collection) model, persitentClasses, inheritedMetas);
      SimpleValue simpleValue = new SimpleValue(model.CollectionTable);
      new ValuePropertyBinder(simpleValue, this.Mappings).BindSimpleValue(idbagMapping.collectionid, "id");
      model.Identifier = (IKeyValue) simpleValue;
      new IdGeneratorBinder(this.Mappings).BindGenerator(simpleValue, idbagMapping.collectionid.generator);
      simpleValue.Table.SetIdentifierValue(simpleValue);
    }

    private void BindSetSecondPass(
      HbmSet setMapping,
      Set model,
      IDictionary<string, PersistentClass> persistentClasses,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      this.BindCollectionSecondPass((ICollectionPropertiesMapping) setMapping, (Collection) model, persistentClasses, inheritedMetas);
      if (model.IsOneToMany)
        return;
      model.CreatePrimaryKey();
    }

    private void BindMapSecondPass(
      HbmMap mapMapping,
      Map model,
      IDictionary<string, PersistentClass> persistentClasses,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      this.BindCollectionSecondPass((ICollectionPropertiesMapping) mapMapping, (Collection) model, persistentClasses, inheritedMetas);
      if (mapMapping.Item is HbmIndex indexMapping)
      {
        SimpleValue simpleValue = new SimpleValue(model.CollectionTable);
        new ValuePropertyBinder(simpleValue, this.Mappings).BindSimpleValue(indexMapping, "idx", model.IsOneToMany);
        model.Index = simpleValue;
        if (string.IsNullOrEmpty(model.Index.TypeName))
          throw new MappingException("map index element must specify a type: " + model.Role);
      }
      else if (mapMapping.Item is HbmMapKey mapKeyMapping)
      {
        SimpleValue simpleValue = new SimpleValue(model.CollectionTable);
        new ValuePropertyBinder(simpleValue, this.Mappings).BindSimpleValue(mapKeyMapping, "idx", model.IsOneToMany);
        model.Index = simpleValue;
        if (string.IsNullOrEmpty(model.Index.TypeName))
          throw new MappingException("map index element must specify a type: " + model.Role);
      }
      else if (mapMapping.Item is HbmIndexManyToMany indexManyToManyMapping)
      {
        ManyToOne model1 = new ManyToOne(model.CollectionTable);
        this.BindIndexManyToMany(indexManyToManyMapping, model1, "idx", model.IsOneToMany);
        model.Index = (SimpleValue) model1;
      }
      else if (mapMapping.Item is HbmMapKeyManyToMany mapKeyManyToManyMapping)
      {
        ManyToOne model2 = new ManyToOne(model.CollectionTable);
        this.BindMapKeyManyToMany(mapKeyManyToManyMapping, model2, "idx", model.IsOneToMany);
        model.Index = (SimpleValue) model2;
      }
      else if (mapMapping.Item is HbmCompositeIndex hbmCompositeIndex)
      {
        Component model3 = new Component((Collection) model);
        this.BindComponent((IComponentMapping) hbmCompositeIndex, model3, (System.Type) null, (string) null, model.Role + ".index", model.IsOneToMany, inheritedMetas);
        model.Index = (SimpleValue) model3;
      }
      else if (mapMapping.Item is HbmCompositeMapKey hbmCompositeMapKey)
      {
        Component model4 = new Component((Collection) model);
        this.BindComponent((IComponentMapping) hbmCompositeMapKey, model4, (System.Type) null, (string) null, model.Role + ".index", model.IsOneToMany, inheritedMetas);
        model.Index = (SimpleValue) model4;
      }
      else if (mapMapping.Item is HbmIndexManyToAny indexManyToAnyMapping)
      {
        Any any = new Any(model.CollectionTable);
        this.BindIndexManyToAny(indexManyToAnyMapping, any, model.IsOneToMany);
        model.Index = (SimpleValue) any;
      }
      bool flag = model.Index.ColumnIterator.Any<ISelectable>((Func<ISelectable, bool>) (x => x.IsFormula));
      if (!model.IsOneToMany || model.Key.IsNullable || model.IsInverse || flag)
        return;
      PersistentClass persistentClass = this.mappings.GetClass(((OneToMany) model.Element).ReferencedEntityName);
      IndexBackref p = new IndexBackref();
      p.Name = '_'.ToString() + model.OwnerEntityName + "." + mapMapping.Name + "IndexBackref";
      p.IsUpdateable = false;
      p.IsSelectable = false;
      p.CollectionRole = model.Role;
      p.EntityName = model.Owner.EntityName;
      p.Value = (IValue) model.Index;
      persistentClass.AddProperty((Property) p);
    }

    private void BindIndexManyToAny(
      HbmIndexManyToAny indexManyToAnyMapping,
      Any any,
      bool isNullable)
    {
      any.IdentifierTypeName = indexManyToAnyMapping.idtype;
      new TypeBinder((SimpleValue) any, this.Mappings).Bind(indexManyToAnyMapping.idtype);
      this.BindAnyMeta((IAnyMapping) indexManyToAnyMapping, any);
      new ColumnsBinder((SimpleValue) any, this.Mappings).Bind(indexManyToAnyMapping.Columns, isNullable, (Func<HbmColumn>) (() => new HbmColumn()
      {
        name = this.mappings.NamingStrategy.PropertyToColumnName(indexManyToAnyMapping.column1)
      }));
    }

    private void BindMapKeyManyToMany(
      HbmMapKeyManyToMany mapKeyManyToManyMapping,
      ManyToOne model,
      string defaultColumnName,
      bool isNullable)
    {
      new ValuePropertyBinder((SimpleValue) model, this.Mappings).BindSimpleValue(mapKeyManyToManyMapping, defaultColumnName, isNullable);
      model.ReferencedEntityName = ClassBinder.GetEntityName((IRelationship) mapKeyManyToManyMapping, this.mappings);
      this.BindForeignKey(mapKeyManyToManyMapping.foreignkey, (SimpleValue) model);
    }

    private void BindIndexManyToMany(
      HbmIndexManyToMany indexManyToManyMapping,
      ManyToOne model,
      string defaultColumnName,
      bool isNullable)
    {
      new ValuePropertyBinder((SimpleValue) model, this.Mappings).BindSimpleValue(indexManyToManyMapping, defaultColumnName, isNullable);
      model.ReferencedEntityName = ClassBinder.GetEntityName((IRelationship) indexManyToManyMapping, this.mappings);
      this.BindForeignKey(indexManyToManyMapping.foreignkey, (SimpleValue) model);
    }

    private void BindCollectionSecondPass(
      ICollectionPropertiesMapping collectionMapping,
      Collection model,
      IDictionary<string, PersistentClass> persistentClasses,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      if (model.IsOneToMany)
      {
        OneToMany element = (OneToMany) model.Element;
        string referencedEntityName = element.ReferencedEntityName;
        PersistentClass persistentClass;
        if (!persistentClasses.TryGetValue(referencedEntityName, out persistentClass))
          throw new MappingException("Association references unmapped class: " + referencedEntityName);
        element.AssociatedClass = persistentClass;
        model.CollectionTable = persistentClass.Table;
        if (model.IsInverse && persistentClass.JoinClosureSpan > 0)
        {
          using (IEnumerator<Join> enumerator = persistentClass.JoinClosureIterator.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              Join joined = enumerator.Current;
              if (collectionMapping.Key.Columns.Select<HbmColumn, string>((Func<HbmColumn, string>) (x => x.name)).All<string>((Func<string, bool>) (x => joined.Table.ColumnIterator.Select<Column, string>((Func<Column, string>) (jc => jc.Name)).Contains<string>(x))))
              {
                model.CollectionTable = joined.Table;
                break;
              }
            }
          }
        }
        if (Binder.log.IsInfoEnabled)
          Binder.log.Info((object) ("mapping collection: " + model.Role + " -> " + model.CollectionTable.Name));
      }
      if (!string.IsNullOrEmpty(collectionMapping.Check))
        model.CollectionTable.AddCheckConstraint(collectionMapping.Check);
      this.BindKey(collectionMapping.Key, model);
      if (collectionMapping.ElementRelationship is HbmElement elementRelationship4)
        this.BindElement(elementRelationship4, model);
      else if (collectionMapping.ElementRelationship is HbmManyToMany elementRelationship3)
        this.BindManyToMany(elementRelationship3, model);
      else if (collectionMapping.ElementRelationship is HbmCompositeElement elementRelationship2)
        this.BindCompositeElement(elementRelationship2, model, inheritedMetas);
      else if (collectionMapping.ElementRelationship is HbmManyToAny elementRelationship1)
        this.BindManyToAny(elementRelationship1, model);
      CollectionBinder.BindCache(collectionMapping.Cache, model);
      if (!model.IsOneToMany || model.IsInverse || model.Key.IsNullable)
        return;
      PersistentClass persistentClass1 = this.mappings.GetClass(((OneToMany) model.Element).ReferencedEntityName);
      Backref p = new Backref();
      p.Name = '_'.ToString() + model.OwnerEntityName + "." + collectionMapping.Name + "Backref";
      p.IsUpdateable = false;
      p.IsSelectable = false;
      p.CollectionRole = model.Role;
      p.EntityName = model.Owner.EntityName;
      p.Value = (IValue) model.Key;
      persistentClass1.AddProperty((Property) p);
    }

    private void BindManyToMany(HbmManyToMany manyToManyMapping, Collection model)
    {
      ManyToOne manyToOne = new ManyToOne(model.CollectionTable);
      model.Element = (IValue) manyToOne;
      new ValuePropertyBinder((SimpleValue) manyToOne, this.Mappings).BindSimpleValue(manyToManyMapping, "elt", false);
      this.InitOuterJoinFetchSetting(manyToManyMapping, (IFetchable) manyToOne);
      ClassBinder.InitLaziness(manyToManyMapping.lazySpecified ? new HbmRestrictedLaziness?(manyToManyMapping.lazy) : new HbmRestrictedLaziness?(), (ToOne) manyToOne, true);
      if (!string.IsNullOrEmpty(manyToManyMapping.propertyref))
        manyToOne.ReferencedPropertyName = manyToManyMapping.propertyref;
      manyToOne.ReferencedEntityName = ClassBinder.GetEntityName((IRelationship) manyToManyMapping, this.mappings);
      manyToOne.IsIgnoreNotFound = manyToManyMapping.NotFoundMode == HbmNotFoundMode.Ignore;
      if (!string.IsNullOrEmpty(manyToManyMapping.foreignkey))
        manyToOne.ForeignKeyName = manyToManyMapping.foreignkey;
      this.BindManyToManySubelements(manyToManyMapping, model);
    }

    private void BindCompositeElement(
      HbmCompositeElement compositeElementMapping,
      Collection model,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      Component model1 = new Component(model);
      model.Element = (IValue) model1;
      this.BindComponent((IComponentMapping) compositeElementMapping, model1, (System.Type) null, (string) null, model.Role + ".element", true, inheritedMetas);
    }

    private void BindManyToAny(HbmManyToAny manyToAnyMapping, Collection model)
    {
      Any model1 = new Any(model.CollectionTable);
      model.Element = (IValue) model1;
      model1.IdentifierTypeName = manyToAnyMapping.idtype;
      new TypeBinder((SimpleValue) model1, this.Mappings).Bind(manyToAnyMapping.idtype);
      this.BindAnyMeta((IAnyMapping) manyToAnyMapping, model1);
      new ColumnsBinder((SimpleValue) model1, this.Mappings).Bind(manyToAnyMapping.Columns, true, (Func<HbmColumn>) (() => new HbmColumn()
      {
        name = this.mappings.NamingStrategy.PropertyToColumnName(manyToAnyMapping.column1)
      }));
    }

    private void BindElement(HbmElement elementMapping, Collection model)
    {
      SimpleValue simpleValue = new SimpleValue(model.CollectionTable);
      model.Element = (IValue) simpleValue;
      if (model.IsGeneric)
      {
        switch (model.GenericArguments.Length)
        {
          case 1:
            simpleValue.TypeName = model.GenericArguments[0].AssemblyQualifiedName;
            break;
          case 2:
            simpleValue.TypeName = model.GenericArguments[1].AssemblyQualifiedName;
            break;
        }
      }
      new ValuePropertyBinder(simpleValue, this.Mappings).BindSimpleValue(elementMapping, "id", true);
    }

    private static void BindCache(HbmCache cacheSchema, Collection collection)
    {
      if (cacheSchema == null)
        return;
      collection.CacheConcurrencyStrategy = cacheSchema.usage.ToCacheConcurrencyStrategy();
      collection.CacheRegionName = cacheSchema.region;
    }

    private void BindKey(HbmKey keyMapping, Collection model)
    {
      if (keyMapping == null)
        return;
      string referencedPropertyName = model.ReferencedPropertyName;
      IKeyValue prototype = referencedPropertyName != null ? (IKeyValue) model.Owner.GetProperty(referencedPropertyName).Value : model.Owner.Identifier;
      DependantValue dependantValue1 = new DependantValue(model.CollectionTable, prototype);
      dependantValue1.IsCascadeDeleteEnabled = keyMapping.ondelete == HbmOndelete.Cascade;
      DependantValue dependantValue2 = dependantValue1;
      new ValuePropertyBinder((SimpleValue) dependantValue2, this.Mappings).BindSimpleValue(keyMapping, "id", model.IsOneToMany);
      if (dependantValue2.Type.ReturnedClass.IsArray)
        throw new MappingException("illegal use of an array as an identifier (arrays don't reimplement Equals)");
      model.Key = (IKeyValue) dependantValue2;
      dependantValue2.SetNullable(!keyMapping.IsNullable.HasValue || keyMapping.IsNullable.Value);
      dependantValue2.SetUpdateable(!keyMapping.IsUpdatable.HasValue || keyMapping.IsUpdatable.Value);
      this.BindForeignKey(keyMapping.foreignkey, (SimpleValue) dependantValue2);
    }

    private void BindManyToManySubelements(HbmManyToMany manyToManyMapping, Collection collection)
    {
      string where = string.IsNullOrEmpty(manyToManyMapping.where) ? (string) null : manyToManyMapping.where;
      collection.ManyToManyWhere = where;
      string orderby = string.IsNullOrEmpty(manyToManyMapping.orderby) ? (string) null : manyToManyMapping.orderby;
      collection.ManyToManyOrdering = orderby;
      HbmFilter[] filter = manyToManyMapping.filter;
      if ((filter != null && filter.Length > 0 || where != null) && collection.FetchMode == FetchMode.Join && collection.Element.FetchMode != FetchMode.Join)
        throw new MappingException(string.Format("many-to-many defining filter or where without join fetching not valid within collection using join fetching [{0}]", (object) collection.Role));
      new FiltersBinder((IFilterable) collection, this.Mappings).Bind((IEnumerable<HbmFilter>) filter, new Action<string, string>(collection.AddManyToManyFilter));
    }

    private System.Type GetPropertyType(
      System.Type containingType,
      string propertyName,
      string propertyAccess)
    {
      string access = propertyAccess ?? this.Mappings.DefaultAccess;
      return containingType != null ? ReflectHelper.ReflectedPropertyClass(containingType, propertyName, access) : (System.Type) null;
    }
  }
}

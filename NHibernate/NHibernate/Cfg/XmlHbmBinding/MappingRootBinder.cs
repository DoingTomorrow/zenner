// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.XmlHbmBinding.MappingRootBinder
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
  public class MappingRootBinder : Binder
  {
    private readonly NHibernate.Dialect.Dialect dialect;

    public MappingRootBinder(Mappings mappings, NHibernate.Dialect.Dialect dialect)
      : base(mappings)
    {
      this.dialect = dialect;
    }

    public void Bind(HbmMapping mappingSchema)
    {
      IDictionary<string, MetaAttribute> emptyMeta = Binder.EmptyMeta;
      IDictionary<string, MetaAttribute> metas = Binder.GetMetas((IDecoratable) mappingSchema, emptyMeta, true);
      this.SetMappingsProperties(mappingSchema);
      this.AddFilterDefinitions(mappingSchema);
      this.AddTypeDefs(mappingSchema);
      this.AddEntitiesMappings(mappingSchema, metas);
      this.AddQueries(mappingSchema);
      this.AddSqlQueries(mappingSchema);
      this.AddImports(mappingSchema);
      this.AddAuxiliaryDatabaseObjects(mappingSchema);
      this.AddResultSetMappingDefinitions(mappingSchema);
    }

    private void AddEntitiesMappings(
      HbmMapping mappingSchema,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      foreach (HbmClass rootClass in mappingSchema.RootClasses)
        this.AddRootClasses(rootClass, inheritedMetas);
      foreach (HbmSubclass subClass in mappingSchema.SubClasses)
        this.AddSubclasses(subClass, inheritedMetas);
      foreach (HbmJoinedSubclass joinedSubclass in mappingSchema.JoinedSubclasses)
        this.AddJoinedSubclasses(joinedSubclass, inheritedMetas);
      foreach (HbmUnionSubclass unionSubclass in mappingSchema.UnionSubclasses)
        this.AddUnionSubclasses(unionSubclass, inheritedMetas);
    }

    private void SetMappingsProperties(HbmMapping mappingSchema)
    {
      this.mappings.SchemaName = mappingSchema.schema ?? this.mappings.DefaultSchema;
      this.mappings.CatalogName = mappingSchema.catalog ?? this.mappings.DefaultCatalog;
      this.mappings.DefaultCascade = mappingSchema.defaultcascade;
      this.mappings.DefaultAccess = mappingSchema.defaultaccess;
      this.mappings.DefaultLazy = mappingSchema.defaultlazy;
      this.mappings.IsAutoImport = mappingSchema.autoimport;
      this.mappings.DefaultNamespace = mappingSchema.@namespace ?? this.mappings.DefaultNamespace;
      this.mappings.DefaultAssembly = mappingSchema.assembly ?? this.mappings.DefaultAssembly;
    }

    private void AddFilterDefinitions(HbmMapping mappingSchema)
    {
      foreach (HbmFilterDef filterDefinition in mappingSchema.FilterDefinitions)
        this.mappings.AddFilterDefinition(FilterDefinitionFactory.CreateFilterDefinition(filterDefinition));
    }

    private void AddRootClasses(
      HbmClass rootClass,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      new RootClassBinder(this.Mappings, this.dialect).Bind(rootClass, inheritedMetas);
    }

    private void AddUnionSubclasses(
      HbmUnionSubclass unionSubclass,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      new UnionSubclassBinder(this.Mappings, this.dialect).Bind(unionSubclass, inheritedMetas);
    }

    private void AddJoinedSubclasses(
      HbmJoinedSubclass joinedSubclass,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      new JoinedSubclassBinder(this.Mappings, this.dialect).Bind(joinedSubclass, inheritedMetas);
    }

    private void AddSubclasses(
      HbmSubclass subClass,
      IDictionary<string, MetaAttribute> inheritedMetas)
    {
      new SubclassBinder((Binder) this, this.dialect).Bind(subClass, inheritedMetas);
    }

    private void AddQueries(HbmMapping mappingSchema)
    {
      NamedQueryBinder namedQueryBinder = new NamedQueryBinder(this.Mappings);
      System.Array.ForEach<HbmQuery>(mappingSchema.HqlQueries, new Action<HbmQuery>(namedQueryBinder.AddQuery));
    }

    private void AddSqlQueries(HbmMapping mappingSchema)
    {
      NamedSQLQueryBinder namedSqlQueryBinder = new NamedSQLQueryBinder(this.Mappings);
      System.Array.ForEach<HbmSqlQuery>(mappingSchema.SqlQueries, new Action<HbmSqlQuery>(namedSqlQueryBinder.AddSqlQuery));
    }

    public void AddImports(HbmMapping mappingSchema)
    {
      foreach (HbmImport import in mappingSchema.Imports)
      {
        string str = Binder.FullQualifiedClassName(import.@class, this.mappings);
        string rename = import.rename ?? StringHelper.GetClassname(str);
        Binder.log.DebugFormat("Import: {0} -> {1}", (object) rename, (object) str);
        this.mappings.AddImport(str, rename);
      }
    }

    public void AddTypeDefs(HbmMapping mappingSchema)
    {
      foreach (HbmTypedef typeDefinition in mappingSchema.TypeDefinitions)
      {
        string typeClass = Binder.FullQualifiedClassName(typeDefinition.@class, this.mappings);
        string name = typeDefinition.name;
        IEnumerable<HbmParam> hbmParams = (IEnumerable<HbmParam>) (typeDefinition.param ?? new HbmParam[0]);
        Dictionary<string, string> paramMap = new Dictionary<string, string>(5);
        foreach (HbmParam hbmParam in hbmParams)
          paramMap.Add(hbmParam.name, hbmParam.GetText().Trim());
        this.mappings.AddTypeDef(name, typeClass, (IDictionary<string, string>) paramMap);
      }
    }

    private void AddAuxiliaryDatabaseObjects(HbmMapping mappingSchema)
    {
      foreach (HbmDatabaseObject databaseObject in mappingSchema.DatabaseObjects)
        this.mappings.AddAuxiliaryDatabaseObject(AuxiliaryDatabaseObjectFactory.Create(this.mappings, databaseObject));
    }

    private void AddResultSetMappingDefinitions(HbmMapping mappingSchema)
    {
      ResultSetMappingBinder binder = new ResultSetMappingBinder(this.Mappings);
      foreach (HbmResultSet resultSet in mappingSchema.ResultSets)
      {
        HbmResultSet tempResultSetSchema = resultSet;
        this.mappings.AddSecondPass((SecondPassCommand) (param0 => this.mappings.AddResultSetMapping(binder.Create(tempResultSetSchema))));
      }
    }
  }
}

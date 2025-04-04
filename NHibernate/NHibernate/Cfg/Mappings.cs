// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Mappings
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Cfg
{
  [Serializable]
  public class Mappings
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (Mappings));
    private readonly IDictionary<string, PersistentClass> classes;
    private readonly IDictionary<string, Collection> collections;
    private readonly IDictionary<string, Table> tables;
    private readonly IDictionary<string, NamedQueryDefinition> queries;
    private readonly IDictionary<string, NamedSQLQueryDefinition> sqlqueries;
    private readonly IDictionary<string, ResultSetMappingDefinition> resultSetMappings;
    private readonly IList<SecondPassCommand> secondPasses;
    private readonly IDictionary<string, string> imports;
    private string schemaName;
    private string catalogName;
    private string defaultCascade;
    private string defaultNamespace;
    private readonly NHibernate.Dialect.Dialect dialect;
    private string defaultAssembly;
    private string defaultAccess;
    private bool autoImport;
    private bool defaultLazy;
    private readonly IList<Mappings.PropertyReference> propertyReferences;
    private readonly IDictionary<string, FilterDefinition> filterDefinitions;
    private readonly IList<IAuxiliaryDatabaseObject> auxiliaryDatabaseObjects;
    private readonly Queue<FilterSecondPassArgs> filtersSecondPasses;
    private readonly INamingStrategy namingStrategy;
    protected internal IDictionary<string, TypeDef> typeDefs;
    protected internal ISet<ExtendsQueueEntry> extendsQueue;
    protected internal IDictionary<Table, Mappings.ColumnNames> columnNameBindingPerTable;
    protected internal IDictionary<string, Mappings.TableDescription> tableNameBinding;

    protected internal Mappings(
      IDictionary<string, PersistentClass> classes,
      IDictionary<string, Collection> collections,
      IDictionary<string, Table> tables,
      IDictionary<string, NamedQueryDefinition> queries,
      IDictionary<string, NamedSQLQueryDefinition> sqlqueries,
      IDictionary<string, ResultSetMappingDefinition> resultSetMappings,
      IDictionary<string, string> imports,
      IList<SecondPassCommand> secondPasses,
      Queue<FilterSecondPassArgs> filtersSecondPasses,
      IList<Mappings.PropertyReference> propertyReferences,
      INamingStrategy namingStrategy,
      IDictionary<string, TypeDef> typeDefs,
      IDictionary<string, FilterDefinition> filterDefinitions,
      ISet<ExtendsQueueEntry> extendsQueue,
      IList<IAuxiliaryDatabaseObject> auxiliaryDatabaseObjects,
      IDictionary<string, Mappings.TableDescription> tableNameBinding,
      IDictionary<Table, Mappings.ColumnNames> columnNameBindingPerTable,
      string defaultAssembly,
      string defaultNamespace,
      string defaultCatalog,
      string defaultSchema,
      string preferPooledValuesLo,
      NHibernate.Dialect.Dialect dialect)
    {
      this.classes = classes;
      this.collections = collections;
      this.queries = queries;
      this.sqlqueries = sqlqueries;
      this.resultSetMappings = resultSetMappings;
      this.tables = tables;
      this.imports = imports;
      this.secondPasses = secondPasses;
      this.propertyReferences = propertyReferences;
      this.namingStrategy = namingStrategy;
      this.typeDefs = typeDefs;
      this.filterDefinitions = filterDefinitions;
      this.extendsQueue = extendsQueue;
      this.auxiliaryDatabaseObjects = auxiliaryDatabaseObjects;
      this.tableNameBinding = tableNameBinding;
      this.columnNameBindingPerTable = columnNameBindingPerTable;
      this.defaultAssembly = defaultAssembly;
      this.defaultNamespace = defaultNamespace;
      this.DefaultCatalog = defaultCatalog;
      this.DefaultSchema = defaultSchema;
      this.PreferPooledValuesLo = preferPooledValuesLo;
      this.dialect = dialect;
      this.filtersSecondPasses = filtersSecondPasses;
    }

    public void AddClass(PersistentClass persistentClass)
    {
      if (this.classes.ContainsKey(persistentClass.EntityName))
        throw new DuplicateMappingException("class/entity", persistentClass.EntityName);
      this.classes[persistentClass.EntityName] = persistentClass;
    }

    public void AddCollection(Collection collection)
    {
      if (this.collections.ContainsKey(collection.Role))
        throw new DuplicateMappingException("collection role", collection.Role);
      this.collections[collection.Role] = collection;
    }

    public void AddUniquePropertyReference(string referencedClass, string propertyName)
    {
      this.propertyReferences.Add(new Mappings.PropertyReference()
      {
        referencedClass = referencedClass,
        propertyName = propertyName,
        unique = true
      });
    }

    public void AddPropertyReference(string referencedClass, string propertyName)
    {
      this.propertyReferences.Add(new Mappings.PropertyReference()
      {
        referencedClass = referencedClass,
        propertyName = propertyName
      });
    }

    public PersistentClass GetClass(string className)
    {
      PersistentClass persistentClass;
      this.classes.TryGetValue(className, out persistentClass);
      return persistentClass;
    }

    public NHibernate.Dialect.Dialect Dialect => this.dialect;

    public INamingStrategy NamingStrategy => this.namingStrategy;

    public string DefaultNamespace
    {
      get => this.defaultNamespace;
      set => this.defaultNamespace = value;
    }

    public string DefaultAssembly
    {
      get => this.defaultAssembly;
      set => this.defaultAssembly = value;
    }

    public string DefaultCatalog { get; set; }

    public string DefaultSchema { get; set; }

    public string PreferPooledValuesLo { get; set; }

    public Collection GetCollection(string role) => this.collections[role];

    public void AddImport(string className, string rename)
    {
      if (rename == null)
        throw new ArgumentNullException(nameof (rename));
      string str;
      this.imports.TryGetValue(rename, out str);
      this.imports[rename] = className;
      if (str == null)
        return;
      if (str.Equals(className))
        Mappings.log.Info((object) ("duplicate import: " + className + "->" + rename));
      else
        throw new DuplicateMappingException("duplicate import: " + rename + " refers to both " + className + " and " + str + " (try using auto-import=\"false\")", "import", rename);
    }

    public Table AddTable(
      string schema,
      string catalog,
      string name,
      string subselect,
      bool isAbstract,
      string schemaAction)
    {
      string key = subselect ?? this.dialect.Qualify(catalog, schema, name);
      Table table;
      if (!this.tables.TryGetValue(key, out table))
      {
        table = new Table();
        table.IsAbstract = isAbstract;
        table.Name = name;
        table.Schema = schema;
        table.Catalog = catalog;
        table.Subselect = subselect;
        table.SchemaActions = Mappings.GetSchemaActions(schemaAction);
        this.tables[key] = table;
      }
      else if (!isAbstract)
        table.IsAbstract = false;
      return table;
    }

    private static SchemaAction GetSchemaActions(string schemaAction)
    {
      if (string.IsNullOrEmpty(schemaAction))
        return SchemaAction.All;
      SchemaAction schemaActions = SchemaAction.None;
      string str1 = schemaAction;
      char[] chArray = new char[2]{ ',', ' ' };
      foreach (string str2 in str1.Split(chArray))
      {
        switch (str2.ToLowerInvariant())
        {
          case "":
          case "all":
            schemaActions |= SchemaAction.All;
            break;
          case "drop":
            schemaActions |= SchemaAction.Drop;
            break;
          case "update":
            schemaActions |= SchemaAction.Update;
            break;
          case "export":
            schemaActions |= SchemaAction.Export;
            break;
          case "validate":
            schemaActions |= SchemaAction.Validate;
            break;
          case "none":
            schemaActions = schemaActions;
            break;
          default:
            throw new MappingException(string.Format("Invalid schema-export value; Expected(all drop update export validate none), Found ({0})", (object) str2));
        }
      }
      return schemaActions;
    }

    public Table AddDenormalizedTable(
      string schema,
      string catalog,
      string name,
      bool isAbstract,
      string subselect,
      Table includedTable)
    {
      string key = subselect ?? this.dialect.Qualify(schema, catalog, name);
      DenormalizedTable denormalizedTable = new DenormalizedTable(includedTable);
      denormalizedTable.IsAbstract = isAbstract;
      denormalizedTable.Name = name;
      denormalizedTable.Catalog = catalog;
      denormalizedTable.Schema = schema;
      denormalizedTable.Subselect = subselect;
      Table table1 = (Table) denormalizedTable;
      Table table2;
      if (this.tables.TryGetValue(key, out table2) && table2.IsPhysicalTable)
        throw new DuplicateMappingException("table", name);
      this.tables[key] = table1;
      return table1;
    }

    public void AddTableBinding(
      string schema,
      string catalog,
      string logicalName,
      string physicalName,
      Table denormalizedSuperTable)
    {
      string key = Mappings.BuildTableNameKey(schema, catalog, physicalName);
      Mappings.TableDescription tableDescription1 = new Mappings.TableDescription(logicalName, denormalizedSuperTable);
      Mappings.TableDescription tableDescription2;
      this.tableNameBinding.TryGetValue(key, out tableDescription2);
      this.tableNameBinding[key] = tableDescription1;
      if (tableDescription2 != null && !tableDescription2.logicalName.Equals(logicalName))
        throw new MappingException("Same physical table name reference several logical table names: " + physicalName + " => '" + tableDescription2.logicalName + "' and '" + logicalName + "'");
    }

    public Table GetTable(string schema, string catalog, string name)
    {
      return this.tables[this.dialect.Qualify(catalog, schema, name)];
    }

    public string SchemaName
    {
      get => this.schemaName;
      set => this.schemaName = value;
    }

    public string CatalogName
    {
      get => this.catalogName;
      set => this.catalogName = value;
    }

    public string DefaultCascade
    {
      get => this.defaultCascade;
      set => this.defaultCascade = value;
    }

    public string DefaultAccess
    {
      get => this.defaultAccess;
      set => this.defaultAccess = value;
    }

    private void CheckQueryExists(string name)
    {
      if (this.queries.ContainsKey(name) || this.sqlqueries.ContainsKey(name))
        throw new DuplicateMappingException("query / sql-query", name);
    }

    public void AddQuery(string name, NamedQueryDefinition query)
    {
      this.CheckQueryExists(name);
      this.queries[name] = query;
    }

    public void AddSQLQuery(string name, NamedSQLQueryDefinition query)
    {
      this.CheckQueryExists(name);
      this.sqlqueries[name] = query;
    }

    public NamedQueryDefinition GetQuery(string name) => this.queries[name];

    public void AddSecondPass(SecondPassCommand command) => this.secondPasses.Add(command);

    public void AddSecondPass(SecondPassCommand command, bool onTopOfTheQueue)
    {
      if (onTopOfTheQueue)
        this.secondPasses.Insert(0, command);
      else
        this.secondPasses.Add(command);
    }

    public bool IsAutoImport
    {
      get => this.autoImport;
      set => this.autoImport = value;
    }

    public bool DefaultLazy
    {
      get => this.defaultLazy;
      set => this.defaultLazy = value;
    }

    public IDictionary<string, FilterDefinition> FilterDefinitions => this.filterDefinitions;

    public void AddFilterDefinition(FilterDefinition definition)
    {
      FilterDefinition filterDefinition;
      if (this.filterDefinitions.TryGetValue(definition.FilterName, out filterDefinition) && filterDefinition != null)
        throw new MappingException("Duplicated filter-def named: " + definition.FilterName);
      this.filterDefinitions[definition.FilterName] = definition;
    }

    public FilterDefinition GetFilterDefinition(string name)
    {
      FilterDefinition filterDefinition;
      this.filterDefinitions.TryGetValue(name, out filterDefinition);
      return filterDefinition;
    }

    public void AddAuxiliaryDatabaseObject(IAuxiliaryDatabaseObject auxiliaryDatabaseObject)
    {
      this.auxiliaryDatabaseObjects.Add(auxiliaryDatabaseObject);
    }

    public void AddResultSetMapping(ResultSetMappingDefinition sqlResultSetMapping)
    {
      string name = sqlResultSetMapping.Name;
      if (this.resultSetMappings.ContainsKey(name))
        throw new DuplicateMappingException("resultSet", name);
      this.resultSetMappings[name] = sqlResultSetMapping;
    }

    public void AddToExtendsQueue(ExtendsQueueEntry entry)
    {
      if (this.extendsQueue.Contains(entry))
        return;
      this.extendsQueue.Add(entry);
    }

    public void AddTypeDef(string typeName, string typeClass, IDictionary<string, string> paramMap)
    {
      TypeDef typeDef = new TypeDef(typeClass, paramMap);
      this.typeDefs[typeName] = typeDef;
      Mappings.log.Debug((object) ("Added " + typeName + " with class " + typeClass));
    }

    public TypeDef GetTypeDef(string typeName)
    {
      if (string.IsNullOrEmpty(typeName))
        return (TypeDef) null;
      TypeDef typeDef;
      this.typeDefs.TryGetValue(typeName, out typeDef);
      return typeDef;
    }

    public void AddColumnBinding(string logicalName, Column finalColumn, Table table)
    {
      Mappings.ColumnNames columnNames;
      if (!this.columnNameBindingPerTable.TryGetValue(table, out columnNames))
      {
        columnNames = new Mappings.ColumnNames();
        this.columnNameBindingPerTable[table] = columnNames;
      }
      string str1;
      columnNames.logicalToPhysical.TryGetValue(logicalName.ToLowerInvariant(), out str1);
      columnNames.logicalToPhysical[logicalName.ToLowerInvariant()] = finalColumn.GetQuotedName();
      if (str1 != null && (finalColumn.IsQuoted ? (str1.Equals(finalColumn.GetQuotedName()) ? 1 : 0) : (str1.Equals(finalColumn.GetQuotedName(), StringComparison.InvariantCultureIgnoreCase) ? 1 : 0)) == 0)
        throw new MappingException("Same logical column name referenced by different physical ones: " + table.Name + "." + logicalName + " => '" + str1 + "' and '" + finalColumn.GetQuotedName() + "'");
      string str2;
      columnNames.physicalToLogical.TryGetValue(finalColumn.GetQuotedName(), out str2);
      columnNames.physicalToLogical[finalColumn.GetQuotedName()] = logicalName;
      if (str2 != null && !str2.Equals(logicalName))
        throw new MappingException("Same physical column represented by different logical column names: " + table.Name + "." + finalColumn.GetQuotedName() + " => '" + str2 + "' and '" + logicalName + "'");
    }

    public string GetLogicalColumnName(string physicalName, Table table)
    {
      string str = (string) null;
      Table key = table;
      Mappings.TableDescription tableDescription;
      do
      {
        Mappings.ColumnNames columnNames;
        if (this.columnNameBindingPerTable.TryGetValue(key, out columnNames))
          columnNames.physicalToLogical.TryGetValue(physicalName, out str);
        if (this.tableNameBinding.TryGetValue(Mappings.BuildTableNameKey(key.Schema, key.Catalog, key.Name), out tableDescription))
          key = tableDescription.denormalizedSupertable;
      }
      while (str == null && key != null && tableDescription != null);
      return str != null ? str : throw new MappingException("Unable to find logical column name from physical name " + physicalName + " in table " + table.Name);
    }

    public string GetPhysicalColumnName(string logicalName, Table table)
    {
      logicalName = logicalName.ToLowerInvariant();
      string str = (string) null;
      Table key = table;
      do
      {
        Mappings.ColumnNames columnNames;
        if (this.columnNameBindingPerTable.TryGetValue(key, out columnNames))
          str = columnNames.logicalToPhysical[logicalName];
        Mappings.TableDescription tableDescription;
        if (this.tableNameBinding.TryGetValue(Mappings.BuildTableNameKey(key.Schema, key.Catalog, key.Name), out tableDescription))
          key = tableDescription.denormalizedSupertable;
      }
      while (str == null && key != null);
      return str != null ? str : throw new MappingException("Unable to find column with logical name " + logicalName + " in table " + table.Name);
    }

    private static string BuildTableNameKey(string schema, string catalog, string name)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (schema != null)
        stringBuilder.Append(schema);
      stringBuilder.Append(".");
      if (catalog != null)
        stringBuilder.Append(catalog);
      stringBuilder.Append(".");
      stringBuilder.Append(name);
      return stringBuilder.ToString();
    }

    private string GetLogicalTableName(string schema, string catalog, string physicalName)
    {
      Mappings.TableDescription tableDescription;
      if (!this.tableNameBinding.TryGetValue(Mappings.BuildTableNameKey(schema, catalog, physicalName), out tableDescription))
        throw new MappingException("Unable to find physical table: " + physicalName);
      return tableDescription.logicalName;
    }

    public string GetLogicalTableName(Table table)
    {
      return this.GetLogicalTableName(table.GetQuotedSchema(), table.Catalog, table.GetQuotedName());
    }

    public ResultSetMappingDefinition GetResultSetMapping(string name)
    {
      return this.resultSetMappings[name];
    }

    public IEnumerable<Collection> IterateCollections
    {
      get => (IEnumerable<Collection>) this.collections.Values;
    }

    public IEnumerable<Table> IterateTables => (IEnumerable<Table>) this.tables.Values;

    public PersistentClass LocatePersistentClassByEntityName(string entityName)
    {
      PersistentClass persistentClass;
      string key;
      if (!this.classes.TryGetValue(entityName, out persistentClass) && this.imports.TryGetValue(entityName, out key))
        this.classes.TryGetValue(key, out persistentClass);
      return persistentClass;
    }

    public void ExpectedFilterDefinition(
      IFilterable filterable,
      string filterName,
      string condition)
    {
      FilterDefinition filterDefinition = this.GetFilterDefinition(filterName);
      if (string.IsNullOrEmpty(condition) && filterDefinition != null)
        condition = filterDefinition.DefaultFilterCondition;
      if (string.IsNullOrEmpty(condition) && filterDefinition == null)
      {
        Mappings.log.Debug((object) string.Format("Adding filter second pass [{0}]", (object) filterName));
        this.filtersSecondPasses.Enqueue(new FilterSecondPassArgs(filterable, filterName));
      }
      else if (string.IsNullOrEmpty(condition) && filterDefinition != null)
        throw new MappingException("no filter condition found for filter: " + filterName);
      if (filterDefinition != null)
        return;
      this.FilterDefinitions[filterName] = (FilterDefinition) null;
    }

    [Serializable]
    public class ColumnNames
    {
      public readonly IDictionary<string, string> logicalToPhysical = (IDictionary<string, string>) new Dictionary<string, string>();
      public readonly IDictionary<string, string> physicalToLogical = (IDictionary<string, string>) new Dictionary<string, string>();
    }

    [Serializable]
    public class TableDescription
    {
      public readonly string logicalName;
      public readonly Table denormalizedSupertable;

      public TableDescription(string logicalName, Table denormalizedSupertable)
      {
        this.logicalName = logicalName;
        this.denormalizedSupertable = denormalizedSupertable;
      }
    }

    [Serializable]
    public sealed class PropertyReference
    {
      public string referencedClass;
      public string propertyName;
      public bool unique;
    }
  }
}

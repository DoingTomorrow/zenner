// Decompiled with JetBrains decompiler
// Type: NHibernate.Cfg.Configuration
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using Iesi.Collections.Generic;
using NHibernate.Bytecode;
using NHibernate.Cfg.ConfigurationSchema;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Cfg.XmlHbmBinding;
using NHibernate.Dialect;
using NHibernate.Dialect.Function;
using NHibernate.Dialect.Schema;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Id;
using NHibernate.Impl;
using NHibernate.Mapping;
using NHibernate.Proxy;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

#nullable disable
namespace NHibernate.Cfg
{
  [Serializable]
  public class Configuration : ISerializable
  {
    public const string DefaultHibernateCfgFileName = "hibernate.cfg.xml";
    private string currentDocumentName;
    private bool preMappingBuildProcessed;
    protected IDictionary<string, PersistentClass> classes;
    protected IDictionary<string, Collection> collections;
    protected IDictionary<string, Table> tables;
    protected IList<SecondPassCommand> secondPasses;
    protected Queue<FilterSecondPassArgs> filtersSecondPasses;
    protected IList<Mappings.PropertyReference> propertyReferences;
    private IInterceptor interceptor;
    private IDictionary<string, string> properties;
    protected IList<IAuxiliaryDatabaseObject> auxiliaryDatabaseObjects;
    private INamingStrategy namingStrategy = DefaultNamingStrategy.Instance;
    private MappingsQueue mappingsQueue;
    private EventListeners eventListeners;
    protected IDictionary<string, TypeDef> typeDefs;
    protected ISet<ExtendsQueueEntry> extendsQueue;
    protected IDictionary<string, Mappings.TableDescription> tableNameBinding;
    protected IDictionary<Table, Mappings.ColumnNames> columnNameBindingPerTable;
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (NHibernate.Cfg.Configuration));
    protected internal SettingsFactory settingsFactory;
    private static readonly XmlSerializer mappingDocumentSerializer = new XmlSerializer(typeof (HbmMapping));
    private IMapping mapping;
    private static readonly IInterceptor emptyInterceptor = (IInterceptor) new EmptyInterceptor();
    private string defaultAssembly;
    private string defaultNamespace;
    private XmlSchemas schemas;

    public Configuration(SerializationInfo info, StreamingContext context)
    {
      this.Reset();
      this.EntityNotFoundDelegate = this.GetSerialedObject<IEntityNotFoundDelegate>(info, "entityNotFoundDelegate");
      this.auxiliaryDatabaseObjects = this.GetSerialedObject<IList<IAuxiliaryDatabaseObject>>(info, nameof (auxiliaryDatabaseObjects));
      this.classes = this.GetSerialedObject<IDictionary<string, PersistentClass>>(info, nameof (classes));
      this.collections = this.GetSerialedObject<IDictionary<string, Collection>>(info, nameof (collections));
      this.columnNameBindingPerTable = this.GetSerialedObject<IDictionary<Table, Mappings.ColumnNames>>(info, nameof (columnNameBindingPerTable));
      this.defaultAssembly = this.GetSerialedObject<string>(info, nameof (defaultAssembly));
      this.defaultNamespace = this.GetSerialedObject<string>(info, nameof (defaultNamespace));
      this.eventListeners = this.GetSerialedObject<EventListeners>(info, nameof (eventListeners));
      this.FilterDefinitions = this.GetSerialedObject<IDictionary<string, FilterDefinition>>(info, "filterDefinitions");
      this.Imports = this.GetSerialedObject<IDictionary<string, string>>(info, "imports");
      this.interceptor = this.GetSerialedObject<IInterceptor>(info, nameof (interceptor));
      this.mapping = this.GetSerialedObject<IMapping>(info, nameof (mapping));
      this.NamedQueries = this.GetSerialedObject<IDictionary<string, NamedQueryDefinition>>(info, "namedQueries");
      this.NamedSQLQueries = this.GetSerialedObject<IDictionary<string, NamedSQLQueryDefinition>>(info, "namedSqlQueries");
      this.namingStrategy = this.GetSerialedObject<INamingStrategy>(info, nameof (namingStrategy));
      this.properties = this.GetSerialedObject<IDictionary<string, string>>(info, nameof (properties));
      this.propertyReferences = this.GetSerialedObject<IList<Mappings.PropertyReference>>(info, nameof (propertyReferences));
      this.settingsFactory = this.GetSerialedObject<SettingsFactory>(info, nameof (settingsFactory));
      this.SqlFunctions = this.GetSerialedObject<IDictionary<string, ISQLFunction>>(info, "sqlFunctions");
      this.SqlResultSetMappings = this.GetSerialedObject<IDictionary<string, ResultSetMappingDefinition>>(info, "sqlResultSetMappings");
      this.tableNameBinding = this.GetSerialedObject<IDictionary<string, Mappings.TableDescription>>(info, nameof (tableNameBinding));
      this.tables = this.GetSerialedObject<IDictionary<string, Table>>(info, nameof (tables));
      this.typeDefs = this.GetSerialedObject<IDictionary<string, TypeDef>>(info, nameof (typeDefs));
      this.filtersSecondPasses = this.GetSerialedObject<Queue<FilterSecondPassArgs>>(info, nameof (filtersSecondPasses));
    }

    private T GetSerialedObject<T>(SerializationInfo info, string name)
    {
      return (T) info.GetValue(name, typeof (T));
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      this.ConfigureProxyFactoryFactory();
      this.SecondPassCompile();
      this.Validate();
      info.AddValue("entityNotFoundDelegate", (object) this.EntityNotFoundDelegate);
      info.AddValue("auxiliaryDatabaseObjects", (object) this.auxiliaryDatabaseObjects);
      info.AddValue("classes", (object) this.classes);
      info.AddValue("collections", (object) this.collections);
      info.AddValue("columnNameBindingPerTable", (object) this.columnNameBindingPerTable);
      info.AddValue("defaultAssembly", (object) this.defaultAssembly);
      info.AddValue("defaultNamespace", (object) this.defaultNamespace);
      info.AddValue("eventListeners", (object) this.eventListeners);
      info.AddValue("filterDefinitions", (object) this.FilterDefinitions);
      info.AddValue("imports", (object) this.Imports);
      info.AddValue("interceptor", (object) this.interceptor);
      info.AddValue("mapping", (object) this.mapping);
      info.AddValue("namedQueries", (object) this.NamedQueries);
      info.AddValue("namedSqlQueries", (object) this.NamedSQLQueries);
      info.AddValue("namingStrategy", (object) this.namingStrategy);
      info.AddValue("properties", (object) this.properties);
      info.AddValue("propertyReferences", (object) this.propertyReferences);
      info.AddValue("settingsFactory", (object) this.settingsFactory);
      info.AddValue("sqlFunctions", (object) this.SqlFunctions);
      info.AddValue("sqlResultSetMappings", (object) this.SqlResultSetMappings);
      info.AddValue("tableNameBinding", (object) this.tableNameBinding);
      info.AddValue("tables", (object) this.tables);
      info.AddValue("typeDefs", (object) this.typeDefs);
      info.AddValue("filtersSecondPasses", (object) this.filtersSecondPasses);
    }

    protected void Reset()
    {
      this.classes = (IDictionary<string, PersistentClass>) new Dictionary<string, PersistentClass>();
      this.Imports = (IDictionary<string, string>) new Dictionary<string, string>();
      this.collections = (IDictionary<string, Collection>) new Dictionary<string, Collection>();
      this.tables = (IDictionary<string, Table>) new Dictionary<string, Table>();
      this.NamedQueries = (IDictionary<string, NamedQueryDefinition>) new Dictionary<string, NamedQueryDefinition>();
      this.NamedSQLQueries = (IDictionary<string, NamedSQLQueryDefinition>) new Dictionary<string, NamedSQLQueryDefinition>();
      this.SqlResultSetMappings = (IDictionary<string, ResultSetMappingDefinition>) new Dictionary<string, ResultSetMappingDefinition>();
      this.secondPasses = (IList<SecondPassCommand>) new List<SecondPassCommand>();
      this.propertyReferences = (IList<Mappings.PropertyReference>) new List<Mappings.PropertyReference>();
      this.FilterDefinitions = (IDictionary<string, FilterDefinition>) new Dictionary<string, FilterDefinition>();
      this.interceptor = NHibernate.Cfg.Configuration.emptyInterceptor;
      this.properties = Environment.Properties;
      this.auxiliaryDatabaseObjects = (IList<IAuxiliaryDatabaseObject>) new List<IAuxiliaryDatabaseObject>();
      this.SqlFunctions = (IDictionary<string, ISQLFunction>) new Dictionary<string, ISQLFunction>();
      this.mappingsQueue = new MappingsQueue();
      this.eventListeners = new EventListeners();
      this.typeDefs = (IDictionary<string, TypeDef>) new Dictionary<string, TypeDef>();
      this.extendsQueue = (ISet<ExtendsQueueEntry>) new HashedSet<ExtendsQueueEntry>();
      this.tableNameBinding = (IDictionary<string, Mappings.TableDescription>) new Dictionary<string, Mappings.TableDescription>();
      this.columnNameBindingPerTable = (IDictionary<Table, Mappings.ColumnNames>) new Dictionary<Table, Mappings.ColumnNames>();
      this.filtersSecondPasses = new Queue<FilterSecondPassArgs>();
    }

    protected Configuration(SettingsFactory settingsFactory)
    {
      this.InitBlock();
      this.settingsFactory = settingsFactory;
      this.Reset();
    }

    private void InitBlock() => this.mapping = this.BuildMapping();

    public virtual IMapping BuildMapping() => (IMapping) new NHibernate.Cfg.Configuration.Mapping(this);

    public Configuration()
      : this(new SettingsFactory())
    {
    }

    public ICollection<PersistentClass> ClassMappings => this.classes.Values;

    public ICollection<Collection> CollectionMappings => this.collections.Values;

    private ICollection<Table> TableMappings => this.tables.Values;

    public PersistentClass GetClassMapping(System.Type persistentClass)
    {
      return this.GetClassMapping(persistentClass.FullName);
    }

    public PersistentClass GetClassMapping(string entityName)
    {
      PersistentClass classMapping;
      this.classes.TryGetValue(entityName, out classMapping);
      return classMapping;
    }

    public Collection GetCollectionMapping(string role)
    {
      return !this.collections.ContainsKey(role) ? (Collection) null : this.collections[role];
    }

    public NHibernate.Cfg.Configuration AddFile(string xmlFile) => this.AddXmlFile(xmlFile);

    public NHibernate.Cfg.Configuration AddFile(FileInfo xmlFile) => this.AddFile(xmlFile.FullName);

    private static void LogAndThrow(Exception exception)
    {
      if (NHibernate.Cfg.Configuration.log.IsErrorEnabled)
        NHibernate.Cfg.Configuration.log.Error((object) exception.Message, exception);
      throw exception;
    }

    public NHibernate.Cfg.Configuration AddXmlFile(string xmlFile)
    {
      NHibernate.Cfg.Configuration.log.Info((object) ("Mapping file: " + xmlFile));
      XmlTextReader hbmReader = (XmlTextReader) null;
      try
      {
        hbmReader = new XmlTextReader(xmlFile);
        this.AddXmlReader((XmlReader) hbmReader, xmlFile);
      }
      catch (MappingException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        NHibernate.Cfg.Configuration.LogAndThrow((Exception) new MappingException("Could not configure datastore from file " + xmlFile, ex));
      }
      finally
      {
        hbmReader?.Close();
      }
      return this;
    }

    public NHibernate.Cfg.Configuration AddXml(string xml) => this.AddXml(xml, "(string)");

    public NHibernate.Cfg.Configuration AddXml(string xml, string name)
    {
      if (NHibernate.Cfg.Configuration.log.IsDebugEnabled)
        NHibernate.Cfg.Configuration.log.Debug((object) ("Mapping XML:\n" + xml));
      XmlTextReader hbmReader = (XmlTextReader) null;
      try
      {
        hbmReader = new XmlTextReader(xml, XmlNodeType.Document, (XmlParserContext) null);
        this.AddXmlReader((XmlReader) hbmReader, name);
      }
      catch (MappingException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        NHibernate.Cfg.Configuration.LogAndThrow((Exception) new MappingException("Could not configure datastore from XML string " + name, ex));
      }
      finally
      {
        hbmReader?.Close();
      }
      return this;
    }

    public NHibernate.Cfg.Configuration AddXmlString(string xml) => this.AddXml(xml);

    public NHibernate.Cfg.Configuration AddUrl(string url) => this.AddFile(url);

    public NHibernate.Cfg.Configuration AddUrl(Uri url) => this.AddUrl(url.AbsolutePath);

    public NHibernate.Cfg.Configuration AddDocument(XmlDocument doc)
    {
      return this.AddDocument(doc, "(XmlDocument)");
    }

    public NHibernate.Cfg.Configuration AddDocument(XmlDocument doc, string name)
    {
      if (NHibernate.Cfg.Configuration.log.IsDebugEnabled)
        NHibernate.Cfg.Configuration.log.Debug((object) ("Mapping XML:\n" + doc.OuterXml));
      try
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          doc.Save((Stream) memoryStream);
          memoryStream.Position = 0L;
          this.AddInputStream((Stream) memoryStream, name);
        }
        return this;
      }
      catch (MappingException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        NHibernate.Cfg.Configuration.LogAndThrow((Exception) new MappingException("Could not configure datastore from XML document " + name, ex));
        return this;
      }
    }

    private void AddValidatedDocument(NamedXmlDocument doc)
    {
      this.AddDeserializedMapping(doc.Document, doc.Name);
    }

    public event EventHandler<BindMappingEventArgs> BeforeBindMapping;

    public event EventHandler<BindMappingEventArgs> AfterBindMapping;

    public void AddDeserializedMapping(HbmMapping mappingDocument, string documentFileName)
    {
      if (mappingDocument == null)
        throw new ArgumentNullException(nameof (mappingDocument));
      try
      {
        NHibernate.Dialect.Dialect dialect = NHibernate.Dialect.Dialect.GetDialect(this.properties);
        this.OnBeforeBindMapping(new BindMappingEventArgs(dialect, mappingDocument, documentFileName));
        new MappingRootBinder(this.CreateMappings(dialect), dialect).Bind(mappingDocument);
        this.OnAfterBindMapping(new BindMappingEventArgs(dialect, mappingDocument, documentFileName));
      }
      catch (Exception ex)
      {
        NHibernate.Cfg.Configuration.LogAndThrow((Exception) new MappingException(documentFileName == null ? "Could not compile deserialized mapping document." : "Could not compile the mapping document: " + documentFileName, ex));
      }
    }

    public void AddMapping(HbmMapping mappingDocument)
    {
      this.AddDeserializedMapping(mappingDocument, "mapping_by_code");
    }

    private void OnAfterBindMapping(BindMappingEventArgs bindMappingEventArgs)
    {
      EventHandler<BindMappingEventArgs> afterBindMapping = this.AfterBindMapping;
      if (afterBindMapping == null)
        return;
      afterBindMapping((object) this, bindMappingEventArgs);
    }

    private void OnBeforeBindMapping(BindMappingEventArgs bindMappingEventArgs)
    {
      EventHandler<BindMappingEventArgs> beforeBindMapping = this.BeforeBindMapping;
      if (beforeBindMapping == null)
        return;
      beforeBindMapping((object) this, bindMappingEventArgs);
    }

    public Mappings CreateMappings(NHibernate.Dialect.Dialect dialect)
    {
      string defaultCatalog = PropertiesHelper.GetString("default_catalog", this.properties, (string) null);
      string defaultSchema = PropertiesHelper.GetString("default_schema", this.properties, (string) null);
      string preferPooledValuesLo = PropertiesHelper.GetString("id.optimizer.pooled.prefer_lo", this.properties, (string) null);
      this.ProcessPreMappingBuildProperties();
      return new Mappings(this.classes, this.collections, this.tables, this.NamedQueries, this.NamedSQLQueries, this.SqlResultSetMappings, this.Imports, this.secondPasses, this.filtersSecondPasses, this.propertyReferences, this.namingStrategy, this.typeDefs, this.FilterDefinitions, this.extendsQueue, this.auxiliaryDatabaseObjects, this.tableNameBinding, this.columnNameBindingPerTable, this.defaultAssembly, this.defaultNamespace, defaultCatalog, defaultSchema, preferPooledValuesLo, dialect);
    }

    private void ProcessPreMappingBuildProperties()
    {
      if (this.preMappingBuildProcessed)
        return;
      this.ConfigureCollectionTypeFactory();
      this.preMappingBuildProcessed = true;
    }

    private void ConfigureCollectionTypeFactory()
    {
      string property = this.GetProperty("collectiontype.factory_class");
      if (string.IsNullOrEmpty(property) || !(Environment.BytecodeProvider is IInjectableCollectionTypeFactoryClass bytecodeProvider))
        return;
      bytecodeProvider.SetCollectionTypeFactoryClass(property);
    }

    public NHibernate.Cfg.Configuration AddInputStream(Stream xmlInputStream)
    {
      return this.AddInputStream(xmlInputStream, (string) null);
    }

    public NHibernate.Cfg.Configuration AddInputStream(Stream xmlInputStream, string name)
    {
      XmlTextReader hbmReader = (XmlTextReader) null;
      try
      {
        hbmReader = new XmlTextReader(xmlInputStream);
        this.AddXmlReader((XmlReader) hbmReader, name);
        return this;
      }
      catch (MappingException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        NHibernate.Cfg.Configuration.LogAndThrow((Exception) new MappingException("Could not configure datastore from input stream " + name, ex));
        return this;
      }
      finally
      {
        hbmReader?.Close();
      }
    }

    public NHibernate.Cfg.Configuration AddResource(string path, Assembly assembly)
    {
      string name = path;
      NHibernate.Cfg.Configuration.log.Info((object) ("Mapping resource: " + name));
      Stream manifestResourceStream = assembly.GetManifestResourceStream(path);
      if (manifestResourceStream == null)
        NHibernate.Cfg.Configuration.LogAndThrow((Exception) new MappingException("Resource not found: " + name));
      try
      {
        return this.AddInputStream(manifestResourceStream, name);
      }
      catch (MappingException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        NHibernate.Cfg.Configuration.LogAndThrow((Exception) new MappingException("Could not configure datastore from resource " + name, ex));
        return this;
      }
      finally
      {
        manifestResourceStream?.Close();
      }
    }

    public NHibernate.Cfg.Configuration AddResources(IEnumerable<string> paths, Assembly assembly)
    {
      if (paths == null)
        throw new ArgumentNullException(nameof (paths));
      foreach (string path in paths)
        this.AddResource(path, assembly);
      return this;
    }

    public NHibernate.Cfg.Configuration AddClass(System.Type persistentClass)
    {
      return this.AddResource(persistentClass.FullName + ".hbm.xml", persistentClass.Assembly);
    }

    public NHibernate.Cfg.Configuration AddAssembly(string assemblyName)
    {
      NHibernate.Cfg.Configuration.log.Info((object) ("Searching for mapped documents in assembly: " + assemblyName));
      Assembly assembly = (Assembly) null;
      try
      {
        assembly = Assembly.Load(assemblyName);
      }
      catch (Exception ex)
      {
        NHibernate.Cfg.Configuration.LogAndThrow((Exception) new MappingException("Could not add assembly " + assemblyName, ex));
      }
      return this.AddAssembly(assembly);
    }

    public NHibernate.Cfg.Configuration AddAssembly(Assembly assembly)
    {
      IList<string> xmlResourceNames = NHibernate.Cfg.Configuration.GetAllHbmXmlResourceNames(assembly);
      if (xmlResourceNames.Count == 0)
        NHibernate.Cfg.Configuration.log.Warn((object) ("No mapped documents found in assembly: " + assembly.FullName));
      foreach (string path in (IEnumerable<string>) xmlResourceNames)
        this.AddResource(path, assembly);
      return this;
    }

    private static IList<string> GetAllHbmXmlResourceNames(Assembly assembly)
    {
      List<string> xmlResourceNames = new List<string>();
      foreach (string manifestResourceName in assembly.GetManifestResourceNames())
      {
        if (manifestResourceName.EndsWith(".hbm.xml"))
          xmlResourceNames.Add(manifestResourceName);
      }
      return (IList<string>) xmlResourceNames;
    }

    public NHibernate.Cfg.Configuration AddDirectory(DirectoryInfo dir)
    {
      foreach (DirectoryInfo directory in dir.GetDirectories())
        this.AddDirectory(directory);
      foreach (FileInfo file in dir.GetFiles("*.hbm.xml"))
        this.AddFile(file);
      return this;
    }

    public string[] GenerateDropSchemaScript(NHibernate.Dialect.Dialect dialect)
    {
      this.SecondPassCompile();
      string defaultCatalog = PropertiesHelper.GetString("default_catalog", this.properties, (string) null);
      string defaultSchema = PropertiesHelper.GetString("default_schema", this.properties, (string) null);
      List<string> stringList = new List<string>();
      if (!dialect.SupportsForeignKeyConstraintInAlterTable && !string.IsNullOrEmpty(dialect.DisableForeignKeyConstraintsString))
        stringList.Add(dialect.DisableForeignKeyConstraintsString);
      for (int index = this.auxiliaryDatabaseObjects.Count - 1; index >= 0; --index)
      {
        IAuxiliaryDatabaseObject auxiliaryDatabaseObject = this.auxiliaryDatabaseObjects[index];
        if (auxiliaryDatabaseObject.AppliesToDialect(dialect))
          stringList.Add(auxiliaryDatabaseObject.SqlDropString(dialect, defaultCatalog, defaultSchema));
      }
      if (dialect.DropConstraints)
      {
        foreach (Table tableMapping in (IEnumerable<Table>) this.TableMappings)
        {
          if (tableMapping.IsPhysicalTable && NHibernate.Cfg.Configuration.IncludeAction(tableMapping.SchemaActions, SchemaAction.Drop))
          {
            foreach (ForeignKey foreignKey in tableMapping.ForeignKeyIterator)
            {
              if (foreignKey.HasPhysicalConstraint && NHibernate.Cfg.Configuration.IncludeAction(foreignKey.ReferencedTable.SchemaActions, SchemaAction.Drop))
                stringList.Add(foreignKey.SqlDropString(dialect, defaultCatalog, defaultSchema));
            }
          }
        }
      }
      foreach (Table tableMapping in (IEnumerable<Table>) this.TableMappings)
      {
        if (tableMapping.IsPhysicalTable && NHibernate.Cfg.Configuration.IncludeAction(tableMapping.SchemaActions, SchemaAction.Drop))
          stringList.Add(tableMapping.SqlDropString(dialect, defaultCatalog, defaultSchema));
      }
      foreach (IPersistentIdentifierGenerator iterateGenerator in this.IterateGenerators(dialect))
      {
        string[] strArray = iterateGenerator.SqlDropString(dialect);
        if (strArray != null)
        {
          foreach (string str in strArray)
            stringList.Add(str);
        }
      }
      if (!dialect.SupportsForeignKeyConstraintInAlterTable && !string.IsNullOrEmpty(dialect.EnableForeignKeyConstraintsString))
        stringList.Add(dialect.EnableForeignKeyConstraintsString);
      return stringList.ToArray();
    }

    public static bool IncludeAction(SchemaAction actionsSource, SchemaAction includedAction)
    {
      return (actionsSource & includedAction) != SchemaAction.None;
    }

    public string[] GenerateSchemaCreationScript(NHibernate.Dialect.Dialect dialect)
    {
      this.SecondPassCompile();
      string defaultCatalog = PropertiesHelper.GetString("default_catalog", this.properties, (string) null);
      string defaultSchema = PropertiesHelper.GetString("default_schema", this.properties, (string) null);
      List<string> stringList = new List<string>();
      foreach (Table tableMapping in (IEnumerable<Table>) this.TableMappings)
      {
        if (tableMapping.IsPhysicalTable && NHibernate.Cfg.Configuration.IncludeAction(tableMapping.SchemaActions, SchemaAction.Export))
        {
          stringList.Add(tableMapping.SqlCreateString(dialect, this.mapping, defaultCatalog, defaultSchema));
          stringList.AddRange((IEnumerable<string>) tableMapping.SqlCommentStrings(dialect, defaultCatalog, defaultSchema));
        }
      }
      foreach (Table tableMapping in (IEnumerable<Table>) this.TableMappings)
      {
        if (tableMapping.IsPhysicalTable && NHibernate.Cfg.Configuration.IncludeAction(tableMapping.SchemaActions, SchemaAction.Export))
        {
          if (!dialect.SupportsUniqueConstraintInCreateAlterTable)
          {
            foreach (Constraint constraint in tableMapping.UniqueKeyIterator)
            {
              string str = constraint.SqlCreateString(dialect, this.mapping, defaultCatalog, defaultSchema);
              if (str != null)
                stringList.Add(str);
            }
          }
          foreach (Index index in tableMapping.IndexIterator)
            stringList.Add(index.SqlCreateString(dialect, this.mapping, defaultCatalog, defaultSchema));
          if (dialect.SupportsForeignKeyConstraintInAlterTable)
          {
            foreach (ForeignKey foreignKey in tableMapping.ForeignKeyIterator)
            {
              if (foreignKey.HasPhysicalConstraint && NHibernate.Cfg.Configuration.IncludeAction(foreignKey.ReferencedTable.SchemaActions, SchemaAction.Export))
                stringList.Add(foreignKey.SqlCreateString(dialect, this.mapping, defaultCatalog, defaultSchema));
            }
          }
        }
      }
      foreach (IPersistentIdentifierGenerator iterateGenerator in this.IterateGenerators(dialect))
        stringList.AddRange((IEnumerable<string>) iterateGenerator.SqlCreateStrings(dialect));
      foreach (IAuxiliaryDatabaseObject auxiliaryDatabaseObject in (IEnumerable<IAuxiliaryDatabaseObject>) this.auxiliaryDatabaseObjects)
      {
        if (auxiliaryDatabaseObject.AppliesToDialect(dialect))
          stringList.Add(auxiliaryDatabaseObject.SqlCreateString(dialect, this.mapping, defaultCatalog, defaultSchema));
      }
      return stringList.ToArray();
    }

    private void Validate()
    {
      this.ValidateEntities();
      this.ValidateCollections();
      this.ValidateFilterDefs();
    }

    private void ValidateFilterDefs()
    {
      HashedSet<string> hashedSet = new HashedSet<string>();
      foreach (KeyValuePair<string, FilterDefinition> filterDefinition in (IEnumerable<KeyValuePair<string, FilterDefinition>>) this.FilterDefinitions)
      {
        if (filterDefinition.Value == null)
          hashedSet.Add(filterDefinition.Key);
      }
      if (hashedSet.Count > 0)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("filter-def for filter named ");
        foreach (string str in (Iesi.Collections.Generic.Set<string>) hashedSet)
          stringBuilder.AppendLine(str);
        stringBuilder.AppendLine("was not found.");
        throw new MappingException(stringBuilder.ToString());
      }
      if (this.FilterDefinitions.Count <= 0)
        return;
      hashedSet.Clear();
      foreach (PersistentClass classMapping in (IEnumerable<PersistentClass>) this.ClassMappings)
        hashedSet.AddAll(classMapping.FilterMap.Keys);
      foreach (Collection collectionMapping in (IEnumerable<Collection>) this.CollectionMappings)
      {
        hashedSet.AddAll(collectionMapping.FilterMap.Keys);
        hashedSet.AddAll(collectionMapping.ManyToManyFilterMap.Keys);
      }
      foreach (string key in (IEnumerable<string>) this.FilterDefinitions.Keys)
      {
        if (!hashedSet.Contains(key))
          NHibernate.Cfg.Configuration.log.ErrorFormat("filter-def for filter named '{0}' was never used to filter classes nor collections.\r\nThis may result in unexpected behavior during queries", (object) key);
      }
    }

    private void ValidateCollections()
    {
      foreach (Collection collection in (IEnumerable<Collection>) this.collections.Values)
        collection.Validate(this.mapping);
    }

    private void ValidateEntities()
    {
      bool boolean = PropertiesHelper.GetBoolean("use_proxy_validator", this.properties, true);
      HashedSet<string> errors = (HashedSet<string>) null;
      IProxyValidator proxyValidator = Environment.BytecodeProvider.ProxyFactoryFactory.ProxyValidator;
      foreach (PersistentClass persistentClass in (IEnumerable<PersistentClass>) this.classes.Values)
      {
        persistentClass.Validate(this.mapping);
        if (boolean)
        {
          ICollection<string> strings = NHibernate.Cfg.Configuration.ValidateProxyInterface(persistentClass, proxyValidator);
          if (strings != null)
          {
            if (errors == null)
              errors = new HashedSet<string>(strings);
            else
              errors.AddAll(strings);
          }
        }
      }
      if (errors != null)
        throw new InvalidProxyTypeException((ICollection<string>) errors);
    }

    private static ICollection<string> ValidateProxyInterface(
      PersistentClass persistentClass,
      IProxyValidator validator)
    {
      if (!persistentClass.IsLazy)
        return (ICollection<string>) null;
      return persistentClass.ProxyInterface == null ? (ICollection<string>) null : validator.ValidateType(persistentClass.ProxyInterface);
    }

    public virtual void BuildMappings() => this.SecondPassCompile();

    private void SecondPassCompile()
    {
      NHibernate.Cfg.Configuration.log.Info((object) "checking mappings queue");
      this.mappingsQueue.CheckNoUnavailableEntries();
      NHibernate.Cfg.Configuration.log.Info((object) "processing one-to-many association mappings");
      foreach (SecondPassCommand secondPass in (IEnumerable<SecondPassCommand>) this.secondPasses)
        secondPass(this.classes);
      this.secondPasses.Clear();
      NHibernate.Cfg.Configuration.log.Info((object) "processing one-to-one association property references");
      foreach (Mappings.PropertyReference propertyReference in (IEnumerable<Mappings.PropertyReference>) this.propertyReferences)
      {
        PersistentClass classMapping = this.GetClassMapping(propertyReference.referencedClass);
        if (classMapping == null)
          throw new MappingException("property-ref to unmapped class: " + propertyReference.referencedClass);
        ((SimpleValue) classMapping.GetReferencedProperty(propertyReference.propertyName).Value).IsAlternateUniqueKey = true;
      }
      NHibernate.Cfg.Configuration.log.Info((object) "processing foreign key constraints");
      ISet done = (ISet) new HashedSet();
      foreach (Table tableMapping in (IEnumerable<Table>) this.TableMappings)
        this.SecondPassCompileForeignKeys(tableMapping, done);
      NHibernate.Cfg.Configuration.log.Info((object) "processing filters (second pass)");
      foreach (FilterSecondPassArgs filtersSecondPass in this.filtersSecondPasses)
      {
        string filterName = filtersSecondPass.FilterName;
        FilterDefinition filterDefinition;
        this.FilterDefinitions.TryGetValue(filterName, out filterDefinition);
        if (filterDefinition == null)
          throw new MappingException("filter-def for filter named " + filterName + " was not found.");
        filtersSecondPass.Filterable.FilterMap[filterName] = !string.IsNullOrEmpty(filterDefinition.DefaultFilterCondition) ? filterDefinition.DefaultFilterCondition : throw new MappingException("no filter condition found for filter: " + filterName);
      }
    }

    private void SecondPassCompileForeignKeys(Table table, ISet done)
    {
      table.CreateForeignKeys();
      foreach (ForeignKey o in table.ForeignKeyIterator)
      {
        if (!done.Contains((object) o))
        {
          done.Add((object) o);
          string referencedEntityName = o.ReferencedEntityName;
          if (string.IsNullOrEmpty(referencedEntityName))
            throw new MappingException(string.Format("An association from the table {0} does not specify the referenced entity", (object) o.Table.Name));
          if (NHibernate.Cfg.Configuration.log.IsDebugEnabled)
            NHibernate.Cfg.Configuration.log.Debug((object) ("resolving reference to class: " + referencedEntityName));
          PersistentClass referencedClass;
          if (!this.classes.TryGetValue(referencedEntityName, out referencedClass))
          {
            NHibernate.Cfg.Configuration.LogAndThrow((Exception) new MappingException(string.Format("An association from the table {0} refers to an unmapped class: {1}", (object) o.Table.Name, (object) referencedEntityName)));
          }
          else
          {
            if (referencedClass.IsJoinedSubclass)
              this.SecondPassCompileForeignKeys(referencedClass.Superclass.Table, done);
            try
            {
              o.AddReferencedTable(referencedClass);
              o.AlignColumns();
            }
            catch (MappingException ex)
            {
              NHibernate.Cfg.Configuration.LogAndThrow((Exception) ex);
            }
          }
        }
      }
    }

    public IDictionary<string, NamedQueryDefinition> NamedQueries { get; protected set; }

    private EventListeners GetInitializedEventListeners()
    {
      EventListeners initializedEventListeners = this.eventListeners.ShallowCopy();
      initializedEventListeners.InitializeListeners(this);
      return initializedEventListeners;
    }

    public IEntityNotFoundDelegate EntityNotFoundDelegate { get; set; }

    public EventListeners EventListeners => this.eventListeners;

    protected virtual void ConfigureProxyFactoryFactory()
    {
      IInjectableProxyFactoryFactory bytecodeProvider = Environment.BytecodeProvider as IInjectableProxyFactoryFactory;
      string typeName;
      this.properties.TryGetValue("proxyfactory.factory_class", out typeName);
      if (bytecodeProvider == null || string.IsNullOrEmpty(typeName))
        return;
      bytecodeProvider.SetProxyFactoryFactory(typeName);
    }

    public ISessionFactory BuildSessionFactory()
    {
      this.ConfigureProxyFactoryFactory();
      this.SecondPassCompile();
      this.Validate();
      Environment.VerifyProperties(this.properties);
      Settings settings = this.BuildSettings();
      this.Schemas = (XmlSchemas) null;
      return (ISessionFactory) new SessionFactoryImpl(this, this.mapping, settings, this.GetInitializedEventListeners());
    }

    public IInterceptor Interceptor
    {
      get => this.interceptor;
      set => this.interceptor = value;
    }

    public IDictionary<string, string> Properties
    {
      get => this.properties;
      set => this.properties = value;
    }

    public IDictionary<string, string> GetDerivedProperties()
    {
      IDictionary<string, string> derivedProperties = (IDictionary<string, string>) new Dictionary<string, string>();
      if (this.Properties.ContainsKey("dialect"))
      {
        foreach (KeyValuePair<string, string> defaultProperty in (IEnumerable<KeyValuePair<string, string>>) NHibernate.Dialect.Dialect.GetDialect(this.Properties).DefaultProperties)
          derivedProperties[defaultProperty.Key] = defaultProperty.Value;
      }
      foreach (KeyValuePair<string, string> property in (IEnumerable<KeyValuePair<string, string>>) this.Properties)
        derivedProperties[property.Key] = property.Value;
      return derivedProperties;
    }

    public NHibernate.Cfg.Configuration SetDefaultAssembly(string newDefaultAssembly)
    {
      this.defaultAssembly = newDefaultAssembly;
      return this;
    }

    public NHibernate.Cfg.Configuration SetDefaultNamespace(string newDefaultNamespace)
    {
      this.defaultNamespace = newDefaultNamespace;
      return this;
    }

    public NHibernate.Cfg.Configuration SetInterceptor(IInterceptor newInterceptor)
    {
      this.interceptor = newInterceptor;
      return this;
    }

    public NHibernate.Cfg.Configuration SetProperties(IDictionary<string, string> newProperties)
    {
      this.properties = newProperties;
      return this;
    }

    public NHibernate.Cfg.Configuration AddProperties(
      IDictionary<string, string> additionalProperties)
    {
      foreach (KeyValuePair<string, string> additionalProperty in (IEnumerable<KeyValuePair<string, string>>) additionalProperties)
        this.properties.Add(additionalProperty.Key, additionalProperty.Value);
      return this;
    }

    public NHibernate.Cfg.Configuration SetProperty(string name, string value)
    {
      this.properties[name] = value;
      return this;
    }

    public string GetProperty(string name)
    {
      return PropertiesHelper.GetString(name, this.properties, (string) null);
    }

    private void AddProperties(ISessionFactoryConfiguration factoryConfiguration)
    {
      foreach (KeyValuePair<string, string> property in (IEnumerable<KeyValuePair<string, string>>) factoryConfiguration.Properties)
      {
        if (NHibernate.Cfg.Configuration.log.IsDebugEnabled)
          NHibernate.Cfg.Configuration.log.Debug((object) (property.Key + "=" + property.Value));
        this.properties[property.Key] = property.Value;
      }
      Environment.VerifyProperties(this.properties);
    }

    public NHibernate.Cfg.Configuration Configure()
    {
      return ConfigurationManager.GetSection("hibernate-configuration") is IHibernateConfiguration section && section.SessionFactory != null ? this.DoConfigure(section.SessionFactory) : this.Configure(this.GetDefaultConfigurationFilePath());
    }

    public NHibernate.Cfg.Configuration Configure(string fileName)
    {
      return this.Configure(fileName, false);
    }

    private NHibernate.Cfg.Configuration Configure(string fileName, bool ignoreSessionFactoryConfig)
    {
      if (ignoreSessionFactoryConfig)
      {
        Environment.ResetSessionFactoryProperties();
        this.properties = Environment.Properties;
      }
      XmlTextReader textReader = (XmlTextReader) null;
      try
      {
        textReader = new XmlTextReader(fileName);
        return this.Configure((XmlReader) textReader);
      }
      finally
      {
        textReader?.Close();
      }
    }

    public NHibernate.Cfg.Configuration Configure(Assembly assembly, string resourceName)
    {
      if (assembly == null)
        throw new HibernateException("Could not configure NHibernate.", (Exception) new ArgumentNullException(nameof (assembly)));
      if (resourceName == null)
        throw new HibernateException("Could not configure NHibernate.", (Exception) new ArgumentNullException(nameof (resourceName)));
      Stream input = (Stream) null;
      try
      {
        input = assembly.GetManifestResourceStream(resourceName);
        return input != null ? this.Configure((XmlReader) new XmlTextReader(input)) : throw new HibernateException("A ManifestResourceStream could not be created for the resource " + resourceName + " in Assembly " + assembly.FullName);
      }
      finally
      {
        input?.Close();
      }
    }

    public NHibernate.Cfg.Configuration Configure(XmlReader textReader)
    {
      if (textReader == null)
        throw new HibernateConfigException("Could not configure NHibernate.", (Exception) new ArgumentException("A null value was passed in.", nameof (textReader)));
      try
      {
        return this.DoConfigure(new HibernateConfiguration(textReader).SessionFactory);
      }
      catch (Exception ex)
      {
        NHibernate.Cfg.Configuration.log.Error((object) "Problem parsing configuration", ex);
        throw;
      }
    }

    protected NHibernate.Cfg.Configuration DoConfigure(
      ISessionFactoryConfiguration factoryConfiguration)
    {
      if (!string.IsNullOrEmpty(factoryConfiguration.Name))
        this.properties["session_factory_name"] = factoryConfiguration.Name;
      this.AddProperties(factoryConfiguration);
      foreach (MappingConfiguration mapping in (IEnumerable<MappingConfiguration>) factoryConfiguration.Mappings)
      {
        if (mapping.IsEmpty())
          throw new HibernateConfigException("<mapping> element in configuration specifies no attributes");
        if (!string.IsNullOrEmpty(mapping.Resource) && !string.IsNullOrEmpty(mapping.Assembly))
        {
          NHibernate.Cfg.Configuration.log.Debug((object) (factoryConfiguration.Name + "<-" + mapping.Resource + " in " + mapping.Assembly));
          this.AddResource(mapping.Resource, Assembly.Load(mapping.Assembly));
        }
        else if (!string.IsNullOrEmpty(mapping.Assembly))
        {
          NHibernate.Cfg.Configuration.log.Debug((object) (factoryConfiguration.Name + "<-" + mapping.Assembly));
          this.AddAssembly(mapping.Assembly);
        }
        else if (!string.IsNullOrEmpty(mapping.File))
        {
          NHibernate.Cfg.Configuration.log.Debug((object) (factoryConfiguration.Name + "<-" + mapping.File));
          this.AddFile(mapping.File);
        }
      }
      foreach (ClassCacheConfiguration cacheConfiguration in (IEnumerable<ClassCacheConfiguration>) factoryConfiguration.ClassesCache)
      {
        string region = string.IsNullOrEmpty(cacheConfiguration.Region) ? cacheConfiguration.Class : cacheConfiguration.Region;
        bool includeLazy = cacheConfiguration.Include != ClassCacheInclude.NonLazy;
        this.SetCacheConcurrencyStrategy(cacheConfiguration.Class, EntityCacheUsageParser.ToString(cacheConfiguration.Usage), region, includeLazy);
      }
      foreach (CollectionCacheConfiguration cacheConfiguration in (IEnumerable<CollectionCacheConfiguration>) factoryConfiguration.CollectionsCache)
      {
        string collection = cacheConfiguration.Collection;
        if (this.GetCollectionMapping(collection) == null)
          throw new HibernateConfigException("collection-cache Configuration: Cannot configure cache for unknown collection role " + collection);
        string region = string.IsNullOrEmpty(cacheConfiguration.Region) ? collection : cacheConfiguration.Region;
        this.SetCollectionCacheConcurrencyStrategy(collection, EntityCacheUsageParser.ToString(cacheConfiguration.Usage), region);
      }
      foreach (EventConfiguration eventConfiguration in (IEnumerable<EventConfiguration>) factoryConfiguration.Events)
      {
        string[] strArray = new string[eventConfiguration.Listeners.Count];
        for (int index = 0; index < eventConfiguration.Listeners.Count; ++index)
          strArray[index] = eventConfiguration.Listeners[index].Class;
        NHibernate.Cfg.Configuration.log.Debug((object) ("Event listeners: " + (object) eventConfiguration.Type + "=" + StringHelper.ToString((object[]) strArray)));
        this.SetListeners(eventConfiguration.Type, strArray);
      }
      foreach (ListenerConfiguration listener in (IEnumerable<ListenerConfiguration>) factoryConfiguration.Listeners)
      {
        NHibernate.Cfg.Configuration.log.Debug((object) ("Event listener: " + (object) listener.Type + "=" + listener.Class));
        this.SetListeners(listener.Type, new string[1]
        {
          listener.Class
        });
      }
      if (!string.IsNullOrEmpty(factoryConfiguration.Name))
        NHibernate.Cfg.Configuration.log.Info((object) ("Configured SessionFactory: " + factoryConfiguration.Name));
      NHibernate.Cfg.Configuration.log.Debug((object) ("properties: " + (object) this.properties));
      return this;
    }

    internal RootClass GetRootClassMapping(string clazz)
    {
      try
      {
        return (RootClass) this.GetClassMapping(clazz);
      }
      catch (InvalidCastException ex)
      {
        throw new HibernateConfigException("class-cache Configuration: You may only specify a cache for root <class> mappings (cache was specified for " + clazz + ")");
      }
    }

    internal RootClass GetRootClassMapping(System.Type clazz)
    {
      PersistentClass classMapping = this.GetClassMapping(clazz);
      if (classMapping == null)
        throw new HibernateConfigException("class-cache Configuration: Cache specified for unmapped class " + (object) clazz);
      return classMapping is RootClass rootClass ? rootClass : throw new HibernateConfigException("class-cache Configuration: You may only specify a cache for root <class> mappings (cache was specified for " + (object) clazz + ")");
    }

    public NHibernate.Cfg.Configuration SetCacheConcurrencyStrategy(
      string clazz,
      string concurrencyStrategy)
    {
      this.SetCacheConcurrencyStrategy(clazz, concurrencyStrategy, clazz);
      return this;
    }

    public void SetCacheConcurrencyStrategy(
      string clazz,
      string concurrencyStrategy,
      string region)
    {
      this.SetCacheConcurrencyStrategy(clazz, concurrencyStrategy, region, true);
    }

    internal void SetCacheConcurrencyStrategy(
      string clazz,
      string concurrencyStrategy,
      string region,
      bool includeLazy)
    {
      RootClass rootClassMapping = this.GetRootClassMapping(StringHelper.GetFullClassname(clazz));
      if (rootClassMapping == null)
        throw new HibernateConfigException("Cannot cache an unknown entity: " + clazz);
      rootClassMapping.CacheConcurrencyStrategy = concurrencyStrategy;
      rootClassMapping.CacheRegionName = region;
      rootClassMapping.SetLazyPropertiesCacheable(includeLazy);
    }

    public NHibernate.Cfg.Configuration SetCollectionCacheConcurrencyStrategy(
      string collectionRole,
      string concurrencyStrategy)
    {
      this.SetCollectionCacheConcurrencyStrategy(collectionRole, concurrencyStrategy, collectionRole);
      return this;
    }

    internal void SetCollectionCacheConcurrencyStrategy(
      string collectionRole,
      string concurrencyStrategy,
      string region)
    {
      Collection collectionMapping = this.GetCollectionMapping(collectionRole);
      collectionMapping.CacheConcurrencyStrategy = concurrencyStrategy;
      collectionMapping.CacheRegionName = region;
    }

    public IDictionary<string, string> Imports { get; protected set; }

    private Settings BuildSettings()
    {
      Settings settings = this.settingsFactory.BuildSettings(this.GetDerivedProperties());
      PersistentIdGeneratorParmsNames.SqlStatementLogger.FormatSql = settings.SqlStatementLogger.FormatSql;
      PersistentIdGeneratorParmsNames.SqlStatementLogger.LogToStdout = settings.SqlStatementLogger.LogToStdout;
      return settings;
    }

    public IDictionary<string, NamedSQLQueryDefinition> NamedSQLQueries { get; protected set; }

    public INamingStrategy NamingStrategy => this.namingStrategy;

    public NHibernate.Cfg.Configuration SetNamingStrategy(INamingStrategy newNamingStrategy)
    {
      this.namingStrategy = newNamingStrategy;
      return this;
    }

    public IDictionary<string, ResultSetMappingDefinition> SqlResultSetMappings { get; protected set; }

    public IDictionary<string, FilterDefinition> FilterDefinitions { get; protected set; }

    public void AddFilterDefinition(FilterDefinition definition)
    {
      this.FilterDefinitions.Add(definition.FilterName, definition);
    }

    public void AddAuxiliaryDatabaseObject(IAuxiliaryDatabaseObject obj)
    {
      this.auxiliaryDatabaseObjects.Add(obj);
    }

    public IDictionary<string, ISQLFunction> SqlFunctions { get; protected set; }

    public void AddSqlFunction(string functionName, ISQLFunction sqlFunction)
    {
      this.SqlFunctions[functionName] = sqlFunction;
    }

    public NamedXmlDocument LoadMappingDocument(XmlReader hbmReader, string name)
    {
      XmlReaderSettings mappingReaderSettings = this.Schemas.CreateMappingReaderSettings();
      mappingReaderSettings.ValidationEventHandler += new ValidationEventHandler(this.ValidationHandler);
      using (XmlReader reader = XmlReader.Create(hbmReader, mappingReaderSettings))
      {
        this.currentDocumentName = name;
        try
        {
          XmlDocument document = new XmlDocument();
          document.Load(reader);
          return new NamedXmlDocument(name, document, NHibernate.Cfg.Configuration.mappingDocumentSerializer);
        }
        catch (MappingException ex)
        {
          throw;
        }
        catch (Exception ex)
        {
          NHibernate.Cfg.Configuration.LogAndThrow((Exception) new MappingException("Could not compile the mapping document: " + (name ?? "(unknown)"), ex));
        }
        finally
        {
          this.currentDocumentName = (string) null;
        }
      }
      return (NamedXmlDocument) null;
    }

    public NHibernate.Cfg.Configuration AddXmlReader(XmlReader hbmReader)
    {
      return this.AddXmlReader(hbmReader, (string) null);
    }

    public NHibernate.Cfg.Configuration AddXmlReader(XmlReader hbmReader, string name)
    {
      this.AddDocumentThroughQueue(this.LoadMappingDocument(hbmReader, name));
      return this;
    }

    private void AddDocumentThroughQueue(NamedXmlDocument document)
    {
      this.mappingsQueue.AddDocument(document);
      this.ProcessMappingsQueue();
    }

    private void ProcessMappingsQueue()
    {
      NamedXmlDocument availableResource;
      while ((availableResource = this.mappingsQueue.GetNextAvailableResource()) != null)
        this.AddValidatedDocument(availableResource);
    }

    private void ValidationHandler(object o, ValidationEventArgs args)
    {
      NHibernate.Cfg.Configuration.LogAndThrow((Exception) new MappingException(string.Format("{0}({1},{2}): XML validation error: {3}", (object) this.currentDocumentName, (object) args.Exception.LineNumber, (object) args.Exception.LinePosition, (object) args.Exception.Message), (Exception) args.Exception));
    }

    protected virtual string GetDefaultConfigurationFilePath()
    {
      return Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ((IEnumerable<string>) (AppDomain.CurrentDomain.RelativeSearchPath ?? string.Empty).Split(';')).First<string>()), "hibernate.cfg.xml");
    }

    private XmlSchemas Schemas
    {
      get => this.schemas = this.schemas ?? new XmlSchemas();
      set => this.schemas = value;
    }

    public void SetListeners(ListenerType type, string[] listenerClasses)
    {
      if (listenerClasses == null || listenerClasses.Length == 0)
      {
        this.ClearListeners(type);
      }
      else
      {
        object[] instance = (object[]) System.Array.CreateInstance(this.eventListeners.GetListenerClassFor(type), listenerClasses.Length);
        for (int index = 0; index < instance.Length; ++index)
        {
          try
          {
            instance[index] = Environment.BytecodeProvider.ObjectsFactory.CreateInstance(ReflectHelper.ClassForName(listenerClasses[index]));
          }
          catch (Exception ex)
          {
            throw new MappingException("Unable to instantiate specified event (" + (object) type + ") listener class: " + listenerClasses[index], ex);
          }
        }
        this.SetListeners(type, instance);
      }
    }

    public void SetListener(ListenerType type, object listener)
    {
      if (listener == null)
      {
        this.ClearListeners(type);
      }
      else
      {
        object[] instance = (object[]) System.Array.CreateInstance(this.eventListeners.GetListenerClassFor(type), 1);
        instance[0] = listener;
        this.SetListeners(type, instance);
      }
    }

    private void ClearListeners(ListenerType type)
    {
      switch (type)
      {
        case ListenerType.Autoflush:
          this.eventListeners.AutoFlushEventListeners = new IAutoFlushEventListener[0];
          break;
        case ListenerType.Merge:
          this.eventListeners.MergeEventListeners = new IMergeEventListener[0];
          break;
        case ListenerType.Create:
          this.eventListeners.PersistEventListeners = new IPersistEventListener[0];
          break;
        case ListenerType.CreateOnFlush:
          this.eventListeners.PersistOnFlushEventListeners = new IPersistEventListener[0];
          break;
        case ListenerType.Delete:
          this.eventListeners.DeleteEventListeners = new IDeleteEventListener[0];
          break;
        case ListenerType.DirtyCheck:
          this.eventListeners.DirtyCheckEventListeners = new IDirtyCheckEventListener[0];
          break;
        case ListenerType.Evict:
          this.eventListeners.EvictEventListeners = new IEvictEventListener[0];
          break;
        case ListenerType.Flush:
          this.eventListeners.FlushEventListeners = new IFlushEventListener[0];
          break;
        case ListenerType.FlushEntity:
          this.eventListeners.FlushEntityEventListeners = new IFlushEntityEventListener[0];
          break;
        case ListenerType.Load:
          this.eventListeners.LoadEventListeners = new ILoadEventListener[0];
          break;
        case ListenerType.LoadCollection:
          this.eventListeners.InitializeCollectionEventListeners = new IInitializeCollectionEventListener[0];
          break;
        case ListenerType.Lock:
          this.eventListeners.LockEventListeners = new ILockEventListener[0];
          break;
        case ListenerType.Refresh:
          this.eventListeners.RefreshEventListeners = new IRefreshEventListener[0];
          break;
        case ListenerType.Replicate:
          this.eventListeners.ReplicateEventListeners = new IReplicateEventListener[0];
          break;
        case ListenerType.SaveUpdate:
          this.eventListeners.SaveOrUpdateEventListeners = new ISaveOrUpdateEventListener[0];
          break;
        case ListenerType.Save:
          this.eventListeners.SaveEventListeners = new ISaveOrUpdateEventListener[0];
          break;
        case ListenerType.PreUpdate:
          this.eventListeners.PreUpdateEventListeners = new IPreUpdateEventListener[0];
          break;
        case ListenerType.Update:
          this.eventListeners.UpdateEventListeners = new ISaveOrUpdateEventListener[0];
          break;
        case ListenerType.PreLoad:
          this.eventListeners.PreLoadEventListeners = new IPreLoadEventListener[0];
          break;
        case ListenerType.PreDelete:
          this.eventListeners.PreDeleteEventListeners = new IPreDeleteEventListener[0];
          break;
        case ListenerType.PreInsert:
          this.eventListeners.PreInsertEventListeners = new IPreInsertEventListener[0];
          break;
        case ListenerType.PreCollectionRecreate:
          this.eventListeners.PreCollectionRecreateEventListeners = new IPreCollectionRecreateEventListener[0];
          break;
        case ListenerType.PreCollectionRemove:
          this.eventListeners.PreCollectionRemoveEventListeners = new IPreCollectionRemoveEventListener[0];
          break;
        case ListenerType.PreCollectionUpdate:
          this.eventListeners.PreCollectionUpdateEventListeners = new IPreCollectionUpdateEventListener[0];
          break;
        case ListenerType.PostLoad:
          this.eventListeners.PostLoadEventListeners = new IPostLoadEventListener[0];
          break;
        case ListenerType.PostInsert:
          this.eventListeners.PostInsertEventListeners = new IPostInsertEventListener[0];
          break;
        case ListenerType.PostUpdate:
          this.eventListeners.PostUpdateEventListeners = new IPostUpdateEventListener[0];
          break;
        case ListenerType.PostDelete:
          this.eventListeners.PostDeleteEventListeners = new IPostDeleteEventListener[0];
          break;
        case ListenerType.PostCommitUpdate:
          this.eventListeners.PostCommitUpdateEventListeners = new IPostUpdateEventListener[0];
          break;
        case ListenerType.PostCommitInsert:
          this.eventListeners.PostCommitInsertEventListeners = new IPostInsertEventListener[0];
          break;
        case ListenerType.PostCommitDelete:
          this.eventListeners.PostCommitDeleteEventListeners = new IPostDeleteEventListener[0];
          break;
        case ListenerType.PostCollectionRecreate:
          this.eventListeners.PostCollectionRecreateEventListeners = new IPostCollectionRecreateEventListener[0];
          break;
        case ListenerType.PostCollectionRemove:
          this.eventListeners.PostCollectionRemoveEventListeners = new IPostCollectionRemoveEventListener[0];
          break;
        case ListenerType.PostCollectionUpdate:
          this.eventListeners.PostCollectionUpdateEventListeners = new IPostCollectionUpdateEventListener[0];
          break;
        default:
          NHibernate.Cfg.Configuration.log.Warn((object) ("Unrecognized listener type [" + (object) type + "]"));
          break;
      }
    }

    public void SetListeners(ListenerType type, object[] listeners)
    {
      if (listeners == null)
        this.ClearListeners(type);
      switch (type)
      {
        case ListenerType.Autoflush:
          this.eventListeners.AutoFlushEventListeners = (IAutoFlushEventListener[]) listeners;
          break;
        case ListenerType.Merge:
          this.eventListeners.MergeEventListeners = (IMergeEventListener[]) listeners;
          break;
        case ListenerType.Create:
          this.eventListeners.PersistEventListeners = (IPersistEventListener[]) listeners;
          break;
        case ListenerType.CreateOnFlush:
          this.eventListeners.PersistOnFlushEventListeners = (IPersistEventListener[]) listeners;
          break;
        case ListenerType.Delete:
          this.eventListeners.DeleteEventListeners = (IDeleteEventListener[]) listeners;
          break;
        case ListenerType.DirtyCheck:
          this.eventListeners.DirtyCheckEventListeners = (IDirtyCheckEventListener[]) listeners;
          break;
        case ListenerType.Evict:
          this.eventListeners.EvictEventListeners = (IEvictEventListener[]) listeners;
          break;
        case ListenerType.Flush:
          this.eventListeners.FlushEventListeners = (IFlushEventListener[]) listeners;
          break;
        case ListenerType.FlushEntity:
          this.eventListeners.FlushEntityEventListeners = (IFlushEntityEventListener[]) listeners;
          break;
        case ListenerType.Load:
          this.eventListeners.LoadEventListeners = (ILoadEventListener[]) listeners;
          break;
        case ListenerType.LoadCollection:
          this.eventListeners.InitializeCollectionEventListeners = (IInitializeCollectionEventListener[]) listeners;
          break;
        case ListenerType.Lock:
          this.eventListeners.LockEventListeners = (ILockEventListener[]) listeners;
          break;
        case ListenerType.Refresh:
          this.eventListeners.RefreshEventListeners = (IRefreshEventListener[]) listeners;
          break;
        case ListenerType.Replicate:
          this.eventListeners.ReplicateEventListeners = (IReplicateEventListener[]) listeners;
          break;
        case ListenerType.SaveUpdate:
          this.eventListeners.SaveOrUpdateEventListeners = (ISaveOrUpdateEventListener[]) listeners;
          break;
        case ListenerType.Save:
          this.eventListeners.SaveEventListeners = (ISaveOrUpdateEventListener[]) listeners;
          break;
        case ListenerType.PreUpdate:
          this.eventListeners.PreUpdateEventListeners = (IPreUpdateEventListener[]) listeners;
          break;
        case ListenerType.Update:
          this.eventListeners.UpdateEventListeners = (ISaveOrUpdateEventListener[]) listeners;
          break;
        case ListenerType.PreLoad:
          this.eventListeners.PreLoadEventListeners = (IPreLoadEventListener[]) listeners;
          break;
        case ListenerType.PreDelete:
          this.eventListeners.PreDeleteEventListeners = (IPreDeleteEventListener[]) listeners;
          break;
        case ListenerType.PreInsert:
          this.eventListeners.PreInsertEventListeners = (IPreInsertEventListener[]) listeners;
          break;
        case ListenerType.PreCollectionRecreate:
          this.eventListeners.PreCollectionRecreateEventListeners = (IPreCollectionRecreateEventListener[]) listeners;
          break;
        case ListenerType.PreCollectionRemove:
          this.eventListeners.PreCollectionRemoveEventListeners = (IPreCollectionRemoveEventListener[]) listeners;
          break;
        case ListenerType.PreCollectionUpdate:
          this.eventListeners.PreCollectionUpdateEventListeners = (IPreCollectionUpdateEventListener[]) listeners;
          break;
        case ListenerType.PostLoad:
          this.eventListeners.PostLoadEventListeners = (IPostLoadEventListener[]) listeners;
          break;
        case ListenerType.PostInsert:
          this.eventListeners.PostInsertEventListeners = (IPostInsertEventListener[]) listeners;
          break;
        case ListenerType.PostUpdate:
          this.eventListeners.PostUpdateEventListeners = (IPostUpdateEventListener[]) listeners;
          break;
        case ListenerType.PostDelete:
          this.eventListeners.PostDeleteEventListeners = (IPostDeleteEventListener[]) listeners;
          break;
        case ListenerType.PostCommitUpdate:
          this.eventListeners.PostCommitUpdateEventListeners = (IPostUpdateEventListener[]) listeners;
          break;
        case ListenerType.PostCommitInsert:
          this.eventListeners.PostCommitInsertEventListeners = (IPostInsertEventListener[]) listeners;
          break;
        case ListenerType.PostCommitDelete:
          this.eventListeners.PostCommitDeleteEventListeners = (IPostDeleteEventListener[]) listeners;
          break;
        case ListenerType.PostCollectionRecreate:
          this.eventListeners.PostCollectionRecreateEventListeners = (IPostCollectionRecreateEventListener[]) listeners;
          break;
        case ListenerType.PostCollectionRemove:
          this.eventListeners.PostCollectionRemoveEventListeners = (IPostCollectionRemoveEventListener[]) listeners;
          break;
        case ListenerType.PostCollectionUpdate:
          this.eventListeners.PostCollectionUpdateEventListeners = (IPostCollectionUpdateEventListener[]) listeners;
          break;
        default:
          NHibernate.Cfg.Configuration.log.Warn((object) ("Unrecognized listener type [" + (object) type + "]"));
          break;
      }
    }

    public void AppendListeners(ListenerType type, object[] listeners)
    {
      switch (type)
      {
        case ListenerType.Autoflush:
          this.eventListeners.AutoFlushEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IAutoFlushEventListener>(this.eventListeners.AutoFlushEventListeners, (IAutoFlushEventListener[]) listeners);
          break;
        case ListenerType.Merge:
          this.eventListeners.MergeEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IMergeEventListener>(this.eventListeners.MergeEventListeners, (IMergeEventListener[]) listeners);
          break;
        case ListenerType.Create:
          this.eventListeners.PersistEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IPersistEventListener>(this.eventListeners.PersistEventListeners, (IPersistEventListener[]) listeners);
          break;
        case ListenerType.CreateOnFlush:
          this.eventListeners.PersistOnFlushEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IPersistEventListener>(this.eventListeners.PersistOnFlushEventListeners, (IPersistEventListener[]) listeners);
          break;
        case ListenerType.Delete:
          this.eventListeners.DeleteEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IDeleteEventListener>(this.eventListeners.DeleteEventListeners, (IDeleteEventListener[]) listeners);
          break;
        case ListenerType.DirtyCheck:
          this.eventListeners.DirtyCheckEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IDirtyCheckEventListener>(this.eventListeners.DirtyCheckEventListeners, (IDirtyCheckEventListener[]) listeners);
          break;
        case ListenerType.Evict:
          this.eventListeners.EvictEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IEvictEventListener>(this.eventListeners.EvictEventListeners, (IEvictEventListener[]) listeners);
          break;
        case ListenerType.Flush:
          this.eventListeners.FlushEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IFlushEventListener>(this.eventListeners.FlushEventListeners, (IFlushEventListener[]) listeners);
          break;
        case ListenerType.FlushEntity:
          this.eventListeners.FlushEntityEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IFlushEntityEventListener>(this.eventListeners.FlushEntityEventListeners, (IFlushEntityEventListener[]) listeners);
          break;
        case ListenerType.Load:
          this.eventListeners.LoadEventListeners = NHibernate.Cfg.Configuration.AppendListeners<ILoadEventListener>(this.eventListeners.LoadEventListeners, (ILoadEventListener[]) listeners);
          break;
        case ListenerType.LoadCollection:
          this.eventListeners.InitializeCollectionEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IInitializeCollectionEventListener>(this.eventListeners.InitializeCollectionEventListeners, (IInitializeCollectionEventListener[]) listeners);
          break;
        case ListenerType.Lock:
          this.eventListeners.LockEventListeners = NHibernate.Cfg.Configuration.AppendListeners<ILockEventListener>(this.eventListeners.LockEventListeners, (ILockEventListener[]) listeners);
          break;
        case ListenerType.Refresh:
          this.eventListeners.RefreshEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IRefreshEventListener>(this.eventListeners.RefreshEventListeners, (IRefreshEventListener[]) listeners);
          break;
        case ListenerType.Replicate:
          this.eventListeners.ReplicateEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IReplicateEventListener>(this.eventListeners.ReplicateEventListeners, (IReplicateEventListener[]) listeners);
          break;
        case ListenerType.SaveUpdate:
          this.eventListeners.SaveOrUpdateEventListeners = NHibernate.Cfg.Configuration.AppendListeners<ISaveOrUpdateEventListener>(this.eventListeners.SaveOrUpdateEventListeners, (ISaveOrUpdateEventListener[]) listeners);
          break;
        case ListenerType.Save:
          this.eventListeners.SaveEventListeners = NHibernate.Cfg.Configuration.AppendListeners<ISaveOrUpdateEventListener>(this.eventListeners.SaveEventListeners, (ISaveOrUpdateEventListener[]) listeners);
          break;
        case ListenerType.PreUpdate:
          this.eventListeners.PreUpdateEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IPreUpdateEventListener>(this.eventListeners.PreUpdateEventListeners, (IPreUpdateEventListener[]) listeners);
          break;
        case ListenerType.Update:
          this.eventListeners.UpdateEventListeners = NHibernate.Cfg.Configuration.AppendListeners<ISaveOrUpdateEventListener>(this.eventListeners.UpdateEventListeners, (ISaveOrUpdateEventListener[]) listeners);
          break;
        case ListenerType.PreLoad:
          this.eventListeners.PreLoadEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IPreLoadEventListener>(this.eventListeners.PreLoadEventListeners, (IPreLoadEventListener[]) listeners);
          break;
        case ListenerType.PreDelete:
          this.eventListeners.PreDeleteEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IPreDeleteEventListener>(this.eventListeners.PreDeleteEventListeners, (IPreDeleteEventListener[]) listeners);
          break;
        case ListenerType.PreInsert:
          this.eventListeners.PreInsertEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IPreInsertEventListener>(this.eventListeners.PreInsertEventListeners, (IPreInsertEventListener[]) listeners);
          break;
        case ListenerType.PreCollectionRecreate:
          this.eventListeners.PreCollectionRecreateEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IPreCollectionRecreateEventListener>(this.eventListeners.PreCollectionRecreateEventListeners, (IPreCollectionRecreateEventListener[]) listeners);
          break;
        case ListenerType.PreCollectionRemove:
          this.eventListeners.PreCollectionRemoveEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IPreCollectionRemoveEventListener>(this.eventListeners.PreCollectionRemoveEventListeners, (IPreCollectionRemoveEventListener[]) listeners);
          break;
        case ListenerType.PreCollectionUpdate:
          this.eventListeners.PreCollectionUpdateEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IPreCollectionUpdateEventListener>(this.eventListeners.PreCollectionUpdateEventListeners, (IPreCollectionUpdateEventListener[]) listeners);
          break;
        case ListenerType.PostLoad:
          this.eventListeners.PostLoadEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IPostLoadEventListener>(this.eventListeners.PostLoadEventListeners, (IPostLoadEventListener[]) listeners);
          break;
        case ListenerType.PostInsert:
          this.eventListeners.PostInsertEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IPostInsertEventListener>(this.eventListeners.PostInsertEventListeners, (IPostInsertEventListener[]) listeners);
          break;
        case ListenerType.PostUpdate:
          this.eventListeners.PostUpdateEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IPostUpdateEventListener>(this.eventListeners.PostUpdateEventListeners, (IPostUpdateEventListener[]) listeners);
          break;
        case ListenerType.PostDelete:
          this.eventListeners.PostDeleteEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IPostDeleteEventListener>(this.eventListeners.PostDeleteEventListeners, (IPostDeleteEventListener[]) listeners);
          break;
        case ListenerType.PostCommitUpdate:
          this.eventListeners.PostCommitUpdateEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IPostUpdateEventListener>(this.eventListeners.PostCommitUpdateEventListeners, (IPostUpdateEventListener[]) listeners);
          break;
        case ListenerType.PostCommitInsert:
          this.eventListeners.PostCommitInsertEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IPostInsertEventListener>(this.eventListeners.PostCommitInsertEventListeners, (IPostInsertEventListener[]) listeners);
          break;
        case ListenerType.PostCommitDelete:
          this.eventListeners.PostCommitDeleteEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IPostDeleteEventListener>(this.eventListeners.PostCommitDeleteEventListeners, (IPostDeleteEventListener[]) listeners);
          break;
        case ListenerType.PostCollectionRecreate:
          this.eventListeners.PostCollectionRecreateEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IPostCollectionRecreateEventListener>(this.eventListeners.PostCollectionRecreateEventListeners, (IPostCollectionRecreateEventListener[]) listeners);
          break;
        case ListenerType.PostCollectionRemove:
          this.eventListeners.PostCollectionRemoveEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IPostCollectionRemoveEventListener>(this.eventListeners.PostCollectionRemoveEventListeners, (IPostCollectionRemoveEventListener[]) listeners);
          break;
        case ListenerType.PostCollectionUpdate:
          this.eventListeners.PostCollectionUpdateEventListeners = NHibernate.Cfg.Configuration.AppendListeners<IPostCollectionUpdateEventListener>(this.eventListeners.PostCollectionUpdateEventListeners, (IPostCollectionUpdateEventListener[]) listeners);
          break;
        default:
          NHibernate.Cfg.Configuration.log.Warn((object) ("Unrecognized listener type [" + (object) type + "]"));
          break;
      }
    }

    private static T[] AppendListeners<T>(T[] existing, T[] listenersToAdd)
    {
      List<T> objList = new List<T>((IEnumerable<T>) (existing ?? new T[0]));
      objList.AddRange((IEnumerable<T>) listenersToAdd);
      return objList.ToArray();
    }

    public string[] GenerateSchemaUpdateScript(NHibernate.Dialect.Dialect dialect, DatabaseMetadata databaseMetadata)
    {
      this.SecondPassCompile();
      string defaultCatalog = PropertiesHelper.GetString("default_catalog", this.properties, (string) null);
      string defaultSchema = PropertiesHelper.GetString("default_schema", this.properties, (string) null);
      List<string> stringList = new List<string>(50);
      foreach (Table tableMapping in (IEnumerable<Table>) this.TableMappings)
      {
        if (tableMapping.IsPhysicalTable && NHibernate.Cfg.Configuration.IncludeAction(tableMapping.SchemaActions, SchemaAction.Update))
        {
          ITableMetadata tableMetadata = databaseMetadata.GetTableMetadata(tableMapping.Name, tableMapping.Schema ?? defaultSchema, tableMapping.Catalog ?? defaultCatalog, tableMapping.IsQuoted);
          if (tableMetadata == null)
          {
            stringList.Add(tableMapping.SqlCreateString(dialect, this.mapping, defaultCatalog, defaultSchema));
          }
          else
          {
            string[] collection = tableMapping.SqlAlterStrings(dialect, this.mapping, tableMetadata, defaultCatalog, defaultSchema);
            stringList.AddRange((IEnumerable<string>) collection);
          }
          string[] collection1 = tableMapping.SqlCommentStrings(dialect, defaultCatalog, defaultSchema);
          stringList.AddRange((IEnumerable<string>) collection1);
        }
      }
      foreach (Table tableMapping in (IEnumerable<Table>) this.TableMappings)
      {
        if (tableMapping.IsPhysicalTable && NHibernate.Cfg.Configuration.IncludeAction(tableMapping.SchemaActions, SchemaAction.Update))
        {
          ITableMetadata tableMetadata = databaseMetadata.GetTableMetadata(tableMapping.Name, tableMapping.Schema, tableMapping.Catalog, tableMapping.IsQuoted);
          if (dialect.SupportsForeignKeyConstraintInAlterTable)
          {
            foreach (ForeignKey foreignKey in tableMapping.ForeignKeyIterator)
            {
              if (foreignKey.HasPhysicalConstraint && NHibernate.Cfg.Configuration.IncludeAction(foreignKey.ReferencedTable.SchemaActions, SchemaAction.Update) && (tableMetadata == null || tableMetadata.GetForeignKeyMetadata(foreignKey.Name) == null && (!(dialect is MySQLDialect) || tableMetadata.GetIndexMetadata(foreignKey.Name) == null)))
                stringList.Add(foreignKey.SqlCreateString(dialect, this.mapping, defaultCatalog, defaultSchema));
            }
          }
          foreach (Index index in tableMapping.IndexIterator)
          {
            if (tableMetadata == null || tableMetadata.GetIndexMetadata(index.Name) == null)
              stringList.Add(index.SqlCreateString(dialect, this.mapping, defaultCatalog, defaultSchema));
          }
        }
      }
      foreach (IPersistentIdentifierGenerator iterateGenerator in this.IterateGenerators(dialect))
      {
        string key = iterateGenerator.GeneratorKey();
        if (!databaseMetadata.IsSequence((object) key) && !databaseMetadata.IsTable((object) key))
        {
          foreach (string str in iterateGenerator.SqlCreateStrings(dialect))
            stringList.Add(str);
        }
      }
      return stringList.ToArray();
    }

    public void ValidateSchema(NHibernate.Dialect.Dialect dialect, DatabaseMetadata databaseMetadata)
    {
      this.SecondPassCompile();
      string str1 = PropertiesHelper.GetString("default_catalog", this.properties, (string) null);
      string str2 = PropertiesHelper.GetString("default_schema", this.properties, (string) null);
      foreach (Table tableMapping in (IEnumerable<Table>) this.TableMappings)
      {
        if (tableMapping.IsPhysicalTable && NHibernate.Cfg.Configuration.IncludeAction(tableMapping.SchemaActions, SchemaAction.Validate))
          tableMapping.ValidateColumns(dialect, this.mapping, databaseMetadata.GetTableMetadata(tableMapping.Name, tableMapping.Schema ?? str2, tableMapping.Catalog ?? str1, tableMapping.IsQuoted) ?? throw new HibernateException("Missing table: " + tableMapping.Name));
      }
      foreach (IPersistentIdentifierGenerator iterateGenerator in this.IterateGenerators(dialect))
      {
        string key = iterateGenerator.GeneratorKey();
        if (!databaseMetadata.IsSequence((object) key) && !databaseMetadata.IsTable((object) key))
          throw new HibernateException(string.Format("Missing sequence or table: " + key));
      }
    }

    private IEnumerable<IPersistentIdentifierGenerator> IterateGenerators(NHibernate.Dialect.Dialect dialect)
    {
      Dictionary<string, IPersistentIdentifierGenerator> dictionary = new Dictionary<string, IPersistentIdentifierGenerator>();
      string defaultCatalog = PropertiesHelper.GetString("default_catalog", this.properties, (string) null);
      string defaultSchema = PropertiesHelper.GetString("default_schema", this.properties, (string) null);
      foreach (PersistentClass persistentClass in (IEnumerable<PersistentClass>) this.classes.Values)
      {
        if (!persistentClass.IsInherited && persistentClass.Identifier.CreateIdentifierGenerator(dialect, defaultCatalog, defaultSchema, (RootClass) persistentClass) is IPersistentIdentifierGenerator identifierGenerator)
          dictionary[identifierGenerator.GeneratorKey()] = identifierGenerator;
      }
      foreach (Collection collection in (IEnumerable<Collection>) this.collections.Values)
      {
        if (collection.IsIdentified && ((IdentifierCollection) collection).Identifier.CreateIdentifierGenerator(dialect, defaultCatalog, defaultSchema, (RootClass) null) is IPersistentIdentifierGenerator identifierGenerator)
          dictionary[identifierGenerator.GeneratorKey()] = identifierGenerator;
      }
      return (IEnumerable<IPersistentIdentifierGenerator>) dictionary.Values;
    }

    [Serializable]
    private class Mapping : IMapping
    {
      private readonly NHibernate.Cfg.Configuration configuration;

      public Mapping(NHibernate.Cfg.Configuration configuration)
      {
        this.configuration = configuration;
      }

      private PersistentClass GetPersistentClass(string className)
      {
        PersistentClass persistentClass;
        if (!this.configuration.classes.TryGetValue(className, out persistentClass))
          throw new MappingException("persistent class not known: " + className);
        return persistentClass;
      }

      public IType GetIdentifierType(string className)
      {
        return this.GetPersistentClass(className).Identifier.Type;
      }

      public string GetIdentifierPropertyName(string className)
      {
        PersistentClass persistentClass = this.GetPersistentClass(className);
        return !persistentClass.HasIdentifierProperty ? (string) null : persistentClass.IdentifierProperty.Name;
      }

      public IType GetReferencedPropertyType(string className, string propertyName)
      {
        PersistentClass persistentClass = this.GetPersistentClass(className);
        return (persistentClass.GetProperty(propertyName) ?? throw new MappingException("property not known: " + persistentClass.MappedClass.FullName + (object) '.' + propertyName)).Type;
      }

      public bool HasNonIdentifierPropertyNamedId(string className)
      {
        return "id".Equals(this.GetIdentifierPropertyName(className));
      }
    }
  }
}

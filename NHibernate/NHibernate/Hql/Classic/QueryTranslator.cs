// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Classic.QueryTranslator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using Iesi.Collections.Generic;
using NHibernate.Dialect.Function;
using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Event;
using NHibernate.Impl;
using NHibernate.Loader;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

#nullable disable
namespace NHibernate.Hql.Classic
{
  public class QueryTranslator : BasicLoader, IFilterTranslator, IQueryTranslator
  {
    private static readonly string[] NoReturnAliases = new string[0];
    private readonly string queryIdentifier;
    private readonly string queryString;
    private readonly IDictionary<string, string> typeMap = (IDictionary<string, string>) new LinkedHashMap<string, string>();
    private readonly IDictionary<string, string> collections = (IDictionary<string, string>) new LinkedHashMap<string, string>();
    private IList<string> returnedTypes = (IList<string>) new System.Collections.Generic.List<string>();
    private readonly IList<string> fromTypes = (IList<string>) new System.Collections.Generic.List<string>();
    private readonly IList<IType> scalarTypes = (IList<IType>) new System.Collections.Generic.List<IType>();
    private readonly IDictionary<string, System.Collections.Generic.List<int>> namedParameters = (IDictionary<string, System.Collections.Generic.List<int>>) new Dictionary<string, System.Collections.Generic.List<int>>();
    private readonly IDictionary<string, string> aliasNames = (IDictionary<string, string>) new Dictionary<string, string>();
    private readonly IDictionary<string, string> oneToOneOwnerNames = (IDictionary<string, string>) new Dictionary<string, string>();
    private readonly IDictionary<string, IAssociationType> uniqueKeyOwnerReferences = (IDictionary<string, IAssociationType>) new Dictionary<string, IAssociationType>();
    private readonly IDictionary<string, IPropertyMapping> decoratedPropertyMappings = (IDictionary<string, IPropertyMapping>) new Dictionary<string, IPropertyMapping>();
    private readonly IList<SqlString> scalarSelectTokens = (IList<SqlString>) new System.Collections.Generic.List<SqlString>();
    private readonly IList<SqlString> whereTokens = (IList<SqlString>) new System.Collections.Generic.List<SqlString>();
    private readonly IList<SqlString> havingTokens = (IList<SqlString>) new System.Collections.Generic.List<SqlString>();
    private readonly IDictionary<string, JoinSequence> joins = (IDictionary<string, JoinSequence>) new LinkedHashMap<string, JoinSequence>();
    private readonly IList<SqlString> orderByTokens = (IList<SqlString>) new System.Collections.Generic.List<SqlString>();
    private readonly IList<SqlString> groupByTokens = (IList<SqlString>) new System.Collections.Generic.List<SqlString>();
    private readonly ISet<string> querySpaces = (ISet<string>) new HashedSet<string>();
    private readonly ISet<string> entitiesToFetch = (ISet<string>) new HashedSet<string>();
    private readonly IDictionary<string, string> pathAliases = (IDictionary<string, string>) new Dictionary<string, string>();
    private readonly IDictionary<string, JoinSequence> pathJoins = (IDictionary<string, JoinSequence>) new Dictionary<string, JoinSequence>();
    private NHibernate.Persister.Entity.IQueryable[] persisters;
    private int[] owners;
    private EntityType[] ownerAssociationTypes;
    private string[] names;
    private bool[] includeInSelect;
    private int selectLength;
    private IType[] returnTypes;
    private IType[] actualReturnTypes;
    private string[][] scalarColumnNames;
    private IDictionary<string, string> tokenReplacements;
    private int nameCount;
    private int parameterCount;
    private bool distinct;
    private bool compiled;
    private SqlString sqlString;
    private System.Type holderClass;
    private ConstructorInfo holderConstructor;
    private bool hasScalars;
    private bool shallowQuery;
    private QueryTranslator superQuery;
    private IList<IParameterSpecification> collectedParameters = (IList<IParameterSpecification>) new System.Collections.Generic.List<IParameterSpecification>();
    private int positionalParameterFound;
    private readonly QueryTranslator.FetchedCollections fetchedCollections = new QueryTranslator.FetchedCollections();
    private string[] suffixes;
    private IDictionary<string, NHibernate.IFilter> enabledFilters;
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (QueryTranslator));
    private static readonly ISet beforeClassTokens = (ISet) new HashedSet();
    private static readonly ISet notAfterClassTokens = (ISet) new HashedSet();

    public QueryTranslator(
      string queryIdentifier,
      string queryString,
      IDictionary<string, NHibernate.IFilter> enabledFilters,
      ISessionFactoryImplementor factory)
      : base(factory)
    {
      this.queryIdentifier = queryIdentifier;
      this.queryString = queryString;
      this.enabledFilters = enabledFilters;
    }

    public QueryTranslator(
      ISessionFactoryImplementor factory,
      string queryString,
      IDictionary<string, NHibernate.IFilter> enabledFilters)
      : this(queryString, queryString, enabledFilters, factory)
    {
    }

    protected internal void Compile(QueryTranslator superquery)
    {
      this.tokenReplacements = superquery.tokenReplacements;
      this.superQuery = superquery;
      this.shallowQuery = true;
      this.enabledFilters = superquery.EnabledFilters;
      this.Compile();
    }

    private int GetPositionalParameterPosition()
    {
      return this.superQuery == null ? this.positionalParameterFound++ : this.superQuery.GetPositionalParameterPosition();
    }

    public SqlString GetNamedParameter(string name)
    {
      NamedParameterSpecification parameterSpecification = new NamedParameterSpecification(1, 0, name);
      Parameter placeholder = Parameter.Placeholder;
      placeholder.BackTrack = (object) parameterSpecification.GetIdsForBackTrack((IMapping) this.Factory).First<string>();
      this.AddNamedParameter(name);
      this.collectedParameters.Add((IParameterSpecification) parameterSpecification);
      return new SqlString(placeholder);
    }

    public SqlString GetPositionalParameter()
    {
      PositionalParameterSpecification parameterSpecification = new PositionalParameterSpecification(1, 0, this.GetPositionalParameterPosition());
      Parameter placeholder = Parameter.Placeholder;
      placeholder.BackTrack = (object) parameterSpecification.GetIdsForBackTrack((IMapping) this.Factory).First<string>();
      this.collectedParameters.Add((IParameterSpecification) parameterSpecification);
      return new SqlString(placeholder);
    }

    public IEnumerable<IParameterSpecification> CollectedParameterSpecifications
    {
      get
      {
        return (this.superQuery != null ? this.superQuery.CollectedParameterSpecifications : Enumerable.Empty<IParameterSpecification>()).Concat<IParameterSpecification>((IEnumerable<IParameterSpecification>) this.collectedParameters);
      }
    }

    protected override void ResetEffectiveExpectedType(
      IEnumerable<IParameterSpecification> parameterSpecs,
      QueryParameters queryParameters)
    {
      parameterSpecs.ResetEffectiveExpectedType(queryParameters);
    }

    protected override IEnumerable<IParameterSpecification> GetParameterSpecifications()
    {
      return this.CollectedParameterSpecifications;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Compile(IDictionary<string, string> replacements, bool scalar)
    {
      if (this.Compiled)
        return;
      this.tokenReplacements = replacements;
      this.shallowQuery = scalar;
      this.Compile();
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Compile(
      string collectionRole,
      IDictionary<string, string> replacements,
      bool scalar)
    {
      if (this.Compiled)
        return;
      this.AddFromAssociation("this", collectionRole);
      this.Compile(replacements, scalar);
    }

    protected void Compile()
    {
      QueryTranslator.log.Debug((object) "compiling query");
      try
      {
        ParserHelper.Parse((IParser) new PreprocessingParser(this.tokenReplacements), this.queryString, " \n\r\f\t,()=<>&|+-=/*'^![]#~\\;", this);
        this.RenderSql();
      }
      catch (QueryException ex)
      {
        ex.QueryString = this.queryString;
        throw;
      }
      catch (MappingException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        QueryTranslator.log.Debug((object) "unexpected query compilation problem", ex);
        throw new QueryException("Incorrect query syntax", ex)
        {
          QueryString = this.queryString
        };
      }
      this.PostInstantiate();
      this.compiled = true;
    }

    public override ILoadable[] EntityPersisters => (ILoadable[]) this.persisters;

    public virtual IType[] ReturnTypes => this.returnTypes;

    public NHibernate.Loader.Loader Loader => (NHibernate.Loader.Loader) this;

    public virtual IType[] ActualReturnTypes => this.actualReturnTypes;

    public ParameterMetadata BuildParameterMetadata()
    {
      IEnumerable<IParameterSpecification> parameterSpecifications = this.CollectedParameterSpecifications;
      return new ParameterMetadata(parameterSpecifications.OfType<PositionalParameterSpecification>().Select<PositionalParameterSpecification, OrdinalParameterDescriptor>((System.Func<PositionalParameterSpecification, OrdinalParameterDescriptor>) (op => new OrdinalParameterDescriptor(op.HqlPosition, op.ExpectedType))), (IDictionary<string, NamedParameterDescriptor>) parameterSpecifications.OfType<NamedParameterSpecification>().Distinct<NamedParameterSpecification>().Select(np => new
      {
        Name = np.Name,
        Descriptor = new NamedParameterDescriptor(np.Name, np.ExpectedType, false)
      }).ToDictionary(ep => ep.Name, ep => ep.Descriptor));
    }

    public virtual string[][] ScalarColumnNames => this.scalarColumnNames;

    private static void LogQuery(string hql, string sql)
    {
      if (!QueryTranslator.log.IsDebugEnabled)
        return;
      QueryTranslator.log.Debug((object) ("HQL: " + hql));
      QueryTranslator.log.Debug((object) ("SQL: " + sql));
    }

    internal void SetAliasName(string alias, string name) => this.aliasNames.Add(alias, name);

    internal string GetAliasName(string alias)
    {
      string aliasName;
      if (!this.aliasNames.TryGetValue(alias, out aliasName))
        aliasName = this.superQuery == null ? alias : this.superQuery.GetAliasName(alias);
      return aliasName;
    }

    internal string Unalias(string path)
    {
      string alias = StringHelper.Root(path);
      string aliasName = this.GetAliasName(alias);
      return aliasName != null ? aliasName + path.Substring(alias.Length) : path;
    }

    public void AddEntityToFetch(
      string name,
      string oneToOneOwnerName,
      IAssociationType ownerAssociationType)
    {
      this.AddEntityToFetch(name);
      if (oneToOneOwnerName != null)
        this.oneToOneOwnerNames[name] = oneToOneOwnerName;
      if (ownerAssociationType == null)
        return;
      this.uniqueKeyOwnerReferences[name] = ownerAssociationType;
    }

    public void AddEntityToFetch(string name) => this.entitiesToFetch.Add(name);

    [CLSCompliant(false)]
    public override SqlString SqlString => this.sqlString;

    private int NextCount()
    {
      return this.superQuery != null ? this.superQuery.nameCount++ : this.nameCount++;
    }

    internal string CreateNameFor(string type)
    {
      return StringHelper.GenerateAlias(StringHelper.UnqualifyEntityName(type), this.NextCount());
    }

    internal string CreateNameForCollection(string role)
    {
      return StringHelper.GenerateAlias(role, this.NextCount());
    }

    internal string GetType(string name)
    {
      string type;
      if (!this.typeMap.TryGetValue(name, out type) && this.superQuery != null)
        type = this.superQuery.GetType(name);
      return type;
    }

    internal string GetRole(string name)
    {
      string role;
      if (!this.collections.TryGetValue(name, out role) && this.superQuery != null)
        role = this.superQuery.GetRole(name);
      return role;
    }

    internal bool IsName(string name)
    {
      if (this.aliasNames.ContainsKey(name) || this.typeMap.ContainsKey(name) || this.collections.ContainsKey(name))
        return true;
      return this.superQuery != null && this.superQuery.IsName(name);
    }

    public IPropertyMapping GetPropertyMapping(string name)
    {
      IPropertyMapping decoratedPropertyMapping = this.GetDecoratedPropertyMapping(name);
      if (decoratedPropertyMapping != null)
        return decoratedPropertyMapping;
      string type = this.GetType(name);
      if (type == null)
        return (IPropertyMapping) this.GetCollectionPersister(this.GetRole(name) ?? throw new QueryException(string.Format("alias not found: {0}", (object) name)));
      return (IPropertyMapping) (this.GetPersister(type) ?? throw new QueryException(string.Format("Persistent class not found for entity named: {0}", (object) type)));
    }

    public IPropertyMapping GetDecoratedPropertyMapping(string name)
    {
      IPropertyMapping decoratedPropertyMapping;
      this.decoratedPropertyMappings.TryGetValue(name, out decoratedPropertyMapping);
      return decoratedPropertyMapping;
    }

    public void DecoratePropertyMapping(string name, IPropertyMapping mapping)
    {
      this.decoratedPropertyMappings.Add(name, mapping);
    }

    internal NHibernate.Persister.Entity.IQueryable GetPersisterForName(string name)
    {
      string type = this.GetType(name);
      return this.GetPersister(type) ?? throw new QueryException("Persistent class not found for entity named: " + type);
    }

    internal NHibernate.Persister.Entity.IQueryable GetPersisterUsingImports(string className)
    {
      return this.Helper.FindQueryableUsingImports(className);
    }

    internal NHibernate.Persister.Entity.IQueryable GetPersister(string clazz)
    {
      try
      {
        return (NHibernate.Persister.Entity.IQueryable) this.Factory.GetEntityPersister(clazz);
      }
      catch (Exception ex)
      {
        throw new QueryException("Persistent class not found for entity named: " + clazz);
      }
    }

    internal IQueryableCollection GetCollectionPersister(string role)
    {
      try
      {
        return (IQueryableCollection) this.Factory.GetCollectionPersister(role);
      }
      catch (InvalidCastException ex)
      {
        throw new QueryException(string.Format("collection role is not queryable: {0}", (object) role));
      }
      catch (Exception ex)
      {
        throw new QueryException(string.Format("collection role not found: {0}", (object) role));
      }
    }

    internal void AddType(string name, string type) => this.typeMap[name] = type;

    internal void AddCollection(string name, string role) => this.collections[name] = role;

    internal void AddFrom(string name, string type, JoinSequence joinSequence)
    {
      this.AddType(name, type);
      this.AddFrom(name, joinSequence);
    }

    internal void AddFromCollection(string name, string collectionRole, JoinSequence joinSequence)
    {
      this.AddCollection(name, collectionRole);
      this.AddJoin(name, joinSequence);
    }

    internal void AddFrom(string name, JoinSequence joinSequence)
    {
      this.fromTypes.Add(name);
      this.AddJoin(name, joinSequence);
    }

    internal void AddFromClass(string name, NHibernate.Persister.Entity.IQueryable classPersister)
    {
      JoinSequence joinSequence = new JoinSequence(this.Factory).SetRoot((IJoinable) classPersister, name);
      this.AddFrom(name, classPersister.EntityName, joinSequence);
    }

    internal void AddSelectClass(string name) => this.returnedTypes.Add(name);

    internal void AddSelectScalar(IType type) => this.scalarTypes.Add(type);

    internal void AppendWhereToken(SqlString token) => this.whereTokens.Add(token);

    internal void AppendHavingToken(SqlString token) => this.havingTokens.Add(token);

    internal void AppendOrderByToken(string token) => this.orderByTokens.Add(new SqlString(token));

    internal void AppendOrderByParameter(string name)
    {
      this.orderByTokens.Add(this.GetNamedParameter(name));
    }

    internal void AppendOrderByParameter() => this.orderByTokens.Add(this.GetPositionalParameter());

    internal void AppendGroupByToken(string token) => this.groupByTokens.Add(new SqlString(token));

    internal void AppendGroupByParameter(string name)
    {
      this.groupByTokens.Add(this.GetNamedParameter(name));
    }

    internal void AppendGroupByParameter() => this.groupByTokens.Add(this.GetPositionalParameter());

    internal void AppendScalarSelectToken(string token)
    {
      this.scalarSelectTokens.Add(new SqlString(token));
    }

    internal void AppendScalarSelectTokens(string[] tokens)
    {
      this.scalarSelectTokens.Add(new SqlString((object[]) tokens));
    }

    internal void AppendScalarSelectParameter(string name)
    {
      this.scalarSelectTokens.Add(this.GetNamedParameter(name));
    }

    internal void AppendScalarSelectParameter()
    {
      this.scalarSelectTokens.Add(this.GetPositionalParameter());
    }

    internal void AddJoin(string name, JoinSequence joinSequence)
    {
      if (this.joins.ContainsKey(name))
        return;
      this.joins.Add(name, joinSequence);
    }

    internal void AddNamedParameter(string name)
    {
      if (this.superQuery != null)
        this.superQuery.AddNamedParameter(name);
      int num = this.parameterCount++;
      System.Collections.Generic.List<int> intList;
      if (!this.namedParameters.TryGetValue(name, out intList))
        this.namedParameters[name] = new System.Collections.Generic.List<int>(4)
        {
          num
        };
      else
        intList.Add(num);
    }

    public static string ScalarName(int x, int y)
    {
      return new StringBuilder().Append('x').Append(x).Append('_').Append(y).Append('_').ToString();
    }

    private void RenderSql()
    {
      int count1;
      if (this.returnedTypes.Count == 0 && this.scalarTypes.Count == 0)
      {
        this.returnedTypes = this.fromTypes;
        count1 = this.returnedTypes.Count;
      }
      else
      {
        count1 = this.returnedTypes.Count;
        foreach (string str in (IEnumerable<string>) this.entitiesToFetch)
          this.returnedTypes.Add(str);
      }
      int count2 = this.returnedTypes.Count;
      this.persisters = new NHibernate.Persister.Entity.IQueryable[count2];
      this.names = new string[count2];
      this.owners = new int[count2];
      this.ownerAssociationTypes = new EntityType[count2];
      this.suffixes = new string[count2];
      this.includeInSelect = new bool[count2];
      for (int index = 0; index < count2; ++index)
      {
        string returnedType = this.returnedTypes[index];
        this.persisters[index] = this.GetPersisterForName(returnedType);
        this.suffixes[index] = count2 == 1 ? string.Empty : index.ToString() + (object) '_';
        this.names[index] = returnedType;
        this.includeInSelect[index] = !this.entitiesToFetch.Contains(returnedType);
        if (this.includeInSelect[index])
          ++this.selectLength;
        string str;
        this.owners[index] = !this.oneToOneOwnerNames.TryGetValue(returnedType, out str) ? -1 : this.returnedTypes.IndexOf(str);
        IAssociationType associationType;
        if (this.uniqueKeyOwnerReferences.TryGetValue(returnedType, out associationType))
          this.ownerAssociationTypes[index] = (EntityType) associationType;
      }
      this.fetchedCollections.InitializeCollectionOwnerColumns(this.returnedTypes);
      if (ArrayHelper.IsAllNegative(this.owners))
        this.owners = (int[]) null;
      SqlString fragment = this.RenderScalarSelect();
      int count3 = this.scalarTypes.Count;
      this.hasScalars = this.scalarTypes.Count != count1;
      this.returnTypes = new IType[count3];
      for (int index = 0; index < count3; ++index)
        this.returnTypes[index] = this.scalarTypes[index];
      QuerySelect sql = new QuerySelect(this.Factory.Dialect);
      sql.Distinct = this.distinct;
      if (!this.shallowQuery)
      {
        this.RenderIdentifierSelect(sql);
        this.RenderPropertiesSelect(sql);
      }
      this.fetchedCollections.AddSelectFragmentString(sql);
      if (this.hasScalars || this.shallowQuery)
        sql.AddSelectFragmentString(fragment);
      this.MergeJoins(sql.JoinFragment);
      this.RenderFunctions(this.whereTokens);
      sql.SetWhereTokens((ICollection) this.whereTokens);
      this.RenderFunctions(this.groupByTokens);
      sql.SetGroupByTokens((ICollection) this.groupByTokens);
      this.RenderFunctions(this.havingTokens);
      sql.SetHavingTokens((ICollection) this.havingTokens);
      this.RenderFunctions(this.orderByTokens);
      sql.SetOrderByTokens((ICollection) this.orderByTokens);
      this.fetchedCollections.AddOrderBy(sql);
      this.scalarColumnNames = QueryTranslator.GenerateColumnNames(this.returnTypes, this.Factory);
      foreach (string role in (IEnumerable<string>) this.collections.Values)
        this.AddQuerySpaces(this.GetCollectionPersister(role).CollectionSpaces);
      foreach (string key in (IEnumerable<string>) this.typeMap.Keys)
        this.AddQuerySpaces(this.GetPersisterForName(key).QuerySpaces);
      this.sqlString = sql.ToQuerySqlString();
      try
      {
        if (this.holderClass != null)
          this.holderConstructor = ReflectHelper.GetConstructor(this.holderClass, this.returnTypes);
      }
      catch (Exception ex)
      {
        throw new QueryException("could not find constructor for: " + this.holderClass.Name, ex);
      }
      if (this.hasScalars)
      {
        this.actualReturnTypes = this.returnTypes;
      }
      else
      {
        this.actualReturnTypes = new IType[this.selectLength];
        int num = 0;
        for (int index = 0; index < this.persisters.Length; ++index)
        {
          if (this.includeInSelect[index])
            this.actualReturnTypes[num++] = NHibernateUtil.Entity(this.persisters[index].EntityName);
        }
      }
    }

    private void RenderIdentifierSelect(QuerySelect sql)
    {
      int count = this.returnedTypes.Count;
      for (int index = 0; index < count; ++index)
      {
        string returnedType = this.returnedTypes[index];
        string suffix = count == 1 ? string.Empty : index.ToString() + (object) '_';
        sql.AddSelectFragmentString(new SqlString(this.persisters[index].IdentifierSelectFragment(returnedType, suffix)));
      }
    }

    private void RenderPropertiesSelect(QuerySelect sql)
    {
      int count = this.returnedTypes.Count;
      for (int index = 0; index < count; ++index)
      {
        string suffix = count == 1 ? string.Empty : index.ToString() + (object) '_';
        string returnedType = this.returnedTypes[index];
        sql.AddSelectFragmentString(new SqlString(this.persisters[index].PropertySelectFragment(returnedType, suffix, false)));
      }
    }

    private SqlString RenderScalarSelect()
    {
      bool flag1 = this.superQuery != null;
      SqlStringBuilder sqlStringBuilder1 = new SqlStringBuilder();
      if (this.scalarTypes.Count == 0)
      {
        int count = this.returnedTypes.Count;
        for (int index = 0; index < count; ++index)
        {
          this.scalarTypes.Add(NHibernateUtil.Entity(this.persisters[index].EntityName));
          string[] identifierColumnNames = this.persisters[index].IdentifierColumnNames;
          for (int y = 0; y < identifierColumnNames.Length; ++y)
          {
            sqlStringBuilder1.Add(this.returnedTypes[index].ToString()).Add('.'.ToString()).Add(identifierColumnNames[y]);
            if (!flag1)
              sqlStringBuilder1.Add(" as ").Add(QueryTranslator.ScalarName(index, y));
            if (y != identifierColumnNames.Length - 1 || index != count - 1)
              sqlStringBuilder1.Add(", ");
          }
        }
      }
      else
      {
        int x1 = 0;
        bool flag2 = false;
        int num1 = 0;
        for (int tokenIdx = 0; tokenIdx < this.scalarSelectTokens.Count; ++tokenIdx)
        {
          SqlString scalarSelectToken = this.scalarSelectTokens[tokenIdx];
          if (scalarSelectToken.Count == 1)
          {
            string sql = scalarSelectToken.ToString();
            string lowerInvariant = sql.ToLowerInvariant();
            ISQLFunction sqlFunction = this.Factory.SQLFunctionRegistry.FindSQLFunction(lowerInvariant);
            if (sqlFunction != null)
            {
              SqlString sqlString = this.RenderFunctionClause(sqlFunction, this.scalarSelectTokens, ref tokenIdx);
              sqlStringBuilder1.Add(sqlString);
            }
            else
            {
              if ("(".Equals(sql))
                ++num1;
              else if (")".Equals(sql))
                --num1;
              else if (lowerInvariant.Equals(", "))
              {
                if (flag2)
                  flag2 = false;
                else if (!flag1 && num1 == 0)
                  sqlStringBuilder1.Add(" as ").Add(QueryTranslator.ScalarName(x1++, 0));
              }
              sqlStringBuilder1.Add(sql);
              if (lowerInvariant.Equals("distinct") || lowerInvariant.Equals("all"))
                sqlStringBuilder1.Add(" ");
            }
          }
          else
          {
            flag2 = true;
            int y = 0;
            foreach (object part in (IEnumerable) scalarSelectToken.Parts)
            {
              sqlStringBuilder1.AddObject(part);
              if (!flag1)
                sqlStringBuilder1.Add(" as ").Add(QueryTranslator.ScalarName(x1, y));
              if (y != scalarSelectToken.Count - 1)
                sqlStringBuilder1.Add(", ");
              ++y;
            }
            ++x1;
          }
        }
        if (!flag1 && !flag2)
        {
          SqlStringBuilder sqlStringBuilder2 = sqlStringBuilder1.Add(" as ");
          int x2 = x1;
          int num2 = x2 + 1;
          string sql = QueryTranslator.ScalarName(x2, 0);
          sqlStringBuilder2.Add(sql);
        }
      }
      return sqlStringBuilder1.ToSqlString();
    }

    private void RenderFunctions(IList<SqlString> tokens)
    {
      for (int index1 = 0; index1 < tokens.Count; ++index1)
      {
        ISQLFunction sqlFunction = this.Factory.SQLFunctionRegistry.FindSQLFunction(tokens[index1].ToString().ToLowerInvariant());
        if (sqlFunction != null)
        {
          int tokenIdx = index1;
          SqlString sqlString = this.RenderFunctionClause(sqlFunction, tokens, ref tokenIdx);
          for (int index2 = 0; index2 < tokenIdx - index1; ++index2)
            tokens.RemoveAt(index1 + 1);
          tokens[index1] = new SqlString(new object[1]
          {
            (object) sqlString
          });
        }
      }
    }

    private IList<SqlString> ExtractFunctionClause(IList<SqlString> tokens, ref int tokenIdx)
    {
      SqlString token = tokens[tokenIdx];
      IList<SqlString> functionClause = (IList<SqlString>) new System.Collections.Generic.List<SqlString>();
      functionClause.Add(token);
      ++tokenIdx;
      if (tokenIdx >= tokens.Count || !"(".Equals(tokens[tokenIdx].ToString()))
        throw new QueryException("'(' expected after function " + (object) token);
      functionClause.Add(new SqlString("("));
      ++tokenIdx;
      int num = 1;
      while (tokenIdx < tokens.Count && num > 0)
      {
        if (tokens[tokenIdx].Parts.Count == 1 && (object) (tokens[tokenIdx].Parts.First() as Parameter) != null)
        {
          functionClause.Add(tokens[tokenIdx]);
        }
        else
        {
          if (tokens[tokenIdx].StartsWithCaseInsensitive(":"))
          {
            string name = tokens[tokenIdx].Substring(1).ToString();
            functionClause.Add(this.GetNamedParameter(name));
          }
          else if ("?".Equals(tokens[tokenIdx].ToString()))
            functionClause.Add(this.GetPositionalParameter());
          else
            functionClause.Add(tokens[tokenIdx]);
          if ("(".Equals(tokens[tokenIdx].ToString()))
            ++num;
          else if (")".Equals(tokens[tokenIdx].ToString()))
            --num;
        }
        ++tokenIdx;
      }
      --tokenIdx;
      if (num > 0)
        throw new QueryException("')' expected for function " + (object) token);
      return functionClause;
    }

    private SqlString RenderFunctionClause(
      ISQLFunction func,
      IList<SqlString> tokens,
      ref int tokenIdx)
    {
      if (!func.HasArguments)
      {
        if (func.HasParenthesesIfNoArguments)
          this.ExtractFunctionClause(tokens, ref tokenIdx);
        return func.Render((IList) new System.Collections.Generic.List<object>(), this.Factory);
      }
      IList<SqlString> functionClause = this.ExtractFunctionClause(tokens, ref tokenIdx);
      if (!(func is IFunctionGrammar functionGrammar))
        functionGrammar = (IFunctionGrammar) new CommonGrammar();
      IList args = (IList) new System.Collections.Generic.List<object>();
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      for (int tokenIdx1 = 2; tokenIdx1 < functionClause.Count - 1; ++tokenIdx1)
      {
        object part = (object) functionClause[tokenIdx1];
        if (functionGrammar.IsKnownArgument(part.ToString()))
        {
          if (sqlStringBuilder.Count > 0)
          {
            args.Add((object) sqlStringBuilder.ToSqlString());
            sqlStringBuilder = new SqlStringBuilder();
          }
          args.Add(part);
        }
        else if (functionGrammar.IsSeparator(part.ToString()))
        {
          if (sqlStringBuilder.Count > 0)
          {
            args.Add((object) sqlStringBuilder.ToSqlString());
            sqlStringBuilder = new SqlStringBuilder();
          }
        }
        else
        {
          ISQLFunction sqlFunction = this.Factory.SQLFunctionRegistry.FindSQLFunction(part.ToString().ToLowerInvariant());
          if (sqlFunction != null)
            sqlStringBuilder.Add(this.RenderFunctionClause(sqlFunction, functionClause, ref tokenIdx1));
          else
            sqlStringBuilder.AddObject(part);
        }
      }
      if (sqlStringBuilder.Count > 0)
        args.Add((object) sqlStringBuilder.ToSqlString());
      return func.Render(args, this.Factory);
    }

    private void MergeJoins(JoinFragment ojf)
    {
      foreach (KeyValuePair<string, JoinSequence> join in (IEnumerable<KeyValuePair<string, JoinSequence>>) this.joins)
      {
        string key = join.Key;
        JoinSequence joinSequence = join.Value;
        joinSequence.SetSelector((JoinSequence.ISelector) new QueryTranslator.Selector(this));
        if (this.typeMap.ContainsKey(key))
          ojf.AddFragment(joinSequence.ToJoinFragment(this.enabledFilters, true));
        else if (this.collections.ContainsKey(key))
          ojf.AddFragment(joinSequence.ToJoinFragment(this.enabledFilters, true));
      }
    }

    public ISet<string> QuerySpaces => this.querySpaces;

    public bool IsShallowQuery => this.shallowQuery;

    internal bool Distinct
    {
      set => this.distinct = value;
    }

    public bool IsSubquery => this.superQuery != null;

    protected override ICollectionPersister[] CollectionPersisters
    {
      get => this.fetchedCollections.CollectionPersisters;
    }

    protected override string[] CollectionSuffixes => this.fetchedCollections.CollectionSuffixes;

    public void AddCollectionToFetch(
      string role,
      string name,
      string ownerName,
      string entityName)
    {
      IQueryableCollection collectionPersister = this.GetCollectionPersister(role);
      this.fetchedCollections.Add(name, (ICollectionPersister) collectionPersister, ownerName);
      if (!collectionPersister.ElementType.IsEntityType)
        return;
      this.AddEntityToFetch(entityName);
    }

    protected override string[] Suffixes => this.suffixes;

    protected void AddFromAssociation(string elementName, string collectionRole)
    {
      IType elementType = this.GetCollectionPersister(collectionRole).ElementType;
      if (!elementType.IsEntityType)
        throw new QueryException("collection of values in filter: " + elementName);
      IQueryableCollection collectionPersister = this.GetCollectionPersister(collectionRole);
      string[] keyColumnNames = collectionPersister.KeyColumnNames;
      JoinSequence joinSequence = new JoinSequence(this.Factory);
      string str = collectionPersister.IsOneToMany ? elementName : this.CreateNameForCollection(collectionRole);
      joinSequence.SetRoot((IJoinable) collectionPersister, str);
      if (!collectionPersister.IsOneToMany)
      {
        this.AddCollection(str, collectionRole);
        try
        {
          joinSequence.AddJoin((IAssociationType) collectionPersister.ElementType, elementName, JoinType.InnerJoin, collectionPersister.GetElementColumnNames(str));
        }
        catch (MappingException ex)
        {
          throw new QueryException((Exception) ex);
        }
      }
      joinSequence.AddCondition(str, keyColumnNames, " = ", true);
      EntityType entityType = (EntityType) elementType;
      this.AddFrom(elementName, entityType.GetAssociatedEntityName(), joinSequence);
    }

    internal string GetPathAlias(string path)
    {
      string pathAlias;
      this.pathAliases.TryGetValue(path, out pathAlias);
      return pathAlias;
    }

    internal JoinSequence GetPathJoin(string path)
    {
      JoinSequence pathJoin;
      this.pathJoins.TryGetValue(path, out pathJoin);
      return pathJoin;
    }

    internal void AddPathAliasAndJoin(string path, string alias, JoinSequence joinSequence)
    {
      this.pathAliases.Add(path, alias);
      this.pathJoins.Add(path, joinSequence);
    }

    public IList List(ISessionImplementor session, QueryParameters queryParameters)
    {
      return this.List(session, queryParameters, this.QuerySpaces, this.actualReturnTypes);
    }

    public IEnumerable GetEnumerable(QueryParameters parameters, IEventSource session)
    {
      bool statisticsEnabled = session.Factory.Statistics.IsStatisticsEnabled;
      Stopwatch stopwatch = new Stopwatch();
      if (statisticsEnabled)
        stopwatch.Start();
      IDbCommand dbCommand = this.PrepareQueryCommand(parameters, false, (ISessionImplementor) session);
      IDataReader resultSet = this.GetResultSet(dbCommand, parameters.HasAutoDiscoverScalarTypes, false, parameters.RowSelection, (ISessionImplementor) session);
      HolderInstantiator holderInstantiator = HolderInstantiator.CreateClassicHolderInstantiator(this.holderConstructor, parameters.ResultTransformer);
      IEnumerable enumerable = (IEnumerable) new EnumerableImpl(resultSet, dbCommand, session, parameters.IsReadOnly((ISessionImplementor) session), this.ReturnTypes, this.ScalarColumnNames, parameters.RowSelection, holderInstantiator);
      if (statisticsEnabled)
      {
        stopwatch.Stop();
        session.Factory.StatisticsImplementor.QueryExecuted("HQL: " + this.queryString, 0, stopwatch.Elapsed);
        session.Factory.StatisticsImplementor.QueryExecuted(this.QueryIdentifier, 0, stopwatch.Elapsed);
      }
      return enumerable;
    }

    public string[] ConcreteQueries(string query, ISessionFactoryImplementor factory)
    {
      string[] strArray1 = StringHelper.Split(" \n\r\f\t(),", query, true);
      if (strArray1.Length == 0)
        return new string[1]{ query };
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      StringBuilder stringBuilder = new StringBuilder(40);
      int num = 0;
      string o = (string) null;
      int index1 = 0;
      string str1 = (string) null;
      stringBuilder.Append(strArray1[0]);
      for (int index2 = 1; index2 < strArray1.Length; ++index2)
      {
        if (!ParserHelper.IsWhitespace(strArray1[index2 - 1]))
          o = strArray1[index2 - 1].ToLowerInvariant();
        string str2 = strArray1[index2];
        if (!ParserHelper.IsWhitespace(str2) || o == null)
        {
          if (index1 <= index2)
          {
            for (index1 = index2 + 1; index1 < strArray1.Length; ++index1)
            {
              str1 = strArray1[index1].ToLowerInvariant();
              if (!ParserHelper.IsWhitespace(str1))
                break;
            }
          }
          if (o != null && QueryTranslator.beforeClassTokens.Contains((object) o) && (str1 == null || !QueryTranslator.notAfterClassTokens.Contains((object) str1)) || "class".Equals(o))
          {
            System.Type importedClass = this.Helper.GetImportedClass(str2);
            if (importedClass != null)
            {
              string[] implementors = factory.GetImplementors(importedClass.FullName);
              string str3 = "$clazz" + (object) num++ + "$";
              if (implementors != null)
              {
                arrayList1.Add((object) str3);
                arrayList2.Add((object) implementors);
              }
              str2 = str3;
            }
          }
        }
        stringBuilder.Append(str2);
      }
      string[] strArray2 = StringHelper.Multiply(stringBuilder.ToString(), arrayList1.GetEnumerator(), arrayList2.GetEnumerator());
      if (strArray2.Length == 0)
        QueryTranslator.log.Warn((object) ("no persistent classes found for query class: " + query));
      return strArray2;
    }

    static QueryTranslator()
    {
      QueryTranslator.beforeClassTokens.Add((object) "from");
      QueryTranslator.beforeClassTokens.Add((object) ",");
      QueryTranslator.notAfterClassTokens.Add((object) "in");
      QueryTranslator.notAfterClassTokens.Add((object) "from");
      QueryTranslator.notAfterClassTokens.Add((object) ")");
    }

    private static string[][] GenerateColumnNames(IType[] types, ISessionFactoryImplementor f)
    {
      string[][] columnNames = new string[types.Length][];
      for (int x = 0; x < types.Length; ++x)
      {
        int columnSpan = types[x].GetColumnSpan((IMapping) f);
        columnNames[x] = new string[columnSpan];
        for (int y = 0; y < columnSpan; ++y)
          columnNames[x][y] = QueryTranslator.ScalarName(x, y);
      }
      return columnNames;
    }

    protected override object GetResultColumnOrRow(
      object[] row,
      IResultTransformer resultTransformer,
      IDataReader rs,
      ISessionImplementor session)
    {
      IType[] returnTypes = this.ReturnTypes;
      row = this.ToResultRow(row);
      bool flag = this.holderClass != null || resultTransformer != null;
      if (this.hasScalars)
      {
        string[][] scalarColumnNames = this.ScalarColumnNames;
        int length = returnTypes.Length;
        if (this.holderClass == null && length == 1)
          return returnTypes[0].NullSafeGet(rs, scalarColumnNames[0], session, (object) null);
        row = new object[length];
        for (int index = 0; index < length; ++index)
          row[index] = returnTypes[index].NullSafeGet(rs, scalarColumnNames[index], session, (object) null);
        return (object) row;
      }
      if (flag)
        return (object) row;
      return row.Length != 1 ? (object) row : row[0];
    }

    public override IList GetResultList(IList results, IResultTransformer resultTransformer)
    {
      HolderInstantiator holderInstantiator = HolderInstantiator.CreateClassicHolderInstantiator(this.holderConstructor, resultTransformer);
      if (holderInstantiator.IsRequired)
      {
        for (int index = 0; index < results.Count; ++index)
        {
          object[] result = (object[]) results[index];
          results[index] = holderInstantiator.Instantiate(result);
        }
        if (this.holderConstructor == null && resultTransformer != null)
          return resultTransformer.TransformList(results);
      }
      return results;
    }

    private object[] ToResultRow(object[] row)
    {
      if (this.selectLength == row.Length)
        return row;
      object[] resultRow = new object[this.selectLength];
      int num = 0;
      for (int index = 0; index < row.Length; ++index)
      {
        if (this.includeInSelect[index])
          resultRow[num++] = row[index];
      }
      return resultRow;
    }

    internal QueryJoinFragment CreateJoinFragment(bool useThetaStyleInnerJoins)
    {
      return new QueryJoinFragment(this.Factory.Dialect, useThetaStyleInnerJoins);
    }

    internal System.Type HolderClass
    {
      set => this.holderClass = value;
    }

    public override LockMode[] GetLockModes(IDictionary<string, LockMode> lockModes)
    {
      Dictionary<string, LockMode> dictionary = new Dictionary<string, LockMode>();
      if (lockModes != null)
      {
        foreach (KeyValuePair<string, LockMode> lockMode in (IEnumerable<KeyValuePair<string, LockMode>>) lockModes)
          dictionary[this.GetAliasName(lockMode.Key)] = lockMode.Value;
      }
      LockMode[] lockModes1 = new LockMode[this.names.Length];
      for (int index = 0; index < this.names.Length; ++index)
      {
        LockMode none;
        if (!dictionary.TryGetValue(this.names[index], out none))
          none = LockMode.None;
        lockModes1[index] = none;
      }
      return lockModes1;
    }

    protected override SqlString ApplyLocks(
      SqlString sql,
      IDictionary<string, LockMode> lockModes,
      NHibernate.Dialect.Dialect dialect)
    {
      SqlString sqlString;
      if (lockModes == null || lockModes.Count == 0)
      {
        sqlString = sql;
      }
      else
      {
        Dictionary<string, LockMode> aliasedLockModes = new Dictionary<string, LockMode>();
        foreach (KeyValuePair<string, LockMode> lockMode in (IEnumerable<KeyValuePair<string, LockMode>>) lockModes)
          aliasedLockModes[this.GetAliasName(lockMode.Key)] = lockMode.Value;
        Dictionary<string, string[]> keyColumnNames = (Dictionary<string, string[]>) null;
        if (dialect.ForUpdateOfColumns)
        {
          keyColumnNames = new Dictionary<string, string[]>();
          for (int index = 0; index < this.names.Length; ++index)
            keyColumnNames[this.names[index]] = this.persisters[index].IdentifierColumnNames;
        }
        sqlString = dialect.ApplyLocksToSql(sql, (IDictionary<string, LockMode>) aliasedLockModes, (IDictionary<string, string[]>) keyColumnNames);
      }
      QueryTranslator.LogQuery(this.queryString, sqlString.ToString());
      return sqlString;
    }

    protected override bool UpgradeLocks() => true;

    protected override int[] CollectionOwners => this.fetchedCollections.CollectionOwners;

    protected bool Compiled => this.compiled;

    public override string ToString() => this.queryString;

    protected override int[] Owners => this.owners;

    public IDictionary<string, NHibernate.IFilter> EnabledFilters => this.enabledFilters;

    public void AddFromJoinOnly(string name, JoinSequence joinSequence)
    {
      this.AddJoin(name, joinSequence.GetFromPart());
    }

    public override bool IsSubselectLoadingEnabled => this.HasSubselectLoadableCollections();

    protected override string[] Aliases => this.names;

    protected override EntityType[] OwnerAssociationTypes => this.ownerAssociationTypes;

    public int ExecuteUpdate(QueryParameters queryParameters, ISessionImplementor session)
    {
      throw new NotSupportedException();
    }

    public string SQLString => this.sqlString.ToString();

    public IList<string> CollectSqlStrings
    {
      get
      {
        IList<string> collectSqlStrings = (IList<string>) new System.Collections.Generic.List<string>(1);
        collectSqlStrings.Add(this.sqlString.ToString());
        return collectSqlStrings;
      }
    }

    public string QueryString => this.queryString;

    public string[] ReturnAliases => QueryTranslator.NoReturnAliases;

    public string[][] GetColumnNames() => this.scalarColumnNames;

    public bool ContainsCollectionFetches => false;

    public bool IsManipulationStatement => false;

    public override string QueryIdentifier => this.queryIdentifier;

    internal void AddQuerySpaces(string[] spaces)
    {
      for (int index = 0; index < spaces.Length; ++index)
        this.querySpaces.Add(spaces[index]);
      if (this.superQuery == null)
        return;
      this.superQuery.AddQuerySpaces(spaces);
    }

    private class FetchedCollections
    {
      private int count;
      private System.Collections.Generic.List<ICollectionPersister> persisters;
      private System.Collections.Generic.List<string> names;
      private System.Collections.Generic.List<string> ownerNames;
      private System.Collections.Generic.List<string> suffixes;
      private bool hasUnsafeCollection;
      private System.Collections.Generic.List<int> ownerColumns;

      private static string GenerateSuffix(int count) => count.ToString() + "__";

      private static bool IsUnsafe(ICollectionPersister collectionPersister)
      {
        return collectionPersister.CollectionType is BagType || collectionPersister.CollectionType is IdentifierBagType;
      }

      public void Add(string name, ICollectionPersister collectionPersister, string ownerName)
      {
        if (this.persisters == null)
        {
          this.persisters = new System.Collections.Generic.List<ICollectionPersister>(2);
          this.names = new System.Collections.Generic.List<string>(2);
          this.ownerNames = new System.Collections.Generic.List<string>(2);
          this.suffixes = new System.Collections.Generic.List<string>(2);
        }
        ++this.count;
        this.hasUnsafeCollection = this.hasUnsafeCollection || QueryTranslator.FetchedCollections.IsUnsafe(collectionPersister);
        this.names.Add(name);
        this.persisters.Add(collectionPersister);
        this.ownerNames.Add(ownerName);
        this.suffixes.Add(QueryTranslator.FetchedCollections.GenerateSuffix(this.count - 1));
      }

      public int[] CollectionOwners
      {
        get => this.ownerColumns == null ? (int[]) null : this.ownerColumns.ToArray();
      }

      public ICollectionPersister[] CollectionPersisters
      {
        get => this.persisters == null ? (ICollectionPersister[]) null : this.persisters.ToArray();
      }

      public string[] CollectionSuffixes
      {
        get => this.suffixes == null ? (string[]) null : this.suffixes.ToArray();
      }

      public void AddSelectFragmentString(QuerySelect sql)
      {
        if (this.persisters == null)
          return;
        for (int index = 0; index < this.count; ++index)
          sql.AddSelectFragmentString(new SqlString(((IQueryableCollection) this.persisters[index]).SelectFragment(this.names[index], this.suffixes[index])));
      }

      public void AddOrderBy(QuerySelect sql)
      {
        if (this.persisters == null)
          return;
        for (int index = 0; index < this.count; ++index)
        {
          IQueryableCollection persister = (IQueryableCollection) this.persisters[index];
          if (persister.HasOrdering)
            sql.AddOrderBy(persister.GetSQLOrderByString(this.names[index]));
        }
      }

      public void InitializeCollectionOwnerColumns(IList<string> returnedTypes)
      {
        if (this.count == 0)
          return;
        this.ownerColumns = new System.Collections.Generic.List<int>(this.count);
        for (int index = 0; index < this.count; ++index)
        {
          string ownerName = this.ownerNames[index];
          this.ownerColumns.Add(returnedTypes.IndexOf(ownerName));
        }
      }
    }

    private class Selector : JoinSequence.ISelector
    {
      private QueryTranslator outer;

      public Selector(QueryTranslator outer) => this.outer = outer;

      public bool IncludeSubclasses(string alias)
      {
        return this.outer.returnedTypes.Contains(alias) && !this.outer.IsShallowQuery;
      }
    }
  }
}

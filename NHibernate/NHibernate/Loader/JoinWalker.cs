// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.JoinWalker
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#nullable disable
namespace NHibernate.Loader
{
  public class JoinWalker
  {
    private readonly ISessionFactoryImplementor factory;
    protected readonly IList<OuterJoinableAssociation> associations = (IList<OuterJoinableAssociation>) new List<OuterJoinableAssociation>();
    private readonly ISet<JoinWalker.AssociationKey> visitedAssociationKeys = (ISet<JoinWalker.AssociationKey>) new HashedSet<JoinWalker.AssociationKey>();
    private readonly IDictionary<string, IFilter> enabledFilters;
    private readonly IDictionary<string, IFilter> enabledFiltersForManyToOne;
    private static readonly Regex aliasRegex = new Regex("([\\w]+)\\.", RegexOptions.IgnoreCase | RegexOptions.Compiled);
    private string[] suffixes;
    private string[] collectionSuffixes;
    private ILoadable[] persisters;
    private int[] owners;
    private EntityType[] ownerAssociationTypes;
    private ICollectionPersister[] collectionPersisters;
    private int[] collectionOwners;
    private string[] aliases;
    private LockMode[] lockModeArray;
    private SqlString sql;
    private readonly List<JoinWalker.DependentAlias> _dependentAliases = new List<JoinWalker.DependentAlias>();

    public string[] CollectionSuffixes
    {
      get => this.collectionSuffixes;
      set => this.collectionSuffixes = value;
    }

    public LockMode[] LockModeArray
    {
      get => this.lockModeArray;
      set => this.lockModeArray = value;
    }

    public string[] Suffixes
    {
      get => this.suffixes;
      set => this.suffixes = value;
    }

    public string[] Aliases
    {
      get => this.aliases;
      set => this.aliases = value;
    }

    public int[] CollectionOwners
    {
      get => this.collectionOwners;
      set => this.collectionOwners = value;
    }

    public ICollectionPersister[] CollectionPersisters
    {
      get => this.collectionPersisters;
      set => this.collectionPersisters = value;
    }

    public EntityType[] OwnerAssociationTypes
    {
      get => this.ownerAssociationTypes;
      set => this.ownerAssociationTypes = value;
    }

    public int[] Owners
    {
      get => this.owners;
      set => this.owners = value;
    }

    public ILoadable[] Persisters
    {
      get => this.persisters;
      set => this.persisters = value;
    }

    public SqlString SqlString
    {
      get => this.sql;
      set => this.sql = value;
    }

    protected ISessionFactoryImplementor Factory => this.factory;

    protected NHibernate.Dialect.Dialect Dialect => this.factory.Dialect;

    protected IDictionary<string, IFilter> EnabledFilters => this.enabledFilters;

    protected virtual bool IsTooManyCollections => false;

    protected JoinWalker(
      ISessionFactoryImplementor factory,
      IDictionary<string, IFilter> enabledFilters)
    {
      this.factory = factory;
      this.enabledFilters = enabledFilters;
      this.enabledFiltersForManyToOne = FilterHelper.GetEnabledForManyToOne(enabledFilters);
    }

    private void AddAssociationToJoinTreeIfNecessary(
      IAssociationType type,
      string[] aliasedLhsColumns,
      string alias,
      string path,
      int currentDepth,
      JoinType joinType)
    {
      if (joinType < JoinType.InnerJoin)
        return;
      this.AddAssociationToJoinTree(type, aliasedLhsColumns, alias, path, currentDepth, joinType);
    }

    protected virtual SqlString GetWithClause(string path) => SqlString.Empty;

    private void AddAssociationToJoinTree(
      IAssociationType type,
      string[] aliasedLhsColumns,
      string alias,
      string path,
      int currentDepth,
      JoinType joinType)
    {
      IJoinable associatedJoinable = type.GetAssociatedJoinable(this.Factory);
      string tableAlias = this.GenerateTableAlias(this.associations.Count + 1, path, associatedJoinable);
      OuterJoinableAssociation association = new OuterJoinableAssociation(type, alias, aliasedLhsColumns, tableAlias, joinType, this.GetWithClause(path), this.Factory, this.enabledFilters);
      association.ValidateJoin(path);
      this.AddAssociation(tableAlias, association);
      int currentDepth1 = currentDepth + 1;
      if (!associatedJoinable.IsCollection)
      {
        if (!(associatedJoinable is IOuterJoinLoadable persister))
          return;
        this.WalkEntityTree(persister, tableAlias, path, currentDepth1);
      }
      else
      {
        if (!(associatedJoinable is IQueryableCollection persister))
          return;
        this.WalkCollectionTree(persister, tableAlias, path, currentDepth1);
      }
    }

    private static int[] GetTopologicalSortOrder(List<JoinWalker.DependentAlias> fields)
    {
      TopologicalSorter topologicalSorter = new TopologicalSorter(fields.Count);
      Dictionary<string, int> dictionary = new Dictionary<string, int>();
      for (int index = 0; index < fields.Count; ++index)
        dictionary[fields[index].Alias.ToLower()] = topologicalSorter.AddVertex(index);
      for (int index1 = 0; index1 < fields.Count; ++index1)
      {
        if (fields[index1].DependsOn != null)
        {
          for (int index2 = 0; index2 < fields[index1].DependsOn.Length; ++index2)
          {
            string lower = fields[index1].DependsOn[index2].ToLower();
            if (dictionary.ContainsKey(lower))
              topologicalSorter.AddEdge(index1, dictionary[lower]);
          }
        }
      }
      return topologicalSorter.Sort();
    }

    private void AddAssociation(string subalias, OuterJoinableAssociation association)
    {
      subalias = subalias.ToLower();
      List<string> stringList = new List<string>();
      string input = association.On.ToString();
      if (!string.IsNullOrEmpty(input))
      {
        foreach (Match match in JoinWalker.aliasRegex.Matches(input))
        {
          string str = match.Groups[1].Value;
          if (!(str == subalias))
            stringList.Add(str.ToLower());
        }
      }
      this._dependentAliases.Add(new JoinWalker.DependentAlias()
      {
        Alias = subalias,
        DependsOn = stringList.ToArray()
      });
      this.associations.Add(association);
    }

    protected void WalkEntityTree(IOuterJoinLoadable persister, string alias)
    {
      this.WalkEntityTree(persister, alias, string.Empty, 0);
    }

    protected void WalkCollectionTree(IQueryableCollection persister, string alias)
    {
      this.WalkCollectionTree(persister, alias, string.Empty, 0);
    }

    private void WalkCollectionTree(
      IQueryableCollection persister,
      string alias,
      string path,
      int currentDepth)
    {
      if (persister.IsOneToMany)
      {
        this.WalkEntityTree((IOuterJoinLoadable) persister.ElementPersister, alias, path, currentDepth);
      }
      else
      {
        IType elementType = persister.ElementType;
        if (elementType.IsAssociationType)
        {
          IAssociationType type = (IAssociationType) elementType;
          string[] elementColumnNames1 = persister.GetElementColumnNames(alias);
          string[] elementColumnNames2 = persister.ElementColumnNames;
          bool flag = currentDepth == 0;
          JoinType joinType = this.GetJoinType(type, persister.FetchMode, path, persister.TableName, elementColumnNames2, !flag, currentDepth - 1, (CascadeStyle) null);
          this.AddAssociationToJoinTreeIfNecessary(type, elementColumnNames1, alias, path, currentDepth - 1, joinType);
        }
        else
        {
          if (!elementType.IsComponentType)
            return;
          this.WalkCompositeElementTree((IAbstractComponentType) elementType, persister.ElementColumnNames, persister, alias, path, currentDepth);
        }
      }
    }

    private void WalkEntityAssociationTree(
      IAssociationType associationType,
      IOuterJoinLoadable persister,
      int propertyNumber,
      string alias,
      string path,
      bool nullable,
      int currentDepth,
      ILhsAssociationTypeSqlInfo associationTypeSQLInfo)
    {
      string[] aliasedColumnNames = associationTypeSQLInfo.GetAliasedColumnNames(associationType, 0);
      string[] columnNames = associationTypeSQLInfo.GetColumnNames(associationType, 0);
      string tableName = associationTypeSQLInfo.GetTableName(associationType);
      string path1 = JoinWalker.SubPath(path, persister.GetSubclassPropertyName(propertyNumber));
      JoinType joinType = this.GetJoinType(associationType, persister.GetFetchMode(propertyNumber), path1, tableName, columnNames, nullable, currentDepth, persister.GetCascadeStyle(propertyNumber));
      this.AddAssociationToJoinTreeIfNecessary(associationType, aliasedColumnNames, alias, path1, currentDepth, joinType);
    }

    protected virtual void WalkEntityTree(
      IOuterJoinLoadable persister,
      string alias,
      string path,
      int currentDepth)
    {
      int num = persister.CountSubclassProperties();
      for (int index = 0; index < num; ++index)
      {
        IType subclassPropertyType = persister.GetSubclassPropertyType(index);
        ILhsAssociationTypeSqlInfo lhsSqlInfo = JoinHelper.GetLhsSqlInfo(alias, index, persister, (IMapping) this.Factory);
        if (subclassPropertyType.IsAssociationType)
          this.WalkEntityAssociationTree((IAssociationType) subclassPropertyType, persister, index, alias, path, persister.IsSubclassPropertyNullable(index), currentDepth, lhsSqlInfo);
        else if (subclassPropertyType.IsComponentType)
          this.WalkComponentTree((IAbstractComponentType) subclassPropertyType, 0, alias, JoinWalker.SubPath(path, persister.GetSubclassPropertyName(index)), currentDepth, lhsSqlInfo);
      }
    }

    protected void WalkComponentTree(
      IAbstractComponentType componentType,
      int begin,
      string alias,
      string path,
      int currentDepth,
      ILhsAssociationTypeSqlInfo associationTypeSQLInfo)
    {
      IType[] subtypes = componentType.Subtypes;
      string[] propertyNames = componentType.PropertyNames;
      for (int i = 0; i < subtypes.Length; ++i)
      {
        if (subtypes[i].IsAssociationType)
        {
          IAssociationType type = (IAssociationType) subtypes[i];
          string[] aliasedColumnNames = associationTypeSQLInfo.GetAliasedColumnNames(type, begin);
          string[] columnNames = associationTypeSQLInfo.GetColumnNames(type, begin);
          string tableName = associationTypeSQLInfo.GetTableName(type);
          string path1 = JoinWalker.SubPath(path, propertyNames[i]);
          bool[] propertyNullability = componentType.PropertyNullability;
          JoinType joinType = this.GetJoinType(type, componentType.GetFetchMode(i), path1, tableName, columnNames, propertyNullability == null || propertyNullability[i], currentDepth, componentType.GetCascadeStyle(i));
          this.AddAssociationToJoinTreeIfNecessary(type, aliasedColumnNames, alias, path1, currentDepth, joinType);
        }
        else if (subtypes[i].IsComponentType)
        {
          string path2 = JoinWalker.SubPath(path, propertyNames[i]);
          this.WalkComponentTree((IAbstractComponentType) subtypes[i], begin, alias, path2, currentDepth, associationTypeSQLInfo);
        }
        begin += subtypes[i].GetColumnSpan((IMapping) this.Factory);
      }
    }

    private void WalkCompositeElementTree(
      IAbstractComponentType compositeType,
      string[] cols,
      IQueryableCollection persister,
      string alias,
      string path,
      int currentDepth)
    {
      IType[] subtypes = compositeType.Subtypes;
      string[] propertyNames = compositeType.PropertyNames;
      int begin = 0;
      for (int i = 0; i < subtypes.Length; ++i)
      {
        int columnSpan = subtypes[i].GetColumnSpan((IMapping) this.factory);
        string[] strArray = ArrayHelper.Slice(cols, begin, columnSpan);
        if (subtypes[i].IsAssociationType)
        {
          IAssociationType type = subtypes[i] as IAssociationType;
          string[] aliasedLhsColumns = StringHelper.Qualify(alias, strArray);
          string path1 = JoinWalker.SubPath(path, propertyNames[i]);
          bool[] propertyNullability = compositeType.PropertyNullability;
          JoinType joinType = this.GetJoinType(type, compositeType.GetFetchMode(i), path1, persister.TableName, strArray, propertyNullability == null || propertyNullability[i], currentDepth, compositeType.GetCascadeStyle(i));
          this.AddAssociationToJoinTreeIfNecessary(type, aliasedLhsColumns, alias, path1, currentDepth, joinType);
        }
        else if (subtypes[i].IsComponentType)
        {
          string path2 = JoinWalker.SubPath(path, propertyNames[i]);
          this.WalkCompositeElementTree((IAbstractComponentType) subtypes[i], strArray, persister, alias, path2, currentDepth);
        }
        begin += columnSpan;
      }
    }

    protected static string SubPath(string path, string property)
    {
      return !string.IsNullOrEmpty(path) ? StringHelper.Qualify(path, property) : property;
    }

    protected virtual JoinType GetJoinType(
      IAssociationType type,
      FetchMode config,
      string path,
      string lhsTable,
      string[] lhsColumns,
      bool nullable,
      int currentDepth,
      CascadeStyle cascadeStyle)
    {
      return !this.IsJoinedFetchEnabled(type, config, cascadeStyle) || this.IsTooDeep(currentDepth) || type.IsCollectionType && this.IsTooManyCollections || this.IsDuplicateAssociation(lhsTable, lhsColumns, type) ? JoinType.None : this.GetJoinType(nullable, currentDepth);
    }

    protected JoinType GetJoinType(bool nullable, int currentDepth)
    {
      return nullable || currentDepth != 0 ? JoinType.LeftOuterJoin : JoinType.InnerJoin;
    }

    protected virtual bool IsTooDeep(int currentDepth)
    {
      int maximumFetchDepth = this.Factory.Settings.MaximumFetchDepth;
      return maximumFetchDepth >= 0 && currentDepth >= maximumFetchDepth;
    }

    protected bool IsJoinedFetchEnabledInMapping(FetchMode config, IAssociationType type)
    {
      if (!type.IsEntityType && !type.IsCollectionType)
        return false;
      switch (config)
      {
        case FetchMode.Default:
          return type.IsEntityType && !this.factory.GetEntityPersister(((EntityType) type).GetAssociatedEntityName()).HasProxy;
        case FetchMode.Select:
          return false;
        case FetchMode.Join:
          return true;
        default:
          throw new ArgumentOutOfRangeException(nameof (config), (object) config, "Unknown OJ strategy " + (object) config);
      }
    }

    protected virtual bool IsJoinedFetchEnabled(
      IAssociationType type,
      FetchMode config,
      CascadeStyle cascadeStyle)
    {
      return type.IsEntityType && this.IsJoinedFetchEnabledInMapping(config, type);
    }

    protected virtual string GenerateTableAlias(int n, string path, IJoinable joinable)
    {
      return StringHelper.GenerateAlias(joinable.Name, n);
    }

    protected virtual string GenerateRootAlias(string description)
    {
      return StringHelper.GenerateAlias(description, 0);
    }

    protected virtual bool IsDuplicateAssociation(
      string foreignKeyTable,
      string[] foreignKeyColumns)
    {
      return !this.visitedAssociationKeys.Add(new JoinWalker.AssociationKey(foreignKeyColumns, foreignKeyTable));
    }

    protected virtual bool IsDuplicateAssociation(
      string lhsTable,
      string[] lhsColumnNames,
      IAssociationType type)
    {
      string foreignKeyTable;
      string[] foreignKeyColumns;
      if (type.ForeignKeyDirection.Equals((object) ForeignKeyDirection.ForeignKeyFromParent))
      {
        foreignKeyTable = lhsTable;
        foreignKeyColumns = lhsColumnNames;
      }
      else
      {
        foreignKeyTable = type.GetAssociatedJoinable(this.Factory).TableName;
        foreignKeyColumns = JoinHelper.GetRHSColumnNames(type, this.Factory);
      }
      return this.IsDuplicateAssociation(foreignKeyTable, foreignKeyColumns);
    }

    protected bool IsJoinable(
      JoinType joinType,
      ISet<JoinWalker.AssociationKey> visitedAssociationKeys,
      string lhsTable,
      string[] lhsColumnNames,
      IAssociationType type,
      int depth)
    {
      if (joinType < JoinType.InnerJoin)
        return false;
      if (joinType == JoinType.InnerJoin)
        return true;
      int maximumFetchDepth = this.Factory.Settings.MaximumFetchDepth;
      return (maximumFetchDepth < 0 || depth < maximumFetchDepth) && !this.IsDuplicateAssociation(lhsTable, lhsColumnNames, type);
    }

    protected SqlString OrderBy(IList<OuterJoinableAssociation> associations, SqlString orderBy)
    {
      return this.MergeOrderings(this.OrderBy(associations), orderBy);
    }

    protected SqlString OrderBy(IList<OuterJoinableAssociation> associations, string orderBy)
    {
      return this.MergeOrderings(this.OrderBy(associations), new SqlString(orderBy));
    }

    protected SqlString MergeOrderings(SqlString ass, SqlString orderBy)
    {
      if (ass.Length == 0)
        return orderBy;
      return orderBy.Length == 0 ? ass : ass.Append(", ").Append(orderBy);
    }

    protected SqlString MergeOrderings(string ass, SqlString orderBy)
    {
      return this.MergeOrderings(new SqlString(ass), orderBy);
    }

    protected SqlString MergeOrderings(string ass, string orderBy)
    {
      return this.MergeOrderings(new SqlString(ass), new SqlString(orderBy));
    }

    protected JoinFragment MergeOuterJoins(IList<OuterJoinableAssociation> associations)
    {
      IList<OuterJoinableAssociation> joinableAssociationList = (IList<OuterJoinableAssociation>) new List<OuterJoinableAssociation>();
      int[] topologicalSortOrder = JoinWalker.GetTopologicalSortOrder(this._dependentAliases);
      for (int index = topologicalSortOrder.Length - 1; index >= 0; --index)
        joinableAssociationList.Add(associations[topologicalSortOrder[index]]);
      JoinFragment outerJoinFragment = this.Dialect.CreateOuterJoinFragment();
      OuterJoinableAssociation joinableAssociation = (OuterJoinableAssociation) null;
      foreach (OuterJoinableAssociation other in (IEnumerable<OuterJoinableAssociation>) joinableAssociationList)
      {
        if (joinableAssociation != null && joinableAssociation.IsManyToManyWith(other))
        {
          other.AddManyToManyJoin(outerJoinFragment, (IQueryableCollection) joinableAssociation.Joinable);
        }
        else
        {
          other.AddJoins(outerJoinFragment);
          if (this.enabledFiltersForManyToOne.Count > 0)
          {
            string str = other.Joinable.FilterFragment(other.RHSAlias, this.enabledFiltersForManyToOne);
            if (outerJoinFragment.ToFromFragmentString.IndexOfCaseInsensitive(str) == -1)
              outerJoinFragment.AddCondition(str);
          }
        }
        joinableAssociation = other;
      }
      return outerJoinFragment;
    }

    protected static int CountEntityPersisters(IList<OuterJoinableAssociation> associations)
    {
      int num = 0;
      foreach (OuterJoinableAssociation association in (IEnumerable<OuterJoinableAssociation>) associations)
      {
        if (association.Joinable.ConsumesEntityAlias())
          ++num;
      }
      return num;
    }

    protected static int CountCollectionPersisters(IList<OuterJoinableAssociation> associations)
    {
      int num = 0;
      foreach (OuterJoinableAssociation association in (IEnumerable<OuterJoinableAssociation>) associations)
      {
        if (association.JoinType == JoinType.LeftOuterJoin && association.Joinable.IsCollection)
          ++num;
      }
      return num;
    }

    protected SqlString OrderBy(IList<OuterJoinableAssociation> associations)
    {
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      OuterJoinableAssociation joinableAssociation = (OuterJoinableAssociation) null;
      foreach (OuterJoinableAssociation association in (IEnumerable<OuterJoinableAssociation>) associations)
      {
        if (association.JoinType == JoinType.LeftOuterJoin)
        {
          if (association.Joinable.IsCollection)
          {
            IQueryableCollection joinable = (IQueryableCollection) association.Joinable;
            if (joinable.HasOrdering)
            {
              string sqlOrderByString = joinable.GetSQLOrderByString(association.RHSAlias);
              sqlStringBuilder.Add(sqlOrderByString).Add(", ");
            }
          }
          else if (joinableAssociation != null && joinableAssociation.Joinable.IsCollection)
          {
            IQueryableCollection joinable = (IQueryableCollection) joinableAssociation.Joinable;
            if (joinable.IsManyToMany && joinableAssociation.IsManyToManyWith(association) && joinable.HasManyToManyOrdering)
            {
              string manyOrderByString = joinable.GetManyToManyOrderByString(association.RHSAlias);
              sqlStringBuilder.Add(manyOrderByString).Add(", ");
            }
          }
        }
        joinableAssociation = association;
      }
      if (sqlStringBuilder.Count > 0)
        sqlStringBuilder.RemoveAt(sqlStringBuilder.Count - 1);
      return sqlStringBuilder.ToSqlString();
    }

    protected virtual string GenerateAliasForColumn(string rootAlias, string column) => rootAlias;

    protected SqlStringBuilder WhereString(string alias, string[] columnNames, int batchSize)
    {
      if (columnNames.Length == 1)
      {
        InFragment inFragment = new InFragment().SetColumn(this.GenerateAliasForColumn(alias, columnNames[0]), columnNames[0]);
        for (int index = 0; index < batchSize; ++index)
          inFragment.AddValue((object) Parameter.Placeholder);
        return new SqlStringBuilder(inFragment.ToFragmentString());
      }
      ConditionalFragment[] fragments = new ConditionalFragment[batchSize];
      for (int index = 0; index < batchSize; ++index)
        fragments[index] = new ConditionalFragment().SetTableAlias(alias).SetCondition(columnNames, Parameter.GenerateParameters(columnNames.Length));
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder();
      if (fragments.Length == 1)
      {
        sqlStringBuilder.Add(fragments[0].ToSqlStringFragment());
      }
      else
      {
        DisjunctionFragment disjunctionFragment = new DisjunctionFragment((IEnumerable<ConditionalFragment>) fragments);
        sqlStringBuilder.Add("(");
        sqlStringBuilder.Add(disjunctionFragment.ToFragmentString());
        sqlStringBuilder.Add(")");
      }
      return sqlStringBuilder;
    }

    protected void InitPersisters(IList<OuterJoinableAssociation> associations, LockMode lockMode)
    {
      int length1 = JoinWalker.CountEntityPersisters(associations);
      int length2 = JoinWalker.CountCollectionPersisters(associations);
      this.collectionOwners = length2 == 0 ? (int[]) null : new int[length2];
      this.collectionPersisters = length2 == 0 ? (ICollectionPersister[]) null : new ICollectionPersister[length2];
      this.collectionSuffixes = BasicLoader.GenerateSuffixes(length1 + 1, length2);
      this.persisters = new ILoadable[length1];
      this.aliases = new string[length1];
      this.owners = new int[length1];
      this.ownerAssociationTypes = new EntityType[length1];
      this.lockModeArray = ArrayHelper.FillArray(lockMode, length1);
      int index1 = 0;
      int index2 = 0;
      foreach (OuterJoinableAssociation association in (IEnumerable<OuterJoinableAssociation>) associations)
      {
        if (!association.IsCollection)
        {
          this.persisters[index1] = (ILoadable) association.Joinable;
          this.aliases[index1] = association.RHSAlias;
          this.owners[index1] = association.GetOwner(associations);
          this.ownerAssociationTypes[index1] = (EntityType) association.JoinableType;
          ++index1;
        }
        else
        {
          IQueryableCollection joinable = (IQueryableCollection) association.Joinable;
          if (association.JoinType == JoinType.LeftOuterJoin)
          {
            this.collectionPersisters[index2] = (ICollectionPersister) joinable;
            this.collectionOwners[index2] = association.GetOwner(associations);
            ++index2;
          }
          if (joinable.IsOneToMany)
          {
            this.persisters[index1] = (ILoadable) joinable.ElementPersister;
            this.aliases[index1] = association.RHSAlias;
            ++index1;
          }
        }
      }
      if (ArrayHelper.IsAllNegative(this.owners))
        this.owners = (int[]) null;
      if (this.collectionOwners == null || !ArrayHelper.IsAllNegative(this.collectionOwners))
        return;
      this.collectionOwners = (int[]) null;
    }

    public string SelectString(IList<OuterJoinableAssociation> associations)
    {
      if (associations.Count == 0)
        return string.Empty;
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder(associations.Count * 3);
      int index1 = 0;
      int index2 = 0;
      for (int index3 = 0; index3 < associations.Count; ++index3)
      {
        OuterJoinableAssociation association1 = associations[index3];
        OuterJoinableAssociation association2 = index3 == associations.Count - 1 ? (OuterJoinableAssociation) null : associations[index3 + 1];
        IJoinable joinable = association1.Joinable;
        string suffix = this.suffixes == null || index1 >= this.suffixes.Length ? (string) null : this.suffixes[index1];
        string collectionSuffix = this.collectionSuffixes == null || index2 >= this.collectionSuffixes.Length ? (string) null : this.collectionSuffixes[index2];
        string sql = joinable.SelectFragment(association2 == null ? (IJoinable) null : association2.Joinable, association2 == null ? (string) null : association2.RHSAlias, association1.RHSAlias, suffix, collectionSuffix, association1.JoinType == JoinType.LeftOuterJoin);
        if (sql.Trim().Length > 0)
          sqlStringBuilder.Add(", ").Add(sql);
        if (joinable.ConsumesEntityAlias())
          ++index1;
        if (joinable.ConsumesCollectionAlias() && association1.JoinType == JoinType.LeftOuterJoin)
          ++index2;
      }
      return sqlStringBuilder.ToSqlString().ToString();
    }

    public class DependentAlias
    {
      public string Alias { get; set; }

      public string[] DependsOn { get; set; }
    }

    protected sealed class AssociationKey
    {
      private readonly string[] columns;
      private readonly string table;
      private readonly int hashCode;

      public AssociationKey(string[] columns, string table)
      {
        this.columns = columns;
        this.table = table;
        this.hashCode = table.GetHashCode();
      }

      public override bool Equals(object other)
      {
        return other is JoinWalker.AssociationKey associationKey && associationKey.table.Equals(this.table) && CollectionHelper.CollectionEquals<string>((ICollection<string>) this.columns, (ICollection<string>) associationKey.columns);
      }

      public override int GetHashCode() => this.hashCode;
    }
  }
}

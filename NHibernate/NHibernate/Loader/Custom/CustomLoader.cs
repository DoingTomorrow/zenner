// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Custom.CustomLoader
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Hql;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace NHibernate.Loader.Custom
{
  public class CustomLoader : NHibernate.Loader.Loader
  {
    private readonly SqlString sql;
    private readonly ISet<string> querySpaces = (ISet<string>) new HashedSet<string>();
    private System.Collections.Generic.List<IParameterSpecification> parametersSpecifications;
    private readonly NHibernate.Persister.Entity.IQueryable[] entityPersisters;
    private readonly int[] entityOwners;
    private readonly IEntityAliases[] entityAliases;
    private readonly IQueryableCollection[] collectionPersisters;
    private readonly int[] collectionOwners;
    private readonly ICollectionAliases[] collectionAliases;
    private readonly LockMode[] lockModes;
    private readonly CustomLoader.ResultRowProcessor rowProcessor;
    private IType[] resultTypes;
    private string[] transformerAliases;

    public CustomLoader(ICustomQuery customQuery, ISessionFactoryImplementor factory)
      : base(factory)
    {
      this.sql = customQuery.SQL;
      this.querySpaces.AddAll((ICollection<string>) customQuery.QuerySpaces);
      this.parametersSpecifications = customQuery.CollectedParametersSpecifications.ToList<IParameterSpecification>();
      System.Collections.Generic.List<NHibernate.Persister.Entity.IQueryable> queryableList = new System.Collections.Generic.List<NHibernate.Persister.Entity.IQueryable>();
      System.Collections.Generic.List<int> intList1 = new System.Collections.Generic.List<int>();
      System.Collections.Generic.List<IEntityAliases> entityAliasesList = new System.Collections.Generic.List<IEntityAliases>();
      System.Collections.Generic.List<IQueryableCollection> queryableCollectionList = new System.Collections.Generic.List<IQueryableCollection>();
      System.Collections.Generic.List<int> intList2 = new System.Collections.Generic.List<int>();
      System.Collections.Generic.List<ICollectionAliases> collectionAliasesList = new System.Collections.Generic.List<ICollectionAliases>();
      System.Collections.Generic.List<LockMode> lockModeList = new System.Collections.Generic.List<LockMode>();
      System.Collections.Generic.List<CustomLoader.IResultColumnProcessor> resultColumnProcessorList = new System.Collections.Generic.List<CustomLoader.IResultColumnProcessor>();
      System.Collections.Generic.List<IReturn> returnList = new System.Collections.Generic.List<IReturn>();
      System.Collections.Generic.List<IType> typeList = new System.Collections.Generic.List<IType>();
      System.Collections.Generic.List<string> stringList = new System.Collections.Generic.List<string>();
      int num1 = 0;
      bool hasScalars = false;
      foreach (IReturn customQueryReturn in (IEnumerable<IReturn>) customQuery.CustomQueryReturns)
      {
        switch (customQueryReturn)
        {
          case ScalarReturn _:
            ScalarReturn scalarReturn = (ScalarReturn) customQueryReturn;
            typeList.Add(scalarReturn.Type);
            stringList.Add(scalarReturn.ColumnAlias);
            resultColumnProcessorList.Add((CustomLoader.IResultColumnProcessor) new CustomLoader.ScalarResultColumnProcessor(scalarReturn.ColumnAlias, scalarReturn.Type));
            hasScalars = true;
            continue;
          case RootReturn _:
            RootReturn rootReturn = (RootReturn) customQueryReturn;
            NHibernate.Persister.Entity.IQueryable entityPersister1 = (NHibernate.Persister.Entity.IQueryable) factory.GetEntityPersister(rootReturn.EntityName);
            queryableList.Add(entityPersister1);
            lockModeList.Add(rootReturn.LockMode);
            resultColumnProcessorList.Add((CustomLoader.IResultColumnProcessor) new CustomLoader.NonScalarResultColumnProcessor(num1++));
            returnList.Add(customQueryReturn);
            intList1.Add(-1);
            typeList.Add(entityPersister1.Type);
            stringList.Add(rootReturn.Alias);
            entityAliasesList.Add(rootReturn.EntityAliases);
            this.querySpaces.AddAll((ICollection<string>) entityPersister1.QuerySpaces);
            continue;
          case CollectionReturn _:
            CollectionReturn collectionReturn = (CollectionReturn) customQueryReturn;
            string role1 = collectionReturn.OwnerEntityName + "." + collectionReturn.OwnerProperty;
            IQueryableCollection collectionPersister1 = (IQueryableCollection) factory.GetCollectionPersister(role1);
            queryableCollectionList.Add(collectionPersister1);
            lockModeList.Add(collectionReturn.LockMode);
            resultColumnProcessorList.Add((CustomLoader.IResultColumnProcessor) new CustomLoader.NonScalarResultColumnProcessor(num1++));
            returnList.Add(customQueryReturn);
            intList2.Add(-1);
            typeList.Add(collectionPersister1.Type);
            stringList.Add(collectionReturn.Alias);
            collectionAliasesList.Add(collectionReturn.CollectionAliases);
            IType elementType1 = collectionPersister1.ElementType;
            if (elementType1.IsEntityType)
            {
              NHibernate.Persister.Entity.IQueryable associatedJoinable = (NHibernate.Persister.Entity.IQueryable) ((EntityType) elementType1).GetAssociatedJoinable(factory);
              queryableList.Add(associatedJoinable);
              intList1.Add(-1);
              entityAliasesList.Add(collectionReturn.ElementEntityAliases);
              this.querySpaces.AddAll((ICollection<string>) associatedJoinable.QuerySpaces);
              continue;
            }
            continue;
          case EntityFetchReturn _:
            EntityFetchReturn entityFetchReturn = (EntityFetchReturn) customQueryReturn;
            NonScalarReturn owner1 = entityFetchReturn.Owner;
            int num2 = returnList.IndexOf((IReturn) owner1);
            intList1.Add(num2);
            lockModeList.Add(entityFetchReturn.LockMode);
            string associatedEntityName = ((EntityType) this.DetermineAppropriateOwnerPersister(owner1).GetPropertyType(entityFetchReturn.OwnerProperty)).GetAssociatedEntityName(this.Factory);
            NHibernate.Persister.Entity.IQueryable entityPersister2 = (NHibernate.Persister.Entity.IQueryable) factory.GetEntityPersister(associatedEntityName);
            queryableList.Add(entityPersister2);
            returnList.Add(customQueryReturn);
            stringList.Add(entityFetchReturn.Alias);
            entityAliasesList.Add(entityFetchReturn.EntityAliases);
            this.querySpaces.AddAll((ICollection<string>) entityPersister2.QuerySpaces);
            continue;
          case CollectionFetchReturn _:
            CollectionFetchReturn collectionFetchReturn = (CollectionFetchReturn) customQueryReturn;
            NonScalarReturn owner2 = collectionFetchReturn.Owner;
            int num3 = returnList.IndexOf((IReturn) owner2);
            intList2.Add(num3);
            lockModeList.Add(collectionFetchReturn.LockMode);
            string role2 = this.DetermineAppropriateOwnerPersister(owner2).EntityName + (object) '.' + collectionFetchReturn.OwnerProperty;
            IQueryableCollection collectionPersister2 = (IQueryableCollection) factory.GetCollectionPersister(role2);
            queryableCollectionList.Add(collectionPersister2);
            returnList.Add(customQueryReturn);
            stringList.Add(collectionFetchReturn.Alias);
            collectionAliasesList.Add(collectionFetchReturn.CollectionAliases);
            IType elementType2 = collectionPersister2.ElementType;
            if (elementType2.IsEntityType)
            {
              NHibernate.Persister.Entity.IQueryable associatedJoinable = (NHibernate.Persister.Entity.IQueryable) ((EntityType) elementType2).GetAssociatedJoinable(factory);
              queryableList.Add(associatedJoinable);
              intList1.Add(num3);
              entityAliasesList.Add(collectionFetchReturn.ElementEntityAliases);
              this.querySpaces.AddAll((ICollection<string>) associatedJoinable.QuerySpaces);
              continue;
            }
            continue;
          default:
            throw new HibernateException("unexpected custom query return type : " + customQueryReturn.GetType().FullName);
        }
      }
      this.entityPersisters = queryableList.ToArray();
      this.entityOwners = intList1.ToArray();
      this.entityAliases = entityAliasesList.ToArray();
      this.collectionPersisters = queryableCollectionList.ToArray();
      this.collectionOwners = intList2.ToArray();
      this.collectionAliases = collectionAliasesList.ToArray();
      this.lockModes = lockModeList.ToArray();
      this.resultTypes = typeList.ToArray();
      this.transformerAliases = stringList.ToArray();
      this.rowProcessor = new CustomLoader.ResultRowProcessor(hasScalars, resultColumnProcessorList.ToArray());
    }

    public ISet<string> QuerySpaces => this.querySpaces;

    protected override int[] CollectionOwners => this.collectionOwners;

    protected override int[] Owners => this.entityOwners;

    private string[] ReturnAliasesForTransformer => this.transformerAliases;

    protected override IEntityAliases[] EntityAliases => this.entityAliases;

    protected override ICollectionAliases[] CollectionAliases => this.collectionAliases;

    private NHibernate.Persister.Entity.IQueryable DetermineAppropriateOwnerPersister(
      NonScalarReturn ownerDescriptor)
    {
      string entityName = (string) null;
      switch (ownerDescriptor)
      {
        case RootReturn rootReturn:
          entityName = rootReturn.EntityName;
          break;
        case CollectionReturn _:
          CollectionReturn collectionReturn = (CollectionReturn) ownerDescriptor;
          entityName = ((EntityType) this.Factory.GetCollectionPersister(collectionReturn.OwnerEntityName + "." + collectionReturn.OwnerProperty).ElementType).GetAssociatedEntityName(this.Factory);
          break;
        case FetchReturn _:
          FetchReturn fetchReturn = (FetchReturn) ownerDescriptor;
          IType propertyType = this.DetermineAppropriateOwnerPersister(fetchReturn.Owner).GetPropertyType(fetchReturn.OwnerProperty);
          if (propertyType.IsEntityType)
          {
            entityName = ((EntityType) propertyType).GetAssociatedEntityName(this.Factory);
            break;
          }
          if (propertyType.IsCollectionType)
          {
            IType elementType = ((CollectionType) propertyType).GetElementType(this.Factory);
            if (elementType.IsEntityType)
            {
              entityName = ((EntityType) elementType).GetAssociatedEntityName(this.Factory);
              break;
            }
            break;
          }
          break;
      }
      return entityName != null ? (NHibernate.Persister.Entity.IQueryable) this.Factory.GetEntityPersister(entityName) : throw new HibernateException("Could not determine fetch owner : " + (object) ownerDescriptor);
    }

    public override string QueryIdentifier => this.sql.ToString();

    public override SqlString SqlString => this.sql;

    public override LockMode[] GetLockModes(IDictionary<string, LockMode> lockModesMap)
    {
      return this.lockModes;
    }

    public override ILoadable[] EntityPersisters => (ILoadable[]) this.entityPersisters;

    protected override ICollectionPersister[] CollectionPersisters
    {
      get => (ICollectionPersister[]) this.collectionPersisters;
    }

    public IList List(ISessionImplementor session, QueryParameters queryParameters)
    {
      return this.List(session, queryParameters, this.querySpaces, this.resultTypes);
    }

    protected override object GetResultColumnOrRow(
      object[] row,
      IResultTransformer resultTransformer,
      IDataReader rs,
      ISessionImplementor session)
    {
      return this.rowProcessor.BuildResultRow(row, rs, resultTransformer != null, session);
    }

    public override IList GetResultList(IList results, IResultTransformer resultTransformer)
    {
      HolderInstantiator holderInstantiator = HolderInstantiator.GetHolderInstantiator((IResultTransformer) null, resultTransformer, this.ReturnAliasesForTransformer);
      if (!holderInstantiator.IsRequired)
        return results;
      for (int index = 0; index < results.Count; ++index)
      {
        object[] result = (object[]) results[index];
        object obj = holderInstantiator.Instantiate(result);
        results[index] = obj;
      }
      return resultTransformer.TransformList(results);
    }

    protected override void AutoDiscoverTypes(IDataReader rs)
    {
      CustomLoader.MetaData metadata = new CustomLoader.MetaData(rs);
      System.Collections.Generic.List<string> aliases = new System.Collections.Generic.List<string>();
      System.Collections.Generic.List<IType> types = new System.Collections.Generic.List<IType>();
      this.rowProcessor.PrepareForAutoDiscovery(metadata);
      foreach (CustomLoader.IResultColumnProcessor columnProcessor in this.rowProcessor.ColumnProcessors)
        columnProcessor.PerformDiscovery(metadata, (IList<IType>) types, (IList<string>) aliases);
      this.resultTypes = types.ToArray();
      this.transformerAliases = aliases.ToArray();
    }

    protected override void ResetEffectiveExpectedType(
      IEnumerable<IParameterSpecification> parameterSpecs,
      QueryParameters queryParameters)
    {
      parameterSpecs.ResetEffectiveExpectedType(queryParameters);
    }

    protected override IEnumerable<IParameterSpecification> GetParameterSpecifications()
    {
      return (IEnumerable<IParameterSpecification>) this.parametersSpecifications;
    }

    public IType[] ResultTypes => this.resultTypes;

    public string[] ReturnAliases => this.transformerAliases;

    public IEnumerable<string> NamedParameters
    {
      get
      {
        return this.parametersSpecifications.OfType<NamedParameterSpecification>().Select<NamedParameterSpecification, string>((System.Func<NamedParameterSpecification, string>) (np => np.Name));
      }
    }

    public class ResultRowProcessor
    {
      private readonly bool hasScalars;
      private CustomLoader.IResultColumnProcessor[] columnProcessors;

      public CustomLoader.IResultColumnProcessor[] ColumnProcessors => this.columnProcessors;

      public ResultRowProcessor(
        bool hasScalars,
        CustomLoader.IResultColumnProcessor[] columnProcessors)
      {
        this.hasScalars = hasScalars || columnProcessors == null || columnProcessors.Length == 0;
        this.columnProcessors = columnProcessors;
      }

      public object BuildResultRow(
        object[] data,
        IDataReader resultSet,
        bool hasTransformer,
        ISessionImplementor session)
      {
        object[] objArray;
        if (!this.hasScalars && (hasTransformer || data.Length == 0))
        {
          objArray = data;
        }
        else
        {
          objArray = new object[this.columnProcessors.Length];
          for (int index = 0; index < this.columnProcessors.Length; ++index)
            objArray[index] = this.columnProcessors[index].Extract(data, resultSet, session);
        }
        if (hasTransformer)
          return (object) objArray;
        return objArray.Length != 1 ? (object) objArray : objArray[0];
      }

      public void PrepareForAutoDiscovery(CustomLoader.MetaData metadata)
      {
        if (this.columnProcessors != null && this.columnProcessors.Length != 0)
          return;
        int columnCount = metadata.GetColumnCount();
        this.columnProcessors = new CustomLoader.IResultColumnProcessor[columnCount];
        for (int position = 0; position < columnCount; ++position)
          this.columnProcessors[position] = (CustomLoader.IResultColumnProcessor) new CustomLoader.ScalarResultColumnProcessor(position);
      }
    }

    public interface IResultColumnProcessor
    {
      object Extract(object[] data, IDataReader resultSet, ISessionImplementor session);

      void PerformDiscovery(
        CustomLoader.MetaData metadata,
        IList<IType> types,
        IList<string> aliases);
    }

    public class NonScalarResultColumnProcessor : CustomLoader.IResultColumnProcessor
    {
      private readonly int position;

      public NonScalarResultColumnProcessor(int position) => this.position = position;

      public object Extract(object[] data, IDataReader resultSet, ISessionImplementor session)
      {
        return data[this.position];
      }

      public void PerformDiscovery(
        CustomLoader.MetaData metadata,
        IList<IType> types,
        IList<string> aliases)
      {
      }
    }

    public class ScalarResultColumnProcessor : CustomLoader.IResultColumnProcessor
    {
      private int position = -1;
      private string alias;
      private IType type;

      public ScalarResultColumnProcessor(int position) => this.position = position;

      public ScalarResultColumnProcessor(string alias, IType type)
      {
        this.alias = alias;
        this.type = type;
      }

      public object Extract(object[] data, IDataReader resultSet, ISessionImplementor session)
      {
        return this.type.NullSafeGet(resultSet, this.alias, session, (object) null);
      }

      public void PerformDiscovery(
        CustomLoader.MetaData metadata,
        IList<IType> types,
        IList<string> aliases)
      {
        if (string.IsNullOrEmpty(this.alias))
          this.alias = metadata.GetColumnName(this.position);
        else if (this.position < 0)
          this.position = metadata.GetColumnPosition(this.alias);
        if (this.type == null)
          this.type = metadata.GetHibernateType(this.position);
        types.Add(this.type);
        aliases.Add(this.alias);
      }
    }

    public class MetaData
    {
      private readonly IDataReader resultSet;

      public MetaData(IDataReader resultSet) => this.resultSet = resultSet;

      public int GetColumnCount() => this.resultSet.FieldCount;

      public int GetColumnPosition(string columnName) => this.resultSet.GetOrdinal(columnName);

      public string GetColumnName(int position) => this.resultSet.GetName(position);

      public IType GetHibernateType(int columnPos)
      {
        return TypeFactory.Basic(this.resultSet.GetFieldType(columnPos).Name);
      }
    }
  }
}

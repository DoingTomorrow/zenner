// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Criteria.CriteriaLoader
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Param;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using NHibernate.Util;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Loader.Criteria
{
  public class CriteriaLoader : OuterJoinLoader
  {
    private readonly CriteriaQueryTranslator translator;
    private readonly ISet<string> querySpaces;
    private readonly IType[] resultTypes;
    private readonly string[] userAliases;

    public CriteriaLoader(
      IOuterJoinLoadable persister,
      ISessionFactoryImplementor factory,
      CriteriaImpl rootCriteria,
      string rootEntityName,
      IDictionary<string, NHibernate.IFilter> enabledFilters)
      : base(factory, enabledFilters)
    {
      this.translator = new CriteriaQueryTranslator(factory, rootCriteria, rootEntityName, CriteriaQueryTranslator.RootSqlAlias);
      this.querySpaces = this.translator.GetQuerySpaces();
      CriteriaJoinWalker walker = new CriteriaJoinWalker(persister, this.translator, factory, (ICriteria) rootCriteria, rootEntityName, enabledFilters);
      this.InitFromWalker((JoinWalker) walker);
      this.userAliases = walker.UserAliases;
      this.resultTypes = walker.ResultTypes;
      this.PostInstantiate();
    }

    public ISet<string> QuerySpaces => this.querySpaces;

    public override bool IsSubselectLoadingEnabled => this.HasSubselectLoadableCollections();

    public CriteriaQueryTranslator Translator => this.translator;

    public IType[] ResultTypes => this.resultTypes;

    public IList List(ISessionImplementor session)
    {
      return this.List(session, this.translator.GetQueryParameters(), this.querySpaces, this.resultTypes);
    }

    protected override object GetResultColumnOrRow(
      object[] row,
      IResultTransformer customResultTransformer,
      IDataReader rs,
      ISessionImplementor session)
    {
      object[] objArray;
      if (this.translator.HasProjection)
      {
        IType[] projectedTypes = this.translator.ProjectedTypes;
        objArray = new object[projectedTypes.Length];
        string[] projectedColumnAliases = this.translator.ProjectedColumnAliases;
        int index = 0;
        int begin = 0;
        for (; index < objArray.Length; ++index)
        {
          int columnSpan = projectedTypes[index].GetColumnSpan((IMapping) session.Factory);
          if (columnSpan > 1)
          {
            string[] names = ArrayHelper.Slice(projectedColumnAliases, begin, columnSpan);
            objArray[index] = projectedTypes[index].NullSafeGet(rs, names, session, (object) null);
          }
          else
            objArray[index] = projectedTypes[index].NullSafeGet(rs, projectedColumnAliases[begin], session, (object) null);
          begin += columnSpan;
        }
      }
      else
        objArray = row;
      return customResultTransformer == null ? objArray[objArray.Length - 1] : (object) objArray;
    }

    protected override SqlString ApplyLocks(
      SqlString sqlSelectString,
      IDictionary<string, LockMode> lockModes,
      NHibernate.Dialect.Dialect dialect)
    {
      if (lockModes == null || lockModes.Count == 0)
        return sqlSelectString;
      Dictionary<string, LockMode> dictionary = new Dictionary<string, LockMode>();
      Dictionary<string, string[]> keyColumnNames = dialect.ForUpdateOfColumns ? new Dictionary<string, string[]>() : (Dictionary<string, string[]>) null;
      string[] aliases = this.Aliases;
      for (int index = 0; index < aliases.Length; ++index)
      {
        LockMode lockMode;
        if (lockModes.TryGetValue(aliases[index], out lockMode))
        {
          ILockable entityPersister = (ILockable) this.EntityPersisters[index];
          string rootTableAlias = entityPersister.GetRootTableAlias(aliases[index]);
          dictionary[rootTableAlias] = lockMode;
          if (keyColumnNames != null)
            keyColumnNames[rootTableAlias] = entityPersister.RootTableIdentifierColumnNames;
        }
      }
      return dialect.ApplyLocksToSql(sqlSelectString, lockModes, (IDictionary<string, string[]>) keyColumnNames);
    }

    public override LockMode[] GetLockModes(IDictionary<string, LockMode> lockModes)
    {
      string[] aliases = this.Aliases;
      if (aliases == null)
        return (LockMode[]) null;
      int length = aliases.Length;
      LockMode[] lockModes1 = new LockMode[length];
      for (int index = 0; index < length; ++index)
      {
        LockMode none;
        if (!lockModes.TryGetValue(aliases[index], out none))
          none = LockMode.None;
        lockModes1[index] = none;
      }
      return lockModes1;
    }

    public override IList GetResultList(IList results, IResultTransformer customResultTransformer)
    {
      if (customResultTransformer == null)
        return results;
      for (int index = 0; index < results.Count; ++index)
      {
        if (!(results[index] is object[] objArray))
          objArray = new object[1]{ results[index] };
        object[] tuple = objArray;
        object obj = customResultTransformer.TransformTuple(tuple, this.translator.HasProjection ? this.translator.ProjectedAliases : this.userAliases);
        results[index] = obj;
      }
      return customResultTransformer.TransformList(results);
    }

    protected override IEnumerable<IParameterSpecification> GetParameterSpecifications()
    {
      return (IEnumerable<IParameterSpecification>) this.translator.CollectedParameterSpecifications;
    }
  }
}

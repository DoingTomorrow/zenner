// Decompiled with JetBrains decompiler
// Type: NHibernate.Criterion.SubqueryExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Loader.Criteria;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Criterion
{
  [Serializable]
  public abstract class SubqueryExpression : AbstractCriterion
  {
    private readonly CriteriaImpl criteriaImpl;
    private readonly string quantifier;
    private readonly bool prefixOp;
    private readonly string op;
    private QueryParameters parameters;
    private IType[] types;
    [NonSerialized]
    private CriteriaQueryTranslator innerQuery;

    protected SubqueryExpression(string op, string quantifier, DetachedCriteria dc)
      : this(op, quantifier, dc, true)
    {
    }

    protected SubqueryExpression(string op, string quantifier, DetachedCriteria dc, bool prefixOp)
    {
      this.criteriaImpl = dc.GetCriteriaImpl();
      this.quantifier = quantifier;
      this.prefixOp = prefixOp;
      this.op = op;
    }

    public IType[] GetTypes() => this.types;

    protected abstract SqlString ToLeftSqlString(ICriteria criteria, ICriteriaQuery outerQuery);

    public override SqlString ToSqlString(
      ICriteria criteria,
      ICriteriaQuery criteriaQuery,
      IDictionary<string, IFilter> enabledFilters)
    {
      this.InitializeInnerQueryAndParameters(criteriaQuery);
      if (!this.innerQuery.HasProjection)
        throw new QueryException("Cannot use subqueries on a criteria without a projection.");
      ISessionFactoryImplementor factory = criteriaQuery.Factory;
      CriteriaJoinWalker criteriaJoinWalker = new CriteriaJoinWalker((IOuterJoinLoadable) factory.GetEntityPersister(this.criteriaImpl.EntityOrClassName), this.innerQuery, factory, (ICriteria) this.criteriaImpl, this.criteriaImpl.EntityOrClassName, enabledFilters);
      this.parameters = this.innerQuery.GetQueryParameters();
      SqlString sqlString = criteriaJoinWalker.SqlString;
      if (this.criteriaImpl.FirstResult != 0 || this.criteriaImpl.MaxResults != RowSelection.NoValue)
      {
        int? offsetUsingDialect = NHibernate.Loader.Loader.GetOffsetUsingDialect(this.parameters.RowSelection, factory.Dialect);
        int? limitUsingDialect = NHibernate.Loader.Loader.GetLimitUsingDialect(this.parameters.RowSelection, factory.Dialect);
        Parameter skipParameter = offsetUsingDialect.HasValue ? this.innerQuery.CreateSkipParameter(offsetUsingDialect.Value) : (Parameter) null;
        Parameter takeParameter = limitUsingDialect.HasValue ? this.innerQuery.CreateTakeParameter(limitUsingDialect.Value) : (Parameter) null;
        sqlString = factory.Dialect.GetLimitString(sqlString, offsetUsingDialect, limitUsingDialect, skipParameter, takeParameter);
      }
      this.innerQuery = (CriteriaQueryTranslator) null;
      SqlStringBuilder sqlStringBuilder = new SqlStringBuilder().Add(this.ToLeftSqlString(criteria, criteriaQuery));
      if (this.op != null)
        sqlStringBuilder.Add(" ").Add(this.op).Add(" ");
      if (this.quantifier != null && this.prefixOp)
        sqlStringBuilder.Add(this.quantifier).Add(" ");
      sqlStringBuilder.Add("(").Add(sqlString).Add(")");
      if (this.quantifier != null && !this.prefixOp)
        sqlStringBuilder.Add(" ").Add(this.quantifier);
      return sqlStringBuilder.ToSqlString();
    }

    public override string ToString()
    {
      return this.prefixOp ? string.Format("{0} {1} ({2})", (object) this.op, (object) this.quantifier, (object) this.criteriaImpl) : string.Format("{0} ({1}) {2}", (object) this.op, (object) this.criteriaImpl, (object) this.quantifier);
    }

    public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
    {
      return this.parameters.NamedParameters.Values.ToArray<TypedValue>();
    }

    public override IProjection[] GetProjections() => (IProjection[]) null;

    public void InitializeInnerQueryAndParameters(ICriteriaQuery criteriaQuery)
    {
      if (this.innerQuery != null)
        return;
      this.innerQuery = new CriteriaQueryTranslator(criteriaQuery.Factory, this.criteriaImpl, this.criteriaImpl.EntityOrClassName, criteriaQuery.GenerateSQLAlias(), criteriaQuery);
      this.types = this.innerQuery.HasProjection ? this.innerQuery.ProjectedTypes : (IType[]) null;
    }

    public ICriteria Criteria => (ICriteria) this.criteriaImpl;
  }
}

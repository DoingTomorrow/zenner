// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.QueryModel
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Collections;
using Remotion.Linq.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq
{
  public class QueryModel : ICloneable
  {
    private readonly UniqueIdentifierGenerator _uniqueIdentifierGenerator;
    private MainFromClause _mainFromClause;
    private SelectClause _selectClause;

    public QueryModel(MainFromClause mainFromClause, SelectClause selectClause)
    {
      ArgumentUtility.CheckNotNull<MainFromClause>(nameof (mainFromClause), mainFromClause);
      ArgumentUtility.CheckNotNull<SelectClause>(nameof (selectClause), selectClause);
      this._uniqueIdentifierGenerator = new UniqueIdentifierGenerator();
      this.MainFromClause = mainFromClause;
      this.SelectClause = selectClause;
      this.BodyClauses = new ObservableCollection<IBodyClause>();
      this.BodyClauses.ItemInserted += new EventHandler<ObservableCollectionChangedEventArgs<IBodyClause>>(this.BodyClauses_Added);
      this.BodyClauses.ItemSet += new EventHandler<ObservableCollectionChangedEventArgs<IBodyClause>>(this.BodyClauses_Added);
      this.ResultOperators = new ObservableCollection<ResultOperatorBase>();
      this.ResultOperators.ItemInserted += new EventHandler<ObservableCollectionChangedEventArgs<ResultOperatorBase>>(this.ResultOperators_ItemAdded);
      this.ResultOperators.ItemSet += new EventHandler<ObservableCollectionChangedEventArgs<ResultOperatorBase>>(this.ResultOperators_ItemAdded);
    }

    public Type ResultTypeOverride { get; set; }

    public Type GetResultType() => this.GetOutputDataInfo().DataType;

    public IStreamedDataInfo GetOutputDataInfo()
    {
      IStreamedDataInfo streamedDataInfo = this.ResultOperators.Aggregate<ResultOperatorBase, IStreamedDataInfo>((IStreamedDataInfo) this.SelectClause.GetOutputDataInfo(), (Func<IStreamedDataInfo, ResultOperatorBase, IStreamedDataInfo>) ((current, resultOperator) => resultOperator.GetOutputDataInfo(current)));
      return this.ResultTypeOverride != null ? streamedDataInfo.AdjustDataType(this.ResultTypeOverride) : streamedDataInfo;
    }

    public MainFromClause MainFromClause
    {
      get => this._mainFromClause;
      set
      {
        ArgumentUtility.CheckNotNull<MainFromClause>(nameof (value), value);
        this._mainFromClause = value;
        this._uniqueIdentifierGenerator.AddKnownIdentifier(value.ItemName);
      }
    }

    public SelectClause SelectClause
    {
      get => this._selectClause;
      set
      {
        ArgumentUtility.CheckNotNull<SelectClause>(nameof (value), value);
        this._selectClause = value;
      }
    }

    public ObservableCollection<IBodyClause> BodyClauses { get; private set; }

    public ObservableCollection<ResultOperatorBase> ResultOperators { get; private set; }

    public UniqueIdentifierGenerator GetUniqueIdentfierGenerator()
    {
      return this._uniqueIdentifierGenerator;
    }

    private void ResultOperators_ItemAdded(
      object sender,
      ObservableCollectionChangedEventArgs<ResultOperatorBase> e)
    {
      ArgumentUtility.CheckNotNull<ResultOperatorBase>("e.Item", e.Item);
    }

    public void Accept(IQueryModelVisitor visitor)
    {
      ArgumentUtility.CheckNotNull<IQueryModelVisitor>(nameof (visitor), visitor);
      visitor.VisitQueryModel(this);
    }

    public override string ToString()
    {
      string seed;
      if (this.IsIdentityQuery())
        seed = FormattingExpressionTreeVisitor.Format(this.MainFromClause.FromExpression);
      else
        seed = this.MainFromClause.ToString() + this.BodyClauses.Aggregate<IBodyClause, string>("", (Func<string, IBodyClause, string>) ((s, b) => s + " " + (object) b)) + " " + (object) this.SelectClause;
      return this.ResultOperators.Aggregate<ResultOperatorBase, string>(seed, (Func<string, ResultOperatorBase, string>) ((s, r) => s + " => " + (object) r));
    }

    public QueryModel Clone() => this.Clone(new QuerySourceMapping());

    public QueryModel Clone(QuerySourceMapping querySourceMapping)
    {
      ArgumentUtility.CheckNotNull<QuerySourceMapping>(nameof (querySourceMapping), querySourceMapping);
      CloneContext cloneContext = new CloneContext(querySourceMapping);
      QueryModelBuilder queryModelBuilder = new QueryModelBuilder();
      queryModelBuilder.AddClause((IClause) this.MainFromClause.Clone(cloneContext));
      foreach (IBodyClause bodyClause in (Collection<IBodyClause>) this.BodyClauses)
        queryModelBuilder.AddClause((IClause) bodyClause.Clone(cloneContext));
      queryModelBuilder.AddClause((IClause) this.SelectClause.Clone(cloneContext));
      foreach (ResultOperatorBase resultOperator1 in (Collection<ResultOperatorBase>) this.ResultOperators)
      {
        ResultOperatorBase resultOperator2 = resultOperator1.Clone(cloneContext);
        queryModelBuilder.AddResultOperator(resultOperator2);
      }
      QueryModel queryModel = queryModelBuilder.Build();
      queryModel.TransformExpressions((Func<Expression, Expression>) (ex => CloningExpressionTreeVisitor.AdjustExpressionAfterCloning(ex, cloneContext.QuerySourceMapping)));
      queryModel.ResultTypeOverride = this.ResultTypeOverride;
      return queryModel;
    }

    object ICloneable.Clone() => (object) this.Clone();

    public void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.MainFromClause.TransformExpressions(transformation);
      foreach (IClause bodyClause in (Collection<IBodyClause>) this.BodyClauses)
        bodyClause.TransformExpressions(transformation);
      this.SelectClause.TransformExpressions(transformation);
      foreach (ResultOperatorBase resultOperator in (Collection<ResultOperatorBase>) this.ResultOperators)
        resultOperator.TransformExpressions(transformation);
    }

    public string GetNewName(string prefix)
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (prefix), prefix);
      return this._uniqueIdentifierGenerator.GetUniqueIdentifier(prefix);
    }

    private void BodyClauses_Added(
      object sender,
      ObservableCollectionChangedEventArgs<IBodyClause> e)
    {
      ArgumentUtility.CheckNotNull<IBodyClause>("e.Item", e.Item);
      if (!(e.Item is FromClauseBase fromClauseBase))
        return;
      this._uniqueIdentifierGenerator.AddKnownIdentifier(fromClauseBase.ItemName);
    }

    public IStreamedData Execute(IQueryExecutor executor)
    {
      ArgumentUtility.CheckNotNull<IQueryExecutor>(nameof (executor), executor);
      return this.GetOutputDataInfo().ExecuteQueryModel(this, executor);
    }

    public bool IsIdentityQuery()
    {
      return this.BodyClauses.Count == 0 && this.SelectClause.Selector is QuerySourceReferenceExpression && ((QuerySourceReferenceExpression) this.SelectClause.Selector).ReferencedQuerySource == this.MainFromClause;
    }

    public QueryModel ConvertToSubQuery(string itemName)
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (itemName), itemName);
      if (!(this.GetOutputDataInfo() is StreamedSequenceInfo outputDataInfo))
        throw new InvalidOperationException(string.Format("The query must return a sequence of items, but it selects a single object of type '{0}'.", (object) this.GetOutputDataInfo().DataType));
      MainFromClause mainFromClause = new MainFromClause(itemName, outputDataInfo.ResultItemType, (Expression) new SubQueryExpression(this));
      SelectClause selectClause = new SelectClause((Expression) new QuerySourceReferenceExpression((IQuerySource) mainFromClause));
      return new QueryModel(mainFromClause, selectClause);
    }
  }
}

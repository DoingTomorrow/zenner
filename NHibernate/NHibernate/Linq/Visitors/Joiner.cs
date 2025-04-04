// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.Joiner
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  public class Joiner : IJoiner
  {
    private readonly Dictionary<string, NhJoinClause> _joins = new Dictionary<string, NhJoinClause>();
    private readonly NameGenerator _nameGenerator;
    private readonly QueryModel _queryModel;

    internal Joiner(QueryModel queryModel)
    {
      this._nameGenerator = new NameGenerator(queryModel);
      this._queryModel = queryModel;
    }

    public IEnumerable<NhJoinClause> Joins => (IEnumerable<NhJoinClause>) this._joins.Values;

    public Expression AddJoin(Expression expression, string key)
    {
      NhJoinClause nhJoinClause;
      if (!this._joins.TryGetValue(key, out nhJoinClause))
      {
        nhJoinClause = new NhJoinClause(this._nameGenerator.GetNewName(), expression.Type, expression);
        this._queryModel.BodyClauses.Add((IBodyClause) nhJoinClause);
        this._joins.Add(key, nhJoinClause);
      }
      return (Expression) new QuerySourceReferenceExpression((IQuerySource) nhJoinClause);
    }

    public void MakeInnerIfJoined(string key)
    {
      if (!this._joins.ContainsKey(key))
        return;
      this._joins[key].MakeInner();
    }

    public bool CanAddJoin(Expression expression)
    {
      IQuerySource querySource = Joiner.QuerySourceExtractor.GetQuerySource(expression);
      if (this._queryModel.MainFromClause == querySource || querySource is IBodyClause bodyClause && this._queryModel.BodyClauses.Contains(bodyClause))
        return true;
      return querySource is ResultOperatorBase resultOperatorBase && this._queryModel.ResultOperators.Contains(resultOperatorBase);
    }

    private class QuerySourceExtractor : NhExpressionTreeVisitor
    {
      private IQuerySource _querySource;

      public static IQuerySource GetQuerySource(Expression expression)
      {
        Joiner.QuerySourceExtractor querySourceExtractor = new Joiner.QuerySourceExtractor();
        querySourceExtractor.VisitExpression(expression);
        return querySourceExtractor._querySource;
      }

      protected override Expression VisitQuerySourceReferenceExpression(
        QuerySourceReferenceExpression expression)
      {
        this._querySource = expression.ReferencedQuerySource;
        return base.VisitQuerySourceReferenceExpression(expression);
      }
    }
  }
}

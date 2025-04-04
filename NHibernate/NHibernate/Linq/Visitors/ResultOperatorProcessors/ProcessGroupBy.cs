// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.ResultOperatorProcessors.ProcessGroupBy
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast;
using Remotion.Linq.Clauses.ResultOperators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors.ResultOperatorProcessors
{
  public class ProcessGroupBy : IResultOperatorProcessor<GroupResultOperator>
  {
    public void Process(
      GroupResultOperator resultOperator,
      QueryModelVisitor queryModelVisitor,
      IntermediateHqlTree tree)
    {
      IEnumerable<Expression> source1;
      if (resultOperator.KeySelector is NewExpression)
        source1 = (IEnumerable<Expression>) (resultOperator.KeySelector as NewExpression).Arguments;
      else
        source1 = (IEnumerable<Expression>) new Expression[1]
        {
          resultOperator.KeySelector
        };
      IEnumerable<HqlExpression> source2 = source1.Select<Expression, HqlExpression>((Func<Expression, HqlExpression>) (k => HqlGeneratorExpressionTreeVisitor.Visit(k, queryModelVisitor.VisitorParameters).AsExpression()));
      tree.AddGroupByClause(tree.TreeBuilder.GroupBy(source2.ToArray<HqlExpression>()));
    }
  }
}

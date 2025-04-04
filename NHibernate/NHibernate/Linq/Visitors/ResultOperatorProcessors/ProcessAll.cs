// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.ResultOperatorProcessors.ProcessAll
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast;
using Remotion.Linq.Clauses.ResultOperators;

#nullable disable
namespace NHibernate.Linq.Visitors.ResultOperatorProcessors
{
  public class ProcessAll : IResultOperatorProcessor<AllResultOperator>
  {
    public void Process(
      AllResultOperator resultOperator,
      QueryModelVisitor queryModelVisitor,
      IntermediateHqlTree tree)
    {
      tree.AddWhereClause((HqlBooleanExpression) tree.TreeBuilder.BooleanNot(HqlGeneratorExpressionTreeVisitor.Visit(resultOperator.Predicate, queryModelVisitor.VisitorParameters).AsBooleanExpression()));
      tree.SetRoot((HqlTreeNode) tree.TreeBuilder.BooleanNot((HqlBooleanExpression) tree.TreeBuilder.Exists((HqlQuery) tree.Root)));
    }
  }
}

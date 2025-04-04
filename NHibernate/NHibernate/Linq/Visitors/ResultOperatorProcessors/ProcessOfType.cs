// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.ResultOperatorProcessors.ProcessOfType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast;
using Remotion.Linq.Clauses.ResultOperators;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors.ResultOperatorProcessors
{
  public class ProcessOfType : IResultOperatorProcessor<OfTypeResultOperator>
  {
    public void Process(
      OfTypeResultOperator resultOperator,
      QueryModelVisitor queryModelVisitor,
      IntermediateHqlTree tree)
    {
      Expression itemExpression = queryModelVisitor.Model.SelectClause.GetOutputDataInfo().ItemExpression;
      tree.AddWhereClause((HqlBooleanExpression) tree.TreeBuilder.Equality((HqlExpression) tree.TreeBuilder.Dot(HqlGeneratorExpressionTreeVisitor.Visit(itemExpression, queryModelVisitor.VisitorParameters).AsExpression(), (HqlExpression) tree.TreeBuilder.Class()), (HqlExpression) tree.TreeBuilder.Ident(resultOperator.SearchedItemType.FullName)));
    }
  }
}

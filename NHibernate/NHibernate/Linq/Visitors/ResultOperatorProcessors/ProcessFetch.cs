// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.ResultOperatorProcessors.ProcessFetch
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast;
using Remotion.Linq.Clauses;
using Remotion.Linq.EagerFetching;

#nullable disable
namespace NHibernate.Linq.Visitors.ResultOperatorProcessors
{
  public class ProcessFetch
  {
    public void Process(
      FetchRequestBase resultOperator,
      QueryModelVisitor queryModelVisitor,
      IntermediateHqlTree tree)
    {
      IQuerySource querySource = QuerySourceLocator.FindQuerySource(queryModelVisitor.Model, resultOperator.RelationMember.DeclaringType);
      this.Process(resultOperator, queryModelVisitor, tree, querySource.ItemName);
    }

    public void Process(
      FetchRequestBase resultOperator,
      QueryModelVisitor queryModelVisitor,
      IntermediateHqlTree tree,
      string sourceAlias)
    {
      HqlDot expression = tree.TreeBuilder.Dot((HqlExpression) tree.TreeBuilder.Ident(sourceAlias), (HqlExpression) tree.TreeBuilder.Ident(resultOperator.RelationMember.Name));
      string newName = queryModelVisitor.Model.GetNewName("_");
      tree.AddFromClause((HqlTreeNode) tree.TreeBuilder.LeftFetchJoin((HqlExpression) expression, tree.TreeBuilder.Alias(newName)));
      tree.AddDistinctRootOperator();
      foreach (FetchRequestBase innerFetchRequest in resultOperator.InnerFetchRequests)
        this.Process(innerFetchRequest, queryModelVisitor, tree, newName);
    }
  }
}

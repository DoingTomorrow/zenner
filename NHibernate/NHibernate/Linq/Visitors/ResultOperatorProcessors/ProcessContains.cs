// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.ResultOperatorProcessors.ProcessContains
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast;
using NHibernate.Param;
using Remotion.Linq.Clauses.ResultOperators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq.Visitors.ResultOperatorProcessors
{
  public class ProcessContains : IResultOperatorProcessor<ContainsResultOperator>
  {
    public void Process(
      ContainsResultOperator resultOperator,
      QueryModelVisitor queryModelVisitor,
      IntermediateHqlTree tree)
    {
      HqlExpression hqlExpression = HqlGeneratorExpressionTreeVisitor.Visit(resultOperator.Item, queryModelVisitor.VisitorParameters).AsExpression();
      HqlTreeNode source = ProcessContains.GetFromRangeClause(tree.Root).Children.First<HqlTreeNode>();
      if (source is HqlParameter)
      {
        if (ProcessContains.IsEmptyList((HqlParameter) source, queryModelVisitor.VisitorParameters))
          tree.SetRoot((HqlTreeNode) tree.TreeBuilder.Equality((HqlExpression) tree.TreeBuilder.Constant((object) 1), (HqlExpression) tree.TreeBuilder.Constant((object) 0)));
        else
          tree.SetRoot((HqlTreeNode) tree.TreeBuilder.In(hqlExpression, source));
      }
      else if (hqlExpression is HqlParameter)
      {
        tree.AddWhereClause((HqlBooleanExpression) tree.TreeBuilder.Equality((HqlExpression) tree.TreeBuilder.Ident(ProcessContains.GetFromAlias(tree.Root).AstNode.Text), hqlExpression));
        tree.SetRoot((HqlTreeNode) tree.TreeBuilder.Exists((HqlQuery) tree.Root));
      }
      else
        tree.SetRoot((HqlTreeNode) tree.TreeBuilder.In(hqlExpression, tree.Root));
    }

    private static HqlRange GetFromRangeClause(HqlTreeNode node)
    {
      return node.NodesPreOrder.OfType<HqlRange>().First<HqlRange>();
    }

    private static HqlAlias GetFromAlias(HqlTreeNode node)
    {
      return node.NodesPreOrder.Single<HqlTreeNode>((Func<HqlTreeNode, bool>) (n => n is HqlRange)).Children.Single<HqlTreeNode>((Func<HqlTreeNode, bool>) (n => n is HqlAlias)) as HqlAlias;
    }

    private static bool IsEmptyList(HqlParameter source, VisitorParameters parameters)
    {
      string parameterName = source.NodesPreOrder.Single<HqlTreeNode>((Func<HqlTreeNode, bool>) (n => n is HqlIdent)).AstNode.Text;
      return !((IEnumerable) parameters.ConstantToParameterMap.Single<KeyValuePair<ConstantExpression, NamedParameter>>((Func<KeyValuePair<ConstantExpression, NamedParameter>, bool>) (p => p.Value.Name == parameterName)).Key.Value).Cast<object>().Any<object>();
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.SelectExpressionList
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using NHibernate.Hql.Ast.ANTLR.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public abstract class SelectExpressionList(IToken token) : HqlSqlWalkerNode(token)
  {
    public ISelectExpression[] CollectSelectExpressions() => this.CollectSelectExpressions(false);

    public ISelectExpression[] CollectSelectExpressions(bool recurse)
    {
      IASTNode selectExpression1 = this.GetFirstSelectExpression();
      List<ISelectExpression> selectExpressionList = new List<ISelectExpression>(this.ChildCount);
      for (IASTNode ast = selectExpression1; ast != null; ast = ast.NextSibling)
      {
        if (recurse)
        {
          switch (ast)
          {
            case ConstructorNode constructorNode:
              for (IASTNode astNode = constructorNode.GetChild(1); astNode != null; astNode = astNode.NextSibling)
              {
                if (astNode is ISelectExpression selectExpression2)
                  selectExpressionList.Add(selectExpression2);
              }
              continue;
            case ISelectExpression selectExpression3:
              selectExpressionList.Add(selectExpression3);
              continue;
            default:
              throw new InvalidOperationException("Unexpected AST: " + ast.GetType().FullName + " " + new ASTPrinter().ShowAsString(ast, ""));
          }
        }
        else
        {
          if (!(ast is ISelectExpression selectExpression4))
            throw new InvalidOperationException("Unexpected AST: " + ast.GetType().FullName + " " + new ASTPrinter().ShowAsString(ast, ""));
          selectExpressionList.Add(selectExpression4);
        }
      }
      return selectExpressionList.ToArray();
    }

    protected internal abstract IASTNode GetFirstSelectExpression();
  }
}

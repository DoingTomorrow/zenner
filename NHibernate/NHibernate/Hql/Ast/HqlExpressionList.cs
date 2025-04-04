// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.HqlExpressionList
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast.ANTLR.Tree;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Hql.Ast
{
  public class HqlExpressionList : HqlStatement
  {
    public HqlExpressionList(IASTFactory factory, params HqlExpression[] expressions)
      : base(75, "expr_list", factory, (HqlTreeNode[]) expressions)
    {
    }

    public HqlExpressionList(IASTFactory factory, IEnumerable<HqlExpression> expressions)
      : base(75, "expr_list", factory, expressions.Cast<HqlTreeNode>())
    {
    }
  }
}

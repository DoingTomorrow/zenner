// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.HqlCoalesce
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast.ANTLR.Tree;

#nullable disable
namespace NHibernate.Hql.Ast
{
  public class HqlCoalesce : HqlExpression
  {
    public HqlCoalesce(IASTFactory factory, HqlExpression lhs, HqlExpression rhs)
      : base(81, "coalesce", factory)
    {
      this.AddChild((HqlTreeNode) new HqlIdent(factory, "coalesce"));
      this.AddChild((HqlTreeNode) new HqlExpressionList(factory, new HqlExpression[2]
      {
        lhs,
        rhs
      }));
    }
  }
}

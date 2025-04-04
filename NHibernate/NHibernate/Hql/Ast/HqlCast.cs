// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.HqlCast
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast.ANTLR.Tree;
using System;

#nullable disable
namespace NHibernate.Hql.Ast
{
  public class HqlCast : HqlExpression
  {
    public HqlCast(IASTFactory factory, HqlExpression expression, Type type)
      : base(81, "method", factory)
    {
      this.AddChild((HqlTreeNode) new HqlIdent(factory, "cast"));
      this.AddChild((HqlTreeNode) new HqlExpressionList(factory, new HqlExpression[2]
      {
        expression,
        (HqlExpression) new HqlIdent(factory, type)
      }));
    }
  }
}

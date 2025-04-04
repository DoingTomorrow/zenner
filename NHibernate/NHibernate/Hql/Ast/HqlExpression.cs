// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.HqlExpression
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast.ANTLR.Tree;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Hql.Ast
{
  public abstract class HqlExpression : HqlTreeNode
  {
    protected HqlExpression(
      int type,
      string text,
      IASTFactory factory,
      IEnumerable<HqlTreeNode> children)
      : base(type, text, factory, children)
    {
    }

    protected HqlExpression(
      int type,
      string text,
      IASTFactory factory,
      params HqlTreeNode[] children)
      : base(type, text, factory, children)
    {
    }
  }
}

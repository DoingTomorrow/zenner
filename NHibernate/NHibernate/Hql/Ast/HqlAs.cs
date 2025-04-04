// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.HqlAs
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Hql.Ast.ANTLR.Tree;
using System;

#nullable disable
namespace NHibernate.Hql.Ast
{
  public class HqlAs : HqlExpression
  {
    public HqlAs(IASTFactory factory, HqlExpression expression, Type type)
      : base(7, "as", factory, (HqlTreeNode) expression)
    {
      if (Type.GetTypeCode(type) != TypeCode.Int32)
        throw new InvalidOperationException();
      this.AddChild((HqlTreeNode) new HqlIdent(factory, "integer"));
    }
  }
}

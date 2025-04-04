// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.LiteralNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using NHibernate.Hql.Ast.ANTLR.Util;
using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class LiteralNode(IToken token) : AbstractSelectExpression(token)
  {
    public override void SetScalarColumnText(int i)
    {
      ColumnHelper.GenerateSingleScalarColumn(this.ASTFactory, (IASTNode) this, i);
    }

    public override IType DataType
    {
      get
      {
        switch (this.Type)
        {
          case 20:
          case 51:
            return (IType) NHibernateUtil.Boolean;
          case 95:
            return (IType) NHibernateUtil.Int32;
          case 96:
            return (IType) NHibernateUtil.Double;
          case 97:
            return (IType) NHibernateUtil.Decimal;
          case 98:
            return (IType) NHibernateUtil.Single;
          case 99:
            return (IType) NHibernateUtil.Int64;
          case 124:
            return (IType) NHibernateUtil.String;
          default:
            return (IType) null;
        }
      }
      set => base.DataType = value;
    }
  }
}

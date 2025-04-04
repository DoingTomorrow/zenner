// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.CaseNode
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
  public class CaseNode(IToken token) : AbstractSelectExpression(token), ISelectExpression
  {
    public override IType DataType
    {
      get
      {
        for (int index = 0; index < this.ChildCount; ++index)
        {
          IASTNode child1 = this.GetChild(index);
          if (child1.Type == 61)
          {
            IASTNode child2 = child1.GetChild(1);
            if (child2 is ISelectExpression && !(child2 is ParameterNode))
              return (child2 as ISelectExpression).DataType;
          }
          else
          {
            IASTNode astNode = child1.Type == 59 ? child1.GetChild(0) : throw new HibernateException("Was expecting a WHEN or ELSE, but found a: " + child1.Text);
            if (astNode is ISelectExpression && !(astNode is ParameterNode))
              return (astNode as ISelectExpression).DataType;
          }
        }
        throw new HibernateException("Unable to determine data type of CASE statement.");
      }
      set => base.DataType = value;
    }

    public override void SetScalarColumnText(int i)
    {
      ColumnHelper.GenerateSingleScalarColumn(this.ASTFactory, (IASTNode) this, i);
    }
  }
}

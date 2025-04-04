// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.AggregateNode
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
  public class AggregateNode(IToken token) : AbstractSelectExpression(token), ISelectExpression
  {
    public override IType DataType
    {
      get => this.SessionFactoryHelper.FindFunctionReturnType(this.Text, this.GetChild(0));
      set => base.DataType = value;
    }

    public override void SetScalarColumnText(int i)
    {
      ColumnHelper.GenerateSingleScalarColumn(this.ASTFactory, (IASTNode) this, i);
    }
  }
}

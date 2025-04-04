// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.BetweenOperatorNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class BetweenOperatorNode(IToken token) : SqlNode(token), IOperatorNode
  {
    public void Initialize()
    {
      IASTNode fixtureOperand = this.GetFixtureOperand();
      if (fixtureOperand == null)
        throw new SemanticException("fixture operand of a between operator was null");
      IASTNode lowOperand = this.GetLowOperand();
      if (lowOperand == null)
        throw new SemanticException("low operand of a between operator was null");
      IASTNode highOperand = this.GetHighOperand();
      if (highOperand == null)
        throw new SemanticException("high operand of a between operator was null");
      BetweenOperatorNode.Check(fixtureOperand, lowOperand, highOperand);
      BetweenOperatorNode.Check(lowOperand, highOperand, fixtureOperand);
      BetweenOperatorNode.Check(highOperand, fixtureOperand, lowOperand);
    }

    public override IType DataType
    {
      get => (IType) NHibernateUtil.Boolean;
      set => base.DataType = value;
    }

    private IASTNode GetFixtureOperand() => this.GetChild(0);

    private IASTNode GetLowOperand() => this.GetChild(1);

    private IASTNode GetHighOperand() => this.GetChild(2);

    private static void Check(IASTNode check, IASTNode first, IASTNode second)
    {
      if (!typeof (IExpectedTypeAwareNode).IsAssignableFrom(check.GetType()))
        return;
      IType type = (IType) null;
      if (typeof (SqlNode).IsAssignableFrom(first.GetType()))
        type = ((SqlNode) first).DataType;
      if (type == null && typeof (SqlNode).IsAssignableFrom(second.GetType()))
        type = ((SqlNode) second).DataType;
      ((IExpectedTypeAwareNode) check).ExpectedType = type;
    }
  }
}

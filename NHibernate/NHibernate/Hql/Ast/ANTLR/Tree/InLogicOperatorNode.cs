// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.InLogicOperatorNode
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
  public class InLogicOperatorNode(IToken token) : 
    BinaryLogicOperatorNode(token),
    IBinaryOperatorNode,
    IOperatorNode
  {
    private IASTNode InList => this.RightHandOperand;

    public override void Initialize()
    {
      IASTNode leftHandOperand = this.LeftHandOperand;
      if (leftHandOperand == null)
        throw new SemanticException("left-hand operand of in operator was null");
      IASTNode inList = this.InList;
      if (inList == null)
        throw new SemanticException("right-hand operand of in operator was null");
      if (!typeof (SqlNode).IsAssignableFrom(leftHandOperand.GetType()))
        return;
      IType dataType = ((SqlNode) leftHandOperand).DataType;
      for (IASTNode astNode = inList.GetChild(0); astNode != null; astNode = astNode.NextSibling)
      {
        if (typeof (IExpectedTypeAwareNode).IsAssignableFrom(astNode.GetType()))
          ((IExpectedTypeAwareNode) astNode).ExpectedType = dataType;
      }
    }
  }
}

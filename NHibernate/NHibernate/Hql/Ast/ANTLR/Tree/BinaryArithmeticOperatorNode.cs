// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.BinaryArithmeticOperatorNode
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
  public class BinaryArithmeticOperatorNode(IToken token) : 
    AbstractSelectExpression(token),
    IBinaryOperatorNode,
    IOperatorNode,
    IDisplayableNode
  {
    public void Initialize()
    {
      IASTNode leftHandOperand = this.LeftHandOperand;
      IASTNode rightHandOperand = this.RightHandOperand;
      if (leftHandOperand == null)
        throw new SemanticException("left-hand operand of a binary operator was null");
      if (rightHandOperand == null)
        throw new SemanticException("right-hand operand of a binary operator was null");
      IType dataType1 = leftHandOperand is SqlNode ? ((SqlNode) leftHandOperand).DataType : (IType) null;
      IType dataType2 = rightHandOperand is SqlNode ? ((SqlNode) rightHandOperand).DataType : (IType) null;
      if (typeof (IExpectedTypeAwareNode).IsAssignableFrom(leftHandOperand.GetType()) && dataType2 != null)
      {
        IType type = !BinaryArithmeticOperatorNode.IsDateTimeType(dataType2) ? dataType2 : (this.Type == 118 ? (IType) NHibernateUtil.Double : dataType2);
        ((IExpectedTypeAwareNode) leftHandOperand).ExpectedType = type;
      }
      else
      {
        if (!typeof (ParameterNode).IsAssignableFrom(rightHandOperand.GetType()) || dataType1 == null)
          return;
        IType type = (IType) null;
        if (BinaryArithmeticOperatorNode.IsDateTimeType(dataType1))
        {
          if (this.Type == 118)
            type = (IType) NHibernateUtil.Double;
        }
        else
          type = dataType1;
        ((IExpectedTypeAwareNode) rightHandOperand).ExpectedType = type;
      }
    }

    public override IType DataType
    {
      get
      {
        if (base.DataType == null)
          base.DataType = this.ResolveDataType();
        return base.DataType;
      }
      set => base.DataType = value;
    }

    private IType ResolveDataType()
    {
      IASTNode leftHandOperand = this.LeftHandOperand;
      IASTNode rightHandOperand = this.RightHandOperand;
      IType dataType1 = leftHandOperand is SqlNode ? ((SqlNode) leftHandOperand).DataType : (IType) null;
      IType dataType2 = rightHandOperand is SqlNode ? ((SqlNode) rightHandOperand).DataType : (IType) null;
      if (BinaryArithmeticOperatorNode.IsDateTimeType(dataType1) || BinaryArithmeticOperatorNode.IsDateTimeType(dataType2))
        return this.ResolveDateTimeArithmeticResultType(dataType1, dataType2);
      if (dataType1 == null)
        return dataType2 ?? (IType) NHibernateUtil.Double;
      if (dataType2 == null)
        return dataType1;
      if (dataType1 == NHibernateUtil.Double || dataType2 == NHibernateUtil.Double)
        return (IType) NHibernateUtil.Double;
      if (dataType1 == NHibernateUtil.Decimal || dataType2 == NHibernateUtil.Decimal)
        return (IType) NHibernateUtil.Decimal;
      if (dataType1 == NHibernateUtil.Int64 || dataType2 == NHibernateUtil.Int64)
        return (IType) NHibernateUtil.Int64;
      return dataType1 == NHibernateUtil.Int32 || dataType2 == NHibernateUtil.Int32 ? (IType) NHibernateUtil.Int32 : dataType1;
    }

    private static bool IsDateTimeType(IType type)
    {
      return type != null && typeof (DateTime).IsAssignableFrom(type.ReturnedClass);
    }

    private IType ResolveDateTimeArithmeticResultType(IType lhType, IType rhType)
    {
      bool flag1 = BinaryArithmeticOperatorNode.IsDateTimeType(lhType);
      bool flag2 = BinaryArithmeticOperatorNode.IsDateTimeType(rhType);
      if (this.Type == 118)
        return !flag1 ? rhType : lhType;
      if (this.Type == 119)
      {
        if (flag1 && !flag2)
          return lhType;
        if (flag1 && flag2)
          return (IType) NHibernateUtil.Double;
      }
      return (IType) null;
    }

    public override void SetScalarColumnText(int i)
    {
      ColumnHelper.GenerateSingleScalarColumn(this.ASTFactory, (IASTNode) this, i);
    }

    public IASTNode LeftHandOperand => this.GetChild(0);

    public IASTNode RightHandOperand => this.GetChild(1);

    public string GetDisplayText() => "{dataType=" + (object) this.DataType + "}";
  }
}

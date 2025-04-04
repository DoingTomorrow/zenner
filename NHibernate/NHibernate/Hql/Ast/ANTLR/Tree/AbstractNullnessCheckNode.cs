// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.AbstractNullnessCheckNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using NHibernate.Engine;
using NHibernate.Type;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public abstract class AbstractNullnessCheckNode(IToken token) : UnaryLogicOperatorNode(token)
  {
    public override void Initialize()
    {
      IType dataType = AbstractNullnessCheckNode.ExtractDataType(this.Operand);
      if (dataType == null)
        return;
      ISessionFactoryImplementor factory = this.SessionFactoryHelper.Factory;
      int columnSpan = dataType.GetColumnSpan((IMapping) factory);
      if (columnSpan <= 1)
        return;
      this.MutateRowValueConstructorSyntax(columnSpan);
    }

    protected abstract int ExpansionConnectorType { get; }

    protected abstract string ExpansionConnectorText { get; }

    private void MutateRowValueConstructorSyntax(int operandColumnSpan)
    {
      int type = this.Type;
      string text = this.Text;
      int expansionConnectorType = this.ExpansionConnectorType;
      string expansionConnectorText = this.ExpansionConnectorText;
      this.Type = expansionConnectorType;
      this.Text = expansionConnectorText;
      string[] mutationTexts = AbstractNullnessCheckNode.ExtractMutationTexts(this.Operand, operandColumnSpan);
      IASTNode astNode = (IASTNode) this;
      astNode.ClearChildren();
      for (int index = operandColumnSpan - 1; index > 0; --index)
      {
        if (index == 1)
        {
          astNode.AddChildren(this.ASTFactory.CreateNode(type, text, this.ASTFactory.CreateNode(143, mutationTexts[0])), this.ASTFactory.CreateNode(type, text, this.ASTFactory.CreateNode(143, mutationTexts[1])));
        }
        else
        {
          astNode.AddChildren(this.ASTFactory.CreateNode(expansionConnectorType, expansionConnectorText), this.ASTFactory.CreateNode(type, text, this.ASTFactory.CreateNode(143, mutationTexts[index])));
          astNode = this.GetChild(0);
        }
      }
    }

    private static IType ExtractDataType(IASTNode operand)
    {
      IType dataType = (IType) null;
      if (operand is SqlNode)
        dataType = ((SqlNode) operand).DataType;
      if (dataType == null && operand is IExpectedTypeAwareNode)
        dataType = ((IExpectedTypeAwareNode) operand).ExpectedType;
      return dataType;
    }

    private static string[] ExtractMutationTexts(IASTNode operand, int count)
    {
      if (operand is ParameterNode)
      {
        string[] mutationTexts = new string[count];
        for (int index = 0; index < count; ++index)
          mutationTexts[index] = "?";
        return mutationTexts;
      }
      if (operand.Type == 92)
      {
        string[] mutationTexts = new string[operand.ChildCount];
        int num = 0;
        foreach (IASTNode astNode in (IEnumerable<IASTNode>) operand)
          mutationTexts[num++] = astNode.Text;
        return mutationTexts;
      }
      string str = operand is SqlNode ? operand.Text : throw new HibernateException("dont know how to extract row value elements from node : " + (object) operand);
      if (str.StartsWith("("))
        str = str.Substring(1);
      if (str.EndsWith(")"))
        str = str.Substring(0, str.Length - 1);
      string[] mutationTexts1 = str.Split(new string[1]
      {
        ", "
      }, StringSplitOptions.None);
      if (count != mutationTexts1.Length)
        throw new HibernateException("SqlNode's text did not reference expected number of columns");
      return mutationTexts1;
    }
  }
}

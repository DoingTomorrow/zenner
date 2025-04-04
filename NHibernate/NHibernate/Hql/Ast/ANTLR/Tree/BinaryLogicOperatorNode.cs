// Decompiled with JetBrains decompiler
// Type: NHibernate.Hql.Ast.ANTLR.Tree.BinaryLogicOperatorNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Antlr.Runtime;
using NHibernate.Engine;
using NHibernate.Param;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace NHibernate.Hql.Ast.ANTLR.Tree
{
  [CLSCompliant(false)]
  public class BinaryLogicOperatorNode(IToken token) : 
    HqlSqlWalkerNode(token),
    IBinaryOperatorNode,
    IOperatorNode,
    IParameterContainer
  {
    private List<IParameterSpecification> embeddedParameters;

    public IASTNode LeftHandOperand => this.GetChild(0);

    public IASTNode RightHandOperand => this.GetChild(1);

    public virtual void Initialize()
    {
      IASTNode leftHandOperand = this.LeftHandOperand;
      if (leftHandOperand == null)
        throw new SemanticException("left-hand operand of a binary operator was null");
      IASTNode rightHandOperand = this.RightHandOperand;
      if (rightHandOperand == null)
        throw new SemanticException("right-hand operand of a binary operator was null");
      this.ProcessMetaTypeDiscriminatorIfNecessary(leftHandOperand, rightHandOperand);
      IType lhsType = BinaryLogicOperatorNode.ExtractDataType(leftHandOperand);
      IType rhsType = BinaryLogicOperatorNode.ExtractDataType(rightHandOperand);
      if (lhsType == null)
        lhsType = rhsType;
      if (rhsType == null)
        rhsType = lhsType;
      if (leftHandOperand is IExpectedTypeAwareNode expectedTypeAwareNode1)
        expectedTypeAwareNode1.ExpectedType = rhsType;
      if (rightHandOperand is IExpectedTypeAwareNode expectedTypeAwareNode2)
        expectedTypeAwareNode2.ExpectedType = lhsType;
      this.MutateRowValueConstructorSyntaxesIfNecessary(lhsType, rhsType);
    }

    protected void MutateRowValueConstructorSyntaxesIfNecessary(IType lhsType, IType rhsType)
    {
      ISessionFactoryImplementor factory = this.SessionFactoryHelper.Factory;
      if (lhsType == null || rhsType == null)
        return;
      int columnSpan1 = lhsType.GetColumnSpan((IMapping) factory);
      int columnSpan2 = rhsType.GetColumnSpan((IMapping) factory);
      if (columnSpan1 != columnSpan2 && !BinaryLogicOperatorNode.AreCompatibleEntityTypes(lhsType, rhsType))
        throw new TypeMismatchException("left and right hand sides of a binary logic operator were incompatibile [" + lhsType.Name + " : " + rhsType.Name + "]");
      if (columnSpan1 <= 1 || factory.Dialect.SupportsRowValueConstructorSyntax)
        return;
      this.MutateRowValueConstructorSyntax(columnSpan1);
    }

    private static bool AreCompatibleEntityTypes(IType lhsType, IType rhsType)
    {
      if (!lhsType.IsEntityType || !rhsType.IsEntityType)
        return false;
      return lhsType.ReturnedClass.IsAssignableFrom(rhsType.ReturnedClass) || rhsType.ReturnedClass.IsAssignableFrom(lhsType.ReturnedClass);
    }

    private void MutateRowValueConstructorSyntax(int valueElements)
    {
      string comparisonText = "==".Equals(this.Text) ? "=" : this.Text;
      this.Type = 143;
      string[] mutationTexts1 = BinaryLogicOperatorNode.ExtractMutationTexts(this.LeftHandOperand, valueElements);
      string[] mutationTexts2 = BinaryLogicOperatorNode.ExtractMutationTexts(this.RightHandOperand, valueElements);
      IParameterSpecification parameterSpecification1 = !(this.LeftHandOperand is ParameterNode leftHandOperand) ? (IParameterSpecification) null : leftHandOperand.HqlParameterSpecification;
      IParameterSpecification parameterSpecification2 = !(this.RightHandOperand is ParameterNode rightHandOperand) ? (IParameterSpecification) null : rightHandOperand.HqlParameterSpecification;
      string str = this.Translate(valueElements, comparisonText, mutationTexts1, mutationTexts2);
      if (parameterSpecification1 != null)
        this.AddEmbeddedParameter(parameterSpecification1);
      if (parameterSpecification2 != null)
        this.AddEmbeddedParameter(parameterSpecification2);
      this.ClearChildren();
      this.Text = str;
    }

    public override SqlString RenderText(ISessionFactoryImplementor sessionFactory)
    {
      if (!this.HasEmbeddedParameters)
        return base.RenderText(sessionFactory);
      SqlString sqlString = SqlString.Parse(this.Text);
      Parameter[] array = sqlString.GetParameters().ToArray<Parameter>();
      int num = 0;
      foreach (string str in this.embeddedParameters.SelectMany<IParameterSpecification, string>((Func<IParameterSpecification, IEnumerable<string>>) (specification => specification.GetIdsForBackTrack((IMapping) sessionFactory))))
        array[num++].BackTrack = (object) str;
      return sqlString;
    }

    public void AddEmbeddedParameter(IParameterSpecification specification)
    {
      if (this.embeddedParameters == null)
        this.embeddedParameters = new List<IParameterSpecification>();
      this.embeddedParameters.Add(specification);
    }

    public bool HasEmbeddedParameters
    {
      get => this.embeddedParameters != null && this.embeddedParameters.Count != 0;
    }

    public IParameterSpecification[] GetEmbeddedParameters() => this.embeddedParameters.ToArray();

    private string Translate(
      int valueElements,
      string comparisonText,
      string[] lhsElementTexts,
      string[] rhsElementTexts)
    {
      List<string> stringList = new List<string>();
      for (int index = 0; index < valueElements; ++index)
        stringList.Add(string.Format("{0} {1} {2}", (object) lhsElementTexts[index], (object) comparisonText, (object) rhsElementTexts[index]));
      return "(" + string.Join(" and ", stringList.ToArray()) + ")";
    }

    private static string[] ExtractMutationTexts(IASTNode operand, int count)
    {
      if (operand is ParameterNode)
        return Enumerable.Repeat<string>("?", count).ToArray<string>();
      if (operand is SqlNode)
      {
        string str = operand.Text;
        if (str.StartsWith("("))
          str = str.Substring(1);
        if (str.EndsWith(")"))
          str = str.Substring(0, str.Length - 1);
        string[] mutationTexts = str.Split(new string[1]
        {
          ", "
        }, StringSplitOptions.None);
        if (count != mutationTexts.Length)
          throw new HibernateException("SqlNode's text did not reference expected number of columns");
        return mutationTexts;
      }
      string[] mutationTexts1 = operand.Type == 92 ? new string[operand.ChildCount] : throw new HibernateException("dont know how to extract row value elements from node : " + (object) operand);
      for (int index = 0; index < operand.ChildCount; ++index)
        mutationTexts1[index] = operand.GetChild(index).Text;
      return mutationTexts1;
    }

    protected static IType ExtractDataType(IASTNode operand)
    {
      IType dataType = (IType) null;
      if (operand is SqlNode sqlNode)
        dataType = sqlNode.DataType;
      IExpectedTypeAwareNode expectedTypeAwareNode = operand as IExpectedTypeAwareNode;
      if (dataType == null && expectedTypeAwareNode != null)
        dataType = expectedTypeAwareNode.ExpectedType;
      return dataType;
    }

    private void ProcessMetaTypeDiscriminatorIfNecessary(IASTNode lhs, IASTNode rhs)
    {
      SqlNode sqlNode1 = lhs as SqlNode;
      SqlNode sqlNode2 = rhs as SqlNode;
      if (sqlNode1 == null || sqlNode2 == null)
        return;
      if (sqlNode2.Text == null && sqlNode1.DataType is MetaType dataType1)
      {
        string importedClassName = this.SessionFactoryHelper.GetImportedClassName(sqlNode2.OriginalText);
        object metaValue = dataType1.GetMetaValue(TypeNameParser.Parse(importedClassName).Type);
        sqlNode2.Text = metaValue.ToString();
      }
      else
      {
        if (sqlNode1.Text != null || !(sqlNode2.DataType is MetaType dataType))
          return;
        string importedClassName = this.SessionFactoryHelper.GetImportedClassName(sqlNode1.OriginalText);
        object metaValue = dataType.GetMetaValue(TypeNameParser.Parse(importedClassName).Type);
        sqlNode1.Text = metaValue.ToString();
      }
    }
  }
}

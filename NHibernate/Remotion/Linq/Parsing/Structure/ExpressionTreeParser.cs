// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.ExpressionTreeParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation;
using Remotion.Linq.Parsing.Structure.ExpressionTreeProcessors;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using Remotion.Linq.Parsing.Structure.NodeTypeProviders;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.Structure
{
  public class ExpressionTreeParser
  {
    private static readonly MethodInfo s_getArrayLengthMethod = typeof (Array).GetMethod("get_Length");
    private readonly UniqueIdentifierGenerator _identifierGenerator = new UniqueIdentifierGenerator();
    private readonly INodeTypeProvider _nodeTypeProvider;
    private readonly IExpressionTreeProcessor _processor;
    private readonly MethodCallExpressionParser _methodCallExpressionParser;

    [Obsolete("This method has been removed. Use QueryParser.CreateDefault, or create a customized ExpressionTreeParser using the constructor. (1.13.93)", true)]
    public static ExpressionTreeParser CreateDefault() => throw new NotImplementedException();

    public static CompoundNodeTypeProvider CreateDefaultNodeTypeProvider()
    {
      Type[] types = typeof (MethodInfoBasedNodeTypeRegistry).Assembly.GetTypes();
      return new CompoundNodeTypeProvider((IEnumerable<INodeTypeProvider>) new INodeTypeProvider[2]
      {
        (INodeTypeProvider) MethodInfoBasedNodeTypeRegistry.CreateFromTypes((IEnumerable<Type>) types),
        (INodeTypeProvider) MethodNameBasedNodeTypeRegistry.CreateFromTypes(types)
      });
    }

    public static CompoundExpressionTreeProcessor CreateDefaultProcessor(
      IExpressionTranformationProvider tranformationProvider)
    {
      ArgumentUtility.CheckNotNull<IExpressionTranformationProvider>(nameof (tranformationProvider), tranformationProvider);
      return new CompoundExpressionTreeProcessor((IEnumerable<IExpressionTreeProcessor>) new IExpressionTreeProcessor[2]
      {
        (IExpressionTreeProcessor) new PartialEvaluatingExpressionTreeProcessor(),
        (IExpressionTreeProcessor) new TransformingExpressionTreeProcessor(tranformationProvider)
      });
    }

    public ExpressionTreeParser(
      INodeTypeProvider nodeTypeProvider,
      IExpressionTreeProcessor processor)
    {
      ArgumentUtility.CheckNotNull<INodeTypeProvider>(nameof (nodeTypeProvider), nodeTypeProvider);
      ArgumentUtility.CheckNotNull<IExpressionTreeProcessor>(nameof (processor), processor);
      this._nodeTypeProvider = nodeTypeProvider;
      this._processor = processor;
      this._methodCallExpressionParser = new MethodCallExpressionParser(this._nodeTypeProvider);
    }

    public INodeTypeProvider NodeTypeProvider => this._nodeTypeProvider;

    public IExpressionTreeProcessor Processor => this._processor;

    public IExpressionNode ParseTree(Expression expressionTree)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expressionTree), expressionTree);
      return expressionTree.Type != typeof (void) ? this.ParseNode(this._processor.Process(expressionTree), (string) null) : throw new ParserException(string.Format("Expressions of type void ('{0}') are not supported.", (object) expressionTree));
    }

    public MethodCallExpression GetQueryOperatorExpression(Expression expression)
    {
      switch (expression)
      {
        case MethodCallExpression operatorExpression:
          return operatorExpression;
        case MemberExpression memberExpression:
          if (!(memberExpression.Member is PropertyInfo member))
            return (MethodCallExpression) null;
          MethodInfo getMethod = member.GetGetMethod();
          return getMethod == null || !this._nodeTypeProvider.IsRegistered(getMethod) ? (MethodCallExpression) null : Expression.Call(memberExpression.Expression, getMethod);
        case UnaryExpression unaryExpression:
          if (unaryExpression.NodeType == ExpressionType.ArrayLength && this._nodeTypeProvider.IsRegistered(ExpressionTreeParser.s_getArrayLengthMethod))
            return Expression.Call(unaryExpression.Operand, ExpressionTreeParser.s_getArrayLengthMethod);
          break;
      }
      return (MethodCallExpression) null;
    }

    private IExpressionNode ParseNode(Expression expression, string associatedIdentifier)
    {
      if (string.IsNullOrEmpty(associatedIdentifier))
        associatedIdentifier = this._identifierGenerator.GetUniqueIdentifier("<generated>_");
      MethodCallExpression operatorExpression = this.GetQueryOperatorExpression(expression);
      return operatorExpression != null ? this.ParseMethodCallExpression(operatorExpression, associatedIdentifier) : this.ParseNonQueryOperatorExpression(expression, associatedIdentifier);
    }

    private IExpressionNode ParseMethodCallExpression(
      MethodCallExpression methodCallExpression,
      string associatedIdentifier)
    {
      string associatedIdentifier1 = this.InferAssociatedIdentifierForSource(methodCallExpression);
      Expression expression;
      IEnumerable<Expression> arguments;
      if (methodCallExpression.Object != null)
      {
        expression = methodCallExpression.Object;
        arguments = (IEnumerable<Expression>) methodCallExpression.Arguments;
      }
      else
      {
        expression = methodCallExpression.Arguments[0];
        arguments = methodCallExpression.Arguments.Skip<Expression>(1);
      }
      IExpressionNode node = this.ParseNode(expression, associatedIdentifier1);
      return this._methodCallExpressionParser.Parse(associatedIdentifier, node, arguments, methodCallExpression);
    }

    private IExpressionNode ParseNonQueryOperatorExpression(
      Expression expression,
      string associatedIdentifier)
    {
      Expression expression1 = SubQueryFindingExpressionTreeVisitor.Process(expression, this._nodeTypeProvider);
      try
      {
        return (IExpressionNode) new MainSourceExpressionNode(associatedIdentifier, expression1);
      }
      catch (ArgumentTypeException ex)
      {
        throw new ParserException(string.Format("Cannot parse expression '{0}' as it has an unsupported type. Only query sources (that is, expressions that implement IEnumerable) and query operators can be parsed.", (object) expression1), (Exception) ex);
      }
    }

    private string InferAssociatedIdentifierForSource(MethodCallExpression methodCallExpression)
    {
      LambdaExpression lambdaArgument = this.GetLambdaArgument(methodCallExpression);
      return lambdaArgument != null && lambdaArgument.Parameters.Count == 1 ? lambdaArgument.Parameters[0].Name : (string) null;
    }

    private LambdaExpression GetLambdaArgument(MethodCallExpression methodCallExpression)
    {
      return methodCallExpression.Arguments.Select<Expression, LambdaExpression>((Func<Expression, LambdaExpression>) (argument => this.GetLambdaExpression(argument))).FirstOrDefault<LambdaExpression>((Func<LambdaExpression, bool>) (lambdaExpression => lambdaExpression != null));
    }

    private LambdaExpression GetLambdaExpression(Expression expression)
    {
      switch (expression)
      {
        case LambdaExpression lambdaExpression:
          return lambdaExpression;
        case UnaryExpression unaryExpression:
          return unaryExpression.Operand as LambdaExpression;
        default:
          return (LambdaExpression) null;
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.MethodCallExpressionParser
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Parsing.Structure
{
  public class MethodCallExpressionParser
  {
    private readonly INodeTypeProvider _nodeTypeProvider;

    public MethodCallExpressionParser(INodeTypeProvider nodeTypeProvider)
    {
      ArgumentUtility.CheckNotNull<INodeTypeProvider>(nameof (nodeTypeProvider), nodeTypeProvider);
      this._nodeTypeProvider = nodeTypeProvider;
    }

    public IExpressionNode Parse(
      string associatedIdentifier,
      IExpressionNode source,
      IEnumerable<Expression> arguments,
      MethodCallExpression expressionToParse)
    {
      ArgumentUtility.CheckNotNull<MethodCallExpression>(nameof (expressionToParse), expressionToParse);
      Type nodeType = this.GetNodeType(expressionToParse);
      Expression[] array = arguments.Select<Expression, Expression>((Func<Expression, Expression>) (expr => this.ProcessArgumentExpression(expr))).ToArray<Expression>();
      MethodCallExpressionParseInfo parseInfo = new MethodCallExpressionParseInfo(associatedIdentifier, source, expressionToParse);
      return this.CreateExpressionNode(nodeType, parseInfo, (object[]) array);
    }

    private Type GetNodeType(MethodCallExpression expressionToParse)
    {
      return this._nodeTypeProvider.GetNodeType(expressionToParse.Method) ?? throw new ParserException(string.Format("Could not parse expression '{0}': This overload of the method '{1}.{2}' is currently not supported.", (object) FormattingExpressionTreeVisitor.Format((Expression) expressionToParse), (object) expressionToParse.Method.DeclaringType.FullName, (object) expressionToParse.Method.Name));
    }

    private Expression ProcessArgumentExpression(Expression argumentExpression)
    {
      return SubQueryFindingExpressionTreeVisitor.Process(this.UnwrapArgumentExpression(argumentExpression), this._nodeTypeProvider);
    }

    private Expression UnwrapArgumentExpression(Expression expression)
    {
      if (expression.NodeType == ExpressionType.Quote)
        return ((UnaryExpression) expression).Operand;
      return expression.NodeType == ExpressionType.Constant && ((ConstantExpression) expression).Value is LambdaExpression ? (Expression) ((ConstantExpression) expression).Value : expression;
    }

    private IExpressionNode CreateExpressionNode(
      Type nodeType,
      MethodCallExpressionParseInfo parseInfo,
      object[] additionalConstructorParameters)
    {
      try
      {
        return MethodCallExpressionNodeFactory.CreateExpressionNode(nodeType, parseInfo, additionalConstructorParameters);
      }
      catch (ExpressionNodeInstantiationException ex)
      {
        throw new ParserException(string.Format("Could not parse expression '{0}': {1}", (object) FormattingExpressionTreeVisitor.Format((Expression) parseInfo.ParsedExpression), (object) ex.Message));
      }
    }
  }
}

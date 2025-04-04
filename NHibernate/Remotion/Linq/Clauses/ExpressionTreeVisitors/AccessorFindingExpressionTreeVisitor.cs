// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ExpressionTreeVisitors.AccessorFindingExpressionTreeVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Parsing;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Clauses.ExpressionTreeVisitors
{
  public class AccessorFindingExpressionTreeVisitor : ExpressionTreeVisitor
  {
    private readonly Expression _searchedExpression;
    private readonly ParameterExpression _inputParameter;
    private readonly Stack<Expression> _accessorPathStack = new Stack<Expression>();

    public static LambdaExpression FindAccessorLambda(
      Expression searchedExpression,
      Expression fullExpression,
      ParameterExpression inputParameter)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (searchedExpression), searchedExpression);
      ArgumentUtility.CheckNotNull<Expression>(nameof (fullExpression), fullExpression);
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (inputParameter), inputParameter);
      if (inputParameter.Type != fullExpression.Type)
        throw new ArgumentTypeException("The inputParameter's type must match the fullExpression's type.", nameof (inputParameter), fullExpression.Type, inputParameter.Type);
      AccessorFindingExpressionTreeVisitor expressionTreeVisitor = new AccessorFindingExpressionTreeVisitor(searchedExpression, inputParameter);
      expressionTreeVisitor.VisitExpression(fullExpression);
      return expressionTreeVisitor.AccessorPath != null ? expressionTreeVisitor.AccessorPath : throw new ArgumentException(string.Format("The given expression '{0}' does not contain the searched expression '{1}' in a nested NewExpression with member assignments or a MemberBindingExpression.", (object) FormattingExpressionTreeVisitor.Format(fullExpression), (object) FormattingExpressionTreeVisitor.Format(searchedExpression)), nameof (fullExpression));
    }

    private AccessorFindingExpressionTreeVisitor(
      Expression searchedExpression,
      ParameterExpression inputParameter)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (searchedExpression), searchedExpression);
      ArgumentUtility.CheckNotNull<ParameterExpression>(nameof (inputParameter), inputParameter);
      this._searchedExpression = searchedExpression;
      this._inputParameter = inputParameter;
      this._accessorPathStack.Push((Expression) this._inputParameter);
    }

    public LambdaExpression AccessorPath { get; private set; }

    public override Expression VisitExpression(Expression expression)
    {
      if (object.Equals((object) expression, (object) this._searchedExpression))
      {
        this.AccessorPath = Expression.Lambda(this._accessorPathStack.Peek(), this._inputParameter);
        return expression;
      }
      switch (expression)
      {
        case NewExpression _:
        case MemberInitExpression _:
        case UnaryExpression _:
          return base.VisitExpression(expression);
        default:
          return expression;
      }
    }

    protected override Expression VisitNewExpression(NewExpression expression)
    {
      if (expression.Members != null && expression.Members.Count > 0)
      {
        for (int index = 0; index < expression.Members.Count; ++index)
          this.CheckAndVisitMemberAssignment(expression.Members[index], expression.Arguments[index]);
      }
      return (Expression) expression;
    }

    protected override Expression VisitUnaryExpression(UnaryExpression expression)
    {
      if (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked)
      {
        this._accessorPathStack.Push((Expression) Expression.Convert(this._accessorPathStack.Peek(), expression.Operand.Type));
        base.VisitUnaryExpression(expression);
        this._accessorPathStack.Pop();
      }
      return (Expression) expression;
    }

    protected override MemberBinding VisitMemberBinding(MemberBinding memberBinding)
    {
      return memberBinding is MemberAssignment ? base.VisitMemberBinding(memberBinding) : memberBinding;
    }

    protected override MemberBinding VisitMemberAssignment(MemberAssignment memberAssigment)
    {
      this.CheckAndVisitMemberAssignment(memberAssigment.Member, memberAssigment.Expression);
      return (MemberBinding) memberAssigment;
    }

    private void CheckAndVisitMemberAssignment(MemberInfo member, Expression expression)
    {
      this._accessorPathStack.Push(this.GetMemberAccessExpression(this._accessorPathStack.Peek(), member));
      this.VisitExpression(expression);
      this._accessorPathStack.Pop();
    }

    private Expression GetMemberAccessExpression(Expression input, MemberInfo member)
    {
      return member is MethodInfo method ? (Expression) Expression.Call(input, method) : (Expression) Expression.MakeMemberAccess(input, member);
    }
  }
}

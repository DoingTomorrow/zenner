// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.ExpressionKeyVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Param;
using Remotion.Linq.Clauses.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  public class ExpressionKeyVisitor : NhExpressionTreeVisitor
  {
    private readonly IDictionary<ConstantExpression, NamedParameter> _constantToParameterMap;
    private readonly StringBuilder _string = new StringBuilder();
    private bool insideSelectClause;

    private ExpressionKeyVisitor(
      IDictionary<ConstantExpression, NamedParameter> constantToParameterMap)
    {
      this._constantToParameterMap = constantToParameterMap;
    }

    public static string Visit(
      Expression expression,
      IDictionary<ConstantExpression, NamedParameter> parameters)
    {
      ExpressionKeyVisitor expressionKeyVisitor = new ExpressionKeyVisitor(parameters);
      expressionKeyVisitor.VisitExpression(expression);
      return expressionKeyVisitor.ToString();
    }

    public override string ToString() => this._string.ToString();

    protected override Expression VisitBinaryExpression(BinaryExpression expression)
    {
      if (expression.Method != null)
      {
        this._string.Append(expression.Method.DeclaringType.Name);
        this._string.Append(".");
        this.VisitMethod(expression.Method);
      }
      else
        this._string.Append((object) expression.NodeType);
      this._string.Append("(");
      this.VisitExpression(expression.Left);
      this._string.Append(", ");
      this.VisitExpression(expression.Right);
      this._string.Append(")");
      return (Expression) expression;
    }

    protected override Expression VisitConditionalExpression(ConditionalExpression expression)
    {
      this.VisitExpression(expression.Test);
      this._string.Append(" ? ");
      this.VisitExpression(expression.IfTrue);
      this._string.Append(" : ");
      this.VisitExpression(expression.IfFalse);
      return (Expression) expression;
    }

    protected override Expression VisitConstantExpression(ConstantExpression expression)
    {
      NamedParameter namedParameter;
      if (this._constantToParameterMap.TryGetValue(expression, out namedParameter) && !this.insideSelectClause)
      {
        if (namedParameter.Value == null)
          this._string.Append("NULL");
        if (namedParameter.Value is IEnumerable && !((IEnumerable) namedParameter.Value).Cast<object>().Any<object>())
          this._string.Append("EmptyList");
        else
          this._string.Append(namedParameter.Name);
      }
      else if (expression.Value == null)
        this._string.Append("NULL");
      else
        this._string.Append(expression.Value);
      return base.VisitConstantExpression(expression);
    }

    protected override ElementInit VisitElementInit(ElementInit elementInit)
    {
      return base.VisitElementInit(elementInit);
    }

    private T AppendCommas<T>(T expression) where T : Expression
    {
      this.VisitExpression((Expression) expression);
      this._string.Append(", ");
      return expression;
    }

    protected override Expression VisitInvocationExpression(InvocationExpression expression)
    {
      return base.VisitInvocationExpression(expression);
    }

    protected override Expression VisitLambdaExpression(LambdaExpression expression)
    {
      this._string.Append('(');
      this.VisitList<ParameterExpression>(expression.Parameters, new Func<ParameterExpression, ParameterExpression>(this.AppendCommas<ParameterExpression>));
      this._string.Append(") => (");
      this.VisitExpression(expression.Body);
      this._string.Append(')');
      return (Expression) expression;
    }

    protected override Expression VisitListInitExpression(ListInitExpression expression)
    {
      return base.VisitListInitExpression(expression);
    }

    protected override MemberBinding VisitMemberAssignment(MemberAssignment memberAssigment)
    {
      return base.VisitMemberAssignment(memberAssigment);
    }

    protected override MemberBinding VisitMemberBinding(MemberBinding memberBinding)
    {
      return base.VisitMemberBinding(memberBinding);
    }

    protected override Expression VisitMemberExpression(MemberExpression expression)
    {
      base.VisitMemberExpression(expression);
      this._string.Append('.');
      this._string.Append(expression.Member.Name);
      return (Expression) expression;
    }

    protected override Expression VisitMemberInitExpression(MemberInitExpression expression)
    {
      return base.VisitMemberInitExpression(expression);
    }

    protected override MemberBinding VisitMemberListBinding(MemberListBinding listBinding)
    {
      return base.VisitMemberListBinding(listBinding);
    }

    protected override MemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
    {
      return base.VisitMemberMemberBinding(binding);
    }

    protected override Expression VisitMethodCallExpression(MethodCallExpression expression)
    {
      bool insideSelectClause = this.insideSelectClause;
      switch (expression.Method.Name)
      {
        case "First":
        case "FirstOrDefault":
        case "Single":
        case "SingleOrDefault":
        case "Select":
          this.insideSelectClause = true;
          break;
        default:
          this.insideSelectClause = false;
          break;
      }
      this.VisitExpression(expression.Object);
      this._string.Append('.');
      this.VisitMethod(expression.Method);
      this._string.Append('(');
      this.VisitList<Expression>(expression.Arguments, new Func<Expression, Expression>(this.AppendCommas<Expression>));
      this._string.Append(')');
      this.insideSelectClause = insideSelectClause;
      return (Expression) expression;
    }

    protected override Expression VisitNewArrayExpression(NewArrayExpression expression)
    {
      return base.VisitNewArrayExpression(expression);
    }

    protected override Expression VisitNewExpression(NewExpression expression)
    {
      this._string.Append("new ");
      this._string.Append(expression.Constructor.DeclaringType.Name);
      this._string.Append('(');
      this.VisitList<Expression>(expression.Arguments, new Func<Expression, Expression>(this.AppendCommas<Expression>));
      this._string.Append(')');
      return (Expression) expression;
    }

    protected override Expression VisitParameterExpression(ParameterExpression expression)
    {
      this._string.Append(expression.Name);
      return (Expression) expression;
    }

    protected override Expression VisitTypeBinaryExpression(TypeBinaryExpression expression)
    {
      this._string.Append("IsType(");
      this.VisitExpression(expression.Expression);
      this._string.Append(", ");
      this._string.Append(expression.TypeOperand.FullName);
      this._string.Append(")");
      return (Expression) expression;
    }

    protected override Expression VisitUnaryExpression(UnaryExpression expression)
    {
      this._string.Append((object) expression.NodeType);
      this._string.Append('(');
      this.VisitExpression(expression.Operand);
      this._string.Append(')');
      return (Expression) expression;
    }

    protected override Expression VisitQuerySourceReferenceExpression(
      QuerySourceReferenceExpression expression)
    {
      this._string.Append(expression.ReferencedQuerySource.ItemName);
      return (Expression) expression;
    }

    private void VisitMethod(MethodInfo methodInfo)
    {
      this._string.Append(methodInfo.Name);
      if (!methodInfo.IsGenericMethod)
        return;
      this._string.Append('[');
      this._string.Append(string.Join(",", ((IEnumerable<Type>) methodInfo.GetGenericArguments()).Select<Type, string>((Func<Type, string>) (a => a.FullName)).ToArray<string>()));
      this._string.Append(']');
    }
  }
}

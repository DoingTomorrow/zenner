// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.ThrowingExpressionTreeVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Utilities;
using System;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Parsing
{
  public abstract class ThrowingExpressionTreeVisitor : ExpressionTreeVisitor
  {
    protected abstract Exception CreateUnhandledItemException<T>(
      T unhandledItem,
      string visitMethod);

    protected virtual TResult VisitUnhandledItem<TItem, TResult>(
      TItem unhandledItem,
      string visitMethod,
      Func<TItem, TResult> baseBehavior)
      where TItem : TResult
    {
      ArgumentUtility.CheckNotNull<TItem>(nameof (unhandledItem), unhandledItem);
      ArgumentUtility.CheckNotNullOrEmpty(nameof (visitMethod), visitMethod);
      ArgumentUtility.CheckNotNull<Func<TItem, TResult>>(nameof (baseBehavior), baseBehavior);
      throw this.CreateUnhandledItemException<TItem>(unhandledItem, visitMethod);
    }

    protected internal override Expression VisitExtensionExpression(ExtensionExpression expression)
    {
      return expression.CanReduce ? this.VisitExpression(expression.ReduceAndCheck()) : this.VisitUnhandledItem<ExtensionExpression, Expression>(expression, nameof (VisitExtensionExpression), new Func<ExtensionExpression, Expression>(this.BaseVisitExtensionExpression));
    }

    protected Expression BaseVisitExtensionExpression(ExtensionExpression expression)
    {
      return base.VisitExtensionExpression(expression);
    }

    protected internal override Expression VisitUnknownNonExtensionExpression(Expression expression)
    {
      return expression is ExtensionExpression extensionExpression && extensionExpression.CanReduce ? this.VisitExpression(extensionExpression.ReduceAndCheck()) : this.VisitUnhandledItem<Expression, Expression>(expression, nameof (VisitUnknownNonExtensionExpression), new Func<Expression, Expression>(this.BaseVisitUnknownNonExtensionExpression));
    }

    protected Expression BaseVisitUnknownNonExtensionExpression(Expression expression)
    {
      return base.VisitUnknownNonExtensionExpression(expression);
    }

    protected override Expression VisitUnaryExpression(UnaryExpression expression)
    {
      return this.VisitUnhandledItem<UnaryExpression, Expression>(expression, nameof (VisitUnaryExpression), new Func<UnaryExpression, Expression>(this.BaseVisitUnaryExpression));
    }

    protected Expression BaseVisitUnaryExpression(UnaryExpression expression)
    {
      return base.VisitUnaryExpression(expression);
    }

    protected override Expression VisitBinaryExpression(BinaryExpression expression)
    {
      return this.VisitUnhandledItem<BinaryExpression, Expression>(expression, nameof (VisitBinaryExpression), new Func<BinaryExpression, Expression>(this.BaseVisitBinaryExpression));
    }

    protected Expression BaseVisitBinaryExpression(BinaryExpression expression)
    {
      return base.VisitBinaryExpression(expression);
    }

    protected override Expression VisitTypeBinaryExpression(TypeBinaryExpression expression)
    {
      return this.VisitUnhandledItem<TypeBinaryExpression, Expression>(expression, nameof (VisitTypeBinaryExpression), new Func<TypeBinaryExpression, Expression>(this.BaseVisitTypeBinaryExpression));
    }

    protected Expression BaseVisitTypeBinaryExpression(TypeBinaryExpression expression)
    {
      return base.VisitTypeBinaryExpression(expression);
    }

    protected override Expression VisitConstantExpression(ConstantExpression expression)
    {
      return this.VisitUnhandledItem<ConstantExpression, Expression>(expression, nameof (VisitConstantExpression), new Func<ConstantExpression, Expression>(this.BaseVisitConstantExpression));
    }

    protected Expression BaseVisitConstantExpression(ConstantExpression expression)
    {
      return base.VisitConstantExpression(expression);
    }

    protected override Expression VisitConditionalExpression(ConditionalExpression expression)
    {
      return this.VisitUnhandledItem<ConditionalExpression, Expression>(expression, nameof (VisitConditionalExpression), new Func<ConditionalExpression, Expression>(this.BaseVisitConditionalExpression));
    }

    protected Expression BaseVisitConditionalExpression(ConditionalExpression arg)
    {
      return base.VisitConditionalExpression(arg);
    }

    protected override Expression VisitParameterExpression(ParameterExpression expression)
    {
      return this.VisitUnhandledItem<ParameterExpression, Expression>(expression, nameof (VisitParameterExpression), new Func<ParameterExpression, Expression>(this.BaseVisitParameterExpression));
    }

    protected Expression BaseVisitParameterExpression(ParameterExpression expression)
    {
      return base.VisitParameterExpression(expression);
    }

    protected override Expression VisitLambdaExpression(LambdaExpression expression)
    {
      return this.VisitUnhandledItem<LambdaExpression, Expression>(expression, nameof (VisitLambdaExpression), new Func<LambdaExpression, Expression>(this.BaseVisitLambdaExpression));
    }

    protected Expression BaseVisitLambdaExpression(LambdaExpression expression)
    {
      return base.VisitLambdaExpression(expression);
    }

    protected override Expression VisitMethodCallExpression(MethodCallExpression expression)
    {
      return this.VisitUnhandledItem<MethodCallExpression, Expression>(expression, nameof (VisitMethodCallExpression), new Func<MethodCallExpression, Expression>(this.BaseVisitMethodCallExpression));
    }

    protected Expression BaseVisitMethodCallExpression(MethodCallExpression expression)
    {
      return base.VisitMethodCallExpression(expression);
    }

    protected override Expression VisitInvocationExpression(InvocationExpression expression)
    {
      return this.VisitUnhandledItem<InvocationExpression, Expression>(expression, nameof (VisitInvocationExpression), new Func<InvocationExpression, Expression>(this.BaseVisitInvocationExpression));
    }

    protected Expression BaseVisitInvocationExpression(InvocationExpression expression)
    {
      return base.VisitInvocationExpression(expression);
    }

    protected override Expression VisitMemberExpression(MemberExpression expression)
    {
      return this.VisitUnhandledItem<MemberExpression, Expression>(expression, nameof (VisitMemberExpression), new Func<MemberExpression, Expression>(this.BaseVisitMemberExpression));
    }

    protected Expression BaseVisitMemberExpression(MemberExpression expression)
    {
      return base.VisitMemberExpression(expression);
    }

    protected override Expression VisitNewExpression(NewExpression expression)
    {
      return this.VisitUnhandledItem<NewExpression, Expression>(expression, nameof (VisitNewExpression), new Func<NewExpression, Expression>(this.BaseVisitNewExpression));
    }

    protected Expression BaseVisitNewExpression(NewExpression expression)
    {
      return base.VisitNewExpression(expression);
    }

    protected override Expression VisitNewArrayExpression(NewArrayExpression expression)
    {
      return this.VisitUnhandledItem<NewArrayExpression, Expression>(expression, nameof (VisitNewArrayExpression), new Func<NewArrayExpression, Expression>(this.BaseVisitNewArrayExpression));
    }

    protected Expression BaseVisitNewArrayExpression(NewArrayExpression expression)
    {
      return base.VisitNewArrayExpression(expression);
    }

    protected override Expression VisitMemberInitExpression(MemberInitExpression expression)
    {
      return this.VisitUnhandledItem<MemberInitExpression, Expression>(expression, nameof (VisitMemberInitExpression), new Func<MemberInitExpression, Expression>(this.BaseVisitMemberInitExpression));
    }

    protected Expression BaseVisitMemberInitExpression(MemberInitExpression expression)
    {
      return base.VisitMemberInitExpression(expression);
    }

    protected override Expression VisitListInitExpression(ListInitExpression expression)
    {
      return this.VisitUnhandledItem<ListInitExpression, Expression>(expression, nameof (VisitListInitExpression), new Func<ListInitExpression, Expression>(this.BaseVisitListInitExpression));
    }

    protected Expression BaseVisitListInitExpression(ListInitExpression expression)
    {
      return base.VisitListInitExpression(expression);
    }

    protected override ElementInit VisitElementInit(ElementInit elementInit)
    {
      return this.VisitUnhandledItem<ElementInit, ElementInit>(elementInit, nameof (VisitElementInit), new Func<ElementInit, ElementInit>(this.BaseVisitElementInit));
    }

    protected ElementInit BaseVisitElementInit(ElementInit elementInit)
    {
      return base.VisitElementInit(elementInit);
    }

    protected override MemberBinding VisitMemberAssignment(MemberAssignment memberAssigment)
    {
      return this.VisitUnhandledItem<MemberAssignment, MemberBinding>(memberAssigment, nameof (VisitMemberAssignment), new Func<MemberAssignment, MemberBinding>(this.BaseVisitMemberAssignment));
    }

    protected MemberBinding BaseVisitMemberAssignment(MemberAssignment memberAssigment)
    {
      return base.VisitMemberAssignment(memberAssigment);
    }

    protected override MemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
    {
      return this.VisitUnhandledItem<MemberMemberBinding, MemberBinding>(binding, nameof (VisitMemberMemberBinding), new Func<MemberMemberBinding, MemberBinding>(this.BaseVisitMemberMemberBinding));
    }

    protected MemberBinding BaseVisitMemberMemberBinding(MemberMemberBinding binding)
    {
      return base.VisitMemberMemberBinding(binding);
    }

    protected override MemberBinding VisitMemberListBinding(MemberListBinding listBinding)
    {
      return this.VisitUnhandledItem<MemberListBinding, MemberBinding>(listBinding, nameof (VisitMemberListBinding), new Func<MemberListBinding, MemberBinding>(this.BaseVisitMemberListBinding));
    }

    protected MemberBinding BaseVisitMemberListBinding(MemberListBinding listBinding)
    {
      return base.VisitMemberListBinding(listBinding);
    }

    protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
    {
      return this.VisitUnhandledItem<SubQueryExpression, Expression>(expression, nameof (VisitSubQueryExpression), new Func<SubQueryExpression, Expression>(this.BaseVisitSubQueryExpression));
    }

    protected Expression BaseVisitSubQueryExpression(SubQueryExpression expression)
    {
      return base.VisitSubQueryExpression(expression);
    }

    protected override Expression VisitQuerySourceReferenceExpression(
      QuerySourceReferenceExpression expression)
    {
      return this.VisitUnhandledItem<QuerySourceReferenceExpression, Expression>(expression, nameof (VisitQuerySourceReferenceExpression), new Func<QuerySourceReferenceExpression, Expression>(this.BaseVisitQuerySourceReferenceExpression));
    }

    protected Expression BaseVisitQuerySourceReferenceExpression(
      QuerySourceReferenceExpression expression)
    {
      return base.VisitQuerySourceReferenceExpression(expression);
    }
  }
}

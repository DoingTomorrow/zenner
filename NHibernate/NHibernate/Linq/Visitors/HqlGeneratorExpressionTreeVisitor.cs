// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.Visitors.HqlGeneratorExpressionTreeVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine.Query;
using NHibernate.Hql.Ast;
using NHibernate.Linq.Expressions;
using NHibernate.Linq.Functions;
using NHibernate.Param;
using NHibernate.Type;
using Remotion.Linq.Clauses.Expressions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq.Visitors
{
  public class HqlGeneratorExpressionTreeVisitor : IHqlExpressionVisitor
  {
    private readonly HqlTreeBuilder _hqlTreeBuilder = new HqlTreeBuilder();
    private readonly VisitorParameters _parameters;
    private readonly ILinqToHqlGeneratorsRegistry _functionRegistry;

    public static HqlTreeNode Visit(Expression expression, VisitorParameters parameters)
    {
      return new HqlGeneratorExpressionTreeVisitor(parameters).VisitExpression(expression);
    }

    public HqlGeneratorExpressionTreeVisitor(VisitorParameters parameters)
    {
      this._functionRegistry = parameters.SessionFactory.Settings.LinqToHqlGeneratorsRegistry;
      this._parameters = parameters;
    }

    public ISessionFactory SessionFactory => (ISessionFactory) this._parameters.SessionFactory;

    public HqlTreeNode Visit(Expression expression) => this.VisitExpression(expression);

    protected HqlTreeNode VisitExpression(Expression expression)
    {
      if (expression == null)
        return (HqlTreeNode) null;
      switch (expression.NodeType)
      {
        case ExpressionType.Add:
        case ExpressionType.AddChecked:
        case ExpressionType.And:
        case ExpressionType.AndAlso:
        case ExpressionType.ArrayIndex:
        case ExpressionType.Coalesce:
        case ExpressionType.Divide:
        case ExpressionType.Equal:
        case ExpressionType.ExclusiveOr:
        case ExpressionType.GreaterThan:
        case ExpressionType.GreaterThanOrEqual:
        case ExpressionType.LeftShift:
        case ExpressionType.LessThan:
        case ExpressionType.LessThanOrEqual:
        case ExpressionType.Modulo:
        case ExpressionType.Multiply:
        case ExpressionType.MultiplyChecked:
        case ExpressionType.NotEqual:
        case ExpressionType.Or:
        case ExpressionType.OrElse:
        case ExpressionType.Power:
        case ExpressionType.RightShift:
        case ExpressionType.Subtract:
        case ExpressionType.SubtractChecked:
          return this.VisitBinaryExpression((BinaryExpression) expression);
        case ExpressionType.ArrayLength:
        case ExpressionType.Convert:
        case ExpressionType.ConvertChecked:
        case ExpressionType.Negate:
        case ExpressionType.UnaryPlus:
        case ExpressionType.NegateChecked:
        case ExpressionType.Not:
        case ExpressionType.Quote:
        case ExpressionType.TypeAs:
          return this.VisitUnaryExpression((UnaryExpression) expression);
        case ExpressionType.Call:
          return this.VisitMethodCallExpression((MethodCallExpression) expression);
        case ExpressionType.Conditional:
          return this.VisitConditionalExpression((ConditionalExpression) expression);
        case ExpressionType.Constant:
          return this.VisitConstantExpression((ConstantExpression) expression);
        case ExpressionType.Invoke:
          return this.VisitInvocationExpression((InvocationExpression) expression);
        case ExpressionType.Lambda:
          return this.VisitLambdaExpression((LambdaExpression) expression);
        case ExpressionType.MemberAccess:
          return this.VisitMemberExpression((MemberExpression) expression);
        case ExpressionType.NewArrayInit:
          return this.VisitNewArrayExpression((NewArrayExpression) expression);
        case ExpressionType.Parameter:
          return this.VisitParameterExpression((ParameterExpression) expression);
        case ExpressionType.TypeIs:
          return this.VisitTypeBinaryExpression((TypeBinaryExpression) expression);
        default:
          switch (expression)
          {
            case SubQueryExpression expression1:
              return this.VisitSubQueryExpression(expression1);
            case QuerySourceReferenceExpression expression2:
              return this.VisitQuerySourceReferenceExpression(expression2);
            case VBStringComparisonExpression expression3:
              return this.VisitVBStringComparisonExpression(expression3);
            default:
              switch (expression.NodeType)
              {
                case (ExpressionType) 10000:
                  return this.VisitNhAverage((NhAverageExpression) expression);
                case (ExpressionType) 10001:
                  return this.VisitNhMin((NhMinExpression) expression);
                case (ExpressionType) 10002:
                  return this.VisitNhMax((NhMaxExpression) expression);
                case (ExpressionType) 10003:
                  return this.VisitNhSum((NhSumExpression) expression);
                case (ExpressionType) 10004:
                  return this.VisitNhCount((NhCountExpression) expression);
                case (ExpressionType) 10005:
                  return this.VisitNhDistinct((NhDistinctExpression) expression);
                case (ExpressionType) 10007:
                  return this.VisitNhStar((NhStarExpression) expression);
                default:
                  throw new NotSupportedException(expression.GetType().Name);
              }
          }
      }
    }

    private HqlTreeNode VisitTypeBinaryExpression(TypeBinaryExpression expression)
    {
      return (HqlTreeNode) this._hqlTreeBuilder.Equality((HqlExpression) this._hqlTreeBuilder.Dot(this.Visit(expression.Expression).AsExpression(), (HqlExpression) this._hqlTreeBuilder.Class()), (HqlExpression) this._hqlTreeBuilder.Ident(expression.TypeOperand.FullName));
    }

    protected HqlTreeNode VisitNhStar(NhStarExpression expression)
    {
      return (HqlTreeNode) this._hqlTreeBuilder.Star();
    }

    private HqlTreeNode VisitInvocationExpression(InvocationExpression expression)
    {
      return this.VisitExpression(expression.Expression);
    }

    protected HqlTreeNode VisitNhAverage(NhAverageExpression expression)
    {
      HqlExpression expression1 = this.VisitExpression(expression.Expression).AsExpression();
      if (expression.Type != expression.Expression.Type)
        expression1 = (HqlExpression) this._hqlTreeBuilder.Cast(expression1, expression.Type);
      return (HqlTreeNode) this._hqlTreeBuilder.Cast((HqlExpression) this._hqlTreeBuilder.Average(expression1), expression.Type);
    }

    protected HqlTreeNode VisitNhCount(NhCountExpression expression)
    {
      return (HqlTreeNode) this._hqlTreeBuilder.Cast((HqlExpression) this._hqlTreeBuilder.Count(this.VisitExpression(expression.Expression).AsExpression()), expression.Type);
    }

    protected HqlTreeNode VisitNhMin(NhMinExpression expression)
    {
      return (HqlTreeNode) this._hqlTreeBuilder.Cast((HqlExpression) this._hqlTreeBuilder.Min(this.VisitExpression(expression.Expression).AsExpression()), expression.Type);
    }

    protected HqlTreeNode VisitNhMax(NhMaxExpression expression)
    {
      return (HqlTreeNode) this._hqlTreeBuilder.Cast((HqlExpression) this._hqlTreeBuilder.Max(this.VisitExpression(expression.Expression).AsExpression()), expression.Type);
    }

    protected HqlTreeNode VisitNhSum(NhSumExpression expression)
    {
      return (HqlTreeNode) this._hqlTreeBuilder.Cast((HqlExpression) this._hqlTreeBuilder.Sum(this.VisitExpression(expression.Expression).AsExpression()), expression.Type);
    }

    protected HqlTreeNode VisitNhDistinct(NhDistinctExpression expression)
    {
      HqlGeneratorExpressionTreeVisitor expressionTreeVisitor = new HqlGeneratorExpressionTreeVisitor(this._parameters);
      return (HqlTreeNode) this._hqlTreeBuilder.ExpressionSubTreeHolder((HqlTreeNode) this._hqlTreeBuilder.Distinct(), expressionTreeVisitor.VisitExpression(expression.Expression));
    }

    protected HqlTreeNode VisitQuerySourceReferenceExpression(
      QuerySourceReferenceExpression expression)
    {
      return (HqlTreeNode) this._hqlTreeBuilder.Ident(this._parameters.QuerySourceNamer.GetName(expression.ReferencedQuerySource));
    }

    private HqlTreeNode VisitVBStringComparisonExpression(VBStringComparisonExpression expression)
    {
      return this.VisitExpression(expression.Comparison);
    }

    protected HqlTreeNode VisitBinaryExpression(BinaryExpression expression)
    {
      HqlExpression hqlExpression1 = this.VisitExpression(expression.Left).AsExpression();
      HqlExpression hqlExpression2 = this.VisitExpression(expression.Right).AsExpression();
      switch (expression.NodeType)
      {
        case ExpressionType.Add:
          if (expression.Left.Type != typeof (string) || expression.Right.Type != typeof (string))
            return (HqlTreeNode) this._hqlTreeBuilder.Add(hqlExpression1, hqlExpression2);
          return (HqlTreeNode) this._hqlTreeBuilder.MethodCall("concat", hqlExpression1, hqlExpression2);
        case ExpressionType.And:
          return (HqlTreeNode) this._hqlTreeBuilder.BitwiseAnd(hqlExpression1, hqlExpression2);
        case ExpressionType.AndAlso:
          return (HqlTreeNode) this._hqlTreeBuilder.BooleanAnd(hqlExpression1.AsBooleanExpression(), hqlExpression2.AsBooleanExpression());
        case ExpressionType.Coalesce:
          return this._hqlTreeBuilder.Coalesce(hqlExpression1, hqlExpression2);
        case ExpressionType.Divide:
          return (HqlTreeNode) this._hqlTreeBuilder.Divide(hqlExpression1, hqlExpression2);
        case ExpressionType.Equal:
          return this.TranslateEqualityComparison(expression, hqlExpression1, hqlExpression2, (Func<HqlExpression, HqlTreeNode>) (expr => (HqlTreeNode) this._hqlTreeBuilder.IsNull(expr)), (Func<HqlExpression, HqlExpression, HqlTreeNode>) ((l, r) => (HqlTreeNode) this._hqlTreeBuilder.Equality(l, r)));
        case ExpressionType.GreaterThan:
          return (HqlTreeNode) this._hqlTreeBuilder.GreaterThan(hqlExpression1, hqlExpression2);
        case ExpressionType.GreaterThanOrEqual:
          return (HqlTreeNode) this._hqlTreeBuilder.GreaterThanOrEqual(hqlExpression1, hqlExpression2);
        case ExpressionType.LessThan:
          return (HqlTreeNode) this._hqlTreeBuilder.LessThan(hqlExpression1, hqlExpression2);
        case ExpressionType.LessThanOrEqual:
          return (HqlTreeNode) this._hqlTreeBuilder.LessThanOrEqual(hqlExpression1, hqlExpression2);
        case ExpressionType.Modulo:
          return (HqlTreeNode) this._hqlTreeBuilder.MethodCall("mod", hqlExpression1, hqlExpression2);
        case ExpressionType.Multiply:
          return (HqlTreeNode) this._hqlTreeBuilder.Multiply(hqlExpression1, hqlExpression2);
        case ExpressionType.NotEqual:
          return this.TranslateEqualityComparison(expression, hqlExpression1, hqlExpression2, (Func<HqlExpression, HqlTreeNode>) (expr => (HqlTreeNode) this._hqlTreeBuilder.IsNotNull(expr)), (Func<HqlExpression, HqlExpression, HqlTreeNode>) ((l, r) => (HqlTreeNode) this._hqlTreeBuilder.Inequality(l, r)));
        case ExpressionType.Or:
          return (HqlTreeNode) this._hqlTreeBuilder.BitwiseOr(hqlExpression1, hqlExpression2);
        case ExpressionType.OrElse:
          return (HqlTreeNode) this._hqlTreeBuilder.BooleanOr(hqlExpression1.AsBooleanExpression(), hqlExpression2.AsBooleanExpression());
        case ExpressionType.Subtract:
          return (HqlTreeNode) this._hqlTreeBuilder.Subtract(hqlExpression1, hqlExpression2);
        default:
          throw new InvalidOperationException();
      }
    }

    private HqlTreeNode TranslateEqualityComparison(
      BinaryExpression expression,
      HqlExpression lhs,
      HqlExpression rhs,
      Func<HqlExpression, HqlTreeNode> applyNullComparison,
      Func<HqlExpression, HqlExpression, HqlTreeNode> applyRegularComparison)
    {
      if (expression.Right is ConstantExpression && expression.Right.Type.IsNullableOrReference() && ((ConstantExpression) expression.Right).Value == null)
        rhs = (HqlExpression) null;
      if (expression.Left is ConstantExpression && expression.Left.Type.IsNullableOrReference() && ((ConstantExpression) expression.Left).Value == null)
        lhs = (HqlExpression) null;
      if (lhs is HqlBooleanExpression || rhs is HqlBooleanExpression)
      {
        if (lhs != null)
          lhs = this.GetExpressionForBooleanEquality(expression.Left, lhs);
        if (rhs != null)
          rhs = this.GetExpressionForBooleanEquality(expression.Right, rhs);
      }
      if (lhs == null)
        return applyNullComparison(rhs);
      return rhs == null ? applyNullComparison(lhs) : applyRegularComparison(lhs, rhs);
    }

    private HqlExpression GetExpressionForBooleanEquality(
      Expression @operator,
      HqlExpression original)
    {
      if (@operator is ConstantExpression key)
      {
        NamedParameter namedParameter;
        if (!this._parameters.ConstantToParameterMap.TryGetValue(key, out namedParameter))
          return (HqlExpression) this._hqlTreeBuilder.Constant(key.Value);
        this._parameters.RequiredHqlParameters.Add(new NamedParameterDescriptor(namedParameter.Name, (IType) null, false));
        return this._hqlTreeBuilder.Parameter(namedParameter.Name).AsExpression();
      }
      MemberExpression memberExpression = @operator as MemberExpression;
      if (ExpressionType.MemberAccess.Equals((object) @operator.NodeType) && memberExpression != null && typeof (bool).Equals(memberExpression.Type))
        return original;
      return (HqlExpression) this._hqlTreeBuilder.Case(new HqlWhen[1]
      {
        this._hqlTreeBuilder.When(original, (HqlExpression) this._hqlTreeBuilder.True())
      }, (HqlExpression) this._hqlTreeBuilder.False());
    }

    protected HqlTreeNode VisitUnaryExpression(UnaryExpression expression)
    {
      switch (expression.NodeType)
      {
        case ExpressionType.Convert:
        case ExpressionType.ConvertChecked:
        case ExpressionType.TypeAs:
          return (expression.Operand.Type.IsPrimitive || expression.Operand.Type == typeof (Decimal)) && (expression.Type.IsPrimitive || expression.Type == typeof (Decimal)) ? (HqlTreeNode) this._hqlTreeBuilder.Cast(this.VisitExpression(expression.Operand).AsExpression(), expression.Type) : this.VisitExpression(expression.Operand);
        case ExpressionType.Not:
          return (HqlTreeNode) this._hqlTreeBuilder.BooleanNot(this.VisitExpression(expression.Operand).AsBooleanExpression());
        default:
          throw new NotSupportedException(expression.ToString());
      }
    }

    protected HqlTreeNode VisitMemberExpression(MemberExpression expression)
    {
      if (expression.Member.Name == "Value" && expression.Expression.Type.IsNullable())
        return this.VisitExpression(expression.Expression);
      IHqlGeneratorForProperty generator;
      return this._functionRegistry.TryGetGenerator(expression.Member, out generator) ? generator.BuildHql(expression.Member, expression.Expression, this._hqlTreeBuilder, (IHqlExpressionVisitor) this) : (HqlTreeNode) this._hqlTreeBuilder.Dot(this.VisitExpression(expression.Expression).AsExpression(), (HqlExpression) this._hqlTreeBuilder.Ident(expression.Member.Name));
    }

    protected HqlTreeNode VisitConstantExpression(ConstantExpression expression)
    {
      if (expression.Value != null)
      {
        System.Type type = expression.Value.GetType();
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (NhQueryable<>))
          return (HqlTreeNode) this._hqlTreeBuilder.Ident(type.GetGenericArguments()[0].FullName);
      }
      NamedParameter namedParameter;
      if (!this._parameters.ConstantToParameterMap.TryGetValue(expression, out namedParameter))
        return (HqlTreeNode) this._hqlTreeBuilder.Constant(expression.Value);
      this._parameters.RequiredHqlParameters.Add(new NamedParameterDescriptor(namedParameter.Name, (IType) null, false));
      return namedParameter.Value is bool ? (HqlTreeNode) this._hqlTreeBuilder.Equality(this._hqlTreeBuilder.Parameter(namedParameter.Name).AsExpression(), (HqlExpression) this._hqlTreeBuilder.Constant((object) true)) : (HqlTreeNode) this._hqlTreeBuilder.Parameter(namedParameter.Name).AsExpression();
    }

    protected HqlTreeNode VisitMethodCallExpression(MethodCallExpression expression)
    {
      MethodInfo method = expression.Method;
      IHqlGeneratorForMethod generator;
      if (!this._functionRegistry.TryGetGenerator(method, out generator))
        throw new NotSupportedException(method.ToString());
      return generator.BuildHql(method, expression.Object, expression.Arguments, this._hqlTreeBuilder, (IHqlExpressionVisitor) this);
    }

    protected HqlTreeNode VisitLambdaExpression(LambdaExpression expression)
    {
      return this.VisitExpression(expression.Body);
    }

    protected HqlTreeNode VisitParameterExpression(ParameterExpression expression)
    {
      return (HqlTreeNode) this._hqlTreeBuilder.Ident(expression.Name);
    }

    protected HqlTreeNode VisitConditionalExpression(ConditionalExpression expression)
    {
      HqlExpression predicate = this.VisitExpression(expression.Test).AsExpression();
      HqlExpression ifTrue = BooleanToCaseConvertor.ConvertBooleanToCase(this.VisitExpression(expression.IfTrue).AsExpression());
      HqlExpression ifFalse = expression.IfFalse != null ? BooleanToCaseConvertor.ConvertBooleanToCase(this.VisitExpression(expression.IfFalse).AsExpression()) : (HqlExpression) null;
      HqlCase hqlCase = this._hqlTreeBuilder.Case(new HqlWhen[1]
      {
        this._hqlTreeBuilder.When(predicate, ifTrue)
      }, ifFalse);
      return expression.Type != typeof (bool) && expression.Type != typeof (bool?) ? (HqlTreeNode) this._hqlTreeBuilder.Cast((HqlExpression) hqlCase, expression.Type) : (HqlTreeNode) this._hqlTreeBuilder.Equality((HqlExpression) hqlCase, (HqlExpression) this._hqlTreeBuilder.True());
    }

    protected HqlTreeNode VisitSubQueryExpression(SubQueryExpression expression)
    {
      return QueryModelVisitor.GenerateHqlQuery(expression.QueryModel, this._parameters, false).Statement;
    }

    protected HqlTreeNode VisitNewArrayExpression(NewArrayExpression expression)
    {
      HqlGeneratorExpressionTreeVisitor visitor = new HqlGeneratorExpressionTreeVisitor(this._parameters);
      return (HqlTreeNode) this._hqlTreeBuilder.ExpressionSubTreeHolder(expression.Expressions.Select<Expression, HqlTreeNode>((Func<Expression, HqlTreeNode>) (exp => visitor.Visit(exp))));
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.ExpressionTransformers.SimplifyCompareTransformer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Linq.Functions;
using Remotion.Linq.Parsing.ExpressionTreeVisitors.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq.ExpressionTransformers
{
  internal class SimplifyCompareTransformer : IExpressionTransformer<BinaryExpression>
  {
    private static readonly IDictionary<ExpressionType, ExpressionType> ActingOperators = (IDictionary<ExpressionType, ExpressionType>) new Dictionary<ExpressionType, ExpressionType>()
    {
      {
        ExpressionType.LessThan,
        ExpressionType.GreaterThan
      },
      {
        ExpressionType.LessThanOrEqual,
        ExpressionType.GreaterThanOrEqual
      },
      {
        ExpressionType.GreaterThan,
        ExpressionType.LessThan
      },
      {
        ExpressionType.GreaterThanOrEqual,
        ExpressionType.LessThanOrEqual
      },
      {
        ExpressionType.Equal,
        ExpressionType.Equal
      },
      {
        ExpressionType.NotEqual,
        ExpressionType.NotEqual
      }
    };
    private static readonly IDictionary<Type, MethodInfo> dummies = (IDictionary<Type, MethodInfo>) new Dictionary<Type, MethodInfo>()
    {
      {
        typeof (string),
        ReflectionHelper.GetMethod((Expression<Action>) (() => SimplifyCompareTransformer.DummyComparison<string>(default (string), default (string))))
      },
      {
        typeof (bool),
        ReflectionHelper.GetMethod((Expression<Action>) (() => SimplifyCompareTransformer.DummyComparison<bool>(false, false)))
      },
      {
        typeof (bool?),
        ReflectionHelper.GetMethod((Expression<Action>) (() => SimplifyCompareTransformer.DummyComparison<bool?>(new bool?(), new bool?())))
      },
      {
        typeof (Guid),
        ReflectionHelper.GetMethod((Expression<Action>) (() => SimplifyCompareTransformer.DummyComparison<Guid>(Guid.Empty, Guid.Empty)))
      },
      {
        typeof (Guid?),
        ReflectionHelper.GetMethod((Expression<Action>) (() => SimplifyCompareTransformer.DummyComparison<Guid?>(new Guid?(), new Guid?())))
      }
    };

    public ExpressionType[] SupportedExpressionTypes
    {
      get => SimplifyCompareTransformer.ActingOperators.Keys.ToArray<ExpressionType>();
    }

    public Expression Transform(BinaryExpression expression)
    {
      ExpressionType et;
      if (SimplifyCompareTransformer.ActingOperators.TryGetValue(expression.NodeType, out et))
      {
        if (SimplifyCompareTransformer.IsCompare(expression.Left) && SimplifyCompareTransformer.IsConstantZero(expression.Right))
          return this.Build(expression.NodeType, expression.Left);
        if (SimplifyCompareTransformer.IsConstantZero(expression.Left) && SimplifyCompareTransformer.IsCompare(expression.Right))
          return this.Build(et, expression.Right);
      }
      return (Expression) expression;
    }

    private static bool IsConstantZero(Expression expression)
    {
      return expression is ConstantExpression constantExpression && (constantExpression.Type == typeof (int) && (int) constantExpression.Value == 0 || constantExpression.Type == typeof (long) && (long) constantExpression.Value == 0L);
    }

    private static bool IsCompare(Expression expression)
    {
      return expression is MethodCallExpression methodCallExpression && CompareGenerator.IsCompareMethod(methodCallExpression.Method);
    }

    private Expression Build(ExpressionType et, Expression expression)
    {
      MethodCallExpression methodCallExpression = expression as MethodCallExpression;
      Expression left = methodCallExpression.Arguments.Count == 1 ? methodCallExpression.Object : methodCallExpression.Arguments[0];
      Expression right = methodCallExpression.Arguments.Count == 1 ? methodCallExpression.Arguments[0] : methodCallExpression.Arguments[1];
      MethodInfo method;
      return SimplifyCompareTransformer.dummies.TryGetValue(methodCallExpression.Arguments[0].Type, out method) ? (Expression) Expression.MakeBinary(et, left, right, false, method) : (Expression) Expression.MakeBinary(et, left, right);
    }

    private static bool DummyComparison<T>(T lhs, T rhs)
    {
      throw new NotSupportedException("This method is not intended to be called.");
    }
  }
}

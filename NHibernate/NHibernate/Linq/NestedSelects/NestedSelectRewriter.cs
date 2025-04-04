// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.NestedSelects.NestedSelectRewriter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Linq.GroupBy;
using NHibernate.Linq.Visitors;
using NHibernate.Metadata;
using NHibernate.Util;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace NHibernate.Linq.NestedSelects
{
  internal static class NestedSelectRewriter
  {
    private static readonly MethodInfo CastMethod = EnumerableHelper.GetMethod("Cast", new Type[1]
    {
      typeof (IEnumerable)
    }, new Type[1]{ typeof (object[]) });

    public static void ReWrite(QueryModel queryModel, ISessionFactory sessionFactory)
    {
      NestedSelectDetector nestedSelectDetector = new NestedSelectDetector(sessionFactory);
      nestedSelectDetector.VisitExpression(queryModel.SelectClause.Selector);
      if (!nestedSelectDetector.HasSubqueries)
        return;
      List<ExpressionHolder> expressionHolderList1 = new List<ExpressionHolder>();
      ParameterExpression group = Expression.Parameter(typeof (IGrouping<Tuple, Tuple>), "g");
      Dictionary<Expression, Expression> dictionary = new Dictionary<Expression, Expression>();
      foreach (Expression expression1 in (IEnumerable<Expression>) nestedSelectDetector.Expressions)
      {
        Expression expression2 = NestedSelectRewriter.ProcessExpression(queryModel, sessionFactory, expression1, expressionHolderList1, group);
        if (expression2 != null)
          dictionary.Add(expression1, expression2);
      }
      MemberExpression parameter = Expression.Property((Expression) group, "Key");
      List<ExpressionHolder> expressionHolderList2 = new List<ExpressionHolder>();
      Expression identifier = NestedSelectRewriter.GetIdentifier(sessionFactory, (Expression) new QuerySourceReferenceExpression((IQuerySource) queryModel.MainFromClause));
      Expression body = new SelectClauseRewriter((Expression) parameter, (ICollection<ExpressionHolder>) expressionHolderList2, identifier, dictionary).VisitExpression(queryModel.SelectClause.Selector);
      expressionHolderList1.AddRange((IEnumerable<ExpressionHolder>) expressionHolderList2);
      LambdaExpression selector1 = NestedSelectRewriter.CreateSelector((IEnumerable<ExpressionHolder>) expressionHolderList1, 0);
      LambdaExpression selector2 = NestedSelectRewriter.CreateSelector((IEnumerable<ExpressionHolder>) expressionHolderList1, 1);
      MethodInfo method = EnumerableHelper.GetMethod("GroupBy", new Type[3]
      {
        typeof (IEnumerable<>),
        typeof (Func<,>),
        typeof (Func<,>)
      }, new Type[3]
      {
        typeof (object[]),
        typeof (Tuple),
        typeof (Tuple)
      });
      ParameterExpression parameterExpression = Expression.Parameter(typeof (IEnumerable<object>), "input");
      LambdaExpression selectClause = Expression.Lambda((Expression) Expression.Call(method, (Expression) Expression.Call(NestedSelectRewriter.CastMethod, (Expression) parameterExpression), (Expression) selector1, (Expression) selector2), parameterExpression);
      queryModel.ResultOperators.Add((ResultOperatorBase) new ClientSideSelect2(selectClause));
      queryModel.ResultOperators.Add((ResultOperatorBase) new ClientSideSelect(Expression.Lambda(body, group)));
      IEnumerable<Expression> initializers = expressionHolderList1.Select<ExpressionHolder, Expression>((Func<ExpressionHolder, Expression>) (e => NestedSelectRewriter.ConvertToObject(e.Expression)));
      queryModel.SelectClause.Selector = (Expression) Expression.NewArrayInit(typeof (object), initializers);
    }

    private static Expression ProcessExpression(
      QueryModel queryModel,
      ISessionFactory sessionFactory,
      Expression expression,
      List<ExpressionHolder> elementExpression,
      ParameterExpression group)
    {
      switch (expression)
      {
        case MemberExpression memberExpression:
          return NestedSelectRewriter.ProcessMemberExpression(sessionFactory, (ICollection<ExpressionHolder>) elementExpression, queryModel, (Expression) group, (Expression) memberExpression);
        case SubQueryExpression subQueryExpression:
          return NestedSelectRewriter.ProcessSubquery(sessionFactory, (ICollection<ExpressionHolder>) elementExpression, queryModel, (Expression) group, subQueryExpression.QueryModel);
        default:
          return (Expression) null;
      }
    }

    private static Expression ProcessSubquery(
      ISessionFactory sessionFactory,
      ICollection<ExpressionHolder> elementExpression,
      QueryModel queryModel,
      Expression group,
      QueryModel subQueryModel)
    {
      MainFromClause mainFromClause = subQueryModel.MainFromClause;
      NhJoinClause newClause = new NhJoinClause(mainFromClause.ItemName, mainFromClause.ItemType, mainFromClause.FromExpression);
      queryModel.BodyClauses.Add((IBodyClause) newClause);
      SwapQuerySourceVisitor querySourceVisitor = new SwapQuerySourceVisitor((IQuerySource) mainFromClause, (IQuerySource) newClause);
      queryModel.TransformExpressions(new Func<Expression, Expression>(querySourceVisitor.Swap));
      Expression selector = subQueryModel.SelectClause.Selector;
      Type resultType = subQueryModel.GetResultType();
      Type type = selector.Type;
      QuerySourceReferenceExpression source = new QuerySourceReferenceExpression((IQuerySource) newClause);
      return NestedSelectRewriter.BuildSubCollectionQuery(sessionFactory, elementExpression, group, (Expression) source, selector, type, resultType);
    }

    private static Expression ProcessMemberExpression(
      ISessionFactory sessionFactory,
      ICollection<ExpressionHolder> elementExpression,
      QueryModel queryModel,
      Expression group,
      Expression memberExpression)
    {
      NhJoinClause nhJoinClause = new NhJoinClause(new NameGenerator(queryModel).GetNewName(), NestedSelectRewriter.GetElementType(memberExpression.Type), memberExpression);
      queryModel.BodyClauses.Add((IBodyClause) nhJoinClause);
      QuerySourceReferenceExpression referenceExpression = new QuerySourceReferenceExpression((IQuerySource) nhJoinClause);
      return NestedSelectRewriter.BuildSubCollectionQuery(sessionFactory, elementExpression, group, (Expression) referenceExpression, (Expression) referenceExpression, referenceExpression.Type, memberExpression.Type);
    }

    private static Expression BuildSubCollectionQuery(
      ISessionFactory sessionFactory,
      ICollection<ExpressionHolder> expressions,
      Expression group,
      Expression source,
      Expression select,
      Type elementType,
      Type collectionType)
    {
      LambdaExpression predicate = NestedSelectRewriter.MakePredicate(expressions.Count);
      Expression identifier = NestedSelectRewriter.GetIdentifier(sessionFactory, source);
      LambdaExpression selector = NestedSelectRewriter.MakeSelector(expressions, select, identifier);
      return NestedSelectRewriter.SubCollectionQuery(collectionType, elementType, group, (Expression) predicate, (Expression) selector);
    }

    private static LambdaExpression MakeSelector(
      ICollection<ExpressionHolder> elementExpression,
      Expression select,
      Expression identifier)
    {
      ParameterExpression parameter = Expression.Parameter(typeof (Tuple), "value");
      return Expression.Lambda(new SelectClauseRewriter((Expression) parameter, elementExpression, identifier, 1, new Dictionary<Expression, Expression>()).VisitExpression(select), parameter);
    }

    private static Expression SubCollectionQuery(
      Type collectionType,
      Type elementType,
      Expression source,
      Expression predicate,
      Expression selector)
    {
      MethodInfo method = EnumerableHelper.GetMethod("Where", new Type[2]
      {
        typeof (IEnumerable<>),
        typeof (Func<,>)
      }, new Type[1]{ typeof (Tuple) });
      MethodCallExpression methodCallExpression = Expression.Call(EnumerableHelper.GetMethod("Select", new Type[2]
      {
        typeof (IEnumerable<>),
        typeof (Func<,>)
      }, new Type[2]{ typeof (Tuple), elementType }), (Expression) Expression.Call(method, source, predicate), selector);
      if (collectionType.IsArray)
        return (Expression) Expression.Call(EnumerableHelper.GetMethod("ToArray", new Type[1]
        {
          typeof (IEnumerable<>)
        }, new Type[1]{ elementType }), (Expression) methodCallExpression);
      MethodCallExpression instance = Expression.Call(EnumerableHelper.GetMethod("ToList", new Type[1]
      {
        typeof (IEnumerable<>)
      }, new Type[1]{ elementType }), (Expression) methodCallExpression);
      ConstructorInfo collectionConstructor = NestedSelectRewriter.GetCollectionConstructor(collectionType, elementType);
      if (collectionConstructor == null)
        return (Expression) Expression.Call((Expression) instance, "AsReadonly", Type.EmptyTypes);
      return (Expression) Expression.New(collectionConstructor, (Expression) instance);
    }

    private static ConstructorInfo GetCollectionConstructor(Type collectionType, Type elementType)
    {
      if (collectionType.IsInterface)
      {
        if (!collectionType.IsGenericType || collectionType.GetGenericTypeDefinition() != typeof (ISet<>))
          return (ConstructorInfo) null;
        return typeof (HashedSet<>).MakeGenericType(elementType).GetConstructor(new Type[1]
        {
          typeof (ICollection<>).MakeGenericType(elementType)
        });
      }
      return collectionType.GetConstructor(new Type[1]
      {
        typeof (IEnumerable<>).MakeGenericType(elementType)
      });
    }

    private static LambdaExpression MakePredicate(int index)
    {
      ParameterExpression parameterExpression = Expression.Parameter(typeof (Tuple), "t");
      return Expression.Lambda((Expression) Expression.Not((Expression) Expression.Call(typeof (object), "ReferenceEquals", Type.EmptyTypes, NestedSelectRewriter.ArrayIndex((Expression) Expression.Property((Expression) parameterExpression, Tuple.ItemsProperty), index), (Expression) Expression.Constant((object) null))), parameterExpression);
    }

    private static Expression GetIdentifier(ISessionFactory sessionFactory, Expression expression)
    {
      IClassMetadata classMetadata = sessionFactory.GetClassMetadata(expression.Type);
      return classMetadata == null ? (Expression) Expression.Constant((object) null) : NestedSelectRewriter.ConvertToObject((Expression) Expression.PropertyOrField(expression, classMetadata.IdentifierPropertyName));
    }

    private static LambdaExpression CreateSelector(
      IEnumerable<ExpressionHolder> expressions,
      int tuple)
    {
      ParameterExpression parameter = Expression.Parameter(typeof (object[]), "x");
      NewArrayExpression newArrayExpression = Expression.NewArrayInit(typeof (object), expressions.Select((x, index) => new
      {
        Tuple = x.Tuple,
        index = index
      }).Where(x => x.Tuple == tuple).Select(x => NestedSelectRewriter.ArrayIndex((Expression) parameter, x.index)));
      return Expression.Lambda((Expression) Expression.New(Tuple.Constructor, (Expression) newArrayExpression), parameter);
    }

    private static Expression ArrayIndex(Expression param, int value)
    {
      return (Expression) Expression.ArrayIndex(param, (Expression) Expression.Constant((object) value));
    }

    private static Expression ConvertToObject(Expression expression)
    {
      return (Expression) Expression.Convert(expression, typeof (object));
    }

    private static Type GetElementType(Type type)
    {
      return ReflectHelper.GetCollectionElementType(type) ?? throw new NotSupportedException("Unknown collection type " + type.FullName);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Parsing.Structure.IntermediateModel.GroupByWithResultSelectorExpressionNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Parsing.ExpressionTreeVisitors;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Parsing.Structure.IntermediateModel
{
  public class GroupByWithResultSelectorExpressionNode(
    MethodCallExpressionParseInfo parseInfo,
    LambdaExpression keySelector,
    LambdaExpression elementSelectorOrResultSelector,
    LambdaExpression resultSelectorOrNull) : 
    SelectExpressionNode(GroupByWithResultSelectorExpressionNode.CreateParseInfoWithGroupNode(parseInfo, ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (keySelector), keySelector), ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (elementSelectorOrResultSelector), elementSelectorOrResultSelector), resultSelectorOrNull), GroupByWithResultSelectorExpressionNode.CreateSelectorForSelectNode(keySelector, elementSelectorOrResultSelector, resultSelectorOrNull)),
    IQuerySourceExpressionNode,
    IExpressionNode
  {
    public new static readonly MethodInfo[] SupportedMethods = new MethodInfo[4]
    {
      MethodCallExpressionNodeBase.GetSupportedMethod<IQueryable<object>>((Expression<Func<IQueryable<object>>>) (() => default (IQueryable<object>).GroupBy<object, object, object, object>((Expression<Func<object, object>>) (o => (object) null), (Expression<Func<object, object>>) (o => (object) null), (Expression<Func<object, IEnumerable<object>, object>>) ((k, g) => (object) null)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<IQueryable<object>>((Expression<Func<IQueryable<object>>>) (() => default (IQueryable<object>).GroupBy<object, object, object>((Expression<Func<object, object>>) (o => (object) null), (Expression<Func<object, IEnumerable<object>, object>>) ((k, g) => (object) null)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<IEnumerable<object>>((Expression<Func<IEnumerable<object>>>) (() => default (IEnumerable<object>).GroupBy<object, object, object, object>((Func<object, object>) (o => (object) null), (Func<object, object>) (o => (object) null), (Func<object, IEnumerable<object>, object>) ((k, g) => (object) null)))),
      MethodCallExpressionNodeBase.GetSupportedMethod<IEnumerable<object>>((Expression<Func<IEnumerable<object>>>) (() => default (IEnumerable<object>).GroupBy<object, object, object>((Func<object, object>) (o => (object) null), (Func<object, IEnumerable<object>, object>) ((k, g) => (object) null))))
    };

    private static MethodCallExpressionParseInfo CreateParseInfoWithGroupNode(
      MethodCallExpressionParseInfo parseInfo,
      LambdaExpression keySelector,
      LambdaExpression elementSelectorOrResultSelector,
      LambdaExpression resultSelectorOrNull)
    {
      LambdaExpression optionalElementSelector = GroupByWithResultSelectorExpressionNode.GetOptionalElementSelector(elementSelectorOrResultSelector, resultSelectorOrNull);
      Type typeOfIenumerable = ReflectionUtility.GetItemTypeOfIEnumerable(parseInfo.ParsedExpression.Arguments[0].Type, "parseInfo.ParsedExpression.Arguments[0].Type");
      MethodCallExpression parsedExpression;
      if (optionalElementSelector == null)
        parsedExpression = Expression.Call(typeof (Enumerable), "GroupBy", new Type[2]
        {
          typeOfIenumerable,
          keySelector.Body.Type
        }, parseInfo.ParsedExpression.Arguments[0], (Expression) keySelector);
      else
        parsedExpression = Expression.Call(typeof (Enumerable), "GroupBy", new Type[3]
        {
          typeOfIenumerable,
          keySelector.Body.Type,
          optionalElementSelector.Body.Type
        }, parseInfo.ParsedExpression.Arguments[0], (Expression) keySelector, (Expression) optionalElementSelector);
      GroupByExpressionNode source = new GroupByExpressionNode(new MethodCallExpressionParseInfo(parseInfo.AssociatedIdentifier, parseInfo.Source, parsedExpression), keySelector, optionalElementSelector);
      return new MethodCallExpressionParseInfo(parseInfo.AssociatedIdentifier, (IExpressionNode) source, parseInfo.ParsedExpression);
    }

    private static LambdaExpression CreateSelectorForSelectNode(
      LambdaExpression keySelector,
      LambdaExpression elementSelectorOrResultSelector,
      LambdaExpression resultSelectorOrNull)
    {
      LambdaExpression resultSelector = GroupByWithResultSelectorExpressionNode.GetResultSelector(elementSelectorOrResultSelector, resultSelectorOrNull);
      LambdaExpression optionalElementSelector = GroupByWithResultSelectorExpressionNode.GetOptionalElementSelector(elementSelectorOrResultSelector, resultSelectorOrNull);
      Type type1 = optionalElementSelector != null ? optionalElementSelector.Body.Type : keySelector.Parameters[0].Type;
      Type type2 = typeof (IGrouping<,>).MakeGenericType(keySelector.Body.Type, type1);
      PropertyInfo property = type2.GetProperty("Key");
      ParameterExpression parameterExpression = Expression.Parameter(type2, "group");
      MemberExpression memberExpression = Expression.MakeMemberAccess((Expression) parameterExpression, (MemberInfo) property);
      return Expression.Lambda(MultiReplacingExpressionTreeVisitor.Replace((IDictionary<Expression, Expression>) new Dictionary<Expression, Expression>(2)
      {
        {
          (Expression) resultSelector.Parameters[1],
          (Expression) parameterExpression
        },
        {
          (Expression) resultSelector.Parameters[0],
          (Expression) memberExpression
        }
      }, resultSelector.Body), parameterExpression);
    }

    private static LambdaExpression GetOptionalElementSelector(
      LambdaExpression elementSelectorOrResultSelector,
      LambdaExpression resultSelectorOrNull)
    {
      return resultSelectorOrNull != null ? elementSelectorOrResultSelector : (LambdaExpression) null;
    }

    private static LambdaExpression GetResultSelector(
      LambdaExpression elementSelectorOrResultSelector,
      LambdaExpression resultSelectorOrNull)
    {
      if (resultSelectorOrNull != null)
      {
        if (resultSelectorOrNull.Parameters.Count != 2)
          throw new ArgumentException("ResultSelector must have exactly two parameters.", nameof (resultSelectorOrNull));
        return resultSelectorOrNull;
      }
      if (elementSelectorOrResultSelector.Parameters.Count != 2)
        throw new ArgumentException("ResultSelector must have exactly two parameters.", nameof (elementSelectorOrResultSelector));
      return elementSelectorOrResultSelector;
    }
  }
}

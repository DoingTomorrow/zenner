// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperatorBase
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Clauses
{
  public abstract class ResultOperatorBase
  {
    public abstract IStreamedData ExecuteInMemory(IStreamedData input);

    public abstract IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo);

    public abstract ResultOperatorBase Clone(CloneContext cloneContext);

    public virtual void Accept(IQueryModelVisitor visitor, QueryModel queryModel, int index)
    {
      ArgumentUtility.CheckNotNull<IQueryModelVisitor>(nameof (visitor), visitor);
      ArgumentUtility.CheckNotNull<QueryModel>(nameof (queryModel), queryModel);
      visitor.VisitResultOperator(this, queryModel, index);
    }

    public abstract void TransformExpressions(Func<Expression, Expression> transformation);

    protected TResult InvokeGenericExecuteMethod<TInput, TResult>(
      IStreamedData input,
      Func<TInput, TResult> genericExecuteCaller)
      where TInput : IStreamedData
      where TResult : IStreamedData
    {
      ArgumentUtility.CheckNotNull<IStreamedData>(nameof (input), input);
      ArgumentUtility.CheckNotNull<Func<TInput, TResult>>(nameof (genericExecuteCaller), genericExecuteCaller);
      MethodInfo method = genericExecuteCaller.Method;
      if (!method.IsGenericMethod || method.GetGenericArguments().Length != 1)
        throw new ArgumentException("Method to invoke ('" + method.Name + "') must be a generic method with exactly one generic argument.", nameof (genericExecuteCaller));
      return (TResult) this.InvokeExecuteMethod(input.DataInfo.MakeClosedGenericExecuteMethod(method.GetGenericMethodDefinition()), (object) input);
    }

    protected object InvokeExecuteMethod(MethodInfo method, object input)
    {
      if (!method.IsPublic)
        throw new ArgumentException("Method to invoke ('" + method.Name + "') must be a public method.", nameof (method));
      ResultOperatorBase resultOperatorBase = method.IsStatic ? (ResultOperatorBase) null : this;
      try
      {
        return method.Invoke((object) resultOperatorBase, new object[1]
        {
          input
        });
      }
      catch (TargetInvocationException ex)
      {
        throw ex.InnerException;
      }
      catch (ArgumentException ex)
      {
        throw new ArgumentException(string.Format("Cannot call method '{0}' on input of type '{1}': {2}", (object) method.Name, (object) input.GetType(), (object) ex.Message), nameof (method));
      }
    }

    protected T GetConstantValueFromExpression<T>(string expressionName, Expression expression)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (expression), expression);
      if (!typeof (T).IsAssignableFrom(expression.Type))
        throw new InvalidOperationException(string.Format("The value stored by the {0} expression ('{1}') is not of type '{2}', it is of type '{3}'.", (object) expressionName, (object) FormattingExpressionTreeVisitor.Format(expression), (object) typeof (T), (object) expression.Type));
      return expression is ConstantExpression constantExpression ? (T) constantExpression.Value : throw new InvalidOperationException(string.Format("The {0} expression ('{1}') is no ConstantExpression, it is a {2}.", (object) expressionName, (object) FormattingExpressionTreeVisitor.Format(expression), (object) expression.GetType().Name));
    }

    protected void CheckSequenceItemType(StreamedSequenceInfo sequenceInfo, Type expectedItemType)
    {
      if (!expectedItemType.IsAssignableFrom(sequenceInfo.ResultItemType))
        throw new ArgumentTypeException(string.Format("The input sequence must have items of type '{0}', but it has items of type '{1}'.", (object) expectedItemType, (object) sequenceInfo.ResultItemType), "inputInfo", typeof (IEnumerable<>).MakeGenericType(expectedItemType), sequenceInfo.ResultItemType);
    }
  }
}

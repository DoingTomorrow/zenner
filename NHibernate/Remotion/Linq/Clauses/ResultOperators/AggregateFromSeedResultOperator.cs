// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.AggregateFromSeedResultOperator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Utilities;
using System;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses.ResultOperators
{
  public class AggregateFromSeedResultOperator : ValueFromSequenceResultOperatorBase
  {
    private Expression _seed;
    private LambdaExpression _func;
    private LambdaExpression _resultSelector;

    public AggregateFromSeedResultOperator(
      Expression seed,
      LambdaExpression func,
      LambdaExpression optionalResultSelector)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (seed), seed);
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (func), func);
      this.Seed = seed;
      this.Func = func;
      this.OptionalResultSelector = optionalResultSelector;
    }

    public LambdaExpression Func
    {
      get => this._func;
      set
      {
        ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (value), value);
        this._func = this.DescribesValidFuncType(value) ? value : throw new ArgumentTypeException(string.Format("The aggregating function must be a LambdaExpression that describes an instantiation of 'Func<TAccumulate,TAccumulate>', but it is '{0}'.", (object) value.Type), nameof (value), typeof (System.Func<,,>), value.Type);
      }
    }

    public Expression Seed
    {
      get => this._seed;
      set => this._seed = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    public LambdaExpression OptionalResultSelector
    {
      get => this._resultSelector;
      set
      {
        this._resultSelector = this.DescribesValidResultSelectorType(value) ? value : throw new ArgumentTypeException(string.Format("The result selector must be a LambdaExpression that describes an instantiation of 'Func<TAccumulate,TResult>', but it is '{0}'.", (object) value.Type), nameof (value), typeof (System.Func<,,>), value.Type);
      }
    }

    public T GetConstantSeed<T>() => this.GetConstantValueFromExpression<T>("seed", this.Seed);

    public override StreamedValue ExecuteInMemory<TInput>(StreamedSequence input)
    {
      ArgumentUtility.CheckNotNull<StreamedSequence>(nameof (input), input);
      return (StreamedValue) this.InvokeExecuteMethod(typeof (AggregateFromSeedResultOperator).GetMethod("ExecuteAggregateInMemory").MakeGenericMethod(typeof (TInput), this.Seed.Type, this.GetResultType()), (object) input);
    }

    public StreamedValue ExecuteAggregateInMemory<TInput, TAggregate, TResult>(
      StreamedSequence input)
    {
      ArgumentUtility.CheckNotNull<StreamedSequence>(nameof (input), input);
      TAggregate aggregate = input.GetTypedSequence<TInput>().Aggregate<TInput, TAggregate>(this.GetConstantSeed<TAggregate>(), (System.Func<TAggregate, TInput, TAggregate>) ReverseResolvingExpressionTreeVisitor.ReverseResolveLambda(input.DataInfo.ItemExpression, this.Func, 1).Compile());
      StreamedValueInfo outputDataInfo = (StreamedValueInfo) this.GetOutputDataInfo((IStreamedDataInfo) input.DataInfo);
      return this.OptionalResultSelector == null ? new StreamedValue((object) aggregate, outputDataInfo) : new StreamedValue((object) ((System.Func<TAggregate, TResult>) this.OptionalResultSelector.Compile())(aggregate), outputDataInfo);
    }

    public override ResultOperatorBase Clone(CloneContext cloneContext)
    {
      return (ResultOperatorBase) new AggregateFromSeedResultOperator(this.Seed, this.Func, this.OptionalResultSelector);
    }

    public override IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo)
    {
      ArgumentUtility.CheckNotNullAndType<StreamedSequenceInfo>(nameof (inputInfo), (object) inputInfo);
      Type genericArgument1 = this.Func.Type.GetGenericArguments()[0];
      if (!genericArgument1.IsAssignableFrom(this.Seed.Type))
        throw new InvalidOperationException(string.Format("The seed expression and the aggregating function don't have matching types. The seed is of type '{0}', but the function aggregates '{1}'.", (object) this.Seed.Type, (object) genericArgument1));
      Type genericArgument2 = this.OptionalResultSelector != null ? this.OptionalResultSelector.Type.GetGenericArguments()[0] : (Type) null;
      if (genericArgument2 != null && genericArgument1 != genericArgument2)
        throw new InvalidOperationException(string.Format("The aggregating function and the result selector don't have matching types. The function aggregates type '{0}', but the result selector takes '{1}'.", (object) genericArgument1, (object) genericArgument2));
      return (IStreamedDataInfo) new StreamedScalarValueInfo(this.GetResultType());
    }

    public override void TransformExpressions(System.Func<Expression, Expression> transformation)
    {
      this.Seed = transformation(this.Seed);
      this.Func = (LambdaExpression) transformation((Expression) this.Func);
      if (this.OptionalResultSelector == null)
        return;
      this.OptionalResultSelector = (LambdaExpression) transformation((Expression) this.OptionalResultSelector);
    }

    public override string ToString()
    {
      return this.OptionalResultSelector != null ? string.Format("Aggregate({0}, {1}, {2})", (object) FormattingExpressionTreeVisitor.Format(this.Seed), (object) FormattingExpressionTreeVisitor.Format((Expression) this.Func), (object) FormattingExpressionTreeVisitor.Format((Expression) this.OptionalResultSelector)) : string.Format("Aggregate({0}, {1})", (object) FormattingExpressionTreeVisitor.Format(this.Seed), (object) FormattingExpressionTreeVisitor.Format((Expression) this.Func));
    }

    private Type GetResultType()
    {
      return this.OptionalResultSelector == null ? this.Func.Body.Type : this.OptionalResultSelector.Body.Type;
    }

    private bool DescribesValidFuncType(LambdaExpression value)
    {
      Type type = value.Type;
      if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof (System.Func<,>))
        return false;
      Type[] genericArguments = type.GetGenericArguments();
      return genericArguments[0] == genericArguments[1];
    }

    private bool DescribesValidResultSelectorType(LambdaExpression value)
    {
      if (value == null)
        return true;
      return value.Type.IsGenericType && value.Type.GetGenericTypeDefinition() == typeof (System.Func<,>);
    }
  }
}

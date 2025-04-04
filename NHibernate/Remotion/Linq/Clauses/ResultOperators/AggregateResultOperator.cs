// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.AggregateResultOperator
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
  public class AggregateResultOperator : ValueFromSequenceResultOperatorBase
  {
    private LambdaExpression _func;

    public AggregateResultOperator(LambdaExpression func)
    {
      ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (func), func);
      this.Func = func;
    }

    public LambdaExpression Func
    {
      get => this._func;
      set
      {
        ArgumentUtility.CheckNotNull<LambdaExpression>(nameof (value), value);
        this._func = this.DescribesValidFuncType(value) ? value : throw new ArgumentTypeException(string.Format("The aggregating function must be a LambdaExpression that describes an instantiation of 'Func<T,T>', but it is '{0}'.", (object) value.Type), nameof (value), typeof (System.Func<,>), value.Type);
      }
    }

    public override StreamedValue ExecuteInMemory<T>(StreamedSequence input)
    {
      ArgumentUtility.CheckNotNull<StreamedSequence>(nameof (input), input);
      return new StreamedValue((object) input.GetTypedSequence<T>().Aggregate<T>((System.Func<T, T, T>) ReverseResolvingExpressionTreeVisitor.ReverseResolveLambda(input.DataInfo.ItemExpression, this.Func, 1).Compile()), (StreamedValueInfo) this.GetOutputDataInfo((IStreamedDataInfo) input.DataInfo));
    }

    public override ResultOperatorBase Clone(CloneContext cloneContext)
    {
      return (ResultOperatorBase) new AggregateResultOperator(this.Func);
    }

    public override IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo)
    {
      this.CheckSequenceItemType(ArgumentUtility.CheckNotNullAndType<StreamedSequenceInfo>(nameof (inputInfo), (object) inputInfo), this.GetExpectedItemType());
      return (IStreamedDataInfo) new StreamedScalarValueInfo(this.Func.Body.Type);
    }

    public override void TransformExpressions(System.Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<System.Func<Expression, Expression>>(nameof (transformation), transformation);
      this.Func = (LambdaExpression) transformation((Expression) this.Func);
    }

    public override string ToString()
    {
      return "Aggregate(" + FormattingExpressionTreeVisitor.Format((Expression) this.Func) + ")";
    }

    private bool DescribesValidFuncType(LambdaExpression value)
    {
      Type type = value.Type;
      if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof (System.Func<,>))
        return false;
      Type[] genericArguments = type.GetGenericArguments();
      return genericArguments[0].IsAssignableFrom(genericArguments[1]);
    }

    private Type GetExpectedItemType() => this.Func.Type.GetGenericArguments()[0];
  }
}

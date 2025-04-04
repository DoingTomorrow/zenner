// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.CastResultOperator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses.ResultOperators
{
  public class CastResultOperator : SequenceFromSequenceResultOperatorBase
  {
    private Type _castItemType;

    public CastResultOperator(Type castItemType)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (castItemType), castItemType);
      this.CastItemType = castItemType;
    }

    public Type CastItemType
    {
      get => this._castItemType;
      set
      {
        ArgumentUtility.CheckNotNull<Type>(nameof (value), value);
        this._castItemType = value;
      }
    }

    public override ResultOperatorBase Clone(CloneContext cloneContext)
    {
      return (ResultOperatorBase) new CastResultOperator(this.CastItemType);
    }

    public override StreamedSequence ExecuteInMemory<TInput>(StreamedSequence input)
    {
      ArgumentUtility.CheckNotNull<StreamedSequence>(nameof (input), input);
      IEnumerable<TInput> typedSequence = input.GetTypedSequence<TInput>();
      return new StreamedSequence((IEnumerable) ((IEnumerable) this.InvokeExecuteMethod(typeof (Enumerable).GetMethod("Cast", new Type[1]
      {
        typeof (IEnumerable)
      }).MakeGenericMethod(this.CastItemType), (object) typedSequence)).AsQueryable(), (StreamedSequenceInfo) this.GetOutputDataInfo((IStreamedDataInfo) input.DataInfo));
    }

    public override IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo)
    {
      StreamedSequenceInfo streamedSequenceInfo = ArgumentUtility.CheckNotNullAndType<StreamedSequenceInfo>(nameof (inputInfo), (object) inputInfo);
      return (IStreamedDataInfo) new StreamedSequenceInfo(typeof (IQueryable<>).MakeGenericType(this.CastItemType), (Expression) this.GetNewItemExpression(streamedSequenceInfo.ItemExpression));
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
    }

    public override string ToString() => "Cast<" + (object) this.CastItemType + ">()";

    private UnaryExpression GetNewItemExpression(Expression inputItemExpression)
    {
      return Expression.Convert(inputItemExpression, this.CastItemType);
    }
  }
}

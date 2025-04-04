// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.OfTypeResultOperator
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
  public class OfTypeResultOperator : SequenceFromSequenceResultOperatorBase
  {
    private Type _searchedItemType;

    public OfTypeResultOperator(Type searchedItemType)
    {
      ArgumentUtility.CheckNotNull<Type>(nameof (searchedItemType), searchedItemType);
      this.SearchedItemType = searchedItemType;
    }

    public Type SearchedItemType
    {
      get => this._searchedItemType;
      set
      {
        ArgumentUtility.CheckNotNull<Type>(nameof (value), value);
        this._searchedItemType = value;
      }
    }

    public override ResultOperatorBase Clone(CloneContext cloneContext)
    {
      return (ResultOperatorBase) new OfTypeResultOperator(this.SearchedItemType);
    }

    public override StreamedSequence ExecuteInMemory<TInput>(StreamedSequence input)
    {
      IEnumerable<TInput> typedSequence = input.GetTypedSequence<TInput>();
      return new StreamedSequence((IEnumerable) ((IEnumerable) this.InvokeExecuteMethod(typeof (Enumerable).GetMethod("OfType", new Type[1]
      {
        typeof (IEnumerable)
      }).MakeGenericMethod(this.SearchedItemType), (object) typedSequence)).AsQueryable(), (StreamedSequenceInfo) this.GetOutputDataInfo((IStreamedDataInfo) input.DataInfo));
    }

    public override IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo)
    {
      StreamedSequenceInfo streamedSequenceInfo = ArgumentUtility.CheckNotNullAndType<StreamedSequenceInfo>(nameof (inputInfo), (object) inputInfo);
      return (IStreamedDataInfo) new StreamedSequenceInfo(typeof (IQueryable<>).MakeGenericType(this.SearchedItemType), (Expression) this.GetNewItemExpression(streamedSequenceInfo.ItemExpression));
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
    }

    public override string ToString() => "OfType<" + (object) this.SearchedItemType + ">()";

    private UnaryExpression GetNewItemExpression(Expression inputItemExpression)
    {
      return Expression.Convert(inputItemExpression, this.SearchedItemType);
    }
  }
}

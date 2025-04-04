// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.ContainsResultOperator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses.ResultOperators
{
  public class ContainsResultOperator : ValueFromSequenceResultOperatorBase
  {
    private Expression _item;

    public ContainsResultOperator(Expression item)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (item), item);
      this.Item = item;
    }

    public Expression Item
    {
      get => this._item;
      set => this._item = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    public T GetConstantItem<T>() => this.GetConstantValueFromExpression<T>("item", this.Item);

    public override StreamedValue ExecuteInMemory<T>(StreamedSequence input)
    {
      ArgumentUtility.CheckNotNull<StreamedSequence>(nameof (input), input);
      return new StreamedValue((object) input.GetTypedSequence<T>().Contains<T>(this.GetConstantItem<T>()), (StreamedValueInfo) this.GetOutputDataInfo((IStreamedDataInfo) input.DataInfo));
    }

    public override ResultOperatorBase Clone(CloneContext cloneContext)
    {
      return (ResultOperatorBase) new ContainsResultOperator(this.Item);
    }

    public override IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo)
    {
      StreamedSequenceInfo streamedSequenceInfo = ArgumentUtility.CheckNotNullAndType<StreamedSequenceInfo>(nameof (inputInfo), (object) inputInfo);
      if (!streamedSequenceInfo.ResultItemType.IsAssignableFrom(this.Item.Type))
        throw new ArgumentTypeException(string.Format("The items of the input sequence of type '{0}' are not compatible with the item expression of type '{1}'.", (object) streamedSequenceInfo.ResultItemType, (object) this.Item.Type), nameof (inputInfo), typeof (IEnumerable<>).MakeGenericType(this.Item.Type), streamedSequenceInfo.ResultItemType);
      return (IStreamedDataInfo) new StreamedScalarValueInfo(typeof (bool));
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.Item = transformation(this.Item);
    }

    public override string ToString()
    {
      return "Contains(" + FormattingExpressionTreeVisitor.Format(this.Item) + ")";
    }
  }
}

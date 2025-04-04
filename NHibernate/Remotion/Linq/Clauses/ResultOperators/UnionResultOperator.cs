// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.UnionResultOperator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ExpressionTreeVisitors;
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
  public class UnionResultOperator : SequenceFromSequenceResultOperatorBase, IQuerySource
  {
    private string _itemName;
    private Type _itemType;
    private Expression _source2;

    public UnionResultOperator(string itemName, Type itemType, Expression source2)
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (itemName), itemName);
      ArgumentUtility.CheckNotNull<Type>(nameof (itemType), itemType);
      ArgumentUtility.CheckNotNull<Expression>(nameof (source2), source2);
      this.ItemName = itemName;
      this.ItemType = itemType;
      this.Source2 = source2;
    }

    public string ItemName
    {
      get => this._itemName;
      set => this._itemName = ArgumentUtility.CheckNotNullOrEmpty(nameof (value), value);
    }

    public Type ItemType
    {
      get => this._itemType;
      set => this._itemType = ArgumentUtility.CheckNotNull<Type>(nameof (value), value);
    }

    public Expression Source2
    {
      get => this._source2;
      set => this._source2 = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    public IEnumerable GetConstantSource2()
    {
      return this.GetConstantValueFromExpression<IEnumerable>("source2", this.Source2);
    }

    public override ResultOperatorBase Clone(CloneContext cloneContext)
    {
      return (ResultOperatorBase) new UnionResultOperator(this._itemName, this._itemType, this._source2);
    }

    public override StreamedSequence ExecuteInMemory<T>(StreamedSequence input)
    {
      return new StreamedSequence((IEnumerable) input.GetTypedSequence<T>().Union<T>((IEnumerable<T>) this.GetConstantSource2()).AsQueryable<T>(), (StreamedSequenceInfo) this.GetOutputDataInfo((IStreamedDataInfo) input.DataInfo));
    }

    public override IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo)
    {
      this.CheckSequenceItemType(ArgumentUtility.CheckNotNullAndType<StreamedSequenceInfo>(nameof (inputInfo), (object) inputInfo), this._itemType);
      return (IStreamedDataInfo) new StreamedSequenceInfo(typeof (IQueryable<>).MakeGenericType(this._itemType), (Expression) new QuerySourceReferenceExpression((IQuerySource) this));
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.Source2 = transformation(this.Source2);
    }

    public override string ToString()
    {
      return "Union(" + FormattingExpressionTreeVisitor.Format(this.Source2) + ")";
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.GroupResultOperator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Utilities;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses.ResultOperators
{
  public class GroupResultOperator : SequenceFromSequenceResultOperatorBase, IQuerySource
  {
    private string _itemName;
    private Expression _keySelector;
    private Expression _elementSelector;

    public GroupResultOperator(string itemName, Expression keySelector, Expression elementSelector)
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (itemName), itemName);
      ArgumentUtility.CheckNotNull<Expression>(nameof (keySelector), keySelector);
      ArgumentUtility.CheckNotNull<Expression>(nameof (elementSelector), elementSelector);
      this._itemName = itemName;
      this._elementSelector = elementSelector;
      this._keySelector = keySelector;
    }

    public string ItemName
    {
      get => this._itemName;
      set => this._itemName = ArgumentUtility.CheckNotNullOrEmpty(nameof (value), value);
    }

    public Type ItemType
    {
      get
      {
        return typeof (IGrouping<,>).MakeGenericType(this.KeySelector.Type, this.ElementSelector.Type);
      }
    }

    public Expression KeySelector
    {
      get => this._keySelector;
      set => this._keySelector = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    public Expression ElementSelector
    {
      get => this._elementSelector;
      set
      {
        this._elementSelector = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
      }
    }

    public override ResultOperatorBase Clone(CloneContext cloneContext)
    {
      ArgumentUtility.CheckNotNull<CloneContext>(nameof (cloneContext), cloneContext);
      return (ResultOperatorBase) new GroupResultOperator(this.ItemName, this.KeySelector, this.ElementSelector);
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.KeySelector = transformation(this.KeySelector);
      this.ElementSelector = transformation(this.ElementSelector);
    }

    public override StreamedSequence ExecuteInMemory<TInput>(StreamedSequence input)
    {
      ArgumentUtility.CheckNotNull<StreamedSequence>(nameof (input), input);
      return (StreamedSequence) this.InvokeExecuteMethod(typeof (GroupResultOperator).GetMethod("ExecuteGroupingInMemory").MakeGenericMethod(typeof (TInput), this.KeySelector.Type, this.ElementSelector.Type), (object) input);
    }

    public StreamedSequence ExecuteGroupingInMemory<TSource, TKey, TElement>(StreamedSequence input)
    {
      ArgumentUtility.CheckNotNull<StreamedSequence>(nameof (input), input);
      return new StreamedSequence((IEnumerable) input.GetTypedSequence<TSource>().GroupBy<TSource, TKey, TElement>((Func<TSource, TKey>) ReverseResolvingExpressionTreeVisitor.ReverseResolve(input.DataInfo.ItemExpression, this.KeySelector).Compile(), (Func<TSource, TElement>) ReverseResolvingExpressionTreeVisitor.ReverseResolve(input.DataInfo.ItemExpression, this.ElementSelector).Compile()).AsQueryable<IGrouping<TKey, TElement>>(), (StreamedSequenceInfo) this.GetOutputDataInfo((IStreamedDataInfo) input.DataInfo));
    }

    public override IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo)
    {
      ArgumentUtility.CheckNotNullAndType<StreamedSequenceInfo>(nameof (inputInfo), (object) inputInfo);
      return (IStreamedDataInfo) new StreamedSequenceInfo(typeof (IQueryable<>).MakeGenericType(this.ItemType), (Expression) new QuerySourceReferenceExpression((IQuerySource) this));
    }

    public override string ToString()
    {
      return string.Format("GroupBy({0}, {1})", (object) FormattingExpressionTreeVisitor.Format(this.KeySelector), (object) FormattingExpressionTreeVisitor.Format(this.ElementSelector));
    }
  }
}

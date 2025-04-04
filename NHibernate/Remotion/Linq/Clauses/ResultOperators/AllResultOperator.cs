// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.AllResultOperator
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
  public class AllResultOperator : ValueFromSequenceResultOperatorBase
  {
    private Expression _predicate;

    public AllResultOperator(Expression predicate)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (predicate), predicate);
      this.Predicate = predicate;
    }

    public Expression Predicate
    {
      get => this._predicate;
      set => this._predicate = ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
    }

    public override StreamedValue ExecuteInMemory<T>(StreamedSequence input)
    {
      ArgumentUtility.CheckNotNull<StreamedSequence>(nameof (input), input);
      return new StreamedValue((object) input.GetTypedSequence<T>().All<T>((Func<T, bool>) ReverseResolvingExpressionTreeVisitor.ReverseResolve(input.DataInfo.ItemExpression, this.Predicate).Compile()), (StreamedValueInfo) this.GetOutputDataInfo((IStreamedDataInfo) input.DataInfo));
    }

    public override ResultOperatorBase Clone(CloneContext cloneContext)
    {
      return (ResultOperatorBase) new AllResultOperator(this.Predicate);
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.Predicate = transformation(this.Predicate);
    }

    public override IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo)
    {
      ArgumentUtility.CheckNotNullAndType<StreamedSequenceInfo>(nameof (inputInfo), (object) inputInfo);
      return (IStreamedDataInfo) new StreamedScalarValueInfo(typeof (bool));
    }

    public override string ToString()
    {
      return "All(" + FormattingExpressionTreeVisitor.Format(this.Predicate) + ")";
    }
  }
}

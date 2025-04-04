// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.IntersectResultOperator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

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
  public class IntersectResultOperator : SequenceTypePreservingResultOperatorBase
  {
    private Expression _source2;

    public IntersectResultOperator(Expression source2)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (source2), source2);
      this.Source2 = source2;
    }

    public Expression Source2
    {
      get => this._source2;
      set
      {
        ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
        ReflectionUtility.GetItemTypeOfIEnumerable(value.Type, nameof (value));
        this._source2 = value;
      }
    }

    public IEnumerable GetConstantSource2()
    {
      return this.GetConstantValueFromExpression<IEnumerable>("source2", this.Source2);
    }

    public override ResultOperatorBase Clone(CloneContext cloneContext)
    {
      return (ResultOperatorBase) new IntersectResultOperator(this.Source2);
    }

    public override StreamedSequence ExecuteInMemory<T>(StreamedSequence input)
    {
      return new StreamedSequence((IEnumerable) input.GetTypedSequence<T>().Intersect<T>((IEnumerable<T>) this.GetConstantSource2()).AsQueryable<T>(), (StreamedSequenceInfo) this.GetOutputDataInfo((IStreamedDataInfo) input.DataInfo));
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.Source2 = transformation(this.Source2);
    }

    public override string ToString()
    {
      return "Intersect(" + FormattingExpressionTreeVisitor.Format(this.Source2) + ")";
    }
  }
}

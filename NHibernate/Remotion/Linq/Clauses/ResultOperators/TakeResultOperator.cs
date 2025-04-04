// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.TakeResultOperator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

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
  public class TakeResultOperator : SequenceTypePreservingResultOperatorBase
  {
    private Expression _count;

    public TakeResultOperator(Expression count)
    {
      ArgumentUtility.CheckNotNull<Expression>(nameof (count), count);
      this.Count = count;
    }

    public Expression Count
    {
      get => this._count;
      set
      {
        ArgumentUtility.CheckNotNull<Expression>(nameof (value), value);
        this._count = value.Type == typeof (int) ? value : throw new ArgumentException(string.Format("The value expression returns '{0}', an expression returning 'System.Int32' was expected.", (object) value.Type), nameof (value));
      }
    }

    public int GetConstantCount() => this.GetConstantValueFromExpression<int>("count", this.Count);

    public override ResultOperatorBase Clone(CloneContext cloneContext)
    {
      return (ResultOperatorBase) new TakeResultOperator(this.Count);
    }

    public override StreamedSequence ExecuteInMemory<T>(StreamedSequence input)
    {
      return new StreamedSequence((IEnumerable) input.GetTypedSequence<T>().Take<T>(this.GetConstantCount()).AsQueryable<T>(), (StreamedSequenceInfo) this.GetOutputDataInfo((IStreamedDataInfo) input.DataInfo));
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      this.Count = transformation(this.Count);
    }

    public override string ToString()
    {
      return "Take(" + FormattingExpressionTreeVisitor.Format(this.Count) + ")";
    }
  }
}

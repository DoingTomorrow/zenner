// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.ReverseResultOperator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.StreamedData;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses.ResultOperators
{
  public class ReverseResultOperator : SequenceTypePreservingResultOperatorBase
  {
    public override ResultOperatorBase Clone(CloneContext cloneContext)
    {
      return (ResultOperatorBase) new ReverseResultOperator();
    }

    public override StreamedSequence ExecuteInMemory<T>(StreamedSequence input)
    {
      return new StreamedSequence((IEnumerable) input.GetTypedSequence<T>().Reverse<T>().AsQueryable<T>(), (StreamedSequenceInfo) this.GetOutputDataInfo((IStreamedDataInfo) input.DataInfo));
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
    }

    public override string ToString() => "Reverse()";
  }
}

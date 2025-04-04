// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.MaxResultOperator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.StreamedData;
using System;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses.ResultOperators
{
  public class MaxResultOperator : ChoiceResultOperatorBase
  {
    public MaxResultOperator()
      : base(false)
    {
    }

    public override ResultOperatorBase Clone(CloneContext cloneContext)
    {
      return (ResultOperatorBase) new MaxResultOperator();
    }

    public override StreamedValue ExecuteInMemory<T>(StreamedSequence input)
    {
      return new StreamedValue((object) input.GetTypedSequence<T>().Max<T>(), (StreamedValueInfo) this.GetOutputDataInfo((IStreamedDataInfo) input.DataInfo));
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
    }

    public override string ToString() => "Max()";
  }
}

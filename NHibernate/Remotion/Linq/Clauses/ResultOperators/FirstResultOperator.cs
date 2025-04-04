// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.FirstResultOperator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.StreamedData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Remotion.Linq.Clauses.ResultOperators
{
  public class FirstResultOperator(bool returnDefaultWhenEmpty) : ChoiceResultOperatorBase(returnDefaultWhenEmpty)
  {
    public override ResultOperatorBase Clone(CloneContext cloneContext)
    {
      return (ResultOperatorBase) new FirstResultOperator(this.ReturnDefaultWhenEmpty);
    }

    public override StreamedValue ExecuteInMemory<T>(StreamedSequence input)
    {
      IEnumerable<T> typedSequence = input.GetTypedSequence<T>();
      return new StreamedValue((object) (this.ReturnDefaultWhenEmpty ? typedSequence.FirstOrDefault<T>() : typedSequence.First<T>()), (StreamedValueInfo) this.GetOutputDataInfo((IStreamedDataInfo) input.DataInfo));
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
    }

    public override string ToString()
    {
      return this.ReturnDefaultWhenEmpty ? "FirstOrDefault()" : "First()";
    }
  }
}

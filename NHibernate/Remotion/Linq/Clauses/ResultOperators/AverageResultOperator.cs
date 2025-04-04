// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.AverageResultOperator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Remotion.Linq.Clauses.ResultOperators
{
  public class AverageResultOperator : ValueFromSequenceResultOperatorBase
  {
    public override ResultOperatorBase Clone(CloneContext cloneContext)
    {
      return (ResultOperatorBase) new AverageResultOperator();
    }

    public override StreamedValue ExecuteInMemory<T>(StreamedSequence input)
    {
      ArgumentUtility.CheckNotNull<StreamedSequence>(nameof (input), input);
      return new StreamedValue((typeof (Enumerable).GetMethod("Average", BindingFlags.Static | BindingFlags.Public, (Binder) null, new Type[1]
      {
        typeof (IEnumerable<T>)
      }, (ParameterModifier[]) null) ?? throw new NotSupportedException(string.Format("Cannot calculate the average of objects of type '{0}' in memory.", (object) typeof (T).FullName))).Invoke((object) null, (object[]) new IEnumerable<T>[1]
      {
        input.GetTypedSequence<T>()
      }), (StreamedValueInfo) this.GetOutputDataInfo((IStreamedDataInfo) input.DataInfo));
    }

    public override IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo)
    {
      return (IStreamedDataInfo) new StreamedScalarValueInfo(this.GetResultType(ArgumentUtility.CheckNotNullAndType<StreamedSequenceInfo>(nameof (inputInfo), (object) inputInfo).ResultItemType));
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
    }

    public override string ToString() => "Average()";

    private Type GetResultType(Type inputItemType)
    {
      if (inputItemType == typeof (int) || inputItemType == typeof (long))
        return typeof (double);
      return inputItemType == typeof (int?) || inputItemType == typeof (long?) ? typeof (double?) : inputItemType;
    }
  }
}

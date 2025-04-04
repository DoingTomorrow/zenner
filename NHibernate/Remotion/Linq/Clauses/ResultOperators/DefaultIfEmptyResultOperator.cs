// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Clauses.ResultOperators.DefaultIfEmptyResultOperator
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
  public class DefaultIfEmptyResultOperator : SequenceTypePreservingResultOperatorBase
  {
    public DefaultIfEmptyResultOperator(Expression optionalDefaultValue)
    {
      this.OptionalDefaultValue = optionalDefaultValue;
    }

    public Expression OptionalDefaultValue { get; set; }

    public object GetConstantOptionalDefaultValue()
    {
      return this.OptionalDefaultValue == null ? (object) null : this.GetConstantValueFromExpression<object>("default value", this.OptionalDefaultValue);
    }

    public override ResultOperatorBase Clone(CloneContext cloneContext)
    {
      return (ResultOperatorBase) new DefaultIfEmptyResultOperator(this.OptionalDefaultValue);
    }

    public override StreamedSequence ExecuteInMemory<T>(StreamedSequence input)
    {
      IEnumerable<T> typedSequence = input.GetTypedSequence<T>();
      return new StreamedSequence((IEnumerable) (this.OptionalDefaultValue != null ? typedSequence.DefaultIfEmpty<T>((T) this.GetConstantOptionalDefaultValue()) : typedSequence.DefaultIfEmpty<T>()).AsQueryable<T>(), (StreamedSequenceInfo) this.GetOutputDataInfo((IStreamedDataInfo) input.DataInfo));
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
      ArgumentUtility.CheckNotNull<Func<Expression, Expression>>(nameof (transformation), transformation);
      if (this.OptionalDefaultValue == null)
        return;
      this.OptionalDefaultValue = transformation(this.OptionalDefaultValue);
    }

    public override string ToString()
    {
      return this.OptionalDefaultValue == null ? "DefaultIfEmpty()" : "DefaultIfEmpty(" + FormattingExpressionTreeVisitor.Format(this.OptionalDefaultValue) + ")";
    }
  }
}

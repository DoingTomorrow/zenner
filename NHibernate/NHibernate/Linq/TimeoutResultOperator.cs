// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.TimeoutResultOperator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.StreamedData;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq
{
  internal class TimeoutResultOperator : ResultOperatorBase
  {
    public MethodCallExpressionParseInfo ParseInfo { get; private set; }

    public ConstantExpression Timeout { get; private set; }

    public TimeoutResultOperator(
      MethodCallExpressionParseInfo parseInfo,
      ConstantExpression timeout)
    {
      this.ParseInfo = parseInfo;
      this.Timeout = timeout;
    }

    public override IStreamedData ExecuteInMemory(IStreamedData input)
    {
      throw new NotImplementedException();
    }

    public override IStreamedDataInfo GetOutputDataInfo(IStreamedDataInfo inputInfo) => inputInfo;

    public override ResultOperatorBase Clone(CloneContext cloneContext)
    {
      throw new NotImplementedException();
    }

    public override void TransformExpressions(Func<Expression, Expression> transformation)
    {
    }
  }
}

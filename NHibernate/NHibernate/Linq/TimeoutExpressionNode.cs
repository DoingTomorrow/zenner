// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.TimeoutExpressionNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq
{
  internal class TimeoutExpressionNode : ResultOperatorExpressionNodeBase
  {
    private readonly MethodCallExpressionParseInfo _parseInfo;
    private readonly ConstantExpression _timeout;

    public TimeoutExpressionNode(
      MethodCallExpressionParseInfo parseInfo,
      ConstantExpression timeout)
      : base(parseInfo, (LambdaExpression) null, (LambdaExpression) null)
    {
      this._parseInfo = parseInfo;
      this._timeout = timeout;
    }

    public override Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      return this.Source.Resolve(inputParameter, expressionToBeResolved, clauseGenerationContext);
    }

    protected override ResultOperatorBase CreateResultOperator(
      ClauseGenerationContext clauseGenerationContext)
    {
      return (ResultOperatorBase) new TimeoutResultOperator(this._parseInfo, this._timeout);
    }
  }
}

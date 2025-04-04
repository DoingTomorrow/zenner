// Decompiled with JetBrains decompiler
// Type: NHibernate.Linq.CacheableExpressionNode
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Clauses;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using System;
using System.Linq.Expressions;

#nullable disable
namespace NHibernate.Linq
{
  public class CacheableExpressionNode : ResultOperatorExpressionNodeBase
  {
    private readonly MethodCallExpressionParseInfo _parseInfo;
    private readonly ConstantExpression _data;

    public CacheableExpressionNode(MethodCallExpressionParseInfo parseInfo, ConstantExpression data)
      : base(parseInfo, (LambdaExpression) null, (LambdaExpression) null)
    {
      this._parseInfo = parseInfo;
      this._data = data;
    }

    public override Expression Resolve(
      ParameterExpression inputParameter,
      Expression expressionToBeResolved,
      ClauseGenerationContext clauseGenerationContext)
    {
      throw new NotImplementedException();
    }

    protected override ResultOperatorBase CreateResultOperator(
      ClauseGenerationContext clauseGenerationContext)
    {
      return (ResultOperatorBase) new CacheableResultOperator(this._parseInfo, this._data);
    }
  }
}
